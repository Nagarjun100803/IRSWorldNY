
/****************************************
* Carlos H. Raymundo (c) 2000 PNB       *
* Requires commons.js                   *
*****************************************/ 
function datagrid_column()
{
    this.header = 'Column';
	this.datafield = 'column';
	this.type = 'text';
	this.options = null;
	this.href = null;
	this.onchange = '';
	this.oninit = '';
	this.onclick = '';
	this.onblur = ''; 
	this.onkeydown = '';
	this.onkeyup = '';
	this.align = 'left';
	this.editable = true;
	this.value = this.type;
	this.formatter = '';
	this.edit_button = (this.value.toUpperCase() == 'EDIT');
	this.delete_button = (this.value.toUpperCase() == 'DELETE');
	this.add_button = (this.value.toUpperCase() == 'ADD');
	this.classNames =
	{
	    header: '',
	    datafield: '',
	    cell: ''
	};
	this.styles =
	{
	    header: 'font-family:Arial;font-size: 12px;font-weight:bold;text-align:center;border:solid .1pt silver;border-collapse:collapse;',
	    cell: 'font-family:Arial;font-size:12px;font-weight:bold;border:solid .1pt silver;border-collapse:collapse;',
	    datafield: 'font-family:Arial;font-size:12px;font-weight:bold;background-color:transparent;'
	};
	// for type == 'image'
	this.src = '';
	this.imgAlign = 'absmiddle';
	// for type == 'datagrid';
	this.datagrid_create_function = null; // name of function  that creates columns
	this.datagrid_databind_function = null; // name of function that get data and binds.
}

function datagrid_editcolumn()
{
    this.datafield = '';
    this.index = -1;
    this.value = null;
}

function datagrid_editrow()
{
    this.datarow = -1;
    this.gridrow = -1;
    this.action = '';
    this.columns = new Array();
}

datagrid_editrow.prototype.addColumn = function(coldat)
{
    this.columns.push(coldat);
}

function datagrid_filter()
{
    this.datafield = '';
    this.operator = '';
    this.value = '';
}

function datagrid()
{
    this.id = null;
    this.parent_id = null;
    this.datatable = null;
    this.columns = new Array();
    this.cellspacing = 0;
    this.cellpadding = 0;
    this.rows_page = 12;
    this.num_pages = 1;
    this.curr_row = 0;
    this.curr_page = 1;
    this.caption = '';
    this.classNames =
    {
    	table: '',
        header: '',
        caption: ''
    };
    this.styles =
    {
        header: 'font-family:Arial;font-weight:bold;font-size:12px;width:100%;height: 22px;background-color:#003399;color:#CCCCFF;text-align:center;border:solid .2pt silver;border-collapse:collapse;',
        table: 'width:100%;font-family:Arial;font-weight:bold;font-size:12px; border: solid 1px azure;color:midnightblue;',
        caption: 'font-family:Arial;font-weight:bold;background-color:lavender;color:midnightblue;text-align:center;vertical-align:middle;height:20px;width:100%;'
    };
    this.norecord =
    {
        text: 'NO RECORD FOUND',
        styles: 'width:100%;font-family:Arial;font-weight:bold;font-size:16px;color:maroon;text-align:center'
    }
    this.row =
    {
        onclick_enabled: true,
    	onclick: '',
    	selected_style: 'background-color:lavender',
    	normal_style: 'background-color: transparent',
    	selected: -1,
    	selected_data_row: -1
    };
    this.footer =
    {
        none: false,
        edit_button: 
        {
            none: true,
            value: 'Edit',
            style: 'width: 100%;',
            beforepost: '',
            onpost: '',
            onclick: ''
        },        
        add_button: 
        {
            disabled: false,
            none: true,
            value: 'Add',
            style: 'width: 100%;',
            beforepost: '',
            onpost: '',
            onclick: ''
        },
        delete_button: 
        {
            none: true,
            value: 'Delete',
            style: 'width: 100%;',
            onpost: '',
            onclick: ''
        },        
        save_button: 
        {
            always_enabled: false,
            none: true,
            value: 'Save',
            style: 'width: 100%;',
            onclick: ''
        },        
        cancel_button: 
        {
            none: true,
            value: 'Cancel',
            style: 'width: 100%;',
            onclick: ''
        },
        refresh_button: 
        {
            none: true,
            value: 'Reload',
            style: 'width: 100%;',
            onclick: ''
        },
        find_button: 
        {
            none: true,
            value: 'Find',
            style: 'width: 100%;',
            onclick: ''
        },                        
        styles: 
        {
            body: 'background-color:#99CCCC;color:#fffff7;font-family:Arial;font-weight:bold;font-size:10px;',
            literals: 'background-color:#99CCCC;color:#fffff7;font-family:Arial;font-weight:bold;font-size:10px;',
            select: 'font-family: Arial; font-weight:bold; font-size:10px; color: midnightblue;'
        }
    }
    this.curract = '';
    this.newRows;
    this.editRows;
    this.deletedRows;
    this.edit_button_prefix = '';
    this.delete_button_prefix = '';
    this.add_button_prefix = '';
    this.autoPost = true;
    this.postToServer = false;
    this.subgrid = false;
    this.addOnTop = false;
    this.addOnTopRerender = true;
    
    this.filter =
    {
        enabled: false,
        filters: new Array(),
        operators: new Array()
    };
    
    this.datagrids = null;
    this.noheader = false;
    
    this.save_row_sep = '~';
    this.save_col_sep = '|';
    this.save_colname_value_sep = '^';
    this.on_page_change = "";
}

var gv = datagrid.prototype;

gv.addFilter = function(filter, oper)
{
    this.filter.filters.push(filter);
    if (oper != null) this.filter.operators.push(oper);
}

gv.clearFilter = function()
{
    this.filter.enabled = false;
    this.filter.filters = new Array();
    this.filter.operators = new Array();
}

gv.addNewRow = function(row)
{
    if (this.newRows == null) this.newRows = new Array();
    this.newRows.push(row);
}

gv.addEditedRow = function(row)
{
    if (this.editRows == null) this.editRows = new Array();
    this.editRows.push(row);
}

gv.addDeletedRow = function(row)
{
    if (this.deletedRows == null) this.deletedRows = new Array();
    this.deletedRows.push(row);
}

gv.addColumn = function(col)
{
    this.columns.push(col);
}

gv.setStyles = function(ele, stilo)
{
	ele.style.cssText = stilo;
}

gv.createCaption = function(tbl)
{
    var cap = document.createElement('caption');
    setText(cap, this.caption); //cap.innerText = this.caption;
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

gv.createHeaders = function(tbl)
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
    for (var i = 0; i < this.columns.length; i++)
    {
        var th = document.createElement('th');
        if (this.columns[i].styles.header.length > 0)
        {
            this.setStyles(th, this.columns[i].styles.header);
            setText(th, this.columns[i].header); //th.innerText = this.columns[i].header;
        }
        tr.appendChild(th);
    }
    thed.appendChild(tr);
    tbl.appendChild(thed);
}

gv.createOption = function(ddl, code, desc)
{
    var opt_new = document.createElement('option');
    ddl.options.add(opt_new);
    opt_new.value = code;
    setText(opt_new, desc); 
}

gv.pg_do_post = function()
{
    if ((this.curract == 'Edit' | this.curract == 'Add'))
    {
        var post = (this.autoPost) ? true : confirm('Post changes?');
        if (post)
        {
            var btnid = (this.curract == 'Edit') ? this.edit_button_prefix : this.add_button_prefix;
            if (!document.getElementById(btnid))
            {
                btnid += this.row.selected;
            }
            var val = this.getDataField(btnid);
            this.checkEditButton(btnid, this.row.selected_data_row, this.row.selected, val);
        }
    }
}

gv.pg_previous_clicked = function()
{
    this.pg_do_post();
    
    if (this.addOnTop) 
    {
        if ((this.curr_row + this.rows_page) < (this.datatable.Rows.length + this.newRows.length))
        {
            this.curr_page--;
            this.curr_row += this.rows_page;
            this.createRows();
            this.updateTfoot();
            return;
        }    
    }
    else
    {
        if ((this.curr_row - this.rows_page) >= 0)
        {
            this.curr_page--;
            this.curr_row -= this.rows_page;
            this.createRows();
            this.updateTfoot();
        }
    }
    
    if (this.on_page_change.length > 0)
    {
        //window.execScript
        eval
        (
            this.on_page_change + '(' + this.curr_page + ');'
        );                
    }    
}

