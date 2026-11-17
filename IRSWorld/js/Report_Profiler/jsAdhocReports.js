function close()
{
    var response = self.returnValue;
    close_frame_dialog(false);
}

function adhoc_reports()
{
    this.initializePage = function()
    {
        this.getReportDDL();
    }
    
    this.getReportDDL = function()
    {
        adhoc_reports.ajaxTimeout();
        return IRSWorld.Report_Profiler.AdhocReports.getReportDDL(this.getReportDDL_callback);
    }
    
    this.getReportDDL_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            adhoc_reports.fillDDL(response, "ddlAdhocReports", "FldDescription", "FldCode", "", "");
        }
    }
    
    this.generateReport = function()
    {
        //adhoc_reports.ajaxTimeout();
        //return IRSWorld.Report_Profiler.AdhocReports.getAdhocReports(adhoc_reports.getDDL("ddlAdhocReports", false), adhoc_reports.getDDL("ddlAdhocReports", true), this.generateReport_callback);
        if (document.getElementById("ddlAdhocReports").selectedIndex == 0)
        {
            adhoc_reports.showAlert("Please select a report name");
        }
        else
        {
            if (adhoc_reports.getValue("txtDateFrom", false) == "")
            {
                adhoc_reports.showAlert("Please enter a (date from) value");
            }
            else
            {
                if (adhoc_reports.getValue("txtDateTo", false) == "")
                {
                    adhoc_reports.showAlert("Please enter a (date to) value");
                }
                else
                {
                    var result;
                    result = IRSWorld.Report_Profiler.AdhocReports.getAdhocReports(adhoc_reports.getDDL("ddlAdhocReports", false), adhoc_reports.getDDL("ddlAdhocReports", true), adhoc_reports.getValue("txtDateFrom", false), adhoc_reports.getValue("txtDateTo", false));
                    var response = result.value;
                    if (response != null && response != "" && response != undefined)
                    {
                        if (response != "No record(s) found!")
                        {
                            show_frame_dialog("../Query/GRSQueryUtil.aspx?download_path=" + response, "close", "100%", "100%", "0");
                            close();
                        }
                        else
                        {
                            adhoc_reports.showAlert(response);
                        }
                    }
                }
            }
        }
    }
    
    /*
    this.generateReport_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            show_frame_dialog("../Query/GRSQueryUtil.aspx?download_path=" + response, "close", "100%", "100%", "0");
            close();
        }
    }
    */
    
    this.getValue = function(element, isspan)
    {
        return (isspan == true) ? document.getElementById(element).innerHTML : document.getElementById(element).value;
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
                        if (iRow.Rows[i][arrSplit[arr]] != null && adhoc_reports.trim(iRow.Rows[i][arrSplit[arr]]) != "")
                        {
                            arrValue = arrValue + iRow.Rows[i][arrSplit[arr]];
                        }
                    }
                    else
                    {
                        if (iRow.Rows[i][arrSplit[arr]] != null && adhoc_reports.trim(iRow.Rows[i][arrSplit[arr]]) != "")
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
            //adhoc_reports.getDDL(element, selected, true);
        }
    }
    
    this.getDDL = function(element, isValue)
    {
        var select = document.getElementById(element);
        var retval = "";
        retval = (isValue == true) ? select.options[select.selectedIndex].value : select.options[select.selectedIndex].text;
        return retval;
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

    this.ajaxTimeout = function()
    {
        AjaxPro.timeoutPeriod = 1000 * 1000;
        AjaxPro.onTimeout = function()
        {
            adhoc_reports.showAlert("THE SERVER STOPPED RESPONDING. PLEASE TRY AGAIN");
        }
    }

    this.showAlert = function(message)
    {
        (message != "") ? alert(message) : alert("An error occured while getting information. Please report to the nearest PNB branch.");
    }
}