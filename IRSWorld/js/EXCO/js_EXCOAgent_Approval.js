var js_EXCO = new js_EXCOAgent_Approval();
var js_EXCOpopup = new js_EXCO_popup();
var js_EXCOFP = new js_EXCO_ForcePush();

var dbgrid;
var dbgridItems;
var dbgridAmount;
var dsforApproval;
var dtItems;
var dtAmount;
var datatable;
var dtAgentFunds;
var dtCurrencyRates;
var CurrencyTable = new CurrTable();
var CurrencyFundsTable = new CurrTable();
var SelectTable =   new WorkingTable();
var drcrResult;
function CurrTable()
{
	this.Rows = new Array();
}

CurrTable.prototype.addRow = function(row)
{
	this.Rows.push(row);
}

function cRow()
{
	this.currency = '';		
}

function WorkingTable()
{
    this.Rows = new Array();
}
WorkingTable.prototype.AddRow = function(row)
{
//    var pass = false;
//    
//    
//    if (this.Rows.length > 0 )
//    {
//        for(i= 0; i < this.Rows.length; i++)
//        {
//           if (row.seq_id == Rows[i].seq_id) 
//           {
//            pass = true;
//           }
//        }
//    }
//    if (pass)
    this.Rows.push(row)
}
WorkingTable.prototype.RemoveRow = function(instanceindx)
{
    this.Rows.splice(instanceindx,1);
}
function WorkingRow()
{
    this.seq_id = '';
    this.currency = '';    
}
function js_EXCOAgent_Approval()
{   
    this.match_index            = -1;
    this.curr_pos               = -1;
    this.curr_agent_currency    = "";
    
    this.get_current_branch = function()
    {
        var hd = document.getElementById("hdBranch");
        
        return hd.value;
    } 
    this.set_current_branch = function(sbranch)
    {
        document.getElementById("hdBranch").value = sbranch;
    } 
    this.get_selected_agent = function()
    {
        var cbo = document.getElementById("ddlEXCOBranch");
        var indx = 0;
        if (cbo.length > 0)
        {
            indx = cbo.selectedIndex;
            return cbo[indx].value;
        }
        return cbo.value;
    }
    this.get_selected_agent_text = function()
    {
        var cbo = document.getElementById("ddlEXCOBranch");
        var indx = 0;
        if (cbo.length > 0)
        {
            indx = cbo.selectedIndex;
            return getText(cbo[indx]);
        }
        return getText(cbo.id);
   }
  
    this.set_selected_filedetails = function(sfiledtl)
    {
        document.getElementById("hdselectedfiledtls").value = sfiledtl;
    }
    
    this.get_exco_switch = function()
    {    
        return document.getElementById("hdexcoswitch").value;
    }
    this.set_exco_switch = function(sval)
    {
        document.getElementById("hdexcoswitch").value = sval;
    }
    this.display_filedetails = function()
    {
        var sfiledtls = document.getElementById("hdselectedfiledtls").value;
        var sarr      = sfiledtls.split(";");        
        setText("lblAgent",sarr[0]);
        setText("lblFileName",sarr[1]);
        setText("lblupload_date_by",sarr[2]);
    }
    this.clear_filedetails = function()
    {        
        setText("lblAgent","");
        setText("lblFileName","");
        setText("lblupload_date_by","");
    }
    this.get_selected_file = function()
    {
        return document.getElementById("hdselectedfile").value
    } 
    this.set_selected_file = function(fileseqid)
    {
        document.getElementById("hdselectedfile").value = fileseqid;
    }  
    this.get_selected_curr = function()
    {
        return document.getElementById("hdselectedcurr").value
    } 
    this.set_selected_curr = function(curr)
    {
        document.getElementById("hdselectedcurr").value = curr;
    } 
    this.get_selected_funds = function()
    {
        return document.getElementById("hdselectedfunds").value
    } 
    this.set_selected_funds = function(sfundsval)
    {
        document.getElementById("hdselectedfunds").value = sfundsval;
    }  
    this.init_checkboxes = function(args)
    {
        document.getElementById(args.id).disabled = false;
    }
    this.toggle_panels = function(optpnl)
    {
        // 1 filelist : 2 for approval : 3 Approval summary
        switch (optpnl)
        {
            case 2 :
                document.getElementById("divFileListing").style.display = "none";
                document.getElementById("divForApproval").style.display = "block";   
                document.getElementById("divFundsComputation").style.display = "none";
                document.getElementById("divTransDetail").style.display = "none";
                break;
            case 3 :
                document.getElementById("divFileListing").style.display = "none";
                document.getElementById("divForApproval").style.display = "none";   
                document.getElementById("divFundsComputation").style.display = "none";
                document.getElementById("divTransDetail").style.display = "block";
                break;
            default :                
                this.set_selected_curr(0);
                document.getElementById("divFileListing").style.display = "block";
                document.getElementById("divForApproval").style.display = "none";                
                document.getElementById("divFundsComputation").style.display = "none";
                document.getElementById("divTransDetail").style.display = "none";
                break;
        }
    }
    //
    this.check_currency_rates = function()
    {
        var hasrates = false;
        var branch = this.get_current_branch();
        js_EXCOpopup.open_msg_prompt("CHECKING CURRENCY RATES");
        var dt = IRSWorld.EXCO.EXCOAgent_Approval.get_exco_currency_rates(branch,2,1).value;  
        js_EXCOpopup.close_msg_prompt()
        if (dt.Rows.length > 0)
        hasrates = true;
        
        return hasrates;
    }
    this.check_setup =   function()
    {
        if (!this.check_currency_rates())
        {
            alert("PLEASE SETUP CURRENCY RATES.");
            return false;
        }
        return true;
    }
    this.init_state = function()
    {
        js_EXCOpopup.open_msg_prompt("GETTING BRANCH");
        var branchcode = IRSWorld.EXCO.EXCOAgent_Approval.get_logged_branch().value;
        js_EXCOpopup.close_msg_prompt();
        
        this.set_current_branch(branchcode);
        
        if (this.check_setup())
        {
            this.load_ddl_excobranches();    
            this.toggle_panels(4);    
            this.set_selected_file(0);         
        }
    }
    this.load_ddl_excobranches = function()
    {
        AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. GETTING OF EXCO BRANCH FAILED.';
            alert(errmsg);           return;  
        } 
        js_EXCOpopup.open_msg_prompt("LOADING EXCO BRANCHES");
        IRSWorld.EXCO.EXCOAgent_Approval.get_exco_branches(this.load_dll_excobranches_callback);
    }
    
    this.load_dll_excobranches_callback = function(result)
    {   
        js_EXCOpopup.close_msg_prompt();    
        if (result.value != null && result.value != undefined)
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddlEXCOBranch", null, "VALUE", "CODE",result.value);
        }
    }
    this.create_filelist_grid = function()
    {
         gv = new datagrid();             
         {
		    var col = new datagrid_column(); //0
		    col.align = 'left';
		    col.header = 'SEQ ID';
		    col.datafield = 'SEQ_ID';
            col.styles.cell += 'width:auto;display:none';
		    col.styles.header += 'display:none';
		    col.styles.datafield += 'display:none';
		    
		    gv.addColumn(col);
        }       
        {
		    var col = new datagrid_column(); // 2
		    col.header = 'FILENAME';
		    col.datafield = 'FLDFILENAME';
		    col.align = 'right';
		    col.styles.cell += 'width:auto';
		    col.editable = false;
		    gv.addColumn(col);
        }
        {
		    var col = new datagrid_column(); // 3
		    col.align = 'left';
		    col.header = 'UPLOAD DATE';
		    col.datafield = 'FLDUPLOADDATE';
		    col.editable = false;            
		    gv.addColumn(col);
        }
        {
		    var col = new datagrid_column(); // 4
		    col.header = 'UPLOADED BY';
		    col.datafield = 'FLDUPLOADEDBY';
		    col.align = 'left';
		    col.editable = false;
		    col.styles.cell += 'width:auto';
		    gv.addColumn(col);
        }       
            
            gv.row.onclick = 'js_EXCO.grid_click';                      
            gv.row.normal_style = 'background-color: #f7ffff;';                      
            return gv;
    }
    this.display_file_listing = function()
    {        
        AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. DISPLAYING OF FILES FAILED.';
            alert(errmsg);        return;     
        } 
        var branch = this.get_selected_agent();
        
        js_EXCOpopup.open_msg_prompt("GETTING FILES");
        
        IRSWorld.EXCO.EXCOAgent_Approval.get_exco_files(branch,this.display_file_listing_callback)
    }
    this.display_file_listing_callback = function(result)
    {
        js_EXCOpopup.close_msg_prompt();
        
        if (result.error != null)
        {
            spawn_errpage(result.error);
            return;
        }
        
        datatable = result.value;
        dbgrid = js_EXCO.create_filelist_grid()
        dbgrid.clearFilter();                    
        dbgrid.filter.enabled = false;  
        dbgrid.bind(datatable, 'tdEXCOfilelisting', 'tblEXCOfilelisting');
        
        if (datatable.Rows.length > 0 )
        {
            document.getElementById("btnView").disabled = false;
            document.getElementById("btnViewCancel").disabled = false;
        }
        else
        {
            document.getElementById("btnView").disabled = true;
            document.getElementById("btnViewCancel").disabled = true;
        }
    }
    this.btnView_click = function()
    {
        if (this.get_selected_file() == "0")
        {
            alert("PLEASE SELECT FILE.")
            return;
        }
        this.toggle_panels(2);
        this.display_for_approval();
    }
    this.btnViewCancel_click = function()
    {
        this.toggle_panels(4);
    }
    this.btnCancelApprove_click = function()
    {
       this.toggle_panels(4);       
    }
    
    this.grid_click = function(args)
    {       
       try
       {
           this.set_selected_file(dbgrid.getDataAt(args.datarow,0))
           var filedtls = this.get_selected_agent_text() + ";" +
                            dbgrid.getDataAt(args.datarow,1) + ";" + 
                            dbgrid.getDataAt(args.datarow,2) +  '/' +
                            dbgrid.getDataAt(args.datarow,3)  ; 
           
           this.set_selected_filedetails(filedtls)
           this.display_filedetails();
       }
       catch(e) {}
    }
    this.create_for_approval_grid = function()
    {
        gv = new datagrid();             
        {
            var col = new datagrid_column();
            col.header = ' &nbsp; <br /><input type=checkbox id=chkAll style=background-color:transparent;border:none onclick=javascript:js_EXCO.select_for_approval_items(0); />';
            col.datafield = 'CHECKBOX';
            col.align = 'center';
            col.styles.cell += 'width:8%;';
            col.type = 'check';
            col.onclick = 'js_EXCO.recompute_amount';
            //col.on_init = 'js_EXCO.init_checkboxes'
            gv.addColumn(col);
         }
         {
		    var col = new datagrid_column(); //0
		    col.align = 'left';
		    col.header = 'SEQ ID';
		    col.datafield = 'SEQ_ID';
            col.styles.cell += 'width:auto;display:none';
		    col.styles.header += 'display:none';
		    col.styles.datafield += 'display:none';
		    
		    gv.addColumn(col);
        }       
        {
		    var col = new datagrid_column(); // 2
		    col.header = 'REMITTER NAME';
		    col.datafield = 'REMITTERFULLNAME';
		    col.align = 'right';
		    col.styles.cell += 'width:auto';
		    col.editable = false;
		    gv.addColumn(col);
        }
        /*{
		    var col = new datagrid_column(); // 3
		    col.align = 'right';
		    col.header = 'REMITTER ADDRESS';
		    col.datafield = 'REMITTERADDRESS';
		    col.editable = false;            
		    gv.addColumn(col);
        }*/
        {
		    var col = new datagrid_column(); // 4
		    col.header = 'BENE FULLNAME';
		    col.datafield = 'BENEFULLNAME';
		    col.align = 'right';
		    col.editable = false;
		    col.styles.cell += 'width:auto';
		    gv.addColumn(col);
        }       
        /*{
		    var col = new datagrid_column(); // 4
		    col.header = 'BENE ADDRESS';
		    col.datafield = 'BENEADDRESS';
		    col.align = 'right';
		    col.editable = false;
		    col.styles.cell += 'width:auto';
		    gv.addColumn(col);
        } */      
        {
		    var col = new datagrid_column(); // 4
		    col.header = 'IMPLEMENTATION';
		    col.datafield = 'IMPLEMENTATION';
		    col.align = 'right';
		    col.editable = false;
		    col.styles.cell += 'width:auto';
		    gv.addColumn(col);
        }             
        {
		    var col = new datagrid_column(); // 4
		    col.header = 'REMITTING CURRENCY CODE';
		    col.datafield = 'REMITTINGCURRENCYCODE';
		    col.align = 'center';
		    col.editable = false;		    
		    col.styles.cell += 'width:auto;display:none';
		    col.styles.header += 'display:none';
		    col.styles.datafield += 'display:none';
		    gv.addColumn(col);
        }     
        {
		    var col = new datagrid_column(); // 4
		    col.header = 'REMITTING CURRENCY';
		    col.datafield = 'REMITTINGCURRENCY';
		    col.align = 'center';
		    col.editable = false;
		    col.styles.cell += 'width:auto';
		    gv.addColumn(col);
        }   
        {
		    var col = new datagrid_column(); // 4
		    col.header = 'REMITTING AMOUNT';
		    col.datafield = 'REMITTINGAMOUNT';
		    col.align = 'right';
		    col.editable = false;
		    col.styles.cell += 'width:auto';
		    gv.addColumn(col);
        }   
        {
		    var col = new datagrid_column(); // 4
		    col.header = 'STATUS';
		    col.datafield = 'STATUS';
		    col.align = 'right';
		    col.editable = false;
		    col.styles.cell += 'width:auto';
		    gv.addColumn(col);
        }   
        
        
            //gv.row.onclick = 'js_EXCO.grid_click';                      
            gv.row.normal_style = 'background-color: #f7ffff;';                      
            return gv;
    }
    this.display_for_approval = function()
    {        
        AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. DISPLAYING OF FOR APPROVAL ITEMS FAILED.';
            alert(errmsg);            return;
        } 
        var branch = this.get_selected_agent();
        var fileselected = this.get_selected_file();
        var curr    = this.get_selected_curr();
        
        js_EXCOpopup.open_msg_prompt("GETTING FILE ITEM DETAILS");
        
        IRSWorld.EXCO.EXCOAgent_Approval.get_exco_file_details(branch,fileselected,curr,this.display_for_approval_callback)
    }
    this.display_for_approval_callback = function(result)
    {
        js_EXCOpopup.close_msg_prompt();
        
        if (result.error != null)
        {
            spawn_errpage(result.error);
            return;
        }       
        
        dsforApproval  = result.value;
        dtItems = dsforApproval.Tables[0];
        dtAmount = dsforApproval.Tables[1]; 
        dbgridItems = js_EXCO.create_for_approval_grid();
        dbgridItems.clearFilter();                    
        dbgridItems.filter.enabled = false;  
        dbgridItems.bind(dtItems, 'tdEXCOForApproval', 'tblEXCOForApproval');    
        
        js_EXCO.display_amounts_summary_callback(dtAmount);            
        js_EXCO.toggle_checkboxes(false,false);         
        var curr    = js_EXCO.get_selected_curr();
        document.getElementById("chkAll").checked  =  (curr == 0) ? true : false;        
        js_EXCO.populate_WorkingTable(curr);
        
        if (dtItems.Rows.length > 0)
        {   
            document.getElementById("btnAll").disabled = false;
            document.getElementById("btnAllPHP").disabled = false;
            document.getElementById("btnAllUSD").disabled = false;
        }
        else
        {
            document.getElementById("btnAll").disabled = true;
            document.getElementById("btnAllPHP").disabled = true;
            document.getElementById("btnAllUSD").disabled = true;
        }
    }
    this.populate_WorkingTable  = function(curr)
    {  
        SelectTable = null;
        SelectTable  = new WorkingTable();
        var push = false;
        for(i = 0 ; i < dtItems.Rows.length; i++)
        {
            push = false;
            if (parseInt(curr) != parseInt(0))
            {
                if (parseInt(curr) == parseInt(dtItems.Rows[i].REMITTINGCURRENCYCODE))
                {
                    push = true;
                }
                else
                {
                    push = false;
                }
            }
            else
            {
                push = true;
            }
            if (push)
            {
                var r = new WorkingRow();
                r.seq_id = dtItems.Rows[i].SEQ_ID;
                r.currency = dtItems.Rows[i].REMITTINGCURRENCYCODE;
                SelectTable.AddRow(r);
            }
        }
    }
    this.check_all = function(objid)
    {          
        this.display_for_approval()
    }
    this.check_per_row = function(args)
    {
        document.getElementById("chkAll").checked = false;
    }
    this.toggle_checkboxes = function(disable,selectall)
    {
        var frm = document.forms[0];
        
        if (frm != null)
        {
            for(i = 0 ; i < frm.length; i++)
            { 
                var obj = frm.elements[i];
                try
                {
                    if (obj.type == "checkbox")
                    {
                        obj.disabled = disable;                       
                        //obj.checked = selectall;
                    }
                }
                catch(e){}
            }
        }
    }
    this.recompute_amount = function(args)
    {    
        var sarr = (args.id).split("_");
        var indx = parseInt(sarr[3]);
        var seq_id = parseInt(dbgridItems.getValueAt(indx, 1));
        var curr = dbgridItems.getValueAt(indx, 6);
        var currcode = dbgridItems.getValueAt(indx, 5);
        var amt = parseFloat(dbgridItems.getValueAt(indx, 7));
        var ischecked  = dbgridItems.getValueAt(indx, 0);        
        var rows = document.getElementById(dbgridAmount.id).rows;
        var amttodeduct = 0;
        var amttoadd = 0;
        document.getElementById("chkAll").checked = false;

        dbgridAmount.row.selected_data_row = rows.length;		       
        
        var currtotitems = parseInt(dbgridAmount.getValueAt(rows.length - 3, 1));        
        var currtotamt = parseFloat(dbgridAmount.getValueAt(rows.length - 3, 2));        
        
        var spos  = -1;
	    for (var i = 0; i < rows.length; i++)
	    {
	        try
	        {
		        var curramt = dbgridAmount.getValueAt(i,0);
		        var curr_amt = parseFloat(dbgridAmount.getValueAt(i,2));
		        var curr_items = parseFloat(dbgridAmount.getValueAt(i,1));		        
		        var newamt = 0;
		        var newitems = 0;
		        if (curramt == curr)
		        {
		            if (!ischecked)
		            {
		                if (curr_amt >= amt )
		                {
		                    newamt = parseFloat(curr_amt) - parseFloat(amt);		                    
		                }
		                else
		                {
		                    newamt = parseFloat(amt) - parseFloat(curr_amt);
		                }	
		                currtotitems -= 1	             
		                newitems = curr_items - 1;
		                amttodeduct += amt;
		                
		                
		                var pos = 0;
		                var pass = false;
		                
		                for(j=0; j < SelectTable.Rows.length; j++)
		                {
		                    if (SelectTable.Rows[j].seq_id == seq_id)
		                    {
		                        pos = j;
		                        pass = true;
		                        break;
		                    }
		                }
		                
		                if (pass)
		                SelectTable.RemoveRow(pos);
		            }
		            else
		            {   
		                newamt = parseFloat(curr_amt) + parseFloat(amt);
		                newitems = curr_items + 1;		
		                amttoadd += amt;                   
		                currtotitems += 1;
		                
		                var r = new WorkingRow();
		                r.seq_id   = dbgridItems.getValueAt(indx, 1);
		                r.currency = currcode;
		                SelectTable.AddRow(r);
		            }
		           
		            spos = i;
		            dbgridAmount.row.selected_data_row = i ;
		            dbgridAmount.setColumn(2,newamt);
		            dbgridAmount.setColumn(1,newitems);		            		            
		            
		        }
		        
	        }    		                
	        catch (e)
	        {
		        alert(e);
	        }
        }
        
        

            if (amttodeduct > 0 )
		    currtotamt   = currtotamt -  amttodeduct;
		    else
		    currtotamt   = currtotamt +  amttoadd;
    		
            dbgridAmount.row.selected_data_row = rows.length - 3;		            
            dbgridAmount.setColumn(2,currtotamt);
            dbgridAmount.setColumn(1,currtotitems);

        this.check_approval_items();
    }
    this.check_approval_items = function()
    {
        var rows = document.getElementById(dbgridAmount.id).rows;
        var item_ctr = 0;
        var curr_item_ctr = 0;
        for (var i = 0; i < rows.length; i++)
	    {
	        try
	        {
	            var items = dbgridAmount.getValueAt(i,1);
	            
	            if (!isNaN(items))
	            {
	                items = parseInt(dbgridAmount.getValueAt(i,1));
	                item_ctr += (items == 0 )  ?  1 : 0;
	                curr_item_ctr++;
	            }
	        }
	        catch(e)
	        {
	            alert(e);
	        }
	     }	     
    }
    this.select_for_approval_items = function(currency)
    {        
        this.set_selected_curr(currency);
        this.display_for_approval();
    }
    
    this.create_amount_summary_grid = function()
    {
        gv = new datagrid();                       
        {
		    var col = new datagrid_column(); // 2
		    col.header = 'REMITTING CURRENCY';
		    col.datafield = 'REMITTINGCURRENCY';
		    col.align = 'right';
		    col.styles.cell += 'width:auto';
		    col.editable = false;
		    gv.addColumn(col);
        }
        {
		    var col = new datagrid_column(); // 2
		    col.header = 'ITEMS';
		    col.datafield = 'ITEMS';
		    col.align = 'right';
		    col.styles.cell += 'width:auto';
		    col.editable = false;
		    gv.addColumn(col);
        }
        {
		    var col = new datagrid_column(); // 2
		    col.header = 'TOTALREMITTINGAMOUNT';
		    col.datafield = 'TOTALREMITTINGAMOUNT';
		    col.align = 'right';
		    col.styles.cell += 'width:auto';
		    col.editable = false;
		    gv.addColumn(col);
        }              
        {
		    var col = new datagrid_column(); // 2
		    col.header = 'CURRENCY';
		    col.datafield = 'CURRENCYCODE';
		    col.align = 'right';
		    col.styles.cell += 'width:auto';
		    col.editable = false;
		    col.styles.cell += 'width:auto;display:none';
		    col.styles.header += 'display:none';
		    col.styles.datafield += 'display:none';
		    gv.addColumn(col);
        }            
            gv.row.normal_style = 'background-color: #f7ffff;';                      
            return gv;
    }
    
    this.display_amounts_summary = function()
    {
        AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. DISPLAYING OF FOR APPROVAL ITEMS FAILED.';
            alert(errmsg);           return;  
        } 
        var branch = this.get_selected_agent();
        var fileselected = this.get_selected_file();
        IRSWorld.EXCO.EXCOAgent_Approval.get_exco_file_details(branch,fileselected,this.display_amounts_summary_callback)
    }
    this.display_amounts_summary_callback = function(dt)
    {   
        dbgridAmount = js_EXCO.create_amount_summary_grid()
        dbgridAmount.clearFilter();                    
        dbgridAmount.filter.enabled = false;  
        dbgridAmount.bind(dt, 'tdEXCOAmountSummary', 'tblEXCOAmountSummary');           
        js_EXCO.load_currency(dt);
                
        if (dt.Rows.length > 0)
        {            
            if (dt.Rows.length == 1 )
            document.getElementById("btnViewFunds").disabled = true;            
            else
            document.getElementById("btnViewFunds").disabled = false;            
        }
        else
        {
            document.getElementById("btnViewFunds").disabled = true;
            
        }
    }
    this.load_currency = function(dt)    
    {
        CurrencyTable = new CurrTable();
        for(i = 0 ;i < dt.Rows.length; i++)
        {
            if (dt.Rows[i].REMITTINGCURRENCY.toString().length > 0)
            {
                var r = new cRow();
                r.currency = dt.Rows[i][0];
                CurrencyTable.addRow(r);
            }
        }
    }
    this.load_currency_funds = function(dt)    
    {
        CurrencyFundsTable = new CurrTable();
        for(i = 0 ;i < dt.Rows.length; i++)
        {
            if (dt.Rows[i].CURRENCY.toString().length > 0)
            {
                var r = new cRow();
                r.currency = dt.Rows[i][0];
                CurrencyTable.addRow(r);
            }
        }
        
    }
    this.btnViewFunds_click = function()
    {        
        AjaxPro.timeoutPeriod = 1000 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. VALIDATING OF ACCOUNTS FAILED.';
            alert(errmsg);           return;  
        } 
        var branch = this.get_selected_agent();
        js_EXCOpopup.open_msg_prompt("VALIDATING ACCOUNTS");
        IRSWorld.EXCO.EXCOAgent_Approval.validate_accounts(branch,this.validate_accounts_callback);
    }       
    this.validate_accounts_callback = function(result)
    {
        js_EXCOpopup.close_msg_prompt();
        sres = result.value
        try
        {
            if (sres.toString().length == 0)
                js_EXCO.display_agent_funds();  
            else
                alert("ACCOUNTS NOT SETUP PROPERLY: " + sres);
        }
        catch(e)
        {
            alert("THERE WAS AN ERROR ENCOUNTERED WHILE VALIDATING ACCOUNTS: PLEASE RETRY.");
        }
    }
    this.create_funds_grid = function()
    {
        gv = new datagrid();                       
        {
		    var col = new datagrid_column(); // 0
		    col.header = 'ACCOUNTNO';
		    col.datafield = 'ACCOUNTNO';
		    col.align = 'right';
		    col.styles.cell += 'width:auto';
		    col.editable = false;
		    gv.addColumn(col);
        }
        {
		    var col = new datagrid_column(); // 1
		    col.header = 'CURRENCY';
		    col.datafield = 'CURRENCY';
		    col.align = 'right';
		    col.styles.cell += 'width:auto';
		    col.editable = false;
		    gv.addColumn(col);
        }       
        {
		    var col = new datagrid_column(); // 2
		    col.header = 'FUNDSVALUE';
		    col.datafield = 'FUNDSVALUE';
		    col.align = 'right';
		    col.styles.cell += 'width:auto';
		    col.editable = false;
		    gv.addColumn(col);
        }
        {
		    var col = new datagrid_column(); // 3
		    col.header = 'O. AMOUNT';
		    col.datafield = 'AMOUNTTODEBIT';
		    col.align = 'right';
		    col.styles.cell += 'width:auto';
		    col.editable = false;
		    col.styles.cell += 'width:auto;display:none';
		    col.styles.header += 'display:none';
		    col.styles.datafield += 'display:none';
		    gv.addColumn(col);
        }
        {
		    var col = new datagrid_column(); // 4
		    col.header = 'CNVRTD AMOUNT';
		    col.datafield = 'CONVERTEDAMOUNT';
		    col.align = 'right';
		    col.styles.cell += 'width:auto';
		    col.editable = false;
		    col.styles.cell += 'width:auto;display:none';
		    col.styles.header += 'display:none';
		    col.styles.datafield += 'display:none';
		    gv.addColumn(col);
        }
        {
		    var col = new datagrid_column(); // 5
		    col.header = 'TOTAL AMT TO DEBIT';
		    col.datafield = 'TOTALDEBIT';
		    col.align = 'right';
		    col.styles.cell += 'width:auto';
		    col.editable = false;
		    gv.addColumn(col);
        }
        {
		    var col = new datagrid_column(); // 6
		    col.header = 'TOTAL AMOUNT';
		    col.datafield = 'TOTALAMOUNT';
		    col.align = 'right';
		    col.styles.cell += 'width:auto';
		    col.editable = false;
		    gv.addColumn(col);
        }
        {
		    var col = new datagrid_column(); // 7
		    col.header = 'CURRENCYCODE';
		    col.datafield = 'CURRENCYCODE';
		    col.align = 'right';
		    col.styles.cell += 'width:auto;display:none';
		    col.styles.header += 'display:none';
		    col.styles.datafield += 'display:none';
		    gv.addColumn(col);
        }      
        
            gv.row.normal_style = 'background-color: #f7ffff;';                      
            return gv;
    }
    
    
    this.display_agent_funds = function()
    {
        AjaxPro.timeoutPeriod = 1000 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. DISPLAYING OF FOR APPROVAL ITEMS FAILED.';
            alert(errmsg);           return;  
        } 
        var branch = this.get_selected_agent();
        var fileselected = this.get_selected_file();
        
        js_EXCOpopup.open_msg_prompt("GETTING AGENT FUNDS");
        
        IRSWorld.EXCO.EXCOAgent_Approval.get_exco_accountbalance(branch,this.display_agent_funds_callback)
    }
    this.display_agent_funds_callback = function(result)
    {
        js_EXCOpopup.close_msg_prompt();
        
        js_EXCOpopup.create_popup();
        document.getElementById("content_mod_div_exco").innerHTML = js_EXCOpopup.construct_funds_html();
    
        dtAgentFunds = result.value;
        dbgridFunds = js_EXCO.create_funds_grid()
        dbgridFunds.clearFilter();                    
        dbgridFunds.filter.enabled = false;  
        dbgridFunds.bind(dtAgentFunds, 'tdComputeFunds', 'tblComputeFunds');
        
        if (js_EXCO.check_negative_amount() == false)
        {
            js_EXCO.load_currency_funds(dtAgentFunds);
            js_EXCO.compute_amount_to_debit();
            document.getElementById("btnApprove").disabled = false;
        }
        else
        {
            document.getElementById("btnApprove").disabled = true;
        }
    }
    this.check_negative_amount = function()
    {
        for(i = 0 ; i < dtAgentFunds.Rows.length; i++)
        {
            dbgridFunds.row.selected_data_row = i;                    
            var currFunds    =  dbgridFunds.getValueAt(i,1)
            var funds   = parseFloat(dbgridFunds.getValueAt(i,2));    
            
            if (funds.toString() == "-1")
            {  
                alert("SYSTEM INQUIRY IS DOWN. PLEASE RETRY. ");
                return true;
            }                         
        }           
        return false;
    }
    this.compute_amount_to_debit = function()
    {
        var branch = this.get_selected_agent();     
        IRSWorld.EXCO.EXCOAgent_Approval.get_exco_currency_rates(branch,2,1,this.compute_amount_to_debit_callback);                
    }
    this.compute_amount_to_debit_callback = function(result)
    {
        dtCurrencyRates = result.value ;
        setText("lblRatesUsed",parseFloat(dtCurrencyRates.Rows[0].TARGETTOSOURCE.toString()));        
       
         for(z = 0; z < dtAmount.Rows.length; z++)
        {
            var curr_row  = z + 1
            
            dbgridAmount.row.selected_data_row = curr_row;
            
            var amounttodebit = parseFloat(dbgridAmount.getValueAt(curr_row - 1,2));                
            var currAmount    =  dbgridAmount.getValueAt(curr_row - 1,0)               
        
            for(i = 0 ; i < dtAgentFunds.Rows.length; i++)
            {
                dbgridFunds.row.selected_data_row = i;                    
                var currFunds    =  dbgridFunds.getValueAt(i,1)
                var funds   = parseFloat(dbgridFunds.getValueAt(i,2));    
                
                if (currAmount == currFunds)
                {                        
                    var totamt  = funds - amounttodebit;
                    dbgridFunds.setColumn(3,amounttodebit);
                    dbgridFunds.setColumn(6,totamt);
                    dbgridFunds.setColumn(5,amounttodebit);
                }                    
            }
        }            
    }   
    this.get_index_currency_funds = function(curr)
    {
        var i;
        for(i = 0 ; i < dtAgentFunds.Rows.length ;i++)   
        {
            dbgridFunds.row.selected_data_row = i; 
            if (dbgridFunds.getDataAt(i,7) == curr.toString()) return i;
        }
    }
    this.get_currency_rates = function(sourcecurr,targetcurr)
    {
        AjaxPro.timeoutPeriod = 1000 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. GETTING OF CURRENCY RATES FAILED.';
            alert(errmsg);           return;  
        } 
        var branch = this.get_current_branch();        
        IRSWorld.EXCO.EXCOAgent_Approval.get_exco_currency_rates(branch,sourcecurr,targetcurr,js_EXCO.get_currency_rates_callback)
    }
    this.get_currency_rates_callback = function(result)
    {
        dtCurrencyRates = result.value;      
    }
    this.get_loaded_rates = function()
    {
    }
    this.approve_click = function()
    {
	    if (!this.validate_funds())
	    {
		    alert("CANNOT PROCEED WITH NEGATIVE FUNDS");
		    return;
	    }	
        AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. TAGGING OF APPROVAL ITEMS FAILED.';
            alert(errmsg);           return;  
        } 
        var branch = this.get_selected_agent();
        var fileselected = this.get_selected_file();
        var selecteditems = this.get_approve_items();
        var selectedfunds = this.get_selected_funds();
        
        
        js_EXCOpopup.open_msg_prompt("SYSTEM PROCESSING... PLEASE WAIT");
               
        var isexcoon = this.check_exco_switch(branch);
        
        setText("tdswitch",isexcoon);
        
        if (!isexcoon)
        {
            IRSWorld.EXCO.EXCOAgent_Approval.approve_items(branch,fileselected,selecteditems,selectedfunds,this.approve_click_callback)	
        }
        else
        {
            IRSWorld.EXCO.EXCOAgent_Approval.force_push_items(branch,fileselected,selecteditems,selectedfunds,this.approve_click_callback)	
        }
        
        
    }
    
    
    this.check_exco_switch = function(branch)
    {
        AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. CHECKING OF SWITCH FAILED.';
            alert(errmsg);           return;  
        } 
        var pass = false;
        var dt = null;
        try
        {
            dt = IRSWorld.EXCO.EXCOAgent_Approval.get_exco_switch(branch).value;
            
            pass = (parseInt(dt.Rows[0].switchitem) > 0) ? true : false;
            
        }
        catch(e)
        {
            
        }
        
        return pass;
    }
    this.force_push_click = function()
    {
      
        AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. TAGGING OF FORCE PUSH ITEMS FAILED.';
            alert(errmsg);           return;  
        } 
        var branch = this.get_selected_agent();
        var fileselected = this.get_selected_file();
        var selecteditems = this.get_approve_items();
        var selectedfunds = this.get_selected_funds();
        js_EXCOpopup.open_msg_prompt("SYSTEM PROCESSING... PLEASE WAIT");
        
        IRSWorld.EXCO.EXCOAgent_Approval.force_push_items(branch,fileselected,selecteditems,selectedfunds,this.approve_click_callback)	
    }
    this.approve_click_callback = function(result)
    {
	    js_EXCOpopup.close_msg_prompt();
	    js_EXCOpopup.close_modal_window();
	    
	    var pass =  result.value
	    try
	    {
	        if (pass.toString().length > 0 )
	        {	    		    		        
		        js_EXCO.populate_tranSummary(pass);
	        }
	        else
	        {
	            alert("THERE WAS AN ERROR UPDATING ITEMS FOR APPROVAL PLEASE TRY AGAIN");
	        }
	    }	 
	    catch(e)
	    {
	        alert(result.error + " " + e);
	    }   
    }
    this.validate_funds = function()
    {	
        var sfundsselected = "";
        for(i=0; i < dtAgentFunds.Rows.length; i++)
        {
	        dbgridFunds.row.selected_data_row = i;
	        
	        
	        if (parseFloat(dbgridFunds.getValueAt(i,6)) < 0)
	        {
		        return false;
	        }
	        else
	        {
	            var amounttodebit = parseFloat(dbgridFunds.getValueAt(i,5));
	            sfundsselected +=  dbgridFunds.getValueAt(i,0) + "=" + amounttodebit.toString() + "=" + dbgridFunds.getValueAt(i,7) + ";";
	        }
        }
        this.set_selected_funds(sfundsselected);
        return true;
    }
    this.get_approve_items = function()
    {
	    var approve_items = "";
	    for(i=0; i < SelectTable.Rows.length; i++)
	    {
			    approve_items +=  SelectTable.Rows[i].seq_id + ",";		 
	    }
	    return approve_items;
    }
    this.populate_tranSummary = function(sarr)
    {   
        try
        {
            drcrResult = sarr;       
            var splitarr = sarr.split(";");
            var errmsg = splitarr[6].toString();
            if (errmsg.indexOf("ERROR") > -1)
            {
                alert("THERE WAS AN ERROR ENCOUNTERED WHILE PROCESSING YOUR REQUEST. \n" + errmsg);
                return;
            }
            else            
            {
            alert("APPROVING OF ITEMS SUCCESSFUL.AN EMAIL CONFIRMATION WILL BE SENT IN A WHILE.");
            js_EXCO.toggle_panels(3);
            document.getElementById("divTransDetail").innerHTML = js_EXCO.construct_drcr_summary(sarr);
            var branch =  js_EXCO.get_current_branch();        
            IRSWorld.EXCO.EXCOAgent_Approval.send_exco_email(branch,splitarr[7],splitarr[8],splitarr[9],js_EXCO.send_email_callback);        
            }
        }
        
        catch(e)
        {
            alert("THERE WAS AN ERROR ENCOUNTERED WHILE PROCESSING YOUR REQUEST.")
        }
    }
    this.send_email_callback = function(result)   
    {
        if (result != null)
        {
            var pass = result.value
            if (pass)
            {
                alert("SENDING OF EMAIL SUCCESSFUL");
            }
            else
            {
               alert("FAILED TO SEND EMAIL NOTIFICATION");
            }
        }
    }
    this.construct_drcr_summary = function(arrval)
    {
        var shtml = "";
        var arrcontents = arrval.split('~');
        
        shtml  = '<table id="Table1" style="width: 100%;text-align:center;" class="table_border">';
        
        for (i = 0; i < arrcontents.length; i++)
        {
            statvalues = arrcontents[i].split(';');
            
            if (statvalues.length > 0 )
            {
                if (statvalues[0].toString() != "")
                {
                    shtml += '<tr>';
                    shtml += '<td class="cell_caption" colspan="4" style="height: 21px; text-align: center">DR/CR Transaction Summary</td>';             
                    shtml += '</tr>';
                    shtml += '<tr>';
                    shtml += '<td class="cell_caption" style="width: 197px; height: 13px;">System Reference:</td>';
                    shtml += '<td id="td0" class="cell_desc" align="left" style="width: 140px; height: 13px">' + statvalues[0];
                    shtml += '</td>';
                    shtml += '<td align="left" class="cell_caption" style="width: 167px; height: 13px">Transaction Reference:</td>';
                    shtml += '<td align="left" class="cell_desc" style="height: 13px">' + statvalues[2];
                    shtml += '</td>';
                    shtml += '</tr>';
                    shtml += '<tr>';
                    shtml += '<td class="cell_caption" style="width: 197px; height: 18px;">Transaction Date:</td>';
                    shtml += '<td id="td1"  class="cell_desc" align="left" style="width: 140px; height: 18px">' + statvalues[1];
                    shtml += '</td>';
                    shtml += '<td align="left" class="cell_caption" style="width: 167px; height: 18px">';
                    shtml += '</td>';
                    shtml += '<td align="left" class="cell_desc" style="height: 18px">';
                    shtml += '</td>';
                    shtml += '</tr>';
                    shtml += '<tr>';
                    shtml += '<td class="cell_caption" style="width: 197px; height: 11px">Source Account:</td>';
                    shtml += '<td id="td10" align="left" class="cell_desc" style="height: 11px; width: 140px;">' + statvalues[3];
                    shtml += '</td>';
                    shtml += '<td align="left" class="cell_caption" style="width: 167px; height: 11px">Target Account:</td>';
                    shtml += '<td align="left" class="cell_desc" style="height: 11px">' + statvalues[4]
                    shtml += '</td>';
                    shtml += '</tr>';
                    shtml += '<tr>';
                    shtml += '<td class="cell_caption" style="width: 197px; height: 7px">Currency:</td>';
                    shtml += '<td id="td3" style="height: 7px; width: 140px;" class="cell_desc" align="left">' + statvalues[10];
                    shtml += '</td>';
                    shtml += '<td align="left" class="cell_caption" style="width: 167px; height: 7px">Amount to Debit:</td>';
                    shtml += '<td align="left" class="cell_desc" style="height: 7px">' + statvalues[5];
                    shtml += '</td>';
                    shtml += '</tr>';
                    shtml += '<tr>';
                    shtml += '<td class="cell_caption" style="width: 197px">Status/Remarks:</td>';
                    shtml += '<td id="td4" class="cell_desc" align="left" colspan="3">'; + statvalues[6]
                    shtml += '</td>';
                    shtml += '</tr>';
                }
            }
        }
            shtml += '<tr>';
            shtml += '<td colspan="4">';
            shtml += '<input id="btnPrint" type="button" value="PRINT" style="width: 15%" onclick="javascript:js_EXCO.btnPrint_click();"/>&nbsp;';
            shtml += '<input id="btnClose" type="button" value="CLOSE" style="width: 15%" onclick="javascript:js_EXCO.btnCancelApprove_click();"/></td>';
            shtml += '</tr>';
            shtml += '</table>'
      
        return shtml;
    }
    
     this.construct_drcr_plain_summary = function(arrval)
    {
        var shtml = "";
        var arrcontents = arrval.split('~');
        
        shtml  = '<table id="tblPlainSummary" style="width: 100%;text-align:center;font-family:arial" border="1" cellpadding="0" cellspacing="0">';
        
        for (i = 0; i < arrcontents.length; i++)
        {
            statvalues = arrcontents[i].split(';');
            
            if (statvalues.length > 0 )
            {
                if (statvalues[0].toString() != "")
                {
                    shtml += '<tr>';
                    shtml += '<td  colspan="4" style="height: 21px; text-align: center">DR/CR Transaction Summary</td>';             
                    shtml += '</tr>';
                    shtml += '<tr>';
                    shtml += '<td  style="width: 197px; height: 13px;">System Reference:</td>';
                    shtml += '<td id="td0" align="left" style="width: 140px; height: 13px">' + statvalues[0];
                    shtml += '</td>';
                    shtml += '<td align="left"  style="width: 167px; height: 13px">Transaction Reference:</td>';
                    shtml += '<td align="left"  style="height: 13px">' + statvalues[2];
                    shtml += '</td>';
                    shtml += '</tr>';
                    shtml += '<tr>';
                    shtml += '<td  style="width: 197px; height: 18px;">Transaction Date:</td>';
                    shtml += '<td id="td1"   align="left" style="width: 140px; height: 18px">' + statvalues[1];
                    shtml += '</td>';
                    shtml += '<td align="left"  style="width: 167px; height: 18px">';
                    shtml += '</td>';
                    shtml += '<td align="left"  style="height: 18px">';
                    shtml += '</td>';
                    shtml += '</tr>';
                    shtml += '<tr>';
                    shtml += '<td  style="width: 197px; height: 11px">Source Account:</td>';
                    shtml += '<td id="td10" align="left"  style="height: 11px; width: 140px;">' + statvalues[2];
                    shtml += '</td>';
                    shtml += '<td align="left" style="width: 167px; height: 11px">Target Account:</td>';
                    shtml += '<td align="left" style="height: 11px">' + statvalues[3]
                    shtml += '</td>';
                    shtml += '</tr>';
                    shtml += '<tr>';
                    shtml += '<td style="width: 197px; height: 7px">Currency:</td>';
                    shtml += '<td id="td3" style="height: 7px; width: 140px;" align="left">' + statvalues[10];
                    shtml += '</td>';
                    shtml += '<td align="left"  style="width: 167px; height: 7px">Amount to Debit:</td>';
                    shtml += '<td align="left"  style="height: 7px">' + statvalues[5];
                    shtml += '</td>';
                    shtml += '</tr>';
                    shtml += '<tr>';
                    shtml += '<td style="width: 197px">Status/Remarks:</td>';
                    shtml += '<td id="td4"  align="left" colspan="3">'; + statvalues[7]
                    shtml += '</td>';
                    shtml += '</tr>';
                }
            }
        }

            shtml += '</table>'
      
        return shtml;
    }
    this.btnPrint_click = function()
    {
        document.getElementById("divmainexco").style.display = "none";
        document.getElementById("divprintexco").style.display = "block";
        document.getElementById("divprintexco").innerHTML = js_EXCO.construct_drcr_plain_summary(drcrResult);
        window.print();
        js_EXCO.btnCancelApprove_click();
        document.getElementById("divmainexco").style.display = "block";
        document.getElementById("divprintexco").style.display = "none";
    }
}

