    var internet_page = new internet_page();
    var cif = new js_CIFmisc();
    var ide =   new js_ibatchdataentry();
    var ajax_grid = null;
    var sb =   new SideBar();
    var flag_mode = "";
    var tab_mode = "DEBIT";
    var datatable_lookup;
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
                new Array(3, 0, 'Search Batch', 'ide.search_batch()') ,               
                new Array(4, 0, 'Search CIF', 'internet_page.searchCIF(true)') ,               
                new Array(5, 0, 'Create CIF', "show_frame_dialog('../Internet/InternetRemitter.aspx?isRemit=true', 'internetClose', '100%', '100%', '0')")                
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
    
    
    function js_ibatchdataentry()
    {
        var panels = new Array(7);
        var common = new common_method();
        var cls = this;
        var datatable ;
        panels[0] = 'diviinquiry';
        panels[1] = 'diviDebit';
        panels[2] = 'diviCredit';
        panels[3] = 'diviComputation';
        panels[4] = 'divibatchholder';
        panels[5] = 'diviCharges';
        panels[6] = 'divisummary';
        panels[7] = 'divicreatebatch';
        
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
            
            this.showhide_elements(panels[0],false);
          
            ajax_show_status(true);   
            datatable_lookup = IRSWorld.GRS.GRSDataEntry.load_lookup_values();
            ajax_show_status(false);
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
        this.add_record = function()
        {
            
        }
        this.edit_record = function()
        {
            
        }        
         this.create_new_batch = function()
        {            
            this.hide_all_panel();    
            //document.style.cursor = 'hourglass';
            ajax_show_status(true);                   
            if (datatable_lookup != null)            
            {
                
                this.fileformat_ddl_callback(datatable_lookup.value.Tables[0]);
                this.ratetype_ddl_callback(datatable_lookup.value.Tables[1]);
                this.rembank_ddl_callback(datatable_lookup.value.Tables[2]);
                this.protbank_ddl_callback(datatable_lookup.value.Tables[2]);
                this.product_ddl_callback(datatable_lookup.value.Tables[4]);
                this.drremcurr_ddl_callback(datatable_lookup.value.Tables[5]);
                this.cracctcurr_ddl_callback(datatable_lookup.value.Tables[6]);
                ajax_show_status(false);
            }
            this.showhide_elements('divicreatebatch',false);
        }   
        this.ratetype_ddl_callback = function(result)
        {
            common.to_load_dropdownlist('ddlRateType',null, "VALUE", "CODE",result);
        }
        this.fileformat_ddl_callback = function(result)
        {
            common.to_load_dropdownlist('ddFileFormat', null, "VALUE", "CODE",result);
        }
        this.rembank_ddl_callback = function(result)
        {
            common.to_load_dropdownlist('ddRemBank',null, "VALUE", "CODE",result);
        }
        this.protbank_ddl_callback = function(result)
        {
            common.to_load_dropdownlist('ddProtBank',null, "VALUE", "CODE",result);
        }
        this.product_ddl_callback = function(result)
        {
            common.to_load_dropdownlist('ddProdCode',null, "VALUE", "CODE",result);
        }
        this.drremcurr_ddl_callback = function(result)
        {
            common.to_load_dropdownlist('ddDRRemCurr',null, "VALUE", "CODE",result);
        }
        this.cracctcurr_ddl_callback = function(result)
        {
            common.to_load_dropdownlist('ddCRAcctCurr',null, "VALUE", "CODE",result);
        }
        this.toggle_sidebar_menu = function(mode)
        {
            if ( mode == "BATCH")
            {
                document.getElementById("trCreateBatch").style.display = "none";
                document.getElementById("trSearchBatch").style.display = "none";
                document.getElementById("trSearchCIF").style.display = "block";
                document.getElementById("trAddCIF").style.display = "block";
            }
            else
            {
                document.getElementById("trCreateBatch").style.display = "block";
                document.getElementById("trSearchBatch").style.display = "block";
                document.getElementById("trSearchCIF").style.display = "none";
                document.getElementById("trAddCIF").style.display = "none";  
            }
        }
        this.prepare_new_batch = function()
        {            
            
            if (this.validate_batch_header())
            {
                if (confirm("Create Batch?"))
                {
                    this.flag_mode = "ADD";
                    this.clear_records();
                    this.construct_batch_items();
                }
            }
        }  
        this.construct_batch_items = function()
        {
               var totitems = document.getElementById("txtbatchitems").value ;
               var txttotamt = document.getElementById("txttotremamt").value;
               var ratetype = this.get_ddlvalue('ddlRateType',false);               
               var fileformat = this.get_ddlvalue('ddFileFormat',false); 
               var guid = this.create_guid();                
               document.getElementById("txtctr").value = 1;
               document.getElementById("txttotitems_ctr").value = totitems;               
               ratetype = "1";
               document.getElementById("hfGUID").value = guid.value.toString();
               ajax_show_status(true);
               IRSWorld.GRS.GRSDataEntry.construct_batch_items(guid.value.toString(),totitems,txttotamt,ratetype,fileformat,ide.construct_batch_items_callback)
               
                   
        }
        this.create_guid = function()
        {
            return IRSWorld.GRS.GRSDataEntry.create_guid()
        }
        
        this.construct_batch_items_callback = function(result)
        {
            ide.datatable = result;   
            ajax_show_status(false);
            ide.hide_all_panel();  
            ide.toggle_sidebar_menu("BATCH");          
            ide.showhide_elements('divibatchholder|diviDebit',false);
        }
        this.load_batch_header_info = function(datatable)
        {
            var rows = datatable.Rows[0];
            
        }
        this.load_batch_record = function(datatable)
        {
            try
            {
                ide.load_debit_records(datatable);
                ide.load_credit_records(datatable);
                ide.load_charges_records(datatable);
                ide.load_summary_records(datatable);
                ide.showhide_elements('divibatchholder|diviCredit',false);        
            }
            catch(e)
            {
                alert("cannot load transactioh details");
            }
            
            
        }
        this.load_debit_records = function(datatable)
        {
            var rows = databale.Rows[0];
            document.getElementById("txtOutSeqno").value = rows.OutSeqNo;
            document.getElementById("txtRefnumTheirs").value = rows.RefnumTheirs;            
            document.getElementById("txtDateSent").value = rows.DateSent;
            document.getElementById("txtDateReceived").value = rows.DateReceived;
            document.getElementById("txtRemBankRelCode").value = rows.RemBankRelCode;
            document.getElementById("txtCurrCode").value = rows.CurrCode;
        }        
        this.load_credit_records = function(datatable)
        {
            var rows = databale.Rows[0];
            
            document.getElementById("txtRemId").value = rows.RemId;            
            document.getElementById("txtRemAcct").value = rows.RemAcct;
            document.getElementById("txtRemName").value = rows.RemName;
            document.getElementById("txtRemAdd").value = rows.RemAdd;
            document.getElementById("txtBeneId").value = rows.BeneId;
            document.getElementById("txtBeneAcct").value = rows.BeneAcct;
            document.getElementById("txtBeneName").value = rows.BeneName;
            document.getElementById("txtBeneAdd").value = rows.Address
            document.getElementById("txtWType").value = rows.Wtype;
            document.getElementById("txtBeneAcctStat").value = rows.BeneAcctStat;
        }  
        this.load_charges_records = function(datatable)
        {
               
        }  
        this.load_summary_records = function(datatable)
        {
               
        }  
        this.load_implementation_callback = function(result)
        {
            common.to_load_dropdownlist('ddImplem',null, "VALUE", "CODE",result.value.Tables[0]);
        }
        this.load_implementation = function()
        {
            var prodcode    = this.get_ddlvalue('ddProdCode',false); 
            var remccy      = this.get_ddlvalue('ddDRRemCurr',false); 
            if (remccy.toString().length == 0) 
            {
                alert("Please select remitting Currency");
                document.getElementById("ddDRRemCurr").focus();
                return; 
            }
            IRSWorld.GRS.GRSDataEntry.get_implementation(prodcode,remccy,ide.load_implementation_callback);
            
        }
        this.load_remitter_info = function(param)
        {
            var sparam = param.split("~");
            document.getElementById("txtRemId").value = sparam[1];                        
            document.getElementById("txtRemName").value = sparam[2];
            document.getElementById("txtRemAdd").value = sparam[3];
            document.getElementById("txtRemAcct").value = sparam[4];
            
        }
        this.load_bene_info = function(param)
        {
            var sparam = param.split("~");
            
            document.getElementById("txtBeneId").value = sparam[1];            
            document.getElementById("txtBeneName").value = sparam[2];
            document.getElementById("txtBeneAdd").value = sparam[3]
            document.getElementById("txtBeneAcct").value = sparam[4];
            document.getElementById("txtWType").value = sparam[5];
            document.getElementById("txtBeneAcctStat").value = sparam[6];
            
        }
        this.validate_batch_header = function()
        {
            var txtitems = document.getElementById("txtbatchitems"); 
            var txttotamt = document.getElementById("txttotremamt"); 
            if (isNaN(txtitems.value) || txtitems.value.length == 0)
            {
                alert("Please input no of items.");
                txtitems.focus();                
                return false;
            }
            if (isNaN(txttotamt.value) || txttotamt.value.length == 0)
            {
                alert("Please input total amount.");
                txttotamt.focus();                
                return false;
            }
            return true;
        }
        this.clear_records = function()
        {
            document.getElementById("txtOutSeqno").value = "";
            document.getElementById("txtRefnumTheirs").value = "";            
            document.getElementById("txtDateSent").value = "";
            document.getElementById("txtDateReceived").value = "";                        
            document.getElementById("txtRemId").value = "";
            document.getElementById("txtRemAcct").value = "";
            document.getElementById("txtRemName").value = "";
            document.getElementById("txtRemAdd").value = "";
            document.getElementById("txtBeneId").value = "";
            document.getElementById("txtBeneAcct").value = "";
            document.getElementById("txtBeneName").value = "";
            document.getElementById("txtBeneAdd").value = "";
            document.getElementById("txtWType").value = "";
            document.getElementById("txtBeneAcctStat").value = "";
            document.getElementById("ddDRRemCurr").selectedIndex = 0;
            document.getElementById("ddCRAcctCurr").selectedIndex = 0;            
        }       
        this.btnCancelCreate_click = function()
        {
            this.init_state();
        }
        this.btnSaveTransaction = function()
        {
            if (ide.flag_mode == "ADD")
            {
                this.add_record();                
            }
            if (ide.flag_mode == "EDIT")
            {
                this.edit_record();
            }
        }
        this.close_client = function(windowresult)
        {
            if (windowresult != null)
            {
                
            }
        }
        this.get_remitter_info = function(cifid)
        {
            
        }
        this.get_bene_info = function(cifid)
        {
            
        }
        this.handle_panel_toggle = function(mode)
        {
            this.hide_all_panel();
            this.showhide_elements('divibatchholder',false);
            
            switch (mode)
            {
                case "DEBIT" :
                        this.panel_mode = "DEBIT";
                        document.getElementById('diviDebit').style.display = "block";
                break;
                case "CREDIT" :
                        this.panel_mode = "CREDIT";
                        document.getElementById('diviCredit').style.display = "block";
                break;
                case "CHARGES" :
                        this.panel_mode = "CHARGES";
                        document.getElementById('diviCharges').style.display = "block";
                break;
                case "SUMMARY" :
                        this.panel_mode = "SUMMARY";
                        document.getElementById('diviSummary').style.display = "block";
                break;
                default:
                    alert ("no panel selected");
            }
        }
        this.handle_mouseover = function(objid,mouse_move,htmlobj)
        {
            var obj = event.srcElement;
            //alert(obj);
            switch (mouse_move)
            {
                case "hoverin":
                    obj.style.cursor='hand';
                    obj.className = 'tab_selected';
                break;
                case "hoverout":
                    obj.style.cursor='';
                    obj.className = 'tab_unselected';
                break;
                case "click":                
                    
                    this.hide_all_panel();
                    this.showhide_elements('divibatchholder',false);
                    if (objid.indexOf("Debit") > -1)
                    {
                        this.handle_panel_toggle("DEBIT");
                    }
                    else if (objid.indexOf("Credit") > -1)
                    {
                        this.handle_panel_toggle("CREDIT");
                    }
                    else if (objid.indexOf("Charges") > -1)
                    {
                        this.handle_panel_toggle("CHARGES");
                    }
                    else if (objid.indexOf("Summary") > -1)
                    {
                        this.handle_panel_toggle("SUMMARY");
                    }
                break;
            }
           
        }
   
    }
    
