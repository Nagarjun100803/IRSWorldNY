var report_name;
var sp_name;
var uid;
var loc;
var fixed;
var tbl_src;
var exp_type;
var roleid;

function page_setup()
{
    show_hide_details('none');
    uid = APASReports.get_irsclient_data('UID').value;
    loc = APASReports.get_irsclient_data('LOCATION').value;
    roleid = APASReports.get_irsclient_data('ROLE').value;
    load_report_list();
    export_type();
    if(loc == '1170')
    {
        document.getElementById('exp_cell').style.display = 'block';
        document.getElementById('exp_data').style.display = 'block';
    }
    else
    {
        document.getElementById('exp_cell').style.display = 'none';
        document.getElementById('exp_data').style.display = 'none';
    }
    
}


function show_hide_details(hidden)
{
    document.getElementById('tblReportParam').style.display = hidden;
}


function load_report_list()
{
    APASReports.getAllReports(uid,roleid,load_report_list_callback);
}

function load_report_list_callback(result)
{
    if (result != null && result != "undefined")
    {
        this.commons = new common_method();
        this.commons.to_load_dropdownlist("ddlTables", result.value, "RptID", "ReportTitle");
    }
}



function create_dynamic_fields()
{
    show_hide_details('block');
    var rptid = document.getElementById('ddlTables').value;
    var dt = APASReports.getReportData(roleid,rptid).value;
    var hit=0;
    
    if(dt.Rows.length >0)
    {
        sp_name = dt.Rows[0].spName.toString();
        report_name = dt.Rows[0].RptNm.toString();
        
        removeTableRow();
        var theTable = document.getElementById("tblReportParam");
        var tb = theTable.tBodies[0];
        var newCell;
        
        /*Datatable should be here*/
        for(var x=0;x<=dt.Rows.length - 1;x++)
        {
        
            var rptdesc = dt.Rows[x].VarNm.toString();
            var rptid = dt.Rows[x].VarID.toString();
            rptdesc = rptdesc.split(',');
        
            for(var y=0;y<= rptdesc.length - 1;y++)
            {   
                var rowData = [rptdesc[y], rptid[y]];
                var newRow = tb.insertRow(tb.rows.length);
                //document.getElementById('tblReportParam').style.display = (fixed) ? 'none' : 'block';
                fixed = false; //reset fix
            
                for(var i=0;i<=rowData.length - 1;i++)
                {
                    
                    newCell = newRow.insertCell(i);
                    if (i == 0)
                    {                          
                          newCell.innerHTML =  rowData[i]
                          newCell.id = rowData[i];
                          newCell.className='cell_caption'
                          newCell.style.width = "40%"                                             
                   }
                   else
                   {
                       var type = APASReports.getDataType(rowData[0]).value;
                       
                       if(type=='Date')
                       {
                          var el = document.createElement('input')
                          el.type = 'text';
                          el.name = 'lbl' + rowData[0];//'MerchantInfo'; //'dt' + dfld;
                          el.id = 'txt' + rowData[0];//'dt' + dfld;
                          el.readOnly = true;
                          el.style.width = 'auto';
                          newCell.appendChild(el);
                          
                          var el2 = document.createElement('img')
                          el2.src = '../images/calendar/imgcalen.png';
                          el2.alt = 'Pick a date.'
                          el2.id = 'img' + rowData[0];
                          newCell.appendChild(el2);
                          var popc = 'popUpCalendar(this,document.forms[0].' + el.id + ',"m/dd/yyyy",document.getElementById("divmain"))';
                          document.getElementById(el2.id).onclick = Function(popc);

                       }
                       else if(type=='Combo')
                       {
                           if(rowData[0] == 'BranchCode' | rowData[0] == 'Location' | rowData[0] == 'PartnerCode' | rowData[0] == 'PNBBranch')
                           {  
                                fixed = APASReports.isFixed(rowData[0]).value;  //fixed is applicable only in branchcode, if the user wants to get branchcode in clientdata else display all the list of branches.
                           }
                          
                           var el = document.createElement('select')
                           el.id = 'ddl' + rowData[0];
                           el.width = "60%"
                           newCell.appendChild(el);
                           tbl_src = APASReports.tbl_src(rowData[0]).value;
                           popDDL(el,tbl_src);
                           
                           if(fixed)
                           {
                                el.name = (fixed) ? 'ddl' + rowData[0] + '-' + 'true' : 'ddl' + rowData[0] + '-' + 'false';
                                document.getElementById('ddl' + rowData[0]).style.display = 'none';
                                document.getElementById(rowData[0]).style.display = 'none';
                           }
                       }
                       else if(type == 'Text')
                       {
                           if(rowData[0] == 'DatePaid')
                           {  
                                fixed = APASReports.isFixed(rowData[0]).value;  //fixed is applicable only in branchcode, if the user wants to get branchcode in clientdata else display all the list of branches.
                           }
                           var el = document.createElement('input')
                           el.type = 'text';
                           el.id = 'txt' + rowData[0];
                           el.width = "60%"
                           newCell.appendChild(el);
                           if(fixed)
                           {
                                el.name = (fixed) ? 'txt' + rowData[0] + '-' + 'true' : 'txt' + rowData[0] + '-' + 'false';
                                document.getElementById('txt' + rowData[0]).style.display = 'none';
                                document.getElementById(rowData[0]).style.display = 'none';
                                
                           }
                       }
                   }
                
                }
            }
        }
    }
    else
    {
        alert('No Report found!');
        return;
    }
}

