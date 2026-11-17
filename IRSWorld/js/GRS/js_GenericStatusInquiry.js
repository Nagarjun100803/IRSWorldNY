
var gsi = new js_GenericStatusInquiry()
var dbgrid;
var datatable;



function  js_GenericStatusInquiry()
{
    igsi = this;
    var common = new common_method(); // common javascript external
    var dataset_lookup;
            
    this.page_setup =  function()
    {
        if (top == self)
        {
            window.open('../../BranchSetup/BranchSetupMain.aspx?load=GenericStatusInquiry.aspx', '_top');
        }
        else if (top.isAuthenticated())
        {   
            if (gsi == null)
            {
                gsi = new js_GenericStatusInquiry()
            }
            gsi.init_state();
        }
        else
        {
            spawn_errpage("This module requires login!");
        }      
    }
        
    this.page_setup2 =  function()
    {
        
        if (gsi == null)
        {
            gsi = new js_GenericStatusInquiry()
        }
        gsi.init_state();
 
    }
    
    this.init_state = function()
    {
        if (this.dataset_lookup != null || this.dataset_lookup == undefined)
        {
            this.load_status_info();         
        }
        else
        {
            this.load_lookup_values(dataset_lookup);
        }
        
    }
    
    this.create_grid = function()
    {
            gv = new datagrid();     
            {
		        var col = new datagrid_column(); // 0
		        col.header = 'TRANXDATE.';
		        col.datafield = 'TRANXDATE'; //used in alias SQL
		        col.align = 'right';
		        col.styles.cell += 'width:auto;text-align:right;';
		        col.editable = false;
		        gv.addColumn(col);
            }   
            {
		        var col = new datagrid_column(); // 0
		        col.header = 'REMITTER';
		        col.datafield = 'REMITTER'; //used in alias SQL
		        col.align = 'left';
		        col.styles.cell += 'width:auto;text-align:right;';
		        col.editable = false;
		        gv.addColumn(col);
            }
            {
		        var col = new datagrid_column(); // 1
		        col.header = 'BENEFICIARY';
		        col.datafield = 'BENEFICIARY';
		        col.align = 'left';
		        col.editable = false;
		        col.styles.cell += 'width:auto;text-align:right;';
		        gv.addColumn(col);
            }
            {
		        var col = new datagrid_column(); // 2
		        col.header = 'AMOUNT';
		        col.datafield = 'AMOUNT';
		        col.align = 'right';			        		
		        col.styles.cell += 'width:auto;text-align:right;';
		        gv.addColumn(col);
            }
           
                // for footer                     
               
                gv.row.normal_style = 'background-color: #f7ffff;';                      
                return gv;
    }
    this.load_status_info  = function()
    {
            IRSWorld.GRS.GenericStatusInquiry.load_lookup_values(gsi.load_lookup_values)
    }
    this.load_datagrid = function(result)
    {        
        try
        {
            datatable = result.value.Tables[0];       
            dbgrid = gsi.create_grid();                        
            dbgrid.filter.enabled = false;                        
            dbgrid.bind(datatable, 'tdStatus', 'tblStatus');     
        }
        catch(e)
        {
            alert(result.error);
        }
    }   
    this.load_lookup_values = function(result)
    {
        try
        {
            common.to_load_dropdownlist('ddlStatus',null, "VALUE", "CODE",result.value.Tables[0]);
            setText("tdtransource",result.value.Tables[1].Rows[0].fldcode.toString() + " STATUS");
            return;
        }
        catch(e)
        {
        
        }
        alert("LOOKUP VALUES NOT DEFINE")
    }
    
    this.search_generic_status = function()
    {
        var todate = document.getElementById("txtToDate").value;
        var fromdate = document.getElementById("txtFromDate").value;
        var status = document.getElementById("ddlStatus").value;
        IRSWorld.GRS.GenericStatusInquiry.search_genericstatus(todate,fromdate,status, gsi.load_datagrid)        
    }
    this.btnSearch_Click = function()
    {
        this.search_generic_status();
    }
}