gv.pg_next_clicked = function()
{
    this.pg_do_post();
    
    if (this.addOnTop)
    {
        if ((this.curr_row - this.rows_page) >= 0)
        {
            this.curr_page++;
            this.curr_row -= this.rows_page;
            this.createRows();
            this.updateTfoot();
            return;
        }    
    }
    else
    {
        if ((this.curr_row + this.rows_page) < (this.datatable.Rows.length + this.newRows.length))
        {
            this.curr_page++;
            this.curr_row += this.rows_page;
            this.createRows();
            this.updateTfoot();
            return;
        }
    }
    
    if (this.on_page_change.length > 0)
    {
        //window.execScript
        eval
        (
            this.on_page_change + '(' + this.curr_page + ');'
        );                
    }
}

gv.pg_select_clicked = function()
{
    this.pg_do_post();
    
    this.pg_position(document.getElementById(this.id + '_pg_select').selectedIndex + 1);
}

gv.pg_position = function(pgno)
{
    this.curr_page = pgno;
    this.curr_row = (this.addOnTop) ? this.dataRowsSize() - ((pgno - 1) * this.rows_page) : (pgno - 1) * this.rows_page;
    this.createRows();
    this.updateTfoot();
    if (this.on_page_change.length > 0)
    {
        //window.execScript
        eval
        (
            this.on_page_change + '(' + pgno + ');'
        );                
    }
}

gv.updateTfoot = function()
{
    setText(document.getElementById(this.id + '_pg_start'), '' + this.curr_page);
    document.getElementById(this.id + '_pg_select').selectedIndex = this.curr_page - 1;
    setText(document.getElementById(this.id + '_pg_end'), '' + this.num_pages);
    var sel = document.getElementById(this.id + '_pg_select');
    var opt = sel.options;
    if (this.num_pages > opt.length)
    {
        this.createOption(sel, '' + this.num_pages, '' + this.num_pages);
    }
}

gv.createPager = function()
{
    var tbfoot = document.createElement('table');
    tbfoot.width = '100%';
    tbfoot.cellPadding = '2px';
    tbfoot.cellSpacing = 0;

    var frow = tbfoot.insertRow(-1);
    var me = this;

    var td1 = frow.insertCell(-1);
    this.setStyles(td1, this.footer.styles.literals);
    td1.style.width = '5%';
    td1.align = 'right';
    var a_prev = document.createElement('a');
    a_prev.id = this.id + '_pg_prev';
    a_prev.style.width = '100%';
    a_prev.style.cursor = 'hand';
    a_prev.onclick = function ()
    {
        me.pg_previous_clicked();
    };
    setText(a_prev, String.fromCharCode(171)); 
    this.setStyles(a_prev,this.footer.styles.literals);
    td1.appendChild(a_prev);

    var td2 = frow.insertCell(-1);
    this.setStyles(td2, this.footer.styles.literals);
    td2.style.width = '5%';
    td2.align = 'right';
    var s_span = document.createElement('span');
    s_span.id = this.id + '_pg_start';
    s_span.style.width = '100%';
    setText(s_span, '0'); 
    td2.appendChild(s_span);

    var td3 = frow.insertCell(-1);
    td3.align = 'center';
    this.setStyles(td3, this.footer.styles.literals);
    td3.style.width = '5%';
    setText(td3, ' Of '); 

    var td4 = frow.insertCell(-1);
    this.setStyles(td4, this.footer.styles.literals);
    td4.align = 'left';
    td4.style.width = '5%';
    var e_span = document.createElement('span');
    e_span.id = this.id + '_pg_end';
    e_span.style.width = '100%';
    setText(e_span, '0'); 
    td4.appendChild(e_span);

    var td5 = frow.insertCell(-1);
    this.setStyles(td5, this.footer.styles.literals);
    td5.style.width = '5%';
    td5.align = 'left';
    var a_next = document.createElement('a');
    a_next.id = this.id + '_pg_next';
    a_next.style.cursor = 'hand';
    a_next.onclick = function ()
    {
        me.pg_next_clicked();
    };
    setText(a_next, String.fromCharCode(187)); 
    this.setStyles(a_next, this.footer.styles.literals);
    td5.appendChild(a_next);

    var td6 = frow.insertCell(-1);
	if (this.footer.styles.select.length > 0) this.setStyles(td6, this.footer.styles.select);
    td6.align = "left";
    td6.style.width = '15%';
    var tsel = document.createElement('select');
    tsel.style.width = '60px';
    tsel.id = this.id + '_pg_select';
    tsel.onchange = function()
        {
            me.pg_select_clicked();
        };
    td6.appendChild(tsel);
    
    setText(s_span, this.curr_page); 
    setText(e_span, this.num_pages); 

    for (var i = 0; i < this.num_pages; i++)
    {
        var j = i + 1;
        this.createOption(tsel, '' + j, '' + j);
    }
    
    return tbfoot;
}

gv.enableFooterBtn = function(btnid, enable)
{
    var fbtn = document.getElementById(btnid);
    if (fbtn != null) 
    {
        var x = !enable;
        fbtn.disabled = x;
        if (fbtn.disabled != x)
        {
            alert(ftbn.id + " disabled property not " + x)
        }
    }
}

gv.hasDataRows = function()
{
    return ((this.datatable.Rows.length + this.newRows.length - this.deletedRows.length) > 0);
}

gv.dataRowsSize = function()
{
    return (this.datatable.Rows.length + this.newRows.length) - this.deletedRows.length;
}

gv.enableFooterBtns = function(curract)
{
    var enasav = (((this.newRows.length > 0 | this.editRows.length > 0 | this.deletedRows.length > 0) > 0) | this.footer.save_button.always_enabled);
    this.enableFooterBtn(this.id + '_footerbtn_' + this.footer.save_button.value, enasav);   

    if (curract == 'SEL')
    {
        this.enableFooterBtn(this.id + '_footerbtn_' + this.footer.cancel_button.value, false);
        this.enableFooterBtn(this.id + '_footerbtn_' + this.footer.delete_button.value, this.hasDataRows());
        this.enableFooterBtn(this.id + '_footerbtn_' + this.footer.add_button.value, !this.footer.add_button.disabled);
        this.enableFooterBtn(this.id + '_footerbtn_' + this.footer.edit_button.value,  this.hasDataRows());
        if (!this.footer.find_button.none)
        {
            this.enableFooterBtn(this.id + '_footerbtn_' + this.footer.find_button.value,  this.hasDataRows());
        }
        return;
    }
    if (curract == 'Add')
    {
        this.enableFooterBtn(this.id + '_footerbtn_' + this.footer.cancel_button.value, true);
        this.enableFooterBtn(this.id + '_footerbtn_' + this.footer.delete_button.value, false);
        this.enableFooterBtn(this.id + '_footerbtn_' + this.footer.add_button.value, !this.footer.add_button.disabled);
        this.enableFooterBtn(this.id + '_footerbtn_' + this.footer.edit_button.value, false);
        if (!this.footer.find_button.none)
        {
            this.enableFooterBtn(this.id + '_footerbtn_' + this.footer.find_button.value,  false);
        }        
        return;
    }    
    if (curract == 'Edit')
    {
        this.enableFooterBtn(this.id + '_footerbtn_' + this.footer.cancel_button.value, true);
        this.enableFooterBtn(this.id + '_footerbtn_' + this.footer.delete_button.value, false);
        this.enableFooterBtn(this.id + '_footerbtn_' + this.footer.add_button.value, false);
        this.enableFooterBtn(this.id + '_footerbtn_' + this.footer.edit_button.value, true);
        if (!this.footer.find_button.none)
        {
            this.enableFooterBtn(this.id + '_footerbtn_' + this.footer.find_button.value, false);
        }        
        return;
    }
    if (curract == 'Delete')
    {
        this.enableFooterBtn(this.id + '_footerbtn_' + this.footer.cancel_button.value, false);
        this.enableFooterBtn(this.id + '_footerbtn_' + this.footer.delete_button.value, false);
        this.enableFooterBtn(this.id + '_footerbtn_' + this.footer.add_button.value, false);
        this.enableFooterBtn(this.id + '_footerbtn_' + this.footer.edit_button.value, false);
        if (!this.footer.find_button.none)
        {
            this.enableFooterBtn(this.id + '_footerbtn_' + this.footer.find_button.value,  false);
        }        
        return;
    }
    if (curract == 'Cancel')
    {
        this.enableFooterBtn(this.id + '_footerbtn_' + this.footer.cancel_button.value, false);
        this.enableFooterBtn(this.id + '_footerbtn_' + this.footer.delete_button.value, false);
        this.enableFooterBtn(this.id + '_footerbtn_' + this.footer.add_button.value, false);
        this.enableFooterBtn(this.id + '_footerbtn_' + this.footer.edit_button.value, false);
        if (!this.footer.find_button.none)
        {
            this.enableFooterBtn(this.id + '_footerbtn_' + this.footer.find_button.value,  this.hasDataRows());
        }        
        return;
    } 
    if (curract == 'Init')
    {
        this.enableFooterBtn(this.id + '_footerbtn_' + this.footer.cancel_button.value, false);
        this.enableFooterBtn(this.id + '_footerbtn_' + this.footer.delete_button.value, false);
        this.enableFooterBtn(this.id + '_footerbtn_' + this.footer.add_button.value, !this.footer.add_button.disabled);
        this.enableFooterBtn(this.id + '_footerbtn_' + this.footer.edit_button.value, false);
        if (!this.footer.find_button.none)
        {
            this.enableFooterBtn(this.id + '_footerbtn_' + this.footer.find_button.value, this.hasDataRows());
        }        
        return;
    }     
}

