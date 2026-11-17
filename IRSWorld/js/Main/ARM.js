function arm_page()
{
    // **************************************************************************************** //
    // *** Variables
    // **************************************************************************************** //
    var tblDefaults = "";
    var tblCurrency = "";
    var tblProductCode = "";
    var tblImplementationCode = "";
    var tblComputationLevel = "";
    var tblRemitMethod = "";
    var tblCards = "";
    var tblChargesDefault = "";
    var tblRemittance = "";
    var tblPayMode = "";
    var tblReceiptMode = "";
    var tblOBPSLookup = "";
    var _lookupField = "";
    var _calc = "";
    var _arrCalc = "";
    var _sourcecurrency = "";
    var _transactionsource = "";
    var _remittancetype = "";
    var _isremittance = false;
    var _nextpage = "";
    var _qwerty = "";
    var _iscontribution = false;
    var _totalamount = "";
    var _merchantName = "";
    var _merchantProduct = "";
    var _merchantSubProductCode = "";
    var _SSS = "";
    var _isSSS = false;
    var _swipeCard = false;
    var _ARMLimit = "0";
    var _receiptMode = "4"; //PNB Automated Remittance Machine
    var _merchantORNo = "";
    var _exchangeRate = "";
    var _remittingCurrency = "";
    var _remittingCurrencyCode = "";
    var _arrOBPS = "";
    var _arrMerchantValidation = "";
    var _calender = false;
    var _isGFCLimit = false;
    var _auditRemittance = "";
    var _stopclock = "";
    var _bCharge = false;

    // **************************************************************************************** //
    // *** Common Functions and Methods
    // **************************************************************************************** //
    this.clock = function(isenable)
    {
        if (isenable == true)
        {
            var digital = new Date();
            var month = digital.toDateString();
            var hours = digital.getHours();
            var minutes = digital.getMinutes();
            var seconds = digital.getSeconds();
            var amOrPm = "AM";
            
            if (hours > 11) amOrPm = "PM";
            if (hours > 12) hours = hours - 12;
            if (hours == 0) hours = 12;
            if (minutes <= 9) minutes = "0" + minutes;
            if (seconds <= 9) seconds = "0" + seconds;
            
            dispMonth = "<b>"+month +"</b>";
            dispTime = "<b>"+hours + ":" + minutes + ":" + seconds + " " + amOrPm+"</b>";
            if (arm_page._stopclock == false)
            {
                document.getElementById("spnClock").innerHTML = dispTime;
                setTimeout("arm_page.clock(true)", 1000);
            }
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
        var whichCode = (window.Event) ? e.which : e.keyCode;

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
        }
        return false;
    }
    
    this.formatCurrency = function(num, places)
    {
        num = num.toString().replace(/\$|\,/g,'');
        if (isNaN(num) == false)
        {
            sign = (num == (num = Math.abs(num)));
            num = Math.floor(num * 100 + 0.50000000001);
            cents = num % 100;
            num = Math.floor(num / 100).toString();
            len = 0;
            cents = cents.toString();
            len = cents.length;
            
            for (var x = 0; x < (places - len); x++)
            {
                cents = cents + "0";
            }
            
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
            arm_page.closeWindow();
        }
    }
    
    this.clearDDL = function(element)
    {
        var select = document.getElementById(element);
        select.options.length = 0;
    }
    
    this.addValue = function(value, element)
    {
        document.getElementById(element).value = document.getElementById(element).value + value;
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

    this.showControl = function(element, isvisible)
    {
        (isvisible == true) ? document.getElementById(element).style.display = "block" : document.getElementById(element).style.display = "none";
    }
    
    this.assignPosition = function(element)
    {
        document.getElementById(element).style.left = (window.event.clientX + 10) + "px";
		document.getElementById(element).style.top = (window.event.clientY + 10) + "px";
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
    
    this.showAlert = function(message, willclose)
    {
        if (willclose == true)
        {
            (message != "") ? alert(message) : alert("An error occured while getting kiosk information. Please report to the nearest PNB branch.  \r\n\r\n Kiosk will close now.");
            arm_page.edButtons("btnContinue", true);
            arm_page.edButtons("btnBack", true);
            arm_page.closeWindow();
        }
        else
        {
            (message != "") ? alert(message) : alert("An error occured while getting kiosk information. Please report to the nearest PNB branch.");
            arm_page.edButtons("btnContinue", false);
            arm_page.edButtons("btnBack", false);
        }
    }
    
    this.closeWindow = function()
    {
        arm_page.writeLog("", "LOGOFF");
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
        arm_page.enterValue(returnvalue, element, false);
    }
    
    this.getTableValue = function(table, fieldValue, row)
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
                    if (iRow.Rows[i][sFieldValue[j]] == sMatchValue[j])
                    {
                        boolMatch[count] = true;
                        count = count + 1;
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
            
            select.options[0] = new Option("", "");
            for (var i = 0; i <= (iRow.Rows.length - 1); i++)
            {
                for (var arr = 0; arr <= (arrSplit.length - 1); arr++)
                {
                    if ((arrSplit.length - 1) == arr)
                    {
                        if (iRow.Rows[i][arrSplit[arr]] != null && arm_page.trim(iRow.Rows[i][arrSplit[arr]]) != "")
                        {
                            arrValue = arrValue + iRow.Rows[i][arrSplit[arr]];
                        }
                    }
                    else
                    {
                        if (iRow.Rows[i][arrSplit[arr]] != null && arm_page.trim(iRow.Rows[i][arrSplit[arr]]) != "")
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
            arm_page.getDDL(element, selected, true);
        }
    }
    
    this.findDDL = function(element, findvalue, selected)
    {
        var select = document.getElementById(element);
        for (var i = 0; i <= (select.length - 1); i++)
        {
            if (select.options[i].text == findvalue)
            {
                select.selectedIndex = i;
                arm_page.getDDL(element, selected, true);
                break;
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
    
    this.showPanel = function(divName)
    {
        var div = document.getElementById(divName);
        
        divmask = new maskpanel();
        divmask.show();
        arm_page.showControl(divName, true);
        
        div.style.top = (((document.documentElement.clientHeight / 2) + document.documentElement.scrollTop) - (div.offsetHeight / 2)) + "px";
        div.style.left = (((document.documentElement.clientWidth / 2) + document.documentElement.scrollLeft) - (div.offsetWidth / 2)) + "px";
    }

    this.hidePanel = function(divName)
    {
        arm_page.showControl(divName, false);
        divmask.hide();
    }
    
    var maskpanel = function()
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
    
    // **************************************************************************************** //
    // *** Page Functions and Methods
    // **************************************************************************************** //
    this.getPageID = function(isnextpage)
    {
        if (document.getElementById("lblPageID").value == "")
        {
            document.getElementById("lblPageID").value = "1";
            return 1;
        }
        else
        {
            if (isnextpage == true)
            {
                document.getElementById("lblPageID").value = Number(document.getElementById("lblPageID").value) + Number(1);
            }
            else
            {
                document.getElementById("lblPageID").value = Number(document.getElementById("lblPageID").value) - Number(1);
            }
            return document.getElementById("lblPageID").value;
        }
    }
    
    this.setFocus = function()
    {
        if (arm_page.getValue("lblPageID", false) == "6" || arm_page.getValue("lblPageID", false) == "15")
        {
            arm_page.removeValue("txtCard", false);
            arm_page.showFocus("txtCard");
        }
    }
    
    this.edBackContinue = function(backbutton, continuebutton)
    {
        (backbutton == true) ? arm_page.edButtons("btnBack", false) : arm_page.edButtons("btnBack", true);
        (continuebutton == true) ? arm_page.edButtons("btnContinue", false) : arm_page.edButtons("btnContinue", true);
    }
    
    this.calculate = function(value)
    {
        x = 0;
        values = "n";
        token = 0;
        var boolFound = false;
        var amount = document.getElementById("txtAmount");
        
        if (value == ".")
        {
            for (var i = 0; i <= (amount.value.length - 1); i++)
            {
                if (amount.value.substr(i, 1) == ".")
                {
                    return;
                }
            }
        }
        
        if (!isNaN(value) || value == ".")
        {
            if (!token)
            {
                if (amount.value == 0)
                {
                    arm_page.enterValue(value, "txtAmount", false);
                }
                else
                {
                    arm_page.enterValue(amount.value + value, "txtAmount", false);
                }
            }
            else
            {
                arm_page.enterValue(value, "txtAmount", false);
                token = 0;
            }
            return;
        }
        else
        {
            if (value == "C")
            {
                arm_page.enterValue("0", "txtAmount", false);
                token = 0;
                return;
            }

            if (value == "CE")
            {
                arm_page.enterValue("0", "txtAmount", false);
                return;
            }

            if (value == "%")
            {
                arm_page.enterValue(amount.value / 100.0, "txtAmount", false);
                token = 1;
                return;
            }

            if (value == "+/-")
            {
                amount.value = -amount.value;
                token = 1;
                return;
            }

            if (value == "+" || value == "*" || value == "/" || value == "-" || value == "=")
            {
                token = 1;
                if (values != "n")
                {
                    if (values == "+")
                    {
                        x = x -(- amount.value);
                        amount.value = x;
                    }
                    
                    if (values == "-")
                    {
                        x = x - amount.value;
                        amount.value = x;
                    }
                    
                    if (values == "/")
                    {
                        x = x / amount.value;
                        amount.value = x;
                    }
                    
                    if (values == "*")
                    {
                        x = x * amount.value;
                        amount.value = x;
                    }
                }
                else
                {
                    x = amount.value;
                }
      
                if (value != "=")
                {
                    values = value;
                }
                else
                {
                    values = "n";
                }
                return;
            }
        }
    }

    this.disablePage = function(pageid)
    {
        var sPage = "divPage";
        for (var i = 1; i <= 11; i++) //increment the value when adding page.
        {
            if (i == Number(pageid))
            {
                document.getElementById(sPage + pageid).style.display = "block";
            }
            else
            {
                document.getElementById(sPage + i).style.display = "none";
            }
        }
    }
    
    this.submitCard = function()
    {
        if (arm_page._swipeCard == true)
        {
            if (arm_page.getValue("txtCard", false) != "")
            {
                if (window.event.srcElement.tagName == "INPUT" & window.event.srcElement.type == "text")
                {
                    if (window.event.keyCode == 13) // Enter key
                    {
                        arm_page._bCharge = true;
                        arm_page._sourcecurrency = "";
                        arm_page._sourcecurrency = arm_page.validateCard();
                        if (arm_page._sourcecurrency != "ERROR" && arm_page._sourcecurrency != undefined)
                        {
                            if (arm_page._remittanceType == "merchant")
                            {
                                arm_page.enterValue(arm_page.findValue(arm_page.tblProductCode, "Rapid Remit", "FldDescription|", "FldValue"), "lblProductCode", false);
                                arm_page.enterValue(arm_page.findValue(arm_page.tblImplementationCode, "Credit To Account", "FldDescription|", "FldValue"), "lblImplementationCode", false);
                                arm_page.enterValue(arm_page.findValue(arm_page.tblComputationLevel, "Net to Beneficiary", "FldDescription|", "FldValue"), "lblComputationLevel", false);
                                var arrSplit = arm_page.getValue("lblMerchant", false).split("|"); //CIFID|Currency|AccountSeqID|MemberIDNo|MerchantProductCode|SEQ_ID
                                arm_page.enterValue(arrSplit[0], "lblMerchantID", false);
                                arm_page.enterValue(arrSplit[1], "lblTargetCurrency", false);
                                arm_page.enterValue(arrSplit[2], "lblMerchantAcctSeqID", false);
                                arm_page.enterValue(arrSplit[3], "lblAccountNo", false);
                                arm_page.enterValue(arrSplit[4], "lblMerchantProductCode", false);
                                arm_page.enterValue(arrSplit[5], "lblMerchantSeqID", false);
                                arm_page.enterValue(arm_page.getValue("txtAmount", false), "lblAmount", false);
                            }
                            arm_page.showLoading("Verifying card information... Please wait", false);
                            if (arm_page._sourcecurrency != "")
                            {
                                arm_page.getCharges(arm_page._sourcecurrency);
                            }
                            else
                            {
                                while (arm_page._sourcecurrency == "")
                                {
                                    arm_page._sourcecurrency = arm_page.validateCard();
                                    if (arm_page._sourcecurrency != "")
                                    {
                                        break;
                                    }
                                }
                                arm_page.getCharges(arm_page._sourcecurrency);
                            }
                        }
                    }
                }
            }
        }
    }

    this.validateCard = function()
    {
        var cardno = "";
        cardno = arm_page.getCardInfo(arm_page.getValue("txtCard", false), 1);
        
        if (cardno != "" || cardno.length > 0)
        {
            if (arm_page.getCardStatus(cardno) == true)
            {
                arm_page.enterValue(arm_page.getCardInfo(arm_page.getValue("txtCard", false), 2), "lblCardName", false);
                arm_page.enterValue(arm_page.getCardInfo(arm_page.getValue("txtCard", false), 3), "lblExpiryDate", false);
                arm_page._sourcecurrency = arm_page.findValue(arm_page.tblCards, arm_page.getValue("lblDCCard", false), "CardNo|", "Currency");
                arm_page.enterValue(arm_page.findValue(arm_page.tblCards, arm_page.getValue("lblDCCard", false), "CardNo|", "CurrencyCode"), "lblSourceCurrencyCode", false);
                if (arm_page._sourcecurrency == "")
                {
                    arm_page._sourcecurrency = arm_page.findValue(arm_page.tblCards, arm_page.getValue("lblDCCard", false), "CardNo|", "Currency");
                    arm_page.enterValue(arm_page.findValue(arm_page.tblCards, arm_page.getValue("lblDCCard", false), "CardNo|", "CurrencyCode"), "lblSourceCurrencyCode", false);
                }
                arm_page.enterValue(arm_page._sourcecurrency, "lblSourceCurrency", false);
                if (arm_page._remittingCurrency != arm_page._sourcecurrency)
                {
                    arm_page._sourcecurrency = "ERROR";
                    arm_page.showAlert("DEBIT/CREDIT card currency [" + arm_page.getValue("lblSourceCurrencyCode", false) + "] and Remitted currency [" + arm_page._remittingCurrencyCode + "] is not the same. Please use another card and swipe again.\r\n\r\nFor any concerns, Please contact your nearest PNB branch.", false);
                    arm_page.removeValue("txtCard", false);
                }
                else
                {
                    return arm_page._sourcecurrency;
                }
            }
            else
            {
                arm_page._sourcecurrency = "ERROR";
                arm_page.showAlert("A problem occured while getting DEBIT/CREDIT card information. Please check possible reasons why card was rejected: \r\n\r\n" + "1. DEBIT/CREDIT card is not valid. Please check card and try again. \r\n" + "2. DEBIT/CREDIT card is not yet pre-enrolled. Please go to the nearest PNB branch. \r\n" + "3. DEBIT/CREDIT card is expired. Please use another card.", false);
                arm_page.removeValue("txtCard", false);
            }
        }
    }

    this.getCardStatus = function(cardno)
    {
        var arrSplit = document.getElementById("lblDBCards").value.split("|");
        var boolValid = false;
        var cardValue = "";
        var cardtype = "";
        
        for (var i = 0; i <= (arrSplit.length - 1); i++)
        {
            cardValue = arm_page.trimCard(cardno, arrSplit[i]);
            if (cardValue.length == arrSplit[i].length && cardValue != "")
            {
                if (cardValue == arrSplit[i] && cardValue.length == arrSplit[i].length)
                {
                    if (arm_page.findValue(arm_page.tblCards, cardValue, "CardNo|", "CardClassDesc") == arm_page._transactionsource)
                    {
                        arm_page.getAcctSeqID(cardValue);
                        arm_page.enterValue(cardValue, "lblDCCard", false);
                        boolValid = true;
                    }
                    else
                    {
                        arm_page.showAlert(arm_page._transactionsource + " is not valid. Please try another card.", false);
                        boolValid = false;
                    }
                    break;
                }
            }
        }
        return boolValid;
    }

    this.trimCard = function(cardno, cardValue)
    {
        var returnvalue = "";
        returnvalue = (cardno.length != cardValue.length) ? returnvalue = cardno.substring(0, cardValue.length) : returnvalue = cardno;
        return returnvalue;
    }

    this.getCardInfo = function(cardno, cardinfo) //1=cardno, 2=name, 3=expirydate
    {
        var returnvalue = "";
        var trimvalue = "";
        var icount = 0;
        
        if (cardno != "" && cardno != null && cardno != undefined)
        {
            for (var i = 0; i <= (cardno.length - 1); i++)
            {
                trimvalue = cardno.substr(i, 1);
                if (icount <= 2)
                {
                    icount = (trimvalue == "^") ? icount = icount + 1 : icount = icount;
                }
                
                switch (cardinfo)
                {
                    case 1:
                        if (arm_page.isNumeric(trimvalue) == true && returnvalue.length < 19)
                        {
                            returnvalue = returnvalue + trimvalue;
                        }
                        break;
                    case 2:
                        returnvalue = (icount == 1 && trimvalue != "^") ? returnvalue + trimvalue : returnvalue = returnvalue;                    
                        break;
                    case 3:
                        returnvalue = (icount == 2  && trimvalue != "^" && returnvalue.length < 4) ? returnvalue + trimvalue : returnvalue = returnvalue;
                        break;
                }
            }
        }
        return returnvalue;
    }
    
    this.validateMerchantMandatory = function(ddl1, ddl2, datefrom, dateto, text1, text2, remarks)
    {
        if (ddl1 == true)
        {
            if (arm_page.trim(arm_page.getValue("lblMerchantType1", false)) == "")
            {
                arm_page.showAlert(arm_page.getValue("spnMerchantDDL1", true) + " is a mandatory field.", false);
                arm_page.getPageID(false);
                return false;
            }
        }
        if (ddl2 == true)
        {
            if (arm_page.trim(arm_page.getValue("lblMerchantType2", false)) == "")
            {
                arm_page.showAlert(arm_page.getValue("spnMerchantDDL2", true) + " is a mandatory field.", false);
                arm_page.getPageID(false);
                return false;
            }
        }
        if (datefrom == true)
        {
            if (arm_page.trim(arm_page.getValue("txtDateFrom", false)) == "" || arm_page.getValue("txtDateFrom", false) == "Click here to edit")
            {
                arm_page.showAlert(arm_page.getValue("spnDateFrom", true) + " is a mandatory field.", false);
                arm_page.getPageID(false);
                return false;
            }
        }
        if (dateto == true)
        {
            if (arm_page.trim(arm_page.getValue("txtDateTo", false)) == "" || arm_page.getValue("txtDateTo", false) == "Click here to edit")
            {
                arm_page.showAlert(arm_page.getValue("spnDateTo", true) + " is a mandatory field.", false);
                arm_page.getPageID(false);
                return false;
            }
        }
        if (text1 == true)
        {
            if (arm_page.trim(arm_page.getValue("txtMerchantDetail1", false)) == "" || arm_page.getValue("txtMerchantDetail1", false) == "Click here to edit")
            {
                arm_page.showAlert(arm_page.getValue("spnMerchantDetail1", true) + " is a mandatory field.", false);
                arm_page.getPageID(false);
                return false;
            }
        }
        if (text2 == true)
        {
            if (arm_page.trim(arm_page.getValue("txtMerchantDetail2", false)) == "" || arm_page.getValue("txtMerchantDetail2", false) == "Click here to edit")
            {
                arm_page.showAlert(arm_page.getValue("spnMerchantDetail2", true) + " is a mandatory field.", false);
                arm_page.getPageID(false);
                return false;
            }
        }
        if (remarks == true)
        {
            if (arm_page.trim(arm_page.getValue("txtMerchantDetail3", false)) == "" || arm_page.getValue("txtMerchantDetail3", false) == "Click here to edit")
            {
                arm_page.showAlert(arm_page.getValue("spnMerchantDetail3", true) + " is a mandatory field.", false);
                arm_page.getPageID(false);
                return false;
            }
        }
        return true;
    }
    
    this.validatePage = function(type)
    {
        switch (type)
        {
            case "Merchant":
                if (document.getElementById("ddlMerchant").selectedIndex <= 0)
                {
                    arm_page.showAlert("Please select a merchant.", false);
                    arm_page.getPageID(false);
                    return false;
                }
                else
                {
                    return true;
                }
                break;
            case "Beneficiary":
                if (document.getElementById("ddlBeneficiary").selectedIndex <= 0)
                {
                    arm_page.showAlert("Please select a beneficiary.", false);
                    arm_page.getPageID(false);
                    return false;
                }
                else
                {
                    return true;
                }
                break;
            case "Amount":
                if (arm_page.getValue("txtAmount", false) == "0.00" || arm_page.getValue("txtAmount", false) == "" || Number(arm_page.getValue("txtAmount", false)) <= 0 || arm_page.getValue("txtAmount", false) == ".")
                {
                    arm_page.showAlert("Amount cannot be left blank. Please enter an amount before pressing the continue button.", false);
                    arm_page.getPageID(false);
                    return false;
                }
                else
                {
                    if (Number(parseFloat(arm_page.getValue("txtAmount", false).replace(",", "")).toFixed(2)) > Number(parseFloat(arm_page._ARMLimit.replace(",", "")).toFixed(2)))
                    {
                        arm_page.showAlert("You have exceeded the remittance amount limit. Please enter a smaller amount.", false);
                        arm_page.getPageID(false);
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                break;
            case "Purpose":
                if (document.getElementById("ddlPurpose").selectedIndex == 0)
                {
                    arm_page.showAlert("Please select purpose of remittance.", false);
                    arm_page.getPageID(false);
                    return false;
                }
                else
                {
                    return true;
                }
                break;
            case "Source":
                if (document.getElementById("ddlTransactionSource").selectedIndex == 0)
                {
                    arm_page.showAlert("Please select a transaction source.", false);
                    arm_page.getPageID(false);
                    return false;
                }
                else
                {
                    arm_page._transactionsource = document.getElementById("ddlTransactionSource").options[document.getElementById("ddlTransactionSource").selectedIndex].text;
                    return true;
                }
                break;
            case "SecurityCode":
                if (arm_page.getValue("txtSecurityCode", false) == "")
                {
                    arm_page.showAlert("Security code cannot be left blank. Please enter your security code (see sample beside) before pressing the continue button.", false);
                    arm_page.getPageID(false);
                    return false;
                }
                else
                {
                    if (arm_page.getValue("txtSecurityCode", false).length != 3)
                    {
                        arm_page.showAlert("Security code should have 3 numeric values. Please enter your security code (see sample beside) before pressing the continue button.", false);
                        arm_page.getPageID(false);
                        return false;
                    }
                    else
                    {
                        if (arm_page._isGFCLimit == false)
                        {
                            return true;
                        }
                        else
                        {
                            arm_page.showAlert("You have exceeded the maximum aggregated amount for this account. Please enter a smaller amount.", false);
                            arm_page.getPageID(false);
                            return false;
                        }
                    }
                }
                break;
        }
    }
    
    this.edNumbers = function(isdisable)
    {
        if (isdisable == true)
        {
            document.getElementById("btnOne").disabled = "disabled";
            document.getElementById("btnTwo").disabled = "disabled";
            document.getElementById("btnThree").disabled = "disabled";
            document.getElementById("btnFour").disabled = "disabled";
            document.getElementById("btnFive").disabled = "disabled";
            document.getElementById("btnSix").disabled = "disabled";
            document.getElementById("btnSeven").disabled = "disabled";
            document.getElementById("btnEight").disabled = "disabled";
            document.getElementById("btnNine").disabled = "disabled";
            document.getElementById("btnZero").disabled = "disabled";
            document.getElementById("btnPoint").disabled = "disabled";
            document.getElementById("btnClear").disabled = "disabled";
        }
        else
        {
            document.getElementById("btnOne").disabled = "";
            document.getElementById("btnTwo").disabled = "";
            document.getElementById("btnThree").disabled = "";
            document.getElementById("btnFour").disabled = "";
            document.getElementById("btnFive").disabled = "";
            document.getElementById("btnSix").disabled = "";
            document.getElementById("btnSeven").disabled = "";
            document.getElementById("btnEight").disabled = "";
            document.getElementById("btnNine").disabled = "";
            document.getElementById("btnZero").disabled = "";
            document.getElementById("btnPoint").disabled = "";
            document.getElementById("btnClear").disabled = "";
        }
    }
    
    this.submitBackContinue = function(pageid, isnext)
    {
        arm_page._nextpage = isnext;
        arm_page.edButtons("btnBack", true);
        arm_page.edButtons("btnContinue", true);
        switch (Number(pageid))
        {
            case 1: //Page 1 Remitter & Forex
                arm_page.showControl("divSecurity", false);
                arm_page.disablePage(1);
                arm_page.edButtons("btnBack", true);
                arm_page.edButtons("btnContinue", false);
                arm_page.showControl("btnBack", false);
                arm_page.showControl("btnContinue", false);
                break;
            case 2: //Page 2 Beneficiary
                arm_page.showControl("divSecurity", false);
                arm_page.disablePage(2);
                arm_page.edButtons("btnBack", false);
                arm_page.edButtons("btnContinue", false);
                arm_page.showControl("btnBack", true);
                arm_page.showControl("btnContinue", true);
                arm_page.loadARMInfo(2);
                break;
            case 3: //Page 3 Amount
                arm_page.showControl("divSecurity", false);
                if (arm_page.validatePage("Beneficiary") == true)
                {
                    arm_page.disablePage(3);
                    arm_page.edButtons("btnBack", false);
                    arm_page.edButtons("btnContinue", false);
                    arm_page.showControl("btnContinue", true);
                    arm_page.loadARMInfo(3);
                }
                break;
            case 4: //Page 4 Purpose
                arm_page.showControl("divSecurity", false);
                if (arm_page.validatePage("Amount") == true)
                {
                    arm_page.disablePage(4);
                    arm_page.enterValue(arm_page.getValue("txtAmount", false), "lblAmount", false);
                    arm_page.edButtons("btnBack", false);
                    arm_page.edButtons("btnContinue", false);
                    arm_page.showControl("btnContinue", true);
                    arm_page.loadARMInfo(4);
                }
                break;
            case 5: //Page 4 Source Of Funds
                arm_page.showControl("divSecurity", false);
                if (arm_page.validatePage("Purpose") == true)
                {
                    arm_page.disablePage(5);
                    arm_page.edButtons("btnBack", false);
                    arm_page.edButtons("btnContinue", false);
                    arm_page.showControl("btnContinue", true);
                    arm_page.loadARMInfo(5);
                }
                break;
            case 6: //Page 5 Swipe Debit/Credit Cards
                arm_page.showControl("divSecurity", false);
                if (arm_page.validatePage("Source") == true)
                {
                    arm_page._swipeCard = true;
                    arm_page.edButtons("btnBack", false);
                    arm_page.edButtons("btnContinue", false);
                    arm_page.showControl("btnContinue", false);
                    arm_page.disablePage(6);
                    arm_page.loadARMInfo(6);
                }
                break;
            case 7: //Page 6 Security Code
                arm_page._swipeCard = false;
                arm_page.removeValue("txtSecurityCode", false);
                arm_page.showControl("divSecurity", true);
                arm_page.disablePage(7);
                arm_page.edButtons("btnBack", false);
                arm_page.edButtons("btnContinue", false);
                arm_page.showControl("btnContinue", true);
                break;
            case 8: //Page 7 Transaction Information
                if (arm_page.validatePage("SecurityCode") == true)
                {
                    arm_page.getGFCAggregate();
                    arm_page.disablePage(8);
                    arm_page.edButtons("btnBack", true);
                    arm_page.edButtons("btnContinue", false);
                    arm_page.showControl("divSecurity", false);
                    arm_page.showControl("btnBack", false);
                    arm_page.showControl("btnContinue", false);                    
                    arm_page.loadARMInfo(7);
                }
                else
                {
                    arm_page.showControl("divSecurity", true);
                }
                break;
            case 9: //Page 8 Print Receipt
                arm_page.edButtons("btnBack", false);
                arm_page.edButtons("btnContinue", false);
                arm_page.showControl("divSecurity", false);
                arm_page.showControl("btnBack", false);
                arm_page.showControl("btnContinue", false);
                arm_page.loadARMInfo(8);
                break;
            case 10: //Page 9 Thank you
                if (_isremittance == true)
                {
                    arm_page.disablePage(10);
                    arm_page.showControl("divSecurity", false);
                    arm_page.showControl("btnBack", false);
                    arm_page.showControl("btnContinue", false);
                    setTimeout("arm_page.closeWindow();", 3000);
                }
                else
                {
                    arm_page.showControl("divSecurity", false);
                    arm_page.disablePage(1);
                    arm_page.edButtons("btnBack", true);
                    arm_page.edButtons("btnContinue", false);
                    arm_page.showControl("btnBack", false);
                    arm_page.showControl("btnContinue", false);
                }
                break;
            case 11: //Merchant
                arm_page.showControl("divSecurity", false);
                arm_page.disablePage(11);
                arm_page.edButtons("btnBack", false);
                arm_page.edButtons("btnContinue", false);
                arm_page.showControl("btnBack", true);
                arm_page.showControl("btnContinue", true);
                arm_page.loadARMInfo(9);
                break;
            case 12: //Merchant Amount
                arm_page.showControl("divSecurity", false);
                if (arm_page.validatePage("Merchant") == true)
                {
                    arm_page.setMerchantORNo();
                    if (arm_page._iscontribution == true && arm_page._totalamount != "")
                    {
                        arm_page.enterValue(arm_page._totalamount, "txtAmount", false);
                        arm_page.edNumbers(true);
                    } 
                    else
                    {
                        arm_page.enterValue("0.00", "txtAmount", false);
                        arm_page.edNumbers(false);
                    }
                    arm_page.disablePage(3);
                    arm_page.edButtons("btnBack", false);
                    arm_page.edButtons("btnContinue", false);
                    arm_page.showControl("btnBack", true);
                    arm_page.showControl("btnContinue", true);                    
                    arm_page.loadARMInfo(3);
                }
                else
                {
                    arm_page.validateMerchant();
                }
                break;
            case 13: //Merchant Purpose
                arm_page.showControl("divSecurity", false);
                if (arm_page.validatePage("Amount") == true)
                {
                    arm_page.disablePage(4);
                    arm_page.enterValue(arm_page.getValue("txtAmount", false), "lblAmount", false);
                    arm_page.showControl("btnBack", false);
                    arm_page.showControl("btnContinue", false);
                    arm_page.loadARMInfo(4);
                }
                break;
            case 14: //Merchant Source Of Funds
                arm_page.showControl("divSecurity", false);
                if (arm_page.validatePage("Purpose") == true)
                {
                    arm_page.disablePage(5);
                    arm_page.edButtons("btnBack", false);
                    arm_page.edButtons("btnContinue", false);
                    arm_page.showControl("btnBack", true);
                    arm_page.showControl("btnContinue", true);
                    arm_page.loadARMInfo(5);
                }
                break;
            case 15: //Merchant Swipe Debit/Credit Cards
                arm_page.showControl("divSecurity", false);
                if (arm_page.validatePage("Source") == true)
                {
                    arm_page._swipeCard = true;
                    arm_page.edButtons("btnBack", false);
                    arm_page.edButtons("btnContinue", false);
                    arm_page.showControl("btnContinue", false);
                    arm_page.disablePage(6);
                    arm_page.loadARMInfo(6);
                }
                break;
            case 16: //Merchant Security Code
                arm_page._swipeCard = false;
                arm_page.removeValue("txtSecurityCode", false);
                arm_page.showControl("divSecurity", true);
                arm_page.disablePage(7);
                arm_page.edButtons("btnBack", false);
                arm_page.edButtons("btnContinue", false);
                arm_page.showControl("btnContinue", true);
                break;
            case 17: //Merchant Transaction Information            
                if (arm_page.validatePage("SecurityCode") == true)
                {
                    arm_page.disablePage(8);
                    arm_page.edButtons("btnBack", true);
                    arm_page.edButtons("btnContinue", false);
                    arm_page.showControl("divSecurity", false);
                    arm_page.showControl("btnBack", false);
                    arm_page.showControl("btnContinue", false);                    
                    arm_page.loadARMInfo(7);
                }
                else
                {
                    arm_page.showControl("divSecurity", true);
                }
                break;
            case 18: //Merchant Print Receipt            
                arm_page.edButtons("btnBack", false);
                arm_page.edButtons("btnContinue", false);
                arm_page.showControl("divSecurity", false);
                arm_page.showControl("btnBack", false);
                arm_page.showControl("btnContinue", false);
                arm_page.loadARMInfo(8);
                break;
            case 19: //Merchant Thank you
                arm_page.disablePage(10);
                arm_page.showControl("divSecurity", false);
                arm_page.showControl("btnBack", false);
                arm_page.showControl("btnContinue", false);
                setTimeout("arm_page.closeWindow();", 3000);
                break;
        }
    }
    
    // **************************************************************************************** //
    // *** Page Initialize
    // **************************************************************************************** //
    this.loadARMInfo = function(pageid)
    {
        arm_page._stopclock = false;
        arm_page.clock(true);
        if (arm_page._iscontribution == true)
        {
            arm_page.enterValue("Please click CONTINUE button", "spnAmount", true);
        }
        else
        {
            if (arm_page._remittanceType == "remittance")
            {
                arm_page.enterValue("Please enter amount to be paid to your beneficiary:", "spnAmount", true);
            }
            else
            {
                arm_page.enterValue("Please enter amount to be paid to your merchant:", "spnAmount", true);
            }
        }
        //this.enterValue("Copyright 2009, Philippine National Bank", "spnFooter", true);
        switch (pageid)
        {
            case 1:
                // *** Set Default Values
                arm_page._isGFCLimit = false;
                arm_page._bCharge = false;
                
                // *** Get Exchange Rate
                this.showLoading("Retrieving Today's Exchange Rate... Please wait", false);
                this.getSourceCurrencyCode();
                this.getTargetCurrencyCode();
                this.getExchangeRate();
                this.getSourceCurrency();
                this.getSourceCurrencyFldCode();
                
                // *** Get Branch Code
                this.showLoading("Retrieving Kiosk Information... Please wait", false);
                this.getBranchCode();
                this.getLocalTime();
                this.getServerTime();
                this.getARMLimit();
                
                // *** Get Look Values
                arm_page._lookupField = "Currency";
                this.getLookup();
                arm_page._lookupField = "ProductCode";
                this.getLookup();
                arm_page._lookupField = "ImplementationType";
                this.getLookup();
                arm_page._lookupField = "Computation";
                this.getLookup();
                arm_page._lookupField = "RemitMethod";
                this.getLookup();
                arm_page._lookupField = "PayMode";
                this.getLookup();
                arm_page._lookupField = "ReceiptMode";
                this.getLookup();
                
                // *** Get Kiosk Default Currency
                this.getKioskCurrency();
                
                // *** Get GFMC Information
                this.showLoading("Retrieving GFMC Membership Information... Please wait", false);
                this.getGFMC();
                this.getGFMCRemitter();
                this.getRemitterID();
                this.getRemitterCards();
                
                // *** Increment Page ID
                this.getPageID(true);
                break;
            case 2:
                // *** Get Beneficiaries Information
                this.showLoading("Retrieving Beneficiaries Information... Please wait", false);
                arm_page.enterValue("Please enter amount to be sent to your beneficiary:", "spnAmount", true);
                arm_page.enterValue("0.00", "txtAmount", false);
                this.getGFMCBeneficiary();
                break;
            case 3:
                // *** Get Product Code / Implementation Code / Currency
                var arrSplit = arm_page.getValue("lblBeneficiary", false).split("|"); //CIFID|ProductCode|ImplementationCode|BenAcctSeqID|Currency
                arm_page.enterValue(arrSplit[0], "lblBeneficiaryID", false);
                arm_page.enterValue(arrSplit[1], "lblProductCode", false);
                arm_page.enterValue(arrSplit[2], "lblImplementationCode", false);
                arm_page.enterValue(arrSplit[3], "lblBenAcctSeqID", false);
                arm_page.enterValue(arrSplit[4], "lblTargetCurrency", false);
                arm_page.enterValue(arm_page.getTableValue(arm_page.tblDefaults, "ChargeType", 0), "lblChargeType", false);
                arm_page.enterValue(arm_page.getTableValue(arm_page.tblDefaults, "Computation", 0), "lblComputationLevel", false);
                if (arm_page._remittanceType == "remittance")
                {
                    arm_page.enterValue(arm_page.getValue("lblSourceCurrencyShortCode", false), "spnTargetCurrency", true);
                }
                break;
            case 4:
                this.showLoading("Retrieving Transaction Purpose... Please wait", false);
                this.getBeneficiaryAddress();
                this.getTransactionPurpose();
                break;
            case 5:
                this.showLoading("Retrieving Transaction Source... Please wait", false);
                this.getTransactionSource();
                break;
            case 6:
                this.enterValue(arm_page.getDDLValue("ddlTransactionSource", false), "spnCardType", true);
                this.removeValue("txtCard");
                this.setFocus();
                break;
            case 7:
                this.enterValue(arm_page.getValue("txtSecurityCode", false), "lblSecurityCode", false);
                this.showLoading("Connecting to Payment Gateway... Please wait", false);
                this.submitPaymentGateway();
                break;
            case 8:
                this.showLoading("Transmitting transaction... Please wait", false);
                this.submitRemittance();
                break;
            case 9:
                arm_page.enterValue("PHP", "spnTargetCurrency", true);
                arm_page.enterValue("0.00", "txtAmount", false);
                break;
        }
    }
    
    this.loadCalendar = function()
    {
	    new JsDatePick(
	    {
		    useMode:2,
		    target:"txtDateFrom"
		    //limitToToday:true <-- Add this should you want to limit the calendar until today.
	    });
	    
	    new JsDatePick(
	    {
		    useMode:2,
		    target:"txtDateTo"
		    //limitToToday:true <-- Add this should you want to limit the calendar until today.
	    });
    }
    
    this.validateMerchant = function()
    {
        arm_page.loadCalendar();
        arm_page.removeValue("spnMerchant", true);
        arm_page.removeValue("spnMerchantDDL1", true);
        arm_page.removeValue("spnMerchantDDL2", true);
        arm_page.removeValue("spnMerchantDDL3", true);
        arm_page.removeValue("spnMerchantDetail1", true);
        arm_page.removeValue("spnMerchantDetail2", true);
        arm_page.removeValue("spnMerchantDetail3", true);
        arm_page.getDDL("ddlMerchant", "lblMerchant", true);
        arm_page.getDDL("ddlMerchant", "lblMerchantName", false);
        arm_page.clearDDL("ddlMerchantType1");
        arm_page.clearDDL("ddlMerchantType2");
        arm_page.clearDDL("ddlMerchantType3");
        arm_page.showControl("divCalendar", false);
        arm_page.showControl("divMerchantDDL1", false);
        arm_page.showControl("divMerchantDDL2", false);
        arm_page.showControl("divMerchantDDL3", false);
        arm_page.showControl("divMerchantDesc1", false);
        arm_page.showControl("divMerchantDesc2", false);
        arm_page.showControl("divMerchantDesc3", false);
        arm_page.enterValue("Click here to edit", "txtDateFrom", false);
        arm_page.enterValue("Click here to edit", "txtDateTo", false);
        arm_page.enterValue("Click here to edit", "txtMerchantDetail1", false);
        arm_page.enterValue("Click here to edit", "txtMerchantDetail2", false);
        arm_page.enterValue("Click here to edit", "txtMerchantDetail3", false);
        
        var arrMerchant = arm_page.getValue("lblMerchantName", false);
        var merchant = arrMerchant.split(","); //MerchantCode|MemberIDNo|MerchantProductCodeDesc
        
        var arrMerchant2 = arm_page.getValue("lblMerchant", false);
        var merchant2 = arrMerchant2.split("|"); //CIFID|Currency|AccountSeqID|MemberIDNo|MerchantProductCode|SEQ_ID
        
        arm_page.enterValue("Application Period (FROM)", "spnDateFrom", true);
        arm_page.enterValue("Application Period (TO)", "spnDateTo", true);
        
        (merchant[0] != "") ? arm_page._merchantName = arm_page.trim(merchant[0]) : arm_page._merchantName = "";
        (merchant[2] != "") ? arm_page._merchantProduct = arm_page.trim(merchant[2]) : arm_page._merchantProduct = "";
        (merchant2[4] != "") ? arm_page._merchantSubProductCode = merchant2[4] : arm_page._merchantSubProductCode = "";
        
        if (arm_page._merchantName != "")
        {
            arm_page.showLoading("Retrieving Merchant Information... Please wait", false);
            arm_page.enterValue(arm_page._merchantName + " - " + arm_page._merchantProduct, "spnMerchantName", true);
            arm_page.getMerchantLookup(arm_page._merchantName);
        }
        else
        {
            arm_page.closeQwerty(true);
        }
    }
    
    this.showMerchantInformation = function()
    {
        arm_page._iscontribution = false;
        var arrMerchant = arm_page.getValue("lblMerchantName", false);
        var merchant = arrMerchant.split(","); //MerchantCode|MemberIDNo|MerchantProductCodeDesc
        var span = document.getElementById("spnMerchant");
        var ddl1 = "";
        var ddl2 = "";
        var ddl3 = "";
        var month = "";
        var span1 = "";
        var select = "";
        var ddlvalue = "";
       
        select = document.getElementById("ddlMerchantType1");
        (select.selectedIndex > 0) ? ddl1 = select.options[select.selectedIndex].text : ddl1 = "";
        select = document.getElementById("ddlMerchantType2");
        (select.selectedIndex > 0) ? ddl2 = select.options[select.selectedIndex].text : ddl2 = "";
        select = document.getElementById("ddlMerchantType3");
        (select.selectedIndex > 0) ? ddl3 = select.options[select.selectedIndex].text : ddl3 = "";
        
        arm_page._totalamount = "";
        
        for (var j = 1; j <= 3; j++)
        {
            if (arm_page.getValue("spnMerchantDDL" + j, true) != "")
            {
                ddlvalue = eval("ddl" + j);
                if (ddlvalue != "")
                {
                    span1 += arm_page.getValue("spnMerchantDDL" + j, true) + ": " + ddlvalue + "<br>";
                }
            }
            
        }
        for (var k = 1; k <= 3; k++)
        {
            if (arm_page.getValue("spnMerchantDetail" + k, true) != "" && arm_page.getValue("txtMerchantDetail" + k, false) != "Click here to edit")
            {
                span1 += arm_page.getValue("spnMerchantDetail" + k, true) + ": " + arm_page.getValue("txtMerchantDetail" + k, false) + "<br>";
            }
        }
        
        if (arm_page.trim(merchant[0]) == "SSS")
        {
            arm_page._SSS = arm_page.trim(merchant[2]);
            if (arm_page.trim(merchant[2]) == "CONT")
            {
                arm_page._iscontribution = true;
                month = arm_page.computeMonths(arm_page.formatdate(arm_page.getValue("txtDateFrom", false)), arm_page.formatdate(arm_page.getValue("txtDateTo", false)));
                arm_page._totalamount = Number(ddl1) * Number(month);
                arm_page._totalamount = arm_page.formatCurrency(Number(arm_page._totalamount), 2);
                span1 += "<p>Contribution Amount: P" + arm_page.formatCurrency(Number(ddl1), 2) + "<br>";
                span1 += "Total Month: " + month + "<br>";
                span1 += "Total Payment: P" + arm_page._totalamount + "<br>"
            }
        }
        
        if (arm_page.getValue("txtDateFrom", false) != "Click here to edit" && arm_page.getValue("txtDateTo", false) != "Click here to edit")
        {
            span1 += arm_page.getValue("spnDateFrom", true) + ": " + arm_page.getValue("txtDateFrom", false) + "<br>";
            span1 += arm_page.getValue("spnDateTo", true) + ": " + arm_page.getValue("txtDateTo", false) + "<br>";
        }
        
        span.innerHTML = "";
        span.innerHTML = span1;
        arm_page.enterValue("", "spnQwerty", true);
        arm_page.enterValue("", "spnMerchantSelect", true);
    }
    
    this.validateMerchantValue = function(element, elementtype, caption, ismandatory, datatype)
    {
        var select = "";
        switch (elementtype)
        {
            case "ddl":
                select = document.getElementById(element);
                if (select.options[select.selectedIndex].text == "" && ismandatory == 1)
                {
                    arm_page.showAlert(caption + " is a mandatory field. Please select a value before pressing the submit button.", false);
                    arm_page.showFocus(element);
                    return false;
                }
                else
                {
                    return true;
                }
                break;
            case "dt":
                break;
            case "txt":
                if (arm_page.trim(arm_page.getValue(element, false)) == "" && ismandatory == 1)
                {
                    arm_page.showAlert(caption + " is a mandatory field. Please enter value before pressing the submit button.", false);
                    arm_page.showFocus(element);
                    return false;
                }
                else
                {
                    if (arm_page.getValue(element, false) == "Click here to edit" && ismandatory == 1)
                    {
                        arm_page.showAlert(caption + " is a mandatory field. Please enter value before pressing the submit button.", false);
                        arm_page.showFocus(element);
                        return false;
                    }
                    else
                    {
                        if (datatype == "N")
                        {
                            if (arm_page.isNumeric(arm_page.getValue(element, false)) == true)
                            {
                                return true;
                            }
                            else
                            {
                                arm_page.showAlert(caption + " accepts only numeric values. Please check and try again.", false);
                                arm_page.showFocus(element);
                                return false;
                            }
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
                break;
        }
    }
    
    this.clearQwerty = function()
    {
        (arm_page.getValue("txtDateFrom", false) == "") ? arm_page.enterValue("Click here to edit", "txtDateFrom", false) : "";
        (arm_page.getValue("txtDateTo", false) == "") ? arm_page.enterValue("Click here to edit", "txtDateTo", false) : "";
        (arm_page.getValue("txtMerchantDetail1", false) == "") ? arm_page.enterValue("Click here to edit", "txtMerchantDetail1", false) : "";
        (arm_page.getValue("txtMerchantDetail2", false) == "") ? arm_page.enterValue("Click here to edit", "txtMerchantDetail2", false) : "";
        (arm_page.getValue("txtMerchantDetail3", false) == "") ? arm_page.enterValue("Click here to edit", "txtMerchantDetail3", false) : "";
        arm_page.enterValue("", 'spnQwerty', true);
    }
    
    this.showQwerty = function(inputid)
    {
        arm_page.removeValue(inputid, false);
        arm_page._qwerty = inputid;
        arm_page.enterValue(arm_page.getValue("divQwerty", true), "spnQwerty", true);
    }
    
    this.closeQwerty = function(iscancel)
    {
        if (iscancel == true)
        {
            arm_page.enterValue("Click here to edit", "txtDateFrom", false);
            arm_page.enterValue("Click here to edit", "txtDateTo", false);
            arm_page.enterValue("Click here to edit", "txtMerchantDetail1", false);
            arm_page.enterValue("Click here to edit", "txtMerchantDetail2", false);
            arm_page.enterValue("Click here to edit", "txtMerchantDetail3", false);
            arm_page.removeValue("spnMerchant", true);
            document.getElementById("ddlMerchantType1").selectedIndex = 0;
            document.getElementById("ddlMerchantType2").selectedIndex = 0;
            document.getElementById("ddlMerchantType3").selectedIndex = 0;
            document.getElementById("ddlMerchant").selectedIndex = 0;
            arm_page.hidePanel("divMerchantType");
        }
        else
        {
            var arrSplit = "";
            var boolValid = false;
            var month = "";
            var isValid = 0;
            
            for (var i = 0; i <= (arm_page._arrMerchantValidation.length - 1); i++)
            {
                arrSplit = arm_page._arrMerchantValidation[i].split("|");
                for (var j = 1; j <= 3; j++)
                {
                    if (arm_page.getValue("spnMerchantDDL" + j, true) == arrSplit[3])
                    {
                        boolValid = arm_page.validateMerchantValue("ddlMerchantType" + j, "ddl", arrSplit[3], arrSplit[4], arrSplit[5]);
                        if (boolValid == false)
                        {
                            isValid += 1;
                            return;
                        }
                    }
                    if (arm_page.getValue("spnMerchantDetail" + j, true) == arrSplit[3])
                    {
                        boolValid = arm_page.validateMerchantValue("txtMerchantDetail" + j, "txt", arrSplit[3], arrSplit[4], arrSplit[5]);
                        if (boolValid == false)
                        {
                            isValid += 1;
                            return;
                        }
                    }
                }
            }
            
            if (arm_page._calender == true)
            {
                if (arm_page.getValue("txtDateFrom", false) == "Click here to edit")
                {
                    arm_page.showAlert("Please enter application date from.", false);
                    arm_page.showFocus("txtDateFrom");
                    isValid += 1;
                }
                else
                {
                    if (arm_page.getValue("txtDateTo", false) == "Click here to edit")
                    {
                        arm_page.showAlert("Please enter application date to.", false);
                        arm_page.showFocus("txtDateTo");
                        isValid += 1;
                    }
                }
            }
            if (arm_page.getValue("txtDateFrom", false) != "Click here to edit" && arm_page.getValue("txtDateTo", false) != "Click here to edit")
            {
                month = arm_page.computeMonths(arm_page.formatdate(arm_page.getValue("txtDateFrom", false)), arm_page.formatdate(arm_page.getValue("txtDateTo", false)));
                var valid = /^\d+$/.test(month);
                if (valid == false || month == "0")
                {
                    arm_page.showAlert("Invalid date supplied. Please try again.", false);
                    arm_page.validateMerchant();
                    isValid += 1;
                }
            }
            
            if (isValid == 0)
            {
                arm_page.showControl("divQwerty", false);
                arm_page.hidePanel("divMerchantType");
                arm_page.showMerchantInformation();
            }
        }
    }
    
    this.submitLetter = function(letter)
    {
        var oldvalue = arm_page.getValue(arm_page._qwerty, false);
        
        switch (letter)
        {
            case "backspace":
                oldvalue = oldvalue.substring(0, oldvalue.length - 1);
                break;
            case "space":
                oldvalue = oldvalue + " ";
                break;
            case "clear":
                oldvalue = "";
                break;
        }
        if (letter == "space" || letter == "clear" || letter == "backspace")
        {
            arm_page.enterValue(oldvalue, arm_page._qwerty, false);
        }
        else
        {
            arm_page.enterValue(oldvalue + letter, arm_page._qwerty, false);
        }
    }
    
    this.checkCalendar = function(element)
    {
        if (arm_page.getValue(element, false) == "Click here to edit")
        {
            arm_page.removeValue(element, false);
        }
    }
    
    this.submitRemittanceMerchant = function(isremittance)
    {
        arm_page.enterValue("", "spnTargetCurrency", true);
        arm_page.removeValue("spnMerchant", true);
        if (arm_page.getValue("lblRemitterID", false) != "")
        {
            if (isremittance == false)
            {
                arm_page.edNumbers(false);
                arm_page.enterValue("Please select your merchant:", "spnMerchantSelect", true);
                arm_page._isSSS = false;
                arm_page.showLoading("Retrieving Merchant Information... Please wait", false);
                arm_page._remittanceType = "merchant";
                arm_page.enterValue("10", "lblPageID", false);
                arm_page.clearDDL("ddlMerchant");
                arm_page.getMerchant();
                //arm_page.enterValue(arm_page.getValue("divLetters", true), "spnMerchant", true);
                //arm_page.removeValue("spnMerchantCount", true);
            }
            else
            {
                arm_page.enterValue("1", "lblPageID", false);
                arm_page._remittanceType = "remittance";
            }
            arm_page.edBackContinue(false, false);
            arm_page.submitBackContinue(arm_page.getPageID(true), arm_page._nextpage);
            arm_page.edBackContinue(true, true);
        }
        else
        {
            arm_page.showAlert("Please wait while we retrieve your information.", false);
        }
    }
    
    // **************************************************************************************** //
    // *** Asynchoronous Callback AJAX
    // **************************************************************************************** //
    this.writeLog = function(description, type)
    {
        IRSWorld.Main.ARMMain.writeLog(description, type);
    }
    
    this.writeLogger = function(remittance, tablename, key)
    {
        IRSWorld.Main.ARMMain.writeLogger(remittance, tablename, key);
    }
    
    this.getMerchantLookup = function(merchantname)
    {
        return IRSWorld.Main.ARMMain.getMerchantLookup(merchantname, this.getMerchantLookup_callback);
    }
    
    this.getMerchantLookup_callback = function(result)
    {
        var response = result.value;
        var arrSplit = "";
        var count = 0;
        if (response != null && response != "" && response != undefined)
        {
            for (var i = 0; i <= (response.Rows.length - 1); i++)
            {
                arrSplit = response.Rows[i]["FldCode"].split("|");
                if (arrSplit[2] == arm_page._merchantSubProductCode)
                {
                    count = i;
                    arm_page._arrOBPS = response.Rows[count]["FldCode"].split("|");
                    arm_page.tblOBPSLookup = response;
                    arm_page._lookupField = arm_page._arrOBPS[0];
                    arm_page.getLookup();
                }
            }
        }
        if (arm_page._merchantName != "")
        {
            arm_page.getMerchantInfo(arm_page._merchantName, arm_page._merchantSubProductCode);
        }
    }
    
    this.getMerchantInfo = function(merchantname, productcode)
    {
        return IRSWorld.Main.ARMMain.getMerchantInfo(merchantname, productcode, this.getMerchantInfo_callback);
    }
    
    this.getMerchantInfo_callback = function(result)
    {
        var response = result.value;
        var ddl = 0;
        var dt = 0;
        var txt = 0;
        
        if (response != null && response != "" && response != undefined)
        {
            arm_page._calender = false;
            arm_page._arrMerchantValidation = new Array();
            for (var i = 0; i <= (response.Rows.length - 1); i++)
            {
                arm_page._arrMerchantValidation[i] = response.Rows[i]["MerchantInfo"];
                switch (response.Rows[i]["ValidationSeq"])
                {
                    case 1:
                        ddl = ddl + 1;
                        arm_page.enterValue(response.Rows[i]["FieldLabel"], "spnMerchantDDL" + ddl, true);
                        (response.Rows[i]["IsMandatory"] == 1) ? arm_page.enterValue("*", "spnMerchantTypeAsterisk" + ddl, true) : arm_page.enterValue("", "spnMerchantTypeAsterisk" + ddl, true);
                        break;
                    case 2:
                        dt = dt + 1;
                        arm_page.showControl("divCalendar", true);
                        if (response.Rows[i]["IsMandatory"] == 1) 
                        {
                            arm_page._calender = true;
                            arm_page.enterValue("*", "spnDateAsterisk" + dt, true)
                        }
                        else
                        {
                            arm_page._calender = false;
                            arm_page.enterValue("", "spnDateAsterisk" + dt, true);
                        }
                        break;
                    case 3:
                        txt = txt + 1;
                        arm_page.enterValue(response.Rows[i]["FieldLabel"], "spnMerchantDetail" + txt, true);
                        (response.Rows[i]["IsMandatory"] == 1) ? arm_page.enterValue("*", "spnMerchantDetailAsterisk" + txt, true) : arm_page.enterValue("", "spnMerchantDetailAsterisk" + txt, true);
                        break;
                }
            }
            
            for (var j = 1; j <= ddl; j++)
            {
                arm_page.showControl("divMerchantDDL" + j, true);
            }
            for (var k = 1; k <= txt; k++)
            {
                arm_page.showControl("divMerchantDesc" + k, true);
            }
            arm_page.showPanel("divMerchantType");
            arm_page.showLoading("", true);
        }
        else
        {
            arm_page.showAlert("There was a problem retrieving billers mandatory fields. Please report to the nearest PNB branch.", true);
        }
    }
    
    this.setMerchantORNo = function()
    {
        return IRSWorld.Main.ARMMain.setMerchantORNo(arm_page._merchantName, arm_page._merchantSubProductCode, this.setMerchantORNo_callback);
    }
    
    this.setMerchantORNo_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            arm_page._merchantORNo = response;
        }
    }
    
    this.getMerchant = function()
    {
        arm_page.clearDDL("ddlMerchant");
        arm_page.edButtons("ddlMerchant", true);
        return IRSWorld.Main.ARMMain.getMerchants(arm_page.getValue("lblRemitterID"), this.getMerchant_callback);
    }
    
    this.getMerchant_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            arm_page.fillDDL(response, "ddlMerchant", "MerchantCode|MemberIDNo|MerchantProductCodeDesc", "CIFID|Currency|AccountSeqID|MemberIDNo|MerchantProductCode|SEQ_ID", "lblMerchant", ", ");
            arm_page.edButtons("ddlMerchant", false);
        }
        else
        {
            arm_page.showLoading("", true);
            arm_page.showAlert("There is no merchant maintained for this remitter. Please go to the nearest PNB branch to enroll.", false);
        }
        arm_page.showLoading("", true);
    }
    
    this.getLookup = function()
    {
        return IRSWorld.Main.ARMMain.getLookup(arm_page._lookupField, this.getLookup_callback);
    }
    
    this.getLookup_callback = function(result)
    {
        var response = result.value;
        var arrSplit = "";
        if (response != null && response != "" && response != undefined)
        {
            switch (response.Rows[0]["FldGroup"])
            {
                case "Currency":
                    arm_page.tblCurrency = response;
                    break;
                case "ProductCode":
                    arm_page.tblProductCode = response;
                    break;
                case "ImplementationType":
                    arm_page.tblImplementationCode = response;
                    break;
                case "Computation":
                    arm_page.tblComputationLevel = response;
                    break;
                case "RemitMethod":
                    arm_page.tblRemitMethod = response;
                    break;
                case "PayMode":
                    arm_page.tblPayMode = response;
                    arm_page.enterValue(arm_page.findValue(response, "DCC", "FldCode", "FldValue"), "lblPayMode", false);
                case "ReceiptMode":
                    arm_page.tblReceiptMode = response;
                    arm_page._receiptMode = arm_page.findValue(response, "PNB Automated Remittance Machine", "FldDescription", "FldValue");
                    if (arm_page._receiptMode == "" || arm_page._receiptMode == undefined || arm_page._receiptMode == null)
                    {
                        arm_page._receiptMode = "4";
                    }
                    break;
                default:
                    if (arm_page._remittanceType == "merchant")
                    {
                        arrSplit = response.Rows[0]["FldCode"].split("|");
                        if (arrSplit.length == 1)
                        {
                            arrSplit[1] = 1;
                        }
                        arm_page.fillDDL(response, "ddlMerchantType" + arrSplit[1], "FldDescription", "FldValue", "lblMerchantType" + arrSplit[1], ", ");
                        arm_page.edButtons("ddlMerchantType" + arrSplit[1], false);
                    }
                    break;
            }
            arm_page._lookupField = "";
            arm_page.showLoading("", true);
        }
    }
    
    this.getGFCAggregate = function()
    {
        var beneficiary = arm_page.getDDLValue("ddlBeneficiary", false).split(",");
        var beneficiary_value = arm_page.getDDLValue("ddlBeneficiary", true).split("|");
        if (beneficiary[2] != null && beneficiary[2] != "" && beneficiary[2] != undefined)
        {
            if (arm_page._arrCalc[28] == null || arm_page._arrCalc[28] == .00)
            {
                arm_page.getCharges();
            }
            return IRSWorld.Main.ARMMain.getGFCAggregate(beneficiary_value[0], beneficiary[2], beneficiary_value[4], arm_page._arrCalc[28], this.getGFCAggregate_callback);
        }
    }
    
    this.getGFCAggregate_callback = function(result)
    {
        var response = result.value;
        if (response == true)
        {
            arm_page._isGFCLimit = false;
        }
        else
        {
            arm_page._isGFCLimit = true;
        }
    }
    
    this.getExchangeRate = function()
    {
        var response = IRSWorld.Main.ARMMain.getExchangeRate().value;
        if (response != null && response != "" && response != undefined)
        {
            arm_page._exchangeRate = arm_page.getValue("lblSourceCurrencyShortCode", false) + "1 to " + arm_page.getValue("lblTargetCurrencyShortCode", false) + response;
            arm_page.enterValue(arm_page._exchangeRate, "spnExchangeRate", true);
            arm_page.writeLog("GET EXCHANGE RATE: [Rate=" + arm_page._exchangeRate + "]", "EXCHANGERATE");
        }
        else
        {
            arm_page.showAlert("There is no exchange rate defined. Please report to the nearest PNB branch. \n\r\n\r Kiosk will close now.", true);
        }
        arm_page.showLoading("", true);
    }
    
    this.getSourceCurrency = function()
    {
        return IRSWorld.Main.ARMMain.getSourceCurrency(this.getSourceCurrency_callback);
    }

    this.getSourceCurrency_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            arm_page._remittingCurrency = response;
        }
        arm_page.showLoading("", true);
    }
    
    this.getSourceCurrencyFldCode = function()
    {
        return IRSWorld.Main.ARMMain.getSourceCurrencyFldCode(this.getSourceCurrencyFldCode_callback);
    }

    this.getSourceCurrencyFldCode_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            arm_page._remittingCurrencyCode = response;
        }
        arm_page.showLoading("", true);
    }
    
    this.getSourceCurrencyCode = function()
    {
        var response = IRSWorld.Main.ARMMain.getSourceCurrencyCode().value;
        if (response != null && response != "" && response != undefined)
        {
            arm_page.enterValue(response, "lblSourceCurrencyShortCode", false);
        }
        arm_page.showLoading("", true);
    }

    this.getTargetCurrencyCode = function()
    {
        var response = IRSWorld.Main.ARMMain.getTargetCurrencyCode().value;
        if (response != null && response != "" && response != undefined)
        {
            arm_page.enterValue(response, "lblTargetCurrencyShortCode", false);
        }
        arm_page.showLoading("", true);
    }
    
    this.getBranchCode = function()
    {
        return IRSWorld.Main.ARMMain.getBranchCode(this.getBranchCode_callback);
    }

    this.getBranchCode_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            arm_page.enterValue(response, "lblBranchCode", false);
            arm_page.getTransactionDate();
            arm_page.getDefaults(response);
        }
        arm_page.showLoading("", true);
    }
    
    this.getTransactionDate = function()
    {
        return IRSWorld.Main.ARMMain.getTransactionDate(this.getTransactionDate_callback);
    }
    
    this.getTransactionDate_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            arm_page.enterValue(response, "lblTransactionDate", false);
        }
    }
    
    this.getDefaults = function(branchcode)
    {
        return IRSWorld.Main.ARMMain.getDefaults(branchcode, this.getDefaults_callback);
    }

    this.getDefaults_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            arm_page.tblDefaults = response;
        }
        arm_page.showLoading("", true);
    }
    
    this.getLocalTime = function()
    {
        return IRSWorld.Main.ARMMain.getLocalTime(this.getLocalTime_callback);
    }

    this.getLocalTime_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            arm_page.enterValue(response, "lblLocalTime", false);
        }
        arm_page.showLoading("", true);
    }
    
    this.getServerTime = function()
    {
        return IRSWorld.Main.ARMMain.getServerTime(this.getServerTime_callback);
    }

    this.getServerTime_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            arm_page.enterValue(response, "lblServerTime", false);
        }
        arm_page.showLoading("", true);
    }

    this.getARMLimit = function()
    {
        return IRSWorld.Main.ARMMain.getARMLimit(this.getARMLimit_callback);
    }

    this.getARMLimit_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            arm_page._ARMLimit = response;
        }
        arm_page.showLoading("", true);
    }
    
    this.getKioskCurrency = function()
    {
        return IRSWorld.Main.ARMMain.getKioskCurrency(this.getKioskCurrency_callback);
    }

    this.getKioskCurrency_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            arm_page.enterValue(response, "lblKioskCurrency", false);
        }
        arm_page.showLoading("", true);
    }
    
    this.getGFMC = function()
    {
        return IRSWorld.Main.ARMMain.getGFMC(this.getGFMC_callback);
    }

    this.getGFMC_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            arm_page.enterValue(response, "lblGFMCNo", false);
        }
        arm_page.showLoading("", true);
    }
    
    this.getGFMCRemitter = function()
    {
        return IRSWorld.Main.ARMMain.getGFMCRemitter(this.getGFMCRemitter_callback);
    }

    this.getGFMCRemitter_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            arm_page.enterValue(response, "spnRemitterName", true);
        }
        arm_page.showLoading("", true);
    }

    this.getRemitterID = function()
    {
        return IRSWorld.Main.ARMMain.getRemitterID(this.getRemitterID_callback);
    }

    this.getRemitterID_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            arm_page.enterValue(response, "lblRemitterID", false);
        }
        arm_page.showLoading("", true);
    }

    this.getRemitterCards = function()
    {
        return IRSWorld.Main.ARMMain.getRemitterCards(this.getRemitterCards_callback);
    }

    this.getRemitterCards_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            arm_page.tblCards = response;
            arm_page.concatString(response, "lblDBCards", "CardNo", "|");
        }
        arm_page.showLoading("", true);
    }

    this.getAcctSeqID = function(cardno)
    {
        arm_page.enterValue(arm_page.findValue(arm_page.tblCards, cardno, "CardNo", "RemAcctSeqID"), "lblRemAcctSeqID", false);
        arm_page.enterValue(arm_page.findValue(arm_page.tblCards, cardno, "CardNo", "CardTypeDesc"), "lblCardType", false);
    }
    
    this.getGFMCBeneficiary = function()
    {
        arm_page.edButtons("ddlBeneficiary", true);
        return IRSWorld.Main.ARMMain.getGFMCBeneficiary(this.getGFMCBeneficiary_callback);
    }

    this.getGFMCBeneficiary_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            arm_page.edButtons("ddlBeneficiary", false);
            arm_page.fillDDL(response, "ddlBeneficiary", "ProductName|BeneficiaryName|AccountNo", "CIFID|ProductCode|ImplementationCode|BenAcctSeqID|Currency", "lblBeneficiary", ", ");
        }
        arm_page.showLoading("", true);
    }
    
    this.getBeneficiaryAddress = function()
    {
        return IRSWorld.Main.ARMMain.getBeneficiaryAddress(arm_page.getValue("lblBeneficiaryID", false), this.getBeneficiaryAddress_callback);
    }

    this.getBeneficiaryAddress_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            arm_page.enterValue(response, "lblBeneficiaryAddress", false);
        }
        arm_page.showLoading("", true);
    }
    
    this.getTransactionPurpose = function()
    {
        return IRSWorld.Main.ARMMain.getTransactionPurpose(this.getTransactionPurpose_callback);
    }

    this.getTransactionPurpose_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            arm_page.fillDDL(response, "ddlPurpose", "FldDescription", "FldValue", "lblPurpose", ", ");
            if (arm_page._remittanceType == "merchant")
            {
                arm_page.findDDL("ddlPurpose", "Bills Payment", "lblPurpose");
                if (arm_page._nextpage == true)
                {
                    arm_page.showControl("btnContinue", false);
                    arm_page.edBackContinue(false, false);
                    arm_page.submitBackContinue(arm_page.getPageID(true), arm_page._nextpage);
                    arm_page.edBackContinue(true, true);
                }
                else
                {
                    arm_page.showControl("btnContinue", true);
                    arm_page.edBackContinue(false, false);
                    arm_page.submitBackContinue(arm_page.getPageID(false), arm_page._nextpage);
                    arm_page.edBackContinue(true, true);
                }
            }
        }
        arm_page.showLoading("", true);
    }
    
    this.getCharges = function(sourcecurrency)
    {
        var computationtype = "";
        ((arm_page._remittanceType == "remittance") ? computationtype = true : computationtype = false);
        var response = IRSWorld.Main.ARMMain.getCharges(arm_page.getValue("lblAmount", false), arm_page.getValue("lblBranchCode", false), arm_page.getValue("lblProductCode", false), arm_page.getValue("lblImplementationCode", false), String(sourcecurrency), arm_page.getValue("lblTargetCurrency", false), arm_page.getValue("lblChargeType", false), arm_page.getValue("lblPayMode", false), arm_page.getValue("lblComputationLevel", false), computationtype).value;
        if (response != null && response != "" && response != undefined && response != "NOCHARGE")
        {
            arm_page._calc = response;
            arm_page._arrCalc = response.split("|");
            arm_page.enterValue(arm_page._arrCalc[31], "lblCharges", false);
            arm_page.getChargesDefaults(arm_page._arrCalc[39]);
            if (arm_page._bCharge == true)
            {
                arm_page.showControl("btnContinue", false);
                arm_page.edBackContinue(false, false);
                arm_page.submitBackContinue(arm_page.getPageID(true), arm_page._nextpage);
            }
        }
        else
        {
            arm_page.showAlert("There is no applicable charge maintained. Please report to the nearest PNB branch. \n\r\n\r Please choose another beneficiary.", false);
        }
        arm_page.showLoading("", true);
    }
    
    this.getChargesDefaults = function(chargeid)
    {
        return IRSWorld.Main.ARMMain.getChargesDefaults(chargeid, this.getChargesDefaults_callback);
    }
    
    this.getChargesDefaults_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            arm_page.tblChargesDefault = response;
        }
        arm_page.showLoading("", true);
    }

    this.getTransactionSource = function()
    {
        return IRSWorld.Main.ARMMain.getTransactionSource(this.getTransactionSource_callback);
    }

    this.getTransactionSource_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            arm_page.fillDDL(response, "ddlTransactionSource", "FldDescription", "FldValue", "lblTransactionSource", ", ");
        }
        arm_page.showLoading("", true);
    }
    
    this.submitPaymentGateway = function()
    {
        var arrResponse = "";
        var boolOK = false;
        var amount = Number(parseFloat(arm_page.getValue("lblAmount", false).replace(",", "")).toFixed(2)) + Number(parseFloat(arm_page.getValue("lblCharges", false).replace(",", "")).toFixed(2));
        var response = IRSWorld.Main.ARMMain.submitPaymentGateway(arm_page.getValue("lblBranchCode", false), arm_page.getValue("lblRemitterID", false), arm_page.getValue("lblDCCard", false), arm_page.getValue("lblCardName", false), arm_page.getValue("lblExpiryDate", false), arm_page.getValue("lblSecurityCode", false), arm_page.getValue("lblCardType", false), arm_page.getValue("lblSourceCurrencyCode", false), String(amount), "Automated Remittance Transaction [Kiosk]").value;
        
        if (response != null && response != "" && response != undefined)
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
            */
            // *****************************************
            // *** FOR TESTING PURPOSES [REMOVE] - END
            // *****************************************
            if (arrResponse[0] == "OK")
            {
                arm_page.writeLog("SUCCESSFUL RESPONSE FROM PAYMENT GATEWAY: [AuthCode= " + arrResponse[1] + "][Status=" + arrResponse[2] + "][Payment Gateway Response=" + arrResponse[3] + "]", "PAYMENTGATEWAY");
                arm_page.getORNo();
                arm_page.enterValue(arrResponse[1], "lblAuthCode", false);
                arm_page.enterValue(arrResponse[2], "lblStatusDetail", false);
                arm_page.enterValue(arrResponse[3], "lblPGMessage", false);
                arm_page.edButtons("btnBack", true);
                arm_page.showControl("btnContinue", true);
                arm_page.showLoading("Retrieving Beneficiary Information... Please wait", false);
                arm_page.showTransaction();
                arm_page.showLoading("", true);
                return true;
            }
            else
            {
                arm_page.writeLog("ERROR RESPONSE FROM PAYMENT GATEWAY: [Payment Gateway Response=" + arrResponse[0] + "]", "PAYMENTGATEWAY");
                arm_page.showLoading("", true);
                arm_page.showAlert("The transaction received an error. " + arrResponse[0], false);
                arm_page.getPageID(false);
                arm_page.edBackContinue(false, false);
                arm_page.submitBackContinue(arm_page.getPageID(false), arm_page._nextpage);
                arm_page.edBackContinue(true, true);
                arm_page.showControl("btnBack", true);
                return false;
            }
        }
        else
        {
            arm_page.writeLog("ERROR RESPONSE FROM PAYMENT GATEWAY: [AuthCode= " + arrResponse[1] + "][Status=" + arrResponse[2] + "][Payment Gateway Response=" + arrResponse[3] + "]", "PAYMENTGATEWAY");
            arm_page.showLoading("", true);
            arm_page.showAlert("A problem occured while connecting/receiving information from the payment gateway. Please try again.", false);
            arm_page.getPageID(false);
            arm_page.edBackContinue(false, false);
            arm_page.submitBackContinue(arm_page.getPageID(false), arm_page._nextpage);
            arm_page.edBackContinue(true, true);
            arm_page.showControl("btnBack", true);
            return false;
        }
    }
    
    this.getORNo = function()
    {
        return IRSWorld.Main.ARMMain.getORNo(this.getORNo_callback);
    }
    
    this.getORNo_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            arm_page.enterValue(response, "lblORNo", false);
        }
    }
    
    this.submitRemittance = function()
    {
        IRSWorld.Main.ARMMain.submitRemittance(arm_page.createRemittance(), this.submitRemittance_callback);
    }

    this.submitRemittance_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            if (Number(response) >= 0)
            {
                arm_page.disablePage(9);
                arm_page.showLoading("Retrieving Transaction Information... Please wait", false);
                arm_page.getReferenceNo();
            }
            else
            {
                arm_page.showAlert("Transaction failed. Please report to the nearest PNB branch. \r\n\r\n Session will close now.", true);
            }
        }
        else
        {
            arm_page.showAlert("Transaction failed. Please report to the nearest PNB branch. \r\n\r\n Session will close now.", true);
        }
        arm_page.showLoading("", true);
    }
    
    this.submitOBPS = function(arrOBPS)
    {
        IRSWorld.Main.ARMMain.submitOBPS(arrOBPS, this.submitOBPS_callback);
    }

    this.submitOBPS_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            if (Number(response) >= 0)
            {
                return true;
            }
            else
            {
                arm_page.showAlert("Transaction failed. Please report to the nearest PNB branch. \r\n\r\n Session will close now.", true);
                return false;
            }
        }
        else
        {
            arm_page.showAlert("Transaction failed. Please report to the nearest PNB branch. \r\n\r\n Session will close now.", true);
            return false;
        }
        arm_page.showLoading("", true);
    }
    
    this.getReferenceNo = function()
    {
        return IRSWorld.Main.ARMMain.getReferenceNo(arm_page.getValue("lblORNo", false), arm_page.getValue("lblTransactionDate", false), this.getReferenceNo_callback);
    }

    this.getReferenceNo_callback = function(result)
    {
        var response = result.value;
        var refno = "";
        var txnseqid = "";
        if (response != null && response != "" && response != undefined)
        {
            _isremittance = true;
            arm_page.tblRemittance = response;
            txnseqid = arm_page.getTableValue(arm_page.tblRemittance, "SEQ_ID", 0);
            refno = arm_page.getTableValue(arm_page.tblRemittance, "ReferenceNo", 0);
            arm_page.enterValue(refno, "spnReferenceNo", true);
            arm_page.enterValue(txnseqid, "lblTransactionSEQID", false);
            arm_page.writeLogger(arm_page._auditRemittance, "TBL_REMITTANCE", txnseqid);
            arm_page.submitRemittanceCharges(txnseqid);
            arm_page.submitRemittanceCards(txnseqid, arm_page.getValue("lblRemAcctSeqID", false), arm_page.getValue("lblStatusDetail", false), arm_page.getValue("lblPGMessage", false), arm_page.getValue("lblAuthCode", false));
            arm_page.submitGFC(refno);
            if (arm_page._remittanceType == "merchant")
            {
                arm_page.submitExcessPayment(refno, arm_page.getValue("txtAmount", false), arm_page._arrCalc[37]);
                var arrMerchant = arm_page.getValue("lblMerchantName", false).split(","); //MerchantCode|MemberIDNo|MerchantProductCodeDesc
                var arrOBPS = new Array();
                var arrOBPSValues = refno + "|" + arrMerchant[0] + "|" + arm_page.getValue("lblMerchantProductCode", false) + "|";
                var select = "";
                var i = 0;
                
                if (arrMerchant[1] != "" && arrMerchant[1] != null && arrMerchant[1] != undefined)
                {
                    arrOBPS[i] = arrOBPSValues + "Member ID" + "|" + arrMerchant[1] + "|" + arm_page.getValue("lblMerchantSeqID", false);
                    i = i + 1;
                }
                if (arm_page.getValue("lblMerchantType1", false) != "" && arm_page.getValue("lblMerchantType1", false) != null && arm_page.getValue("lblMerchantType1", false) != undefined)
                {
                    select = document.getElementById("ddlMerchantType1");
                    arrOBPS[i] = arrOBPSValues + arm_page.getValue("spnMerchantDDL1", true) + "|" + select.options[select.selectedIndex].text + "|" + arm_page.getValue("lblMerchantSeqID", false);
                    i = i + 1;
                }
                if (arm_page.getValue("lblMerchantType2", false) != "" && arm_page.getValue("lblMerchantType2", false) != null && arm_page.getValue("lblMerchantType2", false) != undefined)
                {
                    select = document.getElementById("ddlMerchantType2");
                    arrOBPS[i] = arrOBPSValues + arm_page.getValue("spnMerchantDDL2", true) + "|" + select.options[select.selectedIndex].text + "|" + arm_page.getValue("lblMerchantSeqID", false);
                    i = i + 1;
                }
                if (arm_page.getValue("lblMerchantType3", false) != "" && arm_page.getValue("lblMerchantType3", false) != null && arm_page.getValue("lblMerchantType3", false) != undefined)
                {
                    select = document.getElementById("ddlMerchantType3");
                    arrOBPS[i] = arrOBPSValues + arm_page.getValue("spnMerchantDDL3", true) + "|" + select.options[select.selectedIndex].text + "|" + arm_page.getValue("lblMerchantSeqID", false);
                    i = i + 1;
                }
                if (arm_page.getValue("txtMerchantDetail1", false) != "" && arm_page.getValue("txtMerchantDetail1", false) != null && arm_page.getValue("txtMerchantDetail1", false) != undefined && arm_page.getValue("txtMerchantDetail1", false) != "Click here to edit")
                {
                    arrOBPS[i] = arrOBPSValues + arm_page.getValue("spnMerchantDetail1", true) + "|" + arm_page.getValue("txtMerchantDetail1", false) + "|" + arm_page.getValue("lblMerchantSeqID", false);
                    i = i + 1;
                }
                if (arm_page.getValue("txtMerchantDetail2", false) != "" && arm_page.getValue("txtMerchantDetail2", false) != null && arm_page.getValue("txtMerchantDetail2", false) != undefined && arm_page.getValue("txtMerchantDetail2", false) != "Click here to edit")
                {
                    arrOBPS[i] = arrOBPSValues + arm_page.getValue("spnMerchantDetail2", true) + "|" + arm_page.getValue("txtMerchantDetail2", false) + "|" + arm_page.getValue("lblMerchantSeqID", false);
                    i = i + 1;
                }
                if (arm_page.getValue("txtMerchantDetail3", false) != "" && arm_page.getValue("txtMerchantDetail3", false) != null && arm_page.getValue("txtMerchantDetail3", false) != undefined && arm_page.getValue("txtMerchantDetail3", false) != "Click here to edit")
                {
                    arrOBPS[i] = arrOBPSValues + arm_page.getValue("spnMerchantDetail3", true) + "|" + arm_page.getValue("txtMerchantDetail3", false) + "|" + arm_page.getValue("lblMerchantSeqID", false);
                    i = i + 1;
                }
                if (arm_page.getValue("txtDateFrom", false) != "" && arm_page.getValue("txtDateFrom", false) != null && arm_page.getValue("txtDateFrom", false) != undefined && arm_page.getValue("txtDateFrom", false) != "Click here to edit")
                {
                    arrOBPS[i] = arrOBPSValues + arm_page.getValue("spnDateFrom", true) + "|" + arm_page.getValue("txtDateFrom", false) + "|" + arm_page.getValue("lblMerchantSeqID", false);
                    i = i + 1;
                }
                if (arm_page.getValue("txtDateTo", false) != "" && arm_page.getValue("txtDateTo", false) != null && arm_page.getValue("txtDateTo", false) != undefined && arm_page.getValue("txtDateTo", false) != "Click here to edit")
                {
                    arrOBPS[i] = arrOBPSValues + arm_page.getValue("spnDateTo", true) + "|" + arm_page.getValue("txtDateTo", false) + "|" + arm_page.getValue("lblMerchantSeqID", false);
                    i = i + 1;
                }
                arrOBPS[i] = arrOBPSValues + "Target Amount (PHP)" + "|" + arm_page.getValue("txtAmount", false) + "|" + arm_page.getValue("lblMerchantSeqID", false);
                arm_page.submitOBPS(arrOBPS); //@ReferenceNo, @MerchantCode, @MerchantProductCode, @FieldLabel, @FieldValue, @MerSeqID
            }
        }
    }
    
    this.submitRemittanceCharges = function(seqid)
    {
        return IRSWorld.Main.ARMMain.submitRemittanceCharges(seqid, this.submitRemittanceCharges_callback);
    }
    
    this.submitRemittanceCharges_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            if (response != "1")
            {
                arm_page.showAlert("Transaction was successfully committed however error was encountered while saving charges. " + response + ". \r\n\r\n Please report to the nearest PNB branch.", false);
            }
        }
    }
    
    this.submitRemittanceCards = function(remittanceseqid, remitteracct, message, response, authcode)
    {
        return IRSWorld.Main.ARMMain.submitRemittanceCards(remittanceseqid, remitteracct, message, response, authcode, true, this.submitRemittanceCards_callback);
    }
    
    this.submitRemittanceCards_callback = function(result)
    {
        var response = result.value;
        if (response != "1")
        {
            arm_page.showAlert("Transaction was successfully committed however error was encountered while saving card information. " + response + ". \r\n\r\n Please report to the nearest PNB branch.", false);
        }
    }
    
    this.submitGFC = function(refno)
    {
        var beneficiary = arm_page.getDDLValue("ddlBeneficiary", false).split(",");
        var beneficiary_value = arm_page.getDDLValue("ddlBeneficiary", true).split("|");
        IRSWorld.Main.ARMMain.submitGFC(refno, beneficiary[2], beneficiary_value[4], arm_page._arrCalc[28]).value;
    }
    
    this.submitExcessPayment = function(refno, targetamount, actualamount)
    {
        return IRSWorld.Main.ARMMain.submitExcessPayment(refno, targetamount, actualamount, this.submitExcessPayment_callback);
    }
    
    this.submitExcessPayment_callback = function(result)
    {
        var response = result.value;
        if (response != "1")
        {
            arm_page.showAlert("Transaction was successfully committed however error was encountered while saving excess payment. " + response + ". \r\n\r\n Please report to the nearest PNB branch.", false);
        }
    }
    
    this.createRemittance = function()
    {
        var remittance = "";
        var orno = "";
        var remitMethod = "";
        if (arm_page._remittanceType == "remittance")
        {
            orno = arm_page.getValue("lblORNo", false);
        }
        else
        {
            if (arm_page._merchantORNo != "" && arm_page._merchantORNo != undefined)
            {
                orno = arm_page._merchantORNo;
                arm_page.enterValue(orno, "lblORNo", false);
            }
            else
            {
                orno = arm_page.getValue("lblORNo", false);
            }
        }
        remitMethod = arm_page.findValue(arm_page.tblRemitMethod, "ARM", "FldDescription", "FldValue");
                
        remittance = arm_page.getValue("lblGFMCNo", false) + "|";                                               //UserID
        remittance += remitMethod + "|";                                                                        //RemitMethod
        remittance += "NULL" + "|";                                                                             //RemitStationID
        remittance += arm_page.getValue("lblBranchCode", false) + "|";                                          //RemittingBranch
        remittance += "FALSE" + "|";                                                                            //ChargesLocally
        remittance += "NULL" + "|";                                                                             //ReferenceNo
        remittance += orno + "|";                                                                               //ExtlReferenceNo
        remittance += "15" + "|";                                                                               //Transtatus
        //remittance += (arm_page.tblChargesDefault != undefined ? arm_page.getTableValue(arm_page.tblChargesDefault, "DefaultStatus", 0) : "15") + "|"; //Transtatus
        remittance += arm_page.getValue("lblTransactionDate", false) + "|";                                     // TranxDate
        remittance += arm_page._arrCalc[39] + "|";                                                              //ProductChargesID
        remittance += arm_page.getValue("lblRemitterID", false) + "|";                                          //RemitterCIFID
        remittance += ((arm_page._remittanceType == "remittance") ? arm_page.getValue("lblBeneficiaryID", false) : arm_page.getValue("lblMerchantID", false)) + "|"; //BeneficiaryCIFID
        remittance += ((arm_page._remittanceType == "remittance") ? arm_page.getValue("lblBenAcctSeqID", false) : arm_page.getValue("lblMerchantAcctSeqID", false)) + "|"; //AccountSEQID
        remittance += arm_page._arrCalc[35] + "|";                                                              //RemittingCurrency
        remittance += arm_page._arrCalc[0] + "|";                                                               //LocalCurrency
        remittance += arm_page._arrCalc[24] + "|";                                                              //GrossRemAmount
        remittance += arm_page._arrCalc[27] + "|";                                                              //NetRemAmount
        remittance += arm_page._arrCalc[14] + "|";                                                              //NettoBeneCurrency
        remittance += arm_page._arrCalc[29] + "|";                                                              //NettoBeneAmount
        remittance += arm_page._arrCalc[28] + "|";                                                              //NettoBeneAmountPHP
        remittance += arm_page._arrCalc[31] + "|";                                                              //TotalLocalCharge
        remittance += arm_page.getValue("lblTransactionSource", false) + "|";                                   //TransactionSource
        remittance += arm_page._receiptMode + "|";                                                              //ReceiptMode
        remittance += arm_page.getValue("lblPurpose", false) + "|";                                             //TransactionPurpose
        remittance += arm_page.getTableValue(arm_page.tblDefaults, "SourceOfFunds", 0) + "|";                   //SourceofFunds
        remittance += "NULL" + "|";                                                                             //OtherPurpose
        remittance += "NULL" + "|";                                                                             //SpecialMessage
        remittance += "TRUE" + "|";                                                                             //Printed
        remittance += arm_page._arrCalc[7] + "|";                                                               //ForexRemittingToAccount
        remittance += arm_page._arrCalc[20] + "|";                                                              //ForexRemittingToLocal
        remittance += arm_page._arrCalc[21] + "|";                                                              //ForexRemittingToUSD
        remittance += arm_page._arrCalc[18] + "|";                                                              //ForexRemittingToPHP
        remittance += arm_page._arrCalc[16] + "|";                                                              //ForexRemittingToEUR
        remittance += arm_page._arrCalc[7] + "|";                                                               //ForexLocalToRemitting
        remittance += arm_page._arrCalc[10] + "|";                                                              //ForexLocalToUSD
        remittance += arm_page._arrCalc[4] + "|";                                                               //ForexLocalToPHP
        remittance += arm_page.getValue("lblLocalTime", false) + "|";                                           //DatePostedOverseas
        remittance += arm_page.getValue("lblLocalTime", false) + "|";                                           //DatePostedLocal
        remittance += arm_page.getValue("lblServerTime", false) + "|";                                          //DateImplementedOverseas
        remittance += arm_page.getValue("lblServerTime", false) + "|";                                          //DateImplementedLocal
        remittance += "0" + "|";                                                                                //BatchNo
        remittance += arm_page._arrCalc[38];                                                                    //Total_Backend_Charges
        
        arm_page._auditRemittance = "UserID~"                     + arm_page.getValue("lblGFMCNo", false) + "|";
        arm_page._auditRemittance += "RemitMethod~"               + remitMethod + "|";
        arm_page._auditRemittance += "RemitStationID~NULL|";
        arm_page._auditRemittance += "RemittingBranch~"           + arm_page.getValue("lblBranchCode", false) + "|";
        arm_page._auditRemittance += "ChargesLocally~FALSE|";
        arm_page._auditRemittance += "ReferenceNo~NULL|";
        arm_page._auditRemittance += "ExtlReferenceNo~"           + orno + "|";
        arm_page._auditRemittance += "Transtatus~"                + "15" + "|";
        arm_page._auditRemittance += "TranxDate~"                 + arm_page.getValue("lblTransactionDate", false) + "|";
        arm_page._auditRemittance += "ProductChargesID~"          + arm_page._arrCalc[39] + "|";
        arm_page._auditRemittance += "RemitterCIFID~"             + arm_page.getValue("lblRemitterID", false) + "|";
        arm_page._auditRemittance += "BeneficiaryCIFID~"          + ((arm_page._remittanceType == "remittance") ? arm_page.getValue("lblBeneficiaryID", false) : arm_page.getValue("lblMerchantID", false)) + "|";
        arm_page._auditRemittance += "AccountSEQID~"              + ((arm_page._remittanceType == "remittance") ? arm_page.getValue("lblBenAcctSeqID", false) : arm_page.getValue("lblMerchantAcctSeqID", false)) + "|";
        arm_page._auditRemittance += "RemittingCurrency~"         + arm_page._arrCalc[35] + "|";
        arm_page._auditRemittance += "LocalCurrency~"             + arm_page._arrCalc[0] + "|";
        arm_page._auditRemittance += "GrossRemAmount~"            + arm_page._arrCalc[24] + "|";
        arm_page._auditRemittance += "NetRemAmount~"              + arm_page._arrCalc[27] + "|";
        arm_page._auditRemittance += "NettoBeneCurrency~"         + arm_page._arrCalc[14] + "|";
        arm_page._auditRemittance += "NettoBeneAmount~"           + arm_page._arrCalc[29] + "|";
        arm_page._auditRemittance += "NettoBeneAmountPHP~"        + arm_page._arrCalc[28] + "|";
        arm_page._auditRemittance += "TotalLocalCharge~"          + arm_page._arrCalc[31] + "|";
        arm_page._auditRemittance += "TransactionSource~"         + arm_page.getValue("lblTransactionSource", false) + "|";
        arm_page._auditRemittance += "ReceiptMode~"               + arm_page._receiptMode + "|";
        arm_page._auditRemittance += "TransactionPurpose~"        + arm_page.getValue("lblPurpose", false) + "|";
        arm_page._auditRemittance += "SourceofFunds~"             + arm_page.getTableValue(arm_page.tblDefaults, "SourceOfFunds", 0) + "|";
        arm_page._auditRemittance += "OtherPurpose~NULL|";
        arm_page._auditRemittance += "SpecialMessage~NULL|";
        arm_page._auditRemittance += "Printed~TRUE|";
        arm_page._auditRemittance += "ForexRemittingToAccount~"   + arm_page._arrCalc[7] + "|";
        arm_page._auditRemittance += "ForexRemittingToLocal~"     + arm_page._arrCalc[20] + "|";
        arm_page._auditRemittance += "ForexRemittingToUSD~"       + arm_page._arrCalc[21] + "|";
        arm_page._auditRemittance += "ForexRemittingToPHP~"       + arm_page._arrCalc[18] + "|";
        arm_page._auditRemittance += "ForexRemittingToEUR~"       + arm_page._arrCalc[16] + "|";
        arm_page._auditRemittance += "ForexLocalToRemitting~"     + arm_page._arrCalc[7] + "|";
        arm_page._auditRemittance += "ForexLocalToUSD~"           + arm_page._arrCalc[10] + "|";
        arm_page._auditRemittance += "ForexLocalToPHP~"           + arm_page._arrCalc[4] + "|";
        arm_page._auditRemittance += "DatePostedOverseas~"        + arm_page.getValue("lblLocalTime", false) + "|";
        arm_page._auditRemittance += "DatePostedLocal~"           + arm_page.getValue("lblLocalTime", false) + "|";
        arm_page._auditRemittance += "DateImplementedOverseas~"   + arm_page.getValue("lblServerTime", false) + "|";
        arm_page._auditRemittance += "DateImplementedLocal~"      + arm_page.getValue("lblServerTime", false) + "|";
        arm_page._auditRemittance += "BatchNo~0|";
        arm_page._auditRemittance += "Total_Backend_Charges~"     + arm_page._arrCalc[38];
        return remittance;
    }
    
    // **************************************************************************************** //
    // *** Transaction Information
    // **************************************************************************************** //
    this.showTransaction = function()
    {
        var span = document.getElementById("spnTransaction");
        var spanHeader = document.getElementById("spnTransactionInfo");
        spanHeader.style.display = "block";
        span.innerHTML = "<b>"                                      + ((arm_page._remittanceType == "remittance") ? "Beneficiary " : "Merchant ") + "Information: </b> " + ((arm_page._remittanceType == "remittance") ? arm_page.getTransactionInformation("Beneficiary", 2) : arm_page.getTransactionInformation("Merchant", 1)) + "<br>";
        span.innerHTML += "<b>"                                     + ((arm_page._remittanceType == "remittance") ? "Beneficiary Address: </b> " + arm_page.getValue("lblBeneficiaryAddress", false) : "Merchant Address: </b> ") + "<br>";
        span.innerHTML += "<b>Mode of Payment: </b> "               + ((arm_page._remittanceType == "remittance") ? arm_page.getTransactionInformation("Beneficiary", 1) + " (" + arm_page.getTransactionInformation("Beneficiary", 3) : arm_page.getTransactionInformation("Merchant", 3) + " (" + arm_page.getTransactionInformation("Merchant", 2)) + ")<br><p>";
        span.innerHTML += "<b>Remitted Amount: </b> "               + arm_page.getValue("lblSourceCurrencyShortCode", false) + arm_page._arrCalc[27] + "<br>";
        span.innerHTML += "<b>Bank Charge: </b> "                   + arm_page.getValue("lblSourceCurrencyShortCode", false) + arm_page._arrCalc[31] + "<br>";
        span.innerHTML += "<b>Gross Amount: </b> "                  + arm_page.getValue("lblSourceCurrencyShortCode", false) + arm_page._arrCalc[24] + "<br>";
        span.innerHTML += "<b>Debit/Credit Card Number: </b> "      + arm_page.getValue("lblDCCard", false) + "<br>";
        span.innerHTML += "<b>Expiry Date: </b> "                   + arm_page.getValue("lblExpiryDate", false) + "<br><p>";
        span.innerHTML += "<b>Today's Exchange Rate: </b> "         + arm_page._exchangeRate + "<br>";
        if (arm_page._remittanceType == "remittance")
        {
            span.innerHTML += "<b>Total amount to be received by your beneficiary: " + "<u> " + arm_page.getValue("lblTargetCurrencyShortCode", false) + arm_page._arrCalc[29] + "</u></b><br><p>";
        }
        else
        {
            span.innerHTML += "<b>Total amount to be received by your merchant: " + "<u> " + arm_page.getValue("lblTargetCurrencyShortCode", false) + arm_page._arrCalc[28] + "</u></b><br><p>";
        }
        span.innerHTML += "<b>NOTE: PLEASE WRITE DOWN YOUR AUTHENTICATION CODE FOR YOUR REFERENCE</b><br>";
        span.innerHTML += "<b>*** Authentication Code: </b> "       + arm_page.getValue("lblAuthCode", false) + "<b> ***</b><br>";
        span.innerHTML += "<b>*** Payment Gateway Response: </b> "  + arm_page.getValue("lblStatusDetail", false) + "<b> ***</b><br><p>";
    }

    this.getBeneficiaryInformation = function(type)
    {
        var arrSplit = "";
        var selected = "";
        var select = document.getElementById("ddlBeneficiary");
        var returnvalue = "";
        
        selected = select.options[select.selectedIndex].text;
        arrSplit = selected.split(",");
        switch (type)
        {
            case 1:
                returnvalue = arrSplit[0]; //ProductName
                break;
            case 2:
                returnvalue = arrSplit[1]; //BeneficiaryName
                break;
            case 3:
                returnvalue = arrSplit[2]; //AccountNo
                break;
        }
        return arm_page.trim(returnvalue);
    }
    
    this.getMerchantInformation = function(type)
    {
        var arrSplit = "";
        var selected = "";
        var select = document.getElementById("ddlMerchant");
        var returnvalue = "";
        
        selected = select.options[select.selectedIndex].text;
        arrSplit = selected.split(",");
        switch (type)
        {
            case 1:
                returnvalue = arrSplit[0]; //MerchantCode
                break;
            case 2:
                returnvalue = arrSplit[1]; //MemberIDNo
                break;
            case 3:
                returnvalue = arrSplit[2]; //MerchantProductCodeDesc
                break;
        }
        return arm_page.trim(returnvalue);
    }

    this.getTransactionInformation = function(type, field)
    {
        switch (type)
        {
            case "Beneficiary":
                return arm_page.getBeneficiaryInformation(field);
                break;
            case "Merchant":
                return arm_page.getMerchantInformation(field);
                break;
        }
    }
    
    this.submitPrint = function(print)
    {
        if (print == true)
        {
            arm_page.showLoading("Please wait while we print your receipt...", false);
            arm_page.printReceipt();
        }
        else
        {
            arm_page.showLoading("", true);
            arm_page.edBackContinue(false, false);
            arm_page.submitBackContinue(arm_page.getPageID(true), arm_page._nextpage);
            arm_page.edBackContinue(true, true);
        }
    }
    
    this.printReceipt = function()
    {
        return IRSWorld.Main.ARMMain.printReceipt(arm_page.getValue("spnReferenceNo", true), this.printReceipt_callback);
    }
    
    this.printReceipt_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            var remittingcurrency = arm_page.getTableValue(response, "RemittingCurrency", 0);
            var nettobenecurrency = arm_page.getTableValue(response, "NettoBeneCurrency", 0);
            arm_page.enterValue(arm_page.getTableValue(response, "Branch", 0), "spnRBranch", true);
            arm_page.enterValue(arm_page.getTableValue(response, "Date", 0), "spnRTransactionDate", true);
            arm_page.enterValue(arm_page.getTableValue(response, "ReferenceNo", 0), "spnRReferenceNo", true);
            arm_page.enterValue(arm_page.getTableValue(response, "Remitter", 0), "spnRRemitter", true);
            arm_page.enterValue(arm_page.getTableValue(response, "Beneficiary", 0), "spnRBeneficiary", true);
            arm_page.enterValue(arm_page.getTableValue(response, "CardNo", 0), "spnRCardNumber", true);
            arm_page.enterValue(arm_page.getTableValue(response, "AuthorizationCode", 0), "spnRAuthorizationCode", true);
            arm_page.enterValue(remittingcurrency + arm_page.formatCurrency(arm_page.getTableValue(response, "RemittanceAmount", 0)), "spnRRemittanceAmount", true);
            arm_page.enterValue(remittingcurrency + arm_page.formatCurrency(arm_page.getTableValue(response, "ServiceFee", 0)), "spnRServiceFee", true);
            arm_page.enterValue(remittingcurrency + arm_page.formatCurrency(arm_page.getTableValue(response, "TotalAmount", 0)), "spnRTotalAmount", true);
            arm_page.enterValue(arm_page.formatCurrency(arm_page.getTableValue(response, "ExchangeRate", 0)), "spnRExchangeRate", true);
            arm_page.enterValue(nettobenecurrency + arm_page.formatCurrency(arm_page.getTableValue(response, "AmountToBeneficiary", 0)), "spnRAmountToBeneficiary", true);
            arm_page._stopclock = true;
            arm_page.printDiv("divReceipt");
            arm_page._stopclock = false;
            arm_page.showAlert("Please get your transaction receipt from the printer.", false);
            //window.open('../Reports/Pages/Report_Viewer.aspx' + response);
            arm_page.showLoading("", true);
            arm_page.edBackContinue(false, false);
            arm_page.submitBackContinue(arm_page.getPageID(true), arm_page._nextpage);
            arm_page.edBackContinue(true, true);
        }
        else
        {
            if (confirm("There was a problem printing your receipt. Would you like to re-try?"))
            {
                arm_page.printReciept();
            }
            else
            {
                arm_page.showAlert("Sorry for the inconvenience. We will email your receipt thru email. If not receive, please report to the nearest PNB branch.", false);
                arm_page.showLoading("", true);
                arm_page.edBackContinue(false, false);
                arm_page.submitBackContinue(arm_page.getPageID(true), arm_page._nextpage);
                arm_page.edBackContinue(true, true);
            }
        }
    }
    
    this.printDiv = function(divName)
    {
        var headstr = "<html><head><title></title></head><body>";
        var footstr = "</body>";
        var newstr = document.all.item(divName).innerHTML;
        var oldstr = document.body.innerHTML;
        document.body.innerHTML = headstr + newstr + footstr;
        window.print();
        document.body.innerHTML = oldstr;
    }
}