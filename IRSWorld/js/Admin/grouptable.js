        function gcolumn()
        {
            this.gtype = 0;
            this.header = 'Column';
			this.datafield = 'column';
			this.type = 'text';
			this.options = null;
			this.onchange = '';
			this.classNames = 
			{
			    header: '',
			    datafield: ''
			};
			this.styles = 
			{
			    header: 'text-align: center',
			    datafield: 'width:25%;text-align:left;border:solid .1pt silver'
			};        
		}
		
		function gfield(type, item)
		{
		    this.gtype = type; // 0 = group, 1 = subgroup, 2 = item node, 3 = element
		    this.gitem = item;
		}
    
        function grow(lno, gid, ino, pid)
        {
            this.lno = lno;             // seq id
            this.gid = gid;             // group id
            this.ino = ino;             // item no
            this.pid = pid;             // parent id
            this.changed = false;
            this.gfields = new Array();
        }
        
        grow.prototype.addGfield = function(dat)
        {
            this.gfields.push(dat);
        }
    
        function grouptable()
        {
            this.gcolumns = new Array();
            this.grows = new Array();
            this.caption = '';
            this.classNames =
            {
                header: '',
                datafield: '',
                caption: ''
            };
            this.styles =
            {
                header: 'font-family:Arial;font-weight:bold;font-size:12px;height: 22px;background-color:#003399;color:#CCCCFF;text-align:center;border:solid .2pt silver',
                table: 'width:100%; border: solid 1px azure',
                caption: 'text-align:center;vertical-align:middle;height:20px;width:100%'
            };
            this.changed = 0;
        }
        
        var grptab = grouptable.prototype;
        
        grptab.addGrow = function(row)
        {
            this.grows.push(row);
        }
        
        grptab.addGColumn = function(col)
        {
            this.gcolumns.push(col);
        }
        
        grptab.setStyles = function(ele, stilo)
        {
            var starr = stilo.split(';');
            for (var i = 0; i < starr.length; i++)
            {
                atarr = starr[i].split(':');
                try 
                { 
                    ele.style.setAttribute(atarr[0], atarr[1]); 
                    if (atarr[0] == 'background-color') ele.style.backgroundColor = atarr[1];
                    if (atarr[0] == 'text-align') ele.style.textAlign = atarr[1];
                    if (atarr[0] == 'border') ele.style.border = atarr[1];
                } 
                catch (e) 
                {}
            }
        }
        
        grptab.createCaption = function(tbl)
        {
            var cap = document.createElement('caption');
            cap.innerText = this.caption;
            if (this.styles.caption.length > 0)
            {
                this.setStyles(cap, this.styles.caption);
            }
            if (this.classNames.caption.length > 0)
            {
                cap.className = this.classNames.caption;
            }
            tbl.appendChild(cap);
        }
        
        grptab.createHeaders = function(tbl)
        {
            var thed = document.createElement('thead');
            thed.id = tbl.id + '_thead';
            
            if (this.styles.header.length > 0)
            {
                this.setStyles(thed, this.styles.header);
            }
            
            if (this.classNames.header.length > 0)
            {
                thed.className = this.classNames.header;
            } 
            
            var tr = document.createElement('tr');
            for (var i = 0; i < this.gcolumns.length; i++)
            {
                var th = document.createElement('th');
                if (this.gcolumns[i].styles.header.length > 0)
                {
                    this.setStyles(th, this.gcolumns[i].styles.header);
                    th.innerText = this.gcolumns[i].header;
                }
                tr.appendChild(th);
            }
            thed.appendChild(tr);
            tbl.appendChild(thed);
        }
        
        grptab.createNobody = function(tbl, tbodi)
        {
            var ths = tbl.getElementsByTagName('th');
            var tr = document.createElement('tr');
            var td = document.createElement('td');
            td.colSpan = ths.length;
            td.style.width = '100%';
            td.style.fontFamily = 'Arial';
            td.style.fontWeight = 'bold';
            td.style.fontSize = '16px';
            td.style.color = 'maroon';
            td.align = 'center';
            td.innerText = 'NO NODES TO DISPLAY';
            tr.appendChild(td);
            tbodi.appendChild(tr);        
        }
        
        grptab.createSelectOptions = function(sel, opt)
        {
            if (opt.length > 0)
            {
                for (var i = 0; i < opt.length; i++)
                {
                    var ss = opt[i].split('~');
                    var opt_new = document.createElement('option');
                    sel.options.add(opt_new);
                    opt_new.value = ss[0];
                    opt_new.innerText = ss[1];
                }                
            }
        }
        
        grptab.createSelect = function(opt, elid, field)
        {
            var sel = document.createElement('select');
            sel.id = elid;
            this.createSelectOptions(sel, opt);
            sel.selectedIndex = (field > 2) ? 2 : field;
            sel.style.width = '98%';
            sel.style.border = 'solid 1px midnightblue';
            return sel;
        }

        grptab.displayRows = function(tbodi)
        {
            for (var i = 0; i < this.grows.length; i++)
            {
                var tr = document.createElement('tr');
                tr.id = this.id + '_TR_' + i;
                for (var j = 0; j < this.gcolumns.length; j++)
                {
                    var td = document.createElement('td');
                    td.id = this.id + '_TD_' + i + '_' + j;
                    if (this.gcolumns[j].styles.datafield.length > 0)
                    {
                        this.setStyles(td, this.gcolumns[j].styles.datafield);
                    }
                    td.innerText = ' ';
                    tr.appendChild(td);
                }
                
                var tds = tr.getElementsByTagName('td');
                
                for (var j = 0; j < this.gcolumns.length; j++)
                {
                    var fld = this.grows[i].gfields[j];

                    if (this.gcolumns[j].type == 'text')
                    {
                        var td = tds.item(fld.gtype);
                        if (fld.gitem != null && fld.gitem.length > 0)
                            td.innerText = fld.gitem;
                    }
                    else if (this.gcolumns[j].type == 'select')
                    {
                        var td = tds.item(j);
                        td.innerText = '';
                        var sel = this.createSelect(this.gcolumns[j].options, this.id + '_' + 'ddl_' + i + '_' + j, fld.gitem);
                        if (this.gcolumns[j].onchange.length > 0)
                        {
                            var me = this;
                            sel.attachEvent('onchange', 
                                function () 
                                { 
                                    var ss = event.srcElement.id.split('_');
                                    if (event.srcElement.id.indexOf('ddl') > -1)
                                    {
                                        var j = parseInt(ss[3]);
                                        eval(me.gcolumns[j].onchange + "('" + event.srcElement.id + "');");
                                    }
                                });
                        }
                        td.appendChild(sel); 
                    }
                }
                tbodi.appendChild(tr);
            }
        }
        
        grptab.createBody = function(tbl)
        {
            var tbodi = document.createElement('tbody');
            tbodi.id = tbl.id + '_body';

            if (this.grows.length > 0)
            {
                this.displayRows(tbodi);
            }
            else
            {
                this.createNobody(tbl, tbodi);
            }
        
            tbl.appendChild(tbodi);
        }
        
        grptab.createTable = function(pid, tid)
        {
            this.pid = pid;
            this.id = tid;
            var prent = document.getElementById(pid);
            if (prent != null)
            {
                var kid = document.getElementById(tid);
                if (kid != null)
                {   
                    try { prent.removeChild(kid); } catch (e) {}
                }
                var tbl = document.createElement('table');
                tbl.id = tid;

                tbl.cellSpacing = 0;
                tbl.cellPadding = 0;
                if (this.styles.table.length > 0)
                {
                    this.setStyles(tbl, this.styles.table);
                }
                if (this.caption != null && this.caption.length > 0)
                {
                    this.createCaption(tbl);
                }                
                this.createHeaders(tbl);
                this.createBody(tbl);
                prent.appendChild(tbl);
            }
        }