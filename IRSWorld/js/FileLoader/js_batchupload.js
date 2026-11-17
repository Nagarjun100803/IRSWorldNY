var btns = "";
var ftpGrid;
var type;
var myFedAchVar = null;
var fedAchFilename = null;

function js_batchupload()
{
    this.cls            = this;
    this.js_com         = null;    
    this.dsfile_types   = null;
    this.dsfile_formats = null;
    this.sfile_type     = null;
    this.err_msg        = null;
    
    this.get_roledesc   =   function()
    {
        return document.getElementById("hfuserrole").value;
    }
    
    this.filter_ddlindx  = function(objid,searchtxt)
    {
        var cbo     =  document.getElementById(objid);
        for(i=0;i< cbo.length;i++)
        {
            var s = cbo[i].value.toString();
            if (s.indexOf(searchtxt) > -1)
            {
                cbo.selectedIndex = i;
                return;
            }
        }
        return;
    }
    
    this.filter_ddlvalue  = function(objid,searchtxt,istext)
    {
        var cbo     =  document.getElementById(objid);
        for(i=0;i< cbo.length;i++)
        {
            var s = cbo[i].value.toString();
            if (s.indexOf(searchtxt) > -1)
            {
                if(istext)
                {
                    return cbo[i].text;
                }
                else
                return cbo[i].value;
            }
        }
        return;
    }
    
    this.get_ddlvalue = function(objid,istext)
    {
        var cbo     =  document.getElementById(objid);
        var indx    =  cbo.selectedIndex;
        
        try
        {
            
            if (indx > -1)
            {
                if (istext)
                return cbo[indx].text;
                else
                return cbo[indx].value;
            }
        }
        catch(e)
        {
            if (indx > -1)
            {
                if (istext)
                return cbo.text;
                else
                return cbo.value;
            }
        }
        return 0;
    }
    
    this.up_type_select = function()
    {   
        
        upf.sfile_type = document.getElementById('ddFileTypeUpload')[document.getElementById('ddFileTypeUpload').selectedIndex].text;
        var ifile_type  = document.getElementById('ddFileTypeUpload')[document.getElementById('ddFileTypeUpload').selectedIndex].value;
        if (ifile_type.toString() != "3" && ifile_type.toString() != "7")
        {
            document.getElementById("ddlRateType").selectedIndex = 1;
            document.getElementById("pnlFTPFiles").innerHTML = '';
            document.getElementById("tdRateRow").style.display = "none";        
            document.getElementById("btn_show_summary").disabled = true;
            //document.getElementById("tdSummary").style.display = "none";
        }       
        else
        {
            document.getElementById("ddlRateType").disabled = false;
            document.getElementById("btn_show_summary").disabled = false;
            //document.getElementById("tdSummary").style.display = "block";
        }
        
        AjaxPro.timeoutPeriod = 1000 * 1000;

        AjaxPro.onTimeout = function()
        {
            alert("THE SERVER STOPPED RESPONDING. PLEASE TRY AGAIN");
            //this.close_msg_prompt(); setText("lbl_otc_caption", "SERVER NOT RESPONDING. CHECKING AVAILABILITY OF CURRENCY RATES FAILED."); alert("Server not responding. Checking availability of currency rates failed.");
        }
        
        IRSWorld.IRSFileLoader.BatchUploader.file_format(upf.sfile_type, upf.up_type_select_callback);
    }
    
    this.up_type_select_callback = function(result)
    {
        if (result != null && result != undefined)
        {
            new common_method().to_load_dropdownlist('ddFileFormatUpload', result.value, "VALUE", "CODE");
            if (result.value.Tables[0].Rows.length == 1) select_index('ddFileFormatUpload', 1);
        }
    }
    
    this.vw_type_select = function()
    {        
        AjaxPro.timeoutPeriod = 1000 * 1000;

        AjaxPro.onTimeout = function()
        {
            alert("THE SERVER STOPPED RESPONDING. PLEASE TRY AGAIN");
            //this.close_msg_prompt(); setText("lbl_otc_caption", "SERVER NOT RESPONDING. CHECKING AVAILABILITY OF CURRENCY RATES FAILED."); alert("Server not responding. Checking availability of currency rates failed.");
        }
        upf.sfile_type = document.getElementById('ddFileTypeSearch')[document.getElementById('ddFileTypeSearch').selectedIndex].text;
        IRSWorld.IRSFileLoader.BatchUploader.file_format(upf.sfile_type, upf.vw_type_select_callback);
    }
    
    this.vw_type_select_callback = function()
    {
        if (result != null && result != undefined)
        {
            new common_method().to_load_dropdownlist('ddFileFormatSearch', result.value, "VALUE", "CODE");
        }
    }
    
    this.view_details = function()
    {
        document.getElementById("ddFileFormatSearch").options.length = 0;
        hide_elements('divUploadFile~divDetails');
        show_elements('divSearch');
    }

    /* RFC-32416 OFAC UPLOADING SEQUENCE VALIDATION */
    this.allow_ofac_files = function(compliance_type, ofac_type, file_name)
    {
        var allowed = "";

        try
        {
            allowed = IRSWorld.IRSFileLoader.BatchUploader.ofac_validation_routine(compliance_type, ofac_type, file_name).value;

            return allowed;
        }
        catch(e)
        {
            return "Error in uploading. Please retry.";
        }
    }
    /* RFC-32416 OFAC UPLOADING SEQUENCE VALIDATION */
    
    this.file_browse = function()
    {
        var type        = upf.get_ddlvalue("ddFileTypeUpload",false);
        var format      = upf.get_ddlvalue("ddFileFormatUpload",false);
        var filename    = getFileName('fileBrowse');
        var ratetype = document.getElementById('ddlRateType').selectedIndex;

        /* RFC-32416 OFAC UPLOADING SEQUENCE VALIDATION */
        var is_allowed_ofac = this.allow_ofac_files(type, format, filename);

        if (is_allowed_ofac != null && is_allowed_ofac != " ")
        {
            alert(is_allowed_ofac); return;
        }
        /* RFC-32416 OFAC UPLOADING SEQUENCE VALIDATION */

        if (type < 1 || type == null || type.toUpperCase() == "NULL")
        {
            alert('Please select file type.'); return;
        }
        if (format < 1 || format == null || format.toUpperCase() == "NULL")
        {
            alert('Please select file format.'); return;
        }
        
        if (format.toUpperCase() != "NULL" && format == "6")
        {
            if ((filename.toUpperCase().indexOf("CONS") >= 0 && filename.toUpperCase().indexOf("SDN") == -1 && filename.toUpperCase().indexOf(".PIP") >= 0)
                || (filename.toUpperCase().indexOf("CONS") == -1 && filename.toUpperCase().indexOf("SDN") == -1 && filename.toUpperCase().indexOf(".PIP") >= 0)
                || (filename.toUpperCase().indexOf("CONS") == -1 && filename.toUpperCase().indexOf("SDN") == -1 && filename.toUpperCase().indexOf(".PIP") == -1))
            {
                alert('Please select a valid SDN file.'); return;
            }
        }

        if (format.toUpperCase() != "NULL" && format == "7")
        {
            if ((filename.toUpperCase().indexOf("CONS") >= 0 && filename.toUpperCase().indexOf("ADD") == -1 && filename.toUpperCase().indexOf(".PIP") >= 0)
                || (filename.toUpperCase().indexOf("CONS") >= 0 && filename.toUpperCase().indexOf("ADD") >= 0 && filename.toUpperCase().indexOf(".PIP") >= 0)
                || (filename.toUpperCase().indexOf("CONS") == -1 && filename.toUpperCase().indexOf("ADD") == -1 && filename.toUpperCase().indexOf(".PIP") >= 0)
                || (filename.toUpperCase().indexOf("CONS") == -1 && filename.toUpperCase().indexOf("ADD") == -1 && filename.toUpperCase().indexOf(".PIP") == -1))                
            {
                alert('Please select a valid ADD file.'); return;
            }    
        } 
        
        if (format.toUpperCase() != "NULL" && format == "8") 
        {
            if ((filename.toUpperCase().indexOf("CONS") >= 0 && filename.toUpperCase().indexOf("ALT") == -1 && filename.toUpperCase().indexOf(".PIP") >= 0)
                || (filename.toUpperCase().indexOf("CONS") >= 0 && filename.toUpperCase().indexOf("ALT") >= 0 && filename.toUpperCase().indexOf(".PIP") >= 0)
                || (filename.toUpperCase().indexOf("CONS") == -1 && filename.toUpperCase().indexOf("ALT") == -1 && filename.toUpperCase().indexOf(".PIP") >= 0)
                || (filename.toUpperCase().indexOf("CONS") == -1 && filename.toUpperCase().indexOf("ALT") == -1 && filename.toUpperCase().indexOf(".PIP") == -1))
            {
                alert('Please select a valid ALT file.'); return;
            }                
        }
        
        if (format.toUpperCase() != "NULL" && format == "30")
        {
            if ((filename.toUpperCase().indexOf("CONS") >= 0 && filename.toUpperCase().indexOf("PRIM") == -1 && filename.toUpperCase().indexOf(".PIP") >= 0)
                || (filename.toUpperCase().indexOf("CONS") == -1 && filename.toUpperCase().indexOf("PRIM") == -1 && filename.toUpperCase().indexOf(".PIP") >= 0)
                || (filename.toUpperCase().indexOf("CONS") == -1 && filename.toUpperCase().indexOf("PRIM") == -1 && filename.toUpperCase().indexOf(".PIP") == -1))
            {
                alert('Please select a valid CONSO-PRIM file.'); return;
            }    
        }

        if (format.toUpperCase() != "NULL" && format == "31")
        {
            if ((filename.toUpperCase().indexOf("CONS") >= 0 && filename.toUpperCase().indexOf("ADD") == -1 && filename.toUpperCase().indexOf(".PIP") >= 0)
                || (filename.toUpperCase().indexOf("CONS") == -1 && filename.toUpperCase().indexOf("ADD") >= 0 && filename.toUpperCase().indexOf(".PIP") >= 0)
                || (filename.toUpperCase().indexOf("CONS") == -1 && filename.toUpperCase().indexOf("ADD") == -1 && filename.toUpperCase().indexOf(".PIP") >= 0)
                || (filename.toUpperCase().indexOf("CONS") == -1 && filename.toUpperCase().indexOf("ADD") == -1 && filename.toUpperCase().indexOf(".PIP") == -1))
            {
                alert('Please select a valid CONSO-ADD file.'); return;
            }    
        }        
        
       if (format.toUpperCase() != "NULL" && format == "32")
        {
            if ((filename.toUpperCase().indexOf("CONS") >= 0 && filename.toUpperCase().indexOf("ALT") == -1 && filename.toUpperCase().indexOf(".PIP") >= 0)
                || (filename.toUpperCase().indexOf("CONS") == -1 && filename.toUpperCase().indexOf("ALT") >= 0 && filename.toUpperCase().indexOf(".PIP") >= 0)
                || (filename.toUpperCase().indexOf("CONS") == -1 && filename.toUpperCase().indexOf("ALT") == -1 && filename.toUpperCase().indexOf(".PIP") >= 0)
                || (filename.toUpperCase().indexOf("CONS") == -1 && filename.toUpperCase().indexOf("ALT") == -1 && filename.toUpperCase().indexOf(".PIP") == -1))
            {
                alert('Please select a valid CONSO-ALT file.'); return;
            }    
        }         
        
        var ini = IRSWorld.IRSFileLoader.BatchUploader.upload_ini(parseInt(type), parseInt(format), filename, ratetype.toString());
        
        if (ini.value)
        {
            document.getElementById('btnPost').click();
        }
        else
        {
            AjaxPro.timeoutPeriod = 1000 * 1000;

            AjaxPro.onTimeout = function()
            {
                alert("THE SERVER STOPPED RESPONDING. PLEASE TRY AGAIN");
                //this.close_msg_prompt(); setText("lbl_otc_caption", "SERVER NOT RESPONDING. CHECKING AVAILABILITY OF CURRENCY RATES FAILED."); alert("Server not responding. Checking availability of currency rates failed.");
            }
            IRSWorld.IRSFileLoader.BatchUploader.uploader_msg(upf.msg_callback);
        }
    }
    
    this.search_file = function()
    {
        var searchval = document.getElementById("txtsearch").value;
        if (searchval.length == 0)
        {
            dbgrid.clearFilter();            
            dbgrid.filter.enabled = false;
            dbgrid.refresh(datatable);
            hide_elements('tblShowUpload_footerbtn_Edit');
            return;
        }
        dbgrid.clearFilter();            
        dbgrid.filter.enabled = true;
        var filter = new datagrid_filter();
        filter.datafield = 'FILENAME';
        filter.operator = '!=';
        filter.value = "'" + searchval + '@' + "' && row.FILENAME.toString().toLowerCase().indexOf('" +  searchval.toLowerCase() + "') > -1";
        dbgrid.addFilter(filter);
        dbgrid.refresh(datatable);
        hide_elements('tblShowUpload_footerbtn_Edit');
    }
    
    this.search_uploaded_file = function()
    {
        var searchval = document.getElementById("txtsearchupload").value;
        if (searchval.length == 0)
        {
            dbgrid.clearFilter();            
            dbgrid.filter.enabled = false;
            dbgrid.refresh(datatable);
            hide_elements('tblShowUpload_footerbtn_Edit');
            return;
        }
        dbgrid.clearFilter();            
        dbgrid.filter.enabled = true;
        var filter = new datagrid_filter();
        filter.datafield = 'FILES';
        filter.operator = '!=';
        filter.value = "'" + searchval + '@' + "' && row.Files.toString().toLowerCase().indexOf('" +  searchval.toLowerCase() + "') > -1";
        dbgrid.addFilter(filter);
        dbgrid.refresh(datatable);
        hide_elements('tblShowUpload_footerbtn_Edit');
    }
    
    this.show_loading_msg = function(msg)
    {
        new common_method().to_show_loading_message('divLoadingMsg');
        setText(document.getElementById('lblLoadingMsg'), msg);
        showpanel('divPopProcess');
    }
    
    this.btnUpload_click = function()
    {
        var type        = upf.get_ddlvalue("ddFileTypeUpload",false);
        var userrole    = new js_batchupload().get_roledesc();
        var ratetype;
        
        if (userrole.indexOf("AGENT") > -1 || type != 3)
        {
            ratetype = -1;
        }
        else
        {
            ratetype = document.getElementById("ddlRateType").value.toString();
        }
        
        var r = ratetype;
        
        if (isNaN(r))
        {
            alert("Please select rate type.");
            document.getElementById("ddlRateType").focus();
            return;
        }
        
        if (!confirm("Continue with Upload?"))
        {
            return;
        }
        
        var format      = upf.get_ddlvalue("ddFileFormatUpload",false);
        var file_path   = document.getElementById("fileBrowse").value;
        var totrec      = getText(document.getElementById("recordCount"));
        var runamt      = getText(document.getElementById("runamount"));
        if (type > 0)
        {
            if (format > 0)
            {
                var userrole = new js_batchupload().get_roledesc();
                if (userrole.indexOf("AGENT") > -1)
                {
                    ratetype = 0;
                }
                
                AjaxPro.timeoutPeriod = 1000 * 1000;

                AjaxPro.onTimeout = function()
                {
                    alert("THE SERVER STOPPED RESPONDING. PLEASE TRY AGAIN");
                    //this.close_msg_prompt(); setText("lbl_otc_caption", "SERVER NOT RESPONDING. CHECKING AVAILABILITY OF CURRENCY RATES FAILED."); alert("Server not responding. Checking availability of currency rates failed.");
                }
        
                IRSWorld.IRSFileLoader.BatchUploader.save_file(parseInt(type), parseInt(format), r.toString(), totrec, runamt, upf.upload_callback);
                upf.show_loading_msg('Uploading, Please wait...');
            }
            else
            {
                alert("Please select file format!");
            }
        }
        else
        {
            alert("Please select file type!");
        }
    }
    
    this.upload_callback = function(result)
    {
        var i = 0;
//        if (result.value)
//        {
//            IRSWorld.IRSFileLoader.BatchUploader.uploader_msg(upf.msg_callback);
//        }
//        else
//        {
        var msg = IRSWorld.IRSFileLoader.BatchUploader.uploader_msg();
        upf.msg_callback(msg);
        //}
        hidepanel('divLoadingMsg');
        hidepanel('divPopProcess');
        hide_elements('divAddlField~divfilename');
        show_elements('divfileBrowse');
    }
    
    this.msg_callback = function(result)
    {
        alert(result.value);
        hidepanel('divPopProcess');
        page_load();
    }
    
    this.btnProcess_Click = function()
    {
        var ratetype = document.getElementById("ddlRateType");
        var r = ratetype.value.toString();
        
        if (isNaN(r))
        {
            alert("Please select rate type.");
            ratetype.focus();
            return ;
        }
        
        AjaxPro.timeoutPeriod = 1000 * 1000;

        AjaxPro.onTimeout = function()
        {
            alert("THE SERVER STOPPED RESPONDING. PLEASE TRY AGAIN");
            //this.close_msg_prompt(); setText("lbl_otc_caption", "SERVER NOT RESPONDING. CHECKING AVAILABILITY OF CURRENCY RATES FAILED."); alert("Server not responding. Checking availability of currency rates failed.");
        }
        
        IRSWorld.IRSFileLoader.BatchUploader.process_file(r, upf.process_callback);
        new common_method().to_show_loading_message('divLoadingMsg');
        setText(document.getElementById('lblLoadingMsg'));
        showpanel('divPopProcess');
    }
    
    this.process_callback = function(result)
    {
      
        IRSWorld.IRSFileLoader.BatchUploader.uploader_msg(upf.msg_callback);
        upf.reset_objects();
        
        hide_elements('divAddlField~divfilename');
        show_elements('divfileBrowse');
    }
    
    /*for summary*/
    this.show_summary = function()    
    {
        var type        = document.getElementById('ddFileTypeUpload')[document.getElementById('ddFileTypeUpload').selectedIndex].text;//this.get_ddlvalue("ddFileTypeUpload",false);
        var format      = document.getElementById('ddFileFormatUpload')[document.getElementById('ddFileFormatUpload').selectedIndex].text;//this.get_ddlvalue("ddFileFormatUpload",false);
        var filename    = getText(document.getElementById('divfilename'));//document.getElementById('divfilename').innerText;
        var ratetype    = upf.get_ddlvalue("ddlRateType",false);
        
        AjaxPro.timeoutPeriod = 1000 * 1000;

        AjaxPro.onTimeout = function()
        {
            alert("THE SERVER STOPPED RESPONDING. PLEASE TRY AGAIN");
            //this.close_msg_prompt(); setText("lbl_otc_caption", "SERVER NOT RESPONDING. CHECKING AVAILABILITY OF CURRENCY RATES FAILED."); alert("Server not responding. Checking availability of currency rates failed.");
        }
        IRSWorld.IRSFileLoader.BatchUploader.show_summary(type, format, filename, ratetype.toString(), upf.show_summary_callback);
    }
    
    this.show_summary_callback = function(result)
    {
        upf.load_datagrid(result);
    }
    
    this.load_datagrid = function(result)
    {
        
        try
        {
            datatable = result.value.Tables[0];
            var cur = datatable.Rows[0].CURRENCY;
            if (cur.length > 0)
            {
                var amt = datatable.Rows[0].AMOUNT;
                if (parseInt(amt) > 0)
                {
                    dbgrid = upf.create_grid();                        
                    dbgrid.filter.enabled = false;            
                    dbgrid.bind(datatable, 'pnlPreSummary', 'tblTEMPFILE');            
                    upf.show_grid();
                    return;
                }
            }
         }
         catch(e)
         {
            
         }
         alert("Unable to read file.\nThe type and/or format may not be intended to the file you are uploading.");
         upf.cancel_click();
    }
    
    this.cancel_click = function()
    {
        upf.reset_objects();
        hide_elements('divAddlField~divfilename~pnlFTPFiles~divShowUpload');
        show_elements('divfileBrowse~divUploadFile');
        document.getElementById("tdRateRow").style.display = "none";        
    }
    
    this.show_grid = function()
    {
        var dom = document.getElementById("divpresummary")
        
        dom.style.left = "50%"  ;
        dom.style.top = "2";       
        dom.style.display = 'block'; 
        showpanel('divpresummary');
    }
    
    this.show_ftp_summary_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            if (result.value != null)
            {
                datatable = result.value;       
                dbgrid = upf.create_ftp_grid();                        
                dbgrid.filter.enabled = false;            
                dbgrid.bind(datatable, 'tdFTPSummary', 'tblFTPFILE');            
                upf.show_ftp_grid();            
            }
            else
            {
                document.getElementById("pnlFTPFiles").style.display = "none";
            }
        }
        else
        {
            document.getElementById("pnlFTPFiles").style.display = "none";
        }
    }
    
    this.show_ftp_grid = function()
    {
        document.getElementById("pnlFTPFiles").style.display = "block";
    }
    
    this.display_ftpfiles = function()
    {
        var type        = upf.get_ddlvalue("ddFileTypeUpload",false);
        var format      = upf.get_ddlvalue("ddFileFormatUpload",false);
        var filename    = getText(document.getElementById('divfilename'));
        var ratetype    = upf.get_ddlvalue("ddlRateType",false);
        
        hide_elements('divDetails~divAddlField~divfilename~pnlFTPFiles');
        show_elements('divUploadFile');        
        show_elements('divfileBrowse');                
        IRSWorld.IRSFileLoader.BatchUploader.show_ftp_summary(parseInt(type), parseInt(format), filename, ratetype.toString(), upf.show_ftp_summary_callback);
    }
    
    this.selected_ftp_file = function(args)
    {
            var objid = dbgrid.getElementsAt(args.datarow, 4).item(0).id;
            
            var objcel   = document.getElementById(objid).value;
            document.getElementById('divfilename').innerText = objcel
            var type        = upf.get_ddlvalue("ddFileTypeUpload",false);
            var format      = upf.get_ddlvalue("ddFileFormatUpload",false);
            var ratetype    = upf.get_ddlvalue("ddlRateType",false);
            var filename    = objcel;
            
            var loaded = IRSWorld.IRSFileLoader.BatchUploader.upload_from_ftp(parseInt(type), parseInt(format), filename, ratetype.toString());
            if (loaded)
            {
                document.getElementById("pnlFTPFiles").style.display = "none"; 
                
                var val = IRSWorld.IRSFileLoader.BatchUploader.ctrl_vals().value.split('|');
                if (val != null | val != undefined)
                {
                    var file_types = IRSWorld.IRSFileLoader.BatchUploader.file_type(type).value;
                    if (file_types != null | file_types != undefined)
                    {
                        new common_method().to_load_dropdownlist("ddFileTypeUpload", file_types, "VALUE", "CODE");
                        upf.filter_ddlindx('ddFileTypeUpload', parseInt(val[0]));                        
                        upf.filter_ddlindx('ddlRateType',parseInt(val[6]));                        
                        var sfile_type   = upf.filter_ddlvalue('ddFileTypeUpload', parseInt(val[0]),true);
                        var file_formats = IRSWorld.IRSFileLoader.BatchUploader.file_format(sfile_type).value;
                        if (file_formats != null | file_formats != undefined)
                        {
                            new common_method().to_load_dropdownlist("ddFileFormatUpload", file_formats, "VALUE", "CODE");                            
                            upf.filter_ddlindx('ddFileFormatUpload', parseInt(val[1]));
                            setText(val[3]);
                            setText(val[4]);
                            setText(val[2]);
                            setText(val[5]);
                            hide_elements('divfileBrowse');
                            document.getElementById("btnUpload").style.display = "none";
                            document.getElementById("tdUpload").style.display = "none";
                            document.getElementById("tdProcess").style.display = "block";
                            document.getElementById("btnProcess").style.display = "block";
                            document.getElementById("divAddlField").style.display = "block";
                            document.getElementById("divfilename").style.display = "block";
                            document.getElementById("divUploadFile").style.display = "block";
                        }
                        else
                        {
                            IRSWorld.IRSFileLoader.BatchUploader.set_is_postback(false);
                            alert('Unable to get file format list from the database, Please try loading the file again.');
                        }
                    }
                    else
                    {
                        IRSWorld.IRSFileLoader.BatchUploader.set_is_postback(false);
                        alert('Unable to get file type list from the database, Please try loading the file again.');
                    }                             
               }
           }
    }
    
    this.create_ftp_grid = function()
    {
        gv = new datagrid();        
        {
		    var col = new datagrid_column(); // 0
		    col.header = 'FILE DATE';
		    col.datafield = 'FILEDATE'; //used in alias SQL
		    col.align = 'right';
		    col.styles.cell += 'width:auto;text-align:right;';
		    col.editable = false;
		    gv.addColumn(col);
        }
       {
		    var col = new datagrid_column(); // 1
		    col.header = 'FILENAME';
		    col.datafield = 'FILENAME';
		    col.align = 'right';
		    col.editable = false;
		    col.styles.cell += 'width:auto;text-align:right;';
		    gv.addColumn(col);
        }
        {
		    var col = new datagrid_column(); // 2
		    col.header = 'FILE SIZE';
		    col.datafield = 'FILESIZE';
		    col.align = 'right';			        		
		    col.styles.cell += 'width:auto;text-align:right;';
		    gv.addColumn(col);
        }
        {
		    var col = new datagrid_column(); // 3
		    col.header = 'ENCRYPTED';
		    col.datafield = 'ENCRYPTED';
		    col.align = 'center';			        		
		    col.styles.cell += 'width:auto;text-align:right;';
		    gv.addColumn(col);
        }
        {
		    var col = new datagrid_column(); // 4
		    col.header = 'FULLPATH';
		    col.datafield = 'FULLPATH';		    
		    col.styles.cell += 'width:0%;display=none';
            col.styles.header += 'display:none';
            col.styles.datafield += 'display:none';
		    gv.addColumn(col);
        }
            // for footer                     
            gv.row.onclick = 'select_ftp_files'
            gv.row.normal_style = 'background-color: #f7ffff;';                      
            return gv;
    }
    
    this.show_upload_file = function()
    {
            hide_elements('divAddlField~divfilename~divShowUpload');
            show_elements('divUploadFile');
    }
    
    this.show_uploaded_grid = function()
    {
            hide_elements('divAddlField~divfilename~divUploadFile');
            show_elements('divShowUpload');
            
            IRSWorld.IRSFileLoader.BatchUploader.show_uploaded_files(type, upf.load_uploaded_datagrid);
    }
    
    this.load_uploaded_datagrid = function(result)
    {
        if (result != null && result != undefined)
        {
            datatable = result.value;
       
            dbgrid = upf.create_uploaded_grid();                        
            dbgrid.filter.enabled = false;            
            dbgrid.bind(datatable, 'tdShowUpload', 'tblShowUpload');
            hide_elements('tblShowUpload_footerbtn_Edit');
        }
    }
    
    this.download_file = function(fname, ext)
    {
        if (fname.length > 0)
        {
            AjaxPro.timeoutPeriod = 1000 * 1000;

            AjaxPro.onTimeout = function()
            {
                alert("THE SERVER STOPPED RESPONDING. PLEASE TRY AGAIN");
                //this.close_msg_prompt(); setText("lbl_otc_caption", "SERVER NOT RESPONDING. CHECKING AVAILABILITY OF CURRENCY RATES FAILED."); alert("Server not responding. Checking availability of currency rates failed.");
            }
            IRSWorld.IRSFileLoader.BatchUploader.download_file(fname, ext, upf.download_file_callback);
        }
    }
    
    this.download_file_callback = function(result)
    {
        if (result.value) {
            var dl_pop = window.open("../IRSFileLoader/FileDownloader.aspx", "dl_pop","location=1,status=1,scrollbars=1, width=500,height=100");
            dl_pop.moveTo(0,0);
        }
        else {
            alert("Your previous request is being processed. Please Wait.");
            return;
        }
    }
    
    this.get_file_info = function(args) {
        var fname = ftpGrid.getValueAt(args.datarow, 0);
        upf.download_file(fname, "");
    }
    
    this.get_file_info_log = function(args) {
        var fname = ftpGrid.getValueAt(args.datarow, 0);
        upf.download_file(fname, "logs");
    }
    
    this.get_file_info_err = function(args) {
        var fname = ftpGrid.getValueAt(args.datarow, 0);
        upf.download_file(fname, "err");
    }
    
    this.reject = function(args) {
        var fname = ftpGrid.getValueAt(args.datarow, 0);
        
        var selected_row = dbgrid.row.selected_data_row;
        var status = document.getElementById("tblShowUpload_txt_Status_" + selected_row.toString()).value;
        if (status.toUpperCase() == "PROCESSED" | status.toUpperCase() == "REJECTED") {
            alert("BAKA!!! Rejecting approved/processed or rejected file is a baka move.");
            return;
        }
        
        AjaxPro.timeoutPeriod = 1000 * 1000;

        AjaxPro.onTimeout = function()
        {
            alert("THE SERVER STOPPED RESPONDING. PLEASE TRY AGAIN");
            //this.close_msg_prompt(); setText("lbl_otc_caption", "SERVER NOT RESPONDING. CHECKING AVAILABILITY OF CURRENCY RATES FAILED."); alert("Server not responding. Checking availability of currency rates failed.");
            return;
        }
        
        IRSWorld.IRSFileLoader.BatchUploader.reject_file(fname, upf.reject_callback);
    }
    
    this.reject_callback = function(result) {
        var selected_row = dbgrid.row.selected_data_row;
        if (result.value)
        {
            document.getElementById("tblShowUpload_btn_Approve_" + selected_row.toString()).disabled = true;
            document.getElementById("tblShowUpload_txt_Status_" + selected_row.toString()).value = "REJECTED";
            document.getElementById("tblShowUpload_btn_Reject_" + selected_row.toString()).disabled = true;
        }
        else
        {
            var msg = IRSWorld.IRSFileLoader.BatchUploader.uploader_msg().value.toString();
            alert(msg);
        }
    }
        
    this.create_uploaded_grid = function() {
        var user = new js_batchupload().get_roledesc();
        gv = new datagrid();
        {
            var col = new datagrid_column(); // 2
            col.header = 'File Name';
            col.datafield = 'Files';
            col.align = 'right';
            col.editable = false;
            col.styles.cell += 'width:auto;text-align:right;';
            gv.addColumn(col);
        }  
        {
            var col = new datagrid_column();
            col.header = 'User';
            col.datafield = 'User';
            col.align = 'right';
            col.styles.cell += 'width:auto;text-align:right;';
            col.editable = false;
            gv.addColumn(col);
        }
//        {
//            var col = new datagrid_column();
//            col.header = 'Total Records';
//            col.datafield = 'TOTALRECORD';
//            col.align = 'right';
//            col.styles.cell += 'width:auto;text-align:right;';
//            col.editable = false;
//            gv.addColumn(col);
//        }
//        {
//            var col = new datagrid_column();
//            col.header = 'Records Loaded';
//            col.datafield = 'LOADED';
//            col.align = 'right';
//            col.styles.cell += 'width:auto;text-align:right;';
//            col.editable = false;
//            gv.addColumn(col);
//        }
//        {
//            var col = new datagrid_column();
//            col.header = 'Records Error';
//            col.datafield = 'ERROR';
//            col.align = 'right';
//            col.styles.cell += 'width:auto;text-align:right;';
//            col.editable = false;
//            gv.addColumn(col);
//        }
        {
            var col = new datagrid_column(); // 0
            col.header = 'Date Uploaded';
            col.datafield = 'DateUploaded'; //used in alias SQL
            col.align = 'right';
            col.styles.cell += 'width:auto;text-align:right;';
            col.editable = false;
            gv.addColumn(col);
        }
//        if (user.indexOf('AGENT') > -1)
        {
            var col = new datagrid_column(); // 3
            col.header = 'Upload Status';
            col.datafield = 'Status';
            col.align = 'right';
            col.editable = false;
            col.styles.cell += 'width:auto;text-align:right;';
            gv.addColumn(col);
        }
        if (user.indexOf('APPROVE') > -1 & user.indexOf('AGENT') > -1)
        {
            {
                var col = new datagrid_column(); // 4
                col.header = 'For Approvals';
                col.type = 'button';
                col.value = 'Approve';
                col.onclick = 'upf.approve_file';
                col.datafield = '';
                col.align = 'center';
                col.styles.cell += 'width:auto;text-align:right;';
                gv.addColumn(col);
            }
            {
                var col = new datagrid_column(); // 4
                col.header = 'Reject';
                col.type = 'button';
                col.value = 'Reject';
                col.onclick = 'upf.reject';
                col.datafield = '';
                col.align = 'center';
                col.styles.cell += 'width:50px;text-align:right;';
                gv.addColumn(col);
            }
//            {
//                var col = new datagrid_column(); // 5
//                col.header = 'Show Summary';
//                col.type = 'button';
//                col.value = 'Show Summary';
//                col.onclick = 'upf.show_summary';
//                col.datafield = '';
//                col.align = 'center';
//                col.styles.cell += 'width:auto;text-align:right;';
//                gv.addColumn(col);
//            }
        }
        //if (user.indexOf('AGENT') > -1)
        {
            var col = new datagrid_column(); // 5
            col.header = 'Download Uploaded File';
            col.type = 'button';
            col.value = 'DownloadFile';
            col.onclick = 'upf.get_file_info';
            col.datafield = '';
            col.align = 'center';
            col.styles.cell += 'width:auto;text-align:right;';
            gv.addColumn(col);
        }
        {
            var col = new datagrid_column(); // 6
            col.header = 'Download Error File';
            col.type = 'button';
            col.value = 'DownloadErr';
            col.onclick = 'upf.get_file_info_err';
            col.datafield = '';
            col.align = 'center';
            col.styles.cell += 'width:auto;text-align:right;';
            gv.addColumn(col);
        }
        {
            var col = new datagrid_column(); // 7
            col.header = 'Download Log File';
            col.type = 'button';
            col.value = 'DownloadLog';
            col.onclick = 'upf.get_file_info_log';
            col.datafield = '';
            col.align = 'center';
            col.styles.cell += 'width:auto;text-align:right;';
            gv.addColumn(col);
        }
//        {
//            var col = new datagrid_column();
//            col.header = 'Reject';
//            col.type = 'button';
//            col.value = 'Reject';
//            col.onclick = 'upf.reject';
//            col.datafield = '';
//            col.align = 'center';
//            col.styles.cell += 'width:auto;text-align:right;';
        //        }

		/*************************************************/
        //Added code for FedAch Optimization 04052018
        {
            var col = new datagrid_column(); //8
            col.header = 'View Loading Status';
            col.type = 'button';
            col.value = 'View Loading';
            col.onclick = 'upf.view_fedach_loading';
            col.datafield = 'View';
            col.align = 'center';
            col.styles.cell += 'width:auto;text-align:right;';
            gv.addColumn(col);
        }
        /*************************************************/

        gv.row.normal_style = 'background-color: #f7ffff;';
        gv.row.onclick = 'upf.click_edit';
        gv.footer.edit_button.none = false;
        gv.footer.edit_button.style += 'background-color: lavender;';
        ftpGrid = gv;
        return gv;
    }
    
    this.approve_file = function(args)
    {
        var selected_row = dbgrid.row.selected_data_row;
        document.getElementById("tblShowUpload_btn_Approve_" + selected_row.toString()).disabled = true;
        
        btns = dbgrid.row.selected_data_row.toString() + '~' + btns;
        
        var fname = ftpGrid.getValueAt(args.datarow, 0);
        var ratetype = 0;
        
        try
        {
            ratetype = document.getElementById("ddlRateType").selectedIndex.toString();
        }
        catch (ex)
        {
            ratetype = 0;
        }
        
        AjaxPro.timeoutPeriod = 1000 * 1000;

        AjaxPro.onTimeout = function()
        {
            alert("THE SERVER STOPPED RESPONDING. PLEASE TRY AGAIN");
            //this.close_msg_prompt(); setText("lbl_otc_caption", "SERVER NOT RESPONDING. CHECKING AVAILABILITY OF CURRENCY RATES FAILED."); alert("Server not responding. Checking availability of currency rates failed.");
        }
        
        IRSWorld.IRSFileLoader.BatchUploader.approve_file(fname, parseInt(ratetype), upf.approve_file_callback);
        upf.show_loading_msg('Your request is being processed. Please Wait...');
    }
    
    this.approve_file_callback = function(result)
    {
        var selected_row = dbgrid.row.selected_data_row;
        if (result.value)
        {
            var selected_row = dbgrid.row.selected_data_row;
            alert('The file will now be processed at our end. An email of transaction summary will be sent shortly.');
        }
        else
        {
            var msg = IRSWorld.IRSFileLoader.BatchUploader.uploader_msg().value.toString();
            alert(msg);
        }
        document.getElementById("tblShowUpload_txt_Status_" + selected_row.toString()).value = "APPROVED";
        hidepanel('divLoadingMsg');
        hidepanel('divPopProcess');
        upf.click_edit();
    }

    /*************************************************/
    //Added code for FedAch Optimization 04052018
    this.view_fedach_loading = function (args) {
        var selected_row = dbgrid.row.selected_data_row;
        fedAchFilename = ftpGrid.getValueAt(args.datarow, 0);
        document.getElementById("tblShowUpload_btn_View_" + selected_row.toString()).disabled = true;
        document.getElementById("lblFileName").innerHTML = "Records will display shortly. Please Wait...";
        showpanel('divFedAchloadingsummary');
        myFedAchVar = window.setInterval(upf.fedach_loading_summary, 5000);
    }

    this.fedach_loading_summary = function () {
        document.getElementById("lblFileName").innerHTML = fedAchFilename;
        var recordloaded = IRSWorld.IRSFileLoader.BatchUploader.get_fedach_loading_summary(document.getElementById("lblFileName").innerHTML).value;
        document.getElementById("lblLoadedRecord").innerHTML = recordloaded.toString();
    }

    this.close_fedach_summary = function ()
    {
        fedAchFilename = "";
        this.clear("tblFedAchloadingsummary");
        window.clearInterval(myFedAchVar);
        hidepanel('divFedAchloadingsummary');
        upf.click_edit();
    }

    this.clear = function (tbl) {
        this.commons = new common_method();
        this.commons.to_clear_screen(tbl);
        this.commons = null;
    }
    /*************************************************/
    
    this.click_edit = function()
    {
        try
        {
            var edit_btn = document.getElementById("tblShowUpload_footerbtn_Edit");
            var role = new js_batchupload().get_roledesc();
            
            edit_btn.click();
            try
            {
                var selected_row = dbgrid.row.selected_data_row;
                var status = document.getElementById("tblShowUpload_txt_Status_" + selected_row.toString()).value;
                document.getElementById("tblShowUpload_btn_Approve_" + selected_row.toString()).disabled = true;
                
                if (role.toUpperCase().indexOf('AGENT'))
                {
                    if (status.toUpperCase() == "FOR APPROVAL") 
                    {
                        document.getElementById("tblShowUpload_btn_Approve_" + selected_row.toString()).disabled = false;
                        document.getElementById("tblShowUpload_btn_DownloadFile_" + selected_row.toString()).disabled = false;
                        document.getElementById("tblShowUpload_btn_DownloadErr_" + selected_row.toString()).disabled = true;
                        document.getElementById("tblShowUpload_btn_DownloadLog_" + selected_row.toString()).disabled = true;
                        document.getElementById("tblShowUpload_btn_Reject_" + selected_row.toString()).disabled = false;
                        /*************************************************/
                        //Added code for FedAch Optimization 04052018
                        document.getElementById("tblShowUpload_btn_View_" + selected_row.toString()).disabled = true;
                        /*************************************************/
                    }
                    else if (status.toUpperCase() == "REJECTED") {
                        document.getElementById("tblShowUpload_btn_Approve_" + selected_row.toString()).disabled = true;
                        document.getElementById("tblShowUpload_btn_DownloadFile_" + selected_row.toString()).disabled = false;
                        document.getElementById("tblShowUpload_btn_DownloadErr_" + selected_row.toString()).disabled = true;
                        document.getElementById("tblShowUpload_btn_DownloadLog_" + selected_row.toString()).disabled = true;
                        document.getElementById("tblShowUpload_btn_Reject_" + selected_row.toString()).disabled = true;
                        /*************************************************/
                        //Added code for FedAch Optimization 04052018
                        document.getElementById("tblShowUpload_btn_View_" + selected_row.toString()).disabled = true;
                        /*************************************************/
                    }
                    else {
                        document.getElementById("tblShowUpload_btn_Approve_" + selected_row.toString()).disabled = true;
                        document.getElementById("tblShowUpload_btn_DownloadFile_" + selected_row.toString()).disabled = false;
                        document.getElementById("tblShowUpload_btn_DownloadErr_" + selected_row.toString()).disabled = false;
                        document.getElementById("tblShowUpload_btn_DownloadLog_" + selected_row.toString()).disabled = false;
                        document.getElementById("tblShowUpload_btn_Reject_" + selected_row.toString()).disabled = true;
                        /*************************************************/
                        //Added code for FedAch Optimization 04052018
                        document.getElementById("tblShowUpload_btn_View_" + selected_row.toString()).disabled = false;
                        /*************************************************/
                    }
                }
            }
            catch (e)
            {
                document.getElementById("tblShowUpload_btn_Approve_" + selected_row.toString()).disabled = true;
            }
            finally
            {
                edit_btn.display = 'none';
            }     
        } 
        catch (e) { }
    }
    
    this.reset_objects = function()
    {
        IRSWorld.IRSFileLoader.BatchUploader.reset_objects();
        document.getElementById("ddFileTypeUpload").selectedIndex = (get_dropdown_index('ddFileTypeUpload') == 2) ? 1 : 0;
        document.getElementById("ddFileFormatUpload").selectedIndex = (get_dropdown_index('ddFileFormatUpload') == 2) ? 1 : 0;
        document.getElementById("ddlRateType").selectedIndex = 0;
    }
    
    this.create_grid = function()
    {
        gv = new datagrid();
        
        {
		    var col = new datagrid_column(); // 0
		    col.header = 'Currency';
		    col.datafield = 'CURRENCY'; //used in alias SQL
		    col.align = 'center';
		    col.styles.cell += 'width:auto';
		    col.editable = false;
		    gv.addColumn(col);
        }
        {
		    var col = new datagrid_column(); // 1
		    col.header = 'No of ITEMS';
		    col.datafield = 'NOOFITEMS';
		    col.align = 'right';
		    col.editable = false;
		    col.styles.cell += 'width:auto';
		    gv.addColumn(col);
        }
        {
		    var col = new datagrid_column(); // 2
		    col.header = 'Amount';
		    col.datafield = 'AMOUNT';
		    col.align = 'right';			        		
		    col.styles.cell += 'width:auto';
		    gv.addColumn(col);
        }
            //gv.autoPost = false;  
            // for footer                     
            
            gv.row.normal_style = 'background-color: #f7ffff;';                      
            return gv;
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
        
        if (userrole.indexOf('AGENT') > -1)
        {
            if (userrole.indexOf('APPROVER') > -1)
            {
                side_bar = new Array
                (
                    new Array(3, 0, 'Approvals', 'upf.show_uploaded_grid()')
                );
            }
            if (userrole.indexOf('PROCESS') > -1)
            {
                side_bar = new Array
                (
                    new Array(2, 0, 'Upload File', 'upf.cancel_click()'),
                    new Array(3, 0, 'Approvals', 'upf.show_uploaded_grid()')
                );
            }
        }
        else
        {
            side_bar = new Array
            (
                new Array(2, 0, 'Upload File', 'upf.cancel_click()'),
                new Array(3, 0, 'FTP Files', 'upf.show_uploaded_grid()')
            );
        }
        
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
    
    this.create_summary_grid = function()
    {
        AjaxPro.timeoutPeriod = 1000 * 1000;

        AjaxPro.onTimeout = function()
        {
            alert("THE SERVER STOPPED RESPONDING. PLEASE TRY AGAIN");
            //this.close_msg_prompt(); setText("lbl_otc_caption", "SERVER NOT RESPONDING. CHECKING AVAILABILITY OF CURRENCY RATES FAILED."); alert("Server not responding. Checking availability of currency rates failed.");
        }
        IRSWorld.IRSFileLoader.BatchUploader.get_presummary(summary_callback);
    }
    
    this.summary_callback = function(res)
    {
        
    }
}

