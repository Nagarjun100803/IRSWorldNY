function epsco_client()
{
    //Remitter
    this.RemLastName = null;
    this.RemFirstName = null;
    this.RemMiddleName = null;
    this.RemBirthDate = "";
    this.RemAge = null;
    
    this.RemAddress = null;
    this.RemRegion = null;
    this.RemCountry = null;
    
    this.RemZipCode = null;
    
    this.RemEmail = null;
    this.RemTelNo = null;
    this.RemMobNo = null;
    this.RemBranchCode = null;
    
    //Beneficiary
    this.BenLastName = null;
    this.BenFirstName = null;
    this.BenMiddleName = null;
    this.BenBirthDate = "";
    this.BenAge = null;
    
    this.BenAddress = null;
    this.BenZipCode = null;
    
    this.BenTelNo = null;
    this.BenMobNo = null;
    
    this.BenAcctNo = null;
    this.BenBranchCode = null;
    
    //Misc.
    
    this.HK_ID = null;
    this.EnrollingBranch = null;
    
    this.Rem_SEQID = null;
    this.Ben_SEQID = null;
    this.Ben_EPSCO_SEQID = null;
    
    this.CIF_SEQ_ID = null;
    this.CIFClass = null;
    
    this.PrintOnly = false;
    this.enrollStatus = 0;
    
    this.approverMode = false;
    this.approver_selected_id = null;
    
    this.selected_field = null;
    
    this.RemLName = null;
    this.RemFName = null;
    this.RemMName = null;
    this.BenLName = null;
    this.BenFName = null;
    this.BenMName = null;
    this.BenAcctNo = null;
    
    this.isRemitterShow = null; 
    this.isZipCodeShow = null;
    
    this.RemMandatoryFields = null;
    this.BenMandatoryFields = null;
    
    this.no2digt = false;
    
    this.dataTable = null;
    
    this.dbgrid = null;
    
    this.grid_header_caption = "";
    this.grid_btn_caption = "";
    this.grid_btn_enabled = true;
    
    //---------------------------------------------------------------
    
    this.generic = new generic_method();
    
    this.page_initialize = function()
    {
        AjaxPro.timeoutPeriod = 1000 * 1000;
        AjaxPro.onTimeout = function()
        {
            alert("THE SERVER STOPPED RESPONDING. PLEASE TRY AGAIN");
            ajax_show_status(false);
        }
        
        uRole = IRSWorld.EPSCO.EPSCOEnrollment.getUserRole().value;
        uLoc = IRSWorld.EPSCO.EPSCOEnrollment.getUserLoc().value;
        uID = IRSWorld.EPSCO.EPSCOEnrollment.getUserID().value;
        epsco_client.RemBranchCode = uLoc;
        
        if(uRole == 4)
        {
            epsco_client.showEnrolleeForm();
            epsco_client.approverMode = true;
        }
        else
        {
            epsco_client.showEnrollmentForm();
            epsco_client.approverMode = false;
        }
        
        epsco_client.setMandatoryFields();
    }
    
    this.showEnrollmentForm = function()
    {
        showPanelEP('divEnrollCIF');
        divmask.hide();
        clearForm('divEnrollCIF');
        epsco_trans.popDDL('ddlEnrollingBranch','');
        epsco_trans.popDDL('ddlRemCountry','');
        epsco_trans.popDDL('ddlBenBranch','');
    }
    
    this.showEnrolleeForm = function()
    {
        showPanelEP('divEnrollees');
        divmask.hide();
        epsco_client.autoRemSearch(2);
    }
    
    this.isFound = function(str, i)
    {
        if(i==1) array = this.RemMandatoryFields;
        if(i==2) array = this.BenMandatoryFields;
        
        for(x in array)
        {
            if(array[x] == str) return true;
        }
        
        return false;
    }
    
    this.setMandatoryFields = function()
    {
        this.RemMandatoryFields = IRSWorld.EPSCO.EPSCOEnrollment.getMandatoryFields(1).value.split('~');
        this.BenMandatoryFields = IRSWorld.EPSCO.EPSCOEnrollment.getMandatoryFields(2).value.split('~');
        
        if(this.isFound("BranchCode", 1)) document.getElementById("mnRemBranch").style.color = "#FF0000";
        if(this.isFound("LastName", 1)) document.getElementById("mnRemLName").style.color = "#FF0000";
        if(this.isFound("FirstName", 1)) document.getElementById("mnRemFName").style.color = "#FF0000";
        if(this.isFound("MiddleName", 1)) document.getElementById("mnRemMName").style.color = "#FF0000";
        if(this.isFound("BirthDate", 1)) document.getElementById("mnRemBDate").style.color = "#FF0000";
        if(this.isFound("Address1", 1)) document.getElementById("mnRemAdd1").style.color = "#FF0000";
        if(this.isFound("Address2", 1)) document.getElementById("mnRemAdd2").style.color = "#FF0000";
        if(this.isFound("ZipCodeSeqId", 1)) document.getElementById("mnRemZipCode").style.color = "#FF0000";
        if(this.isFound("EmailAddress", 1)) document.getElementById("mnRemEmail").style.color = "#FF0000";
        
        if(this.isFound("LastName", 2)) document.getElementById("mnBenLName").style.color = "#FF0000";
        if(this.isFound("FirstName", 2)) document.getElementById("mnBenFName").style.color = "#FF0000";
        if(this.isFound("MiddleName", 2)) document.getElementById("mnBenMName").style.color = "#FF0000";
        if(this.isFound("BirthDate", 2)) document.getElementById("mnBenBDate").style.color = "#FF0000";
        if(this.isFound("Address1", 2)) document.getElementById("mnBenAdd1").style.color = "#FF0000";
        if(this.isFound("Address2", 2)) document.getElementById("mnBenAdd2").style.color = "#FF0000";
        if(this.isFound("ZipCodeSeqId", 2)) document.getElementById("mnBenZipCode").style.color = "#FF0000";
    }
    
    this.PrintRemCard = function(elem)
    {
        var originalContents = document.body.innerHTML;
        
        document.getElementById(elem).style.height = "100%";
        document.getElementById(elem).style.top = (((document.documentElement.clientHeight / 2) + document.documentElement.scrollTop) - (document.getElementById(elem).offsetHeight / 2)) + 'px';
        document.getElementById(elem).style.left = (((document.documentElement.clientWidth / 2) + document.documentElement.scrollLeft) - (document.getElementById(elem).offsetWidth / 2)) + 'px';
        
        document.getElementById("btnPrintRemCard").style.visibility = "hidden";
        document.getElementById("btnCloseRemCard").style.visibility = "hidden";
        
        var printContents = document.getElementById(elem).innerHTML;
        document.body.innerHTML = printContents;
        
        window.print();
        
        document.body.innerHTML = originalContents;
    }
    
    this.enroll_client = function(CIFClass, method)
    {
        if(epsco_client.validate_input(CIFClass) == true)
        {  
                if(CIFClass == 1 || CIFClass == 0)
                {
                    var CIF = document.getElementById("txtRemLastName").value
                              + '~' + document.getElementById("txtRemFirstName").value + '~';
                              
                    if(document.getElementById("txtRemBDate").value.replace(" ","") == "")
                    {
                        CIF += "01/01/1753";
                    }
                    else
                    {
                        CIF += document.getElementById("txtRemBDate").value;
                    }
                              
                    if(method == "UPDATE")
                    {
                        CIF = CIF + '~~~01/01/1753~1~' +  epsco_client.Rem_SEQID + '~0~~0';
                    }
                    else
                    {
                        if(epsco_client.Rem_SEQID == null){epsco_client.Rem_SEQID = "0";}
                        if(epsco_client.Ben_SEQID == null){epsco_client.Ben_SEQID = "0";}
                        
                        CIF = CIF + '~' + document.getElementById("txtBenefLastName").value
                            + '~' + document.getElementById("txtBenefFirstName").value + '~';
                            
                        if(document.getElementById("txtBenefBDate").value.replace(" ","") == "")
                        {
                            CIF += "01/01/1753";
                        }
                        else
                        {
                            CIF += document.getElementById("txtBenefBDate").value;
                        }
                        
                        CIF += '~1~' + epsco_client.Rem_SEQID + '~' + epsco_client.Ben_SEQID + '~' + document.getElementById('txtBenefAcctNo').value + '~1';
                    }
                }
                
                if(CIFClass == 2)
                {
                    var CIF = document.getElementById("txtBenefLastName").value
                            + '~' + document.getElementById("txtBenefFirstName").value + '~';
                    
                    if(document.getElementById("txtBenefBDate").value.replace(" ","") == "")
                    {
                        CIF += "01/01/1753";
                    }
                    else
                    {
                        CIF += document.getElementById("txtBenefBDate").value;
                    }
                    
                    CIF += '~~~01/01/1753~2' + '~0~' + epsco_client.Ben_SEQID + '~~0';
                }
                
                CIF += "~" + uLoc;
                
                ajax_show_status(true);
                
                IRSWorld.EPSCO.EPSCOEnrollment.getMatchedCIF(CIF.toString(), CIFClass.toString(), method.toString() , epsco_client.enroll_client_callback);
        }
    }
    
    this.enroll_client_callback = function(param)
    {
        ajax_show_status(false);
        
        var result = param.value.toString().split('~');
              
        var CIFClass = result[1];
        var method = result[2];
        
        //if(result[0] > 0)
        if(result[0] > 0 && method != "UPDATE")
        {
            alert("Client is already existing.");
        }
        else
        {
            if(CIFClass == 2 || CIFClass == 0)
            {
                epsco_client.check_acct_no(document.getElementById("txtBenefAcctNo").value, method.toString());
            }
            else
            {
                epsco_client.setClientInfo(CIFClass);
                epsco_client.setAddress(CIFClass);
                epsco_client.setAge(CIFClass);
                epsco_client.construct_insert_record(CIFClass, method);
            }
        }
    }
    
    this.popCIFGrid = function(CIFClass)
    {
        epsco_client.CIFClass = CIFClass;
        epsco_client.load_grid_viewer(CIFClass);
        
        document.getElementById("divSearchClient").style.minHeight = "350px";
        document.getElementById("txtsearchLName1").value = "";
        document.getElementById("txtsearchFName1").value = "";
        document.getElementById("txtsearchMName1").value = "";
        
        showPanelEP("divSearchClient");
        
        document.getElementById("divSearchClient").style.minHeight = "0px";
    }
    
    this.load_remitter_search = function()
    {
        epsco_client.CIFClass = 1;
        
        this.ajax_grid_sel = new grid_selector();
        
        identity_column = 0;
        page = "../AjxPage/ajaxgrid.aspx";
        query = "sp_getCIFList~1~0~~~~0~~~" + uLoc;           
        
        this.ajax_grid_sel.open_selection_grid(page,query, identity_column, 0);
        
        epsco_client.isRemitterShow = true; 
    }
    
    this.load_grid_viewer = function(CIFClass)
    {   
        this.commons = new common_method();
        this.commons = null;   
        this.grid = new grid_viewer();    
        this.grid.session_end = true;
        
        if(CIFClass == 0)
        {
            var dateNow = new Date();
            
            document.getElementById("txtsearchLName2").value = "";
            document.getElementById("txtsearchFName2").value = "";
            document.getElementById("txtsearchMName2").value = "";
            document.getElementById("ddlEnrollStatus").selectedIndex = 0;
            //document.getElementById("txtEnrollDate1").value = epsco_client.getDateFormat(dateNow);
            //document.getElementById("txtEnrollDate2").value = epsco_client.getDateFormat(dateNow);
            
            this.grid.sql_source     = "sp_getCIFList~0~0~~~~0~" + document.getElementById("txtEnrollDate1").value + "~" + document.getElementById("txtEnrollDate2").value + "~" + uLoc;
            this.grid.div_container  = document.getElementById("grdEnrollees");
            
            epsco_client.approver_selected_id = null;
            
            document.getElementById("btnApproverGenerateCard").disabled = "disabled";
            
        }      
        if(CIFClass == 1)
        {
            this.grid.sql_source     = "sp_getCIFList~1~0~~~~0~~~" + uLoc;
            this.grid.div_container  = document.getElementById("grdRemitter");
        }
        if(CIFClass == 2)
        {
            this.grid.sql_source     = "sp_getCIFList~2~" + epsco_client.Rem_SEQID + "~~~~0~~~" + uLoc;
            this.grid.div_container  = document.getElementById("grdBeneficiary");
            
            document.getElementById("grdBeneficiary").style.display = "block";
            document.getElementById("btnDeleteEnrollee").disabled = "disabled";
            document.getElementById("btnSaveBene").disabled = "disabled";
        }
        
        this.grid.datarow_column = 0;
        this.grid.page_size      = 10;
        this.grid.open_grid_view();
        this.grid = null;
        
    }
    
    this.load_zipcode_search = function()
    {
        this.ajax_grid_sel = new grid_selector();
        
        identity_column = 0;
        page = "../AjxPage/ajaxgrid.aspx";
        query = "sp_cmcs_search_zipcode~" + document.getElementById("txtsearchZipCode").value;           
        
        this.ajax_grid_sel.open_selection_grid(page,query, identity_column, 0);
        
        epsco_client.isZipCodeShow = true; 
    }
    
    
    this.autoRemSearch = function(x)
    {
        if(x == 1)
        {
            var lname = document.getElementById("txtsearchLName1").value;
            var fname = document.getElementById("txtsearchFName1").value;
            var mname = document.getElementById("txtsearchMName1").value;
            
            this.grid = new grid_viewer();    
            this.grid.session_end = true; 
            this.grid.sql_source     = "sp_cmcs_search_remitter~" + lname + "~" + fname + "~" + mname + "~" + uLoc;
            this.grid.div_container  = document.getElementById("grdRemitter");
            this.grid.sql_source     = "sp_getCIFList~0~0~" + param + "~" + uLoc;
            this.grid.div_container  = document.getElementById("grdEnrollees");
            this.grid.datarow_column = 0;
            this.grid.page_size      = 10;
            this.grid.open_grid_view();
            this.grid = null;
        }
        else if(x == 2)
        {
            epsco_client.populate_data();
        }
    }
      
    this.do_ajaxgridview = function(parameter)
    {         
        this.ajax_grid = new grid_viewer();    
        this.ajax_grid.session_end = true; 
        this.ajax_grid.datarow_column = 0;
        this.ajax_grid.page_size      = 10;
                
        if (parameter != null && parameter != "undefined")
        {
            var command_param = parameter.split("$");
            
            if (command_param.length > 0)
            {
                if (command_param[0] == "ajx_selected")
                {
                    if(epsco_client.approverMode == true)
                    {
                        document.getElementById("btnApproverGenerateCard").disabled = "";
      
                        epsco_client.approver_selected_id = command_param[1];
                        epsco_client.getEnrolleeDetails(epsco_client.approver_selected_id);
                    }
                    else
                    {
                        if(epsco_client.isZipCodeShow == true)
                        {
                            var result = IRSWorld.EPSCO.EPSCOEnrollment.getZipCodeDetail(parseInt(command_param[1]),'').value.split('~');
                            
                            document.getElementById("txtBenefZipCode").value = result[0];
                            epsco_client.BenZipCode = result[1];

                            this.ajax_grid_sel.process_ajax(parameter);
                            epsco_client.isZipCodeShow = false; 
                        }
                        else //if(epsco_client.isRemitterShow == true)
                        {
                            epsco_client.Ben_EPSCO_SEQID = 0;
                            
                            if(epsco_client.CIFClass == 1)
                            {
                                epsco_client.Rem_SEQID = command_param[1];
                                document.getElementById("btnAddBene").disabled = "";
                                document.getElementById("btnSaveRem").disabled = "";
                            }
                            else
                            {
                                epsco_client.Ben_SEQID = command_param[1];
                                epsco_client.Ben_EPSCO_SEQID = epsco_client.dbgrid.getValueAt(epsco_client.dbgrid.row.selected + (epsco_client.dbgrid.curr_page - 1) * epsco_client.dbgrid.rows_page , 10);
                                
                            }
                            
                            epsco_client.getCIFDetails(command_param[1]);
                            
                            this.ajax_grid_sel.process_ajax(parameter);
                            epsco_client.isRemitterShow = false;
                        }
                    }
                }
                else if (command_param[0] == "ajx_paging")
                {        
                    if(epsco_client.isZipCodeShow == true || epsco_client.isRemitterShow == true)
                    {
                        this.ajax_grid_sel.process_ajax(parameter);
                    }
                    /*else if(document.getElementById("divSearchClient").style.display != "none")
                    {   
                        this.ajax_grid.sql_source     = "sp_getCIFList~1~0~~~~0~~~" + uLoc;
                        this.ajax_grid.div_container  = document.getElementById("grdRemitter");
                        this.ajax_grid.process_ajax(parameter);
                    }
                    else if(document.getElementById("divZipCodes").style.display != "none")
                    {
                        this.ajax_grid.sql_source     = "sp_cmcs_search_zipcode~" + document.getElementById("txtsearchZipCode").value;
                        this.ajax_grid.div_container  = document.getElementById("grdZipCodes");
                        this.ajax_grid.process_ajax(parameter);
                    }
                    */
                    else if(document.getElementById("divEnrollees").style.display != "none")
                    {
                        document.getElementById("btnApproverGenerateCard").disabled = "disabled";
                        
                        this.ajax_grid.sql_source     = "sp_getCIFList~0~0~~~~" + document.getElementById("ddlEnrollStatus").value + "~" + document.getElementById("txtEnrollDate1").value + "~" + document.getElementById("txtEnrollDate2").value + "~" + uLoc;
                        this.ajax_grid.div_container  = document.getElementById("grdEnrollees");
                        this.ajax_grid.process_ajax(parameter);
                    }
                    else
                    {     
                        this.ajax_grid.sql_source     = "sp_getCIFList~2~" + epsco_client.Rem_SEQID + "~~~~0~~~" + uLoc;
                        this.ajax_grid.div_container  = document.getElementById("grdBeneficiary");
                        this.ajax_grid.process_ajax(parameter);
                    }
                    
                }
                else if (command_param[0] == "ajx_search")
                {
                    this.ajax_grid_sel.process_ajax(parameter);
                }
            }
        }
    }
    
    this.getAcctBranchCode = function(acctno, e)
    {
          if(e.keyCode == 13)
          {
               ajax_show_status(true); 
               IRSWorld.EPSCO.EPSCOEnrollment.getAcctBranchCode(acctno, epsco_client.getAcctBranchCode_callback);
          }
    }

    this.getAcctBranchCode_callback = function(param)
    {
        if(param.value == "" || param.value == null)
        {
            alert("Account no. is invalid.");
            document.getElementById("ddlBenBranch").selectedIndex = 0;
        }
        else if(param.value.indexOf("Closed and Dormant") > -1)
        {
            alert(param.value);
            document.getElementById("ddlBenBranch").selectedIndex = 0;
        }
        else
        {
            epsco_client.getBranchName(param.value);
        }
        
        ajax_show_status(false); 
    }
    
    this.getCIFDetails = function(SEQ_ID)
    {
        IRSWorld.EPSCO.EPSCOEnrollment.getCIFDetails(SEQ_ID,epsco_client.CIFClass,epsco_client.Ben_EPSCO_SEQID,epsco_client.getCIFDetails_callback);
    }
    
    this.getCIFDetails_callback = function(param)
    {
        var dt = null;
        dt = param.value;
        
        if (dt != null && dt.Rows.length > 0)
        {
           
           var addr = dt.Rows[0].Address.split("/");
            
           if(epsco_client.CIFClass == 1)
           {
              setText(document.getElementById("txtHK_ID"),dt.Rows[0].HK_ID.toString());
              epsco_client.getEnrollingBranchName(dt.Rows[0].EnrollingBranch.toString());
              setText(document.getElementById("txtRemLastName"),dt.Rows[0].LastName.toString());
              setText(document.getElementById("txtRemFirstName"),dt.Rows[0].FirstName.toString());
              setText(document.getElementById("txtRemMiddleName"),dt.Rows[0].MiddleName.toString());
              setText(document.getElementById("txtRemBDate"),epsco_client.getDateFormat(dt.Rows[0].Birthdate).toString());
              if(document.getElementById("txtRemBDate").value == "1/1/1753")document.getElementById("txtRemBDate").value = "";
              if(addr.length>0){setText(document.getElementById("txtRemAdd1"),addr[0].toString());}
              if(addr.length>1){setText(document.getElementById("txtRemAdd2"),addr[1].toString());}
              if(addr.length>2){setText(document.getElementById("txtRemAdd3"),addr[2].toString());}
              if(addr.length>3){setText(document.getElementById("txtRemAdd4"),addr[3].toString());}
              if(addr.length>4){setText(document.getElementById("txtRemAdd5"),addr[4].toString());}
              if(addr.length>5){setText(document.getElementById("txtRemAdd6"),addr[5].toString());}
              setText(document.getElementById("txtRemAdd7"),dt.Rows[0].Region.toString());
              epsco_client.getCountryName(dt.Rows[0].Country.toString());
              setText(document.getElementById("txtRemZipCode"),dt.Rows[0].ZipCode.toString());
              setText(document.getElementById("txtRemEmail"),dt.Rows[0].Email.toString());
              setText(document.getElementById("txtRemTelNo"),dt.Rows[0].TelNo.toString());
              setText(document.getElementById("txtRemMobNo"),dt.Rows[0].MobNo.toString());
              
              document.getElementById("txtBenefLastName").value = "";
              document.getElementById("txtBenefFirstName").value = "";
              document.getElementById("txtBenefMiddleName").value = "";
              document.getElementById("txtBenefBDate").value = "";
              document.getElementById("txtBenefAdd1").value = "";
              document.getElementById("txtBenefAdd2").value = "";
              document.getElementById("txtBenefAdd3").value = "";
              document.getElementById("txtBenefZipCode").value = "";
              document.getElementById("txtBenefTelNo").value = "";
              document.getElementById("txtBenefMobNo").value = "";
              document.getElementById("txtBenefAcctNo").value = "";
              document.getElementById("ddlBenBranch").selectedIndex = 0;
              
              epsco_client.Ben_SEQID = null;
              
              epsco_client.CIFClass = 2;
              epsco_client.load_grid_viewer_data_custom();//epsco_client.load_grid_viewer(2);
           }
           else if(epsco_client.CIFClass == 2)
           {
              setText(document.getElementById("txtBenefLastName"),dt.Rows[0].LastName.toString());
              setText(document.getElementById("txtBenefFirstName"),dt.Rows[0].FirstName.toString());
              setText(document.getElementById("txtBenefMiddleName"),dt.Rows[0].MiddleName.toString());
              setText(document.getElementById("txtBenefBDate"),epsco_client.getDateFormat(dt.Rows[0].Birthdate));
              if(document.getElementById("txtBenefBDate").value == "1/1/1753")document.getElementById("txtBenefBDate").value = "";
              if(addr.length>0){setText(document.getElementById("txtBenefAdd1"),addr[0].toString());}
              if(addr.length>1){setText(document.getElementById("txtBenefAdd2"),addr[1].toString());}
              if(addr.length>2){setText(document.getElementById("txtBenefAdd3"),addr[2].toString());}
              
              try
              {
                var result = IRSWorld.EPSCO.EPSCOEnrollment.getZipCodeDetail('0',dt.Rows[0].ZipCode.toString()).value.split('~');
                setText(document.getElementById("txtBenefZipCode"),result[0]);
                epsco_client.BenZipCode = result[1];
              }
              catch(e){epsco_client.BenZipCode = '';}

              setText(document.getElementById("txtBenefTelNo"),dt.Rows[0].TelNo.toString());
              setText(document.getElementById("txtBenefMobNo"),dt.Rows[0].MobNo.toString());
              setText(document.getElementById("txtBenefAcctNo"),dt.Rows[0].AcctNo.toString());
              epsco_client.getBranchName(dt.Rows[0].BranchCode.toString());
              
              epsco_client.enrollStatus = parseInt(IRSWorld.EPSCO.EPSCOEnrollment.getEnrollStatus(epsco_client.Rem_SEQID,epsco_client.Ben_SEQID).value);
              
              if(epsco_client.enrollStatus == 0)//Not Enrolled - No Card
              {
                document.getElementById("btnAddBene").disabled = "";
                document.getElementById("btnDeleteEnrollee").disabled = "disabled";
              }
              
              else if(epsco_client.enrollStatus == 1)//Enrolled - Card 
              {
                document.getElementById("btnAddBene").disabled = "disabled";
                document.getElementById("btnDeleteEnrollee").disabled = "disabled";
              }
              else if(epsco_client.enrollStatus == 2) //Enrolled - No Card 
              {
                document.getElementById("btnAddBene").disabled = "disabled";
                document.getElementById("btnDeleteEnrollee").disabled = "";
              }
              
              document.getElementById("btnSaveBene").disabled="";
           }
        }
        
        hidePanelEP('divSearchClient');
        
    }
    
    this.getEnrolleeDetails = function(ben_seqid)
    {
        IRSWorld.EPSCO.EPSCOEnrollment.getEnrolleeDetails(ben_seqid, epsco_client.getEnrolleeDetails_callback);
    }
    
    this.getEnrolleeDetails_callback = function(param)
    {
        var dt = null;
        dt = param.value;
        
        if (dt != null && dt.Rows.length > 0)
        {
            document.getElementById("txtsearchLName2").value = dt.Rows[0].Ben_LastName.toString();
            document.getElementById("txtsearchFName2").value = dt.Rows[0].Ben_FirstName.toString();
            document.getElementById("txtsearchMName2").value = dt.Rows[0].Ben_MiddleName.toString();
            epsco_client.BenLName = dt.Rows[0].Ben_LastName.toString();
            epsco_client.BenFName = dt.Rows[0].Ben_FirstName.toString();
            epsco_client.BenMName = dt.Rows[0].Ben_MiddleName.toString();
            epsco_client.BenAcctNo = dt.Rows[0].AcctNo.toString();
            epsco_client.RemLName = dt.Rows[0].Rem_LastName.toString();
            epsco_client.RemFName = dt.Rows[0].Rem_FirstName.toString();
            epsco_client.RemMName = dt.Rows[0].Rem_MiddleName.toString();    
        }
        else
        {
            epsco_client.BenLName = null;
            epsco_client.BenFName = null;
            epsco_client.BenMName = null;
            epsco_client.BenAcctNo = null;
            epsco_client.RemLName = null;
            epsco_client.RemFName = null;
            epsco_client.RemMName = null;
        }
    }
    
      
    this.getDateFormat = function(dateobj)
    {
        var month = dateobj.getMonth() + 1;
        var day = dateobj.getDate();
        var year = dateobj.getFullYear();
        
        var bdate = month + "/" + day + "/" + year;
        
        if(bdate == "1/1/1900"){bdate = "";}

        return bdate;
        
    }   
    
    this.getCountryName = function(code)
    {
        var ddl = document.getElementById("ddlRemCountry").getElementsByTagName('option');

        for(i=0;i<ddl.length;i++)
        {
            if(ddl[i].value == code)
            { 
                document.getElementById("ddlRemCountry").selectedIndex = i;
            }
        }   
    }
    
    this.getEnrollingBranchName = function(code)
    {
        
        document.getElementById("ddlEnrollingBranch").selectedIndex = 0;
        
        var ddl = document.getElementById("ddlEnrollingBranch").getElementsByTagName('option');

        for(i=0;i<ddl.length;i++)
        {
            if(ddl[i].value == code)
            { 
                document.getElementById("ddlEnrollingBranch").selectedIndex = i;
            }
        }
    }
    
    this.getBranchName = function(code)
    {
        var ddl = document.getElementById("ddlBenBranch").getElementsByTagName('option');

        for(i=0;i<ddl.length;i++)
        {
            if(code.length >=4)
            {
                code = "1" +  code.substring(1,4);
            }
        
            if(ddl[i].value == code)
            { 
                document.getElementById("ddlBenBranch").selectedIndex = i;
            }
        }   
    }
   
    this.setClientInfo = function(CIFClass)
    {
        if(CIFClass == 1 || CIFClass == 0)
        {
            this.HK_ID = document.getElementById("txtHK_ID").value;
            this.EnrollingBranch = document.getElementById("ddlEnrollingBranch").value;
            this.RemLastName = document.getElementById("txtRemLastName").value;
            this.RemFirstName = document.getElementById("txtRemFirstName").value;
            this.RemMiddleName = document.getElementById("txtRemMiddleName").value;
            if(document.getElementById("txtRemBDate").value.replace(" ","") == "")
            {
                this.RemBirthDate = "01/01/1753";
            }
            else
            {
                this.RemBirthDate = document.getElementById("txtRemBDate").value;
            }
            this.RemRegion = document.getElementById("txtRemAdd7").value;
            if(document.getElementById("ddlRemCountry").selectedIndex == 0)
            {
                this.RemCountry = "0";
            }
            else
            {
                this.RemCountry = document.getElementById("ddlRemCountry").value;
            }
            this.RemZipCode = document.getElementById("txtRemZipCode").value;
            this.RemEmail = document.getElementById("txtRemEmail").value;
            this.RemTelNo = document.getElementById("txtRemTelNo").value;
            this.RemMobNo = document.getElementById("txtRemMobNo").value;
        }
        if(CIFClass == 2 || CIFClass == 0)
        {
            this.BenLastName = document.getElementById("txtBenefLastName").value;
            this.BenFirstName = document.getElementById("txtBenefFirstName").value;
            this.BenMiddleName = document.getElementById("txtBenefMiddleName").value;
            if(document.getElementById("txtBenefBDate").value.replace(" ","") == "")
            {
                this.BenBirthDate = "01/01/1753";
            }
            else
            {
                this.BenBirthDate = document.getElementById("txtBenefBDate").value;
            }
            //this.BenZipCode = document.getElementById("txtBenefZipCode").value;
            this.BenTelNo = document.getElementById("txtBenefTelNo").value;
            this.BenMobNo = document.getElementById("txtBenefMobNo").value;
            this.BenAcctNo = document.getElementById("txtBenefAcctNo").value;
            this.BenBranchCode = document.getElementById("ddlBenBranch").value;
        }
    }

    this.setAddress = function(CIFClass)
    {
        if(CIFClass == 1 || CIFClass == 0)
        {
            var rem_addr =  document.getElementById("txtRemAdd1").value
                        + '/' + document.getElementById("txtRemAdd2").value
                        + '/' + document.getElementById("txtRemAdd3").value
                        + '/' + document.getElementById("txtRemAdd4").value
                        + '/' + document.getElementById("txtRemAdd5").value
                        + '/' + document.getElementById("txtRemAdd6").value;
                        
             epsco_client.RemAddress = rem_addr;
        }
        if(CIFClass == 2 || CIFClass == 0)
        {           
            var ben_addr = document.getElementById("txtBenefAdd1").value
                        + '/' + document.getElementById("txtBenefAdd2").value
                        + '/' + document.getElementById("txtBenefAdd3").value;
                        
            epsco_client.BenAddress = ben_addr;
        }   
    }
    
    this.setAge = function(CIFClass)
    {
        if(CIFClass == 1 || CIFClass == 0)
        {
            if(document.getElementById("txtRemBDate").value.replace(" ","") == "")
            {
                epsco_client.RemAge = 0;
            }
            else
            {
                epsco_client.RemAge = epsco_client.generic.compute_age(document.getElementById("txtRemBDate").value);
            }
        }
        if(CIFClass == 2 || CIFClass == 0)
        {
            if(document.getElementById("txtBenefBDate").value.replace(" ","") == "")
            {
                epsco_client.BenAge = 0;
            }
            else
            {
                epsco_client.BenAge = epsco_client.generic.compute_age(document.getElementById("txtBenefBDate").value);
            }
        }
    } 
     
    this.construct_insert_record = function(CIFClass, method)
    {
            var msg = "";
            
            if(method=="ADD")
            {
                msg = "Are you sure to enroll client record?\n\n";
                msg += "Remitter Name:\t" + epsco_client.RemFirstName + ' ' + epsco_client.RemMiddleName + ' ' + epsco_client.RemLastName + "\n";
                msg += "Beneficiary Name:\t" + epsco_client.BenFirstName + ' ' + epsco_client.BenMiddleName + ' ' + epsco_client.BenLastName + "\n";
                msg += "Account Number:\t" + epsco_client.BenAcctNo + "\n\n";
                msg += "Click OK if details above is correct, otherwise click CANCEL."
            } 
            if(method=="UPDATE"){msg = "Are you sure to update client record?";}
            
            var x = window.confirm(msg);
            
            if(x)
            {
                var dRow = new Array();
                
                if(CIFClass == 1 || CIFClass == 0)
                {
                    dRow[dRow.length] = epsco_client.RemLastName;
	                dRow[dRow.length] = epsco_client.RemFirstName;
	                dRow[dRow.length] = epsco_client.RemMiddleName;
	                dRow[dRow.length] = epsco_client.RemBirthDate;
	                dRow[dRow.length] = epsco_client.RemAge;
	                dRow[dRow.length] = document.getElementById("txtRemAdd1").value;
	                dRow[dRow.length] = document.getElementById("txtRemAdd2").value;
	                dRow[dRow.length] = document.getElementById("txtRemAdd3").value;
	                dRow[dRow.length] = document.getElementById("txtRemAdd4").value;
	                dRow[dRow.length] = document.getElementById("txtRemAdd5").value;
	                dRow[dRow.length] = epsco_client.RemAddress;
	                dRow[dRow.length] = epsco_client.RemRegion;
	                dRow[dRow.length] = epsco_client.RemCountry;
	                dRow[dRow.length] = epsco_client.RemZipCode;
	                dRow[dRow.length] = epsco_client.RemEmail;
	                dRow[dRow.length] = epsco_client.RemTelNo;
	                dRow[dRow.length] = epsco_client.RemMobNo;
	                if(CIFClass != 0){dRow[dRow.length] = '';}
	                dRow[dRow.length] = epsco_client.RemBranchCode;
	            }
	            if(CIFClass == 2 || CIFClass == 0)
	            {
	                dRow[dRow.length] = epsco_client.BenLastName;
	                dRow[dRow.length] = epsco_client.BenFirstName;
	                dRow[dRow.length] = epsco_client.BenMiddleName;
	                dRow[dRow.length] = epsco_client.BenBirthDate;
	                dRow[dRow.length] = epsco_client.BenAge;
	                dRow[dRow.length] = document.getElementById("txtBenefAdd1").value;
	                dRow[dRow.length] = document.getElementById("txtBenefAdd2").value;
	                dRow[dRow.length] = document.getElementById("txtBenefAdd3").value;
	                if(CIFClass != 0){dRow[dRow.length] = '';}
	                if(CIFClass != 0){dRow[dRow.length] = '';}
	                dRow[dRow.length] = epsco_client.BenAddress;
	                if(CIFClass != 0){dRow[dRow.length] = '';}
	                if(CIFClass != 0){dRow[dRow.length] = '0';}
	                dRow[dRow.length] = epsco_client.BenZipCode;
	                if(CIFClass != 0){dRow[dRow.length] = '';}
	                dRow[dRow.length] = epsco_client.BenTelNo;
	                dRow[dRow.length] = epsco_client.BenMobNo; 
	                dRow[dRow.length] = epsco_client.BenAcctNo;
	                dRow[dRow.length] = epsco_client.BenBranchCode;
	            }
	            
	            if(method == "UPDATE" && CIFClass == 2)
	            {
	                dRow[dRow.length] = '';
	                dRow[dRow.length] = '';
	            }
	            else
	            {   
	               dRow[dRow.length] = epsco_client.EnrollingBranch;
	               dRow[dRow.length] = epsco_client.HK_ID;
	            }
                
                dRow[dRow.length] = uLoc;
                
                if(method == "UPDATE")
                {
                    if(CIFClass == 1){dRow[dRow.length] = epsco_client.Rem_SEQID;}
                    if(CIFClass == 2){dRow[dRow.length] = epsco_client.Ben_SEQID;}
                }
                else
                {
                    dRow[dRow.length] = epsco_client.Rem_SEQID;
                    dRow[dRow.length] = epsco_client.Ben_SEQID;
                }
                
                dRow[dRow.length] = uID;
                
                if(epsco_client.Ben_SEQID != null && epsco_client.Ben_SEQID != 0)
                {
                    dRow[dRow.length] = epsco_client.Ben_EPSCO_SEQID;
                }
                else
                {
                    dRow[dRow.length] = 0;
                }
                ajax_show_status(true);
                
                IRSWorld.EPSCO.EPSCOEnrollment.enroll_Client(dRow.join("~"), method, epsco_client.construct_insert_record_callback);
           }
	 }
	 
	 this.construct_insert_record_callback = function(msg)
     {
        ajax_show_status(false);
        
        alert(msg.value);
        
        if(!msg.value.match("^Error"))
        {
            if(epsco_client.Rem_SEQID != null && epsco_client.Rem_SEQID != "0" && epsco_client.Rem_SEQID != ""  && epsco_client.Rem_SEQID != "null")
            {
                if(epsco_client.enrollStatus == 0)
                {
                    epsco_client.clearfields(2);
                    document.getElementById("btnAddBene").disabled = "";
                }
                else
                {
                    document.getElementById("btnAddBene").disabled = "disabled";
                }
                
                 epsco_client.load_grid_viewer_data_custom();//epsco_client.load_grid_viewer(2);
            }
            else
            {
                clearForm("divEnrollCIF");
            }
        }
        
        epsco_client.Ben_SEQID = null;
        epsco_client.enrollStatus = 0;
     }
     
     this.delete_client = function()
     {
        if(epsco_client.Rem_SEQID == null || epsco_client.Ben_SEQID == null || epsco_client.Rem_SEQID == "0" || epsco_client.Ben_SEQID == "0")
        {
            alert("Please select beneficiary.");
        }
        else
        {
            var msg = "Are you sure to delete client record?";
                
            var x = window.confirm(msg);
                
            if(x)
            {
                alert(IRSWorld.EPSCO.EPSCOEnrollment.delete_Client(epsco_client.Rem_SEQID,epsco_client.Ben_SEQID).value);
                
                epsco_client.load_grid_viewer_data_custom();//epsco_client.load_grid_viewer(2);
                epsco_client.clearfields(2);

                document.getElementById("btnAddBene").disabled = "";
                
                epsco_client.Ben_SEQID = null;
                epsco_client.enrollStatus = 0;
                
            }
         }  
     }

     this.approver_generate_reference_no = function()
     {
        if(epsco_client.PrintOnly == true)
        {
            epsco_client.generate_card_details(epsco_client.approver_selected_id);
        }
        else
        {
            if(epsco_client.approver_selected_id == null)
            {
                alert("Please select enrollee.")
            }
            else
            {
                var msg = "Are you sure to generate client's card no.?";
                
                var x = window.confirm(msg);
                
                if(x)
                {
                    var result = IRSWorld.EPSCO.EPSCOEnrollment.app_generate_RefNo(epsco_client.approver_selected_id);
                    
                    if(result == "null")
                    {
                        alert(result.value);
                    }
                    else
                    {
                        epsco_client.generate_card_details(epsco_client.approver_selected_id);
                        
                        document.getElementById("txtsearchLName2").value = "";
                        document.getElementById("txtsearchFName2").value = "";
                        document.getElementById("txtsearchMName2").value = "";
                        
                        epsco_client.RemLName = null;
                        epsco_client.RemFName = null;
                        epsco_client.RemMName = null;
                        epsco_client.BenLName = null;
                        epsco_client.BenFName = null;
                        epsco_client.BenMName = null;
                        epsco_client.BenAcctNo = null;
                        
                        epsco_client.autoRemSearch(2);
                    }
                }
             }
         }
     }
	 
	 this.generate_card_details = function(seqid)
	 {
	    var refno = IRSWorld.EPSCO.EPSCOEnrollment.getEPSCORefNo(seqid).value;
	    var acctno = epsco_client.BenAcctNo;
	    var rem_name = epsco_client.RemFName + " " + epsco_client.RemMName + " " + epsco_client.RemLName;
	    var ben_name = epsco_client.BenFName + " " + epsco_client.BenMName + " " + epsco_client.BenLName;         
	    
	    if(!refno)
	    {
	        document.getElementById("lblEPSCORefNo").value = "REFERENCE NO. NOT FOUND";
	    }
	    else
	    {
	        document.getElementById("lblEPSCORefNo").value = refno;
	        document.getElementById("lblEPSCORefNoCode").value = '*' + refno + '*';
	    }               
	    
	    document.getElementById("lblRemName").value = rem_name.toUpperCase();
	    document.getElementById("lblBenName").value = ben_name.toUpperCase();
	    //document.getElementById("lblBenAcctNo").value = 'A/C NO. ' + acctno;
	    
	    showPanelEP('divRemCard');
	 }
	 
    //validation
	 
	this.validate_input = function(CIFClass)
    {   
        var error_input = '';
        var foc_elem     = '';
        
		if(CIFClass == 1 || CIFClass == 0)
		{
		    if(epsco_client.generic.is_empty(document.getElementById("txtHK_ID").value) == true && CIFClass == 0)
		    {
		        error_input += "Please enter client's HK ID.\n"; 
                if (foc_elem.length == 0) foc_elem = 'txtHK_ID';
		    }
		    else if(epsco_client.is_HKID(document.getElementById("txtHK_ID").value) == false)
		    {
		        error_input += "Invalid HK ID format.\n"; 
                if (foc_elem.length == 0) foc_elem = 'txtHK_ID';
		    }
		    if(document.getElementById("ddlEnrollingBranch").selectedIndex == 0 && CIFClass == 0)
		    {
		        if(this.isFound("BranchCode", 1))
		        {
		            error_input += "Please enter client's enrolling branch.\n"; 
                    if (foc_elem.length == 0) foc_elem = 'ddlEnrollingBranch';
                }  
		    }
		    if(epsco_client.generic.is_empty(document.getElementById("txtRemLastName").value) == true)
		    {
		        if(this.isFound("LastName", 1))
		        {
		            error_input += "Please enter remitter's last name.\n"; 
                    if (foc_elem.length == 0) foc_elem = 'txtRemLastName';
                }  
		    } 
		    if (epsco_client.generic.is_alpha(document.getElementById("txtRemLastName").value) == false)
            {       
                error_input += "Last name contains invalid character(s).\n"; 
                if (foc_elem.length == 0) foc_elem = 'txtRemLastName';      
            }
            if (epsco_client.generic.is_empty(document.getElementById("txtRemFirstName").value) == true)
            {   
                if(this.isFound("FirstName", 1))
                {        
                    error_input += "Please enter remitter's first name.\n"; 
                    if (foc_elem.length == 0) foc_elem = 'txtRemFirstName';
                }            
            }
            if (epsco_client.generic.is_alpha(document.getElementById("txtRemFirstName").value) == false)
            {        
                error_input += "First name contains invalid character(s).\n"; 
                if (foc_elem.length == 0) foc_elem = 'txtRemFirstName';        
            }
            if (epsco_client.generic.is_empty(document.getElementById("txtRemMiddleName").value) == true)
            {   
                if(this.isFound("MiddleName", 1))
                {        
                    error_input += "Please enter remitter's middle name.\n"; 
                    if (foc_elem.length == 0) foc_elem = 'txtRemMiddleName'; 
                }           
            }
            if (epsco_client.generic.is_alpha(document.getElementById("txtRemMiddleName").value) == false)
            {
                error_input += "Middle name contains invalid character(s).\n"; 
                if (foc_elem.length == 0) foc_elem = 'txtRemMiddleName';           
            }
            if(document.getElementById("txtRemBDate").value == "")
            {
                if(this.isFound("BirthDate", 1))
                {
                    error_input += "Please enter remitter's birthdate.\n";    
                    if (foc_elem.length == 0) foc_elem = 'txtRemBDate';
                }
            }		   	
		    if (epsco_client.generic.to_validate_bdate(document.getElementById("txtRemBDate").value) == false)
            {           
                error_input += "Invalid birth date.\n";    
                if (foc_elem.length == 0) foc_elem = 'txtRemBDate';       
            }
            if (epsco_client.generic.is_empty(document.getElementById("txtRemZipCode").value) == true)
            {   
                if(this.isFound("ZipCodeSeqId", 1))
                {        
                    error_input += "Please enter remitter's zip code.\n"; 
                    if (foc_elem.length == 0) foc_elem = 'txtRemZipCode';
                }             
            }
            if (epsco_client.generic.is_empty(document.getElementById("txtRemAdd1").value) == true)
            {   
                if(this.isFound("Address1", 1))
                {        
                    error_input += "Please enter remitter's room no./bldg.\n"; 
                    if (foc_elem.length == 0) foc_elem = 'txtRemAdd1';
                }             
            }
            if (epsco_client.generic.is_empty(document.getElementById("txtRemAdd2").value) == true)
            {   
                if(this.isFound("Address2", 1))
                {        
                    error_input += "Please enter remitter's no./street.\n"; 
                    if (foc_elem.length == 0) foc_elem = 'txtRemAdd2';
                }            
            }
            if(epsco_client.generic.is_empty(document.getElementById("txtRemEmail").value) == true)
            {
                if(this.isFound("EmailAddress", 1))
                {
                    error_input += "Please enter remitter's e-mail address.\n"; 
                    if (foc_elem.length == 0) foc_elem = 'txtRemEmail';
                }
            }
		    if(epsco_client.generic.to_validate_email(document.getElementById("txtRemEmail").value) == false)
		    {
			    error_input += "Invalid email address.\n";
			    if (foc_elem.length == 0) foc_elem = 'txtRemEmail'; 
		    }
		    if (epsco_client.generic.is_empty(document.getElementById("txtRemTelNo").value) == true && epsco_client.generic.is_empty(document.getElementById("txtRemMobNo").value) == true)
            {   
                if(this.isFound("ContactNo", 1))
                {       
                    error_input += "System requires at least 1 remitter's contact number.\n"; 
                    if (foc_elem.length == 0) foc_elem = 'txtRemTelNo';
                }            
            }            
		}
		if(CIFClass == 2 || CIFClass == 0)
		{
		    if (epsco_client.generic.is_empty(document.getElementById("txtBenefLastName").value) == true)
            {   
                if(this.isFound("LastName", 2))
                {        
                    error_input += "Please enter beneficiary's last name.\n"; 
                    if (foc_elem.length == 0) foc_elem = 'txtBenefLastName';
                }            
            }
		    if (epsco_client.generic.is_alpha(document.getElementById("txtBenefLastName").value) == false)
            {           
                error_input += "Last name contains invalid character(s).\n"; 
                if (foc_elem.length == 0) foc_elem = 'txtBenefLastName';            
            }
            if (epsco_client.generic.is_empty(document.getElementById("txtBenefFirstName").value) == true)
            {   
                if(this.isFound("FirstName", 2))
                {        
                    error_input += "Please enter beneficiary's first name.\n"; 
                    if (foc_elem.length == 0) foc_elem = 'txtBenefFirstName';
                }            
            }
            if (epsco_client.generic.is_alpha(document.getElementById("txtBenefFirstName").value) == false)
            {           
                error_input += "First name contains invalid character(s).\n"; 
                if (foc_elem.length == 0) foc_elem = 'txtBenefFirstName';            
            }
            if (epsco_client.generic.is_empty(document.getElementById("txtBenefMiddleName").value) == true)
            {   
                if(this.isFound("MiddleName", 2))
                {        
                    error_input += "Please enter beneficiary's middle name.\n"; 
                    if (foc_elem.length == 0) foc_elem = 'txtBenefMiddleName';
                }            
            }
            if (epsco_client.generic.is_alpha(document.getElementById("txtBenefMiddleName").value) == false)
            {           
                error_input += "Middle name contains invalid character(s).\n"; 
                if (foc_elem.length == 0) foc_elem = 'txtBenefMiddleName';            
            }
            if (document.getElementById("txtBenefBDate").value == "")
            {
                if(this.isFound("BirthDate", 2))
                {
                    error_input += "Invalid birth date.\n";    
                    if (foc_elem.length == 0) foc_elem = 'txtBenefBDate';
                }   
            }
            if (epsco_client.generic.to_validate_bdate(document.getElementById("txtBenefBDate").value) == false)
            {           
                error_input += "Invalid beneficiary's birth date.\n";    
                if (foc_elem.length == 0) foc_elem = 'txtBenefBDate';      
            }
		    if (epsco_client.generic.is_empty(document.getElementById("txtBenefAcctNo").value) == true)
            {           
                error_input += "Please enter account no.\n"; 
                if (foc_elem.length == 0) foc_elem = 'txtBenefAcctNo';            
            }
            if(document.getElementById("ddlBenBranch").value == '')
		    {
			    error_input += "Beneficiary's branch location not supplied.\n";
                if (foc_elem.length == 0) foc_elem = "ddlBenBranch";
		    }
		    if (epsco_client.generic.is_empty(document.getElementById("txtBenefZipCode").value) == true)
            {   
                if(this.isFound("ZipCodeSeqId", 2))
                {        
                    error_input += "Please enter beneficiary's zip code.\n"; 
                    if (foc_elem.length == 0) foc_elem = 'txtBenefZipCode';
                }             
            }
		    if (epsco_client.generic.is_empty(document.getElementById("txtBenefAdd1").value) == true)
            {   
                if(this.isFound("Address1", 2))
                {        
                    error_input += "Please enter beneficiary's room no./bldg.\n"; 
                    if (foc_elem.length == 0) foc_elem = 'txtBenefAdd1';
                }            
            }
            if (epsco_client.generic.is_empty(document.getElementById("txtBenefAdd2").value) == true)
            {           
                if(this.isFound("Address2", 2))
                {
                    error_input += "Please enter beneficiary's no./street.\n"; 
                    if (foc_elem.length == 0) foc_elem = 'txtBenefAdd2';
                }            
            }
            if (epsco_client.generic.is_empty(document.getElementById("txtBenefTelNo").value) == true && epsco_client.generic.is_empty(document.getElementById("txtBenefMobNo").value) == true)
            {   
                if(this.isFound("ContactNo", 2))
                {        
                    error_input += "System requires at least 1 beneficiary's contact number.\n"; 
                    if (foc_elem.length == 0) foc_elem = 'txtBenefTelNo';
                }            
            }
		}
        if (foc_elem.length > 0)
        {
            try
            {
                document.getElementById(foc_elem).focus();
                if(foc_elem.substring(0,3) == 'txt'){document.getElementById(foc_elem).select();}
            }
            catch(e){}
        }
        if (error_input != '')
        {
            alert(error_input);
            return false;
        }
        
        return true;
    } 
	
    this.check_acct_no = function(acct_num, method)
	{
	     IRSWorld.EPSCO.EPSCOEnrollment.check_BeneAcctNo(acct_num, method, epsco_client.check_acct_no_callback);
	}
	 
	this.check_acct_no_callback = function(msg)
	{
	    var result = msg.value.toString().split('~');

	    if(result[0] != "")
	    {
	       alert("Invalid account number");
	       document.getElementById("ddlBenBranch").selectedIndex = 0;
	    }
	    else
	    {
	        if(result[1] == "ADD")
	        {
	            epsco_client.setClientInfo(0);
                epsco_client.setAddress(0);
                epsco_client.setAge(0);
                epsco_client.construct_insert_record(0, result[1]);
	        }
	        else
	        {
	            epsco_client.setClientInfo(2);
                epsco_client.setAddress(2);
                epsco_client.setAge(2);
                epsco_client.construct_insert_record(2, result[1]);
            }
	    }
	}
	
	this.clearfields = function(x)
	{
	    if(x == 1)
	    {
	        document.getElementById("txtRemLastName").value = "";
	        document.getElementById("txtRemFirstName").value = "";
            document.getElementById("txtRemMiddleName").value = "";
            document.getElementById("txtRemBDate").value = "";
            document.getElementById("txtRemAdd1").value = "";
            document.getElementById("txtRemAdd2").value = "";
            document.getElementById("txtRemAdd3").value = "";
            document.getElementById("txtRemAdd4").value = "";
            document.getElementById("txtRemAdd5").value = "";
            document.getElementById("txtRemAdd6").value = "";
            document.getElementById("txtRemAdd7").value = "";
            document.getElementById("txtRemZipCode").value = "";
            document.getElementById("txtRemEmail").value = "";
            document.getElementById("txtRemTelNo").value = "";
            document.getElementById("txtRemMobNo").value = "";
            document.getElementById("ddlRemCountry").selectedIndex = 0;
	    }
	    else
	    {
            document.getElementById("txtBenefLastName").value = "";
            document.getElementById("txtBenefFirstName").value = "";
            document.getElementById("txtBenefMiddleName").value = "";
            document.getElementById("txtBenefBDate").value = "";
            document.getElementById("txtBenefAdd1").value = "";
            document.getElementById("txtBenefAdd2").value = "";
            document.getElementById("txtBenefAdd3").value = "";
            document.getElementById("txtBenefZipCode").value = "";
            document.getElementById("txtBenefTelNo").value = "";
            document.getElementById("txtBenefMobNo").value = "";
            document.getElementById("txtBenefAcctNo").value = "";
            document.getElementById("ddlBenBranch").selectedIndex = 0;
         }
	} 
	
	this.is_HKID = function(elem)
	{
	    var regexps = /^[A-z]\w{0,1}[\d]{6}\(\w\)$/;
      
        return regexps.test(elem);
	}
	
	this.formatHKID = function(n, e)
	{
	   if(e.shiftKey) return false;
	   if(e.keyCode == 9) return true; //tab
	   
	   chr = (e.keyCode >= 96 && e.keyCode <= 105) ? String.fromCharCode(e.keyCode - 48) : String.fromCharCode(e.keyCode);
	   
	   if(epsco_client.getCaretPos(n) >= 10 && e.keyCode == 8)// trim parenthesis
	   {
	        var x = /\(\w\)$/;
	        
	        if(x.test(n.value)) 
	        {
	            n.value = n.value.replace("(","");
	            n.value = n.value.replace(")","");
	            return true;
	        }     
	   }
	   
	   if(e.keyCode == 8 || e.keyCode == 37 || e.keyCode == 39) return true;
	   
	   if(n.value.length >= 7 && n.value.length < 11 && epsco_client.getCaretPos(n) >= 7)
	   {
	        n.value = n.value.replace("(","");
	        n.value = n.value.replace(")","");
	        n.value = n.value + "(" + chr + ")";
	        return false;
	   }
	   
	   return true;
	   
	}
	
	this.getCaretPos = function(input)
	{
	    input.focus();
        var sel = document.selection.createRange();
        sel.moveStart('character', -input.value.length);
       
        return sel.text.length;
	}
	
	

	this.populate_data = function()
	{
	    
	    var param = document.getElementById("txtsearchLName2").value + "~" + 
                    document.getElementById("txtsearchFName2").value + "~" +
                    document.getElementById("txtsearchMName2").value + "~" + 
                    document.getElementById("ddlEnrollStatus").value + "~" + 
                    document.getElementById("txtEnrollDate1").value + "~" +
                    document.getElementById("txtEnrollDate2").value;
        
        ajax_show_status(true);            
	    IRSWorld.EPSCO.EPSCOEnrollment.getDataTableValue("sp_getCIFList~0~0~" + param + "~" + uLoc, epsco_client.load_custom_grid);
	}
	
	this.populate_data_callback = function(args)
	{
	    epsco_client.load_custom_grid(args.value);
	}
	
	this.load_custom_grid = function(dt)
	{
	
	    ajax_show_status(false);
	    
        epsco_client.dbgrid = new datagrid();
        
        var opt = document.getElementById('ddlEnrollStatus').selectedIndex;
        
        if(opt == 0)
        {
            var cols = {1:'selected'
                    ,2:'Product Card No.'
                    ,3:'Remitter Fullname'
                    ,4:'Beneficiary Fullname'
                    ,5:'Beneficiary Account No.'
                    ,6:'Enrolling Branch'
                    ,7:'Mobile No.'
                    ,8:'Date Created'
                    ,9:'SEQ_ID'};
                    
            document.getElementById('lbldtfrom').innerHTML = 'Date From:';
            document.getElementById('btnExportAll').disabled = 'disabled';
            document.getElementById('btnApproverGenerateCard').value = 'Generate Card';
        }
        else
        {
            var cols = {1:'selected'
                    ,2:'Product Card No.'
                    ,3:'Remitter Fullname'
                    ,4:'Beneficiary Fullname'
                    ,5:'Beneficiary Account No.'
                    ,6:'Enrolling Branch'
                    ,7:'SEQ_ID'
                    ,8:'Barcode No.'
                    ,9:'Mobile No.'};
                    
            document.getElementById('lbldtfrom').innerHTML = 'Date Approved From:';
            document.getElementById('btnExportAll').disabled = '';
            document.getElementById('btnApproverGenerateCard').value = (opt == 1 ? 'Re-Export' : 'Export');
        }

        for(var x in cols)
        {
            epsco_client.dbgrid.addColumn(epsco_client.create_column(cols[x],'left',(cols[x] == 'SEQ_ID' ? 'none' : '')));
        }
         
        epsco_client.dbgrid.rows_page = 10;
        epsco_client.dbgrid.bind(dt.value, 'tdEnrollees', 'grdEnrollees');
        
        if(epsco_client.dbgrid.datatable.Rows.length == 0)
        {
            document.getElementById('btnApproverGenerateCard').disabled = 'disabled';
            document.getElementById('btnExportAll').disabled = 'disabled';
        }
        else
        {
            document.getElementById('btnApproverGenerateCard').disabled = '';
            if(opt != 0) document.getElementById('btnExportAll').disabled = '';
        }
        
        epsco_client.enabled_checkbox('grdEnrollees_tbody');
        
        document.getElementById('grdEnrollees_pg_next').onclick = function(){epsco_client.dbgrid.pg_next_clicked();epsco_client.enabled_checkbox('grdEnrollees_tbody');};
        document.getElementById('grdEnrollees_pg_prev').onclick = function(){epsco_client.dbgrid.pg_previous_clicked();epsco_client.enabled_checkbox('grdEnrollees_tbody');};
        document.getElementById('grdEnrollees_pg_select').onclick = function(){epsco_client.dbgrid.pg_select_clicked();epsco_client.enabled_checkbox('grdEnrollees_tbody');};
        
	}
	
    this.create_column = function(name, align, display)
	{
	    var col = new datagrid_column();
	    
	    col.header = (name == 'selected' ? " &nbsp; <br /><input type='checkbox' id='chkAll' style='background-color:transparent;border:none;width:30px;' onclick=javascript:epsco_client.check_all(this.checked); />" : name);
    	col.datafield = name;
    	col.align = align;
    	col.styles.cell += (name == 'selected' ? 'width:30px;' : 'width:auto;');
    	col.styles.header += 'display:' + display + ";";
    	col.styles.datafield += 'display:' + display + ";";
    	col.styles.cell += 'display:' + display + ";";
    	col.styles.cell += 'cursor:pointer;'
    	col.styles.datafield += 'cursor:pointer;';
    	if(name == 'selected') col.type= 'check';
    	
    	return col;
	}
	
	this.load_grid_viewer_data_custom = function()
	{
	    IRSWorld.EPSCO.EPSCOEnrollment.getDataTableValue("sp_getCIFList~2~" + epsco_client.Rem_SEQID + "~~~~0~~~" + uLoc, epsco_client.load_grid_viewer_custom);
	}
	
	this.load_grid_viewer_custom = function(args)
	{
	    epsco_client.dbgrid = new datagrid();
        

            var cols = {1:'SEQ_ID'
                    ,2:'CIF ID'
                    ,3:'Last Name'
                    ,4:'First Name'
                    ,5:'Middle Name'
                    ,6:'Birth Date'
                    ,7:'Age'
                    ,8:'Address'
                    ,9:'Card'
                    ,10:'Card No.' 
                    ,11:'EPSCO_SEQID'};


        for(var x in cols)
        {
            epsco_client.dbgrid.addColumn(epsco_client.create_column(cols[x],'left',(cols[x] == 'SEQ_ID' || cols[x] == 'EPSCO_SEQID' ? 'none' : '')));
        }
         
        epsco_client.dbgrid.rows_page = 10;
        epsco_client.dbgrid.row.onclick_enabled = true;
        epsco_client.dbgrid.row.onclick = "epsco_client.do_custom_ajaxgridview(); ";
        epsco_client.dbgrid.bind(args.value, 'tdBeneficiary', 'grdBeneficiary');
        document.getElementById("grdBeneficiary").style.display = "";
        document.getElementById("btnDeleteEnrollee").disabled = "disabled";
        document.getElementById("btnSaveBene").disabled = "disabled";
	}
	
	this.do_custom_ajaxgridview = function()
	{
	    var param = 'ajx_selected$' + epsco_client.dbgrid.getValueAt(epsco_client.dbgrid.row.selected + (epsco_client.dbgrid.curr_page - 1) * epsco_client.dbgrid.rows_page , 0);
	    epsco_client.do_ajaxgridview(param);
	}
	
	
	this.enabled_checkbox = function(obj)
	{
	    var tbody = document.getElementById(obj);
        
        var e = tbody.getElementsByTagName("*");
             
        for(var i=0;i<e.length;i++)
        {
            if(e[i].type == "checkbox")
            {
              if(e[i].disabled) e[i].disabled = false;
            }
        }
	}
	
	this.batch_approve = function()
	{
	    var msg = "Are you sure to approve selected enrollees?";
                    
        var x = window.confirm(msg);
                             
        if(x)
        {
           ajax_show_status(true);
           
	       for(var i=0; i <= 9;i++)
           {
                if(epsco_client.dbgrid.getValueAt(i + ((epsco_client.dbgrid.curr_page - 1) * epsco_client.dbgrid.rows_page),0))
                {
                    var seq_id = epsco_client.dbgrid.getValueAt(i + ((epsco_client.dbgrid.curr_page - 1) * epsco_client.dbgrid.rows_page),8);
                        
                    var result = IRSWorld.EPSCO.EPSCOEnrollment.app_generate_RefNo(seq_id);
               }
           }
           
           ajax_show_status(false);
           
           alert('Record commited successfully.');
           
           document.getElementById("txtsearchLName2").value = "";
           document.getElementById("txtsearchFName2").value = "";
           document.getElementById("txtsearchMName2").value = "";
                            
           epsco_client.RemLName = null;
           epsco_client.RemFName = null;
           epsco_client.RemMName = null;
           epsco_client.BenLName = null;
           epsco_client.BenFName = null;
           epsco_client.BenMName = null;
           epsco_client.BenAcctNo = null;
           epsco_client.populate_data();
       }
	}
	
	this.batch_export_data = function()
	{   
	    var data = "";
	    
	    for(var i=0; i<=9; i++)
        {
            if(epsco_client.dbgrid.getValueAt(i + ((epsco_client.dbgrid.curr_page - 1) * epsco_client.dbgrid.rows_page),0))
            {        
                data +=     epsco_client.dbgrid.getValueAt(i + ((epsco_client.dbgrid.curr_page - 1) * epsco_client.dbgrid.rows_page), 6)
                            + "," + epsco_client.dbgrid.getValueAt(i + ((epsco_client.dbgrid.curr_page - 1) * epsco_client.dbgrid.rows_page), 1)
                            + "," + epsco_client.dbgrid.getValueAt(i + ((epsco_client.dbgrid.curr_page - 1) * epsco_client.dbgrid.rows_page), 2) 
                            + "," + epsco_client.dbgrid.getValueAt(i + ((epsco_client.dbgrid.curr_page - 1) * epsco_client.dbgrid.rows_page), 3) 
                            + "," + epsco_client.dbgrid.getValueAt(i + ((epsco_client.dbgrid.curr_page - 1) * epsco_client.dbgrid.rows_page), 4) 
                            + "," + epsco_client.dbgrid.getValueAt(i + ((epsco_client.dbgrid.curr_page - 1) * epsco_client.dbgrid.rows_page), 5)
                            + "," + epsco_client.dbgrid.getValueAt(i + ((epsco_client.dbgrid.curr_page - 1) * epsco_client.dbgrid.rows_page), 7)
                            + "," + epsco_client.dbgrid.getValueAt(i + ((epsco_client.dbgrid.curr_page - 1) * epsco_client.dbgrid.rows_page), 8) + "\n";
            }
        }
    
        document.getElementById('h_obj_data').value = data;
        epsco_client.populate_data();
        document.getElementById('invoke_obj_data').click();
	}
	
	this.batch_export_data_all = function()
    {
        var msg = "Are you sure to export all approved enrollees?";
                    
        var x = window.confirm(msg);
                             
        if(x)
        {
	        var data = "";
        
	        var row_length = epsco_client.dbgrid.datatable.Rows.length - 1;
	        var col_length = epsco_client.dbgrid.datatable.Columns.length - 2;

	        for(var i=0; i<= row_length; i++)
	        {
		        for(var j=1; j<= col_length; j++)
		        {
			        data += epsco_client.dbgrid.getDataAt(i,j) + ",";
			        if (j==col_length)  data += "\n";
		        }		
	        }
	        document.getElementById('h_Export_all').value = data;
            epsco_client.populate_data();
            document.getElementById('invoke_export_all').click();
        }
	}
	
	this.check_all = function(isCheck)
    {
	   for(var i=0; i<=9; i++)
       {
           var childs = epsco_client.dbgrid.getElementsAt(i + ((epsco_client.dbgrid.curr_page - 1) * epsco_client.dbgrid.rows_page),0);
           
           if(childs != null && childs != 'null')
           {
               for (var k=0; k < childs.length; k++)
               {
                    if (childs[k].type == 'checkbox')
                    {
                        childs[k].checked = isCheck;
                    } 
               }
           }
       }
	}
} 

function clearForm(obj)
{
        var div = document.getElementById(obj);
        
        var e = div.getElementsByTagName("*");
             
        for(var i=0;i<e.length;i++)
        {
            if(e[i].type == "text")
            {
                e[i].value="";
            }
            else if(e[i].type == "select-one")
            {
                e[i].selectedIndex = 0;
            }
        }
        
        epsco_client.Rem_SEQID = null;
        epsco_client.Ben_SEQID = null;
        epsco_client.CIF_SEQ_ID = null;
        epsco_client.CIFClass = null;
        
        epsco_client.isZipCodeShow = null; 
        
        document.getElementById("btnAddBene").disabled = "";
        document.getElementById("btnSaveRem").disabled = "disabled";
        document.getElementById("btnSaveBene").disabled = "disabled";
        document.getElementById("btnDeleteEnrollee").disabled = "disabled";
        document.getElementById("grdBeneficiary").style.display = "none";
        
}
