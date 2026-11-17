
var js_FEDpopup = new js_FED_popup();


function execute_ajaxgridview_command(param)
{
    if (fed_viewer != null)
    {
        fed_viewer.run_grid_viewer(param);
    }
}


function fedach_watchlist_viewer()
{
    this.commons                = null;
    this.generic                = null;
	this.SEQ_ID                 = null;	
	this.grid_ajax_query        = null;
	this.message                = null;
	this.grid                   = null;
	this.accountno              = null;
	this.fullname               = null;	
	this.seq_id                 = null;
	this.jmode                  = null;
    this.grid_ajax_page         = "../AjxPage/ajaxgridview.aspx"; 
    this.ajax_grid              = "GRIDCLASSADD";
    this.grid_ajax_query        = null;
    this.grid_ajax_page         = "../AjxPage/ajaxgrid.aspx";
    this.TotalCountUnconverted  = null; 
    this.BranchCode             = null;
    this.cls                    = this;
    this.reply                  = null;
	
	this.view_fed_watchlist = function(parameter)
    {   
        this.accountno              = document.getElementById("txtAccountNo").value;
	    this.fullname               = document.getElementById("txtName").value;	
	    this.seq_id                 = document.getElementById("hidSeq_id").value;	
	    
        if (parameter == null)
        {
            parameter = 'ajx_loading$';
        }
        
        this.grid = new grid_viewer();        
        //this.grid.sql_source     = "proc_maintenance_fed_wl~" + this.seq_id + "~" + this.accountno + "~" + this.fullname + "~SELECT" ;
        
        //this.grid.sql_source     = "proc_select_fed_wl~" + "~" + this.accountno + "~" + this.fullname ;
        this.grid.sql_source     = "proc_select_fed_wl~" + this.accountno + "~" + this.fullname ;
        
        this.grid.div_container  = document.getElementById("divfedwlgrid");
        this.grid.datarow_column = "0,1,2";
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
               
               if (command_param[0] == "ajx_selected")
               {
                    this.proc_cancel();
                      // this.reply  = confirm("Create FED Watchlist File for " + command_param[2]  + "?");
                     document.getElementById("txtAccountNo").value = command_param[2] ;
                     document.getElementById("txtName").value = command_param[3] ;
                     document.getElementById("hidSeq_id").value = command_param[1]; 
                     document.getElementById("btnDelete").disabled = false;
               }
               else
               {
                
                        this.view_fed_watchlist(parameter);
               }
                js_FEDpopup.close_msg_prompt();
            }
        }
    }
   this.toggle_buttons = function(mode,btnID)
   {
   	    this.accountno = document.getElementById("txtAccountno").value;
		this.fullname  = document.getElementById("txtName").value;
		this.seq_id    =  document.getElementById("hidSeq_id").value;
   
		switch(mode) {
			case 'Add' :
				this.proc_add();
			break;
			case 'Edit' :
				this.proc_edit();
			break;
			case 'Update' :
				if (confirm("Update Record?")) { this.proc_save(btnID);}
			break;
			case 'Cancel' :
				this.proc_cancel();
			break;
			case 'Save' :
				if (confirm("Save Record?")) { this.proc_save(btnID);}
			break;
			case 'Delete' :
				if (confirm("Delete Record?")) { this.proc_delete(btnID);}
			break;
		}
   }
   this.proc_add = function()
   {
		document.getElementById("txtAccountNo").value = '';		
		document.getElementById("txtName").value = '';  
		document.getElementById("txtAccountNo").focus();		   
		document.getElementById("txtAccountNo").disabled = false;       
		document.getElementById("txtName").disabled = false;           
		document.getElementById("btnAdd").value = 'Save';
		document.getElementById("btnEdit").disabled = true;
		document.getElementById("btnCancel").disabled = false;
		document.getElementById("btnSearch").disabled = true;
		document.getElementById("btnDelete").disabled = true;
   }
   this.proc_edit = function()
   {
        document.getElementById("txtAccountNo").disabled = false;       
	    document.getElementById("txtName").disabled = false;     
	    document.getElementById("btnEdit").value = 'Update';
	    document.getElementById("btnAdd").disabled = true;
	    document.getElementById("btnCancel").disabled = false;
	    document.getElementById("btnSearch").disabled = true;
	    document.getElementById("btnDelete").disabled = true;
   }
   this.proc_cancel = function()
   {
		document.getElementById("txtAccountNo").value = '';
		document.getElementById("txtName").value = '';            
		document.getElementById("btnAdd").value = 'Add';
		document.getElementById("btnEdit").value = 'Edit';
		document.getElementById("txtAccountNo").disabled = true;       
		document.getElementById("txtName").disabled = true;     
		document.getElementById("btnEdit").disabled = false;
		document.getElementById("btnAdd").disabled = false;
		document.getElementById("btnCancel").disabled = true;
		document.getElementById("btnSearch").disabled = false;
		document.getElementById("btnDelete").disabled = true;
   }
   
   this.proc_search = function(value)
   {
        if(value == "Search")        
        {
            document.getElementById("txtAccountNo").disabled = false;       
		    document.getElementById("txtName").disabled = false;     
            document.getElementById("btnSearch").value = "Go";
        }
        else 
        {
            document.getElementById("txtAccountNo").disabled = true;       
		    document.getElementById("txtName").disabled = true;     
            document.getElementById("btnSearch").value = "Search";
            this.view_fed_watchlist();
        }
   }
   this.proc_save = function(btnID,indx)
   {

        if (!this.validate_entry())
        {
            return;
        }
        AjaxPro.timeoutPeriod = 100 * 1000;
        AjaxPro.onTimeout = function()
        {                    
            var errmsg = 'SERVER NOT RESPONDING. DATA OPERATION FAILED';
            alert(errmsg);           return;  
        } 	
        
		switch(btnID) {
		case 'btnAdd' :
			
			this.message = IRSWorld.FED.FEDACHWLViewer.maintenance_record(this.seq_id,this.accountno,this.fullname,'ADD').value;
			alert(this.message);
			if (this.message.toString().indexOf('RECORD SAVED') >= 0)
			{
				this.proc_cancel();
				this.view_fed_watchlist();				
			}
			break;
		case 'btnEdit' :
			//this.accountno = document.getElementById("txtAccountno").value
			//this.fullname  = document.getElementById("txtName").value
			this.message = IRSWorld.FED.FEDACHWLViewer.maintenance_record(this.seq_id,this.accountno,this.fullname,'UPDATE').value;
			alert(this.message);
			if (this.message.toString().indexOf('NAME ALREADY TAKEN') < 0)
			{
				this.proc_cancel();
			}
		break;
		
		}
		this.proc_cancel();
		this.view_fed_watchlist();	
		
   }
   this.proc_delete = function(btnID,indx)
   {            
        this.message = IRSWorld.FED.FEDACHWLViewer.maintenance_record(this.seq_id,this.accountno,this.fullname,'DELETE').value;
        alert(this.message);
    	this.proc_cancel();
		this.view_fed_watchlist();

   }
   this.validate_entry = function()
   {
        this.accountno = document.getElementById("txtAccountno").value;
        this.fullname = document.getElementById("txtName").value;
        if (this.accountno.toString().length == 0)
        { alert("Please input Accountno."); document.getElementById("txtAccountno").focus();return false;}
        if (this.fullname.toString().length == 0)
        { alert("Please input Beneficiary Name."); document.getElementById("txtName").focus();return false;}
        return true;
   }
   this.isNumberKey = function(evt)
   {
        var charCode = (evt.which) ? evt.which : event.keyCode
        if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

        return true;
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