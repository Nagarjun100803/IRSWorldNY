

var reportholder = new watchlist_report();
 
function watchlist_report()
{
    this.load_report = function()
    {
       var qstr = HTMLRep.get_querystring();
        
        if (qstr.length > 0)
        {
            if (qstr.toLowerCase().indexOf("proc_display_recordsinwatchlist" ) > -1 )
            {
                var orps = new HTMLWatchlistRecord();
                orps.evaluate_Watchlist_Rec_query(qstr);
                return;
            }  
            if (qstr.toLowerCase().indexOf("proc_display_recordspossiblematch" ) > -1 )
            {
                var orps = new HTMLWatchlistMatch();
                orps.evaluate_WatchlistMatch_query(qstr);
                return;
            }    
        }
    }    
    
}
/* START Watchlist Records REPORT */
function HTMLWatchlistRecord()
{
    var cls = this;
    var _sdate;
    var _branchcode;
    
    
    this.evaluate_Watchlist_Rec_query = function(query)
    {
        var squery = unescape(query.toString());

        if (squery.toLowerCase().indexOf("proc_display_recordsinwatchlist") > -1)
        {
            
             this.get_Watchlist_Rec_data(
                                    HTMLRep.parse_merge_param_query(squery,"T@v_date"),
                                    HTMLRep.parse_merge_param_query(squery,"T@v_branchcode")
									)
								
            return;
        }
    }

    this.get_Watchlist_Rec_data = function(sdate,BranchCode)
    {
        var params = ""; 
        var sp_name = "";
        
                _sdate =   " @v_date="  + sdate;    
                _branchcode    =   "@v_branchcode="     + branchcode;
				        
        sp_name = "proc_display_recordsinwatchlist";
		params  = _sdate + "^" + _branchcode;
		
		
		AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {               
                 
            var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
            alert(errmsg);    return;         
        } 
	    IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params,this.get_Watchlist_Rec_data_callback);
    }
    
    this.get_Watchlist_Rec_data_callback = function(result)
    {
        try
        {
            var ds = result.value;
            cls.display_Watchlist_Rec_report(ds);
            
        }
        catch(e)
        {
            alert("ERROR WHILE GETTING WATCHLIST DATA");
        }
    }
    
    this.display_Watchlist_Rec_report = function(ds)
    {
        var shtml = "";        
        
        shtml   +=  HTMLRep.print_html_button();
        shtml   +=  this.Watchlist_Rec_body(ds.Tables[0]);
        
        document.getElementById("bodydiv").innerHTML = shtml
        //this.Fill_text(ds.Tables[0]);      
		
    }
    this.Watchlist_Rec_body = function(dt)
    {
        var vd = window.location.href.split('/')[3];
	    var shtml = "";
        shtml +=  '<div>';
        shtml +=  '<div>';
        shtml +=  '<table style="width: 8.5in; height: 0pt">';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" style="vertical-align: text-top; height: 0px; text-align: center">';
        shtml +=  '<table style="width: 304px">';
        shtml +=  '<tr>';
        shtml +=  '<td style="vertical-align: top; width: 48px; height: 21px">';
        shtml +=  '<img src="../../../'+ vd +'/images/pnb_black_logo.JPG" style="height: 69px" /></td>';
        shtml +=  '<td style="vertical-align: top; width: 295px; height: 21px; text-align: center">';
        shtml +=  '<table style="font-size: 9pt; width: 100%; font-family: arial">';
        shtml +=  '<tr>';
        shtml +=  '<td>';
        shtml +=  '<span style="font-size: 8pt; font-family: Arial Black"><strong>PHILIPPINE NATIONAL BANK</strong></span></td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td style="height: 16px">';
        shtml +=  '<span style="font-size: 8pt">' + dt.Rows[0].flddescription + '</span></td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td>';
        shtml +=  '<span style="font-size: 7pt">INTERGRATED REMITTANCE SYSTEM</span></td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td>';
        shtml +=  '<span style="font-size: 8pt">Records in Watchlist</span></td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td>';
        shtml +=  '' + dt.Rows[0].sdate + '</td>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="2" style="vertical-align: top; text-align: center">';
        shtml +=  '<table style="font-size: 8pt; width: 100%; font-family: arial">';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="4" rowspan="2">';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '<table border="0" cellpadding="0" style="table-layout: fixed; padding-right: 0px;';
        shtml +=  'padding-left: 0px; font-size: 9pt; padding-bottom: 0px; width: 100%; padding-top: 0px;';
        shtml +=  'font-family: arial; border-collapse: collapse">';
        shtml +=  '<tr>';
        shtml +=  '<td style="padding-right: 0px; padding-left: 0px; border-left-color: black; border-bottom-color: black;';
        shtml +=  'padding-bottom: 0px; width: 93px; border-top-style: solid; border-top-color: black;';
        shtml +=  'padding-top: 0px; border-right-style: solid; border-left-style: solid; height: 18px;';
        shtml +=  'border-right-color: black; border-bottom-style: solid">';
        shtml +=  '<strong>FULLNAME</strong></td>';
        shtml +=  '<td style="padding-right: 0px; padding-left: 0px; border-left-color: black; border-bottom-color: black;';
        shtml +=  'padding-bottom: 0px; width: 93px; border-top-style: solid; border-top-color: black;';
        shtml +=  'padding-top: 0px; border-right-style: solid; border-left-style: solid; height: 18px;';
        shtml +=  'border-right-color: black; border-bottom-style: solid">';
        shtml +=  '<strong style="width: 15%">Nationality</strong></td>';
        shtml +=  '<td style="padding-right: 0px; padding-left: 0px; border-left-color: black; border-bottom-color: black;';
        shtml +=  'padding-bottom: 0px; width: 121px; border-top-style: solid; border-top-color: black;';
        shtml +=  'padding-top: 0px; border-right-style: solid; border-left-style: solid; height: 18px;';
        shtml +=  'border-right-color: black; border-bottom-style: solid">';
        shtml +=  '<strong>Date of Registration</strong></td>';
        shtml +=  '<td style="padding-right: 0px; padding-left: 0px; border-left-color: black; border-bottom-color: black;';
        shtml +=  'padding-bottom: 0px; width: 107px; border-top-style: solid; border-top-color: black;';
        shtml +=  'padding-top: 0px; border-right-style: solid; border-left-style: solid; height: 18px;';
        shtml +=  'border-right-color: black; border-bottom-style: solid">';
        shtml +=  '<strong>Regulation No.</strong></td>';
        shtml +=  '<td style="padding-right: 0px; padding-left: 0px; border-left-color: black; border-bottom-color: black;';
        shtml +=  'padding-bottom: 0px; width: 93px; border-top-style: solid; border-top-color: black;';
        shtml +=  'padding-top: 0px; border-right-style: solid; border-left-style: solid; height: 18px;';
        shtml +=  'border-right-color: black; border-bottom-style: solid">';
        shtml +=  '<strong>REMARKS</strong></td>';
        shtml +=  '<td style="padding-right: 0px; padding-left: 0px; border-left-color: black; border-bottom-color: black;';
        shtml +=  'padding-bottom: 0px; width: 93px; border-top-style: solid; border-top-color: black;';
        shtml +=  'padding-top: 0px; border-right-style: solid; border-left-style: solid; height: 18px;';
        shtml +=  'border-right-color: black; border-bottom-style: solid">';
        shtml +=  '<strong>REGULATORY</strong></td>';
        shtml +=  '</tr>';
        for (i=0;i<dt.Rows.length;i++)
        {
        shtml +=  '<tr>';
        shtml +=  '<td style="height: 15px">';
        shtml +=  '' + dt.Rows[0].FullName + '</td>';
        shtml +=  '<td style="width: 84px; height: 15px">';
        shtml +=  '' + dt.Rows[0].Nationality + '</td>';
        shtml +=  '<td style="width: 121px; height: 15px">';
        shtml +=  '' + dt.Rows[0].DateofRegistration + '</td>';
        shtml +=  '<td style="width: 107px; height: 15px">';
        shtml +=  '' + dt.Rows[0].regulationnum + '</td>';
        shtml +=  '<td style="width: 195px; height: 15px">';
        shtml +=  '' + dt.Rows[0].Remarks + '</td>';
        shtml +=  '<td style="height: 15px">';
        shtml +=  '' + dt.Rows[0].regulatory + '</td>';
        shtml +=  '</tr>';
        }
        shtml +=  '</table>';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '</div>';
        shtml +=  '';
        shtml +=  '</div>';

        return shtml;
    }
 }   
