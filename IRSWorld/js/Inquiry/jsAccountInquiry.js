function account_inquiry()
{
    var FCMethod = "";
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
        if (ishide == true)
        {
            document.getElementById("lblLoadingMsg").innerText = "";
            account_inquiry.hidePanel("divPopProcess");
        }
        else
        {
            new common_method().to_show_loading_message('divLoadingMsg');
            document.getElementById("lblLoadingMsg").innerText = message;
            account_inquiry.showPanel("divPopProcess");
        }
    }
    
    this.showAlert = function(message)
    {
        (message != "") ? alert(message) : alert("An error occured while getting information. Please report to the nearest PNB branch.");
    }
    
    this.inquireCard = function()
    {
        if (account_inquiry.trim(account_inquiry.getValue("txtCardNo", false)) == "")
        {
            account_inquiry.showAlert("Please enter a GFC card number.");
        }
        else
        {
            account_inquiry.getGFCtoCK(account_inquiry.getValue("txtCardNo", false));
        }
    }
    
    this.inquireAccount = function()
    {
        account_inquiry.FCMethod = 0;
        if (account_inquiry.isChecked("rbBalanceInquiry") == true)
        {
            account_inquiry.FCMethod = 1;
        }
        else
        {
            if (account_inquiry.isChecked("rbLastTransactionInquiry") == true)
            {
                account_inquiry.FCMethod = 2;
            }
            else
            {
                if (account_inquiry.isChecked("rbHoldInquiry") == true)
                {
                    account_inquiry.FCMethod = 3;
                }
                else
                {
                    if (account_inquiry.isChecked("rbFloatInquiry") == true)
                    {
                        account_inquiry.FCMethod = 4;
                    }
                    else
                    {
                        if (account_inquiry.isChecked("rbAccountStatusInquiry") == true)
                        {
                            account_inquiry.FCMethod = 5;
                        }
                    }
                }
            }
        }
        if (account_inquiry.FCMethod == 0)
        {
            account_inquiry.showAlert("Please select an inquiry method.");
        }
        else
        {
            if (account_inquiry.trim(account_inquiry.getValue("txtAccountNo", false)) == "")
            {
                account_inquiry.showAlert("Please enter an account number.");
            }
            else
            {
                if (account_inquiry.isChecked("chkOnline") == true)
                {
                    account_inquiry.getFC(account_inquiry.FCMethod, true);
                }
                else
                {
                    account_inquiry.getFC(account_inquiry.FCMethod, false);
                }
            }
        }
    }
    
    // **************************************************************************************** //
    // *** Asynchoronous Callback AJAX
    // **************************************************************************************** //
    this.getGFCtoCK = function(cardno)
    {
        account_inquiry.edButtons("btnGFCtoCK", true);
        account_inquiry.showLoading("Please wait...", false);
        return IRSWorld.Inquiry.AccountInquiry.getGFCtoCK(cardno, this.getGFCtoCK_callback);
    }
    
    this.getGFCtoCK_callback = function(result)
    {
        var response = result.value;
        account_inquiry.showLoading("", true);
        if (response != null && response != "" && response != undefined)
        {
            account_inquiry.showAlert(response);
        }
        else
        {
            account_inquiry.showAlert("There was no response from the remote host. Please try again later.");
        }
        account_inquiry.edButtons("btnGFCtoCK", false);
    }
    
    this.getFC = function(FCMethod, isOnline)
    {
        AjaxPro.timeoutPeriod = 1000 * 1000;
        AjaxPro.onTimeout = function()
        {
            account_inquiry.showAlert("THE SERVER STOPPED RESPONDING. PLEASE TRY AGAIN");
        }
        account_inquiry.edButtons("btnSubmit", true);
        account_inquiry.showLoading("Please wait...", false);
        return IRSWorld.Inquiry.AccountInquiry.getFC(account_inquiry.getValue("txtAccountNo", false), FCMethod, account_inquiry.getValue("txtTransactionCount", false), isOnline, this.getFC_callback);
    }
    
    this.getFC_callback = function(result)
    {
        var response = result.value;
        account_inquiry.showLoading("", true);
        if (response != null && response != "" && response != undefined)
        {
            account_inquiry.showAlert(response);
            if (account_inquiry.isChecked("chkOnline") == true)
            {
                if (confirm("Do you want to view and print?"))
                {
                    window.open('../Reports/Pages/Report_viewer.aspx?ACCOUNTINQUIRY=' + account_inquiry.FCMethod);
                }
            }
        }
        else
        {
            account_inquiry.showAlert("There was no response from the remote host. Please try again later.");
        }
        account_inquiry.edButtons("btnSubmit", false);
    }
    
    // **************************************************************************************** //
    // *** Panel
    // **************************************************************************************** //
    this.showPanel = function(divName)
    {
        var div = document.getElementById(divName);
        
        divmask = new maskpanel();
        divmask.show();
        account_inquiry.showControl(divName, true);
        
        div.style.top = (((document.documentElement.clientHeight / 2) + document.documentElement.scrollTop) - (div.offsetHeight / 2)) + "px";
        div.style.left = (((document.documentElement.clientWidth / 2) + document.documentElement.scrollLeft) - (div.offsetWidth / 2)) + "px";
    }

    this.hidePanel = function(divName)
    {
        account_inquiry.showControl(divName, false);
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
}