gv.doAddRow = function()
{
    if (this.addOnTop & this.addOnTopRerender) 
    {
        this.createRows();
    }
    else
        this.pg_position(this.num_pages);
    var i = this.dataRowsSize();
    var grow = (! this.addOnTop) ? document.getElementById(this.id + '_tbody').rows.length : 0;
    this.curract = 'Add';
    var tr = this.createDataRow(i, grow);
    for (var j = 0; j < this.columns.length; j++)
    {
        tr.appendChild(this.createCell(this.columns[j], i, grow, ''));
    }
    if (! this.addOnTop)
    {
        document.getElementById(this.id + '_tbody').appendChild(tr);
    }
    this.setRowToEditMode(i, grow, true);
	var ebtn = document.getElementById(this.add_button_prefix);
	if (!ebtn)
	{
	    ebtn = document.getElementById(this.add_button_prefix + i);
	}
	if (ebtn) ebtn.value = 'Post';
}

gv.getDataAt = function(datarow, col)
{
    var val;
    
    if (datarow >= this.datatable.Rows.length)
    {
        var ndx = datarow - this.datatable.Rows.length;
        val = this.newRows[ndx].columns[col].value;
    }                    
    else
    {
        val = this.datatable.Rows[datarow][this.columns[col].datafield];
    }
    
    return val;
}

gv.getValues = function (grid_row)
{
    var retval = new Array();
    var tr = document.getElementById(this.id + '_tr_' + grid_row);
    if (tr)
    {
        var sells = tr.cells;
        for (var j = 0; j < sells.length; j++)
        {
            retval.push(this.getValueAt(grid_row, j));
        }
        
        return retval;
    }
    
    return null;
}

gv.getElementsAt = function (grid_row, col)
{
    var tr = document.getElementById(this.id + '_tr_' + grid_row);
    if (tr)
    {
        var sell = tr.cells[col];
        return sell.childNodes;
    }
    
    return null;
}

gv.getValueAt = function(grid_row, col)
{
    var val;
    var tr = document.getElementById(this.id + '_tr_' + grid_row);
    if (tr)
    {
        var sell = tr.cells[col];
        var childs = sell.childNodes;
        for (var k=0; k < childs.length; k++)
        {
            var tag = childs[k].nodeName;
            var typ = childs[k].type;
            if ( (tag == 'INPUT' & (typ == 'text' | typ == 'password')) | tag == 'SELECT' )
            {
                val = childs[k].value;
            } 
            else if (tag == 'INPUT' & typ == 'checkbox')
            {
                val = childs[k].checked;
            } 
            else
            {
                val = getText(childs[k]); //val = childs[k].innerText;
            }     
        }        
    }
    return val;
}

gv.setColumn = function(col, val)
{
    var tr = document.getElementById(this.id + '_tr_' + this.row.selected_data_row);
    if (tr)
    {
        var sell = tr.cells[col];
        var childs = sell.childNodes;
        for (var k=0; k < childs.length; k++)
        {
            var tag = childs[k].nodeName;
            var typ = childs[k].type;
            if (tag == 'IMG') continue;
            var xval = val;
            
            if (val == null) xval = '';
            else if (this.columns[col].formatter.length > 0)
            {
                xval = eval(this.columns[col].formatter + '(' + val + ');');
            }
            if ( (tag == 'INPUT' & (typ == 'text' | typ == 'password')) | tag == 'SELECT' )
            {
                childs[k].value = xval;
            } 
            else if  (tag == 'INPUT' & typ == 'checkbox') 
            {
                childs[k].checked = xval;
            }
            else
            {
                setText(childs[k], xval); 
            }     
        }        
    }
}

gv.setColumnByName = function(colname, val)
{
    var tr = document.getElementById(this.id + '_tr_' + this.row.selected_data_row);
    if (tr)
    {
        var sells = tr.getElementsByTagName('TD');
        for (var k = 0; k < sells.length; k++)
        {
            if (sells.item(k).id.indexOf(colname) > -1) 
            {
                this.setColumn(k, val);
                return;
            }
        }
    }
}

gv.refreshRow = function()
{
    var i = this.row.selected_data_row;
    var x = (this.addOnTop) ? i : this.row.selected;
    var trid = this.id + '_tr_' + x;
    var tr = document.getElementById(trid);
    if (tr)
    {
        var sells = tr.cells;
        for (var j = 0; j < sells.length; j++)
        {
            if (sells.item(j).hasChildNodes())
            {
                var childs = sells.item(j).childNodes;
                for (var k=0; k < childs.length; k++)
                {
                    var tag = childs[k].nodeName;
                    var typ = childs[k].type;
                    
                    if (tag == 'IMG') continue;
                    
                    var ss = this.columns[j].datafield; 
                    var val;
                    if (i >= this.datatable.Rows.length)
                    {
                        var ndx = i - this.datatable.Rows.length;
                        val = this.newRows[ndx].columns[j].value;
                    }                    
                    else
                    {
                        val = this.datatable.Rows[i][ss];
                    }
                    
                    if (val == null) val = '';
                    if (this.columns[j].formatter.length > 0)
                    {
                        val = eval(this.columns[j].formatter + '(' + val + ');');
                    }
                    else if (val.constructor == Date)
                    {
                        val = dateToString(val);
                    }
                    if ( (tag == 'INPUT' & (typ == 'text' | typ == 'password')) | tag == 'SELECT' )
                    {
                        childs[k].value = val;
                    }
                    else if (tag == 'INPUT' & typ == 'checkbox')
                    {
                        childs[k].checked = val;
                    }
                    else if (tag == 'input' & typ != 'button') setText(childs[k], val); 
                }
            }
        }
        
    }

}

gv.doCancel = function()
{
    var pref = '';
    var val;
    
    if (this.curract == 'Edit')
    {
        this.refreshRow();
        this.setRowToEditMode(this.row.selected_data_row, this.row.selected, false);
        pref = this.edit_button_prefix;
        if (pref.indexOf('_footerbtn_') > -1) val = this.footer.edit_button.value;
    }
    if (this.curract == 'Add')
    {
        var tbodi = document.getElementById(this.id + '_tbody');
        var grow = (this.addOnTop) ? 0 : tbodi.rows.length - 1;
        var tr = tbodi.rows.item(grow);
        tbodi.removeChild(tr);
        if (tbodi.rows.length == 0)
        {
            this.createNobody();
        }
        pref = this.add_button_prefix;
        if (pref.indexOf('_footerbtn_') > -1) val = this.footer.add_button.value;
    }
    if (this.curract == 'Add' | this.curract == 'Edit')
    {    
        var fbtn = document.getElementById(pref);
        if (!fbtn)
        {
            fbtn = document.getElementById(pref + this.row.selected_data_row);
        }
        if (fbtn) fbtn.value = (val != null) ? val : this.getDataField(fbtn.id);
    }
    this.row.onclick_enabled = true;
}

gv.getChanges = function(crud, uprow)
{
    var s = crud + this.save_row_sep; //'~';
    var ss = '';
    var sep = this.save_colname_value_sep; //'^';
    for (var i = 0; i < uprow.length; i++)
    {
        if (uprow[i] == null) continue;
        if (crud == 'delete' & uprow[i].datarow > this.datatable.Rows.length) continue;
        ss += uprow[i].datarow + this.save_col_sep; //'|';
        for (var j = 0; j < uprow[i].columns.length; j++)
        {
            if (uprow[i].columns[j].datafield.length == 0) continue;
            
            ss += uprow[i].columns[j].datafield + sep + uprow[i].columns[j].value;
            if (j < (uprow[i].columns.length - 1)) ss += this.save_col_sep; //'|';
        }
        if (i < (uprow.length - 1)) ss += this.save_row_sep; //'~';
    }    
    
    if (ss.length > 0) return s + ss;
    return '';    
}

gv.doSave = function(fun)
{
    var se = '';
    var sd = '';
    var sn = '';
    
    if (this.editRows.length > 0)
    {
        se = this.getChanges('edit', this.editRows);
    }
    if (this.deletedRows.length > 0)
    {
        sd = this.getChanges('delete', this.deletedRows);
    }
    if (this.newRows.length > 0)
    {
        sn = this.getChanges('add', this.newRows);
    }

    eval
    (
        'var arg = ' +
        '{' +
            'value:"' + this.footer.save_button.value + '", ' +
            'edits: "' + se + '", ' +
            'deletes: "' + sd + '", ' +
            'additions: "' + sn + '", ' +
            'control: "' + this.id + '"' +
        '}; ' +
        fun + '( arg );'
    );	
    
    this.enableFooterBtns('SEL');
    this.enableFooterBtn(this.id + '_footerbtn_' + this.footer.save_button.value, false);   
}

