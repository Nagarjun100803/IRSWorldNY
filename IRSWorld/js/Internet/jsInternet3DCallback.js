// **************************************************************************************** //
// *** Internet 3D Callback Page
// **************************************************************************************** //
function internet3D_page()
{
    var _MD = "";
    var _PaRes = "";
    
    this.initializePage = function()
    {
        this.getMD();
    }
    
    this.getMD = function()
    {
        internet3D_page.ajaxTimeout();
        return IRSWorld.Internet.Internet3DCallback.getMD(this.getMD_callback);
    }

    this.getMD_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            internet3D_page._MD = "MD=" + response;
            internet3D_page.getPaRes();
        }
    }
    
    this.getPaRes = function()
    {
        internet3D_page.ajaxTimeout();
        return IRSWorld.Internet.Internet3DCallback.getPaRes(this.getPaRes_callback);
    }

    this.getPaRes_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            internet3D_page._PaRes = "PaRes=" + response;
            internet3D_page.submitPaymentGateway3D();
        }
    }
    
    this.submitPaymentGateway3D = function()
    {
        internet3D_page.ajaxTimeout();
        return IRSWorld.Internet.Internet3DCallback.submitPaymentGateway3D(internet3D_page.createRequest(), this.submitPaymentGateway3D_callback);
    }
    
    this.submitPaymentGateway3D_callback = function(result)
    {
        response = result.value;
        if (response != null && response != "" && response != undefined && response != "|||" && response != "||||")
        {
            parent.returnValue = response;
            eval('parent.internetClose();');
        }
        if (parent.document.getElementById('xdiv'))
        {
            parent.document.getElementById('xdiv').style.display='none';
            parent.document.forms[0].removeChild(parent.document.getElementById('xdiv'));
        }
        else if (document.getElementById('xdiv'))
        {
            document.getElementById('xdiv').style.display='none';
            document.forms[0].removeChild(document.getElementById('xdiv'));
        }
    }
    
    this.createRequest = function()
    {
        return internet3D_page._MD + "&" + internet3D_page._PaRes;
    }
    
    this.ajaxTimeout = function()
    {
        AjaxPro.timeoutPeriod = 1000 * 1000;
        AjaxPro.onTimeout = function()
        {
            alert("THE SERVER STOPPED RESPONDING. PLEASE TRY AGAIN");
        }
    }
}