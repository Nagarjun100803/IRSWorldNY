

var dtopts;

function init_ddl(ddl, dt, val, desc, ival)
{
    if (dt.Rows.length > 0)
    {
        create_option(ddl, '0', 'Select');
        
        for (var i = 0; i < dt.Rows.length; i++)
        {
            create_option(ddl, dt.Rows[i][val], dt.Rows[i][desc]);
        }  
        
        ddl.value = ival;          
    }
}

function modtab_acctype_change(val, nid, npid, ntitle)
{
    btn_enable('Save', true);
    btn_enable('Cancel', true);
}

function modtab_edit_click()
{
    btn_enable('Edit', false);
    btn_enable('Cancel', true);
    document.getElementById('ddroles').disabled = true;
    
    set_read_only(document.getElementById('modtab'), false);
}

function updatesApplied(rs)
{
    btn_enable('Edit', true);
    btn_enable('Save', false);
    btn_enable('Cancel', false);
    document.getElementById('ddroles').disabled = false;
    var msgalert = null;
   
    /* RFC-31620 - IRSW Maker-Checker Function for User Admin */
    if (IRSWorld.Admin.MenuProfiles.isAdminApproverTag().value == "1") {
        alert("RECORD SUBMITTED FOR APPROVAL");
    }
    else {
        alert("Succesfully saved.");
    }
    /* RFC-31620 - IRSW Maker-Checker Function for User Admin */
    hasDefaultProfile(rs);
}

function modtab_save_click()
{
    btn_enable('Save', false);
    
    var rec = "Role=" + document.getElementById('ddroles').value;
    
    var ddls = document.getElementById('modtab').getElementsByTagName('select');
    if (ddls.length > 0)
    {
        for (var i = 0; i < ddls.length; i++)
        {
            var ss = ddls.item(i).id.split('_');
            rec += '|';
            rec += ss[ss.length - 3] + '~'; // id
            rec += ss[ss.length - 1] + '~'; // gid
            rec += ss[ss.length - 2] + '~'; // pid
            rec += ddls.item(i).value + '~'; // actype
            var tid = '' + ddls.item(i).id;
            tid = tid.replace('_dd_', '_txt_');
            var txt = document.getElementById(tid);
            var s = (txt) ? txt.value : ''; //effdate
            rec += s;
        }
    }
    
    IRSWorld.Admin.MenuProfiles.applyUpdates(rec, updatesApplied);
}

function hasDefaultProfile(rs)
{
    if (rs.error != null)
    {
        alert(rs.errror.Message);
        return;
    }
    
    document.getElementById('divmods').removeChild(document.getElementById('modtab'));
    create_modtab(rs.value);
}

function modtab_cancel_click()
{
    btn_enable('Edit', true);
    btn_enable('Save', false);
    btn_enable('Cancel', false);
    
    document.getElementById('ddroles').disabled = false;
    
    var irole = parseInt(document.getElementById('ddroles').value);
    IRSWorld.Admin.MenuProfiles.getDefaultProfile(irole, hasDefaultProfile);
}     

function btn_enable(btn, enable)
{
    if (btn == 'All') 
    {
        var tbl = document.getElementById('tblbuttons');
        if (tbl)
        {
            var btns = tbl.getElementsByTagName('input');
            for (var i = 0; i < btns.length; i++)
            {
                enable_button(btns.item(i), enable);
            }
        }
    }
    else
    {
        var btnobj = document.getElementById('btn' + btn);
        if (btnobj)
        {
            enable_button(btnobj, enable);
        }
    }
} 

var dtopt = null;

function create_modtab(dt)
{
    var mtab = new treetab();
    mtab.create_treetab(dt, 'modtab', 'divmods', 'modtab_acctype_change', dtopt);
    
    btn_enable("Edit", true);
    btn_enable("Save", false);
    btn_enable("Cancel", false);        
}               
    
function hasSetupData(rs)
{
    if (rs.error != null)
    {
        alert(rs.error.Message);
        spawn_errpage(rs.error.Message);
        return;
    }
    
    dtopt = rs.value[0];
    init_ddl(document.getElementById('ddroles'), rs.value[1], 'FldValue', 'FldDescription', rs.value[3]);
    create_modtab(rs.value[2]);
}

function fixdim()
{
    if (top.document.getElementById('divCenter'))
    {
        var div = document.getElementById('divmain');
        if (div)
        {
            div.style.width = top.document.getElementById('divCenter').offsetWidth + 'px';
            div.style.height = (top.document.getElementById('divCenter').offsetHeight - 30) + 'px';
        }
    }
}

function page_setup()
{
    if (top != self)
    {
        fixdim();
        IRSWorld.Admin.MenuProfiles.getSetupData(hasSetupData);
        document.getElementById('divmain').style.display = 'block';
    }
    else
    {
        document.irs_loader.submit();
    }            
}

function close_session()
{
    if (is_closing())
    {
        IRSWorld.Admin.MenuProfiles.close_session();
    }
    else
    {
        IRSWorld.Admin.MenuProfiles.drop_default_profile();
    }
}

        