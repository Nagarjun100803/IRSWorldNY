var ro = new rem_Override();
var ropopups = new RO_popups();
var divmask;
var override_list;
var override_type;
var override_rows;
        
function rem_Override()
{
    this.msg_prompt_control = null;
    
   
    
    this.prepare_override = function(prodchargesid,overridetype)
    {
        override_type = overridetype;
        if (ropopups == null)
            roppopups = new RO_popups();
            
        ropopups.construct_ro_objects();
        this.setProductChargesID(prodchargesid);
        this.setOverrideType(overridetype);        
    }    
    this.setProductChargesID = function(pcdval)
    {
        document.getElementById("hidPCD").value = pcdval;        
    }
    this.getProductChargesID = function()
    {
        return document.getElementById("hidPCD").value;
    }
     this.setOverrideType = function(otype)
    {
        document.getElementById("hidOT").value = otype;        
    }
    this.getOverrideType = function()
    {
        return document.getElementById("hidOT").value;
    }
    this.getTranDesc = function()
    {
        return getText("lbl_product_type") + "-" + getText("lbl_implementation_type");
    }
    this.overrideid = function()
    {
        return document.getElementById("txtsessid").value.toString();
    }
    this.get_amount = function()
    {
        return document.getElementById("txt_remit_amount").value.toString();
    }
    
    this.check_status = function()     
    {                   
        try
        {
            var oid = ro.overrideid();
            if (isNaN(oid))
            {
                return;
            }
            setText("lblstatus","CHECKING STATUS");
        }
        catch(e)
        {   
            return;
        }
        
        AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. CHECKING OF STATUS FAILED.';
            alert(errmsg); 
            var result = new Array("99","TIMEOUT",errmsg);
            eval("javascript:remote_override_result(result);")
        } 
        IRSWorld.TranClass.rem_modal_denomination.check_override_stat(ro.overrideid(),ro.check_status_callback)              
    }
    
    this.check_status_callback = function(result)
    {
        
        setText("lblstatus","WAITING FOR APPROVAL");
        // 1 : verifiying 2: approved 3: cancelled        
        var sres = result.value.toString();
        var arr_res;
        var stringval;
        try
        {
            arr_res = sres.split("~");
        }
        catch(e)
        {
            result = new Array("2","ERROR IN RESULT","");
            ro.toggle_override("none");
            hidePanel("divROWaiting");            
            return;
        }
        
        var nisok  = parseInt(arr_res[0]);
        
        if ( nisok == 1)
        {
            ro.toggle_override("block");
            window.setTimeout("ro.check_status()",5000);            
            return;
        }        
        else
        {   
            result = new Array(arr_res[0].toString(),arr_res[1].toString(),arr_res[2].toString(), arr_res[3].toString());            
            
            ro.toggle_override("none");
            hidePanel("divROWaiting");            
            
            if  (nisok == 5)
            {                
                document.getElementById("txtsessid").value = null;    
                ropopups.close_modal_window();
                //alert("REMOTE TRANSACTION CANCELLED");          
            }
            else
            {
                ropopups.close_modal_window();
                eval("javascript:remote_override_result(result);")            
                
            }
            
        } 
    }
    this.click_Override = function()
    {    
        var tranxType = window.location.pathname;
        tranxType = tranxType.substring(tranxType.lastIndexOf('/') + 1);
        tranxType = tranxType.split('.');
        
        hidePanel("divROButtons");
            showPanel("divROWaiting");
            
            setText("lblstatus","TAGGING STATUS");
            AjaxPro.timeoutPeriod = 100 * 1000;
            AjaxPro.onTimeout = function()
            {                    
                var errmsg = 'SERVER NOT RESPONDING. TAGGING OF STATUS FAILED.'
                alert(errmsg);             
                var result = new Array("99","TIMEOUT",errmsg);
                eval("javascript:remote_override_result(result);")
            } 
        
        if(tranxType[0] == "OBPSTransactions")
        {
            IRSWorld.TranClass.rem_modal_denomination.tag_override_stat("RAPID REMIT-CREDIT TO ACCOUNT",ro.getProductChargesID(), ro.get_splmsg() ,"OBPS;" + forOBPS(),ro.getOverrideType(), ro.set_override_details(override_rows) ,ro.override_callback);               
        }
        else if(tranxType[0] == "RemOTC")
        {
            IRSWorld.TranClass.rem_modal_denomination.tag_override_stat(ro.getTranDesc(),ro.getProductChargesID(), ro.get_splmsg() ,escape(ro.get_details()),ro.getOverrideType(), ro.set_override_details(override_rows) ,ro.override_callback);               
        }     
    }
    this.click_CancelOverride = function()
    {   
        ropopups.close_modal_window();
    }
    this.override_callback = function(results)    
    {
        setText("lblstatus","TAGGING DONE");
        try
        {
            var res = results.value;
            
            var sres = res.split(";");
            if (sres[0] == 1) 
            {   
                document.getElementById("txtsessid").value = sres[1];
                ro.check_status();
                return;
            }
            else
                {spawn_errpage("SESSION EXPIRED");}
        }        
        catch(e)
        {
            spawn_errpage("SESSION EXPIRED");
        }
    }
    this.toggle_override = function(toggle)
    {
        try
        {
            document.getElementById("divROWaiting").style.display = toggle;
        }
        catch(e) {}
    }
    this.toggle_divs = function(objid,isvisible)
    {
        if (isvisible)
        {
            document.getElementById(objid).style.display = "block";
        }
        else
        {
            document.getElementById(objid).style.display = "none";
        }        
    }
    this.get_details = function()
    {   
        var otc_values1 = ro.get_otc_table1_details();        
        var otc_values2 = ro.get_otc_table2_details();        
        var otc_values3 = ro.get_otc_table3_details();        
        return otc_values1 + "^" + otc_values2 + "^" + otc_values3 + "^";
    }    
    this.get_compute_mode = function()
    {
        var cbo = document.getElementById("ddl_compute_mode");
        
        try
        {
           return cbo[cbo.selectedIndex].innerText;
        }
        catch(e)
        {
            return cbo.innerText;
        }
        
    }    
    this.get_otc_table1_details = function()
    {
        var otc_values = "";
        otc_values += ro.get_compute_mode() + "~";    
        otc_values += ro.get_amount() + "~";    
        otc_values += getText("txt_messages") + "~";    
        otc_values += getText("lbl_payment_type") + "~";    
        otc_values += getText("lbl_remitting_currency") + "~";            
        otc_values += getText("lbl_charge_type") + "~";    
        otc_values += getText("lbl_transaction_charge") + "~";    
        otc_values += getText("lbl_amount_to_be_paid") + "~";    
        otc_values += getText("lbl_beneficiary_amount") + "~";    
        otc_values += getText("lbl_peso_equivalent") + "~";    
        otc_values += getText("lbl_transaction_date") + "~";    
        otc_values += getText("lbl_transaction_status") + "~";    
        otc_values += getText("lbl_branch_name") + "~";    
        otc_values += getText("lbl_account_currency") + "~";    
        otc_values += getText("lbl_account_number") + "~";    
        otc_values += getText("lbl_product_type") + "~";    
        otc_values += getText("lbl_implementation_type") + "~";            
        otc_values += getText("lbl_receipt_mode") + "~";    
        otc_values += getText("lbl_purpose") + "~";    
        otc_values += getText("lbl_transaction_source") + "~";    
        otc_values += getText("lbl_source_of_fund") + "~";    
        
        return otc_values;
    }
    this.get_otc_table2_details = function()
    {
        var otc_values = "";
        otc_values += getText("lbl_remitter_id") + "~";    
        otc_values += getText("lbl_remitter_name") + "~";    
        otc_values += getText("lbl_remitter_address") + "~";                    
        return otc_values;
    }
    this.get_otc_table3_details = function()
    {
        var otc_values = "";
        otc_values += getText("lbl_beneficiary_id") + "~";    
        otc_values += getText("lbl_beneficiary_name") + "~";    
        otc_values += getText("lbl_beneficiary_address") + "~";                    
        return otc_values;
    }
    this.get_splmsg = function()
    {   
        return document.getElementById("txtsplmsg").value;
    }
    this.cancel_click = function()
    {
        //5 is cancelled                
        var overrideid = document.getElementById("txtsessid").value;
        setText("lblstatus","CANCELLING STATUS");
        AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. CANCELLING OF STATUS FAILED.'
            alert(errmsg);             
            var result = new Array("99","TIMEOUT",errmsg);
            eval("javascript:remote_override_result(result);")
        } 
        IRSWorld.TranClass.rem_modal_denomination.cancel_override_stat(overrideid,ro.cancel_click_callback);     
    }    
    this.cancel_click_callback = function(result)
    {
        try
        {
            if (isNaN(result.value))
            {
                var errmsg = "THERE WAS AN ERROR WHILE CANCELLING THE TRANSACTION "                
                var result = new Array("99","TIMEOUT",errmsg);
                setText("lblstatus","ERROR WHILE CANCELLING");
                eval("javascript:remote_override_result(result);")
            }
            else
            {
                setText("lblstatus","CANCELLING DONE");
                document.getElementById("btnWACancel").style.display = 'none';
                window.setTimeout("ro.check_status()",5000);
            }
        }
        catch(e)
        {
            
        }
    }
    this.imposeMaxLength = function(Object, MaxLen)
    {
        return (Object.value.length <= MaxLen);
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
    
        //FOR OVERRIDE
    
    this.set_override_details = function(irs_calculator_data_length)
    {
        override_rows = irs_calculator_data_length;
        if(override_type == 2)
        {
            override_list = document.getElementById("txt_remitting_to_account_currency_hidden").value + "~" + document.getElementById("txt_currency_rate_remitting_to_account").value +"~";
            override_list += document.getElementById("txt_account_to_remitting_currency_hidden").value + "~" + document.getElementById("txt_currency_rate_account_to_remitting").value +"~";
        
//            if (irs_calculator_data_length == null || irs_calculator_data_length < 1 || irs_calculator_data_length == undefined)
//            {
//                irs_calculator_data_length = 0;
//            }
            
            if (irs_calculator_data_length > 0)
            {
                if (irs_calculator_data_length != null)
                {        
                    for(var i = 0; i < irs_calculator_data_length; i++)
                    {
                        override_list += document.getElementById("txt_charge_desc_hidden_" + (i + 1)).value+"~"; 
                        override_list += document.getElementById("txt_currency_desc_hidden_" + (i + 1)).value+"~"; 
                        override_list += document.getElementById("txt_charges_value_" + (i + 1)).value+"~";
                    }
                    override_list += irs_calculator_data_length;
                }  
            }
        }
        return override_list;     
    }
    
}
function RO_popups()
{
    var cls = this;
    
    this.test_nsbrow = function()
    {
        return (document.all) ? false : true;
    }    
    this.construct_ro_objects = function()
    {
        var divpopup = document.getElementById("divROButtons");
        
        if (divpopup == null)
        {
            this.create_popup();
        }        
    } 
    this.hide_obj = function(opt,objid)             
    {
        if (opt == 1 ) {hidePanel(objid)}
        else {showPanel(objid)}
    }
    this.romsg_popup_html = function()
    {               
        var shtml = "";
        shtml = '<div id="divROWaiting" style="display:none;width:400px;height:auto;background-color:transparent;">'
        shtml +=  '<table border="0" style="border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid;width:400px;" cellpadding="0" cellspacing="1">';
        shtml += '<tr>';
        shtml += '<td style="text-align: right; height: 20px; " >';        
        shtml += '&nbsp;<input type="hidden" id="txtsessid"> </td>';
        shtml += '</tr>';
        shtml += '<tr>';
        shtml += '<td style="text-align: center;height: 30px;" >';
        shtml += '<img src = ".././images/ajaxloader.gif"></img></td>';
        shtml += '</tr>';
        shtml += '<tr>';
        shtml += '<td style="text-align: center; height: 30px;" >';
        shtml += '<label id="lblstatus" class="cell_desc">WAITING FOR APPROVAL</label></td>';
        shtml += '</tr>';
        shtml += '<tr>';
        shtml += '<td style="text-align: center;  height: 31px;" >';
        shtml += '<input id="btnWACancel" style="width: 18%; height: 24px" type="button" value="CANCEL"  onclick="javascript:ro.cancel_click();"/></td>'
        shtml += '</tr></table>';
        shtml += '</div>';
        return shtml;
    }        
    this.romsg_prompt_html = function()
    {                   
        var  shtml = "";
        shtml = '<div id="divROButtons" style="display:block;width:400px;height:auto;background-color:transparent;">'
        shtml += '<table border="0" cellpadding="0" cellspacing="0" style="border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid;width:400px;height:auto" cellpadding="0" cellspacing="1">';
        shtml += '<tr class="table_caption">';
        shtml += '    <td  colspan="2" style="height: 31px; text-align: left">REMOTE OVERRIDE PROMPT</td>';
        shtml += '</tr>';
        shtml += '<tr>';
        shtml += '    <td class="cell_caption" style="height: 31px; text-align: center; width: 125px;">Message</td>';
        shtml += '    <td class="cell_caption"  style="height: 31px; text-align: center">';
        shtml += '    <textarea id="txtsplmsg" style="text-align:left; width: 264px;" rows="2" onkeypress="javascript:ro.imposeMaxLength(this, 15);">';
        shtml += '';
        shtml += '    </textarea> </td>';
        shtml += '</tr>';
        shtml += '<tr>';
        shtml += '    <td colspan="2" style="text-align:center; height: 33px;" class="cell_desc" >';
        shtml += '    <input id="btnROCommit" type="button" value="COMMIT" onclick="javascript:ro.click_Override();" style="width: 18%; height: 24px"/>';
        shtml += '    <input id="hidPCD" type="hidden" value="" />';
        shtml += '    <input id="hidOT" type="hidden" value="" />';
        shtml += '    <input id="btnROCancel" type="button" value="CANCEL" onclick="javascript:ro.click_CancelOverride();" style="width: 18%; height: 24px" /></td>';
        shtml += '</tr>';
        shtml += '</table>';
        shtml += '</div>';
        return shtml;
    }
    this.open_modal_window = function()
    {
        this.load_modal_css();
    
        this.outer_modal_div           = document.createElement("div");
        this.outer_modal_div.id        = "outer_mod_div_ro";
        this.outer_modal_div.className = "outer_div";
    
        document.body.style.margin = "0px";
        document.body.appendChild(this.outer_modal_div);
        document.getElementById("outer_mod_div_ro").style.height = screen.height;
    
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
            this.inner_modal_div.id           = "inner_mod_div_ro";
            this.inner_modal_div.style.height = "auto";		
            this.inner_modal_div.style.width  = "400px";
            this.inner_modal_div.style.top    = "30px";
            this.inner_modal_div.style.left   = (parseInt(screen.width) - parseInt(this.inner_modal_div.style.width)) / 2 - 10 + "px";
            this.inner_modal_div.style.border = "medium solid white";

            content_div                       = document.createElement("div");
            content_div.id                    = "content_mod_div_ro";
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
                if (divelement_outer.item(i).id == "outer_mod_div_ro")
                {
                    divelement_outer.item(i).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop;break;
                }	        
            }
            for(m = 0; m < divelement_inner.length;m++)
            {
                if (divelement_inner.item(m).id == "inner_mod_div_ro")
                {
                    divelement_outer.item(m).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop + 30 : document.body.scrollTop + 30;break;
                }
            }
    }
    this.create_popup = function()
    {
        this.open_modal_window();

        document.getElementById("content_mod_div_ro").innerHTML = this.romsg_popup_html();
        document.getElementById("content_mod_div_ro").innerHTML += this.romsg_prompt_html();
        
    }
    
}
/** hide panel*/
function showPanel(divname)
{   
    var div = document.getElementById(divname);                  
    if (div != null)
    div.style.display = "block";
}
function hidePanel(divname)
{
    var div = document.getElementById(divname);                  
    if (div != null)
    div.style.display = "none";     
}
/** end of hide panel ***/

