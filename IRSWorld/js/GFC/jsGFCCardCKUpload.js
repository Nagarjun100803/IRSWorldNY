function execute_ajaxgridview_command(param)
{
    if (cls_GFCCardCKUpload != null && cls_GFCCardCKUpload != "undefined")
    {
        cls_GFCCardCKUpload.do_ajaxgridview(param);
    }
   // Do nothing 
}

function GFC_Card_CK_Upload()
{
    var icls = this;
    var mode = null; // 0 = Upload Card - CK : 1 = Batch Upload 
    
    this.page_onload = function()
    {       
        this.mode = getQueryString(window, 'mode');
        document.getElementById('HMode').innerText = this.mode; 
        
        if (this.mode != null && this.mode != "")
        {        
            this.popDropDown();
            this.changeMode();
        }
        else        
        {
            document.getElementById("tblMain").style.display = 'none';
        }   
    }
    
    this.popDropDown = function()
    {
         this.showLoadingIcon(false); 
         return GFCCardCKUpload.popDropDown(this.mode,this.popDropDown_callback).value;
    }
   
    this.changeMode = function()
    {
        switch (this.mode)
        {
            case '0':
                document.getElementById('trBatchName').style.display = 'block';
                document.getElementById('tdDDLCaption').innerText = 'Upload Type :'; 
                document.getElementById('tbl_Forms').style.display = 'none'; 
                document.getElementById('td_Header').innerText = 'GFC CARD / CK UPLOAD'; 
                document.getElementById('btnPrintForm').style.display = 'none'; 
                break;  
            case '1':
                document.getElementById('trBatchName').style.display = 'none'; 
                document.getElementById('tdDDLCaption').innerText = 'Group :'; 
                document.getElementById('td_Header').innerText = 'GFC CARD BATCH UPLOAD'; 
                document.getElementById('tbl_Forms').style.display = 'none';                  
                
                break;
            default:
                document.getElementById("tblMain").style.display = 'none'; 
        }  
    }  
   
    this.popDropDown_callback = function(response)
    {
        if(response.error != null)
        {
            alert(response.error); 
        }  
        else
        {
            var DDL =  document.getElementById('ddlUploadType');
            DDL.length = 0;
                       
            var dt = response.value;
                              
            if (dt != null && dt.Rows.length > 0)
            {            
                for (var i = 0; i < dt.Rows.length; i++)
                {
                    var opt = document.createElement("option");    
                    opt.text = dt.Rows[i].FldCode.toString();
                    opt.value = dt.Rows[i].FldValue.toString();
                    DDL.options.add(opt); 
                }      
            }
         } 
    }
    
    this.showLoadingIcon = function(show)
    {
        if (show == true)
        {
            document.getElementById('trLoading').style.display = "block";   
        }
        else        
        {
            document.getElementById('trLoading').style.display = "none";  
        }    
    } 
    
    this.uploadFile = function()
    {
        this.showLoadingIcon(true);     
        document.getElementById('lblStatus').innerText = '';
        document.getElementById('Button1').click();
    }     
   
    this.cancel = function()
    {
        document.getElementById('txtBatchName').innerText = '';
        document.getElementById('lblStatus').innerText = '';
        document.getElementById('File1').value = '';
    } 
   
     this.uploadFile_callback = function(response)     
     {
        if (response.error != null)
        {
            icls.showLoadingIcon(false); 
            alert(response.error);
            return;
        }  
        icls.showLoadingIcon(false);
        alert("File successfully uploaded");
     } 
    
     this.btnPrintForm_click = function()
     {
        //alert(document.getElementById('HBatchNum').value);
        var param =  document.getElementById('HBatchNum').value.toString().split('~');
        //this.pop_batch_items_grid('ajx_loading$','04172010064605');
        
        this.pop_batch_items_grid('ajx_loading$',param[0]);
        document.getElementById('txtFileName').innerText = param[1];
          
        document.getElementById('tbl_Forms').style.display = 'block';
        document.getElementById('tblUploads').style.display = 'none';
     }

     this.btnBack_click = function()
     {
        document.getElementById('tbl_Forms').style.display = 'none';
        document.getElementById('tblUploads').style.display = 'block';
     }
    
     this.btnPrintReleaseForm_click = function()
     {
        var param =  document.getElementById('HBatchNum').value.toString().split('~'); 
        return GFCCardCKUpload.set_report_form(param[0], 'Release', this.show_report_callback).value; 
     }
    
    this.btnPrintTermsAndConditions_click = function()
    {
        var param =  document.getElementById('HBatchNum').value.toString().split('~'); 
        return GFCCardCKUpload.set_report_form(param[0], 'Terms', this.show_report_callback).value;
    }
    
    this.show_report_callback = function(response)
    {
        if (response.error != null){alert('Failed to load report');}
        else
        {            
           window.open('../Reports/Pages/Report_Viewer.aspx' + response.value.toString(),1) 
        }         
    }   
     
    this.pop_batch_items_grid = function(param, reference)
    {     
        this.grid = new grid_viewer();
        this.grid.sql_source     = "sp_gfc_get_batch_upload_data~" + reference + "~"
        this.grid.div_container  = document.getElementById("diviGridBatchItems");
        this.grid.datarow_column = 0;
        this.grid.page_size = 10;
        this.grid.process_ajax(param);
        this.grid = null;
    }     
     
    this.HBatchNum_onchange = function()
    {
        alert("Success"); 
    }  
    
   this.do_ajaxgridview = function(parameter)
   {
        var command_param = parameter.split("$");
        if (command_param.length > 0)
        {
            if (command_param[0] == "ajx_selected")
            {   
                // do nothing
            }
            else
            {
                 if (command_param[0] != "ajx_sorting") 
                 {
                    this.pop_batch_items_grid(parameter);
                 }
            }
        }          
   }    
    
}
