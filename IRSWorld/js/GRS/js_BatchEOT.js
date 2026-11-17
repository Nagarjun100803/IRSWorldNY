var dbgrid;
var userBranch;
var tranxDate;
var dateTransfer;
var fileInd;
var cntMsg;
var rows;

function hide_show()
{
    this.chkbox = function()
    {   
        var chkReload = document.getElementById("chkReload");

            document.getElementById("trReupload").style.display = (chkReload.checked == true) ? 'block' : 'none';
            document.getElementById("txtDateTransfer").disabled = (chkReload.checked == true) ? false : true;
            document.getElementById("txtDateTransfer").value = (chkReload.checked == true) ? "" : dateTransfer;
            document.getElementById("imgCalendar").style.display = (chkReload.checked == true) ? 'block' : 'none';
            document.getElementById("txtFileIndicator").disabled = (chkReload.checked == true) ? false : true;
            document.getElementById("txtFileIndicator").value = (chkReload.checked == true) ? "" : fileInd;
            document.getElementById("txtOverrideDate").disabled = (chkReload.checked == true) ? false : true;
            document.getElementById("btnOK").disabled = (chkReload.checked == true) ? false : true;
            document.getElementById("btnCancel").disabled = (chkReload.checked == true) ? false : true;
            document.getElementById("chkOverride").disabled = (chkReload.checked == true) ? false : true;
            document.getElementById("chkReupload").checked = false;
    }
    
    this.override = function()
    {
        var chkOverride = document.getElementById("chkOverride");
        var dateOverride = document.getElementById("txtOverrideDate").value;
        document.getElementById("trOverrideDate").style.display = (chkOverride.checked == true) ? 'block' : 'none';
        document.getElementById("txtOverrideDate").value = (chkOverride.checked == true) ? dateOverride : '';
    }
}

function page_load()
{  
    userBranch = Batch_EOT.getUserBranch().value;
    tranxDate = Batch_EOT.getTransactionDate(userBranch, false).value;

    dateTransfer = tranxDate.getMonth()+1 + "/" + tranxDate.getDate() + "/" + tranxDate.getFullYear();
       // dateTransfer = '11/16/2010';
    fileInd = Batch_EOT.getFileInd().value;
    cntMsg = Batch_EOT.checkEOT(dateTransfer).value;
            
    document.getElementById("txtDateTransfer").value = dateTransfer;
    document.getElementById("txtFileIndicator").value = fileInd;
        
    document.getElementById("btnOK").disabled = (cntMsg[0] == 0) ? true : false;
    document.getElementById("btnCancel").disabled = (cntMsg[0] == 0) ? true : false;
    document.getElementById("chkOverride").disabled = (cntMsg[0] == 0) ? true : false;
       
    alert(cntMsg[1]);

}

function saveStatus()
{
    var dateTransfer = document.getElementById("txtDateTransfer").value;
    var fileInd = document.getElementById("txtFileIndicator").value;
    var desc = "Initializing...";
    var overrideDate = document.getElementById("txtOverrideDate").value;
    var reload = document.getElementById("chkReload").checked;
    var reupload = document.getElementById("chkReupload").checked;
    var status = 0;

    var re_upload = (reload==true) ? ((reupload==true) ? 2 : 1) : 0;
    
    Batch_EOT.saveStatus(dateTransfer, parseInt(fileInd), desc, overrideDate, parseInt(re_upload), parseInt(status));
    
    checkStatus();
}

function showPanel(divName)
{   
    var div = document.getElementById(divName);
    
    divmask = new maskpanel();
    divmask.show();
    
    document.getElementById(divName).style.display = 'block';
    
    div.style.top = (((document.documentElement.clientHeight / 2) + document.documentElement.scrollTop) - (div.offsetHeight / 2)) + 'px';
    div.style.left = (((document.documentElement.clientWidth / 2) + document.documentElement.scrollLeft) - (div.offsetWidth / 2)) + 'px';
}

function hidePanel()
{
    var divmask = document.getElementById("xmaskframe");
    var divobj = document.getElementById("xdivmasking");
    divmask.style.display = divobj.style.display = "none";
    document.getElementById('divImgLoad').style.display = 'none';
}

function showProcess()
{
    showPanel('divImgLoad');
    new common_method().to_show_loading_message('divLoading');
    document.getElementById("lblLoad").innerHTML = "Processing EOT. Please wait...";
}

function create_grid()
{
    dbgrid = new datagrid();
    {
        var col = new datagrid_column();
        col.header = 'Transaction Source';
        col.datafield = 'transource';
        col.align = 'left';
        col.styles.cell += 'width:18%;';
        col.editable = false;
        dbgrid.addColumn(col);
    }
    {
        var col = new datagrid_column();
        col.header = 'Items';
        col.datafield = 'items';
        col.align = 'left';
        col.styles.cell += 'width:18%;';
        col.editable = false;
        dbgrid.addColumn(col);
    }
    
    dbgrid.caption = 'Batch EOT Summary';
    dbgrid.classNames.table = 'table_body';
    dbgrid.rows_page = 12;
    dbgrid.row.normal_style = 'background-color: #f7ffff;'; 
    dbgrid.footer.none = true;
}

function populate_rec()
{
    var SEQ_ID = 1;
    
    Batch_EOT.showSummary(SEQ_ID, bind_grid);
}

function bind_grid(SEQ_ID)
{
    if (SEQ_ID.error != null)
    {
        spawn_errpage(SEQ_ID.error);
        return;
    }
    
    rows = SEQ_ID.value.Rows.length;

    document.getElementById("tblBtnReport").style.display = (rows > 0) ? 'block' : 'none';
    document.getElementById("trBack").style.display = (rows > 0) ? 'none' : 'block';
    
    create_grid();
    ajax_show_status(true);
    
    dbgrid.bind(SEQ_ID.value, 'tdEOTsummary', 'gridEOTsummary');
    ajax_show_status(false);
} 

function checkStatus()
{   
    showProcess();
    var stat = Batch_EOT.checkStatus().value;
    
    if(stat[1] == 2)
    {
        showSummary();
        alert(stat[0]);
        return;
    }
    else if(stat[1] == 3)
    {
        hidePanel();
        alert(stat[0]);
        return;
    }
    else
    {
        setTimeout("checkStatus();",3500);
    }

}

function showSummary()
{
    hidePanel();
    
    document.getElementById("imgCalendar").style.display = 'none';
    document.getElementById("txtDateTransfer").disabled = true;
    document.getElementById("txtFileIndicator").disabled = true;
    document.getElementById("chkOverride").disabled = true;
    document.getElementById("txtOverrideDate").disabled = true;
    document.getElementById("tblChkbox").style.display = 'none';
    document.getElementById("tblEOTsummary").style.display = 'block';
    populate_rec();

    
    
}