function execute_ajx_command(param)
{
    if (OBPS_Denomination != null && OBPS_Denomination != "undefined")
    {
        OBPS_Denomination.do_gridsearch(param);
    }
}


function execute_ajaxgridview_command(param)
{
    if (obps_denomination != null && obps_denomination != "undefined")
    {
        obps_denomination.do_gridview(param);
    }
}
function OBPS_Denomination()
{
    this.grid_ajax_page      = "../AjxPage/ajaxgrid.aspx";
    this.grid_ajax_query     = null;    
    this.ajax_grid           = null;
    this.ajax_container      = null;
    this.txn_id              = null;
    this.denom_code          = null;
    this.denom_value         = null;
    this.result              = null;
    
    this.load = function()
    {
        this.dropdown_denomination();
        this.load_denomination_grid();
    }
    
    this.dropdown_denomination = function()
    {
        OBPSDenomination.getdropdown_denomination("GenericDenomination", this.dropdown_denomination_callback)
    }
    
    this.dropdown_denomination_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();         
            this.commons.to_load_dropdownlist("ddlDenominations", result.value, "FldCode", "FldValue");
            this.commons = null;           
        }
    }
    
    this.load_denomination_grid = function()
    {
        this.txn_id = document.getElementById("lblRefNo").innerText;
        this.grid = new grid_viewer();
        this.grid.sql_source     = "sp_denomination_grid~" + this.txn_id;
        this.grid.div_container  = document.getElementById("DIV_DENOM");
        this.grid.datarow_column = 0;
        this.grid.open_grid_view();
        this.grid = null;
    }
    
   
    this.set_denomination = function()
    {   
        this.txn_id                   = document.getElementById("lblRefNo").innerText;
        var index                     = document.getElementById("ddlDenominations").selectedIndex;
        this.denom_code               = document.getElementById("ddlDenominations").options[index].text;
        this.denom_value              = document.getElementById("txtValue").value;
        return this.push_data_to_server();
        this.load_denomination_grid();
     }
 
    this.push_data_to_server = function()    
    {    
        var data_row = new Array();
        
        data_row[data_row.length]  = "Txn_SeqId~"          + this.txn_id;   
        data_row[data_row.length]  = "Denom_Code~"         + this.denom_code;          
        data_row[data_row.length]  = "Denom_Value~"        + this.denom_value;                  
                     
        return data_row.join("|");       
    }
    
     
    this.insert_record = function()
    {     
        OBPSDenomination.insert_record(this.insert_record_callback);
    }
      
    
    this.insert_record_callback = function(result)
    {       
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = OBPSDenomination.read_message(); alert(msg.value); return;
            }
                   
        }
    }
    
    this.commit_record = function()
    {
        if (confirm("Are you sure you want to save this record?"))
        {
            OBPSDenomination.commit_record(this.set_denomination(),this.commit_record_callback);
        }

    }

    this.commit_record_callback = function(result)
    {
        if (result != null && result.value != "undefined")
        {        
            if (result.value == false)
            {
                var msg = OBPSDenomination.read_message(); alert(msg.value); return;
            }
            
            this.SEQ_ID = OBPSDenomination.DENOMINATION_SEQ_ID().value;
            
            alert("RECORD COMITTED SUCCESSFULLY");   
        }
    }
    
    this.do_gridview = function(parameter)
    {
        this.ajax_grid = new grid_selector();
        if (parameter != null && parameter != "undefined")
        {
            var command_param = parameter.split("$");
                                    
            if (command_param.length > 0)
            {                                                
                if (command_param[0] == "ajx_selected")
                {
                        
                    var command_value = command_param[1].split('~');

                    this.ProductChargesID   = command_value[0];
                    product_charges_id = command_value[0];
                    
                    document.getElementById("div_details").style.display                                = "block";
                    sidebar.get_side_bar_visibility("divtdBillsPayOBPSTransactionsMenu","btnAnchor");
                    document.getElementById("tdBillsPayOBPSTransactions").style.display                 = "block";
                    document.getElementById('tdMerchantTable').style.height = '1px';
                    document.getElementById("tblMain").style.display                                    = "block";                                      
                    document.getElementById("tblBPRemitter").style.display                              = "block";
                    document.getElementById('TrnxDefaults').style.display                               = "none";
                    document.getElementById("tblBillsPayOBPSTransactionsMerchants").style.display       = "block";
                    document.getElementById("tblBillsPayOBPSTransactionsMerchants").align               = "right";
                    document.getElementById("tblBillsPayOBPSTransactionsMerchants").style.height        = "100%";
                }
                this.ajax_grid.process_ajax(parameter); 
                
            }
        }
    }
    
   
}

