
/******************* 
  CHR (c) PNB 2009
********************/

var isIE = (navigator.userAgent.indexOf("MSIE") > -1 && get_ie_version() < 9);
var isIE6 = (navigator.userAgent.indexOf("MSIE 6.") > -1);
var isIE7 = (navigator.userAgent.indexOf("MSIE 7.") > -1);
var isIE8 = (navigator.userAgent.indexOf("MSIE 8.") > -1);
var isIE9 = (navigator.userAgent.indexOf("MSIE 9.") > -1);
var isIE10 = (navigator.userAgent.indexOf("MSIE 10.") > -1);
var isFF = (navigator.userAgent.indexOf("Firefox") > -1);
var isChrome = (navigator.userAgent.indexOf("Chrome") > -1);
var isSafari =(navigator.userAgent.indexOf("Safari") > -1);
var isDom = ((navigator.userAgent.indexOf("MSIE") > -1 && get_ie_version() > 8) || isFF || isChrome || isSafari);
var isCompatible = (isIE7 && navigator.userAgent.indexOf("compatible") > -1);

function get_ie_version()
{
    var ie_ver = -1.0;
    if (isIE)
    {
        var ie_start = navigator.userAgent.indexOf("MSIE") + 5;
        var ie_len = navigator.userAgent.substring(ie_start).indexOf(";");
        ie_ver = parseFloat(navigator.userAgent.substr(ie_start, ie_len + 1));
    }
    
    return ie_ver;
}

function fire_event(ctl, evtname)
{
    if (isIE) 
    {
        if (evtname == "onclick")  ctl.click();
        else
        {
            ctl.fireEvent(evtname);  
        }
    }
    else
    {
        var evt;
        var sevt = (evtname.substr(0, 2) == 'on') ? evtname.substr(2) : evtname;
        if (sevt == "click")
        {
            evt = document.createEvent("MouseEvents");
        }
        else
        {   
            evt = document.createEvent('HTMLEvents');
        }
        evt.initEvent(sevt, false, false);
        ctl.dispatchEvent(evt);	
    }   
}

function setText(elem, txt)
{
    if (elem == null)
    {
        alert('setText with null element');
        return;
    }
    
    var l = (elem.constructor == String) ? document.getElementById(elem) : elem;
    if (l.nodeName == 'INPUT' && (l.type == 'text' | l.type == 'button')) l.value = txt;
    else
    {
        try
        {
            if (l.textContent) l.textContent = txt;
            else if (l.innerText) l.innerText = txt;
            else if (l.text) l.text = txt;
            else
            {
                l.innerHTML = txt;
            }
        }
        catch (e)
        { 
            alert(e.Description);
        }
    }
}

function getText(elem)
{
    var l = (elem.constructor == String) ? document.getElementById(elem) : elem;
    if (l.nodeName == 'INPUT' && (l.type == 'text'  | l.type == 'button')) return l.value;
    else
    {
        if (l.textContent) return l.textContent;
        return l.innerText;
    }
}

function dom(tid)
{
    return document.getElementById(tid);
}

function show_popup(url)
{
    show_frame_dialog(url, '', '100%', '100%', 0); 
}
        
function spawn_errpage(errmsg)
{
    var errurl = '';
    var vd = window.location.href.split('/')[3];
    if (window.location.port == null || window.location.port.length == 0) errurl += "/" + vd;
    //errurl += '/ErrorPage.aspx?ErrorMessage=' + errmsg;
    errurl += '/ErrorPage.aspx';
    sessionStorage.setItem("ErrorMessage", errmsg);
    //varer = sessionStorage.getItem("ErrorMessage");
    window.open(errurl, '_top');
}

function isStandAlone()
{
    return (top.window == self);
}

function hasQueryStrings(loc)
{
    var xloc = (loc == null) ? window : loc;
    return (xloc.location.search.length > 0);
}

function getQueryStrings(loc)
{
    var xloc = (loc == null) ? window : loc;

    if (hasQueryStrings(xloc))
    {
        var qs_array = xloc.location.search.substring(1).split("&");
        var out_array = new Array(qs_array.length);
        for (var i = 0; i < qs_array.length; i++)
        {
            out_array[i] = qs_array[i].split("=");
        }
        return out_array;
    }
    return null;
}