function forOBPS()
{
    var obpsDetails = getText("lblfxRate");
    obpsDetails += "~" + getText("txtTargetAmount");
    
    var ddlOBPSsrcCur = document.getElementById("ddlSourceCurrency");
    obpsDetails += "~" + ddlOBPSsrcCur.options[ddlOBPSsrcCur.selectedIndex].text; 
    obpsDetails += "~" + getText("spnTotalCharges"); //Total Charge (RMT-CCY)
    obpsDetails += "~" + getText("spnNetAmount"); //Total Amount (RMT-CCY)
    obpsDetails += "~" + getText("spnBenefAmount"); //Amount to be received (Merchant)
    
    //Remitter Info
    obpsDetails += "~" + getText("lbl_rem_id"); //Remitter ID
    obpsDetails += "~" + getText("lbl_rem_fullname"); //Complete Name
    obpsDetails += "~" + getText("lbl_rem_address"); //Address
    obpsDetails += "~" + getText("lbl_rem_telno"); //Telephone No
    
    //Merchant Info
    obpsDetails += "~" + getText("lbl_merchant_name"); //Merchant Name
    obpsDetails += "~" + getText("lbl_MID");
    obpsDetails += "~" + getText("txtMemberID");
    obpsDetails += "~" + getText("lbl_merchant_currency");
    obpsDetails += "~" + getText("lbl_merchant_prodcode");
    
    var datatbl = IRSWorld.TranClass.rem_modal_denomination.getEnrollMerchantInfo(getText("lbl_rem_id"),getText("lbl_merchant_name"),getText("lbl_merchant_prodcode")).value;
        
    if (datatbl != null && datatbl.Rows.length != 0)
    {
        this.MerchantCode = datatbl.Rows[0].MerchantCode.toString();
        this.MerProdCode = datatbl.Rows[0].MerchantProductCode.toString();
    }    
   
    var sp = 'spGetOptionalField';     
    var dt = IRSWorld.TranClass.rem_modal_denomination.getOBPData(sp,this.MerchantCode,this.MerProdCode).value;
    
    for (var cnt = 0; cnt < dt.Rows.length; cnt++)
    {                
            var el = document.createElement('input')
            el.type = 'text';
            el.name = dt.Rows[cnt].ControlName.toString();
            el.id = dt.Rows[cnt].FieldName.toString();
            el.value = dt.Rows[cnt].FieldLabel.toString();
            
            if(el.id != "id1")
            {
                if(dt.Rows[cnt].ValidationType.toString() == "LI")
                {
                    var el = document.createElement('select')
                    el.name = dt.Rows[cnt].ControlName.toString();
                    el.id = dt.Rows[cnt].FieldName.toString();
                    
                    var ddlOBPS = document.getElementById(el.id);
                    obpsDetails += "~" + ddlOBPS.options[ddlOBPS.selectedIndex].text; 
                }
                else if(!el.value.match(/Account.*/))
                {
                    var colName = el.id;
                    
                    obpsDetails += "~" + getText(colName);
                }
            }
    }
    
    return obpsDetails;
}       