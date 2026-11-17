using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace IRSClassLib
{
    [Serializable]
    public class csRemittanceWS : sqlHelperWs
    {
        public csRemittanceWS()
        {
            //base.IsWinApp = true;
        }

        #region Process Simulation
        public bool simulateIRSW()
        {
            try
            {
                DataTable dt = getTransactionBySEQID();
                int i = 0;
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (i = 0; i <= (dt.Rows.Count - 1); i++)
                    {
                        if (this.checkRemittance(Convert.ToInt64(dt.Rows[i]["SEQ_ID"].ToString().Trim())) == false)
                        {
                            this.saveRemittanceBySEQID(Convert.ToInt64(dt.Rows[i]["SEQ_ID"].ToString().Trim()));
                        }
                    }
                }
                this.simulateIRSWUpdate();
                //this.simulateIRSWDelete();
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit(_userID, "SAVE REMITTANCE ERROR", "ERROR SAVE REMITTANCE: [ErrorMessage=" + ex.Message + "][StackTrace=" + ex.StackTrace + "]", "simulateIRSW", string.Empty, csAuditLog.Category.Select);
            }
            return true;
        }

        public bool simulateIRSWUpdate()
        {
            try
            {
                DataTable dt = getTransactionForUpdateBySEQID();
                int i = 0;
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (i = 0; i <= (dt.Rows.Count - 1); i++)
                    {
                        this.updateRemittanceBySEQID(Convert.ToInt64(dt.Rows[i]["SEQ_ID"].ToString().Trim()));
                    }
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit(_userID, "UPDATE REMITTANCE ERROR", "ERROR UPDATE REMITTANCE: [ErrorMessage=" + ex.Message + "]", "simulateIRSWUpdate", string.Empty, csAuditLog.Category.Select);
            }
            return true;
        }

        public bool simulateIRSWDelete()
        {
            try
            {
                DataTable dt = getTransactionForDeletionBySEQID();
                int i = 0;
                string sSEQID = string.Empty;
                string sReferenceNo = string.Empty;

                if (dt != null && dt.Rows.Count > 0)
                {
                    sSEQID = dt.Rows[0]["SEQ_ID"].ToString();
                    sReferenceNo = dt.Rows[0]["ReferenceNo"].ToString();
                    for (i = 1; i <= (dt.Rows.Count - 1); i++)
                    {
                        if (sReferenceNo != dt.Rows[i]["ReferenceNo"].ToString().Trim())
                        {
                            sSEQID = dt.Rows[i]["SEQ_ID"].ToString();
                            sReferenceNo = dt.Rows[i]["ReferenceNo"].ToString();
                        }
                        else
                        {
                            this.deleteRemittanceBySEQID(Convert.ToInt64(sSEQID), sReferenceNo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit(_userID, "DELETE REMITTANCE ERROR", "ERROR DELETE REMITTANCE: [ErrorMessage=" + ex.Message + "]", "simulateIRSWDelete", string.Empty, csAuditLog.Category.Select);
            }
            return true;
        }

        public bool fixRemittanceCharges()
        {
            try
            {
                DataTable dt = getRemittanceWithNoCharges();
                int i = 0;
                string sSEQID = string.Empty;

                if (dt != null && dt.Rows.Count > 0)
                {
                    sSEQID = dt.Rows[0]["SEQ_ID"].ToString();
                    for (i = 0; i <= (dt.Rows.Count - 1); i++)
                    {
                        this.fixRemittanceChargesBySEQID(Convert.ToInt64(sSEQID));
                    }
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit(_userID, "FIX REMITTANCE CHARGES ERROR", "ERROR FIXING REMITTANCE CHARGES: [ErrorMessage=" + ex.Message + "]", "fixRemittanceCharges", string.Empty, csAuditLog.Category.Select);
            }
            return true;
        }
        #endregion

        #region Enum
        public enum CIFClass
        {
            Remitter = 1,
            Beneficiary = 2,
            MerchantBiller = 3,
            Agent = 4
        }

        public enum RemitMethod
        {
            Batch = 1,
            OBPS = 2,
            ARM = 3,
            OTC = 4,
            Web = 5,
            API = 6,
            R8 = 7,
            ARS = 8,
            WebOBP = 9,
            ARSOBP = 10
        }

        public enum ChargeName
        {
            Commission = 1,
            CableCharges = 2,
            BackEndCharges = 3,
            ServiceFee = 4,
            OPE = 5,
            DocumentaryStamps = 6,
            InterchangeFee = 7
        }
        #endregion

        #region Variables
        private string _userID;
        private string _location;
        #endregion

        #region Properties
        public string userID
        {
            get
            {
                return _userID;
            }
            set
            {
                _userID = value;
            }
        }

        public string location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
            }
        }
        #endregion

        #region Functions and Methods
        private DataTable createTable(string tableName)
        {
            DataTable dt = new DataTable();
            switch (tableName)
            {
                case "SAVEBYSEQID":
                    //dt.Columns.Add("SEQ_ID");
                    dt.Columns.Add("RemitMethod");
                    dt.Columns.Add("vRemitMethod");
                    dt.Columns.Add("MakerID");
                    dt.Columns.Add("MakerIDName");
                    dt.Columns.Add("CheckerID");
                    dt.Columns.Add("CheckerIDName");
                    dt.Columns.Add("ReferenceNo");
                    dt.Columns.Add("ExtlReferenceNo");
                    dt.Columns.Add("TranxStatus");
                    dt.Columns.Add("vTranxStatus");
                    dt.Columns.Add("TranxDate");
                    dt.Columns.Add("ClearingDate");
                    dt.Columns.Add("CheckNo");
                    dt.Columns.Add("APIMessage");
                    dt.Columns.Add("RemittingBranch");
                    dt.Columns.Add("RemittingBranchName");
                    dt.Columns.Add("NegBranchCode");
                    dt.Columns.Add("CoveringBank");
                    dt.Columns.Add("RemitterID");
                    dt.Columns.Add("RemType");
                    dt.Columns.Add("RemLastName");
                    dt.Columns.Add("RemFirstName");
                    dt.Columns.Add("RemMiddleName");
                    dt.Columns.Add("RemBirthDate");
                    dt.Columns.Add("RemHomePhone");
                    dt.Columns.Add("RemMobilePhone");
                    dt.Columns.Add("RemPOBox");
                    dt.Columns.Add("RemAddress");
                    dt.Columns.Add("RemZipCode");
                    dt.Columns.Add("RemCountry");
                    dt.Columns.Add("RemEmailAddress");
                    dt.Columns.Add("RemVIPType");
                    dt.Columns.Add("RemID1");
                    dt.Columns.Add("RemID2");
                    dt.Columns.Add("RemID3");
                    dt.Columns.Add("RemID4");
                    dt.Columns.Add("BeneficiaryID");
                    dt.Columns.Add("BenType");
                    dt.Columns.Add("BenLastName");
                    dt.Columns.Add("BenFirstName");
                    dt.Columns.Add("BenMiddleName");
                    dt.Columns.Add("BenBirthDate");
                    dt.Columns.Add("BenHomePhone");
                    dt.Columns.Add("BenMobilePhone");
                    dt.Columns.Add("BenPOBox");
                    dt.Columns.Add("BenAddress");
                    dt.Columns.Add("BenZipCode");
                    dt.Columns.Add("BenCountry");
                    dt.Columns.Add("BenEmailAddress");
                    dt.Columns.Add("Relationship");
                    dt.Columns.Add("vRelationship");
                    dt.Columns.Add("NotificationMode");
                    dt.Columns.Add("vNotificationMode");
                    dt.Columns.Add("SelfAllotee");
                    dt.Columns.Add("NSVS");
                    dt.Columns.Add("CardNo");
                    dt.Columns.Add("CardName");
                    dt.Columns.Add("CardExpiryDate");
                    dt.Columns.Add("CardType");
                    dt.Columns.Add("CardClass");
                    dt.Columns.Add("CardCurrency");
                    dt.Columns.Add("AccountNo");
                    dt.Columns.Add("AccountType");
                    dt.Columns.Add("vAccountType");
                    dt.Columns.Add("AccountCurrency");
                    dt.Columns.Add("AccountBranchCode");
                    dt.Columns.Add("OtherBankAccountNo");
                    dt.Columns.Add("OtherBankName");
                    dt.Columns.Add("OtherBankCurrency");
                    dt.Columns.Add("OtherBankBranchCode");
                    dt.Columns.Add("PDDTSBankCode");
                    dt.Columns.Add("GFCCardNo");
                    dt.Columns.Add("KYCName");
                    dt.Columns.Add("KYCAddress");
                    dt.Columns.Add("OSA");
                    dt.Columns.Add("ProductCode");
                    dt.Columns.Add("vProductCode");
                    dt.Columns.Add("ImplementationType");
                    dt.Columns.Add("vImplementationType");
                    dt.Columns.Add("LocalCurrency");
                    dt.Columns.Add("vLocalCurrency");
                    dt.Columns.Add("ChargeType");
                    dt.Columns.Add("vChargeType");
                    dt.Columns.Add("PayMode");
                    dt.Columns.Add("vPayMode");
                    dt.Columns.Add("ComputationLevel");
                    dt.Columns.Add("vComputationLevel");
                    dt.Columns.Add("TranxSource");
                    dt.Columns.Add("vTranxSource");
                    dt.Columns.Add("TranxPurpose");
                    dt.Columns.Add("vTranxPurpose");
                    dt.Columns.Add("OtherPurpose");
                    dt.Columns.Add("ReceiptMode");
                    dt.Columns.Add("vReceiptMode");
                    dt.Columns.Add("SourceOfFunds");
                    dt.Columns.Add("vSourceOfFunds");
                    dt.Columns.Add("SpecialMessage");
                    dt.Columns.Add("RemittingCurrency");
                    dt.Columns.Add("vRemittingCurrency");
                    dt.Columns.Add("GrossAmount");
                    dt.Columns.Add("NetAmount");
                    dt.Columns.Add("NetToBenCurrency");
                    dt.Columns.Add("vNetToBenCurrency");
                    dt.Columns.Add("NetToBenAmount");
                    dt.Columns.Add("NetToBenAmountPHP");
                    dt.Columns.Add("TotalCharges");
                    dt.Columns.Add("TotalBackend");
                    dt.Columns.Add("BackendCurrency");
                    dt.Columns.Add("TotalCommision");
                    dt.Columns.Add("CommissionCurrency");
                    dt.Columns.Add("TotalCable");
                    dt.Columns.Add("CableCurrency");
                    dt.Columns.Add("TotalServiceFee");
                    dt.Columns.Add("ServiceCurrency");
                    dt.Columns.Add("TotalOPE");
                    dt.Columns.Add("OPECurrency");
                    dt.Columns.Add("TotalDocStamp");
                    dt.Columns.Add("DocstampCurrency");
                    dt.Columns.Add("TotalInterchangeFee");
                    dt.Columns.Add("InterchangefeeCurrency");
                    dt.Columns.Add("ChargeLocally");
                    dt.Columns.Add("Denominations");
                    dt.Columns.Add("ForexRemittingToAccount");
                    dt.Columns.Add("ForexRemittingToLocal");
                    dt.Columns.Add("ForexRemittingToUSD");
                    dt.Columns.Add("ForexRemittingToPHP");
                    dt.Columns.Add("ForexRemittingToEUR");
                    dt.Columns.Add("ForexLocalToRemitting");
                    dt.Columns.Add("ForexLocalToUSD");
                    dt.Columns.Add("ForexLocalToPHP");
                    dt.Columns.Add("ForexUSDToPHP");
                    dt.Columns.Add("DatePostedOverseas");
                    dt.Columns.Add("DatePostedLocal");
                    dt.Columns.Add("DateImplementedOverseas");
                    dt.Columns.Add("DateImplementedLocal");
                    dt.Columns.Add("BatchNo");
                    dt.Columns.Add("FileSEQID");
                    dt.Columns.Add("MerchantName");
                    dt.Columns.Add("MerchantID");
                    dt.Columns.Add("MerchantProductCode");
                    dt.Columns.Add("vMerchantProductCode");
                    dt.Columns.Add("OBPDateFrom");
                    dt.Columns.Add("OBPDateTo");
                    dt.Columns.Add("Printed");
                    dt.Columns.Add("IsExtracted");
                    dt.Columns.Add("DateExtracted");
                    dt.Columns.Add("ExtractCTR");
                    //dt.Columns.Add("Deleted");
                    dt.Columns.Add("TXN_SEQ_ID");
                    dt.Columns.Add("DateCreated");
                    dt.Columns.Add("ReceiptHeader1");
                    dt.Columns.Add("ReceiptHeader2");
                    dt.Columns.Add("ReceiptNameOfBranch");
                    dt.Columns.Add("ReceiptAddress");
                    dt.Columns.Add("ReceiptContactNo");
                    dt.Columns.Add("ReceiptEmail");
                    dt.Columns.Add("ReceiptID1");
                    dt.Columns.Add("ReceiptID2");
                    dt.Columns.Add("ReceiptID3");
                    dt.Columns.Add("ReceiptID4");
                    dt.Columns.Add("ReceiptType");
                    dt.Columns.Add("vReceiptType");
                    dt.Columns.Add("ReceiptCorpAddress");
                    dt.Columns.Add("ReceiptCorpTelNo");
                    dt.Columns.Add("ReceiptDeclaration1");
                    dt.Columns.Add("ReceiptDeclaration2");
                    dt.Columns.Add("ReceiptDeclaration3");
                    dt.Columns.Add("ReceiptDeclaration4");
                    dt.Columns.Add("ReceiptDeclaration5");
                    dt.Columns.Add("RemOldCIFID");
                    dt.Columns.Add("RemCreatedDate");
                    dt.Columns.Add("BenCreatedDate");
                    dt.Columns.Add("CoveringBankName");
                    dt.Columns.Add("vPassport");
                    dt.Columns.Add("Passport");
                    dt.Columns.Add("PassportExpiryDate");
                    dt.Columns.Add("PassportCountry");
                    dt.Columns.Add("vDriverLicense");
                    dt.Columns.Add("DriverLicense");
                    dt.Columns.Add("DriverLicenseExpiryDate");
                    dt.Columns.Add("vRemOccupation");
                    dt.Columns.Add("RemOccupation");
                    dt.Columns.Add("RemDetailedOccupation");
                    dt.Columns.Add("Salary");
                    dt.Columns.Add("Position");
                    dt.Columns.Add("Employer");
                    dt.Columns.Add("vRiskRating");
                    dt.Columns.Add("RiskRating");
                    dt.Columns.Add("GWAccountOfficer");
                    dt.Columns.Add("GWAccountOfficerTel");
                    dt.Columns.Add("GWAccountOfficerEmail");
                    dt.Columns.Add("GWComplianceOfficer");
                    dt.Columns.Add("GWComplianceOfficerTel");
                    dt.Columns.Add("GWComplianceOfficerEmail");
                    dt.Columns.Add("GWIsDomestic");
                    dt.Columns.Add("RemAddress1");
                    dt.Columns.Add("RemAddress2");
                    dt.Columns.Add("RemAddress3");
                    dt.Columns.Add("RemAddress4");
                    dt.Columns.Add("RemAddress5");
                    dt.Columns.Add("RemAddress6");
                    dt.Columns.Add("BenAddress1");
                    dt.Columns.Add("BenAddress2");
                    dt.Columns.Add("BenAddress3");
                    dt.Columns.Add("BenAddress4");
                    dt.Columns.Add("BenAddress5");
                    dt.Columns.Add("BenAddress6");
                    break;
            }
            return dt;
        }

        public DateTime getTimeZoneOffline()
        {
            DateTime dtBranchTime = new irsTimeZone().branchLocalTime_nWs(_location);
            return dtBranchTime;
        }

        public DateTime getTimeZone()
        {
            DateTime dtBranchTime = new irsTimeZone().branchLocalTime(_location);
            return dtBranchTime;
        }

        public string getLocation()
        {
            return ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location;
        }

        public string getUserID()
        {
            return ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId;
        }

        private bool validateDS(DataSet ds)
        {
            if (ds != null)
            {
                if (ds.Tables.Count >= 0)
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

        private string[] generateEmptyString(string[] arr, int start, int end)
        {
            int i = 0;
            for (i = start; i <= end; i++)
            {
                arr.SetValue(string.Empty, i);
            }
            return arr;
        }

        public DataTable getLookup(string GroupName)
        {
            base.EnforceConstraint = false;
            base.upds = null;
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

        public string getLookupValue(string GroupName, string FindColumn, string LookValue, string ReturnColumn)
        {
            DataTable dt = this.getLookup(GroupName);
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

        public DataTable getCIF(string CIFID)
        {
            base.EnforceConstraint = false;
            base.upds = null;
            getUpdateDS("csp_getCIF", new Object[] { CIFID });
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

        public DataTable getCIFAddress(long AddressSEQID, CIFClass CIFClass)
        {
            base.EnforceConstraint = false;
            base.upds = null;
            getUpdateDS("csp_getCIFAddress", new Object[] { AddressSEQID, CIFClass });
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

        public DataTable getCIFContacts(long ContactSEQID)
        {
            base.EnforceConstraint = false;
            base.upds = null;
            getUpdateDS("csp_getCIFContacts", new Object[] { ContactSEQID });
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

        public DataTable getCIFIDs(string CIFID)
        {
            base.EnforceConstraint = false;
            base.upds = null;
            getUpdateDS("csp_getCIFIDs", new Object[] { CIFID });
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

        public DataTable getCIFOccupation(string CIFID)
        {
            base.EnforceConstraint = false;
            base.upds = null;
            getUpdateDS("csp_getCIFOccupation", new Object[] { CIFID });
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

        public DataTable getCIFRiskRating(string CIFID)
        {
            base.EnforceConstraint = false;
            base.upds = null;
            getUpdateDS("csp_getCIFRiskRating", new Object[] { CIFID });
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

        public DataTable getGiftsWebSettings(string BranchCode)
        {
            base.EnforceConstraint = false;
            base.upds = null;
            getUpdateDS("csp_getGiftsWebSettings", new Object[] { BranchCode });
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

        public DataTable getCIFCardAccounts(long ID, bool isAccount)
        {
            base.EnforceConstraint = false;
            base.upds = null;
            getUpdateDS("csp_getCIFCardAccounts", new Object[] { ID, (isAccount) ? 1 : 0 });
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

        public DataTable getProductCharges(long SEQID)
        {
            base.EnforceConstraint = false;
            base.upds = null;
            getUpdateDS("csp_getProductCharges", new Object[] { SEQID });
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

        public DataTable getBranchCode(string BranchCode)
        {
            base.EnforceConstraint = false;
            base.upds = null;
            getUpdateDS("csp_getBranchCode", new Object[] { BranchCode });
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

        public DataTable getTransactionBySEQID()
        {
            base.EnforceConstraint = false;
            base.upds = null;
            getUpdateDS("csp_getTransactionBySEQID", new Object[] { this.getLookupValue("IRSWRetention", "FldValue", "1", "FldCode") });
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

        public DataTable getTransactionForUpdateBySEQID()
        {
            base.EnforceConstraint = false;
            base.upds = null;
            getUpdateDS("csp_getTransactionForUpdateBySEQID", new Object[] { });
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

        public DataTable getTransactionForDeletionBySEQID()
        {
            base.EnforceConstraint = false;
            base.upds = null;
            getUpdateDS("csp_getTransactionForDeletionBySEQID", new Object[] { });
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

        public DataTable getRemittanceWithNoCharges()
        {
            base.EnforceConstraint = false;
            base.upds = null;
            getUpdateDS("csp_getRemittanceWithNoCharges", new Object[] { });
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

        public DataTable getRemittanceBySEQID(long SEQID)
        {
            base.EnforceConstraint = false;
            base.upds = null;
            getUpdateDS("csp_getRemittanceBySEQID", new Object[] { SEQID });
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

        public DataTable getCombinationsBySEQID(long SEQID)
        {
            base.EnforceConstraint = false;
            base.upds = null;
            getUpdateDS("csp_getCombinationsBySEQID", new Object[] { SEQID });
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
        public DataTable getPNBBranchWithNoAccount(long SEQID)
        {
            base.EnforceConstraint = false;
            base.upds = null;
            getUpdateDS("csp_getPNBBranchWithNoAccount", new Object[] { SEQID });
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

        public DataTable getOtherDetailsSEQID(long SEQID)
        {
            base.EnforceConstraint = false;
            base.upds = null;
            getUpdateDS("csp_getOtherDetailsBySEQID", new Object[] { SEQID });
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

        public DataTable getCheckDetailsBySEQID(long SEQID)
        {
            base.EnforceConstraint = false;
            base.upds = null;
            getUpdateDS("csp_getCheckDetailsBySEQID", new Object[] { SEQID });
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

        public DataTable getCIFBeneficiaries(string RemitterCIFID, string BeneficiaryCIFID)
        {
            base.EnforceConstraint = false;
            base.upds = null;
            getUpdateDS("csp_getCIFBeneficiaries", new Object[] { RemitterCIFID, BeneficiaryCIFID });
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

        public DataTable getRemitterCards(string ReferenceNo)
        {
            base.EnforceConstraint = false;
            base.upds = null;
            getUpdateDS("csp_getRemitterCards", new Object[] { ReferenceNo });
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

        public DataTable getKYCBySEQID(long SEQID)
        {
            base.EnforceConstraint = false;
            base.upds = null;
            getUpdateDS("csp_getKYCBySEQID", new Object[] { SEQID });
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

        public DataTable getOBPSBySEQID(string ReferenceNo)
        {
            base.EnforceConstraint = false;
            base.upds = null;
            getUpdateDS("csp_getOBPSBySEQID", new Object[] { ReferenceNo });
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

        public DataTable getOBPSMerchantBySEQID(string RemitterCIFID, string BeneficiaryCIFID)
        {
            base.EnforceConstraint = false;
            base.upds = null;
            getUpdateDS("csp_getOBPSMerchantBySEQID", new Object[] { RemitterCIFID, BeneficiaryCIFID });
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
            base.upds = null;
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

        public string getUserID(string UserID)
        {
            base.EnforceConstraint = false;
            base.upds = null;
            getUpdateDS("csp_getUser", new Object[] { UserID });
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

        public string getNSVS(string BranchCode, string Currency)
        {
            base.EnforceConstraint = false;
            base.upds = null;
            getUpdateDS("csp_getBranchNSVS", new Object[] { BranchCode, Currency });
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

        public DataTable getRemittanceCharges(long SEQID)
        {
            base.EnforceConstraint = false;
            base.upds = null;
            getUpdateDS("csp_getRemittanceCharges", new Object[] { SEQID });
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

        public string getDenominations(long SEQID)
        {
            base.EnforceConstraint = false;
            base.upds = null;
            getUpdateDS("SELECT dbo.Func_Get_Remittance_Denomination(" + SEQID + ", 1)");
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

        public DataTable getGFCCardNo(long SEQID)
        {
            base.EnforceConstraint = false;
            base.upds = null;
            getUpdateDS("csp_getGFCCardNo", new Object[] { SEQID });
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

        public DataTable getBranchReceipt(string BranchCode)
        {
            base.EnforceConstraint = false;
            base.upds = null;
            getUpdateDS("csp_getBranchReceipt", new Object[] { BranchCode });
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

        public DataTable getBranchDeclaration(string BranchCode, string NegBranchCode)
        {
            base.EnforceConstraint = false;
            base.upds = null;
            getUpdateDS("csp_getBranchDeclaration", new Object[] { BranchCode, NegBranchCode });
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

        public bool checkRemittance(long SEQID)
        {
            base.EnforceConstraint = false;
            base.upds = null;
            getUpdateDS("SELECT SEQ_ID FROM TBL_REMITTANCE_IRSW WHERE TXN_SEQ_ID = " + SEQID);
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

        private string setDTValue(DataTable dt, string FieldName, string DefaultValue)
        {
            string sReturn = string.Empty;
            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.Rows[0][FieldName] != DBNull.Value)
                {
                    sReturn = dt.Rows[0][FieldName].ToString().Trim();
                }
                else
                {
                    sReturn = DefaultValue;
                }
            }
            else
            {
                sReturn = DefaultValue;
            }
            return sReturn.Replace("'", string.Empty);
        }

        private bool isDTFieldNull(DataTable dt, string FieldName)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.Rows[0][FieldName] != DBNull.Value)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        public string computeCharges(long SEQID)
        {
            DataTable dt = getRemittanceCharges(SEQID);
            double commission = 0.00;
            int commcurr = 0;
            double cable = 0.00;
            int cblecurr = 0;
            double backEnd = 0.00;
            int backendcurr = 0;
            double serviceFee = 0.00;
            int servicecurr = 0;
            double ope = 0.00;
            int opecurr = 0;
            double docStamp = 0.00;
            int docstampcurr = 0;
            double interchangeFee = 0.00;
            int interchangecurr = 0;
            int curr = 0;
            double amount = 0.00;
            int i = 0;
            string sReturn = string.Empty;

            if (dt != null && dt.Rows.Count > 0)
            {
                for (i = 0; i <= (dt.Rows.Count - 1); i++)
                {
                    amount = 0.00;
                    if (dt.Rows[i]["OverrideFlag"] != DBNull.Value && dt.Rows[i]["OverrideFlag"].ToString().Trim().ToLower() == "true")
                    {
                        if (dt.Rows[i]["OverrideAmount"] != DBNull.Value && Convert.ToDouble(dt.Rows[i]["OverrideAmount"].ToString().Trim()) > 0)
                        {
                            amount = Convert.ToDouble(dt.Rows[i]["OverrideAmount"].ToString().Trim());
                        }
                    }
                    else
                    {
                        if (dt.Rows[i]["Amount"] != DBNull.Value && Convert.ToDouble(dt.Rows[i]["Amount"].ToString().Trim()) > 0)
                        {
                            amount = Convert.ToDouble(dt.Rows[i]["Amount"].ToString().Trim());
                        }
                    }
                    if (dt.Rows[i]["ChargeName"] != DBNull.Value)
                    {
                        curr = Convert.ToInt32(dt.Rows[i]["Currency"].ToString().Trim());

                        switch (Convert.ToInt32(dt.Rows[i]["ChargeName"].ToString().Trim()))
                        {
                            case (int)ChargeName.Commission:
                                commission += amount;
                                commcurr = curr;
                                break;
                            case (int)ChargeName.CableCharges:
                                cable += amount;
                                cblecurr = curr;
                                break;
                            case (int)ChargeName.BackEndCharges:
                                backEnd += amount;
                                backendcurr = curr;
                                break;
                            case (int)ChargeName.ServiceFee:
                                serviceFee += amount;
                                servicecurr = curr;
                                break;
                            case (int)ChargeName.OPE:
                                ope += amount;
                                opecurr = curr;
                                break;
                            case (int)ChargeName.DocumentaryStamps:
                                docStamp += amount;
                                docstampcurr = curr;
                                break;
                            case (int)ChargeName.InterchangeFee:
                                interchangeFee += amount;
                                interchangecurr = curr;
                                break;
                        }
                    }
                }
            }
            sReturn = ChargeName.Commission.ToString()      + "="       + commission.ToString()         + "|" +
                    ChargeName.CableCharges.ToString()      + "="       + cable.ToString()              + "|" +
                    ChargeName.BackEndCharges.ToString()    + "="       + backEnd.ToString()            + "|" +
                    ChargeName.ServiceFee.ToString()        + "="       + serviceFee.ToString()         + "|" +
                    ChargeName.OPE.ToString()               + "="       + ope.ToString()                + "|" +
                    ChargeName.DocumentaryStamps.ToString() + "="       + docStamp.ToString()           + "|" +
                    ChargeName.InterchangeFee.ToString()    + "="       + interchangeFee.ToString()     + "|" +
                    ChargeName.Commission.ToString()        + "curr="   + commcurr.ToString()           + "|" +
                    ChargeName.CableCharges.ToString()      + "curr="   + cblecurr.ToString()           + "|" +
                    ChargeName.BackEndCharges.ToString()    + "curr="   + backendcurr.ToString()        + "|" +
                    ChargeName.ServiceFee.ToString()        + "curr="   + servicecurr.ToString()        + "|" +
                    ChargeName.OPE.ToString()               + "curr="   + opecurr.ToString()            + "|" +
                    ChargeName.DocumentaryStamps.ToString() + "curr="   + docstampcurr.ToString()       + "|" +
                    ChargeName.InterchangeFee.ToString()    + "curr="   + interchangecurr.ToString();

            new csAuditLog().writeAudit(_userID, "REMITTANCE COMPUTED CHARGES", "[TransactionSEQID=" + SEQID.ToString() + "][Charges=" + sReturn + "]", "computeCharges", string.Empty, csAuditLog.Category.Select);
            return sReturn;
        }

        private string getCharges(irsCalculator irsCalc)
        {
            string sReturn = string.Empty;
            sReturn = irsCalc.get_branch_local_currency().ToString() + "|";                 // [0] Branch Local Currency Field Value
            sReturn += irsCalc.get_forex_remitting_to_account().ToString("#,###.00000000") + "|"; // [1] Source to Account Forex Amount
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
            sReturn += irsCalc.get_remittance_net_remit_amt().ToString("#,###.00") + "|";   // [27] Net Amount (less charges)
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
            return sReturn;
        }
        #endregion

        #region Save Remittance Parallel
        public int saveRemittanceBySEQID(long SEQID)
        {
            DataTable dtRemittance = getRemittanceBySEQID(SEQID);
            DataTable dtCombinations = getCombinationsBySEQID((isDTFieldNull(dtRemittance, "ProductChargesID")) ? 0 : Convert.ToInt64(dtRemittance.Rows[0]["ProductChargesID"].ToString().Trim()));
            DataTable dtOtherDetails = getOtherDetailsSEQID(SEQID);
            DataTable dtCheckDetails = getCheckDetailsBySEQID(SEQID);
            DataTable dtBranches = getBranches((isDTFieldNull(dtRemittance, "RemittingBranch")) ? "" : dtRemittance.Rows[0]["RemittingBranch"].ToString().Trim());
            DataTable dtRemitter = getCIF((isDTFieldNull(dtRemittance, "RemitterCIFID")) ? "" : dtRemittance.Rows[0]["RemitterCIFID"].ToString().Trim());
            DataTable dtRemitterAddress = getCIFAddress((isDTFieldNull(dtRemittance, "RemitterAddressSEQID")) ? 0 : Convert.ToInt64(dtRemittance.Rows[0]["RemitterAddressSEQID"].ToString().Trim()), CIFClass.Remitter);
            DataTable dtRemitterContact = getCIFContacts((isDTFieldNull(dtRemittance, "RemitterContactSEQID")) ? 0 : Convert.ToInt64(dtRemittance.Rows[0]["RemitterContactSEQID"].ToString().Trim()));
            DataTable dtRemitterIDs = getCIFIDs((isDTFieldNull(dtRemittance, "RemitterCIFID")) ? "" : dtRemittance.Rows[0]["RemitterCIFID"].ToString().Trim());
            DataTable dtRemitterOccupation = getCIFOccupation((isDTFieldNull(dtRemittance, "RemitterCIFID")) ? "" : dtRemittance.Rows[0]["RemitterCIFID"].ToString().Trim());
            DataTable dtRemitterRiskRating = getCIFRiskRating((isDTFieldNull(dtRemittance, "RemitterCIFID")) ? "" : dtRemittance.Rows[0]["RemitterCIFID"].ToString().Trim());
            DataTable dtBeneficiary = getCIF((isDTFieldNull(dtRemittance, "BeneficiaryCIFID")) ? "" : dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString().Trim());
            DataTable dtBeneficiaryAddress = getCIFAddress((isDTFieldNull(dtRemittance, "BeneficiaryAddressSEQID")) ? 0 : Convert.ToInt64(dtRemittance.Rows[0]["BeneficiaryAddressSEQID"].ToString().Trim()), CIFClass.Beneficiary);
            DataTable dtBeneficiaryContact = getCIFContacts((isDTFieldNull(dtRemittance, "BeneficiaryContactSEQID")) ? 0 : Convert.ToInt64(dtRemittance.Rows[0]["BeneficiaryContactSEQID"].ToString().Trim()));
            DataTable dtCIFBeneficies = getCIFBeneficiaries(dtRemittance.Rows[0]["RemitterCIFID"].ToString().Trim(), dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString().Trim());
            DataTable dtRemitterCards = getRemitterCards(dtRemittance.Rows[0]["ReferenceNo"].ToString().Trim());
            DataTable dtBeneficiaryAccounts = getCIFCardAccounts((isDTFieldNull(dtRemittance, "AccountSEQID")) ? 0 : Convert.ToInt64(dtRemittance.Rows[0]["AccountSEQID"].ToString().Trim()), false);
            DataTable dtPNBBranchNoAccount = getPNBBranchWithNoAccount((isDTFieldNull(dtOtherDetails, "BR_SEQ_ID")) ? 0 : Convert.ToInt64(dtOtherDetails.Rows[0]["BR_SEQ_ID"].ToString().Trim()));
            DataTable dtKYC = getKYCBySEQID(SEQID);
            DataTable dtOBPS = getOBPSBySEQID(dtRemittance.Rows[0]["ReferenceNo"].ToString().Trim());
            DataTable dtOBPSMerchant = getOBPSMerchantBySEQID(dtRemittance.Rows[0]["RemitterCIFID"].ToString().Trim(), dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString().Trim());
            DataTable dtGFC = getGFCCardNo((isDTFieldNull(dtRemittance, "AccountSEQID")) ? 0 : Convert.ToInt64(dtRemittance.Rows[0]["AccountSEQID"].ToString().Trim()));
            DataTable dtReceipt = getBranchReceipt(dtRemittance.Rows[0]["RemittingBranch"].ToString().Trim());
            DataTable dtDeclaration = getBranchDeclaration(dtRemittance.Rows[0]["RemittingBranch"].ToString().Trim(), setDTValue(getBranches(dtRemittance.Rows[0]["RemittingBranch"].ToString().Trim()), "NegBranchCode", string.Empty));
            DataTable dtGiftswebSettings = getGiftsWebSettings(dtRemittance.Rows[0]["RemittingBranch"].ToString());

            string[] arrRemittance = new string[206];
            int i = 0;
            int j = 0;
            int k = 0;
            int l = 0;
            int m = 0;
            int n = 0;
            int o = 0;
            int p = 0;
            string[] arrCharges;
            string[] arrChargesValue;
            bool bFrom = false;
            bool bTo = false;
            string sLog = string.Empty;
            int iReturn = 0;
            string sCharges = string.Empty;

            try
            {
                if (dtRemittance != null && dtRemittance.Rows.Count > 0)
                {
                    sCharges = computeCharges(Convert.ToInt64(dtRemittance.Rows[0]["SEQ_ID"].ToString().Trim()));
                    arrRemittance.SetValue(this.getLookupValue("RemitMethod", "FldValue", dtRemittance.Rows[0]["RemitMethod"].ToString().Trim(), "FldDescription"), 0);                 //RemitMethod
                    arrRemittance.SetValue(dtRemittance.Rows[0]["RemitMethod"].ToString().Trim(), 1);                                                                                   //vRemitMethod
                    arrRemittance.SetValue(dtRemittance.Rows[0]["UserID"].ToString().Trim(), 2);                                                                                        //MakerID
                    arrRemittance.SetValue(getUserID(dtRemittance.Rows[0]["UserID"].ToString().Trim()), 3);                                                                             //MakerIDName
                    arrRemittance.SetValue(dtRemittance.Rows[0]["ApprovingOfficer"].ToString().Trim(), 4);                                                                              //CheckerID
                    arrRemittance.SetValue(getUserID(dtRemittance.Rows[0]["ApprovingOfficer"].ToString().Trim()), 5);                                                                   //CherkerIDName
                    arrRemittance.SetValue(setDTValue(dtRemittance, "ReferenceNo", string.Empty), 6);                                                                                   //ReferenceNo
                    arrRemittance.SetValue(setDTValue(dtRemittance, "ExtlReferenceNo", string.Empty), 7);                                                                               //ExtlReferenceNo
                    arrRemittance.SetValue(this.getLookupValue("TranxStatus", "FldValue", dtRemittance.Rows[0]["Transtatus"].ToString().Trim(), "FldDescription"), 8);                  //TranxStatus
                    arrRemittance.SetValue(dtRemittance.Rows[0]["Transtatus"].ToString().Trim(), 9);                                                                                    //vTranxStatus
                    arrRemittance.SetValue(dtRemittance.Rows[0]["TranxDate"].ToString().Trim(), 10);                                                                                    //TranxDate
                    if (dtCombinations != null && dtCombinations.Rows.Count > 0)
                    {
                        if (dtCombinations.Rows[0]["PayMode"].ToString().Trim() == "2")
                        {
                            arrRemittance.SetValue(setDTValue(dtCheckDetails, "Clearing_Date", "01/01/1900 12:00:00 AM"), 11);                                                          //ClearingDate
                            arrRemittance.SetValue(setDTValue(dtCheckDetails, "Check_No", string.Empty), 12);                                                                           //CheckNo
                        }
                        else
                        {
                            arrRemittance.SetValue(setDTValue(dtOtherDetails, "ACH_Clearing_Date", "01/01/1900 12:00:00 AM"), 11);
                            arrRemittance.SetValue(string.Empty, 12);
                        }
                    }
                    else
                    {
                        arrRemittance.SetValue("01/01/1900 12:00:00 AM", 11);
                        arrRemittance.SetValue(string.Empty, 12);
                    }
                    arrRemittance.SetValue(setDTValue(dtRemittance, "API_Message", string.Empty), 13);                                                                                  //APIMessage
                    arrRemittance.SetValue(setDTValue(dtRemittance, "RemittingBranch", string.Empty), 14);                                                                              //RemittingBranch
                    arrRemittance.SetValue(setDTValue(this.getBranchCode(dtRemittance.Rows[0]["RemittingBranch"].ToString().Trim()), "BranchName", string.Empty), 15);                  //RemittingBranchName
                    arrRemittance.SetValue(setDTValue(dtBranches, "NegBranchCode", string.Empty), 16);                                                                                  //NegBranchCode
                    arrRemittance.SetValue(setDTValue(dtBranches, "CoveringBankCode", string.Empty), 17);                                                                               //CoveringBank
                    arrRemittance.SetValue(setDTValue(dtRemittance, "RemitterCIFID", string.Empty), 18);                                                                                //RemitterID
                    arrRemittance.SetValue(setDTValue(dtRemitter, "CIFType", string.Empty), 19);                                                                                        //RemType
                    arrRemittance.SetValue(setDTValue(dtRemitter, "LastName", string.Empty), 20);                                                                                       //RemLastName
                    arrRemittance.SetValue(setDTValue(dtRemitter, "FirstName", string.Empty), 21);                                                                                      //RemFirstName
                    arrRemittance.SetValue(setDTValue(dtRemitter, "MiddleName", string.Empty), 22);                                                                                     //RemMiddleName
                    arrRemittance.SetValue(setDTValue(dtRemitter, "BirthDate", string.Empty), 23);                                                                                      //RemBirthDate
                    /*** REMITTER CONTACTS ***/
                    if (dtRemitterContact != null && dtRemitterContact.Rows.Count > 0)
                    {
                        if (dtRemitterContact.Rows[0]["ContactType"].ToString().Trim().IndexOf("LandLine") > 0)
                        {
                            arrRemittance.SetValue(setDTValue(dtRemitterContact, "ContactNo", string.Empty), 24);                                                                       //RemHomePhone
                            arrRemittance.SetValue(string.Empty, 25);
                        }
                        else
                        {
                            arrRemittance.SetValue(string.Empty, 24);
                            arrRemittance.SetValue(setDTValue(dtRemitterContact, "ContactNo", string.Empty), 25);                                                                       //RemMobilePhone
                        }
                    }
                    else
                    {
                        arrRemittance = this.generateEmptyString(arrRemittance, 24, 25);
                    }
                    /*** REMITTER ADDRESS ***/
                    arrRemittance.SetValue(setDTValue(dtRemitterAddress, "POBox", string.Empty), 26);                                                                                   //RemPOBox
                    arrRemittance.SetValue(setDTValue(dtRemitterAddress, "Address", string.Empty), 27);                                                                                 //RemAddress
                    arrRemittance.SetValue(setDTValue(dtRemitterAddress, "ZipCode", string.Empty), 28);                                                                                 //RemZipCode
                    arrRemittance.SetValue(setDTValue(dtRemitterAddress, "Country", string.Empty), 29);                                                                                 //RemCountry
                    arrRemittance.SetValue(setDTValue(dtRemitterContact, "EmailAddress", string.Empty), 30);                                                                            //RemEmailAddress
                    arrRemittance.SetValue(setDTValue(dtRemitter, "VIPType", string.Empty), 31);                                                                                        //RemVIPType
                    /*** REMITTER IDS ***/
                    if (dtRemitterIDs != null && dtRemitterIDs.Rows.Count > 0)
                    {
                        j = 32;
                        int iIDs = 0;
                        if (dtRemitterIDs.Rows.Count > 3)
                        {
                            iIDs = 4;
                        }
                        else
                        {
                            iIDs = dtRemitterIDs.Rows.Count;
                        }
                        for (i = 0; i <= (iIDs - 1); i++)
                        {
                            arrRemittance.SetValue(dtRemitterIDs.Rows[i]["IDNoMerge"].ToString(), j);                                                                                        //RemID1, RemID2, RemID3, RemID4
                            j++;
                        }
                    }
                    j = ((j == 0) ? 32 : j);
                    while (j <= 35)
                    {
                        arrRemittance.SetValue(string.Empty, j);
                        j++;
                    }
                    arrRemittance.SetValue(setDTValue(dtRemittance, "BeneficiaryCIFID", string.Empty), 36);                                                                             //BeneficiaryID
                    arrRemittance.SetValue(setDTValue(dtBeneficiary, "CIFType", string.Empty), 37);                                                                                     //BenType
                    arrRemittance.SetValue(setDTValue(dtBeneficiary, "LastName", string.Empty), 38);                                                                                    //BenLastName
                    arrRemittance.SetValue(setDTValue(dtBeneficiary, "FirstName", string.Empty), 39);                                                                                   //BenFirstName
                    arrRemittance.SetValue(setDTValue(dtBeneficiary, "MiddleName", string.Empty), 40);                                                                                  //BenMiddleName
                    arrRemittance.SetValue(setDTValue(dtBeneficiary, "BirthDate", string.Empty), 41);                                                                                   //BenBirthDate
                    /*** BENEFICIARY CONTACTS ***/
                    if (dtBeneficiaryContact != null && dtBeneficiaryContact.Rows.Count > 0)
                    {
                        if (dtBeneficiaryContact.Rows[0]["ContactType"].ToString().Trim().IndexOf("LandLine") > 0)
                        {
                            arrRemittance.SetValue(setDTValue(dtBeneficiaryContact, "ContactNo", string.Empty), 42);                                                                    //BenHomePhone
                            arrRemittance.SetValue(string.Empty, 43);
                        }
                        else
                        {
                            arrRemittance.SetValue(string.Empty, 42);
                            arrRemittance.SetValue(setDTValue(dtBeneficiaryContact, "ContactNo", string.Empty), 43);                                                                    //BenMobilePhone
                        }
                    }
                    else
                    {
                        arrRemittance = this.generateEmptyString(arrRemittance, 42, 43);
                    }
                    /*** BENEFICIARY ADDRESS ***/
                    arrRemittance.SetValue(setDTValue(dtBeneficiaryAddress, "POBox", string.Empty), 44);                                                                                //BenPOBox
                    arrRemittance.SetValue(setDTValue(dtBeneficiaryAddress, "Address", string.Empty), 45);                                                                              //BenAddress
                    arrRemittance.SetValue(setDTValue(dtBeneficiaryAddress, "ZipCode", string.Empty), 46);                                                                              //BenZipCode
                    arrRemittance.SetValue(setDTValue(dtBeneficiaryAddress, "Country", string.Empty), 47);                                                                              //BenCountry
                    arrRemittance.SetValue(setDTValue(dtBeneficiaryContact, "EmailAddress", string.Empty), 48);                                                                         //BenEmailAddress
                    arrRemittance.SetValue(this.getLookupValue("Relationship", "FldValue", setDTValue(dtCIFBeneficies, "Relationship", string.Empty), "FldDescription"), 49);           //Relationship
                    arrRemittance.SetValue(setDTValue(dtCIFBeneficies, "Relationship", string.Empty), 50);                                                                              //vRelationship
                    arrRemittance.SetValue(this.getLookupValue("NotificationMode", "FldValue", setDTValue(dtCIFBeneficies, "NotificationMode", string.Empty), "FldDescription"), 51);   //NotificationMode
                    arrRemittance.SetValue(setDTValue(dtCIFBeneficies, "NotificationMode", string.Empty), 52);                                                                          //vNotificationMode
                    if (dtRemittance.Rows[0]["RemitterCIFID"].ToString().Trim() == dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString().Trim())
                    {
                        arrRemittance.SetValue("True", 53);                                                                                                                                //SelfAllotee
                    }
                    else
                    {
                        arrRemittance.SetValue("False", 53);
                    }
                    arrRemittance.SetValue(getNSVS(dtRemittance.Rows[0]["RemittingBranch"].ToString().Trim(), dtRemittance.Rows[0]["NetToBeneCurrency"].ToString().Trim()), 54);        //NSVS
                    /*** REMITTER CARDS DETAIL ***/
                    arrRemittance.SetValue(setDTValue(dtRemitterCards, "CardNo", string.Empty), 55);                                                                                    //CardNo
                    arrRemittance.SetValue(setDTValue(dtRemitterCards, "CardName", string.Empty), 56);                                                                                  //CardName
                    arrRemittance.SetValue(setDTValue(dtRemitterCards, "ExpiryDate", string.Empty), 57);                                                                                //CardExpiryDate
                    arrRemittance.SetValue(setDTValue(dtRemitterCards, "CardType", string.Empty), 58);                                                                                  //CardType
                    arrRemittance.SetValue(setDTValue(dtRemitterCards, "CardClass", string.Empty), 59);                                                                                 //CardClass
                    arrRemittance.SetValue(setDTValue(dtRemitterCards, "CardCurrency", string.Empty), 60);                                                                              //CardCurrency
                    if (dtCombinations != null)
                    {
                        //if (dtCombinations.Rows[0]["ProductCode"].ToString() == "12" || dtCombinations.Rows[0]["ProductCode"].ToString() == "13" || dtCombinations.Rows[0]["ProductCode"].ToString() == "16" || dtCombinations.Rows[0]["ProductCode"].ToString() == "23") //PDDTS|NON-PDDTS|DTB|IBFT
                        if (dtCombinations.Rows[0]["ProductCode"].ToString() == "12" || dtCombinations.Rows[0]["ProductCode"].ToString() == "13" || dtCombinations.Rows[0]["ProductCode"].ToString() == "16" || dtCombinations.Rows[0]["ProductCode"].ToString() == "23" || dtCombinations.Rows[0]["ProductCode"].ToString() == "258") //PDDTS|NON-PDDTS|DTB|IBFT|INSTAPAY
                        {
                            for (k = 61; k <= 65; k++)
                            {
                                arrRemittance.SetValue(string.Empty, k);
                            }
                            arrRemittance.SetValue(setDTValue(dtBeneficiaryAccounts, "AccountNo", string.Empty), 66);                                                                   //OtherBankAccountNo
                            arrRemittance.SetValue(setDTValue(dtBeneficiaryAccounts, "OtherBankName", string.Empty), 67);                                                               //OtherBankName
                            arrRemittance.SetValue(setDTValue(dtBeneficiaryAccounts, "Currency", string.Empty), 68);                                                                    //OtherBankCurrency
                            if (dtCombinations.Rows[0]["ProductCode"].ToString() == "12")
                            {
                                arrRemittance.SetValue(setDTValue(this.getBranchCode(dtBeneficiaryAccounts.Rows[0]["OtherBankCode"].ToString().Trim()), "BranchName", string.Empty), 69);
                                arrRemittance.SetValue(setDTValue(dtBeneficiaryAccounts, "OtherBankCode", string.Empty), 70);                                                           //PDDTSBankCode
                            }
                            else
                            {
                                arrRemittance.SetValue(setDTValue(dtBeneficiaryAccounts, "OtherBankCode", string.Empty), 69);                                                           //OtherBankBranchCode
                                arrRemittance.SetValue(string.Empty, 70);
                            }
                        }
                        else
                        {
                            if (dtCombinations.Rows[0]["ProductCode"].ToString() == "27") //APAS
                            {
                                arrRemittance.SetValue(this.getLookupValue("APAS_Online", "FldValue", setDTValue(dtRemittance, "NetToBeneCurrency", string.Empty), "FldCode"), 61);     //AccountNo
                            }
                            else
                            {
                                arrRemittance.SetValue(setDTValue(dtBeneficiaryAccounts, "AccountNo", string.Empty), 61);                                                               //AccountNo
                            }
                            arrRemittance.SetValue(setDTValue(dtBeneficiaryAccounts, "AccountType", string.Empty), 62);                                                                 //AccountType
                            arrRemittance.SetValue(setDTValue(dtBeneficiaryAccounts, "vAccountType", string.Empty), 63);                                                                //vAccountType
                            arrRemittance.SetValue(setDTValue(dtBeneficiaryAccounts, "Currency", string.Empty), 64);                                                                    //AccountCurrency
                            if (setDTValue(dtBeneficiaryAccounts, "PNBBranchCode", string.Empty) != String.Empty)
                            {
                                arrRemittance.SetValue(setDTValue(dtBeneficiaryAccounts, "PNBBranchCode", string.Empty), 65);                                                           //AccountBranchCode
                            }
                            else
                            {
                                arrRemittance.SetValue(setDTValue(dtPNBBranchNoAccount, "PNBBranchCode", string.Empty), 65);
                            }
                            arrRemittance = this.generateEmptyString(arrRemittance, 66, 70);
                        }
                    }
                    else
                    {
                        arrRemittance = this.generateEmptyString(arrRemittance, 61, 70);
                    }
                    arrRemittance.SetValue(setDTValue(dtGFC, "CardNo", string.Empty), 71);                                                                                              //GFCCardNo
                    arrRemittance.SetValue(setDTValue(dtKYC, "FullName", string.Empty), 72);                                                                                            //KYCName
                    arrRemittance.SetValue(setDTValue(dtKYC, "Address", string.Empty), 73);                                                                                             //KYCAddress
                    arrRemittance.SetValue(setDTValue(dtBeneficiaryAddress, "OSA", string.Empty), 74);                                                                                  //OSA
                    arrRemittance.SetValue(this.getLookupValue("ProductCode", "FldValue", setDTValue(dtCombinations, "ProductCode", string.Empty), "FldDescription"), 75);              //ProductCode
                    arrRemittance.SetValue(setDTValue(dtCombinations, "ProductCode", string.Empty), 76);                                                                                //vProductCode
                    arrRemittance.SetValue(this.getLookupValue("ImplementationType", "FldValue", setDTValue(dtCombinations, "ImplementationType", string.Empty), "FldDescription"), 77);//ImplementationType
                    arrRemittance.SetValue(setDTValue(dtCombinations, "ImplementationType", string.Empty), 78);                                                                         //vImplementationType
                    arrRemittance.SetValue(this.getLookupValue("Currency", "FldValue", setDTValue(dtRemittance, "LocalCurrency", string.Empty), "FldCode"), 79);                        //LocalCurrency
                    arrRemittance.SetValue(setDTValue(dtRemittance, "LocalCurrency", string.Empty), 80);                                                                                //vLocalCurrency
                    arrRemittance.SetValue(this.getLookupValue("ChargeType", "FldValue", setDTValue(dtCombinations, "ChargeType", string.Empty), "FldDescription"), 81);                //ChargeType
                    arrRemittance.SetValue(setDTValue(dtCombinations, "ChargeType", string.Empty), 82);                                                                                 //vChargeType
                    arrRemittance.SetValue(this.getLookupValue("PayMode", "FldValue", setDTValue(dtCombinations, "PayMode", string.Empty), "FldDescription"), 83);                      //PayMode
                    arrRemittance.SetValue(setDTValue(dtCombinations, "PayMode", string.Empty), 84);                                                                                    //vPayMode
                    arrRemittance.SetValue(this.getLookupValue("Computation", "FldValue", setDTValue(dtCombinations, "ComputationLevel", string.Empty), "FldDescription"), 85);         //ComputationLevel
                    arrRemittance.SetValue(setDTValue(dtCombinations, "ComputationLevel", string.Empty), 86);                                                                           //vComputationLevel
                    arrRemittance.SetValue(this.getLookupValue("TranxSource", "FldValue", setDTValue(dtRemittance, "TransactionSource", string.Empty), "FldDescription"), 87);          //TranxSource
                    arrRemittance.SetValue(setDTValue(dtRemittance, "TransactionSource", string.Empty), 88);                                                                            //vTranxSource
                    arrRemittance.SetValue(this.getLookupValue("TranxPurpose", "FldValue", setDTValue(dtRemittance, "TransactionPurpose", string.Empty), "FldDescription"), 89);        //TranxPurpose
                    arrRemittance.SetValue(setDTValue(dtRemittance, "TransactionPurpose", string.Empty), 90);                                                                           //vTranxPurpose
                    arrRemittance.SetValue(setDTValue(dtRemittance, "OtherPurpose", string.Empty), 91);                                                                                 //OtherPurpose
                    arrRemittance.SetValue(this.getLookupValue("ReceiptMode", "FldValue", setDTValue(dtRemittance, "ReceiptMode", string.Empty), "FldDescription"), 92);                //ReceiptMode
                    arrRemittance.SetValue(setDTValue(dtRemittance, "ReceiptMode", string.Empty), 93);                                                                                  //vReceiptMode
                    arrRemittance.SetValue(this.getLookupValue("SourceOfFunds", "FldValue", setDTValue(dtRemittance, "SourceOfFunds", string.Empty), "FldDescription"), 94);            //SourceOfFunds
                    arrRemittance.SetValue(setDTValue(dtRemittance, "SourceOfFunds", string.Empty), 95);                                                                                //vSourceOfFunds
                    arrRemittance.SetValue(setDTValue(dtRemittance, "SpecialMessage", string.Empty), 96);                                                                               //SpecialMessage
                    arrRemittance.SetValue(this.getLookupValue("Currency", "FldValue", setDTValue(dtRemittance, "RemittingCurrency", string.Empty), "FldCode"), 97);                    //RemittingCurrency
                    arrRemittance.SetValue(setDTValue(dtRemittance, "RemittingCurrency", string.Empty), 98);                                                                            //vRemittingCurrency
                    arrRemittance.SetValue(setDTValue(dtRemittance, "GrossRemAmount", string.Empty), 99);                                                                               //GrossAmount
                    arrRemittance.SetValue(setDTValue(dtRemittance, "NetRemAmount", string.Empty), 100);                                                                                //NetAmount
                    arrRemittance.SetValue(this.getLookupValue("Currency", "FldValue", setDTValue(dtRemittance, "NetToBeneCurrency", string.Empty), "FldCode"), 101);                   //NetToBenCurrency
                    arrRemittance.SetValue(setDTValue(dtRemittance, "NetToBeneCurrency", string.Empty), 102);                                                                           //vNetToBenCurrency
                    arrRemittance.SetValue(setDTValue(dtRemittance, "NetToBeneAmount", string.Empty), 103);                                                                             //NetToBenAmount
                    arrRemittance.SetValue(setDTValue(dtRemittance, "NetToBeneAmountPHP", string.Empty), 104);                                                                          //NetToBenAmountPHP
                    arrRemittance.SetValue(setDTValue(dtRemittance, "TotalLocalCharge", string.Empty), 105);                                                                            //TotalCharges
                    
                    arrCharges = sCharges.Split('|');
                    for (m = 0; m <= (arrCharges.Length - 1); m++)
                    {
                        arrChargesValue = arrCharges.GetValue(m).ToString().Split('=');
                        if (arrChargesValue.GetValue(0).ToString().Equals("BackEndCharges"))
                        {
                            arrRemittance.SetValue(arrChargesValue.GetValue(1).ToString().Trim(), 106);                                                                                 //TotalBackend
                        }
                        if (arrChargesValue.GetValue(0).ToString().Equals("BackEndChargescurr"))
                        {
                            arrRemittance.SetValue(arrChargesValue.GetValue(1).ToString().Trim(), 107);                                                                                 //BackendCurrency
                        }
                        if (arrChargesValue.GetValue(0).ToString().Equals("Commission"))
                        {
                            arrRemittance.SetValue(arrChargesValue.GetValue(1).ToString().Trim(), 108);                                                                                 //TotalCommision
                        }
                        if (arrChargesValue.GetValue(0).ToString().Equals("Commissioncurr"))
                        {
                            arrRemittance.SetValue(arrChargesValue.GetValue(1).ToString().Trim(), 109);                                                                                 //CommisionCurrency
                        }
                        if (arrChargesValue.GetValue(0).ToString().Equals("CableCharges"))
                        {
                            arrRemittance.SetValue(arrChargesValue.GetValue(1).ToString().Trim(), 110);                                                                                 //TotalCable
                        }
                        if (arrChargesValue.GetValue(0).ToString().Equals("CableChargescurr"))
                        {
                            arrRemittance.SetValue(arrChargesValue.GetValue(1).ToString().Trim(), 111);                                                                                 //CableCurrency
                        }
                        if (arrChargesValue.GetValue(0).ToString().Equals("ServiceFee"))
                        {
                            arrRemittance.SetValue(arrChargesValue.GetValue(1).ToString().Trim(), 112);                                                                                 //TotalServiceFee
                        }
                        if (arrChargesValue.GetValue(0).ToString().Equals("ServiceFeecurr"))
                        {
                            arrRemittance.SetValue(arrChargesValue.GetValue(1).ToString().Trim(), 113);                                                                                 //ServiceCurrency
                        }
                        if (arrChargesValue.GetValue(0).ToString().Equals("OPE"))
                        {
                            arrRemittance.SetValue(arrChargesValue.GetValue(1).ToString().Trim(), 114);                                                                                 //TotalOPE
                        }
                        if (arrChargesValue.GetValue(0).ToString().Equals("OPEcurr"))
                        {
                            arrRemittance.SetValue(arrChargesValue.GetValue(1).ToString().Trim(), 115);                                                                                 //OPECurrency
                        }
                        if (arrChargesValue.GetValue(0).ToString().Equals("DocumentaryStamps"))
                        {
                            arrRemittance.SetValue(arrChargesValue.GetValue(1).ToString().Trim(), 116);                                                                                 //TotalDocStamp
                        }
                        if (arrChargesValue.GetValue(0).ToString().Equals("DocumentaryStampscurr"))
                        {
                            arrRemittance.SetValue(arrChargesValue.GetValue(1).ToString().Trim(), 117);                                                                                 //DocstampCurrency
                        }
                        if (arrChargesValue.GetValue(0).ToString().Equals("InterchangeFee"))
                        {
                            arrRemittance.SetValue(arrChargesValue.GetValue(1).ToString().Trim(), 118);                                                                                 //TotalInterchangeFee
                        }
                        if (arrChargesValue.GetValue(0).ToString().Equals("InterchangeFeecurr"))
                        {
                            arrRemittance.SetValue(arrChargesValue.GetValue(1).ToString().Trim(), 119);                                                                                 //InterchangefeeCurrency
                        }
                    }
                    arrRemittance.SetValue(setDTValue(dtRemittance, "ChargeLocally", "0"), 120);                                                                                        //ChargesLocally
                    arrRemittance.SetValue(getDenominations(Convert.ToInt64(dtRemittance.Rows[0]["SEQ_ID"].ToString().Trim())), 121);                                                   //Denominations
                    arrRemittance.SetValue(setDTValue(dtRemittance, "ForexRemittingToAccount", string.Empty), 122);                                                                     //ForexRemittingToAccount
                    arrRemittance.SetValue(setDTValue(dtRemittance, "ForexRemittingToLocal", string.Empty), 123);                                                                       //ForexRemittingToLocal
                    arrRemittance.SetValue(setDTValue(dtRemittance, "ForexRemittingToUSD", string.Empty), 124);                                                                         //ForexRemittingToUSD
                    arrRemittance.SetValue(setDTValue(dtRemittance, "ForexRemittingToPHP", string.Empty), 125);                                                                         //ForexRemittingToPHP
                    arrRemittance.SetValue(setDTValue(dtRemittance, "ForexRemittingToEUR", string.Empty), 126);                                                                         //ForexRemittingToEUR
                    arrRemittance.SetValue(setDTValue(dtRemittance, "ForexLocalToRemitting", string.Empty), 127);                                                                       //ForexLocalToRemitting
                    arrRemittance.SetValue(setDTValue(dtRemittance, "ForexLocalToUSD", string.Empty), 128);                                                                             //ForexLocalToUSD
                    arrRemittance.SetValue(setDTValue(dtRemittance, "ForexLocalToPHP", string.Empty), 129);                                                                             //ForexLocalToPHP
                    arrRemittance.SetValue(setDTValue(dtRemittance, "ForexUSDToPHP", string.Empty), 130);                                                                               //ForexUSDToPHP
                    arrRemittance.SetValue(setDTValue(dtRemittance, "DatePostedOverseas", string.Empty), 131);                                                                          //DatePostedOverseas
                    arrRemittance.SetValue(setDTValue(dtRemittance, "DatePostedLocal", string.Empty), 132);                                                                             //DatePostedLocal
                    arrRemittance.SetValue(setDTValue(dtRemittance, "DateImplementedOverseas", string.Empty), 133);                                                                     //DateImplementedOverseas
                    arrRemittance.SetValue(setDTValue(dtRemittance, "DateImplementedLocal", string.Empty), 134);                                                                        //DateImplementedLocal
                    arrRemittance.SetValue(setDTValue(dtRemittance, "BatchNo", string.Empty), 135);                                                                                     //BatchNo
                    arrRemittance.SetValue(setDTValue(dtOtherDetails, "FileInvSEQID", string.Empty), 136);                                                                              //FileSEQID
                    arrRemittance.SetValue(setDTValue(dtOBPS, "MerchantCode", string.Empty), 137);                                                                                      //MerchantName
                    arrRemittance.SetValue(setDTValue(dtOBPSMerchant, "MemberIDNo", string.Empty), 138);                                                                                //MerchantID
                    arrRemittance.SetValue(setDTValue(dtOBPS, "MerchantProductCode", string.Empty), 139);                                                                               //MerchangeProductCode
                    arrRemittance.SetValue(setDTValue(dtOBPS, "vMerchantProductCode", string.Empty), 140);                                                                              //vMerchangeProductCode
                    if (dtOBPS != null && dtOBPS.Rows.Count > 0)
                    {
                        for (l = 0; l <= (dtOBPS.Rows.Count - 1); l++)
                        {
                            if (dtOBPS.Rows[l]["FieldLabel"] != DBNull.Value && dtOBPS.Rows[l]["FieldLabel"].ToString().Trim().ToUpper().IndexOf("(FROM)") > 0)
                            {
                                arrRemittance.SetValue(dtOBPS.Rows[l]["FieldValue"].ToString().Trim(), 141);                                                                            //OBPDateFrom
                                bFrom = true;
                            }
                            if (dtOBPS.Rows[l]["FieldLabel"] != DBNull.Value && dtOBPS.Rows[l]["FieldLabel"].ToString().Trim().ToUpper().IndexOf("(TO)") > 0)
                            {
                                arrRemittance.SetValue(dtOBPS.Rows[l]["FieldValue"].ToString().Trim(), 142);                                                                            //OBPDateTo
                                bTo = true;
                            }
                        }
                    }
                    if (bFrom == false)
                    {
                        arrRemittance.SetValue("01/01/1900 12:00:00 AM", 141);
                    }
                    if (bTo == false)
                    {
                        arrRemittance.SetValue("01/01/1900 12:00:00 AM", 142);
                    }
                    arrRemittance.SetValue(setDTValue(dtRemittance, "Printed", "False"), 143);                                                                                          //Printed
                    arrRemittance.SetValue(setDTValue(dtRemittance, "IsExtracted", "False"), 144);                                                                                      //IsExtracted
                    arrRemittance.SetValue(setDTValue(dtRemittance, "DateExtracted", "01/01/1900 12:00:00 AM"), 145);                                                                   //DateExtracted
                    arrRemittance.SetValue(setDTValue(dtRemittance, "ExtractCTR", "False"), 146);                                                                                       //ExtractCTR
                    arrRemittance.SetValue(setDTValue(dtRemittance, "SEQ_ID", string.Empty), 147);                                                                                      //TXN_SEQ_ID
                    arrRemittance.SetValue(DateTime.Now.ToString(), 148);                                                                                                               //DateCreated
                    arrRemittance.SetValue(setDTValue(dtReceipt, "Header1", string.Empty), 149);                                                                                        //ReceiptHeader1
                    arrRemittance.SetValue(setDTValue(dtReceipt, "Header2", string.Empty), 150);                                                                                        //ReceiptHeader2
                    arrRemittance.SetValue(setDTValue(dtReceipt, "NameOfBranch", string.Empty), 151);                                                                                   //ReceiptNameOfBranch
                    arrRemittance.SetValue(setDTValue(dtReceipt, "ReceiptAddress", string.Empty), 152);                                                                                 //ReceiptAddress
                    arrRemittance.SetValue(setDTValue(dtReceipt, "ReceiptContactNo", string.Empty), 153);                                                                               //ReceiptContactNo
                    arrRemittance.SetValue(setDTValue(dtReceipt, "ReceiptEmail", string.Empty), 154);                                                                                   //ReceiptEmail
                    arrRemittance.SetValue(setDTValue(dtReceipt, "ReceiptID1", string.Empty), 155);                                                                                     //ReceiptID1
                    arrRemittance.SetValue(setDTValue(dtReceipt, "ReceiptID2", string.Empty), 156);                                                                                     //ReceiptID2
                    arrRemittance.SetValue(setDTValue(dtReceipt, "ReceiptID3", string.Empty), 157);                                                                                     //ReceiptID3
                    arrRemittance.SetValue(setDTValue(dtReceipt, "ReceiptID4", string.Empty), 158);                                                                                     //ReceiptID4
                    arrRemittance.SetValue(setDTValue(dtReceipt, "ReceiptType", string.Empty), 159);                                                                                    //ReceiptType
                    arrRemittance.SetValue(setDTValue(dtReceipt, "vReceiptType", string.Empty), 160);                                                                                   //vReceiptType
                    arrRemittance.SetValue(setDTValue(dtReceipt, "Corp_Address", string.Empty), 161);                                                                                   //ReceiptCorpAddress
                    arrRemittance.SetValue(setDTValue(dtReceipt, "Corp_TelNo", string.Empty), 162);                                                                                     //ReceiptCorpTelNo
                    arrRemittance.SetValue(setDTValue(dtDeclaration, "Declaration1", string.Empty), 163);                                                                               //ReceiptDeclaration1
                    arrRemittance.SetValue(setDTValue(dtDeclaration, "Declaration2", string.Empty), 164);                                                                               //ReceiptDeclaration2
                    arrRemittance.SetValue(setDTValue(dtDeclaration, "Declaration3", string.Empty), 165);                                                                               //ReceiptDeclaration3
                    arrRemittance.SetValue(setDTValue(dtDeclaration, "Declaration4", string.Empty), 166);                                                                               //ReceiptDeclaration4
                    arrRemittance.SetValue(setDTValue(dtDeclaration, "Declaration5", string.Empty), 167);                                                                               //ReceiptDeclaration5
                    arrRemittance.SetValue(setDTValue(dtRemitter, "OldCIFID", string.Empty), 168);                                                                                      //RemOldCIFID
                    arrRemittance.SetValue(setDTValue(dtRemitter, "DateCreated", string.Empty), 169);                                                                                   //RemCreatedDate
                    arrRemittance.SetValue(setDTValue(dtBeneficiary, "DateCreated", string.Empty), 170);                                                                                //BenCreatedDate
                    arrRemittance.SetValue(this.getLookupValue("CoveringBank", "FldCode", setDTValue(dtBranches, "CoveringBankCode", string.Empty), "FldDescription"), 171);            //CoveringBankName
                    o = ((o == 0) ? 172 : o);
                    while (o <= 178)
                    {
                        arrRemittance.SetValue(string.Empty, o);
                        o++;
                    }
                    if (dtRemitterIDs != null && dtRemitterIDs.Rows.Count > 0)
                    {
                        for (o = 0; o <= (dtRemitterIDs.Rows.Count - 1); o++)
                        {
                            if (dtRemitterIDs.Rows[o]["IDType"].ToString().IndexOf("Passport") > 0)
                            {
                                arrRemittance.SetValue(setDTValue(dtRemitterIDs, "vIDType", string.Empty), 172);                                                                        //vPassport
                                arrRemittance.SetValue(setDTValue(dtRemitterIDs, "IDNo", string.Empty), 173);                                                                           //Passport
                                arrRemittance.SetValue(setDTValue(dtRemitterIDs, "ExpiryDate", string.Empty), 174);                                                                     //PassportExpiryDate
                                arrRemittance.SetValue(setDTValue(dtRemitterIDs, "PassportCountry", string.Empty), 175);                                                                //PassportCountry
                            }
                            if (dtRemitterIDs.Rows[o]["IDType"].ToString().IndexOf("License") > 0)
                            {
                                arrRemittance.SetValue(setDTValue(dtRemitterIDs, "vIDType", string.Empty), 176);                                                                        //vDriverLicense
                                arrRemittance.SetValue(setDTValue(dtRemitterIDs, "IDNo", string.Empty), 177);                                                                           //DriverLicense
                                arrRemittance.SetValue(setDTValue(dtRemitterIDs, "ExpiryDate", string.Empty), 178);                                                                     //DriverLicenseExpiryDate
                            }
                        }
                    }
                    arrRemittance.SetValue(setDTValue(dtRemitterOccupation, "vOccupation", string.Empty), 179);                                                                         //vRemOccupation
                    arrRemittance.SetValue(setDTValue(dtRemitterOccupation, "Occupation", string.Empty), 180);                                                                          //RemOccupation
                    arrRemittance.SetValue(setDTValue(dtRemitterOccupation, "DetailedOccupation", string.Empty), 181);                                                                  //RemDetailedOccupation
                    arrRemittance.SetValue(setDTValue(dtRemitterOccupation, "Salary", string.Empty), 182);                                                                              //Salary
                    arrRemittance.SetValue(setDTValue(dtRemitterOccupation, "Position", string.Empty), 183);                                                                            //Position
                    arrRemittance.SetValue(setDTValue(dtRemitterOccupation, "Employer", string.Empty), 184);                                                                            //Employer
                    arrRemittance.SetValue(setDTValue(dtRemitterRiskRating, "vRiskRating", string.Empty), 185);                                                                         //vRiskRating
                    arrRemittance.SetValue(setDTValue(dtRemitterRiskRating, "RiskRating", string.Empty), 186);                                                                          //RiskRating
                    arrRemittance.SetValue(setDTValue(dtGiftswebSettings, "AccountOfficer", string.Empty), 187);                                                                        //GWAccountOfficer
                    arrRemittance.SetValue(setDTValue(dtGiftswebSettings, "AccountOfficerTel", string.Empty), 188);                                                                     //GWAccountOfficerTel
                    arrRemittance.SetValue(setDTValue(dtGiftswebSettings, "AccountOfficerEmail", string.Empty), 189);                                                                   //GWAccountOfficerEmail
                    arrRemittance.SetValue(setDTValue(dtGiftswebSettings, "ComplianceOfficer", string.Empty), 190);                                                                     //GWComplianceOfficer
                    arrRemittance.SetValue(setDTValue(dtGiftswebSettings, "ComplianceOfficerTel", string.Empty), 191);                                                                  //GWComplianceOfficerTel
                    arrRemittance.SetValue(setDTValue(dtGiftswebSettings, "ComplianceOfficerEmail", string.Empty), 192);                                                                //GWComplianceOfficerEmail
                    arrRemittance.SetValue(setDTValue(dtGiftswebSettings, "IsDomestic", string.Empty), 193);                                                                            //GWIsDomestic
                    arrRemittance.SetValue(setDTValue(dtRemitterAddress, "Address1", string.Empty), 194);                                                                               //RemAddress1
                    arrRemittance.SetValue(setDTValue(dtRemitterAddress, "Address2", string.Empty), 195);                                                                               //RemAddress2
                    arrRemittance.SetValue(setDTValue(dtRemitterAddress, "Address3", string.Empty), 196);                                                                               //RemAddress3
                    arrRemittance.SetValue(setDTValue(dtRemitterAddress, "Address4", string.Empty), 197);                                                                               //RemAddress4
                    arrRemittance.SetValue(setDTValue(dtRemitterAddress, "Address5", string.Empty), 198);                                                                               //RemAddress5
                    arrRemittance.SetValue(setDTValue(dtRemitterAddress, "Address6", string.Empty), 199);                                                                               //RemAddress6
                    arrRemittance.SetValue(setDTValue(dtBeneficiaryAddress, "Address1", string.Empty), 200);                                                                            //BenAddress1
                    arrRemittance.SetValue(setDTValue(dtBeneficiaryAddress, "Address2", string.Empty), 201);                                                                            //BenAddress2
                    arrRemittance.SetValue(setDTValue(dtBeneficiaryAddress, "Address3", string.Empty), 202);                                                                            //BenAddress3
                    arrRemittance.SetValue(setDTValue(dtBeneficiaryAddress, "Address4", string.Empty), 203);                                                                            //BenAddress4
                    arrRemittance.SetValue(setDTValue(dtBeneficiaryAddress, "Address5", string.Empty), 204);                                                                            //BenAddress5
                    arrRemittance.SetValue(setDTValue(dtBeneficiaryAddress, "Address6", string.Empty), 205);                                                                            //BenAddress6

                    //Always increase the array declaration above for each new field is added. e.g. string[] arrRemittance = new string[172];
                    DataTable dt = createTable("SAVEBYSEQID");
                    try
                    {
                        l = 0;
                        string sInsert = string.Empty;
                        sInsert = "INSERT INTO tbl_Remittance_IRSW (";
                        for (l = 0; l <= (dt.Columns.Count - 1); l++)
                        {
                            sLog += dt.Columns[l].ToString() + ", ";
                        }
                        sLog = sLog.Substring(0, sLog.Length - 2); //Remove Ending Comma (,)
                        sLog += ") VALUES (";
                        sInsert += sLog;
                        sLog = string.Empty;
                        for (l = 0; l <= (arrRemittance.Length - 1); l++)
                        {
                            sLog += "'" + arrRemittance.GetValue(l).ToString() + "', ";
                        }
                        sLog = sLog.Substring(0, sLog.Length - 2); //Remove Ending Comma (,)
                        sLog += ")";
                        sInsert += sLog;
                        sLog = sInsert;
                        /*
                        for (l = 0; l <= (arrRemittance.Length - 1); l++)
                        {
                            sLog += dt.Columns[l].ToString() + "=" + arrRemittance.GetValue(l).ToString() + "|";
                        }
                        sLog = sLog.Substring(0, sLog.Length - 1); //Remove Ending Pipe (|)
                        */
                    }
                    catch (Exception ex)
                    {
                        sLog = ex.Message;
                    }
                    base.Message = string.Empty;
                    base.addMode = true;
                    bool ok = false;
                    int db = 0;
                    string sSQL = "SELECT TOP 1 ";

                    for (db = 0; db <= (dt.Columns.Count - 1); db++)
                    {
                        sSQL += dt.Columns[db].ToString().Trim() + ", ";
                    }
                    sSQL = sSQL.Substring(0, sSQL.Length - 2); //Remove Ending Comma (,)
                    sSQL += " FROM tbl_Remittance_IRSW";
                    this.setBase(sSQL, true);

                    for (n = 0; n <= (arrRemittance.Length - 1); n++)
                    {
                        setColumn(dt.Columns[n].ToString(), arrRemittance.GetValue(n).ToString());
                    }
                    ok = base.save(sqlHelperWs.RETAIN_DATASET);
                    if (ok == true)
                    {
                        iReturn = 1;
                    }
                    else
                    {
                        iReturn = -1;
                    }
                    new csAuditLog().writeAudit(_userID, "SAVE REMITTANCE", "SAVE REMITTANCE: [ReturnValue=" + ok.ToString() + "][Message=" + base.Message + "]", "saveRemittanceBySEQID", sLog, csAuditLog.Category.Insert);
                }
            }
            catch (Exception ex)
            {
                iReturn = -1;
                new csAuditLog().writeAudit(_userID, "SAVE REMITTANCE", "ERROR SAVING REMITTANCE: [ErrorMessage=" + ex.Message + "][Message=" + base.Message + "]", "saveRemittanceBySEQID", sLog, csAuditLog.Category.Insert);
            }
            return iReturn;
        }

        public int updateRemittanceBySEQID(long SEQID)
        {
            int iReturn = 0;
            try
            {
                iReturn = base.execStoredProcedureUpdate("csp_updateTransactionBySEQID", new Object[] { SEQID });
                new csAuditLog().writeAudit(_userID, "UPDATE REMITTANCE", "UPDATE REMITTANCE: [ReturnValue=" + iReturn.ToString() + "][SEQ_ID=" + SEQID.ToString() + "]", "csp_updateTransactionBySEQID", string.Empty, csAuditLog.Category.Update);
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit(_userID, "UPDATE REMITTANCE", "ERROR UPDATE REMITTANCE: [ErrorMessage=" + ex.Message + "][SEQ_ID=" + SEQID.ToString() + "]", "csp_updateTransactionBySEQID", string.Empty, csAuditLog.Category.Update);
            }
            return iReturn;
        }

        public int deleteRemittanceBySEQID(long SEQID, string ReferenceNo)
        {
            int iReturn = 0;
            try
            {
                iReturn = base.execStoredProcedureUpdate("csp_deleteTransactionBySEQID", new Object[] { SEQID, ReferenceNo });
                new csAuditLog().writeAudit(_userID, "DELETE REMITTANCE", "DELETE REMITTANCE: [ReturnValue=" + iReturn.ToString() + "][SEQ_ID=" + SEQID.ToString() + "][ReferenceNo=" + ReferenceNo + "]", "deleteRemittanceBySEQID", string.Empty, csAuditLog.Category.Delete);
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit(_userID, "DELETE REMITTANCE", "ERROR DELETE REMITTANCE: [ErrorMessage=" + ex.Message + "][SEQ_ID=" + SEQID.ToString() + "][ReferenceNo=" + ReferenceNo + "]", "deleteRemittanceBySEQID", string.Empty, csAuditLog.Category.Delete);
            }
            return iReturn;
        }

        public int fixRemittanceChargesBySEQID(long SEQID)
        {
            int iReturn = 0;
            try
            {
                iReturn = base.execStoredProcedureUpdate("csp_fixRemittanceWithNoCharges", new Object[] { SEQID });
                new csAuditLog().writeAudit(_userID, "FIX REMITTANCE CHARGES", "FIX REMITTANCE CHARGES: [ReturnValue=" + iReturn.ToString() + "][SEQ_ID=" + SEQID.ToString() + "]", "fixRemittanceChargesBySEQID", string.Empty, csAuditLog.Category.Insert);
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit(_userID, "FIX REMITTANCE CHARGES", "ERROR FIX REMITTANCE CHARGES: [ErrorMessage=" + ex.Message + "][SEQ_ID=" + SEQID.ToString() + "]", "fixRemittanceChargesBySEQID", string.Empty, csAuditLog.Category.Insert);
            }
            return iReturn;
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
        #endregion
    }
}