function popDDL(DDL, src)
{   
    var arr = src.split(',');
    var dt =  create_select_statement(arr[0],arr[1]);
    var dt;  
    var dt2; 
    cleanDDL(DDL);
    
    if(arr[0] == 'Year')
    {
        var d = new Date();
        for (var y = 2007;y<= d.getFullYear();y++)
        {
            var opt = document.createElement("option");                 
            opt.text = y;
            opt.value = y;
            DDL.options.add(opt);
        }
    }
    else if(arr[0] == 'Month')
    {
        var x=0;
        var month=new Array(12);
        month[0]="January";
        month[1]="February";
        month[2]="March";
        month[3]="April";
        month[4]="May";
        month[5]="June";
        month[6]="July";
        month[7]="August";
        month[8]="September";
        month[9]="October";
        month[10]="November";
        month[11]="December";
        
        for (var y = 0;y<= month.length;y++)
        {
            x++;
            var opt = document.createElement("option");                 
            opt.text = month[y];
            opt.value = x;
            DDL.options.add(opt);
        }
    }
    
    
    
    if (dt != null && dt.Rows.length > 0)
    {   
        if(arr[0] == 'TBL_LOOKUP')
        {
            for (var i = 0; i < dt.Rows.length; i++)
            {
                var opt = document.createElement("option");                 
                opt.text = dt.Rows[i].FldValue.toString() + '-' + dt.Rows[i].FldDescription.toString();
                opt.value = dt.Rows[i].FldCode.toString();
                DDL.options.add(opt);
            }
        }
        
        else
        {   
            var opt = document.createElement("option");                 
            opt.text =  'ALL';
            opt.value = 'ALL';
            DDL.options.add(opt);  
            for (var i = 0; i < dt.Rows.length; i++)
            {
                var opt = document.createElement("option");                 
                opt.text = dt.Rows[i].BranchCode.toString() + '-' + dt.Rows[i].BranchName.toString();
                opt.value = dt.Rows[i].BranchCode.toString();
                DDL.options.add(opt);
            }
        }      
    }
}


function cleanDDL(DDL)
{        
    DDL.length = 0;
}  

function create_select_statement(tbl_name,where_clause)
{
    return APASReports.getSQL(tbl_name,where_clause).value;
}


