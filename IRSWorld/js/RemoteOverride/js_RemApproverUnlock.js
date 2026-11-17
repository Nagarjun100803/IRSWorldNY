var dbgrid;
var userRole;
var branchcode;

function page_load()
{
    userRole = IRSWorld.RemoteOverride.ApproverUnlock.isSystemAdmin().value;
    userRole == true ? document.getElementById("divForAdmin").style.display = 'block' : document.getElementById("divForAdmin").style.display = 'none';
    
    var getBranch = new getBranches();
    getBranch.Load_Branches();
    
    populate_rec();
}

function create_grid()
{
    dbgrid = new datagrid();
    {
        var col = new datagrid_column();
        col.header = 'SEQ_ID';
        col.styles.header += 'display:none;';
        col.styles.datafield += 'display:none;';
        col.datafield = 'SEQ_ID';
        col.align = 'left';
        col.styles.cell += 'width:0%;';
        col.styles.cell += 'display:none;';
        dbgrid.addColumn(col);
    
        var col = new datagrid_column();
        col.header = 'Product Charges ID';
        col.datafield = 'PRODUCTCHARGESID';
        col.align = 'left';
        col.styles.cell += 'width:13%;';
        col.editable = false;
        dbgrid.addColumn(col);
        
        var col = new datagrid_column();
        col.header = 'Date';
        col.datafield = 'RODATE';
    	col.align = 'left';
    	col.styles.cell += 'width:13%';
    	dbgrid.addColumn(col);
        
        var col = new datagrid_column();
        col.header = 'Transaction Description';
        col.datafield = 'TRANDESC';
        col.align = 'left';
        col.styles.cell += 'width:28%;';
        col.editable = false;
        dbgrid.addColumn(col);
        
        var col = new datagrid_column();
        col.header = 'Remarks';
        col.datafield = 'SPLMSG';
        col.align = 'left';
        col.styles.cell += 'width:38%;';
        col.editable = false;
        dbgrid.addColumn(col);
        
        var col = new datagrid_column();
        col.header = ' Unlock <br /><input type=checkbox id=chkAll style=background-color:transparent;border:none onclick=javascript:check_box(); />';
        col.datafield = 'LOCKED';
        col.align = 'center';
        col.styles.cell += 'width:8%;';
        col.type = 'check';
        col.onclick = 'chkClick';
        dbgrid.addColumn(col);
    }
    
    dbgrid.classNames.table = 'table_body';
    dbgrid.rows_page = 12;
    dbgrid.row.normal_style = 'background-color: #f7ffff;'; 
   // dbgrid.footer.none = true;
}

function populate_rec()
{
   
    branchcode = IRSWorld.RemoteOverride.ApproverUnlock.getUserBranch().value;

     
    IRSWorld.RemoteOverride.ApproverUnlock.checkLockedItems(branchcode, bind_grid);
   
}

function bind_grid(branchcode)
{
    if (branchcode.error != null)
    {
        spawn_errpage(branchcode.error);
        return;
    }
    
    create_grid();
    ajax_show_status(true);
    
    dbgrid.bind(branchcode.value, 'tdApproverUnlock', 'gridApproverUnlock');
    var rlength = branchcode.value.Rows.length;
    toggle_checkbox(rlength);
    
     
    ajax_show_status(false);
} 

function    toggle_checkbox(rlength)
{   
    var frm = document.forms[0];
    
    if (frm != null)
    {
        for(i = 0 ; i < frm.length; i++)
        { 
            var obj = frm.elements[i];
            try
            {
                if (obj.type = "checkbox")
                {
                    obj.disabled = false;
                    if (obj.id == "chkAll" && rlength == 0)
                    {   
                        obj.style.display = 'none';
                    }
                    obj.checked = false;
                }
            }
            catch(e){}
        }
    }
}

function chkClick()
{   
    var rows = document.getElementById(dbgrid.id).rows;
    var chkID = new Array();
    var chkState = new Array();
    var seqid = new Array();
    var seq_id="";
	                
	for (var i = 1; i < rows.length; i++)
	{
	    try
	    {
		    var sels = rows.item(i).cells;
		    var tdCurr = sels.item(5);
		     
		    if(tdCurr == null)
		    {
		        break;
		    } 
		    else
		    {               
	            chkID[i] = tdCurr.id;
	            chkState = tdCurr.innerHTML.split(' ');
	        
	        
	            if (chkState[4] == 'CHECKED')
	            {
	                seqid = sels.item(0).innerHTML.split(' ');
	                seqid = seqid[23].split('=');
	                if (seq_id == "")
	                {
    	                seq_id = seqid[1].substring(0, seqid[1].length-1);
    	            }
    	            else
    	            {
    	                seq_id = seq_id+","+seqid[1].substring(0, seqid[1].length-1);
    	            }
	            }
	        }
	    }
		                
	    catch (e)
	    {
		    alert(e);
	    }
    }
     return seq_id;
}

function unlock()
{
    branchcode = IRSWorld.RemoteOverride.ApproverUnlock.getUserBranch().value;
    IRSWorld.RemoteOverride.ApproverUnlock.saveUnlock(chkClick(), branchcode);
    populate_rec();
}

function check_box()
{   
    var frm = document.forms[0];
    
    if (frm != null)
    {
        for(i = 0 ; i < frm.length; i++)
        { 
            var obj = frm.elements[i];
            try
            {
                if (obj.id == "chkAll")
                {
                    chkBox = obj.checked;
                }
                if (obj.type == "checkbox")
                {
                    chkBox == true ? obj.checked = true : obj.checked = false;
                }
                
            }
            catch(e){}
        }
    }
}

function getBranches()
{
    this.Load_Branches = function()
    {
        IRSWorld.RemoteOverride.ApproverUnlock.getBranches(this.load_ddlBranch_callback);
    }
    
    this.load_ddlBranch_callback = function(result)
    {
        if (result.value != null && result.value != undefined)
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddlBranch", result.value, "BranchCode", "BranchName");
            
            var branchcode = IRSWorld.RemoteOverride.ApproverUnlock.getUserBranch().value;
            this.commons.to_set_dropdownlist_index('ddlBranch',branchcode);
        }
    }
}

function chngBranch()
{
    branchcode = document.getElementById('ddlBranch').value;
    
    IRSWorld.RemoteOverride.ApproverUnlock.checkLockedItems(branchcode, bind_grid);
}