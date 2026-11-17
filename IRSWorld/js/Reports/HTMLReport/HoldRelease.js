

var reportholder = new account_rec_report();
 
function account_rec_report()
{
    this.load_report = function()
    {
       var qstr = HTMLRep.get_querystring();
        
        if (qstr.length > 0)
        {
            if (qstr.toLowerCase().indexOf("proc_get_rci_holdrelease_transaction" ) > -1 )
            {
                var orps = new HTMLRCI_Hold_Release_Report();
                orps.evaluate_RCI_Hold_Release_query(qstr);
                return;
            }  
            
        }
    }    
    
}
/* START RCI HOLD AND RELEASE REPORT */
function HTMLRCI_Hold_Release_Report()
{
    var cls = this;
    var _branchcode;
    var _startdate;
    var _enddate;
    
    this.evaluate_RCI_Hold_Release_query = function(query)
    {
        var squery = unescape(query.toString());

        if (squery.toLowerCase().indexOf("proc_get_rci_holdrelease_transaction") > -1)
        {
            
             this.get_RCI_Hold_Release_data(
                                    HTMLRep.parse_merge_param_query(squery,"T@v_branchcode"),
                                    HTMLRep.parse_merge_param_query(squery,"T@v_startdate"),
                                    HTMLRep.parse_merge_param_query(squery,"T@v_enddate")
									)
								
            return;
        }
    }

    this.get_RCI_Hold_Release_data = function(tran_date,BranchCode,CurrCode,telletID)
    {
        var params = ""; 
        var sp_name = "";
        
                _branchcode    =   "@v_branchcode="     + _branchcode;
                _startdate =   " @v_startdate="  + _startdate;    
                _enddate    =   "@v_enddate="     + _enddate;
				        
        sp_name = "proc_get_rci_holdrelease_transaction";
		params  = _branchcode + "^" + _startdate + "^" + _enddate;
		
		
		AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {               
                 
            var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
            alert(errmsg);    return;         
        } 
	    IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params,this.get_RCI_Hold_Release_data_callback);
    }
    
    this.get_RCI_Hold_Release_data_callback = function(result)
    {
        try
        {
            var ds = result.value;
            cls.display_RCI_Hold_Release_report(ds);
            
        }
        catch(e)
        {
            alert("ERROR WHILE GETTING CASH MANAGEMENT DATA");
        }
    }
    
    this.display_RCI_Hold_Release_report = function(ds)
    {
        var shtml = "";        
        
        shtml   +=  HTMLRep.print_html_button();
        shtml   +=  this.RCI_Hold_Release_body(ds.Tables[0],ds.Tables[1],ds.Tables[2],ds.Tables[3]);
        
        document.getElementById("bodydiv").innerHTML = shtml
        //this.Fill_text(ds.Tables[0]);      
		
    }
    this.RCI_Hold_Release_body = function(dt1,dt2,dt3,dt4)
    {
        var vd = window.location.href.split('/')[3];
	    var shtml = "";
shtml +=  '<div>';
shtml +=  '<div>';
shtml +=  '<div>';
shtml +=  '<div>';
shtml +=  '&nbsp;';
shtml +=  '<table style="width: 11in">';
shtml +=  '<tr>';
shtml +=  '<td colspan="3" style="width: 0in">';
shtml +=  '<table style="width: 100%">';
shtml +=  '<tr>';
shtml +=  '<td colspan="3" style="vertical-align: top; height: 15px; text-align: center">';
shtml +=  '<span style="font-family: Arial"><span><span style="font-size: 8pt"></span></span></span>';
shtml +=  '<span style="font-family: Arial"><span><span style="font-size: 8pt">';
shtml +=  '<table style="width: 347px">';
shtml +=  '<tr>';
shtml +=  '<td rowspan="3" style="width: 2px">';
shtml +=  '<img src="logo.JPG" style="width: 59px; height: 75px" /></td>';
shtml +=  '<td colspan="2" style="text-align: center">';
shtml +=  '<strong><span style="font-size: 10pt">PHILIPPINE NATIONAL BANK</span></strong></td>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '<td colspan="2" style="text-align: center">';
shtml +=  'INTEGRATED REMITTANCE SYSTEM WORLD';
shtml +=  '<br />';
shtml +=  'HOLD AND &nbsp;RELEASE TRANSACTION REPORT</td>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '<td style="width: 50%; height: 16px; text-align: center;">';
shtml +=  'Date From</td>';
shtml +=  '<td style="width: 55%; height: 16px; text-align: center;">';
shtml +=  'Date to</td>';
shtml +=  '</tr>';
shtml +=  '</table>';
shtml +=  '</span></span></span>';
shtml +=  '</td>';
shtml +=  '</tr>';
shtml +=  '</table>';
shtml +=  '</td>';
shtml +=  '</tr>';
shtml +=  '</table>';
shtml +=  '<table cellpadding="0" cellspacing="0" style="font-family: arial; height: 1.14in; width: 11in;">';
shtml +=  '<tr>';
shtml +=  '<td colspan="3" style="HEIGHT: 108px; vertical-align: top;">';
shtml +=  '<table style="width: 100%; height: 20px">';
shtml +=  '<tr>';
shtml +=  '<td style="width: 48%; height: 87px; vertical-align: top;">';
shtml +=  '<table style="border-right: black thin solid; border-top: black thin solid; font-size: 8pt;';
shtml +=  'border-left: black thin solid; width: 100%; border-bottom: black thin solid">';
shtml +=  '<tr>';
shtml +=  '<td colspan="4" style="text-align: center">';
shtml +=  '<strong>HOLD and RELEASE TRANSACTION REPORT</strong></td>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '<td colspan="4" style="text-align: center">';
shtml +=  '<strong>USD-PHP HOLD TRANSACTIONS</strong></td>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '<td style="border-bottom-color: black; width: 25%; border-bottom-style: solid">';
shtml +=  '<strong>BRANCH</strong></td>';
shtml +=  '<td style="border-bottom-color: black; width: 25%; border-bottom-style: solid">';
shtml +=  '<strong>ITEMS</strong></td>';
shtml +=  '<td style="border-bottom-color: black; width: 25%; border-bottom-style: solid">';
shtml +=  '<strong>VOLUME</strong></td>';
shtml +=  '<td style="border-bottom-color: black; width: 25%; border-bottom-style: solid">';
shtml +=  '<strong>NET TO BENEFICIARY</strong></td>';
shtml +=  '</tr>';

//loop here
for (i=0;i<dt1.Rows.length;i++)
{
    shtml +=  '<tr>';
    shtml +=  '<td style="width: 91px">';
    shtml +=  '' + dt1.Rows[0].branchname + '</td>';
    shtml +=  '<td style="width: 98px">';
    shtml +=  '' + dt1.Rows[0].items + '</td>';
    shtml +=  '<td>';
    shtml +=  '' + dt1.Rows[0].volume + '</td>';
    shtml +=  '<td>';
    shtml +=  '' + dt1.Rows[0].nettobeneamt + '</td>';
    shtml +=  '</tr>';
}
//loop stop here

//total calculations
shtml +='<tr>';
shtml +='<td style="width: 91px">';
shtml +='TOTAL</td>';
shtml +='<td style="width: 98px">';
shtml +='TOTAL ITEMS</td>';
shtml +='<td>';
shtml +='TOTAL VOLUME</td>';
shtml +='<td>';
shtml +='TOTAL NET 2 BENE</td>';
shtml +='</tr>';
//end total calculations

shtml +=  '<tr>';
shtml +=  '<td colspan="4" style="height: 16px">';
shtml +=  '</td>';
shtml +=  '</tr>';
shtml +=  '</table>';
shtml +=  '<table style="border-right: black thin solid; border-top: black thin solid; font-size: 8pt;';
shtml +=  'border-left: black thin solid; width: 100%; border-bottom: black thin solid">';
shtml +=  '<tr>';
shtml +=  '<td colspan="4" style="text-align: center">';
shtml +=  '<strong>USD-PHP RELEASE TRANSACTIONS</strong></td>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '<td style="border-bottom-color: black; width: 25%; border-bottom-style: solid">';
shtml +=  '<strong>BRANCH</strong></td>';
shtml +=  '<td style="border-bottom-color: black; width: 25%; border-bottom-style: solid">';
shtml +=  '<strong>ITEMS</strong></td>';
shtml +=  '<td style="border-bottom-color: black; width: 25%; border-bottom-style: solid">';
shtml +=  '<strong>VOLUME</strong></td>';
shtml +=  '<td style="border-bottom-color: black; width: 25%; border-bottom-style: solid">';
shtml +=  '<strong>NET TO BENEFICIARY</strong></td>';
shtml +=  '</tr>';

//loop here
for (i=0;i<dt2.Rows.length;i++)
{
    shtml +=  '<tr>';
    shtml +=  '<td style="width: 91px">';
    shtml +=  '' + dt2.Rows[0].branchname + '</td>';
    shtml +=  '<td style="width: 98px">';
    shtml +=  '' + dt2.Rows[0].items + '</td>';
    shtml +=  '<td>';
    shtml +=  '' + dt2.Rows[0].volume + '</td>';
    shtml +=  '<td>';
    shtml +=  '' + dt2.Rows[0].nettobeneamt + '</td>';
    shtml +=  '</tr>';
    }
//loop end

//total calculations
shtml +='<tr>';
shtml +='<td style="width: 91px">';
shtml +='TOTAL</td>';
shtml +='<td style="width: 98px">';
shtml +='TOTAL ITEMS</td>';
shtml +='<td>';
shtml +='TOTAL VOLUME</td>';
shtml +='<td>';
shtml +='TOTAL NET 2 BENE</td>';
shtml +='</tr>';
//end total calculations

shtml +=  '<tr>';
shtml +=  '<td colspan="4">';
shtml +=  '</td>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '<td colspan="3" style="text-align: center">';
shtml +=  '<strong>HOLD RELEASE TRANSACTIONS</strong></td>';
shtml +=  '<td>HR TOTAL USD-PHP</td>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '</tr>';
shtml +=  '</table>';
shtml +=  '</td>';
shtml +=  '<td style="height: 87px">';
shtml +=  '</td>';
shtml +=  '<td style="width: 48%; height: 87px"><table style="border-right: black thin solid; border-top: black thin solid; font-size: 8pt;';
shtml +=  'border-left: black thin solid; width: 100%; border-bottom: black thin solid">';
shtml +=  '<tr>';
shtml +=  '<td colspan="4" style="text-align: center">';
shtml +=  '<strong>HOLD and RELEASE TRANSACTION REPORT</strong></td>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '<td colspan="4" style="text-align: center">';
shtml +=  '<strong>USD-USD HOLD TRANSACTIONS</strong></td>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '<td style="border-bottom-color: black; width: 25%; border-bottom-style: solid">';
shtml +=  '<strong>BRANCH</strong></td>';
shtml +=  '<td style="border-bottom-color: black; width: 25%; border-bottom-style: solid">';
shtml +=  '<strong>ITEMS</strong></td>';
shtml +=  '<td style="border-bottom-color: black; width: 25%; border-bottom-style: solid">';
shtml +=  '<strong>VOLUME</strong></td>';
shtml +=  '<td style="border-bottom-color: black; width: 25%; border-bottom-style: solid">';
shtml +=  '<strong>NET TO BENEFICIARY</strong></td>';
shtml +=  '</tr>';

//loop here
for (i=0;i<dt3.Rows.length;i++)
{
    shtml +=  '<tr>';
    shtml +=  '<td style="width: 91px">';
    shtml +=  '' + dt3.Rows[0].branchname + '</td>';
    shtml +=  '<td style="width: 98px">';
    shtml +=  '' + dt3.Rows[0].items + '</td>';
    shtml +=  '<td>';
    shtml +=  '' + dt3.Rows[0].volume + '</td>';
    shtml +=  '<td>';
    shtml +=  '' + dt3.Rows[0].nettobeneamt + '</td>';
    shtml +=  '</tr>';
    }
//loop ends here

//total calculations
shtml +='<tr>';
shtml +='<td style="width: 91px">';
shtml +='TOTAL</td>';
shtml +='<td style="width: 98px">';
shtml +='TOTAL ITEMS</td>';
shtml +='<td>';
shtml +='TOTAL VOLUME</td>';
shtml +='<td>';
shtml +='TOTAL NET 2 BENE</td>';
shtml +='</tr>';
//end total calculations

shtml +=  '<tr>';
shtml +=  '<td colspan="4" style="height: 16px">';
shtml +=  '</td>';
shtml +=  '</tr>';
shtml +=  '</table>';
shtml +=  '<table style="border-right: black thin solid; border-top: black thin solid; font-size: 8pt;';
shtml +=  'border-left: black thin solid; width: 100%; border-bottom: black thin solid">';
shtml +=  '<tr>';
shtml +=  '<td colspan="4" style="text-align: center">';
shtml +=  '<strong>USD-USD RELEASE TRANSACTIONS</strong></td>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '<td style="border-bottom-color: black; width: 25%; border-bottom-style: solid">';
shtml +=  '<strong>BRANCH</strong></td>';
shtml +=  '<td style="border-bottom-color: black; width: 25%; border-bottom-style: solid">';
shtml +=  '<strong>ITEMS</strong></td>';
shtml +=  '<td style="border-bottom-color: black; width: 25%; border-bottom-style: solid">';
shtml +=  '<strong>VOLUME</strong></td>';
shtml +=  '<td style="border-bottom-color: black; width: 25%; border-bottom-style: solid">';
shtml +=  '<strong>NET TO BENEFICIARY</strong></td>';
shtml +=  '</tr>';

//loop here
for (i=0;i<dt4.Rows.length;i++)
{
    shtml +=  '<tr>';
    shtml +=  '<td style="width: 91px">';
    shtml +=  '' + dt4.Rows[0].branchname + '</td>';
    shtml +=  '<td style="width: 98px">';
    shtml +=  '' + dt4.Rows[0].items + '</td>';
    shtml +=  '<td>';
    shtml +=  '' + dt4.Rows[0].volume + '</td>';
    shtml +=  '<td>';
    shtml +=  '' + dt4.Rows[0].nettobeneamt + '</td>';
    shtml +=  '</tr>';
}
//loop ends here

//total calculations
shtml +='<tr>';
shtml +='<td style="width: 91px">';
shtml +='TOTAL</td>';
shtml +='<td style="width: 98px">';
shtml +='TOTAL ITEMS</td>';
shtml +='<td>';
shtml +='TOTAL VOLUME</td>';
shtml +='<td>';
shtml +='TOTAL NET 2 BENE</td>';
shtml +='</tr>';
//end total calculations

shtml +=  '<tr>';
shtml +=  '<td colspan="4">';
shtml +=  '</td>';
shtml +=  '</tr>';
shtml +=  '<tr>';
shtml +=  '<td colspan="3" style="text-align: center">';
shtml +=  '<strong>HOLD RELEASE TRANSACTIONS</strong></td>';
shtml +=  '<td>HR TOTAL USD-USD</td>';
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
shtml +=  '</div>';
shtml +=  '</div>';
shtml +=  '</div>';
shtml +=  '';
shtml +=  '</div>';

        return shtml;
    }
 }   
/* END RCI HOLD AND RELEASE REPORT */