gv.callOnPost = function(fun)
{
    //window.execScript
    eval
    (
        'var arg = ' +
        '{' +
            'action: "' + this.curract + '", ' +
            'datarow: ' + this.row.selected_data_row + ', ' +
            'gridrow: ' + this.row.selected + ', ' +
            'control: "' + this.id + '_tr_' + this.row.selected_data_row + '"' +
        '}; ' +
        fun + '( arg );'
    );    
}

gv.getFooterButtonElement = function(val)
{
    return document.getElementById(this.id + '_footerbtn_' + val);
}

gv.footerButtonClick = function(fun, val)
{
	if (val == this.footer.add_button.value)
	{
	    this.doAddRow();
	}
	else if (val == this.footer.delete_button.value)
	{
	    this.deleteRow(this.row.selected_data_row, this.row.selected);
        if (this.postToServer & this.footer.delete_button.onpost.length > 0)
        {
            this.doSave(this.footer.delete_button.onpost);
        }	    
	}
	else if (val == this.footer.edit_button.value)
	{
	    if (this.row.selected == -1)
	    {
	        alert("Please select a grid row.");
	        return;
	    }
	    this.checkEditButton(this.id + '_footerbtn_' + this.footer.edit_button.value, this.row.selected_data_row, this.row.selected, this.footer.edit_button.value);
	}
	else if (val == 'Post')
	{
		var save_act = this.curract;

	    if (this.curract == 'Add' & this.footer.add_button.beforepost.length > 0)
	    {
            this.callOnPost(this.footer.add_button.beforepost);
	    }
	    else if (this.curract == 'Edit' & this.footer.edit_button.beforepost.length > 0)
	    {
            this.callOnPost(this.footer.edit_button.beforepost);
	    }
	    if (this.curract == '') 
	    {
	        this.curract = save_act; 
	        return; // might have been canceled.
	    }
	    var btnid = (this.curract == 'Add') ? this.id + '_footerbtn_' + this.footer.add_button.value : this.id + '_footerbtn_' + this.footer.edit_button.value;
	    var btnval = (this.curract == 'Add') ? this.footer.add_button.value : this.footer.edit_button.value;
	    this.checkEditButton(btnid, this.row.selected_data_row, this.row.selected, btnval);

        if (save_act == 'Add' & this.footer.add_button.onpost.length > 0)
        {
            if (this.postToServer)
            {
                this.doSave(this.footer.add_button.onpost);
                this.clearChanges();
            }
            else
            {
                this.callOnPost(this.footer.add_button.onpost);
            }
	    }
	    
        if (save_act == 'Edit' & this.footer.edit_button.onpost.length > 0)
        {
            if (this.postToServer)
            {
                this.doSave(this.footer.edit_button.onpost);
                this.clearChanges();
            }
            else
            {
                this.callOnPost(this.footer.edit_button.onpost);
            }
	    }
	    save_act = '';
	    return;
	}
	else if (val == this.footer.cancel_button.value)
	{
	    this.doCancel();
	    if (this.hasDataRows()) this.enableFooterBtns('SEL');
	    this.curract = '';
	}
	else if (val == this.footer.save_button.value)
	{
	    if (fun.length == 0 | this.footer.save_button.onclick.length == 0)
	    {
	        alert ('Caller must provide a handler for the save operation');
	        return;
	    }
	    if (this.curract == 'Add' | this.curract == 'Edit')
	    {
	        this.footerButtonClick('', 'Post');
	    }
	    if (document.getElementById(this.id + '_footerbtn_' + this.footer.save_button.value))
	    {
	        document.getElementById(this.id + '_footerbtn_' + this.footer.save_button.value).disabled = true;
	    }
	}
    if (fun.length > 0)
    {
        if (val != this.footer.save_button.value)
        {
	        eval
	        (
                'var arg = ' +
                '{' +
                    'datarow: ' + this.row.selected_data_row + ', ' +
                    'gridrow: ' + this.row.selected + ', ' +
                    'value: "' + val + '" ' +
                '}; ' +
	            fun + '( arg );'
	        );
	    }
	    else
	    {
	        this.doSave(fun);
	    }
	}
}


gv.createFooterButtons = function(fbtn)
{
    var btn = document.createElement('input');
    btn.type = 'button';
    btn.id = this.id + '_footerbtn_' + fbtn.value;
    btn.value = fbtn.value;
    if (fbtn.style.length > 0) this.setStyles(btn, fbtn.style);
    var me = this;
    btn.onclick = function(evt)
    { 
        if (!evt) var evt = window.event;
        me.footerButtonClick(fbtn.onclick, btn.value); 
        evt.cancelBubble = true; 
    };        
    btn.disabled = (btn.value != 'Reload' | btn.value != 'Find' | btn.value != 'Add');
    return btn;
}

gv.createFooterAddons = function(td)
{
    var count = 0;
    var tb = document.createElement('table');
    tb.cellSpacing = '2px';
    tb.cellPadding = 0;
    var tr = tb.insertRow(-1);
    if (!this.footer.edit_button.none)
    {
        var td1 = tr.insertCell(-1);
        var abtn = this.createFooterButtons(this.footer.edit_button);
        if (this.edit_button_prefix.length == 0) this.edit_button_prefix = abtn.id;
        td1.appendChild(abtn);
        td1.style.width = '45px';
        td1.align = 'left';
        td1.id = this.id + '_footertd_' + this.footer.edit_button.value;
        count++;
    }
    if (!this.footer.add_button.none)
    {
        var td1 = tr.insertCell(-1);
        var abtn = this.createFooterButtons(this.footer.add_button);
        if (this.add_button_prefix.length == 0) this.add_button_prefix = abtn.id;
        td1.appendChild(abtn);
        td1.style.width = '45px';
        td1.align = 'left';
        td1.id = this.id + '_footertd_' + this.footer.add_button.value;
        count++;
    }
    if (!this.footer.delete_button.none)
    {
        var td1 = tr.insertCell(-1);
        var abtn = this.createFooterButtons(this.footer.delete_button);
        if (this.delete_button_prefix.length == 0) this.delete_button_prefix = abtn.id;
        td1.appendChild(abtn);
        td1.style.width = '45px';
        td1.align = 'left';
        td1.id = this.id + '_footertd_' + this.footer.delete_button.value;
        count++;
    }    
    if (!this.footer.save_button.none)
    {
        var td2 = tr.insertCell(-1);
        var sbtn = this.createFooterButtons(this.footer.save_button);
        td2.appendChild(sbtn);
        td2.style.width = '45px';
        td2.align = 'left';
        td2.id = this.id + '_footertd_' + this.footer.save_button.value;
        count++;
    }    
    if (!this.footer.cancel_button.none)
    {
        var td3 = tr.insertCell(-1);
        var cbtn = this.createFooterButtons(this.footer.cancel_button);
        td3.appendChild(cbtn);
        td3.style.width = '45px';
        td3.align = 'left';
        td3.id = this.id + '_footertd_' + this.footer.cancel_button.value;
        
        count++;
    } 
    if (!this.footer.refresh_button.none)
    {
        var td4 = tr.insertCell(-1);
        var rbtn = this.createFooterButtons(this.footer.refresh_button);
        td4.appendChild(rbtn);
        td4.style.width = '45px';
        td4.align = 'left';
        td4.id = this.id + '_footertd_' + this.footer.refresh_button.value;
        count++;
    }  
    if (!this.footer.find_button.none)
    {
        var td5 = tr.insertCell(-1);
        var rbtn = this.createFooterButtons(this.footer.find_button);
        td5.appendChild(rbtn);
        td5.style.width = '45px';
        td5.align = 'left';
        td5.id = this.id + '_footertd_' + this.footer.find_button.value;
        count++;
    }            
    var w = count * 47;      
    tb.style.width = '' + w + 'px';
    if (tr.cells.length > 0)
        td.appendChild(tb);
}

gv.hideFooterButton = function(val, hide)
{
    if (hide)
    {
        var fbtn = document.getElementById(this.id + '_footerBtn_' + val);
        var ftd = document.getElementById(this.id + '_footertd_' + val);
        
        if (ftd != null & fbtn != null)
        {
            ftd.style.display = (hide) ? 'none' : '';
        }
    }
}

gv.createFooterElements = function()
{
    var tb = document.createElement('table');
    tb.style.width = '100%';
    tb.cellSpacing = 0;
    tb.cellPadding = 0;
    var tr = tb.insertRow(-1);
    var td1 = tr.insertCell(-1);
    var td2 = tr.insertCell(-1);
    
    td1.style.width = '50%';
    td1.align = 'left';
    this.createFooterAddons(td1);
    
    td2.style.width = '50%';
    td2.align = 'right';
    td2.valign = 'middle';
    td2.appendChild(this.createPager());
    
    return tb;
}

