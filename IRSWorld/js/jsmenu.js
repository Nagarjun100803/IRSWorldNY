/*********************************************************
* IRS MENU implementation               CHR (c) PNB 2009 *
**********************************************************/
var jsmenu_a_home;
//var isDom = (isIE8 | isFF);

function jsmenu_show_popup(url)
{
    var center = window.frames[0];
    if (! center)
    {
        center = top.document.frames[0];
    }
    center.show_popup(url);
}

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

function jsmenu_item_over(aid)
{
    
    var aobj = document.getElementById(aid);
    var ptag = aobj.parentNode.tagName;
    var ss = aid.split('_');
    var divid = ss[0] + '_div_' + ss[2] + '_' + ss[3];
    var div = document.getElementById(divid);
    if (div)
    {
        div.style.display = '';
        var td = document.getElementById(aid).parentNode;
        var pos = calcpos(td);
        div.style.top = (pos.top + td.offsetHeight) + 'px';
        div.style.left = (ptag == 'td') ? (pos.left + 10) + 'px' : (pos.left + aobj.offsetWidth - 5) + 'px';
    }
}

function get_top_div(aobj)
{
    var atag = aobj;
    do
    {
        if (atag.parentNode.tagName == 'DIV') return atag.parentNode;
        atag = atag.parentNode;
    }
    while (atag.tagName != 'TD');
}

function jsmenu_div_enter(divid)
{
    var ss = divid.split('_');
    if (parseInt(ss[3]) > 0)
    {
        var acaller = document.getElementById(ss[0] + '_a_' + ss[2] + '_' + ss[3]);
        var pdiv = get_top_div(acaller);
        if (pdiv != null && pdiv.jsmenu_timerid != null)
        {
            jsmenu_clear_timeout(pdiv);
        }                
    }
    var div = document.getElementById(divid);
    if (div.jsmenu_timerid != null)
    {
        jsmenu_clear_timeout(div);
    }
}

function jsmenu_clear_timeout(obj)
{
    window.clearTimeout(obj.jsmenu_timerid);
    obj.jsmenu_timerid = null;
}

function jsmenu_set_timeout(obj)
{
    if (obj)
    {
        obj.jsmenu_timerid = window.setTimeout(
            function()
            {
                if (obj.jsmenu_timerid) 
                {
                    window.clearTimeout(obj.jsmenu_timerid);
                    obj.style.display = 'none';                            
                    obj.jsmenu_timerid = null;
                }
            },
            450); 
    }
}

function jsmenu_div_leave(divid)
{
    var div = document.getElementById(divid);
    if (div)
    {
        jsmenu_set_timeout(div);
    }
    var ss = div.id.split('_');
    var acaller = document.getElementById(ss[0] + '_a_' + ss[2] + '_' + ss[3]);
    var pdiv = get_top_div(acaller);
    if (pdiv != null) jsmenu_set_timeout(pdiv);
     
}

function jsmenu_item_out(aid)
{
    var ss = aid.split('_');
    var divid = ss[0] + '_div_' + ss[2] + '_' + ss[3];
    var div = document.getElementById(divid);
    if (div)
    {
        div.jsmenu_timerid = window.setTimeout(
            function()
            {
                if (div.jsmenu_timerid) 
                {
                    window.clearTimeout(div.jsmenu_timerid);
                    div.style.display = 'none';
                    div.jsmenu_timerid = null;
                }
            },
            450); 
    }
}

function jsmenu_div_click(adiv)
{
    adiv.style.display = 'none';
} 
           
function jsmenu_node()
{
    this.id = 0;
    this.pid = 0;
    this.title = '';
    this.url = '';
    this.target = '';
    this.home = false;
    this.child_menus = new Array();
}

jsmenu_node.prototype.addItem = function(item)
{
    this.child_menus.push(item);
}

function jsmenu(datatable, id, pid)
{
	this.id = id;
	this.pid = pid;
	this.child_menus = new Array();
	this.unlinked_items = new Array();
	this.found = null; 
	
    this.parent_ind = String.fromCharCode(187);
    this.pdir = '..';
    this.zidx = 2000;
	  		
	this.create_menu(datatable);
}

var jm = jsmenu.prototype;

jm.put_anchor = function(child_menu, cid)
{
    var shtm = '';
    var aid = this.id + '_a_' + child_menu.id + '_' + cid;
    
    shtm += '<a id=' + aid + ' ';
    if (child_menu.url.length > 0)
    {
        shtm += 'href=';
        var target = (child_menu.target.length > 0 && child_menu.target.charAt(0) == '_') ? child_menu.target : 'center_frame';
        if (target == '_popup')
        {
            shtm += '"javascript:jsmenu_show_popup(\'' + child_menu.url + '\');"';
        }
        else
        {
            if (child_menu.url.indexOf('http:') == 0)
            {
                shtm += '"' + child_menu.url + '" ';
            }
            else
            {
                shtm += '"' + this.pdir + child_menu.url + '" ';
            }
            shtm += 'target=' + target;
        }
    }
    if (child_menu.child_menus.length > 0)
    {
        shtm += ' onmouseover=\'jsmenu_item_over(this.id);\'';
        shtm += ' onmouseout=\'jsmenu_item_out(this.id);\'';    	        
    }
    shtm += '>' + child_menu.title;
    if (child_menu.child_menus.length > 0) shtm += ' ' + this.parent_ind;
    shtm += '</a>\n';
    if (child_menu.home) jsmenu_a_home = aid;
    return shtm;
}

