/*********************************************************
* IRS SIDE BAR                          CHR (c) PNB 2009 *
**********************************************************/

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
	
        function js_sidebar_item_over(aid)
        {
            var ss = aid.split('_');
            var divid = ss[0] + '_div_' + ss[2] + '_' + ss[3];
            var div = document.getElementById(divid);
            if (div)
            {
                div.style.display = 'block';
                var td = document.getElementById(aid).parentElement;
                var pos = calcpos(td);
                div.style.top = pos.top + 'px';
                div.style.left = (pos.left + div.clientWidth) + 'px';
            }
        }

        function get_top_div(aobj)
        {
            var atag = aobj;
            do
            {
                if (atag.parentElement.tagName == 'DIV') return atag.parentElement;
                atag = atag.parentElement;
            }
            while (atag.tagName != 'TD');
        }

        function js_sidebar_div_enter(divid)
        {
            var ss = divid.split('_');
            if (parseInt(ss[3]) > 0)
            {
                var acaller = document.getElementById(ss[0] + '_a_' + ss[2] + '_' + ss[3]);
                var pdiv = get_top_div(acaller);
                if (pdiv)
                {
                    if (pdiv.js_sidebar_timerid != null)
                    {
                        js_sidebar_clear_timeout(pdiv);
                    }  
                }              
            }
            var div = document.getElementById(divid);
            if (div.js_sidebar_timerid != null)
            {
                js_sidebar_clear_timeout(div);
            }
        }

        function js_sidebar_clear_timeout(obj)
        {
            window.clearTimeout(obj.js_sidebar_timerid);
            obj.js_sidebar_timerid = null;
        }

        function js_sidebar_set_timeout(obj)
        {
            if (obj)
            {
                obj.js_sidebar_timerid = window.setTimeout(
                    function()
                    {
                        if (obj.js_sidebar_timerid) 
                        {
                            var td = obj.parentElement;
                            window.clearTimeout(obj.js_sidebar_timerid);
                            obj.style.display = 'none';                            
                            obj.js_sidebar_timerid = null;
                        }
                    },
                    500); 
            }
        }

        function js_sidebar_div_leave(divid)
        {
            var div = document.getElementById(divid);
            if (div)
            {
                js_sidebar_set_timeout(div);
            }
            var ss = div.id.split('_');
            var acaller = document.getElementById(ss[0] + '_a_' + ss[2] + '_' + ss[3]);
            var pdiv = get_top_div(acaller);
            if (pdiv != null) js_sidebar_set_timeout(pdiv);
             
        }

        function js_sidebar_item_out(aid)
        {
            var ss = aid.split('_');
            var divid = ss[0] + '_div_' + ss[2] + '_' + ss[3];
            var div = document.getElementById(divid);
            if (div)
            {
                div.js_sidebar_timerid = window.setTimeout(
                    function()
                    {
                        if (div.js_sidebar_timerid) 
                        {
                            window.clearTimeout(div.js_sidebar_timerid);
                            div.style.display = 'none';
                            div.js_sidebar_timerid = null;
                        }
                    },
                    500); 
            }
        }
        
        function js_sidebar_div_click(adiv)
        {
            adiv.style.display = 'none';
        }
                
        function js_sidebar_click(id, seqid, pid)
        {
            var lnk = document.getElementById(id +'_a_' + seqid + '_' + pid);
            if (lnk) 
            {
                lnk.onclick();
            }        
        }
        
        function js_sidebar_element(id, seqid, pid)
        {
            return document.getElementById(id +'_a_' + seqid + '_' + pid);
        }
        
        function js_sidebar_container(id, seqid, pid)
        {
            return document.getElementById(id +'_td_' + seqid + '_' + pid);
        }	
	
        function js_sidebar_item()
        {
            this.id = 0;
            this.pid = 0;
            this.title = '';
            this.url = '';
            this.menu_items = new Array();
        }
        
        js_sidebar_item.prototype.addItem = function(item)
        {
            this.menu_items.push(item);
        }
                    
	    function js_sidebar(id, menu_data, parent_id)
	    {
	        this.id = id;
	        this.pid = parent_id;
	        this.cellpadding = 0;
	        this.cellspacing = 5;
	        this.menu_items = new Array();
	        this.lost_items = new Array();
	        this.found = null;
	        if (menu_data != null)
	        {
	            this.parseItems(menu_data);
	        }
            this.zidx = 50;
            this.popup_background = "#f6f9fc";
            this.highlight = "Azure";
            this.normal = "transparent";
            this.use_href = false;	        
	    }
	    
	    var sb = js_sidebar.prototype;
	    
        sb.findItem = function(id, arry)
        {
            for (var i = 0; i < arry.length; i++)
            {
                if (this.found != null) return;
                
                if (arry[i].id == id) 
                {
                    this.found = arry[i];
                    return;
                }
                else if (arry[i].menu_items.length > 0)
                {
                    this.findItem(id, arry[i].menu_items);
                }
            }

        }

        sb.addItem = function(item, lost)
        {
            if (item.pid > 0)
            {    	        
                this.findItem(item.pid, this.menu_items);
                if (this.found != null) this.found.addItem(item);
                else
                {
                    if (! lost) this.lost_items.push(item);
                    else this.menu_items.push(item);
                }
                this.found = null;
                
                return;
            }
            
            this.menu_items.push(item);
        }
	    	    
	    sb.parseItems = function(item_array)
	    {
	        if (item_array.length > 0 && item_array[0].length > 3)
	        {
	            for (var i = 0; i < item_array.length; i++)
	            {
	                var item = new js_sidebar_item();
	                if (!isNaN(parseInt(item_array[i][0])))
	                {
	                    item.id = item_array[i][0];
	                }
	                else
	                {
	                    throw "ID must be numeric";
	                }
	                
	                if (!isNaN(parseInt(item_array[i][1])))
	                {
	                    item.pid = item_array[i][1];
	                }
	                else
	                {
	                    throw "Parent ID must be numeric";
	                }
	                
	                if (item_array[i][2] != null)
	                {
	                    item.title = item_array[i][2];
	                }
	                else
	                {
	                    throw "Emtpy String is allowed but not null.";
	                }  
	                
	                if (item_array[i][3] != null)
	                {
	                    item.url = item_array[i][3];
	                }
	                else
	                {
	                    throw "Emtpy String is allowed but not null.";
	                }
	                
	                this.addItem (item);	            
	            }
	        }
	        else throw "The array supplied is empty.";
	    }
	    
        sb.create_links = function(item, cid)
        {
            var shtm = '';
            
            shtm += '<a id=' + this.id + '_a_' + item.id + '_' + cid + ' ';
            if (item.url.length > 0)
            {
                shtm += (this.use_href) ? 'href=\'' : 'href=# onclick=\'';
                shtm += item.url + '\' ';
            }
            if (item.menu_items.length > 0)
            {
                shtm += ' onmouseover=\'js_sidebar_item_over(this.id);\'';
                shtm += ' onmouseout=\'js_sidebar_item_out(this.id);\'';    	        
            }
            shtm += '>' + item.title;
            shtm += '</a>\n';
            
            return shtm;
        }
        
        sb.put_subitems = function(menu_item, cid, pid)
        {
            var shtm = '';
            if (menu_item.length > 0)
            {
                var divid = this.id + '_div_' + cid + '_' + pid;
                var ulid = this.id + '_ul_' + cid + '_' + pid;
                shtm += '<div id=' + divid + ' ';
                shtm += 'onmouseenter=\'js_sidebar_div_enter(this.id);\' ';
                shtm += 'onmouseleave=\'js_sidebar_div_leave(this.id);\' ';
                shtm += 'onclick=\'js_sidebar_div_click(this);\' ';
                shtm += 'style="background-color:' + this.popup_background + '; border:solid 1px #5697c1;display:none;position:absolute;z-index:' + this.zidx + '">\n';
                shtm += '<ul id=' + ulid + '>\n';
                for (var i = 0; i < menu_item.length; i++)
                {
                    var lid = this.id + '_li_' + menu_item[i].id + '_' + cid;
                    shtm += '<li id=' + lid;
                    shtm += ' onmouseover=\'this.style.backgroundColor = "' + this.highlight + '";\' onmouseout=\'this.style.backgroundColor="' + this.normal + '";\'';
                    shtm += '>\n';
                    shtm += this.create_links(menu_item[i], cid);
                    shtm += '</li>\n';
                }
                shtm += '</ul>\n</div>\n';
                this.zidx++;
                for (var i = 0; i < menu_item.length; i++)
                {
                    if (menu_item[i].menu_items.length > 0)
                    {
                        shtm += this.put_subitems(menu_item[i].menu_items, menu_item[i].id, menu_item[i].pid);
                    }
                }
            }
            
            return shtm;
        }
	    
	    sb.toString = function()
	    {
	        if (this.menu_items.length > 0)
	        {
                var shtm = '<table id=' + this.id + ' cellspacing=' + this.cellspacing + ' cellpadding=' + this.cellpadding + ' class=js_sidebar>\n';
                for (var i = 0; i < this.menu_items.length; i++)
                {
                    shtm += '<tr>\n<td id=' + this.id + '_td_' + this.menu_items[i].id + '_' + this.menu_items[i].pid;
                    shtm += ' onmouseover=\'this.style.backgroundColor = "' + this.highlight + '";\' onmouseout=\'this.style.backgroundColor="' + this.normal + '";\' ' + '>';
                    shtm += this.create_links(this.menu_items[i], this.menu_items[i].pid);
                    if (this.menu_items[i].menu_items.length > 0)
                    {
                        this.zidx = 50;
                        shtm += this.put_subitems(this.menu_items[i].menu_items, this.menu_items[i].id, this.menu_items[i].pid)
                    }
                    shtm += '</td>\n</tr>\n';
                }
                shtm += '</table>\n';

                return shtm;
            }
            
            return "No menu items defined.";
        }
	    
	    sb.create_sidebar = function()
	    {	        
            if (this.pid != null)
            {
                var pobj = document.getElementById(this.pid);
                if (pobj)
                {
                    pobj.innerHTML += this.toString();
                }
            }
	    }