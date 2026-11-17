
var reportholder = new HTMLDailyTellerTotalHolder();

function TotalTable()
{
    this.Rows = new Array();
}
function TotalRow()
{
    this.currency = '';
    this.amount = 0;
}

TotalTable.prototype.addRow = function(row)
{
    this.Rows.push(row);
}

function HTMLDailyTellerTotalHolder()
{
    this.load_report = function()
    {
        var qstr = HTMLRep.get_hidden_querystring("hidQueryString");
        
        if (qstr.length > 0)
        {            
            
            if (qstr.toLowerCase().indexOf("sp_rpt_dailytellerstotalreporttotalamount") > -1 || qstr.toLowerCase().indexOf("sp_rpt_dailytellerstotalreportnetamountonline") > -1 )
            {
                var dtjs = new HTMLDailyTellerTotal();
                dtjs.evaluate_querystring(qstr);
                return;
            }  
        }  
    }
}

function HTMLDailyTellerTotal()
{
    var cls =   this;
    this.group_total = new TotalTable();
    this.group_total_charges = new TotalTable();
    this.dailyreporttype = 1;
    
    this.evaluate_querystring = function(query)
    {
        var squery = unescape(query.toString());
        var reporttype = 1;
        
        if (squery.toLowerCase().indexOf("sp_rpt_dailytellerstotalreporttotalamount") > -1 )
        {
            this.dailyreporttype = 1;
        }
        if(squery.toLowerCase().indexOf("sp_rpt_dailytellerstotalreportnetamountonline") > -1 )
        {
            this.dailyreporttype = 2;
        }
        
        
        this.get_data(
                      HTMLRep.parse_merge_param_query(squery,"T@SDATE"),
                      HTMLRep.parse_merge_param_query(squery,"T@USERID"),
                      HTMLRep.parse_merge_param_query(squery,"T@BRANCHCODE")
                     )
        return;
    }
    this.get_data = function(startdate,userid,branchcode,reporttype)
    {
        var params;
        var sp_name;
        if (this.dailyreporttype == 2)
        {
            sp_name = "sp_rpt_dailytellerstotalreportnetamountonline";
        }
        else
        {
            sp_name = "sp_rpt_DailyTellersTotalReportTotalAmount";
        }
        
                _sdate      =   "@sDate="   + startdate;
                _branchcode =   "@BranchCode="  + branchcode;    
                _userid    =   "@UserId="     + userid;
                
        
        
        params  = _sdate + "^" + _userid + "^" +  _branchcode;
        AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
            alert(errmsg);       return;      
        }        
        IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params,cls.get_data_callback);
    
    }
    this.get_data_callback = function(res)
    {
        
        try
        {
        
            var ds = res.value;
            var dt = ds.Tables[0];
            
            if (dt.Rows.length > 0)
            {
                cls.display_report(res.value);
            }
            else
            {
                alert("no record found");
            }
            
        }
        catch(e)
        {
            alert("There was an error encountered while getting data for the report. Please try again.");
        }        
    }
    
    this.display_report = function(ds)
    {
        var shtml = "";
        
        this.group_total = [];
        this.group_total_charges = [];
        this.total_charges_peso = 0;
        
        this.group_total =  new TotalTable();
        this.group_total_charges =  new TotalTable();
        this.group_total_charges_peso =  new TotalTable();
        
        shtml +=  '<table class="stable"  border="0" style="width:100%">';
        shtml +=  '<tr>';
        shtml +=  '  <td align="center" nowrap>';
        shtml +=  cls.create_report_header(ds.Tables[0]);
        shtml +=  '  </td>';
        shtml +=  '</tr>';
        shtml +=  '<tr><td><hr size="1" style="width:100%" color="black"/> </td></tr>';
        shtml +=  '<tr><td nowrap>' + cls.create_column_header(ds.Tables[0]) + '</td></tr>';
        var tran_source = "";
        var dt =  ds.Tables[0];
        var subtotal = 0;
        var i = 0;
        for (i=0; i < dt.Rows.length ; i++)
        {
            
            if (tran_source != dt.Rows[i].TransactionSource)
            {
                shtml +=  '<tr>';
                shtml +=  '  <td style="width:100%" nowrap>';
                shtml +=  cls.create_main_detail(dt.Rows[i]);
                tran_source = dt.Rows[i].TransactionSource;
                shtml +=  '  </td>';        
                shtml +=  '</tr>';
            }           
            
            shtml +=  '<tr>';
            shtml +=  '  <td style="width:100%" nowrap>';
            shtml +=  cls.create_sub_detail(dt.Rows[i]);
            shtml +=  '  </td>'  ;      
            shtml +=  '</tr>';
            
            r = dt.Rows[i];
            subtotal = 0;
            subtotal = r["Total Commission"] + r["Total Local Cable Charges"] + r["Total Back End Charges Charges"];
            
            this.add_group_total(dt.Rows[i].RemittingCurrency,dt.Rows[i]["Total Gross Remittance Amount"] ,subtotal);
            
            subtotal = 0;
            var tds     =   r["Total Documentrary Stamp"].toString();
            var tope    =   r["Total OPE"].toString();
            var tsf     =   r["Total Service Fee"].toString();
            subtotal =      tds + tope + tsf;
            
            
            this.total_charges_peso +=  parseFloat(subtotal);
        }
        shtml +=  '<tr><td>' + this.create_grand_total_charges() + '</td></tr>';
        shtml +=  '<tr><td>' + this.create_grand_total() + '</td></tr>';
        shtml +=  '</table>';
        
        document.getElementById("bodydiv").innerHTML = shtml;
        
    }
    this.add_group_total = function(currency,amount,chargeamount)
    {
        for(i=0;i < this.group_total.Rows.length; i++)
        {
            if (this.group_total.Rows[i].currency == currency)
            {
                this.group_total.Rows[i].amount += parseFloat(amount);
                this.group_total_charges.Rows[i].amount += parseFloat(chargeamount);
                return;
            }
        }
        var trow = new TotalRow();
        trow.amount = parseFloat(amount);
        trow.currency = currency;
        this.group_total.addRow(trow);
        
        var trow = new TotalRow();
        trow.amount = parseFloat(chargeamount);
        trow.currency = currency;
        this.group_total_charges.addRow(trow);
    }
    this.create_report_header = function(dt)
    {
        var vd = window.location.href.split('/')[3];
        var shtml ="";
        shtml +=        '   <table class="stable"  border="0" cellpadding="0" cellspacing="0" style="width: 341px; height: 36px">';
        shtml +=        '                <tr> ';
        shtml +=        '                    <td rowspan="5" style="width: 84px"> <img src="../../../'+ vd +'/images/pnb_black_logo.JPG" alt="pnb_black_log.jpg" style="width: 62px; height: 67px"/>';
        shtml +=        '                    </td> ';
        shtml +=        '                    <td class="colheader" style="width: 213px">';
        shtml +=        '                        PHILIPPINE NATIONAL BANK</td>';
        shtml +=        '                </tr>';
        shtml +=        '                <tr>';
        shtml +=        '                    <td class="colheader" style="height: 19px; width: 213px;">' + dt.Rows[0].RemittingBranch;
        shtml +=        '                    </td>';
        shtml +=        '                </tr>';
        shtml +=        '                <tr>';
        shtml +=        '                    <td class="colheader" style="width: 213px">';
        shtml +=        '                        IRSWorld</td>';
        shtml +=        '                </tr>';
        shtml +=        '                <tr>';
        shtml +=        '                    <td class="colheader" style="width: 213px">';
        if (this.dailyreporttype == 1)
        {
            shtml +=        '                        Daily Teller'+ "&#39;" + 's Total</td>';
        }
        else
        {
            shtml +=        '                        Daily Teller'+ "&#39;" + 's Total - Credited Online</td>';
        }
        shtml +=        '                </tr>';
        shtml +=        '                <tr>';
        shtml +=        '                    <td style="width: 213px">' + dt.Rows[0].StartDate;
        shtml +=        '                    </td>';
        shtml +=        '                </tr>';
        shtml +=        '</table>';
        return shtml;
    }
    this.create_column_header = function(dt)
    {
        var shtml= "";
        shtml +=        '   <table border="0" cellpadding="0" cellspacing="0" style="height: 36px; width:100%">';
        shtml +=        '            <tr>';
        shtml +=        '                <td class="colheader" align="center" style="width: 83px; height: 36px;">';
        shtml +=        '                    TELLER</td>';
        shtml +=        '                <td class="colheader" style="width: 213px; height: 36px;" align="center">';
        shtml +=        '                    NO.OF ITEMS</td>';
        shtml +=        '                <td class="colheader" style="width: 213px; height: 36px;" align="center">';
        shtml +=        '                    TOTAL AMOUNT(RMT CCY)</td>';
        shtml +=        '                <td  class="colheader"style="width: 213px; height: 36px;" align="center">';
        shtml +=        '                    NET TO BENEF (A/C CCY)</td>';
        shtml +=        '            </tr>';
        shtml +=        '           <tr><td  class="colheader" colspan="4" align="left">' + dt.Rows[0].TellerID + '</td></tr>'
        shtml +=        '        </table>';
        return shtml;
    }
    this.create_main_detail = function(r)
    {
        var shtml = "";
        shtml +=  '<table border="0" cellpadding="0" cellspacing="0" style="width: 100%; ">';
        shtml +=  '                <tr>';
        shtml +=  '                    <td style="width:100%" colspan="2" class="colheader">' + r.TransactionSource + '</td>';
        shtml +=  '                </tr>';
        shtml +=  '                <tr>';
        shtml +=  '                    <td  align="right" valign="top" style="width:50%">';
        shtml +=  '                    <table class="stable" border="0" cellpadding="0" cellspacing="0" style="width: 346px; ">';
        shtml +=  '                        <tr>';
        shtml +=  '                            <td align="center" style="width: 151px; "></td>';
        shtml +=  '                            <td align="center" style="width: 140px; "></td>';
        shtml +=  '                            <td style="width: 333px; " align="center"> '+ r.RemittingCurrency +'</td>';
        shtml +=  '                        </tr>';
        shtml +=  '                        <tr>';
        shtml +=  '                            <td style="width: 151px; ">' + r.CodeNoOfItems + '</td>';
        shtml +=  '                            <td style="width: 140px; "></td>';
        shtml +=  '                            <td style=" width: 333px;" align="right">'  + HTMLRep.format_numbers(r["Total Gross Remittance Amount"],2) ;
        shtml +=  '                            </td>';
        shtml +=  '                        </tr>';
        shtml +=  '                        <tr><td></td><td colspan="2"><hr size="1px" style="width:100%" color="black" /></td></tr>';
        shtml +=  '                        <tr>';
        shtml +=  '                            <td style="width: 151px; "></td>';
        shtml +=  '                            <td style="width: 140px; ">TOTAL</td>';
        shtml +=  '                            <td style="width: 333px; " align="right">' +  HTMLRep.format_numbers(r["Total Gross Remittance Amount"],2);
        shtml +=  '                            </td>';
        shtml +=  '                        </tr>';
        shtml +=  '                        <tr><td></td><td colspan="2"><hr size="1px" style="width:100%" color="black" /></td></tr>';
        shtml +=  '                    </table>';
        shtml +=  '                    </td>';
        shtml +=  '                    <td style="width:50%; height: 90px" valign="top" align="right">';
        shtml +=  '                    <table class="stable" border="0" cellpadding="0" cellspacing="0" style="width: 346px;" >';
        shtml +=  '                        <tr>';
        shtml +=  '                            <td align="center" style="width: 151px;" ></td>';
        shtml +=  '                            <td align="center" style="width: 140px;"></td>';
        shtml +=  '                            <td style="width: 333px;" align="right">PHP</td>';
        shtml +=  '                        </tr>';
        shtml +=  '                        <tr>';
        shtml +=  '                            <td style="width: 151px; "></td>';
        shtml +=  '                            <td style="width: 140px;"></td>';
        shtml +=  '                            <td style="width: 333px;" align="right">' + HTMLRep.format_numbers(r["Total NettoBeneAmount"],2);
        shtml +=  '                            </td>';
        shtml +=  '                        </tr>';
        shtml +=  '                        <tr><td></td><td colspan="2"><hr size="1px" style="width:100%" color="black" /></td></tr>';
        shtml +=  '                        <tr>';
        shtml +=  '                            <td style="width: 151px;" ></td>';
        shtml +=  '                            <td style="width: 140px;">TOTAL</td>';
        shtml +=  '                            <td style="width: 333px; " align="right">'+ HTMLRep.format_numbers(r["Total NettoBeneAmount"],2) +'</td>';
        shtml +=  '                        </tr>';
        shtml +=  '                        <tr><td></td><td colspan="2"><hr size="1px" style="width:100%" color="black"></td></tr>';
        shtml +=  '                    </table>';
        shtml +=  '                    </td>';
        shtml +=  '                </tr>';
        shtml +=  '                </table>';
        return shtml;
    }
    this.create_sub_detail = function(r)
    {
        var subtotal = 0;
        try
        {
        var shtml = "";
        shtml +=  '<table border="0" style="width:100%">';
        shtml +=  '                <tr>';
        shtml +=  '                <td style="width: 50%">';
        
        shtml +=  '                    <table class="stable" border="0" cellpadding="0" cellspacing="0" style="width: auto; ">';
        shtml +=  '                    <tr>';
        shtml +=  '                        <td align="center" style="width: 140px; height: 24px">Remitting CCY</td>';
        shtml +=  '                        <td style="width: 213px; height: 24px;" align="center">' +  r.RemittingCurrency +'</td>';
        shtml +=  '                    </tr>';
        shtml +=  '                    <tr>';
        shtml +=  '                        <td style="width: 140px; height: 34px">Local Charges</td>';
        shtml +=  '                        <td style="height: 34px; width: 213px;" >' + r.RemittingCurrency + '</td>';
        shtml +=  '                    </tr>';
        shtml +=  '                    <tr>';
        shtml +=  '                        <td align="right" style="width: 140px; height: 34px">Commission:</td>';
        shtml +=  '                        <td style="width: 213px; height: 34px" align="right">' + HTMLRep.format_numbers(r["Total Commission"],2) + '</td>';
        shtml +=  '                    </tr>';
        shtml +=  '                    <tr>';
        shtml +=  '                        <td align="right" style="width: 140px; height: 34px">Cable:</td>';
        shtml +=  '                        <td style="width: 213px; height: 34px" align="right">' + HTMLRep.format_numbers(r["Total Local Cable Charges"],2) + '</td>';
        shtml +=  '                    </tr>';
        
        shtml +=  '                    <tr>';
        shtml +=  '                        <td align="right" style="width: 140px; height: 34px">Back End Charges:</td>';
        shtml +=  '                        <td style="width: 213px; height: 34px" align="right">' + HTMLRep.format_numbers(r["Total Back End Charges Charges"],2) + '</td>';
        shtml +=  '                    </tr>';
        shtml +=  '                     <tr><td></td><td><hr size="1px" style="width:100%" color="black"/></td></tr>';
        
        subtotal = 0;
        subtotal = r["Total Commission"] + r["Total Local Cable Charges"] + r["Total Back End Charges Charges"];
        
        shtml +=  '                    <tr>';
        shtml +=  '                        <td align="right" style="width: 140px; height: 25px">SUB TOTAL</td>';
        shtml +=  '                        <td style="width: 213px; height: 25px;" align="right">' + HTMLRep.format_numbers(subtotal,2) + '</td>';
        shtml +=  '                    </tr>';
        shtml +=  '                </table>';
        
        
        
        shtml +=  '              </td>';
        
        shtml +=  '                <td style="width:50%">';
        
        shtml +=  '                    <table class="stable" border="0" cellpadding="0" cellspacing="0" style="width: auto%; height: 36px">';
        shtml +=  '                    <tr>';
        shtml +=  '                        <td align="center" style="width: 140px; height: 24px"></td>';
        shtml +=  '                        <td style="width: 213px; height: 24px;" align="center"></td>';
        shtml +=  '                    </tr>';
        shtml +=  '                    <tr>';
        shtml +=  '                        <td style="width: 140px; height: 34px">PHP Charges</td>';
        shtml +=  '                        <td style="height: 34px; width: 213px;" ></td>';
        shtml +=  '                    </tr> ';
        shtml +=  '                    <tr> ';
        shtml +=  '                        <td align="right" style="width: 140px; height: 34px">Doc Stamps:</td> ';
        shtml +=  '                        <td style="width: 213px; height: 34px" align="right"> ' + HTMLRep.format_numbers(r["Total Documentrary Stamp"],2) + '</td> ';
        shtml +=  '                    </tr> ';
        shtml +=  '                    <tr> ';
        shtml +=  '                        <td align="right" style="width: 140px; height: 34px">OPE:</td> ';
        shtml +=  '                        <td style="width: 213px; height: 34px" align="right"> ' + HTMLRep.format_numbers(r["Total OPE"],2) + ' </td> ';
        shtml +=  '                    </tr> ';
        shtml +=  '                    <tr> ';
        shtml +=  '                        <td align="right" style="width: 140px; height: 34px">Service Fee:</td> ';
        shtml +=  '                        <td style="width: 213px; height: 34px" align="right"> ' + HTMLRep.format_numbers(r["Total Service Fee"],2) + '</td> ';
        shtml +=  '                    </tr> ';
        
        
        subtotal = 0;
        var tds     =   r["Total Documentrary Stamp"].toString();
        var tope    =   r["Total OPE"].toString();
        var tsf     =   r["Total Service Fee"].toString();
        subtotal =      tds + tope + tsf;
        
        
        shtml +=  '                     <tr><td></td><td><hr size="1px" style="width:100%" color="black"/></td></tr>';
        shtml +=  '                    <tr> ';
        shtml +=  '                        <td align="right" style="width: 140px; height: 25px">SUB TOTAL</td> ';
        shtml +=  '                        <td style="width: 213px; height: 25px;" align="right"> ' + HTMLRep.format_numbers(subtotal,2);
        shtml +=  '                        </td> ';
        shtml +=  '                    </tr> ';
        shtml +=  '                </table> ';
      
        shtml +=  '                </td> ';
        shtml +=  '            </tr> ';
        shtml +=  '            </table>';
        }
        catch(e)
        {
            alert(e);
            alert(shtml);
        }
        
        
        return shtml;
    }
    this.create_grand_total_charges = function()
    {
        var shtml = "";
        shtml += '<table class="stable" border="0" style="width:100%">';
        shtml += '<tr><td colspan=2" style="width:100%"><hr size="1px" style="width:100%" color="black" /></td></tr>';
        shtml += '<tr><td style="width:50%">';
        shtml += '  <table class="stable" border="0">';
        var i = 0;
        if (this.group_total_charges.Rows.length > 0 )
        {
            
            shtml += '  <tr><td class="colheader" nowrap>TOTAL LOCAL CHARGES &nbsp;&nbsp;</td>';
            for(i = 0;i < this.group_total_charges.Rows.length; i++)
            {
                shtml += '  <td>' +  this.group_total_charges.Rows[i].currency + '</td>';
            }
            shtml += '  </tr>';
            shtml += '  <tr><td></td>';
            for(i = 0;i < this.group_total_charges.Rows.length; i++)
            {
                shtml += '  <td>' +  HTMLRep.format_numbers(this.group_total_charges.Rows[i].amount,2) + '</td>';
            }
            shtml += '  </tr>';
        }
        else
        {
           shtml += '<tr><td></td></tr>';
        }
        shtml += '  </table>';
        shtml += '  </td>';
        shtml += '  <td style="width:50%" nowrap class="colheader">TOTAL PESO CHARGES &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + HTMLRep.format_numbers(this.total_charges_peso,2);
        shtml += '  </td>';
        shtml += '</tr></table>';
        
        return shtml;
        
    }
    this.create_grand_total = function()
    {
        var shtml = "";
        shtml += '<table class="stable" border="0" style="width:100%">';
        shtml += '<tr><td class="colheader" style="width:100px">GRAND TOTAL</td><td>';
        shtml += '  <table class="stable" border="0">';
        shtml += '  <tr>';
        var i = 0;
        if (this.group_total.Rows.length > 0 )
        {
            for(i = 0;i < this.group_total.Rows.length; i++)
            {
                shtml += '  <td>' +  this.group_total.Rows[i].currency + '</td>';
            }
            shtml += '  </tr>';
            shtml += '  <tr>';
            for(i = 0;i < this.group_total.Rows.length; i++)
            {
                shtml += '  <td>' +  HTMLRep.format_numbers(this.group_total.Rows[i].amount,2) + '</td>';
            }
        }
        else
        {
           shtml += '<td></td>';
        }
        shtml += '  </tr></table>';
        shtml += '  </td></tr></table>';
        
        return shtml;
        
    }
    this.create_report_footer = function(dt)
    {
    }
}