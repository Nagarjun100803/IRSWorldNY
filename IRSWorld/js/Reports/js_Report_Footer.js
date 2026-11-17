
var rf = new  Report_Footer();
var commons = new common_method();  


function Report_Footer()
{

    
    
    this.init_state = function()
    {
    
//        if (top == self)
//        {
//            window.open('../../BranchSetup/BranchSetupMain.aspx?load=BranchLookup.aspx', '_top');
//        }
//        else if (top.isAuthenticated())
//        {   
//            IRSWorld.Reports.Pages.Report_Footer.load_init_state(rf.init_state_callback());
//        }
//        else
//        {
//            spawn_errpage("This module requires login!");
//        }           
        IRSWorld.Reports.Pages.Report_Footer.load_init_state(rf.init_state_callback);
    }    
    this.get_footer_info = function()
    {
    }
    /* AJAX CALL BACKS */    
    
    this.init_state_callback = function(results)
    {
        try
        {
            
            var dt      =   results.value.Tables[1];
            var enable  =   dt.Rows[0].hasfooter
            var filepath  =   dt.Rows[0].filepath
            
            setText("txtfilepath" ,filepath );
            document.getElementById("chkEnable").checked = ((parseInt(enable) >= 1 ) ? "true" : "false" );
            
            rf.toggle_upload(2);
            commons.to_load_dropdownlist("ddBranch", null, "BranchCode", "BranchName",results.value.Tables[0]);                                
            
        }
        catch(e)
        {
        }
    }
    this.toggle_upload = function(opt)
    {
        if (opt == 1)
        {
            document.getElementById("divuploaded").style.display = "none";
            document.getElementById("divupload").style.display = "block";
        }
        else
        {
            document.getElementById("divuploaded").style.display = "block";
            document.getElementById("divupload").style.display = "none";
        }
    }
    this.change_click = function()
    {
        rf.toggle_upload(1);       
        document.getElementById("fileBrowse").click();
     
    }
    this.cancel_click = function()
    {
        rf.toggle_upload(2);                    
    }
    this.save_click = function()
    {
        //rf.toggle_upload(2);                    
    }
}