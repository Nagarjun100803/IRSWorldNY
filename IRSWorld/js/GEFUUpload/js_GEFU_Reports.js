
function GEFU_Reports()
{
    var gtranxdate      = "";
    var glocationcode   = "";
    this.BranchCode     = null;
    this.BookingRatesCreationResult = null;
    this.tranxdate      = "";
    this.Role           = "";
    
    this.download = function()
    {
        IRSWorld.GEFUUpload.GEFUMaintenance.download();
    }
    
    this.ActiveModule = function(divName)
    {
        IRSWorld.GEFUUpload.GEFUMaintenance.ActiveModule(divName);
    }
    
    this.check_is_report_created = function (locationcode, tranxdate, tranxstatus, reportname)
    {
        return IRSWorld.GEFUUpload.GEFUMaintenance.Check_Is_Report_Created(locationcode, tranxdate, tranxstatus, reportname);
    }
    
    this.create_report = function (reportname, tranxdate, locationcode, tranxstatus, tranxsource)
    {
        if (reportname == "Booking Rates Used")
        {
            this.create_bookingrate(tranxdate, locationcode);
        }
        else if (reportname == "GEFU Upload File")
        {
            if (trim(tranxstatus) == "")
            {
                alert ("Select transaction status");
                return;
            }
            var br_complete = null
            br_complete = this.check_bookingrates_complete(tranxdate, locationcode);
            if (br_complete == true)
            {
                var acc_complete = null
                acc_complete = this.check_accountingentries_complete (tranxdate, locationcode)
                if (acc_complete == true)
                {
                    this.create_gefufile(tranxdate, locationcode, tranxstatus);
                }                
            }
        }
        else if (reportname == "Transaction Details by Receipt Mode")
        {   
            this.create_receiptmode_report(tranxdate, locationcode);
        }
        else if (reportname == "Transaction Details by Transaction Source")
        {
            if (tranxsource == "''")
            {
                alert ("Select transaction source");
            }
            this.create_transactionsource_report (tranxdate, locationcode, tranxsource)
        }
        else if (reportname == "Accounting Entries Combination")
        {
            this.create_accounting_entries(tranxdate, locationcode);
        }
        else if (reportname == "Preliminary Summary")
        {
            if (trim(tranxstatus) == "")
            {
                alert ("Select transaction status");
                return;
            }
            this.create_preliminary_summary_report(locationcode, tranxdate,tranxstatus);
        }
        else if (reportname == "Summary")
        {
            if (trim(tranxstatus) == "")
            {
                alert ("Select transaction status");
                return;
            }
            this.create_summary_report(locationcode, tranxdate,tranxstatus);
        }
        else if (reportname == "Consolidated Summary")
        {
            if (trim(tranxstatus) == "")
            {
                alert ("Select transaction status");
                return;
            }
            this.create_consolidated_report(locationcode, tranxdate,tranxstatus);
        }
        else if (reportname == "Consolidated Summary (Online and Non-online)")
        {
            this.create_consolidated_report_online_nononline(locationcode, tranxdate,tranxstatus);
        }
        else if (reportname == "Consolidated Summary (by GL Codes)")
        {   
            if (trim(tranxstatus) == "")
            {
                alert ("Select transaction status");
                return;
            }
            this.create_consolidated_glcodes_report(locationcode, tranxdate,tranxstatus);
        }
        else if (reportname == "Consolidated Summary (by GL Codes) - (Online and Non-online)")
        {   
            this.create_consolidated_glcodes_report_online_nononline(locationcode, tranxdate,tranxstatus);
        }        
        else if (reportname == "Transactions not included in GEFU File")
        {   
            this.create_excluded_transactions_report(tranxdate, locationcode);
        }        
    }
    
    this.create_excluded_transactions_report = function (tranxdate,locationcode)
    {
        var excluded_report = null;
        excluded_report = IRSWorld.GEFUUpload.GEFUMaintenance.Create_Excluded_Transactions_Report(tranxdate, locationcode);
        
        if (excluded_report.value == true)
        {
            document.getElementById("txtReportsDate").value = document.getElementById("txtTranxdate").value; 
            this.searchreports();
            alert("Transactions not included in GEFU File report successfully extracted");
        }
        else if (excluded_report.value == false)
        {
            alert ("No data to be extracted");
        }
        else
        {
            alert("Creation of Transactions not included in GEFU File report failed");
        }
    }
    
    this.create_consolidated_glcodes_report_online_nononline = function (locationcode, tranxdate, tranxstatus)
    {
        var consol_glcodes_report = null;
        consol_glcodes_report = IRSWorld.GEFUUpload.GEFUMaintenance.Create_Consolidated_GLCodes_Report_Online_NonOnline(tranxdate, locationcode, tranxstatus);
        
        if (consol_glcodes_report.value == true)
        {
            document.getElementById("txtReportsDate").value = document.getElementById("txtTranxdate").value; 
            this.searchreports();
            alert("Consolidated Summary (by GL Codes) - (Online and Non-online) report successfully extracted");
        }
        else if (consol_glcodes_report.value == false)
        {
            alert ("No data to be extracted");
        }
        else
        {
            alert("Creation of Consolidated Summary (by GL Codes) - (Online and Non-online) report failed");
        }
    }

    this.create_consolidated_glcodes_report = function (locationcode, tranxdate, tranxstatus)
    {
        var consol_glcodes_report = null;
        consol_glcodes_report = IRSWorld.GEFUUpload.GEFUMaintenance.Create_Consolidated_GLCodes_Report(tranxdate, locationcode, tranxstatus);
        
        if (consol_glcodes_report.value == true)
        {
            document.getElementById("txtReportsDate").value = document.getElementById("txtTranxdate").value; 
            this.searchreports();
            alert("Consolidated Summary (by GL Codes) report successfully extracted");
        }
        else if (consol_glcodes_report.value == false)
        {
            alert ("No data to be extracted");
        }
        else
        {
            alert("Creation of Consolidated Summary (by GL Codes) report failed");
        }
    }
    
    this.create_consolidated_report_online_nononline = function (locationcode, tranxdate, tranxstatus)
    {
        var consol_on_non_report = null;
        consol_on_non_report = IRSWorld.GEFUUpload.GEFUMaintenance.Create_Consolidated_Report_Online_NonOnline(tranxdate, locationcode, tranxstatus);
        
        if (consol_on_non_report.value == true)
        {
            document.getElementById("txtReportsDate").value = document.getElementById("txtTranxdate").value; 
            this.searchreports();
            alert("Consolidated Summary (Online and Non-online) report successfully extracted");
        }
        else if (consol_on_non_report.value == false)
        {
            alert ("No data to be extracted");
        }
        else
        {
            alert("Creation of Consolidated Summary (Online and Non-online) report failed");
        }
    }
    
    this.create_consolidated_report = function (locationcode, tranxdate, tranxstatus)
    {
        var consol_report = null;
        consol_report = IRSWorld.GEFUUpload.GEFUMaintenance.Create_Consolidated_Report(tranxdate, locationcode, tranxstatus);
        
        if (consol_report.value == true)
        {
            document.getElementById("txtReportsDate").value = document.getElementById("txtTranxdate").value; 
            this.searchreports();
            alert("Consolidated Summary report successfully extracted");
        }
        else if (consol_report.value == false)
        {
            alert ("No data to be extracted");
        }
        else
        {
            alert("Creation of Consolidated Summary report failed");
        }
    }
    
    this.create_summary_report = function (locationcode, tranxdate, tranxstatus)
    {
        var summary_report = null;
        summary_report = IRSWorld.GEFUUpload.GEFUMaintenance.Create_Summary_Report(tranxdate, locationcode, tranxstatus);
        
        if (summary_report.value == true)
        {
            document.getElementById("txtReportsDate").value = document.getElementById("txtTranxdate").value; 
            this.searchreports();
            alert("Summary report successfully extracted");
        }
        else if (summary_report.value == false)
        {
            alert ("No data to be extracted");
        }
        else
        {
            alert("Creation of Summary report failed");
        }
    }
    
    this.create_preliminary_summary_report = function (locationcode, tranxdate, tranxstatus)
    {
        var prelim_report = null;
        prelim_report = IRSWorld.GEFUUpload.GEFUMaintenance.Create_Preliminary_Report(tranxdate, locationcode, tranxstatus);
        
        if (prelim_report.value == true)
        {
            document.getElementById("txtReportsDate").value = document.getElementById("txtTranxdate").value; 
            this.searchreports();
            alert("Preliminary Summary report successfully extracted");
        }
        else if (prelim_report.value == false)
        {
            alert ("No data to be extracted");
        }
        else
        {
            alert("Creation of Preliminary Summary report failed");
        }
    }
    
    this.create_transactionsource_report = function(tranxdate, locationcode, tranxsource)
    {
        var transource = null;
        transource = IRSWorld.GEFUUpload.GEFUMaintenance.Create_TransactionSource_Report(tranxdate, locationcode, tranxsource);
        
        if (transource.value == true)
        {
            document.getElementById("txtReportsDate").value = document.getElementById("txtTranxdate").value; 
            this.searchreports();
            alert("Transaction Report by Transaction Source successfully extracted");
        }
        else if (transource.value == false)
        {
            alert ("No data to be extracted");
        }
        else
        {
            alert("Creation of Transaction Report by Transaction Source failed");
        }
    }
    
    this.create_receiptmode_report = function(tranxdate, locationcode)
    {
        var receipmode = null;
        receipmode = IRSWorld.GEFUUpload.GEFUMaintenance.Create_ReceiptMode_Report(tranxdate, locationcode);
        
        if (receipmode.value == true)
        {
            document.getElementById("txtReportsDate").value = document.getElementById("txtTranxdate").value; 
            this.searchreports();
            alert("Transaction Report by Receipt Mode successfully extracted");                        
        }
        else if (receipmode.value == false)
        {
            alert ("No data to be extracted");
        }
        else
        {
            alert("Creation of Transaction Report by Receipt Mode failed");
        }
    }
    
    this.check_bookingrates_complete = function (effectivitydate, locationcode)
    {
        var bookingratesresult = null;
        bookingratesresult = IRSWorld.GEFUUpload.GEFUMaintenance.Check_Booking_Rates(effectivitydate, locationcode);
        if (bookingratesresult.value  != "")
        {
            var CcyPerCombination = new Array();
            var CcyIncomplete;
            CcyIncomplete = "";
            
            CcyPerCombination = bookingratesresult.value.split("*");
                        
            var div = document.getElementById("divIncBookingRAtes");
	
            var g; 
            g = "<table style='width:100%'> ";
                g = g + "<tr> ";
                g = g + "<td style='width:100%; BACKGROUND-COLOR: lavender; text-align:center; border-bottom: solid 1px midnightblue; color: MidnightBlue; font-family:Arial; font-weight: bold; font-size: 12px' colspan='2'> ";
                g = g + "COMPLETE THE FOLLOWING BOOKING RATES";
                g = g + "</td> ";
                g = g + "</tr> ";
                g = g + "<tr> ";
                g = g + "<td style='width:50%; BACKGROUND-COLOR: lavender; text-align:center; border-bottom: solid 1px midnightblue; color: MidnightBlue; font-family:Arial; font-weight: bold; font-size: 12px'> ";
                g = g + "Source Currency";
                g = g + "</td> ";
                g = g + "<td style='width:50%; BACKGROUND-COLOR: lavender; text-align:center; border-bottom: solid 1px midnightblue; color: MidnightBlue; font-family:Arial; font-weight: bold; font-size: 12px'> ";
                g = g + "Target Currency";
                g = g + "</td> ";
                g = g + "</tr> ";

            CcyPerCombination = bookingratesresult.value.split("*");
            for (var i = 1; i < CcyPerCombination.length; i++)
            {                        
                var Ccy                
                Ccy = CcyPerCombination[i].split("~");
                
                g = g + "<tr> ";
                g = g + "<td style='width:50%; text-align:center'> ";
                g = g + Ccy[0];
                g = g + "</td> ";
                g = g + "<td style='width:50%; text-align:center'> ";
                g = g + Ccy[1];
                g = g + "</td> ";	
                g = g + "</tr> ";
            }
            g = g + " </table>"
            div.innerHTML = g;
            document.getElementById("divIncBookingRAtes").style.display    = "block";
            return false;
            
        }
        else
        {
            return true;
        }
    }
    
    this.check_accountingentries_complete = function (tranxdate, locationcode)
    {
        var accountingeresult = null;
        accountingeresult = IRSWorld.GEFUUpload.GEFUMaintenance.Check_AccountingEntries(tranxdate, locationcode);
        if (accountingeresult.value  == true)
        {   
            var folder;                
            folder = "C:\\IRSWorldReports\\" + locationcode + "\\" + tranxdate.replace("/","").replace("/","") + "\\Additional_AccountingEntries_" + locationcode + "_" +  tranxdate.replace("/","").replace("/","") + ".xls";
            alert ("Add accounting entries combination \n" + folder);
            document.getElementById("txtReportsDate").value = document.getElementById("txtTranxdate").value; 
            this.searchreports();
            return false;
        }
        else
        {
            return true;
        }
    }
    
    this.check_fc_branch_code = function ()
    {
        return IRSWorld.GEFUUpload.GEFUMaintenance.check_fc_branch_code(this.BranchCode.value);
    }    
    
    this.create_gefufile = function (tranxdate, locationcode, tranxstatus)
    {   
        var gefuresult = null;
        gefuresult = IRSWorld.GEFUUpload.GEFUMaintenance.Create_GEFUFile(tranxdate, locationcode, tranxstatus);
        
        if (gefuresult.value == "true")
        {
            document.getElementById("txtReportsDate").value = document.getElementById("txtTranxdate").value;            
            this.searchreports();
            alert("GEFU successfully extracted");            
            document.getElementById("divIncAccnt").style.display    = "none";
            this.populate_dropdownboxes();
        }
        else if (gefuresult.value == "No Data to be extracted")
        {
            alert(gefuresult.value);
        }
        else
        {        
            var updateAccnt;
            updateAccnt = gefuresult.value.split(";");
            
            var div = document.getElementById("divIncAccnt");
	
            var g; 
            g = "<table style='width:100%'> ";
                g = g + "<tr> ";
                g = g + "<td style='width:100%; BACKGROUND-COLOR: lavender; text-align:center; border-bottom: solid 1px midnightblue; color: MidnightBlue; font-family:Arial; font-weight: bold; font-size: 12px' colspan='2'> ";
                g = g + "COMPLETE THE FOLLOWING ACCOUNTING ENTRIES";
                g = g + "</td> ";
                g = g + "</tr> ";
                g = g + "<tr> ";
                g = g + "<td style='width:50%; BACKGROUND-COLOR: lavender; text-align:center; border-bottom: solid 1px midnightblue; color: MidnightBlue; font-family:Arial; font-weight: bold; font-size: 12px'> ";
                g = g + "GL Code Type";
                g = g + "</td> ";
                g = g + "<td style='width:50%; BACKGROUND-COLOR: lavender; text-align:center; border-bottom: solid 1px midnightblue; color: MidnightBlue; font-family:Arial; font-weight: bold; font-size: 12px'> ";
                g = g + "Accounting Entries SEQ_ID";
                g = g + "</td> ";
                g = g + "</tr> ";

            for (var i = 0; i < updateAccnt.length -1; i++)
            {            
                var Accnt                
                Accnt = updateAccnt[i].split("~");
                
                g = g + "<tr> ";
                g = g + "<td style='width:50%; text-align:center'> ";
                g = g + Accnt[0];
                g = g + "</td> ";
                g = g + "<td style='width:50%; text-align:center'> ";
                g = g + Accnt[1];
                g = g + "</td> ";	
                g = g + "</tr> ";
            }
            g = g + " </table>"
            div.innerHTML = g;             
            document.getElementById("divIncAccnt").style.display    = "block";
        }
    }
    
    this.create_bookingrate = function(tranxdate, locationcode)
    {
        gtranxdate = tranxdate;
        glocationcode = locationcode;
        IRSWorld.GEFUUpload.GEFUMaintenance.Validate_Bookingrates(tranxdate, locationcode, this.validate_bookingRates_callback);
    }    
       
    this.create_accounting_entries = function(tranxdate, locationcode)
    {
        gtranxdate = tranxdate;
        glocationcode = locationcode;
        IRSWorld.GEFUUpload.GEFUMaintenance.Create_Accounting_Entries_Report(tranxdate, locationcode, this.create_accounting_entries_callback);
    }
    
    this.create_accounting_entries_callback = function (result)
    {
        if (result.value == true)
        {
            gefuMaintenance.jsFunction.searchreports();
            alert ("Accounting Entries data successfully extracted");
        }
        else
        {
            alert ("Creation of Accounting Entries Failed")
        }
    }
    
    this.load_bookingrates_grid_viewer = function(tranxdate)
    {        
        this.populate_dropdownboxes();
        
        if (document.getElementById("ddReportName").value == "11")
         {
            this.accountingentries_grid = new grid_viewer();
            this.accountingentries_grid.sql_source  = "sp_bookingrates_grid_reports~" + this.BranchCode.value + "~" + this.tranxdate;
            this.accountingentries_grid.div_container = document.getElementById("divGEFUBookingRates");
            this.accountingentries_grid.datarow_column = 0;
            this.accountingentries_grid.open_grid_view();
            this.grid = null;
            document.getElementById("divGEFUBookingRates").style.display    = "block";
        }
    }
    
    this.validate_bookingRates_callback = function(result)
    {
        if(result.value == true)
        {
            var verifier = null;
            var authorizer = null
            
            authorizer = document.getElementById("txtAuthorizer").value
            verifier = document.getElementById("txtVerifier").value
                     
            if (verifier == "")
            {
                alert("Input verifier name");
                return;
            }   
            
            if (authorizer == "")
            {
                alert("Input authorizer name");
                return;
            }


            this.BookingRatesCreationResult = IRSWorld.GEFUUpload.GEFUMaintenance.Create_BookingRates_Report(gtranxdate, glocationcode, verifier, authorizer);//, this.Create_BookingRates_Report_callback);            
            if (this.BookingRatesCreationResult.value == true)
            {
                document.getElementById("txtReportsDate").value = document.getElementById("txtTranxdate").value;            
                gefuMaintenance.jsFunction.searchreports();
                alert("Booking rates data successfully extracted");
            }
            else
            {
                alert ("Creation of Booking Rates Failed");
            }
        }
        else
        {
            alert ("No Bookingrates Data found");
            return;
        }
    }
    
    this.populate_dropdownboxes = function ()
    {
        IRSWorld.GEFUUpload.GEFUMaintenance.getTranxSource_sublookup(this.BranchCode.value, this.tranxdate, this.dropdown_transaction_source_callback);        
    }
    
    this.set_default_date = function ()
    {
        document.getElementById("txtTranxdate").value = get_current_date_string().substring(0, 10);
        document.getElementById("txtReportsDate").value = get_current_date_string().substring(0, 10);
    }       
     
    this.dropdown_transaction_source_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddTransactionSource", result.value, "fldvalue", "FldDescription");
            this.commons = null;
        }
    }
    
    this.check_data = function (date)
    {
        IRSWorld.GEFUUpload.GEFUMaintenance.check_data (date);
    }
    
    this.search_specific_reports = function (branchcode, reportname)
    {
        IRSWorld.GEFUUpload.GEFUMaintenance.search_specific_reports(branchcode, reportname, this.searchreports_callback);    
    }
    
    this.searchreports = function ()
    {
        IRSWorld.GEFUUpload.GEFUMaintenance.searchreports(this.BranchCode.value, document.getElementById("txtReportsDate").value, this.searchreports_callback);
    }
    
    this.searchreports_callback = function(result)
    {
        document.getElementById("divReportsDownload").style.display = "block";
        
        var tbgrid = document.getElementById("tblReportsDownload");
        var tbExist = document.getElementById('tbBody'); 
        var tbbody = document.createElement('tbody');  
        
        if (result.value.Tables[0].Rows.length > 0)
        {
            for (var i = 0; i < result.value.Tables[0].Rows.length; i++)
            {
                var row = document.createElement('tr');
                var tdReportFileName = document.createElement('td');
                var tdReportFilePath = document.createElement('td');
                var tdDownloadButton = document.createElement('td');

                var lblFileName = document.createElement('label');
                //lblFileName.innerText = result.value.Tables[0].Rows[i].filename;
                lblFileName.innerHTML = result.value.Tables[0].Rows[i].filename;
                lblFileName.setAttribute('align', 'left');
                tdReportFileName.appendChild(lblFileName);            

                var hrefDownload = document.createElement('a');
                //hrefDownload.innerText = 'Download';
                hrefDownload.innerHTML = 'Download';
                hrefDownload.setAttribute('align', 'center');
                hrefDownload.setAttribute('href',"DownloadReports.aspx?download_path=" + result.value.Tables[0].Rows[i].filepath);
                tdDownloadButton.appendChild(hrefDownload);
                                    
                row.appendChild(tdReportFileName);
                row.appendChild(tdReportFilePath);
                row.appendChild(tdDownloadButton);
                
                tbbody.appendChild(row);  
            }
        }
        else
        {
            var row = document.createElement('tr');
            var tdReportFileName = document.createElement('td');
            var tdReportFilePath = document.createElement('td');
            var tdDownloadButton = document.createElement('td');

            var lblFileName = document.createElement('label');
            //lblFileName.innerText = "NO REPORTS FOUND";
            lblFileName.innerHTML = "NO REPORTS FOUND";
            lblFileName.setAttribute('align', 'left');
            tdReportFileName.appendChild(lblFileName);
                                
            row.appendChild(tdReportFileName);
            row.appendChild(tdReportFilePath);
            row.appendChild(tdDownloadButton);
            
            tbbody.appendChild(row);  
        }
        
        
        if (tbExist != null)
        {
         
            tbgrid.replaceChild(tbbody, tbExist);
        }
        else
        {
        
            tbgrid.appendChild(tbbody);
        }        
        tbbody.setAttribute('id', 'tbBody');
    }
        
    this.do_ajaxgridviewer = function (parameter)
    {
        this.tranxdate = document.getElementById("txtTranxdate").value;
        if (parameter != null && parameter != "undefined")
        {
            var command_param = parameter.split("$");

            if (command_param.length > 0)
            {
                this.process_grid_viewer(parameter);
            }
        }
    }   

    this.process_grid_viewer = function (parameter)
    {    
        this.bookingratesview = new grid_viewer();
        this.bookingratesview.sql_source  = "sp_bookingrates_grid_reports~" + this.BranchCode.value + "~" + this.tranxdate;
        this.bookingratesview.div_container = document.getElementById("divGEFUBookingRates");
        this.bookingratesview.datarow_column = 0;
        this.bookingratesview.process_ajax(parameter);
        this.grid = null;
    }   
}
