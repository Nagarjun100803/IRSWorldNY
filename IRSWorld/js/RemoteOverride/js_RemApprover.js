
var ra = new rem_Approver();
var rapopups = new RA_popups();
var dtforApproval;
function rem_Approver()
{    
    var ajax_grid;
    
    this.toggle_viewer = function(showval)
    {        
        if (showval == "block")
        {
            document.getElementById("btnApprove").disabled = false;   
            document.getElementById("btnDisapprove").disabled = false;           
            document.getElementById("btnCancel").disabled = false;
        }
        else
        {
            document.getElementById("btnApprove").disabled = true;   
            document.getElementById("btnDisapprove").disabled = true;           
            document.getElementById("btnCancel").disabled = true;
            this.clear_contents();
        }
    }
    this.get_role = function()
    {
        //return document.getElementById("hRole").value;
    }
    this.set_status = function(status)
    {
        document.getElementById("hStatus").value = status;
    }
    this.get_status = function()
    {
        return document.getElementById("hStatus").value;
    }   
    this.center_popup = function(objid) 
    {
    }
    this.display_terminal = function(dt)
    {
        try
        {
            ra.toggle_viewer("block");
            document.getElementById("hseqid").value = dt.Rows[0].SEQ_ID        
            var otc_values  = unescape(dt.Rows[0].DETAILS)
            var arr_otc_val = otc_values.split("^");
            
            this.display_otc1_details(arr_otc_val[0]);
            this.display_otc2_details(arr_otc_val[1]);
            this.display_otc3_details(arr_otc_val[2]);
        }
        catch(e)
        {
            alert("Cannot Display Contents : " );
        }
    }
    
    this.display_otc1_details = function(svalues)
    {
        var scontents = svalues.split("~");
        setText("ddl_compute_mode", scontents[0]) ;
        setText("txt_remit_amount", scontents[1]) ; 
        setText("txt_messages", scontents[2]) ;
        setText("lbl_payment_type", scontents[3]) ;
        setText("lbl_remitting_currency", scontents[4]) ;
        setText("lbl_charge_type", scontents[5]) ;
        setText("lbl_transaction_charge", scontents[6]) ;
        setText("lbl_amount_to_be_paid", scontents[7]) ;
        setText("lbl_beneficiary_amount", scontents[8]) ;
        setText("lbl_peso_equivalent", scontents[9]) ; 
        setText("lbl_transaction_date", scontents[10]) ;
        setText("lbl_transaction_status", scontents[11]) ;
        setText("lbl_branch_name", scontents[12]) ; 
        setText("lbl_account_currency", scontents[13]) ;
        setText("lbl_account_number", scontents[14]) ;
        setText("lbl_product_type", scontents[15]) ; 
        setText("lbl_implementation_type", scontents[16]) ;
        setText("lbl_receipt_mode", scontents[17]) ;
        setText("lbl_purpose", scontents[18]) ;
        setText("lbl_transaction_source", scontents[19]) ;
        setText("lbl_source_of_fund", scontents[20]) ; 
    }
    this.display_otc2_details = function(svalues)
    {
        var scontents = svalues.split("~");
        setText("lbl_remitter_id", scontents[0]) ;
        setText("lbl_remitter_name", scontents[1]) ;
        setText("lbl_remitter_address", scontents[2]) ; 
    }
    this.display_otc3_details = function(svalues)
    {
        var scontents = svalues.split("~");
        setText("lbl_beneficiary_id", scontents[0]) ;
        setText("lbl_beneficiary_name", scontents[1]) ;
        setText("lbl_beneficiary_address", scontents[2]) ; 
    }
    this.check_items_forApproval = function()
    {        
        window.setTimeout("ra.get_items_forApproval()",5000);                
    }
    this.clear_contents = function()
    {
        setText("ddl_compute_mode", "") ;
        setText("txt_remit_amount", "") ; 
        setText("txt_messages", "") ;
        setText("lbl_payment_type", "") ;
        setText("lbl_remitting_currency", "") ;
        setText("lbl_charge_type", "") ;
        setText("lbl_transaction_charge", "") ;
        setText("lbl_amount_to_be_paid", "") ;
        setText("lbl_beneficiary_amount", "") ;
        setText("lbl_peso_equivalent", "") ; 
        setText("lbl_transaction_date", "") ;
        setText("lbl_transaction_status", "") ;
        setText("lbl_branch_name", "") ; 
        setText("lbl_account_currency", "") ;
        setText("lbl_account_number", "") ;
        setText("lbl_product_type", "") ; 
        setText("lbl_implementation_type", "") ;
        setText("lbl_receipt_mode", "") ;
        setText("lbl_purpose", "") ;
        setText("lbl_transaction_source", "") ;
        setText("lbl_source_of_fund", "") ; 
        setText("lbl_remitter_id", "") ;
        setText("lbl_remitter_name", "") ;
        setText("lbl_remitter_address", "") ; 
        setText("lbl_beneficiary_id", "") ;
        setText("lbl_beneficiary_name", "") ;
        setText("lbl_beneficiary_address", "") ; 
    }
    
    this.get_items_forApproval = function()
    {
        try
        {
            hidePanel("divPassword");
        }
        catch(e) { }
        if (ra.get_status() == "not busy")
        {   
            this.display_listings();
            return;
        }
        else
        {
            ra.get_items_forApproval();
        }
    }
    this.get_items_forApproval_callback  = function(results)
    {
        ra.close_msg_prompt();
        try
        {
            if (results.value.Rows.length > 0)
            {
                var dt = results.value;   
                ra.set_status("busy");
                ra.display_terminal(dt);                                   
            }
            else            
            {
                alert("No Items for Approval");
                ra.set_status("not busy");
                window.setTimeout("ra.get_items_forApproval()",5000);
            }
        }
        catch(e)
        {
            alert("Error while retrieving Data");
            ra.set_status("busy");
            return;
        }
    }

    this.approve_click  = function(opt)
    {       
        if (opt != 4)
        {            
            ra.validate_password(opt, ra.get_password());            
        }
        else
        {
            var seq_id = document.getElementById("hseqid").value;
            try
            {
            hidePanel("divPassword");
            }
            catch(e){}
            ra.open_msg_prompt("APPROVING STATUS");
            AjaxPro.onTimeout = function()
            {                    
                alert("SERVER NOT RESPONDING. APPROVING OF STATUS FAILED.");             
            } 
            IRSWorld.RemoteOverride.Approver.cancel_items_forApproval(seq_id.toString(),ra.approve_click_callback());
        }
    }
    this.get_password = function()
    {
        return document.getElementById("txtpassword").value;
    }
    this.approve_click_callback = function(result)
    {        
        ra.close_msg_prompt();
        ra.toggle_viewer("none");
        ra.set_status("not busy");       
        window.setTimeout("ra.get_items_forApproval()",5000);
    }
    this.validate_password  = function(stat, password)
    {
        ra.open_msg_prompt("VALIDATING STATUS");
        AjaxPro.onTimeout = function()
        {                    
            alert("SERVER NOT RESPONDING. VALIDATING FAILED.");             
        } 
        IRSWorld.RemoteOverride.Approver.validate_password(password,ra.validate_password_callback)
    }
    this.validate_password_callback = function(result)
    {    
        var stat = document.getElementById("hAppstat").value;            
        try        
        {
            ra.close_msg_prompt();
            if (parseInt(result.value) == 1 )
            {
                var seq_id = document.getElementById("hseqid").value;
                var password = document.getElementById("txtpassword").value;
                try 
                {
                    hidePanel("divPassword");
                }
                catch(e) {}
                
                if (stat == 2)
                {
                    ra.open_msg_prompt("APPROVING");
                }
                else if (stat == 3)
                {
                    ra.open_msg_prompt("DISAPPROVING");
                }
                
                AjaxPro.onTimeout = function()
                {                    
                    alert("SERVER NOT RESPONDING. APPROVING FAILED.");             
                } 
                IRSWorld.RemoteOverride.Approver.approve_items(password.toString(), seq_id.toString(), stat, ra.approve_click_callback);
            }
            else
            {
                alert("Invalid Password");
            }
     
        }
        catch(e)
        {
                alert("ERROR:" + e.Message);
        }
    }
    this.display_listings = function()
    {     
//        ra.open_msg_prompt("CHECKING ITEMS");        
//        AjaxPro.onTimeout = function()
//        {                    
//            alert("SERVER NOT RESPONDING. CHECKING ITEMS FAILED.");             
//        } 
//        IRSWorld.RemoteOverride.Approver.check_items_forApproval(ra.check_items_forApproval_callback); 
        ra.open_msg_prompt("GETTING ITEMS");
        
        AjaxPro.onTimeout = function()
        {                    
            alert("SERVER NOT RESPONDING. GETTING ITEMS FAILED.");             
        } 
        IRSWorld.RemoteOverride.Approver.get_branch_and_tranxdate(ra.display_listings_callback)               
    }
    this.check_items_forApproval_callback = function(result)
    {
        try
        {
            ra.close_msg_prompt();
            var hasRecord = result.value;       
            if (hasRecord)
            {
                ra.open_msg_prompt("GETTING ITEMS");
                
                AjaxPro.onTimeout = function()
                {                    
                    alert("SERVER NOT RESPONDING. GETTING ITEMS FAILED.");             
                } 
                IRSWorld.RemoteOverride.Approver.get_branch(ra.display_listings_callback)
            }
            else
            {
                alert("NO Item(s) for Approval."); 
                this.check_items_forApproval()
            } 
        }
        catch(e)
        {
            spawn_errpage("SESSION EXPIRED");
        }
    }
    this.display_listings_callback = function(result)
    {
         try
         {
             var sres = result.value ;
             
             var arrsres = sres.split(";");
             
             ra.close_msg_prompt();
             ra.set_status("busy");
             ra.ajax_grid = new grid_selector();
             ra.grid_ajax_query = "proc_select_items_override~" + arrsres[0] + "~" + arrsres[1]  + "~";                                
             ra.ajax_grid.open_selection_grid(null, ra.grid_ajax_query, 0, null);             
         }
         catch(e)
         {
            spawn_errpage('ERRROR: CANNOT DISPLAY LISTINGS SESSION EXPIRED');
         }
    }
    
    
    this.display_password_prompt = function(appstat)
    {        
        showPanel("divPassword");
        document.getElementById("txtpassword").focus();
        document.getElementById("hAppstat").value = appstat;
    }  
    
    this.get_approve_stat = function()
    {
        ra.approve_click(document.getElementById("hAppstat").value);
    }
     
    this.remove_password_prompt = function()
    {        
        hidePanel("divPassword");
    }
    this.handle_keypress = function(key_code)
    {
        var e      = window.event || key_code;
        var keynum = e.charCode   || e.keyCode;
        if (keynum == 13)
        {
            ra.approve_click(2);
        }
    }
    this.do_ajax = function(parameter)
    {   
       if (parameter != null && parameter != "undefined")
       {
            var command_param = parameter.split("$");
            if (command_param.length > 0)
            {
                if (command_param[0] == "ajx_selected")
                {                   
                   ra.lock_select_record(command_param[1]);
                   document.getElementById("hseqid").value = command_param[1];
                }
            }
            else
            {
                ra.set_status("not busy");
            }
        }                
       ra.ajax_grid.process_ajax(parameter);
    }
    
    this.lock_select_record = function(seqid)
    {         
        ra.open_msg_prompt("GETTING ITEMS FOR APPROVAL");        
        AjaxPro.onTimeout = function()
        {                    
            alert("SERVER NOT RESPONDING. GETTING ITEMS FOR APPROVAL FAILED.");             
        } 
        IRSWorld.RemoteOverride.Approver.get_items_forApproval(seqid,ra.get_items_forApproval_callback)
    }
   
    
    this.otc_override_info = function()
    {
        this.global_values = "";        
        this.set_otc_values = function(arr_values)
        {
            this.global_values = arr_values.split("~");
            var arr1 = global_values[0].split(";")
            var arr2 = global_values[1].split(";")
            try
            {
                var dt1 = new DataTable1();                
                var dr = new DataTable1_Rows();
                dr.remitting_to_account = arr1[0].split("=")[1];
                dr.account_to_remitting_currency = arr1[1].split("=")[1];
                dr.account_to_remitting = arr1[2].split("=")[1];
                dr.remitting_to_account_currency = arr1[3].split("=")[1];                
                dt1.addRows(dr);
            }
            catch(e)
            {
                alert("CANNOT GET TABLE ONE VALUES");
            }
            try
            {
                var dt1 = new DataTable1();                
                for(i = 0; i < arr2.length; i++)
                {
                    var dt1 = new DataTable2();
                    var dr = new DataTable2_Rows();                    
                    dr.ChargeNameDesc = arr2[0].split("=")[1];
                    dr.CurrencyShortCode = arr1[1].split("=")[1];                                        
                    dt1.addRows(dr);
                }
            }
            catch(e)
            {
                alert("CANNOT GET TABLE TWO VALUES");
            }
            
        }
        /* START OF DATATABLE1*/
        this.DataTable1 = function()
        {
            this.Rows = new Array();
        }
        this.DataTable1.prototype.addRows = function(row)
        {
            this.Rows.push(row);
        }        
        this.DataTable1_Rows = function() 
        {
            this.remitting_to_account = "";
            this.account_to_remitting_currency ="";
            this.account_to_remitting = "";
            this.remitting_to_account_currency ="";
        }
        /* END OF DATATABLE1*/
        /* START OF DATATABLE2*/
        this.DataTable2 = function()
        {
            this.Rows = new Array();
        }
        this.DataTable2.prototype.addRows = function(row)
        {
            this.Rows.push(row);
        }        
        this.DataTable2_Rows = function()
        {
            this.ChargeNameDesc = "";
            this.CurrencyShortCode ="";            
        }
        /* END OF DATATABLE1*/
        this.display_override_info = function()
        {
            
        }
        
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


function RA_popups()
{
    var cls = this;   
    this.test_nsbrow = function()
    {
        return (document.all) ? false : true;
    }     
    this.construct_ra_objects = function()
    {    
       ra.toggle_viewer("none");  
       ra.check_items_forApproval();  
    }
    this.init_popup = function()
    {        
        var div = document.createElement('div');		        
        var NS4 = this.test_nsbrow();
        
        div.innerHTML = rapopups.romsg_popup_html();
        div.setAttribute('id', 'divMonitor');
        div.setAttribute('style','z-index: 101;')	    
        div.id = 'divMonitor';		
        div.style.display = "none";
        div.style.position = "absolute";
        div.style.border = "solid 1px midnightblue";
        
        if (NS4) {div.zIndex = 101;}
        else {div.style.zIndex = 101;}
        
        if (document.body.firstChild)
        {      	
            document.body.insertBefore(div, document.body.firstChild);		    
        } 
        else 
        {      	
            document.body.appendChild(div);		    
        }
        ra.center_popup('divMonitor');       
    }  
    this.hide_obj = function(opt,objid)             
    {
        if (opt == 1 ) {hidePanel(objid)}
        else {showPanel(objid)}
    }
    this.romsg_popup_html = function()
    {                
        shtml = '<table style="height: auto;" class="table_caption" cellspacing="0" cellpadding="0">';
        shtml += '<tr>';
        shtml += '<td style="height:auto;" class="cell_caption">';
        shtml += '<input type="hidden" id="hseqid" value=""/>';
        shtml += '<div id="divforApproval">';
        shtml += '</div>';
        shtml += '</td>';              
        shtml += '</tr>';
        shtml += '<tr>';
        shtml += '<td class="cell_caption"  style="text-align:center; height: 33px;" nowrap>';
        shtml += '<input id="btnApprove" type="button" value="Approve" onclick="javascript:ra.display_password_prompt();" style="width: 24%"/>&nbsp;';   
        shtml += '<input id="btnDisapprove" type="button" value="Disapprove" onclick="javascript:display_password_prompt();" style="width: 24%"/>';                                                     
        shtml += '<input id="btnCancel" type="button" value="Cancel" onclick="javascript:ra.approve_click(4);" style="width: 24%"/>';                        
        shtml += '</td>';
        shtml += '</tr>';
        shtml += '<tr>';
        shtml += '<td nowrap class="cell_caption">';
        shtml += '<input type="hidden" id="txtApproverID" size ="10" maxlength="10" value=""/>';
        shtml += '<input type="hidden" id="hStatus" value ="not busy"  />';
        shtml += '<input type="hidden" id="hRole" value ="Admin"  />';
        shtml += '</td>';
        shtml += '</tr>';                
        shtml += '</table>';        
        return shtml;
    }
    
    this.construct_override_window = function(arrdata1,arrdata2)
    {
        var irs_calculator_data = this.irs_calculator;
        var html_string         = "";        

        if(irs_calculator_data.Tables[0].Rows.length > 0)
        {
            this.remitting_to_account          = arrdata1.remitting_to_account;
            this.remitting_to_account_currency = arrdata1.account_to_remitting_currency;
            this.account_to_remitting          = arrdata1.account_to_remitting;
            this.account_to_remitting_currency = arrdata1.remitting_to_account_currency;
        }

        var remitting_to_account = "REMITTING TO ACCOUNT" + "</br>" + this.remitting_to_account_currency + " TO " + this.account_to_remitting_currency;
        var account_to_remitting = "ACCOUNT TO REMITTING" + "</br>" + this.account_to_remitting_currency + " TO " + this.remitting_to_account_currency;

        html_string += "<table style='border: solid 1px #336699;' cellspacing='1' cellpadding='0' width='100%'>";
        html_string += "    <tr  class = 'cell_caption'>";
        html_string += "        <td align = 'center'>" + remitting_to_account + "</td>";
        html_string += "        <td align = 'center'>" + account_to_remitting + "</td>";
        html_string += "    </tr>";
        html_string += "    <tr class = 'cell_desc' align = 'center'>";
        html_string += "        <td><input id = 'txt_currency_rate_remitting_to_account' type='text' style = 'text-align:center; width:auto' value = '" + this.remitting_to_account.toFixed(9) + "' /></td>";
        html_string += "        <td><input id = 'txt_currency_rate_account_to_remitting' type='text' style = 'text-align:center; width:auto' value = '" + this.account_to_remitting.toFixed(9) + "' /></td>";        
        html_string += "    </tr>";
        html_string += "</table></br>";
        
        html_string += "<table id = 'tbl_override_charges_setup' style='border: solid 1px #336699;' cellspacing='1' cellpadding='0' width='100%'>";
        html_string += "    <tr  class = 'cell_caption'>";
        html_string += "        <td align = 'center' style = 'width: 30%'>CHARGE NAME</td>";
        html_string += "        <td align = 'center' style = 'width: 30%'>CURRENCY</td>";
        html_string += "        <td align = 'center' style = 'width: 30%'>AMOUNT</td>";
        html_string += "    </tr>"       
        
        if (irs_calculator_data != null)
        {        
            for(var i = 0; i < arrdata2.Rows.length; i++)
            {
                var overrideflag           = arrdata2.Rows[i].OverrideFlag;
                var txt_charge_name_hidden = "<input type = 'hidden' style = 'text-align:right; width:auto' id = 'hidden_charges_id_" + (i + 1) + "' value = '" + irs_calculator_data.Tables[1].Rows[i].ChargeName + "'/>";
                
                if (overrideflag == true) 
                {
                    var txt_charge_name_txtbox = "<input type = 'text'   style = 'text-align:right; width:auto' id = 'txt_charges_value_" + (i + 1) + "' value = '" + ( 1 * irs_calculator_data.Tables[1].Rows[i].OverrideAmount).toFixed(2) + "'/>";
                }
                else
                {
                    var txt_charge_name_txtbox = "<input type = 'text'   style = 'text-align:right; width:auto' id = 'txt_charges_value_" + (i + 1) + "' value = '" + ( 1 * irs_calculator_data.Tables[1].Rows[i].Amount).toFixed(2) + "'/>";
                }

                html_string += "    <tr  class = 'cell_desc'>";
                html_string += "        <td align = 'left'>"   + arrdata2.Rows[i].ChargeNameDesc.toUpperCase()    + "</td>";
                html_string += "        <td align = 'center'>" + arrdata2.Rows[i].CurrencyShortCode + "</td>";
                html_string += "        <td align = 'right'>"  + txt_charge_name_hidden + txt_charge_name_txtbox + "</td>";
                html_string += "    </tr>"
            }
        }        

        html_string += "</table></br>";

        html_string += "<table style='border: solid 1px #336699;' cellspacing='1' cellpadding='0' width='100%'>";
        html_string += "    <tr>";
        html_string += "        <td class = 'cell_caption' style = 'width: 30%'>LOGIN NAME:</td>";
        html_string += "        <td class = 'cell_desc'><input type='text' id = 'txt_username' style = 'width: auto'/></td>";
        html_string += "    </tr>";
        html_string += "    <tr>";
        html_string += "        <td class = 'cell_caption' style = 'width: 30%'>PASSWORD:</td>";
        html_string += "        <td class = 'cell_desc'><input type='password' id = 'txt_password' style = 'width: auto'/></td>";
        html_string += "    </tr>";
        html_string += "</table></br>";

        html_string += "</br><center><input id='btn_override_apply' style='width: 30%; height:15%' type='submit' value='APPLY CHANGES' />&nbsp<input id='btn_override_cancel' style='width: 30%; height:15%' type='submit' value='CANCEL' /></center></br>";

        document.getElementById("content_mod_div").innerHTML = html_string;                                             
    }
        this.open_modal_window = function()
    {
        this.load_modal_css();
    
        this.outer_modal_div           = document.createElement("div");
        this.outer_modal_div.id        = "outer_mod_div_ra";
        this.outer_modal_div.className = "outer_div";
    
        document.body.style.margin = "0px";
        document.body.appendChild(this.outer_modal_div);
        document.getElementById("outer_mod_div_ra").style.height = screen.height;
    
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
            this.inner_modal_div.id           = "inner_mod_div_ra";
            this.inner_modal_div.style.height = "auto";		
            this.inner_modal_div.style.width  = "400px";
            this.inner_modal_div.style.top    = "30px";
            this.inner_modal_div.style.left   = (parseInt(screen.width) - parseInt(this.inner_modal_div.style.width)) / 2 - 10 + "px";
            this.inner_modal_div.style.border = "medium solid white";

            content_div                       = document.createElement("div");
            content_div.id                    = "content_mod_div_ra";
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
     window.onscroll = function () 
    {            
        var i = 0;
        var m = 0;
    
        var divelement_outer = document.documentElement.getElementsByTagName("div");
        var divelement_inner = document.documentElement.getElementsByTagName("div");
                        
            for(i = 0; i < divelement_outer.length;i++)
            {
                if (divelement_outer.item(i).id == "outer_mod_div_ra")
                {
                    divelement_outer.item(i).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop;break;
                }	        
            }
            for(m = 0; m < divelement_inner.length;m++)
            {
                if (divelement_inner.item(m).id == "inner_mod_div_ra")
                {
                    divelement_outer.item(m).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop + 30 : document.body.scrollTop + 30;break;
                }
            }
    }


}

/** hide panel*/
function showPanel(divname)
{   
    var div = document.getElementById(divname);
    
    divmask = new maskpanel();
    divmask.show();
    
    document.getElementById(divname).style.display = "block";
    
    div.style.top = (((document.documentElement.clientHeight / 2) + document.documentElement.scrollTop) - (div.offsetHeight / 2)) + 'px';
    div.style.left = (((document.documentElement.clientWidth / 2) + document.documentElement.scrollLeft) - (div.offsetWidth / 2)) + 'px';
}
function hidePanel(divname)
{
    document.getElementById(divname).style.display = "none";
    divmask.hide();
}
/** end of hide panel ***/

function execute_ajx_command(param)
{
    if (ra != null && ra != "undefined")
    
    {
        ra.do_ajax(param);
    }
}