jm.put_submenu = function(child_menu, cid, pid)
{
    var shtm = '';
    if (child_menu.length > 0)
    {
        var divid = this.id + '_div_' + cid + '_' + pid;
        var ulid = this.id + '_ul_' + cid + '_' + pid;
        shtm += '<div id=' + divid + ' ';
        shtm += 'onmouseover=\'jsmenu_div_enter(this.id);\' ';
        shtm += 'onmouseout=\'jsmenu_div_leave(this.id);\' ';
        shtm += 'onclick=\'jsmenu_div_click(this);\' ';
        shtm += 'style="display:none;position:absolute;z-index:' + this.zidx + '">\n';
        shtm += '<ul id=' + ulid + '>\n';
        for (var i = 0; i < child_menu.length; i++)
        {
            var lid = this.id + '_li_' + child_menu[i].id + '_' + cid;
            shtm += '<li id=' + lid + ' ';
            shtm += 'onmouseover=\'this.setAttribute((isDom) ? "class" : "className", "jsmenu_li_over");\' ';
            shtm += 'onmouseout=\'this.setAttribute((isDom) ? "class" : "className", "");\' ';
            shtm += '>\n';
            shtm += this.put_anchor(child_menu[i], cid);
            shtm += '</li>\n';
        }
        shtm += '</ul>\n</div>\n';
        this.zidx++;
        for (var i = 0; i < child_menu.length; i++)
        {
            if (child_menu[i].child_menus.length > 0)
            {
                shtm += this.put_submenu(child_menu[i].child_menus, child_menu[i].id, child_menu[i].pid);
            }
        }
    }
    
    return shtm;
}

jm.put_menu_string = function()
{
    var shtm = '';
    
    for (var i = 0; i < this.child_menus.length; i++)
    {
    
        if (this.child_menus[i].child_menus.length == 0 & this.child_menus[i].title.toLowerCase() != 'logout') continue;
    
        var tdid = this.id + '_td_' + this.child_menus[i].id;
        shtm += '<td id=' + tdid + ' align=center ';
        shtm += 'onmouseover=\'this.setAttribute((isDom) ? "class" : "className", "jsmenu_td_hilight");\' onmouseout=\'this.setAttribute((isDom) ? "class" : "className", "");\' ';
        shtm += '>\n';
        shtm += this.put_anchor(this.child_menus[i], this.child_menus[i].pid);
        if (this.child_menus[i].child_menus.length > 0)
        {
            this.zidx = 2000;
            shtm += this.put_submenu(this.child_menus[i].child_menus, this.child_menus[i].id, this.child_menus[i].pid);
        }
        
        shtm += '</td>\n';
    }
    return shtm;
}

jm.toString = function()
{
    var shtm = '<table id=' + this.id + ' class=jsmenu>\n';
    shtm += '<tr>\n';
    shtm += this.put_menu_string();
    shtm += '</tr>\n';
    shtm += '</table>';
    
    return shtm;
}

jm.findItem = function(id, arry)
{
    for (var i = 0; i < arry.length; i++)
    {
        if (this.found != null) return;
        
        if (arry[i].id == id) 
        {
            this.found = arry[i];
            return;
        }
        else if (arry[i].child_menus.length > 0)
        {
            this.findItem(id, arry[i].child_menus);
        }
    }

}

jm.addItem = function(item, lost)
{
    if (item.pid > 0)
    {    	        
        this.findItem(item.pid, this.child_menus);
        if (this.found != null) this.found.addItem(item);
        else
        {
            if (! lost) this.unlinked_items.push(item);
            else this.child_menus.push(item);
        }
        this.found = null;
        return;
    }
    
    this.child_menus.push(item);
}

jm.linkItems = function()
{
    for (var i = 0; i < this.unlinked_items.length; i++)
    {
        this.addItem(this.unlinked_items[i], true);
    }
}

jm.create_menu = function(dt)
{
    for (var i = 0; i < dt.Rows.length; i++)
    {
        var jn = new jsmenu_node();
        jn.id = dt.Rows[i]['SEQ_ID'];
        jn.pid = dt.Rows[i]['MenuParentID'];
        jn.title = dt.Rows[i]['MenuName'];
        jn.url = (dt.Rows[i]['ModuleUrl'] == null) ? '' : dt.Rows[i]['ModuleUrl'];
        jn.target = (dt.Rows[i]['ModuleTarget'] == null) ? '' : dt.Rows[i]['ModuleTarget'];
        jn.home = (dt.Rows[i]['AccessType'] == 3 & jn.url.length > 0);
        this.addItem(jn, false);
    }
    if (this.unlinked_items.length > 0) this.linkItems();
    if (this.pid != null)
    {
        var pobj = document.getElementById(this.pid);
        if (pobj)
        {
            pobj.innerHTML += this.toString();
        }
    }
    
    if (jsmenu_a_home != null && jsmenu_a_home.length > 0) 
    {
        document.getElementById(jsmenu_a_home).click();
    }
}

