var testgrid;
var myfilename;
var codeopts = new Array();
var gridtype;
var searchfilter;

function Get_codeopts()
{
    var rows =  IRSWorld.RITS.POLINK.getCodeOpts('POlink_status').value.Rows;
    codeopts = new Array();
    for(var i = 0;i <= rows.length - 1;i++)
    {
        codeopts[codeopts.length] = rows[i].FLDVALUE + '~' + rows[i].FLDDESCRIPTION;
    }
}
function Get_codeopts_va()
{
    var rows =  IRSWorld.RITS.POLINK.getCodeOpts('VA_Status').value.Rows;
    codeopts = new Array();
    for(var i = 0;i <= rows.length - 1;i++)
    {
        codeopts[codeopts.length] = rows[i].FLDVALUE + '~' + rows[i].FLDDESCRIPTION;
    }
}
function display_my_name()
{
    IRSWorld.RITS.POLINK.MyDigit(display_my_name_async);
    document.getElementById('Button1').value = 'onProcess';
  
}
function txt_preloader(result)
{
    
    if(result.value != false)
    {
        document.getElementById('container').innerHTML = ""; 
        alert('Process Successful');
        
    }
    else
    {
        document.getElementById('container').innerHTML = ""; 
        alert('No Records to Export. Please Try Again');
    }
}
function grid_preloader(result)
{

}
function preloader(result)
{
    
    if(result.value != false)
    {
         document.getElementById('container').innerHTML = ""; 
       
         if ( document.getElementById('dd1').value == "PO-Link")
            {
            document.getElementById('button4').disabled = false;            
            
            IRSWorld.RITS.POLINK.get_value_table(bind_grid);
            }else{
            document.getElementById('button4').disabled = false;            
            IRSWorld.RITS.POLINK.get_value_table(bind_grid_virtual);
            }
    }
    else
    {
    document.getElementById('container').innerHTML = ""; 
        if ( document.getElementById('dd1').value == "PO-Link")
        {
            alert('Invalid PO-Link txt file');
        }else{
            alert('Invalid Virtual Access txt file');
        }
    }
}


function getString()
{
    var x = IRSWorld.RITS.POLINK.returnString().value;
    alert(x);
}

function upload_file(temp1)
{
    
    if (temp1==null)
    {
    alert("Please Specify File")
    }else{
    document.getElementById('button2').disabled = true; 
    document.getElementById('Button1').disabled = false;
    document.getElementById('Button3').disabled = false; 
     myfilepath = temp1;
    myfilename = myfilepath.match(/[^\/\\]+$/);
   }
}
function clear_upload()
{
 
    var fil = document.getElementById("FileUpload1");
    fil.select();
    n=fil.createTextRange();
    n.execCommand('delete');
    fil.focus();
    document.getElementById('dd1').disabled = false;
    document.getElementById('Button1').disabled = true;
    document.getElementById('Button3').disabled = true; 
}

function populate_rec()
{
    document.getElementById('button2').disabled = true; 
    container.style.visibility = "visible";
    document.getElementById('container').innerHTML = "<img src='../images/ajaxloader.gif' />" + "   Loading Data"; 
    gridtype = 'upload';
if ( document.getElementById('dd1').value == "PO-Link")
{
    Get_codeopts();
    AjaxPro.timeoutPeriod = 1000 * 1000;
    AjaxPro.onTimeout = function()
{
    alert("THE SERVER STOPPED RESPONDING. PLEASE TRY AGAIN");
    
}

    IRSWorld.RITS.POLINK.txt_parser(preloader);
}else{
    Get_codeopts_va();
    AjaxPro.timeoutPeriod = 1000 * 1000;
    AjaxPro.onTimeout = function()
    {
        alert("THE SERVER STOPPED RESPONDING. PLEASE TRY AGAIN");
    }

    IRSWorld.RITS.POLINK.txt_parser_virtual(preloader);
}
    document.getElementById('dd1').disabled = false;
}
function generate_txt(gentype)
{
    document.getElementById('container').innerHTML = "<img src='../images/ajaxloader.gif' />" + "   Loading Data"; 
    IRSWorld.RITS.POLINK.generate_txt(txt_preloader);
    if (gentype == "POLINK")
    {
    show_grid()
    }else{
    show_grid_virtual()
    }
}
function load_preloader()
{
    document.getElementById('imgcontainer').innerHTML = "<img src='../images/ajaxloader.gif' />";  
}
function disable_process()
{
    document.getElementById('dd1').disabled = true;
    document.getElementById('button2').disabled = false; 
    document.getElementById('Button1').disabled = true; 
    document.getElementById('Button3').disabled = true; 
}
function bind_grid(result)
{
    if (result.error != null)
    {
        spawn_errpage(result.error.Message);
        return;
    }
    create_grid();
    document.getElementById('container').innerHTML = ""; 
    dbgrid.bind(result.value, 'container','testid');
       
}
function bind_grid_virtual(result)
{
    if (result.error != null)
    {
        spawn_errpage(result.error.Message);
        return;
    }
    create_grid_virtual();
    document.getElementById('container').innerHTML = ""; 
    dbgrid.bind(result.value, 'container','testid');
       
}

