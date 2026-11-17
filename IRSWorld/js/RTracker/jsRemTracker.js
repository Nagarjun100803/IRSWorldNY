function showpanel(divname)
{   
    var div = document.getElementById(divname);
    
    divmask = new maskpanel();
    divmask.show();
    
    document.getElementById(divname).style.display = "block";
    
    div.style.top = (((document.documentElement.clientHeight / 2) + document.documentElement.scrollTop) - (div.offsetHeight / 2)) + 'px';
    div.style.left = (((document.documentElement.clientWidth / 2) + document.documentElement.scrollLeft) - (div.offsetWidth / 2)) + 'px';
}

function hidepanel(divname)
{
    document.getElementById(divname).style.display = "none";
    divmask.hide();
} 

function remTracker()
{
    /*
        changeMode()        
            0  =   Search Mode
            1  =   Display Mode  
    */

    var icls = this;
    
    this.btnSubmit_click = function()
    {   
        if (document.getElementById("txtSearchTrxnNo").value != "")
        {
            showpanel('divPopProcess');
            
            AjaxPro.timeoutPeriod = 100 * 1000;
        
            AjaxPro.onTimeout = function()
            {
                alert("SESSION TIMEOUT");
                hidepanel('divPopProcess'); 
                return; 
            }        
            
            IRSWorld.RTracker.RemTracker.getDetails(document.getElementById("txtSearchTrxnNo").value, this.btnSubmit_click_callback);
        }
        else
        {
            alert("Please Enter Transaction Reference Number");
        }
    }
   
    this.btnSubmit_click_callback = function(response)
    {
         hidepanel('divPopProcess');
    
        if (response.error != null)
        {
            alert(response.error.Message);
        }
        else          
        {
            var ds = response.value
        
            document.getElementById("txtTrxnNo").innerText = document.getElementById("txtSearchTrxnNo").value;
            document.getElementById('txtDate').innerText = ds.Tables[0].Rows[0].Date.toString();
            document.getElementById('txtAmount').innerText = ds.Tables[0].Rows[0].Amount.toString();
            document.getElementById('txtCurr').innerText = ds.Tables[0].Rows[0].Curr.toString();               
            document.getElementById('txtStatus').innerText = ds.Tables[0].Rows[0].Status.toString();
           
            icls.changeMode(1);
        }
    }  
    
    this.btnReset_click = function()
    {
        this.changeMode(0);
    }
   
    //private Methods
   
    this.init = function()
    {        
        this.changeMode(0);
    }
     
   this.enable = function(oName)
    {
        var id = document.getElementById(oName);
        var cset = id.disabled;
       
        id.disabled = cset == "false";
    }  

    this.disable = function(oName)
    {        
        var id = document.getElementById(oName);
        var cset = id.disabled;
       
        id.disabled = (cset == "false") ? "true" : "false";
    }  
   
    this.readOnly = function(oName, isReadOnly)
    {
        if (isReadOnly == true)
        {
            document.getElementById(oName).readOnly = true;
        }
        else
        {
            document.getElementById(oName).readOnly = false;
        }
    }
   
    this.visible = function(oName, isVisible)    
    {
        if(isVisible == true)
        {
            document.getElementById(oName).style.display = 'block';
        }
        else
        {
            document.getElementById(oName).style.display = 'none';
        }    
    }  
   
    this.on_key_press = function()
    {
        if (window.event.keyCode == 13)
        {
            document.getElementById('btnSubmit').click();
        }
    }      
   
    this.changeMode = function(mode)
    {
        //this.visible('DIVGFCSEARCH', true);
         
        switch(mode)
        {
            case 0:
                  this.readOnly('txtTrxnNo', false);   
                  this.readOnly('txtAmount', false);
                  this.readOnly('txtCurr', false); 
                  this.readOnly('txtDate', false);
                  this.readOnly('txtStatus', false);
                 
                  this.visible('trTrxnNo', true);   
                  this.visible('trAmount', false);
                  this.visible('trCurr', false); 
                  this.visible('trDate', false);
                  this.visible('trStatus', false); 
                   
                  document.getElementById('txtStatus').innerText = "";
                  document.getElementById('txtSearchTrxnNo').innerText = ""; 
                  document.getElementById('txtTrxnNo').innerText = "";
                  document.getElementById('txtDate').innerText = "";
                  document.getElementById('txtAmount').innerText = "";
                  document.getElementById('txtCurr').innerText = "";
                      
                  this.enable('btnSubmit');
                  this.visible('DIVMAIN', false);
                  this.visible('DIVFRONT', true); 
                  document.getElementById('txtSearchTrxnNo').focus(); 
                  break;
            case 1:
                  this.readOnly('txtTrxnNo', true);   
                  this.readOnly('txtAmount', true);
                  this.readOnly('txtCurr', true); 
                  this.readOnly('txtDate', true);  
                  this.readOnly('txtStatus', true);       

                  this.visible('trTrxnNo', true);
                  this.visible('trAmount', true);
                  this.visible('trCurr', true); 
                  this.visible('trDate', true);
                  this.visible('trStatus', true);
                 
                  this.visible('DIVMAIN', true);
                  this.visible('DIVFRONT', false);                   
                            
                  this.disable('btnSubmit');
                  break;
            default:
                  this.visible('DIVMAIN', false);
                  this.visible('DIVFRONT', false);
                  break;    
        } 
    }      
}