function getQueryString(loc, qs_key) 
{
    try
    {
        var xloc = (loc == null) ? window : loc;
        var qstring = xloc.location.search.substring(1);
        var qs_array = qstring.split("&");
        for (var i=0; i< qs_array.length; i++) 
        {
            var parms = qs_array[i].split("=");
            if (parms[0] == qs_key) 
            {
                return parms[1];
            }
        }
    }
    catch (e)
    {}
    
    return null;
}

function enable_button(btn, enable)
{
    if (enable)
    {
        if (isIE) 
        {
            btn.className = 'button';
        }
        else
        {
            btn.setAttribute('class', 'button');
        }
    }
    else
    {
        if (isIE)
        {
            btn.className = 'disabled_button';
        }
        else
        {
            btn.setAttribute('class', 'disabled_button');
        }
    }

    btn.disabled = !enable;
}

function set_read_only(tabid, read_only)
{
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
                        if (childs[k].type == 'text') childs[k].readOnly = read_only;
                        else if (childs[k].type == 'checkbox' | childs[k].type == 'button') 
                        {
                            childs[k].disabled = read_only;
                        }
                    }
                    else if (childs[k].nodeName == 'SELECT' | childs[k].nodeName == 'img' | (childs[k].nodeName == 'LABEL' && childs[k].innerHTML != '*'))
                    {
                        childs[k].disabled = read_only;
                    } 
                    else if (childs[k].nodeName == 'TABLE')
                    {
                        set_read_only(childs[k], read_only); // go deeper!
                    }               
                }
            }
        }

    }
}

function clear_fields(tabid)
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
                        if (childs[k].type == 'text') childs[k].value = '';
                        else if (childs[k].type == 'checkbox') childs[k].checked = false;
                        else if (childs[k].type == 'file')
                        {
                            var file_upload  = childs[k];
                            var file_upload2 = file_upload.cloneNode(false);
                            file_upload2.onchange = file_upload.onchange;
                            file_upload.parentNode.replaceChild(file_upload2,file_upload); 
                        }
                    }
                    else if (childs[k].nodeName == 'SELECT')
                    {
                        childs[k].selectedIndex = 0;
                    }
                    else if (childs[k].nodeName == 'SPAN') 
                    {
                        setText(childs[k], '');
                    }
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
                        clear_fields(childs[k]);
                    }
                }
            }
        }

    }
}

function is_closing(evt)
{
    if (!evt) evt = window.event;
    if (evt == null) 
    {
        return false;
    }
    var iever = get_ie_version();

    if (iever > 8 || isCompatible)
    {
        return false;
    }
    
    return (evt.clientX < 0 || evt.clientY < 0);
}

function ajax_status_click()
{
    if (confirm('Close wait icon?'))
    {
        ajax_show_status(false);
        document.forms[0].onmousemove = null;
    }
}

function ajax_status_move(evt)
{
    if (!evt) evt = window.event;
    var adiv = document.getElementById('ajax_div');
    try
    {
        adiv.style.top = evt.clientY + 'px';
        adiv.style.left = evt.clientX + 'px';
    }
    catch (e) {}
}

function ajax_show_status(on)
{
    if (on)
    {
        if (document.form1.ajax_timerid != null) return;
        
        document.form1.ajax_timerid = window.setTimeout
        (
            function()
            {
                if (document.getElementById('ajax_div') == null)
                {
                    var adiv = document.createElement('div');
                    adiv.id = 'ajax_div';
                    adiv.style.position = 'absolute';
                    adiv.style.zIndex = 3000;
                    adiv.style.top = ((screen.height / 2) - 16) + 'px';
                    adiv.style.left = ((screen.width / 2) - 16) + 'px';
                    adiv.style.backgrounColor = 'red';
                    adiv.style.width = '32px';
                    adiv.style.height = '32px';
                    var img = document.createElement('img');
                    img.id = 'ajax_img';
                    img.style.alignment = 'absmiddle';
                    img.src = '../images/ajaxloader.gif';
                    adiv.appendChild(img);
                    document.forms[0].appendChild(adiv);
                    adiv.style.display = '';
                    img.onclick = ajax_status_click;
                    document.forms[0].onmousemove = ajax_status_move;
                }
                else
                {
                    document.getElemenById('ajax_div').style.display = '';
                }
            }, 100);
    }
    else
    {
        if (document.form1.ajax_timerid)
        {
            window.clearTimeout(document.form1.ajax_timerid);
            document.form1.ajax_timerid = null;
        }
        var adiv = document.getElementById('ajax_div');
        if (adiv != null) 
        {
            adiv.style.display = 'none';
            document.form1.removeChild(adiv);
        }
    }
}

