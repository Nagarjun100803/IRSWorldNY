    
    var ide =   new js_ibatchbatchentries();    
    var ajax_grid = null;
    var sb =   new SideBar();
    var flag_mode = "";
    var tab_mode = "DEBIT";
    var datatable_lookup;
    var dbgrid;
    var datatable;
    function SideBar()
    {
   
    var  v_sidebarid = "divDataEntryMenu";
    
    this.construct_side_bar = function()
    {
        document.getElementById(v_sidebarid).innerHTML = this.get_side_bar();
    }
    this.get_side_bar = function(v_sidebarid)
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
                new Array(2, 0, 'Create Batch', 'ide.create_new_batch()'),
                new Array(3, 0, 'Search Batch', 'ide.search_batch()'),
                new Array(4, 0, 'Show Summary', 'sbs.open_batch_summary_screen()')
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
        document.getElementById(v_sidebarid).style.width = (cset == 'block') ? '0%' : '18%';
        this.construct_side_bar();
    }
   
}
    
    
    function js_ibatchbatchentries()
    {
        var panels = new Array(7);
        var common = new common_method();
        
        
        panels[0] = 'diviinquiry';
        panels[1] = 'diviDebit';
        panels[2] = 'diviCredit';
        panels[3] = 'diviComputation';
        panels[4] = 'divibatchholder';
        panels[5] = 'diviCharges';
        panels[6] = 'divisummary';
        panels[7] = 'divicreatebatch';
        
        
         this.page_setup = function()
        {
            if (top == self)
            {
                window.open('../../BranchSetup/BranchSetupMain.aspx?load=GRSBatchEntries.aspx', '_top');
            }
            else if (top.isAuthenticated())
            {   
                this.init_state();
            }
            else
            {
                spawn_errpage("This module requires login!");
            }      
        }
        
         this.get_ddlvalue = function(objid,istext)
        {
            var cbo     =  document.getElementById(objid);
            var indx    =  cbo.selectedIndex;
            
            
            try
            {
                if (istext)
                return cbo[indx].innerText;
                else
                return cbo[indx].value;
            }
            catch(e)
            {
                if (istext)
                return cbo.innerText;
                else
                return cbo.value;
            }
        }
        this.get_total_ctr = function()
        {
            return document.getElementById("txtbatchitems").value;
        }
        this.init_state = function()
        {
            this.hide_all_panel();
            this.toggle_sidebar_menu("");
            this.showhide_elements('diviinquiry',false);
            this.showhide_elements('divbatchinfo',true);
            
            ajax_show_status(true);   
            datatable_lookup = IRSWorld.GRS.GRSBatchEntries.load_lookup_values();
            document.getElementById("txtBatchDate").value = document.getElementById("hfTranxDate").value;
            if (datatable_lookup != null)            
            {
                this.fileformat_ddl_callback(datatable_lookup.value.Tables[0]);                
                this.searchfileformat_ddl_callback(datatable_lookup.value.Tables[0]);                
                this.ratetype_ddl_callback(datatable_lookup.value.Tables[1]);                
                ajax_show_status(false);
            }
            else
            {
                alert("Unable to load Lookup!");
            }
            ajax_show_status(false);   
        }
        
       this.clear_search_entry = function()
       {
            document.getElementById("txtbatchnum").value = '';
       }
        this.showhide_elements = function(objids,ishide)
        {
            var sarr = objids.split("|");
            for(i = 0; i < sarr.length; i++)
            {
                if (sarr[i].length > 0)
                try
                {
                    
                    document.getElementById(sarr[i]).style.display  = ((ishide) ? "none": "block");                
                }
                catch(e)
                {
                    //alert(e)
                }
            }            
        }
        this.hide_all_panel = function()
        {
            this.showhide_elements(panels.join("|"),true);
        } 
        this.hide_sidebar = function()
        {
            document.getElementById("trCreateBatch").style.display = "none";
            document.getElementById("trSearchBatch").style.display = "none";
            document.getElementById("trSearchCIF").style.display = "none";
            document.getElementById("trAddCIF").style.display = "none";
        }              
        
         this.create_new_batch = function()
        {            
            this.hide_all_panel();    
            //document.style.cursor = 'hourglass';
            ajax_show_status(true);                   
//            if (datatable_lookup != null)            
//            {
//                this.fileformat_ddl_callback(datatable_lookup.value.Tables[0]);                
//                ajax_show_status(false);
//            }
            this.showhide_elements('divicreatebatch',false);
            ajax_show_status(false);                   
        }   
        
        this.search_batch = function()
        {            
            this.hide_all_panel();
            this.toggle_sidebar_menu("");
            ajax_show_status(true);                               
            this.showhide_elements('diviinquiry',false);
            alert("");
            
        }   
        
        this.ratetype_ddl_callback = function(result)
        {
            common.to_load_dropdownlist('ddlRateType',null, "VALUE", "CODE",result);
        }
        
        this.fileformat_ddl_callback = function(result)
        {
            common.to_load_dropdownlist('ddFileFormat', null, "VALUE", "CODE",result);
            ajax_show_status(false);
        }
        this.searchfileformat_ddl_callback = function(result)
        {
            common.to_load_dropdownlist('ddSFileFormat', null, "VALUE", "CODE",result);
            ajax_show_status(false);
        }   
        this.toggle_sidebar_menu = function(mode)
        {
           
        }
        
        this.prepare_new_batch = function()
        {            
            
            if (this.validate_batch_header())
            {
                if (confirm("Create Batch?"))
                {
                    this.flag_mode = "ADD";                    
                    this.construct_batch_items();
                }
            }
        }  
        
        this.construct_batch_items = function()
        {
               var totitems = document.getElementById("txtbatchitems").value ;
               var txttotamt = document.getElementById("txttotremamt").value;               
               var fileformat = this.get_ddlvalue('ddFileFormat',false);                
               var filetype = "3"
               var  ratetype  = this.get_ddlvalue('ddlRateType',false);                
               ajax_show_status(true);               
               
               IRSWorld.GRS.GRSBatchEntries.construct_batch_items(totitems,txttotamt,fileformat,filetype,ratetype,ide.construct_batch_items_callback)                                  
        }
        
     
        this.construct_batch_items_callback = function(result)
        {   
            try
            {         
            var stemp =  result.value.toString();
            }
            catch(e)
            {
                ajax_show_status(false);  
                alert("Session was terminated.")
                return;
            }
            
            
            if (isNaN(stemp) )
            {
                alert(stemp);
            }
            else
            {
                if (    parseInt(stemp) != 0  && stemp != -1    )
                {
                    // passed values to OTC
                    
                    location.href = "../Remittance/RemOTC.aspx";
                    
                }
                else
                {
                    alert("No Setup define for this batch");
                }
            }
            ajax_show_status(false);            
        }
        
        this.load_batch_header_info = function(datatable)
        {
            var rows = datatable.Rows[0];
        }
       
        this.validate_batch_header = function()
        {
            var txtitems = document.getElementById("txtbatchitems"); 
            var txttotamt = document.getElementById("txttotremamt"); 
            
            
            if (document.getElementById("ddFileFormat").selectedIndex == 0)
            {
                alert("Please select File Format.");
                return false;   
            }
            
            if (document.getElementById("ddlRateType").selectedIndex == 0)
            {
                alert("Please select Rate Type.");
                return false;   
            }
            
            if (isNaN(txtitems.value) || txtitems.value.length == 0)
            {
                alert("Please input no of items.");
                txtitems.focus();                
                return false;
            }
            if (parseInt(txttotamt) >= 0)
            {
                alert("Please input total amount.");
                txttotamt.focus();                
                return false;
            }
            
            return true;
        }        
          
        this.btnCancelCreate_click = function()
        {
            this.init_state();
        }
        this.lnkSearch_click = function()
        {
            this.init_state();
        }
     
        this.btnSearch_click = function()
        {
            var fileformat  = this.get_ddlvalue("ddSFileFormat",false);
            var tranxdate   = document.getElementById("txtBatchDate").value;
            var batchnum    = document.getElementById("txtbatchnum").value; 
                       
            
            
            if (this.validate_search(fileformat,tranxdate,batchnum) == true)
            {            
            IRSWorld.GRS.GRSBatchEntries.load_batch_info(tranxdate, batchnum,fileformat, ide.search_batch_callback)
            }
        }
        
        this.validate_search = function(ff,td,bn)
        {
            if (ff.length == 0)
            {
                alert("please choose fileformat.")
                document.getElementById("ddSFileFormat").focus();
                return false;
            }
            if (td.length == 0)
            {
                alert("please choose date.")
                document.getElementById("txtBatchDate").focus();
                return false;
            }
            
            if (bn.length == 0)
            {
                alert("please enter batch number.")
                document.getElementById("txtbatchnum").focus();
                return false;
            }
            return true;
        }
        this.btnAddItem_click = function()
        {
            var fileformat  = this.get_ddlvalue("ddSFileFormat",false);
            var tranxdate   = document.getElementById("txtBatchDate").value;
            var batchnum    = document.getElementById("txtbatchnum").value;                        
            IRSWorld.GRS.GRSBatchEntries.select_batch(tranxdate,fileformat,"3",batchnum, ide.addItem_callback)
        }
        
        this.addItem_callback = function(result)
        {
            
            try
            {
                if (result != null && result != "undefined")
                {
                
                    var s = result.value.toString();
                    
                    if (s.indexOf("ERROR") > -1)
                    {
                        alert(s)
                        return;
                    }
                    else
                    {
                        location.href = "../Remittance/RemOTC.aspx";
                    }
                }
            }
            catch(e)
            {
                alert(e);
            }
        }
        
         this.create_search_grid = function()
        {
            gv = new datagrid();     
            {
		        var col = new datagrid_column(); // 0
		        col.header = 'Batch No.';
		        col.datafield = 'batchnum'; //used in alias SQL
		        col.align = 'right';
		        col.styles.cell += 'width:auto;text-align:right;';
		        col.editable = false;
		        gv.addColumn(col);
            }   
            {
		        var col = new datagrid_column(); // 0
		        col.header = 'REMITTER';
		        col.datafield = 'remittername'; //used in alias SQL
		        col.align = 'right';
		        col.styles.cell += 'width:auto;text-align:right;';
		        col.editable = false;
		        gv.addColumn(col);
            }
           {
		        var col = new datagrid_column(); // 1
		        col.header = 'BENEFICIARY';
		        col.datafield = 'benename';
		        col.align = 'right';
		        col.editable = false;
		        col.styles.cell += 'width:auto;text-align:right;';
		        gv.addColumn(col);
            }
            {
		        var col = new datagrid_column(); // 2
		        col.header = 'AMOUNT';
		        col.datafield = 'amount';
		        col.align = 'right';			        		
		        col.styles.cell += 'width:auto;text-align:right;';
		        gv.addColumn(col);
            }
           
                // for footer                     
                gv.row.onclick = 'ide.btnAddItem_click'
                gv.row.normal_style = 'background-color: #f7ffff;';                      
                return gv;
         }
         
          this.search_batch_callback = function(result)
        {
            if (result != null && result != "undefined")
            {
                if (result.value != null)
                {
                    try
                    {
                        dtuser = result.value.Tables[0];    
                        if (dtuser.Rows.length <= 0)   
                        {
                            alert("BATCH DOES NOT EXISTS");
                            return;
                        }
                        if (dtuser.Rows[0].RMSG.toString().indexOf("OK") < 0)
                        {
                            alert("BATCH CURRENTLY IN USE");
                            return;
                        }
                        
                        if (parseInt(dtuser.Rows[0].batch_items) <= parseInt(dtuser.Rows[0].curr_items))
                        {
                            document.getElementById("btnAddItem").style.display = "none";
                        }
                        else
                        {
                            document.getElementById("btnAddItem").style.display = "block";
                        }
                        datatable = result.value.Tables[1];       
                        dbgrid = ide.create_search_grid();                        
                        dbgrid.filter.enabled = false;            
                        
                        dbgrid.bind(datatable, 'tdbatchinfo', 'tblBATCH');            
                        ide.show_search_grid();            
                        return;
                    }
                    catch(e)
                    {
                        
                    }
                }
               
            }
            alert(result.error);
           
        }
        this.show_search_grid = function()
        {
            
            this.showhide_elements('divbatchinfo',false);   
        }
        
    }
  
 

function showPanel(divName)
{
    var div = document.getElementById(divName);
    
    divmask = new maskpanel();
    divmask.show();
    showDiv(divName, true);
    
    div.style.top = (((document.documentElement.clientHeight/2) + document.documentElement.scrollTop) - (div.offsetHeight/2)) + 'px';
    div.style.left = (((document.documentElement.clientWidth/2) + document.documentElement.scrollLeft) - (div.offsetWidth/2)) + 'px';
}

function hidePanel(divName)
{
    showDiv(divName, false);
    divmask.hide();
}
function showDiv(element, isVisible)
{
    if (isVisible == true)
    {
        document.getElementById(element).style.display = 'block';
    }
    else
    {
        document.getElementById(element).style.display = 'none';
    }
}
function internetClose()
{
    if (ide != null)
    {
        if (self.returnValue != null && self.returnValue != "0")
        {
            ide.close_client(self.returnValue);
            
        }
    }
}
function execute_ajx_command(param)
{
    if (ide != null && ide != "undefined")
    
    {
        cif.do_ajax(param);
    }
}

