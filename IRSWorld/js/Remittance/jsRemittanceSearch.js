    var chargrid;
    
    var formatDate = function (formatDate, formatString) 
    {
        var dt = Date.parse(formatDate);
        var dt2 = new Date();
        dt2.setTime(dt);
	    if (isNaN(dt2) == false) 
	    {
		    var months = new Array("Jan","Feb","Mar","Apr","May","Jun","Jul","Aug","Sep","Oct","Nov","Dec");
		    var yyyy = dt2.getFullYear();
		    var yy = yyyy.toString().substring(2);
		    var m = dt2.getMonth() + 1;
		    var mm = m < 10 ? "0" + m : m;
		    var mmm = months[m];
		    var d = dt2.getDate();
		    var dd = d < 10 ? "0" + d : d;
    		
		    var h = dt2.getHours();
		    var hh = h < 10 ? "0" + h : h;
		    var n = dt2.getMinutes();
		    var nn = n < 10 ? "0" + n : n;
		    var s = dt2.getSeconds();
		    var ss = s < 10 ? "0" + s : s;

		    formatString = formatString.replace(/yyyy/i, yyyy);
		    formatString = formatString.replace(/yy/i, yy);
		    formatString = formatString.replace(/mmm/i, mmm);
		    formatString = formatString.replace(/mm/i, mm);
		    formatString = formatString.replace(/m/i, m);
		    formatString = formatString.replace(/dd/i, dd);
		    formatString = formatString.replace(/d/i, d);
		    formatString = formatString.replace(/hh/i, hh);
		    formatString = formatString.replace(/h/i, h);
		    formatString = formatString.replace(/nn/i, nn);
		    formatString = formatString.replace(/n/i, n);
		    formatString = formatString.replace(/ss/i, ss);
		    formatString = formatString.replace(/s/i, s);

		    return formatString;
        } 
        else 
        {
		    return "";
	    }
    }

    function create_forapproval_grid(dt)
    {
        chargrid = new datagrid();
        {//0
            var col = new datagrid_column();
            col.header = 'ID';
            col.datafield = 'seq_id';
            col.align = 'center';
	        col.styles.cell += 'width:5%;display:none';
	        col.styles.header += 'display:none';
	        col.styles.datafield += 'display:none';
            chargrid.addColumn(col);	
	    }
        {//1
            var col = new datagrid_column();
            col.header = 'Ref No';
            col.datafield = 'referenceno';
            col.align = 'center';
	        col.styles.cell += 'width:5%';
	        //col.styles.header += 'display:none';
	        col.styles.datafield += 'width: 100%; border: solid 1px navy; font-size:10px ; font-style:Times New Roman;font-weight:normal';
            chargrid.addColumn(col);	
	    }		    
        {//2
            var col = new datagrid_column();
	        col.header = 'Remitter';
	        col.datafield = 'Remitter';
	        col.align = 'left';
	        //col.type = 'select';
	        //col.options = chargopts;
	        //col.onchange = 'chargrid_chargename_change';
	        col.styles.cell += 'width:auto;text-align:left; font-size:8px';
	        col.styles.datafield += 'width: auto; border: solid 1px navy; font-size:10px; font-style:Times New Roman;font-weight:normal';
            chargrid.addColumn(col);	
	    }
        {//3
            var col = new datagrid_column();
	        col.header = 'Benef';
	        col.datafield = 'Benef';
	        col.align = 'left';
	        //col.type = 'select';
	        //col.onchange = 'chargrid_comptype_change';
	        //col.options = comptypes;
	        col.styles.cell += 'width:auto;text-align:left; font-size:8px';
	        col.styles.datafield += 'width:auto;border: solid 1px navy; font-size:10px ; font-style:Times New Roman;font-weight:normal';
            chargrid.addColumn(col);	
	    }	
	    /*
        {//4
            var col = new datagrid_column();
	        col.header = 'Currency';
	        col.datafield = 'Currency';
	        col.align = 'left';
	        col.type = 'select';
	        col.options = local_currs;
	        col.styles.cell += 'width:12%;text-align:left;';
	        col.styles.datafield += 'width: 100%; border: solid 1px navy;';
            chargrid.addColumn(col);	
	    }	
	    */		    
        {//5 -- 4
	        var col = new datagrid_column();
	        col.header = 'Tran Status';
	        col.datafield = 'Transtatus';
	        col.align = 'center';
	        col.styles.cell += 'width:10%;padding-right:3px;';
	        col.styles.datafield += 'width:100%; text-align:right; font-size:10px ; font-style:Times New Roman;font-weight:normal';
	       // col.formatter = 'value_formatter';
	       // col.onblur = 'charge_value_change';
	        chargrid.addColumn(col);
	    }     
        {//6 -- 5
	        var col = new datagrid_column();
	        col.header = 'Tranxdate';
	        col.datafield = 'Tranxdate';
	        col.align = 'center';
	        col.styles.cell += 'width:10%;padding-right:3px;';
	        col.styles.datafield += 'width:100%;text-align:right; font-size:10px ; font-style:Times New Roman;font-weight:normal';
	       // col.formatter = 'value_formatter';
	       // col.onblur = 'min_amt_change';
	        chargrid.addColumn(col);
	    } 
	    {//7 -- 6
	        var col = new datagrid_column();
	        col.header = 'Rem. CCY';
	        col.datafield = 'RemittingCurrency';
	        col.align = 'center';
	        col.styles.cell += 'width:8%;padding-right:3px;';
	        col.styles.datafield += 'text-align:right; font-size:10px ; font-style:Times New Roman;font-weight:normal';
	        //col.formatter = 'value_formatter';
	        //col.onblur = 'deno_amt_change';
	        chargrid.addColumn(col);
	    }	
	    {//8 -- 7
	        var col = new datagrid_column();
	        col.header = 'Gross';
	        col.datafield = 'GrossRemAmount';
	        col.align = 'center';
	        //col.type = 'check';
	        col.styles.cell += 'width:8%;padding-right:3px;';
	        col.styles.datafield += 'text-align:right; font-size:10px ; font-style:Times New Roman;font-weight:normal';
	        //col.onclick = 'check_charge_changed';
	        chargrid.addColumn(col);
	    }
	    {//9 -- 8
	        var col = new datagrid_column();
	        col.header = 'Net';
	        col.datafield = 'NetRemAmount';
	        col.align = 'center';
	        //col.type = 'check';
	        //col.onclick = 'check_charge_changed';
	        col.styles.cell += 'width:8%;padding-right:3px;';
	        col.styles.datafield += 'text-align:right; font-size:10px ; font-style:Times New Roman;font-weight:normal';
	        chargrid.addColumn(col);
	    }
	     {//9 -- 9
	        var col = new datagrid_column();
	        col.header = 'Charge';
	        col.datafield = 'TotalLocalCharge';
	        col.align = 'center';
	        //col.type = 'check';
	        //col.onclick = 'check_charge_changed';
	        col.styles.cell += 'width:6%;padding-right:3px;';
	        col.styles.datafield += 'text-align:right; font-size:10px ; font-style:Times New Roman;font-weight:normal';
	        chargrid.addColumn(col);
	    }
	     {//9 -- 10
	        var col = new datagrid_column();
	        col.header = 'Bene Ccy';
	        col.datafield = 'NettoBeneCurrency';
	        col.align = 'center';
	        //col.type = 'check';
	        //col.onclick = 'check_charge_changed';
	        col.styles.cell += 'width:6%;padding-right:3px;';
	        col.styles.datafield += 'text-align:right; font-size:10px ; font-style:Times New Roman;font-weight:normal';
	        chargrid.addColumn(col);
	    }
	     {//9 -- 11
	        var col = new datagrid_column();
	        col.header = 'Net(Benef)';
	        col.datafield = 'NettoBeneAmount';
	        col.align = 'center';
	        //col.type = 'check';
	        //col.onclick = 'check_charge_changed';
	        col.styles.cell += 'width:10%;padding-right:3px;';
	        col.styles.datafield += 'text-align:right; font-size:10px ; font-style:Times New Roman;font-weight:normal';
	        chargrid.addColumn(col);
	    }
	      {//9 -- 12
	        var col = new datagrid_column();
	        col.header = 'Account No';
	        col.datafield = 'AccountNo';
	        col.align = 'center';
	        //col.type = 'check';
	        //col.onclick = 'check_charge_changed';
	        col.styles.cell += 'width:10%;padding-right:3px;';
	        col.styles.datafield += 'text-align:right; font-size:10px ; font-style:Times New Roman;font-weight:normal';
	        chargrid.addColumn(col);
	    }
	     {//9 -- 13
	        var col = new datagrid_column();
	        col.header = 'Exclude';
	        col.datafield = 'exclude';
	        col.align = 'center';
	        col.type = 'check';
	        //col.onclick = 'check_charge_changed';
	        col.styles.cell += 'width:3%;padding-right:3px;';
	        col.styles.datafield += 'font-size:8px';
	        chargrid.addColumn(col);
	    }
	    {
	        var col = new datagrid_column();
            col.header = '';
            col.datafield = '';
            col.align = 'center';
            col.type = 'button';
            col.value = 'Edit';
            col.edit_button = true;
            col.styles.cell += 'width:2%';
            col.styles.datafield += 'width:100%;';
            chargrid.addColumn(col);
	    }
	    //chargrid.caption = 'Charges (Details)';	
	    //chargrid.row.onclick = 'chargrid_row_selected';
        chargrid.classNames.table = 'table_body';
        // chargrid.row.normal_style = 'background-color: #f7ffff;';  
        // chargrid.footer.edit_button.none = false;
        // chargrid.footer.edit_button.style += 'background-color: lavender;'; 
        // chargrid.footer.edit_button.onclick = 'chargrid_edit_clicked';
        // chargrid.footer.edit_button.beforepost = 'chargrid_add_beforepost';
        // chargrid.footer.add_button.none = false;
        // chargrid.footer.add_button.style += 'background-color: lavender;';
        // chargrid.footer.add_button.onclick = 'chargrid_add_clicked'; 
        // chargrid.footer.add_button.beforepost = 'chargrid_add_beforepost';
        // chargrid.footer.delete_button.none = false;
        // chargrid.footer.delete_button.style += 'background-color: lavender;';             
        // chargrid.footer.save_button.none = false;
        // chargrid.footer.save_button.style += 'background-color: lavender;'; 
        // chargrid.footer.save_button.onclick = 'chargrid_save_clicked';
        // chargrid.footer.cancel_button.none = false;
        // chargrid.footer.cancel_button.style += 'background-color: lavender;'; 
        // chargrid.footer.cancel_button.onclick = 'chargrid_cancel_clicked';
        chargrid.bind(dt, 'div_forreview_list', 'tblchgrid');
	    if (dt.Rows.length > 0)
	    {
            chargrid.rowSelected(0, 0, 'tblchgrid_tr_0');
        }
        try
        {
            chargrid.rows_page = chargopts.length;
        }
        catch(e) {}
            
    }	
    
    function execute_ajaxgridview_command(param)
    {
        if (rem_search != null && rem_search != "undefined")
        {
            rem_search.do_gridview(param);
        }
    }

    function removeOption(param)
    {
        var elSel = document.getElementById(param);
        var i;
        for (i = elSel.length - 1; i>=0; i--) 
        {
            elSel.remove(i);
        }
    }

    function updateClock ( )
    {
        var currentTime = new Date ( );

        var currentHours = currentTime.getHours ( );
        var currentMinutes = currentTime.getMinutes ( );
        var currentSeconds = currentTime.getSeconds ( );

        // Pad the minutes and seconds with leading zeros, if required
        currentMinutes = ( currentMinutes < 10 ? "0" : "" ) + currentMinutes;
        currentSeconds = ( currentSeconds < 10 ? "0" : "" ) + currentSeconds;

        // Choose either "AM" or "PM" as appropriate
        var timeOfDay = ( currentHours < 12 ) ? "AM" : "PM";

        // Convert the hours component to 12-hour format if needed
        currentHours = ( currentHours > 12 ) ? currentHours - 12 : currentHours;

        // Convert an hours component of "0" to "12"
        currentHours = ( currentHours == 0 ) ? 12 : currentHours;

        // Compose the string for display
        var currentTimeString = currentHours + ":" + currentMinutes + ":" + currentSeconds + " " + timeOfDay;

        // Update the time display
        document.getElementById("clock").firstChild.nodeValue = currentTimeString;
    }


    function IsNumeric(strString)
    //  check for valid numeric strings	
    {
        var strValidChars = "0123456789.-";
        var strChar;
        var blnResult = true;

        if (strString.length == 0) return false;

        //  test strString consists of valid characters listed above
        for (i = 0; i < strString.length && blnResult == true; i++)
        {
            strChar = strString.charAt(i);
            if (strValidChars.indexOf(strChar) == -1)
            {
                blnResult = false;
            }
        }
        return blnResult;
    }

    function showDiv(element, isVisible)
    {
        if (isVisible == true)
        {
            document.getElementById(element).style.display = '';
        }
        else
        {
            document.getElementById(element).style.display = 'none';
        }
    }

    function showPanel(divName)
    {
        var div = document.getElementById(divName);
        
        divmask = new maskpanel();
        divmask.show();
        showDiv(divName, true);
        
        div.style.top = (((document.documentElement.clientHeight / 2) + document.documentElement.scrollTop) - (div.offsetHeight / 2)) + 'px';
        div.style.left = (((document.documentElement.clientWidth / 2) + document.documentElement.scrollLeft) - (div.offsetWidth / 2)) + 'px';
    }

    function hidePanel(divName)
    {
        showDiv(divName, false);
        divmask.hide();
    }

