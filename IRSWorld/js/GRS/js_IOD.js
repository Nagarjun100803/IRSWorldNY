var datatable = null;
var dbgrid = null;
function js_IOD()
{
    var branchcode = this.get_ddlvalue('ddBranchCode',false);
    var tranxdate = this.get_text_value('txtDate');
    
    this.init_state = function()
    {
        
    }
    
    this.load_iod_data = function()
    {
    }
    
    this.btnSearch_Click = function()
    {
        this.load_iod_data();
    }
    
    this.create_grid  = function()
    {
            gv = new datagrid();     
            
            {
		        var col = new datagrid_column(); // 0
		        col.header = 'REMITTER';
		        col.datafield = 'remitter'; //used in alias SQL
		        col.align = 'right';
		        col.styles.cell += 'width:auto;text-align:right;';
		        col.editable = false;
		        gv.addColumn(col);
            }
           {
		        var col = new datagrid_column(); // 1
		        col.header = 'BENEFICIARY';
		        col.datafield = 'beneficiary';
		        col.align = 'right';
		        col.editable = false;
		        col.styles.cell += 'width:auto;text-align:right;';
		        gv.addColumn(col);
            }
            {
		        var col = new datagrid_column(); // 2
		        col.header = 'AMOUNT';
		        col.datafield = 'amount';
		        col.align = 'right';			        		
		        col.styles.cell += 'width:auto;text-align:right;';
		        gv.addColumn(col);
            }
           
                // for footer                     
                //gv.row.onclick = 'select_batch'
                gv.row.normal_style = 'background-color: #f7ffff;';                      
                return gv;
        
    }
    this.load_datagrid = function(result)
    {
        datatable = result.value.Tables[1];       
        dbgrid = ide.create_search_grid();                        
        dbgrid.filter.enabled = false;                    
        dbgrid.bind(datatable, 'tdIOD', 'tblIOD');       
    }
    
    this.get_ddlvalue = function(objid,istext)
    {
        var cbo     =  document.getElementById(objid);
        var indx    =  cbo.selectedIndex;            
        
        try
        {
            if (istext)
            return cbo[indx].innerText;
            else
            return cbo[indx].value;
        }
        catch(e)
        {
            if (istext)
            return cbo.innerText;
            else
            return cbo.value;
        }
     }
     this.get_text_value = function(objectid)
     {
        return document.getelementById(objectid).value;
     }
    
}