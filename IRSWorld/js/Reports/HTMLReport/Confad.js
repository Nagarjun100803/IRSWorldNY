
/* START CONFAD REPORT */


var reportholder = new HTMLConfadHolder()




function HTMLConfadHolder()
{
    this.load_report = function()
    {
       
       
        
        var qstr = HTMLRep.get_hidden_querystring("hidQueryString");
        
        if (qstr.length > 0)
        {
            if (qstr.toLowerCase().indexOf("trantype=eod" ) > -1 || qstr.toLowerCase().indexOf("mergeparam") > -1 && qstr.toLowerCase().indexOf("eod") > -1)
            {
                var confad = new HTMLConfadReport();
                confad.evaluate_querystring(qstr);
                return;
            }
            if (qstr.toLowerCase().indexOf("trantype=orps" ) > -1 || qstr.toLowerCase().indexOf("mergeparam") > -1 && qstr.toLowerCase().indexOf("orps") > -1 )
            {
                var orps = new HTMLPushtoORPSReport();
                orps.evaluate_querystring(qstr);
                return;
            }
            if (qstr.toLowerCase().indexOf("sp_rpt_dailytransactionjournalsummary") > -1)
            {
                var dtjs = new HTMLDailyTranJournalSummaryReport();
                dtjs.evaluate_querystring(qstr);
                return;
            }  
        }  
    }
    
}
function HTMLConfadReport()
{
    var cls = this;
    var _sdate;
    var _branchcode;    
    var _eoddate;
    var _eodoption;
    var _extractctr;
    
    
    this.evaluate_querystring = function(query)
    {
        var squery = unescape(query.toString());
        if (squery.toLowerCase().indexOf("trantype=eod") > -1)
        {
            this.get_confad_data(
                                    HTMLRep.get_querystring_value(unescape(query),"tranvalue"),
                                    HTMLRep.get_branchcode(),
                                    HTMLRep.get_querystring_value(unescape(query),"tranvalue"),
                                    1,
                                    0
                                 )
            return;
        }
        else if (squery.toLowerCase().indexOf("mergeparam") > -1 && squery.toLowerCase().indexOf("confad") )
        {
            
             this.get_confad_data(
                                    HTMLRep.parse_merge_param_query(squery,"T@SDATE"),
                                    HTMLRep.parse_merge_param_query(squery,"T@V_BRANCHCODE"),
                                    HTMLRep.parse_merge_param_query(squery,"T@SDATE"),
                                    3,
                                    HTMLRep.parse_merge_param_query(squery,"T@V_EXTRACTCTR")
                                 )
            return;
        }        
    }
    
    this.get_querystring_value  = function(query,sfield)
    {
        var squery = query.toString();
        var stemp = squery.split("&");
        var itemp;
        var i = 0;
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
    this.get_confad_data = function(sdate,branchcode,eoddate,eodoption,extractctr)
    {
        var params; 
        var sp_name = "proc_confad_html_report";
        
                _sdate      =   "@sDate="   + sdate;
                _branchcode =   "@RemittingBranch="  + branchcode;    
                _eoddate    =   "@eoddate="     + eoddate;
                _eodoption  =   "@eodoption="  + eodoption;
                _extractctr =   "@extractctr=" + extractctr;
        
        switch (eodoption)
        {
            case    1:
            {
                _extractctr =   "@extractctr=0" ;
                
                AjaxPro.timeoutPeriod = 100 * 1000;
                AjaxPro.onTimeout = function()
                {                    
                    var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
                    alert(errmsg);       return;      
                } 
                params = _branchcode;
                IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset("proc_selectitems_for_eod",params,this.select_eod_items_callback);                                
                return;
            }
            break;
            case    2: 
            {               
                _extractctr =   "@extractctr=0" ;
                params  = _sdate + "^" + _branchcode + "^" + _eoddate + "^" + _eodoption + "^" + _extractctr;
                AjaxPro.timeoutPeriod = 100 * 1000;
                AjaxPro.onTimeout = function()
                {                    
                    var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
                    alert(errmsg);       return;      
                } 
                IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params,cls.get_confad_data_callback);
                return;
             }
              break;
             case    3: 
            {               
                _extractctr =   "@extractctr=1" ;
                params  = _sdate + "^" + _branchcode + "^" + _eoddate + "^" + _eodoption + "^" + _extractctr;
                AjaxPro.timeoutPeriod = 100 * 1000;
                AjaxPro.onTimeout = function()
                {                    
                    var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
                    alert(errmsg);       return;      
                } 
                IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params,cls.get_confad_data_callback);
                return;
             }
            break;   
                
            default:
            break;
               
        }        
        
    }
    
    this.select_eod_items_callback = function(result)
    {
        try
        {
            var ds = result.value;
            var i = 0;
            if  (ds.Tables[0].Rows.length > 0)            
            {
                dt = ds.Tables[0];
                var seqid = "";
                for(i=0;i < dt.Rows.length; i++)
                {
                    seqid += dt.Rows[i].seq_id + ",";
                }               
                
                sp_name = "proc_tag_items_foreod";
                
                var query = HTMLRep.get_hidden_querystring("hidQueryString");
                
                var sdate   = HTMLRep.get_querystring_value(unescape(query),"tranvalue");
                var sbranch = HTMLRep.get_branchcode();
                var eoddate = HTMLRep.get_querystring_value(unescape(query),"tranvalue");
                                    
                params = "@RemittingBranch=" + sbranch  +"^@sDate="+ sdate +"^@eoddate=" + eoddate + "^@v_eotitems=" + seqid.substring(0,seqid.length - 1);
                
                IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params,cls.get_confad_data_callback);  
            } 
        }
        catch(e)
        {
            alert("THERE ARE NO EOD ITEMS FOUND");
        }
    }
    
    this.get_confad_data_callback = function(result)
    {
        try
        {
            var ds = result.value;
            cls.display_confad_report(ds);
            
        }
        catch(e)
        {
            alert("NO DATA FOUND");
        }
    }
    
    this.display_confad_report = function(ds)
    {
     
        var shtml = "";
        shtml  += HTMLRep.general_styles();
        shtml   += '<div style="width:100%" align="center">';        
        shtml   +=  HTMLRep.print_html_button();
        
        shtml   +=  this.confad_header(ds.Tables[0]);
        shtml   +=  this.confad_message(ds.Tables[0]);
        shtml   +=  this.confad_body(ds.Tables[0]);        
        var pass =false;
        try
        {
            if (ds.Tables[1].Rows.length >= 0 )
            {   shtml   +=  this.confad_ope(ds.Tables[1]);}
            else
            {
                shtml   +=  this.confad_ope_blank();
            }
        }
        catch(e)
        { shtml   +=  this.confad_ope_blank()}
        
        try
        {
            shtml   +=  this.confad_forex(ds.Tables[2]);
        }
        catch(e)
        {
            shtml   +=  this.confad_forex_blank();
        }
        shtml   +=  this.confad_signatory();
        shtml   += '</div>';

        document.getElementById("bodydiv").innerHTML = shtml;

    }
    
    this.confad_header = function(dt)
    {
        var vd = window.location.href.split('/')[3];
    
        var shtml = "";
        shtml += '<table  class="stable" cellpadding="0" cellspacing="0" style="width: 461px">';
        shtml +=     '<tr>';
        shtml +=          '<td rowspan="3" width="67px" nowrap>';
        shtml +=          '<img src="../../../'+ vd +'/images/pnb_black_logo.JPG" style="width: 62px; height: 67px" />'
        shtml +=          '</td>';
        shtml +=          '<td  valign="bot" class="tdheader" style="height: 24px">&nbsp;PHILIPPINE NATIONAL BANK</td>';
        shtml +=     '</tr>';
        shtml +=     '<tr>';
        shtml +=         '<td  valign="top" class="tdlabel"> &nbsp;<label id="branchname">'+ dt.Rows[0].BranchName +'</label>';
        shtml +=         '</td>'
        shtml +=     '</tr>'
        shtml +=     '<tr>'
        shtml +=         '<td  valign="top" class="tdlabel" > &nbsp;<label id="branchaddress">'+ dt.Rows[0].BranchAddress  +'</label>'
        shtml +=         '</td>'
        shtml +=     '</tr>'
        shtml += '</table>'
        return shtml;
    }
    this.confad_message = function(dt)
    {
        var shtml = "";
        shtml += '<table class="stable"  border="0" style="width: 750px">';
        shtml +=    '<tr>';
        shtml +=        '<td style="height: 42px"></td>';
        shtml +=    '</tr>';
        shtml +=    '<tr>';
        shtml +=        '<td>Date:' + dt.Rows[0].ExecTimeofEOD +'</td>';
        shtml +=    '</tr>';
        shtml +=    '<tr>';
        shtml +=        '<td style="height: 35px"></td>';
        shtml +=    '</tr>';
        shtml +=    '<tr>';
        shtml +=        '<td>Attention: ORD Data Control</td>';
        shtml +=    '</tr>';
        shtml +=     '<tr>';
        shtml +=        '<td style="height: 32px"></td>';
        shtml +=    '</tr>';
        shtml +=    '<tr>';
        shtml +=        '<td class="header" align="center">IRSWorld Confirmation Advice '+ HTMLRep.formatDate(dt.Rows[0].ServerDate,"/") + " " +  HTMLRep.formatTime(dt.Rows[0].ServerDate)  +'</td>';
        shtml +=    '</tr>';
        shtml +=    '<tr>';
        shtml +=        '<td height="60px"></td>';
        shtml +=    '</tr>';
        shtml +=    '<tr>';
        shtml +=        '<td>This confirms our non-online remittance transactions today, viz:</td>';
        shtml +=    '</tr>';
        shtml +=    '<tr>';
        shtml +=        '<td></td>';
        shtml +=    '</tr>';
        shtml += '</table>';
        return shtml;
    }
    this.confad_body = function(dt)
    {
        var shtml = "";
        var i = 0;
        shtml += '<table class="stable"  cellspacing="0" cellpadding="0" style="width: 600px">';
        shtml +=    '<tr>';
        shtml +=        '<td style="height: 58px">';
        shtml +=            '<table border="1px" bordercolor="black" cellpadding="0" cellspacing="0" style="width: 600px;font-size:10pt;" class="tab_border">';
        shtml +=            '<tr>';
        shtml +=                '<td style="width: 73px; height: 18px" class="colheader"></td>';
        shtml +=                '<td style="width: 156px; height: 18px" class="colheader"></td>';
        shtml +=                '<td style="height: 18px; width: 189px;" align="center" class="colheader">Remittance Amount</td>';
        shtml +=                '<td style="height: 18px; width: 78px;" align="center" class="colheader">USD Items</td>';
        shtml +=                '<td style="height: 18px; width: 131px;" align="center" class="colheader">USD Amount</td>';
        shtml +=                '<td style="height: 18px;width: 78px;" align="center" class="colheader">PHP Items</td>';
        shtml +=                '<td style="height: 18px;width:131px" align="center" class="colheader">PHP Amount</td>';
        shtml +=            '</tr>';
        
        var totremamt       = 0.00;
        var totphpitems     = 0;
        var totphpamt       = 0.00;
        var totusditems     = 0;
        var totusdamt       = 0.00;
        var totitems        = 0;
        shtml +=            '<tr>';
        shtml +=                '<td rowspan="'+ dt.Rows.length +'">'+ dt.Rows[0].RemittingCurrency + '</td>';
        for(i=0;i<dt.Rows.length;i++)
        {       
            shtml +=                '<td style="width: 156px; height: 18px" nowrap>'+ dt.Rows[i].ImplementationType +'</td>';
            shtml +=                '<td align="right" style="width: 170px; height: 18px">'+ HTMLRep.format_numbers(dt.Rows[i].RemittanceAmount,2) + '</td>';
            shtml +=                '<td align="center" style="width: 60px; height: 18px">'+ dt.Rows[i].usditems + '</td>';
            shtml +=                '<td align="right" style="width: 120px; height: 18px">'+ HTMLRep.format_numbers(dt.Rows[i].usdamt,2)  + '</td>';
            shtml +=                '<td align="right" style="width: 60px; height: 18px">'+ dt.Rows[i].phpitems + '</td>';
            shtml +=                '<td align="right" style="width: 120px; height: 18px">'+ HTMLRep.format_numbers(dt.Rows[i].phpamt,2) + '</td>';
            shtml +=            '</tr>';
        
            //        totphpamt   +=    parseFloat(dt.Rows[i].phpamt);
            //        totphpitems +=  parseInt(dt.Rows[i].phpitems);
            //        totusdamt   +=    parseFloat(dt.Rows[i].usdamt);
            //        totusditems +=  parseInt(dt.Rows[i].usditems);
            //        totremamt   +=    parseFloat(dt.Rows[i].RemittanceAmount);
            //        
            totphpamt   = parseFloat(totphpamt.toFixed(2))  + parseFloat(dt.Rows[i].phpamt.toFixed(2));
            totusdamt   = parseFloat(totusdamt.toFixed(2))  + parseFloat(dt.Rows[i].usdamt.toFixed(2));
            totremamt   =  parseFloat(totremamt.toFixed(2)) + parseFloat(dt.Rows[i].RemittanceAmount.toFixed(2));
            totphpitems +=  dt.Rows[i].phpitems;
            totusditems +=  dt.Rows[i].usditems;
        }            
        totphpamt = totphpamt.toFixed(2);
        totusdamt = totusdamt.toFixed(2);
        totremamt = totremamt.toFixed(2);
        
        shtml +=            '<tr>';
        shtml +=                '<td colspan="2" class="colheader" >&nbsp; TOTAL</td>';
        shtml +=                '<td align="right" style="width: 189px; height: 18px">'+ HTMLRep.format_numbers(totremamt,2) + '</td>';
        shtml +=                '<td align="center" style="width: 78px; height: 18px">'+ totusditems + '</td>';
        shtml +=                '<td align="right" style="width: 131px; height: 18px">'+ HTMLRep.format_numbers(totusdamt,2)  + '</td>';
        shtml +=                '<td align="right" style="width: 78px; height: 18px">'+ totphpitems + '</td>';
        shtml +=                '<td align="right" style="width: 131px; height: 18px">'+ HTMLRep.format_numbers(totphpamt,2) + '</td>';
        shtml +=            '</tr>';
        shtml +=            '</table>';
        shtml +=        '</td>';
        shtml +=    '</tr>';        
        shtml += '</table>';
        shtml += '<br />'
        return shtml;
    }
    this.confad_ope = function(dt)
    {
        var pass =false;
        try
        {
            if (ds.Tables[1].Rows.length >= 0 )
            { pass = true}
        }
        catch(e)
        {}
        var shtml = "";
        shtml += '<table class="stable"  border="1" style="width: 600" cellspacing ="0" cellpadding="0" class="tab_border">';
        shtml +=    '<tr>';
        shtml +=        '<td style="height: 58px">';
        shtml +=            '<table border="0" cellpadding="0" cellspacing="0" style="width: 600">';
        shtml +=            '<tr>';
        shtml +=                '<td class="header" style="width: 204px; height: 6px"></td>';
        shtml +=                '<td style="width: 120px; height: 6px"></td>';
        shtml +=            '</tr>';
        
        if (pass)
        {
            var i = 0;
            for (i = 0 ;i < dt.Rows.length; i++)
            {
            shtml +=            '<tr>';
            shtml +=                '<td class="header" style="width: 204px; height: 27px">OPE</td>';
            shtml +=                '<td style="width: 120px; height: 27px">' + dt.Rows[i].OPE + '</td>';
            shtml +=            '</tr>';
            shtml +=            '<tr>';
            shtml +=                '<td class= "header" style="width: 204px; height: 22px">DOCUMENTARY STAMP</td>';
            shtml +=                '<td style="width: 120px; height: 22px">' + dt.Rows[i].DOCStamp  + '</td>';
            shtml +=            '</tr>';
            }
        }
        else
        {
            shtml +=            '<tr>';
            shtml +=                '<td class="header" style="width: 204px; height: 27px">OPE</td>';
            shtml +=                '<td style="width: 120px; height: 27px"> 0.00 </td>';
            shtml +=            '</tr>';
            shtml +=            '<tr>';
            shtml +=                '<td class= "header" style="width: 204px; height: 22px">DOCUMENTARY STAMP</td>';
            shtml +=                '<td style="width: 120px; height: 22px">0.00</td>';
            shtml +=            '</tr>';
        }
        shtml +=            '</table>';
        shtml +=        '</td>';
        shtml +=    '</tr>';
        shtml += '</table>';
        shtml += '<br />';
        
        return shtml;
    }
    this.confad_ope_blank = function()
    {
        
        var shtml = "";
        shtml += '<table class="stable"  border="1" style="width: 600" cellspacing ="0" cellpadding="0" class="tab_border">';
        shtml +=    '<tr>';
        shtml +=        '<td style="height: 58px">';
        shtml +=            '<table border="0" cellpadding="0" cellspacing="0" style="width: 600">';
        shtml +=            '<tr>';
        shtml +=                '<td class="header" style="width: 204px; height: 6px"></td>';
        shtml +=                '<td style="width: 120px; height: 6px"></td>';
        shtml +=            '</tr>';
        
      
            shtml +=            '<tr>';
            shtml +=                '<td class="header" style="width: 204px; height: 27px">OPE</td>';
            shtml +=                '<td style="width: 120px; height: 27px"> 0.00 </td>';
            shtml +=            '</tr>';
            shtml +=            '<tr>';
            shtml +=                '<td class= "header" style="width: 204px; height: 22px">DOCUMENTARY STAMP</td>';
            shtml +=                '<td style="width: 120px; height: 22px">0.00</td>';
            shtml +=            '</tr>';
    
        shtml +=            '</table>';
        shtml +=        '</td>';
        shtml +=    '</tr>';
        shtml += '</table>';
        shtml += '<br />';
        
        return shtml;
    }
    this.confad_forex = function(dt)
    {
        var shtml ="";
        var i = 0;
        shtml += '<table class="stable"  border="1" style="width: 600px" cellspacing="0" cellpadding="0" class="tab_border">';
        shtml +=    '<tr>';
        shtml +=        '<td style="height: 58px" align="center">';
        shtml +=            '<table class="stable"  border="0" cellpadding="0" cellspacing="0" style="width: 600px">';
        shtml +=            '<tr>';
        shtml +=                '<td colspan="5" style="height: 27px" align="left"><strong>FX RATES USED:' + HTMLRep.formatDate(dt.Rows[0].TranxDate,"/") +'</strong></td>';
        shtml +=            '</tr>';
        shtml +=            '<tr>';
        shtml +=                '<td nowrap style="width: 150px; height: 27px">Date Inserted</td>';
        shtml +=                '<td nowrap style="width: 100px; height: 27px">Source Currency</td>';
        shtml +=                '<td nowrap style="width: 100px; height: 27px">Target Currency</td>';
        shtml +=                '<td nowrap style="width: 100px; height: 27px">Source To Target</td>';
        shtml +=                '<td nowrapstyle="width: 100px; height: 27px">Target To Source</td>';
        shtml +=            '</tr>';
        for(i = 0; i < dt.Rows.length; i++)
        {
        shtml +=            '<tr>';
        shtml +=                '<td nowrap style="width: 150px; height: 22px">' + dt.Rows[i].DateInserted + '</td>';
        shtml +=                '<td nowrap style="width: 100px; height: 22px">' + dt.Rows[i].SourceCurrency + '</td>';
        shtml +=                '<td nowrap style="width: 100px; height: 22px">' + dt.Rows[i].TargetCurrency + '</td>';
        shtml +=                '<td nowrap style="width: 100px; height: 22px">' + dt.Rows[i].SourceToTarget + '</td>';
        shtml +=                '<td nowrap style="width: 100px; height: 22px">' + dt.Rows[i].TargetToSource + '</td>';
        shtml +=            '</tr>';
        }
        shtml +=            '</table>';
        shtml +=        '</td>';
        shtml +=    '</tr>';
        shtml += '</table><br />';
        return shtml;
    }
    
    this.confad_forex_blank = function(dt)
    {
        var shtml ="";
        shtml += '<table class="stable"  border="1" style="width: 600px" cellspacing="0" cellpadding="0" class="tab_border">';
        shtml +=    '<tr>';
        shtml +=        '<td style="height: 58px" align="center">';
        shtml +=            '<table class="stable"  border="0" cellpadding="0" cellspacing="0" style="width: 600px">';
        
        var date;
        
        
        try
        {
            sdate = HTMLRep.formatDate(dt.Rows[0].TranxDate,"/") ;
        }
        catch(e)
        {
            sdate = "";
        }
        shtml +=            '<tr>';
        shtml +=                '<td colspan="5" style="height: 27px" align="left"><strong>FX RATES USED:' + sdate +'</strong></td>';
        shtml +=            '</tr>';
        shtml +=            '<tr>';
        shtml +=                '<td nowrap style="width: 150px; height: 27px">Date Inserted</td>';
        shtml +=                '<td nowrap style="width: 100px; height: 27px">Source Currency</td>';
        shtml +=                '<td nowrap style="width: 100px; height: 27px">Target Currency</td>';
        shtml +=                '<td nowrap style="width: 100px; height: 27px">Source To Target</td>';
        shtml +=                '<td nowrapstyle="width: 100px; height: 27px">Target To Source</td>';
        shtml +=            '</tr>';        
        shtml +=            '<tr>';
        shtml +=                '<td nowrap style="width: 150px; height: 22px">&nbsp;</td>';
        shtml +=                '<td nowrap style="width: 100px; height: 22px">&nbsp;</td>';
        shtml +=                '<td nowrap style="width: 100px; height: 22px">&nbsp;</td>';
        shtml +=                '<td nowrap style="width: 100px; height: 22px">&nbsp;</td>';
        shtml +=                '<td nowrap style="width: 100px; height: 22px">&nbsp;</td>';
        shtml +=            '</tr>';        
        shtml +=            '</table>';
        shtml +=        '</td>';
        shtml +=    '</tr>';
        shtml += '</table><br />';
        return shtml;
    }
    this.confad_signatory = function()
    {
        var shtml = "";
        shtml += '<table class="stable" border="0" style="width: 750px">';
        shtml +=    '<tr>';
        shtml +=        '<td class="header" style="height: 42px">Transaction Reference Nos.</td>';
        shtml +=    '</tr>';
        
        shtml +=    '<tr>';
        shtml +=        '<td valign="bottom" style="height: 35px">Authorized Signatory</td>';
        shtml +=    '</tr>';                          
        shtml +=    '<tr>';
        shtml +=        '<td vaign="top"><hr color="black"></td>';
        shtml +=    '</tr>';
        shtml += '</table>';
        return shtml;
        
    }
}

