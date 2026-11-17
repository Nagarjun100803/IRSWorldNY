/**********************************/
/*** DATE VALIDATE (DD/MM/YYYY) ***/
/**********************************/

var dtCh = "/";
var minYear=1900;
var maxYear=2100;

function isInteger(s)
{
	var i;
    for (i = 0; i < s.length; i++)
    {
        // Check that current character is number.
        var c = s.charAt(i);
        if (((c < "0") || (c > "9"))) return false;
    }
    // All characters are numbers.
    return true;
}

function stripCharsInBag(s, bag)
{
	var i;
    var returnString = "";
    // Search through string's characters one by one.
    // If character is not in bag, append to returnString.
    for (i = 0; i < s.length; i++)
    {
        var c = s.charAt(i);
        if (bag.indexOf(c) == -1) returnString += c;
    }
    return returnString;
}

function daysInFebruary (year)
{
	// February has 29 days in any year evenly divisible by four,
    // EXCEPT for centurial years which are not also divisible by 400.
    return (((year % 4 == 0) && ( (!(year % 100 == 0)) || (year % 400 == 0))) ? 29 : 28 );
}

function DaysArray(n) 
{
	for (var i = 1; i <= n; i++) 
	{
		this[i] = 31
		if (i==4 || i==6 || i==9 || i==11) {this[i] = 30}
		if (i==2) {this[i] = 29}
   } 
   return this
}

function isDate(dtStr)
{
    var dt = document.getElementById(dtStr).value;
    if (dt != null && dt != "")
    {
        var daysInMonth = DaysArray(12)
	    var pos1 = dt.indexOf(dtCh)
	    var pos2 = dt.indexOf(dtCh,pos1+1)
	    var strDay = dt.substring(0,pos1)
	    var strMonth = dt.substring(pos1+1,pos2)
	    var strYear = dt.substring(pos2+1)
	    strYr = strYear
    	
	    if (strDay.charAt(0)=="0" && strDay.length>1) strDay=strDay.substring(1)
	    if (strMonth.charAt(0)=="0" && strMonth.length>1) strMonth=strMonth.substring(1)
	    for (var i = 1; i <= 3; i++) 
	    {
		    if (strYr.charAt(0)=="0" && strYr.length>1) strYr=strYr.substring(1)
	    }
	    month = parseInt(strMonth)
	    day = parseInt(strDay)
	    year = parseInt(strYr)
	    if (pos1==-1 || pos2==-1)
	    {
		    alert("The date format should be : dd/mm/yyyy")
		    return false
	    }
	    if (strMonth.length<1 || month<1 || month>12){
		    alert("Please enter a valid month")
		    return false
	    }
	    if (strDay.length<1 || day<1 || day>31 || (month==2 && day>daysInFebruary(year)) || day > daysInMonth[month]){
		    alert("Please enter a valid day")
		    return false
	    }
	    if (strYear.length != 4 || year==0 || year<minYear || year>maxYear){
		    alert("Please enter a valid 4 digit year between "+minYear+" and "+maxYear)
		    return false
	    }
	    if (dt.indexOf(dtCh,pos2+1)!=-1 || isInteger(stripCharsInBag(dt, dtCh))==false){
		    alert("Please enter a valid date")
		    return false
	    }
        return true
    }
}

function close()
{
    var response = self.returnValue;
    close_frame_dialog(false);
}

/************************/
/*** AJAX GRID        ***/
/************************/
function execute_ajaxgridview_command(param)
{
    if (vip_page != null && vip_page != undefined)
    {
        vip_page.do_ajaxgridview(param);
    }
}

function execute_ajx_command(param)
{
    if (vip_page != null && vip_page != undefined)
    {
        vip_page.do_ajaxgridview_search(param);
    }
}

