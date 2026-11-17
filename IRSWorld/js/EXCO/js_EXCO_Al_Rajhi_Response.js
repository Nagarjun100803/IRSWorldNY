var js_Al_Rajhi = new js_EXCO_Al_Rajhi_Response();
var dbgrid;
var edit_mode = false;
var status_opts = new Array();


function update_taropts()
{
    var status = IRSWorld.EXCO.EXCO_Al_Rajhi_Response.get_status();
    if (status.error != null)
    {
        alert(status.error.Message);
    }
    else
    {
        dt = status.value;
        for (var i = 0; i < dt.Rows.length; i++)
        {
            status_opts[status_opts.length] = dt.Rows[i].FldCode + '~' + dt.Rows[i].FldDescription;
        }
    }
}

if (!Array.prototype.indexOf)
{
  Array.prototype.indexOf = function(elt /*, from*/)
  {
    var len = this.length;

    var from = Number(arguments[1]) || 0;
    from = (from < 0)
         ? Math.ceil(from)
         : Math.floor(from);
    if (from < 0)
      from += len;

    for (; from < len; from++)
    {
      if (from in this &&
          this[from] === elt)
        return from;
    }
    return -1;
  };
}

function js_EXCO_Al_Rajhi_Response()
{
    this.init = function()
    {
        update_taropts();        
        this.pop_view();
        edit_mode = false;
        this.set_mode();
        document.getElementById('tdEXCOfilelisting').style.display = 'none';
        document.getElementById('Button1').style.display = 'none';
    }

    this.display_file_listing = function(Filename, FileDate, ViewCode)
    {
        
        document.getElementById('tdEXCOfilelisting').style.display = 'block';
        
        AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. DISPLAYING OF FILES FAILED.';
            alert(errmsg);             
        } 

        IRSWorld.EXCO.EXCO_Al_Rajhi_Response.get_transactions(Filename, FileDate, ViewCode, this.display_file_listing_callback)
    }
    this.display_file_listing_callback = function(result)
    {
        if (result.error != null)
        {
            alert(result.error.Message)
            return;
        }
        dbgrid = js_Al_Rajhi.create_filelist_grid()
        dbgrid.clearFilter();                    
        dbgrid.filter.enabled = false;
        dbgrid.rows_page = 5;
        dbgrid.on_page_change = 'js_Al_Rajhi.on_page_change';
        dbgrid.autoPost = true;
        dbgrid.bind(result.value, 'tdEXCOfilelisting', 'tblEXCOfilelisting');
  
        js_Al_Rajhi.enableRow(edit_mode);
    }
    
    this.create_filelist_grid = function()
    {
         //==================================================
         // Col No 0; SEQ_ID
         //==================================================
         gv = new datagrid();             
         {
		    var col = new datagrid_column();
		    col.align = 'left';
		    col.header = 'SEQ ID';
		    col.datafield = 'SEQ_ID';
            col.styles.cell += 'width:auto;display:none';
		    col.styles.header += 'display:none';
		    col.styles.datafield += 'display:none';
		    
		    gv.addColumn(col);
         }       
         //==================================================
         // Col No 1; IRSWRefNo
         //==================================================        
         {
		    var col = new datagrid_column();
		    col.header = 'IRSWRefNo';
		    col.datafield = 'IRSWRefNo';
		    col.align = 'right';
		    col.styles.cell += 'width:auto';
		    col.editable = false;
		    gv.addColumn(col);
         }
         //==================================================
         // Col No 2; PON
         //==================================================           
         {
		    var col = new datagrid_column();
		    col.header = 'PON';
		    col.datafield = 'PON';
		    col.align = 'right';
		    col.styles.cell += 'width:auto';
		    col.editable = false;
		    gv.addColumn(col);
         }
         //==================================================
         // Col No 3; TranxStatus
         //==================================================           
         {
            var col = new datagrid_column();
            col.header = 'TranxStatus';
            col.datafield = 'TranxStatus';
            col.align = 'left';
            col.type = 'select';
            col.options = status_opts;
            col.styles.cell += 'width:15%';
            col.styles.datafield += 'width: 100%;';
            col.editable = false;
            col.onchange = 'js_Al_Rajhi.on_status_change';
            gv.addColumn(col);
         }
         //==================================================
         // Col No 4; Remitter_Name
         //==================================================     
         {
		    var col = new datagrid_column();
		    col.header = 'Remitter_Name';
		    col.datafield = 'Remitter_Name';
		    col.align = 'right';
		    col.styles.cell += 'width:auto';
		    col.editable = false;
		    gv.addColumn(col);
         } 
         //==================================================
         // Col No 5; Beneficiary_Name
         //==================================================             
         {
		    var col = new datagrid_column();
		    col.header = 'Beneficiary_Name';
		    col.datafield = 'Beneficiary_Name';
		    col.align = 'right';
		    col.styles.cell += 'width:auto';
		    col.editable = false;
		    gv.addColumn(col);
         } 
         //==================================================
         // Col No 6; Pay_Method
         //==================================================             
         {
		    var col = new datagrid_column();
		    col.header = 'Pay_Method';
		    col.datafield = 'Pay_Method';
		    col.align = 'right';
		    col.styles.cell += 'width:auto';
		    col.editable = false;
		    gv.addColumn(col);
         } 
//         //==================================================
//         // Col No 7; Prod_Code
//         //==================================================             
//         {
//		    var col = new datagrid_column();
//		    col.header = 'Prod_Code';
//		    col.datafield = 'Prod_Code';
//		    col.align = 'right';
//		    col.styles.cell += 'width:auto';
//		    col.editable = false;
//		    col.styles.header += 'display:none';
//		    col.styles.datafield += 'display:none';		    
//		    gv.addColumn(col);
//         }          
         //==================================================
         // Col No 7; Account_Number
         //==================================================             
         {
		    var col = new datagrid_column();
		    col.header = 'Account_Number';
		    col.datafield = 'Account_Number';
		    col.align = 'right';
		    col.styles.cell += 'width:auto';
		    col.editable = false;
		    gv.addColumn(col);
         } 
         //==================================================
         // Col No 8; Amount
         //==================================================             
         {
		    var col = new datagrid_column();
		    col.header = 'Amount';
		    col.datafield = 'Amount';
		    col.align = 'right';
		    col.styles.cell += 'width:auto';
		    col.editable = false;
		    gv.addColumn(col);
        
         }
         //==================================================
         // Col No 9; Currency
         //==================================================             
         {
		    var col = new datagrid_column();
		    col.header = 'Currency';
		    col.datafield = 'Currency';
		    col.align = 'right';
		    col.styles.cell += 'width:auto';
		    col.editable = false;
		    gv.addColumn(col);
         }                                                                    
                  
        gv.row.normal_style = 'background-color: #f7ffff;';                      
        return gv;
    }
   
    this.update_callback = function(res)
    {
        if (res.error != null)
        {
            alert(res.error.Message);
        }
        else
        {
            alert('update successfull');
        }
    }
    
    this.on_page_change = function()
    {   
        this.enableRow(edit_mode);
    }
    
    this.get_updated_rows_values = function()
    {
        var EditedRows = dbgrid.editRows;
        var params = "";
        var rows = new Array();
        // Get Edited Rows
        for (var i = 0; i < EditedRows.length; i++)
        {
            if (rows.indexOf(EditedRows[i].datarow) < 0)
            {
                rows[rows.length] = EditedRows[i].datarow;
            }
        }
        
        for (var j = 0; j < rows.length; j++)
        {
            params = params + dbgrid.getDataAt(rows[j],0) + '~' + dbgrid.getDataAt(rows[j],3) + '~' + dbgrid.getDataAt(rows[j],2) + '|';  
        }       
        return params
    }

    this.on_status_change = function(args)
    {
        dbgrid.curract = 'Edit';
        a = args;    
        dbgrid.doPost(a.datarow, false);
        dbgrid.curract = '';
    }
    
    this.FileDate_on_change = function()
    {
        document.getElementById('ddlView').value = '';
        document.getElementById('tdEXCOfilelisting').style.display = 'none';
            
        this.pop_filelist(document.getElementById('txtFileDate').value);
    }
    
    this.pop_filelist = function(Date)
    {
        var result = IRSWorld.EXCO.EXCO_Al_Rajhi_Response.get_filelist(Date);
        if (result.error != null)
        {
            alert(result.error.Message);
        }
        else
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddlFileList",  result.value, "FldCode", "FldDescription");
            this.commons = null;            
        }
    }
    
    this.pop_view = function()
    {
        var result = IRSWorld.EXCO.EXCO_Al_Rajhi_Response.get_viewlist();
        if (result.error != null)
        {
            alert(result.error.Message);
        }
        else
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddlView",  result.value, "FldCode", "FldDescription");
            this.commons = null;            
        }        
    }
    
    this.view_onchange = function()
    {
        var filename = document.getElementById('ddlFileList').value;
        var filedate = document.getElementById('txtFileDate').value;
        var viewcode = document.getElementById('ddlView').value;
    
        if (filename != 'null' && filedate != 'null' && viewcode != 'null' && filename != '' && filedate != '' && viewcode != '')
        {
            this.display_file_listing(filename, filedate, viewcode);
        }
            
        edit_mode = false;
        this.set_mode();
    }
    
    this.ddlfilelist_onchange = function()
    {
        document.getElementById('ddlView').value = '';
        document.getElementById('tdEXCOfilelisting').style.display = 'none';
    }
    
    this.btnSave_click = function()
    {
       if (edit_mode == false)
       {
            edit_mode = true;
            this.set_mode();
       }
       else
       {
            var param = this.get_updated_rows_values();
            if(param == null | param == '' | param == 'null')
            {
                alert('No changes has been made.')
                return;
            }
                        
            var result = IRSWorld.EXCO.EXCO_Al_Rajhi_Response.update_status(param);
            
            if (result.error != null)
            {
                alert(result.error.Message);
            }
            else
            {
                edit_mode = false;
                this.set_mode();
                
                var filename = document.getElementById('ddlFileList').value;
                var filedate = document.getElementById('txtFileDate').value;
                var viewcode = document.getElementById('ddlView').value;                
                
                if (filename != 'null' && filedate != 'null' && viewcode != 'null' && filename != '' && filedate != '' && viewcode != '')
                {
                    this.display_file_listing(filename, filedate, viewcode);
                }                
                alert('Status where sucessfuly updated.')               
            }
       }
       
       this.enableRow(edit_mode)
    }
    
    this.btnCancel_onclick = function()
    {
        var filename = document.getElementById('ddlFileList').value;
        var filedate = document.getElementById('txtFileDate').value;
        var viewcode = document.getElementById('ddlView').value;
    
        if (filename != 'null' && filedate != 'null' && viewcode != 'null' && filename != '' && filedate != '' && viewcode != '')
        {
            this.display_file_listing(filename, filedate, viewcode);
        }
        
        edit_mode = false;
        this.set_mode();
    }
    
    this.set_mode = function()
    {
        
        if (edit_mode == true)
        {
            document.getElementById('calFileDate').disabled = true;
            document.getElementById('ddlFileList').disabled = true;
            document.getElementById('ddlView').disabled = true;
            document.getElementById('btnCancel').disabled = false;
            document.getElementById('btnGenerate').disabled = true;

            if (document.getElementById('ddlView').value == '01')
            {
                document.getElementById('btnSave').disabled = false;
            }
            else
            {
                document.getElementById('btnSave').disabled = true;
            }
            
            setText('btnSave','Save');
            
        }
        else
        {
            document.getElementById('calFileDate').disabled = false;
            document.getElementById('ddlFileList').disabled = false;
            document.getElementById('ddlView').disabled = false;
            document.getElementById('btnCancel').disabled = true;
            document.getElementById('btnGenerate').disabled = false;

            if (document.getElementById('ddlView').value == '01')
            {
                document.getElementById('btnSave').disabled = false;
            }
            else
            {
                document.getElementById('btnSave').disabled = true;
            }
            
            setText('btnSave','Edit');            
        }
    }
    
    this.btnGenerate_onclick = function()
    {
        document.getElementById('HPath').value = '';
        var filename = document.getElementById('ddlFileList').value;
        
        if (filename != null && filename != 'null' && filename != "")
        {
            document.getElementById('HPath').value = filename;
            document.getElementById('Button1').click();
        }
        else
        {
            alert('No File Selected');
        }
    }
    
    this.enableRow = function(enable)
    {
        var tabid =  document.getElementById('tblEXCOfilelisting');
        var read_only = !enable;
        
        var tblrows = tabid.rows;
        if (tblrows == undefined) return;
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
                        if (childs[k].nodeName == 'SELECT' | childs[k].nodeName == 'img' )
                        {
                            childs[k].disabled = read_only;
                        }               
                    }
                }
            }
        }
    }
}