/* END CONFAD REPORT */
/* START PUSHED TO ORPS REPORT */
function HTMLPushtoORPSReport()
{
    var _tranxdate;
    var _branchcode;
    var cls = this;
    this.evaluate_querystring = function(query)
    {
        var squery = unescape(query.toString());
        
        if (squery.toLowerCase().indexOf("trantype=orps") > -1)
        {
            this.get_orps_data(
                                    HTMLRep.get_querystring_value(unescape(query),"tranvalue"),
                                    HTMLRep.get_branchcode()
                                 )
            return;
        }
        if (squery.toLowerCase().indexOf("mergeparam") > -1)
        {
            
             this.get_orps_data(
                                    HTMLRep.parse_merge_param_query(unescape(squery),"T@V_TRANXDATE"),
                                    HTMLRep.parse_merge_param_query(unescape(squery),"T@V_BRANCHCODE")                                    
                                 )
            return;
        }
    }
    
    this.get_orps_data = function(tdate,branchcode)
    {
        var params; 
        var sp_name = "proc_pushedtoORPS_html";
        
                _tranxdate    =   "@v_tranxdate="   + HTMLRep.formatDate(tdate,"/");
                _branchcode   =   "@v_branchcode="  + branchcode;    
        
        params  = _tranxdate + "^" + _branchcode ;
        AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
            alert(errmsg);        
            return;     
        } 
        IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params,this.get_orps_data_callback);
    }
    
    this.get_orps_data_callback = function(result)
    {
        try
        {
            var ds = result.value;
            cls.display_push_to_orps_report(ds);            
        }
        catch(e)
        {
            alert("NO DATA FOUND");
        }
    }
    this.display_push_to_orps_report = function(ds)
    {
         var shtml = "";
         shtml  += HTMLRep.general_styles();
         shtml   += '<div style="width:100%" align="center">';
         shtml   +=  HTMLRep.print_html_button();
         shtml   +=  cls.orps_body(ds.Tables[0]);
         shtml   +=  '</div>';
         
         document.getElementById("bodydiv").innerHTML = shtml;
        
    }
    this.orps_body = function(dt)
    {
    
        var vd = window.location.href.split('/')[3];
        var i = 0;
        var shtml= "";
        shtml += '<table class="stable" style="width: 1108px">';
        shtml += '        <tr>';
        shtml += '            <td align="center">';
        shtml += '                <table class="stable" style="width: 35%" border="0" cellpadding="0" cellspacing="0">';
        shtml += '                    <tr>';
        shtml += '                        <td rowspan="5" style="width: 77px">';
        shtml += '                            <img src="../../../'+ vd  +'/images/pnb_black_logo.JPG" style="width: 74px; height: 75px" /></td>';
        shtml += '                        <td align="left" class="header">PHILIPPINE NATIONAL BANK</td>';
        shtml += '                    </tr>';        
        shtml += '                     <tr>';
        shtml += '                        <td align="left">' + dt.Rows[0].CoveringBank + '</td>';
        shtml += '                    </tr>';
        shtml += '                    <tr>';
        shtml += '                        <td align="left">IRSWorld</td>';
        shtml += '                    </tr>';
        shtml += '                    <tr>';
        shtml += '                        <td align="left">PUSHED TO ORPS</td>';
        shtml += '                    </tr>';
        shtml += '                     <tr>';
        shtml += '                        <td align="left">' + dt.Rows[0].Tranxdate + '</td>';
        shtml += '                    </tr>';
        shtml += '                </table>';
        shtml += '            </td>';
        shtml += '        </tr>';
        shtml += '        <tr>';
        shtml += '            <td ><table  class="stable" style="width: 100%" border="0" cellpadding="0" cellspacing="0">';
        shtml += '                <tr>';
        shtml += '                    <td style="height: 2px" colspan="8" valign="bottom">';
        shtml += '                <hr style="color: black" />';
        shtml += '                       </td>';
        shtml += '                </tr>';
        shtml += '                <tr class="header">';
        shtml += '            <td style="width: 112px; height: 21px" align="center">TRANXDATE</td>';
        shtml += '            <td style="width: 121px; height: 21px" align="center">REFERENCENO</td>';
        shtml += '            <td style="height: 21px" align="center">REMITTER NAME</td>';
        shtml += '                    <td align="center" style="height: 21px">BENEFICIARY NAME</td>';
        shtml += '                    <td align="center" style="width: 115px; height: 21px">PRODUCT CODE</td>';
        shtml += '                    <td align="center" style="height: 21px">COVERING BANK</td>';
        shtml += '                    <td align="center" style="height: 21px">NET TO BENEFICIARY</td>';
        shtml += '                    <td align="center" style="height: 21px">GROSS AMOUNT</td>';
        shtml += '                </tr>';       
        for(i=0; i < dt.Rows.length; i++)
        {
        shtml += '                <tr>';
        shtml += '                    <td nowrap style="width: 112px">' + dt.Rows[i].Tranxdate + '</td>';
        shtml += '                    <td align="center" nowrap style="width: 121px">' + dt.Rows[i].ReferenceNo + '</td>';
        shtml += '                    <td nowrap>' + dt.Rows[i].RemitterName + '</td>';
        shtml += '                    <td nowrap>' + dt.Rows[i].BeneName + '</td>';
        shtml += '                    <td align="right" nowrap style="width: 115px">' + dt.Rows[i].ProductCode + '</td>';
        shtml += '                    <td align="right" nowrap >' + dt.Rows[i].CoveringBank + '</td>';
        shtml += '                    <td align="right" nowrap>' + dt.Rows[i].NettoBeneAmount + '</td>';
        shtml += '                    <td align="right" nowrap>' + HTMLRep.format_numbers(dt.Rows[i].GrossAmt,2) + '</td>';
        shtml += '                </tr>';
        }
        shtml += '            </table>';
        shtml += '            </td>';
        shtml += '        </tr>';        
        shtml += '    </table>';
        
        return shtml;
    }
}
/* END OF PUSHED TO OPRS REPORT */
/* START OF DAILY TRANSACTION JOURNAL SUMMARY*/
function HTMLDailyTranJournalSummaryReport()
{
    var _tranxdate;
    var _branchcode;
    var cls = this;
    this.evaluate_querystring = function(query)
    {
        var squery = unescape(query.toString());        
      
        if (squery.toLowerCase().indexOf("mergeparam") > -1)
        {
            
             this.get_data(
                                    HTMLRep.parse_merge_param_query(unescape(squery),"T@sDate"),
                                    HTMLRep.parse_merge_param_query(unescape(squery),"T@BranchCode")                                    
                                 )
            return;
        }
    }
    
    this.get_data = function(tdate,branchcode)
    {
        var params; 
        var sp_name = "sp_rpt_DailyTransactionJournalSummary";
        
                _tranxdate    =   "@sDate="   + HTMLRep.formatDate(tdate,"/");
                _branchcode   =   "@BranchCode="  + branchcode;    
        
        params  = _tranxdate + "^" + _branchcode ;
        AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
            alert(errmsg);      return;       
        } 
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
           alert("NO DATA FOUND");
        }
    }
    this.display_report = function(ds)
    {
         var shtml = "";
         shtml  += HTMLRep.general_styles();
         shtml   += '<div style="width:100%" align="center">';
         shtml   +=  HTMLRep.print_html_button();
         shtml   +=  cls.daily_tran_jour_summ_body(ds.Tables[0]);
         shtml   +=  '</div>';
         
         document.getElementById("bodydiv").innerHTML = shtml;
        
    }
    this.daily_tran_jour_summ_body = function(dt)
    {
        var vd = window.location.href.split('/')[3];
        
        var i = 0;
        var shtml= "";
        shtml += '<table class="stable" style="width: 1108px">';
        shtml += '        <tr>';
        shtml += '            <td align="center">';
        shtml += '                <table  class="stable"  style="width: 35%" border="0" cellpadding="1" cellspacing="1">';
        shtml += '                    <tr>';
        shtml += '                        <td rowspan="5" style="width: 77px">';
        shtml += '                            <img src="../../../'+ vd +'/images/pnb_black_logo.JPG" style="width: 74px; height: 75px" /></td>';
        shtml += '                        <td align="left" class="header">PHILIPPINE NATIONAL BANK</td>';
        shtml += '                    </tr>';
        shtml += '                    <tr>';
        shtml += '                        <td align="left">' + dt.Rows[0].RemittingBranch +'</td>';
        shtml += '                    </tr>';
        shtml += '                    <tr>';
        shtml += '                        <td align="left">IRSWorld</td>';
        shtml += '                    </tr>';
        shtml += '                    <tr>';
        shtml += '                        <td align="left">DAILY TRANSACTION JOURNAL SUMMARY</td>';
        shtml += '                    </tr>';
        shtml += '                     <tr>';
        shtml += '                        <td align="left">' + dt.Rows[0].TranxDate + '</td>';
        shtml += '                    </tr>';
        shtml += '                </table>';
        shtml += '            </td>';
        shtml += '        </tr>';
        shtml += '        <tr>';
        shtml += '            <td ><table class="stable"  style="width: 100%" border="0" cellpadding="0" cellspacing="0">';
        shtml += '                <tr>';
        shtml += '                    <td colspan ="16"  style="height: 2px" colspan="8" valign="bottom">';
        shtml += '                <hr style="color: black" />';
        shtml += '                       </td>';
        shtml += '                </tr>';
        shtml += '                <tr style="text-align:center;font-size:9pt;">';
        shtml += '                      <td class="tdsmallb" nowrap>TRANSACTION</td>';
        shtml += '                      <td class="tdsmallb" nowrap>CCY</td>';
        shtml += '                      <td class="tdsmallb" nowrap>AMOUNT</td>';
        shtml += '                      <td class="tdsmallb" nowrap>CCY (NET</td>';
        shtml += '                      <td class="tdsmallb" nowrap>AMOUNT</td>';
        shtml += '                      <td class="tdsmallb" nowrap>PRODUCT</td>';
        shtml += '                      <td class="tdsmallb" nowrap>REMITTER</td>';
        shtml += '                      <td class="tdsmallb" nowrap>BENEFICIARY</td>';
        shtml += '                      <td class="tdsmallb" nowrap>PNB</td>';
        shtml += '                      <td class="tdsmallb" nowrap>PAYEE BANK</td>';
        shtml += '                      <td class="tdsmallb" nowrap>ACCOUNT</td>';
        shtml += '                      <td class="tdsmallb" nowrap>TRANSACTION</td>';
        shtml += '                      <td class="tdsmallb" nowrap>FX</td>';
        shtml += '                      <td class="tdsmallb" nowrap>OVERRIDING</td>';
        shtml += '                      <td class="tdsmallb" nowrap>TRANSACTION</td>';
        shtml += '                      <td class="tdsmallb" nowrap>REMITTER</td>';        
        shtml += '                </tr>';
        shtml += '                <tr style="text-align:center;font-size:9pt;">';
        shtml += '                      <td class="tdsmallb">REF NO</td>';
        shtml += '                      <td class="tdsmallb">(GROSS)</td>';
        shtml += '                      <td class="tdsmallb">(GROSS)</td>';
        shtml += '                      <td class="tdsmallb">TO BENEF)</td>';
        shtml += '                      <td class="tdsmallb">(NET</td>';
        shtml += '                      <td class="tdsmallb">TYPE</td>';
        shtml += '                      <td class="tdsmallb">NAME</td>';
        shtml += '                      <td class="tdsmallb">NAME</td>';
        shtml += '                      <td class="tdsmallb">BRANCH</td>';
        shtml += '                      <td class="tdsmallb">NAME</td>';
        shtml += '                      <td class="tdsmallb">NUMBER</td>';
        shtml += '                      <td class="tdsmallb">STATUS</td>';
        shtml += '                      <td class="tdsmallb">RATES</td>';
        shtml += '                      <td class="tdsmallb">OFFICERS</td>';
        shtml += '                      <td class="tdsmallb">SOURCE</td>';
        shtml += '                      <td class="tdsmallb">ID</td>';        
        shtml += '                </tr>';
        shtml += '                <tr style="text-align:center;font-size:9pt;">';
        shtml += '                      <td class="tdsmallb"></td>';
        shtml += '                      <td class="tdsmallb"></td>';
        shtml += '                      <td class="tdsmallb"></td>';
        shtml += '                      <td class="tdsmallb"></td>';
        shtml += '                      <td class="tdsmallb" nowrap>TO BENEF)</td>';
        shtml += '                      <td class="tdsmallb"></td>';
        shtml += '                      <td class="tdsmallb"></td>';
        shtml += '                      <td class="tdsmallb"></td>';
        shtml += '                      <td class="tdsmallb">CODE</td>';
        shtml += '                      <td class="tdsmallb"></td>';
        shtml += '                      <td class="tdsmallb"></td>';
        shtml += '                      <td class="tdsmallb"></td>';
        shtml += '                      <td class="tdsmallb">USED</td>';
        shtml += '                      <td class="tdsmallb"></td>';
        shtml += '                      <td class="tdsmallb"></td>';
        shtml += '                      <td class="tdsmallb"></td>';        
        shtml += '                </tr>';
        shtml += '                <tr>';
        shtml += '                    <td colspan ="16" style="height: 2px" colspan="8" valign="bottom">';
        shtml += '                <hr style="color: black" />';
        shtml += '                       </td>';
        shtml += '                </tr>';
        var userid  = "";
        for(i=0; i < dt.Rows.length; i++)
        {
            if (userid != dt.Rows[i].UserID)
            {
                shtml   += '                <tr style="text-align:center;font-size:10pt;">';
                shtml   += '                      <td colspan="16" align="left"class="tdsmallb">' + dt.Rows[i].UserID + '</td>';
                shtml   += '                </tr>';
                userid = dt.Rows[i].UserID;
            }
            shtml += '                <tr style="text-align:center;font-size:9pt;">';
            shtml += '                      <td class="tdsmall">' + dt.Rows[i].ReferenceNo  + '</td>';
            shtml += '                      <td class="tdsmall">' + dt.Rows[i].RemittingCurrency  + '</td>';
            shtml += '                      <td class="tdsmall" align="right">' + HTMLRep.format_numbers(dt.Rows[i].GrossAmount)  + '</td>';
            shtml += '                      <td class="tdsmall">' + dt.Rows[i].NettoBeneCurrency  + '</td>';
            shtml += '                      <td class="tdsmall" align="right">' + HTMLRep.format_numbers(dt.Rows[i].NettoBeneAmount)  + '</td>';
            shtml += '                      <td class="tdsmall">' + dt.Rows[i].ProductCode  + '</td>';
            shtml += '                      <td class="tdsmall" align="left">' + dt.Rows[i].RemitterName  + '</td>';
            shtml += '                      <td class="tdsmall" align="left">' + dt.Rows[i].BeneficiaryName  + '</td>';
            shtml += '                      <td class="tdsmall">' + dt.Rows[i].BenefBranch  + '</td>';
            shtml += '                      <td class="tdsmall">' + dt.Rows[i].OtherBank  + '</td>';
            shtml += '                      <td class="tdsmall">' + dt.Rows[i].AccountNo  + '</td>';
            shtml += '                      <td class="tdsmall">' + dt.Rows[i].TransactionStatus  + '</td>';
            shtml += '                      <td class="tdsmall"  align="right"' + HTMLRep.format_numbers(dt.Rows[i].FXRemitToBenef)  + '</td>';
            shtml += '                      <td class="tdsmall">' + dt.Rows[i].OverridingOfficer  + '</td>';
            shtml += '                      <td class="tdsmall">' + dt.Rows[i].TransactionSource  + '</td>';
            shtml += '                      <td class="tdsmall">' + dt.Rows[i].RemitterCIFID  + '</td>';        
            shtml += '                </tr>';
        }
        shtml   += '                <tr style="text-align:center;font-size:9pt;">';
        shtml   += '                      <td class="tdsmallb" align="right">GRAND TOTAL</td>';    
        shtml   += '                      <td class="tdsmallb" colspan="15" align="left">' + dt.Rows.length + '</td>';
        shtml   += '                </tr>';
        shtml += '            </table>';
        shtml += '            </td>';
        shtml += '        </tr>';        
        shtml += '    </table>';
        
        return shtml;
    }
}

/* END OF DAILY TRANSACTION JOURNAL SUMMARY*/
