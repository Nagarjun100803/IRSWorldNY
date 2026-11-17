using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace IRSClassLib
{
    [Serializable]
    public class irsARSW : sqlHelperWs
    {
        public irsARSW()
        {
            //base.IsWinApp = true;
        }

        private void setBase(string SQL, bool IsAdd)
        {
            base.getUpdateDataSet(getSqlCommand(SQL, false));
            if (!base.hasRows)
            {
                base.newRow(sqlHelperWs.RETAIN_DATASET);
            }
            else
            {
                if (IsAdd)
                {
                    base.newRow(sqlHelperWs.RETAIN_DATASET);
                }
                else
                {
                    selectedRow = 0;
                }
            }
        }

        public string getCharges(string Amount, string ChargeID, string Currency, string Computation, bool ReCompute)
        {
            irsCalculator irsCalc = null;
            try
            {
                string sReturn = string.Empty;
                irsCalc = new irsCalculator(Convert.ToInt32(ChargeID), Convert.ToInt32(Currency), Convert.ToInt32(Computation), false, 0);

                if (ReCompute)
                {
                    if (System.Web.HttpContext.Current.Session["ARSWCALCULATOR"] != null)
                    {
                        irsCalc = (irsCalculator)System.Web.HttpContext.Current.Session["ARSWCALCULATOR"];
                        irsCalc.recompute_irsw_charges(Convert.ToDecimal(Amount));
                    }
                    else
                    {
                        irsCalc.compute_irsw_charges(Convert.ToDecimal(Amount));
                    }
                }
                else
                {
                    irsCalc.compute_irsw_charges(Convert.ToDecimal(Amount));
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
                System.Web.HttpContext.Current.Session["ARSWCALCULATOR"] = irsCalc;
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

        public DataTable getRemittingCurrency(string BranchCode)
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

        public DataTable getCIFInformation(string RemitterID, string BeneficiaryID)
        {
            base.EnforceConstraint = false;
            getUpdateDS("asp_getCIFInformation", new Object[] { RemitterID, BeneficiaryID });
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

        public DataTable getForex(string BranchCode, string RemittingCurrency, string TargetCurrency)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getExchangeRate", new Object[] { BranchCode, RemittingCurrency, TargetCurrency, true });
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

        public DataTable getRemittanceAggregate(string BranchCode, string TranxDate, string RemittingCurrency, string RemitterID)
        {
            base.EnforceConstraint = false;
            getUpdateDS("sp_rem_otc_remitter_aggregate2", new Object[] { BranchCode, TranxDate, RemittingCurrency, RemitterID });
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

        public DataTable getChargeMaster(string SEQID)
        {
            base.EnforceConstraint = false;
            getUpdateDS("asp_getChargeMaster", new Object[] { SEQID });
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

        public DataTable getRemittanceByGUID(string GUID)
        {
            base.EnforceConstraint = false;
            getUpdateDS("asp_getRemittanceByGUID", new Object[] { GUID });
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

        public DataTable getPreReceipt(string ReferenceNo)
        {
            base.EnforceConstraint = false;
            getUpdateDS("asp_getPreReceipt", new Object[] { ReferenceNo });
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

        public DataTable getCIFAccount(string SEQID)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getCIFAccount", new Object[] { SEQID });
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

        public DataTable getBranchSetup(string BranchCode)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getBranchSetup", new Object[] { BranchCode });
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

        public string remit(string UserID, string RemitMethod, string RemittingBranch, string TranxDate, 
            string ProductChargesID, string RemitterID, string BeneficiaryID, string AccountSEQID, 
            string TranxPurpose, string DatePostedOverseas, string DatePostedLocal, 
            string RemitterAddressSEQID, string RemitterContactSEQID, 
            string BeneficiaryAddressSEQID, string BeneficiaryContactSEQID,
            string ClearingDate, string CheckNumber)
        {
            bool ok = false;
            string message = string.Empty;
            irsCalculator irsCalc = null;
            DataTable dtChargeMaster = this.getChargeMaster(ProductChargesID);
            DataTable dtDefaults = new irsInternetPage().getDefaults(RemittingBranch);
            string guid = string.Empty;
            string seqid = string.Empty;
            string referenceNo = string.Empty;
            string tranxstatus = string.Empty;
            DataSet dsRemit;
            int i = 0;
            string remitMessage = string.Empty;
            bool ishold = false;
            DataTable dtAccount;

            if (dtChargeMaster == null || dtChargeMaster.Rows.Count < 0)
            {
                ok = false;
                message = "ERROR1: Unable to retrieve charges from database.";
                return ok.ToString() + "|" + message;
            }
            if (dtDefaults == null || dtDefaults.Rows.Count < 0)
            {
                ok = false;
                message = "ERROR2: Unable to retrieve branch defaults.";
                return ok.ToString() + "|" + message;
            }

            irsCalc = (irsCalculator)System.Web.HttpContext.Current.Session["ARSWCALCULATOR"];
            if (System.Web.HttpContext.Current.Session["ARSWCALCULATOR"] != null)
            {
                base.addMode = true;
                guid = Guid.NewGuid().ToString();
                tranxstatus = dtChargeMaster.Rows[0]["DefaultStatus"].ToString();
                if (dtChargeMaster.Rows[0]["PayMode"].ToString() == "2")
                {
                    ishold = true;
                    tranxstatus = "14";
                }
                this.setBase("SELECT TOP 1 * FROM tbl_Remittance", true);
                setColumn("UserID", UserID);
                setColumn("RemitMethod", RemitMethod);
                setColumn("RemittingBranch", RemittingBranch);
                setColumn("ChargeLocally", "0");
                setColumn("ExtlReferenceNo", new irsPaymentGateway().generateReference(RemittingBranch));
                setColumn("Transtatus", tranxstatus);
                setColumn("Tranxdate", TranxDate);
                setColumn("ProductChargesID", ProductChargesID);
                setColumn("RemitterCIFID", RemitterID);
                setColumn("BeneficiaryCIFID", BeneficiaryID);
                setColumn("AccountSEQID", AccountSEQID);
                setColumn("RemittingCurrency", dtChargeMaster.Rows[0]["Currency"].ToString());
                setColumn("LocalCurrency", dtDefaults.Rows[0]["LocalCurrency"].ToString());
                setColumn("GrossRemAmount", irsCalc.get_remittance_gross_branch().ToString("#,###.00"));
                setColumn("NetRemAmount", irsCalc.get_remittance_net_remit_amt().ToString("#,###.0000"));
                setColumn("NetToBeneCurrency", irsCalc.get_forex_remitting_to_account_code().ToString());
                setColumn("NetToBeneAmount", irsCalc.get_remittance_net_to_bene_amt().ToString("#,###.00"));
                setColumn("NetToBeneAmountPHP", irsCalc.get_remittance_net_remit_php_equivalent().ToString("#,###.00"));
                setColumn("TotalLocalCharge", irsCalc.get_remittance_total_charges_amt().ToString("#,###.00"));
                setColumn("TransactionSource", dtDefaults.Rows[0]["TranxSource"].ToString());
                setColumn("ReceiptMode", dtDefaults.Rows[0]["ReceiptMode"].ToString());
                setColumn("TransactionPurpose", TranxPurpose);
                setColumn("SourceOfFunds", dtDefaults.Rows[0]["SourceOfFunds"].ToString());
                if (dtChargeMaster.Rows[0]["ImplementationType"].ToString() != "2")
                {
                    setColumn("Printed", true);
                }
                else
                {
                    setColumn("Printed", false);
                }
                setColumn("ForexRemittingToAccount", irsCalc.get_forex_target_to_account().ToString("#,###.00"));
                setColumn("ForexRemittingToLocal", irsCalc.get_forex_target_to_source().ToString("#,###.00"));
                setColumn("ForexRemittingToUSD", irsCalc.get_forex_target_to_usd().ToString());
                setColumn("ForexRemittingToPHP", irsCalc.get_forex_target_to_php().ToString("#,###.00"));
                setColumn("ForexRemittingToEUR", irsCalc.get_forex_target_to_eur().ToString("#,###.00"));
                setColumn("ForexLocalToRemitting", irsCalc.get_forex_source_to_target().ToString("#,###.00"));
                setColumn("ForexLocalToUSD", irsCalc.get_forex_source_to_usd().ToString("#,###.00"));
                setColumn("ForexLocalToPHP", irsCalc.get_forex_source_to_php().ToString("#,###.00"));
                setColumn("DatePostedOverseas", DatePostedOverseas);
                setColumn("DatePostedLocal", DatePostedLocal);
                //setColumn("ForexUSDtoPHP", "");
                setColumn("Total_Backend_Charges", "0.00");
                setColumn("RemitterAddressSEQID", RemitterAddressSEQID);
                setColumn("RemitterContactSEQID", RemitterContactSEQID);
                setColumn("BeneficiaryAddressSEQID", BeneficiaryAddressSEQID);
                setColumn("BeneficiaryContactSEQID", BeneficiaryContactSEQID);
                setColumn("GUID", guid);
                ok = base.save(sqlHelperWs.RETAIN_DATASET);
                if (ok)
                {
                    DataTable dtRemittance = this.getRemittanceByGUID(guid);
                    if (dtRemittance != null && dtRemittance.Rows.Count > 0)
                    {
                        seqid = dtRemittance.Rows[0]["SEQ_ID"].ToString();
                        referenceNo = dtRemittance.Rows[0]["ReferenceNo"].ToString();
                        irsCalc.commit_remittance_charge_details(Convert.ToInt32(seqid));
                    }
                    if (tranxstatus == "1")
                    {
                        dsRemit = new irsRemTeller().RemitToAgent(Convert.ToInt32(seqid));
                        if (dsRemit != null && dsRemit.Tables[0].Rows.Count > 0)
                        {
                            for (i = 0; i <= (dsRemit.Tables[0].Columns.Count - 1); i++)
                            {
                                remitMessage += dsRemit.Tables[0].Rows[0][i].ToString() + "|";
                            }
                            remitMessage = remitMessage.Substring(0, remitMessage.Length - 1);
                        }

                        dtAccount = this.getCIFAccount(AccountSEQID);
                        if (dtAccount != null && dtAccount.Rows.Count > 0)
                        {
                            //new irsLinkToGFC().insert_GFC_Aggregate(referenceNo, dtAccount.Rows[0]["AccountNo"].ToString(), Convert.ToInt32(irsCalc.get_forex_remitting_to_account_code().ToString()), Convert.ToDouble(irsCalc.get_remittance_net_to_bene_amt().ToString()));
                        }
                    }
                    if (ishold)
                    {
                        this.setBase("SELECT TOP 1 * FROM tbl_Remittance_Check_Details", true);
                        setColumn("REM_SEQ_ID", seqid);
                        setColumn("Clearing_Date", ClearingDate);
                        setColumn("Check_No", CheckNumber);
                        ok = base.save(sqlHelperWs.RETAIN_DATASET);
                    }
                    message = "Successful|" + referenceNo + "|" + seqid + "|" + remitMessage;
                }
                else
                {
                    message = base.Message;
                }
            }
            else
            {
                ok = false;
                message = "ERROR3: Unable to retrieve computed charges.";
            }
            return ok.ToString() + "|" + message;
        }
    }
}