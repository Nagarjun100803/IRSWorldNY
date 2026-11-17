using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using System.Net;
using System.Net.Mail;
using System.Data.SqlClient;

namespace IRSClassLib
{
    [Serializable]
    public class irsWebRemittance : sqlHelperWs
    {
        public irsWebRemittance()
        {
            //REMOVE THIS:
            //base.IsWinApp = true;
        }

        public DataTable getModeOfRemittance(string Value, string BranchCode)
        {
            base.EnforceConstraint = false;
            getUpdateDS("sp_internet_dropdown_products", new Object[] { Value, BranchCode });
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

        public DataTable getTargetCurrency(string ProductCode, string Role)
        {
            base.EnforceConstraint = false;
            getUpdateDS("sp_internet_dropdown_filtered_currency", new Object[] { ProductCode, Role });
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

        public DataTable getPNBBranchCode()
        {
            base.EnforceConstraint = false;
            getUpdateDS("wsp_getPNBBank", new Object[] { });
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

        public DataTable getLookupFldGroup(string FldGroup, string BranchCode, string Role)
        {
            base.EnforceConstraint = false;
            getUpdateDS("sp_internet_dropdown", new Object[] { FldGroup, BranchCode, Role });
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

        public DataTable getOtherBank()
        {
            base.EnforceConstraint = false;
            getUpdateDS("wsp_getDomesticBank", new Object[] { });
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

        public string getAccountSEQID(string CIFID, string AccountNo)
        {
            base.EnforceConstraint = false;
            getUpdateDS("wsp_getAccountSEQID", new Object[] { CIFID, AccountNo });
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

        public string saveModeOfImplementation(string ProductCode, string TargetCurrency, string AccountBranchCode, string AccountType, string AccountNo, string AccountStatus, string BeneficiaryID, string RemitterID, string BeneSEQID, string BranchCode, bool IsAdd)
        {
            string sImplementationType = string.Empty;
            try
            {
                if (ProductCode == "27")
                {
                    sImplementationType = "1";
                }
                else
                { 
                    if ((ProductCode == "10") || (ProductCode == "22"))
                    {
                        sImplementationType = "2";
                    }
                    else
                    {
                        if (ProductCode == "8")
                        {
                            sImplementationType = "3";
                        }
                        else
                        {
                            if (ProductCode == "13")
                            {
                                int i = 0;
                                DataTable dtPDDTS = new irsInternetPage().getLookup("PDDTSBank");
                                bool bFound = false;

                                if (dtPDDTS != null && dtPDDTS.Rows.Count > 0)
                                {
                                    for (i = 0; i <= (dtPDDTS.Rows.Count - 1); i++)
                                    {
                                        if (dtPDDTS.Rows[i]["FldCode"].ToString().Trim() == AccountBranchCode)
                                        {
                                            bFound = true;
                                            ProductCode = "12";
                                            sImplementationType = "4";
                                            break;
                                        }
                                    }
                                    if (bFound == false)
                                    {
                                        ProductCode = "13";
                                        sImplementationType = "5";
                                    }
                                }
                                else
                                {
                                    return "NOT OK|PDDTS Bank Code setup is incomplete (LOOKUP TABLE) / " + Message;
                                }
                            }
                        }
                    }
                }

                DataTable dtDefaults = new irsInternetPage().getDefaults(BranchCode);
                bool ok = false;
                string sAccountSEQID = string.Empty;

                if (dtDefaults == null || dtDefaults.Rows.Count == 0)
                {
                    new csAuditLog().writeAudit("", "Mode of Remittance", "Error while retrieving branch setup (DEFAULTS TABLE)", "", "", csAuditLog.Category.Insert);
                    return "NOT OK|Error while retrieving branch setup (DEFAULTS TABLE) / " + Message;
                }
                if (IsAdd == true)
                {
                    base.addMode = true;
                }
                else
                {
                    base.addMode = false;
                }
                if (AccountNo != string.Empty)
                {
                    ok = this.saveToTable("CIFAccounts", "", BeneficiaryID, "", dtDefaults, "", sImplementationType, AccountNo, AccountType, TargetCurrency, AccountBranchCode, AccountStatus, "", IsAdd);
                    if (ok == true)
                    {
                        ok = false;
                        sAccountSEQID = this.getAccountSEQID(BeneficiaryID, AccountNo);

                        if (sAccountSEQID != string.Empty)
                        {
                            ok = this.saveToTable("CIFBeneficiaries", RemitterID, BeneficiaryID, BeneSEQID, dtDefaults, ProductCode, sImplementationType, "", "", TargetCurrency, AccountBranchCode, "", sAccountSEQID, IsAdd);
                            if (ok == true)
                            {
                                new csAuditLog().writeAudit("", "Mode of Remittance", "Successfully saved to CIF info in CIF Beneficiaries table", "", "", csAuditLog.Category.Insert);
                                return "OK|Successfully saved";
                            }
                            else
                            {
                                new csAuditLog().writeAudit("", "Mode of Remittance", "Failed to saved CIF info in CIF Beneficiaries table", "", "", csAuditLog.Category.Insert);
                                return "NOT OK|Failed to save beneficiary info (CIF BENEFICIARIES TABLE) / " + Message;
                            }
                        }
                        else
                        {
                            new csAuditLog().writeAudit("", "Mode of Remittance", "Failed to retrieve account no in CIF Accounts table", "", "", csAuditLog.Category.Insert);
                            return "NOT OK|Failed to retrieve account no (CIF ACCOUNTS TABLE) / " + Message;
                        }
                    }
                    else
                    {
                        new csAuditLog().writeAudit("", "Mode of Remittance", "Failed to saved accounts info in CIF Accounts table", "", "", csAuditLog.Category.Insert);
                        return "NOT OK|Failed to save account info (CIF ACCOUNTS TABLE) / " + Message;
                    }
                }
                else
                {
                    ok = this.saveToTable("CIFBeneficiaries", RemitterID, BeneficiaryID, BeneSEQID, dtDefaults, ProductCode, sImplementationType, "", "", TargetCurrency, AccountBranchCode, "", sAccountSEQID, IsAdd);
                    if (ok == true)
                    {
                        new csAuditLog().writeAudit("", "Mode of Remittance", "Successfully saved CIF info in CIF Beneficiaries table", "", "", csAuditLog.Category.Insert);
                        return "OK|Successfully saved";
                    }
                    else
                    {
                        new csAuditLog().writeAudit("", "Mode of Remittance", "Failed to saved CIF info in CIF Beneficiaries table", "", "", csAuditLog.Category.Insert);
                        return "NOT OK|Failed to save beneficiary info (CIF BENEFICIARIES TABLE) / " + Message;
                    }
                }
            }
            catch (Exception e)
            {
                return "NOT OK|" + e.Message;
            }
            return string.Empty;
        }

        private bool saveToTable(string TableName, string RemitterID, string CIFID, string BeneSEQID, DataTable dt,
            string ProductCode, string ImplementationType, string AccountNo, string AccountType, 
            string Currency, string BranchCode, string AccountStatus, string AccountSEQID, bool IsAdd)
        {
            bool ok = false;
            switch (TableName)
            {
                case "CIFBeneficiaries":
                    if (IsAdd)
                    {
                        this.setBase("SELECT TOP 1 * FROM tbl_CIF_Beneficiaries", IsAdd);
                        setColumn("CIFID", CIFID);
                        setColumn("RemitterCIFID", RemitterID);
                        setColumn("Relationship", dt.Rows[0]["Relationship"].ToString().Trim());
                        setColumn("NotificationMode", dt.Rows[0]["NotificationMode"].ToString().Trim());
                        setColumn("ProductCode", ProductCode);
                        setColumn("ImplementationCode", ImplementationType);
                        setColumn("AccountSEQID", AccountSEQID);
                        setColumn("Currency", Currency);
                        setColumn("BranchCode", BranchCode);
                        setColumn("AccountType", AccountType);
                        setColumn("Deleted", false);
                        ok = base.save(sqlHelperWs.RETAIN_DATASET);
                    }
                    else 
                    {
                        this.setBase("SELECT TOP 1 * FROM tbl_CIF_Beneficiaries WHERE SEQ_ID = '" + BeneSEQID + "'", IsAdd);
                        setColumn("ProductCode", ProductCode);
                        setColumn("ImplementationCode", ImplementationType);
                        setColumn("AccountSEQID", AccountSEQID);
                        setColumn("Currency", Currency);
                        setColumn("BranchCode", BranchCode);
                        setColumn("AccountType", AccountType);
                        ok = base.save(sqlHelperWs.RETAIN_DATASET);
                    }
                    break;

                case "CIFAccounts":
                    this.setBase("SELECT TOP 1 * FROM tbl_CIF_Accounts", IsAdd);
                    setColumn("CIFID", CIFID);
                    setColumn("AccountNo", AccountNo);
                    setColumn("AccountType", AccountType);
                    setColumn("Currency", Currency);
                    setColumn("Remarks", "Web Remittance Enrollment - Mode of Remittance");
                    setColumn("Status", AccountStatus);
                    setColumn("Deleted", false);

                    if (ImplementationType == "4" || ImplementationType == "5")
                    {
                        setColumn("DomBankCode", BranchCode);
                    }
                    else
                    {
                        setColumn("BranchCode", BranchCode);
                    }
                    ok = base.save(sqlHelperWs.RETAIN_DATASET);
                    break;
            }
            return ok;
        }

         private void setBase(string SQL, bool IsAdd)
        {
            base.getUpdateDataSet(getSqlCommand(SQL, false));
            if (!base.hasRows)
            {
                selectedRow = 0;
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

        public DataTable getTodayForex(string BranchCode)
        {
            base.EnforceConstraint = false;
            getUpdateDS("usw_getTodayForex", new Object[] { BranchCode });
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

        public DataTable getviewmode(string remcifid, string cifid)
        {
            base.EnforceConstraint = false;
            getUpdateDS("usp_view_moderem", new Object[] { remcifid, cifid });
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

        public DataTable getRemitterBeneficiary(string RemitterID)
        {
            base.EnforceConstraint = false;
            getUpdateDS("usp_view_mybene", new Object[] { RemitterID });
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

        public Boolean  delCIF(int SeqID)
        {
            base.EnforceConstraint = false;
            SqlCommand  cmd = base.getSqlCommand( "usp_del_cif",true );
            cmd.Parameters.Add("@seqid", SqlDbType.BigInt).Value = SeqID;
            int ra = base.sqlUpdate(cmd);            
            if (ra > 0)
            {                
                return true;
            }
            else
            {
                return false;
            }
        }
      
    }
}
