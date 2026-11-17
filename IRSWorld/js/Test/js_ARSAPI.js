var keyword = null;
var ajax_timeout = 60 * 1000;

function ARS_API()
{
    this.commons                        = null;
    this.keyword                        = null;

    this.load = function()
    { 
        AjaxPro.timeoutPeriod = ajax_timeout; 
        
        this.open_division("divARSAPIMain");
        this.dropdown_keyword();
    }
    
    this.close_divisions = function()
    {
        document.getElementById("divARSAPIMain").style.display = 'none';
        document.getElementById("divBodyARSAPIRBDENROLL").style.display = 'none';
        document.getElementById("divBodyARSAPIBENENROLL").style.display = 'none';
        document.getElementById("divBodyARSAPIINFOAMEND").style.display = 'none';
        document.getElementById("divBodyARSAPINEWREMIT").style.display = 'none';
        document.getElementById("divBodyARSAPICHGPWD").style.display = 'none';
        document.getElementById("divBodyARSAPICHKENROLL").style.display = 'none';
        document.getElementById("divBodyARSAPICHKBENE").style.display = 'none';
        document.getElementById("divBodyARSAPICHKAMEND").style.display = 'none';
        document.getElementById("divBodyARSAPICHKREMIT").style.display = 'none';
        document.getElementById("divBodyARSAPICHKREMIT").style.display = 'none';
        document.getElementById("divBodyARSAPICHKRB").style.display = 'none';
        document.getElementById("divBodyARSAPICHKONLINE").style.display = 'none';
        document.getElementById("divBodyARSAPIRBDREMIT").style.display = 'none';
        document.getElementById("divResult").style.display = 'none';
    }
    
    this.open_division = function(division_id)
    {
        this.close_divisions(); document.getElementById(division_id).style.display = 'block';       
    }
    
    this.dropdown_keyword = function()
    {
        IRSWorld.Test.ARS.getdropdown_keyword("ARSKeyword", this.dropdown_keyword_callback)
    }
    
    this.dropdown_keyword_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();         
            this.commons.to_load_dropdownlist("ddlKeyword", result.value, "FldValue", "FldDescription");
            this.commons = null;           
        }
    }
    
    this.keyword = function()
    {
        keyword = document.getElementById("ddlKeyword").selectedIndex;
        switch(keyword)
        {
            case 1:
            {
                this.open_division('divBodyARSAPIRBDENROLL');
                break;
            }
            case 2:
            {
                this.open_division('divBodyARSAPIBENENROLL');
                break;
            }
            case 3:
            {
                this.open_division('divBodyARSAPIINFOAMEND');
                break;
            }
            case 4:
            {
                this.open_division('divBodyARSAPINEWREMIT');
                break;
            }
            case 5:
            {
                this.open_division('divBodyARSAPICHGPWD');
                break;
            }
            case 6:
            {
                this.open_division('divBodyARSAPICHKENROLL');
                break;
            }
            case 7:
            {
                this.open_division('divBodyARSAPICHKBENE');
                break;
            }
            case 8:
            {
                this.open_division('divBodyARSAPICHKAMEND');
                break;
            }
            case 9:
            {
                this.open_division('divBodyARSAPICHKREMIT');
                break;
            }
            case 10:
            {
                this.open_division('divBodyARSAPICHKRB');
                break;
            }
            case 11:
            {
                this.open_division('divBodyARSAPICHKONLINE');
                break;
            }
            case 12:
            {
                this.open_division('divBodyARSAPIRBDREMIT');
                break;
            }
            default :
            {
                alert("Invalid Keyword!");
                break;
            }
        }    
    }
    
    this.rbdenroll_commit_record = function()
    {
        var data_row = "RBDENROLL"                                      + ";";
        data_row  = data_row + document.getElementById("txtRBDENROLLUserId").value                      + ";";
        data_row  = data_row + document.getElementById("txtRBDENROLLPassword").value                    + ";";
        data_row  = data_row + document.getElementById("txtRBDENROLLAgentTransactionRefNo").value       + ";";
        data_row  = data_row + document.getElementById("txtRBDENROLLAgentRemitterCustomerId").value     + ";";
        data_row  = data_row + document.getElementById("txtRBDENROLLAgentBeneCustomerId").value         + ";";
        data_row  = data_row + document.getElementById("txtRBDENROLLRemitterFirstName").value           + ";";
        data_row  = data_row + document.getElementById("txtRBDENROLLRemitterMiddleName").value          + ";";
        data_row  = data_row + document.getElementById("txtRBDENROLLRemitterLastName").value            + ";";
        data_row  = data_row + document.getElementById("txtRBDENROLLRemitterBitrthdate").value          + ";";
        data_row  = data_row + document.getElementById("txtRBDENROLLRemitterBirthplace").value          + ";";
        data_row  = data_row + document.getElementById("txtRBDENROLLRemitterMobileNo").value            + ";";
        data_row  = data_row + document.getElementById("txtRBDENROLLRemitterContactNo").value           + ";";
        data_row  = data_row + document.getElementById("txtRBDENROLLRemitterRmBldg").value              + ";";
        data_row  = data_row + document.getElementById("txtRBDENROLLRemitterStreet").value              + ";";
        data_row  = data_row + document.getElementById("txtRBDENROLLRemitterDistrict").value            + ";";
        data_row  = data_row + document.getElementById("txtRBDENROLLRemitterState").value               + ";";
        data_row  = data_row + document.getElementById("txtRBDENROLLRemitterCity").value                + ";";
        data_row  = data_row + document.getElementById("txtRBDENROLLRemitterRegion").value              + ";";
        data_row  = data_row + document.getElementById("txtRBDENROLLRemitterZipCode").value             + ";";
        data_row  = data_row + document.getElementById("txtRBDENROLLRemitterEmail").value               + ";";
        data_row  = data_row + document.getElementById("txtRBDENROLLRemitterCountryCode").value         + ";";
        data_row  = data_row + document.getElementById("txtRBDENROLLRemitterOccupation").value          + ";";
        data_row  = data_row + document.getElementById("txtRBDENROLLRemitterID1").value                 + ";";
        data_row  = data_row + document.getElementById("txtRBDENROLLRemitterID2").value                 + ";";
        data_row  = data_row + document.getElementById("txtRBDENROLLBeneFirstName").value               + ";";
        data_row  = data_row + document.getElementById("txtRBDENROLLBeneMiddleName").value              + ";";
        data_row  = data_row + document.getElementById("txtRBDENROLLBeneLastName").value                + ";";
        data_row  = data_row + document.getElementById("txtRBDENROLLBeneBirthDate").value               + ";";
        data_row  = data_row + document.getElementById("txtRBDENROLLBeneBirthplace").value              + ";";
        data_row  = data_row + document.getElementById("txtRBDENROLLBeneMobileNo").value                + ";";
        data_row  = data_row + document.getElementById("txtRBDENROLLBeneContactNo").value               + ";";
        data_row  = data_row + document.getElementById("txtRBDENROLLBeneRmBldg").value                  + ";";
        data_row  = data_row + document.getElementById("txtRBDENROLLBeneStreet").value                  + ";";
        data_row  = data_row + document.getElementById("txtRBDENROLLBeneDistrict").value                + ";";
        data_row  = data_row + document.getElementById("txtRBDENROLLBeneState").value                   + ";";
        data_row  = data_row + document.getElementById("txtRBDENROLLBeneCity").value                    + ";";
        data_row  = data_row + document.getElementById("txtRBDENROLLBeneRegion").value                  + ";";
        data_row  = data_row + document.getElementById("txtRBDENROLLBeneZipCode").value                 + ";";
        data_row  = data_row + document.getElementById("txtRBDENROLLBeneEmail").value                   + ";";
        data_row  = data_row + document.getElementById("txtRBDENROLLBeneCountryCode").value                  ;
        
        IRSWorld.Test.ARS.commit_record(data_row, this.commit_record_callback)
        
    }
    
    this.benenroll_commit_record = function()
    {
        var data_row = "BENENROLL"                                      + ";";
        data_row  = data_row + document.getElementById("txtBENENROLLUSerId").value                      + ";";
        data_row  = data_row + document.getElementById("txtBENENROLLPassword").value                    + ";";
        data_row  = data_row + document.getElementById("txtBENENROLLAgentTransactionRefNo").value       + ";";
        data_row  = data_row + document.getElementById("txtBENENROLLAgentRemitterCustomerId").value     + ";";
        data_row  = data_row + document.getElementById("txtBENENROLLAgentBeneCustomerId").value         + ";";
        data_row  = data_row + document.getElementById("txtBENENROLLBeneFirstName").value               + ";";
        data_row  = data_row + document.getElementById("txtBENENROLLBeneMiddleName").value              + ";";
        data_row  = data_row + document.getElementById("txtBENENROLLBeneLastName").value                + ";";
        data_row  = data_row + document.getElementById("txtBENENROLLBeneBirthDate").value               + ";";
        data_row  = data_row + document.getElementById("txtBENENROLLBeneBirthplace").value              + ";";
        data_row  = data_row + document.getElementById("txtBENENROLLBeneMobileNo").value                + ";";
        data_row  = data_row + document.getElementById("txtBENENROLLBeneContactNo").value               + ";";
        data_row  = data_row + document.getElementById("txtBENENROLLBeneRmBldg").value                  + ";";
        data_row  = data_row + document.getElementById("txtBENENROLLBeneStreet").value                  + ";";
        data_row  = data_row + document.getElementById("txtBENENROLLBeneDistrict").value                + ";";
        data_row  = data_row + document.getElementById("txtBENENROLLBeneState").value                   + ";";
        data_row  = data_row + document.getElementById("txtBENENROLLBeneCity").value                    + ";";
        data_row  = data_row + document.getElementById("txtBENENROLLBeneRegion").value                  + ";";
        data_row  = data_row + document.getElementById("txtBENENROLLBeneZipCode").value                 + ";";
        data_row  = data_row + document.getElementById("txtBENENROLLBeneEmail").value                   + ";";
        data_row  = data_row + document.getElementById("txtBENENROLLBeneCountryCode").value                  ;
        
        IRSWorld.Test.ARS.commit_record(data_row, this.commit_record_callback)
         
    }
    
    this.infoamend_commit_record = function()
    {
        var data_row = "INFOAMEND"                                  + ";";
        data_row  = data_row + document.getElementById("txtINFOAMENDUSerId").value                  + ";";
        data_row  = data_row + document.getElementById("txtINFOAMENDPassword").value                + ";";
        data_row  = data_row + document.getElementById("txtINFOAMENDAgentTransactionRefNo").value   + ";";
        data_row  = data_row + document.getElementById("txtINFOAMENDAgentCustomerId").value         + ";";
        data_row  = data_row + document.getElementById("txtINFOAMENDFirstName").value               + ";";
        data_row  = data_row + document.getElementById("txtINFOAMENDMiddleName").value              + ";";
        data_row  = data_row + document.getElementById("txtINFOAMENDLastName").value                + ";";
        data_row  = data_row + document.getElementById("txtINFOAMENDBirthDate").value               + ";";
        data_row  = data_row + document.getElementById("txtINFOAMENDBirthplace").value              + ";";
        data_row  = data_row + document.getElementById("txtINFOAMENDMobileNo").value                + ";";
        data_row  = data_row + document.getElementById("txtINFOAMENDContactNo").value               + ";";
        data_row  = data_row + document.getElementById("txtINFOAMENDRmBldg").value                  + ";";
        data_row  = data_row + document.getElementById("txtINFOAMENDStreet").value                  + ";";
        data_row  = data_row + document.getElementById("txtINFOAMENDDistrict").value                + ";";
        data_row  = data_row + document.getElementById("txtINFOAMENDState").value                   + ";";
        data_row  = data_row + document.getElementById("txtINFOAMENDCity").value                    + ";";
        data_row  = data_row + document.getElementById("txtINFOAMENDRegion").value                  + ";";
        data_row  = data_row + document.getElementById("txtINFOAMENDZipCode").value                 + ";";
        data_row  = data_row + document.getElementById("txtINFOAMENDEmail").value                   + ";";
        data_row  = data_row + document.getElementById("txtINFOAMENDCountryCode").value                  ;
        
        IRSWorld.Test.ARS.commit_record(data_row, this.commit_record_callback)
        
    }
    
    this.newremit_commit_record = function()
    {
        var data_row = "NEWREMIT"                                     + ";";
        data_row  = data_row + document.getElementById("txtNEWREMITUserId").value                      + ";";
        data_row  = data_row + document.getElementById("txtNEWREMITPassword").value                    + ";";
        data_row  = data_row + document.getElementById("txtNEWREMITAgentTransactionRefNo").value       + ";";
        data_row  = data_row + document.getElementById("txtNEWREMITAgentRemitterCustomerId").value     + ";";
        data_row  = data_row + document.getElementById("txtNEWREMITAgentBeneCustomerId").value         + ";";
        data_row  = data_row + document.getElementById("txtNEWREMITRemitterFirstName").value           + ";";
        data_row  = data_row + document.getElementById("txtNEWREMITRemitterMiddleName").value          + ";";
        data_row  = data_row + document.getElementById("txtNEWREMITRemitterLastName").value            + ";";
        data_row  = data_row + document.getElementById("txtNEWREMITRemitterAccountNo").value           + ";";
        data_row  = data_row + document.getElementById("txtNEWREMITBeneFirstName").value               + ";";
        data_row  = data_row + document.getElementById("txtNEWREMITBeneMiddleName").value              + ";";
        data_row  = data_row + document.getElementById("txtNEWREMITBeneLastName").value                + ";";
        data_row  = data_row + document.getElementById("txtNEWREMITProductCode").value                 + ";";
        data_row  = data_row + document.getElementById("txtNEWREMITAccountNo").value                   + ";";
        data_row  = data_row + document.getElementById("txtNEWREMITGFMCCardNo").value                  + ";";
        data_row  = data_row + document.getElementById("txtNEWREMITBankName").value                    + ";";
        data_row  = data_row + document.getElementById("txtNEWREMITSourceAmount").value                + ";";
        data_row  = data_row + document.getElementById("txtNEWREMITSourceCurrency").value              + ";";
        data_row  = data_row + document.getElementById("txtNEWREMITSourcetoUSD").value                 + ";";
        data_row  = data_row + document.getElementById("txtNEWREMITSourceToPHP").value                 + ";";
        data_row  = data_row + document.getElementById("txtNEWREMITUSDToPHP").value                    + ";";
        data_row  = data_row + document.getElementById("txtNEWREMITServiceCharge").value               + ";";
        data_row  = data_row + document.getElementById("txtNEWREMITTargetAmount").value                + ";";
        data_row  = data_row + document.getElementById("txtNEWREMITTargetCurrency").value              + ";";
        data_row  = data_row + document.getElementById("txtNEWREMITRemittancePurpose").value                ;
        
        IRSWorld.Test.ARS.commit_record(data_row, this.commit_record_callback)
        
    }
    
    this.chgpwd_commit_record = function()
    {
        var data_row = "CHGPWD"                                   + ";";
        data_row  = data_row + document.getElementById("txtCHGPWDUserId").value                      + ";";
        data_row  = data_row + document.getElementById("txtCHGPWDPassword").value                    + ";";
        data_row  = data_row + document.getElementById("txtCHGPWDAgentTransactionRefNo").value       + ";";
        data_row  = data_row + document.getElementById("txtCHGPWDNewPassword").value                      ;
        
        IRSWorld.Test.ARS.commit_record(data_row, this.commit_record_callback)
        
    }
    
    this.chkenroll_commit_record = function()
    {
        var data_row = "CHKENROLL"                                   + ";";
        data_row  = data_row + document.getElementById("txtCHKENROLLUserId").value                   + ";";
        data_row  = data_row + document.getElementById("txtCHKENROLLPassword").value                 + ";";
        data_row  = data_row + document.getElementById("txtCHKENROLLTransactionRefNo").value              ;
        
        IRSWorld.Test.ARS.commit_record(data_row, this.commit_record_callback)
        
    }
    
    this.chkbene_commit_record = function()
    {
        var data_row = "CHKBENE"                                 + ";";
        data_row  = data_row + document.getElementById("txtCHKBENEUserId").value                   + ";";
        data_row  = data_row + document.getElementById("txtCHKBENEPassword").value                 + ";";
        data_row  = data_row + document.getElementById("txtCHKBENETransactionRefNo").value              ;
        
        IRSWorld.Test.ARS.commit_record(data_row, this.commit_record_callback)
        
    }
    
    this.chkamend_commit_record = function()
    {
        var data_row = "CHKAMEND"                                  + ";";
        data_row  = data_row + document.getElementById("txtCHKAMENDUserId").value                   + ";";
        data_row  = data_row + document.getElementById("txtCHKAMENDPassword").value                 + ";";
        data_row  = data_row + document.getElementById("txtCHKAMENDTransactionRefNo").value              ;
        
        IRSWorld.Test.ARS.commit_record(data_row, this.commit_record_callback)
        
    }
    
    this.chkremit_commit_record = function()
    {
        var data_row = "CHKREMIT"                                  + ";";
        data_row  = data_row + document.getElementById("txtCHKREMITUserId").value                   + ";";
        data_row  = data_row + document.getElementById("txtCHKREMITPassword").value                 + ";";
        data_row  = data_row + document.getElementById("txtCHKREMITTransactionRefNo").value              ;
        
        IRSWorld.Test.ARS.commit_record(data_row, this.commit_record_callback)
        
    }
    
    this.chkrb_commit_record = function()
    {
        var data_row = "CHKRB"                              + ";";
        data_row  = data_row + document.getElementById("txtCHKRBUserId").value                   + ";";
        data_row  = data_row + document.getElementById("txtCHKRBPassword").value                 + ";";
        data_row  = data_row + document.getElementById("txtCHKRBAgentRemitterCustomerID").value  + ";";
        data_row  = data_row + document.getElementById("txtCHKRBAgentBeneCustomerID").value           ;
        
        IRSWorld.Test.ARS.commit_record(data_row, this.commit_record_callback)
        
    }
    
    this.chkonline_commit_record = function()
    {
        var data_row = "CHKONLINE"                                   + ";";
        data_row  = data_row + document.getElementById("txtCHKONLINEUserId").value                   + ";";
        data_row  = data_row + document.getElementById("txtCHKONLINEPassword").value                 + ";";
        data_row  = data_row + document.getElementById("txtCHKONLINEAccountNo").value                     ;
        
        IRSWorld.Test.ARS.commit_record(data_row, this.commit_record_callback)
        
    }
    
    this.rbdremit_commit_record = function()
    {
        var data_row = "RBDREMIT"                                      + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITUserId").value                      + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITPassword").value                    + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITAgentTransactionRefNo").value       + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITAgentRemitterCustomerId").value     + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITAgentBeneCustomerId").value         + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITRemitterFirstName").value           + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITRemitterMiddleName").value          + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITRemitterLastName").value            + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITRemitterBirthDate").value          + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITRemitterBirthPlace").value          + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITRemitterMobileNo").value            + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITRemitterContactNo").value           + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITRemitterRmBldg").value              + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITRemitterStreet").value              + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITRemitterDistrict").value            + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITRemitterState").value               + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITRemitterCity").value                + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITRemitterRegion").value              + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITRemitterZipCode").value             + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITRemitterEmail").value               + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITRemitterCountryCode").value         + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITRemitterOccupation").value          + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITRemitterID1").value                 + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITRemitterID2").value                 + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITRemitterAccountNo").value           + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITBeneFirstName").value               + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITBeneMiddleName").value              + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITBeneLastName").value                + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITBeneBirthDate").value               + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITBeneBirthplace").value              + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITBeneMobileNo").value                + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITBeneContactNo").value               + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITBeneRmBldg").value                  + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITBeneStreet").value                  + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITBeneDistrict").value                + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITBeneState").value                   + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITBeneCity").value                    + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITBeneRegion").value                  + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITBeneZipCode").value                 + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITBeneEmail").value                   + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITBeneCountryCode").value             + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITProductCode").value                 + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITAccountNo").value                   + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITGFMCCardNo").value                  + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITBankName").value                    + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITSourceAmount").value                + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITSourceCurrency").value              + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITSourcetoUSD").value                 + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITSourceToPHP").value                 + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITUSDToPHP").value                    + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITServiceCharge").value               + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITTargetAmount").value                + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITTargetCurrency").value              + ";";
        data_row  = data_row + document.getElementById("txtRBDREMITRemittancePurpose").value           + ";";     
        data_row  = data_row + document.getElementById("txtRBDREMITTransactionType").value              ;
        
        IRSWorld.Test.ARS.commit_record(data_row, this.commit_record_callback)
        
    }
    
    
    this.commit_record_callback = function(result)
    {
        alert("Transaction Passed to PNB ARS API!");
        if(result == null || result.value == null)
        {
            document.getElementById("lblResult").innerText = "Web service not available : \n" + result.error.Message;
        }
        else
        {
            document.getElementById("lblResult").innerText = result.value;
//            document.getElementById("divBodyARSAPI").style.display = 'none';
//            document.getElementById("divResult").style.display = 'block';

        }
        ARS_API.open_division('divResult');
    }
    
}
    