function js_CIFmisc()
{
    
    this.searchCIF = function()
    {   
        showPanel("divSearch");        
    }
    this.btnCancel_click = function()
    {
        hidePanel("divSearch");        
    }
    this.load_cif_info = function()
    {
         hidePanel("divSearch");        
         ajax_grid =  new grid_selector();
        
         var   sp_to_use = "sp_get_cifinfo";
         var   grid_ajax_query = sp_to_use + "~" +  
               ide.get_ddlvalue("ddSearch",false) + "~" + 
               document.getElementById("txtSLastName").value   + "~" + 
               document.getElementById("txtSFirstName").value  + "~" + 
               document.getElementById("txtSMiddleName").value + "~" + 
               document.getElementById("txtSAcctNo").value + "~" + 
               document.getElementById("hfBranchCode").value  +"~";
         ajax_grid.open_selection_grid(null, grid_ajax_query, 0, null);
    }
     this.do_ajax = function(parameter)
    {   
       if (parameter != null && parameter != "undefined")
       {
            var command_param = parameter.split("$");
            if (command_param.length > 0)
            {
                if (command_param[0] == "ajx_selected")
                {                    
                    var classtype = ide.get_ddlvalue("ddlSearchBy",false);
                    this.handle_search(classtype,parameter)
                }
            }
        }                
       ajax_grid.process_ajax(parameter);
    }
    this.handle_search = function(classtype,params)
    {
        ide.hide_all_panel();
        var command_param = params.split("$");
        if (classtype == 1)
        {    //remitter
            ide.load_remitter_info(command_param[1])            
        }
        else
        {
            ide.load_bene_info(command_param[1])
        }
        ide.handle_panel_toggle("CREDIT");
        
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