function internet_page()
{
    var _isAgent                    = true;
    var _isOSA                      = false;
    var _isSubmit                   = false;
    var _expiringOK                 = false;
    var _beneficiaryAddressSEQID    = "";
    var _beneficiaryZipCodeSEQID    = "";
    var _beneficiaryContactSEQID    = "";
    var _beneficiaryEmailSEQID      = "";
    var _computation                = "";
    var _viptype                    = "0";
    var _vippercent                 = "0";
    var _aggCheckIDAmount           = 0;
    var _aggCheckIDCIF              = false;
    var _userLocation               = "";
    var _userID                     = "";
    var _agentProcessor             = "7";
    var _agentApprover              = "27";
    var _agentApproverProcessor     = "25";
    var _agentIsApprover            = false;
    var tblRemitMethod              = null;
    var tblCurrency                 = null;
    var tblReceiptMode              = null;
    var _receiptMode                = "";
    var tblDefaults                 = null;
    var tblComputation              = null;
    var tblChargeType               = null;
    var tblTranxSource              = null;
    var tblBranchSetup              = null;
    var _isCashAggLimit             = 0;
    var _cashAggAmtLimit            = 0;
    var _sysControl                 = 0;
    var _sysControlAmount           = 0;
    var _aggOutrightReject          = 0;
    var _remittanceAmtLimit         = 0;
    var _aggCheckIDAmount           = 0;
    var _branchCountry              = 0;
    var _RemittingCurrency          = 0;
    var _PaymentMethod              = 0;
    var _ChargeType                 = 0;
    var _TranxSource                = 0;
    var _TranxPurpose               = 0;
    var _agentSelectedRemitterID    = "";
    var _agentSelectedBeneficiary   = "";
    var _lookupField                = "";
    var _BeneProductSEQID           = "";
    var _TargetCurrency             = "";
    var tblBeneProduct              = null;
    var _ProductCode                = "";
    var _ImplementationCode         = "";
    var _arrCalc                    = "";
    var _aggregate                  = null;
    var _isplaceofissuance          = "";
    var _accountno                  = "";
    var _auditRemittance            = "";
    var _tranStatus                 = "";
    var _accountSEQID               = "";
    var _ORNo                       = "";
    var _tranxDate                  = "";
    var _serverTime                 = "";
    var _refno                      = "";
    var _txnseqid                   = "";
    var _erefno                     = "";
    var _fcSuccess                  = false;
    var tblRemittance               = null;
    var _RemIDs                     = false;
    var _RemitterIDs                = false;
    var cheque_payments             = null;
    var ach_payments                = null;
    var _acctNotValid               = false;
    var _isHit                      = false;
    
    this.ajaxTimeout = function()
    {
        AjaxPro.timeoutPeriod = 1000 * 1000;
        AjaxPro.onTimeout = function()
        {
            internet_page.showAlert("THE SERVER STOPPED RESPONDING. PLEASE TRY AGAIN");
        }
    }
    
    this.showAlert = function(message)
    {
        (message != "") ? alert(message) : alert("An error occured while getting information. Please report to the nearest PNB branch.");
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
    
    this.autoFormatCurrency = function(fld, milSep, decSep, e)
    {
        var sep = 0;
        var key = "";
        var i = j = 0;
        var len = len2 = 0;
        var strCheck = "0123456789";
        var aux = aux2 = "";
        var whichCode = (window.event) ? e.keyCode : e.which;
        
        if(internet_page._isOSA == true || internet_page._acctNotValid == true)
        {
            return false;
        }
        
        if(fld.id == "txtCNetAmount")
        {
            internet_page.set_computationlevel("2");
        }
        else if(fld.id == "txtCGrossAmount")
        {
            internet_page.set_computationlevel("1");
        }
        else if(fld.id == "txtCNetToBenefAmount")
        {
            internet_page.set_computationlevel("3");
        }
        else if(fld.id == "txtCBankChargeAmount")
        {
            if(parseFloat(internet_page.getValue("txtCNetAmount", false)) <= 0 || parseFloat(internet_page.getValue("txtCGrossAmount", false)) <= 0 || parseFloat(internet_page.getValue("txtCNetToBenefAmount", false)) <= 0)
            {
                return false;
            }
        }
        
        if (internet_page._isAgent == true)
        {
            if (internet_page.getValue(fld.id, false) == '' || parseFloat(internet_page.getValue(fld.id, false)) < 0)
            {
                internet_page.edButtons("btnCompute", true);
            }
            else
            {
                if(internet_page._isOSA == false && internet_page._acctNotValid == false)
                {
                    internet_page.edButtons("btnCompute", false);
                }
                if (whichCode == 13)
                {
                    if (new common_method().is_decimal(new common_method().to_remove_comma(internet_page.getValue(fld.id, false))) == false)
                    {
                        internet_page.showAlert("Amount entered is not valid!\n Click ok to correct amount.");
                        return false;
                    }
                    else
                    {
                        internet_page.enterValue(new common_method().to_currency(internet_page.getValue("txtCNetAmount", false)), "txtCNetAmount", false);
                    }
                }
            }
        }

        if (whichCode == 13) return true; // Enter
        if (whichCode == 8) return true; // Delete
        if (whichCode == 9) return true; // Tab
        if (whichCode == 11) return true; // Tab
        key = String.fromCharCode(whichCode); // Get key value from key code
        if (strCheck.indexOf(key) == -1) return false; // Not a valid key
//        len = fld.value.length;
//        
//        for(i = 0; i < len; i++)
//        if ((fld.value.charAt(i) != "0") && (fld.value.charAt(i) != decSep)) break;
//        aux = "";
//        for(; i < len; i++)
//        {
//            if (strCheck.indexOf(fld.value.charAt(i))!=-1) 
//            {
//                if (i == 0)
//                {
//                    aux += key + fld.value.charAt(i);
//                    len = len - 1;
//                }
//                else if (i > 0)
//                {
//                    aux += fld.value.charAt(i);
//                    len = len - 1;
//                }
//                else
//                {
//                    aux += fld.value.charAt(i);
//                    aux += key;
//                }
//            }
//        }
//        if (aux == "")
//        {
//            aux += key;
//        }
//        len = aux.length;
//        if (len == 0) fld.value = "";
//        if (len == 1) fld.value = aux + decSep + "0" + "0";
//        if (len == 2) fld.value =  aux + decSep + "0";
//        if (len > 2) 
//        {
//            aux2 = '';
//            for (j = 0, i = len - 3; i >= 0; i--) 
//            {
//                if (j == 3) 
//                {
//                    aux2 += milSep;
//                    j = 0;
//                }
//                aux2 += aux.charAt(i);
//                j++;
//            }
//            fld.value = "";
//            len2 = aux2.length;
//            for (i = len2 - 1; i >= 0; i--)
//            fld.value += aux2.charAt(i);
//            fld.value += decSep + aux.substr(len - 2, len);
            if (internet_page.getValue(fld.id, false) == '' || parseFloat(internet_page.getValue(fld.id, false)) <= 0)
            {
                if (parseFloat(key) > 0)
                {
                    if(internet_page._isOSA == false && internet_page._acctNotValid == false)
                    {
                        internet_page.edButtons("btnCompute", false);
                    }
                    else
                    {
                        internet_page.edButtons("btnCompute", true);
                    }
                    return true;
                }
                else
                {
                    internet_page.edButtons("btnCompute", true);
                    return true;
                }
            }
            else
            {
                if (parseFloat(internet_page.getValue(fld.id, false)) > 0)
                {
                    if(internet_page._isOSA == false && internet_page._acctNotValid == false)
                    {
                        internet_page.edButtons("btnCompute", false);
                    }
                    else
                    {
                        internet_page.edButtons("btnCompute", true);
                    }
                    return true;
                }
                else
                {
                    internet_page.edButtons("btnCompute", true);
                    return true;
                }
            }
//        }
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
            num = Math.floor(num / 100).toString();
            len = 0;
            len = cents.length;

            for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
            {
                num = num.substring(0, num.length - (4 * i + 3)) + ',' + num.substring(num.length - (4 * i + 3));
            }
            return (((sign) ? "" : "-") + num + "." + cents);
        }
    }
    
    this.divDisplay = function(divName)
    {
        var div = document.getElementById(divName);
        
        internet_page.showControl(divName, true);
        
        div.style.top = (((document.documentElement.clientHeight / 2) + document.documentElement.scrollTop) - (div.offsetHeight / 2)) + "px";
        div.style.left = (((document.documentElement.clientWidth / 2) + document.documentElement.scrollLeft) - (div.offsetWidth / 2)) + "px";
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
        internet_page.showControl(divName, false);
        divmask.hide();
    }

    var maskpanel = function(source)
    {
        this.divobj;
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
          this.waitifrm.style.display = this.divobj.style.display = "block";
       }
       
       this.hide = function()
       {
           this.waitifrm.style.display = this.divobj.style.display = "none";
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
                //select.options[i + 1] = new Option(arrValue, iRow.Rows[i][value]);
                select.options[i + 1] = new Option(arrValue, arrIDValue);
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
        var selectedval = (isValue == true) ? select.options[select.selectedIndex].value : select.options[select.selectedIndex].text;
        //eval(selected + "=" + selectedval);
        window[selected] = selectedval;
        //Function("javascript:" + selected + "=" + selectedval)
    }

    this.getDDLValue = function(element, isValue)
    {
        var select = document.getElementById(element);
        return (isValue == true) ? select.options[select.selectedIndex].value : select.options[select.selectedIndex].text;
    }
    
    this.clearDDL = function(element)
    {
        var select = document.getElementById(element);
        select.options.length = 0;
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
    
    this.writeLogger = function(remittance, tablename, key)
    {
        IRSWorld.Internet.InternetPageV2.writeLogger(remittance, tablename, key);
    }
    
    this.fixdim = function()
    {
//        if (internet_page._isprinting == false)
//        {
//            if (top.document.getElementById('divCenter'))
//            {
//                var div = document.getElementById('divPage');
//                div.style.height = (top.document.getElementById('divCenter').offsetHeight) + 'px';
//                div.style.width = (top.document.getElementById('divCenter').offsetWidth + 5) + 'px';
//                if (document.getElementById('divInternetPageMenu'))
//                {
//                    document.getElementById('tdInternetPagemenu').style.width = '245px';
//                    document.getElementById('tdInternetPagemenu').style.height = div.style.height;
//                    document.getElementById('tdInternetPageCenter').style.width = (parseInt(div.style.width) - 725) + 'px';
//                    document.getElementById('tdInternetPageCenter').style.height = div.style.height;                    
//                    
//                    var div_menu = document.getElementById('divInternetPageMenu');
//                    div_menu.style.height = (parseInt(div.style.height)) + 'px';
//                    div_menu.style.width = document.getElementById('tdInternetPagemenu').style.width;
//                }
//            }
//        }
    }
    
    this.getLookup = function()
    {
        internet_page.ajaxTimeout();
        return IRSWorld.Internet.InternetPageV2.getSubLookup(internet_page._lookupField, this.getLookup_callback);
    }
    
    this.getLookup_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            switch (response.Rows[0]["FldGroup"])
            {
                case "ReceiptMode":
                    internet_page.tblReceiptMode = response;
                    internet_page._receiptMode = internet_page.getTableValue(internet_page.tblDefaults, "ReceiptMode");
                    if (internet_page._receiptMode == "" || internet_page._receiptMode == undefined || internet_page._receiptMode == null)
                    {
                        internet_page._receiptMode = "5";
                    }
                    break;
                case "Computation":
                    internet_page.tblComputation = response;
                    //internet_page.fillDDL(response, "ddlComputation", "FldDescription", "FldValue", "lblComputation", ", "); 
                    //internet_page._computation = internet_page.findValue(response, "Gross", "FldDescription", "FldValue");
                    break;
                case "ChargeType":
                    internet_page.tblChargeType = response;
                    internet_page.fillDDL(response, "ddlChargeType", "FldDescription", "FldValue", "internet_page._ChargeType", ", ");
                    break;
                case "TranxSource":
                    internet_page.tblTranxSource = response;
                    internet_page.fillDDL(response, "ddlTranxSource", "FldDescription", "FldValue", "internet_page._TranxSource", ", ");
                    break;
                case "TranxPurpose":
                    internet_page.tblTranxSource = response;
                    internet_page.fillDDL(response, "ddlPurpose", "FldDescription", "FldValue", "internet_page._TranxPurpose", ", ");
                    break;
            }
            internet_page._lookupField = "";
        }
    }
    
    this.initializePage = function()
    {
        //internet_page.divDisplay("divSearch");
        internet_page.showTransactionLoading("Initialize Page....", false);
        internet_page.showControl("divTransaction", false);
        internet_page.clearSearch();
        internet_page.clearCompute();
        internet_page._lookupField              = "";
        internet_page._agentProcessor           = "7";
        internet_page._agentApprover            = "27";
        internet_page._agentApproverProcessor   = "25";
        internet_page._isOSA                    = false;
        internet_page._isSubmit                 = false;
        internet_page._expiringOK               = false;
        internet_page._isprinting               = false;
        internet_page._beneficiaryAddressSEQID  = "0";
        internet_page._beneficiaryContactSEQID  = "0";
        internet_page._beneficiaryEmailSEQID    = "0";
        internet_page._computation              = "";
        internet_page._viptype                  = "0";
        internet_page._vippercent               = "0";
        internet_page._aggCheckIDAmount         = 0;
        internet_page._aggCheckIDCIF            = false;
        internet_page._agentSelectedRemitterID  = "";
        internet_page._agentSelectedBeneficiary = "";
        internet_page._ProductCode              = "";
        internet_page.tblBeneProduct            = null;
        internet_page._arrCalc                  = "";
        internet_page._isplaceofissuance        = "";
        internet_page._tranStatus               = "1";
        internet_page._ORNo                     = "";
        internet_page._tranxDate                = "";
        internet_page._serverTime               = "";
        internet_page._refno                    = "";
        internet_page._txnseqid                 = "";
        internet_page._erefno                   = "";
        internet_page._fcSuccess                = false;
        internet_page._RemIDs                   = false;
        internet_page._RemitterIDs              = false;
        internet_page._acctNotValid             = false;
        internet_page._isHit                    = false;
        internet_page.removeValue("spnHitMessage", true);
        internet_page.removeValue("spnHitMessage2", true);
        internet_page.getUserLocation();
        internet_page.getDefaults();
        internet_page.getUserID();
        internet_page.getUserRole();
        IRSWorld.Internet.InternetPageV2.getBranchSetup(internet_page.getBranchSetup_callback);
    }
    
    this.getBranchSetup_callback = function(result)
    {        
        internet_page.tblBranchSetup = result.value;        
        if(internet_page.tblBranchSetup != null && internet_page.tblBranchSetup != "null" && internet_page.tblBranchSetup != undefined && internet_page.tblBranchSetup != "undefined")
        {
            internet_page._isCashAggLimit = internet_page.getTableValueRow(internet_page.tblBranchSetup, "CashAggregationLimit", 0);
            internet_page._cashAggAmtLimit = internet_page.getTableValueRow(internet_page.tblBranchSetup, "CashAggregationAmtLimit", 0);
            internet_page._sysControl = internet_page.getTableValueRow(internet_page.tblBranchSetup, "AgentSysControl", 0);
            internet_page._sysControlAmount = internet_page.getTableValueRow(internet_page.tblBranchSetup, "AgentLogReport", 0);
            internet_page._aggOutrightReject = internet_page.getTableValueRow(internet_page.tblBranchSetup, "AgentAggAmtReject", 0);
            internet_page._remittanceAmtLimit = internet_page.getTableValueRow(internet_page.tblBranchSetup, "RemittanceAmtLimit", 0);
            internet_page._aggCheckIDAmount = internet_page.getTableValueRow(internet_page.tblBranchSetup, "AgentCheckIDAmount", 0);
            internet_page._branchCountry = IRSWorld.Internet.InternetPageV2.getBranchCountry().value;
            //internet_page.hidePanel("divPopProcess");
            //IRSWorld.Internet.InternetPage.checkInternetRemittanceAggregateLimit(internet_page.getValue("lblRemitterID", false), ((internet_page._arrCalc[35] == "0") ? internet_page.getValue("lblRemittingCurrency", false).toString() : internet_page._arrCalc[35]),internet_page.getValue("txtCGrossAmount", false), internet_page.AggregateInternetRem_callback);
            internet_page.checkSession();
        }
        else
        {
            internet_page.showAlert("FAILED TO GET VALIDATION DATA.");
        }
    }
    
    this.checkSession = function()
    {
        internet_page.showTransactionLoading("", true);
        var session = IRSWorld.Internet.InternetPageV2.getSession().value;
        if (session == "")
        {
            internet_page.divDisplay("divSearch");
        }
        else
        {
            var sessionSplit = session.split("|");
            if(sessionSplit[0] == "TRANS")
            {
                internet_page._agentSelectedRemitterID = sessionSplit[1];
                internet_page._agentSelectedBeneficiary = sessionSplit[2] + "|";
                internet_page._BeneProductSEQID = sessionSplit[3];
                internet_page.DisplayTransaction(sessionSplit[1], sessionSplit[2]);
                IRSWorld.Internet.InternetPageV2.DeleteCIFTransSession("ARSTRANSACTION");
            }
            else if(sessionSplit[0] == "REM")
            {
                internet_page._agentSelectedRemitterID = sessionSplit[1];
                internet_page._agentSelectedBeneficiary = sessionSplit[2] + "|";
                internet_page._BeneProductSEQID = sessionSplit[3];
                internet_page.DisplayTransaction(sessionSplit[1], sessionSplit[2]);
                IRSWorld.Internet.InternetPageV2.DeleteCIFTransSession("ARSEDITREMITTER");
            }
            else if(sessionSplit[0] == "BEN")
            {
                internet_page._agentSelectedRemitterID = sessionSplit[1];
                internet_page._agentSelectedBeneficiary = sessionSplit[2] + "|";
                internet_page._BeneProductSEQID = sessionSplit[3];
                internet_page.DisplayTransaction(sessionSplit[1], sessionSplit[2]);
                IRSWorld.Internet.InternetPageV2.DeleteCIFTransSession("ARSEDITBENEFICIARY");
            }
            else if(sessionSplit[0] == "PROD")
            {
                internet_page._agentSelectedRemitterID = sessionSplit[1];
                internet_page._agentSelectedBeneficiary = sessionSplit[2] + "|";
                internet_page._BeneProductSEQID = sessionSplit[3];
                internet_page.DisplayTransaction(sessionSplit[1], sessionSplit[2]);
                IRSWorld.Internet.InternetPageV2.DeleteCIFTransSession("ARSADDPRODUCT");
            }
        }
    }
    
    this.createSession = function(sessionname)
    {
        var strvalue = internet_page._agentSelectedRemitterID + "|" + internet_page._agentSelectedBeneficiary.split("|")[0] + "|" + internet_page._BeneProductSEQID;
        IRSWorld.Internet.InternetPageV2.CreateCIFTransSession(sessionname, strvalue);
        window.open('../Internet/InternetRemitterARS.aspx','_self');
    }
    
    this.getUserLocation = function()
    {
        internet_page.ajaxTimeout();
        return IRSWorld.Internet.InternetPageV2.getUserLocation(this.getUserLocation_callback);
    }
    
    this.getUserLocation_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            internet_page._userLocation = response;
        }
    }
    
    this.getDefaults = function()
    {
        internet_page.ajaxTimeout();
        return IRSWorld.Internet.InternetPageV2.getDefaults(this.getDefaults_callback);
    }
    
    this.getDefaults_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            internet_page.tblDefaults = response;
            internet_page._receiptMode = internet_page.getTableValue(internet_page.tblDefaults, "ReceiptMode");
