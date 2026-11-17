    
        var authenticated;
        
        var sidebar = new Array
        (
		    new Array(1, 0,	'Branch Code Table', 'javascript:loadframe("./Branch_Definition.aspx");'),
		    new Array(2, 0, 'Branch Setup', 'javascript:loadframe("./BranchSetup.aspx");'),
		    new Array(3, 0, 'Currency Categories', 'javascript:loadframe("./CurrencyGroup.aspx");'),
            new Array(4, 0, 'Currency Rates', 'javascript:loadframe("./CurrencyRates.aspx");'),
		    new Array(5, 0,	'Branch Defaults', 'javascript:loadframe("./BranchDefaults.aspx");'),
		    new Array(6, 0,	'Required IDs', 'javascript:loadframe("./Valid_IDs.aspx");'),
		    new Array(7, 0,	'Receipts', 'javascript:loadframe("./Receipts.aspx");'), 
		    new Array(8, 0,	'Dynamic Validations', 'javascript:loadframe("./DynaValidation.aspx");'),  
		    new Array(9, 0,	'Holidays', 'javascript:loadframe("./Holidays.aspx");'), 
		    new Array(10,0, 'Accounts', 'javascript:loadframe("./Accounts.aspx");'), 
		    new Array(11,0, 'Charges', 'javascript:loadframe("./Charges.aspx");'), 
		    new Array(12,0, 'Message Groups', 'javascript:loadframe("./MessageGroup.aspx");'),
		    new Array(13,0, 'Messages', 'javascript:loadframe("./Messages.aspx");'),
		    new Array(14,0, 'Lookup Setup', 'javascript:loadframe("./BranchLookup.aspx");'),
		    new Array(15,0, 'Exceptions', 'javascript:loadframe("./Exceptions.aspx");'),
//		    new Array(16,0, 'Payment Gateway Defaults', 'javascript:loadframe("./PaymentGatewayDefaults.aspx");'),
		    new Array(17,0, 'Disclaimer', 'javascript:loadframe("./Disclaimer.aspx");'),
		    new Array(18,0, 'Branch Operation', 'javascript:loadframe("./BranchOperation.aspx");'),
		    new Array(19,0, 'Machine Setup', 'javascript:loadframe("./MachineSetup.aspx");'),
		    new Array(20,0, 'GiftsWeb Settings', 'javascript:loadframe("./GiftsWebSettings.aspx");'),
		    new Array(21,0, 'End Of Transfer', 'javascript:loadframe("./EndOfTransfer.aspx");')
		);
		
		function filter_sidebar_items()
		{
		    var rs = IRSWorld.BranchSetup.BranchSetupMain.getMenuData(sidebar);
		    if (rs.error != null)
		    {
		        spawn_errpage(rs.error.Message);
		        return;
		    }
		       
		    var sb = new Array();
		    var k = 0;
		    
		    for (var i = 0; i < rs.value.length; i++)
		    {
		        var iid = parseInt(rs.value[i]);
		        for (j = 0; j < sidebar.length; j++)
		        {
		            var item = sidebar[j];
		            var sbid = parseInt(item[0]);
		            if (sbid == iid)
		            {
		                sb[k++] = item;
		                break;
		            }
		        }
		    }
		    
		    if (sb.length == 0)
		    {
		        alert("No Branch Setup module is accessible to your login.");
		        return;
		    }
    
            var sider = new js_sidebar('sidemenu', sb);
            document.getElementById('divBranchSetupMenu').innerHTML = sider.toString();
		}
		
		function is_branchsetup_main()
		{
		    return true;
		}
        
        function hide_menu()
        {
            var div = document.getElementById("divBranchSetupMenuTop");
            var cset = div.style.display;
            div.style.display = (cset == "block") ? "none" : "block";
            document.getElementById("tdbrsetupmenu").style.width = (cset == "block") ? "1%" : "18%";
            setText(document.getElementById("ahider"), ((cset == "block") ? String.fromCharCode(187) : String.fromCharCode(171)));
        }   
             
        function isAuthenticated()
        {
            return authenticated;
        }
    
        function do_loader()
        {
            var furl = getQueryString(top, 'load');
            
            if (furl != undefined)
            {
                loadframe(furl);
            }        
        }
        
        function fix_height()
        {
            if (top.document.getElementById('divCenter'))
            {
                var tbl = document.getElementById('tblContent');
                var div = document.getElementById('divContent');
                
                tbl.style.width = top.document.getElementById('divCenter').offsetWidth + 'px';
                tbl.style.height = (top.document.getElementById('divCenter').offsetHeight-5) + 'px';
                div.style.height = (top.document.getElementById('divCenter').offsetHeight - 5) + 'px';
                
                var divmenu = document.getElementById("divBranchSetupMenu");
                divmenu.style.height =  (top.document.getElementById('divCenter').offsetHeight - 40) + 'px';
            }
        }
        
        function page_setup()
        {
            if (top == self)
            {
                window.open('../Main/IRSMain.aspx?load=../BranchSetup/BranchSetupMain.aspx', '_parent');
            }
            else
            {
                if (top.isAuthenticated())
                {
                    authenticated = true;
                    fix_height();
                    top.frame_scroll('no');
                    filter_sidebar_items();
                    if (hasQueryStrings(top) && getQueryString(top, 'redirect') == null)
                    {
                        do_loader();
                    }
                    else
                    {
                        document.getElementById('tblContent').style.display = 'block';
                        js_sidebar_click('sidemenu', 2, 0);
                    }
                }
                else
                {
                    spawn_errpage("Branch setup functions requires login!");
                }
            }
        }
        
        function loadframe(url)
        {
            var ifrem = document.getElementById('ifbranchsetup');
            if (ifrem)
            {
                ifrem.src = url;
            }
        }
        