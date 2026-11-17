// **************************************************************************************** //
// *** AJAX GRID
// **************************************************************************************** //
function execute_ajaxgridview_command(param)
{
    if (promotions != null && promotions!= undefined)
    {
        promotions.do_ajaxgridview(param);
    }
}


var promoCode="";
var promoDescription="";
var endDate="";
var startDate="";
var promoContent="";
var promoTerms="";
var minTransCount="";
var minRemAmount="";
var state="";
var SEQ_ID;
var State_SEQ_ID;
var FldValue;
var IsReceiveVoucher = 0;


function promotions()
{
var _isSearchPromo = false;
var _isSearchState = false;
var _isLoadAllPromo = true;


// **************************************************************************************** //
// *** AJAX GRID *** AJAX GRID *** AJAX GRID *** AJAX GRID *** AJAX GRID *** AJAX GRID ***
// **************************************************************************************** //
    this.do_ajaxgridview = function(parameter)
    {
        if (parameter != null && parameter != undefined)
        {
            var command_param = parameter.split("$");
            var valueAndCode = command_param[1].split("~"); 
            
            if (command_param[1] == 'State Code Asc') //if grid table header is selected
                return;
             
             if (command_param[0] == 'ajx_paging')
             {
             this.process_grid_viewer(parameter);
             return;
             }
                
            if (valueAndCode.length > 0)
            {
            var cnfrm = confirm("Remove State?");
            if (cnfrm == true)
            {            
                promotions.updateState(valueAndCode[0], valueAndCode[1]);
            }
                return;
            }
        }
    }

    this.load_grid_viewer = function()
    {
        this.process_grid_viewer("ajx_loading");
    }
    
    this.process_grid_viewer = function(parameter)
    {
        var _sql                            = "";
        var _griddiv                        = "";
        var _gridrow                        = 7;
    
        if (promotions._isSearchState == true)
        {  
            promotions._sql      = "sp_PromoGetState~" + SEQ_ID;
            promotions._griddiv  = "divStateList";
            promotions._gridrow  = 7;
            promotions._isSearchState == false;
            document.getElementById("divStateList").style.display = "block";
            document.getElementById("ddState").disabled = false;
            document.getElementById("btnAddState").disabled = false;
        }           
        
        this.grid = new grid_viewer();
        this.grid.sql_source        = promotions._sql;
        this.grid.div_container     = document.getElementById(promotions._griddiv);
        this.grid.datarow_column    = 0;
        this.grid.page_size         = promotions._gridrow;
        this.grid.grd_header_back_color = "#26afe5";
        this.grid.grd_header_fore_color = "#FFFFFF";
        this.grid.grd_customed_date = true;
        this.grid.process_ajax(parameter);
        this.grid                   = null;
    }
    
    this.loadStates = function()
    {
        this._isSearchState = true;
        this.load_grid_viewer();
    }
//=============================================================================================
//END of AJAX GRID | END of AJAX GRID | END of AJAX GRID | END of AJAX GRID | END of AJAX GRID
//=============================================================================================

    this.initializePage = function()
    {
        this.ajaxTimeout();
        clearFields();
        //this.countRecords();
    }
    
    this.ajaxTimeout = function()
    {
        AjaxPro.timeoutPeriod = 1000 * 1000;
        AjaxPro.onTimeout = function()
        {
            promotions.showAlert("THE SERVER STOPPED RESPONDING. PLEASE TRY AGAIN.");
        }
    }
    
//=========================================================
//Populating of DropDownList
//=========================================================
     this.populate_drop_down_list_AllState = function()
    {
        promotions.ajaxTimeout();
        return IRSWorld.BranchSetup.Promotions.callPopulate_drop_down_list(this.populate_drop_down_list_AllState_callback);    
    }
     
    this.populate_drop_down_list_AllState_callback = function(response)
    {
        var dt = response.value;
        var commons = new common_method();
        commons.to_load_dropdownlist('ddState', null, 'FldValue', 'FldCode', dt);
    }
    
        this.populate_drop_down_list_NotExisting = function(SEQ_ID)
    {
        promotions.ajaxTimeout();
        return IRSWorld.BranchSetup.Promotions.callPopulate_drop_down_list_NotExisting(SEQ_ID, this.populate_drop_down_list_NotExisting_callback);    
    }
    
    this.populate_drop_down_list_NotExisting_callback = function(response)
    {
        var dt = response.value;
        var commons = new common_method();
        commons.to_load_dropdownlist('ddState', null, 'FldValue', 'FldCode', dt);
        
        if (dt == null)
        {
        return;
        }
        else 
        {
            //Adding 'All States' option in Drop Down List
            var opt = document.createElement("option");
            opt.text = 'All States';
            opt.value = 0;
            document.getElementById('ddState').options.add(opt);
        }
    }
    
//=========================================================
//END Populating of DropDownList
//=========================================================
    
    this.searchRecord = function(paramSEQ_ID)
    {
        SEQ_ID = paramSEQ_ID;
        promotions.ajaxTimeout();
        return IRSWorld.BranchSetup.Promotions.callSearchRecord(SEQ_ID, this.searchRecord_callback);    
    }
    
    this.searchRecord_callback = function(result)
    {
        if (result != null && result != "" && result != undefined)
        {
            var response = result.value;
            this.commons = new common_method();
            
            var startDate = this.commons.to_mmddyyyy_date(response.Rows[0].StartDate);
            var endDate = this.commons.to_mmddyyyy_date(response.Rows[0].EndDate);
            
            document.getElementById("txtPromoCode").value = response.Rows[0].PromoCode;
            document.getElementById("txtPromoDescription").value = response.Rows[0].PromoDescription;
            document.getElementById("txtPopValidFrom").value = startDate;
            document.getElementById("txtPopValidTo").value = endDate;
            document.getElementById("txtPromoContent").value = response.Rows[0].PromoContent;
            document.getElementById("txtPromoTerms").value = response.Rows[0].PromoTerms;
            document.getElementById("txtMinTransCount").value = response.Rows[0].MinTransactionCount;
            document.getElementById("txtMinRemAmount").value = response.Rows[0].MinRemittanceAmount;
            document.getElementById("ddReceiveVoucher").value = (response.Rows[0].IsReceiveVoucher == null) ? -1 : (response.Rows[0].IsReceiveVoucher == true) ? 1 : 0;
            
            SEQ_ID = response.Rows[0].SEQ_ID;
            
            promotions.loadStates();
            promotions.populate_drop_down_list_NotExisting(SEQ_ID);
            
            return;
        }
        else
        {
            alert("Record not found.");
            clearFields();
        }
    }
    
    this.countRecords = function(paramSEQ_ID)
    {
        promotions.ajaxTimeout();
        return IRSWorld.BranchSetup.Promotions.countRecords(this.countRecords_callback);    
    }
    
    this.countRecords_callback = function(result)
    {
        if (result != null && result != "" && result != undefined)
        {
            //var response = result.value;
            alert(result);
            return;
        }
       
    }
    
    this.insertRecord = function()
    {
        if (!promotions.validateInsert())
        {
            alert('Blank fields are not allowed.');
            return;
        }
        
        if (!promotions.checkIfIsNumber())
        {
            alert('Remittance Amount must be a numeric value.');
            return;
        }
        
        if(!promotions.checkIfWholeNumber(document.getElementById("txtMinTransCount").value))
        {
            alert('Transaction count must be a numeric value.');
            return;
        }
        
//        if (!promotions.validateDate(document.getElementById("txtPopValidFrom").value))
//        {
//            alert('Start Date is not a valid format (MM/DD/YYYY)');
//            return;
//        }
//        
//         if (!promotions.validateDate(document.getElementById("txtPopValidTo").value))
//        {
//            alert('End Date is not a valid format (MM/DD/YYYY)');
//            return;
//        }
        
        var cnfrm = confirm("Insert Record?");
        if (cnfrm == false)
        {  
            return;
        }
        
        var isAdd = true;
        
        promoCode = document.getElementById("txtPromoCode").value;
        promoDescription = document.getElementById("txtPromoDescription").value;
        startDate = document.getElementById("txtPopValidFrom").value;
        endDate = document.getElementById("txtPopValidTo").value;
        promoContent = document.getElementById("txtPromoContent").value;
        promoTerms = document.getElementById("txtPromoTerms").value;
        minTransCount = document.getElementById("txtMinTransCount").value;
        minRemAmount = document.getElementById("txtMinRemAmount").value;
        IsReceiveVoucher = document.getElementById("ddReceiveVoucher").value;
        
        promotions.ajaxTimeout();
        return IRSWorld.BranchSetup.Promotions.callInsertRecord(0, promoCode, promoDescription, minTransCount, minRemAmount, startDate, endDate, promoContent,promoTerms, isAdd, IsReceiveVoucher, this.insertRecord_callback);
    }
   
    this.insertRecord_callback = function(result)
    {
        var response = result.value;
        alert(response);
        clearFields();
    }
    
    this.updateRecord = function()
    {
        if(SEQ_ID == null || SEQ_ID == 'null' || SEQ_ID == "")
        {
            alert('Please select a record to update.');
            return;
        }
    
        if (!promotions.validateInsert())
        {
            alert('Blank fields are not allowed.');
            return;
        }
        
        if (!promotions.checkIfIsNumber())
        {
            alert('Numeric Values Only for Transaction Count and Remittance Amount');
            return;
        }
        
//        if (!promotions.validateDate(document.getElementById("txtPopValidFrom").value))
//        {
//            alert('Start Date is not a valid format (MM/DD/YYYY)');
//            return;
//        }
//        
//        if (!promotions.validateDate(document.getElementById("txtPopValidTo").value))
//        {
//            alert('End Date is not a valid format (MM/DD/YYYY)');
//            return;
//        }
        
        var cnfrm = confirm("Update Record?");
        if (cnfrm == false)
        {  
            return;
        }
        
        var isAdd = false;
        var paramSEQ_ID = SEQ_ID;
         
        if (paramSEQ_ID == 0 || paramSEQ_ID == 'null' || paramSEQ_ID == 'undefined' || paramSEQ_ID == '')
        {
        alert('Please select a record to update.');
        return;
        }
         
        promoCode = document.getElementById("txtPromoCode").value;
        promoDescription = document.getElementById("txtPromoDescription").value;
        startDate = document.getElementById("txtPopValidFrom").value;
        endDate = document.getElementById("txtPopValidTo").value;
        promoContent = document.getElementById("txtPromoContent").value;
        promoTerms = document.getElementById("txtPromoTerms").value;
        minTransCount = document.getElementById("txtMinTransCount").value;
        minRemAmount = document.getElementById("txtMinRemAmount").value;
        IsReceiveVoucher = document.getElementById("ddReceiveVoucher").value;
        
        promotions.ajaxTimeout();
        return IRSWorld.BranchSetup.Promotions.callInsertRecord(paramSEQ_ID, promoCode, promoDescription, minTransCount, minRemAmount, startDate, endDate, promoContent, promoTerms, isAdd, IsReceiveVoucher, this.updateRecord_callback);
    }
    
    this.updateRecord_callback = function(result)
    {
        var response = result.value;
        alert(response);
    }
    
    this.insertState = function()
    {
        var isAdd = true;
        var x = document.getElementById("ddState").value;
        if (document.getElementById("ddState").value == '' || document.getElementById("ddState").value == 'null')
        {
          alert("State is empty.");
          return;
        }
        
        var cnfrm = confirm("Add State?");
        if (cnfrm == false)
        {  
            return;
        }
        
        var promoSEQ_ID = SEQ_ID;
        var dd= document.getElementById("ddState");
        var stateCode = dd.options[dd.selectedIndex].text;
                
        promotions.ajaxTimeout();
        return IRSWorld.BranchSetup.Promotions.callInsertState(promoSEQ_ID, stateCode, isAdd, this.insertState_callback);
    }
    
    this.insertState_callback = function(result)
    {
        var response = result.value;
        promotions.loadStates();
        promotions.populate_drop_down_list_NotExisting(SEQ_ID);
        alert(response);
    }
    
    this.updateState = function(paramFldValue, paramFldCode)
    {
        var isAdd = false;
        var promoSEQ_ID = SEQ_ID;
        
        promotions.ajaxTimeout();
        return IRSWorld.BranchSetup.Promotions.callupdateState(promoSEQ_ID, paramFldValue, paramFldCode, isAdd, this.updateState_callback);
    }
    
    this.updateState_callback = function(result)
    {
        var response = result.value;
        promotions.loadStates();
        promotions.populate_drop_down_list_NotExisting(SEQ_ID);
        alert(response);
    }   
    
    this.removePromoCode = function()
    {
        if(SEQ_ID == null || SEQ_ID == 'null' || SEQ_ID == "")
        {
            alert('Please select a record to Delete.');
            return;
        }
    
        if (document.getElementById("txtPromoCode").value == '' || document.getElementById("txtPromoCode").value == 'null' || document.getElementById("txtPromoCode").value == 'undefined')
        {
        alert('Please choose a Promo Code.');
        return;        
        }
        
        var cnfrm = confirm("Delete Promo?");
        if (cnfrm == false)
        {  
            return;
        }
    
        promoCode = document.getElementById("txtPromoCode").value;
        promotions.ajaxTimeout();
        return IRSWorld.BranchSetup.Promotions.callRemovePromoCode(SEQ_ID, this.removePromoCode_callback);
    }
    
    this.removePromoCode_callback = function(result)
    {
        var response = result.value;
        alert(response);
        clearFields();
    }
    
    
//===================================================================
//Validation of Fields
//===================================================================
    this.validateInsert = function()
    {
        var xzxc = document.getElementById("ddReceiveVoucher").value;
        if (document.getElementById("txtPromoCode").value == '' ||
        document.getElementById("txtPromoDescription").value == '' || 
        document.getElementById("txtPopValidFrom").value == ''|| 
        document.getElementById("txtPopValidTo").value == '' || 
        document.getElementById("txtPromoContent").value == '' || 
        document.getElementById("txtPromoTerms").value == '' ||
        document.getElementById("txtMinTransCount").value == '' || 
        document.getElementById("txtMinRemAmount").value == '' ||
        document.getElementById("ddReceiveVoucher").value == '')
        return false;
        else
        return true;
    }
    
//    this.validateDate = function(birthdate)
//    {
//        var beeday = new RegExp(/^\d{1,2}\/\d{1,2}\/\d{4}$/);
//        
//        if (birthdate != null && birthdate != '')
//        {
//            if (beeday.test(birthdate))
//            {
//                return true;
//            }
//            return false;
//        }
//        return true;              
//    }
    
    this.checkIfIsNumber = function()
    {
        if (isNaN(document.getElementById("txtMinTransCount").value) || isNaN(document.getElementById("txtMinRemAmount").value))
           return false;
        else
           return true;
    }
    
    this.checkIfWholeNumber = function(num)
    {
        return /^\d+$/.test(num);
    }
//===================================================================
//END Validation of Fields
//===================================================================
   
    
}