function show_frame_dialog(src_url, close_script, w_unit, h_unit, padtop, xtop, xleft, show_close_button)
{
    var c_js;
    var d_url;
    
    /*
    if (document.getElementById('_xdiv'))
    {
        alert("A dialog is already active");
        return;
    }
    */
    
    if (close_script != null && close_script.length > 0)
    {
        c_js = (src_url.indexOf('?') > -1) ? '&' : '?';
        d_url = src_url + c_js + 'js_closer=' + close_script;
    }
    else
    {
        d_url = src_url;
    }
        
    var ddiv = document.createElement('div');
    ddiv.id = '_xdiv';
    ddiv.className = 'div_undock';
    var xw = w_unit;
    var xh = h_unit;
    if (xw == '100%')
    {
        xw = (top) ? top.document.getElementById('divCenter').offsetWidth + 'px' : (parent.top) ? parent.top.document.getElementById('divCenter').offsetWidth + 'px' : w_unit;
    }
    if (xh == '100%')
    {
        xh = (top) ? top.document.getElementById('divCenter').offsetHeight + 'px' : (parent.top) ? parent.top.document.getElementById('divCenter').offsetHeight + 'px' : h_unit;
    }    
    ddiv.style.width = xw;
    ddiv.style.height = xh;
    ddiv.style.left = ((xleft == undefined) ? 0 : xleft) + 'px';
    ddiv.style.top = ((xtop == undefined) ? 0 : xtop) + 'px';
    ddiv.style.position = 'absolute';
    var xframe = document.createElement("iframe");
    xframe.id = '_xframe';
    xframe.backgroundColor = "Transparent";
    xframe.zIndex='100';
    xframe.frameBorder = 0;
    xframe.allowTransparency = true;
    xframe.style.width = ddiv.style.width;
    xframe.style.height = ddiv.style.height;
    ddiv.appendChild(xframe);
    if (padtop != null)
        ddiv.style.paddingTop = padtop + 'px';
    xframe.src = d_url;
    document.forms[0].appendChild(ddiv);
    ddiv.style.display = "";
    ddiv.style.backgroundColor = "Transparent";
    
    if (show_close_button && parent.top != null)
    {
        var btndiv = parent.top.document.getElementById('div_frame_closer');
        var divcent = parent.top.document.getElementById('divCenter');
        if (btndiv)
        {
            btndiv.style.display = '';
            btndiv.style.top = (divcent.offsetTop - 21) + 'px';
            btndiv.style.left = (divcent.offsetWidth - 66) + 'px';
        }
    }
}

function close_frame_dialog(showerror)
{
    var js = getQueryString(window, 'js_closer');
    if (js != undefined)
    {
        parent.returnValue = window.returnValue;
        eval('parent.' + js +'();');
    }    
    
    if (parent.top != null)
    {
        var btndiv = parent.top.document.getElementById('div_frame_closer');
        if (btndiv)
        {
            btndiv.style.display = 'none';
        }
    } 
      
    var found = 0; 
    var ddiv = null;
    if (parent.document.getElementById('_xdiv'))
    {
        ddiv = parent.document.getElementById('_xdiv');
        found++;
    }
    else if (document.getElementById('_xdiv'))
    {
        var ddiv = document.getElementById('_xdiv');
        found++;
    }

    if (ddiv)
    {
        ddiv.style.display='none';
        try
        {
            //ddiv.removeNode(true);
            ddiv.removeChild(ddiv.childNodes[0]);
            ddiv.parentNode.removeChild(ddiv);
        }
        catch (e) {}
    }    
    
    if (found < 1)
    {
        if (showerror)
        {
            alert("Unable to find the component to close");
        }
    }
    
    if (document.js_closer)
    {
        eval(document.js_closer + '();');        
    }    
}

function close_msgboard()
{
    var mb = document.getElementById('_msg_board');
    if (mb == undefined) return;
    
    mb.style.display = 'none';
}

