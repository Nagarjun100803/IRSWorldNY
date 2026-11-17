using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace IRSClassLib
{
    [Serializable]
    public class irsVIP : sqlHelperWs
    {
        public irsVIP()
        {
            //base.IsWinApp = true;
        }

        public enum ConnectionStringField
        {
            UserID,
            Password,
            Server,
            Database
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

        public DataTable searchCIF(string BranchCode, string CIFID, string LastName, string FirstName, string MiddleName, string SEQ_ID)
        {
            base.EnforceConstraint = false;
            getUpdateDS("vsp_searchCIF", new Object[] { BranchCode, CIFID, LastName, FirstName, MiddleName, SEQ_ID });
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

        public bool saveVIP(string BranchCode, string UserID, string CIFID, string LastName, string FirstName, string MiddleName, string MobileNo, string vOrganization, string Organization, string HKID, string ApplicationDate, string ExpiryDate)
        {
            bool ok = false;
            string RemitterName = LastName + ", " + FirstName + " " + MiddleName;

            base.addMode = true;
            this.setBase("SELECT TOP 1 * FROM tbl_VIP", true);
            setColumn("CIFID", CIFID);
            setColumn("LastName", LastName);
            setColumn("FirstName", FirstName);
            setColumn("MiddleName", MiddleName);
            setColumn("RemitterName", RemitterName);
            if (CIFID != string.Empty)
            {
                setColumn("isLinkToCIF", true);
            }
            else
            {
                setColumn("isLinkToCIF", false);
            }
            setColumn("BranchCode", BranchCode);
            setColumn("MobileNo", MobileNo);
            setColumn("vOrganization", vOrganization);
            setColumn("Organization", Organization);
            setColumn("HKID", HKID);
            setColumn("CreatedBy", UserID);
            setColumn("DateCreated", DateTime.Now.ToString());
            setColumn("Deleted", false);
            setColumn("Printed", false);
            setColumn("ApplicationDate", ApplicationDate);
            setColumn("ExpiryDate", ExpiryDate);
            ok = base.save(sqlHelperWs.RETAIN_DATASET);
            return ok;
        }

        public bool updateVIP(string UserID, string SEQ_ID, string CIFID, string MobileNo, string vOrganization, string Organization, string HKID, string isPrinted, string ApplicationDate, string ExpiryDate)
        {
            bool ok = false;

            base.addMode = false;
            this.setBase("SELECT TOP 1 * FROM tbl_VIP WHERE SEQ_ID = " + SEQ_ID, false);
            setColumn("CIFID", CIFID);
            setColumn("MobileNo", MobileNo);
            setColumn("vOrganization", vOrganization);
            setColumn("Organization", Organization);
            setColumn("HKID", HKID);
            setColumn("ModifiedBy", UserID);
            setColumn("DateModified", DateTime.Now.ToString());
            if (isPrinted == "1")
            {
                setColumn("Printed", true);
            }
            else
            {
                setColumn("Printed", false);
            }
            setColumn("ApplicationDate", ApplicationDate);
            setColumn("ExpiryDate", ExpiryDate);
            ok = base.save(sqlHelperWs.RETAIN_DATASET);
            return ok;
        }

        public bool deleteVIP(string UserID, string SEQ_ID)
        {
            bool ok = false;

            base.addMode = false;
            this.setBase("SELECT TOP 1 * FROM tbl_VIP WHERE SEQ_ID = " + SEQ_ID, false);
            setColumn("Deleted", true);
            setColumn("ModifiedBy", UserID);
            setColumn("DateModified", DateTime.Now.ToString());
            ok = base.save(sqlHelperWs.RETAIN_DATASET);
            return ok;
        }

        public bool updatePrinted(string UserID, string SEQ_ID)
        {
            bool ok = false;

            base.addMode = false;
            this.setBase("SELECT TOP 1 * FROM tbl_VIP WHERE SEQ_ID = " + SEQ_ID, false);
            setColumn("Printed", true);
            setColumn("PrintedBy", UserID);
            setColumn("PrintedDate", DateTime.Now.ToString());
            ok = base.save(sqlHelperWs.RETAIN_DATASET);
            return ok;
        }

        public DataTable extractToFile(string BranchCode, string vOrganization, string DateFrom, string DateTo)
        {
            base.EnforceConstraint = false;
            getUpdateDS("vsp_getVIPData", new Object[] { BranchCode, vOrganization, DateFrom, DateTo });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return new DataTable();
            }
        }

        public DataTable getOrganizationWithTransaction(string DateFrom, string DateTo)
        {
            base.EnforceConstraint = false;
            getUpdateDS("vsp_getOrganizationWithTransaction", new Object[] { DateFrom, DateTo });
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

        public DataTable getOrganizationWithTransactionByMembers(string DateFrom, string DateTo)
        {
            base.EnforceConstraint = false;
            getUpdateDS("vsp_getOrganizationWithTransactionByMembers", new Object[] { DateFrom, DateTo });
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

        public DataTable getZeroRemittanceFromMember(string DateFrom, string DateTo)
        {
            base.EnforceConstraint = false;
            getUpdateDS("vsp_getZeroRemittanceFromMember", new Object[] { DateFrom, DateTo });
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

        public DataTable getTransactionPerOrganizationPerQuarter(string Year, string Quarter)
        {
            base.EnforceConstraint = false;
            getUpdateDS("vsp_getTransactionPerOrganizationPerQuarter", new Object[] { Year, Quarter });
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

        public DataTable getRemitterPerOrganizationWithTransactionHavingTwoPerQuarter(string Year, string Quarter)
        {
            base.EnforceConstraint = false;
            getUpdateDS("vsp_getRemitterPerOrganizationWithTransactionHavingTwoPerQuarter", new Object[] { Year, Quarter });
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

        public DataTable getZeroRemittanceFromOrganization(string Year, string Quarter)
        {
            base.EnforceConstraint = false;
            getUpdateDS("vsp_getZeroRemittanceFromMemberPerQuarter", new Object[] { Year, Quarter });
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

        public string getConnectionStringValue(ConnectionStringField cs)
        {
            string[] arrConnectionString = base.ConnectionString.Split(';');
            string[] server = arrConnectionString[0].Split('=');
            string[] database = arrConnectionString[2].Split('=');
            string[] userid = arrConnectionString[3].Split('=');
            string[] password = arrConnectionString[4].Split('=');
            string sReturn = string.Empty;

            switch (cs)
            { 
                case ConnectionStringField.UserID:
                    sReturn = userid.GetValue(1).ToString();
                    break;

                case ConnectionStringField.Password:
                    sReturn = password.GetValue(1).ToString();
                    break;

                case ConnectionStringField.Server:
                    sReturn = server.GetValue(1).ToString();
                    break;

                case ConnectionStringField.Database:
                    sReturn = database.GetValue(1).ToString();
                    break;
            }
            return sReturn;
        }
    }
}
