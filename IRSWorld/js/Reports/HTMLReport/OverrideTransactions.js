var reportholder = new HTMLOverrideTransactionHolder();

function HTMLOverrideTransactionHolder()
{
    this.load_report = function()
    {
        var qstr = HTMLRep.get_hidden_querystring("hidQueryString");
        
        if (qstr.length > 0)
        {                        
            if (qstr.toLowerCase().indexOf("sp_rpt_overridetransactionreport") > -1  )
            {
                var over = new HTMLOverrideTransaction();
                over.evaluate_querystring(qstr);
                return;
            }
            if (qstr.toLowerCase().indexOf("sp_rpt_remitterexception") > -1  )
            {
                var over = new HTMLRemitterException();
                over.evaluate_querystring(qstr);
                return;
            }
            if (qstr.toLowerCase().indexOf("irsw_get_payout_allowed_currency") > -1  )
            {
                var over = new HTMLAllowedPayCurr();
                over.evaluate_querystring(qstr);
                return;
            }
            
            if (qstr.toLowerCase().indexOf("proc_remitter_transaction_history") > -1  )
            {
                var over = new HTMLRemitterTranHistory();
                over.evaluate_querystring(qstr);
                return;
            }
        }  
    }
}

function HTMLOverrideTransaction()
{
    this.evaluate_querystring = function(query)
    {
        var squery = unescape(query.toString());
        var reporttype = 1;     
        this.get_data(                      
                        HTMLRep.parse_merge_param_query(squery,"T@SDATE"),
                        HTMLRep.parse_merge_param_query(squery,"T@EDATE"),
                        HTMLRep.parse_merge_param_query(squery,"T@BranchCode")
                     )
        return;
    }
    this.get_data = function(sdate,edate,branchcode)
    {
        var params;
        var sp_name;
       
        sp_name = "sp_rpt_OverrideTransactionReport";      
        _sdate = "SDATE=" + sdate;
        _edate = "EDATE=" + edate;
        _branchcode =   "@BRANCHCODE="  + branchcode;    
        
        params  =  _sdate + "^" +  _edate + "^" + _branchcode;
        
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
            var ds = res.value
            cls.display_report(ds.Tables[0]);
        }
        catch(e)
        {
            alert( " " + e );
        }        
    }
    this.display_report = function(dt)
    {
        var vd = window.location.href.split('/')[3];
        var imgs =  '<img src="../../../'+ vd +'/images/pnb_black_logo.JPG" style="width: 58px; height: 68px" />'
        
        var shtml = "";     
       
        shtml +=  '<div>';
        shtml +=  '    <table border="0" cellpadding="0" cellspacing="0" style="width: 11in; height: 8px">';
        shtml +=  '        <tr>';
        shtml +=  '            <td align="center" style="width: 100%; height: 92px">';
        shtml +=  '                <table border="0" cellpadding="0" cellspacing="0" style="width: 38%; height: 36px">';
        shtml +=  '                    <tr>';
        shtml +=  '                        <td rowspan="5" style="width: 82px">';
        shtml +=   imgs; 
        shtml +=  '                        </td>';
        shtml +=  '                        <td style="width: 252px; text-align: center">';
        shtml +=  '                            <strong><span style="font-size: 10pt; font-family: Arial">PHILIPPINE NATIONAL BANK</span></strong></td>';
        shtml +=  '                    </tr>';
        shtml +=  '                    <tr style="font-size: 12pt; font-family: Times New Roman">';
        shtml +=  '                        <td style="width: 252px; height: 19px; text-align: center">';
        shtml +=  '                            <span style="font-size: 10pt; font-family: Arial"><strong>' + dt.Rows[0].RemittingBranch + '</strong></span></td>';
        shtml +=  '                    </tr>';
        shtml +=  '                    <tr style="font-size: 12pt; font-family: Times New Roman">';
        shtml +=  '                        <td style="width: 252px; height: 19px; text-align: center">';
        shtml +=  '                            <span style="font-size: 10pt; font-family: Arial"><strong>IRSWorld</strong></span></td>';
        shtml +=  '                    </tr>';
        shtml +=  '                    <tr>';
        shtml +=  '                        <td style="width: 252px">';
        shtml +=  '                            <span style="font-size: 9pt; font-family: Arial"><strong>Override Transaction Report</strong></span></td>';
        shtml +=  '                    </tr>';
        shtml +=  '                    <tr>';
        shtml +=  '                        <td style="width: 252px">';
        shtml +=  '                        </td>';
        shtml +=  '                    </tr>';
        shtml +=  '                    <tr>';
        shtml +=  '                        <td align="center" colspan="2" rowspan="1">';
        shtml +=  '                            <span style="font-size: 10pt; font-family: Arial"><strong>Date from '+ HTMLRep.formatDate(dt.Rows[0].StartDate,"/") +' to ';
        shtml +=  '                                ' +  HTMLRep.formatDate(dt.Rows[i].EndDate,"/") +' </strong></span></td>';
        shtml +=  '                    </tr>';
        shtml +=  '                </table>';
        shtml +=  '            </td>';
        shtml +=  '        </tr>';
        shtml +=  '        <tr>';
        shtml +=  '            <td align="center" style="width: 100%; height: 9px">';
        shtml +=  '            </td>';
        shtml +=  '        </tr>';
        shtml +=  '        <tr>';
        shtml +=  '            <td style="width: 1057px; height: 19px">';
        shtml +=  '                <table cellpadding="0" cellspacing="0" style="border-top-width: thin; border-left-width: thin;';
        shtml +=  '                    border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
        shtml +=  '                    width: 100%; border-top-color: black; font-family: Arial; border-right-width: thin;';
        shtml +=  '                    border-right-color: black">';
        shtml +=  '                    <tr>';
        shtml +=  '                        <td colspan="9" style="border-top-width: thin; font-weight: bold; border-left-width: thin;';
        shtml +=  '                            font-size: 9pt; border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
        shtml +=  '                            border-top-color: black; font-family: Arial; text-align: center; border-right-width: thin;';
        shtml +=  '                            border-right-color: black" valign="bottom">';
        shtml +=  '                            <hr style="border-right: black thin solid; border-top: black thin solid; border-left: black thin solid;';
        shtml +=  '                                border-bottom: black thin solid" />';
        shtml +=  '                        </td>';
        shtml +=  '                    </tr>';
        shtml +=  '                    <tr>';
        shtml +=  '                        <td style="border-top-width: thin; font-weight: bold; border-left-width: thin; font-size: 9pt;';
        shtml +=  '                            border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
        shtml +=  '                            border-top-color: black; font-family: Arial; height: 30px; text-align: center;';
        shtml +=  '                            border-right-width: thin; border-right-color: black">';
        shtml +=  '                            TRANSACTION';
        shtml +=  '                            <br />';
        shtml +=  '                            DATE</td>';
        shtml +=  '                        <td style="border-top-width: thin; font-weight: bold; border-left-width: thin; font-size: 9pt;';
        shtml +=  '                            border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
        shtml +=  '                            border-top-color: black; font-family: Arial; height: 30px; text-align: center;';
        shtml +=  '                            border-right-width: thin; border-right-color: black">';
        shtml +=  '                            TRANSACTION';
        shtml +=  '                            <br />';
        shtml +=  '                            REF NO</td>';
        shtml +=  '                        <td style="border-top-width: thin; font-weight: bold; border-left-width: thin; font-size: 9pt;';
        shtml +=  '                            border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
        shtml +=  '                            border-top-color: black; font-family: Arial; height: 30px; text-align: center;';
        shtml +=  '                            border-right-width: thin; border-right-color: black">';
        shtml +=  '                            REMITTER NAME</td>';
        shtml +=  '                        <td style="border-top-width: thin; font-weight: bold; border-left-width: thin; font-size: 9pt;';
        shtml +=  '                            border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
        shtml +=  '                            border-top-color: black; font-family: Arial; height: 30px; text-align: center;';
        shtml +=  '                            border-right-width: thin; border-right-color: black">';
        shtml +=  '                            BENEFICIARY NAME</td>';
        shtml +=  '                        <td style="border-top-width: thin; font-weight: bold; border-left-width: thin; font-size: 9pt;';
        shtml +=  '                            border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
        shtml +=  '                            border-top-color: black; font-family: Arial; height: 30px; text-align: center;';
        shtml +=  '                            border-right-width: thin; border-right-color: black">';
        shtml +=  '                            ACCOUNT';
        shtml +=  '                            <br />';
        shtml +=  '                            NUMBER</td>';
        shtml +=  '                        <td style="border-top-width: thin; font-weight: bold; border-left-width: thin; font-size: 9pt;';
        shtml +=  '                            border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
        shtml +=  '                            border-top-color: black; font-family: Arial; height: 30px; text-align: center;';
        shtml +=  '                            border-right-width: thin; border-right-color: black">';
        shtml +=  '                            CURRENCY</td>';
        shtml +=  '                        <td style="border-top-width: thin; font-weight: bold; border-left-width: thin; font-size: 9pt;';
        shtml +=  '                            border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
        shtml +=  '                            border-top-color: black; font-family: Arial; height: 30px; text-align: center;';
        shtml +=  '                            border-right-width: thin; border-right-color: black">';
        shtml +=  '                            GROSS';
        shtml +=  '                            <br />';
        shtml +=  '                            AMOUNT</td>';
        shtml +=  '                        <td style="border-top-width: thin; font-weight: bold; border-left-width: thin; font-size: 9pt;';
        shtml +=  '                            border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
        shtml +=  '                            border-top-color: black; font-family: Arial; height: 30px; text-align: center;';
        shtml +=  '                            border-right-width: thin; border-right-color: black">';
        shtml +=  '                            OVERRIDING';
        shtml +=  '                            <br />';
        shtml +=  '                            OFFICERS</td>';
        shtml +=  '                        <td style="border-top-width: thin; font-weight: bold; border-left-width: thin; font-size: 9pt;';
        shtml +=  '                            border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
        shtml +=  '                            border-top-color: black; font-family: Arial; height: 30px; text-align: center;';
        shtml +=  '                            border-right-width: thin; border-right-color: black">';
        shtml +=  '                            NATURE OF';
        shtml +=  '                            <br />';
        shtml +=  '                            TRANSACTION</td>';
        shtml +=  '                    </tr>';
        shtml +=  '                    <tr>';
        shtml +=  '                        <td colspan="9" style="border-top-width: thin; font-weight: bold; border-left-width: thin;';
        shtml +=  '                            font-size: 9pt; border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
        shtml +=  '                            border-top-color: black; font-family: Arial; height: 15px; text-align: center;';
        shtml +=  '                            border-right-width: thin; border-right-color: black" valign="top">';
        shtml +=  '                            <hr style="border-right: black thin solid; border-top: black thin solid; border-left: black thin solid;';
        shtml +=  '                                border-bottom: black thin solid" />';
        shtml +=  '                            &nbsp;</td>';
        shtml +=  '                    </tr>';
        shtml +=  '                </table>';
        shtml +=  '                <table cellpadding="0" cellspacing="0" style="border-top-width: thin; border-left-width: thin;';
        shtml +=  '                    border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
        shtml +=  '                    width: 100%; border-top-color: black; font-family: Arial; border-right-width: thin;';
        shtml +=  '                    border-right-color: black">';
        
        for(i=0;i<dt.Rows.length;i++)
        {
            shtml +=  '                    <tr>';
            shtml +=  '                        <td style="border-top-width: thin; border-left-width: thin; font-size: 9pt;';
            shtml +=  '                            border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
            shtml +=  '                            border-top-color: black; font-family: Arial; height: 30px; text-align: center;';
            shtml +=  '                            border-right-width: thin; border-right-color: black">';
            
            shtml +=    dt.Rows[i].ReferenceNo ;            
            shtml +=  '                         </td>';
            shtml +=  '                        <td style="border-top-width: thin; border-left-width: thin; font-size: 9pt;';
            shtml +=  '                            border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
            shtml +=  '                            border-top-color: black; font-family: Arial; height: 30px; text-align: center;';
            shtml +=  '                            border-right-width: thin; border-right-color: black">';
            
            shtml +=    dt.Rows[i].RemitterName;            
            shtml +=  '                         </td>';
            
            shtml +=  '                        <td style="border-top-width: thin; border-left-width: thin; font-size: 9pt;';
            shtml +=  '                            border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
            shtml +=  '                            border-top-color: black; font-family: Arial; height: 30px; text-align: center;';
            shtml +=  '                            border-right-width: thin; border-right-color: black">';
            shtml +=    dt.Rows[i].BeneficiaryName;            
            shtml +=  '                         </td>';
            
            shtml +=  '                        <td style="border-top-width: thin; border-left-width: thin; font-size: 9pt;';
            shtml +=  '                            border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
            shtml +=  '                            border-top-color: black; font-family: Arial; height: 30px; text-align: center;';
            shtml +=  '                            border-right-width: thin; border-right-color: black">';
            shtml +=    dt.Rows[i].AccountNo;            
            shtml +=  '                         </td>';
            
            shtml +=  '                        <td style="border-top-width: thin; border-left-width: thin; font-size: 9pt;';
            shtml +=  '                            border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
            shtml +=  '                            border-top-color: black; font-family: Arial; height: 30px; text-align: center;';
            shtml +=  '                            border-right-width: thin; border-right-color: black">';
            shtml +=    dt.Rows[i].RemittingCurrency;            
            shtml +=  '                         </td>';
            
            shtml +=  '                        <td style="border-top-width: thin; border-left-width: thin; font-size: 9pt;';
            shtml +=  '                            border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
            shtml +=  '                            border-top-color: black; font-family: Arial; height: 30px; text-align: center;';
            shtml +=  '                            border-right-width: thin; border-right-color: black">';
            shtml +=    HTMLRep.format_numbers(dt.Rows[i].GrossAmount,2);            
            shtml +=  '                         </td>';
            
            shtml +=  '                        <td style="border-top-width: thin; border-left-width: thin; font-size: 9pt;';
            shtml +=  '                            border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
            shtml +=  '                            border-top-color: black; font-family: Arial; height: 30px; text-align: center;';
            shtml +=  '                            border-right-width: thin; border-right-color: black">';
            shtml +=    dt.Rows[i].OverridingOfficer;            
            shtml +=  '                         </td>';
            
            shtml +=  '                        <td style="border-top-width: thin; border-left-width: thin; font-size: 9pt;';
            shtml +=  '                            border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
            shtml +=  '                            border-top-color: black; font-family: Arial; height: 30px; text-align: center;';
            shtml +=  '                            border-right-width: thin; border-right-color: black">';
            shtml +=    dt.Rows[i].OverrideType;            
            shtml +=  '                         </td>';
            shtml +=  '                    </tr>';
        }
        shtml +=  '                </table>';
        shtml +=  '            </td>';
        shtml +=  '        </tr>';
        shtml +=  '        <tr>';
        shtml +=  '            <td style="width: 1057px; height: 19px">';
        shtml +=  '            </td>';
        shtml +=  '        </tr>';
        shtml +=  '    </table>';
shtml +=  '</div>';

        
        document.getElementById("bodydiv").innerHTML = shtml;
    }
}
function HTMLRemitterException()
{
    this.evaluate_querystring = function(query)
    {
        var squery = unescape(query.toString());
        var reporttype = 1;     
        this.get_data(                      
                        HTMLRep.parse_merge_param_query(squery,"T@SDATE"),
                        HTMLRep.parse_merge_param_query(squery,"T@EDATE"),
                        HTMLRep.parse_merge_param_query(squery,"T@BranchCode")
                     )
        return;
    }
    this.get_data = function(sdate,edate,branchcode)
    {
        var params;
        var sp_name;
       
        sp_name = "sp_rpt_RemitterException";      
        _sdate =    "@SDATE=" + sdate;
        _edate =    "@EDATE=" + edate;
        _branchcode =   "@BRANCHCODE="  + branchcode;    
        
        params  =  _sdate + "^" +  _edate + "^" + _branchcode;
        
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
            var ds = res.value
            cls.display_report(ds.Tables[0]);
        }
        catch(e)
        {
            alert( " " + e );
        }        
    }
    this.display_report = function(dt)
    {
        var vd = window.location.href.split('/')[3];
        var imgs =  '<img src="../../../'+ vd +'/images/pnb_black_logo.JPG" style="width: 58px; height: 68px" />'
        
        var shtml = "";
        
        shtml +=  '<div>';
        shtml +=  '    <table border="0" cellpadding="0" cellspacing="0" style="width: 11in; height: 8px">';
        shtml +=  '        <tr>';
        shtml +=  '            <td align="center" style="width: 100%; height: 92px">';
        shtml +=  '                <table border="0" cellpadding="0" cellspacing="0" style="width: 38%; height: 36px">';
        shtml +=  '                    <tr>';
        shtml +=  '                        <td rowspan="5" style="width: 82px">';
        
        shtml +=    imgs;
        
        shtml +=  '                        </td>';
        shtml +=  '                        <td style="width: 252px; text-align: center">';
        shtml +=  '                            <strong><span style="font-size: 10pt; font-family: Arial">PHILIPPINE NATIONAL BANK</span></strong></td>';
        shtml +=  '                    </tr>';
        shtml +=  '                    <tr style="font-size: 12pt; font-family: Times New Roman">';
        shtml +=  '                        <td style="width: 252px; height: 19px; text-align: center">';
        shtml +=  '                            <span style="font-size: 10pt; font-family: Arial"><strong>Branch Here</strong></span></td>';
        shtml +=  '                    </tr>';
        shtml +=  '                    <tr style="font-size: 12pt; font-family: Times New Roman">';
        shtml +=  '                        <td style="width: 252px; height: 19px; text-align: center">';
        shtml +=  '                            <span style="font-size: 10pt; font-family: Arial"><strong>IRSWorld</strong></span></td>';
        shtml +=  '                    </tr>';
        shtml +=  '                    <tr>';
        shtml +=  '                        <td style="width: 252px">';
        shtml +=  '                            <span style="font-size: 9pt; font-family: Arial"><strong>Remitter Exception Report</strong></span></td>';
        shtml +=  '                    </tr>';
        shtml +=  '                    <tr>';
        shtml +=  '                        <td style="width: 252px">';
        shtml +=  '                        </td>';
        shtml +=  '                    </tr>';
        shtml +=  '                    <tr>';
        shtml +=  '                        <td align="center" colspan="2" rowspan="1">';
        shtml +=  '                            <span style="font-size: 10pt; font-family: Arial"><strong>Transaction from ' + dt.Rows[0].StartDate + ' to';
        shtml +=  '                                ' + dt.Rows[0].EndDate + ' </strong></span></td>';
        shtml +=  '                    </tr>';
        shtml +=  '                </table>';
        shtml +=  '            </td>';
        shtml +=  '        </tr>';
        shtml +=  '        <tr>';
        shtml +=  '            <td align="center" style="width: 100%; height: 9px">';
        shtml +=  '            </td>';
        shtml +=  '        </tr>';
        shtml +=  '        <tr>';
        shtml +=  '            <td style="width: 1057px; height: 19px">';
        shtml +=  '                <table cellpadding="0" cellspacing="0" style="border-top-width: thin; border-left-width: thin;';
        shtml +=  '                    border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
        shtml +=  '                    width: 100%; border-top-color: black; font-family: Arial; border-right-width: thin;';
        shtml +=  '                    border-right-color: black">';
        shtml +=  '                    <tr>';
        shtml +=  '                        <td colspan="8" style="border-top-width: thin; font-weight: bold; border-left-width: thin;';
        shtml +=  '                            font-size: 9pt; border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
        shtml +=  '                            border-top-color: black; font-family: Arial; text-align: center; border-right-width: thin;';
        shtml +=  '                            border-right-color: black" valign="bottom">';
        shtml +=  '                            <hr style="border-right: black thin solid; border-top: black thin solid; border-left: black thin solid;';
        shtml +=  '                                border-bottom: black thin solid" />';
        shtml +=  '                        </td>';
        shtml +=  '                    </tr>';
        shtml +=  '                    <tr>';
        shtml +=  '                        <td style="border-top-width: thin; font-weight: bold; border-left-width: thin; font-size: 9pt;';
        shtml +=  '                            border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
        shtml +=  '                            border-top-color: black; font-family: Arial; height: 30px; text-align: center;';
        shtml +=  '                            border-right-width: thin; border-right-color: black">';
        shtml +=  '                            TRANSACTION';
        shtml +=  '                            <br />';
        shtml +=  '                            DATE</td>';
        shtml +=  '                        <td style="border-top-width: thin; font-weight: bold; border-left-width: thin; font-size: 9pt;';
        shtml +=  '                            border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
        shtml +=  '                            border-top-color: black; font-family: Arial; height: 30px; text-align: center;';
        shtml +=  '                            border-right-width: thin; border-right-color: black">';
        shtml +=  '                            TRANSACTION';
        shtml +=  '                            <br />';
        shtml +=  '                            REF NO</td>';
        
        shtml +=  '                        <td style="border-top-width: thin; font-weight: bold; border-left-width: thin; font-size: 9pt;';
        shtml +=  '                            border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
        shtml +=  '                            border-top-color: black; font-family: Arial; height: 30px; text-align: center;';
        shtml +=  '                            border-right-width: thin; border-right-color: black">';
        shtml +=  '                            REMITTER';
        shtml +=  '                            <br />';
        shtml +=  '                            ID</td>';
        
        shtml +=  '                        <td style="border-top-width: thin; font-weight: bold; border-left-width: thin; font-size: 9pt;';
        shtml +=  '                            border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
        shtml +=  '                            border-top-color: black; font-family: Arial; height: 30px; text-align: center;';
        shtml +=  '                            border-right-width: thin; border-right-color: black">';
        shtml +=  '                            REMITTER NAME</td>';
        shtml +=  '                        <td style="border-top-width: thin; font-weight: bold; border-left-width: thin; font-size: 9pt;';
        shtml +=  '                            border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
        shtml +=  '                            border-top-color: black; font-family: Arial; height: 30px; text-align: center;';
        shtml +=  '                            border-right-width: thin; border-right-color: black">';
        shtml +=  '                            BENEFICIARY NAME</td>';
        shtml +=  '                        <td style="border-top-width: thin; font-weight: bold; border-left-width: thin; font-size: 9pt;';
        shtml +=  '                            border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
        shtml +=  '                            border-top-color: black; font-family: Arial; height: 30px; text-align: center;';
        shtml +=  '                            border-right-width: thin; border-right-color: black">';
        shtml +=  '                            ACCOUNT';
        shtml +=  '                            <br />';
        shtml +=  '                            NUMBER</td>';
        shtml +=  '                        <td style="border-top-width: thin; font-weight: bold; border-left-width: thin; font-size: 9pt;';
        shtml +=  '                            border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
        shtml +=  '                            border-top-color: black; font-family: Arial; height: 30px; text-align: center;';
        shtml +=  '                            border-right-width: thin; border-right-color: black">';
        shtml +=  '                            TRANSACTION<br />';
        shtml +=  '                            CCY</td>';
        shtml +=  '                        <td style="border-top-width: thin; font-weight: bold; border-left-width: thin; font-size: 9pt;';
        shtml +=  '                            border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
        shtml +=  '                            border-top-color: black; font-family: Arial; height: 30px; text-align: center;';
        shtml +=  '                            border-right-width: thin; border-right-color: black">';
        shtml +=  '                            TRANSACTION GROSS';
        shtml +=  '                            <br />';
        shtml +=  '                            AMOUNT</td>';
        shtml +=  '                        <td style="border-top-width: thin; font-weight: bold; border-left-width: thin; font-size: 9pt;';
        shtml +=  '                            border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
        shtml +=  '                            border-top-color: black; font-family: Arial; height: 30px; text-align: center;';
        shtml +=  '                            border-right-width: thin; border-right-color: black">';
        shtml +=  '                            DEFICIT DOCUMENTS / ID</td>';
        shtml +=  '                    </tr>';
        shtml +=  '                    <tr>';
        shtml +=  '                        <td colspan="8" style="border-top-width: thin; font-weight: bold; border-left-width: thin;';
        shtml +=  '                            font-size: 9pt; border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
        shtml +=  '                            border-top-color: black; font-family: Arial; height: 15px; text-align: center;';
        shtml +=  '                            border-right-width: thin; border-right-color: black" valign="top">';
        shtml +=  '                            <hr style="border-right: black thin solid; border-top: black thin solid; border-left: black thin solid;';
        shtml +=  '                                border-bottom: black thin solid" />';
        shtml +=  '                            &nbsp;</td>';
        shtml +=  '                    </tr>';
        shtml +=  '                </table>';
        shtml +=  '                <table cellpadding="0" cellspacing="0" style="border-top-width: thin; border-left-width: thin;';
        shtml +=  '                    border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
        shtml +=  '                    width: 100%; border-top-color: black; font-family: Arial; border-right-width: thin;';
        shtml +=  '                    border-right-color: black">';
        for(i=0;i<dt.Rows.length;i++)
        {
            shtml +=  '                    <tr>';
            shtml +=  '                        <td style="border-top-width: thin; border-left-width: thin; font-size: 9pt;';
            shtml +=  '                            border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
            shtml +=  '                            border-top-color: black; font-family: Arial; height: 30px; text-align: center;';
            shtml +=  '                            border-right-width: thin; border-right-color: black">';
            
            shtml +=    dt.Rows[i].TransactionDate ;            
            shtml +=  '                         </td>';
            
            shtml +=  '                        <td style="border-top-width: thin; border-left-width: thin; font-size: 9pt;';
            shtml +=  '                            border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
            shtml +=  '                            border-top-color: black; font-family: Arial; height: 30px; text-align: center;';
            shtml +=  '                            border-right-width: thin; border-right-color: black">';
            
            shtml +=    dt.Rows[i].ReferenceNo ;            
            shtml +=  '                         </td>';
            
            shtml +=  '                        <td style="border-top-width: thin; border-left-width: thin; font-size: 9pt;';
            shtml +=  '                            border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
            shtml +=  '                            border-top-color: black; font-family: Arial; height: 30px; text-align: center;';
            shtml +=  '                            border-right-width: thin; border-right-color: black">';
            
            shtml +=    dt.Rows[i].RemitterID ;            
            shtml +=  '                         </td>';
            
            shtml +=  '                        <td style="border-top-width: thin; border-left-width: thin; font-size: 9pt;';
            shtml +=  '                            border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
            shtml +=  '                            border-top-color: black; font-family: Arial; height: 30px; text-align: center;';
            shtml +=  '                            border-right-width: thin; border-right-color: black">';
            
            shtml +=    dt.Rows[i].RemitterName ;            
            shtml +=  '                         </td>';
            
            shtml +=  '                        <td style="border-top-width: thin; border-left-width: thin; font-size: 9pt;';
            shtml +=  '                            border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
            shtml +=  '                            border-top-color: black; font-family: Arial; height: 30px; text-align: center;';
            shtml +=  '                            border-right-width: thin; border-right-color: black">';
            
            shtml +=    dt.Rows[i].BeneficiaryName ;            
            shtml +=  '                         </td>';
            
            shtml +=  '                        <td style="border-top-width: thin; border-left-width: thin; font-size: 9pt;';
            shtml +=  '                            border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
            shtml +=  '                            border-top-color: black; font-family: Arial; height: 30px; text-align: center;';
            shtml +=  '                            border-right-width: thin; border-right-color: black">';
            
            shtml +=    dt.Rows[i].RemittingCurrency ;            
            shtml +=  '                         </td>';
            
            shtml +=  '                        <td style="border-top-width: thin; border-left-width: thin; font-size: 9pt;';
            shtml +=  '                            border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
            shtml +=  '                            border-top-color: black; font-family: Arial; height: 30px; text-align: center;';
            shtml +=  '                            border-right-width: thin; border-right-color: black">';
            
            shtml +=    HTMLRep.format_numbers(dt.Rows[i].GrossAmount,2) ;            
            shtml +=  '                         </td>';
            
            shtml +=  '                        <td style="border-top-width: thin; border-left-width: thin; font-size: 9pt;';
            shtml +=  '                            border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
            shtml +=  '                            border-top-color: black; font-family: Arial; height: 30px; text-align: center;';
            shtml +=  '                            border-right-width: thin; border-right-color: black">';
            
            shtml +=    dt.Rows[i].DeficitIDs ;            
            shtml +=  '                         </td>';
            
            shtml +=  '                    </tr>';
        }
        shtml +=  '                </table>';
        shtml +=  '            </td>';
        shtml +=  '        </tr>';
        shtml +=  '        <tr>';
        shtml +=  '            <td style="width: 1057px; height: 19px">';
        shtml +=  '            </td>';
        shtml +=  '        </tr>';
        shtml +=  '    </table>';
        shtml +=  '</div>';

        

        
        document.getElementById("bodydiv").innerHTML = shtml;
    }
}
function HTMLAllowedPayCurr()
{    
    this.evaluate_querystring = function(query)
    {
        var squery = unescape(query.toString());
        var reporttype = 1;     
        this.get_data(                      
                        HTMLRep.parse_merge_param_query(squery,"T@PARAMDATE"),
                        HTMLRep.parse_merge_param_query(squery,"T@BRANCHCODE")
                     )
        return;
    }
    this.get_data = function(paramdate,branchcode)
    {
        var params;
        var sp_name;
       
        sp_name = "sp_rpt_RemitterException";      
        _sdate = "@PARAMDATE=" + paramdate;
        _edate = "@BRANCHCODE=" + branchcode;
        
        
        params  =  _sdate + "^" + _branchcode;
        
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
            var ds = res.value
            cls.display_report(ds.Tables[0]);
        }
        catch(e)
        {
            alert( " " + e );
        }        
    }
    this.display_report = function(dt)
    {
        var vd = window.location.href.split('/')[3];
        var imgs =  '<img src="../../../'+ vd +'/images/pnb_black_logo.JPG" style="width: 58px; height: 68px" />'
        
        var shtml = "";
        
        shtml +=  '<div>';
        shtml +=  '    <table border="0" cellpadding="0" cellspacing="0" style="width: 8in; height: 11px">';
        shtml +=  '        <tr>';
        shtml +=  '            <td align="center" style="width: 100%; height: 22px">';
        shtml +=  '            </td>';
        shtml +=  '        </tr>';
        shtml +=  '        <tr>';
        shtml +=  '            <td align="center" style="width: 100%; height: 92px">';
        shtml +=  '                <table border="0" cellpadding="0" cellspacing="0" style="width: 54%; height: 36px">';
        shtml +=  '                    <tr>';
        shtml +=  '                        <td rowspan="4" style="width: 82px">';
        shtml +=  '                            '+  imgs + '</td>';
        shtml +=  '                        <td style="width: 252px; text-align: center">';
        shtml +=  '                            <strong><span style="font-family: Arial Black">PHILIPPINE NATIONAL BANK</span></strong></td>';
        shtml +=  '                    </tr>';
        shtml +=  '                    <tr style="font-size: 12pt; font-family: Times New Roman">';
        shtml +=  '                        <td style="width: 252px; height: 19px; text-align: center">';
        shtml +=  '                            <span style="font-size: 10pt; font-family: Arial"><strong>IRSWorld APAS PAYOUT Setup</strong></span></td>';
        shtml +=  '                    </tr>';
        shtml +=  '                    <tr style="font-size: 12pt; font-family: Times New Roman">';
        shtml +=  '                        <td style="width: 252px; height: 20px; text-align: center">';
        shtml +=  '                            <span style="font-size: 10pt; font-family: Arial"><strong>Allowed Currency</strong></span></td>';
        shtml +=  '                    </tr>';
        shtml +=  '                    <tr>';
        shtml +=  '                        <td style="width: 252px">';
        shtml +=  '                            ' + HTMLRep.formatDate(dt.Rows[0].paramdate,"/") + '</td>';
        shtml +=  '                    </tr>';
        shtml +=  '                </table>';
        shtml +=  '            </td>';
        shtml +=  '        </tr>';
        shtml +=  '        <tr>';
        shtml +=  '            <td style="width: 1057px; height: 19px">';
        shtml +=  '                <table cellpadding="0" cellspacing="0" style="border-top-width: thin; border-left-width: thin;';
        shtml +=  '                    border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
        shtml +=  '                    width: 100%; border-top-color: black; font-family: Arial; border-right-width: thin;';
        shtml +=  '                    border-right-color: black">';
        shtml +=  '                    <tr>';
        shtml +=  '                        <td colspan="3" style="border-top-width: thin; font-weight: bold; border-left-width: thin;';
        shtml +=  '                            font-size: 9pt; border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
        shtml +=  '                            border-top-color: black; font-family: Arial; height: 40px; text-align: center;';
        shtml +=  '                            border-right-width: thin; border-right-color: black">';
        shtml +=  '                            <table cellpadding="0" cellspacing="0" style="border-top-width: thin; border-left-width: thin;';
        shtml +=  '                                border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
        shtml +=  '                                width: 73%; border-top-color: black; font-family: Arial; border-right-width: thin;';
        shtml +=  '                                border-right-color: black">';
        shtml +=  '                                <tr>';
        shtml +=  '                                    <td style="border-top-width: thin; font-weight: bold; border-left-width: thin; font-size: 9pt;';
        shtml +=  '                                        border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
        shtml +=  '                                        width: 227px; border-top-color: black; font-family: Arial; height: 25px; text-align: center;';
        shtml +=  '                                        border-right-width: thin; border-right-color: black">';
        shtml +=  '                                        Branch Code</td>';
        shtml +=  '                                    <td style="border-top-width: thin; font-weight: bold; border-left-width: thin; font-size: 9pt;';
        shtml +=  '                                        border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
        shtml +=  '                                        width: 233px; border-top-color: black; font-family: Arial; height: 25px; text-align: center;';
        shtml +=  '                                        border-right-width: thin; border-right-color: black">';
        shtml +=  '                                        Branch Name</td>';
        shtml +=  '                                    <td style="border-top-width: thin; font-weight: bold; border-left-width: thin; font-size: 9pt;';
        shtml +=  '                                        border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
        shtml +=  '                                        border-top-color: black; font-family: Arial; height: 25px; text-align: center;';
        shtml +=  '                                        border-right-width: thin; border-right-color: black; width: 103px;">';
        shtml +=  '                                        Currency</td>';
        shtml +=  '                                </tr>';
        shtml +=  '                            </table>';
        shtml +=  '                        </td>';
        shtml +=  '                    </tr>';
        shtml +=  '                    <tr>';
        shtml +=  '                        <td colspan="3" style="border-top-width: thin; font-weight: bold; border-left-width: thin;';
        shtml +=  '                            font-size: 9pt; border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
        shtml +=  '                            border-top-color: black; font-family: Arial; height: 40px; text-align: center;';
        shtml +=  '                            border-right-width: thin; border-right-color: black">';
        shtml +=  '                            <hr style="border-left-color: black; border-bottom-color: black; border-top-style: solid;';
        shtml +=  '                                border-top-color: black; border-right-style: solid; border-left-style: solid;';
        shtml +=  '                                border-right-color: black; border-bottom-style: solid" />';
        shtml +=  '                            &nbsp;</td>';
        shtml +=  '                    </tr>';
        shtml +=  '                    <tr>';
        shtml +=  '                        <td colspan="3" style="border-top-width: thin; font-weight: bold; border-left-width: thin;';
        shtml +=  '                            font-size: 9pt; border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
        shtml +=  '                            border-top-color: black; font-family: Arial; height: 40px; text-align: center;';
        shtml +=  '                            border-right-width: thin; border-right-color: black">';
        shtml +=  '                            <table cellpadding="0" cellspacing="0" style="border-top-width: thin; border-left-width: thin;';
        shtml +=  '                                border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
        shtml +=  '                                width: 73%; border-top-color: black; font-family: Arial; border-right-width: thin;';
        shtml +=  '                                border-right-color: black">';
        for (i=0;i < dt.Rows.length;i++)
        {
            shtml +=  '                                <tr>';
            shtml +=  '                                    <td style="border-top-width: thin; font-weight: bold; border-left-width: thin; font-size: 9pt;';
            shtml +=  '                                        border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
            shtml +=  '                                        width: 232px; border-top-color: black; font-family: Arial; height: 25px; text-align: center;';
            shtml +=  '                                        border-right-width: thin; border-right-color: black">';
            
            shtml +=    dt.Rows[i].BranchCode ;            
            shtml +=  '                         </td>';
            
            shtml +=  '                                    <td style="border-top-width: thin; font-weight: bold; border-left-width: thin; font-size: 9pt;';
            shtml +=  '                                        border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
            shtml +=  '                                        width: 230px; border-top-color: black; font-family: Arial; height: 25px; text-align: center;';
            shtml +=  '                                        border-right-width: thin; border-right-color: black">';
            
            shtml +=    dt.Rows[i].BRANCHNAME ;            
            shtml +=  '                         </td>';
            
            shtml +=  '                                    <td style="border-top-width: thin; font-weight: bold; border-left-width: thin; font-size: 9pt;';
            shtml +=  '                                        border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
            shtml +=  '                                        border-top-color: black; font-family: Arial; height: 25px; text-align: center;';
            shtml +=  '                                        border-right-width: thin; border-right-color: black; width: 105px;">';
            
            shtml +=    dt.Rows[i].Currency ;            
            shtml +=  '                         </td>';
            
            shtml +=  '                                </tr>';        
        }
        shtml +=  '                            </table>';
        shtml +=  '                        </td>';
        shtml +=  '                    </tr>';
        shtml +=  '                </table>';
        shtml +=  '            </td>';
        shtml +=  '        </tr>';
        shtml +=  '        <tr>';
        shtml +=  '            <td style="width: 1057px; height: 19px">';
        shtml +=  '            </td>';
        shtml +=  '        </tr>';
        shtml +=  '    </table>';
        shtml +=  '</div>';


        

        
        document.getElementById("bodydiv").innerHTML = shtml;
    }
}

