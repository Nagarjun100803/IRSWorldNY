var ftpGrid;

function js_uploadfile()
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
                    return cbo[i].innerText;
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
    
    this.up_type_select = function()
    {   
        
        upf.sfile_type = document.getElementById('ddFileTypeUpload')[document.getElementById('ddFileTypeUpload').selectedIndex].text;
        var ifile_type  = document.getElementById('ddFileTypeUpload')[document.getElementById('ddFileTypeUpload').selectedIndex].value;
        if (ifile_type.toString() != "3" )
        {
            document.getElementById("ddlRateType").selectedIndex = 1;
            //document.getElementById("ddlRateType").disabled = true;
            document.getElementById("pnlFTPFiles").innerHTML = '';
            document.getElementById("tdRateRow").style.display = "none";        
            document.getElementById("Button2").disabled = true;
        }
        else
        {
            document.getElementById("ddlRateType").disabled = false;
            //document.getElementById("tdRateRow").style.display = "block";        
            document.getElementById("Button2").disabled = false;
        }
        IRSWorld.IRSFileLoader.UploadFile.file_format(upf.sfile_type, upf.up_type_select_callback);
    }
    
    this.up_type_select_callback = function(result)
    {
        if (result != null && result != undefined)
        {
            new common_method().to_load_dropdownlist('ddFileFormatUpload', result.value, "VALUE", "CODE");
        }
    }
    
    this.vw_type_select = function()
    {
        upf.sfile_type = document.getElementById('ddFileTypeSearch')[document.getElementById('ddFileTypeSearch').selectedIndex].text;
        IRSWorld.IRSFileLoader.UploadFile.file_format(upf.sfile_type, upf.vw_type_select_callback);
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
        hide_elements('divUploadFile~divDetails');//document.getElementById("divUploadFile").style.display       = "none";
        show_elements('divSearch');//document.getElementById("divSearch").style.display           = "block";
        //document.getElementById("divDetails").style.display          = "none";
    }
    
    this.file_browse = function()
    {
        var type        = upf.get_ddlvalue("ddFileTypeUpload",false);
        var format      = upf.get_ddlvalue("ddFileFormatUpload",false);
        var filename    = getFileName('fileBrowse');//document.getElementById('fileBrowse').value;
        var ratetype    = document.getElementById('ddlRateType').selectedIndex;        

        if (type < 1)
        {
            alert('Please select file type.'); return;
        }
        if (format < 1)
        {
            alert('Please select file format.'); return;
        }
        
         //IRSWorld.IRSFileLoader.UploadFile.upload_ini(type, format, cls.upload_ini_callback);
        var ini = IRSWorld.IRSFileLoader.UploadFile.upload_ini(parseInt(type), parseInt(format), filename, ratetype.toString());
        
        if (ini.value)
        {
            document.getElementById('btnPost').click();
        }
        else
        {
            IRSWorld.IRSFileLoader.UploadFile.uploader_msg(upf.msg_callback);
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
        filter.datafield = 'Files';
        filter.operator = '!=';
        filter.value = "'" + searchval + '@' + "' && row.Files.toString().toLowerCase().indexOf('" +  searchval.toLowerCase() + "') > -1";
        dbgrid.addFilter(filter);
        dbgrid.refresh(datatable);
        hide_elements('tblShowUpload_footerbtn_Edit');
    }
    
    this.show_loading_msg = function(msg)
    {
        new common_method().to_show_loading_message('divLoadingMsg');
        setText(document.getElementById('lblLoadingMsg'), msg);//document.getElementById('lblLoadingMsg').innerText = msg;
        showpanel('divPopProcess');
    }
    
    this.btnUpload_click = function()
    {
        var type        = upf.get_ddlvalue("ddFileTypeUpload",false);
        var userrole    = new js_uploadfile().get_roledesc();
        var ratetype;
        
        if (userrole.indexOf("AGENT") > -1 || type != 3)
        {
            ratetype = 0;
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
            return ;
        }
        
        if (!confirm("Continue with Upload?"))
        {
            return;
        }
        
        var format      = upf.get_ddlvalue("ddFileFormatUpload",false);
        var ratetype    = upf.get_ddlvalue("ddlRateType");
        var file_path   = document.getElementById("fileBrowse").value;
        var totrec      = getText(document.getElementById("recordCount"));//document.getElementById("recordCount").innerText;
        var runamt      = getText(document.getElementById("runamount"));//document.getElementById("runamount").innerText;
        if (type > 0)
        {
            if (format > 0)
            {
                var userrole = new js_uploadfile().get_roledesc();
                if (userrole.indexOf("AGENT") > -1)
                {
                    ratetype = 0;
                }
                IRSWorld.IRSFileLoader.UploadFile.save_file(parseInt(type), parseInt(format), ratetype.toString(), totrec, runamt, upf.upload_callback);
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
        if (result.value)
        {
            IRSWorld.IRSFileLoader.UploadFile.uploader_msg(upf.msg_callback);
        }
        else
        {
            IRSWorld.IRSFileLoader.UploadFile.uploader_msg(upf.msg_callback);
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
        
        IRSWorld.IRSFileLoader.UploadFile.process_file(r, upf.process_callback);
        new common_method().to_show_loading_message('divLoadingMsg');
        setText(document.getElementById('lblLoadingMsg'));//document.getElementById('lblLoadingMsg').innerText = 'Uploading, Please wait...';
        showpanel('divPopProcess');
    }
    
    this.process_callback = function(result)
    {
      
        IRSWorld.IRSFileLoader.UploadFile.uploader_msg(upf.msg_callback);
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
        
        IRSWorld.IRSFileLoader.UploadFile.show_summary(type, format, filename, ratetype.toString(), upf.show_summary_callback)
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
        var xy = calcpos(document.getElementById("divPopProcess"));
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
        var filename    = getText(document.getElementById('divfilename'));//document.getElementById('divfilename').innerText;
        var ratetype    = upf.get_ddlvalue("ddlRateType",false);
        
        //document.getElementById("pnlFTPFiles").style.display = "none";
        //document.getElementById("divfilename").style.display = "none";                        
        hide_elements('divDetails~divAddlField~divfilename~pnlFTPFiles');
        show_elements('divUploadFile');        
        show_elements('divfileBrowse');                
        IRSWorld.IRSFileLoader.UploadFile.show_ftp_summary(parseInt(type), parseInt(format), filename, ratetype.toString(), upf.show_ftp_summary_callback);
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
            
            var loaded = IRSWorld.IRSFileLoader.UploadFile.upload_from_ftp(parseInt(type), parseInt(format), filename, ratetype.toString());
            if (loaded)
            {
                document.getElementById("pnlFTPFiles").style.display = "none"; 
                
                var val = IRSWorld.IRSFileLoader.UploadFile.ctrl_vals().value.split('|');
                if (val != null | val != undefined)
                {
                    var file_types = IRSWorld.IRSFileLoader.UploadFile.file_type().value;
                    if (file_types != null | file_types != undefined)
                    {
                        new common_method().to_load_dropdownlist("ddFileTypeUpload", file_types, "VALUE", "CODE");
                        upf.filter_ddlindx('ddFileTypeUpload', parseInt(val[0]));                        
                        upf.filter_ddlindx('ddlRateType',parseInt(val[6]));                        
                        var sfile_type   = upf.filter_ddlvalue('ddFileTypeUpload', parseInt(val[0]),true);
                        var file_formats = IRSWorld.IRSFileLoader.UploadFile.file_format(sfile_type).value;
                        if (file_formats != null | file_formats != undefined)
                        {
                            new common_method().to_load_dropdownlist("ddFileFormatUpload", file_formats, "VALUE", "CODE");                            
                            upf.filter_ddlindx('ddFileFormatUpload', parseInt(val[1]));
                            setText(val[3]);//document.getElementById('recordCount').innerText               = val[3];
                            setText(val[4]);//document.getElementById('fileSize').innerText                  = val[4];
                            setText(val[2]);//document.getElementById('divfilename').innerText               = val[2];
                            setText(val[5]);//document.getElementById('runamount').innerText                 = val[5];
                            hide_elements('divfileBrowse');//document.getElementById('divfileBrowse').style.display         = 'none';
                            document.getElementById("btnUpload").style.display = "none";
                            document.getElementById("btnProcess").style.display = "block";
                            document.getElementById("divAddlField").style.display = "block";
                            document.getElementById("divfilename").style.display = "block";
                            document.getElementById("divUploadFile").style.display = "block";
                            //show_elements('divAddlField~divfilename~divUploadFile');
                        }
                        else
                        {
                            IRSWorld.IRSFileLoader.UploadFile.set_is_postback(false);
                            alert('Unable to get file format list from the database, Please try loading the file again.');
                        }
                    }
                    else
                    {
                        IRSWorld.IRSFileLoader.UploadFile.set_is_postback(false);
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
            
            IRSWorld.IRSFileLoader.UploadFile.show_uploaded_files(upf.load_uploaded_datagrid);
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
    
    this.download_file = function(fname)
    {
        if (fname.length > 0)
        {
            IRSWorld.IRSFileLoader.UploadFile.download_file(fname, upf.download_file_callback);
        }
        
        alert("Your download will start shortly. Pls wait...");
    }
    
    this.download_file_callback = function(result)
    {
        var dl_pop = window.open("../IRSFileLoader/FileDownloader.aspx", "dl_pop","location=1,status=1,scrollbars=1, width=1,height=1");
        dl_pop.moveTo(0,0);
    }
    
    this.get_file_info = function(args)
    {
        var fname = ftpGrid.getValueAt(args.gridrow, 0);
        upf.download_file(fname);
    }
        
    this.create_uploaded_grid = function()
    {
        var user = new js_uploadfile().get_roledesc();
        gv = new datagrid();
        {
            var col = new datagrid_column(); // 2
            col.header = 'FILENAME';
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
        {
            var col = new datagrid_column(); // 3
            col.header = 'Upload Status';
            col.datafield = 'Status';
            col.align = 'right';
            col.editable = false;
            col.styles.cell += 'width:auto;text-align:right;';
            gv.addColumn(col);
        }      
        {
            var col = new datagrid_column(); // 0
            col.header = 'DATE UPLOADED';
            col.datafield = 'DateUploaded'; //used in alias SQL
            col.align = 'right';
            col.styles.cell += 'width:auto;text-align:right;';
            col.editable = false;
            gv.addColumn(col);
        }
        if (user.indexOf('APPROVE') > -1)
        {
            {
                var col = new datagrid_column(); // 5
                col.header = 'Approvals';
                col.type = 'button';
                col.value = 'Approve';
                col.onclick = 'upf.approve_file';
                col.datafield = '';
                col.align = 'center';
                col.styles.cell += 'width:auto;text-align:right;';
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
        {
            var col = new datagrid_column(); // 4 
            col.header = 'Downloads';
            col.type = 'button';
            col.value = 'Download';
            col.onclick = 'upf.get_file_info';
            col.datafield = '';
            col.align = 'center';
            col.styles.cell += 'width:auto;text-align:right;';
            gv.addColumn(col);
        }

        gv.row.normal_style = 'background-color: #f7ffff;';
        gv.row.onclick = 'upf.click_edit';
        gv.footer.edit_button.none = false;
        gv.footer.edit_button.style += 'background-color: lavender;';
        ftpGrid = gv;                      
        return gv;
    }
    
    this.approve_file = function(args)
    {
        var fname = ftpGrid.getValueAt(args.gridrow, 0);
        var ratetype = 0;
        
        try
        {
            ratetype = document.getElementById("ddlRateType").selectedIndex.toString();
        }
        catch (ex)
        {
            ratetype = 0;
        }
        
        IRSWorld.IRSFileLoader.UploadFile.approve_file(fname, parseInt(ratetype), upf.approve_file_callback);
        upf.show_loading_msg('Your request is being processed. Please Wait...');
    }
    
    this.approve_file_callback = function(result)
    {
        if (result.value)
        {
            alert('The file will now be processed at our end. \nAn email of transaction summary will be sent shortly.');
        }
        else
        {
            var msg = IRSWorld.IRSFileLoader.UploadFile.uploader_msg().value.toString();
            alert(msg);
        }
        hidepanel('divPopProcess');
        upf.show_uploaded_grid();
        upf.click_edit();
    }
    
    this.click_edit = function()
    {
        var edit_btn = document.getElementById("tblShowUpload_footerbtn_Edit");
        edit_btn.click();
        try
        {
            var selected_row = dbgrid.row.selected_data_row;
            var status = document.getElementById("tblShowUpload_txt_Status_" + selected_row.toString()).value;
            
            if (status == "UNPROCESSED") 
            {
                document.getElementById("tblShowUpload_btn_Approve_" + selected_row.toString()).disabled = false;
            }
            else
            {
                document.getElementById("tblShowUpload_btn_Approve_" + selected_row.toString()).disabled = true;
            }
        }
        catch (e)
        {
        }
        
        edit_btn.display = 'none';
    }
    
    this.reset_objects = function()
    {
        IRSWorld.IRSFileLoader.UploadFile.reset_objects();
        document.getElementById("ddFileTypeUpload").selectedIndex = 0;
        document.getElementById("ddFileFormatUpload").selectedIndex = 0;
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
		    var col = new datagrid_column(); // 4
		    col.header = 'No of ITEMS';
		    col.datafield = 'NOOFITEMS';
		    col.align = 'right';
		    col.editable = false;
		    col.styles.cell += 'width:auto';
		    gv.addColumn(col);
        }
        {
		    var col = new datagrid_column(); // 5
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
                    //new Array(2, 0, 'Upload File', 'upf.cancel_click()'),
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
        IRSWorld.IRSFileLoader.UploadFile.get_presummary(summary_callback)
    }
    
    this.summary_callback = function(res)
    {
        
    }
    

}

page_load = function()
{
    hasRates();
    var hidfld = document.getElementById("hfAllowedView").value.toString();
    var userrole = new js_uploadfile().get_roledesc();
    if ( hidfld.length == 0)
    {
        showpanel('divPopProcess');
        document.getElementById("divLoadingMsg").innerText = "You are not allowed to view this page.";
        return;
    }
    
    document.getElementById("btnProcess").style.display = "none";    
    var client_log = IRSWorld.IRSFileLoader.UploadFile.client_logged_in().value;    
    document.getElementById("tdRateRow").style.display = "none";        
     
    if (client_log)
    {
        var is_postback = IRSWorld.IRSFileLoader.UploadFile.get_is_postback().value;
        new SideBar().construct_side_bar();
        
        if (!is_postback)
        {   
            page_init();
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
            var val = IRSWorld.IRSFileLoader.UploadFile.ctrl_vals().value;
            
            if (val != null | val != undefined)
            {
                val = val.split('|');
                var rate_type = IRSWorld.IRSFileLoader.UploadFile.load_rate_type().value;
        
        
                if (rate_type != null | rate_type != undefined )
                {
                    new common_method().to_load_dropdownlist("ddlRateType", rate_type, "VALUE", "CODE");                
                }
                else
                {
                    alert('NO RATES SETUP')
                    return;
                }
                
                
                var file_types = IRSWorld.IRSFileLoader.UploadFile.file_type().value;
                
                if (userrole.indexOf("AGENT") > -1 || parseInt(val[0]) != 3 )
                {
                    document.getElementById("tdRateRow").style.display = "none";        
                    document.getElementById("Button2").style.display = "none";
                }
                else
                {
                    document.getElementById("tdRateRow").style.display = "block";
                    document.getElementById("tdRateRow").selectedIndex = 1;
                    document.getElementById("Button2").disabled = false;
                }
                
                
                if (file_types != null | file_types != undefined)
                {
                    new common_method().to_load_dropdownlist("ddFileTypeUpload", file_types, "VALUE", "CODE");
                    upf.filter_ddlindx('ddFileTypeUpload', parseInt(val[0]));
                    //document.getElementById('ddFileTypeUpload').selectedIndex = parseInt(val[0]);
                    upf.filter_ddlindx('ddlRateType',parseInt(val[6]));
                    //document.getElementById('ddlRateType').selectedIndex  = parseInt(val[6]);
                    var sfile_type   = upf.filter_ddlvalue('ddFileTypeUpload', parseInt(val[0]),true);
                    var file_formats = IRSWorld.IRSFileLoader.UploadFile.file_format(sfile_type).value;
                    
                    
                    if (sfile_type != 3)
                    {
                        document.getElementById("Button2").disabled = true;
                    }
                    {
                        document.getElementById("Button2").disabled = false;
                    }
                    if (file_formats != null | file_formats != undefined)
                    {
                        if (sfile_type == "IBatch")
                        {
                            if (parseInt(val[5]) <= 0)
                            {
                                alert("Unable to read file.\nThe type and/or format may not be intended to the file you are uploading.");
                                IRSWorld.IRSFileLoader.UploadFile.set_is_postback(false);
                                page_load();
                            }
                        }
                        
                        new common_method().to_load_dropdownlist("ddFileFormatUpload", file_formats, "VALUE", "CODE");
                        //sdocument.getElementById('ddFileFormatUpload').selectedIndex = parseInt(val[1]);
                        upf.filter_ddlindx('ddFileFormatUpload', parseInt(val[1]));
                        document.getElementById('recordCount').innerText                = val[3];
                        document.getElementById('fileSize').innerText                   = val[4];
                        document.getElementById('divfilename').innerText                = val[2];
                        document.getElementById('runamount').innerText                  = val[5];
                        hide_elements('divfileBrowse~trfilesize');//document.getElementById('divfileBrowse').style.display         = 'none';
                        show_elements('divAddlField~divfilename~divUploadFile');
                        if (sfile_type.toUpperCase() != "IBATCH" & sfile_type.toUpperCase() != "ARS")
                        {
                            hide_elements('trrunamount');
                        }
                    }
                    else
                    {
                        IRSWorld.IRSFileLoader.UploadFile.set_is_postback(false);
                        alert('Unable to get file format list from the database, Please try loading the file again.');
                    }
                }
                else
                {
                    IRSWorld.IRSFileLoader.UploadFile.set_is_postback(false);
                    alert('Unable to get file type list from the database, Please try loading the file again.');
                }
            }
            else
            {
                var msg = IRSWorld.IRSFileLoader.UploadFile.uploader_msg(alert_code_behind_msg);
                //alert_code_behind_msg(msg);
                IRSWorld.IRSFileLoader.UploadFile.set_is_postback(false);
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
                
    var file_types = IRSWorld.IRSFileLoader.UploadFile.file_type().value;
    if (file_types != null | file_types != undefined)
    {
        new common_method().to_load_dropdownlist("ddFileTypeUpload", file_types, "VALUE", "CODE");
        show_elements('divUploadFile');//document.getElementById("divUploadFile").style.display       = "block";
        hide_elements('divDetails');//document.getElementById("divDetails").style.display          = "none";
    }
    else
    {
        alert('Error initializing file type. Please check if FileUploadType exists in the look up maintenance.');
        hide_elements('divUploadFile');//document.getElementById('divUploadFile').style.display = 'none';
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
                
    var file_types = IRSWorld.IRSFileLoader.UploadFile.file_type().value;
    if (file_types != null | file_types != undefined)
    {
        new common_method().to_load_dropdownlist("ddFileTypeUpload", file_types, "VALUE", "CODE");
        show_elements('divUploadFile');//document.getElementById("divUploadFile").style.display       = "block";
        hide_elements('divDetails');//document.getElementById("divDetails").style.display          = "none";
    }
    else
    {
        alert('Error initializing file type. Please check if FileUploadType exists in the look up maintenance.');
        hide_elements('divUploadFile');//document.getElementById('divUploadFile').style.display = 'none';
    }
}

function hasRates()
{
    var a = IRSWorld.IRSFileLoader.UploadFile.check_rates();
    
    if (!a.value)
    {
        spawn_errpage("Currency rates for this location is not yet defined. \nPls contact your system administrator.");
        return;
    }
}