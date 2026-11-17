
function common_method()
{
    /*    
        SA LAHAT NG MAGLALAGAY NG KANILANG METHOD DITO SA "common_method" NA JS OBJECT 
        
        1. PAKILAGAY PANGALAN NYO.
        2. SMALL CASE LANG LAHAT NG METHOD NAME. SEPARATED NG UNDERSCORE KAPAG MEDYO MAHABA YUNG METHOD NAME NYO.
        
        SALAMAT!            
    */

    /* START OF JS COMMON METHOD. AUTHOR: DANIEL D. COLCOL III DATE MODIFIED: FEB 2010*/

    this.to_show_loading_message = function(htmldivs)
    {
        var msg = "<CENTER><IMG SRC = '../images/ajaxloader.gif'></IMG></CENTER>";
        var divs = htmldivs.split("~");
        var x = 0;

        for(x = 0; x < divs.length; x++)
        {
            if (divs[x] != null || divs[x] != "")
            {
                document.getElementById(divs[x]).innerHTML = '';
                document.getElementById(divs[x]).innerHTML = msg;
            }
        }
    }
    
    this.is_numeric = function(field_value)
    {        
        var regexps = /(^-?\d\d*\.\d*$)|(^-?\d\d*$)|(^-?\.\d\d*$)|(^\d*$)/; return regexps.test(field_value);
    }
    
    this.is_decimal = function(decimal_value)
    {
        var regexps = /^((\d*\.\d{2})|(\d*)|(\d*\.\d{2}))$/; return regexps.test(decimal_value);
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

    this.to_load_dropdownlist = function(dropdown_id, dataset, column_value, column_desc, data_table)
    {            
        var i = 0; 
        var dcontrol     = document.getElementById(dropdown_id);
        var drop_options = document.createElement("option");
        var the_record   = null;
        
        if (dataset == null && data_table != null)
        {
            the_record = data_table;
        }
        else if (dataset != null && data_table == null)
        {
            the_record = dataset.Tables[0];
        }
        
        dcontrol.options.length = 0;
        drop_options.value      = null;
        drop_options.text       = '';            
        
        try
        {
            dcontrol.add(drop_options,null); // NON INTERNET EXPLORER BROWSER
        }
        catch(ex)
        {
            dcontrol.add(drop_options); // INTERNET EXPLORER ONLY
        }                                 
        
        if (the_record != null)
        {
            for(i = 0; i < the_record.Rows.length; i++)
            {
                var drop_options = document.createElement("option");
                
                drop_options.value = the_record.Rows[i][column_value];
                drop_options.text  = the_record.Rows[i][column_desc];
                                                       
                try
                {
                    dcontrol.add(drop_options,null); // NON INTERNET EXPLORER BROWSER
                }
                catch(ex)
                {
                    dcontrol.add(drop_options); // INTERNET EXPLORER ONLY
                }
            }
        }
    }
    
    this.to_set_dropdownlist_index = function(dropdown_id, column_value)
    {
        var dcontrol = document.getElementById(dropdown_id);

        if (dcontrol != null && dcontrol.options.length > 0)
        {
            dcontrol.options[0].selected = true;
            
            for (i = 0; i < dcontrol.options.length; i++)
            {
                if (dcontrol.options[i].value == column_value)
                {
                    dcontrol.options[i].selected = true; return;
                }
            }
        }
    }

    this.to_mmddyyyy_date = function(dt)
    {
        if (dt == null) return null;
        if (dt.constructor == Date)
        {
            var mo = "" + (dt.getMonth() + 1);
            var dd = "" + dt.getDate();
                            
            return ((mo.length == 1) ? "0" + mo : "" + mo) + "/" + ((dd.length == 1) ? "0" + dd : "" + dd) + "/" + dt.getFullYear();
        }
    }

    this.to_remove_comma = function(comma_field)
    {
        if (comma_field != null || comma_field != '' || comma_field != 'undefined')
        {    
            var regexps = /,/g;  return comma_field.replace(regexps,'');
        }

        return comma_field;
    }        
    
    this.to_validate_email = function (field_value)
    {
       if (field_value != null && field_value != '')        
       {
            var regexps  = /(^[a-z]([a-z_\.\d]*)@([a-z_\.]*)([.][a-z]{3})$)|(^[a-z]([a-z_\.]*)@([a-z_\.]*)(\.[a-z]{3})(\.[a-z]{2})*$)/i;            
        
            if (!regexps.test(field_value)) 
            {
                return false;
            }   
            return true;
       }
       return true;
    }
          
    this.to_left_trim = function(trimfield) 
    {
        
        var regexps = /^(\s*)(\b[\w\W]*)$/;                                    

        if(regexps.test(trimfield)) 
        {                                               
            trimfield = trimfield.replace(regexps, '$2');                
        }            
        
        return trimfield;
    }

    this.to_right_trim = function(trimfield) 
    {
        var regexps = /^([\w\W]*)(\b\s*)$/;

        if(regexps.test(trimfield)) 
        {
            trimfield = trimfield.replace(regexps, '$1');
        }
        
        return trimfield;
    }
    
    this.to_trim_all = function(trimfield) 
    {
        trimfield = this.to_right_trim(trimfield);
        trimfield = this.to_left_trim(trimfield);
        return trimfield;
    }

    this.to_show_hide_combobox = function(bools)
    {
        var x = document.getElementsByTagName("SELECT");
        
        for (var k = 0; k < x.length; k++)
        {                                                    
            if (x[k].nodeName == 'SELECT')
            {
                  if (bools == false)
                  {
                    x[k].style.visibility = "hidden";                                    
                  }
                  else
                  {
                    x[k].style.visibility = "visible";
                  }
            }
        }
    }

    this.to_currency = function(amount)
    {
        if (amount == null || amount == '' || amount == undefined || amount == 'undefined')
        {
            return '';
        }

        amount = this.to_remove_comma(amount.toString());

        var a         = amount.split('.',2)
        var d         = a[1];
        var i         = parseInt(a[0]);
        var minus     = '';
        var delimiter = "," ;

        if(isNaN(i)) 
        { 
            if (isNaN(parseInt(d)) == false)
            {
                if (parseInt(d) > 0)
                {
                    return "0." + d.toString();
                }
                else
                {
                    return "0.00";
                }
            }
            else
            {
                return ''; 
            }
        }                       
      
        if(i < 0) 
        { 
            minus = '-'; 
        }

        i = Math.abs(i);

        var n = new String(i);
        var a = [];

        while(n.length > 3)
        {                                                
            var nn = n.substr(n.length-3);
            
            a.unshift(nn);                               
            n = n.substr(0,n.length-3);
        }
        
        if(n.length > 0) 
        {             
            a.unshift(n); 
        }
        
        n = a.join(delimiter);
        
        if(d == null || d == "undefined" || d.length < 1) 
        {                                 
            amount = n;                 
        }
        else 
        {             
            amount = n + '.' + d;                 
        }
                    
        if (d == null || d == "undefined" || d.length < 1)
        {
            return minus + amount + '.' + '00';
        }
                    
        return minus + amount;
    }
    
    this.to_clear_screen = function(tabid)
    {
        // chr avoid crash due to early call
        var tblrows;
        try
        {
            var tid = (tabid.constructor == String) ? document.getElementById(tabid) : tabid;
            tblrows = tid.rows;
        }
        catch (e) {}
        // chr   
                           
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
                        else if (childs[k].nodeName == 'SPAN'  ){childs[k].innerHTML = '';  }            
                        else if (childs[k].nodeName == 'LABEL') 
                        {
                            /* edited for red asterisks */                            
                            if ((childs[k].id != null || childs[k].id != '') && childs[k].innerHTML != '*')
                            {
                                childs[k].innerHTML = '';
                            }
                        }
                       else if (childs[k].nodeName == 'TABLE')
                       {
                           this.to_clear_screen(childs[k]); // go deeper! chr
                       }                        
                    }
                }
            }
        }        
    }
    
    
    this.to_unlock_button = function(tabid, buttons)
    {
        var tblrows = document.getElementById(tabid).rows;
        var button_array = buttons.split("~");
        
        if (tblrows == null || tblrows == "undefined" || button_array == null || button_array == "undefined")
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
                        if (childs[k].nodeName == 'INPUT' && childs[k].type == 'button')
                        {
                            childs[k].disabled = true;

                            for(var z = 0; z < button_array.length; z++)
                            {
                                if (button_array[z] == childs[k].id)
                                {
                                    childs[k].disabled = false;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    
    this.to_lock_button = function(tabid)
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
                        if (childs[k].nodeName == 'INPUT' && childs[k].type == 'button')
                        {
                            childs[k].disabled = true;
                        }
                    }
                }
            }
        }        
    }
    
    this.to_read_only = function(tabid, read_only)
    {
        // chr avoid crash due to early call
        var tblrows;
        try
        {
            var tid = (tabid.constructor == String) ? document.getElementById(tabid) : tabid;
            tblrows = tid.rows;
        }
        catch (e) {}
        // chr                      
        
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
                                childs[k].disabled = read_only;
                                childs[k].readOnly = read_only;
                            }
                            else if (childs[k].type == 'checkbox' | childs[k].type == 'button') 
                            {
                                childs[k].disabled = read_only;
                            }
                        }
                        else if (childs[k].nodeName == 'SELECT' | childs[k].nodeName == 'IMG' | (childs[k].nodeName == 'LABEL' && childs[k].innerHTML != '*'))
                        {
                            childs[k].disabled = read_only;
                        } 
                        else if (childs[k].nodeName == 'TABLE')
                        {
                            this.to_read_only(childs[k], read_only); // go deeper!
                        }               
                    }
                }
            }
        }        
    } 
    
    this.to_read_only_2 = function(tabid, read_only)
    {
        this.to_read_only(tabid, read_only);
        /*
        // chr avoid crash due to early call
        
        var tblrows;
        try
        {
            var tid = (tabid.constructor == String) ? document.getElementById(tabid) : tabid;
            tblrows = tid.rows;
        }
        catch (e) {}
        // chr                      
        
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
                       // edited for red asterisks 
                       if (childs[k].nodeName == 'INPUT' && childs[k].className != "button" && childs[k].type != "checkbox")
                       {
                            childs[k].disabled = false;
                            childs[k].readOnly = read_only;
                       }
                       else if ((childs[k].nodeName == 'INPUT' && childs[k].className == "button") || childs[k].nodeName == 'SELECT' || (childs[k].nodeName == 'LABEL' && childs[k].innerHTML != '*') || childs[k].nodeName == 'IMG' || (childs[k].nodeName == 'INPUT' && childs[k].type == "checkbox"))
                       {    
                            childs[k].disabled = read_only;
                       }
                       else if (childs[k].nodeName == 'TABLE')
                       {
                           this.to_read_only_2(childs[k], read_only); // go deeper!
                       }
                    }
                }
            }
        }
        */
    }     
    
           
    /* END OF JS COMMON METHOD. AUTHOR: DANIEL D. COLCOL III DATE MODIFIED: FEB 2010*/
    
    /*added by Liz*/
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
    
    this.is_empty = function(field_value)
    {   
        if (this.to_trim(field_value).length > 0) return false;
        else return true;
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
}


