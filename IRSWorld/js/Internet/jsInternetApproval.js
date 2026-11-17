function Internet_Approval()
{
    this.download_rem_supporting_docs = function()
    {
        document.getElementById('HPath').value = '';
        var filename = document.getElementById('ddSupportingDocs').value;//'C:\\temp\\INTERNETCLIENT\\156\\Blue hills.jpg';//document.getElementById('ddlFileList').value;
        
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
    
    this.load_defaults = function()
    {
//        Internet_Remitter_Main.active_panel = "INTERNET_REM_APP_BENE";
//        this.ajax_loading();
//        Internet_Beneficiary.load_grid_viewer_appbene();
        this.show_panel_app_rem();

    }
    
	this.delete_temp_files = function()
	{   
	    if (Internet_Remitter_Main.active_panel == "INTERNET_REM_APP_REM")
	    {
	        IRSWorld.Internet.InternetApproval.delete_temp_files(Internet_Remitter.SEQ_ID.toString());
	    }
	}    
    
    this.construct_side_bar = function(div_container)
    {
        this.Internet_Approval_Sidebar_class = new Internet_Approval_Sidebar(); 
	   
		document.getElementById(div_container).innerHTML = this.Internet_Approval_Sidebar_class.get_side_bar();	
    }
	  
    this.set_side_bar_visibility = function(division_id, anchor_id)
    {
        this.Internet_Approval_Sidebar_class.get_side_bar_visibility(division_id, anchor_id);
    }
    
    this.ajax_loading = function()
    {
        //this.commons = new common_method();
        show_loading_message("DIVBENEFORAPP~DIVCARDFORAPP~DIVBENEREJ~DIVCARDREJ");
    }
    
    this.show_panel_app_bene = function()
    {
        this.check_session();
        
        Internet_Remitter_Main.active_panel = "INTERNET_REM_APP_BENE";
        this.show_elements("divInternetBeneApproval~divAppMenu~divIntBeneForApp~DIVBENEFORAPP", true);
        this.ajax_loading();
        Internet_Beneficiary.load_grid_viewer_appbene();
    }
    
    this.show_panel_app_rem = function()
    {
        this.check_session();
        
        Internet_Remitter_Main.active_panel = "INTERNET_REM_APP_REM";
        this.show_elements("divInternetBeneApproval~divAppMenu~divIntRemForApp~DIVREMFORAPP", true);
        this.ajax_loading();
        Internet_Remitter.load_grid_viewer_apprem();
    }
   
    this.show_panel_app_card = function()
    {
        Internet_Remitter_Main.active_panel = "INTERNET_REM_APP_CARD";
        this.show_elements("divInternetBeneApproval~divAppMenu~divIntCardForApp~DIVCARDFORAPP", true);
        this.ajax_loading();
        Internet_Remitter_Cards.load_grid_viewer_appcard();
    }
    
    this.show_panel_rej_bene = function()
    {
        Internet_Remitter_Main.active_panel = "INTERNET_REM_REJ_BENE";
        this.show_elements("divInternetBeneApproval~divAppMenu~divIntBeneRej~DIVBENEREJ", true);
        this.ajax_loading();
        Internet_Beneficiary.load_grid_viewer_rejbene();
    }
    
    this.show_panel_rej_rem = function()
    {
        Internet_Remitter_Main.active_panel = "INTERNET_REM_REJ_REM";
        this.show_elements("divInternetBeneApproval~divAppMenu~divIntRemRej~DIVREMREJ", true);
        this.ajax_loading();
        Internet_Remitter.load_grid_viewer_rejrem();
    }    
    
    this.show_panel_rej_card = function()
    {
        Internet_Remitter_Main.active_panel = "INTERNET_REM_REJ_CARD";
        this.show_elements("divInternetBeneApproval~divAppMenu~divIntCardRej~DIVCARDREJ", true);
        this.ajax_loading();
        Internet_Remitter_Cards.load_grid_viewer_rejcard();
    }
    
    this.approve = function()
    {
        var FName   =   '';
        var LName   =   '';
        var MName   =   '';
        
        if (Internet_Remitter_Main.active_panel == "INTERNET_REM_APP_REM")
        {
            FName   =   Internet_Remitter.Firstname;
            LName   =   Internet_Remitter.Lastname;
            MName   =   Internet_Remitter.Middlename;
                
            //Internet_Remitter.approve_rem();    
            Internet_Remitter_Main.check_and_get_hitlist(FName, LName, MName, 1, true)
        }
        else
        {
            FName   =   Internet_Beneficiary.Firstname;
            LName   =   Internet_Beneficiary.Lastname;
            MName   =   Internet_Beneficiary.Middlename;
                    
            //Internet_Beneficiary.approve_bene();
            Internet_Remitter_Main.check_and_get_hitlist(FName, LName, MName, 2, true)
        }
    }  
    
    this.reject = function()
    {
        if (Internet_Remitter_Main.active_panel == "INTERNET_REM_APP_REM")
        {
            Internet_Remitter.reject_rem();    
        }
        else
        {
            Internet_Beneficiary.reject_bene();
        }
    }       
    
    this.check_session = function()
    {
        /*
        var userid = IRSWorld.Internet.InternetRemitter.get_userid().value;
        document.getElementById("txtHiddenUserID").value = userid;
        
        if (userid == null)
        {
            spawn_errpage("Session Expired");
        }
        */
        
        IRSWorld.Internet.InternetRemitter.get_userid(
            function (rs)
            {
                if (rs.value == null)
                {
                    spawn_errpage("Session Expired.");
                }
                
                document.getElementById("txtHiddenUserID").value = rs.value;
            }
        );    
    }
    
    this.hide_all = function()
    {
        var elements = "DIVREMFORAPP~DIVBENEFORAPP~DIVCARDFORAPP~divInternetBeneApproval~divAppMenu~divIntCardForApp~divIntRemForApp~divIntBeneForApp~divIntRemRej~divIntBeneRej~divIntCardRej";
        
        var elems = elements.split("~");
        var y = 0;
        
        for(y = 0; y < elems.length; y++)
        {
            if (elems[y] != null || elems[y] != "")
            { 
                document.getElementById(elems[y]).style.display   = "none";
            } 
        }
    }
   
    this.show_elements = function(elements, isVisible)
    {
       this.check_session();
        
       var elems = elements.split("~");
       var y = 0;
        
       this.hide_all();
                
       for(y = 0; y < elems.length; y++)
       {
           if (elems[y] != null || elems[y] != "")
           { 
               if(isVisible == true)
               {
                   document.getElementById(elems[y]).style.display   = "block";
               }
               else
               {
                   document.getElementById(elems[y]).style.display   = "none";
               }               
           } 
       }
    }
       
}



function Internet_Approval_Sidebar()
{
    this.get_side_bar_visibility = function(division_id, anchor_id)
    {
        var div  = document.getElementById(division_id);
        var cset = div.style.display;

        div.style.display = (cset == "block") ? "none" : "block";
        document.getElementById(anchor_id).innerHTML = (cset == "block") ? String.fromCharCode(187) : String.fromCharCode(171);
    }

    this.get_side_bar = function()
    {
        var side_array  = this.get_side_bar_list();
        var side_winder = new js_sidebar('sidemenu', side_array);
        
        return side_winder.toString();      
    }
  
    this.get_side_bar_list = function()
    {  
        var app_bene = new Array(1,0,'Beneficiaries (For Approval)',  'javascript:Internet_Approval.show_panel_app_bene();');
        var app_rem  = new Array(2,0, 'Remitter (For Approval)', 'javascript:Internet_Approval.show_panel_app_rem();')
        var app_card = new Array(2,0,'Cards (For Approval)',          'javascript:Internet_Approval.show_panel_app_card();');
        var rej_rem = new Array(2,0,'Remitters (Rejected)',      'javascript:Internet_Approval.show_panel_rej_rem();');
        var rej_bene = new Array(2,0,'Beneficiaries (Rejected)',      'javascript:Internet_Approval.show_panel_rej_bene();');
        var rej_card = new Array(2,0,'Cards (Rejected)',              'javascript:Internet_Approval.show_panel_rej_card();');
        //var sidebar  = new Array(app_bene, app_rem, app_card, rej_bene, rej_rem, rej_card);
        var sidebar  = new Array(app_rem, app_bene, app_card, rej_rem, rej_bene, rej_card);
        return sidebar;       
    }
}