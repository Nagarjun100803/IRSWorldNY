var ajax_timeout = 60 * 1000;
var dbgrid = null;

/*
    [SEQ_ID] [bigint] IDENTITY(1,1) NOT NULL,
    [BatchNO] [bigint] NOT NULL,
    [BatchDate] [datetime] NOT NULL,
    [BarCodeNo] [varchar](19) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [EPSCO_SEQID] [bigint] NULL,
    [CreatedBy] [varchar](32) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [AssignedBy] [varchar](32) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    [DateAssigned] [datetime] NULL,
*/
function hide_menu()
{
    var div = document.getElementById("divMenu");
    var cset = div.style.display;
    div.style.display = (cset == "") ? "none" : "";
    document.getElementById("tdMenu").style.width = (cset == "") ? "1%" : "18%";
    setText(document.getElementById("ahider"), ((cset == "") ? String.fromCharCode(187) : String.fromCharCode(171)));
}   

function close_bcgen()
{
    var rs = IRSWorld.EPSCO.EPSCOBarcodeGenerator.close_bcgen();
    if (top.document.getElementById('divCenter'))
    {
        top.show_center();    
    }
}

function show_for_print()
{
    ajax_show_status(true);
    IRSWorld.EPSCO.EPSCOBarcodeGenerator.getBarcodesForPrinting(barcodes_generated);
}

function create_gen_grid()
{
    dbgrid = new datagrid();
     { //0
        var col = new datagrid_column();
        col.header = 'ID';
        col.datafield = 'SEQ_ID';
        col.align = 'center';
        col.styles.cell += 'width:5%;display:none';
        col.styles.header += 'display:none';
        col.styles.datafield += 'display:none';
        dbgrid.addColumn(col);	
    }             
    { //1
	    var col = new datagrid_column();
	    col.header = 'Batch No.';
	    col.datafield = 'BatchNO';
	    col.align = 'right';
	    col.styles.cell += 'width:20%';
	    col.styles.datafield += 'width:100%';
	    dbgrid.addColumn(col);
    }
    { //2
	    var col = new datagrid_column();
	    col.header = 'Date Created';
	    col.datafield = 'BatchDate';
	    col.align = 'center';
	    col.styles.cell += 'width:20%';
	    col.styles.datafield += 'width:100%';
	    dbgrid.addColumn(col);
    }
    { //3
	    var col = new datagrid_column();
	    col.header = 'Barcodes';
	    col.datafield = 'BarCodeNo';
	    col.align = 'left';
	    col.styles.cell += 'width:40%';
	    col.styles.datafield += 'width:100%';
	    dbgrid.addColumn(col);
    }
    { //4
	    var col = new datagrid_column();
	    col.header = 'Creator';
	    col.datafield = 'CreatedBy';
	    col.align = 'left';
	    col.styles.cell += 'width:20%';
	    col.styles.datafield += 'width:100%';
	    dbgrid.addColumn(col);
    }  
                    
    dbgrid.caption = 'Generated Barcodes';
}

function print_gen()
{
    if (confirm("Export generated barcodes To .CSV for printing?"))
    {
        document.getElementById('btnHidden').click();
        close_gen();
    }
}

function barcodes_generated(rs)
{
    ajax_show_status(false);
    if (rs.error != null)
    {
        if (rs.error.Message.indexOf('Expired.') > -1)
        {
            spawn_errpage(rs.error.Message);
            return;
        }
        alert("Error retreiving generated barcodes. " + rs.error.Message);
        cancel_gen();
        return;
    }
    ajax_show_status(true);
    create_gen_grid();
    dbgrid.bind(rs.value, 'divGridContainer', 'dbgrid');
    
    var div = document.getElementById('divGrid');
    div.style.zIndex = 10;
    div.style.position = 'absolute';
    div.style.top = 0;
    div.style.left = 0;
    div.style.backgrounColor = 'DimGray';
    div.className = 'opacified';
    div.style.display = '';
    document.getElementById('btnPrintGen').disabled = (rs.value.Rows.length > 0) ? '' : 'disabled';
    ajax_show_status(false);
}

function close_gen()
{
    var div = document.getElementById('divGrid');
    div.style.display = 'none';
    cancel_gen();
}

function start_gen()
{
    document.getElementById('btnGenerate').disabled = "disabled";
    document.getElementById('txtNum2Gen').readOnly = true;
    var val = document.getElementById('txtNum2Gen').value;
    if (confirm('Generate ' + val + ' Barcodes?'))
    {
        ajax_show_status(true);
        IRSWorld.EPSCO.EPSCOBarcodeGenerator.generate_barcodes(val, barcodes_generated);
    }
    else
    {
        cancel_gen();
    }
}

function cancel_gen()
{
    document.getElementById('btnGenerate').disabled = "";

    var txt = document.getElementById('txtNum2Gen');
    txt.readOnly = false;
    txt.value = 0;
    txt.focus();
    txt.select();
    
    dbgrid = null;
}

function blur_num2gen(val)
{
    if (!isNumeric(val)) 
    {
        alert('The value entered is not numeric');
        document.getElementById('txtNum2Gen').focus();
        document.getElementById('txtNum2Gen').select();
        return false;
    }
    return true;
}

function fixdim()
{
    var topcent = top.document.getElementById('divCenter');
    if (topcent)
    {
        var div = document.getElementById('divMain');
        if (div)
        {
            div.style.width = topcent.offsetWidth + 'px';
            div.style.height = (topcent.offsetHeight) + 'px';
            
            var td = document.getElementById('tdMain');
            if (td)
            {
                td.style.width = div.style.width;
                td.style.height = div.style.height;
            }
        }
        div = document.getElementById('divGrid');
        if (div);
        {
            div.style.width = topcent.offsetWidth + 'px';
            div.style.height = (topcent.offsetHeight) + 'px';
            var td = document.getElementById('tdGrid');
            if (td)
            {
                td.style.width = div.style.width;
                td.style.height = div.style.height;
            }                                    
        }
    }
}

function page_setup()
{
    if (top != self)
    {
        fixdim();
        document.getElementById("divMain").style.display = '';
        AjaxPro.timeoutPeriod = ajax_timeout;
        document.getElementById('txtNum2Gen').focus();
        document.getElementById('txtNum2Gen').select();
    }
    else
    {
        var act = "../Main/IRSMain.aspx?load=../EPSCO/EPSCOBarcodeGenerator.aspx";
        window.open(act, "_top");
    }        
}
