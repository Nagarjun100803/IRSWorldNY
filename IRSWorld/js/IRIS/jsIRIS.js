
function fixdim()
{
    if (top.document.getElementById('divCenter'))
    {
        var dvcent = top.document.getElementById('divCenter');
        var div = document.getElementById('DIV_Search');
        if (div)
        {
            div.style.height = (parseInt(dvcent.offsetHeight) - 30) + 'px';
            div.style.widht = parseInt(dvcent.offsetWidht) + 'px';
            var td = document.getElementById('tdsearch');
            td.style.height = div.style.height;
            td.style.width = div.style.width;
        }
        
        div = document.getElementById('div_results');
        if (div)
        {
            div.style.height = (parseInt(dvcent.offsetHeight) - 30) + 'px';
            div.style.widht = parseInt(dvcent.offsetWidht) + 'px';
            var td = document.getElementById('tdresult');
            td.style.height = div.style.height;
            td.style.width = div.style.width;
        }
    }
}

document.onkeydown = function(ev) 
{
    var key;
    ev = ev || event;
    key = ev.keyCode;
    if (key == 13) 
    {
       cls_IRIS.search();
    }
}
function execute_ajx_command(param)
{
    if (cls_IRIS != null && cls_IRIS != "undefined")
    {
        cls_IRIS.do_gridsearch(param);
    }
}
function IRIS()
{
    this.ajax_grid          = null;
    this.ajax_container     = null;
    this.strsearch          = null;    
    this.ctrl               = null;   
    this.msg_prompt_control = null;
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
                    
                    this.ajax_grid.process_ajax(parameter);
                    this.data_.get_data(command_values);
                    
                    return;
                }

                this.ajax_grid.process_ajax(parameter); // FROM TEMPLATE (process_ajax)
            }
        }
    }
    this.setsearch = function()
    {
        var vr = trim(document.getElementById("txtRefno").value) + '~' +      // refnum
                trim(document.getElementById("txtFDate").value) + '~' +       // dtfrom
                trim(document.getElementById("txtTDate").value) + '~' +       // dtto
                trim(document.getElementById("txtRemLname").value) + '~' +    // remlname
                trim(document.getElementById("txtRemFname").value) + '~' +    // remfname
                trim(document.getElementById("txtBeneLname").value) + '~' +   // benelname
                trim(document.getElementById("txtBeneFname").value) + '~' +   // benefname
                trim(document.getElementById("txtAcct").value) + '~' +        // acct
                trim(document.getElementById("txtAcct").value)                // ck
                
        return vr;
    
        /* Re-implement in stored proc
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
        */          
        /* originally commented codes
        if (remcif.length != 0)
        {
            if (i != 0)
            {
                vr += " and ";
            }
            i = 1;
            vr += " remid = '" + remcif+ "'";
        } */ 
        /* reimplement in stored proc         
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
        */
        /* originally commented codes
        if (remmname.length != 0)
        {
            if (i != 0)
            {
                vr += " and ";
            }
            i = 1;
            vr += " remmname like '%" + remmname + "%'";
        } */ 
        /* reimplement in stored proc          
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
        */
        /* originally commented code
        if (benemname.length != 0)
        {
            if (i != 0)
            {
                vr += " and ";
            }
            i = 1;
            vr += " benemname like '%" + benemname + "%'";
        }*/
        /* reimplement in stored proc
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
        */
    } 
     
    this.search = function()
    {   
        if (this.validate_input())
        {    
            //commented to reimplement search parsing in stored proc     
            //this.setsearch();
            //
            //var sql = "sp_irsIRISvw~" + this.strsearch;
            //var sql = "sp_irsIRISvw~" + this.setsearch();
            var sql = "sp_irsIRISvw2~" + this.setsearch();
            this.ajax_grid = new grid_selector();                                                     
            this.ajax_grid.open_selection_grid(null, sql, 0, this.ajax_container );
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
	    if((this.commons.is_empty(trim(document.getElementById("txtFDate").value)) == true | this.commons.is_empty(trim(document.getElementById("txtTDate").value)) == true) & this.commons.is_empty(trim(document.getElementById("txtRefno").value)))
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
{   
    this.param = " where SEQ_id = ";
    this.val   = null;  
    
    this.setdetails = function(dataset)
    {        
        if (dataset != null && typeof(dataset) == "object" && dataset.Tables != null)
        {
            var rname = dataset.Tables[0].Rows[0]['RemLname'] + ', ' + dataset.Tables[0].Rows[0]['RemFname'] + ' ' + dataset.Tables[0].Rows[0]['RemMname']
            var bname = dataset.Tables[0].Rows[0]['BeneLname'] + ', ' + dataset.Tables[0].Rows[0]['BeneFname'] + ' ' + dataset.Tables[0].Rows[0]['BeneMname']            
            
            setText("tranx_date",dateToString(dataset.Tables[0].Rows[0]['TranDate'])); 
            setText("clearing_date",dateToString(dataset.Tables[0].Rows[0]['ClearingDate']));
            setText("date_imp",dateToString(dataset.Tables[0].Rows[0]['DateImplementedLocal']));
            setText("tranx_ref",dataset.Tables[0].Rows[0]['ReferenceNo']);
            setText("rem_office",dataset.Tables[0].Rows[0]['BranchName']);
            setText("rem_cif",dataset.Tables[0].Rows[0]['RemID']); 
            setText("Rem_name",rname); 
            setText("Bene_name",bname); 
            setText("Gross_amt",formatNumber(dataset.Tables[0].Rows[0]['GrossRemAmount'],2)); 
            setText("Net_amt",dataset.Tables[0].Rows[0]['Acctcurr'] + ' ' + formatNumber(dataset.Tables[0].Rows[0]['NettoBeneAmount'],2));            
            setText("fx_rate",formatNumber(dataset.Tables[0].Rows[0]['ForexRemittingToAccount'],4));            
            setText("product_type",dataset.Tables[0].Rows[0]['Product_Type_Desc']);
            setText("acctno",dataset.Tables[0].Rows[0]['account']);             
            setText("GFMC",dataset.Tables[0].Rows[0]['GFMC']);             
            setText("tranx_status",dataset.Tables[0].Rows[0]['Transaction_status']);             
            setText("Sys_source",dataset.Tables[0].Rows[0]['remitmethod']); 
            setText("Source",dataset.Tables[0].Rows[0]['source']);
            setText("3DBankName",dataset.Tables[0].Rows[0]['PayBankName']);
            
        }
    }    
    this.get_data = function(val)
    {
        AjaxPro.timeoutPeriod = 1000 * 1000;
        AjaxPro.onTimeout = function()
        {
            this.close_msg_prompt(); alert("SERVER NOT RESPONDING. UNABLE TO DISPLAY RECORD."); return;
        }
        
        this.open_msg_prompt("LOADING PLEASE WAIT");
        this.val = this.param + "'" + val + "'";

        IRSWorld.IRIS.IRISMain.get_details(this.val, this.get_data_callback)
    }    
    this.get_data_callback = function(result)
    {
        cls_IRIS.data_.close_msg_prompt();
    
         if (result != null && result != "undefined")
        {
            var obj = new data();
            obj.setdetails(result.value);            
        }     
        else{alert('No data found!');}
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
}