function client_search()
{
    var client_search     = this;

    this.outer_modal_div  = null;
    this.inner_modal_div  = null;
    this.commons          = null;
    
    window.onscroll = function () 
    {		
        var i = 0;
        var m = 0;
    	
	    var divelement_outer = document.documentElement.getElementsByTagName("div");
	    var divelement_inner = document.documentElement.getElementsByTagName("div");
    	    	    	
        for(i = 0; i < divelement_outer.length;i++)
        {
            if (divelement_outer.item(i).id == "outer_mod_div")
            {
                divelement_outer.item(i).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop;break;
            }	        
        }
        for(m = 0; m < divelement_inner.length;m++)
        {
            if (divelement_inner.item(m).id == "inner_mod_div")
            {
                //divelement_outer.item(m).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop + 10 : document.body.scrollTop + 10;break;
                divelement_outer.item(m).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop + 30 : document.body.scrollTop + 30;break;
            }
        }
    }
}

function exec_modal_lookup(param)
{
    var command_param = param.split("$");

    if (command_param != null)
    {
        if (command_param[0].toUpperCase() == "AJX_SELECTED")
        {
            var param_value = command_param[1].split("~");
            switch(this.grd_search_keyword.toUpperCase())
            {
              case "PROMO_SEARCH" : this.grd_search.process_ajax(param); 
                                        (command_param[1] == 'null' || command_param[1] == '' || command_param[1] == 'undefined') ? false : promotions.searchRecord(command_param[1]); break;
            }
        }
        else if (command_param[0].toUpperCase() == "AJX_SEARCH" || command_param[0].toUpperCase() == "AJX_PAGING")
        {
            this.grd_search.process_ajax(param); 
        }
    }

    if (this.grd_search_keyword.toUpperCase() != "PROMO_SEARCH")
    {
        this.grd_search.process_ajax(param);
    }
        
}

