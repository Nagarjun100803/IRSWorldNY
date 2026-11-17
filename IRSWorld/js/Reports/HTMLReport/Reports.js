var HTMLRep = new HTMLReport();

function testPopupBlocker()
{
    //Formatted for layout sake
    var myPopup = window.open("about:blank", "",    "directories=no,height=150,width=150,menubar=no,    resizable=no,scrollbars=no,status=no,titlebar=no,top=0,location=no");

    if (!myPopup)
        return true;
    else {
            setTimeout(function() {
                //Use to test for Google Chrome Pop-Up
                if (myPopup.screenX == 0) {
                    alert("Please disable popup blocker for this site then try again.");
                    return true;
                } else {
                   
		            return false;
                    myPopup.close();
                }
            }, 0);
        }
}


function HTMLReport()
{
    this.load_report = function()
    {        
    
        var qstr = this.get_hidden_querystring("hidQueryString");
        
        if (qstr.length > 0)
        {
              
              var query = this.get_hidden_querystring("hidQueryString");
              var newwindow;
              try
              {
             
                  var dom  = parent.document;
    
    
                  newwindow   =   window.open(' ' +  '../HTMLReport/Reports.aspx?' + query ,'mywin1','location=0,addressbar=0,menubar=0,toolbar=0,resizable=1,status=0,scrollbars=1');                                   
                  newwindow.test = dom;
                  newwindow.focus();
              }
              catch(e)
              {
                  alert(e);
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
                    return jtemp[1].replace(/\+/g," ");  
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
    this.get_merge_param_values = function(squery, indx)
    {
        var squery = unescape(squery.toString());
        var stemp = squery.split("&");
        var rtemp = "";
        try
        {
            for(i = 0; i < stemp.length; i++ )
            {
                if (stemp[0].length > 0)       
                {
                    rtemp = stemp[0].split("^");
                    
                    return rtemp[indx];
                }
            }
        }
        catch(e)
        {
        }
        return "";
    }
    this.get_querystring = function()
    {
        return window.location.search.substring(1);
    }
    this.general_styles = function()
    {
        var shtml = "";
        shtml  += '<style type="text/css">';        
        shtml  += 'body ';
        shtml  += '{ zoom: 100%;background-color:white;';
        shtml  += '}';
        shtml  += 'table ';
        shtml  += '{';
        shtml  += '    font-family: arial;';
        shtml  += '}';
        shtml  += 'td';
        shtml  += '{';
        shtml  += '    font-family: arial;';
        shtml  += '    font-size:9pt;      ';      
        shtml  += '}';
        shtml  += '.header';
        shtml  += '{';
        shtml  += '    font-weight:bold;';
        shtml  += '    font-family: arial;';
        shtml  += '    font-size:12pt;      ';
        shtml  += '}';
        shtml  += '.tdheader';
        shtml  += '{';
        shtml  += '    font-weight:bold;';
        shtml  += '    width: 306px;';
        shtml  += '    height: 30 px;';
        shtml  += '    font-size: 12pt;';
        shtml  += '}';
        shtml  += ' .colheader';
        shtml  += '{';
        shtml  += '    font-weight:bold;            ';
        shtml  += '    font-size: 9pt;';
        shtml  += '}';
        shtml  += '.tdlabel';
        shtml  += '{';
        shtml  += '    font-weight:bold;';
        shtml  += '    width: 306px;';
        shtml  += '    height: 30 px;';
        shtml  += '}';
        shtml  += '.imgtd';
        shtml  += '{       ';    
        shtml  += '               ';
        shtml  += '}';
        shtml  += '.tab_border';
        shtml  += '{';
        shtml  += '    border-color:black;';
        shtml  += '}';
        shtml  += 'body';
        shtml  += '{';
        shtml  += 'text-align:center;';
        shtml  += 'font-family: Arial;';
        shtml  += 'font-size: 10.5px;';
        shtml  += '}';    
        shtml  += ' .tdsmall';
        shtml  += '{';
        shtml  += '    font-family: arial;';
        shtml  += '    font-size:8pt;';
        shtml  += '}';
        shtml  += '.tdsmallb';
        shtml  += '{';
        shtml  += '    font-family: arial;';
        shtml  += '    font-weight:bold;';
        shtml  += '    font-size:8pt;';
        shtml  += '} ';
        shtml  += '</style>';
        return shtml;
    }
     this.general_print_styles = function()
    {
        var shtml = "";
        shtml  += '<style type="text/css" media="print">';
        shtml  += 'body ';
        shtml  += '{';
        shtml  += '    width:1700px;';
        shtml  += '    height:1100px';
        shtml  += '}';
        shtml  += 'table ';
        shtml  += '{';
        shtml  += '    font-family: arial;';
        shtml  += '}';
        shtml  += 'td';
        shtml  += '{';
        shtml  += '    font-family: arial;';
        shtml  += '    font-size:9pt;      ';      
        shtml  += '}';
        shtml  += '.header';
        shtml  += '{';
        shtml  += '    font-weight:bold;';
        shtml  += '    font-family: arial;';
        shtml  += '    font-size:12pt;      ';
        shtml  += '}';
        shtml  += '.tdheader';
        shtml  += '{';
        shtml  += '    font-weight:bold;';
        shtml  += '    width: 306px;';
        shtml  += '    height: 30 px;';
        shtml  += '    font-size: 12pt;';
        shtml  += '}';
        shtml  += ' .colheader';
        shtml  += '{';
        shtml  += '    font-weight:bold;            ';
        shtml  += '    font-size: 9pt;';
        shtml  += '}';
        shtml  += '.tdlabel';
        shtml  += '{';
        shtml  += '    font-weight:bold;';
        shtml  += '    width: 306px;';
        shtml  += '    height: 30 px;';
        shtml  += '}';
        shtml  += '.imgtd';
        shtml  += '{       ';    
        shtml  += '               ';
        shtml  += '}';
        shtml  += '.tab_border';
        shtml  += '{';
        shtml  += '    border-color:black;';
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
    this.print_html_button = function()
    {
        var shtml = "";
        shtml += '<table id="tableprint">';
        shtml += '<tr>';
        shtml += '<td> <input type="button" value="PRINT" onclick="javascript:HTMLRep.print_window();"> </td>';
        shtml += '</tr>';
        shtml += '</table>';
        return shtml;
    }
    this.print_window = function()
    {
        document.getElementById("tableprint").style.display = "none";
        window.focus();window.print();window.close();
        var o = test;
    }
  
     this.format_numbers = function(snum,decpla)
    {
        try
        {
    	    if (isNaN(snum))
	        {
	          return snum;
	        }

        		
	        var sign = (snum == (snum = Math.abs(snum)));

	        snum = Math.abs(snum);
	        snum =  snum.toString();
	        snum = snum.replace(",","");

	        var dotidx = snum.indexOf(".");
	        var decnum = (dotidx == -1 ) ? 0 : snum.substring(dotidx);
	        
	     

	        if (decnum > 0)
	        {
	            if ((decnum.length - 1) >= decpla)
	            {
		            decnum = decnum.substring(0,decpla + 1)
	            }	          
	            else
	            {
	                
	                decnum  = this.populate_zero(decnum,decpla);
	            }
            }
            else
	        {
	                decnum  = this.populate_zero(decnum,decpla);
	        }
	        
            var snumbers;
            if (dotidx > 0 )
            {
	            snumbers = (dotidx > 0) ? snum.substring(0, dotidx) : snumbers.length ;
	        }
	        else
	        {
	            snumbers = snum;
	        }

	        var stempnum = "";
	        var ctr = 0;

	        for(i = snumbers.length; i > 0 ; i--)
	        {	
		        ctr++;

		        if ((ctr  % 3 )  == 0 && ctr !=  snumbers.length)
		        {
		          stempnum =      "," +  snumbers.substring(i, i - 1).toString() + stempnum ;
		        }
		        else
		        {
		          stempnum = snumbers.substring(i, i - 1).toString() + stempnum ;
		        }

	        }
	        return ((sign) ? "" : "-" ) +  stempnum + decnum;
	     }
	     catch(e)
	     {
	        return snum + ".00";
	     }
    }


    this.populate_zero = function(decnum,decpla)
    {
        var tempdecnum = "";
        
        if (decnum == 0)
        {
            return ".00";
        }
        for(z=0; z < (decpla - (decnum.length - 1)); z++)
        {
            tempdecnum += "0";
        }
	    decnum =  (decnum.indexOf(".") > -1 )? decnum.toString()  +  tempdecnum.toString() :  "." + decnum.toSTring()  +  tempdecnum.toString() ;
	    
	    return decnum;
    }

}