function setstring(str)
{
    IRSWorld.RITS.POLINK.getfilename(document.getElementById('File1').value);
}
function save_changes2(args)
{
    IRSWorld.RITS.POLINK.applyEdits(args.additions, args.deletes, args.edits,updates_saved);
    IRSWorld.RITS.POLINK.get_value_table(bind_grid);

}
function save_changes_virtual(args)
{
    IRSWorld.RITS.POLINK.applyEdits(args.additions, args.deletes, args.edits,updates_saved);
    IRSWorld.RITS.POLINK.get_value_table_virtual(bind_grid_virtual);

}
function save_changes2(args)
{
    IRSWorld.RITS.POLINK.applyEdits(args.additions, args.deletes, args.edits,updates_saved);
    IRSWorld.RITS.POLINK.get_value_table(bind_grid);
}
function upload_getinfo(str)
{
    alert(document.getElementById('FileUpload1').filename);
    IRSWorld.RITS.POLINK.upload_get_fileinfo(document.getElementById('FileUpload1').filename);
}

function updates_saved(cm)
{   
    if (cm.error != null)
    {
        alert(cm.error.Message);
        return;
    }
    alert('successfully saved!');     
}
function show_infobox()
{
infobox.style.visibility = "visible";
}
function hide_infobox()
{
infobox.style.visibility = "hidden";
}
function show_error_msg()
{
alert('File is already uploaded. Please try other file');
clear_upload();
}
function show_grid()
{
    Get_codeopts();   
    IRSWorld.RITS.POLINK.get_value_table(bind_grid);
}
function show_grid_virtual()
{
    Get_codeopts_va();   
    IRSWorld.RITS.POLINK.get_value_table_virtual(bind_grid_virtual);
}
function show_panel(divName,filtertype)
{
       searchfilter = filtertype;
       var div = document.getElementById(divName);
       
        divmask = new maskpanel();
        divmask.show();
        
        document.getElementById(divName).style.display = "";
        
        div.style.top = (((document.documentElement.clientHeight / 2) + document.documentElement.scrollTop) - (div.offsetHeight / 2)) + 'px';
        div.style.left = (((document.documentElement.clientWidth / 2) + document.documentElement.scrollLeft) - (div.offsetWidth / 2)) + 'px';
        if (filtertype == "ENROLLMENT")
        {
            
            document.getElementById('testframe').src = "../Internet/InternetRemitter.aspx?isRemit=otc";
        }
}
 
function hidePanel(divName,filtertype)
{
        document.getElementById(divName).style.display = "none";
        divmask.hide();
        if (filtertype == "ENROLLMENT")
        {
            
            document.getElementById('testframe').src = "";
        }
}