//            internet_page._lookupField = "RemitMethod";
//            internet_page.getLookup();
//            internet_page._lookupField = "Currency";
//            internet_page.getLookup();
//            internet_page._lookupField = "ReceiptMode";
//            internet_page.getLookup();
//            internet_page._lookupField = "Computation";
//            internet_page.getLookup();
            internet_page._lookupField = "TranxSource";
            internet_page.getLookup();
            internet_page._lookupField = "TranxPurpose";
            internet_page.getLookup();
        }
    }
    
    this.getUserID = function()
    {
        internet_page.ajaxTimeout();
        return IRSWorld.Internet.InternetPageV2.getUserID(this.getUserID_callback);
    }

    this.getUserID_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            internet_page._userID = response;
        }
    }
    
    this.getUserRole = function()
    {
        internet_page.ajaxTimeout();
        return IRSWorld.Internet.InternetPageV2.getUserRole(this.getUserRole_callback);
    }

    this.getUserRole_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            if (response == internet_page._agentProcessor || response == internet_page._agentApprover || response == internet_page._agentApproverProcessor) //Processor - Agent (7) | Approver - Agent (27) | Approver & Processor - Agent (25)
            {
                internet_page._isAgent = true;
                if (response == internet_page._agentApprover || response == internet_page._agentApproverProcessor)
                {
                    internet_page._agentIsApprover = true;
                }
                else
                {
                    internet_page._agentIsApprover = false;
                }
                internet_page.getAgentCurrency();
                internet_page.showAgentControl(true);
                internet_page._lookupField = "ChargeType";
                internet_page.getLookup();
            }
            else
            {
                internet_page.showAlert("Cannot determine user role. Please close your browser and try to login again.");
            }
            internet_page.getPaymentMethod();
        }
    }
    
    this.getAgentCurrency = function()
    {
        internet_page.clearDDL("ddlRemittingCurrency");
        return IRSWorld.Internet.InternetPageV2.getAgentCurrency(this.getAgentCurrency_callback);
    }
    
    this.getAgentCurrency_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            internet_page.fillDDL(response, "ddlRemittingCurrency", "FldCode", "FldValue", "internet_page._RemittingCurrency", "");
