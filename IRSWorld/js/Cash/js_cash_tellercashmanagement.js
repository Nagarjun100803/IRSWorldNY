    var dbgrid;        
    var taropts = new Array(5);
    var save_row = -1;

    taropts[0] = 'center~Center';
    taropts[1] = '_popup~Popup';
    taropts[2] = '_top~Top';
    taropts[3] = '_parent~Parent';
    taropts[4] = '_self~Self';

    function cash_management()
    {
    

        this.commons = null;     
                
        this.load_ddlCurrency = function()
        {
            TellerCashManagement.getdropdown_ddlCurrency(this.load_ddlCurrency_callback);
        }
        
        this.load_ddlCurrency_callback = function(result)
        {

        
            if (result != null && result != "undefined")
            {
                document.getElementById("divDenomination").style.display  = "block";   
                document.getElementById("tblDenomination").style.display  = "block";   
                document.getElementById("tblGridDenomination").style.display  = "block";   
            
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlCurrency", result.value, "FldCode", "FldName");
                this.commons = null;
            }
        }
       
      
}   

         this.Denomination = function()
        {              
            document.getElementById("tblCurrency").style.display  = "block";    
            document.getElementById("divCashMain").style.display  = "block";     
            document.getElementById("divCurrency").style.display  = "block";    

        
        }          

   // Create Table Grid 
    function create_grid()
    {
        dbgrid = new datagrid();
        {
		    var col = new datagrid_column();
		    col.header = 'Currency';
		    col.datafield = 'currency';
		    col.align = 'left';
		    col.styles.cell += 'width:30%';
		    dbgrid.addColumn(col);
        }
        {
		    var col = new datagrid_column();
		    col.header = 'Denomination';
		    col.datafield = 'denomination';
		    col.align = 'left';
		    col.styles.cell += 'width:40%';
		    dbgrid.addColumn(col);
        }
       {
		    var col = new datagrid_column();
		    col.header = 'Quantity';
		    col.datafield = 'quantity_begin';
		    col.align = 'right';
		    col.styles.cell += 'width:20%';
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
		    col.styles.cell += 'width:10%';
		    col.styles.datafield += 'width:100%;';
		    dbgrid.addColumn(col);
        }               

        dbgrid.caption = 'Cash Breakdown';
        dbgrid.classNames.table = 'table_body';
        dbgrid.footer.add_button.none = false;
        dbgrid.footer.add_button.style += 'background-color: lavender;'; 
        dbgrid.footer.delete_button.none = false;
        dbgrid.footer.delete_button.style += 'background-color: lavender;color:maroon;';            
        dbgrid.footer.save_button.none = false;
        dbgrid.footer.save_button.onclick = 'grid_btn_click';  
        dbgrid.footer.save_button.style += 'background-color: lavender;';  
        dbgrid.footer.cancel_button.none = false;
        dbgrid.footer.cancel_button.style += 'background-color: lavender;';
        dbgrid.row.normal_style = 'background-color: #f7ffff;';          

    }

    // Get Table
    function gotCashMgt(cm)
    {
        if (cm.error != null)
        {
            spawn_errpage(cm.error);
            return;
        }
        
        create_grid();
        dbgrid.bind(cm.value, 'tdDenomination', 'tblCashM');
    }           
   
  function populate_denomination(fldCode)     
 {
        TellerCashManagement.getCashMgt(fldCode, gotCashMgt); 
 }  

  function close_session()
    {
        if (is_closing())
        {
            TellerCashManagement.close_session();
        }
        else
        {
            TellerCashManagement.drop_irsCashMgt();
        }
    }        
   
   // Add
   
   function updates_saved(cm)
    {   
        
        if (cm.error != null)
        {
            alert(cm.error);
            
            return;
        }
                
       populate_denomination(document.getElementById("ddlCurrency").value);
               
    }
           
    function grid_btn_click(args)
    {
        if (args.value == dbgrid.footer.save_button.value)
        {
            TellerCashManagement.applyEdits(args.additions, args.deletes, args.edits, updates_saved);
        }
    }
        
       
    
 
        
    
    
    