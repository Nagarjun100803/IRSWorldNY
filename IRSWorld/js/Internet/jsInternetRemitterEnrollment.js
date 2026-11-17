function show_idology_questionaire(Show)
{
    var divname = 'divQandA';
   
    if (Show == true)
    {
        var div = document.getElementById(divname);
        
        divmask = new maskpanel();
        divmask.show();
        document.getElementById(divname).style.display = "block";
       
        div.align        = "center";
	    div.style.height = "auto";		
	    div.style.width  = parseInt(parent.screen.availWidth)  - 800;
	    div.style.top    = 40;
	    div.style.left   = (parseInt(screen.width) - parseInt(div.style.width)) / 2 - 100; 
    }
    else
    {
        document.getElementById(divname).style.display = "none";
        try
        {
            divmask.hide();
        }
        catch(e){}
    }
}

function InternetRemitterEnrollment()
{
    
    this.SEQ_ID          =   null;
    var CIFID            =   null;
	var Lastname         =   null; 
	var Firstname        =   null;
	var Middlename       =   null;
	var Gender           =   null;
	var Birthdate        =   null;
	var BirthPlace       =   null;
	var Occupation       =   null;
	var TelNo            =   null;
	var MobileNo         =   null;
	var Email            =   null;
	var CompanyName      =   null;
	var Address1         =   null;
	var Address2         =   null;
	var Address3         =   null;
	var Address4         =   null;
	var Address5         =   null;
	var Address6         =   null;
	var ZipCode          =   null;
	var CountryCode      =   null;
	var POBox            =   null;
	
	var Address1_Com         =   null;
	var Address2_Com         =   null;
	var Address3_Com         =   null;
	var Address4_Com         =   null;
	var Address5_Com         =   null;
	var Address6_Com         =   null;
	var ZipCode_Com          =   null;
	var CountryCode_Com      =   null;
	var POBox_Com            =   null;
	
	var Address1_Mail         =   null;
	var Address2_Mail         =   null;
	var Address3_Mail         =   null;
	var Address4_Mail         =   null;
	var Address5_Mail         =   null;
	var Address6_Mail         =   null;
	var ZipCode_Mail          =   null;	
	var CountryCode_Mail      =   null;
	var POBox_Mail            =   null;
	
	var BranchCode       =   null;
	var AccountNo        =   null;
	var AccountCurrency  =   null;	
	var UserID           =   null;
	
	var IDNo             =   null;
	var SSN              =   null;
	var IDType           =   null;
	var IDExpiry         =   null;
	
	// For Idology
	var myarray          =   null;
	var qcount           =   null;
	var maxAnswer        =   null;
	var reqAnswered      =   null;
    var idology_number   =   null;
    var idology_enabled  =   null;
    
    this.init = function()
    {
       if (hasQueryStrings(window))
       {
            this.BranchCode = getQueryString(window, 'BranchCode');
            fixdim();
            this.populate_dropdown();       
            this.check_idology_enabled();
            this.activate_text(true);
            
            if(this.BranchCode == "USW")
            {
                document.getElementById("ddCountry").value = 18;
            }
            else if(this.BranchCode == "CAW")
            {
                document.getElementById("ddCountry").value = 8;
            }
            
            
       }
//        else
//        {
//            document.getElementById('Main').style.display = 'none';
//            alert('Invalid Branch Code');
//        }
                
//        var up_msg = IRSWorld.Internet.InternetRemitterEnrollment.getUploadMessage().value;
//        if (up_msg != null && up_msg != "")
//        {
//            var data = IRSWorld.Internet.InternetRemitterEnrollment.getCurrData().value;
//            if (data != null)
//            {
//                this.re_pop_data(data);
//                set_read_only(document.getElementById('tblRemInfo'), true);
//            }
//            
//            if (up_msg == "success")
//            {
//                alert("Request was successfuly sent for approval.\nYou will receive an email notification to your registered email address\nthe status of your request\nThank you very much.")
//                
//            }
//            else
//            {
//                alert(up_msg);
//            }
//            
//        }
    }
    
    this.check_if_user_exist = function(e)
    {
        if(e.keyCode == 13)
        {
            var uname = document.getElementById('txtUserID').value;
            if(uname == "")
            {
                alert('Username field is empty');
                return;
            }
            if(uname.length < 8)
            {
                alert('Username must be greater than 8 characters');
                return;
            }
            
            
            if (this.user_exists(document.getElementById("txtUserID").value))
            {
                if(confirm("Would you like to reset your password?"))
                {
                    var retval = IRSWorld.Internet.InternetRemitterEnrollment.reset_password(document.getElementById("txtUserID").value);
                    alert('Password successfully changed');
                    document.location.href = "../Main/IRSMain.aspx?load=../Admin/Login.aspx";
                }
                else
                {
                    alert("Please type another user ID");
                    return;
                }
            }
            else
            {
                document.getElementById("txtUserID").disabled = true;
                this.activate_text(false);
                alert('Username available, Please fill out the needed Information');
            }
            
            this.activate_text(false); 
        }
    }
    
    this.activate_text = function(b)
    {
        if(b)
        {
            document.getElementById("txtLName").disabled       =    'disabled';    
	        document.getElementById("txtFName").disabled       =     'disabled';    
	        document.getElementById("txtMName").disabled       =     'disabled';    
	        //document.getElementById("ddGender").disabled       =    'disabled';    
	        document.getElementById("txtBirthDate").disabled   =    'disabled';    
	        //document.getElementById("txtBirthPlace").disabled  =     'disabled';    
	        document.getElementById("ddOccupation").disabled   =    'disabled';    
	        document.getElementById("txtTelNo").disabled       =    'disabled';    
	        document.getElementById("txtMobileNo").disabled    =     'disabled';    
	        document.getElementById("txtEmailAdd").disabled    =     'disabled';    
	        document.getElementById("txtEmpName").disabled     =     'disabled';    
    	    
	        document.getElementById("txtRoomNo").disabled      =    'disabled';    
	        document.getElementById("txtStreet").disabled      =     'disabled';    
//	        document.getElementById("txtDistrict").disabled    =     'disabled';    
	        document.getElementById("txtCity").disabled        =     'disabled';    
	        document.getElementById("ddState").disabled       =     'disabled';    
//	        document.getElementById("txtRegion").disabled      =     'disabled';    
	        document.getElementById("txtZipCode").disabled     =     'disabled';    
	        document.getElementById("ddCountry").disabled      =     'disabled';    
//	        document.getElementById("txtPOBox").disabled       =    'disabled';    
    	    
//	        document.getElementById("txtRoomNo_Com").disabled      =     'disabled';    
//	        document.getElementById("txtStreet_Com").disabled      =     'disabled';    
//	        document.getElementById("txtDistrict_Com").disabled    =     'disabled';    
//	        document.getElementById("txtCity_Com").disabled        =     'disabled';    
//	        document.getElementById("txtState_Com").disabled       =     'disabled';    
//	        document.getElementById("txtRegion_Com").disabled      =     'disabled';    
//	        document.getElementById("txtZipCode_Com").disabled     =     'disabled';    
//	        document.getElementById("ddCountry_Com").disabled      =     'disabled';    
//	        document.getElementById("txtPOBox_Com").disabled       =     'disabled';    
//    	    
//	        document.getElementById("txtRoomNo_Mail").disabled      =     'disabled';    
//	        document.getElementById("txtStreet_Mail").disabled      =     'disabled';    
//	        document.getElementById("txtDistrict_Mail").disabled    =    'disabled';    
//	        document.getElementById("txtCity_Mail").disabled        =     'disabled';    
//	        document.getElementById("txtState_Mail").disabled       =     'disabled';    
//	        document.getElementById("txtRegion_Mail").disabled      =     'disabled';    
//	        document.getElementById("txtZipCode_Mail").disabled     =    'disabled';    
//	        document.getElementById("ddCountry_Mail").disabled      =     'disabled';    
//	        document.getElementById("txtPOBox_Mail").disabled       =     'disabled';
            document.getElementById("ddlSalutation").disabled = true;
            document.getElementById("ddIDType").disabled = true;
            document.getElementById("File1").disabled = true;
            document.getElementById("File2").disabled = true;
            document.getElementById("File3").disabled = true;    
	    }
	    else
	    {
	    
            document.getElementById("txtLName").disabled       =    "";    
	        document.getElementById("txtFName").disabled       =     "";    
	        document.getElementById("txtMName").disabled       =     "";    
	        //document.getElementById("ddGender").disabled       =    "";    
	        document.getElementById("txtBirthDate").disabled   =    "";    
	        //document.getElementById("txtBirthPlace").disabled  =     "";    
	        document.getElementById("ddOccupation").disabled   =    "";    
	        document.getElementById("txtTelNo").disabled       =    "";    
	        document.getElementById("txtMobileNo").disabled    =     "";    
	        document.getElementById("txtEmailAdd").disabled    =     "";    
	        document.getElementById("txtEmpName").disabled     =     "";    
    	    
	        document.getElementById("txtRoomNo").disabled      =    "";    
	        document.getElementById("txtStreet").disabled      =     "";    
//          document.getElementById("txtDistrict").disabled    =     "";    
	        document.getElementById("txtCity").disabled        =     "";    
	        document.getElementById("ddState").disabled       =     "";    
//	        document.getElementById("txtRegion").disabled      =     "";    
	        document.getElementById("txtZipCode").disabled     =     "";    
	        document.getElementById("ddCountry").disabled      =     "";    
//	        document.getElementById("txtPOBox").disabled       =    "";    
    	    
//	        document.getElementById("txtRoomNo_Com").disabled      =     "";    
//	        document.getElementById("txtStreet_Com").disabled      =     "";    
//	        document.getElementById("txtDistrict_Com").disabled    =     "";    
//	        document.getElementById("txtCity_Com").disabled        =     "";    
//	        document.getElementById("txtState_Com").disabled       =     "";    
//	        document.getElementById("txtRegion_Com").disabled      =     "";    
//	        document.getElementById("txtZipCode_Com").disabled     =     "";    
//	        document.getElementById("ddCountry_Com").disabled      =     "";    
//	        document.getElementById("txtPOBox_Com").disabled       =     "";    
//    	    
//	        document.getElementById("txtRoomNo_Mail").disabled      =     "";    
//	        document.getElementById("txtStreet_Mail").disabled      =     "";    
//	        document.getElementById("txtDistrict_Mail").disabled    =    "";    
//	        document.getElementById("txtCity_Mail").disabled        =     "";    
//	        document.getElementById("txtState_Mail").disabled       =     "";    
//	        document.getElementById("txtRegion_Mail").disabled      =     "";    
//	        document.getElementById("txtZipCode_Mail").disabled     =    "";    
//	        document.getElementById("ddCountry_Mail").disabled      =     "";    
//	        document.getElementById("txtPOBox_Mail").disabled       =     ""; 
            document.getElementById("ddlSalutation").disabled = false;
            document.getElementById("ddIDType").disabled = false; 
            document.getElementById("File1").disabled = false;
            document.getElementById("File2").disabled = false;
            document.getElementById("File3").disabled = false;    

	    }
	    //document.getElementById("ddBranchCode").value   =    b;
    }
    

//    this.re_pop_data = function(data)
//    {
//        document.getElementById("txtUserID").value      =    data[0];
//	    document.getElementById("txtLName").value       =    data[1];    
//	    document.getElementById("txtFName").value       =    data[2];
//	    document.getElementById("txtMName").value       =    data[3];
//	    //document.getElementById("ddGender").value       =    data[4];
//	    document.getElementById("txtBirthDate").value   =    data[5];
//	    //document.getElementById("txtBirthPlace").value  =    data[6];
//	    document.getElementById("ddOccupation").value   =    data[7];
//	    document.getElementById("txtTelNo").value       =    data[8];
//	    document.getElementById("txtMobileNo").value    =    data[9];
//	    document.getElementById("txtEmailAdd").value    =    data[10];
//	    document.getElementById("txtEmpName").value     =    data[11];
//	    
//	    document.getElementById("txtRoomNo").value      =    data[12];
//	    document.getElementById("txtStreet").value      =    data[13];
//	    document.getElementById("txtDistrict").value    =    data[14];
//	    document.getElementById("txtCity").value        =    data[15];
//	    document.getElementById("ddState").value       =    data[16];
//	    document.getElementById("txtRegion").value      =    data[17];
//	    document.getElementById("txtZipCode").value     =    data[18];
//	    document.getElementById("ddCountry").value      =    data[19];
//	    document.getElementById("txtPOBox").value       =    data[23];
//	    
////	    document.getElementById("txtRoomNo_Com").value      =    data[24];
////	    document.getElementById("txtStreet_Com").value      =    data[25];
////	    document.getElementById("txtDistrict_Com").value    =    data[26];
////	    document.getElementById("txtCity_Com").value        =    data[27];
////	    document.getElementById("txtState_Com").value       =    data[28];
////	    document.getElementById("txtRegion_Com").value      =    data[29];
////	    document.getElementById("txtZipCode_Com").value     =    data[30];
////	    document.getElementById("ddCountry_Com").value      =    data[31];
////	    document.getElementById("txtPOBox_Com").value       =    data[32]; 
////	    
////	    document.getElementById("txtRoomNo_Mail").value      =    data[33];
////	    document.getElementById("txtStreet_Mail").value      =    data[34];
////	    document.getElementById("txtDistrict_Mail").value    =    data[35];
////	    document.getElementById("txtCity_Mail").value        =    data[36];
////	    document.getElementById("txtState_Mail").value       =    data[37];
////	    document.getElementById("txtRegion_Mail").value      =    data[38];
////	    document.getElementById("txtZipCode_Mail").value     =    data[39];
////	    document.getElementById("ddCountry_Mail").value      =    data[40];
////	    document.getElementById("txtPOBox_Mail").value       =    data[41];
//	      
//	    document.getElementById("ddBranchCode").value   =    data[42];
//	    //this.AccountNo        =   '1234'=    data[21];
//	    //this.AccountCurrency  =   '1'=    data[22];
//	           
//    }

    this.populate_dropdown = function()
    {
        this.dropdown_country();
        this.dropdown_Occupation();
        //this.dropdown_gender();
        this.dropdown_idtype();
        this.dropdown_states();
        //this.dropdown_BranchCode();
    }
    
	this.dropdown_country = function()
	{
	    var result = IRSWorld.Internet.InternetRemitterEnrollment.getdropdown_title("Country",this.BranchCode,"10");
        if (result != null && result != undefined)
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddCountry",  result.value, "FldValue", "FldDescription");
//            this.commons.to_load_dropdownlist("ddCountry_Com",  result.value, "FldValue", "FldDescription");
//            this.commons.to_load_dropdownlist("ddCountry_Mail",  result.value, "FldValue", "FldDescription");
            this.commons = null;
        }  	    
	}
	
		
    this.dropdown_states = function()
	{
	    var result = IRSWorld.Internet.InternetRemitterEnrollment.getdropdown_title("States",this.BranchCode,"10");
        if (result != null && result != undefined)
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddState",  result.value, "FldCode", "FldDescription");
            this.commons.to_load_dropdownlist("ddlStateIssuance",  result.value, "FldCode", "FldDescription");
            this.commons = null;
        }  	    
	}	

