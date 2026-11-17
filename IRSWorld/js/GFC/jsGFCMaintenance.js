function showPanel(divName)
{
    var div = document.getElementById(divName);
    
    divmask = new maskpanel();
    divmask.show();
    
    document.getElementById(divName).style.display = 'block';
    
    div.style.top = (((document.documentElement.clientHeight / 2) + document.documentElement.scrollTop) - (div.offsetHeight / 2)) + 'px';
    div.style.left = (((document.documentElement.clientWidth / 2) + document.documentElement.scrollLeft) - (div.offsetWidth / 2)) + 'px';
}


function execute_ajx_command(param)
{
    if (GFC_Main != null && GFC_Main != "undefined")
    
    {
        GFC_Main.do_ajax(param);
    }
}

function execute_ajaxgridview_command(param)
{
    if (GFC_Main != null && GFC_Main != "undefined")
    {
        GFC_Main.do_ajaxgridview(param);
    }
}
/*===================================================================================================

Note*     
    Module modes (Mode)
        0 = claim 
        1 = replace - cancel 
        2 = replace approval 
        3 = cancellation approval  
        4 = Purchase 
        5 = Availment
        6 = Inquiry
        7 = BatchEnrollment    
 
    Replace Cancel (repCan)
        0 = Replacement
        1 = Cancellation 
       
    Grid Mode (GridMode)
        0 = Beneficiary
        1 = Groups
        2 = Other Grids     
    
    Other Grid Mode (OtherGridMode)
        0 = Grid Address
        1 = Grid Contacts
        2 = Grid Card History  
    
    Client Type (ClientType)
        1 = Remitter
        2 = Beneficiary       

===================================================================================================*/

