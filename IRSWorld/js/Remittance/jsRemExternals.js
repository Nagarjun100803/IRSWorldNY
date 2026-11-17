/* 1.) WAIVED CHARGES */
function otc_waive_charges()
{
    var icls = this;

    this.outer_modal_div    = null;
    this.inner_modal_div    = null;
    this.commons            = null;
    this.msg_prompt_control = null;
    this.remote_override    = null;

    this.product_charge_id       = 0;
    this.account_currency        = 0;
    this.amount_rem              = 0;
    this.net_to_bene_currency    = 0;
    this.local_currency          = 0;
    this.gross_rem_amount        = 0;
    this.net_rem_amount          = 0;
    this.net_to_bene_amount      = 0;
    this.total_local_charge      = 0;
    this.forex_target_to_account = 0;
    this.forex_target_to_source  = 0;
    this.forex_target_to_usd     = 0;
    this.forex_target_to_php     = 0;
    this.forex_target_to_eur     = 0;
    this.forex_source_to_target  = 0;
    this.forex_source_to_usd     = 0;
    this.forex_source_to_php     = 0;
    this.beneficiary_amount      = 0;
    this.transaction_charge      = 0;
    this.peso_equivalent         = 0;
    this.total_backend_charges   = 0;
    this.calculator_session_name = '';
    
    this.txt_otc_rem_waived_user_id        = null;
    this.txt_otc_rem_waived_user_password  = null;
    
    this.pending_override_status = 0;
    this.override_type_code = "1";
    this.refinitiv_transaction_override_status = 0; /* RFC-37590 - IRSW NY Interface with Refinitiv WorldCheck via API */
    
    this.run_rmover_closer = function()
    {
        this.compute_waived();
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
    
    this.open_waive_login = function()
    {
        this.open_modal_window();
        this.construct_waive_auth_screen();
    }
    
    this.close_waive_login = function()
    {
        this.close_modal_window();
    }

    this.open_waiver_screen = function(product_charge_id, account_currency, amount_rem, calculator_session_name)
    {
        this.product_charge_id       = product_charge_id;
        this.account_currency        = account_currency;
        this.amount_rem              = amount_rem;
        this.calculator_session_name = calculator_session_name;        
        this.open_modal_window();
        this.construct_waive_auth_screen();
        
        icls = this;
    }

    this.construct_waive_auth_screen = function()
    {
        var html_string = "";
        
        html_string = "<table id = 'tbl_otc_waive_sunako' style='border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; width: 100%; border-bottom: #336699 1px solid' cellpadding='0' cellspacing='1'>";
        html_string += "    <tr><td colspan='2' style='text-align: center' class='table_caption'>WAIVE CHARGES - ENTER USERNAME AND PASSWORD</td></tr>";
        html_string += "    <tr style='height: 19px' >";
        html_string += "        <td class='cell_caption' style='width: 30%; text-align: right'>USER NAME :</td>";
        html_string += "        <td class='cell_desc' style='width: 70%; text-align: left'><input type = 'text' id = 'txt_otc_rem_waived_user_id' /></td>";
        html_string +=      "</tr>";
        html_string += "    <tr style='height: 19px'>";
        html_string += "        <td style='width: 30%; text-align: right;' class='cell_caption'>PASSWORD :</td>";
        html_string += "        <td style='width:70%; text-align: left;' class='cell_desc'><input type = 'password' id = 'txt_otc_rem_waived_user_password' /></td>";
        html_string += "    </tr>";
        html_string += "    <tr style='height: 19px'>";
        html_string += "        <td class='cell_desc' style='text-align: center' colspan='2'>";
        html_string += "            <input id='btn_otc_rem_enter_ok' type='button' value='OK' style='width: 18%; height: 24px' />";
        html_string += "            <input id='btn_otc_rem_enter_cancel' type='button' value='CANCEL' style='width: 18%; height: 24px'/>";
        html_string += "            <input id='btn_otc_rem_remote_override' type='button' value='REMOTE OVERRIDE' style='width: 35%; height: 24px'/>";
        html_string += "        </td>";
        html_string += "    </tr>";
        html_string += "</table>";

        document.getElementById("content_mod_div").innerHTML = html_string;

        document.getElementById("btn_otc_rem_enter_ok").onclick = function()
        {
            icls.check_user_rights();
        }

        document.getElementById("btn_otc_rem_enter_cancel").onclick = function()
        {
            icls.close_modal_window();
        }
        
        document.getElementById("btn_otc_rem_remote_override").onclick = function()
        {
            /*
            icls.remote_override = new rem_Override();
            
            if (icls.remote_override != null)
            {            
                icls.remote_override.prepare_override(0,1);
            }
            */
            
            var rem_ovr = new rem_ovr_scr();

            rem_ovr.jscls = icls;
            rem_ovr.chk_branch_is_allowed();
                
        }
    }

    this.check_user_rights = function()
    {
        this.txt_otc_rem_waived_user_id       = document.getElementById("txt_otc_rem_waived_user_id").value;
        this.txt_otc_rem_waived_user_password = document.getElementById("txt_otc_rem_waived_user_password").value;
    
        AjaxPro.timeoutPeriod = 1000 * 1000;
        
        AjaxPro.onTimeout = function()
        {
            this.close_msg_prompt(); alert("Server not responding. Checking user rights to waived charges failed.");
        }

        this.close_waive_login();
        this.open_msg_prompt("Checking access rights. Please wait ...");

        IRSWorld.TranClass.RemTranExtra.user_can_waived_charges(this.txt_otc_rem_waived_user_id, this.txt_otc_rem_waived_user_password, this.check_user_rights_callback);
    }

    this.check_user_rights_callback = function(result)
    {
        icls.close_msg_prompt();
    
        if (result != null && result.value != null)
        {
            if (result.value[0].toString().toUpperCase() == "FALSE")
            {
                alert(result.value[1]); 

                icls.open_waive_login(); 
                
                document.getElementById("txt_otc_rem_waived_user_id").value       = icls.txt_otc_rem_waived_user_id;
                document.getElementById("txt_otc_rem_waived_user_password").value = icls.txt_otc_rem_waived_user_password;

                return;
            }

            icls.compute_waived();
        }
    }

    this.compute_waived = function()
    {
        AjaxPro.timeoutPeriod = 1000 * 1000;
        
        AjaxPro.onTimeout = function()
        {
            alert("Server not responding. Computed waived charges failed.");
        }    
    
        this.open_msg_prompt("Please wait ..."); IRSWorld.TranClass.RemTranExtra.compute_waived(parseInt(icls.product_charge_id), parseInt(icls.account_currency), parseFloat(icls.amount_rem), icls.calculator_session_name ,this.compute_waived_callback);
    }

    this.compute_waived_callback = function(result)
    {
        icls.close_msg_prompt();
        
        var calc = result.value;

        if (calc != null)
        {
            var array_calc = calc.split("~");

            icls.beneficiary_amount      = array_calc[0];
            icls.transaction_charge      = array_calc[1];
            icls.peso_equivalent         = array_calc[2];
            icls.net_to_bene_currency    = array_calc[3];
            icls.local_currency          = array_calc[4];
            icls.gross_rem_amount        = array_calc[5];
            icls.net_rem_amount          = array_calc[6];
            icls.net_to_bene_amount      = array_calc[7];
            icls.total_local_charge      = array_calc[8];
            icls.forex_target_to_account = array_calc[9];
            icls.forex_target_to_source  = array_calc[10];
            icls.forex_target_to_usd     = array_calc[11];
            icls.forex_target_to_php     = array_calc[12];
            icls.forex_target_to_eur     = array_calc[13];
            icls.forex_source_to_target  = array_calc[14];
            icls.forex_source_to_usd     = array_calc[15];
            icls.forex_source_to_php     = array_calc[16];
            icls.total_backend_charges   = array_calc[17];
            icls.close_modal_window();

            eval("javascript:push_waive_charges_result(icls);");
        }
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
	
	    document.body.style.margin = "";
    }

    this.construct_div_content = function()
    {
	    var content_div = null;
    	
	    content_div                       = document.createElement("div");
	    content_div.id                    = "content_mod_div";
	    content_div.align                 = "left";
	    content_div.style.background      = "transparent";
	    content_div.innerHTML             = "<center><img src = '.././images/ajaxloader.gif'></img></center>";

	    this.inner_modal_div              = document.createElement("div");		
	    this.inner_modal_div.align        = "center";
	    this.inner_modal_div.className    = "inner_div";		
	    this.inner_modal_div.id           = "inner_mod_div";
	    this.inner_modal_div.style.height = "auto";		
	    this.inner_modal_div.style.width  = "400px";
	    this.inner_modal_div.style.top    = "30px";
	    this.inner_modal_div.style.left   = (parseInt(screen.width) - parseInt(this.inner_modal_div.style.width)) / 2 - 10 + "px";
	    this.inner_modal_div.style.border = "medium solid white";
	    this.inner_modal_div.appendChild(content_div);
	
	    document.body.appendChild(this.inner_modal_div);
    }

    this.load_modal_css = function()
    {
	    var file_reference = document.createElement("link");
    	
	    file_reference.setAttribute("rel", "stylesheet");
	    file_reference.setAttribute("type", "text/css");
	    file_reference.setAttribute("href", ".././css/cssmodals.css");
    	
	    if (typeof file_reference != "undefined")
	    {		    
	        document.getElementsByTagName("head")[0].appendChild(file_reference);
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
	        if (divelement_outer.item(i).id == "outer_mod_div")
	        {
		    divelement_outer.item(i).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop;break;
	        }
	    }
	    for(m = 0; m < divelement_inner.length;m++)
	    {
	        if (divelement_inner.item(m).id == "inner_mod_div")
	        {
		    divelement_outer.item(m).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop + 20 : document.body.scrollTop + 20;break;
	        }
	    }
    }
}
/*WAIVED CHARGES END OF CODE */


/* 2.) OVERRIDE RATES AND CHARGES */
function override_calculator()
{
    var icls = this;

    this.login_result_flag = null;
    this.login_result_msgs = null;
    this.outer_modal_div   = null;
    this.inner_modal_div   = null;
    this.commons           = null;

    this.irs_calculator              = null;
    this.array_override_data         = null;
    this.irs_calculator_session_name = null;
    this.array_override_joined       = null;
        
    this.charges_rows = 0;
    this.remitting_to_account          = 0;
    this.account_to_remitting          = 0;
    this.remitting_to_account_currency = '';
    this.account_to_remitting_currency = '';
    this.pending_override_status = 0;
    this.override_type_code = "3~4";
    
    this.run_rmover_closer = function()
    {
        this.run_forex_charge_ro();
    }

    this.open_override_form = function(irs_calculator_session_name)
    {
        AjaxPro.timeoutPeriod = 1000 * 1000;
        
        AjaxPro.onTimeout = function()
        {
            alert("Server not responding. Loading calculator for override value failed.");
        }
    
        this.irs_calculator = IRSWorld.TranClass.RemTranExtra.get_calculator_for_override_values(irs_calculator_session_name).value;
        this.irs_calculator_session_name = irs_calculator_session_name;

        if (this.irs_calculator != null)
        {
            this.open_modal_window();
            this.construct_override_window();
        }
    }
    
    this.construct_override_window = function()
    {
        var irs_calculator_data = this.irs_calculator;
        var html_string         = "";        

        if(irs_calculator_data.Tables[0].Rows.length > 0)
        {
            this.remitting_to_account          = irs_calculator_data.Tables[0].Rows[0].remitting_to_account;
            this.remitting_to_account_currency = irs_calculator_data.Tables[0].Rows[0].account_to_remitting_currency;
            this.account_to_remitting          = irs_calculator_data.Tables[0].Rows[0].account_to_remitting;
            this.account_to_remitting_currency = irs_calculator_data.Tables[0].Rows[0].remitting_to_account_currency;
        }

        var remitting_to_account = "REMITTING TO ACCOUNT" + "</br>" + this.remitting_to_account_currency + " TO " + this.account_to_remitting_currency;
        var account_to_remitting = "ACCOUNT TO REMITTING" + "</br>" + this.account_to_remitting_currency + " TO " + this.remitting_to_account_currency;
        
        var txt_remitting_to_account_currency_hidden = "<input type = 'hidden' style = 'text-align:right; width:auto' id = 'txt_remitting_to_account_currency_hidden' value = '" + this.remitting_to_account_currency + "'/>";
        var txt_account_to_remitting_currency_hidden = "<input type = 'hidden' style = 'text-align:right; width:auto' id = 'txt_account_to_remitting_currency_hidden' value = '" + this.account_to_remitting_currency + "'/>";
        

        html_string += "<table style='border: solid 1px #336699;' cellspacing='1' cellpadding='0' width='100%'>";
        html_string += "    <tr  class = 'cell_caption'>";
        html_string += "        <td align = 'center'>" + txt_remitting_to_account_currency_hidden + remitting_to_account + "</td>";
        html_string += "        <td align = 'center'>" + txt_account_to_remitting_currency_hidden + account_to_remitting + "</td>";
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
            this.charges_rows = irs_calculator_data.Tables[1].Rows.length;
        
            for(var i = 0; i < irs_calculator_data.Tables[1].Rows.length; i++)
            {
                var overrideflag             = irs_calculator_data.Tables[1].Rows[i].OverrideFlag;
                var txt_charge_name_hidden   = "<input type = 'hidden' style = 'text-align:right; width:auto' id = 'hidden_charges_id_" + (i + 1) + "' value = '" + irs_calculator_data.Tables[1].Rows[i].ChargeName + "'/>";
                var txt_charge_desc_hidden   = "<input type = 'hidden' style = 'text-align:right; width:auto' id = 'txt_charge_desc_hidden_" + (i + 1) + "' value = '" + irs_calculator_data.Tables[1].Rows[i].ChargeNameDesc.toUpperCase() + "'/>";
                var txt_currency_desc_hidden = "<input type = 'hidden' style = 'text-align:right; width:auto' id = 'txt_currency_desc_hidden_" + (i + 1) + "' value = '" + irs_calculator_data.Tables[1].Rows[i].CurrencyShortCode.toUpperCase() + "'/>";
                
                if (overrideflag == true) 
                {
                    var txt_charge_name_txtbox = "<input type = 'text'   style = 'text-align:right; width:auto' id = 'txt_charges_value_" + (i + 1) + "' value = '" + ( 1 * irs_calculator_data.Tables[1].Rows[i].OverrideAmount).toFixed(2) + "'/>";
                }
                else
                {
                    var txt_charge_name_txtbox = "<input type = 'text'   style = 'text-align:right; width:auto' id = 'txt_charges_value_" + (i + 1) + "' value = '" + ( 1 * irs_calculator_data.Tables[1].Rows[i].Amount).toFixed(2) + "'/>";
                }

                html_string += "    <tr  class = 'cell_desc'>";
                html_string += "        <td align = 'left'>"   + txt_charge_desc_hidden   + irs_calculator_data.Tables[1].Rows[i].ChargeNameDesc.toUpperCase() + "</td>";
                html_string += "        <td align = 'center'>" + txt_currency_desc_hidden + irs_calculator_data.Tables[1].Rows[i].CurrencyShortCode + "</td>";
                html_string += "        <td align = 'right'>"  + txt_charge_name_hidden   + txt_charge_name_txtbox + "</td>";
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

        html_string += "</br><center><input id='btn_override_apply' style='width: 30%; height:24%' type='submit' value='APPLY CHANGES' />&nbsp<input id='btn_override_cancel' style='width: 30%; height:24%' type='submit' value='CANCEL' />&nbsp<input id='btn_request_remote_override' style='width: 30%; height:24%' type='submit' value='REMOTE OVERRIDE' /></center></br>";

        document.getElementById("content_mod_div").innerHTML = html_string;

        document.getElementById("btn_request_remote_override").onclick = function()
        {
            /*
            icls.remote_override = new rem_Override();

            if (icls.remote_override != null)
            {
                icls.remote_override.set_override_details(icls.charges_rows);
                icls.remote_override.prepare_override(0,2);
            } 
            */

            var rem_ovr = new rem_ovr_scr();

            rem_ovr.jscls = icls;
            rem_ovr.chk_branch_is_allowed();
        }
        
        document.getElementById("btn_override_apply").onclick = function()
        {
            AjaxPro.timeoutPeriod = 1000 * 1000;
            
            AjaxPro.onTimeout = function()
            {
                alert("Server not responding. Checking user rights to override rates failed.");
            }
            IRSWorld.TranClass.RemTranExtra.user_can_override_rates(document.getElementById("txt_username").value, document.getElementById("txt_password").value, icls.override_callback);
        }
        
        document.getElementById("btn_override_cancel").onclick = function()
        {
            icls.close_modal_window();
        }
        
        document.getElementById("txt_currency_rate_remitting_to_account").onkeyup = function()
        {
            var rate = document.getElementById("txt_currency_rate_remitting_to_account").value;
            
            if (rate > 0)
            {                
                document.getElementById("txt_currency_rate_account_to_remitting").value = (1 / parseFloat(rate)).toFixed(9);
            }
            else
            {
                document.getElementById("txt_currency_rate_account_to_remitting").value = 0;
            }
        }
        
        document.getElementById("txt_currency_rate_account_to_remitting").onkeyup = function()
        {
            var rate = document.getElementById("txt_currency_rate_account_to_remitting").value;
            
            if (rate > 0)
            {            
                document.getElementById("txt_currency_rate_remitting_to_account").value = (1 / parseFloat(rate)).toFixed(9);
            }
            else
            {
                document.getElementById("txt_currency_rate_remitting_to_account").value = 0;
            }
        }
        
        document.getElementById("txt_currency_rate_remitting_to_account").onblur = function()
        {
            //document.getElementById("txt_currency_rate_remitting_to_account").value = (1 / parseFloat(document.getElementById("txt_currency_rate_account_to_remitting").value)).toFixed(9);
            document.getElementById("txt_currency_rate_account_to_remitting").value = (1 / parseFloat(document.getElementById("txt_currency_rate_remitting_to_account").value)).toFixed(9);
        }
        
        document.getElementById("txt_currency_rate_account_to_remitting").onblur = function()
        {
            document.getElementById("txt_currency_rate_remitting_to_account").value = (1 / parseFloat(document.getElementById("txt_currency_rate_account_to_remitting").value)).toFixed(9);
            //document.getElementById("txt_currency_rate_account_to_remitting").value = (1 / parseFloat(document.getElementById("txt_currency_rate_remitting_to_account").value)).toFixed(9);
        }        

        irs_calculator_data = null;
    }
    
    this.override_execute = function()
    {
        AjaxPro.timeoutPeriod = 1000 * 1000;
        
        AjaxPro.onTimeout = function()
        {
            alert("Server not responding. Checking user rights to override rates failed.");
        }

        IRSWorld.TranClass.RemTranExtra.user_can_override_rates(document.getElementById("txt_username").value, document.getElementById("txt_password").value, icls.override_callback);            
    }
    
    this.override_callback = function(result)
    {
        if (result.value != null && result.value.length > 0)
        {
            icls.login_result_flag = result.value[0];
            icls.login_result_msgs = result.value[1];

            if (result.value[0] == false)
            {
                alert(result.value[1]); return;
            }
            else
            {
                icls.parse_data_changes(); 
                icls.send_parsed_data();
            }
        }
    }
    
    this.send_parsed_data = function()
    {
        AjaxPro.timeoutPeriod = 1000 * 1000;
        
        AjaxPro.onTimeout = function()
        {
            alert("Server not responding. Sending for override values failed.");
        }    
    
        IRSWorld.TranClass.RemTranExtra.set_calculator_override_values(icls.array_override_joined, icls.irs_calculator_session_name, this.send_parsed_data_callback);
    }
    
    this.send_parsed_data_callback = function(result)
    {
        if (result != null)
        {
            eval("javascript:push_override_result(icls);");
        }
    }
    
    this.parse_data_changes = function()
    {
        if (this.irs_calculator != null)
        {
            this.array_override_data = new Array();

            if (this.irs_calculator.Tables[0].Rows.length > 0)
            {
                this.array_override_data[this.array_override_data.length] = "FOREX_RATES" + "~" + "remitting_to_account_rate" + "~" + document.getElementById("txt_currency_rate_remitting_to_account").value
                this.array_override_data[this.array_override_data.length] = "FOREX_RATES" + "~" + "account_to_remitting_rate" + "~" + document.getElementById("txt_currency_rate_account_to_remitting").value
            }
        
            if (this.irs_calculator.Tables[1].Rows.length > 0)
            {
                var contents = document.getElementById("tbl_override_charges_setup");
                
                for(var i = 0; i < contents.rows.length; i++)
                {
                    if (i > 0)
                    {
                        this.array_override_data[this.array_override_data.length] = "CHARGES_DATA" + "~" + document.getElementById("hidden_charges_id_" + i).value + "~" + document.getElementById("txt_charges_value_" + i).value;                        
                    }
                }
            }
            
            this.array_override_joined = this.array_override_data.join("|");
        }
    }
    
    this.run_forex_charge_ro = function()
    {
        this.open_msg_prompt("Please wait ...");
        this.parse_data_changes();
        
        AjaxPro.timeoutPeriod = 1000 * 1000;
        AjaxPro.onTimeout = function()
        {
            alert("Server not responding. Sending for override values failed.");
        }    
    
        IRSWorld.TranClass.RemTranExtra.set_calculator_override_values(icls.array_override_joined, icls.irs_calculator_session_name, this.run_forex_charge_ro_callback);
    }
    
    this.run_forex_charge_ro_callback = function(result)
    {
        icls.close_msg_prompt();

        if (result != null)
        {
            eval("javascript:push_override_result(icls);");
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
	
	    document.body.style.margin = "";
    }

    this.construct_div_content = function()
    {
	    var content_div = null;

	    content_div                       = document.createElement("div");		
	    content_div.id                    = "content_mod_div";
	    content_div.align                 = "left";
	    content_div.style.background      = "transparent";
	    content_div.style.width           = "95%";
	    content_div.innerHTML             = "<center><img src = '.././images/ajaxloader.gif'></img></center>";    	
    	
	    this.inner_modal_div              = document.createElement("div");		
	    this.inner_modal_div.align        = "center";
	    this.inner_modal_div.className    = "inner_div";		
	    this.inner_modal_div.id           = "inner_mod_div";
	    this.inner_modal_div.style.height = "auto";
	    this.inner_modal_div.style.width  = "500px";
	    this.inner_modal_div.style.top    = "30px";
	    this.inner_modal_div.style.left   = (parseInt(screen.width) - parseInt(this.inner_modal_div.style.width)) / 2 - 10 + "px";
	    this.inner_modal_div.style.border = "medium solid white";
	    this.inner_modal_div.appendChild(document.createElement("br"));
	    this.inner_modal_div.appendChild(content_div);
    	
	    document.body.appendChild(this.inner_modal_div);
    }
	
    this.load_modal_css = function()
    {	    
	    var file_reference = document.createElement("link");
    	
	    file_reference.setAttribute("rel", "stylesheet");
	    file_reference.setAttribute("type", "text/css");
	    file_reference.setAttribute("href", ".././css/cssmodals.css");
    
	    if (typeof file_reference != "undefined")
	    {		    
	        document.getElementsByTagName("head")[0].appendChild(file_reference);
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
	        if (divelement_outer.item(i).id == "outer_mod_div")
	        {
		        divelement_outer.item(i).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop;break;
	        }	        
	    }	    
	    for(m = 0; m < divelement_inner.length; m++)
	    {
	        if (divelement_inner.item(m).id == "inner_mod_div")
	        {
		        divelement_outer.item(m).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop + 20 : document.body.scrollTop + 20;break;
	        }
	    }
    }
}
/* END OVERRIDE RATES AND CHARGES */

/* 3.) BREAKDOWN OF CHARGES */
function compute_breakdown()
{
    var compute_breakdown = this;

    this.outer_modal_div  = null;
    this.inner_modal_div  = null;
    this.commons          = null;
    
    this.open_breakdown = function(s)
    {
        this.open_modal_window();
        
        document.getElementById("content_mod_div").innerHTML = s;
        
        document.getElementById("btn_close_breakdown").onclick = function()
        {
            compute_breakdown.close_modal_window();
        }
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

		document.body.style.margin = "";
	}

    this.construct_div_content = function()
	{
	    var content_div = null;

		content_div                       = document.createElement("div");
		content_div.id                    = "content_mod_div";
		content_div.style.background      = "transparent";
		content_div.innerHTML             = "<center><img src = '.././images/ajaxloader.gif'></img></center>";
		
        close_div                         = document.createElement("div");
		close_div.id                      = "close_mod_div";
		close_div.style.background        = "transparent";
		close_div.innerHTML               = "<center></br><input id = 'btn_close_breakdown' type = 'button' value = 'CLOSE' style = 'width: 23%; height: 24px'/></center>";

		this.inner_modal_div              = document.createElement("div");
		this.inner_modal_div.className    = "inner_div";		
		this.inner_modal_div.id           = "inner_mod_div";
		this.inner_modal_div.style.height = "auto";		
		this.inner_modal_div.style.width  = "400px";
		this.inner_modal_div.style.top    = "30px";
		this.inner_modal_div.style.left   = (parseInt(screen.width) - parseInt(this.inner_modal_div.style.width)) / 2 - 10 + "px";
		this.inner_modal_div.style.border = "medium solid white";
		this.inner_modal_div.appendChild(content_div);
		this.inner_modal_div.appendChild(close_div);

		document.body.appendChild(this.inner_modal_div);
	}
	
	this.load_modal_css = function()
	{	    
		var file_reference = document.createElement("link");
		
	  	file_reference.setAttribute("rel", "stylesheet");
	  	file_reference.setAttribute("type", "text/css");
	  	file_reference.setAttribute("href", ".././css/cssmodals.css");

		if (typeof file_reference != "undefined")
		{		    
	  		document.getElementsByTagName("head")[0].appendChild(file_reference);
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
	        if (divelement_outer.item(i).id == "outer_mod_div")
	        {
	            divelement_outer.item(i).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop;break;
	        }	        
	    }
	    for(m = 0; m < divelement_inner.length;m++)
	    {
	        if (divelement_inner.item(m).id == "inner_mod_div")
	        {
	            divelement_outer.item(m).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop + 30 : document.body.scrollTop + 30;break;
	        }
	    }
	}
}
/* END BREAKDOWN OF CHARGES */

/* 4.) CLIENT SEARCH */
function client_search()
{
    var client_search     = this;

    this.outer_modal_div  = null;
    this.inner_modal_div  = null;
    this.commons          = null;
    
    this.open_search = function()
    {
        this.open_modal_window();
        this.modal_client_search();

        document.getElementById("txt_search_swipecard").focus();
        document.getElementById("txt_search_cifid_gfmc").value = "592108";
    }

    this.modal_client_search = function()
    {
        var iContent = "";

        iContent += '<table cellpadding = "0"; cellspacing = "1" style = "width:400px;border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid" id="tbl_popsearchbox">';
        iContent += '    <tr><td class = "table_caption" colspan="2" style ="text-align:center">SEARCH CLIENT</td></tr>';
        iContent += '    <tr style ="height: 19px">';
        iContent += '       <td class = "cell_caption" style="text-align: right; width: 30%;">SWIPE CARD :</td>';
        iContent += '       <td class = "cell_desc" style="text-align: left; width: 70%;"><input id="txt_search_swipecard" type="text" style="width: 70%" /></td>';
        iContent += '   </tr>';
        iContent += '    <tr style ="height: 19px">';
        iContent += '       <td class = "cell_caption" style="text-align: right; width: 30%;">CIFID :</td>';
        iContent += '       <td class = "cell_desc" style="text-align: left; width: 70%;"><input id="txt_search_cifid" type="text" style="width: 70%"/></td>';
        iContent += '   </tr>';
        iContent += '    <tr style ="height: 19px">';
        iContent += '       <td class = "cell_caption" style="text-align: right; width: 30%;">Last Name :</td>';
        iContent += '       <td class = "cell_desc" style="text-align: left; width: 70%;"><input id="txt_search_lastname" type="text" style="width: 70%"/></td>';
        iContent += '   </tr>';
        iContent += '   <tr style ="height: 19px">';
        iContent += '       <td class = "cell_caption" style="text-align: right; width: 30%;">First Name :</td>';
        iContent += '       <td class = "cell_desc" style="text-align: left; width: 70%;"><input id="txt_search_firstname" type="text" style="width: 70%"/></td>';
        iContent += '   </tr>';
        iContent += '   <tr style ="height: 19px">';
        iContent += '       <td class = "cell_caption" style="text-align: right; width: 30%; height: 16px;">Middle Name :</td>';
        iContent += '       <td class = "cell_desc" style="text-align: left; width: 70%; height: 16px;"><input id="txt_search_middlename" type="text" style="width: 70%"/></td>';
        iContent += '   </tr>';
        
        // For added search criteria
        iContent += '    <tr style ="height: 19px">';
        iContent += '       <td class = "cell_caption" style="text-align: right; width: 30%;">IRSWorld Systems :</td>';
        iContent += '       <td align="left" style="width: 70%;"><select id="dd_search_sys_criteria" style="width: 72%"><option selected="selected"></option></select></td>';
        iContent += '   </tr>';
        iContent += '    <tr style ="height: 19px">';
        iContent += '       <td class = "cell_caption" style="text-align: right; width: 30%;">OLD CIFID :</td>';
        iContent += '       <td class = "cell_desc" style="text-align: left; width: 70%;"><input id="txt_search_cifid_old" type="text" style="width: 70%"/></td>';
        iContent += '   </tr>';
        iContent += '    <tr style ="height: 19px">';
        iContent += '       <td class = "cell_caption" style="text-align: right; width: 30%;">GFMC NO :</td>';
        iContent += '       <td class = "cell_desc" style="text-align: left; width: 70%;"><input id="txt_search_cifid_gfmc" type="text" style="width: 70%"/></td>';
        iContent += '   </tr>';                  
        // end
        // For Perfect Name Matching
        iContent += '    <tr style ="height: 19px">';
        iContent += '       <td class = "cell_caption" style="text-align: right; width: 30%;">is exact :</td>';
        iContent += '       <td class = "cell_desc" style="text-align: left; width: 70%;"><input id="chkIsExact" style="width: 21%" type="checkbox" /></td>';
        iContent += '   </tr>';         
        
        iContent += '   <tr style="height: 19px;text-align:center">';
        iContent += '       <td class="cell_desc" colspan="2"><input id="btn_search_now" type="button" value="SEARCH" style="width: 23%; height: 24px"/>&nbsp;<input id="btn_close_now" type="button" value="CANCEL" style="width: 23%; height: 24px"/></td>';
        iContent += '   </tr>';
        iContent += '</table>';
        
        document.getElementById("content_mod_div").innerHTML = iContent;
        
        document.getElementById("txt_search_swipecard").onkeyup = function(ev)
        {
            var e      = window.event || ev;
            var keynum = e.charCode || e.keyCode;

            if (keynum == 13)
            {        
                client_search.execute_search();
            }
        }
        
        document.getElementById("btn_search_now").onclick = function()
        {
            client_search.execute_search();
        }
        
        document.getElementById("btn_close_now").onclick = function()
        {
            client_search.close_modal_window();
        }
        
        document.getElementById("txt_search_cifid").onkeypress = function(ev)
        {
            var e      = window.event || ev;
            var keynum = e.charCode || e.keyCode;

            if (keynum == 13)
            {        
                client_search.execute_search();
            }
        }

        document.getElementById("txt_search_cifid_old").onkeypress = function(ev)
        {
            var e      = window.event || ev;
            var keynum = e.charCode || e.keyCode;

            if (keynum == 13)
            {        
                client_search.execute_search();
            }
        }
        
        document.getElementById("txt_search_cifid_gfmc").onkeypress = function(ev)
        {
            var e      = window.event || ev;
            var keynum = e.charCode || e.keyCode;

            if (keynum == 13)
            {        
                eval("javascript:execute_gfmc_nosearch(this.value)");
            }
        }

        document.getElementById("txt_search_lastname").onkeypress = function(ev)
        {
            var e      = window.event || ev;
            var keynum = e.charCode || e.keyCode;

            if (keynum == 13)
            {        
                client_search.execute_search();
            }
        }

        document.getElementById("txt_search_firstname").onkeypress = function(ev)
        {
            var e      = window.event || ev;
            var keynum = e.charCode || e.keyCode;

            if (keynum == 13)
            {        
                client_search.execute_search();
            }
        }

        document.getElementById("txt_search_middlename").onkeypress = function(ev)
        {
            var e      = window.event || ev;
            var keynum = e.charCode || e.keyCode;

            if (keynum == 13)
            {        
                client_search.execute_search();
            }
        }
        
        document.getElementById("chkIsExact").onkeypress = function(ev)
        {
            var e      = window.event || ev;
            var keynum = e.charCode || e.keyCode;

            if (keynum == 13)
            {        
                client_search.execute_search();
            }
        }        
        
        this.populate_dd_search_sys_criteria();
    }
    
    this.populate_dd_search_sys_criteria = function()
    {
        IRSWorld.TranClass.RemTranExtra.getdropdown_search_sys_criteria(this.populate_dd_search_sys_criteria_callback);    
    }
    
    this.populate_dd_search_sys_criteria_callback = function(result)
	{
	    if(result != null && result != "undefined")
	    {
	        this.commons = new common_method();         
			this.commons.to_load_dropdownlist("dd_search_sys_criteria", result.value, "FldValue", "FldDescription");			
			this.commons = null; 
	    }
	}
    
    this.execute_search = function()
    {
        var swipe_card = document.getElementById("txt_search_swipecard").value;

        if (swipe_card != null && swipe_card != "")
        {
            eval("javascript:execute_swipe_card(swipe_card)");
        }
        else
        {
            var customer_class  = 1;
            var customer_id     = document.getElementById("txt_search_cifid").value;
            var customer_id_old = document.getElementById("txt_search_cifid_old").value;
            var last_name       = document.getElementById("txt_search_lastname").value;
            var first_name      = document.getElementById("txt_search_firstname").value;
            var middle_name     = document.getElementById("txt_search_middlename").value;
            var sys_criteria    = document.getElementById("dd_search_sys_criteria").value;
            var isExact         = document.getElementById("chkIsExact").checked;
            var gfmc_cardno     = document.getElementById("txt_search_cifid_gfmc").value;
            
            var search_string = last_name + "~" + first_name + "~" + middle_name + "~" + customer_id + "~" + customer_id_old + "~" + ((sys_criteria == "null") ? "" : sys_criteria.toString()) + "~" + ((isExact == true) ? "true" : "false");

            if (last_name == '' && first_name == '' && middle_name == '' && customer_id == '' && customer_id_old == '' && gfmc_cardno != null && gfmc_cardno != '' && gfmc_cardno != '592108')
            {
                eval("javascript:execute_gfmc_nosearch(gfmc_cardno)");
            }
            else if (last_name == '' && first_name == '' && middle_name == '' && customer_id == '' && customer_id_old == '')
            {
                alert("Invalid search criteria"); return;
            }
            else
            {
                eval("javascript:execute_search_client(search_string)");
            }
        }
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

		document.body.style.margin = "";
	}

    this.construct_div_content = function()
	{
	    var content_div = null;

		content_div                       = document.createElement("div");
		content_div.id                    = "content_mod_div";
		content_div.style.background      = "transparent";
		content_div.innerHTML             = "<center><img src = '.././images/ajaxloader.gif'></img></center>";

		this.inner_modal_div              = document.createElement("div");
		this.inner_modal_div.className    = "inner_div";		
		this.inner_modal_div.id           = "inner_mod_div";
		this.inner_modal_div.style.height = "auto";		
		this.inner_modal_div.style.width  = "400px";
		this.inner_modal_div.style.top    = "30px";
		this.inner_modal_div.style.left   = (parseInt(screen.width) - parseInt(this.inner_modal_div.style.width)) / 2 - 10 + "px";
		this.inner_modal_div.style.border = "medium solid white";
		this.inner_modal_div.appendChild(content_div);

		document.body.appendChild(this.inner_modal_div);
	}
	
	this.load_modal_css = function()
	{	    
		var file_reference = document.createElement("link");
		
	  	file_reference.setAttribute("rel", "stylesheet");
	  	file_reference.setAttribute("type", "text/css");
	  	file_reference.setAttribute("href", ".././css/cssmodals.css");

		if (typeof file_reference != "undefined")
		{		    
	  		document.getElementsByTagName("head")[0].appendChild(file_reference);
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
	        if (divelement_outer.item(i).id == "outer_mod_div")
	        {
	            divelement_outer.item(i).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop;break;
	        }	        
	    }
	    for(m = 0; m < divelement_inner.length;m++)
	    {
	        if (divelement_inner.item(m).id == "inner_mod_div")
	        {
	            divelement_outer.item(m).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop + 30 : document.body.scrollTop + 30;break;
	        }
	    }
	}
}
/* END CLIENT SEARCH */

/* 5.) CUSTOMER ACCOUNT INFORMATION */
function rem_modal_accounts()
{
    var icls = this;

    this.branch_type        = null;
    this.account_number     = null;
    this.account_type       = null;
    this.bank_name          = null;
    this.CIFID              = null;
    this.currency_code      = null;
    this.mode               = null;
    this.view_mode          = null;
    this.account_seqid      = null;
    this.ref                = null;
    this.product_code       = null;
    this.implem_code        = null;
    this.remitter_id        = null;
    this.account_curtag     = null;
    this.msg_prompt_control = null;
    this.pddts_flag         = null;
    
    this.cif_fname          = null;
    this.cif_lname          = null;
    this.cif_mname          = null;
    this.cif_branchcode     = null;
    this.cif_userid         = null;
    
    this.branch_insert_code = null;
    this.branch_insert_name = null;
    this.bank_name_non_pchc = null;
    
    this.rural_bank_in_view_mode = true;
   
    this.page_onload = function()
    {
        AjaxPro.timeoutPeriod = 1000 * 1000;

        AjaxPro.onTimeout = function()
        {
            alert("Server not responding. Checking remitter beneficiary account failed.");
        }

        IRSWorld.TranClass.rem_account_screen.has_accounts(this.CIFID, parseInt(this.branch_type), this.page_onload_callback);
    }
  
    this.page_onload_callback = function(response)
    {
        if (response != null && response.value != null)
        {
            if (response.value == true)
            {
                icls.process_viewer('ajx_loading$');
                icls.mode = '0';
            }
            else
            {
                icls.mode = '1';
            }

            icls.change_mode();
            icls.populate_bene_info();
            if(parseInt(icls.product_code) == 13)
            {
                eval("javascript:btnRMANEWBANK.click();");
            }
        }
    }

    this.commit_transaction = function(transaction_id)
    {
        var column_value = "REM_SEQ_ID~" + transaction_id  + "|" + 
                           "BR_SEQ_ID~"  + this.bank_name;
        
        AjaxPro.timeoutPeriod = 1000 * 1000;

        AjaxPro.onTimeout = function()
        {
            alert("Server not responding. Commit remittance other details failed.");
        }
        
        IRSWorld.TranClass.rem_other_details.insert_record(data, this.commit_transaction_callback);
    }
    
    this.commit_transaction_callback = function(result)
    {
        if (result != null)
        {
            if (result.value == true)
            {
                icls.close_modal_window(); eval("javascript:push_door_to_bank_commit_response('1');");
            }
            else
            {
                icls.close_modal_window(); eval("javascript:push_door_to_bank_commit_response('0~ERROR SAVING OTHER DOOR TO BANK DETAILS.');");
            }
        }
    }

    this.change_mode = function()
    {
        this.hide_all();

        switch(this.mode)
        {
            case '0':

                if (document.getElementById('RMAACCOUNTGRID') != null){document.getElementById('RMAACCOUNTGRID').style.display = 'block';}
                if (document.getElementById('RMAADDBTN')      != null){document.getElementById('RMAADDBTN').style.display = 'block';}

                this.disable('txtRMAAcctnumber');
                this.disable('ddlRMACurr');
                this.disable('ddlRMAAcctType');
                this.disable('ddlRMABankName');                
                
                break;
                
            case '1':
                
                document.getElementById('RMAACCOUNTINFO').style.display = 'block';

                this.enable('txtRMAAcctnumber');
                this.enable('ddlRMACurr');
                this.enable('ddlRMAAcctType');
                this.enable('ddlRMABankName');

                break;     
        }  
    }
   
    this.populate_bene_info = function()
    {
        AjaxPro.timeoutPeriod = 1000 * 1000;
        
        AjaxPro.onTimeout = function()
        {
            alert("Server not responding. Loading beneficiary name failed.");
        }    
    
        IRSWorld.TranClass.rem_account_screen.getBeneName(this.CIFID, this.populate_bene_info_callback);
    }
    
    this.populate_bene_info_callback = function(response)
    {
        if (response != null && response.value != null)
        {
            var dt = response.value;

            setText("lblRMACIFID",     (dt.Rows[0].CIFID      != null) ? dt.Rows[0].CIFID.toString()      : "");
            setText("lblRMACustLName", (dt.Rows[0].LastName   != null) ? dt.Rows[0].LastName.toString()   : "");
            setText("lblRMACustFName", (dt.Rows[0].FirstName  != null) ? dt.Rows[0].FirstName.toString()  : "");
            setText("lblRMACustMName", (dt.Rows[0].MiddleName != null) ? dt.Rows[0].MiddleName.toString() : "");

            icls.populate_info(icls.account_seqid);
        }
    }
   
    this.clear_all = function()
    {
        var commons = new common_method();
        
        
        setText("txtRMAAcctnumber", "");              

        commons.to_set_dropdownlist_index('ddlRMACurr',     "");
        commons.to_set_dropdownlist_index('ddlRMAAcctType', "");
        commons.to_set_dropdownlist_index('ddlRMABankName', "");
    }       
   
    this.hide_all = function()
    {
        if (document.getElementById('RMAACCOUNTGRID') != null){document.getElementById('RMAACCOUNTGRID').style.display = 'none';}        
        if (document.getElementById('RMAACCOUNTINFO') != null){document.getElementById('RMAACCOUNTINFO').style.display = 'none';}
        if (document.getElementById('RMAADDBTN')      != null){document.getElementById('RMAADDBTN').style.display      = 'none';}
        if (document.getElementById('RMAANEWBANK')    != null){document.getElementById('RMAANEWBANK').style.display    = 'none';}
    }
    
    this.btn_RMA_cancel_click = function()
    {
         this.close_modal_window();
    }
     
    this.btn_RMA_ok_click = function()
    {
        this.currency_code = document.getElementById("ddlRMACurr").value;

        if (this.account_curtag != null && parseInt(this.account_curtag) > 0)
        {
            if (this.currency_code != null && parseInt(this.currency_code) > 0)
            {
                if (this.account_curtag == 1 && this.currency_code != 1)
                {
                    alert("Account currency must be in peso."); return;
                }
                if (this.account_curtag == 2 && this.currency_code != 2)
                {
                    alert("Account currency must be in dollar."); return;
                }
            }
        }

        this.set_variable();
         
        if (this.mode == 0)
        {
            this.close_modal_window();  eval("javascript:push_account_screen_result();");
        }
        else if (this.mode == 1)
        {
            var msg = this.validate();

            if (msg == "")
            {
                var data =  "CIFID~"    + this.CIFID;
                    
                data += "|AccountNo~"   + this.account_number;
                data += "|AccountType~" + this.account_type;
                data += "|Currency~"    + this.currency_code;
                //===============================================================================
                //RFC-24548 : E2E API Enhancement
                //Fix for saving account branch code
                //data += "|BranchCode~"  + this.bank_name;
                if (this.product_code == 10 && this.implem_code == 2) { data += "|BranchCode~" + this.bank_name; }
                else if (this.product_code == 22 && this.implem_code == 2) { data += "|BranchCode~" + this.bank_name; }
                else if (this.product_code == 11 && this.implem_code == 11) { data += "|BranchCode~" + this.bank_name; }
                else if (this.product_code == 11 && this.implem_code == 1) { data += "|BranchCode~" + this.bank_name; }
                else if (this.product_code == 14 && this.implem_code == 2) { data += "|BranchCode~" + this.bank_name; }
                else if (this.product_code == 16 && this.implem_code == 10) { data += "|DomBankCode~" + this.bank_name; }
                else if (this.product_code == 13 && this.implem_code == 5) { data += "|DomBankCode~" + this.bank_name; }
                else if (this.product_code == 12 && this.implem_code == 4) { data += "|DomBankCode~" + this.bank_name; }
                else if (this.product_code == 12 && this.implem_code == 5) { data += "|DomBankCode~" + this.bank_name; }
                else if (this.product_code == 2 && this.implem_code == 1) { data += "|BranchCode~" + this.bank_name; }
                else if (this.product_code == 5 && this.implem_code == 1) { data += "|BranchCode~" + this.bank_name; }
                else if (this.product_code == 6 && this.implem_code == 1) { data += "|BranchCode~" + this.bank_name; }
                else if (this.product_code == 258 && this.implem_code == 29) { data += "|DomBankCode~" + this.bank_name; }
                else if (this.product_code == 8 && this.implem_code == 3) { data += "|BranchCode~" + this.bank_name; }
                else if (this.product_code == 9 && this.implem_code == 3) { data += "|BranchCode~" + this.bank_name; }
                else if (this.product_code == 15 && this.implem_code == 7) { data += "|BranchCode~" + this.bank_name; }
                else { data += "|BranchCode~" + this.bank_name; }
                //===============================================================================
                data += "|BranchType~"  + this.branch_type;   

                AjaxPro.timeoutPeriod = 1000 * 1000;
                
                AjaxPro.onTimeout = function()
                {
                    alert("Server not responding. Commit account registration failed.");
                }

                IRSWorld.TranClass.rem_account_screen.insert_record(data, this.btn_RMA_ok_click_callback);
            }
            else
            {
                alert(msg);
            }
        }
    }
    
    this.validate = function()
    {
        if (this.account_number == null || this.account_number == "" || this.account_number.length < 1)
        {
            return "Invalid account number.";
        }
        
        if (this.account_type == null || this.account_type.toString().toUpperCase() == 'NULL' || this.account_type == "" || this.account_type.length < 1)
        {
            return "Invalid account type.";
        }
        
        if (this.currency_code == null || this.currency_code.toString().toUpperCase() == 'NULL' || this.currency_code == "" || this.currency_code.length < 1)
        {
            return "Invalid currency code.";
        }
        
        if (this.bank_name == null || this.bank_name.toString().toUpperCase() == 'NULL' || this.bank_name == "" || this.bank_name.length < 1)
        {
            return "Invalid bank/branch name.";
        }

        if (this.bank_name != null && this.bank_name == '000')
        {
            if (this.bank_name_non_pchc != null && this.bank_name_non_pchc.length < 1)
            {
                return "Invalid bank/branch name.";
            }
        }

        if (this.account_curtag != null && parseInt(this.account_curtag) > 0)
        {
            if (this.bank_name == '000')
            {
                return "";
            }

            if (this.currency_code != null && parseInt(this.currency_code) > 0)
            {
                if (this.account_curtag == 1 && this.currency_code != 1)
                {
                    return "Account currency must be in peso.";
                }
                if (this.account_curtag == 2 && this.currency_code != 2)
                {
                    return "Account currency must be in dollar.";
                }                
            }
        }
        
        return "";
    }
    
    this.set_variable = function()
    {
        this.account_number = document.getElementById('txtRMAAcctnumber').value;
        this.account_type   = document.getElementById('ddlRMAAcctType').value;
        this.bank_name      = document.getElementById('ddlRMABankName').value;
        this.currency_code  = document.getElementById('ddlRMACurr').value;
    }
    
    this.set_variable_none_pchc = function()
    {
        if (document.getElementById('txtRMAAcctnumber2') != null)
        {    
            this.account_number = document.getElementById('txtRMAAcctnumber2').value;
        }
        
        if (document.getElementById('ddlRMAAcctType2') != null)
        {        
            this.account_type = document.getElementById('ddlRMAAcctType2').value;
        }
        
        if (document.getElementById('ddlRMACurr2') != null)
        {        
            this.currency_code = document.getElementById('ddlRMACurr2').value;
        }
        
        if (document.getElementById('txtRMABankNameIns') != null)
        {                        
            this.bank_name_non_pchc = document.getElementById('txtRMABankNameIns').value;
        }

        this.bank_name = "000";
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
    
    this.load_flx_account_details = function(ev)
    {
        var e      = window.event || ev;
        var keynum = e.charCode   || e.keyCode;

        if (parseInt(this.branch_type) == 2)
        {
            if (keynum == 13)
            {
                AjaxPro.timeoutPeriod = 1000 * 1000;

                AjaxPro.onTimeout = function()
                {
                    this.close_msg_prompt(); alert("Server not responding. Loading account information failed.");
                }

                this.account_number = document.getElementById('txtRMAAcctnumber').value;
                this.open_msg_prompt("Loading account details. Please wait ..."); IRSWorld.TranClass.RemTranExtra.get_flexcube_account_information(this.account_number, this.load_flx_account_details_callback);
            }
        }
    }

    this.load_flx_account_details_callback = function(result)
    {
        icls.close_msg_prompt();

        if (result != null && result.value != null)
        {
            var account_details = result.value.split("~");

            if (account_details != null && account_details.length > 1)
            {                
                if (account_details[4] != null && account_details[4].length > 0)
                {
                    alert("Unable to load account information: " + account_details[4]);
                }
                else
                {
                    document.getElementById('txtRMAAcctnumber').value = account_details[3];
                    
                    var commons = new common_method();

                    commons.to_set_dropdownlist_index('ddlRMABankName', account_details[0]);
                    commons.to_set_dropdownlist_index('ddlRMAAcctType', account_details[1]);
                    commons.to_set_dropdownlist_index('ddlRMACurr',     account_details[2]);
                    commons = null;
                    
                    //Added for SG RFS 20160502
                    if (account_details.length >= 6)
                    {
                        var arr = account_details[6].split("=");
                        if (arr[0] == "1")
                        {
                            document.getElementById("trAccountName").style.display = "";
                            document.getElementById("txtAccountName").value = arr[1];
                        }
                        else
                        {
                            document.getElementById("trAccountName").style.display = "none";
                            document.getElementById("txtAccountName").value = "";
                        }
                    }
                    
                    icls.set_variable();
                }
            }
        }
        // CJ Alert on Invalid Account number input
        alert("Invalid Account number.");
        // CJ END
    }

    this.btn_RMA_ok_click_callback = function(response)
    {
        if (response != null && response.value != null)
        {
            if (response.value)
            {
                AjaxPro.timeoutPeriod = 1000 * 1000;
                
                AjaxPro.onTimeout = function()
                {
                    alert("Server not found. Loading account sequence failed.");
                }
            
                icls.account_seqid = IRSWorld.TranClass.rem_account_screen.get_seq_id().value; alert("Account created successfully.");
                icls.close_modal_window();

                eval("javascript:push_account_screen_result();");
            }
            else
            {
                AjaxPro.timeoutPeriod = 1000 * 1000;

                AjaxPro.onTimeout = function()
                {
                    alert("Server not responding. Checking account registration error message failed.");
                }
            
                var message = IRSWorld.TranClass.rem_account_screen.get_server_error_messages().value; alert(message);
            }
        }
    }

    this.get_seq_id_callback = function(response)
    {
        if (response != null && response.value != null)
        {
            icls.account_seqid = response.value;
        }
    }

    this.btn_RMA_add_new_click = function()
    {
        this.mode = '1';
        this.clear_all();
        this.change_mode();
    }  
  
    this.populate_drop_down_list = function() 
    {
        /* pddts_flag: 0 = ALL OTHER BANKS; 1 = PDDTS; 2 = NON PDDTS */
    
        this.pddts_flag = 0;

        if (this.branch_type == null)
        {
            this.branch_type = 0;
        }
    
        AjaxPro.timeoutPeriod = 1000 * 1000;
        
        AjaxPro.onTimeout = function()
        {
            alert("Server not responding. Loading dropdown information failed.");
        }        

        if (this.branch_type != null && parseInt(this.branch_type) == 5)
        {
            if (parseInt(this.implem_code) == 4)
            {
                this.pddts_flag = 1;
            }
            
            if (parseInt(this.implem_code) == 5)
            {
                this.pddts_flag = 2;
            }            
        }

        IRSWorld.TranClass.rem_account_screen.get_dropdown_data(this.branch_type, this.pddts_flag, this.populate_drop_down_list_callback);
    }

    this.populate_drop_down_list_callback = function(response)
    {
        if (response != null && response.value != null)
        {
            var ds      = response.value;
            var commons = new common_method();

            commons.to_load_dropdownlist('ddlRMACurr',     null, 'Value', 'Text', ds.Tables[0]);
            commons.to_load_dropdownlist('ddlRMAAcctType', null, 'Value', 'Text', ds.Tables[1]);
            commons.to_load_dropdownlist('ddlRMABankName', null, 'Value', 'Text', ds.Tables[2]);

            commons.to_set_dropdownlist_index('ddlRMACurr',     icls.currency_code);
            commons.to_set_dropdownlist_index('ddlRMAAcctType', icls.account_type);
            commons.to_set_dropdownlist_index('ddlRMABankName', icls.bank_name);
          
            commons.to_load_dropdownlist('ddlRMACurr2',     null, 'Value', 'Text', ds.Tables[0]);
            commons.to_load_dropdownlist('ddlRMAAcctType2', null, 'Value', 'Text', ds.Tables[1]);

            commons.to_set_dropdownlist_index('ddlRMACurr2',     icls.currency_code);
            commons.to_set_dropdownlist_index('ddlRMAAcctType2', icls.account_type);
                        
            commons = null;
        }
    }

    this.enable = function(element_id)
    {
        var id   = document.getElementById(element_id);
        var cset = id.disabled;
       
        id.disabled = cset == "false";
    }  

    this.disable = function(element_id)
    {
        var id   = document.getElementById(element_id);
        var cset = id.disabled;

        id.disabled = (cset == "false") ? "true" : "false";
    }

    this.display_account_scr = function(dt)
    {
        var dombank_code = dt.Rows[0].DomBankCode;
        
        if (document.getElementById('RMAACCOUNTINFO') != null)
        {
            document.getElementById('RMAACCOUNTINFO').style.display = 'none';
        }
        
        if (document.getElementById('RMAANEWBANK') != null)
        {
            document.getElementById('RMAANEWBANK').style.display = 'none';
        }

        if (document.getElementById('txtRMAAcctnumber') != null)
        {
            document.getElementById('txtRMAAcctnumber').value = (dt.Rows[0].AccountNo == null) ? "" : dt.Rows[0].AccountNo;
        }
        
        if (document.getElementById('txtRMAAcctnumber2') != null)
        {
            document.getElementById('txtRMAAcctnumber2').value = (dt.Rows[0].AccountNo == null) ? "" : dt.Rows[0].AccountNo;
        }
        
        if (dombank_code == null)
        {
            if (document.getElementById('RMAACCOUNTINFO') != null)
            {
                document.getElementById('RMAACCOUNTINFO').style.display = 'block'; return;
            }
        }

        if (dombank_code != null && dombank_code != '000')
        {        
            if (document.getElementById('RMAACCOUNTINFO') != null)
            {
                document.getElementById('RMAACCOUNTINFO').style.display = 'block';
            }
        }
        else 
        {
            if (this.product_code != null && parseInt(this.product_code) == 13)
            {
                if (this.implem_code != null && parseInt(this.implem_code) == 5)
                {
                    this.rural_bank_in_view_mode = true;

                    if (document.getElementById('RMAANEWBANK') != null)
                    {
                        document.getElementById('RMAANEWBANK').style.display = 'block';
                    }

                    if (document.getElementById('txtRMABankNameIns') != null)
                    {
                        document.getElementById('txtRMABankNameIns').value = dt.Rows[0].branchname;
                    }
                }
            }
            else
            {
                alert("Account number is not valid for processing. (Bank Name : " + dt.Rows[0].branchname + ")");
            }
        }
    }

    this.populate_info = function(account_seqid)
    {
        if (account_seqid == null)
        {
            account_seqid = 0;
        }

        AjaxPro.timeoutPeriod = 1000 * 1000;
        
        AjaxPro.onTimeout = function()
        {
            alert("Server not responding. Loading account information failed.");
        }

        IRSWorld.TranClass.rem_account_screen.getAccountInfo(parseInt(account_seqid), this.populate_info_callback);  
    }  
   
    this.populate_info_callback = function(response)
    {
        if (document.getElementById('btnRMANEWBANK') != null)
        {
            icls.disable('btnRMANEWBANK');
        }

        if (icls.product_code != null && parseInt(icls.product_code) == 13)
        {
            if (icls.implem_code != null && parseInt(icls.implem_code) == 5)
            {
                if (document.getElementById('btnRMANEWBANK') != null)
                {
                   icls.enable('btnRMANEWBANK');
                }
            }
        }

        if (response != null && response.value != null)
        {
            var dt = response.value;

            if (dt.Rows.length > 0)
            {
                icls.account_type   = dt.Rows[0].AccountType;
                icls.bank_name      = dt.Rows[0].BranchCode;
                icls.currency_code  = dt.Rows[0].Currency;
                icls.account_seqid  = dt.Rows[0].SEQ_ID;
                icls.account_number = dt.Rows[0].AccountNo;

                icls.display_account_scr(dt);
            }
        }

        icls.populate_drop_down_list();
    }

    this.do_ajaxgridview = function(parameter)
    {
        if (parameter != null && parameter != "undefined")
        {
            var command_param = parameter.split("$");

            if (command_param.length > 0)
            {
                if (command_param[0] == "ajx_selected")
                {
                    this.account_seqid =  command_param[1];
                    this.populate_info(command_param[1]);

                    return;
                }
               else
               {
                    this.process_viewer(parameter);
               }  
            }
        }
    }
       
    this.process_viewer = function(param)
    { 
        this.grid = new grid_viewer();       
        this.grid.sql_source     = "sp_get_accountlist~" + this.CIFID + "~" + this.branch_type+ "~";
        this.grid.div_container  = document.getElementById("RMAACCOUNTGRID");
        this.grid.datarow_column = 4;
        this.grid.page_size      = 10;
        this.grid.process_ajax(param);                               
        this.grid = null;
    }
   
    this.account_form = function(CIFID, branch_type, ref, account_seqid)
    {
       this.CIFID         = CIFID;
       this.branch_type   = parseInt(branch_type);
       this.account_seqid = parseInt(account_seqid);
       this.ref           = ref;

       this.open_modal_window();
       this.construct_override_window();
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
	
	this.construct_div_content = function()
	{
	    var content_div = null;

		content_div                       = document.createElement("div");		
		content_div.id                    = "content_mod_div";
		content_div.align                 = "left";
		content_div.style.background      = "transparent";
		content_div.style.width           = "100%";
		content_div.innerHTML             = "<center><img src = '.././images/ajaxloader.gif'></img></center>";

		this.inner_modal_div              = document.createElement("div");		
		this.inner_modal_div.align        = "center";
		this.inner_modal_div.className    = "inner_div";		
		this.inner_modal_div.id           = "inner_mod_div";
		this.inner_modal_div.style.height = "auto";		
		this.inner_modal_div.style.width  = "700px";
		this.inner_modal_div.style.top    = "30px";
		this.inner_modal_div.style.left   = (parseInt(screen.width) - parseInt(this.inner_modal_div.style.width)) / 2 - 10 + "px";
		this.inner_modal_div.style.border = "medium solid white";
		this.inner_modal_div.Code         = this;
		this.inner_modal_div.appendChild(content_div);
		
		document.body.appendChild(this.inner_modal_div);
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
	
	this.load_modal_css = function()
	{	    
		var file_reference = document.createElement("link");
		
	  	file_reference.setAttribute("rel", "stylesheet");
	  	file_reference.setAttribute("type", "text/css");
	  	file_reference.setAttribute("href", ".././css/cssmodals.css");

		if (typeof file_reference != "undefined")
		{		    
	  		document.getElementsByTagName("head")[0].appendChild(file_reference);
		}
	}	 
	
	window.onscroll = function() 
	{		
	    var i = 0;
	    var m = 0;
    	
    	var divelement_outer = document.documentElement.getElementsByTagName("div");
    	var divelement_inner = document.documentElement.getElementsByTagName("div");
    	    	    	
	    for(i = 0; i < divelement_outer.length;i++)
	    {
	        if (divelement_outer.item(i).id == "outer_mod_div")
	        {
	            divelement_outer.item(i).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop;break;
	        }	        
	    }
	    
	    for(m = 0; m < divelement_inner.length;m++)
	    {
	        if (divelement_inner.item(m).id == "inner_mod_div")
	        {
	            divelement_outer.item(m).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop + 20 : document.body.scrollTop + 20;break;
	        }	        
	    }	    
	}
   
    this.btn_add_new_bank = function()
    {
        var commons = new common_method();
    
        this.rural_bank_in_view_mode = false;

        if (document.getElementById('RMAANEWBANK')    != null){document.getElementById('RMAANEWBANK').style.display    = 'block';}
        if (document.getElementById('RMAACCOUNTINFO') != null){document.getElementById('RMAACCOUNTINFO').style.display = 'none';}
        if (document.getElementById('RMAADDBTN')      != null){document.getElementById('RMAADDBTN').style.display      = 'none';}
        if (document.getElementById('RMAACCOUNTGRID') != null){document.getElementById('RMAACCOUNTGRID').style.display = 'none';}

        if (document.getElementById('txtRMAAcctnumber') != null && document.getElementById('txtRMAAcctnumber').value != null && document.getElementById('txtRMAAcctnumber').value != "")
        {    
            document.getElementById('txtRMAAcctnumber2').value = document.getElementById('txtRMAAcctnumber').value;
        }
        
        if (document.getElementById('ddlRMAAcctType') != null && document.getElementById('ddlRMAAcctType').value != null && document.getElementById('ddlRMAAcctType').value != "")
        {
            this.account_type = document.getElementById('ddlRMAAcctType').value; commons.to_set_dropdownlist_index('ddlRMAAcctType2', this.account_type);
        }
        
        if (document.getElementById('ddlRMACurr') != null && document.getElementById('ddlRMACurr').value != null && document.getElementById('ddlRMACurr').value != "")
        {
            this.currency_code = document.getElementById('ddlRMACurr').value; commons.to_set_dropdownlist_index('ddlRMACurr2', this.currency_code);
        }
        
        commons = null;
    }

    this.btn_add_new_bank_save = function()
    {
        if (this.rural_bank_in_view_mode == true)
        {
            this.close_modal_window(); eval("javascript:push_account_screen_result();"); return;
        }

        this.set_variable_none_pchc();

        AjaxPro.timeoutPeriod = 1000 * 1000;

        AjaxPro.onTimeout = function()
        {
            alert("Connection Timeout."); return;
        }

        var msg = this.validate();

        if (msg != null && msg == "")
        {
            IRSWorld.TranClass.rem_account_screen.insert_rural_bank_account(this.CIFID, this.bank_name_non_pchc, this.account_number, this.currency_code, this.account_type , this.btn_add_new_bank_save_callback);
        }
        else
        {
            alert(msg);
        }
    }

    this.btn_add_new_bank_save_callback = function(response)
    {
        if (response != null && response.value != null)
        {
            var msg = response.value.split("~");

            if (msg != null)
            {
                if (parseInt(msg[0]) == 1)
                {
                    alert(msg[1]); return;
                }
                
                alert("Account created successfully.");                
                
                icls.account_seqid = msg[1];
                icls.close_modal_window();
                
                eval("javascript:push_account_screen_result();");
                
            }
        }
    }

    this.btn_cancel_new_bank = function()
    {
        if (this.rural_bank_in_view_mode == true)
        {
            this.close_modal_window(); eval("javascript:push_account_screen_result();"); return;
        }

        if (document.getElementById('RMAANEWBANK') != null)
        {
            document.getElementById('RMAANEWBANK').style.display = 'none';
        }

        this.page_onload();
    }

    this.construct_override_window = function()
    {
        if (this.CIFID != null && this.CIFID != "undefined" && this.CIFID != "" && this.branch_type != null && this.branch_type != "undefined" && this.branch_type != "")
        {            
            var html_string = "";
            
            html_string += "<center>";
            html_string += "<table style='width: 700px; border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid'>";
            html_string += "    <tr>";
            html_string += "        <td style='width: 100%; height: 100%;'>";
            
            html_string += "            <div id = 'RMACIFINFO' style= 'display:block'>";
            html_string += "                <table width='100%' cellpadding='0'>";
            html_string += "                    <tr>";
            html_string += "                        <td class='cell_desc' style='width: 19%; height: 24px; text-align: right;'>Beneficiary Name :</td>";
            html_string += "                        <td align='left' style='width: 38%; height: 24px;' colspan='4'>";
            html_string += "                            <label id='lblRMACustLName' class='cell_desc'></label>&nbsp;";
            html_string += "                            <label id='lblRMACustFName' class='cell_desc'></label>&nbsp;";
            html_string += "                            <label id='lblRMACustMName' class='cell_desc'></label>";
            html_string += "                        </td>";
            html_string += "                        <td class='cell_desc' style='width: 10%; height: 24px; text-align: right;' >CIF ID :</td>";
            html_string += "                        <td align='left' style='width: 26%; height: 24px;' colspan='4'><label id='lblRMACIFID' class='cell_desc'></label></td>";
            html_string += "                    </tr>";
            html_string += "                </table>";
            html_string += "            </div>";
            
            html_string += "            <div id = 'RMAACCOUNTGRID' style= 'display:block'>";
            html_string += "                <table id='Table10' width='100%' cellpadding='0' cellspacing='5' style='border: solid 1px #336699; background:azure;'>";
            html_string += "                    <tr><td style='width: 100%; text-align: left; text-indent: 5px; color: Black; height: 19px;' colspan='5'></td></tr>";
            html_string += "                    <tr><td style='height: 2px'><div id = 'Div2' style= 'display:none'>  PLACE GRIDVIEW ID'S</div></td></tr>";
            html_string += "                </table>";
            html_string += "            </div>";
            
            html_string += "            <div id = 'RMAADDBTN'>";
            html_string += "                <table width='100%' cellpadding='0' cellspacing='5' style='background:azure;'>";
            html_string += "                    <tr><td width='100%' style='height: 25px' align='center'><input style=' width:150px; height:25px' id='btnRMAADD' type='button' value='ADD NEW' onclick='javascript:" + this.ref + ".btn_RMA_add_new_click();'/>&nbsp;</td></tr>";
            html_string += "                </table>";
            html_string += "            </div>";
            
            html_string += "            <div id = 'RMAACCOUNTINFO'>";
            html_string += "                <table width='100%' cellpadding ='0' cellspacing ='3'>";
            html_string += "                    <tr>";
            html_string += "                        <td class = 'cell_caption' style = 'width: 11%; height:24px; text-align: right;'>Account Number :&nbsp;</td>";
            html_string += "                        <td align = 'left'         style = 'width:40%;  height:24px;' class='cell_desc'><input id='txtRMAAcctnumber' type='text' width='98%' maxlength='50' onkeydown = 'javascript:" + this.ref + ".load_flx_account_details(event);'/></td>";
            html_string += "                    </tr>";
            html_string += "                    <tr id='trAccountName' style='display:none;'>";
            html_string += "                        <td class='cell_caption' style='width: 11%; height:24px; text-align: right;'>Account Name :&nbsp;</td>";
            html_string += "                        <td align='left' style='width:35%; height: 24px;'><input id='txtAccountName' type='text' width='98%' /></td>";
            html_string += "                    </tr>";
            html_string += "                    <tr>";
            html_string += "                        <td class='cell_caption' style='width: 11%; height:24px; text-align: right;'>Currency :&nbsp;</td>";
            html_string += "                        <td align='left' style='width:35%; height: 24px;'><label id= 'Label2' class='cell_desc' style='font-size: 15px; font-weight:normal;'><select id='ddlRMACurr' style='width: 98%'><option selected='selected'></option></select></label></td>";
            html_string += "                    </tr>";
            html_string += "                    <tr>";
            html_string += "                        <td class='cell_caption' style='width: 11%; height:24px; text-align: right;'>Account Type :&nbsp; </td>";
            html_string += "                        <td align='left' style='width:40%; height: 24px;'><select id='ddlRMAAcctType' style='width: 98%'><option selected='selected'></option></select></td>";
            html_string += "                    </tr>";
            html_string += "                    <tr>";
            html_string += "                        <td class='cell_caption' style='width: 11%; height:24px; text-align: right;'>Bank Name :&nbsp; </td>";
            html_string += "                        <td align='left' style='width:40%; height: 24px;'><select id='ddlRMABankName' style='width: 98%'><option selected='selected'></option></select></td>";
            html_string += "                    </tr>";
            html_string += "                </table>";

            html_string += "                <table width = '100%' cellpadding = '0' cellspacing = '5' style = 'background:azure;'>";
            html_string += "                    <tr>";
            html_string += "                        <td style = 'text-align: center;'>";
            html_string += "                            <input style = 'width:100px; height: 25px;' id = 'btnRMANEWBANK' type = 'button' value = 'RURAL BANK' onclick = 'javascript:" + this.ref + ".btn_add_new_bank();'/>&nbsp;";
            html_string += "                            <input style = 'width:100px; height: 25px;' id = 'btnRMAOK'      type = 'button' value = 'OK'         onclick = 'javascript:" + this.ref + ".btn_RMA_ok_click();'/>&nbsp;";
            html_string += "                            <input style = 'width:100px; height: 25px;' id = 'btnRMACANCEL'  type = 'button' value = 'CLOSE'      onclick = 'javascript:" + this.ref + ".btn_RMA_cancel_click();'/>&nbsp;";
            html_string += "                        </td>";
            html_string += "                    </tr>";
            html_string += "                </table>";
            html_string += "            </div>";            

            html_string += "            <div id = 'RMAANEWBANK' style = 'display:none'>";
            html_string += "                <table width='100%' cellpadding='0' cellspacing='3'>";
            html_string += "                    <tr>";
            html_string += "                        <td class='cell_caption' style='width: 11%; height:24px; text-align: right;'>Account Number :&nbsp;</td>";
            html_string += "                        <td align='left' style='width:40%; height: 24px;' class='cell_desc'><input id='txtRMAAcctnumber2' type='text' width='98%' maxlength='50' onkeydown = 'javascript:" + this.ref + ".load_flx_account_details(event);'/></td>";
            html_string += "                    </tr>";
            html_string += "                    <tr>";
            html_string += "                        <td class='cell_caption' style='width: 11%; height:24px; text-align: right;'>Currency :&nbsp;</td>";
            html_string += "                        <td align='left' style='width:35%; height: 24px;'><label id= 'Label2' class='cell_desc' style='font-size: 15px; font-weight:normal;'><select id='ddlRMACurr2' style='width: 98%'><option selected='selected'></option></select></label></td>";
            html_string += "                    </tr>";
            html_string += "                    <tr>";
            html_string += "                        <td class='cell_caption' style='width: 11%; height:24px; text-align: right;'>Account Type :&nbsp; </td>";
            html_string += "                        <td align='left' style='width:40%; height: 24px;'><select id='ddlRMAAcctType2' style='width: 98%'><option selected='selected'></option></select></td>";
            html_string += "                    </tr>";
            html_string += "                    <tr>";
            html_string += "                        <td class='cell_caption' style='width: 11%; height:24px; text-align: right;'>Bank Name :&nbsp;</td>";
            html_string += "                        <td align='left' style='width:40%; height: 24px;' class='cell_desc'><input id='txtRMABankNameIns' type='text' width='98%' maxlength='50'/></td>";
            html_string += "                    </tr>";
            html_string += "                </table>";
            html_string += "                <table width='100%' cellpadding='0' cellspacing='5' style='background:azure;'>";
            html_string += "                    <tr>";
            html_string += "                       <td width='100%' style='height: 25px' align='center'>";
            html_string += "                           <input style=' width:150px;height: 25px' id = 'btnRMAADDNEWBANK' type = 'button' value = 'OK'    onclick = 'javascript:" + this.ref + ".btn_add_new_bank_save();'/>";
            //html_string += "                           <input style=' width:150px;height: 25px' id = 'btnRMACANNEWBANK' type = 'button' value = 'CLOSE' onclick = 'javascript:" + this.ref + ".btn_cancel_new_bank();'/>";
            html_string += "                           <input style=' width:150px;height: 25px' id = 'btnRMACANNEWBANK' type = 'button' value = 'CLOSE' onclick = 'javascript:" + this.ref + ".btn_RMA_cancel_click();'/>";
            
            
            html_string += "                       </td>";
            html_string += "                    <tr>";
            html_string += "                </table>";
            html_string += "            </div>";
            
            html_string += "     </td>";
            html_string += "    </tr>";
            html_string += "</table>";
            html_string += "</center>";

            document.getElementById("content_mod_div").innerHTML = html_string; this.page_onload();
       }
    }
}
/* END CUSTOMER ACCOUNT INFORMATION */


/* 6.) CLIENT OPENING OF ACCOUNT */
function rem_modal_open_accounts()
{
    var icls = this;
    
    this.branch_type   = null;
    this.account_type  = null;
    this.branch_name   = null;
    this.branch_code   = null; 
    this.branch_seq_id = null; 
    this.currency_code = null;
    this.temp_br_seqid = null;
    this.ref           = null;
   
    this.page_onload = function()
    {       
        this.temp_br_seqid = this.branch_seq_id;
             
        this.populate_drop_down_list();
        this.process_viewer('ajx_loading$');
        this.disable('txtRMOBranchName');
        this.disable('txtRMOBranchCode');  
    }
    
    this.reload = function()
    {
        var commons = new common_method();

        commons.to_set_dropdownlist_index('ddlRMOAcctType', this.account_type);
        commons.to_set_dropdownlist_index('ddlRMOCurrency', this.currency_code);
        commons = null;

        this.populate_info();
    }
    
    this.commit_transaction = function(REM_SEQ_ID)
    {
        var data =  "REM_SEQ_ID~" + REM_SEQ_ID;

        data += "|BR_SEQ_ID~"   + this.branch_seq_id;
        data += "|AccountType~" + this.account_type;
        data += "|Currency~"    + this.currency_code;
        
        AjaxPro.timeoutPeriod = 1000 * 1000;
        
        AjaxPro.onTimeout = function()
        {
            alert("Server not responding. Opening of accounts failed to commit.");
        }        
        
        IRSWorld.TranClass.rem_modal_open_accounts.commit_transaction(data, this.commit_transaction_callback);
    }
     
    this.commit_transaction_callback = function(response)
    {
        if (response.error != null)
        {
            eval("javascript:push_open_accounts_commit_response('0~FAILED TO SAVE ACCOUNT INFORMATION.');");
        }
        else
        {
            if (response.value.toString() == "1")
            {
                eval("javascript:push_open_accounts_commit_response('1');");
            }
            else
            {
                eval("javascript:push_open_accounts_commit_response(0~" + response.value + ");");
            }    
        }       
    }
     
    this.btn_RMO_cancel_click = function()
    {
         this.close_modal_window();
    }
     
    this.btn_RMO_ok_click = function()
    {
        this.set_variable();
    
        if (this.account_type == null  || this.account_type == '')
        {
            alert('Account type is empty.'); return;
        }
        
        if (this.currency_code == null || this.currency_code == '')
        {
            alert('Currency code is empty.'); return;
        }
        
        if (this.branch_code == null || this.branch_code == '')
        {
            alert('Branch code is empty.'); return;
        }

        this.close_modal_window();  eval("javascript:push_open_account_result();"); 
    }
    
    this.set_variable = function()
    {
        this.branch_code   = document.getElementById('txtRMOBranchCode').value;
        this.branch_name   = document.getElementById('txtRMOBranchName').value;        
        this.account_type  = parseInt(document.getElementById('ddlRMOAcctType').value);
        this.currency_code = parseInt(document.getElementById('ddlRMOCurrency').value);        
        this.branch_seq_id = parseInt(this.temp_br_seqid);
    } 
   
    this.populate_drop_down_list = function() 
    {
        AjaxPro.timeoutPeriod = 1000 * 1000;
        
        AjaxPro.onTimeout = function()
        {
            alert("Server not responding. Loading dropdown list failed.");
        }    
    
        IRSWorld.TranClass.rem_modal_open_accounts.getDDLData(this.populate_drop_down_list_callback);
    }
   
    this.populate_drop_down_list_callback = function(response)
    {
        if (response != null && response.value != null)
        {
            var ds      = response.value;
            var commons = new common_method();
            
            commons.to_load_dropdownlist('ddlRMOAcctType',null, 'Value', 'Text', ds.Tables[1]);
            commons.to_load_dropdownlist('ddlRMOCurrency',null, 'Value', 'Text', ds.Tables[0]);
            
            commons.to_set_dropdownlist_index("ddlRMOAcctType", parseInt(icls.account_type));
            commons.to_set_dropdownlist_index("ddlRMOCurrency", parseInt(icls.currency_code));
            commons = null;
        }

        icls.populate_info(icls.branch_seq_id);
    }
     
    this.enable = function(oName)
    {
        var id   = document.getElementById(oName);
        var cset = id.disabled;

        id.disabled = cset == "false";
    }

    this.disable = function(oName)
    {
        var id   = document.getElementById(oName);
        var cset = id.disabled;

        id.disabled = (cset == "false") ? "true" : "false";
    }
   
    this.populate_info = function(branch_seq_id)
    {
        AjaxPro.timeoutPeriod = 1000 * 1000;
        
        AjaxPro.onTimeout = function()
        {
            alert("Server not responding. Loading branch description failed.");
        }    
    
        IRSWorld.TranClass.rem_modal_open_accounts.get_branch_desc_name(parseInt(branch_seq_id), this.populate_info_callback);
    }  
   
    this.populate_info_callback = function(response)
    {
        if (response != null && response.value != null)
        {
            var dt = response.value;

            if (dt != null && dt.Rows.length > 0)
            {
                setText("txtRMOBranchName", (dt.Rows[0].BranchName == null) ? "" : dt.Rows[0].BranchName.toString());
                setText("txtRMOBranchCode", (dt.Rows[0].BranchCode == null) ? "" : dt.Rows[0].BranchCode.toString());
            }
        }
    }
         
    this.do_ajaxgridview = function(parameter)
    {
        if (parameter != null && parameter != "undefined")
        {
            var command_param = parameter.split("$");

            if (command_param.length > 0)
            {
                if (command_param[0] == "ajx_selected")
                {
                    this.temp_br_seqid = command_param[1];
                    this.populate_info(command_param[1]);
                    
                    return;
                }
               else if (command_param[0] == "ajx_search")
               {
                    this.process_viewer(parameter + document.getElementById('txtRMOFind').value); return;
               }
               else
               {
                    this.process_viewer(parameter);
               }
            }
        }
    }

    this.process_viewer = function(param)
    {
        this.grid = new grid_viewer();       
        this.grid.sql_source     = "sp_get_rem_open_accounts_branchlist~"; 
        this.grid.div_container  = document.getElementById("DVRMOGRID");
        this.grid.datarow_column = 2;
        this.grid.page_size      = 10;
        this.grid.process_ajax(param);                               
        this.grid = null;
    }
       
    this.open_account_form = function(branch_type, ref)
    {
       this.branch_type =  branch_type;
       this.ref         = ref;

       this.open_modal_window();
       this.construct_override_window();
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
	
	this.construct_div_content = function()
	{
	    var content_div = null;

		content_div                       = document.createElement("div");		
		content_div.id                    = "content_mod_div";
		content_div.align                 = "left";
		content_div.style.background      = "transparent";
		content_div.style.width           = "100%";
		content_div.innerHTML             = "<center><img src = '.././images/ajaxloader.gif'></img></center>";

		this.inner_modal_div              = document.createElement("div");		
		this.inner_modal_div.align        = "center";
		this.inner_modal_div.className    = "inner_div";		
		this.inner_modal_div.id           = "inner_mod_div";
		this.inner_modal_div.style.height = "auto";		
		this.inner_modal_div.style.width  = "700px";
		this.inner_modal_div.style.top    = "30px";
		this.inner_modal_div.style.left   = (parseInt(screen.width) - parseInt(this.inner_modal_div.style.width)) / 2 - 10 + "px";
		this.inner_modal_div.style.border = "medium solid white";
		this.inner_modal_div.Code         = this;
		this.inner_modal_div.appendChild(content_div);
		
		document.body.appendChild(this.inner_modal_div);
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
	
	this.load_modal_css = function()
	{	    
		var file_reference = document.createElement("link");
		
	  	file_reference.setAttribute("rel", "stylesheet");
	  	file_reference.setAttribute("type", "text/css");
	  	file_reference.setAttribute("href", ".././css/cssmodals.css");

		if (typeof file_reference != "undefined")
		{		    
	  		document.getElementsByTagName("head")[0].appendChild(file_reference);
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
	        if (divelement_outer.item(i).id == "outer_mod_div")
	        {
	            divelement_outer.item(i).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop;break;
	        }	        
	    }
	    for(m = 0; m < divelement_inner.length;m++)
	    {
	        if (divelement_inner.item(m).id == "inner_mod_div")
	        {
	            divelement_outer.item(m).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop + 20 : document.body.scrollTop + 20;break;
	        }	        
	    }	    
	}      
   
    this.construct_override_window = function()
    {
        if (this.branch_type != null && this.branch_type != "undefined" && this.branch_type != "")
        {            
            var html_string = "";

            html_string += "<center>";
            html_string += "<table style='width: 700px; border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid'>";
            html_string += "    <tr>";
            html_string += "        <td style='width: 100%; height: 100%;'>";
            html_string += "            <table width='100%' cellpadding='0' cellspacing='3' >";
            html_string += "                <tr>";
            html_string += "                    <td class='cell_caption' style='width: 11%; height:24px; text-align: right;'>Account Type :&nbsp;</td>";
            html_string += "                    <td align='left' style='width:35%; height: 24px;'><label class='cell_desc' style='font-size: 15px; font-weight:normal;'>&nbsp;<select id='ddlRMOAcctType' style='width: 98%'><option selected='selected'></option></select></label></td>";
            html_string += "                </tr>";
            html_string += "                <tr>";
            html_string += "                    <td class='cell_caption' style='width: 11%; height:24px; text-align: right;'>Currency :&nbsp;</td>";
            html_string += "                    <td align='left' style='width:35%; height: 24px;'><label class='cell_desc' style='font-size: 15px; font-weight:normal;'>&nbsp;<select id='ddlRMOCurrency' style='width: 98%'><option selected='selected'></option></select></label></td>";
            html_string += "                </tr>";
            html_string += "                <tr>";
            html_string += "                    <td class='cell_caption' style='width: 11%; height:24px; text-align: right;'>Branch Name :&nbsp;</td>";
            html_string += "                    <td align='left' style='width:35%; height: 24px;'>&nbsp;<input id='txtRMOBranchName' type='text' style='font-weight:normal; width: 97%;'/></td>";
            html_string += "                </tr>";
            html_string += "                <tr>";
            html_string += "                    <td class='cell_caption' style='width: 11%; height:24px; text-align: right;'>Branch Code :&nbsp;</td>";
            html_string += "                    <td align='left' style='width:35%; height: 24px;'>&nbsp;<input id='txtRMOBranchCode' type='text' style='font-weight:normal; width: 97%;'/></td>";
            html_string += "                </tr>";
            html_string += "                <tr>";
            html_string += "                    <td class='cell_caption' style='width: 11%; height:24px; text-align: right;'>Find Branch :&nbsp;</td>";
            html_string += "                    <td align='left' style='width:35%; height: 24px;'>&nbsp;<input id='txtRMOFind' type='text' style='font-weight:normal; width: 97%;' onkeyup = \"javascript:execute_ajaxgridview_command('ajx_search$') \"/></td>";
            html_string += "                </tr>";
            html_string += "            </table>";
            html_string += "        </td>";
            html_string += "    </tr>";
            html_string += "    <tr>";
            html_string += "        <td style='width: 100%; height: 100%;'>";            
            html_string += "            <div id = 'DVRMOGRID' style= 'display:block'></div>";
            html_string += "        </td>";
            html_string += "    </tr>";            
            html_string += "    <tr>";
            html_string += "        <td style='width: 100%; height: 100%;'>";
            html_string += "            <table width='100%' cellpadding='0' cellspacing='5' style='background:azure;'>";
            html_string += "                <tr>";
            html_string += "                    <td width='20%' style='height: 20px' align='right'>";
            html_string += "                        <input style=' width:150px; height: 25px;' id='btnRMOOK'     type='button' value='OK'     onclick='javascript:" + this.ref + ".btn_RMO_ok_click();'/>&nbsp;";
            html_string += "                        <input style=' width:150px; height: 25px;' id='btnRMOCANCEL' type='button' value='CANCEL' onclick='javascript:" + this.ref + ".btn_RMO_cancel_click();'/>&nbsp;";
            html_string += "                    </td>";
            html_string += "                </tr>";
            html_string += "            </table>";
            html_string += "        </td>";
            html_string += "    </tr>";
            html_string += "</table>";
            html_string += "</center>";
            
            document.getElementById("content_mod_div").innerHTML = html_string; this.page_onload();
       }
    }
}
/* END CLIENT OPENING OF ACCOUNT */

/* 7.) CHECK PAYMENT SCREEN */
function rem_modal_check_payments()
{
    var icls = this;

    this.check_number   = null;
    this.clearing_date  = null;
    this.branch_code    = null;
    this.ref            = null;
    this.seq_id         = null; 
    this.transaction_id = null;
   
    this.page_onload = function()
    {
        this.process_viewer('ajx_loading$');
    }    
    
    this.commit_transaction = function()
    {
        var data =  "rem_seq_id~" + this.transaction_id;
        
        data += "|bank_code~"     + ((this.seq_id        == null) ? ' ' : this.seq_id);
        data += "|clearing_date~" + ((this.clearing_date == null) ? ' ' : this.clearing_date);
        data += "|check_no~"      + ((this.check_number  == null) ? ' ' : this.check_number);

        AjaxPro.timeoutPeriod = 1000 * 1000;
        
        AjaxPro.onTimeout = function()
        {
            alert("Server not responding. Commit cheque payment failed.");
        }

        IRSWorld.TranClass.rem_modal_check_payments.commit_transaction(data, this.commit_transaction_callback);
    }
     
    this.commit_transaction_callback = function(response)
    {
        if (response.error != null)
        {
            eval("javascript:push_check_payments_commit_response('0~FAILED TO UPDATE CHEQUE PAYMENT DETAILS');");
        }
        else
        {
            if (response.value.toString() == "1")
            {
                eval("javascript:push_check_payments_commit_response('1');");
            }
            else
            {
                var s = '0~' + response.value; eval("javascript:push_check_payments_commit_response(s);");
            }
        }
    }
    
    this.btn_CP_ok_click = function()
    {   
        this.set_variable();
       
        if (this.check_number == null || this.check_number == '' || this.check_number.toUpperCase() == 'NULL' )
        {
            alert('Cheque number is empty.'); return;
        }
        
       /* commented by : boyet/neil 06.06.12
        if (!isNumeric(this.check_number))
        {
            alert('Cheque number not in numeric.'); return;
        }  
        */
        
        if (this.clearing_date == null || this.clearing_date == '' || this.clearing_date.toUpperCase() == 'NULL' )
        {
            alert('Clearing date is empty.'); return;
        }
        
        /*
        /if (this.branch_code == null || this.branch_code == '' || this.branch_code.toUpperCase() == 'NULL' )
        //{
            //alert('Branch code is empty.'); return;
        //}
        */

        this.chek_clearing();
    }
    
    this.chek_clearing = function()
    {
        AjaxPro.timeoutPeriod = 1000 * 1000;
        
        AjaxPro.onTimeout = function()
        {
            alert("Server not responding. Checking clearing date failed.");
        }    
    
        IRSWorld.TranClass.rem_modal_check_payments.check_clearing_date(this.clearing_date, this.chek_clearing_callback);
    }
    
    this.chek_clearing_callback = function(result)
    {
        if (result != null && result.value != null)
        {
            if (result.value == false)
            {
                alert('Invalid clearing date.'); return;
            }
        }
        
        icls.close_modal_window();  eval("javascript:push_check_payment_result();");
    }
    
    
    this.btn_CP_cancel_click = function()
    {
        this.close_modal_window();
    }
    
    this.set_variable = function()
    {
        this.check_number  = document.getElementById('txtCPCheckNo').value;
        this.clearing_date = document.getElementById('txtCPClearingDate').value;
    }

    this.populateInfo = function()
    {
        AjaxPro.timeoutPeriod = 1000 * 1000;
        
        AjaxPro.onTimeout = function()
        {
            alert("Server not responding. Loading branch information failed.");
        }    
    
        IRSWorld.TranClass.rem_modal_check_payments.get_branch_info(this.seq_id, this.populateInfo_Callback);  
    }  
   
    this.populateInfo_Callback = function(response)
    {
        if (response.error != null)
        {   
            alert("NO DATASET FOUND."); 
        }
        else
        {
            var dt = response.value;

            if (dt != null && dt.Rows.length > 0)
            {
                setText("txtCPBankName", ((dt.Rows[0].BranchCode == null) ? "" : dt.Rows[0].BranchCode.toString()) + " - " + ((dt.Rows[0].BranchName == null) ? "" : dt.Rows[0].BranchName.toString()));
                
                icls.branch_code =  (dt.Rows[0].BranchCode == null) ? "" : dt.Rows[0].BranchCode.toString();
                this.value = icls;
            }
        }
    }

    this.do_ajaxgridview = function(parameter)
    {               
        if (parameter != null && parameter != "undefined")
        {
            var command_param = parameter.split("$");

            if (command_param.length > 0)
            {
                if (command_param[0] == "ajx_selected")
                {
                    this.seq_id =  command_param[1];
                    this.populateInfo();

                    return;
                }
               else if (command_param[0] == "ajx_search")
               {
                    this.process_viewer(parameter + document.getElementById('txtCPSearchBank').value); 
               }  
               else
               {
                    this.process_viewer(parameter);
               }  
            }
           document.getElementById('txtCPSearchBank').focus();                                
        }                            
    }   
    this.process_viewer = function(param)
    { 
        this.grid = new grid_viewer();       
        
        this.grid.sql_source     = "sp_get_rem_check_payment_branchlist~";
        this.grid.div_container  = document.getElementById("CPCHECKPAYMENTGRID");
        this.grid.datarow_column = 2;
        this.grid.page_size      = 10;
        this.grid.process_ajax(param);                               
        this.grid = null; 
        
        document.getElementById('txtCPSearchBank').focus();                  
    }

    this.open_check_payment_form = function(ref)
    {       
       this.ref = ref;       
       this.open_modal_window();
       this.construct_override_window();
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
	
	this.construct_div_content = function()
	{
	    var content_div = null;

		content_div                       = document.createElement("div");		
		content_div.id                    = "content_mod_div";
		content_div.align                 = "left";
		content_div.style.background      = "transparent";
		content_div.style.width           = "100%";
		content_div.innerHTML             = "<center><img src = '.././images/ajaxloader.gif'></img></center>";

		this.inner_modal_div              = document.createElement("div");		
		this.inner_modal_div.align        = "center";
		this.inner_modal_div.className    = "inner_div";		
		this.inner_modal_div.id           = "inner_mod_div";
		this.inner_modal_div.style.height = "auto";		
		this.inner_modal_div.style.width  = "700px";
		this.inner_modal_div.style.top    = "30px"
		this.inner_modal_div.style.left   = (parseInt(screen.width) - parseInt(this.inner_modal_div.style.width)) / 2 - 10;
		this.inner_modal_div.style.border = "medium solid white";
		this.inner_modal_div.Code         = this;
		this.inner_modal_div.appendChild(content_div);
		
		document.body.appendChild(this.inner_modal_div);
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
	
	this.load_modal_css = function()
	{	    
		var file_reference = document.createElement("link");
		
	  	file_reference.setAttribute("rel", "stylesheet");
	  	file_reference.setAttribute("type", "text/css");
	  	file_reference.setAttribute("href", ".././css/cssmodals.css");

		if (typeof file_reference != "undefined")
		{		    
	  		document.getElementsByTagName("head")[0].appendChild(file_reference);
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
	        if (divelement_outer.item(i).id == "outer_mod_div")
	        {
	            divelement_outer.item(i).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop;break;
	        }	        
	    }
	    for(m = 0; m < divelement_inner.length;m++)
	    {
	        if (divelement_inner.item(m).id == "inner_mod_div")
	        {
	            divelement_outer.item(m).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop + 20 : document.body.scrollTop + 20;break;
	        }	        
	    }	    
	}      
   
    this.construct_override_window = function()
    {
        if (this.ref != null && this.ref != "undefined" && this.ref != "")
        {            
            var html_string = "";
            
            html_string += "<center>";
            html_string += "<table style='border: solid 1px #336699;' cellspacing='1' cellpadding='0' width='700px'>";
            html_string += "    <tr>";
            html_string += "        <td>";
            html_string += "            <table width='100%' cellpadding='0' cellspacing='3' >";
            html_string += "                <tr>";
            html_string += "                    <td class='cell_caption' style='width: 11%; height:24px; text-align: right;'>Clearing Date :&nbsp;</td>";
            html_string += "                    <td align='left' style='width:35%; height: 24px;'><label class='cell_desc' style='font-size: 15px; font-weight:normal;' id='txtCPAcctNumber'>&nbsp;<input id='txtCPClearingDate' type='text' style='font-weight:normal; width: 90%;' maxlength='50' disabled='disabled';/><img src='../images/calendar/imgcalen.png' align='absmiddle' onclick='popUpCalendar(this, document.getElementById(\"txtCPClearingDate\"), \"mm/dd/yyyy\")' /></label></td>";
            html_string += "                </tr>";
            html_string += "                <tr>";
            html_string += "                    <td class='cell_caption' style='width: 11%; height:24px; text-align: right;'>Check Number :&nbsp;</td>";
            html_string += "                    <td align='left' style='width:35%; height: 24px;'><label class='cell_desc' style='font-size: 15px; font-weight:normal;'>&nbsp;<input id='txtCPCheckNo' type='text' style='font-weight:normal; width: 97%;' maxlength='50';/></label></td>";
            html_string += "                </tr>";
            
            /*
            //html_string += "                <tr>";
            //html_string += "                    <td class='cell_caption' style='width: 11%; height:24px; text-align: right;'>Bank Name :&nbsp;</td>";
            //html_string += "                    <td align='left' style='width:35%; height: 24px;'>&nbsp;<input id='txtCPBankName' type='text' style='font-weight:normal; width: 97%;' maxlength='100' disabled='disabled';/></td>";
            //html_string += "                </tr>";
            //html_string += "                <tr>";
            //html_string += "                    <td class='cell_caption' style='width: 11%; height:24px; text-align: right;'>Search Bank :&nbsp;</td>";
            //html_string += "                    <td align='left' style='width:35%; height: 24px;'>&nbsp;<input id='txtCPSearchBank' type='text' style='font-weight:normal; width: 97%;' maxlength='50' onkeyup = \"javascript:execute_ajaxgridview_command('ajx_search$') \"/></td>";
            //html_string += "                </tr>";
            */
            
            html_string += "            </table>";
            html_string += "        </td>";
            html_string += "    </tr>";
            
            /*
            //html_string += "    <tr>";
            //html_string += "        <td>";
            //html_string += "            <div id = 'CPCHECKPAYMENTGRID' style= 'display:block'>PLACE GRIDVIEW ID'S</div>";
            //html_string += "        </td>";
            //html_string += "    </tr>";
            */
            
            html_string += "    <tr>";
            html_string += "        <td>";
            html_string += "            <input style=' width:150px;' id='Button1' type='button' value='OK' onclick='javascript:" + this.ref + ".btn_CP_ok_click();'/>&nbsp";
            html_string += "        </td>";
            html_string += "    </tr>";
            html_string += "</table>";
            html_string += "</center>";

            document.getElementById("content_mod_div").innerHTML = html_string; 
            
            //this.page_onload();
       }
       else
       {
            this.close_modal_window(); alert('PARAMETER WAS NOT SUPPLIED.');
       }
    }
}
/* END CHECK PAYMENT SCREEN */


/* 8.) 3D APAS BATCH APAS ONLINE PAW BPO SCREEN */
function rem_modal_popups()
{
    var global_screens      = this;
    var current_branch_type = null;

    this.pop_mode       = null;
    this.br_seq_id      = null;
    this.temp_br_seq_id = null;
    this.currency       = null;
    this.account_number = null;
    this.branch_code    = null;
    this.msg_prompt_control = null;

    this.product_code        = 0;
    this.implementation_code = 0;
    this.beneficiary_id      = 0;
    this.account_type        = 0;
    this.account_sequence_id = 0;

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
    
    this.check_bpo_account_details = function(account_number)
    {
        this.open_msg_prompt("Checking account please wait ...");
        this.account_number = account_number;

        IRSWorld.TranClass.RemTranExtra.get_flexcube_account_information(account_number, this.check_bpo_account_details_callback);
    }
    
    this.check_bpo_account_details_callback = function(result)
    {
        var account_info = null;
        
        if (result != null && result.value != null)
        {
            account_info = result.value.split('~');

            if (account_info != null && account_info != '' && account_info[4] != null && account_info[4] == '')
            {
                global_screens.br_seq_id      = account_info[5];
                global_screens.temp_br_seq_id = account_info[5];
                global_screens.currency       = account_info[2];
                global_screens.account_type   = account_info[1];
                global_screens.branch_code    = account_info[0];
            }
        }
        
        if (global_screens.br_seq_id == null || global_screens.br_seq_id == '' || parseInt(global_screens.br_seq_id) == 0)
        {
            alert("Branch code is required."); return;
        }
        
        if (global_screens.currency == null || global_screens.currency == '' || parseInt(global_screens.currency) == 0)
        {
            alert("Account currency is required."); return;
        }
        
        if (global_screens.account_number == null || global_screens.account_number == '' || global_screens.account_number.length == 0)
        {
            alert("Account number is required."); return;
        }

        IRSWorld.TranClass.rem_bpo_create_accounts.insert_record(global_screens.beneficiary_id, global_screens.account_number, global_screens.account_type, global_screens.currency, global_screens.br_seq_id , global_screens.branch_code, global_screens.commit_bpo_account_setup_callback);

    }
    
    this.commit_bpo_account_setup_callback = function(result)
    {
        global_screens.close_msg_prompt();
    
        if (result != null && result.value != null)
        {
            if (parseInt(result.value) > 0)
            {
                global_screens.account_sequence_id = result.value;
                global_screens.close_modal_window();

                eval("javascript:push_bulk_payment_order_result();");
            }
            else
            {
                alert("Invalid domestic account.");
            }
        }
    }

    this.commit_transaction = function(REM_SEQ_ID)
    {
        var data =  "REM_SEQ_ID~" + REM_SEQ_ID;
            data += "|BR_SEQ_ID~" + ((this.br_seq_id == null) ? ' ' : this.br_seq_id);
            data += "|Currency~"  + ((this.currency == null)  ? ' ' : this.currency);


        AjaxPro.timeoutPeriod = 1000 * 1000;
        
        AjaxPro.onTimeout = function()
        {
            alert("Server not responding. Commit remittance other information failed.");
        }

        IRSWorld.TranClass.rem_modal_open_accounts.commit_transaction(data, this.commit_transaction_callback);
    }
     
    this.commit_transaction_callback = function(response)
    {
        var response_method = null;

        switch(global_screens.pop_mode)
        {
            case 0 : response_method = 'push_door_to_door_commit_response'; break;
            case 1 : response_method = 'push_apas_batch_commit_response';   break;
            case 2 : response_method = 'push_apas_online_commit_response';  break;
            case 3 : response_method = 'push_paw_commit_response';          break;
            case 4 : response_method = 'push_bpo_commit_response';          break;
        }

        if (response.error != null)
        {
            eval("javascript:" + response_method + "('0~FAILED TO UPDATE OTHER REMITTANCE INFORMATION.');");
        }
        else
        {
            if (response.value.toString() == "1")
            {
                eval("javascript:" + response_method + "('1');");
            }
            else
            {
                eval("javascript:" + response_method + "(0~" + response.value.toString() + ");");
            }    
        }      
    }
    
    this.open_door_to_door= function(cifid)
    {        
        this.pop_mode  = 0;
        this.check_serviceable_location(cifid);    
    }    
    
    this.open_apas_batch = function()
    {        
        this.pop_mode = 1;
        this.open_modal_window();
        this.construct_apas_batch_list();
    }
    this.open_apas_online = function()
    {
        this.pop_mode = 2;
        this.open_modal_window();
        this.construct_apas_online_list();
    }

    this.open_paw = function()
    {        
        this.pop_mode  = 3;
        this.open_modal_window();
        this.construct_paw_list();
    }
    
    this.open_bpo_implem = function()
    {
        this.pop_mode  = 4;
        this.open_modal_window();
        this.construct_bulk_payment_order();
    }
    
    this.reload_record = function()
    {
        global_screens.open_msg_prompt("Loading please wait ...");
        
        if (this.pop_mode != null && parseInt(this.pop_mode) != 2)
        {
            AjaxPro.timeoutPeriod = 1000 * 1000;

            AjaxPro.onTimeout = function()
            {
                alert("Server not responding. Loading branch name failed.");
            }
        
            IRSWorld.TranClass.rem_modal_open_accounts.get_branch_desc_name(parseInt(this.br_seq_id), this.reload_record_callback);
        }
    }
    
    this.reload_record_callback = function(response)
    {
        global_screens.close_msg_prompt();
    
        if (response != null && response.value != null)
        {
            var dt = response.value;
            
            if (dt != null && dt.Rows.length > 0)
            {
                setText("lbl_3d_branch_name", dt.Rows[0].BranchName.toString());
                setText("lbl_3d_branch_code", dt.Rows[0].BranchCode.toString());
                setText("lbl_3d_branch_name", dt.Rows[0].BranchName.toString());
                setText("lbl_3d_branch_code", dt.Rows[0].BranchCode.toString());
            }
        }
    }    
    
    this.zip_code_viewer = function(param)
    { 
        this.grid = new grid_viewer();       
        this.grid.sql_source     = "sp_get_zipcode~"; 
        this.grid.div_container  = document.getElementById("div_3D_osa");
        this.grid.datarow_column = 0;
        this.grid.page_size      = 10;
        this.grid.process_ajax(param);                               
        this.grid = null;
    } 

    this.branch_viewer = function(param, branchtype)
    {
        if (branchtype != null || typeof branchtype != 'undefined')
        {
            current_branch_type = branchtype;
        }
        else
        {
            branchtype = current_branch_type;
        }

        this.grid = new grid_viewer();       
        this.grid.sql_source     = "sp_get_branches~" + branchtype + "~"; 
        this.grid.div_container  = document.getElementById("div_3d_non_osa");
        this.grid.datarow_column = 0;
        this.grid.page_size      = 5;
        this.grid.process_ajax(param);                               
        this.grid = null;
    }
    
    this.construct_serviceable_location = function()
    {
        var ihtml = "";
        
        ihtml = '<table cellpadding = "0"; cellspacing = "1" style = "width:700px; border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid">';
        ihtml += '    <tr>';
        ihtml += '        <td class = "cell_caption" height: style="width: 15%; height: 20px">ADDRESS:</td>';
        ihtml += '        <td class = "cell_desc" style = "width: 50%; height: 20px" text-align:><label id="lbl_3d_address"></label></td>';
        ihtml += '    </tr>';
        ihtml += '    <tr>';
        ihtml += '        <td class = "cell_caption" height: style="width: 15%; height: 20px">ZIP CODE:</td>';
        ihtml += '        <td class = "cell_desc" style = "width: 50%; height: 20px" text-align:><label id="lbl_3d_zipcode"></label></td>';
        ihtml += '    </tr>';
        ihtml += '    <tr>';
        ihtml += '        <td class = "cell_caption" height: style="width: 15%; height: 20px">TARGET CURRENCY:</td>';
        ihtml += '        <td class = "cell_desc" style = "width: 50%; height: 20px" text-align:><select id="ddltargetccy"></select></td>';
        ihtml += '    </tr>';
        ihtml += '    <tr>';
        ihtml += '        <td colspan="2" style="text-align: center"><input id="btn_3D_ZIP_OK" type="button" value="OK" style="width: 100px; height: 25px" />&nbsp;<input id="btn_3D_ZIP_Cancel" type="button" value="Cancel" style="width: 100px; height: 25px" /></td>';
        ihtml += '    </tr>';
        ihtml += '</table>';

        document.getElementById("content_mod_div").innerHTML = ihtml;
                
        document.getElementById("btn_3D_ZIP_OK").onclick = function()
        {
            global_screens.currency = document.getElementById('ddltargetccy').value;  
            global_screens.close_modal_window();

            eval("javascript:push_door_to_door_result();");            
        }
        
        document.getElementById("btn_3D_ZIP_Cancel").onclick = function()
        {
            global_screens.close_modal_window();
        }
    }
    
    this.construct_door_branch_currency_list = function()
    {
        var ihtml = "";
        
        ihtml = '<table cellpadding = "0"; cellspacing = "1" style = "width:700px;border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid">';
        ihtml += '    <tr>';
        ihtml += '        <td class = "cell_desc" colspan="2" style = "height: 23px"><center><label id="lbl_3d_note"></label></center></td>';
        ihtml += '    </tr>';
        ihtml += '    <tr>';
        ihtml += '        <td class = "cell_caption" style = "width: 10%; height: 20px">FIND:</td>';
        ihtml += '        <td class = "cell_desc"    style = "width: 20%; height: 23px; text-align: left"><input id = "Text2" onkeyup = "javascript:execute_ajaxgridview_command(\'ajx_search$\')" type = "text" /></td>';
        ihtml += '    </tr>';
        ihtml += '    <tr>';
        ihtml += '        <td colspan="2" style="height: 23px"><div id="div_3d_non_osa"></div></td>';
        ihtml += '    </tr>';
        ihtml += '    <tr>';
        ihtml += '        <td class = "cell_caption" style = "width: 10%; height: 20px">BRANCH CODE:</td>';
        ihtml += '        <td class = "cell_desc"    style = "width: 40%; height: 20px; text-align: left" ><label id="lbl_3d_branch_code"></label></td>';
        ihtml += '    </tr>';
        ihtml += '    <tr>';
        ihtml += '        <td class = "cell_caption" style = "width: 10%; height: 20px">BRANCH NAME:</td>';
        ihtml += '        <td class = "cell_desc"    style = "width: 40%; height: 20px; text-align: left"><label id="lbl_3d_branch_name"></label></td>';
        ihtml += '    </tr>';
        ihtml += '    <tr>';
        ihtml += '        <td class = "cell_caption" style = "width: 10%; height: 20px">TARGET CURRENCY:</td>';
        ihtml += '        <td class = "cell_desc"    style = "width: 40%; height: 20px; text-align: left"><select id="ddltargetccy"></select></td>';
        ihtml += '    </tr>';
        ihtml += '    <tr>';
        ihtml += '        <td colspan="2" style="height: 23px; text-align:center"><input id="btn_3D_branch_ok" style="width: 100px; height: 25px" type="button" value="OK" />&nbsp;<input id="btn_3d_branch_cancel" style="width: 100px; height: 25px" type="button" value="Cancel" /></td>';
        ihtml += '    </tr>';
        ihtml += '</table>';
        
        document.getElementById("content_mod_div").innerHTML = ihtml;
                
        document.getElementById("btn_3D_branch_ok").onclick = function()
        {
            global_screens.currency = document.getElementById('ddltargetccy').value;
            global_screens.close_modal_window();

            eval("javascript:push_door_to_door_result();");            
        }

        document.getElementById("btn_3d_branch_cancel").onclick = function()
        {  
            global_screens.close_modal_window();
        }
    }
        
     this.construct_apas_batch_list = function()
     {
        var ihtml = "";

        ihtml = '<table cellpadding = "0"; cellspacing = "1" style = "width:700px;border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid">';
        ihtml += '    <tr>';
        ihtml += '        <td class = "cell_desc" colspan="2" style="height: 23px"><center><label id="lbl_3d_note"></label></center></td>';
        ihtml += '    </tr>';
        ihtml += '    <tr>';
        ihtml += '        <td class = "cell_caption" style = "width: 10%; height: 20px">FIND:</td>';
        ihtml += '        <td class = "cell_desc"    style = "width: 20%; height: 23px; text-align: left"><input id = "Text2" onkeyup = "javascript:execute_ajaxgridview_command(\'ajx_search$\')" type = "text" /></td>';
        ihtml += '    </tr>';
        ihtml += '    <tr>';
        ihtml += '        <td colspan="2" style="height: 23px"><div id="div_3d_non_osa"></div></td>';
        ihtml += '    </tr>';
        ihtml += '    <tr>';
        ihtml += '        <td class = "cell_caption" style = "width: 10%; height: 20px">BRANCH CODE:</td>';
        ihtml += '        <td class = "cell_desc"    style = "width: 40%; height: 20px; text-align: left" ><label id="lbl_3d_branch_code"></label></td>';
        ihtml += '    </tr>';
        ihtml += '    <tr>';
        ihtml += '        <td class = "cell_caption" style = "width: 10%; height: 20px">BRANCH NAME:</td>';
        ihtml += '        <td class = "cell_desc"    style = "width: 40%; height: 20px; text-align: left"><label id="lbl_3d_branch_name"></label></td>';
        ihtml += '    </tr>';
        ihtml += '    <tr>';
        ihtml += '        <td class = "cell_caption" style = "width: 10%; height: 20px">TARGET CURRENCY:</td>';
        ihtml += '        <td class = "cell_desc"    style = "width: 40%; height: 20px; text-align: left"><select id="ddltargetccy"></select></td>';
        ihtml += '    </tr>';
        ihtml += '    <tr>';
        ihtml += '        <td colspan="2" style="height: 23px; text-align:center"><input id="btn_3D_branch_ok" style="width: 100px; height: 25px" type="button" value="OK" />&nbsp;<input id="btn_3d_branch_cancel" style="width: 100px; height: 25px" type="button" value="Cancel" /></td>';
        ihtml += '    </tr>';
        ihtml += '</table>';
    
        document.getElementById("content_mod_div").innerHTML = ihtml;

        document.getElementById("btn_3D_branch_ok").onclick = function()
        {
            global_screens.currency  = document.getElementById('ddltargetccy').value;
            global_screens.br_seq_id = global_screens.temp_br_seq_id;
            global_screens.close_modal_window();

            eval("javascript:push_apas_batch_result();");
        }

        document.getElementById("btn_3d_branch_cancel").onclick = function()
        {
            global_screens.close_modal_window();
        }        

        global_screens.branch_viewer('ajx_loading',"2");
        global_screens.get_target_currency();
        global_screens.reload_record();
    }

    this.construct_apas_online_list = function()
    {
        var ihtml = "";
    
        ihtml = '<table cellpadding = "0"; cellspacing = "1" style = "width:700px;border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid">';
        ihtml += '    <tr>';
        ihtml += '        <td class = "cell_caption" style = "width: 12%; height: 23px">TARGET CURRENCY :</td>';
        ihtml += '        <td class = "cell_desc"    style = "width: 20%; height: 23px; text-align: left"><select id="ddltargetccy"></select></td>';
        ihtml += '    </tr>';
        ihtml += '    <tr>';
        ihtml += '        <td colspan="2" style="height: 23px; text-align: center"><input id="btn_3D_branch_ok" style="width: 100px; height: 25px" type="button" value="OK" />&nbsp;<input id="btn_3d_branch_cancel" style="width: 100px; height: 25px" type="button" value="Cancel" /></td>';
        ihtml += '    </tr>';
        ihtml += '</table>';    

        document.getElementById("content_mod_div").innerHTML = ihtml;
                
        document.getElementById("btn_3D_branch_ok").onclick = function()
        {
            global_screens.currency = document.getElementById('ddltargetccy').value;
            global_screens.close_modal_window();

            eval("javascript:push_apas_online_result();");
        }
        
        document.getElementById("btn_3d_branch_cancel").onclick = function()
        {
            global_screens.close_modal_window();
        }

        global_screens.get_target_currency();
    }
    
    this.construct_paw_list = function()
    {
        var ihtml = "";
        
        ihtml = '<table cellpadding = "0"; cellspacing = "1" style = "width:700px;border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid">';
        ihtml += '    <tr>';
        ihtml += '        <td class = "cell_desc" colspan="2" style="height: 23px"><center><label id="lbl_3d_note"></label></center></td>';
        ihtml += '    </tr>';
        ihtml += '    <tr>';
        ihtml += '        <td class = "cell_caption" style = "width: 10%; height: 20px">FIND:</td>';
        ihtml += '        <td class = "cell_desc"    style = "width: 20%; height: 23px; text-align: left"><input id = "Text2" onkeyup = "javascript:execute_ajaxgridview_command(\'ajx_search$\')" type = "text" /></td>';
        ihtml += '    </tr>';
        ihtml += '    <tr>';
        ihtml += '        <td colspan="2" style="height: 23px"><div id="div_3d_non_osa"></div></td>';
        ihtml += '    </tr>';
        ihtml += '    <tr>';
        ihtml += '        <td class = "cell_caption" style = "width: 10%; height: 20px">BRANCH CODE:</td>';
        ihtml += '        <td class = "cell_desc"    style = "width: 40%; height: 20px; text-align: left" ><label id="lbl_3d_branch_code"></label></td>';
        ihtml += '    </tr>';
        ihtml += '    <tr>';
        ihtml += '        <td class = "cell_caption" style = "width: 10%; height: 20px">BRANCH NAME:</td>';
        ihtml += '        <td class = "cell_desc"    style = "width: 40%; height: 20px; text-align: left"><label id="lbl_3d_branch_name"></label></td>';
        ihtml += '    </tr>';
        ihtml += '    <tr>';
        ihtml += '        <td class = "cell_caption" style = "width: 10%; height: 20px">TARGET CURRENCY:</td>';
        ihtml += '        <td class = "cell_desc"    style = "width: 40%; height: 20px; text-align: left"><select id="ddltargetccy"></select></td>';
        ihtml += '    </tr>';
        ihtml += '    <tr>';
        ihtml += '        <td colspan="2" style="height: 23px; text-align:center"><input id="btn_3D_branch_ok" style="width: 100px; height: 25px" type="button" value="OK" />&nbsp;<input id="btn_3d_branch_cancel" style="width: 100px; height: 25px" type="button" value="Cancel" /></td>';
        ihtml += '    </tr>';
        ihtml += '</table>';
    
        document.getElementById("content_mod_div").innerHTML = ihtml;
                
        document.getElementById("btn_3D_branch_ok").onclick = function()
        {
            global_screens.currency  = document.getElementById('ddltargetccy').value;
            global_screens.br_seq_id = global_screens.temp_br_seq_id;
            global_screens.close_modal_window();

            eval("javascript:push_pay_any_where_result();");
        }
        
        document.getElementById("btn_3d_branch_cancel").onclick = function()
        {
            global_screens.close_modal_window();
        }

        global_screens.branch_viewer('ajx_loading',"3");
        global_screens.get_target_currency();
        global_screens.reload_record();
    }
 
    this.construct_bulk_payment_order = function()
    {
        var ihtml = "";
        
        ihtml = '<table cellpadding = "0"; cellspacing = "1" style = "width:700px;border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid">';
        ihtml += '    <tr>';
        ihtml += '        <td class = "cell_desc" colspan="2" style="height: 23px"><center><label id="lbl_3d_note"></label></center></td>';
        ihtml += '    </tr>';
        ihtml += '    <tr>';
        ihtml += '        <td class = "cell_caption" style = "width: 10%; height: 20px">FIND:</td>';
        ihtml += '        <td class = "cell_desc     style = "width: 20%; height: 23px; text-align: left"><input id = "Text2" onkeyup = "javascript:execute_ajaxgridview_command(\'ajx_search$\')" type = "text" /></td>';
        ihtml += '    </tr>';
        ihtml += '    <tr>';
        ihtml += '        <td colspan = "2" style = "height: 23px"><div id="div_3d_non_osa"></div></td>';
        ihtml += '    </tr>';
        ihtml += '    <tr>';
        ihtml += '        <td class = "cell_caption" style = "width: 10%; height: 20px">BRANCH CODE:</td>';
        ihtml += '        <td class = "cell_desc"    style = "width: 40%; height: 20px; text-align: left" ><label id="lbl_3d_branch_code"></label></td>';
        ihtml += '    </tr>';
        ihtml += '    <tr>';
        ihtml += '        <td class = "cell_caption" style = "width: 10%; height: 20px">BRANCH NAME:</td>';
        ihtml += '        <td class = "cell_desc"    style = "width: 40%; height: 20px; text-align: left"><label id="lbl_3d_branch_name"></label></td>';
        ihtml += '    </tr>';
        ihtml += '    <tr>';
        ihtml += '        <td class = "cell_caption" style = "width: 10%; height: 20px">TARGET CURRENCY:</td>';
        ihtml += '        <td class = "cell_desc"    style = "width: 40%; height: 20px; text-align: left"><select id="ddltargetccy"></select></td>';
        ihtml += '    </tr>';
        
        if (parseInt(global_screens.product_code) == 2 || parseInt(global_screens.product_code) == 5 || parseInt(global_screens.product_code) == 6)
        {
            if (parseInt(global_screens.implementation_code) == 1)
            {
                ihtml += '    <tr>';
                ihtml += '        <td class = "cell_caption" style = "width: 10%; height: 20px">ACCOUNT NUMBER:</td>';
                ihtml += '        <td class = "cell_desc"    style = "width: 40%; height: 20px; text-align: left"><input id = "txt_bpo_account" type = "text" />';
                ihtml += '    </tr>';
            }
        }

        ihtml += '    <tr>';
        ihtml += '        <td colspan = "2" style = "height: 23px; text-align:center"><input id="btn_3D_branch_ok" style="width: 100px; height: 25px" type="button" value="OK" />&nbsp;<input id="btn_3d_branch_cancel" style="width: 100px; height: 25px" type="button" value="Cancel" /></td>';
        ihtml += '    </tr>';
        ihtml += '</table>';
            
        document.getElementById("content_mod_div").innerHTML = ihtml;
        
        if (parseInt(global_screens.product_code) == 2 || parseInt(global_screens.product_code) == 5 || parseInt(global_screens.product_code) == 6)
        {
            if (parseInt(global_screens.implementation_code) == 1)
            {
                if (global_screens.account_number != null && global_screens.account_number != '')
                {
                    document.getElementById('txt_bpo_account').value = global_screens.account_number
                }
            }
        }
                
        document.getElementById("btn_3D_branch_ok").onclick = function()
        {
            global_screens.currency  = document.getElementById('ddltargetccy').value;
            global_screens.br_seq_id = global_screens.temp_br_seq_id;
            
            if (parseInt(global_screens.product_code) == 2 || parseInt(global_screens.product_code) == 5 || parseInt(global_screens.product_code) == 6)
            {
                if (parseInt(global_screens.implementation_code) == 1)
                {                                                    
                    var account_nos = document.getElementById('txt_bpo_account').value;
                    
                    if (global_screens.br_seq_id == null || global_screens.br_seq_id == '' || parseInt(global_screens.br_seq_id) == 0)
                    {
                        alert("Branch code is required."); return;
                    }
                    
                    if (global_screens.currency == null || global_screens.currency == '' || parseInt(global_screens.currency) == 0)
                    {
                        alert("Account currency is required."); return;
                    }

                    if (account_nos == null || account_nos == '' || account_nos.length == 0)
                    {
                        alert("Account number is required."); return;
                    }

                    if (account_nos != null && account_nos != '' && account_nos.length > 0)
                    {            
                        global_screens.check_bpo_account_details(account_nos); return;
                    }
                }
            }
            
            global_screens.close_modal_window(); eval("javascript:push_bulk_payment_order_result();");
        }
        
        document.getElementById("btn_3d_branch_cancel").onclick = function()
        {
            global_screens.close_modal_window();
        }

        global_screens.branch_viewer('ajx_loading',"2");
        global_screens.get_target_currency();
        global_screens.reload_record();
    }
 
    this.check_serviceable_location = function(cifid)
    {
        AjaxPro.timeoutPeriod = 1000 * 1000;
        
        AjaxPro.onTimeout = function()
        {
            alert("Server not responding. Checking beneficiary zipcode failed.");
        }    
    
        IRSWorld.TranClass.RemTranExtra.get_cif_zipcode(cifid, this.check_serviceable_location_callback);  
    } 
    
    this.check_serviceable_location_callback = function(response)
    {
        if(response.error != null)
        {
            alert(response.error); 
        }
        else
        {
            var ds      = response.value;
            var zipcode = null;
            var address = null;
            var note    = null;

            if (ds!= null)
            {
                if (ds.Tables[0].Rows.length > 0)
                {
                   zipcode = ds.Tables[0].Rows[0].truezipcode;
                   address = ds.Tables[0].Rows[0].Address;
                }
            }
            
            global_screens.open_modal_window();
            
            if (zipcode != null)
            {   
                global_screens.construct_serviceable_location();

                setText("lbl_3d_address", address);
                setText("lbl_3d_zipcode", zipcode);
                setText("lbl_3d_address", address);
                setText("lbl_3d_zipcode", zipcode);

            }
            else
            {
                note = "ZIP CODE NOT SPECIFIED IN BENEFICIARY ADDRESS. YOU MAY SEND TO ANY NEAREST PNB BRANCH.";                

                global_screens.construct_door_branch_currency_list();
                global_screens.branch_viewer('ajx_loading',"2");                                                
                setText("lbl_3d_note", note);
            }

            global_screens.get_target_currency();
        }
    }
     
    this.get_target_currency = function()
    {    
        AjaxPro.timeoutPeriod = 1000 * 1000;
        
        AjaxPro.onTimeout = function()
        {
            alert("Server not responding. Loading target currency failed.");
        }    
    
        IRSWorld.TranClass.RemTranExtra.get_targetcurrency(this.get_target_currency_callback);
    }

    this.get_target_currency_callback = function(response)
    {
        if(response.error != null)
        {
            alert(response.error);
        }
        else
        {
            var commons = new common_method(); 

            commons.to_load_dropdownlist('ddltargetccy',null, 'Fldvalue', 'Fldcode', response.value.Tables[0]);
            
            if (document.getElementById("ddltargetccy") != null)
            {
                if (global_screens.currency != null && parseInt(global_screens.currency) > 0)
                {
                    commons.to_set_dropdownlist_index("ddltargetccy", global_screens.currency);
                }
            }

            commons = null;
        }
    }

    this.do_ajaxgridview = function(parameter)
    {
        if (parameter != null && parameter != "undefined")
        {
            var command_param = parameter.split("$");

            if (command_param.length > 0)
            {
               if (command_param[0] == "ajx_selected")
               {
                    var param = command_param[1].split("~");

                    if (param != null)
                    {
                        this.populate_info(param[0]); return;
                    }
               }
               else if (command_param[0] == "ajx_search")
               {
                    this.branch_viewer(parameter + document.getElementById('Text2').value); 
               }
               else
               {
                    this.branch_viewer(parameter);
               }
            }
        }
    }
      
    this.populate_info = function(param)
    {
        global_screens.temp_br_seq_id = param; 
        
        AjaxPro.timeoutPeriod = 1000 * 1000;
        
        AjaxPro.onTimeout = function()
        {
            alert("Server not responding. Loading branch name failed.");
        }        
        
        IRSWorld.TranClass.RemTranExtra.get_branch_info(param, this.populate_info_callback);
    }
   
    this.populate_info_callback = function(response)
    {
        if (response.error != null)
        {
            alert("Can not connect to database.");
        }
        else
        {
            var dt = response.value;

            if (dt != null && dt.Rows.length > 0)
            {
                setText("lbl_3d_branch_name", dt.Rows[0].BranchName.toString());
                setText("lbl_3d_branch_code", dt.Rows[0].BranchCode.toString());
                setText("lbl_3d_branch_name", dt.Rows[0].BranchName.toString());
                setText("lbl_3d_branch_code", dt.Rows[0].BranchCode.toString());
            }
        }
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
	
	this.construct_div_content = function()
	{
	    var content_div = null;
	    
		content_div                       = document.createElement("div");		
		content_div.id                    = "content_mod_div";
		content_div.align                 = "left";
		content_div.style.background      = "transparent";
		content_div.style.width           = "100%";
		content_div.innerHTML             = "<center><img src = '.././images/ajaxloader.gif'></img></center>";	    

		this.inner_modal_div              = document.createElement("div");		
		this.inner_modal_div.align        = "center";
		this.inner_modal_div.className    = "inner_div";		
		this.inner_modal_div.id           = "inner_mod_div";
		this.inner_modal_div.style.height = "auto";
		this.inner_modal_div.style.width  = "700px";
		this.inner_modal_div.style.top    = "30px";
		this.inner_modal_div.style.left   = (parseInt(screen.width) - parseInt(this.inner_modal_div.style.width)) / 2 - 10 + "px";
		this.inner_modal_div.style.border = "medium solid white";
		this.inner_modal_div.Code         = this;
		this.inner_modal_div.appendChild(content_div);
		
		document.body.appendChild(this.inner_modal_div);
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
	
	this.load_modal_css = function()
	{	    
		var file_reference = document.createElement("link");
		
	  	file_reference.setAttribute("rel", "stylesheet");
	  	file_reference.setAttribute("type", "text/css");
	  	file_reference.setAttribute("href", ".././css/cssmodals.css");

		if (typeof file_reference != "undefined")
		{		    
	  		document.getElementsByTagName("head")[0].appendChild(file_reference);
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
	        if (divelement_outer.item(i).id == "outer_mod_div")
	        {
	            divelement_outer.item(i).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop;break;
	        }	        
	    }
	    for(m = 0; m < divelement_inner.length;m++)
	    {
	        if (divelement_inner.item(m).id == "inner_mod_div")
	        {
	            divelement_outer.item(m).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop + 20 : document.body.scrollTop + 20;break;
	        }
	    }
	}
}
/* END 3D APAS BATCH APAS ONLINE PAW BPO SCREEN */


/* 9.) REMITTER REPRESENTATIVE */
function remitter_proxy()
{
    var iscreen = this;
    
    this.outer_modal_div    = null;
    this.inner_modal_div    = null;
    this.commons            = null;
    
    this.proxy_lastname     = '';
    this.proxy_firstname    = '';
    this.proxy_middlename   = '';
    this.proxy_present_addr = '';
    
    this.remitter_id        = null;
    this.transaction_seq_id = 0;

    this.open_remitter_proxy = function()
    {
        this.open_modal_window(); 
        this.construct_remitter_proxy_screen();
        this.load_remitters_proxy_grid("ajx_loading");
    }

    this.load_remitters_proxy_grid = function(param)
    {
        var grid = new grid_viewer();                               

        grid.sql_source     = "sp_otc_rem_representatives~" + this.remitter_id; 
        grid.div_container  = document.getElementById("div_proxy_remitters");
        grid.datarow_column = 0;
        grid.page_size      = 5;
        grid.process_ajax(param);                               
        grid = null;    
    }
    
    this.do_ajaxgridview = function(parameter)
    {
        if (parameter != null && typeof parameter != "undefined")
        {
            var command_param = parameter.split("$");

            if (command_param.length > 0)
            {
                if (command_param[0] == "ajx_selected")
                {
                    var param = command_param[1].split("~"); 
                    
                    if (param != null && param.length > 0)
                    {
                        var ihit = confirm("Selected representative will be mark on this transaction. Do you want to continue?");

                        if (ihit == true)
                        {
                            document.getElementById("txt_proxy_lastname").value        = param[0].toString().replace(/^\s+|\s+$/g,"");
                            document.getElementById("txt_proxy_firstname").value       = param[1].toString().replace(/^\s+|\s+$/g,"");
                            document.getElementById("txt_proxy_middlename").value      = param[2].toString().replace(/^\s+|\s+$/g,"");
                            document.getElementById("txt_proxy_present_address").value = param[3].toString().replace(/^\s+|\s+$/g,"");

                            this.proxy_lastname     = document.getElementById("txt_proxy_lastname").value;
                            this.proxy_firstname    = document.getElementById("txt_proxy_firstname").value;
                            this.proxy_middlename   = document.getElementById("txt_proxy_middlename").value;
                            this.proxy_present_addr = document.getElementById("txt_proxy_present_address").value;
                        }
                    }
                    return;
                }
                this.load_remitters_proxy_grid(parameter);
            }
        }
    }
    
    this.commit_remitters_proxy = function(transaction_sequence_id)
    {
        var commit_data = "";
        
        commit_data = "LastName~"         + this.proxy_lastname;
        commit_data += "|FirstName~"      + this.proxy_firstname;
        commit_data += "|MiddleName~"     + this.proxy_middlename;
        commit_data += "|PresentAddress~" + this.proxy_present_addr;
        commit_data += "|TXN_SEQ_ID~"     + transaction_sequence_id;
        
        AjaxPro.timeoutPeriod = 1000 * 1000;
        
        AjaxPro.onTimeout = function()
        {
            alert("Server not responding. Commit remitter representative failed.");
        }        

        IRSWorld.TranClass.RemTranExtra.insert_remitter_representative(commit_data, this.commit_remitters_proxy_callback);
    }
    
    this.commit_remitters_proxy_callback = function(result)
    {
        var s = null;

        if (result != null && typeof result != "undefined" && result.error == null)
        {
            s = result.value;
        }
        else
        {
            s = '0~' + result.error;
        }

        eval("javascript:push_rem_proxy_commit_result(s);"); return;
    }

    this.construct_remitter_proxy_screen = function()
    {
        var ihtml = "";

        ihtml = '<center>'
        ihtml += '<table cellpadding = "0"; cellspacing = "1" style = "width:700px;border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid">';
        ihtml += '    <tr>';
        ihtml += '        <td class = "cell_desc" colspan="2" style="text-align: center"><label>SELECT REMITTERS REPRESENTATIVE ON THE LIST OR FILL OUT THE FIELDS BELOW.</label></td>';
        ihtml += '    </tr>';
        ihtml += '    <tr>';
        ihtml += '        <td colspan="2" style="text-align: center" ><div id = "div_proxy_remitters"></div></td>';
        ihtml += '    </tr>';
        ihtml += '   <tr>';
        ihtml += '       <td class = "cell_caption" style = "width: 20%;  height: 20px">LAST NAME :</td>';
        ihtml += '       <td class = "cell_desc"    style = "width: 60%; height: 20px; text-align: left"><input id = "txt_proxy_lastname" type = "text"  style = "width:auto"/></label></td>';
        ihtml += '   </tr>';
        ihtml += '   <tr>';
        ihtml += '       <td class = "cell_caption" style = "width: 20%;  height: 20px">FIRST NAME :</td>';
        ihtml += '       <td class = "cell_desc"    style = "width: 60%; height: 20px; text-align: left"><input id = "txt_proxy_firstname" type = "text" style = "width:auto" /></label></td>';
        ihtml += '   </tr>';
        ihtml += '   <tr>';
        ihtml += '       <td class = "cell_caption" style = "width: 20%;  height: 20px">MIDDLE NAME :</td>';
        ihtml += '       <td class = "cell_desc"    style = "width: 60%; height: 20px; text-align: left"><input id = "txt_proxy_middlename" type = "text" style = "width:auto" /></td>';
        ihtml += '   </tr>';
        ihtml += '   <tr>';
        ihtml += '       <td class = "cell_caption" style = "width: 20%;  height: 20px">PRESENT ADDRESS :</td>';
        ihtml += '       <td class = "cell_desc"    style = "width: 60px; height: 20px; text-align: left"><input id = "txt_proxy_present_address" type = "text" style = "width:400px"  /></td>';
        ihtml += '   </tr>';
        ihtml += '   <tr>';
        ihtml += '       <td colspan="2" style="text-align: center"><input id = "btn_remitter_proxy_ok" type = "button" value = "OK" style="width: 100px; height: 25px" />&nbsp;<input id = "btn_remitter_proxy_cancel" type = "button" value = "CANCEL" style="width: 100px; height: 25px" /></td>';        
        ihtml += '   </tr>';
        ihtml += '</table>';
        ihtml += '</center>'
        
        document.getElementById("content_mod_div").innerHTML = ihtml;
        document.getElementById("txt_proxy_lastname").value        = iscreen.proxy_lastname.toString().replace(/^\s+|\s+$/g,"");
        document.getElementById("txt_proxy_firstname").value       = iscreen.proxy_firstname.toString().replace(/^\s+|\s+$/g,"");
        document.getElementById("txt_proxy_middlename").value      = iscreen.proxy_middlename.toString().replace(/^\s+|\s+$/g,"");
        document.getElementById("txt_proxy_present_address").value = iscreen.proxy_present_addr.toString().replace(/^\s+|\s+$/g,"");
                
        document.getElementById("btn_remitter_proxy_ok").onclick = function()
        {
            iscreen.proxy_lastname     = document.getElementById("txt_proxy_lastname").value.toString().replace(/^\s+|\s+$/g,"");
            iscreen.proxy_firstname    = document.getElementById("txt_proxy_firstname").value.toString().replace(/^\s+|\s+$/g,"");
            iscreen.proxy_middlename   = document.getElementById("txt_proxy_middlename").value.toString().replace(/^\s+|\s+$/g,"");
            iscreen.proxy_present_addr = document.getElementById("txt_proxy_present_address").value.toString().replace(/^\s+|\s+$/g,"");
            
            if (iscreen.proxy_lastname.length < 1 || iscreen.proxy_firstname.length < 1 || iscreen.proxy_middlename.length < 1 || iscreen.proxy_present_addr.length < 1)
            {
                alert("Please fill out all fields.");
            }
            else
            {
                iscreen.close_modal_window(); eval("javascript:push_rem_proxy_screen_result();");
            }
        }
        
        document.getElementById("content_mod_div").document.getElementById("btn_remitter_proxy_cancel").onclick = function()
        {
            iscreen.close_modal_window(); eval("javascript:push_rem_proxy_screen_result();");
        }
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
	
	this.construct_div_content = function()
	{
	    var content_div = null;

		content_div                       = document.createElement("div");
		content_div.id                    = "content_mod_div";
		content_div.align                 = "left";
		content_div.style.background      = "transparent";
		content_div.style.width           = "100%";
		content_div.innerHTML             = "<center><img src = '.././images/ajaxloader.gif'></img></center>";

		this.inner_modal_div              = document.createElement("div");		
		this.inner_modal_div.align        = "center";
		this.inner_modal_div.className    = "inner_div";		
		this.inner_modal_div.id           = "inner_mod_div";
		this.inner_modal_div.style.height = "auto";
		this.inner_modal_div.style.width  = "700px";
		this.inner_modal_div.style.top    = "30px";
		this.inner_modal_div.style.left   = (parseInt(screen.width) - parseInt(this.inner_modal_div.style.width)) / 2 - 10 + "px";
		this.inner_modal_div.style.border = "medium solid white";
		this.inner_modal_div.Code         = this;
		this.inner_modal_div.appendChild(content_div);
		
		document.body.appendChild(this.inner_modal_div);
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
	
	this.load_modal_css = function()
	{	    
		var file_reference = document.createElement("link");
		
	  	file_reference.setAttribute("rel", "stylesheet");
	  	file_reference.setAttribute("type", "text/css");
	  	file_reference.setAttribute("href", ".././css/cssmodals.css");

		if (typeof file_reference != "undefined")
		{		    
	  		document.getElementsByTagName("head")[0].appendChild(file_reference);
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
	        if (divelement_outer.item(i).id == "outer_mod_div")
	        {
	            divelement_outer.item(i).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop;break;
	        }
	    }

	    for(m = 0; m < divelement_inner.length;m++)
	    {
	        if (divelement_inner.item(m).id == "inner_mod_div")
	        {
	            divelement_outer.item(m).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop + 20 : document.body.scrollTop + 20;break;
	        }
	    }
	}
}
/* END REMITTER REPRESENTATIVE */

/* 10.) REMITTANCE EXPCEPTIONS */
function rem_exceptions()
{
    var iexceptions = this;

    this.outer_modal_div = null;
    this.inner_modal_div = null;
    this.commons         = null;
    this.remitter_id     = null;

    this.open_remitter_exceptions = function()
    {
        this.open_modal_window();
        this.construct_rem_exceptions();
    }

    this.construct_rem_exceptions = function()
    {
        var ihtml = "";
        
        ihtml = '<table style="width: 700px; border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid">';
        ihtml += '    <tr>';
        ihtml += '        <td class = "table_caption" colspan = "2" style = "height: 20px; text-align: center">EXCEPTION LIST</td>';
        ihtml += '    </tr>';
        ihtml += '    <tr>';
        ihtml += '        <td class = "cell_caption" style = "width: 10%; height: 20px">REMITTER ID :</td>';
        ihtml += '        <td class = "cell_desc"    style = "width: 60%; height: 20px; text-align: left"><label id = "lbl_rem_exception_remitter_id"></label></td>';
        ihtml += '    </tr>';
        ihtml += '    <tr>';
        ihtml += '        <td class = "cell_caption" style = "width: 10%; height: 20px">LAST NAME :</td>';
        ihtml += '        <td class = "cell_desc"    style = "width: 60%; height: 20px; text-align: left"><label id = "lbl_rem_exception_last_name"></label></td>';
        ihtml += '    </tr>';
        ihtml += '    <tr>';
        ihtml += '        <td class = "cell_caption" style = "width: 10%; height: 20px">FIRST NAME :</td>';
        ihtml += '        <td class = "cell_desc"    style = "width: 60%; height: 20px; text-align: left"><label id = "lbl_rem_exception_first_name"></label></td>';
        ihtml += '    </tr>';
        ihtml += '    <tr>';
        ihtml += '        <td class = "cell_caption" style = "width: 10%; height: 20px">MIDDLE NAME :</td>';
        ihtml += '        <td class = "cell_desc"    style = "width: 60%; height: 20px; text-align: left"><label id = "lbl_rem_exception_middle_name"></label></td>';
        ihtml += '    </tr>';
        ihtml += '    <tr>';
        ihtml += '        <td class = "cell_caption" style = "width: 10%; height: 20px">REASON FOR EXCEPTION :</td>';
        ihtml += '        <td class = "cell_desc"    style = "width: 60%; height: 20px; text-align: left"><label id = "lbl_rem_exception_reason_for_exception"></label></td>';
        ihtml += '    </tr>';
        ihtml += '    <tr>';
        ihtml += '        <td class = "cell_caption" style = "width: 10%; height: 20px">REASON FOR CANCELLATION :</td>';
        ihtml += '        <td class = "cell_desc"    style = "width: 60%; height: 20px; text-align: left"><label id = "lbl_rem_exception_reason_for_cancellation"></label></td>';
        ihtml += '    </tr>';
        ihtml += '    <tr>';
        ihtml += '        <td class = "cell_caption" style = "width: 20%; height: 20px">USER NAME :</td>';
        ihtml += '        <td class = "cell_desc"    style = "width: 20%; height: 20px; text-align: left"><input id="txt_rem_exception_username" type="text" /></td>';
        ihtml += '    </tr>';
        ihtml += '    <tr>';
        ihtml += '        <td class = "cell_caption" style = "width: 20%; height: 20px">PASSWORD :</td>';
        ihtml += '        <td class = "cell_desc"    style = "width: 20%; height: 20px; text-align: left"><input id="txt_rem_exception_password" type="password" /></td>';
        ihtml += '    </tr>';
        ihtml += '    <tr>';
        ihtml += '        <td colspan = "2" style="height: 23px; text-align: center">';
        ihtml += '            <input id = "btn_rem_exception_continue" style="width: 100px; height: 25px" type="button" value="CONTINUE" />&nbsp;<input id="btn_rem_exception_revoke" style="width: 100px; height: 25px" type="button" value="REVOKE" />';
        ihtml += '        </td>';
        ihtml += '    </tr>';
        ihtml += '</table>';

        document.getElementById("content_mod_div").innerHTML = ihtml; iexceptions.load_exception_list();

        document.getElementById("btn_rem_exception_continue").onclick = function()
        {
            iexceptions.exception_login();
        }

        document.getElementById("btn_rem_exception_revoke").onclick = function()
        {
            var irevoke = confirm("Do you want to revoke this transaction?");
            
            if (irevoke == true)
            {
                iexceptions.close_modal_window(); eval("javascript:push_exception_screen_result(0);");
            }
        }
    }
    
    this.load_exception_list = function()
    {
        AjaxPro.timeoutPeriod = 1000 * 1000;
        
        AjaxPro.onTimeout = function()
        {
            alert("Server not responding. Loading remitter exception list failed.");
        }    
    
        IRSWorld.TranClass.RemTranExtra.remitter_exception_list(this.remitter_id, this.load_exception_list_callback);
    }
    
    this.load_exception_list_callback = function(result)
    {
        if (result != null && result.value != null)
        {
            ds = result.value;

            setText("lbl_rem_exception_remitter_id",            ds.Tables[0].Rows[0].CIFID);
            setText("lbl_rem_exception_last_name",              ds.Tables[0].Rows[0].LastName);
            setText("lbl_rem_exception_first_name",             ds.Tables[0].Rows[0].FirstName);
            setText("lbl_rem_exception_middle_name",            ds.Tables[0].Rows[0].MiddleName);
            setText("lbl_rem_exception_reason_for_exception",   ds.Tables[0].Rows[0].Ereason);
            setText("lbl_rem_exception_reason_for_cancellation",ds.Tables[0].Rows[0].CReason);

        }
    }

    this.exception_login = function()
    {
        var user_name = document.getElementById("txt_rem_exception_username").value;
        var user_pwd  = document.getElementById("txt_rem_exception_password").value;

        AjaxPro.timeoutPeriod = 1000 * 1000;
        
        AjaxPro.onTimeout = function()
        {
            alert("Server not responding. Checking user rights to allow remitter exceptions failed.");
        }

        IRSWorld.TranClass.RemTranExtra.user_can_log_exceptions(user_name, user_pwd, this.exception_login_callback);
    }
    
    this.exception_login_callback = function(result)
    {
        if (result != null && result.value != null)
        {
            if (result.value[0] == false)
            {
                alert(result.value[1]); return;
            }
            else
            {
                iexceptions.close_modal_window(); eval("javascript:push_exception_screen_result(1);"); return;
            }
        }
     
        iexceptions.close_modal_window(); eval("javascript:push_exception_screen_result(0);");
        
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
	
	this.construct_div_content = function()
	{
	    var content_div = null;

		content_div                       = document.createElement("div");		
		content_div.id                    = "content_mod_div";
		content_div.align                 = "left";
		content_div.style.background      = "transparent";
		content_div.style.width           = "100%";
		content_div.innerHTML             = "<center><img src = '.././images/ajaxloader.gif'></img></center>";

		this.inner_modal_div              = document.createElement("div");		
		this.inner_modal_div.align        = "center";
		this.inner_modal_div.className    = "inner_div";		
		this.inner_modal_div.id           = "inner_mod_div";
		this.inner_modal_div.style.height = "auto";
		this.inner_modal_div.style.width  = "700px";
		this.inner_modal_div.style.top    = "30px";
		this.inner_modal_div.style.left   = (parseInt(screen.width) - parseInt(this.inner_modal_div.style.width)) / 2 - 10 + "px";
		this.inner_modal_div.style.border = "medium solid white";
		this.inner_modal_div.Code         = this;
		this.inner_modal_div.appendChild(content_div);
		
		document.body.appendChild(this.inner_modal_div);
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
	
	this.load_modal_css = function()
	{	    
		var file_reference = document.createElement("link");
		
	  	file_reference.setAttribute("rel", "stylesheet");
	  	file_reference.setAttribute("type", "text/css");
	  	file_reference.setAttribute("href", ".././css/cssmodals.css");

		if (typeof file_reference != "undefined")
		{		    
	  		document.getElementsByTagName("head")[0].appendChild(file_reference);
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
	        if (divelement_outer.item(i).id == "outer_mod_div")
	        {
	            divelement_outer.item(i).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop;break;
	        }
	    }

	    for(m = 0; m < divelement_inner.length;m++)
	    {
	        if (divelement_inner.item(m).id == "inner_mod_div")
	        {
	            divelement_outer.item(m).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop + 20 : document.body.scrollTop + 20;break;
	        }
	    }
	}
}
/* END REMITTANCE EXPCEPTIONS */

/* 11.) REMITTANCE AGGREGATES */
function rem_aggregates()
{
    var irem_aggr = this;

    this.grd_source         = null;
    this.grd_aggregates     = null;
    this.branch_code        = null;
    this.branch_date        = null;
    this.remitting_currency = null;
    this.outer_modal_div    = null;
    this.inner_modal_div    = null;
    this.commons            = null;
    this.remitter_id        = null;
    this.isCommit           = null;

    this.open_remitter_aggregates = function(remitter_id, remitting_currency)
    {
        this.remitter_id = remitter_id;
        this.remitting_currency = remitting_currency;
        this.open_modal_window(); 
        this.construct_aggregate_screen();
    }

    this.process_ajax = function(param)
    {
        var grid = new grid_viewer();                               

        grid.sql_source     = "sp_rem_otc_remitter_aggregate2~" + irem_aggr.branch_code + "~" + irem_aggr.branch_date + "~" + irem_aggr.remitting_currency + "~" + irem_aggr.remitter_id;
        grid.div_container  = document.getElementById("div_rem_aggregates_jeje");
        grid.datarow_column = 0;
        grid.page_size      = 10;
        grid.process_ajax(param);                               
        grid = null;    
    }
    
    this.logged_setup = function()
    {
        AjaxPro.timeoutPeriod = 1000 * 1000;
        
        AjaxPro.onTimeout = function()
        {
            alert("Server not responding. Loading user logged data failed.");
        }

        IRSWorld.TranClass.RemTranExtra.get_logged_data(this.logged_setup_callback);
    }
    
    this.logged_setup_callback = function(result)
    {
        if (result != null && result.value != null)
        {
            irem_aggr.branch_code = result.value[0];
            irem_aggr.branch_date = result.value[1];
            irem_aggr.process_ajax("ajx_loading");
        }
    }
    
    this.construct_aggregate_screen = function()
    {
        var ihtml = "";
        
        ihtml = '<center><table style="width: 400px; border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid">';
        
        ihtml += '<tr>';
        ihtml += '    <td colspan="2" class="table_caption" style="height: 23px; text-align: center">REMITTANCE AGGREGATION</td>';
        ihtml += '</tr>';
        
        ihtml += '<tr>';
        ihtml += '    <td colspan="2" style="height: 23px; text-align: left"><div id = "div_rem_aggregates_jeje"><center><label class = "cell_desc">Loading, please wait...</label><img src = "../images/ajaxloader.gif"></img></center></div></td>';
        ihtml += '</tr>';
        ihtml += '<tr>';
        ihtml += '    <td colspan="2" style="height: 23px; text-align: center"><input id="div_rem_aggregates_jeje_ok" style="width: 100px; height: 25px" type="button" value="OK" /></td>';
        ihtml += '</tr>';
        ihtml += '</table></center>';

        document.getElementById("content_mod_div").innerHTML = ihtml;

        irem_aggr.logged_setup();

        document.getElementById("div_rem_aggregates_jeje_ok").onclick = function()
        {
            irem_aggr.close_modal_window();
            // Added by Hajz : 11/15/2012
            // Fot OTC Transaction Commit Button Triggered.
            // Call get_overseas_datetime command when aggregate screen closes.
            // Catch errors if called by other modules
            try
            {
                if (irem_aggr.isCommit == true)
                {
                    eval("javascript:rem_aggregate_closer_response();");
                }
            }
            catch(e){}
        }
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
	
	this.construct_div_content = function()
	{
	    var content_div = null;

		content_div                       = document.createElement("div");		
		content_div.id                    = "content_mod_div";
		content_div.align                 = "left";
		content_div.style.background      = "transparent";
		content_div.style.width           = "100%";
		content_div.innerHTML             = "<center><img src = '.././images/ajaxloader.gif'></img></center>";

		this.inner_modal_div              = document.createElement("div");		
		this.inner_modal_div.align        = "center";
		this.inner_modal_div.className    = "inner_div";		
		this.inner_modal_div.id           = "inner_mod_div";
		this.inner_modal_div.style.height = "auto";
		this.inner_modal_div.style.width  = "400px";
		this.inner_modal_div.style.top    = "30px";
		this.inner_modal_div.style.left   = (parseInt(screen.width) - parseInt(this.inner_modal_div.style.width)) / 2 - 10 + "px";
		this.inner_modal_div.style.border = "medium solid white";
		this.inner_modal_div.Code         = this;
		this.inner_modal_div.appendChild(content_div);
		
		document.body.appendChild(this.inner_modal_div);
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
	
	this.load_modal_css = function()
	{	    
		var file_reference = document.createElement("link");
		
	  	file_reference.setAttribute("rel", "stylesheet");
	  	file_reference.setAttribute("type", "text/css");
	  	file_reference.setAttribute("href", ".././css/cssmodals.css");

		if (typeof file_reference != "undefined")
		{		    
	  		document.getElementsByTagName("head")[0].appendChild(file_reference);
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
	        if (divelement_outer.item(i).id == "outer_mod_div")
	        {
	            divelement_outer.item(i).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop;break;
	        }
	    }

	    for(m = 0; m < divelement_inner.length;m++)
	    {
	        if (divelement_inner.item(m).id == "inner_mod_div")
	        {
	            divelement_outer.item(m).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop + 20 : document.body.scrollTop + 20;break;
	        }
	    }
	}
}
/* END REMITTANCE AGGREGATES */

/* 12.) FLEXCUBE CREDIT ONLINE SCREEN */
function credit_casa()
{
    var icasa = this;

    this.outer_modal_div          = null;
    this.inner_modal_div          = null;
    this.commons                  = null;
    this.transaction_sequence_id  = null;
    
    this.open_online_credit = function(transaction_sequence_id)
    {
        var ihtml = "";

        this.transaction_sequence_id = transaction_sequence_id;
        this.open_modal_window();

        ihtml = '<center>';
        ihtml += '  <table style="width: 400px; border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid">';
        ihtml += '      <tr><td colspan="2" style="height: 23px; text-align: center"><img src="../images/ajaxloader.gif" /></td></tr>';
        ihtml += '      <tr><td class = "cell_desc" colspan="2" style="height: 23px; text-align: center"><label id = "lbl_credit_casa_message">System is processing your request ...</label></td></tr>';
        ihtml += '      <tr><td colspan="2" style="height: 23px; text-align: center"></td></tr>';
        ihtml += '  </table>';
        ihtml += '<center>';

        document.getElementById("content_mod_div").innerHTML = ihtml;

        icasa.transaction_sequence_id = transaction_sequence_id;
        icasa.online_credit_response();
    }
    
    this.online_credit_response = function()
    {
        AjaxPro.timeoutPeriod = 100 * 1000;
        
        AjaxPro.onTimeout = function()
        {
            s = "0~" + result.error.Message; icasa.close_modal_window(); eval("javascript:push_online_credit_response(s);");
        }
    
        IRSWorld.TranClass.RemTranExtra.credit_remittance_online(this.transaction_sequence_id, this.online_credit_response_callback);
    }
    
    this.online_credit_response_callback = function(result)
    {
        var s = "0~Server not responding to client request.";

        if (result != null && result.error != null)
        {
            s = "0~" + result.error.Message;
        }

        if (result != null && result.error == null && result.value != null)
        {
            s = "1~" + result.value.Tables[0].Rows[0].Message    + "~" +
                       result.value.Tables[0].Rows[0].Transtatus + "~" +
                       result.value.Tables[0].Rows[0].TranStatusDesc;
        }

        icasa.close_modal_window(); eval("javascript:push_online_credit_response(s);");
        
    }

    this.open_credit_casa = function(transaction_sequence_id)
    {
        this.transaction_sequence_id = transaction_sequence_id;
        this.open_modal_window();
        this.construct_credit_casa_screen();
    }

    this.construct_credit_casa_screen = function()
    {
        var ihtml = "";

        ihtml =  '<center>';
        ihtml += '  <table style="width: 400px; border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid">';
        ihtml += '      <tr><td colspan="2" style="height: 23px; text-align: center"><img src="../images/ajaxloader.gif" /></td></tr>';
        ihtml += '      <tr><td class = "cell_desc" colspan="2" style="height: 23px; text-align: center"><label id = "lbl_credit_casa_message">System is processing your request ...</label></td></tr>';
        ihtml += '      <tr><td colspan="2" style="height: 23px; text-align: center"></td></tr>';
        ihtml += '  </table>';
        ihtml += '<center>';

        document.getElementById("content_mod_div").innerHTML = ihtml;

        icasa.credit_response();        
    }

    this.credit_response = function()
    {
        AjaxPro.timeoutPeriod = 100 * 1000;

        AjaxPro.onTimeout = function()
        {
            s = "0~" + result.error.Message; icasa.close_modal_window(); eval("javascript:push_credit_casa_response(s);");
        }
        
        IRSWorld.TranClass.RemTranExtra.credit_remittance_online(this.transaction_sequence_id, this.credit_response_callback);
    }
    
    this.credit_response_callback = function(result)
    {
        var s = "0~SERVER NOT RESPONDING TO REQUEST";
        
        setText("lbl_credit_casa_message", "RETREVING RESPONSE FROM SERVER ...");

        if (result != null && result.error != null)
        {
            s = "0~" + result.error.Message;
        }

        if (result != null && result.error == null && result.value != null)
        {
            s = "1~" + result.value.Tables[0].Rows[0].Message    + "~" +
                       result.value.Tables[0].Rows[0].Transtatus + "~" +
                       result.value.Tables[0].Rows[0].TranStatusDesc;
        }

        icasa.close_modal_window(); eval("javascript:push_credit_casa_response(s);");
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
	
	this.construct_div_content = function()
	{
	    var content_div = null;

		content_div                       = document.createElement("div");		
		content_div.id                    = "content_mod_div";
		content_div.align                 = "left";
		content_div.style.background      = "transparent";
		content_div.style.width           = "100%";
		content_div.innerHTML             = "<center><img src = '.././images/ajaxloader.gif'></img></center>";

		this.inner_modal_div              = document.createElement("div");		
		this.inner_modal_div.align        = "center";
		this.inner_modal_div.className    = "inner_div";		
		this.inner_modal_div.id           = "inner_mod_div";
		this.inner_modal_div.style.height = "auto";
		this.inner_modal_div.style.width  = "400px";
		this.inner_modal_div.style.top    = "30px";
		this.inner_modal_div.style.left   = (parseInt(screen.width) - parseInt(this.inner_modal_div.style.width)) / 2 - 10 + "px";
		this.inner_modal_div.style.border = "medium solid white";
		this.inner_modal_div.Code         = this;
		this.inner_modal_div.appendChild(content_div);
		
		document.body.appendChild(this.inner_modal_div);
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
	
	this.load_modal_css = function()
	{	    
		var file_reference = document.createElement("link");
		
	  	file_reference.setAttribute("rel", "stylesheet");
	  	file_reference.setAttribute("type", "text/css");
	  	file_reference.setAttribute("href", ".././css/cssmodals.css");

		if (typeof file_reference != "undefined")
		{		    
	  		document.getElementsByTagName("head")[0].appendChild(file_reference);
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
	        if (divelement_outer.item(i).id == "outer_mod_div")
	        {
	            divelement_outer.item(i).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop;break;
	        }
	    }

	    for(m = 0; m < divelement_inner.length;m++)
	    {
	        if (divelement_inner.item(m).id == "inner_mod_div")
	        {
	            divelement_outer.item(m).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop + 20 : document.body.scrollTop + 20;break;
	        }
	    }
	}
}
/* END FLEXCUBE CREDIT ONLINE SCREEN */

/* 13.) REMITTANCE AMOUNT LIMIT SCREEN */
function remittance_amt_limits()
{
    var irem_amt_limit = this;

    this.outer_modal_div              = null;
    this.inner_modal_div              = null;
    this.commons                      = null;
    this.beneficiary_id               = null;
    this.beneficiary_account_number   = null;
    this.beneficiary_account_currency = null;
    this.remittance_amount_limit      = null;
    this.remittance_teller_limit      = null;
    this.remittance_supervisor_limit  = null;
    this.remittance_aggregate_tag     = null;
    this.remitter_id                  = null;
    this.remitting_currency           = null;    
    this.gross_amount                 = null;
    this.net_amount                   = null;    
    this.product_code                 = null;    
    this.implementation_type          = null;
    this.iremit_object                = null;
    this.msg_screen                   = null;

    this.payment_type                 = null;
    this.cash_aggregation_enabled     = null;
    this.cash_aggregation_limit       = null;

    this.txt_otc_rem_limits_user_id       = null;
    this.txt_otc_rem_limits_user_password = null;
    this.limit_exceeds_flag = 0;
    this.pending_override_status = 0;
    this.override_type_code = 0;
    
    this.run_rmover_closer = function()
    {
        this.tran_limit_remote_ovr();
    }

    this.check_amount_limit = function(gross_amount, net_amount, iremit_object)
    {       
        if (gross_amount  == null){this.gross_amount  = 0; }
        if (net_amount    == null){this.net_amount    = 0; }
        if (iremit_object != null){this.iremit_object = iremit_object;}

        if (this.cash_aggregation_limit == null){this.cash_aggregation_limit = 0;}
        if (this.remittance_amount_limit == null){this.remittance_amount_limit = 0;}
        if (this.remittance_teller_limit == null){this.remittance_teller_limit = 0;}
        if (this.remittance_supervisor_limit == null){this.remittance_supervisor_limit = 0;}

        this.gross_amount = new common_method().to_remove_comma(gross_amount);
        this.net_amount   = new common_method().to_remove_comma(net_amount);

        //FOR CHECKING
	    if (parseInt(this.iremit_object.product_code) == 10 || parseInt(this.iremit_object.product_code) == 22)
	    {
            this.check_aggregate_gfc();
	    }
	    else
	    {
            this.check_teller_and_remitter_limit();
	    }
	    //FOR CHECKING
    }
    
    this.tran_limit_remote_ovr = function()
    {
        var array_values = "100~" + this.pending_override_status; array_values = array_values.split('~');
    
        this.close_loggin_screen();

        eval("javascript:push_remit_limit_response(array_values);");
    }

    this.check_teller_and_remitter_limit = function()
    {
        var msg = null;

        this.iremit_object.dailyrecordkeepinglog = false; //Added by Troy 20120806

        /*        
        Commented. For users disposition on how to handle transaction that above the limit of supervisor
        
        if (parseFloat(this.gross_amount) > parseFloat(this.remittance_supervisor_limit))
        {
            msg = "Transaction amount exceeds Teller and Supervisor limit."; this.override_type_code = "7";
            //msg = "Transaction amount exceeds Teller and Supervisor limit. Transaction will be for approval of Head of Office. \n\n Press OK to continue or CANCEL to discard transaction."; this.override_type_code = "7";
        }
        */

        if (parseFloat(this.gross_amount) > parseFloat(this.remittance_supervisor_limit))
        {
            msg = "Transaction amount exceeds Teller's limit. For Supervisor's override. \n\n Press OK to continue or CANCEL to discard transaction.";  this.override_type_code = "5";
        }
        else if (parseFloat(this.gross_amount) > parseFloat(this.remittance_teller_limit))
        {
            msg = "Transaction amount exceeds Teller's limit. For Supervisor's override. \n\n Press OK to continue or CANCEL to discard transaction.";  this.override_type_code = "5";
        }
        else if (parseFloat(this.gross_amount) >= parseFloat(this.remittance_amount_limit))
        {
            msg = "Transaction amount reached Required ID limit. Please ask Remitter to present valid ID."; this.override_type_code = "8";
        }

        if (parseFloat(this.gross_amount) >= parseFloat(this.remittance_amount_limit))
        {
            this.iremit_object.dailyrecordkeepinglog = true; 
        }

        if (parseInt(this.override_type_code) == 8)
        {
            alert(msg); eval("javascript:push_remit_limit_response(1);"); return;
        }

        if (msg != null)
        {
            if (this.iremit_object != null)
            {
                this.iremit_object.close_msg_prompt();
            }

            if (parseInt(this.override_type_code) == 7)
            {
                alert(msg); eval("javascript:push_remit_limit_response(0);"); return;
            }

            var iconfirm = confirm(msg);

            if (iconfirm == true)
            {
                this.open_loggin_screen(); return;
            }
            else
            {
                eval("javascript:push_remit_limit_response(0);"); return;
            }
        }

        /*  edited by : Neil 07.31.2012 */

        if (this.remittance_aggregate_tag == true | this.cash_aggregation_enabled == true)
        {
            AjaxPro.timeoutPeriod = 1000 * 1000;
        
            AjaxPro.onTimeout = function()
            {
                alert("Server not responding. Checking remitter aggregate amount per currency failed.");
            }

            IRSWorld.TranClass.RemTranExtra.get_rem_aggregate_per_currency(this.remitter_id, this.remitting_currency, this.remittance_aggregate_tag, this.cash_aggregation_enabled, this.check_aggregate_amount_callback); return;
        }

        eval("javascript:push_remit_limit_response(1);");
    }
    
    this.check_aggregate_gfc = function()
    {
        AjaxPro.timeoutPeriod = 1000 * 1000;
        
        AjaxPro.onTimeout = function()
        {
            alert("Server not responding. Checking gfc aggregate amount failed.");
        }
    
        IRSWorld.TranClass.RemTranExtra.gfc_not_reach_max_transaction(this.beneficiary_id, this.beneficiary_account_number, parseInt(this.beneficiary_account_currency), parseFloat(this.net_amount) , this.check_aggregate_gfc_callback);
    }

    this.check_aggregate_gfc_callback = function(result)
    {
        if (result != null)
        {
            if (result.error != null && result.error != "")
            {
                alert("System encountered error while checking gfc aggregate amount."); irem_amt_limit.check_teller_and_remitter_limit(); return;
            }
        
            if (result.value == true)
            {
                irem_amt_limit.check_teller_and_remitter_limit(); return;
            }
            else
            {
                eval("javascript:push_remit_limit_response(2);"); return; /* GFC REACHED MAX TRANSACTION AMOUNT */
            }
        }

        eval("javascript:push_remit_limit_response(1);");

    }

    this.check_aggregate_amount_callback = function(result)
    {
        var total_aggregate = 0; 
        
        if (result != null && result.value != null)
        {
            if (irem_amt_limit.iremit_object != null)
            {
                irem_amt_limit.iremit_object.close_msg_prompt(); /* DO NOT REMOVE */
            }
            
            if (irem_amt_limit.cash_aggregation_enabled == true)
            {
                var cash_aggregate = parseFloat(result.value[1]);
                if (irem_amt_limit.payment_type == 1) cash_aggregate += parseFloat(irem_amt_limit.gross_amount); 
                
                if (parseFloat(cash_aggregate) > parseFloat(irem_amt_limit.cash_aggregation_limit))
                {                
                    var msg = "Aggregate cash transaction now ( " + new common_method().to_currency((1 * parseFloat(cash_aggregate)).toFixed(2)) +  " ) and exceeds limit."; alert(msg); irem_amt_limit.iremit_object.remit_limit_response(0); return;
                }
            }
        
            if (irem_amt_limit.remittance_aggregate_tag == true)
            {
                total_aggregate = parseFloat(result.value[0]) + parseFloat(irem_amt_limit.gross_amount); //Modified RDG as Faultlog of Italy 02/12/2013
                
                if (parseFloat(total_aggregate) > parseFloat(irem_amt_limit.remittance_amount_limit))
                {                
                    var iconfirm = confirm("The total aggregated remittance amount of this remitter is now " + new common_method().to_currency((1 * parseFloat(total_aggregate)).toFixed(2)) + " Prior to this transaction.\n\n Press ok to continue or cancel to discard transaction.");

                    if (iconfirm == true)
                    {
                        irem_amt_limit.open_loggin_screen(); return;
                    }
                    else
                    {
                        eval("javascript:push_remit_limit_response(0);"); return;
                    }
                }
            }

            irem_amt_limit.iremit_object.remit_limit_response(1); return;
        }
        
        alert("Server nor responding. Transaction cancelled."); eval("javascript:push_remit_limit_response(0);");

    }
    
    this.check_right = function()
    {
        this.txt_otc_rem_limits_user_id       = document.getElementById("txt_otc_rem_limits_user_id").value;
        this.txt_otc_rem_limits_user_password = document.getElementById("txt_otc_rem_limits_user_password").value;
    
        AjaxPro.timeoutPeriod = 1000 * 1000;

        AjaxPro.onTimeout = function()
        {
            if (this.iremit_object != null)
            {
                this.iremit_object.close_msg_prompt();
            }

            alert("Server not responding. Checking user access to override failed.");
        }

        this.close_loggin_screen();

        if (this.iremit_object != null)
        {
            this.iremit_object.open_msg_prompt("Checking override access rights. Please wait ...");
        }

        IRSWorld.TranClass.RemTranExtra.user_can_exec_aggregates(this.txt_otc_rem_limits_user_id, this.txt_otc_rem_limits_user_password, this.check_right_callback);
    }

    this.check_right_callback = function(result)
    {
        if (irem_amt_limit.iremit_object != null)
        {
            irem_amt_limit.iremit_object.close_msg_prompt();
        }

        if (result != null && result.value != null)
        {
            /*

            if (result.value[0] == true) 
            {
                if (irem_amt_limit.limit_exceeds_flag == 3)
                {
                    eval("javascript:push_remit_limit_response(101);");
                }

                eval("javascript:push_remit_limit_response(100);");
            }

            */        

            if (result.value[0] == false)
            {
                alert(result.value[1]);

                irem_amt_limit.open_loggin_screen();                

                document.getElementById("txt_otc_rem_limits_user_id").value       = irem_amt_limit.txt_otc_rem_limits_user_id;
                document.getElementById("txt_otc_rem_limits_user_password").value = irem_amt_limit.txt_otc_rem_limits_user_password;

            }
            else
            {
                eval("javascript:push_remit_limit_response(100);");
            }
        }
    }
    
    this.chk_sv_br_limit = function()
    {        
        AjaxPro.timeoutPeriod = 1000 * 1000;
        AjaxPro.onTimeout = function()
        {
            alert("Server not responding. Checking approver limit.");
        }

        IRSWorld.TranClass.RemTranExtra.chk_ho_approver(document.getElementById("txt_otc_rem_limits_user_id").value, document.getElementById("txt_otc_rem_limits_user_password").value, this.chk_sv_br_limit_callback);
    }
    
    this.chk_sv_br_limit_callback = function(result)
    {
        var amt_limit_msg = null;

        if (result != null && result.value != null)
        {
            if (result.value == 2)
            {
                if (limit_exceeds_flag > 0)
                {
                    if (parseFloat(irem_amt_limit.gross_amount) > parseFloat(irem_amt_limit.remittance_supervisor_limit))
                    {
                        amt_limit_msg = "Amount exceeds supervisor limit. Transaction is set to For Approval - HO.";
                    }
                }
            }
            else if (result.value == 0)        
            {
                amt_limit_msg =  "Invalid login credential.";
            }
            else if (result.value == 3)
            {
                amt_limit_msg =  "Invalid access rights.";
            }

            if (amt_limit_msg == null || irem_amt_limit.limit_exceeds_flag == 3)
            {
                irem_amt_limit.check_right(); return;
            }

            alert(amt_limit_msg);
        }
    }

    this.open_loggin_screen = function()
    {
        this.open_modal_window();
        this.construct_loggin_screen();
    }
    
    this.close_loggin_screen = function()
    {
        this.close_modal_window();
    }
    
    this.construct_loggin_screen = function()
    {
        var html_string = "";

        html_string = "<table id = 'tbl_otc_amt_limits' style='border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; width: 400px; border-bottom: #336699 1px solid' cellpadding='0' cellspacing='1'>";
        html_string += "    <tr><td colspan='2' style='text-align: center' class='table_caption'>ENTER USERNAME AND PASSWORD</td></tr>";
        html_string += "    <tr style='height: 19px' >";
        html_string += "        <td class='cell_caption' style='width: 30%; text-align: right'>USER NAME :</td>";
        html_string += "        <td class='cell_desc' style='width: 70%; text-align: left'><input type = 'text' id = 'txt_otc_rem_limits_user_id' /></td>";
        html_string +=      "</tr>";
        html_string += "    <tr style='height: 19px'>";
        html_string += "        <td style='width: 30%; text-align: right;' class='cell_caption'>PASSWORD :</td>";
        html_string += "        <td style='width:70%; text-align: left;' class='cell_desc'><input type = 'password' id = 'txt_otc_rem_limits_user_password' /></td>";
        html_string += "    </tr>";
        html_string += "    <tr style='height: 19px'>";
        html_string += "        <td class='cell_desc' style='text-align: center' colspan='2'>";
        html_string += "            <input id='btn_otc_rem_limits_enter_ok' type='button' value='OK' style='width: 18%; height: 24px' />";
        html_string += "            <input id='btn_otc_rem_limits_enter_cancel' type='button' value='CANCEL' style='width: 18%; height: 24px'/>";
        html_string += "            <input id='btn_otc_rem_limits_rem_override' type='button' value='REMOTE OVERRIDE' style='width: 35%; height: 24px'/>";
        html_string += "        </td>";
        html_string += "    </tr>";
        html_string += "</table>";

        document.getElementById("content_mod_div").innerHTML = html_string;

        document.getElementById("btn_otc_rem_limits_enter_ok").onclick = function()
        {
            irem_amt_limit.check_right();
        }

        document.getElementById("btn_otc_rem_limits_enter_cancel").onclick = function()
        {
            irem_amt_limit.close_loggin_screen();
        }
        
        document.getElementById("btn_otc_rem_limits_rem_override").onclick = function()
        {
            var rem_ovr = new rem_ovr_scr();

            rem_ovr.jscls = irem_amt_limit;
            rem_ovr.chk_branch_is_allowed();
        }
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
	
	this.construct_div_content = function()
	{
	    var content_div = null;

		content_div                       = document.createElement("div");		
		content_div.id                    = "content_mod_div";
		content_div.align                 = "left";
		content_div.style.background      = "transparent";
		content_div.style.width           = "100%";
		content_div.innerHTML             = "<center><img src = '.././images/ajaxloader.gif'></img></center>";

		this.inner_modal_div              = document.createElement("div");		
		this.inner_modal_div.align        = "center";
		this.inner_modal_div.className    = "inner_div";		
		this.inner_modal_div.id           = "inner_mod_div";
		this.inner_modal_div.style.height = "auto";
		this.inner_modal_div.style.width  = "400px";
		this.inner_modal_div.style.top    = "30px";
		this.inner_modal_div.style.left   = (parseInt(screen.width) - parseInt(this.inner_modal_div.style.width)) / 2 - 10 + "px";
		this.inner_modal_div.style.border = "medium solid white";
		this.inner_modal_div.Code         = this;
		this.inner_modal_div.appendChild(content_div);
		
		document.body.appendChild(this.inner_modal_div);
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
	
	this.load_modal_css = function()
	{	    
		var file_reference = document.createElement("link");
		
	  	file_reference.setAttribute("rel", "stylesheet");
	  	file_reference.setAttribute("type", "text/css");
	  	file_reference.setAttribute("href", ".././css/cssmodals.css");

		if (typeof file_reference != "undefined")
		{		    
	  		document.getElementsByTagName("head")[0].appendChild(file_reference);
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
	        if (divelement_outer.item(i).id == "outer_mod_div")
	        {
	            divelement_outer.item(i).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop;break;
	        }
	    }

	    for(m = 0; m < divelement_inner.length;m++)
	    {
	        if (divelement_inner.item(m).id == "inner_mod_div")
	        {
	            divelement_outer.item(m).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop + 20 : document.body.scrollTop + 20;break;
	        }
	    }
	}
}
/* END REMITTANCE AMOUNT LIMIT SCREEN */

/* 14.) ACCOUNT STATUS INQUIRY SCREEN */
function account_screen_status()
{
    var account_stat = this;

    this.msg_screen      = null;
    this.outer_modal_div = null;
    this.inner_modal_div = null;
    this.commons         = null;
    
    this.get_acccount_status = function(account_number)
    {    
        AjaxPro.timeoutPeriod = 100 * 1000;
        
        AjaxPro.onTimeout = function()
        {
            alert("Server not responding. Checking account status failed.");
        }
        
        this.msg_screen = new screen_msg_demo();
        this.msg_screen.open_screen(); 
        this.msg_screen.change_msg("Checking account details. Please wait ...");
        
        IRSWorld.TranClass.RemTranExtra.get_account_details(account_number, this.get_acccount_status_callback);
    }
    
    this.get_acccount_status_callback = function(result)
    {
        account_stat.msg_screen.close_screen();        

        if (result != null && result.value != null)
        {
            account_stat.open_account_status_screen(result.value);
        }
    }

    this.open_account_status_screen = function(account_status)
    {
        this.open_modal_window();
        
        var ihtml = "";

        ihtml = '<table style="width: 400px; border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid">';        
        ihtml += '    <tr><td class = "cell_desc" style="height: 23px; text-align: left"><label id = "lbl_message_etc_inquiry">LOADING RESULT ...</label></td></tr>';
        ihtml += '    <tr><td class = "cell_desc" style="height: 23px; text-align: center"><input id="btn_otc_account_inquiry" type="button" value="OK" style="width: 18%; height: 24px"/></td></tr>';
        ihtml += '</table>';
                
        document.getElementById("content_mod_div").innerHTML = ihtml; 
        document.getElementById("btn_otc_account_inquiry").onclick = function()
        {
            account_stat.close_modal_window();
        }
        
        setText('lbl_message_etc_inquiry', account_status);
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
	    var content_div = null;
	    
		content_div                       = document.createElement("div");
		content_div.id                    = "content_mod_div";
		content_div.align                 = "left";
		content_div.style.background      = "transparent";
		content_div.style.width           = "100%";
		content_div.innerHTML             = "<center><img src = '.././images/ajaxloader.gif'></img></center>";

		this.inner_modal_div              = document.createElement("div");
		this.inner_modal_div.align        = "center";
		this.inner_modal_div.className    = "inner_div";
		this.inner_modal_div.id           = "inner_mod_div";
		this.inner_modal_div.style.height = "auto";
		this.inner_modal_div.style.width  = "400px";
		this.inner_modal_div.style.top    = "30px";
		this.inner_modal_div.style.left   = (parseInt(screen.width) - parseInt(this.inner_modal_div.style.width)) / 2 - 10 + "px";
		this.inner_modal_div.style.border = "medium solid white";
		this.inner_modal_div.Code         = this;
		this.inner_modal_div.appendChild(content_div); document.body.appendChild(this.inner_modal_div);
	}
	
	this.load_modal_css = function()
	{	    
		var file_reference = document.createElement("link");
		
	  	file_reference.setAttribute("rel", "stylesheet");
	  	file_reference.setAttribute("type", "text/css");
	  	file_reference.setAttribute("href", ".././css/cssmodals.css");

		if (typeof file_reference != "undefined")
		{		    
	  		document.getElementsByTagName("head")[0].appendChild(file_reference);
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
	        if (divelement_outer.item(i).id == "outer_mod_div")
	        {
	            divelement_outer.item(i).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop;break;
	        }
	    }

	    for(m = 0; m < divelement_inner.length;m++)
	    {
	        if (divelement_inner.item(m).id == "inner_mod_div")
	        {
	            divelement_outer.item(m).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop + 20 : document.body.scrollTop + 20;break;
	        }
	    }
	}
}
/* END ACCOUNT STATUS INQUIRY SCREEN */

/* 15.) GENERIC STATUS MESSAGE SCREEN */
function screen_msg_demo()
{
    this.outer_modal_div = null;
    this.inner_modal_div = null;
    this.commons         = null;

    this.open_screen = function()
    {
        this.open_modal_window();
        this.construct_modal_msg_layout();
    }
    
    this.construct_modal_msg_layout = function()
    {
        var ihtml = "";

        ihtml = '<table style="width: 400px; border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid">';
        ihtml += '    <tr><td colspan="2" style="height: 23px; text-align: center"><img src="../images/ajaxloader.gif" /></td></tr>';
        ihtml += '    <tr><td class = "cell_desc" colspan="2" style="height: 23px; text-align: center"><label id = "lbl_message_etc_otc">PLEASE WAIT ...</label></td></tr>';
        ihtml += '    <tr><td colspan="2" style="height: 23px; text-align: center"></td></tr>';
        ihtml += '</table>';
                
        document.getElementById("content_mod_div_screen_generic_msg").innerHTML = ihtml;        
    }
    
    this.close_screen = function()
    {
        this.close_modal_window();    
    }
    
    this.change_msg = function(msg)
    {
        if (msg != null && msg != '')
        {            
            setText("lbl_message_etc_otc", msg);
        }
    }

    this.open_modal_window = function()
	{
        this.load_modal_css();
		this.outer_modal_div           = document.createElement("div");
		this.outer_modal_div.id        = "outer_mod_div_screen_generic_msg";
		this.outer_modal_div.className = "outer_div";

		document.body.style.margin = "0px";
		document.body.appendChild(this.outer_modal_div);
		document.getElementById("outer_mod_div_screen_generic_msg").style.height = screen.height;

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
	    var content_div = null;
	    
		content_div                       = document.createElement("div");		
		content_div.id                    = "content_mod_div_screen_generic_msg";
		content_div.align                 = "left";
		content_div.style.background      = "transparent";
		content_div.style.width           = "100%";
		content_div.innerHTML             = "<center><img src = '.././images/ajaxloader.gif'></img></center>";	    

		this.inner_modal_div              = document.createElement("div");		
		this.inner_modal_div.align        = "center";
		this.inner_modal_div.className    = "inner_div";		
		this.inner_modal_div.id           = "inner_mod_div_screen_generic_msg";
		this.inner_modal_div.style.height = "auto";
		this.inner_modal_div.style.width  = "400px";
		this.inner_modal_div.style.top    = "30px";
		this.inner_modal_div.style.left   = (parseInt(screen.width) - parseInt(this.inner_modal_div.style.width)) / 2 - 10 + "px";
		this.inner_modal_div.style.border = "medium solid white";
		this.inner_modal_div.Code         = this;
		this.inner_modal_div.appendChild(content_div);
		
		document.body.appendChild(this.inner_modal_div);
	}	
	
	this.load_modal_css = function()
	{	    
		var file_reference = document.createElement("link");
		
	  	file_reference.setAttribute("rel", "stylesheet");
	  	file_reference.setAttribute("type", "text/css");
	  	file_reference.setAttribute("href", ".././css/cssmodals.css");

		if (typeof file_reference != "undefined")
		{		    
	  		document.getElementsByTagName("head")[0].appendChild(file_reference);
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
	        if (divelement_outer.item(i).id == "outer_mod_div_screen_generic_msg")
	        {
	            divelement_outer.item(i).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop;break;
	        }
	    }

	    for(m = 0; m < divelement_inner.length;m++)
	    {
	        if (divelement_inner.item(m).id == "inner_mod_div_screen_generic_msg")
	        {
	            divelement_outer.item(m).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop + 20 : document.body.scrollTop + 20;break;
	        }
	    }
	}
}
/* END GENERIC STATUS MESSAGE SCREEN */

/* 16.) ORD DATA ENTRY SCREEN */
function rem_modal_ord_data_entry()
{
    var orde = this;
    
    this.branchcode       = null;
    this.test_key_entry   = null;
    this.rembank_rel_code = null;
    this.due_code         = null;
    this.charge_code      = null;
    this.remitting_bank   = null;
    this.protecting_bank  = null;
    this.date_received    = null;
    this.date_value       = null;
    this.date_sent        = null;
    this.origin_code      = null;
    this.CIFID            = null;
    this.mode             = 0;
    this.ref              = null;
    this.transaction_id   = null;
    this.file_format      = null;
    
    this.page_onload = function()
    {
        this.file_format = IRSWorld.TranClass.ord_data_entry.getBatchHeader().value;
        this.populate_drop_down_list();
        this.set_rembank_rel_code();        
    }
    
    this.ode_form = function(ref, CIFID, branchcode)
    {
        this.CIFID = CIFID;
        this.branchcode = branchcode;
        this.mode = 1;
        this.ref = ref;
        this.open_modal_window();
        this.construct_override_window();
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
	
	this.construct_div_content = function()
	{
	    var content_div = null;

		content_div                       = document.createElement("div");
		content_div.id                    = "content_mod_div";
		content_div.align                 = "left";
		content_div.style.background      = "transparent";
		content_div.style.width           = "100%";
		content_div.style.overflow        = "auto";
		content_div.innerHTML             = "<center><img src = '.././images/ajaxloader.gif'></img></center>";

		this.inner_modal_div              = document.createElement("div");
		this.inner_modal_div.align        = "center";
		this.inner_modal_div.className    = "inner_div";
		this.inner_modal_div.id           = "inner_mod_div";
		this.inner_modal_div.style.height = "auto";		
		this.inner_modal_div.style.width  = parseInt(parent.screen.availWidth)  - 350 + "px";
		this.inner_modal_div.style.top    = "30px";
		this.inner_modal_div.style.left   = (parseInt(screen.width) - parseInt(this.inner_modal_div.style.width)) / 2 - 10 + "px";
		this.inner_modal_div.style.border = "medium solid white";		
		this.inner_modal_div.Code         = this;
		this.inner_modal_div.appendChild(content_div);

		document.body.appendChild(this.inner_modal_div);
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

	this.load_modal_css = function()
	{
		var file_reference = document.createElement("link");

	  	file_reference.setAttribute("rel", "stylesheet");
	  	file_reference.setAttribute("type", "text/css");
	  	file_reference.setAttribute("href", ".././css/cssmodals.css");

		if (typeof file_reference != "undefined")
		{		    
	  		document.getElementsByTagName("head")[0].appendChild(file_reference);
		}
	}
	
	this.construct_override_window = function()
    {
        if (this.CIFID != null && this.CIFID != "undefined" && this.CIFID != "" && this.branchcode != null && this.branchcode != "undefined" && this.branchcode != "")
        {            
            var html_string = "";
            
            html_string += "<center>";            
            html_string +="<table width='100%' cellpadding='0' cellspacing='1'>";
            html_string +="    <tr>";
            html_string +="        <td style='width: 100%; height: 100%;'>";
            html_string +="            <div id = 'ODECIFINFO' style= 'display:block'>";
            html_string +="                <table width='100%' cellpadding='0'>";
            html_string +="                    <tr>";
            html_string +="                        <td align='center' class='table_caption' style='width: 40%; border-collapse: collapse;height: 24px'>DATA ENTRY</td>";
            html_string +="                    </tr>";
            html_string +="                </table>";
            html_string +="            </div>";
            html_string +="            <div id = 'ODEACCOUNTINFO' style = 'overflow:auto'>";
            html_string +="                <table width='100%' cellpadding='0' cellspacing='3'>";
            html_string +="                    <tr>";
            html_string +="                        <td class='cell_caption' style='width: 15%; height:24px'>Test Key Entry :&nbsp;</td>";
            html_string +="                        <td align='left' style='width:30%; height: 24px;' class='cell_desc'><input id='txtODEtestkey' type='text' style = 'width:98%' maxlength='50'/></td>";
            html_string +="                        <td class='cell_caption' style='width: 15%; height:24px'>Date Received :&nbsp;</td>";
            html_string +="                        <td align='left' style='width:30%; height: 24px;' class='cell_desc'><input id='txtODEdatereceived' type='text' style = 'width:88%' maxlength='50' disabled = 'true'/><img id = 'imgDateReceieved' src = '../images/calendar/imgcalen.png' align'absmiddle' alt = 'Pick a date.' onclick = 'popUpCalendar(document.getElementById(\"imgDateReceieved\"),document.getElementById(\"txtODEdatereceived\"),\"m/dd/yyyy\")' /></td>";
            html_string +="                    </tr>";
            html_string +="                    <tr>";
            html_string +="                        <td class='cell_caption' style='width: 15%; height:24px'>RemBank Rel Code :&nbsp;</td>";
            html_string +="                        <td align='left' style='width:30%; height: 24px;' class='cell_desc'><input id='txtODERelcode' type='text' style = 'width:98%' maxlength='50' disabled = 'true'></input></td>";
            html_string +="                        <td class='cell_caption' style='width: 15%; height:24px'>Date Value :&nbsp;</td>";
            html_string +="                        <td align='left' style='width:30%; height: 24px;' class='cell_desc'><input id='txtODEdatevalue' type='text' style = 'width:88%' maxlength='50' disabled = 'true'/><img id = 'imgDateValue' src = '../images/calendar/imgcalen.png' align'absmiddle' alt = 'Pick a date.' onclick = 'popUpCalendar(document.getElementById(\"imgDateValue\"),document.getElementById(\"txtODEdatevalue\"),\"m/dd/yyyy\")' /></td>";
            html_string +="                    </tr>";
            html_string +="                    <tr>";
            html_string +="                        <td class='cell_caption' style='width: 15%; height:24px'>Remitting Bank :&nbsp; </td>";
            html_string +="                        <td align='left' style='width:30%; height: 24px;' class='cell_desc'><label id=\"lblODErembank\" class=\"cell_desc\">"
            html_string +="                            <a href=\"#\" id=\"lnkODErembank\" class=\"cell_desc\" style=\"text-decoration:underline\" onclick=\"iremit.ord_data_entry.get_grs_corrbank('lnkODErembank')\">(Please select)</a></label></td>";
            html_string +="                        <td class='cell_caption' style='width: 15%; height:24px'>Date Sent :&nbsp; </td>";
            html_string +="                        <td align='left' style='width:30%; height: 24px; display:inline;' class='cell_desc'><input id='txtODEdatesent' style = 'width:88%' type='text' maxlength='50' disabled = 'true' /><img id = 'imgDateSent' src = '../images/calendar/imgcalen.png' align'absmiddle' alt = 'Pick a date.' onclick = 'popUpCalendar(document.getElementById(\"imgDateSent\"),document.getElementById(\"txtODEdatesent\"),\"m/dd/yyyy\")' /></td>";
            html_string +="                    </tr>";
            html_string +="                    <tr>";
            html_string +="                        <td class='cell_caption' style='width: 15%; height:24px'>Protecting Bank :&nbsp; </td>";
            html_string +="                        <td align='left' style='width:30%; height: 24px;' class='cell_desc'><label id=\"lblODEprocbank\" class=\"cell_desc\">"
            html_string +="                            <a href=\"#\" id=\"lnkODEprocbank\" class=\"cell_desc\" style=\"text-decoration:underline\" onclick=\"iremit.ord_data_entry.get_grs_corrbank('lnkODEprocbank')\">(Please select)</a></label></td>";
            html_string +="                        <td id='tbCompIDcap' class='cell_caption' style='width: 15%; height:24px; display:none'>Company ID :&nbsp; </td>";
            html_string +="                        <td id='tbCompIDdesc' align='left' style='width:30%; height: 24px; display:none' class='cell_desc'><select id = 'ddlODEcompid' style='width: 98%'><option selected='selected'></option></select></input></td>";
            html_string +="                    </tr>";
            html_string +="                </table>";
            html_string +="                <table width='100%' cellpadding='0' cellspacing='5' style='background:azure;'>";
            html_string +="                    <tr id='trWait' style='display:none'>";
            html_string +="                        <td style='height: 20px;width:20%' align='center' class='cell_desc'><label id='lblProcessing'></label><img src = '.././images/ajaxloader.gif'></img></td>";
            html_string +="                    </tr>";
            html_string +="                    <tr>";
            html_string +="                        <td width='20%' style='height: 20px' align='right'>";
            html_string +="                            <input style=' width:150px; height: 25px;' id='btnODEOK' type='button' value='OK' onclick='javascript:" + this.ref + ".btn_ODE_ok_click();'/>&nbsp;";
            html_string +="                            <input style=' width:150px; height: 25px;' id='btnODECANCEL' type='button' value='CLOSE' onclick='javascript:" + this.ref + ".btn_ODE_cancel_click();'/>&nbsp;";
            html_string +="                        </td>";
            html_string +="                    </tr>";
            html_string +="                </table>";
            html_string +="            </div>";
            html_string +="        </td>";
            html_string +="    </tr>";
            html_string +="</table>";
            html_string += "</center>";

            document.getElementById("content_mod_div").innerHTML = html_string; 
            
            this.page_onload();
       }
       else
       {
            this.close_modal_window(); alert('PARAMETER WAS NOT SUPPLIED.')
       }
    }
    
    this.btn_ODE_cancel_click = function()
    {
         this.close_modal_window();
    }
    
    this.btn_ODE_ok_click = function()
    {         
        if (this.mode == 0)
        {
            this.close_modal_window();  eval("javascript:push_ord_data_entry_result();");
        }
        else if (this.mode == 1)
        {
            var msg = this.validate();
            
            if(msg == "")
            {
                this.set_variable();
                
                var data ="REM_SEQ_ID~<SEQ_ID>";
                
                data +="|TestKeyEntry~"       + this.test_key_entry;
                data +="|RembankRelCode~"     + this.rembank_rel_code ;
                data +="|RemittingBankCode~"  + this.remitting_bank;
                data +="|ProtBankCd~"         + this.protecting_bank;
                data +="|DateReceived~"       + this.date_received;
                data +="|DateValue~"          + this.date_value;
                data +="|DateSent~"           + this.date_sent;
                
                if(iremit.ord_data_entry.file_format == "Payroll")
                {
                    data +="|RecordID~" + this.origin_code;
                }
                
                IRSWorld.TranClass.ord_data_entry.save_ord_data(data, this.btn_ODE_ok_click_callback);
            }
            else
            {
                alert(msg);
            }
        }
    }
    
    this.btn_ODE_ok_click_callback = function(response)
    {
        if(response.value == "")
        {
            alert("FAILED TO GET DATA.");
        }
        else
        {
            iremit.ord_data_entry.close_modal_window();eval("javascript:push_ord_data_entry_result('" + String(iremit.ord_data_entry.origin_code) + "');");
        }        
    }
    
    this.set_variable = function()
    {
        this.due_code         = "";
        this.charge_code      = "";
        this.test_key_entry   = document.getElementById("txtODEtestkey").value;
        this.rembank_rel_code = document.getElementById("txtODERelcode").value;
        this.remitting_bank   = (document.getElementById("lnkODErembank").value  == "null" || document.getElementById("lnkODErembank").value == "" || document.getElementById("lnkODErembank").value == undefined) ? "0000000000" : document.getElementById("lnkODErembank").value;
        this.protecting_bank  = (document.getElementById("lnkODEprocbank").value == "null" || document.getElementById("lnkODEprocbank").value == "" || document.getElementById("lnkODEprocbank").value == undefined) ? "0000000000" : document.getElementById("lnkODEprocbank").value;
        this.date_received    = document.getElementById("txtODEdatereceived").value;
        this.date_value       = document.getElementById("txtODEdatevalue").value;
        this.date_sent        = document.getElementById("txtODEdatesent").value;
        this.origin_code      = document.getElementById("ddlODEcompid").value == "" ? "" : document.getElementById("ddlODEcompid").value;
    }
    
    this.populate_drop_down_list = function() 
    {                
        if(iremit.ord_data_entry.file_format == "Payroll")
        {
            document.getElementById("trWait").style.display = 'block';
            
            setText(document.getElementById("lblProcessing"),"PLEASE WAIT...");
            IRSWorld.TranClass.ord_data_entry.getDDLData("Payroll",this.populate_drop_down_list_Callback);
        }        
    }
   
    this.populate_drop_down_list_Callback = function(response)
    {
        if(response.error != null)
        {
            alert("NO DATASET FOUND."); 
        }
        else
        {
            var ds = response.value;
            var commons = new common_method();
                        
            if(iremit.ord_data_entry.file_format == "Payroll")
            {
                document.getElementById("ddlODERemBank").disabled     = true;
                document.getElementById("ddlODEProcBank").disabled    = true;
                document.getElementById("tbCompIDcap").style.display  = 'block';
                document.getElementById("tbCompIDdesc").style.display = 'block';
                
                commons.to_load_dropdownlist('ddlODEcompid', null, 'Value', 'Text', ds.Tables[0]);
                commons = null;
            }
        }

        document.getElementById("trWait").style.display = 'none'; setText(document.getElementById("lblProcessing"),"");
    }
    
    this.grd_ordsearch = null;
    this.lblcrtls = null;
    this.get_grs_corrbank = function(lbl)
    {
        var grd_container  = null;
        var grd_source     = "SELECT DISTINCT CORR_BANK_CODE VALUE, CORR_BANK_NAME [TEXT] FROM TBL_GRS_CORRBANK ORDER BY CORR_BANK_NAME";
        this.lblcrtls = document.getElementById(lbl);
        iremit.grd_search_keyword = 'GRS_CORRBANK';
        iremit.ord_data_entry.grd_ordsearch = new grid_selector();
        iremit.ord_data_entry.grd_ordsearch.request_database_source = grd_source;
        iremit.ord_data_entry.grd_ordsearch.open_selection_grid("../AjxPage/ajaxgrid.aspx", grd_source, 0 , grd_container);
    }
    
    this.lookup_corrbank = function(parameter)
    {
        iremit.ord_data_entry.lblcrtls.value = parameter;
        iremit.ord_data_entry.set_rembank_rel_code();        
        IRSWorld.TranClass.ord_data_entry.getCorrbank(parameter,this.lookup_corrbank_callback);
    }
    
    this.lookup_corrbank_callback = function(param)
    {
        if (param != null)
        {
            setText(iremit.ord_data_entry.lblcrtls, param.value);            
        }
    }
    
    this.set_rembank_rel_code = function()
    {
        var procbank = document.getElementById('lnkODEprocbank').value;
        
        if (procbank == "null" || procbank == "" || procbank == undefined)
        {
            document.getElementById('txtODERelcode').value = 3;
        }
        else
        {
            document.getElementById('txtODERelcode').value = 4;
        }
    }
    
    this.commit_transaction = function(transaction_amount)
    {
        if (transaction_amount != null || transaction_amount != '')
        {
            transaction_amount = parseFloat(new common_method().to_remove_comma(transaction_amount));
        }
        else
        {
            transaction_amount = 0;
        }
                
        IRSWorld.TranClass.ord_data_entry.commit_transactions(this.transaction_id.toString(), transaction_amount  , this.commit_transaction_callback);
    }
    
    this.commit_transaction_callback = function(response)
    {
        if (response.error != null)
        {
            eval("javascript:push_ord_data_entry_commit_response('0~FAILED TO SAVE ORD DATA ENTRIES.');");
        }
        else
        {
            if (response.value.toString() == "1")
            {
                eval("javascript:push_ord_data_entry_commit_response('1');");
            }
            else
            {
                eval("javascript:push_ord_data_entry_commit_response(0~" + response.value + ");");
            }    
        }
    }
    
    this.validate = function()
    {
        var msg = "";
        
        if (document.getElementById("txtODERelcode").value == "")
        {
            msg += "Remitting Bank Rel Code is empty. \n";
        }
        
        if(document.getElementById("lnkODErembank").value == "" || document.getElementById("lnkODErembank").value == "null")
        {
            if(document.getElementById("lnkODErembank").disabled != true)
            {
                msg += "Please select remitting bank. \n";
            }
        }
        
        if(document.getElementById("lnkODEprocbank").value == "" || document.getElementById("lnkODEprocbank").value == "null")
        {
            msg += "";
        }
        
        if(document.getElementById("txtODEdatereceived").value == "")
        {
            msg += "Date received is empty. \n";
        }
        
        if(document.getElementById("txtODEdatevalue").value == "")
        {
            msg += "Date value is empty. \n";
        }
        
        if(document.getElementById("txtODEdatesent").value == "")
        {
                msg += "Date sent is empty. \n";
        }
        
        if(document.getElementById("txtODEtestkey").value == "")
        {
            msg += "Test key entry is empty. \n";
        }
        
        return msg;
    }
}
/* END ORD DATA ENTRY SCREEN */

/* 17.) DEBIT CREDIT CARD PAYMENT SCREEN */
function rem_modal_card_payments()
{
    var icrd = this;
    
    this.transaction_amount = 0;
    this.card_number        = null;
    this.issuedate          = null;
    this.expirydate         = null;
    this.card_name          = null;
    this.security_code      = null;
    this.cifid              = null;    
    this.transaction_refno  = null;
    this.card_seqid         = null;
    this.ref                = null;
    this.rem_currency       = null;
    this.card_type          = null;
    this.card_class         = null;
    this.response           = null;
    this.message            = null;
    this.auth_code          = null;
    this.country            = null;
    this.generic            = null;
    this.response_all       = null;
    this.otc_card_payment   = false;
    this.rem_currencycode   = null; //Added by Troy 20120628
    this.txn_seqid          = null; //Added by Troy 20120629
    this.dccancel           = false; //Added by Troy 20120629
    this.iremit_object      = null; //Added by Troy 20120810
    
    this.page_onload = function()
    {
        IRSWorld.TranClass.rem_card_screen.has_cards(this.cifid, this.page_onload_callback);
    }
    
    this.page_onload_callback = function(response)
    {
        if (response.error != null)
        {
            alert("No dataset returned");
        }
        else
        {
            if (response.value == true)
            {
                icrd.process_viewer('ajx_loading$');
            }
            
            icrd.populate_rem_info();
            icrd.populate_drop_down_list();
            
            //document.getElementById('txtCurrency').value = icrd.rem_currency;
            document.getElementById('txtCurrency').value = icrd.rem_currencycode; //Edited by Troy 20120628

            setText("txtAmount", icrd.transaction_amount);
            
            if (icrd.card_seqid != null && icrd.card_seqid != '' && icrd.card_seqid != 'undefined')
            {
                 icrd.populateInfo(icrd.card_seqid);
            }
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
    
    this.card_form = function(CIFID, trxAmount, ref, trxcurrency, currencycode, iremit_object)
    {
        if (iremit_object != null){this.iremit_object = iremit_object;}
        this.cifid              = CIFID;
        this.transaction_amount = trxAmount;
        this.ref                = ref;
        this.rem_currency       = trxcurrency;
        this.rem_currencycode   = currencycode; //Added by Troy 20120628

        this.open_modal_window();
        this.construct_override_window();
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
	
	this.construct_div_content = function()
	{
	    var content_div = null;

		content_div                       = document.createElement("div");		
		content_div.id                    = "content_mod_div";
		content_div.align                 = "left";
		content_div.style.background      = "transparent";
		content_div.style.width           = "100%";
		content_div.style.overflow        = "auto";
		content_div.innerHTML             = "<center><img src = '.././images/ajaxloader.gif'></img></center>";

		this.inner_modal_div              = document.createElement("div");		
		this.inner_modal_div.align        = "center";
		this.inner_modal_div.className    = "inner_div";		
		this.inner_modal_div.id           = "inner_mod_div";
		this.inner_modal_div.style.height = "auto";		
		this.inner_modal_div.style.width  = "600px";
		this.inner_modal_div.style.top    = "30px";
		this.inner_modal_div.style.left   = (parseInt(screen.width) - parseInt(this.inner_modal_div.style.width)) / 2 - 10 + "px";
		this.inner_modal_div.style.border = "medium solid white";		
		this.inner_modal_div.Code         = this;
		this.inner_modal_div.appendChild(content_div);
		
		document.body.appendChild(this.inner_modal_div);
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
	
	this.load_modal_css = function()
	{	    
		var file_reference = document.createElement("link");
		
	  	file_reference.setAttribute("rel", "stylesheet");
	  	file_reference.setAttribute("type", "text/css");
	  	file_reference.setAttribute("href", ".././css/cssmodals.css");

		if (typeof file_reference != "undefined")
		{		    
	  		document.getElementsByTagName("head")[0].appendChild(file_reference);
		}
	}
	
	this.construct_override_window = function()
    {
        if (this.cifid != null && this.cifid != "undefined" && this.cifid != "" && this.transaction_amount != null && this.transaction_amount != "undefined" && this.transaction_amount != 0)
        {            
            var html_string = "";
            
            html_string += "<center><table style='width: 600px; border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid'>";
            html_string += "    <tr>";
            html_string += "        <td>";
            html_string += "            <div id = 'DACCTCIFINFO' style= 'display:block'>";
            html_string += "                <table width='100%' cellpadding='0'>";
            html_string += "                    <tr>";
            html_string += "                        <td class='cell_desc' style='width: 10%; height: 24px; text-align: right;'>CIF ID :&nbsp;</td>";
            html_string += "                        <td class='cell_desc' style='width: 20%; height: 24px; text-align: left;'colspan='4'><label id='lblCARDCIFID' class='cell_desc'></label></td>";
            html_string += "                        <td class='cell_desc' style='width: 19%; height: 24px; text-align: right;'>Remitter Name :</td>";
            html_string += "                        <td align='left'      style='width: 61%; height: 24px;' colspan='4'>";
            html_string += "                            <label id='lblCARDCustFName' class='cell_desc'></label>&nbsp;";
            html_string += "                            <label id='lblCARDCustMName' class='cell_desc'></label>&nbsp;";
            html_string += "                            <label id='lblCARDCustLName' class='cell_desc'></label>&nbsp;";
            html_string += "                        </td>";
            html_string += "                    </tr>";
            html_string += "                </table>";
            html_string += "            </div>";
            html_string += "            <div id = 'CARDACCOUNTGRID' style= 'display:block'>";
            html_string += "                <table id='Table10' width='100%' cellpadding='0' cellspacing='5' style='border: solid 1px #336699; background:azure;'>";
            html_string += "                    <tr>";
            html_string += "                        <td style='width: 100%; text-align: left; text-indent: 5px; color: Black; height: 19px;' colspan='5'></td>";
            html_string += "                    </tr>";
            html_string += "                    <tr>";
            html_string += "                        <td style='height: 2px'><div id = 'Div2' style= 'display:none'>  PLACE GRIDVIEW ID'S</div></td>";
            html_string += "                    </tr>";
            html_string += "                </table>";
            html_string += "            </div>";
            html_string += "            <div id = 'DACCOUNTINFO' style = 'overflow:auto'>";
            html_string += "                <table width='100%' cellpadding='0' cellspacing='3'>";
            html_string += "                    <tr>";
            html_string += "                        <td class='cell_caption' style='width: 15%; height:24px; text-align: right;'>Card Number :&nbsp;</td>";
            html_string += "                        <td align='left' style='width:30%; height: 24px;' class='cell_desc'><input id='txtCardNumber' type='text' style = 'width:88%' maxlength='50' disabled/></td>";
            html_string += "                    </tr>";
            html_string += "                    <tr>";
            html_string += "                        <td class='cell_caption' style='width: 15%; height:24px; text-align: right;'>Card Type :&nbsp;</td>";
            html_string += "                        <td align='left' style='width:30%; height: 24px;' class='cell_desc'><select id='ddCardType' style = 'width:88%' disabled><option selected='selected'></option></select></td>";
            html_string += "                    </tr>";
            html_string += "                    <tr>";
            html_string += "                        <td class='cell_caption' style='width: 15%; height:24px; text-align: right;'>Currency :&nbsp;</td>";
            html_string += "                        <td align='left' style='width:30%; height: 24px;' class='cell_desc'><input id='txtCurrency' type='text' style = 'width:88%' maxlength='50'disabled/></td>";
            html_string += "                    </tr>";
            html_string += "                    <tr>";
            html_string += "                        <td class='cell_caption' style='width: 15%; height:24px; text-align: right;'>Amount :&nbsp;</td>";
            html_string += "                        <td align='left' style='width:30%; height: 24px;' class='cell_desc'><input id='txtAmount' type='text' style = 'width:88%' maxlength='50'disabled/></td>";
            html_string += "                    </tr>";
            html_string += "                    <tr>";
            html_string += "                        <td class='cell_caption' style='width: 25%; height:24px; text-align: right;'>Response :&nbsp;</td>";
            html_string += "                        <td align='left' style='width:30%; height: 24px;' class='cell_desc'><textarea id='txtaResponse' rows='2' style = 'width:88%' maxlength='500' readonly = 'true'></textarea></td>";
            html_string += "                    </tr>";
            html_string += "                </table>";
            html_string += "                <table width='100%' cellpadding='0' cellspacing='5' style='background:azure;'>";
            html_string += "                    <tr>";
            html_string += "                        <td width='20%' style='height: 20px' align='right'>";
            html_string += "                            <input style='width:150px; height: 25px;' id='btnCARDOK' type='button' value='OK' onclick='javascript:" + this.ref + ".card_security_form();'/>&nbsp;";
            html_string += "                            <input style='width:150px; height: 25px;' id='btnCARDCLOSE' type='button' value='CLOSE' disabled='disabled' onclick='javascript:" + this.ref + ".btn_CARD_close_click();'/>&nbsp;";
            html_string += "                            <input style='width:150px; height: 25px;' id='btnCARDCANCEL' type='button' value='CANCEL' onclick='javascript:" + this.ref + ".btn_CARD_cancel_click();'/>&nbsp;";
            html_string += "                        </td>";
            html_string += "                    </tr>";
            html_string += "                </table>";
            html_string += "            </div>";
            html_string += "        </td>";
            html_string += "    </tr>";
            html_string += "</table></center>";
            
            document.getElementById("content_mod_div").innerHTML = html_string; this.page_onload();
       }
       else
       {
            this.close_modal_window(); alert('PARAMETER WAS NOT SUPPLIED.');
       }
    }
    
    this.card_security_form = function()
    {
        if (this.otc_card_payment == true)
        {
            if (this.response_all != null)
            {
                if(this.response_all.indexOf('OK') > 0)
                {
                    alert("Previous payment was done already. Click cancel or close to continue."); return;
                }
            }
        }

        //Added by Troy 20120629
        if (document.getElementById("txtCardNumber").value == null || document.getElementById("txtCardNumber").value == undefined || document.getElementById("txtCardNumber").value == "")
        {
            alert("Please select a debit/credit card.");
        }
        else
        {
            this.open_security_modal_window();
            this.construct_security_window();
        }
    }
    
    this.construct_security_div_content = function()
	{
	    var content_security_div = null;
	    var inner_security_modal_div_width = parseInt(parent.screen.availWidth)  - 350 + "px";
	    
	    if (this.otc_card_payment == true)
	    {
            inner_security_modal_div_width = "600px";
	    }

		security_content_div                       = document.createElement("div");		
		security_content_div.id                    = "divSecurity";
		security_content_div.align                 = "left";
		security_content_div.style.background      = "transparent";
		security_content_div.style.width           = "100%";
		security_content_div.style.overflow        = "auto";
		security_content_div.innerHTML             = "<center><img src = '.././images/ajaxloader.gif'></img></center>";

		this.inner_security_modal_div              = document.createElement("div");		
		this.inner_security_modal_div.align        = "center";
		this.inner_security_modal_div.className    = "inner_div";		
		this.inner_security_modal_div.id           = "inner_mod_div";
		this.inner_security_modal_div.style.height = "auto";		
		this.inner_security_modal_div.style.width  = inner_security_modal_div_width;
		this.inner_security_modal_div.style.top    = "30px";
		this.inner_security_modal_div.style.left   = (parseInt(screen.width) - parseInt(this.inner_security_modal_div.style.width)) / 2 - 10 + "px";
		this.inner_security_modal_div.style.border = "medium solid white";		
		this.inner_security_modal_div.Code         = this;
		this.inner_security_modal_div.appendChild(security_content_div);
		
		document.body.appendChild(this.inner_security_modal_div);
	}
	
    this.construct_security_window = function()
    {
        var html_string = "";
        
        html_string = "<table cellpadding=\"0\" cellspacing=\"0\" style=\"FONT-SIZE: 12px; width: 100%;\">";
        html_string +=      "<tr>";
        html_string +=         "<td align=\"left\" valign=\"bottom\" style=\"width:171px; border-collapse:collapse;height:24px;\"><img src=\"../images/toe.GIF\" /></td>";
        html_string +=         "<td class=\"table_head_dark\" style=\"width: 75%;height:24px;\">Please enter your security code</td>";
        html_string +=         "<td align=\"left\" style=\"width:100%;height:24px;\" valign=\"bottom\"><img align=\"absBottom\" src=\"../images/rend.gif\" /></td>";
        html_string +=     "</tr>";
        html_string +=     "<tr>";
        html_string +=        "<td style=\"border-right: #336699 3px solid; border-top: #336699 3px solid; border-left: #336699 3px solid; border-bottom: #336699 3px solid;\" colspan=\"3\">";
        html_string +=             "<table>";
        html_string +=                 "<tr>";
        html_string +=                     "<td style=\"width:50%;height:149px;\"><img src=\"../images/cardsecurity.jpg\" width=\"200\" style=\"height: 147px;\" /></td>";
        html_string +=                     "<td style=\width:50%;height:149px;\">";
        html_string +=                          "<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" style=\"FONT-SIZE: 12px; \">";
        html_string +=                             "<tr>";
        html_string +=                                "<td><input id=\"txtSecurityCode\" type=\"password\" maxlength=\"3\" style=\"width: 95%\" onkeypress=\"javascript:return " + this.ref + ".isNumberKey(event);\" /></td>";
        html_string +=                             "</tr>";
        html_string +=                             "<tr>";
        html_string +=                                "<td style=\"height:22px\">";
        html_string +=                                     "<input id=\"btnSubmitTransaction\" class=\"button\" onclick=\"javascript:" + this.ref +  ".btn_CARD_ok_click();\" onmouseout=\"this.className='button';\" onmouseover=\"this.className='button_over';\" type=\"button\" value=\"Submit\" style=\"width: 90px\" />";
        html_string +=                                     "<input id=\"btnCancelTransaction\" class=\"button\" onclick=\"javascript:" + this.ref + ".submitCancel();\" onmouseout=\"this.className='button';\" onmouseover=\"this.className='button_over';\" type=\"button\" value=\"Cancel\" style=\"width: 90px\" />";
        html_string +=                                "</td>";
        html_string +=                             "</tr>";
        html_string +=                          "</table>";
        html_string +=                      "</td>";
        html_string +=                   "</tr>";
        html_string +=              "</table>";
        html_string +=         "</td>";
        html_string +=     "</tr>";
        html_string +=     "<tr>";
        html_string +=         "<td colspan=\"3\">";
        html_string +=             "<span id=\"spnPGLoading\"></span>";
        html_string +=         "</td>";
        html_string +=     "</tr>";
        html_string +="</table>";
        
        if (this.otc_card_payment == true)
        {
            html_string = "";
            html_string += "<table style='width: 600px; border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid'>";
            html_string += "    <tr>";
            html_string += "        <td class = 'cell_caption' style = 'width: 15%; height:24px; text-align: right'>ENTER SECURITY CODE :</td>";
            html_string += "        <td align = 'left' style = 'width:30%; height: 24px' class = 'cell_desc'>";
            html_string += "            <input id = 'txt_security_code' type = 'password' style = 'width:88%' maxlength = '50' onkeypress = 'javascript:return '" + this.ref + ".isNumberKey(event)'/>";
            html_string += "        </td>";
            html_string += "    </tr>";
            html_string += "    <tr>";
            html_string += "        <td class = 'cell_desc' colspan = '2' style = 'height: 24px; text-align: center'>";
            html_string += "            <input id = 'btn_card_validate'  type ='button' value = 'VALIDATE' style = 'width: 90px; text-align:center'  onclick = 'javascript:" + this.ref + ".card_code_validate_submit();'/>";
            html_string += "            <input id = 'btn_card_cancelled' type ='button' value = 'CANCEL'   style = 'width: 90px; text-align:center'  onclick = 'javascript:" + this.ref + ".card_code_val_cancelled();'/>";
            html_string += "        </td>";
            html_string += "    </tr>";
            html_string += "</table>";
        }
        
        document.getElementById("divSecurity").innerHTML = html_string;
    }
    
    this.close_security_modal_window = function()
	{
	    this.commons = new common_method();
        this.commons.to_show_hide_combobox(true);
        this.commons = null;

		if(this.outer_security_modal_div != null){document.body.removeChild(this.outer_security_modal_div);this.outer_security_modal_div  = null;}
		if(this.inner_security_modal_div != null){document.body.removeChild(this.inner_security_modal_div);this.inner_security_modal_div  = null;}
		if(this.content_security_iframe  != null){this.content_iframe = null;}

		document.body.style.margin = "";		
	}
	
	this.open_security_modal_window = function()
	{
        this.load_modal_css();
		this.outer_security_modal_div           = document.createElement("div");
		this.outer_security_modal_div.id        = "outer_security_mod_div";
		this.outer_security_modal_div.className = "outer_div";		
		
		document.body.style.margin = "0px";
		document.body.appendChild(this.outer_security_modal_div);
		document.getElementById("outer_security_mod_div").style.height = screen.height;		
		
		this.construct_security_div_content();
        this.commons = new common_method();
        this.commons.to_show_hide_combobox(false);
        this.commons = null;
	}
	
    this.isNumberKey = function(evt)
    {
        var charCode = (evt.which) ? evt.which : event.keyCode
        if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

        return true;
    }
    
    this.card_code_val_cancelled = function()
    {
        this.close_security_modal_window();
    }
    
    this.submitCancel = function()
    {        
        alert("Request cancelled by user.");
        this.close_security_modal_window();
    }
    
    this.threedsecure_form = function(htmlval,htmllink)
    {
        icrd.open_threedsecure_modal_window();        
        icrd.construct_threedsecure_window(htmlval,htmllink);
    }
    
    this.construct_threedsecure_div_content = function()
	{
	    var content_threedsecure_div = null;
	    
		threedsecure_content_div                       = document.createElement("div");		
		threedsecure_content_div.id                    = "div3DSecure";
		threedsecure_content_div.align                 = "left";
		threedsecure_content_div.style.background      = "transparent";
		threedsecure_content_div.style.width           = "100%";
		threedsecure_content_div.style.overflow        = "auto";
		threedsecure_content_div.innerHTML             = "<center><img src = '.././images/ajaxloader.gif'></img></center>";	    

		this.inner_threedsecure_modal_div              = document.createElement("div");		
		this.inner_threedsecure_modal_div.align        = "center";
		this.inner_threedsecure_modal_div.className    = "inner_div";		
		this.inner_threedsecure_modal_div.id           = "inner_mod_div";
		this.inner_threedsecure_modal_div.style.height = "auto";		
		this.inner_threedsecure_modal_div.style.width  = parseInt(parent.screen.availWidth)  - 350 + "px";
		this.inner_threedsecure_modal_div.style.top    = "30px";
		this.inner_threedsecure_modal_div.style.left   = (parseInt(screen.width) - parseInt(this.inner_threedsecure_modal_div.style.width)) / 2 - 10 + "px";
		this.inner_threedsecure_modal_div.style.border = "medium solid white";		
		this.inner_threedsecure_modal_div.Code         = this;
		this.inner_threedsecure_modal_div.appendChild(threedsecure_content_div);
		
		document.body.appendChild(this.inner_threedsecure_modal_div);
	}
	
	this.construct_threedsecure_window = function(htmlval,htmllink)
    {
        document.getElementById("div3DSecure").innerHTML = '<input id="lbl3DSecure" type="hidden" /><input id="lbl3DSecureURL" type="hidden" />';
        document.getElementById('lbl3DSecure').value = htmlval;
        document.getElementById('lbl3DSecureURL').value = htmllink;
        show_frame_dialog("../Internet/Internet3DSecure.aspx", "internetClose()", "100%", "100%", "0");
    }
    
    this.close_threedsecure_modal_window = function()
	{
	    this.commons = new common_method();
        this.commons.to_show_hide_combobox(true);
        this.commons = null;

		if(this.outer_threedsecure_modal_div != null){document.body.removeChild(this.outer_threedsecure_modal_div);this.outer_threedsecure_modal_div  = null;}
		if(this.inner_threedsecure_modal_div != null){document.body.removeChild(this.inner_threedsecure_modal_div);this.inner_threedsecure_modal_div  = null;}
		if(this.content_threedsecure_iframe  != null){this.content_iframe = null;}
		close_frame_dialog(false);
		//if(this.content_iframe  != null){this.content_iframe = null;}

		document.body.style.margin = "";		
	}
	
	this.open_threedsecure_modal_window = function()
	{
        icrd.load_modal_css();

		this.outer_threedsecure_modal_div           = document.createElement("div");
		this.outer_threedsecure_modal_div.id        = "outer_threedsecure_mod_div";
		this.outer_threedsecure_modal_div.className = "outer_div";		
		
		document.body.style.margin = "0px";
		document.body.appendChild(this.outer_threedsecure_modal_div);
		document.getElementById("outer_threedsecure_mod_div").style.height = screen.height;		
		
		this.construct_threedsecure_div_content();

        this.commons = new common_method();
        this.commons.to_show_hide_combobox(false);
        this.commons = null;
	}

    this.commit_transaction_failed_callback = function(result)
    {
        if (result != null)
        {
            eval("javascript:push_card_payment_commit_response('0~FAILED TO UPDATE CARD PAYMENT DETAILS');");
        }
    }

    this.commit_transaction = function()
    {
        if (this.otc_card_payment == true)
        {
            if (this.response_all == null)
            {
                IRSWorld.TranClass.rem_card_screen.TransactionError(this.transaction_refno); 
                //Added by Troy 20120629
                if (this.dccancel == true)
                {
                    IRSWorld.TranClass.rem_card_screen.commit_transactions(String(this.txn_seqid), this.card_seqid, this.message, this.response,this.auth_code,true, this.commit_transaction_callback);
                }
                return;
            }
        
            if(this.response_all.indexOf('OK') < 0)
            {
                //IRSWorld.TranClass.rem_card_screen.TransactionError(this.transaction_refno, this.commit_transaction_failed_callback);
                IRSWorld.TranClass.rem_card_screen.TransactionError(this.transaction_refno); 
                //Added by Troy 20120629
                if (this.dccancel == true)
                {
                    IRSWorld.TranClass.rem_card_screen.commit_transactions(String(this.txn_seqid), this.card_seqid, this.message, this.response,this.auth_code,true, this.commit_transaction_callback);
                }
                return;
            }
            else
            {        
                //IRSWorld.TranClass.rem_card_screen.commit_transactions(this.transaction_refno, this.card_seqid, this.message, this.response,this.auth_code,true, this.commit_transaction_callback);
                IRSWorld.TranClass.rem_card_screen.commit_transactions(String(this.txn_seqid), this.card_seqid, this.message, this.response,this.auth_code,true, this.commit_transaction_callback);
            }
            
             return;
        }

        if(this.response_all.indexOf('OK') < 0)
        {
            IRSWorld.TranClass.rem_card_screen.TransactionError(this.transaction_refno);
        }

        IRSWorld.TranClass.rem_card_screen.commit_transactions(this.transaction_refno, this.card_seqid, this.message, this.response,this.auth_code,true, this.commit_transaction_callback);
    }
     
    this.commit_transaction_callback = function(response)
    {
        if (response.error != null && this.dccancel == false)
        {
            eval("javascript:push_card_payment_commit_response('0~FAILED TO UPDATE CARD PAYMENT DETAILS');");
        }
        else
        {
            if (response.value.toString() == "1")
            {
                eval("javascript:push_card_payment_commit_response('1');");
            }
            else
            {
                var s = '0~' + response.value; eval("javascript:push_card_payment_commit_response(s);");
            }
        }
    }
    
    this.populate_rem_info = function()
    {  
        IRSWorld.TranClass.rem_card_screen.getRemName(this.cifid, this.populate_rem_info_callback);
    }
    
    this.populate_rem_info_callback = function(response)
    {  
        if(response.error != null)
        {
            alert("NO DATASET RETURNED."); 
        }
        else
        {
            var dt = response.value;

            setText("lblCARDCIFID",     (dt.Rows[0].CIFID      != null) ? dt.Rows[0].CIFID.toString()      : "");
            setText("lblCARDCustLName", (dt.Rows[0].LastName   != null) ? dt.Rows[0].LastName.toString()   : "");
            setText("lblCARDCustFName", (dt.Rows[0].FirstName  != null) ? dt.Rows[0].FirstName.toString()  : "");
            setText("lblCARDCustMName", (dt.Rows[0].MiddleName != null) ? dt.Rows[0].MiddleName.toString() : "");
        }    
    }
    
    this.populate_drop_down_list = function() 
    {
        IRSWorld.TranClass.rem_card_screen.getDDLData(this.populate_drop_down_list_Callback);  
    }
   
    this.populate_drop_down_list_Callback = function(response)
    {
        if(response.error != null)
        {
            alert("NO DATASET FOUND."); 
        }
        else
        {
            var ds = response.value;
            var commons = new common_method();
                        
            commons.to_load_dropdownlist('ddCardType',null, 'Value', 'Text', ds.Tables[1]);            
            commons = null;
        }
    }
    
    this.process_viewer = function(param)
    { 
        this.grid = new grid_viewer();       
        this.grid.sql_source     = "sp_rem_tran_grid_cards~" + this.cifid + "~";
        this.grid.div_container  = document.getElementById("CARDACCOUNTGRID");
        this.grid.datarow_column = 0;
        this.grid.page_size      = 10;
        this.grid.process_ajax(param);                               
        this.grid = null;
    }
    
    this.do_ajaxgridview = function(parameter)
    {
        if (parameter != null && parameter != "undefined")
        {
            var command_param = parameter.split("$");

            if (command_param.length > 0)
            {
                if (command_param[0] == "ajx_selected")
                {
                    this.card_seqid =  command_param[1];
                    this.populateInfo(command_param[1]);

                    return;
                }
               else
               {
                    this.process_viewer(parameter);
               }  
            }
        }
    }
    
    this.populateInfo = function(account_seqid)
    {        
        IRSWorld.TranClass.rem_card_screen.getCardInfo(account_seqid, this.populateInfo_Callback);  
    }  
   
    this.populateInfo_Callback = function(response)
    {
        icrd.generic = new generic_method();

        if (response.error != null)
        {
            alert("NO DATASET FOUND.");
        }
        else
        {
            var dt = response.value;

            if (dt != null && dt.Tables[0].Rows.length > 0)
            {
                var commons = new common_method();
                
                icrd.card_name     = (dt.Tables[0].Rows[0].CardName     == null) ? "" : dt.Tables[0].Rows[0].CardName.toString();
                icrd.card_class    = (dt.Tables[0].Rows[0].CardClass    == null) ? "" : dt.Tables[0].Rows[0].CardClass.toString();
                icrd.issuedate     = (dt.Tables[0].Rows[0].IssueDate    == null) ? "" : icrd.generic.to_mmyyyy_date(dt.Tables[0].Rows[0].IssueDate).replace('/','');
                icrd.expirydate    = (dt.Tables[0].Rows[0].ExpiryDate   == null) ? "" : icrd.generic.to_mmyyyy_date(dt.Tables[0].Rows[0].ExpiryDate).replace('/','');
                icrd.security_code = (dt.Tables[0].Rows[0].SecurityCode == null) ? "" : dt.Tables[0].Rows[0].SecurityCode.toString();
                icrd.country       = (dt.Tables[0].Rows[0].Country      == null) ? "" : dt.Tables[0].Rows[0].Country.toString();                
                icrd.card_number   = (dt.Tables[0].Rows[0].CardNo       == null) ? "" : dt.Tables[0].Rows[0].CardNo.toString();

                setText("txtCardNumber", icrd.convert_card_no(icrd.card_number) + icrd.card_number.substring(icrd.card_number.length,icrd.card_number.length - 4));
                document.getElementById("txtaResponse").value = ""; //Added by Troy 20120629
                document.getElementById("btnCARDOK").disabled = ""; //Added by Troy 20120810

                commons.to_set_dropdownlist_index('ddCardType', ((dt.Tables[0].Rows[0].CardType == null) ? "" : dt.Tables[0].Rows[0].CardType.toString()));                
                commons = null;
                
                icrd.set_variable();
            }
        }
    }
    
    this.writeLog = function(message)
    {
        IRSWorld.TranClass.RemTranExtra.writeLog(message);
    }
    
    this.btn_CARD_cancel_click = function()
    {
        if (this.otc_card_payment == true)
        {
            this.close_modal_window(); 
            return;
        }
    }
    
    this.btn_CARD_close_click = function()
    {
        if (this.otc_card_payment == true)
        {
            if (this.response_all != null && this.response_all != undefined) //Edited by Troy 20120629
            {
                if(this.response_all.indexOf('OK') < 0)
                {
                    if (confirm("Debiting from remitter's card was unsuccessful. Are you sure you want to close this form w/out debiting the client?"))
                    {
                        this.writeLog("Debiting from client card was canceled by the teller due to " + this.response_all);
                        this.dccancel = true;
                        this.close_modal_window(); 
                        this.iremit_object.is_debit_success(true, icrd);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    this.close_modal_window(); 
                    this.iremit_object.is_debit_success(true, icrd);
                }
            }
            else
            {
                if (confirm("Are you sure you want to close this form w/out debiting the client?"))
                {
                    this.writeLog("Debiting from client card was canceled by the teller due to " + this.response_all);
                    this.dccancel = true;
                    this.close_modal_window(); 
                    this.iremit_object.is_debit_success(true, icrd);
                }
            }

            return;
        }
    }
    
    this.card_code_validate_submit = function()
    {
        this.security_code = document.getElementById('txt_security_code').value;
    
        if (this.card_number == null || this.card_number == '')
        {
            alert("Card not found."); this.close_security_modal_window(); return;
        }
    
        if (this.security_code == null || this.security_code == '')
        {
            alert("Please enter security code."); return;
        }

        document.getElementById('btn_card_validate').disabled  = true;
        document.getElementById('btn_card_cancelled').disabled = true;
        
        AjaxPro.timeoutPeriod = 1000 * 1000;

        AjaxPro.onTimeout = function()
        {
            alert("Server not responding. Unable to connect to payment gateway.");
        }
        this.open_msg_prompt("Connecting to payment gateway, Please wait ...");
        
        IRSWorld.TranClass.rem_card_screen.submit_payment_gateway(this.cifid,this.card_number,this.card_name,this.expirydate,this.security_code,this.card_type,this.rem_currencycode,this.transaction_amount,'PNB IRSW Payment Gateway', this.card_code_validate_submit_callback); //Edited by Troy 20120628
        //IRSWorld.TranClass.rem_card_screen.submit_payment_gateway(this.cifid,this.card_number,this.card_name,this.expirydate,this.security_code,this.card_type,this.rem_currency,this.transaction_amount,'PNB IRSW Payment Gateway', this.card_code_validate_submit_callback);
    }
    
    this.card_code_validate_submit_callback = function(param)
    {
        icrd.close_msg_prompt();
        if (param != null)
        {
            if (param.error != null)
            {
                alert("Error encountered while connecting to payment gateway.");

                document.getElementById('btn_card_validate').disabled  = false;
                document.getElementById('btn_card_cancelled').disabled = false;

                return;
            }
            else
            {
                var response_array = param.value.split('|');
                var authorization  = param.value.substring(param.value.indexOf('TxAuthNo'),param.value.indexOf('AVSCV2')).replace('\n','').replace('\r','').split('=');
                
                icrd.response_all = param.value;
                icrd.response     = icrd.response_string(param.value);
                icrd.message      = response_array[2];
                
                document.getElementById('txtaResponse').value = "Auth Code: " + authorization[1] + "\r\n" + "Payment Gateway Response: " + icrd.message; //Edited by Troy 20120629
                
                if (response_array[0] == "OK")
                {
                     icrd.auth_code = authorization[1];
                     document.getElementById("btnCARDOK").disabled = "disabled"; //Added by Troy 20120629
                     document.getElementById("btnCARDCANCEL").disabled = "disabled"; //Added by Troy 20120809
                     document.getElementById("btnCARDCLOSE").disabled = ""; //Added by Troy 20120810
                }
                else
                {
                     document.getElementById("btnCARDOK").disabled = "disabled"; //Added by Troy 20120810
                     document.getElementById("btnCARDCANCEL").disabled = ""; //Added by Troy 20120810
                     document.getElementById("btnCARDCLOSE").disabled = "disabled"; //Added by Troy 20120810
                }
                if (response_array[0] == "3DSECURE")
                {
                    icrd.threedsecure_form(response_array[1],response_array[2]);
                }

                icrd.close_security_modal_window();
            }
        }
    }
    
    this.btn_CARD_ok_click = function()
    {
        document.getElementById('btnSubmitTransaction').disabled = true;
        document.getElementById('btnCancelTransaction').disabled = true;
        this.showPGLoading('Connecting to Payment Gateway... Please wait',false);
        this.set_variable();
        this.security_code = document.getElementById('txtSecurityCode').value;
        
        if(this.card_number == null || this.card_number == '')
        {
            alert('Please select a card.');return;
        }
        else
        {
            IRSWorld.TranClass.rem_card_screen.submitPaymentGateway('',this.cifid,this.card_number,this.card_name,this.expirydate,this.security_code,this.card_type,this.rem_currency,this.transaction_amount,'PNB IRSW Payment Gateway', this.btn_CARD_ok_click_callback);  
        }
    }
    
    this.btn_CARD_ok_click_callback = function(param)
    {
        if(param.error != null)
        {            
            alert('Failed to connect to Payment Gateway');
        }
        else
        {
            icrd.showPGLoading('',true);
            document.getElementById('txtaResponse').value = param.value;
            icrd.response_all = param.value
            icrd.response = icrd.response_string(param.value);
            icrd.close_security_modal_window();
            var rtn = document.getElementById('txtaResponse').value.split('|');
            icrd.message = rtn[2];
            document.getElementById('btnCARDOK').disabled = true;
            if(rtn[0] == 'OK')
            {
                var authorization = param.value.substring(param.value.indexOf('TxAuthNo'),param.value.indexOf('AVSCV2')).replace('\n','').replace('\r','').split('=');
                icrd.auth_code = authorization[1];
            }
            else if(rtn[0] == '3DSECURE')
            {
                icrd.threedsecure_form(rtn[1],rtn[2]);                
            }
        }
    }
    
    this.showPGLoading = function(message, ishide)
    {
        var span = document.getElementById("spnPGLoading");
        if (ishide == true)
        {
            span.innerHTML = "";
        }
        else
        {
            span.innerHTML = "<IMG SRC = '../images/ajaxloader.gif'></IMG>&nbsp;" + message;
        }
    }
    
    this.convert_card_no = function(cardno)
    {
        var sConvert = '';
        for (var i = 0; i < cardno.length - 4; i++)
        {
            sConvert += '*';
        }
        return sConvert;
    }
    
    this.response_string = function(response)
    {
        var rsp = response.split('|');
        var rspReturn = '';
        for (var i = 0; i < rsp.length ; i++)
        {
            if(i >= '3')
            {
                rspReturn += rsp[i] + '|';
            }
        }
        return rspReturn;
    }
    
    this.set_variable = function()
    {
        this.card_type     = document.getElementById('ddCardType')[document.getElementById('ddCardType').selectedIndex].text;        
        this.message       = null;
    }
}
/* END DEBIT CREDIT CARD PAYMENT SCREEN */

/* 18.) DEBIT TO PNB ACCOUNT SCREEN */
function rem_modal_debit_account_payments()
{
    var dacc = this;
    
    this.transaction_amount   = 0;

    this.debit_account_number = null;    
    this.debit_account_name   = null;    
    this.cifid                = null;    
    this.transaction_id       = null;
    this.transaction_refno    = null;
    this.debit_account_seqid  = null;
    this.ref                  = null;
    this.rem_currency         = null;
    this.debit_account_type   = null;
    this.debit_account_class  = null;    
    this.generic              = null;
    
    this.page_onload = function()
    {
        IRSWorld.TranClass.rem_debit_account_screen.has_debit_accounts(this.cifid, this.page_onload_callback);
    }
    
    this.page_onload_callback = function(response)
    {
        if (response.error != null)
        {
            alert("No dataset returned");
        }
        else
        {
            if (response.value == true)
            {
                dacc.process_viewer('ajx_loading$');
            }
            
            dacc.populate_rem_info();
            dacc.populate_drop_down_list();
            
            if (dacc.debit_account_seqid != null && dacc.debit_account_seqid != '' && dacc.debit_account_seqid != 'undefined')
            {
                 dacc.populateInfo(dacc.debit_account_seqid);
            }
        }
    }
    
    this.debit_account_form = function(CIFID, trxAmount, ref)
    {
        this.cifid = CIFID;
        this.transaction_amount = trxAmount;
        this.ref = ref;        
        this.open_modal_window();
        this.construct_override_window();
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
	
	this.construct_div_content = function()
	{
	    var content_div = null;
	    
		content_div                       = document.createElement("div");		
		content_div.id                    = "content_mod_div";
		content_div.align                 = "left";
		content_div.style.background      = "transparent";
		content_div.style.width           = "100%";
		content_div.style.overflow        = "auto";
		content_div.innerHTML             = "<center><img src = '.././images/ajaxloader.gif'></img></center>";	    

		this.inner_modal_div              = document.createElement("div");		
		this.inner_modal_div.align        = "center";
		this.inner_modal_div.className    = "inner_div";		
		this.inner_modal_div.id           = "inner_mod_div";
		this.inner_modal_div.style.height = "auto";
		this.inner_modal_div.style.width  = "600px";
		this.inner_modal_div.style.top    = "30px";
		this.inner_modal_div.style.left   = (parseInt(screen.width) - parseInt(this.inner_modal_div.style.width)) / 2 - 10 + "px";
		this.inner_modal_div.style.border = "medium solid white";		
		this.inner_modal_div.Code         = this;
		this.inner_modal_div.appendChild(content_div);
		
		document.body.appendChild(this.inner_modal_div);
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
	
	this.load_modal_css = function()
	{	    
		var file_reference = document.createElement("link");
		
	  	file_reference.setAttribute("rel", "stylesheet");
	  	file_reference.setAttribute("type", "text/css");
	  	file_reference.setAttribute("href", ".././css/cssmodals.css");

		if (typeof file_reference != "undefined")
		{		    
	  		document.getElementsByTagName("head")[0].appendChild(file_reference);
		}
	}
	
	this.construct_override_window = function()
    {
        if (this.cifid != null && this.cifid != "undefined" && this.cifid != "" && this.transaction_amount != null && this.transaction_amount != "undefined" && this.transaction_amount != 0)
        {            
            var html_string = "";
            
            html_string += "<center><table style='width: 600px; border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid'>";
            html_string += "    <tr>";
            html_string += "        <td>";
            html_string += "            <div id = 'DACCTCIFINFO' style= 'display:block'>";
            html_string += "                <table width='100%' cellpadding='0'>";
            html_string += "                    <tr>";
            html_string += "                        <td class='cell_desc' style='width: 10%; height: 24px; text-align: right;'>CIF ID :&nbsp;</td>";
            html_string += "                        <td class='cell_desc' style='width: 20%; height: 24px; text-align: left;'colspan='4'><label id='lblDACCTCIFID' class='cell_desc'></label></td>";
            html_string += "                        <td class='cell_desc' style='width: 19%; height: 24px; text-align: right;'>Remitter Name :</td>";
            html_string += "                        <td align='left'      style='width: 61%; height: 24px;' colspan='4'>";
            html_string += "                            <label id='lblDACCTCustFName' class='cell_desc'></label>&nbsp;";
            html_string += "                            <label id='lblDACCTCustMName' class='cell_desc'></label>&nbsp;";
            html_string += "                            <label id='lblDACCTCustLName' class='cell_desc'></label>&nbsp;";
            html_string += "                        </td>";
            html_string += "                    </tr>";
            html_string += "                </table>";
            html_string += "            </div>";
            html_string += "            <div id = 'DACCTACCOUNTGRID' style= 'display:block'>";
            html_string += "                <table id='Table10' width='100%' cellpadding='0' cellspacing='5' style='border: solid 1px #336699; background:azure;'>";
            html_string += "                    <tr>";
            html_string += "                        <td style='width: 100%; text-align: left; text-indent: 5px; color: Black; height: 19px;' colspan='5'></td>";
            html_string += "                    </tr>";
            html_string += "                    <tr>";
            html_string += "                        <td style='height: 2px'><div id = 'Div2' style= 'display:none'>  PLACE GRIDVIEW ID'S</div></td>";
            html_string += "                    </tr>";
            html_string += "                </table>";
            html_string += "            </div>";
            html_string += "            <div id = 'DACCOUNTINFO' style = 'overflow:auto'>";
            html_string += "                <table width='100%' cellpadding='0' cellspacing='3'>";
            html_string += "                    <tr>";
            html_string += "                        <td class='cell_caption' style='width: 15%; height:24px; text-align: right;'>Account Number :&nbsp;</td>";
            html_string += "                        <td align='left' style='width:30%; height: 24px;' class='cell_desc'><input id='txtDAcctNumber' type='text' style = 'width:88%' maxlength='50' disabled/></td>";
            html_string += "                    </tr>";
            html_string += "                    <tr>";
            html_string += "                        <td class='cell_caption' style='width: 15%; height:24px; text-align: right;'>Account Type :&nbsp;</td>";
            html_string += "                        <td align='left' style='width:30%; height: 24px;' class='cell_desc'><select id='ddDAcctType' style = 'width:88%' disabled><option selected='selected'></option></select></td>";
            html_string += "                    </tr>";
            html_string += "                    <tr>";
            html_string += "                        <td class='cell_caption' style='width: 15%; height:24px; text-align: right;'>Currency :&nbsp;</td>";
            html_string += "                        <td align='left' style='width:30%; height: 24px;' class='cell_desc'><select id='ddDAcctCurrency' style = 'width:88%' disabled><option selected='selected'></option></select></td>";
            html_string += "                    </tr>";
            html_string += "                    <tr>";
            html_string += "                        <td class='cell_caption' style='width: 15%; height:24px; text-align: right;'>Amount :&nbsp;</td>";
            html_string += "                        <td align='left' style='width:30%; height: 24px;' class='cell_desc'><input id='txtAmount' type='text' style = 'width:88%' maxlength='50' disabled/></td>";
            html_string += "                    </tr>";
            html_string += "                </table>";
            html_string += "                <table width='100%' cellpadding='0' cellspacing='5' style='background:azure;'>";
            html_string += "                    <tr>";
            html_string += "                        <td width='20%' style='height: 20px' align='right'>";
            html_string += "                            <input style='width:150px; height: 25px;' id='btnDACCTOK'     type='button' value='OK'    onclick='javascript:" + this.ref + ".btn_DACCT_ok_click();'/>&nbsp;";
            html_string += "                            <input style='width:150px; height: 25px;' id='btnDACCTCANCEL' type='button' value='CLOSE' onclick='javascript:" + this.ref + ".btn_DACCT_cancel_click();'/>&nbsp;";
            html_string += "                        </td>";
            html_string += "                    </tr>";
            html_string += "                </table>";
            html_string += "            </div>";
            html_string += "        </td>";
            html_string += "    </tr>";
            html_string += "</table></center>";

            document.getElementById("content_mod_div").innerHTML = html_string; this.page_onload();
       }
       else
       {
            this.close_modal_window(); alert('Parameter was not supplied.')
       }
    }
    
    this.commit_transaction = function()
    {
        var data =  "ReferenceNo~" + this.transaction_refno;
                
        data += "|CardNo~"   + ((this.debit_account_number  == null) ? ' ' : this.debit_account_number);        
        data += "|Currency~" + ((this.rem_currency          == null) ? ' ' : this.rem_currency);
        data += "|isCard~false";
        
        document.getElementById("btnDACCTOK").disabled = true;
        
        IRSWorld.TranClass.rem_debit_account_screen.commit_transactions(this.transaction_refno, this.debit_account_number, parseInt(this.rem_currency),this.transaction_amount, this.commit_transaction_callback);
    }
     
    this.commit_transaction_callback = function(response)
    {
        if (response.error != null)
        {
            eval("javascript:push_debit_account_payment_commit_response('0~FAILED TO UPDATE DEBIT ACCOUNT PAYMENT DETAILS');");
        }
        else
        {
            if (response.value.toString() == "1")
            {
                dacc.commit_transaction_details();
            }
            else
            {
                dacc.close_modal_window(); var s = '0~' + response.value; eval("javascript:push_debit_account_payment_commit_response(s);");
            }
        }
    }
    
    this.commit_transaction_details = function()
    {
        IRSWorld.TranClass.rem_debit_account_screen.commit_transaction_details(dacc.transaction_id, dacc.debit_account_seqid,dacc.commit_transaction_details_callback);
    }
    
    this.commit_transaction_details_callback = function(response)
    {
        if (response.error != null)
        {
            eval("javascript:push_debit_account_payment_commit_response('0~FAILED TO UPDATE DEBIT ACCOUNT PAYMENT DETAILS');");
        }
        else
        {
            if (response.value.toString() == "1")
            {
                dacc.close_modal_window(); eval("javascript:push_debit_account_payment_commit_response('1');");
            }
            else
            {
                dacc.close_modal_window(); var s = '0~' + response.value; eval("javascript:push_debit_account_payment_commit_response(s);");
            }
        }
    }
    this.populate_rem_info = function()
    {  
        IRSWorld.TranClass.rem_debit_account_screen.getRemName(this.cifid, this.populate_rem_info_callback);
    }
    
    this.populate_rem_info_callback = function(response)
    {  
        if(response.error != null)
        {
            alert("No dataset returned"); 
        }
        else
        {
            var dt = response.value; 

            setText(document.getElementById('lblDACCTCIFID')    , (dt.Rows[0].CIFID      != null) ? dt.Rows[0].CIFID.toString()      : "");
            setText(document.getElementById('lblDACCTCustLName'), (dt.Rows[0].LastName   != null) ? dt.Rows[0].LastName.toString()   : "");
            setText(document.getElementById('lblDACCTCustFName'), (dt.Rows[0].FirstName  != null) ? dt.Rows[0].FirstName.toString()  : "");
            setText(document.getElementById('lblDACCTCustMName'), (dt.Rows[0].MiddleName != null) ? dt.Rows[0].MiddleName.toString() : "");                           
        }    
    }
    
    this.populate_drop_down_list = function() 
    {
        IRSWorld.TranClass.rem_debit_account_screen.getDDLData(this.populate_drop_down_list_Callback);  
    }
   
    this.populate_drop_down_list_Callback = function(response)
    {
        if(response.error != null)
        {
            alert("NO DATASET FOUND."); 
        }
        else
        {
            var ds = response.value;
            var commons = new common_method();
                        
            commons.to_load_dropdownlist('ddDAcctType',null, 'Value', 'Text', ds.Tables[0]);
            commons.to_load_dropdownlist('ddDAcctCurrency',null, 'Value', 'Text', ds.Tables[1]);
            commons = null;
        }
    }
    
    this.process_viewer = function(param)
    { 
        this.grid = new grid_viewer();       
        this.grid.sql_source     = "sp_cif_grid_accounts~" + this.cifid + "~";
        this.grid.div_container  = document.getElementById("DACCTACCOUNTGRID");
        this.grid.datarow_column = 0;
        this.grid.page_size      = 10;
        this.grid.process_ajax(param);                               
        this.grid = null;
    }
    
    this.do_ajaxgridview = function(parameter)
    {
        if (parameter != null && parameter != "undefined")
        {
            var command_param = parameter.split("$");

            if (command_param.length > 0)
            {
                if (command_param[0] == "ajx_selected")
                {
                    this.debit_account_seqid =  command_param[1];
                    this.populateInfo(command_param[1]);
                    
                    return;
                }
               else
               {
                    this.process_viewer(parameter);
               }  
            }
        }
    }
    
    this.populateInfo = function(account_seqid)
    {        
        IRSWorld.TranClass.rem_debit_account_screen.getDAcctInfo(account_seqid, this.populateInfo_Callback);  
    }  
   
    this.populateInfo_Callback = function(response)
    {
        dacc.generic = new generic_method();

        if (response.error != null)
        {
            alert("NO DATASET FOUND.");
        }
        else
        {
            var dt = response.value;

            if (dt != null && dt.Tables[0].Rows.length > 0)
            {
                var commons = new common_method();
                
                dacc.rem_currency = (dt.Tables[0].Rows[0].Currency == null) ? "" : dt.Tables[0].Rows[0].Currency.toString();
                
                setText(document.getElementById('txtDAcctNumber'), (dt.Tables[0].Rows[0].AccountNo == null) ? "" : dt.Tables[0].Rows[0].AccountNo.toString());                
                
                commons.to_set_dropdownlist_index('ddDAcctCurrency', dacc.rem_currency);
                commons.to_set_dropdownlist_index('ddDAcctType',     ((dt.Tables[0].Rows[0].AccountType == null) ? "" : dt.Tables[0].Rows[0].AccountType.toString()));                
                commons = null;                
                
                dacc.set_variable();
            }
        }
    }
    
    this.btn_DACCT_cancel_click = function()
    {
         if(confirm('Are you sure you want to cancel debit to account payment?'))
         {
            this.close_modal_window(); eval("javascript:push_debit_account_payment_commit_response('0~FAILED TO UPDATE DEBIT ACCOUNT PAYMENT DETAILS');");
         }
    }
    
    this.btn_DACCT_ok_click = function()
    {
        this.set_variable();
        
        if(this.debit_account_number == null || this.debit_account_number == '')
        {
            alert('Please select account.');return;
        }
        else
        {
            eval("javascript:push_debit_account_payment_result();");            
        }
    }
    
    this.btn_DACCT_ok_click_callback = function(param)
    {
        if(param.error != null)
        {
            alert('FAILED TO CONNECT TO PAYMENT GATEWAY');
        }
    }

    this.set_variable = function()
    {
        this.debit_account_number = document.getElementById('txtDAcctNumber').value;
        this.debit_account_type   = document.getElementById('ddDAcctType')[document.getElementById('ddDAcctType').selectedIndex].text;

        setText(document.getElementById('txtAmount'), IRSWorld.TranClass.rem_debit_account_screen.computeAmount(dacc.transaction_amount,'1',dacc.rem_currency).value);
    }
}
/* END DEBIT TO PNB ACCOUNT SCREEN */

/* 19.) START FOR OTC BATCH SUMMARY*/

function otc_show_batch_summary()
{
    var icls = this;
    
    this.outer_modal_div = null;
    this.inner_modal_div = null;
    this.commons         = null;

    this.open_batch_summary_screen = function()
    {      
        this.open_modal_window();
        this.load_batch_data();
    }

    this.construct_batch_summary_screen = function(sarr)
    {
        var html_string = "";
                
        html_string +=  '<center>'
        html_string +=  "<table id = 'tbl_otc_batch_summary' style='border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; width: 400px; border-bottom: #336699 1px solid' cellpadding='0' cellspacing='1'>"
        html_string +=  '   <tr>'
        html_string +=  '       <td style="width: 30%; " align="left" class="cell_caption" nowrap>&nbsp; BATCH NUMBER:</td>'
        html_string +=  '       <td align="left" colspan="2" style="width:60%;text-align:left;" ><label class="cell_desc"  align="right" id="txtviewbatchnum" /></td>'
        html_string +=  '   </tr>'
        html_string +=  '   <tr>'
        html_string +=  '       <td style="width: 30%; height: 21px;" align="left">&nbsp; </td>'        
        html_string +=  '       <td align="left" style="width:35%; text-align: center;" class="cell_caption">AMOUNT</td>'        
        html_string +=  '       <td align="left" class="cell_caption" style="width:35%; text-align: center;">ITEMS</td>'        
        html_string +=  '   </tr>'
        html_string +=  '   <tr>'
        html_string +=  '       <td align="left" class="cell_caption" style="width: 30%;">BATCH TOTAL:</td>'        
        html_string +=  '       <td align="left" class="cell_desc" style="width:35%;text-align:right;"><label id="txtviewbatchtotal" /></td>'
        html_string +=  '       <td align="left" class="cell_desc" style="width:35%;text-align:center;"><label id="txtviewbatchitems" /></td>'
        html_string +=  '   </tr>'
        html_string +=  '   <tr>'
        html_string +=  '       <td align="left" class="cell_caption" style="width: 30%; ">COMPUTED:</td>'        
        html_string +=  '       <td align="left" class="cell_desc" style="width:35%;text-align:right;" ><label id="txtviewcompamt" /></td>'
        html_string +=  '       <td align="left" class="cell_desc" style="width:35%;text-align:center;"><label id="txtviewcompitems" /></td>'
        html_string +=  '   </tr>'
        html_string +=  '   <tr>'
        html_string +=  '       <td align="left" class="cell_caption" style="width: 30%;">DIFFERENCE:</td>'        
        html_string +=  '       <td align="left" class="cell_desc" style="width:35%;text-align:right;"><label id="txtviewdiffamt"  /></td>'
        html_string +=  '       <td align="left" class="cell_desc" style="width:35%;text-align:center;"><label id="txtviewdiffitems" /></td>'
        html_string +=  '   </tr>'
        html_string +=  '   <tr>'
        html_string +=  '       <td align="center" colspan="3" style="width:100%"><input id="btn_otc_batch_summary_ok" style="width: 17%" type="button" value="OK" /></td>'        
        html_string +=  '   </tr>'
        html_string +=  '</table>'
        html_string +=  '</center>'
        
        document.getElementById("content_mod_div").innerHTML = html_string;
        
        setText("txtviewbatchnum",      sarr[0]);            
        setText("txtviewbatchtotal",    sarr[1]);
        setText("txtviewbatchitems",    sarr[2]);
        setText("txtviewcompamt",       sarr[3]);
        setText("txtviewcompitems",     sarr[4]);
        setText("txtviewdiffamt",       sarr[5]);
        setText("txtviewdiffitems",     sarr[6]);               

        document.getElementById("btn_otc_batch_summary_ok").onclick = function()
        {
            icls.close_modal_window();
        }
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
	
    this.load_batch_data = function()
    {
        IRSWorld.TranClass.RemTranExtra.load_batch_data(icls.load_batch_data_callback)
    }

    this.load_batch_data_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            var s    = result.value.toString();
            var sarr = s.split("|");

            icls.construct_batch_summary_screen(sarr); return;
        }

        alert(result.error);
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

        content_div                       = document.createElement("div");		
		content_div.id                    = "content_mod_div";
		content_div.align                 = "left";
		content_div.style.background      = "transparent";
		content_div.innerHTML             = "<center><img src = '.././images/ajaxloader.gif'></img></center>";

		this.inner_modal_div              = document.createElement("div");		
		this.inner_modal_div.align        = "center";
		this.inner_modal_div.className    = "inner_div";		
		this.inner_modal_div.id           = "inner_mod_div";
		this.inner_modal_div.style.height = "auto";		
		this.inner_modal_div.style.width  = "400px";
		this.inner_modal_div.style.top    = "30px";
		this.inner_modal_div.style.left   = (parseInt(screen.width) - parseInt(this.inner_modal_div.style.width)) / 2 - 10 + "px";
		this.inner_modal_div.style.border = "medium solid white";
		this.inner_modal_div.appendChild(content_div);
		
		document.body.appendChild(this.inner_modal_div);
	}

	this.load_modal_css = function()
	{
		var file_reference = document.createElement("link");

	  	file_reference.setAttribute("rel", "stylesheet");
	  	file_reference.setAttribute("type", "text/css");
	  	file_reference.setAttribute("href", ".././css/cssmodals.css");

		if (typeof file_reference != "undefined")
		{		    
	  		document.getElementsByTagName("head")[0].appendChild(file_reference);
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
	        if (divelement_outer.item(i).id == "outer_mod_div")
	        {
	            divelement_outer.item(i).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop;break;
	        }	        
	    }
	    for(m = 0; m < divelement_inner.length;m++)
	    {
	        if (divelement_inner.item(m).id == "inner_mod_div")
	        {
	            divelement_outer.item(m).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop + 20 : document.body.scrollTop + 20;break;
	        }
	    }
	}
}
/*END FOR OTC BATCH SUMMARY*/

/* 20.) DENOMINATION OF AMOUNT */
function rem_modal_denomination()
{
    var icls = this;
    this.SEQ_ID             =   null;
    this.TenThou            =   null;
    this.FiveThou           =   null;
    this.TwoThou            =   null;
    this.OneThou            =   null;
    this.FiveHund           =   null;
    this.TwoHund            =   null;
    this.OneHund            =   null;
    this.Fifty              =   null;
    this.Twenty             =   null;
    this.Ten                =   null;
    this.Five               =   null;
    this.Two                =   null;
    this.One                =   null;
    this.Coins              =   null;
    this.CK_MO_OTH          =   null;
    this.ATM_DEBIT          =   null;
    this.OTHERS             =   null;
    this.Details            =   null;
    this.ref                =   null;
    this.Total_Amt          =   null;
    this.Total_Denom        =   null;
    this.Total_Change       =   null;
   
    this.page_onload = function()
    {       
        this.populate_info();
    }
    
    this.reload = function()
    {
        this.populate_info();
    }
    
    this.commit_transaction = function()
    {
        var data =  "";
        data += "REM_SEQ_ID~"  + this.SEQ_ID;
        data += "|TenThou~"     + this.TenThou;
        data += "|FiveThou~"    + this.FiveThou;
        data += "|TwoThou~"     + this.TwoThou;
        data += "|OneThou~"     + this.OneThou;
        data += "|FiveHund~"    + this.FiveHund;
        data += "|TwoHund~"     + this.TwoHund;
        data += "|OneHund~"     + this.OneHund;
        data += "|Fifty~"       + this.Fifty;
        data += "|Twenty~"      + this.Twenty;
        data += "|Ten~"         + this.Ten;
        data += "|Five~"        + this.Five;
        data += "|Two~"         + this.Two;
        data += "|One~"         + this.One;
        data += "|Coins~"       + this.Coins;
        data += "|CK_MO_OTH~"   + this.CK_MO_OTH;
        data += "|ATM_DEBIT~"   + this.ATM_DEBIT;
        data += "|OTHERS~"      + this.OTHERS;
        data += "|Total_Amt~"   + this.Total_Amt;
        data += "|Details~"     + this.Details;
        data += "|Total_Change~" + this.Total_Change;
        
        AjaxPro.timeoutPeriod = 1000 * 1000;
        
        AjaxPro.onTimeout = function()
        {
            alert("Server not responding. cash denomination failed to commit. Please try again.");
            this.disable_cd_buttons(false);
            document.getElementById('trCDLoadingNotification').style.display = 'none';
        }        
        
        IRSWorld.TranClass.rem_modal_denomination.commit_transaction(data, this.commit_transaction_callback);
    }
     
    this.commit_transaction_callback = function(response)
    {
        //icls.close_modal_window();
        
        if (response.error != null)
        {
            //eval("javascript:push_denomination_commit_response('0~FAILED TO SAVE CASH DENOMINATION INFORMATION.');");
            icls.disable_cd_buttons(false);
            document.getElementById('trCDLoadingNotification').style.display = 'none';
            alert('FAILED TO SAVE CASH DENOMINATION INFORMATION.\nREASON : ' + response.error.Message);
        }
        else
        {
            if (response.value.toString() == "1")
            {
                document.getElementById('trCDLoadingNotification').style.display = 'none';
                alert('SUCCESSFULLY SAVED CASH DENOMINATION.');   
                icls.close_modal_window();
                eval("javascript:push_denomination_commit_response('1');");
            }
            else
            {
                //eval("javascript:push_denomination_commit_response('0~" + response.value + "');");
                //eval("javascript:push_denomination_commit_response('0~FAILED TO SAVE CASH DENOMINATION INFORMATION.');");
                document.getElementById('trCDLoadingNotification').style.display = 'none';
                icls.disable_cd_buttons(false);
                alert('FAILED TO SAVE CASH DENOMINATION INFORMATION.\nREASON : ' +  response.value.toString());
            }    
        }       
    }
    
    this.compute = function()
    {
        setText("lblTotalCash", this.get_totalCash());
        setText("lblTotalChecks", document.getElementById("txtCK_MO_OTH").value);
        setText("lblTotalATMDebit", document.getElementById("txtATM_DEBIT").value);
        setText("lblTotalOthers", document.getElementById("txtAMOTHERS").value);
        
        var totalDenum = 0;
        totalDenum += this.parseFloat2(getText("lblTotalCash"));
        totalDenum += this.parseFloat2(getText("lblTotalChecks"));
        totalDenum += this.parseFloat2(getText("lblTotalATMDebit"));
        totalDenum += this.parseFloat2(getText("lblTotalOthers")); 
        
        this.Total_Denom = totalDenum;
        var totalChange = this.parseFloat2(getText("lblGrandTotal")) - totalDenum;
        
        setText("lblChange", totalChange.toString());
        setText("lblTotal", totalDenum.toString());
    }
    
    this.get_totalCash = function()
    {
        var totalCash = 0;
        totalCash   +=  this.parseFloat2(document.getElementById("txtTenThou").value) * 10000;
        totalCash   +=  this.parseFloat2(document.getElementById("txtFiveThou").value) * 5000;
        totalCash   +=  this.parseFloat2(document.getElementById("txtTwoThou").value) * 2000;
        totalCash   +=  this.parseFloat2(document.getElementById("txtOneThou").value) * 1000;
        totalCash   +=  this.parseFloat2(document.getElementById("txtFiveHund").value) * 500;
        totalCash   +=  this.parseFloat2(document.getElementById("txtTwoHund").value) * 200;
        totalCash   +=  this.parseFloat2(document.getElementById("txtOneHund").value) * 100;
        totalCash   +=  this.parseFloat2(document.getElementById("txtFifty").value) * 50;
        totalCash   +=  this.parseFloat2(document.getElementById("txtTwenty").value) * 20;
        totalCash   +=  this.parseFloat2(document.getElementById("txtTen").value) * 10;
        totalCash   +=  this.parseFloat2(document.getElementById("txtFive").value) * 5;
        totalCash   +=  this.parseFloat2(document.getElementById("txtTwo").value) * 2;
        totalCash   +=  this.parseFloat2(document.getElementById("txtOne").value) * 1;
        totalCash   +=  this.parseFloat2(document.getElementById("txtCoins").value);
        
        return totalCash;
    }
    
    this.parseInt2 = function(NumWord)
    {   
        var num = parseInt(((NumWord == 'NaN' | NumWord == NaN | NumWord == null | NumWord == 'null' | NumWord == '') ? '0' : NumWord));
        return num;
    }
    
    this.parseFloat2 = function(NumWord)
    {
        var num = parseFloat(((NumWord == 'NaN' | NumWord == NaN | NumWord == null | NumWord == 'null' | NumWord == '') ? '0' : NumWord));
        return num;    
    }
    
    this.isNumeric = function(NumWord)
    {
        try {return !isNaN(((NumWord == 'NaN' | NumWord == NaN | NumWord == null | NumWord == 'null' | NumWord == '') ? '0' : NumWord));}
        catch(e){}
        return false;
    }
        
    this.set_variable = function()
    {
        this.TenThou            =   this.parseFloat2(document.getElementById('txtTenThou').value);
        this.FiveThou           =   this.parseFloat2(document.getElementById('txtFiveThou').value);
        this.TwoThou            =   this.parseFloat2(document.getElementById('txtTwoThou').value);
        this.OneThou            =   this.parseFloat2(document.getElementById('txtOneThou').value);
        this.FiveHund           =   this.parseFloat2(document.getElementById('txtFiveHund').value);
        this.TwoHund            =   this.parseFloat2(document.getElementById('txtTwoHund').value);
        this.OneHund            =   this.parseFloat2(document.getElementById('txtOneHund').value);
        this.Fifty              =   this.parseFloat2(document.getElementById('txtFifty').value);
        this.Twenty             =   this.parseFloat2(document.getElementById('txtTwenty').value);
        this.Ten                =   this.parseFloat2(document.getElementById('txtTen').value);
        this.Five               =   this.parseFloat2(document.getElementById('txtFive').value);
        this.Two                =   this.parseFloat2(document.getElementById('txtTwo').value);
        this.One                =   this.parseFloat2(document.getElementById('txtOne').value);
        this.Coins              =   this.parseFloat2(document.getElementById('txtCoins').value);
        this.CK_MO_OTH          =   this.parseFloat2(document.getElementById('txtCK_MO_OTH').value);
        this.ATM_DEBIT          =   this.parseFloat2(document.getElementById('txtATM_DEBIT').value);
        this.OTHERS             =   this.parseFloat2(document.getElementById('txtAMOTHERS').value);
        this.Details            =   document.getElementById('txtDetails').value;
        this.Total_Change       =   this.parseFloat2(getText("lblChange"));
    }
    
    this.re_populate_denom = function(ds)
    {
        document.getElementById('txtTenThou').value     =   dt.Rows[0].TenThou.toString();
        document.getElementById('txtFiveThou').value    =   dt.Rows[0].FiveThou.toString();
        document.getElementById('txtTwoThou').value     =   dt.Rows[0].TwoThou.toString();
        document.getElementById('txtOneThou').value     =   dt.Rows[0].OneThou.toString();
        document.getElementById('txtFiveHund').value    =   dt.Rows[0].FiveHund.toString();
        document.getElementById('txtTwoHund').value     =   dt.Rows[0].TwoHund.toString();
        document.getElementById('txtOneHund').value     =   dt.Rows[0].OneHund.toString();
        document.getElementById('txtFifty').value       =   dt.Rows[0].Fifty.toString();
        document.getElementById('txtTwenty').value      =   dt.Rows[0].Twenty.toString();
        document.getElementById('txtTen').value         =   dt.Rows[0].Ten.toString();
        document.getElementById('txtFive').value        =   dt.Rows[0].Five.toString();
        document.getElementById('txtTwo').value         =   dt.Rows[0].Two.toString();
        document.getElementById('txtOne').value         =   dt.Rows[0].One.toString();
        document.getElementById('txtCoins').value       =   dt.Rows[0].Coins.toString();
        document.getElementById('txtCK_MO_OTH').value   =   dt.Rows[0].CK_MO_OTH.toString();
        document.getElementById('txtATM_DEBIT').value   =   dt.Rows[0].ATM_DEBIT.toString();
        document.getElementById('txtAMOTHERS').value      =   dt.Rows[0].OTHERS.toString();
        document.getElementById('txtDetails').value     =   dt.Rows[0].Details.toString();
        setText("lblChange", dt.Rows[0].Total_Change.toString());
    }
    
    this.validate_input = function()
    {
        var error = "";
        
        if (this.isNumeric(document.getElementById("txtTenThou").value) == false)       error += "Field Ten Thousand is not numeric\n";
        if (this.isNumeric(document.getElementById("txtFiveThou").value) == false)      error += "Field Five Thousand is not numeric\n";
        if (this.isNumeric(document.getElementById("txtTwoThou").value) == false)       error += "Field Two Thousand is not numeric\n";
        if (this.isNumeric(document.getElementById("txtOneThou").value) == false)       error += "Field One Thousand is not numeric\n";
        if (this.isNumeric(document.getElementById("txtFiveHund").value) == false)      error += "Field Five Hundred is not numeric\n";
        if (this.isNumeric(document.getElementById("txtTwoHund").value) == false)       error += "Field Two Hundred is not numeric\n";
        if (this.isNumeric(document.getElementById("txtOneHund").value) == false)       error += "Field One Hundred is not numeric\n";
        if (this.isNumeric(document.getElementById("txtFifty").value) == false)         error += "Field Fifty is not numeric\n";
        if (this.isNumeric(document.getElementById("txtTwenty").value) == false)        error += "Field Twenty is not numeric\n";
        if (this.isNumeric(document.getElementById("txtTen").value) == false)           error += "Field Ten is not numeric\n";
        if (this.isNumeric(document.getElementById("txtFive").value) == false)          error += "Field Five is not numeric\n";
        if (this.isNumeric(document.getElementById("txtTwo").value) == false)           error += "Field Two is not numeric\n";
        if (this.isNumeric(document.getElementById("txtOne").value) == false)           error += "Field One is not numeric\n";
        
        if (this.isNumeric(document.getElementById('txtCoins').value) == false)         error += "Field Coins is not numeric\n";
        if (this.isNumeric(document.getElementById('txtCK_MO_OTH').value) == false)     error += "Field CK / MO is not numeric\n";
        if (this.isNumeric(document.getElementById('txtATM_DEBIT').value) == false)     error += "Field ATM / Debit is not numeric\n";
        if (this.isNumeric(document.getElementById('txtAMOTHERS').value) == false)      error += "Field Others is not numeric\n";
                
        if (this.parseFloat2(document.getElementById("txtTenThou").value) % 1 != 0)     error += 'Field Ten Thousand only accepts whole numbers\n';
        if (this.parseFloat2(document.getElementById("txtFiveThou").value) % 1 != 0)    error += 'Field Five Thousand only accepts whole numbers\n';
        if (this.parseFloat2(document.getElementById("txtTwoThou").value) % 1 != 0)     error += 'Field Two Thousand only accepts whole numbers\n';
        if (this.parseFloat2(document.getElementById("txtOneThou").value) % 1 != 0)     error += 'Field One Thousand only accepts whole numbers\n';
        if (this.parseFloat2(document.getElementById("txtFiveHund").value) % 1 != 0)    error += 'Field Five Hundred only accepts whole numbers\n';
        if (this.parseFloat2(document.getElementById("txtTwoHund").value) % 1 != 0)     error += 'Field Two Hundred only accepts whole numbers\n';
        if (this.parseFloat2(document.getElementById("txtOneHund").value) % 1 != 0)     error += 'Field One Hundred only accepts whole numbers\n';
        if (this.parseFloat2(document.getElementById("txtFifty").value) % 1 != 0)       error += 'Field Fifty only accepts whole numbers\n';
        if (this.parseFloat2(document.getElementById("txtTwenty").value) % 1 != 0)      error += 'Field Twenty only accepts whole numbers\n';
        if (this.parseFloat2(document.getElementById("txtTen").value) % 1 != 0)         error += 'Field Ten only accepts whole numbers\n';
        if (this.parseFloat2(document.getElementById("txtFive").value) % 1 != 0)        error += 'Field Five only accepts whole numbers\n';
        if (this.parseFloat2(document.getElementById("txtTwo").value) % 1 != 0)         error += 'Field Two only accepts whole numbers\n';
        if (this.parseFloat2(document.getElementById("txtOne").value) % 1 != 0)         error += 'Field One only accepts whole numbers\n';
        
        if (error != null && error != "")
        {            
            alert(error);
            return false;
        }
        else
        {
            return true;
        }      
    }
     
    this.enable = function(oName)
    {
        var id   = document.getElementById(oName);
        var cset = id.disabled;

        id.disabled = cset == "false";
    }

    this.disable = function(oName)
    {
        var id   = document.getElementById(oName);
        var cset = id.disabled;

        id.disabled = (cset == "false") ? "true" : "false";
    }
   
    this.populate_info = function()
    {
        AjaxPro.timeoutPeriod = 1000 * 1000;
        
        AjaxPro.onTimeout = function()
        {
            alert("Server not responding. Loading Remittance Details failed.");
        }    
    
        IRSWorld.TranClass.rem_modal_denomination.get_remittance_details(parseInt(this.SEQ_ID), this.populate_info_callback);
    }  
   
    this.populate_info_callback = function(response)
    {
        if (response.error != null)
        {
            alert(response.error.Message);
        }
        else if (response != null && response.value != null)
        {
            var dt = response.value;

            if (dt != null && dt.Rows.length > 0)
            {
                setText("lblGrandTotal", (dt.Rows[0].GrossRemAmount == null) ? "" : dt.Rows[0].GrossRemAmount.toString());
                setText("lblCDRefNo", (dt.Rows[0].ReferenceNo == null) ? "" : dt.Rows[0].ReferenceNo.toString());
                setText("lblCDCurr", (dt.Rows[0].remcurr == null) ? "" : dt.Rows[0].remcurr.toString());
                icls.Total_Amt = dt.Rows[0].GrossRemAmount;
            }

            alert('Done loading cash denomination. Please click ok to continue');
        }
        else
        {
            alert('No Response');
        }
    }

    this.btn_cd_close_click = function()
    {
         var res = IRSWorld.TranClass.rem_modal_denomination.Cancel(parseInt(this.SEQ_ID));
         
         if (res.error == null && res.value == true)
         {
             if(confirm('Are you sure you want to cancel cash denomination?'))
             {
                this.close_modal_window(); eval("javascript:push_denomination_commit_response('0~CASH DENOMINATION WAS CANCELLED');");
             }            
         }
         else
         {
            alert("Cancelling of Cash Denomination is disabled.");
         }        
    }
    
    this.btn_cd_save_click = function()
    {   
        this.disable_cd_buttons(true);        
        document.getElementById('trCDLoadingNotification').style.display = 'block';
        
        if (this.validate_input() == true)
        {
            if (this.Total_Amt != this.Total_Denom)
            {
                if (this.Total_Amt > this.Total_Denom)
                {
                    alert("The total denomination is less than the actual amount.\nPlease check denomination and try again.");
                }
                else
                {
                    alert("The total denomination is greater than the actual amount.\nPlease indicate the change properly.");
                }
                
                this.disable_cd_buttons(false);
                document.getElementById('trCDLoadingNotification').style.display = 'none';
                return;
            }
            else
            {
                this.set_variable();        
                this.commit_transaction();
            }
        }
        else
        {
            this.disable_cd_buttons(false);
            document.getElementById('trCDLoadingNotification').style.display = 'none';
        } 
    }
    
    this.disable_cd_buttons = function(disable)
    {
        document.getElementById('btnCDSave').disabled = disable;
        document.getElementById('btnCDClose').disabled = disable;   
    }
               
    this.open_denomination_form = function(SEQ_ID, ref)
    {
       this.SEQ_ID =  SEQ_ID;
       this.ref         = ref;

       this.open_modal_window();
       this.construct_denomination_window();
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
	
	this.construct_div_content = function()
	{
	    var content_div = null;

		content_div                       = document.createElement("div");		
		content_div.id                    = "content_mod_div";
		content_div.align                 = "left";
		content_div.style.background      = "transparent";
		content_div.style.width           = "100%";
		content_div.innerHTML             = "<center><img src = '.././images/ajaxloader.gif'></img></center>";

		this.inner_modal_div              = document.createElement("div");		
		this.inner_modal_div.align        = "center";
		this.inner_modal_div.className    = "inner_div";		
		this.inner_modal_div.id           = "inner_mod_div";
		this.inner_modal_div.style.height = "auto";		
		this.inner_modal_div.style.width  = "700px";
		this.inner_modal_div.style.top    = "30px";
		this.inner_modal_div.style.left   = (parseInt(screen.width) - parseInt(this.inner_modal_div.style.width)) / 2 - 10 + "px";
		this.inner_modal_div.style.border = "medium solid white";
		this.inner_modal_div.Code         = this;
		this.inner_modal_div.appendChild(content_div);
		
		document.body.appendChild(this.inner_modal_div);
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
	
	this.load_modal_css = function()
	{	    
		var file_reference = document.createElement("link");
		
	  	file_reference.setAttribute("rel", "stylesheet");
	  	file_reference.setAttribute("type", "text/css");
	  	file_reference.setAttribute("href", ".././css/cssmodals.css");

		if (typeof file_reference != "undefined")
		{		    
	  		document.getElementsByTagName("head")[0].appendChild(file_reference);
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
	        if (divelement_outer.item(i).id == "outer_mod_div")
	        {
	            divelement_outer.item(i).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop;break;
	        }	        
	    }
	    for(m = 0; m < divelement_inner.length;m++)
	    {
	        if (divelement_inner.item(m).id == "inner_mod_div")
	        {
	            divelement_outer.item(m).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop + 20 : document.body.scrollTop + 20;break;
	        }	        
	    }	    
	}      
   
    this.construct_denomination_window = function()
    {
        if (this.SEQ_ID != null && this.SEQ_ID != "undefined" && this.SEQ_ID != "")
        {
            var html_string = "";
            
            html_string += "<center>";
            html_string += "<table style='border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; width: 700px; border-bottom: #336699 1px solid' cellpadding='0' cellspacing='1'>";
            html_string += "    <tr>";
            html_string += "        <td align='center' style='width: 17%; height:24px'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "            <strong>";            
            html_string += "                Denomination";
            html_string += "            </strong></label></td>";
            html_string += "        <td align='center' style='width:10%; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "            <strong>";            
            html_string += "                Available";
            html_string += "            </strong></label></td>";
            html_string += "        </td>";
            html_string += "        <td style='width: 17%; height:25px'>";
            html_string += "            &nbsp;</td>";
            html_string += "        <td style='width: 27px; height: 25px;'></td>";
            html_string += "        <td style='width: 115px; height: 25px'></td>";
            html_string += "        <td style='width: 218px; height: 25px'></td>";
            html_string += "    </tr>";
            html_string += "    <tr>";
            html_string += "        <td align='center' style='width:17%; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                10,000";
            html_string += "            </label>";
            html_string += "        </td>";
            html_string += "        <td align='center' style='width:10%; height: 24px;'>";
            html_string += "            <label id='lblTenThou' class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                0";
            html_string += "            </label>";
            html_string += "        </td>";
            html_string += "        <td align='left' style='width:17%; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                &nbsp;<input id='txtTenThou' type='text' style='font-weight:normal; width: 92%; height: 15px; text-align:right' maxlength='50'; onchange='javascript:"+ this.ref +".compute();'/></label></td>";
            html_string += "        <td style='width: 27px'></td>";
            html_string += "        <td align='left' style='width:115px; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                <strong>";
            html_string += "                Reference No.</strong></label></td>";
            html_string += "        <td align='left' style='width:218px; height: 24px;'>";
            html_string += "            <label id='lblCDRefNo' class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "            </label>";
            html_string += "        </td>";
            html_string += "    </tr>";
            html_string += "    <tr>";
            html_string += "        <td align='center' style='width:17%; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                5,000";
            html_string += "            </label>";
            html_string += "        </td>";
            html_string += "        <td align='center' style='width:10%; height: 24px;'>";
            html_string += "            <label id='lblFiveThou' class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                0";
            html_string += "            </label>";
            html_string += "        </td>";
            html_string += "        <td align='left' style='width:17%; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                &nbsp;<input id='txtFiveThou' type='text' style='font-weight:normal; width: 92%; height: 15px; text-align:right' maxlength='50'; onchange='javascript:"+ this.ref +".compute();'/></label></td>";
            html_string += "        <td style='width: 27px'></td>";
            html_string += "        <td align='left' style='width:115px; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "            </label>";
            html_string += "        </td>";
            html_string += "        <td align='left' style='width:218px; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "            </label>";
            html_string += "        </td>";
            html_string += "    </tr>";
            html_string += "    <tr>";
            html_string += "        <td align='center' style='width:17%; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                2,000";
            html_string += "            </label>";
            html_string += "        </td>";
            html_string += "        <td align='center' style='width:10%; height: 24px;'>";
            html_string += "            <label id='lblTwoThou' class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                0";
            html_string += "            </label>";
            html_string += "        </td>";
            html_string += "        <td align='left' style='width:17%; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                &nbsp;<input id='txtTwoThou' type='text' style='font-weight:normal; width: 92%; height: 15px; text-align:right' maxlength='50'; onchange='javascript:"+ this.ref +".compute();'/></label></td>";
            html_string += "        <td style='width: 27px'></td>";
//            html_string += "        <td align='left' style='width:115px; height: 24px;'>";
//            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
//            html_string += "            </label>";
//            html_string += "        </td>";
//            html_string += "        <td align='left' style='width:218px; height: 24px;'>";
//            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
//            html_string += "            </label>";
//            html_string += "        </td>";
            html_string += "        <td align='left' style='width:115px; height: 24px;' rowspan = '2'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                <strong>";
            html_string += "                Details</strong></label></td>";
            html_string += "        <td align='left' style='width:218px; height: 24px;' rowspan = '2'>";
            html_string += "            <textarea cols='20' rows='2' id='txtDetails' style='font-weight:normal; width: 96%; height: 38px;'></textarea>";
            html_string += "        </td>";
            html_string += "    </tr>";
            html_string += "    <tr>";
            html_string += "        <td align='center' style='width:17%; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                1,000";
            html_string += "            </label>";
            html_string += "        </td>";
            html_string += "        <td align='center' style='width:10%; height: 24px;'>";
            html_string += "            <label id='lblOneThou' class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                0";
            html_string += "            </label>";
            html_string += "        </td>";
            html_string += "        <td align='left' style='width:17%; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                &nbsp;<input id='txtOneThou' type='text' style='font-weight:normal; width: 92%; height: 15px; text-align:right' maxlength='50'; onchange='javascript:"+ this.ref +".compute();'/></label></td>";
            html_string += "        <td style='width: 27px'></td>";
//            html_string += "        <td align='left' style='width:115px; height: 24px;'>";
//            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
//            html_string += "                <strong>";
//            html_string += "                Details</strong></label></td>";
//            html_string += "        <td align='left' style='width:218px; height: 24px;'>";
//            html_string += "            <input id='txtDetails' type='text' style='font-weight:normal; width: 101%; height: 15px;' maxlength='50';/>";
//            html_string += "        </td>";
            html_string += "    </tr>";
            html_string += "    <tr>";
            html_string += "        <td align='center' style='width:17%; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                500&nbsp;</label></td>";
            html_string += "        <td align='center' style='width:10%; height: 24px;'>";
            html_string += "            <label id='lblFiveHund' class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                0";
            html_string += "            </label>";
            html_string += "        </td>";
            html_string += "        <td align='left' style='width:17%; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                &nbsp;<input id='txtFiveHund' type='text' style='font-weight:normal; width: 92%; height: 15px; text-align:right' maxlength='50'; onchange='javascript:"+ this.ref +".compute();'/></label></td>";
            html_string += "        <td style='width: 27px; height: 24px;'></td>";
            html_string += "        <td align='left' style='width:115px; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                <strong>Currency</strong></label></td>";
            html_string += "        <td align='left' style='width:218px; height: 24px;'>";
            html_string += "            <label id='lblCDCurr' class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "            </label>";
            html_string += "        </td>";
            html_string += "    </tr>";
            html_string += "    <tr>";
            html_string += "        <td align='center' style='width:17%; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                200&nbsp;</label></td>";
            html_string += "        <td align='center' style='width:10%; height: 24px;'>";
            html_string += "            <label id='lblTwoHund' class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                0";
            html_string += "            </label>";
            html_string += "        </td>";
            html_string += "        <td align='left' style='width:17%; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                &nbsp;<input id='txtTwoHund' type='text' style='font-weight:normal; width: 92%; height: 15px; text-align:right' maxlength='50'; onchange='javascript:"+ this.ref +".compute();'/></label></td>";
            html_string += "        <td style='width: 27px'></td>";
            html_string += "        <td align='left' style='width:115px; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "            </label>";
            html_string += "        </td>";
            html_string += "        <td align='left' style='width:218px; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "            </label>";
            html_string += "        </td>         ";                                                                                               
            html_string += "    </tr>";
            html_string += "    <tr>";
            html_string += "        <td align='center' style='width:17%; height: 22px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                100&nbsp;</label></td>";
            html_string += "        <td align='center' style='width:10%; height: 22px;'>";
            html_string += "            <label id='lblOneHund' class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                0";
            html_string += "            </label>";
            html_string += "        </td>";
            html_string += "        <td align='left' style='width:17%; height: 22px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                &nbsp;<input id='txtOneHund' type='text' style='font-weight:normal; width: 92%; height: 15px; text-align:right' maxlength='50'; onchange='javascript:"+ this.ref +".compute();'/></label></td>";
            html_string += "        <td style='width: 27px; height: 22px;'></td>";
            html_string += "        <td align='left' style='width:115px; height: 22px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                <strong>";
            html_string += "                Total Cash</strong></label></td>";
            html_string += "        <td align='left' style='width:218px; height: 22px;'>";
            html_string += "            <label id='lblTotalCash' class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "            </label>";
            html_string += "        </td>";
            html_string += "    </tr>";
            html_string += "    <tr>";
            html_string += "        <td align='center' style='width:17%; height: 14px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                50&nbsp;</label></td>";
            html_string += "        <td align='center' style='width:10%; height: 14px;'>";
            html_string += "            <label id='lblFifty' class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                0";
            html_string += "            </label>";
            html_string += "        </td>";
            html_string += "        <td align='left' style='width:17%; height: 14px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                &nbsp;<input id='txtFifty' type='text' style='font-weight:normal; width: 92%; height: 15px; text-align:right' maxlength='50'; onchange='javascript:"+ this.ref +".compute();'/></label></td>";
            html_string += "        <td style='width: 27px; height: 14px;'></td>";
            html_string += "        <td align='left' style='width:115px; height: 14px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                <strong>";
            html_string += "                Total Checks</strong></label></td>";
            html_string += "        <td align='left' style='width:218px; height: 14px;'>";
            html_string += "            <label id='lblTotalChecks' class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "            </label>";
            html_string += "        </td>";
            html_string += "    </tr>";
            html_string += "    <tr>";
            html_string += "        <td align='center' style='width:17%; height: 18px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                20&nbsp;</label></td>";
            html_string += "        <td align='center' style='width:10%; height: 18px;'>";
            html_string += "            <label id='lblTwenty' class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                0";
            html_string += "            </label>";
            html_string += "        </td>";
            html_string += "        <td align='left' style='width:17%; height: 18px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                &nbsp;<input id='txtTwenty' type='text' style='font-weight:normal; width: 92%; height: 15px; text-align:right' maxlength='50'; onchange='javascript:"+ this.ref +".compute();'/></label></td>";
            html_string += "        <td style='width: 27px; height: 18px;'></td>";
            html_string += "        <td align='left' style='width:115px; height: 18px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                <strong>";
            html_string += "                Total ATM/Debit</strong></label></td>";
            html_string += "        <td align='left' style='width:218px; height: 18px;'>";
            html_string += "            <label id='lblTotalATMDebit' class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "            </label>";
            html_string += "        </td>";
            html_string += "    </tr>";
            html_string += "    <tr>";
            html_string += "        <td align='center' style='width:17%; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                10&nbsp;</label></td>";
            html_string += "        <td align='center' style='width:10%; height: 24px;'>";
            html_string += "            <label id='lblTen' class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                0";
            html_string += "            </label>";
            html_string += "        </td>";
            html_string += "        <td align='left' style='width:17%; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                &nbsp;<input id='txtTen' type='text' style='font-weight:normal; width: 92%; height: 15px; text-align:right' maxlength='50'; onchange='javascript:"+ this.ref +".compute();'/></label></td>";
            html_string += "        <td style='width: 27px'></td>";
            html_string += "        <td align='left' style='width:115px; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                <strong>Total Others</strong></label></td>";
            html_string += "        <td align='left' style='width:218px; height: 24px;'>";
            html_string += "            <label id='lblTotalOthers' class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "            </label>";
            html_string += "        </td>";
            html_string += "    </tr>";
            html_string += "    <tr>";
            html_string += "        <td align='center' style='width:17%; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                5&nbsp;</label></td>";
            html_string += "        <td align='center' style='width:10%; height: 24px;'>";
            html_string += "            <label id='lblFive' class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                0";
            html_string += "            </label>";
            html_string += "        </td>";
            html_string += "        <td align='left' style='width:17%; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                &nbsp;<input id='txtFive' type='text' style='font-weight:normal; width: 92%; height: 15px; text-align:right' maxlength='50'; onchange='javascript:"+ this.ref +".compute();'/></label></td>";
            html_string += "        <td style='width: 27px'></td>";
            html_string += "        <td align='left' style='width:115px; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "            </label>";
            html_string += "        </td>";
            html_string += "        <td align='left' style='width:218px; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "            </label>";
            html_string += "        </td>";
            html_string += "    </tr>";
            html_string += "    <tr>";
            html_string += "        <td align='center' style='width:17%; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                2&nbsp;</label></td>";
            html_string += "        <td align='center' style='width:10%; height: 24px;'>";
            html_string += "            <label id='lblTwo' class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                0";
            html_string += "            </label>";
            html_string += "        </td>";
            html_string += "        <td align='left' style='width:17%; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                &nbsp;<input id='txtTwo' type='text' style='font-weight:normal; width: 92%; height: 15px; text-align:right' maxlength='50'; onchange='javascript:"+ this.ref +".compute();'/></label></td>";
            html_string += "        <td style='width: 27px'></td>";
            html_string += "        <td align='left' style='width:115px; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                <strong>";
            html_string += "                Grand Total</strong></label></td>";
            html_string += "        <td align='left' style='width:218px; height: 24px;'>";
            html_string += "            <label id='lblGrandTotal' class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "            </label>";
            html_string += "        </td>";
            html_string += "    </tr>";
            html_string += "    <tr>";
            html_string += "        <td align='center' style='width:17%; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                1&nbsp;</label></td>";
            html_string += "        <td align='center' style='width:10%; height: 24px;'>";
            html_string += "            <label id='lblOne' class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                0";
            html_string += "            </label>";
            html_string += "        </td>";
            html_string += "        <td align='left' style='width:17%; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                &nbsp;<input id='txtOne' type='text' style='font-weight:normal; width: 92%; height: 15px; text-align:right' maxlength='50'; onchange='javascript:"+ this.ref +".compute();'/></label></td>";
            html_string += "        <td style='width: 27px'></td>";
            html_string += "        <td align='left' style='width:115px; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "            </label>";
            html_string += "        </td>";
            html_string += "        <td align='left' style='width:218px; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "            </label>";
            html_string += "        </td>";
            html_string += "    </tr>";
            html_string += "    <tr>";
            html_string += "        <td align='center' style='width:17%; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                Coins&nbsp;</label></td>";
            html_string += "        <td align='center' style='width:10%; height: 24px;'>";
            html_string += "            <label id='lblCoins' class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                0";
            html_string += "            </label>";
            html_string += "        </td>";
            html_string += "        <td align='left' style='width:17%; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                &nbsp;<input id='txtCoins' type='text' style='font-weight:normal; width: 92%; height: 15px; text-align:right' maxlength='50'; onchange='javascript:"+ this.ref +".compute();'/></label></td>";
            html_string += "        <td style='width: 27px'></td>";
            html_string += "        <td align='left' style='width:115px; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                <strong>";
            html_string += "                Change</strong></label></td>";
            html_string += "        <td align='left' style='width:218px; height: 24px;'>";
            html_string += "            <label id='lblChange' class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "            </label>";
            html_string += "        </td>";
            html_string += "    </tr>";
            html_string += "    <tr>";
            html_string += "        <td align='center' style='width:17%; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                CK / MO&nbsp;</label></td>";
            html_string += "        <td align='center' style='width:10%; height: 24px;'>";
            html_string += "            <label id='lblCK_MO_OTH' class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                0";
            html_string += "            </label>";
            html_string += "        </td>";
            html_string += "        <td align='left' style='width:17%; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                &nbsp;<input id='txtCK_MO_OTH' type='text' style='font-weight:normal; width: 92%; height: 15px; text-align:right' maxlength='50'; onchange='javascript:"+ this.ref +".compute();'/></label></td>";
            html_string += "        <td style='width: 27px'></td>";
            html_string += "        <td align='left' style='width:115px; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "            </label>";
            html_string += "        </td>";
            html_string += "        <td align='left' style='width:218px; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "            </label>";
            html_string += "        </td>";
            html_string += "    </tr>";
            html_string += "    <tr>";
            html_string += "        <td align='center' style='width:17%; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                ATM / Debit&nbsp;</label></td>";
            html_string += "        <td align='center' style='width:10%; height: 24px;'>";
            html_string += "            <label id='lblATM_DEBIT' class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                0";
            html_string += "            </label>";
            html_string += "        </td>";
            html_string += "        <td align='left' style='width:17%; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                &nbsp;<input id='txtATM_DEBIT' type='text' style='font-weight:normal; width: 92%; height: 15px; text-align:right' maxlength='50'; onchange='javascript:"+ this.ref +".compute();'/></label></td>";
            html_string += "        <td style='width: 27px'></td>";
            html_string += "        <td align='left' style='width:115px; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "            </label>";
            html_string += "        </td>";
            html_string += "        <td align='left' style='width:218px; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "            </label>";
            html_string += "        </td>";
            html_string += "    </tr>";
            html_string += "    <tr>";
            html_string += "        <td align='center' style='width:17%; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                Others&nbsp;</label></td>";
            html_string += "        <td align='center' style='width:10%; height: 24px;'>";
            html_string += "            <label id='lblOTHERS' class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                0";
            html_string += "            </label>";
            html_string += "        </td>";
            html_string += "        <td align='left' style='width:17%; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "                &nbsp;<input id='txtAMOTHERS' type='text' style='font-weight:normal; width: 92%; height: 15px; text-align:right' maxlength='50'; onchange='javascript:"+ this.ref +".compute();'/></label></td>";
            html_string += "        <td style='width: 27px'></td>";
            html_string += "        <td align='left' style='width:74px; height: 24px;' colspan='2'>";
            html_string += "                <table width='100%' cellpadding='0' cellspacing='5' style='background:azure;'>";
            html_string += "                    <tr>";
            html_string += "                        <td width='20%' style='height: 20px' align='right'>";
            html_string += "                            <input style='width:150px; height: 25px;' id='btnCDSave'     type='button' value='SAVE'    onclick='javascript:" + this.ref + ".btn_cd_save_click();'/>&nbsp;";
            html_string += "                            <input style='width:150px; height: 25px;' id='btnCDClose' type='button' value='CLOSE' onclick='javascript:" + this.ref + ".btn_cd_close_click();'/>&nbsp;";
            html_string += "                        </td>";
            html_string += "                    </tr>";
            html_string += "                </table>";
            html_string += "        </td>";
            html_string += "    </tr>";
            html_string += "    <tr>";
            html_string += "        <td align='left' style='width:17%; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "            </label>";
            html_string += "        </td>";
            html_string += "        <td align='left' style='width:10%; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "            </label>";
            html_string += "        </td>";
            html_string += "        <td align='right' style='width:17%; height: 24px;'>";
            html_string += "            <label id='lblTotal' class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "            </label></td>";
            html_string += "        <td style='width: 27px'></td>";
            html_string += "        <td align='left' style='width:115px; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "            </label>";
            html_string += "        </td>";
            html_string += "        <td align='left' style='width:218px; height: 24px;'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>";
            html_string += "            </label>";
            html_string += "       </td>";
            html_string += "    </tr>";
            
            html_string += "    <tr id  = 'trCDLoadingNotification' style='display:none'>";
            html_string += "        <td align='left' style='width:17%; height: 24px;' colspan='5' class='comment'>";
            html_string += "            <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>&nbsp;&nbsp;Saving Cash Denomination. Please wait..";
            html_string += "            </label>";
            html_string += "        </td>";
            html_string += "    </tr>";
            
            html_string += "</table>";
            html_string += "</center>";
            
            document.getElementById("content_mod_div").innerHTML = html_string; 
            try{document.getElementById("txtTenThou").focus()}catch(e){};
            this.page_onload();
       }
    }
}
/* END DINOMINATION */

/* 21.) GENERIC OVERRIDE */
function generic_override()
{
    var genovr = this;

    this.outer_modal_div    = null;
    this.inner_modal_div    = null;
    this.msg_prompt_control = null;
    this.generic_caption    = null;
    this.generic_flag       = null;
    this.grid               = null;       
    this.remitter_id        = null;
    this.beneficiary_id     = null;
    this.user_name          = null;
    this.user_pass          = null;
    
    this.enroll_phonetics_1 = null;
    this.enroll_phonetics_2 = null;
    this.enroll_phonetics_3 = null;
    this.enroll_phonetics_4 = null;
    this.enroll_phonetics_5 = null;
    this.enroll_phonetics_6 = null;
    this.enroll_phonetics_7 = null;
    this.enroll_phonetics_8 = null;

    this.uwnames_1 = null;
    this.uwnames_2 = null;
    this.uwnames_3 = null;
    this.uwnames_4 = null;

    this.pending_override_status = 0;
    //this.override_type_code = "25"; //COMMENTED FOR  /* RFC-37590 - IRSW NY Interface with Refinitiv WorldCheck via API */
    this.override_type_code = "50";
    this.isCIFRemoteOverride = null;
    /* RFC-37590 - IRSW NY Interface with Refinitiv WorldCheck via API */
    this.isRefinitivResolved = false;
    this.cif_isRefinitivIndividual = 0;
    this.cif_gender = null;
    this.cif_DOB = null;
    this.cif_COB = null;
    this.cif_POB = null;
    this.cif_Nationality = null;
    this.ciftype = null;
    /* RFC-37590 - IRSW NY Interface with Refinitiv WorldCheck via API */

    //=========================================================================================
    /* RFC-30454 Creation of new report in IRSW for OTC transactions with OFAC hits */
    //* RFC-32829 : Request to enhance the OFAC alert and Report in IRSW 01/14/2024 */
    this.sanction_discounting_reason = null;
    this.sanction_discounting_reason_bus = null;
    this.sanction_discounting_reason_other = "";
    this.sanction_disposition_reason = null;
    this.sanction_disposition_reason_remarks = "";
    this.rem_ciftype = "";
    this.ben_ciftype = "";

    this.FullName = null;
    this.FileFormat = null;
    this.TrackingNo = null;
    this.RecordID = null;
    this.Title = null;
    this.LastName = null;
    this.FirstName = null;
    this.MiddleName = null;
    this.Suffix = null;
    this.AliasSuffix = null;
    this.AliasLastName = null;
    this.AliasFirstName = null;
    this.AliasMiddleName = null;
    this.Position = null;
    this.DateOfBirth = null;
    this.TownOfBirth = null;
    this.CountryOfBirth = null;
    this.Nationality = null;
    this.IDNumber = null;
    this.IDType = null;
    this.Street = null;
    this.City = null;
    this.State = null;
    this.Zip = null;
    this.Country = null;
    this.PhoneNo = null;
    this.HomeNo = null;
    this.BusinessName = null;
    this.BusinessPhoneNo = null;
    this.DBAName = null;
    this.OtherInfo = null;
    this.GroupType = null;
    this.GroupID = null;
    this.AliasType = null;
    this.Regime = null;
    this.ListedOn = null;
    this.LastUpdate = null;
    this.PassportDetails = null;
    this.Remarks = null;
    this.IsDeleted = null;
    this.DateLastUpdated = null;
    this.isOBPscreen = 0;
    this.isOBPscreenTransaction = 0;   /* RFC-37590 - IRSW NY Interface with Refinitiv WorldCheck via API */
    //=========================================================================================

    this.run_rmover_closer = function()
    {
        if (parseInt(this.generic_flag) == 2)
        {
            this.close_modal_window(); eval("javascript:push_override_hit_list_result(genovr);");
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

    this.open_client_hit_list_all = function()
    {
        this.generic_flag = 2;
        //=========================================================================================
        //* RFC-32829 : Request to enhance the OFAC alert and Report in IRSW 01/14/2024 */
        //this.open_modal_window();
        //this.construct_otc_hit_list_all();
        //this.process_viewer('ajx_loading$');
        this.ok_hit_list_all();
        //=========================================================================================
        
    }

    this.open_client_hit_list_all_refinitivisdown = function () {
        this.generic_flag = 2;
        genovr.close_modal_window();
        genovr.refinitiv_transaction_override_status = 1;
        genovr.close_msg_prompt();
        genovr.open_modal_window();
        this.construct_generic_screen_refinitivisdown();
        //this.process_viewer('ajx_loading$');
    }

    this.close_client_hit_list_all = function()
    {
        this.close_modal_window();
    }
    
    //=========================================================================================
    //* RFC-32829 : Request to enhance the OFAC alert and Report in IRSW 01/14/2024 */
    this.do_ajaxgridview = function(parameter)
    {
        if (parameter != null && parameter != "undefined")
        {
            var command_param = parameter.split("$");

            if (command_param.length > 0)
            {
                if (command_param[0] == "ajx_selected")
                {
                    //return;
                    this.display_watchlist_record_onclick(command_param[1]);
                }
                else
                {
                    this.process_viewer(parameter);
                }
            }
        }
    }

    this.display_watchlist_record_onclick = function (parameter) {
        document.getElementById("txtrecordno").value = "";

        if (parameter != null) {
            var recordno = parameter.split("~");
            if (recordno.length > 0) {
                IRSWorld.TranClass.RemTranExtra.get_watchlist_record_details(recordno[0], this.display_watchlist_record_callback);
            }
        }
    }
    //=========================================================================================
    
    this.check_stream_lining = function()
    {
        this.close_client_hit_list_all();
        this.open_msg_prompt("Checking stream lining  ...");    

        AjaxPro.timeoutPeriod = 1000 * 1000;
        
        AjaxPro.onTimeout = function()
        {
            this.close_msg_prompt(); alert("Server not responding. Checking stream lining failed.");
        }    
    
        IRSWorld.TranClass.RemTranExtra.branch_in_stream_lining('', this.check_stream_lining_callback);
    }

    this.check_stream_lining_callback = function(result)
    {
        genovr.close_msg_prompt();
        
        if (result != null && result.value != null)
        {
            if (result.value == false)
            {
                genovr.open_modal_window(); 
                genovr.construct_generic_screen();
            }
            else
            {
                eval("javascript:push_override_hit_list_result(true);"); /* for confirmation */
            }
        }
    }
    
    this.ok_hit_list_all = function()
    {
        genovr.close_modal_window();
        genovr.refinitiv_transaction_override_status = 1;
        var isRefinitivEnabled = IRSWorld.TranClass.Refinitiv.isRefinitivEnabled();
        if (isRefinitivEnabled.value == "1") {
            genovr.close_msg_prompt();
            if (this.isOBPscreenTransaction = 1) {
                genovr.open_modal_window();
                genovr.construct_generic_screen_refinitiv();
                genovr.process_viewer();
            }
            else {
                if (result != null && result.value != null) {
                    if (result.value == false) {
                        genovr.open_modal_window();
                        genovr.construct_generic_screen_refinitiv();
                        genovr.process_viewer();
                    }
                    else {
                        eval("javascript:push_override_hit_list_result(true);"); /* for confirmation */
                    }
                }
            }
        }
        else { this.check_stream_lining(); }
    }

    this.cancel_hit_list_all = function()
    {
        this.close_client_hit_list_all();
    }

    this.process_viewer = function(param)
    { 
        /* RFC-37590 - IRSW NY Interface with Refinitiv WorldCheck via API */
        var isRefinitivEnabled = IRSWorld.TranClass.Refinitiv.isRefinitivEnabled();
        if (isRefinitivEnabled.value == "1") {
            if (this.generic_flag == 2) /* TRANSACTIONAL */ {
                //runWaitMe();
                IRSWorld.TranClass.Refinitiv.DisplayWatchlistTransactional_DT(this.remitter_id, this.beneficiary_id, DisplayWatchlistTransactional_DT_callback);
            }
            if (this.generic_flag == 3) /* ONBOARDING */ {
                IRSWorld.TranClass.Refinitiv.DisplayWatchlistOnboarding_DT(this.cif_fname, this.cif_mname, this.cif_lname, this.cifid, this.cif_isRefinitivIndividual, this.cif_gender, this.cif_DOB, this.cif_COB, this.cif_POB, this.cif_Nationality, DisplayWatchlistOnboarding_DT_callback);
            }
        } /* RFC-37590 - IRSW NY Interface with Refinitiv WorldCheck via API */
        else {
            if (this.generic_flag == 2) {
                this.grid = new grid_viewer();
                this.grid.sql_source = "sp_otc_hit_listing~" + this.remitter_id + "~" + this.beneficiary_id + "~";
                this.grid.div_container = document.getElementById("div_client_hit_list_all");
                this.grid.datarow_column = 0;
                this.grid.page_size = 10;
                this.grid.process_ajax(param);
                this.grid = null;
            }
            if (this.generic_flag == 3) {
                this.grid = new grid_viewer();
                //this.grid.sql_source     = "sp_check_and_get_cif_hitlist~" + this.cif_fname + "~" + this.cif_lname + "~" + this.cif_mname + "~" + this.cif_branchcode + "~";

                if (this.uwnames_4 == "UWPM" || this.uwnames_4 == "UWFL") {
                    //this.grid.sql_source     = "sp_check_and_get_cif_hitlist_validations~" + this.uwnames_1 + "~" + this.uwnames_2 + "~" + this.uwnames_3 + "~" + this.cif_branchcode + "~" + this.enroll_phonetics_1 + "~" + this.enroll_phonetics_2 + "~" + this.enroll_phonetics_3 + "~" + this.enroll_phonetics_4 + "~" + this.enroll_phonetics_5 + "~" + this.enroll_phonetics_6 + "~" + this.enroll_phonetics_7 + "~" + this.enroll_phonetics_8; -single name result
                    this.grid.sql_source = "sp_check_and_get_cif_percentage_validations~" + this.cif_userid + "~" + this.cif_branchcode;
                }
                else {
                    this.grid.sql_source = "sp_check_and_get_cif_hitlist_validations~" + this.cif_fname + "~" + this.cif_lname + "~" + this.cif_mname + "~" + this.cif_branchcode + "~" + this.enroll_phonetics_1 + "~" + this.enroll_phonetics_2 + "~" + this.enroll_phonetics_3 + "~" + this.enroll_phonetics_4 + "~" + this.enroll_phonetics_5 + "~" + this.enroll_phonetics_6 + "~" + this.enroll_phonetics_7 + "~" + this.enroll_phonetics_8;
                }

                this.grid.div_container = document.getElementById("div_cif_client_hit_list_all");
                this.grid.datarow_column = 0;
                this.grid.page_size = 10;
                this.grid.process_ajax(param);
                this.grid = null;
            }
        }
    }

        /* RFC-37590 - IRSW NY Interface with Refinitiv WorldCheck via API  */
    function basicAlertMsg(title, message) {
        setText(document.getElementById('lblMessagebox'), '');
        setText(document.getElementById('spanMessagebox'), message);

        $(function () {
            $("#divMessageBox").dialog({
                title: title,
                message: message,
                modal: true,
                overlay: {
                    draggable: false,
                    position: "center",
                    modal: true,
                    backgroundColor: '#000',
                    opacity: 0.75
                }
            });
        });
        $(".ui-widget-header").css("background", "#70c5c5");
    }

    /* RFC-37590 - IRSW NY Interface with Refinitiv WorldCheck via API  */
    function populateHitList(dtHitList) {
        //runWaitMe();
        try {
            $('#tblHitList').DataTable().clear().draw();
            $('#tblHitList').DataTable({
                responsive: true,
                scrollX: true,
                "destroy": true,
                "bSort": false,
                dom: 'Bfrtip',
                "pageLength": 5,
                data: dtHitList,
                columns: [
                    { data: 4 }, //Match Score 
					{ data: 6 }, //Matched Name Type
                    { data: 8 }, //Primary Name 
					{ data: 10 }, //Category
                    { data: 13 }, //Date of Birth
                    { data: 14 }, //Nationality
                    { data: 9 }, //Special Interest Category (Sources)
                    { data: 16 }, //Provider Type
                    { data: 17 }, //Alias
                    { data: 18 }, //Remarks
                    { data: 19 }, //Resolution
                    { data: 20 }, //ResolutionRemarks
                    { data: 21 }, //CreatedDate
                    { data: 22 }, //UpdatedDate
                ]
            });

            var table = $('#tblHitList').DataTable();
            table.responsive.rebuild();
            table.responsive.recalc();
            //stopWaitMe();
        }
        catch (err) { //err.message
            //stopWaitMe();
            spawn_errpage("[HIT LIST] " + err.message);
        }
    }

    function DisplayWatchlistTransactional_DT_callback(result) {
        genovr.close_msg_prompt();
        //stopWaitMe();
        if (result.error == null) {
            var data = result.value;
            if (data.length > 0) {
                gHitValue = 'YES';
                //$('#txtwithHIT').val(gHitValue);
                //$('.spnErrorMsg').text('');
                //confirmHit('HIT FOUND', 'Name found on hitlist, do you want to proceed?');
                $('#remModal').modal('show');
                testtable = data;
                populateHitList(data);
                //with hit
            }
            else {
                hitValidated = true;
                gHitValue = 'NO';
                //$('#txtwithHIT').val(gHitValue);
                gHitRemarks = '';
                $('#txthitrem').val(gHitRemarks);
                basicAlertMsg("HIT NOT FOUND", "No hit found. You may now proceed with the enrollment.");
                //without
            }
        }
        else if (result.error != null) {
            if (result.error.Message != "Failed to get Refinitiv DisplayWatchlist info, Reason : Refinitiv is down.") {
                basicAlertMsg(result.error.Message);
                return;
            }
            else if (result.error.Message == "NO HIT") {
                genovr.close_msg_prompt();
                genovr.close_modal_window();
                eval("javascript:push_override_hit_list_result(true);");
            }
            else {
                basicAlertMsg("Unable to connect to refinitiv.");
                genovr.close_msg_prompt();
                genovr.close_modal_window();
                return;
            }
        }
    }

    function DisplayWatchlistOnboarding_DT_callback(result) {
        //stopWaitMe();
        genovr.close_msg_prompt();
        if (result.error == null) {
            var data = result.value;
            if (data.length > 0) {
                gHitValue = 'YES';
                //$('#txtwithHIT').val(gHitValue);
                //$('.spnErrorMsg').text('');
                //confirmHit('HIT FOUND', 'Name found on hitlist, do you want to proceed?');
                $('#remModal').modal('show');
                testtable = data;
                populateHitList(data);
                //with hit
            }
            else {
                hitValidated = true;
                gHitValue = 'NO';
                //$('#txtwithHIT').val(gHitValue);
                gHitRemarks = '';
                //$('#txthitrem').val(gHitRemarks);
                basicAlertMsg("HIT NOT FOUND", "No hit found. You may now proceed with the enrollment.");
                //without
            }
        }

        else if (result.error != null) {
            if (result.error.Message != "Failed to get Refinitiv DisplayWatchlist info, Reason : Refinitiv is down.") {
                basicAlertMsg(result.error.Message);
                return;
            }
            else if (result.error.Message == "NO HIT") {
                genovr.close_msg_prompt();
                genovr.close_modal_window();
                eval("javascript:push_override_hit_list_result(true);");
            }
            else {
                basicAlertMsg("Unable to connect to refinitiv.");
                genovr.close_msg_prompt();
                genovr.close_modal_window();
                return;
            }
        }
    }



    //=========================================================================================
    /* RFC-30454 Creation of new report in IRSW for OTC transactions with OFAC hits */
    this.enter_button_clicked = function()
    {
        this.user_name = document.getElementById("txt_genovr_user_id").value;
        this.user_pass = document.getElementById("txt_genovr_password").value;

        //=========================================================================================
        //* RFC-32829 : Request to enhance the OFAC alert and Report in IRSW 01/14/2024 */
        this.sanction_discounting_reason = document.getElementById("ddl_reason_discounting").value;
        this.sanction_discounting_reason_bus = document.getElementById("ddl_reason_discounting_bus").value;
        this.sanction_discounting_reason_other = document.getElementById("txt_reason_disc").value;
        this.sanction_disposition_reason = document.getElementById("ddl_approver_disposition").value;
        this.sanction_disposition_reason_remarks = document.getElementById("txt_approver_remarks").value;
        //=========================================================================================
       
        ///*DDC RFS SANCTION LIST DISCOUNTING JULY 20 2016*/
        //if (document.getElementById("txt_reason_disc") != null)
        //{
        //    this.sanction_discounting_reason = "";
        //    this.sanction_discounting_reason = document.getElementById("txt_reason_disc").value;
        //}
        ///*END*/

        if (this.sanction_disposition_reason == "") {
            this.close_msg_prompt();
            alert("Please provide Approvers Disposition.");
            return;
        }
        if (this.sanction_disposition_reason == "null") {
            this.close_msg_prompt();
            alert("Please provide Approvers Disposition.");
            return;
        }
        if (this.sanction_disposition_reason == "99" && this.sanction_disposition_reason_remarks == "") {
            this.close_msg_prompt();
            alert("Please provide Approvers Remarks.");
            return;
        }
        if (this.sanction_disposition_reason == "99" && this.sanction_disposition_reason_remarks == "null") {
            this.close_msg_prompt();
            alert("Please provide Approvers Remarks.");
            return;
        }

        this.close_modal_window();

        AjaxPro.timeoutPeriod = 1000 * 1000;
        
        AjaxPro.onTimeout = function()
        {
            this.close_msg_prompt(); alert("Server not responding. Checking user rights failed.");
        }

        this.open_msg_prompt("Checking access rights. Please wait ...");
        
        if (this.generic_flag == 2)
        {

            try {
                var sanction_disc = IRSWorld.TranClass.RemTranExtra.discounting_required().value;

                if (sanction_disc != null && sanction_disc == true) {
                    //=========================================================================================
                    //* RFC-32829 : Request to enhance the OFAC alert and Report in IRSW 01/14/2024 */
                    //IRSWorld.TranClass.RemTranExtra.user_can_override_hit_list_discounting(this.user_name, this.user_pass, this.sanction_discounting_reason, this.sanction_discounting_reason_other, this.generic_flag, this.check_user_rights_callback);
                    IRSWorld.TranClass.RemTranExtra.user_can_override_hit_list_discounting(this.user_name, this.user_pass, this.sanction_discounting_reason, this.sanction_discounting_reason_other, this.sanction_discounting_reason_bus, this.sanction_disposition_reason, this.sanction_disposition_reason_remarks, this.remitter_id, this.beneficiary_id, this.generic_flag, this.check_user_rights_callback);
                    //=========================================================================================
                }
                else {
                    IRSWorld.TranClass.RemTranExtra.user_can_override_hit_list(this.user_name, this.user_pass, this.check_user_rights_callback);
                }
            }
            catch (error) {
                IRSWorld.TranClass.RemTranExtra.user_can_override_hit_list(this.user_name, this.user_pass, this.check_user_rights_callback);
            }

        }
        if (this.generic_flag == 4)
        {
            IRSWorld.TranClass.RemTranExtra.user_can_override_hit_list(this.user_name, this.user_pass, this.check_user_rights_callback);
        }        
    }
    //=========================================================================================
    
    this.cancel_button_clicked = function()
    {
        if (parseInt(genovr.generic_flag) == 2)
        {
            this.close_modal_window(); eval("javascript:push_override_hit_list_result(false);");
        }
        else if (parseInt(genovr.generic_flag) == 4)
        {
            this.close_modal_window(); Internet_Remitter.cancelOverride();
        }        
    }
    
    this.check_user_rights_callback = function(result)
    {
        genovr.close_msg_prompt();

        if (result != null && result.value != null)
        {
            if (parseInt(genovr.generic_flag) == 2)
            {
                if (result.value[0].toString().toUpperCase() == "FALSE")
                {
                    alert(result.value[1].toString()); genovr.ok_hit_list_all(); return;
                }
                else
                {
                    genovr.close_modal_window();
                    /* RFC-36314 - IRSW (EU) Interface with Refinitiv WorldCheck via API */
                    IRSWorld.TranClass.Refinitiv.ResolveWatchlist_viaCIFID(genovr.remitter_id, false, genovr.sanction_discounting_reason, genovr.user_name);
                    IRSWorld.TranClass.Refinitiv.ResolveWatchlist_viaCIFID(genovr.beneficiary_id, false, genovr.sanction_discounting_reason, genovr.user_name);
                    /* RFC-36314 - IRSW (EU) Interface with Refinitiv WorldCheck via API */
                    eval("javascript:push_override_hit_list_result(true);");
                }
            }
            else if (parseInt(genovr.generic_flag) == 4)
            {
                if (result.value[0].toString().toUpperCase() == "FALSE")
                {   
                    alert(result.value[1].toString());
                    Internet_Remitter.cancelOverride(); 
                    return;
                }
                else
                {
                    genovr.close_modal_window();
                    Internet_Remitter.canOverride(); 
                }
            }
        }
    }

    this.set_genovr_caption = function()
    {
        if (this.generic_flag == 1)
        {
            this.generic_caption = "CUSTOMER HIT LIST - OVERRIDE";
        }
        else if (this.generic_flag == 2)
        {
            this.generic_caption = "CUSTOMER HIT LIST - OVERRIDE";
        }
        else if (this.generic_flag == 4)
        {
            this.generic_caption = "WEB AND PHONE REMITTANCE - OVERRIDE";
        }
        else
        {
            this.generic_caption = "OVERRIDE SCREEN";
        }
    }

    this.construct_otc_hit_list_all = function()
    {
        var html_string = "";
    
        ihtml = '<center>'
        ihtml += '<table cellpadding = "0"; cellspacing = "1" style = "width:700px;border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid">';
        ihtml += '    <tr>';
        ihtml += '        <td class = "cell_desc" colspan="2" style="text-align: center"><label>CLIENT HIT LIST</label></td>';
        ihtml += '    </tr>';
        ihtml += '    <tr>';
        ihtml += '        <td colspan="2" style="text-align: center" ><div id = "div_client_hit_list_all"></div></td>';
        ihtml += '    </tr>';
        ihtml += '   <tr>';
        ihtml += '       <td colspan="2" style="text-align: center"><input id = "btn_client_hit_list_ok_all" type = "button" value = "OK" style="width: 100px; height: 25px" />&nbsp;<input id = "btn_client_hit_list_cancel_all" type = "button" value = "CANCEL" style="width: 100px; height: 25px" /></td>';        
        ihtml += '   </tr>';
        ihtml += '</table>';
        ihtml += '</center>'
        
        document.getElementById("content_mod_div").innerHTML = ihtml;
        
        document.getElementById("btn_client_hit_list_ok_all").onclick = function()
        {
            genovr.ok_hit_list_all();
        }
        
        document.getElementById("btn_client_hit_list_cancel_all").onclick = function()
        {
            genovr.cancel_hit_list_all();
        }
    }


    this.construct_otc_hit_list_all_refinitivisdown = function () {
        var html_string = "";

        ihtml = '<center>'
        ihtml += '<table cellpadding = "0"; cellspacing = "1" style = "width:700px;border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid">';
        ihtml += '    <tr>';
        ihtml += '        <td class = "cell_desc" colspan="2" style="text-align: center"><label>REFINITIV IS DOWN.</label></td>';
        ihtml += '    </tr>';
        ihtml += '    <tr>';
        ihtml += '        <td colspan="2" style="text-align: center" ><div id = "div_client_hit_list_all"></div></td>';
        ihtml += '    </tr>';
        ihtml += '   <tr>';
        ihtml += '       <td colspan="2" style="text-align: center"><input id = "btn_client_hit_list_ok_all" type = "button" value = "OK" style="width: 100px; height: 25px" />&nbsp;<input id = "btn_client_hit_list_cancel_all" type = "button" value = "CANCEL" style="width: 100px; height: 25px" /></td>';
        ihtml += '   </tr>';
        ihtml += '</table>';
        ihtml += '</center>'

        document.getElementById("content_mod_div").innerHTML = ihtml;

        document.getElementById("btn_client_hit_list_ok_all").onclick = function () {
            genovr.ok_hit_list_all();
        }

        document.getElementById("btn_client_hit_list_cancel_all").onclick = function () {
            genovr.cancel_hit_list_all();
        }
    }

    //=========================================================================================
    /* RFC-30454 Creation of new report in IRSW for OTC transactions with OFAC hits */
    this.construct_generic_screen = function()
    {
        this.set_genovr_caption();

        var html_string = "";
        var sanction_disc = null;
        
        /*SANCTION LIST DISCOUNTING - RFS 16165-003 DDC*/
        try
        {
            sanction_disc = IRSWorld.TranClass.RemTranExtra.discounting_required().value;
        }
        catch(error)
        {
            sanction_disc = null;
        }
        /*END*/

        html_string = "<table id = 'tbl_otc_generic_override' style='border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; width: 1000px; border-bottom: #336699 1px solid' cellpadding='0' cellspacing='1'>";
        html_string += "    <tr><td colspan = '2' style = 'text-align: center' class = 'table_caption'>" + this.generic_caption + "</td></tr>";
        html_string += "    <tr style = 'height: 19px' >";
        html_string += "        <td class = 'cell_caption' style = 'width: 30%; text-align: right'>USER NAME :</td>";
        html_string += "        <td class = 'cell_desc'    style = 'width: 70%; text-align: left'><input type = 'text' id = 'txt_genovr_user_id' /></td>";
        html_string +=      "</tr>";
        html_string += "    <tr style='height: 19px'>";
        html_string += "        <td style = 'width: 30%; text-align: right;' class = 'cell_caption'>PASSWORD :</td>";
        html_string += "        <td style = 'width:70%;  text-align: left;'  class = 'cell_desc'><input type = 'password' id = 'txt_genovr_password' /></td>";
        html_string += "    </tr>";

        /*SANCTION LIST DISCOUNTING - RFS 16165-003 DDC*/

        if (parseInt(genovr.generic_flag) == 2 && sanction_disc != null && sanction_disc == true)
        {
            html_string += "    <tr style='height: 5px'>";
            html_string += "        <td style = 'width: 30%; text-align: right; display:none' class = 'cell_caption'></td>";
            html_string += "        <td style = 'width: 70%;  text-align: left; display:none'  class = 'cell_desc'></td>";
            html_string += "    </tr>";

            //=========================================================================================
            //* RFC-32829 : Request to enhance the OFAC alert and Report in IRSW 01/14/2024 */
            //var ciftype_val = IRSWorld.TranClass.RemTranExtra.validate_watchlist(this.remitter_id, this.beneficiary_id,"","","","","1").value;
            var ciftype_val = IRSWorld.TranClass.RemTranExtra.validate_watchlist_refinitiv(this.remitter_id, this.beneficiary_id, "", "", "", "", "1").value;
            var ciftype_newval = ciftype_val.split('~');
            this.rem_ciftype = ciftype_newval[0];
            this.ben_ciftype = ciftype_newval[1];

            if (this.rem_ciftype == "1" && this.ben_ciftype == "1")
            {
                html_string += "    <tr style='height: 19px'>";
                html_string += "        <td style='width: 30%; text-align: right' class='cell_caption'>Tellers Reason for Discounting (Individual) :</td>";
                html_string += "        <td style='width: 70%; text-align: left' class='cell_desc'><select id='ddl_reason_discounting' style = 'width: 700px'/><option selected='selected'></option></select></td>";
                html_string += "    </tr>";
                html_string += "    <tr style='height: 19px; display:none'>";
                html_string += "        <td style='width: 30%; text-align: right' class='cell_caption'>Tellers Reason for Discounting (Business) :</td>";
                html_string += "        <td style='width: 70%; text-align: left' class='cell_desc'><select id='ddl_reason_discounting_bus' style = 'width: 700px'/><option selected='selected'></option></select></td>";
                html_string += "    </tr>";
            }
            else if (this.rem_ciftype == "2" && this.ben_ciftype == "2") {
                html_string += "    <tr style='height: 19px; display:none'>";
                html_string += "        <td style='width: 30%; text-align: right' class='cell_caption'>Tellers Reason for Discounting (Individual) :</td>";
                html_string += "        <td style='width: 70%; text-align: left' class='cell_desc'><select id='ddl_reason_discounting' style = 'width: 700px'/><option selected='selected'></option></select></td>";
                html_string += "    </tr>";
                html_string += "    <tr style='height: 19px'>";
                html_string += "        <td style='width: 30%; text-align: right' class='cell_caption'>Tellers Reason for Discounting (Business) :</td>";
                html_string += "        <td style='width: 70%; text-align: left' class='cell_desc'><select id='ddl_reason_discounting_bus' style = 'width: 700px'/><option selected='selected'></option></select></td>";
                html_string += "    </tr>";
            }
            else if (this.rem_ciftype == "1" && this.ben_ciftype == "0") {
                html_string += "    <tr style='height: 19px'>";
                html_string += "        <td style='width: 30%; text-align: right' class='cell_caption'>Tellers Reason for Discounting (Individual) :</td>";
                html_string += "        <td style='width: 70%; text-align: left' class='cell_desc'><select id='ddl_reason_discounting' style = 'width: 700px'/><option selected='selected'></option></select></td>";
                html_string += "    </tr>";
                html_string += "    <tr style='height: 19px; display:none'>";
                html_string += "        <td style='width: 30%; text-align: right' class='cell_caption'>Tellers Reason for Discounting (Business) :</td>";
                html_string += "        <td style='width: 70%; text-align: left' class='cell_desc'><select id='ddl_reason_discounting_bus' style = 'width: 700px'/><option selected='selected'></option></select></td>";
                html_string += "    </tr>";
            }
            else if (this.rem_ciftype == "2" && this.ben_ciftype == "0") {
                html_string += "    <tr style='height: 19px; display:none'>";
                html_string += "        <td style='width: 30%; text-align: right' class='cell_caption'>Tellers Reason for Discounting (Individual) :</td>";
                html_string += "        <td style='width: 70%; text-align: left' class='cell_desc'><select id='ddl_reason_discounting' style = 'width: 700px'/><option selected='selected'></option></select></td>";
                html_string += "    </tr>";
                html_string += "    <tr style='height: 19px'>";
                html_string += "        <td style='width: 30%; text-align: right' class='cell_caption'>Tellers Reason for Discounting (Business) :</td>";
                html_string += "        <td style='width: 70%; text-align: left' class='cell_desc'><select id='ddl_reason_discounting_bus' style = 'width: 700px'/><option selected='selected'></option></select></td>";
                html_string += "    </tr>";
            }
            else if (this.rem_ciftype == "0" && this.ben_ciftype == "1") {
                html_string += "    <tr style='height: 19px'>";
                html_string += "        <td style='width: 30%; text-align: right' class='cell_caption'>Tellers Reason for Discounting (Individual) :</td>";
                html_string += "        <td style='width: 70%; text-align: left' class='cell_desc'><select id='ddl_reason_discounting' style = 'width: 700px'/><option selected='selected'></option></select></td>";
                html_string += "    </tr>";
                html_string += "    <tr style='height: 19px; display:none'>";
                html_string += "        <td style='width: 30%; text-align: right' class='cell_caption'>Tellers Reason for Discounting (Business) :</td>";
                html_string += "        <td style='width: 70%; text-align: left' class='cell_desc'><select id='ddl_reason_discounting_bus' style = 'width: 700px'/><option selected='selected'></option></select></td>";
                html_string += "    </tr>";
            }
                //===================================================================================================
                /* RFC-35248 : Enhancement in Bills Payment OFAC Screening Process & Report */
            else if (this.rem_ciftype == "0" && this.ben_ciftype == "2") {
                html_string += "    <tr style='height: 19px; display:none'>";
                html_string += "        <td style='width: 30%; text-align: right' class='cell_caption'>Tellers Reason for Discounting (Individual) :</td>";
                html_string += "        <td style='width: 70%; text-align: left' class='cell_desc'><select id='ddl_reason_discounting' style = 'width: 700px'/><option selected='selected'></option></select></td>";
                html_string += "    </tr>";
                html_string += "    <tr style='height: 19px'>";
                html_string += "        <td style='width: 30%; text-align: right' class='cell_caption'>Tellers Reason for Discounting (Business) :</td>";
                html_string += "        <td style='width: 70%; text-align: left' class='cell_desc'><select id='ddl_reason_discounting_bus' style = 'width: 700px'/><option selected='selected'></option></select></td>";
                html_string += "    </tr>";
                //===================================================================================================
            }
            else if (this.rem_ciftype == "1" && this.ben_ciftype == "2") {
                html_string += "    <tr style='height: 19px'>";
                html_string += "        <td style='width: 30%; text-align: right' class='cell_caption'>Tellers Reason for Discounting (Individual) :</td>";
                html_string += "        <td style='width: 70%; text-align: left' class='cell_desc'><select id='ddl_reason_discounting' style = 'width: 700px'/><option selected='selected'></option></select></td>";
                html_string += "    </tr>";
                html_string += "    <tr style='height: 19px'>";
                html_string += "        <td style='width: 30%; text-align: right' class='cell_caption'>Tellers Reason for Discounting (Business) :</td>";
                html_string += "        <td style='width: 70%; text-align: left' class='cell_desc'><select id='ddl_reason_discounting_bus' style = 'width: 700px'/><option selected='selected'></option></select></td>";
                html_string += "    </tr>";
                //===================================================================================================
            }
            else {
                html_string += "    <tr style='height: 19px'>";
                html_string += "        <td style='width: 30%; text-align: right' class='cell_caption'>Tellers Reason for Discounting (Individual) :</td>";
                html_string += "        <td style='width: 70%; text-align: left' class='cell_desc'><select id='ddl_reason_discounting' style = 'width: 700px'/><option selected='selected'></option></select></td>";
                html_string += "    </tr>";
                html_string += "    <tr style='height: 19px'>";
                html_string += "        <td style='width: 30%; text-align: right' class='cell_caption'>Tellers Reason for Discounting (Business) :</td>";
                html_string += "        <td style='width: 70%; text-align: left' class='cell_desc'><select id='ddl_reason_discounting_bus' style = 'width: 700px'/><option selected='selected'></option></select></td>";
                html_string += "    </tr>";
            }

            html_string += "    <tr style='height: 19px'>";
            html_string += "        <td style = 'width: 30%; text-align: right; display:none' class = 'cell_caption'>Reason Other Details :</td>";
            html_string += "        <td style = 'width: 70%;  text-align: left;'  class = 'cell_desc'><input type = 'text' id = 'txt_reason_disc' style = 'width: 700px; display:none' disabled='disabled'/></td>";
            html_string += "    </tr>";
            html_string += "    <tr style='height: 19px'>";
            html_string += "        <td style='width: 30%; text-align: right' class='cell_caption'>Approvers Disposition :</td>";
            html_string += "        <td style='width: 70%; text-align: left' class='cell_desc'><select id='ddl_approver_disposition' style = 'width: 700px'/><option selected='selected'></option></select></td>";
            html_string += "    </tr>";
            html_string += "    <tr style='height: 19px'>";
            html_string += "        <td style = 'width: 30%; text-align: right;' class = 'cell_caption'>Approvers Remarks :</td>";
            html_string += "        <td style = 'width: 70%;  text-align: left;'  class = 'cell_desc'><input type = 'text' id = 'txt_approver_remarks' style = 'width: 700px'/></td>";
            html_string += "    </tr>";
            html_string += "    <tr style='height: 19px'>";
            html_string += "        <td class = 'cell_desc' colspan='2' style='text-align: center'><label>CLIENT HIT LIST</label></td>";
            html_string += "    </tr>";
            html_string += "    <tr style='height: 19px'>";
            html_string += "        <td colspan='2' style='text-align: center' ><div id = 'div_client_hit_list_all'></div></td>";
            html_string += "    </tr>";
            html_string += "    <tr style='height: 5px'>";
            html_string += "        <td class = 'cell_desc' colspan='2' style='text-align: center'></td>";
            html_string += "    </tr>";
            html_string += "    <tr>";
            html_string += "        <td colspan='2' style='text-align: center'>";
            html_string += "            <table id='tblwatchlistdisplay' cellpadding='0' cellspacing='1' style='width: 100%'>";
            html_string += "                <tr style='height: 19px'>";
            html_string += "                    <td class='cell_caption' style='width: 20%; height: 18px; text-align: right'>Search details via Record No. :</td>";
            html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtrecordno' style = 'width: 98%' /></td>";
            html_string += "                    <td style='text-align: left' class='comment'>Press enter</td>";
            html_string += "                </tr>";
            html_string += "                <tr style='height: 19px'>";
            html_string += "                    <td class='cell_caption' style='width: 20%; height: 18px; text-align: right'>Full Name :</td>";
            html_string += "                    <td colspan='3' class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtFullName' style = 'width: 100%' readonly='readonly'/></td>";
            html_string += "                </tr>";
            html_string += "                <tr style='height: 19px'>";
            html_string += "                    <td class='cell_caption' style='width: 20%; height: 18px; text-align: right'>Last Name :</td>";
            html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtLastName' style = 'width: 98%' readonly='readonly'/></td>";
            html_string += "                    <td class='cell_caption' style='width: 20%; text-align: right'>Alias Last Name :</td>";
            html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtAliasLName' style = 'width: 98%' readonly='readonly'/></td>";
            html_string += "                </tr>";
            html_string += "                <tr style='height: 19px'>";
            html_string += "                    <td class='cell_caption' style='width: 20%; height: 18px; text-align: right'>First Name :</td>";
            html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtFirstName' style = 'width: 98%' readonly='readonly'/></td>";
            html_string += "                    <td class='cell_caption' style='width: 20%; text-align: right'>Alias First Name :</td>";
            html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtAliasFName' style = 'width: 98%' readonly='readonly'/></td>";
            html_string += "                </tr>";
            html_string += "                <tr style='height: 19px'>";
            html_string += "                    <td class='cell_caption' style='width: 20%; height: 18px; text-align: right'>Midde Initial :</td>";
            html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtMiddleInitial' style = 'width: 98%' readonly='readonly'/></td>";
            html_string += "                    <td class='cell_caption' style='width: 20%; text-align: right'>Alias Middle :</td>";
            html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtAliasMName' style = 'width: 98%' readonly='readonly'/></td>";
            html_string += "                </tr>";
            html_string += "                <tr style='height: 19px'>";
            html_string += "                    <td class='cell_caption' style='width: 20%; height: 18px; text-align: right'>Suffix :</td>";
            html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtSuffix' style = 'width: 98%' readonly='readonly'/></td>";
            html_string += "                    <td class='cell_caption' style='width: 20%; text-align: right'>Alias Suffix :</td>";
            html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtAliasSuffix' style = 'width: 98%' readonly='readonly'/></td>";
            html_string += "                </tr>";
            html_string += "                <tr style='height: 19px'>";
            html_string += "                    <td class='cell_caption' style='width: 20%; height: 18px; text-align: right'>Title :</td>";
            html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtTitle' style = 'width: 98%' readonly='readonly'/></td>";
            html_string += "                    <td class='cell_caption' style='width: 20%; text-align: right'>Nationality :</td>";
            html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtNationality' style = 'width: 98%' readonly='readonly'/></td>";
            html_string += "                </tr>";
            html_string += "                <tr style='height: 19px'>";
            html_string += "                    <td class='cell_caption' style='width: 20%; height: 18px; text-align: right'>Date of Birth :</td>";
            html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtDateofBirth' style = 'width: 98%' readonly='readonly'/></td>";
            html_string += "                    <td class='cell_caption' style='width: 20%; text-align: right'>Town of Birth :</td>";
            html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtTownofBirth' style = 'width: 98%' readonly='readonly'/></td>";
            html_string += "                </tr>";
            html_string += "                <tr style='height: 19px'>";
            html_string += "                    <td class='cell_caption' style='width: 20%; height: 18px; text-align: right'>Age :</td>";
            html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtAge' style = 'width: 98%' readonly='readonly'/></td>";
            html_string += "                    <td class='cell_caption' style='width: 20%; text-align: right'>Country of Birth :</td>";
            html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtCountryofBirth' style = 'width: 98%' readonly='readonly'/></td>";
            html_string += "                </tr>";
            html_string += "                <tr style='height: 19px'>";
            html_string += "                    <td class='cell_caption' style='width: 20%; height: 18px; text-align: right'>Position :</td>";
            html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtPosition' style = 'width: 98%' readonly='readonly'/></td>";
            html_string += "                    <td class='cell_caption' style='width: 20%; text-align: right'>Business Name :</td>";
            html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtBusinessName' style = 'width: 98%' readonly='readonly'/></td>";
            html_string += "                </tr>";
            html_string += "                <tr style='height: 19px'>";
            html_string += "                    <td class='cell_caption' style='width: 20%; height: 18px; text-align: right'>Days Listed :</td>";
            html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtDays' style = 'width: 98%' readonly='readonly'/></td>";
            html_string += "                </tr>";
            html_string += "                <tr style='height: 19px'>";
            html_string += "                    <td class='cell_caption' style='width: 20%; height: 18px; text-align: right'>Listed On :</td>";
            html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtListedOn' style = 'width: 98%' readonly='readonly'/></td>";
            html_string += "                    <td class='cell_caption' style='width: 20%; text-align: right'>Last Updated :</td>";
            html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtLastUpdated' style = 'width: 98%' readonly='readonly'/></td>";
            html_string += "                </tr>";
            html_string += "                <tr style='height: 19px'>";
            html_string += "                    <td class='cell_caption' style='width: 20%; height: 18px; text-align: right'>City :</td>";
            html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtCity' style = 'width: 98%' readonly='readonly'/></td>";
            html_string += "                    <td class='cell_caption' style='width: 20%; text-align: right'>State :</td>";
            html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtState' style = 'width: 98%' readonly='readonly'/></td>";
            html_string += "                </tr>";
            html_string += "                <tr style='height: 19px'>";
            html_string += "                    <td class='cell_caption' style='width: 20%; height: 18px; text-align: right'>Country :</td>";
            html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtCountry' style = 'width: 98%' readonly='readonly'/></td>";
            html_string += "                    <td class='cell_caption' style='width: 20%; text-align: right'>Zip Code :</td>";
            html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtZipCode' style = 'width: 98%' readonly='readonly'/></td>";
            html_string += "                </tr>";
            html_string += "                <tr style='height: 19px'>";
            html_string += "                    <td class='cell_caption' style='width: 20%; height: 18px; text-align: right'>Phone Number :</td>";
            html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtPhoneNumber' style = 'width: 98%' readonly='readonly'/></td>";
            html_string += "                    <td class='cell_caption' style='width: 20%; text-align: right'>Passport Details :</td>";
            html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtPassport' style = 'width: 98%' readonly='readonly'/></td>";
            html_string += "                </tr>";
            html_string += "                <tr style='height: 19px'>";
            html_string += "                    <td class='cell_caption' style='width: 20%; height: 18px; text-align: right'>DBA Name :</td>";
            html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtDbaName' style = 'width: 98%' readonly='readonly'/></td>";
            html_string += "                    <td class='cell_caption' style='width: 20%; text-align: right'>Group Type :</td>";
            html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtGroupType' style = 'width: 98%' readonly='readonly'/></td>";
            html_string += "                </tr>";
            html_string += "                <tr style='height: 19px'>";
            html_string += "                    <td class='cell_caption' style='width: 20%; height: 18px; text-align: right'>Alias Type :</td>";
            html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtAliasType' style = 'width: 98%' readonly='readonly'/></td>";
            html_string += "                    <td class='cell_caption' style='width: 20%; text-align: right'>Regime :</td>";
            html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtRegime' style = 'width: 98%' readonly='readonly'/></td>";
            html_string += "                </tr>";
            html_string += "                <tr style='height: 19px'>";
            html_string += "                    <td class='cell_caption' style='width: 20%; height: 18px; text-align: right'>Group ID :</td>";
            html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtGroupId' style = 'width: 98%' readonly='readonly'/></td>";
            html_string += "                    <td class='cell_caption' style='width: 20%; text-align: right'>File Type :</td>";
            html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtRegulatory' style = 'width: 98%' readonly='readonly'/></td>";
            html_string += "                </tr>";
            html_string += "                <tr style='height: 19px'>";
            html_string += "                    <td class='cell_caption' style='width: 20%; height: 18px; text-align: right'>Remarks :</td>";
            html_string += "                    <td colspan='3' class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtRemarks' style = 'width: 98%' readonly='readonly'/></td>";
            html_string += "                </tr>";
            html_string += "            </table>";
            html_string += "        </td>";
            html_string += "    </tr>";
            html_string += "    <tr style='height: 5px'>";
            html_string += "    </tr>"
            //=========================================================================================
        }
        /*END*/

        html_string += "    <tr style='height: 19px'>";
        html_string += "        <td class = 'cell_desc' style = 'text-align: center' colspan='2'>";
        html_string += "            <input id = 'btn_genovr_ok'        type = 'button' value ='OK'     style ='width: 18%; height: 24px' />";
        html_string += "            <input id = 'btn_genovr_ok_cancel' type = 'button' value ='CANCEL' style ='width: 18%; height: 24px'/>";

        if (parseInt(genovr.generic_flag) == 2)
        {
            html_string += "            <input id = 'btn_genovr_ok_remote_ovr' type = 'button' value ='REMOTE OVERRIDE' style ='width: 18%; height: 24px'/>";
        }

        html_string += "        </td>";
        html_string += "    </tr>";
        html_string += "</table>";

        document.getElementById("content_mod_div").innerHTML = html_string;        

        if (this.user_name != null && this.user_name != '')
        {
            document.getElementById("txt_genovr_user_id").value = this.user_name;            
        }
        
        if (this.user_pass != null && this.user_pass != '')
        {
            document.getElementById("txt_genovr_password").value = this.user_pass;
        }

        if (this.sanction_discounting_reason_other != null && this.sanction_discounting_reason_other != '')
        {
            if (document.getElementById("txt_reason_disc") != null)
            {
                document.getElementById("txt_reason_disc").value = this.sanction_discounting_reason_other;
            }
        }

        document.getElementById("btn_genovr_ok").onclick = function()
        {
            genovr.enter_button_clicked();
        }

        document.getElementById("btn_genovr_ok_cancel").onclick = function()
        {
            genovr.cancel_button_clicked();
        }

        //=========================================================================================
        //* RFC-32829 : Request to enhance the OFAC alert and Report in IRSW 01/14/2024 */
        //document.getElementById("ddl_reason_discounting").onchange = function () {
        //    genovr.reason_on_change();
        //}

        //document.getElementById("ddl_reason_discounting_bus").onchange = function () {
        //    genovr.reason_on_change();
        //}

        if (this.sanction_disposition_reason_remarks != null && this.sanction_disposition_reason_remarks != '') {
            if (document.getElementById("txt_approver_remarks") != null) {
                document.getElementById("txt_approver_remarks").value = this.sanction_disposition_reason_remarks;
            }
        }

        document.getElementById("ddl_approver_disposition").onchange = function () {
            genovr.disposition_on_change();
        }

        document.getElementById("txtrecordno").onkeypress = function (ev) {
            var e = window.event || ev;
            var keynum = e.charCode || e.keyCode;

            if (keynum == 13) {
                genovr.display_watchlist_record(document.getElementById("txtrecordno").value);
            }
        }
        //=========================================================================================

        if (document.getElementById("btn_genovr_ok_remote_ovr") != null)
        {
            document.getElementById("btn_genovr_ok_remote_ovr").onclick = function()
            {
                //=========================================================================================
                //* RFC-32829 : Request to enhance the OFAC alert and Report in IRSW 01/14/2024 */
                if ((genovr.rem_ciftype == "1" && genovr.ben_ciftype == "1") && (document.getElementById("ddl_reason_discounting").value == "" || document.getElementById("ddl_reason_discounting").value == "null")) {
                    alert("Please provide Teller Reason for Discounting (Individual)");
                    return;
                }
                else if ((genovr.rem_ciftype == "2" && genovr.ben_ciftype == "2") && (document.getElementById("ddl_reason_discounting_bus").value == "" || document.getElementById("ddl_reason_discounting_bus").value == "null")) {
                    alert("Please provide Teller Reason for Discounting (Business)");
                    return;
                }
                else if ((genovr.rem_ciftype == "1" && genovr.ben_ciftype == "0") && (document.getElementById("ddl_reason_discounting").value == "" || document.getElementById("ddl_reason_discounting").value == "null")) {
                    alert("Please provide Teller Reason for Discounting (Individual)");
                    return;
                }
                else if ((genovr.rem_ciftype == "0" && genovr.ben_ciftype == "1") && (document.getElementById("ddl_reason_discounting").value == "" || document.getElementById("ddl_reason_discounting").value == "null")) {
                    alert("Please provide Teller Reason for Discounting (Individual)");
                    return;
                }
                else if ((genovr.rem_ciftype == "2" && genovr.ben_ciftype == "0") && (document.getElementById("ddl_reason_discounting_bus").value == "" || document.getElementById("ddl_reason_discounting_bus").value == "null")) {
                    alert("Please provide Teller Reason for Discounting (Business)");
                    return;
                }
                else if ((genovr.rem_ciftype == "0" && genovr.ben_ciftype == "2") && (document.getElementById("ddl_reason_discounting_bus").value == "" || document.getElementById("ddl_reason_discounting_bus").value == "null")) {
                    alert("Please provide Teller Reason for Discounting (Business)");
                    return;
                }
                else if ((genovr.rem_ciftype == "1" && genovr.ben_ciftype == "2") && (document.getElementById("ddl_reason_discounting").value == "" || document.getElementById("ddl_reason_discounting").value == "null") && (document.getElementById("ddl_reason_discounting_bus").value == "" || document.getElementById("ddl_reason_discounting_bus").value == "null")) {
                    alert("Please provide Teller Reason for Discounting (Individual Type Remitter).\nPlease provide Teller Reason for Discounting (Business Type Beneficiary).");
                    return;
                }
                else if ((genovr.rem_ciftype == "2" && genovr.ben_ciftype == "1") && (document.getElementById("ddl_reason_discounting").value == "" || document.getElementById("ddl_reason_discounting").value == "null") && (document.getElementById("ddl_reason_discounting_bus").value == "" || document.getElementById("ddl_reason_discounting_bus").value == "null")) {
                    alert("Please provide Teller Reason for Discounting (Business Type Remitter).\nPlease provide Teller Reason for Discounting (Individual Type Beneficiary).");
                    return;
                }
                else if ((genovr.rem_ciftype == "1" && genovr.ben_ciftype == "2") && (document.getElementById("ddl_reason_discounting").value != "" || document.getElementById("ddl_reason_discounting").value != "null") && (document.getElementById("ddl_reason_discounting_bus").value == "" || document.getElementById("ddl_reason_discounting_bus").value == "null")) {
                    alert("Please provide Teller Reason for Discounting (Business Type Beneficiary).");
                    return;
                }
                else if ((genovr.rem_ciftype == "1" && genovr.ben_ciftype == "2") && (document.getElementById("ddl_reason_discounting").value == "" || document.getElementById("ddl_reason_discounting").value == "null") && (document.getElementById("ddl_reason_discounting_bus").value != "" || document.getElementById("ddl_reason_discounting_bus").value != "null")) {
                    alert("Please provide Teller Reason for Discounting (Individual Type Remitter).");
                    return;
                }
                else if ((genovr.rem_ciftype == "2" && genovr.ben_ciftype == "1") && (document.getElementById("ddl_reason_discounting").value != "" || document.getElementById("ddl_reason_discounting").value != "null") && (document.getElementById("ddl_reason_discounting_bus").value == "" || document.getElementById("ddl_reason_discounting_bus").value == "null")) {
                    alert("Please provide Teller Reason for Discounting (Individual Type Beneficiary).");
                    return;
                }
                else if ((genovr.rem_ciftype == "2" && genovr.ben_ciftype == "1") && (document.getElementById("ddl_reason_discounting").value == "" || document.getElementById("ddl_reason_discounting").value == "null") && (document.getElementById("ddl_reason_discounting_bus").value != "" || document.getElementById("ddl_reason_discounting_bus").value != "null")) {
                    alert("Please provide Teller Reason for Discounting (Business Type Remitter).");
                    return;
                }
                //=========================================================================================

                /* NEW */
                var rem_ovr = new rem_ovr_scr();
                rem_ovr.jscls = genovr;
                rem_ovr.chk_branch_is_allowed();

                //=========================================================================================
                /* RFC-30454 Creation of new report in IRSW for OTC transactions with OFAC hits */
                rem_ovr.clear_pending_tranx_session();
                //=========================================================================================
                //=========================================================================================
                //* RFC-32829 : Request to enhance the OFAC alert and Report in IRSW 01/14/2024 */
                rem_ovr.set_pending_tranx_session(document.getElementById("ddl_reason_discounting").value.toString(), document.getElementById("ddl_reason_discounting_bus").value.toString());
                //=========================================================================================
            }
        }

        //=========================================================================================
        //* RFC-32829 : Request to enhance the OFAC alert and Report in IRSW 01/14/2024 */
        this.process_viewer('ajx_loading$');
        //=========================================================================================
    }

    this.construct_generic_screen_refinitiv = function () {
        this.set_genovr_caption();

        var html_string = "";
        var sanction_disc = null;

        /*SANCTION LIST DISCOUNTING - RFS 16165-003 DDC*/
        try {
            sanction_disc = IRSWorld.TranClass.RemTranExtra.discounting_required().value;
        }
        catch (error) {
            sanction_disc = null;
        }
        /*END*/


        html_string = "<table id = 'tbl_otc_generic_override' style='border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; width: 1000px; border-bottom: #336699 1px solid' cellpadding='0' cellspacing='1'>";
        html_string += '        <img align="absBottom" src="../images/rfv_lg_pb_hz_rgb_pos_World-Check.png" width="200px" height="50px"/>&nbsp;';
        html_string += "    <tr><td colspan = '2' style = 'text-align: center' class = 'table_caption'>" + this.generic_caption + "</td></tr>";
        html_string += "    <tr style = 'height: 19px' >";
        html_string += "        <td class = 'cell_caption' style = 'width: 30%; text-align: right'>USER NAME :</td>";
        html_string += "        <td class = 'cell_desc'    style = 'width: 70%; text-align: left'><input type = 'text' id = 'txt_genovr_user_id' /></td>";
        html_string += "</tr>";
        html_string += "    <tr style='height: 19px'>";
        html_string += "        <td style = 'width: 30%; text-align: right;' class = 'cell_caption'>PASSWORD :</td>";
        html_string += "        <td style = 'width:70%;  text-align: left;'  class = 'cell_desc'><input type = 'password' id = 'txt_genovr_password' /></td>";
        html_string += "    </tr>";

        /*SANCTION LIST DISCOUNTING - RFS 16165-003 DDC*/

        if (parseInt(genovr.generic_flag) == 2 && sanction_disc != null && sanction_disc == true) {
            html_string += "    <tr style='height: 5px'>";
            html_string += "        <td style = 'width: 30%; text-align: right; display:none' class = 'cell_caption'></td>";
            html_string += "        <td style = 'width: 70%;  text-align: left; display:none'  class = 'cell_desc'></td>";
            html_string += "    </tr>";

            //=========================================================================================
            //* RFC-32829 : Request to enhance the OFAC alert and Report in IRSW 01/14/2024 */
            //var ciftype_val = IRSWorld.TranClass.RemTranExtra.validate_watchlist(this.remitter_id, this.beneficiary_id, "", "", "", "", "1").value;
            var ciftype_val = IRSWorld.TranClass.RemTranExtra.validate_watchlist_refinitiv(this.remitter_id, this.beneficiary_id, "", "", "", "", "1").value;
            var ciftype_newval = ciftype_val.split('~');
            this.rem_ciftype = ciftype_newval[0];
            this.ben_ciftype = ciftype_newval[1];

            if (this.rem_ciftype == "1" && this.ben_ciftype == "1") {
                html_string += "    <tr style='height: 19px'>";
                html_string += "        <td style='width: 30%; text-align: right' class='cell_caption'>Tellers Reason for Discounting (Individual) :</td>";
                html_string += "        <td style='width: 70%; text-align: left' class='cell_desc'><select id='ddl_reason_discounting' style = 'width: 700px'/><option selected='selected'></option></select></td>";
                html_string += "    </tr>";
                html_string += "    <tr style='height: 19px; display:none'>";
                html_string += "        <td style='width: 30%; text-align: right' class='cell_caption'>Tellers Reason for Discounting (Business) :</td>";
                html_string += "        <td style='width: 70%; text-align: left' class='cell_desc'><select id='ddl_reason_discounting_bus' style = 'width: 700px'/><option selected='selected'></option></select></td>";
                html_string += "    </tr>";
            }
            else if (this.rem_ciftype == "2" && this.ben_ciftype == "2") {
                html_string += "    <tr style='height: 19px; display:none'>";
                html_string += "        <td style='width: 30%; text-align: right' class='cell_caption'>Tellers Reason for Discounting (Individual) :</td>";
                html_string += "        <td style='width: 70%; text-align: left' class='cell_desc'><select id='ddl_reason_discounting' style = 'width: 700px'/><option selected='selected'></option></select></td>";
                html_string += "    </tr>";
                html_string += "    <tr style='height: 19px'>";
                html_string += "        <td style='width: 30%; text-align: right' class='cell_caption'>Tellers Reason for Discounting (Business) :</td>";
                html_string += "        <td style='width: 70%; text-align: left' class='cell_desc'><select id='ddl_reason_discounting_bus' style = 'width: 700px'/><option selected='selected'></option></select></td>";
                html_string += "    </tr>";
            }
            else if (this.rem_ciftype == "1" && this.ben_ciftype == "0") {
                html_string += "    <tr style='height: 19px'>";
                html_string += "        <td style='width: 30%; text-align: right' class='cell_caption'>Tellers Reason for Discounting (Individual) :</td>";
                html_string += "        <td style='width: 70%; text-align: left' class='cell_desc'><select id='ddl_reason_discounting' style = 'width: 700px'/><option selected='selected'></option></select></td>";
                html_string += "    </tr>";
                html_string += "    <tr style='height: 19px; display:none'>";
                html_string += "        <td style='width: 30%; text-align: right' class='cell_caption'>Tellers Reason for Discounting (Business) :</td>";
                html_string += "        <td style='width: 70%; text-align: left' class='cell_desc'><select id='ddl_reason_discounting_bus' style = 'width: 700px'/><option selected='selected'></option></select></td>";
                html_string += "    </tr>";
            }
            else if (this.rem_ciftype == "2" && this.ben_ciftype == "0") {
                html_string += "    <tr style='height: 19px; display:none'>";
                html_string += "        <td style='width: 30%; text-align: right' class='cell_caption'>Tellers Reason for Discounting (Individual) :</td>";
                html_string += "        <td style='width: 70%; text-align: left' class='cell_desc'><select id='ddl_reason_discounting' style = 'width: 700px'/><option selected='selected'></option></select></td>";
                html_string += "    </tr>";
                html_string += "    <tr style='height: 19px'>";
                html_string += "        <td style='width: 30%; text-align: right' class='cell_caption'>Tellers Reason for Discounting (Business) :</td>";
                html_string += "        <td style='width: 70%; text-align: left' class='cell_desc'><select id='ddl_reason_discounting_bus' style = 'width: 700px'/><option selected='selected'></option></select></td>";
                html_string += "    </tr>";
            }
            else if (this.rem_ciftype == "0" && this.ben_ciftype == "1") {
                html_string += "    <tr style='height: 19px'>";
                html_string += "        <td style='width: 30%; text-align: right' class='cell_caption'>Tellers Reason for Discounting (Individual) :</td>";
                html_string += "        <td style='width: 70%; text-align: left' class='cell_desc'><select id='ddl_reason_discounting' style = 'width: 700px'/><option selected='selected'></option></select></td>";
                html_string += "    </tr>";
                html_string += "    <tr style='height: 19px; display:none'>";
                html_string += "        <td style='width: 30%; text-align: right' class='cell_caption'>Tellers Reason for Discounting (Business) :</td>";
                html_string += "        <td style='width: 70%; text-align: left' class='cell_desc'><select id='ddl_reason_discounting_bus' style = 'width: 700px'/><option selected='selected'></option></select></td>";
                html_string += "    </tr>";
            }
                //===================================================================================================
                /* RFC-35248 : Enhancement in Bills Payment OFAC Screening Process & Report */
            else if (this.rem_ciftype == "0" && this.ben_ciftype == "2") {
                html_string += "    <tr style='height: 19px; display:none'>";
                html_string += "        <td style='width: 30%; text-align: right' class='cell_caption'>Tellers Reason for Discounting (Individual) :</td>";
                html_string += "        <td style='width: 70%; text-align: left' class='cell_desc'><select id='ddl_reason_discounting' style = 'width: 700px'/><option selected='selected'></option></select></td>";
                html_string += "    </tr>";
                html_string += "    <tr style='height: 19px'>";
                html_string += "        <td style='width: 30%; text-align: right' class='cell_caption'>Tellers Reason for Discounting (Business) :</td>";
                html_string += "        <td style='width: 70%; text-align: left' class='cell_desc'><select id='ddl_reason_discounting_bus' style = 'width: 700px'/><option selected='selected'></option></select></td>";
                html_string += "    </tr>";
                //===================================================================================================
            }
            else {
                html_string += "    <tr style='height: 19px'>";
                html_string += "        <td style='width: 30%; text-align: right' class='cell_caption'>Tellers Reason for Discounting (Individual) :</td>";
                html_string += "        <td style='width: 70%; text-align: left' class='cell_desc'><select id='ddl_reason_discounting' style = 'width: 700px'/><option selected='selected'></option></select></td>";
                html_string += "    </tr>";
                html_string += "    <tr style='height: 19px'>";
                html_string += "        <td style='width: 30%; text-align: right' class='cell_caption'>Tellers Reason for Discounting (Business) :</td>";
                html_string += "        <td style='width: 70%; text-align: left' class='cell_desc'><select id='ddl_reason_discounting_bus' style = 'width: 700px'/><option selected='selected'></option></select></td>";
                html_string += "    </tr>";
            }

            html_string += "    <tr style='height: 19px'>";
            html_string += "        <td style = 'width: 30%; text-align: right; display:none' class = 'cell_caption'>Reason Other Details :</td>";
            html_string += "        <td style = 'width: 70%;  text-align: left;'  class = 'cell_desc'><input type = 'text' id = 'txt_reason_disc' style = 'width: 700px; display:none' disabled='disabled'/></td>";
            html_string += "    </tr>";
            html_string += "    <tr style='height: 19px'>";
            html_string += "        <td style='width: 30%; text-align: right' class='cell_caption'>Approvers Disposition :</td>";
            html_string += "        <td style='width: 70%; text-align: left' class='cell_desc'><select id='ddl_approver_disposition' style = 'width: 700px'/><option selected='selected'></option></select></td>";
            html_string += "    </tr>";
            html_string += "    <tr style='height: 19px'>";
            html_string += "        <td style = 'width: 30%; text-align: right;' class = 'cell_caption'>Approvers Remarks :</td>";
            html_string += "        <td style = 'width: 70%;  text-align: left;'  class = 'cell_desc'><input type = 'text' id = 'txt_approver_remarks' style = 'width: 700px'/></td>";
            html_string += "    </tr>";
            html_string += "    <tr style='height: 19px'>";
            html_string += "        <td class = 'cell_desc' colspan='2' style='text-align: center'><label>CLIENT HIT LIST</label></td>";
            html_string += "    </tr>";
            //html_string += "    <tr style='height: 19px'>";
            //html_string += "        <td colspan='2' style='text-align: center' ><div id = 'div_client_hit_list_all'></div></td>";
            //html_string += "    </tr>";
            //html_string += "    <tr style='height: 5px'>";
            //html_string += "        <td class = 'cell_desc' colspan='2' style='text-align: center'></td>";
            //html_string += "    </tr>";
            //html_string += "    <tr>";
            //html_string += "        <td colspan='2' style='text-align: center'>";
            html_string += '   <tr><table id="tblHitList" class="table table-striped table-bordered" style="width:auto">';
            html_string += '       <thead>';
            html_string += '           <tr>';
            html_string += '               <th class="text-center">Match Score</th>';
            html_string += '               <th class="text-center">Matched Name Type</th>';
            html_string += '               <th class="text-center">Primary Name</th>';
            html_string += '               <th class="text-center">Category</th>';
            html_string += '               <th class="text-center">Date Of Birth</th>';
            html_string += '               <th class="text-center">Nationality</th>';
            html_string += '               <th class="text-center">Special Interest Category</th>';
            html_string += '               <th class="text-center">Provider Type</th>';
            html_string += '               <th class="text-center">Alias</th>';
            html_string += '               <th class="text-center">Remarks</th>';
            html_string += '               <th class="text-center">Resolution</th>';
            html_string += '               <th class="text-center">Resolution Remarks</th>';
            html_string += '               <th class="text-center">Created Date</th>';
            html_string += '               <th class="text-center">Updated Date</th>';
            html_string += '           </tr>';
            html_string += '       </thead>';
            html_string += '   </table></tr>'
            //html_string += "            <table id='tblwatchlistdisplay' cellpadding='0' cellspacing='1' style='width: 100%'>";
            //html_string += "                <tr style='height: 19px'>";
            //html_string += "                    <td class='cell_caption' style='width: 20%; height: 18px; text-align: right'>Search details via Record No. :</td>";
            //html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtrecordno' style = 'width: 98%' /></td>";
            //html_string += "                    <td style='text-align: left' class='comment'>Press enter</td>";
            //html_string += "                </tr>";
            //html_string += "                <tr style='height: 19px'>";
            //html_string += "                    <td class='cell_caption' style='width: 20%; height: 18px; text-align: right'>Full Name :</td>";
            //html_string += "                    <td colspan='3' class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtFullName' style = 'width: 100%' readonly='readonly'/></td>";
            //html_string += "                </tr>";
            //html_string += "                <tr style='height: 19px'>";
            //html_string += "                    <td class='cell_caption' style='width: 20%; height: 18px; text-align: right'>Last Name :</td>";
            //html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtLastName' style = 'width: 98%' readonly='readonly'/></td>";
            //html_string += "                    <td class='cell_caption' style='width: 20%; text-align: right'>Alias Last Name :</td>";
            //html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtAliasLName' style = 'width: 98%' readonly='readonly'/></td>";
            //html_string += "                </tr>";
            //html_string += "                <tr style='height: 19px'>";
            //html_string += "                    <td class='cell_caption' style='width: 20%; height: 18px; text-align: right'>First Name :</td>";
            //html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtFirstName' style = 'width: 98%' readonly='readonly'/></td>";
            //html_string += "                    <td class='cell_caption' style='width: 20%; text-align: right'>Alias First Name :</td>";
            //html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtAliasFName' style = 'width: 98%' readonly='readonly'/></td>";
            //html_string += "                </tr>";
            //html_string += "                <tr style='height: 19px'>";
            //html_string += "                    <td class='cell_caption' style='width: 20%; height: 18px; text-align: right'>Midde Initial :</td>";
            //html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtMiddleInitial' style = 'width: 98%' readonly='readonly'/></td>";
            //html_string += "                    <td class='cell_caption' style='width: 20%; text-align: right'>Alias Middle :</td>";
            //html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtAliasMName' style = 'width: 98%' readonly='readonly'/></td>";
            //html_string += "                </tr>";
            //html_string += "                <tr style='height: 19px'>";
            //html_string += "                    <td class='cell_caption' style='width: 20%; height: 18px; text-align: right'>Suffix :</td>";
            //html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtSuffix' style = 'width: 98%' readonly='readonly'/></td>";
            //html_string += "                    <td class='cell_caption' style='width: 20%; text-align: right'>Alias Suffix :</td>";
            //html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtAliasSuffix' style = 'width: 98%' readonly='readonly'/></td>";
            //html_string += "                </tr>";
            //html_string += "                <tr style='height: 19px'>";
            //html_string += "                    <td class='cell_caption' style='width: 20%; height: 18px; text-align: right'>Title :</td>";
            //html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtTitle' style = 'width: 98%' readonly='readonly'/></td>";
            //html_string += "                    <td class='cell_caption' style='width: 20%; text-align: right'>Nationality :</td>";
            //html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtNationality' style = 'width: 98%' readonly='readonly'/></td>";
            //html_string += "                </tr>";
            //html_string += "                <tr style='height: 19px'>";
            //html_string += "                    <td class='cell_caption' style='width: 20%; height: 18px; text-align: right'>Date of Birth :</td>";
            //html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtDateofBirth' style = 'width: 98%' readonly='readonly'/></td>";
            //html_string += "                    <td class='cell_caption' style='width: 20%; text-align: right'>Town of Birth :</td>";
            //html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtTownofBirth' style = 'width: 98%' readonly='readonly'/></td>";
            //html_string += "                </tr>";
            //html_string += "                <tr style='height: 19px'>";
            //html_string += "                    <td class='cell_caption' style='width: 20%; height: 18px; text-align: right'>Age :</td>";
            //html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtAge' style = 'width: 98%' readonly='readonly'/></td>";
            //html_string += "                    <td class='cell_caption' style='width: 20%; text-align: right'>Country of Birth :</td>";
            //html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtCountryofBirth' style = 'width: 98%' readonly='readonly'/></td>";
            //html_string += "                </tr>";
            //html_string += "                <tr style='height: 19px'>";
            //html_string += "                    <td class='cell_caption' style='width: 20%; height: 18px; text-align: right'>Position :</td>";
            //html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtPosition' style = 'width: 98%' readonly='readonly'/></td>";
            //html_string += "                    <td class='cell_caption' style='width: 20%; text-align: right'>Business Name :</td>";
            //html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtBusinessName' style = 'width: 98%' readonly='readonly'/></td>";
            //html_string += "                </tr>";
            //html_string += "                <tr style='height: 19px'>";
            //html_string += "                    <td class='cell_caption' style='width: 20%; height: 18px; text-align: right'>Days Listed :</td>";
            //html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtDays' style = 'width: 98%' readonly='readonly'/></td>";
            //html_string += "                </tr>";
            //html_string += "                <tr style='height: 19px'>";
            //html_string += "                    <td class='cell_caption' style='width: 20%; height: 18px; text-align: right'>Listed On :</td>";
            //html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtListedOn' style = 'width: 98%' readonly='readonly'/></td>";
            //html_string += "                    <td class='cell_caption' style='width: 20%; text-align: right'>Last Updated :</td>";
            //html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtLastUpdated' style = 'width: 98%' readonly='readonly'/></td>";
            //html_string += "                </tr>";
            //html_string += "                <tr style='height: 19px'>";
            //html_string += "                    <td class='cell_caption' style='width: 20%; height: 18px; text-align: right'>City :</td>";
            //html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtCity' style = 'width: 98%' readonly='readonly'/></td>";
            //html_string += "                    <td class='cell_caption' style='width: 20%; text-align: right'>State :</td>";
            //html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtState' style = 'width: 98%' readonly='readonly'/></td>";
            //html_string += "                </tr>";
            //html_string += "                <tr style='height: 19px'>";
            //html_string += "                    <td class='cell_caption' style='width: 20%; height: 18px; text-align: right'>Country :</td>";
            //html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtCountry' style = 'width: 98%' readonly='readonly'/></td>";
            //html_string += "                    <td class='cell_caption' style='width: 20%; text-align: right'>Zip Code :</td>";
            //html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtZipCode' style = 'width: 98%' readonly='readonly'/></td>";
            //html_string += "                </tr>";
            //html_string += "                <tr style='height: 19px'>";
            //html_string += "                    <td class='cell_caption' style='width: 20%; height: 18px; text-align: right'>Phone Number :</td>";
            //html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtPhoneNumber' style = 'width: 98%' readonly='readonly'/></td>";
            //html_string += "                    <td class='cell_caption' style='width: 20%; text-align: right'>Passport Details :</td>";
            //html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtPassport' style = 'width: 98%' readonly='readonly'/></td>";
            //html_string += "                </tr>";
            //html_string += "                <tr style='height: 19px'>";
            //html_string += "                    <td class='cell_caption' style='width: 20%; height: 18px; text-align: right'>DBA Name :</td>";
            //html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtDbaName' style = 'width: 98%' readonly='readonly'/></td>";
            //html_string += "                    <td class='cell_caption' style='width: 20%; text-align: right'>Group Type :</td>";
            //html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtGroupType' style = 'width: 98%' readonly='readonly'/></td>";
            //html_string += "                </tr>";
            //html_string += "                <tr style='height: 19px'>";
            //html_string += "                    <td class='cell_caption' style='width: 20%; height: 18px; text-align: right'>Alias Type :</td>";
            //html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtAliasType' style = 'width: 98%' readonly='readonly'/></td>";
            //html_string += "                    <td class='cell_caption' style='width: 20%; text-align: right'>Regime :</td>";
            //html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtRegime' style = 'width: 98%' readonly='readonly'/></td>";
            //html_string += "                </tr>";
            //html_string += "                <tr style='height: 19px'>";
            //html_string += "                    <td class='cell_caption' style='width: 20%; height: 18px; text-align: right'>Group ID :</td>";
            //html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtGroupId' style = 'width: 98%' readonly='readonly'/></td>";
            //html_string += "                    <td class='cell_caption' style='width: 20%; text-align: right'>File Type :</td>";
            //html_string += "                    <td class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtRegulatory' style = 'width: 98%' readonly='readonly'/></td>";
            //html_string += "                </tr>";
            //html_string += "                <tr style='height: 19px'>";
            //html_string += "                    <td class='cell_caption' style='width: 20%; height: 18px; text-align: right'>Remarks :</td>";
            //html_string += "                    <td colspan='3' class='cell_desc' style='width: 30%; text-align: left'><input type = 'text' id = 'txtRemarks' style = 'width: 98%' readonly='readonly'/></td>";
            //html_string += "                </tr>";
            //html_string += "            </table>";
            //html_string += "        </td>";
            //html_string += "    </tr>";
            html_string += "    <tr style='height: 5px'>";
            html_string += "    </tr>"
            //=========================================================================================
        }
        /*END*/

        html_string += "    <tr style='height: 19px'>";
        html_string += "        <td class = 'cell_desc' style = 'text-align: center' colspan='2'>";
        html_string += "            <input id = 'btn_genovr_ok'        type = 'button' value ='OK'     style ='width: 18%; height: 24px' />";
        html_string += "            <input id = 'btn_genovr_ok_cancel' type = 'button' value ='CANCEL' style ='width: 18%; height: 24px'/>";

        if (parseInt(genovr.generic_flag) == 2) {
            html_string += "            <input id = 'btn_genovr_ok_remote_ovr' type = 'button' value ='REMOTE OVERRIDE' style ='width: 18%; height: 24px'/>";
        }

        html_string += "        </td>";
        html_string += "    </tr>";
        html_string += "</table>";

        document.getElementById("content_mod_div").innerHTML = html_string;

        if (this.user_name != null && this.user_name != '') {
            document.getElementById("txt_genovr_user_id").value = this.user_name;
        }

        if (this.user_pass != null && this.user_pass != '') {
            document.getElementById("txt_genovr_password").value = this.user_pass;
        }

        if (this.sanction_discounting_reason_other != null && this.sanction_discounting_reason_other != '') {
            if (document.getElementById("txt_reason_disc") != null) {
                document.getElementById("txt_reason_disc").value = this.sanction_discounting_reason_other;
            }
        }

        document.getElementById("btn_genovr_ok").onclick = function () {
            genovr.enter_button_clicked();
        }

        document.getElementById("btn_genovr_ok_cancel").onclick = function () {
            genovr.cancel_button_clicked();
        }

        //=========================================================================================
        //* RFC-32829 : Request to enhance the OFAC alert and Report in IRSW 01/14/2024 */
        //document.getElementById("ddl_reason_discounting").onchange = function () {
        //    genovr.reason_on_change();
        //}

        //document.getElementById("ddl_reason_discounting_bus").onchange = function () {
        //    genovr.reason_on_change();
        //}

        if (this.sanction_disposition_reason_remarks != null && this.sanction_disposition_reason_remarks != '') {
            if (document.getElementById("txt_approver_remarks") != null) {
                document.getElementById("txt_approver_remarks").value = this.sanction_disposition_reason_remarks;
            }
        }

        document.getElementById("ddl_approver_disposition").onchange = function () {
            genovr.disposition_on_change();
        }

        //document.getElementById("txtrecordno").onkeypress = function (ev) {
        //    var e = window.event || ev;
        //    var keynum = e.charCode || e.keyCode;

        //    if (keynum == 13) {
        //        genovr.display_watchlist_record(document.getElementById("txtrecordno").value);
        //    }
        //}
        //=========================================================================================

        if (document.getElementById("btn_genovr_ok_remote_ovr") != null) {
            document.getElementById("btn_genovr_ok_remote_ovr").onclick = function () {
                //=========================================================================================
                //* RFC-32829 : Request to enhance the OFAC alert and Report in IRSW 01/14/2024 */
                if ((genovr.rem_ciftype == "1" && genovr.ben_ciftype == "1") && (document.getElementById("ddl_reason_discounting").value == "" || document.getElementById("ddl_reason_discounting").value == "null")) {
                    alert("Please provide Teller Reason for Discounting (Individual)");
                    return;
                }
                else if ((genovr.rem_ciftype == "2" && genovr.ben_ciftype == "2") && (document.getElementById("ddl_reason_discounting_bus").value == "" || document.getElementById("ddl_reason_discounting_bus").value == "null")) {
                    alert("Please provide Teller Reason for Discounting (Business)");
                    return;
                }
                else if ((genovr.rem_ciftype == "1" && genovr.ben_ciftype == "0") && (document.getElementById("ddl_reason_discounting").value == "" || document.getElementById("ddl_reason_discounting").value == "null")) {
                    alert("Please provide Teller Reason for Discounting (Individual)");
                    return;
                }
                else if ((genovr.rem_ciftype == "0" && genovr.ben_ciftype == "1") && (document.getElementById("ddl_reason_discounting").value == "" || document.getElementById("ddl_reason_discounting").value == "null")) {
                    alert("Please provide Teller Reason for Discounting (Individual)");
                    return;
                }
                else if ((genovr.rem_ciftype == "2" && genovr.ben_ciftype == "0") && (document.getElementById("ddl_reason_discounting_bus").value == "" || document.getElementById("ddl_reason_discounting_bus").value == "null")) {
                    alert("Please provide Teller Reason for Discounting (Business)");
                    return;
                }
                else if ((genovr.rem_ciftype == "0" && genovr.ben_ciftype == "2") && (document.getElementById("ddl_reason_discounting_bus").value == "" || document.getElementById("ddl_reason_discounting_bus").value == "null")) {
                    alert("Please provide Teller Reason for Discounting (Business)");
                    return;
                }
                else if ((genovr.rem_ciftype == "1" && genovr.ben_ciftype == "2") && (document.getElementById("ddl_reason_discounting").value == "" || document.getElementById("ddl_reason_discounting").value == "null") && (document.getElementById("ddl_reason_discounting_bus").value == "" || document.getElementById("ddl_reason_discounting_bus").value == "null")) {
                    alert("Please provide Teller Reason for Discounting (Individual Type Remitter).\nPlease provide Teller Reason for Discounting (Business Type Beneficiary).");
                    return;
                }
                else if ((genovr.rem_ciftype == "2" && genovr.ben_ciftype == "1") && (document.getElementById("ddl_reason_discounting").value == "" || document.getElementById("ddl_reason_discounting").value == "null") && (document.getElementById("ddl_reason_discounting_bus").value == "" || document.getElementById("ddl_reason_discounting_bus").value == "null")) {
                    alert("Please provide Teller Reason for Discounting (Business Type Remitter).\nPlease provide Teller Reason for Discounting (Individual Type Beneficiary).");
                    return;
                }
                else if ((genovr.rem_ciftype == "1" && genovr.ben_ciftype == "2") && (document.getElementById("ddl_reason_discounting").value != "" || document.getElementById("ddl_reason_discounting").value != "null") && (document.getElementById("ddl_reason_discounting_bus").value == "" || document.getElementById("ddl_reason_discounting_bus").value == "null")) {
                    alert("Please provide Teller Reason for Discounting (Business Type Beneficiary).");
                    return;
                }
                else if ((genovr.rem_ciftype == "1" && genovr.ben_ciftype == "2") && (document.getElementById("ddl_reason_discounting").value == "" || document.getElementById("ddl_reason_discounting").value == "null") && (document.getElementById("ddl_reason_discounting_bus").value != "" || document.getElementById("ddl_reason_discounting_bus").value != "null")) {
                    alert("Please provide Teller Reason for Discounting (Individual Type Remitter).");
                    return;
                }
                else if ((genovr.rem_ciftype == "2" && genovr.ben_ciftype == "1") && (document.getElementById("ddl_reason_discounting").value != "" || document.getElementById("ddl_reason_discounting").value != "null") && (document.getElementById("ddl_reason_discounting_bus").value == "" || document.getElementById("ddl_reason_discounting_bus").value == "null")) {
                    alert("Please provide Teller Reason for Discounting (Individual Type Beneficiary).");
                    return;
                }
                else if ((genovr.rem_ciftype == "2" && genovr.ben_ciftype == "1") && (document.getElementById("ddl_reason_discounting").value == "" || document.getElementById("ddl_reason_discounting").value == "null") && (document.getElementById("ddl_reason_discounting_bus").value != "" || document.getElementById("ddl_reason_discounting_bus").value != "null")) {
                    alert("Please provide Teller Reason for Discounting (Business Type Remitter).");
                    return;
                }
                //=========================================================================================

                /* NEW */
                var rem_ovr = new rem_ovr_scr();
                rem_ovr.jscls = genovr;
                rem_ovr.chk_branch_is_allowed();

                //=========================================================================================
                /* RFC-30454 Creation of new report in IRSW for OTC transactions with OFAC hits */
                rem_ovr.clear_pending_tranx_session();
                //=========================================================================================
                //=========================================================================================
                //* RFC-32829 : Request to enhance the OFAC alert and Report in IRSW 01/14/2024 */
                rem_ovr.set_pending_tranx_session(document.getElementById("ddl_reason_discounting").value.toString(), document.getElementById("ddl_reason_discounting_bus").value.toString());
                //=========================================================================================
            }
        }

        //=========================================================================================
        //* RFC-32829 : Request to enhance the OFAC alert and Report in IRSW 01/14/2024 */
        //this.process_viewer('ajx_loading$');
        //=========================================================================================
    }


    this.construct_generic_screen_refinitivisdown = function () {
        this.set_genovr_caption();

        var html_string = "";
        var sanction_disc = null;

        /*SANCTION LIST DISCOUNTING - RFS 16165-003 DDC*/
        try {
            sanction_disc = IRSWorld.TranClass.RemTranExtra.discounting_required().value;
        }
        catch (error) {
            sanction_disc = null;
        }
        /*END*/


        html_string = "<table id = 'tbl_otc_generic_override' style='border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; width: 1000px; border-bottom: #336699 1px solid' cellpadding='0' cellspacing='1'>";
        html_string += '        <img align="absBottom" src="../images/rfv_lg_pb_hz_rgb_pos_World-Check.png" width="200px" height="50px"/>&nbsp;';
        html_string += "    <tr><td colspan = '2' style = 'text-align: center' class = 'table_caption'> REFINITIV IS DOWN. </td></tr>";
        //html_string += "    <tr style = 'height: 19px' >";
        //html_string += "        <td class = 'cell_caption' style = 'width: 30%; text-align: right'>USER NAME :</td>";
        //html_string += "        <td class = 'cell_desc'    style = 'width: 70%; text-align: left'><input type = 'text' id = 'txt_genovr_user_id' /></td>";
        //html_string += "</tr>";
        //html_string += "    <tr style='height: 19px'>";
        //html_string += "        <td style = 'width: 30%; text-align: right;' class = 'cell_caption'>PASSWORD :</td>";
        //html_string += "        <td style = 'width:70%;  text-align: left;'  class = 'cell_desc'><input type = 'password' id = 'txt_genovr_password' /></td>";
        //html_string += "    </tr>";

        /*SANCTION LIST DISCOUNTING - RFS 16165-003 DDC*/

        if (parseInt(genovr.generic_flag) == 2 && sanction_disc != null && sanction_disc == true) {
            html_string += "    <tr style='height: 5px'>";
            html_string += "        <td style = 'width: 30%; text-align: right; display:none' class = 'cell_caption'></td>";
            html_string += "        <td style = 'width: 70%;  text-align: left; display:none'  class = 'cell_desc'></td>";
            html_string += "    </tr>";

            //=========================================================================================
            //* RFC-32829 : Request to enhance the OFAC alert and Report in IRSW 01/14/2024 */
            //var ciftype_val = IRSWorld.TranClass.RemTranExtra.validate_watchlist(this.remitter_id, this.beneficiary_id, "", "", "", "", "1").value;
            var ciftype_val = IRSWorld.TranClass.RemTranExtra.validate_watchlist_refinitivisdown(this.remitter_id, this.beneficiary_id, "", "", "", "", "1").value;
            var ciftype_newval = ciftype_val.split('~');
            this.rem_ciftype = ciftype_newval[0];
            this.ben_ciftype = ciftype_newval[1];

            if (this.rem_ciftype == "1" && this.ben_ciftype == "1") {
                html_string += "    <tr style='height: 19px'>";
                html_string += "        <td style='width: 30%; text-align: right' class='cell_caption'>Tellers Reason for Discounting (Individual) :</td>";
                html_string += "        <td style='width: 70%; text-align: left' class='cell_desc'><select id='ddl_reason_discounting' style = 'width: 700px'/><option selected='selected'></option></select></td>";
                html_string += "    </tr>";
                html_string += "    <tr style='height: 19px; display:none'>";
                html_string += "        <td style='width: 30%; text-align: right' class='cell_caption'>Tellers Reason for Discounting (Business) :</td>";
                html_string += "        <td style='width: 70%; text-align: left' class='cell_desc'><select id='ddl_reason_discounting_bus' style = 'width: 700px'/><option selected='selected'></option></select></td>";
                html_string += "    </tr>";
            }
            else if (this.rem_ciftype == "2" && this.ben_ciftype == "2") {
                html_string += "    <tr style='height: 19px; display:none'>";
                html_string += "        <td style='width: 30%; text-align: right' class='cell_caption'>Tellers Reason for Discounting (Individual) :</td>";
                html_string += "        <td style='width: 70%; text-align: left' class='cell_desc'><select id='ddl_reason_discounting' style = 'width: 700px'/><option selected='selected'></option></select></td>";
                html_string += "    </tr>";
                html_string += "    <tr style='height: 19px'>";
                html_string += "        <td style='width: 30%; text-align: right' class='cell_caption'>Tellers Reason for Discounting (Business) :</td>";
                html_string += "        <td style='width: 70%; text-align: left' class='cell_desc'><select id='ddl_reason_discounting_bus' style = 'width: 700px'/><option selected='selected'></option></select></td>";
                html_string += "    </tr>";
            }
            else if (this.rem_ciftype == "1" && this.ben_ciftype == "0") {
                html_string += "    <tr style='height: 19px'>";
                html_string += "        <td style='width: 30%; text-align: right' class='cell_caption'>Tellers Reason for Discounting (Individual) :</td>";
                html_string += "        <td style='width: 70%; text-align: left' class='cell_desc'><select id='ddl_reason_discounting' style = 'width: 700px'/><option selected='selected'></option></select></td>";
                html_string += "    </tr>";
                html_string += "    <tr style='height: 19px; display:none'>";
                html_string += "        <td style='width: 30%; text-align: right' class='cell_caption'>Tellers Reason for Discounting (Business) :</td>";
                html_string += "        <td style='width: 70%; text-align: left' class='cell_desc'><select id='ddl_reason_discounting_bus' style = 'width: 700px'/><option selected='selected'></option></select></td>";
                html_string += "    </tr>";
            }
            else if (this.rem_ciftype == "2" && this.ben_ciftype == "0") {
                html_string += "    <tr style='height: 19px; display:none'>";
                html_string += "        <td style='width: 30%; text-align: right' class='cell_caption'>Tellers Reason for Discounting (Individual) :</td>";
                html_string += "        <td style='width: 70%; text-align: left' class='cell_desc'><select id='ddl_reason_discounting' style = 'width: 700px'/><option selected='selected'></option></select></td>";
                html_string += "    </tr>";
                html_string += "    <tr style='height: 19px'>";
                html_string += "        <td style='width: 30%; text-align: right' class='cell_caption'>Tellers Reason for Discounting (Business) :</td>";
                html_string += "        <td style='width: 70%; text-align: left' class='cell_desc'><select id='ddl_reason_discounting_bus' style = 'width: 700px'/><option selected='selected'></option></select></td>";
                html_string += "    </tr>";
            }
            else if (this.rem_ciftype == "0" && this.ben_ciftype == "1") {
                html_string += "    <tr style='height: 19px'>";
                html_string += "        <td style='width: 30%; text-align: right' class='cell_caption'>Tellers Reason for Discounting (Individual) :</td>";
                html_string += "        <td style='width: 70%; text-align: left' class='cell_desc'><select id='ddl_reason_discounting' style = 'width: 700px'/><option selected='selected'></option></select></td>";
                html_string += "    </tr>";
                html_string += "    <tr style='height: 19px; display:none'>";
                html_string += "        <td style='width: 30%; text-align: right' class='cell_caption'>Tellers Reason for Discounting (Business) :</td>";
                html_string += "        <td style='width: 70%; text-align: left' class='cell_desc'><select id='ddl_reason_discounting_bus' style = 'width: 700px'/><option selected='selected'></option></select></td>";
                html_string += "    </tr>";
            }
                //===================================================================================================
                /* RFC-35248 : Enhancement in Bills Payment OFAC Screening Process & Report */
            else if (this.rem_ciftype == "0" && this.ben_ciftype == "2") {
                html_string += "    <tr style='height: 19px; display:none'>";
                html_string += "        <td style='width: 30%; text-align: right' class='cell_caption'>Tellers Reason for Discounting (Individual) :</td>";
                html_string += "        <td style='width: 70%; text-align: left' class='cell_desc'><select id='ddl_reason_discounting' style = 'width: 700px'/><option selected='selected'></option></select></td>";
                html_string += "    </tr>";
                html_string += "    <tr style='height: 19px'>";
                html_string += "        <td style='width: 30%; text-align: right' class='cell_caption'>Tellers Reason for Discounting (Business) :</td>";
                html_string += "        <td style='width: 70%; text-align: left' class='cell_desc'><select id='ddl_reason_discounting_bus' style = 'width: 700px'/><option selected='selected'></option></select></td>";
                html_string += "    </tr>";
                //===================================================================================================
            }
            else {
                html_string += "    <tr style='height: 19px'>";
                html_string += "        <td style='width: 30%; text-align: right' class='cell_caption'>Tellers Reason for Discounting (Individual) :</td>";
                html_string += "        <td style='width: 70%; text-align: left' class='cell_desc'><select id='ddl_reason_discounting' style = 'width: 700px'/><option selected='selected'></option></select></td>";
                html_string += "    </tr>";
                html_string += "    <tr style='height: 19px'>";
                html_string += "        <td style='width: 30%; text-align: right' class='cell_caption'>Tellers Reason for Discounting (Business) :</td>";
                html_string += "        <td style='width: 70%; text-align: left' class='cell_desc'><select id='ddl_reason_discounting_bus' style = 'width: 700px'/><option selected='selected'></option></select></td>";
                html_string += "    </tr>";
            }

            html_string += "    <tr style='height: 19px'>";
            html_string += "        <td style = 'width: 30%; text-align: right; display:none' class = 'cell_caption'>Reason Other Details :</td>";
            html_string += "        <td style = 'width: 70%;  text-align: left;'  class = 'cell_desc'><input type = 'text' id = 'txt_reason_disc' style = 'width: 700px; display:none' disabled='disabled'/></td>";
            html_string += "    </tr>";
            html_string += "    <tr style='height: 19px'>";
            html_string += "        <td style='width: 30%; text-align: right' class='cell_caption'>Approvers Disposition :</td>";
            html_string += "        <td style='width: 70%; text-align: left' class='cell_desc'><select id='ddl_approver_disposition' style = 'width: 700px'/><option selected='selected'></option></select></td>";
            html_string += "    </tr>";
            html_string += "    <tr style='height: 19px'>";
            html_string += "        <td style = 'width: 30%; text-align: right;' class = 'cell_caption'>Approvers Remarks :</td>";
            html_string += "        <td style = 'width: 70%;  text-align: left;'  class = 'cell_desc'><input type = 'text' id = 'txt_approver_remarks' style = 'width: 700px'/></td>";
            html_string += "    </tr>";
            html_string += "    <tr style='height: 19px'>";
            html_string += "        <td class = 'cell_desc' colspan='2' style='text-align: center'><label>CLIENT HIT LIST</label></td>";
            html_string += "    </tr>";
            html_string += "    <tr style='height: 5px'>";
            html_string += "    </tr>"
            //=========================================================================================
        }
        /*END*/

        html_string += "    <tr style='height: 19px'>";
        html_string += "        <td class = 'cell_desc' style = 'text-align: center' colspan='2'>";
        //html_string += "            <input id = 'btn_genovr_ok'        type = 'button' value ='OK'     style ='width: 18%; height: 24px' />";
        html_string += "            <input id = 'btn_genovr_ok_cancel' type = 'button' value ='CANCEL' style ='width: 18%; height: 24px'/>";

        if (parseInt(genovr.generic_flag) == 2) {
            html_string += "            <input id = 'btn_genovr_ok_remote_ovr' type = 'button' value ='REMOTE OVERRIDE' style ='width: 18%; height: 24px'/>";
        }

        html_string += "        </td>";
        html_string += "    </tr>";
        html_string += "</table>";

        document.getElementById("content_mod_div").innerHTML = html_string;

        //if (this.user_name != null && this.user_name != '') {
        //    document.getElementById("txt_genovr_user_id").value = this.user_name;
        //}

        //if (this.user_pass != null && this.user_pass != '') {
        //    document.getElementById("txt_genovr_password").value = this.user_pass;
        //}

        if (this.sanction_discounting_reason_other != null && this.sanction_discounting_reason_other != '') {
            if (document.getElementById("txt_reason_disc") != null) {
                document.getElementById("txt_reason_disc").value = this.sanction_discounting_reason_other;
            }
        }

        //document.getElementById("btn_genovr_ok").onclick = function () {
        //    genovr.enter_button_clicked();
        //}

        document.getElementById("btn_genovr_ok_cancel").onclick = function () {
            genovr.cancel_button_clicked();
        }

        //=========================================================================================
        //* RFC-32829 : Request to enhance the OFAC alert and Report in IRSW 01/14/2024 */
        //document.getElementById("ddl_reason_discounting").onchange = function () {
        //    genovr.reason_on_change();
        //}

        //document.getElementById("ddl_reason_discounting_bus").onchange = function () {
        //    genovr.reason_on_change();
        //}

        if (this.sanction_disposition_reason_remarks != null && this.sanction_disposition_reason_remarks != '') {
            if (document.getElementById("txt_approver_remarks") != null) {
                document.getElementById("txt_approver_remarks").value = this.sanction_disposition_reason_remarks;
            }
        }

        document.getElementById("ddl_approver_disposition").onchange = function () {
            genovr.disposition_on_change();
        }

        //document.getElementById("txtrecordno").onkeypress = function (ev) {
        //    var e = window.event || ev;
        //    var keynum = e.charCode || e.keyCode;

        //    if (keynum == 13) {
        //        genovr.display_watchlist_record(document.getElementById("txtrecordno").value);
        //    }
        //}
        //=========================================================================================

        if (document.getElementById("btn_genovr_ok_remote_ovr") != null) {
            document.getElementById("btn_genovr_ok_remote_ovr").onclick = function () {
                //=========================================================================================
                //* RFC-32829 : Request to enhance the OFAC alert and Report in IRSW 01/14/2024 */
                if ((genovr.rem_ciftype == "1" && genovr.ben_ciftype == "1") && (document.getElementById("ddl_reason_discounting").value == "" || document.getElementById("ddl_reason_discounting").value == "null")) {
                    alert("Please provide Teller Reason for Discounting (Individual)");
                    return;
                }
                else if ((genovr.rem_ciftype == "2" && genovr.ben_ciftype == "2") && (document.getElementById("ddl_reason_discounting_bus").value == "" || document.getElementById("ddl_reason_discounting_bus").value == "null")) {
                    alert("Please provide Teller Reason for Discounting (Business)");
                    return;
                }
                else if ((genovr.rem_ciftype == "1" && genovr.ben_ciftype == "0") && (document.getElementById("ddl_reason_discounting").value == "" || document.getElementById("ddl_reason_discounting").value == "null")) {
                    alert("Please provide Teller Reason for Discounting (Individual)");
                    return;
                }
                else if ((genovr.rem_ciftype == "0" && genovr.ben_ciftype == "1") && (document.getElementById("ddl_reason_discounting").value == "" || document.getElementById("ddl_reason_discounting").value == "null")) {
                    alert("Please provide Teller Reason for Discounting (Individual)");
                    return;
                }
                else if ((genovr.rem_ciftype == "2" && genovr.ben_ciftype == "0") && (document.getElementById("ddl_reason_discounting_bus").value == "" || document.getElementById("ddl_reason_discounting_bus").value == "null")) {
                    alert("Please provide Teller Reason for Discounting (Business)");
                    return;
                }
                else if ((genovr.rem_ciftype == "0" && genovr.ben_ciftype == "2") && (document.getElementById("ddl_reason_discounting_bus").value == "" || document.getElementById("ddl_reason_discounting_bus").value == "null")) {
                    alert("Please provide Teller Reason for Discounting (Business)");
                    return;
                }
                else if ((genovr.rem_ciftype == "1" && genovr.ben_ciftype == "2") && (document.getElementById("ddl_reason_discounting").value == "" || document.getElementById("ddl_reason_discounting").value == "null") && (document.getElementById("ddl_reason_discounting_bus").value == "" || document.getElementById("ddl_reason_discounting_bus").value == "null")) {
                    alert("Please provide Teller Reason for Discounting (Individual Type Remitter).\nPlease provide Teller Reason for Discounting (Business Type Beneficiary).");
                    return;
                }
                else if ((genovr.rem_ciftype == "2" && genovr.ben_ciftype == "1") && (document.getElementById("ddl_reason_discounting").value == "" || document.getElementById("ddl_reason_discounting").value == "null") && (document.getElementById("ddl_reason_discounting_bus").value == "" || document.getElementById("ddl_reason_discounting_bus").value == "null")) {
                    alert("Please provide Teller Reason for Discounting (Business Type Remitter).\nPlease provide Teller Reason for Discounting (Individual Type Beneficiary).");
                    return;
                }
                else if ((genovr.rem_ciftype == "1" && genovr.ben_ciftype == "2") && (document.getElementById("ddl_reason_discounting").value != "" || document.getElementById("ddl_reason_discounting").value != "null") && (document.getElementById("ddl_reason_discounting_bus").value == "" || document.getElementById("ddl_reason_discounting_bus").value == "null")) {
                    alert("Please provide Teller Reason for Discounting (Business Type Beneficiary).");
                    return;
                }
                else if ((genovr.rem_ciftype == "1" && genovr.ben_ciftype == "2") && (document.getElementById("ddl_reason_discounting").value == "" || document.getElementById("ddl_reason_discounting").value == "null") && (document.getElementById("ddl_reason_discounting_bus").value != "" || document.getElementById("ddl_reason_discounting_bus").value != "null")) {
                    alert("Please provide Teller Reason for Discounting (Individual Type Remitter).");
                    return;
                }
                else if ((genovr.rem_ciftype == "2" && genovr.ben_ciftype == "1") && (document.getElementById("ddl_reason_discounting").value != "" || document.getElementById("ddl_reason_discounting").value != "null") && (document.getElementById("ddl_reason_discounting_bus").value == "" || document.getElementById("ddl_reason_discounting_bus").value == "null")) {
                    alert("Please provide Teller Reason for Discounting (Individual Type Beneficiary).");
                    return;
                }
                else if ((genovr.rem_ciftype == "2" && genovr.ben_ciftype == "1") && (document.getElementById("ddl_reason_discounting").value == "" || document.getElementById("ddl_reason_discounting").value == "null") && (document.getElementById("ddl_reason_discounting_bus").value != "" || document.getElementById("ddl_reason_discounting_bus").value != "null")) {
                    alert("Please provide Teller Reason for Discounting (Business Type Remitter).");
                    return;
                }
                //=========================================================================================

                /* NEW */
                var rem_ovr = new rem_ovr_scr();
                rem_ovr.jscls = genovr;
                rem_ovr.chk_branch_is_allowed();

                //=========================================================================================
                /* RFC-30454 Creation of new report in IRSW for OTC transactions with OFAC hits */
                rem_ovr.clear_pending_tranx_session();
                //=========================================================================================
                //=========================================================================================
                //* RFC-32829 : Request to enhance the OFAC alert and Report in IRSW 01/14/2024 */
                rem_ovr.set_pending_tranx_session(document.getElementById("ddl_reason_discounting").value.toString(), document.getElementById("ddl_reason_discounting_bus").value.toString());
                //=========================================================================================
            }
        }

        //=========================================================================================
        //* RFC-32829 : Request to enhance the OFAC alert and Report in IRSW 01/14/2024 */
        //this.process_viewer('ajx_loading$');
        //=========================================================================================
    }

    //=========================================================================================
    //* RFC-32829 : Request to enhance the OFAC alert and Report in IRSW 01/14/2024 */
    this.load_discounting = function () {
        IRSWorld.TranClass.RemTranExtra.load_reason_discounting(this.load_discounting_callback);
    }

    this.load_discounting_callback = function (response) {
        if (response != null && response.value != null) {
            var ds = response.value;
            var commons = new common_method();

            commons.to_load_dropdownlist('ddl_reason_discounting', response.value, 'FldValue', 'FldDescription');
            commons = null;

            ds = null;
        }
    }

    this.load_discounting_bus = function () {
        IRSWorld.TranClass.RemTranExtra.load_reason_discounting_bus(this.load_discounting_bus_callback);
    }

    this.load_discounting_bus_callback = function (response) {
        if (response != null && response.value != null) {
            var ds = response.value;
            var commons = new common_method();

            commons.to_load_dropdownlist('ddl_reason_discounting_bus', response.value, 'FldValue', 'FldDescription');
            commons = null;

            ds = null;
        }
    }

    this.reason_on_change = function () {
        if (document.getElementById("ddl_reason_discounting").value == "99") {
            document.getElementById("txt_reason_disc").disabled = false;
            document.getElementById("txt_reason_disc").value = "";
        }
        else {
            document.getElementById("txt_reason_disc").disabled = true;
            document.getElementById("txt_reason_disc").value = "";
        }

        if (document.getElementById("ddl_reason_discounting_bus").value == "99") {
            document.getElementById("txt_reason_disc").disabled = false;
            document.getElementById("txt_reason_disc").value = "";
        }
        else {
            document.getElementById("txt_reason_disc").disabled = true;
            document.getElementById("txt_reason_disc").value = "";
        }
    }

    this.load_disposition = function () {
        IRSWorld.TranClass.RemTranExtra.load_approver_disposition(this.load_disposition_callback);
    }

    this.load_disposition_callback = function (response) {
        if (response != null && response.value != null) {
            var ds = response.value;
            var commons = new common_method();

            commons.to_load_dropdownlist('ddl_approver_disposition', response.value, 'FldValue', 'FldDescription');
            commons = null;

            ds = null;
        }
    }

    this.disposition_on_change = function () {
        //*txt_approver_remarks field should always be enabled*
        //if (document.getElementById("ddl_approver_disposition").value == "99") {
        //    document.getElementById("txt_approver_remarks").disabled = false;
        //    document.getElementById("txt_approver_remarks").value = "";
        //}
        //else {
        //    document.getElementById("txt_approver_remarks").disabled = true;
        //    document.getElementById("txt_approver_remarks").value = "";
        //}
    }

    this.display_watchlist_record = function (recordno) {
        IRSWorld.TranClass.RemTranExtra.get_watchlist_record_details(recordno, this.display_watchlist_record_callback);
    }

    this.display_watchlist_record_callback = function (result) {
        if (result != null && result.value != null) {
            var ds = result.value;

            if (ds.Tables[0].Rows.length > 0) {
                this.FileFormat = ds.Tables[0].Rows[0].FileFormat;
                this.TrackingNo = ds.Tables[0].Rows[0].TrackingNo;
                this.RecordID = ds.Tables[0].Rows[0].RecordID;
                this.Title = ds.Tables[0].Rows[0].Title;
                this.FullName = ds.Tables[0].Rows[0].FullName;
                this.LastName = ds.Tables[0].Rows[0].LastName;
                this.FirstName = ds.Tables[0].Rows[0].FirstName;
                this.MiddleName = ds.Tables[0].Rows[0].MiddleName;
                this.MiddleName = ds.Tables[0].Rows[0].MiddleName;
                this.Suffix = ds.Tables[0].Rows[0].Suffix;
                this.AliasLastName = ds.Tables[0].Rows[0].AliasLastName;
                this.AliasFirstName = ds.Tables[0].Rows[0].AliasFirstName;
                this.AliasMiddleName = ds.Tables[0].Rows[0].AliasMiddleName;
                this.AliasSuffix = ds.Tables[0].Rows[0].AliasSuffix;
                this.Position = ds.Tables[0].Rows[0].Position;
                this.DateOfBirth = ds.Tables[0].Rows[0].DateOfBirth;
                this.TownOfBirth = ds.Tables[0].Rows[0].TownOfBirth;
                this.CountryOfBirth = ds.Tables[0].Rows[0].CountryOfBirth;
                this.Nationality = ds.Tables[0].Rows[0].Nationality;
                this.IDNumber = ds.Tables[0].Rows[0].IDNumber;
                this.IDType = ds.Tables[0].Rows[0].IDType;
                this.Street = ds.Tables[0].Rows[0].Street;
                this.City = ds.Tables[0].Rows[0].City;
                this.State = ds.Tables[0].Rows[0].State;
                this.Zip = ds.Tables[0].Rows[0].Zip;
                this.Country = ds.Tables[0].Rows[0].Country;
                this.PhoneNo = ds.Tables[0].Rows[0].PhoneNo;
                this.HomeNo = ds.Tables[0].Rows[0].HomeNo;
                this.BusinessName = ds.Tables[0].Rows[0].BusinessName;
                this.BusinessPhoneNo = ds.Tables[0].Rows[0].BusinessPhoneNo;
                this.DBAName = ds.Tables[0].Rows[0].DBAName;
                this.OtherInfo = ds.Tables[0].Rows[0].OtherInfo;
                this.GroupType = ds.Tables[0].Rows[0].GroupType;
                this.GroupID = ds.Tables[0].Rows[0].GroupID;
                this.AliasType = ds.Tables[0].Rows[0].AliasType;
                this.Regime = ds.Tables[0].Rows[0].Regime;
                this.ListedOn = ds.Tables[0].Rows[0].ListedOn;
                this.LastUpdate = ds.Tables[0].Rows[0].LastUpdate;
                this.PassportDetails = ds.Tables[0].Rows[0].PassportDetails;
                this.Remarks = ds.Tables[0].Rows[0].Remarks;
                this.DateLastUpdated = ds.Tables[0].Rows[0].DateLastUpdated;
                this.IsDeleted = ds.Tables[0].Rows[0].IsDeleted;

                this.commons = new common_method();
                this.generic = new generic_method();
                document.getElementById("txtFullName").value = this.FullName;
                document.getElementById("txtLastName").value = this.LastName;
                document.getElementById("txtFirstName").value = this.FirstName;
                document.getElementById("txtMiddleInitial").value = this.MiddleName;
                document.getElementById("txtAliasLName").value = this.AliasLastName;
                document.getElementById("txtAliasFName").value = this.AliasFirstName;
                document.getElementById("txtAliasMName").value = this.AliasMiddleName;
                document.getElementById("txtSuffix").value = this.Suffix;
                document.getElementById("txtAliasSuffix").value = this.AliasSuffix;
                document.getElementById("txtBusinessName").value = this.BusinessName;
                document.getElementById("txtTitle").value = this.Title;
                document.getElementById("txtDateofBirth").value = this.commons.to_mmddyyyy_date(this.DateOfBirth);
                document.getElementById("txtAge").value = this.generic.compute_age(document.getElementById("txtDateofBirth").value);
                document.getElementById("txtTownofBirth").value = this.TownOfBirth;
                document.getElementById("txtCountryofBirth").value = this.CountryOfBirth;
                document.getElementById("txtNationality").value = this.Nationality;
                document.getElementById("txtPosition").value = this.Position;
                document.getElementById("txtListedOn").value = this.commons.to_mmddyyyy_date(this.ListedOn);
                document.getElementById("txtDays").value = this.generic.days_passed(document.getElementById("txtListedOn").value);
                document.getElementById("txtLastUpdated").value = this.commons.to_mmddyyyy_date(this.LastUpdate);
                document.getElementById("txtDbaName").value = this.DBAName;
                document.getElementById("txtPhoneNumber").value = this.PhoneNo;
                document.getElementById("txtCity").value = this.City;
                document.getElementById("txtState").value = this.State;
                document.getElementById("txtZipCode").value = this.Zip;
                document.getElementById("txtCountry").value = this.Country;
                document.getElementById("txtRemarks").value = this.Remarks;
                document.getElementById("txtGroupType").value = this.GroupType;
                document.getElementById("txtAliasType").value = this.AliasType;
                document.getElementById("txtRegime").value = this.Regime;
                document.getElementById("txtGroupId").value = this.GroupID;
                document.getElementById("txtPassport").value = this.PassportDetails;
                document.getElementById("txtRegulatory").value = this.FileFormat;
            }
            else {
                alert("NO RECORDS FOUND on searched / selected Watchlist RecordNo.");
                return;
            }
        }
        else {
            alert("NO RECORDS FOUND on searched / selected Watchlist RecordNo.");
            return;
        }
    }
    //=========================================================================================

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

        //===================================================================================================
        /* RFC-35248 : Enhancement in Bills Payment OFAC Screening Process & Report */
	    if (this.isOBPscreen == 0) {
	        //=========================================================================================
	        /* RFC-30454 Creation of new report in IRSW for OTC transactions with OFAC hits */
	        this.load_discounting();
	        //=========================================================================================

	        //=========================================================================================
	        //* RFC-32829 : Request to enhance the OFAC alert and Report in IRSW 01/14/2024 */
	        this.load_discounting_bus();
	        this.load_disposition();
	        //=========================================================================================
	    }
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
    	
	    content_div                       = document.createElement("div");
	    content_div.id                    = "content_mod_div";
	    content_div.align                 = "left";
	    content_div.style.background      = "transparent";
	    content_div.innerHTML             = "<center><img src = '.././images/ajaxloader.gif'></img></center>";

	    this.inner_modal_div              = document.createElement("div");		
	    this.inner_modal_div.align        = "center";
	    this.inner_modal_div.className    = "inner_div";		
	    this.inner_modal_div.id           = "inner_mod_div";
	    this.inner_modal_div.style.height = "auto";		
	    this.inner_modal_div.style.width  = "1000px";
	    this.inner_modal_div.style.top    = "30px";
	    this.inner_modal_div.style.left   = (parseInt(screen.width) - parseInt(this.inner_modal_div.style.width)) / 2 - 10 + "px";
	    this.inner_modal_div.style.border = "medium solid white";
	    this.inner_modal_div.appendChild(content_div);
	
	    document.body.appendChild(this.inner_modal_div);
    }

    this.load_modal_css = function()
    {
	    var file_reference = document.createElement("link");
    	
	    file_reference.setAttribute("rel", "stylesheet");
	    file_reference.setAttribute("type", "text/css");
	    file_reference.setAttribute("href", ".././css/cssmodals.css");
    	
	    if (typeof file_reference != "undefined")
	    {		    
	        document.getElementsByTagName("head")[0].appendChild(file_reference);
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
	        if (divelement_outer.item(i).id == "outer_mod_div")
	        {
		    divelement_outer.item(i).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop;break;
	        }
	    }
	    for(m = 0; m < divelement_inner.length;m++)
	    {
	        if (divelement_inner.item(m).id == "inner_mod_div")
	        {
		    divelement_outer.item(m).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop + 20 : document.body.scrollTop + 20;break;
	        }
	    }
    }
    
    this.open_cif_client_hit_list = function(FName, LName, MName, UserID, BranchCode)
    {
        //===================================================================================================
        /* RFC-35248 : Enhancement in Bills Payment OFAC Screening Process & Report */
        this.isOBPscreen = 0;
        //===================================================================================================

        var ciftype = Internet_Remitter_Main.xciftype.toString();
        
        if (ciftype == 1)
        {
        this.cif_fname = FName;
        this.cif_lname = LName;
        this.cif_mname = MName;
        }
        
        if (ciftype == 2)
        {
        this.cif_fname = "";
        this.cif_lname = LName;
        this.cif_mname = "";
        }
        
        this.ciftype = ciftype;
        this.cif_branchcode = BranchCode;
        this.cif_userid = UserID;
        //this.isStreamlined = this.check_if_streamlined();
    
        this.generic_flag = 3;
        this.open_modal_window(); 
        this.construct_cif_hit_list_all();
        this.process_viewer('ajx_loading$');
    }
    
    //=========================================================================================
    /* RFC-30454 Creation of new report in IRSW for OTC transactions with OFAC hits */


    /* RFC-36314 - IRSW (EU) Interface with Refinitiv WorldCheck via API */
    this.open_cif_client_hit_list_refinitiv = function (FName, LName, MName, UserID, BranchCode, isRefinitivIndividual, Gender, DOB, COB, POB, Nationality) {
        //===================================================================================================
        /* RFC-35248 : Enhancement in Bills Payment OFAC Screening Process & Report */
        this.isOBPscreen = 0;
        //===================================================================================================
        var ciftype = Internet_Remitter_Main.xciftype.toString();

        if (ciftype == 1) { this.cif_fname = FName; this.cif_lname = LName; this.cif_mname = MName; this.cif_isRefinitivIndividual = isRefinitivIndividual; this.cif_gender = Gender; this.cif_DOB = DOB; this.cif_COB = COB; this.cif_POB = POB; this.cif_Nationality = Nationality; } //Remitter
        if (ciftype == 2) { this.cif_fname = ""; this.cif_lname = LName; this.cif_mname = ""; this.cif_isRefinitivIndividual = isRefinitivIndividual; this.cif_gender = Gender; this.cif_DOB = DOB; this.cif_COB = COB; this.cif_POB = POB; this.cif_Nationality = Nationality; } //Beneficiary

        this.ciftype = ciftype;
        this.cif_branchcode = BranchCode;
        this.cif_userid = UserID;
        //this.isStreamlined = this.check_if_streamlined();

        this.generic_flag = 3;
        this.open_modal_window();
        this.construct_cif_hit_list_all();
        this.process_viewer('ajx_loading$');
    }
    /* RFC-36314 - IRSW (EU) Interface with Refinitiv WorldCheck via API */
    this.open_cif_client_hit_list_refinitivisdown = function (FName, LName, MName, UserID, BranchCode, isRefinitivIndividual, Gender, DOB, COB, POB, Nationality) {
        //===================================================================================================
        /* RFC-35248 : Enhancement in Bills Payment OFAC Screening Process & Report */
        this.isOBPscreen = 0;
        //===================================================================================================
        var ciftype = Internet_Remitter_Main.xciftype.toString();

        if (ciftype == 1) { this.cif_fname = FName; this.cif_lname = LName; this.cif_mname = MName; this.cif_isRefinitivIndividual = isRefinitivIndividual; this.cif_gender = Gender; this.cif_DOB = DOB; this.cif_COB = COB; this.cif_POB = POB; this.cif_Nationality = Nationality; } //Remitter
        if (ciftype == 2) { this.cif_fname = ""; this.cif_lname = LName; this.cif_mname = ""; this.cif_isRefinitivIndividual = isRefinitivIndividual; this.cif_gender = Gender; this.cif_DOB = DOB; this.cif_COB = COB; this.cif_POB = POB; this.cif_Nationality = Nationality; } //Beneficiary

        this.ciftype = ciftype;
        this.cif_branchcode = BranchCode;
        this.cif_userid = UserID;
        //this.isStreamlined = this.check_if_streamlined();

        this.generic_flag = 3;
        this.open_modal_window();
        //this.construct_cif_hit_list_all();
        //this.process_viewer('ajx_loading$');
        this.construct_cif_hit_list_all_refinitivisdown();
    }


    //===================================================================================================
    /* RFC-35248 : Enhancement in Bills Payment OFAC Screening Process & Report */
    this.open_cif_client_hit_list_obp = function (FName, LName, MName, UserID, BranchCode, ciftype) {
        this.isOBPscreen = 1;

        if (ciftype == 1) {
            this.cif_fname = FName;
            this.cif_lname = LName;
            this.cif_mname = MName;
        }

        if (ciftype == 2) {
            this.cif_fname = "";
            this.cif_lname = LName;
            this.cif_mname = "";
        }

        this.cif_branchcode = BranchCode;
        this.cif_userid = UserID;

        this.generic_flag = 3;
        this.open_modal_window();
        this.construct_cif_hit_list_all();
        this.process_viewer('ajx_loading$');
    }
    //===================================================================================================

    /* RFC-37590 - IRSW NY Interface with Refinitiv WorldCheck via API */
    this.open_cif_client_hit_list_obp_refinitiv = function (FName, LName, MName, UserID, BranchCode, isRefinitivIndividual, Gender, DOB, COB, POB, Nationality, CIFClass, ciftype) {
        this.isOBPscreen = 1;

        if (ciftype == 1) { this.cif_fname = FName; this.cif_lname = LName; this.cif_mname = MName; this.cif_isRefinitivIndividual = isRefinitivIndividual; this.cif_gender = Gender; this.cif_DOB = DOB; this.cif_COB = COB; this.cif_POB = POB; this.cif_Nationality = Nationality; } //Remitter
        if (ciftype == 2) { this.cif_fname = ""; this.cif_lname = LName; this.cif_mname = ""; this.cif_isRefinitivIndividual = isRefinitivIndividual; this.cif_gender = Gender; this.cif_DOB = DOB; this.cif_COB = COB; this.cif_POB = POB; this.cif_Nationality = Nationality; } //Beneficiary

        this.ciftype = ciftype;
        this.cif_branchcode = BranchCode;
        this.cif_userid = UserID;

        this.generic_flag = 3;
        this.open_modal_window();
        this.construct_cif_hit_list_all();
        this.process_viewer('ajx_loading$');
    }

    /* RFC-37590 - IRSW NY Interface with Refinitiv WorldCheck via API */
    this.open_cif_client_hit_list_obp_refinitivisdown = function (FName, LName, MName, UserID, BranchCode, isRefinitivIndividual, Gender, DOB, COB, POB, Nationality, CIFClass, ciftype) {
        this.isOBPscreen = 1;

        if (ciftype == 1) { this.cif_fname = FName; this.cif_lname = LName; this.cif_mname = MName; this.cif_isRefinitivIndividual = isRefinitivIndividual; this.cif_gender = Gender; this.cif_DOB = DOB; this.cif_COB = COB; this.cif_POB = POB; this.cif_Nationality = Nationality; } //Remitter
        if (ciftype == 2) { this.cif_fname = ""; this.cif_lname = LName; this.cif_mname = ""; this.cif_isRefinitivIndividual = isRefinitivIndividual; this.cif_gender = Gender; this.cif_DOB = DOB; this.cif_COB = COB; this.cif_POB = POB; this.cif_Nationality = Nationality; } //Beneficiary

        this.ciftype = ciftype;
        this.cif_branchcode = BranchCode;
        this.cif_userid = UserID;

        this.generic_flag = 3;
        this.open_modal_window();
        this.construct_cif_hit_list_all_refinitivisdown();
        //this.process_viewer('ajx_loading$');
    }


    this.construct_cif_hit_list_all = function()
    {
        var isRefinitivEnabled = IRSWorld.TranClass.Refinitiv.isRefinitivEnabled();
        var html_string = "";
        var ihtml = "";       
        /*SANCTION LIST DISCOUNTING - RFS 16165-003 (CIF CREATION)*/
        var sanction_disc = null;
        try
        {
            sanction_disc = IRSWorld.TranClass.RemTranExtra.discounting_required().value;
        }
        catch(error)
        {
            sanction_disc = null;
        }
        /*END*/
    
        ihtml = '<center>'
        ihtml += '<table cellpadding = "0"; cellspacing = "1" style = "width:700px;border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid">';
        ihtml += '    <tr>';
        ihtml += '        <td class = "table_caption" colspan="2" style="text-align: left"> WATCHED LIST</td>';
        ihtml += '    </tr>';  
        ihtml += '    <tr>';
        ihtml += '        <td class = "cell_desc" colspan="2" style="text-align: center"><label>&nbsp;</label></td>';
        if (isRefinitivEnabled.value == "1") {
            ihtml += '        <img align="absBottom" src="../images/rfv_lg_pb_hz_rgb_pos_World-Check.png" width="200px" height="50px"/>';
        }
        ihtml += '    </tr>';        
        ihtml += '    <tr>';
        if (this.ciftype == 2) {
            ihtml += '        <td class = "cell_desc" colspan="2" style="text-align: center"><label>Client ' + this.cif_lname + ' was found in the watched list</label></td>';
        }
        else {
            ihtml += '        <td class = "cell_desc" colspan="2" style="text-align: center"><label>Client ' + this.cif_fname + ' ' + this.cif_mname + ' ' + this.cif_lname + ' was found in the watched list</label></td>';
        }
        ihtml += '    </tr>';
        ihtml += '    <tr>';
        ihtml += '        <td class = "cell_desc" colspan="2" style="text-align: left"><label>&nbsp;</label></td>';
        ihtml += '    </tr>';                
        ihtml += '    <tr>';
        ihtml += '        <td class = "cell_desc" colspan="2" style="text-align: center">';
        ihtml += '             <table border="0" style="border-right: #336699 3px solid; border-top: #336699 3px solid; border-left: #336699 3px solid;border-bottom: #336699 3px solid">';
        ihtml += '                  <tr>';
        ihtml += '                      <td style="width:250px;" class="cell_caption">';
        ihtml += '                          <span id="Span1" style="font-size: 12px">User Name</span></td>';
        ihtml += '                      <td style="width:450px;" align="left">';
        ihtml += '                          <input id="txtCIFHitlistOVRU" style="width:97%;text-align:left;" type="text"/>';
        ihtml += '                      </td>';
        ihtml += '                  </tr>';
        ihtml += '                  <tr>';
        ihtml += '                      <td style="width:250px;" class="cell_caption">';
        ihtml += '                          <span id="Span2" style="font-size: 12px">Password</span>';
        ihtml += '                      </td>';
        ihtml += '                      <td style="width:450px;" align="left">';
        ihtml += '                          <input id="txtCIFHitlistOVRP" style="width:97%;text-align:left;" type="password"/>';
        ihtml += '                      </td>';
        ihtml += '                  </tr>';
        ihtml += '                  <tr>';
        ihtml += '                      <td style="width:250px;">';
        ihtml += '                      </td>';
        ihtml += '                      <td style="width:450px;" align="right">';
        //===================================================================================================
        /* RFC-35248 : Enhancement in Bills Payment OFAC Screening Process & Report */
        ////No Remote Override Functionality for Adding of Merchant Details for specific Remitter 
        ////ihtml += '                          <input id = "btn_cif_client_hit_list_ok_all" type = "button" value = "OVERRIDE" style="width: 100px; height: 25px" />&nbsp;<input id = "btn_cif_client_remote_override_ok_all" type = "button" value = "REMOTE OVERRIDE" style="width: 200px; height: 25px" />&nbsp;<input id = "btn_cif_client_hit_list_cancel_all" type = "button" value = "CANCEL" style="width: 100px; height: 25px" />&nbsp;&nbsp;&nbsp;';
        ihtml += '                          <input id = "btn_cif_client_hit_list_ok_all" type = "button" value = "OVERRIDE" style="width: 100px; height: 25px" />&nbsp';
        if (this.isOBPscreen == 0) {
            ihtml += '                          <input id = "btn_cif_client_remote_override_ok_all" type = "button" value = "REMOTE OVERRIDE" style="width: 200px; height: 25px" />&nbsp';
        }
        ihtml += '                          <input id = "btn_cif_client_hit_list_cancel_all" type = "button" value = "CANCEL" style="width: 100px; height: 25px" />&nbsp;&nbsp;&nbsp;';
        //===================================================================================================
        ihtml += '                      </td>';
        ihtml += '                  </tr>';                
        ihtml += '              </table>';
        ihtml += '        </td>';
        ihtml += '    </tr>';
//        ihtml += '   <tr>';
//        ihtml += '       <td colspan="2" style="text-align: right"><input id = "btn_cif_client_hit_list_ok_all" type = "button" value = "OVERRIDE" style="width: 100px; height: 25px" />&nbsp;<input id = "btn_cif_client_hit_list_cancel_all" type = "button" value = "CANCEL" style="width: 100px; height: 25px" /></td>';        
        //        ihtml += '   </tr>';   
        if (isRefinitivEnabled.value == "1") {
            ihtml += '   <tr><table id="tblHitList" class="table table-striped table-bordered" style="width:70%">';
            ihtml += '       <thead>';
            ihtml += '           <tr>';
            ihtml += '               <th class="text-center">Match Score</th>';
            ihtml += '               <th class="text-center">Matched Name Type</th>';
            ihtml += '               <th class="text-center">Primary Name</th>';
            ihtml += '               <th class="text-center">Category</th>';
            ihtml += '               <th class="text-center">Date Of Birth</th>';
            ihtml += '               <th class="text-center">Nationality</th>';
            ihtml += '               <th class="text-center">Special Interest Category</th>';
            ihtml += '               <th class="text-center">Provider Type</th>';
            ihtml += '               <th class="text-center">Alias</th>';
            ihtml += '               <th class="text-center">Remarks</th>';
            ihtml += '               <th class="text-center">Resolution</th>';
            ihtml += '               <th class="text-center">Resolution Remarks</th>';
            ihtml += '               <th class="text-center">Created Date</th>';
            ihtml += '               <th class="text-center">Updated Date</th>';
            ihtml += '           </tr>';
            ihtml += '       </thead>';
            ihtml += '   </table></tr>';
        }
        else {
            ihtml += '    <tr>';
            ihtml += '        <td colspan="2" style="text-align: center" ><div id = "div_cif_client_hit_list_all"></div></td>';
            ihtml += '    </tr>';
        }
        ihtml += '</table>';
        ihtml += '</center>'
        
        document.getElementById("content_mod_div").innerHTML = ihtml;
        
        document.getElementById("btn_cif_client_hit_list_ok_all").onclick = function()
        {
            if (genovr.user_can_override()) {
                //===================================================================================================
                /* RFC-35248 : Enhancement in Bills Payment OFAC Screening Process & Report */
                if (genovr.isOBPscreen == 0) {

                    /* Refinitiv WorldCheck via API */
                    if (isRefinitivEnabled.value == "1") {
                        var msg_over = "Do you want to proceed resolving refinitiv hits?";
                    } else { var msg_over = "Do you want to proceed?"; }
                    var ok_to_proceed = confirm(msg_over);

                    if (ok_to_proceed == true) {
                        Internet_Remitter.isRefinitivResolved = ok_to_proceed;
                        Internet_Remitter.LocalOverride_txtCIFHitlistOVRU = document.getElementById("txtCIFHitlistOVRU").value;
                        Internet_Beneficiary.isRefinitivResolved = ok_to_proceed;
                        Internet_Beneficiary.LocalOverride_txtCIFHitlistOVRU = document.getElementById("txtCIFHitlistOVRU").value;
                        genovr.close_modal_window();
                        Internet_Remitter_Main.proceed_hit_list();
                    }
                    else { return; }
                    /* Refinitiv WorldCheck via API */
                }
                else {
                    /* Refinitiv WorldCheck via API */
                    if (isRefinitivEnabled.value == "1") {
                        var msg_over = "Do you want to proceed resolving refinitiv hits?";
                    } else { var msg_over = "Do you want to proceed?"; }
                    var ok_to_proceed = confirm(msg_over);

                    if (ok_to_proceed == true) {
                        cif_main.isRefinitivResolved = ok_to_proceed;
                        cif_main.LocalOverride_txtCIFHitlistOVRU = document.getElementById('txtCIFHitlistOVRU').value;
                        genovr.close_modal_window();
                        cif_main.populate_merchant_details_watchlist();
                    }
                    else { return; }
                    /* Refinitiv WorldCheck via API */
                }
                //===================================================================================================
            }
        }
        
        //===================================================================================================
        /* RFC-35248 : Enhancement in Bills Payment OFAC Screening Process & Report */
        if (genovr.isOBPscreen == 0) {
            document.getElementById("btn_cif_client_remote_override_ok_all").onclick = function () {
                //===================================================================================================
                /* RFC-35248 : Enhancement in Bills Payment OFAC Screening Process & Report */
                if (genovr.isOBPscreen == 0) {
                    genovr.chk_branch_is_allowed_to_remOvr();
                }
                else {
                    alert("No remote override process in merchant/biller enrollment. Please input user credentials for local override."); return;
                }
                //Internet_Remitter_Main.proceed_hit_list();
                //===================================================================================================
            }
        }
       
        document.getElementById("btn_cif_client_hit_list_cancel_all").onclick = function()
        {
            genovr.close_modal_window();

            //===================================================================================================
            /* RFC-35248 : Enhancement in Bills Payment OFAC Screening Process & Report */
            //Internet_Remitter_Main.cancel_hit_list();
            if (genovr.isOBPscreen == 0) {
                Internet_Remitter_Main.cancel_hit_list();
            }
            else {
                cif_main.panel_search_merchants();
            }
            //genovr.cancel_hit_list_all();
            //===================================================================================================
        }
    }
    //=========================================================================================
    

    this.construct_cif_hit_list_all_refinitivisdown = function () {
        var isRefinitivEnabled = IRSWorld.TranClass.Refinitiv.isRefinitivEnabled();
        var html_string = "";
        var ihtml = "";
        /*SANCTION LIST DISCOUNTING - RFS 16165-003 (CIF CREATION)*/
        var sanction_disc = null;
        try {
            sanction_disc = IRSWorld.TranClass.RemTranExtra.discounting_required().value;
        }
        catch (error) {
            sanction_disc = null;
        }
        /*END*/

        ihtml = '<center>'
        ihtml += '<table cellpadding = "0"; cellspacing = "1" style = "width:700px;border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid">';
        ihtml += '    <tr>';
        ihtml += '        <td class = "table_caption" colspan="2" style="text-align: left"> REFINITIV IS DOWN. </td>';
        ihtml += '    </tr>';
        ihtml += '    <tr>';
        ihtml += '        <td class = "cell_desc" colspan="2" style="text-align: center"><label>&nbsp;</label></td>';
        if (isRefinitivEnabled.value == "1") {
            ihtml += '        <img align="absBottom" src="../images/rfv_lg_pb_hz_rgb_pos_World-Check.png" width="200px" height="50px"/>';
        }
        ihtml += '    </tr>';
        ihtml += '    <tr>';
        if (this.ciftype == 2) {
            ihtml += '        <td class = "cell_desc" colspan="2" style="text-align: center"><label>REFINITIV IS DOWN. Client ' + this.cif_lname + ' might be in the watched list. </label></td>';
        }
        else {
            ihtml += '        <td class = "cell_desc" colspan="2" style="text-align: center"><label>REFINITIV IS DOWN. Client ' + this.cif_fname + ' ' + this.cif_mname + ' ' + this.cif_lname + ' might be in the watched list. </label></td>';
        }
        ihtml += '    </tr>';
        ihtml += '    <tr>';
        ihtml += '        <td class = "cell_desc" colspan="2" style="text-align: left"><label>&nbsp;</label></td>';
        ihtml += '    </tr>';
        ihtml += '    <tr>';
        ihtml += '        <td class = "cell_desc" colspan="2" style="text-align: center">';
        ihtml += '             <table border="0" style="border-right: #336699 3px solid; border-top: #336699 3px solid; border-left: #336699 3px solid;border-bottom: #336699 3px solid">';
        ihtml += '                  <tr>';
        ihtml += '                      <td style="width:250px;" class="cell_caption">';
        ihtml += '                          <span id="Span1" style="font-size: 12px">User Name</span></td>';
        ihtml += '                      <td style="width:450px;" align="left">';
        ihtml += '                          <input id="txtCIFHitlistOVRU" style="width:97%;text-align:left;" type="text"/>';
        ihtml += '                      </td>';
        ihtml += '                  </tr>';
        ihtml += '                  <tr>';
        ihtml += '                      <td style="width:250px;" class="cell_caption">';
        ihtml += '                          <span id="Span2" style="font-size: 12px">Password</span>';
        ihtml += '                      </td>';
        ihtml += '                      <td style="width:450px;" align="left">';
        ihtml += '                          <input id="txtCIFHitlistOVRP" style="width:97%;text-align:left;" type="password"/>';
        ihtml += '                      </td>';
        ihtml += '                  </tr>';
        ihtml += '                  <tr>';
        ihtml += '                      <td style="width:250px;">';
        ihtml += '                      </td>';
        ihtml += '                      <td style="width:450px;" align="right">';
        //===================================================================================================
        /* RFC-35248 : Enhancement in Bills Payment OFAC Screening Process & Report */
        ////No Remote Override Functionality for Adding of Merchant Details for specific Remitter 
        ////ihtml += '                          <input id = "btn_cif_client_hit_list_ok_all" type = "button" value = "OVERRIDE" style="width: 100px; height: 25px" />&nbsp;<input id = "btn_cif_client_remote_override_ok_all" type = "button" value = "REMOTE OVERRIDE" style="width: 200px; height: 25px" />&nbsp;<input id = "btn_cif_client_hit_list_cancel_all" type = "button" value = "CANCEL" style="width: 100px; height: 25px" />&nbsp;&nbsp;&nbsp;';
        ihtml += '                          <input id = "btn_cif_client_hit_list_ok_all" type = "button" value = "OVERRIDE" style="width: 100px; height: 25px" />&nbsp';
        if (this.isOBPscreen == 0) {
            ihtml += '                          <input id = "btn_cif_client_remote_override_ok_all" type = "button" value = "REMOTE OVERRIDE" style="width: 200px; height: 25px" />&nbsp';
        }
        ihtml += '                          <input id = "btn_cif_client_hit_list_cancel_all" type = "button" value = "CANCEL" style="width: 100px; height: 25px" />&nbsp;&nbsp;&nbsp;';
        //===================================================================================================
        ihtml += '                      </td>';
        ihtml += '                  </tr>';
        ihtml += '              </table>';
        ihtml += '        </td>';
        ihtml += '    </tr>';
        //        ihtml += '   <tr>';
        //        ihtml += '       <td colspan="2" style="text-align: right"><input id = "btn_cif_client_hit_list_ok_all" type = "button" value = "OVERRIDE" style="width: 100px; height: 25px" />&nbsp;<input id = "btn_cif_client_hit_list_cancel_all" type = "button" value = "CANCEL" style="width: 100px; height: 25px" /></td>';        
        //        ihtml += '   </tr>';   
        ihtml += '</table>';
        ihtml += '</center>'

        document.getElementById("content_mod_div").innerHTML = ihtml;

        document.getElementById("btn_cif_client_hit_list_ok_all").onclick = function () {
            if (genovr.user_can_override()) {
                //===================================================================================================
                /* RFC-35248 : Enhancement in Bills Payment OFAC Screening Process & Report */
                if (genovr.isOBPscreen == 0) {

                    /* Refinitiv WorldCheck via API */
                    if (isRefinitivEnabled.value == "1") {
                        var msg_over = "REFINITIV IS DOWN. Do you want to proceed resolving refinitiv hits?";
                    } else { var msg_over = "Do you want to proceed?"; }
                    var ok_to_proceed = confirm(msg_over);

                    if (ok_to_proceed == true) {
                        Internet_Remitter.isRefinitivResolved = ok_to_proceed;
                        Internet_Remitter.LocalOverride_txtCIFHitlistOVRU = document.getElementById("txtCIFHitlistOVRU").value;
                        Internet_Beneficiary.isRefinitivResolved = ok_to_proceed;
                        Internet_Beneficiary.LocalOverride_txtCIFHitlistOVRU = document.getElementById("txtCIFHitlistOVRU").value;
                        genovr.close_modal_window();
                        Internet_Remitter_Main.proceed_hit_list();
                    }
                    else { return; }
                    /* Refinitiv WorldCheck via API */
                }
                else {
                    /* Refinitiv WorldCheck via API */
                    if (isRefinitivEnabled.value == "1") {
                        var msg_over = "REFINITIV IS DOWN. Do you want to proceed resolving refinitiv hits?";
                    } else { var msg_over = "Do you want to proceed?"; }
                    var ok_to_proceed = confirm(msg_over);

                    if (ok_to_proceed == true) {
                        cif_main.isRefinitivResolved = ok_to_proceed;
                        cif_main.LocalOverride_txtCIFHitlistOVRU = document.getElementById('txtCIFHitlistOVRU').value;
                        genovr.close_modal_window();
                        cif_main.populate_merchant_details_watchlist();
                    }
                    else { return; }
                    /* Refinitiv WorldCheck via API */
                }
                //===================================================================================================
            }
        }

        //===================================================================================================
        /* RFC-35248 : Enhancement in Bills Payment OFAC Screening Process & Report */
        if (genovr.isOBPscreen == 0) {
            document.getElementById("btn_cif_client_remote_override_ok_all").onclick = function () {
                //===================================================================================================
                /* RFC-35248 : Enhancement in Bills Payment OFAC Screening Process & Report */
                if (genovr.isOBPscreen == 0) {
                    genovr.chk_branch_is_allowed_to_remOvr();
                }
                else {
                    alert("No remote override process in merchant/biller enrollment. Please input user credentials for local override."); return;
                }
                //Internet_Remitter_Main.proceed_hit_list();
                //===================================================================================================
            }
        }

        document.getElementById("btn_cif_client_hit_list_cancel_all").onclick = function () {
            genovr.close_modal_window();

            //===================================================================================================
            /* RFC-35248 : Enhancement in Bills Payment OFAC Screening Process & Report */
            //Internet_Remitter_Main.cancel_hit_list();
            if (genovr.isOBPscreen == 0) {
                Internet_Remitter_Main.cancel_hit_list();
            }
            else {
                cif_main.panel_search_merchants();
            }
            //genovr.cancel_hit_list_all();
            //===================================================================================================
        }
    }

    this.chk_branch_is_allowed_to_remOvr = function()
    {
        genovr.open_msg_prompt("Please wait ...");

        AjaxPro.timeoutPeriod = 1000 * 1000;
        AjaxPro.onTimeout = function()
        {
            alert("Server not responding. Sending for override values failed.");
        }

        IRSWorld.TranClass.RemTranExtra.chk_branch_remote_allowed('',this.chk_branch_is_allowed_to_remOvr_callback);
    }

    this.chk_branch_is_allowed_to_remOvr_callback = function(result)
    {
        genovr.close_msg_prompt();

        if (result != null && result.value != null)
        {
            var branch_ok = result.value.split('~');

            if (branch_ok != null && branch_ok.length == 2 && branch_ok[0] == 1)
            {
                var msg_over = "Enrollment will be queued for overriding";
                var ok_to_proceed = confirm(msg_over);

                if (ok_to_proceed == true)
                {
                        Internet_Remitter.isCIFRemoteOverride = ok_to_proceed;
                        Internet_Beneficiary.isCIFRemoteOverride = ok_to_proceed;
                        Internet_Remitter_Main.proceed_cif_remote_override();
                }
                else
                {
                    genovr.close_modal_window();
                    Internet_Remitter_Main.cancel_hit_list();
                }
            }
            else
            {
                alert("Remote Override functionality is not activated."); return;
            }
        }
    }
	
	this.user_can_override = function()
    {
        var ovru = null;
        var ovrp = null;
	    /* RFC-32606 - Transaction Search and Local Transaction Override */
        var validateApproverLocation = IRSWorld.TranClass.RemTranExtra.validateApproverLocation(document.getElementById('txtCIFHitlistOVRU').value).value;
        if (validateApproverLocation == true) {
            /* RFC-32606 - Transaction Search and Local Transaction Override */
            try {
                ovru = document.getElementById('txtCIFHitlistOVRU').value;
                ovrp = document.getElementById('txtCIFHitlistOVRP').value;

                var result = IRSWorld.TranClass.RemTranExtra.user_can_override_hit_list(ovru, ovrp);

                if (result == null || result.value == null) {
                    return false;
                }

                if (result.value[0] != null && result.value[0].toString().toUpperCase() == "FALSE") {
                    if (result.value[1] != null) {
                        alert(result.value[1].toString()); return false;
                    }
                }
                else if (result.value[0] != null && result.value[0].toString().toUpperCase() == "TRUE") {
                    return true;
                }

                alert("Unable to validate credential"); return false;
            }
            catch (error) {
                alert(error); return false;
            }
        }
        /* RFC-32606 - Transaction Search and Local Transaction Override */
        else { alert("User Id is not allowed to approve this CIF."); return false; }
	    /* RFC-32606 - Transaction Search and Local Transaction Override */
    }    
    
    
//    this.check_if_streamlined = function()
//    {
//        return IRSWorld.TranClass.RemTranExtra.branch_in_stream_lining('').value;
//    }                        

//  ================================================================================
//  For CIF Web / Phone Remitter Tag    
//  ================================================================================

    this.open_Web_Phone_Remitter_Tag_Override = function(result)
    {
        this.generic_flag = 4;
        this.open_modal_window(); 
        this.construct_generic_screen();
    }    
    
//  End For CIF Web / Phone Remitter Tag  


}
/* GENERIC OVERRIDE */


/* 22.) FOR CLEARING DATE */
function ach_payment()
{
    var ach_pay = this;

    this.transaction_id = null;
    this.clearing_date  = null;

    this.load_clearing_css = function()
    {	    
        var file_reference = document.createElement("link");

        file_reference.setAttribute("rel", "stylesheet");
        file_reference.setAttribute("type", "text/css");
        file_reference.setAttribute("href", ".././css/cssmodals.css");

        if (typeof file_reference != "undefined")
        {		    
            document.getElementsByTagName("head")[0].appendChild(file_reference);
        }
    }
    
    this.construct_clearing_div_content = function()
    {
        var content_div = null;

        this.inner_modal_div              = document.createElement("div");
        this.inner_modal_div.className    = "inner_div";		
        this.inner_modal_div.id           = "inner_mod_div_cd";
        this.inner_modal_div.style.height = "auto";		
        this.inner_modal_div.style.width  = "250px";
        this.inner_modal_div.style.top    = "30px";
        this.inner_modal_div.style.left   = (parseInt(screen.width) - parseInt(this.inner_modal_div.style.width)) / 2 - 10 + "px";
        this.inner_modal_div.style.border = "medium solid white";

        content_div                       = document.createElement("div");
        content_div.id                    = "content_mod_div_cd";
        content_div.style.background      = "transparent";
        content_div.innerHTML             = "<center><img src = '.././images/ajaxloader.gif'></img></center>";

        this.inner_modal_div.appendChild(content_div);

        document.body.appendChild(this.inner_modal_div);
    }
    
    this.create_clearing_popup = function()
    {
        this.open_clearing_popup(); document.getElementById("content_mod_div_cd").innerHTML = this.clearingdate_html();
        document.getElementById("btn_ach_ok").onclick = function()
        {
            if (document.getElementById("txtClearingDate").value == null || document.getElementById("txtClearingDate").value == '' || document.getElementById("txtClearingDate").value.toUpperCase() == 'NULL')
            {
                alert('Clearing date is empty.'); return;
            }
            else
            {
                ach_pay.chek_clearing();
            }
        }

        document.getElementById("btn_ach_cancel").onclick = function()
        {
            ach_pay.close_clearing_popup();
        }
    }
    
    this.chek_clearing = function()
    {
        AjaxPro.timeoutPeriod = 1000 * 1000;
        
        AjaxPro.onTimeout = function()
        {
            alert("Server not responding. Checking clearing date failed.");
        }    
    
        this.clearing_date = document.getElementById("txtClearingDate").value; IRSWorld.TranClass.rem_modal_check_payments.check_clearing_date(this.clearing_date, this.chek_clearing_callback);
    }
    
    this.chek_clearing_callback = function(result)
    {
        if (result != null && result.value != null)
        {
            if (result.value == false)
            {
                alert('Invalid clearing date.'); return;
            }
        }
        
        ach_pay.close_clearing_popup();  eval("javascript:push_ach_payment_result();");
    }

    this.open_clearing_popup = function()
    {
        this.load_clearing_css();

        this.outer_modal_div           = document.createElement("div");
        this.outer_modal_div.id        = "outer_mod_div_cd";
        this.outer_modal_div.className = "outer_div";

        document.body.style.margin = "0px";
        document.body.appendChild(this.outer_modal_div);
        document.getElementById("outer_mod_div_cd").style.height = screen.height;

        this.construct_clearing_div_content();
        this.commons = new common_method();
        this.commons.to_show_hide_combobox(false);
        this.commons = null;
    }

    this.close_clearing_popup = function()
    {
        this.commons = new common_method();
        this.commons.to_show_hide_combobox(true);
        this.commons = null;

        if(this.outer_modal_div != null){document.body.removeChild(this.outer_modal_div);this.outer_modal_div  = null;}
        if(this.inner_modal_div != null){document.body.removeChild(this.inner_modal_div);this.inner_modal_div  = null;}

        document.body.style.margin = "";
    }
    
    this.clearingdate_html = function()
    {                   
        var  shtml = "";
        
        shtml += '<table border="0" cellpadding="0" cellspacing="0" style="border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid;width:250px;height:auto" cellpadding="0" cellspacing="1">';
        shtml += '<tr class="table_caption">';
        shtml += '    <td  colspan="3" style="height: 31px; text-align: left"> &nbsp;Enter Clearing Date</td>';
        shtml += '</tr>';
        shtml += '<tr>';
        shtml += '    <td class="cell_caption" style="height: 31px; text-align: center; ">Clearing Date:</td>';
        shtml += '    <td class="cell_caption"  style="height: 31px; text-align: left;width:100px">';
        shtml += '    <input disabled   type="text" id="txtClearingDate" size="20" ></td> <td class="cell_caption" nowrap align="left"><img align="left"';            
        shtml += ' align="absMiddle" onclick="' + "javascript:click_calendar('txtClearingDate',this)" + '"' + " src='../images/calendar/imgcalen.png' /></td>";
        shtml += '</tr>';
        shtml += '<tr>';
        shtml += '    <td colspan="3" style="text-align:center; height: 33px;" class="cell_desc" >';
        shtml += '    <input id = "btn_ach_ok"     type = "button" value = "OK"     style = "width: 80px; height: 24px"/>';
        shtml += '    <input id = "btn_ach_cancel" type = "button" value = "CANCEL" style = "width: 80px; height: 24px" /></td>';
        shtml += '</tr>';
        shtml += '</table>';
        
        return shtml;
    }
    
    this.commit_transaction = function()
    {
        var data =  "rem_seq_id~" + this.transaction_id + "|ach_clearing_date~" + this.clearing_date;

        AjaxPro.timeoutPeriod = 1000 * 1000;
        
        AjaxPro.onTimeout = function()
        {
            alert("Server not responding. Commit ach payment failed.");
        }
        
        IRSWorld.TranClass.rem_ach_payments.commit_transaction(data, this.commit_transaction_callback);
    }
     
    this.commit_transaction_callback = function(response)
    {
        if (response != null && response.value != null)
        {
            if (response.value == true)
            {
                eval("javascript:push_ach_payments_commit_response(true);");
            }
            else
            {
                eval("javascript:push_ach_payments_commit_response(false);");
            }
        }
    }    
}    
/*END OF CLEARING DATE*/

/* 23. GENERIC POPUP OR WINDOW GENERATOR */

function gen_winpop()
{
    this.commons = null;    
    this.outer_modal_div = null;
    this.inner_modal_div = null;
    this.inner_div_w     = null;
    this.winpop_id   = null;
    this.winpop_cont = null;

    this.load_css_default = function()
    {
        var file_reference = document.createElement("link");

        file_reference.setAttribute("rel", "stylesheet");
        file_reference.setAttribute("type", "text/css");
        file_reference.setAttribute("href", ".././css/cssmodals.css");

        if (file_reference != null || typeof file_reference != "undefined")
        {
            document.getElementsByTagName("head")[0].appendChild(file_reference);
        }
    }
    
    this.construct_generic_div_content = function()
    {
        var content_div = null;
        
        content_div                       = document.createElement("div");
        content_div.id                    = "content_mod_" + this.winpop_id;
        content_div.style.background      = "transparent";
        content_div.innerHTML             = "<center><img src = '.././images/ajaxloader.gif'></img></center>";                

        this.inner_modal_div              = document.createElement("div");
        this.inner_modal_div.className    = "inner_div";		
        this.inner_modal_div.id           = "inner_mod_div_" + this.winpop_id;
        this.inner_modal_div.style.height = "auto";		
        this.inner_modal_div.style.width  = this.inner_div_w;
        this.inner_modal_div.style.top    = "30px";
        this.inner_modal_div.style.left   = (parseInt(screen.width) - parseInt(this.inner_modal_div.style.width)) / 2 - 10 + "px";
        this.inner_modal_div.style.border = "medium solid white";
        this.inner_modal_div.appendChild(content_div);

        document.body.appendChild(this.inner_modal_div);
        document.getElementById("content_mod_" + this.winpop_id).innerHTML = this.winpop_cont;
    }
    
    this.open_winpop = function()
    {
        this.load_css_default();
        this.outer_modal_div           = document.createElement("div");
        this.outer_modal_div.id        = "outer_mod_div_" + this.winpop_id;
        this.outer_modal_div.className = "outer_div";

        document.body.style.margin = "0px";
        document.body.appendChild(this.outer_modal_div);
        document.getElementById("outer_mod_div_" + this.winpop_id).style.height = screen.height;

        this.construct_generic_div_content();
        this.commons = new common_method();
        this.commons.to_show_hide_combobox(false);
        this.commons = null;
    }
    
    this.close_winpop = function()
    {
        this.commons = new common_method();
        this.commons.to_show_hide_combobox(true);
        this.commons = null;

        if(this.outer_modal_div != null){document.body.removeChild(this.outer_modal_div);this.outer_modal_div  = null;}
        if(this.inner_modal_div != null){document.body.removeChild(this.inner_modal_div);this.inner_modal_div  = null;}

        document.body.style.margin = "";
    }    
}
/* END GENERIC POPUP OR WINDOW GENERATOR */


/* 25. EXPIRED IDS MAINTENANCE */

function expired_ids_scr()
{
    this.client_req_id_seq = null;
    this.client_id = null;
    this.teller_datetime = null;
    this.found_new = null;
    
    this.method_to_call = function(){};


    var exp_id_cls = this;
    var msg_prompt_control = null;
    var content_string = null;
    var winpop = null;
    
    var button_type = null;
    var grid_type   = null;

    content_string = "<center><table style='width: 600px' style='border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid;width:250px;height:auto'>"
    content_string += "    <tr><td colspan='2'><label id = 'lbl_grid_caption' class='cell_desc'></label></td></tr>"
    content_string += "    <tr><td colspan='2'><div id='div_exp_ids'></div></td></tr>"
    content_string += "    <tr>"
    content_string += "        <td style='width: 20%; text-align: right' class='cell_desc'>ID NUMBER:</td>"
    content_string += "        <td style='width: 80%; text-align: left' class='cell_desc'><input id='txt_id_no' type='text' /></td>"
    content_string += "    </tr>"
    content_string += "    <tr>"
    content_string += "        <td style='width: 20%; text-align: right' class='cell_desc'>ID TYPE :</td>"
    content_string += "        <td style='width: 80%; text-align: left' class='cell_desc'><select id='ddl_id_type'><option selected='selected'></option></select></td>"
    content_string += "    </tr>"
    content_string += "    <tr>"
    content_string += "        <td style='width: 20%; text-align: right' class='cell_desc'>EXPIRY DATE:</td>"
    content_string += "        <td style='width: 80%; text-align: left' class='cell_desc'><input id='txt_exp_dt' type='text' /></td>"
    content_string += "    </tr>"    
    content_string += "    <tr>"
    content_string += "        <td style='width: 20%; text-align: right' class='cell_desc'>PLACE OF ISSUANCE:</td>"
    content_string += "        <td style='width: 80%; text-align: left' class='cell_desc'><input id='txt_place_issued' type='text' /></td>"
    content_string += "    </tr>"        
    content_string += "    <tr>"
    content_string += "        <td align='right' style='width: 20%' >"
    content_string += "        </td>"
    content_string += "        <td style='width: 80%'>"
    content_string += "            <input id='btn_id_add' style='width: 50px' type='button' value='ADD' />"
    content_string += "            <input id='btn_id_edit' style='width: 50px' type='button' value='EDIT' />"
    content_string += "            <input id='btn_id_view' style='width: 150px' type='button' value='VIEW ALL CLIENT ID' />"
    content_string += "            <input id='btn_id_commit' style='width: 50px' type='button' value='SAVE' />"
    content_string += "            <input id='btn_id_cancel' style='width: 50px' type='button' value='CANCEL' />"
    content_string += "            <input id='btn_id_close' style='width: 50px' type='button' value='CLOSE' /></td>"
    content_string += "    </tr>"
    content_string += "</table></center>"

    this.open_expired_ids = function()
    {
        winpop = new gen_winpop();

        if (winpop != null)
        {        
            winpop.inner_div_w = "600px";
            winpop.winpop_id   = "id_exp";
            winpop.winpop_cont = content_string;
            winpop.open_winpop();           

            document.getElementById("btn_id_add").onclick = function()
            {
                document.getElementById("ddl_id_type").value = "";
                document.getElementById("txt_id_no").value   = "";
                document.getElementById("txt_exp_dt").value  = ""
                document.getElementById("txt_place_issued").value  = "";

                button_type = 1; exp_id_cls.control_disable(true);
            }

            document.getElementById("btn_id_edit").onclick = function()
            {
                button_type = 2; exp_id_cls.control_disable(true);
            }

            document.getElementById("btn_id_commit").onclick = function()
            {
                if (button_type != null && button_type == 1)
                {
                    exp_id_cls.insert_exp_id();
                }
                else if (button_type != null && button_type == 2)
                {
                    exp_id_cls.edit_exp_id();
                }
            }
            
            document.getElementById("btn_id_view").onclick = function()
            {
                if (document.getElementById("btn_id_view").value == "VIEW ALL CLIENT ID")
                {
                    exp_id_cls.load_rem_all_ids();
                }
                else if (document.getElementById("btn_id_view").value == "VIEW CLIENT EXPIRED ID")
                {
                    exp_id_cls.load_rem_exp_ids();
                }
            }

            document.getElementById("btn_id_cancel").onclick = function()
            {
                button_type = null; exp_id_cls.control_disable(false);
            }

            document.getElementById("btn_id_close").onclick = function()
            {
                if (winpop != null)
                {
                    winpop.close_winpop(); exp_id_cls.method_to_call(); return;
                }
            }

            this.load_req_ids();
        }
    }
    
    this.do_ajaxgridview = function(param)
    {
        var command_param = param.split("$");

        if (command_param.length > 0)
        {
            if (command_param[0] == "ajx_selected")
            {
                this.load_grid_detail(command_param[1]); return;
            }
        }

        if (grid_type == 1)
        {
            this.load_rem_exp_ids(param);
        }
        else if (grid_type == 2)
        {
            this.load_rem_all_ids(param);
        }
    }

    this.load_grid_detail = function(param)
    {
        var param_det = null;
        
        if (param != null)
        {
            param_det = param.split('~');
        }
        
        if (param_det != null && param_det.length > 0)
        {
            this.client_req_id_seq = param_det[0];

            document.getElementById("txt_id_no").value   = param_det[1];
            document.getElementById("txt_exp_dt").value  = param_det[2];
            document.getElementById("txt_place_issued").value  = param_det[4];

            var commons = new common_method();

            commons.to_set_dropdownlist_index("ddl_id_type", param_det[3]);
            commons = null;
        }
    }

    this.insert_exp_id = function()
    {
        msg_prompt_control = new screen_msg_demo();
        msg_prompt_control.open_screen();
        msg_prompt_control.change_msg("Saving ID information please wait ...");
    
        AjaxPro.timeoutPeriod = 1000 * 1000;

        AjaxPro.onTimeout = function()
        {
            alert("Server not responding. Unable to add client required IDs.");
        }

        var id_no     = document.getElementById("txt_id_no").value;
        var id_type   = document.getElementById("ddl_id_type").value;
        var id_dt_exp = document.getElementById("txt_exp_dt").value;
        var id_issued = document.getElementById("txt_place_issued").value;

        IRSWorld.TranClass.rem_exp_ids.insert_rec(this.client_id, id_no, id_type, id_dt_exp, id_issued ,this.insert_exp_id_callback);
    }

    this.insert_exp_id_callback = function(result)
    {
        if (msg_prompt_control != null)
        {
            msg_prompt_control.close_screen();
        }

        msg_prompt_control = null;
        
        if (result != null && result.value != null && result.value == '')
        {
            alert('Record saved.'); exp_id_cls.control_disable(false); exp_id_cls.found_new = true;
        }
        else
        {
            alert(result.value);
        }                
    }
    
    this.edit_exp_id = function()
    {
        msg_prompt_control = new screen_msg_demo();
        msg_prompt_control.open_screen();
        msg_prompt_control.change_msg("Saving ID information please wait ...");

        AjaxPro.timeoutPeriod = 1000 * 1000;

        AjaxPro.onTimeout = function()
        {
            alert("Server not responding. Unable to update client expired.");
        }

        var id_no     = document.getElementById("txt_id_no").value;
        var id_type   = document.getElementById("ddl_id_type").value;
        var id_dt_exp = document.getElementById("txt_exp_dt").value;
        var id_issued = document.getElementById("txt_place_issued").value;

        IRSWorld.TranClass.rem_exp_ids.edit_rec(this.client_req_id_seq, this.client_id, id_no, id_type, id_dt_exp, id_issued ,this.edit_exp_id_callback);
    }

    this.edit_exp_id_callback = function(result)
    {
        if (msg_prompt_control != null)
        {
            msg_prompt_control.close_screen();
        }

        msg_prompt_control = null;

        if (result != null && result.value != null && result.value == '')
        {
            alert('Record saved.'); exp_id_cls.control_disable(false);
        }
        else
        {
            alert(result.value);
        }
    }
    
    this.load_rem_exp_ids = function(param)
    {
        var grid = new grid_viewer();

        if (param == null | param == '')
        {
            param = 'ajx_loading$';
        }
        
        setText("lbl_grid_caption", "LIST OF CLIENT EXPIRED ID - SELECT TO EDIT RECORD");

        grid.sql_source     = "remitter_exp_ids~" + this.client_id + "~" + this.teller_datetime;
        grid.div_container  = document.getElementById("div_exp_ids");
        grid.datarow_column = 0;
        grid.page_size      = 10;
        grid.process_ajax(param);
        grid = null;
        
        grid_type = 1;

        document.getElementById("btn_id_view").value = "VIEW ALL CLIENT ID";
    }
    
    this.load_rem_all_ids = function(param)
    {
        var grid = new grid_viewer();

        if (param == null | param == '')
        {
            param = 'ajx_loading$';
        }

        setText("lbl_grid_caption", "LIST OF CLIENT ID - SELECT TO EDIT RECORD");

        grid.sql_source     = "remitter_all_ids~" + this.client_id;
        grid.div_container  = document.getElementById("div_exp_ids");
        grid.datarow_column = 0;
        grid.page_size      = 10;
        grid.process_ajax(param);
        grid = null;
        
        grid_type = 2;
        
        document.getElementById("btn_id_view").value = "VIEW CLIENT EXPIRED ID";
    }
    
    this.load_req_ids = function()
    {
        this.control_disable(false);

        msg_prompt_control = new screen_msg_demo();
        msg_prompt_control.open_screen();
        msg_prompt_control.change_msg("Loading branch required IDs please wait ...");

        AjaxPro.timeoutPeriod = 1000 * 1000;

        AjaxPro.onTimeout = function()
        {
            alert("Server not responding. Unable to load branch required ids.");
        }

        IRSWorld.TranClass.rem_exp_ids.get_rem_id_req(this.client_id.substring(1,4), this.load_req_ids_callback);
    }
    
    this.load_req_ids_callback = function(result)
    {
        var commons = new common_method();

        commons.to_load_dropdownlist("ddl_id_type", result.value, "id_code_val", "id_code_val_desc");
        commons = null;
        
        if (msg_prompt_control != null)
        {
            msg_prompt_control.close_screen();
        }

        msg_prompt_control = null;
        
        exp_id_cls.load_rem_exp_ids('ajx_loading$');
    }
    
    this.control_disable = function(bool)
    {
        document.getElementById("btn_id_add").disabled  = bool;
        document.getElementById("btn_id_edit").disabled = bool;
        document.getElementById("btn_id_view").disabled = bool;
        document.getElementById("btn_id_commit").disabled  = !bool;
        document.getElementById("btn_id_cancel").disabled = !bool;
        document.getElementById("btn_id_close").disabled = bool;
        
        if (!bool)
        {
            var commons = new common_method();
            
            document.getElementById("txt_id_no").value   = "";
            document.getElementById("txt_exp_dt").value  = ""
            document.getElementById("txt_place_issued").value = "";

            commons.to_set_dropdownlist_index("ddl_id_type", 0);
            commons = null;

            if (grid_type == null || grid_type == 1)
            {
                this.load_rem_exp_ids(null);
            }
            else if (grid_type == 2)
            {
                this.load_rem_all_ids(null);
            }
        }
    }
}

/* END EXPIRED IDS MAINTENANCE */

/* 25. EXPIRED IDS MAINTENANCE */

function rem_states()
{    
    var winpop = null;
    var content_string = null;
    
    content_string = "<table style='width: 400px' style='border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid;width:250px;height:auto'>";
    content_string += "    <tr><td align='center' colspan='2' valign='middle' class='cell_desc'>PLEASE SELECT REMITTER STATE</td></tr>";
    content_string += "    <tr>";
    content_string += "        <td style='width: 20%; text-align: right' class='cell_caption'>States :</td>";
    content_string += "        <td style='width: 80%; text-align: left' class='cell_desc'><select id='ddl_states'><option selected='selected'></option></select></td>"
    content_string += "    </tr>";
    content_string += "    <tr>";
    content_string += "        <td align='right' style='text-align: center;' colspan='2'><input id='btn_ok' type='button' value='OK' style='width: 50px'/></td>";
    content_string += "    </tr>";
    content_string += "</table>";
    

    this.remitter_address_seq_id = 0;
    this.method_to_call = function(){};
    
    var rem_state_cls = this;
    
    this.open_remitter_states = function()
    {
        winpop = new gen_winpop();
        winpop.inner_div_w = "400px";
        winpop.winpop_id   = "id_states";
        winpop.winpop_cont = content_string;
        winpop.open_winpop();
        
        document.getElementById("ddl_states").style.visibility = "visible";
                
        document.getElementById("btn_ok").onclick = function()
        {
            rem_state_cls.update_states();
        }
        
        this.load_states();
    }
    
    this.update_states = function()
    {
        var remitter_state_desc = document.getElementById("ddl_states").value;
        
        if (remitter_state_desc == null || remitter_state_desc == "null" || remitter_state_desc == "NULL" || remitter_state_desc == "")
        {
            alert("Please select remitter state"); return;
        }

        IRSWorld.TranClass.rem_states_chk.update_remitter_state(this.remitter_address_seq_id, remitter_state_desc , this.update_states_callback);
    }
    
    this.update_states_callback = function(result)
    {
        if (result != null && result.value == false)
        {
            alert("Error saving remitter state."); return;
        }

        winpop.close_winpop(); 
        rem_state_cls.method_to_call();
    }
    
    this.chk_rem_states = function()
    {
        IRSWorld.TranClass.rem_states_chk.chk_state_valid(rem_state_cls.remitter_address_seq_id, rem_state_cls.chk_rem_states_callback);
    }
  
    this.chk_rem_states_callback = function(result)
    {
        if (result != null)
        {
            if (result.value == true)
            {
                rem_state_cls.method_to_call();
            }
            else
            {
                rem_state_cls.open_remitter_states();
            }
        }
    }
    
    this.load_states = function()
    {
        IRSWorld.TranClass.rem_states_chk.get_states(this.load_states_callback);
    }
    
    this.load_states_callback = function(result)
    {
        if (result != null)
        {
            var commons = new common_method();

            commons.to_load_dropdownlist("ddl_states", result.value, "fldcode", "flddescription");
            commons = null;
        }
    }
}

/* END EXPIRED IDS MAINTENANCE */

/* 26. IBFT - BANK SCREEN */

function ibft_scr()
{
    this.beneficiary_id = "";
    this.remitter_id = "";
    this.account_no = "";
    
    this.account_seq_id = 0;
    this.account_currency = 0;
    this.account_type = 0;
    this.ibft_bank = 0;

    this.msg_prompt_control = null;

    var winpop = null;
    
    var content_string1 = null;
    var content_string2 = null;
    var content_string3 = null;
    
    var ibt_class = this;    
    
    content_string1 = "<table style='width: 100%; border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid;height:auto'>";    
    content_string1 +="     <tr>";
    content_string1 +="         <td class='cell_desc' style='width: 258px; text-align: right;'>Account Number :</td>";
    content_string1 +="         <td class='cell_desc' style='width: 480px'><input id='txt_account_no_ft' type='text' /></td>";
    content_string1 +="     </tr>";
    content_string1 +="     <tr>";
    content_string1 +="         <td class='cell_desc' style='width: 258px; text-align: right'>Account Type :</td>";
    content_string1 +="         <td class='cell_desc' style='width: 480px'><select id='ddl_account_type_ft'><option selected='selected'></option></select></td>";
    content_string1 +="     </tr>";
    content_string1 +="     <tr>";
    content_string1 +="         <td class='cell_desc' style='width: 258px; text-align: right'>Account Currency :</td>";
    content_string1 +="         <td class='cell_desc' style='width: 480px'><select id='ddl_account_curr_ft'><option selected='selected'></option></select></td>";
    content_string1 +="     </tr>";
    content_string1 +="     <tr>";
    content_string1 +="         <td class='cell_desc' style='width: 258px; text-align: right'>Bank Name :</td>";
    content_string1 +="         <td class='cell_desc' style='width: 480px'><select id='ddl_ibft_bank_ft'><option selected='selected'></option></select></td>";
    content_string1 +="     </tr>"; 
    content_string1 +="     <tr>";
    content_string1 +="        <td colspan='2' style='text-align: center'>";
    content_string1 +="            <input id='btn_save' type='button' value='ADD' style='width: 100px' />";
    content_string1 +="            <input id='btn_back' type='button' value='BACK' style='width: 100px' />";
    content_string1 +="        </td>";
    content_string1 +="    </tr>"
    content_string1 +="</table>";;
    
    content_string2 = "<table style='width: 100%; border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid;height:auto'>";
    content_string2 +="    <tr>";
    content_string2 +="        <td class='cell_desc' style='text-align: right; width: 189px;'>Select Account:</td>";
    content_string2 +="        <td style='width: 286px; height: 26px;'><select id='ddl_acct'><option selected='selected'></option></select></td>";
    content_string2 +="    </tr>";
    content_string2 +="    <tr>";
    content_string2 +="        <td colspan='2' style='text-align: center'>";
    content_string2 +="            <input id='btn_ok' type='button' value='OK' style='width: 100px' />";
    content_string2 +="            <input id='btn_add' type='button' value='ADD ACCOUNT' style='width: 100px' />";
    content_string2 +="        </td>";
    content_string2 +="    </tr>";
    content_string2 +="</table>";
    
    content_string3 = "<table style='width: 100%; border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid;height:auto'>";
    content_string3 +="<tr>";
    content_string3 +="    <td class='cell_desc' style='text-align: center; width: 189px;'>BENEFICARY ID</td>";
    content_string3 +="    <td class='cell_desc' style='width: 286px; height: 26px; text-align: center;'>BENEFICIARY NAME</td>";
    content_string3 +="</tr>";
    content_string3 +="<tr>";
    content_string3 +="     <td style='width: 189px; text-align: center'><label id='lbl_benef_id'></label></td>";
    content_string3 +="     <td style='width: 286px; height: 26px; text-align: center'><label id='lbl_benef'></label></td>";
    content_string3 +="</tr>";
    content_string3 +="</table><p></p>";
    
    this.load_beneficiary = function()
    {
        IRSWorld.TranClass.ibft_scr.get_beneficiary_name(this.beneficiary_id, this.load_beneficiary_callback);
    }

    this.load_beneficiary_callback = function(response)
    {
        if (response != null && response.value != null)
        {
            var ds = response.value;

            if (ds != null)
            {
                setText("lbl_benef_id", ds.Tables[0].Rows[0].CIFID);
                setText("lbl_benef", ds.Tables[0].Rows[0].beneficiary_name);
            }
        }
    }
    
    this.open_ibft = function()
    {
        this.load_acct_scr();
    }
    
    this.load_acct_scr = function()
    {
        if (winpop != null)
        {
            winpop.close_winpop();
            winpop = null;
        }

        winpop = new gen_winpop();
        winpop.inner_div_w = "600px";
        winpop.winpop_id   = "ibft_scr";
        winpop.winpop_cont = content_string3 + content_string2;
        winpop.open_winpop();

        document.getElementById("ddl_acct").style.visibility = "visible";
        
        document.getElementById("btn_ok").onclick = function()
        {
            var combo_split = document.getElementById("ddl_acct").value;
            
            if (combo_split != null && combo_split != "")
            {
                combo_split = combo_split.split('~');
                
                if (combo_split != null && combo_split.length > 0)
                {                    
                    ibt_class.account_seq_id = combo_split[0];
                    ibt_class.ibft_bank = combo_split[1];
                    ibt_class.account_type = combo_split[2];
                    ibt_class.account_currency = combo_split[3];
                    ibt_class.account_no = combo_split[4];
                    
                    if (ibt_class.account_currency != null && ibt_class.account_currency != "" && ibt_class.account_seq_id != null && ibt_class.account_seq_id != "" && ibt_class.ibft_bank != null && ibt_class.ibft_bank != "")
                    {
                        if (winpop != null)
                        {
                            winpop.close_winpop();
                            winpop = null;

                            eval("javascript:push_ibft_src();"); return;
                        }
                    }
                }
            }
            
            alert("Please select account");
        }
        
        document.getElementById("btn_add").onclick = function()
        {
            ibt_class.load_acct_add();
            ibt_class.load_beneficiary();
        }
        
        this.load_beneficiary();
        this.load_ddl_accounts();
    }
    
    this.load_acct_add = function()
    {
        if (winpop != null)
        {
            winpop.close_winpop();
            winpop = null;
        }

        winpop = new gen_winpop();
        winpop.inner_div_w = "600px";
        winpop.winpop_id   = "ibft_scr";
        winpop.winpop_cont = content_string3 + content_string1;
        winpop.open_winpop();
        
        document.getElementById("ddl_account_type_ft").style.visibility = "visible";
        document.getElementById("ddl_account_curr_ft").style.visibility = "visible";
        document.getElementById("ddl_ibft_bank_ft").style.visibility    = "visible";
        
        document.getElementById("btn_back").onclick = function()
        {
            ibt_class.load_acct_scr();
        }
        
        document.getElementById("btn_save").onclick = function()
        {
            var incomplete = false;
            var err_msg = "";
        
            var a = document.getElementById("txt_account_no_ft").value;
            var b = document.getElementById("ddl_account_curr_ft").value;
            var c = document.getElementById("ddl_ibft_bank_ft").value;
            
            if (a == null || a == "null" || a == "")
            {
                incomplete = true; err_msg += "\nAccount number is required.";
            }
            
            if (b == null || b == "null" || b == "")
            {
                incomplete = true; err_msg += "\nAccount currency is required.";
            }
            
            if (c == null || c == "null" || c == "")
            {
                incomplete = true; err_msg += "\nBank  name is required.";
            }
            
            if (incomplete == true)
            {
                alert(err_msg); return;
            }
        
            if (incomplete == false && confirm("Do you want to save this record?") == true)
            {
                ibt_class.insert_record();
            }
        }

        this.load_ddl_ibft();
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
    
    this.load_ddl_accounts = function()
    {   
        IRSWorld.TranClass.ibft_scr.get_ibft_acct(this.beneficiary_id, this.load_ddl_accounts_callback);
    }
    
    this.load_ddl_accounts_callback = function(response)
    {
    
        if (response != null && response.value != null)
        {
            var ds      = response.value;
            var commons = new common_method();

            commons.to_load_dropdownlist('ddl_acct', response.value, 'fldvalue', 'flddescription');
            commons = null;
            ds = null;
        }
    }
    
    this.load_ddl_ibft = function()
    {
        IRSWorld.TranClass.ibft_scr.get_droplist(this.load_ddl_ibft_callback);
    }
    
    this.load_ddl_ibft_callback = function(response)
    {
        if (response != null && response.value != null)
        {
            var ds      = response.value;
            var commons = new common_method();

            commons.to_load_dropdownlist('ddl_account_type_ft', null, 'FldValue', 'FldDescription', ds.Tables[0]);
            commons.to_load_dropdownlist('ddl_account_curr_ft', null, 'FldValue', 'FldDescription', ds.Tables[1]);
            commons.to_load_dropdownlist('ddl_ibft_bank_ft', null, 'FldValue', 'FldDescription', ds.Tables[2]);
            commons = null;
            
            ds = null;
        }
    }
    
    this.insert_record = function()
    {
        var beneficiary_id = this.beneficiary_id;
        var account_no = "";
        var bank_code = "";
        var account_type = 0;
        var account_currency = 0;

        account_no = document.getElementById("txt_account_no_ft").value;
        account_type = document.getElementById("ddl_account_type_ft").value;
        account_currency = document.getElementById("ddl_account_curr_ft").value;
        bank_code = document.getElementById("ddl_ibft_bank_ft").value;

        if (account_type == null || account_type == "null" || account_type == '')
        {
            account_type = 0;
        }

        if (account_currency == null || account_currency == "null" ||account_currency == '')
        {
            account_currency = 0;
        }

        if (bank_code == null || bank_code == "null" ||bank_code == '')
        {
            bank_code = "";
        }

        IRSWorld.TranClass.ibft_scr.insert_ibft_branch(beneficiary_id, account_no,bank_code,account_type, account_currency,this.insert_record_callback);
    }
    
    this.insert_record_callback = function(response)
    {
        if (response != null && response.value != null)
        {
            if (response.value == '')
            {
                alert("Record Saved.");
            }
            else
            {
                alert(response.value);
            }
        }
    }
}

/* END IBFT - BANK SCREEN */

/* 27. ABUK UNIQUE KEY SEARCH */

function abuk_unique_id_search()
{
    this.grd_source = null;
    this.grd_search = null;

    var hi_search = this;
    var winpop = null;
    var content_string = null;
    
    content_string = "<table style='width: 100%; border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid;height:auto'>";
    content_string +="    <tr>";
    content_string +="        <td class='cell_desc' style='text-align: right; width: 189px;'>ENTER UNIQUE KEY:</td>";
    content_string +="        <td style='width: 80%; text-align: left' class='cell_desc'><input id='txt_hi_grade_key' type='text'/></td>";
    content_string +="    </tr>";
    content_string +="    <tr>";
    content_string +="        <td colspan='2' style='text-align: center'>";
    content_string +="            <input id='btn_ok_search' type='button' value='SEARCH' style='width: 100px' />";
    content_string +="            <input id='btn_no_search' type='button' value='CANCEL' style='width: 100px' />";
    content_string +="        </td>";
    content_string +="    </tr>";
    content_string +="</table>";

    this.close_hi_grade = function()
    {
        if (winpop != null)
        {
            winpop.close_winpop();
            winpop = null;
        }
    }

    this.open_hi_grade = function()
    {
        if (winpop != null)
        {
            winpop.close_winpop();
            winpop = null;
        }

        winpop = new gen_winpop();
        winpop.inner_div_w = "600px";
        winpop.winpop_id   = "abuk_unique_scr";
        winpop.winpop_cont = content_string;
        winpop.open_winpop();
        
        document.getElementById("btn_ok_search").onclick = function()
        {
            hi_search.load_hi_grade_list(document.getElementById("txt_hi_grade_key").value);
        }
        
        document.getElementById("btn_no_search").onclick = function()
        {
            if (winpop != null)
            {
                winpop.close_winpop();
                winpop = null;
            }
        }

        document.getElementById("txt_hi_grade_key").onkeypress = function(ev)
        {
            var e      = window.event || ev;
            var keynum = e.charCode || e.keyCode;

            if (keynum == 13)
            {
                hi_search.load_hi_grade_list(document.getElementById("txt_hi_grade_key").value);
            }
        }
    }
    
    this.load_hi_grade_list = function(old_cifid)
    {
        this.grd_search = new grid_selector();
        this.grd_source = "otc_hi_grade_search~1~" + old_cifid + "~~";
        this.grd_search.open_selection_grid(null, this.grd_source, 0 , null);
    }
    
    this.load_hi_grade_benef = function(param)
    {
        this.grd_search = new grid_selector();
        this.grd_source = "otc_hi_grade_search~2~" + param[0] + "~" + param[1] + "~";
        this.grd_search.open_selection_grid(null, this.grd_source, 0 , null);
    }
    
    this.process_ajax = function(param)
    {
        this.grd_search.process_ajax(param);
    }
}

/* ABUK UNIQUE KEY SEARCH */

/* 28. REMOTE OVERRIDE FUNCTION */

function rem_ovr_scr()
{
    var removerride_cls = this;

    this.msg_prompt_control = null;
    this.jscls = null;
    this.branch_code = "";

    this.chk_branch_is_allowed = function()
    {
        this.open_msg_prompt("Please wait ...");

        AjaxPro.timeoutPeriod = 1000 * 1000;
        AjaxPro.onTimeout = function()
        {
            alert("Server not responding. Sending for override values failed.");
        }

        IRSWorld.TranClass.RemTranExtra.chk_branch_remote_allowed(this.branch_code, this.chk_branch_is_allowed_callback);
    }

    this.chk_branch_is_allowed_callback = function(result)
    {
        removerride_cls.close_msg_prompt();

        if (result != null && result.value != null)
        {
            var branch_ok = result.value.split('~');

            if (branch_ok != null && branch_ok.length == 2 && branch_ok[0] == 1)
            {
                var msg_over = "Transaction will be queued for overriding";
                var ok_to_proceed = confirm(msg_over);

                if (ok_to_proceed == true)
                {
                    removerride_cls.jscls.pending_override_status = branch_ok[1];
                    removerride_cls.queue_ovr_type();
                }
            }
            else
            {
                alert("Remote Override functionality is not activated."); return;
            }
        }
    }
    
    this.queue_ovr_type = function()
    {
        this.open_msg_prompt("Please wait ...");

        AjaxPro.timeoutPeriod = 1000 * 1000;

        AjaxPro.onTimeout = function()
        {
            alert("Server not responding. Setting override type failed");
        }

        IRSWorld.TranClass.RemTranExtra.remote_queue_ovrtype(this.jscls.override_type_code, this.queue_ovr_type_callback);
    }

    this.queue_ovr_type_callback = function(result)
    {
        removerride_cls.close_msg_prompt();

        if (result != null)
        {
            removerride_cls.jscls.run_rmover_closer();
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

    //=========================================================================================
    /* RFC-30454 Creation of new report in IRSW for OTC transactions with OFAC hits */
    this.clear_pending_tranx_session = function ()
    {
        IRSWorld.TranClass.RemTranExtra.clear_pending_tranx_session();
    }

    //* RFC-32829 : Request to enhance the OFAC alert and Report in IRSW 01/14/2024 */
    this.set_pending_tranx_session = function (reson_indv, reason_bus) {
        IRSWorld.TranClass.RemTranExtra.set_pending_tranx_session(reson_indv, reason_bus);
    }
    //=========================================================================================
}

function notes_grid_screen()
{

    this.do_ajaxgridview = function(parameter)
    {
        if (parameter != null && parameter != "undefined")
        {
            var command_param = parameter.split("$");

            if (command_param.length > 0)
            {
                iremit.process_viewer_notes(parameter);
            }
        }
     }
}

/* END REMOTE OVERRIDE FUNCTION */

/* 29. INSTAPAY ALYSSA SEPTEMBER 2019 - BANK SCREEN */

function instapay_scr() {
    this.beneficiary_id = "";
    this.remitter_id = "";
    this.account_no = "";

    this.account_seq_id = 0;
    this.account_currency = 0;
    this.account_type = 0;
    this.instapay_bank = 0;

    this.msg_prompt_control = null;

    var winpop = null;

    var content_string1 = null;
    var content_string2 = null;
    var content_string3 = null;

    var instapay_class = this;

    content_string1 = "<table style='width: 100%; border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid;height:auto'>";
    content_string1 += "     <tr>";
    content_string1 += "         <td class='cell_desc' style='width: 258px; text-align: right;'>Account Number :</td>";
    content_string1 += "         <td class='cell_desc' style='width: 480px'><input id='txt_account_no_ft' type='text' /></td>";
    content_string1 += "     </tr>";
    content_string1 += "     <tr>";
    content_string1 += "         <td class='cell_desc' style='width: 258px; text-align: right'>Account Type :</td>";
    content_string1 += "         <td class='cell_desc' style='width: 480px'><select id='ddl_account_type_ft'><option selected='selected'></option></select></td>";
    content_string1 += "     </tr>";
    content_string1 += "     <tr>";
    content_string1 += "         <td class='cell_desc' style='width: 258px; text-align: right'>Account Currency :</td>";
    content_string1 += "         <td class='cell_desc' style='width: 480px'><select id='ddl_account_curr_ft'><option selected='selected'></option></select></td>";
    content_string1 += "     </tr>";
    content_string1 += "     <tr>";
    content_string1 += "         <td class='cell_desc' style='width: 258px; text-align: right'>Bank Name :</td>";
    content_string1 += "         <td class='cell_desc' style='width: 480px'><select id='ddl_instapay_bank_ft'><option selected='selected'></option></select></td>";
    content_string1 += "     </tr>";
    content_string1 += "     <tr>";
    content_string1 += "        <td colspan='2' style='text-align: center'>";
    content_string1 += "            <input id='btn_save' type='button' value='ADD' style='width: 100px' />";
    content_string1 += "            <input id='btn_back' type='button' value='BACK' style='width: 100px' />";
    content_string1 += "        </td>";
    content_string1 += "    </tr>"
    content_string1 += "</table>";;

    content_string2 = "<table style='width: 100%; border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid;height:auto'>";
    content_string2 += "    <tr>";
    content_string2 += "        <td class='cell_desc' style='text-align: right; width: 189px;'>Select Account:</td>";
    content_string2 += "        <td style='width: 286px; height: 26px;'><select id='ddl_acct'><option selected='selected'></option></select></td>";
    content_string2 += "    </tr>";
    content_string2 += "    <tr>";
    content_string2 += "        <td colspan='2' style='text-align: center'>";
    content_string2 += "            <input id='btn_ok' type='button' value='OK' style='width: 100px' />";
    content_string2 += "            <input id='btn_add' type='button' value='ADD ACCOUNT' style='width: 100px' />";
    content_string2 += "        </td>";
    content_string2 += "    </tr>";
    content_string2 += "</table>";

    content_string3 = "<table style='width: 100%; border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid;height:auto'>";
    content_string3 += "<tr>";
    content_string3 += "    <td class='cell_desc' style='text-align: center; width: 189px;'>BENEFICARY ID</td>";
    content_string3 += "    <td class='cell_desc' style='width: 286px; height: 26px; text-align: center;'>BENEFICIARY NAME</td>";
    content_string3 += "</tr>";
    content_string3 += "<tr>";
    content_string3 += "     <td style='width: 189px; text-align: center'><label id='lbl_benef_id'></label></td>";
    content_string3 += "     <td style='width: 286px; height: 26px; text-align: center'><label id='lbl_benef'></label></td>";
    content_string3 += "</tr>";
    content_string3 += "</table><p></p>";

    this.load_beneficiary = function () {
        IRSWorld.TranClass.instapay_scr.get_beneficiary_name(this.beneficiary_id, this.load_beneficiary_callback);
    }

    this.load_beneficiary_callback = function (response) {
        if (response != null && response.value != null) {
            var ds = response.value;

            if (ds != null) {
                setText("lbl_benef_id", ds.Tables[0].Rows[0].CIFID);
                setText("lbl_benef", ds.Tables[0].Rows[0].beneficiary_name);
            }
        }
    }

    this.open_instapay = function () {
        this.load_acct_scr();
    }

    this.load_acct_scr = function () {
        if (winpop != null) {
            winpop.close_winpop();
            winpop = null;
        }

        winpop = new gen_winpop();
        winpop.inner_div_w = "600px";
        winpop.winpop_id = "instapay_scr";
        winpop.winpop_cont = content_string3 + content_string2;
        winpop.open_winpop();

        document.getElementById("ddl_acct").style.visibility = "visible";

        document.getElementById("btn_ok").onclick = function () {
            var combo_split = document.getElementById("ddl_acct").value;

            if (combo_split != null && combo_split != "") {
                combo_split = combo_split.split('~');

                if (combo_split != null && combo_split.length > 0) {
                    instapay_class.account_seq_id = combo_split[0];
                    instapay_class.instapay_bank = combo_split[1];
                    instapay_class.account_type = combo_split[2];
                    instapay_class.account_currency = combo_split[3];
                    instapay_class.account_no = combo_split[4];

                    if (instapay_class.account_currency != null && instapay_class.account_currency != "" && instapay_class.account_seq_id != null && instapay_class.account_seq_id != "" && instapay_class.instapay_bank != null && instapay_class.instapay_bank != "") {
                        if (winpop != null) {
                            winpop.close_winpop();
                            winpop = null;

                            eval("javascript:push_instapay_src();"); return;
                        }
                    }
                }
            }

            alert("Please select account");
        }

        document.getElementById("btn_add").onclick = function () {
            instapay_class.load_acct_add();
            instapay_class.load_beneficiary();
        }

        this.load_beneficiary();
        this.load_ddl_accounts();
    }

    this.load_acct_add = function () {
        if (winpop != null) {
            winpop.close_winpop();
            winpop = null;
        }

        winpop = new gen_winpop();
        winpop.inner_div_w = "600px";
        winpop.winpop_id = "instapay_scr";
        winpop.winpop_cont = content_string3 + content_string1;
        winpop.open_winpop();

        document.getElementById("ddl_account_type_ft").style.visibility = "visible";
        document.getElementById("ddl_account_curr_ft").style.visibility = "visible";
        document.getElementById("ddl_instapay_bank_ft").style.visibility = "visible";

        document.getElementById("btn_back").onclick = function () {
            instapay_class.load_acct_scr();
        }

        document.getElementById("btn_save").onclick = function () {
            var incomplete = false;
            var err_msg = "";

            var a = document.getElementById("txt_account_no_ft").value;
            var b = document.getElementById("ddl_account_curr_ft").value;
            var c = document.getElementById("ddl_instapay_bank_ft").value;

            if (a == null || a == "null" || a == "") {
                incomplete = true; err_msg += "\nAccount number is required.";
            }

            if (b == null || b == "null" || b == "") {
                incomplete = true; err_msg += "\nAccount currency is required.";
            }

            if (c == null || c == "null" || c == "") {
                incomplete = true; err_msg += "\nBank  name is required.";
            }

            if (incomplete == true) {
                alert(err_msg); return;
            }

            if (incomplete == false && confirm("Do you want to save this record?") == true) {
                instapay_class.insert_record();
            }
        }

        this.load_ddl_instapay()
    }

    this.open_msg_prompt = function (msg) {
        this.msg_prompt_control = new screen_msg_demo();
        this.msg_prompt_control.open_screen();
        this.msg_prompt_control.change_msg(msg);
    }

    this.close_msg_prompt = function () {
        if (this.msg_prompt_control != null) {
            this.msg_prompt_control.close_screen();
        }

        this.msg_prompt_control = null;
    }

    this.load_ddl_accounts = function () {
        IRSWorld.TranClass.instapay_scr.get_instapay_acct(this.beneficiary_id, this.load_ddl_accounts_callback);
    }

    this.load_ddl_accounts_callback = function (response) {

        if (response != null && response.value != null) {
            var ds = response.value;
            var commons = new common_method();

            commons.to_load_dropdownlist('ddl_acct', response.value, 'fldvalue', 'flddescription');
            commons = null;
            ds = null;
        }
    }

    this.load_ddl_instapay = function () {
        IRSWorld.TranClass.instapay_scr.get_droplist(this.load_ddl_instapay_callback);
    }

    this.load_ddl_instapay_callback = function (response) {
        if (response != null && response.value != null) {
            var ds = response.value;
            var commons = new common_method();

            commons.to_load_dropdownlist('ddl_account_type_ft', null, 'FldValue', 'FldDescription', ds.Tables[0]);
            commons.to_load_dropdownlist('ddl_account_curr_ft', null, 'FldValue', 'FldDescription', ds.Tables[1]);
            commons.to_load_dropdownlist('ddl_instapay_bank_ft', null, 'FldValue', 'FldDescription', ds.Tables[2]);
            commons = null;

            ds = null;
        }
    }

    this.insert_record = function () {
        var beneficiary_id = this.beneficiary_id;
        var account_no = "";
        var bank_code = "";
        var account_type = 0;
        var account_currency = 0;

        account_no = document.getElementById("txt_account_no_ft").value;
        account_type = document.getElementById("ddl_account_type_ft").value;
        account_currency = document.getElementById("ddl_account_curr_ft").value;
        bank_code = document.getElementById("ddl_instapay_bank_ft").value;

        if (account_type == null || account_type == "null" || account_type == '') {
            account_type = 0;
        }

        if (account_currency == null || account_currency == "null" || account_currency == '') {
            account_currency = 0;
        }

        if (bank_code == null || bank_code == "null" || bank_code == '') {
            bank_code = "";
        }
        IRSWorld.TranClass.instapay_scr.insert_instapay_branch(beneficiary_id, account_no, bank_code, account_type, account_currency, this.insert_instapay_record_callback);
    }

    this.insert_instapay_record_callback = function (response) {
        if (response != null && response.value != null) {
            if (response.value == '') {
                alert("Record Saved.");
            }
            else {
                alert(response.value);
            }
        }
        instapay_class.load_acct_scr();
    }
}

/* END INSTAPAY - BANK SCREEN */