//    this.dropdown_gender = function()
//	{
//	    var result = IRSWorld.Internet.InternetRemitterEnrollment.getdropdown_title("Gender");
//        if (result != null && result != undefined)
//        {
//            this.commons = new common_method();
//            this.commons.to_load_dropdownlist("ddGender",  result.value, "FldValue", "FldDescription");
//            this.commons = null;
//        } 	    
//	}	

    this.dropdown_BranchCode = function()
	{
	    var result = IRSWorld.Internet.InternetRemitterEnrollment.getdropdown_branch();
	    if (result != null && result != undefined)
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddBranchCode",  result.value, "FldValue", "FldDescription");
            this.commons = null;
        }  
	}
		
	this.dropdown_idtype = function()
	{
	    var result = IRSWorld.Internet.InternetRemitterEnrollment.getdropdown_title("IDType",this.BranchCode,"10");
	    if (result != null && result != undefined)
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddIDType",  result.value, "FldValue", "FldDescription");
            this.commons = null;
        }  
	}  

    this.dropdown_Occupation = function()
    {
        var result = IRSWorld.Internet.InternetRemitterEnrollment.getdropdown_title("Occupation",this.BranchCode,"1");
        if (result != null && result != undefined)
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddOccupation",  result.value, "FldValue", "FldDescription");
            this.commons = null;
        }         
    }
    
    this.check_idology_enabled = function()
    {
        this.idology_enabled = false;
        
        var result = IRSWorld.Internet.InternetRemitterEnrollment.check_idology_enabled(this.BranchCode );
        
        if (result.error != null)
        {
            alert(result.error.Message);
        }
        else
        {
            this.idology_enabled = result.value;
        } 
    }
    
    this.set_plist = function()
    {
        this.UserID           =   document.getElementById("txtUserID").value; 
	    this.Lastname         =   document.getElementById("txtLName").value; 
	    this.Firstname        =   document.getElementById("txtFName").value;
	    this.Middlename       =   document.getElementById("txtMName").value;
	    this.Birthdate        =   IRE.parseBdate(document.getElementById("txtBirthDate").value)//document.getElementById("txtBirthDate").value;
	    this.Occupation       =   (document.getElementById("ddOccupation").value == 'null') ? null : document.getElementById("ddOccupation").value; //document.getElementById("ddOccupation").value;
	    this.TelNo            =   document.getElementById("txtTelNo").value;
	    this.MobileNo         =   document.getElementById("txtMobileNo").value;
	    this.Email            =   document.getElementById("txtEmailAdd").value;	    
	    this.CompanyName      =   document.getElementById("txtEmpName").value;
	    
	    this.Address1         =   document.getElementById("txtRoomNo").value;
	    this.Address2         =   document.getElementById("txtStreet").value;
	    this.Address3         =   null;//document.getElementById("txtDistrict").value;
	    this.Address4         =   document.getElementById("txtCity").value;
	    this.Address5         =   (document.getElementById("ddState").value == 'null') ? null : document.getElementById("ddState").value;
	    this.Address6         =   null;//document.getElementById("txtRegion").value;
	    this.ZipCode          =   document.getElementById("txtZipCode").value;
	    this.CountryCode      =   (document.getElementById("ddCountry").value == 'null') ? null : document.getElementById("ddCountry").value;
	    this.POBox            =   null;//document.getElementById("txtPOBox").value;
	    
	  
	    
	    this.BranchCode       //=   (document.getElementById("ddBranchCode").value == 'null') ? null : document.getElementById("ddBranchCode").value;//document.getElementById("ddBranchCode").value;
	    this.AccountNo        =   "123";
	    this.AccountCurrency  =   "1";
	    
	    this.IDNo                   =   document.getElementById("txtIDNo").value;
	    this.SSN                    =   document.getElementById("txtSSSIDNo").value;
	    this.IDType                 =   (document.getElementById("ddIDType").value == 'null') ? null : document.getElementById("ddIDType").value;
	    this.IDExpiry               =   (document.getElementById("txtIDExpiry").value == "") ? "1/1/1900" : document.getElementById("txtIDExpiry").value;
    }
    
    this.create_plist = function()
    {

        var data_row = new Array(); 
        data_row[data_row.length] = this.UserID;        
        data_row[data_row.length] = this.Lastname; 
	    data_row[data_row.length] = this.Firstname;
	    data_row[data_row.length] = this.Middlename;
	    data_row[data_row.length] = this.Gender;
	    data_row[data_row.length] = this.Birthdate;
	    data_row[data_row.length] = this.BirthPlace;
	    data_row[data_row.length] = this.Occupation;
	    data_row[data_row.length] = this.TelNo;
	    data_row[data_row.length] = this.MobileNo;
	    data_row[data_row.length] = this.Email;
	    data_row[data_row.length] = this.CompanyName;
	    
	    data_row[data_row.length] = this.Address1;
	    data_row[data_row.length] = this.Address2;
	    data_row[data_row.length] = this.Address3;
	    data_row[data_row.length] = this.Address4;
	    data_row[data_row.length] = this.Address5;
	    data_row[data_row.length] = this.Address6;
	    data_row[data_row.length] = this.ZipCode;
	    data_row[data_row.length] = this.CountryCode;
	    data_row[data_row.length] = this.POBox;
	    
	    data_row[data_row.length] = this.Address1_Com;
	    data_row[data_row.length] = this.Address2_Com;
	    data_row[data_row.length] = this.Address3_Com;
	    data_row[data_row.length] = this.Address4_Com;
	    data_row[data_row.length] = this.Address5_Com;
	    data_row[data_row.length] = this.Address6_Com;
	    data_row[data_row.length] = this.ZipCode_Com;
	    data_row[data_row.length] = this.CountryCode_Com;
	    data_row[data_row.length] = this.POBox_Com;
	    
	    data_row[data_row.length] = this.Address1_Mail;
	    data_row[data_row.length] = this.Address2_Mail;
	    data_row[data_row.length] = this.Address3_Mail;
	    data_row[data_row.length] = this.Address4_Mail;
	    data_row[data_row.length] = this.Address5_Mail;
	    data_row[data_row.length] = this.Address6_Mail;
	    data_row[data_row.length] = this.ZipCode_Mail;
	    data_row[data_row.length] = this.CountryCode_Mail;
	    data_row[data_row.length] = this.POBox_Mail;
	    
	    data_row[data_row.length] = this.BranchCode;
	    data_row[data_row.length] = this.AccountNo;
	    data_row[data_row.length] = this.AccountCurrency;
	    
	    data_row[data_row.length] = this.IDNo;
	    data_row[data_row.length] = this.IDType;
	    data_row[data_row.length] = (this.IDExpiry == "") ? "1/1/1900" : this.IDExpiry;
	    
	    data_row[data_row.length] = document.getElementById("File1").value;
  	    data_row[data_row.length] = this.SSN;

//        data_row[data_row.length] = IRSWorld.Internet.InternetRemitterEnrollment.getPassword(this.Firstname +" "+ this.Middlename +" "+  this.Lastname,this.UserID).value;
//	    data_row[data_row.length] = document.getElementById("File3").value;
    
	    return data_row;
    }
        
    this.btnSubmit_click = function()
    {
    
        if (!this.validate_input())
        {
            return;
        }
    
//        if(!this.show_questionaire(true))
//        {
//            alert("Cannot proceed with the enrollment");
//            return;
//        }
    
       
        
        this.set_plist();
        
        if (this.idology_enabled == true)
        {
            this.show_questionaire(true);
        }
        else
        {
            this.commit(this.create_plist());
        }
        
        
    }
    
    this.showTransactionLoading = function(message, ishide)
    {
        if (ishide == true)
        {
            document.getElementById("lblLoadingMsg").innerHTML = "";
            this.hidePanel("divPopProcess");
        }
        else
        {
            new common_method().to_show_loading_message('divLoadingMsg');
            document.getElementById("lblLoadingMsg").innerHTML = message;
            this.showPanel("divPopProcess");
        }
    }
    
    this.showPanel = function(divName)
    {
        var div = document.getElementById(divName);
        
        divmask = new maskpanel();
        divmask.show();
        internet_page.showControl(divName, true);
        
        div.style.top = (((document.documentElement.clientHeight / 2) + document.documentElement.scrollTop) - (div.offsetHeight / 2)) + "px";
        div.style.left = (((document.documentElement.clientWidth / 2) + document.documentElement.scrollLeft) - (div.offsetWidth / 2)) + "px";
    }
    
    this.commit = function(plist)
    {
        AjaxPro.timeoutPeriod = 1000 * 1000;

        AjaxPro.onTimeout = function()
        {
            alert("THE SERVER STOPPED RESPONDING. PLEASE TRY AGAIN");
        }
        
        document.getElementById("btnSubmit").disabled = true;
        IRSWorld.Internet.InternetRemitterEnrollment.submit_record(plist, this.commit_callback);
    }
    
    
    
    
    this.commit_callback = function(res)
    {
        if (res.error != null)
        {
            alert(res.error.Message);
        }
        else
        {
            document.getElementById('HPath').value = res.value;
            if (res.value != null && res.value != "")
            {
                set_read_only(document.getElementById('tblRemInfo'), true);
                //document.getElementById('btnPost').click();
                alert("The Registration has been Successfully Saved!.\n");
                //this.showTransactionLoading("",true);   
            }
            else
            {
                alert("Unable to submit your request, please try again.");
            }
            //set_read_only(document.getElementById('tblRemInfo'), true);
            //ajax_show_status(false);   
            document.getElementById("File1").disabled = true;
            document.getElementById("File2").disabled = true;
            document.getElementById("File3").disabled = true;
            document.getElementById("btnSubmit").disabled = true;
            //document.getElementById("btnCheck").disabled = false;
//            
//            alert("Request was successfuly sent for approval.\nYou will receive an email notification to your registered email address\nthe status of your request\nThank you very much.")
//            document.irs_login.submit();
//            IRSWorld.Internet.InternetRemitterEnrollment.sendForVerificationEmail();
            IRE.getCIFID();
            var btnInsertUserRec = document.getElementById("btnInsertUserRec");
            btnInsertUserRec.click();
            IRE.clear_All();
           
            
        }
    }
    
    
    this.validate_input = function()
    {
        this.generic = new generic_method();  
        var error_input = "";
        if (document.getElementById("txtUserID").value == undefined || document.getElementById("txtUserID").value == null || document.getElementById("txtUserID").value == "")
        {
            error_input += "User ID is required.\n";
        }
        //if (this.user_exists(document.getElementById("txtUserID").value))
        //{
          //  error_input += "User ID " + document.getElementById("txtUserID").value + " exists!\n";
        //}
        if (document.getElementById("txtLName").value == undefined || document.getElementById("txtLName").value == null || document.getElementById("txtLName").value == "")
        {
            error_input += "Last name is required.\n";
        }
	    if (this.generic.is_alpha(document.getElementById("txtLName").value) == false)
        {           
            error_input += "Last name contains invalid character(s).\n";
        }
        if (document.getElementById("txtFName").value == undefined || document.getElementById("txtFName").value == null || document.getElementById("txtFName").value == "")
        {
            error_input += "First name is required.\n";
        }        
	    if (this.generic.is_alpha(document.getElementById("txtFName").value) == false)
        {           
            error_input += "First name contains invalid character(s).\n";
        }
	    if (this.generic.is_alpha(document.getElementById("txtMName").value) == false)
        {           
            error_input += "Middle name contains invalid character(s).\n"; 
        }
        if (document.getElementById("txtBirthDate").value == undefined || document.getElementById("txtBirthDate").value == null || document.getElementById("txtBirthDate").value == "")
        {
            error_input += "Birth date is required.\n";
        }                
//	    if (this.generic.to_validate_bdate(document.getElementById("txtBirthDate").value) == false)
//        {           
//            error_input += "Invalid birth date.\n";
//        }
        else
        {
            var Age = this.generic.compute_age(IRE.parseBdate(document.getElementById("txtBirthDate").value));
            var age_limit = IRSWorld.Internet.InternetRemitterEnrollment.AgeLimit().value;
            
            if (parseFloat(Age) < age_limit)
            {
                error_input += "Client who are below "+ age_limit +" yrs old are not allowed.\n";
            }                
        }
        if (document.getElementById("txtEmailAdd").value == undefined || document.getElementById("txtEmailAdd").value == null || document.getElementById("txtEmailAdd").value == "")
        {
            error_input += "Email Address is required.\n";
        }  
	    if(this.generic.to_validate_email(document.getElementById("txtEmailAdd").value) == false)
	    {
		    error_input += "Invalid email address.\n";
	    }
	    
	    var isExist = IRSWorld.Internet.InternetRemitterEnrollment.checkEmailExist(document.getElementById("txtEmailAdd").value).value;
	    if(isExist == true)
	    {
	        error_input += "Email address already exist.\n";
	    }
	    
	    
//	    if (document.getElementById("ddBranchCode").value == undefined || document.getElementById("ddBranchCode").value == null || document.getElementById("ddBranchCode").value == "" || document.getElementById("ddBranchCode").value == "null")
//        {
//            error_input += "Branch Code is required.\n";
//        }
//        if (document.getElementById("ddGender").value == undefined || document.getElementById("ddGender").value == null || document.getElementById("ddGender").value == "" || document.getElementById("ddGender").value == "null")
//        {
//            error_input += "Gender is required.\n";
//        }  
//        if (document.getElementById("txtTelNo").value == undefined || document.getElementById("txtTelNo").value == null || document.getElementById("txtTelNo").value == "")
//        {
//            error_input += "Telephone number is required.\n";
//        }  
//        if (document.getElementById("txtMobileNo").value == undefined || document.getElementById("txtMobileNo").value == null || document.getElementById("txtMobileNo").value == "")
//        {
//            error_input += "Mobile number is required.\n";
//        }
        
        
        if (document.getElementById("txtStreet").value == undefined || document.getElementById("txtStreet").value == null || document.getElementById("txtStreet").value == "")
        {
            error_input += "No./Street is required.\n";
        }  
        if (document.getElementById("txtCity").value == undefined || document.getElementById("txtCity").value == null || document.getElementById("txtCity").value == "")
        {
            error_input += "City is Required.\n";
        }
        if (document.getElementById("ddState").value == undefined || document.getElementById("ddState").value == null || document.getElementById("ddState").value == "")
        {
            error_input += "Please select State.\n";
        }  
        if (document.getElementById("txtZipCode").value == undefined || document.getElementById("txtZipCode").value == null || document.getElementById("txtZipCode").value == "")
        {
            error_input += "ZIP Code is required.\n";
        }
        if (document.getElementById("txtIDNo").value == undefined || document.getElementById("txtIDNo").value == null || document.getElementById("txtIDNo").value == "")
        {
            error_input += "Please input ID Number.\n";
        }
        if (document.getElementById("ddIDType").value == undefined || document.getElementById("ddIDType").value == "null" || document.getElementById("ddIDType").value == "")
        {
            error_input += "ID Type is required. Please select one.\n";
        }
        if (document.getElementById("txtIDExpiry").value == undefined || document.getElementById("txtIDExpiry").value == null || document.getElementById("txtIDExpiry").value == "")
        {
            error_input += "Please input Id Expiry Date.\n";
        }
        if (document.getElementById("ddlStateIssuance").value == undefined || document.getElementById("ddlStateIssuance").value == "null" || document.getElementById("ddlStateIssuance").value == "")
        {
            error_input += "State of issuance is required. Please select one.\n";
        }

	    if (error_input != "")
	    {
	        alert(error_input);
	        return false;
	    }
	    return true;                                
    }
    
    this.btnACancel_click = function()
    {
        this.clear_All();
    }
   
    this.clear_All = function()
    {
        document.getElementById("txtUserID").value = ""; 
	    document.getElementById("txtLName").value = ""; 
	    document.getElementById("txtFName").value = "";
	    document.getElementById("txtMName").value = "";
//	    document.getElementById("ddGender").value = "null";
	    document.getElementById("txtBirthDate").value = "";
//	    document.getElementById("txtBirthPlace").value = "";
	    document.getElementById("ddOccupation").value = "null";
	    document.getElementById("txtTelNo").value = "";
	    document.getElementById("txtMobileNo").value = "";
	    document.getElementById("txtEmailAdd").value = "";
	    document.getElementById("txtEmpName").value = "";
//	    document.getElementById("txtEmpAddress").value = "";
	    
	    document.getElementById("txtRoomNo").value = "";
	    document.getElementById("txtStreet").value = "";
//	    document.getElementById("txtDistrict").value = "";
	    document.getElementById("txtCity").value = "";
	    document.getElementById("ddState").value = "null";
//	    document.getElementById("txtRegion").value = "";
	    document.getElementById("txtZipCode").value = "";
	    document.getElementById("ddCountry").value = "null";
//	    document.getElementById("txtPOBox").value = "";
	    
//	    document.getElementById("txtRoomNo_Com").value = "";
//	    document.getElementById("txtStreet_Com").value = "";
//	    document.getElementById("txtDistrict_Com").value = "";
//	    document.getElementById("txtCity_Com").value = "";
//	    document.getElementById("txtState_Com").value = "";
//	    document.getElementById("txtRegion_Com").value = "";
//	    document.getElementById("txtZipCode_Com").value = "";
//	    document.getElementById("ddCountry_Com").value = "null";
//	    document.getElementById("txtPOBox_Com").value = "";

//	    document.getElementById("txtRoomNo_Mail").value = "";
//	    document.getElementById("txtStreet_Mail").value = "";
//	    document.getElementById("txtDistrict_Mail").value = "";
//	    document.getElementById("txtCity_Mail").value = "";
//	    document.getElementById("txtState_Mail").value = "";
//	    document.getElementById("txtRegion_Mail").value = "";
//	    document.getElementById("txtZipCode_Mail").value = "";
//	    document.getElementById("ddCountry_Mail").value = "null";
//	    document.getElementById("txtPOBox_Mail").value = "";	    	    
	    
//	    document.getElementById("ddBranchCode").value = "null";
	    document.getElementById("File1").form.reset();
	    document.getElementById("File2").form.reset();
	    document.getElementById("File3").form.reset();	
        document.getElementById("ddlSalutation").selectedIndex = -1;
            
	    try{document.getElementById("txtUserID").focus()}catch(e){};
	    
	    document.getElementById("txtIDNo").value = "";
	    document.getElementById("ddIDType").value = "null";
	    document.getElementById("txtIDExpiry").value = "";
        document.getElementById("txtSSSIDNo").value = "";

    }
   
    this.txtfield_changed = function(tb)
    {
        if (tb.id == 'txtUserId')
        {
            if (this.user_exists(tb.value))
            {
                alert('User ID ' + tb.value + ' exists!');
            }
        }       
    }
    
    this.user_exists = function(userid)
    {
        if (IRSWorld.Internet.InternetRemitterEnrollment.user_exist(userid).value)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    this.disable_buttons = function(disable)
    {
        document.getElementById('btnSubmit').disabled = disable;
        document.getElementById('btnACancel').disabled = disable;
        document.getElementById('btnGoBack').disabled = disable;
    }
    
    this.parseBdate = function(bDate)
    {
       var arrbDate = bDate.split("/");
       var bDay = "01";
       var date = arrbDate[0] + "/" + bDay + "/" + arrbDate[1];
       return date;
    }
    
    
//  For Idology 
    this.show_questionaire = function(visible)
    {
        show_idology_questionaire(visible);
        set_read_only(document.getElementById('tblRemInfo'), visible);
        this.disable_buttons(visible);
        
        if (visible == true)
        {
            this.UserID           =   document.getElementById("txtUserID").value; 
	        this.Lastname         =   document.getElementById("txtLName").value; 
	        this.Firstname        =   document.getElementById("txtFName").value;
	        this.Middlename       =   document.getElementById("txtMName").value;
	        this.Birthdate        =   IRE.parseBdate(document.getElementById("txtBirthDate").value)//document.getElementById("txtBirthDate").value;
	        this.Occupation       =   (document.getElementById("ddOccupation").value == 'null') ? null : document.getElementById("ddOccupation").value; //document.getElementById("ddOccupation").value;
	        this.TelNo            =   document.getElementById("txtTelNo").value;
	        this.MobileNo         =   document.getElementById("txtMobileNo").value;
	        this.Email            =   document.getElementById("txtEmailAdd").value;	    
	        this.CompanyName      =   document.getElementById("txtEmpName").value;
    	    
	        this.Address1         =   document.getElementById("txtRoomNo").value;
	        this.Address2         =   document.getElementById("txtStreet").value;
	        this.Address3         =   null;//document.getElementById("txtDistrict").value;
	        this.Address4         =   document.getElementById("txtCity").value;
	        this.Address5         =   (document.getElementById("ddState").value == 'null') ? null : document.getElementById("ddState").value;
	        this.Address6         =   null;//document.getElementById("txtRegion").value;
	        this.ZipCode          =   document.getElementById("txtZipCode").value;
	        this.CountryCode      =   (document.getElementById("ddCountry").value == 'null') ? null : document.getElementById("ddCountry").value;
	        this.POBox            =   null;//document.getElementById("txtPOBox").value;
    	    
    	  
    	    
	        this.BranchCode       //=   (document.getElementById("ddBranchCode").value == 'null') ? null : document.getElementById("ddBranchCode").value;//document.getElementById("ddBranchCode").value;
	        this.AccountNo        =   "123";
	        this.AccountCurrency  =   "1";
    	    
	        this.IDNo                   =   document.getElementById("txtIDNo").value;
       	    this.SSN                    =   document.getElementById("txtSSSIDNo").value
	        this.IDType                 =   (document.getElementById("ddIDType").value == 'null') ? null : document.getElementById("ddIDType").value;
	        this.IDExpiry               =   document.getElementById("txtIDExpiry").value;
        
        
        
        
            var ipAdd       = document.getElementById('HIPAdd').value;            
            var address     = (((this.Address1 == null || this.Address1 == "null" || this.Address1 == undefined ) ? "" : this.Address1) + ' ' + 
                              ((this.Address2 == null || this.Address2 == "null" || this.Address2 == undefined ) ? "" : this.Address2) + ' ' + 
                              ((this.Address3 == null || this.Address3 == "null" || this.Address3 == undefined ) ? "" : this.Address3)).trim();
                              
            var Lastname    = ((this.Lastname == null || this.Lastname == "null" || this.Lastname == undefined ) ? "" : this.Lastname);
            var Firstname   = ((this.Firstname == null || this.Firstname == "null" || this.Firstname == undefined ) ? "" : this.Firstname);
            var Address4    = ((this.Address4 == null || this.Address4 == "null" || this.Address4 == undefined ) ? "" : this.Address4);
            var Address5    = ((this.Address5 == null || this.Address5 == "null" || this.Address5 == undefined ) ? "" : this.Address5);
            var ZipCode     = ((this.ZipCode == null || this.ZipCode == "null" || this.ZipCode == undefined ) ? "" : this.ZipCode);
            var Birthdate   = ((this.Birthdate == null || this.Birthdate == "null" || this.Birthdate == undefined ) ? "" : this.Birthdate);
            var IDNo        = ((this.IDNo == null || this.IDNo == "null" || this.IDNo == undefined ) ? "" : this.IDNo);
            var SSN         = ((this.SSN == null || this.SSN == "null" || this.SSN == undefined ) ? "" : this.SSN);
            
            var res = IRSWorld.Internet.InternetRemitterEnrollment.check_idology(ipAdd, this.Firstname,this.Lastname , this.Address1 + ' ' + this.Address2 + ' ' + this.Address3, this.Address4, this.Address5, this.ZipCode, this.Birthdate, this.SSN);
            
            if (res.error != null)
            {
                show_idology_questionaire(false);
                set_read_only(document.getElementById('tblRemInfo'), false);
                this.disable_buttons(false);
                alert(res.error.Message);
                return false;
            }
            else if(res.value == null)
            {
                alert("We cannot process your enrollment, please contact your nearest PNB Branch");
                return false;
            }
            else
            {
                this.idology_number = res.value.Tables[0].Rows[0].id_number;
                this.getQuestions(res.value.Tables[1]);
                this.qcount = 0;
                this.reqAnswered = 3;              
                this.createIdologyScreen();
                document.getElementById('btnPrevQuestion').disabled = true;
                document.getElementById('btnNextQuestion').disabled = false;  
                this.populate_questionaire();
                return true;
            }
        }
        return false;
    }
    
    this.populate_questionaire = function()
    {
        document.getElementById('tdQuestion').innerHTML = this.myarray[this.qcount][0];

        for (i=0; i < this.maxAnswer; i++)
        {
            document.getElementById('trQuestionAns' + i.toString()).style.display = 'none';
            
            if (this.myarray[this.qcount][i + 3] != null && this.myarray[this.qcount][i + 3] != undefined)
            {
                document.getElementById('trQuestionAns' + i.toString()).style.display = 'block';
                document.getElementById('tdQuestionAns' + i.toString()).innerHTML = this.myarray[this.qcount][i + 3];        
            }
            else
            {
                document.getElementById('tdQuestionAns' + i.toString()).innerHTML = ""; 
            }           
        }
        
        this.clearCheckBox();
    }
    
    this.createIdologyScreen = function()
    {
        var iContent = "";
        
        iContent += '<table id="Table1" cellpadding="0" cellspacing="5" style="border-right: #394398 1px solid; border-top: #394398 1px solid; background: #FFFFFF; border-left: #394398 1px solid; border-bottom: #394398 1px solid" width="100%;z-index:999"> ';
        iContent += '    <tr> ';
        iContent += '        <td id="Td10" class="table_caption" colspan="5" style="color: white; text-indent: 5px; text-align: left">Verification Questions</td> ';
        iContent += '    </tr> ';
        iContent += '    <tr> ';
        iContent += '        <td style="width: 9%; height: 9px"> &nbsp;</td> ';
        iContent += '        <td id="tdQuestion" align="left" style="width: 51%; height: 9px"></td> ';
        iContent += '    </tr> ';

        for (i=0; i < this.maxAnswer; i++)
        {
            iContent += '    <tr id = "trQuestionAns' + i.toString() + '"> ';
            iContent += '        <td class="cell_caption" style="width: 9%; height: 24px"> ';
            iContent += '            <input id="chkAns' + i.toString() + '" onclick="javascript:IRE.Ans_OnClick('+ i.toString() +');" style="width: 21%" type="checkbox" />&nbsp;</td> ';
            iContent += '        <td id="tdQuestionAns' + i.toString() + '" align="left" style="width: 51%; height: 24px"> &nbsp;</td> ';
            iContent += '    </tr> ';
        }
        
        iContent += '    <tr> ';
        iContent += '        <td class="cell_caption" style="width: 9%; height: 24px"> &nbsp;</td> ';
        iContent += '        <td align="left" style="width: 51%; height: 24px"> &nbsp; ';
        iContent += '            <input id="btnPrevQuestion"    class = "button"    type="button" value = "Prev" style="width: 66px" onclick="javascript:IRE.btnPrev_click();"/> ';
        iContent += '            <input id="btnNextQuestion"    class = "button"    type="button" value = "Next" style="width: 66px" onclick="javascript:IRE.btnNext_click();"/> ';    
        iContent += '            <input id="btnSubmitAns"    class = "button"    type="button" value = "Submit" style="width: 66px" onclick="javascript:IRE.btnSubmitAns_click();"/> ';
        iContent += '            <input id="btnCancelAns"    class = "button"    type="button" value = "Cancel" style="width: 66px" onclick="javascript:IRE.btnCancel_click();"/> ';
        iContent += '       </td> ';                             
        iContent += '    </tr>';
        iContent += '</table>';
        
        document.getElementById("divQandA").innerHTML = iContent;    
    }
    
    this.Ans_OnClick = function(objectID)
    {
        this.clearCheckBox();        
        this.myarray[this.qcount][2] = null;
        
        var answer = document.getElementById('tdQuestionAns' + objectID.toString()).innerHTML;
        
        if (answer == 'Skip Question' && this.hasSkip() == true)
        {
            alert('You can only skip once.')
        }
        else
        {
            document.getElementById('chkAns' + objectID).checked = true;
            this.myarray[this.qcount][2] = answer;
        }
    }
    
    this.hasSkip = function()
    {
        for (var i = 0; i < this.myarray.length; i++)
        {
            if (this.myarray[i][2] != null && this.myarray[i][2] != undefined && this.myarray[i][2] == 'Skip Question')
            {
                return true;
            }
        }        
        return false;
    }
    
    this.numAnswered = function()
    {
        var numberAnswered = 0;
        for (var i = 0; i < this.myarray.length; i++)
        {
            if (this.myarray[i][2] != null && this.myarray[i][2] != undefined && this.myarray[i][2] != 'Skip Question')
            {
                numberAnswered++;
            }
        }        
        return numberAnswered;
    }
    
    this.clearCheckBox = function()
    {
        for (i=0; i < this.maxAnswer; i++)
        {
            document.getElementById('chkAns' + i.toString()).checked = false;
        }
    }
    
    this.btnCancel_click = function()
    {
        this.show_questionaire(false);
    }
    
    this.btnPrev_click = function()
    {
        document.getElementById('btnNextQuestion').disabled = false;
        if (this.qcount > 0)
        {
            this.qcount-- ;
            this.populate_questionaire();  
        }
        else
        {
            document.getElementById('btnPrevQuestion').disabled = true;
        }
    }
    
    this.btnNext_click = function()
    {
        document.getElementById('btnPrevQuestion').disabled = false;
        if (this.myarray[this.qcount][2] != null && this.myarray[this.qcount][2] != undefined)
        {
            if (this.numAnswered() >= (this.reqAnswered - 1) || this.qcount >= this.myarray.length - 2)
            {
                document.getElementById('btnNextQuestion').disabled = true;
            }
            
            this.qcount++ ;
            this.populate_questionaire();  
        }
        else
        {
            alert('Please choose an answer from the list.')
        }
        
    }
    
    this.btnSubmitAns_click = function()
    {        
        var ansString = "";
        
        if (this.myarray[this.qcount][2] != null && this.myarray[this.qcount][2] != undefined)
        {
            for (var i = 0; i < this.myarray.length; i++)
            {   
                if (this.myarray[i][2] != null && this.myarray[i][2] != undefined && this.myarray[i][2] != 'Skip Question')
                {   
                    ansString +=  this.myarray[i][1] + '=' + this.myarray[i][2] + '|';            
                }
            }
            ansString = ansString.substring(0, ansString.length-1)
            //alert(ansString);
            
            if (IRSWorld.Internet.InternetRemitterEnrollment.submit_idology_answer(this.idology_number, ansString).value == true)
            {
                alert('Client verified, Committing transaction.');
                this.commit(this.create_plist());
            }
            else
            {
                alert('Client not Verified');
            }
            
            this.show_questionaire(false);
        }
        else
        {
            alert('Please choose an answer from the list.');
        }
    }
    
    this.getQuestions = function(dt)
    {
        this.myarray = new Array();
        var rowCount = 0;
        var curType = "";
        for (j=0; j < dt.Rows.length; j++)
        {   
            if (curType != dt.Rows[j].question_type)
            {
                this.myarray[rowCount] = new Array();
                this.myarray[rowCount][0] = dt.Rows[j].question_prompt;
                this.myarray[rowCount][1] = dt.Rows[j].question_type;
                this.myarray[rowCount][2] = null;
                rowCount++;
                curType = dt.Rows[j].question_type;
            }
        }
        
        // Get options
        
        this.maxAnswer = 0;
        for (h=0; h < this.myarray.length; h++)
        {
            var ansNumber = 0;            
            for (k=0; k < dt.Rows.length; k++)
            {                
                if (this.myarray[h][1] == dt.Rows[k].question_type)
                {
                    this.myarray[h][ansNumber + 3] = dt.Rows[k].question_answer;                    
                    ansNumber++;
                    this.maxAnswer = (this.maxAnswer < ansNumber) ? ansNumber : this.maxAnswer;                    
                }
            }
        }
    }
    
 this.getCIFID = function()
 {
    this.CIFID = IRSWorld.Internet.InternetRemitterEnrollment.getCIFID().value;  
 }

    
 this.saved = function(rs)
 {
    if (rs.error != null)
    {
        alert('An error occured during save. ' + rs.error.Message);
        return;
    }
    if (rs.value[0] != null)
    {
        IRSWorld.Internet.InternetRemitterEnrollment.sendForVerificationEmail(IRE.create_plist());
    } 
    else
    {
        alert("Error while saving record.");
    }           
    top.document.location.href = "../Main/IRSMain.aspx?load=../Admin/Login.aspx";
//    var retval = null;
//    if (rs.value.length > 1 && rs.value[1] != null && rs.value[1].length > 0)
//    {
//        retval = rs.value[1];
//        IRSWorld.Internet.InternetRemitterEnrollment.sendForVerificationEmail(IRE.create_plist());

//    }
 }


      
 this.maint_save_click = function()
 {
    {
        var errarr = '';
        if (errarr.length > 0)
        {
            alert('Save operation encountered the ff. errors:\n' + errarr);
            return;
        }
        var rec = "add~CIFID="+ this.CIFID +"~MODE=0~USERID="+ this.UserID +"~LOCATION="+ this.BranchCode +"~DAYSEXPIRED=281~ROLE=10~LoggedOn=false";
        if (rec != null)
        {
            email_op = 'New';
            IRSWorld.Internet.InternetRemitterEnrollment.save(rec, this.saved);
        }
        else
        {
            error_cif('An error occured during save.');
        }                
    }
 }
}