page_load = function()
{
    if (!has_rates()) return;
    if (!yes_defaults()) return;
    type = getQueryString(window, 'type');

    /* VAPT 20180222 */
    if (!IRSWorld.IRSFileLoader.BatchUploader.validateRole(type).value) {
        spawn_errpage("You are not authorized to view this page. Access denied!");
        return;
    }
    
    var hidfld = document.getElementById("hfAllowedView").value.toString();
    var userrole = new js_batchupload().get_roledesc();
    if ( hidfld.length == 0)
    {
        showpanel('divPopProcess');
        document.getElementById("divLoadingMsg").innerText = "You are not allowed to view this page.";
        return;
    }
    
    document.getElementById("btnProcess").style.display = "none";
    //document.getElementById("tdProcess").style.display = "none";
    //document.getElementById("tdUpload").style.display = "none";
    //document.getElementById("tdCancel").style.display = "none";
    //document.getElementById("tdSummary").style.display = "none";
    var client_log = IRSWorld.IRSFileLoader.BatchUploader.client_logged_in().value;    
    document.getElementById("tdRateRow").style.display = "none";        
     
    if (client_log)
    {
        var is_postback = IRSWorld.IRSFileLoader.BatchUploader.get_is_postback().value;
        new SideBar().construct_side_bar();
        
        if (!is_postback)
        {
            var msg = IRSWorld.IRSFileLoader.BatchUploader.uploader_msg(alert_code_behind_msg);
            //page_init();
            if (userrole.indexOf('AGENT') > -1)
            {
                if (userrole.indexOf('APPROVER') > -1 & userrole.indexOf('PROCESS') > -1)
                {
                    non_agent();
                }
                else if (userrole.indexOf('APPROVER') > -1)
                {
                    a_a_page();
                }
                else
                {
                    a_p_page();
                }
            }
            else
            {
                non_agent();
            }
        }
        else
        {
            var val = IRSWorld.IRSFileLoader.BatchUploader.ctrl_vals().value;
            
            if (val != null && val != undefined && val.split('|').length > 1)
            {
                val = val.split('|');
                var rate_type = IRSWorld.IRSFileLoader.BatchUploader.load_rate_type().value;
                var file_types = IRSWorld.IRSFileLoader.BatchUploader.file_type(type).value;
        
                if (rate_type != null | rate_type != undefined )
                {
                    new common_method().to_load_dropdownlist("ddlRateType", rate_type, "VALUE", "CODE");                
                }
                else
                {
                    alert('NO RATES SETUP')
                    return;
                }
                
                //Set agent and non agent available controls
                if ((userrole.indexOf("AGENT") > -1  && parseInt(val[0]) != 7 ) || (parseInt(val[0]) != 3 && parseInt(val[0]) != 7 ) )
                {
                    document.getElementById("tdRateRow").style.display = "none";        
                    document.getElementById("btn_show_summary").style.display = "none";
                }
                else
                {
                    document.getElementById("tdRateRow").style.display = "block";
                    document.getElementById("tdRateRow").selectedIndex = 1;
                    document.getElementById("btn_show_summary").disabled = false;
                    //document.getElementById("tdSummary").style.display = "block";
                }
                //###########################################
                
                if (file_types != null | file_types != undefined)
                {
                    new common_method().to_load_dropdownlist("ddFileTypeUpload", file_types, "VALUE", "CODE");
                    upf.filter_ddlindx('ddFileTypeUpload', parseInt(val[0]));
                    //document.getElementById('ddFileTypeUpload').selectedIndex = parseInt(val[0]);
                    upf.filter_ddlindx('ddlRateType',parseInt(val[6]));
                    //document.getElementById('ddlRateType').selectedIndex  = parseInt(val[6]);
                    var sfile_type   = upf.filter_ddlvalue('ddFileTypeUpload', parseInt(val[0]),true);
                    var file_formats = IRSWorld.IRSFileLoader.BatchUploader.file_format(sfile_type).value;
                    
                    if (sfile_type.toUpperCase() == "IBATCH" | sfile_type.toUpperCase() == "ARS" | sfile_type.toUpperCase() == "EXCO" | sfile_type.toUpperCase() == "EXCEL") {
                        //var d = document.getElementById("tdProcess");
                        document.getElementById("btn_show_summary").disabled = false;
                        //document.getElementById("tdSummary").style.display = "block";
                        //document.getElementById("tdUpload").style.display = "block";
                        //document.getElementById("tdCancel").style.display = "block";
                        //d.parentNode.removeChild(d);
                    }
                    else {
                        document.getElementById("btn_show_summary").disabled = true;
                        //document.getElementById("tdUpload").style.display = "block";
                        //document.getElementById("tdCancel").style.display = "block";
                    }
                    if (file_formats != null | file_formats != undefined) {
                        if (sfile_type.toUpperCase() == "IBATCH" | sfile_type.toUpperCase() == "ARS")
                        {
                            if (parseInt(val[5]) <= 0)
                            {
                                alert("Unable to read file.\nThe type and/or format may not be intended to the file you are uploading.");
                                IRSWorld.IRSFileLoader.BatchUploader.set_is_postback(false);
                                page_load();
                            }
                        }
                        
                        new common_method().to_load_dropdownlist("ddFileFormatUpload", file_formats, "VALUE", "CODE");
                        upf.filter_ddlindx('ddFileFormatUpload', parseInt(val[1]));
                        setText(document.getElementById('recordCount'), val[3]);
                        //setText(document.getElementById('fileSize'), val[4]);
                        setText(document.getElementById('divfilename'), val[2]);
                        setText(document.getElementById('runamount'), val[5]);
                        hide_elements('divfileBrowse');//~trfilesize');
                        show_elements('divAddlField~divfilename~divUploadFile');//~trfilesize');
//                        if (sfile_type.toUpperCase() != "IBATCH" & sfile_type.toUpperCase() != "ARS") {
//                            hide_elements('trrunamount');
//                        }
                        if (parseInt(val[5] < 1)) {
                            hide_elements('trrunamount');
                        }
                        if (parseInt(val[3]) < 1) {
                            hide_elements('trrecordCount');
                        }
                        
                        //set_width();
                        return;
                    }
                }
                
                IRSWorld.IRSFileLoader.BatchUploader.set_is_postback(false);
                alert('Unable to get file type list from the database, Please try loading the file again.');
            }
            else
            {
                var msg = IRSWorld.IRSFileLoader.BatchUploader.uploader_msg();
                alert_code_behind_msg(msg);
                IRSWorld.IRSFileLoader.BatchUploader.reset_objects();
                IRSWorld.IRSFileLoader.BatchUploader.set_is_postback(false);
                page_load();
            }
        }
    }
}

