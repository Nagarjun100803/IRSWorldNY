// Added By Jay And Liz 2010

function generic_method()
{
     /****************************************************************************************************
        ADDED by Liz    
    *****************************************************************************************************/
    this.isNumberKey = function(evt)
    {
        var charCode = (evt.which) ? evt.which : event.keyCode
        if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

        return true;
    }
    
    this.autoFormatCurrency = function(fld, milSep, decSep, e)
    {
        var sep = 0;
        var key = "";
        var i = j = 0;
        var len = len2 = 0;
        var strCheck = "0123456789";
        var aux = aux2 = "";
        var whichCode = (window.Event) ? e.which : e.keyCode;

        if (whichCode == 13) return true; // Enter
        if (whichCode == 8) return true; // Delete
        if (whichCode == 9) return true; // Tab
        if (whichCode == 11) return true; // Tab
        key = String.fromCharCode(whichCode); // Get key value from key code
        if (strCheck.indexOf(key) == -1) return false; // Not a valid key
        len = fld.value.length;
        
        for(i = 0; i < len; i++)
        if ((fld.value.charAt(i) != "0") && (fld.value.charAt(i) != decSep)) break;
        aux = "";
        for(; i < len; i++)
        if (strCheck.indexOf(fld.value.charAt(i))!=-1) aux += fld.value.charAt(i);
        aux += key;
        len = aux.length;
        if (len == 0) fld.value = "";
        if (len == 1) fld.value = "0" + decSep + "0" + aux;
        if (len == 2) fld.value = "0" + decSep + aux;
        if (len > 2) 
        {
            aux2 = '';
            for (j = 0, i = len - 3; i >= 0; i--) 
            {
                if (j == 3) 
                {
                    aux2 += milSep;
                    j = 0;
                }
                aux2 += aux.charAt(i);
                j++;
            }
            fld.value = "";
            len2 = aux2.length;
            for (i = len2 - 1; i >= 0; i--)
            fld.value += aux2.charAt(i);
            fld.value += decSep + aux.substr(len - 2, len);
        }
        return false;
    }
    
    this.formatCurrency = function(num)
    {
        if(num == null) num = 0;
        num = num.toString().replace(/\$|\,/g,'');
        if(isNaN(num))
        num = "0";
        sign = (num == (num = Math.abs(num)));
        num = Math.floor(num*100+0.50000000001);
        cents = num%100;
        num = Math.floor(num/100).toString();
        if(cents<10)
        cents = "0" + cents;
        for (var i = 0; i < Math.floor((num.length-(1+i))/3); i++)
        num = num.substring(0,num.length-(4*i+3))+','+
        num.substring(num.length-(4*i+3));
        return (((sign)?'':'-') + num + '.' + cents);
    }
    
    this.is_numeric = function(field_value)
    {
        var regexps = /(^-?\d\d*\.\d*$)|(^-?\d\d*$)|(^-?\.\d\d*$)|(^\d*$)/;
      
        return regexps.test(field_value);
    }
    
    this.is_alpha = function(field_value)
    {
        var regexps = /^([a-zA-Z\s\-])*$/;
        
        return regexps.test(field_value); 
    }
    
    this.is_empty = function(field_value)
    {   
        if (this.to_trim(field_value).length > 0) return false;
        else return true;
    }
    
    this.compute_age = function(field_value)
    {
        this.bdate  = field_value.split("/");
        this.bmonth = Number(this.bdate[0])-1;
        this.bday   = Number(this.bdate[1]);
        this.byear  = Number(this.bdate[2]);
        this.today  = new Date();
        var age     = 0;
        
        if((this.today.getMonth() > this.bmonth)||(this.today.getMonth() == this.bmonth & this.today.getDate() >= this.bday)) age = this.byear;
        else age = this.byear + 1; 
        
        return this.today.getFullYear() - age;
    }
    
    this.to_trim = function(field_value)
    {
        var temp = new Array();
        
        for(i = 0; i< field_value.length; i++)
        {
            if (field_value.charAt(i) != ' ')
            {
                temp += field_value.charAt(i);
            }
        }
        return temp; 
    }
    
    this.to_remove_dash = function(comma_field)
    {
        var regexps = /-/g;  return comma_field.replace(regexps,'');
    }
    
    this.to_read_only = function(tabid, read_only)
    {
        var tblrows = document.getElementById(tabid).rows;
        
        if (tblrows == null || tblrows == "undefined")
        { 
            return;
        }
        
        for (var i = 0; i < tblrows.length; i++)
        {
            var sells = tblrows[i].cells;
            
            for (var j = 0; j < sells.length; j++)
            {
                if (sells.item(j).hasChildNodes())
                {
                    var childs = sells.item(j).childNodes;
                    
                    for (var k = 0; k < childs.length; k++)
                    {
                       if ((childs[k].nodeName == 'INPUT' || childs[k].nodeName == 'SELECT' || childs[k].nodeName == 'LABEL') && childs[k].innerHTML != '*')
                       {
                            childs[k].disabled = read_only;
                       }
                    }
                }
            }
        }
    }
    
    this.to_clear_screen = function(tabid)
    {
        var tblrows = document.getElementById(tabid).rows;
                              
        if (tblrows == null || tblrows == "undefined")
        { 
            return;
        }
        
        for (var i = 0; i < tblrows.length; i++)
        {                            
            var sells = tblrows[i].cells;

            for (var j = 0; j < sells.length; j++)
            {
                if (sells.item(j).hasChildNodes())
                {
                    var childs = sells.item(j).childNodes;

                    for (var k=0; k < childs.length; k++)
                    {   
                        if (childs[k].nodeName == 'INPUT')
                        {
                            if (childs[k].type == 'text') 
                            {
                                childs[k].value = '';
                            }
                            else if (childs[k].type == 'file')
                            {
                                var file_upload  = childs[k];
                                var file_upload2 = file_upload.cloneNode(false);
                                file_upload2.onchange = file_upload.onchange;
                                file_upload.parentNode.replaceChild(file_upload2,file_upload); 
                            }
                            else if (childs[k].type == 'checkbox') 
                            { 
                                childs[k].checked = false;
                            }
                        }
                        else if (childs[k].nodeName == 'SELECT'){childs[k].selectedIndex = 0;}            
                        else if (childs[k].nodeName == 'SPAN'  ){childs[k].innerHTML = '';   }            
                        else if (childs[k].nodeName == 'LABEL') 
                        {
                            /* edited for red asterisk */                            
                            if ((childs[k].id != null || childs[k].id != '') && childs[k].innerHTML != '*')
                            {
                                childs[k].innerHTML = '';
                            }
                        }
                    }
                }
            }
        }        
    }
    
    this.to_clear_null = function(fldval_elems)
    {
        var fldval_array = fldval_elems.split("~");
        var y = 0;
       
        for(y = 0; y < fldval_array.length; y++)
        {
           if (fldval_array[y] != null || fldval[y] != "")
           {    
                if ((fldval_array[y]).substr(0,3).toUpperCase() == 'TXT')
                {
                    if 
                    (
                        document.getElementById(fldval_array[y]).value == '' ||
                        document.getElementById(fldval_array[y]).value.length == 0 ||
                        document.getElementById(fldval_array[y]).value.toString().toUpperCase() == 'NULL' ||
                        document.getElementById(fldval_array[y]).value.toString().toUpperCase() == 'UNDEFINED'
                    )
                    {
                        document.getElementById(fldval_array[y]).value = ''; 
                    }
                }
                else if ((fldval_array[y]).substr(0,3).toUpperCase() == 'LBL' || (fldval_array[y]).substr(0,4).toUpperCase() == 'SPAN' || (fldval_array[y]).substr(0,3).toUpperCase() == 'SPN' ) 
                {
                    if 
                    (
                        document.getElementById(fldval_array[y]).innerHTML == '' || 
                        document.getElementById(fldval_array[y]).innerHTML.length == 0 || 
                        document.getElementById(fldval_array[y]).innerHTML.toString().toUpperCase() == 'NULL' ||
                        document.getElementById(fldval_array[y]).innerHTML.toString().toUpperCase() == 'UNDEFINED'
                    )
                    {
                        document.getElementById(fldval_array[y]).innerHTML = '';
                    }
                }
            } 
        }
    }
    
    this.to_validate_phone = function (field_value)
    {
       if (field_value != null && field_value != '')        
       {
            if (this.to_trim(field_value) == '--') return true;
        
            var phone_number = new RegExp(/^\([1-9]\d{2}\)\s?\d{3}\-\d{4}$/);

            if (!phone_number.test(field_value)) 
            {
             return false;
            }   
            return true;
       }
       return true;
    }
    
    this.to_validate_credit_card_secno = function(field_value)
    {
        var security_number = new RegExp(/^\d{3}$/);
        if(field_value != null && field_value != '' && !security_number.test(field_value)) 
        {
            return false;
        }
        return true;
    }
    
    this.to_validate_debit_card_secno = function(field_value)
    {
        var security_number = new RegExp(/^\d{4}$/);
        if(field_value != null && field_value != '' && !security_number.test(field_value))
        {
            return false;
        }
        return true;
    }
    
    this.to_validate_date = function(field_value)
    {
        var date = new RegExp(/((^\d{1,2}\/\d{1,2}\/\d{4}$)|(^\d{1,2}\/\d{4}$))/);
        
        if(field_value != null && field_value != '' && !date.test(field_value))
        {
            return false;
        }
        else
        {
            var arrayDate = field_value.split('/'); 
        }
        return true;
    }
    
    this.validate_USDate = function(field_value)
    {
        //var objRegExp = /^\d{1,2}(\/)\d{1,2}\1\d{4}$/
        var objRegExp = /((^\d{1,2}\/\d{1,2}\/\d{4}$)|(^\d{1,2}\/\d{4}$))/;
        
        
        if(field_value != '' && !objRegExp.test(field_value))
        {
            return false; //doesn't match pattern, bad date
        }  
        else if (field_value == '')
        {
            return true;
        }      
        else
        {
            var arrayDate = field_value.split('/'); 
            
            if(arrayDate.length == 2)
            {
                var int_month = parseInt(arrayDate[0],10);
                var year      = Number(arrayDate[1]);
                
                if (int_month < 1 || int_month > 12 || year < 1753)
                {
                    return false;
                }
                return true; 
            }
            else
            {
                //create a lookup for months not equal to Feb.
                var arrayLookup = { '01' : 31,'03' : 31, '04' : 30,'05' : 31,
                                    '06' : 30,'07' : 31, '08' : 31,'09' : 30,
                                    '10' : 31,'11' : 30, '12' : 31}
            
                var intMonth    = parseInt(arrayDate[0],10); 
                var intDay      = parseInt(arrayDate[1],10); 
                var stringMonth = String(intMonth);
            
                if(stringMonth.length == 1)
                {
                    stringMonth = '0' + stringMonth;
                }
            
                //check if month value and day value agree
                if(arrayLookup[stringMonth] != null) 
                {
                    if(intDay <= arrayLookup[stringMonth] && intDay != 0)
                    return true; //found in lookup table, good date
                }
                    var intMonth = parseInt(stringMonth,10);
                    if (intMonth == 2) { 
                    var intYear = parseInt(arrayDate[2]);
                    if (intDay > 0 && intDay < 29) {
                        return true;
                    }
                    else if (intDay == 29) 
                    {
                        if ((intYear % 4 == 0) && (intYear % 100 != 0) || (intYear % 400 == 0)) 
                        {
                            return true;
                        }   
                    }
                }    
            }
        }  
        return false; //any other values, bad date
    }
    
    this.to_validate_issdate_mmyyyy = function(issdate)
    {
        var beeday = new RegExp(/((^\d{1,2}\/\d{4}$))/);
        
        if (issdate != null && issdate != '')
        {
            if (beeday.test(issdate))
            {
                this.bdate = issdate.split('/');
                this.Month = Number(this.bdate[0])-1;
                this.Date  = 01;
                this.Year  = Number(this.bdate[1]);
               
                this.today      = new Date();
                this.IssDate    = new Date();
                this.date111753 = new Date(); //the oldest possible date 1/1/1753
                
                this.IssDate.setFullYear(this.Year,this.Month,this.Date);    
                this.date111753.setFullYear(1753, 0, 1);
                
                if(this.IssDate >= this.date111753 && this.IssDate <= this.today)
                {
                    return true;
                }
                return false;                    
            }
            return false;
        }
        return true;              
    }
    
    this.validate_mmyyyy = function(field_value)
    {
        var objRegExp = /((^\d{1,2}\/\d{4}$))/;        
        
        if(field_value != '' && !objRegExp.test(field_value))
        {
            return false; //doesn't match pattern, bad date
        }       
        else
        {
            var arrayDate = field_value.split('/'); 
            
            if(arrayDate.length > 2) return false;
            if(arrayDate.length == 2)
            {
                var int_month = parseInt(arrayDate[0],10);
                var year      = Number(arrayDate[1]);
                
                if (int_month < 1 || int_month > 12 || year < 1753)
                {
                    return false;
                }
                return true; 
            }            
        }  
        return false; //any other values, bad date
    }
    
    this.to_validate_bdate = function(birthdate)
    {
        var beeday = new RegExp(/^\d{1,2}\/\d{1,2}\/\d{4}$/);
        
        if (birthdate != null && birthdate != '')
        {
            if (beeday.test(birthdate))
            {
                this.bdate = birthdate.split('/');
                this.Month = Number(this.bdate[0])-1;
                this.Date  = Number(this.bdate[1]);
                this.Year  = Number(this.bdate[2]);
               
                this.today      = new Date();
                this.BirthDate  = new Date();
                this.date111753 = new Date(); //the oldest possible date 1/1/1753
                
                this.BirthDate.setFullYear(this.Year,this.Month,this.Date);    
                this.date111753.setFullYear(1753, 0, 1);
                
                if(this.BirthDate >= this.date111753 && this.BirthDate <= this.today)
                {
                    return true;
                }
                return false;                    
            }
            return false;
        }
        return true;              
    }
    
    this.is_positive = function(fieldvalue)
    {
        if(fieldvalue != null && fieldvalue != '' && Number(fieldvalue) < 0)
        {
            return false;
        }
        return true;
    }
    
    this.to_validate_expiration = function(expiry_date)
    {
        this.exp_date = expiry_date.split('/');
        if(this.exp_date.length == 3)
        {
            this.Month    = Number(this.exp_date[0])-1;
            this.Date     = Number(this.exp_date[1]);
            this.Year     = Number(this.exp_date[2]);
        }
        else if(this.exp_date.length == 2)
        {
            this.Month    = Number(this.exp_date[0])-1;
            this.Date     = 01;
            this.Year     = Number(this.exp_date[1]);
        }            
       
        this.today          = new Date();
        this.ExpirationDate = new Date();
        
        this.ExpirationDate.setFullYear(this.Year,this.Month,this.Date);    
      
        if(expiry_date != null && expiry_date != '' && this.ExpirationDate < this.today)
        {
            return false;
        }
        return true;              
    }
    
    this.to_mmyyyy_date = function(dt)
    {
        if (dt == null) return null;
        if (dt.constructor == Date)
        {
            var mo = "" + (dt.getMonth() + 1);
                            
            return ((mo.length == 1) ? "0" + mo : "" + mo) + "/" + dt.getFullYear();
        }
    }
    
    this.to_validate_email = function (field_value)
    {
       if (field_value != null && field_value != '')        
       {
            //var regexps  = /(^[a-z]([a-z_\.\d]*)@([a-z_\.]*)([.][a-z]{3})$)|(^[a-z]([a-z_\.]*)@([a-z_\.]*)(\.[a-z]{3})(\.[a-z]{2})*$)/i;            
            var regexps  = /^(([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5}){1,25})+([;.](([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5}){1,25})+)*$/;            
        
            if (!regexps.test(field_value)) 
            {
                return false;
            }   
            return true;
       }
       return true;
    }
    
    this.is_alphanumeric_bus = function(field_value)
    {
        var regexps = /^([\w\s\,\-\.\:\#\'\&\)\(])*$/;
        
        return regexps.test(field_value); 
    }
    
    /****************************************************************************************************/
    
//    this.is_numeric2 = function(field_value)
//    {
//        //added by Jay
//        //for ip address
//        var regexps = /(^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$)/;
//      
//        return regexps.test(field_value);
//    }
    this.checkipadd = function(field_value)
    {
      var regexps =  /^(\d{1,3})\.(\d{1,3})\.(\d{1,3})\.(\d{1,3})$/;
      return regexps.test(field_value);
    }
    this.is_alphanumeric = function(field_value)
    {
        //added by Jay
        var regexps = /^([\w\s ])*$/;
        
        return regexps.test(field_value); 
    }
    this.is_alphanumeric_branchcode = function(field_value)
    {
        //for t$$ branchcode
        var regexps = /^([\w\s ])*/;
        
        return regexps.test(field_value); 
    }
    this.is_alphanumeric2 = function(field_value)
    {
        //added by Jay
        // allows -  . : , ( )
        var regexps = /^([\w\s\,\-\.\:\#\)\(])*/;
        
        return regexps.test(field_value); 
    }
    
    this.to_validate_pass_length = function(field_value)
    {
        if (field_value.length < 6)
        {
            return false;
        }
        return true;
        
    }
    this.to_validate_pass = function(field_value)
    {
        var regexps = /^(\w)*$/;
        
        return regexps.test(field_value); 
    }
    
    this.to_validate_time = function(field_value)
    {
         // Checks if time is in HH:MM:SS AM/PM format.
        // The seconds and AM/PM are optional.

        var regexps = /^(\[1-23]{1,2}):(\[0-59]{2})(:(\[0-59]{2}))?(\s?(AM|am|PM|pm))?$/;
        
        return regexps.test(field_value);
        
    }
    this.to_set_null = function(field_value)
    {
        if (field_value == 'undefined')
        {
            return null;
        }
        
        if (field_value != null)
        {
            if (field_value == '' || field_value.length == 0 || field_value.toString().toUpperCase() == 'NULL')
            {
                return null;
            }
        }
        
        return field_value;
    }
   
    this.to_validate_intlphone = function (field_value)
    {
        //added by Jay
       if (field_value != null && field_value != '')        
       {
            if (this.to_trim(field_value) == '--') return true;
            
            var phone_number = new RegExp(/^((\+[1-9]{1,4}[ \-]*)|(\([0-9]{2,3}\)[ \-]*)|([0-9]{2,4})[ \-]*)*?[0-9]{3,4}?[ \-]*[0-9]{3,4}?$/);
           
            if (!phone_number.test(field_value)) 
            {
             return false;
            }   
            return true;
       }
       return true;
    }
    
    this.days_passed = function(field_value)
    {
        //added by Jay
        this.SetDate = field_value.split('/');
        this.myDate   =new Date(); 
        this.Month    = Number(this.SetDate[0])-1;
        this.Date     = Number(this.SetDate[1]);
        this.Year     = Number(this.SetDate[2]);
        this.yourDate =new Date(this.Year,this.Month,this.Date);   
        this.secondsInADay = 1000*60*60*24;   
      
        return  this.diff = Math.floor( (this.myDate.getTime() - this.yourDate.getTime()) / this.secondsInADay );   
        
    }

}