function HTMLRemitterTranHistory()
{
    var _sdate = "";
    var _vedate = "";
    var _vcifid = "";
    var cls = this;
    this.evaluate_querystring = function(query)
    {
        var squery = unescape(query.toString());
       
        var reporttype = 1;     
        this.get_data(                      
                        HTMLRep.parse_merge_param_query(squery,"T@v_remcifid"),
                        HTMLRep.parse_merge_param_query(squery,"T@v_sdate"),
                        HTMLRep.parse_merge_param_query(squery,"T@v_edate")
                     )
        return;
    }
    this.get_data = function(cifid,sdate,edate)
    {
        var params;
        var sp_name;
       
        sp_name = "proc_remitter_transaction_history";      
        var _sdate =    "@v_sdate=" + sdate;
        var _edate =    "@v_edate=" + edate;
        var _cifid =   "@v_remcifid="  + cifid;    
        
        
        this._vsdate = sdate;
        this._vedate = edate;
        
        params  =   _cifid + "^" + _sdate + "^" +  _edate  ;
        
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
            var ds = res.value
            cls.display_report(ds.Tables[0]);
        }
        catch(e)
        {
            alert( " " + e );
        }        
    }
    this.display_report = function(dt)
    {
        var vd = window.location.href.split('/')[3];
        var imgs =  '<img src="../../../'+ vd +'/images/pnb_black_logo.JPG" style="width: 58px; height: 68px" />'
        
        var shtml = "";
        
        shtml +=  '<div>';
        shtml +=  '    <table border="0" cellpadding="0" cellspacing="0" style="width: 11in; height: 8px">';
        shtml +=  '        <tr>';
        shtml +=  '            <td align="center" style="width: 100%; height: 92px">';
        shtml +=  '                <table border="0" cellpadding="0" cellspacing="0" style="width: 43%; height: 36px">';
        shtml +=  '                    <tr>';
        shtml +=  '                        <td rowspan="5" style="width: 82px">';
        shtml +=    imgs;
        shtml +=  '                        </td>';
        shtml +=  '                        <td style="width: 252px; text-align: center">';
        shtml +=  '                            <strong><span style="font-size: 10pt; font-family: Arial">PHILIPPINE NATIONAL BANK</span></strong></td>';
        shtml +=  '                    </tr>';
        shtml +=  '                    <tr style="font-size: 12pt; font-family: Times New Roman">';
        shtml +=  '                        <td style="width: 252px; height: 19px; text-align: center">';
        shtml +=  '                            <span style="font-size: 10pt; font-family: Arial"><strong></strong></span>';
        shtml +=  '                        </td>';
        shtml +=  '                    </tr>';
        shtml +=  '                    <tr style="font-size: 12pt; font-family: Times New Roman">';
        shtml +=  '                        <td style="width: 252px; height: 19px; text-align: center">';
        shtml +=  '                            <span style="font-size: 10pt; font-family: Arial"><strong>IRSWorld</strong></span>';
        shtml +=  '                        </td>';
        shtml +=  '                    </tr>';
        shtml +=  '                    <tr>';
        shtml +=  '                        <td style="width: 252px">';
        shtml +=  '                            <span style="font-size: 9pt; font-family: Arial"><strong>REMITTER TRANSACTION HISTORY';
        shtml +=  '                                REPORT</strong></span>';
        shtml +=  '                        </td>';
        shtml +=  '                    </tr>';
        shtml +=  '                    <tr>';
        shtml +=  '                        <td style="width: 252px; height: 19px">';
        shtml +=  '                        </td>';
        shtml +=  '                    </tr>';
        shtml +=  '                    <tr>';
        shtml +=  '                        <td align="center" colspan="2" rowspan="1">';
        shtml +=  '                            <span style="font-size: 10pt; font-family: Arial"><strong>From ' + cls._vsdate + ' to';
        shtml +=  '                                ' + cls._vedate + '</strong></span></td>';
        shtml +=  '                    </tr>';
        shtml +=  '                </table>';
        shtml +=  '            </td>';
        shtml +=  '        </tr>';
        shtml +=  '        <tr>';
        shtml +=  '            <td align="center" style="width: 100%; height: 9px">';
        shtml +=  '            </td>';
        shtml +=  '        </tr>';
        shtml +=  '        <tr>';
        shtml +=  '            <td style="width: 1057px; height: 19px">';
        shtml +=  '                <table cellpadding="0" cellspacing="0" style="border-top-width: thin; border-left-width: thin;';
        shtml +=  '                    border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
        shtml +=  '                    width: 100%; border-top-color: black; font-family: Arial; border-right-width: thin;';
        shtml +=  '                    border-right-color: black">';
        shtml +=  '                    <tr>';
        shtml +=  '                        <td colspan="10" style="border-top-width: thin; font-weight: bold; border-left-width: thin;';
        shtml +=  '                            font-size: 9pt; border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
        shtml +=  '                            border-top-color: black; font-family: Arial; text-align: center; border-right-width: thin;';
        shtml +=  '                            border-right-color: black" valign="bottom">';
        shtml +=  '                            <hr style="border-right: black thin solid; border-top: black thin solid; border-left: black thin solid;';
        shtml +=  '                                border-bottom: black thin solid" />';
        shtml +=  '                        </td>';
        shtml +=  '                    </tr>';
        shtml +=  '                    <tr>';
        shtml +=  '                        <td style="border-top-width: thin; font-weight: bold; border-left-width: thin; font-size: 9pt;';
        shtml +=  '                            border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
        shtml +=  '                            border-top-color: black; font-family: Arial; height: 30px; text-align: center;';
        shtml +=  '                            border-right-width: thin; border-right-color: black">';
        shtml +=  '                            REFERENCE NO</td>';
        shtml +=  '                        <td style="border-top-width: thin; font-weight: bold; border-left-width: thin; font-size: 9pt;';
        shtml +=  '                            border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
        shtml +=  '                            border-top-color: black; font-family: Arial; height: 30px; text-align: center;';
        shtml +=  '                            border-right-width: thin; border-right-color: black">';
        shtml +=  '                            TRANSACTION DATE</td>';
        shtml +=  '                        <td style="border-top-width: thin; font-weight: bold; border-left-width: thin; font-size: 9pt;';
        shtml +=  '                            border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
        shtml +=  '                            border-top-color: black; font-family: Arial; height: 30px; text-align: center;';
        shtml +=  '                            border-right-width: thin; border-right-color: black">';
        shtml +=  '                            REMITTER CIF ID</td>';
        shtml +=  '                        <td style="border-top-width: thin; font-weight: bold; border-left-width: thin; font-size: 9pt;';
        shtml +=  '                            border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
        shtml +=  '                            border-top-color: black; font-family: Arial; height: 30px; text-align: center;';
        shtml +=  '                            border-right-width: thin; border-right-color: black">';
        shtml +=  '                            REMITTER NAME</td>';
        shtml +=  '                        <td style="border-top-width: thin; font-weight: bold; border-left-width: thin; font-size: 9pt;';
        shtml +=  '                            border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
        shtml +=  '                            border-top-color: black; font-family: Arial; height: 30px; text-align: center;';
        shtml +=  '                            border-right-width: thin; border-right-color: black">';
        shtml +=  '                            BENEFICIARY NAME</td>';
        shtml +=  '                        <td style="border-top-width: thin; font-weight: bold; border-left-width: thin; font-size: 9pt;';
        shtml +=  '                            border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
        shtml +=  '                            border-top-color: black; font-family: Arial; height: 30px; text-align: center;';
        shtml +=  '                            border-right-width: thin; border-right-color: black">';
        shtml +=  '                            PRODUCT</td>';
        shtml +=  '                        <td style="border-top-width: thin; font-weight: bold; border-left-width: thin; font-size: 9pt;';
        shtml +=  '                            border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
        shtml +=  '                            border-top-color: black; font-family: Arial; height: 30px; text-align: center;';
        shtml +=  '                            border-right-width: thin; border-right-color: black">';
        shtml +=  '                            TRANSACTION STATUS</td>';
        shtml +=  '                        <td style="border-top-width: thin; font-weight: bold; border-left-width: thin; font-size: 9pt;';
        shtml +=  '                            border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
        shtml +=  '                            border-top-color: black; font-family: Arial; height: 30px; text-align: center;';
        shtml +=  '                            border-right-width: thin; border-right-color: black">';
        shtml +=  '                            REMITTING CURRENCY</td>';
        shtml +=  '                        <td style="border-top-width: thin; font-weight: bold; border-left-width: thin; font-size: 9pt;';
        shtml +=  '                            border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
        shtml +=  '                            border-top-color: black; font-family: Arial; height: 30px; text-align: center;';
        shtml +=  '                            border-right-width: thin; border-right-color: black">';
        shtml +=  '                            NET REMITTANCE AMOUNT</td>';
        shtml +=  '                        <td style="border-top-width: thin; font-weight: bold; border-left-width: thin; font-size: 9pt;';
        shtml +=  '                            border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
        shtml +=  '                            border-top-color: black; font-family: Arial; height: 30px; text-align: center;';
        shtml +=  '                            border-right-width: thin; border-right-color: black">';
        shtml +=  '                            SERVICE FEE</td>';
        shtml +=  '                    </tr>';
        shtml +=  '                    <tr>';
        shtml +=  '                        <td colspan="10" style="border-top-width: thin; font-weight: bold; border-left-width: thin;';
        shtml +=  '                            font-size: 9pt; border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
        shtml +=  '                            border-top-color: black; font-family: Arial; height: 30px; text-align: center;';
        shtml +=  '                            border-right-width: thin; border-right-color: black">';
        shtml +=  '                            <hr style="border-right: black thin solid; border-top: black thin solid; border-left: black thin solid;';
        shtml +=  '                                border-bottom: black thin solid" />';
        shtml +=  '                            &nbsp;</td>';
        shtml +=  '                    </tr>';
        for (i=0; i < dt.Rows.length; i++)
        {
            shtml +=  '                    <tr>';
            shtml +=  '                        <td style="border-top-width: thin; border-left-width: thin; font-size: 9pt; border-left-color: black;';
            shtml +=  '                            border-bottom-width: thin; border-bottom-color: black; border-top-color: black;';
            shtml +=  '                            font-family: Arial; height: 30px; text-align: center; border-right-width: thin;';
            shtml +=  '                            border-right-color: black">';
            shtml +=  '                            '+  dt.Rows[i].refno + '</td>';
            shtml +=  '                        <td style="border-top-width: thin; border-left-width: thin; font-size: 9pt; border-left-color: black;';
            shtml +=  '                            border-bottom-width: thin; border-bottom-color: black; border-top-color: black;';
            shtml +=  '                            font-family: Arial; height: 30px; text-align: center; border-right-width: thin;';
            shtml +=  '                            border-right-color: black" nowrap>';
            shtml +=  '                            '+  dt.Rows[i].trandate + '</td>';
            shtml +=  '                        <td style="border-top-width: thin; border-left-width: thin; font-size: 9pt; border-left-color: black;';
            shtml +=  '                            border-bottom-width: thin; border-bottom-color: black; border-top-color: black;';
            shtml +=  '                            font-family: Arial; height: 30px; text-align: center; border-right-width: thin;';
            shtml +=  '                            border-right-color: black" nowrap>';
            shtml +=  '                            '+  dt.Rows[i].remcifid + '</td>';
            shtml +=  '                        <td style="border-top-width: thin; border-left-width: thin; font-size: 9pt; border-left-color: black;';
            shtml +=  '                            border-bottom-width: thin; border-bottom-color: black; border-top-color: black;';
            shtml +=  '                            font-family: Arial; height: 30px; text-align: center; border-right-width: thin;';
            shtml +=  '                            border-right-color: black" nowrap>';
            shtml +=  '                            '+  dt.Rows[i].remname + '</td>';
            shtml +=  '                        <td style="border-top-width: thin; border-left-width: thin; font-size: 9pt; border-left-color: black;';
            shtml +=  '                            border-bottom-width: thin; border-bottom-color: black; border-top-color: black;';
            shtml +=  '                            font-family: Arial; height: 30px; text-align: center; border-right-width: thin;';
            shtml +=  '                            border-right-color: black" nowrap>';
            shtml +=  '                            '+  dt.Rows[i].benename + '</td>';
            shtml +=  '                        <td style="border-top-width: thin; border-left-width: thin; font-size: 9pt; border-left-color: black;';
            shtml +=  '                            border-bottom-width: thin; border-bottom-color: black; border-top-color: black;';
            shtml +=  '                            font-family: Arial; height: 30px; text-align: center; border-right-width: thin;';
            shtml +=  '                            border-right-color: black" nowrap>';
            shtml +=  '                            '+  dt.Rows[i].product + '</td>';
            shtml +=  '                        <td style="border-top-width: thin; border-left-width: thin; font-size: 9pt; border-left-color: black;';
            shtml +=  '                            border-bottom-width: thin; border-bottom-color: black; border-top-color: black;';
            shtml +=  '                            font-family: Arial; height: 30px; text-align: center; border-right-width: thin;';
            shtml +=  '                            border-right-color: black" nowrap>';
            shtml +=  '                            '+  dt.Rows[i].transtatus + '</td>';
            shtml +=  '                        <td style="border-top-width: thin; border-left-width: thin; font-size: 9pt; border-left-color: black;';
            shtml +=  '                            border-bottom-width: thin; border-bottom-color: black; border-top-color: black;';
            shtml +=  '                            font-family: Arial; height: 30px; text-align: center; border-right-width: thin;';
            shtml +=  '                            border-right-color: black" nowrap>';
            shtml +=  '                            '+  dt.Rows[i].remcurr + '</td>';
            shtml +=  '                        <td style="border-top-width: thin; border-left-width: thin; font-size: 9pt; border-left-color: black;';
            shtml +=  '                            border-bottom-width: thin; border-bottom-color: black; border-top-color: black;';
            shtml +=  '                            font-family: Arial; height: 30px; text-align: center; border-right-width: thin;';
            shtml +=  '                            border-right-color: black" nowrap>';
            shtml +=  '                            '+  dt.Rows[i].netremamt + '</td>';
            shtml +=  '                        <td style="border-top-width: thin; border-left-width: thin; font-size: 9pt; border-left-color: black;';
            shtml +=  '                            border-bottom-width: thin; border-bottom-color: black; border-top-color: black;';
            shtml +=  '                            font-family: Arial; height: 30px; text-align: center; border-right-width: thin;';
            shtml +=  '                            border-right-color: black" nowrap>';
            shtml +=  '                            '+  dt.Rows[i].servfee + '</td>';
            shtml +=  '                    </tr>';
        }
        shtml +=  '                    <tr>';
        shtml +=  '                        <td colspan="10" style="border-top-width: thin; font-weight: bold; border-left-width: thin;';
        shtml +=  '                            font-size: 9pt; border-left-color: black; border-bottom-width: thin; border-bottom-color: black;';
        shtml +=  '                            border-top-color: black; font-family: Arial; height: 15px; text-align: center;';
        shtml +=  '                            border-right-width: thin; border-right-color: black" valign="top">';
        shtml +=  '                            &nbsp;</td>';
        shtml +=  '                    </tr>';
        shtml +=  '                </table>';
        shtml +=  '            </td>';
        shtml +=  '        </tr>';
        shtml +=  '        <tr>';
        shtml +=  '            <td style="width: 1057px; height: 19px">';
        shtml +=  '            </td>';
        shtml +=  '        </tr>';
        shtml +=  '    </table>';
        shtml +=  '</div>';

        

        
        document.getElementById("bodydiv").innerHTML = shtml;
    }
}
