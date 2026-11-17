function ibft_key()
{
    this.ajaxTimeout = function()
    {
        AjaxPro.timeoutPeriod = 1000 * 1000;
        AjaxPro.onTimeout = function()
        {
            alert("THE SERVER STOPPED RESPONDING. PLEASE TRY AGAIN");
        }
    }
    
    this.showLoading = function(message, ishide)
    {
        if (ishide == true)
        {
            document.getElementById("lblLoadingMsg").innerHTML = "";
            ibft_key.hidePanel("divPopProcess");
        }
        else
        {
            new common_method().to_show_loading_message('divLoadingMsg');
            document.getElementById("lblLoadingMsg").innerHTML = message;
            ibft_key.showPanel("divPopProcess");
        }
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
        ibft_key.showControl(divName, true);
        
        div.style.top = (((document.documentElement.clientHeight / 2) + document.documentElement.scrollTop) - (div.offsetHeight / 2)) + "px";
        div.style.left = (((document.documentElement.clientWidth / 2) + document.documentElement.scrollLeft) - (div.offsetWidth / 2)) + "px";
    }

    this.hidePanel = function(divName)
    {
        ibft_key.showControl(divName, false);
        divmask.hide();
    }
    
    this.getValue = function(element, isspan)
    {
        return (isspan == true) ? document.getElementById(element).innerHTML : document.getElementById(element).value;
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
    
    this.isNumberKey = function(evt)
    {
        var charCode = (evt.which) ? evt.which : event.keyCode
        if (charCode > 31 && (charCode < 48 || charCode > 57) && (charCode < 65 || charCode > 70))
        return false;

        return true;
    }
    
    this.submitKey = function()
    {
        ibft_key.ajaxTimeout();
        ibft_key.showLoading("Keys are being saved. Please wait...", false);
        if (ibft_key.validate() == true)
        {
            return IRSWorld.BranchSetup.IBFTKey.submitKey(ibft_key.getValue("txtBusiness", false), ibft_key.getValue("txtSecurity", false), this.submitKey_callback);
        }
    }
    
    this.submitKey_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            if (response.toUpperCase() == "TRUE" || response == true)
            {
                alert("Successfully saved IBFT keys");
            }
            else
            {
                alert("Failed to save IBFT keys. Please try again");
            }
        }
        ibft_key.showLoading("", true);
    }
    
    this.validate = function()
    {
        if (ibft_key.getValue("txtBusiness", false).length != 8)
        {
            alert('Business key should be 8 alpha-numeric value. Please try again.');
            return false;
        }
        else
        {
            if (ibft_key.getValue("txtSecurity", false).length != 8)
            {
                alert('Security key should be 8 alpha-numeric value. Please try again.');
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}