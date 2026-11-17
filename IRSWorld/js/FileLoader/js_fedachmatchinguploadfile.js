var ftpGrid;


function execute_ajaxgridview_command(param)
{
    if (upf != null)
    {
        upf.run_grid_viewer(param);
    }
}

function js_fedmatchinguploadfile()
{
    this.commons                = null;
    this.generic                = null;
	this.SEQ_ID                 = null;	
	this.grid_ajax_query        = null;
	this.message                = null;
	this.grid                   = null;	
    this.grid_ajax_page         = "../AjxPage/ajaxgridview.aspx"; 
    this.ajax_grid              = "GRIDCLASSADD";
    this.grid_ajax_query        = null;
    this.grid_ajax_page         = "../AjxPage/ajaxgrid.aspx";
    this.TotalCountUnconverted  = null; 
    this.BranchCode             = null;
    this.cls                    = this;
    this.reply                  = null;
    this.sfile_type             = null;
    this.err_msg                = null;
    this.searchvalue            = null;
    
    this.get_roledesc   =   function()
    {
        return document.getElementById("hfuserrole").value;
    }
    
   
    this.view_details = function()
    {
        document.getElementById("ddFileFormatSearch").options.length = 0;
        hide_elements('divFEDACHMatchingUploadFile~divDetails');//document.getElementById("divFEDACHMatchingUploadFile").style.display       = "none";
        show_elements('divSearch');//document.getElementById("divSearch").style.display           = "block";
        //document.getElementById("divDetails").style.display          = "none";
    }
    
    this.file_browse = function()
    {
       
        var filename    = getFileName('fileBrowse');//document.getElementById('fileBrowse').value;
         
        var ini = IRSWorld.IRSFileLoader.FEDACHMatchingUploadFile.upload_ini( filename);
        
        if (ini.value)
        {
            document.getElementById('btnPost').click();
        }
        else
        {
            IRSWorld.IRSFileLoader.FEDACHMatchingUploadFile.uploader_msg(upf.msg_callback);
        }
    }
    
    this.upload_ini_callback = function(res)
    {
        if (res != null)
        {
             if (res.value)
            {
                document.getElementById('btnPost').click();
            }
        }
    }
    
    this.show_loading_msg = function(msg)
    {
        new common_method().to_show_loading_message('divLoadingMsg');
        setText(document.getElementById('lblLoadingMsg'), msg);//document.getElementById('lblLoadingMsg').innerText = msg;
        showpanel('divPopProcess');
    }
    
    this.btnSearch_click = function()
    {
       this.show_uploaded_grid();
    }
    this.btnUpload_click = function()
    {   
        if (!confirm("Continue with Upload?"))
        {
            return;
        }
     
        var file_path   = document.getElementById("fileBrowse").value;
        
        AjaxPro.timeoutPeriod = 10000 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. TIMEOUT ENCOUNTERED.';
            alert(errmsg);           return;  
        } 
 
        IRSWorld.IRSFileLoader.FEDACHMatchingUploadFile.save_file( upf.upload_callback);
        upf.show_loading_msg('Uploading, Please wait...');
       
    }
    
    this.upload_callback = function(result)
    {
        var i = 0;
        if (result.value)
        {
            IRSWorld.IRSFileLoader.FEDACHMatchingUploadFile.uploader_msg(upf.msg_callback);
        }
        else
        {
            IRSWorld.IRSFileLoader.FEDACHMatchingUploadFile.uploader_msg(upf.msg_callback);
        }
        hide_elements('divAddlField~divfilename');
        show_elements('divfileBrowse');
    }
    
    this.msg_callback = function(result)
    {
        alert(result.value);
        hidepanel('divPopProcess');
        page_load();
    }

   
    this.cancel_click = function()
    {
        upf.reset_objects();
        hide_elements('divAddlField~divfilename~divShowUpload');
        show_elements('divfileBrowse~divFEDACHMatchingUploadFile');
               
    }
    
     this.show_uploaded_grid = function(parameter)
    {
        hide_elements('divAddlField~divfilename~divFEDACHMatchingUploadFile');
        show_elements('divShowUpload');
        
        
        this.searchvalue = document.getElementById("txtsearchupload").value;
        
        if (parameter == null)
        {
            parameter = 'ajx_loading$';
        }
        
        this.grid = new grid_viewer();        
        this.grid.sql_source     = "proc_get_fedmatching_info~" + this.searchvalue;
        this.grid.div_container  = document.getElementById("divFTPSummary");
        this.grid.datarow_column = "0,1";
        this.grid.page_size      = 10;
        //this.grid.open_grid_view();
        this.grid.process_ajax(parameter);
            
            
    }
    
    
    this.show_upload_file = function()
    {
            hide_elements('divAddlField~divfilename~divShowUpload');
            show_elements('divFEDACHMatchingUploadFile');
    }
    
   
    
    this.get_file_info = function(args)
    {
        var fname = ftpGrid.getValueAt(args.gridrow, 0);
        upf.download_file(fname);
    }
      
    this.reset_objects = function()
    {
        IRSWorld.IRSFileLoader.FEDACHMatchingUploadFile.reset_objects();      
    }
    
}

