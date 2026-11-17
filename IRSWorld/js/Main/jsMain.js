        var ajax_timeout = (60 * 3) * 1000;

        var isLogin;
        var authenticated;
        var ldate;
        var furl = '';
        
        function frame_scroll(scroll)
        {
            var ctr = document.getElementById('frmCenter');
            if (ctr)
            {
                ctr.scrolling = scroll;
            }
        }
        
        function get_load_url()
        {
            furl = getQueryString(top, 'load');
            if (furl != undefined)
            {
                var k = furl.indexOf("?");
                if (k > -1)
                {
                    furl = furl.substr(0, k);
                }
                           
                var qs = top.location.search.substring(1).split('?');
                if (qs.length > 0)
                {
                    furl +="?";
                    for (var i = 0; i < qs.length; i++)
                    {
                        if (qs[i].indexOf('load=') > -1) continue;
                        furl += qs[i];
                    }
                }                
                isLogin = (furl.indexOf('Login.aspx') > -1);        
            }
        }
    
        function do_loader(dtmenu)
        {
            if (furl != undefined)
            {
                var isok = false;
                for (var i = 0; i < dtmenu.Rows.length; i++)
                {
                    var murl = dtmenu.Rows[i].ModuleUrl;
                    var surl = '';
                    var hasq = furl.indexOf('?');
                    surl = (hasq > -1) ? furl.substr(0, hasq) : furl;
                    if (murl != null && (murl.indexOf(surl) > -1 | surl.indexOf(murl) > -1))
                    {
                        isok = true;
                        break;
                    }
                }
                if (isok) document.getElementById("frmCenter").src = furl;
                else
                {
                    //spawn_errpage("Non-existent module OR Permission required: " + furl);
                    spawn_errpage("You are not authorized to view this page. Access denied!");
                }
            }
        }
        
        function gotMenuData(rs)
        {
            if (rs.error != null)
            {
                alert(rs.error.Message);
                return;
            }
            
            if (rs.value.Rows.length > 0)
            {
                document.getElementById('tdNavbar').style.display = 'block';
                new jsmenu(rs.value, 'tblMenuBar', 'tdNavbar');
                if (hasQueryStrings(top))
                {
                    do_loader(rs.value);
                } 
                               
                return;
            }
            
            alert ('No menu elements');
        }
        
        function do_login(qstr)
        {
            var surl = '../Admin/Login.aspx';
            if (qstr != null && qstr.length > 0)
            {
                surl += qstr;
            }
            top.document.getElementById("frmCenter").src = surl;
        }
        
        function do_setup()
        {
            top.document.getElementById("frmCenter").src = '../Admin/UserAdmin.aspx';
        }
        
        function showLoginData(rs)
        {
            if (rs.error != null)
            {
                return;
            }
            if (rs.value[0] != null) setText(document.getElementById('hdr_userid'), rs.value[0]);
            if (rs.value[1] != null) setText(document.getElementById('hdr_branch'), rs.value[1]);
            
            if (rs.value[3] != null) 
            {
                var futframe = document.getElementById('frmFooter');
                if (futframe)
                {
                    if (futframe.contentDocument)
                    {
                        setText(futframe.contentDocument.getElementById('footpath'), rs.value[3]);
                    }
                    else 
                    {
                        setText(futframe.contentWindow.document.getElementById('footpath'), rs.value[3]);
                    }
                }
            }
            
            if (rs.value[4] > -1 && rs.value[5] == true)
            {
                alert('Your password will expire in ' + rs.value[4] + ' days!');
            }
            else if (rs.value[4] < 0)
            {
                // this should not happen.
                alert('Your password is expired. (' + rs.value[4] + ')');
            }
            
            ldate = rs.value[2];
            var ndt = ldate;
            setText(document.getElementById('hdr_date'), dateToString(ldate));
            window.setInterval(function()
                {
                    ndt.setTime(ndt.getTime() + 1000);
                    setText(document.getElementById('hdr_date'), dateToString(ndt)); 
                }, 1000);
        }
        
        function load_center()
        {
            show_center();
            doOtherStep();
        }
        
        function show_center()
        {
            document.getElementById("frmCenter").src = 'frmCenter.aspx';
        }
                
        function show_welcome()
        {
            IRSWorld.Main.IRSMain.getLoginData(showLoginData);
            var center = window.frames[0];
            if (center)
            {
                try
                {
                    center.show_welcome();
                }
                catch (e) {}
            }
        }
        
        function adjust_dimensions()
        {
            try
            {
                var head = document.getElementById('divHead');
                var tail = document.getElementById('divTail');
                var center = document.getElementById('divCenter');
                var les = 0; //(isIE) ? 0 : (isFF) ? 3 : 0;
                var iht = (window.innerHeight) ? window.innerHeight : (document.documentElement.clientHeight) ? document.documentElement.clientHeight : document.body.clientHeight;
                center.style.height = ((iht - les) - (parseInt(head.offsetHeight) + parseInt(tail.offsetHeight))) + 'px';
            }
            catch (e)
            {}
        }
        
        function doOtherStep()
        {        
            if (authenticated.value == 1)
            {
                show_welcome();
                
                if (!isLogin)
                {
                    if (mfa_enabled())
                    {
                        if (token_authenticated) {
                            IRSWorld.Main.IRSMain.getMenuData(gotMenuData);
                        }
                    }
                    else
                    {
                        get_load_url();
                        IRSWorld.Main.IRSMain.getMenuData(gotMenuData);
                    }

                   
                }
                
                return;
            } 
            
            var inet_url = IRSWorld.Main.IRSMain.getInetPageUrl();  
            if (inet_url.error != null)
            {
                spawn_errpage(inet_url.error.Message);
                return;
            }   
            
            document.getElementById("frmCenter").src = '..' + inet_url.value;
            IRSWorld.Main.IRSMain.getLoginData(showLoginData);
        }
        
        function imagePhraseShown(rs)
        {   
            var show = (rs.error == null) ? rs.value : false;
            if (show)
            {
                top.document.getElementById("frmCenter").src = '../Admin/PersonalizationPage.aspx';
                return;
            }
            
            doOtherStep();
        }
               
        function hasLoggedin(rs)
        {
            if (rs.error != null)
            {
                alert(rs.error.Message);
                if (rs.error.Message.indexOf('Multiple window session') > -1)
                {
                    window.close();
                }
                else
                {
                    spawn_errpage(rs.error.Message);
                }
                return;
            }
            adjust_dimensions();

            authenticated = rs;
            
            if (authenticated.value == 0)
            {
                do_login(top.location.search);
                return;
            }
            
            if (authenticated.value == 2)
            {
                do_setup();
                return;
            }
            
            // Added by Hajz to handle
            if (authenticated.value == 4)
            {
                top.document.getElementById("frmCenter").src = getQueryString(window, 'load').replace('|','=');//'../Internet/InternetRemitterEnrollment.aspx?BranchCode=PAR';
                return;
            }
            
            IRSWorld.Main.IRSMain.showImagePhrase(imagePhraseShown);
        }
        
        function maxWindow()
        {
            try
            {
                top.window.moveTo(0,0);
                if (document.all)
                {
                    top.window.resizeTo(screen.availWidth,screen.availHeight);
                }

                else if (document.layers || document.getElementById)
                {
                    if (top.window.outerHeight<screen.availHeight||top.window.outerWidth<screen.availWidth)
                    {
                        top.window.outerHeight = screen.availHeight;
                        top.window.outerWidth = screen.availWidth;
                    }
                }
            } catch (e) {}
        }
       
        function main_loader()
        {
            AjaxPro.timeoutPeriod = ajax_timeout;

            document.title = IRSWorld.Main.IRSMain.getIRSWVersionNo().value;
             
            maxWindow();
            
            adjust_dimensions();
            
            var reqLogin = true;
            
            if (hasQueryStrings(window))
            {
                var loadURL = getQueryString(window, 'load');
                if (loadURL != null && loadURL.indexOf("InternetRemitterEnrollment") > 0) reqLogin = false;
            }
            
            //IRSWorld.Main.IRSMain.isAuthenticated(top.window.name, hasLoggedin);
            token_authenticated = IRSWorld.Main.IRSMain.check_if_token_exist().value;
            IRSWorld.Main.IRSMain.isAuthenticated(top.window.name, reqLogin, hasLoggedin);
        }
        
        function isAuthenticated()
        {
            try
            {
                return (authenticated.value > 0);
            }
            catch (e)
            {}
            
            return false;
        }
        
        function close_session()
        {
            if (is_closing())
            {
                IRSWorld.Main.IRSMain.close_session();
            }
        }
        
        function main_close_frame()
        {
            var center = window.frames[0];
            if (center)
            {
                try
                {
                    center.close_frame_dialog();
                }
                catch (e) {}
            }        
        }


        function isTokenAuthenticated() {
            try {
                return IRSWorld.Main.IRSMain.check_if_token_exist().value;
            }
            catch (e)
            { }

            return false;
        }

        function token_response() {
            try {
                return IRSWorld.Main.IRSMain.check_ip_response().value;
            }
            catch (e)
            { }

            return false;
        }



        function validate_enrolled_user() {

            ajax_handler();
            //storeMachineFingerPrint();
            return IRSWorld.Main.IRSMain.enroll_entrust().value;

        }



        function validate_token() {
            ajax_handler();
            var t = document.getElementById('txtTPass').value;
            var jsonData = '';//machineSecret.getDeviceFingerprint();
            IRSWorld.Main._2FAPage.validate_log(t, jsonData, validate_token_callback);
        }

        function validate_token_callback(result) {
            if (result.value == true || result.value == 'true') {


                if (!isLogin) {
                    //../Admin/2FAPage.aspx', 'login_close'
                    token_authenticated = result;
                    close_frame_dialog('../Admin/2FAPage.aspx', 'login_close');
                }

            }
            else {
                spawn_errpage('Invalid token value. Please try again');
            }
        }


        function complete_registration() {

            var regcode = document.getElementById('txtRegistrationCode').value;
            var b = true;//IRSWorld.Admin.RegisterChallengeQuestion.complete_registration(regcode).value;
            if (b) {
                close_frame_dialog('../Admin/RegisterChallengeQuestion.aspx', 'login_close');
                IRSWorld.Main.frmCenter.getMessages(hasMessages);
                show_frame_dialog('../Main/2FAPage.aspx?js_closer=login_close', 'login_close', '100%', '100%', 0);
            }
            else {

                alert('Invalid Registration code. Please try again');
            }

        }

        function mfa_enabled()
        {
           return IRSWorld.Main.IRSMain.MFAEnabled().value;
        }

        function ajax_handler() {
            AjaxPro.timeoutPeriod = 1000 * 1000;
            AjaxPro.onTimeout = function () {
                alert("THE SERVER STOPPED RESPONDING. PLEASE TRY AGAIN");

            }
        }
