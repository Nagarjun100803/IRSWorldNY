
/* By: CHR */
        function treetab_get_parent(obj)
        {
            var pobj = obj;
            while (pobj.parentNode.tagName != 'TABLE')
            {
                if (pobj.tagName == 'BODY') 
                {
                    pobj == null; 
                    break;
                }
                else
                {
                    pobj = pobj.parentNode;
                }
            } 
            
            if (pobj != null) pobj = pobj.parentNode;
            
            return pobj;       
        }
        
        function treetab_get_topmost(obj)
        {
            var pid = obj.id.split("_")[0];
            return document.getElementById(pid);
        }
        
        function treetab_is_valid_home(sel)
        {
            if (sel.options[sel.selectedIndex].text.toUpperCase() != 'HOME')
            {
                return 0;
            }
            var tt = treetab_get_topmost(sel);
            var nhome = -1
            if (tt != null)
            {
                var sels = tt.getElementsByTagName('select');
                if (sels.length > 0)
                {
                    for (var i = 0; i < sels.length; i++)
                    {
                        var dd = sels.item(i);
                        if (dd.options[dd.selectedIndex].text.toUpperCase() == 'HOME')
                        {
                            nhome++;
                        }
                    }
                }
            }
            
            return nhome;
        }
    
        function treetab_access_change(did, sel, nid, npid, ntitle, fun)
        {
            if (treetab_is_valid_home(sel) > 0)
            {
                alert("Home can be set only once.");
            }
            var obj = treetab_get_parent(sel);
            if (obj != null)
            {
                var pddl = null;
                var not_allowed = false;
                var pobj = treetab_get_parent(obj.parentNode);
                if (pobj != null && pobj.id != did)
                {
                    pddl = pobj.getElementsByTagName('select');
                    pddl = pddl.item(0);
                    not_allowed = (pddl.value == 0 & sel.value > 0);
                    if (pobj.id.indexOf('_tbl_') < 0)
                    {
                        not_allowed = false;
                    }
                }
                var ddls = obj.getElementsByTagName('select');
                for (var i = 0; i < ddls.length; i++)
                {
                    ddls.item(i).value = (not_allowed) ? pddl.value : sel.value;
                    if (! not_allowed)
                    {
                        var txtid = '' + ddls.item(i).id;
                        txtid = txtid.replace("_dd_", "_txt_");
                        var txt = document.getElementById(txtid);
                        if (txt)
                        {
                            txt.value = dateToString(new Date());
                        }
                    }
                }
            }
            
            if (fun != null && fun.length > 0)
            {
                eval( fun + '(' + sel.value + ', ' + nid + ', ' + npid + ', \'' + ntitle + '\');');
            }
        }    
            
        function treetab_node()
        {
            this.id = 0;
            this.pid = 0;
            this.gid = 0;
            this.title = '';
            this.access = 0;
            this.modid;
            this.effdate;
            this.onchange = 'access_change';
            this.child_nodes = new Array();
        }
        
        treetab_node.prototype.addItem = function(item)
        {
            this.child_nodes.push(item);
        }
        
    	function treetab(datatable, id, pid)
    	{
    		this.id = id;
    		this.pid = pid;
    		this.child_nodes = new Array();
    		this.unlinked_items = new Array();
    		this.found = null; 
    		this.onchange = '';
    		this.acopt = null;
    	}

    	var tt = treetab.prototype; 
    	
    	tt.findItem = function(id, arry)
    	{
    	    for (var i = 0; i < arry.length; i++)
    	    {
    	        if (this.found != null) return;
    	        
    	        if (arry[i].id == id) 
    	        {
    	            this.found = arry[i];
    	            return;
    	        }
    	        else if (arry[i].child_nodes.length > 0)
    	        {
    	            this.findItem(id, arry[i].child_nodes);
    	        }
    	    }
    	}
    	    	
    	tt.addItem = function(item, lost)
    	{
    	    if (item.pid > 0)
    	    {    	        
    	        this.findItem(item.pid, this.child_nodes);
    	        if (this.found != null) this.found.addItem(item);
    	        else
    	        {
    	            if (! lost) this.unlinked_items.push(item);
    	            else this.child_nodes.push(item);
    	        }
    	        this.found = null;
    	        return;
    	    }
    	    
    	    this.child_nodes.push(item);
    	}    	
    	
    	tt.linkItems = function()
    	{
    	    for (var i = 0; i < this.unlinked_items.length; i++)
    	    {
    	        this.addItem(this.unlinked_items[i], true);
    	    }
    	} 
    	
    	tt.find_option = function(sopt, na)
    	{
    	    var i = 0;
    	    for (; i < sopt.length; i++)
    	    {
    	        var st = sopt[i].indexOf('value=');
    	        if (st > -1)
    	        {
    	            val = sopt[i].substr(st + 'value='.length, 1);
    	            var n = parseInt(val);
    	            if (! isNaN(n) & n == na) return parseInt(i);
    	        }
    	    }
    	    
    	    return -1;
    	}
    	
    	tt.filter_opt = function(optarr, modid)
    	{
            var aray_out = new Array();
            var j = 0;
            for (var i = 0; i < optarr.length; i++)
            {
                if (modid == null & optarr[i].indexOf('>Home</option>') > -1) continue;
                aray_out[j] = optarr[i];
                j++;
            }
            
            return aray_out;    	
        }
    	
    	tt.put_node = function(node)
    	{
	        var tdid = this.id + '_td_' + node.id + '_' + node.pid + '_' + node.gid;
	        var selid = this.id + '_dd_' + node.id + '_' + node.pid + '_' + node.gid;
	        var txtid = this.id + '_txt_' + node.id + '_' + node.pid + '_' + node.gid;
	        var rspan = node.child_nodes.length + 1; 
	        var shtm = '<td id=' + tdid + '_title align=left style="width:200px; vertical-align:top;" ';
	        if (rspan > 0)
	        {
	            shtm += 'RowSpan=' + rspan;
	        }
	        shtm += '>';
	        shtm += node.title;
	        shtm += '</td>\n';
	        shtm += '<td id=' + tdid + '_access style="vertical-align:top;" align=right colspan=2">';
	        shtm += '<select id=' + selid + ' disabled style="width: 150px;"';
	        if (node.onchange != null && node.onchange.length > 0)
	        {
	            shtm += ' onchange="treetab_' + node.onchange + '(\'' + this.id + '\', this, ' + node.id + ', ' + node.pid + ', \'' + node.title + '\', \'' + this.onchange + '\');" ';
	        }
	        shtm += '>\n';
	        if (this.acopt != null)
	        {
	            var sopt = this.filter_opt(this.acopt, node.modid); //array_copy(this.acopt);
	            var i = this.find_option(sopt, node.access);
	            if (i > -1)
	            {
	                var s = sopt[i];
	                if (s != null || s.length > 0)
	                {
	                    var tt = s.split('>');
	                    s = tt[0] + ' SELECTED >' + tt[1] + '>';
	                    sopt[i] = s;
	                }
	            }
	            shtm += sopt.join();
	        }
	        else
	        {
	            shtm += '<option value=0 selected>None</option>\n';
	            shtm += '<option value=1>Allowed</option>\n';
	        }
	        shtm += '</select>'; //'</select>\n';
	        var txtval = (node.effdate == null) ? '' : dateToString(node.effdate);
	        shtm += '<input id=' + txtid + ' type=text style="display:none" value="' +txtval + '" \>\n'
    	    shtm += '</td>\n';
    	    shtm += '</tr>\n';
	        if (node.child_nodes.length > 0)
	        {
	            for (var i = 0; i < node.child_nodes.length; i++)
	            {
                    var tblid = this.id + '_tbl_' + node.child_nodes[i].id + '_' + node.child_nodes[i].pid + '_' + node.child_nodes[i].gid;
                    shtm += '<tr>\n<td>\n<table id=' + tblid + ' style="width:100%;" cellpadding=0 cellspacing=0 class=treetab_node>\n';
	            
	                shtm += '<tr>\n';
	                shtm += this.put_node(node.child_nodes[i]);
	                shtm += '</table>\n</td>\n</tr>\n';
	            }    	    
	        }
    	    
    	    return shtm;
    	}
    	
    	tt.toString = function()
    	{
    	    var shtm = '<table style="width:98%" id=' + this.id + ' cellpadding=0 cellspacing=0 class=treetab>\n';
    	    shtm += '<thead>';
    	    shtm += '<tr>\n<th>Groups - Modules - Permission</th>\n</tr>\n';
    	    shtm += '</thead>\n'
    	    for (var i = 0; i < this.child_nodes.length; i++)
    	    {
    	        var tblid = this.id + '_tbl_' + this.child_nodes[i].id + '_' + this.child_nodes[i].pid + '_' + this.child_nodes[i].gid;
    	        shtm += '<tr>\n<td>\n<table id=' + tblid + ' style="width:100%;" cellpadding=0 cellspacing=0><tr>\n';
    	        shtm += this.put_node(this.child_nodes[i]);
    	        shtm += '</table>\n</td>\n</tr>\n';
    	    }
    	    shtm += '</table>';
            
    	    return shtm;
    	}  
    	
    	tt.editMode = function(enable)
    	{
    	    var tobj = document.getElementById(this.id);
    	    if (tobj)
    	    {
    	        var sels = tobj.getElementsByTagName('select');
    	        for (var i=0; i < sels.length; i++)
    	        {
    	            sels.item(i).disable = !enable;
    	        }
    	    }
    	}
    	
    	tt.create_sel_options = function (dtopt)
    	{
    	    this.acopt = new Array();
    	    for (var i = 0; i < dtopt.Rows.length; i++)
    	    {
    	        var sopt = '';
    	        sopt += '<option value=' + dtopt.Rows[i].FldValue + '>' + dtopt.Rows[i].FldCode + '</option>\n';
    	        this.acopt.push(sopt);
    	    }
    	}  	    		       
        
    	tt.create_treetab = function(dt, id, pid, chgfun, dtopt)
    	{
    	    this.id = id;
    	    this.pid = pid;
    	    this.onchange = chgfun;
    	    
    	    for (var i = 0; i < dt.Rows.length; i++)
    	    {
    	        var jn = new treetab_node();
    	        jn.id = dt.Rows[i]['SEQ_ID'];
    	        jn.pid = dt.Rows[i]['MenuParentID'];
    	        jn.gid = dt.Rows[i]['MenuGroup'];
    	        jn.title = dt.Rows[i]['MenuName'];
    	        jn.access = dt.Rows[i]["AccessType"];
    	        jn.modid = dt.Rows[i]["ModuleId"];
    	        jn.effdate = dt.Rows[i]["EffDate"];
    	        this.addItem(jn, false);
    	    }
    	    if (this.unlinked_items.length > 0) this.linkItems();
    	    if (dtopt != null)
    	    {
    	        this.create_sel_options(dtopt);
    	    }
    	    if (this.pid != null)
    	    {
    	        var pobj = document.getElementById(this.pid);
    	        if (pobj)
    	        {
    	            pobj.innerHTML += this.toString();
    	        }
    	    }
    	}    