
var msg_grid;
var userid = '';
var userbr = '';
var tgrid;
var issa;
var isba;
var ispop;
var pop_function;
var gbtnclick = "";

function reset_search(line_no)
{
    document.getElementById('btnReset').disabled = (line_no == 0);
    document.getElementById('txt_search_point').value = '' + line_no;  
} 

function btn_find_click()
{
    var val = document.getElementById('txtfind').value;
    if (val.length == 0) 
    {
        alert('Find needs search criteria.');
        return;
    }            
    
    var col1 = "ID";
    var col2 = "Description";
    
    var find_args = new datagrid_find_arguments();
    var farg = new datagrid_find_argument();
    farg.column_name = col1;
    farg.operator = "search";
    farg.value = val;
    find_args.addArgument(farg);
    find_args.addConjunction('|');
    farg = new datagrid_find_argument();
    farg.column_name = col2;
    farg.operator = "search";
    farg.value = val;
    find_args.addArgument(farg);
    find_args.select_row = false;
    find_args.start_row = parseInt(document.getElementById('txt_search_point').value);
    var locrow = tgrid.find(find_args, true);
    
    if (locrow == -1)
    {
        alert("No row found.");
        reset_search(0);
    }
    else
    {
        reset_search( (locrow == (tgrid.dataRowsSize() - 1)) ? 0 : locrow + 1 );
    }
    
    find_close_popup();
} 
              
function show_popup_find(args)
{
    var div = document.getElementById('divPopFind');
    var td = document.getElementById('tdPopFind');
    var topsize = top.document.getElementById('divCenter').offsetHeight;
    var topparent = document.getElementById('divBody').offsetHeight;
    div.className = 'div_undock';
    div.style.height = (topsize > topparent) ? topsize : topparent + 30;
    td.style.height = div.offsetHeight; 
    td.style.width = div.offsetWidth; 
    td.style.paddingTop = ((parseFloat(div.style.height) / 2) - ((23*6) / 2)) + 'px';
    div.style.display = '';
    var line_no = parseInt(document.getElementById('txt_search_point').value);
    reset_search(line_no); 
} 

function find_close_popup()
{
    document.getElementById('divPopFind').style.display = 'none';
}
    
function msg_target_selected(args)
{
    document.getElementById('txtmsgto').value = tgrid.getValueAt(args.datarow, 0);
    msg_grid.setColumn(4, document.getElementById('ddtargets').value);
    close_popusers();
}

function create_target_grid()
{
    var grid = new datagrid();
    {
        var col = new datagrid_column();
        col.header = 'ID';
        col.datafield = 'ID';
        col.align = 'left';
        col.styles.cell += 'width:25%;';
        grid.addColumn(col);	
    }	
    {
        var col = new datagrid_column();
        col.header = 'Description';
        col.datafield = 'Description';
        col.align = 'left';
        col.styles.cell += 'width:75%;';
        grid.addColumn(col);	
    }

    grid.classNames.table = 'table_body';
    grid.footer.find_button.none = false;
    grid.footer.find_button.onclick = 'show_popup_find';
    grid.row.normal_style = 'background-color: #f7ffff;'; 
    grid.row.onclick = 'msg_target_selected';
    return grid;         
}

function hasPopTargets(rs)
{
    if (rs.error != null)
    {
        ajax_show_status(false);
        alert(rs.error.Message);
        close_popusers();
        return;
    }
    
    tgrid = create_target_grid();
    tgrid.bind(rs.value[1], 'tdtargets', 'tgtgrid');
    document.getElementById(tgrid.id + '_footerbtn_Find').disabled = false;
    ajax_show_status(false);
}

function ddtargets_change(ddl)
{
    var tgtype = ddl.options[ddl.selectedIndex].text;
    if (tgtype == 'Select')
    {
        alert('Please select a msg target type');
        return;
    }
    
    if (tgtype.toUpperCase() == 'ALL' & issa)
    {
        if (confirm("Post message to all system users?"))
        {
            document.getElementById('txtmsgto').value = ddl.options[ddl.selectedIndex].text;
            msg_grid.setColumn(4, ddl.value);
            close_popusers();
            return;
        }
    }
    ajax_show_status(true);
    IRSWorld.BranchSetup.Messages.getTargets(ddl.value, hasPopTargets);
}

