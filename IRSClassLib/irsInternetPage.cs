using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Net;
using System.Net.Mail;

namespace IRSClassLib
{
    [Serializable]
    public class irsInternetPage : sqlHelperWs
    {
        public bool _bDate = false;

        private String host;
        private int port;
        private String user;
        private String password;
        private MailAddress from;

        private bool use_credential;
        private bool enable_ssl;
        private int timeout;

        private string isRemittanceAggregate;

        private String _message;

        public irsInternetPage()
        {
            //base.IsWinApp = true;
        }

        public String Message
        {
            get { return _message; }
        }


        public string getAgentAccountBalance(string BranchCode, int Currency)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getAgentAccountBalance", new Object[] { BranchCode, Currency });
            if (hasRows)
            {
                selectedRow = 0;
                return table.Rows[0]["AccountNumber"].ToString().Trim();
            }
            else
            {
                return null;
            }
        }

        public DataTable getAgentCurrency(string BranchCode)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getAgentCurrency", new Object[] { BranchCode });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return null;
            }
        }

        public DataTable getAgentRemitter(string BeneficiaryCIFID, string RemitterCIFID)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getAgentRemitter", new Object[] { BeneficiaryCIFID, RemitterCIFID });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return null;
            }
        }

        public string getAgentRoles(int AgentType)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getAgentRoles", new Object[] { AgentType });
            if (hasRows)
            {
                selectedRow = 0;
                return table.Rows[0][0].ToString().Trim();
            }
            else
            {
                return null;
            }
        }

        public DataTable getBeneficiary(string RemitterCIFID, string userRole)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getBeneficiary", new Object[] { RemitterCIFID, userRole });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return null;
            }
        }

        public DataTable getBeneficiaryAddress(string CIFID)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getAddress", new Object[] { CIFID });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return null;
            }
        }

        public DataTable getBeneficiaryContacts(string CIFID)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getContacts", new Object[] { CIFID });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return null;
            }
        }

        public DataTable getBranches(string BranchCode)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getBranches", new Object[] { BranchCode });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return null;
            }
        }

        public string getBranchNSVS(string BranchCode, int Currency)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getBranchNSVS", new Object[] { BranchCode, Currency });
            if (hasRows)
            {
                selectedRow = 0;
                return table.Rows[0][0].ToString().Trim();
            }
            else
            {
                return string.Empty;
            }
        }

        public DataTable getBranchOperations(string RegionID)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getBranchOperation", new Object[] { RegionID });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return null;
            }
        }

        public string getBranchValue(string BranchCode, string FindColumn, string LookValue, string ReturnColumn)
        {
            DataTable dt = this.getBranches(BranchCode);
            string sReturn = string.Empty;
            int i = 0;

            for (i = 0; i <= (dt.Rows.Count - 1); i++)
            {
                if (dt.Rows[i][FindColumn].ToString().Trim() == LookValue)
                {
                    sReturn = dt.Rows[i][ReturnColumn].ToString().Trim();
                }
            }
            return sReturn;
        }

        public string getCIFAccount(string SEQ_ID)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getCIFAccount", new Object[] { SEQ_ID });
            if (hasRows)
            {
                selectedRow = 0;
                return table.Rows[0][0].ToString().Trim();
            }
            else
            {
                return string.Empty;
            }
        }

        public DataTable getCIFHit(string BranchCode, string CIFID)
        {
            base.EnforceConstraint = false;
            //DataTable dt = getDefaults(BranchCode);
            //string sStreamLining = string.Empty;
            //if (dt != null)
            //{
            //    if (dt.Rows.Count > 0)
            //    {
            //        sStreamLining = dt.Rows[0]["StreamLining"].ToString().Trim();
            //    }
            //}
            getUpdateDS("isp_getCIFHit", new Object[] { BranchCode, CIFID });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return null;
            }
        }

        public DataTable getCIFInfo(string CIFID)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getCIFInfo", new Object[] { CIFID });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return null;
            }
        }

        public DataTable getCIFInformation(string CIFID)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getCIFInformation", new Object[] { CIFID });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return null;
            }
        }

        public string getCharges(string Amount, int ChargeID, int Currency, int ComputationLevel, bool IsLocalCharge, bool IsRemittance, bool ReCompute, string Percentage)
        {
            irsCalculator irsCalc = null;
            try
            {
                string sReturn = string.Empty;
                irsCalc = new irsCalculator(ChargeID, Currency, ComputationLevel, IsLocalCharge, Convert.ToDecimal(Percentage) / 100);

                if (IsRemittance == true)
                {
                    if (ReCompute == true)
                    {
                        if (System.Web.HttpContext.Current.Session["ARM_Calculator"] != null)
                        {
                            irsCalc = (irsCalculator)System.Web.HttpContext.Current.Session["ARM_Calculator"];
                            irsCalc.recompute_irsw_charges(Convert.ToDecimal(Amount));
                        }
                    }
                    else
                    {
                        irsCalc.compute_irsw_charges(Convert.ToDecimal(Amount));
                    }
                }
                else
                {
                    irsCalc.compute_obps_charges(Convert.ToDecimal(Amount));
                }
                sReturn = irsCalc.get_branch_local_currency().ToString() + "|";                 // [0] Branch Local Currency Field Value
                sReturn += irsCalc.get_forex_remitting_to_account().ToString("#,###.0000") + "|"; // [1] Source to Account Forex Amount
                sReturn += irsCalc.get_forex_remitting_to_account_code().ToString() + "|";      // [2] Benef Account Currency Field Value
                sReturn += irsCalc.get_forex_remitting_to_account_code_desc().ToString() + "|"; // [3] Benef Account Currency Code
                sReturn += irsCalc.get_forex_source_to_php().ToString("#,###.00") + "|";        // [4] Source to PHP Forex Amount
                sReturn += irsCalc.get_forex_source_to_php_code().ToString() + "|";             // [5] Source to PHP Field Value
                sReturn += irsCalc.get_forex_source_to_php_code_desc().ToString() + "|";        // [6] Source to PHP Code
                sReturn += irsCalc.get_forex_source_to_target().ToString("#,###.00") + "|";     // [7] Source to Remitting Currency Forex Amount
                sReturn += irsCalc.get_forex_source_to_target_code().ToString() + "|";          // [8] Source to Remitting Field Value
                sReturn += irsCalc.get_forex_source_to_target_code_desc().ToString() + "|";     // [9] Source to Remitting Code
                sReturn += irsCalc.get_forex_source_to_usd().ToString("#,###.00") + "|";        // [10] Source to USD Forex Amount
                sReturn += irsCalc.get_forex_source_to_usd_code().ToString() + "|";             // [11] Source to USD Field Value
                sReturn += irsCalc.get_forex_source_to_usd_code_desc().ToString() + "|";        // [12] Source to USD Code
                sReturn += irsCalc.get_forex_target_to_account().ToString("#,###.00") + "|";    // [13] Remitting to Benef Account Forex Amount
                sReturn += irsCalc.get_forex_target_to_account_code().ToString() + "|";         // [14] Benef Account Field Value
                sReturn += irsCalc.get_forex_target_to_account_code_desc().ToString() + "|";    // [15] Benef Account Code
                sReturn += irsCalc.get_forex_target_to_eur().ToString("#,###.00") + "|";        // [16] Remitting to EUR Forex Amount
                sReturn += irsCalc.get_forex_target_to_eur_code_desc().ToString() + "|";        // [17] Remitting to EUR Code
                sReturn += irsCalc.get_forex_target_to_php().ToString("#,###.00") + "|";        // [18] Remitting to PHP Forex Amount
                sReturn += irsCalc.get_forex_target_to_php_code_desc().ToString() + "|";        // [19] Remitting to PHP Code
                sReturn += irsCalc.get_forex_target_to_source().ToString("#,###.00") + "|";     // [20] Remitting to Source Forex Amount
                sReturn += irsCalc.get_forex_target_to_usd().ToString() + "|";                  // [21] Remitting to USD Forex Amount
                sReturn += irsCalc.get_forex_target_to_usd_curr_code_desc().ToString() + "|";   // [22] Remitting to USD Code
                sReturn += irsCalc.get_remittance_base_amount().ToString("#,###.00") + "|";     // [23] 
                sReturn += irsCalc.get_remittance_gross_branch().ToString("#,###.00") + "|";    // [24] Gross Amount (plus charges)
                sReturn += irsCalc.get_remittance_net_branch().ToString("#,###.00") + "|";      // [25] Net Amount (less charges)
                sReturn += irsCalc.get_remittance_net_local().ToString("#,###.00") + "|";       // [26] Net Amount (less charges)
                sReturn += irsCalc.get_remittance_net_remit_amt().ToString("#,###.0000") + "|";   // [27] Net Amount (less charges)
                sReturn += irsCalc.get_remittance_net_remit_php_equivalent().ToString("#,###.00") + "|"; // [28] Net to Benef PHP Converted Amount
                sReturn += irsCalc.get_remittance_net_to_bene_amt().ToString("#,###.00") + "|"; // [29] Net to Benef Amount
                sReturn += irsCalc.get_remittance_total_charges().ToString() + "|";             // [30] Total Charge Field Value
                sReturn += irsCalc.get_remittance_total_charges_amt().ToString("#,###.00") + "|"; // [31] Total Charges Amount
                sReturn += irsCalc.get_target_to_eur_curr_code().ToString() + "|";              // [32] Remitting EUR Currency Field Value
                sReturn += irsCalc.get_target_to_php_curr_code().ToString() + "|";              // [33] Remitting PHP Currency Field Value
                sReturn += irsCalc.get_target_to_usd_curr_code().ToString() + "|";              // [34] Remitting to USD Currency Field Value
                sReturn += irsCalc.get_forex_remitting_currency_code().ToString() + "|";        // [35] Remitting Currency Value
                sReturn += irsCalc.get_forex_remitting_currency_desc().ToString() + "|";        // [36] Remitting Currency Code
                sReturn += irsCalc.get_obps_actual_payment().ToString("#,###.00") + "|";        // [37] OBPS Actual Payment [For Excess Payment]
                sReturn += irsCalc.get_total_backend_charges().ToString("#,###.00") + "|";      // [38] Total Backend Charges
                //sReturn = irsCalc.get_transaction_charge_screen();
                System.Web.HttpContext.Current.Session["ARM_Calculator"] = irsCalc;
                return sReturn;
            }
            catch (Exception ex)
            {
                irsCalc = null;
                return ex.Message;
            }
            finally
            {
                irsCalc = null; 
            }
        }

        public DataTable getChargesDefaults(string ChargeID)
        {
            base.EnforceConstraint = false;
            getUpdateDS("sp_calculator_product_charges", new Object[] { ChargeID });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return null;
            }
        }

        public string getChargesID(string BranchCode, string ProductCode, string ImplementationType, string Currency, string ChargeType, string PayMode)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getChargesID", new Object[] { BranchCode, ProductCode, ImplementationType, Currency, ChargeType, PayMode });
            if (hasRows)
            {
                selectedRow = 0;
                return table.Rows[0][0].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        public bool getChargesUser(string ProductChargesID, string Role)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getChargesUser", new Object[] { ProductChargesID, Role });
            if (hasRows)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public DataTable getDCCard(string CIFID, string cardclass)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getDCCard", new Object[] { CIFID, cardclass });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return null;
            }
        }

        public DataTable getDCCard(string CIFID)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getDCCard", new Object[] { CIFID, "0" });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return null;
            }
        }

        public DataTable getDefaults(string BranchCode)
        {
            base.EnforceConstraint = false;
            getUpdateDS("spGetBranchDefaultSetup", new Object[] { BranchCode });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return null;
            }
        }

        public DataSet getException(string CIFID, string BranchCode)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getRemitterException", new Object[] { CIFID, BranchCode });
            if (hasRows)
            {
                selectedRow = 0;
                return upds;
            }
            else
            {
                return null;
            }
        }

        public DataTable getExceptionCount(string CIFID, string BranchCode)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getRemitterExceptionCount", new Object[] { CIFID, BranchCode });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return null;
            }
        }

        public string getExchangeRate(string BranchCode, string SourceCurrency, string TargetCurrency, bool isAgent)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getExchangeRate", new Object[] { BranchCode, SourceCurrency, TargetCurrency, isAgent });
            if (hasRows)
            {
                selectedRow = 0;
                return "1 " + table.Rows[0]["SourceCurrencyCode"] + " to " + table.Rows[0]["TargetCurrencyCode"] + " " + table.Rows[0]["SourceToTarget"];
            }
            else
            {
                return null;
            }
        }

        public DataTable getExpiredIDs(string RemitterCIFID, DateTime BranchDate)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getExpiredIDs", new Object[] { RemitterCIFID, BranchDate });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return null;
            }
        }

        public DataTable getExpiringIDs(string BranchCode, string RemitterCIFID, DateTime BranchDate)
        {
            base.EnforceConstraint = false;
            DataTable dt = getDefaults(BranchCode);
            string sThreshold = string.Empty;
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    sThreshold = dt.Rows[0]["IDThreshold"].ToString().Trim();
                }
            }
            getUpdateDS("isp_getExpiringIDs", new Object[] { RemitterCIFID, BranchDate, sThreshold });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return null;
            }
        }

        public string getDisclaimer(string Region)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getDisclaimer", new Object[] { Region });
            if (hasRows)
            {
                selectedRow = 0;
                return table.Rows[0][2].ToString();
            }
            else
            {
                return null;
            }
        }

        public DataTable getLookup(string GroupName)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getLookup", new Object[] { GroupName });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return null;
            }
        }
        public bool getParamForApproval(string branchCode, string moduleName)
        {
            base.EnforceConstraint = false;
            getUpdateDS("sp_paramApproval_CheckForApproval", new Object[] { branchCode, moduleName });
            if (hasRows)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string getLookupValue(string GroupName, string FindColumn, string LookValue, string ReturnColumn)
        {
            DataTable dt = this.getLookup(GroupName);
            string sReturn = string.Empty;
            int i = 0;

            /* VAPT 20181016 */
            if (dt != null && dt.Rows.Count > 0)
            {
                for (i = 0; i <= (dt.Rows.Count - 1); i++)
                {
                    if (dt.Rows[i][FindColumn].ToString().Trim() == LookValue)
                    {
                        sReturn = dt.Rows[i][ReturnColumn].ToString().Trim();
                    }
                }
            }
            return sReturn;
        }

        public DataTable getSubLookup(string BranchCode, string GroupName)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getSubLookup", new Object[] { BranchCode, GroupName });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return null;
            }
        }

        public string getInternetLimit(string BranchCode)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getInternetLimit", new Object[] { BranchCode });
            if (hasRows)
            {
                selectedRow = 0;
                if (table.Rows[0]["InetLimit"].ToString() == string.Empty)
                {
                    return "0";
                }
                else
                {
                    return table.Rows[0]["InetLimit"].ToString();
                }
            }
            else
            {
                return null;
            }
        }

        public string getRemittanceLimit(string BranchCode)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getRemittanceLimit", new Object[] { BranchCode });
            if (hasRows)
            {
                selectedRow = 0;                
                if (table.Rows[0]["RemittanceAmtLimit"].ToString() == string.Empty)
                {
                    return "0";
                }
                else
                {
                    this.isRemittanceAggregate = table.Rows[0]["AggregateAmtLimit"].ToString();
                    return table.Rows[0]["RemittanceAmtLimit"].ToString();
                }
            }
            else
            {
                return null;
            }
        }

        public bool getOnlineProduct(int ProductCode)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getOnlineProduct", new Object[] { ProductCode });
            if (hasRows)
            {
                selectedRow = 0;
                return true;
            }
            else
            {
                return false;
            }
        }

        public string getORDHOBranch(bool IsHO)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getORDHOBranch", new Object[] { IsHO });
            if (hasRows)
            {
                selectedRow = 0;
                return table.Rows[0][0].ToString().Trim();
            }
            else
            {
                return string.Empty;
            }
        }

        public bool getOSA(string BeneficiaryID)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getOSA", new Object[] { BeneficiaryID });
            if (hasRows)
            {
                selectedRow = 0;
                if (table.Rows[0]["OSA"].ToString().Trim() == "1")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool getOSAZip(int ZipSeqID)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getOSA", new Object[] { ZipSeqID });
            if (hasRows)
            {
                selectedRow = 0;
                if (table.Rows[0]["OSA_Ind"].ToString().Trim() == "1")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool getPaymentGateway(string Region)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getPaymentGateway", new Object[] { Region, "0" });
            if (hasRows)
            {
                selectedRow = 0;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool getPaymentGateway(string Region, string Country)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getPaymentGateway", new Object[] { Region, Country });
            if (hasRows)
            {
                selectedRow = 0;
                return true;
            }
            else
            {
                return false;
            }
        }

        public DataTable getPaymentMethod(string BranchCode)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getPaymentMethod", new Object[] { BranchCode });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return null;
            }
        }

        public DataTable getPNBAccounts(string CIFID, string BranchCode)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getPNBAccounts", new Object[] { CIFID, BranchCode });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return null;
            }
        }

        public DataTable getOtherAccounts(string CIFID)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getOtherAccounts", new Object[] { CIFID });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return null;
            }
        }

        public DataTable getPurpose()
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getPurpose", new Object[] { });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return null;
            }
        }

        public DataTable getSearchBy()
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getSearchBy", new Object[] { });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return null;
            }
        }

        public string getRegion(string BranchCode, bool IsValue)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getRegion", new Object[] { BranchCode });
            if (hasRows)
            {
                selectedRow = 0;
                return (IsValue == true) ? table.Rows[0]["FldValue"].ToString() : table.Rows[0]["FldDescription"].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        public string getRemitterDisclaimer(string RemitterCIFID)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getRemitterDisclaimer", new Object[] { RemitterCIFID });
            if (hasRows)
            {
                selectedRow = 0;
                if (table.Rows[0]["Disclaimer"].ToString().ToLower() == "true")
                {
                    return "1";
                }
                else
                {
                    return "0";
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public string getRemitterEmail(string CIFID)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getRemitterContacts", new Object[] { CIFID });
            int i = 0;
            if (hasRows)
            {
                for (i = 0; i <= (table.Rows.Count - 1); i++)
                {
                    if (table.Rows[i]["EmailAddress"].ToString().Trim() != "")
                    {
                        selectedRow = i;
                        return table.Rows[i]["EmailAddress"].ToString();
                    }
                }
                return string.Empty;
                //selectedRow = 0;
                //return table.Rows[0]["EmailAddress"].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        public DataTable getRemitterName(string CIFID)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getRemitterName", new Object[] { CIFID });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return null;
            }
        }

        public DataTable getRemittance(string ReferenceNo)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getRemittance", new Object[] { ReferenceNo });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return null;
            }
        }

        public DataTable getRemittanceInfo(int SEQ_ID)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getRemittanceInfo", new Object[] { SEQ_ID });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return null;
            }
        }

        public bool getRecordIfDuplicate(string TableName, string ColumnValue)
        {
            base.EnforceConstraint = false;
            int i = 0;
            string[] arr = ColumnValue.Split('|');
            string[] arrColumnValue;
            string sSQL = "SELECT * FROM " + TableName + " WHERE ";

            for (i = 0; i <= (arr.Length - 1); i++)
            {
                arrColumnValue = new string[2];
                arrColumnValue = arr.GetValue(i).ToString().Split('~');
                if (arrColumnValue.GetValue(1).ToString().Trim() != string.Empty)
                {
                    sSQL += arrColumnValue.GetValue(0).ToString().Trim() + " LIKE '%" + arrColumnValue.GetValue(1).ToString().Trim() + "%' AND ";
                }
            }
            sSQL = sSQL.Substring(0, sSQL.Length - 4); //Remove AND
            getUpdateDS(sSQL);
            if (hasRows)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string getReferenceNo(int SEQ_ID)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getReferenceNo", new Object[] { SEQ_ID });
            if (hasRows)
            {
                selectedRow = 0;
                return table.Rows[0][0].ToString().Trim();
            }
            else
            {
                return string.Empty;
            }
        }

        public bool IsNewDate
        {
            get
            {
                return _bDate;
            }
            set
            {
                _bDate = value;
            }
        }

        public DataTable getTodayForex(string BranchCode)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getTodayForex", new Object[] { BranchCode });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return null;
            }
        }

        public DateTime getTransactionDate(string BranchCode, bool CutOff)
        {
            DateTime dtLocalTime = new irsTimeZone().branchLocalTime(BranchCode);
            DateTime dtSelectDate = dtLocalTime;
            int i = 0;
            string[] arrHoliday;
            string[] arrSelected;
            DataRow dr;
            DataTable dt = new DataTable();
            dt.Columns.Add("Holiday");
            _bDate = false;

            base.EnforceConstraint = false;
            getUpdateDS("isp_getHolidays", new Object[] { BranchCode });
            if (hasRows)
            {
                selectedRow = 0;
                for (i = 0; i <= (table.Rows.Count - 1); i++)
                {
                    arrHoliday = table.Rows[i]["Hol_Date"].ToString().Split('/');
                    if (IsNumeric(arrHoliday[2]))
                    {
                        if (Convert.ToDateTime(table.Rows[i]["Hol_Date"].ToString().Trim()).ToShortDateString() == dtSelectDate.ToShortDateString())
                        {
                            dr = dt.NewRow();
                            dr[0] = dtSelectDate.ToShortDateString();
                            dt.Rows.Add(dr);
                            dtSelectDate = dtSelectDate.AddDays(1);
                            _bDate = true;
                        }
                    }
                    else
                    {
                        arrSelected = dtSelectDate.ToShortDateString().Split('/');
                        if (arrHoliday[0] == arrSelected[0] && arrHoliday[1] == arrSelected[1])
                        {
                            dr = dt.NewRow();
                            dr[0] = dtSelectDate.ToShortDateString();
                            dt.Rows.Add(dr);
                            dtSelectDate = dtSelectDate.AddDays(1);
                            _bDate = true;
                        }
                    }
                }
            }

            if (CutOff == true) //Will only check for the cutoff time (once only no looping)
            {
                int iCutOff = 0;
                bool isHoliday = false;
                DateTime dtCutOff;
                getUpdateDS("isp_getBranchSetup", new Object[] { BranchCode });
                if (hasRows)
                {
                    selectedRow = 0;
                    for (i = 0; i <= (table.Rows.Count - 1); i++)
                    {
                        /* CUTOFF SETTINGS
                        A = if time1 is less than time2       then result is Less than ZERO   < 0
                        B = if time1 equals time2             then result is ZERO             = 0
                        C = if time1 is greater than time2    then result is Greater ZERO     > 0
                        */
                        dtCutOff = new DateTime(dtSelectDate.Year, dtSelectDate.Month, dtSelectDate.Day, Convert.ToDateTime(table.Rows[i]["CutOffEnd"].ToString()).Hour, Convert.ToDateTime(table.Rows[i]["CutOffEnd"].ToString()).Minute, Convert.ToDateTime(table.Rows[i]["CutOffEnd"].ToString()).Second);
                        iCutOff = dtSelectDate.CompareTo(dtCutOff);
                        if (iCutOff >= 1)
                        {
                            for (i = 0; i <= (dt.Rows.Count - 1); i++)
                            {
                                if (Convert.ToDateTime(dt.Rows[i][0].ToString()).ToShortDateString() == dtLocalTime.ToShortDateString())
                                {
                                    isHoliday = true;
                                    _bDate = true;
                                    break;
                                }
                            }
                            if (isHoliday == false) //If isHoliday == true then no need to add days else add one day.
                            {
                                dtSelectDate = dtSelectDate.AddDays(1);
                            }
                        }
                    }
                }
            }
            IsNewDate = _bDate;
            return dtSelectDate;
        }

        public DataTable getUserCanOverride()
        { 
            base.EnforceConstraint = false;
            getUpdateDS("isp_getUserCanOverride", new Object[] { });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return null;
            }
        }

        //function added for non-web apps that uses the 'getTransactionDate' function
        public DateTime getTransactionDate_nWs(string BranchCode, bool CutOff)
        {
            sqlHelper hlp = new sqlHelper();
            DateTime dtLocalTime = new irsTimeZone().branchLocalTime_nWs(BranchCode);
            DateTime dtSelectDate = dtLocalTime;
            int i = 0;
            string[] arrHoliday;
            string[] arrSelected;
            DataRow dr;
            DataTable dt = new DataTable();
            dt.Columns.Add("Holiday");
            _bDate = false;

            hlp.EnforceConstraint = false;
            hlp.getUpdateDS("isp_getHolidays", new Object[] { BranchCode });
            if (hlp.hasRows)
            {
                hlp.selectedRow = 0;
                for (i = 0; i <= (hlp.table.Rows.Count - 1); i++)
                {
                    arrHoliday = hlp.table.Rows[i]["Hol_Date"].ToString().Split('/');
                    if (hlp.IsNumeric(arrHoliday[2]))
                    {
                        if (Convert.ToDateTime(hlp.table.Rows[i]["Hol_Date"].ToString().Trim()).ToShortDateString() == dtSelectDate.ToShortDateString())
                        {
                            dr = dt.NewRow();
                            dr[0] = dtSelectDate.ToShortDateString();
                            dt.Rows.Add(dr);
                            dtSelectDate = dtSelectDate.AddDays(1);
                            _bDate = true;
                        }
                    }
                    else
                    {
                        arrSelected = dtSelectDate.ToShortDateString().Split('/');
                        if (arrHoliday[0] == arrSelected[0] && arrHoliday[1] == arrSelected[1])
                        {
                            dr = dt.NewRow();
                            dr[0] = dtSelectDate.ToShortDateString();
                            dt.Rows.Add(dr);
                            dtSelectDate = dtSelectDate.AddDays(1);
                            _bDate = true;
                        }
                    }
                }
            }

            if (CutOff == true) //Will only check for the cutoff time (once only no looping)
            {
                int iCutOff = 0;
                bool isHoliday = false;
                DateTime dtCutOff;
                hlp.getUpdateDS("isp_getBranchSetup", new Object[] { BranchCode });
                if (hlp.hasRows)
                {
                    hlp.selectedRow = 0;
                    for (i = 0; i <= (hlp.table.Rows.Count - 1); i++)
                    {
                        /* CUTOFF SETTINGS
                        A = if time1 is less than time2       then result is Less than ZERO   < 0
                        B = if time1 equals time2             then result is ZERO             = 0
                        C = if time1 is greater than time2    then result is Greater ZERO     > 0
                        */
                        dtCutOff = new DateTime(dtSelectDate.Year, dtSelectDate.Month, dtSelectDate.Day, Convert.ToDateTime(hlp.table.Rows[i]["CutOffEnd"].ToString()).Hour, Convert.ToDateTime(hlp.table.Rows[i]["CutOffEnd"].ToString()).Minute, Convert.ToDateTime(hlp.table.Rows[i]["CutOffEnd"].ToString()).Second);
                        iCutOff = dtSelectDate.CompareTo(dtCutOff);
                        if (iCutOff >= 1)
                        {
                            for (i = 0; i <= (dt.Rows.Count - 1); i++)
                            {
                                if (Convert.ToDateTime(dt.Rows[i][0].ToString()).ToShortDateString() == dtLocalTime.ToShortDateString())
                                {
                                    isHoliday = true;
                                    _bDate = true;
                                    break;
                                }
                            }
                            if (isHoliday == false) //If isHoliday == true then no need to add days else add one day.
                            {
                                dtSelectDate = dtSelectDate.AddDays(1);
                            }
                        }
                    }
                }
            }
            IsNewDate = _bDate;
            return dtSelectDate;
        }

        public string getVIPPercent(string BranchCode, string VIPType)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getVIPPercent", new Object[] { BranchCode, VIPType });
            if (hasRows)
            {
                selectedRow = 0;
                return table.Rows[0]["Value"].ToString().Trim();
            }
            else
            {
                return null;
            }
        }

        public string updateDisclaimer(string RemitterCIFID, bool IsRead)
        {
            string[] arrDisclaimer = new string[2];
            int iReturn = 0;

            try
            {
                arrDisclaimer.SetValue(RemitterCIFID, 0);
                arrDisclaimer.SetValue(IsRead.ToString(), 1);
                iReturn = execStoredProcedureUpdate("isp_updateDisclaimer", arrDisclaimer);
                return iReturn.ToString() + "|OK";
            }
            catch (Exception ex)
            {
                return "-1|" + ex.Message;
            }
        }

        public DataTable getChargeType(string BranchCode, string ProductCode)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getChargeType", new Object[] { BranchCode, ProductCode });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return null;
            }
        }

        public bool isPNBReceipt(string BranchCode)
        {
            DataSet isReceiptEnable = null;
            try
            {
                //isReceiptEnable = getDataSet(getSqlCommand("select * from tbl_reports a inner join tbl_reports_per_branch b on a.seq_id = b.rep_id where a.subgroupid = 'REPSUB3' and b.branchcode = '" + BranchCode + "'", false));
                isReceiptEnable = getDataSet("select * from tbl_reports a inner join tbl_reports_per_branch b on a.seq_id = b.rep_id where a.subgroupid = 'REPSUB3' and b.branchcode = '" + BranchCode + "'", false, new object[] { });//Performance consideration when using irsSqlCommonsWS and sqlHelperWS
                if (isReceiptEnable.Tables[0].Rows.Count >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                csAuditLog al = new csAuditLog();
                al.IsWinApp = base.IsWinApp;

                al.writeAudit(((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId, "InternetPage-ARS", ex.Message, ex.StackTrace, ex.Source, csAuditLog.Category.Select);
                return false;
            }
        }

        private bool sendMail(MailMessage mail)
        {
            try
            {
                SmtpClient client = new SmtpClient();
                if (use_credential)
                {
                    client.Credentials = new System.Net.NetworkCredential(user, password);
                }
                client.Host = host;
                if (port > -1) client.Port = port;
                if (timeout > -1) client.Timeout = timeout;
                client.EnableSsl = enable_ssl;
                client.Send(mail);
                mail.Dispose();
                client = null;
                mail = null;
                return true;
            }
            catch (Exception ex)
            {
                mail.Dispose();
                mail = null;
                _message = "csMailer: " + ex.Message + " " + ex.InnerException.Message;
                if (ex.InnerException.InnerException != null)
                {
                    _message += " " + ex.InnerException.InnerException.Message;
                }
            }

            return false;
        }

        private void parse_connection()
        {
            port = -1;
            use_credential = false;
            enable_ssl = false;
            timeout = -1;

            string host_value = "";

            //ITG password encrypt
            //String constring = new irsConfiguration().appsetting("SMTPConnection");
            String constring = new irsConfiguration().ConnectionStringNoRunMode("SMTPConnection");
            if (constring != null && constring.Length > 0)
            {
                String[] ss = constring.Split(';');
                foreach (String s in ss)
                {
                    String[] tt = s.Split('=');
                    if (tt[0].ToUpper().Equals("SERVER")) try { host = tt[1]; host_value = tt[1]; }
                        catch { }
                    if (tt[0].ToUpper().Equals("PORT")) try { port = int.Parse(tt[1]); }
                        catch { }
                    if (tt[0].ToUpper().Equals("USER")) try { user = tt[1]; }
                        catch { }
                    if (tt[0].ToUpper().Equals("PASSWORD")) try { password = tt[1]; }
                        catch { }
                    if (tt[0].ToUpper().Equals("USE_CREDENTIAL")) try { use_credential = bool.Parse(tt[1]); }
                        catch { }
                    if (tt[0].ToUpper().Equals("USE_SSL")) try { enable_ssl = bool.Parse(tt[1]); }
                        catch { }
                    if (tt[0].ToUpper().Equals("TIMEOUT")) try { timeout = int.Parse(tt[1]); }
                        catch { }
                }
            }

            //if (host == null || host == String.Empty) host = "mail.pnb.com.ph";
            if (host == null || host == String.Empty) host = host_value;

            try
            {
                from = new MailAddress(new irsConfiguration().appsetting("MailFrom"));
            }
            catch
            {
                from = new MailAddress("irsworld@pnb.com.ph");
            }
        }

        public bool sendMail(String subject, String[] recipient, String[] cc, String msg_text, String[] attachs)
        {
            try
            {
                parse_connection();

                MailMessage mail = new MailMessage();

                foreach (String to in recipient)
                {
                    if (to != "")
                    {
                        mail.To.Add(to);
                    }
                }

                if (cc != null && cc.Length > 0)
                {
                    foreach (String c in cc)
                    {
                        mail.CC.Add(c);
                    }
                }
                mail.From = from;
                mail.Subject = subject;
                mail.SubjectEncoding = System.Text.Encoding.UTF8;
                mail.Body = msg_text;
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;

                if (attachs != null && attachs.Length > 0)
                {
                    //setAttachments(mail, attachs);
                }

                return sendMail(mail);
            }
            catch (Exception ex)
            {
                _message = "csMail: " + ex.Message;
            }
            return false;
        }

        public bool InternetReceiptEmail(string remitterCIF, string message, string refno, out string errmsg)
        {
            irsReceipts ir = new irsReceipts();
            ir.IsWinApp = base.IsWinApp;

            DataTable dt = null;
            DataTable dtORLookup = null;

            string[] EmailAdd = null;
            bool issend = false;
            string strSubject = "";

            try
            {
                dt = ir.getCIFContacts(remitterCIF);
                if (dt.Rows.Count > 0)
                {
                    EmailAdd = new string[dt.Rows.Count];
                    for (int i2 = 0; i2 <= dt.Rows.Count - 1; i2++)
                    {
                        EmailAdd[i2] = dt.Rows[i2][5].ToString();
                    }
                }
                else
                {
                    errmsg = "Remitter has no Email Address.";
                    return false;
                }
                dtORLookup = ir.ORLookup();
                if (dtORLookup == null || dtORLookup.Rows.Count == 0)
                {
                    errmsg = "Failed to get O.R. Email Details.";
                    return false;
                }
                strSubject = "PNB Remittance Receipt " + refno;

                issend = this.sendMail(strSubject, EmailAdd, null, message, null);
                if (issend == true)
                {
                    errmsg = "Receipt Email sent";
                }
                else
                {
                    errmsg = "Failed to send Receipt thru Email.";
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;                
            }
            finally
            {
                dt.Dispose();
                dt = null;
                dtORLookup.Dispose();
                dtORLookup = null;
            }
            return issend;
        }

        public bool aggregateInternet(string amount, string remitterCIFID, string remittingcurrency, string BranchCode, string tranxdate)
        {
            string remittancelimit = "";
            try
            {
                remittancelimit = this.getRemittanceLimit(BranchCode);
                base.EnforceConstraint = false;
                getUpdateDS("isp_internet_rem_aggregate", new Object[] { BranchCode, Convert.ToDateTime(tranxdate).ToShortDateString(), remittingcurrency, remitterCIFID });
                if (hasRows)
                {
                    selectedRow = 0;
                    if (table.Rows[0]["GrossRemAmount"].ToString() == "0")
                    {
                        return true;
                    }
                    else
                    {
                        if (isRemittanceAggregate == "True")
                        {
                            if (Convert.ToDecimal(remittancelimit)  >= (Convert.ToDecimal(table.Rows[0]["GrossRemAmount"].ToString()) + Convert.ToDecimal(amount)))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                csAuditLog al = new csAuditLog();
                al.IsWinApp = base.IsWinApp;
                al.writeAudit(((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId, "InternetPage-ARS-aggregateInternet", ex.Message, ex.StackTrace, ex.Source, csAuditLog.Category.Select);
                return false;
            }
            return false;
        }

        public DataTable CashAggregation(string amount, string remitterCIFID, string remittingcurrency, string BranchCode, string tranxdate)
        {
            try
            {
                base.EnforceConstraint = false;
                getUpdateDS("usp_RequiredIDLimit_aggregate2", new Object[] { BranchCode, remitterCIFID, remittingcurrency, Convert.ToDateTime(tranxdate).ToShortDateString(), 1, 1 });
                if (hasRows)
                {
                    return table;
                }
            }
            catch (Exception ex)
            {
                csAuditLog al = new csAuditLog();
                al.IsWinApp = base.IsWinApp;
                al.writeAudit(((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId, "InternetPage-ARS-CashAggregation", ex.Message, ex.StackTrace, ex.Source, csAuditLog.Category.Select);
            }
            return null;
        }

        public DataTable getBranchSetup(string BranchCode)
        {
            try
            {
                base.EnforceConstraint = false;
                getUpdateDS("isp_getBranchSetup", new Object[] { BranchCode });
                if (hasRows)
                {
                    return table;
                }
            }
            catch (Exception ex)
            {
                csAuditLog al = new csAuditLog();
                al.IsWinApp = base.IsWinApp;
                al.writeAudit(((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId, "InternetPage-ARS-getBranchSetup", ex.Message, ex.StackTrace, ex.Source, csAuditLog.Category.Select);
            }
            return null;
        }

        public string getCountry(string branchCode)
        {
            DataSet branchCountry = null;
            try
            {
                branchCountry = getDataSet("select * from TBL_branches where branchcode = '" + branchCode + "'", false, new object[] { });
                if (branchCountry.Tables[0].Rows.Count >= 1)
                {
                    return branchCountry.Tables[0].Rows[0]["Country"].ToString();
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                csAuditLog al = new csAuditLog();
                al.IsWinApp = base.IsWinApp;
                al.writeAudit(((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId, "InternetPage-ARS", ex.Message, ex.StackTrace, ex.Source, csAuditLog.Category.Select);
                return "";
            }
            finally
            {                
                branchCountry = null;
            }
        }

        public DataTable getSystemControl_RemitterInfo(string remitterID, string beneficiaryID)
        {
            try
            {
                base.EnforceConstraint = false;
                getUpdateDS("isp_agentsys_remitter_info", new Object[] { remitterID, beneficiaryID });
                if (hasRows)
                {
                    return table;
                }
            }
            catch (Exception ex)
            {
                csAuditLog al = new csAuditLog();
                al.IsWinApp = base.IsWinApp;
                al.writeAudit(((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId, "InternetPage-ARS-getSystemControl_RemitterInfo", ex.Message, ex.StackTrace, ex.Source, csAuditLog.Category.Select);
            }
            return null;
        }

        public DataTable getSystemControl_RemitterIDs(string remitterID, string seq_id)
        {
            try
            {
                base.EnforceConstraint = false;
                getUpdateDS("isp_agentsys_remitter_ids", new Object[] { remitterID, seq_id });
                if (hasRows)
                {
                    return table;
                }
            }
            catch (Exception ex)
            {
                csAuditLog al = new csAuditLog();
                al.IsWinApp = base.IsWinApp;
                al.writeAudit(((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId, "InternetPage-ARS-getSystemControl_RemitterIDs", ex.Message, ex.StackTrace, ex.Source, csAuditLog.Category.Select);
            }
            return null;
        }
    }
}