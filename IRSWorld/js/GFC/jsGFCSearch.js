function execute_ajaxgridview_command(param)
{ 
    if (GFC_Search != null && GFC_Search != "undefined")
    {
        gfc.do_ajaxgridview(param);
    }
}
    
function GFC_Search()
{
    var icls = this;
    var loginBranch = null; 
  
    this.page_onload = function()
    {  
         this.getLoginBranch();
    }  
    
            
   //======== Grid View ===================================
      
    this.getLoginBranch = function()
    {
        return GFCSearch.getLoginBranch(this.getLoginBranch_callback).value;  
    }
   
    this.getLoginBranch_callback = function(response)
    {
        if (response.error != null)
        {
             alert(response.error.toString().replace('System.Exception ', '')); 
        }  
        else
        {
            icls.loginBranch = response.value.toString();
            icls.process_viewer('ajx_loading$');
            this.value = icls; 
        }  
    } 
   
    this.do_ajaxgridview = function(parameter)
    {
        if (parameter != null && parameter != "undefined")
        {
            var command_param = parameter.split("$");
            if (command_param.length > 0)
            {
                if (command_param[0] == "ajx_selected")
                {                    
                    this.UnlockCK(command_param[1]);
                    return;
                }
               else
               {
                    this.process_viewer(parameter);
               }  
            }        
        }  
    }   
   
    this.UnlockCK = function(SEQ_ID)
    {
        if (confirm("Do you want to unlock this card reference number (CK)?"))
        {
            return GFCSearch.UnlockCK(SEQ_ID, this.UnlockCK_callback).value; 
        }
    }
   
    this.UnlockCK_callback = function(response)
    {
        if (response.error != null)
        {
            alert(response.error.toString().replace('System.Exception ', '')); 
        }
        else
        {
            icls.process_viewer('ajx_loading$'); 
            alert("Unlock is successful");
        }
    }    
    
    this.process_viewer = function(param)
    { 
        this.grid = new grid_viewer();       
        this.grid.sql_source     = "sp_gfc_get_locked_records~" + "%" + "~" + this.loginBranch + "~"; 
        this.grid.div_container  = document.getElementById("GRID1");
        this.grid.datarow_column = 0;
        this.grid.page_size = 15;
        this.grid.process_ajax(param);                               
        this.grid = null;
    }
}