function show_message(err_type, err_text, undock)
{
    var msg_board = document.getElementById('_msg_board');
    if (msg_board == undefined) return;
    var msg_ind = document.getElementById('_msg_indicator');
    var msg_txt = document.getElementById('_msg_text');

    if (msg_ind != undefined)
    {
        if (err_type == 'E') 
        {
            setText(msg_ind, "X");
            msg_ind.style.color = "coral";
            msg_txt.style.color = "firebrick";
        }
        else if (err_type == 'Q') 
        {
            setText(msg_ind, "?"); 
            msg_ind.style.color = "peachpuff";
            msg_txt.style.color = "orange";
        }
        else
        {
            setText(msg_ind, "!"); 
            msg_ind.style.color = "lightcyan";
            msg_txt.color = "steelblue";
        }
    }
    if (msg_txt != undefined) setText(msg_txt, err_text); 
    
    msg_board.style.display = '';
    if (undock)
    {
        msg_board.className = "div_undock";
        if (!isIE) msg_board.style.height = screen.Height - 136;
        document.getElementById("_msg_tab").className = "tab_undock";
    }
    else
    {
        msg_board.className = "div_docked";
        document.getElementById("_msg_tab").className = "tab_docked";
    }
}

function rtrim(str)
{
    if (str == undefined) return "";
    return str.replace(/(\s*$)/g, "");
}

function ltrim(str)
{
    if (str == undefined) return "";
    return str.replace(/(^\s*)/g, "");
}

function trim(str)
{
    if (str == undefined) return "";
    return str.replace(/(^\s*)|(\s*$)/g, "");
}

// function to create options in select
function create_option(ddl, code, desc)
{
    var opt_new = document.createElement('option');
    ddl.options.add(opt_new);
    opt_new.value = code;
    setText(opt_new, desc); 
}

function set_ddl_options(ddl, opt)
{
    if (opt.length > 0)
    {
        if (ddl) ddl.options.length = 0; else return;
        
        create_option(ddl, '0', 'Select');
        
        for (var i = 0; i < opt.length; i++)
        {
            var ss = opt[i].split('~');
            create_option(ddl, ss[0], ss[1]);
        }
    }        
}

function setSelectedItem(ddl, text)
{
    for (var i = 0; i < ddl.options.length; i++)
    {
        var opts = ddl.options;
        var opt_txt = opts.item(i).text;
        if (text == opt_txt)
        {
            ddl.selectedIndex = i;
            return true;
        }
    }
    
    return false;
}

function ddlContainsOption(ddl, value)
{
    if (ddl.options.length > 0)
    {
        for (var i = 0; i < ddl.options.length; i++)
        {
            if (ddl.options[i].value == value) return i;
        }
    }
    
    return -1;
}
/* date routines */

function dateToString(dt)
{
    if (dt == null) return '';
    if (dt.constructor == Date)
    {
        var mo = "" + (dt.getMonth() + 1);
        var dd = "" + dt.getDate();
        var hh = dt.getHours();
        var mm = "" + dt.getMinutes();
        var ss = "" + dt.getSeconds();
        var tt = (hh > 12 | (hh == 12 & mm > 0)) ? "PM" : "AM";
        var shh = "" + ((hh > 12) ? hh - 12 : hh);
        return ((mo.length == 1) ? "0" + mo : "" + mo) + "/" + ((dd.length == 1) ? "0" + dd : "" + dd) + "/" + dt.getFullYear() + " " + ((shh.length == 1) ? "0" + shh : "" + shh) + ":" + ((mm.length == 1) ? "0" + mm : mm) + ":" + ((ss.length == 1) ? "0" + ss : ss) + " " + tt;
    }
    else if (dt.constructor  == String)
    {
        return dt;
    }
}

function load_ddl(tid, dobj, val, desc, ival, itype, icode, idesc)
{
    var ddl = (tid.constructor == String) ? document.getElementById(tid) : tid;
    if (ddl) ddl.options.length = 0; else return;
    var dt = (dobj.__type.indexOf("DataSet") > -1) ? dobj.Tables[0] : dobj; 
    if (dt.Rows.length > 0)
    {
        if (icode == null && idesc == null) create_option(ddl, null, '');
        else create_option(ddl, icode, idesc);
        
        for (var i = 0; i < dt.Rows.length; i++)
        {
            create_option(ddl, dt.Rows[i][val], dt.Rows[i][desc]);
        }
        if (ival != null)
        {  
            if (itype == null | itype == 'v') ddl.value = ival; 
            else 
            {
                ddl.selectedIndex = (ival == null) ? 0 : ival;
            } 
        }        
    }
}

