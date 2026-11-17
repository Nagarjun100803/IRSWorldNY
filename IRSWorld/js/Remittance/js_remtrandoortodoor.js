

function remtrandoortodoor()
{
    this.load_all_dropdownlist=function()
        {
            this.dropdown_purpose();
            this.dropdown_bank();
        }
     
    this.dropdown_purpose=function()
        {
            RemTranDoorToDoor.getdropdown_source("TranxPurpose",this.dropdown_purpose_callback)
        }
     
    this.dropdown_purpose_callback=function(result)
        {
            if (result !=null && result !="undefined")
                { 
                    this.commons=new common_method();
                    this.commons.to_load_dropdownlist("ddlDoortoDoorRemPurpose", result.value,"FldValue","FldDescription");
                    this.commons=null;  
                }   
        }
    this.dropdown_bank=function()
        { 
            RemTranDoorToDoor.getdropdown_source_1("MegalinkIBFTBank", this.dropdown_bank_callback)
        }
    this.dropdown_bank_callback=function(result)
        { 
            if (result != null && result != "undefined")
                {
                    this.commons=new common_method();
                    this.commons.to_load_dropdownlist("ddlDoortoDoorRemBank", result.value, "FldValue","FldDescription");
                    this.commons-null;
                }
        }
}