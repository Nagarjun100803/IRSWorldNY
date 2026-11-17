var db_grid
var x_year;
var y_year;
var guserrolecode;
var gparamforapproval;

$(document).ready(function () {
    getrolecode();
});

function getrolecode() {
    try {
        IRSWorld.BranchSetup.Holidays.getrolecode(function (result) {
            if (result.error == null) {
                guserrolecode = result.value;
            }
            else {
                alert("Failed to get role, please reload the page");
            }
        });
    }
    catch (err) { //err.message
        spawn_errpage('Failed to get role. Please try again.');
    }
}
function getParamForApproval(brCode) {
    try {
        IRSWorld.BranchSetup.Holidays.getParamForApproval(brCode, function (result) {
            if (result.error == null) {
                return result.value;
            }
            else {
                alert("Failed to check for approval, please reload the page");
                return "error"
            }
        });
    }
    catch (err) { //err.message
        spawn_errpage('Failed to get role. Please try again.');
    }
}
function fixht()
{
    document.getElementById('divBody').style.height = (top.document.getElementById('divCenter').offsetHeight - 30) + 'px';
    document.getElementById('divBody').style.width = (top.document.getElementById('divCenter').offsetWidth - 3) + 'px';
}

function fixmain()
{
    var brmain = false;
    try
    {
         brmain = parent.is_branchsetup_main();
    }
    catch(e){}
    
    if (!brmain)
    {
        document.getElementById('theBody').onresize = fixht;
        document.getElementById('divBody').style.overflow = 'auto';
        
        fixht();
    }        
} 
        function holiday_dropdown()
        {
            this.commons           = null;  
            this.Load_Branches = function()
            {
                IRSWorld.BranchSetup.Holidays.getBranches(0,this.load_ddlBranches_callback);
            }
            
            this.load_ddlBranches_callback = function(result)
            {
                if (result.value != null && result.value != "undefined")
                {
                    this.commons = new common_method();
                    this.commons.to_load_dropdownlist("ddlBranch", result.value, "BranchCode", "BranchName");
                }
            }
            set_visibility('Main');
            
         }
        
        function create_grid()
        {
            dbgrid = new datagrid();

            {
    		    var col = new datagrid_column();
    		    col.header = 'Date';
    		    col.value = 'holdate'
    		    col.datafield = 'HOL_DATE';
    		    col.onchange = 'checkEnteredDate';
    		    col.align = 'left';
    		    col.styles.cell += 'width:20%';
    		    dbgrid.addColumn(col);
            }
            
            {
    		    var col = new datagrid_column();
    		    col.header = '';
    		    col.datafield = '';
    		    col.type = 'image';
    		    col.value = 'Calendar';
    		    col.src = '../images/calendar/imgcalen.png';
    		    col.onclick = 'popout_date';
    		    col.align = 'center';
    		    col.styles.cell += 'width:5%';
    		    col.styles.datafield += 'background-color:skyblue;color:ivory;border:solid 1px navy';
    		    dbgrid.addColumn(col);   
            } 
            {
    		    var col = new datagrid_column();
    		    col.header = 'Description';
    		    col.datafield = 'DESCRIPTION';
    		    col.align = 'right';
    		    col.styles.cell += 'width:60%';
    		    col.onchange = 'check_char_inputted';
    		    dbgrid.addColumn(col);
            }
            
            {
    		    var col = new datagrid_column();
    		    col.header = 'Recurring';
    		    col.datafield = 'Recurring';
                col.type = 'check';
    		    col.align = 'left';
    		    col.styles.cell += 'width:8%';
    		    dbgrid.addColumn(col);
            }
            
            {
    		    var col = new datagrid_column();
    		    col.header = 'No Clearing';
    		    col.datafield = 'NO_CLEAR';
                col.type = 'check';
    		    col.align = 'left';
    		    col.styles.cell += 'width:8%';
    		    dbgrid.addColumn(col);
            }
            
            {
    		    var col = new datagrid_column();
    		    col.header = 'Holiday';
    		    col.datafield = 'HOLIDAY';
                col.type = 'check';
    		    col.align = 'left';
    		    col.styles.cell += 'width:8%';
    		    dbgrid.addColumn(col);
            }

            {
    		    var col = new datagrid_column();
    		    col.header = 'SEQ ID';
    		    col.styles.header += 'display:none;';
    		    col.styles.datafield += 'display:none;';
    		    col.datafield = 'SEQ_ID';
    		    col.align = 'left';
    		    col.styles.cell += 'width:8%;display:none;';
    		    dbgrid.addColumn(col);
            }
            {
    		    var col = new datagrid_column();
    		    col.styles.header += 'display:none;';
    		    col.styles.datafield += 'display:none;';
    		    col.datafield = 'DAYOFWEEK';
    		    col.align = 'left';
    		    col.styles.cell += 'width:8%;display:none;';
    		    dbgrid.addColumn(col);
            }
//            {
//            var col = new datagrid_column();
//		    col.header = '';
//		    col.datafield = '';
//		    col.align = 'center';
//		    col.type = 'button';
//		    col.value = 'Edit';
//		    col.edit_button = true;
//		    col.styles.cell += 'width:10%';
//		    col.styles.datafield += 'width:100%;';
//		        dbgrid.addColumn(col);
//            }    
            
            
            dbgrid.caption = 'Holidays';
            dbgrid.classNames.table = 'table_body';
            dbgrid.rows_page = 10;
            dbgrid.row.normal_style = 'background-color: #f7ffff;'; 
            
            dbgrid.footer.edit_button.none = false;
            dbgrid.footer.edit_button.style += 'background-color: lavender;color:maroon;';            
            dbgrid.footer.edit_button.beforepost += 'add_before_post';
            
            dbgrid.footer.add_button.none = false;
            dbgrid.footer.add_button.style += 'background-color: lavender;color:maroon;';            
            dbgrid.footer.add_button.beforepost = 'add_before_post';
            
            dbgrid.footer.save_button.none = false;
            dbgrid.footer.save_button.onclick = 'save_holidays';  
            dbgrid.footer.save_button.style += 'background-color: lavender;'; 
            
            //dbgrid.footer.delete_button.onclick = 'delete_holidays';  
            dbgrid.footer.delete_button.none = false;
            
            dbgrid.footer.cancel_button.none = false;
            dbgrid.footer.cancel_button.style += 'background-color: lavender;';
        }
        
        
        function change_year(args)
        {
            x_year = dbgrid.datatable.Rows[args].Recurring.toString();
            var txt = document.getElementById(gridHolidays.id + '_txt_HOL_DATE_' + args.datarow);
            popUpCalendar(txt, txt, 'mm/dd/yyyy');
        }
        
        
        function add_before_post(args)
        {
            var holdesc = dbgrid.getValueAt(args.datarow,2);
            var holdate = dbgrid.getValueAt(args.datarow,0);
            var rec     = dbgrid.getValueAt(args.datarow,3);
            if(trim(holdesc) == null | trim(holdesc) == '')
            {
                alert('Holiday description must not be empty');
                dbgrid.curract = '';
                return;
            }
            else if(trim(holdate) == null | trim(holdate) == '')
            {
                alert('Holiday date must not be empty');
                dbgrid.curract = '';
                return;
            }
            for(var i=0;i<= dbgrid.datatable.Rows.length - 1;i++)
            {
                if(args.datarow != i)
                {
                    if(holdate == dbgrid.getValueAt(i,0) & holdesc == dbgrid.getValueAt(i,2))
                    {
                        alert('Duplicate record exist');
                        dbgrid.curract = '';
                        return;
                    }
                }
            
            }
            
            if(rec)
            {
                var split_date = document.getElementById(gridHolidays.id + '_txt_HOL_DATE_' + args.datarow).value.split('/');
                var month = split_date[0];
                var day = split_date[1];
                document.getElementById(gridHolidays.id + '_txt_HOL_DATE_' + args.datarow).value = month + '/' + day + '/' + 'xxxx'; //'1/1/xxxx';
            }
            else
            {
                try
                {
                    //var ms = stringToDate(document.getElementById(gridHolidays.id + '_txt_HOL_DATE_' + args.datarow).value);
                    //Date dt = new Date(document.getElementById(gridHolidays.id + '_txt_HOL_DATE_' + args.datarow).value);
                }
                catch(e)
                {
                    alert('Invalid Date');
                    return;
                }
            }
        }
        
        function popout_date(args)
    	{
    	    var btn = document.getElementById(args.control);
    	    var txt = document.getElementById(gridHolidays.id + '_txt_HOL_DATE_' + args.datarow);
    	    popUpCalendar(txt, txt, 'mm/dd/yyyy');
    	}
    	
    	function checkEnteredDate(args) 
    	{
            var inputDate = document.getElementById(args.control);
            //if statement to check for valid date
            var formatDate = new Date(inputDate.value);
            if(formatDate == NaN | formatDate == 'NaN')
            {
                alert('Invalid Date! Please check the date inputted');
                return;
            }
            
        }

    	
    	
        

        function bind_grid(currgrp)
        {
            if (currgrp.error != null)
            {
                spawn_errpage(currgrp.error.Message);
                return;
            }
            
            create_grid();
            ajax_show_status(true);
            dbgrid.bind(currgrp.value, 'tdHolidays', 'gridHolidays');
            ajax_show_status(false);
        } 
        
     function populate_rec()
     {
        var brcode = document.getElementById('ddlBranch').value;
        IRSWorld.BranchSetup.Holidays.getHoliday(brcode,bind_grid);
        set_visibility('Holidays');
     }
         
       function set_visibility(tbl)
        { 
            if(tbl=='Holidays')
            {
                document.getElementById('tblHolidays').style.display="block";
                document.getElementById('tblMain').style.display="none";
                document.getElementById('btnBack').style.display="block";
                
            }
            else
            {
                document.getElementById('tblHolidays').style.display="none";
                document.getElementById('tblMain').style.display="block"; 
                document.getElementById('btnBack').style.display="none";
            }
        }  
        
        function save_holidays(args)
       {
            if (guserrolecode == '2') {
                alert('Approver is not allowed to modify current set-up.')
                return;
            }

            var val='';
            if (args.value == dbgrid.footer.save_button.value)
            {        
                var brcode = document.getElementById('ddlBranch').value;
                //var pfa = IRSWorld.BranchSetup.Holidays.getParamForApproval(brcode);
                //if(pfa == '1'){
                //    alert('There is still pending for approval under this branch code');
                //    return;
                //}
                //else if (pfa == 'error') {
                //    return;
                //}
                val = IRSWorld.BranchSetup.Holidays.applyEdits(args.additions, args.deletes, args.edits,brcode,updates_saved);
               
            }
        }
        
        function check_char_inputted(args)
        {
            var val;
            var _desc = document.getElementById(gridHolidays.id + '_txt_DESCRIPTION_' + args.datarow).value;
            if(_desc=='' | _desc == 'undefined' | _desc == null)
            {
                alert('Description must not be empty!');
                return;
            }
        
        
            var txt = document.getElementById(gridHolidays.id + '_txt_DESCRIPTION_' + args.datarow);
            var isinvalid = isSpecial(txt.value);
            if(isinvalid)
            {
                alert('Error! Special character(s) not allowed');
                document.getElementById(gridHolidays.id + '_txt_DESCRIPTION_' + args.datarow).value = '';
                return;
            }
        }        
//        function delete_holidays(args)
//        {
//            if (args.value == dbgrid.footer.delete_button.value)
//            {  
//                if(confirm('Are you sure you want to delete this record?'))
//                {
//                    var brcode = document.getElementById('ddlBranch').value;                       
//                    Holidays.DeleteHolidays(indx,brcode,updates_saved);
//                }
//                updates_saved('');
//            }
//        }
        
        function updates_saved(cm)
        {   
            if (cm.error != null)
            {
                alert(cm.error.Message);
                populate_rec();
                return;
            }
            else {
                alert('Holiday change request has been submitted for approval');
                populate_rec();
            }
            //alert('Holiday successfully saved!');
            
        }
        
        function page_setup()
        {
            if (top == self)
            {
                document.irs_loader.submit();
                fixmain();
            }
        }

        function validate_holidays()
        {
            if(document.getElementById('ddlBranch').value != 'null')
            {
                populate_rec();
                //set_visibility('Main');
            }
            else
            {
                alert('Please select branch');
            }
        }        

        function set_init_value()
        {
            
            var loc = IRSWorld.BranchSetup.Holidays.StartUpValue();
            this.commons = new common_method();
            this.commons.to_set_dropdownlist_index('ddlBranch',loc.value);
        }