//File Uploader SideBar
function SideBar()
{
    var userrole = document.getElementById("hfuserrole").value;
    
    this.construct_side_bar = function()
    {
        document.getElementById('divFileUploadSideBar').innerHTML = this.get_side_bar();
    }
    
    this.get_side_bar = function()
    {
        var side_array = this.get_side_bar_list();
        var side_winder = new js_sidebar('sidemenu', side_array);
        
        return side_winder.toString();
    }
    
    this.get_side_bar_list = function()
    {
        var side_bar;
        
        
        side_bar = new Array
        (
            new Array(2, 0, 'Upload File', 'upf.cancel_click()'),
            new Array(3, 0, 'View Status of Files', 'upf.show_uploaded_grid()')
        );
        
        
        return side_bar;
    }
   
    this.get_side_bar_visibility = function(division_id, anchor_id)
    {
        var div = document.getElementById(division_id);
        var anc = document.getElementById(anchor_id);
        var cset = div.style.display;
        
        div.style.display = (cset == 'block') ? 'none' : 'block';
        
        anc.innerText = (cset == 'block') ? String.fromCharCode(187) : String.fromCharCode(171);
        document.getElementById('tdFileUploadSideBar').style.width = (cset == 'block') ? '0%' : '18%';
        this.construct_side_bar();
    }
    
    
    

}

page_load = function()
{
    
        
    var client_log = IRSWorld.IRSFileLoader.FEDACHMatchingUploadFile.client_logged_in().value;    
    
    if (client_log)
    {
        var is_postback = IRSWorld.IRSFileLoader.FEDACHMatchingUploadFile.get_is_postback().value;
        new SideBar().construct_side_bar();
        
        if (!is_postback)
        {   
            page_init();
            show_elements('divFEDACHMatchingUploadFile');
        }
        else
        {
            var val = IRSWorld.IRSFileLoader.FEDACHMatchingUploadFile.ctrl_vals().value;
            
            if (val != null | val != undefined)
            {
                val = val.split('|');
                        
                document.getElementById('recordCount').innerText                = 0;
                document.getElementById('fileSize').innerText                   = val[1];
                document.getElementById('divfilename').innerText                = val[0];                        
                hide_elements('divfileBrowse');//document.getElementById('divfileBrowse').style.display         = 'none';
                show_elements('divAddlField~divfilename~divFEDACHMatchingUploadFile');
              
            }
            else
            {
                var msg = IRSWorld.IRSFileLoader.FEDACHMatchingUploadFile.uploader_msg(alert_code_behind_msg);
                //alert_code_behind_msg(msg);
                IRSWorld.IRSFileLoader.FEDACHMatchingUploadFile.set_is_postback(false);
                page_load();
            }
        }
    }
}