// **************************************************************************************** //
// *** Panel
// **************************************************************************************** //
    var maskpanel = function()
    {
        this.divobj;
        
        this.show = function()  
        {
            if(!document.getElementById("xdivmasking"))
            {
                var divEle = document.createElement('div');
                divEle.setAttribute("id", "xdivmasking");
                document.body.appendChild(divEle);
                
                var divSty = document.getElementById("xdivmasking").style;
                divSty.position = "absolute"; 
                divSty.top = "0px";
                divSty.left = "0px";
                divSty.zIndex = "46";
                divSty.opacity = ".50";
                divSty.backgroundColor="#000";
                divSty.filter = "alpha(opacity=50)";
     
                var divFram = document.createElement('iframe');
                divFram.setAttribute("id", "xmaskframe");
                document.body.appendChild(divFram);
                divSty = document.getElementById("xmaskframe").style;
                divSty.position = "absolute"; 
                divSty.top = "0px"; 
                divSty.left = "0px";
                divSty.zIndex = "45";
                divSty.border = "none";
                divSty.filter = "alpha(opacity=0)";
          }
     
          this.divobj = document.getElementById("xdivmasking");
          this.waitifrm = document.getElementById("xmaskframe");
            
          var dsh = document.documentElement.scrollHeight;
          var dch = document.documentElement.clientHeight;
          var dsw = document.documentElement.scrollWidth;
          var dcw = document.documentElement.clientWidth;
          var bdh = (dsh>dch) ? dsh:dch;
          var bdw = (dsw>dcw) ? dsw:dcw;
     
          this.waitifrm.style.height = this.divobj.style.height = bdh + 'px';
          this.waitifrm.style.width = this.divobj.style.width = bdw + 'px';
          this.waitifrm.style.display = this.divobj.style.display = "";
       }
       
       this.hide = function()
       {
           this.waitifrm.style.display = this.divobj.style.display = "none";
       }
    }

    function Remittance_Search()
    {
        var fnName= this;
        this.rem_txn_sequence_id = null;
        this.grid_ajax_page      = "../AjxPage/ajaxgrid.aspx";
        this.branchcode="";
        this.tranxdate="";
        this.clearingdate="";
        this.tranxstatus="";
        this.paymode="";
        this.refno="";
        this.batchno="";
        this.dtCh= "/";
        this.minYear=1900;
        this.maxYear=2100;
        this.remitmethod="";
        this.productcode="";
        this.implemtype="";
        this.userRole ="";
        this.userID="";
        this.statusDescription="";
        var numtrans;
        var tranxdate2;
        var otherchannels; 
        var paymode2;
        var productcode2;
        var implemtype2;
        var tranxstatus2;
        var ORDBatchNo;
        var ORDBatchNo2;
        var fileformat;
        var filetype;
        var fileformat_forapproval;
        var filetype_forapproval;
        /**
        * DHTML date validation script. Courtesy of SmartWebby.com (http://www.smartwebby.com/dhtml/)
        */
        // Declaring valid date character, minimum year and maximum year


        this.isInteger = function(s)
        {
	        var i;
            for (i = 0; i < s.length; i++){   
                // Check that current character is number.
                var c = s.charAt(i);
                if (((c < "0") || (c > "9"))) return false;
            }
            // All characters are numbers.
            return true;
        }

         this.stripCharsInBag= function(s, bag)
         {
	        var i;
            var returnString = "";
            // Search through string's characters one by one.
            // If character is not in bag, append to returnString.
            for (i = 0; i < s.length; i++){   
                var c = s.charAt(i);
                if (bag.indexOf(c) == -1) returnString += c;
            }
            return returnString;
        }
        
        this.daysInFebruary = function(year)
        {
	        // February has 29 days in any year evenly divisible by four,
            // EXCEPT for centurial years which are not also divisible by 400.
            return (((year % 4 == 0) && ( (!(year % 100 == 0)) || (year % 400 == 0))) ? 29 : 28 );
        }
        
        this.DaysArray= function(n) 
        {
            for (var i = 1; i <= n; i++) 
            {
	            this[i] = 31
	            if (i==4 || i==6 || i==9 || i==11) {this[i] = 30}
	            if (i==2) {this[i] = 29}
            } 
            return this
        }

         this.isDate=function(dtStr)
         {
	        var daysInMonth = this.DaysArray(12)
	        var pos1=dtStr.indexOf(this.dtCh)
	        var pos2=dtStr.indexOf(this.dtCh,pos1+1)
	        var strMonth=dtStr.substring(0,pos1)
	        var strDay=dtStr.substring(pos1+1,pos2)
	        var strYear=dtStr.substring(pos2+1)
	        strYr=strYear
	        if (strDay.charAt(0)=="0" && strDay.length>1) strDay=strDay.substring(1)
	        if (strMonth.charAt(0)=="0" && strMonth.length>1) strMonth=strMonth.substring(1)
	        for (var i = 1; i <= 3; i++) 
	        {
		        if (strYr.charAt(0)=="0" && strYr.length>1) strYr=strYr.substring(1)
	        }
	        month=parseInt(strMonth)
	        day=parseInt(strDay)
	        year=parseInt(strYr)
	        if (pos1==-1 || pos2==-1)
	        {
		        alert("The date format should be : mm/dd/yyyy")
		        return false
	        }
	        if (strMonth.length<1 || month<1 || month>12)
	        {
		        alert("Please enter a valid month")
		        return false
	        }
	        if (strDay.length<1 || day<1 || day>31 || (month==2 && day>this.daysInFebruary(year)) || day > daysInMonth[month])
	        {
		        alert("Please enter a valid day")
		        return false
	        }
	        if (strYear.length != 4 || year==0 || year<this.minYear || year>this.maxYear)
	        {
		        alert("Please enter a valid 4 digit year between "+this.minYear+" and "+this.maxYear)
		        return false
	        }
	        if (dtStr.indexOf(this.dtCh,pos2+1)!=-1 || this.isInteger(this.stripCharsInBag(dtStr, this.dtCh))==false)
	        {
		        alert("Please enter a valid date")
		        return false
	        }
            return true
        }
    
    
        this.remittance_search=function()
        {
            //Validate data entry// Declaring valid date character, minimum year and maximum year
           
            this.remitmethod=document.getElementById("ddlSearchClass").value;
            if (this.remitmethod=="null" ||  this.remitmethod=="undefined")
            {
                //alert("Please select criteria first"); return;
                this.remitmethod="";
            }
            else if (this.remitmethod=="0")
            {
                this.remitmethod="";
            }
            else
            {
                this.remitmethod=document.getElementById("ddlSearchClass").value;
            }
            var trdt =document.getElementById("txtsearchtranxdate").value;
            
            if (trdt != null && trdt != '')
            {
                if (this.isDate(trdt)==false)
                {
                    trdt.focus; return;
                }
            }

            var cldt =document.getElementById("txtsearchclearingdate").value;
            if (cldt != null && cldt != '')
            {
                if (this.isDate(cldt)==false)
                {
                    cldt.focus;return;
                }
            }
            fnName.tranxdate=document.getElementById("txtsearchtranxdate").value;
            fnName.clearingdate=document.getElementById("txtsearchclearingdate").value;
            fnName.ORDBatchNo=document.getElementById("txtordbatchno").value;
            fnName.filetype=document.getElementById("ddlFiletype").value;
            fnName.fileformat=document.getElementById("ddlFileFormat").value;
            //this.tranxstatus=document.getElementById("ddltranxstatus").value;
            if (fnName.tranxstatus=="null" || fnName.tranxstatus=="") 
            {
                fnName.tranxstatus="";
                //document.getElementById("tdStatus").innerText="VIEWER";
                setText("tdStatus", "VIEWER");
            }
            else
            {
                //document.getElementById("tdStatus").innerText=document.getElementById("ddltranxstatus")[document.getElementById("ddltranxstatus").selectedIndex].innerText;
                setText("tdStatus", getText(document.getElementById("ddltranxstatus")[document.getElementById("ddltranxstatus").selectedIndex]));
            }
            fnName.statusDescription=document.getElementById("tdStatus").innerText;
            
            this.paymode=document.getElementById("ddlpaymode").value;
            if (fnName.paymode=="null") fnName.paymode="";
            if (fnName.implemtype=="null") fnName.implemtype="";
            if (fnName.productcode=="null") fnName.productcode="";
            
            if  (fnName.ORDBatchNo=="null") fnName.ORDBatchNo=""
            if  (fnName.filetype=="null") fnName.filetype=""
            if  (fnName.fileformat=="null") fnName.fileformat=""
            fnName.refno=document.getElementById("txtsearchrefno").value;
            fnName.batchno=document.getElementById("txtbatchno").value;
            fnName.division_loading_progress();
            fnName.load_remittance_transaction();
            fnName.open_division("div_results");
        }
        
        this.validate_forapproval_processing = function()
        {
            //Validate before batch processing
         
            numtrans="";
            tranxdate2="";
            otherchannels=""; 
            paymode2="";
            productcode2="";
            implemtype2="";
            tranxstatus2="";
            ORDBatchNo2="";
            fileformat_forapproval=""
            filetype_forapproval=""
            //check transaction status if selected
            if ((document.getElementById("ddltranstatus2").value == "null"))
            {
                alert("Please select transaction status to process");
                document.getElementById("ddltranstatus2").focus();

                 return;
            }
            tranxstatus2=document.getElementById("ddltranstatus2").value ;
            // Numeric Validation
            if ((IsNumeric(document.getElementById("txtnumtrans").value) == false)  && (document.getElementById("txtnumtrans").value != null) && (document.getElementById("txtnumtrans").value !=''))
            {
                alert("Please check - non numeric value!");
            }
            numtrans=document.getElementById("txtnumtrans").value;
                
            // Date Validation
            //            
            tranxdate2=document.getElementById("txttranxdate2").value;
            //        
            if (tranxdate2 != null && tranxdate2 != '')
            {
                if (this.isDate(tranxdate2)==false)
                {
                    tranxdate2.focus; return;
                }
            }
                
            // check for null values
            if ((document.getElementById("ddlotherchannels").value != "null"))
            {
                otherchannels=document.getElementById("ddlotherchannels").value;
            }
            if ((document.getElementById("ddlpaymode2").value != "null"))
            {
                paymode2=document.getElementById("ddlpaymode2").value;
            }   
            if ((document.getElementById("ddlproductcode2").value != "null"))
            {
                productcode2=document.getElementById("ddlproductcode2").value;
            }   
            if ((document.getElementById("ddlimplemtype2").value != "null"))
            {
                implemtype2=document.getElementById("ddlimplemtype2").value;
            }   
            if ((document.getElementById("ddlORDBatchNo").value != "null"))
            {
                ORDBatchNo2=document.getElementById("ddlORDBatchNo").value;
            }     
                
            if ((document.getElementById("ddlfileformat_forapproval").value != "null"))
            {
                fileformat_forapproval=document.getElementById("ddlfileformat_forapproval").value;
            }   
            if ((document.getElementById("ddlfiletype_forapproval").value != "null"))
            {
                filetype_forapproval=document.getElementById("ddlfiletype_forapproval").value;
            }     
                 
            this.process_batch_approval(); 
        }
        
        this.reload_param= function()
        {
            var param= IRSWorld.Remittance.RemTransView.get_previous_paramvalues()
            if (param.value != null && param.value != undefined)
            {
                var command_param = param.value.split("|");
                                        
                if (command_param.length > 0)
                {
                    fnName.remitmethod=command_param[0];
                    fnName.branchcode =command_param[1];
                    fnName.tranxdate = formatDate(command_param[2],"mm/dd/yyyy");
                    //fnName.tranxdate = command_param[2];
                    fnName.clearingdate =formatDate(command_param[3],"mm/dd/yyyy");
                    fnName.tranxstatus =command_param[4];
                    fnName.paymode =command_param[5];
                    fnName.productcode =command_param[6];
                    fnName.implemtype =command_param[7];
                    fnName.refno =command_param[8];
                    fnName.batchno =command_param[9];
                    fnName.userRole =command_param[10];
                    fnName.userID =command_param[11];
                    fnName.ORDBatchNo =command_param[12];
                    fnName.filetype =command_param[13];
                    fnName.fileformat =command_param[14];
                    fnName.statusDescription=command_param[15];
                }
                this.show_hide_menu("0");
                if (fnName.statusDescription=="")
                {
                    //document.getElementById("tdStatus").innerText="VIEWER";
                    setText("tdStatus", "VIEWER");
                }
                else
                {
                    //document.getElementById("tdStatus").innerText=fnName.statusDescription;
                    setText("tdStatus", fnName.statusDescription);
                }
                        
            }
            
            this.load_remittance_transaction();
            this.load_uni_branches();
        }
    
        this.reload_param_callback= function(param)
        {
            if (param.value != null && param.value != undefined)
            {
                var command_param = param.value.split("|");
                                        
                if (command_param.length > 0)
                {
                    fnName.remitmethod=command_param[0];
                    fnName.branchcode =command_param[1];
                    fnName.tranxdate = formatDate(command_param[2],"mm/dd/yyyy");
                    fnName.clearingdate =formatDate(command_param[3],"mm/dd/yyyy");
                    fnName.tranxstatus =command_param[4];
                    fnName.paymode =command_param[5];
                    fnName.productcode =command_param[6];
                    fnName.implemtype =command_param[7];
                    fnName.refno =command_param[8];
                    fnName.batchno =command_param[9];
                    fnName.userRole =command_param[10];
                    fnName.userID =command_param[11];
                    fnName.ORDBatchNo =command_param[12];
                    fnName.filetype =command_param[13];
                    fnName.fileformat =command_param[14];
                            
                    if (isNaN(fnName.remitmethod)  == true){ fnName.remitmethod = "";}
                    if (isNaN(fnName.branchcode)  == true){ fnName.branchcode = "";}
                    if (isNaN(fnName.tranxdate)  == true){ fnName.tranxdate = "";}
                    if (isNaN(fnName.clearingdate)  == true){ fnName.clearingdate = "";}
                    if (isNaN(fnName.tranxstatus)  == true){ fnName.tranxstatus = "";}
                    if (isNaN(fnName.paymode)  == true){ fnName.paymode = "";}
                    if (isNaN(fnName.productcode)  == true){ fnName.productcode = "";}
                    if (isNaN(fnName.implemtype)  == true){ fnName.implemtype = "";}
                    if (isNaN(fnName.refno)  == true){ fnName.refno = "";}
                    if (isNaN(fnName.batchno)  == true){ fnName.batchno = "";}
                    if (isNaN(fnName.userRole)  == true){ fnName.userRole = "";}
                    if (isNaN(fnName.ORDBatchNo)  == true){ fnName.ORDBatchNo = "";}
                    if (isNaN(fnName.filetype)  == true){ fnName.filetype = "";}
                    if (isNaN(fnName.fileformat)  == true){ fnName.fileformat = "";}
                   
                }
            }
        }
        
        this.get_param_beforeloading_trans= function()
        {
            var data_row = new Array();
            
            if (isNaN(parseInt(fnName.remitmethod)) == true){ fnName.remitmethod = "";}

            data_row[data_row.length]  =  fnName.remitmethod;
            data_row[data_row.length]  =  fnName.branchcode;
            data_row[data_row.length]  =  fnName.tranxdate; 
            data_row[data_row.length]  =  fnName.clearingdate;
            data_row[data_row.length]  =  fnName.tranxstatus;
            data_row[data_row.length]  =  fnName.paymode;
            data_row[data_row.length]  =  fnName.productcode;
            data_row[data_row.length]  =  fnName.implemtype;
            data_row[data_row.length]  =  fnName.refno;
            data_row[data_row.length]  =  fnName.batchno;
            data_row[data_row.length]  =  fnName.userRole;
            data_row[data_row.length]  =  fnName.userID;
            data_row[data_row.length]  =  fnName.ORDBatchNo;
            data_row[data_row.length]  =  fnName.filetype;
            data_row[data_row.length]  =  fnName.fileformat;
            data_row[data_row.length]  =  fnName.statusDescription;
            
            return data_row.join("|");    
        }
        
        this.load_uni_branches = function()
        {
            /* boyet */
            IRSWorld.Remittance.RemTransView.load_uni_branches(this.load_uni_branches_callback);
        }
        
        this.load_uni_branches_callback = function(result)
        {
            /* boyet */
            document.getElementById("hidethis").style.display = 'none';

            if (result != null && result.value != null)
            {
                document.getElementById("hidethis").style.display = ''; new common_method().to_load_dropdownlist('cbo_uni_branch', null, 'BranchCode','BranchName', result.value);
            }
        }
        
        this.load_uni_trans = function()
        {
            /* boyet */
            this.branchcode = document.getElementById("cbo_uni_branch").value;
                        
            if (this.branchcode != null && this.branchcode != '' && this.branchcode != 'null')
            {
                this.load_remittance_transaction();
            }
            else
            {
                alert('Please select Branch / Agent');
            }
        }
        
        this.chk_param_vals = function()
        {
            if (this.remitmethod == 'null') {this.remitmethod = '';}
            if (this.branchcode == 'null') {this.branchcode = '';}
            if (this.tranxdate == 'null') {this.tranxdate = '';}
            if (this.clearingdate == 'null') {this.clearingdate = '';}
            if (this.tranxstatus == 'null') {this.tranxstatus = '';}
            if (this.paymode == 'null') {this.paymode = '';}
            if (this.productcode == 'null') {this.productcode = '';}
            if (this.implemtype == 'null') {this.implemtype = '';}
            if (this.refno == 'null') {this.refno = '';}
            
            if (this.batchno == 'null') {this.batchno = '';}
            if (this.userRole == 'null') {this.userRole = '';}
            if (this.userID == 'null') {this.userID = '';}
            if (this.ORDBatchNo == 'null') {this.ORDBatchNo = '';}
            if (this.filetype == 'null') {this.filetype = '';}
            if (this.fileformat == 'null') {this.fileformat = '';}
        }
        
        this.load_remittance_transaction = function()
        {
            this.load_search_criteria() ;
            this.chk_param_vals(); /* boyet */
            this.grid = new grid_viewer();
            this.grid.sql_source     = "sp_VwRemittance~" + fnName.remitmethod + "~" + fnName.branchcode + "~" + fnName.tranxdate + "~" + fnName.clearingdate + "~" + fnName.tranxstatus + "~" + fnName.paymode + "~" + fnName.productcode + "~" + fnName.implemtype + "~" + fnName.refno + "~" + fnName.batchno + "~" + fnName.userRole + "~" + fnName.userID + "~" + fnName.ORDBatchNo + "~" + fnName.filetype + "~" + fnName.fileformat;
            this.grid.div_container  = document.getElementById("DIV_REMITTANCE_LIST");
            this.grid.datarow_column = 0;
            this.grid.page_size = 8;
            this.grid.open_grid_view();
            this.grid = null;   
            this.active_grid         = "DIV_REMITTANCE_LIST";
            this.show_hide_menu('0');
        }
        
        this.open_division = function(division_id)
        {
            if (division_id=="div_search")
            {
                var dcontrol = document.getElementById("ddlSearchClass");
                //dcontrol.options[4].selected = true
            }
            showDiv("div_batchupload_details",false);
            this.manipulate_search_choices();
            this.clear_search_entry();
            this.close_divisions(); 
            document.getElementById(division_id).style.display = '';       
        }
        
        this.close_divisions = function()
        {
            document.getElementById("div_search").style.display        = 'none';
            document.getElementById("div_results").style.display       = 'none';
            document.getElementById("div_tranxdetails").style.display   = 'none';
        }
        
        this.manipulate_search_choices= function()
        {
            //this.tranxstatus=document.getElementById("ddltranxstatus").selectedIndex;
            fnName.cur_value=document.getElementById("ddlSearchClass").value;
            fnName.remitmethod=document.getElementById("ddlSearchClass").value;
            fnName.productcode=document.getElementById("ddlproductcode").value;
            fnName.implemtype=document.getElementById("ddlimplemtype").value;
            fnName.tranxstatus=document.getElementById("ddltranxstatus").value;
            
            if (fnName.paymode=="null") fnName.paymode="";
            if (fnName.implemtype=="null") fnName.implemtype="";
            if (fnName.productcode=="null") fnName.productcode="";
            if (fnName.tranxstatus=="null") fnName.tranxstatus="";
            
        }
        
        this.show_hide_menu= function(param)
        {
            if (param==1)
            {
                //document.getElementById("tdMenu").width='1%';
                //document.getElementById("tdMain").width='99%'
                document.getElementById("tdMenu").style.width='1%';
                document.getElementById("tdMain").style.width='99%'
                this.show_combo_boxes("divShowMenu");
                this.hide_combo_boxes("divSearchMenu");
            }
            else
            {
                //document.getElementById("tdMenu").width='35%';
                document.getElementById("tdMenu").style.width='20%';
                //document.getElementById("tdMain").width='65%';
                document.getElementById("tdMain").style.width='80%';
                this.show_combo_boxes("divSearchMenu");
                this.hide_combo_boxes("divShowMenu");
            }
        }
        
        this.hide_combo_boxes= function(param)
        {
            document.getElementById(param).style.display        = 'none';
        }
        
        this.show_combo_boxes= function(param)
        {
            document.getElementById(param).style.display        = '';
        }
        
        this.load_search_criteria = function()
        {
            IRSWorld.TranClass.RemTranCommons.get_sub_look_up_data_perbranch("RemitMethod",fnName.branchcode,this.load_search_remittance_criteria_callback);
            IRSWorld.TranClass.RemTranCommons.get_sub_look_up_data_perbranch("TranxStatus",fnName.branchcode,this.load_tranxstatus_callback);
            IRSWorld.TranClass.RemTranCommons.get_sub_look_up_data_perbranch("Paymode",fnName.branchcode,this.load_paymode_callback);
            IRSWorld.TranClass.RemTranCommons.get_sub_look_up_data_perbranch("ProductCode",fnName.branchcode,this.load_productcode_callback);
            IRSWorld.TranClass.RemTranCommons.get_sub_look_up_data_perbranch("ImplementationType",fnName.branchcode,this.load_implemtype_callback);
            IRSWorld.Remittance.RemTransView.get_BatchUpload_FileType(this.load_batchupload_filetype_callback);
        }
        
        this.load_search_criteria_forapproval= function()
        {
            IRSWorld.Remittance.RemTransView.get_PaymodeForApproval(this.branchcode,this.load_paymode_forapproval_callback); 
            IRSWorld.Remittance.RemTransView.get_RemitMethodForApproval(this.branchcode,this.load_remitmethod_forapproval_callback); 
            IRSWorld.Remittance.RemTransView.get_ProductCodeForApproval(this.branchcode,this.load_productcode_forapproval_callback); 
            IRSWorld.Remittance.RemTransView.get_ImplemTypeForApproval(this.branchcode,this.load_implemtype_forapproval_callback); 
            IRSWorld.Remittance.RemTransView.get_TranstatusForApproval(this.branchcode,this.load_transtatus_forapproval_callback); 
            IRSWorld.Remittance.RemTransView.get_ORDBatchNoForApproval(this.branchcode,this.load_ordbatchno_forapproval_callback); 
            IRSWorld.Remittance.RemTransView.get_BatchUpload_FileType_ForApproval(this.branchcode,this.load_batchupload_filetype__forapproval_callback); 
        }
        
        this.get_batchuploaf_fileformat = function()
        {
            var batchupload_filetype = document.getElementById("ddlFiletype").value;
            if (batchupload_filetype>0)
            {
                IRSWorld.Remittance.RemTransView.get_BatchUpload_FileFormat(batchupload_filetype,this.load_batchupload_fileformat_callback);
            }
            else
            {
                document.getElementById("ddlFileFormat").selectedIndex = 0;  
            }
        }
        
        this.get_batchuploaf_fileformat_forapproval = function()
        {
            var batchupload_filetype = document.getElementById("ddlfiletype_forapproval").value;
            if (batchupload_filetype>0)
            {
                IRSWorld.Remittance.RemTransView.get_BatchUpload_FileFormat_ForApproval(batchupload_filetype,this.branchcode,this.load_batchupload_fileformat_forapproval_callback);
            }
            else
            {
                document.getElementById("ddlfileformat_forapproval").selectedIndex = 0;  
            }
        }
        
        this.load_count_forapproval= function()
        {
            IRSWorld.Remittance.RemTransView.get_Count_ForApproval(this.branchcode,this.load_count_forapproval_callback); 
        }
        
        this.load_forapproval= function()
        {
            this.clear_forapproval_entry();
            this.load_search_criteria_forapproval();
            showDiv("div_batchupload_details",false);
            showDiv("divbatchupload",false);
            this.open_division('div_tranxdetails');
        }
        
        this.load_count_forapproval_callback =function(result)
        {
            var ds = result.value;
            //document.getElementById("lblCountForReprocess").innerText    ="0";
            setText("lblCountForReprocess", "0");
            //document.getElementById("lblCountForApproval").innerText    ="0";
            setText("lblCountForApproval", "0");
            if (result.value != null && result.value != "undefined")
            {
                if (ds.Tables[0].Rows.length > 0)  
                {
                    for (var i = 0; i < ds.Tables[0].Rows.length; i++) 
                    {
                        if (ds.Tables[0].Rows[i].Transtatus==9)
                            //document.getElementById("lblCountForReprocess").innerText    =ds.Tables[0].Rows[i].Count;
                            setText("lblCountForReprocess", ds.Tables[0].Rows[i].Count);
                        else
                            //document.getElementById("lblCountForApproval").innerText    =ds.Tables[0].Rows[i].Count;
                            setText("lblCountForApproval", ds.Tables[0].Rows[i].Count);
                    }
                } 
            }     
        } 
    
        this.load_batchupload_fileformat_callback =function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlFileFormat", result.value, "FldValue", "FldDescription");
                //this.commons.to_load_dropdownlist("ddlNewCIFClass", result.value, "FldValue", "FldDescription");
                this.commons = null;        
                     
            }     
        } 
        
        this.load_batchupload_filetype_callback =function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlFiletype", result.value, "FldValue", "FldDescription");
                //this.commons.to_load_dropdownlist("ddlNewCIFClass", result.value, "FldValue", "FldDescription");
                this.commons = null;        
            }     
        } 
        
        this.load_batchupload_fileformat_forapproval_callback =function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlfileformat_forapproval", result.value, "FldValue", "FldDescription");
                //this.commons.to_load_dropdownlist("ddlNewCIFClass", result.value, "FldValue", "FldDescription");
                this.commons = null;        
                     
            }     
        } 
        
        this.load_batchupload_filetype__forapproval_callback =function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlfiletype_forapproval", result.value, "FldValue", "FldDescription");
                //this.commons.to_load_dropdownlist("ddlNewCIFClass", result.value, "FldValue", "FldDescription");
                this.commons = null;        
                     
            }     
        }
         
        this.load_transtatus_forapproval_callback =function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddltranstatus2", result.value, "FldValue", "FldDescription");
                //this.commons.to_load_dropdownlist("ddlNewCIFClass", result.value, "FldValue", "FldDescription");
                this.commons = null;        
            }     
        } 
        
        this.load_ordbatchno_forapproval_callback =function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlORDBatchNo", result.value, "FldValue", "FldDescription");
                //this.commons.to_load_dropdownlist("ddlNewCIFClass", result.value, "FldValue", "FldDescription");
                this.commons = null;        
                     
            }     
        } 
        
        this.load_remitmethod_forapproval_callback =function(result)
        {
            this.commons = new common_method();
            if (result.value != null && result.value != "undefined")
            {
                this.commons.to_load_dropdownlist("ddlotherchannels", result.value, "FldValue", "FldDescription");
                //this.commons.to_load_dropdownlist("ddlNewCIFClass", result.value, "FldValue", "FldDescription");
                      
                     
            }   
            this.commons = null;    
        } 
        
        this.load_paymode_forapproval_callback =function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlpaymode2", result.value, "FldValue", "FldDescription");
                //this.commons.to_load_dropdownlist("ddlNewCIFClass", result.value, "FldValue", "FldDescription");
                this.commons = null;                        
            }     
        } 
        
        this.load_productcode_forapproval_callback =function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlproductcode2", result.value, "FldValue", "FldDescription");
                //this.commons.to_load_dropdownlist("ddlNewCIFClass", result.value, "FldValue", "FldDescription");
                this.commons = null;                        
            }     
        } 
        
        this.load_implemtype_forapproval_callback =function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlimplemtype2", result.value, "FldValue", "FldDescription");
                //this.commons.to_load_dropdownlist("ddlNewCIFClass", result.value, "FldValue", "FldDescription");
                this.commons = null;                        
            }     
        }
         
        this.load_search_remittance_criteria_callback = function(result)
        {
            if (result != null && result != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlSearchClass", result.value, "FldValue", "FldDescription");
                //this.commons.to_load_dropdownlist("ddlNewCIFClass", result.value, "FldValue", "FldDescription");
                this.commons = null;                        
            }        
        }
        
        this.clear_search_entry=function()
        {
            //document.getElementById("txtsearchtranxdate").innerText = "";
            setText("txtsearchtranxdate", "");
            //document.getElementById("txtsearchclearingdate").innerText = "";
            setText("txtsearchclearingdate", "");
            document.getElementById("ddltranxstatus").selectedIndex = 0;
            document.getElementById("ddlpaymode").selectedIndex = 0;
            document.getElementById("ddlproductcode").selectedIndex = 0;
            document.getElementById("ddlimplemtype").selectedIndex = 0;
            //document.getElementById("txtsearchrefno").innerText = "";
            setText("txtsearchrefno", "");
            //document.getElementById("txtbatchno").innerText = "";
            setText("txtbatchno", "");
            document.getElementById("txtordbatchno").value ="";
            fnName.cleanDDL();
            
        }
        
        this.clear_forapproval_entry=function()
        {
            //document.getElementById("txttranxdate2").innerText = "";
            setText("txttranxdate2", "");
            //document.getElementById("txtnumtrans").innerText = "";
            setText("txtnumtrans", "");
            document.getElementById("ddltranstatus2").selectedIndex = 0;
            document.getElementById("ddlpaymode2").selectedIndex = 0;
            document.getElementById("ddlproductcode2").selectedIndex = 0;
            document.getElementById("ddlimplemtype2").selectedIndex = 0;
            document.getElementById("ddlotherchannels").selectedIndex = 0;
            document.getElementById("ddlORDBatchNo").selectedIndex = 0;  
            document.getElementById("ddlFileFormat").selectedIndex=0;
            document.getElementById("ddlFiletype").selectedIndex=0;
            document.getElementById("ddlfileformat_forapproval").selectedIndex = 0
            document.getElementById("ddlfiletype_forapproval").selectedIndex = 0
            fnName.cleanDDL();
    //        fnName.cleanDDL("ddltranstatus2");
    //        fnName.cleanDDL("ddlpaymode2");
    //        fnName.cleanDDL("ddlproductcode2");
    //        fnName.cleanDDL("ddlimplemtype2");
        }
        
        this.cleanDDL = function()
        {        
            removeOption("ddltranstatus2");
            removeOption("ddlproductcode2");
            removeOption("ddlimplemtype2");
            removeOption("ddlotherchannels");
            removeOption("ddlpaymode2");
            removeOption("ddlfileformat_forapproval");
            removeOption("ddlfiletype_forapproval");
        }  
        
        this.select_search=function()
        {
            var manip_controls = new common_method();
            
            this.cur_value=document.getElementById("ddlSearchClass").value;
            if (this.cur_value=="null" ||  this.cur_value=="undefined")
            {
                //document.getElementById('tblsearch').disabled = true;
                manip_controls.to_read_only("tblsearch",true);
                this.clear_search_entry();
            }
            else
            {
                //document.getElementById('tblsearch').disabled = false;
                manip_controls.to_read_only("tblsearch",false);
            }
        }
        
        this.do_gridview = function(parameter)
        {
            if (parameter != null && parameter != "undefined")
            {
                var command_param = parameter.split("$");
                                        
                if (command_param.length > 0)
                {                                                
                   if (command_param[0] == "ajx_selected")
                   {
                        switch(this.active_grid.toUpperCase())
                        {
                            case "DIV_REM_TXN" :
                                this.rem_txn_sequence_id = command_param[1];
                                this.remittance_redirection(); 
                                return;
                            case "DIV_FORAPPROVAL_LIST" :
                                return;
                        }
                    }
                    this.process_grid_viewer(parameter);
                }
            }
        }
        
        this.back_to_mainmenu=function()
        {
             IRSWorld.Remittance.RemTransView.back_to_mainmenu(this.remittance_redirection_callback);
        }
        
        this.remittance_redirection = function()
        {
            var redirect_link = null;
            
            redirect_link = "?remittance_txn_id="  + this.rem_txn_sequence_id;
            IRSWorld.Remittance.RemTransView.get_remittance_page(this.rem_txn_sequence_id, redirect_link, this.get_param_beforeloading_trans(),this.remittance_redirection_callback);
        }
        
        this.remittance_redirection_callback = function(result)
        {
            if (result != null && result.value != 'undefined' && result.value != '')
            {
                window.location = result.value; 
                return;
            }        
            alert("PAGE NOT FOUND!");
        }    
        
        this.process_grid_viewer = function(parameter)
        {
            this.grid = new grid_viewer();
            //this.grid.session_end = true;
            
             switch(this.active_grid.toUpperCase())
            {
                case "DIV_REM_TXN" :
                    this.grid.sql_source     = "sp_VwRemittance~" + fnName.remitmethod + "~" + fnName.branchcode + "~" + fnName.tranxdate + "~" + fnName.clearingdate + "~" + fnName.tranxstatus + "~" + fnName.paymode + "~" + fnName.productcode + "~" + fnName.implemtype + "~" + fnName.refno + "~" + fnName.batchno + "~" + fnName.userRole + "~" + fnName.userID  + "~" + fnName.ORDBatchNo + "~" + fnName.filetype + "~" + fnName.fileformat;
                    this.grid.div_container  = document.getElementById("DIV_REMITTANCE_LIST");
                    this.grid.datarow_column = 0; 
                    this.grid.page_size = 8;
                    this.grid.process_ajax(parameter);
                    this.grid = null;
                    break;
                case "DIV_FORAPPROVAL_LIST" :
                    this.grid.sql_source     = "sp_ForApproval_summary~"  + fnName.branchcode + "~" + fnName.userRole + "~" + fnName.userID ;
                    this.grid.div_container  = document.getElementById("div_forapproval_list");
                    this.grid.datarow_column = 0;
                    this.grid.process_ajax(parameter);
                    this.grid = null;   
              }
        }
        
        this.process_batch_approval= function()
        {
            if (document.getElementById("chkreviewdetails").checked==true)
            {
                IRSWorld.Remittance.RemTransView.retrieve_batch_approval(this.branchcode,tranxdate2,numtrans,otherchannels,paymode2,productcode2,implemtype2,tranxstatus2,fnName.userRole,ORDBatchNo2,filetype_forapproval,fileformat_forapproval,this.retrieve_batch_approval_callback);
            }
            else
            {
                IRSWorld.Remittance.RemTransView.process_batch_approval(this.branchcode,tranxdate2,numtrans,otherchannels,paymode2,productcode2,implemtype2,tranxstatus2,fnName.userRole,ORDBatchNo2,filetype_forapproval,fileformat_forapproval,this.process_batch_approval_callback);
                //refresh combo box
            }
        }
        
        this.process_batch_approval_callback= function(param)
        {
                   
            alert(param.value);
            fnName.clear_forapproval_entry();
            fnName.load_search_criteria_forapproval();  
            
        }
        
        this.retrieve_batch_approval_callback= function(param)
        {
            if (param != null && param != "undefined")
            {
                create_forapproval_grid(param.value);
                document.getElementById("div_border").height = (document.getElementById("div_forreview_list").height) + 10;
                document.getElementById("div_reviewdetails").height = (document.getElementById("div_border").height) + 20;
                
                showPanel("div_reviewdetails");
                fnName.clear_forapproval_entry();
                fnName.load_search_criteria_forapproval();  
            }
        }
        
        this.load_tranxstatus= function()
        {
        
            IRSWorld.TranClass.RemTranCommons.get_sub_look_up_data_generic("TranxStatus",this.load_tranxstatus_callback);
            //select value - document.getElementById(element)[document.getElementById(element).selectedIndex].text
            //select id - document.getElementById(element).selectedIndex

            //common_method.to_load_dropdownlist(element, dataset, "VALUE", "CODE")
        }
        
        this.load_tranxstatus_callback= function(result)
        {
            if (result != null && result != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddltranxstatus", result.value, "FldValue", "FldDescription");
                //this.commons.to_load_dropdownlist("ddlNewCIFClass", result.value, "FldValue", "FldDescription");
                this.commons = null;                        
            }  
        }
        
        this.load_paymode_callback=function(result)
        {
           if (result != null && result != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlpaymode", result.value, "FldValue", "FldDescription");
                //this.commons.to_load_dropdownlist("ddlNewCIFClass", result.value, "FldValue", "FldDescription");
                this.commons = null;                        
            }  
        }
        
        this.load_productcode_callback=function(result)
        {
           if (result != null && result != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlproductcode", result.value, "FldValue", "FldDescription");
                //this.commons.to_load_dropdownlist("ddlNewCIFClass", result.value, "FldValue", "FldDescription");
                this.commons = null;                        
            }  
        }
        
        this.load_implemtype_callback=function(result)
        {
           if (result != null && result != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlimplemtype", result.value, "FldValue", "FldDescription");
                //this.commons.to_load_dropdownlist("ddlNewCIFClass", result.value, "FldValue", "FldDescription");
                this.commons = null;                        
            }  
        }
        
        this.division_loading_progress = function()
        {
            this.commons = new common_method();
            //this.commons.to_show_loading_message("div_search");
            this.commons.to_show_loading_message("DIV_REMITTANCE_LIST");
            //this.commons.to_show_loading_message("DIV_PROD_IMPLEM");        
            this.commons = null;
        }
        
        this.load_forapproval_summary = function()
        {
            this.load_search_criteria() ; 
            this.grid = new grid_viewer();
            this.grid.sql_source     = "sp_ForApproval_summary~"  + fnName.branchcode + "~" + fnName.userRole + "~" + fnName.userID ;
            this.grid.div_container  = document.getElementById("div_forapproval_list");
            this.grid.datarow_column = 0;
            
            //this.grid.process_ajax('ajx_loading');
            this.grid.open_grid_view();
            this.grid = null;   
            this.active_grid         = "div_forapproval_list";
            showPanel("div_forapproval");
            this.load_count_forapproval();
        }
        
        this.process_for_approval_with_viewdetails= function()
        {
            fnName.parse_for_approval_trans(chargrid);
        }
        
        this.parse_for_approval_trans = function(dbgrid)
        {
            var refno= "";
            for (var i=0; i < dbgrid.getDataRowsCount(); i++)
            {
	            var val = dbgrid.getDataAt(i, 13);
	            if (val==false)
	            {
	                refno += dbgrid.getDataAt(i, 0) + "~";
	            }
            }
            
            if (refno.length>0)
            {
                IRSWorld.Remittance.RemTransView.process_batch_approval_with_review(refno,this.branchcode,tranxstatus2,this.parse_for_approval_trans_callback);
            }
            else
            {
                alert("No record(s) to be approved.");
            }
        }
        
        this.parse_for_approval_trans_callback = function(param)
        {
            alert("Transaction successfully approved. \r\n\r\n" + param.value);
            fnName.clear_forapproval_entry();
            fnName.load_search_criteria_forapproval();  
            hidePanel("div_reviewdetails");
        }
        
        this.manipulate_typeofremittance_choices1= function()
        {
            var rem_method=document.getElementById("ddlotherchannels").value;
            if (rem_method==1)
                {document.getElementById("ddlfileformat_forapproval").selectedIndex=0;
                document.getElementById("ddlfiletype_forapproval").selectedIndex=0;
                showDiv("divbatchupload",true);}
            else
                {showDiv("divbatchupload",false);}
         
        }
        this.manipulate_typeofremittance_choices2= function()
        {
            var rem_method=document.getElementById("ddlSearchClass").value;
            if (rem_method==1)
                {document.getElementById("ddlFileFormat").selectedIndex=0;
                document.getElementById("ddlFiletype").selectedIndex=0;
                showDiv("div_batchupload_details",true);}
            else
                {showDiv("div_batchupload_details",false);}
                
            this.manipulate_search_choices();
        }
        
    }