gv.createFooter = function(tbl)
{
    var tfut = document.createElement('tfoot');
    tfut.id = tbl.id + '_tfoot';
    if (this.footer.styles.body.length > 0)
    {
        this.setStyles(tfut, this.footer.styles.body);
    }
    var tr = document.createElement('tr');
    var td = document.createElement('td');
    td.colSpan = tbl.getElementsByTagName('th').length;
    td.style.width = '100%';
    td.style.height = '21px';
    td.appendChild(this.createFooterElements());
    tr.appendChild(td);
    tfut.appendChild(tr);
    tbl.appendChild(tfut);
}

gv.getTable = function()
{
    var tbl = document.getElementById(this.id);
    if (tbl == null & this.parent_id.constructor != String)
    {
        var tbls = this.parent_id.getElementsByTagName('table');
        tbl = tbls.item(0);
    }
    
    return tbl;
}

gv.getTBody = function(tbl, tbid)
{
    var found;
    try
    {
        var tbodi = tbl.getElementsByTagName('tbody'); //tbl.tBodies;
        for (var i = 0; i < tbodi.length; i++)
        {
            var bodi = tbodi.item(i);
            if (bodi.id == tbid)
            {
                found = bodi;
                break;
            }
        }
    }
    catch (e) {}
    
    return found;
}

gv.clearTable = function()
{
    var tbl = this.getTable();
    if (tbl == null) return;
    
    var tbid = this.id + '_tbody';
    var new_tbodi = this.getTBody(tbl, tbid);
    
    if (new_tbodi != null)
    {
        tbl.removeChild(new_tbodi);
        new_tbodi = null;
    }
    
    if (new_tbodi == null)
    {
        new_tbodi = document.createElement('tbody');
        new_tbodi.id = tbid;
        tbl.appendChild(new_tbodi);
    }
}

gv.createNobody = function()
{
    this.clearTable();
    
    var tbl = this.getTable();

    var ths = tbl.getElementsByTagName('th');
    var tr = document.createElement('tr');
    tr.id = this.id + '_NoBody';
    var td = document.createElement('td');
    td.colSpan = ths.length;
    if (this.norecord.styles.length > 0)
    {
        this.setStyles(td, this.norecord.styles);
    }
    setText(td, this.norecord.text); //td.innerText = this.norecord.text;
    tr.appendChild(td);
    var me = this;
    tr.onclick = function(evt)
	{
	    if (!evt) var evt = window.event;
        me.enableFooterBtns('SEL');	
        if (! me.subgrid) evt.cancelBubble = true; 
	};
    var tbodi = this.getTBody(this.getTable(), this.id + '_tbody');
    tbodi.appendChild(tr);
    
    this.enableFooterBtns('SEL');
}

gv.removeTable = function()
{
    var pobj = (this.parent_id.constructor == String) ? document.getElementById(this.parent_id) : this.parent_id;
    if (pobj == null) throw this.parent_id + ' not a document object';

    var tbl = this.getTable();
    if (tbl != null)
    {
        try 
        { 
            pobj.removeChild(tbl); 
        } 
        catch (e) 
        {
            alert("Remove Table: " + e.Description);
        }
    }
}

gv.createTable = function()
{
    this.removeTable();
    
    var pobj = (this.parent_id.constructor == String) ? document.getElementById(this.parent_id) : this.parent_id;
    var tbl = document.createElement('table');
    tbl.id = this.id;

    tbl.cellSpacing = this.cellspacing;
    tbl.cellPadding = this.cellpadding;
    if (this.classNames.table.length > 0)
    {
        tbl.className = this.classNames.table;
    }
    if (this.styles.table.length > 0)
    {
        this.setStyles(tbl, this.styles.table);
    }
    if (this.caption != null && this.caption.length > 0)
    {
        this.createCaption(tbl);
    }
    if (!this.noheader) this.createHeaders(tbl);
    if (!this.footer.none) this.createFooter(tbl);
    pobj.appendChild(tbl);
}

gv.createLink = function(col, i, grow, txt)
{
    var a = document.createElement('a');
    a.id = this.id + '_lnk_' + col.datafield + '_' + i;
    if (col.href != null || col.href.length > 0)
    {
        a.href = "javascript:" + col.href + "('" + i + "," + grow + "," + txt + "," + a.id + "');";
    }
    setText(a, txt);
    if (col.styles.datafield.length > 0) this.setStyles(a, col.styles.datafield);

    return a;
}

gv.createSelectOptions = function(sel, opt, val)
{
    if (opt.length > 0)
    {
        for (var i = 0; i < opt.length; i++)
        {
            var ss = opt[i].split('~');
            var opt_new = document.createElement('option');
            sel.options.add(opt_new);
            opt_new.value = ss[0];
            opt_new.selected = (val == ss[0]);
            setText(opt_new, ss[1]); 
        }
    }
}