function filter_by_date()
{
    if ((document.getElementById('txtfdate').value != "") && (document.getElementById('txttdate').value != ""))
    {
        IRSWorld.RITS.POLINK.set_date_variable(document.getElementById('txtfdate').value,document.getElementById('txttdate').value);
        if (searchfilter == "POLINK")
        {
            Get_codeopts();   
            IRSWorld.RITS.POLINK.get_table_date(bind_grid);
        }else{
            Get_codeopts_va();   
            IRSWorld.RITS.POLINK.get_table_date_va(bind_grid_virtual);
        }
        hidePanel('divSearch');
     }else{
        alert("Please provide date for filter");
     }
}
function filter_by_processed(filtertype)
{
    if (filtertype == "POLINK")
    {
        Get_codeopts();   
        IRSWorld.RITS.POLINK.get_value_table_processed(bind_grid_nostat);
    }else{
        Get_codeopts_va();   
        IRSWorld.RITS.POLINK.get_value_table_processed_virtual(bind_grid_virtual);
    }
}
function filter_by_hold(filtertype)
{
    if (filtertype == "POLINK")
    {
        Get_codeopts();   
        IRSWorld.RITS.POLINK.get_value_table_hold(bind_grid);
    }else{
        Get_codeopts_va();   
        IRSWorld.RITS.POLINK.get_value_table_hold_virtual(bind_grid_virtual);
    }
}
function filter_by_other(filtertype)
{
    if (filtertype == "POLINK")
    {
        Get_codeopts();   
        IRSWorld.RITS.POLINK.get_value_table_other(bind_grid);
    }else{
        Get_codeopts_va();   
        IRSWorld.RITS.POLINK.get_value_table_other_virtual(bind_grid_virtual);
    }
}
function filter_by_overlimit(filtertype)
{
    if (filtertype == "POLINK")
    {
        IRSWorld.RITS.POLINK.get_value_table_overlimit(bind_grid);
    }else{
        IRSWorld.RITS.POLINK.get_value_table_overlimit_virtual(bind_grid_virtual);
    }
}
function filter_by_pdep(filtertype)
{
    if (filtertype == "POLINK")
    {
        IRSWorld.RITS.POLINK.get_value_table_pdep(bind_grid_pdep);
    }else{
        IRSWorld.RITS.POLINK.get_value_table_pdep_virtual(bind_grid_pdep);
    }
}




//**Grid Generation**//

function bind_grid_pdep(result)
{
    if (result.error != null)
    {
        spawn_errpage(result.error.Message);
        return;
    }
    create_grid_with_account();
    document.getElementById('container').innerHTML = ""; 
    dbgrid.bind(result.value, 'container','testid');
       
}

function bind_grid_nostat(result)
{
    if (result.error != null)
    {
        spawn_errpage(result.error.Message);
        return;
    }
    create_grid_nostat();
    document.getElementById('container').innerHTML = ""; 
    dbgrid.bind(result.value, 'container','testid');
       
}
function bind_grid_virtual_nostat(result)
{
    if (result.error != null)
    {
        spawn_errpage(result.error.Message);
        return;
    }
    create_grid_virtual_nostat();
    document.getElementById('container').innerHTML = ""; 
    dbgrid.bind(result.value, 'container','testid');
       
}
function create_grid_nostat()
{
 
        dbgrid = new datagrid();
        {
            var col = new datagrid_column();
            col.header = 'ID';
            col.datafield = 'SequenceNumber';
            col.align = 'left';
            col.styles.datafield += 'width:30px';
	        col.styles.header += 'display:none';
	        col.styles.datafield += 'display:none';
            col.styles.cell += 'display:none';
	        
            //col.styles.cell += 'width:auto';
            dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column();
	        
	        col.header = 'Virtual No.';
	        col.datafield = 'VirtualNo';
	        col.align = 'left';
	        col.editable = false;
	        col.styles.cell += 'width:auto';
	        dbgrid.addColumn(col);
        }
         {
	        var col = new datagrid_column();
	        
	        col.header = 'Amount';
	        col.datafield = 'Amount';
	        col.align = 'left';
	        col.editable = false;
	        col.styles.cell += 'width:auto';
	        dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column();
	        
	        col.header = 'Date(Y/M/D)';
	        col.datafield = 'P_date';
	        col.editable = false;
	        col.align = 'left';
	        col.styles.editable = 'false';
	        col.styles.cell += 'width:auto';
	        dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column();
	        
	        col.header = 'Time(HH:MM)';
	        col.datafield = 'P_time';
	        col.align = 'left';
	        col.editable = false;
	        col.styles.cell += 'width:auto';
	        dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column();
	      
	        col.header = 'Beneficiary';
            //col.datasource = dbgrid;
	        col.datafield = 'FullName';
	        col.align = 'left';
	       col.editable = false;
	        col.styles.datafield += 'width:250px';
	        col.styles.cell += 'width:250px';
	        dbgrid.addColumn(col);
        }
        {
            var col = new datagrid_column();
            col.header = 'Hold Remarks';
            col.align = 'left';
	        col.datafield = 'Hold_remarks'
            col.styles.datafield += 'width:180px';
            col.styles.cell += 'width:180px';
            dbgrid.addColumn(col);
        }
        {
            var col = new datagrid_column();
            col.header = 'Status Remarks';
            col.value = '';
            col.align = 'left';
            col.editable = false;
            col.datafield = 'Null_remarks';
            col.styles.datafield += 'width:180px';
            col.styles.cell += 'width:180px';
            dbgrid.addColumn(col);
        }
        dbgrid.caption = 'PO Link Database';
        dbgrid.classNames.table = 'table_body';
        dbgrid.rows_page = 14;
        dbgrid.row.normal_style = 'background-color: #f7ffff;'; 

      
        
      
}

