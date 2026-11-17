        var issa;
        var seqid;
        var crud = '';
        var dtsetup;
        var isagent = false;
    
        function init_ddl(tid, dt, val, desc, ival)
        {
            var ddl = document.getElementById(tid);
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
        
        function btn_enabler(func)
        {
            if (func == 'Add')
            {
                enable_button(document.getElementById('btnAdd'), false);
                enable_button(document.getElementById('btnSave'), false);
                enable_button(document.getElementById('btnEdit'), false);
                enable_button(document.getElementById('btnDelete'), false);
                enable_button(document.getElementById('btnCancel'), true);
                set_read_only(document.getElementById('tblBranchSetupEntry'), false);
                document.getElementById('ddcommsetup').selectedIndex = 1;
                document.getElementById('ddSetupBranch').disabled = true;
                document.getElementById('txtCutStart').value = '';
                document.getElementById('txtCutEnd').value = '';
                document.getElementById('txtStale').value = '0';
                document.getElementById('txtAmountLimit').value = "0.00";
                document.getElementById('ckAggregateAmtLimit').checked = false;
                document.getElementById('txtSuperLimit').value = "0.00";
                document.getElementById('txtTellerLimit').value = "0.00";
                document.getElementById('txtArmLimit').value = "0.00";
                document.getElementById('txtInetLimit').value = "0.00";
                document.getElementById('ckCashAggregation').checked = false;
                document.getElementById('trCashAggregation').style.display = "none";                
                document.getElementById('txtCashAggregationAmt').value = "0.00";
                document.getElementById('txtFxDeviate').value = "0.00";
                document.getElementById('txt_tdd_stimp_min').value = "0.00";
                document.getElementById('txt_tdd_stimp_max').value = "0.00";
                document.getElementById('txt_tdd_firsttime_stimp_min').value = "0.00";
                document.getElementById('txt_tdd_firsttime_stimp_max').value = "0.00";
                //document.getElementById('txt_tdd_cldays_min').value = "0.00";
                //document.getElementById('txt_tdd_cldays_max').value = "0.00";
                document.getElementById('ddreqid').selectedIndex = 0;
                document.getElementById('txtThreeDayAggregation').value = "0.00";
                document.getElementById('txtThirtyDayAggregation').value = "0.00";
                document.getElementById('txtCheckIDAmount').value = "0.00";
                seqid = null;
                return;
            }
            if (func == 'Edit')
            {
                enable_button(document.getElementById('btnAdd'), false);
                enable_button(document.getElementById('btnSave'), false);
                enable_button(document.getElementById('btnEdit'), true);
                enable_button(document.getElementById('btnDelete'), true);
                enable_button(document.getElementById('btnCancel'), true);
                document.getElementById('ddSetupBranch').disabled = !issa;
                return;
            }
            if (func == 'Save')
            {
                enable_button(document.getElementById('btnAdd'), false);
                enable_button(document.getElementById('btnSave'), true);
                enable_button(document.getElementById('btnEdit'), false);
                enable_button(document.getElementById('btnDelete'), false);
                enable_button(document.getElementById('btnCancel'), true);
                return;
            } 
            if (func == 'None')
            {
                enable_button(document.getElementById('btnAdd'), true);
                enable_button(document.getElementById('btnSave'), false);
                enable_button(document.getElementById('btnEdit'), false);
                enable_button(document.getElementById('btnDelete'), false);
                enable_button(document.getElementById('btnCancel'), false);
                set_read_only(document.getElementById('tblBranchSetupEntry'), true);
                document.getElementById('ddSetupBranch').disabled = !issa;
                crud = '';
                return;
            }      
        }
        
        function update_setup()
        {
            document.getElementById('ddSetupBranch').disabled = true;
            set_read_only(document.getElementById('tblBranchSetupEntry'), false);
            enable_button(document.getElementById('btnEdit'), false);
            crud = 'Edit';
        }
        
        function new_setup()
        {
            btn_enabler('Add');
            crud = 'Add';
        }
        
        function isTime(tval)
        {
            var tpart = tval.split(' ');
            if (tpart.length > 2) return false;
            var hhmmss = tpart[0].split(':');
            if (hhmmss.length > 3) return false;
            var pdt = Date.parse('07/25/2009 ' + tpart[0]);
            if (isNaN(pdt)) return false;
            
            return true;            
        }
        
        function new_table(rs)
        {   
            if (rs.error != null)
            {
                spawn_errpage(rs.error.Message);
                return;
            }
            show_brsetup_data(rs.value);
        }
        
        function new_branch(brcode)
        {
            btn_enabler('Add');
            IRSWorld.BranchSetup.BranchSetup.getBranchSetup(brcode, new_table)
            document.getElementById('tdd_rows').style.display = (brcode == 'T$$' | brcode == 'USW' | brcode == 'TEL') ? '' : 'none';
            document.getElementById('trDailyLimit').style.display = (brcode == 'T$$' | brcode == 'USW') ? '' : 'none';
        }
        
        function getOldValue(colname)
        {
            var old_value = dtsetup.Rows[0][colname];
            if (colname.indexOf('CutOff') > -1)
            {
                var sdate = dateToString(old_value);
                var ss = sdate.split(' ');
                return ss[1] + ' ' + ss[2];
            }
            return old_value;
        }
        
        function value_changed(obj)
        {
            if (crud == 'Add' | crud == 'Edit') 
            {
                var id = obj.id;
                var colname = obj.col_name;
                var need_focus = false;

                if (id == 'txtStale')
                {
                    if (!isNumeric(obj.value))
                    {
                        if (obj.value.length > 0)
                        {
                            alert('Stale Period must be numeric integer!');
                            need_focus = true;
                        }
                    }
                    else if (parseInt(obj.value) < 0)
                    {
                        alert('Stale Period value must be positive integer!');
                        need_focus = true;
                    }
                    else if (parseInt(obj.value) > 32767)
                    {
                        alert('Stale Period value exceeds capacity of the field.');
                        need_focus = true;
                    }
                }
                else if (id.substr(0, 6) == 'txtCut')
                {
                    if (!isTime(obj.value))
                    {
                        alert(colname + ': entry must be valid time.');
                        need_focus = true;
                    }
                }
                else if (id == 'ddcommsetup')
                {
                    if (obj.value < 1) 
                    {
                        alert(colname + ': please select Online or Batch');
                        need_focus = true;
                    }
                }
                else if (id == 'ddSetupBranch')
                {
                    if (obj.value < 1) 
                    {
                        alert(colname + ': please select a branch');
                        need_focus = true;
                    }            
                }
                else if (id == 'txtAmountLimit' 
                | id == 'txtSuperLimit' 
                | id == 'txtTellerLimit' 
                | id == 'txtFxDeviate' 
                | id == 'txtArmLimit' 
                | id == 'txtInetLimit' 
                | id == 'txtCashAggregationAmt'
                | id == 'txt_tdd_stimp_min'  
                | id == 'txt_tdd_stimp_max'
                | id == 'txt_tdd_firsttime_stimp_min'  
                | id == 'txt_tdd_firsttime_stimp_max'
                //| id == 'txt_tdd_cldays_min'
                //| id == 'txt_tdd_cldays_max'
                | id == 'txtAgentLogReport'
                | id == 'txtThreeDayAggregation'
                | id == 'txtThirtyDayAggregation' 
                | id == 'txt_daily_limit'
                | id == 'txtCheckIDAmount'
                )
                {
                    var mony = parseFloat(obj.value);
                    var pos = (id == 'txt_tdd_stimp_min' | id == 'txt_tdd_stimp_max' | id == 'txt_tdd_firsttime_stimp_min' | id == 'txt_tdd_firsttime_stimp_max') ? 1 : 0;
                    if (!isNumeric(obj.value)) 
                    {
                        var msg = (id == 'txtFxDeviate') ? ': must be numeric' : ': must be numeric in money format';
                        alert (colname + msg);
                        need_focus = true;
                    }
                    else if (mony < pos)
                    {
                        alert (colname + ': value must be positive > ' + pos);
                        need_focus = true;
                    }
                    else
                    {
                        var max = (id == 'txtFxDeviate') ? 999.99 : 999999999.99;
                        if (mony > max) 
                        {
                            alert(colname + " value exceeds limit, (" + max + ").");
                            need_focus = true;
                        }
                    }
                    if (!need_focus && id.indexOf('_max') > -1)
                    {
                        var pref = id.substr(0, id.indexOf('_max'))
                        var minobj = document.getElementById(pref + '_min');
                        if (minobj && parseFloat(minobj.value) > mony)
                        {
                            alert('Max must be greater that min');
                            need_focus = true;
                        }
                    }
                    if (!need_focus) obj.value = formatNumber(mony, 2);
                }
                else if (id == 'ckCashAggregation')
                {
                    document.getElementById('trCashAggregation').style.display = (obj.checked) ? '' : 'none';
                    if (!obj.checked)
                    {
                        document.getElementById('txtCashAggregationAmt').value = '0.00';
                    }
                }
                else if (id == 'ckAggregateAmtLimit')
                {
                    if (isagent == true)
                    {
                        document.getElementById('lblAgentReject').style.display = (obj.checked) ? '' : 'none';
                        document.getElementById('chkAgentAggAmtReject').style.display = (obj.checked) ? '' : 'none';
                    }
                }
                
                if (need_focus)
                {
                    obj.focus();
                    if (obj.tagName != 'select') obj.select();
                    enable_button(document.getElementById('btnSave'), false);
                }
                else
                {
                    var old_value = null;
                    if (crud != 'Add')
                    {
                        old_value = getOldValue(colname);
                    }
                    if (crud == 'Add' || obj.value != old_value) btn_enabler('Save', true);
                }
            }
        }
        
        function setup_saved(rs)
        {
            if (rs.error != null) 
            {
                alert(rs.error.Message);
                return;
            }

            /* RFC-31620 */
            var msg = IRSWorld.BranchSetup.BranchSetup.isAdminApproverTag().value;
            if (msg != "") {
                alert(msg);
                cancel_setup();
                return;
            }
            
            show_brsetup_data(rs.value);
        }
        
        function save_setup()
        {
            var brcode = document.getElementById('ddSetupBranch').value;
            if ((brcode == 'T$$' | brcode == 'USW' | brcode == 'TEL') && 
                (parseFloat('txt_tdd_stimp_min') < 1 & parseFloat('txt_tdd_stimp_max') < 1 & parseFloat('txt_tdd_firsttime_stimp_min') < 1 & parseFloat('txt_tdd_firsttime_stimp_max') < 1))
            {
                alert('TDD Straight Implementation min/max field(s) must have value > 0');
                return;
            } 
            
            var setup_record = "crud=" + crud + "~";
                
            if (crud == 'Edit') 
            {
                setup_record += "SEQ_ID=" + seqid + "~";
            }
            
            setup_record += "BranchCode=" + brcode + "~";
            setup_record += "CutOffStart=" + document.getElementById('txtCutStart').value + "~";
            setup_record += "CutOffEnd=" + document.getElementById('txtCutEnd').value + "~";
            setup_record += "StalePeriod=" + document.getElementById('txtStale').value + "~";
            var comset = (Math.abs(parseInt(document.getElementById('ddcommsetup').value) - 2) > 0);
            setup_record += "CommSetup=" + comset + "~";
            setup_record += "RemittanceAmtLimit=" + document.getElementById('txtAmountLimit').value + "~";
            setup_record += "AggregateAmtLimit=" + document.getElementById('ckAggregateAmtLimit').checked + "~";
            setup_record += "SupervisorLimit=" + document.getElementById('txtSuperLimit').value + "~";
            setup_record += "TellerLimit=" + document.getElementById('txtTellerLimit').value + "~";
            setup_record += "ARMLimit=" + document.getElementById('txtArmLimit').value + "~";
            setup_record += "InetLimit=" + document.getElementById('txtInetLimit').value + "~";
            setup_record += "RequiredID=" + document.getElementById('ddreqid').value + "~";
            setup_record += "FxDeviation=" + document.getElementById('txtFxDeviate').value + "~";
            setup_record += "TDD_StImp_Min=" + document.getElementById('txt_tdd_stimp_min').value + "~";
            setup_record += "TDD_StImp_Max=" + document.getElementById('txt_tdd_stimp_max').value + "~";
            setup_record += "TDD_FirstTimeStImp_Min=" + document.getElementById('txt_tdd_firsttime_stimp_min').value + "~";
            setup_record += "TDD_FirstTImeStImp_Max=" + document.getElementById('txt_tdd_firsttime_stimp_max').value + "~";
            //setup_record += "TDD_ClDays_Min=" + document.getElementById('txt_tdd_cldays_min').value + "~";
            //setup_record += "TDD_ClDays_Max=" + document.getElementById('txt_tdd_cldays_max').value + "~";
            setup_record += "CashAggregationLimit=" + document.getElementById('ckCashAggregation').checked + "~";
            setup_record += "CashAggregationAmtLimit=" + document.getElementById('txtCashAggregationAmt').value + "~";
            setup_record += "AgentSysControl=" + document.getElementById('chkAgentLogReport').checked + "~";
            setup_record += "AgentLogReport=" + document.getElementById('txtAgentLogReport').value + "~";
            setup_record += "ThreeDayAggregation=" + document.getElementById('txtThreeDayAggregation').value + "~";
            setup_record += "ThirtyDayAggregation=" + document.getElementById('txtThirtyDayAggregation').value + "~";
            setup_record += "AgentAggAmtReject=" + document.getElementById('chkAgentAggAmtReject').checked + "~";
            setup_record += "DailyLimit=" + document.getElementById('txt_daily_limit').value  + "~";
            setup_record += "AgentCheckIDAmount=" + document.getElementById('txtCheckIDAmount').value
            
            IRSWorld.BranchSetup.BranchSetup.save_setup(setup_record, setup_saved);
        }
        
        function setup_deleted(rs)
        {
            if (rs.error != null)
            {
                alert(rs.error.Message);
                return;
            }

            /* RFC-31620 */
            var msg = IRSWorld.BranchSetup.BranchSetup.isAdminApproverTag().value;
            if (msg != "") {
                alert(msg);
                cancel_setup();
                return;
            }
            else {
                alert("Deletion is successful");
            }
            
            clear_fields(document.getElementById('tblBranchSetupEntry'));
            show_brsetup_data(rs.value);
        }
        
        function delete_setup()
        {
            if (confirm("Delete Branch Setup data for: " + document.getElementById('ddSetupBranch').value))
            {
                IRSWorld.BranchSetup.BranchSetup.delete_setup(seqid, document.getElementById('ddSetupBranch').value, setup_deleted);
            }
        }
        
        function cancel_setup()
        {
            crud = '';
            show_brsetup_data(dtsetup);
        }
        
        function show_brsetup_data(dt)
        {
            dtsetup = dt;
            crud = '';
            if (dt.Rows.length > 0)
            {
                var idx = dt.Rows.length - 1;
                set_read_only(document.getElementById('tblBranchSetupEntry'), true);
                seqid = dt.Rows[idx].SEQ_ID;
                document.getElementById('ddSetupBranch').value = dt.Rows[idx].BranchCode;
                document.getElementById('ddSetupBranch').col_name = 'BranchCode';
                document.getElementById('txtCutStart').value = dateToString(dt.Rows[idx].CutOffStart).substring(11);
                document.getElementById('txtCutStart').col_name = 'CutOffStart';
                document.getElementById('txtCutEnd').value = dateToString(dt.Rows[idx].CutOffEnd).substring(11);
                document.getElementById('txtCutEnd').col_name = 'CutOffEnd';
                document.getElementById('txtStale').value = dt.Rows[idx].StalePeriod;
                document.getElementById('txtStale').col_name = 'StalePeriod';
                document.getElementById('ddcommsetup').value = (dt.Rows[idx].CommSetup) ? 1 : 2;
                document.getElementById('ddcommsetup').col_name = 'CommSetup';
                document.getElementById('txtAmountLimit').value = formatNumber(dt.Rows[idx].RemittanceAmtLimit, 2);
                document.getElementById('txtAmountLimit').col_name = 'RemittanceAmtLimit';
                document.getElementById('ckAggregateAmtLimit').checked = dt.Rows[idx].AggregateAmtLimit;
                document.getElementById('ckAggregateAmtLimit').col_name = 'AggregateAmtLimit';
                document.getElementById('txtSuperLimit').value = formatNumber(dt.Rows[idx].SupervisorLimit, 2);
                document.getElementById('txtSuperLimit').col_name = 'SupervisorLimit';
                document.getElementById('txtTellerLimit').value = formatNumber(dt.Rows[idx].TellerLimit, 2);
                document.getElementById('txtTellerLimit').col_name = 'TellerLimit';
                document.getElementById('txtArmLimit').value = formatNumber(dt.Rows[idx].ARMLimit, 2);
                document.getElementById('txtArmLimit').col_name = 'ARMLimit';
                document.getElementById('txtInetLimit').value = formatNumber(dt.Rows[idx].InetLimit, 2);
                document.getElementById('txtInetLimit').col_name = 'InetLimit';
                //Web Remittance Aggregation
                document.getElementById('txtThreeDayAggregation').value = formatNumber(dt.Rows[idx].ThreeDayAggregation, 2);
                document.getElementById('txtThreeDayAggregation').col_name = 'ThreeDayAggregation';
                document.getElementById('txtThirtyDayAggregation').value = formatNumber(dt.Rows[idx].ThirtyDayAggregation, 2);
                document.getElementById('txtThirtyDayAggregation').col_name = 'ThirtyDayAggregation';
                //Cash Aggregation
                document.getElementById('ckCashAggregation').checked = dt.Rows[idx].CashAggregationLimit;
                document.getElementById('ckCashAggregation').col_name = 'CashAggregationLimit';
                document.getElementById('trCashAggregation').style.display = (dt.Rows[idx].CashAggregationLimit == null || dt.Rows[idx].CashAggregationLimit == false) ? "none" : "";
                document.getElementById('txtCashAggregationAmt').value = formatNumber(dt.Rows[idx].CashAggregationAmtLimit, 2);
                document.getElementById('txtCashAggregationAmt').col_name = 'CashAggregationAmtLimit';
                //End Cash Aggregation
                //Agent System Control
                document.getElementById('chkAgentLogReport').checked = dt.Rows[idx].AgentSysControl;
                document.getElementById('chkAgentLogReport').col_name = "AgentSysControl";
                document.getElementById('txtAgentLogReport').value = formatNumber(dt.Rows[idx].AgentLogReport, 2);
                document.getElementById('txtAgentLogReport').col_name = "AgentLogReport";
                //End Agent System Control
                //Agent Aggregate Outright Reject
                document.getElementById('chkAgentAggAmtReject').checked = dt.Rows[idx].AgentAggAmtReject;
                document.getElementById('chkAgentAggAmtReject').col_name = "AgentAggAmtReject";
                //End Agent Aggregate Outright Reject
                document.getElementById('txtFxDeviate').value = formatNumber(dt.Rows[idx].FxDeviation, 2);
                document.getElementById('txtFxDeviate').col_name = 'FxDeviation';
                document.getElementById('txt_tdd_stimp_min').value = formatNumber(dt.Rows[idx].TDD_StImp_Min, 2);
                document.getElementById('txt_tdd_stimp_min').col_name = 'TDD_StImp_Min';
                document.getElementById('txt_tdd_stimp_max').value = formatNumber(dt.Rows[idx].TDD_StImp_Max, 2);
                document.getElementById('txt_tdd_stimp_max').col_name = 'TDD_StImp_Max';
                document.getElementById('txt_tdd_firsttime_stimp_min').value = formatNumber(dt.Rows[idx].TDD_FirstTimeStImp_Min, 2);
                document.getElementById('txt_tdd_firsttime_stimp_min').col_name = 'TDD_FirstTimeStImp_Min';
                document.getElementById('txt_tdd_firsttime_stimp_max').value = formatNumber(dt.Rows[idx].TDD_FirstTimeStImp_Max, 2);
                document.getElementById('txt_tdd_firsttime_stimp_max').col_name = 'TDD_FirstTimeStImp_Max';
                //document.getElementById('txt_tdd_cldays_min').value = formatNumber(dt.Rows[idx].TDD_ClDays_Min, 2);
                //document.getElementById('txt_tdd_cldays_min').col_name = 'TDD_ClDays_Min';
                //document.getElementById('txt_tdd_cldays_max').value = formatNumber(dt.Rows[idx].TDD_ClDays_Max, 2);
                //document.getElementById('txt_tdd_cldays_max').col_name = 'TDD_ClDays_Max';
                
                document.getElementById('txt_daily_limit').value = formatNumber(dt.Rows[idx].DailyLimit, 2);
                document.getElementById('txt_daily_limit').col_name = 'DailyLimit';
                
                document.getElementById('txtCheckIDAmount').value = formatNumber(dt.Rows[idx].AgentCheckIDAmount, 2);
                document.getElementById('txtCheckIDAmount').col_name = 'AgentCheckIDAmount';
                
                document.getElementById('ddreqid').value = dt.Rows[idx].RequiredID;
                document.getElementById('ddreqid').col_name = 'RequiredID';
                document.getElementById('ckAggregateAmtLimit').ReadOnly = true;
                document.getElementById('ckCashAggregation').ReadOnly = true;
                
                isAgent(dt.Rows[idx].BranchCode);
                btn_enabler('Edit');
            }
            else 
            {
                if (confirm("No setup record for branch: " + document.getElementById('ddSetupBranch').value + "\nSetup Now?"))
                {
                    isAgent(document.getElementById('ddSetupBranch').value);
                    btn_enabler('Add');
                    crud = 'Add';
                }
                else
                {
                    btn_enabler('None');
                }
            }
        }

        function had_branches(retval)
        {
            if (retval.error != null)
            {
                spawn_errpage(retval.error.Message);
                return;
            }
            
            var selbr = retval.value[0];
            var dtbr = retval.value[1];
            var dtcom = retval.value[2];
            var dtid = retval.value[3];
            var dt = retval.value[4];
            issa = retval.value[5];
            
            init_ddl('ddSetupBranch', dtbr, 'BranchCode', 'BranchName', selbr);
            document.getElementById('ddSetupBranch').disabled = !issa;
            init_ddl('ddcommsetup', dtcom, 'FldValue', 'FldCode', 1);
            init_ddl('ddreqid', dtid, 'FldValue', 'FldDescription', 0);
            
            document.getElementById('divBody').style.display = ''; 
            document.getElementById('tdd_rows').style.display = (selbr == 'T$$' | selbr == 'USW' | selbr == 'TEL') ? '' : 'none';
            document.getElementById('trDailyLimit').style.display = (selbr == 'T$$' | selbr == 'USW') ? '' : 'none';

            //Cash Aggregation Changes
            document.getElementById('trCashAggregation').style.display = 'none';
            document.getElementById('ckCashAggregation').checked = false;
            //end Cash Aggregation Changes
            show_brsetup_data(dt);
            ajax_show_status(false);
        }
        
        function fixht()
        {
            document.getElementById('divBody').style.height = (top.document.getElementById('divCenter').offsetHeight - 30) + 'px';
            document.getElementById('divBody').style.width = (top.document.getElementById('divCenter').offsetWidth - 3) + 'px';
            document.getElementById('tdContent').style.width =  document.getElementById('divBody').style.width; 
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
                document.getElementById('tdContent').style.verticalAlign = 'top';
                fixht();
            } 
            else
            {
                document.getElementById('divBranchSetupContent').style.width = '95%';
            }       
        }        

        function page_setup()
        {
            if (top == self)
            {
                window.open('./BranchSetupMain.aspx?load=BranchSetup.aspx', 'top');
            }
            else if (top.isAuthenticated())
            {
                fixmain();
                var x = '';
                ajax_show_status(true);
                IRSWorld.BranchSetup.BranchSetup.getBranches(x, had_branches);
            }
            else
            {
                spawn_errpage("This module requires login!");
            }      
        }
        
        function close_session()
        {
            if (is_closing())
            {
                IRSWorld.BranchSetup.BranchSetup.close_session();
            }
            else
            {
                IRSWorld.BranchSetup.BranchSetup.drop_brsetup();
            }
        }
        
        function isAgent(brcode)
        {
            IRSWorld.BranchSetup.BranchSetup.isAgent(brcode, enableAgentSysControl);
        }
        
        function enableAgentSysControl(rs)
        {
            if (rs.error != null)
            {
                spawn_errpage(rs.error.Message);
                return;
            }
            isagent = rs.value;
            if(rs.value == true)
            {
                document.getElementById('trAgentLogReport').style.display ='';
                if(document.getElementById('ckAggregateAmtLimit').checked == true)
                {
                    document.getElementById('lblAgentReject').style.display ='';
                    document.getElementById('chkAgentAggAmtReject').style.display ='';
                }
                else
                {
                    document.getElementById('lblAgentReject').style.display ='none';
                    document.getElementById('chkAgentAggAmtReject').style.display ='none';
                }
            }
            else
            {
                document.getElementById('trAgentLogReport').style.display ='none';
                document.getElementById('lblAgentReject').style.display ='none';
                document.getElementById('chkAgentAggAmtReject').style.display ='none';
            }
        }