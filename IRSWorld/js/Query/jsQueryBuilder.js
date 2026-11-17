function query_builder()
{
    // **************************************************************************************** //
    // *** Common Functions and Methods
    // **************************************************************************************** //
    this.isNumberKey = function(evt)
    {
        var charCode = (evt.which) ? evt.which : event.keyCode
        if (charCode > 31 && (charCode < 48 || charCode > 57))
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
            document.getElementById(element).style.display = "block";
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
        query_builder.enterValue(returnvalue, element, false);
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
                        if (quert_builder.trim(iRow.Rows[i][sFieldValue[j]].toString()) == query_builder.trim(sMatchValue[j].toString()))
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
            
            select.options[0] = new Option("", "");
            for (var i = 0; i <= (iRow.Rows.length - 1); i++)
            {
                for (var arr = 0; arr <= (arrSplit.length - 1); arr++)
                {
                    if ((arrSplit.length - 1) == arr)
                    {
                        if (iRow.Rows[i][arrSplit[arr]] != null && query_builder.trim(iRow.Rows[i][arrSplit[arr]]) != "")
                        {
                            arrValue = arrValue + iRow.Rows[i][arrSplit[arr]];
                        }
                    }
                    else
                    {
                        if (iRow.Rows[i][arrSplit[arr]] != null && query_builder.trim(iRow.Rows[i][arrSplit[arr]]) != "")
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
            if (selected != "")
            {
                query_builder.getDDL(element, selected, true);
            }
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
                    query_builder.getDDL(element, selected, true);
                    break;
                }
            }
            else
            {
                if (select.options[i].text == findvalue)
                {
                    select.selectedIndex = i;
                    query_builder.getDDL(element, selected, true);
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
    
    this.fillList = function(table, element)
    {
        var select = document.getElementById(element);
        if (select.length == 0)
        {
            select.options[0] = new Option("", "");
            for (var i = 0; i <= (table.Rows.length - 1); i++)
            {
                select.options[i + 1] = new Option(table.Rows[i].Column1, table.Rows[i].Column1);
            }
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
    
    // **************************************************************************************** //
    // *** Page Functions and Methods
    // **************************************************************************************** //
    this.initializePage = function()
    {
        query_builder.getOverseasBranch();
        query_builder.getDomesticBranch();
        query_builder.getAgentBranch();
        query_builder.getRemitMethod();
        query_builder.getProductCode();
    }
    
    // **************************************************************************************** //
    // *** Asynchoronous Callback AJAX
    // **************************************************************************************** //
    this.getOverseasBranch = function()
    {
        return QueryBuilder.getOverseasBranch(this.getOverseasBranch_callback).value;
    }
    
    this.getOverseasBranch_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            query_builder.fillList(response, "lstOverseasBranch");
        }
    }
    
    this.getDomesticBranch = function()
    {
        return QueryBuilder.getDomesticBranch(this.getDomesticBranch_callback).value;
    }
    
    this.getDomesticBranch_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            query_builder.fillList(response, "lstDomesticBranch");
        }
    }
    
    this.getAgentBranch = function()
    {
        return QueryBuilder.getAgentBranch(this.getAgentBranch_callback).value;
    }
    
    this.getAgentBranch_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            query_builder.fillList(response, "lstAgentBranch");
        }
    }
    
    this.getRemitMethod = function()
    {
        return QueryBuilder.getRemitMethod(this.getRemitMethod_callback).value;
    }
    
    this.getRemitMethod_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            query_builder.fillDDL(response, "ddlRemitMethod", "FldDescription", "FldValue", "lblRemitMethod", "");
        }
    }
    
    this.getProductCode = function()
    {
        return QueryBuilder.getProductCode(this.getProductCode_callback).value;
    }
    
    this.getProductCode_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            query_builder.fillDDL(response, "ddlProductCode", "FldDescription", "FldValue", "lblProductCode", "");
        }
    }
}