function GFC_Main()
{   
    var icls = this;
    this.grid_ajax_query = null;
    this.grid_ajax_page  = "../AjxPage/ajaxgrid.aspx";
    this.CIFID = null;
    this.grid = null;

    this.Mode   =  3; 
    this.CurrCd = null;
    this.repCan = null;
    this.Date = null;
    this.AppSEQID = null;
    this.RemData = null;
    this.inqMode = null;
    this.GridMode = 0;
    this.ClientType = 0;
    this.OtherGridMode = 0; 
   
    this.rem_seq_id = null;
    this.bene_seq_id = null; 
   
    this.page_onload = function()
    {         
        document.getElementById('trAddNewClient').style.display = 'none';           
        this.Mode = getQueryString(window, 'mode');        
        
        if (this.Mode == '4' || this.Mode == '5' || this.Mode == '7')
        {
            document.getElementById('trAddNewClient').style.display = 'block';
            this.AddNewCIF();
        }  
                 
        this.changeHeaderTitle(); 
        this.viewMode(0);
        this.popDDL();
    }
   
    this.page_unload = function()
    {
        if (this.Mode == '4' || this.Mode == '5' )
        {  
            if (document.getElementById('txtAccountNo').value != null && document.getElementById('txtAccountNo').value != 'undefined' && document.getElementById('txtAccountNo').value != "")
            {
                IRSWorld.GFC.GFCMaintenance.UnlockCK(document.getElementById('txtAccountNo').value, this.page_unload_callback);
            }
        }  
    }
   
    this.page_unload_callback = function(response)
    {
        if(response.error != null)
        {
            alert(response.error.Message.toString().replace('System.Exception ', ''));
        }
        else
        {
             //document.getElementById('txtAccountNo').innerText = '';
             setText(document.getElementById('txtAccountNo'), '');
        }
    }  
   
    this.changeHeaderTitle = function()
    {
        var Header = '';
        var Header2 = ''; 
 
        switch(this.Mode)
        {
        case '0':
          Header = 'Claim GFC';
          Header2 = 'Search Unclaimed GFC';
          break;
        case '1':
          Header = 'GFC Replacement / Cancellation';
          Header2 = 'Search Claimed GFC';
          break;
        case '2':
          Header = 'GFC Replacement Approval';
          Header2 = 'Search GFC For Replacement';
          break;
        case '3':
          Header = 'GFC Cancellation Approval';
          Header2 = 'Search GFC For Cancellation';
          break;  
        case '4':
          Header = 'GFC Availment';
          Header2 = 'Search Remitter';
          break;
        case '5':
          Header = 'GFC Purchase';
          Header2 = 'Search Remitter';
          break;
        case '6':
          Header = 'GFC Inquiry';
          Header2 = 'Search GFC';
          break;
        case '7':
          Header = 'GFC Batch Enrollment';
          Header2 = 'Search Remitter';
          break; 
        default:
          Header = 'Global Filipino Card';
          Header2 = '';
          break;   
        }  
        //document.getElementById('tdHeader').innerText = Header;
        setText(document.getElementById('tdHeader'), Header);
        //document.getElementById('tdSearchHeader').innerText = Header2; 
        setText(document.getElementById('tdSearchHeader'), Header2);   
    }    
    
    this.panel_main_search = function()
    {
        if (this.check_search_input() == true)
        {
            if (this.Mode == null | this.Mode == 'undefined')
            {
                alert('Session Time Out'); 
                return;     
            }
            var sp_to_use = (document.getElementById('ddlClientType').value == 1) ? 'sp_GFCSearchRemitter' : 'sp_GFCSearchBeneficiary';
            this.ajax_grid = new grid_selector();
            this.grid_ajax_query = sp_to_use + "~" +  
                document.getElementById("txtSearchLastName").value   + "~" + 
                document.getElementById("txtSearchFirstName").value  + "~" + 
                document.getElementById("txtSearchMiddleName").value + "~" + 
                document.getElementById("txtSearchCIFID").value + "~" + 
                document.getElementById("txtSearchOLDCIFID").value + "~" +
                document.getElementById("txtSearchCARDNo").value + "~" +
                document.getElementById("txtSearchCKNo").value + "~"  +
                this.Mode.toString() + "~";
                
            this.ajax_grid.open_selection_grid(null, this.grid_ajax_query, 0, null);
        }
    }
    
    this.check_search_input = function()
    {
        if (document.getElementById('ddlClientType').value == 0)
        {
            alert('Invalid Client Type'); 
            return false;
        } 
    
        var lastname_length = (document.getElementById("txtSearchLastName").value).length;
        var firstname_length = (document.getElementById("txtSearchFirstName").value).length;
        var middlename_length = (document.getElementById("txtSearchMiddleName").value).length;
        var cifid_length = (document.getElementById("txtSearchCIFID").value).length;
        var oldcifid_len = (document.getElementById("txtSearchOLDCIFID").value).length;
        var CKNumber_len =  (document.getElementById("txtSearchCKNo").value).length;
        var CardNumber_len =  (document.getElementById("txtSearchCARDNo").value).length;
          
        var total_length = lastname_length + firstname_length + middlename_length + cifid_length 
                                 + oldcifid_len + CKNumber_len + CardNumber_len;
        
        if (total_length == 0)
        {
             alert("Invalid search value!");
             return false;   
        }
        
        return true;
    }     
   
    this.do_ajax = function(parameter)
    {   
       if (parameter != null && parameter != "undefined")
       {
            var command_param = parameter.split("$");
            if (command_param.length > 0)
            {
                if (command_param[0] == "ajx_selected")
                {
                    if (this.ClientType == 1)
                    {
                        this.AppSEQID =  command_param[1];
                        this.populateInfo(command_param[1], "true");
                    }
                    else if(this.ClientType == 2)
                    {
                        this.AppSEQID =  command_param[1];
                        this.populateInfo(command_param[1], "false");
                    }
                }
            }
        }                
       this.ajax_grid.process_ajax(parameter);
    }
   
    this.do_ajaxgridview = function(parameter)
    {
        if (parameter != null && parameter != "undefined")
        {
            var command_param = parameter.split("$");
            if (command_param.length > 0)
            {
                if (command_param[0] == "ajx_selected")
                {
                    this.AppSEQID =  command_param[1];
                    if (this.GridMode == 0)
                    {
                        this.populateInfo(command_param[1], "false");
                    }
                    else if (this.GridMode == 1)
                    {
                        this.popCardAndAccounts(command_param[1]);
                    }
                    else if (this.GridMode == 2)
                    {
                        // do nothing
                    }                    
                    return;
                }
               else
               {
                    switch(this.GridMode)
                    {
                        case 0:  this.process_viewer_benef(parameter); break;
                        case 1:  this.process_viewer_batch(parameter); break;
                        case 2:  
                                    if (this.OtherGridMode == 0) {this.process_viewer_address(parameter); }
                                    else if (this.OtherGridMode == 1) {this.process_viewer_contacts(parameter); } 
                                    else if (this.OtherGridMode == 2) {this.process_viewer_cardhistory(parameter); }                                    
                                    break;
                    }  
               }  
            }        
        }  
    }  
   
    this.populateInfo = function(SEQ_ID, isRem)
    {
         if (isRem == "true")
         {
            IRSWorld.GFC.GFCMaintenance.getSearchRemDetails(SEQ_ID, this.populateRemInfo_callback);
         }
         else
         {
             if (this.Mode != null && this.Mode != 'undefined')
             {
                if (this.Mode != 6)
                {
                    IRSWorld.GFC.GFCMaintenance.getSearchDetails(SEQ_ID, this.Mode.toString(), this.populateInfo_callback);
                }
                else
                {
                    var param = SEQ_ID.split("~");
                    IRSWorld.GFC.GFCMaintenance.getSearchDetailsInquiry(param[0], param[1], this.populateInfo_callback);
                }
             }
         }
    }
    
   this.populateRemInfo_callback = function(response)
   {
        if (response.error != null)
        {
            alert(response.error.Message.toString().replace('System.Exception ', ''));
        }
        else
        {
            var ds = response.value;
            
            if(ds != null && ds != "undefined" && ds.Tables[0].Rows.length > 0)
            { 
                //document.getElementById('tdMainInfo').innerText = "Remitter Information";  
                setText(document.getElementById('tdMainInfo'), "Remitter Information");  
                icls.viewMode(7);
                icls.RemData = ds;
                icls.popFields(ds);
                icls.popRemMinInfo();                  
                icls.process_viewer_benef('ajx_loading$');
            }
           
            this.value = icls;     
        }       
   }
   
    this.popFields = function(data)
    {
        var dt = data.Tables[0];            
        //document.getElementById('txtCIFID').innerText = (dt.Rows[0].CIFID == null) ? "" : dt.Rows[0].CIFID.toString();
        setText(document.getElementById('txtCIFID'), (dt.Rows[0].CIFID == null) ? "" : dt.Rows[0].CIFID.toString());
        //document.getElementById('txtLastname').innerText = (dt.Rows[0].LastName == null) ? "" : dt.Rows[0].LastName.toString();
        setText(document.getElementById('txtLastname'), (dt.Rows[0].LastName == null) ? "" : dt.Rows[0].LastName.toString());
        //document.getElementById('txtFirstname').innerText = (dt.Rows[0].FirstName == null) ? "" : dt.Rows[0].FirstName.toString();
        setText(document.getElementById('txtFirstname'), (dt.Rows[0].FirstName == null) ? "" : dt.Rows[0].FirstName.toString());
        //document.getElementById('txtMiddlename').innerText = (dt.Rows[0].MiddleName == null) ? "" : dt.Rows[0].MiddleName.toString();
        setText(document.getElementById('txtMiddlename'), (dt.Rows[0].MiddleName == null) ? "" : dt.Rows[0].MiddleName.toString());
        //document.getElementById('txtIndAlias').innerText = (dt.Rows[0].Alias == null) ? "" : dt.Rows[0].Alias.toString();
        //document.getElementById('txtTitle').innerText = (dt.Rows[0].Title == null) ? "" : dt.Rows[0].Title.toString();
        //document.getElementById('txtBirthday').innerText = (dt.Rows[0].BirthDate == null) ? "" : dt.Rows[0].BirthDate.toString();
        setText(document.getElementById('txtBirthday'), (dt.Rows[0].BirthDate == null) ? "" : dt.Rows[0].BirthDate.toString());
        //document.getElementById('txtCitizenship').innerText = (dt.Rows[0].Citizenship == null) ? "" : dt.Rows[0].Citizenship.toString();
        //document.getElementById('txtReligion').innerText = (dt.Rows[0].Religion == null) ? "" : dt.Rows[0].Religion.toString();
        //document.getElementById('txtStatus').innerText = (dt.Rows[0].Status == null) ? "" : dt.Rows[0].Status.toString();
        setText(document.getElementById('txtStatus'), (dt.Rows[0].Status == null) ? "" : dt.Rows[0].Status.toString());
        //document.getElementById('txtBirthplace').innerText = (dt.Rows[0].BirthPlace == null) ? "" : dt.Rows[0].BirthPlace.toString();
        //document.getElementById('txtAge').innerText = icls.compute_age(((dt.Rows[0].BirthDate == null) ? "" : dt.Rows[0].BirthDate.toString()));
        setText(document.getElementById('txtAge'), icls.compute_age(((dt.Rows[0].BirthDate == null) ? "" : dt.Rows[0].BirthDate.toString())));
        //document.getElementById('txtCivilStatus').innerText = (dt.Rows[0].CivilStatus == null) ? "" : dt.Rows[0].CivilStatus.toString();
        //document.getElementById('txtGender').innerText = (dt.Rows[0].Gender == null) ? "" : dt.Rows[0].Gender.toString();
        //document.getElementById('txtBranch').innerText = (dt.Rows[0].BranchCode == null) ? "" : dt.Rows[0].BranchCode.toString();
        setText(document.getElementById('txtBranch'), (dt.Rows[0].BranchCode == null) ? "" : dt.Rows[0].BranchCode.toString());
        
        //document.getElementById('txtAccountNo').innerText = dt.Rows[0].AccountNo.toString();
        setText(document.getElementById('txtAccountNo'), dt.Rows[0].AccountNo.toString());
       
        if (this.Mode == 4 | this.Mode == 5 | this.Mode == 7)
        {
            document.getElementById('ddlCurr').selectedIndex = 0;
        }
        else
        {
            if  (dt.Rows[0].CurrCd == null)
            {
                document.getElementById('ddlCurr').selectedIndex = 0;
            }
            else
            {
                document.getElementById('ddlCurr').selectedIndex = this.getIndex('ddlCurr',dt.Rows[0].CurrCd.toString()); 
            }
        }

        //document.getElementById('txtCardNo').innerText = (dt.Rows[0].GFCNo == null) ? "" : dt.Rows[0].GFCNo.toString();
        setText(document.getElementById('txtCardNo'), (dt.Rows[0].GFCNo == null) ? "" : dt.Rows[0].GFCNo.toString());
        //document.getElementById('txtCardType').innerText = (dt.Rows[0].GFCCardType == null) ? "" : dt.Rows[0].GFCCardType.toString();
        setText(document.getElementById('txtCardType'), (dt.Rows[0].GFCCardType == null) ? "" : dt.Rows[0].GFCCardType.toString());
        //document.getElementById('txtDateClaimed').innerText = (dt.Rows[0].DateClaimed == null) ? "" : dt.Rows[0].DateClaimed.toString();
        setText(document.getElementById('txtDateClaimed'), (dt.Rows[0].DateClaimed == null) ? "" : dt.Rows[0].DateClaimed.toString());
        //document.getElementById('txtIDPresented').innerText = (dt.Rows[0].IDPresented == null) ? "" : dt.Rows[0].IDPresented.toString();
        setText(document.getElementById('txtIDPresented'), (dt.Rows[0].IDPresented == null) ? "" : dt.Rows[0].IDPresented.toString());
        //document.getElementById('txtNewCardNo').innerText =  (dt.Rows[0].NewCardNo == null) ? "" : dt.Rows[0].NewCardNo.toString();
        setText(document.getElementById('txtNewCardNo'),  (dt.Rows[0].NewCardNo == null) ? "" : dt.Rows[0].NewCardNo.toString());
                               
        var reason =  (dt.Rows[0].Reason == null) ? "" : dt.Rows[0].Reason.toString();  
        if (this.Mode == 2 | this.Mode == 3)
        {   
            var index = this.getIndexText('ddlReason',reason);
            
            if (index != null)
            {
                document.getElementById('ddlReason').selectedIndex = index;
            }
            else
            {
                 document.getElementById('ddlReason').selectedIndex = this.getIndex('ddlReason', '0'); // Others
                 //document.getElementById('txtSpecify').innerText = reason;
                 setText(document.getElementById('txtSpecify'), reason);
                 this.visible('trSpecifyReason'); 
            }
        }
        else if (this.Mode == 6)
        {
            if (this.inqMode == 3 |  this.inqMode == 4)
            {
                var index = this.getIndexText('ddlReason',reason);
                if (index != null)   
                {
                    document.getElementById('ddlReason').selectedIndex = index;
                }
                else
                {
                     document.getElementById('ddlReason').selectedIndex = this.getIndex('ddlReason', '0'); // Others
                     //document.getElementById('txtSpecify').innerText = reason;
                     setText(document.getElementById('txtSpecify'), reason);
                     this.visible('trSpecifyReason'); 
                }
            }
        }         
    }
    
    this.populateInfo_callback = function(response)
    {
        if (response.error != null)
        {
            alert(response.error.toString().replace('System.Exception ', ''));
        }
        else
        {
            //document.getElementById('tdMainInfo').innerText = "Beneficiary Information";   
            setText(document.getElementById('tdMainInfo'), "Beneficiary Information");   
            var ds = response.value;
            
            if(ds != null && ds != "undefined" && ds.Tables[0].Rows.length > 0)
            {   
                if (icls.Mode == 0) {icls.viewMode(1);}
                else if (icls.Mode == 1) {icls.viewMode(2);} 
                else if (icls.Mode == 2) {icls.viewMode(3);}
                else if (icls.Mode == 3) {icls.viewMode(4);}
                else if (icls.Mode == 4) {icls.viewMode(5);}
                else if (icls.Mode == 5) {icls.viewMode(6);}
                else if (icls.Mode == 6) 
                {
                    var param = icls.AppSEQID.split("~");
                    icls.inqMode = parseFloat(param[1]);
                    icls.inqMode = icls.inqMode + 1;                    
                    icls.viewMode(icls.inqMode); 
                    
                    icls.invisible('CIFACCOUNTSMAIN');
                    icls.invisible('CIFCARDSMAIN');
                    
                    icls.process_viewer_cardhistory('ajx_loading$', ds.Tables[0].Rows[0].CIFID);
                    icls.invisible('SHOWHIDEOTHERDETAILS');
                    icls.visible('GRIDOTHERDETAILS');
                    icls.visible('GRID2');
                    
                    icls.invisible('CARDREPCANBTN');
                    icls.invisible('APPRREJECTBTN');
                    icls.invisible('SAVECANCELBTN');                    
                    icls.invisible('FORMS');
                    icls.invisible('SUBMITCANCELBTN');
                    icls.visible('trIDPresented');
                    icls.visible('trClaimeDate');
                    if (icls.inqMode == 1){icls.invisible('CIFCARDSMAIN');}       
                }      
                else if (icls.Mode == 7) {icls.viewMode(8);} 
                 
                icls.popFields(ds);
                icls.CurrCd = ds.Tables[0].Rows[0].CurrCd;
                icls.CIFID =  ds.Tables[0].Rows[0].CIFID;
               
                // grid view's 
                //icls.process_viewer_cardhistory('ajx_loading$', icls.CIFID);
                icls.process_viewer_address('ajx_loading$', icls.CIFID);
                icls.process_viewer_contacts('ajx_loading$', icls.CIFID);
                //icls.visible('GRID2');
                icls.visible('GRIDADDRESS');
                icls.visible('GRIDCONTACTS');

                icls.GridMode = 2;
                
                if (icls.Mode == 2 | icls.Mode == 3)
                {  
                    icls.AppSEQID = ds.Tables[0].Rows[0].AppSEQID;
                }
               
               if (icls.Mode == 7)
               {
                    if (document.getElementById("ddlGroups").value != null && document.getElementById("ddlGroups").value != "" && document.getElementById("ddlGroups").value != "undefined")
                    {
                        icls.GridMode = 1;
                        this.value = icls;
                        icls.process_viewer_batch('ajx_loading$');
                    } 
               }   
            }
           
            this.value = icls;     
        }    
    }
   
    this.popDDL = function()
    {
        IRSWorld.GFC.GFCMaintenance.getDDLdata(this.popDDL_callback); 
    }
    
    this.popDDL_callback = function(response)
    {
         if (response.error != null)
         {
            alert(response.error.Message.toString().replace('System.Exception ', ''));
         }
         else
         {
            ds = response.value;
             
            if (ds != null)
            {
                // For Reasons 
                if(ds.Tables[0].Rows.length > 0 )
                {       
                    var DDL = document.getElementById('ddlReason');
                                    
                    for (var i = 0; i < ds.Tables[0].Rows.length; i++)
                    {
                        var opt = document.createElement("option");                 
                        opt.text = ds.Tables[0].Rows[i].fldDescription.toString();
                        opt.value = ds.Tables[0].Rows[i].fldValue.toString();
                        DDL.options.add(opt); 
                    }
                 }
                 // For Currency
                 if (ds.Tables[1].Rows.length > 0)
                 {  
                    var DDL2 = document.getElementById('ddlCurr');
                    
                    for (var j = 0; j < ds.Tables[1].Rows.length; j++)
                    {                   
                        var opt2 = document.createElement("option");                 
                        opt2.text = ds.Tables[1].Rows[j].FldCode.toString();
                        opt2.value = ds.Tables[1].Rows[j].FldValue.toString();
                        DDL2.options.add(opt2); 
                    }
                }
                // For Groups
                 if (ds.Tables[2].Rows.length > 0)
                 {  
                    var DDL3 = document.getElementById('ddlGroups');
                    
                    for (var k = 0; k < ds.Tables[2].Rows.length; k++)
                    {                   
                        var opt3 = document.createElement("option");                 
                        opt3.text = ds.Tables[2].Rows[k].FldCode.toString();
                        opt3.value = ds.Tables[2].Rows[k].FldValue.toString();
                        DDL3.options.add(opt3); 
                    }
                } 
               //For Class Type
                
                if (ds.Tables[3].Rows.length > 0)
                 {  
                    var DDL4 = document.getElementById('ddlClientType');
                    
                    for (var k = 0; k < ds.Tables[3].Rows.length; k++)
                    {                   
                        var opt4 = document.createElement("option");                 
                        opt4.text = ds.Tables[3].Rows[k].FldCode.toString();
                        opt4.value = ds.Tables[3].Rows[k].FldValue.toString();
                        DDL4.options.add(opt4); 
                    }
                } 
             }   
         } 
    } 
    
    this.clearValues = function()
    {
        //document.getElementById('txtCIFID').innerText = "";
        setText(document.getElementById('txtCIFID'), "");
        //document.getElementById('txtLastname').innerText = "";
        setText(document.getElementById('txtLastname'), "");
        //document.getElementById('txtFirstname').innerText = ""; 
        setText(document.getElementById('txtFirstname'), ""); 
        //document.getElementById('txtMiddlename').innerText = "";
        setText(document.getElementById('txtMiddlename'), "");
        
        //document.getElementById('txtIndAlias').innerText = "";
        //document.getElementById('txtTitle').innerText = "";
        //document.getElementById('txtBirthday').innerText = "";
        setText(document.getElementById('txtBirthday'), "");        
        //document.getElementById('txtCitizenship').innerText = "";
        //document.getElementById('txtReligion').innerText = "";
        //document.getElementById('txtStatus').innerText = "";
        setText(document.getElementById('txtStatus'), "");        
        //document.getElementById('txtBirthplace').innerText = "";
        //document.getElementById('txtAge').innerText = "";
        setText(document.getElementById('txtAge'), "");
        //document.getElementById('txtCivilStatus').innerText = "";
        //document.getElementById('txtGender').innerText = "";
        //document.getElementById('txtBranch').innerText = "";
        setText(document.getElementById('txtBranch'), "");
        
        //document.getElementById('txtAccountNo').innerText = "";
        setText(document.getElementById('txtAccountNo'), "");                        
        document.getElementById('ddlCurr').selectedIndex = 0;

        //document.getElementById('txtCardNo').innerText = "";
        setText(document.getElementById('txtCardNo'), "");
        //document.getElementById('txtCardType').innerText = "";
        setText(document.getElementById('txtCardType'), "");
        //document.getElementById('txtDateClaimed').innerText = "";
        setText(document.getElementById('txtDateClaimed'), "");
        //document.getElementById('txtIDPresented').innerText = "";
        setText(document.getElementById('txtIDPresented'), "");

        document.getElementById('btnReplacement').value = 'For Replacement';
        document.getElementById('btnCancellation').value = 'For Cancellation';
               
        document.getElementById('ddlReason').selectedIndex = 0;          
    }   
    
    this.formatDate = function(date)
    {
        return dateToString(date);
    } 
    
    this.viewMode = function(mode) // Search = 0 // Claim = 1 // Replace - Cancellation = 2 // Approve - Disaaprove = 3
    {
        this.HideAllDiv();
        this.clearValues(); 
        this.GridMode = 0;  
        //document.getElementById('tdGRID1').innerText = 'Beneficiaries'; 
        if (mode == 0) 
        {
             this.visible('CIFSearch');
             this.RemData = null;
        } 
        else if(mode == 1)
        {
             this.visible('CIFINDIVIDUAL');
             this.visible('CIFACCOUNTSMAIN');
             this.visible('CIFCARDSMAIN');
             this.visible('SAVECANCELBTN');
             //this.visible('FORMS');
             this.visible('CIFPERSONALINFOENTRY');
             this.visible('trIDPresented');
             
             this.visible('SHOWHIDEOTHERDETAILS');
             
             this.enable('txtCardNo');
             this.enable('txtIDPresented');
        }   
        else if(mode == 2)
        {
             this.visible('CIFINDIVIDUAL');
             this.visible('CIFACCOUNTSMAIN');
             this.visible('CIFCARDSMAIN');
             this.visible('CARDREPCANBTN');
             this.visible('CIFPERSONALINFOENTRY');
             
             this.visible('SHOWHIDEOTHERDETAILS');
             
             this.visible('trCardType');
        }        
        else if(mode == 3) //  Approval
        {
             this.visible('CIFINDIVIDUAL');
             this.visible('CIFACCOUNTSMAIN');
             this.visible('CIFCARDSMAIN');
             this.visible('APPRREJECTBTN');
             this.visible('CIFPERSONALINFOENTRY');  
             
             this.visible('SHOWHIDEOTHERDETAILS');
             
             this.visible('trCardType');             
             this.visible('trReason');
             this.visible('trNewGFCNo');
             if (document.getElementById('ddlReason').value == 4)
             {
                 this.visible('trSpecifyReason'); 
             }                      
        }        
        else if(mode == 4) //  Approval
        {
             this.visible('CIFINDIVIDUAL');
             this.visible('CIFACCOUNTSMAIN');
             this.visible('CIFCARDSMAIN');
             this.visible('APPRREJECTBTN');
             this.visible('CIFPERSONALINFOENTRY');  
             
             this.visible('SHOWHIDEOTHERDETAILS');
             
             this.visible('trCardType');
             this.visible('trReason');             
             if (document.getElementById('ddlReason').value == 4)
             {
                 this.visible('trSpecifyReason'); 
             }                        
        }                
        else if(mode == 5) // Availment
        {
             this.visible('CIFINDIVIDUAL');
             this.visible('CIFACCOUNTSMAIN');
             this.visible('SUBMITCANCELBTN');
             this.visible('CIFPERSONALINFOENTRY');
             
             this.visible('SHOWHIDEOTHERDETAILS');
             
             this.enable('ddlCurr');
             this.enable('btnGETCK');
             this.visible('btnGETCK');   
        }
        else if(mode == 6) // Purchase
        {
             this.visible('CIFINDIVIDUAL');
             this.visible('CIFACCOUNTSMAIN');
             this.visible('CIFCARDSMAIN');
             this.visible('SAVECANCELBTN');
             //this.visible('FORMS');
             this.visible('CIFPERSONALINFOENTRY');
             this.visible('trIDPresented');
             
             this.visible('SHOWHIDEOTHERDETAILS');             
             
             this.enable('txtCardNo');
             this.enable('txtIDPresented');
             this.visible('btnGETCK');
             this.enable('btnGETCK');
             this.enable('ddlCurr');
        }
        else if(mode == 7) // Remitter info view
        {
             this.visible('CIFINDIVIDUAL');
             //this.visible('CIFIDGRIDBENEF');
             this.visible('GRID1');
        }
        else if(mode == 8) // GFC Batch Enrollment
        {
             this.visible('CIFINDIVIDUAL');
             this.visible('CIFACCOUNTSMAIN');
             this.visible('CIFCARDSMAIN');
             this.visible('SAVECANCELBTN');
             //this.visible('FORMS');
             this.visible('CIFPERSONALINFOENTRY');
             
             this.visible('SHOWHIDEOTHERDETAILS');
             
             this.visible('dvGROUP');
             this.visible('dvCARDLIST');
             this.visible("trCardType");
        }    
       
        if (this.ClientType == 2)
        {
            this.invisible('CIFPERSONALINFOENTRY');
        }
    }
   
    this.HideAllDiv = function()
    {
         this.invisible('CIFSearch');
         this.invisible('CIFINDIVIDUAL');
         this.invisible('CIFACCOUNTSMAIN');
         this.invisible('CIFCARDSMAIN');
         this.invisible('SAVECANCELBTN');    
         this.invisible('FORMS'); 
         this.invisible('CARDREPCANBTN');
         this.invisible('APPRREJECTBTN'); 
         this.invisible('SUBMITCANCELBTN');
         //this.invisible('CIFIDGRIDBENEF');  
         this.invisible('GRID1');          
         this.invisible('GRID2');
         this.invisible('CIFPERSONALINFOENTRY');
         
         this.invisible('GRIDOTHERDETAILS');
         this.invisible('SHOWHIDEOTHERDETAILS');  
         
         this.invisible('REASONDIALOG');
         this.invisible('dvGROUP');
         this.invisible('dvCARDLIST');       
           
         this.invisible('trReason');
         this.invisible('trSpecifyReason');
         this.invisible('trNewGFCNo');
         this.invisible('trCardType');
         this.invisible('trClaimeDate');
         this.invisible('trIDPresented');   
        
         this.disable('txtCardNo');
         this.disable('txtIDPresented');
         this.disable('ddlReason');
         this.disable('txtSpecify');
         this.disable('txtNewCardNo');
         this.disable('ddlCurr');
         
         this.invisible('btnGETCK');   
    } 
   
    this.compute_age = function(birthDate)
    {
        if (birthDate == "")
        {
            return "";            
        }
        
        this.generic = new generic_method();
        
        var age = this.generic.compute_age(birthDate);
        
        if (age == "NaN")
        {
            return ""; 
        }
        else
        {
            return age; 
        }    
    }   
    
  
    this.enable = function(oName)
    {
        var id = document.getElementById(oName);
        var cset = id.disabled;
       
        id.disabled = cset == "false";
    }  

    this.disable = function(oName)
    {        
         
        var id = document.getElementById(oName);
        var cset = id.disabled;
       
        id.disabled = (cset == "false") ? "true" : "false";
    }  
  
    this.getIndex = function(DDL, value)
    {         
        DDL = document.getElementById(DDL);         
         
        for(var i = 0; i < DDL.options.length; i++)
        {
             if (DDL.options[i].value == value)
             {
                return i;
             }
        }
        alert('value is not in the list.');
        return null;
    }
   
    this.getIndexText = function(DDL, Text)
    {         
        DDL = document.getElementById(DDL);         
         
        for(var i = 0; i < DDL.options.length; i++)
        {
             if (DDL.options[i].text == Text)
             {
                return i;
             }
        }
        return null;   
    } 
   
   this.visible = function(oName)
   {
        document.getElementById(oName).style.display = 'block';
   }     
   this.invisible = function(oName)
   {
        document.getElementById(oName).style.display = 'none';
   }
   
    //=========== Button Method==============================
   this.btnReleaseGFC_click = function()
   {
        if (confirm('Are you sure you want to release this card?'))
        {  
            showpanel('divPopProcess');
            
            var CardNo = document.getElementById('txtCardNo').value;
            var AcctNo = document.getElementById('txtAccountNo').value;             
            var IDPresented = document.getElementById('txtIDPresented').value;
            var curr = document.getElementById('ddlCurr').value; 
           
            if(this.Mode == 4 | this.Mode == 5)
            {           
               IRSWorld.GFC.GFCMaintenance.purchaseCard(this.CIFID, AcctNo, CardNo, IDPresented, curr, this.btnReleaseGFC_click_callback);        
            }
            else if(this.Mode == 7)
            {            
               IRSWorld.GFC.GFCMaintenance.linkCIFToCard(this.CIFID, AcctNo, CardNo, IDPresented, curr, this.btnReleaseGFC_click_callback); 
            } 
            else
            {            
               IRSWorld.GFC.GFCMaintenance.releaseGFC(this.CIFID, AcctNo, CardNo, IDPresented, curr, this.btnReleaseGFC_click_callback);        
            }
        }
   } 
   
   this.btnReleaseGFC_click_callback = function(response)
   {
        hidepanel('divPopProcess');
        
        if (response.error !=  null)
        {
            alert(response.error.Message.toString().replace('System.Exception ', ''));
        }
        else
        {
            var dt = response.value;                
            icls.visible('trCardType');
            icls.visible('trClaimeDate');
            icls.invisible('SAVECANCELBTN');
            icls.visible('FORMS'); 
            icls.disable('txtCardNo');
            icls.disable('txtIDPresented');                 
            //document.getElementById('txtDateClaimed').innerText = dt[7].toString();      
            setText(document.getElementById('txtDateClaimed'), dt[7].toString());
            icls.invisible('dvGROUP');
            icls.invisible('dvCARDLIST'); 
                      
           if (icls.Mode != 7)
            {   
                //document.getElementById('txtCardType').innerText = dt[6].toString();
                setText(document.getElementById('txtCardType'), dt[6].toString());
            }  
           
            alert('GFC Release is successful.');
                        
            icls.checkCKlimit();
            icls.checkCardlimit(); 
        }
   }
   
   this.btnCancel_click = function()
   {
       if (confirm('Are you sure you want to cancel this request'))
       {
           if (this.Mode == 4 | this.Mode == 5)
           {            
                if (document.getElementById('txtAccountNo').value != null && document.getElementById('txtAccountNo').value != 'undefined' && document.getElementById('txtAccountNo').value != "") 
                { 
                    var ckAccount = document.getElementById('txtAccountNo').value;
                    IRSWorld.GFC.GFCMaintenance.UnlockCK(ckAccount, this.btnACancel_click_callback);             
               }
               else
               {
                    this.backToRemInfo();
               }   
           }
           else
           {            
                this.backToRemInfo();
           }
       }
   }
   
   this.btnReplacement_click = function()
   {
        if (document.getElementById('btnReplacement').value == 'For Replacement')
        {
            //document.getElementById('btnReplacement').innerText = 'Save';
            setText(document.getElementById('btnReplacement'), 'Save');
            //document.getElementById('btnCancellation').innerText = 'Cancel';
            setText(document.getElementById('btnCancellation'), 'Cancel');
            
            this.visible('trReason');
            this.visible('trNewGFCNo');
            this.enable('ddlReason');
            this.enable('txtSpecify');
            this.enable('txtNewCardNo');
            this.repCan = '0'; 
        }
        else
        {
            if (confirm('Are you sure you want to save this request?'))
            {
                // check if reason has value 
                var action = (this.repCan == '0') ? 'Card Replacement' : 'Card Cancellation';
                
                if (document.getElementById('ddlReason').selectedIndex != null && document.getElementById('ddlReason').selectedIndex != 0)
                {
                    if (document.getElementById('ddlReason').value == 0  ) 
                    {
                        if (document.getElementById('txtSpecify').value == null || trim(document.getElementById('txtSpecify').value) == '')
                        {
                            alert('Reason for ' + action + ' is required'); 
                            return;    
                        }
                    }
                }
                else
                {   
                    alert('Reason for ' + action + ' is required');
                    return;  
                }        
                
                var Mode = this.repCan;
                var Cardno = document.getElementById('txtCardNo').value;
                var AcctNo =  document.getElementById('txtAccountNo').value;
                var Reason = (document.getElementById('ddlReason').value == 0) ? document.getElementById('txtSpecify').value : document.getElementById('ddlReason').options[document.getElementById('ddlReason').selectedIndex].text;//document.getElementById('ddlReason').options[i].text;;
                var NewCardNum = (Mode == '0') ? document.getElementById('txtNewCardNo').value : "";
                var CurrCode = this.CurrCd;
                var d =  new Date();
                this.Date = this.formatDate(d);     
                     
                showpanel('divPopProcess');
             
                IRSWorld.GFC.GFCMaintenance.saveReplaceCancel(Mode, Cardno, AcctNo, Reason, NewCardNum, CurrCode, this.CIFID, this.btnReplacement_click_callback);
            }
        }
   }
   
   this.btnReplacement_click_callback = function(response)
   {
         hidepanel('divPopProcess');
   
        if(response.error != null)
        {
            alert(response.error.Message.toString().replace('System.Exception ', ''));
        }
        else
        {
            icls.disable('ddlReason');
            icls.disable('txtSpecify');
            icls.disable('txtNewCardNo');
            icls.invisible('CARDREPCANBTN');
            
            var Action = "";
            var gfcDetails = response.value; 
             
            if  (icls.repCan == '0')
            {
                 Action = 'Card Replacement';
                 //document.getElementById('txtCardType').innerText = gfcDetails[6];
                 setText(document.getElementById('txtCardType'), gfcDetails[6]);
            }
            else
            {
                Action =  'Card Cancellation';
            } 
           
            //document.getElementById('txtDateClaimed').innerText = gfcDetails[7];
           setText(document.getElementById('txtDateClaimed'), gfcDetails[7]);
            alert(Action + ' already sent for approval.'); 
        }   
   }
   
   this.ddlReason_OnChange = function()
    {
        if (document.getElementById('ddlReason').value == '0')
        {
             document.getElementById('trSpecifyReason').style.display = 'block';
        }
        else
        {
             document.getElementById('trSpecifyReason').style.display = 'none';
             //document.getElementById('txtSpecify').innerText = '';
             setText(document.getElementById('txtSpecify'), '');
        }           
    }  
   
   this.btnCancellation_click = function()
   {
        if (document.getElementById('btnCancellation').value == 'For Cancellation')
        {
            document.getElementById('btnReplacement').value = 'Save';
            document.getElementById('btnCancellation').value = 'Cancel';
            
            this.visible('trReason');
            this.enable('ddlReason');
            this.enable('txtSpecify');
            this.repCan = '1'; 
        }
        else
        {
            if (confirm('Are you sure you want to cancel this request?'))
            {
                document.getElementById('btnReplacement').value = 'For Replacement';
                document.getElementById('btnCancellation').value = 'For Cancellation';
                this.invisible('trReason');
                this.invisible('trNewGFCNo');   
                this.disable('ddlReason');
                this.disable('txtSpecify');
                this.invisible('trSpecifyReason'); 
                this.disable('txtNewCardNo');            
               
                //document.getElementById('txtSpecify').innerText = "";
                setText(document.getElementById('txtSpecify'), "");
                document.getElementById('ddlReason').selectedIndex = 0;
                //document.getElementById('txtNewCardNo').innerText =  "";
                setText(document.getElementById('txtNewCardNo'), "");
                this.repCan = null;
            }
        }          
   }
   
   this.btnApprove_click = function()
   {        
        if (confirm('Are you sure you want to approve this transaction?'))
        {
            showpanel('divPopProcess'); 
            IRSWorld.GFC.GFCMaintenance.approveRejectTransaction(this.AppSEQID, 'Approved', 'App', this.btnApprove_click_callback);  
        }
   }
   
   this.btnApprove_click_callback = function(response)
   {
        hidepanel('divPopProcess');
   
        if (response.error != null)
        {
            alert(response.error.Message.toString().replace('System.Exception ', ''));
        }
        else
        {        
            icls.invisible('APPRREJECTBTN');            
            alert('Request successfully approved');           
        }               
   }
   
   this.btnReject_click = function()
   {
        if (confirm('Are you sure you want to reject this request?'))
        {
            this.visible('REASONDIALOG');
            this.disable('btnReject');
            this.disable('btnApprove');
            //document.getElementById('txtReason').innerText = '';
            setText(document.getElementById('txtReason'), '');
        } 
   }
   
   this.btnProceed_click = function()
   {
        if (document.getElementById('txtReason').value != null && trim(document.getElementById('txtReason').value) != '')
        {            
           showpanel('divPopProcess'); 
           IRSWorld.GFC.GFCMaintenance.approveRejectTransaction(this.AppSEQID, document.getElementById('txtReason').value, 'Rej', this.btnReject_click_callback);             
        }
        else
        {
            alert('Reason for rejection is required to reject this request.'); 
        }
   }
   
   this.closeReasonDialog_click = function()
   {
        this.enable('btnReject');
        this.enable('btnApprove');
        this.invisible('REASONDIALOG');
   }
   
   this.btnReject_click_callback = function(response)
   {
        hidepanel('divPopProcess');
   
        if (response.error != null)
        {
            alert(response.error.Message.toString().replace('System.Exception ', ''));
        }
        else
        {
            icls.invisible('APPRREJECTBTN');
            icls.enable('btnReject');
            icls.enable('btnApprove');
            icls.invisible('REASONDIALOG');   
            alert('Request successfully rejected');
        }        
   }
   
   this.btnReleaseForm_click = function()
   {
        // Release form printing here
        IRSWorld.GFC.GFCMaintenance.set_report_release_form(document.getElementById('txtCardNo').value, this.show_report_callback); 
   }
   
   this.btnTermsCondition_click = function()
   {
        // Terms and Condition printing here
        IRSWorld.GFC.GFCMaintenance.set_report_term_and_condition(this.show_report_callback);        
   }
   
   this.show_report_callback = function(response)
    {
        if (response.error != null){alert('Failed to load report');}
        else
        {            
           window.open('../Reports/Pages/Report_Viewer.aspx' + response.value.toString(),1) 
        }
    }
   
   this.btnGETCK_click = function()
   {
        if (document.getElementById('ddlCurr').value != null && document.getElementById('ddlCurr').value != '')
        {
            var curr = document.getElementById('ddlCurr').value.toString();
            this.disable('btnGETCK');
            this.disable('ddlCurr');
            IRSWorld.GFC.GFCMaintenance.GetCKAccount(curr, this.btnGETCK_click_callback);         
        }
        else
        {
            alert('Please select account currency'); 
        }   
        
   }
   
   this.btnGETCK_click_callback = function(response)
   {
        if(response.error != null)
        {
            icls.enable('btnGETCK');
            icls.enable('ddlCurr');  
            alert(response.error.Message.toString().replace('System.Exception ', ''));           
        }
        else
        {
            if (response.value != null && response.value != '')
            {  
                //document.getElementById('txtAccountNo').innerText = response.value;
                setText(document.getElementById('txtAccountNo'), response.value);              
            }
            else
            {
                alert('Available CK for this currency has runout.\r\nPlease call ORD at local 4617/4566') 
                icls.enable('btnGETCK');
                icls.enable('ddlCurr');
            }
        }   
   }
   
   this.btnACancel_click = function()
   {
       if (confirm('Are you sure you want to cancel this account?'))
       {
           if (document.getElementById('txtAccountNo').value != null && document.getElementById('txtAccountNo').value != 'undefined' && document.getElementById('txtAccountNo').value != "") 
           {
                var ckAccount = document.getElementById('txtAccountNo').value;
                IRSWorld.GFC.GFCMaintenance.UnlockCK(ckAccount, this.btnACancel_click_callback); 
           }
       } 
       
   }
   
   this.btnACancel_click_callback = function(response)
   {
        if(response.error != null)
        {
            alert(response.error.Message.toString().replace('System.Exception ', ''));
        }
        else
        {
             //document.getElementById('txtAccountNo').innerText = '';
             setText(document.getElementById('txtAccountNo'), '');
             icls.enable('btnGETCK');
             icls.enable('ddlCurr');
        }
   }
   
   this.btnSubmit_click = function()
   {
        if(confirm('Are you sure you want to submit this account?'))
        {
            var CIFID = this.CIFID;
            var CKNum = document.getElementById('txtAccountNo').value;        
            var CurrCode = document.getElementById('ddlCurr').value;        
           
            showpanel('divPopProcess'); 
            IRSWorld.GFC.GFCMaintenance.submitCK(CIFID, CKNum, CurrCode, this.btnSubmit_click_callback); 
        }
   }
   
   this.btnSubmit_click_callback = function(response)
   {
        hidepanel('divPopProcess');
    
        if(response.error != null)
        {
            alert(response.error.Message.toString().replace('System.Exception ', ''));
        }
        else
        {            
            icls.invisible('SUBMITCANCELBTN');
            alert('Availment is successful'); 
           
            icls.checkCKlimit();
        }
   }
   
   this.ddlClientType_on_change = function()
   {
        this.ClientType = document.getElementById('ddlClientType').value;
   } 
   
   this.btnShowOtherDetails_click = function()
   {
        if (document.getElementById('btnShowOtherDetails').value == "Show Other Details")
        {
            //document.getElementById('btnShowOtherDetails').innerText = "Hide Other Details";
            setText(document.getElementById('btnShowOtherDetails'), "Hide Other Details");
            this.visible('GRIDOTHERDETAILS');
            
//            if (this.Mode == 6){ this.visible('GRID2'); }
//            else { this.invisible('GRID2'); }
        }
        else
        {
            //document.getElementById('btnShowOtherDetails').innerText = "Show Other Details";
            setText(document.getElementById('btnShowOtherDetails'), "Show Other Details");  
            this.invisible('GRIDOTHERDETAILS');
        }   
   }
    
   // GRID VIEWER    
    this.process_viewer_benef = function(param)
    { 
        this.grid = new grid_viewer();
        var REMCIFID;
        if (this.RemData != null && this.RemData != 'undefiined')
        {
             REMCIFID = (this.RemData.Tables[0].Rows[0].CIFID == null) ? '' : this.RemData.Tables[0].Rows[0].CIFID.toString();
        }  
        
        this.grid.sql_source     = "sp_GFCSearch~" + this.Mode.toString() + "~" + REMCIFID + "~"; 
        this.grid.div_container  = document.getElementById("CIFIDGRIDBENEF");
        this.grid.datarow_column = 0;
        this.grid.page_size = 5;
        this.grid.process_ajax(param);
        this.grid = null;
    } 
    // GRID BENEF CARD HISTORY
    this.process_viewer_cardhistory = function(param, CIFID)
    { 
        this.grid = new grid_viewer();
       
        this.grid.sql_source     = "sp_gfc_get_card_history~" + CIFID + "~"; 
        this.grid.div_container  = document.getElementById("GRIDCARDHISTORY");
        this.grid.datarow_column = 0;
        this.grid.page_size = 100;
        this.grid.process_ajax(param);                               
        this.grid = null;
    } 
   
    // GRID BENEF ADDRESS
    this.process_viewer_address = function(param, CIFID)
    { 
        this.grid = new grid_viewer();
       
        this.grid.sql_source     = "sp_gfc_get_address~" + CIFID + "~"; 
        this.grid.div_container  = document.getElementById("GRIDBENEFADDRESS");
        this.grid.datarow_column = 0;
        this.grid.page_size = 100;
        this.grid.process_ajax(param);                               
        this.grid = null;
    }  
    
    // GRID BENEF ADDRESS
    this.process_viewer_contacts = function(param, CIFID)
    { 
        this.grid = new grid_viewer();
       
        this.grid.sql_source     = "sp_gfc_get_contacts~" + CIFID + "~"; 
        this.grid.div_container  = document.getElementById("GRIDBENEFCONTACTS");
        this.grid.datarow_column = 0;
        this.grid.page_size = 100;
        this.grid.process_ajax(param);                               
        this.grid = null;
    }  
     
    // GRID Card Batch 
    this.process_viewer_batch = function(param)
    { 
        this.grid = new grid_viewer();
                
        this.grid.sql_source     = "sp_get_GFC_BatchCards~" + document.getElementById("ddlGroups").value + "~"; 
        this.grid.div_container  = document.getElementById("dvCARDLIST");
        this.grid.datarow_column = 3;
        this.grid.page_size = 10;
        this.grid.process_ajax(param);
        this.grid = null;
    }  
   
    this.backToRemInfo = function()
    {
        if (this.RemData != null && this.RemData != "undefined")
        {   
            //document.getElementById('tdMainInfo').innerText = "Remitter Information";
            setText(document.getElementById('tdMainInfo'), "Remitter Information"); 
            this.viewMode(7); 
            this.popFields(this.RemData);
            this.process_viewer_benef('ajx_loading$');
        }
    }
   
    this.popRemMinInfo = function()
    {    
        if (this.RemData != null && this.RemData != "undefined")
        {        
            var dr = this.RemData.Tables[0].Rows[0]; 
            //document.getElementById('lblCustLName').innerText = (dr.LastName == null) ? "" : dr.LastName.toString();
            setText(document.getElementById('lblCustLName'), (dr.LastName == null) ? "" : dr.LastName.toString());
            //document.getElementById('lblCustFName').innerText = (dr.FirstName == null) ? "" : dr.FirstName.toString();
            setText(document.getElementById('lblCustFName'), (dr.FirstName == null) ? "" : dr.FirstName.toString());
            //document.getElementById('lblCustMName').innerText = (dr.MiddleName == null) ? "" : dr.MiddleName.toString();
            setText(document.getElementById('lblCustMName'), (dr.MiddleName == null) ? "" : dr.MiddleName.toString());
            //document.getElementById('lblCIFID').innerText = (dr.CIFID == null) ? "" : dr.CIFID.toString();
            setText(document.getElementById('lblCIFID'), (dr.CIFID == null) ? "" : dr.CIFID.toString());
        }     
    }
   
    this.ddlGroups_OnChange = function()
    {      
         if (document.getElementById('ddlGroups').value != null && document.getElementById('ddlGroups').value != "" && document.getElementById('ddlGroups').value != "undefined")
         {
            this.GridMode = 1;            
            this.process_viewer_batch('ajx_loading$');
         }    
    }
   
    this.popCardAndAccounts = function(SEQID)
    {
        IRSWorld.GFC.GFCMaintenance.GetBatchDataDetails(SEQID, this.popCardAndAccounts_callback);
    }
    
    this.popCardAndAccounts_callback = function(response)
    {
        if (response.error != null)
        {
            alert(response.error.Message); 
        }
        else
        {
            var ds = response.value;
             
            //document.getElementById('txtAccountNo').innerText = ds.Rows[0].AccountNum.toString();
            setText(document.getElementById('txtAccountNo'), ds.Rows[0].AccountNum.toString());
            document.getElementById('ddlCurr').selectedIndex = icls.getIndex('ddlCurr', ds.Rows[0].CurrCode.toString());
            //document.getElementById('txtCardNo').innerText = ds.Rows[0].CardNo.toString();
            setText(document.getElementById('txtCardNo'), ds.Rows[0].CardNo.toString());
            //document.getElementById('txtCardType').innerText = ds.Rows[0].CardType.toString();
            setText(document.getElementById('txtCardType'), ds.Rows[0].CardType.toString());
        }
    }   
   
    this.checkCardlimit = function()
    {
        IRSWorld.GFC.GFCMaintenance.checkLimit('card', this.checkCardlimit_callback);
    }  
   
    this.checkCardlimit_callback = function(response)
    {
        if (response.error != null)
        {
            alert(response.error.Message); 
        }
        else
        {
             if (response.value == true)
             {
                alert('Number of GFC Cards has reached its limit, please call ORD at local 4617/4566');
             }
        }    
    } 
   
    this.checkCKlimit = function()
    {
        IRSWorld.GFC.GFCMaintenance.checkLimit('ck', this.checkCKlimit_callback);
    }  
   
    this.checkCKlimit_callback = function(response)           
    {
        if (response.error != null)
        {
            alert(response.error.Message); 
        }
        else
        {
             if (response.value == true)
             {
                alert('Number of GFC CK number has reached its limit, please call ORD at local 4617/4566');
             }
        }    
    }  
   
    this.change_other_grid_mode = function(mode)
    {
        this.OtherGridMode = mode; 
    }  
   
    this.on_key_press = function()
    {
        if (document.getElementById("CIFSearch").style.display == 'block')
        {
            if (window.event.keyCode == 13)
            {
                document.getElementById('btnSearch').click();                
            }
        } 
    }  
   
    this.AddNewCIF = function()
    {
        document.getElementById('ddlClientType').style.display = 'none';     
        show_frame_dialog('../Internet/InternetRemitter.aspx?isRemit=gfc', 'cifclose', '100%', '100%', '0');
    } 
   
    this.showDetails = function(param)
    {
         IRSWorld.GFC.GFCMaintenance.get_rem_bene_seq_id(param, this.get_rem_bene_seq_id_callback);
    }  
   
    this.get_rem_bene_seq_id_callback = function(response)
    {
        if (response.error != null)
        {
            alert("Failed to get sequence id"); 
        }  
        else
        {
            var param = response.value.toString().split('|');   
            
            icls.rem_seq_id = param[0];
            icls.bene_seq_id = param[1];  
                   
            IRSWorld.GFC.GFCMaintenance.getSearchRemDetails(param[0], icls.update_rem_datatable_callback);   
        }  
    }  
   
    this.update_rem_datatable_callback = function(response)
    {
        if (response.error != null)
        {
            alert('Failed to get remitter info'); 
        }  
        else
        {
             icls.RemData  =  response.value;
             icls.popRemMinInfo();
             
             icls.AppSEQID = icls.bene_seq_id;
             icls.populateInfo(icls.bene_seq_id, "false");
        }  
    }  
             
}

function cifclose()
{
    if (GFC_Main != null)
    {
        if (self.returnValue != null && self.returnValue != "0" && self.returnValue != "" && self.returnValue != "|")
        {  
            //GFC_Main.showDetails('1752030610005264|1752030610007680');
            GFC_Main.showDetails(self.returnValue); 
        }
    }
    
    document.getElementById('ddlClientType').style.display = 'block';  
}
    