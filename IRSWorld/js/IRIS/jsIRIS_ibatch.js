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
        var vr = "where";
        var i = 0;
        var refnum          =   document.getElementById("txtRefno").value;
        var dtfrom          =   document.getElementById("txtFDate").value;
        var dtto            =   document.getElementById("txtTDate").value;        
        var remlname        =   document.getElementById("txtRemLname").value;
        var remfname        =   document.getElementById("txtRemFname").value;      
        var benelname       =   document.getElementById("txtBeneLname").value;
        var benefname       =   document.getElementById("txtBeneFname").value;                            
        var acct            =   document.getElementById("txtAcct").value;                    
        var ck              =   document.getElementById("txtAcct").value;            
        
        if (refnum.length != 0)
        {
            vr += " ReferenceNo = '" + trim(refnum) + "'";
            i = 1;
        }          
        if (dtfrom.length != 0 && dtto.length != 0)
        {                
            if (i != 0)
            {
                vr += " and ";
            }
            i = 1;
            vr += "  trandate between convert(datetime|'" + dtfrom + " 00:00:00') and convert(datetime|'" + dtto + " 23:59:59.997')";  // piped used in convert(datetime|....) method & replaced (,) comma in SP.
        }           
        /*if (remcif.length != 0)
        {
            if (i != 0)
            {
                vr += " and ";
            }
            i = 1;
            vr += " remid = '" + remcif+ "'";
        } */          
        if (remlname.length != 0)
        {
            if (i != 0)
            {
                vr += " and ";                
            }
            i = 1;
            vr += " remlname like '%" + trim(remlname) + "%'";                
        }                      
        if (remfname.length != 0)
        {
            if (i != 0)
            {
                vr += " and ";
            }
            i = 1;
            vr += " remfname like '%" + trim(remfname) + "%'";
        } 
        /*if (remmname.length != 0)
        {
            if (i != 0)
            {
                vr += " and ";
            }
            i = 1;
            vr += " remmname like '%" + remmname + "%'";
        } */           
        if (benelname.length != 0)
        {
            if (i != 0)
            {
                vr += " and ";
            }
            i = 1;
            vr += " benelname like '%" + trim(benelname) + "%'";
        }
        if (benefname.length != 0)
        {
            if (i != 0)
            {
                vr += " and ";
            }
            i = 1;
            vr += " benefname like '%" + trim(benefname) + "%'";
        }
        /*if (benemname.length != 0)
        {
            if (i != 0)
            {
                vr += " and ";
            }
            i = 1;
            vr += " benemname like '%" + benemname + "%'";
        }*/
         if (acct.length != 0)
        {
            if (i != 0)
            {
                vr += " and ";
            }
            i = 1;
            vr += " (account = '" + trim(acct) + "' or GFMC = '" + trim(acct) + "')";
        }
        
        if (vr == "where")
        {
            vr = "";
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
		
	    if (isValidDate(document.getElementById("txtFDate").value)[0] == false && document.getElementById("txtFDate").value.length !=0)
	    {           
		    this.error_input += "From date transaction is invalid date.\n";            
	    }
	    if (isValidDate(document.getElementById("txtTDate").value)[0] == false && document.getElementById("txtTDate").value.length !=0)
	    {           
		    this.error_input += "To date transaction is invalid date.\n";            
	    }
	    if(this.commons.is_empty(trim(document.getElementById("txtFDate").value)) == true && this.commons.is_empty(trim(document.getElementById("txtTDate").value)) == true && this.commons.is_empty(trim(document.getElementById("txtRefno").value)))
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
            
            document.getElementById('tranx_date').innerHTML     = dateToString(dataset.Tables[0].Rows[0]['TranDate']);
            document.getElementById('clearing_date').innerHTML  = dateToString(dataset.Tables[0].Rows[0]['ClearingDate']);
            document.getElementById('date_imp').innerHTML       = dateToString(dataset.Tables[0].Rows[0]['DateImplementedLocal']);
            document.getElementById('tranx_ref').innerHTML      = dataset.Tables[0].Rows[0]['ReferenceNo'];
            document.getElementById('rem_office').innerHTML     = dataset.Tables[0].Rows[0]['BranchName'];
            document.getElementById('rem_cif').innerHTML        = dataset.Tables[0].Rows[0]['RemID'];
            document.getElementById('Rem_name').innerHTML       = rname;
            document.getElementById('Bene_name').innerHTML      = bname;
            document.getElementById('Gross_amt').innerHTML      = formatNumber(dataset.Tables[0].Rows[0]['GrossRemAmount'],2);
            document.getElementById('Net_amt').innerHTML        = dataset.Tables[0].Rows[0]['Acctcurr'] + ' ' + formatNumber(dataset.Tables[0].Rows[0]['NettoBeneAmount'],2);
            document.getElementById('fx_rate').innerHTML        = formatNumber(dataset.Tables[0].Rows[0]['ForexRemittingToAccount'],4);//to be verified
            document.getElementById('product_type').innerHTML   = dataset.Tables[0].Rows[0]['Product_Type_Desc'];
            document.getElementById('acctno').innerHTML         = dataset.Tables[0].Rows[0]['account'];
            document.getElementById('GFMC').innerHTML           = dataset.Tables[0].Rows[0]['GFMC'];
            document.getElementById('tranx_status').innerHTML   = dataset.Tables[0].Rows[0]['Transaction_status'];
            document.getElementById('Sys_source').innerHTML     = dataset.Tables[0].Rows[0]['remitmethod'];
            document.getElementById('Source').innerHTML         = dataset.Tables[0].Rows[0]['source'];
            document.getElementById('DateReceived').innerHTML   = dateToString(dataset.Tables[0].Rows[0]['DateReceived']);
            document.getElementById('RefnumTheirs').innerHTML   = dataset.Tables[0].Rows[0]['RefnumTheirs'];
            document.getElementById('RemittingBankName').innerHTML      = dataset.Tables[0].Rows[0]['RemittingBankName'];
            document.getElementById('PayBankName').innerHTML    = dataset.Tables[0].Rows[0]['PayBankName'];
            document.getElementById('BranchAccountName').innerHTML      = dataset.Tables[0].Rows[0]['BranchAccountName'];
            document.getElementById('Implementation').innerHTML         = dataset.Tables[0].Rows[0]['Implementation'];
            document.getElementById('Courier').innerHTML        = dataset.Tables[0].Rows[0]['Courier'];
            document.getElementById('SpecialMessage').innerHTML = dataset.Tables[0].Rows[0]['SpecialMessage'];
            document.getElementById('Processor').innerHTML      = dataset.Tables[0].Rows[0]['Processor'];
            document.getElementById('BeneAddress').innerHTML    = dataset.Tables[0].Rows[0]['BeneAddress'];
            
        }
    }    
    this.get_data = function(val)
    {
        this.val = this.param + "'" + val + "'";
        IRIS_iBatch.get_details(this.val, this.get_data_callback)
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

