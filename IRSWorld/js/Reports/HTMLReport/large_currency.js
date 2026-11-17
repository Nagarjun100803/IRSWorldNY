
//var OTCRep = new OTCReport();

var reportholder = new HolderReport();
 
function HolderReport()
{
    this.load_report = function()
    {
        var qstr = this.get_hidden_querystring("hidQueryString");
        
        if (qstr.length > 0)
        {
            if (qstr.toLowerCase().indexOf("mergeparam") > -1 && qstr.toLowerCase().indexOf("sp_rpt_largecurrencyreport") > -1)
            {
                var orps = new HTMLLargeCurrReport(); //change this to new report name
                orps.evaluate_largecurr_query(qstr); 
                return;
            }      
            if (qstr.toLowerCase().indexOf("mergeparam") > -1 && qstr.toLowerCase().indexOf("sp_rpt_accountingreconciliation") > -1)
            {
                var orps = new HTMLAcctReconSummReport(); //change this to new report name
                orps.evaluate_recon_query(qstr);
                return;
            }   
        }
    }    
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




/* START Account Reconcillation Summary report */
function HTMLAcctReconSummReport()
{
    var cls = this;
    var _branchcode;    
    var _seqid;
	var _referenceno;
    
    this.evaluate_recon_query = function(query)
    {
        var squery = unescape(query.toString());
       /*
          @sDate DATETIME,  
            @BranchCode VARCHAR (4) 
       */
        if (squery.toLowerCase().indexOf("mergeparam") > -1)
        {
         
             this.get_data(        
                                    HTMLRep.parse_merge_param_query(squery,"T@sDate"),
                                    HTMLRep.parse_merge_param_query(squery,"T@BranchCode")
									)
            return;
        }
    }
    this.get_data = function(sdate,branchcode)
    {
        //change to sp parameter
        var params; 
        var sp_name = "sp_rpt_accountingreconciliation";
        
                _branchcode =   "@BranchCode="  + branchcode;    
                _sdate      =   "@sDate="     + sdate;
	
		params  = _sdate + "^" + _branchcode ; 
		
	    IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params,this.get_data_callback);
    }
    
    this.get_data_callback = function(result)
    {
        try
        {
            var ds = result.value;
            cls.display_report(ds);
        }
        catch(e)
        {
            alert("ERROR WHILE GETTING Receipt DATA");
        }
    }
    
    this.display_report = function(ds)
    {
        var shtml = "";        
        shtml +=  HTMLRep.print_html_button();
        shtml +=  this._body(ds.Tables[0]);
        document.getElementById("bodydiv").innerHTML = shtml
    }
    this._body = function(dt)
    {
	
        var shtml = "";
	    shtml +=  '<table style="width: 8in; height: 11in">';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" rowspan="3" style="vertical-align: top; text-align: center">';
        shtml +=  '<table style="width: 304px">';
        shtml +=  '<tr>';
        shtml +=  '<td style="vertical-align: top; width: 48px; height: 21px">';
        shtml +=  '<img src="pnbiconblack.JPG" style="height: 69px" /></td>';
        shtml +=  '<td style="vertical-align: top; width: 295px; height: 21px; text-align: center">';
        shtml +=  '<table style="font-size: 9pt; width: 100%; font-family: arial">';
        shtml +=  '<tr>';
        shtml +=  '<td>';
        shtml +=  '<span style="font-size: 8pt; font-family: Arial Black"><strong>PHILIPPINE NATIONAL BANK</strong></span></td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td>';
        shtml +=  '<span style="font-size: 8pt">remitting branch here</span></td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td>';
        shtml +=  '<span style="font-size: 7pt">INTERGRATED REMITTANCE SYSTEM</span></td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td>';
        shtml +=  '<span style="font-size: 8pt">Account Reconcillation Summary</span></td>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="2" style="vertical-align: top; text-align: center">';
        shtml +=  '<table style="font-size: 8pt; width: 100%; font-family: arial">';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="4" rowspan="2">';
        shtml +=  '' + dt.Rows[0].StartDate + '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '<br />';
        shtml +=  '<br />';
        shtml +=  '<table style="font-weight: bold; font-size: 8pt; width: 100%; color: black; font-family: arial">';
        shtml +=  '<tr>';
        shtml +=  '<td rowspan="1" style="vertical-align: top; width: 40%; text-align: right">';
        shtml +=  '</td>';
        shtml +=  '<td colspan="2" rowspan="1" style="vertical-align: top; text-align: left">';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td rowspan="3" style="vertical-align: top; width: 40%; text-align: right">';
        shtml +=  '<span style="font-size: 8pt">';
        shtml +=  'REMITTING BRANCH:</span></td>';
        shtml +=  '<td colspan="2" rowspan="3" style="vertical-align: top; text-align: left">';
        shtml +=  '<table style="border-right: black thin solid; border-top: black thin solid; border-left: black thin solid;';
        shtml +=  'width: 291px; border-bottom: black thin solid; font-weight: normal; font-size: 8pt; color: black; font-family: arial;">';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" rowspan="3" style="height: 17px">';
        shtml +=  '' + dt.Rows[0].RemittingBranch + '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
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
        shtml +=  '<td rowspan="1" style="vertical-align: top; width: 40%; height: 18px; text-align: right">';
        shtml +=  '</td>';
        shtml +=  '<td colspan="2" rowspan="1" style="vertical-align: top; height: 18px; text-align: left">';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td rowspan="1" style="vertical-align: top; width: 40%; text-align: right">';
        shtml +=  '<span style="font-size: 8pt">TOTAL ITEMS RECEIVED</span></td>';
        shtml +=  '<td colspan="2" rowspan="1" style="vertical-align: top; text-align: left"><table style="border-right: black thin solid; border-top: black thin solid; border-left: black thin solid;';
        shtml +=  'width: 291px; border-bottom: black thin solid; font-weight: normal; font-size: 8pt; color: black; font-family: arial;">';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" rowspan="3" style="height: 17px">';
        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].TOTALITEMSRECEIVED,2) + '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td rowspan="1" style="vertical-align: top; width: 40%; text-align: right">';
        shtml +=  '</td>';
        shtml +=  '<td colspan="2" rowspan="1" style="vertical-align: top; text-align: left">';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td rowspan="1" style="vertical-align: top; width: 40%; height: 45px; text-align: right">';
        shtml +=  '<span style="font-size: 8pt">TOTAL</span> AMOUNT RECEIVED</td>';
        shtml +=  '<td colspan="2" rowspan="1" style="vertical-align: top; height: 45px; text-align: left"><table style="border-right: black thin solid; border-top: black thin solid; border-left: black thin solid;';
        shtml +=  'width: 291px; border-bottom: black thin solid; font-weight: normal; font-size: 8pt; color: black; font-family: arial;">';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" rowspan="3" style="height: 17px">';
        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].TOTALAMOUNTRECEIVED,2) + '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td rowspan="1" style="vertical-align: top; width: 40%; text-align: right">';
        shtml +=  '</td>';
        shtml +=  '<td colspan="2" rowspan="1" style="vertical-align: top; text-align: left">';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td rowspan="1" style="vertical-align: top; width: 40%; text-align: right">';
        shtml +=  'PESO ITEMS SENT</td>';
        shtml +=  '<td colspan="2" rowspan="1" style="vertical-align: top; text-align: left"><table style="border-right: black thin solid; border-top: black thin solid; border-left: black thin solid;';
        shtml +=  'width: 291px; border-bottom: black thin solid; font-weight: normal; font-size: 8pt; color: black; font-family: arial;">';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" rowspan="3" style="height: 17px">';
        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].PESOITEMSSENT,2) + '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td rowspan="1" style="vertical-align: top; width: 40%; text-align: right">';
        shtml +=  '</td>';
        shtml +=  '<td colspan="2" rowspan="1" style="vertical-align: top; text-align: left">';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td rowspan="1" style="vertical-align: top; width: 40%; text-align: right">';
        shtml +=  'PESO AMOUNT SENT</td>';
        shtml +=  '<td colspan="2" rowspan="1" style="vertical-align: top; text-align: left"><table style="border-right: black thin solid; border-top: black thin solid; border-left: black thin solid;';
        shtml +=  'width: 291px; border-bottom: black thin solid; font-weight: normal; font-size: 8pt; color: black; font-family: arial;">';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" rowspan="3" style="height: 17px">';
        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].PESOAMOUNTSENT,2) + '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td rowspan="1" style="vertical-align: top; width: 40%; text-align: right">';
        shtml +=  '</td>';
        shtml +=  '<td colspan="2" rowspan="1" style="vertical-align: top; text-align: left">';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td rowspan="1" style="vertical-align: top; width: 40%; text-align: right">';
        shtml +=  'COMMISSION ON PESO SENT</td>';
        shtml +=  '<td colspan="2" rowspan="1" style="vertical-align: top; text-align: left"><table style="border-right: black thin solid; border-top: black thin solid; border-left: black thin solid;';
        shtml +=  'width: 291px; border-bottom: black thin solid; font-weight: normal; font-size: 8pt; color: black; font-family: arial;">';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" rowspan="3" style="height: 17px">';
        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].COMMISSIONONPESOSENT,2) + '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td rowspan="1" style="vertical-align: top; width: 40%; text-align: right">';
        shtml +=  '</td>';
        shtml +=  '<td colspan="2" rowspan="1" style="vertical-align: top; text-align: left">';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td rowspan="1" style="vertical-align: top; width: 40%; text-align: right">';
        shtml +=  'PESO NET AMOUNT&nbsp;</td>';
        shtml +=  '<td colspan="2" rowspan="1" style="vertical-align: top; text-align: left"><table style="border-right: black thin solid; border-top: black thin solid; border-left: black thin solid;';
        shtml +=  'width: 291px; border-bottom: black thin solid; font-weight: normal; font-size: 8pt; color: black; font-family: arial;">';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" rowspan="3" style="height: 17px">';
        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].PESONETAMOUNT,2) + '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td rowspan="1" style="vertical-align: top; width: 40%; text-align: right">';
        shtml +=  '</td>';
        shtml +=  '<td colspan="2" rowspan="1" style="vertical-align: top; text-align: left">';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td rowspan="1" style="vertical-align: top; width: 40%; text-align: right">';
        shtml +=  'DOLLAR ITEMS SENT</td>';
        shtml +=  '<td colspan="2" rowspan="1" style="vertical-align: top; text-align: left"><table style="border-right: black thin solid; border-top: black thin solid; border-left: black thin solid;';
        shtml +=  'width: 291px; border-bottom: black thin solid; font-weight: normal; font-size: 8pt; color: black; font-family: arial;">';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" rowspan="3" style="height: 17px">';
        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].DOLLARITEMSSENT,2) + '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td rowspan="1" style="vertical-align: top; width: 40%; text-align: right">';
        shtml +=  '</td>';
        shtml +=  '<td colspan="2" rowspan="1" style="vertical-align: top; text-align: left">';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td rowspan="1" style="vertical-align: top; width: 40%; text-align: right">';
        shtml +=  'COMMISSION OF DOLLAR</td>';
        shtml +=  '<td colspan="2" rowspan="1" style="vertical-align: top; text-align: left"><table style="border-right: black thin solid; border-top: black thin solid; border-left: black thin solid;';
        shtml +=  'width: 291px; border-bottom: black thin solid; font-weight: normal; font-size: 8pt; color: black; font-family: arial;">';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" rowspan="3" style="height: 17px">';
        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].COMMISSIONOFDOLLAR,2) + '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td rowspan="1" style="vertical-align: top; width: 40%; text-align: right">';
        shtml +=  '</td>';
        shtml +=  '<td colspan="2" rowspan="1" style="vertical-align: top; text-align: left">';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td rowspan="1" style="vertical-align: top; width: 40%; text-align: right">';
        shtml +=  'DOLLAR NET AMOUNT</td>';
        shtml +=  '<td colspan="2" rowspan="1" style="vertical-align: top; text-align: left"><table style="border-right: black thin solid; border-top: black thin solid; border-left: black thin solid;';
        shtml +=  'width: 291px; border-bottom: black thin solid; font-weight: normal; font-size: 8pt; color: black; font-family: arial;">';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" rowspan="3" style="height: 17px">';
        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].DOLLARNETAMOUNT,2) + '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td rowspan="1" style="vertical-align: top; width: 40%; text-align: right">';
        shtml +=  '</td>';
        shtml +=  '<td colspan="2" rowspan="1" style="vertical-align: top; text-align: left">';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td rowspan="1" style="vertical-align: top; width: 40%; text-align: right">';
        shtml +=  'TOTAL ITEMS CLEARED</td>';
        shtml +=  '<td colspan="2" rowspan="1" style="vertical-align: top; text-align: left"><table style="border-right: black thin solid; border-top: black thin solid; border-left: black thin solid;';
        shtml +=  'width: 291px; border-bottom: black thin solid; font-weight: normal; font-size: 8pt; color: black; font-family: arial;">';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" rowspan="3" style="height: 17px">';
        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].TOTALITEMSCLEARED,2) + '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td rowspan="1" style="vertical-align: top; width: 40%; text-align: right">';
        shtml +=  '</td>';
        shtml +=  '<td colspan="2" rowspan="1" style="vertical-align: top; text-align: left">';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td rowspan="1" style="vertical-align: top; width: 40%; text-align: right">';
        shtml +=  'TOTAL AMOUNT CLEARED</td>';
        shtml +=  '<td colspan="2" rowspan="1" style="vertical-align: top; text-align: left"><table style="border-right: black thin solid; border-top: black thin solid; border-left: black thin solid;';
        shtml +=  'width: 291px; border-bottom: black thin solid; font-weight: normal; font-size: 8pt; color: black; font-family: arial;">';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" rowspan="3" style="height: 17px">';
        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].TOTALAMOUNTCLEAR,2) + '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td rowspan="1" style="vertical-align: top; width: 40%; text-align: right">';
        shtml +=  '</td>';
        shtml +=  '<td colspan="2" rowspan="1" style="vertical-align: top; text-align: left">';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td rowspan="1" style="vertical-align: top; width: 40%; text-align: right">';
        shtml +=  'TOTAL ITEMS HELD</td>';
        shtml +=  '<td colspan="2" rowspan="1" style="vertical-align: top; text-align: left"><table style="border-right: black thin solid; border-top: black thin solid; border-left: black thin solid;';
        shtml +=  'width: 291px; border-bottom: black thin solid; font-weight: normal; font-size: 8pt; color: black; font-family: arial;">';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" rowspan="3" style="height: 17px">';
        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].TOTALITEMSHELD,2) + '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td rowspan="1" style="vertical-align: top; width: 40%; height: 16px; text-align: right">';
        shtml +=  '</td>';
        shtml +=  '<td colspan="2" rowspan="1" style="vertical-align: top; height: 16px; text-align: left">';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td rowspan="1" style="vertical-align: top; width: 40%; height: 16px; text-align: right">';
        shtml +=  'TOTAL AMOUNT HELD</td>';
        shtml +=  '<td colspan="2" rowspan="1" style="vertical-align: top; height: 16px; text-align: left">';
        shtml +=  '<table style="border-right: black thin solid; border-top: black thin solid; border-left: black thin solid;';
        shtml +=  'width: 291px; border-bottom: black thin solid; font-weight: normal; font-size: 8pt; color: black; font-family: arial;">';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" rowspan="3" style="height: 17px">';
        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].TOTALAMOUNTHELD,2) + '</td>';
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
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '</tr>';
        shtml +=  '</table>';

    
   
    
        return shtml;
    }
}