function js_EXCO_popup()
{
    this.msg_prompt_control = null;
    
    this.create_popup = function()
    {
        this.open_modal_window();
    }
    this.close_popup = function()
    {
        this.close_modal_window();
    }
     this.open_msg_prompt = function(msg)
    {
        this.msg_prompt_control = new screen_msg_demo();
        this.msg_prompt_control.open_screen();
        this.msg_prompt_control.change_msg(msg);
    }
    
    this.close_msg_prompt = function()
    {
        if (this.msg_prompt_control != null)
        {
            this.msg_prompt_control.close_screen();
        }

        this.msg_prompt_control = null;
    }    
    
    this.open_modal_window = function()
    {
        this.load_modal_css();
    
        this.outer_modal_div           = document.createElement("div");
        this.outer_modal_div.id        = "outer_mod_div_exco";
        this.outer_modal_div.className = "outer_div";
    
        document.body.style.margin = "0px";
        document.body.appendChild(this.outer_modal_div);
        document.getElementById("outer_mod_div_exco").style.height = screen.height;
    
        this.construct_div_content();
        this.commons = new common_method();
        this.commons.to_show_hide_combobox(false);
        this.commons = null;
    }

    this.close_modal_window = function()
    {
        this.commons = new common_method();
        this.commons.to_show_hide_combobox(true);
        this.commons = null;

        if(this.outer_modal_div != null){document.body.removeChild(this.outer_modal_div);this.outer_modal_div  = null;}
        if(this.inner_modal_div != null){document.body.removeChild(this.inner_modal_div);this.inner_modal_div  = null;}

        document.body.style.margin = "";
    }

    this.construct_div_content = function()
    {
        var content_div = null;

            this.inner_modal_div              = document.createElement("div");
            this.inner_modal_div.className    = "inner_div";		
            this.inner_modal_div.id           = "inner_mod_div_exco";
            this.inner_modal_div.style.height = "auto";		
            this.inner_modal_div.style.width  = "650px";
            this.inner_modal_div.style.top    = "30px";
            this.inner_modal_div.style.left   = (parseInt(screen.width) - parseInt(this.inner_modal_div.style.width)) / 2 - 10 + "px";
            this.inner_modal_div.style.border = "medium solid white";

            content_div                       = document.createElement("div");
            content_div.id                    = "content_mod_div_exco";
            content_div.style.background      = "transparent";
            content_div.innerHTML             = "<center><img src = '.././images/ajaxloader.gif'></img></center>";

            this.inner_modal_div.appendChild(content_div);

            document.body.appendChild(this.inner_modal_div);
    }
	
    this.load_modal_css = function()
    {	    
            var file_reference = document.createElement("link");
            
            file_reference.setAttribute("rel", "stylesheet");
            file_reference.setAttribute("type", "text/css");
            file_reference.setAttribute("href", ".././css/cssmodals.css");

            if (typeof file_reference !="undefined")
            {		    
                    document.getElementsByTagName("head")[0].appendChild(file_reference)
            }
    }
    this.construct_funds_html = function()
    {
        var shtml = ""
        shtml +=        '<table id="tblComputeFunds_holder" style="width: 100%;text-align:center;" class="table_border">'
        shtml +=        '<tr>'
        shtml +=            '<td colspan="2" class="cell_caption" style="width: 100%; height: 21px;text-align:center;">Funds Summary</td>'                                                     
        shtml +=        '</tr>'
        shtml +=        '<tr style="display:none;">'
        shtml +=           '<td class="cell_caption" style="height: 21px;text-align:center;">Rates Used:</td>'
        shtml +=           '<td class="cell_desc"  align="left"> <label id="lblRatesUsed"></label></td>'
        shtml +=        '</tr>'
        shtml +=        '<tr>'
        shtml +=            '<td  colspan="2" id="tdComputeFunds" style="height: 21px" ></td>'
        shtml +=        '</tr>'
        shtml +=        '<tr>'
        shtml +=        '<td colspan="2"> '
        shtml +=        '<input id="btnApprove" onclick="javascript:js_EXCO.approve_click();" style="width: 15%" type="button" value="APPROVE" />&nbsp;'
        shtml +=        '<input id="btnClose" type="button" value="CLOSE" style="width: 15%" onclick="javascript:js_EXCOpopup.close_popup();"/>'
        shtml +=        '</td>'
        shtml +=        '</tr>'
        shtml +=         '</table>'
        
        return shtml;
    }
    
    window.onscroll = function () 
    {            
        var i = 0;
        var m = 0;
    
        var divelement_outer = document.documentElement.getElementsByTagName("div");
        var divelement_inner = document.documentElement.getElementsByTagName("div");
                        
            for(i = 0; i < divelement_outer.length;i++)
            {
                if (divelement_outer.item(i).id == "outer_mod_div_exco")
                {
                    divelement_outer.item(i).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop;break;
                }	        
            }
            for(m = 0; m < divelement_inner.length;m++)
            {
                if (divelement_inner.item(m).id == "inner_mod_div_exco")
                {
                    divelement_outer.item(m).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop + 30 : document.body.scrollTop + 30;break;
                }
            }
    }
}

