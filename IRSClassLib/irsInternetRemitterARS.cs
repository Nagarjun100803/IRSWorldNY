using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using IRSClassLib;

namespace IRSClassLib
{
    [Serializable]
    public class irsInternetRemitterARS : sqlHelperWs
    {
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

        public void setAddMode(bool isAdd)
        {
            if (isAdd)
            {
                base.addMode = true;
            }
            else
            {
                base.addMode = false;
            }
        }

        #region Fetch Data
        public string getZipCodeValue(Int32 SEQ_ID)
        {
            base.EnforceConstraint = false;
            getUpdateDS("asp_Get_Zipcode", new Object[] { SEQ_ID });
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

        public DataTable getCIFDetails(string REMCIFID, string BENCIFID, string BRANCHCODE, string PRODUCTID)
        {
            base.EnforceConstraint = false;
            getUpdateDS("asp_Retrieve_CIF_Details", new Object[] { REMCIFID, BENCIFID, BRANCHCODE, 0 });
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

        public string getCIFMasterSEQID(string paramCIFID)
        {
            base.EnforceConstraint = false;
            getUpdateDS("SELECT SEQ_ID FROM tbl_cif_master WHERE CIFID = '" + paramCIFID + "'");
            if (hasRows)
            {
                selectedRow = 0;
                return table.Rows[0][0].ToString();
            }
            else
            {
                return null;
            }
        }

        public string getCIFContactsSEQID(string paramCIFID)
        {
            base.EnforceConstraint = false;
            getUpdateDS("SELECT SEQ_ID FROM tbl_cif_contacts WHERE CIFID = '" + paramCIFID + "'");
            if (hasRows)
            {
                selectedRow = 0;
                return table.Rows[0][0].ToString();
            }
            else
            {
                return null;
            }
        }

        public string getCIFAddressSEQID(string paramCIFID, int addressType)
        {
            base.EnforceConstraint = false;
            getUpdateDS("SELECT SEQ_ID FROM tbl_cif_address WHERE CIFID = '" + paramCIFID + "' AND AddressType = '" + addressType + "'");
            if (hasRows)
            {
                selectedRow = 0;
                return table.Rows[0][0].ToString();
            }
            else
            {
                return null;
            }
        }

        public string getCIFIDsSEQID(string paramCIFID)
        {
            base.EnforceConstraint = false;
            getUpdateDS("SELECT SEQ_ID FROM tbl_cif_ids WHERE CIFID = '" + paramCIFID + "'");
            if (hasRows)
            {
                selectedRow = 0;
                return table.Rows[0][0].ToString();
            }
            else
            {
                return null;
            }
        }

        public string getCIFOccupationsSEQID(string paramCIFID)
        {
            base.EnforceConstraint = false;
            getUpdateDS("SELECT SEQ_ID FROM tbl_cif_occupations WHERE CIFID = '" + paramCIFID + "'");
            if (hasRows)
            {
                selectedRow = 0;
                return table.Rows[0][0].ToString();
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

        public DataTable getModeOfRemittance(string Value, string BranchCode)
        {
            base.EnforceConstraint = false;
            getUpdateDS("asp_internet_dropdown_products", new Object[] { Value, BranchCode });
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

        public DataTable getOtherBanks(string CurrencyCode)
        {
            base.EnforceConstraint = false;
            getUpdateDS("asp_getOtherBanks", new Object[] { CurrencyCode });
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
        
        public string getZipCodeRegion(string ZipCode)
        {
           base.EnforceConstraint = false;
           getUpdateDS("asp_getZipCodeRegion", new Object[] { ZipCode });
            if (hasRows)
            {
                selectedRow = 0;
                return Convert.ToString(table.Rows[0][0]);
            }
            else
            {
                return "";
            }
            return "";
        }

        public string getBranchName(string BranchCode)
        {
            base.EnforceConstraint = false;
            getUpdateDS("SELECT BranchName FROM tbl_branches WHERE BranchCode = '" + BranchCode + "'");
            if (hasRows)
            {
                selectedRow = 0;
                return Convert.ToString(table.Rows[0][0]);
            }
            else
            {
                return "";
            }
            return "";
        }

        public DataTable searchRem(string LastName, string FirstName, string MiddleName, string BranchCode)
        {
            base.EnforceConstraint = false;
            getUpdateDS("asp_Search_CIF", new Object[] { LastName, FirstName, MiddleName, BranchCode });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return null;
            }
            return null;
        }

        public DataTable getCIFInformation(string sRemitterCIFID, string sBeneficiaryCIFID, string sProductID, string sBranchCode)
        {
            base.EnforceConstraint = false;
            getUpdateDS("asp_Retrieve_CIF_Details", new Object[] { sRemitterCIFID, sBeneficiaryCIFID, sBranchCode, sProductID });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return null;
            }
            return null;
        }
        #endregion


        public string saveRemitterCIF(
            string[] CIFMasterRemitter, string[] CIFClassRemitter,
            string[] CIFRemitterAddressHome, string[] CIFRemitterAddressBusiness, 
            string[] CIFRemitterContacts, string[] CIFRemitterIDs, 
            string[] CIFRemitterOccupation, string[] CIFRemitter,
            bool IsAdd)
        {
            string sGUID;
            string sRemitterCIFID = string.Empty;
            bool ok = false;
            string sReturn = string.Empty;
            string LastName = string.Empty;
            string FirstName = string.Empty;
            string MiddleName = string.Empty;
            string FullName = string.Empty;
            string SEQ_ID = string.Empty;

            try
            {                
                //REMITTER MASTER TABLE
                sGUID = Guid.NewGuid().ToString();
                if (CIFMasterRemitter.Length > 0)
                {
                    this.setAddMode(IsAdd);
                    if (base.addMode)
                    {
                        LastName = CIFMasterRemitter.GetValue(1).ToString().Trim();
                        FirstName = CIFMasterRemitter.GetValue(2).ToString().Trim();
                        MiddleName = CIFMasterRemitter.GetValue(3).ToString().Trim();
                        FullName = LastName + ", " + FirstName + " " + MiddleName;

                        this.setBase("SELECT TOP 1 * FROM tbl_CIF_Master", IsAdd); //SAVE TO TBL_CIF_MASTER
                        setColumn("CIFType", CIFMasterRemitter.GetValue(0).ToString().Trim());
                        setColumn("LastName", LastName);
                        setColumn("FirstName", FirstName);
                        setColumn("MiddleName", MiddleName);
                        setColumn("Citizenship", CIFMasterRemitter.GetValue(4).ToString().Trim());
                        setColumn("BirthDate", CIFMasterRemitter.GetValue(5).ToString().Trim());
                        setColumn("BirthPlace", CIFMasterRemitter.GetValue(6).ToString().Trim());
                        setColumn("Remarks", "ARS Remitter Enrollment");
                        setColumn("Deleted", false);
                        setColumn("BranchCode", CIFMasterRemitter.GetValue(7).ToString().Trim());
                        setColumn("FullName", FullName);
                        setColumn("CIFClass", CIFMasterRemitter.GetValue(8).ToString().Trim());
                        setColumn("DateCreated", CIFMasterRemitter.GetValue(9).ToString().Trim());
                        setColumn("CreatedBy", CIFMasterRemitter.GetValue(10).ToString().Trim());
                        setColumn("CIFGUID", sGUID);
                        ok = base.save(sqlHelperWs.RETAIN_DATASET);
                    }
                    else
                    {
                        SEQ_ID = CIFMasterRemitter.GetValue(0).ToString().Trim();
                        LastName = CIFMasterRemitter.GetValue(1).ToString().Trim();
                        FirstName = CIFMasterRemitter.GetValue(2).ToString().Trim();
                        MiddleName = CIFMasterRemitter.GetValue(3).ToString().Trim();
                        FullName = LastName + ", " + FirstName + " " + MiddleName;

                        this.setBase("SELECT TOP 1 * FROM tbl_CIF_Master WHERE SEQ_ID = " + SEQ_ID, IsAdd); //SAVE TO TBL_CIF_MASTER
                        setColumn("LastName", LastName);
                        setColumn("FirstName", FirstName);
                        setColumn("MiddleName", MiddleName);
                        setColumn("FullName", FullName);
                        setColumn("Citizenship", CIFMasterRemitter.GetValue(4).ToString().Trim());
                        setColumn("BirthDate", CIFMasterRemitter.GetValue(5).ToString().Trim());
                        setColumn("BirthPlace", CIFMasterRemitter.GetValue(6).ToString().Trim());
                        setColumn("DateModified", CIFMasterRemitter.GetValue(7).ToString().Trim());
                        setColumn("ModifiedBy", CIFMasterRemitter.GetValue(8).ToString().Trim());
                        ok = base.save(sqlHelperWs.RETAIN_DATASET);
                    }
                }

                //REMITTER CIF CLASS TABLE
                if (ok)
                {
                    if (CIFClassRemitter.Length > 0)
                    {
                        this.setAddMode(IsAdd);
                        if (base.addMode)
                        {
                            sRemitterCIFID = new irsWebRemitAddBenef().getCIFID(sGUID);
                            System.Web.HttpContext.Current.Session["ars_clientRemitterCIFID"] = sRemitterCIFID;
                            if (sRemitterCIFID != string.Empty)
                            {
                                this.setBase("SELECT TOP 1 * FROM tbl_CIF_Class", IsAdd); //SAVE TO TBL_CIF_CLASS
                                setColumn("CIFID", sRemitterCIFID);
                                setColumn("CIFClass", CIFClassRemitter.GetValue(0).ToString().Trim());
                                setColumn("Deleted", false);
                                ok = base.save(sqlHelperWs.RETAIN_DATASET);
                            }
                            else
                            {
                                sReturn = "NOT OK|System was not able to retrieve Remitter CIFID / " + Message;
                            }
                        }
                        else
                        {
                            //no editing for cif class
                            ok = true;
                        }
                    }
                    else
                    {
                        sReturn = "NOT OK|Remitter CIF Class Information is Missing / " + Message;
                    }
                }
                else
                {
                    sReturn = "NOT OK|Error while saving Remitter CIF information (MASTER TABLE)" + Message;
                }

                //REMITTER ADDRESS HOME
                if (ok)
                {
                    if (CIFRemitterAddressHome.Length > 0)
                    {
                        this.setAddMode(IsAdd);
                        if (base.addMode)
                        {
                            this.setBase("SELECT TOP 1 * FROM tbl_CIF_Address", IsAdd); //SAVE TO TBL_CIF_ADDRESS
                            setColumn("CIFID", sRemitterCIFID);
                            setColumn("Address1", CIFRemitterAddressHome.GetValue(0).ToString().Trim());
                            setColumn("Address2", CIFRemitterAddressHome.GetValue(1).ToString().Trim());
                            setColumn("Address3", CIFRemitterAddressHome.GetValue(2).ToString().Trim());
                            setColumn("Address4", CIFRemitterAddressHome.GetValue(3).ToString().Trim());
                            setColumn("Address5", CIFRemitterAddressHome.GetValue(4).ToString().Trim());
                            setColumn("Address6", CIFRemitterAddressHome.GetValue(5).ToString().Trim());
                            setColumn("CountryCode", CIFRemitterAddressHome.GetValue(6).ToString().Trim());
                            setColumn("Deleted", false);
                            setColumn("AddressType", 2);
                            setColumn("Remarks", "ARS Remitter Home Address Enrollment");
                            setColumn("ZipCode", CIFRemitterAddressHome.GetValue(7).ToString().Trim());
                            setColumn("CIFClass", CIFRemitterAddressHome.GetValue(8).ToString().Trim());
                            setColumn("PreferredMailAdd", Convert.ToBoolean(CIFRemitterAddressHome.GetValue(9)));
                            ok = base.save(sqlHelperWs.RETAIN_DATASET);
                        }
                        else
                        {
                            SEQ_ID = CIFRemitterAddressHome.GetValue(0).ToString().Trim();
                            this.setBase("SELECT TOP 1 * FROM tbl_CIF_Address WHERE SEQ_ID = " + SEQ_ID, IsAdd); //SAVE TO TBL_CIF_ADDRESS
                            setColumn("Address1", CIFRemitterAddressHome.GetValue(1).ToString().Trim());
                            setColumn("Address2", CIFRemitterAddressHome.GetValue(2).ToString().Trim());
                            setColumn("Address3", CIFRemitterAddressHome.GetValue(3).ToString().Trim());
                            setColumn("Address4", CIFRemitterAddressHome.GetValue(4).ToString().Trim());
                            setColumn("Address5", CIFRemitterAddressHome.GetValue(5).ToString().Trim());
                            setColumn("Address6", CIFRemitterAddressHome.GetValue(6).ToString().Trim());
                            setColumn("CountryCode", CIFRemitterAddressHome.GetValue(7).ToString().Trim());
                            setColumn("ZipCode", CIFRemitterAddressHome.GetValue(8).ToString().Trim());
                            setColumn("PreferredMailAdd", Convert.ToBoolean(CIFRemitterAddressHome.GetValue(9)));
                            ok = base.save(sqlHelperWs.RETAIN_DATASET);
                        }
                    }
                    else
                    {
                        sReturn = "NOT OK|Remitter Home Address Information is Missing / " + Message;
                    }
                }
                else
                {
                    sReturn = "NOT OK|Error while saving Remitter CIF CLASS information (CLASS TABLE)" + Message;
                }

                //REMITTER ADDRESS BUSINESS
                if (ok)
                {
                    if (CIFRemitterAddressBusiness.Length > 0)
                    {
                        this.setAddMode(IsAdd);
                        if (base.addMode)
                        {
                            this.setBase("SELECT TOP 1 * FROM tbl_CIF_Address", IsAdd); //SAVE TO TBL_CIF_ADDRESS
                            setColumn("CIFID", sRemitterCIFID);
                            setColumn("Address1", CIFRemitterAddressBusiness.GetValue(0).ToString().Trim());
                            setColumn("Address2", CIFRemitterAddressBusiness.GetValue(1).ToString().Trim());
                            setColumn("Address3", CIFRemitterAddressBusiness.GetValue(2).ToString().Trim());
                            setColumn("Address4", CIFRemitterAddressBusiness.GetValue(3).ToString().Trim());
                            setColumn("Address5", CIFRemitterAddressBusiness.GetValue(4).ToString().Trim());
                            setColumn("Address6", CIFRemitterAddressBusiness.GetValue(5).ToString().Trim());
                            setColumn("CountryCode", CIFRemitterAddressBusiness.GetValue(6).ToString().Trim());
                            setColumn("Deleted", false);
                            setColumn("AddressType", 3);
                            setColumn("Remarks", "ARS Remitter Business Address Enrollment");
                            setColumn("ZipCode", CIFRemitterAddressBusiness.GetValue(7).ToString().Trim());
                            setColumn("CIFClass", CIFRemitterAddressBusiness.GetValue(8).ToString().Trim());
                            setColumn("PreferredMailAdd", Convert.ToBoolean(CIFRemitterAddressBusiness.GetValue(9)));
                            ok = base.save(sqlHelperWs.RETAIN_DATASET);
                        }
                        else
                        {
                            SEQ_ID = CIFRemitterAddressBusiness.GetValue(0).ToString().Trim();
                            this.setBase("SELECT TOP 1 * FROM tbl_CIF_Address WHERE SEQ_ID = " + SEQ_ID, IsAdd); //SAVE TO TBL_CIF_ADDRESS
                            setColumn("Address1", CIFRemitterAddressBusiness.GetValue(1).ToString().Trim());
                            setColumn("Address2", CIFRemitterAddressBusiness.GetValue(2).ToString().Trim());
                            setColumn("Address3", CIFRemitterAddressBusiness.GetValue(3).ToString().Trim());
                            setColumn("Address4", CIFRemitterAddressBusiness.GetValue(4).ToString().Trim());
                            setColumn("Address5", CIFRemitterAddressBusiness.GetValue(5).ToString().Trim());
                            setColumn("Address6", CIFRemitterAddressBusiness.GetValue(6).ToString().Trim());
                            setColumn("CountryCode", CIFRemitterAddressBusiness.GetValue(7).ToString().Trim());
                            setColumn("ZipCode", CIFRemitterAddressBusiness.GetValue(8).ToString().Trim());
                            setColumn("PreferredMailAdd", Convert.ToBoolean(CIFRemitterAddressBusiness.GetValue(9)));
                            ok = base.save(sqlHelperWs.RETAIN_DATASET);
                        }
                    }
                    else
                    {
                        sReturn = "NOT OK|Remitter Business Address Information is Missing / " + Message;
                    }
                }
                else
                {
                    sReturn = "NOT OK|Error while saving Remitter Home Address information (ADDRESS TABLE)" + Message;
                }

                //REMITTER CONTACTS
                if (ok)
                {
                    if (CIFRemitterContacts.Length > 0)
                    { 
                        this.setAddMode(IsAdd);
                        if (base.addMode)
                        {
                            this.setBase("SELECT TOP 1 * FROM tbl_CIF_Contacts", IsAdd); //SAVE TO TBL_CIF_CONTACTS
                            setColumn("CIFID", sRemitterCIFID);
                            setColumn("ContactType", 1);
                            setColumn("ContactNo", CIFRemitterContacts.GetValue(0).ToString().Trim());
                            setColumn("Remarks", "ARS Remitter Contact Enrollment");
                            setColumn("EmailAddress", CIFRemitterContacts.GetValue(1).ToString().Trim());
                            setColumn("PreferredNo", true);
                            setColumn("Deleted", false);
                            setColumn("CIFClass", CIFRemitterContacts.GetValue(2).ToString().Trim());
                            ok = base.save(sqlHelperWs.RETAIN_DATASET);
                        }
                        else
                        {
                            SEQ_ID = CIFRemitterContacts.GetValue(0).ToString().Trim();
                            this.setBase("SELECT TOP 1 * FROM tbl_CIF_Contacts WHERE SEQ_ID = " + SEQ_ID, IsAdd); //SAVE TO TBL_CIF_CONTACTS
                            setColumn("ContactNo", CIFRemitterContacts.GetValue(1).ToString().Trim());
                            setColumn("EmailAddress", CIFRemitterContacts.GetValue(2).ToString().Trim());
                            ok = base.save(sqlHelperWs.RETAIN_DATASET);
                        }
                    }
                    else
                    {
                        sReturn = "NOT OK|Remitter Contact Information is Missing / " + Message;
                    }
                }
                else
                {
                    sReturn = "NOT OK|Error while saving Remitter Business Address information (ADDRESS TABLE)" + Message;
                }

                //REMITTER IDS
                if (ok)
                {
                    if (CIFRemitterIDs.Length > 0)
                    { 
                        this.setAddMode(IsAdd);
                        if (base.addMode)
                        {
                            this.setBase("SELECT TOP 1 * FROM tbl_CIF_IDs", IsAdd); //SAVE TO TBL_CIF_IDS
                            setColumn("CIFID", sRemitterCIFID);
                            setColumn("IDType", CIFRemitterIDs.GetValue(0).ToString().Trim());
                            setColumn("IDNo", CIFRemitterIDs.GetValue(1).ToString().Trim());
                            setColumn("ExpiryDate", CIFRemitterIDs.GetValue(2).ToString().Trim());
                            setColumn("Remarks", "ARS Remitter ID Enrollment");
                            setColumn("Deleted", false);
                            setColumn("PlaceOfIssuance", CIFRemitterIDs.GetValue(3).ToString().Trim());
                            ok = base.save(sqlHelperWs.RETAIN_DATASET);
                        }
                        else
                        {
                            SEQ_ID = CIFRemitterIDs.GetValue(0).ToString().Trim();
                            this.setBase("SELECT TOP 1 * FROM tbl_CIF_IDs WHERE SEQ_ID = " + SEQ_ID, IsAdd); //SAVE TO TBL_CIF_IDS
                            //setColumn("ContactNo", CIFRemitterIDs.GetValue(2).ToString().Trim());
                            //setColumn("EmailAddress", CIFRemitterIDs.GetValue(3).ToString().Trim());
                            setColumn("IDType", CIFRemitterIDs.GetValue(1).ToString().Trim());
                            setColumn("IDNo", CIFRemitterIDs.GetValue(2).ToString().Trim());
                            setColumn("ExpiryDate", CIFRemitterIDs.GetValue(3).ToString().Trim());
                            setColumn("PlaceOfIssuance", CIFRemitterIDs.GetValue(4).ToString().Trim());
                            ok = base.save(sqlHelperWs.RETAIN_DATASET);
                        }
                    }
                    else
                    {
                        sReturn = "NOT OK|Remitter ID Information is Missing / " + Message;
                    }
                }
                else
                {
                    sReturn = "NOT OK|Error while saving Remitter Contacts information (CONTACTS TABLE)" + Message;
                }

                //REMITTER OCCUPATION
                if (ok)
                {
                    if (CIFRemitterOccupation.Length > 0)
                    {
                        this.setAddMode(IsAdd);
                        if (base.addMode)
                        {
                            this.setBase("SELECT TOP 1 * FROM tbl_CIF_Occupations", IsAdd); //SAVE TO TBL_CIF_OCCUPATIONS
                            setColumn("CIFID", sRemitterCIFID);
                            setColumn("Occupation", "75"); //OTHERS
                            setColumn("DetailedDesc", CIFRemitterOccupation.GetValue(0).ToString().Trim());
                            ok = base.save(sqlHelperWs.RETAIN_DATASET);
                        }
                        else
                        {
                            SEQ_ID = CIFRemitterOccupation.GetValue(0).ToString().Trim();
                            this.setBase("SELECT TOP 1 * FROM tbl_CIF_Occupations WHERE SEQ_ID = " + SEQ_ID, IsAdd); //SAVE TO TBL_CIF_OCCUPATION
                            setColumn("DetailedDesc", CIFRemitterOccupation.GetValue(1).ToString().Trim());
                            ok = base.save(sqlHelperWs.RETAIN_DATASET);
                        }
                    }
                    else
                    {
                        sReturn = "NOT OK|Remitter Occupation Information is Missing / " + Message;
                    }
                }
                else
                {
                    sReturn = "NOT OK|Error while saving Remitter ID information (ID TABLE)" + Message;
                }
                if (sReturn == string.Empty)
                {
                    sReturn = "OK|" + sRemitterCIFID;
                }
            }
            catch (Exception ex)
            {
                sReturn = "NOT OK|Exception found: " + ex.Message;
            }
            return sReturn;
        }

        public string saveBeneficiaryCIF(
            string[] CIFMasterBeneficiary, string[] CIFClassBeneficiary,
            string[] CIFBeneficiaryAddress, string[] CIFBeneficiaryContacts,
            string[] CIFBeneficiaries, bool IsAdd)
        { 
            string sGUID;
            string sBeneficiaryCIFID = string.Empty;
            bool ok = false;
            string sReturn = string.Empty;
            string LastName = string.Empty;
            string FirstName = string.Empty;
            string MiddleName = string.Empty;
            string FullName = string.Empty;
            string SEQ_ID = string.Empty;

            try
            {
                //BENEFICIARY MASTER TABLE
                sGUID = Guid.NewGuid().ToString();
                if (CIFMasterBeneficiary.Length > 0)
                {
                    this.setAddMode(IsAdd);
                    if (base.addMode)
                    {
                        LastName = CIFMasterBeneficiary.GetValue(1).ToString().Trim();
                        FirstName = CIFMasterBeneficiary.GetValue(2).ToString().Trim();
                        MiddleName = CIFMasterBeneficiary.GetValue(3).ToString().Trim();
                        FullName = LastName + ", " + FirstName + " " + MiddleName;

                        this.setBase("SELECT TOP 1 * FROM tbl_CIF_Master", IsAdd); //SAVE TO TBL_CIF_MASTER
                        setColumn("CIFType", CIFMasterBeneficiary.GetValue(0).ToString().Trim());
                        setColumn("LastName", LastName);
                        setColumn("FirstName", FirstName);
                        setColumn("MiddleName", MiddleName);
                        setColumn("Citizenship", CIFMasterBeneficiary.GetValue(4).ToString().Trim());
                        setColumn("BirthDate", CIFMasterBeneficiary.GetValue(5).ToString().Trim());
                        setColumn("BirthPlace", CIFMasterBeneficiary.GetValue(6).ToString().Trim());
                        setColumn("Remarks", "ARS Beneficiary Enrollment");
                        setColumn("Deleted", false);
                        setColumn("BranchCode", CIFMasterBeneficiary.GetValue(7).ToString().Trim());
                        setColumn("FullName", FullName);
                        setColumn("CIFClass", CIFMasterBeneficiary.GetValue(8).ToString().Trim());
                        setColumn("DateCreated", CIFMasterBeneficiary.GetValue(9).ToString().Trim());
                        setColumn("CreatedBy", CIFMasterBeneficiary.GetValue(10).ToString().Trim());
                        setColumn("CIFGUID", sGUID);
                        ok = base.save(sqlHelperWs.RETAIN_DATASET);
                    }
                    else
                    {
                        SEQ_ID = CIFMasterBeneficiary.GetValue(0).ToString().Trim();
                        LastName = CIFMasterBeneficiary.GetValue(1).ToString().Trim();
                        FirstName = CIFMasterBeneficiary.GetValue(2).ToString().Trim();
                        MiddleName = CIFMasterBeneficiary.GetValue(3).ToString().Trim();
                        FullName = LastName + ", " + FirstName + " " + MiddleName;

                        this.setBase("SELECT TOP 1 * FROM tbl_CIF_Master WHERE SEQ_ID = " + SEQ_ID, IsAdd); //SAVE TO TBL_CIF_MASTER
                        setColumn("LastName", LastName);
                        setColumn("FirstName", FirstName);
                        setColumn("MiddleName", MiddleName);
                        setColumn("Citizenship", CIFMasterBeneficiary.GetValue(4).ToString().Trim());
                        setColumn("BirthDate", CIFMasterBeneficiary.GetValue(5).ToString().Trim());
                        setColumn("BirthPlace", CIFMasterBeneficiary.GetValue(6).ToString().Trim());
                        setColumn("FullName", FullName);
                        setColumn("DateModified", CIFMasterBeneficiary.GetValue(7).ToString().Trim());
                        setColumn("ModifiedBy", CIFMasterBeneficiary.GetValue(8).ToString().Trim());
                        ok = base.save(sqlHelperWs.RETAIN_DATASET);
                    }
                }

                //BENEFICIARY CIF CLASS TABLE
                if (ok)
                {
                    if (CIFClassBeneficiary.Length > 0)
                    {
                        this.setAddMode(IsAdd);
                        if (base.addMode)
                        {
                            sBeneficiaryCIFID = new irsWebRemitAddBenef().getCIFID(sGUID);
                            if (sBeneficiaryCIFID != string.Empty)
                            {
                                this.setBase("SELECT TOP 1 * FROM tbl_CIF_Class", IsAdd); //SAVE TO TBL_CIF_CLASS
                                setColumn("CIFID", sBeneficiaryCIFID);
                                setColumn("CIFClass", CIFClassBeneficiary.GetValue(0).ToString().Trim());
                                setColumn("Deleted", false);
                                ok = base.save(sqlHelperWs.RETAIN_DATASET);
                            }
                            else
                            {
                                sReturn = "NOT OK|System was not able to retrieve Beneficiary CIFID / " + Message;
                            }
                        }
                        else
                        {
                            //no editing for cif class
                            ok = true;
                        }
                    }
                    else
                    {
                        sReturn = "NOT OK|Beneficiary CIF Class Information is Missing / " + Message;
                    }
                }
                else
                {
                    sReturn = "NOT OK|Error while saving Beneficiary CIF information (MASTER TABLE)" + Message;
                }

                //BENEFICIARY ADDRESS HOME
                if (ok)
                {
                    if (CIFBeneficiaryAddress.Length > 0)
                    {
                        this.setAddMode(IsAdd);
                        if (base.addMode)
                        {
                            this.setBase("SELECT TOP 1 * FROM tbl_CIF_Address", IsAdd); //SAVE TO TBL_CIF_ADDRESS
                            setColumn("CIFID", sBeneficiaryCIFID);
                            setColumn("Address1", CIFBeneficiaryAddress.GetValue(0).ToString().Trim());
                            setColumn("Address2", CIFBeneficiaryAddress.GetValue(1).ToString().Trim());
                            setColumn("Address3", CIFBeneficiaryAddress.GetValue(2).ToString().Trim());
                            setColumn("Address4", CIFBeneficiaryAddress.GetValue(3).ToString().Trim());
                            setColumn("Address5", CIFBeneficiaryAddress.GetValue(4).ToString().Trim());
                            setColumn("Address6", CIFBeneficiaryAddress.GetValue(5).ToString().Trim());
                            setColumn("CountryCode", CIFBeneficiaryAddress.GetValue(6).ToString().Trim());
                            setColumn("Deleted", false);
                            setColumn("AddressType", 2);
                            setColumn("Remarks", "ARS Beneficiary Home Address Enrollment");
                            setColumn("ZipCodeSeqId", CIFBeneficiaryAddress.GetValue(7).ToString().Trim());
                            setColumn("CIFClass", CIFBeneficiaryAddress.GetValue(8).ToString().Trim());
                            setColumn("PreferredMailAdd", true);
                            ok = base.save(sqlHelperWs.RETAIN_DATASET);
                        }
                        else
                        {
                            SEQ_ID = CIFBeneficiaryAddress.GetValue(0).ToString().Trim();
                            this.setBase("SELECT TOP 1 * FROM tbl_CIF_Address WHERE SEQ_ID = " + SEQ_ID, IsAdd); //SAVE TO TBL_CIF_ADDRESS
                            setColumn("Address1", CIFBeneficiaryAddress.GetValue(1).ToString().Trim());
                            setColumn("Address2", CIFBeneficiaryAddress.GetValue(2).ToString().Trim());
                            setColumn("Address3", CIFBeneficiaryAddress.GetValue(3).ToString().Trim());
                            setColumn("Address4", CIFBeneficiaryAddress.GetValue(4).ToString().Trim());
                            setColumn("Address5", CIFBeneficiaryAddress.GetValue(5).ToString().Trim());
                            setColumn("Address6", CIFBeneficiaryAddress.GetValue(6).ToString().Trim());
                            setColumn("CountryCode", CIFBeneficiaryAddress.GetValue(7).ToString().Trim());
                            setColumn("ZipCodeSeqId", CIFBeneficiaryAddress.GetValue(8).ToString().Trim());
                            ok = base.save(sqlHelperWs.RETAIN_DATASET);
                        }
                    }
                    else
                    {
                        sReturn = "NOT OK|Beneficiary Home Address Information is Missing / " + Message;
                    }
                }
                else
                {
                    sReturn = "NOT OK|Error while saving Beneficiary CIF CLASS information (CLASS TABLE)" + Message;
                }

                //BENEFICIARY CONTACTS
                if (ok)
                {
                    if (CIFBeneficiaryContacts.Length > 0)
                    {
                        this.setAddMode(IsAdd);
                        if (base.addMode)
                        {
                            this.setBase("SELECT TOP 1 * FROM tbl_CIF_Contacts", IsAdd); //SAVE TO TBL_CIF_CONTACTS
                            setColumn("CIFID", sBeneficiaryCIFID);
                            setColumn("ContactType", 2);
                            setColumn("ContactNo", CIFBeneficiaryContacts.GetValue(0).ToString().Trim());
                            setColumn("Remarks", "ARS Beneficiary Contact Enrollment");
                            setColumn("EmailAddress", CIFBeneficiaryContacts.GetValue(1).ToString().Trim());
                            setColumn("PreferredNo", true);
                            setColumn("Deleted", false);
                            setColumn("CIFClass", CIFBeneficiaryContacts.GetValue(2).ToString().Trim());
                            ok = base.save(sqlHelperWs.RETAIN_DATASET);
                        }
                        else
                        {
                            SEQ_ID = CIFBeneficiaryContacts.GetValue(0).ToString().Trim();
                            this.setBase("SELECT TOP 1 * FROM tbl_CIF_Contacts WHERE SEQ_ID = " + SEQ_ID, IsAdd); //SAVE TO TBL_CIF_CONTACTS
                            setColumn("ContactNo", CIFBeneficiaryContacts.GetValue(1).ToString().Trim());
                            setColumn("EmailAddress", CIFBeneficiaryContacts.GetValue(2).ToString().Trim());
                            ok = base.save(sqlHelperWs.RETAIN_DATASET);
                        }
                    }
                    else
                    {
                        sReturn = "NOT OK|Beneficiary Contact Information is Missing / " + Message;
                    }
                }
                else
                {
                    sReturn = "NOT OK|Error while saving Beneficiary Home Address information (ADDRESS TABLE)" + Message;
                }

                if (ok)
                {
                    this.setAddMode(IsAdd);
                    if (base.addMode)
                    {
                        this.setBase("SELECT TOP 1 * FROM tbl_CIF_Beneficiaries", IsAdd);
                        setColumn("CIFID", sBeneficiaryCIFID);
                        setColumn("RemitterCIFID", System.Web.HttpContext.Current.Session["ars_clientRemitterCIFID"].ToString());//sRemitterCIFID);
                        setColumn("Relationship", 25);
                        setColumn("NotificationMode", 3);
                        setColumn("Deleted", false);
                        ok = base.save(sqlHelperWs.RETAIN_DATASET);
                    }
                    else
                    {
                        ok = true;
                    }
                }
                if (sReturn == string.Empty)
                {
                    System.Web.HttpContext.Current.Session["ars_clientRemitterCIFID"] = null;
                    sReturn = "OK|" + sBeneficiaryCIFID;
                }
            }
            catch (Exception ex)
            {
                return "NOT OK|Exception found: " + ex.Message;
            }
            return sReturn;
        }
    }
}