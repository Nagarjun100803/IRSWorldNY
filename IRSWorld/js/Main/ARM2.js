function arm_page()
{
    // **************************************************************************************** //
    // *** Variables
    // **************************************************************************************** //
    var _GFCNo = "";
    var _GFCRemitterCIFID = "";
    var _GFCRemitter = "";
    var _GFCLocation = "";
    var _exchangeRate = "";
    
    // **************************************************************************************** //
    // *** Common Functions and Methods
    // **************************************************************************************** //
    this.getValue = function(element, isspan)
    {
        return (isspan == true) ? document.getElementById(element).innerHTML : document.getElementById(element).value;
    }
    
    this.enterValue = function(value, element, isspan)
    {
        (isspan == true) ? document.getElementById(element).innerHTML = value : document.getElementById(element).value = value;
    }
    
    // **************************************************************************************** //
    // *** Page Functions and Methods
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
            var timetype = "";
            
            if (hours > 11) amOrPm = "PM";
            if (hours > 12) hours = hours - 12;
            if (hours == 0) hours = 12;
            if (minutes <= 9) minutes = "0" + minutes;
            if (seconds <= 9) seconds = "0" + seconds;
            
            dispMonth = "<b>"+month +"</b>";
            dispTime = "<b>"+hours + ":" + minutes + ":" + seconds + " " + amOrPm+"</b>";
            if (amOrPm == "AM")
            {
                timetype = "Good Morning";
            }
            else
            {
                if (hours >= 6)
                {
                    timetype = "Good Evening";
                }
                else
                {
                    timetype = "Good Afternoon";
                }
            }
            arm_page.enterValue(dispTime, "spnClock", true);
            arm_page.enterValue(timetype, "spnTimeType", true);
            setTimeout("arm_page.clock(true)", 1000);
        }
    }
    
    this.initialize = function()
    {
        arm_page.clock(true);
        arm_page.getGFCRemitter("5921081689000157");
    }
    
    // **************************************************************************************** //
    // *** Asynchoronous Callback AJAX
    // **************************************************************************************** //
    this.getGFCRemitter = function(cardno)
    {
        return ARM.getGFCRemitter(cardno, this.getGFMCRemitter_callback).value;
    }

    this.getGFMCRemitter_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            var arrSplit = response.split("|"); //GFCNo, RemitterCIFID, RemitterName, GFCLocation
            arm_page._GFCNo = arrSplit[0];
            arm_page._GFCRemitterCIFID = arrSplit[1];
            arm_page._GFCRemitter = arrSplit[2];
            arm_page._GFCLocation = arrSplit[3];
            arm_page.enterValue(arm_page._GFCRemitter, "spnUser", true);
            arm_page.enterValue(arm_page._GFCNo, "spnGFCNo", true);
            arm_page.enterValue(arm_page._GFCLocation, "spnBranch", true);
        }
        else
        {
            //show alert
        }
    }
    
    this.getExchangeRate = function()
    {
        return ARM.getExchangeRate(this.getExchangeRate_callback).value;
    }

    this.getExchangeRate_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            arm_page._exchangeRate = response;
            arm_page.enterValue(arm_page._exchangeRate, "spnExchangeRate", true);
        }
        else
        {
            
        }
    }  
}