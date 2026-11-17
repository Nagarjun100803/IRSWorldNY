    
        var rv;
    
        function retval()
        {
            this.success = false;
            this.cancelled = false;
            this.userid = '';
            this.location = '';
            this.role = 0;
            this.flag = 0;
            this.tries = 0;
            this.chargeid = 0;
            this.message = '';
        }
    
        function dom(tid)
        {
            return document.getElementById(tid);
        }
        
        function pass_close()
        {
            window.returnValue = rv;
            close_frame_dialog();        
        }
        
        function pwd_cancel()
        {
            rv.cancelled = true;
            rv.message = "Override authentication cancelled.";
            pass_close(rv);
        }
        
        function override_checked(rs)
        {
            if (rs.error != null)
            {
                rv.message = rs.error.Message;
                pass_close(rv);
                return;
            }
            
            rv.location = rs.value[1];
            var autar = rs.value[0];
            
            rv.success = autar[0];
            rv.message = autar[1];
            rv.userid =  autar[2];
            rv.role = rs.value[2];
            rv.flag = autar[4];
            rv.tries = autar[3];
            pass_close();
        }
        
        function pwd_submit()
        {
            if (dom('txtUserId').value.length == 0)
            {
                alert('Please enter override userid.');
                dom('txtUserId').focus();
                return;
            }
            if (dom('txtPasswd').value.length == 0)
            {
                alert('Password is required.');
                dom('txtUserId').focus();
                return;
            }
            
            IRSWorld.Admin.Override.verify_password(dom('txtUserId').value, dom('txtPasswd').value, rv.chargeid, rv.tries, override_checked);
        }
        
    
        function page_setup()
        {
            if (top == self)
            {
                spawn_errpage("Override dialog is not to be run standalone.");
                return;
            }
            
            if (window.frameElement == undefined)
            {
                spawn_errpage("Override dialog requires defined activation method.");
                return;
            }
            
            rv = new retval();
            
            var cid = getQueryString(window, "ChargeId");
            rv.chargeid = parseInt(cid);
            if (isNaN(rv.chargeid) && rv.chargeid > 0)
            {
                rv.success = false;
                rv.message = "ChargeId is a required parameter.";
                pass_close();
                return;
            }
            
            dom('divmain').style.display = 'block';
            dom('txtUserId').focus();
        }
    
        function pwd_click()
        {
            if (window.event.srcElement.tagName == "INPUT")
            {
                if (window.event.keyCode == 13) // Enter key
                {
                    dom('btnPasswdOk').click();
                }
                else if (window.event.keyCode == 27) // Escape Key
                {
                    window.event.returnValue = false;
                    dom('btnPasswdCancel').click();
                }
            }
        }           
