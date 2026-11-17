function execute_ajx_command(param)
{
    if (aggr != null && aggr != "undefined")
    
    {
        aggr.do_ajax(param);
    }
}

function execute_ajaxgridview_command(param)
{
    if (aggr != null && aggr != "undefined")
    {
        aggr.do_ajaxgridview(param);
    }
}

/*

Note*
    Page Mode (mode)
        0  =   Aggregation limit setup
        1  =   Aggregation Report 
    
    Aggregation Report Mode (aggreMode)
        0  =   Search Mode
        1  =   Detail Mode
           
*/

function GFC_Aggregation()
{
    var icls = this;
    var mode = null;
    var aggreMode = null;
    var CIFID = null;
    var GFCNumber = null 
      
    //var amount = null; 
    
    this.on_load = function()
    {
         this.mode = getQueryString(window, 'mode');
         
         this.disable('txtAmount');
         this.disable('btnCancel');
         this.getAggreAmount();
        
         this.change_page_mode(this.mode);    
    }
    
    this.change_page_mode = function(mode)
    {
        this.HideAll();
         
         switch(mode)
         {
            case '0':
                this.visible('DIVAGGRELIMIT');                
                break; 
            case '1':
                this.visible('DIVSEARCHAGGRE');
                this.change_aggre_report_mode(0);
                this.populate_drop_down_list();                   
                break; 
         } 
    }
   
    this.populate_drop_down_list = function()
    {
        return GFCAggregation.get_drop_down_data(this.populate_drop_down_list_callback).value;  
        
    } 
   
    this.populate_drop_down_list_callback = function(response)
    {
         if (response.error != null)
         {
            alert(response.error); 
         }
         else
         {
            var ds = response.value; 
            var commons = new common_method(); 
            commons.to_load_dropdownlist('ddlMonth',    null, 'FldValue', 'FldDescription', ds.Tables[0]);        
            commons = null; 
         }   
    }  
   
    this.change_aggre_report_mode = function(mode)
    {
        switch(mode)
        {
            case 0:
                this.visible('DIVSEARCH');
                this.visible('DIVSEARCHBTN');
               
                this.invisible('DIVCIFINFO');
                this.invisible('DIVAGGREPARAM');
                this.invisible('DIVAGGREDETAILS');                
                break;   
            case 1:  
                this.visible('DIVCIFINFO');
                this.visible('DIVAGGREPARAM');
                this.visible('DIVAGGREDETAILS');
               
                this.invisible('DIVSEARCH');
                this.invisible('DIVSEARCHBTN');  
                break;     
        }  
    }
   
    this.HideAll = function()
    {
        this.invisible('DIVAGGRELIMIT'); 
        this.invisible('DIVSEARCHAGGRE');
        this.invisible('DIVSEARCH');
        this.invisible('DIVSEARCHBTN');
        this.invisible('DIVCIFINFO');
        this.invisible('DIVAGGREPARAM');     
        this.invisible('DIVAGGREDETAILS');    
    }     
    
    this.getAggreAmount = function()
    {
        return GFCAggregation.getAggreAmount(this.getAggreAmount_callback).value; 
    }
    
    this.getAggreAmount_callback = function(response)
    {
        if (response.error != null)
        {
            alert(response.error.toString().replace('System.Exception ', ''));
        }
        else
        {            
            document.getElementById('txtAmount').innerText = response.value.toString();
        }   
    }     
   
    this.getGlobalVariables = function()
    {
        var dt = RemTranGFMCCommons.getGlobalVariable().value;
        
         if (dt != null && typeof(dt) == "object" && dt.Rows.length > 0)
         {      
            this.UserID = dt.Rows[0].UserLogged.toString();
            this.BranchCode = dt.Rows[0].BranchCode.toString() 
         } 
    }  
   
     this.btnUpdate_click = function()
     {
        if (document.getElementById('btnUpdate').value == 'Update')
        {
            document.getElementById('btnUpdate').innerText = 'Save'; 
            this.enable('txtAmount');
            this.enable('btnCancel');
        }
        else
        {
            if (confirm("Are you sure you want to update the aggregation amount?"))
            {
                if (isNumeric(document.getElementById('txtAmount').value.toString()))
                {
                    return GFCAggregation.UpdateAggreAmount(document.getElementById('txtAmount').value, this.btnUpdate_click_callback).value; 
                }
                else
                {
                    alert('Invalid Amount');
                }
            }  
            
        }     
     }
    
    this.btnUpdate_click_callback = function(response)
    {
         if (response.error != null)
         {
            alert(response.error.toString().replace('System.Exception ', ''));
         }
         else
         {
            document.getElementById('btnUpdate').innerText = 'Update'; 
            icls.disable('txtAmount');
            icls.disable('btnCancel');
            icls.getAggreAmount();
            alert('Aggregation Amount was successfuly updated');          
         }   
    }   
    
    this.btnCancel_click = function()
    {
        this.getAggreAmount(); 
        document.getElementById('btnUpdate').innerText = 'Update'; 
        this.disable('txtAmount');
        this.disable('btnCancel');           
    }
   
    this.btnSearch_click = function()
    {
        if (this.check_search_input() == true)
        {  
            this.ajax_grid = new grid_selector();
            this.grid_ajax_query = "sp_GFCSearchBeneficiary" + "~" +  
                document.getElementById("txtLastName").value   + "~" + 
                document.getElementById("txtFirstName").value  + "~" + 
                document.getElementById("txtMiddleName").value + "~" + 
                document.getElementById("txtCIFID").value + "~" + 
                document.getElementById("txtOldCIFID").value + "~" +
                document.getElementById("txtGFCNumber").value + "~" +
                "" + "~" +
                "1" + "~"; 
                
            this.ajax_grid.open_selection_grid(null, this.grid_ajax_query, 0, null);
        }         
    }
   
    this.btnPreview_click = function()
    {
         this.process_viewer_benef('ajx_loading$'); 
    }
    
    this.btnReport_click = function()
    {
        var Month = document.getElementById("ddlMonth").value.toString();
        var Year = document.getElementById("txtYear").value.toString();
        var CIF = document.getElementById("lblCIFID").innerText;   
        var CardNo =  document.getElementById("lblGFCNo").innerText;
        return GFCAggregation.set_report_Monthly_Transactions(Month, Year, CIF, CardNo,  this.show_report_callback).value;
    }
    
    this.show_report_callback = function(response)
    {
        if (response.error != null){alert('Failed to load report');}
        else
        {            
           window.open('../Reports/Pages/Report_Viewer.aspx' + response.value.toString(),1) 
        }
    } 
   
    this.btnBack_click = function()
    {
         this.change_aggre_report_mode(0);
    }       
    
    this.check_search_input = function()
    {
        var lastname_length = (document.getElementById("txtLastName").value).length;
        var firstname_length = (document.getElementById("txtFirstName").value).length;
        var middlename_length = (document.getElementById("txtMiddleName").value).length;
        var cifid_length = (document.getElementById("txtCIFID").value).length;
        var oldcifid_len = (document.getElementById("txtOldCIFID").value).length;        
        var CardNumber_len =  (document.getElementById("txtGFCNumber").value).length;
          
        var total_length = lastname_length + firstname_length + middlename_length + cifid_length 
                                 + oldcifid_len + CardNumber_len;
        
        if (total_length == 0)
        {
             alert("Invalid search value!");
             return false;   
        }
        
        return true;
    }       
   
    this.do_ajax = function(parameter)
    {   
       if (parameter != null && parameter != "undefined")
       {
            var command_param = parameter.split("$");
            if (command_param.length > 0)
            {
                if (command_param[0] == "ajx_selected")
                {
                    this.populateInfo(command_param[1], "true");                   
                }
            }
        }                
       this.ajax_grid.process_ajax(parameter);
    }    
   
    this.populateInfo = function(SEQ_ID)
    {
        var param = SEQ_ID.split("~");
        return GFCAggregation.getSearchDetails(param[0], '1', this.populateInfo_callback).value;
    }    
   
    this.populateInfo_callback = function(response)
    {
        if (response.error != null)
        {
            alert(response.error.toString().replace('System.Exception ', ''));
        }
        else
        {
            var ds = response.value;
           
            document.getElementById('lblCustLName').innerText = ds.Tables[0].Rows[0].LastName.toString();
            document.getElementById('lblCustFName').innerText = ds.Tables[0].Rows[0].FirstName.toString();
            document.getElementById('lblCustMName').innerText = ds.Tables[0].Rows[0].MiddleName.toString();
           
            document.getElementById('lblCIFID').innerText = ds.Tables[0].Rows[0].CIFID.toString();
            document.getElementById('lblGFCNo').innerText = ds.Tables[0].Rows[0].GFCNo.toString();
            document.getElementById('lblOldCIFID').innerText = ds.Tables[0].Rows[0].BirthDate.toString();
            
            icls.CIFID = ds.Tables[0].Rows[0].CIFID.toString();
            icls.GFCNumber = ds.Tables[0].Rows[0].GFCNo.toString();
            
            icls.change_aggre_report_mode(1);
            icls.process_viewer_benef('ajx_loading$');      
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
                    // do nothing
                }
               else
               {
                     if (command_param[0] != "ajx_sorting") 
                     {
                        this.process_viewer_benef(parameter);
                     }
               }  
            }        
        }  
    }   
   
   // GRID VIEWER    
    this.process_viewer_benef = function(param)
    { 
        this.grid = new grid_viewer();
        var Month = (document.getElementById('ddlMonth').value == 'null') ? 0 : document.getElementById('ddlMonth').value;
        var Year = (document.getElementById('txtYear').value == '') ? 0 : document.getElementById('txtYear').value;
        
        this.grid.sql_source     = "sp_gfc_get_aggregate_details~" + this.CIFID + "~" + this.GFCNumber + "~" + Month.toString() + "~" + Year.toString() + "~"; 
        this.grid.div_container  = document.getElementById("AGGREDETAILS");
        this.grid.datarow_column = 0;
        this.grid.page_size = 20;
        this.grid.process_ajax(param);
        this.grid = null;
    } 
        
   /*Private function*/
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
   
   this.visible = function(oName)
   {
        document.getElementById(oName).style.display = 'block';
   }     
   this.invisible = function(oName)
   {
        document.getElementById(oName).style.display = 'none';
   }     
    
} 