function set_width() {
    var td1 = document.getElementById('tdCancel');
    var td2 = document.getElementById('tdSummary');
    var td3 = document.getElementById('tdProcess');
    var td4 = document.getElementById('tdUpload');
        
    if (td1 != null) { td1.style.width = (td1.style.display == 'block') ? '33%' : '25%'; }
    if (td2 != null) { td2.style.width = (td2.style.display == 'block') ? '33%' : '25%'; }
    if (td3 != null) { td3.style.width = (td3.style.display == 'block') ? '33%' : '25%'; }
    if (td4 != null) { td4.style.width = (td4.style.display == 'block') ? '33%' : '25%'; }
}

function page_init()
{
    //Cross browser
    
    var dv = document.getElementById('fileBrowse');
    
    
    if (navigator.appName.indexOf('Explorer') == -1)
    {
        dv.innerHTML = '';
        var a = '<table style="width:100%"><tr><td><input id="fileBrowse" runat="server" style="width:300px; height:24px" type="file" /></td>';
        a += '<td><input value="Search" style="width:25%; height:25px" type="button" class="button" onclick="javascript:upf.btnSearch_click()" /></td></tr></table>';
        
        dv.innerHTML = a;
    }
}

function alert_code_behind_msg(msg)
{
    if (msg.value != null && msg.value != undefined && msg.value.length > 0)
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
    try
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
    catch (e)
    {
        
    }
}