function vip_page()
{
    var _branchcode     = "";
    var _organization   = "";
    var _porganization  = "";
    var _seq_id         = "";

    /************************/
    /*** INITIALIZE PAGE  ***/
    /************************/

    this.initialize = function()
    {
        vip_page._branchcode    = "";
        vip_page._organization  = "";
        vip_page._porganization = "";
        vip_page.removeValue("lblSEQID", false);
        
        this.getBranchCode();
        this.getOrganization();
        this.getYear();
        this.getQuarter();
        this.getReports();
    }
    
    /************************/
    /*** COMMON FUNCTIONS ***/
    /************************/
    
    this.ajaxTimeout = function()
    {
        AjaxPro.timeoutPeriod = 1000 * 1000;
        AjaxPro.onTimeout = function()
        {
            vip_page.showAlert("THE SERVER STOPPED RESPONDING. PLEASE TRY AGAIN");
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
                        if (iRow.Rows[i][arrSplit[arr]] != null && vip_page.trim(iRow.Rows[i][arrSplit[arr]]) != "")
                        {
                            arrValue = arrValue + iRow.Rows[i][arrSplit[arr]];
                        }
                    }
                    else
                    {
                        if (iRow.Rows[i][arrSplit[arr]] != null && vip_page.trim(iRow.Rows[i][arrSplit[arr]]) != "")
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
                vip_page.getDDL(element, selected, true);
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
                        vip_page.getDDL(element, selected, true);
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
                        vip_page.getDDL(element, selected, true);
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
            vip_page.hidePanel("divPopProcess");
        }
        else
        {
            new common_method().to_show_loading_message('divLoadingMsg');
            document.getElementById("lblLoadingMsg").innerHTML = message;
            vip_page.showPanel("divPopProcess");
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
        vip_page.showControl(divName, true);
        
        div.style.top = (((document.documentElement.clientHeight / 2) + document.documentElement.scrollTop) - (div.offsetHeight / 2)) + "px";
        div.style.left = (((document.documentElement.clientWidth / 2) + document.documentElement.scrollLeft) - (div.offsetWidth / 2)) + "px";
    }

    this.hidePanel = function(divName)
    {
        vip_page.showControl(divName, false);
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
                        vip_page.selectedVIPCIF(command_param[1]);
                    }
                    else
                    {
                        vip_page.showAlert("Unable to select. Please refresh the page and try again.");
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
    
    this.load_grid_viewer_search = function()
    {
        this.process_grid_viewer_search("ajx_loading");
    }
    
    this.process_grid_viewer_search = function(parameter)
    {
        this.ajax_grid = new grid_selector();
        this.grid_ajax_query = "vsp_getVIP~";
        this.ajax_grid.open_selection_grid(this.grid_ajax_page, this.grid_ajax_query, 0, vip_page._seq_id); 
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
                    if (command_param[1] != '0')
                    {
                        vip_page.selectedCIF(command_param[1]);
                    }
                    else
                    {
                        vip_page.showAlert("Unable to select. Please try re-loading again.");
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
        vip_page.showPanel("divSearchGrid");
        var sql = "vsp_SearchCIF~" + 
                    vip_page._branchCode + "~" + 
                    vip_page.getValue("txtSRemitterID", false) + "~" + 
                    vip_page.getValue("txtSLastName", false) + "~" + 
                    vip_page.getValue("txtSFirstName", false) + "~" + 
                    vip_page.getValue("txtSMiddleName", false) + "~" +
                    "";

        this.grid = new grid_viewer();
        this.grid.sql_source        = sql;
        this.grid.div_container     = document.getElementById("divSearchGridTop");
        this.grid.datarow_column    = 0;
        this.grid.page_size         = 8;
        this.grid.grd_header_back_color = "#26afe5";
        this.grid.grd_header_fore_color = "#FFFFFF";
        this.grid.grd_customed_date = false;
        this.grid.process_ajax(parameter);
        this.grid                   = null;
        document.getElementById("divLoading").innerHTML = "";
    }
    
    /************************/
    /*** FUNCTIONS        ***/
    /************************/
    
    this.callSearchCIF = function()
    {
        document.getElementById("divSearchGridTop").innerHTML = "";
        document.getElementById("divSearchGridTop").innerHTML = "<IMG SRC = '../images/ajaxloader.gif'></IMG>&nbsp;Retrieving CIF Information. Please Wait.";
        vip_page.load_grid_viewer();
    }
    
    this.clearText = function()
    {
        vip_page.enterValue("NEW", "spnStatus", true);
        vip_page.removeValue("txtRemitterID", false);
        vip_page.removeValue("txtLastName", false);
        vip_page.removeValue("txtFirstName", false);
        vip_page.removeValue("txtMiddleName", false);
        vip_page.removeValue("txtMobileNo", false);
        vip_page.removeValue("txtHKID", false);
        vip_page.removeValue("txtApplicationDate", false);
        vip_page.removeValue("txtExpiryDate", false);
        vip_page.roElement("txtLastName", false);
        vip_page.roElement("txtFirstName", false);
        vip_page.roElement("txtMiddleName", false);
        vip_page.roElement("txtMobileNo", false);
        vip_page.roElement("txtHKID", false);
        vip_page.roElement("txtApplicationDate", false);
        vip_page.roElement("txtExpiryDate", true);
        vip_page.removeValue("lblSEQID", false);
        document.getElementById("chkPrinted").checked = false;
        var select = document.getElementById("ddlOrganization");
        select.selectedIndex = -1;
        var select = document.getElementById("ddlPOrganization");
        select.selectedIndex = -1;
    }
    
    this.clearSearch = function()
    {
        vip_page.removeValue("txtSRemitterID", false);
        vip_page.removeValue("txtSLastName", false);
        vip_page.removeValue("txtSFirstName", false);
        vip_page.removeValue("txtSMiddleName", false);
    }
    
    this.addNewVIP = function()
    {
        vip_page.clearText();
        vip_page.enterValue("", "txtRemitterID", false);
        vip_page.enterValue("NEW", "spnStatus", true);
        vip_page.enterValue(vip_page.getValue("txtSLastName", false), "txtLastName", false);
        vip_page.enterValue(vip_page.getValue("txtSFirstName", false), "txtFirstName", false);
        vip_page.enterValue(vip_page.getValue("txtSMiddleName", false), "txtMiddleName", false);
        vip_page.clearSearch();
        vip_page.showDataEntry(true);
        vip_page.allowEdit(true);
    }
    
    this.showSearch = function()
    {
        vip_page.clearSearch();
        vip_page.showControl("divSearch", true);
        vip_page.showControl("divDataEntry", false);
        vip_page.showControl("divPrint", false);
        vip_page.showControl("divReport", false);
        vip_page.hidePanel("divSearchGrid");
    }
    
    this.showDataEntry = function(isAdd)
    {
        vip_page.showControl("divSearch", false);
        vip_page.showControl("divDataEntry", true);
        if (isAdd)
        {
            vip_page.showControl("divButtonsAdd", true);
            vip_page.showControl("divButtonsUpdate", false);
            vip_page.showControl("trPrinted", false);
        }
        else
        {
            vip_page.showControl("divButtonsAdd", false);
            vip_page.showControl("divButtonsUpdate", true);
            vip_page.showControl("trPrinted", true);
        }
        vip_page.showControl("divPrint", false);
        vip_page.showControl("divReport", false);
    }
    
    this.showPrint = function()
    {
        vip_page.clearText();
        vip_page.showControl("divSearch", false);
        vip_page.showControl("divDataEntry", false);
        vip_page.showControl("divPrint", true);
        vip_page.showControl("divReport", false);
    }
    
    this.showReport = function()
    {
        vip_page.clearText();
        vip_page.showControl("divSearch", false);
        vip_page.showControl("divDataEntry", false);
        vip_page.showControl("divPrint", false);
        vip_page.showControl("divReport", true);
    }
    
    this.allowEdit = function(isadd)
    {
        if (isadd)
        {
            vip_page.roElement("txtLastName", false);
            vip_page.roElement("txtFirstName", false);
            vip_page.roElement("txtMiddleName", false);
        }
        else
        {
            vip_page.roElement("txtLastName", true);
            vip_page.roElement("txtFirstName", true);
            vip_page.roElement("txtMiddleName", true);
        }
        vip_page.roElement("txtMobileNo", false);
        vip_page.roElement("txtHKID", false);
        vip_page.roElement("txtApplicationDate", false);
        vip_page.roElement("txtExpiryDate", true);
    }
    
    this.getDDLAjax = function(ddlname)
    {
        switch (ddlname)
        {
            case "ddlOrganization":
                if (document.getElementById(ddlname).selectedIndex > 0)
                {
                    vip_page._organization = vip_page.getDDLValue(ddlname, true);
                    break;
                }
                
            case "ddlPOrganization":
                if (document.getElementById(ddlname).selectedIndex > 0)
                {
                    vip_page._porganization = vip_page.getDDLValue(ddlname, true);
                    break;
                }
                
            case "ddlReport":
                if (document.getElementById(ddlname).selectedIndex > 0)
                {
                    switch (vip_page.getDDLValue(ddlname, true))
                    {
                        case "1":
                            vip_page.showControl("trDate", true);
                            vip_page.showControl("trQuarter", false);
                            vip_page.edElement("btnGenerateReport", false);
                            break;
                            
                        case "2":
                            vip_page.showControl("trDate", false);
                            vip_page.showControl("trQuarter", true);
                            vip_page.edElement("btnGenerateReport", false);
                            break;
                    }
                }
                else
                {
                    vip_page.showControl("trDate", false);
                    vip_page.showControl("trQuarter", false);
                    vip_page.edElement("btnGenerateReport", true);
                }
                break;
        }
    }
    
    this.selectedCIF = function(param)
    {
        var arrSplit = param.split("~");
        vip_page.enterValue("Existing", "spnStatus", true);
        vip_page.enterValue(arrSplit[1], "txtRemitterID", false);
        vip_page.enterValue(arrSplit[2], "txtLastName", false);
        vip_page.enterValue(arrSplit[3], "txtFirstName", false);
        vip_page.enterValue(arrSplit[4], "txtMiddleName", false);
        vip_page.enterValue(arrSplit[5], "txtMobileNo", false);
        vip_page.enterValue(arrSplit[6], "txtHKID", false);
        vip_page.allowEdit(false);
        vip_page.showControl("divSearchGrid", true);
        vip_page.showDataEntry(true);
        vip_page.hidePanel("divSearchGrid");
    }
    
    this.selectedVIPCIF = function(param)
    {
        var arrSplit = param.split("~");
        vip_page.enterValue("Existing", "spnStatus", true);
        vip_page.enterValue(arrSplit[0], "lblSEQID", false);
        vip_page.enterValue(arrSplit[1], "txtRemitterID", false);
        vip_page.enterValue(arrSplit[2], "txtLastName", false);
        vip_page.enterValue(arrSplit[3], "txtFirstName", false);
        vip_page.enterValue(arrSplit[4], "txtMiddleName", false);
        vip_page.enterValue(arrSplit[5], "txtMobileNo", false);
        vip_page.enterValue(arrSplit[6], "txtHKID", false);
        vip_page.findDDL("ddlOrganization", arrSplit[7], "", false);
        if (arrSplit[8] == "1")
        {
            document.getElementById("chkPrinted").checked = true;
        }
        else
        {
            document.getElementById("chkPrinted").checked = false;
        }
        vip_page.enterValue(vip_page.setDate(arrSplit[9], "ddmmyyyy", "/", false, false), "txtApplicationDate", false);
        vip_page.enterValue(vip_page.setDate(arrSplit[10], "ddmmyyyy", "/", false, false), "txtExpiryDate", false);
        vip_page.allowEdit(false);
        vip_page.showControl("divSearchGrid", false);
        vip_page.showDataEntry(false);
        vip_page.hidePanel("divSearchGrid");
    }
    
    this.validatePage = function()
    {
        if (vip_page.getValue("txtLastName", false) == "")
        {
            vip_page.showAlert("Please check client information. Last name is blank.");
            vip_page.showFocus("txtLastName");
            return false;
        }
        if (vip_page.getValue("txtFirstName", false) == "")
        {
            vip_page.showAlert("Please check client information. First name is blank.")
            vip_page.showFocus("txtFirstName");
            return false;
        }
        if (vip_page.getValue("txtHKID", false) == "")
        {
            vip_page.showAlert("Please check client information. HK ID is blank.")
            vip_page.showFocus("txtHKID");
            return false;
        }
        if (document.getElementById("ddlOrganization").selectedIndex <= 0)
        {
            vip_page.showAlert("Please select an organization.")
            vip_page.showFocus("ddlOrganization");
            return false;
        }
        if (vip_page.getValue("txtApplicationDate", false) == "")
        {
            vip_page.showAlert("Please check application date.")
            vip_page.showFocus("txtApplicationDate");
            return false;
        }
        return true;
    }
    
    this.validatePrint = function()
    {
        if (document.getElementById("ddlPOrganization").selectedIndex <= 0)
        {
            vip_page.showAlert("Please select an organization.")
            vip_page.showFocus("ddlPOrganization");
            return false;
        }
        if (vip_page.getValue("txtDateFrom", false) == "")
        {
            vip_page.showAlert("Please check DATE FROM field.")
            vip_page.showFocus("txtDateFrom");
            return false;
        }
        if (vip_page.getValue("txtDateTo", false) == "")
        {
            vip_page.showAlert("Please check DATE TO field.")
            vip_page.showFocus("txtDateTo");
            return false;
        }
        return true;
    }
    
    this.showLink = function(param)
    {
        switch (param)
        {
            case "Search":
                vip_page.showSearch();
                break;
                
            case "View":
                vip_page.load_grid_viewer_search();
                break;
                
            case "Extract":
                vip_page.showPrint();
                break;
                
            case "Report":
                vip_page.showReport();
                break;
        }
    }
    
    this.showBalloon = function(name)
    {
        switch (name)
        {
            case "HKID":
                return "e.g. [XX123456(7)]";
                break;
                
            case "ApplicationDate":
                return "e.g. DD/MM/YYYY";
                break;
        }
    }
    
    this.setDate = function(date, format, delimeter, addyear, isexpiry)
    {
        var arrSplit
        var year;
        var month;
        var day;
        var isDash;
        var ret;
        var bLeap;
        var currentyear;
        
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
        currentyear = year;
        
        if (addyear)
        {
            year = Number(year) + 1;
        }
        if (isexpiry)
        {
            if (Number(day) != 1) //validate if day == 1, should not subtract
            {
                day = Number(day) - 1;
                bLeap = true; //not really leap year but avoid putting another value to day
            }
            else
            {
                if (Number(month) != 1)
                {
                    month = Number(month) - 1;
                }
                else
                {
                    month = "12"; //if january 1 then current year should be the same
                    year = currentyear;
                }
                
                if (Number(year) % 4 == 0)
                {
                    if (month == 2)
                    {
                        day = "29"; //leap year
                        bLeap = true;
                    }
                }
            }
            if (Number(month) == 12 && Number(year) != Number(currentyear) && !bLeap)
            {
                day = "30"; //if december 31 then subtract 1
            }
            else
            {
                if (!bLeap)
                {
                    switch (Number(month))
                    {
                        case 1:
                        case 3:
                        case 5:
                        case 7:
                        case 8:
                        case 10:
                        case 12:
                            day = "31";
                            break;
                        case 2:
                            day = "28";
                            break;
                        default:
                            day = "30";
                            break;
                    }
                }
            }
        }
        
        if (month.toString().length == 1)
        {
            month = "0" + month;
        }
        if (day.toString().length == 1)
        {
            day = "0" + day;
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
    
    this.setExpiry = function()
    {
        var date = vip_page.getValue("txtApplicationDate", false);
        if (date != '' && date.length == 10)
        {
            var applicationDate;
            var expiryDate;
            
            applicationDate = vip_page.setDate(date, "ddmmyyyy", "/", false, false);
            expiryDate = vip_page.setDate(date, "ddmmyyyy", "/", true, true);
            vip_page.enterValue(applicationDate, "txtApplicationDate", false);
            vip_page.enterValue(expiryDate, "txtExpiryDate", false);
        }
    }
    
    this.extractToFile = function()
    {
        if (vip_page.validatePrint() == true)
        {
            vip_page.ajaxTimeout();
            vip_page.showLoading("Extracting to File. Please wait...", false);
            /*
            return IRSWorld.VIP.VIP.extractToFile(
                                vip_page._porganization, 
                                vip_page.getValue("txtDateFrom", false), 
                                vip_page.getValue("txtDateTo", false), 
                                this.extractToFile_callback);
            */
            var response = IRSWorld.VIP.VIP.extractToFile(
                                vip_page._porganization, 
                                vip_page.getValue("txtDateFrom", false), 
                                vip_page.getValue("txtDateTo", false)).value;
            
            if (response != null && response != "" && response != undefined)
            {
                if (response != "No record(s) found!")
                {
                    window.open("../Query/GRSQueryUtil.aspx?download_path=" + response);
                    //show_frame_dialog("../Query/GRSQueryUtil.aspx?download_path=" + response, "close", "100%", "100%", "0");
                    //close();
                }
                else
                {
                    vip_page.showLoading("", true);
                    vip_page.showAlert(response);
                }
            }
            vip_page.showLoading("", true);
        }
    }
    
    this.generateReport = function()
    {
        if (document.getElementById("ddlReport").selectedIndex > 0)
        {
            vip_page.ajaxTimeout();
            vip_page.showLoading("Generating Report. Please wait...", false);
            var reportName = vip_page.getDDLValue("ddlReport", false);
            var year = vip_page.getDDLValue("ddlYear", false);
            var quarter = vip_page.getDDLValue("ddlQuarter", true);
            var response = IRSWorld.VIP.VIP.generateReport(
                            reportName, 
                            vip_page.getValue("txtRDateFrom", false),
                            vip_page.getValue("txtRDateTo", false),
                            year, quarter).value;
            
            if (response != null && response != "" && response != undefined)
            {
                if (response != "No record(s) found!")
                {
                    show_frame_dialog("../Query/GRSQueryUtil.aspx?download_path=" + response, "close", "100%", "100%", "0");
                    close();
                }
                else
                {
                    vip_page.showLoading("", true);
                    vip_page.showAlert(response);
                }
            }
            vip_page.showLoading("", true);
        }
        else
        {
            vip_page.showAlert("Please select a report to generate.");
            vip_page.showFocus("ddlReport");
        }
    }
    
    /************************/
    /*** AJAX CALLBACK    ***/
    /************************/
    
    this.getBranchCode = function()
    {
        vip_page.ajaxTimeout();
        vip_page.showLoading("Retrieving Branch Code. Please wait...", false);
        return IRSWorld.VIP.VIP.getLocation(this.getBranchCode_callback);
    }
    
    this.getBranchCode_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            vip_page._branchCode = response
        }
        vip_page.showLoading("", true);
    }
    
    this.getOrganization = function()
    {
        vip_page.ajaxTimeout();
        vip_page.showLoading("Loading Organization. Please wait...", false);
        return IRSWorld.VIP.VIP.getOrganization(this.getOrganization_callback);
    }
    
    this.getOrganization_callback = function(result)
    {
        vip_page.clearDDL("ddlOrganization");
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            vip_page.fillDDL(response, "ddlOrganization", "FldDescription", "FldValue", "", "");
            vip_page.fillDDL(response, "ddlPOrganization", "FldDescription", "FldValue", "", "");
        }
        vip_page.showLoading("", true);
    }
    
    this.getYear = function()
    {
        vip_page.ajaxTimeout();
        vip_page.showLoading("Please wait...", false);
        return IRSWorld.VIP.VIP.getYear(this.getYear_callback);
    }
    
    this.getYear_callback = function(result)
    {
        vip_page.clearDDL("ddlYear");
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            vip_page.fillDDL(response, "ddlYear", "Year", "SEQID", "", "");
        }
        vip_page.showLoading("", true);
    }
    
    this.getQuarter = function()
    {
        vip_page.ajaxTimeout();
        vip_page.showLoading("Please wait...", false);
        return IRSWorld.VIP.VIP.getQuarter(this.getQuarter_callback);
    }
    
    this.getQuarter_callback = function(result)
    {
        vip_page.clearDDL("ddlQuarter");
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            vip_page.fillDDL(response, "ddlQuarter", "Quarter", "SEQID", "", "");
        }
        vip_page.showLoading("", true);
    }
    
    this.getReports = function()
    {
        vip_page.ajaxTimeout();
        vip_page.showLoading("Please wait...", false);
        return IRSWorld.VIP.VIP.getReports(this.getReports_callback);
    }
    
    this.getReports_callback = function(result)
    {
        vip_page.clearDDL("ddlReport");
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            vip_page.fillDDL(response, "ddlReport", "FldDescription", "FldCode", "", "");
        }
        vip_page.showLoading("", true);
    }
    
    this.searchCIF = function(seqid)
    {
        vip_page.ajaxTimeout();
        vip_page.showControl("divSearchGrid", false);
        vip_page.showLoading("Searching CIF. Please wait...", false);
        return IRSWorld.VIP.VIP.searchCIF("", "", "", "", seqid, this.searchCIF_callback);
    }
    
    this.searchCIF_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            vip_page.enterValue("Existing", "spnStatus", true);
            vip_page.enterValue(response.Rows[0]["CIFID"], "txtRemitterID", false);
            vip_page.enterValue(response.Rows[0]["LastName"], "txtLastName", false);
            vip_page.enterValue(response.Rows[0]["FirstName"], "txtFirstName", false);
            vip_page.enterValue(response.Rows[0]["MiddleName"], "txtMiddleName", false);
            vip_page.enterValue(response.Rows[0]["ContactNo"], "txtMobileNo", false);
            vip_page.enterValue(response.Rows[0]["IDNo"], "txtHKID", false);
            vip_page.roElement("txtLastName", true);
            vip_page.roElement("txtFirstName", true);
            vip_page.roElement("txtMiddleName", true);
            vip_page.roElement("txtMobileNo", true);
            vip_page.roElement("txtHKID", true);
            vip_page.showControl("divSearch", false);
            vip_page.showControl("divDataEntry", true);
            vip_page.showControl("divButtonsAdd", true);
            vip_page.showControl("divButtonsUpdate", false);
        }
        else
        {
            vip_page.showAlert("No record(s) found.");
        }
        vip_page.hidePanel("divSearchGrid");
        vip_page.showLoading("", true);
    }
    
    this.saveVIP = function()
    {
        if (vip_page.validatePage() == true)
        {
            vip_page.ajaxTimeout();
            vip_page.showLoading("Saving CIF Information. Please wait...", false);
            return IRSWorld.VIP.VIP.saveVIP(
                        vip_page.getValue("txtRemitterID", false),
                        vip_page.getValue("txtLastName", false),
                        vip_page.getValue("txtFirstName", false),
                        vip_page.getValue("txtMiddleName", false),
                        vip_page.getValue("txtMobileNo", false),
                        vip_page._organization,
                        vip_page.getDDLValue("ddlOrganization", false),
                        vip_page.getValue("txtHKID", false),
                        vip_page.getValue("txtApplicationDate", false),
                        vip_page.getValue("txtExpiryDate", false),
                        this.saveVIP_callback);
        }
    }
    
    this.saveVIP_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            if (response == "OK")
            {
                vip_page.showAlert("Successfully saved VIP client.");
                vip_page.clearText();
                vip_page.clearSearch();
            }
            else
            {
                vip_page.showAlert("There was a problem saving your CIF. Please try again.");
            }
        }
        vip_page.showLoading("", true);
    }
    
    this.updateVIP = function()
    {
        if (vip_page.getValue("lblSEQID", false) == "")
        {
            vip_page.showAlert("There is nothing to update.");
        }
        else
        {
            var isPrinted = "0";
            if (document.getElementById("chkPrinted").checked == true)
            {
                isPrinted = "1";
            }
            
            vip_page.ajaxTimeout();
            vip_page.showLoading("Updating VIP Client. Please wait...", false);
            return IRSWorld.VIP.VIP.updateVIP(
                                vip_page.getValue("lblSEQID", false), 
                                vip_page.getValue("txtRemitterID", false),
                                vip_page.getValue("txtMobileNo", false),
                                vip_page._organization,
                                vip_page.getDDLValue("ddlOrganization", false),
                                vip_page.getValue("txtHKID", false),
                                isPrinted, 
                                vip_page.getValue("txtApplicationDate", false),
                                vip_page.getValue("txtExpiryDate", false),
                                this.updateVIP_callback);
        }
    }
    
    this.updateVIP_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            if (response == "OK")
            {
                vip_page.showAlert("Successfully updated VIP client.");
                vip_page.clearText();
                vip_page.clearSearch();
            }
            else
            {
                vip_page.showAlert("There was a problem updating your CIF. Please try again.");
            }
        }
        vip_page.removeValue("lblSEQID", false);
        vip_page.showLoading("", true);
    }
    
    this.deleteVIP = function()
    {
        if (vip_page.getValue("lblSEQID", false) == "")
        {
            vip_page.showAlert("There is nothing to delete.");
        }
        else
        {
            vip_page.ajaxTimeout();
            vip_page.showLoading("Deleting VIP Client. Please wait...", false);
            return IRSWorld.VIP.VIP.deleteVIP(
                                vip_page.getValue("lblSEQID", false), 
                                this.deleteVIP_callback);
        }
    }
    
    this.deleteVIP_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            if (response == "OK")
            {
                vip_page.showAlert("Successfully deleted VIP client.");
                vip_page.clearText();
                vip_page.clearSearch();
            }
            else
            {
                vip_page.showAlert("There was a problem deleting your CIF. Please try again.");
            }
        }
        vip_page.removeValue("lblSEQID", false);
        vip_page.showLoading("", true);
    }
}