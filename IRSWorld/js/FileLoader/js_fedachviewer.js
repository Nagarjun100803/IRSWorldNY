
var js_FEDpopup = new js_FED_popup();
var edit_mode = false;

var dbgrid;

function execute_ajaxgridview_command(param)
{
    if (fed_viewer != null)
    {
        fed_viewer.run_grid_viewer(param);
    }
}


function fedach_viewer()
{
    this.commons                = null;
    this.generic                = null;
	this.SEQ_ID                 = null;	
	this.grid_ajax_query        = null;
	this.message                = null;
	this.grid                   = null;
	this.dtTo                   = null;
	this.dtFrom                 = null;
    this.grid_ajax_page         = "../AjxPage/ajaxgridview.aspx"; 
    this.ajax_grid              = "GRIDCLASSADD";
    this.grid_ajax_query        = null;
    this.grid_ajax_page         = "../AjxPage/ajaxgrid.aspx";
    this.TotalCountUnconverted  = null; 
    this.BranchCode             = null;
    this.cls                    = this;
    this.reply                  = null;
    
    
    this.load_grid_viewer = function()
    {     
   
    } 
    
    this.view_fed_files = function(parameter)
    {
        this.dtTo = document.getElementById("txtTo").value;
        this.dtFrom = document.getElementById("txtFrom").value;
        this.BranchCode  = document.getElementById("hidBranch").value; 
        
        if (parameter == null)
        {
            parameter = 'ajx_loading$';
        }
        
        this.grid = new grid_viewer();        
        this.grid.sql_source     = "proc_Get_FedAch_Watchlist_Files~" + this.dtFrom + "~" +  this.dtTo +  "~"  + "NYC";
        this.grid.div_container  = document.getElementById("fedgrid");
        this.grid.datarow_column = "0,1";
        this.grid.page_size      = 10;
        //this.grid.open_grid_view();
        this.grid.process_ajax(parameter);   
    }
   
     this.run_grid_viewer = function(parameter)
     {
        if (parameter != null)
        {
            var command_param = parameter.split("$");

            if (command_param.length > 0)
            {            
                js_FEDpopup.open_msg_prompt("SELECTING DATA");                             
               //implement search
               
               if (command_param[0] == "ajx_selected")
               {
                       this.reply  = confirm("Create FED Watchlist File for " + command_param[2]  + "?");
               
                       js_FEDpopup.close_msg_prompt();
                       if (this.reply)
                       {                              
                             var okforload =  parseInt(IRSWorld.IRSFileLoader.FEDACHViewer.check_watchlist_loading().value);
                            
                            js_FEDpopup.close_msg_prompt(); 
                            
                            if (okforload == 0)
                            {
                                this.generate_extract(command_param[2]);
                            }
                            else {
                                if (okforload == 1)
                                {
                                    alert("WATCHLIST NAMES MATCHING STILL ONGOING");
                                }
                                else { alert("ERROR OCCURRED");}
                            }
                            return
                       }
               }
               else
               {
                
                        this.view_fed_files(parameter);
               }
                js_FEDpopup.close_msg_prompt();
            }
        }
    }
    
    this.display_file_list = function()
    {
   
        this.dateto = document.getElementById("txtTo").value;
        this.datefrom = document.getElementById("txtFrom").value;
            
        AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. DISPLAYING OF FILES FAILED.';
            alert(errmsg);             
        } 

        IRSWorld.IRSFileLoader.FEDACHViewer.Get_File_List(this.datefrom,this.dateto,this.display_file_list_callback)
    }
    
    this.display_file_list_callback = function(result)
    {
        if (result.error != null)
        {
            alert(result.error.Message)
            return;
        }
        
        edit_mode = true;
        dbgrid = fed_viewer.create_filelist_grid()
        dbgrid.clearFilter();                    
        dbgrid.filter.enabled = true;
        dbgrid.rows_page = 5;
        dbgrid.on_page_change = 'fed_viewer.on_page_change';
        dbgrid.autoPost = true;
        dbgrid.bind(result.value, 'tdfedgrid', 'tblFedViewer');
  
        fed_viewer.enableRow(edit_mode);
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
		    col.datafield = 'Seq_Id';
            col.styles.cell += 'width:auto;display:none';
		    col.styles.header += 'display:none';
		    col.styles.datafield += 'display:none';
		    
		    gv.addColumn(col);
         }       
         //==================================================
         // Col No 1; FileName
         //==================================================        
         {
		    var col = new datagrid_column();
		    col.header = 'FileName';
		    col.datafield = 'FileName';
		    col.align = 'right';
		    col.styles.cell += 'width:30%';
		    col.editable = false;
		    gv.addColumn(col);
         }
          //==================================================
         // Col No 2; DateUploadedLocal
         //==================================================        
         {
		    var col = new datagrid_column();
		    col.header = 'DateUploadedLocal';
		    col.datafield = 'DateUploadedLocal';
		    col.align = 'right';
		    col.styles.cell += 'width:20%';
		    col.editable = false;
		    gv.addColumn(col);
         }
         //==================================================
         // Col No 3; Generate Regular Match
         //==================================================              
         {
            var col = new datagrid_column(); 
            col.header = 'Regular Watchlist Report';
            col.datafield = 'btnRegular';
            col.styles.cell += 'width:auto';
            col.width = '10px';
            col.align = 'center';
            col.type = 'button';
            col.value = 'Generate Report';
            col.editable = true;
            col.onclick = "fed_viewer.generate_grid_viewer";
            gv.addColumn(col);
         } 
         
         //==================================================
         // Col No 4; Generate Alias Match
         //==================================================              
         {
            var col = new datagrid_column(); 
            col.header = 'Alias Watchlist Report';
            col.datafield = 'btnAlias';
            col.styles.cell += 'width:auto';
            col.width = '10px';
            col.align = 'center';
            col.type = 'button';
            col.value = 'Generate Report';
            col.editable = true;
            col.onclick = "fed_viewer.generate_grid_viewer";
            gv.addColumn(col);
         } 
         
         //==================================================
         // Col No 5; Generate Exception Match
         //==================================================              
         {
            var col = new datagrid_column(); 
            col.header = 'Exception Watchlist Report';
            col.datafield = 'btnException';
            col.styles.cell += 'width:auto';
            col.width = '10px';
            col.align = 'center';
            col.type = 'button';
            col.value = 'Generate Report';
            col.editable = true;
            col.onclick = "fed_viewer.generate_grid_viewer";
            gv.addColumn(col);
         } 
         
        gv.row.normal_style = 'background-color: #f7ffff;';                      
        return gv;
    }
    
    this.on_page_change = function()
    {   
        this.enableRow(edit_mode);
    }
    
    this.enableRow = function(enable)
    {
        var tabid =  document.getElementById('tblFedViewer');
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
                        if (childs[k].nodeName == 'INPUT' | childs[k].nodeName == 'button' )
                        {
                            childs[k].disabled = read_only;
                        }               
                    }
                }
            }
        }
    }
    
    this.generate_grid_viewer = function(args)
     {
        if (args != null)
        {
            var command_param = args.control.split("_");
            var file_name = dbgrid.getValueAt(args.datarow, 1);
            
            if (command_param.length > 0)
            {            
                js_FEDpopup.open_msg_prompt("SELECTING DATA");                             
               
               if (command_param[2] == "btnRegular")
               {
                    this.reply  = confirm("Create FEDACH Regular Watchlist File for File Name : " + file_name + "?");
               }
               else if (command_param[2] == "btnAlias")
               {
                    this.reply  = confirm("Create FEDACH Alias Watchlist File for File Name : " + file_name + "?");
               }
               else if (command_param[2] == "btnException")
               {
                    this.reply  = confirm("Create FEDACH Exception Watchlist File for File Name : " + file_name + "?");
               }
       
               js_FEDpopup.close_msg_prompt();
               if (this.reply)
               {                              
                     var okforload =  parseInt(IRSWorld.IRSFileLoader.FEDACHViewer.check_watchlist_loading().value);
                    
                    js_FEDpopup.close_msg_prompt(); 
                    
                    if (okforload == 0)
                    {
                        if (command_param[2] == "btnRegular")
                        {
                            this.generate_extract(file_name, 1);
                        }
                        else if (command_param[2] == "btnAlias")
                        {
                            this.generate_extract(file_name, 2);
                        }
                        else if (command_param[2] == "btnException")
                        {
                            this.generate_extract(file_name, 3);
                        }
                    }
                    else {
                        if (okforload == 1)
                        {
                            alert("WATCHLIST NAMES MATCHING STILL ONGOING");
                        }
                        else { alert("ERROR OCCURRED");}
                    }
                    return
               }
               
                js_FEDpopup.close_msg_prompt();
            }
        }
    }
    
        this.generate_extract = function(epath, report_type)
        {
            AjaxPro.timeoutPeriod = 10000 * 1000;
            AjaxPro.onTimeout = function()
            {                    
                var errmsg = 'SERVER NOT RESPONDING. SELECTING OF DATA FAILED.';
                alert(errmsg);           return;  
            } 
            js_FEDpopup.open_msg_prompt("CREATING FILE");     
            IRSWorld.IRSFileLoader.FEDACHViewer.create_fed_file(epath,report_type,this.create_fed_file_callback);
        }
        this.create_fed_file_callback = function(res)
        {
            js_FEDpopup.close_msg_prompt();
            if (res.value != null)            
            {
                var s = res.value;
                if (s.indexOf("ERROR") > -1 )
                {
                   alert("AN ERROR OCCURRED WHILE CREATING FILE " + s);
                }
                else
                {                    
                    if (s.length > 0 && s.indexOf(".") > 0)
                    {
                        window.open("../ExtractPath/" + res.value);
                    }
                    else 
                    {
                        alert("No File to generate.");
                    }
                }
            }
            
            
        }
    
    
}


