var courier = new courier();
var search_3D = new search_3D();

function execute_ajx_command(param)
{
    if (courier != null)
    {
        courier.exec_modal_summary(param);
    }
}


function execute_ajaxgridview_command(param)
{
    if (courier != null)
    {
        courier.exec_modal_summary(param);
    }
}

function fixht()
{
    document.getElementById('divBody').style.height = (top.document.getElementById('divCenter').offsetHeight - 30) + 'px';
    document.getElementById('divBody').style.width = (top.document.getElementById('divCenter').offsetWidth - 3) + 'px';
}

function fixmain()
{
    var brmain = false;
    try
    {
         brmain = parent.is_branchsetup_main();
    }
    catch(e){}
    
    if (!brmain)
    {
        document.getElementById('theBody').onresize = fixht;
        document.getElementById('divBody').style.overflow = 'auto';
        
        fixht();
    }        
} 
    
function courier()
{
    var icls = this;
    this.summary = null;
    this.grd_source = null;
    this.grd_search = null;
    this.display_grid = null;
    this.sequence_id = null;
    
    this.do_cancel = function()
    {
        search_3D.enable('btnSave');
        search_3D.enable('btnUpdateCancel');
        search_3D.invisible('TRANDETAILS');
        search_3D.invisible('TRANXUPDATES');
        search_3D.visible('SEARCH3D'); 
        this.load_popup_summary()
    }
    
    this.update = function()
    {
        this.saveupdate(parseInt(this.sequence_id));
    }
    
    this.open_summary = function()
    {
        this.summary = new summary_3D();        
        this.summary.open_search();        
    }
    
    this.load_currency = function()
    {
        IRSWorld._3D.Extract_3D.load_currency(this.load_currency_callback);
    }
    
    this.load_currency_callback = function(result)
    {
        var ddl = new common_method(); ddl.to_load_dropdownlist("ddl_3D_Currency", result.value, "fldvalue", "flddescription", null);
    }
    
    this.acknowledge_txns = function()
    {
        //code here
        var amt=0;
        var head_count=0;
        var dt = IRSWorld._3D.Courier_3D_List.load_courier_3d_details(document.getElementById("ddl_3D_Status_Code").value, document.getElementById("txtCouVal").value , document.getElementById("ddl_3D_search_currency").value, document.getElementById("txtRemDateFrom").value,document.getElementById("txtRemDateTo").value).value;
        if(dt.Rows.length <= 0)
        {
            alert("No Record(s) to update")
            return;
        }
        for(var i=0;i<=dt.Rows.length - 1;i++)
        {
             amt += parseFloat(dt.Rows[i].NetToBeneAmount.toString());
             head_count+=1;
             this.update_3d_status(dt.Rows[i].tag.toString(),"259");
        }
        
        alert("Record Successfully Updated \n Total Amount: " + amt + "\n" + "Total Count: " + head_count);
        search_3D.load_search_currency();
    }
    
    
    this.for_delivery_txns = function()
    {
        //code here
        var amt=0;
        var head_count=0;
        var dt = IRSWorld._3D.Courier_3D_List.load_courier_3d_details(document.getElementById("ddl_3D_Status_Code").value, document.getElementById("txtCouVal").value , document.getElementById("ddl_3D_search_currency").value, document.getElementById("txtRemDateFrom").value,document.getElementById("txtRemDateTo").value).value;
        
        if(dt.Rows.length <= 0)
        {
            alert("No Record(s) to update")
            return;
        }
        
        for(var i=0;i<=dt.Rows.length - 1;i++)
        {
             amt += parseFloat(dt.Rows[i].NetToBeneAmount.toString());
             head_count+=1;
             this.update_3d_status(dt.Rows[i].tag.toString(),"261");
        }
        
        alert("Record Successfully Updated \n Total Amount: " + amt + "\n" + "Total Count: " + head_count);
        search_3D.load_search_currency();
    }
    
    this.update_3d_status = function(seq_id,statusCode)
    {
        var data_row = new Array();
            
        data_row[data_row.length] = "Transtatus~" + statusCode;

        IRSWorld._3D.remittance_update.update_remittance(document.getElementById("ddl_3D_search_currency").value, document.getElementById("ddl_Courier_Status").value, document.getElementById("txtCouVal").innertext, document.getElementById('lblTransactionAmount').innerText, data_row.join("|"),parseInt(seq_id));   
    }
    
    this.set_button_visibility = function(statusCode)
    {
        if(statusCode == "257")
        {
            document.getElementById('btnAcknowledge').disabled = '';
            document.getElementById('btnForDelivery').disabled = 'disabled';
        }
        else if(statusCode == "259")
        {
            document.getElementById('btnAcknowledge').disabled = 'disabled';
            document.getElementById('btnForDelivery').disabled = '';
        }
        else
        {
            document.getElementById('btnAcknowledge').disabled = 'disabled';
            document.getElementById('btnForDelivery').disabled = 'disabled';
        }
    }
    
    
    
    
    
    this.load_popup_summary = function()    
    {
        if  (
            document.getElementById("ddl_3D_Status_Code").value == null | document.getElementById("ddl_3D_Status_Code").value == "null" | 
            //document.getElementById("ddlCourier").value == null | document.getElementById("ddlCourier").value == "null" | 
            document.getElementById("ddl_3D_search_currency").value == null | document.getElementById("ddl_3D_search_currency").value == "null" | 
            document.getElementById("txtRemDateFrom").value == null | document.getElementById("txtRemDateFrom").value == "null" | document.getElementById("txtRemDateFrom").value == "" |
            document.getElementById("txtRemDateTo").value == null | document.getElementById("txtRemDateTo").value == "null" | document.getElementById("txtRemDateTo").value == ""
            )
        {
            alert("Please complete fields for searching!");
        }
        else
        {
//            this.grd_source  = "sp_get_3D_Details~" + document.getElementById("ddl_3D_Status_Code").value + "~" + document.getElementById("txtCouVal").value + "~" + document.getElementById("ddl_3D_search_currency").value + "~" + document.getElementById("txtRemDateFrom").value + "~" + document.getElementById("txtRemDateTo").value;
//            this.grd_search  = new grid_selector();
//            this.grd_search.open_selection_grid(null, this.grd_source, 0 , 'div_courier_search');
//            var test = new common_method();
            
            this.grid = new grid_viewer();
            this.grid.sql_source     = "sp_get_3D_Details~" + document.getElementById("ddl_3D_Status_Code").value + "~" + document.getElementById("txtCouVal").value + "~" + document.getElementById("ddl_3D_search_currency").value + "~" + document.getElementById("txtRemDateFrom").value + "~" + document.getElementById("txtRemDateTo").value;
            this.grid.div_container  = document.getElementById("div_courier_search");
            this.grid.datarow_column = 0;
            this.grid.open_grid_view();
            this.grid = null;
            document.getElementById('tbl_search_3D').style.display = 'block';
        }
    }
    
    this.display_update = function()
    {
        //search_3D.invisible('TRANDETAILS');
        search_3D.disable('btnSave');
        search_3D.disable('btnUpdateCancel');
        search_3D.visible('TRANXUPDATES');
    }
    
    this.returndetails = function()
    {
        search_3D.enable('btnSave');
        search_3D.enable('btnUpdateCancel');
        search_3D.invisible('TRANXUPDATES');
    }
    
    this.display_search = function()
    {
        this.grd_source  = "sp_get_3D_Details~" + document.getElementById("ddl_3D_Status_Code").value + "~" + document.getElementById("txtCouVal").innertext + "~" + document.getElementById("ddl_3D_search_currency").value + "~" + document.getElementById("txtRemDateFrom").value + "~" + document.getElementById("txtRemDateTo").value;
        this.grd_search  = new grid_selector();
        this.grd_search.open_selection_grid(null, this.grd_source, 0 , null);
        search_3D.invisible('TRANXUPDATES'); search_3D.visible('SEARCH3D');
    }
    
    this.load_details_callback = function(result)
    {
        if (result.error != null)
        {
            alert(result.error);
        }
        else
        {  
            var ds = result.value;
            
            if(ds != null && ds.Tables[0].Rows.length > 0)
            {                                 
                var dt = ds.Tables[0];            
                document.getElementById('lblReference').innerText = (dt.Rows[0].ReferenceNo == null) ? "" : dt.Rows[0].ReferenceNo.toString();
                document.getElementById('lblRefnumTheirs').innerText = (dt.Rows[0].ExtlReferenceNo == null) ? "" : dt.Rows[0].ExtlReferenceNo.toString();
                document.getElementById('lblStatusDate').innerText = (dt.Rows[0].RemittanceDate == null) ? "" : dt.Rows[0].StatusDate.toString();
                document.getElementById('lblRemittanceDate').innerText = (dt.Rows[0].TranxDate == null) ? "" : dt.Rows[0].TranxDate.toString();
                document.getElementById('lblProductName').innerText = (dt.Rows[0].fldcode == null) ? "" : dt.Rows[0].fldcode.toString();
                document.getElementById('lblTransactionDate').innerText = (dt.Rows[0].flddescription == null) ? "" : dt.Rows[0].flddescription.toString();
                document.getElementById('lblPayeeBank').innerText = (dt.Rows[0].branchname == null) ? "" : dt.Rows[0].branchname.toString();
                document.getElementById('lblAccountCurrency').innerText = (dt.Rows[0].AccountCurrency == null) ? "" : dt.Rows[0].AccountCurrency.toString();
                document.getElementById('lblRemitterName').innerText = (dt.Rows[0].RemitterName == null) ? "" : dt.Rows[0].RemitterName.toString();
                document.getElementById('lblRemitterAddress').innerText = (dt.Rows[0].RemitterAddress == null) ? "" : dt.Rows[0].RemitterAddress.toString();
                document.getElementById('lblBeneficiaryName').innerText = (dt.Rows[0].BeneficiaryName == null) ? "" : dt.Rows[0].BeneficiaryName.toString();
                document.getElementById('lblBeneficiaryAddress').innerText = (dt.Rows[0].BeneficiaryAddress == null) ? "" : dt.Rows[0].BeneficiaryAddress.toString();
                document.getElementById('lblTransactionAmount').innerText = (dt.Rows[0].TransactionAmount == null) ? "" : dt.Rows[0].TransactionAmount.toString();
                document.getElementById('lblStatus').innerText = (dt.Rows[0].TranStatus == null) ? "" : dt.Rows[0].TranStatus.toString();
                document.getElementById('lblCou_Reasons').innerText = (dt.Rows[0].Reason == null) ? "" : dt.Rows[0].Reason.toString();
                document.getElementById('lblCou_Remarks').innerText = (dt.Rows[0].Remarks == null) ? "" : dt.Rows[0].Remarks.toString();
                document.getElementById('lblMessage').innerText = (dt.Rows[0].SpecialMessage == null) ? "" : dt.Rows[0].SpecialMessage.toString();
                document.getElementById('lblBeneContact').innerText = (dt.Rows[0].ContactNo == null) ? "" : dt.Rows[0].ContactNo.toString();                
                
                courier.txn_seq_id = dt.Rows[0].txn_seq_id.toString();
             }
        }
    }
    
    this.saveupdate = function(seq_id)
    {
        var data_row = new Array();

        data_row[data_row.length] = "Ret_Reason~" + document.getElementById("ddl_3D_Reason").value;
        data_row[data_row.length] = "Remarks~" + document.getElementById("txtRemarks").value;
        data_row[data_row.length] = "ID_Received~" + document.getElementById("txtIDReceived").value;
        data_row[data_row.length] = "received_by~" + document.getElementById("txtReceivedBy").value;
        data_row[data_row.length] = "date_processed~" + document.getElementById("txtDateUpdated").value;
        
        IRSWorld._3D.courier_record_update.update_record(document.getElementById("ddl_Courier_Status").value, data_row.join("|"),parseInt(seq_id) ,this.update_record_callback);   
    }
    
    this.update_record_callback = function(result)
    {
        this.icls = this;
        
        if (result != null && result.value != null)
        {
            var s = result.value.split('~');
            
            if (parseInt(s[0]) == 1)
            {
                alert(s[1]);
            }
            else
            {
                alert("TRANSACTION UPDATED");
                icls.saveremittance(icls.txn_seq_id);
            }
        }
    }
    
    this.saveremittance = function(seq_id)
    {
        var data_row = new Array();
            
        data_row[data_row.length] = "Transtatus~" + document.getElementById("ddl_Courier_Status").value;

        IRSWorld._3D.remittance_update.update_remittance(document.getElementById("ddl_3D_search_currency").value, document.getElementById("ddl_Courier_Status").value, document.getElementById("txtCouVal").innertext, document.getElementById('lblTransactionAmount').innerText, data_row.join("|"),parseInt(seq_id) ,this.saveremittance_callback);   
    }
    
    this.saveremittance_callback = function(result)
    {
        
        courier.do_cancel();
    }
    
    this.exec_modal_summary = function(param)
    {
        var command_param = param.split("$");

        if (command_param != null)
        {
            if (command_param[0].toUpperCase() == "AJX_SELECTED")
            {
                
                if (document.getElementById("ddl_3D_Status_Code").value == "262" || document.getElementById("ddl_3D_Status_Code").value == "263")
                {
                    search_3D.disable('btnSave');
                }
                else
                {
                    search_3D.enable('btnSave');
                }
                
                this.sequence_id = command_param[1];
                this.sequence_id = command_param[1];
                
                search_3D.invisible('SEARCH3D'); 
                search_3D.visible('TRANDETAILS'); 
                IRSWorld._3D.Courier_3D_List.load_details(parseInt(command_param[1]), this.load_details_callback);
                
                //this.grd_search.process_ajax(param);
            }
        }

        //this.grd_search.process_ajax(param);
    }   
}

