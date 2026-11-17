function cif_remitters()
{
    this.generic          = null;
    this.SEQ_ID           = null;
    this.CIFID            = null;
    this.VIPStatus        = null;
    this.RemitterPoints   = null;
    this.ChargeType       = null;
    this.RiskRating       = null;
    var iRem              = this;  

    this.load_record = function()
    {       
        this.pull_server_data();        
    }
    
    this.set_name_module = function(module_name)
    {
        IRSWorld.CIF.cifmain.set_module_name(module_name);
    }
    
    this.set_record = function()
    {    
        this.VIPStatus      = document.getElementById("ddVIPStatus").value; 
        this.RemitterPoints = document.getElementById("txtRemitterPoints").value; 
        this.ChargeType     = document.getElementById("txtHiddenChargeTypeFldVal").value;
        this.RiskRating     = document.getElementById("ddRiskRating").value;
        
        return this.push_data_to_server(); 
    }
    
    this.pull_server_data = function()
    {
        IRSWorld.CIF.cifmain.GET_REMITTERS_RECORD(IRSWorld.CIF.cifmain.GET_REMITTERS_SEQ_ID(this.CIFID).value,this.pull_server_data_callback);
    }   
     
    this.pull_server_data_callback = function(result)
    {                        
        if (result != null && result.value != null)
        {            
            var ds = result.value;
                        
            if (ds.Tables[0].Rows.length > 0)
            {
                iRem.SEQ_ID         = ds.Tables[0].Rows[0].SEQ_ID;
                iRem.VIPStatus      = ds.Tables[0].Rows[0].VIPStatus;
                iRem.RemitterPoints = ds.Tables[0].Rows[0].RemitterPoints;
                iRem.ChargeType     = ds.Tables[0].Rows[0].ChargeType;
                iRem.RiskRating     = ds.Tables[0].Rows[0].RiskRating;
                
                document.getElementById("txtHiddenSeqId").value        = iRem.SEQ_ID; 
                document.getElementById("txtHiddenCIFID").value        = iRem.CIFID; 
                
                document.getElementById("ddVIPStatus").value           = iRem.VIPStatus; 
                document.getElementById("txtRemitterPoints").value     = iRem.RemitterPoints; 
                document.getElementById("ddRiskRating").value          = iRem.RiskRating; 
                
                document.getElementById("txtHiddenChargeTypeFldVal").value = iRem.ChargeType;
                if(iRem.ChargeType != null)
                {
                    this.remitters = new cif_remitters();
                    this.remitters.get_charge_type_desc(iRem.ChargeType);
                }                
                
                this.generic = new generic_method();
                this.generic.to_clear_null("txtRemitterPoints~lblChargeType");
            }
        }          
    }
        
    this.get_charge_type_desc = function(chargetype)
    {
        document.getElementById("lblChargeType").innerHTML = '';
        
        var ds = null;
        ds = IRSWorld.CIF.cifmain.get_chargetype(chargetype).value;
        
        if(ds.Tables[0].Rows.length > 0) document.getElementById("lblChargeType").innerHTML = ds.Tables[0].Rows[0].FldDescription;
    }
    
    this.push_data_to_server = function()
    {
        var data_row;
        
        data_row = new Array();
        
        data_row[data_row.length]  = "CIFID~"           + this.CIFID;
        data_row[data_row.length]  = "VIPStatus~"       + this.VIPStatus;
        data_row[data_row.length]  = "RemitterPoints~"  + this.RemitterPoints;
        data_row[data_row.length]  = "ChargeType~"      + this.ChargeType;
        data_row[data_row.length]  = "RiskRating~"      + this.RiskRating;

        return data_row.join("|");        
    }
    
    this.commit_record = function()
    {
        IRSWorld.CIF.cifmain.commit_record( this.set_record(), this.commit_record_callback );
    }
    
    this.commit_record_callback = function(result)
    {
	    iRem.load_record();
        
        if (typeof(iRem.load_grid_viewer) != 'undefined')
        {
            iRem.load_grid_viewer();
        } 
            
        if (result.error != null && result.error != "undefined")
        {            
            alert("UNABLE TO COMMIT RECORD. " + result.error); return;
        }
                                
        if (result != null && result.value != "undefined")
        {                                
            if (result.value == false)
            {     
                var msg = IRSWorld.CIF.cifmain.read_message();
                   
                if (msg != null && msg.value != "undefined")
                {
                    alert(msg.value); return;
                }                
            }
            
            alert("RECORD COMMITTED SUCCESSFULLY");
         }
    }
    
    this.set_branchdef_values = function()
    {
        var ds = IRSWorld.CIF.cifmain.get_branchdef_values(document.getElementById("txtHiddenUserLocation").value).value;
        
        if(ds.Tables[0].Rows.length > 0)
        {
            this.VIPStatus  = ds.Tables[0].Rows[0].VIPCardStatus;
            this.ChargeType = ds.Tables[0].Rows[0].ChargeType;
            this.RiskRating = ds.Tables[0].Rows[0].RiskRating;
            
            this.commons = new common_method();
            this.commons.to_set_dropdownlist_index("ddVIPStatus",this.VIPStatus);
            this.commons.to_set_dropdownlist_index("ddRiskRating",this.RiskRating);
            
            this.commons = null;
            
            document.getElementById("txtHiddenChargeTypeFldDesc").value = '';
            document.getElementById("txtHiddenChargeTypeFldVal").value  = this.ChargeType;
            
            if(this.ChargeType != null)
            {
                var ds = IRSWorld.CIF.cifmain.get_chargetype(this.ChargeType).value;
                if (ds.Tables[0].Rows.length > 0) var charge_type = ds.Tables[0].Rows[0].FldDescription;
                
                document.getElementById("lblChargeType").innerHTML          = charge_type;
                document.getElementById("txtHiddenChargeTypeFldDesc").value = charge_type;
            }
            
            alert('Default values are shown only but not yet saved. Please click Update button to save the record.');
        }
    }
    
    this.validate_input = function()
    {
        this.generic     = new generic_method();        
        this.error_input = '';
        var foc_elem     = '';
        
        if (document.getElementById("ddVIPStatus").value == 'null' &&
            this.generic.is_empty(document.getElementById("txtRemitterPoints").value) == true && 
            this.generic.is_empty(document.getElementById("txtHiddenChargeTypeFldVal").value) == true)
        {
            this.error_input += "Nothing to be saved.";
            foc_elem = 'ddVIPStatus';
        }
        if(this.generic.is_numeric(document.getElementById("txtRemitterPoints").value) == false)
        {   
            this.error_input += "Invalid remitter points";
            if (foc_elem.length == 0) foc_elem = 'txtRemitterPoints';    
        }
        if (foc_elem.length > 0)
        {
            document.getElementById(foc_elem).focus();
        }
        if (this.error_input != '')
        {
            return false;
        }
        return true;
    }
}