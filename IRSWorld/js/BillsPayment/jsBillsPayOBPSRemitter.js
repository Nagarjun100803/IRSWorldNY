

function  BillsPayOBPSRemitter()
{

    //drop down start
    this.load_all_dropdownlist = function()
    {
        this.dropdown_title();
        this.dropdown_civil_status();
        this.dropdown_gender();
        this.dropdown_religion();
        this.dropdown_citizenship();
        this.dropdown_occupation_type();
        this.dropdown_worker_type();
        this.dropdown_country();
    } 
            
    this.dropdown_title = function()
    {
        OBPSRemitter.getdropdown_description("DisplayTitle", this.dropdown_title_callback)
    }
    this.dropdown_title_callback = function (result)
    {   
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();                                             
            this.commons.to_load_dropdownlist("ddlBillsPayOBPSRemitterTitle", result.value, "FldValue", "FldDescription")
            this.commons = null; 
        }
    }
    
    
    this.dropdown_religion = function()
    {
        OBPSRemitter.getdropdown_description("Religion", this.dropdown_religion_callback)
    }
    this.dropdown_religion_callback = function (result)
    {
         if (result != null && result != "undefined")
        {
            this.commons = new common_method(); 
            this.commons.to_load_dropdownlist("ddlBillsPayOBPSRemitterReligion", result.value, "FldValue", "FldDescription")
            this.commons = null; 
        }
    }



    this.dropdown_civil_status = function()
    {
        OBPSRemitter.getdropdown_description("CivilStatus", this.dropdown_civil_status_callback)
    }
    this.dropdown_civil_status_callback = function (result)
    {
         if (result != null && result != "undefined")
        {
            this.commons = new common_method(); 
            this.commons.to_load_dropdownlist("ddlBillsPayOBPSRemitterCivilStatus", result.value, "FldValue", "FldDescription")
            this.commons = null; 
        }
    }
    
    
    this.dropdown_gender = function()
    {
        OBPSRemitter.getdropdown_description("Gender", this.dropdown_gender_callback)
    }
    this.dropdown_gender_callback = function (result)
    {
         if (result != null && result != "undefined")
        {
            this.commons = new common_method(); 
            this.commons.to_load_dropdownlist("ddlBillsPayOBPSRemitterGender", result.value, "FldValue", "FldDescription")
            this.commons = null; 
        }
    }
    
    
    this.dropdown_citizenship = function()
    {
        OBPSRemitter.getdropdown_description("Citizenship", this.dropdown_citizenship_callback)
    }
    this.dropdown_citizenship_callback = function (result)
    {
         if (result != null && result != "undefined")
        {
            this.commons = new common_method(); 
            this.commons.to_load_dropdownlist("ddlBillsPayOBPSRemitterCitizenship", result.value, "FldValue", "FldDescription")
            this.commons = null; 
        }
    }
    
    
    this.dropdown_occupation_type = function()
    {
        OBPSRemitter.getdropdown_description("OccupationType", this.dropdown_occupation_type_callback)
    }
    this.dropdown_occupation_type_callback = function (result)
    {
         if (result != null && result != "undefined")
        {
            this.commons = new common_method(); 
            this.commons.to_load_dropdownlist("ddlBillsPayOBPSRemitterOccupationType", result.value, "FldValue", "FldDescription")
            this.commons = null; 
        }
    }
    
    
    this.dropdown_worker_type = function()
    {
        OBPSRemitter.getdropdown_description("WorkerType", this.dropdown_worker_type_callback)
    }
    this.dropdown_worker_type_callback = function (result)
    {
         if (result != null && result != "undefined")
        {
            this.commons = new common_method(); 
            this.commons.to_load_dropdownlist("ddlBillsPayOBPSRemitterWorkerType", result.value, "FldValue", "FldDescription")
            this.commons = null; 
        }
    }
    
    
    this.dropdown_country = function()
    {
        OBPSRemitter.getdropdown_description("Country", this.dropdown_country_callback)
    }
    this.dropdown_country_callback = function (result)
    {
         if (result != null && result != "undefined")
        {
            this.commons = new common_method(); 
            this.commons.to_load_dropdownlist("ddlBillsPayOBPSRemitterCountry", result.value, "FldValue", "FldDescription")
            this.commons = null; 
        }
    }
    //dropdown end
    
    //sidebar start
    this.construct_side_bar = function(div_container)
    {
        document.getElementById(div_container).innerHTML = this.get_side_bar();
    }
    
    this.get_side_bar = function()
    {
        var side_array  = this.get_side_bar_list();
        var side_winder = new js_sidebar('sidemenu', side_array);
        return side_winder.toString();
    }

    this.get_side_bar_list = function()
    {
        var sidebar = new Array
        (
	        new Array(1,0,'Log-in',     ''),
	        new Array(2,0,'CIF',        ''),
            new Array(3,0,'Remittance', ''),
	        new Array(4,0,'Internet',   '')
	      //new Array(100,0,'Cancel Transaction', '')
        );
        return sidebar; 
    }

    this.set_side_bar_visibility = function (division_id, anchor_id)
    {
        var div  = document.getElementById(division_id);
        var cset = div.style.display;

        div.style.display = (cset == "block") ? "none" : "block";
        document.getElementById(anchor_id).innerText = (cset == "block") ? String.fromCharCode(187) : String.fromCharCode(171);
    }
    //sidebar end
    
    //clear start
    this.clear_all_fields = function()
    {
        this.clear_field('txtBillsPayOBPSRemitterLastName')
        this.clear_field('txtBillsPayOBPSRemitterFirstName')
        this.clear_field('txtBillsPayOBPSRemitterMiddleName')
        this.clear_field('txtBillsPayOBPSRemitterAlias')
        this.clear_field('txtBillsPayOBPSRemitterBirthdate')
        this.clear_field('txtBillsPayOBPSRemitterAge')
        this.clear_field('txtBillsPayOBPSRemitterBirthplace')
        this.clear_field('txtBillsPayOBPSRemitterContactNos')
        this.clear_field('txtBillsPayOBPSRemitterEmail')
        this.clear_field('txtBillsPayOBPSRemitterRoomNo')
        this.clear_field('txtBillsPayOBPSRemitterStreetNo')
        this.clear_field('txtBillsPayOBPSRemitterDistrict')
        this.clear_field('txtBillsPayOBPSRemitterCity')
        this.clear_field('txtBillsPayOBPSRemitterState')
        this.clear_field('txtBillsPayOBPSRemitterRegion')
        this.clear_field('txtBillsPayOBPSRemitterZipCode')
        this.clear_field('txtBillsPayOBPSRemitterRemarks')
        
        this.clear_field('ddlBillsPayOBPSRemitterTitle')
        this.clear_field('ddlBillsPayOBPSRemitterCivilStatus')
        this.clear_field('ddlBillsPayOBPSRemitterGender')
        this.clear_field('ddlBillsPayOBPSRemitterReligion')
        this.clear_field('ddlBillsPayOBPSRemitterCitizenship')
        this.clear_field('ddlBillsPayOBPSRemitterOccupationType')
        this.clear_field('ddlBillsPayOBPSRemitterWorkerType')
        this.clear_field('ddlBillsPayOBPSRemitterCountry')
    }
    
    this.clear_field = function(FieldId)
    {
        var id  = document.getElementById(FieldId);
        
        id.value="";
    }
