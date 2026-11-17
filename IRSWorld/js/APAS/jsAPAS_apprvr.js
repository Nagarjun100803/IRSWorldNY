
function execute_ajx_command(param)
{
    if (cls_APAS != null && cls_APAS != "undefined")
    { cls_APAS.do_gridsearch(param); }
}
function APAS()
{
    var cls = this;
    this.ajax_grid      = null;
    this.ajax_container = null;
    this.strsearch      = null;
    this.url            = "../AjxPage/ajaxgrid.aspx";
    this.ctrl           = null;       
    this.SEQ_ID         = null;
    this.trxn_SEQ_id    = null;
    this.Pay_Branch_code= null;
    this.MC_No          = null;
    this.Remarks        = null;
    this.ProcessedBy    = null;
    this.DateProcessed  = null;    
    this.DatePaid       = null;
    this.param          = " where SEQ_id = ";
    this.val            = null;  
    this.rembankcode    = null;
    this.trxn_seq_id    = null;
    this.approver       = null;   
    this.trxn_stat      = null; 
    this.ErrMsg         = null;
    
    this.get_apas_trxnstat = function()
    { APASMain_Apprvr.get_apas_trxnstat(this.get_apas_trxnstat_callback); }    
    this.get_apas_trxnstat_callback = function(ds)
    {
        var comm_method = new common_method();
        if (ds != null && ds != "undefined")
        { comm_method.to_load_dropdownlist("cmb_trxn_stat", ds.value, "FldCode", "FldDescription", null);return; }
    }
    
    this.set_dateprocessed = function()
    { APASMain_Apprvr.get_time_zone(this.set_dateporcessed_callback) }
    
    this.set_dateporcessed_callback = function(result)
    { cls.DateProcessed = dateToString(result.value); }
    
    this.pay = function()
    { return this.do_apas_trnx("UPDATE"); }  
    
    this.do_apas_trnx = function(instruction)
    { var s = this.construct_column_array();APASMain_Apprvr.do_process_apas(instruction,s,cls.SEQ_ID,this.do_apas_trnx_callback); }
    
    this.do_apas_trnx_callback = function(result)
    {
        if (result.value == null)
        {
            controls.hide_div('div_results',true);                 
            controls.hide_div('DIV_Search',false);
            alert("Transaction successfully processed.");            
        }        
        else{alert(result.value);}
    }
    
    this.clientdata = function()
    { APASMain_Apprvr.clientdata(this.clientdata_callback); }
    
    this.clientdata_callback = function(result)
    {
        s = result.value;
        cls.approver           = s[3];
        cls.ProcessedBy        = s[4];
        cls.DatePaid           = dateToString(s[5]);
        cls.Pay_Branch_code    = s[1];         
        cls_APAS.pay();
    }
    this.verify_password = function(uid,pwd)
    {
        if (uid.length == 0 && pwd.length == 0)
        {
            this.ErrMsg = 'ERROR: Please enter User id & password';
            alert(this.ErrMsg);
            this.audit_log('INSERT',this.ErrMsg);
            return false;
        }
        else if (uid.length == 0)
        {
            this.ErrMsg = 'ERROR: Please enter User id';
            alert(this.ErrMsg);
            this.audit_log('INSERT',this.ErrMsg);
            return false;
        }
        else if (pwd.length == 0)
        {
            this.ErrMsg = 'ERROR: Please enter password'
            alert(this.ErrMsg);
            this.audit_log('INSERT',this.ErrMsg)
            return false;
        }
        
        APASMain_Apprvr.verify_password(uid,pwd,this.verify_password_callback);
    }
        
    this.verify_password_callback = function(result)
    {   
        if (result != null && result != "undefined")        
        {
            s = result.value[0];
            if (s[0])
            {
                approval_window.close_modal_window();
                cls_APAS.clientdata();
            }
            else{alert(s[1]);}
        }
    }     
    
    this.audit_log = function(instruction,errorlog)
    {
        var data_row = new Array();
        data_row[data_row.length] = "ErrorDesc~"              +  errorlog; 
        data_row[data_row.length] = "ErrorDate~"              +  dateToString(APASMain.get_time_zone().value);
        data_row[data_row.length] = "UserID~"                 +  this.UserName;
        data_row[data_row.length] = "BranchCode~"             +  this.BranchCode;
        data_row[data_row.length] = "ModuleNm~"               +  'REACTIVATE TRANSACTION'
        var s = data_row.join('|');
        APASMain.do_process_apas(instruction,s,"AUDIT"); 
    }
    
    this.construct_column_array = function()
    {
        var data_row = new Array();
              
        data_row[data_row.length] = "SEQ_ID~"               + cls.SEQ_ID;             
        data_row[data_row.length] = "trxn_stat~1";            
        return data_row.join("|");
    } 
    
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
                    this.get_data(command_values);                
                }                
                    controls.hide_div('DIV_Search',true);
                    controls.hide_div('div_results',false); 
                    this.ajax_grid.process_ajax(parameter); // FROM TEMPLATE (process_ajax)                
            }
        }
    }
    
    /*        
        Added Date  : Jan 2010
        Purpose     : For APAS trxn search
    */
    this.search = function()
    {   
        if (this.validate_input())
        {         
            this.setsearch();                                
            var sql = "sp_APAS_search~" + this.strsearch;
            this.ajax_grid = new grid_selector();                                                     
            this.ajax_grid.open_selection_grid(this.url, sql, 0, this.ajax_container );
        }
        else{alert(this.error_input);}
    }   
    
    this.setsearch = function()
    {
        var vr = "where";
        var i = 0;
        var DatePaid    =   document.getElementById("txtDatePaid").value;        
        var Refno       =   document.getElementById("txtRefno").value;
        var BeneLname   =   document.getElementById("txtBeneLname").value;
        var BeneFname   =   document.getElementById("txtBeneFname").value;
        var BeneMname   =   document.getElementById("txtBeneMname").value;
        var Amount      =   document.getElementById("txtAmount").value;
        var PayBr       =   document.getElementById("txtPayBr").value;
        var trnx_stat   =   document.getElementById("cmb_trxn_stat").value;
        
        if (Refno.length != 0)
        { vr += " ReferenceNo = '" + Refno + "'";i = 1; }          
        if (DatePaid.length != 0)
        {                
            if (i != 0)
            { vr += " and "; }
            i = 1;vr += "  convert(varchar|TranxDate|101) = '" + DatePaid + "'";  // piped used in convert(datetime|....) method & replaced (,) comma in SP.
        }           
        if (BeneLname.length != 0)
        {
            if (i != 0)
            { vr += " and "; }
            i = 1;vr += " BeneFull_Name like '%" + BeneLname + "%'";
        }           
        if (BeneFname.length != 0)
        {
            if (i != 0)
            { vr += " and "; }
            i = 1;vr += " BeneFull_Name like '%" + BeneFname + "%'";
        }                      
        if (BeneMname.length != 0)
        {
            if (i != 0)
            { vr += " and "; }
            i = 1;vr += " BeneFull_Name like '%" + BeneFname + "%'";
        } 
        if (Amount.length != 0)
        {
            if (i != 0)
            { vr += " and "; }
            i = 1;vr += " Amount = '" + Amount + "'";
        }            
        if (PayBr.length != 0)
        {
            if (i != 0)
            { vr += " and "; }
            i = 1;vr += " PayBr like '%" + PayBr + "%'";
        }
        if (trnx_stat != "null")
        {
            if (i != 0)
            { vr += " and "; }
            i = 1;vr += " TransactionStatusCode = '" + trnx_stat + "'";
        }                        
        if (vr == "where")
        { vr = ""; }
        this.strsearch = vr;
    }      
    this.setdetails = function(dataset)
    {        
        if (dataset != null && typeof(dataset) == "object" && dataset.Tables != null)
        {   
            document.getElementById('ImplemDate').innerHTML         = dateToString(dataset.Tables[0].Rows[0]['TranxDate']);
            document.getElementById('RefNoTheirs').innerHTML        = dataset.Tables[0].Rows[0]['ReferenceNo'];
            document.getElementById('RefNoOurs').innerHTML          = dataset.Tables[0].Rows[0]['ReferenceNo'];
            document.getElementById('ORC').innerHTML                = 'ORC';
            document.getElementById('BeneName').innerHTML           = dataset.Tables[0].Rows[0]['BeneFull_Name'];
            document.getElementById('BeneAdd').innerHTML            = dataset.Tables[0].Rows[0]['Bene_Address'];
            document.getElementById('BeneContactNo').innerHTML      = dataset.Tables[0].Rows[0]['Bene_ContactNum'];
            document.getElementById('SpecialMSG').innerHTML         = dataset.Tables[0].Rows[0]['SpecialMessage'];
            document.getElementById('Purpose').innerHTML            = dataset.Tables[0].Rows[0]['Purpose'];
            document.getElementById('RemID').innerHTML              = dataset.Tables[0].Rows[0]['RemitterID'];
            document.getElementById('RemName').innerHTML            = dataset.Tables[0].Rows[0]['RemFull_Name'];
            document.getElementById('RemAdd').innerHTML             = dataset.Tables[0].Rows[0]['Rem_Adress'];
            document.getElementById('RemBank').innerHTML            = dataset.Tables[0].Rows[0]['RemittingBank'];
            document.getElementById('ProdCode').innerHTML           = dataset.Tables[0].Rows[0]['ProductName'];
            document.getElementById('CurrencyCode').innerHTML       = dataset.Tables[0].Rows[0]['Currency'];
            document.getElementById('RateUsed').innerHTML           = formatNumber(dataset.Tables[0].Rows[0]['RateUsed'],3);
            document.getElementById('Amount').innerHTML             = formatNumber(dataset.Tables[0].Rows[0]['Amount'],2);            
            document.getElementById('EVAA').innerHTML               = 'EVAA';
            document.getElementById('NSVS').innerHTML               = 'NSVS';
            document.getElementById('trxn_stat').innerHTML          = dataset.Tables[0].Rows[0]['TransactionStatus'];
            cls.SEQ_ID                                              = dataset.Tables[0].Rows[0]['SEQ_id'];                                 
            var stat                                                = dataset.Tables[0].Rows[0]['TransactionStatusCode'];
            if (stat == "3")                
            {controls.enable("activate")}            
            else 
            {controls.disable("activate")}           
            
            
            
        }
    }
    this.get_data = function(val)
    {   
        this.val = this.param + "'" + val + "'";
        APASMain_Apprvr.get_details(this.val, this.get_data_callback)
    }  
          
    this.get_data_callback = function(result)
    {
         if (result != null && result != "undefined")
        {            
            cls_APAS.setdetails(result.value);            
        }     
        else{alert('No data found!');}
    }    
      
    this.validate_input = function()
    { 
	    this.commons     = new common_method();        
	    this.error_input = "";   
		
	    if (this.commons.to_validate_bdate(document.getElementById("txtDatePaid").value) == false)
	    {           
		    this.error_input += "Invalid date Date Paid.\n";            
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
        { document.getElementById(div_id).style.display = 'block'; }
        else
        { document.getElementById(div_id).style.display = 'none'; }
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
}

function approval_window()
{
    this.outer_modal_div  = null;
    this.inner_modal_div  = null;
    this.commons          = null;     
    this.approver         = null;
    var html_string;   
     
    this.open_window_approval = function()
    {
        var ibol = confirm("Are you sure you want to activate this?");
        if (ibol)
        {        
            this.open_modal_window();
            this.constract_window();
        }
    }    
    
    this.constract_window = function()
    {
        html_string = "<table id ='tbl_approval' width='100%' class='table_border'>";
        html_string += "<tr><td colspan='3' class='table_caption' style='text-align: center; width = 100%'>Enter Approver UID &amp; PWD</td></tr>";
        html_string += "<tr>                                                                    ";
        html_string += "        <td colspan='2' class='cell_caption' style='width: 50%'>        ";
        html_string += "            User ID :</td>                                              ";
        html_string += "        <td style='width: 50%' class='cell_desc'>                       ";
        html_string += "            <input id='txtuid' type='text'/></td>                       ";
        html_string += "</tr>                                                                   ";
        html_string += "<tr>                                                                    ";
        html_string += "        <td colspan='2' class='cell_caption' style='width: 50%'>        ";
        html_string += "            Password :</td>                                             ";
        html_string += "        <td style='width: 50%' class='cell_desc'>                       ";
        html_string += "            <input id='txtpwd' type='password'/></td>                   ";
        html_string += "</tr>                                                                   ";
        html_string += "<tr>                                                                    ";
        html_string += "        <td colspan='2' class='cell_caption' style='width: 50%'>        ";
        html_string += "            <input id='btn_approve' type='button' value='Approve' style='width: 50%' /></td>    ";  
        html_string += "<td style='width: 50%'>                                                                     ";
        html_string += "<input id='btn_cancel' type='button' value='Cancel' style='width: 50%' /></td>                 ";
        html_string += "</tr>";
        html_string += "</table>";

        document.getElementById("content_mod_div").innerHTML = html_string;
        
        var control_finder  = document.getElementById("content_mod_div");
        var btn_approve     = control_finder.document.getElementById("btn_approve");
        var btn_cancel      = control_finder.document.getElementById("btn_cancel");
        var i_cls           = this;        
        control_finder.document.getElementById('txtuid').focus();      
        cls_APAS.set_dateprocessed();
            
        btn_approve.onclick = function()
        {
            var txtuid          = control_finder.document.getElementById("txtuid").value;
            var txtpwd          = control_finder.document.getElementById("txtpwd").value;
            this.approver       = txtuid;
            cls_APAS.verify_password(txtuid,txtpwd);            
        }
        btn_cancel.onclick = function()
        { approval_window.close_modal_window(); }
    }
    this.open_modal_window = function()
	{
        this.load_modal_css();
		this.outer_modal_div           = document.createElement("div");
		this.outer_modal_div.id        = "outer_mod_div";
		this.outer_modal_div.className = "outer_div";		
		
		document.body.style.margin = "0px";
		document.body.appendChild(this.outer_modal_div);
		document.getElementById("outer_mod_div").style.height = screen.height;		
		
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
		if(this.content_iframe  != null){this.content_iframe = null;}

		document.body.style.margin = "";		
	}

    this.construct_div_content = function()
	{
	    var close_div   = null;	    
	    var content_div = null;

		this.inner_modal_div              = document.createElement("div");		
		this.inner_modal_div.align        = "center";
		this.inner_modal_div.className    = "inner_div";		
		this.inner_modal_div.id           = "inner_mod_div";
		this.inner_modal_div.style.height = "auto";		
		this.inner_modal_div.style.width  = parseInt(parent.screen.availWidth)  - 650;
		this.inner_modal_div.style.top    = 20;
		this.inner_modal_div.style.left   = (parseInt(screen.width) - parseInt(this.inner_modal_div.style.width)) / 2 - 10;
		this.inner_modal_div.style.border = "medium solid white";		

		content_div                  = document.createElement("div");		
		content_div.id               = "content_mod_div";
		content_div.align            = "left";
		content_div.style.background = "transparent";
		content_div.style.width      = "95%";
		content_div.innerHTML        = "<center><img src = '../images/ajaxloader.gif'></img></center>";		
		
		this.inner_modal_div.appendChild(content_div);		
		document.body.appendChild(this.inner_modal_div);
	}
	
	this.load_modal_css = function()
	{	    
		var file_reference = document.createElement("link");
		
	  	file_reference.setAttribute("rel", "stylesheet")
	  	file_reference.setAttribute("type", "text/css")
	  	file_reference.setAttribute("href", "../../css/cssmodals.css")

		if (typeof file_reference !="undefined")
		{ document.getElementsByTagName("head")[0].appendChild(file_reference) }
	}

	window.onscroll = function () 
	{		
	    var i = 0;
	    var m = 0;
    	
    	var divelement_outer = document.documentElement.getElementsByTagName("div");
    	var divelement_inner = document.documentElement.getElementsByTagName("div");
    	    	    	
	    for(i = 0; i < divelement_outer.length;i++)
	    {
	        if (divelement_outer.item(i).id == "outer_mod_div")
	        { divelement_outer.item(i).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop;break; }	        	    }
	    for(m = 0; m < divelement_inner.length;m++)
	    {
	        if (divelement_inner.item(m).id == "inner_mod_div")
	        { divelement_outer.item(m).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop + 20 : document.body.scrollTop + 20;break; }	        
	    }	    
	}    
}