function export_type()
{

    var DDL = document.getElementById('ddlExport');
    var proc = document.createElement('option');
    proc.text = 'EXCEL'
    proc.value = 'EXCEL';
    DDL.options.add(proc);
    
    var can = document.createElement('option');
    can.text = 'PDF'
    can.value = 'PDF';
    DDL.options.add(can);

}

function load_report()
{
    var val = get_data_value("tblReportParam");
    ajax_show_status(true);
    load_report_callback(true,val.substring(0,val.length -1));
    //APASReports.load_report(val.substring(0,val.length -1),report_name,sp_name,load_report_callback);
}


function load_report_callback(result,value)
{
    //window.open('../Reports/Pages/Report_viewer.aspx?value ='+ value +'&apas='+ result.value + '&mergeparam=' + result.value + ';' + result.value +  '&paramvalues=' + result.value + ';' + result.value + '&exporttype='+ document.getElementById('ddlExport').value + '&autoprint=false','mywindow','');
    var open = window.open('../Reports/Pages/Report_viewer.aspx?spname='+ sp_name +'&rptnm='+ report_name +'&value='+ value +'&apas='+ result.value + '&mergeparam=' + result.value + ';' + result.value +  '&paramvalues=' + result.value + ';' + result.value + '&exporttype='+ document.getElementById('ddlExport').value + '&autoprint=false','','toolbar=no,location=no, directories=no, status=0,resizable=yes,titlebar=no,fullscreen=no');
    //show_frame_dialog('../Reports/Pages/Report_viewer.aspx?spname='+ sp_name +'&rptnm='+ report_name +'&value='+ value +'&apas='+ result.value + '&mergeparam=' + result.value + ';' + result.value +  '&paramvalues=' + result.value + ';' + result.value + '&exporttype='+ document.getElementById('ddlExport').value + '&autoprint=false', 'test_close', '100%', '100%', 0); 
    ajax_show_status(false);
}



function test_close()
{

}

function get_data_value(tabid)/*This Validates merchant details input*/
{
    var tabid = document.getElementById(tabid);
    var strreturn = '';
    var comma_value = '';
    tblrows = tabid.rows;
    if (tblrows == undefined) return;
    for (var i = 0; i < tblrows.length; i++)
    {
        var sells = tblrows(i).cells;
        for (var j = 0; j < sells.length; j++)
        {
            if (sells.item(j).hasChildNodes())
            {
                var childs = sells.item(j).childNodes;
                for (var k=0; k < childs.length; k++)
                {
                    if (childs(k).nodeName == 'INPUT') //&& childs(k).type == 'text')
                    {
                         var arr = childs(k).name.split('-');
                         if(arr.length >0)
                         {
                            isfix = arr[1];
                         }
                         else
                         {
                            isfix = false;
                         }
                             if(!isfix)
                             {
                                comma_value +=  childs(k).value + ',';
                             }
                             else
                             {
                                comma_value +=  dateToString(APASReports.get_branch_localtime(loc).value) + ',';
                             }
                    }
                    else if (childs(k).nodeName == 'SELECT')
                    {
                        if (childs(k).length > 0)
                        {
                             var arr = childs(k).name.split('-');
                             if(arr.length >0)
                             {
                                isfix = arr[1];
                             }
                             else
                             {
                                isfix = false;
                             }
                                  if(!isfix) //applicable only in branches
                                  {
                                    comma_value +=  childs(k).value + ',';
                                  }
                                  else
                                  {
                                    comma_value += loc + ',';
                                  }
                        }
                    }
                    else if (childs(k).nodeName == 'TEXTAREA')
                    {

                       comma_value += childs(k).value + ',';
                    } 
                                 
                }
            }
        }        
    }
    return comma_value;

}


function removeTableRow() 
{
    //var theTable = document.getElementById("tblBillsPayOBPSTransactionsMDetails");
    var theTable = document.getElementById("tblReportParam");
    var theTableBody = theTable.tBodies[0];
    var lastrow = theTableBody.rows.length;
    
    for (var i = 0; i < lastrow; i++)
    {
         theTableBody.deleteRow(theTableBody.rows.length - 1)
    }
}