function a_p_page()
{
    this.js_com = new common_method();
                
    var file_types = IRSWorld.IRSFileLoader.BatchUploader.file_type(type).value;
    if (file_types != null | file_types != undefined)
    {
        new common_method().to_load_dropdownlist("ddFileTypeUpload", file_types, "VALUE", "CODE");
        if (file_types.Tables[0].Rows.length == 1)
        {
            select_index('ddFileTypeUpload', 1);
            upf.up_type_select();
            try
            {
                select_index('ddFileFormatUpload', 1)
            }
            catch (e)
            {
                select_index('ddFileFormatUpload', 0)
            }
        }
        show_elements('divUploadFile');
        hide_elements('divDetails');
    }
    else
    {
        alert('Error initializing file type. Please check if FileUploadType exists in the look up maintenance.');
        hide_elements('divUploadFile');
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
                
    var file_types = IRSWorld.IRSFileLoader.BatchUploader.file_type(type).value;
    if (file_types != null | file_types != undefined)
    {
        new common_method().to_load_dropdownlist("ddFileTypeUpload", file_types, "VALUE", "CODE");
        if (file_types.Tables[0].Rows.length == 1)
        {
            select_index('ddFileTypeUpload', 1);
            upf.up_type_select();
        }
        show_elements('divUploadFile');
        hide_elements('divDetails');
    }
    else
    {
        alert('Error initializing file type. Please check if FileUploadType exists in the look up maintenance.');
        hide_elements('divUploadFile');
    }
}

function select_index(elem_name, ndx)
{
    document.getElementById(elem_name).selectedIndex = ndx;
}

function has_rates()
{
    var type = getQueryString(window, 'type');
    if (type != 'C') {
        var a = IRSWorld.IRSFileLoader.BatchUploader.check_rates(type.toString());
        if (!a.value)
        {
            alert('Cannot proceed. Rates is not yet defined for this branch. \nPlease define currency rates or contact system administrator.');
            window.open('../Main/frmCenter.aspx', '_self');
            
            return false;
        }
    }
    return true;
}

function yes_defaults()
{
    var type = getQueryString(window, 'type');
    if (type != 'C') {
        var a = IRSWorld.IRSFileLoader.BatchUploader.check_defaults();
        if (!a.value) {
            var msg = IRSWorld.IRSFileLoader.BatchUploader.uploader_msg();
            alert(msg.value);
            window.open('../Main/frmCenter.aspx', '_self');
            return false;
        }
    }
    return true;
}

function get_dropdown_index(dd_name)
{
    var inner_html = IRSWorld.IRSFileLoader.BatchUploader.replace(document.getElementById(dd_name).innerHTML.toUpperCase(), '</OPTION><OPTION', '~');
    var list = inner_html.value.split('~');
    
    return list.length;
}