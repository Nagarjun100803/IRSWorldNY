function epsco_proc()
{
    this.dataTable = null;
    
    this.dbgrid = null;
    
    this.grid_header_caption = "";
    this.grid_btn_caption = "";
    this.grid_btn_enabled = true;

    this.page_initialize = function()
    {
        AjaxPro.timeoutPeriod = 1000 * 1000;
        AjaxPro.onTimeout = function()
        {
            alert("THE SERVER STOPPED RESPONDING. PLEASE TRY AGAIN");
            ajax_show_status(false);
        }
        
        showPanelEP('divPreProcessing');
        divmask.hide();
        epsco_proc.populate_data();
    }
    
    this.insert_into_remittance = function()
    {
        var data = "";
	    
	    for(var i=0; i<=9; i++)
        {
            if(epsco_proc.dbgrid.getValueAt(i + ((epsco_proc.dbgrid.curr_page - 1) * epsco_proc.dbgrid.rows_page),0))
            {   
                var seq_id =  epsco_proc.dbgrid.getValueAt(i + ((epsco_proc.dbgrid.curr_page - 1) * epsco_proc.dbgrid.rows_page), 9);    
                var result = IRSWorld.EPSCO.EPSCOProcessing.InsertToRemittance(seq_id);
        
                if(result.value != "") alert("Error encountered on transaction no. " + seq_id.toString());
            }
        }
        
        alert("Transaction processing has been completed.");

        epsco_proc.populate_data();         
    }
    
    this.populate_data = function()
	{   
	    var param = document.getElementById("txtUploadDate1").value + "~" + 
                    document.getElementById("txtUploadDate2").value + "~" +
                    (document.getElementById("chkTrapped").checked ? "1" : "0"); 
        
        ajax_show_status(true);            
	    IRSWorld.EPSCO.EPSCOProcessing.getDataTableValue("ksp_getEPSCOTransaction~" + param, epsco_proc.load_custom_grid);
	}
	
	this.populate_data_callback = function(args)
	{
	    epsco_proc.load_custom_grid(args.value);
	}
	
	this.load_custom_grid = function(dt)
	{
	
	    ajax_show_status(false);
	    
        epsco_proc.dbgrid = new datagrid();
        
        var cols = {1:'selected'
                    ,2:'EPSCO No.'
                    ,3:'Merchant Number'
                    ,4:'Remitter Name'
                    ,5:'Beneficiary Name'
                    ,6:'Target Account Number'
                    ,7:'Currency'
                    ,8:'Gross Amount'
                    ,9:'Transaction Date'
                    ,10:'SEQ_ID'};
                    

        for(var x in cols)
        {
            epsco_proc.dbgrid.addColumn(epsco_proc.create_column(cols[x],'left',(cols[x] == 'SEQ_ID' ? 'none' : '')));
        }
         
        epsco_proc.dbgrid.rows_page = 10;
        epsco_proc.dbgrid.bind(dt.value, 'tdTransactions', 'grdTransactions');
        
        if(epsco_proc.dbgrid.datatable.Rows.length == 0)
        {
            document.getElementById('btnSubmitTransactions').disabled = 'disabled';
        }
        else
        {
            document.getElementById('btnSubmitTransactions').disabled = '';
        }
        
        epsco_proc.enabled_checkbox('grdTransactions_tbody');
        
        document.getElementById('grdTransactions_pg_next').onclick = function(){epsco_proc.dbgrid.pg_next_clicked();epsco_proc.enabled_checkbox('grdTransactions_tbody');};
        document.getElementById('grdTransactions_pg_prev').onclick = function(){epsco_proc.dbgrid.pg_previous_clicked();epsco_proc.enabled_checkbox('grdTransactions_tbody');};
        document.getElementById('grdTransactions_pg_select').onclick = function(){epsco_proc.dbgrid.pg_select_clicked();epsco_proc.enabled_checkbox('grdTransactions_tbody');};
        
	}
	
    this.create_column = function(name, align, display)
	{
	    var col = new datagrid_column();
	    
	    col.header = (name == 'selected' ? " &nbsp; <br /><input type='checkbox' id='chkAll' style='background-color:transparent;border:none;width:30px;' onclick=javascript:epsco_proc.check_all(this.checked); />" : name);
    	col.datafield = name;
    	col.align = align;
    	col.styles.cell += (name == 'selected' ? 'width:30px;' : 'width:auto;');
    	col.styles.header += 'display:' + display + ";";
    	col.styles.datafield += 'display:' + display + ";";
    	col.styles.cell += 'display:' + display + ";";
    	
    	if(name == 'selected') col.type= 'check';
    	
    	return col;
	}
	
	this.enabled_checkbox = function(obj)
	{
	    var tbody = document.getElementById(obj);
        
        var e = tbody.getElementsByTagName("*");
             
        for(var i=0;i<e.length;i++)
        {
            if(e[i].type == "checkbox")
            {
              if(e[i].disabled) e[i].disabled = false;
            }
        }
	}
	
	this.check_all = function(isCheck)
    {
	   for(var i=0; i<=9; i++)
       {
           var childs = epsco_proc.dbgrid.getElementsAt(i + ((epsco_proc.dbgrid.curr_page - 1) * epsco_proc.dbgrid.rows_page),0);
           
           if(childs != null && childs != 'null')
           {
               for (var k=0; k < childs.length; k++)
               {
                    if (childs[k].type == 'checkbox')
                    {
                        childs[k].checked = isCheck;
                    } 
               }
           }
       }
	}
}
