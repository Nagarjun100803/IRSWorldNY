function execute_ajx_command(param)
{
    if (cls_IRIS != null && cls_IRIS != "undefined")
    {
        cls_IRIS.do_gridsearch(param);
    }
}
function IRIS()
{
        this.ajax_grid      = null;
        this.ajax_container = null;
        this.strsearch      = null;
        this.url            = "../AjxPage/ajaxgrid.aspx";
        this.ctrl           = null;   
        this.data_ = new data();
        /* USER DEFINED METHOD */        
        
        this.do_gridsearch = function (parameter)
        {
            if (parameter != null && parameter != "undefined")
            {
                var command_param = parameter.split("$");

                if (command_param.length > 0)
                {
                    if (command_param[0] == "ajx_selected")
                    {
                        var command_values = command_param[1].split("~"); // TO GET FIELD VALUE                        
                        
                        controls.hide_div('DIV_Search',true);
                        controls.hide_div('div_results',false);
                        this.data_.get_data(command_values);
                    }                
                    this.ajax_grid.process_ajax(parameter); // FROM TEMPLATE (process_ajax)
                }
            }
        }        
        
        this.setsearch = function()
        {
            var vr              =   "where";
            var cif             =   "1072121409000010"; //get the cif of the internet user for filtering.            
            var refnum          =   document.getElementById("txtRefno").value;
            var dtfrom          =   document.getElementById("txtFDate").value;
            var dtto            =   document.getElementById("txtTDate").value;            
            var benelname       =   document.getElementById("txtBeneLname").value;
            var benefname       =   document.getElementById("txtBeneFname").value;                    
            var benemname       =   document.getElementById("txtBeneMname").value;            
            var acct            =   document.getElementById("txtAcct").value;                    
            var ck              =   document.getElementById("txtCK").value;            
            
            vr += " RemID = '" + cif + "'";            
            if (refnum.length != 0)
            {
                vr += " ReferenceNo = '" + refnum + "'";
                
            }          
            if (dtfrom.length != 0 && dtto.length != 0)
            {                
                vr += " and ";
                vr += "  convert(varchar|trandate|101) between '" + dtfrom + "' and '" + dtto + "'";  // piped used in convert(datetime|....) method & replaced (,) comma in SP.
            }                                
            if (benelname.length != 0)
            {
                vr += " and ";                
                vr += " benelname like '%" + benelname + "%'";
            }
            if (benefname.length != 0)
            {
                vr += " and ";                
                vr += " benefname like '%" + benefname + "%'";
            }
            if (benemname.length != 0)
            {
                vr += " and ";                
                vr += " benemname like '%" + benemname + "%'";
            }
             if (acct.length != 0)
            {
                vr += " and ";                
                vr += " account = '" + acct + "'";
            }
            if (ck.length != 0)
            {
                vr += " and ";                
                vr += " GFMC = '" + ck + "'";
            }                                                
            this.strsearch = vr;
        }  
       this.search = function()
    {   
        if (this.validate_input())
        {         
            this.setsearch();                                
            var sql = "sp_irsIRISvw~" + this.strsearch;
            this.ajax_grid = new grid_selector();                                                     
            this.ajax_grid.open_selection_grid(this.url, sql, 0, this.ajax_container );
        }else{alert(this.error_input);}
    }    
    this.validate_input = function()
    { 
	    this.commons     = new common_method();        
	    this.error_input = "";   
		
	    if (this.commons.to_validate_bdate(document.getElementById("txtFDate").value) == false)
	    {           
		    this.error_input += "From date transaction is invalid date.\n";            
	    }
	    if (this.commons.to_validate_bdate(document.getElementById("txtTDate").value) == false)
	    {           
		    this.error_input += "To date transaction is invalid date.\n";            
	    }
	    if(this.commons.is_empty(trim(document.getElementById("txtFDate").value)) == true && this.commons.is_empty(trim(document.getElementById("txtTDate").value)) == true)
	    {
	        this.error_input += "Transaction Dates are required fields.\n";
        }
        if (this.error_input != '')
	    {   
		    return false;
	    }   
	    return true;     
    }
}

function controls()
{
    this.hide_div = function(div_id, show)
    {
        if (!show)
        {
            document.getElementById(div_id).style.display = 'block';
        }
        else
        {
            document.getElementById(div_id).style.display = 'none';
        }
    }
}
function data()
{   this.param  = " where SEQ_id = ";
    this.val    = null;  
    
    this.setdetails = function(dataset)
    {        
        if (dataset != null && typeof(dataset) == "object" && dataset.Tables != null)
        {
            var rname = dataset.Tables[0].Rows[0]['RemLname'] + ', ' + dataset.Tables[0].Rows[0]['RemFname'] + ' ' + dataset.Tables[0].Rows[0]['RemMname']
            var bname = dataset.Tables[0].Rows[0]['BeneLname'] + ', ' + dataset.Tables[0].Rows[0]['BeneFname'] + ' ' + dataset.Tables[0].Rows[0]['BeneMname']            
            
            document.getElementById('tranx_date').innerHTML     = dataset.Tables[0].Rows[0]['TranDate'];
            document.getElementById('clearing_date').innerHTML  = dataset.Tables[0].Rows[0]['ClearingDate'];
            document.getElementById('date_imp').innerHTML       = dataset.Tables[0].Rows[0]['DateImplemented'];
            document.getElementById('tranx_ref').innerHTML      = dataset.Tables[0].Rows[0]['ReferenceNo'];
            document.getElementById('rem_office').innerHTML     = dataset.Tables[0].Rows[0]['BranchName'];
            document.getElementById('rem_cif').innerHTML        = dataset.Tables[0].Rows[0]['RemID'];
            document.getElementById('Rem_name').innerHTML       = rname;
            document.getElementById('Bene_name').innerHTML      = bname;
            document.getElementById('Gross_amt').innerHTML      = dataset.Tables[0].Rows[0]['GrossRemAmount'];
            document.getElementById('Net_amt').innerHTML        = dataset.Tables[0].Rows[0]['Acctcurr'] + ' ' + dataset.Tables[0].Rows[0]['NettoBeneAmount'];
            document.getElementById('fx_rate').innerHTML        = 0.00;//to be verified
            document.getElementById('product_type').innerHTML   = dataset.Tables[0].Rows[0]['Product_Type_Desc'];
            document.getElementById('acctno').innerHTML         = dataset.Tables[0].Rows[0]['account'];
            document.getElementById('GFMC').innerHTML           = dataset.Tables[0].Rows[0]['GFMC'];
            document.getElementById('tranx_status').innerHTML   = dataset.Tables[0].Rows[0]['Transaction_status'];
            document.getElementById('Sys_source').innerHTML     = 'IRS WORLD';           
        }
    }    
    this.get_data = function(val)
    {
        this.val = this.param + "'" + val + "'";
        IRISMain.get_details(this.val, this.get_data_callback)
    }    
    this.get_data_callback = function(result)
    {
         if (result != null && result != "undefined")
        {
            var obj = new data();
            obj.setdetails(result.value);            
        }     
        else{alert('No data found!');}
    }
}