gv.createChangeEvent = function(obj, evname, fun, i, grow, val)
{
    if (isIE)
    {
        obj.attachEvent(evname, function()
        {
            var valx = obj.value.replace(/["]/g, "");
            eval
            (
                'var arg = ' +
                '{' +
                    'datarow: ' + i + ', ' +
                    'gridrow: ' + grow + ', ' +
                    'oldvalue: "' + val + '", ' +
                    'newvalue: "' + valx + '", ' +
                    'control: "' + obj.id + '"' +
                '}; ' +
                fun + '( arg );'	            
            );
            event.cancelBubble = true;
        });
    }
    else
    {
        obj.addEventListener(evname, function(evt)
        {
            var valx = obj.value.replace(/["]/g, "");
            eval
            (
                'var arg = ' +
                '{' +
                    'datarow: ' + i + ', ' +
                    'gridrow: ' + grow + ', ' +
                    'oldvalue: "' + val + '", ' +
                    'newvalue: "' + valx + '", ' +
                    'control: "' + obj.id + '"' +
                '}; ' +
                fun + '( arg );'	            
            );
            evt.cancelBubble = true;        
        }, false);
    }
}

gv.createSelect = function(col, i, grow, val)
{
    var sel = document.createElement('select');
    sel.id = this.id + '_ddl_' + col.datafield + '_' + i;
    if (col.options != null) this.createSelectOptions(sel, col.options, val);
    sel.value = val;
    if (col.styles.datafield.length > 0) this.setStyles(sel, col.styles.datafield);
    sel.disabled = true;
	if (col.onchange.length > 0)
	{
	    this.createChangeEvent(sel, 'onchange', col.onchange, i, grow, val);
	} 
	if (col.onblur.length > 0)
	{
	    this.createChangeEvent(sel, 'onblur', col.onblur, i, grow, val);
	}  
    
    return sel;
}

gv.createCheck = function(td, col, i, grow, val)
{
    var s = (val) ? 'checked' : '';
    if (col.styles.datafield.length > 0)
    {
        s += ' "style="' + col.styles.datafield + '"'
    }
    s += ' DISABLED="true"';
    var shtm = '<input type=\"checkbox\" id=\"' + this.id + '_cb_' + col.datafield + '_' + i + '" ' + s;
    if (col.onclick.length > 0)
    {
        shtm += ' onclick=\"' + col.onclick + '(this);\"';
    }
    if (col.onblur.length > 0)
    {
        shtm += ' onblur=\"' + col.onblur + '(this);\"';
    }
    if (col.onchange.length > 0)
    {
        shtm += ' onchange=\"' + col.onchange + '(this);\"';
    }
    
    shtm += ' />';
    
    td.innerHTML = shtm;
}

gv.setCellsToEditMode = function(tr, read_only)
{
    var sells = tr.cells;
    for (var j = 0; j < sells.length; j++)
    {
        if (! this.columns[j].editable) continue;
        if (sells.item(j).hasChildNodes())
        {
            var childs = sells.item(j).childNodes;
            for (var k=0; k < childs.length; k++)
            {
                var typ = childs[k].type;
                var tag = childs[k].nodeName;
                if (tag == 'INPUT')
                {
                    if (typ == 'text' | typ == 'password') 
                    {
                        childs[k].readOnly = read_only; 
                    }
                    else if (typ == 'checkbox' | typ == 'button') 
                    {
                        childs[k].disabled = read_only;
                    }
                }
                else if (tag == 'SELECT' | tag == 'IMG')
                {
                    childs[k].disabled = read_only;
                } 
            }
        }
    }
}

gv.setRowToEditMode = function(i, grow, edit_mode)
{
    var trid = this.id + '_tr_' + i;
    var tr = document.getElementById(trid);
    if (! tr)
    {
        var bodi = document.getElementById(this.id + '_tbody');
        var rows = bodi.getElementsByTagName('tr');
        for (var j = 0; j < rows.length; j++)
        {
            tr = rows.item(j);
            if (tr.id == trid) break;
        }
    }
    if (tr)
    {
        this.setCellsToEditMode(tr, !edit_mode);
        this.row.selected = grow;
        this.row.selected_data_row = i;
        this.enableFooterBtns(this.curract);
    }
}

gv.getDataField = function(tid)
{
    var sid = tid.split('_');
    
    if (sid.length > 4)
    {
        var xid = '';
        for (var i = 2; i < (sid.length - 1); i++)
        {
            xid += sid[i];
            if (i < (sid.length - 2)) xid += '_';
        }
        return xid;
    }
    
    return sid[2];
}

gv.getUpdates = function(tr, i)
{
    var upobj = new datagrid_editrow();

    var sells = tr.cells;
    for (var j = 0; j < sells.length; j++)
    {
        if (sells.item(j).hasChildNodes())
        {
            var childs = sells.item(j).childNodes;
            for (var k=0; k < childs.length; k++)
            {
                var tag = childs[k].nodeName;
                var upcol = new datagrid_editcolumn();
                upcol.index = j;
                
                if ((tag == 'INPUT' & (childs[k].type == 'text' | childs[k].type == 'password' | childs[k].type == 'checkbox')) | tag == 'SELECT' | tag == 'A')
                {
                    var cid = this.columns[j].datafield; 
                    upcol.datafield = cid;
                    if (childs[k].type == 'checkbox')
                    {
                        upcol.value = childs[k].checked;
                    }
                    else if (tag == 'A')
                    {
                        upcol.value = getText(childs[k]); //childs[k].innerText;
                    }
                    else
                    {
                        upcol.value = childs[k].value;
                    }
                    if (this.curract == 'Edit')
                    {
                        if (i < this.datatable.Rows.length)
                        {
                            this.datatable.Rows[i][cid] = upcol.value;
                        }
                        else
                        {
                            var ndx = i - this.datatable.Rows.length;
                            this.newRows[ndx].columns[j].value = childs[k].value;
                        }
                    }
                }
                else
                {
                    upcol.datafield = '';
                    upcol.value = '';
                }
                upobj.addColumn(upcol);
            }
        }
    }
    
    return upobj;
}

gv.doPost = function(i, grow)
{
    var trid = this.id + '_tr_' + i;
    var tr = document.getElementById(trid);
    if (tr)
    {
        var upobj = this.getUpdates(tr, i);
        upobj.action = this.curract;
        upobj.datarow = i;
        upobj.gridrow = grow;
        if (upobj.action == 'Add') 
        {
            this.addNewRow(upobj);
            this.num_pages = this.getNumPages();
        }
        else if (upobj.action == 'Edit') this.addEditedRow(upobj);
    }
}

gv.checkEditButton = function(btnid, i, grow, val)
{
    var ebtn = (btnid.indexOf('_footerbtn_') > -1) ? document.getElementById(btnid) : document.getElementById(btnid + i);
    if (ebtn)
    {
        if (ebtn.value == 'Post') 
        {
            this.setRowToEditMode(i, grow, false);
            this.doPost(i, grow);
            ebtn.value = val;
            this.enableFooterBtns('SEL');
            this.curract = '';
        }
        else
        {
            this.curract = 'Edit';
            this.setRowToEditMode(i, grow, true);
            ebtn.value = 'Post';
        }
    }
}

gv.deleteRow = function(i, grow)
{
    if (confirm("Delete selected row?"))
    {
        var trid = this.id + '_tr_' + i;
        var tr = document.getElementById(trid);
        if (tr)
        {
            this.curract = 'Delete';
            var upobj = this.getUpdates(tr, i);
            upobj.datarow = i;
            upobj.gridrow = grow;
            upobj.action = this.curract;
            var tbl = document.getElementById(this.id + '_tbody');
            if (tbl) tbl.removeChild(tr);
            var deladded = false;
            if (i >= this.datatable.Rows.length)
            {
                var ndx = i - this.datatable.Rows.length;
                this.newRows[ndx] = null;
                deladded = true;
            }
            else
            {
                this.datatable.Rows[i] = null;
            }
            if (!deladded) this.addDeletedRow(upobj);
            this.enableFooterBtns('Cancel');
            this.curract = '';
            var j = i + 1;
            if (j > (this.datatable.Rows.length + this.newRows.length - this.deletedRows.length))
            {
                j = i - 1;
            }
            if (j >= 0)
            {
                tr = document.getElementById(this.id + '_tr_' + j);
                if (tr)
                {
                    tr.onclick();
                }        
            }    
        }
    }
}

gv.checkDeleteButton = function(btnid, i, grow, val)
{
    var ebtn = document.getElementById(btnid + i);
    if (ebtn)
    {
        this.deleteRow(i, grow);
    }
}

gv.gridButtonClick = function(fun, i, grow, btn_type, val, btn)
{
    var btnid = this.id + '_' + btn_type + '_' + val + '_';
    if (btnid == this.edit_button_prefix | btn.value == 'Post')
    {
        this.checkEditButton(btnid, i, grow, val);
    }
    else if (btnid == this.delete_button_prefix)
    {
        this.checkDeleteButton(btnid, i, grow, val);
    }
    else if (btnid == this.add_button_prefix)
    {
        this.doAddRow();
    }    
    if (fun.length > 0)
    {
	    eval
	    (
            'var arg = ' +
            '{' +
                'datarow: ' + i + ', ' +
                'gridrow: ' + grow + ', ' +
                'value: "' + val + '", ' +
                'control: "' + btn.id + '"' +
            '}; ' +
	        fun + '( arg );'
	    );
	}
}

gv.createTextBox = function(col, i, grow, val)
{
	var tb = document.createElement('input');
	tb.type = col.type;
	tb.id = this.id + '_txt_' + col.datafield + '_' + i;
	tb.readOnly = true;
	tb.value = val;
	if (col.styles.datafield.length > 0) this.setStyles(tb, col.styles.datafield + 'width:100%; border:0;background-color: transparent;');
	if (col.onchange.length > 0)
	{
	    this.createChangeEvent(tb, 'onchange', col.onchange, i, grow, val);
	}
	if (col.onblur.length > 0)
	{
	    this.createChangeEvent(tb, 'onblur', col.onblur, i, grow, val);
	}
	if (col.onkeydown.length > 0)
	{
	    this.createChangeEvent(tb, 'onkeydown', col.onkeydown, i, grow, val);
	}
	if (col.onkeyup.length > 0)
	{
	    this.createChangeEvent(tb, 'onkeyup', col.onkeyup, i, grow, val);
	}
	
	return tb;
}

gv.createDatagrid = function(col, i, grow, val)
{
    var colgrid;
    
    if (col.datagrid_create_function != null)
    {
        colgrid = eval
        (
            'var arg = ' +
            '{' +
                'datarow: ' + i + ', ' +
                'gridrow: ' + grow + ', ' +
                'parentid: "' + val.id + '", ' +
                'gridid: "' + val.id + '_gv"' +
            '}; ' +
            col.datagrid_create_function + '( arg );'	            
        );
    }
    
    if (colgrid != null)
    {
        colgrid.parent_id = val;
        colgrid.id = val.id + '_gv';
        colgrid.subgrid = true;
        
        if (this.datagrids == null) this.datagrids = new Array();
        this.datagrids[grow] = colgrid;
        
        if (col.datagrid_databind_function != null)
        {
            eval
            (
                'var arg = ' +
                '{' +
                    'datarow: ' + i + ', ' +
                    'grow: ' + grow + ', ' +
                    'parentid: "' + val.id + '", ' +
                    'gridid: "' + val.id + '_gv"' +
                '}; ' +
                col.datagrid_databind_function + '( arg );'	            
            );    
        }
    }
}

gv.createImage = function(col, i, grow, val)
{
    var sel = document.createElement('img');
    var s = (col.datafield.length > 0) ? col.datafield : col.value;
    sel.id = this.id + '_img_' + s + '_' + i;
    sel.name = sel.id;
    sel.src = col.src;
    sel.align = col.imgAlign;
    if (col.onclick.length > 0 | col.edit_button | col.delete_button | col.add_button)
    {
        var me = this;
        
    	sel.onclick = function(evt)
    	{
    	    if (!evt) var evt = window.event;
    		me.gridButtonClick(col.onclick, i, grow, 'img', col.value, sel);
    		evt.cancelBubble = true;
    	};
    }
    if (col.styles.datafield.length > 0) this.setStyles(sel, col.styles.datafield);
    sel.disabled = true;
    if (col.edit_button) this.edit_button_prefix = this.id + '_btn_' + s + '_';
    else if (col.delete_button) this.delete_button_prefix = this.id + '_btn_' + s + '_';
    else if (col.add_button) this.add_button_prefix = this.id + '_btn_' + s + '_';    
    return sel;
}

gv.createButton = function(col, i, grow, val)
{
    var sel = document.createElement('input');
    sel.type = 'button';
    var s = (col.datafield.length > 0) ? col.datafield : col.value;
    sel.id = this.id + '_btn_' + s + '_' + i;
    sel.name = sel.id;
    sel.value = (val == null || val.length == 0) ? col.value : val;
    if (col.onclick.length > 0 | col.edit_button | col.delete_button | col.add_button)
    {
        var me = this;
    	sel.onclick = function(evt)
    	{
    	    if (!evt) var evt = window.event;
    		me.gridButtonClick(col.onclick, i, grow, 'btn', col.value, sel);
    		evt.cancelBubble = true;
    	};
    }
    if (col.styles.datafield.length > 0) this.setStyles(sel, col.styles.datafield);
    sel.disabled = true;
    if (col.edit_button) this.edit_button_prefix = this.id + '_btn_' + s + '_';
    else if (col.delete_button) this.delete_button_prefix = this.id + '_btn_' + s + '_';
    else if (col.add_button) this.add_button_prefix = this.id + '_btn_' + s + '_';    
    return sel;
}

gv.doRowButtonsDisabled = function(btns, val)
{
    for (var i = 0; i < btns.length; i++)
    {
        var btn = btns.item(i);
        if (btn.type && (btn.type == 'text' | btn.type == 'password' | btn.type == 'checkbox')) continue;
        
        var bs = this.getDataField(btn.id); 
        var be = this.getDataField(this.edit_button_prefix);
        var ba = this.getDataField(this.add_button_prefix);
        var bd = this.getDataField(this.delete_button_prefix);
        if ((bs == be | bs == ba | bs == bd) & val == false)
            btn.disabled = val;
        else if (!val & (this.curract == 'Add' | this.curract == 'Edit'))
        {
            btn.disabled = val;
        }
        else if (val)
        {
            btn.disabled = val;
        }
    }
}

gv.rowImageButtonsDisabled = function(tr, val)
{
    var btns = tr.getElementsByTagName('image');
    this.doRowButtonsDisabled(btns, val);
}

gv.rowButtonsDisabled = function(tr, val)
{
    var btns = tr.getElementsByTagName('input');
    this.doRowButtonsDisabled(btns, val);
}

gv.gridsButtonsInit = function(tr, i, grow)
{
    var grids = tr.getElementsByTagName('table');
    if (grids.length > 0 & (this.datagrids != null && this.datagrids.length > 0))
    {
        var tempgrid = this.datagrids[grow];
        if (tempgrid)
        {
            tempgrid.enableFooterBtns('SEL');
        }
    }
}

gv.rowSelected = function(i, grow, val)
{
	if (this.row.selected_data_row > -1)
	{
	    var tr_prev = document.getElementById(this.id + '_tr_' + this.row.selected_data_row);
	    if (tr_prev)
	    {
		    tr_prev.style.cssText = this.row.normal_style;
		    this.rowButtonsDisabled(tr_prev, true);
		    this.rowImageButtonsDisabled(tr_prev, true);
        }
	}
	var trid = this.id + '_tr_' + i;
	var tr = document.getElementById(trid)
	tr.style.cssText = this.row.selected_style;
	this.rowButtonsDisabled(tr, false);
	this.rowImageButtonsDisabled(tr, false);
	this.gridsButtonsInit(tr, i, grow);
	this.row.selected = grow;
	this.row.selected_data_row = i;
	if (this.curract == 'Edit' | this.curract == 'Add') {} else this.enableFooterBtns('SEL');
	if (this.row.onclick_enabled && this.row.onclick.length > 0) 
	{
	    eval 
	    (
            'var arg = ' +
            '{' +
                'datarow: ' + i + ', ' +
                'gridrow: ' + grow + ', ' +
                'value: "' + this.id + '_tr_' + i + '" ' +
            '}; ' +
	        this.row.onclick + '( arg );'
	    );
	}
}

gv.createDataRow = function(i, grow)
{
    var tr = null;
    if (this.addOnTop)
    {
        var tbodi = document.getElementById(this.id + '_tbody');
        tr = tbodi.insertRow(0);
        if (tbodi.rows.length > this.rows_page)
        {
            var ltr = tbodi.rows.item(tbodi.rows.length - 1);
            tbodi.removeChild(ltr);
        }
    }
    else
    {
	    tr = document.createElement('tr');
	}
	tr.id = this.id + '_tr_' + i;
	if (this.row.onclick.length > 0)
	{
	    tr.style.cursor = 'hand';
	}
	var me = this;
	
	tr.onclick = function(evt)
    {
        if (!evt) var evt = window.event;

        if (! me.row.onclick_enabled) 
        { 
            evt.cancelBubble = true; 
            return; 
        }
        if ((me.curract == 'Edit' | me.curract == 'Add') & me.row.selected != parseInt(grow))
        {
            var post = (me.autoPost) ? true : confirm('Post changes?');
            if (post)
            {
                var save_act = me.curract;

                if (me.curract == 'Add' & me.footer.add_button.beforepost.length > 0)
                {
                    me.callOnPost(me.footer.add_button.beforepost);
                }
                else if (me.curract == 'Edit' & me.footer.edit_button.beforepost.length > 0)
                {
                    me.callOnPost(me.footer.edit_button.beforepost);
                }
                if (me.curract == '') 
                {
                    me.curract = save_act; 
                }
                else
                {
                    var btnid = (me.curract == 'Add') ? me.id + '_footerbtn_' + me.footer.add_button.value : me.id + '_footerbtn_' + me.footer.edit_button.value;
                    var btnval = (me.curract == 'Add') ? me.footer.add_button.value : me.footer.edit_button.value;
                    me.checkEditButton(btnid, me.row.selected_data_row, me.row.selected, btnval);

                    if (save_act == 'Add' & me.footer.add_button.onpost.length > 0)
                    {
                        if (me.postToServer)
                        {
                            me.doSave(me.footer.add_button.onpost);
                            me.clearChanges();
                        }
                        else
                        {
                            me.callOnPost(me.footer.add_button.onpost);
                        }
                    }
            	    
                    if (save_act == 'Edit' & me.footer.edit_button.onpost.length > 0)
                    {
                        if (me.postToServer)
                        {
                            me.doSave(me.footer.edit_button.onpost);
                            me.clearChanges();
                        }
                        else
                        {
                            me.callOnPost(me.footer.edit_button.onpost);
                        }
                    }
                    save_act = '';
                }        
            }
            else
            {
                return;
            }
        }
        if (!me.subgrid) 
            evt.cancelBubble = true;		    
        if (me.row.selected == -1 | ((me.row.selected != parseInt(grow) | me.row.selected == 0) & me.curract == '')) me.rowSelected(i, grow, tr.id);
    };
		    
	return tr;
}

gv.createCell = function(col, i, grow, val)
{
    var td = document.createElement('td');
    td.id = this.id + '_td_' + col.datafield + '_' + i
    td.align = col.align;
    var s = col.type.toLowerCase();
    var obj = null;
    if (s == 'link')
    {
        obj = this.createLink(col, i, grow, val);
    }
    else if (s == 'select')
    {
        obj = this.createSelect(col, i, grow, val);
    }
    else if (s == 'check')
    {
        this.createCheck(td, col, i, grow, val);
    }
    else if (s == 'button')
    {
        obj = this.createButton(col, i, grow, val);
    }
    else if (s == 'image')
    {
        obj = this.createImage(col, i, grow, val);
    }
    else if (s == 'datagrid')
    {
		this.createDatagrid(col, i, grow, td);
    }
    else
    {
        obj = this.createTextBox(col, i, grow, val);
    }
    
    if (obj != null) 
    {
        td.appendChild(obj);
        if (col.oninit.length > 0)
        {
            //window.execScript
            eval
            (
                'var arg = ' +
                '{' +
                    'datarow: ' + i + ', ' +
                    'gridrow: ' + grow + ', ' +
                    'value: ' + obj.value + ', ' +
                    'control: "' + obj.id + '"' +
                '}; ' +
                col.oninit + '( arg );'
            );            
        }
    }
    
    if (col.styles.cell.length > 0)
    {
        this.setStyles(td, col.styles.cell);
    }    
    
    return td;    
}

gv.isFiltered = function(row)
{
    if (! this.filter.enabled) return true;
    if (this.filter.filters.length < 1) return true;
    
    var estr = '';
    if (this.filter.operators.length > 0) estr += '(';
    for (var i = 0; i < this.filter.filters.length; i++)
    {
        estr += '(row.' + this.filter.filters[i].datafield;
        estr += ' ' + this.filter.filters[i].operator;
        estr += ' ' + this.filter.filters[i].value + ')';
        
        if (this.filter.operators.length > i)
        {
            estr += ' ' + this.filter.operators[i] + ' ';
        }
    }
    if (this.filter.operators.length > 0) estr += ')';
    estr += ';';
    
    var rc = eval(estr);
    
    return rc;
}

gv.renderRows = function(i, k)
{
    var ndx = i;
    if (i < this.datatable.Rows.length)
    {
        if (this.datatable.Rows[i] == null) return false;
    }
    else if (i >= this.datatable.Rows.length)
    {
        ndx = i - this.datatable.Rows.length;
        if (this.newRows[ndx] == null) return false;
    }
    
    var src = (i < this.datatable.Rows.length) ? this.datatable.Rows[ndx] : this.newRows[ndx];
    
    var grow = Math.abs(i - this.curr_row);
    var tr = this.createDataRow(i, grow);

    for (var j = 0; j < this.columns.length; j++)
    {
        var vtype;
        try
        {
            vtype = (this.columns[j].datafield.length > 0) ? (i < this.datatable.Rows.length) ? src[this.columns[j].datafield].constructor : src.columns[j].value.constructor : '';
        }
        catch (e) 
        {
            vtype = 'String';
        }
        
        var val = (this.columns[j].datafield.length > 0) ? (i < this.datatable.Rows.length) ? src[this.columns[j].datafield] : src.columns[j].value : '';
        var xval;
        if (val == null) xval = '';
        if (this.columns[j].formatter.length > 0)
        {
            xval = eval ( this.columns[j].formatter + '(' + val + ');');
        }
        else
        {
            if (vtype == Date)
            {
                xval = dateToString(val);
            }
            else xval = val;
        }
        var td = this.createCell(this.columns[j], i, grow, xval);
        tr.appendChild(td);
    }
    var bodi = this.getTBody(this.getTable(), this.id + '_tbody');
    bodi.appendChild(tr);
    
    return true;
}

gv.createRowsAscending = function()
{
    var i = this.curr_row;
    var k = i;
    for (; k < (this.curr_row + this.rows_page) & i < this.dataRowsSize(); i++)
    {
        if (!this.renderRows(i, k)) continue;
        k++;
    }
}

gv.createRowsDescending = function()
{
    var i = (this.curr_row == 0) ? this.dataRowsSize() - 1 : this.curr_row;
    var k = i;
    
    for (; k > (this.curr_row - this.rows_page) & i >= 0; i--)
    {
        if (!this.renderRows(i, k)) continue;
        k--;
    }
}

gv.createRows = function()
{
    this.clearTable();
    this.datagrids = null;
    
    if (this.addOnTop) // & this.addOnTopRerender)
    {
        this.createRowsDescending();
    }
    else
    {
        this.createRowsAscending();
    }

    try
    {
        this.enableFooterBtns('Init');
    }
    catch (e) {}
}

gv.getDataRowsCount = function()
{
    return this.datatable.Rows.length + this.newRows.length - this.deletedRows.length;
}

gv.getNumPages = function()
{
	var npg = (this.datatable.Rows.length + this.newRows.length - this.deletedRows.length) / this.rows_page;
	if (npg < 1) return 1;

	var rpg = Math.round(npg);
	
	if ((npg - rpg) > 0) return rpg + 1;

	return rpg;
}

gv.clearChanges = function()
{
    this.newRows = new Array();
    this.editRows = new Array();
    this.deletedRows = new Array();
}

gv.hasChanges = function()
{
    return (
        (this.newRows != null && this.newRows.length > 0) | 
        (this.editRows!= null && this.editRows.length > 0) | 
        (this.deleteRows != null && this.deleteRows.length > 0) 
    );
}

gv.doFilter = function(dt)
{
    var datatable =
    {
        Rows: new Array()
    };
    
    for (var i = 0; i < dt.Rows.length; i++)
    {
        if (this.isFiltered(dt.Rows[i]))
        {
            datatable.Rows.push(dt.Rows[i]);
        }
    }
    
    return datatable;
}

gv.bind = function(datatable, parent_id, id)
{
    this.datatable = (this.filter.enabled) ? this.doFilter(datatable) : datatable;
    if (parent_id != null) this.parent_id = parent_id;
    if (id != null) this.id = id;
    this.clearChanges();
    this.num_pages = this.getNumPages();
    this.curr_row = (this.addOnTop) ? this.dataRowsSize() - 1 : 0;
    this.curr_page = 1;
    this.curract = '';

    this.createTable();

    if (this.datatable.Rows.length == 0)
    {
        this.createNobody();
        return;
    }
    this.createRows();
}

gv.unbound = function()
{
    try
    {
        this.datatable = null;  
    }
    catch (e)
    {}  
}

gv.refresh = function(datatable)
{
    this.datatable = (this.filter.enabled) ? this.doFilter(datatable) : datatable;
    this.num_pages = this.getNumPages();
    this.curr_row = (this.addOnTop) ? this.dataRowsSize() - 1 : 0;
    this.curr_page = 1;
    
    this.createTable();

    if (this.datatable.Rows.length == 0)
    {
        this.createNobody();
        return;
    }
    this.createRows();
}

gv.newRows_colIndex = function(colname)
{
    if (this.newRows.length > 0)
    {
        for (var i = 0; i < this.newRows[0].columns.length; i++)
        {
            if (colname == this.newRows[0].columns[i].datafield)
            {
                return i;
            }
        }
    }
    
    return -1;
}

gv.find = function(args, show)
{
    if (args.constructor == datagrid_find_arguments)
    {
        var locrow = -1;
        var newrow = false;
        var i = args.start_row;
        var fin = (args.end_row == -1) ? this.datatable.Rows.length + this.newRows.length : args.end_row;
        for (; i < fin; i++)
        {
            var ndx = i;
            if (i >= this.datatable.Rows.length) 
            {
                ndx = i - this.datatable.Rows.length;
                newrow = true;
            }
            var row = (i < this.datatable.Rows.length) ? this.datatable.Rows[ndx] : this.newRows[ndx];
            var estring = args.toEvalString(this, 'row', newrow);
            var rc = eval(estring);
            if (rc) 
            {
                locrow = i;
                break;
            }
        }
        if (locrow > -1)
        {
            if (!show) return locrow;
            var pgx = ((locrow + 1) > this.rows_page) ? (locrow + 1) / this.rows_page : 0;
            var pgno = parseInt(pgx);
            if ((pgx - pgno) > 0 | pgno == 0) pgno++;
            this.pg_position(pgno);
            var grow = (locrow - ((pgno * this.rows_page) - this.rows_page));
            if (grow > -1 && args.select_row) this.rowSelected(locrow, grow);
            else if (grow > -1)
            {
                document.getElementById(this.id + '_tr_' + locrow).style.cssText = this.row.selected_style;;
            }
            return locrow;
        }
    }
    return -1;
}

gv.getGridRowIndex = function(datarow)
{
    var pgno = ((datarow + 1) > this.rows_page) ? parseInt((datarow + 1) / this.rows_page) : 0;
    var grow = (datarow - (pgno * this.rows_page));
    return grow;
}

function datagrid_find_arguments()
{
    this.start_row = 0;
    this.end_row = -1;
    this.select_row = true;
    this.arguments = new Array();
    this.conjunctions = new Array();
}

datagrid_find_arguments.prototype.addArgument = function(args)
{
    this.arguments.push(args);
}

datagrid_find_arguments.prototype.addConjunction = function(oper)
{
    this.conjunctions.push(oper);
}

datagrid_find_arguments.prototype.toEvalString = function(gv, row_name, newrow)
{

    var evalstring = '';
    if (this.conjunctions.length > 0) evalstring += '(';
    for (var i = 0; i < this.arguments.length; i++)
    {
        if (newrow)
        {
            evalstring += '(' + row_name + '.';
            evalstring += 'columns[' + gv.newRows_colIndex(this.arguments[i].column_name) + '].value != null && '
            evalstring += row_name + '.';
            evalstring += 'columns[' + gv.newRows_colIndex(this.arguments[i].column_name) + '].value';
        }
        else
        {
            evalstring += '(' + row_name + '.';
            evalstring +=  this.arguments[i].column_name + ' != null && ';
            evalstring += row_name + '.';
            evalstring +=  this.arguments[i].column_name;
        }
        if (this.arguments[i].operator == 'search')
        {
            evalstring += ".toString().search(/" + this.arguments[i].value + "/i) > -1"
        }
        else
        {
            evalstring += ' ' + this.arguments[i].operator;
            evalstring += ' ' + this.arguments[i].value;
        }
        evalstring += ')';
        
        if (this.conjunctions.length > i)
        {
            evalstring += ' ' + this.conjunctions[i] + ' ';
        }
    }
    if (this.conjunctions.length > 0) evalstring += ')';
    if (evalstring.length > 0) evalstring += ';';
    
    return evalstring;
}

function datagrid_find_argument()
{
    this.column_name = '';
    this.operator = '==';
    this.value = null;
}