function fixdim()
{
    if (top.document.getElementById('divCenter'))
    {
        var div = document.getElementById('Main');
                
        div.style.height = (top.document.getElementById('divCenter').offsetHeight - 30) + 'px';
        div.style.width = (top.document.getElementById('divCenter').offsetWidth + 5) + 'px';
    }        
}

//function get_ip_address()
//{
//    if (window.XMLHttpRequest) xmlhttp = new XMLHttpRequest();
//    else xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");

//    xmlhttp.open("GET","http://api.hostip.info/get_html.php",false);
//    xmlhttp.send();

//    hostipInfo = xmlhttp.responseText.split("\n");

//    for (i=0; hostipInfo.length >= i; i++) {
//        ipAddress = hostipInfo[i].split(":");
//        if ( ipAddress[0] == "IP" ) return ipAddress[1];
//    }

//    return false;
    
//    xmlconn = new xml_http_req().get_xml_http_connection();
//    xmlconn.open("GET", "http://api.hostip.info/get_html.php",false);
//    xmlconn.setRequestHeader("Content-Type","application/x-www-form-urlencoded");
//            
//    if (xmlconn.timeout != undefined || xmlconn.timeout != null)
//    { 
//        xmlconn.timeout = 2000000;
//    }
//    xmlconn.send();
//    hostipInfo = xmlconn.responseText.split("\n");
//    
//    for (i=0; hostipInfo.length >= i; i++) {
//        ipAddress = hostipInfo[i].split(":");
//        if ( ipAddress[0] == "IP" ) return ipAddress[1];
//    }
//    
//    return false;    
//}

//function get_ip_address()
//{
//    var netobj = get_client_properties();
//    if (netobj != null)
//    {
//        return netobj.IPAddress;
//    }
//}



