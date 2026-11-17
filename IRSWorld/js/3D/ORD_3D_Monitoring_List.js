var ORD3D = new ORD3D();
var search_ORD3D = new search_ORD3D();
var isbatch=0;
function execute_ajaxgridview_command(param)
{
    if (ORD3D != null)
    {
        ORD3D.extract_table_click(param);
    }
}

function fixht()
{
    document.getElementById('divBody').style.height = (top.document.getElementById('divCenter').offsetHeight - 30) + 'px';
    document.getElementById('divBody').style.width = (top.document.getElementById('divCenter').offsetWidth - 3) + 'px';
}

function showPanel(divName)
{
    //isbatch = 1;
    var div = document.getElementById(divName);
    
    divmask = new maskpanel();
    divmask.show();
    
    document.getElementById(divName).style.display = "";
    
    div.style.top = (((document.documentElement.clientHeight / 2) + document.documentElement.scrollTop) - (div.offsetHeight / 2)) + 'px';
    div.style.left = (((document.documentElement.clientWidth / 2) + document.documentElement.scrollLeft) - (div.offsetWidth / 2)) + 'px';
}

function hidePanel(divName)
{
    document.getElementById(divName).style.display = "none";
    divmask.hide();
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

//function showPanel(divName)
//{
//    
//    var div = document.getElementById(divName);
//    
//    divmask = new maskpanel();
//    divmask.show();
//    
//    document.getElementById(divName).style.display = "block";
//    
//    div.style.top = (((document.documentElement.clientHeight / 2) + document.documentElement.scrollTop) - (div.offsetHeight / 2)) + 'px';
//    div.style.left = (((document.documentElement.clientWidth / 2) + document.documentElement.scrollLeft) - (div.offsetWidth / 2)) + 'px';
//}

//function hidePanel(divName)
//{
//    document.getElementById(divName).style.display = "none";
//    divmask.hide();
//}


    
function ORD3D()
{
    var dbgrid;
    var icls = this;
    this.summary = null;
    this.grd_source = null;
    this.grd_search = null;
    this.display_grid = null;
    this.sequence_id = null;
    this.is_batch_send = false;
    
    this.do_cancel = function()
    {
        this.hide_ORD3D_summary('tbl_search3D');
        search_ORD3D.invisible('TRANORDDETAILS');
        search_ORD3D.invisible('TRANXORDUPDATES');
        search_ORD3D.visible('ORDSEARCH3D'); 
        this.load_popup_summary()
    }
    
    this.send_txn = function()
    {
        this.is_batch_send = true;
        this.savefordelivery(parseInt(this.sequence_id));
    }
    
    this.savefordelivery = function(seq_id)
    {
        var data_row = new Array();

        data_row[data_row.length] = "Transtatus~" + "257";
        
        IRSWorld._3D.ORDRemittance_update.update_remittance_ORD3D(document.getElementById("ddl_ORD3D_search_currency").value,"257", document.getElementById("ddlORDCourier").value, document.getElementById('lblORDTransactionAmount').innerText, data_row.join("|"),parseInt(seq_id) ,this.send_to_courier_callback);   
    }
    
    this.savefordelivery_batch = function(seq_id,amount)
    {
        var data_row = new Array();

        data_row[data_row.length] = "Transtatus~" + "257";
        
        IRSWorld._3D.ORDRemittance_update.update_remittance_ORD3D(document.getElementById("ddl_ORD3D_search_currency").value,"257", document.getElementById("ddlORDCourier").value, amount, data_row.join("|"),parseInt(seq_id) ,this.send_to_courier_callback);   
    }
    
    
    
    this.send_to_courier_callback = function(result)
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
                if(ORD3D.is_batch_send==true)
                {
                    alert("TRANSACTION SUCCESSFULLY SENT TO COURIER.");
                }
                this.is_batch_send = false;
                this.sequence_id = 0;
                icls.save_sent(icls.txn_seq_id);
                ORD3D.populate_rec();
            }
        }
    }
    
    this.save_sent = function(seq_id)
    {
        var data_row = new Array();
            
        data_row[data_row.length] = "Transtatus~" + "257";

        IRSWorld._3D.ORDRemittance_update.update_remittance_ORD3D(document.getElementById("ddl_ORD3D_search_currency").value,"257", document.getElementById("ddlORDCourier").value, document.getElementById('lblORDTransactionAmount').innerText, data_row.join("|"),parseInt(seq_id) ,this.save_sent_callback);   
    }
    
    this.save_sent_callback = function(seq_id)
    {
        ORD3D.do_cancel();
    }
    
    // Send all to Courier
    this.send_to_all = function()
    {
        if (document.getElementById("ddl_ORD3D_Status_Code").value == "258" | document.getElementById("ddl_ORD3D_Status_Code").value == "265")
        {
//            IRSWorld._3D.ORDRemittance_update.sendall( document.getElementById("ddlORDCourier").value,document.getElementById("ddl_ORD3D_search_currency").value,document.getElementById("ddl_ORD3D_Status_Code").value, this.save_sent_callback);
            alert('Transactions Successfully Sent!');
        }
        else
        {
            alert('Transaction Status not valid for sending to courier!');
        }
    }
    
    this.update = function()
    {
        if(document.getElementById("ddlORDCourier").value == document.getElementById("ddl_ORD3D_CourierTo").value)
        {
            alert("Please re-assign to other courier!");
        }
        else
        {
            if(isbatch == 1)
            {
                this.update_reassign_batch();
            }
            else    
            {
                this.saveupdate(parseInt(this.sequence_id));
            }
        }
    }
    
    
    this.update_reassign_batch= function()
    {
        var amt = 0;
        var x=0;
        if(document.getElementById("ddlORDCourier").value == document.getElementById("ddl_ORD3D_CourierTo").value)
        {
            alert("Please re-assign to other courier!");
        }
        else
        {
            document.getElementById('txtORDCourierFrom').value = document.getElementById('ddlORDCourier').value;
            for(i=0;i<=dbgrid.datatable.Rows.length - 1;i++)
            {
                if(dbgrid.getValueAt(i,0) == true | dbgrid.getValueAt(i,0) == 'true')
                {   
                    x+=1;
                    amt += parseFloat(dbgrid.datatable.Rows[i].NetToBeneAmount.toString());
                    this.saveupdate(parseInt(dbgrid.datatable.Rows[i].tag));
                }
            }
            if(x==0)
            {
                alert('Please select a record');
                return;
            }
            else
            {
                alert("Successfully re-assigned to courier " +  document.getElementById("ddl_ORD3D_CourierTo").value  + "\n Total count: " + i + "\n" + "Total Amount: " + amt);
                this.populate_rec();
                x=0;
                isbatch = 0;
            }
            
        }
    }
    
    this.open_summary = function()
    {
        this.summary = new summary_3D();        
        this.summary.open_search();        
    }
    
    this.batch_send_to_courier = function()
    {
        var amt=0;
        var i=0;
        var x=0;
        for(i=0;i<=dbgrid.datatable.Rows.length - 1;i++)
        {
            if(dbgrid.getValueAt(i,0) == true | dbgrid.getValueAt(i,0) == 'true')
            {   
                x+=1;
                amt += parseFloat(dbgrid.datatable.Rows[i].NetToBeneAmount.toString());
                this.savefordelivery_batch(dbgrid.datatable.Rows[i].tag,dbgrid.datatable.Rows[i].NetToBeneAmount.toString());
            }
        }
        if(x==0)
        {
            alert('Please select a record');
            return;
        }
        else
        {
            alert("Successfully sent to courier \n" + "Total count: " + i + "\n" + "Total Amount: " + amt);
            this.populate_rec();
            x=0;
        }
        
        
    }
    
       
    this.load_popup_summary = function()    
    {
        if  (
            document.getElementById("ddl_ORD3D_Status_Code").value == null | document.getElementById("ddl_ORD3D_Status_Code").value == "null" | 
            document.getElementById("ddlORDCourier").value == null | document.getElementById("ddlORDCourier").value == "null" | 
            document.getElementById("ddl_ORD3D_search_currency").value == null | document.getElementById("ddl_ORD3D_search_currency").value == "null" | 
            document.getElementById("txtORDRemDateFrom").value == null | document.getElementById("txtORDRemDateFrom").value == "null" | document.getElementById("txtORDRemDateFrom").value == "" |
            document.getElementById("txtORDRemDateTo").value == null | document.getElementById("txtORDRemDateTo").value == "null" | document.getElementById("txtORDRemDateTo").value == ""
            )
        {
            alert("Please complete fields for searching!");
        }
        else
        {
        
        
//            this.grid = new grid_viewer();
//            this.grd_search  = new grid_selector();       
//            this.grid.sql_source     = "sp_get_3D_Details~" + document.getElementById("ddl_ORD3D_Status_Code").value + "~" + document.getElementById("ddlORDCourier").value + "~" + document.getElementById("ddl_ORD3D_search_currency").value + "~" + document.getElementById("txtORDRemDateFrom").value + "~" + document.getElementById("txtORDRemDateTo").value;
//            this.grid.div_container  = document.getElementById("div_courier_search");
//            this.grid.datarow_column = 0;
//            this.grid.page_size = 10;
//            this.grid.process_ajax('ajx_loading$');                               
//            this.grid = null;

            this.populate_rec();
            
            //for viewing detailed
            this.show_ORD3D_summary('tbl_search3D');
        }
    }
    
    this.enable_button = function(bool)
    {
        for(var i=0;i<=dbgrid.datatable.Rows.length - 1;i++)
        {
            var id =  'grids_tr_' + i;
            var tr = document.getElementById(id);
            if(tr != null)
            {

                dbgrid.setCellsToEditMode(tr, !bool);
                dbgrid.row.selected_data_row = i;
                dbgrid.setColumn(12,bool);
            }
        }
    }
    
    
    this.bind_grid = function(result)
    {
        if (result.error != null)
        {
            alert(result.error);
            return;
        }
        ORD3D.create_grid();
        dbgrid.bind(result.value, 'div_courier_search', 'grids');
        ORD3D.enable_button(true);
    }
    
    this.check_all = function(bool)
    {
        for(var i=0;i<=dbgrid.datatable.Rows.length - 1;i++)
        {
            var id =  'grids_tr_' + i;
            var tr = document.getElementById(id);
            if(tr != null)
            {
                if(bool) 
                {
                    document.getElementById('btnSendAll').disabled = '';
                    document.getElementById('btnReassign').disabled = '';
                }

                dbgrid.setCellsToEditMode(tr, !bool);
                dbgrid.row.selected_data_row = i;
                dbgrid.setColumn(0,bool);
            }
            //dbgrid.row.selected = grow;
            //dbgrid.row.selected_data_row = i;
        }
    }
    
    
    this.create_grid = function()
    {
        dbgrid = new datagrid();
        {
            var col = new datagrid_column();
            col.header = 'check';
            col.datafield = 'checked';
            col.align = 'left';
            col.type= 'check';
            col.styles.cell += 'width:10%';
            col.styles.datafield += 'width:100%;';
            dbgrid.addColumn(col);
        }
        {
            var col = new datagrid_column();
            col.header = 'Reference Number';
            col.datafield = 'REFERENCE NUMBER';
            col.align = 'left';
            col.styles.cell += 'width:auto';
            col.editable = false;
            dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column();
	        col.header = 'Reference Number (Theirs)';
	        col.datafield = 'REFERENCE NUMBER (THEIRS)';
	        col.align = 'left';
	        col.editable = false;
	        col.styles.cell += 'width:auto';
	        dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column();
	        col.header = 'Remitting Branch';
	        col.datafield = 'REMITTING BRANCH';
	        col.align = 'left';
	        col.editable = false;
	        col.styles.cell += 'width:auto';
	        dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column();
	        col.header = 'Transaction Status';
	        col.datafield = 'TRANSACTION STATUS';
	        col.align = 'left';
	        col.editable = false;
	        col.styles.cell += 'width:auto';
	        dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column();
	        col.header = 'Product Name';
	        col.datafield = 'PRODUCT NAME';
	        col.align = 'left';
	        col.editable = false;
	        col.styles.cell += 'width:auto';
	        dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column();
	        col.header = 'Transaction Date';
	        col.datafield = 'TRANSACTION DATE';
	        col.align = 'left';
	        col.editable = false;
	        col.styles.cell += 'width:auto';
	        dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column();
	        col.header = 'Remitter Name';
	        col.datafield = 'REMITTER NAME';
	        col.align = 'left';
	        col.editable = false;
	        col.styles.cell += 'width:auto';
	        dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column();
	        col.header = 'Beneficiary Name';
	        col.datafield = 'BENEFICIARY NAME';
	        col.align = 'left';
	        col.editable = false;
	        col.styles.cell += 'width:auto';
	        dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column();
	        col.header = 'Amount';
	        col.datafield = 'NetToBeneAmount';
	        col.align = 'left';
	        col.editable = false;
	        col.styles.cell += 'width:auto';
	        dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column();
	        col.header = 'SEQ ID';
	        col.styles.header += 'display:none;';
	        col.styles.datafield += 'display:none;';
	        col.datafield = 'tag';
	        col.align = 'left';
	        col.styles.cell += 'width:8%;display:none;';
	        dbgrid.addColumn(col);
        }
        {
            var col = new datagrid_column();
	        col.header = '';
	        col.datafield = '';
	        col.align = 'center';
	        col.type = 'button';
	        col.value = 'Edit';
	        col.edit_button = true;
	        col.styles.cell += 'width:2%';
	        col.styles.datafield += 'width:100%;';
	        dbgrid.addColumn(col);
        }
        {
            var col = new datagrid_column();
	        col.header = '';
	        col.datafield = '';
	        col.align = 'center';
	        col.type = 'button';
	        col.value = 'View';
	        col.onclick = 'ORD3D.navigate';
	        col.styles.cell += 'width:2%';
	        col.styles.datafield += 'width:100%;';
	        dbgrid.addColumn(col);
        }

      
        
        dbgrid.caption = '3D';
        dbgrid.classNames.table = 'table_body';
        dbgrid.rows_page = 10;
        dbgrid.row.normal_style = 'background-color: #f7ffff;';
    }
    
    this.navigate = function(args)
    {
        search_ORD3D.invisible('ORDSEARCH3D'); 
        search_ORD3D.visible('TRANORDDETAILS'); 
        this.sequence_id = parseInt(dbgrid.getValueAt(args.datarow,10));
        IRSWorld._3D.ORD_3D_Monitoring_List.load_details_ORD3D(parseInt(dbgrid.getValueAt(args.datarow,10)), this.load_details_callback);
    }
    
    
    this.populate_rec = function()
    {
        //document.getElementById("ddl_ORD3D_Status_Code").value + "~" + document.getElementById("ddlORDCourier").value + "~" + document.getElementById("ddl_ORD3D_search_currency").value + "~" + document.getElementById("txtORDRemDateFrom").value + "~" + document.getElementById("txtORDRemDateTo").value;
        IRSWorld._3D.ORD_3D_Monitoring_List.load_3d_details_courier(document.getElementById("ddl_ORD3D_Status_Code").value,document.getElementById("ddlORDCourier").value ,document.getElementById("ddl_ORD3D_search_currency").value , document.getElementById("txtORDRemDateFrom").value ,document.getElementById("txtORDRemDateTo").value,this.bind_grid);
        
    }
    
    
    
    this.extract_table_click = function(param)
    {
        var command_param = param.split("$");

        if (command_param != null)
        {
            if (command_param[0].toUpperCase() == "AJX_SELECTED")
            {
                if (document.getElementById("ddl_ORD3D_Status_Code").value == "258" | document.getElementById("ddl_ORD3D_Status_Code").value == "265")
                {
                     search_ORD3D.enable('btnORDSend');
                }
                else
                {
                     search_ORD3D.disable('btnORDSend');
                }
                
                if (document.getElementById("ddl_ORD3D_Status_Code").value == "258" | document.getElementById("ddl_ORD3D_Status_Code").value == "263")
                {
                    search_ORD3D.enable('btnORDReassign');
                }
                else
                {
                    search_ORD3D.disable('btnORDReassign');
                }
                this.sequence_id = command_param[1];
                
                search_ORD3D.invisible('ORDSEARCH3D'); 
                search_ORD3D.visible('TRANORDDETAILS'); 
                IRSWorld._3D.ORD_3D_Monitoring_List.load_details_ORD3D(parseInt(command_param[1]), this.load_details_callback);
            }
        }
    }
    
    this.show_ORD3D_summary = function(param)
    {
        document.getElementById(param).style.display = 'block';
    }
    
    this.hide_ORD3D_summary = function(param)
    {
        document.getElementById(param).style.display = 'none';
    }
  
    this.display_update = function()
    {
        isbatch = 0;
//        search_ORD3D.disable('btnORDSend');
//        search_ORD3D.disable('btnORDReassign');
//        search_ORD3D.disable('btnORD3DCancel');
//        //search_ORD3D.invisible('TRANORDDETAILS'); 
        //search_ORD3D.visible('TRANXORDUPDATES');
        showPanel('TRANXORDUPDATES');
    }
    
    this.popup_update = function()
    {
        isbatch = 1;
        showPanel('TRANXORDUPDATES');
    }
    
    
    
    
    
    this.returndetails = function()
    {
        hidePanel('TRANXORDUPDATES');
        search_ORD3D.enable('btnORDSend');
        search_ORD3D.enable('btnORDReassign');
        search_ORD3D.enable('btnORD3DCancel');
        //search_ORD3D.visible('TRANORDDETAILS'); 
        search_ORD3D.invisible('TRANXORDUPDATES');
    }
    
    this.display_search = function()
    {
        this.grd_source  = "sp_get_3D_Details~" + document.getElementById("ddl_ORD3D_Status_Code").value + "~" + document.getElementById("ddlORDCourier").value + "~" + document.getElementById("ddl_ORD3D_search_currency").value + "~" + document.getElementById("txtORDRemDateFrom").value + "~" + document.getElementById("txtORDRemDateTo").value;
        this.grd_search  = new grid_selector();
        this.grd_search.open_selection_grid(null, this.grd_source, 0 , null);
        search_ORD3D.invisible('TRANXORDUPDATES'); search_ORD3D.visible('ORDSEARCH3D');
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
            var cbo = document.getElementById('ddlORDCourier');
            if(ds != null && ds.Tables[0].Rows.length > 0)
            {                                 
                var dt = ds.Tables[0];            
                document.getElementById('lblORDReference').innerText = (dt.Rows[0].ReferenceNo == null) ? "" : dt.Rows[0].ReferenceNo.toString();
                document.getElementById('lblORDRefnumTheirs').innerText = (dt.Rows[0].ExtlReferenceNo == null) ? "" : dt.Rows[0].ExtlReferenceNo.toString();
                document.getElementById('lblORDStatusDate').innerText = (dt.Rows[0].RemittanceDate == null) ? "" : dt.Rows[0].StatusDate.toString();
                document.getElementById('lblORDRemittanceDate').innerText = (dt.Rows[0].TranxDate == null) ? "" : dt.Rows[0].TranxDate.toString();
                document.getElementById('lblORDProductName').innerText = (dt.Rows[0].fldcode == null) ? "" : dt.Rows[0].fldcode.toString();
                document.getElementById('lblORDTransactionDate').innerText = (dt.Rows[0].flddescription == null) ? "" : dt.Rows[0].flddescription.toString();
                document.getElementById('lblORDPayeeBank').innerText = (dt.Rows[0].branchname == null) ? "" : dt.Rows[0].branchname.toString();
                document.getElementById('lblORDAccountCurrency').innerText = (dt.Rows[0].AccountCurrency == null) ? "" : dt.Rows[0].AccountCurrency.toString();
                document.getElementById('lblORDRemitterName').innerText = (dt.Rows[0].RemitterName == null) ? "" : dt.Rows[0].RemitterName.toString();
                document.getElementById('lblORDRemitterAddress').innerText = (dt.Rows[0].RemitterAddress == null) ? "" : dt.Rows[0].RemitterAddress.toString();
                document.getElementById('lblORDBeneficiaryName').innerText = (dt.Rows[0].BeneficiaryName == null) ? "" : dt.Rows[0].BeneficiaryName.toString();
                document.getElementById('lblORDBeneficiaryAddress').innerText = (dt.Rows[0].BeneficiaryAddress == null) ? "" : dt.Rows[0].BeneficiaryAddress.toString();
                document.getElementById('lblORDTransactionAmount').innerText = (dt.Rows[0].TransactionAmount == null) ? "" : dt.Rows[0].TransactionAmount.toString();
                document.getElementById('lblORDStatus').innerText = (dt.Rows[0].TranStatus == null) ? "" : dt.Rows[0].TranStatus.toString();
                document.getElementById('txtORDCourierFrom').innerText = cbo[cbo.selectedIndex].innerText;
                document.getElementById('lblSpecialMesg').innerText = (dt.Rows[0].SpecialMessage == null) ? "" : dt.Rows[0].SpecialMessage.toString();
                document.getElementById('lblContact').innerText = (dt.Rows[0].ContactNo == null) ? "" : dt.Rows[0].ContactNo.toString();
                                
                ORD3D.txn_seq_id = dt.Rows[0].txn_seq_id.toString();
             }
        }
    }
    
    this.saveupdate = function(seq_id)
    {
       
        var data_row = new Array();

        data_row[data_row.length] = "from_courier~" + document.getElementById("txtORDCourierFrom").value;
        data_row[data_row.length] = "to_courier~" + document.getElementById("ddl_ORD3D_CourierTo").value;
        data_row[data_row.length] = "date_processed~" + new common_method().to_mmddyyyy_date(new Date());
        
        IRSWorld._3D.ORDCourier_record_update.update_record_ORD3D("265",data_row.join("|"),parseInt(seq_id) ,this.update_record_callback);   
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
                //alert("TRANSACTION RE-ASSIGNED SUCCESSFULLY.");
                hidePanel('TRANXORDUPDATES');
                icls.saveremittance(icls.txn_seq_id);
            }
        }
    }

    this.saveremittance = function(seq_id)
    {
        var data_row = new Array();
            
        data_row[data_row.length] = "Transtatus~" + "265";

        IRSWorld._3D.ORDRemittance_update.update_remittance_ORD3D(document.getElementById("ddl_ORD3D_search_currency").value,"265", document.getElementById("ddlORDCourier").value, document.getElementById('lblORDTransactionAmount').innerText, data_row.join("|"),parseInt(seq_id) ,this.saveremittance_callback);   
    }
    
    this.saveremittance_callback = function(result)
    {
        ORD3D.do_cancel();
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

function search_ORD3D()
{
    this.load_search_currency = function()
    {
        document.getElementById("txtORDRemDateFrom").innerText = new common_method().to_mmddyyyy_date(new Date());
        document.getElementById("txtORDRemDateTo").innerText = new common_method().to_mmddyyyy_date(new Date());
        this.invisible('TRANORDDETAILS');
        this.invisible('TRANXORDUPDATES');
        IRSWorld._3D.ORD_3D_Monitoring_List.load_currency_ORD3D(this.load_search_currency_callback);
        IRSWorld._3D.ORD_3D_Monitoring_List.load_Courier_ORD3D(this.load_courier_callback);
        IRSWorld._3D.ORD_3D_Monitoring_List.load_3D_Status_ORD3D(this.load_3D_Status_callback);
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
        var ddl = new common_method(); ddl.to_load_dropdownlist("ddl_ORD3D_search_currency", result.value, "fldvalue", "flddescription", null);
    }
    
    this.load_3D_Status_callback = function(result)
    {
        var ddl = new common_method(); ddl.to_load_dropdownlist("ddl_ORD3D_Status_Code", result.value, "fldvalue", "flddescription", null);
    }
    
    this.load_courier_callback = function(result)
    {
        var ddlTo = new common_method(); ddlTo.to_load_dropdownlist("ddl_ORD3D_CourierTo", result.value, "alias", "Courier", null);
        var ddl = new common_method(); ddl.to_load_dropdownlist("ddlORDCourier", result.value, "alias", "Courier", null);
    }
}