function get_current_date_string()
{
    var dt = new Date();
    return dateToString(dt);
}

function stringToDate(sdt)
{
    try
    {
        var ms = Date.parse(sdt);
        return new Date(ms);
    }
    catch (e)
    {}
    
    return null;
}

function isValidDate(dateStr) 
{
    var retval = new Array();
    // This function returns array [bool,string] 
    // Checks for the following valid date formats:
    // MM/DD/YY   MM/DD/YYYY   MM-DD-YY   MM-DD-YYYY
    // Also separates date into month, day, and year variables
    var datePat = /^(\d{1,2})(\/|-)(\d{1,2})\2(\d{2}|\d{4})$/;
    // To require a 4 digit year entry, use this line instead:
    // var datePat = /^(\d{1,2})(\/|-)(\d{1,2})\2(\d{4})$/;
    var matchArray = dateStr.match(datePat); // is the format ok?
    if (matchArray == null) 
    {
        retval[0] = false;
        retval[1] ="Date is not in a valid format.";
        return retval;
    }
    month = matchArray[1]; // parse date into variables
    day = matchArray[3];
    year = matchArray[4];
    if (month < 1 || month > 12) 
    { 
        retval[1] = "Month must be between 1 and 12.";
        retval[0] = false;
        return retval;
    }
    if (day < 1 || day > 31) 
    {
        retval[1] = "Day must be between 1 and 31.";
        retval[0] = false;
        return retval;
    }
    if ((month==4 || month==6 || month==9 || month==11) && day==31) 
    {
        retval[1] = "Month "+month+" doesn't have 31 days!";
        retval[0] = false;
        return retval;
    }
    if (month == 2) 
    { // check for february 29th
        var isleap = (year % 4 == 0 && (year % 100 != 0 || year % 400 == 0));
        if (day>29 || (day==29 && !isleap)) 
        {
	        retval[1] = "February " + year + " doesn't have " + day + " days!";
	        retval[0] = false;
	        return retval;
        }
    }
    retval[0] = true;  // date is valid
    retval[1] = ((month.length == 1) ? "0" + month : month) + "/" + ((day.length == 1) ? "0" + day : day) + "/" + year;
    return retval;
}

show_a_popup = function (url)
{
    var center = document.frames[0];
    if (! center)
    {
        center = top.document.frames[0];
    }
    center.show_popup(url);
}

// calculate position of object from body tag
function calcpos(obj) 
{
    var xy =
    {
        left: 0,
        top: 0
    }

    var tag = obj
    do {
        xy.left += tag.offsetLeft;
        xy.top += tag.offsetTop;
        tag = tag.offsetParent;
        if (tag == null) break;
    } while(tag.tagName!= 'BODY');
    
    return xy;
}

function formatNumber(num, dec)
{
    var numx = (num == null) ? 0 : num;
    if (numx.toFixed)
        return numx.toFixed(dec);
    return numx;
}

function array_copy(aray_in)
{
    var aray_out = new Array();
    for (var i = 0; i < aray_in.length; i++)
    {
        aray_out[i] = aray_in[i];
    }
    
    return aray_out;
}

function isNumeric(s)
{
    var red = /[^\d,\.{\-|\+}]/g;
    rs = s.match(red);
    return (rs == null);
}