/* END Watchlist Record REPORT */

/*Start Watchlist Matching */
function HTMLWatchlistMatch()
{
    var cls = this;
    var _sdate;
    
    this.evaluate_WatchlistMatch_query = function(query)
    {
        var squery = unescape(query.toString());

        if (squery.toLowerCase().indexOf("proc_display_recordspossiblematch") > -1)
        {
            
             this.get_WatchlistMatch_data(
                                    HTMLRep.parse_merge_param_query(squery,"T@v_date"),
                                    HTMLRep.parse_merge_param_query(squery,"T@v_branchcode")
									)
								
            return;
        }
    }

    this.get_WatchlistMatch_data = function(sdate)
    {
        var params = ""; 
        var sp_name = "";
        
                _sdate =   " @v_date="  + sdate;    
                _branchcode    =   "@v_branchcode="     + branchcode;
				        
        sp_name = "proc_display_recordspossiblematch";
		params  = _sdate + "^" + _branchcode;
		
		
		AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {               
                 
            var errmsg = 'SERVER NOT RESPONDING. ERROR GETTING DATA';
            alert(errmsg);    
            return;         
        } 
	    IRSWorld.Reports.HTMLReport.HTML_Reports.get_dataset(sp_name,params,this.get_WatchlistMatch_data_callback);
    }
    
    this.get_WatchlistMatch_data_callback = function(result)
    {
        try
        {
            var ds = result.value;
            cls.display_WatchlistMatch_report(ds);
            
        }
        catch(e)
        {
            alert("ERROR WHILE GETTING RCI Daily Total DATA");
        }
    }
    
    this.display_WatchlistMatch_report = function(ds)
    {
        var shtml = "";        
        
        shtml   +=  HTMLRep.print_html_button();
        shtml   +=  this.WatchlistMatch_body(ds.Tables[0]);
        
        document.getElementById("bodydiv").innerHTML = shtml
        //this.Fill_text(ds.Tables[0]);      
		
    }

this.WatchlistMatch_body = function(dt)
    {
        var vd = window.location.href.split('/')[3];
	    var shtml = "";
        shtml +=  '<div>';
        shtml +=  '<table style="width: 11in; height: 0pt">';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="3" style="vertical-align: text-top; height: 0px; text-align: center">';
        shtml +=  '<table style="width: 304px">';
        shtml +=  '<tr>';
        shtml +=  '<td style="vertical-align: top; width: 48px; height: 21px">';
        shtml +=  '<img src="../../../'+ vd +'/images/pnb_black_logo.JPG" style="height: 69px" /></td>';
        shtml +=  '<td style="vertical-align: top; width: 295px; height: 21px; text-align: center">';
        shtml +=  '<table style="font-size: 9pt; width: 100%; font-family: arial">';
        shtml +=  '<tr>';
        shtml +=  '<td>';
        shtml +=  '<span style="font-size: 8pt; font-family: Arial Black"><strong>PHILIPPINE NATIONAL BANK</strong></span></td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td style="height: 16px">';
        shtml +=  '<span style="font-size: 8pt">remitting branch here</span></td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td>';
        shtml +=  '<span style="font-size: 7pt">INTERGRATED REMITTANCE SYSTEM</span></td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td>';
        shtml +=  '<span style="font-size: 8pt">Report on Possible Match</span></td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td>';
        shtml +=  'start date here</td>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="2" style="vertical-align: top; text-align: center">';
        shtml +=  '<table style="font-size: 8pt; width: 100%; font-family: arial">';
        shtml +=  '<tr>';
        shtml +=  '<td colspan="4" rowspan="2">';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '<tr>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '<table border="0" cellpadding="0" style="font-size: 9pt; width: 100%; font-family: arial; table-layout: fixed; padding-right: 0px; padding-left: 0px; padding-bottom: 0px; padding-top: 0px; border-collapse: collapse;">';
        shtml +=  '<tr>';
        shtml +=  '<td style="border-left-color: black; border-bottom-color: black; width: 93px; border-top-style: solid;';
        shtml +=  'border-top-color: black; border-right-style: solid; border-left-style: solid;';
        shtml +=  'height: 18px; border-right-color: black; border-bottom-style: solid; padding-right: 0px; padding-left: 0px; padding-bottom: 0px; padding-top: 0px;">';
        shtml +=  '<strong>Name</strong></td>';
        shtml +=  '<td style="border-left-color: black; border-bottom-color: black; width: 93px; border-top-style: solid;';
        shtml +=  'border-top-color: black; border-right-style: solid; border-left-style: solid;';
        shtml +=  'height: 18px; border-right-color: black; border-bottom-style: solid; padding-right: 0px; padding-left: 0px; padding-bottom: 0px; padding-top: 0px;">';
        shtml +=  '<strong style="width: 15%">Address</strong></td>';
        shtml +=  '<td style="border-left-color: black; border-bottom-color: black; width: 121px; border-top-style: solid;';
        shtml +=  'border-top-color: black; border-right-style: solid; border-left-style: solid;';
        shtml +=  'height: 18px; border-right-color: black; border-bottom-style: solid; padding-right: 0px; padding-left: 0px; padding-bottom: 0px; padding-top: 0px;">';
        shtml +=  '<strong>Birthdate</strong></td>';
        shtml +=  '<td style="border-left-color: black; border-bottom-color: black; width: 107px; border-top-style: solid;';
        shtml +=  'border-top-color: black; border-right-style: solid; border-left-style: solid;';
        shtml +=  'height: 18px; border-right-color: black; border-bottom-style: solid; padding-right: 0px; padding-left: 0px; padding-bottom: 0px; padding-top: 0px;">';
        shtml +=  '<strong>Birthplace</strong></td>';
        shtml +=  '<td style="border-left-color: black; border-bottom-color: black; width: 93px; border-top-style: solid;';
        shtml +=  'border-top-color: black; border-right-style: solid; border-left-style: solid;';
        shtml +=  'height: 18px; border-right-color: black; border-bottom-style: solid; padding-right: 0px; padding-left: 0px; padding-bottom: 0px; padding-top: 0px;">';
        shtml +=  '<strong>Citizenship</strong></td>';
        shtml +=  '<td style="border-left-color: black; border-bottom-color: black; width: 93px; border-top-style: solid;';
        shtml +=  'border-top-color: black; border-right-style: solid; border-left-style: solid;';
        shtml +=  'height: 18px; border-right-color: black; border-bottom-style: solid; padding-right: 0px; padding-left: 0px; padding-bottom: 0px; padding-top: 0px;">';
        shtml +=  '<strong>Classification Remitter/Beneficiary</strong></td>';
        shtml +=  '<td style="padding-right: 0px; padding-left: 0px; border-left-color: black; border-bottom-color: black;';
        shtml +=  'padding-bottom: 0px; width: 93px; border-top-style: solid; border-top-color: black;';
        shtml +=  'padding-top: 0px; border-right-style: solid; border-left-style: solid; height: 18px;';
        shtml +=  'border-right-color: black; border-bottom-style: solid">';
        shtml +=  '<strong>Name if OFAC List</strong></td>';
        shtml +=  '<td style="padding-right: 0px; padding-left: 0px; border-left-color: black; border-bottom-color: black;';
        shtml +=  'padding-bottom: 0px; width: 93px; border-top-style: solid; border-top-color: black;';
        shtml +=  'padding-top: 0px; border-right-style: solid; border-left-style: solid; height: 18px;';
        shtml +=  'border-right-color: black; border-bottom-style: solid">';
        shtml +=  '<strong>Alias</strong></td>';
        shtml +=  '</tr>';
        for (i=0;i<dt.Rows.length;i++)
        {
        shtml +=  '<tr>';
        shtml +=  '<td style="height: 15px">';
        shtml +=  '' + dt.Rows[0].name + '</td>';
        shtml +=  '<td style="width: 84px; height: 15px;">';
        shtml +=  '' + dt.Rows[0].address + '</td>';
        shtml +=  '<td style="width: 121px; height: 15px">';
        shtml +=  '' + dt.Rows[0].birthdate + '</td>';
        shtml +=  '<td style="width: 107px; height: 15px;">';
        shtml +=  '' + dt.Rows[0].birthplace + '</td>';
        shtml +=  '<td style="width: 195px; height: 15px;">';
        shtml +=  '' + dt.Rows[0].citizenship + '</td>';
        shtml +=  '<td style="height: 15px">';
        shtml +=  '' + dt.Rows[0]["class"] + '</td>';
        shtml +=  '<td style="height: 15px">';
        shtml +=  '' + dt.Rows[0].ofac + '</td>';
        shtml +=  '<td style="height: 15px">';
        shtml +=  '' + dt.Rows[0].alias + '</td>';
        shtml +=  '</tr>';
        }
        shtml +=  '</table>';
        shtml +=  '</td>';
        shtml +=  '</tr>';
        shtml +=  '</table>';
        shtml +=  '';
        shtml +=  '</div>';


        return shtml;
    }
 }   
/* END Watchlist Matching REPORT */