function create_grid_virtual_nostat()
{
 
        dbgrid = new datagrid();
        {
            var col = new datagrid_column();
            col.header = 'ID';
            col.datafield = 'SequenceNumber';
            col.align = 'left';
            col.styles.datafield += 'width:30px';
	        col.styles.header += 'display:none';
	        col.styles.datafield += 'display:none';
            col.styles.cell += 'display:none';
	        
            //col.styles.cell += 'width:auto';
            dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column();
	        
	        col.header = 'Virtual No.';
	        col.datafield = 'VirtualNo';
	        col.align = 'left';
	        col.editable = false;
	        col.styles.cell += 'width:auto';
	        dbgrid.addColumn(col);
        }
         {
	        var col = new datagrid_column();
	        
	        col.header = 'Amount';
	        col.datafield = 'Amount';
	        col.align = 'left';
	        col.editable = false;
	        col.styles.cell += 'width:auto';
	        dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column();
	        
	        col.header = 'Date(Y/M/D)';
	        col.datafield = 'P_date';
	        col.editable = false;
	        col.align = 'left';
	        col.styles.editable = 'false';
	        col.styles.cell += 'width:auto';
	        dbgrid.addColumn(col);
        }
       
            {
                var col = new datagrid_column();
    	      
	            col.header = 'Card Name';
                //col.datasource = dbgrid;
	            col.datafield = 'V_customerinfo';
	            col.align = 'left';
	           col.editable = false;
	            col.styles.datafield += 'width:250px';
	            col.styles.cell += 'width:250px';
	            dbgrid.addColumn(col);
            }
            {
	            var col = new datagrid_column();
    	      
	            col.header = 'Beneficiary';
                //col.datasource = dbgrid;
	            col.datafield = 'FullName';
	            col.align = 'left';
	           col.editable = false;
	            col.styles.datafield += 'width:250px';
	            col.styles.cell += 'width:250px';
	            dbgrid.addColumn(col);
            }
            {
	            var col = new datagrid_column();
	            col.header = 'Hold Remarks';
	            col.datafield = 'Hold_remarks';
	            col.align = 'left';
    	        
	            col.styles.datafield += 'width:180px';
	            col.styles.cell += 'width:180px';
	            dbgrid.addColumn(col);
            }
            {
	            var col = new datagrid_column();
	            col.header = 'Status Remarks';
	            col.datafield = 'Null_remarks';
	            col.align = 'left';
	            col.editable = false;
	            col.styles.datafield += 'width:180px';
	            col.styles.cell += 'width:180px';
	            dbgrid.addColumn(col);
            }
        dbgrid.caption = 'Virtual Access Database';
        dbgrid.classNames.table = 'table_body';
        dbgrid.rows_page = 14;
        dbgrid.row.normal_style = 'background-color: #f7ffff;'; 
}
function create_grid_with_account()
{
 
        dbgrid = new datagrid();
        {
            var col = new datagrid_column();
            col.header = 'ID';
            col.datafield = 'SequenceNumber';
            col.align = 'left';
            col.styles.datafield += 'width:30px';
	        col.styles.header += 'display:none';
	        col.styles.datafield += 'display:none';
            col.styles.cell += 'display:none';
	        
            //col.styles.cell += 'width:auto';
            dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column();
	        
	        col.header = 'Virtual No.';
	        col.datafield = 'VirtualNo';
	        col.align = 'left';
	        col.editable = false;
	        col.styles.cell += 'width:auto';
	        dbgrid.addColumn(col);
        }
         {
	        var col = new datagrid_column();
	        
	        col.header = 'Amount';
	        col.datafield = 'Amount';
	        col.align = 'left';
	        col.editable = false;
	        col.styles.cell += 'width:auto';
	        dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column();
	        
	        col.header = 'Date(Y/M/D)';
	        col.datafield = 'P_date';
	        col.editable = false;
	        col.align = 'left';
	        col.styles.editable = 'false';
	        col.styles.cell += 'width:auto';
	        dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column();
	        
	        col.header = 'Time(HH:MM)';
	        col.datafield = 'P_time';
	        col.align = 'left';
	        col.editable = false;
	        col.styles.cell += 'width:auto';
	        dbgrid.addColumn(col);
        }
        
        {
	        var col = new datagrid_column();
	      
	        col.header = 'Remitter';
            //col.datasource = dbgrid;
	        col.datafield = 'FullName';
	        col.align = 'left';
	       col.editable = false;
	        col.styles.datafield += 'width:250px';
	        col.styles.cell += 'width:250px';
	        dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column();
	      
	        col.header = 'Beneficiary';
            //col.datasource = dbgrid;
	        col.datafield = 'Beneficiary';
	        col.align = 'left';
	       col.editable = false;
	        col.styles.datafield += 'width:250px';
	        col.styles.cell += 'width:250px';
	        dbgrid.addColumn(col);
        }
        {
            var col = new datagrid_column();
            col.header = 'Hold Remarks';
            col.align = 'left';
	        col.datafield = 'Hold_remarks'
            col.styles.datafield += 'width:180px';
            col.styles.cell += 'width:180px';
            dbgrid.addColumn(col);
        }
        {
            var col = new datagrid_column();
            col.header = 'Status Remarks';
            col.value = '';
            col.align = 'left';
            col.editable = false;
            col.datafield = 'Null_remarks';
            col.styles.datafield += 'width:180px';
            col.styles.cell += 'width:180px';
            dbgrid.addColumn(col);
        }
        {
            var col = new datagrid_column();
            col.header = 'Deposit Account';
            col.value = '';
            col.align = 'left';
            col.editable = false;
            col.datafield = 'Accountno';
            col.styles.datafield += 'width:180px';
            col.styles.cell += 'width:180px';
            dbgrid.addColumn(col);
        }
        dbgrid.footer.edit_button.none = true;
        dbgrid.footer.edit_button.style += 'background-color: lavender;'; 
        dbgrid.footer.save_button.none = false;
        dbgrid.footer.save_button.style += 'background-color: lavender;'; 
        dbgrid.footer.cancel_button.none = false;
        dbgrid.footer.cancel_button.style += 'background-color: lavender;'; 
        
        dbgrid.caption = 'PO Link Database';
        dbgrid.classNames.table = 'table_body';
        dbgrid.rows_page = 14;
        dbgrid.row.normal_style = 'background-color: #f7ffff;'; 
        //document.getElementById('bottom_menu').style.visibility = "visible"
        dbgrid.footer.edit_button.none = false;
        dbgrid.footer.edit_button.style += 'background-color: lavender;color:maroon;';            
        //dbgrid.footer.edit_button.beforepost += 'add_before_post';
        
        dbgrid.footer.add_button.none = true;
        //dbgrid.footer.add_button.style += 'background-color: lavender;color:maroon;';            
        //dbgrid.footer.add_button.beforepost = 'add_before_post';
        
        dbgrid.footer.save_button.none = false;
        dbgrid.footer.save_button.onclick = 'save_changes2';  
        dbgrid.footer.save_button.style += 'background-color: lavender;'; 
        
        //dbgrid.footer.delete_button.onclick = 'delete_holidays';  
        //dbgrid.footer.delete_button.none = false;
        
        dbgrid.footer.cancel_button.none = false;
        dbgrid.footer.cancel_button.style += 'background-color: lavender;';
        
      
}
function create_grid()
{
 
        dbgrid = new datagrid();
        {
            var col = new datagrid_column();
            col.header = 'ID';
            col.datafield = 'SequenceNumber';
            col.align = 'left';
            col.styles.datafield += 'width:30px';
	        col.styles.header += 'display:none';
	        col.styles.datafield += 'display:none';
            col.styles.cell += 'display:none';
	        
            //col.styles.cell += 'width:auto';
            dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column();
	        
	        col.header = 'Virtual No.';
	        col.datafield = 'VirtualNo';
	        col.align = 'left';
	        col.editable = false;
	        col.styles.cell += 'width:auto';
	        dbgrid.addColumn(col);
        }
         {
	        var col = new datagrid_column();
	        
	        col.header = 'Amount';
	        col.datafield = 'Amount';
	        col.align = 'left';
	        col.editable = false;
	        col.styles.cell += 'width:auto';
	        dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column();
	        
	        col.header = 'Date(Y/M/D)';
	        col.datafield = 'P_date';
	        col.editable = false;
	        col.align = 'left';
	        col.styles.editable = 'false';
	        col.styles.cell += 'width:auto';
	        dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column();
	        
	        col.header = 'Time(HH:MM)';
	        col.datafield = 'P_time';
	        col.align = 'left';
	        col.editable = false;
	        col.styles.cell += 'width:auto';
	        dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column();
	        col.type = 'Select';
	        col.header = 'Status';
	        col.options = codeopts;
	        //col.datasource = dbgrid;
            
	        col.datafield = 'P_status';
	        col.align = 'left';
	        col.styles.datafield += 'width:130px';
	        col.styles.cell += 'width:130px';
	        dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column();
	      
	        col.header = 'Remitter';
            //col.datasource = dbgrid;
	        col.datafield = 'FullName';
	        col.align = 'left';
	       col.editable = false;
	        col.styles.datafield += 'width:250px';
	        col.styles.cell += 'width:250px';
	        dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column();
	      
	        col.header = 'Beneficiary';
            //col.datasource = dbgrid;
	        col.datafield = 'Beneficiary';
	        col.align = 'left';
	       col.editable = false;
	        col.styles.datafield += 'width:250px';
	        col.styles.cell += 'width:250px';
	        dbgrid.addColumn(col);
        }
        {
            var col = new datagrid_column();
            col.header = 'Hold Remarks';
            col.align = 'left';
	        col.datafield = 'Hold_remarks'
            col.styles.datafield += 'width:180px';
            col.styles.cell += 'width:180px';
            dbgrid.addColumn(col);
        }
        {
            var col = new datagrid_column();
            col.header = 'Status Remarks';
            col.value = '';
            col.align = 'left';
            col.editable = false;
            col.datafield = 'Null_remarks';
            col.styles.datafield += 'width:180px';
            col.styles.cell += 'width:180px';
            dbgrid.addColumn(col);
        }
        
        dbgrid.footer.edit_button.none = true;
        dbgrid.footer.edit_button.style += 'background-color: lavender;'; 
        dbgrid.footer.save_button.none = false;
        dbgrid.footer.save_button.style += 'background-color: lavender;'; 
        dbgrid.footer.cancel_button.none = false;
        dbgrid.footer.cancel_button.style += 'background-color: lavender;'; 
        
        dbgrid.caption = 'PO Link Database';
        dbgrid.classNames.table = 'table_body';
        dbgrid.rows_page = 14;
        dbgrid.row.normal_style = 'background-color: #f7ffff;'; 
        //document.getElementById('bottom_menu').style.visibility = "visible"
        dbgrid.footer.edit_button.none = false;
        dbgrid.footer.edit_button.style += 'background-color: lavender;color:maroon;';            
        //dbgrid.footer.edit_button.beforepost += 'add_before_post';
        
        dbgrid.footer.add_button.none = true;
        //dbgrid.footer.add_button.style += 'background-color: lavender;color:maroon;';            
        //dbgrid.footer.add_button.beforepost = 'add_before_post';
        
        dbgrid.footer.save_button.none = false;
        dbgrid.footer.save_button.onclick = 'save_changes2';  
        dbgrid.footer.save_button.style += 'background-color: lavender;'; 
        
        //dbgrid.footer.delete_button.onclick = 'delete_holidays';  
        //dbgrid.footer.delete_button.none = false;
        
        dbgrid.footer.cancel_button.none = false;
        dbgrid.footer.cancel_button.style += 'background-color: lavender;';
        
      
}

