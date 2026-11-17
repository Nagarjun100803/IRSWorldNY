function execute_ajaxgridview_command(param)
{
    if (aggr != null && aggr != "undefined")
    {
        aggr.do_ajaxgridview(param);
    }
}

function showpanel(divname)
{   
    var div = document.getElementById(divname);
    
    divmask = new maskpanel();
    divmask.show();
    
    document.getElementById(divname).style.display = "block";
    
    div.style.top = (((document.documentElement.clientHeight / 2) + document.documentElement.scrollTop) - (div.offsetHeight / 2)) + 'px';
    div.style.left = (((document.documentElement.clientWidth / 2) + document.documentElement.scrollLeft) - (div.offsetWidth / 2)) + 'px';
}

function hidepanel(divname)
{
    document.getElementById(divname).style.display = "none";
    divmask.hide();
} 


function GFCInquiry()
{
    this.CardNum = null;
    this.CKNum = null;
    this.Bal = null;
    this.Curr = null; 
    var icls = this;
   
    var oldatatable = null;
    var dbgrid = null;
    var dbgrid2 = null;    
    var datatable = null;
    var opt_values = new Array(2);
    var pg_no = 1;

    var rowchanged = false;

    this.btnSubmit_click = function()
    {
        var result = this.validate();
        
        if (result == "")
        {
            showpanel('divPopProcess');
            this.getAvailableBalance();
        }
        else
        {
            alert(result);
        }
    }
    
    this.btnReset_click = function()
    {
        document.getElementById('txtGFCNo').innerText = "";
        document.getElementById('txtPIN').innerText = "";
    }
   
    this.btnShowTrxn_click = function()
    {
        showpanel('divPopProcess'); 
        
        AjaxPro.timeoutPeriod = 100 * 1000;
        
        AjaxPro.onTimeout = function()
        {
            document.getElementById('DIVGRIDTRXNHISTORY').style.display = 'none'; 
            hidepanel('divPopProcess'); 
            alert("SESSION TIMEOUT");
            return; 
        }   
                
        IRSWorld.GFC.GFCInquiry.getTransactionHistory(this.CKNum, this.getTransactionHistory_callback);
    }
   
    this.getTransactionHistory_callback = function(response)
    {
        hidepanel('divPopProcess');
         
        if (response.error != null)
        {
            document.getElementById('DIVGRIDTRXNHISTORY').style.display = 'none';    
            alert(response.error.Message);            
        }
        else
        {
             document.getElementById('DIVGRIDTRXNHISTORY').style.display = 'block'; 
             icls.load_trxn_grid(response.value, 'tdTrxnHistory');
             //icls.load_trxn_grid(response.value, 'tdRepGrid');
             icls.load_trxn_grid_rep(response.value);
        }   
    }  
   
    this.btnPrint_click = function()
    { 
        this.printDiv('DIVREPORTVIEW');
    }
   
    this.btnBack_click = function()
    {
        this.btnReset_click();
        this.changeMode(0);
    }
   
    //private Methods
   
    this.init = function()
    {    
        this.changeMode(0);
    }  
   
    this.validate = function()
    {
        var a = document.getElementById("txtGFCNo").value;
        if (document.getElementById("txtGFCNo").value.toString().length == 0){ return "Required GFC Number"; }
        else if (document.getElementById("txtGFCNo").value.toString().length != 16) { return "Invalid GFC Number"; }
        //else if (document.getElementById("txtGFCNo").value.toString().substring(0, 6) != "592108") { return "Invalid GFC Number"; }
        else if (document.getElementById("txtPIN").value.toString().length == 0) { return "Required PIN"; }
        else if (document.getElementById("txtPIN").value.toString() != document.getElementById("txtGFCNo").value.toString().substring(12, 16)) { return "Invalid PIN"; }
             
        return "";
    }
   
    this.getAvailableBalance = function()
    {
        this.readOnly("txtAvailableBal", false); 
        this.readOnly("txtGFCNum", false);
        this.readOnly("txtCurr", false);  
       
        AjaxPro.timeoutPeriod = 100 * 1000;
        
        AjaxPro.onTimeout = function()
        {
            alert("SESSION TIMEOUT");
            hidepanel('divPopProcess'); 
            return; 
        }        
        
        IRSWorld.GFC.GFCInquiry.getAvailableBalance(document.getElementById("txtGFCNo").value, this.getAvailableBalance_callback);
    }
   
    this.getAvailableBalance_callback = function(response)
    {
         hidepanel('divPopProcess');
    
        if (response.error != null)
        {
            alert(response.error.Message);
        }
        else          
        {
            var res = response.value.toString().split('|');
        
            icls.CardNum = document.getElementById("txtGFCNo").value;
            icls.CKNum = res[0].toString();
            icls.Bal = res[1].toString();
            icls.Curr = res[2].toString();
            
            document.getElementById('txtAvailableBal').innerText = icls.Bal;
            document.getElementById('txtGFCNum').innerText = icls.CardNum;
            document.getElementById('txtCurr').innerText = icls.Curr; 
                     
            icls.updateReportView();
            icls.changeMode(1);
           
            this.value = icls;  
        }
        
        icls.readOnly("txtAvailableBal", true); 
        icls.readOnly("txtGFCNum", true);
        icls.readOnly("txtCurr", true); 
       
    } 
   
    this.changeMode = function(mode)
    {
        switch(mode)
        {
            case 0:
                  //document.getElementById('DIVGFCSEARCH').style.display = 'block';
                  document.getElementById('DIVFRONTPAGE').style.display = 'block'; 
                  document.getElementById('DIVGFCDETAILS').style.display = 'none';
                  document.getElementById('DIVGRIDTRXNHISTORY').style.display = 'none';                   
                  document.getElementById('DIVMAIN').style.display = 'none'; 
                  break;
            case 1:
                  //document.getElementById('DIVGFCSEARCH').style.display = 'none';
                  document.getElementById('DIVFRONTPAGE').style.display = 'none'; 
                  document.getElementById('DIVGFCDETAILS').style.display = 'block';
                  document.getElementById('DIVGRIDTRXNHISTORY').style.display = 'none';
                  document.getElementById('DIVMAIN').style.display = 'block';             
                  break;
            default:
                  //document.getElementById('DIVGFCSEARCH').style.display = 'none';
                  document.getElementById('DIVFRONTPAGE').style.display = 'none'; 
                  document.getElementById('DIVGFCDETAILS').style.display = 'none';
                  document.getElementById('DIVGRIDTRXNHISTORY').style.display = 'none';
                  document.getElementById('DIVMAIN').style.display = 'block';              
                  break;    
        } 
    }  
   
   this.enable = function(oName)
    {
        var id = document.getElementById(oName);
        var cset = id.disabled;
       
        id.disabled = cset == "false";
    }  

    this.disable = function(oName)
    {        
        var id = document.getElementById(oName);
        var cset = id.disabled;
       
        id.disabled = (cset == "false") ? "true" : "false";
    }  
   
    this.readOnly = function(oName, isReadOnly)
    {
        if (isReadOnly == true)
        {
            document.getElementById(oName).readOnly = true;
        }
        else
        {
            document.getElementById(oName).readOnly = false;
        }
    }      
   
    this.on_key_press = function()
    {
        if (document.getElementById('DIVFRONTPAGE').style.display == 'block')
        {
            if (window.event.keyCode == 13)
            {
                document.getElementById('btnSubmit').click();
            }        
        }
    }      

    this.load_trxn_grid = function(dt, table)
    {   
        try
        {
//        dbgrid = this.create_grid();
//        datatable = dt;
//        dbgrid.clearFilter();                    
//        dbgrid.filter.enabled = false;  
//        dbgrid.bind(datatable, table, 'tblGFCInquiry');              
//        dbgrid.refresh(datatable);
//        oldatatable = datatable;
        dbgrid = this.create_grid();
        dbgrid.clearFilter();
        dbgrid.filter.enabled = false;
        dbgrid.bind(dt, table, 'tblGFCInquiry');
        dbgrid.refresh(dt);
        oldatatable = dt;
        }
        catch(e){}
    }

    this.load_trxn_grid_rep = function(dt)
    {   
        try
        {
        dbgrid2 = this.create_grid();        
        dbgrid2.clearFilter();                    
        dbgrid2.filter.enabled = false;
        dbgrid2.rows_page = dt.Rows.length;
        dbgrid2.bind(dt, 'tdRepGrid', 'tblGFCInquiry2');
        dbgrid2.refresh(dt);
        oldatatable = dt;
        }
        catch(e){}
    }    
       
    this.create_grid = function()
    {
        gv = new datagrid();
       
         {
		    var col = new datagrid_column(); 
		    col.align = 'center';
		    col.header = 'TranDate';
		    col.datafield = 'TransactionDate';
            col.styles.cell += 'width:auto';
            col.editable = false; 
	    
		    gv.addColumn(col);
        }

        {
		    var col = new datagrid_column(); 
		    col.header = 'Description';
		    col.datafield = 'Description'; 
		    col.align = 'left';
		    col.styles.cell += 'width:auto';
		    col.editable = false;
		    gv.addColumn(col);
        }
       
        {
		    var col = new datagrid_column(); 
		    col.header = 'Debit';
		    col.datafield = 'Debit'; 
		    col.align = 'right';
		    col.styles.cell += 'width:auto';
		    col.editable = false;
		    gv.addColumn(col);
        }      
          
        {
		    var col = new datagrid_column(); 
		    col.header = 'Credit';
		    col.datafield = 'Credit'; 
		    col.align = 'right';
		    col.styles.cell += 'width:auto';
		    col.editable = false;
		    gv.addColumn(col);
        }              
       
        {
		    var col = new datagrid_column(); 
		    col.header = 'Balance';
		    col.datafield = 'RunningTotal'; 
		    col.align = 'right';
		    col.styles.cell += 'width:auto';
		    col.editable = false;
		    gv.addColumn(col);
        }            
            return gv;
    }    
   
    this.printDiv = function(divName)
    {
//        var headstr = "<html><head><title></title></head><body>";
//        var footstr = "</body>";
//        var newstr = document.all.item(divName).innerHTML;
//        var oldstr = document.body.innerHTML;
//        document.body.innerHTML = headstr + newstr + footstr;
//        window.print();
//        document.body.innerHTML = oldstr;
        document.getElementById('tbl_main').style.display = 'none'
        document.getElementById('DIVREPORTVIEW').style.display = 'block' 

        window.print();
       
        document.getElementById('tbl_main').style.display = 'block'
        document.getElementById('DIVREPORTVIEW').style.display = 'none'
    }  
   
    this.updateReportView = function()
    {
        document.getElementById('txtRepBal').innerText = document.getElementById('txtAvailableBal').value;
        document.getElementById('txtRepCardNo').innerText = document.getElementById('txtGFCNum').value;
        document.getElementById('txtRepCurr').innerText = document.getElementById('txtCurr').value; 
    }    
}