function isSpecialEx(s)
{
    var pat = /[`~!@#$%^&*\(\)-+|\\\{\}\[\]:;"'<>?,.\/]/g;
    rs = s.match(pat);
    return (rs != null);
}

function isSpecial(sText)
{
    var s = sText;
    var invalid_chars = "!@#$%^&*()+/";

    for (var i = 0; i < s.length; i++) 
    { 
        var c = s.charAt(i); 
        if (invalid_chars.indexOf(c) > -1) 
        {
            return true;
        }
    }
    return false;
}

function KBIAlgo(sno)
{
    var rc =
    {
        valid: true,
        message: ''
    };
    
    try
    {
        var xno = trim(sno);
        var w = [4, 3, 2, 7, 6, 5, 4, 3, 2, 1];
        if (xno.length != w.length)
        {
            rc.message = "Account no. length is not " + w.length;
            rc.valid = false;
        }
        else
        {
            var sumprod = 0;
            
            for (var i = 0; i < w.length; i++)
            {
                var s = xno.substr(i, 1);
                if (isNaN(parseInt(s)))
                {
                    rc.message = "Account no. position " + (i + 1) + " is not numeric";
                    rc.valid = false;
                    break;
                }
                else
                {
                    sumprod += parseInt(s) * w[i];
                }
            }

            if (rc.valid)
            {
                rc.valid = ((sumprod % 11) == 0);
                rc.message = (rc.valid) ? "" : "Account no. is not KBI format";
            }           
        }
    }
    catch (e)
    {
        rc.message = e.Description;
        rc.valid = false;
    }

    return rc;
}

function FCAlgo(sno)
{
    var rc =
    {
        valid: true,
        message: ''
    };
    
    try
    {
        var xno = trim(sno);
        if (xno.length > 12 && xno.substring(0, 4) == "0000")
        {
            xno = xno.substring(4);
        }
        
        if (isNumeric(xno))
        {
            var weights_var = [ 4, 3, 2, 1, 2, 3, 4, 5, 6, 7, 8 ];
            var sum_of_product = 0;
            var check_digit = 0;
            
            if (xno.substr(0, 2) != "00")
            {
                for (var i = 0; i < weights_var.length; i++)
                {                        
                    sum_of_product += parseInt(xno.substr(i, 1)) * weights_var[i];
                }
                check_digit = sum_of_product % 10;
                rc.valid = (parseInt(xno.substring(11)) == check_digit);
            }
            else 
            {
                rc.valid = false;
            }
            
            if (!rc.valid)
            {
                rc.message = "Account no. is not FC format";
            }         
        }
        else
        {
            rc.valid = false;
            rc.message = "Account no. is not entirely numeric";
        }
    }
    catch (e)
    {
        rc.message = e.Description;
        rc.valid = false;
    }

    return rc;
}

function check_account(acctno)
{
    var ac =
    {
        valid: false,
        type: '',
        message: ''
    };
    
    var rc = FCAlgo(acctno);
    if (!rc.valid)
    {
        rc = KBIAlgo(acctno);
        if (rc.valid)
        {
            ac.type = 'KBI';
        }
        else
        {
            ac.type = 'INVALID';
        }
    }
    else
    {
        ac.type = 'FC';
    }

    ac.valid = rc.valid;
    ac.message = rc.message;
    
    return ac;
}

//lifted from jscommonmethods to call method withoud instantiation
function show_loading_message(htmldivs)
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

if (isIE)//By RDG 05/27/2013
{
    document.onmousedown=disableclick;
}
else
{
    document.captureEvents(Event.MOUSEDOWN);
    document.onmousedown=disableclick;
}
function disableclick(e)//By RDG 05/27/2013
{
    if (isIE)
    {
	    if(event.button==2)
	    {
		    //alert(status);
		    alert("Sorry, you are not allowed to right click!");
		    return false;	
	    }
	}
	else
	{
	    if(document.layers||(document.getElementById&&!document.all))
        {
            if (e.which==2||e.which==3) 
            {
                alert("Sorry, you are not allowed to right click!");
                return false;
            }
        }
	}
}

function checkKeyCode(e)//By RDG 05/27/2013
{
	var key, node;
	if (e)
    {
	    key = e.which;
	    node = e.target;
    }
	else
    {
	    key = event.keyCode;
	    node = event.srcElement;
    }
	//F5 and Back button
	//if(key == 8 || key == 116)
	//F5; BACKSPACE; CTRL+R ; CTRL+N
	if(key == 116 || key == 123 ||(event.ctrlKey && (event.keyCode == 78 || event.keyCode == 82)))
    {
	    if (e)
        {
		        e.which = 0;
		        e.returnValue = false;
        }
	    else
        {
	        event.keyCode=0;
	        event.returnValue = false;
        }
	    return false
    }
	return true;
}
document.onkeydown = checkKeyCode;//By RDG 05/27/2013