function js_EXCO_ForcePush()
{
    this.exco_approval = new js_EXCOAgent_Approval();
    this.page_setup = function()
    {
        if (top == self)
        {
            window.open('./BranchSetupMain.aspx?load=BranchLookup.aspx', '_top');
        }
        else if (top.isAuthenticated())
        {   
            this.load_defaults();
        }
        else
        {
            spawn_errpage("This module requires login!");
        }      
    }
    this.load_defaults = function()
    {
        this.exco_approval.init_state();           
    }
    
    this.display_file_listing = function()
    {        
        AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. DISPLAYING OF FILES FAILED.';
            alert(errmsg);        return;     
        } 
        var branch = this.exco_approval.get_selected_agent();
        
        js_EXCOpopup.open_msg_prompt("GETTING FILES");
        
        IRSWorld.EXCO.EXCO_ForcePush.get_exco_files(branch,js_EXCOFP.display_file_listing_callback)
    }
    this.display_file_listing_callback = function(result)
    {
        js_EXCOpopup.close_msg_prompt();
        
        if (result.error != null)
        {
            spawn_errpage(result.error);
            return;
        }
        
        datatable = result.value;
        dbgrid = js_EXCOFP.create_filelist_grid()
        dbgrid.clearFilter();                    
        dbgrid.filter.enabled = false;  
        dbgrid.bind(datatable, 'tdEXCOfilelisting', 'tblEXCOfilelisting');
        
        if (datatable.Rows.length > 0 )
        {
            document.getElementById("btnView").disabled = false;
            document.getElementById("btnViewCancel").disabled = false;
        }
        else
        {
            document.getElementById("btnView").disabled = true;
            document.getElementById("btnViewCancel").disabled = true;
        }
    }
     this.create_filelist_grid = function()
    {
         gv = new datagrid();             
         {
	        var col = new datagrid_column(); //0
	        col.align = 'left';
	        col.header = 'SEQ ID';
	        col.datafield = 'SEQ_ID';
            col.styles.cell += 'width:auto;display:none';
	        col.styles.header += 'display:none';
	        col.styles.datafield += 'display:none';
		    
	        gv.addColumn(col);
        }       
        {
	        var col = new datagrid_column(); // 2
	        col.header = 'FILENAME';
	        col.datafield = 'FLDFILENAME';
	        col.align = 'right';
	        col.styles.cell += 'width:auto';
	        col.editable = false;
	        gv.addColumn(col);
        }
        {
	        var col = new datagrid_column(); // 3
	        col.align = 'left';
	        col.header = 'UPLOAD DATE';
	        col.datafield = 'FLDUPLOADDATE';
	        col.editable = false;            
	        gv.addColumn(col);
        }
        {
	        var col = new datagrid_column(); // 4
	        col.header = 'UPLOADED BY';
	        col.datafield = 'FLDUPLOADEDBY';
	        col.align = 'left';
	        col.editable = false;
	        col.styles.cell += 'width:auto';
	        gv.addColumn(col);
        }       
            
            gv.row.onclick = 'js_EXCO.grid_click';                      
            gv.row.normal_style = 'background-color: #f7ffff;';                      
            return gv;
    }
    
}