function show_pop_targets()
{
    var div = document.getElementById('divPopusers');
    var xy = calcpos(document.getElementById('btnmsgto'));
    div.style.left = xy.left - 335;
    div.style.top = xy.top + 20;
    div.style.position = 'absolute';
    div.style.display = '';
    
    var ddl = document.getElementById('ddtargets');
    if (ddl.options.length == 0)
    {
        var opts = new Array();
        for (var i = 0; i < msg_target_type_opts.length; i++)
        {
            var ss = msg_target_type_opts[i].split('~');
            var s = ss[1].toUpperCase();
            if (s == 'ALL' & !(issa | isba)) continue;
            opts[opts.length] = msg_target_type_opts[i];
        }
        set_ddl_options(ddl, opts);
    }
    ddl.value = 2;
    fire_event(ddl, 'onchange');
}

function close_popusers()
{
    var s = document.getElementById('txtmsgto').value;
    var ok = true;
    if (s == null || s.length == 0 || s == undefined)
    {
        ok = confirm("Close without msg target?");
    }
    if (ok)
    {
        if (tgrid != null) 
        {
            document.getElementById(tgrid.id + '_footerbtn_Find').disabled = true;
            tgrid = null;
        }
        var div = document.getElementById('divPopusers');
        div.style.display = 'none';
    }
}