/* END Account Reconcillation Summary REPORT */

/* START large currency REPORT */
function HTMLLargeCurrReport()
{
    var cls = this;
    var _branchcode;    
    var _seqid;
	var _referenceno;
    
    this.evaluate_largecurr_query = function(query)
    {
        var squery = unescape(query.toString());
        
            /*@sDate DATETIME,    
            @eDate DATETIME,    
            @AmountFrom MONEY,    
            @AmountTo MONEY,  
            @BranchCode VARCHAR (4)  */
        
        if (squery.toLowerCase().indexOf("mergeparam") > -1)
        {
            
             this.get_data(         HTMLRep.parse_merge_param_query(squery,"T@sDate"),
                                    HTMLRep.parse_merge_param_query(squery,"T@eDate"),
                                    HTMLRep.parse_merge_param_query(squery,"T@AmountFrom"),
                                    HTMLRep.parse_merge_param_query(squery,"T@AmountTo"),
                                    HTMLRep.parse_merge_param_query(squery,"T@BranchCode")                                    
									)
            return;
        }
    }
    this.get_data = function(sdate,edate,amtfrom,amtto,branchcode)
    {
        var params; 
        var sp_name = "sp_rpt_largecurrencyreport";
        
                _sdate =   " @sDate="  + sdate;    
                _sedate =   " @eDate="  + edate;    
                _amtfrom =   " @AmountFrom="  + amtfrom;    
                _amtto =   " @AmountTo="  + amtto;    
                _branchcode =   " @BranchCode="  + branchcode;    
       
		params  = _sdate + "^" + _sedate + "^" +  _amtfrom  + "^" + _amtto + "^" + _branchcode;
	
	    IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params,this.get_data_callback);
    }
    
    this.get_data_callback = function(result)
    {
        try
        {
            var ds = result.value;
            cls.display_report(ds);
            
        }
        catch(e)
        {
            alert("ERROR WHILE GETTING DAILY KEEPING DATA");
        }
    }
    
    this.display_report = function(ds)
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
        shtml +=  '<table style="width: 11in">';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" style="height: 46px; text-align: center">';
        shtml +=  '<table style="width: 304px">';
        shtml +=  '<tr>';
        shtml +=  '<td style="width: 48px; vertical-align: top; height: 21px;">';
        shtml +=  '<img src="pnbiconblack.JPG" style="height: 69px" /></td>';
        shtml +=  '<td style="vertical-align: top; width: 295px; text-align: center; height: 21px;">';
        shtml +=  '<table style="font-size: 9pt; width: 100%; font-family: arial">';
        shtml +=  '<tr>';
        shtml +=  '<td>';
        shtml +=  '<span style="font-size: 8pt; font-family: Arial Black"><strong>PHILIPPINE NATIONAL BANK</strong></span></td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td>';
        shtml +=  '<span style="font-size: 8pt">';
        shtml +=  'remitting branch here</span></td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td>';
        shtml +=  '<span style="font-size: 7pt">INTERGRATED REMITTANCE SYSTEM</span></td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td>';
        shtml +=  '<span style="font-size: 8pt">Large Currency Transaction Report</span></td>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="2" style="vertical-align: top; text-align: center">';
        shtml +=  '<table style="font-size: 8pt; width: 100%; font-family: arial">';
        shtml +=  '<tr>';
        shtml +=  '<td style="width: 94px">';
        shtml +=  'date from</td>';
        shtml +=  '<td style="width: 86px">';
        shtml +=  '' + dt.Rows[0].StartDate + '';
        shtml +=  '</td>';
        shtml +=  '<td>';
        shtml +=  'to';
        shtml +=  '</td>';
        shtml +=  '<td>';
        shtml +=  '' + dt.Rows[0].EndDate + '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td style="width: 94px">';
        shtml +=  'amount</td>';
        shtml +=  '<td style="width: 86px">';
        shtml +=  '' + dt.Rows[0].AmountFrom + '</td>';
        shtml +=  '<td>';
        shtml +=  'to</td>';
        shtml +=  '<td>';
        shtml +=  '' + HTMLRep.format_numbers(dt.Rows[0].AmountTo,2) + '</td>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" style="border-bottom-color: black; border-top-style: solid; border-top-color: black; border-bottom-style: solid">';
        shtml +=  '<table style="font-weight: bold; font-size: 8pt; width: 100%; font-family: arial;';
        shtml +=  'text-align: center">';
        shtml +=  '<tr>';
        shtml +=  '<td style="width: 130px">';
        shtml +=  'TRANSACTION DATE</td>';
        shtml +=  '<td style="width: 131px">';
        shtml +=  'TRANSACTION REF. NO.</td>';
        shtml +=  '<td style="width: 130px">';
        shtml +=  'REMITTER NAME</td>';
        shtml +=  '<td style="width: 130px">';
        shtml +=  'BENEFICIARY NAME</td>';
        shtml +=  '<td style="width: 129px">';
        shtml +=  'PRODUCT TYPE</td>';
        shtml +=  '<td style="width: 127px">';
        shtml +=  'TRANSACTION SOURCE</td>';
        shtml +=  '<td style="width: 125px">';
        shtml +=  'ACCOUNT NO.</td>';
        shtml +=  '<td>';
        shtml +=  'CURRENCY RECEIVED</td>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" style="height: 21px">';
        shtml +=  '<table style="font-weight: normal; font-size: 7pt; width: 100%; font-family: arial;';
        shtml +=  'text-align: center">';
        for(i=0;i<dt.Rows.length;i++)
        {
        shtml +=  '<tr>';
        shtml +=  '<td style="width: 130px">';
        shtml +=  '' + dt.Rows[0].TransactionDate + '</td>';
        shtml +=  '<td style="width: 131px">';
        shtml +=  '' + dt.Rows[0].TransactionRefNo + '</td>';
        shtml +=  '<td style="width: 130px">';
        shtml +=  '' + dt.Rows[0].RemitterName + '</td>';
        shtml +=  '<td style="width: 130px">';
        shtml +=  '' + dt.Rows[0].BeneficiaryName + '</td>';
        shtml +=  '<td style="width: 129px">';
        shtml +=  '' + dt.Rows[0].ProductType + '</td>';
        shtml +=  '<td style="width: 127px">';
        shtml +=  '' + dt.Rows[0].TransactionSource + '</td>';
        shtml +=  '<td style="width: 125px">';
        shtml +=  '' + dt.Rows[0].AccountNo + '</td>';
        shtml +=  '<td>';
        shtml +=  '' + dt.Rows[0].CcyReceived + '</td>';
        shtml +=  '</tr>';
        }
        shtml +=  '</table>';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '</table>';




        return shtml;
    }
    
   

}

/* Large currency end*/