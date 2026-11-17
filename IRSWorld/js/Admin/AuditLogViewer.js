    var dbgrid;
    
    function auditlog_viewer()
    {
        this.commons           = null;  
        this.Load_ServerRole = function()
        {
            AuditLogViewer.getRolesOptions('', this.load_ddlRoles_callback);
        }
        
        this.load_ddlRoles_callback = function(result)
        {
            if (result != null && result != "undefined")
            {
                //document.getElementById('divSearch').style.display="block";
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlRole", result.value, "fldvalue", "fldcode");
                //this.commons.to_load_dropdownlist("TESTTER", result.value, "FldValue", "FldCode");
                this.commons = null;
            }
        }
        
        this.commons           = null;
        this.load_branches = function()
        {
            AuditLogViewer.getBranches('',this.load_ddl_loc_callback);
        }
        this.load_ddl_loc_callback = function(result)
        {
            if (result != null && result != "undefined")
            {
                //document.getElementById('divSearch').style.display="block";
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddl_loc", result.value, "BranchCode", "BranchName");
                this.commons = null;
            } 
        }
              
     }  
     
             //calendar show
     function show_search_calendar()
     {
            if (document.getElementById('ddl_search').selectedIndex == 2)
            {
                document.getElementById('img_search_calendar').style.visibility = 'visible';
                return;
            }
            document.getElementById('img_search_calendar').style.visibility = 'hidden';
     }      
        
            

       // Create Table Grid 
        function create_grid()
        {
            dbgrid = new datagrid();
            {
    		    var col = new datagrid_column();
    		    col.header = 'UserID';
    		    col.datafield = 'USERID';
    		    col.align = 'left';
    		    col.styles.cell += 'width:20%';
    		    dbgrid.addColumn(col);
            }
            {
    		    var col = new datagrid_column();
    		    col.header = 'Full Name';
    		    col.datafield = 'FullName';
    		    col.align = 'left';
    		    col.styles.cell += 'width:25%';
    		    dbgrid.addColumn(col);
            }
           {
    		    var col = new datagrid_column();
    		    col.header = 'EventType';
    		    col.datafield = 'EVENTTYPE';
    		    col.align = 'right';
    		    col.styles.cell += 'width:8%';
    		    dbgrid.addColumn(col);
            }
  
            {
    		    var col = new datagrid_column();
    		    col.header = 'Role';
    		    col.datafield = 'ROLE';
    		    col.align = 'right';
    		    col.styles.cell += 'width:10%';
    		    dbgrid.addColumn(col);
            } 
             {
    		    var col = new datagrid_column();
    		    col.header = 'Old Value';
    		    col.datafield = 'OLDVALUE';
    		    col.align = 'right';
    		    col.styles.cell += 'width:10%';
    		    dbgrid.addColumn(col);
            }
             {
    		    var col = new datagrid_column();
    		    col.header = 'New value';
    		    col.datafield = 'NEWVALUE';
    		    col.align = 'right';
    		    col.styles.cell += 'width:10%';
    		    dbgrid.addColumn(col);
            }           
            {
    		    var col = new datagrid_column();
    		    col.header = 'Logged On';
    		    col.datafield = 'LOGGEDON';
    		    col.align = 'right';
    		    col.styles.cell += 'width:5%';
    		    dbgrid.addColumn(col);
            }
            {
    		    var col = new datagrid_column();
    		    col.header = 'Source';
    		    col.datafield = 'SOURCE';
    		    col.align = 'right';
    		    col.styles.cell += 'width:13%';
    		    dbgrid.addColumn(col);
            }
            {
    		    var col = new datagrid_column();
    		    col.header = 'BranchCode';
    		    col.datafield = 'BRANCHCODE';
    		    col.align = 'right';
    		    col.styles.cell += 'width:5%';
    		    dbgrid.addColumn(col);
            }
            {
    		    var col = new datagrid_column();
    		    col.header = 'EventDate';
    		    col.datafield = 'EVENTDATE';
    		    col.align = 'right';
    		    col.styles.cell += 'width:5%';
    		    dbgrid.addColumn(col);
            }
            
            
            dbgrid.caption = 'Audit Search';
            dbgrid.classNames.table = 'table_body';
            dbgrid.rows_page = 12;
            dbgrid.row.normal_style = 'background-color: #f7ffff;'; 
            
        }
        
        function on_search_click()
	    {
	        var usernm="";
	        var userid="";
	        var eventdt="";
	        var location="";
	        var eventtype="";
	        var logon="";
	        var roleid="";
	        var val= document.getElementById('ddl_search').value;
	        switch(val)
	        {
	            case "1":
	                userid = document.getElementById('txt_search').value;
	                break;
	            case "2":
	                usernm = document.getElementById('txt_search').value;
	                break;
	            case "3":
	                eventdt = document.getElementById('txt_search').value;
	                break;
	        }
	  
	        
	        location = document.getElementById('ddl_loc').value;
	        logon = document.getElementById('ddl_search_logon').value;
	        eventtype = document.getElementById('ddl_search_event').value;
            roleid = document.getElementById('ddlRole').value;
            
	        if((userid=='undefined') || (userid=='null')) userid="";
	        if((usernm=='undefined') || (usernm=='null')) usernm="";
	        if((eventdt=='undefined') || (eventdt=='null')) eventdt="";
            if((location=='undefined') || (location=='null')) location="";
            if((logon=='undefined') || (logon=='null')) logon="";
            if((eventtype=='undefined') || (eventtype=='null')) eventtype="";
            if((roleid=='undefined') || (roleid=='null')) roleid="";

	        AuditLogViewer.GetSearchInfo(eventdt,usernm,userid,location,eventtype,roleid,logon,bind_audit);    
        }
        
    function bind_audit(audit)
    {
        if (audit.error != null)
        {
            spawn_errpage(audit.error);
            return;
        }
        
        create_grid();
        ajax_show_status(true);
        dbgrid.bind(audit.value, 'tdAudit', 'gridAudit');
        set_visibility('Audit');
        ajax_show_status(false);
    } 
    
    function set_visibility(divid)
    { 
        if(divid=='Audit')
        {
            document.getElementById('tblSearch').style.display="none";
            document.getElementById('tblAudit').style.display="block";
        }
        else
        {
            document.getElementById('tblSearch').style.display="block";
            document.getElementById('tblAudit').style.display="none"; 
        }
    }  