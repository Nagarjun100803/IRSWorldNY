
function override_calculator()
{
    var icls = this;

    this.currency_rate_source_to_target                = 0;
    this.currency_rate_source_to_target_curr_code_desc = '';    
    this.currency_rate_target_to_source                = 0;
    this.currency_rate_target_to_source_curr_code_desc = '';
    
    this.currency_rate_target_to_usd                   = 0;    
    this.currency_rate_target_to_usd_curr_code_desc    = '';
    this.currency_rate_target_to_php                   = 0;
    this.currency_rate_target_to_php_curr_code_desc    = '';
    this.currency_rate_target_to_eur                   = 0;
    this.currency_rate_target_to_eur_curr_code_desc    = '';
    
    this.currency_rate_source_to_usd                   = 0;
    this.currency_rate_source_to_usd_code_desc         = '';
    this.currency_rate_source_to_php                   = 0;
    this.currency_rate_source_to_php_code_desc         = '';

    this.outer_modal_div  = null;
    this.inner_modal_div  = null;
    this.commons          = null;

    this.login_result_flag = null;
    this.login_result_msgs = null;

    this.irs_calculator = null;
    this.array_override_data = null;    
    this.irs_calculator_session_name = null;
    
    this.array_override_joined = null;
    var mw = new modal_window();
    this.open_override_form = function(irs_calculator_session_name)
    {
        this.irs_calculator = IRSWorld.BillsPayment.OBPSTransactions.get_calculator_for_override_values(irs_calculator_session_name).value;
        this.irs_calculator_session_name = irs_calculator_session_name;

        if (this.irs_calculator != null)
        {            
            //mw.open_modal_window();
            document.getElementById("trOverride").style.display = 'block';
            document.getElementById("trOverrideDetails").style.display = 'block';
            document.getElementById("btnOverrideCharges").style.display = 'none';
            document.getElementById("btnBClosePanel").style.display = 'none';
            document.getElementById("btnWaive").style.display = 'none';
            document.getElementById("trCharges").style.display = 'none';
            document.getElementById("trChargesDetails").style.display = 'none';
//            showPanel("trOverride");
//            showPanel("trOverrideDetails");
//            showPanel("tdlblCharges");
//            showPanel("calc");
            
            //this.construct_override_window();
        }
    }
    
    this.construct_override_window = function()
    {
        var irs_calculator_data = this.irs_calculator;

        if (irs_calculator_data != null)
        {
            for(var i = 0; i < irs_calculator_data.Tables[0].Rows.length; i++)
            {
                this.currency_rate_source_to_target                = irs_calculator_data.Tables[0].Rows[i].SourceToTarget;
                this.currency_rate_target_to_source                = irs_calculator_data.Tables[0].Rows[i].TargetToSource;
                this.currency_rate_source_to_target_curr_code_desc = irs_calculator_data.Tables[0].Rows[i].SourceCurrShort;
                this.currency_rate_target_to_source_curr_code_desc = irs_calculator_data.Tables[0].Rows[i].TargetCurrShort;

                switch(irs_calculator_data.Tables[0].Rows[i].currency_factor_code.toLowerCase())
                {
                    case "currency_rate_target_to_usd" :

                        this.currency_rate_target_to_usd           = irs_calculator_data.Tables[0].Rows[i].currency_factor_value;
                        this.currency_rate_target_to_usd_code_desc = irs_calculator_data.Tables[0].Rows[i].TargetCurrShort;
                        break;

                    case "currency_rate_target_to_php" :

                        this.currency_rate_target_to_php           = irs_calculator_data.Tables[0].Rows[i].currency_factor_value;
                        this.currency_rate_target_to_php_code_desc = irs_calculator_data.Tables[0].Rows[i].TargetCurrShort;
                        break;

                    case "currency_rate_target_to_eur" :

                        this.currency_rate_target_to_eur           = irs_calculator_data.Tables[0].Rows[i].currency_factor_value;
                        this.currency_rate_target_to_eur_code_desc = irs_calculator_data.Tables[0].Rows[i].TargetCurrShort;
                        break;

                    case "currency_rate_source_to_usd" :

                        this.currency_rate_source_to_usd           = irs_calculator_data.Tables[0].Rows[i].currency_factor_value;
                        this.currency_rate_source_to_eur_code_desc = irs_calculator_data.Tables[0].Rows[i].SourceCurrShort;
                        break;
                    
                    case "currency_rate_source_to_php" :
                    
                        this.currency_rate_source_to_php           = irs_calculator_data.Tables[0].Rows[i].currency_factor_value;
                        this.currency_rate_source_to_php_code_desc = irs_calculator_data.Tables[0].Rows[i].SourceCurrShort;
                        break;
                }
            }
        }
        
        var html_string = "";
        
//        html_string += "<table style='border: solid 1px #336699;' cellspacing='1' cellpadding='0' width='100%'>";
//        html_string += "    <tr  class = 'cell_caption'>";
//        html_string += "        <td align = 'center'>" + this.currency_rate_target_to_usd_code_desc + " TO USD </br> (TARGET TO USD)</td>";
//        html_string += "        <td align = 'center'>" + this.currency_rate_target_to_php_code_desc + " TO PHP </br> (TARGET TO PHP)</td>";
//        html_string += "        <td align = 'center'>" + this.currency_rate_target_to_eur_code_desc + " TO EUR </br> (TARGET TO EUR)</td>";
//        html_string += "        <td align = 'center'>" + this.currency_rate_target_to_source_curr_code_desc + " TO " + this.currency_rate_source_to_target_curr_code_desc + "</br>(TARGET TO SOURCE)</td>";
//        html_string += "        <td align = 'center'>" + this.currency_rate_source_to_target_curr_code_desc + " TO " + this.currency_rate_target_to_source_curr_code_desc + "</br>(SOURCE TO TARGET)</td>";
//        html_string += "    </tr>";
//        html_string += "    <tr class = 'cell_desc' align = 'center'>";
//        html_string += "        <td>" + this.currency_rate_target_to_usd.toFixed(9)    + "</td>";
//        html_string += "        <td>" + this.currency_rate_target_to_php.toFixed(9)    + "</td>";
//        html_string += "        <td>" + this.currency_rate_target_to_eur.toFixed(9)    + "</td>";
//        html_string += "        <td>" + this.currency_rate_target_to_source.toFixed(9) + "</td>";
//        html_string += "        <td>" + this.currency_rate_source_to_target.toFixed(9) + "</td>";
//        html_string += "    </tr>";
//        html_string += "</table></br>";

//        html_string += "<table style='border: solid 1px #336699;' cellspacing='1' cellpadding='0' width='100%'>";
//        html_string += "    <tr  class = 'cell_caption'>";
//        html_string += "        <td align = 'center'>USD TO " + this.currency_rate_target_to_usd_code_desc + "</br>(USD TO TARGET)</td>";
//        html_string += "        <td align = 'center'>PHP TO " + this.currency_rate_target_to_php_code_desc + "</br>(PHP TO TARGET)</td>";
//        html_string += "        <td align = 'center'>EUR TO " + this.currency_rate_target_to_eur_code_desc + "</br>(EUR TO TARGET)</td>";
//        html_string += "    </tr>";
//        html_string += "    <tr class = 'cell_desc' align = 'center'>";
//        html_string += "        <td>" + (1 / this.currency_rate_target_to_usd).toFixed(9) + "</td>";
//        html_string += "        <td>" + (1 / this.currency_rate_target_to_php).toFixed(9) + "</td>";
//        html_string += "        <td>" + (1 / this.currency_rate_target_to_eur).toFixed(9) + "</td>";
//        html_string += "    </tr>";
//        html_string += "</table></br>";
        
        html_string += "<table id = 'tbl_override_forex_rates' style='border: solid 1px #336699;' cellspacing='1' cellpadding='0' width='100%'>";
        html_string += "    <tr  class = 'cell_caption'>";
        html_string += "        <td align = 'center'>" + this.currency_rate_target_to_usd_code_desc + " TO USD </br> (TARGET TO USD)</td>";
        html_string += "        <td align = 'center'>" + this.currency_rate_target_to_php_code_desc + " TO PHP </br> (TARGET TO PHP)</td>";
        html_string += "        <td align = 'center'>" + this.currency_rate_target_to_eur_code_desc + " TO EUR </br> (TARGET TO EUR)</td>";
        html_string += "        <td align = 'center'>" + this.currency_rate_target_to_source_curr_code_desc + " TO " + this.currency_rate_source_to_target_curr_code_desc + "</br>(TARGET TO SOURCE)</td>";
        html_string += "    </tr>";
        html_string += "    <tr class = 'cell_desc' align = 'center'>";
        html_string += "        <td><input id= 'txt_currency_rate_target_to_usd'    onkeyup = 'javascript: this.orig_value = this.value' onblur = 'javascript: eval(this.value = (1 * this.orig_value).toFixed(9))' onfocus = 'javascript: this.orig_value = this.value; eval(this.value = (1 * this.value).toFixed(2))' type='text' style = 'text-align:center; width:auto' value = '" + this.currency_rate_target_to_usd.toFixed(9)    + "'/></td>";
        html_string += "        <td><input id= 'txt_currency_rate_target_to_php'    onkeyup = 'javascript: this.orig_value = this.value' onblur = 'javascript: eval(this.value = (1 * this.orig_value).toFixed(9))' onfocus = 'javascript: this.orig_value = this.value; eval(this.value = (1 * this.value).toFixed(2))' type='text' style = 'text-align:center; width:auto' value = '" + this.currency_rate_target_to_php.toFixed(9)    + "'/></td>";
        html_string += "        <td><input id= 'txt_currency_rate_target_to_eur'    onkeyup = 'javascript: this.orig_value = this.value' onblur = 'javascript: eval(this.value = (1 * this.orig_value).toFixed(9))' onfocus = 'javascript: this.orig_value = this.value; eval(this.value = (1 * this.value).toFixed(2))' type='text' style = 'text-align:center; width:auto' value = '" + this.currency_rate_target_to_eur.toFixed(9)    + "'/></td>";
        html_string += "        <td><input id= 'txt_currency_rate_target_to_source' onkeyup = 'javascript: this.orig_value = this.value' onblur = 'javascript: eval(this.value = (1 * this.orig_value).toFixed(9))' onfocus = 'javascript: this.orig_value = this.value; eval(this.value = (1 * this.value).toFixed(2))' type='text' style = 'text-align:center; width:auto' value = '" + this.currency_rate_target_to_source.toFixed(9) + "'/></td>";
        html_string += "    </tr>";
        html_string += "</table></br>";
        
        html_string += "<table id = 'tbl_override_charges_setup' style='border: solid 1px #336699;' cellspacing='1' cellpadding='0' width='100%'>";
        html_string += "    <tr  class = 'cell_caption'>";
        html_string += "        <td align = 'center'>CHARGE NAME</td>";
        html_string += "        <td align = 'center'>CURRENCY</td>";
        html_string += "        <td align = 'center'>AMOUNT</td>";
        html_string += "    </tr>"
        
        if (irs_calculator_data != null)
        {   
            var chargename = ''; 
            var intcount = 0;
            var chargecurrency = '';    
            for(var i = 0; i < irs_calculator_data.Tables[1].Rows.length; i++)
            {
                var txt_charge_name_hidden = "<input type = 'hidden' style = 'text-align:right; width:auto' id = 'hidden_charges_id_" + (i + 1) + "' value = '" + irs_calculator_data.Tables[1].Rows[i].ChargeName + "'/>";
                var txt_charge_name_txtbox = "<input type = 'text'   style = 'text-align:right; width:auto' id = 'txt_charges_value_" + (i + 1) + "' value = '" + ( 1 * irs_calculator_data.Tables[1].Rows[i].ChargeValue).toFixed(2) + "'/>";
                
                html_string += "    <tr  class = 'cell_desc'>";
                html_string += "        <td align = 'left'>"   + irs_calculator_data.Tables[1].Rows[i].ChargeNameDesc.toUpperCase()    + "</td>";
                html_string += "        <td align = 'center'>" + irs_calculator_data.Tables[1].Rows[i].CurrencyShortCode + "</td>";
                html_string += "        <td align = 'right'>"  + txt_charge_name_hidden + txt_charge_name_txtbox + "</td>";
                html_string += "    </tr>"
                chargename += irs_calculator_data.Tables[1].Rows[i].ChargeNameDesc.toUpperCase() + "~";
                intcount += 1;
                chargecurrency = irs_calculator_data.Tables[1].Rows[i].CurrencyShortCode;
            }
            var charges = IRSWorld.BillsPayment.OBPSTransactions.getLookUp('ChargeName').value;
            
            for(var i2 = 0; i2 < charges.Tables[0].Rows.length; i2++)
            {
                var equalcount = 0;
                if(chargename.split('~').length > 0)
                {
                    for(var i3 = 0; i3 < chargename.split('~').length; i3++)
                    {
                        if(chargename.split('~')[i3].length > 0)
                        {
                            if(chargename.split('~')[i3].toUpperCase() == charges.Tables[0].Rows[i2].FldDescription.toUpperCase())
                            {
                                equalcount += 1;
                                break;
                            }
                        }
                    }
                }
                if (equalcount > 0)
                {                    
                    
                }
                else
                {
                    if (intcount == 3)
                    {
                        chargecurrency = 'PHP';
                    }                    
                    var txt_charge_name_hidden = "<input type = 'hidden' style = 'text-align:right; width:auto' id = 'hidden_charges_id_" + (intcount + 1) + "' value = '" + charges.Tables[0].Rows[i2].FldDescription + "'/>";
                    var txt_charge_name_txtbox = "<input type = 'text'   style = 'text-align:right; width:auto' id = 'txt_charges_value_" + (intcount + 1) + "' value = '0.00'/>";
                    
                    html_string += "    <tr  class = 'cell_desc'>";
                    html_string += "        <td align = 'left'>"   + charges.Tables[0].Rows[i2].FldDescription.toUpperCase()    + "</td>";
                    html_string += "        <td align = 'center'>" + chargecurrency + "</td>";
                    html_string += "        <td align = 'right'>"  + txt_charge_name_hidden + txt_charge_name_txtbox + "</td>";
                    html_string += "    </tr>"
                    intcount += 1;
                }
            }
        }
        
        html_string += "</table></br>";
        
        html_string += "<table style='border: solid 1px #336699;' cellspacing='1' cellpadding='0' width='100%'>";
        html_string += "    <tr>";
        html_string += "        <td class = 'cell_caption' style = 'width: 30%'>LOGIN NAME:</td>";
        html_string += "        <td class = 'cell_desc' align='left' style = 'width: 30%'><input type='text' id = 'txt_username' style = 'width: 100%'/></td>";
        html_string += "        <td class = 'cell_desc' align='center'><input id='btn_override' style='width: 50%; height:15%' type='button' value='APPLY CHANGES' /></td>";
        html_string += "    </tr>";
        html_string += "    <tr>";
        html_string += "        <td class = 'cell_caption' style = 'width: 30%'>PASSWORD:</td>";
        html_string += "        <td class = 'cell_desc' align='left' style = 'width: 30%'><input type='password' id = 'txt_password' style = 'width: 100%'/></td>";
        html_string += "        <td class = 'cell_desc' align='center'><input id='btn_cancel' style='width: 50%; height:15%' type='button' value='CANCEL'/></td>";
        html_string += "    </tr>";
        html_string += "</table></br>";
        
//        html_string += "</br><center><input id='btn_override' style='width: 15%; height:15%' type='button' value='APPLY CHANGES' />&nbsp<input id='btn_cancel' style='width: 15%; height:15%' type='button' value='CANCEL'/></center></br>";

        document.getElementById("content_mod_div").innerHTML = html_string;
        
        document.getElementById("content_mod_div").document.getElementById("btn_override").onclick = function()
        {
            if(document.getElementById("content_mod_div").document.getElementById("txt_username").value == '')
            {
                alert('Username is empty.');
            }
            else if (document.getElementById("content_mod_div").document.getElementById("txt_password").value == '')
            {
                alert('Password is empty.');
            }
            else
            {
                //RemTranCommons.user_can_override_rates(document.getElementById("content_mod_div").document.getElementById("txt_username").value, document.getElementById("content_mod_div").document.getElementById("txt_password").value, icls.override_callback);
            }
        }
        document.getElementById("btn_cancel").onclick = Function('hidePanel("divMerchantDetails");');
//        document.getElementById("content_mod_div").document.getElementById("btn_cancel").onclick = Function('hidePanel("divMerchantDetails");');//function()
//        {
//            //mw.close_modal_window();
//            hidePanel('divMerchantDetails');
//        }
        
        irs_calculator_data = null;
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
    
    this.parse_data_changes = function()
    {
        if (this.irs_calculator != null)
        {
            this.array_override_data = new Array();

            if (this.irs_calculator.Tables[0].Rows.length > 0)
            {
                this.array_override_data[this.array_override_data.length] = "FOREX_RATES" + "~" + "currency_rate_target_to_usd"    + "~" + document.getElementById("txt_currency_rate_target_to_usd").value;
                this.array_override_data[this.array_override_data.length] = "FOREX_RATES" + "~" + "currency_rate_target_to_php"    + "~" + document.getElementById("txt_currency_rate_target_to_php").value;
                this.array_override_data[this.array_override_data.length] = "FOREX_RATES" + "~" + "currency_rate_target_to_eur"    + "~" + document.getElementById("txt_currency_rate_target_to_eur").value;
                this.array_override_data[this.array_override_data.length] = "FOREX_RATES" + "~" + "currency_rate_target_to_source" + "~" + document.getElementById("txt_currency_rate_target_to_source").value;                
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
            
            this.array_override_joined =  this.array_override_data.join("|");
        }
    }
    
    this.send_parsed_data_callback = function(result)
    {
        if (result != null)
        {
            //ETO HINDI KO MAINTINDIHAN KUNG PANO IPASA YUNG VALUES
            eval("javascript:trigger_override_result(icls);");
        }
    }
    
    
    this.open_override_window = function()
    {
        var RemID = document.getElementById('lbl_rem_id').innerText;
        if (RemID != '')
        {            
            this.obps_override_cls = new override_calculator();
            this.obps_override_cls.open_override_form("OBPSCalculator");
        }
        else
        {
            alert('Search a remitter first.');
        }
    }
    
    this.waivecharges = function()
    {
        var target_amt = document.getElementById('txtTargetAmount').value;
        var account_curr = document.getElementById('lbl_merchant_currency').value;
        var display = IRSWorld.BillsPayment.OBPSTransactions.waiveCharges(product_charges_id,account_curr,false,3,target_amt).value;
        document.getElementById('calc').innerHTML = display;
        remteller.getchargesdet();
    }
}