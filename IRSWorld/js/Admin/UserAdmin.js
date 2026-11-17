        var ajax_timeout = 60 * 1000;

        var user_rec;
        var is_setup;
        var xdays;
        var access_types;
        var flags_options;
        var flags_options_update;
        var issa;
        var isba;
        var email_op = '';
        
        function btn_enable(btn, enable)
        {
            var btnobj = document.getElementById('btn' + btn);
            if (btnobj)
            {
                enable_button(btnobj, enable);
            }
        }
                        
        function prompt_for_email(retval, msg)
        {
            var ss = retval.split('~');
            setText(document.getElementById('popUserId'), ss[0]); 
            setText(document.getElementById('popUserName'), ss[1]); 
            setText(document.getElementById('popCifId'), ss[2]);
            document.getElementById('btnClose').disabled = true;
            var emails = null;
            if (ss.length > 3 && ss[3].length > 0)
            {
                emails = ss[3].split(';');
            }
            if (emails != null && emails.length > 0)
            {
                document.getElementById('tblnoemail').style.display = 'none';
                document.getElementById('tblemail').style.display = 'block';
                document.getElementById('btnSend').disabled = false;
                //document.getElementById('btnView').disabled = false;
                emailopts = new Array();
                for (var i = 0; i < emails.length; i++)
                {
                    emailopts[i] = emails[i] + "~" + emails[i];
                }
                set_ddl_options(document.getElementById('popddemail'), emailopts);
                document.getElementById('popddemail').selectedIndex = 1;
                
                if (emailopts.length == 1)
                {
                    document.getElementById('btnSend').onclick();
                }
            }
            else
            {
                document.getElementById('popTxtEmail').value = '';
                document.getElementById('tblnoemail').style.display = 'block';
                document.getElementById('tblemail').style.display = 'none';
                document.getElementById('btnSend').disabled = false;
            }
        
            setText(document.getElementById('pop_heading'), msg); //document.getElementById('pop_heading').innerText = msg;
            
            var div = document.getElementById('divPopup');
            var td= document.getElementById('td_popup');
            div.style.height = top.document.getElementById('divCenter').offsetHeight + 'px';
            div.style.width = document.documentElement.clientWidth + 'px';
            div.style.display = 'block';
            if (isIE) div.className = 'div_undock';
            else div.setAttribute("class", "div_undock");
            td.style.height = div.offsetHeight + 'px';        
        }
        
        function pop_email_changed(tobj)
        {
            document.getElementById('btnSend').disabled = false;
            emailopts = new Array();
            emailopts[0] = tobj.value + "~" + tobj.value;
            set_ddl_options(document.getElementById('popddemail'), emailopts);
            document.getElementById('popddemail').selectedIndex = 1;
            document.getElementById('tblnoemail').style.display = 'none';
            document.getElementById('tblemail').style.display = 'block';
        }
        
        function email_sent(rs)
        {
            if (rs.error != null)
            {
                alert(rs.error.Message);
                document.getElementById('btnClose').disabled = false; 
                return;
            }
            
            alert("Password sent to: " + rs.value);
            close_popup();
        }
        
        function btn_view_click(btn)
        {
            if (document.getElementById('tblviewpwd').style.display != 'block')
            {
                var div = document.getElementById('divPoppwd');
                var td= document.getElementById('tdPoppwd');
                div.style.height = top.document.getElementById('divCenter').offsetHeight + 'px';
                div.style.width = document.documentElement.clientWidth + 'px';
                div.style.display = 'block';
                if (isIE) div.className = 'div_undock';
                else div.setAttribute("class", "div_undock");
                td.style.height = div.offsetHeight + 'px';        
            }       
        }
        
        function btn_send_click(btn)
        {
            document.getElementById('tblviewpwd').style.display = 'none';              

            var email = document.getElementById('popddemail').value;
            if (email == 'Select' || trim(email).length == 0)
            {
                document.getElementById('tblnoemail').style.display = 'block';
                document.getElementById('tblemail').style.display = 'none';  
                alert("No email address provided.");
                return;
            }
            else
            {
                document.getElementById('tblemail').style.display = 'block';
                document.getElementById('tblnoemail').style.display = 'none';
            }

            var userid = getText(document.getElementById('popUserId'));
            IRSWorld.Admin.UserAdmin.send_email(userid, email, email_op, email_sent);
        }
        
        function btn_pwdok_click()
        {
            var pwd = document.getElementById('pwdDiscloser').value;
            if (pwd == null || pwd.length == 0)
            {
                alert ('Please enter your password!');
                return;
            }
            
            var userid = getText(document.getElementById('popUserId'));
            var rs = IRSWorld.Admin.UserAdmin.disclose_password(userid, pwd);
            if (rs.error != null)
            {
                alert(rs.error.Message);
                return;
            }
            
            pwd_close_popup();
            
            if (rs.error != null)
            {
                alert(rs.error.Message);
                close_popup();
                return;
            }
            document.getElementById('tblnoemail').style.display = 'none';
            document.getElementById('tblemail').style.display = 'none';
            setText(document.getElementById('tdviewpwd'), rs.value);
            document.getElementById('tblviewpwd').style.display = 'block';
            document.getElementById('btnClose').disabled = false;
            document.getElementById('btnSend').disabled = false;
        }
        
        function close_popup()
        {
            document.getElementById('tblnoemail').style.display = 'none';
            document.getElementById('tblemail').style.display = 'none';
            document.getElementById('popTxtEmail').value = '';
            setText(document.getElementById('tdviewpwd'), ''); //document.getElementById('tdviewpwd').innerText = '';
            document.getElementById('tblviewpwd').style.display = 'none';
            document.getElementById('divPopup').style.display = 'none';
        }
        
        function pwd_close_popup()
        {
            document.getElementById('pwdDiscloser').value = '';
            document.getElementById('divPoppwd').style.display = 'none';
        }  
              
        function saved(rs)
        {
            if (rs.error != null)
            {
                error_cif('An error occured during save. ' + rs.error.Message);
                return;
            }
            
            /* RFC-31620 */
            var admFlag = true;
            if (IRSWorld.Admin.UserAdmin.isAdminApproverTag().value == "1") {
                alert("RECORD SUBMITTED FOR APPROVAL");
                admFlag = false;
            }
            else {
                alert("Record saved.");
            }
            
            //set_ddl_options(document.getElementById("ddFlags"), flags_options);
            set_read_only(document.getElementById('tblUserProfile'), true);

            /* RFC-31620 */
            if (admFlag) {
                btn_enable('Cancel', false);
                btn_enable('Save', false);
                btn_enable('Edit', true);
                btn_enable('Add', true);
                btn_enable('Delete', true);
            }
            else {
                btn_enable('Cancel', false);
                btn_enable('Save', false);
                btn_enable('Edit', false);
                btn_enable('Add', false);
                btn_enable('Delete', false);
            }

            if (rs.value[0] != null)
            {
			    db_grid.bind(rs.value[0], 'tdSearchGrid', 'tblSearchGrid');    		
            }
            else
            {
                alert("Error saving record.");
            }
            
            if (admFlag) {
                var retval = null;
                if (rs.value.length > 1 && rs.value[1] != null && rs.value[1].length > 0) {
                    retval = rs.value[1];
                    prompt_for_email(retval, 'Password Mailing');
                }
            }
        }
              
        function maint_save_click()
        {
            if (user_rec.error == null)
            {
                var errarr = '';
                if (!user_rec.setDdlValue('location', document.getElementById('ddLocation').value)) errarr += user_rec.error + '\n';
                if (!user_rec.setDdlValue('role', document.getElementById('ddRoles').value)) errarr += user_rec.error + '\n';
                if (!user_rec.setDdlValue('flag', document.getElementById('ddFlags').value)) errarr += user_rec.error + '\n';
                
                if (errarr.length > 0)
                {
                    alert('Save operation encountered the ff. errors:\n' + errarr);
                    return;
                }
                                
                /* Password dialog will not be required for new user
                *  instead the system should assign a default password and let
                *  or force the user to change it at first login.
                
                hide_combos(true);
                show_frame_dialog('Password.aspx?function=new&userid=' + user_rec.userid, 'password_close', '100%', '100%', 0);
                
                */
                
                var rec = user_rec.toString();
                if (rec != null)
                {
                    email_op = 'New';
                    IRSWorld.Admin.UserAdmin.save(rec, saved);
                }
                else
                {
                    error_cif('An error occured during save. ' + user_rec.error);
                }                
            }
        }
        
        function set_days_expired(ndays)
        {
            user_rec.setDaysExpired(ndays);
            if (user_rec.error != null)
            {
                alert(user_rec.error);
                return false;
            }
            setText(document.getElementById('spanDateExpired'), dateToString(user_rec.getExpirationDate())); //document.getElementById('spanDateExpired').innerText = dateToString(user_rec.getExpirationDate());
            return true;     
        }
        
        function focus_userid()
        {
            btn_enable('Save', false);
            set_read_only(document.getElementById('tblUserProfile'), true);
            document.getElementById('txtUserId').readOnly = false;
            document.getElementById('txtUserId').focus();
            document.getElementById('txtUserId').select();
            user_rec.userid = null;
        }
        
        function set_userid(rs)
        {
            if (rs.error != null)
            {
                var msg = 'UserAdmin@set_userid: ' + rs.error.Message;
                alert(msg);
                focus_userid();
                return;
            }
            
            var user_exist = rs.value;
            if (user_exist && user_rec.crud == 'add')
            {
                alert('Userid ' + user_rec.userid + ' exist!');
                focus_userid();
                return;
            } 
            
            set_read_only(document.getElementById('tblUserProfile'), (user_rec.crud != 'add')); 
        }        
        
        function txtfield_changed(tb)
        {
            var ena = true;
            
            if (tb.id == 'txtUserId')
            {
                ena = user_rec.setUserId(trim(tb.value));
                if (ena)
                {
                    IRSWorld.Admin.UserAdmin.user_exist('USERID', tb.value, set_userid);
                } 
                else
                {
                    if (user_rec.error != null)
                    {
                        alert(user_rec.error);
                    }
                }       
            }
            else if (tb.id == 'txtDaysExpired')
            {
                ena = set_days_expired(tb.value);
            }
            else if (tb.nodeName == 'SELECT' && tb.selectedIndex < 1)
            {
                alert('A value must be selected for item with word "Select"');
                ena = false;
            }
            else if (tb.id == 'ddRoles' | tb.id == 'ddLocation')
            {
                if (user_rec.crud != 'add' & user_rec.hasModProfile)
                {
                    if (confirm("Change in role or location shall remove any custom module profile attached to location or role?"))
                    {
                        IRSWorld.Admin.UserAdmin.removeModuleProfile(user_rec.userid, user_rec.location, user_rec.role, 
                            function(rs) 
                            {
                                if (rs.error != null)
                                {
                                    alert(rs.error.Message);
                                    return;
                                }
                            } 
                        );
                    }
                    else
                    {
                        if (tb.id == 'ddRole') tb.value = user_rec.role;
                        if (tb.id == 'ddLocation') tb.value = user_rec.location;
                    }
                }
            }
            else if (tb.id == 'ckLoggedOn')
            {
                user_rec.logon = tb.checked;
            }
            else if (tb.id == 'ddLogGroup')
            {
                user_rec.setLoginGroup(parseInt(tb.value));
            }
            
            if (! ena)
            {
                tb.focus();
                if (tb.type == 'text') tb.select();
            }
            if (ena & user_rec.crud != 'add')
            {
                user_rec.dateModified = new Date();
                setText(document.getElementById('spanDateModified'), dateToString(user_rec.dateModified)); //document.getElementById('spanDateModified').innerText = dateToString(user_rec.dateModified);
            }
            /* RFC-31620 */
            user_rec.error = null;
            btn_enable('Save', ena);
        }
        
        function focus_out(tb)
        {
            if (user_rec.error != null)
            {
                tb.focus();
                if (tb.type == 'text') tb.select();
            }
        }

        function maint_cancel_click()
        {
            if (user_rec.crud == 'edit')
            {
                var args = "" + user_rec.rowIndex + "," + user_rec.userid;
                link_userid_clicked(args);
            }
            else if (user_rec.crud == 'add')
            {
                if (is_setup)
                {
                    spawn_errpage("Operation Cancelled by user");
                }
                else
                {
                    show_search();
                }
            }
        }
        
        function error_cif(errmsg)
        {
            if (! confirm(errmsg + '\nTry Again?'))
            {
                //spawn_errpage(errmsg + ' Cancelled by user.');
                show_search();
            }
            else
            {
                if (user_rec.crud == 'add') maint_add_click();
            }
        }
        
        function init_maint_fields()
        {
            set_read_only(document.getElementById('tblUserProfile'), (user_rec.crud != 'add')); 
            
            if (user_rec.crud != 'add')
            {
                setText(document.getElementById('txtCifId'), user_rec.cifid); //document.getElementById('txtCifId').innerText = user_rec.cifid;

                var rs = IRSWorld.Main.IRSMain.getLoginData();
                
                if (rs.value[0] == user_rec.userid) {//DMP RFC-20327
                    btn_enable('Edit', false);
                    btn_enable('Add', false);
                    btn_enable('Delete', false);                    
                }
                else {
                    btn_enable('Edit', true);
                    btn_enable('Add', true);
                    btn_enable('Delete', true);
                }
                
                document.getElementById('txtUserId').value = user_rec.userid;
                setText(document.getElementById('lblFullname'), user_rec.fullname); //document.getElementById('lblFullname').innerText = user_rec.fullname;
                document.getElementById('ddRoles').value = user_rec.role;
                document.getElementById('ddFlags').value = user_rec.flag;
                setText(document.getElementById('spanDateModified'), dateToString(user_rec.dateModified)); //document.getElementById('spanDateModified').innerText = dateToString(user_rec.dateModified);
                document.getElementById('ddLogGroup').value = user_rec.login_group;
            }
            else
            {
                document.getElementById('txtUserId').value = '';
                document.getElementById('ddRoles').selectedIndex = 1;
                document.getElementById('ddFlags').selectedIndex = 1;
                document.getElementById('ddFlags').disabled = true; 
                document.getElementById('ddRoles').disabled = !(issa);
                document.getElementById('ddLocation').disabled = !(issa);
                document.getElementById('ddLogGroup').selectedIndex = user_rec.login_group;
                document.getElementById('ddLogGroup').disabled = true;
            }

            document.getElementById('ddLocation').value = user_rec.location;
            
            document.getElementById('txtDaysExpired').value = '' + user_rec.xdays;
            
            var dt = user_rec.getExpirationDate();
            
            setText(document.getElementById('spanDateCreated'), dateToString(user_rec.creationDate)); //document.getElementById('spanDateCreated').innerText = dateToString(user_rec.creationDate);
            setText(document.getElementById('spanDatePwdChange'), dateToString(user_rec.lastPwdChangeDate)); //document.getElementById('spanDatePwdChange').innerText = dateToString(user_rec.lastPwdChangeDate);
            setText(document.getElementById('spanDateExpired'), dateToString(user_rec.getExpirationDate())); //document.getElementById('spanDateExpired').innerText = dateToString(user_rec.getExpirationDate());
            document.getElementById('ckLoggedOn').checked = user_rec.logon;
            if (user_rec.lastAccessDate == null)
            {
                setText(document.getElementById('spanLastAccessDate'), ''); 
            }
            else
            {
                setText(document.getElementById('spanLastAccessDate'), dateToString(user_rec.lastAccessDate)); 
            }
        }
        
        function cif_detail_close()
        {
            hide_combos(false);
        }
                
        function cif_main_close()
        {
            hide_combos(false);
            var retval = self.returnValue;
            if (retval == null)
            {
                error_cif('CIF query cancelled: ' + retval);
                show_search_result();
            }
            else
            {
                if (user_rec.setCifId(retval[0]))
                {
                    clear_fields(document.getElementById('tblUserProfile'));
                
                    user_rec.fullname = retval[1];
                    user_rec.cif_class = retval[2];
                    user_rec.location = retval[3];
                    var rs =IRSWorld.Admin.UserAdmin.getLoginGroup(user_rec.location); 
                    if (rs.error != null)
                    {
                        alert(rs.error.Message);
                        user_rec.setOrigGroup(1);
                    }
                    else
                    {
                        user_rec.setOrigGroup(parseInt(rs.value));
                    }
                    user_rec.email = retval[4];
                    setText(document.getElementById('txtCifId'), '' + user_rec.cifid); 
                    setText(document.getElementById('lblFullname'), user_rec.fullname); 
                    init_maint_fields();
                    return;
                }
                error_cif(user_rec.error);             
            }
        }
        
        function hide_combos(hiden)
        {
            var vis = (hiden) ? 'hidden' : 'visible';
            document.getElementById('ddLocation').style.visibility = vis;
            document.getElementById('ddRoles').style.visibility = vis;
            document.getElementById('ddFlags').style.visibility = vis;
        } 
               
        function maint_add_click()
        {
            user_rec.setup(is_setup, xdays);
            user_rec.crud = 'add';
            
            set_ddl_options(document.getElementById("ddFlags"), flags_options_update);
                        
            btn_enable('Edit', false);
            btn_enable('Add', false);     
            btn_enable('Delete', false);  
            btn_enable('Save', false);  
            btn_enable('Cancel', true); 
            
            hide_combos(true);
 
            show_frame_dialog('../CIF/cifmain.aspx?function=adduser', 'cif_main_close', '100%', '100%', '0');
        }
        
        function maint_edit_click()
        {
            user_rec.crud = 'edit';
            
            set_ddl_options(document.getElementById("ddFlags"), flags_options_update);
            
            set_read_only(document.getElementById('tblUserProfile'), false); 
            document.getElementById('txtUserId').readOnly = 'true';
            document.getElementById('ddLocation').disabled = !(issa);
            document.getElementById('ddRoles').disabled = !(issa);
            btn_enable('Delete', false);
            btn_enable('Edit', false);
            btn_enable('Add', false);
            btn_enable('Cancel', true);
        }
        
        function user_deleted(rs)
        {
            ajax_show_status(false);
            if (rs.error != null)
            {
                alert("Delete: " + rs.error.Message);
                btn_enable('Edit', true);
                btn_enable('Add', true);
                btn_enable('Delete', true);
                btn_enable('Cancel', false);
                return;
            }
            /* RFC-31620 */
            if (IRSWorld.Admin.UserAdmin.isAdminApproverTag().value == "1") {
                alert("RECORD SUBMITTED FOR APPROVAL");
            }
            else {
                alert("User succesfully deleted.");
            }

            if (db_grid != null) db_grid.refresh(rs.value[0]);

            set_read_only(document.getElementById('tblUserProfile'), true);
            btn_enable('Edit', false);
            btn_enable('Add', true);
            btn_enable('Delete', false);
            btn_enable('Cancel', false);
        }
        
        function maint_del_click()
        {
            if (confirm("Delete " + user_rec.userid + "?"))
            {
                btn_enable('Edit', false);
                btn_enable('Add', false);
                btn_enable('Delete', false);
                btn_enable('Cancel', false);
                ajax_show_status(true);
                user_rec.crud = "delete";
                IRSWorld.Admin.UserAdmin.save(user_rec.toString(), user_deleted);
            }
        }
        
        var db_grid = null;
        
        function show_row_clicked(idx, row)
        {
            user_rec = new user_record();
            user_rec.parseRow(idx, row);
            IRSWorld.Admin.UserAdmin.hasModuleProfile(user_rec.userid, user_rec.location, user_rec.role, 
                function (rs)
                {
                    if (rs.error != null) return;
                    
                    user_rec.hasModProfile = rs.value;
                }
            );
                        
            set_ddl_options(document.getElementById("ddFlags"), flags_options);

            show_user_maint();
            init_maint_fields();
            
            //btn_enable('Edit', true);//DMP RFC-20327
            //btn_enable('Delete', true);//DMP RFC-20327
            //btn_enable('Add', true);//DMP RFC-20327
            btn_enable('Save', false);
            btn_enable('Cancel', false);        
        }
        
        function link_userid_clicked(args)
        {
            var plist = args.split(',');
            var idx = parseInt(trim(plist[0]));
            
            show_row_clicked(idx, db_grid.datatable.Rows[idx]);
        }
        
        function result_row_clicked(args)
        {
            show_row_clicked(args.datarow, db_grid.datatable.Rows[args.datarow]);
        }
        
        function btn_find_click()
        {
            var val = document.getElementById('txtfind').value;
            if (val.length == 0) 
            {
                alert('Find needs search criteria.');
                return;
            }
            var find_args = new datagrid_find_arguments();
            var farg = new datagrid_find_argument();
            farg.column_name = 'UserID';
            farg.operator = "search";
            farg.value = val;
            find_args.addArgument(farg);
            find_args.addConjunction('|');
            farg = new datagrid_find_argument();
            farg.column_name = 'FullName';
            farg.operator = "search";
            farg.value = val;
            find_args.addArgument(farg);
            find_args.select_row = false;
            var locrow = db_grid.find(find_args, true);
            
            if (locrow == -1)
            {
                alert("No row found.");
            }
            
            find_close_popup();
        }
        
        function show_popup_find(args)
        {
            var div = document.getElementById('divPopFind');
            var td = document.getElementById('tdPopFind');
            var topsize = top.document.getElementById('divCenter').offsetHeight;
            var topparent = document.getElementById('divContent').offsetHeight;
            div.style.height = ((topsize > topparent) ? topsize : topparent + 30) + 'px';
            if (isIE) div.className = 'div_undock';
            else div.setAttribute("class", "div_undock");
            td.style.height = div.offsetHeight + 'px'; 
            td.style.width = div.offsetWidth + 'px'; 
            td.style.paddingTop = ( (parseFloat(div.style.height) / 2) - (parseFloat(document.getElementById('tblPopFind').offsetHeight) / 2) ) - 30 + 'px';         
            div.style.display = 'block'; 
        }
        
        function find_close_popup()
        {
            document.getElementById('divPopFind').style.display = 'none';
        }
        
    	function create_result_grid()
    	{
    		db_grid = new datagrid();

    		var col1 = new datagrid_column();
    		col1.header = 'CIF ID';
    		col1.datafield = 'CIFID';
    		col1.align = 'left';
    		col1.styles.cell += 'width:10%';
    		db_grid.addColumn(col1);

    		var col2 = new datagrid_column();
    		col2.header = 'User ID';
    		col2.datafield = 'UserID';
    		col2.styles.cell += 'width:10%';
    		db_grid.addColumn(col2);

    		var col3 = new datagrid_column();
    		col3.header = 'User Name';
    		col3.datafield = 'FullName';
    		col3.styles.cell += 'width:20%';
    		db_grid.addColumn(col3);

    		var col4 = new datagrid_column();
    		col4.header = 'Location';
    		col4.datafield = 'BranchName';
    		col4.styles.cell += 'width:20%';
    		db_grid.addColumn(col4);
    		
    		var col5 = new datagrid_column();
    		col5.header = 'Role';
    		col5.datafield = 'RoleDesc';
    		col5.styles.cell += 'width:20%';
    		db_grid.addColumn(col5);

    		var col6 = new datagrid_column();
    		col6.header = 'Status';
    		col6.datafield = 'FlagDesc';
    		col6.styles.cell += 'width:10%';
    		db_grid.addColumn(col6);
    		
    		db_grid.row.onclick = 'result_row_clicked';
    		db_grid.footer.find_button.none = false;
    		db_grid.footer.find_button.onclick = 'show_popup_find';

    	}
    	  
        function searched(rs)
        {
            ajax_show_status(false);
            if (rs.error != null) 
            {
                alert('UserAdmin@search: ' + rs.error.Message);
                return;
            }
            
            if (rs.value != null)
            {
                create_result_grid();
			    db_grid.bind(rs.value, 'tdSearchGrid', 'tblSearchGrid');    		
            }            
            
            document.onkeyup = null;
            show_search_result();
        }
        
        function search_clicked()
        {
            var args = document.getElementById('txt_search').value;
            var cat = parseInt(document.getElementById('ddl_search_cat').value);
            var loc = document.getElementById('ddl_search_location').value;
            var role = parseInt(document.getElementById('ddl_search_roles').value);
            var flag = parseInt(document.getElementById('ddl_search_flags').value);
            var logged = parseInt(document.getElementById('ddl_search_logon').value);
            if (args.length == 0 & loc == "0" & role == 0 & flag == 0)
            {
                if (!confirm('No criteria selected/entered, list all users?')) return; 
            }
            else
            {
                if (parseInt(cat) > 2)
                {
                    var rd = isValidDate(args);
                    if (rd[0] == false)
                    {
                        alert(rd[1]);
                        return;
                    }
                }
            }
            
            ajax_show_status(true);
            setText(document.getElementById('span_srch_txt'), cat + ', ' + args + ', ' + loc + ', ' + flag + ', ' + logged); //document.getElementById('span_srch_txt').innerText = cat + ', ' + args + ', ' + loc + ', ' + flag + ', ' + logged;
            IRSWorld.Admin.UserAdmin.search(cat, args, loc, role, flag, logged, searched);
        }
        
        function show_search_calendar()
        {
            if (document.getElementById('ddl_search_cat').selectedIndex > 1)
            {
                document.getElementById('img_search_calendar').style.visibility = 'visible';
                return;
            }
            document.getElementById('img_search_calendar').style.visibility = 'hidden';
        }        
    
        function show_user_maint()
        {
            var lnk = js_sidebar_element('sidemenuB', 3, 0);
            var cls = (getText(lnk) == 'Modules Profile'); //(lnk.innerText == 'Modules Profile');
            document.getElementById('divSearch').style.display="none";
            document.getElementById('divMaint').style.display="block";
            document.getElementById('divUserProfile').style.display="block";
            document.getElementById('divModules').style.display="none";
            document.getElementById('divResult').style.display="none";
            
            if (issa)
            {
                setText(lnk, "Modules Profile");
                lnk.href = "javascript:show_maint_modules();";
            }
            else
            {
                lnk.parentElement.style.display = 'none';
            }
            
            if (cls)
            {
                clear_fields(document.getElementById('tblUserProfile'));
                set_read_only(document.getElementById('tblUserProfile'), true);
            }  
        }

        function parse_qs_args(qs)
        {
            var ss = qs.split(';');
            var ok_count = 0;
            for (var i = 0; i < ss.length; i++)
            {
                var tt = ss[i].split(':');
                var re = /%20/g;
                tt[0] = tt[0].replace(re, " "); 
                
                if (setSelectedItem(document.getElementById('ddl_search_cat'), tt[0]))
                {
                    document.getElementById('txt_search').value = tt[1];
                    ok_count++;
                    continue;
                }
                
                if (tt[0] == 'Location')
                {
                    document.getElementById('ddl_search_location').value = tt[1];
                    ok_count++;
                    continue;
                }
                
                if (tt[0] == 'Role')
                {
                    if (setSelectedItem(document.getElementById('ddl_search_roles'), tt[1]))
                    {
                        ok_count++;
                        continue;
                    }
                }
            } 
            
            if (ok_count < ss.length)
            {
                alert('Some of the query argument did not qualify');
            }       
        
            return (ok_count > 0);
        }
        
        function show_search()
        {
            document.getElementById('divSearch').style.display="block";
            document.getElementById('divMaint').style.display="none";
            document.getElementById('divUserProfile').style.display="none";
            document.getElementById('divModules').style.display="none";
            document.getElementById('divResult').style.display="none";
            document.getElementById('tdvert').style.verticalAlign = "middle";
            document.onkeyup = search_enter_click;
            
            if (!issa)
            {
                document.getElementById('ddl_search_location').selectedIndex = 1;
                document.getElementById('ddl_search_location').disabled = true;
                document.getElementById('ddl_search_roles').selectedIndex = 1;
                document.getElementById('ddl_search_roles').disabled = true;
            }
            else
            {
                document.getElementById('ddl_search_location').selectedIndex = 0;
                document.getElementById('ddl_search_location').disabled = false;
                document.getElementById('ddl_search_roles').selectedIndex = 0;
                document.getElementById('ddl_search_roles').disabled = false;
            }
            document.getElementById('txt_search').focus();

            var qs = getQueryString(window, "search");
            if (qs != null && qs.length > 0)
            {
                var ok = parse_qs_args(qs);
                if (ok)
                {
                    document.getElementById('btn_search').onclick();
                }
            }
        }
        
        function search_enter_click(e)
        {
            if (!e) var e = window.event;
            
            if (e.keyCode == 13) // Enter key
            {
                document.getElementById('btn_search').onclick();
            }
        }
        
        function show_search_result()
        {
            document.getElementById('divSearch').style.display="none";
            document.getElementById('divMaint').style.display="none";
            document.getElementById('divUserProfile').style.display="none";
            document.getElementById('divModules').style.display="none";
            document.getElementById('divResult').style.display="block";
            document.getElementById('tdvert').style.verticalAlign="top";
            
            var div = document.getElementById('tdSearchGrid');
            var side = document.getElementById('sideserch');
            div.style.width = (parseInt(top.document.getElementById('divCenter').offsetWidth) - (parseInt(side.offsetWidth) + 50)) + 'px';
            div.style.height = (parseInt(document.getElementById('tblSearchGrid').offsetHeight) + 15) + 'px';
        } 
        
        function modtab_acctype_change(val, nid, npid, ntitle)
        {
            btn_enable('ModSave', true);
            btn_enable('ModCancel', true);
        }
        
        function maint_mod_disall(allow)
        {
            var tbl = document.getElementById('tblModProfile');
            if (tbl)
            {
                var ddls = tbl.getElementsByTagName('select');
                var cnt = 0;
                for (var i = 0; i < ddls.length; i++)
                {
                    var val = (allow) ? 0 : 1;
                    ddls.item(i).selectedIndex = val;
                    cnt++;
                }
                
                if (cnt > 0)
                {
                    modtab_acctype_change();
                }
            }
        }
        
        function create_modtab(dt)
        {
            try
            {
                document.getElementById('divModProfile').removeChild(document.getElementById('tblModProfile'));
            }
            catch (e) {}
            var modtab = new treetab();
            modtab.create_treetab(dt, 'tblModProfile', 'divModProfile', 'modtab_acctype_change', access_types);
            
            btn_enable('ModEdit', true);
            btn_enable('ModSave', false);
            btn_enable('ModCancel', false); 
            document.getElementById('aModDisAll').removeAttribute('href');
            document.getElementById('aModAllowAll').removeAttribute('href');
        }
        
        function mod_edit_click()
        {
            set_read_only(document.getElementById('tblModProfile'), false);
            btn_enable('ModEdit', false);
            btn_enable('ModCancel', true);
            
            document.getElementById('aModDisAll').setAttribute('href' ,'javascript:maint_mod_disall(true);');
            document.getElementById('aModAllowAll').setAttribute('href', 'javascript:maint_mod_disall(false)');
        }
        
        function mod_cancel_click()
        {
            show_maint_modules();
        }
        
        function mod_saved(rs)
        {
            ajax_show_status(false);
            if (rs.error != null)
            {
                alert("UserAdmin@mod_saved: " + rs.error.Message);
                btn_enable('ModCancel', true);
                btn_enable('ModSave', true);
                return;
            }
            
            alert("Updates saved.");
            user_rec.hasModProfile = true;
            mod_cancel_click();
        }
        
        function mod_save_click()
        {
            btn_enable('ModEdit', false);
            var s = 'USERID=' + getText(document.getElementById('spanModUserid')) + '~'; //.innerText
            s += 'BRANCHCODE=' + document.getElementById('ddLocation').value + '~';
            s += 'ROLE=' + document.getElementById('ddRoles').value + '~';
            var ddls = document.getElementById('tblModProfile').getElementsByTagName('select');
            for (var i = 0; i < ddls.length; i++)
            {
                var ddl_id = ddls.item(i).id.split('_');
                var txt_id = ddls.item(i).id.replace('_dd_', '_txt_');
                var txt = document.getElementById(txt_id);
                var txt_val = (txt == null) ? '' : '' + txt.value; 
                s += ddl_id[ddl_id.length - 3] + '=' + ddls.item(i).value + '^' + txt_val + '~';
            }
            ajax_show_status(true);
            set_read_only(document.getElementById('tblModProfile'), true);
            btn_enable('ModSave', false);
            btn_enable('ModCancel', false);
            IRSWorld.Admin.UserAdmin.savePermissions(s, mod_saved);       
        }
        
        function got_module_permission(rs)
        {
            ajax_show_status(false);
            if (rs.error != null) 
            {
                alert("GET Module Permissions: " + rs.error.Message);
                return;
            }
            
            create_modtab(rs.value);
                       
            document.getElementById('divSearch').style.display="none";
            document.getElementById('divMaint').style.display="block";
            document.getElementById('divUserProfile').style.display="none";
            document.getElementById('divModules').style.display="block";
            document.getElementById('divResult').style.display="none";
            
            var lnk = js_sidebar_element('sidemenuB', 3, 0);
            setText(lnk, "User Profile");
            lnk.href = "javascript:show_user_maint();";
            setText(document.getElementById('spanModUserid'), document.getElementById('txtUserId').value);
            setText(document.getElementById('spanModFullName'), getText(document.getElementById('lblFullname')));
            setText(document.getElementById('spanModRole'), document.getElementById('ddRoles').options[document.getElementById('ddRoles').selectedIndex].text); 
            try
            {       
                setText(document.getElementById('spanModLoc'), document.getElementById('ddLocation').options[document.getElementById('ddLocation').selectedIndex].text);    
            }
            catch (e) {}                
        }
        
        function show_maint_modules()
        {
            /* RFC-31620 */
            /*
            var uid = document.getElementById('txtUserId').value;
            var loc = document.getElementById('ddLocation').value;
            var rol = document.getElementById('ddRoles').value;
            ajax_show_status(true);
            IRSWorld.Admin.UserAdmin.getModulePermissions(uid, loc, rol, got_module_permission);
            */
            alert('Function has been disabled. Please use UAM')
        } 
        
        function password_reset(rs)
        {
            ajax_show_status(false);
            if (rs.error != null)
            {
                spawn_errpage(rs.error.Message);
                return;
            }
            
            if (rs.value[0])
            {
                email_op = 'Reset';
                var retval = rs.value[1];
                prompt_for_email(retval, 'Password reset successfully!');
            }
        }
        
        function reset_password()
        {
            var userid = document.getElementById('txtUserId').value;
            if (confirm('Reset Password of user: ' + userid))
            {
                ajax_show_status(true);
                IRSWorld.Admin.UserAdmin.reset_password(userid, password_reset);
            }
        }      
        
        function set_ddlroles_option(opt_roles)
        {
            if (opt_roles == null)
            {
                var msg = "UserAdmin@set_ddlroles_option: server did not return values";
                spawn_errpage(msg);
            }
            
            set_ddl_options(document.getElementById("ddRoles"), opt_roles);
            set_ddl_options(document.getElementById("ddl_search_roles"), opt_roles);
        }
        
        function set_ddlflags_option(opt_flags)
        {
            if (opt_flags == null)
            {
                var msg = "UserAdmin@set_ddlflags_option: server did not return values";
                spawn_errpage(msg);
            }
            
            set_ddl_options(document.getElementById("ddFlags"), opt_flags);
            set_ddl_options(document.getElementById("ddl_search_flags"), opt_flags);
        }
        
        function set_ddllgroup_option(opt_lgroup)
        {
            if (opt_lgroup == null)
            {
                var msg = "UserAdmin@set_ddllgroup_option: server did not return values";
                spawn_errpage(msg);
            }
            
            set_ddl_options(document.getElementById("ddLogGroup"), opt_lgroup);
        }        
        
        function set_ddlbranch_option(opt_br)
        {
            if (opt_br == null)
            {
                var msg = "UserAdmin@set_ddlbranch_option: server did not return values";
                spawn_errpage(msg);
            }
            
            set_ddl_options(document.getElementById("ddLocation"), opt_br);
            set_ddl_options(document.getElementById("ddl_search_location"), opt_br);
        }
        
        function remove_expire_option(aopts)
        {
            var sso = new Array();
            for (var i = 0; i < aopts.length; i++)
            {
                if (aopts[i].indexOf('Expired') > -1) continue;
                sso.push(aopts[i]);
            }
            return sso;
        }   
        
        function set_init_data(rs)
        {
            if (rs.error != null)
            {
                var msg = 'UserAdmin@getInitData: Failed ' + rs.error.Message;
                spawn_errpage(msg);
                return;
            }
            
            set_ddlroles_option(rs.value[0]);
            set_ddlbranch_option(rs.value[2]);
            
            access_types = rs.value[3];
            
            flags_options = rs.value[1];
            flags_options_update = remove_expire_option(flags_options);
            set_ddlflags_option(flags_options);
            
            issa = rs.value[4];
            isba = rs.value[5];
            
            set_ddllgroup_option(rs.value[6]);
            
            show_search();
        }
        
        function upset(rs)
        {
            if (rs.error != null)
            {
                var msg = "UserAdmin@detect_setup: " + rs.error.Message;
                spawn_errpage(msg);
                return;            
            }
            
            xdays = rs.value[1];
            is_setup = rs.value[0];
            
            IRSWorld.Admin.UserAdmin.getDdlData(set_init_data);
            
            if (is_setup)
            {
                show_user_maint();
                document.getElementById('btnAdd').onclick();
            }
        }
        
        function fixdim()
        {
            if (top.document.getElementById('divCenter'))
            {
                var div = document.getElementById('divContent');
                
                div.style.height = (top.document.getElementById('divCenter').offsetHeight + 10) + 'px';
                div.style.width = (top.document.getElementById('divCenter').offsetWidth + 5) + 'px';
            }        
        }
                
        function page_setup()
        {
            fixdim();
            if (top != self)
            {
                AjaxPro.timeoutPeriod = ajax_timeout;
                var ay = "0";
                user_rec = new user_record();
                IRSWorld.Admin.UserAdmin.setup(upset);
            }
            else
            {
                var act = "../Main/IRSMain.aspx?load=../Admin/UserAdmin.aspx";
                if (window.location.search.length > 0)
                {
                    act += window.location.search; 
                }
                document.forms[2].action = act;
                document.forms[2].submit();
            }
        }
        
        function close_session()
        {
            if (is_closing())
            {
                IRSWorld.Admin.UserAdmin.close_session();
            }
            else
            {
                IRSWorld.Admin.UserAdmin.drop_irsusers();
            }
        }

        function GenerateUsersReport()//DMP RFC-20327
        {
            this.generate = function () {
                var Location = document.getElementById("ddl_search_location").options[document.getElementById("ddl_search_location").selectedIndex].text
                var UserRole = document.getElementById("ddl_search_roles").options[document.getElementById("ddl_search_roles").selectedIndex].text
                var Flag = document.getElementById("ddl_search_flags").options[document.getElementById("ddl_search_flags").selectedIndex].text

                var result = IRSWorld.Admin.UserAdmin.GenerateUsersReport(Location, UserRole, Flag).value;//.toString();//, this.GenerateUsersReport_callback);
                this.GenerateUsersReport_callback(result);//alert(result);
            }

            this.GenerateUsersReport_callback = function (result) {
                document.getElementById("divReportsDownload").style.display = "block";

                var tbgrid = document.getElementById("tblReportsDownload");
                var tbExist = document.getElementById('tbBody');
                var tbbody = document.createElement('tbody');

                if (result != "") {
                    var resultSplitted = result.split("~");

                    var row = document.createElement('tr');
                    var tdReportFileName = document.createElement('td');
                    var tdReportFilePath = document.createElement('td');
                    var tdDownloadButton = document.createElement('td');

                    var lblFileName = document.createElement('label');
                    lblFileName.innerHTML = resultSplitted[0];
                    lblFileName.setAttribute('align', 'left');
                    tdReportFileName.appendChild(lblFileName);

                    var hrefDownload = document.createElement('a');
                    hrefDownload.innerHTML = 'Download';
                    hrefDownload.setAttribute('align', 'center');
                    hrefDownload.setAttribute('href', "DownloadReports.aspx?download_path=" + resultSplitted[1]);
                    tdDownloadButton.appendChild(hrefDownload);

                    row.appendChild(tdReportFileName);
                    row.appendChild(tdReportFilePath);
                    row.appendChild(tdDownloadButton);

                    tbbody.appendChild(row);
                }
                else {
                    var row = document.createElement('tr');
                    var tdReportFileName = document.createElement('td');
                    var tdReportFilePath = document.createElement('td');
                    var tdDownloadButton = document.createElement('td');

                    var lblFileName = document.createElement('label');
                    lblFileName.innerHTML = "NO RECORDS FOUND";
                    lblFileName.setAttribute('align', 'left');
                    tdReportFileName.appendChild(lblFileName);

                    row.appendChild(tdReportFileName);
                    row.appendChild(tdReportFilePath);
                    row.appendChild(tdDownloadButton);

                    tbbody.appendChild(row);
                }


                if (tbExist != null) {

                    tbgrid.replaceChild(tbbody, tbExist);
                }
                else {

                    tbgrid.appendChild(tbbody);
                }
                tbbody.setAttribute('id', 'tbBody');
            }
        }