function open_remitter_result()
{
    //this.grd_search_keyword = "REMITTER_SEARCH";
    this.grd_search_keyword = "PROMO_SEARCH";
    this.grd_source  = "sp_PromoSearchGetAllPromo~";
    this.grd_search  = new grid_selector();
    this.grd_search.open_selection_grid(null, this.grd_source, 0 , null);        
}

function execute_ajx_command(param)
{
    if (client_search != null)
    {
       exec_modal_lookup(param);
    }
}

function clearFields()
{
    //Clear Fields
    document.getElementById("txtPromoCode").value = "";
    document.getElementById("txtPromoDescription").value = "";
    document.getElementById("txtPopValidFrom").value = "";
    document.getElementById("txtPopValidTo").value = "";
    document.getElementById("txtPromoContent").value = "";
    document.getElementById("txtMinTransCount").value = "";
    document.getElementById("txtMinRemAmount").value = "";
    document.getElementById("txtPromoTerms").value = "";
    document.getElementById("ddReceiveVoucher").value = "";
    promoCode="";
    promoDescription="";
    endDate="";
    startDate="";
    promoContent="";
    minTransCount="";
    minRemAmount="";
    IsReceiveVoucher = 0;
    
    //Default Value of Fields
    promoCode="";
    promoDescription="";
    endDate="";
    startDate="";
    promoContent="";
    promoTerms="";
    minTransCount="";
    minRemAmount="";
    state="";
    SEQ_ID = null;
    State_SEQ_ID = null;
    FldValue = null;
    
    //Default State of Fields
    document.getElementById("ddState").disabled = true;
    document.getElementById("btnAddState").disabled = true;
    document.getElementById("divStateList").style.display = "none";
    
}