function js_FED_popup()
{
    this.msg_prompt_control = null;
    
    this.create_popup = function()
    {
        this.open_modal_window();
    }
    this.close_popup = function()
    {
        this.close_modal_window();
    }
     this.open_msg_prompt = function(msg)
    {
        this.msg_prompt_control = new screen_msg_demo();
        this.msg_prompt_control.open_screen();
        this.msg_prompt_control.change_msg(msg);
    }
    
    this.close_msg_prompt = function()
    {
        if (this.msg_prompt_control != null)
        {
            this.msg_prompt_control.close_screen();
        }

        this.msg_prompt_control = null;
    }    
    
    this.open_modal_window = function()
    {
        this.load_modal_css();
    
        this.outer_modal_div           = document.createElement("div");
        this.outer_modal_div.id        = "outer_mod_div_fed";
        this.outer_modal_div.className = "outer_fed";
    
        document.body.style.margin = "0px";
        document.body.appendChild(this.outer_modal_div);
        document.getElementById("outer_mod_div_fed").style.height = screen.height;
    
        this.construct_div_content();
        this.commons = new common_method();
        this.commons.to_show_hide_combobox(false);
        this.commons = null;
    }

    this.close_modal_window = function()
    {
        this.commons = new common_method();
        this.commons.to_show_hide_combobox(true);
        this.commons = null;

        if(this.outer_modal_div != null){document.body.removeChild(this.outer_modal_div);this.outer_modal_div  = null;}
        if(this.inner_modal_div != null){document.body.removeChild(this.inner_modal_div);this.inner_modal_div  = null;}

        document.body.style.margin = "";
    }

    this.construct_div_content = function()
    {
        var content_div = null;

            this.inner_modal_div              = document.createElement("div");
            this.inner_modal_div.className    = "inner_div";		
            this.inner_modal_div.id           = "inner_mod_div_fed";
            this.inner_modal_div.style.height = "auto";		
            this.inner_modal_div.style.width  = "650px";
            this.inner_modal_div.style.top    = "30px";
            this.inner_modal_div.style.left   = (parseInt(screen.width) - parseInt(this.inner_modal_div.style.width)) / 2 - 10 + "px";
            this.inner_modal_div.style.border = "medium solid white";

            content_div                       = document.createElement("div");
            content_div.id                    = "content_mod_div_fed";
            content_div.style.background      = "transparent";
            content_div.innerHTML             = "<center><img src = '.././images/ajaxloader.gif'></img></center>";

            this.inner_modal_div.appendChild(content_div);

            document.body.appendChild(this.inner_modal_div);
    }
	
    this.load_modal_css = function()
    {	    
            var file_reference = document.createElement("link");
            
            file_reference.setAttribute("rel", "stylesheet");
            file_reference.setAttribute("type", "text/css");
            file_reference.setAttribute("href", ".././css/cssmodals.css");

            if (typeof file_reference !="undefined")
            {		    
                    document.getElementsByTagName("head")[0].appendChild(file_reference)
            }
    }
        
    window.onscroll = function () 
    {            
        var i = 0;
        var m = 0;
    
        var divelement_outer = document.documentElement.getElementsByTagName("div");
        var divelement_inner = document.documentElement.getElementsByTagName("div");
                        
            for(i = 0; i < divelement_outer.length;i++)
            {
                if (divelement_outer.item(i).id == "outer_mod_div_fed")
                {
                    divelement_outer.item(i).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop;break;
                }	        
            }
            for(m = 0; m < divelement_inner.length;m++)
            {
                if (divelement_inner.item(m).id == "inner_mod_div_fed")
                {
                    divelement_outer.item(m).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop + 30 : document.body.scrollTop + 30;break;
                }
            }
    }
}