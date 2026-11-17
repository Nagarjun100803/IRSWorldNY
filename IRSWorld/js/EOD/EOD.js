var param ;
function EOD()
{   
    this.get_transaction_date = function()    
    {        
        document.getElementById("txttranDate").value = document.getElementById("hdTranxDate").value;
    }        
    
    this.to_mmddyyyy_date = function(dt)
    {
        if (dt == null) return null;
        if (dt.constructor == Date)
        {
            var mo = "" + (dt.getMonth() + 1);
            var dd = "" + dt.getDate();
                            
            return ((mo.length == 1) ? "0" + mo : "" + mo) + "/" + ((dd.length == 1) ? "0" + dd : "" + dd) + "/" + dt.getFullYear();
        }
    }
   
    this.PrintCA = function()
    {
       
        param = document.getElementById("txttranDate").value;
        var url = "../Reports/Pages/Report_viewer.aspx?trantype=EOD&tranvalue=" + escape(param.toString());
        document.js_closer = "close_report_eod"
        show_frame_dialog(url, null, '100%', '100%', '0',null, null, true);    
        
        
        
    }   
    
    this.chk_eod_items = function()
    {
        var eoddate = document.getElementById("txttranDate").value
        eoddate = eoddate.toString();
        
         document.getElementById("btnEODclick").disabled = true;
        
        if (eoddate.length == 0 || eoddate == "null" )
        {
            spawn_errpage("SESSION EXPIRED");
            return;
        }
        
           this.toggle_divmsg("block");
           AjaxPro.timeoutPeriod = 100 * 1000;
    
           AjaxPro.onTimeout = function()
           {           
                spawn_errpage("SESSION EXPIRED");
                return; 
           }
           var brcode = document.getElementById("hdBranchCode").value;
           IRSWorld.EOD.EndOfDay.chk_eod_items(brcode,cls_EOD.chk_eod_items_callback)
    }
    this.toggle_divmsg = function(_toggle)
    {
        var div = document.getElementById("divWaitMsg");
        
        if (_toggle == "block")
        {
            divmask = new maskpanel();
            divmask.show();            
            div.align        = "center";
	        div.style.height = "auto";		
	        div.style.width  = parseInt(parent.screen.availWidth)  - 600;
	        div.style.top    = 40;
	        //div.style.left   = (parseInt(screen.width) - parseInt(div.style.width)) / 2 - 310; 
	    }
	    else
	    {
	        try
	        {
	            divmask.hide();
	        }
	        catch(e) {}
	    }
        document.getElementById("divWaitMsg").style.display = _toggle;
    }
    this.has_session_timeout = function()
        {
            var url = "../../Main/IRSMain.aspx";            
            if (top == self)
            {
                window.open(url, '_top');
            }
            else
            {                
                spawn_errpage("SESSION EXPIRED");
            }          
        }    
    this.chk_eod_items_callback = function(result)
    {
        cls_EOD.toggle_divmsg("none");
        
        if (result != null || result != undefined)
        {               
                try
                {
                var dt      = result.value.Tables[0]
                var dtapp   = result.value.Tables[1]
                var dtPO    = result.value.Tables[2]
                
                var hasapprovalitems = false;
                var hasPendingOverride =false;
                    if (dt.Rows.length == 0)
                    {
                        alert("THERE ARE NO ITEMS FOR EOD.")
                        document.getElementById("btnEODclick").disabled = false;
                        return
                    }                    
                    if (parseInt(dt.Rows[0].NoofItems ) == 0)
                    {
                        alert("THERE ARE NO ITEMS FOR EOD.")
                        document.getElementById("btnEODclick").disabled = false;
                        return
                    }
                    if (dtapp.Rows.length == 0)
                    {
                        alert("THERE ARE NO ITEMS FOR APPROVAL")
                     
                    }
                    else 
                    {
                        if (parseInt(dtapp.Rows[0].ApprovalItems ) == 0)
                        {
                            alert("THERE ARE NO ITEMS FOR APPROVAL")                            
                        }
                        else
                        {
                            hasapprovalitems = true;
                        }
                    }
                    
                    //new code for pending override
                     if (dtPO.Rows.length == 0)
                    {
                        alert("THERE ARE NO ITEMS FOR PENDING OVERRIDE")
                     
                    }
                    else 
                    {
                        if (parseInt(dtPO.Rows[0].NoOfPendingOverride ) == 0)
                        {
                            alert("THERE ARE NO ITEMS FOR PENDING OVERRIDE")                            
                        }
                        else
                        {
                            hasPendingOverride = true;
                        }
                    }
                    //----------------------------------------------------------------------
                    
                    if (hasapprovalitems)
                    {
                        if (confirm("THERE ARE " + dtapp.Rows[0].ApprovalItems + " APPROVAL ITEM(S). CONTINUE WITH EOD?" ))
                        {
                           // cls_EOD.PrintCA();
                           // return;
                        }
                        else
                        {
                            return;
                        }
                    }
                   /* else 
                    {                       
                        cls_EOD.PrintCA();                       
                        return;
                    }*/
                    
                    //new code for Pending Override
                    if (hasPendingOverride)
                    {
                        if (confirm("THERE ARE " + dtPO.Rows[0].NoOfPendingOverride + " PENDING OVERRIDE ITEM(S). CONTINUE WITH EOD?"))
                        {
                            cls_EOD.PrintCA();
                            return;
                        }
                    }
                     else 
                    {                       
                        cls_EOD.PrintCA();                       
                        return;
                    }
                    //----------------------------------------------------------------------------------------------
                    
                }
                catch(e)
                {                    
                    try
                    {
                        cls_EOD.has_session_timeout();
                    }
                    catch(e){}        
                }
               
        } 
     
    
    }
    
   
    
}


 function close_report_eod()
 {
        document.js_closer = null;
        if (confirm("DO YOU WANT TO VIEW PUSHED TO ORPS REPORTS?"))
        {            
            var url1 = "../Reports/Pages/Report_viewer.aspx?trantype=ORPS&tranvalue=" + escape(param.toString());         
            
            show_frame_dialog(url1, null, '100%', '100%', '0',null, null, true);    
            return;
        }
        

 }