//clear end
             
//save start

    this.SEQ_ID                = null;
    this.RemTitle              = null;
    this.RemLastname           = null;
    this.RemFirstname          = null;
    this.RemMiddlename         = null;
    this.RemAlias              = null;
    this.RemGender             = null;
    this.RemAge                = null;
    this.RemBirthdate          = null;
    this.RemBirthPlace         = null;
    this.RemReligion           = null;
    this.RemCivilStatus        = null;
    this.RemCitizenship        = null;
    this.RemOccupationType     = null;
    this.RemWorkerType         = null;
    this.RemContactNo          = null;
    this.RemEmail              = null;
    this.RemAddress1           = null;
    this.RemAddress2           = null;
    this.RemAddress3           = null;
    this.RemAddress4           = null;
    this.RemAddress5           = null;
    this.RemAddress6           = null;
    this.RemZipCodeSeqId       = null;
    this.RemCountryCode        = null;
    this.RemReasonRequest      = null;
    this.RemRemitterClass      = null;
    
    
        this.set_OBPS_remitter = function()
    {   
        this.RemTitle          = document.getElementById("ddlBillsPayOBPSRemitterTitle").value;
        this.RemLastname       = document.getElementById("txtBillsPayOBPSRemitterLastName").value;
        this.RemFirstname      = document.getElementById("txtBillsPayOBPSRemitterFirstName").value;
        this.RemMiddlename     = document.getElementById("txtBillsPayOBPSRemitterMiddleName").value;
        this.RemAlias          = document.getElementById("txtBillsPayOBPSRemitterAlias").value;
        this.RemGender         = document.getElementById("ddlBillsPayOBPSRemitterGender").value;
        this.RemAge            = document.getElementById("txtBillsPayOBPSRemitterAge").value;
        this.RemBirthdate      = document.getElementById("txtBillsPayOBPSRemitterBirthdate").value;
        this.RemBirthPlace     = document.getElementById("txtBillsPayOBPSRemitterBirthplace").value;
        this.RemReligion       = document.getElementById("ddlBillsPayOBPSRemitterReligion").value;
        this.RemCivilStatus    = document.getElementById("ddlBillsPayOBPSRemitterCivilStatus").value;
        this.RemCitizenship    = document.getElementById("ddlBillsPayOBPSRemitterCitizenship").value;
        this.RemOccupationType = document.getElementById("ddlBillsPayOBPSRemitterOccupationType").value;
        this.RemWorkerType     = document.getElementById("ddlBillsPayOBPSRemitterWorkerType").value;
        this.RemContactNo      = document.getElementById("txtBillsPayOBPSRemitterContactNos").value;
        this.RemEmail          = document.getElementById("txtBillsPayOBPSRemitterEmail").value;
        this.RemAddress1       = document.getElementById("txtBillsPayOBPSRemitterRoomNo").value;
        this.RemAddress2       = document.getElementById("txtBillsPayOBPSRemitterStreetNo").value;
        this.RemAddress3       = document.getElementById("txtBillsPayOBPSRemitterDistrict").value;
        this.RemAddress4       = document.getElementById("txtBillsPayOBPSRemitterCity").value;
        this.RemAddress5       = document.getElementById("txtBillsPayOBPSRemitterState").value;
        this.RemAddress6       = document.getElementById("txtBillsPayOBPSRemitterRegion").value;
        this.RemZipCodeSeqId   = document.getElementById("txtBillsPayOBPSRemitterZipCode").value;
        this.RemCountryCode    = document.getElementById("ddlBillsPayOBPSRemitterCountry").value;
        this.RemReasonRequest  = document.getElementById("txtBillsPayOBPSRemitterRemarks").value;
        this.RemRemitterClass  = '2'
        
        return this.push_data_to_server();
     }
     
         this.push_data_to_server = function()    
    {    
        var data_row = new Array();
             
        data_row[data_row.length] = "RemTitle~"            + this.RemTitle
        data_row[data_row.length] = "RemLastname~"         + this.RemLastname
        data_row[data_row.length] = "RemFirstname~"        + this.RemFirstname
        data_row[data_row.length] = "RemMiddlename~"       + this.RemMiddlename
        data_row[data_row.length] = "RemAlias~"            + this.RemAlias
        data_row[data_row.length] = "RemGender~"           + this.RemGender
        data_row[data_row.length] = "RemAge~"              + this.RemAge
        data_row[data_row.length] = "RemBirthdate~"        + this.RemBirthdate
        data_row[data_row.length] = "RemBirthPlace~"       + this.RemBirthPlace
        data_row[data_row.length] = "RemReligion~"         + this.RemReligion
        data_row[data_row.length] = "RemCivilStatus~"      + this.RemCivilStatus
        data_row[data_row.length] = "RemCitizenship~"      + this.RemCitizenship
        data_row[data_row.length] = "RemOccupationType~"   + this.RemOccupationType
        data_row[data_row.length] = "RemWorkerType~"       + this.RemWorkerType
        data_row[data_row.length] = "RemContactNo~"        + this.RemContactNo
        data_row[data_row.length] = "RemEmail~"            + this.RemEmail
        data_row[data_row.length] = "RemAddress1~"         + this.RemAddress1
        data_row[data_row.length] = "RemAddress2~"         + this.RemAddress2
        data_row[data_row.length] = "RemAddress3~"         + this.RemAddress3
        data_row[data_row.length] = "RemAddress4~"         + this.RemAddress4
        data_row[data_row.length] = "RemAddress5~"         + this.RemAddress5
        data_row[data_row.length] = "RemAddress6~"         + this.RemAddress6
        data_row[data_row.length] = "RemZipCodeSeqId~"     + this.RemZipCodeSeqId
        data_row[data_row.length] = "RemCountryCode~"      + this.RemCountryCode
        data_row[data_row.length] = "RemReasonRequest~"    + this.RemReasonRequest   
        data_row[data_row.length] = "RemRemitterClass~"    + this.RemRemitterClass                  
           
        return data_row.join("|");       
    }
    
    this.insert_record = function()
    {   
        OBPSRemitter.insert_record(this.insert_record_callback);
    }  
    
    this.insert_record_callback = function(result)
    {       
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = InternetRemitter.read_message(); alert(msg.value); return;
            }
             
        }
    }
    
    //book1
    //this.Internet_Sidebar_class = null;        
    
    this.commit_record = function()
    {
        this.insert_record();
       
        if (typeof(this.validate_input) != 'undefined')
            {
                if (this.validate_input() == false)
                {
                   alert(this.error_input); return;
                }                            
            }       
            OBPSRemitter.commit_record(this.set_OBPS_remitter(),this.commit_record_callback);
    }
    
    this.commit_record_callback = function(result)
    {
        if (result != null && result.value != "undefined")
        {        
            if (result.value == false)
            {
                var msg = OBPSRemitter.read_message(); alert(msg.value); return;
            }
            
            this.SEQ_ID = OBPSRemitter.OBPS_REMITTER_SEQ_ID().value;
            
            alert("RECORD COMITTED SUCCESSFULLY");       
        }
    }
    
    this.compute_age = function()
    {
        if (document.getElementById("txtBillsPayOBPSRemitterBirthdate").value == "")
        {
            document.getElementById("txtBillsPayOBPSRemitterAge").value = ""; return; 
        }

        this.bdate  = document.getElementById("txtBillsPayOBPSRemitterBirthdate").value.split("/");
        this.bmonth = Number(this.bdate[0])-1;
        this.bday   = Number(this.bdate[1]);
        this.byear  = Number(this.bdate[2]);
        this.today  = new Date();
        
        var age     = 0;

        if((this.today.getMonth() > this.bmonth)||(this.today.getMonth() == this.bmonth & this.today.getDate() >= this.bday)) age = this.byear;
        else age = this.byear + 1; 
        
        document.getElementById("txtBillsPayOBPSRemitterAge").value = this.today.getFullYear() - age;
    }

    this.btn_save = function()
    {   
            if (typeof(this.validate_input) != "undefined")
            {
                if (this.validate_input() == false)
                {
                   alert(this.error_input); return;
                }
               else
                {
                //book2
                    this.commit_record();
//                    this.disable_field();
                        alert("saved");
                }
            }  
    }
    
    this.validate_input = function()
    { 
        this.commons     = new common_method();        
        this.error_input = "";   
        
        if (this.commons.to_validate_bdate(document.getElementById("txtBillsPayOBPSRemitterBirthdate").value) == false)
        {           
            this.error_input += "Birth date is invalid date.\n";            
        }
        
        if (this.commons.to_validate_age(document.getElementById("txtBillsPayOBPSRemitterAge").value) == false)
        {           
            this.error_input += "Age is invalid.\n";            
        }        
        
         if(this.commons.is_empty(document.getElementById("txtBillsPayOBPSRemitterLastName").value) == true ||
            this.commons.is_empty(document.getElementById("txtBillsPayOBPSRemitterFirstName").value) == true ||
            this.commons.is_empty(document.getElementById("txtBillsPayOBPSRemitterMiddleName").value) == true)
        {
            this.error_input += "Name is a required field.\n";  
        }
        
        if(this.commons.to_validate_intlphone(document.getElementById("txtBillsPayOBPSRemitterContactNos").value) == false)
        {
            this.error_input += "Invalid contact number.\n";
        }
        
        if(this.commons.to_validate_email(document.getElementById("txtBillsPayOBPSRemitterEmail").value) == false)
        {
            this.error_input += "Invalid email address.\n";
        }
  
        if (this.error_input != '')
        {
            return false;
        }
        
        return true; 
    }

    this.clear_field = function()
    {
        document.getElementById("ddlBillsPayOBPSRemitterTitle").value            = "";
        document.getElementById("txtBillsPayOBPSRemitterLastName").value         = "";
        document.getElementById("txtBillsPayOBPSRemitterFirstName").value        = "";
        document.getElementById("txtBillsPayOBPSRemitterMiddleName").value       = "";
        document.getElementById("txtBillsPayOBPSRemitterAlias").value            = "";
        document.getElementById("ddlBillsPayOBPSRemitterGender").value           = "";
        document.getElementById("txtBillsPayOBPSRemitterAge").value              = "";
        document.getElementById("txtBillsPayOBPSRemitterBirthdate").value        = "";
        document.getElementById("txtBillsPayOBPSRemitterBirthplace").value       = "";
        document.getElementById("ddlBillsPayOBPSRemitterReligion").value         = "";
        document.getElementById("ddlBillsPayOBPSRemitterCivilStatus").value      = "";
        document.getElementById("ddlBillsPayOBPSRemitterCitizenship").value      = "";
        document.getElementById("ddlBillsPayOBPSRemitterOccupationType").value   = "";
        document.getElementById("ddlBillsPayOBPSRemitterWorkerType").value       = "";
        document.getElementById("txtBillsPayOBPSRemitterContactNos").value       = "";
        document.getElementById("txtBillsPayOBPSRemitterEmail").value            = "";
        document.getElementById("txtBillsPayOBPSRemitterRoomNo").value           = "";
        document.getElementById("txtBillsPayOBPSRemitterStreetNo").value         = "";
        document.getElementById("txtBillsPayOBPSRemitterDistrict").value         = "";
        document.getElementById("txtBillsPayOBPSRemitterCity").value             = "";
        document.getElementById("txtBillsPayOBPSRemitterState").value            = "";
        document.getElementById("txtBillsPayOBPSRemitterRegion").value           = "";
        document.getElementById("txtBillsPayOBPSRemitterZipCode").value          = "";
        document.getElementById("ddlBillsPayOBPSRemitterCountry").value          = "";
        document.getElementById("txtBillsPayOBPSRemitterRemarks").value          = "";
    }

    
    this.disable_field = function()
    {
        document.getElementById("ddlBillsPayOBPSRemitterTitle").disabled            = true;
        document.getElementById("txtBillsPayOBPSRemitterLastName").disabled         = true;
        document.getElementById("txtBillsPayOBPSRemitterFirstName").disabled        = true;
        document.getElementById("txtBillsPayOBPSRemitterMiddleName").disabled       = true;
        document.getElementById("txtBillsPayOBPSRemitterAlias").disabled            = true;
        document.getElementById("ddlBillsPayOBPSRemitterGender").disabled           = true;
        document.getElementById("txtBillsPayOBPSRemitterAge").disabled              = true;
        document.getElementById("txtBillsPayOBPSRemitterBirthdate").disabled        = true;
        document.getElementById("txtBillsPayOBPSRemitterBirthplace").disabled       = true;
        document.getElementById("ddlBillsPayOBPSRemitterReligion").disabled         = true;
        document.getElementById("ddlBillsPayOBPSRemitterCivilStatus").disabled      = true;
        document.getElementById("ddlBillsPayOBPSRemitterCitizenship").disabled      = true;
        document.getElementById("ddlBillsPayOBPSRemitterOccupationType").disabled   = true;
        document.getElementById("ddlBillsPayOBPSRemitterWorkerType").disabled       = true;
        document.getElementById("txtBillsPayOBPSRemitterContactNos").disabled       = true;
        document.getElementById("txtBillsPayOBPSRemitterEmail").disabled            = true;
        document.getElementById("txtBillsPayOBPSRemitterRoomNo").disabled           = true;
        document.getElementById("txtBillsPayOBPSRemitterStreetNo").disabled         = true;
        document.getElementById("txtBillsPayOBPSRemitterDistrict").disabled         = true;
        document.getElementById("txtBillsPayOBPSRemitterCity").disabled             = true;
        document.getElementById("txtBillsPayOBPSRemitterState").disabled            = true;
        document.getElementById("txtBillsPayOBPSRemitterRegion").disabled           = true;
        document.getElementById("txtBillsPayOBPSRemitterZipCode").disabled          = true;
        document.getElementById("ddlBillsPayOBPSRemitterCountry").disabled          = true;
        document.getElementById("txtBillsPayOBPSRemitterRemarks").disabled          = true;
        document.getElementById("btnSave").disabled                                 = true;
    }