function create_grid_virtual()
{
 
        dbgrid = new datagrid();
            {
                var col = new datagrid_column();
                col.header = 'ID';
                col.datafield = 'SequenceNumber';
                col.align = 'left';
                col.styles.datafield += 'width:30px';
	            col.styles.header += 'display:none';
	            col.styles.datafield += 'display:none';
                col.styles.cell += 'display:none';
	            dbgrid.addColumn(col);
            }
            {
	            var col = new datagrid_column();
	            col.header = 'Virtual No.';
	            col.datafield = 'VirtualNo';
	            col.align = 'left';
	            col.editable = false;
	            col.styles.cell += 'width:auto';
	            dbgrid.addColumn(col);
            }
            {
	            var col = new datagrid_column();
	            col.header = 'Amount';
	            col.datafield = 'Amount';
	            col.align = 'left';
	            col.editable = false;
	            col.styles.cell += 'width:auto';
	            dbgrid.addColumn(col);
            }
            {
	            var col = new datagrid_column();
	            col.header = 'Date(Y/M/D)';
	            col.datafield = 'P_date';
	            col.editable = false;
	            col.align = 'left';
	            col.styles.editable = 'false';
	            col.styles.cell += 'width:auto';
	            dbgrid.addColumn(col);
            }
            {
	            var col = new datagrid_column();
	            col.type = 'Select';
	            col.header = 'Status';
	            col.options = codeopts;
                col.datafield = 'P_status';
	            col.align = 'left';
	            col.styles.datafield += 'width:130px';
	            col.styles.cell += 'width:130px';
	            dbgrid.addColumn(col);
            }
            {
                var col = new datagrid_column();
    	        col.header = 'Card Name';
                col.datafield = 'V_customerinfo';
	            col.align = 'left';
                col.editable = false;
	            col.styles.datafield += 'width:250px';
	            col.styles.cell += 'width:250px';
	            dbgrid.addColumn(col);
            }
            {
	            var col = new datagrid_column();
    	      
	            col.header = 'Beneficiary';
                col.datafield = 'FullName';
	            col.align = 'left';
	            col.editable = false;
	            col.styles.datafield += 'width:250px';
	            col.styles.cell += 'width:250px';
	            dbgrid.addColumn(col);
            }
            {
	            var col = new datagrid_column();
	            col.header = 'Hold Remarks';
	            col.datafield = 'Hold_remarks';
	            col.align = 'left';
    	        
	            col.styles.datafield += 'width:180px';
	            col.styles.cell += 'width:180px';
	            dbgrid.addColumn(col);
            }
            {
	            var col = new datagrid_column();
	            col.header = 'Status Remarks';
	            col.datafield = 'Null_remarks';
	            col.align = 'left';
	            col.editable = false;
	            col.styles.datafield += 'width:180px';
	            col.styles.cell += 'width:180px';
	            dbgrid.addColumn(col);
            }
        
        dbgrid.footer.edit_button.none = true;
        dbgrid.footer.edit_button.style += 'background-color: lavender;'; 
        dbgrid.footer.save_button.none = false;
        dbgrid.footer.save_button.style += 'background-color: lavender;'; 
        dbgrid.footer.cancel_button.none = false;
        dbgrid.footer.cancel_button.style += 'background-color: lavender;'; 
        
        dbgrid.caption = 'Virtual Access Database';
        dbgrid.classNames.table = 'table_body';
        dbgrid.rows_page = 14;
        dbgrid.row.normal_style = 'background-color: #f7ffff;'; 
      
        dbgrid.footer.edit_button.none = false;
        dbgrid.footer.edit_button.style += 'background-color: lavender;color:maroon;';            
        
        dbgrid.footer.add_button.none = true;
        
        dbgrid.footer.save_button.none = false;
        dbgrid.footer.save_button.onclick = 'save_changes_virtual';  
        dbgrid.footer.save_button.style += 'background-color: lavender;'; 
        
        dbgrid.footer.cancel_button.none = false;
        dbgrid.footer.cancel_button.style += 'background-color: lavender;';
        
      
}

