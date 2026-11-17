 
var IRA = new initiliaze_Page(); 
var dbgrid;
var dbgrid_files;
 
function initiliaze_Page()
{
    this.populate_rec = function()
    {
        IRSWorld.Internet.InternetRemitterApproval.getRemitter(IRA.bind_grid);
    }

    this.bind_grid = function(table)
    {
        if (table.error != null)
        {
            spawn_errpage(table.error);
            return;
        }
        IRA.create_grid();
        ajax_show_status(true);
        
        IRA.set_visibility("tblInternetRemApproval",true);
        dbgrid.bind(table.value, "InterNetRemApp", "gridInterNetRemApp");
        ajax_show_status(false);

    } 

    this.set_visibility = function(div,value)
    { 
        if(value)
        {
            document.getElementById(div).style.display="block";
        }
        else
        {
            document.getElementById(div).style.display="none";
        }
    }
    
    this.create_grid = function()
    {
        dbgrid = new datagrid(); 
        {
    	    var col = new datagrid_column();
    	    col.header = "CIFID";
    	    col.styles.header += "display:none;";
    	    col.styles.datafield += "display:none;";
    	    col.datafield = "CIFID";
    	    col.align = "left";
    	    col.styles.cell += "width:8%;display:none;";
    	    dbgrid.addColumn(col);
        }
        {
    	    var col = new datagrid_column();
    	    col.header = "UserID";
    	    col.datafield = "userid";
    	    col.align = "left";
    	    col.styles.cell += "width:8%'";
    	    dbgrid.addColumn(col);
        }
        
        {
	        var col = new datagrid_column();
	        col.header = "FirstName";
	        col.datafield = "FirstName";
	        col.align = "left";
	        col.editable = false;
	        col.styles.cell += "width:16%";
	        dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column();
	        col.header = "MiddleName";
	        col.datafield = "MiddleName";
	        col.align = "left";
	        col.styles.cell += "width:13%";
	        col.editable = false;
	        dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column();
	        col.header = "LastName";
	        col.datafield = "LastName";
	        col.align = "right";
	        col.styles.cell += "width:16%";
	        col.editable = false;
	        dbgrid.addColumn(col);
        }
//        {
//	        var col = new datagrid_column();
//	        col.header = "SSN";
//	        col.datafield = "SSN";
//	        col.align = "left";
//	        col.styles.cell += "width:16%";
//	        col.editable = false;
//	        dbgrid.addColumn(col);
//        }
        {
    	    var col = new datagrid_column();
    	    col.header = "DONWLOAD";
    	    col.styles.header += "display:block;";
    	    col.styles.datafield += "display:block;";
    	    col.type = "button"
    	    col.onclick = "IRA.onTable_Click_Download";
    	    col.editable = true;
    	    col.value = "DownloadFile"
    	    col.align = "center";
    	    col.styles.cell += "width:13%;display:block;";
    	    dbgrid.addColumn(col);
        }
        {
    	    var col = new datagrid_column();
    	    col.header = "APPROVE";
    	    col.styles.header += "display:block;";
    	    col.styles.datafield += "display:block;";
    	    col.type = "button"
    	    col.onclick = "IRA.onTable_Click_Approved";
    	    col.editable = true;
    	    col.value = "Approvefile"
    	    col.align = "center";
    	    col.styles.cell += "width:13%;display:block;";
    	    dbgrid.addColumn(col);
        }

        {
    	    var col = new datagrid_column();
    	    col.header = "REJECT";
    	    col.styles.header += "display:block;";
    	    col.styles.datafield += "display:block;";
    	    col.type = "button"
    	    col.onclick = "IRA.onTable_Click_Reject";
    	    col.editable = true;
    	    col.value = "RejectFile"
    	    col.align = "center";
    	    col.styles.cell += "width:19%;display:block;";
    	    dbgrid.addColumn(col);
        }
         
        dbgrid.caption = "Remitter For Approval";
        dbgrid.classNames.table = "table_body";
        dbgrid.row.onclick = "IRA.onTable_Click"
        dbgrid.footer.edit_button.none = false;
        dbgrid.footer.edit_button.style += "background-color: lavender; display:none;";
        dbgrid.rows_page = 8;
        dbgrid.row.normal_style = "background-color: #f7ffff;"; 
        
     }
     
     
     this.create_grid_list = function()
     {
        dbgrid_files = new datagrid(); 
        {
            var col = new datagrid_column();
            col.header = "Document";
            col.styles.header += "display:block;";
            col.styles.datafield += "display:block;";
            col.datafield = "Document";
            col.align = "left";
            col.styles.cell += "width:18%;display:block;";
            dbgrid_files.addColumn(col);
        }
        {
            var col = new datagrid_column();
            col.header = "Download File";
            col.styles.header += "display:block;";
            col.styles.datafield += "display:block;";
            col.type = "button"
            col.onclick = "IRA.onTable_Click_Download_List";
            col.editable = true;
            col.value = "DownloadFile"
            col.align = "center";
            col.styles.cell += "width:7%;display:block;";
            dbgrid_files.addColumn(col);
        }
         
        dbgrid_files.caption = "Supporting Documents";
        dbgrid_files.classNames.table = "table_body";
        dbgrid_files.row.onclick = "IRA.onTableList_Click"
        dbgrid_files.footer.edit_button.none = false;
        dbgrid_files.footer.edit_button.style += "background-color: lavender; display:none;";
        dbgrid_files.rows_page = 8;
        dbgrid_files.row.normal_style = "background-color: #f7ffff;"; 

     }
     
     this.onTable_Click = function()
     {
        var edit_btn = document.getElementById("gridInterNetRemApp_footerbtn_Edit");            
        edit_btn.click();
     }
     
     this.onTableList_Click = function()
     {
        var edit_btn = document.getElementById("griddocumentList_footerbtn_Edit");            
        edit_btn.click();
     }

     
     this.onTable_Click_Reject = function(args)
     {
        var cifid = dbgrid.getValueAt(args.datarow, 0);
        var option = "reject";
        
        if(confirm("Are you sure you want to reject documents?")) 
        {
            IRSWorld.Internet.InternetRemitterApproval.updateDocument(cifid,option);
            alert("The Document has been Rejected.");
            this.populate_rec()

        }
        else { }
     }
     
     this.onTable_Click_Approved = function(args)
     {
        var cifid = dbgrid.getValueAt(args.datarow, 0);
        var option = "approve";
        
        if(confirm("Are you sure you want to approved documents?")) 
        {
            IRSWorld.Internet.InternetRemitterApproval.updateDocument(cifid,option);
            alert("The Document is succesfully Approved.");
            this.populate_rec()
        } 
        else { }
     }
     
     this.onTable_Click_Download_List = function(args)
     {
        var value = dbgrid_files.getValueAt(args.datarow, 0)
        document.getElementById("hf_Filename").value = value; 
        document.getElementById("Button1").click()

     }
     
     this.onTable_Click_Download = function(args)
     {   
        var cifid = dbgrid.getValueAt(args.datarow, 0); 
        var download = "";
        downloadFile = IRSWorld.Internet.InternetRemitterApproval.file_Exists(cifid);
        document.getElementById("dl_cifid").value = cifid; 
         
        if(downloadFile.value != undefined || downloadFile.value != null)
        {  
            if(downloadFile.value.Rows.length != 0)
            {
                if(downloadFile.value.Rows.length > 1)
                {
                    IRA.create_grid_list();
                    ajax_show_status(true);
                    dbgrid_files.bind(downloadFile.value, "documentList", "griddocumentList");
                    IRA.showPanel("DocList");
                    ajax_show_status(false);
                    //IRA.hidePanel("remList");   
                }
                else
                {
                    var value = downloadFile.value.Rows[0];
                    document.getElementById("hf_FIlename").value = downloadFile.value.Rows[0]["Document"]; 
                    document.getElementById('Button1').click();
                }

                }
             else
             {
                alert("File Does Not Exists!.")
             }
         }
         else
         {
            alert("File Does Not Exists!.")
         }

     }
          
     this.showPanel = function(divName)
     {
        var div = document.getElementById(divName);
        
        divmask = new maskpanel();
        divmask.show()
        IRA.set_visibility(divName,true);

        div.style.top = (((document.documentElement.clientHeight / 2) + document.documentElement.scrollTop) - (div.offsetHeight / 2)) + "px";
        div.style.left = (((document.documentElement.clientWidth / 2) + document.documentElement.scrollLeft) - (div.offsetWidth / 2)) + "px";
     }
     
    this.hidePanel = function(divName)
    {        
        divmask = new maskpanel();  
        IRA.set_visibility(divName,false);     
        divmask.hide();
        
    }
    
    
    var maskpanel = function(source)
    {

        this.show = function()
        {
            if(!document.getElementById("xdivmasking"))
            {
                var divEle = document.createElement("div");
                divEle.setAttribute("id", "xdivmasking");
                document.body.appendChild(divEle);

                var divSty = document.getElementById("xdivmasking").style;
                divSty.position = "absolute"; 
                divSty.top = "0px";
                divSty.left = "0px";
                divSty.zIndex = "46";
                divSty.opacity = ".50";
                divSty.backgroundColor="#000";
                divSty.filter = "alpha(opacity=50)";

                var divFram = document.createElement("iframe");
                divFram.setAttribute("id", "xmaskframe");
                document.body.appendChild(divFram);
                divSty = document.getElementById("xmaskframe").style;
                divSty.position = "absolute";
                divSty.top = "0px";
                divSty.left = "0px";
                divSty.zIndex = "45";
                divSty.border = "none";
                divSty.filter = "alpha(opacity=0)";
          }

          this.divobj = document.getElementById("xdivmasking");
          this.waitifrm = document.getElementById("xmaskframe");

          var dsh = document.documentElement.scrollHeight;
          var dch = document.documentElement.clientHeight;
          var dsw = document.documentElement.scrollWidth;
          var dcw = document.documentElement.clientWidth;
          var bdh = (dsh>dch) ? dsh:dch;
          var bdw = (dsw>dcw) ? dsw:dcw;

          this.waitifrm.style.height = this.divobj.style.height = bdh + "px";
          this.waitifrm.style.width = this.divobj.style.width = bdw + "px";
          this.waitifrm.style.display = this.divobj.style.display = "block";
       }
       
       this.hide = function()
       {
          document.getElementById("xmaskframe").style.display = document.getElementById("xdivmasking").style.display = "none";
       }
    }
  
}