// **************************************************************************************** //
// *** Panel added by Ronald Tuazon 
// *** Date 
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
      var bdh = (dsh>dch)?dsh:dch;
      var bdw = (dsw>dcw)?dsw:dcw;
 
      this.waitifrm.style.height = this.divobj.style.height = bdh + 'px';
      this.waitifrm.style.width = this.divobj.style.width = bdw + 'px';
      this.waitifrm.style.display = this.divobj.style.display = "";
   }
   
   this.hide = function()
   {
      this.waitifrm.style.display = this.divobj.style.display = "none";
   }
}
    
    function error_msg(img,msg,sourceid,targetid,defaultmsgbox)
    {
        if (defaultmsgbox == 1)
        {
            alert(msg);
            if (!document.getElementById(sourceid).disabled)
            {
                document.getElementById(sourceid).focus();
            }
            return;
        }
        var pass = false;
        try
        {
            document.getElementById("diverrorbox").innerHTML = "";
            newdiv = document.getElementById("diverrorbox");
        }
        catch(e)
        {
            pass = true
            var newdiv = document.createElement("div");
            newdiv.setAttribute('id','diverrorbox');    
        }
        var shtml = "";     
        shtml += '<table cellspacing="0" cellpadding="0">';
        shtml += '<tr><td nowrap class="table_caption" colspan="2" >&nbsp;</td></tr>';
        if (img.length > 0 ) 
        {
        shtml += '<tr><td height="30px" nowrap class="cell_desc"><img src="'+ img +'" /></td> <td nowrap class="cell_desc" ><label>'+ msg  +'&nbsp;</label> </td></tr>';
        }
        else
        {
        shtml += '<tr><td height="30px" nowrap class="cell_desc" nowrap><img height="30px" onmousedown="disabletext()" src="../images/error.png" /></td> <td  id="td1" nowrap class="cell_desc"  >&nbsp;'+ msg  +'&nbsp; </td></tr>';
        }
        shtml += '<tr><td colspan="2" class="cell_desc" nowrap align="center" style="color: red;"  ><input type="button" value="OK" style="width:30px" onclick="javascript:close_errormsg()" /></td></tr>';
        shtml += '<tr><td colspan="2" class="cell_desc" nowrap align="center"></td></tr>';
        shtml += '</table>';
        newdiv.innerHTML = shtml;
        if (pass)
        {
            var  holderdiv = document.getElementById(targetid);
                holderdiv.appendChild(newdiv);
        }
        
        var div = document.getElementById("diverrorbox");    
            div.className = "ds_gray";
            div.style.left = "50%";
            div.style.top =  "20%";
            div.style.display = '';
            div.style.position = "absolute";   
            div.style.border = "solid 1px midnightblue ";         
            div.style.zIndex = 101;
            try
            {        
                var txtobj = document.getElementById(sourceid);
                if (txtobj.type == 'text')
                {
                    if (!txtobj.disabled)
                    {
                        txtobj.focus();                    
                    }
                }
            }
            catch(e) {}
        showPanel('diverrorbox');
    }
    function close_errormsg()
    {
     var div = document.getElementById("diverrorbox"); 
     div.style.display = "none";
     hidePanel('diverrorbox');
    }
    function disableSelection(target)
    {
    if (typeof target.onselectstart!="undefined") //IE route
	    target.onselectstart=function(){return false}
    else if (typeof target.style.MozUserSelect!="undefined") //Firefox route
	    target.style.MozUserSelect="none"
    else //All other route (ie: Opera)
	    target.onmousedown=function(){return false}
    target.style.cursor = "default"
    }
    function disable_text(id)
    {
        disableSelection(document.getElementById(id));
    }