function page_init()
{
    //Cross browser
    //var file = document.getElementById('fileBrowse');
    var dv = document.getElementById('fileBrowse');
        
    if (navigator.appName.indexOf('Explorer') == -1)
    {		
        dv.innerHTML = '';
        var a = '<table style="width:100%"><tr><td><input id="fileBrowse" runat="server" style="width:75%; height:24px" type="file" /></td>';
        a += '<td><input value="Search" style="width:25%; height:25px" type="button" class="button" onclick="javascript:upf.btnSearch_click()" /></td></tr></table>';
        //dv.innerHTML = '<table style="width:100%"><tr><td><input id="fileBrowse" runat="server" style="width:75%; height:24px" type="file" /></td>';
        //dv.innerHTML += '<td><input value="Search" style="width:25%; height:25px" type="button" class="button" onclick="javascript:upf.btnSearch_click()" /></td><tr><table>';
        dv.innerHTML = a;
    }
	
}

function alert_code_behind_msg(msg)//object object
{
    if (msg.value != null | msg.value != undefined)
    {
        alert(msg.value);
    }
}
    
function show_elements(elements)
{
    var elem_list = elements.split('~');
    
    for (var i = 0; i < elem_list.length; i++)
    {
        document.getElementById(elem_list[i]).style.display = 'block';
    }
}
    
function hide_elements(elements)
{
    var elem_list = elements.split('~');
    
    for (var i = 0; i < elem_list.length; i++)
    {
        document.getElementById(elem_list[i]).style.display = 'none';
    }
}

function getFileName(file)
{
    var file = (file.constructor == String) ? dom(file).value : file.value;
    var val  = null;
    
    if (file != undefined)
    {
        if (file.indexOf('\\') > -1)
        {
            val = file.split('\\');
        }
        else if (file.indexOf('/') > -1)
        {
            val = file.split('/');
        }
    }
    
    return (val.constructor == String) ? val : val[val.length - 1];
}


function a_p_page()
{
    this.js_com = new common_method();
                
    var file_types = IRSWorld.IRSFileLoader.FEDACHMatchingUploadFile.file_type().value;
    if (file_types != null | file_types != undefined)
    {
        new common_method().to_load_dropdownlist("ddFileTypeUpload", file_types, "VALUE", "CODE");
        show_elements('divFEDACHMatchingUploadFile');//document.getElementById("divFEDACHMatchingUploadFile").style.display       = "block";
        hide_elements('divDetails');//document.getElementById("divDetails").style.display          = "none";
    }
    else
    {
        alert('Error initializing file type. Please check if FileUploadType exists in the look up maintenance.');
        hide_elements('divFEDACHMatchingUploadFile');//document.getElementById('divFEDACHMatchingUploadFile').style.display = 'none';
    }
}

function a_a_page()
{
    document.getElementById('divFileUploadSideBar').innerHTML = '';
    //dom().style.
    upf.show_uploaded_grid();
}

function non_agent()
{
    this.js_com = new common_method();
                
    var file_types = IRSWorld.IRSFileLoader.FEDACHMatchingUploadFile.file_type().value;
    if (file_types != null | file_types != undefined)
    {
        new common_method().to_load_dropdownlist("ddFileTypeUpload", file_types, "VALUE", "CODE");
        show_elements('divFEDACHMatchingUploadFile');//document.getElementById("divFEDACHMatchingUploadFile").style.display       = "block";
        hide_elements('divDetails');//document.getElementById("divDetails").style.display          = "none";
    }
    else
    {
        alert('Error initializing file type. Please check if FileUploadType exists in the look up maintenance.');
        hide_elements('divFEDACHMatchingUploadFile');//document.getElementById('divFEDACHMatchingUploadFile').style.display = 'none';
    }
}


