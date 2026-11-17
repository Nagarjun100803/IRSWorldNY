using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using IRSClassLib;
using IRSWorld.TranClass;
using WordsMatching;

namespace IRSWorld.Internet
{
    public partial class InternetRemitterARS : System.Web.UI.Page
    {
        #region Private Variables
        private string parameter_values = string.Empty;
        private String coms_errmsg = "Unable to access Sql Commons Service.";
        //Arrays
        string[] CIFMasterRemitter;
        string[] CIFClassRemitter;
        string[] CIFRemitterAddressHome;
        string[] CIFRemitterAddressBusiness;
        string[] CIFRemitterContacts;
        string[] CIFRemitterIDs;
        string[] CIFRemitterOccupation;
        string[] CIFCards;
        string[] CIFRemitter;
        string[] CIFMasterBeneficiary;
        string[] CIFClassBeneficiary;
        string[] CIFBeneficiaryAddress;
        string[] CIFBeneficiaryContacts;
        string[] CIFBeneficiaries;
        string[] CIFAccounts;

        //Array value holder
        string CIFMasterRemitter_val = "";
        string CIFClassRemitter_val = "";
        string CIFRemitterAddressHome_val = "";
        string CIFRemitterAddressBusiness_val = "";
        string CIFRemitterContacts_val = "";
        string CIFRemitterIDs_val = "";
        string CIFRemitterOccupation_val = "";
        string CIFMasterBeneficiary_val = "";
        string CIFClassBeneficiary_val = "";
        string CIFBeneficiaryAddress_val = "";
        string CIFBeneficiaryContacts_val = "";
        string CIFBeneficiaries_val = "";
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(InternetRemitter));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(InternetRemitterARS));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(RemTranGFMCCommons));
        }

        private irsClientData IRSClientData
        {
            get
            {
                if (Session["IRS_CLIENT_DATA"] != null)
                {
                    return ((irsClientData)Session["IRS_CLIENT_DATA"]);
                }
                else
                {
                    call_errpage();
                    return null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getBranchCode()
        {
            return new Internet.InternetPage().getLocation();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getLookup(string fldgroup)
        {
            return new Internet.InternetPage().getLookup(fldgroup);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getSubLookup(string GroupName)
        {
            return new irsInternetPage().getSubLookup(getBranchCode(), GroupName);
        }

        public void call_errpage()
        {
            csControl cs_con = new csControl();
            cs_con.execScript(this.Page, "int_rem", "spawn_errpage('Session Expired')");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void close_session()
        {
            Session.Abandon();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void drop_session()
        {
            Session["MODULE_NAME"] = null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DateTime get_branch_localtime(string brcode)
        {
            DateTime dt = new irsTimeZone().branchLocalTime(brcode);
            return dt;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String get_userid()
        {
            return IRSClientData.UserId;
        }

        private String get_branch_country(String brcode)
        {
            try
            {
                Object o = new irsSqlCommonsWs().execScalar("select Country from tbl_branches where branchcode = '" + brcode + "'");
                short sh = Convert.ToInt16(o);
                return sh.ToString();
            }
            catch { }
            return String.Empty;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string add_bene(string plist)
        {
            string errmsg = null;
            string cifid = null;
            irsClientEnrollment enr_agt = new irsClientEnrollment();

            cifid = enr_agt.enroll_agent(irsClientEnrollment.Function_Name.ADD_BENE, plist, out errmsg);

            if (errmsg != String.Empty && errmsg != null && errmsg != "") return errmsg;
            else return cifid;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string update_bene(string plist)
        {
            string errmsg = null;
            string cifid = null;
            irsClientEnrollment enr_agt = new irsClientEnrollment();

            cifid = enr_agt.enroll_agent(irsClientEnrollment.Function_Name.UPDATE_BENE, plist, out errmsg);

            if (errmsg != String.Empty && errmsg != null && errmsg != "") return errmsg;
            else return cifid;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string Get_Zipcode(string ZipSeqID)
        {
            return new irsInternetRemitterARS().getZipCodeValue(Convert.ToInt32(ZipSeqID));
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable Get_CIF_Details(string RemCIFID, string BenCIFID)
        {
            //Session["ARSWREMITTERID"] = RemitterCIFID;
            //Session["ARSWBENEFICIARYID"] = BeneficiaryCIFID;
            //Session["ARSWPRODUCTID"] = ProductID;
            string productID = "";
            if (Session["ARSWPRODUCTID"] != null && Session["ARSWPRODUCTID"].ToString() != "")
            {
                productID = Session["ARSWPRODUCTID"].ToString();
            }
            DataTable dt;
            dt = new irsInternetRemitterARS().getCIFDetails(RemCIFID, BenCIFID, getBranchCode(), productID);
            //((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).RemitterCIFID = Convert.ToString(dt.Rows[0]["RemitterCIFID"]);
            Session["ars_clientRemitterCIFID"] = Convert.ToString(dt.Rows[0]["RemitterCIFID"]);
            Session["ars_clientBeneficiaryCIFID"] = Convert.ToString(dt.Rows[0]["BenefCIFID"]);

            return dt;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String check_and_get_hitlist(String FirstName, String LastName, String MiddleName)
        {
            try
            {
                return get_hitlist(((FirstName == null) ? "" : FirstName), ((LastName == null) ? "" : LastName), ((MiddleName == null) ? "" : MiddleName), IRSClientData.Location);
            }
            catch { }
            return "";
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String get_hitlist(String FirstName, String LastName, String MiddleName, string branchCode)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            String hitlist = "";
            Session["strNewEnrolledCIFPhonetics1"] = "";
            Session["strNewEnrolledCIFPhonetics2"] = "";
            Session["strNewEnrolledCIFPhonetics3"] = "";
            Session["strNewEnrolledCIFPhonetics4"] = "";
            Session["strNewEnrolledCIFPhonetics5"] = "";
            Session["strNewEnrolledCIFPhonetics6"] = "";
            Session["strNewEnrolledCIFPhonetics13"] = "";
            Session["strNewEnrolledCIFPhonetics14"] = "";

            Session["strFirstName"] = "";
            Session["strLastName"] = "";
            Session["strMiddleName"] = "";
            Session["strPercentageScore"] = "";

            string strNewEnrolledCIFPhonetics1 = "";
            string strNewEnrolledCIFPhonetics2 = "";
            string strNewEnrolledCIFPhonetics3 = "";
            string strNewEnrolledCIFPhonetics4 = "";
            string strNewEnrolledCIFPhonetics5 = "";
            string strNewEnrolledCIFPhonetics6 = "";
            string strNewEnrolledCIFPhonetics13 = "";
            string strNewEnrolledCIFPhonetics14 = "";

            try
            {
                DataSet dsp = commons.execStoredProcedure("proc_get_branch_setup_matching", new Object[] { IRSClientData.Location });
                DataTable dtp = new DataTable();
                dtp = dsp.Tables[0];
                if (dtp.Select("fldcode in ('UWPM', 'UW')").Length > 0)
                {
                    DataSet dsComplianceNames = new DataSet();
                    DataTable dtComplianceNames = new DataTable();
                    irsSqlCommonsWs getDistinctComplianceNames = new irsSqlCommonsWs();

                    string SQLSelect = "SELECT MAX(seq_id) uw_seqid, lastname, firstname, middlename, aliaslastname, aliasfirstname, aliasmiddlename, FileFormat " +
                                         "FROM tbl_unifiedwatchlist " +
                                         "WHERE FileFormat IN ('24', '20', '4') " +
                                         "AND UploadingBranch IN (SELECT SUBSTRING(fldcode, 1, 2) " +
                                                            "FROM   tbl_lookup " +
                                                            "WHERE  fldgroup = 'overseasgroup' " +
                                                            "AND    SUBSTRING(fldcode, 4, 3) = '" + branchCode + "') " +
                                         "GROUP BY	LastName, " +
                                                    "FirstName, " +
                                                    "MiddleName, " +
                                                    "AliasLastName, " +
                                                    "AliasFirstName, " +
                                                    "AliasMiddleName, " +
                                                    "FileFormat " +
                                         "UNION ALL " +
                                        "SELECT MAX(seq_id) uw_seqid, lastname, firstname, middlename, aliaslastname, aliasfirstname, aliasmiddlename, FileFormat " +
                                        "FROM tbl_unifiedwatchlist " +
                                        "WHERE FileFormat = '1' " +
                                        "GROUP BY	LastName, " +
                                                    "FirstName, " +
                                                    "MiddleName, " +
                                                    "AliasLastName, " +
                                                    "AliasFirstName, " +
                                                    "AliasMiddleName, " +
                                                    "FileFormat";

                    dsComplianceNames = getDistinctComplianceNames.getDataSet(getDistinctComplianceNames.getSqlCommand(SQLSelect, false), "ComplianceName");
                    dtComplianceNames = dsComplianceNames.Tables[0];

                    if (dtComplianceNames.Rows.Count != 0)
                    {
                        string uwfirstname = "";
                        string uwlastname = "";
                        string uwmiddlename = "";
                        string v_pscore = "";
                        string uw_seqid = "";

                        irsSqlCommonsWs cmns = new irsSqlCommonsWs();
                        DataSet dsps = cmns.execStoredProcedure("proc_get_branch_setup_percentagescore", new Object[] { IRSClientData.Location });
                        DataTable dtps = new DataTable();

                        dtps = dsps.Tables[0];
                        v_pscore = dtps.Rows[0]["flddescription"].ToString();

                        if (dtComplianceNames.Rows.Count > 0)
                        {
                            DelRecPercentTemp();
                        }

                        for (int i = 0; i < dtComplianceNames.Rows.Count; i++)
                        {
                            uwfirstname = dtComplianceNames.Rows[i]["FirstName"].ToString();
                            uwlastname = dtComplianceNames.Rows[i]["LastName"].ToString();
                            uwmiddlename = dtComplianceNames.Rows[i]["MiddleName"].ToString();
                            uw_seqid = dtComplianceNames.Rows[i]["uw_seqid"].ToString();

                            string uwname = uwfirstname.ToString() + " " + uwmiddlename.ToString() + " " + uwlastname.ToString();
                            string cifname = FirstName.ToString() + " " + MiddleName.ToString() + " " + LastName.ToString();

                            MatchsMaker match = new MatchsMaker(uwname, cifname);
                            float pscore = (match.Score);

                            if (pscore >= Convert.ToInt32(v_pscore))
                            {
                                irsSqlCommonsWs com = new irsSqlCommonsWs();

                                String errmsg = "";
                                insert_pname(uwfirstname.ToString(), uwmiddlename.ToString(), uwlastname.ToString(), out errmsg);

                                Session["strFirstName"] = uwfirstname.ToString();
                                Session["strLastName"] = uwlastname.ToString();
                                Session["strMiddleName"] = uwmiddlename.ToString();
                                Session["strPercentageScore"] = "UWPM";

                                hitlist += "No of Hits" + "\n";
                            }
                        }
                        return hitlist;
                    }
                    return string.Empty;
                }

                else
                {
                    irsMethaphone myMethaphone = new irsMethaphone();
                    string strFirstNamePhonetics = "";
                    string strLastNamePhonetics = "";
                    string strMiddleNamePhonetics = "";

                    strFirstNamePhonetics = myMethaphone.DoubleMethaphone(FirstName);
                    strLastNamePhonetics = myMethaphone.DoubleMethaphone(LastName);
                    strMiddleNamePhonetics = myMethaphone.DoubleMethaphone(MiddleName);

                    strNewEnrolledCIFPhonetics1 = strFirstNamePhonetics + strMiddleNamePhonetics + strLastNamePhonetics;
                    strNewEnrolledCIFPhonetics2 = strFirstNamePhonetics + strLastNamePhonetics + strMiddleNamePhonetics;
                    strNewEnrolledCIFPhonetics3 = strLastNamePhonetics + strFirstNamePhonetics + strMiddleNamePhonetics;
                    strNewEnrolledCIFPhonetics4 = strLastNamePhonetics + strMiddleNamePhonetics + strFirstNamePhonetics;
                    strNewEnrolledCIFPhonetics5 = strMiddleNamePhonetics + strFirstNamePhonetics + strLastNamePhonetics;
                    strNewEnrolledCIFPhonetics6 = strMiddleNamePhonetics + strLastNamePhonetics + strFirstNamePhonetics;
                    strNewEnrolledCIFPhonetics13 = strLastNamePhonetics + strFirstNamePhonetics;
                    strNewEnrolledCIFPhonetics14 = strFirstNamePhonetics + strLastNamePhonetics;

                    Session["strNewEnrolledCIFPhonetics1"] = strNewEnrolledCIFPhonetics1.ToString();
                    Session["strNewEnrolledCIFPhonetics2"] = strNewEnrolledCIFPhonetics2.ToString();
                    Session["strNewEnrolledCIFPhonetics3"] = strNewEnrolledCIFPhonetics3.ToString();
                    Session["strNewEnrolledCIFPhonetics4"] = strNewEnrolledCIFPhonetics4.ToString();
                    Session["strNewEnrolledCIFPhonetics5"] = strNewEnrolledCIFPhonetics5.ToString();
                    Session["strNewEnrolledCIFPhonetics6"] = strNewEnrolledCIFPhonetics6.ToString();
                    Session["strNewEnrolledCIFPhonetics13"] = strNewEnrolledCIFPhonetics13.ToString();
                    Session["strNewEnrolledCIFPhonetics14"] = strNewEnrolledCIFPhonetics14.ToString();
                    if (commons != null)
                    {
                        DataSet ds = commons.execStoredProcedure("sp_check_and_get_cif_hitlist_validations", new Object[] { FirstName, LastName, MiddleName, branchCode, strNewEnrolledCIFPhonetics1, strNewEnrolledCIFPhonetics2, strNewEnrolledCIFPhonetics3, strNewEnrolledCIFPhonetics4, strNewEnrolledCIFPhonetics5, strNewEnrolledCIFPhonetics6, strNewEnrolledCIFPhonetics13, strNewEnrolledCIFPhonetics14 });
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ((ds.Tables[0].Rows.Count > 5) ? 5 : ds.Tables[0].Rows.Count); i++)
                            {
                                hitlist += ds.Tables[0].Rows[i][1].ToString().ToUpper().Trim() + "\n";
                            }

                            if (ds.Tables[0].Rows.Count > 5)
                            {
                                hitlist = "\n" + hitlist + "\nShowing maximum of five(5) possible hits.\nPlease get complete list from your local compliance officer.\n";
                            }

                            return hitlist;
                        }
                    }
                    return "";
                }
            }
            catch
            {
                return "";
            }
            finally
            {
                if (commons != null) { commons = null; }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        private void DelRecPercentTemp()
        {
            string SQLDEL = "exec PROC_DEL_PERCENTAGE_TEMP '" + get_userid().ToString() + "'";
            irsSqlCommonsWs sql = new irsSqlCommonsWs();
            sql.execScalar(SQLDEL);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool insert_pname(String FirstName, String LastName, String MiddleName, out String errmsg)
        {
            errmsg = "";
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            SqlCommand sqcommands = null;

            try
            {
                if (commons != null)
                {
                    int res = commons.execStoredProcedureUpdate("proc_save_pname", new Object[] { FirstName, MiddleName, LastName, get_userid().ToString(), IRSClientData.Location.ToString() });
                    if (res == -1)
                    {
                        throw new Exception(commons.Message);
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
            }
            finally
            {
                if (commons != null) { commons = null; }
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void save_soundexlist(String CIFID, String FirstName, String LastName, String MiddleName, int CIFClass)
        {
            try
            {
                String errmsg = "";
                insert_to_soundex(CIFID, out errmsg);
                insert_newcif(CIFID, FirstName, LastName, MiddleName, out errmsg);
            }
            catch { }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool insert_to_soundex(String CIFID, out String errmsg)
        {
            errmsg = "";
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            SqlCommand sqcommands = null;

            try
            {
                if (commons != null)
                {
                    int res = commons.execStoredProcedureUpdate("sp_save_cif_hitlist_validations", new Object[] { CIFID, Session["strNewEnrolledCIFPhonetics1"], Session["strNewEnrolledCIFPhonetics2"], Session["strNewEnrolledCIFPhonetics3"], Session["strNewEnrolledCIFPhonetics4"], Session["strNewEnrolledCIFPhonetics5"], Session["strNewEnrolledCIFPhonetics6"], Session["strNewEnrolledCIFPhonetics13"], Session["strNewEnrolledCIFPhonetics14"], IRSClientData.Location.ToString() });
                    if (res == -1)
                    {
                        throw new Exception(commons.Message);
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
            }
            finally
            {
                if (commons != null) { commons = null; }
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
            return false;
        }

        public bool insert_newcif(String CIFID, String FirstName, String LastName, String MiddleName, out String errmsg)
        {
            errmsg = "";
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            SqlCommand sqcommands = null;

            try
            {
                if (commons != null)
                {
                    int res = commons.execStoredProcedureUpdate("proc_save_newcif", new Object[] { CIFID, FirstName, LastName, MiddleName, IRSClientData.Location.ToString() });
                    if (res == -1)
                    {
                        throw new Exception(commons.Message);
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
            }
            finally
            {
                if (commons != null) { commons = null; }
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
            return false;
        }




        //bool saveData(string dataList, bool isAdd)
        //{
        //    setData(
        //    return new irsInternetRemitterARS().saveCIF(Convert.ToInt32(Rem_Seq_ID), Convert.ToInt32(Ben_Seq_ID), BranchCode);

        //}

        void setData(string dataList, bool IsAdd)
        {
            #region Private variables
            string createdBy = "";
            #endregion

            try
            {
                #region Set Column Field Values in Array
                if (IsAdd == true)
                {
                    CIFMasterRemitter = new string[] { "R CIF Type", "R Last Name", "R First Name", "R Middle Name", "R Citizenship", "R Date of Birth", "R Place of Birth", "Branch Code", "R CIF Class", "Date Created", "Created By" };
                }
                else
                {
                    CIFMasterRemitter = new string[] { "R CIF Master SEQID", "R Last Name", "R First Name", "R Middle Name", "R Citizenship", "R Date of Birth", "R Place of Birth", "Branch Code", "R CIF Class", "Date Created", "Created By" };
                }

                CIFClassRemitter = new string[] { "R CIF Class" };
                if (IsAdd == true)
                {
                    CIFRemitterAddressHome = new string[] { "R Address1 Home", "R Address2 Home", "R Address3 Home", "R Address4 Home", "R Address5 Home", "R Address6 Home", "R CountryID Home", "R Zip Code Home", "R CIF Class", "R Preferred Residence" };
                }
                else
                {
                    CIFRemitterAddressHome = new string[] { "R Address Home SEQID", "R Address1 Home", "R Address2 Home", "R Address3 Home", "R Address4 Home", "R Address5 Home", "R Address6 Home", "R CountryID Home", "R Zip Code Home", "R Preferred Residence" };
                }

                if (IsAdd == true)
                {
                    CIFRemitterAddressBusiness = new string[] { "R Address1 Mail", "R Address2 Mail", "R Address3 Mail", "R Address4 Mail", "R Address5 Mail", "R Address6 Mail", "R CountryID Mail", "R Zip Code Mail", "R CIF Class", "R Preferred Mailing" };
                }
                else
                {
                    CIFRemitterAddressBusiness = new string[] { "R Address Mail SEQID", "R Address1 Mail", "R Address2 Mail", "R Address3 Mail", "R Address4 Mail", "R Address5 Mail", "R Address6 Mail", "R CountryID Mail", "R Zip Code Mail", "R Preferred Mailing" };
                }
                if (IsAdd == true)
                {
                    CIFRemitterContacts = new string[] { "R Contact No", "R Email", "R CIF Class" };
                }
                else
                {
                    CIFRemitterContacts = new string[] { "R Contact SEQID", "R Contact No", "R Email" };
                }
                if (IsAdd == true)
                {
                    CIFRemitterIDs = new string[] { "R ID Type", "R ID No", "R ID Expiration Date", "R ID Place of Issuance" };
                }
                else
                {
                    CIFRemitterIDs = new string[] { "R ID SEQID", "R ID Type", "R ID No", "R ID Expiration Date", "R ID Place of Issuance" };
                }
                if (IsAdd == true)
                {
                    CIFRemitterOccupation = new string[] { "R Occupation" };
                }
                else
                {
                    CIFRemitterOccupation = new string[] { "R Occupation SEQID", "R Occupation" };
                }
                CIFCards = new string[] { };
                CIFRemitter = new string[] { };
                if (IsAdd == true)
                {
                    CIFMasterBeneficiary = new string[] { "B CIF Type", "B Last Name", "B First Name", "B Middle Name", "B Citizenship", "B Date of Birth", "B Place of Birth", "Branch Code", "B CIF Class", "Date Created", "Created By" };
                }
                else
                {
                    CIFMasterBeneficiary = new string[] { "B CIF Master SEQID", "B Last Name", "B First Name", "B Middle Name", "B Citizenship", "B Date of Birth", "B Place of Birth", "Date Created", "Created By" };
                }
                CIFClassBeneficiary = new string[] { "B CIF Class" };
                if (IsAdd == true)
                {
                    CIFBeneficiaryAddress = new string[] { "B Address1", "B Address2", "B Address3", "B Address4", "B Address5", "B Address6", "B CountryID", "B Zip Code", "B CIF Class" };
                }
                else
                {
                    CIFBeneficiaryAddress = new string[] { "B Address SEQID", "B Address1", "B Address2", "B Address3", "B Address4", "B Address5", "B Address6", "B CountryID", "B Zip Code" };
                }
                if (IsAdd == true)
                {
                    CIFBeneficiaryContacts = new string[] { "B Contact No", "B Email", "B CIF Class" };
                }
                else
                {
                    CIFBeneficiaryContacts = new string[] { "B Contact SEQID", "B Contact No", "B Email" };
                }
                CIFBeneficiaries = new string[] { };
                CIFAccounts = new string[] { };

                #endregion

                string[] pValues = dataList.Split('^');

                #region Populate Array Fields for Saving
                #region Populate Remitter CIF Master Array
                for (int x = 0; x < CIFMasterRemitter.Length; x++)
                {
                    for (int j = 0; j < pValues.Length && x < CIFMasterRemitter.Length; j++)
                    {
                        string[] vals = pValues[j].Split('~');

                        if (CIFMasterRemitter[x].ToUpper() == "CREATED BY")
                        {
                            CIFMasterRemitter_val += get_userid().ToString() + "|";
                            break;
                        }
                        if (CIFMasterRemitter[x].ToUpper() == "DATE CREATED")
                        {
                            CIFMasterRemitter_val += get_branch_localtime(getBranchCode()).ToString() + "|";
                            break;
                        }
                        if (CIFMasterRemitter[x].ToUpper() == "BRANCH CODE")
                        {
                            CIFMasterRemitter_val += getBranchCode() + "|";
                            break;
                        }
                        //if function paremeter == parameter field
                        if (CIFMasterRemitter[x].ToUpper() == vals[0].ToUpper())
                        {
                            CIFMasterRemitter_val += vals[1] + "|";
                            break;
                        }
                    }
                }
                #endregion

                #region Populate Remitter CIF Class
                for (int x = 0; x < CIFClassRemitter.Length; x++)
                {
                    for (int j = 0; j < pValues.Length && x < CIFClassRemitter.Length; j++)
                    {
                        string[] vals = pValues[j].Split('~');
                        //if function paremeter == parameter field
                        if (CIFClassRemitter[x].ToUpper() == vals[0].ToUpper())
                        {
                            CIFClassRemitter_val += vals[1] + "|";
                            break;
                        }
                    }
                }
                #endregion

                #region Populate Remitter CIF Address Home
                for (int x = 0; x < CIFRemitterAddressHome.Length; x++)
                {
                    for (int j = 0; j < pValues.Length && x < CIFRemitterAddressHome.Length; j++)
                    {
                        string[] vals = pValues[j].Split('~');
                        //if function paremeter == parameter field
                        if (CIFRemitterAddressHome[x].ToUpper() == vals[0].ToUpper())
                        {
                            CIFRemitterAddressHome_val += vals[1] + "|";
                            break;
                        }
                    }
                }
                #endregion

                #region Populate Remitter CIF Address Mail
                for (int x = 0; x < CIFRemitterAddressBusiness.Length; x++)
                {
                    for (int j = 0; j < pValues.Length && x < CIFRemitterAddressBusiness.Length; j++)
                    {
                        string[] vals = pValues[j].Split('~');
                        //if function paremeter == parameter field
                        if (CIFRemitterAddressBusiness[x].ToUpper() == vals[0].ToUpper())
                        {
                            CIFRemitterAddressBusiness_val += vals[1] + "|";
                            break;
                        }
                    }
                }
                #endregion

                #region Populate Remitter CIF Contacts
                for (int x = 0; x < CIFRemitterContacts.Length; x++)
                {
                    for (int j = 0; j < pValues.Length && x < CIFRemitterContacts.Length; j++)
                    {
                        string[] vals = pValues[j].Split('~');
                        //if function paremeter == parameter field
                        if (CIFRemitterContacts[x].ToUpper() == vals[0].ToUpper())
                        {
                            CIFRemitterContacts_val += vals[1] + "|";
                            break;
                        }
                    }
                }
                #endregion

                #region Populate Remitter CIF IDs
                for (int x = 0; x < CIFRemitterIDs.Length; x++)
                {
                    for (int j = 0; j < pValues.Length && x < CIFRemitterIDs.Length; j++)
                    {
                        string[] vals = pValues[j].Split('~');
                        //if function paremeter == parameter field
                        if (CIFRemitterIDs[x].ToUpper() == vals[0].ToUpper())
                        {
                            CIFRemitterIDs_val += vals[1] + "|";
                            break;
                        }
                    }
                }
                #endregion

                #region Populate Remitter CIF Occupation
                for (int x = 0; x < CIFRemitterOccupation.Length; x++)
                {
                    for (int j = 0; j < pValues.Length && x < CIFRemitterOccupation.Length; j++)
                    {
                        string[] vals = pValues[j].Split('~');
                        //if function paremeter == parameter field
                        if (CIFRemitterOccupation[x].ToUpper() == vals[0].ToUpper())
                        {
                            CIFRemitterOccupation_val += vals[1] + "|";
                            break;
                        }
                    }
                }
                #endregion

                #region Populate Beneficiary CIF Master Array
                for (int x = 0; x < CIFMasterBeneficiary.Length; x++)
                {
                    for (int j = 0; j < pValues.Length && x < CIFMasterBeneficiary.Length; j++)
                    {
                        string[] vals = pValues[j].Split('~');

                        if (CIFMasterBeneficiary[x].ToUpper() == "CREATED BY")
                        {
                            CIFMasterBeneficiary_val += get_userid().ToString() + "|";
                            break;
                        }
                        if (CIFMasterBeneficiary[x].ToUpper() == "DATE CREATED")
                        {
                            CIFMasterBeneficiary_val += get_branch_localtime(getBranchCode()).ToString() + "|";
                            break;
                        }
                        if (CIFMasterBeneficiary[x].ToUpper() == "BRANCH CODE")
                        {
                            CIFMasterBeneficiary_val += getBranchCode() + "|";
                            break;
                        }
                        //if function paremeter == parameter field
                        if (CIFMasterBeneficiary[x].ToUpper() == vals[0].ToUpper())
                        {
                            CIFMasterBeneficiary_val += vals[1] + "|";
                            break;
                        }
                    }
                }
                #endregion

                #region Populate Beneficiary CIF Class
                for (int x = 0; x < CIFClassBeneficiary.Length; x++)
                {
                    for (int j = 0; j < pValues.Length && x < CIFClassBeneficiary.Length; j++)
                    {
                        string[] vals = pValues[j].Split('~');
                        //if function paremeter == parameter field
                        if (CIFClassBeneficiary[x].ToUpper() == vals[0].ToUpper())
                        {
                            CIFClassBeneficiary_val += vals[1] + "|";
                            break;
                        }
                    }
                }
                #endregion

                #region Populate Beneficiary CIF Address
                for (int x = 0; x < CIFBeneficiaryAddress.Length; x++)
                {
                    for (int j = 0; j < pValues.Length && x < CIFBeneficiaryAddress.Length; j++)
                    {
                        string[] vals = pValues[j].Split('~');
                        //if function paremeter == parameter field
                        if (CIFBeneficiaryAddress[x].ToUpper() == vals[0].ToUpper())
                        {
                            CIFBeneficiaryAddress_val += vals[1] + "|";
                            break;
                        }
                    }
                }
                #endregion

                #region Populate Beneficiary CIF Contacts
                for (int x = 0; x < CIFBeneficiaryContacts.Length; x++)
                {
                    for (int j = 0; j < pValues.Length && x < CIFBeneficiaryContacts.Length; j++)
                    {
                        string[] vals = pValues[j].Split('~');
                        //if function paremeter == parameter field
                        if (CIFBeneficiaryContacts[x].ToUpper() == vals[0].ToUpper())
                        {
                            CIFBeneficiaryContacts_val += vals[1] + "|";
                            break;
                        }
                    }
                }
                #endregion
                #endregion

            }
            catch (Exception ex)
            {

            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string saveInfo(string dataList, bool isSaveBeneOnly, bool isAdd)
        {
            //isSaveBeneOnly = true -> save Beneficiary Only
            //isSaveBeneOnly = false -> save Remitter and Beneficiary
            string resultRemitterSave = "";
            string[] splitResultRemitterSave = null;
            string resultBeneficiarySave = "";
            string[] splitResultBeneficiarySave = null;
            setData(dataList, isAdd);

            try
            {
                //Save Remitter CIF 
                if (isSaveBeneOnly == false) //save remitter and benef
                {
                    resultRemitterSave = new irsInternetRemitterARS().saveRemitterCIF(CIFMasterRemitter_val.Split('|'), CIFClassRemitter_val.Split('|'), CIFRemitterAddressHome_val.Split('|'), CIFRemitterAddressBusiness_val.Split('|'), CIFRemitterContacts_val.Split('|'), CIFRemitterIDs_val.Split('|'), CIFRemitterOccupation_val.Split('|'), null, isAdd);
                    splitResultRemitterSave = resultRemitterSave.Split('|');
                    if (splitResultRemitterSave[0].ToString().ToUpper() == "OK")
                    {
                        //Save Beneficiary CIF
                        resultBeneficiarySave = new irsInternetRemitterARS().saveBeneficiaryCIF(CIFMasterBeneficiary_val.Split('|'), CIFClassBeneficiary_val.Split('|'), CIFBeneficiaryAddress_val.Split('|'), CIFBeneficiaryContacts_val.Split('|'), CIFBeneficiaries_val.Split('|'), isAdd);
                        splitResultBeneficiarySave = resultBeneficiarySave.Split('|');
                        if (splitResultBeneficiarySave[0].ToString().ToUpper() == "OK")
                        {
                            //return OK|RemitterCIFID|BeneficiaryCIFID
                            return "OK" + "|" + splitResultRemitterSave[1] + "|" + splitResultBeneficiarySave[1];
                        }
                        else
                        {
                            //return Save Beneficiary Error Message
                            //NOT OK|Exception Message...
                            return resultBeneficiarySave;
                        }
                    }
                    else
                    {
                        //return Save Remitter Error Message
                        //NOT OK|Exception Message...
                        return resultRemitterSave;
                    }
                } //save benef only
                else
                {
                    //Save Beneficiary CIF
                    resultBeneficiarySave = new irsInternetRemitterARS().saveBeneficiaryCIF(CIFMasterBeneficiary_val.Split('|'), CIFClassBeneficiary_val.Split('|'), CIFBeneficiaryAddress_val.Split('|'), CIFBeneficiaryContacts_val.Split('|'), CIFBeneficiaries_val.Split('|'), isAdd);
                    splitResultBeneficiarySave = resultBeneficiarySave.Split('|');
                    if (splitResultBeneficiarySave[0].ToString().ToUpper() == "OK")
                    {
                        //return OK|RemitterCIFID|BeneficiaryCIFID
                        return "OK" + "|" + splitResultRemitterSave[1] + "|" + splitResultBeneficiarySave[1];
                    }
                    else
                    {
                        //return Save Beneficiary Error Message
                        //NOT OK|Exception Message...
                        return resultBeneficiarySave;
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return "";
            //saving of benef
            //return 
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getCIFMasterSEQID(string paramCIFID)
        {
            return new irsInternetRemitterARS().getCIFMasterSEQID(paramCIFID);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getCIFContactsSEQID(string paramCIFID)
        {
            return new irsInternetRemitterARS().getCIFContactsSEQID(paramCIFID);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getCIFAddressSEQID(string paramCIFID, int addressType)
        {
            return new irsInternetRemitterARS().getCIFAddressSEQID(paramCIFID, addressType);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getCIFIDsSEQID(string paramCIFID)
        {
            return new irsInternetRemitterARS().getCIFIDsSEQID(paramCIFID);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getCIFOccupationsSEQID(string paramCIFID)
        {
            return new irsInternetRemitterARS().getCIFOccupationsSEQID(paramCIFID);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getPDDTSBankCurrency(string PDDTSBankCode)
        {
            DataTable dt = new irsInternetPage().getLookup("PDDTSBankCurrency");
            DataTable dtReturn = null;
            DataRow dr;
            int i = 0;
            string[] arrCurrency = new string[2];
            bool bFound = false;
            string errMsg = string.Empty;

            if (dt != null && dt.Rows.Count > 0)
            {
                dtReturn = new DataTable();
                dtReturn.Columns.Add("CurrName");
                dtReturn.Columns.Add("CurrCode");

                for (i = 0; i <= (dt.Rows.Count - 1); i++)
                {
                    if (dt.Rows[i]["FldCode"].ToString().Trim() == PDDTSBankCode.Trim())
                    {
                        bFound = true;
                        arrCurrency = dt.Rows[i]["FldDescription"].ToString().Split('|');
                        dr = dtReturn.NewRow();
                        dr[0] = arrCurrency.GetValue(1).ToString().Trim();
                        dr[1] = arrCurrency.GetValue(0).ToString().Trim();
                        dtReturn.Rows.Add(dr);
                    }
                }
                if (bFound == true)
                {
                    // writeAudit("Web Remittance - Get PDDTSBankCurrency", "PDDTSBankCurrency=[" + PDDTSBankCode + "] Currency=[" + arrCurrency.GetValue(1).ToString().Trim() + "]", "getPDDTSBankCurrency", string.Empty, csAuditLog.Category.Select);
                    return dtReturn;
                }
                else
                {
                    //errMsg = "Error on retrieving PDDTSBankCurrency";
                    //   writeAudit("Web Remittance - Get PDDTSBankCurrency", "Unable to retrieve PDDTSBankCurrency=[" + PDDTSBankCode + "] Error Message=[" + errMsg + "]", "getPDDTSBankCurrency", string.Empty, csAuditLog.Category.Select);
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getTargetCurrency(string ProductCode)
        {
            return new irsInternetRemitterARS().getTargetCurrency(ProductCode, getUserRole());
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getUserRole()
        {
            return Convert.ToString(((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).Role);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getModeOfRemittance()
        {
            return new irsInternetRemitterARS().getModeOfRemittance(getUserRole(), this.getBranchCode());
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getOtherBanks(string CurrencyCode)
        {
            return new irsInternetRemitterARS().getOtherBanks(CurrencyCode);
        }

        private string getZipCodeRegion(string ZipCode)
        {
            return new irsInternetRemitterARS().getZipCodeRegion(ZipCode);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool isValidZipcode(bool isInMetroManila, string ZipCode)
        {
            bool result;
            string region = "";
            region = getZipCodeRegion(ZipCode.Trim());

            if (region == "")
            {
                return false;
            }
            else
            {
                if (isInMetroManila == true)
                {
                    if (region == "NCR")
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {
                    if (region == "NCR")
                    {
                        result = false;
                    }
                    else
                    {
                        result = true;
                    }
                }
            }
            return result;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getFCAccount(string AccountNo)
        {
            //return new irsAccountInquiry().getAccountStatusAndName(AccountNo);
            return "";
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable searchRem(string LastName, string FirstName, string MiddleName)
        {
            return new irsInternetRemitterARS().searchRem(LastName, FirstName, MiddleName, getBranchCode());
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getEquivalentBranchName(string BranchCode)
        {
            return new irsInternetRemitterARS().getBranchName(BranchCode);

            //DataTable tbl = new Internet.InternetPage().getLookup(BranchCode);
            //string BranchName = "";

            //BranchName = Convert.ToString(tbl.Rows[0]["BranchName"]);
            //return BranchName;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string saveMode(string prodcode, string targetcurr, string accttype, string acctno, string acctstat, string accountBranchCode)
        {

            //string remitter_cifid = id.CIFID;
            //string bene_cifid = Session["WEBBCIFID"].ToString();
            string seqid = "";// (Session["WEBBSEQID"] == null) ? string.Empty : Session["WEBBSEQID"].ToString();
            bool IsAdd = true;
            string pAccountType = "";

            if (seqid == string.Empty)
            {
                seqid = new irsWebRemitAddBenef().getCIFBeneSEQID(getRemitterCIFID(), getBeneficiaryCIFID(), true);
            }
            IsAdd = (seqid.Trim() == string.Empty) ? true : false;
            if (accttype != "")
            {
                if (accttype.ToUpper() == "SAVINGS")
                {
                    pAccountType = "1";
                }
                else
                {
                    pAccountType = "2";
                }
            }

            String uloc = getBranchCode();
            string sReturn = new irsWebRemittance().saveModeOfImplementation(prodcode, targetcurr, accountBranchCode, pAccountType, acctno, acctstat, getBeneficiaryCIFID(), getRemitterCIFID(), seqid, uloc, IsAdd);
            //writeAudit("Web Remittance - Save Mode", "ReturnValue=[" + sReturn + "] IsAdd=[" + IsAdd.ToString() + "] CIFBeneficiariesSEQID=" + seqid + "] RemitterID=[" + remitter_cifid + "] BeneficiaryID=[" + bene_cifid + "] ProductCode=[" + prodcode + "] Currency=[" + targetcurr + "] AccountNo=[" + acctno + "] AccountType=[" + accttype + "] AccountStatus=[" + acctstat + "]", "Save_Mode", string.Empty, csAuditLog.Category.Insert);
            //Session["WEBBSEQID"] = string.Empty;
            //Session["WEBADDEDIT"] = "edit";
            return sReturn;
            //return "";
        }

        private string getRemitterCIFID()
        {
            return Session["ars_clientRemitterCIFID"].ToString();
        }

        private string getBeneficiaryCIFID()
        {
            return Session["ars_clientBeneficiaryCIFID"].ToString();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Boolean storeSessionForTransaction(string RemitterCIFID, string BeneficiaryCIFID, string ProductID)
        {
            Session["ARSWREMITTERID"] = RemitterCIFID;
            Session["ARSWBENEFICIARYID"] = BeneficiaryCIFID;
            Session["ARSWPRODUCTID"] = ProductID;
            return true;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getCIFInformation()
        {
            if (Session["ARSWMODULE"] != null)
            {
                return new irsInternetRemitterARS().getCIFInformation(Session["ARSWREMITTERID"].ToString(), Session["ARSWBENEFICIARYID"].ToString(), Session["ARSWPRODUCTID"].ToString(), getBranchCode());
            }
            else
            {
                return null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getBeneZipCode(string Zipcode)
        {
            DataTable dt;
            int i = 0;
            DataTable dtZip = new DataTable();
            DataRow dr;
            string[] arrZip = new string[2];

            dtZip.Columns.Add("SEQ_ID");
            dtZip.Columns.Add("ZIPCODE");

            try
            {
                dt = new irsWebRemitAddBenef().getZipcode();
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (i = 0; i <= (dt.Rows.Count - 1); i++)
                    {
                        if (dt.Rows[i][0].ToString().Trim().ToUpper().Contains(Zipcode.ToUpper()))
                        {
                            arrZip = dt.Rows[i][0].ToString().Trim().Split('~');
                            if (arrZip.GetValue(0).ToString().ToUpper().Trim() != Zipcode.ToUpper().Trim())
                            {
                                dr = dtZip.NewRow();
                                dr[0] = arrZip.GetValue(0);
                                dr[1] = arrZip.GetValue(1) + "-" + arrZip.GetValue(2) + "-" + arrZip.GetValue(3);
                                dtZip.Rows.Add(dr);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //this.writeAudit("Web Remittance - Search Zipcode", "Unable to find Zipcode=[" + Zipcode + "] Error Message=[" + ex.Message + "]", "getZipcode", string.Empty, csAuditLog.Category.Select);
                dtZip = null;
            }
            return dtZip;
        }
    }
}