function summary_3D()
{
    var client_search     = this;

    this.outer_modal_div  = null;
    this.inner_modal_div  = null;
    this.commons          = null;
    
    this.open_search = function()
    {
        this.open_modal_window();
        this.modal_client_search();
    }
}

function search_3D()
{
    this.load_search_currency = function()
    {
        document.getElementById("txtRemDateFrom").innerText = new common_method().to_mmddyyyy_date(new Date());
        document.getElementById("txtRemDateTo").innerText = new common_method().to_mmddyyyy_date(new Date());
        document.getElementById("txtDateUpdated").innerText = new common_method().to_mmddyyyy_date(new Date());
        this.invisible('TRANDETAILS');
        this.invisible('TRANXUPDATES');
        IRSWorld._3D.Courier_3D_List.load_currency(this.load_search_currency_callback);
        IRSWorld._3D.Courier_3D_List.load_Courier_3D_Status(this.load_Courier_Status_callback);
        IRSWorld._3D.Courier_3D_List.load_Courier(this.load_courier_callback);
        IRSWorld._3D.Courier_3D_List.load_3D_Status(this.load_3D_Status_callback);
        IRSWorld._3D.Courier_3D_List.load_Reasons(this.load_Reasons_callback);
        IRSWorld._3D.Courier_3D_List.load_courier_value(this.load_courier_value_callback);
    }
    
    this.enable = function(param)
    {
        var id = document.getElementById(param);
        var cset = id.disabled;
       
        id.disabled = cset == "false";
    }  
    
    this.disable = function(param)
    {        
        var id = document.getElementById(param);
        var cset = id.disabled;
       
        id.disabled = (cset == "false") ? "true" : "false";
    }  
    
    this.invisible = function(param)
    {
        document.getElementById(param).style.display = 'none';
    }
    
    this.visible = function(param)
    {
        document.getElementById(param).style.display = 'block';
    }
    
    this.load_search_currency_callback = function(result)
    {
        var ddl = new common_method(); ddl.to_load_dropdownlist("ddl_3D_search_currency", result.value, "fldvalue", "flddescription", null);
    }
    
    this.load_Reasons_callback = function(result)
    {
        var ddl = new common_method(); ddl.to_load_dropdownlist("ddl_3D_Reason", result.value, "fldvalue", "flddescription", null);
    }
    
    this.load_3D_Status_callback = function(result)
    {
        var ddl = new common_method(); ddl.to_load_dropdownlist("ddl_3D_Status_Code", result.value, "fldvalue", "flddescription", null);
    }
    
    this.load_Courier_Status_callback = function(result)
    {
        var ddl = new common_method(); ddl.to_load_dropdownlist("ddl_Courier_Status", result.value, "fldvalue", "flddescription", null);
    }
    
    this.load_courier_callback = function(result)
    {
        //var ddl = new common_method(); ddl.to_load_dropdownlist("ddlCourier", result.value, "alias", "Courier", null);
        document.getElementById('txtCourier').innerText = "";
        document.getElementById('txtCourier').innerText = result.value;
    }
    
    this.load_courier_value_callback = function(result)
    {
        //var ddl = new common_method(); ddl.to_load_dropdownlist("ddlCourier", result.value, "alias", "Courier", null);
        document.getElementById('txtCouVal').innerText = "";
        document.getElementById('txtCouVal').innerText = result.value;
    }
    
}