function close_popup()
{
    var is_ok = true;
    var errmsg = '';
    
    if (document.getElementById('txtpostdate').wrong) 
    {
        errmsg = "Posting date is not valid.";
        is_ok = false;
    }
    
    if (document.getElementById('txtexpdate').wrong) 
    {
        errmsg = "Expire date is not valid.";
        is_ok = false;
    }            
    
    var msgtext = jspad_get_text();
    if (is_ok && msgtext.length > 0)
    {
        var sto = trim(document.getElementById('txtmsgto').value);
        if (sto != null && sto.length > 0)
        {
            msg_grid.setColumn(5, sto);
        }
        else
        {
            errmsg += "Recipient(s) not entered\n";
            is_ok = false;
        }
        
        if (is_ok)
        {
            var subj = trim(document.getElementById('txtmsgsubj').value);
            if (document.getElementById('txtmsgsubj').value.length > 0)
            {
                msg_grid.setColumn(6, document.getElementById('txtmsgsubj').value);
            }
            
            msg_grid.setColumn(7, document.getElementById('txtpostdate').value);
            msg_grid.setColumn(8, document.getElementById('txtexpdate').value);
            
            msgtext = msgtext.replace(/[|^~]/g, "");
            msgtext = msgtext.replace(/\"/g, '\\"');
            
            msg_grid.setColumn(9, msgtext); //"'" + msgtext + "'");
            if (userid == null || userid.length == 0)
            {
                msg_grid.setColumn(3, "Guest");
            }
            msg_grid.setColumn(10, userbr);
        }
    }
    
    if (is_ok && msgtext.length == 0)
    {
        errmsg = 'The message is empty.\n';
        is_ok = false;
    }
    
    if (!is_ok)
    {
        is_ok = confirm(errmsg + 'Close anyway?');
    }
    
    if (is_ok)
    {
        document.getElementById('divPopnew').style.display = 'none';
        document.getElementById('tblContent').style.visibility = 'visible';
        if (errmsg.length > 0)
        {
            msg_grid.footerButtonClick('', 'Cancel');
        }
        else
        {
            msg_grid.footerButtonClick('', 'Post');
        }
    }
}

function blur_msgto(txt)
{
    if (isSpecialEx(txt.value))
    {
        alert("Message target contains invalid character.\n" + txt.value);
        txt.focus();
    }
}

function is_valid_datetime(sdt)
{
    //.match(/^(0[1-9]|1[012])[\/.](?:(0[1-9]|[12][0-9]|3[01])[\/.](19|20)[0-9]{2}\s[0-1][0-9]:[0-5][0-9]:[0-5][0-9]\s(a|p|A|P)(m|M))$/))
    if (sdt.match(/^(0[1-9]|1[012])[\/.](?:(0[1-9]|[12][0-9]|3[01])[\/.][0-9]{4}\s[0-1][0-9]:[0-5][0-9]:[0-5][0-9]\s(a|p|A|P)(m|M))$/)) return true;
    else if (sdt.match(/^(0[1-9]|1[012])[\/.](?:(0[1-9]|[12][0-9]|3[01])[\/.][0-9]{4}\s[0-1][0-9]:[0-5][0-9]\s(a|p|A|P)(m|M))$/)) return true;

    return false;
}

function validate_postexdate(txt)
{
    var is_date = isValidDate(txt.value);
    var wrong = false;
    if (!is_date[0])
    {
        if (!is_valid_datetime(txt.value))
        {
            alert(is_date[1] + ', date must be in MM/dd/yyyy or MM/dd/yyyy HH:mm[:ss] AM|PM');
            wrong = true;
        }
    }
    try
    {
        var postday = Date.parse(txt.value);
        var tod = new Date();
        var today = Date.parse((tod.getMonth() + 1) + "/" + tod.getDate() + "/" + tod.getFullYear());
        /*
        if (postday < today)
        {
            alert("The date supplied is less than today.");
            wrong = true;
        }
        */
    }
    catch (e) 
    {
        alert( "The date supplied cannot be parsed.");
        wrong = true;
    }
    
    txt.wrong = (wrong) ? wrong : null;
    if (wrong) txt.focus();
}

function show_msg_pad()
{
    var div = document.getElementById('divPopnew');
    var xy = calcpos(document.getElementById('tblBody'));
    div.style.left = (xy.left + 10) + 'px';
    div.style.top = (xy.top) + 'px';
    div.style.position = 'absolute';
    div.style.display = '';
    document.getElementById('tblContent').style.visibility = 'hidden';
    
    var txta = document.getElementById('divmsgarea');
    var jsp = new jspad('msgpad', false);
    if (ispop) jspad_canedit = false;
    txta.innerHTML = jsp.toString();
}
        
function new_msg_clicked(args)
{
    show_msg_pad();

    document.getElementById('txtpostdate').value = get_current_date_string().split(' ')[0];
    document.getElementById('txtexpdate').value = document.getElementById('txtpostdate').value;
    
    document.getElementById('txtpostdate').wrong = false;
    document.getElementById('txtexpdate').wrong = false;
    
    msg_grid.setColumn(1, get_current_date_string());
    if (userid != null && userid.length > 0) msg_grid.setColumn(3, userid);
    msg_grid.setColumn(9, 2);  // msg target type
}

function edit_msg_clicked(args)
{
    show_msg_pad();
    
    var owner = msg_grid.getValueAt(args.gridrow, 3);
    if (userid != owner | owner != 'Guest') japad_canedit = false;
    
    document.getElementById('txtmsgto').value = msg_grid.getValueAt(args.gridrow, 5);
    document.getElementById('txtmsgsubj').value = msg_grid.getValueAt(args.gridrow, 6);
    document.getElementById('txtpostdate').value = msg_grid.getValueAt(args.gridrow, 7);
    document.getElementById('txtexpdate').value = msg_grid.getValueAt(args.gridrow, 8);
    
    var msgtxt = msg_grid.getValueAt(args.gridrow, 9);
    //msgtxt = msgtxt.replace(/'/g, "");
    
    jspad_set_text(msgtxt);
    
    if (ispop)
    {
        document.getElementById('txtmsgto').readOnly = true;
        document.getElementById('txtmsgsubj').readOnly = true;
        document.getElementById('txtpostdate').readOnly = true;
        document.getElementById('txtexpdate').readOnly = true;
        document.getElementById('btnmsgto').style.visibility = 'hidden';
        document.getElementById('calpostdate').style.visibility = 'hidden';
        document.getElementById('calexpdate').style.visibility = 'hidden';
    }
}

function updates_saved(rs)
{
    if (rs.error != null)
    {
        alert(rs.error.Message);
        return;
    }
    else {
        if (gbtnclick == "Save") {
            gbtnclick = "";
            alert("Record has been submitted for approval");
            msg_grid.clearChanges();
            do_search();
        }
        else {
            msg_grid.clearChanges();
            msg_grid.footer.save_button.always_enabled = rs.value[1];
            msg_grid.refresh(rs.value[0]);
        }
        
    }
    
    
}

function msg_post_clicked(args)
{
    var rex = /'/g;
    var adds = (args.additions.length > 0) ? args.additions.replace(rex, "") : args.additions;
    var eds = (args.edits.length > 0) ? args.edits.replace(rex, "") : args.edits;
    var dels = (args.deletes.length > 0) ? args.deletes.replace(rex, "") : args.deletes;
    gbtnclick = "Post";
    IRSWorld.BranchSetup.Messages.applyEdits(adds, dels, eds, updates_saved);        
}

function save_msg_clicked(args)
{
    var rex = /'/g;
    var adds = (args.additions.length > 0) ? args.additions.replace(rex, "") : args.additions;
    var eds = (args.edits.length > 0) ? args.edits.replace(rex, "") : args.edits;
    var dels = (args.deletes.length > 0) ? args.deletes.replace(rex, "") : args.deletes;
    gbtnclick = "Save";
    IRSWorld.BranchSetup.Messages.saveUpdates(adds, dels, eds, updates_saved);
}

function msg_row_clicked(args)
{
    var orig = msg_grid.getValueAt(args.datarow, 3);
    var origbr = msg_grid.getValueAt(args.datarow, 10);
    
    if (userid == null || userid.length == 0 || (userid != orig | userbr != origbr))
    {
        var btndel = msg_grid.getFooterButtonElement("Delete");
        if (btndel) btndel.disabled = true;
        
        var btnedit = msg_grid.getFooterButtonElement("Edit");
        if (btnedit) btnedit.disabled = true;
    }
}

var msg_type_opts;
var msg_target_type_opts;

function create_grid()
{
    msg_grid = new datagrid();
    
     { //0
        var col = new datagrid_column();
        col.header = 'ID';
        col.datafield = 'SEQ_ID';
        col.align = 'center';
        col.styles.cell += 'width:5%;display:none';
        col.styles.header += 'display:none';
        col.styles.datafield += 'display:none';
        msg_grid.addColumn(col);	
    } 
    { //1
	    var col = new datagrid_column();
	    col.header = 'Date Created';
	    col.datafield = 'MsgCreateDate';
        col.align = 'left';
        col.styles.cell += 'width:10%;text-align:center;';
	    msg_grid.addColumn(col);
    }		    
    { //2
        var col = new datagrid_column();
        col.header = 'Type';
        col.datafield = 'MsgType';
        col.align = 'left';
        col.type = 'select';
        col.options = msg_type_opts;
        col.styles.cell += 'width:10%;text-align:left;';
        col.styles.datafield += 'width: 100%; border: solid 1px navy;';
        msg_grid.addColumn(col);
    }		    
    { //3
	    var col = new datagrid_column();
	    col.header = 'Origin';
	    col.datafield = 'MsgUserId';
	    col.align = 'left';
	    col.styles.cell += 'width:10%';
	    msg_grid.addColumn(col);
    }
    { //4
        var col = new datagrid_column();
        col.header = 'Target Type';
        col.datafield = 'MsgTargetType';
        col.align = 'left';
        col.type = 'select';
        col.options = msg_target_type_opts;
        col.styles.cell += 'width:10%;text-align:left;';
        col.styles.datafield += 'width: 100%; border: solid 1px navy;';
        msg_grid.addColumn(col);
    }		              
    { //5
	    var col = new datagrid_column();
	    col.header = 'Target';
	    col.datafield = 'MsgTarget';
	    col.align = 'left';
	    col.styles.cell += 'width:15%';
	    msg_grid.addColumn(col);
    } 
    { //6
	    var col = new datagrid_column();
	    col.header = 'Subject';
	    col.datafield = 'MsgSubject';
	    col.align = 'left';
	    col.styles.cell += 'width:35%';
	    msg_grid.addColumn(col);
    }
    { //7
        var col = new datagrid_column();
        col.header = 'Posting Date';
        col.datafield = 'MsgPostingDate';
        col.align = 'left';
        col.styles.cell += 'width:10%;text-align:center;';
        msg_grid.addColumn(col);	
    } 
    { //8
        var col = new datagrid_column();
        col.header = 'Expire Date';
        col.datafield = 'MsgExpireDate';
        col.align = 'center';
        col.styles.cell += 'width:5%;display:none';
        col.styles.header += 'display:none';
        col.styles.datafield += 'display:none';
        msg_grid.addColumn(col);	
    } 
    { //9
        var col = new datagrid_column();
        col.header = 'Msg Text';
        col.datafield = 'MsgText';
        col.align = 'center';
        col.styles.cell += 'width:5%;display:none';
        col.styles.header += 'display:none';
        col.styles.datafield += 'display:none';
        msg_grid.addColumn(col);	
    }    
    { //10
        var col = new datagrid_column();
        col.header = 'Branch';
        col.datafield = 'MsgOriginBranch';
        col.align = 'left';
        col.styles.cell += 'width:5%;display:none';
        col.styles.header += 'display:none';
        col.styles.datafield += 'display:none';
        msg_grid.addColumn(col);	
    }   		       		     		                               

    msg_grid.caption = 'Messages';
    msg_grid.classNames.table = 'table_body';
    
    msg_grid.footer.edit_button.none = false;
    msg_grid.footer.edit_button.onclick = 'edit_msg_clicked';
    msg_grid.footer.edit_button.onpost = 'msg_post_clicked';
    
    if (!ispop)
    {
        msg_grid.footer.add_button.none = false;
        msg_grid.footer.add_button.value = "New";
        msg_grid.footer.add_button.style += 'background-color: lavender;'; 
        msg_grid.footer.add_button.onclick = 'new_msg_clicked';
        msg_grid.footer.add_button.onpost = 'msg_post_clicked';
        
        msg_grid.footer.delete_button.none = false;
        msg_grid.footer.delete_button.style += 'background-color: lavender;color:maroon;'; 
        msg_grid.footer.delete_button.onpost = 'msg_post_clicked';
                 
        msg_grid.footer.save_button.none = false;
        msg_grid.footer.save_button.style += 'background-color: lavender;'; 
        msg_grid.footer.save_button.onclick = 'save_msg_clicked'; 
        
        msg_grid.footer.cancel_button.none = false;
        msg_grid.footer.cancel_button.style += 'background-color: lavender;'; 
        
        msg_grid.row.onclick = 'msg_row_clicked';
    }
    else
    {
        msg_grid.footer.edit_button.value = 'View';
    }
    
    msg_grid.row.normal_style = 'background-color: #f7ffff;';  
    msg_grid.autoPost = true;
    msg_grid.postToServer = true;
}

function gotMessages(rs)
{
    if (rs.error != null)
    {
        alert(rs.error.Message);
        return;
    }
    
    enable_button(document.getElementById('btnSearch'), false);
    
    if (msg_grid == null || msg_grid.columns.length == 0)
    {
        create_grid();
    }
    
    msg_grid.footer.save_button.always_enabled = rs.value[1];
    msg_grid.bind(rs.value[0], 'tdmsgindex', 'tblmsgindex');
}

function do_search()
{
    var dttype = (document.getElementById('dddatetype').value == 1) ? 'Posting' : 'Create';
    var orig = null;
    try
    {
        orig = document.getElementById('ddorigin').value;
    }
    catch (e) {}
    if (orig == null || orig == "") orig = userid;
    IRSWorld.BranchSetup.Messages.getMessages(orig, dttype, document.getElementById('txtdtfrom').value, document.getElementById('txtdtto').value, gotMessages);
}
        
function hasOrigins(rs)
{
    if (rs.error != null) 
    {
        alert(rs.error.Message);
        return;
    }
    
    var ddl = document.getElementById('ddorigin');
    var val = (rs.value.length > 1) ? rs.value[1] : 'ALL';
    if (val != 'All') userid = val;
    userbr = rs.value[2];
    msg_type_opts = rs.value[3];
    msg_target_type_opts = rs.value[4];
    issa = rs.value[5];
    isba = rs.value[6];
   
    if (rs.value[0].Rows.length > 0)
    {
        for (var i = 0; i < rs.value[0].Rows.length; i++)
        {
            create_option(ddl, rs.value[0].Rows[i]["origin"], rs.value[0].Rows[i]["origin"]);
        }  
    } 
    
    ddl.value = val;          
}

function is_popup()
{
    ispop = (hasQueryStrings(window));
    
    if (ispop)
    {
        pop_function = getQueryString(window, 'function');
        if (pop_function == 'listview') 
        {
            document.getElementById('btnPopclose').style.display = 'block';
            return;
        }
        
        ispop = false;
        pop_function = '';
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
        document.getElementById('tdBody').style.verticalAlign = 'top';
        
        fixht();
    }        
}                        

function page_setup()
{
    if (top == self)
    {
        window.open('./BranchSetupMain.aspx?load=Messages.aspx', '_top');
    }
    else
    {
        var isauthorized = false;
        try
        {
            isauthorized = top.isAuthenticated();
        }
        catch (e) {}
        
        fixmain();
        
        is_popup();

        if (isauthorized | ispop)
        {
            document.getElementById('divBody').style.display = '';
            document.getElementById('txtdtfrom').value = get_current_date_string().split(' ')[0];
            document.getElementById('txtdtto').value = get_current_date_string().split(' ')[0];
            
            IRSWorld.BranchSetup.Messages.getOrigins( hasOrigins);
            
            do_search();
        }
        else
        {
            spawn_errpage("This module cannot be called directly!");
        }
    }
}

function close_session()
{
    if (is_closing())
    {
        IRSWorld.BranchSetup.Messages.close_session();
    }
    else
    {
        IRSWorld.BranchSetup.Messages.drop_msg();
    }
}    