//save end 
        

    
}

function execute_ajaxgridview_command(param)
{
    if (dfields != null && dfields != "undefined")
    {
        dfields.do_ajaxgridview(param);
    }
}

function OBPSDynamicFields()
{
    this.MerchantCode = null;
    this.MerchantProductCode = null;
    this.FieldSEQ_ID = null;
    this.FieldName = null;
    this.DisplaySeq = null;
    this.DisplayDtDef = null;
    this.FieldLabel = null;
    this.IsMandatory = null;
    this.DataType = null;
    this.ValidationType = null;
    this.Argument = null;
    this.ValidateOnValue = null;
    this.FieldName2 = null;
    this.FieldName3 = null;
    this.LIFldRef = null;
    this.isDeleted = null;
    this.ControlName = null;
    this.ISARM = null;
    this.ValidationSeq = null;
    
    this.ClearForm = function()
    {
        document.getElementById("txtOBPSDynamicFieldLabel").value = "";
        document.getElementById("ddlOBPSDynamicOBPSValidationType").value = "";
        document.getElementById("ddlOBPSDynamicFieldName").value = "";
        document.getElementById("txtOBPSDynamicDisplaySeq").checked = false;
        document.getElementById("chbIsMandatory").checked = false;
        document.getElementById("txtValOnVal").value = "";
        document.getElementById("ddlOBPSDynamicListFIeldRef").value = "";
        this.FieldSEQ_ID = null;
    }
    
    this.EnableDisableCrtls = function(bln)
    {
        document.getElementById("txtOBPSDynamicFieldLabel").disabled = bln;
        document.getElementById("ddlOBPSDynamicOBPSValidationType").disabled = bln;
        document.getElementById("ddlOBPSDynamicFieldName").disabled = bln;
        document.getElementById("txtOBPSDynamicDisplaySeq").disabled = bln;
        document.getElementById("chbIsMandatory").disabled = bln;
        document.getElementById("txtValOnVal").disabled = bln;
        document.getElementById("ddlOBPSDynamicListFIeldRef").disabled = bln;
    }
    
    this.GetLookupFldGroup = function()
    {
        var LookupGroup = IRSWorld.BillsPayment.OBPSDynamicField.getLookupField().value;
        if (LookupGroup.Rows.length > 0)
        {
            if (document.getElementById('ddlOBPSDynamicListFIeldRef').length != 0)
            {
                this.cleanDDL(document.getElementById('ddlOBPSDynamicListFIeldRef'));
            }
            for (var i = 0; i < LookupGroup.Rows.length; i++)
            {
                if (i == 0)
                {
                    var opt2 = document.createElement("option");
                    opt2.text = '';
                    opt2.value = '';
                    document.getElementById('ddlOBPSDynamicListFIeldRef').options.add(opt2);
                }
                var opt = document.createElement("option");                 
                opt.text = LookupGroup.Rows[i].FldGroup.toString();
                opt.value = LookupGroup.Rows[i].FldGroups.toString();
                document.getElementById('ddlOBPSDynamicListFIeldRef').options.add(opt);
            }
        }
    }
    
    this.LoadFieldsPage = function()
    {
        this.getMerchantCode();
    }
    
    this.load_grid_viewer = function()
    {
        this.ClearForm();
        this.GetLookupFldGroup();
        this.MerchantProductCode = document.getElementById("ddlOBPSDynamicProductCode")[document.getElementById("ddlOBPSDynamicProductCode").selectedIndex].value;
        this.commons = new common_method();     
        //this.commons.to_clear_screen("tblMerchantOtherInfoEntry");
        this.commons = null;
        
        this.grid = new grid_viewer();
        this.session_end         = true;
        this.grid.sql_source     = "spGetOptionalFields~" + this.MerchantCode + "~" + this.MerchantProductCode;
        this.grid.div_container  = document.getElementById("dvFieldGridView");
        this.grid.datarow_column = 0;
        this.grid.open_grid_view();
        this.grid = null;
    }
    
    this.process_grid_viewer = function(parameter)
    {
        this.grid = new grid_viewer();
        
        this.grid.sql_source     = "spGetOptionalFields~" + this.MerchantCode + "~" + this.MerchantProductCode;
        this.grid.div_container  = document.getElementById("dvFieldGridView");
        this.grid.datarow_column = 0;
        this.grid.process_ajax(parameter);
        this.grid = null;
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
                    var dt = null;
                    var dt2 = null;
                    dt = IRSWorld.BillsPayment.OBPSDynamicField.getFields(this.MerchantCode,this.MerchantProductCode).value;
                    if (dt.Rows.length > 0)
                    {
                        if (dt.Columns.length > 8)
                        {
                            document.getElementById("FieldRef").style.display = 'block';
                            document.getElementById("crtlFieldRef").style.display = 'block';
                            document.getElementById("VOV").style.display = 'block';
                            document.getElementById("crtlVOV").style.display = 'block';
                        }
                        else
                        {
                            document.getElementById("FieldRef").style.display = 'none';
                            document.getElementById("crtlFieldRef").style.display = 'none';
                            document.getElementById("VOV").style.display = 'none';
                            document.getElementById("crtlVOV").style.display = 'none';
                        }
                        for (var i = 0; i < dt.Rows.length; i++)
                        {
                            if (dt.Rows[i].DISPLAYSEQ == command_param[1])
                            {
                                document.getElementById("txtOBPSDynamicFieldLabel").value = dt.Rows[i].FIELDLABEL.toString();
                                document.getElementById("ddlOBPSDynamicOBPSValidationType").value = dt.Rows[i].VALIDATIONTYPE.toString();
                                document.getElementById("ddlOBPSDynamicFieldName").value = dt.Rows[i].FIELDNAME.toString().substring(0,2);
                                document.getElementById("txtOBPSDynamicDisplaySeq").checked = dt.Rows[i].ISARM;
                                document.getElementById("chbIsMandatory").checked = dt.Rows[i].ISMANDATORY;
                                this.FieldSEQ_ID = command_param[1];
                                this.FieldName = dt.Rows[i].FIELDNAME.toString();
                                dt2 = IRSWorld.BillsPayment.OBPSDynamicField.getAllFields(this.FieldSEQ_ID,this.MerchantCode,this.MerchantProductCode).value;
                                
                                this.DisplaySeq = dt2.Rows[i].DisplaySeq.toString();
                                this.DisplayDtDef = dt2.Rows[i].DisplayDtDef.toString();
                                this.FieldLabel = dt2.Rows[i].FieldLabel.toString();
                                this.IsMandatory = dt2.Rows[i].IsMandatory.toString();
                                this.DataType = dt2.Rows[i].DataType.toString();
                                this.ValidationType = dt2.Rows[i].ValidationType.toString();
                                this.Argument = dt2.Rows[i].Argument.toString();
                                this.ValidateOnValue = dt2.Rows[i].ValidateOnValue.toString();
                                this.FieldName2 = dt2.Rows[i].FieldName2.toString();
                                this.FieldName3 = dt2.Rows[i].FieldName3.toString();
                                this.LIFldRef = dt2.Rows[i].LIFldRef.toString();
                                this.isDeleted = dt2.Rows[i].isDeleted.toString();
                                this.ControlName = dt2.Rows[i].ControlName.toString();
                                this.ISARM = dt2.Rows[i].ISARM.toString();
                                this.ValidationSeq = dt2.Rows[i].ValidationSeq.toString();
                                if (document.getElementById("FieldRef").style.display == 'block')
                                {
                                    document.getElementById("txtValOnVal").value = dt.Rows[i].VALIDATEONVALUE;
                                    document.getElementById("ddlOBPSDynamicListFIeldRef").value = dt.Rows[i].LIFLDREF.toString();
                                }
                            }
                        }
                        document.getElementById("btnOBPSDynamicEdit").disabled = false;
                    }
//                    document.getElementById("txtHiddenSeqId").value = command_param[1];
//                    
//                    var panel = this.active_panel;
//                    if(this.active_panel == "CIF_MASTER_INDIVIDUAL" || this.active_panel == "CIF_MASTER_BUSINESS")
//                    {
//                        panel = "CIF_MASTER";
//                    }
//                    
//                    switch(panel)
//                    {
//                        case "CIF_CLASSIFICATIONS":
//                        {
//                            this.customers_info.CIFClass = command_param[1];                        
//                            this.customers_info.do_class(command_param[1]);
//                            this.show_cif_menu();
//                            this.customers_info.load_grid_viewer_add();
//                            this.customers_info.load_grid_viewer_remove(); 
//                            return; 
//                            break;
//                        }
//                        case "CIF_SPOUSE":
//                        {
//                            this.show_btnAddtoBene();
//                            break;
//                        }
//                        case "CIF_MERCHANTS":
//                        {
//                            this.generic   = new generic_method();
//                            this.merchants = new cif_merchant();
//                            
//                            var fieldlabel = document.getElementById("txtHiddenMerFieldLabel").value;
//                            
//                            var ds = null;
//                            ds = cifmain.get_merchants_info(command_param[1]).value;
//                            
//                            if (ds.Tables[0].Rows.length > 0) 
//                            {
//                                document.getElementById("txtHiddenMerCode").value = ds.Tables[0].Rows[0].FirstName;
//                            } 
//        
//                            this.customers_info = new cif_merchant();
//                            this.customers_info.load_flag = true;   
//                            break;
//                        }
//                        case "CIF_MASTER":
//                        {
//                            var class_desc = '';
//                            var ds = cifmain.get_cif_classdesc(command_param[1]).value;
//                            
//                            if(ds.Tables[0].Rows.length > 0)
//                            {
//                                class_desc = ds.Tables[0].Rows[0].FldDescription; 
//                            }
//                            
//                            if(confirm("Create " + class_desc + " CIF?"))
//                            {
//                                document.getElementById("txtHiddenDummyClass").value = command_param[1];
//                                this.customers_info.dummy_class = command_param[1];
//                                hidePanel('divAddCIFClass');
//                            }
//                            break;
//                        }
//                        default: break;
//                    }
//                    
//                    this.SEQ_ID = command_param[1];
//                    
//                    if(panel != "CIF_OTHER_IDS" && panel != "CIF_WATCHLIST" && panel != "CIF_MASTER")
//                    {
//                        this.btn_load(); 
//                    }
                    return;           
                }
                
                this.process_grid_viewer(parameter);
            }
            command_param = null;
        }
    }
    
    this.getMerchantCode = function()
    {
        var MerchantList = IRSWorld.BillsPayment.OBPSDynamicField.getMerchantCode().value;
                
        if (MerchantList != null && MerchantList.Rows.length > 0)
        {
            if (document.getElementById('ddlOBPSDynamicMerchantCode').length != 0)
            {
                this.cleanDDL(document.getElementById('ddlOBPSDynamicMerchantCode'));
            }
            for (var i = 0; i < MerchantList.Rows.length; i++)
            {                
                var opt = document.createElement("option");                 
                opt.text = MerchantList.Rows[i].MerchantCode.toString();
                opt.value = MerchantList.Rows[i].MerchantCode.toString();
                document.getElementById('ddlOBPSDynamicMerchantCode').options.add(opt);
            }
            this.MerchantCode = document.getElementById("ddlOBPSDynamicMerchantCode")[document.getElementById("ddlOBPSDynamicMerchantCode").selectedIndex].value;
            this.getMerProductCode();
            
            document.getElementById("dvFieldGridView").style.display = 'block';
        }
        
    }
    
    this.getMerProductCode = function()
    {
        this.MerchantCode = document.getElementById("ddlOBPSDynamicMerchantCode")[document.getElementById("ddlOBPSDynamicMerchantCode").selectedIndex].value;
        var MerProductCode = IRSWorld.BillsPayment.OBPSDynamicField.getMerProductbyMerchant(this.MerchantCode).value;
        if (MerProductCode != null && MerProductCode.Rows.length > 0)
        {
            if (document.getElementById('ddlOBPSDynamicProductCode').length != 0)
            {
                this.cleanDDL(document.getElementById('ddlOBPSDynamicProductCode'));
            }
            
            for (var i = 0; i < MerProductCode.Rows.length; i++)
            {                
                var opt = document.createElement("option");                 
                opt.text = MerProductCode.Rows[i].MerchantProductCodeDesc.toString();
                opt.value = MerProductCode.Rows[i].MerchantProductCode.toString();
                document.getElementById('ddlOBPSDynamicProductCode').options.add(opt);
            }
            this.MerchantProductCode = document.getElementById("ddlOBPSDynamicProductCode")[document.getElementById("ddlOBPSDynamicProductCode").selectedIndex].value;
            this.load_grid_viewer();
            document.getElementById("dvFieldGridView").style.display = 'block';
        }
    }        
        
    this.popDDL = function(DDL, DT)
    {
        if (DT != null && DT.Rows.length > 0)
        {            
            for (var i = 0; i < DT.Rows.length; i++)
            {
                if (i == 0)
                {
                    var opt2 = document.createElement("option");
                    opt2.text = '';
                    opt2.value = '';
                    DDL.options.add(opt2);
                }
                var opt = document.createElement("option");                 
                opt.text = DT.Rows[i].Description.toString();
                opt.value = DT.Rows[i].Code.toString();
                DDL.options.add(opt);
            }      
        }
    }
    
    this.cleanDDL = function(DDL)
    {        
        DDL.length = 0;
    }
    
    this.editRecord = function()
    {
        if (this.FieldSEQ_ID == null)
        {
            alert('Please select an item to edit.');
            return;
        }
        if (document.getElementById("btnOBPSDynamicEdit").value == "Edit")
        {
            document.getElementById("btnOBPSDynamicAdd").disabled = true;
            
            document.getElementById("btnOBPSDynamicSave").disabled = false;
            this.EnableDisableCrtls(false);
            document.getElementById("btnOBPSDynamicEdit").value = "Cancel";
        }
        else
        {
            document.getElementById("btnOBPSDynamicAdd").disabled = false;
            
            document.getElementById("btnOBPSDynamicSave").disabled = true;
            this.EnableDisableCrtls(true);
            document.getElementById("btnOBPSDynamicEdit").value = "Edit";
            document.getElementById("btnOBPSDynamicEdit").disabled = true;
        }
    }
    
    this.addRecord = function()
    {
        if (document.getElementById("btnOBPSDynamicAdd").value == "Add")
        {
            
            document.getElementById("btnOBPSDynamicEdit").disabled = true;
            document.getElementById("btnOBPSDynamicSave").disabled = false;
            this.ClearForm();
            this.EnableDisableCrtls(false);
            document.getElementById("btnOBPSDynamicAdd").value = "Cancel";
        }
        else
        {
            
            document.getElementById("btnOBPSDynamicEdit").disabled = false;
            document.getElementById("btnOBPSDynamicSave").disabled = true;
            this.EnableDisableCrtls(true);
            document.getElementById("btnOBPSDynamicAdd").value = "Add"
        }
    }
    
    this.saveRecord = function()
    {
        document.getElementById("btnOBPSDynamicAdd").disabled = false;
        document.getElementById("btnOBPSDynamicEdit").disabled = false;
        document.getElementById("btnOBPSDynamicSave").disabled = true;
        this.EnableDisableCrtls(true);
        
        var data_value = this.getCurrentRecord();
        IRSWorld.BillsPayment.OBPSDynamicField.saveFields(this.FieldSEQ_ID,this.MerchantCode,this.MerchantProductCode,data_value,this.saveRecord_callback)        
    }
    
    this.saveRecord_callback = function(param)
    {
        alert(param.value);
        if(document.getElementById("btnOBPSDynamicAdd").value == "Cancel")
        {
            document.getElementById("btnOBPSDynamicAdd").value = "Add";
        }
        if (document.getElementById("btnOBPSDynamicEdit").value == "Cancel")
        {
            document.getElementById("btnOBPSDynamicEdit").value = "Edit";
        }
        dfields.load_grid_viewer();
    }
    
    this.getCurrentRecord = function()
    {
        var data = null;
        
        data = "MerchantCode=" + this.MerchantCode + ",";
        data += "ProductCode=" + this.MerchantProductCode + ",";
        data += "FieldName=" + ((this.FieldName.length > 2) ? this.FieldName + "," : document.getElementById("ddlOBPSDynamicFieldName").value + ",");
        data += "DisplaySeq=" + this.DisplaySeq + ",";
        data += "DisplayDtDef=" + this.DisplayDtDef + ",";
        data += "FieldLabel=" + document.getElementById("txtOBPSDynamicFieldLabel").value + ",";
        data += "IsMandatory=" + document.getElementById("chbIsMandatory").checked + ",";
        data += "DataType=" + this.DataType + ",";
        data += "ValidationType=" + document.getElementById("ddlOBPSDynamicOBPSValidationType").value + ",";
        data += "ValidationSeq=" + "" + ",";
        data += "Argument=" + this.Argument + ",";
        data += "ValidateOnValue=" + document.getElementById("txtValOnVal").value + ",";
        data += "FieldName2=" + this.FieldName2 + ",";
        data += "FieldName3=" + this.FieldName3 + ",";
        data += "LIFldRef=" + document.getElementById("ddlOBPSDynamicListFIeldRef").value + ",";
        data += "isDeleted=" + this.isDeleted + ",";
        data += "ControlName=" + this.ControlName + ",";
        data += "IsARM=" + document.getElementById("txtOBPSDynamicDisplaySeq").checked;
        
        return data;
    }
}