//            if(internet_page.tblDefaults != undefined && internet_page.tblDefaults != null && internet_page.tblDefaults != "undefined" && internet_page.tblDefaults != "")
//            {
//                internet_page.enterValue(internet_page.getTableValue(internet_page.tblDefaults, "RemittingCurrency"),"ddlRemittingCurrency",false);
//                internet_page.getDDL("ddlRemittingCurrency", "lblRemittingCurrency", true);
//                internet_page.getDDLAjax("ddlRemittingCurrency", "lblRemittingCurrency", true);
//            }
        }
    }
    
    this.getPaymentMethod = function()
    {
        internet_page.ajaxTimeout();
        internet_page.clearDDL("ddlPaymentMethod");
        //internet_page.removeValue("spnSourceCurrency", true);
        return IRSWorld.Internet.InternetPageV2.getPaymentMethod(this.getPaymentMethod_callback);
    }
    
    this.getPaymentMethod_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            internet_page.fillDDL(response, "ddlPaymentMethod", "FldDescription", "FldValue", "internet_page._PaymentMethod", "");
            
            if(response.Rows.length > 1)
            {
                
            }
            else
            {
                //internet_page.setPaymentMethod(false);
            }
        }
    }
    
    this.showAgentControl = function(show)
    {
        IRSWorld.Internet.InternetPageV2.getSubLookup("WEB REMITTANCE ACTIVE FIELDS", internet_page.tranxsourceField_callback);
        IRSWorld.Internet.InternetPageV2.getSubLookup("WEB REMITTANCE ACTIVE FIELDS", internet_page.specialmessageField_callback);
    }
    
    this.tranxsourceField_callback = function(result)
    {
        var fieldtable = result.value;
        if (fieldtable != null && fieldtable.Rows.length > 0)
        {
            for (var i = 0; i <= (fieldtable.Rows.length - 1); i++)
            {
                if(fieldtable.Rows[i].FldCode.toString() == "trAgent5")
                {
                    internet_page.showControl(fieldtable.Rows[i].FldCode.toString(), true);
                }
            }
        }
    }
    
    this.specialmessageField_callback = function(result)
    {
        var fieldtable = result.value;
        if (fieldtable != null && fieldtable.Rows.length > 0)
        {
            for (var i = 0; i <= (fieldtable.Rows.length - 1); i++)
            {
                if(fieldtable.Rows[i].FldCode.toString() == "trAgent6")
                {
                    internet_page.showControl(fieldtable.Rows[i].FldCode.toString(), true);
                }
            }
        }
    }
    
    this.callSearch = function()
    {
        if (window.event.keyCode == 13) // Enter key
        {
            internet_page.searchCIF(false);
        }
    }
    
    this.searchCIF = function(issearch)
    { 
//        internet_page.showCIFLoading("Please wait...", false);
//        internet_page.showControl("btnSearch", true);
//        if (issearch == true)
//        {
//            internet_page.showSearch("", "divSearch");
//        }
//        else
//        {
//            if (document.getElementById("ddlSearchBy").selectedIndex > 0)
//            {
//                internet_page.showSearch("divSearch", "divSearchGrid");
//            }
//            else
//            {
//                internet_page.showAlert("Please select a search filter.");
//            }
//        }
        internet_page.load_grid_viewer();        
        internet_page.showControl("trSearchResults", true);
        internet_page.divDisplay("divSearch");
        //internet_page.clearSearch();

    }
    
    this.clearSearch = function()
    {        
        internet_page.removeValue("txtSLastName", false);
        internet_page.removeValue("txtSFirstName", false);
        internet_page.removeValue("txtSMiddleName", false);        
        internet_page.showControl("trSearchResults", false);
        internet_page.divDisplay("divSearch");
    }
    
    this.DisplayTransaction = function(remitterCIFID, beneficiaryCIFID)
    {
        internet_page.showControl("divSearch", false);
        internet_page.divDisplay("divTransaction");
        internet_page.showTransactionLoading("Getting CIF Information....", false);
        IRSWorld.Internet.InternetPageV2.getCIFTranxInfo(remitterCIFID, beneficiaryCIFID, this.getCIFTranxInfo_callback)
    }
    
    this.getCIFTranxInfo_callback = function(result)
    {
        if (result != null && result != undefined)
        {
            var ds = result.value;
            if(ds.Tables[0].Rows.length > 0)
            {
                internet_page.enterValue(internet_page.getTableValueRow(ds.Tables[0], "RemFullName", 0), "spnRemitterName", true);
                internet_page.enterValue(internet_page.getTableValueRow(ds.Tables[0], "RemAddress", 0), "spnRemitterAddress", true);
                internet_page.enterValue(internet_page.getTableValueRow(ds.Tables[0], "RemOccupation", 0), "spnRemitterOccupation", true);
                internet_page.enterValue(internet_page.getTableValueRow(ds.Tables[0], "BirthDate", 0), "spnRemitterDOB", true);
                internet_page.check_watchlist(internet_page.getTableValueRow(ds.Tables[0], "RemCIFID", 0),false);
                if(ds.Tables[0].Rows.length > 1)
                {
                    var strRemIds = "";
                    for (var i = 0; i <= (ds.Tables[0].Rows.length - 1); i++)
                    {
                        strRemIds = strRemIds + internet_page.getTableValueRow(ds.Tables[0], "RemIds", i) + "<br />";
                    }
                    internet_page.enterValue(strRemIds, "spnRemitterIds", true);
                }
                else
                {
                    internet_page.enterValue(internet_page.getTableValueRow(ds.Tables[0], "RemIds", 0), "spnRemitterIds", true);
                }
            }
            
            if(ds.Tables[1].Rows.length > 0)
            {
                internet_page.enterValue(internet_page.getTableValueRow(ds.Tables[1], "FullName", 0), "spnBeneficiaryName", true);
                internet_page.enterValue(internet_page.getTableValueRow(ds.Tables[1], "BeneAddress", 0), "spnBeneficiaryAddress", true);
                internet_page.enterValue(internet_page.getTableValueRow(ds.Tables[1], "BeneZipCode", 0), "spnBeneficiaryZipCode", true);
                internet_page.enterValue(internet_page.getTableValueRow(ds.Tables[1], "BeneBirthDate", 0), "spnBeneficiaryDOB", true);
                internet_page.enterValue(internet_page.getTableValueRow(ds.Tables[1], "BeneContacts", 0), "spnBeneficiaryContactNo", true);
                internet_page._beneficiaryAddressSEQID = internet_page.getTableValueRow(ds.Tables[1], "BeneAddSEQID", 0);
                internet_page._beneficiaryContactSEQID = internet_page.getTableValueRow(ds.Tables[1], "BeneContactSEQID", 0);
                internet_page._beneficiaryZipCodeSEQID = internet_page.getTableValueRow(ds.Tables[1], "ZipCodeSeqId", 0);
                internet_page.check_watchlist(internet_page.getTableValueRow(ds.Tables[1], "CIFID", 0),true);
            }
            
            if(ds.Tables[2].Rows.length > 0)
            {
                internet_page.tblBeneProduct = ds.Tables[2];
                internet_page.fillDDL(internet_page.tblBeneProduct, "ddlBeneficiaryProduct", "Product", "SEQ_ID", "internet_page._BeneProductSEQID", "");
                if(internet_page.tblBeneProduct.Rows.length > 1)
                {
                    var benearrSplit = internet_page._agentSelectedBeneficiary.split("|");
                    
                    for (var i = 0; i <= (internet_page.tblBeneProduct.Rows.length - 1); i++)
                    {
                        if(internet_page.tblBeneProduct.Rows[i]["CIFID"].toString() == benearrSplit[0].toString() && internet_page.tblBeneProduct.Rows[i]["ProductCode"].toString() == benearrSplit[1].toString() && internet_page.tblBeneProduct.Rows[i]["AccountSeqId"].toString() == benearrSplit[3].toString() && internet_page.tblBeneProduct.Rows[i]["CurrencyValue"].toString() == benearrSplit[4].toString())
                        {
                            internet_page.enterValue(internet_page.getTableValueRow(internet_page.tblBeneProduct, "Currency", i), "spnCNetToBenefAmountCurrency", true);
                            internet_page._TargetCurrency = internet_page.getTableValueRow(internet_page.tblBeneProduct, "CurrencyValue", i);
                            internet_page._BeneProductSEQID = internet_page.getTableValueRow(internet_page.tblBeneProduct, "SEQ_ID", i);
                            internet_page._ProductCode = internet_page.getTableValueRow(internet_page.tblBeneProduct, "ProductCode", i);
                            internet_page._ImplementationCode = internet_page.getTableValueRow(internet_page.tblBeneProduct, "ImplementationCode", i);
                            internet_page._accountno = internet_page.getTableValueRow(internet_page.tblBeneProduct, "AccountNo", i);
                            internet_page._accountSEQID = internet_page.getTableValueRow(internet_page.tblBeneProduct, "AccountSeqId", i);
                            internet_page.findDDL("ddlBeneficiaryProduct", internet_page._BeneProductSEQID, "internet_page._BeneProductSEQID", true);
                            break;
                        }
                        else if (internet_page._BeneProductSEQID != "" && internet_page.tblBeneProduct.Rows[i]["CIFID"].toString() == benearrSplit[0].toString() && internet_page.tblBeneProduct.Rows[i]["SEQ_ID"].toString() == internet_page._BeneProductSEQID)
                        {
                            internet_page.enterValue(internet_page.getTableValueRow(internet_page.tblBeneProduct, "Currency", i), "spnCNetToBenefAmountCurrency", true);
                            internet_page._TargetCurrency = internet_page.getTableValueRow(internet_page.tblBeneProduct, "CurrencyValue", i);
                            internet_page._BeneProductSEQID = internet_page.getTableValueRow(internet_page.tblBeneProduct, "SEQ_ID", i);
                            internet_page._ProductCode = internet_page.getTableValueRow(internet_page.tblBeneProduct, "ProductCode", i);
                            internet_page._ImplementationCode = internet_page.getTableValueRow(internet_page.tblBeneProduct, "ImplementationCode", i);
                            internet_page._accountno = internet_page.getTableValueRow(internet_page.tblBeneProduct, "AccountNo", i);
                            internet_page._accountSEQID = internet_page.getTableValueRow(internet_page.tblBeneProduct, "AccountSeqId", i);
                            internet_page.findDDL("ddlBeneficiaryProduct", internet_page._BeneProductSEQID, "internet_page._BeneProductSEQID", true);
                            break;
                        }
                    }
                }
                else
                {
                    document.getElementById("ddlBeneficiaryProduct").selectedIndex = 1;
                    internet_page.enterValue(internet_page.getTableValueRow(internet_page.tblBeneProduct, "Currency", 0), "spnCNetToBenefAmountCurrency", true);
                    internet_page._TargetCurrency = internet_page.getTableValueRow(internet_page.tblBeneProduct, "CurrencyValue", 0);
                    internet_page._BeneProductSEQID = internet_page.getTableValueRow(internet_page.tblBeneProduct, "SEQ_ID", 0);
                    internet_page._ProductCode = internet_page.getTableValueRow(internet_page.tblBeneProduct, "ProductCode", 0);
                    internet_page._ImplementationCode = internet_page.getTableValueRow(internet_page.tblBeneProduct, "ImplementationCode", 0);
                    internet_page._accountno = internet_page.getTableValueRow(internet_page.tblBeneProduct, "AccountNo", 0);
                    internet_page._accountSEQID = internet_page.getTableValueRow(internet_page.tblBeneProduct, "AccountSeqId", 0);
                }
                if(internet_page.tblDefaults != undefined && internet_page.tblDefaults != null && internet_page.tblDefaults != "undefined" && internet_page.tblDefaults != "")
                {
                    internet_page._RemittingCurrency = internet_page.getTableValueRow(internet_page.tblDefaults, "RemittingCurrency", 0);
                    internet_page._ChargeType = internet_page.getTableValueRow(internet_page.tblDefaults, "ChargeType", 0);
                    internet_page._TranxSource = internet_page.getTableValueRow(internet_page.tblDefaults, "TranxSource", 0);
                    internet_page._TranxPurpose = internet_page.getTableValueRow(internet_page.tblDefaults, "TranxPurpose", 0);
                    internet_page._PaymentMethod = internet_page.getTableValueRow(internet_page.tblDefaults, "PayMode", 0);
                    
                    internet_page.findDDL("ddlTranxSource", internet_page._TranxSource, "internet_page._TranxSource", true);
                    internet_page.findDDL("ddlRemittingCurrency", internet_page._RemittingCurrency, "internet_page._RemittingCurrency", true);
                    internet_page.findDDL("ddlChargeType", internet_page._ChargeType, "internet_page._ChargeType", true);
                    internet_page.findDDL("ddlPaymentMethod", internet_page._PaymentMethod, "internet_page._TranxPurpose", true);
                    internet_page.findDDL("ddlPurpose", internet_page._PaymentMethod, "internet_page._TranxPurpose", true);
                    
                    internet_page.enterValue(internet_page.getDDLValue("ddlRemittingCurrency", false), "spnCNetAmountCurrency", true);
                    internet_page.enterValue(internet_page.getDDLValue("ddlRemittingCurrency", false), "spnCBankChargeAmountCurrency", true);
                    internet_page.enterValue(internet_page.getDDLValue("ddlRemittingCurrency", false), "spnCGrossAmountCurrency", true);
                    internet_page.enterValue(internet_page.getDDLValue("ddlRemittingCurrency", false) + " to " + internet_page.getValue("spnCNetToBenefAmountCurrency", true), "spnCExchangeRateCurrency", true);
                    IRSWorld.Internet.InternetPageV2.getExchangeRate(internet_page._RemittingCurrency.toString(), internet_page._TargetCurrency.toString(), internet_page.getExchangeRate_callback);
                }
                if (internet_page._ProductCode.toString() == "8" || internet_page._ProductCode.toString() == "9")
                {
                    if(internet_page.getValue("spnBeneficiaryAddress",true) == "" || internet_page.getValue("spnBeneficiaryZipCode",true) == "")
                    {
                        if(confirm("Incomplete Beneficiary address details for this transaction. Kindly update."))
                        {
                            internet_page.showAlert("Wala pa yung CIF.");
                        }
                        else
                        {
                            internet_page.initializePage();
                        }
                    }
                    else
                    {
                        internet_page.getOSA(internet_page._beneficiaryZipCodeSEQID);
                    }
                }
                else
                {
                    internet_page.getAccountInquiry();
                }
            }
        }
    }
    
    this.check_watchlist = function(cifid, isbene)
    {
        IRSWorld.Internet.InternetPageV2.check_watchlist(cifid, isbene, internet_page.check_watchlist_callback);
    }
    
    this.check_watchlist_callback = function(param)
    {
        var result = "";
        if(param.value != null && param.value != "")
        {
            result = param.value.replace("<BENENAME>", internet_page.getValue("spnBeneficiaryName", true)).replace("<REMNAME>", internet_page.getValue("spnRemitterName", true));
            //param.value.replace("<REMNAME>", internet_page.getValue("spnRemitterName", true));
            internet_page._isHit = true;
            if (internet_page.getValue("spnHitMessage",true) != "")
            {
                internet_page.enterValue(result,"spnHitMessage2",true);
            }
            else
            {
                internet_page.enterValue(result,"spnHitMessage",true);
            }
            internet_page.load_grid_viewer();
            internet_page.showPanel("divHitList");
        }
    }
    
    this.getAccountInquiry = function()
    {
        internet_page.ajaxTimeout();
        internet_page.showTransactionLoading("Validating account. Please wait....", false);
        var accountnumber = internet_page._accountno;
        if (internet_page.trim(accountnumber) != "")
        {
            if (parseInt(accountnumber))
            {
                if (accountnumber.length > 12 && internet_page._ProductCode.toString() == "22")
                {
                    internet_page.showTransactionLoading("", true);
                    internet_page._acctNotValid = false;
                    if (internet_page._aggCheckIDAmount <= 0)
                    {
                        internet_page.check_remitter_ids_required();
                    }
                }
                else if (internet_page._ProductCode.toString() == "8" || internet_page._ProductCode.toString() == "9" || internet_page._ProductCode.toString() == "27" || internet_page._ProductCode.toString() == "12" || internet_page._ProductCode.toString() == "13" || internet_page._ProductCode.toString() == "16")
                {
                    internet_page.showTransactionLoading("", true);
                    internet_page._acctNotValid = false;
                    if (internet_page._aggCheckIDAmount <= 0)
                    {
                        internet_page.check_remitter_ids_required();
                    }
                }
                else
                {
                    if (Number(accountnumber) > 0 && accountnumber.length >= 10)
                    {
                        return IRSWorld.Internet.InternetPageV2.getAccountInquiry(accountnumber, this.getAccountInquiry_callback);
                    }
                    else
                    {
                        internet_page.showTransactionLoading("", true);
                        internet_page.showAlert("Please check account status of your beneficiary. Kindly call branch.");
                        internet_page._acctNotValid = true;
                    }
                }
            }
            else if (internet_page._ProductCode.toString() == "8" || internet_page._ProductCode.toString() == "9" || internet_page._ProductCode.toString() == "27" || internet_page._ProductCode.toString() == "12" || internet_page._ProductCode.toString() == "13" || internet_page._ProductCode.toString() == "16" || internet_page._ProductCode.toString() == "22")
            {
                internet_page.showTransactionLoading("", true);
                internet_page._acctNotValid = false;
                if (internet_page._aggCheckIDAmount <= 0)
                {
                    internet_page.check_remitter_ids_required();
                }
            }
            else
            {
                internet_page.showTransactionLoading("", true);
                internet_page.showAlert("Please check account status of your beneficiary. Kindly call branch.");
                internet_page._acctNotValid = true;
            }
        }
        else if (internet_page._ProductCode.toString() == "8" || internet_page._ProductCode.toString() == "9" || internet_page._ProductCode.toString() == "27" || internet_page._ProductCode.toString() == "12" || internet_page._ProductCode.toString() == "13" || internet_page._ProductCode.toString() == "16" || internet_page._ProductCode.toString() == "22")
        {
            internet_page.showTransactionLoading("", true);
            internet_page._acctNotValid = false;
            if (internet_page._aggCheckIDAmount <= 0)
            {
                internet_page.check_remitter_ids_required();
            }
        }
        else if (internet_page._ProductCode == undefined || internet_page._ProductCode.toString() == "")
        {
            internet_page.showTransactionLoading("", true);
            internet_page.showAlert("There is no enrolled product to your beneficiary.");
            internet_page._acctNotValid = true;
        }
        else
        {
            internet_page.showTransactionLoading("", true);
            internet_page.showAlert("Please check account status of your beneficiary. Kindly call branch.");
            internet_page._acctNotValid = true;
        }
    }
    
    this.getAccountInquiry_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            internet_page.showTransactionLoading("", true);
            if (response.toUpperCase().indexOf("DORMANT") >= 0 || response.toUpperCase().indexOf("CLOSED") >= 0 || response.toUpperCase().indexOf("INVALID") >= 0 || response.toUpperCase().indexOf("PROBLEM") >= 0)
            {
                internet_page.showAlert("Please check account status of your beneficiary. Kindly call branch.");
                internet_page._acctNotValid = true;
            }
            else
            {
                internet_page._acctNotValid = false;
            }
        }
        else
        {
            internet_page._acctNotValid = true;
            internet_page.showAlert("Please check account status of your beneficiary. Kindly call branch.");
        }
        if (internet_page._aggCheckIDAmount <= 0)
        {
            internet_page.check_remitter_ids_required();
        }
    }
    
    this.getOSA = function(beneficiaryzipseqid)
    {
        internet_page.ajaxTimeout();
        internet_page.showTransactionLoading("Checking serviceable area....", false);
        return IRSWorld.Internet.InternetPageV2.getOSA(beneficiaryzipseqid.toString(), internet_page.getOSA_callback);
    }
    
    this.getOSA_callback = function(result)
    {
        internet_page._isOSA = false;
        var response = result.value;
        internet_page.showTransactionLoading("", true);
        if (response != null && response != "" && response != undefined)
        {
            if (response == true || response == "true")
            {
                internet_page._isOSA = true;
                internet_page.showAlert("Destination is not a serviceable area. Please transact using advise and pay or go to the nearest PNB branch.");
            }
            else
            {
                internet_page._acctNotValid = false;
                if (internet_page._aggCheckIDAmount <= 0)
                {
                    internet_page.check_remitter_ids_required();
                }
            }
        }
        else
        {
            internet_page._isOSA = false;
        }
    }
    
    this.getExchangeRate_callback = function(result)
    {
        internet_page.showTransactionLoading("", true);
        if (result != null && result != undefined)
        {
            var dt = result.value;
            if(dt.Rows.length > 0)
            {
                internet_page.enterValue(internet_page.getTableValueRow(dt, "SourceToTarget", 0), "txtCExchangeRate", false);
            }
            else
            {
                internet_page.showAlert("Please set Currency rates.");
            }
            internet_page.showTransactionLoading("Getting Charge Type", false);
            internet_page.getFilteredChargeType(internet_page._ProductCode.toString());
        }
    }
    
    this.set_computationlevel = function(complevel)
    {
        internet_page._computation = complevel;
    }
    
    this.changeBeneProduct = function()
    {
        internet_page._isOSA = false;
        internet_page._acctNotValid = false;
        if(internet_page.tblBeneProduct != undefined && internet_page.tblBeneProduct != null && internet_page.tblBeneProduct != "undefined" && internet_page.tblBeneProduct != "")
        {            
            internet_page._BeneProductSEQID = internet_page.getDDLValue("ddlBeneficiaryProduct", true);//document.getElementById("ddlBeneficiaryProduct").options[document.getElementById("ddlBeneficiaryProduct").selectedIndex].value;
            for (var i = 0; i <= (internet_page.tblBeneProduct.Rows.length - 1); i++)
            {
                if(internet_page.tblBeneProduct.Rows[i]["SEQ_ID"] == internet_page._BeneProductSEQID)
                {
                    internet_page.showTransactionLoading("Getting CIF Information....", false);
                    internet_page.enterValue(internet_page.getTableValueRow(internet_page.tblBeneProduct, "Currency", i), "spnCNetToBenefAmountCurrency", true);
                    internet_page._TargetCurrency = internet_page.getTableValueRow(internet_page.tblBeneProduct, "CurrencyValue", i);
                    internet_page.enterValue(internet_page.getDDLValue("ddlRemittingCurrency", false) + " to " + internet_page.getValue("spnCNetToBenefAmountCurrency", true), "spnCExchangeRateCurrency", true);
                    internet_page._ProductCode = internet_page.getTableValueRow(internet_page.tblBeneProduct, "ProductCode", i);
                    internet_page._ImplementationCode = internet_page.getTableValueRow(internet_page.tblBeneProduct, "ImplementationCode", i);
                    internet_page._accountno = internet_page.getTableValueRow(internet_page.tblBeneProduct, "AccountNo", i);
                    internet_page._accountSEQID = internet_page.getTableValueRow(internet_page.tblBeneProduct, "AccountSeqId", i);
                    IRSWorld.Internet.InternetPageV2.getExchangeRate(internet_page._RemittingCurrency.toString(), internet_page._TargetCurrency.toString(), internet_page.getExchangeRate_callback);
                }
            }
            if (internet_page._ProductCode.toString() == "8" || internet_page._ProductCode.toString() == "9")
            {
                if(internet_page.getValue("spnBeneficiaryAddress",true) == "" || internet_page.getValue("spnBeneficiaryZipCode",true) == "")
                {
                    if(confirm("Incomplete Beneficiary address details for this transaction. Kindly update."))
                    {
                        internet_page.showAlert("Wala pa yung CIF.");
                    }
                    else
                    {
                        internet_page.initializePage();
                    }
                }
                else
                {
                    internet_page.getOSA(internet_page._beneficiaryZipCodeSEQID);
                }
            }
            else
            {
                internet_page.getAccountInquiry();
            }
        }
        else
        {
            internet_page.showAlert("Session Timeout.");
        }
    }
    
    this.getFilteredChargeType = function(productCode)
    {
        IRSWorld.Internet.InternetPageV2.FilteredChargeType(productCode, internet_page.getFilteredChargeType_callback)
    }
    
    this.getFilteredChargeType_callback = function(result)
    {
        internet_page.showTransactionLoading("", true);
        if (result != null && result != undefined)
        {
            internet_page.tblChargeType = result.value;
            internet_page.clearDDL("ddlChargeType");
            internet_page.fillDDL(result.value, "ddlChargeType", "FldDescription", "FldValue", "internet_page._ChargeType", ", ");
            document.getElementById("ddlChargeType").selectedIndex = 1;
        }
    }
    
    this.clearCompute = function()
    {
        internet_page.removeValue("txtCNetAmount", false);
        internet_page.enterValue("0.00", "txtCNetAmount", false);
        internet_page.removeValue("txtCBankChargeAmount", false);
        internet_page.enterValue("0.00", "txtCBankChargeAmount", false);
        internet_page.removeValue("txtCGrossAmount", false);
        internet_page.enterValue("0.00", "txtCGrossAmount", false);
        internet_page.removeValue("txtCNetToBenefAmount", false);
        internet_page.enterValue("0.00", "txtCNetToBenefAmount", false);
    }
    
    this.getCharges = function(isoverride)
    {
        var amount = "";
        if(internet_page._computation.toString() == "1")
        {
            amount = internet_page.getValue("txtCGrossAmount", false);
        }
        else if(internet_page._computation.toString() == "2")
        {
            amount = internet_page.getValue("txtCNetAmount", false);
        }
        else if(internet_page._computation.toString() == "3")
        {
            amount = internet_page.getValue("txtCNetToBenefAmount", false);
        }
        internet_page._ChargeType = internet_page.getDDLValue("ddlChargeType", true);
        internet_page.showTransactionLoading("Transaction being compute. Please wait...", false);
        IRSWorld.Internet.InternetPageV2.getCharges(amount, internet_page._ProductCode.toString(), internet_page._ImplementationCode.toString(), internet_page._RemittingCurrency.toString(), internet_page._TargetCurrency.toString(), String(internet_page._ChargeType), String(internet_page._PaymentMethod), String(internet_page._computation), true, ((isoverride == true) ? true : false), String(internet_page._vippercent), internet_page.getCharges_callback);
    }
    
    this.getCharges_callback = function(result)
    {
        internet_page.showTransactionLoading("", true);
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            if (response == "NOCHARGE")
            {
                internet_page.showAlert("Session Timeout.");
            }
            else
            {
                internet_page._arrCalc = response.split("|");
                if(internet_page._arrCalc[36] == "")
                {
                    internet_page.showAlert("Last maintained currency rates is incomplete. Kindly complete it first before proceeding with transaction.");
                    return;
                }
                internet_page.enterValue(internet_page._arrCalc[27], "txtCNetAmount", false);
                internet_page.enterValue(internet_page._arrCalc[31], "txtCBankChargeAmount", false);
                internet_page.enterValue(internet_page._arrCalc[24], "txtCGrossAmount", false);
                internet_page.enterValue(internet_page._arrCalc[29], "txtCNetToBenefAmount", false);
                internet_page.edButtons("btnOverride", false);
                internet_page.edButtons("btnSubmit", false);
            }
        }
    }
    
    this.setOverride = function()
    {
        IRSWorld.Internet.InternetPageV2.setOverride(internet_page._arrCalc[1], internet_page.getValue("txtCExchangeRate", false), internet_page._arrCalc[31], internet_page.getValue("txtCBankChargeAmount", false));
        internet_page.getCharges(true);
    }
    
    this.rem_aggregates = function()
    {
        if (internet_page._aggregate == null)
        {
            internet_page._aggregate = new rem_aggregates();
        }
        //internet_page._showAggregate = true;
        internet_page._aggregate.isCommit = true;
        internet_page._aggregate.open_remitter_aggregates(internet_page._agentSelectedRemitterID, internet_page._RemittingCurrency);
    }
    
    this.check_remitter_ids_required = function()
    {
        internet_page.ajaxTimeout();
        if (internet_page._agentSelectedRemitterID != "" && internet_page._agentSelectedRemitterID != null && internet_page._agentSelectedRemitterID != undefined && internet_page._agentSelectedRemitterID != "undefined" && internet_page._agentSelectedRemitterID != "0")
        {
            internet_page.showTransactionLoading("Checking required IDs.", false);
            IRSWorld.Internet.InternetPageV2.check_remitter_ids_required(internet_page._agentSelectedRemitterID,internet_page.check_remitter_ids_required_callback);
        }
    }
    this.check_remitter_ids_required_callback = function(result)
    {
        internet_page.showTransactionLoading("", true);
        if (result != null && result.value != null)
        {
            var s = result.value.split("~");
            
            if (s.length > 0)
            {
                internet_page.edButtons("btnCompute",true);
                if (parseInt(s[0]) == 0)
                {
                    if (internet_page._aggCheckIDAmount > 0)
                    {
                        internet_page._aggCheckIDCIF = true;
                        if (s[1].indexOf("Place Of Issuance:") >= 0)
                        {
                            internet_page._isplaceofissuance = true;
                            internet_page.showAlert(s[1]);
                            return agentRem.loadIDsForm();
                        }
                        else if (s[1].indexOf("Expiration Date:") >= 0)
                        {
                            internet_page._isplaceofissuance = true;
                            internet_page.showAlert(s[1]);
                            return agentRem.loadIDsForm();
                        }
                        else
                        {
                            if (parseFloat(internet_page.getValue("txtCGrossAmount", false).replace(",", "")) >= parseFloat(internet_page._aggCheckIDAmount))
                            {
                                var iconfirm = confirm(s[1] + "\n Cannot process transaction without ID Presented. Please update Remitter information.");

                                if (iconfirm == true)
                                {
                                    return agentRem.loadIDsForm();
                                }
                            }
                            else
                            {
                                if (internet_page._isSubmit == true)
                                {    
                                    if (internet_page._aggCheckIDAmount > 0)
                                    {
                                        internet_page.checkAgentSystemControl();
                                    }
                                    else
                                    {
                                        internet_page.checkAgentSystemControl();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (s[1].indexOf("Place Of Issuance:") >= 0)
                        {
                            internet_page._isplaceofissuance = true;
                            internet_page.showAlert(s[1]);
                            return agentRem.loadIDsForm();
                        }
                        else if (s[1].indexOf("Expiration Date:") >= 0)
                        {
                            internet_page._isplaceofissuance = true;
                            internet_page.showAlert(s[1]);
                            return agentRem.loadIDsForm();
                        }
                        else
                        {
                            var iconfirm = confirm(s[1] + "\n Cannot process transaction without ID Presented. Please update Remitter information.");

                            if (iconfirm == true)
                            {
                                return agentRem.loadIDsForm();
                            }
                        }
                    }                    
                }
                else
                {
//                    if (internet_page._acctNotValid == false)
//                    {
//                        internet_page.edButtons("ddlBeneficiary",false);
//                        internet_page.edButtons("btnSubmit",false);
//                        internet_page.edButtons("txtAmount", false);
//                    }
                    if (internet_page._isSubmit == true)
                    {    
                        if (internet_page._aggCheckIDAmount > 0)
                        {
                            internet_page.checkAgentSystemControl();
                        }
                        else
                        {
                            internet_page.checkAgentSystemControl();
                        }
                    }
                }
            }
            else
            {
//                if (internet_page._acctNotValid == false)
//                {
//                    internet_page.edButtons("ddlBeneficiary",false);
//                    internet_page.edButtons("btnSubmit",false);
//                    internet_page.edButtons("txtAmount", false);
//                }
                if (internet_page._isSubmit == true)
                {
                    if (internet_page._aggCheckIDAmount > 0)
                    {
                        internet_page.checkAgentSystemControl();
                    }
                    else
                    {
                        internet_page.checkAgentSystemControl();
                    }
                }
            }
        }
    }
        
    this.validateTransaction = function()
    {
        internet_page._isSubmit = true;
        if (internet_page._isCashAggLimit == true)
        {
            if (internet_page.getDDLValue("ddlPaymentMethod").match("Cash"))
            {
                internet_page.RemitterCashAgg();
            }
            else
            {
                internet_page.AggregateInternetRem();
            }
        }
        else
        {
            internet_page.AggregateInternetRem();
        }
    }
    
    this.RemitterCashAgg = function()
    {
        internet_page.ajaxTimeout();
        internet_page.showTransactionLoading("Verifying Cash Transaction Limit.", false);
        IRSWorld.Internet.InternetPageV2.RemitterCashAgg(internet_page._agentSelectedRemitterID, ((internet_page._arrCalc[35] == "0") ? internet_page._RemittingCurrency.toString() : internet_page._arrCalc[35]),internet_page.getValue("txtCGrossAmount", false), internet_page.RemitterCashAgg_callback);
    }
    
    this.RemitterCashAgg_callback = function(result)
    {
        internet_page.showTransactionLoading("", true);
        var response = result.value;
        if(response != null && response != "null" && response != undefined && response != "undefined")
        {
            if (parseFloat(internet_page.getTableValueRow(result.value,"GrossCashAmount",0)) + parseFloat(internet_page.getValue("txtCGrossAmount", false).replace(",", "")) > parseFloat(internet_page._cashAggAmtLimit))
            {
                internet_page.edButtons("btnSubmit", true);
                internet_page.showAlert("Aggregate cash transaction now ( " + (parseFloat(internet_page.getTableValueRow(result.value,"GrossCashAmount",0)) + parseFloat(internet_page.getValue("txtCGrossAmount", false).replace(",", ""))) + " ) and exceeds limit.");
            }
            else
            {
                internet_page.AggregateInternetRem();
            }
        }
        else
        {
            internet_page.AggregateInternetRem();
        }
    }
    
    this.AggregateInternetRem = function()
    {
        internet_page.ajaxTimeout();
        if(internet_page.tblBranchSetup == null || internet_page.tblBranchSetup == "null" || internet_page.tblBranchSetup == undefined || internet_page.tblBranchSetup == "undefined")
        {
            internet_page.showAlert("Session Timeout.");
        }
        else
        {
            internet_page.showTransactionLoading("Verifying Transaction Limit.", false);
            IRSWorld.Internet.InternetPageV2.checkInternetRemittanceAggregateLimit(internet_page._agentSelectedRemitterID, ((internet_page._arrCalc[35] == "0") ? internet_page._RemittingCurrency.toString() : internet_page._arrCalc[35]),internet_page.getValue("txtCGrossAmount", false), internet_page.AggregateInternetRem_callback);
        }
    }
    
    this.AggregateInternetRem_callback = function(result)
    {
        internet_page.showTransactionLoading("", true);
        if(result.value == true)
        {
            if (internet_page._aggCheckIDAmount > 0)
            {
                if (parseFloat(internet_page.getValue("txtCGrossAmount", false).replace(",", "")) >= parseFloat(internet_page._aggCheckIDAmount))
                {
                    if(confirm("Please present a valid ID for verification. Proceed?"))
                    {
                        internet_page.check_remitter_ids_required();
                    }
                    else
                    {
                        //internet_page.createTransaction('create');
                        return agentRem.loadIDsForm();
                    }
                }
                else
                {
                    internet_page.check_remitter_ids_required();
                }
            }
            else
            {
                internet_page.check_remitter_ids_required();
            }
        }
        else
        {
            if (internet_page._aggOutrightReject == true)
            {
                internet_page.showAlert("Amount exceeds maximum remittance aggregate limit of " + internet_page.getDDLValue("ddlRemittingCurrency", false) + " " + internet_page.formatCurrency(Number(internet_page._remittanceAmtLimit)));
                internet_page.initializePage();
            }
            else if (internet_page._aggCheckIDAmount > 0)
            {
                if (parseFloat(internet_page.getValue("txtCGrossAmount", false).replace(",", "")) >= parseFloat(internet_page._aggCheckIDAmount))
                {
                    if(confirm("Please present a valid ID for verification. Proceed?"))
                    {
                        internet_page.check_remitter_ids_required();
                    }
                    else
                    {
                        //internet_page.createTransaction('create');                        
                        return agentRem.loadIDsForm();
                    }
                }
                else
                {                    
                    internet_page.check_remitter_ids_required();
                }
            }
            else
            {
                if(confirm("Amount exceeds remittance maximum limit. Remitter must provide latest Bank Statement and/or withdrawal receipt and proof of fund."))
                {
                    internet_page.check_remitter_ids_required();
                }
                else
                {
                    internet_page.initializePage();
                }
            }
        }
    }
    
    this.getGFCAggregate = function()
    {
        internet_page.ajaxTimeout();
        var beneficiary_value = internet_page._agentSelectedBeneficiary.split("|");
        
        if (internet_page._ProductCode.toString() == "10" || internet_page._ProductCode.toString() == "22") //Rapid Remit & GFC
        {
            if (internet_page._accountno != null && internet_page._accountno != "" && internet_page._accountno != undefined)
            {                
                if (internet_page._arrCalc != null || internet_page._arrCalc != undefined)
                {
                    if (internet_page._arrCalc[28] == null || internet_page._arrCalc[28] == .00)
                    {
                        //internet_page.getCharges(false);
                        internet_page.showAlert("Session Timeout.");
                    }
                    internet_page.showTransactionLoading("Checking GFC Aggregation. Please wait....", false);
                    return IRSWorld.Internet.InternetPageV2.getGFCAggregate(beneficiary_value[0], internet_page._accountno, internet_page._TargetCurrency, internet_page._arrCalc[29], internet_page.getGFCAggregate_callback);
                }
                else
                {
                    internet_page.showAlert("Session Timeout.");
                    //internet_page.showAlert("There is no applicable charge for " + internet_page.getValue("spnRemittingCurrency", true) + " currency. Please select another currency.");
                }
            }
            else
            {
                internet_page.showAlert("The account number is missing. Transaction cannot be process.");
            }
        }
        else
        {
            internet_page.verifyOAAMTransaction();
        }
    }
    
    this.getGFCAggregate_callback = function(result)
    {
        var response = result.value;
        internet_page.showTransactionLoading("", true);
        if (response == true)
        {
            internet_page.verifyOAAMTransaction();
        }
        else
        {
            internet_page.showAlert(IRSWorld.Internet.InternetPageV2.getGFCAggregate_Message().value);
        }
    }
    
    this.checkAgentSystemControl = function()
    {
        if(internet_page._sysControl == true)
        {
            if (Number(parseFloat(internet_page.getValue("txtCGrossAmount", false).replace(",", "")).toFixed(2)) >= Number(parseFloat(internet_page._sysControlAmount).toFixed(2)))
            {
                agentRem.loadSystemControl();
            }
            else
            {
                internet_page.submitRemittance();
            }
        }
        else
        {
            internet_page.submitRemittance();
        }
    }
    
    this.getOBPSAccount = function(accountno)
    {
        return IRSWorld.Internet.InternetPageV2.getOBPSAccount(accountno).value;
    }
    
    this.submitRemittance = function()
    {
        var isOBPS = internet_page.getOBPSAccount(internet_page._accountno);
        if (isOBPS == false)
        {
            internet_page.showTransactionLoading("Transaction is being processed. Please wait...", false);
            internet_page.getGFCAggregate();
        }
        else
        {
            internet_page.showAlert("Merchant accounts are not allowed. Please use the bills payment module.");
        }
    }
    
    this.verifyOAAMTransaction = function()
    {
        internet_page.ajaxTimeout();
        internet_page.showTransactionLoading("Transaction is being processed. Please wait...", false);
        IRSWorld.Internet.InternetPageV2.verifyOAAM(internet_page._accountno, internet_page._arrCalc[14], internet_page._arrCalc[29], internet_page.verifyOAAMTransaction_callback);
    }
    
    this.verifyOAAMTransaction_callback = function(result)
    {
        var response = result.value;
        internet_page.showTransactionLoading("", true);
        if (response != null && response != "" && response != undefined)
        {
            if (response == "OK")
            {
                internet_page.transmitRemittance();
            }
            else
            {
                if (response == "Transaction Blocked.")
                {
                    spawn_errpage("Transaction blocked! Please contact your system administrator and/or PNB for assistance.");
                }
                else
                {
                    internet_page.transmitRemittance();
                }
            }
        }
        else
        {
            internet_page.showAlert("No response from OAAM. Please try again.");
        }
    }
    
    this.transmitRemittance = function()
    {
        internet_page.ajaxTimeout();
        internet_page.showTransactionLoading("Transaction is being processed. Please wait...", false);
        IRSWorld.Internet.InternetPageV2.submitRemittance(internet_page.createRemittance(), internet_page.transmitRemittance_callback);
    }
    
    this.transmitRemittance_callback = function(result)
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
        //internet_page.showLoading("", true);
    }
    
    this.createRemittance = function()
    {
        var remittance = "";
        var remitMethod = "8";
        var beneAddress = "";
        var beneContacts = "";
        var printed = "FALSE"
        var tranxsrce = 0;
        var specialmsg = "";
        var beneficiary_value = internet_page._agentSelectedBeneficiary.split("|");
        
        internet_page._auditRemittance = "";
        internet_page._ORNo = IRSWorld.Internet.InternetPageV2.generateORNo().value;
        internet_page._tranxDate = IRSWorld.Internet.InternetPageV2.getTransactionDate().value;
        internet_page._serverTime = IRSWorld.Internet.InternetPageV2.getServerTime().value
        
        if (internet_page._isAgent == true && internet_page._agentIsApprover == false)
        {
            internet_page._tranStatus = "15";
        }
        
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
        //1,3,4,5,7,10,11
        if (internet_page._ImplementationCode == "1" || internet_page._ImplementationCode == "3" || internet_page._ImplementationCode == "4" || internet_page._ImplementationCode == "5" || internet_page._ImplementationCode == "7" || internet_page._ImplementationCode == "10" || internet_page._ImplementationCode == "11")
        {
            printed = "TRUE";
        }
        
        if (document.getElementById("trAgent5").style.display == "none")
        {
            tranxsrce = internet_page._TranxSource;
        }
        else
        {
            tranxsrce = internet_page.getDDLValue("ddlTranxSource", true);
        }
        
        if (document.getElementById("trAgent6").style.display == "none")
        {
            specialmsg = "NULL";
        }
        else
        {
            specialmsg = internet_page.getValue("txtSpecialMessage", false)
        }
        
        remittance = internet_page._userID                                                      + "|"; //UserID        
        remittance += remitMethod                                                               + "|"; //RemitMethod
        remittance += "NULL"                                                                    + "|"; //RemitStationID
        remittance += internet_page._userLocation                                               + "|"; //RemittingBranch
        remittance += "FALSE"                                                                   + "|"; //ChargesLocally
        remittance += "NULL"                                                                    + "|"; //ReferenceNo
        remittance += internet_page._ORNo                                                       + "|"; //ExtlReferenceNo
        remittance += internet_page._tranStatus                                                 + "|"; //Transtatus
        remittance += internet_page._tranxDate                                                  + "|"; //TranxDate
        remittance += internet_page._arrCalc[39]                                                + "|"; //ProductChargesID
        remittance += internet_page._agentSelectedRemitterID                                    + "|"; //RemitterCIFID
        remittance += beneficiary_value[0]                                                      + "|"; //BeneficiaryCIFID
        remittance += internet_page._accountSEQID                                               + "|"; //AccountSEQID
        remittance += ((internet_page._arrCalc[35] == "0") ? internet_page._RemittingCurrency.toString() : internet_page._arrCalc[35])   + "|"; //RemittingCurrency
        remittance += internet_page._arrCalc[0]                                                 + "|"; //LocalCurrency
        remittance += internet_page._arrCalc[24]                                                + "|"; //GrossRemAmount
        remittance += internet_page._arrCalc[27]                                                + "|"; //NetRemAmount
        remittance += internet_page._arrCalc[14]                                                + "|"; //NettoBeneCurrency
        remittance += internet_page._arrCalc[29]                                                + "|"; //NettoBeneAmount
        remittance += internet_page._arrCalc[28]                                                + "|"; //NettoBeneAmountPHP
        remittance += internet_page._arrCalc[31]                                                + "|"; //TotalLocalCharge
        remittance += tranxsrce                                                                 + "|"; //TransactionSource
        remittance += internet_page._receiptMode                                                + "|"; //ReceiptMode
        remittance += internet_page.getDDLValue("ddlPurpose", true)                             + "|"; //TransactionPurpose
        remittance += internet_page.getTableValue(internet_page.tblDefaults, "SourceOfFunds")   + "|"; //SourceofFunds
        remittance += "NULL"                                                                    + "|"; //OtherPurpose
        remittance += specialmsg                                                                + "|"; //SpecialMessage
        remittance += printed                                                                   + "|"; //Printed
        remittance += internet_page._arrCalc[13]                                                + "|"; //ForexRemittingToAccount
        remittance += internet_page._arrCalc[20]                                                + "|"; //ForexRemittingToLocal
        remittance += internet_page._arrCalc[21]                                                + "|"; //ForexRemittingToUSD
        remittance += internet_page._arrCalc[18]                                                + "|"; //ForexRemittingToPHP
        remittance += internet_page._arrCalc[16]                                                + "|"; //ForexRemittingToEUR
        remittance += internet_page._arrCalc[7]                                                 + "|"; //ForexLocalToRemitting
        remittance += internet_page._arrCalc[10]                                                + "|"; //ForexLocalToUSD
        remittance += internet_page._arrCalc[4]                                                 + "|"; //ForexLocalToPHP
        remittance += internet_page._tranxDate                                                  + "|"; //DatePostedOverseas
        remittance += internet_page._serverTime                                                 + "|"; //DatePostedLocal
        remittance += internet_page._tranxDate                                                  + "|"; //DateImplementedOverseas
        remittance += internet_page._serverTime                                                 + "|"; //DateImplementedLocal
        remittance += "0"                                                                       + "|"; //BatchNo
        remittance += internet_page._arrCalc[38]                                                + "|"; //Total_Backend_Charges
        remittance += beneAddress                                                               + "|"; //BeneficiaryAddressSEQID
        remittance += beneContacts                                                              + "|"; //BeneficiaryContactSEQID
        remittance += ""                                    //RemitterAccountNo
        
        internet_page._auditRemittance = "UserID~"                     + internet_page._userID + "|";
        internet_page._auditRemittance += "RemitMethod~"               + remitMethod + "|";
        internet_page._auditRemittance += "RemitStationID~NULL|";
        internet_page._auditRemittance += "RemittingBranch~"           + internet_page._userLocation + "|";
        internet_page._auditRemittance += "ChargesLocally~FALSE|";
        internet_page._auditRemittance += "ReferenceNo~NULL|";
        internet_page._auditRemittance += "ExtlReferenceNo~"           + internet_page._ORNo + "|";
        internet_page._auditRemittance += "Transtatus~"                + internet_page._tranStatus + "|";
        internet_page._auditRemittance += "TranxDate~"                 + internet_page._tranxDate + "|";
        internet_page._auditRemittance += "ProductChargesID~"          + internet_page._arrCalc[39] + "|";
        internet_page._auditRemittance += "RemitterCIFID~"             + internet_page._agentSelectedRemitterID + "|";
        internet_page._auditRemittance += "BeneficiaryCIFID~"          + beneficiary_value[0] + "|";
        internet_page._auditRemittance += "AccountSEQID~"              + internet_page._accountSEQID + "|";
        internet_page._auditRemittance += "RemittingCurrency~"         + ((internet_page._arrCalc[35] == 0) ? internet_page._RemittingCurrency.toString() : internet_page._arrCalc[35]) + "|";
        internet_page._auditRemittance += "LocalCurrency~"             + internet_page._arrCalc[0] + "|";
        internet_page._auditRemittance += "GrossRemAmount~"            + internet_page._arrCalc[24] + "|";
        internet_page._auditRemittance += "NetRemAmount~"              + internet_page._arrCalc[27] + "|";
        internet_page._auditRemittance += "NettoBeneCurrency~"         + internet_page._arrCalc[14] + "|";
        internet_page._auditRemittance += "NettoBeneAmount~"           + internet_page._arrCalc[29] + "|";
        internet_page._auditRemittance += "NettoBeneAmountPHP~"        + internet_page._arrCalc[28] + "|";
        internet_page._auditRemittance += "TotalLocalCharge~"          + internet_page._arrCalc[31] + "|";
        internet_page._auditRemittance += "TransactionSource~"         + tranxsrce + "|";
        internet_page._auditRemittance += "ReceiptMode~"               + internet_page._receiptMode + "|";
        internet_page._auditRemittance += "TransactionPurpose~"        + internet_page.getDDLValue("ddlPurpose", true) + "|";
        internet_page._auditRemittance += "SourceofFunds~"             + internet_page.getTableValue(internet_page.tblDefaults, "SourceOfFunds") + "|";
        internet_page._auditRemittance += "OtherPurpose~NULL|";
        internet_page._auditRemittance += "SpecialMessage~"            + specialmsg + "|";
        internet_page._auditRemittance += "Printed~" + printed + "|";
        internet_page._auditRemittance += "ForexRemittingToAccount~"   + internet_page._arrCalc[13] + "|";
        internet_page._auditRemittance += "ForexRemittingToLocal~"     + internet_page._arrCalc[20] + "|";
        internet_page._auditRemittance += "ForexRemittingToUSD~"       + internet_page._arrCalc[21] + "|";
        internet_page._auditRemittance += "ForexRemittingToPHP~"       + internet_page._arrCalc[18] + "|";
        internet_page._auditRemittance += "ForexRemittingToEUR~"       + internet_page._arrCalc[16] + "|";
        internet_page._auditRemittance += "ForexLocalToRemitting~"     + internet_page._arrCalc[7] + "|";
        internet_page._auditRemittance += "ForexLocalToUSD~"           + internet_page._arrCalc[10] + "|";
        internet_page._auditRemittance += "ForexLocalToPHP~"           + internet_page._arrCalc[4] + "|";
        internet_page._auditRemittance += "DatePostedOverseas~"        + internet_page._tranxDate + "|";
        internet_page._auditRemittance += "DatePostedLocal~"           + internet_page._serverTime + "|";
        internet_page._auditRemittance += "DateImplementedOverseas~"   + internet_page._tranxDate + "|";
        internet_page._auditRemittance += "DateImplementedLocal~"      + internet_page._serverTime + "|";
        internet_page._auditRemittance += "BatchNo~0|";
        internet_page._auditRemittance += "Total_Backend_Charges~"     + internet_page._arrCalc[38] + "|";
        internet_page._auditRemittance += "BeneficiaryAddressSEQID~"   + beneAddress + "|";
        internet_page._auditRemittance += "BeneficiaryContactSEQID~"   + beneContacts + "|";
        internet_page._auditRemittance += "RemitterAccountNo~"         + "";
        return remittance;
    }
    
    this.getReferenceNo = function()
    {
        internet_page.ajaxTimeout();
        internet_page.showTransactionLoading("Transaction is being processed. Please wait...", false);
        return IRSWorld.Internet.InternetPageV2.getReferenceNo(internet_page._ORNo, internet_page._tranxDate, internet_page.getReferenceNo_callback);
    }
    
    this.getReferenceNo_callback = function(result)
    {
        var response = result.value;
        var refno = "";
        var erefno = "";
        var txnseqid = "";
        
        internet_page._fcSuccess = false;
        internet_page.showTransactionLoading("", true);
        if (response != null && response != "" && response != undefined)
        {
            internet_page.tblRemittance = response;
            refno = internet_page.getTableValue(internet_page.tblRemittance, "ReferenceNo");
            erefno = internet_page.getTableValue(internet_page.tblRemittance, "ExtlReferenceNo");
            txnseqid = internet_page.getTableValue(internet_page.tblRemittance, "SEQ_ID");
            
            internet_page._refno = refno;
            internet_page._txnseqid = txnseqid;
            internet_page._erefno = erefno;
            internet_page.writeLogger(internet_page._auditRemittance, "TBL_REMITTANCE", txnseqid);
            internet_page.submitRemittanceCharges(txnseqid);
        }
    }
    
    this.submitRemittanceCharges = function(seqid)
    {
        internet_page.ajaxTimeout();
        internet_page.showTransactionLoading("Transaction is being processed. Please wait...", false);
        IRSWorld.Internet.InternetPageV2.submitRemittanceCharges(seqid, internet_page.submitRemittanceCharges_callback);
    }
    
    this.submitRemittanceCharges_callback = function(result)
    {
        var response = result.value;
        internet_page.showTransactionLoading("", true);
        if (response != null && response != "" && response != undefined)
        {
            if (response != "1")
            {
                internet_page.showAlert("Transaction was successfully committed however error was encountered while saving charges. " + response + ". \r\n\r\n Please report to the nearest PNB branch.");
            }
            else
            {
                internet_page.submitGFC(internet_page._refno);
            }
        }
    }
    
    this.submitGFC = function(refno)
    {
        internet_page.ajaxTimeout();
        internet_page.showTransactionLoading("Transaction is being processed. Please wait...", false);
        IRSWorld.Internet.InternetPageV2.submitGFC(internet_page._refno, internet_page._accountno, internet_page._TargetCurrency, internet_page._arrCalc[29], internet_page.submitGFC_callback);
    }
    
    this.submitGFC_callback = function(result)
    {
        var response = result.value;
        internet_page.showTransactionLoading("", true);
        if (response != null && response != "" && response != undefined)
        {
            if (internet_page.getDDLValue("ddlPaymentMethod").match("Checks"))
            {
                internet_page.check_payment();
            }
            else if (internet_page.getDDLValue("ddlPaymentMethod").match("ACH"))
            {
                internet_page.ach_payment();
            }
            else
            {
                if (internet_page._tranStatus == "1") //New Transaction
                {
                    internet_page.showTransactionLoading("Transaction is being processed. Please wait...", false);
                    internet_page.processTransaction(internet_page._txnseqid);
                }
                else
                {
                    internet_page.transactionSuccess("");
                }
            }
            internet_page._tranStatus = "";            
            internet_page._accountno = "";
            IRSWorld.Internet.InternetPageV2.updateOAAM(internet_page._fcSuccess, internet_page._refno, internet_page._erefno);
        }
        else
        {
            internet_page.showAlert("Session Timeout.");
        }
    }
    
    this.check_payment = function()
    {
        this.cheque_payments = new rem_modal_check_payments();
        this.cheque_payments.transaction_id = internet_page._txnseqid;
        this.cheque_payments.open_check_payment_form("internet_page.cheque_payments");
    }
    
    this.check_payment_result = function()
    {
       internet_page.check_is_post_dated();
    }
    
    this.check_is_post_dated = function()
    {
        internet_page.ajaxTimeout();
        internet_page.showTransactionLoading("Validating cheque date ...", false);
        IRSWorld.Internet.InternetPageV2.check_is_post_dated(internet_page.cheque_payments.clearing_date, internet_page.check_is_post_dated_callback);
    }
    
    this.check_is_post_dated_callback = function(result)
    {
        if (result != null && result.value != null)
        {
            if (result.value == true)
            {
                internet_page.run_transaction_status_update('14');
                internet_page._tranStatus == "14"
            }
            else
            {
                if (internet_page._isAgent == true && internet_page._agentIsApprover == false)
                {
                    internet_page._tranStatus = "15";
                }
                else if (internet_page._isAgent == true && internet_page._agentIsApprover == true)
                {
                    internet_page._tranStatus = "1";
                }
            }
        }
        internet_page.showTransactionLoading("", true);
        internet_page.cheque_payments.commit_transaction();
    }
    
    this.check_payment_commit_result = function(param)
    {
        var s = param.split('~'); this.payment_error_msg = null;

        if (s != null)
        {
            if (parseInt(s[1]) == false)
            {
                internet_page.showAlert("ERROR ENCOUNTERED UPDATING CHEQUE INFORMATION: " + s[1]);
            }
            else
            {
                if (internet_page._tranStatus == "1") //New Transaction
                {
                    internet_page.showTransactionLoading("Transaction is being processed. Please wait...", false);
                    internet_page.processTransaction(internet_page._txnseqid);
                }
                else
                {
                    internet_page.transactionSuccess("");
                }
            }
        }
    }
    
    this.ach_payment = function()
    {
        internet_page.ach_payments = new ach_payment();
        internet_page.ach_payments.create_clearing_popup();
        internet_page.ach_payments.transaction_id = internet_page._txnseqid;
    }
    
    this.chk_ach_pay_result = function()
    {
        internet_page.chk_ach_post_dated();
    }
    
    this.chk_ach_post_dated = function()
    {
        internet_page.ajaxTimeout();
        internet_page.showTransactionLoading("Validating cheque date ...", false);
        IRSWorld.Internet.InternetPageV2.check_is_post_dated(internet_page.ach_payments.clearing_date, internet_page.chk_ach_post_dated_callback);
    }
    
    this.chk_ach_post_dated_callback = function(result)
    {
        if (result != null && result.value != null)
        {
            if (result.value == true)
            {
                internet_page.run_transaction_status_update('14');
                internet_page._tranStatus == "14"
            }
            else
            {
                if (internet_page._agentIsApprover == false)
                {
                    internet_page._tranStatus = "15";
                }
                else if (internet_page._agentIsApprover == true)
                {
                    internet_page._tranStatus = "1";
                }
            }
        }
        internet_page.showTransactionLoading("", true);
        internet_page.ach_payments.commit_transaction();    
    }
    
    this.ach_payment_commit_result = function(param)
    {
        this.payment_error_msg = null;

        if (param != null)
        {
            if (param == false)
            {
                internet_page.showAlert("Error encountered saving ach payment");
            }
            else
            {
                if (internet_page._tranStatus == "1") //New Transaction
                {
                    internet_page.showTransactionLoading("Transaction is being processed. Please wait...", false);
                    internet_page.processTransaction(internet_page._txnseqid);
                }
                else
                {
                    internet_page.transactionSuccess("");
                }
            }
        }
    }
    
    this.run_transaction_status_update = function(transaction_status)
    {
        var data_row = new Array();

        data_row[data_row.length] = "Transtatus~" + transaction_status;
        data_row = data_row.join("|");
        internet_page.ajaxTimeout();
        IRSWorld.TranClass.RemTranSaving.update_otc_transaction_status(data_row, internet_page._txnseqid);    
    }
    
    this.processTransaction = function(seqid)
    {
        internet_page.ajaxTimeout();        
        IRSWorld.Internet.InternetPageV2.processTransaction(seqid, internet_page.processTransaction_callback);        
    }
    
    this.processTransaction_callback = function(param)
    {
        var response = param.value
        internet_page.showTransactionLoading("", true);
        if (response != null && response != "" && response != undefined)
        {
            internet_page.showTransactionLoading("Transaction is being processed. Please wait...", false);
            internet_page.transactionSuccess(response);
        }
        else
        {
            internet_page.transactionSuccess(param.error.Message);
        }
    }
    
    this.transactionSuccess = function(message)
    {
        internet_page.showTransactionLoading("", true);
        var response = "Transaction successfully commited with reference number: " + internet_page._refno + ".";
        var responseerror = "Transaction unsuccessfully commited with reference number: " + internet_page._refno + ".";
        if (message == "")
        {
            internet_page.showAlert(response);
            internet_page._fcSuccess = true;
        }
        else
        {
            var arrSplit = message.split("|");

            if (arrSplit[1] == "9" || arrSplit[1] == "256")
            {
                internet_page.showAlert(responseerror + "\r\n\r\n FC Response: " + arrSplit[0] + "\r\n Status:" + arrSplit[2]);
            }
            else
            {
                internet_page.showAlert(response + "\r\n\r\n FC Response: " + arrSplit[0] + "\r\n Status:" + arrSplit[2]);
            }

            if (arrSplit[1] == "9" || arrSplit[1] == "256")
            {
                internet_page._fcSuccess = false;
            }
            else
            {
                internet_page._fcSuccess = true;
            }
        }
        if (internet_page._fcSuccess != false)
        {
            internet_page.getRemittanceLimit();
        }
        else
        {
            internet_page.initializePage();
        }
    }
    
    this.getRemittanceLimit = function()
    {
        internet_page.showTransactionLoading("Checking transaction limit....", false);
        IRSWorld.Internet.InternetPageV2.getRemittanceLimit(internet_page.getRemittanceLimit_callback);
    }
    
    this.getRemittanceLimit_callback = function(param)
    {
        var remlim = param.value;
        internet_page.showTransactionLoading("", true);
        if  (remlim != "" && remlim != null && remlim != undefined && Number(parseFloat(remlim)) != 0)
        {
            if (internet_page._sysControlAmount != "" && internet_page._sysControlAmount != "0" && internet_page._sysControlAmount != "null" && Number(parseFloat(internet_page.getValue("txtCGrossAmount", false).replace(",", "")).toFixed(2)) >= Number(parseFloat(internet_page._sysControlAmount).toFixed(2))) //remlim.replace(",", "")
            {
                var iconfirm = confirm("Would you like to print daily keeping log?");
                if (iconfirm == true)
                {
                    document.js_closer = "remittancelimitClose"; show_frame_dialog('../Reports/Pages/Report_viewer.aspx?trantype=OTCDAILY&tranvalue='+ internet_page._txnseqid, null , '100%', '100%', '0',null, null, true);
                }
                else
                {
                    if (IRSWorld.TranClass.rem_other_details.chkCIFEmail(internet_page._agentSelectedRemitterID).value == true)
                    {
                        if (confirm("Do you want to send transaction receipt thru email?"))
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
                        internet_page.showPrintReceipt();
                    }
                }
            }
            else
            {
                if (IRSWorld.TranClass.rem_other_details.chkCIFEmail(internet_page._agentSelectedRemitterID).value == true)
                {
                    if (confirm("Do you want to send transaction receipt thru email?"))
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
                    internet_page.showPrintReceipt();
                }
            }
        }
        else
        {
            if (IRSWorld.TranClass.rem_other_details.chkCIFEmail(internet_page._agentSelectedRemitterID).value == true)
            {
                if (confirm("Do you want to send transaction receipt thru email?"))
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
                internet_page.showPrintReceipt();
            }
        }
    }
    
    this.remittance_limit_response = function()
    {
        if (IRSWorld.TranClass.rem_other_details.chkCIFEmail(internet_page._agentSelectedRemitterID).value == true)
        {
            if (confirm("Do you want to send transaction receipt thru email?"))
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
            internet_page.showPrintReceipt();
        }
    }
    
    this.getOREmail = function(referenceno, txnseqid)
    {
        internet_page.ajaxTimeout();
        return IRSWorld.Internet.InternetPageV2.sendOREmail(referenceno.toString(), txnseqid.toString(), this.getOREmail_callback);
    }
    
    this.getOREmail_callback = function(result)
    {
        var response = result.value;
        internet_page.showTransactionLoading("", true);
        if (response != null && response != "" && response != undefined)
        {
            internet_page.showAlert(response);
            internet_page.showPrintReceipt();
        }
    }
    
    this.showPrintReceipt = function()
    {
        if (confirm("Do you want to view transaction receipt?"))
        {
//            if (internet_page._branchCode == "E000")
//            {
//                var isloaded = internet_page.getEhudReceipt(internet_page._refno);
//                if (isloaded.toString() == "true")
//                {
//                    window.open('../Reports/Pages/Report_viewer.aspx?ehud');
//                }
//            }
            show_frame_dialog('../Reports/Pages/Report_viewer.aspx?trantype=OTC&tranvalue='+ internet_page._txnseqid, null , '100%', '100%', '0',null, null, true);
        }
        internet_page.initializePage();
    }
    
    this.checkSysCrtl_RemitterInfo = function()
    {
        internet_page.showTransactionLoading("Checking Required Remitter Info.....", false);
        var beneficiary_value = internet_page._agentSelectedBeneficiary.split("|");
        IRSWorld.Internet.InternetPageV2.checkSystemControl_RemitterInfo(internet_page._agentSelectedRemitterID, beneficiary_value[0], internet_page.checkSysCrtl_RemitterInfo_callback);
    }

    this.checkSysCrtl_RemitterInfo_callback = function(result)
    {
        var response = result.value;
        if(response != null && response != "null" && response != undefined && response != "undefined")
        {
            agentRem._RemitterInfoComplete = response;
            internet_page.checkSysCrtl_RemitterIDs();
        }
        else
        {
            internet_page.checkSysCrtl_RemitterIDs();
        }
    }
    
    this.checkSysCrtl_RemitterIDs = function()
    {
        internet_page.showTransactionLoading("Checking Required Remitter IDs.....", false);
        IRSWorld.Internet.InternetPageV2.checkSystemControl_RemitterIDs(internet_page._agentSelectedRemitterID,internet_page.checkSysCrtl_RemitterIDs_callback);
    }

    this.checkSysCrtl_RemitterIDs_callback = function(result)
    {
        var response = result.value;
        var remids = null;
        var idmessage = "";
        var noIDs = 0;
        var expireIDs = 0;
        if(response != null && response != "null" && response != undefined && response != "undefined")
        {
            remids = response.split("~");
            for (var i = 0; i < (remids.length - 1); i++)
            {
                if (remids[i].split("_")[0] == "0")
                {
                    if(noIDs == 0)
                    {
                        idmessage += "Please add the following IDs: \r\n";
                    }
                    idmessage += remids[i].split("_")[1] + " \r\n";
                    noIDs += 1;
                }
                else if (remids[i].split("_")[0] == "2")
                {
                    if(expireIDs == 0)
                    {
                        idmessage += "Please update the expired IDs: \r\n";
                    }
                    idmessage += remids[i].split("_")[1] + " \r\n";
                    expireIDs += 1;
                }
            }
            if (noIDs == 0 && expireIDs == 0)
            {
                agentRem._RemitterIDsComplete = true;
            }
            else
            {
                agentRem._RemitterIDsComplete = false;
            }
        }
        internet_page.showTransactionLoading("", true);
        if (agentRem._RemitterIDsComplete == true && agentRem._RemitterInfoComplete == true)
        {
            internet_page.hidePanel("divRemitterInfo");
            internet_page._RemitterIDs = false;
            internet_page.submitRemittance();
        }
        else
        {
            //internet_page.showAlert(agentRem._RemitterInfoMessage + "\r\n \r\n" + idmessage);
            agentRem._RemitterIDsMessage = idmessage;
            var beneficiary_value = internet_page._agentSelectedBeneficiary.split("|");
            agentRem.getRemitterInfo(internet_page._agentSelectedRemitterID,beneficiary_value[0]);
            internet_page._RemitterIDs = true;
            internet_page.process_grid_viewer("ajx_loading");
            internet_page.showPanel("divRemitterInfo");
        }
    }
    
    this.do_ajaxgridview = function(parameter)
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
                        if (internet_page._RemitterIDs == true)
                        {
                            agentRem.getSystemControl_RemitterID(internet_page._agentSelectedRemitterID,command_param[1])
                        }
                        else if (internet_page._RemIDs == true)
                        {
                            agentRem.get_RemitterID(internet_page._agentSelectedRemitterID,command_param[1])
                        }
                        else if (internet_page._isHit == true)
                        {
                        }
                        else
                        {
//                                internet_page.isSearch = true;
                            var arrSplit = command_param[1].split("|");
                            internet_page._agentSelectedRemitterID = arrSplit[0];
                            internet_page._agentSelectedBeneficiary = arrSplit[1] + "|" + arrSplit[2] + "|" + arrSplit[3] + "|" + arrSplit[4] + "|" + arrSplit[5];
                            internet_page.DisplayTransaction(arrSplit[0], arrSplit[1]);
//                                if (internet_page.getValue("lblCIFClass", false) == "1") //Remitter
//                                {
//                                    this.getAgentRemitter(arrSplit[0], arrSplit[0]); // [0] = Remitter
//                                }
//                                else
//                                {
//                                    this.getAgentRemitter(arrSplit[1], arrSplit[0]); // [1] = Beneficiary
//                                }
                        }
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
            if (internet_page._RemitterIDs == true)
            {
                internet_page._sql      = "isp_agentsys_gridremitter_ids~" + internet_page._agentSelectedRemitterID;
                internet_page._griddiv  = "divGridIds";
                internet_page._gridrow  = 5;
            }
            else if (internet_page._RemIDs == true)
            {
                internet_page._sql      = "sp_cif_grid_ids~" + internet_page._agentSelectedRemitterID;
                internet_page._griddiv  = "dvRemIDsGrid";
                internet_page._gridrow  = 5;
            }
            else if (internet_page._isHit == true)
            {
                var beneficiary_value = internet_page._agentSelectedBeneficiary.split("|");
                internet_page._sql      = "sp_otc_hit_listing~" + internet_page._agentSelectedRemitterID + "~" + beneficiary_value[0];
                internet_page._griddiv  = "div_client_hit_list_all";
                internet_page._gridrow  = 5;
            }
            else
            {
                // Modified By Hajz : 12/29/2012
                // Description :  For RFS CIF Searching Criteria, added is isExact tag
                internet_page._sql      = "isp_getAgentCIF~" + internet_page._userLocation + "~" + "1"  + "~" + internet_page.getValue("txtSLastName", false) + "~" + internet_page.getValue("txtSFirstName", false) + "~" + internet_page.getValue("txtSMiddleName", false);
                //internet_page._sql      = "isp_getAgentCIF~" + internet_page._userLocation + "~" + internet_page.getValue("lblCIFClass", false)  + "~" + internet_page.getValue("txtSLastName", false) + "~" + internet_page.getValue("txtSFirstName", false) + "~" + internet_page.getValue("txtSMiddleName", false) + "~" + (document.getElementById("chkExactMatch").checked ? "true" : "false");
                internet_page._griddiv  = "divSearchResult";
                internet_page._gridrow  = 5;
            }

        this.grid = new grid_viewer();
        this.grid.sql_source        = internet_page._sql;
        this.grid.div_container     = document.getElementById(internet_page._griddiv);
        this.grid.datarow_column    = 0;
        this.grid.page_size         = internet_page._gridrow;
        this.grid.grd_header_back_color = "#26afe5";
        this.grid.grd_header_fore_color = "#FFFFFF";
        this.grid.grd_customed_date = false;
        this.grid.process_ajax(parameter);
        this.grid                   = null;
    }
    
    this.editRemitter = function()
    {
        window.location = '../Internet/InternetRemitterARS.aspx';
    }
}

// **************************************************************************************** //
// *** External Functions
// **************************************************************************************** //
function execute_ajaxgridview_command(param)
{
    if (internet_page != null && internet_page != undefined)
    {
        internet_page.do_ajaxgridview(param);
    }
}

function rem_aggregate_closer_response()
{   
    if (internet_page != null)
    {
        internet_page.validateTransaction();
    }
}

function remittancelimitClose()
{
    if (internet_page != null)
    {
        document.js_closer = null;
        internet_page.remittance_limit_response();        
    }
}

function push_check_payment_result()
{
    if (internet_page != null)
    {
        internet_page.check_payment_result();
    }
}

function push_check_payments_commit_response(param)
{
    if (internet_page != null)
    {
        internet_page.check_payment_commit_result(param);
    }
}

function push_ach_payments_commit_response(param)
{
    if (internet_page != null)
    {
        internet_page.ach_payment_commit_result(param);
    }
}

function push_ach_payment_result()
{
    if (internet_page != null)
    {
        internet_page.chk_ach_pay_result();
    }
}

function click_calendar(objid,objt)
{
    popUpCalendar(objt, document.getElementById(objid), 'mm-dd-yyyy');
}
// **************************************************************************************** //

//Agent Special Function
function agent_remittance()
{
    var agr = this;
    var _RemitterID             = "";
    var _RemitterName           = "";
    var _RemitterBirthDate      = null;
    var _RemitterEmployer       = "";
    var _RemitterOccupation     = 0;
    var _RemitterOccupationDesc = "";
    var _RemitterSalary         = 0;
    var _RemitterSalaryCcy      = 0;
    var _SourceOfFunds          = 0;
    var _WorkerType             = 0;
    var _BeneficaryID           = "";
    var _BeneficiaryName        = "";
    var _Relationship           = 0;
    var _IDSEQID                = 0;
    var _IDNo                   = "";
    var _IDType                 = 0;
    var _IDClass                = 0;
    var _IDExpirydate           = null;
    var _IDPlaceOfIssuance      = "";
    var _RemitterInfoComplete   = false;
    var _RemitterIDsComplete    = false;
    var _RemitterInfoMessage    = "";
    var _RemitterIDsMessage     = "";
    var _aeIDMode               = false;//true-Add Mode, false-Edit Mode
    var _fillDLL                = 0;
    var _fillIDsDLL             = 0;
    
    this.loadSystemControl = function()
    {
        internet_page.clearDDL("ddlRemInfoOccupation");
        internet_page.clearDDL("ddlRemInfoSourceOfFunds");
        internet_page.clearDDL("ddlRemInfoRelationship");
        internet_page.clearDDL("ddlRemInfoIDType");
        internet_page.clearDDL("ddlRemInfoIDClass");
        agr._fillDLL = 0;
        agr.populateDDL("Occupation");
        agr.populateDDL("SourceOfFunds");
        agr.populateDDL("Relationship");
        agr.populateDDL("IDType");
        agr.populateDDL("IDClassification");        
    }
    
    this.loadIDsForm = function()
    {
        internet_page.clearDDL("ddlRemIDsType");
        internet_page.clearDDL("ddlRemIDsClassification");
        agr._fillIDsDLL = 0;
        if (internet_page._branchCountry != "8")
        {
            internet_page.showControl("trRemIDs", true);
            agr.populateDDL2("IDClassification");
        }
        agr.populateDDL2("IDType");
        if(document.getElementById("btnRemIDsCancel").value == "Cancel")
        {
            setText(document.getElementById("btnRemIDsCancel"), "Close");
        }
        else
        {
            setText(document.getElementById("btnRemIDsCancel"), "Cancel");
        }
    }
    
    this.getRemitterInfo = function(remitterID, beneficiaryID)
    {
        internet_page.hidePanel("divRemitterInfo");
        internet_page.showTransactionLoading("Acquiring Remitter Info.....", false);        
        IRSWorld.Internet.InternetPageV2.getSystemControl_RemitterInfo(remitterID, beneficiaryID, agr.getRemitterInfo_callback)
    }
    
    this.getRemitterInfo_callback = function(result)
    {
        var response = result.value;
        if(response != null && response != "null" && response != undefined && response != "undefined")
        {
            agr._RemitterID = internet_page.getTableValueRow(response, "RemitterCIFID", 0);
            agr._RemitterName = internet_page.getTableValueRow(response, "RemitterFullName", 0);
            agr._RemitterBirthDate = internet_page.getTableValueRow(response, "RemitterBirthDate", 0);
            agr._RemitterEmployer = internet_page.getTableValueRow(response, "Employer", 0);
            agr._RemitterOccupation = internet_page.getTableValueRow(response, "Occupation", 0);
            agr._RemitterOccupationDesc = internet_page.getTableValueRow(response, "DetailedDesc", 0);
            agr._RemitterSalary = internet_page.getTableValueRow(response, "Salary", 0);
            agr._RemitterSalaryCcy = internet_page.getTableValueRow(response, "Currency", 0);
            agr._SourceOfFunds = internet_page.getTableValueRow(response, "SourceofFunds", 0);
            agr._WorkerType = internet_page.getTableValueRow(response, "WorkerType", 0);
            agr._BeneficaryID = internet_page.getTableValueRow(response, "BeneficiaryID", 0);
            agr._BeneficiaryName = internet_page.getTableValueRow(response, "BeneficiaryFullName", 0);
            agr._Relationship = internet_page.getTableValueRow(response, "Relationship", 0);
            
            agr.setRemitterInfo();
            agr.checkRemitterInfoReq();
        }
    }
            
    this.setRemitterInfo = function()
    {
        internet_page.enterValue(agr._RemitterID, "spnRemInfoCIFID", true);
        internet_page.enterValue(agr._RemitterName, "spnRemInfoRemitterName", true);
        internet_page.enterValue(agr._RemitterBirthDate, "txtRemInfoBirthdate", false);
        internet_page.findDDL("ddlRemInfoOccupation", agr._RemitterOccupation, "lblRemInfoOccupation", true);
        internet_page.enterValue(agr._RemitterOccupationDesc, "txtRemInfoDetailedOccupation", false);
        internet_page.findDDL("ddlRemInfoSourceOfFunds", agr._SourceOfFunds, "lblRemInfoSourceOfFunds", true);
        internet_page.enterValue(agr._BeneficiaryName, "spnRemInfoRemBeneficiaryName", true);
        internet_page.findDDL("ddlRemInfoRelationship", agr._Relationship, "lblRemInfoRelationship", true);
    }
    
    this.populateDDL = function(fldgroup)
    {
        internet_page.showTransactionLoading("Populating Controls.....", false); 
        return IRSWorld.Internet.InternetPageV2.getSubLookup(fldgroup, agr.populateDDL_callback);
    }
    
    this.populateDDL_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            switch (response.Rows[0]["FldGroup"].toString().toUpperCase())
            {
                case "OCCUPATION":
                    internet_page.fillDDL(response, "ddlRemInfoOccupation", "FldDescription", "FldValue", "lblRemInfoOccupation", ", ");
                    agr._fillDLL = agr._fillDLL + 1;
                    break;
                case "SOURCEOFFUNDS":
                    internet_page.fillDDL(response, "ddlRemInfoSourceOfFunds", "FldDescription", "FldValue", "lblRemInfoSourceOfFunds", ", ");
                    agr._fillDLL = agr._fillDLL + 1;
                    break;
                case "WORKERTYPE":
                    internet_page.fillDDL(response, "ddlRemInfoWorkerType", "FldDescription", "FldValue", "lblRemInfoWorkerType", ", ");
                    agr._fillDLL = agr._fillDLL + 1;
                    break;
                case "CURRENCY":
                    internet_page.fillDDL(response, "ddlRemInfoSalaryCcy", "FldDescription", "FldValue", "lblRemInfoSalaryCcy", ", ");
                    agr._fillDLL = agr._fillDLL + 1;
                    break;
                case "RELATIONSHIP":
                    internet_page.fillDDL(response, "ddlRemInfoRelationship", "FldDescription", "FldValue", "lblRemInfoRelationship", ", ");
                    agr._fillDLL = agr._fillDLL + 1;
                    break;
                case "IDTYPE":
                    internet_page.fillDDL(response, "ddlRemInfoIDType", "FldDescription", "FldValue", "lblRemInfoIDType", ", ");
                    agr._fillDLL = agr._fillDLL + 1;
                    break;
                case "IDCLASSIFICATION":
                    internet_page.fillDDL(response, "ddlRemInfoIDClass", "FldDescription", "FldValue", "lblRemInfoIDClass", ", ");
                    agr._fillDLL = agr._fillDLL + 1;
                    break;
            }
        }
        
        if (agr._fillDLL >= 5)
        {
            internet_page.showTransactionLoading("", true);
            internet_page.checkSysCrtl_RemitterInfo();
        }
    }
    
    this.populateDDL2 = function(fldgroup)
    {
        internet_page.showTransactionLoading("Populating Controls.....", false); 
        return IRSWorld.Internet.InternetPageV2.getSubLookup(fldgroup, agr.populateDDL2_callback);
    }
    
    this.populateDDL2_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            switch (response.Rows[0]["FldGroup"].toString().toUpperCase())
            {
                case "OCCUPATION":
                    internet_page.fillDDL(response, "ddlRemInfoOccupation", "FldDescription", "FldValue", "lblRemInfoOccupation", ", ");
                    agr._fillIDsDLL = agr._fillIDsDLL + 1;
                    break;
                case "SOURCEOFFUNDS":
                    internet_page.fillDDL(response, "ddlRemInfoSourceOfFunds", "FldDescription", "FldValue", "lblRemInfoSourceOfFunds", ", ");
                    agr._fillIDsDLL = agr._fillIDsDLL + 1;
                    break;
                case "WORKERTYPE":
                    internet_page.fillDDL(response, "ddlRemInfoWorkerType", "FldDescription", "FldValue", "lblRemInfoWorkerType", ", ");
                    agr._fillIDsDLL = agr._fillIDsDLL + 1;
                    break;
                case "CURRENCY":
                    internet_page.fillDDL(response, "ddlRemInfoSalaryCcy", "FldDescription", "FldValue", "lblRemInfoSalaryCcy", ", ");
                    agr._fillIDsDLL = agr._fillIDsDLL + 1;
                    break;
                case "RELATIONSHIP":
                    internet_page.fillDDL(response, "ddlRemInfoRelationship", "FldDescription", "FldValue", "lblRemInfoRelationship", ", ");
                    agr._fillIDsDLL = agr._fillIDsDLL + 1;
                    break;
                case "IDTYPE":
                    internet_page.fillDDL(response, "ddlRemIDsType", "FldDescription", "FldValue", "lblRemInfoIDType", ", ");
                    agr._fillIDsDLL = agr._fillIDsDLL + 1;
                    break;
                case "IDCLASSIFICATION":
                    internet_page.fillDDL(response, "ddlRemIDsClassification", "FldDescription", "FldValue", "lblRemInfoIDClass", ", ");
                    agr._fillIDsDLL = agr._fillIDsDLL + 1;
                    break;
            }
        }
        
        if (agr._fillIDsDLL > 1 && internet_page._branchCountry != "8")
        {
            internet_page.showTransactionLoading("", true);
            internet_page._RemIDs = true;
            internet_page.process_grid_viewer("ajx_loading");
            internet_page.showPanel("divRemitterIds");
        }
        else if (agr._fillIDsDLL > 0 && internet_page._branchCountry == "8")
        {
            internet_page.showTransactionLoading("", true);
            internet_page._RemIDs = true;
            internet_page.process_grid_viewer("ajx_loading");
            internet_page.showPanel("divRemitterIds");
        }
    }
    
    this.checkRemitterInfoReq = function()
    {
        var isRequiredempty = 0;
        
        agr._RemitterInfoMessage = "Please fill up the required fields: \r\n";
        if(internet_page.getValue("txtRemInfoBirthdate", false) == "" || internet_page.getValue("txtRemInfoBirthdate", false) == "01/01/1900")
        {
            isRequiredempty = isRequiredempty + 1;
            document.getElementById("mnBirthDate").style.display = "";
            agr._RemitterInfoMessage += "BirthDate \t\n";
            internet_page.edButtons("txtRemInfoBirthdate",false);
            internet_page.edButtons("imgBirthDate",false);
        }
        else
        {
            internet_page.edButtons("txtRemInfoBirthdate",true);
            internet_page.edButtons("imgBirthDate",true);
        }
        
        if(document.getElementById("ddlRemInfoOccupation").selectedIndex <= 0)
        {
            isRequiredempty = isRequiredempty + 1;
            document.getElementById("mnOccupation").style.display = "";
            agr._RemitterInfoMessage += "Occupation \t\n";
            internet_page.edButtons("ddlRemInfoOccupation",false);
        }
        else
        {
            internet_page.edButtons("ddlRemInfoOccupation",true);
        }
        
        if(document.getElementById("ddlRemInfoSourceOfFunds").selectedIndex <= 0)
        {
            isRequiredempty = isRequiredempty + 1;
            document.getElementById("mnSourceOfFunds").style.display = "";
            agr._RemitterInfoMessage += "Source Of Funds \t\n";
            internet_page.edButtons("ddlRemInfoSourceOfFunds",false);
        }
        else
        {
            internet_page.edButtons("ddlRemInfoSourceOfFunds",true);
        }
        
        if(document.getElementById("ddlRemInfoRelationship").selectedIndex <= 0)
        {
            isRequiredempty = isRequiredempty + 1;
            document.getElementById("mnRelationship").style.display = "";
            agr._RemitterInfoMessage += "Relationship \t\n";
            internet_page.edButtons("ddlRemInfoRelationship",false);
        }
        else
        {
            internet_page.edButtons("ddlRemInfoRelationship",true);
        }
        internet_page.showTransactionLoading("", true);
        internet_page.showPanel("divRemitterInfo");
        if(isRequiredempty >= 1)
        {
            internet_page.showAlert(agr._RemitterInfoMessage + "\r\n \r\n" + agr._RemitterIDsMessage);
            internet_page.edButtons("btnRemitterInfoSave",false);
            internet_page.edButtons("btnRemitterInfoCancel",false);
            internet_page.enterValue("Cancel", "btnRemitterInfoCancel", false);
        }
        else
        {
            internet_page.edButtons("btnRemitterInfoSave",true);
            internet_page.edButtons("btnRemitterInfoCancel",false);
            internet_page.enterValue("Edit", "btnRemitterInfoCancel", false);
            if (agr._RemitterIDsComplete != true)
            {
                internet_page.showAlert(agr._RemitterIDsMessage);
            }
            agr._RemitterInfoMessage = "";
            agr._RemitterIDsMessage = "";
        }
        
    }
    
    this.editSysControl_RemitterInfo = function()
    {
        if (internet_page.getValue("btnRemitterInfoCancel", false) == "Edit")
        {
            internet_page.edButtons("btnRemitterInfoSave",false);
            internet_page.edButtons("btnRemitterInfoCancel",false);
            internet_page.enterValue("Cancel", "btnRemitterInfoCancel", false);
            
            internet_page.edButtons("txtRemInfoBirthdate",false);
            internet_page.edButtons("imgBirthDate",false);
            internet_page.edButtons("ddlRemInfoOccupation",false);
            internet_page.edButtons("ddlRemInfoSourceOfFunds",false);
            internet_page.edButtons("ddlRemInfoRelationship",false);
            internet_page.edButtons("txtRemInfoDetailedOccupation",false);           
        }
        else if (internet_page.getValue("btnRemitterInfoCancel", false) == "Cancel")
        {
            internet_page.edButtons("btnRemitterInfoSave",true);
            internet_page.edButtons("btnRemitterInfoCancel",false);
            internet_page.enterValue("Edit", "btnRemitterInfoCancel", false);
            
            internet_page.edButtons("txtRemInfoBirthdate",true);
            internet_page.edButtons("imgBirthDate",true);
            internet_page.edButtons("ddlRemInfoOccupation",true);
            internet_page.edButtons("ddlRemInfoSourceOfFunds",true);
            internet_page.edButtons("ddlRemInfoRelationship",true);
            internet_page.edButtons("txtRemInfoDetailedOccupation",true);
            agr.setRemitterInfo()
        }
    }
    
    this.saveSysControl_RemitterInfo = function()
    {
        if (internet_page.getValue("btnRemitterInfoCancel", false) == "Edit")
        {
            internet_page.edButtons("btnRemitterInfoSave",false);
            internet_page.edButtons("btnRemitterInfoCancel",false);
            internet_page.enterValue("Cancel", "btnRemitterInfoCancel", false);
            
        }
        else if (internet_page.getValue("btnRemitterInfoCancel", false) == "Cancel")
        {
            agr.checkRemitterInfoReq();
            if(agr._RemitterInfoMessage == "")
            {
                internet_page.edButtons("btnRemitterInfoSave",true);
                internet_page.edButtons("btnRemitterInfoCancel",false);
                internet_page.enterValue("Edit", "btnRemitterInfoCancel", false);
            
                internet_page.edButtons("txtRemInfoBirthdate",true);
                internet_page.edButtons("imgBirthDate",true);
                internet_page.edButtons("ddlRemInfoOccupation",true);
                internet_page.edButtons("ddlRemInfoSourceOfFunds",true);
                internet_page.edButtons("ddlRemInfoRelationship",true);
                internet_page.edButtons("txtRemInfoDetailedOccupation",true);
                internet_page.hidePanel("divRemitterInfo");
                internet_page.showTransactionLoading("Saving Required Remitter Info.....", false);
                IRSWorld.Internet.InternetPageV2.saveSystemControl_RemitterInfo(agentRem.setvalues_RemitterInfo(), agentRem.saveSystemControl_RemitterInfo_callback);
            }
            else
            {
                internet_page.showAlert(agr._RemitterInfoMessage);
            }
        }
    }    
        
    this.saveSystemControl_RemitterInfo_callback = function(result)
    {
        var response = result.value;
        internet_page.showTransactionLoading("", true);
        internet_page.showPanel("divRemitterInfo");
        if(response != null && response != "null" && response != undefined && response != "undefined")
        {
            if(response != -1)
            {
                internet_page.showAlert("Remitter Info successfully saved.");
                agr.setRemitterInfo();
                internet_page.checkSysCrtl_RemitterInfo();
            }
            else
            {
                internet_page.showAlert("Failed to save Remitter Info");
            }
        }
    }
    
    this.setvalues_RemitterInfo = function()
    {
        var sReturn = "";
        agr._RemitterID             = internet_page.getValue("spnRemInfoCIFID", true);
        agr._RemitterName           = internet_page.getValue("spnRemInfoRemitterName", true);
        agr._RemitterBirthDate      = internet_page.getValue("txtRemInfoBirthdate", false);
        agr._RemitterOccupation     = document.getElementById("ddlRemInfoOccupation").value;
        agr._RemitterOccupationDesc = internet_page.getValue("txtRemInfoDetailedOccupation", false);
        agr._SourceOfFunds          = document.getElementById("ddlRemInfoSourceOfFunds").value;
        agr._BeneficiaryName        = internet_page.getValue("spnRemInfoRemBeneficiaryName", true);
        agr._Relationship           = document.getElementById("ddlRemInfoRelationship").value;
        
        sReturn = agr._RemitterID + "~";
        sReturn += agr._RemitterName + "~";
        sReturn += agr._RemitterBirthDate + "~";
        sReturn += agr._RemitterEmployer + "~";
        sReturn += agr._RemitterOccupation + "~";
        sReturn += agr._RemitterOccupationDesc + "~";
        sReturn += agr._SourceOfFunds + "~";
        sReturn += ((agr._WorkerType == "") ? "0" : agr._WorkerType) + "~";
        sReturn += ((agr._RemitterSalary == "" || agr._RemitterSalary == "null" || agr._RemitterSalary == null) ? "0" : agr._RemitterSalary) + "~";
        sReturn += ((agr._RemitterSalaryCcy == "" || agr._RemitterSalaryCcy == "null" || agr._RemitterSalaryCcy == null) ? "0" : agr._RemitterSalaryCcy) + "~";
        sReturn += agr._BeneficaryID + "~";
        sReturn += agr._BeneficiaryName + "~";
        sReturn += agr._Relationship;
        
        return sReturn;
    }
    
    this.getSystemControl_RemitterID = function(remitterid, seqid)
    {
        internet_page.hidePanel("divRemitterInfo");
        internet_page.showTransactionLoading("Acquiring Required Remitter IDs.....", false);
        IRSWorld.Internet.InternetPageV2.getSystemControl_RemitterIDs(remitterid, seqid.toString(), agr.getSystemControl_RemitterID_callback)
    }
    
    this.getSystemControl_RemitterID_callback = function(result)
    {
        var response = result.value;
        if(response != null && response != "null" && response != undefined && response != "undefined")
        {
            agr._IDSEQID        = internet_page.getTableValueRow(response, "SEQ_ID", 0);
            agr._IDNo           = internet_page.getTableValueRow(response, "IDNo", 0);
            agr._IDType         = internet_page.getTableValueRow(response, "IdType", 0);
            agr._IDClass        = internet_page.getTableValueRow(response, "IDClassification", 0);
            agr._IDExpirydate   = internet_page.getTableValueRow(response, "ExpiryDate", 0);
            
            agr.setRemitterIDs();
        }
    }
    
    this.setRemitterIDs = function()
    {
        internet_page.showTransactionLoading("", true);
        internet_page.showPanel("divRemitterInfo");
        internet_page.enterValue(agr._IDNo, "txtRemInfoIDNo", false)
        internet_page.findDDL("ddlRemInfoIDType", agr._IDType, "lblRemInfoIDType", true);
        internet_page.findDDL("ddlRemInfoIDClass", agr._IDClass, "lblRemInfoIDClass", true);
        internet_page.enterValue(agr._IDExpirydate, "txtRemInfoExpiryDate", false);
        
        internet_page.edButtons("btnRemInfoEditID",false);
    }
    
    this.clearRemitterIDs = function()
    {
        agr._IDSEQID = 0;
        agr._IDNo = "";
        agr._IDType = 0;
        agr._IDClass = 0;
        agr._IDExpirydate = null;
        
        internet_page.removeValue("txtRemInfoIDNo", false);
        internet_page.removeValue("txtRemInfoExpiryDate", false);
        document.getElementById("ddlRemInfoIDType").selectedIndex = -1;
        document.getElementById("ddlRemInfoIDClass").selectedIndex = -1;
    }
    
    this.addSystemControl_RemitterIDs = function()
    {
        internet_page.edButtons("btnRemInfoAddID",true);
        internet_page.edButtons("btnRemInfoEditID",true);
        internet_page.edButtons("btnRemInfoSaveID",false);
        internet_page.edButtons("btnRemInfoCancelID",false);
        
        internet_page.edButtons("ddlRemInfoIDType",false);
        internet_page.edButtons("txtRemInfoIDNo",false);
        internet_page.edButtons("imgIDExpiryDate",false);
        internet_page.edButtons("ddlRemInfoIDClass",false);
        
        agr._aeIDMode = true;
    }
    
    this.editSystemControl_RemitterIDs = function()
    {
        internet_page.edButtons("btnRemInfoAddID",true);
        internet_page.edButtons("btnRemInfoEditID",true);
        internet_page.edButtons("btnRemInfoSaveID",false);
        internet_page.edButtons("btnRemInfoCancelID",false);
        
        internet_page.edButtons("ddlRemInfoIDType",false);
        internet_page.edButtons("txtRemInfoIDNo",false);
        internet_page.edButtons("imgIDExpiryDate",false);
        internet_page.edButtons("ddlRemInfoIDClass",false);
        
        agr._aeIDMode = false;
    }
    
    this.saveSystemControl_RemitterIDs = function()
    {
        internet_page.edButtons("btnRemInfoAddID",false);
        internet_page.edButtons("btnRemInfoEditID",true);
        internet_page.edButtons("btnRemInfoSaveID",true);
        internet_page.edButtons("btnRemInfoCancelID",true);
        
        internet_page.edButtons("ddlRemInfoIDType",true);
        internet_page.edButtons("txtRemInfoIDNo",true);
        internet_page.edButtons("imgIDExpiryDate",true);
        internet_page.edButtons("ddlRemInfoIDClass",true);
        
        internet_page.hidePanel("divRemitterInfo");
        internet_page.showTransactionLoading("Saving Required Remitter IDs.....", false);
        if (agr._aeIDMode == true)
        {
            agr._IDSEQID = 0;
            IRSWorld.Internet.InternetPageV2.saveSystemControl_RemitterIDs(agentRem.setvalues_RemitterIDs(),agentRem.saveSystemControl_RemitterIDs_callback);
        }
        else
        {
            IRSWorld.Internet.InternetPageV2.saveSystemControl_RemitterIDs(agentRem.setvalues_RemitterIDs(),agentRem.saveSystemControl_RemitterIDs_callback);
        }
        agr.clearRemitterIDs();
    }
    
    this.cancelSystemControl_RemitterIDs = function()
    {
        internet_page.edButtons("btnRemInfoAddID",false);
        internet_page.edButtons("btnRemInfoEditID",true);
        internet_page.edButtons("btnRemInfoSaveID",true);
        internet_page.edButtons("btnRemInfoCancelID",true);
        
        internet_page.edButtons("ddlRemInfoIDType",true);
        internet_page.edButtons("txtRemInfoIDNo",true);
        internet_page.edButtons("imgIDExpiryDate",true);
        internet_page.edButtons("ddlRemInfoIDClass",true);
        
        agr.clearRemitterIDs();
    }
    
    this.setvalues_RemitterIDs = function()
    {
        var sReturn = "";
        agr._RemitterID     = internet_page.getValue("spnRemInfoCIFID", true);
        agr._IDNo           = internet_page.getValue("txtRemInfoIDNo", false);
        agr._IDType         = document.getElementById("ddlRemInfoIDType").value;
        agr._IDClass        = document.getElementById("ddlRemInfoIDClass").value;
        agr._IDExpirydate   = internet_page.getValue("txtRemInfoExpiryDate", false);
        
        sReturn = agr._RemitterID + "~";
        sReturn += agr._IDNo + "~";
        sReturn += agr._IDType + "~";
        sReturn += agr._IDClass + "~";
        sReturn += ((agr._IDExpirydate == "") ? "01/01/1900" : agr._IDExpirydate) + "~";
        sReturn += agr._IDSEQID;
        
        return sReturn;
    }
    
    this.saveSystemControl_RemitterIDs_callback = function(result)
    {
        var response = result.value;
        internet_page.showTransactionLoading("", true);
        internet_page.showPanel("divRemitterInfo");
        if(response != null && response != "null" && response != undefined && response != "undefined")
        {
            if(response != -1)
            {
                internet_page.showAlert("Remitter ID successfully saved.");
                //agr.setRemitterInfo();
                internet_page.checkAgentSystemControl();
            }
            else
            {
                internet_page.showAlert("Failed to save Remitter ID");
            }
        }
    }
    
    this.add_RemitterIDs = function()
    {
        internet_page.edButtons("btnRemIDsAdd",true);
        internet_page.edButtons("btnRemIDsEdit",true);
        internet_page.edButtons("btnRemIDsSave",false);
        internet_page.edButtons("btnRemIDsCancel",false);
        
        if(document.getElementById("btnRemIDsCancel").value == "Close")
        {
            setText(document.getElementById("btnRemIDsCancel"), "Cancel");
        }
        else
        {
            setText(document.getElementById("btnRemIDsCancel"), "Close");
        }
        
        internet_page.edButtons("ddlRemIDsType",false);
        internet_page.edButtons("txtRemIDsNo",false);
        internet_page.edButtons("imgRemIDsExpDate",false);
        internet_page.edButtons("txtRemIDsPOI",false);
        if (internet_page._branchCountry != "8")
        {
            internet_page.edButtons("ddlRemIDsClassification",false);
        }
        
        agr._aeIDMode = true;
        agr.clearRemIDs();
    }
    
    this.edit_RemitterIDs = function()
    {
        internet_page.edButtons("btnRemIDsAdd",true);
        internet_page.edButtons("btnRemIDsEdit",true);
        internet_page.edButtons("btnRemIDsSave",false);
        internet_page.edButtons("btnRemIDsCancel",false);
        
        if(document.getElementById("btnRemIDsCancel").value == "Close")
        {
            setText(document.getElementById("btnRemIDsCancel"), "Cancel");
        }
        else
        {
            setText(document.getElementById("btnRemIDsCancel"), "Close");
        }
        
        internet_page.edButtons("ddlRemIDsType",false);
        internet_page.edButtons("txtRemIDsNo",false);
        internet_page.edButtons("imgRemIDsExpDate",false);
        internet_page.edButtons("txtRemIDsPOI",false);
        if (internet_page._branchCountry != "8")
        {
            internet_page.edButtons("ddlRemIDsClassification",false);
        }
        
        agr._aeIDMode = false;
    }
    
    this.save_RemitterIDs = function()
    {
        if (document.getElementById("ddlRemIDsType").value <= 0)
        {
            internet_page.showAlert("Please select ID Type.");
            return;
        }
        if(document.getElementById("txtRemIDsNo").value == "")
        {
            internet_page.showAlert("Please enter ID no.");
            return;
        }
        if(document.getElementById("txtRemIDsExpDate").value != "")
        {
            if(IRSWorld.Internet.InternetPageV2.checkExpired(document.getElementById("txtRemIDsExpDate").value).value == true)
            {
                internet_page.showAlert("ID is already expired.");
                return;
            }
        }
        if (internet_page._branchCountry == "8")
        {
            if(document.getElementById("txtRemIDsPOI").value == "")
            {
                internet_page.showAlert("Please enter Place of Issuance.");
                return;
            }
            if(document.getElementById("txtRemIDsExpDate").value == "")
            {
                internet_page.showAlert("Please enter ID Expiry Date.");
                return;
            }
        }
        
        internet_page.edButtons("btnRemIDsAdd",false);
        internet_page.edButtons("btnRemIDsEdit",true);
        internet_page.edButtons("btnRemIDsSave",true);
        internet_page.edButtons("btnRemIDsCancel",true);
        
        internet_page.edButtons("ddlRemIDsType",true);
        internet_page.edButtons("txtRemIDsNo",true);
        internet_page.edButtons("imgRemIDsExpDate",true);
        internet_page.edButtons("txtRemIDsPOI",true);
        if (internet_page._branchCountry != "8")
        {
            internet_page.edButtons("ddlRemIDsClassification",true);
        }
        
        internet_page.hidePanel("divRemitterIds");
        internet_page.showTransactionLoading("Saving Required Remitter IDs.....", false);
        if (agr._aeIDMode == true)
        {
            agr._IDSEQID = 0;
            IRSWorld.Internet.InternetPageV2.save_RemitterIDs(agentRem.setvalues_RemIDs(),agentRem.save_RemitterIDs_callback);
        }
        else
        {
            IRSWorld.Internet.InternetPageV2.save_RemitterIDs(agentRem.setvalues_RemIDs(),agentRem.save_RemitterIDs_callback);
        }
        agr.clearRemIDs();
    }
    
    this.save_RemitterIDs_callback = function(result)
    {
        var response = result.value;
        internet_page.showTransactionLoading("", true);
        internet_page.showPanel("divRemitterIds");
        if(response != null && response != "null" && response != undefined && response != "undefined")
        {
            if(response != -1)
            {
                internet_page.showAlert("Remitter ID successfully saved.");
                //if(document.getElementById("ddlBeneficiary").selectedIndex > 0)
                //{
                    //internet_page.hidePanel("divRemitterIds");
                    //internetRemitterClose();
                //}
                //else
                //{
                    //internet_page.hidePanel("divRemitterIds");
                //}
                if (internet_page._isSubmit == true)
                {    
                    if (internet_page._aggCheckIDAmount > 0)
                    {
                        internet_page.checkAgentSystemControl();
                    }
                    else
                    {
                        internet_page.checkAgentSystemControl();
                    }
                }
                else
                {
                    internet_page.hidePanel("divRemitterIds");
                }
            }
            else
            {
                internet_page.showAlert("Failed to save Remitter ID");
            }
        }
    }
    
    this.cancel_RemitterIDs = function()
    {
        internet_page.edButtons("btnRemIDsAdd",false);
        internet_page.edButtons("btnRemIDsEdit",true);
        internet_page.edButtons("btnRemIDsSave",true);
        internet_page.edButtons("btnRemIDsCancel",false);
        
        if(document.getElementById("btnRemIDsCancel").value == "Cancel")
        {
            setText(document.getElementById("btnRemIDsCancel"), "Close");
        }
        else
        {
            setText(document.getElementById("btnRemIDsCancel"), "Cancel");
            internet_page.hidePanel("divRemitterIds");
        }
        
        internet_page.edButtons("ddlRemIDsType",true);
        internet_page.edButtons("txtRemIDsNo",true);
        internet_page.edButtons("imgRemIDsExpDate",true);
        internet_page.edButtons("txtRemIDsPOI",true);
        if (internet_page._branchCountry != "8")
        {
            internet_page.edButtons("ddlRemIDsClassification",true);
        }
        
        agr.clearRemIDs();
    }
    
    this.get_RemitterID = function(remitterid, seqid)
    {
        internet_page.hidePanel("divRemitterIds");
        internet_page.showTransactionLoading("Acquiring Remitter IDs.....", false);
        IRSWorld.Internet.InternetPageV2.getRemitterIDs(remitterid, seqid.toString(), agr.get_RemitterID_callback)
    }
    
    this.get_RemitterID_callback = function(result)
    {
        var response = result.value;
        if(response != null && response != "null" && response != undefined && response != "undefined")
        {
            agr._IDSEQID        = internet_page.getTableValueRow(response, "SEQ_ID", 0);
            agr._IDNo           = internet_page.getTableValueRow(response, "IDNo", 0);
            agr._IDType         = internet_page.getTableValueRow(response, "IdType", 0);
            agr._IDClass        = internet_page.getTableValueRow(response, "IDClassification", 0);
            agr._IDExpirydate   = internet_page.getTableValueRow(response, "ExpiryDate", 0);
            agr._IDPlaceOfIssuance = internet_page.getTableValueRow(response, "PlaceOfIssuance", 0);
            
            agr.setRemIDs();
        }
    }
    
    this.setvalues_RemIDs = function()
    {
        var sReturn = "";
        agr._RemitterID     = internet_page._agentSelectedRemitterID;
        agr._IDNo           = internet_page.getValue("txtRemIDsNo", false);
        agr._IDType         = document.getElementById("ddlRemIDsType").value;
        agr._IDPlaceOfIssuance = internet_page.getValue("txtRemIDsPOI", false);
        agr._IDExpirydate   = internet_page.getValue("txtRemIDsExpDate", false);
        if (internet_page._branchCountry != "8")
        {
            agr._IDClass = document.getElementById("ddlRemIDsClassification").value;
        }
        
        sReturn = agr._RemitterID + "~";
        sReturn += agr._IDNo + "~";
        sReturn += agr._IDType + "~";
        sReturn += agr._IDClass + "~";
        sReturn += agr._IDPlaceOfIssuance + "~";
        sReturn += agr._IDExpirydate + "~";
        sReturn += agr._IDSEQID;
        
        return sReturn;
    }
    
    this.setRemIDs = function()
    {
        internet_page.showTransactionLoading("", true);
        internet_page.showPanel("divRemitterIds");
        internet_page.enterValue(agr._IDNo, "txtRemIDsNo", false)
        internet_page.findDDL("ddlRemIDsType", agr._IDType, "lblRemIDsType", true);
        internet_page.enterValue(agr._IDPlaceOfIssuance, "txtRemIDsPOI", false);
        internet_page.enterValue(agr._IDExpirydate, "txtRemIDsExpDate", false);
        if (internet_page._branchCountry != "8")
        {
            internet_page.findDDL("ddlRemIDsClassification", agr._IDClass, "lblRemInfoIDClass", true);
        }
        
        internet_page.edButtons("btnRemIDsEdit",false);
    }
    
    this.clearRemIDs = function()
    {
        agr._IDSEQID = 0;
        agr._IDNo = "";
        agr._IDType = 0;
        agr._IDClass = 0;
        agr._IDExpirydate = null;
        
        internet_page.removeValue("txtRemIDsNo", false);
        internet_page.removeValue("txtRemIDsExpDate", false);
        internet_page.removeValue("txtRemIDsPOI", false);
        document.getElementById("ddlRemIDsType").selectedIndex = -1;
        if (internet_page._branchCountry != "8")
        {
            document.getElementById("ddlRemIDsClassification").selectedIndex = -1;
        }
    }
}