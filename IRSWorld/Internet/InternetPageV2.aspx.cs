using System;
using System.Data;
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

// **************************************************************************************************** //
// *** DEVELOPER    : Rogelio D. Guanlao                                                            *** //
// *** DATE CREATED : January 30, 2015                                                              *** //
// *** DATE MODIFIED: January 30, 2015                                                              *** //
// *** DESCRIPTION  : ARS Enhancement New Screen                                                    *** //
// **************************************************************************************************** //

namespace IRSWorld.Internet
{
    [Serializable]
    public partial class InternetPageV2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(InternetPageV2));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(rem_other_details));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(rem_modal_check_payments));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(rem_ach_payments));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(RemTranSaving));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(RemTranExtra));
            //AjaxPro.Utility.RegisterTypeForAjax(typeof(eXact));
            Session["InternetARMModule"] = "ONLINE";
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getSession()
        {
            string sReturnSession = "";
            if (Session["ARSTRANSACTION"] != null)
            {
                sReturnSession = "TRANS|" + Session["ARSTRANSACTION"].ToString();
            }
            else if (Session["ARSEDITREMITTER"] != null)
            {
                sReturnSession = "REM|" + Session["ARSEDITREMITTER"].ToString();
            }
            else if (Session["ARSEDITBENEFICIARY"] != null)
            {
                sReturnSession = "BEN|" + Session["ARSEDITBENEFICIARY"].ToString();
            }
            else if (Session["ARSADDPRODUCT"] != null)
            {
                sReturnSession = "PROD|" + Session["ARSADDPRODUCT"].ToString();
            }
            return sReturnSession;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void DeleteCIFTransSession(string SessionName)
        {
            Session[SessionName] = null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void CreateCIFTransSession(string SessionName, string strvalue)
        {
            Session[SessionName] = strvalue;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void writeAudit(string Description, string NewValue, string OldValue, csAuditLog.Category Category)
        {
            string sDescription = "Location=" + getUserLocation() + " : " + Description;
            string sModule = string.Empty;
            try
            {
                sModule = Session["InternetARMModule"].ToString();
            }
            catch (Exception ex)
            {
                sModule = "SESSION NAME TIMEOUT - " + ex.Message;
            }
            new csAuditLog().writeAudit("", sModule, sDescription, NewValue, OldValue, Category);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void writeLogger(string Log, string TableName, string Key)
        {
            new csAuditLog().writeLogger(Log, TableName, Key);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getUserLocation()
        {
            return ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).Location;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getBranchCountry()
        {
            string x_country = string.Empty;
            if (Session["IRS_CLIENT_DATA"] != null)
            {
                x_country = new irsInternetPage().getCountry(this.getUserLocation());
            }
            return x_country;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getUserID()
        {
            return ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).UserId;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getUserRole()
        {
            sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = null;
            string sReturn = string.Empty;
            try
            {
                ds = commons.getDataSet("SELECT * FROM TBL_LOOKUP WITH (NOLOCK) WHERE fldgroup = 'User Roles' and FLDVALUE ='" + ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).Role + "'", false, new object[] { });//Performance consideration when using irsSqlCommonsWS and sqlHelperWS

                if (ds.Tables[0].Rows[0]["FLDDescription"].ToString().ToUpper().Contains("AGENT") == true && (ds.Tables[0].Rows[0]["FLDDescription"].ToString().ToUpper().Contains("APPROVER") == true && ds.Tables[0].Rows[0]["FLDDescription"].ToString().ToUpper().IndexOf("PROCESSOR") < 0))
                {
                    sReturn = "27";
                }
                else if (ds.Tables[0].Rows[0]["FLDDescription"].ToString().ToUpper().Contains("AGENT") == true && (ds.Tables[0].Rows[0]["FLDDescription"].ToString().ToUpper().Contains("PROCESSOR") == true && ds.Tables[0].Rows[0]["FLDDescription"].ToString().ToUpper().IndexOf("APPROVER") < 0))
                {
                    sReturn = "7";
                }
                else if (ds.Tables[0].Rows[0]["FLDDescription"].ToString().ToUpper().Contains("AGENT") == true && (ds.Tables[0].Rows[0]["FLDDescription"].ToString().ToUpper().Contains("PROCESSOR") == true && ds.Tables[0].Rows[0]["FLDDescription"].ToString().ToUpper().Contains("APPROVER") == true))
                {
                    sReturn = "25";
                }
                else
                {
                    sReturn = Convert.ToString(((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).Role);
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit(((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).UserId, "InternetPage", "getUserRole", ex.Message, "", csAuditLog.Category.Select);
            }
            finally
            {
                ds.Dispose();
                ds = null;
            }
            return sReturn;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getAgentCurrency()
        {
            return new irsInternetPage().getAgentCurrency(getUserLocation());
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getSubLookup(string GroupName)
        {
            return new irsInternetPage().getSubLookup(getUserLocation(), GroupName);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getPaymentMethod()
        {
            return new irsInternetPage().getPaymentMethod(getUserLocation());
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getDefaults()
        {
            return new irsInternetPage().getDefaults(getUserLocation());
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getBranchSetup()
        {
            if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
            {
                return new irsInternetPage().getBranchSetup(((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location);
            }
            else
            {
                return null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getCIFTranxInfo(string remitterCIFID, string beneCIFID)
        {
            sqlHelperWs commons = new sqlHelperWs();
            System.Data.SqlClient.SqlCommand sqcommands = null;
            DataSet ds = null;
            try
            {
                if (commons != null)
                {
                    ds = commons.execStoredProcedure("isp_getCIFTranxDetails", new Object[] { remitterCIFID, beneCIFID });
                    String hitlist = "";
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        return ds;
                    }
                }
                return null;
            }
            catch
            {
                return null;
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
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String check_watchlist(string CIFID, bool isbene)
        {
            string sReturn = string.Empty;
            try
            {
                //sReturn = this.get_hitlist(Firstname, LastName, MiddleName, getUserLocation());
                if (isbene == true)
                {
                    sReturn = this.get_hitlist_all("", CIFID);
                }
                else
                {
                    sReturn = this.get_hitlist_all(CIFID, "");
                }
                if (sReturn != string.Empty && isbene == false)
                {
                    sReturn = "Remitter Name: <REMNAME> is a possible hit in the WatchList";
                }
                else if (sReturn != string.Empty && isbene == true)
                {
                    sReturn = "Beneficiary Name: <BENENAME> is a possible hit in the WatchList";
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit(((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).UserId, "InternetPage", "check_watchlist", ex.Message, "", csAuditLog.Category.Select);
            }
            return sReturn;
        }

        public string get_hitlist_all(string RemCIFID, string BeneCIFID)
        {
            sqlHelperWs commons = new sqlHelperWs();
            System.Data.SqlClient.SqlCommand sqcommands = null;

            try
            {
                if (commons != null)
                {
                    DataSet ds = commons.execStoredProcedure("sp_otc_hit_listing", new Object[] { RemCIFID, BeneCIFID });
                    String hitlist = "";
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
            catch
            {
                return "";
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
        }

        public String get_hitlist(String FirstName, String LastName, String MiddleName, string branchCode)
        {
            sqlHelperWs commons = new sqlHelperWs();
            System.Data.SqlClient.SqlCommand sqcommands = null;
            irsMethaphone myMethaphone = new irsMethaphone();
            string strFirstNamePhonetics = "";
            string strLastNamePhonetics = "";
            string strMiddleNamePhonetics = "";

            Session["strNewEnrolledCIFPhonetics1"] = "";
            Session["strNewEnrolledCIFPhonetics2"] = "";
            Session["strNewEnrolledCIFPhonetics3"] = "";
            Session["strNewEnrolledCIFPhonetics4"] = "";
            Session["strNewEnrolledCIFPhonetics5"] = "";
            Session["strNewEnrolledCIFPhonetics6"] = "";
            Session["strNewEnrolledCIFPhonetics13"] = "";
            Session["strNewEnrolledCIFPhonetics14"] = "";

            string strNewEnrolledCIFPhonetics1 = "";
            string strNewEnrolledCIFPhonetics2 = "";
            string strNewEnrolledCIFPhonetics3 = "";
            string strNewEnrolledCIFPhonetics4 = "";
            string strNewEnrolledCIFPhonetics5 = "";
            string strNewEnrolledCIFPhonetics6 = "";
            string strNewEnrolledCIFPhonetics13 = "";
            string strNewEnrolledCIFPhonetics14 = "";

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

            try
            {
                if (commons != null)
                {
                    /*
                        @v_FirstName		varchar(200)
                        ,@v_LastName		varchar(80)
                        ,@v_MiddleName		varchar(80)
                        ,@v_BranchCode		varchar(4)
                     */
                    /* Hajie orig code
                    DataSet ds = commons.execStoredProcedure("sp_check_and_get_cif_hitlist", new Object[] { FirstName, LastName, MiddleName, branchCode }); 
                    */
                    DataSet ds = commons.execStoredProcedure("sp_check_and_get_cif_hitlist_validations", new Object[] { FirstName, LastName, MiddleName, branchCode, strNewEnrolledCIFPhonetics1, strNewEnrolledCIFPhonetics2, strNewEnrolledCIFPhonetics3, strNewEnrolledCIFPhonetics4, strNewEnrolledCIFPhonetics5, strNewEnrolledCIFPhonetics6, strNewEnrolledCIFPhonetics13, strNewEnrolledCIFPhonetics14 });
                    String hitlist = "";
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
            catch
            {
                return "";
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
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getAccountInquiry(string AccountNo)
        {
            string sReturn = new irsAccountInquiry().getFC(AccountNo, irsAccountInquiry.FCMethod.AccountStatusAndBalInquiry, 0, true);
            if (sReturn.ToUpper().IndexOf("ERROR") >= 0 || sReturn.ToUpper().IndexOf("FAILED") >= 0)
            {
                sReturn = "There was a problem retrieving your account information. Please report to the nearest PNB branch and/or system administrator.";
            }
            return sReturn;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool getOSA(string BeneficiaryZipCodeSEQID)
        {
            sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = null;
            bool bReturn = false;
            try
            {
                ds = commons.getDataSet("select osa_ind from tbl_OSA Where zip_seq_id =" + BeneficiaryZipCodeSEQID, false, new object[] { });

                if (ds.Tables[0].Rows[0]["osa_ind"].ToString() == "1")
                {
                    bReturn = true;
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit(((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).UserId, "InternetPage", "getOSA", ex.Message, "", csAuditLog.Category.Select);
            }
            finally
            {
                ds.Dispose();
                ds = null;
            }
            return bReturn;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getExchangeRate(string SourceCurrency, string TargetCurrency)
        {
            sqlHelperWs commons = new sqlHelperWs();
            System.Data.SqlClient.SqlCommand sqcommands = null;
            DataSet ds = null;
            try
            {
                if (commons != null)
                {
                    ds = commons.execStoredProcedure("isp_getExchangeRate", new Object[] { this.getUserLocation(), SourceCurrency, TargetCurrency, true });
                    String hitlist = "";
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        return ds.Tables[0];
                    }
                }
                return null;
            }
            catch
            {
                return null;
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
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable FilteredChargeType(string productcode)
        {
            return new irsInternetPage().getChargeType(getUserLocation(), productcode);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool getOBPSAccount(string AccountNo)
        {
            return new irsOBPSTransactions().fOBPAccount(AccountNo);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool getGFCAggregate(string BeneCIFID, string AccountNo, int Currency, string Amount)
        {
            bool isGFCAggregate = false;
            isGFCAggregate = new irsLinkToGFC().check_GFC_Aggregate(BeneCIFID, AccountNo, Currency, float.Parse(Amount));
            writeAudit("CHECK GFC AGGREGATE: [Beneficiary ID=" + BeneCIFID + "][Account Number=" + AccountNo + "][Currency=" + Currency.ToString() + "][Amount=" + Amount + "][ReturnValue=" + Convert.ToString(isGFCAggregate) + "]", "", "", csAuditLog.Category.Select);
            return isGFCAggregate;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getGFCAggregate_Message()
        {
            try
            {
                if (Session["GFC_AGGREGATE_MESSAGE"] != null)
                {
                    return (string)Session["GFC_AGGREGATE_MESSAGE"];
                }
                else
                {
                    return "Failed to get GFC aggregate information. Please try again later.";
                }
            }
            catch
            {
                return "Failed to get GFC aggregate information. Please try again later.";
            }
            finally
            {
                Session["GFC_AGGREGATE_MESSAGE"] = null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string check_remitter_ids_required(string remitter_id)
        {
            DataTable dt = null;
            sqlHelperWs commons = new sqlHelperWs();
            string return_string = "1";

            try
            {
                DateTime branch_local_date = Convert.ToDateTime(new irsTimeZone().branchLocalTime(((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location));

                if (Convert.ToInt32(commons.getDataSet("SELECT COUNT(1) FROM TBL_CIF_IDS WHERE CIFID = '" + remitter_id + "' AND ISNULL(DELETED,0) <> 1", false, new Object[] { }).Tables[0].Rows[0][0]) == 0)
                {
                    return "0~Remitter identification card not found.";
                }

                dt = commons.getDataSet("otc_a_req_ids", true, new object[] { remitter_id, ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location, branch_local_date }).Tables[0];
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        int i = 0;
                        string expired_id_container = string.Empty;
                        string placeofissuance_container = string.Empty;

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            if (Convert.ToDateTime(dt.Rows[i]["EXPIRYDATE"].ToString()) < Convert.ToDateTime(branch_local_date.ToString("MM/dd/yyyy")))
                            {
                                expired_id_container += "\n";
                                expired_id_container += "\nIdentification Card: " + dt.Rows[i]["FLDDESCRIPTION"].ToString();
                                expired_id_container += "\nIdentification Number: " + dt.Rows[i]["IDNO"].ToString();
                                expired_id_container += "\nExpiration Date: " + dt.Rows[i]["EXPIRYDATE"].ToString();
                                expired_id_container += "\n";
                            }
                            if (dt.Rows[i]["PlaceOfIssuance"].ToString() == "EMPTY")
                            {
                                placeofissuance_container += "\n";
                                placeofissuance_container += "\nIdentification Card: " + dt.Rows[i]["FLDDESCRIPTION"].ToString();
                                placeofissuance_container += "\nIdentification Number: " + dt.Rows[i]["IDNO"].ToString();
                                placeofissuance_container += "\nPlace Of Issuance: ";
                                placeofissuance_container += "\n";
                            }
                        }

                        if (expired_id_container != null && expired_id_container.Trim().Length > 0)
                        {
                            return_string = "0~" + "Remitter identificaton card expired.\n" + expired_id_container;
                        }
                        if (placeofissuance_container != null && placeofissuance_container.Trim().Length > 0)
                        {
                            if (return_string != "1")
                            {
                                return_string += "\n\nRemitter identificaton has no place of issuance.\n" + placeofissuance_container;
                            }
                            else
                            {
                                return_string = "0~" + "Remitter identificaton has no place of issuance.\n" + placeofissuance_container;
                            }
                        }
                    }
                }

                return return_string;
            }
            catch
            {
                return return_string;
            }
            finally
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getCharges(string Amount, string ProductCode, string ImplementationType, string SourceCurrency, string TargetCurrency, string ChargeType, string PayMode, string ComputationLevel, bool IsRemittance, bool ReCompute, string Percentage)
        {
            string sChargesID = new irsInternetPage().getChargesID(getUserLocation(), ProductCode, ImplementationType, SourceCurrency, ChargeType, PayMode);
            
            string sReturn = string.Empty;
            sChargesID = (sChargesID == string.Empty) ? "NO CHARGE MAINTAINED FOR SELECTED COMBINATION" : sChargesID;
            writeAudit("GET CHARGE ID: [ChargeID=" + sChargesID + "][BranchCode=" + getUserLocation() + "][ProductCode=" + ProductCode + "][ImplementationType=" + ImplementationType + "][SourceCurrency=" + SourceCurrency + "][TargetCurrency=" + TargetCurrency + "][ChargeType=" + ChargeType + "][PayMode=" + PayMode + "][ComputationLevel=" + ComputationLevel + "][IsRemittance=" + IsRemittance + "][Amount=" + Amount + "]", "", "", csAuditLog.Category.Select);
            if (sChargesID == "NO CHARGE MAINTAINED FOR SELECTED COMBINATION")
            {
                return "NOCHARGE";
            }
            else
            {
                sReturn = new irsInternetPage().getCharges(Amount, Convert.ToInt32(sChargesID), Convert.ToInt32(TargetCurrency), Convert.ToInt32(ComputationLevel), false, IsRemittance, ReCompute, Percentage) + sChargesID;
                writeAudit("CHARGE VALUE: [ChargeID=" + sChargesID + "][ReturnValue=" + sReturn + "]", "", "", csAuditLog.Category.Select);
                if (!sReturn.Contains("|"))
                {
                    sReturn = "NOCHARGE";
                }
                return sReturn;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void setOverride(string OldFXRates, string NewFXRates, string OldCommission, string NewCommission)
        {
            string sOverride = string.Empty;
            sOverride = "FOREX_RATES" + "~" + "remitting_to_account_rate" + "~" + NewFXRates + "|";
            sOverride += "FOREX_RATES" + "~" + "account_to_remitting_rate" + "~" + 1 / Convert.ToDecimal(NewFXRates) + "|";
            sOverride += "CHARGES_DATA" + "~" + "1~" + NewCommission;
            writeAudit("OVERRIDE: [FXRates=" + OldFXRates + " to " + NewFXRates + "][Commission=" + OldCommission + " to " + NewCommission + "][OverrideParameter=" + sOverride + "]", "", "", csAuditLog.Category.Update);
            IRSWorld.TranClass.RemTranCommons irsOverride = new IRSWorld.TranClass.RemTranCommons();
            irsOverride.set_calculator_override_values(sOverride, "ARM_Calculator");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable RemitterCashAgg(string remitterid, string remittingcurrency, string amount)
        {
            if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
            {
                DateTime branch_current_date = new irsInternetPage().getTransactionDate(getUserLocation(), true);
                return new irsInternetPage().CashAggregation(amount, remitterid, remittingcurrency, getUserLocation(), branch_current_date.ToString());
            }
            else
            {
                return null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool checkInternetRemittanceAggregateLimit(string remitterid, string remittingcurrency, string amount)
        {
            if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
            {
                DateTime branch_current_date = new irsInternetPage().getTransactionDate(getUserLocation(), true);
                return new irsInternetPage().aggregateInternet(amount, remitterid, remittingcurrency, getUserLocation(), branch_current_date.ToString());
            }
            else
            {
                return false;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string verifyOAAM(string CreditAccount, string Currency, string Amount)
        {
            string sOAAM = new irsConfiguration().appsetting("OAAM_DOWN");
            bool bOAAM;
            int number;
            bool result;
            string DebitAccount = string.Empty;
            

            Object[] oaam_params = null;
            writeAudit("CHECK OAAM CONFIG VALUE: [OAAM=" + sOAAM + "]", "", "", csAuditLog.Category.OAAM);
            if (sOAAM != "true")
            {
                try
                {
                    result = int.TryParse(Currency, out number);
                    TranClass.rem_debit_account_screen OAAM = new rem_debit_account_screen();
                    DebitAccount = new irsInternetPage().getBranchNSVS(getUserLocation(), (result == true ? Convert.ToInt32(Currency) : Convert.ToInt32(new irsInternetPage().getLookupValue("Currency", "FldCode", Currency, "FldValue"))));
                    
                    bOAAM = OAAM.oaam_verify_transaction("FundTransfer", DebitAccount, CreditAccount, Amount.Replace(",", ""), out oaam_params);
                    Session["OAAM_Parameters"] = oaam_params;
                    writeAudit("WRITE TO OAAM (VERIFY): [Transaction Type=FundTransfer][Debit Account=" + DebitAccount + "][Credit Account=" + CreditAccount + "][Currency=" + Currency + "][Amount=" + Amount + "][ReturnValue=" + bOAAM.ToString() + "]", "", "", csAuditLog.Category.OAAM);
                    return "OK";
                }
                catch (Exception ex)
                {
                    Session["OAAM_Parameters"] = oaam_params;
                    Session["OAAM_Blocked"] = "BLOCKED";
                    writeAudit("WRITE TO OAAM (VERIFY FAILED): [Exception Message=" + ex.Message.ToString() + "]", "", "", csAuditLog.Category.OAAM);
                    this.updateOAAM(false, "", "");
                    return ex.Message;
                }
            }
            else
            {
                return "OK";
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void updateOAAM(bool IsFCSuccess, string ReferenceNo, string ExternalReferenceNo)
        {
            string sOAAM = new irsConfiguration().appsetting("OAAM_DOWN");
            writeAudit("CHECK OAAM CONFIG VALUE: [OAAM=" + sOAAM + "]", "", "", csAuditLog.Category.OAAM);
            if (sOAAM != "true")
            {
                bool bOAAM;
                Object[] oaam_params = (Object[])Session["OAAM_Parameters"];
                TranClass.rem_debit_account_screen OAAM = new rem_debit_account_screen();
                if (Session["OAAM_Blocked"] != null)
                {
                    bOAAM = OAAM.oaam_update_transaction_status(rem_debit_account_screen.OAAMTranxStatus.Block, ReferenceNo, ExternalReferenceNo, oaam_params);
                    writeAudit("WRITE TO OAAM (UPDATE): [IsFCSuccess=BLOCKED][Reference No=" + ReferenceNo + "][External Reference No=" + ExternalReferenceNo + "][ReturnValue=" + bOAAM.ToString() + "]", "", "", csAuditLog.Category.OAAM);
                }
                else
                {
                    bOAAM = OAAM.oaam_update_transaction_status((IsFCSuccess == true ? rem_debit_account_screen.OAAMTranxStatus.Allow : rem_debit_account_screen.OAAMTranxStatus.Error), ReferenceNo, ExternalReferenceNo, oaam_params);
                    writeAudit("WRITE TO OAAM (UPDATE): [IsFCSuccess=" + (IsFCSuccess == true ? "ALLOW" : "REJECT") + "][Reference No=" + ReferenceNo + "][External Reference No=" + ExternalReferenceNo + "][ReturnValue=" + bOAAM.ToString() + "]", "", "", csAuditLog.Category.OAAM);
                }
                Session["OAAM_Blocked"] = null;
                Session["OAAM_Parameters"] = null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string generateORNo()
        {
            return new irsPaymentGateway().generateReference(getUserLocation());
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getTransactionDate()
        {
            DateTime dtTransactionDate = new irsInternetPage().getTransactionDate(getUserLocation(), true);
            return Convert.ToString(dtTransactionDate);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getServerTime()
        {
            DateTime dtServerTime = new irsTimeZone().branchLocalTime(new irsInternetPage().getORDHOBranch(true));
            return Convert.ToString(dtServerTime);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public int submitRemittance(string Remittance)
        {
            try
            {
                if ((string)System.Web.HttpContext.Current.Session["Remittance"] == "SUMBIT")
                {
                    System.Web.HttpContext.Current.Session["Remittance"] = "";
                    return -1;
                }

                System.Web.HttpContext.Current.Session["Remittance"] = "SUMBIT";
                writeAudit("CREATE REMITTANCE: " + Remittance, "", "", csAuditLog.Category.Insert);
                return new irsARM().submitRemittance(Remittance);
            }
            catch
            {
                return -1;
            }
            finally
            {
                System.Web.HttpContext.Current.Session["Remittance"] = "";
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getReferenceNo(string ORNo, string TranxDate)
        {
            return new irsARM().getReferenceNo(ORNo, Convert.ToDateTime(TranxDate));
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string submitRemittanceCharges(int SEQID)
        {
            irsCalculator irsCalc;
            string sReturn = "0";

            try
            {
                if (System.Web.HttpContext.Current.Session["ARM_Calculator"] != null)
                {
                    irsCalc = (irsCalculator)System.Web.HttpContext.Current.Session["ARM_Calculator"];
                    irsCalc.commit_remittance_charge_details(SEQID);
                    sReturn = "1";
                }
                else
                {
                    sReturn = "0";
                }
            }
            catch (Exception ex)
            {
                sReturn = ex.Message;
            }
            finally
            {
                irsCalc = null;
                System.Web.HttpContext.Current.Session["ARM_Calculator"] = null;
            }
            writeAudit("SAVE REMITTANCE CHARGES: [SEQID=" + SEQID + "][ReturnValue=" + sReturn + "]", "", "", csAuditLog.Category.Insert);
            return sReturn;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool check_is_post_dated(string check_date)
        {
            sqlHelperWs commons = new sqlHelperWs();
            if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
            {
                DateTime branch_current_date = Convert.ToDateTime(new irsTimeZone().branchLocalTime(((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location));

                string s = "SELECT COUNT(*) AS holidays_count " +
                           "FROM TBL_HOLIDAYS " +
                           "WHERE " +
                                "ISNULL(DELETED, 0) = 0 AND " +
                                "ISNULL(HOLIDAY, 0) = 1 AND " +
                                "BRANCHCODE = '" + ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location + "' AND " +
                                "CONVERT(VARCHAR,HOL_DATE, 101) = '" + branch_current_date.ToString("MM/dd/yyyy") + "'";

                DataTable dt = commons.getDataSet(s, false, new object[] { }).Tables[0];//Performance consideration when using irsSqlCommonsWS and sqlHelperWS
                try
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        if (Convert.ToInt32(dt.Rows[0]["holidays_count"]) > 0)
                        {
                            return true;
                        }
                    }
                }
                catch
                {
                    return false;
                }
                finally
                {
                    if (dt != null)
                    {
                        dt = null;
                    }
                }

                if (Convert.ToDateTime(check_date) > branch_current_date)
                {
                    return true;
                }
            }

            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool submitGFC(string ReferenceNo, string AccountNo, int Currency, string Amount)
        {
            /*
            bool isGFCSave = false;
            try
            {
                isGFCSave = new irsLinkToGFC().insert_GFC_Aggregate(ReferenceNo, AccountNo, Currency, float.Parse(Amount));
                writeAudit("SAVE GFC AGGREGATE: [Reference Number=" + ReferenceNo + "][Account Number=" + AccountNo + "][Currency=" + Currency.ToString() + "][Amount=" + Amount + "][ReturnValue=" + Convert.ToString(isGFCSave) + "]", "", "", csAuditLog.Category.Insert);
            }
            catch (Exception ex)
            {
                writeAudit("SAVE GFC AGGREGATE: [Reference Number=" + ReferenceNo + "][Account Number=" + AccountNo + "][Currency=" + Currency.ToString() + "][Amount=" + Amount + "][ReturnValue=" + Convert.ToString(isGFCSave) + "]", ex.Message, ex.StackTrace, csAuditLog.Category.Insert);
            }
            return isGFCSave;
            */
            return true;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string processTransaction(int SEQID)
        {
            DataSet ds = new irsRemTeller().RemitToAgent(SEQID);
            string sReturn = string.Empty;
            int i = 0;
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (i = 0; i <= (ds.Tables[0].Columns.Count - 1); i++)
                {
                    sReturn += ds.Tables[0].Rows[0][i].ToString() + "|";
                }
                sReturn = sReturn.Substring(0, sReturn.Length - 1);
            }
            writeAudit("PROCESSING REMITTANCE: [SeqID=" + SEQID + "][Response=" + sReturn + "]", "", "", csAuditLog.Category.FlexCube);
            return sReturn;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getRemittanceLimit()
        {
            return new irsInternetPage().getRemittanceLimit(getUserLocation());
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string sendOREmail(string ReferenceNo, string Txn_SEQID)
        {
            string sReturn = string.Empty;
            bool bReturn = false;
            string sRemitterCIFID = string.Empty;
            string sError = string.Empty;
            if (ReferenceNo.Trim() == string.Empty)
            {
                DataTable dtref = new irsInternetPage().getRemittanceInfo(Convert.ToInt32(Txn_SEQID));
                if (dtref != null && dtref.Rows.Count > 0)
                {
                    ReferenceNo = dtref.Rows[0]["ReferenceNo"].ToString().Trim();
                }
            }
            DataTable dt = new irsInternetPage().getRemittance(ReferenceNo);
            CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

            if (dt.Rows.Count > 0)
            {
                sRemitterCIFID = dt.Rows[0]["RemitterCIFID"].ToString().Trim();
            }
            if (new irsInternetPage().getRemitterEmail(sRemitterCIFID) != string.Empty)
            {
                bReturn = new CrystalReportUtil().OREmailSending(new TranClass.CrystalReportUtil().generate_otc_report(getUserLocation(), Txn_SEQID), ReferenceNo, sRemitterCIFID, out sError);
            }
            else
            {
                sError = "No registered email address. Please contact PNB for more info.";
            }
            writeAudit("SEND OR EMAIL: [ReferenceNo=" + ReferenceNo + "][RemitterCIFID=" + sRemitterCIFID + "][Response=" + bReturn.ToString() + "][ErrorMessage=" + sError + "]", "", "", csAuditLog.Category.Email);
            return sError;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getSystemControl_RemitterInfo(string remitterID, string beneficiaryID)
        {
            if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
            {
                return new irsInternetPage().getSystemControl_RemitterInfo(remitterID, beneficiaryID);
            }
            else
            {
                return null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getSystemControl_RemitterIDs(string remitterID, string seq_ID)
        {
            if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
            {
                return new irsInternetPage().getSystemControl_RemitterIDs(remitterID, seq_ID);
            }
            else
            {
                return null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool checkSystemControl_RemitterInfo(string remitterID, string beneficiaryID)
        {
            DataTable remInfo = this.getSystemControl_RemitterInfo(remitterID, beneficiaryID);
            int isincomplete = 0;
            if (remInfo != null || remInfo.Rows.Count > 0)
            {
                if (remInfo.Rows[0]["RemitterBirthDate"].ToString() == "" || remInfo.Rows[0]["RemitterBirthDate"].ToString() == "01/01/1900")
                {
                    isincomplete += 1;
                }

                if (remInfo.Rows[0]["Occupation"].ToString() == "" || remInfo.Rows[0]["Occupation"].ToString() == "0")
                {
                    isincomplete += 1;
                }

                if (remInfo.Rows[0]["SourceofFunds"].ToString() == "" || remInfo.Rows[0]["SourceofFunds"].ToString() == "0")
                {
                    isincomplete += 1;
                }

                if (remInfo.Rows[0]["Relationship"].ToString() == "" || remInfo.Rows[0]["Relationship"].ToString() == "0")
                {
                    isincomplete += 1;
                }
            }

            if (isincomplete > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string checkSystemControl_RemitterIDs(string remitterID)//0=No ID, 1= ID OK, 2=ExpiredID
        {
            DataTable remID = null;
            string sReturn = string.Empty;
            DateTime branch_current_date = new irsInternetPage().getTransactionDate(this.getUserLocation(), true);
            sqlHelperWs commons = new sqlHelperWs();
            int notExpired = 0;
            try
            {
                remID = commons.getDataSet("SELECT * FROM tbl_cif_ids where idtype in (11,15,27,32,33,34) AND CIFID ='" + remitterID + "' AND ISNULL(Deleted,0) = 0", false, new object[] { }).Tables[0];
                if (remID != null || remID.Rows.Count > 0)
                {
                    if (remID.Select("IDType = 15").Length > 0)
                    {
                        sReturn += "1_SSS ID/SSN~";
                    }
                    else
                    {
                        sReturn += "0_SSS ID/SSN~";
                    }

                    if (remID.Select("IDType = 27").Length <= 0 && remID.Select("IDType = 32").Length <= 0 && remID.Select("IDType = 33").Length <= 0 && remID.Select("IDType = 34").Length <= 0)
                    {
                        sReturn += "0_US-based ID or\r\nUS Govt/State ID or\r\nUS Issued Drivers License or\r\nUS Passport~";
                    }

                    if (remID.Select("IDType = 27").Length > 0)
                    {
                        if (Convert.ToDateTime(remID.Select("IDType = 27")[0]["ExpiryDate"].ToString()) < branch_current_date)
                        {
                            sReturn += "2_US-based ID~";
                        }
                        else
                        {
                            sReturn += "1_US-based ID~";
                            notExpired = notExpired + 1;
                        }
                    }
                    //else
                    //{
                    //    sReturn += "0_US-based ID~";
                    //}

                    if (remID.Select("IDType = 32").Length > 0)
                    {
                        if (Convert.ToDateTime(remID.Select("IDType = 32")[0]["ExpiryDate"].ToString()) < branch_current_date)
                        {
                            sReturn += "2_US Govt/State ID~";
                        }
                        else
                        {
                            sReturn += "1_US Govt/State ID~";
                            notExpired = notExpired + 1;
                        }
                    }
                    //else
                    //{
                    //    sReturn += "0_US Govt/State ID~";
                    //}

                    if (remID.Select("IDType = 33").Length > 0)
                    {
                        if (Convert.ToDateTime(remID.Select("IDType = 33")[0]["ExpiryDate"].ToString()) < branch_current_date)
                        {
                            sReturn += "2_US Issued Drivers License~";
                        }
                        else
                        {
                            sReturn += "1_US Issued Drivers License~";
                            notExpired = notExpired + 1;
                        }
                    }
                    //else
                    //{
                    //    sReturn += "0_US Issued Drivers License~";
                    //}

                    if (remID.Select("IDType = 34").Length > 0)
                    {
                        if (Convert.ToDateTime(remID.Select("IDType = 34")[0]["ExpiryDate"].ToString()) < branch_current_date)
                        {
                            sReturn += "2_US Passport~";
                        }
                        else
                        {
                            sReturn += "1_US Passport~";
                            notExpired = notExpired + 1;
                        }
                    }

                    if (notExpired >= 1)
                    {
                        sReturn += "1_SSS ID/SSN~1_US-based ID~1_US Govt/State ID~1_US Issued Drivers License~1_US Passport~";
                    }
                    //else
                    //{
                    //    sReturn += "0_US Passport~";
                    //}
                }
            }
            catch (Exception ex)
            {
                sReturn = ex.Message;
                writeAudit("checkSystemControl_RemitterIDs", sReturn, remitterID + " " + ex.Message, csAuditLog.Category.Select);
            }
            finally
            {
                if (remID != null)
                {
                    remID.Dispose();
                    remID = null;
                }
                if (commons != null)
                {
                    commons = null;
                }
            }
            return sReturn;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public int saveSystemControl_RemitterInfo(string remInfoData)
        {
            int i = 0;
            string[] arrRemitterInfo;
            string[] arrRemitterInfoInsert = null;
            sqlHelperWs commons = new sqlHelperWs();
            int iReturn = 0;

            try
            {
                arrRemitterInfo = remInfoData.Split('~');
                if (arrRemitterInfo.Length > 0)
                {
                    //dt = createTable("Remittance");
                    //dr = dt.NewRow();
                    arrRemitterInfoInsert = new string[arrRemitterInfo.Length];
                    for (i = 0; i <= (arrRemitterInfo.Length - 1); i++)
                    {
                        if (arrRemitterInfo.GetValue(i).ToString() == "NULL")
                        {
                            arrRemitterInfoInsert.SetValue("", i);
                        }
                        else
                        {
                            arrRemitterInfoInsert.SetValue(arrRemitterInfo.GetValue(i), i);
                        }
                    }
                    //dt.Rows.Add(dr);
                    //iReturn = execStoredProcedureUpdate("ksp_InsertRemittance", dr);
                    iReturn = commons.execStoredProcedureUpdate("ksp_UpdateRemitterInfo", arrRemitterInfoInsert);
                }
                else
                {
                    iReturn = -1;
                }
                writeAudit("saveSystemControl_RemitterInfo", remInfoData, iReturn.ToString(), csAuditLog.Category.Update);
                return iReturn;
            }
            catch (Exception ex)
            {
                writeAudit("saveSystemControl_RemitterInfo", remInfoData, iReturn.ToString() + " " + ex.Message, csAuditLog.Category.Update);
                return -1;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public int saveSystemControl_RemitterIDs(string remIDsData)
        {
            int i = 0;
            string[] arrRemitterIDs;
            string[] arrRemitterIDsInsert = null;
            sqlHelperWs commons = new sqlHelperWs();
            int iReturn = 0;

            try
            {
                arrRemitterIDs = remIDsData.Split('~');
                if (arrRemitterIDs.Length > 0)
                {
                    //dt = createTable("Remittance");
                    //dr = dt.NewRow();
                    arrRemitterIDsInsert = new string[arrRemitterIDs.Length];
                    for (i = 0; i <= (arrRemitterIDs.Length - 1); i++)
                    {
                        if (arrRemitterIDs.GetValue(i).ToString() == "NULL")
                        {
                            arrRemitterIDsInsert.SetValue("", i);
                        }
                        else
                        {
                            arrRemitterIDsInsert.SetValue(arrRemitterIDs.GetValue(i), i);
                        }
                    }
                    //dt.Rows.Add(dr);
                    //iReturn = execStoredProcedureUpdate("ksp_InsertRemittance", dr);
                    iReturn = commons.execStoredProcedureUpdate("ksp_SaveRemitterIDs", arrRemitterIDsInsert);
                }
                else
                {
                    iReturn = -1;
                }
                writeAudit("saveSystemControl_RemitterIDs", remIDsData, iReturn.ToString(), csAuditLog.Category.Update);
                return iReturn;
            }
            catch (Exception ex)
            {
                writeAudit("saveSystemControl_RemitterIDs", remIDsData, iReturn.ToString() + " " + ex.Message, csAuditLog.Category.Update);
                return -1;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getRemitterIDs(string CIFID, string seq_ID)
        {
            sqlHelperWs commons = new sqlHelperWs();
            DataTable dtIDs = null;
            try
            {
                dtIDs = commons.getDataSet("SELECT SEQ_ID,CIFID,IdType,IDNo,Convert(varchar(10),ExpiryDate,101) ExpiryDate,Remarks,ScannedID,GovtIdClass,VisaType,Deleted,IDClassification,PlaceOfIssuance FROM TBL_CIF_IDS WHERE CIFID = '" + CIFID + "' AND SEQ_ID ='" + seq_ID + "' AND ISNULL(DELETED,0) = 0", false, new object[] { }).Tables[0];
                writeAudit("getRemitterIDs", CIFID, seq_ID + " ", csAuditLog.Category.Select);
                return dtIDs;
            }
            catch (Exception ex)
            {
                writeAudit("getRemitterIDs", CIFID, seq_ID + " " + ex.Message, csAuditLog.Category.Select);
                return null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool checkExpired(string expdate)
        {
            DateTime branch_current_date = new irsInternetPage().getTransactionDate(this.getUserLocation(), true);
            try
            {
                if (Convert.ToDateTime(expdate) < Convert.ToDateTime(branch_current_date.ToString("MM/dd/yyyy")))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public int save_RemitterIDs(string remIDsData)
        {
            int i = 0;
            string[] arrRemitterIDs;
            string[] arrRemitterIDsInsert = null;
            sqlHelperWs commons = new sqlHelperWs();
            int iReturn = 0;

            try
            {
                arrRemitterIDs = remIDsData.Split('~');
                if (arrRemitterIDs.Length > 0)
                {
                    //dt = createTable("Remittance");
                    //dr = dt.NewRow();
                    arrRemitterIDsInsert = new string[arrRemitterIDs.Length];
                    for (i = 0; i <= (arrRemitterIDs.Length - 1); i++)
                    {
                        if (arrRemitterIDs.GetValue(i).ToString() == "NULL")
                        {
                            arrRemitterIDsInsert.SetValue("", i);
                        }
                        else
                        {
                            arrRemitterIDsInsert.SetValue(arrRemitterIDs.GetValue(i), i);
                        }
                    }
                    //dt.Rows.Add(dr);
                    //iReturn = execStoredProcedureUpdate("ksp_InsertRemittance", dr);
                    iReturn = commons.execStoredProcedureUpdate("ksp_SaveRemIDs", arrRemitterIDsInsert);
                }
                else
                {
                    iReturn = -1;
                }
                writeAudit("save_RemitterIDs", remIDsData, iReturn.ToString(), csAuditLog.Category.Update);
                return iReturn;
            }
            catch (Exception ex)
            {
                writeAudit("save_RemitterIDs", remIDsData, iReturn.ToString() + " " + ex.Message, csAuditLog.Category.Update);
                return -1;
            }
        }
    }
}
