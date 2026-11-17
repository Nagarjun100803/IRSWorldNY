function execute_ajx_command(param)
{
    if (micc != null && micc != "undefined")
    {
        micc.do_gridsearch(param);
    }
}


function execute_ajaxgridview_command(param)
{
    if (MICCSConv != null && MICCSConv != "undefined")
    {
        MICCSConv.do_gridview(param);
    }
}


function getStatus()
{
    var dt =  IRSWorld.RITS.MICCS.getStatus("MICCSStatus").value.Tables[0];
    status = new Array();
    for(var i = 0;i < dt.Rows.length;i++)
    {
        status[status.length] = dt.Rows[i].FldDescription + '~' + dt.Rows[i].FldDescription ;
    }
}


 var status;
 var dbgrid = null;
 var datatable = null;
 var pg_no = 1;
   
    

function MICCSConv()
{
    var miccs_conv = this;
    
    this.ajax_grid = null;
    
   
    this.create_grid = function()
    {
        gv = new datagrid();    
        
        {
            var col = new datagrid_column();
            col.header = 'SEQ_ID';
            col.styles.header += 'display:none;';
            col.styles.datafield += 'display:none;';
            col.datafield = 'SEQ_ID';
            col.align = 'left';
            col.styles.cell += 'width:8%;display:none;';
            gv.addColumn(col);
        }
        {
            var col = new datagrid_column(); // 1
            col.header = 'ACCOUNT NO';
            col.datafield = 'AccountNo'; //used in alias SQL
            col.align = 'right';		    
            col.styles.cell += 'width:auto';
            col.editable = false;
            gv.addColumn(col);
        }
        {
            var col = new datagrid_column(); // 2
            col.header = 'TRANSACTION TYPE';
            col.datafield = 'TransactionType'; //used in alias SQL
            col.align = 'center';		    
            col.styles.cell += 'width:auto';
            col.editable = false;
            gv.addColumn(col);
        }
        
        {
            var col = new datagrid_column(); // 3
            col.align = 'left';
            col.header = 'REMITTER NAME';
            col.datafield = 'RemitterNameJPN';
            col.styles.cell += 'width:auto';
            col.editable = false;
            gv.addColumn(col);
        }
        {
            var col = new datagrid_column(); //4
            col.header = 'CONVERSION';
            col.datafield = 'ButtonCIF';
            col.styles.cell += 'width:auto';
            col.width = '10px';
            col.align = 'center';
            col.type = 'button';
            col.value = '>>';
            col.editable = true;
            col.onclick = "micc.show_explore";
            gv.addColumn(col);
            	
        }  
        
        {	        
            var col = new datagrid_column(); // 5
            col.header = 'REMITTER CIFID';
            col.styles.header += 'display:none;';
            col.datafield = 'RemitterCIFID'; //used in alias SQL
            col.styles.cell += 'display:none;';
            col.align = 'center';	
            col.type = 'text';    		
            col.styles.cell += 'width:auto';            		        
            gv.addColumn(col);		                    
        } 
        
        {	        
            var col = new datagrid_column(); // 6
            col.header = 'REMITTER NAME (conv)';
            col.datafield = 'RemitterNameCONV'; //used in alias SQL
            col.align = 'center';	
            col.type = 'text';	    		
            col.styles.cell += 'width:auto';		        
            gv.addColumn(col);		        
        }      
           

        {
            var col = new datagrid_column(); // 7
            col.align = 'left';
            col.header = 'AMOUNT RECEIVED';
            col.datafield = 'AmountReceived';
            col.styles.cell += 'width:auto';
            col.editable = false;
            gv.addColumn(col);
        } 
        {
            var col = new datagrid_column(); // 8
            col.header = 'Transaction Status';
            col.datafield = 'TransactionStatus';
            col.align = 'center';
            col.type = 'select';
            col.options = status;
            col.editable = true;
            col.styles.cell += 'width:auto';
            gv.addColumn(col);
        }               
            gv.footer.edit_button.none = false;            
            gv.footer.edit_button.style += 'background-color: lavender;'; 
            //gv.footer.edit_button.onclick = 'edit_data';              
            gv.footer.save_button.none = false; 
            gv.footer.save_button.onclick = 'save_data';  
            gv.footer.save_button.style += 'background-color: lavender;';  
            gv.footer.cancel_button.none = false;
            //gv.footer.cancel_button.onclick = ''; 
            gv.footer.cancel_button.style += 'background-color: lavender;'; 
            gv.row.normal_style = 'background-color: #f7ffff;';                                      
            return gv;
                        
    }
       
   
    this.hidePanel = function(divName)
    {
        document.getElementById(divName).style.display = "none";
        divmask.hide();
    }
    
    this.showPanel = function(divName)
    {
        var div = document.getElementById(divName);
        divmask = new maskpanel();
        divmask.show();
        
        document.getElementById(divName).style.display = "";
        
        div.style.top = (((document.documentElement.clientHeight / 2) + document.documentElement.scrollTop) - (div.offsetHeight / 2)) + 'px';
        div.style.left = (((document.documentElement.clientWidth / 2) + document.documentElement.scrollLeft) - (div.offsetWidth / 2)) + 'px';
    }
    
    this.show_explore = function(args)
    {
        this.showPanel('divSearch');
    }
        
    this.do_gridsearch = function(parameter)
    {
        if (parameter != null && parameter != "undefined")
        {
            var command_param = parameter.split("$");                
                
                if (command_param.length > 0)
                {
                    if (command_param[0] == "ajx_selected")
                    { 
                        var command_value = command_param[1].split('~');
                        //CIFID = command_value[0];
                        var  result = IRSWorld.RITS.MICCS.get_remitter(command_value[0]);
                        
                        var remitter_fullname = result.value.Tables[0].Rows[0].FullName;
                        var remitter_cifid = result.value.Tables[0].Rows[0].CIFID;
                        
//                        tablename + _ + elementname + _ + row number
                        var selected_row = dbgrid.row.selected_data_row;
                        dbgrid.setColumn(5, remitter_cifid);  
                        dbgrid.setColumn(6, remitter_fullname);                       
//                        document.getElementById("tblConfList_tr_" + selected_row.toString()).value = remitter_fullname;
                        
                        this.ajax_grid.process_ajax(parameter);
                        
                        return;
                                                       
                    }
                    else
                    {                 
                        this.ajax_grid.process_ajax(parameter); return;
                    }
                                  
                }
            
        }
    }
    

    
    this.check_miccs = function()
    {                
        IRSWorld.RITS.MICCS.checkMICCS(this.check_miccs_callback);        
    }    
    
    this.check_miccs_callback = function(res)
    {
    
        if (res.error != null)
        {
            alert(res.error.Message); return;
        }
    
    
        if (res != null && res.value != null)
        {
            var ds = res.value;
            var msg = 'Items Summary: \n';
            
            for(j = 0; j < ds.Tables[0].Rows.length; j++)
            {
                msg += ds.Tables[0].Rows[j].status + ': ' + ds.Tables[0].Rows[j].totitems + "\n";
            }                        
        
            var a = confirm(msg + '\n' + 'Do you want to continue processing items?');
            
            if (a == true)
            {
                micc.process_miccs();
            }
        }
    }
    
    
    this.process_miccs = function()
    {               
        IRSWorld.RITS.MICCS.processMICCS(this.process_miccs_callback);               
    }

    
    this.process_miccs_callback = function(res)
    {
        if (res.error != null)
        {
            alert(res.error.Message);
        }
        else
        {
            if (res.value != null && parseInt(res.value) > 0)
            {
                alert("Successfully matched items.");
            }
            else
            {
                alert("Items doesn't matched.");
            }
        }
        
        IRSWorld.RITS.MICCS.getConfirmationList(this.load_miccs_callback);
    }
    
    
        
    this.load_miccs = function()
    {
        getStatus();
        IRSWorld.RITS.MICCS.getConfirmationList(this.load_miccs_callback);
    }
    
    
    
    this.load_miccs_callback = function(res)
    {
        if (res.error != null)
        {
            alert(res.error.Message);
            return;
        }   
        try
        {
        dbgrid = miccs_conv.create_grid();
        datatable = res.value.Tables[0];        
        dbgrid.clearFilter();                    
        dbgrid.filter.enabled = false;  
        dbgrid.bind(datatable, 'tdConfList', 'tblConfList');              
        dbgrid.refresh(datatable);
        oldatatable = datatable;
            if (datatable.Rows.length > 0 )
            {
                try
                {
                    var ctr = cls.get_pagectr();
                    cls.dbgrid.pg_position(ctr);
                }
                catch(e)
                {
                    dbgrid.pg_position(1)
                }
             }
        }
        catch(e)
        {
            alert(e);
        }
    }
        
    this.load_status_grid_search = function(panel)
    {
        this.panel = panel;
        this.ajax_grid = new grid_selector();
        this.grid_ajax_query = 'sp_CIFSearch~1~1~~~~~~3~tok~';
        this.ajax_grid.open_selection_grid(this.grid_ajax_page,this.grid_ajax_query, 0, 0);
    }
    
    this.close_divisions = function()
    {
        document.getElementById("divMICCSBody").style.display = 'none';
        document.getElementById("divSearch").style.display = 'none';
    }
    
    this.open_division = function(division_id)
    {
        this.close_divisions(); document.getElementById(division_id).style.display = 'block';       
    }
    
    this.search_cif = function()
    {
        var firstname = document.getElementById('txtSearchFirstname').value;
        var lastname = document.getElementById('txtSearchLastname').value;
        var middlename = document.getElementById('txtSearchMiddlename').value;
        
        if((firstname == null || firstname == '') && (lastname == null || lastname == '') && (middlename == null || middlename == ''))
        {
            alert("Please enter any value to search!");
            showPanel('divSearch');
        }
        else
        {
            this.ajax_grid = new grid_selector();        
            this.grid_ajax_query = 'sp_CIFSearch~1~1~' +  lastname + '~' + firstname + '~' + middlename + '~~~3~tok~';
            if (this.grid_ajax_query != null)
            {
                this.ajax_grid.open_selection_grid(this.grid_ajax_page, this.grid_ajax_query, 3, null);                     
            }
        }
    }  
    
    
           
}
     
function save_data(args)
{
    if(args.value == dbgrid.footer.save_button.value)
    {
        IRSWorld.RITS.MICCS.applyEdits(args.additions, args.deletes, args.edits);
    }
          
     
}
