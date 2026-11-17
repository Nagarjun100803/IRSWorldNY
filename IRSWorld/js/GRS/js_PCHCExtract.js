var datatable;
var dbgrid;

function PCHCExrt()
{
    this.TranDate = null;
    this.FileNo = null;
    this.SFileNo = null;
    this.BankCode = null;
    
    this.popDDL = function(DDL, dt)
    {
        for (var i = 0; i < dt.Rows.length; i++)
        {
            if (i == 0)
            {
                var opt2 = document.createElement("option");
                opt2.text = '';
                opt2.value = '';
                DDL.options.add(opt2);
            }
            var opt = document.createElement("option");                 
            opt.text = dt.Rows[i].FldDescription.toString();
            opt.value = dt.Rows[i].FldCode.toString();
            DDL.options.add(opt);
        }      
    }
    
    this.ClearDDL = function(DDL)
    {
        DDL.length = 0;
        DDL.Value = null;
        DDL.text = '';
    }
    
    this.popDDLFileNo = function(DDL, dt)
    {
        for (var i = 0; i < dt.Rows.length; i++)
        {
            var opt = document.createElement("option");                 
            opt.text = dt.Rows[i].file_ind.toString();
            opt.value = dt.Rows[i].reload.toString();
            DDL.options.add(opt);
        }      
    }
    
    this.popDDLSFileNo = function(DDL, dt)
    {
        for (var i = 0; i < dt.Rows.length; i++)
        {
            var opt = document.createElement("option");                 
            opt.text = dt.Rows[i].SubFileInd.toString();
            opt.value = dt.Rows[i].SubFileInd.toString();
            DDL.options.add(opt);
        }      
    }
    
    this.getBankCode = function()
    {
        var dt = IRSWorld.GRS.Extract.Extract_PCHC.getLookup('GRSXTRCT').value;
        this.TranDate = IRSWorld.GRS.Extract.Extract_PCHC.getlocaltimeString().value;
        document.getElementById("txtDateOfTransfer").value = this.TranDate;
        
        if (dt != null && dt.Rows.length > 0)
        {
            this.ClearDDL(document.getElementById("ddlExtract"));
            this.popDDL(document.getElementById("ddlExtract"), dt);
            document.getElementById("ddlExtract").selectedIndex = 1;
            this.getFileNo();
        }
    }
    
    this.getFileNo = function()
    {
        this.BankCode = document.getElementById("ddlExtract").value;
        this.TranDate = document.getElementById("txtDateOfTransfer").value;
        
        IRSWorld.GRS.Extract.Extract_PCHC.getFileIndList(this.TranDate,this.getFileNo_callback);
        showPanel('divpopProcess');
        new common_method().to_show_loading_message('divLoadingMsg');
        document.getElementById('lblLoadingMsg').innerText = 'SYSTEM IS PROCESSING YOUR REQUEST...';
//        var dt = IRSWorld.GRS.Extract.Extract_PCHC.getFileIndList(this.TranDate).value;
//        if (dt != null && dt.Rows.length > 0)
//        {
//            this.ClearDDL(document.getElementById("ddFileNo"));
//            this.popDDLFileNo(document.getElementById("ddFileNo"), dt);
//            this.getSFileNo();
//        }
    }
    
    this.getFileNo_callback = function(param)
    {
        var dt = param.value;
        
        if (dt != null && dt.Rows.length > 0)
        {
            PCHCx.ClearDDL(document.getElementById("ddFileNo"));
            PCHCx.popDDLFileNo(document.getElementById("ddFileNo"), dt);
            hidePanel('divpopProcess');
            PCHCx.getSFileNo();
        }
    }
    
    this.getSFileNo = function()
    {
        this.FileNo = document.getElementById("ddFileNo")[document.getElementById("ddFileNo").selectedIndex].text;
        
        IRSWorld.GRS.Extract.Extract_PCHC.getSubFileIndList(this.TranDate,this.FileNo,this.BankCode,this.getSFileNo_callback);
        showPanel('divpopProcess');
        new common_method().to_show_loading_message('divLoadingMsg');
        document.getElementById('lblLoadingMsg').innerText = 'SYSTEM IS PROCESSING YOUR REQUEST...';
//        var dt = IRSWorld.GRS.Extract.Extract_PCHC.getSubFileIndList(this.TranDate,this.FileNo,this.BankCode).value;
//        if (dt != null && dt.Rows.length > 0)
//        {
//            this.ClearDDL(document.getElementById("ddSFileNo"));
//            this.popDDLSFileNo(document.getElementById("ddSFileNo"), dt);
//            this.SFileNo = document.getElementById("ddSFileNo")[document.getElementById("ddSFileNo").selectedIndex].text
//        }
    }
    
    this.getSFileNo_callback = function(param)
    {
        var dt = param.value;
        hidePanel('divpopProcess');
        if (dt != null && dt.Rows.length > 0)
        {
            PCHCx.ClearDDL(document.getElementById("ddSFileNo"));
            PCHCx.popDDLSFileNo(document.getElementById("ddSFileNo"), dt);            
            PCHCx.SFileNo = document.getElementById("ddSFileNo")[document.getElementById("ddSFileNo").selectedIndex].text
        }
    }
    
    this.Extract = function()
    {
        var reload = true;
        var FileInd;
        var SFileInd;
        
        if(this.BankCode == null || this.BankCode.length == 0)
        {
            alert("Please choose Extract Type!");
            document.getElementById("ddlExtract").focus()
            return;
        }
        
        if(this.TranDate == null || this.TranDate.length == 0)
        {
            alert("Please choose Transaction Date!");
            document.getElementById("txtDateOfTransfer").focus()
            return;
        }
        
        try
        {
            if(document.getElementById("ddFileNo").selectedIndex == 0)
            {
                FileInd = document.getElementById("ddFileNo")[0].innerText;
            }
            else
            {
                FileInd = document.getElementById("ddFileNo")[document.getElementById("ddFileNo").selectedIndex].innerText;
            }
        }
        catch(e)
        {
            FileInd = '';
        }
        
        try
        {
            if(document.getElementById("ddSFileNo").selectedIndex == 0)
            {
                SFileInd = document.getElementById("ddSFileNo")[0].innerText;
            }
            else
            {
                SFileInd = document.getElementById("ddSFileNo")[document.getElementById("ddSFileNo").selectedIndex].innerText;
            }
        }
        catch(e)
        {
            SFileInd = '';
        }
        
        if(document.getElementById("ddFileNo").value == "1")
        {
            FileInd = "";
            reload = false;
        }
        
        if(this.SFileNo == null || this.SFileNo.length == 0)
        {
            reload = false;
        }
        else
        {
            reload = true;
        }
        
        IRSWorld.GRS.Extract.Extract_PCHC.Extract(this.BankCode,this.TranDate,FileInd,SFileInd,reload,this.Extract_callback);
    }
    
    this.Extract_callback = function(res)
    {
        var cboFileInd = document.getElementById('ddFileNo');
        var cboSFileInd = document.getElementById('ddSFileNo');
        var reload = true;
        var rload = reload;
        
        try
        {
            if (cboFileInd.selectedIndex == 0)
            {
                PCHCx.FileNo = cboFileInd[0].innerText;
            }
            else
            {
                PCHCx.FileNo = cboFileInd[cboFileInd.selectedIndex].innerText;
            }
        }
        catch(e)
        {
            PCHCx.FileNo = '';
        }
        
        try
        {
            if(cboSFileInd.selectedIndex == 0)
            {
                PCHCx.SFileNo = cboSFileInd[0].innerText;
            }
            else
            {
                PCHCx.SFileNo = cboSFileInd[cboSFileInd.selectedIndex].innerText;
            }
        }
        catch(e)
        {
            PCHCx.SFileNo = '';
        }
        
        if(cboFileInd.value == "1")
        {
            this.FileNo = "";
            reload = false;
        }
        if(cboSFileInd.length == 0)
        {
            reload = false;
        }
        else
        {
            reload = true;
        }
        
        rlod = reload;
        
        if(res.error != null)
        {
            document.getElementById('dvPCHCExtractSummary').style.display = 'none';
//            var uID = getQSValue('userID').toString();
            alert(res.error.Message);
            return;
        }
        
        var dt = res.value;
        
        if(dt != null && typeof(dt) == "object" && dt.Rows.length >= 0)
        {
            cboFileInd = document.getElementById('ddFileNo');
            cboSFileInd = document.getElementById('ddSFileNo');
            
            try
            {
                if(cboFileInd.selectedIndex == 0)
                {
                    document.getElementById('lblFileNo').innerText =  cboFileInd[0].innerText
                }
                else
                {
                    document.getElementById('lblFileNo').innerText =  cboFileInd[cboFileInd.selectedIndex].innerText;
                }
            }
            catch(e)
            {
                document.getElementById('lblFileNo').innerText = '';
            }
            
            document.getElementById('lblDateOfTransfer').innerText = PCHCx.TranDate;
            
            for(var i = 0; i < dt.Rows.length; i++)
            {
                document.getElementById('lblSFileNo').innerText = dt.Rows[i].SFileInd.toString();
                
            }
            if(dt.Rows.length != null)
            {
                PCHCx.load_datagrid(res)
            }
        }
    }
    
    this.load_datagrid = function(result)
    {
        try
        {
            datatable = result.value;
       
            dbgrid = this.create_grid();
            dbgrid.filter.enabled = false;
            dbgrid.bind(datatable, 'dvgrvSummaryOfAmount', 'tblTEMPFILE');
            document.getElementById("tblTEMPFILE_footerbtn_Edit").style.display = 'none';
            document.getElementById("tblTEMPFILE_pg_prev").style.display = 'none';
//            document.getElementById("tblTEMPFILE_pg_start").style.display = 'none';
//            document.getElementById("tblTEMPFILE_pg_end").style.display = 'none';
            document.getElementById("tblTEMPFILE_pg_next").style.display = 'none';
            document.getElementById("tblTEMPFILE_pg_select").style.display = 'none';
            PCHCx.show_grid();
        }
        catch(e)
        {
            alert('Cant load DataGrid.');
        }
    }
    
    this.show_grid = function()
    {
    
        var dom = document.getElementById("dvgrvSummaryOfAmount")
//        var xy = calcpos(document.getElementById("divpopProcess"));
//            dom.style.left = "50%"  ;
//            dom.style.top = "2";       
            dom.style.display = 'block';
            document.getElementById("dvPCHCExtractSummary").style.display = 'block';
//            showpanel('dvgrvSummaryOfAmount');                      
    
    }
    
    this.create_grid = function()
    {
        gv = new datagrid();
        {
            var col = new datagrid_column();
            col.header = 'Currency';
            col.datafield = 'Currency';
            col.align = 'center';
            col.styles.cell +='width:auto';
            col.editable = false;
            gv.addColumn(col);
        }
        
        {
            var col = new datagrid_column();
            col.header = 'Gross Amount';
            col.datafield = 'total_amount';
            col.align = 'center';
            col.styles.cell +='width:auto';
            col.editable = false;
            gv.addColumn(col);
        }
        
        {
            var col = new datagrid_column();
            col.header = 'Items';
            col.datafield = 'total_items';
            col.align = 'center';
            col.styles.cell +='width:auto';
            col.editable = false;
            gv.addColumn(col);
        }
        
        {
            var col = new datagrid_column();
            col.header = 'Download';
            col.datafield = '';
            col.align = 'center';
            col.value = 'Download'
            col.type = 'button';
            col.onclick = 'PCHCx.open_download';
            col.styles.cell +='width:auto';
            //col.editable = false;
            gv.addColumn(col);
        }
        
        gv.row.onclick = 'PCHCx.click_edit';
        gv.footer.edit_button.none = false;        
        return gv;
    }
    
    this.click_edit = function()
    {
        var edtbtn = document.getElementById("tblTEMPFILE_footerbtn_Edit");
        edtbtn.click();
        edtbtn.style.display = 'none';
    }
    
    this.open_download = function()
    {
        window.open('../../IRSFileLoader/FileDownloader.aspx')
    }
}

function Page_Setup()
{
    AjaxPro.timeoutPeriod = 100 * 1000;
    PCHCx.getBankCode();
}

function clearMe()
{       
    PCHCx.ClearDDL(document.getElementById('ddFileNo'));
    PCHCx.ClearDDL(document.getElementById('ddSFileNo'));     
           
    document.getElementById('ddlExtract').value = '';
    document.getElementById('txtDateOfTransfer').value = '' 	           
    document.getElementById('dvPCHCExtractSummary').style.display = 'none'
}

function showPanel(divName)
{
    
    var div = document.getElementById(divName);
    
    divmask = new maskpanel();
    divmask.show();
    
    document.getElementById(divName).style.display = "block";
    
    div.style.top = (((document.documentElement.clientHeight / 2) + document.documentElement.scrollTop) - (div.offsetHeight / 2)) + 'px';
    div.style.left = (((document.documentElement.clientWidth / 2) + document.documentElement.scrollLeft) - (div.offsetWidth / 2)) + 'px';
}

function hidePanel(divName)
{
    document.getElementById(divName).style.display = "none";
    divmask.hide();
}