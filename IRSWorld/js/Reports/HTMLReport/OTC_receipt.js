//Modified by: Ruthy

var OTCRep = new OTCReport();
var reportholder = new OTCReport();

function OTCReport()
{
    this.load_report = function()
    {
       var qstr = HTMLRep.get_querystring();
        
        if (qstr.length > 0)
        {
            if (qstr.toLowerCase().indexOf("trantype=otcdaily" ) > -1 || qstr.toLowerCase().indexOf("mergeparam") > -1 && qstr.toLowerCase().indexOf("sp_daily_record_keep_log_ref") > -1)
            {
                var orps = new HTMLDailyRecordReport();
                orps.evaluate_DailyRecord_query(qstr);
                return;
            }      
            if ( (qstr.toLowerCase().indexOf("trantype=otc" ) > -1  && qstr.toLowerCase().indexOf("obp") == -1 ) || qstr.toLowerCase().indexOf("mergeparam") > -1 && qstr.toLowerCase().indexOf("otc") > -1 &&  qstr.toLowerCase().indexOf("obp") == -1 )
            {
                 var isdotmatrix = false;
                 receipttype = this.getreceipttype();
                try
                {
                    var sp_name = 'proc_check_islaser_receipt';
                    var sbranchcode =  ""
                    var squery = qstr;
                    if (squery.toLowerCase().indexOf("trantype=otc") > -1)
                        sbranchcode =  HTMLRep.get_branchcode();
                    if (squery.toLowerCase().indexOf("mergeparam") > -1)
                        sbranchcode = HTMLRep.parse_merge_param_query(squery,"T@v_branchcode");
                    
                    var params  =  '@v_branhcode=' + sbranchcode;                    
                    AjaxPro.timeoutPeriod = 100 * 1000;
                    AjaxPro.onTimeout = function()
                    {                    
                        var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
                        alert(errmsg);     return;        
                    } 
                    
                    var dsisDotMatrix = IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params).value;

                    try
                    {
                        if (dsisDotMatrix.Tables[0].Rows[0]["LaserPrintORNumber"].toString() == "0")
                        {
                            isdotmatrix = true;
                        }
                        else
                        {
                            isdotmatrix = false;
                        }
                        
                    }
                    catch(e)
                    {
                        isdotmatrix = false;
                    }
                        
                }
                catch(e)
                {
                }
                
                //
                if ( (isdotmatrix == false) && (receipttype.Tables[0].Rows[0]["ReceiptType"].toString() == "5") && ( (sbranchcode == "730") || (sbranchcode == "732") || (sbranchcode == "733") || (sbranchcode == "735") || (sbranchcode == "736") || (sbranchcode == "737") || (sbranchcode == "739")))
                {
                    var orps = new HTMLReceiptOTC_HK_OnePage();
                    orps.evaluate_Receipt_query(qstr);
                    return;
                }
                
               else if ( (isdotmatrix == false) && (receipttype.Tables[0].Rows[0]["ReceiptType"].toString() == "5"))
               {
                    var orps = new HTMLReceiptOTC_OnePage();
                    orps.evaluate_Receipt_query(qstr);
                    return;
               }
                //
                
                if ( (isdotmatrix == false) && ( (sbranchcode == "730") || (sbranchcode == "732") || (sbranchcode == "733") || (sbranchcode == "735") || (sbranchcode == "736") || (sbranchcode == "737") || (sbranchcode == "739") ) )
                {
                     var orps = new HTMLReceiptOTC_HONGKONG();
                    orps.evaluate_Receipt_query(qstr);
                    return;
                }
               else if (isdotmatrix == false)
                {
                    var orps = new HTMLReceiptReport();
                    orps.evaluate_Receipt_query(qstr);
                    return;
                }
                else
                {
                    var orps = new HTMLPrePrintedReceiptReport();
                    orps.evaluate_PrePrintedReceipt_query(qstr);
                    return;
                }
            }  
            
            //for OBP RECEIPT
            if ( (qstr.toLowerCase().indexOf("trantype=obp" ) > -1  ) || qstr.toLowerCase().indexOf("mergeparam") > -1 && qstr.toLowerCase().indexOf("obp") > -1 )
            {
                 var isdotmatrix = false;
                 receipttype = this.getreceipttype();
                try
                {
                    var sp_name = 'proc_check_islaser_receipt';
                    var sbranchcode =  ""
                    var squery = qstr;
                    if (squery.toLowerCase().indexOf("trantype=obp") > -1)
                        sbranchcode =  HTMLRep.get_branchcode();
                    if (squery.toLowerCase().indexOf("mergeparam") > -1)
                        sbranchcode = HTMLRep.parse_merge_param_query(squery,"T@v_branchcode");
                    
                    var params  =  '@v_branhcode=' + sbranchcode;                    
                    AjaxPro.timeoutPeriod = 100 * 1000;
                    AjaxPro.onTimeout = function()
                    {                    
                        var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
                        alert(errmsg);     return;        
                    } 
                    
                    var dsisDotMatrix = IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params).value;
                    
                    try
                    {
                        if (dsisDotMatrix.Tables[0].Rows[0]["LaserPrintORNumber"].toString() == "0")
                        {
                            isdotmatrix = true;
                        }
                        else
                        {
                            isdotmatrix = false;
                        }
                    }
                    catch(e)
                    {
                        isdotmatrix = false;
                    }
                    
                }
                catch(e)
                {
                }
                 //
                if ( (isdotmatrix == false) && (receipttype.Tables[0].Rows[0]["ReceiptType"].toString() == "5") && ( (sbranchcode == "730") || (sbranchcode == "732") || (sbranchcode == "733") || (sbranchcode == "735") || (sbranchcode == "736") || (sbranchcode == "737") || (sbranchcode == "739")))
                {
                    var orps = new HTMLReceiptOBP_HK_OnePage();
                    orps.evaluate_Receipt_query(qstr);
                    return;
                }
                
               else if ( (isdotmatrix == false) && (receipttype.Tables[0].Rows[0]["ReceiptType"].toString() == "5"))
               {
                    var orps = new HTMLReceiptOBP_OnePage();
                    orps.evaluate_Receipt_query(qstr);
                    return;
               }
                //
                if ( (isdotmatrix == false) && ( (sbranchcode == "730") || (sbranchcode == "732") || (sbranchcode == "733") || (sbranchcode == "735") || (sbranchcode == "736") || (sbranchcode == "737") || (sbranchcode == "739") ) )
                {
                    var orps = new HTMLOBPReceipt_HONGKONG();
                    orps.evaluate_Receipt_query(qstr);
                    return;
                }
                else if (isdotmatrix == false)
                {
                    var orps = new HTMLOBPReceiptReport();
                    orps.evaluate_Receipt_query(qstr);
                    return;
                }
                else
                {
                    var orps = new HTMLPrePrintedOBPReceiptReport();
                    orps.evaluate_PrePrintedReceipt_query(qstr);
                    return;
                }
            }  
	    if (qstr.toLowerCase().indexOf("sp_rptauditlogreport" ) > -1 )
            {
                var orps = new HTMLAuditLogReport();
                orps.evaluate_auditlog_query(qstr);
                return;
            }   
        }
    }    
    
    //ruthy
    this.getreceipttype = function()
    {
    
	    var sp_name = 'proc_check_tagalog_report';
        var squery = HTMLRep.get_hidden_querystring("hidQueryString");
        var sbranchcode = HTMLRep.get_branchcode();
        
        var params  =  '@v_branchcode=' + sbranchcode;
        AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
            alert(errmsg);     return;        
        } 
        
        var receipt_val = IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params).value;
	
	return receipt_val; 
    }
    
    //ruthy
    
    
    this.get_branchcode = function()
    {
        return this.get_hidden_querystring("hidBranchcode");
    }
    this.get_hidden_querystring = function(sID)
    {
        
        var s = "";
        for(i = 0; i < document.forms[0].length; i++)
        {
            try
            {
                if (document.forms[0].elements[i].type == "hidden")
                {
                    if (document.forms[0].elements[i].id == sID)
                    {
                        s = document.forms[0].elements[i].value;
                    }
                }
            }
            catch(e)
            {
                alert(e);
            }
        }
        return s
    }
    this.get_querystring_value  = function(query,sfield)
    {
        var squery = query.toString();
        var stemp = squery.split("&");
        var itemp;
        try
        {
            for(i = 0 ; i < stemp.length; i++)
            {
                itemp = stemp[i].toString();
                if (itemp.indexOf(sfield) > -1)
                {
                    var jtemp = itemp.split("=");
                    return jtemp[1];  
                }
            }
            return "";
        }
        catch(e)
        {
            alert(e);
            return e;
        }
    }
    this.parse_merge_param_query = function(mquery,qfield)
    {
        var squery = unescape(mquery.toString());
        var stemp = squery.split("&");
        var itemp;
        var jtemp;
        var htemp;
        try
        {
            for(i = 0; i < stemp.length; i++ )
            {
                itemp = stemp[i].toString();
                if (itemp.indexOf("paramvalues") > -1)
                {
                    jtemp = itemp.split("^");
                    for(j = 0 ; j < jtemp.length; j++)
                    {
                        if (jtemp[j].toLowerCase().indexOf(qfield.toLowerCase()) > -1)
                        {
                            htemp = jtemp[j].split(";");
                            return htemp[1];
                        }
                    }
                }
            }
            return "";
        }
        catch(e)
        {
            alert("VALUE NOT FOUND IN QUERY");
            return "";
        }
    }
this.general_styles = function()
    {
        var shtml = "";
        shtml  += '<style type="text/css">';
        shtml  += 'p{ font-weight:bold; font-size: 11.5px; font-family: Arial;}';
        shtml  += '.paper_template';
        shtml  += '{';
        shtml  += '}';
        shtml  += '.border';
        shtml  += '{';
        shtml  += 'background-color: white;';
        shtml  += '}';
        shtml  += '.textbox';
        shtml  += '{';
        shtml  += 'border: none;';
        shtml  += 'background-color: white;';
        shtml  += 'width: 300px;';
        shtml  += '}';
        shtml  += '.subdiv';
        shtml  += '{';
        shtml  += 'text-align:center;';
        shtml  += 'font-family: Arial;';
        shtml  += 'font-size: 12px;';
        shtml  += 'border: none;';
        shtml  += 'background-color: white;';
        shtml  += 'border-right: black thin solid; ';
        shtml  += 'border-top: black thin solid; ';
        shtml  += 'border-left: black thin solid; ';
        shtml  += 'border-bottom: black thin solid;';
        shtml  += '}';
        shtml  += 'body';
        shtml  += '{';
        shtml  += 'text-align:center;';
        shtml  += 'font-family: Arial;';
        shtml  += 'font-size: 10.5px;';
        shtml  += '}';
        shtml  += '</style>';
        return shtml;
    }      
    this.formatTime = function(sdatevalue)
    {
        var currentTime = new Date(sdatevalue);
        var hours = currentTime.getHours();
        var minutes = currentTime.getMinutes();

        var suffix = "AM";
        if (hours >= 12) {
            suffix = "PM";
            hours = hours - 12;
        }
        if (hours == 0) {
            hours = 12;
        }

        if (minutes < 10)
          minutes = "0" + minutes;

        return hours + ":" + minutes + " " + suffix ;
    }
    this.formatDate = function(sdatevalue,divider)
    {
        var d = new Date(sdatevalue);
        var curr_date = d.getDate();
        var curr_month = d.getMonth();
        curr_month++;
        var curr_year = d.getFullYear();
        return (curr_month + divider + curr_date + divider + curr_year);
    }
}




/* START RECEIPT REPORT */
function HTMLReceiptReport()
{
    var cls = this;
    var _branchcode;    
    var _seqid;
	var _referenceno;
    
    this.evaluate_Receipt_query = function(query)
    {
        var squery = unescape(query.toString());
        if (squery.toLowerCase().indexOf("trantype=otc") > -1)
        {
            _branchcode = HTMLRep.get_branchcode();
            this.get_Receipt_data(
                                    HTMLRep.get_querystring_value(query,"tranvalue"),
                                    HTMLRep.get_branchcode(),
                                    ""
                                 )
            return;
        }
        if (squery.toLowerCase().indexOf("mergeparam") > -1)
        {
             _branchcode = HTMLRep.parse_merge_param_query(squery,"T@v_branchcode")
             this.get_Receipt_data(
                                    "",
                                    HTMLRep.parse_merge_param_query(squery,"T@v_branchcode"),
                                    HTMLRep.parse_merge_param_query(squery,"T@v_referenceno")
									)
            return;
        }
    }
    this.check_newdbswitch = function()
    {
    
	var pass = false;
	var sp_name = 'proc_check_newdbswitch';
        var squery = HTMLRep.get_hidden_querystring("hidQueryString");
        var sbranchcode = HTMLRep.get_branchcode();
        
        var params  =  '@branchcode=' + sbranchcode;
        var isRCC  = false;
        AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
            alert(errmsg);     return;        
        } 
        
        var dtcchecksw = IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params).value;
        
        try
        {
            if (dtcchecksw.Tables[0].Rows.length > 0)
            {
                pass = true;
            }
        }
        catch(e)
        {
        }
	
	return pass; 
    }
    this.get_Receipt_data = function(seq_id,branchcode,referenceno)
    {
        var params; 
        var sp_name = "";
	var pass = false;
        
                _branchcode =   " @v_branchcode="  + branchcode;    
                _seqid    =   "@v_seqid="     + seq_id;
				_referenceno = " @v_referenceno="     + referenceno;
        
       if(seq_id.length == 0)
	   {
			pass = this.check_newdbswitch();
			if (pass)
			{
			    sp_name = "SP_OTC_Reprint_Receipt_irsw";
			}
			else
			{
			    sp_name = "SP_OTC_Reprint_Receipt";
			}
			params  = _branchcode + "^" + _referenceno ;
	   }else{
			sp_name = "SP_OTC_Receipt";
			params  = _branchcode + "^" + _seqid ;
		}
		AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
            alert(errmsg);       return;      
        } 
	    IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params,this.get_Receipt_data_callback);
    }
    
    this.get_Receipt_data_callback = function(result)
    {
        try
        {
            var ds = result.value;
            
            cls.check_branch();
            cls.display_Receipt_report(ds);
            
            
        }
        catch(e)
        {
            alert("ERROR WHILE GETTING Receipt DATA");
        }
    }
    
    this.display_Receipt_report = function(ds)
    {
        var sp_name = 'proc_check_reporthalfpage';
        var squery = HTMLRep.get_hidden_querystring("hidQueryString");
        var sbranchcode = ds.Tables[0].Rows[0].ReferenceNo;
        sbranchcode =  sbranchcode.substring(1,4);
        var params  =  '@v_branhcode=' + sbranchcode;
        var ishalfpage  = false;
        AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
            alert(errmsg);     return;        
        } 
        
        var dtcheckhalfpage = IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params).value;
        
        try
        {
            if (dtcheckhalfpage.Tables[0].Rows.length > 0)
            {
                ishalfpage = true;
            }
        }
        catch(e)
        {
        }
    
        
        var shtml = "";        
        shtml +=  HTMLRep.print_html_button();
        var isRCC = cls.check_branch();
        if (!ishalfpage)
        {
           
            shtml +=  this.Receipt_body(ds.Tables[0],isRCC);
               
            shtml += cls.half_page(ds.Tables[0]);
            
	        shtml +=  '</div>';
        }
        else
        {
           shtml +=  this.Receipt_body_halfpage(ds.Tables[0],isRCC);
           
        }
        document.getElementById("bodydiv").innerHTML = shtml
    }
    this.check_branch = function()
    {
        var sp_name = 'sp_report_checkbranch';
        var squery = HTMLRep.get_hidden_querystring("hidQueryString");
        var sbranchcode = HTMLRep.get_branchcode();
        
        var params  =  '@branchcode=' + sbranchcode;
        var isRCC  = false;
        AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
            alert(errmsg);     return;        
        } 
        
        var dtccheckbranch = IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params).value;
        
        try
        {
            if (dtccheckbranch.Tables[0].Rows.length > 0)
            {
                isRCC = true;
            }
        }
        catch(e)
        {
        }
        return isRCC;
    }
    this.Receipt_body = function(dt,isRCC)
    {
	
	    var vd = window.location.href.split('/')[3];
	    
        var shtml = "";
	        shtml +=  '<div>';
            shtml +=  '<table style="width: 8.5in; font-size: 8pt; font-family: arial;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="width: 0in">';
            shtml +=  '<table style="width: 100%;font-size: 10pt;">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 60px; height: 15px">';
            shtml +=  '<img src="../../../'+ vd +'/images/pnb_black_logo.JPG" style="width: 58px; height: 68px" /></td>';
            shtml +=  '<td style="vertical-align: top; width: 520px; height: 15px">';
            shtml +=  '<strong>' + dt.Rows[0].Header1 + '</strong><br />';
            shtml +=  '' + dt.Rows[0].Header2 + '<br />';
            shtml +=  '<strong>' + dt.Rows[0].NameofBranch + '</strong><br />';
            shtml +=  '' + dt.Rows[0].AdressofBranch + '<br />';
            shtml +=  '' + dt.Rows[0].BranchofContact + '<br />';
            shtml +=  '' + dt.Rows[0].BranchofEmail + '<br />';
            shtml +=  '' + dt.Rows[0].corp_address + '<br />';
            shtml +=  '' + dt.Rows[0].corp_telno + '</td>';
            shtml +=  '<td style="vertical-align: top; height: 15px">';
            shtml +=  'Transaction Date: ' + HTMLRep.formatDate(dt.Rows[0].TranxDate,"/") + '<br />';
	        if (dt.Rows[0].ClearingDate == '1900')
	        {
	        shtml +=  'Clearing Date: <br />';
	        }else{
	        shtml +=  'Clearing Date: '+ dt.Rows[0].ClearingDate +'<br />';
	        }
            shtml +=  'Time: '+ HTMLRep.formatTime(dt.Rows[0].TranxDate) +'<br />';
            shtml +=  'Loyalty Points:';
            shtml +=  '<br />';
            shtml +=  '' + dt.Rows[0].TranStatus + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 8.5in; font-size: 10pt; font-family: arial;';
            shtml +=  'height: 0.23in; border-top-width: thin; border-left-width: thin; border-left-color: black; border-bottom-width: thin; border-bottom-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="border-right: black thin solid; border-top: black thin solid;';
            shtml +=  'border-left: black thin solid; border-bottom: black thin solid">';
            shtml +=  '<table style="font-size: 10pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 244px; height: 14px">';
            shtml +=  '<strong>Remitter ID No.: </strong>' + dt.Rows[0].RemitterID + '</td>';
            shtml +=  '<td style="width: 285px; height: 14px">';
            shtml +=  '<strong>Official Receipt No.:</strong> ' + dt.Rows[0].ReceiptNo + '</td>';
            shtml +=  '<td style="width: 257px; height: 14px">';
            shtml +=  '<strong>Reference No.:</strong> ' + dt.Rows[0].ReferenceNo + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';    
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td style="border-right: black thin solid; border-top: black thin solid; vertical-align: top;';
            shtml +=  'border-left: black thin solid; width: 50%; border-bottom: black thin solid; height: 20px">';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 0px; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="vertical-align: top; width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 20px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 10pt; text-decoration: underline">REMITTER INFO</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="2">';
            shtml +=  '<span><span style="font-size: 10pt"><strong>&nbsp;Name: </strong>' + dt.Rows[0].RemitterName + '<br />';
            shtml +=  '</span></span>';
            shtml +=  '<table style="font-size: 10pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="width: 61px; vertical-align: top;">';
            shtml +=  '<strong style="width: 30px">Address:<br />';
            shtml +=  '</strong></td>';
            shtml +=  '<td colspan="2" rowspan="3" style="height: 50px; vertical-align: top;">';
            shtml +=  '' + dt.Rows[0].RemitterAddress + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<span><span style="font-size: 10pt; width: 100%"><strong>Telephone Number:</strong>';
            shtml +=  '' + dt.Rows[0].RemitterTelNo + '</span></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="vertical-align: top; width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 20px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 100% ; height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 10pt; text-decoration: underline">BENEFICIARY INFO</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="2">';
            shtml +=  '<span><span style="font-size: 10pt"><strong>&nbsp;Name:</strong> ' + dt.Rows[0].BenefName + '<br />';
            shtml +=  '</span></span>';
            shtml +=  '<table style="font-size: 10pt; width: 100%; vertical-align: top;">';
            shtml +=  '<tr>';
            shtml +=  '<td  style="width: 60px; vertical-align: top;"><strong style="width: 30px">Address:<br />';
            shtml +=  '</strong>';
            shtml +=  '</td>';
            shtml +=  '<td rowspan="3" style="width: 90%; height: 50px; vertical-align: top;">';
            shtml +=  '' + dt.Rows[0].BenefAddress + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<span><span style="font-size: 10pt"><strong>Telephone Number:</strong> ' + dt.Rows[0].BenefTelNo + '</span></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="vertical-align: top;  width: 100%; height: 20px; border-bottom: black thin solid;">';
            shtml +=  '<table style="font-size: 10pt; width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 10pt; text-decoration: underline">TRANSACTION DETAILS</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 10pt">';
            shtml +=  '<td colspan="3" rowspan="2" style="font-size: 10pt">';
            shtml +=  '<table style="font-size: 10pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Beneficiarys Account No. : </strong>' + dt.Rows[0].BenefAcctNo + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Thru PNB Branch: </strong>' + dt.Rows[0].ThruPNBBranch + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Other Banks Name / Branch: </strong>' + dt.Rows[0].OtherBankName + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Trans Type/Products: </strong>' + dt.Rows[0].TransType + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Source of Funds: </strong>' + dt.Rows[0].SourceFunds + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Member ID:</strong>' + dt.Rows[0].MemberID + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Purpose of Remittance: </strong>' + dt.Rows[0].PurposeRemittance + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Remittance Transaction Source: </strong>' + dt.Rows[0].RemittanceTranxSource + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 10pt">';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 10pt">';
            shtml +=  '<td rowspan="1" style="border-top-width: thin; border-left-width: thin; border-left-color: black;';
            shtml +=  'border-bottom-width: thin; border-bottom-color: black; vertical-align: top; width: 100%;';
            shtml +=  'border-top-color: black; height: 20px; border-right-width: thin; border-right-color: black">';
            shtml +=  '<table style="width: 100%; height: 20px;font-size: 10pt;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px;font-size: 10pt;">';
            shtml +=  '<p style="text-align: center">';
            shtml +=  '<span style="font-size: 10pt; text-decoration: underline"><strong>CLIENT/CUSTOMER UNDERTAKING</strong></span></p>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="2" style="text-align: justify; line-height: 18pt">';
            shtml +=  '<table style="width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="3" style="text-align: justify;font-size: 10pt;line-height: 18pt">';
            shtml +=  '<br />';
            shtml +=  'I certify that all the information contained herein is true and correct, and that';
            shtml +=  'I am amenable to the Terms and Conditions (if any) governing this transaction.';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<br />';
            shtml +=  '<br />';
            shtml +=  '<hr style="width: 378px; height: 1px" />';
            shtml +=  '<div style="width: 100%; height: 25px; text-align: center">';
            shtml +=  'Remitter&#039;s Signature</div>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '<td colspan="2" style="border-right: black thin solid; border-top: black thin solid;';
            shtml +=  'vertical-align: top; border-left: black thin solid; width: 50%; border-bottom: black thin solid;';
            shtml +=  'height: 20px">';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 0px; height: 90px">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 86px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="font-size: 10pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 18px; text-align: center">';
            shtml +=  '<span style="font-size: 10pt; text-decoration: underline"><strong>REMITTANCE DETAILS</strong></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td style="text-align: left; width: 245px; height: 15px;">';
	        shtml +=  '    <strong>';
	        shtml +=  'Net Amount Due to Beneficiary:</strong></td>';
	        shtml +=  '<td style="width: 58px; height: 15px">';
	        if (dt.Rows[0].NetAmountDueToBenefCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].NetAmountDueToBenefCurrCode + '</td>';
	        }
	        shtml +=  '<td style="height: 15px; text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].NetAmountDueToBenefAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Exchange Rate Used:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].ExchangeRateUsedCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].ExchangeRateUsedCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].ExchangeRateUsedAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Remittance Amount:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].RemittanceAmountCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].RemittanceAmountCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].RemittanceAmountAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Cable:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].CableCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].CableCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].CableAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Remittance Fee:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].RemittanceFeeCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].RemittanceFeeCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].RemittanceFeeAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Other Charges:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].OtherChargesCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].OtherChargesCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].OtherChargesAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="width: 221px">';
	        shtml +=  '<strong>';
	        shtml +=  'Total amount received from customer:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].TotalAmtRcvdFromCustCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].TotalAmtRcvdFromCustCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].TotalAmtRcvdFromCustAmt,2) + '</td>';
	        shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 70px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;font-size:10pt;">';
	        shtml +=  '' + dt.Rows[0].Check + '<br />';
	        shtml +=  '' + dt.Rows[0].Cash + '<br />';
	        shtml +=  '' + dt.Rows[0].SpecialMessage + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="font-size:10pt;width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 40px; text-align: center; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 177px;font-size:10pt">';
            shtml +=  '<strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Payment ';
            shtml +=  '<br />';
            shtml +=  'Received / Processed By:</strong></td>';
            shtml +=  '<td style="font-size:10pt;height:30px;">';
            shtml +=  '' + dt.Rows[0].ProcessedBy + '<br />';
            shtml +=  '<hr style="width: 200px; height: 2px;background-color:black;" />';
            shtml +=  '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Authorized Personnel&nbsp;</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="border-top-width: thin; border-left-width: thin; border-left-color: black;';
            shtml +=  'border-bottom-width: thin; border-bottom-color: black; vertical-align: top; width: 100%;';
            shtml +=  'border-top-color: black; height: 20px; text-align: center; border-right-width: thin;';
            shtml +=  'border-right-color: black">';
            shtml +=  '<table style="width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px" align="center">';
            shtml +=  '<span style="font-size: 10pt; text-decoration: underline"><strong>DECLARATIONS</strong></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            if (dt.Rows[0].ReceiptDeclarationSub5.toString().length == 0)
            {
            shtml +=  '<td colspan="3" rowspan="2" style="font-size: 8pt;text-align: justify; line-height: 18pt;">';
            }else{
            shtml +=  '<td colspan="3" rowspan="2" style="font-size: 7pt;text-align: justify; line-height: 18pt;">';
            }
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub1 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub2 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub3 + '<br />';   
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub4 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub5 + '<br />';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
         

        return shtml;
    }
    
    this.half_page = function(dt)
    {
        var shtml = "";
        shtml +=  '<table style="width: 8.5in">';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" style="border-right: black thin dotted; border-top: black thin dotted; border-left: black thin dotted; border-bottom: black thin dotted">';
        shtml +=  '<table style="height: 0%">';
        shtml +=  '<tr>';
        shtml +=  '<td style="font-size: 10pt; width: 371px; color: black; font-family: arial; height: 22px;';
        shtml +=  'text-align: center">';
        shtml +=  '<span style="font-size: 8pt; text-decoration: underline"><span style="font-size: 8.5pt;';
        shtml +=  'text-decoration: underline"><strong>IMPORTANT REMINDERS TO CLIENT / CUSTOMER</strong></span></span></td>';
        shtml +=  '<td rowspan="3" style="font-size: 6.5px; vertical-align: top; width: 50%; color: black;';
        shtml +=  'font-family: arial; height: 101px; text-align: left">';
        shtml +=  '<table style="font-size: 6.5pt; width: 100%; height: 23px">';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" >';
        shtml +=  'To the PNB __________________ , Branch: _____________ Date____________: This is';
        shtml +=  'to authorize your office to change my (pls. mark check the box/es below as applicable),';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" nowrap>';
        shtml +=  '<input id="Checkbox7" style="width: 26px; height: 12px" type="checkbox" />Remitter';
        shtml +=  'Name to _______________________________________________________</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" nowrap>';
        shtml +=  '<input id="Checkbox8" style="width: 26px; height: 12px" type="checkbox" />Address';
        shtml +=  '&nbsp;to _______________________________________ Postal Code: _________</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" nowrap>';
        shtml +=  '<input id="Checkbox9" style="width: 26px; height: 12px" type="checkbox" />Telephone';
        shtml +=  'No to: __________________________ Mobile No to: _________________</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" nowrap>';
        shtml +=  '<input id="Checkbox10" style="width: 26px; height: 12px" type="checkbox" />Beneficiary';
        shtml +=  '&nbsp;Name to: ________________________________Postal Code: _______</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" nowrap>';
        shtml +=  '<input id="Checkbox11" style="width: 26px; height: 12px" type="checkbox" />Beneficiary';
        shtml +=  'Address To: _________________________________________________</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" nowrap>';
        shtml +=  '<input id="Checkbox12" style="width: 26px; height: 12px" type="checkbox" />Beneficiary';
        shtml +=  'Telephone No to: ___________________Mobile No to: ______________</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" nowrap>';
        shtml +=  '<input id="Checkbox13" style="width: 26px; height: 12px" type="checkbox" />Others';
        shtml +=  '(please specify): _________________________________________________</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3">';
        shtml +=  'To support the request for ammendment, photocopy of the following document/s or';
        shtml +=  'ID/s is/are provided (*should match the document/s or ID/s required in No.2 of important';
        shtml +=  'Reminders to Client / Customer):';
        shtml +=  '<br />';
        shtml +=  'A) _______________&nbsp; B_________________ C)_________________ D)_______________</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3">';
        shtml +=  '<table style="width: 100%">';
        shtml +=  '<tr>';
        shtml +=  '<td rowspan="3" style="font-size: 8pt;border-right: black thin solid; border-top: black thin solid;';
        shtml +=  'border-left: black thin solid; border-bottom: black thin solid">';
        shtml +=  'PNB Use Only:<br />';
        shtml +=  'Processed By____Date:___</td>';
        shtml +=  '<td colspan="2" rowspan="3" style="font-size: 8pt;text-align: center">';
        shtml +=  '<hr style="width: 234px; height: 1px" />';
        shtml +=  '&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; Remitter Signature Over Printed Name</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td rowspan="2" style="font-size: 10px; vertical-align: top; width: 371px; color: black;';
        shtml +=  'font-family: arial; height: 113px; text-align: left; line-height: 13pt;">';
        shtml +=  '<span style="font-size: 8pt">1) Please notify the bank of any change in your name,';
        shtml +=  'address, telephone number or other details pertaining to your remittance account';
        shtml +=  '/ record by mailing / submitting the following: a) Photocopy of valid document/s';
        shtml +=  'or ID/s to support the amendment.';
        shtml +=  '<br />';
        shtml +=  'b) Duly accomplished and signed " Authorization and Notice of Amendment" cutout';
        shtml +=  'found on the opposite bottom right corner of this receipt. 2) Mailing address should';
        shtml +=  'tally with the clients registered address shown in the valid document/s or ID/s,';
        shtml +=  'as any irregularity in the registered address may result in non - implementation';
        shtml +=  'of remittance. The valid document/s or ID/s are the following*:';
        shtml +=  '<br />';
        shtml +=  '</span>';
        shtml +=  '<table style="font-size: 8pt; width: 303px; height: 45px">';
        shtml +=  '<tr>';
        shtml +=  '<td style="width: 50%; border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; border-bottom: black thin solid;">';
        shtml +=  'A) ' + dt.Rows[0].ID1 + '</td>';
        shtml +=  '<td style="width: 50%; border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; border-bottom: black thin solid;">';
        shtml +=  'B) ' + dt.Rows[0].ID2 + '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td style="width: 50%; border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; border-bottom: black thin solid;">';
        shtml +=  'C) ' + dt.Rows[0].ID3 + '</td>';
        shtml +=  '<td style="width: 50%; border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; border-bottom: black thin solid;">';
        shtml +=  'D) ' + dt.Rows[0].ID4 + '</td>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '<span style="font-size: 8pt">3) Always provide the bank with clear and proper instructions';
        shtml +=  'as to beneficiary details and remittance amount to facilitate processing of remittance.';
        shtml +=  '</span>';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr style="font-size: 10pt">';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        return shtml;
    }
    this.Receipt_body_halfpage = function(dt,isRCC)
    {
    var vd = window.location.href.split('/')[3];
    
    var shtml = "";
            shtml +=  '<div>';
            shtml +=  '<table style="width: 8.5in; font-size: 8.5pt; font-family: arial;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="width: 0in">';
            shtml +=  '<table style="width: 100%;font-size: 8.5pt">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 60px; height: 15px">';
            shtml +=  '<img src="../../../'+ vd +'/images/pnb_black_logo.JPG" style="width: 58px; height: 68px" /></td>';
            shtml +=  '<td style="vertical-align: top; width: 520px; height: 15px">';
            shtml +=  '<strong>' + dt.Rows[0].Header1 + '</strong><br />';
            shtml +=  '' + dt.Rows[0].Header2 + '<br />';
            shtml +=  '<strong>' + dt.Rows[0].NameofBranch + '</strong><br />';
            shtml +=  '' + dt.Rows[0].AdressofBranch + '<br />';
            shtml +=  '' + dt.Rows[0].BranchofContact + '<br />';
            shtml +=  '' + dt.Rows[0].BranchofEmail + '<br />';
            shtml +=  '' + dt.Rows[0].corp_address + '<br />';
            shtml +=  '' + dt.Rows[0].corp_telno + '</td>';
            shtml +=  '<td style="vertical-align: top; height: 15px">';
            shtml +=  'Transaction Date: ' + HTMLRep.formatDate(dt.Rows[0].TranxDate,"/") + '<br />';
	        if (dt.Rows[0].ClearingDate == '1900')
	        {
	        shtml +=  'Clearing Date: <br />';
	        }else{
	        shtml +=  'Clearing Date: '+ dt.Rows[0].ClearingDate +'<br />';
	        }
            shtml +=  'Time: '+ HTMLRep.formatTime(dt.Rows[0].TranxDate) +'<br />';
            shtml +=  'Loyalty Points:';
            shtml +=  '<br />';
            shtml +=  '' + dt.Rows[0].TranStatus + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 8.5in; font-size: 8pt; font-family: arial;';
            shtml +=  'height: 0.23in; border-top-width: thin; border-left-width: thin; border-left-color: black; border-bottom-width: thin; border-bottom-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="border-right: black thin solid; border-top: black thin solid;';
            shtml +=  'border-left: black thin solid; border-bottom: black thin solid">';
            shtml +=  '<table style="font-size: 8.5pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 244px; height: 14px">';
            shtml +=  '<strong>Remitter ID No.: </strong>' + dt.Rows[0].RemitterID + '</td>';
            shtml +=  '<td style="width: 285px; height: 14px">';
            shtml +=  '<strong>Official Receipt No.:</strong> ' + dt.Rows[0].ReceiptNo + '</td>';
            shtml +=  '<td style="width: 257px; height: 14px">';
            shtml +=  '<strong>Reference No.:</strong> ' + dt.Rows[0].ReferenceNo + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';    
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td style="border-right: black thin solid; border-top: black thin solid; vertical-align: top;';
            shtml +=  'border-left: black thin solid; width: 50%; border-bottom: black thin solid; height: 20px">';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 0px; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="vertical-align: top; width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 20px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 8.5pt; text-decoration: underline">REMITTER INFO</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="2">';
            shtml +=  '<span><span style="font-size: 8.5pt"><strong>&nbsp;Name: </strong>' + dt.Rows[0].RemitterName + '<br />';
            shtml +=  '</span></span>';
            shtml +=  '<table style="font-size: 8.5pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="width: 61px; vertical-align: top;">';
            shtml +=  '<strong style="width: 30px">Address:<br />';
            shtml +=  '</strong></td>';
            shtml +=  '<td colspan="2" rowspan="3" style="height: 50px; vertical-align: top;">';
            shtml +=  '' + dt.Rows[0].RemitterAddress + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<span><span style="font-size: 8.5pt; width: 100%"><strong>Telephone Number:</strong>';
            shtml +=  '' + dt.Rows[0].RemitterTelNo + '</span></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="vertical-align: top; width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 20px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 100% ; height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 10pt; text-decoration: underline">BENEFICIARY INFO</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="2">';
            shtml +=  '<span><span style="font-size: 10pt"><strong>&nbsp;Name:</strong> ' + dt.Rows[0].BenefName + '<br />';
            shtml +=  '</span></span>';
            shtml +=  '<table style="font-size: 10pt; width: 100%; vertical-align: top;">';
            shtml +=  '<tr>';
            shtml +=  '<td  style="width: 60px; vertical-align: top;"><strong style="width: 30px">Address:<br />';
            shtml +=  '</strong>';
            shtml +=  '</td>';
            shtml +=  '<td rowspan="3" style="width: 90%; height: 50px; vertical-align: top;">';
            shtml +=  '' + dt.Rows[0].BenefAddress + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<span><span style="font-size: 8.5pt"><strong>Telephone Number:</strong> ' + dt.Rows[0].BenefTelNo + '</span></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="vertical-align: top;  width: 100%; height: 20px; border-bottom: black thin solid;">';
            shtml +=  '<table style="font-size: 8.5pt; width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 8.5pt; text-decoration: underline">TRANSACTION DETAILS</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 8.5pt">';
            shtml +=  '<td colspan="3" rowspan="2" style="font-size: 8.5pt">';
            shtml +=  '<table style="font-size: 8.5pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Beneficiarys Account No. : </strong>' + dt.Rows[0].BenefAcctNo + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Thru PNB Branch: </strong>' + dt.Rows[0].ThruPNBBranch + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Other Banks Name / Branch: </strong>' + dt.Rows[0].OtherBankName + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Trans Type/Products: </strong>' + dt.Rows[0].TransType + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Source of Funds: </strong>' + dt.Rows[0].SourceFunds + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Member ID:</strong>' + dt.Rows[0].MemberID + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Purpose of Remittance: </strong>' + dt.Rows[0].PurposeRemittance + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Remittance Transaction Source: </strong>' + dt.Rows[0].RemittanceTranxSource + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 8.5pt">';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 8.5pt">';
            shtml +=  '<td rowspan="1" style="border-top-width: thin; border-left-width: thin; border-left-color: black;';
            shtml +=  'border-bottom-width: thin; border-bottom-color: black; vertical-align: top; width: 100%;';
            shtml +=  'border-top-color: black; height: 20px; border-right-width: thin; border-right-color: black">';
            shtml +=  '<table style="width: 100%; height: 20px;font-size: 8.5pt;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px">';
            shtml +=  '<p style="text-align: center">';
            shtml +=  '<span style="font-size: 8.5pt; text-decoration: underline"><strong>CLIENT/CUSTOMER UNDERTAKING</strong></span></p>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="2" style="text-align: justify; line-height: 14pt;">';
            shtml +=  '<table style="width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="3" style="text-align: justify;font-size: 8.5pt;line-height: 14pt;">';                       
            shtml +=  'I certify that all the information contained herein is true and correct, and that ';
            shtml +=  'I am amenable to the Terms and Conditions (if any) governing this transaction.';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<br />';
            shtml +=  '<hr style="width: 378px; height: 1px" />';
            shtml +=  '<div style="width: 100%; height: 25px; text-align: center">';
            shtml +=  'Remitter&#039;s Signature</div>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            // Edited by CHR from colspan=2 to colspan=1
            shtml +=  '<td style="border-right: black thin solid; border-top: black thin solid;';
            shtml +=  'vertical-align: top; border-left: black thin solid; width: 50%; border-bottom: black thin solid;';
            shtml +=  'height: 20px">';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 100%; height: 90px">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 86px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="font-size: 8.5pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 18px; text-align: center">';
            shtml +=  '<span style="font-size: 8.5pt; text-decoration: underline"><strong>REMITTANCE DETAILS</strong></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td style="text-align: left; width: 245px; height: 15px;">';
	        shtml +=  '    <strong>';
	        shtml +=  'Net Amount Due to Beneficiary:</strong></td>';
	        shtml +=  '<td style="width: 58px; height: 15px">';
	        if (dt.Rows[0].NetAmountDueToBenefCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].NetAmountDueToBenefCurrCode + '</td>';
	        }
	        shtml +=  '<td style="height: 15px; text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].NetAmountDueToBenefAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Exchange Rate Used:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].ExchangeRateUsedCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].ExchangeRateUsedCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].ExchangeRateUsedAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Remittance Amount:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].RemittanceAmountCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].RemittanceAmountCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].RemittanceAmountAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Cable:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].CableCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].CableCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].CableAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Remittance Fee:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].RemittanceFeeCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].RemittanceFeeCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].RemittanceFeeAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Other Charges:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].OtherChargesCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].OtherChargesCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].OtherChargesAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="width: 221px">';
	        shtml +=  '<strong>';
	        shtml +=  'Total amount received from customer:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].TotalAmtRcvdFromCustCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].TotalAmtRcvdFromCustCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].TotalAmtRcvdFromCustAmt,2) + '</td>';
	        shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 70px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;font-size: 8pt">';
	        shtml +=  '' + dt.Rows[0].Check + '<br />';
	        shtml +=  '' + dt.Rows[0].Cash + '<br />';
	        shtml +=  '' + dt.Rows[0].SpecialMessage + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="font-size:8.5pt;width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 40px; text-align: center; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 177px;font-size:8.5pt">';
            shtml +=  '<strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Payment ';
            shtml +=  '<br />';
            shtml +=  'Received / Processed By:</strong></td>';
            shtml +=  '<td style="font-size:8.5pt;">';
            shtml +=  '' + dt.Rows[0].ProcessedBy + '<br />';
            shtml +=  '<hr style="width: 200px; height: 2px;background-color:black;" />';
            shtml +=  '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Authorized Personnel&nbsp;</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="border-top-width: thin; border-left-width: thin; border-left-color: black;';
            shtml +=  'border-bottom-width: thin; border-bottom-color: black; vertical-align: top; width: 100%;';
            shtml +=  'border-top-color: black; height: 20px; text-align: center; border-right-width: thin;';
            shtml +=  'border-right-color: black">';
            shtml +=  '<table style="width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px;text-align:center;">';
            shtml +=  '<span style="font-size: 8.5pt; text-decoration: underline"><strong>DECLARATIONS</strong></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            if (dt.Rows[0].ReceiptDeclarationSub5.toString().length == 0)
            {
                       shtml +=  '<td colspan="3" rowspan="2" style="font-size: 8pt;text-align: justify; line-height: 14pt;">';
            }else{
                        shtml +=  '<td colspan="3" rowspan="2" style="font-size: 7pt;text-align: justify; line-height: 14pt;">';
            }
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub1 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub2 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub3 + '<br />';   
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub4 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub5 + '<br />';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</div>';

            return shtml;
    }
}

/* END RECEIPT REPORT */

/* START Daily Record Keeping REPORT */
function HTMLDailyRecordReport()
{
    var cls = this;
    var _branchcode;    
    var _seqid;
	var _referenceno;
    
    this.evaluate_DailyRecord_query = function(query)
    {
        var squery = unescape(query.toString());
        if (squery.toLowerCase().indexOf("trantype=otcdaily") > -1)
        {
            this.get_DailyRecord_data(
                                    HTMLRep.get_querystring_value(query,"tranvalue"),
                                    HTMLRep.get_branchcode(),
                                    ""
                                 )
            return;
        }
        if (squery.toLowerCase().indexOf("mergeparam") > -1)
        {
            
             this.get_DailyRecord_data(
                                    "",
                                    HTMLRep.parse_merge_param_query(squery,"T@v_branchcode"),
                                    HTMLRep.parse_merge_param_query(squery,"T@v_refno")
									)
            return;
        }
    }
    this.get_DailyRecord_data = function(transactionid,branchcode,referenceno)
    {
        var params; 
        var sp_name = "";
        
                _branchcode =   " @v_branchcode="  + branchcode;    
                _transactionid    =   "@transaction_id="     + transactionid;
				_referenceno = " @v_refno="     + referenceno;
        
       if(referenceno.length == 0)
	   {
	   sp_name = "sp_daily_record_keep_log";
			params  = _transactionid;
			
	   }else{
			sp_name = "sp_daily_record_keep_log_ref";
			params  = _branchcode + "^" + _referenceno ;
		}
		
		AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
            alert(errmsg);    return;         
        } 
	    IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params,this.get_DailyRecord_data_callback);
    }
    
    this.get_DailyRecord_data_callback = function(result)
    {
        try
        {
            var ds = result.value;
            cls.display_DailyRecord_report(ds);
            
        }
        catch(e)
        {
            alert("ERROR WHILE GETTING DAILY KEEPING DATA");
        }
    }
    
    this.display_DailyRecord_report = function(ds)
    {
        var shtml = "";        
        
        shtml   +=  HTMLRep.print_html_button();
        shtml   +=  this.DailyRecord_body(ds.Tables[0]);
        
        document.getElementById("bodydiv").innerHTML = shtml
        //this.Fill_text(ds.Tables[0]);      
		
    }
    this.DailyRecord_body = function(dt)
    {
	    var shtml = "";
        
        shtml +=  '<div>';
shtml +=  '<table style="width: 8.5in; height: 100px">';
shtml +=  '<tr>';
shtml +=  '<td colspan="2" rowspan="1" style="vertical-align: top; height: 50px; text-align: left">';
shtml +=  '<table style="font-size: 10pt; width: 100%; color: black; font-family: arial">';
shtml +=  '<tr>';
shtml +=  '<td style="width: 299px">';
shtml +=  '<td colspan="2" rowspan="3" style="font-size: 12pt;">';
shtml +=  '<strong>'+ dt.Rows[0].remittance_amt_limit +' & ABOVE </strong></td>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '<td style="width: 299px; font-size: 12pt;">';
shtml +=  '<strong><span style="font-size: 14pt">DAILY RECORD KEEPING LOG</span></strong></td>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '<td style="width: 299px; height: 13px">';
shtml +=  '<strong>'+ dt.Rows[0].remitting_branch_name +'</strong></td>';
shtml +=  '</tr>';
shtml +=  '</table>';
shtml +=  '</td>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '<td rowspan="3" style="vertical-align: top; width: 50%; height: 87px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-bottom-width: thin; border-bottom-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
shtml +=  '<strong style="font-family: arial">Remitter Data<br />';
shtml +=  '<br />';
shtml +=  '<table cellspacing = "0" cellpadding ="0" border="1px" style="height:100%;border-right: black thin solid; border-top: black thin solid; border-left: black thin solid;';
shtml +=  'width: 398px; border-bottom: black thin solid; height: 168px">';
shtml +=   '<tr><td>';
shtml +=  '<table style="width: 100%; font-size: 10pt; font-family: arial; font-weight: normal;">';
shtml +=  '<tr>';
shtml +=  '<td colspan="3" rowspan="2" style="width: 50%; height: 15px; text-align: left;">';
shtml +=  'Remitter Name:</td>';
shtml +=  '<td colspan="1" rowspan="2" style="height: 15px; text-align: left; width: 193px;">';
shtml +=  ''+ dt.Rows[0].remitter_fullname +'</td>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '<td colspan="3" rowspan="1" style="width: 50%; text-align: left; height: 24px;">';
shtml +=  'Remitter Address:</td>';
shtml +=  '<td colspan="1" rowspan="1" style="text-align: left; width: 193px; height: 24px;">';
shtml +=  ''+ dt.Rows[0].remitter_address +'</td>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '<td colspan="4" rowspan="1" style="height: 15px; text-align: left">';
shtml +=  '</td>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '<td colspan="3" rowspan="1" style="width: 50%; text-align: left; height: 18px;">';
shtml +=  'Telephone Number:</td>';
shtml +=  '<td colspan="1" rowspan="1" style="text-align: left; width: 193px; height: 18px;">';
shtml +=  ''+ dt.Rows[0].remitter_telno +'</td>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '<td colspan="3" rowspan="1" style="width: 50%; text-align: left;">';
shtml +=  'Remitter ID:</td>';
shtml +=  '<td colspan="1" rowspan="1" style="text-align: left; width: 193px;">';
shtml +=  ''+ dt.Rows[0].ReqID +'</td>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '<td colspan="3" rowspan="1" style="width: 50%; text-align: left; height: 17px;">';
shtml +=  'Remitter Passport No.:</td>';
shtml +=  '<td colspan="1" rowspan="1" style="text-align: left; width: 193px; height: 17px;">';
shtml +=  ''+ dt.Rows[0].passport +'</td>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '<td colspan="3" rowspan="1" style="width: 50%; text-align: left;">';
shtml +=  'Remitter SSS No.:</td>';
shtml +=  '<td colspan="1" rowspan="1" style="text-align: left; width: 193px;">';
shtml +=  ''+ dt.Rows[0].sss_id +'</td>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '<td colspan="3" rowspan="1" style="width: 50%; text-align: left;">';
shtml +=  'Remitter State ID No.:</td>';
shtml +=  '<td colspan="1" rowspan="1" style="text-align: left; width: 193px;">';
shtml +=  ''+ dt.Rows[0].state_id +'</td>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '<td colspan="3" rowspan="1" style="width: 50%; text-align: left;">';
shtml +=  'Remitter Driver License No.:</td>';
shtml +=  '<td colspan="1" rowspan="1" style="text-align: left; width: 193px;">';
shtml +=  ''+ dt.Rows[0].drivers_license +'</td>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '<td colspan="3" rowspan="1" style="width: 50%; text-align: left">';
shtml +=  'Remitter Birthday:</td>';
shtml +=  '<td colspan="1" rowspan="1" style="width: 193px; text-align: left">';
shtml +=  ''+ dt.Rows[0].remitter_birthday +'</td>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '<td colspan="3" rowspan="1" style="width: 50%; text-align: left">';
shtml +=  'Remitter Work:</td>';
shtml +=  '<td colspan="1" rowspan="1" style="width: 193px; text-align: left">';
shtml +=  ''+ dt.Rows[0].remitter_occupation +'</td>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '<td colspan="3" rowspan="1" style="width: 50%; height: 18px; text-align: left">';
shtml +=  '</td>';
shtml +=  '<td colspan="1" rowspan="1" style="width: 193px; height: 18px; text-align: left">';
shtml +=  ''+ dt.Rows[0].remitter_occupation_details +'</td>';
shtml +=  '</tr>';
shtml +=  '</table>';
shtml +=  '<strong style="font-family: arial"></strong>';
shtml +=  ' </td></tr></table>';
shtml +=  '</strong>';
shtml +=  '</td>';
shtml +=  '<td rowspan="3" style="width: 50%; height: 87px; vertical-align: top; border-top-width: thin; border-left-width: thin; border-left-color: black; border-bottom-width: thin; border-bottom-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
shtml +=  '<span style="font-family: Arial"><strong>Transaction Info<br />';
shtml +=  '<span style="font-family: Arial"><strong>';
shtml +=  '<br />';
shtml +=  '<table cellspacing = "0" cellpadding ="0" border="1px" style="border-right: black thin solid; border-top: black thin solid; border-left: black thin solid;';
shtml +=  'width: 393px; border-bottom: black thin solid; height: 100px"><tr><td>';
shtml +=  '<table style="font-size: 10pt; width: 100%; font-family: arial; font-weight: normal;">';
shtml +=  '<tr>';
shtml +=  '<td style="width: 50%; text-align: left">';
shtml +=  'Date Sent:</td>';
shtml +=  '<td style="text-align: left">';
shtml +=  ''+ dt.Rows[0].date_sent +'</td>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '<td style="text-align: left">';
shtml +=  'Reference No:</td>';
shtml +=  '<td style="text-align: left">';
shtml +=  ''+ dt.Rows[0].reference_number +'</td>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '<td style="text-align: left">';
shtml +=  'PNB Branch Code:</td>';
shtml +=  '<td style="text-align: left">';
shtml +=  ''+ dt.Rows[0].branch_name +'</td>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '<td style="text-align: left">';
shtml +=  'Account No.:</td>';
shtml +=  '<td style="text-align: left">';
shtml +=  ''+ dt.Rows[0].account_number +'</td>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '<td style="text-align: left">';
shtml +=  'Commission:</td>';
shtml +=  '<td style="text-align: left">';
shtml +=  ''+ dt.Rows[0].commission +'</td>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '<td style="text-align: left">';
shtml +=  'Oth Amount</td>';
shtml +=  '<td style="text-align: left">';
shtml +=  ''+ HTMLRep.format_numbers(dt.Rows[0].oth_amount,2) +'</td>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '<td style="text-align: left">';
shtml +=  'Rec Mode:</td>';
shtml +=  '<td style="text-align: left">';
shtml +=  ''+ dt.Rows[0].receipt_type +'</td>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '<td style="text-align: left">';
shtml +=  'Implementation Type:</td>';
shtml +=  '<td style="text-align: left">';
shtml +=  ''+ dt.Rows[0].implementation_type +'</td>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '<td style="text-align: left">';
shtml +=  'Product Code:</td>';
shtml +=  '<td style="text-align: left">';
shtml +=  ''+ dt.Rows[0].product_description +'</td>';
shtml +=  '</tr>';
shtml +=  '</table>';
shtml +=  '</td></tr></table>';
shtml +=  '</strong></span></strong></span>';
shtml +=  '</td>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '<td rowspan="1" style="vertical-align: top; width: 50%; height: 87px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-bottom-width: thin; border-bottom-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
shtml +=  '<strong><span style="font-family: Arial">Recipient Data<br />';
shtml +=  '<br />';
shtml +=  '<table cellspacing = "0" cellpadding ="0" border="1px" style="width: 401px;';
shtml +=  'height: 100px; border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; border-bottom: black thin solid;"><tr><td>';
shtml +=  '<table style="width: 100%; font-size: 10pt; font-family: arial; font-weight: normal;">';
shtml +=  '<tr>';
shtml +=  '<td colspan="3" rowspan="2" style="width: 50%; height: 15px; text-align: left;">';
shtml +=  'First Name:</td>';
shtml +=  '<td colspan="1" rowspan="2" style="height: 15px; text-align: left">';
shtml +=   dt.Rows[0].beneficiary_firstname +'</td>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '<td colspan="3" rowspan="1" style="width: 50%; text-align: left;">';
shtml +=  'Last Name:</td>';
shtml +=  '<td colspan="1" rowspan="1" style="text-align: left">';
shtml +=  ''+ dt.Rows[0].beneficiary_lastname +'</td>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '<td colspan="3" rowspan="1" style="width: 50%; text-align: left;">';
shtml +=  'Address:</td>';
shtml +=  '<td colspan="1" rowspan="1" style="text-align: left">';
shtml +=  ''+ dt.Rows[0].beneficiary_address +'<td>';
shtml +=  '</tr>';
shtml +=  '</table>';
shtml +=  '<strong><span style="font-family: Arial"></span></strong>';
shtml +=  '</td></td></table>';
shtml +=  '</span></strong>';
shtml +=  '</td>';
shtml +=  '<td rowspan="1" style="vertical-align: top; width: 50%; height: 87px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-bottom-width: thin; border-bottom-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
shtml +=  '<strong><span style="font-family: Arial">Purpose of Transaction<br />';
shtml +=  '<br />';
shtml +=  '<table cellpadding= "0" cellspacing="0" border="1px" style="width: 100%; font-size: 10pt; font-family: arial; border-right: black thin solid; border-top: black thin solid; font-weight: normal; border-left: black thin solid; border-bottom: black thin solid;"><tr><td>';
shtml +=  '<table style="font-size: 10pt">';
shtml +=  '<tr>';
shtml +=  '<td colspan="3" rowspan="2" style="width: 50%; height: 15px; text-align: left;">';
shtml +=  'Purpose of Transaction:</td>';
shtml +=  '<td colspan="1" rowspan="2" style="height: 15px; text-align: left">';
shtml +=  ''+ dt.Rows[0].transaction_purpose +'</td>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '<td colspan="3" rowspan="1" style="width: 50%; text-align: left;">';
shtml +=  'Relationship to Beneficiary:</td>';
shtml +=  '<td colspan="1" rowspan="1" style="text-align: left">';
shtml +=  ''+ dt.Rows[0].relationship_to_benef +'</td>';
shtml +=  '</tr>';
shtml +=  '</table></td></tr></table>';
shtml +=  '<strong><span style="font-family: Arial"></span></strong></span></strong>';
shtml +=  '</td>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '<td rowspan="1" style="vertical-align: top; width: 50%; height: 87px; border-top-width: thin; font-weight: normal; border-left-width: thin; border-left-color: black; border-bottom-width: thin; border-bottom-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
shtml +=  '<strong><span style="font-family: Arial">Employer Data<br />';
shtml +=  '<strong><span style="font-family: Arial">';
shtml +=  '<br />';
shtml +=  '<table cellspacing = "0" cellpadding ="0" border="1px"  style="border-right: black thin solid; border-top: black thin solid; border-left: black thin solid;';
shtml +=  'width: 403px; border-bottom: black thin solid; height: 100px"><tr><td>';
shtml +=  '<table style="width: 100%; font-size: 10pt; font-family: arial; font-weight: normal;">';
shtml +=  '<tr>';
shtml +=  '<td colspan="3" rowspan="2" style="width: 50%; height: 15px; text-align: left;">';
shtml +=  'Employer Name:</td>';
shtml +=  '<td colspan="1" rowspan="2" style="height: 15px; text-align: left">';
shtml +=  ''+ dt.Rows[0].employer_name +'</td>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '<td colspan="3" rowspan="1" style="width: 50%; text-align: left;">';
shtml +=  'Employer Address:</td>';
shtml +=  '<td colspan="1" rowspan="1" style="text-align: left">';
shtml +=  ''+ dt.Rows[0].employer_address +'</td>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '<td colspan="4" rowspan="1" style="height: 15px; text-align: left">';
shtml +=  '</td>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '<td colspan="3" rowspan="1" style="width: 50%; text-align: left;">';
shtml +=  'Employer Telephone Number:</td>';
shtml +=  '<td colspan="1" rowspan="1" style="text-align: left">';
shtml +=  ''+ dt.Rows[0].employer_telno +'</td>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '<td colspan="3" rowspan="1" style="width: 50%; text-align: left;">';
shtml +=  'Source of Funds:</td>';
shtml +=  '<td colspan="1" rowspan="1" style="text-align: left">';
shtml +=  ''+ dt.Rows[0].source_of_funds +'</td>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '<td colspan="3" rowspan="1" style="width: 50%; text-align: left;">';
shtml +=  'Beneficiary Occupation</td>';
shtml +=  '<td colspan="1" rowspan="1" style="text-align: left">';
shtml +=  ''+ dt.Rows[0].beneficiary_occupation +'</td>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '<td colspan="3" rowspan="1" style="width: 50%; text-align: left;">';
shtml +=  '</td>';
shtml +=  '<td colspan="1" rowspan="1" style="text-align: left">';
shtml +=  ''+ dt.Rows[0].beneficiary_occupation_details +'</td>';
shtml +=  '</tr>';
shtml +=  '</table>';
shtml +=  '<strong><span style="font-family: Arial"></span></strong>';
shtml +=  '</td></tr></table>';
shtml +=  '</span></strong></span></strong>';
shtml +=  '</td>';
shtml +=  '<td rowspan="1" style="vertical-align: top; width: 50%; height: 87px; font-size: 12pt;">';
shtml +=  '</td>';
shtml +=  '</tr>';
shtml +=  '<tr style="font-size: 12pt">';
//shtml +=  '<td colspan="2" rowspan="1" style="font-size: 10pt; vertical-align: top; height: 87px;';
//shtml +=  'text-align: left">';
//shtml +=  '<strong style="font-family: arial">I Confirm that the above information is correct and accurate, to the best of';
//shtml +=  'my knowledge<br />';
//shtml +=  '<br />';
//shtml +=  '&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;';
//shtml +=  '<hr style="width: 234px; height: 1px;color:black;" />';
//shtml +=  '</strong>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; <strong style="font-family: arial">REMITTER</strong></td>';
//shtml +=  '</tr>';
 shtml +=  '<td colspan="2" style="font-size: 10pt; vertical-align: top; height: 10px; text-align: left">';
 shtml +=  '                               <strong style="font-family: arial">I confirm that the above information is correct and';
 shtml +=  '                                   accurate, to the best of my knowledge</strong>';
 shtml +=  '                                   </td></tr>';
 shtml +=  '                                   <tr><td colspan="2" valign="bottom" style="height:50px">';
 shtml +=  '                                   <hr align="left" style="width: 234px; height: .5px ; border-left-color: black; border-bottom-color: black; border-top-style: solid; border-top-color: black; border-right-style: solid; border-left-style: solid; border-right-color: black; border-bottom-style: solid;" />';
 shtml +=  '                                   </td></tr>';
 shtml +=  '                                   <tr><td colspan="2">&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; <strong style="font-family: arial">REMITTER</strong></td></tr>';
                    
shtml +=  '</table>';
shtml +=  '';
shtml +=  '</div>';



        return shtml;
    }
    
   

}

/* END DailyRecord REPORT */

/* START auditlog REPORT */
function HTMLAuditLogReport()
{
    var cls = this;
    var _DateFromTrans;
    var _DateToTrans;
    

    
    this.evaluate_auditlog_query = function(query)
    {
        var squery = unescape(query.toString());

        if (squery.toLowerCase().indexOf("sp_rptauditlogreport") > -1)
        {
            
             this.get_auditlog_data(
                                    HTMLRep.parse_merge_param_query(squery,"T@DateFromTrans"),
                                    HTMLRep.parse_merge_param_query(squery,"T@DateToTrans")
									)
            return;
        }
    }
    this.get_auditlog_data = function(DateFromTrans,DateToTrans)
    {
        var params; 
        var sp_name = "";
        
                _DateFromTrans =   " @DateFromTrans="  + DateFromTrans;    
                _DateToTrans    =   "@DateToTrans="     + DateToTrans;
				        
        sp_name = "sp_rptAuditLogReport";
		params  = _DateFromTrans + "^" + _DateToTrans ;
		
		
		AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {               
                 
            var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
            alert(errmsg);    return;         
        } 
	    IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params,this.get_auditlog_data_callback);
    }
    
    this.get_auditlog_data_callback = function(result)
    {
        try
        {
            var ds = result.value;
            cls.display_auditlog_report(ds);
            
        }
        catch(e)
        {
            alert("ERROR WHILE GETTING AUDIT LOG DATA");
        }
    }
    
    this.display_auditlog_report = function(ds)
    {
        var shtml = "";        
        
        shtml   +=  HTMLRep.print_html_button();
        shtml   +=  this.auditlog_body(ds.Tables[0]);
        
        document.getElementById("bodydiv").innerHTML = shtml
        //this.Fill_text(ds.Tables[0]);      
		
    }
    this.auditlog_body = function(dt)
    {
    var vd = window.location.href.split('/')[3];
	    var shtml = "";
        shtml +=  ' <div>';
shtml +=  '        <div>';
shtml +=  '            <div>';
shtml +=  '                <div>';
shtml +=  '                    <div>';
shtml +=  '                        &nbsp;';
shtml +=  '                        <table style="width: 11in">';
shtml +=  '                            <tr>';
shtml +=  '                                <td colspan="3" style="width: 0in">';
shtml +=  '                                    <table style="width: 100%">';
shtml +=  '                                        <tr>';
shtml +=  '                                            <td colspan="3" style="vertical-align: top; height: 15px; text-align: center">';
shtml +=  '                                                <span style="font-family: Arial"><span><span style="font-size: 8pt"></span></span></span>';
shtml +=  '                                                <span style="font-family: Arial"><span><span style="font-size: 8pt">';
shtml +=  '                                                    <table style="width: 347px">';
shtml +=  '                                                        <tr>';
shtml +=  '                                                            <td rowspan="3" style="width: 120px">';
shtml +=  '                                                                <img src="../../../'+ vd +'/images/pnb_black_logo.JPG" /></td>';
shtml +=  '                                                            <td colspan="2">';
shtml +=  '                                                                <strong><span style="font-size: 10pt">PHILIPPINE NATIONAL BANK</span></strong></td>';
shtml +=  '                                                        </tr>';
shtml +=  '                                                        <tr>';
shtml +=  '                                                            <td colspan="2" style="text-align: center; font-size: 9pt">';
shtml +=  '                                                                INTEGRATED REMITTANCE SYSTEM<br />';
shtml +=  '                                                                Audit Log Report</td>';
shtml +=  '                                                        </tr>';
shtml +=  '                                                        <tr>';
shtml +=  '                                                            <td style="width: 30%; height: 16px">';
shtml +=  '                                                                ' +  dt.Rows[i].DateFrom + '</td>';
shtml +=  '                                                            <td style="width: 30%; height: 16px">';
shtml +=  '                                                                ' +  dt.Rows[i].DateTo + '</td>';
shtml +=  '                                                        </tr>';
shtml +=  '                                                    </table>';
shtml +=  '                                                </span></span></span>';
shtml +=  '                                            </td>';
shtml +=  '                                        </tr>';
shtml +=  '                                    </table>';
shtml +=  '                                </td>';
shtml +=  '                            </tr>';
shtml +=  '                        </table>';
shtml +=  '                        <table cellpadding="0" cellspacing="0" style="font-family: arial; height: 1.14in; width: 11in;">';
shtml +=  '                            <tr>';
shtml +=  '                                <td colspan="3" style="HEIGHT: 108px">';
shtml +=  '                                    <table style="width: 100%; height: 0px; font-size: 10pt;">';
shtml +=  '                                        <tr>';
shtml +=  '                                            <td style="width: 168px">';
shtml +=  '                                                <span style="font-size: 10pt"><strong>';
shtml +=  '                                                DATE / TIME</strong></span></td>';
shtml +=  '                                            <td style="width: 173px">';
shtml +=  '                                                <span style="font-size: 10pt"><strong>';
shtml +=  '                                                USER ID</strong></span></td>';
shtml +=  '                                            <td style="width: 177px">';
shtml +=  '                                                <span style="font-size: 10pt"><strong>';
shtml +=  '                                                USERNAME</strong></span></td>';
shtml +=  '                                            <td style="width: 261px">';
shtml +=  '                                                <span style="font-size: 10pt"><strong>';
shtml +=  '                                                MODULE / CATEGORY</strong></span></td>';
shtml +=  '                                            <td>';
shtml +=  '                                                <span style="font-size: 10pt"><strong>';
shtml +=  '                                                DESCRIPTION</strong></span></td>';
shtml +=  '                                        </tr>';
for(i=0;i<dt.Rows.length;i++)
    {
    shtml +=  '                                        <tr>';
    shtml +=  '                                            <td nowrap>';
    shtml +=  '                                                <span style="font-size: 10pt">' +  dt.Rows[i].DateTime + '</span></td>';
    shtml +=  '                                            <td style="width: 173px">';
    shtml +=  '                                                ' +  dt.Rows[i].UserId + '</td>';
    shtml +=  '                                            <td style="width: 177px">';
    shtml +=  '                                                ' +  dt.Rows[i].UserName + '</td>';
    shtml +=  '                                            <td style="width: 261px">';
    shtml +=  '                                                ' +  dt.Rows[i].MODULE + '</td>';
    shtml +=  '                                            <td>';
    shtml +=  '                                                ' +  dt.Rows[i].Description + '</td>';
    shtml +=  '                                        </tr>';
    }
shtml +=  '                                        ';
shtml +=  '                                    </table>';
shtml +=  '                                </td>';
shtml +=  '                            </tr>';
shtml +=  '                            <tr>';
shtml +=  '                            </tr>';
shtml +=  '                        </table>';
shtml +=  '                    </div>';
shtml +=  '                </div>';
shtml +=  '            </div>';
shtml +=  '        </div>';
shtml +=  '    ';
shtml +=  '    </div>';

        




        return shtml;
    }
    
   

}

/* END auditlog REPORT */

/* START preprinted Receipt REPORT */
function HTMLPrePrintedReceiptReport()
{
    var cls = this;
    var _branchcode;    
    var _seqid;
    var _referenceno;
    
    this.evaluate_PrePrintedReceipt_query = function(query)
    {
        var squery = unescape(query.toString());

        var squery = unescape(query.toString());
        if (squery.toLowerCase().indexOf("trantype=otc") > -1)
        {
            _branchcode = HTMLRep.get_branchcode();
            this.get_PrePrintedReceipt_data(
                                    HTMLRep.get_querystring_value(query,"tranvalue"),
                                    HTMLRep.get_branchcode(),
                                    ""
                                 )
            return;
        }
        if (squery.toLowerCase().indexOf("mergeparam") > -1)
        {
             _branchcode = HTMLRep.parse_merge_param_query(squery,"T@v_branchcode")
             this.get_PrePrintedReceipt_data(
                                    "",
                                    HTMLRep.parse_merge_param_query(squery,"T@v_branchcode"),
                                    HTMLRep.parse_merge_param_query(squery,"T@v_referenceno")
									)
            return;
        }
    }
    this.get_PrePrintedReceipt_data = function(seq_id,branchcode,referenceno)
    {
        var params; 
        var sp_name = "";
        
                _branchcode =   " @v_branchcode="  + branchcode;    
                _seqid    =   "@v_seqid="     + seq_id;
		_referenceno = " @v_referenceno="     + referenceno;
		
		
	if(seq_id.length == 0)
	{
			sp_name = "SP_OTC_Reprint_Receipt_preprinted";
			params  = _branchcode + "^" + _referenceno ;
	}
	else{
			sp_name = "SP_OTC_Receipt_preprinted";
			params  = _branchcode + "^" + _seqid ;
	}
	AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
            alert(errmsg);       return;      
        } 
	    IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params,this.get_PrePrintedReceipt_callback);
    }
    
    this.get_PrePrintedReceipt_callback = function(result)
    {
        try
        {
            var ds = result.value;
            cls.display_PrePrintedReceipt_report(ds);
            
        }
        catch(e)
        {
            alert("ERROR WHILE RECEIPT DATA");
        }
    }
    
    this.display_PrePrintedReceipt_report = function(ds)
    {
        var shtml = "";        
        
        shtml   +=  HTMLRep.print_html_button();
        shtml   +=  this.PrePrintedReceipt_body(ds.Tables[0]);
        
        document.getElementById("bodydiv").innerHTML = shtml
        //this.Fill_text(ds.Tables[0]);      
		
    }
    this.PrePrintedReceipt_body = function(dt)
    {
        var vd = window.location.href.split('/')[3];
	    var shtml = "";
    
      //old preprinted
    
    var vd = window.location.href.split('/')[3];
	    var shtml = "";
          //old preprinted
    
    var vd = window.location.href.split('/')[3];
	    var shtml = "";
        shtml +=  '    <div>';
        shtml +=  '        &nbsp;';
        shtml +=  '        <div style="left: -429px; width: 100px; position: absolute; top: 45px; height: 100px">';
        shtml +=  '        </div>';
        shtml +=  '        &nbsp;';
        shtml +=  '        <div style="width: 8in; height: 11in">';
        shtml +=  '            &nbsp;';
        shtml +=  '            <div style="left: 630px; width: 92px; position: absolute; top: 36px; height: 21px">';
        shtml +=  '                <span style="font-size: 9pt; font-family: Arial">Clearing Date:</span></div>';
        shtml +=  '            &nbsp;';
        shtml +=  '            <div style="font-size: 12pt; left: 113px; width: 434px; position: absolute; top: 178px;';
        shtml +=  '                height: 21px">';
        shtml +=  '                <span style="font-size: 12pt; font-family: Arial">' +  dt.Rows[0].RemitterName + '</span></div>';
        shtml +=  '            <div style="font-size: 12pt; left: 708px; width: 131px; position: absolute; top: 36px;';
        shtml +=  '                height: 21px">';
        shtml +=  '                <span style="font-size: 12pt; font-family: Arial">' +  dt.Rows[0].ClearingDate + '</span></div>';
        shtml +=  '            <div style="left: 708px; width: 133px; position: absolute; top: 24px; height: 13px">';
        shtml +=  '                <span style="font-size: 12pt; font-family: Arial">' +  HTMLRep.formatDate(dt.Rows[0].TranxDate,"/") + '</span></div>';
        shtml +=  '            <div style="left: 115px; width: 429px; position: absolute; top: 229px; height: 130px">';
        shtml +=  '                <span style="font-size: 12pt; font-family: Arial">' +  dt.Rows[0].RemitterAddress + '</span></div>';
        shtml +=  '            &nbsp;';
        shtml +=  '            <div style="left: 128px; width: 405px; position: absolute; top: 444px; height: 21px">';
        shtml +=  '                <span style="font-size: 12pt; font-family: Arial">' +  dt.Rows[0].BenefName + '</span></div>';
        shtml +=  '            <div style="left: 129px; width: 329px; position: absolute; top: 485px; height: 48px">';
        shtml +=  '                <span style="font-size: 12pt; font-family: Arial">' +  dt.Rows[0].BenefAddress + '</span></div>';
        shtml +=  '            <div style="left: 136px; width: 318px; position: absolute; top: 534px; height: 61px">';
        shtml +=  '                <span style="font-size: 12pt; font-family: Arial">' +  dt.Rows[0].SpecialMessage + '</span></div>';
        shtml +=  '            <div style="left: 137px; width: 316px; position: absolute; top: 582px; height: 21px">';
        shtml +=  '                <span style="font-size: 12pt; font-family: Arial">' +  dt.Rows[0].OtherBankName + '</span></div>';
        shtml +=  '            <div style="left: 138px; width: 314px; position: absolute; top: 644px; height: 21px">';
        shtml +=  '                <span style="font-size: 12pt; font-family: Arial">' +  dt.Rows[0].ThruPNBBranch + '</span></div>';
        shtml +=  '            <div style="left: 120px; width: 390px; position: absolute; top: 719px; height: 21px">';
        shtml +=  '                <span style="font-size: 12pt; font-family: Arial">' +  dt.Rows[0].TransType + '</span></div>';
        shtml +=  '            <div style="left: 120px; width: 392px; position: absolute; top: 745px; height: 21px">';
        shtml +=  '                <span style="font-size: 12pt; font-family: Arial">' +  dt.Rows[0].SourceFunds + '</span></div>';
        shtml +=  '            <div style="left: 563px; width: 122px; position: absolute; top: 194px; height: 21px;';
        shtml +=  '                text-align: justify">';
        shtml +=  '                <span style="font-size: 12pt; font-family: Arial">( &nbsp;' + dt.Rows[0].ExchangeRateUsedCurrCode + '&nbsp; )</span></div>';
        shtml +=  '            <div style="left: 557px; width: 122px; position: absolute; top: 170px; height: 21px;';
        shtml +=  '                text-align: right">';
        shtml +=  '                <span style="font-size: 12pt; font-family: Arial">' + dt.Rows[0].NetAmountDueToBenefCurrCode  + '</span></div>';
        shtml +=  '            <div style="left: 753px; width: 100px; position: absolute; top: 170px; height: 21px;';
        shtml +=  '                text-align: right">';
        shtml +=  '                <span style="font-size: 12pt; font-family: Arial">' +  HTMLRep.format_numbers(dt.Rows[0].NetAmountDueToBenefAmt,2) + '</span></div>';
        shtml +=  '            <div style="left: 562px; width: 122px; position: absolute; top: 215px; height: 21px;';
        shtml +=  '                text-align: right">';
        shtml +=  '                <span style="font-size: 12pt; font-family: Arial">' +  dt.Rows[0].RemittanceAmountCurrCode + '</span></div>';
        shtml +=  '            <div style="left: 753px; width: 100px; position: absolute; top: 215px; height: 21px;';
        shtml +=  '                text-align: right">';
        shtml +=  '                <span style="font-size: 12pt; font-family: Arial">' +  HTMLRep.format_numbers(dt.Rows[0].RemittanceAmountAmt,2) + '</span></div>';
        shtml +=  '            <div style="left: 613px; width: 122px; position: absolute; top: 274px; height: 21px;';
        shtml +=  '                text-align: right">';
        shtml +=  '                <span style="font-size: 12pt; font-family: Arial">' +  dt.Rows[0].TotalAmtRcvdFromCustCurrCode  + '</span></div>';
        shtml +=  '            <div style="left: 612px; width: 122px; position: absolute; top: 306px; height: 21px;';
        shtml +=  '                text-align: right">';
        shtml +=  '                <span style="font-size: 12pt; font-family: Arial"></span></div>';
        shtml +=  '            <div style="left: 557px; width: 122px; position: absolute; top: 346px; height: 21px;';
        shtml +=  '                text-align: right">';
        shtml +=  '                <span style="font-size: 12pt; font-family: Arial">' +  dt.Rows[0].OtherChargesCurrCode + '</span></div>';
        shtml +=  '            <div style="left: 505px; width: 100px; position: absolute; top: 500px; height: 17px;';
        shtml +=  '                text-align: left">';
        shtml +=  '                <span style="font-size: 8pt; font-family: Arial">Denominations:</span></div>';
        shtml +=  '         <div style="left: 504px; width: 155px; position: absolute; top: 531px; height: 21px;'
        shtml +=  '        text-align: left">'
        shtml +=  '       <span style="font-size: 8pt; font-family: Arial">CHECK: ' +  dt.Rows[i].Check + '</span></div>'
        shtml +=  '         <div style="left: 504px; width: 153px; position: absolute; top: 565px; height: 21px;'
        shtml +=  'text-align: left" id="DIV1">'
        shtml +=  '<span style="font-size: 8pt; font-family: Arial">CASH: ' +  dt.Rows[i].Cash + '</span></div>'
        shtml +=  '            <div style="left: 493px; width: 100px; position: absolute; top: 475px; height: 21px;';
        shtml +=  '                text-align: left">';
        shtml +=  '                <span style="font-size: 12pt; font-family: Arial"></span></div>';
        shtml +=  '            <div style="font-size: 14pt; left: 309px; width: 262px; font-family: Times New Roman;';
        shtml +=  '                position: absolute; top: 15px; height: 21px; text-align: center">';
        shtml +=  '                <strong>' +  dt.Rows[0].TranStatus + '</strong></div>';
        shtml +=  '            <div style="font-size: 12pt; left: 120px; width: 262px; font-family: Times New Roman;';
        shtml +=  '                position: absolute; top: 86px; height: 21px">';
        shtml +=  '                <span style="font-size: 12pt; font-family: Arial">' +  dt.Rows[0].RemitterID + '</span></div><div style="left: 132px; width: 409px; position: absolute; top: 400px; height: 21px">';
        shtml +=  '                    <span style="font-size: 12pt; font-family: Arial">' +  dt.Rows[0].BenefCIFID + '</span></div>';
        shtml +=  '                    <div style="left: 140px; width: 390px; position: absolute; top: 692px; height: 21px">';
        shtml +=  '                        <span style="font-size: 12pt; font-family: Arial">' +  dt.Rows[0].BenefAcctNo + '</span></div>';
        shtml +=  '                        <div style="left: 141px; width: 392px; position: absolute; top: 774px; height: 21px">';
        shtml +=  '                            <span style="font-size: 12pt; font-family: Arial">' +  dt.Rows[0].BenefTelNo + '</span></div><div style="left: 117px; width: 425px; position: absolute; top: 370px; height: 21px">';
        shtml +=  '                                <span style="font-size: 12pt; font-family: Arial">' +  dt.Rows[0].RemitterTelNo + '</span></div>';
        shtml +=  '        </div>';
        shtml +=  '        <div style="font-size: 12pt; left: 754px; width: 100px; font-family: Times New Roman;';
        shtml +=  '            position: absolute; top: 194px; height: 21px; text-align: right">';
        shtml +=  '            <span style="font-size: 12pt; font-family: Arial">' +  dt.Rows[0].ExchangeRateUsedAmt + '</span></div>';
        shtml +=  '        <div style="left: 737px; width: 100px; position: absolute; top: 533px; height: 21px;';
        shtml +=  '            text-align: right">';
        shtml +=  '            <span style="font-size: 12pt; font-family: Arial">' +  dt.Rows[0].Ten + '</span></div>';
        shtml +=  '        <div style="left: 738px; width: 100px; position: absolute; top: 570px; height: 21px;';
        shtml +=  '            text-align: right">';
        shtml +=  '            <span style="font-size: 12pt; font-family: Arial">' +  dt.Rows[0].Two + '</span></div>';
        shtml +=  '        <div style="left: 738px; width: 100px; position: absolute; top: 550px; height: 21px;';
        shtml +=  '            text-align: right">';
        shtml +=  '            <span style="font-size: 12pt; font-family: Arial">' +  dt.Rows[0].Five + '</span></div>';
        shtml +=  '        <div style="left: 738px; width: 100px; position: absolute; top: 515px; height: 21px;';
        shtml +=  '            text-align: right">';
        shtml +=  '            <span style="font-size: 12pt; font-family: Arial">' +  dt.Rows[0].Twenty + '</span></div>';
        shtml +=  '        <div style="left: 737px; width: 100px; position: absolute; top: 590px; height: 21px;';
        shtml +=  '            text-align: right">';
        shtml +=  '            <span style="font-size: 12pt; font-family: Arial">' +  dt.Rows[0].One + '</span></div>';
        shtml +=  '        <div style="left: 738px; width: 100px; position: absolute; top: 501px; height: 21px;';
        shtml +=  '            text-align: right">';
        shtml +=  '            <span style="font-size: 12pt; font-family: Arial">' +  dt.Rows[0].Fifty + '</span></div>';
        shtml +=  '        <div style="left: 738px; width: 100px; position: absolute; top: 482px; height: 21px;';
        shtml +=  '            text-align: right">';
        shtml +=  '            <span style="font-size: 12pt; font-family: Arial">' +  dt.Rows[0].OneHund + '</span></div>';
        shtml +=  '        <div style="left: 738px; width: 100px; position: absolute; top: 464px; height: 21px;';
        shtml +=  '            text-align: right">';
        shtml +=  '            <span style="font-size: 12pt; font-family: Arial">' +  dt.Rows[0].FiveHund + '</span></div>';
        shtml +=  '        <div style="left: 738px; width: 100px; position: absolute; top: 445px; height: 21px;';
        shtml +=  '            text-align: right">';
        shtml +=  '            <span style="font-size: 12pt; font-family: Arial">' +  dt.Rows[0].OneThou + '</span></div>';
        shtml +=  '        &nbsp;';
        shtml +=  '        <div style="left: 660px; width: 183px; position: absolute; top: 777px; height: 21px;';
        shtml +=  '            text-align: right">';
        shtml +=  '            <span style="font-size: 12pt; font-family: Arial">' +  dt.Rows[0].ProcessedBy + '</span></div><div style="left: 738px; width: 100px; position: absolute; top: 433px; height: 21px;';
        shtml +=  '            text-align: right">';
        shtml +=  '                <span style="font-size: 12pt; font-family: Arial">' +  dt.Rows[0].TenThou + '</span></div>';
        shtml +=  '        <div style="left: 751px; width: 100px; position: absolute; top: 346px; height: 21px;';
        shtml +=  '            text-align: right">';
        shtml +=  '            <span style="font-size: 12pt; font-family: Arial">' +  HTMLRep.format_numbers(dt.Rows[0].OtherChargesAmt,2) + '</span></div>';
        shtml +=  '        <div style="left: 751px; width: 100px; position: absolute; top: 305px; height: 21px;';
        shtml +=  '            text-align: right">';
        shtml +=  '            <span style="font-size: 12pt; font-family: Arial">' +  HTMLRep.format_numbers(dt.Rows[0].TotalBackEndCharges,2) + '</span></div>';
        shtml +=  '        <div style="left: 752px; width: 100px; position: absolute; top: 274px; height: 21px;';
        shtml +=  '            text-align: right">';
        shtml +=  '            <span style="font-size: 12pt; font-family: Arial">' +  HTMLRep.format_numbers(dt.Rows[0].TotalAmtRcvdFromCustAmt,2)  + '</span></div>';
        shtml +=  '        <div style="left: 752px; width: 100px; position: absolute; top: 246px; height: 21px;';
        shtml +=  '            text-align: right">';
        shtml +=  '            <span style="font-size: 12pt; font-family: Arial">' +  HTMLRep.format_numbers(dt.Rows[0].CableAmt,2) + '</span></div>';
        shtml +=  '        <div style="left: 712px; width: 148px; position: absolute; top: 86px; height: 21px;';
        shtml +=  '            text-align: left">';
        shtml +=  '            <span style="font-size: 12pt; font-family: Arial">' +  dt.Rows[0].ReferenceNo + '</span></div>';
        shtml +=  '        &nbsp;';
        shtml +=  '        <div style="left: 613px; width: 122px; position: absolute; top: 246px; height: 21px;';
        shtml +=  '            text-align: right">';
        shtml +=  '            <span style="font-size: 12pt; font-family: Arial">' +  dt.Rows[0].CableCurrCode + '</span></div>';
        shtml +=  '    ';
        shtml +=  '    </div>';
    
  

     
    return shtml;
    }
    
   

}

/* END preprinted Receipt REPORT */


/* OBP Receipt */
/* START RECEIPT REPORT */
function HTMLOBPReceiptReport()
{
    var cls = this;
    var _branchcode;    
    var _seqid;
	var _referenceno;
    
    this.evaluate_Receipt_query = function(query)
    {
        var squery = unescape(query.toString());
        if (squery.toLowerCase().indexOf("trantype=obp") > -1)
        {
            _branchcode = HTMLRep.get_branchcode();
            this.get_Receipt_data(
                                    HTMLRep.get_querystring_value(query,"tranvalue"),
                                    HTMLRep.get_branchcode(),
                                    ""
                                 )
            return;
        }
        if (squery.toLowerCase().indexOf("mergeparam") > -1)
        {
             _branchcode = HTMLRep.parse_merge_param_query(squery,"T@v_branchcode")
             this.get_Receipt_data(
                                    "",
                                    HTMLRep.parse_merge_param_query(squery,"T@v_branchcode"),
                                    HTMLRep.parse_merge_param_query(squery,"T@v_referenceno")
									)
            return;
        }
    }
    this.get_Receipt_data = function(seq_id,branchcode,referenceno)
    {
        var params; 
        var sp_name = "";
	var pass  =	false;
        
                _branchcode =   " @v_branchcode="  + branchcode;    
                _seqid    =   "@seqid="     + seq_id;
				_referenceno = " @v_referenceno="     + referenceno;
        
	pass = this.check_newdbswitch();
	
       if(seq_id.length == 0)
	   {
			if (pass)
			{
			    sp_name = "sp_otc_obp_Receipt_reprint_irsw";
			}
			else
			{
			    sp_name = "sp_otc_obp_Receipt_reprint";
			}
			params  = _branchcode + "^" + _referenceno ;
	   }else{
			sp_name = "SP_OTC_OBP_Receipt";
			params  = _branchcode + "^" + _seqid ;
		}
		AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
            alert(errmsg);       return;      
        } 
	    IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params,this.get_Receipt_data_callback);
    }
    
    this.get_Receipt_data_callback = function(result)
    {
        try
        {
            var ds = result.value;
            
            cls.check_branch();
            cls.display_Receipt_report(ds);
            
            
        }
        catch(e)
        {
            alert("ERROR WHILE GETTING Receipt DATA");
        }
    }
    this.check_newdbswitch = function()
    {
    
	var pass = false;
	var sp_name = 'proc_check_newdbswitch';
        var squery = HTMLRep.get_hidden_querystring("hidQueryString");
        var sbranchcode = HTMLRep.get_branchcode();
        
        var params  =  '@branchcode=' + sbranchcode;
        var isRCC  = false;
        AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
            alert(errmsg);     return;        
        } 
        
        var dtcchecksw = IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params).value;
        
        try
        {
            if (dtcchecksw.Tables[0].Rows.length > 0)
            {
                pass = true;
            }
        }
        catch(e)
        {
        }
	
	return pass; 
    }
    
    this.display_Receipt_report = function(ds)
    {
        var sp_name = 'proc_check_reporthalfpage';
        var squery = HTMLRep.get_hidden_querystring("hidQueryString");
        var sbranchcode = ds.Tables[0].Rows[0].ReferenceNo;
        sbranchcode =  sbranchcode.substring(1,4);
        var params  =  '@v_branhcode=' + sbranchcode;
        var ishalfpage  = false;
        AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
            alert(errmsg);     return;        
        } 
        
        var dtcheckhalfpage = IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params).value;
        
        try
        {
            if (dtcheckhalfpage.Tables[0].Rows.length > 0)
            {
                ishalfpage = true;
            }
        }
        catch(e)
        {
        }
    
        
        var shtml = "";        
        shtml +=  HTMLRep.print_html_button();
        var isRCC = cls.check_branch();
	
	isRCC = (cls.check_ars()) ? 0 : isRCC ;
	
        if (!ishalfpage)
        {
           
            shtml +=  this.Receipt_body(ds.Tables[0],isRCC);
               
            shtml += cls.half_page(ds.Tables[0]);
            
	        shtml +=  '</div>';
        }
        else
        {
           shtml +=  this.Receipt_body_halfpage(ds.Tables[0],isRCC);
           
        }
        document.getElementById("bodydiv").innerHTML = shtml
    }
    this.check_branch = function()
    {
        var sp_name = 'sp_report_checkbranch';
        var squery = HTMLRep.get_hidden_querystring("hidQueryString");
        var sbranchcode = HTMLRep.get_branchcode();
        
        var params  =  '@branchcode=' + sbranchcode;
        var isRCC  = false;
        AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
            alert(errmsg);     return;        
        } 
        
        var dtccheckbranch = IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params).value;
        
        try
        {
            if (dtccheckbranch.Tables[0].Rows.length > 0)
            {
                isRCC = true;
            }
        }
        catch(e)
        {
        }
        return isRCC;
    }
    
    this.check_ars = function()
    {
        var sp_name = 'proc_check_ars';
        
        var sbranchcode = HTMLRep.get_branchcode();
        
        var params  =  '@v_branchcode=' + sbranchcode;
        var isARS  = false;
        AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
            alert(errmsg);     return;        
        } 
        
        var dtccheckbranch = IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params).value;
        
        try
        {
            if (dtccheckbranch.Tables[0].Rows.length > 0)
            {
                isARS = true;
            }
        }
        catch(e)
        {
        }
        return isARS;
    }
    this.Receipt_body = function(dt,isRCC)
    {
	
	    var vd = window.location.href.split('/')[3];
	    
        var shtml = "";
	        shtml +=  '<div>';
            shtml +=  '<table style="width: 8.5in; font-size: 8pt; font-family: arial;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="width: 0in">';
            shtml +=  '<table style="width: 100%;font-size: 10pt;">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 60px; height: 15px">';
            shtml +=  '<img src="../../../'+ vd +'/images/pnb_black_logo.JPG" style="width: 58px; height: 68px" /></td>';
            shtml +=  '<td style="vertical-align: top; width: 520px; height: 15px">';
            shtml +=  '<strong>' + dt.Rows[0].Header1 + '</strong><br />';
            shtml +=  '' + dt.Rows[0].Header2 + '<br />';
            shtml +=  '<strong>' + dt.Rows[0].NameofBranch + '</strong><br />';
            shtml +=  '' + dt.Rows[0].AdressofBranch + '<br />';
            shtml +=  '' + dt.Rows[0].BranchofContact + '<br />';
            shtml +=  '' + dt.Rows[0].BranchofEmail + '<br />';
            shtml +=  '' + dt.Rows[0].corp_address + '<br />';
            shtml +=  '' + dt.Rows[0].corp_telno + '</td>';
            shtml +=  '<td style="vertical-align: top; height: 15px">';
            shtml +=  'Transaction Date: ' + HTMLRep.formatDate(dt.Rows[0].TranxDate,"/") + '<br />';
	        if (dt.Rows[0].ClearingDate == '1900')
	        {
	        shtml +=  'Clearing Date: <br />';
	        }else{
	        shtml +=  'Clearing Date: '+ dt.Rows[0].ClearingDate +'<br />';
	        }
            shtml +=  'Time: '+ HTMLRep.formatTime(dt.Rows[0].TranxDate) +'<br />';
            shtml +=  'Loyalty Points:';
            shtml +=  '<br />';
            shtml +=  '' + dt.Rows[0].TranStatus + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 8.5in; font-size: 10pt; font-family: arial;';
            shtml +=  'height: 0.23in; border-top-width: thin; border-left-width: thin; border-left-color: black; border-bottom-width: thin; border-bottom-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="border-right: black thin solid; border-top: black thin solid;';
            shtml +=  'border-left: black thin solid; border-bottom: black thin solid">';
            shtml +=  '<table style="font-size: 10pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 244px; height: 14px">';
            shtml +=  '<strong>Remitter ID No.: </strong>' + dt.Rows[0].RemitterID + '</td>';
            shtml +=  '<td style="width: 285px; height: 14px">';
            shtml +=  '<strong>Official Receipt No.:</strong> ' + dt.Rows[0].ReceiptNo + '</td>';
            shtml +=  '<td style="width: 257px; height: 14px">';
            shtml +=  '<strong>Reference No.:</strong> ' + dt.Rows[0].ReferenceNo + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';    
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td style="border-right: black thin solid; border-top: black thin solid; vertical-align: top;';
            shtml +=  'border-left: black thin solid; width: 50%; border-bottom: black thin solid; height: 20px">';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 0px; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="vertical-align: top; width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 20px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 10pt; text-decoration: underline">REMITTER INFO</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="2">';
            shtml +=  '<span><span style="font-size: 10pt"><strong>&nbsp;Name: </strong>' + dt.Rows[0].RemitterName + '<br />';
            shtml +=  '</span></span>';
            shtml +=  '<table style="font-size: 10pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="width: 61px; vertical-align: top;">';
            shtml +=  '<strong style="width: 30px">Address:<br />';
            shtml +=  '</strong></td>';
            shtml +=  '<td colspan="2" rowspan="3" style="height: 50px; vertical-align: top;">';
            shtml +=  '' + dt.Rows[0].RemitterAddress + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<span><span style="font-size: 10pt; width: 100%"><strong>Telephone Number:</strong>';
            shtml +=  '' + dt.Rows[0].RemitterTelNo + '</span></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="vertical-align: top; width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 20px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 100% ; height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 10pt; text-decoration: underline">BENEFICIARY INFO</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="2">';
            shtml +=  '<span><span style="font-size: 10pt"><strong>&nbsp;Name:</strong> ' + dt.Rows[0].BenefName + '<br />';
            shtml +=  '</span></span>';
            shtml +=  '<table style="font-size: 10pt; width: 100%; vertical-align: top;">';
            shtml +=  '<tr>';
            shtml +=  '<td  style="width: 60px; vertical-align: top;"><strong style="width: 30px">Address:<br />';
            shtml +=  '</strong>';
            shtml +=  '</td>';
            shtml +=  '<td rowspan="3" style="width: 90%; height: 50px; vertical-align: top;">';
            shtml +=  '' + dt.Rows[0].BenefAddress + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<span><span style="font-size: 10pt"><strong>Telephone Number:</strong> ' + dt.Rows[0].BenefTelNo + '</span></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="vertical-align: top;  width: 100%; height: 20px; border-bottom: black thin solid;">';
            shtml +=  '<table style="font-size: 10pt; width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 10pt; text-decoration: underline">TRANSACTION DETAILS</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 10pt">';
            shtml +=  '<td colspan="3" rowspan="2" style="font-size: 10pt">';
            shtml +=  '<table style="font-size: 10pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Beneficiarys Account No. : </strong>' + dt.Rows[0].BenefAcctNo + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Thru PNB Branch: </strong>' + dt.Rows[0].ThruPNBBranch + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Other Banks Name / Branch: </strong>' + dt.Rows[0].OtherBankName + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Trans Type/Products: </strong>' + dt.Rows[0].TransType + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Source of Funds: </strong>' + dt.Rows[0].SourceFunds + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Member ID:</strong>' + dt.Rows[0].MemberID + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Purpose of Remittance: </strong>' + dt.Rows[0].PurposeRemittance + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Remittance Transaction Source: </strong>' + dt.Rows[0].RemittanceTranxSource + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 10pt">';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 10pt">';
            shtml +=  '<td rowspan="1" style="border-top-width: thin; border-left-width: thin; border-left-color: black;';
            shtml +=  'border-bottom-width: thin; border-bottom-color: black; vertical-align: top; width: 100%;';
            shtml +=  'border-top-color: black; height: 20px; border-right-width: thin; border-right-color: black">';
            shtml +=  '<table style="width: 100%; height: 20px;font-size: 10pt;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px;font-size: 10pt;">';
            shtml +=  '<p style="text-align: center">';
            shtml +=  '<span style="font-size: 10pt; text-decoration: underline"><strong>CLIENT/CUSTOMER UNDERTAKING</strong></span></p>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="2" style="text-align: justify; line-height: 18pt">';
            shtml +=  '<table style="width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="3" style="text-align: justify;font-size: 10pt;line-height: 18pt">';
            shtml +=  '<br />';
            shtml +=  'I certify that all the information contained herein is true and correct, and that';
            shtml +=  'I am amenable to the Terms and Conditions (if any) governing this transaction.';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<br />';
            shtml +=  '<br />';
            shtml +=  '<hr style="width: 378px; height: 1px" />';
            shtml +=  '<div style="width: 100%; height: 25px; text-align: center">';
            shtml +=  'Remitter&#039;s Signature</div>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '<td colspan="2" style="border-right: black thin solid; border-top: black thin solid;';
            shtml +=  'vertical-align: top; border-left: black thin solid; width: 50%; border-bottom: black thin solid;';
            shtml +=  'height: 20px">';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 0px; height: 90px">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 86px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="font-size: 10pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 18px; text-align: center">';
            shtml +=  '<span style="font-size: 10pt; text-decoration: underline"><strong>REMITTANCE DETAILS</strong></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td style="text-align: left; width: 245px; height: 15px;">';
	        shtml +=  '    <strong>';
	        shtml +=  'Net Amount Due to Beneficiary:</strong></td>';
	        shtml +=  '<td style="width: 58px; height: 15px">';
	        if (dt.Rows[0].NetAmountDueToBenefCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].NetAmountDueToBenefCurrCode + '</td>';
	        }
	        shtml +=  '<td style="height: 15px; text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].NetAmountDueToBenefAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Exchange Rate Used:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].ExchangeRateUsedCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].ExchangeRateUsedCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].ExchangeRateUsedAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Remittance Amount:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].RemittanceAmountCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].RemittanceAmountCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].RemittanceAmountAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Cable:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].CableCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].CableCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].CableAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Remittance Fee:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].RemittanceFeeCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].RemittanceFeeCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].RemittanceFeeAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Other Charges:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].OtherChargesCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].OtherChargesCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].OtherChargesAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="width: 221px">';
	        shtml +=  '<strong>';
	        shtml +=  'Total amount received from customer:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].TotalAmtRcvdFromCustCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].TotalAmtRcvdFromCustCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].TotalAmtRcvdFromCustAmt,2) + '</td>';
	        shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 70px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;font-size:10pt;">';
	        shtml +=  '' + dt.Rows[0].Check + '<br />';
	        shtml +=  '' + dt.Rows[0].Cash + '<br />';
	        shtml +=  '' + dt.Rows[0].SpecialMessage + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="font-size:10pt;width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 40px; text-align: center; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 177px;font-size:10pt">';
            shtml +=  '<strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Payment ';
            shtml +=  '<br />';
            shtml +=  'Received / Processed By:</strong></td>';
            shtml +=  '<td style="font-size:10pt;height:30px;">';
            shtml +=  '' + dt.Rows[0].ProcessedBy + '<br />';
            shtml +=  '<hr style="width: 200px; height: 2px;background-color:black;" />';
            shtml +=  '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Authorized Personnel&nbsp;</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="border-top-width: thin; border-left-width: thin; border-left-color: black;';
            shtml +=  'border-bottom-width: thin; border-bottom-color: black; vertical-align: top; width: 100%;';
            shtml +=  'border-top-color: black; height: 20px; text-align: center; border-right-width: thin;';
            shtml +=  'border-right-color: black">';
            shtml +=  '<table style="width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px" align="center">';
            shtml +=  '<span style="font-size: 10pt; text-decoration: underline"><strong>DECLARATIONS</strong></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            if (dt.Rows[0].ReceiptDeclarationSub5.toString().length == 0)
            {
            shtml +=  '<td colspan="3" rowspan="2" style="font-size: 8pt;text-align: justify; line-height: 18pt;">';
            }else{
            shtml +=  '<td colspan="3" rowspan="2" style="font-size: 7pt;text-align: justify; line-height: 18pt;">';
            }
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub1 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub2 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub3 + '<br />';   
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub4 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub5 + '<br />';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
         

        return shtml;
    }
    
    this.half_page = function(dt)
    {
        var shtml = "";
        shtml +=  '<table style="width: 8.5in">';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" style="border-right: black thin dotted; border-top: black thin dotted; border-left: black thin dotted; border-bottom: black thin dotted">';
        shtml +=  '<table style="height: 0%">';
        shtml +=  '<tr>';
        shtml +=  '<td style="font-size: 10pt; width: 371px; color: black; font-family: arial; height: 22px;';
        shtml +=  'text-align: center">';
        shtml +=  '<span style="font-size: 8pt; text-decoration: underline"><span style="font-size: 8.5pt;';
        shtml +=  'text-decoration: underline"><strong>IMPORTANT REMINDERS TO CLIENT / CUSTOMER</strong></span></span></td>';
        shtml +=  '<td rowspan="3" style="font-size: 6.5px; vertical-align: top; width: 50%; color: black;';
        shtml +=  'font-family: arial; height: 101px; text-align: left">';
        shtml +=  '<table style="font-size: 6.5pt; width: 100%; height: 23px">';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" >';
        shtml +=  'To the PNB __________________ , Branch: _____________ Date____________: This is';
        shtml +=  'to authorize your office to change my (pls. mark check the box/es below as applicable),';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" nowrap>';
        shtml +=  '<input id="Checkbox7" style="width: 26px; height: 12px" type="checkbox" />Remitter';
        shtml +=  'Name to _______________________________________________________</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" nowrap>';
        shtml +=  '<input id="Checkbox8" style="width: 26px; height: 12px" type="checkbox" />Address';
        shtml +=  '&nbsp;to _______________________________________ Postal Code: _________</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" nowrap>';
        shtml +=  '<input id="Checkbox9" style="width: 26px; height: 12px" type="checkbox" />Telephone';
        shtml +=  'No to: __________________________ Mobile No to: _________________</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" nowrap>';
        shtml +=  '<input id="Checkbox10" style="width: 26px; height: 12px" type="checkbox" />Beneficiary';
        shtml +=  '&nbsp;Name to: ________________________________Postal Code: _______</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" nowrap>';
        shtml +=  '<input id="Checkbox11" style="width: 26px; height: 12px" type="checkbox" />Beneficiary';
        shtml +=  'Address To: _________________________________________________</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" nowrap>';
        shtml +=  '<input id="Checkbox12" style="width: 26px; height: 12px" type="checkbox" />Beneficiary';
        shtml +=  'Telephone No to: ___________________Mobile No to: ______________</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" nowrap>';
        shtml +=  '<input id="Checkbox13" style="width: 26px; height: 12px" type="checkbox" />Others';
        shtml +=  '(please specify): _________________________________________________</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3">';
        shtml +=  'To support the request for ammendment, photocopy of the following document/s or';
        shtml +=  'ID/s is/are provided (*should match the document/s or ID/s required in No.2 of important';
        shtml +=  'Reminders to Client / Customer):';
        shtml +=  '<br />';
        shtml +=  'A) _______________&nbsp; B_________________ C)_________________ D)_______________</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3">';
        shtml +=  '<table style="width: 100%">';
        shtml +=  '<tr>';
        shtml +=  '<td rowspan="3" style="font-size: 8pt;border-right: black thin solid; border-top: black thin solid;';
        shtml +=  'border-left: black thin solid; border-bottom: black thin solid">';
        shtml +=  'PNB Use Only:<br />';
        shtml +=  'Processed By____Date:___</td>';
        shtml +=  '<td colspan="2" rowspan="3" style="font-size: 8pt;text-align: center">';
        shtml +=  '<hr style="width: 234px; height: 1px" />';
        shtml +=  '&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; Remitter Signature Over Printed Name</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td rowspan="2" style="font-size: 10px; vertical-align: top; width: 371px; color: black;';
        shtml +=  'font-family: arial; height: 113px; text-align: left; line-height: 13pt;">';
        shtml +=  '<span style="font-size: 8pt">1) Please notify the bank of any change in your name,';
        shtml +=  'address, telephone number or other details pertaining to your remittance account';
        shtml +=  '/ record by mailing / submitting the following: a) Photocopy of valid document/s';
        shtml +=  'or ID/s to support the amendment.';
        shtml +=  '<br />';
        shtml +=  'b) Duly accomplished and signed " Authorization and Notice of Amendment" cutout';
        shtml +=  'found on the opposite bottom right corner of this receipt. 2) Mailing address should';
        shtml +=  'tally with the clients registered address shown in the valid document/s or ID/s,';
        shtml +=  'as any irregularity in the registered address may result in non - implementation';
        shtml +=  'of remittance. The valid document/s or ID/s are the following*:';
        shtml +=  '<br />';
        shtml +=  '</span>';
        shtml +=  '<table style="font-size: 8pt; width: 303px; height: 45px">';
        shtml +=  '<tr>';
        shtml +=  '<td style="width: 50%; border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; border-bottom: black thin solid;">';
        shtml +=  'A) ' + dt.Rows[0].ID1 + '</td>';
        shtml +=  '<td style="width: 50%; border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; border-bottom: black thin solid;">';
        shtml +=  'B) ' + dt.Rows[0].ID2 + '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td style="width: 50%; border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; border-bottom: black thin solid;">';
        shtml +=  'C) ' + dt.Rows[0].ID3 + '</td>';
        shtml +=  '<td style="width: 50%; border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; border-bottom: black thin solid;">';
        shtml +=  'D) ' + dt.Rows[0].ID4 + '</td>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '<span style="font-size: 8pt">3) Always provide the bank with clear and proper instructions';
        shtml +=  'as to beneficiary details and remittance amount to facilitate processing of remittance.';
        shtml +=  '</span>';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr style="font-size: 10pt">';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        return shtml;
    }
    this.Receipt_body_halfpage = function(dt)
    {
    var vd = window.location.href.split('/')[3];
    
    var shtml = "";
            shtml +=  '<div>';
            shtml +=  '<table style="width: 8.5in; font-size: 8.5pt; font-family: arial;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="width: 0in">';
            shtml +=  '<table style="width: 100%;font-size: 8.5pt">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 60px; height: 15px">';
            shtml +=  '<img src="../../../'+ vd +'/images/pnb_black_logo.JPG" style="width: 58px; height: 68px" /></td>';
            shtml +=  '<td style="vertical-align: top; width: 520px; height: 15px">';
            shtml +=  '<strong>' + dt.Rows[0].Header1 + '</strong><br />';
            shtml +=  '' + dt.Rows[0].Header2 + '<br />';
            shtml +=  '<strong>' + dt.Rows[0].NameofBranch + '</strong><br />';
            shtml +=  '' + dt.Rows[0].AdressofBranch + '<br />';
            shtml +=  '' + dt.Rows[0].BranchofContact + '<br />';
            shtml +=  '' + dt.Rows[0].BranchofEmail + '<br />';
            shtml +=  '' + dt.Rows[0].corp_address + '<br />';
            shtml +=  '' + dt.Rows[0].corp_telno + '</td>';
            shtml +=  '<td style="vertical-align: top; height: 15px">';
            shtml +=  'Transaction Date: ' + HTMLRep.formatDate(dt.Rows[0].TranxDate,"/") + '<br />';
	        if (dt.Rows[0].ClearingDate == '1900')
	        {
	        shtml +=  'Clearing Date: <br />';
	        }else{
	        shtml +=  'Clearing Date: '+ dt.Rows[0].ClearingDate +'<br />';
	        }
            shtml +=  'Time: '+ HTMLRep.formatTime(dt.Rows[0].TranxDate) +'<br />';
            shtml +=  'Loyalty Points:';
            shtml +=  '<br />';
            shtml +=  '' + dt.Rows[0].TranStatus + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 8.5in; font-size: 8pt; font-family: arial;';
            shtml +=  'height: 0.23in; border-top-width: thin; border-left-width: thin; border-left-color: black; border-bottom-width: thin; border-bottom-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="border-right: black thin solid; border-top: black thin solid;';
            shtml +=  'border-left: black thin solid; border-bottom: black thin solid">';
            shtml +=  '<table style="font-size: 8.5pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 244px; height: 14px">';
            shtml +=  '<strong>Remitter ID No.: </strong>' + dt.Rows[0].RemitterID + '</td>';
            shtml +=  '<td style="width: 285px; height: 14px">';
            shtml +=  '<strong>Official Receipt No.:</strong> ' + dt.Rows[0].ReceiptNo + '</td>';
            shtml +=  '<td style="width: 257px; height: 14px">';
            shtml +=  '<strong>Reference No.:</strong> ' + dt.Rows[0].ReferenceNo + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';    
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td style="border-right: black thin solid; border-top: black thin solid; vertical-align: top;';
            shtml +=  'border-left: black thin solid; width: 50%; border-bottom: black thin solid; height: 20px">';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 0px; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="vertical-align: top; width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 20px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 8.5pt; text-decoration: underline">REMITTER INFO</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="2">';
            shtml +=  '<span><span style="font-size: 8.5pt"><strong>&nbsp;Name: </strong>' + dt.Rows[0].RemitterName + '<br />';
            shtml +=  '</span></span>';
            shtml +=  '<table style="font-size: 8.5pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="width: 61px; vertical-align: top;">';
            shtml +=  '<strong style="width: 30px">Address:<br />';
            shtml +=  '</strong></td>';
            shtml +=  '<td colspan="2" rowspan="3" style="height: 50px; vertical-align: top;">';
            shtml +=  '' + dt.Rows[0].RemitterAddress + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<span><span style="font-size: 8.5pt; width: 100%"><strong>Telephone Number:</strong>';
            shtml +=  '' + dt.Rows[0].RemitterTelNo + '</span></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="vertical-align: top; width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 20px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 100% ; height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 10pt; text-decoration: underline">BENEFICIARY INFO</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="2">';
            shtml +=  '<span><span style="font-size: 10pt"><strong>&nbsp;Name:</strong> ' + dt.Rows[0].BenefName + '<br />';
            shtml +=  '</span></span>';
            shtml +=  '<table style="font-size: 10pt; width: 100%; vertical-align: top;">';
            shtml +=  '<tr>';
            shtml +=  '<td  style="width: 60px; vertical-align: top;"><strong style="width: 30px">Address:<br />';
            shtml +=  '</strong>';
            shtml +=  '</td>';
            shtml +=  '<td rowspan="3" style="width: 90%; height: 50px; vertical-align: top;">';
            shtml +=  '' + dt.Rows[0].BenefAddress + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<span><span style="font-size: 8.5pt"><strong>Telephone Number:</strong> ' + dt.Rows[0].BenefTelNo + '</span></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="vertical-align: top;  width: 100%; height: 20px; border-bottom: black thin solid;">';
            shtml +=  '<table style="font-size: 8.5pt; width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 8.5pt; text-decoration: underline">TRANSACTION DETAILS</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 8.5pt">';
            shtml +=  '<td colspan="3" rowspan="2" style="font-size: 8.5pt">';
            shtml +=  '<table style="font-size: 8.5pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Beneficiarys Account No. : </strong>' + dt.Rows[0].BenefAcctNo + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Thru PNB Branch: </strong>' + dt.Rows[0].ThruPNBBranch + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Other Banks Name / Branch: </strong>' + dt.Rows[0].OtherBankName + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Trans Type/Products: </strong>' + dt.Rows[0].TransType + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Source of Funds: </strong>' + dt.Rows[0].SourceFunds + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Member ID:</strong>' + dt.Rows[0].MemberID + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Purpose of Remittance: </strong>' + dt.Rows[0].PurposeRemittance + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Remittance Transaction Source: </strong>' + dt.Rows[0].RemittanceTranxSource + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 8.5pt">';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 8.5pt">';
            shtml +=  '<td rowspan="1" style="border-top-width: thin; border-left-width: thin; border-left-color: black;';
            shtml +=  'border-bottom-width: thin; border-bottom-color: black; vertical-align: top; width: 100%;';
            shtml +=  'border-top-color: black; height: 20px; border-right-width: thin; border-right-color: black">';
            shtml +=  '<table style="width: 100%; height: 20px;font-size: 8.5pt;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px">';
            shtml +=  '<p style="text-align: center">';
            shtml +=  '<span style="font-size: 8.5pt; text-decoration: underline"><strong>CLIENT/CUSTOMER UNDERTAKING</strong></span></p>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="2" style="text-align: justify; line-height: 14pt;">';
            shtml +=  '<table style="width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="3" style="text-align: justify;font-size: 8.5pt;line-height: 14pt;">';                       
            shtml +=  'I certify that all the information contained herein is true and correct, and that ';
            shtml +=  'I am amenable to the Terms and Conditions (if any) governing this transaction.';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<br />';
            shtml +=  '<hr style="width: 378px; height: 1px" />';
            shtml +=  '<div style="width: 100%; height: 25px; text-align: center">';
            shtml +=  'Remitter&#039;s Signature</div>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '<td colspan="2" style="border-right: black thin solid; border-top: black thin solid;';
            shtml +=  'vertical-align: top; border-left: black thin solid; width: 50%; border-bottom: black thin solid;';
            shtml +=  'height: 20px">';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 0px; height: 90px">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 86px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="font-size: 8.5pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 18px; text-align: center">';
            shtml +=  '<span style="font-size: 8.5pt; text-decoration: underline"><strong>REMITTANCE DETAILS</strong></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td style="text-align: left; width: 245px; height: 15px;">';
	        shtml +=  '    <strong>';
	        shtml +=  'Net Amount Due to Beneficiary:</strong></td>';
	        shtml +=  '<td style="width: 58px; height: 15px">';
	        if (dt.Rows[0].NetAmountDueToBenefCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].NetAmountDueToBenefCurrCode + '</td>';
	        }
	        shtml +=  '<td style="height: 15px; text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].NetAmountDueToBenefAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Exchange Rate Used:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].ExchangeRateUsedCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].ExchangeRateUsedCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].ExchangeRateUsedAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Remittance Amount:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].RemittanceAmountCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].RemittanceAmountCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].RemittanceAmountAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Cable:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].CableCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].CableCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].CableAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Remittance Fee:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].RemittanceFeeCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].RemittanceFeeCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].RemittanceFeeAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Other Charges:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].OtherChargesCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].OtherChargesCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].OtherChargesAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="width: 221px">';
	        shtml +=  '<strong>';
	        shtml +=  'Total amount received from customer:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].TotalAmtRcvdFromCustCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].TotalAmtRcvdFromCustCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].TotalAmtRcvdFromCustAmt,2) + '</td>';
	        shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 70px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;font-size: 8pt">';
	        shtml +=  '' + dt.Rows[0].Check + '<br />';
	        shtml +=  '' + dt.Rows[0].Cash + '<br />';
	        shtml +=  '' + dt.Rows[0].SpecialMessage + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="font-size:8.5pt;width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 40px; text-align: center; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 177px;font-size:8.5pt">';
            shtml +=  '<strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Payment ';
            shtml +=  '<br />';
            shtml +=  'Received / Processed By:</strong></td>';
            shtml +=  '<td style="font-size:8.5pt;">';
            shtml +=  '' + dt.Rows[0].ProcessedBy + '<br />';
            shtml +=  '<hr style="width: 200px; height: 2px;background-color:black;" />';
            shtml +=  '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Authorized Personnel&nbsp;</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="border-top-width: thin; border-left-width: thin; border-left-color: black;';
            shtml +=  'border-bottom-width: thin; border-bottom-color: black; vertical-align: top; width: 100%;';
            shtml +=  'border-top-color: black; height: 20px; text-align: center; border-right-width: thin;';
            shtml +=  'border-right-color: black">';
            shtml +=  '<table style="width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px;text-align:center;">';
            shtml +=  '<span style="font-size: 8.5pt; text-decoration: underline"><strong>DECLARATIONS</strong></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            if (dt.Rows[0].ReceiptDeclarationSub5.toString().length == 0)
            {
            shtml +=  '<td colspan="3" rowspan="2" style="font-size: 8pt;text-align: justify; line-height: 14pt;">';
            }else{
            shtml +=  '<td colspan="3" rowspan="2" style="font-size: 7pt;text-align: justify; line-height: 14pt;">';
            }            
            
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub1 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub2 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub3 + '<br />';   
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub4 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub5 + '<br />';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</div>';

            return shtml;
    }
}

/* START preprinted OBP Receipt REPORT */
function HTMLPrePrintedOBPReceiptReport()
{
    var cls = this;
    var _branchcode;    
    var _seqid;
    var _referenceno;
    
    this.evaluate_PrePrintedReceipt_query = function(query)
    {
        var squery = unescape(query.toString());

        var squery = unescape(query.toString());
        if (squery.toLowerCase().indexOf("trantype=obp") > -1)
        {
            _branchcode = HTMLRep.get_branchcode();
            this.get_PrePrintedReceipt_data(
                                    HTMLRep.get_querystring_value(query,"tranvalue"),
                                    HTMLRep.get_branchcode(),
                                    ""
                                 )
            return;
        }
        if (squery.toLowerCase().indexOf("mergeparam") > -1)
        {
             _branchcode = HTMLRep.parse_merge_param_query(squery,"T@v_branchcode")
             this.get_PrePrintedReceipt_data(
                                    "",
                                    HTMLRep.parse_merge_param_query(squery,"T@v_branchcode"),
                                    HTMLRep.parse_merge_param_query(squery,"T@v_referenceno")
									)
            return;
        }
    }
    this.get_PrePrintedReceipt_data = function(seq_id,branchcode,referenceno)
    {
        var params; 
        var sp_name = "";
        
                _branchcode =   " @v_branchcode="  + branchcode;    
                _seqid    =   "@v_seqid="     + seq_id;
		_referenceno = " @v_referenceno="     + referenceno;
		
		
	if(seq_id.length == 0)
	{
			sp_name = "sp_otc_obp_Receipt_reprint_preprinted";
			params  = _branchcode + "^" + _referenceno ;
	}
	else{
			sp_name = "sp_otc_obp_Receipt_preprinted";
			params  = _branchcode + "^" + _seqid ;
	}
	AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
            alert(errmsg);       return;      
        } 
	    IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params,this.get_PrePrintedReceipt_callback);
    }
    
    this.get_PrePrintedReceipt_callback = function(result)
    {
        try
        {
            var ds = result.value;
            cls.display_PrePrintedReceipt_report(ds);
            
        }
        catch(e)
        {
            alert("ERROR WHILE RECEIPT DATA");
        }
    }
    
    this.display_PrePrintedReceipt_report = function(ds)
    {
        var shtml = "";        
        
        shtml   +=  HTMLRep.print_html_button();
        shtml   +=  this.PrePrintedReceipt_body(ds.Tables[0]);
        
        document.getElementById("bodydiv").innerHTML = shtml
        //this.Fill_text(ds.Tables[0]);      
		
    }
    this.PrePrintedReceipt_body = function(dt)
    {
    var vd = window.location.href.split('/')[3];
	    var shtml = "";
        shtml +=  '    <div>';
        shtml +=  '        &nbsp;';
        shtml +=  '        <div style="left: -429px; width: 100px; position: absolute; top: 45px; height: 100px">';
        shtml +=  '        </div>';
        shtml +=  '        &nbsp;';
        shtml +=  '        <div style="width: 8in; height: 11in">';
        shtml +=  '            &nbsp;';
        shtml +=  '            <div style="left: 613px; width: 92px; position: absolute; top: 36px; height: 21px">';
        shtml +=  '                <span style="font-size: 9pt; font-family: Arial">Clearing Date:</span></div>';
        shtml +=  '            &nbsp;';
        shtml +=  '            <div style="font-size: 12pt; left: 113px; width: 434px; position: absolute; top: 167px;';
        shtml +=  '                height: 21px">';
        shtml +=  '                <span style="font-size: 10pt; font-family: Arial">' +  dt.Rows[0].RemitterName + '</span></div>';
        shtml +=  '            <div style="font-size: 12pt; left: 708px; width: 131px; position: absolute; top: 37px;';
        shtml +=  '                height: 21px">';
        shtml +=  '                <span style="font-size: 9pt; font-family: Arial">' +  dt.Rows[0].ClearingDate + '</span></div>';
        shtml +=  '            <div style="left: 708px; width: 133px; position: absolute; top: 20px; height: 13px">';
        shtml +=  '                <span style="font-size: 9pt; font-family: Arial">' +  HTMLRep.formatDate(dt.Rows[0].TranxDate,"/") + '</span></div>';
        shtml +=  '            <div style="left: 115px; width: 429px; position: absolute; top: 229px; height: 139px">';
        shtml +=  '                <span style="font-size: 10pt; font-family: Arial">' +  dt.Rows[0].RemitterAddress + '</span></div>';
        shtml +=  '            &nbsp;';
        shtml +=  '            <div style="left: 128px; width: 405px; position: absolute; top: 437px; height: 21px">';
        shtml +=  '                <span style="font-size: 9pt; font-family: Arial">' +  dt.Rows[0].BenefName + '</span></div>';
        shtml +=  '            <div style="left: 129px; width: 329px; position: absolute; top: 478px; height: 48px">';
        shtml +=  '                <span style="font-size: 9pt; font-family: Arial">' +  dt.Rows[0].BenefAddress + '</span></div>';
        shtml +=  '            <div style="left: 136px; width: 318px; position: absolute; top: 534px; height: 61px">';
        shtml +=  '                <span style="font-size: 9pt; font-family: Arial">' +  dt.Rows[0].SpecialMessage + '</span></div>';
        shtml +=  '            <div style="left: 137px; width: 316px; position: absolute; top: 602px; height: 21px">';
        shtml +=  '                <span style="font-size: 9pt; font-family: Arial">' +  dt.Rows[0].OtherBankName + '</span></div>';
        shtml +=  '            <div style="left: 138px; width: 314px; position: absolute; top: 642px; height: 21px">';
        shtml +=  '                <span style="font-size: 9pt; font-family: Arial">' +  dt.Rows[0].ThruPNBBranch + '</span></div>';
        shtml +=  '            <div style="left: 140px; width: 390px; position: absolute; top: 719px; height: 21px">';
        shtml +=  '                <span style="font-size: 9pt; font-family: Arial">' +  dt.Rows[0].TransType + '</span></div>';
        shtml +=  '            <div style="left: 140px; width: 392px; position: absolute; top: 745px; height: 21px">';
        shtml +=  '                <span style="font-size: 9pt; font-family: Arial">' +  dt.Rows[0].SourceFunds + '</span></div>';
        shtml +=  '            <div style="left: 563px; width: 122px; position: absolute; top: 181px; height: 21px;';
        shtml +=  '                text-align: justify">';
        shtml +=  '                <span style="font-size: 9pt; font-family: Arial">( &nbsp;' + dt.Rows[0].ExchangeRateUsedCurrCode + '&nbsp; )</span></div>';
        shtml +=  '            <div style="left: 557px; width: 122px; position: absolute; top: 161px; height: 21px;';
        shtml +=  '                text-align: right">';
        shtml +=  '                <span style="font-size: 9pt; font-family: Arial">' +  HTMLRep.format_numbers(dt.Rows[0].NetAmountDueToBenefAmt,2) + '</span></div>';
        shtml +=  '            <div style="left: 753px; width: 100px; position: absolute; top: 158px; height: 21px;';
        shtml +=  '                text-align: right">';
        shtml +=  '                <span style="font-size: 9pt; font-family: Arial">' +  dt.Rows[0].NetAmountDueToBenefCurrCode + '</span></div>';
        shtml +=  '            <div style="left: 556px; width: 122px; position: absolute; top: 198px; height: 21px;';
        shtml +=  '                text-align: right">';
        shtml +=  '                <span style="font-size: 9pt; font-family: Arial">' +  dt.Rows[0].RemittanceAmountCurrCode + '</span></div>';
        shtml +=  '            <div style="left: 753px; width: 100px; position: absolute; top: 197px; height: 21px;';
        shtml +=  '                text-align: right">';
        shtml +=  '                <span style="font-size: 9pt; font-family: Arial">' +  HTMLRep.format_numbers(dt.Rows[0].RemittanceAmountAmt,2) + '</span></div>';
        shtml +=  '            <div style="left: 613px; width: 122px; position: absolute; top: 260px; height: 21px;';
        shtml +=  '                text-align: right">';
        shtml +=  '                <span style="font-size: 9pt; font-family: Arial">' +  dt.Rows[0].TotalAmtRcvdFromCustCurrCode  + '</span></div>';
        shtml +=  '            <div style="left: 612px; width: 122px; position: absolute; top: 292px; height: 21px;';
        shtml +=  '                text-align: right">';
        shtml +=  '                <span style="font-size: 9pt; font-family: Arial"></span></div>';
        shtml +=  '            <div style="left: 557px; width: 122px; position: absolute; top: 334px; height: 21px;';
        shtml +=  '                text-align: right">';
        shtml +=  '                <span style="font-size: 9pt; font-family: Arial">' +  dt.Rows[0].OtherChargesCurrCode + '</span></div>';
        shtml +=  '            <div style="left: 467px; width: 100px; position: absolute; top: 516px; height: 17px;';
        shtml +=  '                text-align: left">';
        shtml +=  '                <span style="font-size: 9pt; font-family: Arial">Denominations:</span></div>';
        shtml +=  '            <div style="left: 478px; width: 153px; position: absolute; top: 534px; height: 21px;';
        shtml +=  '                text-align: left" id="DIV1">';
        shtml +=  '                <span style="font-size: 9pt; font-family: Arial">CASH: ' +  dt.Rows[0].Cash + '</span></div>';
        shtml +=  '            <div style="left: 478px; width: 155px; position: absolute; top: 566px; height: 21px;';
        shtml +=  '                text-align: left">';
        shtml +=  '                <span style="font-size: 9pt; font-family: Arial">CHECK: ' +  dt.Rows[0].Check + '</span></div>';
        shtml +=  '            <div style="left: 493px; width: 100px; position: absolute; top: 475px; height: 21px;';
        shtml +=  '                text-align: left">';
        shtml +=  '                <span style="font-size: 9pt; font-family: Arial">' +  dt.Rows[0].Check + '</span></div>';
        shtml +=  '            <div style="font-size: 12pt; left: 309px; width: 262px; font-family: Times New Roman;';
        shtml +=  '                position: absolute; top: 28px; height: 21px; text-align: center">';
        shtml +=  '                <strong>' +  dt.Rows[0].TranStatus + '</strong></div>';
        shtml +=  '            <div style="font-size: 12pt; left: 120px; width: 262px; font-family: Times New Roman;';
        shtml +=  '                position: absolute; top: 54px; height: 21px">';
         shtml +=  '                <span style="font-size: 10pt; font-family: Arial">' +  dt.Rows[0].RemitterID + '</span></div><div style="left: 128px; width: 409px; position: absolute; top: 408px; height: 21px">';
        shtml +=  '                    <span style="font-size: 9pt; font-family: Arial">' +  dt.Rows[0].BenefCIFID + '</span></div>';
        shtml +=  '                    <div style="left: 140px; width: 390px; position: absolute; top: 692px; height: 21px">';
        shtml +=  '                        <span style="font-size: 9pt; font-family: Arial">' +  dt.Rows[0].BenefAcctNo + '</span></div>';
        shtml +=  '                        <div style="left: 141px; width: 392px; position: absolute; top: 774px; height: 21px">';
        shtml +=  '                            <span style="font-size: 9pt; font-family: Arial">' +  dt.Rows[0].BenefTelNo + '</span></div><div style="left: 117px; width: 425px; position: absolute; top: 373px; height: 21px">';
        shtml +=  '                                <span style="font-size: 9pt; font-family: Arial">' +  dt.Rows[0].RemitterTelNo + '</span></div>';
        shtml +=  '        </div>';
        shtml +=  '        <div style="font-size: 12pt; left: 754px; width: 100px; font-family: Times New Roman;';
        shtml +=  '            position: absolute; top: 180px; height: 21px; text-align: right">';
        shtml +=  '            <span style="font-size: 9pt; font-family: Arial">' +  dt.Rows[0].ExchangeRateUsedAmt + '</span></div>';
        shtml +=  '        <div style="left: 737px; width: 100px; position: absolute; top: 533px; height: 21px;';
        shtml +=  '            text-align: right">';
        shtml +=  '            <span style="font-size: 9pt; font-family: Arial">' +  dt.Rows[0].Ten + '</span></div>';
        shtml +=  '        <div style="left: 738px; width: 100px; position: absolute; top: 570px; height: 21px;';
        shtml +=  '            text-align: right">';
        shtml +=  '            <span style="font-size: 9pt; font-family: Arial">' +  dt.Rows[0].Two + '</span></div>';
        shtml +=  '        <div style="left: 738px; width: 100px; position: absolute; top: 550px; height: 21px;';
        shtml +=  '            text-align: right">';
        shtml +=  '            <span style="font-size: 9pt; font-family: Arial">' +  dt.Rows[0].Five + '</span></div>';
        shtml +=  '        <div style="left: 738px; width: 100px; position: absolute; top: 515px; height: 21px;';
        shtml +=  '            text-align: right">';
        shtml +=  '            <span style="font-size: 9pt; font-family: Arial">' +  dt.Rows[0].Twenty + '</span></div>';
        shtml +=  '        <div style="left: 737px; width: 100px; position: absolute; top: 590px; height: 21px;';
        shtml +=  '            text-align: right">';
        shtml +=  '            <span style="font-size: 9pt; font-family: Arial">' +  dt.Rows[0].One + '</span></div>';
        shtml +=  '        <div style="left: 738px; width: 100px; position: absolute; top: 501px; height: 21px;';
        shtml +=  '            text-align: right">';
        shtml +=  '            <span style="font-size: 9pt; font-family: Arial">' +  dt.Rows[0].Fifty + '</span></div>';
        shtml +=  '        <div style="left: 738px; width: 100px; position: absolute; top: 482px; height: 21px;';
        shtml +=  '            text-align: right">';
        shtml +=  '            <span style="font-size: 9pt; font-family: Arial">' +  dt.Rows[0].OneHund + '</span></div>';
        shtml +=  '        <div style="left: 738px; width: 100px; position: absolute; top: 464px; height: 21px;';
        shtml +=  '            text-align: right">';
        shtml +=  '            <span style="font-size: 9pt; font-family: Arial">' +  dt.Rows[0].FiveHund + '</span></div>';
        shtml +=  '        <div style="left: 738px; width: 100px; position: absolute; top: 445px; height: 21px;';
        shtml +=  '            text-align: right">';
        shtml +=  '            <span style="font-size: 9pt; font-family: Arial">' +  dt.Rows[0].OneThou + '</span></div>';
        shtml +=  '        &nbsp;';
        shtml +=  '        <div style="left: 676px; width: 183px; position: absolute; top: 777px; height: 21px;';
        shtml +=  '            text-align: right">';
        shtml +=  '            <span style="font-size: 9pt; font-family: Arial">' +  dt.Rows[0].ProcessedBy + '</span></div><div style="left: 738px; width: 100px; position: absolute; top: 433px; height: 21px;';
        shtml +=  '            text-align: right">';
        shtml +=  '                <span style="font-size: 9pt; font-family: Arial">' +  dt.Rows[0].TenThou + '</span></div>';
        shtml +=  '        <div style="left: 751px; width: 100px; position: absolute; top: 333px; height: 21px;';
        shtml +=  '            text-align: right">';
        shtml +=  '            <span style="font-size: 9pt; font-family: Arial">' +  HTMLRep.format_numbers(dt.Rows[0].OtherChargesAmt,2) + '</span></div>';
        shtml +=  '        <div style="3; width: 100px; position: absolute; top: 291px; height: 21px;';
        shtml +=  '            text-align: right">';
        shtml +=  '            <span style="font-size: 9pt; font-family: Arial">' +  HTMLRep.format_numbers(dt.Rows[0].TotalBackEndCharges,2) + '</span></div>';
        shtml +=  '        <div style="left: 752px; width: 100px; position: absolute; top: 259px; height: 21px;';
        shtml +=  '            text-align: right">';
        shtml +=  '            <span style="font-size: 9pt; font-family: Arial">' +  HTMLRep.format_numbers(dt.Rows[0].TotalAmtRcvdFromCustAmt,2)  + '</span></div>';
        shtml +=  '        <div style="left: 752px; width: 100px; position: absolute; top: 234px; height: 21px;';
        shtml +=  '            text-align: right">';
        shtml +=  '            <span style="font-size: 9pt; font-family: Arial">' +  HTMLRep.format_numbers(dt.Rows[0].CableAmt,2) + '</span></div>';
        shtml +=  '        <div style="left: 712px; width: 148px; position: absolute; top: 78px; height: 21px;';
        shtml +=  '            text-align: left">';
        shtml +=  '            <span style="font-size: 9pt; font-family: Arial">' +  dt.Rows[0].ReferenceNo + '</span></div>';
        shtml +=  '        &nbsp;';
        shtml +=  '        <div style="left: 613px; width: 122px; position: absolute; top: 234px; height: 21px;';
        shtml +=  '            text-align: right">';
        shtml +=  '            <span style="font-size: 9pt; font-family: Arial">' +  dt.Rows[0].CableCurrCode + '</span></div>';
        shtml +=  '    ';
        shtml +=  '    </div>';

     
    return shtml;
    }
    
   

}

/* END preprinted OBP Receipt REPORT */

/* END OBP Receipt */



/*START HONGKONG OTC RECEIPT -- ONE PAGE*/
function HTMLReceiptOTC_HK_OnePage()
{
    var cls = this;
    var _branchcode;    
    var _seqid;
	var _referenceno;
    
    this.evaluate_Receipt_query = function(query)
    {
        var squery = unescape(query.toString());
        if (squery.toLowerCase().indexOf("trantype=otc") > -1)
        {
            _branchcode = HTMLRep.get_branchcode();
            this.get_Receipt_data(
                                    HTMLRep.get_querystring_value(query,"tranvalue"),
                                    HTMLRep.get_branchcode(),
                                    ""
                                 )
            return;
        }
        if (squery.toLowerCase().indexOf("mergeparam") > -1)
        {
             _branchcode = HTMLRep.parse_merge_param_query(squery,"T@v_branchcode")
             this.get_Receipt_data(
                                    "",
                                    HTMLRep.parse_merge_param_query(squery,"T@v_branchcode"),
                                    HTMLRep.parse_merge_param_query(squery,"T@v_referenceno")
									)
            return;
        }
    }
    this.check_newdbswitch = function()
    {
    
	var pass = false;
	var sp_name = 'proc_check_newdbswitch';
        var squery = HTMLRep.get_hidden_querystring("hidQueryString");
        var sbranchcode = HTMLRep.get_branchcode();
        
        var params  =  '@branchcode=' + sbranchcode;
        var isRCC  = false;
        AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
            alert(errmsg);     return;        
        } 
        
        var dtcchecksw = IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params).value;
        
        try
        {
            if (dtcchecksw.Tables[0].Rows.length > 0)
            {
                pass = true;
            }
        }
        catch(e)
        {
        }
	
	return pass; 
    }
    this.get_Receipt_data = function(seq_id,branchcode,referenceno)
    {
        var params; 
        var sp_name = "";
	var pass = false;
        
                _branchcode =   " @v_branchcode="  + branchcode;    
                _seqid    =   "@v_seqid="     + seq_id;
				_referenceno = " @v_referenceno="     + referenceno;
        
       if(seq_id.length == 0)
	   {
			pass = this.check_newdbswitch();
			if (pass)
			{
			    sp_name = "SP_OTC_Reprint_Receipt_irsw";
			}
			else
			{
			    sp_name = "SP_OTC_Reprint_Receipt";
			}
			params  = _branchcode + "^" + _referenceno ;
	   }else{
			sp_name = "SP_OTC_Receipt";
			params  = _branchcode + "^" + _seqid ;
		}
		AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
            alert(errmsg);       return;      
        } 
	    IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params,this.get_Receipt_data_callback);
    }
    
    this.get_Receipt_data_callback = function(result)
    {
        try
        {
            var ds = result.value;
            
            cls.check_branch();
            cls.display_Receipt_report(ds);
            
            
        }
        catch(e)
        {
            alert("ERROR WHILE GETTING Receipt DATA");
        }
    }
    
    this.display_Receipt_report = function(ds)
    {
        var sp_name = 'proc_check_reporthalfpage';
        var squery = HTMLRep.get_hidden_querystring("hidQueryString");
        var sbranchcode = ds.Tables[0].Rows[0].ReferenceNo;
        sbranchcode =  sbranchcode.substring(1,4);
        var params  =  '@v_branhcode=' + sbranchcode;
        var ishalfpage  = false;
        AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
            alert(errmsg);     return;        
        } 
        
        var dtcheckhalfpage = IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params).value;
        
        try
        {
            if (dtcheckhalfpage.Tables[0].Rows.length > 0)
            {
                ishalfpage = true;
            }
        }
        catch(e)
        {
        }
    
        
        var shtml = "";        
        shtml +=  HTMLRep.print_html_button();
        var isRCC = cls.check_branch();
        if (!ishalfpage)
        {
           
            shtml +=  this.Receipt_body(ds.Tables[0],isRCC);
               
            shtml += cls.half_page(ds.Tables[0]);
            
	        shtml +=  '</div>';
        }
        else
        {
           shtml +=  this.Receipt_body_halfpage(ds.Tables[0],isRCC);
           
        }
        document.getElementById("bodydiv").innerHTML = shtml
    }
    this.check_branch = function()
    {
        var sp_name = 'sp_report_checkbranch';
        var squery = HTMLRep.get_hidden_querystring("hidQueryString");
        var sbranchcode = HTMLRep.get_branchcode();
        
        var params  =  '@branchcode=' + sbranchcode;
        var isRCC  = false;
        AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
            alert(errmsg);     return;        
        } 
        
        var dtccheckbranch = IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params).value;
        
        try
        {
            if (dtccheckbranch.Tables[0].Rows.length > 0)
            {
                isRCC = true;
            }
        }
        catch(e)
        {
        }
        return isRCC;
    }
    this.Receipt_body = function(dt,isRCC)
    {
	
	    var vd = window.location.href.split('/')[3];
	    
        var shtml = "";
	        shtml +=  '<div>';
            shtml +=  '<table style="width: 9in; font-size: 8pt; font-family: arial;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="width: 0in">';
            shtml +=  '<table style="width: 100%;font-size: 10pt;">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 60px; height: 15px">';
            shtml +=  '<img src="../../../'+ vd +'/images/pnb_black_logo.JPG" style="width: 58px; height: 68px" /></td>';
            shtml +=  '<td style="vertical-align: top; width: 520px; height: 15px">';
            shtml +=  '<strong>' + dt.Rows[0].Header1 + '</strong><br />';
            shtml +=  '' + dt.Rows[0].Header2 + '<br />';
            shtml +=  '<strong>' + dt.Rows[0].NameofBranch + '</strong><br />';
            shtml +=  '' + dt.Rows[0].AdressofBranch + '<br />';
            shtml +=  '' + dt.Rows[0].BranchofContact + '<br />';
            shtml +=  '' + dt.Rows[0].BranchofEmail + '<br />';
            shtml +=  '' + dt.Rows[0].corp_address + '<br />';
            shtml +=  '' + dt.Rows[0].corp_telno + '</td>';
            shtml +=  '<td style="vertical-align: top; height: 15px">';
            shtml +=  'Transaction Date: ' + HTMLRep.formatDate(dt.Rows[0].TranxDate,"/") + '<br />';
	        if (dt.Rows[0].ClearingDate == '1900')
	        {
	        shtml +=  'Clearing Date: <br />';
	        }else{
	        shtml +=  'Clearing Date: '+ dt.Rows[0].ClearingDate +'<br />';
	        }
            shtml +=  'Time: '+ HTMLRep.formatTime(dt.Rows[0].TranxDate) +'<br />';
            shtml +=  'Loyalty Points:';
            shtml +=  '<br />';
            shtml +=  '' + dt.Rows[0].TranStatus + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 9in; font-size: 10pt; font-family: arial;';
            shtml +=  'height: 0.23in; border-top-width: thin; border-left-width: thin; border-left-color: black; border-bottom-width: thin; border-bottom-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="border-right: black thin solid; border-top: black thin solid;';
            shtml +=  'border-left: black thin solid; border-bottom: black thin solid">';
            shtml +=  '<table style="font-size: 10pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 244px; height: 14px">';
            shtml +=  '<strong>Remitter ID No.: </strong>' + dt.Rows[0].RemitterID + '</td>';
            shtml +=  '<td style="width: 285px; height: 14px">';
            shtml +=  '<strong>Official Receipt No.:</strong> ' + dt.Rows[0].ReceiptNo + '</td>';
            shtml +=  '<td style="width: 257px; height: 14px">';
            shtml +=  '<strong>Reference No.:</strong> ' + dt.Rows[0].ReferenceNo + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';    
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td style="border-right: black thin solid; border-top: black thin solid; vertical-align: top;';
            shtml +=  'border-left: black thin solid; width: 50%; border-bottom: black thin solid; height: 20px">';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 0px; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="vertical-align: top; width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 20px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 10pt; text-decoration: underline">REMITTER INFO</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="2">';
            shtml +=  '<span><span style="font-size: 10pt"><strong>&nbsp;Name: </strong>' + dt.Rows[0].RemitterName + '<br />';
            shtml +=  '</span></span>';
            shtml +=  '<table style="font-size: 10pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="width: 61px; vertical-align: top;">';
            shtml +=  '<strong style="width: 30px">Address:<br />';
            shtml +=  '</strong></td>';
            shtml +=  '<td colspan="2" rowspan="3" style="height: 50px; vertical-align: top;">';
            shtml +=  '' + dt.Rows[0].RemitterAddress + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<span><span style="font-size: 10pt; width: 100%"><strong>Telephone Number:</strong>';
            shtml +=  '' + dt.Rows[0].RemitterTelNo + '</span></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="vertical-align: top; width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 20px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 100% ; height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 10pt; text-decoration: underline">BENEFICIARY INFO</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="2">';
            shtml +=  '<span><span style="font-size: 10pt"><strong>&nbsp;Name:</strong> ' + dt.Rows[0].BenefName + '<br />';
            shtml +=  '</span></span>';
            shtml +=  '<table style="font-size: 10pt; width: 100%; vertical-align: top;">';
            shtml +=  '<tr>';
            shtml +=  '<td  style="width: 60px; vertical-align: top;"><strong style="width: 30px">Address:<br />';
            shtml +=  '</strong>';
            shtml +=  '</td>';
            shtml +=  '<td rowspan="3" style="width: 90%; height: 50px; vertical-align: top;">';
            shtml +=  '' + dt.Rows[0].BenefAddress + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<span><span style="font-size: 10pt"><strong>Telephone Number:</strong> ' + dt.Rows[0].BenefTelNo + '</span></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="vertical-align: top;  width: 100%; height: 20px; border-bottom: black thin solid;">';
            shtml +=  '<table style="font-size: 10pt; width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 10pt; text-decoration: underline">TRANSACTION DETAILS</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 10pt">';
            shtml +=  '<td colspan="3" rowspan="2" style="font-size: 10pt">';
            shtml +=  '<table style="font-size: 10pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Beneficiarys Account No. : </strong>' + dt.Rows[0].BenefAcctNo + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Thru PNB Branch: </strong>' + dt.Rows[0].ThruPNBBranch + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Other Banks Name / Branch: </strong>' + dt.Rows[0].OtherBankName + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Trans Type/Products: </strong>' + dt.Rows[0].TransType + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Source of Funds: </strong>' + dt.Rows[0].SourceFunds + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Member ID:</strong>' + dt.Rows[0].MemberID + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Purpose of Remittance: </strong>' + dt.Rows[0].PurposeRemittance + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Remittance Transaction Source: </strong>' + dt.Rows[0].RemittanceTranxSource + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 10pt">';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 10pt">';
            shtml +=  '<td rowspan="1" style="border-top-width: thin; border-left-width: thin; border-left-color: black;';
            shtml +=  'border-bottom-width: thin; border-bottom-color: black; vertical-align: top; width: 100%;';
            shtml +=  'border-top-color: black; height: 20px; border-right-width: thin; border-right-color: black">';
            shtml +=  '<table style="width: 100%; height: 20px;font-size: 10pt;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px;font-size: 10pt;">';
            shtml +=  '<p style="text-align: center">';
            shtml +=  '<span style="font-size: 10pt; text-decoration: underline"><strong>CLIENT/CUSTOMER UNDERTAKING</strong></span></p>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="2" style="text-align: justify; line-height: 18pt">';
            shtml +=  '<table style="width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="3" style="text-align: justify;font-size: 10pt;line-height: 18pt">';
            shtml +=  '<br />';
            shtml +=  'I/We hereby agree that the PNB GLOBAL REMITTANCE AND FINANCIAL COMPANY (HK) ';
            shtml +=  'LIMITED reserves the right to, at any time, obtain further information/document ';
            shtml +=  'from me/us.';
            shtml +=  '<br/>';
            shtml +=  '<br/>';
            shtml +=  'I/We have read and understood the Notice to Customer relating to the Personal ';
            shtml +=  'Data (Privacy) Ordinance and all the terms and conditions printed overleaf.';
            shtml +=  '<br/>';
            shtml +=  '<br/>';
            shtml +=  'I also confirm that details of my beneficiary are correct.';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<br />';
            shtml +=  '<br />';
            shtml +=  '<hr style="width: 378px; height: 1px" />';
            shtml +=  '<div style="width: 100%; height: 25px; text-align: center">';
            shtml +=  'Remitter&#039;s Signature</div>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '<td colspan="2" style="border-right: black thin solid; border-top: black thin solid;';
            shtml +=  'vertical-align: top; border-left: black thin solid; width: 50%; border-bottom: black thin solid;';
            shtml +=  'height: 20px">';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 0px; height: 90px">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 86px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="font-size: 10pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 18px; text-align: center">';
            shtml +=  '<span style="font-size: 10pt; text-decoration: underline"><strong>REMITTANCE DETAILS</strong></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td style="text-align: left; width: 245px; height: 15px;">';
	        shtml +=  '    <strong>';
	        shtml +=  'Net Amount Due to Beneficiary:</strong></td>';
	        shtml +=  '<td style="width: 58px; height: 15px">';
	        if (dt.Rows[0].NetAmountDueToBenefCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].NetAmountDueToBenefCurrCode + '</td>';
	        }
	        shtml +=  '<td style="height: 15px; text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].NetAmountDueToBenefAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Exchange Rate Used:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].ExchangeRateUsedCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].ExchangeRateUsedCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].ExchangeRateUsedAmt,4) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Remittance Amount:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].RemittanceAmountCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].RemittanceAmountCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].RemittanceAmountAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Cable:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].CableCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].CableCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].CableAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Remittance Fee:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].RemittanceFeeCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].RemittanceFeeCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].RemittanceFeeAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Other Charges:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].OtherChargesCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].OtherChargesCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].OtherChargesAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="width: 221px">';
	        shtml +=  '<strong>';
	        shtml +=  'Total amount received from customer:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].TotalAmtRcvdFromCustCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].TotalAmtRcvdFromCustCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].TotalAmtRcvdFromCustAmt,2) + '</td>';
	        shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 70px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;font-size:10pt;">';
	        shtml +=  '' + dt.Rows[0].Check + '<br />';
	        shtml +=  '' + dt.Rows[0].Cash + '<br />';
	        shtml +=  '' + dt.Rows[0].SpecialMessage + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="font-size:10pt;width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 40px; text-align: center; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 177px;font-size:10pt">';
            shtml +=  '<strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Payment ';
            shtml +=  '<br />';
            shtml +=  'Received / Processed By:</strong></td>';
            shtml +=  '<td style="font-size:10pt;height:30px;">';
            shtml +=  '' + dt.Rows[0].ProcessedBy + '<br />';
            shtml +=  '<hr style="width: 200px; height: 2px;background-color:black;" />';
            shtml +=  '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Authorized Personnel&nbsp;</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="border-top-width: thin; border-left-width: thin; border-left-color: black;';
            shtml +=  'border-bottom-width: thin; border-bottom-color: black; vertical-align: top; width: 100%;';
            shtml +=  'border-top-color: black; height: 20px; text-align: center; border-right-width: thin;';
            shtml +=  'border-right-color: black">';
            shtml +=  '<table style="width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px" align="center">';
            shtml +=  '<span style="font-size: 10pt; text-decoration: underline"><strong>DECLARATIONS</strong></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            if (dt.Rows[0].ReceiptDeclarationSub5.toString().length == 0)
            {
            shtml +=  '<td colspan="3" rowspan="2" style="font-size: 8pt;text-align: justify; line-height: 18pt;">';
            }else{
            shtml +=  '<td colspan="3" rowspan="2" style="font-size: 7pt;text-align: justify; line-height: 18pt;">';
            }
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub1 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub2 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub3 + '<br />';   
            //shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub4 + '<br />';
            //shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub5 + '<br />';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            
            //START - FOR HK 1 PAGE TESTING
            shtml +=  '<table style="width: 9in; font-size: 8pt; font-family: arial;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="width: 0in">';
            shtml +=  '<table style="width: 100%;font-size: 10pt;">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 60px; height: 15px">';
            shtml +=  '<img src="../../../'+ vd +'/images/pnb_black_logo.JPG" style="width: 58px; height: 68px" /></td>';
            shtml +=  '<td style="vertical-align: top; width: 520px; height: 15px">';
            shtml +=  '<strong>' + dt.Rows[0].Header1 + '</strong><br />';
            shtml +=  '' + dt.Rows[0].Header2 + '<br />';
            shtml +=  '<strong>' + dt.Rows[0].NameofBranch + '</strong><br />';
            shtml +=  '' + dt.Rows[0].AdressofBranch + '<br />';
            shtml +=  '' + dt.Rows[0].BranchofContact + '<br />';
            shtml +=  '' + dt.Rows[0].BranchofEmail + '<br />';
            shtml +=  '' + dt.Rows[0].corp_address + '<br />';
            shtml +=  '' + dt.Rows[0].corp_telno + '</td>';
            shtml +=  '<td style="vertical-align: top; height: 15px">';
            shtml +=  'Transaction Date: ' + HTMLRep.formatDate(dt.Rows[0].TranxDate,"/") + '<br />';
	        if (dt.Rows[0].ClearingDate == '1900')
	        {
	        shtml +=  'Clearing Date: <br />';
	        }else{
	        shtml +=  'Clearing Date: '+ dt.Rows[0].ClearingDate +'<br />';
	        }
            shtml +=  'Time: '+ HTMLRep.formatTime(dt.Rows[0].TranxDate) +'<br />';
            shtml +=  'Loyalty Points:';
            shtml +=  '<br />';
            shtml +=  '' + dt.Rows[0].TranStatus + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 9in; font-size: 10pt; font-family: arial;';
            shtml +=  'height: 0.23in; border-top-width: thin; border-left-width: thin; border-left-color: black; border-bottom-width: thin; border-bottom-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="border-right: black thin solid; border-top: black thin solid;';
            shtml +=  'border-left: black thin solid; border-bottom: black thin solid">';
            shtml +=  '<table style="font-size: 10pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 244px; height: 14px">';
            shtml +=  '<strong>Remitter ID No.: </strong>' + dt.Rows[0].RemitterID + '</td>';
            shtml +=  '<td style="width: 285px; height: 14px">';
            shtml +=  '<strong>Official Receipt No.:</strong> ' + dt.Rows[0].ReceiptNo + '</td>';
            shtml +=  '<td style="width: 257px; height: 14px">';
            shtml +=  '<strong>Reference No.:</strong> ' + dt.Rows[0].ReferenceNo + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';    
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td style="border-right: black thin solid; border-top: black thin solid; vertical-align: top;';
            shtml +=  'border-left: black thin solid; width: 50%; border-bottom: black thin solid; height: 20px">';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 0px; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="vertical-align: top; width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 20px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 10pt; text-decoration: underline">REMITTER INFO</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="2">';
            shtml +=  '<span><span style="font-size: 10pt"><strong>&nbsp;Name: </strong>' + dt.Rows[0].RemitterName + '<br />';
            shtml +=  '</span></span>';
            shtml +=  '<table style="font-size: 10pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="width: 61px; vertical-align: top;">';
            shtml +=  '<strong style="width: 30px">Address:<br />';
            shtml +=  '</strong></td>';
            shtml +=  '<td colspan="2" rowspan="3" style="height: 50px; vertical-align: top;">';
            shtml +=  '' + dt.Rows[0].RemitterAddress + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<span><span style="font-size: 10pt; width: 100%"><strong>Telephone Number:</strong>';
            shtml +=  '' + dt.Rows[0].RemitterTelNo + '</span></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="vertical-align: top; width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 20px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 100% ; height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 10pt; text-decoration: underline">BENEFICIARY INFO</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="2">';
            shtml +=  '<span><span style="font-size: 10pt"><strong>&nbsp;Name:</strong> ' + dt.Rows[0].BenefName + '<br />';
            shtml +=  '</span></span>';
            shtml +=  '<table style="font-size: 10pt; width: 100%; vertical-align: top;">';
            shtml +=  '<tr>';
            shtml +=  '<td  style="width: 60px; vertical-align: top;"><strong style="width: 30px">Address:<br />';
            shtml +=  '</strong>';
            shtml +=  '</td>';
            shtml +=  '<td rowspan="3" style="width: 90%; height: 50px; vertical-align: top;">';
            shtml +=  '' + dt.Rows[0].BenefAddress + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<span><span style="font-size: 10pt"><strong>Telephone Number:</strong> ' + dt.Rows[0].BenefTelNo + '</span></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="vertical-align: top;  width: 100%; height: 20px; border-bottom: black thin solid;">';
            shtml +=  '<table style="font-size: 10pt; width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 10pt; text-decoration: underline">TRANSACTION DETAILS</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 10pt">';
            shtml +=  '<td colspan="3" rowspan="2" style="font-size: 10pt">';
            shtml +=  '<table style="font-size: 10pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Beneficiarys Account No. : </strong>' + dt.Rows[0].BenefAcctNo + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Thru PNB Branch: </strong>' + dt.Rows[0].ThruPNBBranch + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Other Banks Name / Branch: </strong>' + dt.Rows[0].OtherBankName + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Trans Type/Products: </strong>' + dt.Rows[0].TransType + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Source of Funds: </strong>' + dt.Rows[0].SourceFunds + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Member ID:</strong>' + dt.Rows[0].MemberID + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Purpose of Remittance: </strong>' + dt.Rows[0].PurposeRemittance + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Remittance Transaction Source: </strong>' + dt.Rows[0].RemittanceTranxSource + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 10pt">';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 10pt">';
            shtml +=  '<td rowspan="1" style="border-top-width: thin; border-left-width: thin; border-left-color: black;';
            shtml +=  'border-bottom-width: thin; border-bottom-color: black; vertical-align: top; width: 100%;';
            shtml +=  'border-top-color: black; height: 20px; border-right-width: thin; border-right-color: black">';
            shtml +=  '<table style="width: 100%; height: 20px;font-size: 10pt;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px;font-size: 10pt;">';
            shtml +=  '<p style="text-align: center">';
            shtml +=  '<span style="font-size: 10pt; text-decoration: underline"><strong>CLIENT/CUSTOMER UNDERTAKING</strong></span></p>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="2" style="text-align: justify; line-height: 18pt">';
            shtml +=  '<table style="width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="3" style="text-align: justify;font-size: 10pt;line-height: 18pt">';
            shtml +=  '<br />';
            shtml +=  'I/We hereby agree that the PNB GLOBAL REMITTANCE AND FINANCIAL COMPANY (HK) ';
            shtml +=  'LIMITED reserves the right to, at any time, obtain further information/document ';
            shtml +=  'from me/us.';
            shtml +=  '<br/>';
            shtml +=  '<br/>';
            shtml +=  'I/We have read and understood the Notice to Customer relating to the Personal ';
            shtml +=  'Data (Privacy) Ordinance and all the terms and conditions printed overleaf.';
            shtml +=  '<br/>';
            shtml +=  '<br/>';
            shtml +=  'I also confirm that details of my beneficiary are correct.';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<br />';
            shtml +=  '<br />';
            shtml +=  '<hr style="width: 378px; height: 1px" />';
            shtml +=  '<div style="width: 100%; height: 25px; text-align: center">';
            shtml +=  'Remitter&#039;s Signature</div>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '<td colspan="2" style="border-right: black thin solid; border-top: black thin solid;';
            shtml +=  'vertical-align: top; border-left: black thin solid; width: 50%; border-bottom: black thin solid;';
            shtml +=  'height: 20px">';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 0px; height: 90px">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 86px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="font-size: 10pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 18px; text-align: center">';
            shtml +=  '<span style="font-size: 10pt; text-decoration: underline"><strong>REMITTANCE DETAILS</strong></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td style="text-align: left; width: 245px; height: 15px;">';
	        shtml +=  '    <strong>';
	        shtml +=  'Net Amount Due to Beneficiary:</strong></td>';
	        shtml +=  '<td style="width: 58px; height: 15px">';
	        if (dt.Rows[0].NetAmountDueToBenefCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].NetAmountDueToBenefCurrCode + '</td>';
	        }
	        shtml +=  '<td style="height: 15px; text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].NetAmountDueToBenefAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Exchange Rate Used:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].ExchangeRateUsedCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].ExchangeRateUsedCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].ExchangeRateUsedAmt,4) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Remittance Amount:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].RemittanceAmountCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].RemittanceAmountCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].RemittanceAmountAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Cable:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].CableCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].CableCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].CableAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Remittance Fee:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].RemittanceFeeCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].RemittanceFeeCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].RemittanceFeeAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Other Charges:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].OtherChargesCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].OtherChargesCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].OtherChargesAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="width: 221px">';
	        shtml +=  '<strong>';
	        shtml +=  'Total amount received from customer:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].TotalAmtRcvdFromCustCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].TotalAmtRcvdFromCustCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].TotalAmtRcvdFromCustAmt,2) + '</td>';
	        shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 70px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;font-size:10pt;">';
	        shtml +=  '' + dt.Rows[0].Check + '<br />';
	        shtml +=  '' + dt.Rows[0].Cash + '<br />';
	        shtml +=  '' + dt.Rows[0].SpecialMessage + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="font-size:10pt;width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 40px; text-align: center; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 177px;font-size:10pt">';
            shtml +=  '<strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Payment ';
            shtml +=  '<br />';
            shtml +=  'Received / Processed By:</strong></td>';
            shtml +=  '<td style="font-size:10pt;height:30px;">';
            shtml +=  '' + dt.Rows[0].ProcessedBy + '<br />';
            shtml +=  '<hr style="width: 200px; height: 2px;background-color:black;" />';
            shtml +=  '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Authorized Personnel&nbsp;</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="border-top-width: thin; border-left-width: thin; border-left-color: black;';
            shtml +=  'border-bottom-width: thin; border-bottom-color: black; vertical-align: top; width: 100%;';
            shtml +=  'border-top-color: black; height: 20px; text-align: center; border-right-width: thin;';
            shtml +=  'border-right-color: black">';
            shtml +=  '<table style="width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px" align="center">';
            shtml +=  '<span style="font-size: 10pt; text-decoration: underline"><strong>DECLARATIONS</strong></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            if (dt.Rows[0].ReceiptDeclarationSub5.toString().length == 0)
            {
            shtml +=  '<td colspan="3" rowspan="2" style="font-size: 8pt;text-align: justify; line-height: 18pt;">';
            }else{
            shtml +=  '<td colspan="3" rowspan="2" style="font-size: 7pt;text-align: justify; line-height: 18pt;">';
            }
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub1 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub2 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub3 + '<br />';   
            //shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub4 + '<br />';
            //shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub5 + '<br />';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            //END - FOR HK 1 PAGE TESTING
         

        return shtml;
    }
    
    this.half_page = function(dt)
    {
        var shtml = "";
        shtml +=  '<table style="width: 8.5in">';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" style="border-right: black thin dotted; border-top: black thin dotted; border-left: black thin dotted; border-bottom: black thin dotted">';
        shtml +=  '<table style="height: 0%">';
        shtml +=  '<tr>';
        shtml +=  '<td style="font-size: 10pt; width: 371px; color: black; font-family: arial; height: 22px;';
        shtml +=  'text-align: center">';
        shtml +=  '<span style="font-size: 8pt; text-decoration: underline"><span style="font-size: 8.5pt;';
        shtml +=  'text-decoration: underline"><strong>IMPORTANT REMINDERS TO CLIENT / CUSTOMER</strong></span></span></td>';
        shtml +=  '<td rowspan="3" style="font-size: 6.5px; vertical-align: top; width: 50%; color: black;';
        shtml +=  'font-family: arial; height: 101px; text-align: left">';
        shtml +=  '<table style="font-size: 6.5pt; width: 100%; height: 23px">';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" >';
        shtml +=  'To the PNB __________________ , Branch: _____________ Date____________: This is';
        shtml +=  'to authorize your office to change my (pls. mark check the box/es below as applicable),';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" nowrap>';
        shtml +=  '<input id="Checkbox7" style="width: 26px; height: 12px" type="checkbox" />Remitter';
        shtml +=  'Name to _______________________________________________________</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" nowrap>';
        shtml +=  '<input id="Checkbox8" style="width: 26px; height: 12px" type="checkbox" />Address';
        shtml +=  '&nbsp;to _______________________________________ Postal Code: _________</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" nowrap>';
        shtml +=  '<input id="Checkbox9" style="width: 26px; height: 12px" type="checkbox" />Telephone';
        shtml +=  'No to: __________________________ Mobile No to: _________________</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" nowrap>';
        shtml +=  '<input id="Checkbox10" style="width: 26px; height: 12px" type="checkbox" />Beneficiary';
        shtml +=  '&nbsp;Name to: ________________________________Postal Code: _______</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" nowrap>';
        shtml +=  '<input id="Checkbox11" style="width: 26px; height: 12px" type="checkbox" />Beneficiary';
        shtml +=  'Address To: _________________________________________________</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" nowrap>';
        shtml +=  '<input id="Checkbox12" style="width: 26px; height: 12px" type="checkbox" />Beneficiary';
        shtml +=  'Telephone No to: ___________________Mobile No to: ______________</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" nowrap>';
        shtml +=  '<input id="Checkbox13" style="width: 26px; height: 12px" type="checkbox" />Others';
        shtml +=  '(please specify): _________________________________________________</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3">';
        shtml +=  'To support the request for ammendment, photocopy of the following document/s or';
        shtml +=  'ID/s is/are provided (*should match the document/s or ID/s required in No.2 of important';
        shtml +=  'Reminders to Client / Customer):';
        shtml +=  '<br />';
        shtml +=  'A) _______________&nbsp; B_________________ C)_________________ D)_______________</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3">';
        shtml +=  '<table style="width: 100%">';
        shtml +=  '<tr>';
        shtml +=  '<td rowspan="3" style="font-size: 8pt;border-right: black thin solid; border-top: black thin solid;';
        shtml +=  'border-left: black thin solid; border-bottom: black thin solid">';
        shtml +=  'PNB Use Only:<br />';
        shtml +=  'Processed By____Date:___</td>';
        shtml +=  '<td colspan="2" rowspan="3" style="font-size: 8pt;text-align: center">';
        shtml +=  '<hr style="width: 234px; height: 1px" />';
        shtml +=  '&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; Remitter Signature Over Printed Name</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td rowspan="2" style="font-size: 10px; vertical-align: top; width: 371px; color: black;';
        shtml +=  'font-family: arial; height: 113px; text-align: left; line-height: 13pt;">';
        shtml +=  '<span style="font-size: 8pt">1) Please notify the bank of any change in your name,';
        shtml +=  'address, telephone number or other details pertaining to your remittance account';
        shtml +=  '/ record by mailing / submitting the following: a) Photocopy of valid document/s';
        shtml +=  'or ID/s to support the amendment.';
        shtml +=  '<br />';
        shtml +=  'b) Duly accomplished and signed " Authorization and Notice of Amendment" cutout';
        shtml +=  'found on the opposite bottom right corner of this receipt. 2) Mailing address should';
        shtml +=  'tally with the clients registered address shown in the valid document/s or ID/s,';
        shtml +=  'as any irregularity in the registered address may result in non - implementation';
        shtml +=  'of remittance. The valid document/s or ID/s are the following*:';
        shtml +=  '<br />';
        shtml +=  '</span>';
        shtml +=  '<table style="font-size: 8pt; width: 303px; height: 45px">';
        shtml +=  '<tr>';
        shtml +=  '<td style="width: 50%; border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; border-bottom: black thin solid;">';
        shtml +=  'A) ' + dt.Rows[0].ID1 + '</td>';
        shtml +=  '<td style="width: 50%; border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; border-bottom: black thin solid;">';
        shtml +=  'B) ' + dt.Rows[0].ID2 + '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td style="width: 50%; border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; border-bottom: black thin solid;">';
        shtml +=  'C) ' + dt.Rows[0].ID3 + '</td>';
        shtml +=  '<td style="width: 50%; border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; border-bottom: black thin solid;">';
        shtml +=  'D) ' + dt.Rows[0].ID4 + '</td>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '<span style="font-size: 8pt">3) Always provide the bank with clear and proper instructions';
        shtml +=  'as to beneficiary details and remittance amount to facilitate processing of remittance.';
        shtml +=  '</span>';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr style="font-size: 10pt">';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        return shtml;
    }
    this.Receipt_body_halfpage = function(dt,isRCC)
    {
    var vd = window.location.href.split('/')[3];
    
    var shtml = "";
            shtml +=  '<br />';
            //shtml +=  '<br />';
            shtml +=  '<div style="width: 970px; margin: 0px;">';
            shtml +=  '<table style="width: 100%; font-size: 8pt; font-family: arial;" cellspacing="0" cellpadding="0">';
            //shtml +=  '<table style="width: 8.5in; font-size: 7pt; font-family: arial;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="width: 0in">';
            shtml +=  '<table style="width: 100%;font-size: 8pt" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 60px; height: 15px">';
            shtml +=  '<img src="../../../'+ vd +'/images/pnb_black_logo.JPG" style="width: 58px; height: 68px" /></td>';
            shtml +=  '<td style="vertical-align: top; width: 520px; height: 15px">';
            shtml +=  '<strong>' + dt.Rows[0].Header1 + '</strong><br />';
            shtml +=  '' + dt.Rows[0].Header2 + '<br />';
            shtml +=  '<strong>' + dt.Rows[0].NameofBranch + '</strong><br />';
            shtml +=  '' + dt.Rows[0].AdressofBranch + '<br />';
            shtml +=  '' + dt.Rows[0].BranchofContact + '<br />';
            shtml +=  '' + dt.Rows[0].BranchofEmail + '<br />';
            shtml +=  '' + dt.Rows[0].corp_address + '<br />';
            shtml +=  '' + dt.Rows[0].corp_telno + '</td>';
            shtml +=  '<td style="vertical-align: top; height: 15px">';
            shtml +=  'Transaction Date: ' + HTMLRep.formatDate(dt.Rows[0].TranxDate,"/") + '<br />';
	        if (dt.Rows[0].ClearingDate == '1900')
	        {
	        shtml +=  'Clearing Date: <br />';
	        }else{
	        shtml +=  'Clearing Date: '+ dt.Rows[0].ClearingDate +'<br />';
	        }
            shtml +=  'Time: '+ HTMLRep.formatTime(dt.Rows[0].TranxDate) +'<br />';
            shtml +=  'Loyalty Points:';
            shtml +=  '<br />';
            shtml +=  '' + dt.Rows[0].TranStatus + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 100%; font-size: 9pt; font-family: arial;';
            shtml +=  'height: 0.23in; border-top-width: thin; border-left-width: thin; border-left-color: black; border-bottom-width: thin; border-bottom-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="border-right: black thin solid; border-top: black thin solid;';
            shtml +=  'border-left: black thin solid; border-bottom: black thin solid">';
            shtml +=  '<table style="font-size: 9pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 244px; height: 14px">';
            shtml +=  '<strong>Remitter ID No.: </strong>' + dt.Rows[0].RemitterID + '</td>';
            shtml +=  '<td style="width: 285px; height: 14px">';
            shtml +=  '<strong>Official Receipt No.:</strong> ' + dt.Rows[0].ReceiptNo + '</td>';
            shtml +=  '<td style="width: 257px; height: 14px">';
            shtml +=  '<strong>Reference No.:</strong> ' + dt.Rows[0].ReferenceNo + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';    
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td style="border-right: black thin solid; border-top: black thin solid; vertical-align: top;';
            shtml +=  'border-left: black thin solid; width: 50%; border-bottom: black thin solid; height: 20px">';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 0px; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="vertical-align: top; width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 20px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 9pt; text-decoration: underline">REMITTER INFO</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="2">';
            shtml +=  '<span><span style="font-size: 8.5pt"><strong>&nbsp;Name: </strong>' + dt.Rows[0].RemitterName + '<br />';
            shtml +=  '</span></span>';
            shtml +=  '<table style="font-size: 8.5pt; width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="width: 61px; vertical-align: top;">';
            shtml +=  '<strong style="width: 30px">Address:<br />';
            shtml +=  '</strong></td>';
            shtml +=  '<td colspan="2" rowspan="3" style="height: 40px; vertical-align: top;">';
            shtml +=  '' + dt.Rows[0].RemitterAddress + '</td>';
            shtml +=  '</tr>';
            //shtml +=  '<tr>';
            //shtml +=  '</tr>';
            //shtml +=  '<tr>';
            //shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<span><span style="font-size: 8.5pt; width: 100%"><strong>Telephone Number:</strong>';
            shtml +=  '' + dt.Rows[0].RemitterTelNo + '</span></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="vertical-align: top; width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 20px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%; height: 20px" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 100% ; height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 9pt; text-decoration: underline">BENEFICIARY INFO</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="2">';
            shtml +=  '<span><span style="font-size: 8.5pt"><strong>&nbsp;Name:</strong> ' + dt.Rows[0].BenefName + '<br />';
            shtml +=  '</span></span>';
            shtml +=  '<table style="font-size: 8.5pt; width: 100%; vertical-align: top;" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td  style="width: 60px; vertical-align: top;"><strong style="width: 30px">Address:<br />';
            shtml +=  '</strong>';
            shtml +=  '</td>';
            shtml +=  '<td rowspan="3" style="width: 90%; height: 40px; vertical-align: top;">';
            shtml +=  '' + dt.Rows[0].BenefAddress + '</td>';
            shtml +=  '</tr>';
            //shtml +=  '<tr>';
            //shtml +=  '</tr>';
            //shtml +=  '<tr>';
            //shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<span><span style="font-size: 8.5pt"><strong>Telephone Number:</strong> ' + dt.Rows[0].BenefTelNo + '</span></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="vertical-align: top;  width: 100%; height: 20px; border-bottom: black thin solid;">';
            shtml +=  '<table style="font-size: 8.5pt; width: 100%; height: 20px" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 9pt; text-decoration: underline">TRANSACTION DETAILS</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 8.5pt">';
            shtml +=  '<td colspan="3" rowspan="2" style="font-size: 8.5pt">';
            shtml +=  '<table style="font-size: 8.5pt; width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Beneficiarys Account No. : </strong>' + dt.Rows[0].BenefAcctNo + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Thru PNB Branch: </strong>' + dt.Rows[0].ThruPNBBranch + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Other Banks Name / Branch: </strong>' + dt.Rows[0].OtherBankName + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Trans Type/Products: </strong>' + dt.Rows[0].TransType + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Source of Funds: </strong>' + dt.Rows[0].SourceFunds + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Member ID:</strong>' + dt.Rows[0].MemberID + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Purpose of Remittance: </strong>' + dt.Rows[0].PurposeRemittance + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Remittance Transaction Source: </strong>' + dt.Rows[0].RemittanceTranxSource + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 8.5pt">';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 8.5pt">';
            shtml +=  '<td rowspan="1" style="border-top-width: thin; border-left-width: thin; border-left-color: black;';
            shtml +=  'border-bottom-width: thin; border-bottom-color: black; vertical-align: top; width: 100%;';
            shtml +=  'border-top-color: black; height: 20px; border-right-width: thin; border-right-color: black">';
            shtml +=  '<table style="width: 100%; height: 20px;font-size: 8pt;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px">';
            shtml +=  '<p style="text-align: center">';
            shtml +=  '<span style="font-size: 9pt; text-decoration: underline"><strong>CLIENT/CUSTOMER UNDERTAKING</strong></span></p>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="2" style="text-align: justify;line-height: 14pt">';// line-height: 14pt;
            shtml +=  '<table style="width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="3" style="text-align: justify;font-size: 8.5pt;line-height: 14pt">'; //line-height: 14pt;                      
            shtml +=  'I/We hereby agree that the PNB GLOBAL REMITTANCE AND FINANCIAL COMPANY (HK) ';
            shtml +=  'LIMITED reserves the right to, at any time, obtain further information/document ';
            shtml +=  'from me/us.';
            shtml +=  '<br/>';
            //shtml +=  '<br/>';
            shtml +=  'I/We have read and understood the Notice to Customer relating to the Personal ';
            shtml +=  'Data (Privacy) Ordinance and all the terms and conditions printed overleaf.';
            //shtml +=  '<br/>';
            shtml +=  '<br/>';
            shtml +=  'I also confirm that details of my beneficiary are correct.';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<br />';
            shtml +=  '<hr style="width: 378px; height: 1px; text-align: center"/>';
            shtml +=  '<div style="width: 100%; height: 25px; text-align: center">';
            shtml +=  'Remitter&#039;s Signature</div>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            // Edited by CHR from colspan=2 to colspan=1
            shtml +=  '<td style="border-right: black thin solid; border-top: black thin solid;';
            shtml +=  'vertical-align: top; border-left: black thin solid; width: 50%; border-bottom: black thin solid;';
            shtml +=  'height: 20px">';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 100%; height: 90px">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 86px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="font-size: 8.5pt; width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 18px; text-align: center">';
            shtml +=  '<span style="font-size: 9pt; text-decoration: underline"><strong>REMITTANCE DETAILS</strong></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td style="text-align: left; width: 245px; height: 15px;">';
	        shtml +=  '    <strong>';
	        shtml +=  'Net Amount Due to Beneficiary:</strong></td>';
	        shtml +=  '<td style="width: 58px; height: 15px">';
	        if (dt.Rows[0].NetAmountDueToBenefCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].NetAmountDueToBenefCurrCode + '</td>';
	        }
	        shtml +=  '<td style="height: 15px; text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].NetAmountDueToBenefAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Exchange Rate Used:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].ExchangeRateUsedCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].ExchangeRateUsedCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].ExchangeRateUsedAmt,4) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Remittance Amount:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].RemittanceAmountCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].RemittanceAmountCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].RemittanceAmountAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Cable:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].CableCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].CableCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].CableAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Remittance Fee:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].RemittanceFeeCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].RemittanceFeeCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].RemittanceFeeAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Other Charges:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].OtherChargesCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].OtherChargesCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].OtherChargesAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="width: 221px">';
	        shtml +=  '<strong>';
	        shtml +=  'Total amount received from customer:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].TotalAmtRcvdFromCustCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].TotalAmtRcvdFromCustCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].TotalAmtRcvdFromCustAmt,2) + '</td>';
	        shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 70px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;font-size: 8.5pt">';
	        shtml +=  '' + dt.Rows[0].Check + '<br />';
	        shtml +=  '' + dt.Rows[0].Cash + '<br />';
	        shtml +=  '' + dt.Rows[0].SpecialMessage + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="font-size:8.5pt;width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 40px; text-align: center; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 177px;font-size:8.5pt">';
            shtml +=  '<strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Payment ';
            shtml +=  '<br />';
            shtml +=  'Received / Processed By:</strong></td>';
            shtml +=  '<td style="font-size:8.5pt;">';
            shtml +=  '' + dt.Rows[0].ProcessedBy + '<br />';
            shtml +=  '<hr style="width: 200px; height: 2px;background-color:black; text-align: left;" />';/////
            shtml +=  '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Authorized Personnel&nbsp;</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="border-top-width: thin; border-left-width: thin; border-left-color: black;';
            shtml +=  'border-bottom-width: thin; border-bottom-color: black; vertical-align: top; width: 100%;';
            shtml +=  'border-top-color: black; height: 20px; text-align: center; border-right-width: thin;';
            shtml +=  'border-right-color: black">';
            shtml +=  '<table style="width: 100%; height: 20px" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px;text-align:center;">';
            shtml +=  '<span style="font-size: 9pt; text-decoration: underline"><strong>DECLARATIONS</strong></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            if (dt.Rows[0].ReceiptDeclarationSub5.toString().length == 0)
            {
                       shtml +=  '<td colspan="3" rowspan="2" style="font-size: 8.5pt;text-align: justify;line-height: 12pt">'; //line-height: 14pt;
            }else{
                        shtml +=  '<td colspan="3" rowspan="2" style="font-size: 8.5pt;text-align: justify; line-height: 12pt">';//line-height: 14pt;
            }
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub1 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub2 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub3 + '<br />';   
           // shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub4 + '<br />';
            //shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub5 + '<br />';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            
            //START - HK OTC 2nd copy --Added by Ruth for RFS 14332-014
            shtml +=  '<br />';
            shtml +=  '<br />';
            shtml +=  '<br />';
            shtml +=  '<br />';
            shtml +=  '<br />';
            shtml +=  '<br />';
            shtml +=  '<br />';
            shtml +=  '<br />';
            shtml +=  '<table style="width: 100%; font-size: 8pt; font-family: arial;" cellspacing="0" cellpadding="0">';
            //shtml +=  '<table style="width: 8.5in; font-size: 7pt; font-family: arial;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="width: 0in">';
            shtml +=  '<table style="width: 100%;font-size: 8pt" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 60px; height: 15px">';
            shtml +=  '<img src="../../../'+ vd +'/images/pnb_black_logo.JPG" style="width: 58px; height: 68px" /></td>';
            shtml +=  '<td style="vertical-align: top; width: 520px; height: 15px">';
            shtml +=  '<strong>' + dt.Rows[0].Header1 + '</strong><br />';
            shtml +=  '' + dt.Rows[0].Header2 + '<br />';
            shtml +=  '<strong>' + dt.Rows[0].NameofBranch + '</strong><br />';
            shtml +=  '' + dt.Rows[0].AdressofBranch + '<br />';
            shtml +=  '' + dt.Rows[0].BranchofContact + '<br />';
            shtml +=  '' + dt.Rows[0].BranchofEmail + '<br />';
            shtml +=  '' + dt.Rows[0].corp_address + '<br />';
            shtml +=  '' + dt.Rows[0].corp_telno + '</td>';
            shtml +=  '<td style="vertical-align: top; height: 15px">';
            shtml +=  'Transaction Date: ' + HTMLRep.formatDate(dt.Rows[0].TranxDate,"/") + '<br />';
	        if (dt.Rows[0].ClearingDate == '1900')
	        {
	        shtml +=  'Clearing Date: <br />';
	        }else{
	        shtml +=  'Clearing Date: '+ dt.Rows[0].ClearingDate +'<br />';
	        }
            shtml +=  'Time: '+ HTMLRep.formatTime(dt.Rows[0].TranxDate) +'<br />';
            shtml +=  'Loyalty Points:';
            shtml +=  '<br />';
            shtml +=  '' + dt.Rows[0].TranStatus + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 100%; font-size: 9pt; font-family: arial;';
            shtml +=  'height: 0.23in; border-top-width: thin; border-left-width: thin; border-left-color: black; border-bottom-width: thin; border-bottom-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="border-right: black thin solid; border-top: black thin solid;';
            shtml +=  'border-left: black thin solid; border-bottom: black thin solid">';
            shtml +=  '<table style="font-size: 9pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 244px; height: 14px">';
            shtml +=  '<strong>Remitter ID No.: </strong>' + dt.Rows[0].RemitterID + '</td>';
            shtml +=  '<td style="width: 285px; height: 14px">';
            shtml +=  '<strong>Official Receipt No.:</strong> ' + dt.Rows[0].ReceiptNo + '</td>';
            shtml +=  '<td style="width: 257px; height: 14px">';
            shtml +=  '<strong>Reference No.:</strong> ' + dt.Rows[0].ReferenceNo + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';    
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td style="border-right: black thin solid; border-top: black thin solid; vertical-align: top;';
            shtml +=  'border-left: black thin solid; width: 50%; border-bottom: black thin solid; height: 20px">';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 0px; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="vertical-align: top; width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 20px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 9pt; text-decoration: underline">REMITTER INFO</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="2">';
            shtml +=  '<span><span style="font-size: 8.5pt"><strong>&nbsp;Name: </strong>' + dt.Rows[0].RemitterName + '<br />';
            shtml +=  '</span></span>';
            shtml +=  '<table style="font-size: 8.5pt; width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="width: 61px; vertical-align: top;">';
            shtml +=  '<strong style="width: 30px">Address:<br />';
            shtml +=  '</strong></td>';
            shtml +=  '<td colspan="2" rowspan="3" style="height: 40px; vertical-align: top;">';
            shtml +=  '' + dt.Rows[0].RemitterAddress + '</td>';
            shtml +=  '</tr>';
            //shtml +=  '<tr>';
            //shtml +=  '</tr>';
            //shtml +=  '<tr>';
            //shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<span><span style="font-size: 8.5pt; width: 100%"><strong>Telephone Number:</strong>';
            shtml +=  '' + dt.Rows[0].RemitterTelNo + '</span></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="vertical-align: top; width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 20px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%; height: 20px" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 100% ; height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 9pt; text-decoration: underline">BENEFICIARY INFO</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="2">';
            shtml +=  '<span><span style="font-size: 8.5pt"><strong>&nbsp;Name:</strong> ' + dt.Rows[0].BenefName + '<br />';
            shtml +=  '</span></span>';
            shtml +=  '<table style="font-size: 8.5pt; width: 100%; vertical-align: top;" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td  style="width: 60px; vertical-align: top;"><strong style="width: 30px">Address:<br />';
            shtml +=  '</strong>';
            shtml +=  '</td>';
            shtml +=  '<td rowspan="3" style="width: 90%; height: 40px; vertical-align: top;">';
            shtml +=  '' + dt.Rows[0].BenefAddress + '</td>';
            shtml +=  '</tr>';
            //shtml +=  '<tr>';
            //shtml +=  '</tr>';
            //shtml +=  '<tr>';
            //shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<span><span style="font-size: 8.5pt"><strong>Telephone Number:</strong> ' + dt.Rows[0].BenefTelNo + '</span></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="vertical-align: top;  width: 100%; height: 20px; border-bottom: black thin solid;">';
            shtml +=  '<table style="font-size: 8.5pt; width: 100%; height: 20px" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 9pt; text-decoration: underline">TRANSACTION DETAILS</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 8.5pt">';
            shtml +=  '<td colspan="3" rowspan="2" style="font-size: 8.5pt">';
            shtml +=  '<table style="font-size: 8.5pt; width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Beneficiarys Account No. : </strong>' + dt.Rows[0].BenefAcctNo + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Thru PNB Branch: </strong>' + dt.Rows[0].ThruPNBBranch + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Other Banks Name / Branch: </strong>' + dt.Rows[0].OtherBankName + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Trans Type/Products: </strong>' + dt.Rows[0].TransType + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Source of Funds: </strong>' + dt.Rows[0].SourceFunds + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Member ID:</strong>' + dt.Rows[0].MemberID + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Purpose of Remittance: </strong>' + dt.Rows[0].PurposeRemittance + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Remittance Transaction Source: </strong>' + dt.Rows[0].RemittanceTranxSource + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 8.5pt">';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 8.5pt">';
            shtml +=  '<td rowspan="1" style="border-top-width: thin; border-left-width: thin; border-left-color: black;';
            shtml +=  'border-bottom-width: thin; border-bottom-color: black; vertical-align: top; width: 100%;';
            shtml +=  'border-top-color: black; height: 20px; border-right-width: thin; border-right-color: black">';
            shtml +=  '<table style="width: 100%; height: 20px;font-size: 8pt;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px">';
            shtml +=  '<p style="text-align: center">';
            shtml +=  '<span style="font-size: 9pt; text-decoration: underline"><strong>CLIENT/CUSTOMER UNDERTAKING</strong></span></p>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="2" style="text-align: justify;line-height: 14pt">';// line-height: 14pt;
            shtml +=  '<table style="width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="3" style="text-align: justify;font-size: 8.5pt;line-height: 14pt">'; //line-height: 14pt;                      
            shtml +=  'I/We hereby agree that the PNB GLOBAL REMITTANCE AND FINANCIAL COMPANY (HK) ';
            shtml +=  'LIMITED reserves the right to, at any time, obtain further information/document ';
            shtml +=  'from me/us.';
            shtml +=  '<br/>';
            //shtml +=  '<br/>';
            shtml +=  'I/We have read and understood the Notice to Customer relating to the Personal ';
            shtml +=  'Data (Privacy) Ordinance and all the terms and conditions printed overleaf.';
            //shtml +=  '<br/>';
            shtml +=  '<br/>';
            shtml +=  'I also confirm that details of my beneficiary are correct.';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<br />';
            shtml +=  '<hr style="width: 378px; height: 1px; text-align: center"/>';
            shtml +=  '<div style="width: 100%; height: 25px; text-align: center">';
            shtml +=  'Remitter&#039;s Signature</div>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            // Edited by CHR from colspan=2 to colspan=1
            shtml +=  '<td style="border-right: black thin solid; border-top: black thin solid;';
            shtml +=  'vertical-align: top; border-left: black thin solid; width: 50%; border-bottom: black thin solid;';
            shtml +=  'height: 20px">';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 100%; height: 90px">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 86px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="font-size: 8.5pt; width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 18px; text-align: center">';
            shtml +=  '<span style="font-size: 9pt; text-decoration: underline"><strong>REMITTANCE DETAILS</strong></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td style="text-align: left; width: 245px; height: 15px;">';
	        shtml +=  '    <strong>';
	        shtml +=  'Net Amount Due to Beneficiary:</strong></td>';
	        shtml +=  '<td style="width: 58px; height: 15px">';
	        if (dt.Rows[0].NetAmountDueToBenefCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].NetAmountDueToBenefCurrCode + '</td>';
	        }
	        shtml +=  '<td style="height: 15px; text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].NetAmountDueToBenefAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Exchange Rate Used:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].ExchangeRateUsedCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].ExchangeRateUsedCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].ExchangeRateUsedAmt,4) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Remittance Amount:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].RemittanceAmountCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].RemittanceAmountCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].RemittanceAmountAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Cable:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].CableCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].CableCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].CableAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Remittance Fee:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].RemittanceFeeCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].RemittanceFeeCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].RemittanceFeeAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Other Charges:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].OtherChargesCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].OtherChargesCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].OtherChargesAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="width: 221px">';
	        shtml +=  '<strong>';
	        shtml +=  'Total amount received from customer:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].TotalAmtRcvdFromCustCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].TotalAmtRcvdFromCustCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].TotalAmtRcvdFromCustAmt,2) + '</td>';
	        shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 70px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;font-size: 8.5pt">';
	        shtml +=  '' + dt.Rows[0].Check + '<br />';
	        shtml +=  '' + dt.Rows[0].Cash + '<br />';
	        shtml +=  '' + dt.Rows[0].SpecialMessage + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="font-size:8.5pt;width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 40px; text-align: center; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 177px;font-size:8.5pt">';
            shtml +=  '<strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Payment ';
            shtml +=  '<br />';
            shtml +=  'Received / Processed By:</strong></td>';
            shtml +=  '<td style="font-size:8.5pt;">';
            shtml +=  '' + dt.Rows[0].ProcessedBy + '<br />';
            shtml +=  '<hr style="width: 200px; height: 2px;background-color:black; text-align: left;" />';/////
            shtml +=  '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Authorized Personnel&nbsp;</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="border-top-width: thin; border-left-width: thin; border-left-color: black;';
            shtml +=  'border-bottom-width: thin; border-bottom-color: black; vertical-align: top; width: 100%;';
            shtml +=  'border-top-color: black; height: 20px; text-align: center; border-right-width: thin;';
            shtml +=  'border-right-color: black">';
            shtml +=  '<table style="width: 100%; height: 20px" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px;text-align:center;">';
            shtml +=  '<span style="font-size: 9pt; text-decoration: underline"><strong>DECLARATIONS</strong></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            if (dt.Rows[0].ReceiptDeclarationSub5.toString().length == 0)
            {
                       shtml +=  '<td colspan="3" rowspan="2" style="font-size: 8.5pt;text-align: justify;line-height: 12pt">'; //line-height: 14pt;
            }else{
                        shtml +=  '<td colspan="3" rowspan="2" style="font-size: 8.5pt;text-align: justify; line-height: 12pt">';//line-height: 14pt;
            }
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub1 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub2 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub3 + '<br />';   
           // shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub4 + '<br />';
            //shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub5 + '<br />';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            //END - HK OTC 2nd copy
            
            shtml +=  '</div>';

            return shtml;
    }
}
/*END HONGKONG OTC RECEIPT -- ONE PAGE*/

/*START HONGKONG OBP RECEIPT -- ONE PAGE*/
function HTMLReceiptOBP_HK_OnePage()
{
    var cls = this;
    var _branchcode;    
    var _seqid;
	var _referenceno;
    
    this.evaluate_Receipt_query = function(query)
    {
        var squery = unescape(query.toString());
        if (squery.toLowerCase().indexOf("trantype=obp") > -1)
        {
            _branchcode = HTMLRep.get_branchcode();
            this.get_Receipt_data(
                                    HTMLRep.get_querystring_value(query,"tranvalue"),
                                    HTMLRep.get_branchcode(),
                                    ""
                                 )
            return;
        }
        if (squery.toLowerCase().indexOf("mergeparam") > -1)
        {
             _branchcode = HTMLRep.parse_merge_param_query(squery,"T@v_branchcode")
             this.get_Receipt_data(
                                    "",
                                    HTMLRep.parse_merge_param_query(squery,"T@v_branchcode"),
                                    HTMLRep.parse_merge_param_query(squery,"T@v_referenceno")
									)
            return;
        }
    }
    this.get_Receipt_data = function(seq_id,branchcode,referenceno)
    {
        var params; 
        var sp_name = "";
	var pass  =	false;
        
                _branchcode =   " @v_branchcode="  + branchcode;    
                _seqid    =   "@seqid="     + seq_id;
				_referenceno = " @v_referenceno="     + referenceno;
        
	pass = this.check_newdbswitch();
	
       if(seq_id.length == 0)
	   {
			if (pass)
			{
			    sp_name = "sp_otc_obp_Receipt_reprint_irsw";
			}
			else
			{
			    sp_name = "sp_otc_obp_Receipt_reprint";
			}
			params  = _branchcode + "^" + _referenceno ;
	   }else{
			sp_name = "SP_OTC_OBP_Receipt";
			params  = _branchcode + "^" + _seqid ;
		}
		AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
            alert(errmsg);       return;      
        } 
	    IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params,this.get_Receipt_data_callback);
    }
    
    this.get_Receipt_data_callback = function(result)
    {
        try
        {
            var ds = result.value;
            
            cls.check_branch();
            cls.display_Receipt_report(ds);
            
            
        }
        catch(e)
        {
            alert("ERROR WHILE GETTING Receipt DATA");
        }
    }
    this.check_newdbswitch = function()
    {
    
	var pass = false;
	var sp_name = 'proc_check_newdbswitch';
        var squery = HTMLRep.get_hidden_querystring("hidQueryString");
        var sbranchcode = HTMLRep.get_branchcode();
        
        var params  =  '@branchcode=' + sbranchcode;
        var isRCC  = false;
        AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
            alert(errmsg);     return;        
        } 
        
        var dtcchecksw = IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params).value;
        
        try
        {
            if (dtcchecksw.Tables[0].Rows.length > 0)
            {
                pass = true;
            }
        }
        catch(e)
        {
        }
	
	return pass; 
    }
    
    this.display_Receipt_report = function(ds)
    {
        var sp_name = 'proc_check_reporthalfpage';
        var squery = HTMLRep.get_hidden_querystring("hidQueryString");
        var sbranchcode = ds.Tables[0].Rows[0].ReferenceNo;
        sbranchcode =  sbranchcode.substring(1,4);
        var params  =  '@v_branhcode=' + sbranchcode;
        var ishalfpage  = false;
        AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
            alert(errmsg);     return;        
        } 
        
        var dtcheckhalfpage = IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params).value;
        
        try
        {
            if (dtcheckhalfpage.Tables[0].Rows.length > 0)
            {
                ishalfpage = true;
            }
        }
        catch(e)
        {
        }
    
        
        var shtml = "";        
        shtml +=  HTMLRep.print_html_button();
        var isRCC = cls.check_branch();
	
	isRCC = (cls.check_ars()) ? 0 : isRCC ;
	
        if (!ishalfpage)
        {
           
            shtml +=  this.Receipt_body(ds.Tables[0],isRCC);
               
            shtml += cls.half_page(ds.Tables[0]);
            
	        shtml +=  '</div>';
        }
        else
        {
           shtml +=  this.Receipt_body_halfpage(ds.Tables[0],isRCC);
           
        }
        document.getElementById("bodydiv").innerHTML = shtml
    }
    this.check_branch = function()
    {
        var sp_name = 'sp_report_checkbranch';
        var squery = HTMLRep.get_hidden_querystring("hidQueryString");
        var sbranchcode = HTMLRep.get_branchcode();
        
        var params  =  '@branchcode=' + sbranchcode;
        var isRCC  = false;
        AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
            alert(errmsg);     return;        
        } 
        
        var dtccheckbranch = IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params).value;
        
        try
        {
            if (dtccheckbranch.Tables[0].Rows.length > 0)
            {
                isRCC = true;
            }
        }
        catch(e)
        {
        }
        return isRCC;
    }
    
    this.check_ars = function()
    {
        var sp_name = 'proc_check_ars';
        
        var sbranchcode = HTMLRep.get_branchcode();
        
        var params  =  '@v_branchcode=' + sbranchcode;
        var isARS  = false;
        AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
            alert(errmsg);     return;        
        } 
        
        var dtccheckbranch = IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params).value;
        
        try
        {
            if (dtccheckbranch.Tables[0].Rows.length > 0)
            {
                isARS = true;
            }
        }
        catch(e)
        {
        }
        return isARS;
    }
    this.Receipt_body = function(dt,isRCC)
    {
	
	    var vd = window.location.href.split('/')[3];
	    
        var shtml = "";
	        shtml +=  '<div>';
            shtml +=  '<table style="width: 8.5in; font-size: 8pt; font-family: arial;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="width: 0in">';
            shtml +=  '<table style="width: 100%;font-size: 10pt;">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 60px; height: 15px">';
            shtml +=  '<img src="../../../'+ vd +'/images/pnb_black_logo.JPG" style="width: 58px; height: 68px" /></td>';
            shtml +=  '<td style="vertical-align: top; width: 520px; height: 15px">';
            shtml +=  '<strong>' + dt.Rows[0].Header1 + '</strong><br />';
            shtml +=  '' + dt.Rows[0].Header2 + '<br />';
            shtml +=  '<strong>' + dt.Rows[0].NameofBranch + '</strong><br />';
            shtml +=  '' + dt.Rows[0].AdressofBranch + '<br />';
            shtml +=  '' + dt.Rows[0].BranchofContact + '<br />';
            shtml +=  '' + dt.Rows[0].BranchofEmail + '<br />';
            shtml +=  '' + dt.Rows[0].corp_address + '<br />';
            shtml +=  '' + dt.Rows[0].corp_telno + '</td>';
            shtml +=  '<td style="vertical-align: top; height: 15px">';
            shtml +=  'Transaction Date: ' + HTMLRep.formatDate(dt.Rows[0].TranxDate,"/") + '<br />';
	        if (dt.Rows[0].ClearingDate == '1900')
	        {
	        shtml +=  'Clearing Date: <br />';
	        }else{
	        shtml +=  'Clearing Date: '+ dt.Rows[0].ClearingDate +'<br />';
	        }
            shtml +=  'Time: '+ HTMLRep.formatTime(dt.Rows[0].TranxDate) +'<br />';
            shtml +=  'Loyalty Points:';
            shtml +=  '<br />';
            shtml +=  '' + dt.Rows[0].TranStatus + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 8.5in; font-size: 10pt; font-family: arial;';
            shtml +=  'height: 0.23in; border-top-width: thin; border-left-width: thin; border-left-color: black; border-bottom-width: thin; border-bottom-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="border-right: black thin solid; border-top: black thin solid;';
            shtml +=  'border-left: black thin solid; border-bottom: black thin solid">';
            shtml +=  '<table style="font-size: 10pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 244px; height: 14px">';
            shtml +=  '<strong>Remitter ID No.: </strong>' + dt.Rows[0].RemitterID + '</td>';
            shtml +=  '<td style="width: 285px; height: 14px">';
            shtml +=  '<strong>Official Receipt No.:</strong> ' + dt.Rows[0].ReceiptNo + '</td>';
            shtml +=  '<td style="width: 257px; height: 14px">';
            shtml +=  '<strong>Reference No.:</strong> ' + dt.Rows[0].ReferenceNo + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';    
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td style="border-right: black thin solid; border-top: black thin solid; vertical-align: top;';
            shtml +=  'border-left: black thin solid; width: 50%; border-bottom: black thin solid; height: 20px">';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 0px; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="vertical-align: top; width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 20px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 10pt; text-decoration: underline">REMITTER INFO</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="2">';
            shtml +=  '<span><span style="font-size: 10pt"><strong>&nbsp;Name: </strong>' + dt.Rows[0].RemitterName + '<br />';
            shtml +=  '</span></span>';
            shtml +=  '<table style="font-size: 10pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="width: 61px; vertical-align: top;">';
            shtml +=  '<strong style="width: 30px">Address:<br />';
            shtml +=  '</strong></td>';
            shtml +=  '<td colspan="2" rowspan="3" style="height: 50px; vertical-align: top;">';
            shtml +=  '' + dt.Rows[0].RemitterAddress + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<span><span style="font-size: 10pt; width: 100%"><strong>Telephone Number:</strong>';
            shtml +=  '' + dt.Rows[0].RemitterTelNo + '</span></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="vertical-align: top; width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 20px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 100% ; height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 10pt; text-decoration: underline">BENEFICIARY INFO</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="2">';
            shtml +=  '<span><span style="font-size: 10pt"><strong>&nbsp;Name:</strong> ' + dt.Rows[0].BenefName + '<br />';
            shtml +=  '</span></span>';
            shtml +=  '<table style="font-size: 10pt; width: 100%; vertical-align: top;">';
            shtml +=  '<tr>';
            shtml +=  '<td  style="width: 60px; vertical-align: top;"><strong style="width: 30px">Address:<br />';
            shtml +=  '</strong>';
            shtml +=  '</td>';
            shtml +=  '<td rowspan="3" style="width: 90%; height: 50px; vertical-align: top;">';
            shtml +=  '' + dt.Rows[0].BenefAddress + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<span><span style="font-size: 10pt"><strong>Telephone Number:</strong> ' + dt.Rows[0].BenefTelNo + '</span></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="vertical-align: top;  width: 100%; height: 20px; border-bottom: black thin solid;">';
            shtml +=  '<table style="font-size: 10pt; width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 10pt; text-decoration: underline">TRANSACTION DETAILS</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 10pt">';
            shtml +=  '<td colspan="3" rowspan="2" style="font-size: 10pt">';
            shtml +=  '<table style="font-size: 10pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Beneficiarys Account No. : </strong>' + dt.Rows[0].BenefAcctNo + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Thru PNB Branch: </strong>' + dt.Rows[0].ThruPNBBranch + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Other Banks Name / Branch: </strong>' + dt.Rows[0].OtherBankName + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Trans Type/Products: </strong>' + dt.Rows[0].TransType + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Source of Funds: </strong>' + dt.Rows[0].SourceFunds + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Member ID:</strong>' + dt.Rows[0].MemberID + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Purpose of Remittance: </strong>' + dt.Rows[0].PurposeRemittance + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Remittance Transaction Source: </strong>' + dt.Rows[0].RemittanceTranxSource + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 10pt">';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 10pt">';
            shtml +=  '<td rowspan="1" style="border-top-width: thin; border-left-width: thin; border-left-color: black;';
            shtml +=  'border-bottom-width: thin; border-bottom-color: black; vertical-align: top; width: 100%;';
            shtml +=  'border-top-color: black; height: 20px; border-right-width: thin; border-right-color: black">';
            shtml +=  '<table style="width: 100%; height: 20px;font-size: 10pt;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px;font-size: 10pt;">';
            shtml +=  '<p style="text-align: center">';
            shtml +=  '<span style="font-size: 10pt; text-decoration: underline"><strong>CLIENT/CUSTOMER UNDERTAKING</strong></span></p>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="2" style="text-align: justify; line-height: 18pt">';
            shtml +=  '<table style="width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="3" style="text-align: justify;font-size: 10pt;line-height: 18pt">';
            shtml +=  '<br />';
            shtml +=  'I/We hereby agree that the PNB GLOBAL REMITTANCE AND FINANCIAL COMPANY (HK) ';
            shtml +=  'LIMITED reserves the right to, at any time, obtain further information/document ';
            shtml +=  'from me/us.';
            shtml +=  '<br/>';
            shtml +=  '<br/>';
            shtml +=  'I/We have read and understood the Notice to Customer relating to the Personal ';
            shtml +=  'Data (Privacy) Ordinance and all the terms and conditions printed overleaf.';
            shtml +=  '<br/>';
            shtml +=  '<br/>';
            shtml +=  'I also confirm that details of my beneficiary are correct.';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<br />';
            shtml +=  '<br />';
            shtml +=  '<hr style="width: 378px; height: 1px" />';
            shtml +=  '<div style="width: 100%; height: 25px; text-align: center">';
            shtml +=  'Remitter&#039;s Signature</div>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '<td colspan="2" style="border-right: black thin solid; border-top: black thin solid;';
            shtml +=  'vertical-align: top; border-left: black thin solid; width: 50%; border-bottom: black thin solid;';
            shtml +=  'height: 20px">';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 0px; height: 90px">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 86px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="font-size: 10pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 18px; text-align: center">';
            shtml +=  '<span style="font-size: 10pt; text-decoration: underline"><strong>REMITTANCE DETAILS</strong></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td style="text-align: left; width: 245px; height: 15px;">';
	        shtml +=  '    <strong>';
	        shtml +=  'Net Amount Due to Beneficiary:</strong></td>';
	        shtml +=  '<td style="width: 58px; height: 15px">';
	        if (dt.Rows[0].NetAmountDueToBenefCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].NetAmountDueToBenefCurrCode + '</td>';
	        }
	        shtml +=  '<td style="height: 15px; text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].NetAmountDueToBenefAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Exchange Rate Used:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].ExchangeRateUsedCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].ExchangeRateUsedCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].ExchangeRateUsedAmt,4) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Remittance Amount:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].RemittanceAmountCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].RemittanceAmountCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].RemittanceAmountAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Cable:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].CableCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].CableCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].CableAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Remittance Fee:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].RemittanceFeeCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].RemittanceFeeCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].RemittanceFeeAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Other Charges:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].OtherChargesCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].OtherChargesCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].OtherChargesAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="width: 221px">';
	        shtml +=  '<strong>';
	        shtml +=  'Total amount received from customer:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].TotalAmtRcvdFromCustCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].TotalAmtRcvdFromCustCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].TotalAmtRcvdFromCustAmt,2) + '</td>';
	        shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 70px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;font-size:10pt;">';
	        shtml +=  '' + dt.Rows[0].Check + '<br />';
	        shtml +=  '' + dt.Rows[0].Cash + '<br />';
	        shtml +=  '' + dt.Rows[0].SpecialMessage + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="font-size:10pt;width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 40px; text-align: center; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 177px;font-size:10pt">';
            shtml +=  '<strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Payment ';
            shtml +=  '<br />';
            shtml +=  'Received / Processed By:</strong></td>';
            shtml +=  '<td style="font-size:10pt;height:30px;">';
            shtml +=  '' + dt.Rows[0].ProcessedBy + '<br />';
            shtml +=  '<hr style="width: 200px; height: 2px;background-color:black;" />';
            shtml +=  '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Authorized Personnel&nbsp;</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="border-top-width: thin; border-left-width: thin; border-left-color: black;';
            shtml +=  'border-bottom-width: thin; border-bottom-color: black; vertical-align: top; width: 100%;';
            shtml +=  'border-top-color: black; height: 20px; text-align: center; border-right-width: thin;';
            shtml +=  'border-right-color: black">';
            shtml +=  '<table style="width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px" align="center">';
            shtml +=  '<span style="font-size: 10pt; text-decoration: underline"><strong>DECLARATIONS</strong></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            if (dt.Rows[0].ReceiptDeclarationSub5.toString().length == 0)
            {
            shtml +=  '<td colspan="3" rowspan="2" style="font-size: 8pt;text-align: justify; line-height: 18pt;">';
            }else{
            shtml +=  '<td colspan="3" rowspan="2" style="font-size: 7pt;text-align: justify; line-height: 18pt;">';
            }
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub1 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub2 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub3 + '<br />';   
            //shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub4 + '<br />';
            //shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub5 + '<br />';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
         

        return shtml;
    }
    
    this.half_page = function(dt)
    {
        var shtml = "";
        shtml +=  '<table style="width: 8.5in">';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" style="border-right: black thin dotted; border-top: black thin dotted; border-left: black thin dotted; border-bottom: black thin dotted">';
        shtml +=  '<table style="height: 0%">';
        shtml +=  '<tr>';
        shtml +=  '<td style="font-size: 10pt; width: 371px; color: black; font-family: arial; height: 22px;';
        shtml +=  'text-align: center">';
        shtml +=  '<span style="font-size: 8pt; text-decoration: underline"><span style="font-size: 8.5pt;';
        shtml +=  'text-decoration: underline"><strong>IMPORTANT REMINDERS TO CLIENT / CUSTOMER</strong></span></span></td>';
        shtml +=  '<td rowspan="3" style="font-size: 6.5px; vertical-align: top; width: 50%; color: black;';
        shtml +=  'font-family: arial; height: 101px; text-align: left">';
        shtml +=  '<table style="font-size: 6.5pt; width: 100%; height: 23px">';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" >';
        shtml +=  'To the PNB __________________ , Branch: _____________ Date____________: This is';
        shtml +=  'to authorize your office to change my (pls. mark check the box/es below as applicable),';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" nowrap>';
        shtml +=  '<input id="Checkbox7" style="width: 26px; height: 12px" type="checkbox" />Remitter';
        shtml +=  'Name to _______________________________________________________</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" nowrap>';
        shtml +=  '<input id="Checkbox8" style="width: 26px; height: 12px" type="checkbox" />Address';
        shtml +=  '&nbsp;to _______________________________________ Postal Code: _________</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" nowrap>';
        shtml +=  '<input id="Checkbox9" style="width: 26px; height: 12px" type="checkbox" />Telephone';
        shtml +=  'No to: __________________________ Mobile No to: _________________</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" nowrap>';
        shtml +=  '<input id="Checkbox10" style="width: 26px; height: 12px" type="checkbox" />Beneficiary';
        shtml +=  '&nbsp;Name to: ________________________________Postal Code: _______</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" nowrap>';
        shtml +=  '<input id="Checkbox11" style="width: 26px; height: 12px" type="checkbox" />Beneficiary';
        shtml +=  'Address To: _________________________________________________</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" nowrap>';
        shtml +=  '<input id="Checkbox12" style="width: 26px; height: 12px" type="checkbox" />Beneficiary';
        shtml +=  'Telephone No to: ___________________Mobile No to: ______________</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" nowrap>';
        shtml +=  '<input id="Checkbox13" style="width: 26px; height: 12px" type="checkbox" />Others';
        shtml +=  '(please specify): _________________________________________________</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3">';
        shtml +=  'To support the request for ammendment, photocopy of the following document/s or';
        shtml +=  'ID/s is/are provided (*should match the document/s or ID/s required in No.2 of important';
        shtml +=  'Reminders to Client / Customer):';
        shtml +=  '<br />';
        shtml +=  'A) _______________&nbsp; B_________________ C)_________________ D)_______________</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3">';
        shtml +=  '<table style="width: 100%">';
        shtml +=  '<tr>';
        shtml +=  '<td rowspan="3" style="font-size: 8pt;border-right: black thin solid; border-top: black thin solid;';
        shtml +=  'border-left: black thin solid; border-bottom: black thin solid">';
        shtml +=  'PNB Use Only:<br />';
        shtml +=  'Processed By____Date:___</td>';
        shtml +=  '<td colspan="2" rowspan="3" style="font-size: 8pt;text-align: center">';
        shtml +=  '<hr style="width: 234px; height: 1px" />';
        shtml +=  '&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; Remitter Signature Over Printed Name</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td rowspan="2" style="font-size: 10px; vertical-align: top; width: 371px; color: black;';
        shtml +=  'font-family: arial; height: 113px; text-align: left; line-height: 13pt;">';
        shtml +=  '<span style="font-size: 8pt">1) Please notify the bank of any change in your name,';
        shtml +=  'address, telephone number or other details pertaining to your remittance account';
        shtml +=  '/ record by mailing / submitting the following: a) Photocopy of valid document/s';
        shtml +=  'or ID/s to support the amendment.';
        shtml +=  '<br />';
        shtml +=  'b) Duly accomplished and signed " Authorization and Notice of Amendment" cutout';
        shtml +=  'found on the opposite bottom right corner of this receipt. 2) Mailing address should';
        shtml +=  'tally with the clients registered address shown in the valid document/s or ID/s,';
        shtml +=  'as any irregularity in the registered address may result in non - implementation';
        shtml +=  'of remittance. The valid document/s or ID/s are the following*:';
        shtml +=  '<br />';
        shtml +=  '</span>';
        shtml +=  '<table style="font-size: 8pt; width: 303px; height: 45px">';
        shtml +=  '<tr>';
        shtml +=  '<td style="width: 50%; border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; border-bottom: black thin solid;">';
        shtml +=  'A) ' + dt.Rows[0].ID1 + '</td>';
        shtml +=  '<td style="width: 50%; border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; border-bottom: black thin solid;">';
        shtml +=  'B) ' + dt.Rows[0].ID2 + '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td style="width: 50%; border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; border-bottom: black thin solid;">';
        shtml +=  'C) ' + dt.Rows[0].ID3 + '</td>';
        shtml +=  '<td style="width: 50%; border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; border-bottom: black thin solid;">';
        shtml +=  'D) ' + dt.Rows[0].ID4 + '</td>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '<span style="font-size: 8pt">3) Always provide the bank with clear and proper instructions';
        shtml +=  'as to beneficiary details and remittance amount to facilitate processing of remittance.';
        shtml +=  '</span>';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr style="font-size: 10pt">';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        return shtml;
    }
    this.Receipt_body_halfpage = function(dt)
    {
    var vd = window.location.href.split('/')[3];
    
    var shtml = "";
            shtml += '<br />';
            shtml +=  '<div style="width: 970px; margin: 0px;">';
            shtml +=  '<table style="width: 100%; font-size: 8pt; font-family: arial;" cellspacing="0" cellpadding="0">';
            //shtml +=  '<table style="width: 8.5in; font-size: 7pt; font-family: arial;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="width: 0in">';
            shtml +=  '<table style="width: 100%;font-size: 8pt" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 60px; height: 15px">';
            shtml +=  '<img src="../../../'+ vd +'/images/pnb_black_logo.JPG" style="width: 58px; height: 68px" /></td>';
            shtml +=  '<td style="vertical-align: top; width: 520px; height: 15px">';
            shtml +=  '<strong>' + dt.Rows[0].Header1 + '</strong><br />';
            shtml +=  '' + dt.Rows[0].Header2 + '<br />';
            shtml +=  '<strong>' + dt.Rows[0].NameofBranch + '</strong><br />';
            shtml +=  '' + dt.Rows[0].AdressofBranch + '<br />';
            shtml +=  '' + dt.Rows[0].BranchofContact + '<br />';
            shtml +=  '' + dt.Rows[0].BranchofEmail + '<br />';
            shtml +=  '' + dt.Rows[0].corp_address + '<br />';
            shtml +=  '' + dt.Rows[0].corp_telno + '</td>';
            shtml +=  '<td style="vertical-align: top; height: 15px">';
            shtml +=  'Transaction Date: ' + HTMLRep.formatDate(dt.Rows[0].TranxDate,"/") + '<br />';
	        if (dt.Rows[0].ClearingDate == '1900')
	        {
	        shtml +=  'Clearing Date: <br />';
	        }else{
	        shtml +=  'Clearing Date: '+ dt.Rows[0].ClearingDate +'<br />';
	        }
            shtml +=  'Time: '+ HTMLRep.formatTime(dt.Rows[0].TranxDate) +'<br />';
            shtml +=  'Loyalty Points:';
            shtml +=  '<br />';
            shtml +=  '' + dt.Rows[0].TranStatus + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 100%; font-size: 9pt; font-family: arial;';
            shtml +=  'height: 0.23in; border-top-width: thin; border-left-width: thin; border-left-color: black; border-bottom-width: thin; border-bottom-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="border-right: black thin solid; border-top: black thin solid;';
            shtml +=  'border-left: black thin solid; border-bottom: black thin solid">';
            shtml +=  '<table style="font-size: 9pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 244px; height: 14px">';
            shtml +=  '<strong>Remitter ID No.: </strong>' + dt.Rows[0].RemitterID + '</td>';
            shtml +=  '<td style="width: 285px; height: 14px">';
            shtml +=  '<strong>Official Receipt No.:</strong> ' + dt.Rows[0].ReceiptNo + '</td>';
            shtml +=  '<td style="width: 257px; height: 14px">';
            shtml +=  '<strong>Reference No.:</strong> ' + dt.Rows[0].ReferenceNo + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';    
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td style="border-right: black thin solid; border-top: black thin solid; vertical-align: top;';
            shtml +=  'border-left: black thin solid; width: 50%; border-bottom: black thin solid; height: 20px">';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 0px; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="vertical-align: top; width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 20px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 9pt; text-decoration: underline">REMITTER INFO</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="2">';
            shtml +=  '<span><span style="font-size: 8.5pt"><strong>&nbsp;Name: </strong>' + dt.Rows[0].RemitterName + '<br />';
            shtml +=  '</span></span>';
            shtml +=  '<table style="font-size: 8.5pt; width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="width: 61px; vertical-align: top;">';
            shtml +=  '<strong style="width: 30px">Address:<br />';
            shtml +=  '</strong></td>';
            shtml +=  '<td colspan="2" rowspan="3" style="height: 40px; vertical-align: top;">';
            shtml +=  '' + dt.Rows[0].RemitterAddress + '</td>';
            shtml +=  '</tr>';
            //shtml +=  '<tr>';
            //shtml +=  '</tr>';
            //shtml +=  '<tr>';
            //shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<span><span style="font-size: 8.5pt; width: 100%"><strong>Telephone Number:</strong>';
            shtml +=  '' + dt.Rows[0].RemitterTelNo + '</span></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="vertical-align: top; width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 20px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%; height: 20px" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 100% ; height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 9pt; text-decoration: underline">BENEFICIARY INFO</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="2">';
            shtml +=  '<span><span style="font-size: 8.5pt"><strong>&nbsp;Name:</strong> ' + dt.Rows[0].BenefName + '<br />';
            shtml +=  '</span></span>';
            shtml +=  '<table style="font-size: 8.5pt; width: 100%; vertical-align: top;" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td  style="width: 60px; vertical-align: top;"><strong style="width: 30px">Address:<br />';
            shtml +=  '</strong>';
            shtml +=  '</td>';
            shtml +=  '<td rowspan="3" style="width: 90%; height: 40px; vertical-align: top;">';
            shtml +=  '' + dt.Rows[0].BenefAddress + '</td>';
            shtml +=  '</tr>';
            //shtml +=  '<tr>';
            //shtml +=  '</tr>';
            //shtml +=  '<tr>';
            //shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<span><span style="font-size: 8.5pt"><strong>Telephone Number:</strong> ' + dt.Rows[0].BenefTelNo + '</span></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="vertical-align: top;  width: 100%; height: 20px; border-bottom: black thin solid;">';
            shtml +=  '<table style="font-size: 8.5pt; width: 100%; height: 20px" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 9pt; text-decoration: underline">TRANSACTION DETAILS</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 8.5pt">';
            shtml +=  '<td colspan="3" rowspan="2" style="font-size: 8.5pt">';
            shtml +=  '<table style="font-size: 8.5pt; width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Beneficiarys Account No. : </strong>' + dt.Rows[0].BenefAcctNo + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Thru PNB Branch: </strong>' + dt.Rows[0].ThruPNBBranch + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Other Banks Name / Branch: </strong>' + dt.Rows[0].OtherBankName + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Trans Type/Products: </strong>' + dt.Rows[0].TransType + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Source of Funds: </strong>' + dt.Rows[0].SourceFunds + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Member ID:</strong>' + dt.Rows[0].MemberID + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Purpose of Remittance: </strong>' + dt.Rows[0].PurposeRemittance + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Remittance Transaction Source: </strong>' + dt.Rows[0].RemittanceTranxSource + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 8.5pt">';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 8.5pt">';
            shtml +=  '<td rowspan="1" style="border-top-width: thin; border-left-width: thin; border-left-color: black;';
            shtml +=  'border-bottom-width: thin; border-bottom-color: black; vertical-align: top; width: 100%;';
            shtml +=  'border-top-color: black; height: 20px; border-right-width: thin; border-right-color: black">';
            shtml +=  '<table style="width: 100%; height: 20px;font-size: 8pt;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px">';
            shtml +=  '<p style="text-align: center">';
            shtml +=  '<span style="font-size: 9pt; text-decoration: underline"><strong>CLIENT/CUSTOMER UNDERTAKING</strong></span></p>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="2" style="text-align: justify;line-height: 14pt">';// line-height: 14pt;
            shtml +=  '<table style="width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="3" style="text-align: justify;font-size: 8.5pt;line-height: 14pt">'; //line-height: 14pt;                      
            shtml +=  'I/We hereby agree that the PNB GLOBAL REMITTANCE AND FINANCIAL COMPANY (HK) ';
            shtml +=  'LIMITED reserves the right to, at any time, obtain further information/document ';
            shtml +=  'from me/us.';
            shtml +=  '<br/>';
            //shtml +=  '<br/>';
            shtml +=  'I/We have read and understood the Notice to Customer relating to the Personal ';
            shtml +=  'Data (Privacy) Ordinance and all the terms and conditions printed overleaf.';
            //shtml +=  '<br/>';
            shtml +=  '<br/>';
            shtml +=  'I also confirm that details of my beneficiary are correct.';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<br />';
            shtml +=  '<hr style="width: 378px; height: 1px; text-align: center"/>';
            shtml +=  '<div style="width: 100%; height: 25px; text-align: center">';
            shtml +=  'Remitter&#039;s Signature</div>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            // Edited by CHR from colspan=2 to colspan=1
            shtml +=  '<td style="border-right: black thin solid; border-top: black thin solid;';
            shtml +=  'vertical-align: top; border-left: black thin solid; width: 50%; border-bottom: black thin solid;';
            shtml +=  'height: 20px">';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 100%; height: 90px">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 86px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="font-size: 8.5pt; width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 18px; text-align: center">';
            shtml +=  '<span style="font-size: 9pt; text-decoration: underline"><strong>REMITTANCE DETAILS</strong></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td style="text-align: left; width: 245px; height: 15px;">';
	        shtml +=  '    <strong>';
	        shtml +=  'Net Amount Due to Beneficiary:</strong></td>';
	        shtml +=  '<td style="width: 58px; height: 15px">';
	        if (dt.Rows[0].NetAmountDueToBenefCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].NetAmountDueToBenefCurrCode + '</td>';
	        }
	        shtml +=  '<td style="height: 15px; text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].NetAmountDueToBenefAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Exchange Rate Used:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].ExchangeRateUsedCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].ExchangeRateUsedCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].ExchangeRateUsedAmt,4) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Remittance Amount:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].RemittanceAmountCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].RemittanceAmountCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].RemittanceAmountAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Cable:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].CableCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].CableCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].CableAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Remittance Fee:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].RemittanceFeeCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].RemittanceFeeCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].RemittanceFeeAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Other Charges:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].OtherChargesCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].OtherChargesCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].OtherChargesAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="width: 221px">';
	        shtml +=  '<strong>';
	        shtml +=  'Total amount received from customer:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].TotalAmtRcvdFromCustCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].TotalAmtRcvdFromCustCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].TotalAmtRcvdFromCustAmt,2) + '</td>';
	        shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 70px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;font-size: 8.5pt">';
	        shtml +=  '' + dt.Rows[0].Check + '<br />';
	        shtml +=  '' + dt.Rows[0].Cash + '<br />';
	        shtml +=  '' + dt.Rows[0].SpecialMessage + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="font-size:8.5pt;width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 40px; text-align: center; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 177px;font-size:8.5pt">';
            shtml +=  '<strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Payment ';
            shtml +=  '<br />';
            shtml +=  'Received / Processed By:</strong></td>';
            shtml +=  '<td style="font-size:8.5pt;">';
            shtml +=  '' + dt.Rows[0].ProcessedBy + '<br />';
            shtml +=  '<hr style="width: 200px; height: 2px;background-color:black; text-align: left;" />';/////
            shtml +=  '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Authorized Personnel&nbsp;</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="border-top-width: thin; border-left-width: thin; border-left-color: black;';
            shtml +=  'border-bottom-width: thin; border-bottom-color: black; vertical-align: top; width: 100%;';
            shtml +=  'border-top-color: black; height: 20px; text-align: center; border-right-width: thin;';
            shtml +=  'border-right-color: black">';
            shtml +=  '<table style="width: 100%; height: 20px" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px;text-align:center;">';
            shtml +=  '<span style="font-size: 9pt; text-decoration: underline"><strong>DECLARATIONS</strong></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            if (dt.Rows[0].ReceiptDeclarationSub5.toString().length == 0)
            {
                       shtml +=  '<td colspan="3" rowspan="2" style="font-size: 8.5pt;text-align: justify;line-height: 12pt">'; //line-height: 14pt;
            }else{
                        shtml +=  '<td colspan="3" rowspan="2" style="font-size: 8.5pt;text-align: justify; line-height: 12pt">';//line-height: 14pt;
            }
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub1 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub2 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub3 + '<br />';   
           // shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub4 + '<br />';
            //shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub5 + '<br />';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            
            //START - HK OBP 2nd copy --Added by Ruth for RFS 14332-014
            shtml +=  '<br />';
            shtml +=  '<br />';
            shtml +=  '<br />';
            shtml +=  '<br />';
            shtml +=  '<br />';
            shtml +=  '<br />';
            shtml +=  '<br />';
            shtml +=  '<br />';
            shtml +=  '<table style="width: 100%; font-size: 8pt; font-family: arial;" cellspacing="0" cellpadding="0">';
            //shtml +=  '<table style="width: 8.5in; font-size: 7pt; font-family: arial;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="width: 0in">';
            shtml +=  '<table style="width: 100%;font-size: 8pt" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 60px; height: 15px">';
            shtml +=  '<img src="../../../'+ vd +'/images/pnb_black_logo.JPG" style="width: 58px; height: 68px" /></td>';
            shtml +=  '<td style="vertical-align: top; width: 520px; height: 15px">';
            shtml +=  '<strong>' + dt.Rows[0].Header1 + '</strong><br />';
            shtml +=  '' + dt.Rows[0].Header2 + '<br />';
            shtml +=  '<strong>' + dt.Rows[0].NameofBranch + '</strong><br />';
            shtml +=  '' + dt.Rows[0].AdressofBranch + '<br />';
            shtml +=  '' + dt.Rows[0].BranchofContact + '<br />';
            shtml +=  '' + dt.Rows[0].BranchofEmail + '<br />';
            shtml +=  '' + dt.Rows[0].corp_address + '<br />';
            shtml +=  '' + dt.Rows[0].corp_telno + '</td>';
            shtml +=  '<td style="vertical-align: top; height: 15px">';
            shtml +=  'Transaction Date: ' + HTMLRep.formatDate(dt.Rows[0].TranxDate,"/") + '<br />';
	        if (dt.Rows[0].ClearingDate == '1900')
	        {
	        shtml +=  'Clearing Date: <br />';
	        }else{
	        shtml +=  'Clearing Date: '+ dt.Rows[0].ClearingDate +'<br />';
	        }
            shtml +=  'Time: '+ HTMLRep.formatTime(dt.Rows[0].TranxDate) +'<br />';
            shtml +=  'Loyalty Points:';
            shtml +=  '<br />';
            shtml +=  '' + dt.Rows[0].TranStatus + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 100%; font-size: 9pt; font-family: arial;';
            shtml +=  'height: 0.23in; border-top-width: thin; border-left-width: thin; border-left-color: black; border-bottom-width: thin; border-bottom-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="border-right: black thin solid; border-top: black thin solid;';
            shtml +=  'border-left: black thin solid; border-bottom: black thin solid">';
            shtml +=  '<table style="font-size: 9pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 244px; height: 14px">';
            shtml +=  '<strong>Remitter ID No.: </strong>' + dt.Rows[0].RemitterID + '</td>';
            shtml +=  '<td style="width: 285px; height: 14px">';
            shtml +=  '<strong>Official Receipt No.:</strong> ' + dt.Rows[0].ReceiptNo + '</td>';
            shtml +=  '<td style="width: 257px; height: 14px">';
            shtml +=  '<strong>Reference No.:</strong> ' + dt.Rows[0].ReferenceNo + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';    
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td style="border-right: black thin solid; border-top: black thin solid; vertical-align: top;';
            shtml +=  'border-left: black thin solid; width: 50%; border-bottom: black thin solid; height: 20px">';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 0px; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="vertical-align: top; width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 20px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 9pt; text-decoration: underline">REMITTER INFO</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="2">';
            shtml +=  '<span><span style="font-size: 8.5pt"><strong>&nbsp;Name: </strong>' + dt.Rows[0].RemitterName + '<br />';
            shtml +=  '</span></span>';
            shtml +=  '<table style="font-size: 8.5pt; width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="width: 61px; vertical-align: top;">';
            shtml +=  '<strong style="width: 30px">Address:<br />';
            shtml +=  '</strong></td>';
            shtml +=  '<td colspan="2" rowspan="3" style="height: 40px; vertical-align: top;">';
            shtml +=  '' + dt.Rows[0].RemitterAddress + '</td>';
            shtml +=  '</tr>';
            //shtml +=  '<tr>';
            //shtml +=  '</tr>';
            //shtml +=  '<tr>';
            //shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<span><span style="font-size: 8.5pt; width: 100%"><strong>Telephone Number:</strong>';
            shtml +=  '' + dt.Rows[0].RemitterTelNo + '</span></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="vertical-align: top; width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 20px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%; height: 20px" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 100% ; height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 9pt; text-decoration: underline">BENEFICIARY INFO</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="2">';
            shtml +=  '<span><span style="font-size: 8.5pt"><strong>&nbsp;Name:</strong> ' + dt.Rows[0].BenefName + '<br />';
            shtml +=  '</span></span>';
            shtml +=  '<table style="font-size: 8.5pt; width: 100%; vertical-align: top;" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td  style="width: 60px; vertical-align: top;"><strong style="width: 30px">Address:<br />';
            shtml +=  '</strong>';
            shtml +=  '</td>';
            shtml +=  '<td rowspan="3" style="width: 90%; height: 40px; vertical-align: top;">';
            shtml +=  '' + dt.Rows[0].BenefAddress + '</td>';
            shtml +=  '</tr>';
            //shtml +=  '<tr>';
            //shtml +=  '</tr>';
            //shtml +=  '<tr>';
            //shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<span><span style="font-size: 8.5pt"><strong>Telephone Number:</strong> ' + dt.Rows[0].BenefTelNo + '</span></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="vertical-align: top;  width: 100%; height: 20px; border-bottom: black thin solid;">';
            shtml +=  '<table style="font-size: 8.5pt; width: 100%; height: 20px" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 9pt; text-decoration: underline">TRANSACTION DETAILS</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 8.5pt">';
            shtml +=  '<td colspan="3" rowspan="2" style="font-size: 8.5pt">';
            shtml +=  '<table style="font-size: 8.5pt; width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Beneficiarys Account No. : </strong>' + dt.Rows[0].BenefAcctNo + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Thru PNB Branch: </strong>' + dt.Rows[0].ThruPNBBranch + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Other Banks Name / Branch: </strong>' + dt.Rows[0].OtherBankName + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Trans Type/Products: </strong>' + dt.Rows[0].TransType + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Source of Funds: </strong>' + dt.Rows[0].SourceFunds + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Member ID:</strong>' + dt.Rows[0].MemberID + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Purpose of Remittance: </strong>' + dt.Rows[0].PurposeRemittance + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Remittance Transaction Source: </strong>' + dt.Rows[0].RemittanceTranxSource + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 8.5pt">';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 8.5pt">';
            shtml +=  '<td rowspan="1" style="border-top-width: thin; border-left-width: thin; border-left-color: black;';
            shtml +=  'border-bottom-width: thin; border-bottom-color: black; vertical-align: top; width: 100%;';
            shtml +=  'border-top-color: black; height: 20px; border-right-width: thin; border-right-color: black">';
            shtml +=  '<table style="width: 100%; height: 20px;font-size: 8pt;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px">';
            shtml +=  '<p style="text-align: center">';
            shtml +=  '<span style="font-size: 9pt; text-decoration: underline"><strong>CLIENT/CUSTOMER UNDERTAKING</strong></span></p>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="2" style="text-align: justify;line-height: 14pt">';// line-height: 14pt;
            shtml +=  '<table style="width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="3" style="text-align: justify;font-size: 8.5pt;line-height: 14pt">'; //line-height: 14pt;                      
            shtml +=  'I/We hereby agree that the PNB GLOBAL REMITTANCE AND FINANCIAL COMPANY (HK) ';
            shtml +=  'LIMITED reserves the right to, at any time, obtain further information/document ';
            shtml +=  'from me/us.';
            shtml +=  '<br/>';
            //shtml +=  '<br/>';
            shtml +=  'I/We have read and understood the Notice to Customer relating to the Personal ';
            shtml +=  'Data (Privacy) Ordinance and all the terms and conditions printed overleaf.';
            //shtml +=  '<br/>';
            shtml +=  '<br/>';
            shtml +=  'I also confirm that details of my beneficiary are correct.';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<br />';
            shtml +=  '<hr style="width: 378px; height: 1px; text-align: center"/>';
            shtml +=  '<div style="width: 100%; height: 25px; text-align: center">';
            shtml +=  'Remitter&#039;s Signature</div>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            // Edited by CHR from colspan=2 to colspan=1
            shtml +=  '<td style="border-right: black thin solid; border-top: black thin solid;';
            shtml +=  'vertical-align: top; border-left: black thin solid; width: 50%; border-bottom: black thin solid;';
            shtml +=  'height: 20px">';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 100%; height: 90px">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 86px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="font-size: 8.5pt; width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 18px; text-align: center">';
            shtml +=  '<span style="font-size: 9pt; text-decoration: underline"><strong>REMITTANCE DETAILS</strong></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td style="text-align: left; width: 245px; height: 15px;">';
	        shtml +=  '    <strong>';
	        shtml +=  'Net Amount Due to Beneficiary:</strong></td>';
	        shtml +=  '<td style="width: 58px; height: 15px">';
	        if (dt.Rows[0].NetAmountDueToBenefCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].NetAmountDueToBenefCurrCode + '</td>';
	        }
	        shtml +=  '<td style="height: 15px; text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].NetAmountDueToBenefAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Exchange Rate Used:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].ExchangeRateUsedCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].ExchangeRateUsedCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].ExchangeRateUsedAmt,4) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Remittance Amount:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].RemittanceAmountCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].RemittanceAmountCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].RemittanceAmountAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Cable:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].CableCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].CableCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].CableAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Remittance Fee:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].RemittanceFeeCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].RemittanceFeeCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].RemittanceFeeAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Other Charges:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].OtherChargesCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].OtherChargesCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].OtherChargesAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="width: 221px">';
	        shtml +=  '<strong>';
	        shtml +=  'Total amount received from customer:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].TotalAmtRcvdFromCustCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].TotalAmtRcvdFromCustCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].TotalAmtRcvdFromCustAmt,2) + '</td>';
	        shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 70px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;font-size: 8.5pt">';
	        shtml +=  '' + dt.Rows[0].Check + '<br />';
	        shtml +=  '' + dt.Rows[0].Cash + '<br />';
	        shtml +=  '' + dt.Rows[0].SpecialMessage + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="font-size:8.5pt;width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 40px; text-align: center; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 177px;font-size:8.5pt">';
            shtml +=  '<strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Payment ';
            shtml +=  '<br />';
            shtml +=  'Received / Processed By:</strong></td>';
            shtml +=  '<td style="font-size:8.5pt;">';
            shtml +=  '' + dt.Rows[0].ProcessedBy + '<br />';
            shtml +=  '<hr style="width: 200px; height: 2px;background-color:black; text-align: left;" />';/////
            shtml +=  '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Authorized Personnel&nbsp;</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="border-top-width: thin; border-left-width: thin; border-left-color: black;';
            shtml +=  'border-bottom-width: thin; border-bottom-color: black; vertical-align: top; width: 100%;';
            shtml +=  'border-top-color: black; height: 20px; text-align: center; border-right-width: thin;';
            shtml +=  'border-right-color: black">';
            shtml +=  '<table style="width: 100%; height: 20px" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px;text-align:center;">';
            shtml +=  '<span style="font-size: 9pt; text-decoration: underline"><strong>DECLARATIONS</strong></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            if (dt.Rows[0].ReceiptDeclarationSub5.toString().length == 0)
            {
                       shtml +=  '<td colspan="3" rowspan="2" style="font-size: 8.5pt;text-align: justify;line-height: 12pt">'; //line-height: 14pt;
            }else{
                        shtml +=  '<td colspan="3" rowspan="2" style="font-size: 8.5pt;text-align: justify; line-height: 12pt">';//line-height: 14pt;
            }
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub1 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub2 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub3 + '<br />';   
           // shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub4 + '<br />';
            //shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub5 + '<br />';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            //END - HK OBP 2nd copy
            
            
            shtml +=  '</div>';

            return shtml;
    }
}
/*END HONGKONG OBP RECEIPT -- ONE PAGE*/

//FOR ONE PAGE RECEIPT
/*START OTC RECEIPT - ONE PAGE*/
function HTMLReceiptOTC_OnePage()
{
    var cls = this;
    var _branchcode;    
    var _seqid;
	var _referenceno;
    
    this.evaluate_Receipt_query = function(query)
    {
        var squery = unescape(query.toString());
        if (squery.toLowerCase().indexOf("trantype=otc") > -1)
        {
            _branchcode = HTMLRep.get_branchcode();
            this.get_Receipt_data(
                                    HTMLRep.get_querystring_value(query,"tranvalue"),
                                    HTMLRep.get_branchcode(),
                                    ""
                                 )
            return;
        }
        if (squery.toLowerCase().indexOf("mergeparam") > -1)
        {
             _branchcode = HTMLRep.parse_merge_param_query(squery,"T@v_branchcode")
             this.get_Receipt_data(
                                    "",
                                    HTMLRep.parse_merge_param_query(squery,"T@v_branchcode"),
                                    HTMLRep.parse_merge_param_query(squery,"T@v_referenceno")
									)
            return;
        }
    }
    this.check_newdbswitch = function()
    {
    
	var pass = false;
	var sp_name = 'proc_check_newdbswitch';
        var squery = HTMLRep.get_hidden_querystring("hidQueryString");
        var sbranchcode = HTMLRep.get_branchcode();
        
        var params  =  '@branchcode=' + sbranchcode;
        var isRCC  = false;
        AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
            alert(errmsg);     return;        
        } 
        
        var dtcchecksw = IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params).value;
        
        try
        {
            if (dtcchecksw.Tables[0].Rows.length > 0)
            {
                pass = true;
            }
        }
        catch(e)
        {
        }
	
	return pass; 
    }
    this.get_Receipt_data = function(seq_id,branchcode,referenceno)
    {
        var params; 
        var sp_name = "";
	var pass = false;
        
                _branchcode =   " @v_branchcode="  + branchcode;    
                _seqid    =   "@v_seqid="     + seq_id;
				_referenceno = " @v_referenceno="     + referenceno;
        
       if(seq_id.length == 0)
	   {
			pass = this.check_newdbswitch();
			if (pass)
			{
			    sp_name = "SP_OTC_Reprint_Receipt_irsw";
			}
			else
			{
			    sp_name = "SP_OTC_Reprint_Receipt";
			}
			params  = _branchcode + "^" + _referenceno ;
	   }else{
			sp_name = "SP_OTC_Receipt";
			params  = _branchcode + "^" + _seqid ;
		}
		AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
            alert(errmsg);       return;      
        } 
	    IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params,this.get_Receipt_data_callback);
    }
    
    this.get_Receipt_data_callback = function(result)
    {
        try
        {
            var ds = result.value;
            
            cls.check_branch();
            cls.display_Receipt_report(ds);
            
            
        }
        catch(e)
        {
            alert("ERROR WHILE GETTING Receipt DATA");
        }
    }
    
    this.display_Receipt_report = function(ds)
    {
        var sp_name = 'proc_check_reporthalfpage';
        var squery = HTMLRep.get_hidden_querystring("hidQueryString");
        var sbranchcode = ds.Tables[0].Rows[0].ReferenceNo;
        sbranchcode =  sbranchcode.substring(1,4);
        var params  =  '@v_branhcode=' + sbranchcode;
        var ishalfpage  = false;
        AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
            alert(errmsg);     return;        
        } 
        
        var dtcheckhalfpage = IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params).value;
        
        try
        {
            if (dtcheckhalfpage.Tables[0].Rows.length > 0)
            {
                ishalfpage = true;
            }
        }
        catch(e)
        {
        }
    
        
        var shtml = "";        
        shtml +=  HTMLRep.print_html_button();
        var isRCC = cls.check_branch();
        if (!ishalfpage)
        {
           
            shtml +=  this.Receipt_body(ds.Tables[0],isRCC);
               
            shtml += cls.half_page(ds.Tables[0]);
            
	        shtml +=  '</div>';
        }
        else
        {
           shtml +=  this.Receipt_body_halfpage(ds.Tables[0],isRCC);
           
        }
        document.getElementById("bodydiv").innerHTML = shtml
    }
    this.check_branch = function()
    {
        var sp_name = 'sp_report_checkbranch';
        var squery = HTMLRep.get_hidden_querystring("hidQueryString");
        var sbranchcode = HTMLRep.get_branchcode();
        
        var params  =  '@branchcode=' + sbranchcode;
        var isRCC  = false;
        AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
            alert(errmsg);     return;        
        } 
        
        var dtccheckbranch = IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params).value;
        
        try
        {
            if (dtccheckbranch.Tables[0].Rows.length > 0)
            {
                isRCC = true;
            }
        }
        catch(e)
        {
        }
        return isRCC;
    }
    this.Receipt_body = function(dt,isRCC)
    {
	
	    var vd = window.location.href.split('/')[3];
	    
        var shtml = "";
	        shtml +=  '<br />';
            shtml +=  '<br />';
            shtml +=  '<div style="width: 970px; margin: 0px;">';
            shtml +=  '<table style="width: 100%; font-size: 9.5pt; font-family: arial;" cellspacing="0" cellpadding="0">';
            //shtml +=  '<table style="width: 8.5in; font-size: 7pt; font-family: arial;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="width: 0in">';
            shtml +=  '<table style="width: 100%;font-size: 9pt" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 60px; height: 15px">';
            shtml +=  '<img src="../../../'+ vd +'/images/pnb_black_logo.JPG" style="width: 58px; height: 68px" /></td>';
            shtml +=  '<td style="vertical-align: top; width: 520px; height: 15px">';
            shtml +=  '<strong>' + dt.Rows[0].Header1 + '</strong><br />';
            shtml +=  '' + dt.Rows[0].Header2 + '<br />';
            shtml +=  '<strong>' + dt.Rows[0].NameofBranch + '</strong><br />';
            shtml +=  '' + dt.Rows[0].AdressofBranch + '<br />';
            shtml +=  '' + dt.Rows[0].BranchofContact + '<br />';
            shtml +=  '' + dt.Rows[0].BranchofEmail + '<br />';
            shtml +=  '' + dt.Rows[0].corp_address + '<br />';
            shtml +=  '' + dt.Rows[0].corp_telno + '</td>';
            shtml +=  '<td style="vertical-align: top; height: 15px">';
            shtml +=  'Transaction Date: ' + HTMLRep.formatDate(dt.Rows[0].TranxDate,"/") + '<br />';
	        if (dt.Rows[0].ClearingDate == '1900')
	        {
	        shtml +=  'Clearing Date: <br />';
	        }else{
	        shtml +=  'Clearing Date: '+ dt.Rows[0].ClearingDate +'<br />';
	        }
            shtml +=  'Time: '+ HTMLRep.formatTime(dt.Rows[0].TranxDate) +'<br />';
            shtml +=  'Loyalty Points:';
            shtml +=  '<br />';
            shtml +=  '' + dt.Rows[0].TranStatus + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 100%; font-size: 9.5pt; font-family: arial;';
            shtml +=  'height: 0.23in; border-top-width: thin; border-left-width: thin; border-left-color: black; border-bottom-width: thin; border-bottom-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="border-right: black thin solid; border-top: black thin solid;';
            shtml +=  'border-left: black thin solid; border-bottom: black thin solid">';
            shtml +=  '<table style="font-size: 9.5pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 244px; height: 14px">';
            shtml +=  '<strong>Remitter ID No.: </strong>' + dt.Rows[0].RemitterID + '</td>';
            shtml +=  '<td style="width: 285px; height: 14px">';
            shtml +=  '<strong>Official Receipt No.:</strong> ' + dt.Rows[0].ReceiptNo + '</td>';
            shtml +=  '<td style="width: 257px; height: 14px">';
            shtml +=  '<strong>Reference No.:</strong> ' + dt.Rows[0].ReferenceNo + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';    
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td style="border-right: black thin solid; border-top: black thin solid; vertical-align: top;';
            shtml +=  'border-left: black thin solid; width: 50%; border-bottom: black thin solid; height: 20px">';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 0px; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="vertical-align: top; width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 20px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 9.5pt; text-decoration: underline">REMITTER INFO</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="2">';
            shtml +=  '<span><span style="font-size: 9pt"><strong>&nbsp;Name: </strong>' + dt.Rows[0].RemitterName + '<br />';
            shtml +=  '</span></span>';
            shtml +=  '<table style="font-size: 9pt; width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="width: 61px; vertical-align: top;">';
            shtml +=  '<strong style="width: 30px">Address:<br />';
            shtml +=  '</strong></td>';
            shtml +=  '<td colspan="2" rowspan="3" style="height: 40px; vertical-align: top;">';
            shtml +=  '' + dt.Rows[0].RemitterAddress + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<span><span style="font-size: 9pt; width: 100%"><strong>Telephone Number:</strong>';
            shtml +=  '' + dt.Rows[0].RemitterTelNo + '</span></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="vertical-align: top; width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 20px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%; height: 20px" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 100% ; height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 9.5pt; text-decoration: underline">BENEFICIARY INFO</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="2">';
            shtml +=  '<span><span style="font-size: 9pt"><strong>&nbsp;Name:</strong> ' + dt.Rows[0].BenefName + '<br />';
            shtml +=  '</span></span>';
            shtml +=  '<table style="font-size: 9pt; width: 100%; vertical-align: top;" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td  style="width: 60px; vertical-align: top;"><strong style="width: 30px">Address:<br />';
            shtml +=  '</strong>';
            shtml +=  '</td>';
            shtml +=  '<td rowspan="3" style="width: 90%; height: 40px; vertical-align: top;">';
            shtml +=  '' + dt.Rows[0].BenefAddress + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<span><span style="font-size: 9pt"><strong>Telephone Number:</strong> ' + dt.Rows[0].BenefTelNo + '</span></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="vertical-align: top;  width: 100%; height: 20px; border-bottom: black thin solid;">';
            shtml +=  '<table style="font-size: 9pt; width: 100%; height: 20px" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 9.5pt; text-decoration: underline">TRANSACTION DETAILS</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 9pt">';
            shtml +=  '<td colspan="3" rowspan="2" style="font-size: 8.5pt">';
            shtml +=  '<table style="font-size: 9pt; width: 100%; line-height: 14pt" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Beneficiarys Account No. : </strong>' + dt.Rows[0].BenefAcctNo + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Thru PNB Branch: </strong>' + dt.Rows[0].ThruPNBBranch + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Other Banks Name / Branch: </strong>' + dt.Rows[0].OtherBankName + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Trans Type/Products: </strong>' + dt.Rows[0].TransType + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Source of Funds: </strong>' + dt.Rows[0].SourceFunds + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Member ID:</strong>' + dt.Rows[0].MemberID + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Purpose of Remittance: </strong>' + dt.Rows[0].PurposeRemittance + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Remittance Transaction Source: </strong>' + dt.Rows[0].RemittanceTranxSource + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 9pt">';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 9pt">';
            shtml +=  '<td rowspan="1" style="border-top-width: thin; border-left-width: thin; border-left-color: black;';
            shtml +=  'border-bottom-width: thin; border-bottom-color: black; vertical-align: top; width: 100%;';
            shtml +=  'border-top-color: black; height: 20px; border-right-width: thin; border-right-color: black">';
            shtml +=  '<table style="width: 100%; height: 20px;font-size: 8pt;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px">';
            shtml +=  '<p style="text-align: center">';
            shtml +=  '<span style="font-size: 9.5pt; text-decoration: underline"><strong>CLIENT/CUSTOMER UNDERTAKING</strong></span></p>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="2" style="text-align: justify;line-height: 16pt">';// line-height: 14pt;
            shtml +=  '<table style="width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="3" style="text-align: justify;font-size: 9pt;line-height: 15pt">'; //line-height: 14pt;                      
            shtml +=  'I certify that all the information contained herein is true and correct, and that';
            shtml +=  'I am amenable to the Terms and Conditions (if any) governing this transaction.';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<br />';
            shtml +=  '<hr style="width: 378px; height: 1px; text-align: center"/>';
            shtml +=  '<div style="width: 100%; height: 25px; text-align: center">';
            shtml +=  'Remitter&#039;s Signature</div>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            // Edited by CHR from colspan=2 to colspan=1
            shtml +=  '<td style="border-right: black thin solid; border-top: black thin solid;';
            shtml +=  'vertical-align: top; border-left: black thin solid; width: 50%; border-bottom: black thin solid;';
            shtml +=  'height: 20px">';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 100%; height: 90px">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 86px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="font-size: 9pt; width: 100%; line-height: 14pt" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 18px; text-align: center">';
            shtml +=  '<span style="font-size: 9.5pt; text-decoration: underline"><strong>REMITTANCE DETAILS</strong></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td style="text-align: left; width: 245px; height: 15px;">';
	        shtml +=  '    <strong>';
	        shtml +=  'Net Amount Due to Beneficiary:</strong></td>';
	        shtml +=  '<td style="width: 58px; height: 15px">';
	        if (dt.Rows[0].NetAmountDueToBenefCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].NetAmountDueToBenefCurrCode + '</td>';
	        }
	        shtml +=  '<td style="height: 15px; text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].NetAmountDueToBenefAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Exchange Rate Used:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].ExchangeRateUsedCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].ExchangeRateUsedCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].ExchangeRateUsedAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Remittance Amount:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].RemittanceAmountCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].RemittanceAmountCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].RemittanceAmountAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Cable:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].CableCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].CableCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].CableAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Remittance Fee:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].RemittanceFeeCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].RemittanceFeeCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].RemittanceFeeAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Other Charges:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].OtherChargesCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].OtherChargesCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].OtherChargesAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="width: 221px">';
	        shtml +=  '<strong>';
	        shtml +=  'Total amount received from customer:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].TotalAmtRcvdFromCustCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].TotalAmtRcvdFromCustCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].TotalAmtRcvdFromCustAmt,2) + '</td>';
	        shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 70px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;font-size: 9pt">';
	        shtml +=  '' + dt.Rows[0].Check + '<br />';
	        shtml +=  '' + dt.Rows[0].Cash + '<br />';
	        shtml +=  '' + dt.Rows[0].SpecialMessage + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="font-size:9pt;width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 40px; text-align: center; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 177px;font-size:9pt">';
            shtml +=  '<strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Payment ';
            shtml +=  '<br />';
            shtml +=  'Received / Processed By:</strong></td>';
            shtml +=  '<td style="font-size:9pt;">';
            shtml +=  '' + dt.Rows[0].ProcessedBy + '<br />';
            shtml +=  '<hr style="width: 200px; height: 2px;background-color:black; text-align: left;" />';/////
            shtml +=  '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Authorized Personnel&nbsp;</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="border-top-width: thin; border-left-width: thin; border-left-color: black;';
            shtml +=  'border-bottom-width: thin; border-bottom-color: black; vertical-align: top; width: 100%;';
            shtml +=  'border-top-color: black; height: 20px; text-align: center; border-right-width: thin;';
            shtml +=  'border-right-color: black">';
            shtml +=  '<table style="width: 100%; height: 20px" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px;text-align:center;">';
            shtml +=  '<span style="font-size: 9.5pt; text-decoration: underline; line-height: 16pt"><strong>DECLARATIONS</strong></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            if (dt.Rows[0].ReceiptDeclarationSub5.toString().length == 0)
            {
                       shtml +=  '<td colspan="3" rowspan="2" style="font-size: 9pt;text-align: justify;line-height: 15pt">'; //line-height: 14pt;
            }else{
                        shtml +=  '<td colspan="3" rowspan="2" style="font-size: 9pt;text-align: justify; line-height: 15pt">';//line-height: 14pt;
            }
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub1 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub2 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub3 + '<br />';   
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub4 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub5 + '<br />';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            
              //START - FOR OTC 2nd Copy
            shtml +=  '<br />';
            shtml +=  '<br />';
            shtml +=  '<br />';
            shtml +=  '<br />';
            //shtml +=  '<br />';
            shtml +=  '<table style="width: 100%; font-size: 9.5pt; font-family: arial;" cellspacing="0" cellpadding="0">';
            //shtml +=  '<table style="width: 8.5in; font-size: 7pt; font-family: arial;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="width: 0in">';
            shtml +=  '<table style="width: 100%;font-size: 9pt" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 60px; height: 15px">';
            shtml +=  '<img src="../../../'+ vd +'/images/pnb_black_logo.JPG" style="width: 58px; height: 68px" /></td>';
            shtml +=  '<td style="vertical-align: top; width: 520px; height: 15px">';
            shtml +=  '<strong>' + dt.Rows[0].Header1 + '</strong><br />';
            shtml +=  '' + dt.Rows[0].Header2 + '<br />';
            shtml +=  '<strong>' + dt.Rows[0].NameofBranch + '</strong><br />';
            shtml +=  '' + dt.Rows[0].AdressofBranch + '<br />';
            shtml +=  '' + dt.Rows[0].BranchofContact + '<br />';
            shtml +=  '' + dt.Rows[0].BranchofEmail + '<br />';
            shtml +=  '' + dt.Rows[0].corp_address + '<br />';
            shtml +=  '' + dt.Rows[0].corp_telno + '</td>';
            shtml +=  '<td style="vertical-align: top; height: 15px">';
            shtml +=  'Transaction Date: ' + HTMLRep.formatDate(dt.Rows[0].TranxDate,"/") + '<br />';
	        if (dt.Rows[0].ClearingDate == '1900')
	        {
	        shtml +=  'Clearing Date: <br />';
	        }else{
	        shtml +=  'Clearing Date: '+ dt.Rows[0].ClearingDate +'<br />';
	        }
            shtml +=  'Time: '+ HTMLRep.formatTime(dt.Rows[0].TranxDate) +'<br />';
            shtml +=  'Loyalty Points:';
            shtml +=  '<br />';
            shtml +=  '' + dt.Rows[0].TranStatus + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 100%; font-size: 9.5pt; font-family: arial;';
            shtml +=  'height: 0.23in; border-top-width: thin; border-left-width: thin; border-left-color: black; border-bottom-width: thin; border-bottom-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="border-right: black thin solid; border-top: black thin solid;';
            shtml +=  'border-left: black thin solid; border-bottom: black thin solid">';
            shtml +=  '<table style="font-size: 9.5pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 244px; height: 14px">';
            shtml +=  '<strong>Remitter ID No.: </strong>' + dt.Rows[0].RemitterID + '</td>';
            shtml +=  '<td style="width: 285px; height: 14px">';
            shtml +=  '<strong>Official Receipt No.:</strong> ' + dt.Rows[0].ReceiptNo + '</td>';
            shtml +=  '<td style="width: 257px; height: 14px">';
            shtml +=  '<strong>Reference No.:</strong> ' + dt.Rows[0].ReferenceNo + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';    
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td style="border-right: black thin solid; border-top: black thin solid; vertical-align: top;';
            shtml +=  'border-left: black thin solid; width: 50%; border-bottom: black thin solid; height: 20px">';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 0px; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="vertical-align: top; width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 20px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 9.5pt; text-decoration: underline">REMITTER INFO</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="2">';
            shtml +=  '<span><span style="font-size: 9pt"><strong>&nbsp;Name: </strong>' + dt.Rows[0].RemitterName + '<br />';
            shtml +=  '</span></span>';
            shtml +=  '<table style="font-size: 9pt; width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="width: 61px; vertical-align: top;">';
            shtml +=  '<strong style="width: 30px">Address:<br />';
            shtml +=  '</strong></td>';
            shtml +=  '<td colspan="2" rowspan="3" style="height: 40px; vertical-align: top;">';
            shtml +=  '' + dt.Rows[0].RemitterAddress + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<span><span style="font-size: 9pt; width: 100%"><strong>Telephone Number:</strong>';
            shtml +=  '' + dt.Rows[0].RemitterTelNo + '</span></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="vertical-align: top; width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 20px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%; height: 20px" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 100% ; height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 9.5pt; text-decoration: underline">BENEFICIARY INFO</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="2">';
            shtml +=  '<span><span style="font-size: 9pt"><strong>&nbsp;Name:</strong> ' + dt.Rows[0].BenefName + '<br />';
            shtml +=  '</span></span>';
            shtml +=  '<table style="font-size: 9pt; width: 100%; vertical-align: top;" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td  style="width: 60px; vertical-align: top;"><strong style="width: 30px">Address:<br />';
            shtml +=  '</strong>';
            shtml +=  '</td>';
            shtml +=  '<td rowspan="3" style="width: 90%; height: 40px; vertical-align: top;">';
            shtml +=  '' + dt.Rows[0].BenefAddress + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<span><span style="font-size: 9pt"><strong>Telephone Number:</strong> ' + dt.Rows[0].BenefTelNo + '</span></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="vertical-align: top;  width: 100%; height: 20px; border-bottom: black thin solid;">';
            shtml +=  '<table style="font-size: 9pt; width: 100%; height: 20px" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 9.5pt; text-decoration: underline">TRANSACTION DETAILS</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 9pt">';
            shtml +=  '<td colspan="3" rowspan="2" style="font-size: 8.5pt">';
            shtml +=  '<table style="font-size: 9pt; width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Beneficiarys Account No. : </strong>' + dt.Rows[0].BenefAcctNo + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Thru PNB Branch: </strong>' + dt.Rows[0].ThruPNBBranch + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Other Banks Name / Branch: </strong>' + dt.Rows[0].OtherBankName + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Trans Type/Products: </strong>' + dt.Rows[0].TransType + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Source of Funds: </strong>' + dt.Rows[0].SourceFunds + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Member ID:</strong>' + dt.Rows[0].MemberID + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Purpose of Remittance: </strong>' + dt.Rows[0].PurposeRemittance + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Remittance Transaction Source: </strong>' + dt.Rows[0].RemittanceTranxSource + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 9pt">';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 9pt">';
            shtml +=  '<td rowspan="1" style="border-top-width: thin; border-left-width: thin; border-left-color: black;';
            shtml +=  'border-bottom-width: thin; border-bottom-color: black; vertical-align: top; width: 100%;';
            shtml +=  'border-top-color: black; height: 20px; border-right-width: thin; border-right-color: black">';
            shtml +=  '<table style="width: 100%; height: 20px;font-size: 8pt;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px">';
            shtml +=  '<p style="text-align: center">';
            shtml +=  '<span style="font-size: 9.5pt; text-decoration: underline"><strong>CLIENT/CUSTOMER UNDERTAKING</strong></span></p>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="2" style="text-align: justify;line-height: 16pt">';// line-height: 14pt;
            shtml +=  '<table style="width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="3" style="text-align: justify;font-size: 9pt;line-height: 14pt">'; //line-height: 14pt;                      
            shtml +=  'I certify that all the information contained herein is true and correct, and that';
            shtml +=  'I am amenable to the Terms and Conditions (if any) governing this transaction.';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<br />';
            shtml +=  '<hr style="width: 378px; height: 1px; text-align: center"/>';
            shtml +=  '<div style="width: 100%; height: 25px; text-align: center">';
            shtml +=  'Remitter&#039;s Signature</div>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            // Edited by CHR from colspan=2 to colspan=1
            shtml +=  '<td style="border-right: black thin solid; border-top: black thin solid;';
            shtml +=  'vertical-align: top; border-left: black thin solid; width: 50%; border-bottom: black thin solid;';
            shtml +=  'height: 20px">';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 100%; height: 90px">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 86px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="font-size: 9pt; width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 18px; text-align: center">';
            shtml +=  '<span style="font-size: 9.5pt; text-decoration: underline"><strong>REMITTANCE DETAILS</strong></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td style="text-align: left; width: 245px; height: 15px;">';
	        shtml +=  '    <strong>';
	        shtml +=  'Net Amount Due to Beneficiary:</strong></td>';
	        shtml +=  '<td style="width: 58px; height: 15px">';
	        if (dt.Rows[0].NetAmountDueToBenefCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].NetAmountDueToBenefCurrCode + '</td>';
	        }
	        shtml +=  '<td style="height: 15px; text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].NetAmountDueToBenefAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Exchange Rate Used:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].ExchangeRateUsedCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].ExchangeRateUsedCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].ExchangeRateUsedAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Remittance Amount:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].RemittanceAmountCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].RemittanceAmountCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].RemittanceAmountAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Cable:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].CableCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].CableCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].CableAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Remittance Fee:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].RemittanceFeeCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].RemittanceFeeCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].RemittanceFeeAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Other Charges:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].OtherChargesCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].OtherChargesCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].OtherChargesAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="width: 221px">';
	        shtml +=  '<strong>';
	        shtml +=  'Total amount received from customer:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].TotalAmtRcvdFromCustCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].TotalAmtRcvdFromCustCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].TotalAmtRcvdFromCustAmt,2) + '</td>';
	        shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 70px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;font-size: 9pt">';
	        shtml +=  '' + dt.Rows[0].Check + '<br />';
	        shtml +=  '' + dt.Rows[0].Cash + '<br />';
	        shtml +=  '' + dt.Rows[0].SpecialMessage + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="font-size:9pt;width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 40px; text-align: center; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 177px;font-size:9pt">';
            shtml +=  '<strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Payment ';
            shtml +=  '<br />';
            shtml +=  'Received / Processed By:</strong></td>';
            shtml +=  '<td style="font-size:9pt;">';
            shtml +=  '' + dt.Rows[0].ProcessedBy + '<br />';
            shtml +=  '<hr style="width: 200px; height: 2px;background-color:black; text-align: left;" />';/////
            shtml +=  '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Authorized Personnel&nbsp;</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="border-top-width: thin; border-left-width: thin; border-left-color: black;';
            shtml +=  'border-bottom-width: thin; border-bottom-color: black; vertical-align: top; width: 100%;';
            shtml +=  'border-top-color: black; height: 20px; text-align: center; border-right-width: thin;';
            shtml +=  'border-right-color: black">';
            shtml +=  '<table style="width: 100%; height: 20px" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px;text-align:center;">';
            shtml +=  '<span style="font-size: 9.5pt; text-decoration: underline; line-height: 16pt"><strong>DECLARATIONS</strong></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            if (dt.Rows[0].ReceiptDeclarationSub5.toString().length == 0)
            {
                       shtml +=  '<td colspan="3" rowspan="2" style="font-size: 9pt;text-align: justify;line-height: 14pt">'; //line-height: 14pt;
            }else{
                        shtml +=  '<td colspan="3" rowspan="2" style="font-size: 9pt;text-align: justify; line-height: 14pt">';//line-height: 14pt;
            }
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub1 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub2 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub3 + '<br />';   
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub4 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub5 + '<br />';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            //END - FOR OTC 2nd copy
            
            shtml +=  '</div>';
         

        return shtml;
    }
    
    this.half_page = function(dt)
    {
          var shtml = "";
          shtml +=  '<br />';
//        shtml +=  '<br />';
//        shtml +=  '<br />';
//        shtml +=  '<br />';
//        shtml +=  '<br />';
//        shtml +=  '<table style="width: 8.5in">';
//        shtml +=  '<tr>';
//        shtml +=  '<td colspan="3" style="border-right: black thin dotted; border-top: black thin dotted; border-left: black thin dotted; border-bottom: black thin dotted">';
//        shtml +=  '<table style="height: 0%">';
//        shtml +=  '<tr>';
//        shtml +=  '<td style="font-size: 10pt; width: 371px; color: black; font-family: arial; height: 22px;';
//        shtml +=  'text-align: center">';
//        shtml +=  '<span style="font-size: 8pt; text-decoration: underline"><span style="font-size: 8.5pt;';
//        shtml +=  'text-decoration: underline"><strong>IMPORTANT REMINDERS TO CLIENT / CUSTOMER</strong></span></span></td>';
//        shtml +=  '<td rowspan="3" style="font-size: 6.5px; vertical-align: top; width: 50%; color: black;';
//        shtml +=  'font-family: arial; height: 101px; text-align: left">';
//        shtml +=  '<table style="font-size: 6.5pt; width: 100%; height: 23px">';
//        shtml +=  '<tr>';
//        shtml +=  '<td colspan="3" >';
//        shtml +=  'To the PNB __________________ , Branch: _____________ Date____________: This is';
//        shtml +=  'to authorize your office to change my (pls. mark check the box/es below as applicable),';
//        shtml +=  '</td>';
//        shtml +=  '</tr>';
//        shtml +=  '<tr>';
//        shtml +=  '<td colspan="3" nowrap>';
//        shtml +=  '<input id="Checkbox7" style="width: 26px; height: 12px" type="checkbox" />Remitter';
//        shtml +=  'Name to _______________________________________________________</td>';
//        shtml +=  '</tr>';
//        shtml +=  '<tr>';
//        shtml +=  '<td colspan="3" nowrap>';
//        shtml +=  '<input id="Checkbox8" style="width: 26px; height: 12px" type="checkbox" />Address';
//        shtml +=  '&nbsp;to _______________________________________ Postal Code: _________</td>';
//        shtml +=  '</tr>';
//        shtml +=  '<tr>';
//        shtml +=  '<td colspan="3" nowrap>';
//        shtml +=  '<input id="Checkbox9" style="width: 26px; height: 12px" type="checkbox" />Telephone';
//        shtml +=  'No to: __________________________ Mobile No to: _________________</td>';
//        shtml +=  '</tr>';
//        shtml +=  '<tr>';
//        shtml +=  '<td colspan="3" nowrap>';
//        shtml +=  '<input id="Checkbox10" style="width: 26px; height: 12px" type="checkbox" />Beneficiary';
//        shtml +=  '&nbsp;Name to: ________________________________Postal Code: _______</td>';
//        shtml +=  '</tr>';
//        shtml +=  '<tr>';
//        shtml +=  '<td colspan="3" nowrap>';
//        shtml +=  '<input id="Checkbox11" style="width: 26px; height: 12px" type="checkbox" />Beneficiary';
//        shtml +=  'Address To: _________________________________________________</td>';
//        shtml +=  '</tr>';
//        shtml +=  '<tr>';
//        shtml +=  '<td colspan="3" nowrap>';
//        shtml +=  '<input id="Checkbox12" style="width: 26px; height: 12px" type="checkbox" />Beneficiary';
//        shtml +=  'Telephone No to: ___________________Mobile No to: ______________</td>';
//        shtml +=  '</tr>';
//        shtml +=  '<tr>';
//        shtml +=  '<td colspan="3" nowrap>';
//        shtml +=  '<input id="Checkbox13" style="width: 26px; height: 12px" type="checkbox" />Others';
//        shtml +=  '(please specify): _________________________________________________</td>';
//        shtml +=  '</tr>';
//        shtml +=  '<tr>';
//        shtml +=  '<td colspan="3">';
//        shtml +=  'To support the request for ammendment, photocopy of the following document/s or';
//        shtml +=  'ID/s is/are provided (*should match the document/s or ID/s required in No.2 of important';
//        shtml +=  'Reminders to Client / Customer):';
//        shtml +=  '<br />';
//        shtml +=  'A) _______________&nbsp; B_________________ C)_________________ D)_______________</td>';
//        shtml +=  '</tr>';
//        shtml +=  '<tr>';
//        shtml +=  '<td colspan="3">';
//        shtml +=  '<table style="width: 100%">';
//        shtml +=  '<tr>';
//        shtml +=  '<td rowspan="3" style="font-size: 8pt;border-right: black thin solid; border-top: black thin solid;';
//        shtml +=  'border-left: black thin solid; border-bottom: black thin solid">';
//        shtml +=  'PNB Use Only:<br />';
//        shtml +=  'Processed By____Date:___</td>';
//        shtml +=  '<td colspan="2" rowspan="3" style="font-size: 8pt;text-align: center">';
//        shtml +=  '<hr style="width: 234px; height: 1px" />';
//        shtml +=  '&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; Remitter Signature Over Printed Name</td>';
//        shtml +=  '</tr>';
//        shtml +=  '<tr>';
//        shtml +=  '</tr>';
//        shtml +=  '<tr>';
//        shtml +=  '</tr>';
//        shtml +=  '</table>';
//        shtml +=  '</td>';
//        shtml +=  '</tr>';
//        shtml +=  '</table>';
//        shtml +=  '</td>';
//        shtml +=  '</tr>';
//        shtml +=  '<tr>';
//        shtml +=  '<td rowspan="2" style="font-size: 10px; vertical-align: top; width: 371px; color: black;';
//        shtml +=  'font-family: arial; height: 113px; text-align: left; line-height: 13pt;">';
//        shtml +=  '<span style="font-size: 8pt">1) Please notify the bank of any change in your name,';
//        shtml +=  'address, telephone number or other details pertaining to your remittance account';
//        shtml +=  '/ record by mailing / submitting the following: a) Photocopy of valid document/s';
//        shtml +=  'or ID/s to support the amendment.';
//        shtml +=  '<br />';
//        shtml +=  'b) Duly accomplished and signed " Authorization and Notice of Amendment" cutout';
//        shtml +=  'found on the opposite bottom right corner of this receipt. 2) Mailing address should';
//        shtml +=  'tally with the clients registered address shown in the valid document/s or ID/s,';
//        shtml +=  'as any irregularity in the registered address may result in non - implementation';
//        shtml +=  'of remittance. The valid document/s or ID/s are the following*:';
//        shtml +=  '<br />';
//        shtml +=  '</span>';
//        shtml +=  '<table style="font-size: 8pt; width: 303px; height: 45px">';
//        shtml +=  '<tr>';
//        shtml +=  '<td style="width: 50%; border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; border-bottom: black thin solid;">';
//        shtml +=  'A) ' + dt.Rows[0].ID1 + '</td>';
//        shtml +=  '<td style="width: 50%; border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; border-bottom: black thin solid;">';
//        shtml +=  'B) ' + dt.Rows[0].ID2 + '</td>';
//        shtml +=  '</tr>';
//        shtml +=  '<tr>';
//        shtml +=  '<td style="width: 50%; border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; border-bottom: black thin solid;">';
//        shtml +=  'C) ' + dt.Rows[0].ID3 + '</td>';
//        shtml +=  '<td style="width: 50%; border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; border-bottom: black thin solid;">';
//        shtml +=  'D) ' + dt.Rows[0].ID4 + '</td>';
//        shtml +=  '</tr>';
//        shtml +=  '</table>';
//        shtml +=  '<span style="font-size: 8pt">3) Always provide the bank with clear and proper instructions';
//        shtml +=  'as to beneficiary details and remittance amount to facilitate processing of remittance.';
//        shtml +=  '</span>';
//        shtml +=  '</td>';
//        shtml +=  '</tr>';
//        shtml +=  '<tr style="font-size: 10pt">';
//        shtml +=  '</tr>';
//        shtml +=  '</table>';
//        shtml +=  '</td>';
//        shtml +=  '</tr>';
//        shtml +=  '</table>';
          return shtml;
    }
    this.Receipt_body_halfpage = function(dt,isRCC)
    {
    var vd = window.location.href.split('/')[3];
    
    var shtml = "";
            shtml +=  '<br />';
            shtml +=  '<br />';
            shtml +=  '<div style="width: 970px; margin: 0px;">';
            shtml +=  '<table style="width: 100%; font-size: 9.5pt; font-family: arial;" cellspacing="0" cellpadding="0">';
            //shtml +=  '<table style="width: 8.5in; font-size: 7pt; font-family: arial;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="width: 0in">';
            shtml +=  '<table style="width: 100%;font-size: 9pt" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 60px; height: 15px">';
            shtml +=  '<img src="../../../'+ vd +'/images/pnb_black_logo.JPG" style="width: 58px; height: 68px" /></td>';
            shtml +=  '<td style="vertical-align: top; width: 520px; height: 15px">';
            shtml +=  '<strong>' + dt.Rows[0].Header1 + '</strong><br />';
            shtml +=  '' + dt.Rows[0].Header2 + '<br />';
            shtml +=  '<strong>' + dt.Rows[0].NameofBranch + '</strong><br />';
            shtml +=  '' + dt.Rows[0].AdressofBranch + '<br />';
            shtml +=  '' + dt.Rows[0].BranchofContact + '<br />';
            shtml +=  '' + dt.Rows[0].BranchofEmail + '<br />';
            shtml +=  '' + dt.Rows[0].corp_address + '<br />';
            shtml +=  '' + dt.Rows[0].corp_telno + '</td>';
            shtml +=  '<td style="vertical-align: top; height: 15px">';
            shtml +=  'Transaction Date: ' + HTMLRep.formatDate(dt.Rows[0].TranxDate,"/") + '<br />';
	        if (dt.Rows[0].ClearingDate == '1900')
	        {
	        shtml +=  'Clearing Date: <br />';
	        }else{
	        shtml +=  'Clearing Date: '+ dt.Rows[0].ClearingDate +'<br />';
	        }
            shtml +=  'Time: '+ HTMLRep.formatTime(dt.Rows[0].TranxDate) +'<br />';
            shtml +=  'Loyalty Points:';
            shtml +=  '<br />';
            shtml +=  '' + dt.Rows[0].TranStatus + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 100%; font-size: 9.5pt; font-family: arial;';
            shtml +=  'height: 0.23in; border-top-width: thin; border-left-width: thin; border-left-color: black; border-bottom-width: thin; border-bottom-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="border-right: black thin solid; border-top: black thin solid;';
            shtml +=  'border-left: black thin solid; border-bottom: black thin solid">';
            shtml +=  '<table style="font-size: 9.5pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 244px; height: 14px">';
            shtml +=  '<strong>Remitter ID No.: </strong>' + dt.Rows[0].RemitterID + '</td>';
            shtml +=  '<td style="width: 285px; height: 14px">';
            shtml +=  '<strong>Official Receipt No.:</strong> ' + dt.Rows[0].ReceiptNo + '</td>';
            shtml +=  '<td style="width: 257px; height: 14px">';
            shtml +=  '<strong>Reference No.:</strong> ' + dt.Rows[0].ReferenceNo + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';    
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td style="border-right: black thin solid; border-top: black thin solid; vertical-align: top;';
            shtml +=  'border-left: black thin solid; width: 50%; border-bottom: black thin solid; height: 20px">';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 0px; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="vertical-align: top; width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 20px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 9.5pt; text-decoration: underline">REMITTER INFO</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="2">';
            shtml +=  '<span><span style="font-size: 9pt"><strong>&nbsp;Name: </strong>' + dt.Rows[0].RemitterName + '<br />';
            shtml +=  '</span></span>';
            shtml +=  '<table style="font-size: 9pt; width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="width: 61px; vertical-align: top;">';
            shtml +=  '<strong style="width: 30px">Address:<br />';
            shtml +=  '</strong></td>';
            shtml +=  '<td colspan="2" rowspan="3" style="height: 40px; vertical-align: top;">';
            shtml +=  '' + dt.Rows[0].RemitterAddress + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<span><span style="font-size: 9pt; width: 100%"><strong>Telephone Number:</strong>';
            shtml +=  '' + dt.Rows[0].RemitterTelNo + '</span></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="vertical-align: top; width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 20px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%; height: 20px" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 100% ; height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 9.5pt; text-decoration: underline">BENEFICIARY INFO</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="2">';
            shtml +=  '<span><span style="font-size: 9pt"><strong>&nbsp;Name:</strong> ' + dt.Rows[0].BenefName + '<br />';
            shtml +=  '</span></span>';
            shtml +=  '<table style="font-size: 9pt; width: 100%; vertical-align: top;" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td  style="width: 60px; vertical-align: top;"><strong style="width: 30px">Address:<br />';
            shtml +=  '</strong>';
            shtml +=  '</td>';
            shtml +=  '<td rowspan="3" style="width: 90%; height: 40px; vertical-align: top;">';
            shtml +=  '' + dt.Rows[0].BenefAddress + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<span><span style="font-size: 9pt"><strong>Telephone Number:</strong> ' + dt.Rows[0].BenefTelNo + '</span></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="vertical-align: top;  width: 100%; height: 20px; border-bottom: black thin solid;">';
            shtml +=  '<table style="font-size: 9pt; width: 100%; height: 20px" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 9.5pt; text-decoration: underline">TRANSACTION DETAILS</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 9pt">';
            shtml +=  '<td colspan="3" rowspan="2" style="font-size: 8.5pt">';
            shtml +=  '<table style="font-size: 9pt; width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Beneficiarys Account No. : </strong>' + dt.Rows[0].BenefAcctNo + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Thru PNB Branch: </strong>' + dt.Rows[0].ThruPNBBranch + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Other Banks Name / Branch: </strong>' + dt.Rows[0].OtherBankName + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Trans Type/Products: </strong>' + dt.Rows[0].TransType + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Source of Funds: </strong>' + dt.Rows[0].SourceFunds + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Member ID:</strong>' + dt.Rows[0].MemberID + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Purpose of Remittance: </strong>' + dt.Rows[0].PurposeRemittance + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Remittance Transaction Source: </strong>' + dt.Rows[0].RemittanceTranxSource + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 9pt">';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 9pt">';
            shtml +=  '<td rowspan="1" style="border-top-width: thin; border-left-width: thin; border-left-color: black;';
            shtml +=  'border-bottom-width: thin; border-bottom-color: black; vertical-align: top; width: 100%;';
            shtml +=  'border-top-color: black; height: 20px; border-right-width: thin; border-right-color: black">';
            shtml +=  '<table style="width: 100%; height: 20px;font-size: 8pt;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px">';
            shtml +=  '<p style="text-align: center">';
            shtml +=  '<span style="font-size: 9.5pt; text-decoration: underline"><strong>CLIENT/CUSTOMER UNDERTAKING</strong></span></p>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="2" style="text-align: justify;line-height: 16pt">';// line-height: 14pt;
            shtml +=  '<table style="width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="3" style="text-align: justify;font-size: 9pt;line-height: 15pt">'; //line-height: 14pt;                      
            shtml +=  'I certify that all the information contained herein is true and correct, and that';
            shtml +=  'I am amenable to the Terms and Conditions (if any) governing this transaction.';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<br />';
            shtml +=  '<hr style="width: 378px; height: 1px; text-align: center"/>';
            shtml +=  '<div style="width: 100%; height: 25px; text-align: center">';
            shtml +=  'Remitter&#039;s Signature</div>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            // Edited by CHR from colspan=2 to colspan=1
            shtml +=  '<td style="border-right: black thin solid; border-top: black thin solid;';
            shtml +=  'vertical-align: top; border-left: black thin solid; width: 50%; border-bottom: black thin solid;';
            shtml +=  'height: 20px">';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 100%; height: 90px">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 86px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="font-size: 9pt; width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 18px; text-align: center">';
            shtml +=  '<span style="font-size: 9.5pt; text-decoration: underline"><strong>REMITTANCE DETAILS</strong></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td style="text-align: left; width: 245px; height: 15px;">';
	        shtml +=  '    <strong>';
	        shtml +=  'Net Amount Due to Beneficiary:</strong></td>';
	        shtml +=  '<td style="width: 58px; height: 15px">';
	        if (dt.Rows[0].NetAmountDueToBenefCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].NetAmountDueToBenefCurrCode + '</td>';
	        }
	        shtml +=  '<td style="height: 15px; text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].NetAmountDueToBenefAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Exchange Rate Used:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].ExchangeRateUsedCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].ExchangeRateUsedCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].ExchangeRateUsedAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Remittance Amount:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].RemittanceAmountCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].RemittanceAmountCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].RemittanceAmountAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Cable:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].CableCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].CableCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].CableAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Remittance Fee:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].RemittanceFeeCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].RemittanceFeeCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].RemittanceFeeAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Other Charges:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].OtherChargesCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].OtherChargesCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].OtherChargesAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="width: 221px">';
	        shtml +=  '<strong>';
	        shtml +=  'Total amount received from customer:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].TotalAmtRcvdFromCustCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].TotalAmtRcvdFromCustCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].TotalAmtRcvdFromCustAmt,2) + '</td>';
	        shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 70px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;font-size: 9pt">';
	        shtml +=  '' + dt.Rows[0].Check + '<br />';
	        shtml +=  '' + dt.Rows[0].Cash + '<br />';
	        shtml +=  '' + dt.Rows[0].SpecialMessage + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="font-size:9pt;width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 40px; text-align: center; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 177px;font-size:9pt">';
            shtml +=  '<strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Payment ';
            shtml +=  '<br />';
            shtml +=  'Received / Processed By:</strong></td>';
            shtml +=  '<td style="font-size:9pt;">';
            shtml +=  '' + dt.Rows[0].ProcessedBy + '<br />';
            shtml +=  '<hr style="width: 200px; height: 2px;background-color:black; text-align: left;" />';/////
            shtml +=  '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Authorized Personnel&nbsp;</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="border-top-width: thin; border-left-width: thin; border-left-color: black;';
            shtml +=  'border-bottom-width: thin; border-bottom-color: black; vertical-align: top; width: 100%;';
            shtml +=  'border-top-color: black; height: 20px; text-align: center; border-right-width: thin;';
            shtml +=  'border-right-color: black">';
            shtml +=  '<table style="width: 100%; height: 20px" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px;text-align:center;">';
            shtml +=  '<span style="font-size: 9.5pt; text-decoration: underline; line-height: 16pt"><strong>DECLARATIONS</strong></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            if (dt.Rows[0].ReceiptDeclarationSub5.toString().length == 0)
            {
                       shtml +=  '<td colspan="3" rowspan="2" style="font-size: 9pt;text-align: justify;line-height: 12pt">'; //line-height: 14pt;
            }else{
                        shtml +=  '<td colspan="3" rowspan="2" style="font-size: 9pt;text-align: justify; line-height: 12pt">';//line-height: 14pt;
            }
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub1 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub2 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub3 + '<br />';   
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub4 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub5 + '<br />';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            //shtml +=  '<tr>';
            //shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            
            //START - OTC RECEIPT 2nd copy 
            shtml +=  '<br />';
            shtml +=  '<br />';
            shtml +=  '<br />';
            shtml +=  '<br />';
            //shtml +=  '<br />';
            shtml +=  '<table style="width: 100%; font-size: 9.5pt; font-family: arial;" cellspacing="0" cellpadding="0">';
            //shtml +=  '<table style="width: 8.5in; font-size: 7pt; font-family: arial;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="width: 0in">';
            shtml +=  '<table style="width: 100%;font-size: 9pt" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 60px; height: 15px">';
            shtml +=  '<img src="../../../'+ vd +'/images/pnb_black_logo.JPG" style="width: 58px; height: 68px" /></td>';
            shtml +=  '<td style="vertical-align: top; width: 520px; height: 15px">';
            shtml +=  '<strong>' + dt.Rows[0].Header1 + '</strong><br />';
            shtml +=  '' + dt.Rows[0].Header2 + '<br />';
            shtml +=  '<strong>' + dt.Rows[0].NameofBranch + '</strong><br />';
            shtml +=  '' + dt.Rows[0].AdressofBranch + '<br />';
            shtml +=  '' + dt.Rows[0].BranchofContact + '<br />';
            shtml +=  '' + dt.Rows[0].BranchofEmail + '<br />';
            shtml +=  '' + dt.Rows[0].corp_address + '<br />';
            shtml +=  '' + dt.Rows[0].corp_telno + '</td>';
            shtml +=  '<td style="vertical-align: top; height: 15px">';
            shtml +=  'Transaction Date: ' + HTMLRep.formatDate(dt.Rows[0].TranxDate,"/") + '<br />';
	        if (dt.Rows[0].ClearingDate == '1900')
	        {
	        shtml +=  'Clearing Date: <br />';
	        }else{
	        shtml +=  'Clearing Date: '+ dt.Rows[0].ClearingDate +'<br />';
	        }
            shtml +=  'Time: '+ HTMLRep.formatTime(dt.Rows[0].TranxDate) +'<br />';
            shtml +=  'Loyalty Points:';
            shtml +=  '<br />';
            shtml +=  '' + dt.Rows[0].TranStatus + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 100%; font-size: 9.5pt; font-family: arial;';
            shtml +=  'height: 0.23in; border-top-width: thin; border-left-width: thin; border-left-color: black; border-bottom-width: thin; border-bottom-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="border-right: black thin solid; border-top: black thin solid;';
            shtml +=  'border-left: black thin solid; border-bottom: black thin solid">';
            shtml +=  '<table style="font-size: 9.5pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 244px; height: 14px">';
            shtml +=  '<strong>Remitter ID No.: </strong>' + dt.Rows[0].RemitterID + '</td>';
            shtml +=  '<td style="width: 285px; height: 14px">';
            shtml +=  '<strong>Official Receipt No.:</strong> ' + dt.Rows[0].ReceiptNo + '</td>';
            shtml +=  '<td style="width: 257px; height: 14px">';
            shtml +=  '<strong>Reference No.:</strong> ' + dt.Rows[0].ReferenceNo + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';    
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td style="border-right: black thin solid; border-top: black thin solid; vertical-align: top;';
            shtml +=  'border-left: black thin solid; width: 50%; border-bottom: black thin solid; height: 20px">';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 0px; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="vertical-align: top; width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 20px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 9.5pt; text-decoration: underline">REMITTER INFO</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="2">';
            shtml +=  '<span><span style="font-size: 9pt"><strong>&nbsp;Name: </strong>' + dt.Rows[0].RemitterName + '<br />';
            shtml +=  '</span></span>';
            shtml +=  '<table style="font-size: 9pt; width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="width: 61px; vertical-align: top;">';
            shtml +=  '<strong style="width: 30px">Address:<br />';
            shtml +=  '</strong></td>';
            shtml +=  '<td colspan="2" rowspan="3" style="height: 40px; vertical-align: top;">';
            shtml +=  '' + dt.Rows[0].RemitterAddress + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<span><span style="font-size: 9pt; width: 100%"><strong>Telephone Number:</strong>';
            shtml +=  '' + dt.Rows[0].RemitterTelNo + '</span></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="vertical-align: top; width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 20px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%; height: 20px" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 100% ; height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 9.5pt; text-decoration: underline">BENEFICIARY INFO</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="2">';
            shtml +=  '<span><span style="font-size: 9pt"><strong>&nbsp;Name:</strong> ' + dt.Rows[0].BenefName + '<br />';
            shtml +=  '</span></span>';
            shtml +=  '<table style="font-size: 9pt; width: 100%; vertical-align: top;" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td  style="width: 60px; vertical-align: top;"><strong style="width: 30px">Address:<br />';
            shtml +=  '</strong>';
            shtml +=  '</td>';
            shtml +=  '<td rowspan="3" style="width: 90%; height: 40px; vertical-align: top;">';
            shtml +=  '' + dt.Rows[0].BenefAddress + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<span><span style="font-size: 9pt"><strong>Telephone Number:</strong> ' + dt.Rows[0].BenefTelNo + '</span></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="vertical-align: top;  width: 100%; height: 20px; border-bottom: black thin solid;">';
            shtml +=  '<table style="font-size: 9pt; width: 100%; height: 20px" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 9.5pt; text-decoration: underline">TRANSACTION DETAILS</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 9pt">';
            shtml +=  '<td colspan="3" rowspan="2" style="font-size: 8.5pt">';
            shtml +=  '<table style="font-size: 9pt; width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Beneficiarys Account No. : </strong>' + dt.Rows[0].BenefAcctNo + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Thru PNB Branch: </strong>' + dt.Rows[0].ThruPNBBranch + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Other Banks Name / Branch: </strong>' + dt.Rows[0].OtherBankName + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Trans Type/Products: </strong>' + dt.Rows[0].TransType + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Source of Funds: </strong>' + dt.Rows[0].SourceFunds + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Member ID:</strong>' + dt.Rows[0].MemberID + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Purpose of Remittance: </strong>' + dt.Rows[0].PurposeRemittance + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Remittance Transaction Source: </strong>' + dt.Rows[0].RemittanceTranxSource + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 9pt">';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 9pt">';
            shtml +=  '<td rowspan="1" style="border-top-width: thin; border-left-width: thin; border-left-color: black;';
            shtml +=  'border-bottom-width: thin; border-bottom-color: black; vertical-align: top; width: 100%;';
            shtml +=  'border-top-color: black; height: 20px; border-right-width: thin; border-right-color: black">';
            shtml +=  '<table style="width: 100%; height: 20px;font-size: 8pt;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px">';
            shtml +=  '<p style="text-align: center">';
            shtml +=  '<span style="font-size: 9.5pt; text-decoration: underline"><strong>CLIENT/CUSTOMER UNDERTAKING</strong></span></p>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="2" style="text-align: justify;line-height: 16pt">';// line-height: 14pt;
            shtml +=  '<table style="width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="3" style="text-align: justify;font-size: 9pt;line-height: 15pt">'; //line-height: 14pt;                      
            shtml +=  'I certify that all the information contained herein is true and correct, and that';
            shtml +=  'I am amenable to the Terms and Conditions (if any) governing this transaction.';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<br />';
            shtml +=  '<hr style="width: 378px; height: 1px; text-align: center"/>';
            shtml +=  '<div style="width: 100%; height: 25px; text-align: center">';
            shtml +=  'Remitter&#039;s Signature</div>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            // Edited by CHR from colspan=2 to colspan=1
            shtml +=  '<td style="border-right: black thin solid; border-top: black thin solid;';
            shtml +=  'vertical-align: top; border-left: black thin solid; width: 50%; border-bottom: black thin solid;';
            shtml +=  'height: 20px">';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 100%; height: 90px">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 86px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="font-size: 9pt; width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 18px; text-align: center">';
            shtml +=  '<span style="font-size: 9.5pt; text-decoration: underline"><strong>REMITTANCE DETAILS</strong></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td style="text-align: left; width: 245px; height: 15px;">';
	        shtml +=  '    <strong>';
	        shtml +=  'Net Amount Due to Beneficiary:</strong></td>';
	        shtml +=  '<td style="width: 58px; height: 15px">';
	        if (dt.Rows[0].NetAmountDueToBenefCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].NetAmountDueToBenefCurrCode + '</td>';
	        }
	        shtml +=  '<td style="height: 15px; text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].NetAmountDueToBenefAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Exchange Rate Used:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].ExchangeRateUsedCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].ExchangeRateUsedCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].ExchangeRateUsedAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Remittance Amount:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].RemittanceAmountCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].RemittanceAmountCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].RemittanceAmountAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Cable:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].CableCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].CableCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].CableAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Remittance Fee:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].RemittanceFeeCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].RemittanceFeeCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].RemittanceFeeAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Other Charges:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].OtherChargesCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].OtherChargesCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].OtherChargesAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="width: 221px">';
	        shtml +=  '<strong>';
	        shtml +=  'Total amount received from customer:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].TotalAmtRcvdFromCustCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].TotalAmtRcvdFromCustCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].TotalAmtRcvdFromCustAmt,2) + '</td>';
	        shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 70px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;font-size: 9pt">';
	        shtml +=  '' + dt.Rows[0].Check + '<br />';
	        shtml +=  '' + dt.Rows[0].Cash + '<br />';
	        shtml +=  '' + dt.Rows[0].SpecialMessage + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="font-size:9pt;width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 40px; text-align: center; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 177px;font-size:9pt">';
            shtml +=  '<strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Payment ';
            shtml +=  '<br />';
            shtml +=  'Received / Processed By:</strong></td>';
            shtml +=  '<td style="font-size:9pt;">';
            shtml +=  '' + dt.Rows[0].ProcessedBy + '<br />';
            shtml +=  '<hr style="width: 200px; height: 2px;background-color:black; text-align: left;" />';/////
            shtml +=  '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Authorized Personnel&nbsp;</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="border-top-width: thin; border-left-width: thin; border-left-color: black;';
            shtml +=  'border-bottom-width: thin; border-bottom-color: black; vertical-align: top; width: 100%;';
            shtml +=  'border-top-color: black; height: 20px; text-align: center; border-right-width: thin;';
            shtml +=  'border-right-color: black">';
            shtml +=  '<table style="width: 100%; height: 20px" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px;text-align:center;">';
            shtml +=  '<span style="font-size: 9.5pt; text-decoration: underline; line-height: 16pt"><strong>DECLARATIONS</strong></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            if (dt.Rows[0].ReceiptDeclarationSub5.toString().length == 0)
            {
                       shtml +=  '<td colspan="3" rowspan="2" style="font-size: 9pt;text-align: justify;line-height: 12pt">'; //line-height: 14pt;
            }else{
                        shtml +=  '<td colspan="3" rowspan="2" style="font-size: 9pt;text-align: justify; line-height: 12pt">';//line-height: 14pt;
            }
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub1 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub2 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub3 + '<br />';   
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub4 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub5 + '<br />';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            //shtml +=  '<tr>';
            //shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            //END - OTC 2nd copy
            
            shtml +=  '</div>';

            return shtml;
    }
}
/*END OTC RECEIPT - ONE PAGE*/

//START HK OTC RECEIPT
/*START HONGKONG OTC RECEIPT*/
function HTMLReceiptOTC_HONGKONG()
{
    var cls = this;
    var _branchcode;    
    var _seqid;
	var _referenceno;
    
    this.evaluate_Receipt_query = function(query)
    {
        var squery = unescape(query.toString());
        if (squery.toLowerCase().indexOf("trantype=otc") > -1)
        {
            _branchcode = HTMLRep.get_branchcode();
            this.get_Receipt_data(
                                    HTMLRep.get_querystring_value(query,"tranvalue"),
                                    HTMLRep.get_branchcode(),
                                    ""
                                 )
            return;
        }
        if (squery.toLowerCase().indexOf("mergeparam") > -1)
        {
             _branchcode = HTMLRep.parse_merge_param_query(squery,"T@v_branchcode")
             this.get_Receipt_data(
                                    "",
                                    HTMLRep.parse_merge_param_query(squery,"T@v_branchcode"),
                                    HTMLRep.parse_merge_param_query(squery,"T@v_referenceno")
									)
            return;
        }
    }
    this.check_newdbswitch = function()
    {
    
	var pass = false;
	var sp_name = 'proc_check_newdbswitch';
        var squery = HTMLRep.get_hidden_querystring("hidQueryString");
        var sbranchcode = HTMLRep.get_branchcode();
        
        var params  =  '@branchcode=' + sbranchcode;
        var isRCC  = false;
        AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
            alert(errmsg);     return;        
        } 
        
        var dtcchecksw = IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params).value;
        
        try
        {
            if (dtcchecksw.Tables[0].Rows.length > 0)
            {
                pass = true;
            }
        }
        catch(e)
        {
        }
	
	return pass; 
    }
    this.get_Receipt_data = function(seq_id,branchcode,referenceno)
    {
        var params; 
        var sp_name = "";
	var pass = false;
        
                _branchcode =   " @v_branchcode="  + branchcode;    
                _seqid    =   "@v_seqid="     + seq_id;
				_referenceno = " @v_referenceno="     + referenceno;
        
       if(seq_id.length == 0)
	   {
			pass = this.check_newdbswitch();
			if (pass)
			{
			    sp_name = "SP_OTC_Reprint_Receipt_irsw";
			}
			else
			{
			    sp_name = "SP_OTC_Reprint_Receipt";
			}
			params  = _branchcode + "^" + _referenceno ;
	   }else{
			sp_name = "SP_OTC_Receipt";
			params  = _branchcode + "^" + _seqid ;
		}
		AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
            alert(errmsg);       return;      
        } 
	    IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params,this.get_Receipt_data_callback);
    }
    
    this.get_Receipt_data_callback = function(result)
    {
        try
        {
            var ds = result.value;
            
            cls.check_branch();
            cls.display_Receipt_report(ds);
            
            
        }
        catch(e)
        {
            alert("ERROR WHILE GETTING Receipt DATA");
        }
    }
    
    this.display_Receipt_report = function(ds)
    {
        var sp_name = 'proc_check_reporthalfpage';
        var squery = HTMLRep.get_hidden_querystring("hidQueryString");
        var sbranchcode = ds.Tables[0].Rows[0].ReferenceNo;
        sbranchcode =  sbranchcode.substring(1,4);
        var params  =  '@v_branhcode=' + sbranchcode;
        var ishalfpage  = false;
        AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
            alert(errmsg);     return;        
        } 
        
        var dtcheckhalfpage = IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params).value;
        
        try
        {
            if (dtcheckhalfpage.Tables[0].Rows.length > 0)
            {
                ishalfpage = true;
            }
        }
        catch(e)
        {
        }
    
        
        var shtml = "";        
        shtml +=  HTMLRep.print_html_button();
        var isRCC = cls.check_branch();
        if (!ishalfpage)
        {
           
            shtml +=  this.Receipt_body(ds.Tables[0],isRCC);
               
            shtml += cls.half_page(ds.Tables[0]);
            
	        shtml +=  '</div>';
        }
        else
        {
           shtml +=  this.Receipt_body_halfpage(ds.Tables[0],isRCC);
           
        }
        document.getElementById("bodydiv").innerHTML = shtml
    }
    this.check_branch = function()
    {
        var sp_name = 'sp_report_checkbranch';
        var squery = HTMLRep.get_hidden_querystring("hidQueryString");
        var sbranchcode = HTMLRep.get_branchcode();
        
        var params  =  '@branchcode=' + sbranchcode;
        var isRCC  = false;
        AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
            alert(errmsg);     return;        
        } 
        
        var dtccheckbranch = IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params).value;
        
        try
        {
            if (dtccheckbranch.Tables[0].Rows.length > 0)
            {
                isRCC = true;
            }
        }
        catch(e)
        {
        }
        return isRCC;
    }
    this.Receipt_body = function(dt,isRCC)
    {
	
	    var vd = window.location.href.split('/')[3];
	    
        var shtml = "";
	        shtml +=  '<div>';
            shtml +=  '<table style="width: 8.5in; font-size: 8pt; font-family: arial;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="width: 0in">';
            shtml +=  '<table style="width: 100%;font-size: 10pt;">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 60px; height: 15px">';
            shtml +=  '<img src="../../../'+ vd +'/images/pnb_black_logo.JPG" style="width: 58px; height: 68px" /></td>';
            shtml +=  '<td style="vertical-align: top; width: 520px; height: 15px">';
            shtml +=  '<strong>' + dt.Rows[0].Header1 + '</strong><br />';
            shtml +=  '' + dt.Rows[0].Header2 + '<br />';
            shtml +=  '<strong>' + dt.Rows[0].NameofBranch + '</strong><br />';
            shtml +=  '' + dt.Rows[0].AdressofBranch + '<br />';
            shtml +=  '' + dt.Rows[0].BranchofContact + '<br />';
            shtml +=  '' + dt.Rows[0].BranchofEmail + '<br />';
            shtml +=  '' + dt.Rows[0].corp_address + '<br />';
            shtml +=  '' + dt.Rows[0].corp_telno + '</td>';
            shtml +=  '<td style="vertical-align: top; height: 15px">';
            shtml +=  'Transaction Date: ' + HTMLRep.formatDate(dt.Rows[0].TranxDate,"/") + '<br />';
	        if (dt.Rows[0].ClearingDate == '1900')
	        {
	        shtml +=  'Clearing Date: <br />';
	        }else{
	        shtml +=  'Clearing Date: '+ dt.Rows[0].ClearingDate +'<br />';
	        }
            shtml +=  'Time: '+ HTMLRep.formatTime(dt.Rows[0].TranxDate) +'<br />';
            shtml +=  'Loyalty Points:';
            shtml +=  '<br />';
            shtml +=  '' + dt.Rows[0].TranStatus + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 8.5in; font-size: 10pt; font-family: arial;';
            shtml +=  'height: 0.23in; border-top-width: thin; border-left-width: thin; border-left-color: black; border-bottom-width: thin; border-bottom-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="border-right: black thin solid; border-top: black thin solid;';
            shtml +=  'border-left: black thin solid; border-bottom: black thin solid">';
            shtml +=  '<table style="font-size: 10pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 244px; height: 14px">';
            shtml +=  '<strong>Remitter ID No.: </strong>' + dt.Rows[0].RemitterID + '</td>';
            shtml +=  '<td style="width: 285px; height: 14px">';
            shtml +=  '<strong>Official Receipt No.:</strong> ' + dt.Rows[0].ReceiptNo + '</td>';
            shtml +=  '<td style="width: 257px; height: 14px">';
            shtml +=  '<strong>Reference No.:</strong> ' + dt.Rows[0].ReferenceNo + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';    
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td style="border-right: black thin solid; border-top: black thin solid; vertical-align: top;';
            shtml +=  'border-left: black thin solid; width: 50%; border-bottom: black thin solid; height: 20px">';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 0px; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="vertical-align: top; width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 20px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 10pt; text-decoration: underline">REMITTER INFO</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="2">';
            shtml +=  '<span><span style="font-size: 10pt"><strong>&nbsp;Name: </strong>' + dt.Rows[0].RemitterName + '<br />';
            shtml +=  '</span></span>';
            shtml +=  '<table style="font-size: 10pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="width: 61px; vertical-align: top;">';
            shtml +=  '<strong style="width: 30px">Address:<br />';
            shtml +=  '</strong></td>';
            shtml +=  '<td colspan="2" rowspan="3" style="height: 50px; vertical-align: top;">';
            shtml +=  '' + dt.Rows[0].RemitterAddress + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<span><span style="font-size: 10pt; width: 100%"><strong>Telephone Number:</strong>';
            shtml +=  '' + dt.Rows[0].RemitterTelNo + '</span></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="vertical-align: top; width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 20px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 100% ; height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 10pt; text-decoration: underline">BENEFICIARY INFO</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="2">';
            shtml +=  '<span><span style="font-size: 10pt"><strong>&nbsp;Name:</strong> ' + dt.Rows[0].BenefName + '<br />';
            shtml +=  '</span></span>';
            shtml +=  '<table style="font-size: 10pt; width: 100%; vertical-align: top;">';
            shtml +=  '<tr>';
            shtml +=  '<td  style="width: 60px; vertical-align: top;"><strong style="width: 30px">Address:<br />';
            shtml +=  '</strong>';
            shtml +=  '</td>';
            shtml +=  '<td rowspan="3" style="width: 90%; height: 50px; vertical-align: top;">';
            shtml +=  '' + dt.Rows[0].BenefAddress + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<span><span style="font-size: 10pt"><strong>Telephone Number:</strong> ' + dt.Rows[0].BenefTelNo + '</span></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="vertical-align: top;  width: 100%; height: 20px; border-bottom: black thin solid;">';
            shtml +=  '<table style="font-size: 10pt; width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 10pt; text-decoration: underline">TRANSACTION DETAILS</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 10pt">';
            shtml +=  '<td colspan="3" rowspan="2" style="font-size: 10pt">';
            shtml +=  '<table style="font-size: 10pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Beneficiarys Account No. : </strong>' + dt.Rows[0].BenefAcctNo + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Thru PNB Branch: </strong>' + dt.Rows[0].ThruPNBBranch + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Other Banks Name / Branch: </strong>' + dt.Rows[0].OtherBankName + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Trans Type/Products: </strong>' + dt.Rows[0].TransType + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Source of Funds: </strong>' + dt.Rows[0].SourceFunds + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Member ID:</strong>' + dt.Rows[0].MemberID + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Purpose of Remittance: </strong>' + dt.Rows[0].PurposeRemittance + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Remittance Transaction Source: </strong>' + dt.Rows[0].RemittanceTranxSource + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 10pt">';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 10pt">';
            shtml +=  '<td rowspan="1" style="border-top-width: thin; border-left-width: thin; border-left-color: black;';
            shtml +=  'border-bottom-width: thin; border-bottom-color: black; vertical-align: top; width: 100%;';
            shtml +=  'border-top-color: black; height: 20px; border-right-width: thin; border-right-color: black">';
            shtml +=  '<table style="width: 100%; height: 20px;font-size: 10pt;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px;font-size: 10pt;">';
            shtml +=  '<p style="text-align: center">';
            shtml +=  '<span style="font-size: 10pt; text-decoration: underline"><strong>CLIENT/CUSTOMER UNDERTAKING</strong></span></p>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="2" style="text-align: justify; line-height: 18pt">';
            shtml +=  '<table style="width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="3" style="text-align: justify;font-size: 10pt;line-height: 18pt">';
            shtml +=  '<br />';
            shtml +=  'I/We hereby agree that the PNB GLOBAL REMITTANCE AND FINANCIAL COMPANY (HK) ';
            shtml +=  'LIMITED reserves the right to, at any time, obtain further information/document ';
            shtml +=  'from me/us.';
            shtml +=  '<br/>';
            shtml +=  '<br/>';
            shtml +=  'I/We have read and understood the Notice to Customer relating to the Personal ';
            shtml +=  'Data (Privacy) Ordinance and all the terms and conditions printed overleaf.';
            shtml +=  '<br/>';
            shtml +=  '<br/>';
            shtml +=  'I also confirm that details of my beneficiary are correct.';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<br />';
            shtml +=  '<br />';
            shtml +=  '<hr style="width: 378px; height: 1px" />';
            shtml +=  '<div style="width: 100%; height: 25px; text-align: center">';
            shtml +=  'Remitter&#039;s Signature</div>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '<td colspan="2" style="border-right: black thin solid; border-top: black thin solid;';
            shtml +=  'vertical-align: top; border-left: black thin solid; width: 50%; border-bottom: black thin solid;';
            shtml +=  'height: 20px">';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 0px; height: 90px">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 86px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="font-size: 10pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 18px; text-align: center">';
            shtml +=  '<span style="font-size: 10pt; text-decoration: underline"><strong>REMITTANCE DETAILS</strong></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td style="text-align: left; width: 245px; height: 15px;">';
	        shtml +=  '    <strong>';
	        shtml +=  'Net Amount Due to Beneficiary:</strong></td>';
	        shtml +=  '<td style="width: 58px; height: 15px">';
	        if (dt.Rows[0].NetAmountDueToBenefCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].NetAmountDueToBenefCurrCode + '</td>';
	        }
	        shtml +=  '<td style="height: 15px; text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].NetAmountDueToBenefAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Exchange Rate Used:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].ExchangeRateUsedCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].ExchangeRateUsedCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].ExchangeRateUsedAmt,4) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Remittance Amount:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].RemittanceAmountCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].RemittanceAmountCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].RemittanceAmountAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Cable:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].CableCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].CableCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].CableAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Remittance Fee:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].RemittanceFeeCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].RemittanceFeeCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].RemittanceFeeAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Other Charges:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].OtherChargesCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].OtherChargesCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].OtherChargesAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="width: 221px">';
	        shtml +=  '<strong>';
	        shtml +=  'Total amount received from customer:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].TotalAmtRcvdFromCustCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].TotalAmtRcvdFromCustCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].TotalAmtRcvdFromCustAmt,2) + '</td>';
	        shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 70px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;font-size:10pt;">';
	        shtml +=  '' + dt.Rows[0].Check + '<br />';
	        shtml +=  '' + dt.Rows[0].Cash + '<br />';
	        shtml +=  '' + dt.Rows[0].SpecialMessage + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="font-size:10pt;width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 40px; text-align: center; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 177px;font-size:10pt">';
            shtml +=  '<strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Payment ';
            shtml +=  '<br />';
            shtml +=  'Received / Processed By:</strong></td>';
            shtml +=  '<td style="font-size:10pt;height:30px;">';
            shtml +=  '' + dt.Rows[0].ProcessedBy + '<br />';
            shtml +=  '<hr style="width: 200px; height: 2px;background-color:black;" />';
            shtml +=  '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Authorized Personnel&nbsp;</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="border-top-width: thin; border-left-width: thin; border-left-color: black;';
            shtml +=  'border-bottom-width: thin; border-bottom-color: black; vertical-align: top; width: 100%;';
            shtml +=  'border-top-color: black; height: 20px; text-align: center; border-right-width: thin;';
            shtml +=  'border-right-color: black">';
            shtml +=  '<table style="width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px" align="center">';
            shtml +=  '<span style="font-size: 10pt; text-decoration: underline"><strong>DECLARATIONS</strong></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            if (dt.Rows[0].ReceiptDeclarationSub5.toString().length == 0)
            {
            shtml +=  '<td colspan="3" rowspan="2" style="font-size: 8pt;text-align: justify; line-height: 18pt;">';
            }else{
            shtml +=  '<td colspan="3" rowspan="2" style="font-size: 7pt;text-align: justify; line-height: 18pt;">';
            }
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub1 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub2 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub3 + '<br />';   
            //shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub4 + '<br />';
            //shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub5 + '<br />';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
         

        return shtml;
    }
    
    this.half_page = function(dt)
    {
        var shtml = "";
        shtml +=  '<table style="width: 8.5in">';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" style="border-right: black thin dotted; border-top: black thin dotted; border-left: black thin dotted; border-bottom: black thin dotted">';
        shtml +=  '<table style="height: 0%">';
        shtml +=  '<tr>';
        shtml +=  '<td style="font-size: 10pt; width: 371px; color: black; font-family: arial; height: 22px;';
        shtml +=  'text-align: center">';
        shtml +=  '<span style="font-size: 8pt; text-decoration: underline"><span style="font-size: 8.5pt;';
        shtml +=  'text-decoration: underline"><strong>IMPORTANT REMINDERS TO CLIENT / CUSTOMER</strong></span></span></td>';
        shtml +=  '<td rowspan="3" style="font-size: 6.5px; vertical-align: top; width: 50%; color: black;';
        shtml +=  'font-family: arial; height: 101px; text-align: left">';
        shtml +=  '<table style="font-size: 6.5pt; width: 100%; height: 23px">';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" >';
        shtml +=  'To the PNB __________________ , Branch: _____________ Date____________: This is';
        shtml +=  'to authorize your office to change my (pls. mark check the box/es below as applicable),';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" nowrap>';
        shtml +=  '<input id="Checkbox7" style="width: 26px; height: 12px" type="checkbox" />Remitter';
        shtml +=  'Name to _______________________________________________________</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" nowrap>';
        shtml +=  '<input id="Checkbox8" style="width: 26px; height: 12px" type="checkbox" />Address';
        shtml +=  '&nbsp;to _______________________________________ Postal Code: _________</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" nowrap>';
        shtml +=  '<input id="Checkbox9" style="width: 26px; height: 12px" type="checkbox" />Telephone';
        shtml +=  'No to: __________________________ Mobile No to: _________________</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" nowrap>';
        shtml +=  '<input id="Checkbox10" style="width: 26px; height: 12px" type="checkbox" />Beneficiary';
        shtml +=  '&nbsp;Name to: ________________________________Postal Code: _______</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" nowrap>';
        shtml +=  '<input id="Checkbox11" style="width: 26px; height: 12px" type="checkbox" />Beneficiary';
        shtml +=  'Address To: _________________________________________________</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" nowrap>';
        shtml +=  '<input id="Checkbox12" style="width: 26px; height: 12px" type="checkbox" />Beneficiary';
        shtml +=  'Telephone No to: ___________________Mobile No to: ______________</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" nowrap>';
        shtml +=  '<input id="Checkbox13" style="width: 26px; height: 12px" type="checkbox" />Others';
        shtml +=  '(please specify): _________________________________________________</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3">';
        shtml +=  'To support the request for ammendment, photocopy of the following document/s or';
        shtml +=  'ID/s is/are provided (*should match the document/s or ID/s required in No.2 of important';
        shtml +=  'Reminders to Client / Customer):';
        shtml +=  '<br />';
        shtml +=  'A) _______________&nbsp; B_________________ C)_________________ D)_______________</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3">';
        shtml +=  '<table style="width: 100%">';
        shtml +=  '<tr>';
        shtml +=  '<td rowspan="3" style="font-size: 8pt;border-right: black thin solid; border-top: black thin solid;';
        shtml +=  'border-left: black thin solid; border-bottom: black thin solid">';
        shtml +=  'PNB Use Only:<br />';
        shtml +=  'Processed By____Date:___</td>';
        shtml +=  '<td colspan="2" rowspan="3" style="font-size: 8pt;text-align: center">';
        shtml +=  '<hr style="width: 234px; height: 1px" />';
        shtml +=  '&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; Remitter Signature Over Printed Name</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td rowspan="2" style="font-size: 10px; vertical-align: top; width: 371px; color: black;';
        shtml +=  'font-family: arial; height: 113px; text-align: left; line-height: 13pt;">';
        shtml +=  '<span style="font-size: 8pt">1) Please notify the bank of any change in your name,';
        shtml +=  'address, telephone number or other details pertaining to your remittance account';
        shtml +=  '/ record by mailing / submitting the following: a) Photocopy of valid document/s';
        shtml +=  'or ID/s to support the amendment.';
        shtml +=  '<br />';
        shtml +=  'b) Duly accomplished and signed " Authorization and Notice of Amendment" cutout';
        shtml +=  'found on the opposite bottom right corner of this receipt. 2) Mailing address should';
        shtml +=  'tally with the clients registered address shown in the valid document/s or ID/s,';
        shtml +=  'as any irregularity in the registered address may result in non - implementation';
        shtml +=  'of remittance. The valid document/s or ID/s are the following*:';
        shtml +=  '<br />';
        shtml +=  '</span>';
        shtml +=  '<table style="font-size: 8pt; width: 303px; height: 45px">';
        shtml +=  '<tr>';
        shtml +=  '<td style="width: 50%; border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; border-bottom: black thin solid;">';
        shtml +=  'A) ' + dt.Rows[0].ID1 + '</td>';
        shtml +=  '<td style="width: 50%; border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; border-bottom: black thin solid;">';
        shtml +=  'B) ' + dt.Rows[0].ID2 + '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td style="width: 50%; border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; border-bottom: black thin solid;">';
        shtml +=  'C) ' + dt.Rows[0].ID3 + '</td>';
        shtml +=  '<td style="width: 50%; border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; border-bottom: black thin solid;">';
        shtml +=  'D) ' + dt.Rows[0].ID4 + '</td>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '<span style="font-size: 8pt">3) Always provide the bank with clear and proper instructions';
        shtml +=  'as to beneficiary details and remittance amount to facilitate processing of remittance.';
        shtml +=  '</span>';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr style="font-size: 10pt">';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        return shtml;
    }
    this.Receipt_body_halfpage = function(dt,isRCC)
    {
    var vd = window.location.href.split('/')[3];
    
    var shtml = "";
            shtml +=  '<div>';
            shtml +=  '<table style="width: 8.5in; font-size: 8.5pt; font-family: arial;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="width: 0in">';
            shtml +=  '<table style="width: 100%;font-size: 8.5pt">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 60px; height: 15px">';
            shtml +=  '<img src="../../../'+ vd +'/images/pnb_black_logo.JPG" style="width: 58px; height: 68px" /></td>';
            shtml +=  '<td style="vertical-align: top; width: 520px; height: 15px">';
            shtml +=  '<strong>' + dt.Rows[0].Header1 + '</strong><br />';
            shtml +=  '' + dt.Rows[0].Header2 + '<br />';
            shtml +=  '<strong>' + dt.Rows[0].NameofBranch + '</strong><br />';
            shtml +=  '' + dt.Rows[0].AdressofBranch + '<br />';
            shtml +=  '' + dt.Rows[0].BranchofContact + '<br />';
            shtml +=  '' + dt.Rows[0].BranchofEmail + '<br />';
            shtml +=  '' + dt.Rows[0].corp_address + '<br />';
            shtml +=  '' + dt.Rows[0].corp_telno + '</td>';
            shtml +=  '<td style="vertical-align: top; height: 15px">';
            shtml +=  'Transaction Date: ' + HTMLRep.formatDate(dt.Rows[0].TranxDate,"/") + '<br />';
	        if (dt.Rows[0].ClearingDate == '1900')
	        {
	        shtml +=  'Clearing Date: <br />';
	        }else{
	        shtml +=  'Clearing Date: '+ dt.Rows[0].ClearingDate +'<br />';
	        }
            shtml +=  'Time: '+ HTMLRep.formatTime(dt.Rows[0].TranxDate) +'<br />';
            shtml +=  'Loyalty Points:';
            shtml +=  '<br />';
            shtml +=  '' + dt.Rows[0].TranStatus + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 8.5in; font-size: 8pt; font-family: arial;';
            shtml +=  'height: 0.23in; border-top-width: thin; border-left-width: thin; border-left-color: black; border-bottom-width: thin; border-bottom-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="border-right: black thin solid; border-top: black thin solid;';
            shtml +=  'border-left: black thin solid; border-bottom: black thin solid">';
            shtml +=  '<table style="font-size: 8.5pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 244px; height: 14px">';
            shtml +=  '<strong>Remitter ID No.: </strong>' + dt.Rows[0].RemitterID + '</td>';
            shtml +=  '<td style="width: 285px; height: 14px">';
            shtml +=  '<strong>Official Receipt No.:</strong> ' + dt.Rows[0].ReceiptNo + '</td>';
            shtml +=  '<td style="width: 257px; height: 14px">';
            shtml +=  '<strong>Reference No.:</strong> ' + dt.Rows[0].ReferenceNo + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';    
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td style="border-right: black thin solid; border-top: black thin solid; vertical-align: top;';
            shtml +=  'border-left: black thin solid; width: 50%; border-bottom: black thin solid; height: 20px">';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 0px; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="vertical-align: top; width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 20px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 8.5pt; text-decoration: underline">REMITTER INFO</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="2">';
            shtml +=  '<span><span style="font-size: 8.5pt"><strong>&nbsp;Name: </strong>' + dt.Rows[0].RemitterName + '<br />';
            shtml +=  '</span></span>';
            shtml +=  '<table style="font-size: 8.5pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="width: 61px; vertical-align: top;">';
            shtml +=  '<strong style="width: 30px">Address:<br />';
            shtml +=  '</strong></td>';
            shtml +=  '<td colspan="2" rowspan="3" style="height: 50px; vertical-align: top;">';
            shtml +=  '' + dt.Rows[0].RemitterAddress + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<span><span style="font-size: 8.5pt; width: 100%"><strong>Telephone Number:</strong>';
            shtml +=  '' + dt.Rows[0].RemitterTelNo + '</span></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="vertical-align: top; width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 20px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 100% ; height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 10pt; text-decoration: underline">BENEFICIARY INFO</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="2">';
            shtml +=  '<span><span style="font-size: 10pt"><strong>&nbsp;Name:</strong> ' + dt.Rows[0].BenefName + '<br />';
            shtml +=  '</span></span>';
            shtml +=  '<table style="font-size: 10pt; width: 100%; vertical-align: top;">';
            shtml +=  '<tr>';
            shtml +=  '<td  style="width: 60px; vertical-align: top;"><strong style="width: 30px">Address:<br />';
            shtml +=  '</strong>';
            shtml +=  '</td>';
            shtml +=  '<td rowspan="3" style="width: 90%; height: 50px; vertical-align: top;">';
            shtml +=  '' + dt.Rows[0].BenefAddress + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<span><span style="font-size: 8.5pt"><strong>Telephone Number:</strong> ' + dt.Rows[0].BenefTelNo + '</span></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="vertical-align: top;  width: 100%; height: 20px; border-bottom: black thin solid;">';
            shtml +=  '<table style="font-size: 8.5pt; width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 8.5pt; text-decoration: underline">TRANSACTION DETAILS</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 8.5pt">';
            shtml +=  '<td colspan="3" rowspan="2" style="font-size: 8.5pt">';
            shtml +=  '<table style="font-size: 8.5pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Beneficiarys Account No. : </strong>' + dt.Rows[0].BenefAcctNo + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Thru PNB Branch: </strong>' + dt.Rows[0].ThruPNBBranch + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Other Banks Name / Branch: </strong>' + dt.Rows[0].OtherBankName + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Trans Type/Products: </strong>' + dt.Rows[0].TransType + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Source of Funds: </strong>' + dt.Rows[0].SourceFunds + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Member ID:</strong>' + dt.Rows[0].MemberID + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Purpose of Remittance: </strong>' + dt.Rows[0].PurposeRemittance + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Remittance Transaction Source: </strong>' + dt.Rows[0].RemittanceTranxSource + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 8.5pt">';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 8.5pt">';
            shtml +=  '<td rowspan="1" style="border-top-width: thin; border-left-width: thin; border-left-color: black;';
            shtml +=  'border-bottom-width: thin; border-bottom-color: black; vertical-align: top; width: 100%;';
            shtml +=  'border-top-color: black; height: 20px; border-right-width: thin; border-right-color: black">';
            shtml +=  '<table style="width: 100%; height: 20px;font-size: 8.5pt;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px">';
            shtml +=  '<p style="text-align: center">';
            shtml +=  '<span style="font-size: 8.5pt; text-decoration: underline"><strong>CLIENT/CUSTOMER UNDERTAKING</strong></span></p>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="2" style="text-align: justify; line-height: 14pt;">';
            shtml +=  '<table style="width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="3" style="text-align: justify;font-size: 8.5pt;line-height: 14pt;">';                       
            shtml +=  'I/We hereby agree that the PNB GLOBAL REMITTANCE AND FINANCIAL COMPANY (HK) ';
            shtml +=  'LIMITED reserves the right to, at any time, obtain further information/document ';
            shtml +=  'from me/us.';
            shtml +=  '<br/>';
            shtml +=  '<br/>';
            shtml +=  'I/We have read and understood the Notice to Customer relating to the Personal ';
            shtml +=  'Data (Privacy) Ordinance and all the terms and conditions printed overleaf.';
            shtml +=  '<br/>';
            shtml +=  '<br/>';
            shtml +=  'I also confirm that details of my beneficiary are correct.';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<br />';
            shtml +=  '<hr style="width: 378px; height: 1px" />';
            shtml +=  '<div style="width: 100%; height: 25px; text-align: center">';
            shtml +=  'Remitter&#039;s Signature</div>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            // Edited by CHR from colspan=2 to colspan=1
            shtml +=  '<td style="border-right: black thin solid; border-top: black thin solid;';
            shtml +=  'vertical-align: top; border-left: black thin solid; width: 50%; border-bottom: black thin solid;';
            shtml +=  'height: 20px">';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 100%; height: 90px">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 86px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="font-size: 8.5pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 18px; text-align: center">';
            shtml +=  '<span style="font-size: 8.5pt; text-decoration: underline"><strong>REMITTANCE DETAILS</strong></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td style="text-align: left; width: 245px; height: 15px;">';
	        shtml +=  '    <strong>';
	        shtml +=  'Net Amount Due to Beneficiary:</strong></td>';
	        shtml +=  '<td style="width: 58px; height: 15px">';
	        if (dt.Rows[0].NetAmountDueToBenefCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].NetAmountDueToBenefCurrCode + '</td>';
	        }
	        shtml +=  '<td style="height: 15px; text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].NetAmountDueToBenefAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Exchange Rate Used:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].ExchangeRateUsedCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].ExchangeRateUsedCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].ExchangeRateUsedAmt,4) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Remittance Amount:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].RemittanceAmountCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].RemittanceAmountCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].RemittanceAmountAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Cable:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].CableCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].CableCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].CableAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Remittance Fee:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].RemittanceFeeCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].RemittanceFeeCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].RemittanceFeeAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Other Charges:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].OtherChargesCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].OtherChargesCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].OtherChargesAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="width: 221px">';
	        shtml +=  '<strong>';
	        shtml +=  'Total amount received from customer:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].TotalAmtRcvdFromCustCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].TotalAmtRcvdFromCustCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].TotalAmtRcvdFromCustAmt,2) + '</td>';
	        shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 70px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;font-size: 8pt">';
	        shtml +=  '' + dt.Rows[0].Check + '<br />';
	        shtml +=  '' + dt.Rows[0].Cash + '<br />';
	        shtml +=  '' + dt.Rows[0].SpecialMessage + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="font-size:8.5pt;width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 40px; text-align: center; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 177px;font-size:8.5pt">';
            shtml +=  '<strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Payment ';
            shtml +=  '<br />';
            shtml +=  'Received / Processed By:</strong></td>';
            shtml +=  '<td style="font-size:8.5pt;">';
            shtml +=  '' + dt.Rows[0].ProcessedBy + '<br />';
            shtml +=  '<hr style="width: 200px; height: 2px;background-color:black;" />';
            shtml +=  '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Authorized Personnel&nbsp;</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="border-top-width: thin; border-left-width: thin; border-left-color: black;';
            shtml +=  'border-bottom-width: thin; border-bottom-color: black; vertical-align: top; width: 100%;';
            shtml +=  'border-top-color: black; height: 20px; text-align: center; border-right-width: thin;';
            shtml +=  'border-right-color: black">';
            shtml +=  '<table style="width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px;text-align:center;">';
            shtml +=  '<span style="font-size: 8.5pt; text-decoration: underline"><strong>DECLARATIONS</strong></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            if (dt.Rows[0].ReceiptDeclarationSub5.toString().length == 0)
            {
                       shtml +=  '<td colspan="3" rowspan="2" style="font-size: 8pt;text-align: justify; line-height: 14pt;">';
            }else{
                        shtml +=  '<td colspan="3" rowspan="2" style="font-size: 7pt;text-align: justify; line-height: 14pt;">';
            }
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub1 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub2 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub3 + '<br />';   
           // shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub4 + '<br />';
            //shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub5 + '<br />';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</div>';

            return shtml;
    }
}
/*END HONGKONG OTC RECEIPT*/          
//END HK OTC RECEIPT
/*START HONGKONG OBP RECEIPT*/
function HTMLOBPReceipt_HONGKONG()
{
    var cls = this;
    var _branchcode;    
    var _seqid;
	var _referenceno;
    
    this.evaluate_Receipt_query = function(query)
    {
        var squery = unescape(query.toString());
        if (squery.toLowerCase().indexOf("trantype=obp") > -1)
        {
            _branchcode = HTMLRep.get_branchcode();
            this.get_Receipt_data(
                                    HTMLRep.get_querystring_value(query,"tranvalue"),
                                    HTMLRep.get_branchcode(),
                                    ""
                                 )
            return;
        }
        if (squery.toLowerCase().indexOf("mergeparam") > -1)
        {
             _branchcode = HTMLRep.parse_merge_param_query(squery,"T@v_branchcode")
             this.get_Receipt_data(
                                    "",
                                    HTMLRep.parse_merge_param_query(squery,"T@v_branchcode"),
                                    HTMLRep.parse_merge_param_query(squery,"T@v_referenceno")
									)
            return;
        }
    }
    this.get_Receipt_data = function(seq_id,branchcode,referenceno)
    {
        var params; 
        var sp_name = "";
	var pass  =	false;
        
                _branchcode =   " @v_branchcode="  + branchcode;    
                _seqid    =   "@seqid="     + seq_id;
				_referenceno = " @v_referenceno="     + referenceno;
        
	pass = this.check_newdbswitch();
	
       if(seq_id.length == 0)
	   {
			if (pass)
			{
			    sp_name = "sp_otc_obp_Receipt_reprint_irsw";
			}
			else
			{
			    sp_name = "sp_otc_obp_Receipt_reprint";
			}
			params  = _branchcode + "^" + _referenceno ;
	   }else{
			sp_name = "SP_OTC_OBP_Receipt";
			params  = _branchcode + "^" + _seqid ;
		}
		AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
            alert(errmsg);       return;      
        } 
	    IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params,this.get_Receipt_data_callback);
    }
    
    this.get_Receipt_data_callback = function(result)
    {
        try
        {
            var ds = result.value;
            
            cls.check_branch();
            cls.display_Receipt_report(ds);
            
            
        }
        catch(e)
        {
            alert("ERROR WHILE GETTING Receipt DATA");
        }
    }
    this.check_newdbswitch = function()
    {
    
	var pass = false;
	var sp_name = 'proc_check_newdbswitch';
        var squery = HTMLRep.get_hidden_querystring("hidQueryString");
        var sbranchcode = HTMLRep.get_branchcode();
        
        var params  =  '@branchcode=' + sbranchcode;
        var isRCC  = false;
        AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
            alert(errmsg);     return;        
        } 
        
        var dtcchecksw = IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params).value;
        
        try
        {
            if (dtcchecksw.Tables[0].Rows.length > 0)
            {
                pass = true;
            }
        }
        catch(e)
        {
        }
	
	return pass; 
    }
    
    this.display_Receipt_report = function(ds)
    {
        var sp_name = 'proc_check_reporthalfpage';
        var squery = HTMLRep.get_hidden_querystring("hidQueryString");
        var sbranchcode = ds.Tables[0].Rows[0].ReferenceNo;
        sbranchcode =  sbranchcode.substring(1,4);
        var params  =  '@v_branhcode=' + sbranchcode;
        var ishalfpage  = false;
        AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
            alert(errmsg);     return;        
        } 
        
        var dtcheckhalfpage = IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params).value;
        
        try
        {
            if (dtcheckhalfpage.Tables[0].Rows.length > 0)
            {
                ishalfpage = true;
            }
        }
        catch(e)
        {
        }
    
        
        var shtml = "";        
        shtml +=  HTMLRep.print_html_button();
        var isRCC = cls.check_branch();
	
	isRCC = (cls.check_ars()) ? 0 : isRCC ;
	
        if (!ishalfpage)
        {
           
            shtml +=  this.Receipt_body(ds.Tables[0],isRCC);
               
            shtml += cls.half_page(ds.Tables[0]);
            
	        shtml +=  '</div>';
        }
        else
        {
           shtml +=  this.Receipt_body_halfpage(ds.Tables[0],isRCC);
           
        }
        document.getElementById("bodydiv").innerHTML = shtml
    }
    this.check_branch = function()
    {
        var sp_name = 'sp_report_checkbranch';
        var squery = HTMLRep.get_hidden_querystring("hidQueryString");
        var sbranchcode = HTMLRep.get_branchcode();
        
        var params  =  '@branchcode=' + sbranchcode;
        var isRCC  = false;
        AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
            alert(errmsg);     return;        
        } 
        
        var dtccheckbranch = IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params).value;
        
        try
        {
            if (dtccheckbranch.Tables[0].Rows.length > 0)
            {
                isRCC = true;
            }
        }
        catch(e)
        {
        }
        return isRCC;
    }
    
    this.check_ars = function()
    {
        var sp_name = 'proc_check_ars';
        
        var sbranchcode = HTMLRep.get_branchcode();
        
        var params  =  '@v_branchcode=' + sbranchcode;
        var isARS  = false;
        AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
            alert(errmsg);     return;        
        } 
        
        var dtccheckbranch = IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params).value;
        
        try
        {
            if (dtccheckbranch.Tables[0].Rows.length > 0)
            {
                isARS = true;
            }
        }
        catch(e)
        {
        }
        return isARS;
    }
    this.Receipt_body = function(dt,isRCC)
    {
	
	    var vd = window.location.href.split('/')[3];
	    
        var shtml = "";
	        shtml +=  '<div>';
            shtml +=  '<table style="width: 8.5in; font-size: 8pt; font-family: arial;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="width: 0in">';
            shtml +=  '<table style="width: 100%;font-size: 10pt;">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 60px; height: 15px">';
            shtml +=  '<img src="../../../'+ vd +'/images/pnb_black_logo.JPG" style="width: 58px; height: 68px" /></td>';
            shtml +=  '<td style="vertical-align: top; width: 520px; height: 15px">';
            shtml +=  '<strong>' + dt.Rows[0].Header1 + '</strong><br />';
            shtml +=  '' + dt.Rows[0].Header2 + '<br />';
            shtml +=  '<strong>' + dt.Rows[0].NameofBranch + '</strong><br />';
            shtml +=  '' + dt.Rows[0].AdressofBranch + '<br />';
            shtml +=  '' + dt.Rows[0].BranchofContact + '<br />';
            shtml +=  '' + dt.Rows[0].BranchofEmail + '<br />';
            shtml +=  '' + dt.Rows[0].corp_address + '<br />';
            shtml +=  '' + dt.Rows[0].corp_telno + '</td>';
            shtml +=  '<td style="vertical-align: top; height: 15px">';
            shtml +=  'Transaction Date: ' + HTMLRep.formatDate(dt.Rows[0].TranxDate,"/") + '<br />';
	        if (dt.Rows[0].ClearingDate == '1900')
	        {
	        shtml +=  'Clearing Date: <br />';
	        }else{
	        shtml +=  'Clearing Date: '+ dt.Rows[0].ClearingDate +'<br />';
	        }
            shtml +=  'Time: '+ HTMLRep.formatTime(dt.Rows[0].TranxDate) +'<br />';
            shtml +=  'Loyalty Points:';
            shtml +=  '<br />';
            shtml +=  '' + dt.Rows[0].TranStatus + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 8.5in; font-size: 10pt; font-family: arial;';
            shtml +=  'height: 0.23in; border-top-width: thin; border-left-width: thin; border-left-color: black; border-bottom-width: thin; border-bottom-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="border-right: black thin solid; border-top: black thin solid;';
            shtml +=  'border-left: black thin solid; border-bottom: black thin solid">';
            shtml +=  '<table style="font-size: 10pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 244px; height: 14px">';
            shtml +=  '<strong>Remitter ID No.: </strong>' + dt.Rows[0].RemitterID + '</td>';
            shtml +=  '<td style="width: 285px; height: 14px">';
            shtml +=  '<strong>Official Receipt No.:</strong> ' + dt.Rows[0].ReceiptNo + '</td>';
            shtml +=  '<td style="width: 257px; height: 14px">';
            shtml +=  '<strong>Reference No.:</strong> ' + dt.Rows[0].ReferenceNo + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';    
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td style="border-right: black thin solid; border-top: black thin solid; vertical-align: top;';
            shtml +=  'border-left: black thin solid; width: 50%; border-bottom: black thin solid; height: 20px">';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 0px; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="vertical-align: top; width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 20px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 10pt; text-decoration: underline">REMITTER INFO</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="2">';
            shtml +=  '<span><span style="font-size: 10pt"><strong>&nbsp;Name: </strong>' + dt.Rows[0].RemitterName + '<br />';
            shtml +=  '</span></span>';
            shtml +=  '<table style="font-size: 10pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="width: 61px; vertical-align: top;">';
            shtml +=  '<strong style="width: 30px">Address:<br />';
            shtml +=  '</strong></td>';
            shtml +=  '<td colspan="2" rowspan="3" style="height: 50px; vertical-align: top;">';
            shtml +=  '' + dt.Rows[0].RemitterAddress + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<span><span style="font-size: 10pt; width: 100%"><strong>Telephone Number:</strong>';
            shtml +=  '' + dt.Rows[0].RemitterTelNo + '</span></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="vertical-align: top; width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 20px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 100% ; height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 10pt; text-decoration: underline">BENEFICIARY INFO</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="2">';
            shtml +=  '<span><span style="font-size: 10pt"><strong>&nbsp;Name:</strong> ' + dt.Rows[0].BenefName + '<br />';
            shtml +=  '</span></span>';
            shtml +=  '<table style="font-size: 10pt; width: 100%; vertical-align: top;">';
            shtml +=  '<tr>';
            shtml +=  '<td  style="width: 60px; vertical-align: top;"><strong style="width: 30px">Address:<br />';
            shtml +=  '</strong>';
            shtml +=  '</td>';
            shtml +=  '<td rowspan="3" style="width: 90%; height: 50px; vertical-align: top;">';
            shtml +=  '' + dt.Rows[0].BenefAddress + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<span><span style="font-size: 10pt"><strong>Telephone Number:</strong> ' + dt.Rows[0].BenefTelNo + '</span></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="vertical-align: top;  width: 100%; height: 20px; border-bottom: black thin solid;">';
            shtml +=  '<table style="font-size: 10pt; width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 10pt; text-decoration: underline">TRANSACTION DETAILS</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 10pt">';
            shtml +=  '<td colspan="3" rowspan="2" style="font-size: 10pt">';
            shtml +=  '<table style="font-size: 10pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Beneficiarys Account No. : </strong>' + dt.Rows[0].BenefAcctNo + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Thru PNB Branch: </strong>' + dt.Rows[0].ThruPNBBranch + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Other Banks Name / Branch: </strong>' + dt.Rows[0].OtherBankName + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Trans Type/Products: </strong>' + dt.Rows[0].TransType + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Source of Funds: </strong>' + dt.Rows[0].SourceFunds + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Member ID:</strong>' + dt.Rows[0].MemberID + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Purpose of Remittance: </strong>' + dt.Rows[0].PurposeRemittance + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Remittance Transaction Source: </strong>' + dt.Rows[0].RemittanceTranxSource + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 10pt">';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 10pt">';
            shtml +=  '<td rowspan="1" style="border-top-width: thin; border-left-width: thin; border-left-color: black;';
            shtml +=  'border-bottom-width: thin; border-bottom-color: black; vertical-align: top; width: 100%;';
            shtml +=  'border-top-color: black; height: 20px; border-right-width: thin; border-right-color: black">';
            shtml +=  '<table style="width: 100%; height: 20px;font-size: 10pt;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px;font-size: 10pt;">';
            shtml +=  '<p style="text-align: center">';
            shtml +=  '<span style="font-size: 10pt; text-decoration: underline"><strong>CLIENT/CUSTOMER UNDERTAKING</strong></span></p>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="2" style="text-align: justify; line-height: 18pt">';
            shtml +=  '<table style="width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="3" style="text-align: justify;font-size: 10pt;line-height: 18pt">';
            shtml +=  '<br />';
            shtml +=  'I/We hereby agree that the PNB GLOBAL REMITTANCE AND FINANCIAL COMPANY (HK) ';
            shtml +=  'LIMITED reserves the right to, at any time, obtain further information/document ';
            shtml +=  'from me/us.';
            shtml +=  '<br/>';
            shtml +=  '<br/>';
            shtml +=  'I/We have read and understood the Notice to Customer relating to the Personal ';
            shtml +=  'Data (Privacy) Ordinance and all the terms and conditions printed overleaf.';
            shtml +=  '<br/>';
            shtml +=  '<br/>';
            shtml +=  'I also confirm that details of my beneficiary are correct.';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<br />';
            shtml +=  '<br />';
            shtml +=  '<hr style="width: 378px; height: 1px" />';
            shtml +=  '<div style="width: 100%; height: 25px; text-align: center">';
            shtml +=  'Remitter&#039;s Signature</div>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '<td colspan="2" style="border-right: black thin solid; border-top: black thin solid;';
            shtml +=  'vertical-align: top; border-left: black thin solid; width: 50%; border-bottom: black thin solid;';
            shtml +=  'height: 20px">';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 0px; height: 90px">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 86px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="font-size: 10pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 18px; text-align: center">';
            shtml +=  '<span style="font-size: 10pt; text-decoration: underline"><strong>REMITTANCE DETAILS</strong></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td style="text-align: left; width: 245px; height: 15px;">';
	        shtml +=  '    <strong>';
	        shtml +=  'Net Amount Due to Beneficiary:</strong></td>';
	        shtml +=  '<td style="width: 58px; height: 15px">';
	        if (dt.Rows[0].NetAmountDueToBenefCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].NetAmountDueToBenefCurrCode + '</td>';
	        }
	        shtml +=  '<td style="height: 15px; text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].NetAmountDueToBenefAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Exchange Rate Used:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].ExchangeRateUsedCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].ExchangeRateUsedCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].ExchangeRateUsedAmt,4) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Remittance Amount:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].RemittanceAmountCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].RemittanceAmountCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].RemittanceAmountAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Cable:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].CableCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].CableCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].CableAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Remittance Fee:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].RemittanceFeeCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].RemittanceFeeCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].RemittanceFeeAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Other Charges:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].OtherChargesCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].OtherChargesCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].OtherChargesAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="width: 221px">';
	        shtml +=  '<strong>';
	        shtml +=  'Total amount received from customer:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].TotalAmtRcvdFromCustCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].TotalAmtRcvdFromCustCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].TotalAmtRcvdFromCustAmt,2) + '</td>';
	        shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 70px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;font-size:10pt;">';
	        shtml +=  '' + dt.Rows[0].Check + '<br />';
	        shtml +=  '' + dt.Rows[0].Cash + '<br />';
	        shtml +=  '' + dt.Rows[0].SpecialMessage + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="font-size:10pt;width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 40px; text-align: center; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 177px;font-size:10pt">';
            shtml +=  '<strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Payment ';
            shtml +=  '<br />';
            shtml +=  'Received / Processed By:</strong></td>';
            shtml +=  '<td style="font-size:10pt;height:30px;">';
            shtml +=  '' + dt.Rows[0].ProcessedBy + '<br />';
            shtml +=  '<hr style="width: 200px; height: 2px;background-color:black;" />';
            shtml +=  '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Authorized Personnel&nbsp;</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="border-top-width: thin; border-left-width: thin; border-left-color: black;';
            shtml +=  'border-bottom-width: thin; border-bottom-color: black; vertical-align: top; width: 100%;';
            shtml +=  'border-top-color: black; height: 20px; text-align: center; border-right-width: thin;';
            shtml +=  'border-right-color: black">';
            shtml +=  '<table style="width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px" align="center">';
            shtml +=  '<span style="font-size: 10pt; text-decoration: underline"><strong>DECLARATIONS</strong></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            if (dt.Rows[0].ReceiptDeclarationSub5.toString().length == 0)
            {
            shtml +=  '<td colspan="3" rowspan="2" style="font-size: 8pt;text-align: justify; line-height: 18pt;">';
            }else{
            shtml +=  '<td colspan="3" rowspan="2" style="font-size: 7pt;text-align: justify; line-height: 18pt;">';
            }
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub1 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub2 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub3 + '<br />';   
            //shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub4 + '<br />';
            //shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub5 + '<br />';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
         

        return shtml;
    }
    
    this.half_page = function(dt)
    {
        var shtml = "";
        shtml +=  '<table style="width: 8.5in">';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" style="border-right: black thin dotted; border-top: black thin dotted; border-left: black thin dotted; border-bottom: black thin dotted">';
        shtml +=  '<table style="height: 0%">';
        shtml +=  '<tr>';
        shtml +=  '<td style="font-size: 10pt; width: 371px; color: black; font-family: arial; height: 22px;';
        shtml +=  'text-align: center">';
        shtml +=  '<span style="font-size: 8pt; text-decoration: underline"><span style="font-size: 8.5pt;';
        shtml +=  'text-decoration: underline"><strong>IMPORTANT REMINDERS TO CLIENT / CUSTOMER</strong></span></span></td>';
        shtml +=  '<td rowspan="3" style="font-size: 6.5px; vertical-align: top; width: 50%; color: black;';
        shtml +=  'font-family: arial; height: 101px; text-align: left">';
        shtml +=  '<table style="font-size: 6.5pt; width: 100%; height: 23px">';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" >';
        shtml +=  'To the PNB __________________ , Branch: _____________ Date____________: This is';
        shtml +=  'to authorize your office to change my (pls. mark check the box/es below as applicable),';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" nowrap>';
        shtml +=  '<input id="Checkbox7" style="width: 26px; height: 12px" type="checkbox" />Remitter';
        shtml +=  'Name to _______________________________________________________</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" nowrap>';
        shtml +=  '<input id="Checkbox8" style="width: 26px; height: 12px" type="checkbox" />Address';
        shtml +=  '&nbsp;to _______________________________________ Postal Code: _________</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" nowrap>';
        shtml +=  '<input id="Checkbox9" style="width: 26px; height: 12px" type="checkbox" />Telephone';
        shtml +=  'No to: __________________________ Mobile No to: _________________</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" nowrap>';
        shtml +=  '<input id="Checkbox10" style="width: 26px; height: 12px" type="checkbox" />Beneficiary';
        shtml +=  '&nbsp;Name to: ________________________________Postal Code: _______</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" nowrap>';
        shtml +=  '<input id="Checkbox11" style="width: 26px; height: 12px" type="checkbox" />Beneficiary';
        shtml +=  'Address To: _________________________________________________</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" nowrap>';
        shtml +=  '<input id="Checkbox12" style="width: 26px; height: 12px" type="checkbox" />Beneficiary';
        shtml +=  'Telephone No to: ___________________Mobile No to: ______________</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" nowrap>';
        shtml +=  '<input id="Checkbox13" style="width: 26px; height: 12px" type="checkbox" />Others';
        shtml +=  '(please specify): _________________________________________________</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3">';
        shtml +=  'To support the request for ammendment, photocopy of the following document/s or';
        shtml +=  'ID/s is/are provided (*should match the document/s or ID/s required in No.2 of important';
        shtml +=  'Reminders to Client / Customer):';
        shtml +=  '<br />';
        shtml +=  'A) _______________&nbsp; B_________________ C)_________________ D)_______________</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3">';
        shtml +=  '<table style="width: 100%">';
        shtml +=  '<tr>';
        shtml +=  '<td rowspan="3" style="font-size: 8pt;border-right: black thin solid; border-top: black thin solid;';
        shtml +=  'border-left: black thin solid; border-bottom: black thin solid">';
        shtml +=  'PNB Use Only:<br />';
        shtml +=  'Processed By____Date:___</td>';
        shtml +=  '<td colspan="2" rowspan="3" style="font-size: 8pt;text-align: center">';
        shtml +=  '<hr style="width: 234px; height: 1px" />';
        shtml +=  '&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; Remitter Signature Over Printed Name</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td rowspan="2" style="font-size: 10px; vertical-align: top; width: 371px; color: black;';
        shtml +=  'font-family: arial; height: 113px; text-align: left; line-height: 13pt;">';
        shtml +=  '<span style="font-size: 8pt">1) Please notify the bank of any change in your name,';
        shtml +=  'address, telephone number or other details pertaining to your remittance account';
        shtml +=  '/ record by mailing / submitting the following: a) Photocopy of valid document/s';
        shtml +=  'or ID/s to support the amendment.';
        shtml +=  '<br />';
        shtml +=  'b) Duly accomplished and signed " Authorization and Notice of Amendment" cutout';
        shtml +=  'found on the opposite bottom right corner of this receipt. 2) Mailing address should';
        shtml +=  'tally with the clients registered address shown in the valid document/s or ID/s,';
        shtml +=  'as any irregularity in the registered address may result in non - implementation';
        shtml +=  'of remittance. The valid document/s or ID/s are the following*:';
        shtml +=  '<br />';
        shtml +=  '</span>';
        shtml +=  '<table style="font-size: 8pt; width: 303px; height: 45px">';
        shtml +=  '<tr>';
        shtml +=  '<td style="width: 50%; border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; border-bottom: black thin solid;">';
        shtml +=  'A) ' + dt.Rows[0].ID1 + '</td>';
        shtml +=  '<td style="width: 50%; border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; border-bottom: black thin solid;">';
        shtml +=  'B) ' + dt.Rows[0].ID2 + '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td style="width: 50%; border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; border-bottom: black thin solid;">';
        shtml +=  'C) ' + dt.Rows[0].ID3 + '</td>';
        shtml +=  '<td style="width: 50%; border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; border-bottom: black thin solid;">';
        shtml +=  'D) ' + dt.Rows[0].ID4 + '</td>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '<span style="font-size: 8pt">3) Always provide the bank with clear and proper instructions';
        shtml +=  'as to beneficiary details and remittance amount to facilitate processing of remittance.';
        shtml +=  '</span>';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr style="font-size: 10pt">';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        return shtml;
    }
    this.Receipt_body_halfpage = function(dt)
    {
    var vd = window.location.href.split('/')[3];
    
    var shtml = "";
            shtml +=  '<div>';
            shtml +=  '<table style="width: 8.5in; font-size: 8.5pt; font-family: arial;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="width: 0in">';
            shtml +=  '<table style="width: 100%;font-size: 8.5pt">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 60px; height: 15px">';
            shtml +=  '<img src="../../../'+ vd +'/images/pnb_black_logo.JPG" style="width: 58px; height: 68px" /></td>';
            shtml +=  '<td style="vertical-align: top; width: 520px; height: 15px">';
            shtml +=  '<strong>' + dt.Rows[0].Header1 + '</strong><br />';
            shtml +=  '' + dt.Rows[0].Header2 + '<br />';
            shtml +=  '<strong>' + dt.Rows[0].NameofBranch + '</strong><br />';
            shtml +=  '' + dt.Rows[0].AdressofBranch + '<br />';
            shtml +=  '' + dt.Rows[0].BranchofContact + '<br />';
            shtml +=  '' + dt.Rows[0].BranchofEmail + '<br />';
            shtml +=  '' + dt.Rows[0].corp_address + '<br />';
            shtml +=  '' + dt.Rows[0].corp_telno + '</td>';
            shtml +=  '<td style="vertical-align: top; height: 15px">';
            shtml +=  'Transaction Date: ' + HTMLRep.formatDate(dt.Rows[0].TranxDate,"/") + '<br />';
	        if (dt.Rows[0].ClearingDate == '1900')
	        {
	        shtml +=  'Clearing Date: <br />';
	        }else{
	        shtml +=  'Clearing Date: '+ dt.Rows[0].ClearingDate +'<br />';
	        }
            shtml +=  'Time: '+ HTMLRep.formatTime(dt.Rows[0].TranxDate) +'<br />';
            shtml +=  'Loyalty Points:';
            shtml +=  '<br />';
            shtml +=  '' + dt.Rows[0].TranStatus + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 8.5in; font-size: 8pt; font-family: arial;';
            shtml +=  'height: 0.23in; border-top-width: thin; border-left-width: thin; border-left-color: black; border-bottom-width: thin; border-bottom-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="border-right: black thin solid; border-top: black thin solid;';
            shtml +=  'border-left: black thin solid; border-bottom: black thin solid">';
            shtml +=  '<table style="font-size: 8.5pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 244px; height: 14px">';
            shtml +=  '<strong>Remitter ID No.: </strong>' + dt.Rows[0].RemitterID + '</td>';
            shtml +=  '<td style="width: 285px; height: 14px">';
            shtml +=  '<strong>Official Receipt No.:</strong> ' + dt.Rows[0].ReceiptNo + '</td>';
            shtml +=  '<td style="width: 257px; height: 14px">';
            shtml +=  '<strong>Reference No.:</strong> ' + dt.Rows[0].ReferenceNo + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';    
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td style="border-right: black thin solid; border-top: black thin solid; vertical-align: top;';
            shtml +=  'border-left: black thin solid; width: 50%; border-bottom: black thin solid; height: 20px">';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 0px; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="vertical-align: top; width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 20px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 8.5pt; text-decoration: underline">REMITTER INFO</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="2">';
            shtml +=  '<span><span style="font-size: 8.5pt"><strong>&nbsp;Name: </strong>' + dt.Rows[0].RemitterName + '<br />';
            shtml +=  '</span></span>';
            shtml +=  '<table style="font-size: 8.5pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="width: 61px; vertical-align: top;">';
            shtml +=  '<strong style="width: 30px">Address:<br />';
            shtml +=  '</strong></td>';
            shtml +=  '<td colspan="2" rowspan="3" style="height: 50px; vertical-align: top;">';
            shtml +=  '' + dt.Rows[0].RemitterAddress + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<span><span style="font-size: 8.5pt; width: 100%"><strong>Telephone Number:</strong>';
            shtml +=  '' + dt.Rows[0].RemitterTelNo + '</span></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="vertical-align: top; width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 20px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 100% ; height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 10pt; text-decoration: underline">BENEFICIARY INFO</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="2">';
            shtml +=  '<span><span style="font-size: 10pt"><strong>&nbsp;Name:</strong> ' + dt.Rows[0].BenefName + '<br />';
            shtml +=  '</span></span>';
            shtml +=  '<table style="font-size: 10pt; width: 100%; vertical-align: top;">';
            shtml +=  '<tr>';
            shtml +=  '<td  style="width: 60px; vertical-align: top;"><strong style="width: 30px">Address:<br />';
            shtml +=  '</strong>';
            shtml +=  '</td>';
            shtml +=  '<td rowspan="3" style="width: 90%; height: 50px; vertical-align: top;">';
            shtml +=  '' + dt.Rows[0].BenefAddress + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<span><span style="font-size: 8.5pt"><strong>Telephone Number:</strong> ' + dt.Rows[0].BenefTelNo + '</span></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="vertical-align: top;  width: 100%; height: 20px; border-bottom: black thin solid;">';
            shtml +=  '<table style="font-size: 8.5pt; width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 8.5pt; text-decoration: underline">TRANSACTION DETAILS</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 8.5pt">';
            shtml +=  '<td colspan="3" rowspan="2" style="font-size: 8.5pt">';
            shtml +=  '<table style="font-size: 8.5pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Beneficiarys Account No. : </strong>' + dt.Rows[0].BenefAcctNo + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Thru PNB Branch: </strong>' + dt.Rows[0].ThruPNBBranch + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Other Banks Name / Branch: </strong>' + dt.Rows[0].OtherBankName + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Trans Type/Products: </strong>' + dt.Rows[0].TransType + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Source of Funds: </strong>' + dt.Rows[0].SourceFunds + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Member ID:</strong>' + dt.Rows[0].MemberID + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Purpose of Remittance: </strong>' + dt.Rows[0].PurposeRemittance + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Remittance Transaction Source: </strong>' + dt.Rows[0].RemittanceTranxSource + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 8.5pt">';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 8.5pt">';
            shtml +=  '<td rowspan="1" style="border-top-width: thin; border-left-width: thin; border-left-color: black;';
            shtml +=  'border-bottom-width: thin; border-bottom-color: black; vertical-align: top; width: 100%;';
            shtml +=  'border-top-color: black; height: 20px; border-right-width: thin; border-right-color: black">';
            shtml +=  '<table style="width: 100%; height: 20px;font-size: 8.5pt;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px">';
            shtml +=  '<p style="text-align: center">';
            shtml +=  '<span style="font-size: 8.5pt; text-decoration: underline"><strong>CLIENT/CUSTOMER UNDERTAKING</strong></span></p>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="2" style="text-align: justify; line-height: 14pt;">';
            shtml +=  '<table style="width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="3" style="text-align: justify;font-size: 8.5pt;line-height: 14pt;">';                       
            shtml +=  'I/We hereby agree that the PNB GLOBAL REMITTANCE AND FINANCIAL COMPANY (HK) ';
            shtml +=  'LIMITED reserves the right to, at any time, obtain further information/document ';
            shtml +=  'from me/us.';
            shtml +=  '<br/>';
            shtml +=  '<br/>';
            shtml +=  'I/We have read and understood the Notice to Customer relating to the Personal ';
            shtml +=  'Data (Privacy) Ordinance and all the terms and conditions printed overleaf.';
            shtml +=  '<br/>';
            shtml +=  '<br/>';
            shtml +=  'I also confirm that details of my beneficiary are correct.';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<br />';
            shtml +=  '<hr style="width: 378px; height: 1px" />';
            shtml +=  '<div style="width: 100%; height: 25px; text-align: center">';
            shtml +=  'Remitter&#039;s Signature</div>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '<td colspan="2" style="border-right: black thin solid; border-top: black thin solid;';
            shtml +=  'vertical-align: top; border-left: black thin solid; width: 50%; border-bottom: black thin solid;';
            shtml +=  'height: 20px">';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 0px; height: 90px">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 86px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="font-size: 8.5pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 18px; text-align: center">';
            shtml +=  '<span style="font-size: 8.5pt; text-decoration: underline"><strong>REMITTANCE DETAILS</strong></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td style="text-align: left; width: 245px; height: 15px;">';
	        shtml +=  '    <strong>';
	        shtml +=  'Net Amount Due to Beneficiary:</strong></td>';
	        shtml +=  '<td style="width: 58px; height: 15px">';
	        if (dt.Rows[0].NetAmountDueToBenefCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].NetAmountDueToBenefCurrCode + '</td>';
	        }
	        shtml +=  '<td style="height: 15px; text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].NetAmountDueToBenefAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Exchange Rate Used:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].ExchangeRateUsedCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].ExchangeRateUsedCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].ExchangeRateUsedAmt,4) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Remittance Amount:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].RemittanceAmountCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].RemittanceAmountCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].RemittanceAmountAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Cable:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].CableCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].CableCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].CableAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Remittance Fee:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].RemittanceFeeCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].RemittanceFeeCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].RemittanceFeeAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Other Charges:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].OtherChargesCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].OtherChargesCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].OtherChargesAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="width: 221px">';
	        shtml +=  '<strong>';
	        shtml +=  'Total amount received from customer:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].TotalAmtRcvdFromCustCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].TotalAmtRcvdFromCustCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].TotalAmtRcvdFromCustAmt,2) + '</td>';
	        shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 70px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;font-size: 8pt">';
	        shtml +=  '' + dt.Rows[0].Check + '<br />';
	        shtml +=  '' + dt.Rows[0].Cash + '<br />';
	        shtml +=  '' + dt.Rows[0].SpecialMessage + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="font-size:8.5pt;width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 40px; text-align: center; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 177px;font-size:8.5pt">';
            shtml +=  '<strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Payment ';
            shtml +=  '<br />';
            shtml +=  'Received / Processed By:</strong></td>';
            shtml +=  '<td style="font-size:8.5pt;">';
            shtml +=  '' + dt.Rows[0].ProcessedBy + '<br />';
            shtml +=  '<hr style="width: 200px; height: 2px;background-color:black;" />';
            shtml +=  '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Authorized Personnel&nbsp;</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="border-top-width: thin; border-left-width: thin; border-left-color: black;';
            shtml +=  'border-bottom-width: thin; border-bottom-color: black; vertical-align: top; width: 100%;';
            shtml +=  'border-top-color: black; height: 20px; text-align: center; border-right-width: thin;';
            shtml +=  'border-right-color: black">';
            shtml +=  '<table style="width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px;text-align:center;">';
            shtml +=  '<span style="font-size: 8.5pt; text-decoration: underline"><strong>DECLARATIONS</strong></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            if (dt.Rows[0].ReceiptDeclarationSub5.toString().length == 0)
            {
            shtml +=  '<td colspan="3" rowspan="2" style="font-size: 8pt;text-align: justify; line-height: 14pt;">';
            }else{
            shtml +=  '<td colspan="3" rowspan="2" style="font-size: 7pt;text-align: justify; line-height: 14pt;">';
            }            
            
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub1 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub2 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub3 + '<br />';   
           // shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub4 + '<br />';
           // shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub5 + '<br />';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</div>';

            return shtml;
    }
}
/*END HONGKONG OBP RECEIPT*/

/*START OBP RECEIPT -- ONE PAGE*/
function HTMLReceiptOBP_OnePage()
{
    var cls = this;
    var _branchcode;    
    var _seqid;
	var _referenceno;
    
    this.evaluate_Receipt_query = function(query)
    {
        var squery = unescape(query.toString());
        if (squery.toLowerCase().indexOf("trantype=obp") > -1)
        {
            _branchcode = HTMLRep.get_branchcode();
            this.get_Receipt_data(
                                    HTMLRep.get_querystring_value(query,"tranvalue"),
                                    HTMLRep.get_branchcode(),
                                    ""
                                 )
            return;
        }
        if (squery.toLowerCase().indexOf("mergeparam") > -1)
        {
             _branchcode = HTMLRep.parse_merge_param_query(squery,"T@v_branchcode")
             this.get_Receipt_data(
                                    "",
                                    HTMLRep.parse_merge_param_query(squery,"T@v_branchcode"),
                                    HTMLRep.parse_merge_param_query(squery,"T@v_referenceno")
									)
            return;
        }
    }
    this.get_Receipt_data = function(seq_id,branchcode,referenceno)
    {
        var params; 
        var sp_name = "";
	var pass  =	false;
        
                _branchcode =   " @v_branchcode="  + branchcode;    
                _seqid    =   "@seqid="     + seq_id;
				_referenceno = " @v_referenceno="     + referenceno;
        
	pass = this.check_newdbswitch();
	
       if(seq_id.length == 0)
	   {
			if (pass)
			{
			    sp_name = "sp_otc_obp_Receipt_reprint_irsw";
			}
			else
			{
			    sp_name = "sp_otc_obp_Receipt_reprint";
			}
			params  = _branchcode + "^" + _referenceno ;
	   }else{
			sp_name = "SP_OTC_OBP_Receipt";
			params  = _branchcode + "^" + _seqid ;
		}
		AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
            alert(errmsg);       return;      
        } 
	    IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params,this.get_Receipt_data_callback);
    }
    
    this.get_Receipt_data_callback = function(result)
    {
        try
        {
            var ds = result.value;
            
            cls.check_branch();
            cls.display_Receipt_report(ds);
            
            
        }
        catch(e)
        {
            alert("ERROR WHILE GETTING Receipt DATA");
        }
    }
    this.check_newdbswitch = function()
    {
    
	var pass = false;
	var sp_name = 'proc_check_newdbswitch';
        var squery = HTMLRep.get_hidden_querystring("hidQueryString");
        var sbranchcode = HTMLRep.get_branchcode();
        
        var params  =  '@branchcode=' + sbranchcode;
        var isRCC  = false;
        AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
            alert(errmsg);     return;        
        } 
        
        var dtcchecksw = IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params).value;
        
        try
        {
            if (dtcchecksw.Tables[0].Rows.length > 0)
            {
                pass = true;
            }
        }
        catch(e)
        {
        }
	
	return pass; 
    }
    
    this.display_Receipt_report = function(ds)
    {
        var sp_name = 'proc_check_reporthalfpage';
        var squery = HTMLRep.get_hidden_querystring("hidQueryString");
        var sbranchcode = ds.Tables[0].Rows[0].ReferenceNo;
        sbranchcode =  sbranchcode.substring(1,4);
        var params  =  '@v_branhcode=' + sbranchcode;
        var ishalfpage  = false;
        AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
            alert(errmsg);     return;        
        } 
        
        var dtcheckhalfpage = IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params).value;
        
        try
        {
            if (dtcheckhalfpage.Tables[0].Rows.length > 0)
            {
                ishalfpage = true;
            }
        }
        catch(e)
        {
        }
    
        
        var shtml = "";        
        shtml +=  HTMLRep.print_html_button();
        var isRCC = cls.check_branch();
	
	isRCC = (cls.check_ars()) ? 0 : isRCC ;
	
        if (!ishalfpage)
        {
           
            shtml +=  this.Receipt_body(ds.Tables[0],isRCC);
               
            shtml += cls.half_page(ds.Tables[0]);
            
	        shtml +=  '</div>';
        }
        else
        {
           shtml +=  this.Receipt_body_halfpage(ds.Tables[0],isRCC);
           
        }
        document.getElementById("bodydiv").innerHTML = shtml
    }
    this.check_branch = function()
    {
        var sp_name = 'sp_report_checkbranch';
        var squery = HTMLRep.get_hidden_querystring("hidQueryString");
        var sbranchcode = HTMLRep.get_branchcode();
        
        var params  =  '@branchcode=' + sbranchcode;
        var isRCC  = false;
        AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
            alert(errmsg);     return;        
        } 
        
        var dtccheckbranch = IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params).value;
        
        try
        {
            if (dtccheckbranch.Tables[0].Rows.length > 0)
            {
                isRCC = true;
            }
        }
        catch(e)
        {
        }
        return isRCC;
    }
    
    this.check_ars = function()
    {
        var sp_name = 'proc_check_ars';
        
        var sbranchcode = HTMLRep.get_branchcode();
        
        var params  =  '@v_branchcode=' + sbranchcode;
        var isARS  = false;
        AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
            alert(errmsg);     return;        
        } 
        
        var dtccheckbranch = IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params).value;
        
        try
        {
            if (dtccheckbranch.Tables[0].Rows.length > 0)
            {
                isARS = true;
            }
        }
        catch(e)
        {
        }
        return isARS;
    }
    this.Receipt_body = function(dt,isRCC)
    {
	
	    var vd = window.location.href.split('/')[3];
	    
        var shtml = "";
	        shtml +=  '<br />';
            shtml +=  '<br />';
            shtml +=  '<div style="width: 970px; margin: 0px;">';
            shtml +=  '<table style="width: 100%; font-size: 9.5pt; font-family: arial;" cellspacing="0" cellpadding="0">';
            //shtml +=  '<table style="width: 8.5in; font-size: 7pt; font-family: arial;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="width: 0in">';
            shtml +=  '<table style="width: 100%;font-size: 9pt" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 60px; height: 15px">';
            shtml +=  '<img src="../../../'+ vd +'/images/pnb_black_logo.JPG" style="width: 58px; height: 68px" /></td>';
            shtml +=  '<td style="vertical-align: top; width: 520px; height: 15px">';
            shtml +=  '<strong>' + dt.Rows[0].Header1 + '</strong><br />';
            shtml +=  '' + dt.Rows[0].Header2 + '<br />';
            shtml +=  '<strong>' + dt.Rows[0].NameofBranch + '</strong><br />';
            shtml +=  '' + dt.Rows[0].AdressofBranch + '<br />';
            shtml +=  '' + dt.Rows[0].BranchofContact + '<br />';
            shtml +=  '' + dt.Rows[0].BranchofEmail + '<br />';
            shtml +=  '' + dt.Rows[0].corp_address + '<br />';
            shtml +=  '' + dt.Rows[0].corp_telno + '</td>';
            shtml +=  '<td style="vertical-align: top; height: 15px">';
            shtml +=  'Transaction Date: ' + HTMLRep.formatDate(dt.Rows[0].TranxDate,"/") + '<br />';
	        if (dt.Rows[0].ClearingDate == '1900')
	        {
	        shtml +=  'Clearing Date: <br />';
	        }else{
	        shtml +=  'Clearing Date: '+ dt.Rows[0].ClearingDate +'<br />';
	        }
            shtml +=  'Time: '+ HTMLRep.formatTime(dt.Rows[0].TranxDate) +'<br />';
            shtml +=  'Loyalty Points:';
            shtml +=  '<br />';
            shtml +=  '' + dt.Rows[0].TranStatus + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 100%; font-size: 9.5pt; font-family: arial;';
            shtml +=  'height: 0.23in; border-top-width: thin; border-left-width: thin; border-left-color: black; border-bottom-width: thin; border-bottom-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="border-right: black thin solid; border-top: black thin solid;';
            shtml +=  'border-left: black thin solid; border-bottom: black thin solid">';
            shtml +=  '<table style="font-size: 9.5pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 244px; height: 14px">';
            shtml +=  '<strong>Remitter ID No.: </strong>' + dt.Rows[0].RemitterID + '</td>';
            shtml +=  '<td style="width: 285px; height: 14px">';
            shtml +=  '<strong>Official Receipt No.:</strong> ' + dt.Rows[0].ReceiptNo + '</td>';
            shtml +=  '<td style="width: 257px; height: 14px">';
            shtml +=  '<strong>Reference No.:</strong> ' + dt.Rows[0].ReferenceNo + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';    
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td style="border-right: black thin solid; border-top: black thin solid; vertical-align: top;';
            shtml +=  'border-left: black thin solid; width: 50%; border-bottom: black thin solid; height: 20px">';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 0px; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="vertical-align: top; width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 20px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 9.5pt; text-decoration: underline">REMITTER INFO</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="2">';
            shtml +=  '<span><span style="font-size: 9pt"><strong>&nbsp;Name: </strong>' + dt.Rows[0].RemitterName + '<br />';
            shtml +=  '</span></span>';
            shtml +=  '<table style="font-size: 9pt; width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="width: 61px; vertical-align: top;">';
            shtml +=  '<strong style="width: 30px">Address:<br />';
            shtml +=  '</strong></td>';
            shtml +=  '<td colspan="2" rowspan="3" style="height: 40px; vertical-align: top;">';
            shtml +=  '' + dt.Rows[0].RemitterAddress + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<span><span style="font-size: 9pt; width: 100%"><strong>Telephone Number:</strong>';
            shtml +=  '' + dt.Rows[0].RemitterTelNo + '</span></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="vertical-align: top; width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 20px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%; height: 20px" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 100% ; height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 9.5pt; text-decoration: underline">BENEFICIARY INFO</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="2">';
            shtml +=  '<span><span style="font-size: 9pt"><strong>&nbsp;Name:</strong> ' + dt.Rows[0].BenefName + '<br />';
            shtml +=  '</span></span>';
            shtml +=  '<table style="font-size: 9pt; width: 100%; vertical-align: top;" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td  style="width: 60px; vertical-align: top;"><strong style="width: 30px">Address:<br />';
            shtml +=  '</strong>';
            shtml +=  '</td>';
            shtml +=  '<td rowspan="3" style="width: 90%; height: 40px; vertical-align: top;">';
            shtml +=  '' + dt.Rows[0].BenefAddress + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<span><span style="font-size: 9pt"><strong>Telephone Number:</strong> ' + dt.Rows[0].BenefTelNo + '</span></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="vertical-align: top;  width: 100%; height: 20px; border-bottom: black thin solid;">';
            shtml +=  '<table style="font-size: 9pt; width: 100%; height: 20px" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 9.5pt; text-decoration: underline">TRANSACTION DETAILS</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 9pt">';
            shtml +=  '<td colspan="3" rowspan="2" style="font-size: 8.5pt">';
            shtml +=  '<table style="font-size: 9pt; width: 100%; line-height: 14pt" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Beneficiarys Account No. : </strong>' + dt.Rows[0].BenefAcctNo + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Thru PNB Branch: </strong>' + dt.Rows[0].ThruPNBBranch + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Other Banks Name / Branch: </strong>' + dt.Rows[0].OtherBankName + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Trans Type/Products: </strong>' + dt.Rows[0].TransType + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Source of Funds: </strong>' + dt.Rows[0].SourceFunds + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Member ID:</strong>' + dt.Rows[0].MemberID + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Purpose of Remittance: </strong>' + dt.Rows[0].PurposeRemittance + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Remittance Transaction Source: </strong>' + dt.Rows[0].RemittanceTranxSource + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 9pt">';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 9pt">';
            shtml +=  '<td rowspan="1" style="border-top-width: thin; border-left-width: thin; border-left-color: black;';
            shtml +=  'border-bottom-width: thin; border-bottom-color: black; vertical-align: top; width: 100%;';
            shtml +=  'border-top-color: black; height: 20px; border-right-width: thin; border-right-color: black">';
            shtml +=  '<table style="width: 100%; height: 20px;font-size: 8pt;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px">';
            shtml +=  '<p style="text-align: center">';
            shtml +=  '<span style="font-size: 9.5pt; text-decoration: underline"><strong>CLIENT/CUSTOMER UNDERTAKING</strong></span></p>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="2" style="text-align: justify;line-height: 16pt">';// line-height: 14pt;
            shtml +=  '<table style="width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="3" style="text-align: justify;font-size: 9pt;line-height: 15pt">'; //line-height: 14pt;                      
            shtml +=  'I certify that all the information contained herein is true and correct, and that';
            shtml +=  'I am amenable to the Terms and Conditions (if any) governing this transaction.';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<br />';
            shtml +=  '<hr style="width: 378px; height: 1px; text-align: center"/>';
            shtml +=  '<div style="width: 100%; height: 25px; text-align: center">';
            shtml +=  'Remitter&#039;s Signature</div>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            // Edited by CHR from colspan=2 to colspan=1
            shtml +=  '<td style="border-right: black thin solid; border-top: black thin solid;';
            shtml +=  'vertical-align: top; border-left: black thin solid; width: 50%; border-bottom: black thin solid;';
            shtml +=  'height: 20px">';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 100%; height: 90px">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 86px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="font-size: 9pt; width: 100%; line-height: 14pt" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 18px; text-align: center">';
            shtml +=  '<span style="font-size: 9.5pt; text-decoration: underline"><strong>REMITTANCE DETAILS</strong></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td style="text-align: left; width: 245px; height: 15px;">';
	        shtml +=  '    <strong>';
	        shtml +=  'Net Amount Due to Beneficiary:</strong></td>';
	        shtml +=  '<td style="width: 58px; height: 15px">';
	        if (dt.Rows[0].NetAmountDueToBenefCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].NetAmountDueToBenefCurrCode + '</td>';
	        }
	        shtml +=  '<td style="height: 15px; text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].NetAmountDueToBenefAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Exchange Rate Used:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].ExchangeRateUsedCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].ExchangeRateUsedCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].ExchangeRateUsedAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Remittance Amount:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].RemittanceAmountCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].RemittanceAmountCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].RemittanceAmountAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Cable:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].CableCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].CableCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].CableAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Remittance Fee:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].RemittanceFeeCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].RemittanceFeeCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].RemittanceFeeAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Other Charges:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].OtherChargesCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].OtherChargesCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].OtherChargesAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="width: 221px">';
	        shtml +=  '<strong>';
	        shtml +=  'Total amount received from customer:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].TotalAmtRcvdFromCustCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].TotalAmtRcvdFromCustCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].TotalAmtRcvdFromCustAmt,2) + '</td>';
	        shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 70px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;font-size: 9pt">';
	        shtml +=  '' + dt.Rows[0].Check + '<br />';
	        shtml +=  '' + dt.Rows[0].Cash + '<br />';
	        shtml +=  '' + dt.Rows[0].SpecialMessage + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="font-size:9pt;width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 40px; text-align: center; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 177px;font-size:9pt">';
            shtml +=  '<strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Payment ';
            shtml +=  '<br />';
            shtml +=  'Received / Processed By:</strong></td>';
            shtml +=  '<td style="font-size:9pt;">';
            shtml +=  '' + dt.Rows[0].ProcessedBy + '<br />';
            shtml +=  '<hr style="width: 200px; height: 2px;background-color:black; text-align: left;" />';/////
            shtml +=  '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Authorized Personnel&nbsp;</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="border-top-width: thin; border-left-width: thin; border-left-color: black;';
            shtml +=  'border-bottom-width: thin; border-bottom-color: black; vertical-align: top; width: 100%;';
            shtml +=  'border-top-color: black; height: 20px; text-align: center; border-right-width: thin;';
            shtml +=  'border-right-color: black">';
            shtml +=  '<table style="width: 100%; height: 20px" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px;text-align:center;">';
            shtml +=  '<span style="font-size: 9.5pt; text-decoration: underline; line-height: 16pt"><strong>DECLARATIONS</strong></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            if (dt.Rows[0].ReceiptDeclarationSub5.toString().length == 0)
            {
                       shtml +=  '<td colspan="3" rowspan="2" style="font-size: 9pt;text-align: justify;line-height: 15pt">'; //line-height: 14pt;
            }else{
                        shtml +=  '<td colspan="3" rowspan="2" style="font-size: 9pt;text-align: justify; line-height: 15pt">';//line-height: 14pt;
            }
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub1 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub2 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub3 + '<br />';   
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub4 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub5 + '<br />';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            
              //START - FOR OBP 2nd Copy
            shtml +=  '<br />';
            shtml +=  '<br />';
            shtml +=  '<br />';
            shtml +=  '<br />';
            //shtml +=  '<br />';
            shtml +=  '<table style="width: 100%; font-size: 9.5pt; font-family: arial;" cellspacing="0" cellpadding="0">';
            //shtml +=  '<table style="width: 8.5in; font-size: 7pt; font-family: arial;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="width: 0in">';
            shtml +=  '<table style="width: 100%;font-size: 9pt" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 60px; height: 15px">';
            shtml +=  '<img src="../../../'+ vd +'/images/pnb_black_logo.JPG" style="width: 58px; height: 68px" /></td>';
            shtml +=  '<td style="vertical-align: top; width: 520px; height: 15px">';
            shtml +=  '<strong>' + dt.Rows[0].Header1 + '</strong><br />';
            shtml +=  '' + dt.Rows[0].Header2 + '<br />';
            shtml +=  '<strong>' + dt.Rows[0].NameofBranch + '</strong><br />';
            shtml +=  '' + dt.Rows[0].AdressofBranch + '<br />';
            shtml +=  '' + dt.Rows[0].BranchofContact + '<br />';
            shtml +=  '' + dt.Rows[0].BranchofEmail + '<br />';
            shtml +=  '' + dt.Rows[0].corp_address + '<br />';
            shtml +=  '' + dt.Rows[0].corp_telno + '</td>';
            shtml +=  '<td style="vertical-align: top; height: 15px">';
            shtml +=  'Transaction Date: ' + HTMLRep.formatDate(dt.Rows[0].TranxDate,"/") + '<br />';
	        if (dt.Rows[0].ClearingDate == '1900')
	        {
	        shtml +=  'Clearing Date: <br />';
	        }else{
	        shtml +=  'Clearing Date: '+ dt.Rows[0].ClearingDate +'<br />';
	        }
            shtml +=  'Time: '+ HTMLRep.formatTime(dt.Rows[0].TranxDate) +'<br />';
            shtml +=  'Loyalty Points:';
            shtml +=  '<br />';
            shtml +=  '' + dt.Rows[0].TranStatus + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 100%; font-size: 9.5pt; font-family: arial;';
            shtml +=  'height: 0.23in; border-top-width: thin; border-left-width: thin; border-left-color: black; border-bottom-width: thin; border-bottom-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="border-right: black thin solid; border-top: black thin solid;';
            shtml +=  'border-left: black thin solid; border-bottom: black thin solid">';
            shtml +=  '<table style="font-size: 9.5pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 244px; height: 14px">';
            shtml +=  '<strong>Remitter ID No.: </strong>' + dt.Rows[0].RemitterID + '</td>';
            shtml +=  '<td style="width: 285px; height: 14px">';
            shtml +=  '<strong>Official Receipt No.:</strong> ' + dt.Rows[0].ReceiptNo + '</td>';
            shtml +=  '<td style="width: 257px; height: 14px">';
            shtml +=  '<strong>Reference No.:</strong> ' + dt.Rows[0].ReferenceNo + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';    
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td style="border-right: black thin solid; border-top: black thin solid; vertical-align: top;';
            shtml +=  'border-left: black thin solid; width: 50%; border-bottom: black thin solid; height: 20px">';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 0px; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="vertical-align: top; width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 20px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 9.5pt; text-decoration: underline">REMITTER INFO</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="2">';
            shtml +=  '<span><span style="font-size: 9pt"><strong>&nbsp;Name: </strong>' + dt.Rows[0].RemitterName + '<br />';
            shtml +=  '</span></span>';
            shtml +=  '<table style="font-size: 9pt; width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="width: 61px; vertical-align: top;">';
            shtml +=  '<strong style="width: 30px">Address:<br />';
            shtml +=  '</strong></td>';
            shtml +=  '<td colspan="2" rowspan="3" style="height: 40px; vertical-align: top;">';
            shtml +=  '' + dt.Rows[0].RemitterAddress + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<span><span style="font-size: 9pt; width: 100%"><strong>Telephone Number:</strong>';
            shtml +=  '' + dt.Rows[0].RemitterTelNo + '</span></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="vertical-align: top; width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 20px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%; height: 20px" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 100% ; height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 9.5pt; text-decoration: underline">BENEFICIARY INFO</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="2">';
            shtml +=  '<span><span style="font-size: 9pt"><strong>&nbsp;Name:</strong> ' + dt.Rows[0].BenefName + '<br />';
            shtml +=  '</span></span>';
            shtml +=  '<table style="font-size: 9pt; width: 100%; vertical-align: top;" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td  style="width: 60px; vertical-align: top;"><strong style="width: 30px">Address:<br />';
            shtml +=  '</strong>';
            shtml +=  '</td>';
            shtml +=  '<td rowspan="3" style="width: 90%; height: 40px; vertical-align: top;">';
            shtml +=  '' + dt.Rows[0].BenefAddress + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<span><span style="font-size: 9pt"><strong>Telephone Number:</strong> ' + dt.Rows[0].BenefTelNo + '</span></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="vertical-align: top;  width: 100%; height: 20px; border-bottom: black thin solid;">';
            shtml +=  '<table style="font-size: 9pt; width: 100%; height: 20px" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 9.5pt; text-decoration: underline">TRANSACTION DETAILS</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 9pt">';
            shtml +=  '<td colspan="3" rowspan="2" style="font-size: 8.5pt">';
            shtml +=  '<table style="font-size: 9pt; width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Beneficiarys Account No. : </strong>' + dt.Rows[0].BenefAcctNo + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Thru PNB Branch: </strong>' + dt.Rows[0].ThruPNBBranch + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Other Banks Name / Branch: </strong>' + dt.Rows[0].OtherBankName + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Trans Type/Products: </strong>' + dt.Rows[0].TransType + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Source of Funds: </strong>' + dt.Rows[0].SourceFunds + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Member ID:</strong>' + dt.Rows[0].MemberID + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Purpose of Remittance: </strong>' + dt.Rows[0].PurposeRemittance + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Remittance Transaction Source: </strong>' + dt.Rows[0].RemittanceTranxSource + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 9pt">';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 9pt">';
            shtml +=  '<td rowspan="1" style="border-top-width: thin; border-left-width: thin; border-left-color: black;';
            shtml +=  'border-bottom-width: thin; border-bottom-color: black; vertical-align: top; width: 100%;';
            shtml +=  'border-top-color: black; height: 20px; border-right-width: thin; border-right-color: black">';
            shtml +=  '<table style="width: 100%; height: 20px;font-size: 8pt;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px">';
            shtml +=  '<p style="text-align: center">';
            shtml +=  '<span style="font-size: 9.5pt; text-decoration: underline"><strong>CLIENT/CUSTOMER UNDERTAKING</strong></span></p>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="2" style="text-align: justify;line-height: 16pt">';// line-height: 14pt;
            shtml +=  '<table style="width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="3" style="text-align: justify;font-size: 9pt;line-height: 14pt">'; //line-height: 14pt;                      
            shtml +=  'I certify that all the information contained herein is true and correct, and that';
            shtml +=  'I am amenable to the Terms and Conditions (if any) governing this transaction.';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<br />';
            shtml +=  '<hr style="width: 378px; height: 1px; text-align: center"/>';
            shtml +=  '<div style="width: 100%; height: 25px; text-align: center">';
            shtml +=  'Remitter&#039;s Signature</div>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            // Edited by CHR from colspan=2 to colspan=1
            shtml +=  '<td style="border-right: black thin solid; border-top: black thin solid;';
            shtml +=  'vertical-align: top; border-left: black thin solid; width: 50%; border-bottom: black thin solid;';
            shtml +=  'height: 20px">';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 100%; height: 90px">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 86px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="font-size: 9pt; width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 18px; text-align: center">';
            shtml +=  '<span style="font-size: 9.5pt; text-decoration: underline"><strong>REMITTANCE DETAILS</strong></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td style="text-align: left; width: 245px; height: 15px;">';
	        shtml +=  '    <strong>';
	        shtml +=  'Net Amount Due to Beneficiary:</strong></td>';
	        shtml +=  '<td style="width: 58px; height: 15px">';
	        if (dt.Rows[0].NetAmountDueToBenefCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].NetAmountDueToBenefCurrCode + '</td>';
	        }
	        shtml +=  '<td style="height: 15px; text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].NetAmountDueToBenefAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Exchange Rate Used:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].ExchangeRateUsedCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].ExchangeRateUsedCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].ExchangeRateUsedAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Remittance Amount:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].RemittanceAmountCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].RemittanceAmountCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].RemittanceAmountAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Cable:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].CableCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].CableCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].CableAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Remittance Fee:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].RemittanceFeeCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].RemittanceFeeCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].RemittanceFeeAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Other Charges:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].OtherChargesCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].OtherChargesCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].OtherChargesAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="width: 221px">';
	        shtml +=  '<strong>';
	        shtml +=  'Total amount received from customer:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].TotalAmtRcvdFromCustCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].TotalAmtRcvdFromCustCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].TotalAmtRcvdFromCustAmt,2) + '</td>';
	        shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 70px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;font-size: 9pt">';
	        shtml +=  '' + dt.Rows[0].Check + '<br />';
	        shtml +=  '' + dt.Rows[0].Cash + '<br />';
	        shtml +=  '' + dt.Rows[0].SpecialMessage + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="font-size:9pt;width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 40px; text-align: center; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 177px;font-size:9pt">';
            shtml +=  '<strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Payment ';
            shtml +=  '<br />';
            shtml +=  'Received / Processed By:</strong></td>';
            shtml +=  '<td style="font-size:9pt;">';
            shtml +=  '' + dt.Rows[0].ProcessedBy + '<br />';
            shtml +=  '<hr style="width: 200px; height: 2px;background-color:black; text-align: left;" />';/////
            shtml +=  '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Authorized Personnel&nbsp;</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="border-top-width: thin; border-left-width: thin; border-left-color: black;';
            shtml +=  'border-bottom-width: thin; border-bottom-color: black; vertical-align: top; width: 100%;';
            shtml +=  'border-top-color: black; height: 20px; text-align: center; border-right-width: thin;';
            shtml +=  'border-right-color: black">';
            shtml +=  '<table style="width: 100%; height: 20px" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px;text-align:center;">';
            shtml +=  '<span style="font-size: 9.5pt; text-decoration: underline; line-height: 16pt"><strong>DECLARATIONS</strong></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            if (dt.Rows[0].ReceiptDeclarationSub5.toString().length == 0)
            {
                       shtml +=  '<td colspan="3" rowspan="2" style="font-size: 9pt;text-align: justify;line-height: 14pt">'; //line-height: 14pt;
            }else{
                        shtml +=  '<td colspan="3" rowspan="2" style="font-size: 9pt;text-align: justify; line-height: 14pt">';//line-height: 14pt;
            }
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub1 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub2 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub3 + '<br />';   
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub4 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub5 + '<br />';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            //END - FOR OBP 2nd copy
            shtml +=  '</div>';

        return shtml;
    }
    
    this.half_page = function(dt)
    {
          var shtml = "";
          shtml +=  '<br />';
//        shtml +=  '<br />';
//        shtml +=  '<br />';
//        shtml +=  '<br />';
//        shtml +=  '<br />';
//        shtml +=  '<table style="width: 8.5in">';
//        shtml +=  '<tr>';
//        shtml +=  '<td colspan="3" style="border-right: black thin dotted; border-top: black thin dotted; border-left: black thin dotted; border-bottom: black thin dotted">';
//        shtml +=  '<table style="height: 0%">';
//        shtml +=  '<tr>';
//        shtml +=  '<td style="font-size: 10pt; width: 371px; color: black; font-family: arial; height: 22px;';
//        shtml +=  'text-align: center">';
//        shtml +=  '<span style="font-size: 8pt; text-decoration: underline"><span style="font-size: 8.5pt;';
//        shtml +=  'text-decoration: underline"><strong>IMPORTANT REMINDERS TO CLIENT / CUSTOMER</strong></span></span></td>';
//        shtml +=  '<td rowspan="3" style="font-size: 6.5px; vertical-align: top; width: 50%; color: black;';
//        shtml +=  'font-family: arial; height: 101px; text-align: left">';
//        shtml +=  '<table style="font-size: 6.5pt; width: 100%; height: 23px">';
//        shtml +=  '<tr>';
//        shtml +=  '<td colspan="3" >';
//        shtml +=  'To the PNB __________________ , Branch: _____________ Date____________: This is';
//        shtml +=  'to authorize your office to change my (pls. mark check the box/es below as applicable),';
//        shtml +=  '</td>';
//        shtml +=  '</tr>';
//        shtml +=  '<tr>';
//        shtml +=  '<td colspan="3" nowrap>';
//        shtml +=  '<input id="Checkbox7" style="width: 26px; height: 12px" type="checkbox" />Remitter';
//        shtml +=  'Name to _______________________________________________________</td>';
//        shtml +=  '</tr>';
//        shtml +=  '<tr>';
//        shtml +=  '<td colspan="3" nowrap>';
//        shtml +=  '<input id="Checkbox8" style="width: 26px; height: 12px" type="checkbox" />Address';
//        shtml +=  '&nbsp;to _______________________________________ Postal Code: _________</td>';
//        shtml +=  '</tr>';
//        shtml +=  '<tr>';
//        shtml +=  '<td colspan="3" nowrap>';
//        shtml +=  '<input id="Checkbox9" style="width: 26px; height: 12px" type="checkbox" />Telephone';
//        shtml +=  'No to: __________________________ Mobile No to: _________________</td>';
//        shtml +=  '</tr>';
//        shtml +=  '<tr>';
//        shtml +=  '<td colspan="3" nowrap>';
//        shtml +=  '<input id="Checkbox10" style="width: 26px; height: 12px" type="checkbox" />Beneficiary';
//        shtml +=  '&nbsp;Name to: ________________________________Postal Code: _______</td>';
//        shtml +=  '</tr>';
//        shtml +=  '<tr>';
//        shtml +=  '<td colspan="3" nowrap>';
//        shtml +=  '<input id="Checkbox11" style="width: 26px; height: 12px" type="checkbox" />Beneficiary';
//        shtml +=  'Address To: _________________________________________________</td>';
//        shtml +=  '</tr>';
//        shtml +=  '<tr>';
//        shtml +=  '<td colspan="3" nowrap>';
//        shtml +=  '<input id="Checkbox12" style="width: 26px; height: 12px" type="checkbox" />Beneficiary';
//        shtml +=  'Telephone No to: ___________________Mobile No to: ______________</td>';
//        shtml +=  '</tr>';
//        shtml +=  '<tr>';
//        shtml +=  '<td colspan="3" nowrap>';
//        shtml +=  '<input id="Checkbox13" style="width: 26px; height: 12px" type="checkbox" />Others';
//        shtml +=  '(please specify): _________________________________________________</td>';
//        shtml +=  '</tr>';
//        shtml +=  '<tr>';
//        shtml +=  '<td colspan="3">';
//        shtml +=  'To support the request for ammendment, photocopy of the following document/s or';
//        shtml +=  'ID/s is/are provided (*should match the document/s or ID/s required in No.2 of important';
//        shtml +=  'Reminders to Client / Customer):';
//        shtml +=  '<br />';
//        shtml +=  'A) _______________&nbsp; B_________________ C)_________________ D)_______________</td>';
//        shtml +=  '</tr>';
//        shtml +=  '<tr>';
//        shtml +=  '<td colspan="3">';
//        shtml +=  '<table style="width: 100%">';
//        shtml +=  '<tr>';
//        shtml +=  '<td rowspan="3" style="font-size: 8pt;border-right: black thin solid; border-top: black thin solid;';
//        shtml +=  'border-left: black thin solid; border-bottom: black thin solid">';
//        shtml +=  'PNB Use Only:<br />';
//        shtml +=  'Processed By____Date:___</td>';
//        shtml +=  '<td colspan="2" rowspan="3" style="font-size: 8pt;text-align: center">';
//        shtml +=  '<hr style="width: 234px; height: 1px" />';
//        shtml +=  '&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; Remitter Signature Over Printed Name</td>';
//        shtml +=  '</tr>';
//        shtml +=  '<tr>';
//        shtml +=  '</tr>';
//        shtml +=  '<tr>';
//        shtml +=  '</tr>';
//        shtml +=  '</table>';
//        shtml +=  '</td>';
//        shtml +=  '</tr>';
//        shtml +=  '</table>';
//        shtml +=  '</td>';
//        shtml +=  '</tr>';
//        shtml +=  '<tr>';
//        shtml +=  '<td rowspan="2" style="font-size: 10px; vertical-align: top; width: 371px; color: black;';
//        shtml +=  'font-family: arial; height: 113px; text-align: left; line-height: 13pt;">';
//        shtml +=  '<span style="font-size: 8pt">1) Please notify the bank of any change in your name,';
//        shtml +=  'address, telephone number or other details pertaining to your remittance account';
//        shtml +=  '/ record by mailing / submitting the following: a) Photocopy of valid document/s';
//        shtml +=  'or ID/s to support the amendment.';
//        shtml +=  '<br />';
//        shtml +=  'b) Duly accomplished and signed " Authorization and Notice of Amendment" cutout';
//        shtml +=  'found on the opposite bottom right corner of this receipt. 2) Mailing address should';
//        shtml +=  'tally with the clients registered address shown in the valid document/s or ID/s,';
//        shtml +=  'as any irregularity in the registered address may result in non - implementation';
//        shtml +=  'of remittance. The valid document/s or ID/s are the following*:';
//        shtml +=  '<br />';
//        shtml +=  '</span>';
//        shtml +=  '<table style="font-size: 8pt; width: 303px; height: 45px">';
//        shtml +=  '<tr>';
//        shtml +=  '<td style="width: 50%; border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; border-bottom: black thin solid;">';
//        shtml +=  'A) ' + dt.Rows[0].ID1 + '</td>';
//        shtml +=  '<td style="width: 50%; border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; border-bottom: black thin solid;">';
//        shtml +=  'B) ' + dt.Rows[0].ID2 + '</td>';
//        shtml +=  '</tr>';
//        shtml +=  '<tr>';
//        shtml +=  '<td style="width: 50%; border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; border-bottom: black thin solid;">';
//        shtml +=  'C) ' + dt.Rows[0].ID3 + '</td>';
//        shtml +=  '<td style="width: 50%; border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; border-bottom: black thin solid;">';
//        shtml +=  'D) ' + dt.Rows[0].ID4 + '</td>';
//        shtml +=  '</tr>';
//        shtml +=  '</table>';
//        shtml +=  '<span style="font-size: 8pt">3) Always provide the bank with clear and proper instructions';
//        shtml +=  'as to beneficiary details and remittance amount to facilitate processing of remittance.';
//        shtml +=  '</span>';
//        shtml +=  '</td>';
//        shtml +=  '</tr>';
//        shtml +=  '<tr style="font-size: 10pt">';
//        shtml +=  '</tr>';
//        shtml +=  '</table>';
//        shtml +=  '</td>';
//        shtml +=  '</tr>';
//        shtml +=  '</table>';
          return shtml;
    }
    this.Receipt_body_halfpage = function(dt)
    {
    var vd = window.location.href.split('/')[3];
    
    var shtml = "";
            shtml +=  '<br />';
            shtml +=  '<br />';
            shtml +=  '<div style="width: 970px; margin: 0px;">';
            shtml +=  '<table style="width: 100%; font-size: 9.5pt; font-family: arial;" cellspacing="0" cellpadding="0">';
            //shtml +=  '<table style="width: 8.5in; font-size: 7pt; font-family: arial;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="width: 0in">';
            shtml +=  '<table style="width: 100%;font-size: 9pt" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 60px; height: 15px">';
            shtml +=  '<img src="../../../'+ vd +'/images/pnb_black_logo.JPG" style="width: 58px; height: 68px" /></td>';
            shtml +=  '<td style="vertical-align: top; width: 520px; height: 15px">';
            shtml +=  '<strong>' + dt.Rows[0].Header1 + '</strong><br />';
            shtml +=  '' + dt.Rows[0].Header2 + '<br />';
            shtml +=  '<strong>' + dt.Rows[0].NameofBranch + '</strong><br />';
            shtml +=  '' + dt.Rows[0].AdressofBranch + '<br />';
            shtml +=  '' + dt.Rows[0].BranchofContact + '<br />';
            shtml +=  '' + dt.Rows[0].BranchofEmail + '<br />';
            shtml +=  '' + dt.Rows[0].corp_address + '<br />';
            shtml +=  '' + dt.Rows[0].corp_telno + '</td>';
            shtml +=  '<td style="vertical-align: top; height: 15px">';
            shtml +=  'Transaction Date: ' + HTMLRep.formatDate(dt.Rows[0].TranxDate,"/") + '<br />';
	        if (dt.Rows[0].ClearingDate == '1900')
	        {
	        shtml +=  'Clearing Date: <br />';
	        }else{
	        shtml +=  'Clearing Date: '+ dt.Rows[0].ClearingDate +'<br />';
	        }
            shtml +=  'Time: '+ HTMLRep.formatTime(dt.Rows[0].TranxDate) +'<br />';
            shtml +=  'Loyalty Points:';
            shtml +=  '<br />';
            shtml +=  '' + dt.Rows[0].TranStatus + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 100%; font-size: 9.5pt; font-family: arial;';
            shtml +=  'height: 0.23in; border-top-width: thin; border-left-width: thin; border-left-color: black; border-bottom-width: thin; border-bottom-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="border-right: black thin solid; border-top: black thin solid;';
            shtml +=  'border-left: black thin solid; border-bottom: black thin solid">';
            shtml +=  '<table style="font-size: 9.5pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 244px; height: 14px">';
            shtml +=  '<strong>Remitter ID No.: </strong>' + dt.Rows[0].RemitterID + '</td>';
            shtml +=  '<td style="width: 285px; height: 14px">';
            shtml +=  '<strong>Official Receipt No.:</strong> ' + dt.Rows[0].ReceiptNo + '</td>';
            shtml +=  '<td style="width: 257px; height: 14px">';
            shtml +=  '<strong>Reference No.:</strong> ' + dt.Rows[0].ReferenceNo + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';    
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td style="border-right: black thin solid; border-top: black thin solid; vertical-align: top;';
            shtml +=  'border-left: black thin solid; width: 50%; border-bottom: black thin solid; height: 20px">';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 0px; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="vertical-align: top; width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 20px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 9.5pt; text-decoration: underline">REMITTER INFO</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="2">';
            shtml +=  '<span><span style="font-size: 9pt"><strong>&nbsp;Name: </strong>' + dt.Rows[0].RemitterName + '<br />';
            shtml +=  '</span></span>';
            shtml +=  '<table style="font-size: 9pt; width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="width: 61px; vertical-align: top;">';
            shtml +=  '<strong style="width: 30px">Address:<br />';
            shtml +=  '</strong></td>';
            shtml +=  '<td colspan="2" rowspan="3" style="height: 40px; vertical-align: top;">';
            shtml +=  '' + dt.Rows[0].RemitterAddress + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<span><span style="font-size: 9pt; width: 100%"><strong>Telephone Number:</strong>';
            shtml +=  '' + dt.Rows[0].RemitterTelNo + '</span></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="vertical-align: top; width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 20px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%; height: 20px" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 100% ; height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 9.5pt; text-decoration: underline">BENEFICIARY INFO</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="2">';
            shtml +=  '<span><span style="font-size: 9pt"><strong>&nbsp;Name:</strong> ' + dt.Rows[0].BenefName + '<br />';
            shtml +=  '</span></span>';
            shtml +=  '<table style="font-size: 9pt; width: 100%; vertical-align: top;" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td  style="width: 60px; vertical-align: top;"><strong style="width: 30px">Address:<br />';
            shtml +=  '</strong>';
            shtml +=  '</td>';
            shtml +=  '<td rowspan="3" style="width: 90%; height: 40px; vertical-align: top;">';
            shtml +=  '' + dt.Rows[0].BenefAddress + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<span><span style="font-size: 9pt"><strong>Telephone Number:</strong> ' + dt.Rows[0].BenefTelNo + '</span></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="vertical-align: top;  width: 100%; height: 20px; border-bottom: black thin solid;">';
            shtml +=  '<table style="font-size: 9pt; width: 100%; height: 20px" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 9.5pt; text-decoration: underline">TRANSACTION DETAILS</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 9pt">';
            shtml +=  '<td colspan="3" rowspan="2" style="font-size: 8.5pt">';
            shtml +=  '<table style="font-size: 9pt; width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Beneficiarys Account No. : </strong>' + dt.Rows[0].BenefAcctNo + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Thru PNB Branch: </strong>' + dt.Rows[0].ThruPNBBranch + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Other Banks Name / Branch: </strong>' + dt.Rows[0].OtherBankName + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Trans Type/Products: </strong>' + dt.Rows[0].TransType + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Source of Funds: </strong>' + dt.Rows[0].SourceFunds + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Member ID:</strong>' + dt.Rows[0].MemberID + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Purpose of Remittance: </strong>' + dt.Rows[0].PurposeRemittance + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Remittance Transaction Source: </strong>' + dt.Rows[0].RemittanceTranxSource + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 9pt">';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 9pt">';
            shtml +=  '<td rowspan="1" style="border-top-width: thin; border-left-width: thin; border-left-color: black;';
            shtml +=  'border-bottom-width: thin; border-bottom-color: black; vertical-align: top; width: 100%;';
            shtml +=  'border-top-color: black; height: 20px; border-right-width: thin; border-right-color: black">';
            shtml +=  '<table style="width: 100%; height: 20px;font-size: 8pt;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px">';
            shtml +=  '<p style="text-align: center">';
            shtml +=  '<span style="font-size: 9.5pt; text-decoration: underline"><strong>CLIENT/CUSTOMER UNDERTAKING</strong></span></p>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="2" style="text-align: justify;line-height: 16pt">';// line-height: 14pt;
            shtml +=  '<table style="width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="3" style="text-align: justify;font-size: 9pt;line-height: 15pt">'; //line-height: 14pt;                      
            shtml +=  'I certify that all the information contained herein is true and correct, and that';
            shtml +=  'I am amenable to the Terms and Conditions (if any) governing this transaction.';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<br />';
            shtml +=  '<hr style="width: 378px; height: 1px; text-align: center"/>';
            shtml +=  '<div style="width: 100%; height: 25px; text-align: center">';
            shtml +=  'Remitter&#039;s Signature</div>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            // Edited by CHR from colspan=2 to colspan=1
            shtml +=  '<td style="border-right: black thin solid; border-top: black thin solid;';
            shtml +=  'vertical-align: top; border-left: black thin solid; width: 50%; border-bottom: black thin solid;';
            shtml +=  'height: 20px">';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 100%; height: 90px">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 86px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="font-size: 9pt; width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 18px; text-align: center">';
            shtml +=  '<span style="font-size: 9.5pt; text-decoration: underline"><strong>REMITTANCE DETAILS</strong></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td style="text-align: left; width: 245px; height: 15px;">';
	        shtml +=  '    <strong>';
	        shtml +=  'Net Amount Due to Beneficiary:</strong></td>';
	        shtml +=  '<td style="width: 58px; height: 15px">';
	        if (dt.Rows[0].NetAmountDueToBenefCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].NetAmountDueToBenefCurrCode + '</td>';
	        }
	        shtml +=  '<td style="height: 15px; text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].NetAmountDueToBenefAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Exchange Rate Used:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].ExchangeRateUsedCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].ExchangeRateUsedCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].ExchangeRateUsedAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Remittance Amount:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].RemittanceAmountCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].RemittanceAmountCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].RemittanceAmountAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Cable:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].CableCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].CableCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].CableAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Remittance Fee:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].RemittanceFeeCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].RemittanceFeeCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].RemittanceFeeAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Other Charges:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].OtherChargesCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].OtherChargesCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].OtherChargesAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="width: 221px">';
	        shtml +=  '<strong>';
	        shtml +=  'Total amount received from customer:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].TotalAmtRcvdFromCustCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].TotalAmtRcvdFromCustCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].TotalAmtRcvdFromCustAmt,2) + '</td>';
	        shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 70px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;font-size: 9pt">';
	        shtml +=  '' + dt.Rows[0].Check + '<br />';
	        shtml +=  '' + dt.Rows[0].Cash + '<br />';
	        shtml +=  '' + dt.Rows[0].SpecialMessage + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="font-size:9pt;width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 40px; text-align: center; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 177px;font-size:9pt">';
            shtml +=  '<strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Payment ';
            shtml +=  '<br />';
            shtml +=  'Received / Processed By:</strong></td>';
            shtml +=  '<td style="font-size:9pt;">';
            shtml +=  '' + dt.Rows[0].ProcessedBy + '<br />';
            shtml +=  '<hr style="width: 200px; height: 2px;background-color:black; text-align: left;" />';/////
            shtml +=  '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Authorized Personnel&nbsp;</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="border-top-width: thin; border-left-width: thin; border-left-color: black;';
            shtml +=  'border-bottom-width: thin; border-bottom-color: black; vertical-align: top; width: 100%;';
            shtml +=  'border-top-color: black; height: 20px; text-align: center; border-right-width: thin;';
            shtml +=  'border-right-color: black">';
            shtml +=  '<table style="width: 100%; height: 20px" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px;text-align:center;">';
            shtml +=  '<span style="font-size: 9.5pt; text-decoration: underline; line-height: 16pt"><strong>DECLARATIONS</strong></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            if (dt.Rows[0].ReceiptDeclarationSub5.toString().length == 0)
            {
                       shtml +=  '<td colspan="3" rowspan="2" style="font-size: 9pt;text-align: justify;line-height: 12pt">'; //line-height: 14pt;
            }else{
                        shtml +=  '<td colspan="3" rowspan="2" style="font-size: 9pt;text-align: justify; line-height: 12pt">';//line-height: 14pt;
            }
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub1 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub2 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub3 + '<br />';   
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub4 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub5 + '<br />';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            //shtml +=  '<tr>';
            //shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            
            //START - OBP RECEIPT 2nd copy 
            shtml +=  '<br />';
            shtml +=  '<br />';
            shtml +=  '<br />';
            shtml +=  '<br />';
            //shtml +=  '<br />';
            shtml +=  '<table style="width: 100%; font-size: 9.5pt; font-family: arial;" cellspacing="0" cellpadding="0">';
            //shtml +=  '<table style="width: 8.5in; font-size: 7pt; font-family: arial;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="width: 0in">';
            shtml +=  '<table style="width: 100%;font-size: 9pt" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 60px; height: 15px">';
            shtml +=  '<img src="../../../'+ vd +'/images/pnb_black_logo.JPG" style="width: 58px; height: 68px" /></td>';
            shtml +=  '<td style="vertical-align: top; width: 520px; height: 15px">';
            shtml +=  '<strong>' + dt.Rows[0].Header1 + '</strong><br />';
            shtml +=  '' + dt.Rows[0].Header2 + '<br />';
            shtml +=  '<strong>' + dt.Rows[0].NameofBranch + '</strong><br />';
            shtml +=  '' + dt.Rows[0].AdressofBranch + '<br />';
            shtml +=  '' + dt.Rows[0].BranchofContact + '<br />';
            shtml +=  '' + dt.Rows[0].BranchofEmail + '<br />';
            shtml +=  '' + dt.Rows[0].corp_address + '<br />';
            shtml +=  '' + dt.Rows[0].corp_telno + '</td>';
            shtml +=  '<td style="vertical-align: top; height: 15px">';
            shtml +=  'Transaction Date: ' + HTMLRep.formatDate(dt.Rows[0].TranxDate,"/") + '<br />';
	        if (dt.Rows[0].ClearingDate == '1900')
	        {
	        shtml +=  'Clearing Date: <br />';
	        }else{
	        shtml +=  'Clearing Date: '+ dt.Rows[0].ClearingDate +'<br />';
	        }
            shtml +=  'Time: '+ HTMLRep.formatTime(dt.Rows[0].TranxDate) +'<br />';
            shtml +=  'Loyalty Points:';
            shtml +=  '<br />';
            shtml +=  '' + dt.Rows[0].TranStatus + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 100%; font-size: 9.5pt; font-family: arial;';
            shtml +=  'height: 0.23in; border-top-width: thin; border-left-width: thin; border-left-color: black; border-bottom-width: thin; border-bottom-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="border-right: black thin solid; border-top: black thin solid;';
            shtml +=  'border-left: black thin solid; border-bottom: black thin solid">';
            shtml +=  '<table style="font-size: 9.5pt; width: 100%">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 244px; height: 14px">';
            shtml +=  '<strong>Remitter ID No.: </strong>' + dt.Rows[0].RemitterID + '</td>';
            shtml +=  '<td style="width: 285px; height: 14px">';
            shtml +=  '<strong>Official Receipt No.:</strong> ' + dt.Rows[0].ReceiptNo + '</td>';
            shtml +=  '<td style="width: 257px; height: 14px">';
            shtml +=  '<strong>Reference No.:</strong> ' + dt.Rows[0].ReferenceNo + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';    
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td style="border-right: black thin solid; border-top: black thin solid; vertical-align: top;';
            shtml +=  'border-left: black thin solid; width: 50%; border-bottom: black thin solid; height: 20px">';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 0px; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="vertical-align: top; width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 20px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%; height: 20px">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 9.5pt; text-decoration: underline">REMITTER INFO</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="2">';
            shtml +=  '<span><span style="font-size: 9pt"><strong>&nbsp;Name: </strong>' + dt.Rows[0].RemitterName + '<br />';
            shtml +=  '</span></span>';
            shtml +=  '<table style="font-size: 9pt; width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="width: 61px; vertical-align: top;">';
            shtml +=  '<strong style="width: 30px">Address:<br />';
            shtml +=  '</strong></td>';
            shtml +=  '<td colspan="2" rowspan="3" style="height: 40px; vertical-align: top;">';
            shtml +=  '' + dt.Rows[0].RemitterAddress + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<span><span style="font-size: 9pt; width: 100%"><strong>Telephone Number:</strong>';
            shtml +=  '' + dt.Rows[0].RemitterTelNo + '</span></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="vertical-align: top; width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 20px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%; height: 20px" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 100% ; height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 9.5pt; text-decoration: underline">BENEFICIARY INFO</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="2">';
            shtml +=  '<span><span style="font-size: 9pt"><strong>&nbsp;Name:</strong> ' + dt.Rows[0].BenefName + '<br />';
            shtml +=  '</span></span>';
            shtml +=  '<table style="font-size: 9pt; width: 100%; vertical-align: top;" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td  style="width: 60px; vertical-align: top;"><strong style="width: 30px">Address:<br />';
            shtml +=  '</strong>';
            shtml +=  '</td>';
            shtml +=  '<td rowspan="3" style="width: 90%; height: 40px; vertical-align: top;">';
            shtml +=  '' + dt.Rows[0].BenefAddress + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<span><span style="font-size: 9pt"><strong>Telephone Number:</strong> ' + dt.Rows[0].BenefTelNo + '</span></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="vertical-align: top;  width: 100%; height: 20px; border-bottom: black thin solid;">';
            shtml +=  '<table style="font-size: 9pt; width: 100%; height: 20px" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px; text-align: center">';
            shtml +=  '<strong><span style="font-size: 9.5pt; text-decoration: underline">TRANSACTION DETAILS</span></strong></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 9pt">';
            shtml +=  '<td colspan="3" rowspan="2" style="font-size: 8.5pt">';
            shtml +=  '<table style="font-size: 9pt; width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Beneficiarys Account No. : </strong>' + dt.Rows[0].BenefAcctNo + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Thru PNB Branch: </strong>' + dt.Rows[0].ThruPNBBranch + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Other Banks Name / Branch: </strong>' + dt.Rows[0].OtherBankName + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Trans Type/Products: </strong>' + dt.Rows[0].TransType + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Source of Funds: </strong>' + dt.Rows[0].SourceFunds + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Member ID:</strong>' + dt.Rows[0].MemberID + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Purpose of Remittance: </strong>' + dt.Rows[0].PurposeRemittance + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td>';
            shtml +=  '<strong>Remittance Transaction Source: </strong>' + dt.Rows[0].RemittanceTranxSource + '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 9pt">';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr style="font-size: 9pt">';
            shtml +=  '<td rowspan="1" style="border-top-width: thin; border-left-width: thin; border-left-color: black;';
            shtml +=  'border-bottom-width: thin; border-bottom-color: black; vertical-align: top; width: 100%;';
            shtml +=  'border-top-color: black; height: 20px; border-right-width: thin; border-right-color: black">';
            shtml +=  '<table style="width: 100%; height: 20px;font-size: 8pt;">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px">';
            shtml +=  '<p style="text-align: center">';
            shtml +=  '<span style="font-size: 9.5pt; text-decoration: underline"><strong>CLIENT/CUSTOMER UNDERTAKING</strong></span></p>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="2" style="text-align: justify;line-height: 16pt">';// line-height: 14pt;
            shtml +=  '<table style="width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" rowspan="3" style="text-align: justify;font-size: 9pt;line-height: 15pt">'; //line-height: 14pt;                      
            shtml +=  'I certify that all the information contained herein is true and correct, and that';
            shtml +=  'I am amenable to the Terms and Conditions (if any) governing this transaction.';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '<br />';
            shtml +=  '<hr style="width: 378px; height: 1px; text-align: center"/>';
            shtml +=  '<div style="width: 100%; height: 25px; text-align: center">';
            shtml +=  'Remitter&#039;s Signature</div>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            // Edited by CHR from colspan=2 to colspan=1
            shtml +=  '<td style="border-right: black thin solid; border-top: black thin solid;';
            shtml +=  'vertical-align: top; border-left: black thin solid; width: 50%; border-bottom: black thin solid;';
            shtml +=  'height: 20px">';
            shtml +=  '<table cellpadding="0" cellspacing="0" style="width: 100%; height: 90px">';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="3" style="width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 86px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="font-size: 9pt; width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 18px; text-align: center">';
            shtml +=  '<span style="font-size: 9.5pt; text-decoration: underline"><strong>REMITTANCE DETAILS</strong></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td style="text-align: left; width: 245px; height: 15px;">';
	        shtml +=  '    <strong>';
	        shtml +=  'Net Amount Due to Beneficiary:</strong></td>';
	        shtml +=  '<td style="width: 58px; height: 15px">';
	        if (dt.Rows[0].NetAmountDueToBenefCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].NetAmountDueToBenefCurrCode + '</td>';
	        }
	        shtml +=  '<td style="height: 15px; text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].NetAmountDueToBenefAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Exchange Rate Used:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].ExchangeRateUsedCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].ExchangeRateUsedCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].ExchangeRateUsedAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Remittance Amount:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].RemittanceAmountCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].RemittanceAmountCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].RemittanceAmountAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Cable:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].CableCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].CableCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].CableAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Remittance Fee:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].RemittanceFeeCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].RemittanceFeeCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].RemittanceFeeAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="text-align: left; width: 221px;">';
	        shtml +=  '<strong>';
	        shtml +=  'Other Charges:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].OtherChargesCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].OtherChargesCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].OtherChargesAmt,2) + '</td>';
	        shtml +=  '</tr>';
	        shtml +=  '<tr>';
	        shtml +=  '<td style="width: 221px">';
	        shtml +=  '<strong>';
	        shtml +=  'Total amount received from customer:</strong></td>';
	        shtml +=  '<td style="width: 58px">';
	        if (dt.Rows[0].TotalAmtRcvdFromCustCurrCode == null){
	        shtml +=  '</td>';
	        }else{
	        shtml +=  '' + dt.Rows[0].TotalAmtRcvdFromCustCurrCode + '</td>';
	        }
	        shtml +=  '<td style="text-align: right">';
	        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].TotalAmtRcvdFromCustAmt,2) + '</td>';
	        shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 70px; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;font-size: 9pt">';
	        shtml +=  '' + dt.Rows[0].Check + '<br />';
	        shtml +=  '' + dt.Rows[0].Cash + '<br />';
	        shtml +=  '' + dt.Rows[0].SpecialMessage + '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="font-size:9pt;width: 100%; border-bottom: black thin solid;';
            shtml +=  'height: 40px; text-align: center; border-top-width: thin; border-left-width: thin; border-left-color: black; border-top-color: black; border-right-width: thin; border-right-color: black;">';
            shtml +=  '<table style="width: 100%" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td style="width: 177px;font-size:9pt">';
            shtml +=  '<strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Payment ';
            shtml +=  '<br />';
            shtml +=  'Received / Processed By:</strong></td>';
            shtml +=  '<td style="font-size:9pt;">';
            shtml +=  '' + dt.Rows[0].ProcessedBy + '<br />';
            shtml +=  '<hr style="width: 200px; height: 2px;background-color:black; text-align: left;" />';/////
            shtml +=  '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Authorized Personnel&nbsp;</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '<td rowspan="1" style="border-top-width: thin; border-left-width: thin; border-left-color: black;';
            shtml +=  'border-bottom-width: thin; border-bottom-color: black; vertical-align: top; width: 100%;';
            shtml +=  'border-top-color: black; height: 20px; text-align: center; border-right-width: thin;';
            shtml +=  'border-right-color: black">';
            shtml +=  '<table style="width: 100%; height: 20px" cellspacing="0" cellpadding="0">';
            shtml +=  '<tr>';
            shtml +=  '<td colspan="3" style="height: 20px;text-align:center;">';
            shtml +=  '<span style="font-size: 9.5pt; text-decoration: underline; line-height: 16pt"><strong>DECLARATIONS</strong></span></td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            if (dt.Rows[0].ReceiptDeclarationSub5.toString().length == 0)
            {
                       shtml +=  '<td colspan="3" rowspan="2" style="font-size: 9pt;text-align: justify;line-height: 12pt">'; //line-height: 14pt;
            }else{
                        shtml +=  '<td colspan="3" rowspan="2" style="font-size: 9pt;text-align: justify; line-height: 12pt">';//line-height: 14pt;
            }
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub1 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub2 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub3 + '<br />';   
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub4 + '<br />';
            shtml +=  '' + dt.Rows[0].ReceiptDeclarationSub5 + '<br />';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            //shtml +=  '<tr>';
            //shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            shtml +=  '</td>';
            shtml +=  '</tr>';
            shtml +=  '<tr>';
            shtml +=  '</tr>';
            shtml +=  '</table>';
            //END - OBP 2nd copy
            
            
            shtml +=  '</div>';

            return shtml;
    }
}
/*END OBP RECEIPT -- ONE PAGE*/


