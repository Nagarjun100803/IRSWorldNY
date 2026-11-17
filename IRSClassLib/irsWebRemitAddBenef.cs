using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;

namespace IRSClassLib
{
    [Serializable]
    public class irsWebRemitAddBenef : sqlHelperWs
    {
        public irsWebRemitAddBenef()
        {
            //REMOVE THIS:
            //base.IsWinApp = true;
        }

        public DataTable getDynamicValidation(string BranchCode, string CIFClass)
        {
            base.EnforceConstraint = false;
            getUpdateDS("wsp_getDynamicValidation", new Object[] { BranchCode, CIFClass });
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

        public DataTable getHitlist(string Lastname, string Firstname, string Middlename, string BranchCode, string Phonetics1, string Phonetics2, string Phonetics3, string Phonetics4, string Phonetics5, string Phonetics6, string Phonetics7, string Phonetics8)
        {
            base.EnforceConstraint = false;
            getUpdateDS("sp_check_and_get_cif_hitlist_validations", new Object[] { Lastname, Firstname, Middlename, BranchCode, Phonetics1, Phonetics2, Phonetics3, Phonetics4, Phonetics5, Phonetics6, Phonetics7, Phonetics8 });
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

        public DataTable getZipcode()
        { 
            base.EnforceConstraint = false;
            getUpdateDS("sp_get_zipcode", new Object[] { });
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

        public string getCIFID(string CIFGUID)
        {
            base.EnforceConstraint = false;
            getUpdateDS("wsp_getCIFID", new Object[] { CIFGUID });
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

        public string getCIFBeneSEQID(string RemitterID, string BeneficiaryID, bool is_add)
        {
            base.EnforceConstraint = false;
            getUpdateDS("wsp_getCIFBeneSEQID2", new Object[] { RemitterID, BeneficiaryID, is_add });
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

        public DataTable getBeneInfo(string CIFID)
        {
            base.EnforceConstraint = false;
            getUpdateDS("wsp_getBeneInfo2", new Object[] { CIFID, ClientData.Location });
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

        public string getRemitterName(string CIFID)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getRemitterName", new Object[] { CIFID });
            if (hasRows)
            {
                selectedRow = 0;
                return table.Rows[0]["RemitterName"].ToString().ToUpper().Trim();
            }
            else
            {
                return null;
            }
        }

        public string getEmailAddres(string RemitterID)
        { 
            base.EnforceConstraint = false;
            getUpdateDS("wsp_getRemitterEmail", new Object[] { RemitterID });
            if (hasRows)
            {
                selectedRow = 0;
                return table.Rows[0]["EmailAddress"].ToString().Trim();
            }
            else
            {
                return null;
            }
        }

        public int getAge(DateTime BirthDate)
        {
            DateTime n = DateTime.Now; // To avoid a race condition around midnight
            int age = n.Year - BirthDate.Year;

            if (n.Month < BirthDate.Month || (n.Month == BirthDate.Month && n.Day < BirthDate.Day))
                age--;
            return age;
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

        public DataTable getExistingBene(string RemitterID, string Lastname, string Firstname, string Middlename, string BranchCode, string BirthDate)
        {
            DataSet ds = getDataSet("usp_getBeneficiary", true, new Object[] { RemitterID, Lastname, Firstname, Middlename, BranchCode, BirthDate }, new String[] { "tbl_cif_master" });
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        public string saveBeneficiary(string RemitterID, string CIFID, string Lastname, string Firstname, string Middlename, string BranchCode, string BirthDate, string UserID, string Address1, string Address2, string ZipCodeSEQID, string TelNo, string EmailAddress, bool IsAdd, string AddressSEQID, string ContactSEQID, String mobileSEQID, String mobileno)
        {
            string sGUID = Guid.NewGuid().ToString();
            string sCIFID = string.Empty;
            string sCIFBeneSEQID = string.Empty;
            bool ok = false;
            string sReturn = string.Empty;

            bool save_isadd = IsAdd;
            if (IsAdd)
            {
                DataTable bt = getExistingBene(RemitterID, Lastname, Firstname, Middlename, BranchCode, BirthDate);
                if (bt != null)
                {
                    CIFID = Convert.ToString(bt.Rows[0]["CIFID"]);
                    try { AddressSEQID = Convert.ToString(bt.Rows[0]["AddressSEQID"]); } catch {}
                    try { ContactSEQID = Convert.ToString(bt.Rows[0]["ContactSEQID"]); } catch { }
                    try { mobileSEQID = Convert.ToString(bt.Rows[0]["MobileSEQID"]); } catch { }
                    IsAdd = false;
                }
            }

            try
            {
                if (IsAdd == false && CIFID == string.Empty)
                {
                    return "NOT OK|Failed to get CIFID from Session";
                }
                DataTable dtDefaults = new irsInternetPage().getDefaults(BranchCode);
                if (dtDefaults == null || dtDefaults.Rows.Count == 0)
                {
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
                if (base.addMode)
                {
                    this.setBase("SELECT TOP 1 * FROM tbl_CIF_Master", IsAdd);
                    //SAVE TO TBL_CIF_MASTER
                    setColumn("LastName", Lastname);
                    setColumn("FirstName", Firstname);
                    setColumn("MiddleName", Middlename);
                    setColumn("FullName", Lastname + ", " + Firstname + " " + Middlename);
                    setColumn("BirthDate", BirthDate);
                    setColumn("Age", getAge(Convert.ToDateTime(BirthDate)));
                    setColumn("BranchCode", BranchCode);
                    setColumn("CIFClass", 2);
                    setColumn("CIFType", 1);
                    setColumn("CIFGUID", sGUID);
                    setColumn("isWebRemitter", true);
                    setColumn("Remarks", "Web Remittance Enrollment - Beneficiary");
                    setColumn("CreatedBy", UserID);
                    setColumn("DateCreated", DateTime.Now.ToShortDateString());
                    ok = base.save(sqlHelperWs.RETAIN_DATASET);
                }
                else
                {
                    this.setBase("SELECT TOP 1 * FROM tbl_CIF_Master WHERE CIFID = '" + CIFID + "'", IsAdd);
                    setColumn("BirthDate", BirthDate);
                    setColumn("Age", getAge(Convert.ToDateTime(BirthDate)));
                    setColumn("ModifiedBy", UserID);
                    setColumn("DateModified", DateTime.Now.ToShortDateString());
                    ok = base.save(sqlHelperWs.RETAIN_DATASET);
                }
                if (ok == true)
                {
                    ok = false;
                    if (IsAdd)
                    {
                        sCIFID = this.getCIFID(sGUID);
                        ok = this.saveToTable("CIFClass", RemitterID, sCIFID, IsAdd, "", "", "", "", "", dtDefaults, "", "", "", "");
                    }
                    else
                    {
                        //cannot modify cif information... proceed to next table.
                        sCIFID = CIFID;
                        ok = true;
                    }
                    if (ok == true)
                    {
                        ok = false;
                        ok = this.saveToTable("CIFAddress", RemitterID, sCIFID, IsAdd, Address1, Address2, ZipCodeSEQID, "", "", dtDefaults, AddressSEQID, "", "", "");
                        if (ok == true)
                        {
                            ok = false;
                            ok = this.saveToTable("CIFContacts", RemitterID, sCIFID, IsAdd, "", "", "", TelNo, EmailAddress, dtDefaults, "", ContactSEQID, mobileSEQID, mobileno);
                            if (ok == true)
                            {
                                ok = false;
                                IsAdd = save_isadd;
                                ok = this.saveToTable("CIFBeneficiaries", RemitterID, sCIFID, IsAdd, "", "", "", TelNo, EmailAddress, dtDefaults, "", ContactSEQID, "", "");
                                if (ok == true)
                                {
                                    sCIFBeneSEQID = this.getCIFBeneSEQID(RemitterID, sCIFID, IsAdd);
                                    if (sCIFBeneSEQID != string.Empty)
                                    {
                                        sReturn = "OK|" + sCIFID + "|" + sCIFBeneSEQID;
                                    }
                                    else
                                    {
                                        sReturn = "NOT OK|Error while retrieving CIF information (CIF BENEFICIARIES TABLE) / " + Message;
                                    }
                                }
                                else
                                {
                                    sReturn = "NOT OK|Error while saving CIF information (CIF BENEFICIARIES TABLE) / " + Message;
                                }
                            }
                            else
                            {
                                sReturn = "NOT OK|Error while saving CIF information (CONTACTS TABLE) / " + Message;
                            }
                        }
                        else
                        {
                            sReturn = "NOT OK|Error while saving CIF information (ADDRESS TABLE) / " + Message;
                        }
                    }
                    else
                    {
                        sReturn = "NOT OK|Error while saving CIF information (CLASS TABLE) / " + Message;
                    }
                }
                else
                {
                    sReturn = "NOT OK|Error while saving CIF information (MASTER TABLE) / " + Message;
                }
                return sReturn;
            }
            catch (Exception ex)
            {
                return "NOT OK|Exception found: " + ex.Message;
            }
        }

        private bool saveToTable(string TableName, string RemitterID, string CIFID, bool IsAdd, string Address1, string Address2, string ZipCodeSEQID, string TelNo, string EmailAddress, DataTable dt, string AddressSEQID, string ContactSEQID, String mobileSEQID, String mobileno)
        {
            bool ok = false;
            switch (TableName)
            { 
                case "CIFClass":
                    if (IsAdd)
                    {
                        this.setBase("SELECT TOP 1 * FROM tbl_CIF_Class", IsAdd);
                        setColumn("CIFID", CIFID);
                        setColumn("CIFClass", 2);
                        setColumn("Deleted", false);
                        ok = base.save(sqlHelperWs.RETAIN_DATASET);
                    }
                    else
                    {
                        ok = true;
                    }
                    break;

                case "CIFAddress":
                    if (IsAdd)
                    {
                        this.setBase("SELECT TOP 1 * FROM tbl_CIF_Address", IsAdd);
                        setColumn("CIFID", CIFID);
                        setColumn("Address1", Address1);
                        setColumn("Address2", Address2);
                        setColumn("Deleted", false);
                        setColumn("ZipCodeSEQID", ZipCodeSEQID);
                        setColumn("PreferredMailAdd", true);
                        setColumn("CIFClass", 2);
                        setColumn("AddressType", dt.Rows[0]["PreferredMailingAddress"].ToString().Trim());
                        ok = base.save(sqlHelperWs.RETAIN_DATASET);
                    }
                    else
                    {
                        this.setBase("SELECT TOP 1 * FROM tbl_CIF_Address WHERE SEQ_ID = '" + AddressSEQID + "'", IsAdd);
                        setColumn("Address1", Address1);
                        setColumn("Address2", Address2);
                        setColumn("Deleted", false);
                        setColumn("ZipCodeSEQID", ZipCodeSEQID);
                        setColumn("PreferredMailAdd", true);
                        setColumn("CIFClass", 2);
                        setColumn("AddressType", dt.Rows[0]["PreferredMailingAddress"].ToString().Trim());
                        ok = base.save(sqlHelperWs.RETAIN_DATASET);
                    }
                    break;

                case "CIFContacts":
                    short mct = 7;
                    Object oct = execScalar("SELECT FldValue FROM tbl_Lookup WHERE FldGroup = 'ContactInfoType' AND FldDescription like 'Mobile%'");
                    if (oct != null && oct.GetType() != typeof(localhost.DBNull))
                    {
                        try
                        {
                            mct = Convert.ToInt16(oct);
                        }
                        catch { }
                    }
                    if (IsAdd)
                    {
                        this.setBase("SELECT TOP 1 * FROM tbl_CIF_Contacts", IsAdd);
                        setColumn("CIFID", CIFID);
                        setColumn("ContactType", dt.Rows[0]["ContactType"].ToString().Trim());
                        setColumn("ContactNo", TelNo);
                        setColumn("Deleted", false);
                        setColumn("Remarks", "Web Remittance Enrollment - Contacts");
                        setColumn("CIFClass", 2);
                        setColumn("EmailAddress", EmailAddress);
                        setColumn("PreferredNo", true);
                        ok = base.save(sqlHelperWs.RETAIN_DATASET);
                        if (ok & (mobileno != null && mobileno.Length > 0))
                        {
                            base.newRow(sqlHelperWs.RETAIN_DATASET);
                            setColumn("CIFID", CIFID);
                            setColumn("ContactType", mct);
                            setColumn("ContactNo", mobileno);
                            setColumn("Deleted", false);
                            setColumn("Remarks", "Web Remittance Enrollment - Mobile Number");
                            setColumn("CIFClass", 2);
                            setColumn("EmailAddress", EmailAddress);
                            setColumn("PreferredNo", false);
                            ok = base.save(sqlHelperWs.RETAIN_DATASET);
                        }
                    }
                    else
                    {
                        this.setBase("SELECT TOP 1 * FROM tbl_CIF_Contacts WHERE SEQ_ID = '" + ContactSEQID + "'", IsAdd);
                        setColumn("ContactType", dt.Rows[0]["ContactType"].ToString().Trim());
                        setColumn("ContactNo", TelNo);
                        setColumn("Deleted", false);
                        setColumn("Remarks", "Web Remittance Enrollment - Contacts");
                        setColumn("CIFClass", 2);
                        setColumn("EmailAddress", EmailAddress);
                        setColumn("PreferredNo", true);
                        ok = base.save(sqlHelperWs.RETAIN_DATASET);

                        if (ok & (mobileno != null && mobileno.Length > 0))
                        {
                            String sql = "SELECT TOP 1 * FROM tbl_CIF_Contacts";
                            if (mobileSEQID != null && mobileSEQID.Length > 0)
                            {
                                sql += " WHERE SEQ_ID = " + mobileSEQID;
                            }

                            this.setBase(sql, (mobileSEQID == null || mobileSEQID.Length == 0));
                            if (mobileSEQID == null || mobileSEQID.Length == 0)
                            {
                                setColumn("CIFID", CIFID);
                            }
                            setColumn("ContactType", mct);
                            setColumn("ContactNo", mobileno);
                            setColumn("Deleted", false);
                            setColumn("Remarks", "Web Remittance Enrollment - Mobile Number");
                            setColumn("CIFClass", 2);
                            setColumn("EmailAddress", EmailAddress);
                            setColumn("PreferredNo", false);
                            ok = base.save(sqlHelperWs.RETAIN_DATASET);
                        }
                    }
                    break;

                case "CIFBeneficiaries":
                    if (IsAdd)
                    {
                        this.setBase("SELECT TOP 1 * FROM tbl_CIF_Beneficiaries", IsAdd);
                        setColumn("CIFID", CIFID);
                        setColumn("RemitterCIFID", RemitterID);
                        setColumn("Relationship", dt.Rows[0]["Relationship"].ToString().Trim());
                        setColumn("NotificationMode", dt.Rows[0]["NotificationMode"].ToString().Trim());
                        setColumn("Deleted", false);
                        ok = base.save(sqlHelperWs.RETAIN_DATASET);
                    }
                    else
                    {
                        ok = true;
                    }
                    break;
            }
            return ok;
        }

        public int saveToHitlist(string CIFID, string BranchCode, string Phonetics1, string Phonetics2, string Phonetics3, string Phonetics4, string Phonetics5, string Phonetics6, string Phonetics7, string Phonetics8)
        {
            return execStoredProcedureUpdate("sp_save_cif_hitlist_validations", new Object[] { CIFID, Phonetics1, Phonetics2, Phonetics3, Phonetics4, Phonetics5, Phonetics6, Phonetics7, Phonetics8, BranchCode } );
        }
    }
}
