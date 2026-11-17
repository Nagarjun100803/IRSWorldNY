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
// *** DEVELOPER    : Ronald James L. Tuazon                                                        *** //
// *** DATE CREATED : January 26, 2010                                                              *** //
// *** DATE MODIFIED: May 05, 2011                                                                  *** //
// *** DESCRIPTION  : Internet Transaction                                                          *** //
// **************************************************************************************************** //

namespace IRSWorld.Internet
{
    public enum TransactionType
    {
        FundTransfer,
        BillsPayment,
        PaymentGateway
    }

    [Serializable]
    public partial class InternetPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(InternetPage));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(rem_other_details));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(rem_modal_check_payments));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(rem_ach_payments));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(RemTranSaving));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(RemTranExtra));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(eXact));
            Session["InternetARMModule"] = "ONLINE";

            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool checkSessionTimeout()
        {
            if (((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).UserId == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void createSession(string SessionName, string Value)
        {
            Session[SessionName] = Value;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void generateORNo()
        {
            new irsPaymentGateway().generateReference(getLocation());
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getAccountInquiry(string AccountNo)
        {
            string actual_response = string.Empty;

            /* E2EAPICHANGE 20220209 Update Code*/
            string sReturn = new irsAccountInquiry().getFC(AccountNo, irsAccountInquiry.FCMethod.AccountStatusInquiry, 0, true);

            actual_response = sReturn;
            
            /* FOR CBIP 20170825 */
            int i = 0;
            DataTable dt = new irsAccountInquiry().BlackOutMode();
            bool b = false;
            if (dt != null && dt.Rows.Count > 0)
            {
                for (i = 0; i <= (dt.Rows.Count - 1); i++)
                {
                    if (sReturn.IndexOf(dt.Rows[i]["FldDescription"].ToString().Trim()) >= 0)
                    {
                        sReturn += "~" + "HOST IS UNAVAILABLE. STORE AND FORWARD WILL ACTIVATE";
                        b = true;
                        break;
                    }
                }
            }
            if (b)
            {
                return sReturn;
            }

            if (sReturn.ToUpper().IndexOf("SYSTEM UNAVAILABLE") >= 0)
            {
                sReturn += "~" + "HOST IS UNAVAILABLE. STORE AND FORWARD WILL ACTIVATE";
                return sReturn;
            }
            if (sReturn.ToUpper().IndexOf("ACCOUNT NOT FOUND") >= 0 || sReturn.ToUpper().IndexOf("CLOSED") >= 0 || sReturn.ToUpper().IndexOf("DORMANT") >= 0 || sReturn.ToUpper().IndexOf("ERROR") >= 0 || sReturn.ToUpper().IndexOf("FAILED") >= 0 || sReturn.ToUpper().IndexOf("INVALID") >= 0 || sReturn.ToUpper().IndexOf("NOT ACTIVE") >= 0)
            {
                sReturn = "There was a problem retrieving your account information. Please report to the nearest PNB branch and/or system administrator.";

                if (string.IsNullOrEmpty(actual_response).Equals(false))
                {
                    if (actual_response.ToUpper().IndexOf("DORMANT") >= 0)
                    {
                        sReturn += "~" + "DORMANT.";
                    }
                    else if (actual_response.ToUpper().IndexOf("CLOSED") >= 0)
                    {
                        sReturn += "~" + "CLOSED.";
                    }
                    else if (actual_response.ToUpper().IndexOf("INVALID") >= 0)
                    {
                        sReturn += "~" + "INVALID OR ACCOUNT NOT ACTIVE.";
                    }
                    else if (actual_response.ToUpper().IndexOf("NOT ACTIVE") >= 0)
                    {
                        sReturn += "~" + "NOT ACTIVE.";
                    }
                    else if (actual_response.ToUpper().IndexOf("ACCOUNT NOT FOUND") >= 0)
                    {
                        sReturn += "~" + "ACCOUNT NOT FOUND.";
                    }
                }
                else
                {
                    return "~INVALID";
                }
            }

            return sReturn;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool getAgentAccountBalance(double Amount, int Currency)
        {
            string sAccountNo = new irsInternetPage().getAgentAccountBalance(getLocation(), Currency);
            bool bReturn = false;
            if (sAccountNo != string.Empty)
            {
                if (new irsAccountInquiry().getAgentAccountBalance(sAccountNo, Amount) == true)
                {
                    bReturn = true;
                }
                else
                {
                    bReturn = false;
                }
            }
            else
            {
                bReturn = false;
            }
            writeAudit("GET AGENT ACCOUNT BALANCE: [Amount=" + Amount.ToString() + "][AccountNo=" + sAccountNo + "][ReturnValue=" + bReturn.ToString() + "]", "", "", csAuditLog.Category.Select);
            return bReturn;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getAgentCurrency()
        {
            return new irsInternetPage().getAgentCurrency(getLocation());
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getAgentRemitter(string BeneficiaryCIFID, string RemitterCIFID)
        {
            return new irsInternetPage().getAgentRemitter(BeneficiaryCIFID, RemitterCIFID);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getAgentRoles(int AgentType)
        {
            return new irsInternetPage().getAgentRoles(AgentType);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getBaseSiteUrl()
        {
            HttpContext context = HttpContext.Current;
            string baseUrl = context.Request.Url.Scheme + "://" + context.Request.Url.Authority + context.Request.ApplicationPath.TrimEnd('/') + '/';
            return baseUrl;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getBeneficiary(string RemitterCIFID)
        {
            return new irsInternetPage().getBeneficiary(RemitterCIFID, this.getUserRole());
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getBeneficiaryAddress(string CIFID)
        {
            return new irsInternetPage().getBeneficiaryAddress(CIFID);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getBeneficiaryContacts(string CIFID)
        {
            return new irsInternetPage().getBeneficiaryContacts(CIFID);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getBranchOperations()
        {
            return new irsInternetPage().getBranchOperations(new irsInternetPage().getBranchValue(getLocation(), "BranchCode", getLocation(), "SEQ_ID"));
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getCharges(string Amount, string BranchCode, string ProductCode, string ImplementationType, string SourceCurrency, string TargetCurrency, string ChargeType, string PayMode, string ComputationLevel, bool IsRemittance, bool ReCompute, string Percentage)
        {
            string sChargesID = new irsInternetPage().getChargesID(BranchCode, ProductCode, ImplementationType, SourceCurrency, ChargeType, PayMode);
            //bool isChargeAllowed = new irsInternetPage().getChargesUser(sChargesID, Convert.ToString(((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).Role));
            string sReturn = string.Empty;
            sChargesID = (sChargesID == string.Empty) ? "NO CHARGE MAINTAINED FOR SELECTED COMBINATION" : sChargesID;
            writeAudit("GET CHARGE ID: [ChargeID=" + sChargesID + "][BranchCode=" + BranchCode + "][ProductCode=" + ProductCode + "][ImplementationType=" + ImplementationType + "][SourceCurrency=" + SourceCurrency + "][TargetCurrency=" + TargetCurrency + "][ChargeType=" + ChargeType + "][PayMode=" + PayMode + "][ComputationLevel=" + ComputationLevel + "][IsRemittance=" + IsRemittance + "][Amount=" + Amount + "]", "", "", csAuditLog.Category.Select);
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
        public DataTable getChargesDefaults(string ChargeID)
        {
            return new irsInternetPage().getChargesDefaults(ChargeID);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getCIFAccount(string SEQ_ID)
        {
            return new irsInternetPage().getCIFAccount(SEQ_ID);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getCIFHit(string CIFID)
        {
            string sReturn = string.Empty;
            int iHitCount = 0;
            DataTable dt = new irsInternetPage().getCIFHit(getLocation(), CIFID);

            iHitCount = (dt == null) ? 0 : dt.Rows.Count;
            if (iHitCount > 0)
            {
                sReturn = "CIF has possible hits: " + iHitCount.ToString() + " row(s) found! \n\r\n\r";
                sReturn += "CIFID: " + dt.Rows[0]["CIFID"].ToString().Trim() + "\n\r";
                sReturn += "NAME: " + dt.Rows[0]["Name"].ToString().Trim() + "\n\r";
                sReturn += "MATCHING: " + dt.Rows[0]["Matching"].ToString().Trim() + "\n\r";
                sReturn += "MATCHING TYPE: " + dt.Rows[0]["MatchingType"].ToString().Trim() + "\n\r";
                sReturn += "COMPLIANCE FILE: " + dt.Rows[0]["ComplianceFile"].ToString().Trim() + "\n\r\n\r";
                sReturn += "Please get complete list from your local compliance officer.";
            }
            else
            {
                sReturn = "OK";
            }
            writeAudit("GET CIF HITS: [CIFID=" + CIFID + "][ReturnValue=" + sReturn + "]", "", "", csAuditLog.Category.Select);
            return sReturn;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getCIFInformation(string CIFID)
        {
            return new irsInternetPage().getCIFInformation(CIFID);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getDCCard(string CIFID, string cardclass)
        {
            return new irsInternetPage().getDCCard(CIFID, cardclass);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getDefaults(string BranchCode)
        {
            return new irsInternetPage().getDefaults(BranchCode);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getDisclaimer()
        {
            //return new irsInternetPage().getDisclaimer(getRegion(true));
            return new irsInternetPage().getDisclaimer(new irsInternetPage().getBranchValue(getLocation(), "BranchCode", getLocation(), "SEQ_ID"));
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool getEhudReceipt(string ReferenceNo)
        {
            //Session["IRS_ReportsUI"] = new CrystalReportUtil().GenerateEHUDReceipt(new IRSWorld.Reports.ARS.EHUD.rptEHUDReceipt(), ReferenceNo);
            Session["IRS_ReportsUI"] = "EHUD:" + ReferenceNo;
            if (Session["IRS_ReportsUI"] != null)
            {
                return true;
            }
            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getExchangeRate(string SourceCurrency, string TargetCurrency, bool isAgent)
        {
            return new irsInternetPage().getExchangeRate(getLocation(), SourceCurrency, TargetCurrency, isAgent);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getExpiringIDs(string RemitterCIFID)
        {
            DateTime dtBranchTime = new irsTimeZone().branchLocalTime(getLocation());
            DataTable dtExpiring = new irsInternetPage().getExpiringIDs(getLocation(), RemitterCIFID, dtBranchTime);
            DataTable dtExpired = new irsInternetPage().getExpiredIDs(RemitterCIFID, dtBranchTime);
            bool bExpiring = false;
            bool bFound = false;
            string sReturn = string.Empty;
            int i = 0;
            int j = 0;
            int count = 0;
            string sTD1 = "<td style='background-color:#FFE3EA; font-size:xx-small; font-family: Arial; height:1px; text-align:left;'>";
            string sTD2 = "<td style='font-size:xx-small; font-family: Arial; height:1px; text-align:left;'>";

            if (dtExpiring != null)
            {
                if (dtExpiring.Rows.Count > 0)
                {
                    bFound = true;
                    sReturn = "<table width='100%' border='0'><tr>";
                    sReturn += "<td class='table_caption'><b>Identification Number</b></td>";
                    sReturn += "<td class='table_caption'><b>Identification Type</b></td>";
                    sReturn += "<td class='table_caption'><b>Expiry Date</b></td>";
                    sReturn += "<td class='table_caption'><b>Remarks</b></td>";
                    bExpiring = true;
                    for (i = 0; i <= (dtExpiring.Rows.Count - 1); i++)
                    {
                        if (i % 2 == 0)
                        {
                            sReturn += "</tr><tr>" + sTD1 + dtExpiring.Rows[i]["IDNo"] + "</td>";
                            sReturn += sTD1 + dtExpiring.Rows[i]["Description"] + "</td>";
                            sReturn += sTD1 + dtExpiring.Rows[i]["ExpiryDate"] + "</td>";
                            sReturn += sTD1 + "This ID will expire in " + dtExpiring.Rows[i]["DateExpired"] + " days.</td></tr>";
                        }
                        else
                        {
                            sReturn += "</tr><tr>" + sTD2 + dtExpiring.Rows[i]["IDNo"] + "</td>";
                            sReturn += sTD2 + dtExpiring.Rows[i]["Description"] + "</td>";
                            sReturn += sTD2 + dtExpiring.Rows[i]["ExpiryDate"] + "</td>";
                            sReturn += sTD2 + "This ID will expire in " + dtExpiring.Rows[i]["DateExpired"] + " days.</td></tr>";
                        }
                    }
                }
            }

            if (dtExpired != null)
            {
                if (dtExpired.Rows.Count > 0)
                {
                    bFound = true;
                    count = i;
                    if (bExpiring == false)
                    {
                        sReturn += "<table width='100%' border='0'><tr>";
                        sReturn += "<td class='table_caption'><b>Identification Number</b></td>";
                        sReturn += "<td class='table_caption'><b>Identification Type</b></td>";
                        sReturn += "<td class='table_caption'><b>Expiry Date</b></td>";
                        sReturn += "<td class='table_caption'><b>Remarks</b></td>";
                    }
                    else
                    {
                        sReturn += "<tr>REMOVE";
                    }

                    for (j = 0; j <= (dtExpired.Rows.Count - 1); j++)
                    {
                        if (count % 2 == 0)
                        {
                            sReturn += "</tr><tr>" + sTD1 + dtExpired.Rows[j]["IDNo"] + "</td>";
                            sReturn += sTD1 + dtExpired.Rows[j]["Description"] + "</td>";
                            sReturn += sTD1 + dtExpired.Rows[j]["ExpiryDate"] + "</td>";
                            sReturn += sTD1 + "The ID you submitted has been expired as of " + dtExpired.Rows[j]["ExpiryDate"] + ".</td></tr>";
                        }
                        else
                        {
                            sReturn += "</tr><tr>" + sTD2 + dtExpired.Rows[j]["IDNo"] + "</td>";
                            sReturn += sTD2 + dtExpired.Rows[j]["Description"] + "</td>";
                            sReturn += sTD2 + dtExpired.Rows[j]["ExpiryDate"] + "</td>";
                            sReturn += sTD2 + "The ID you submitted has been expired as of " + dtExpired.Rows[j]["ExpiryDate"] + ".</td></tr>";
                        }
                        count++;
                    }
                }
            }
            if (bFound == true)
            {
                sReturn = sReturn.Replace("<tr>REMOVE</tr>", "");
                sReturn += "</table>";
            }
            writeAudit("GET EXPIRING IDS: [RemitterCIFID=" + RemitterCIFID + "][ReturnValue=" + sReturn + "]", "", "", csAuditLog.Category.Select);
            return sReturn;
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
        public string getInternetLimit()
        {
            return new irsInternetPage().getInternetLimit(getLocation());
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getRemittanceLimit()
        {
            return new irsInternetPage().getRemittanceLimit(getLocation());
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getLocalTime()
        {
            DateTime dtLocalTime = new irsTimeZone().branchLocalTime(getLocation());
            Session["ARMLocalTime"] = dtLocalTime;
            return Convert.ToString(dtLocalTime);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getLocation()
        {
            return ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).Location;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getLoginID()
        {
            return ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).UserId;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getLookup(string GroupName)
        {
            return new irsInternetPage().getLookup(GroupName);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getLookupValue(string GroupName, string FindColumn, string LookValue, string ReturnColumn)
        {
            return new irsInternetPage().getLookupValue(GroupName, FindColumn, LookValue, ReturnColumn);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getSubLookup(string GroupName)
        {
            return new irsInternetPage().getSubLookup(getLocation(), GroupName);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool getOBPSAccount(string AccountNo)
        {
            return new irsOBPSTransactions().fOBPAccount(AccountNo);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool getOnlineProduct(int ProductCode)
        {
            return new irsInternetPage().getOnlineProduct(ProductCode);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getORNo()
        {
            if (Session["ORNo"] == null)
            {
                return new irsPaymentGateway().generateReference(getLocation());
            }
            else
            {
                return Session["ORNo"].ToString();
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool getOSA(string BeneficiaryID)
        {
            return new irsInternetPage().getOSA(BeneficiaryID);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool getPaymentGateway()
        {
            bool bPaymentGateway = false;
            string x_country = string.Empty;
            if (Session["IRS_CLIENT_DATA"] != null)
            {
                x_country = new irsInternetPage().getCountry(this.getLocation());
            }
            bPaymentGateway = new irsInternetPage().getPaymentGateway(this.getRegion(true), x_country);
            writeAudit("PAYMENT GATEWAY: [Region=" + this.getRegion(false) + "][ReturnValue=" + bPaymentGateway.ToString() + "]", "", "", csAuditLog.Category.Select);
            return bPaymentGateway;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getPaymentMethod()
        {
            return new irsInternetPage().getPaymentMethod(getLocation());
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getPNBAccounts(string CIFID, string BranchCode)
        {
            return new irsInternetPage().getPNBAccounts(CIFID, BranchCode);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getOtherAccounts(string CIFID)
        {
            return new irsInternetPage().getOtherAccounts(CIFID);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getPurpose()
        {
            return new irsInternetPage().getPurpose();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getReferenceNo(string ORNo, string TranxDate)
        {
            return new irsARM().getReferenceNo(ORNo, Convert.ToDateTime(TranxDate));
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getRegion(bool IsValue)
        {
            return new irsInternetPage().getRegion(getLocation(), IsValue);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getRemittance(string ReferenceNo)
        {
            return new irsInternetPage().getRemittance(ReferenceNo);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getRemitterDisclaimer(string RemitterCIFID)
        {
            string sReturn = new irsInternetPage().getRemitterDisclaimer(RemitterCIFID);
            writeAudit("GET REMITTER DISCLAIMER: [RemitterCIFID=" + RemitterCIFID + "][ReturnValue=" + sReturn + "]", "", "", csAuditLog.Category.Select);
            return sReturn;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getRemitterID()
        {
            return ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).CIFID;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getRemitterName(string CIFID)
        {
            string sReturn = string.Empty;
            DataTable dt;
            int i = 0;
            dt = new irsInternetPage().getRemitterName(CIFID);

            for (i = 0; i <= (dt.Columns.Count - 1); i++)
            {
                sReturn += dt.Rows[0][i].ToString().Trim() + "|";
            }
            sReturn = sReturn.Substring(0, sReturn.Length - 1);
            return sReturn;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getSearchBy()
        {
            return new irsInternetPage().getSearchBy();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getServerTime()
        {
            //DateTime dtServerTime = new irsTimeZone().branchLocalTime(new irsConfiguration().appsetting("ServerBranchCode"));
            DateTime dtServerTime = new irsTimeZone().branchLocalTime(new irsInternetPage().getORDHOBranch(true));
            return Convert.ToString(dtServerTime);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getTransactionDate()
        {
            DateTime dtTransactionDate = new irsInternetPage().getTransactionDate(getLocation(), true);
            return Convert.ToString(dtTransactionDate);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getTodayForex()
        {
            return new irsInternetPage().getTodayForex(getLocation());
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getUserID()
        {
            return ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).UserId;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getUserLocation()
        {
            return ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).Location;
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

            //return Convert.ToString(((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).Role);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getVIPPercent(string VIPType)
        {
            string sReturn = string.Empty;
            sReturn = new irsInternetPage().getVIPPercent(getLocation(), VIPType);
            writeAudit("VIP TAGGING: [VIPType=" + VIPType + "][Response=" + sReturn + "]", "", "", csAuditLog.Category.Select);
            return sReturn;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string processTransaction(int SEQID)
        {
            //DataSet ds = new irsRemTeller().Remit2(SEQID, false);
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
                bReturn = new CrystalReportUtil().OREmailSending(new TranClass.CrystalReportUtil().generate_otc_report(getLocation(), Txn_SEQID), ReferenceNo, sRemitterCIFID, out sError);
            }
            else
            {
                sError = "No registered email address. Please contact PNB for more info.";
            }
            writeAudit("SEND OR EMAIL: [ReferenceNo=" + ReferenceNo + "][RemitterCIFID=" + sRemitterCIFID + "][Response=" + bReturn.ToString() + "][ErrorMessage=" + sError + "]", "", "", csAuditLog.Category.Email);
            return sError;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string sendInternetReceipt(string ReferenceNo, string message)
        {
            DataTable dt = null;
            string sRemitterCIFID = string.Empty;
            string sError = string.Empty;

            try
            {
                dt = new irsInternetPage().getRemittance(ReferenceNo);
                if (dt.Rows.Count > 0)
                {
                    sRemitterCIFID = dt.Rows[0]["RemitterCIFID"].ToString().Trim();
                }
                if (new irsInternetPage().InternetReceiptEmail(sRemitterCIFID, message, ReferenceNo, out sError) == true)
                {
                    return "Receipt Email sent";
                }
                else
                {
                    return "Failed to send Receipt thru Email.";
                }
            }
            catch (Exception ex)
            {
                writeAudit("SEND OR EMAIL: [ReferenceNo=" + ReferenceNo + "][RemitterCIFID=" + sRemitterCIFID + "][ErrorMessage=" + sError + "]", ex.Message, "", csAuditLog.Category.Email);
                return "Failed to send Receipt thru Email.";
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
        public string submitORDCharges(string SEQID, string Amount, string ProductCode, string ImplementationType, string TargetCurrency, string ComputationLevel)
        {
            string ChargeType = new irsInternetPage().getLookupValue("ChargeType", "FldDescription", "Standard", "FldValue");
            string PayMode = new irsInternetPage().getLookupValue("PayMode", "FldDescription", "Cash (Notes)", "FldValue");
            string sChargesID = new irsInternetPage().getChargesID(new irsInternetPage().getORDHOBranch(false), ProductCode, ImplementationType, TargetCurrency, ChargeType, PayMode);
            string sReturn = string.Empty;
            string[] arrReturn;

            sChargesID = (sChargesID == string.Empty) ? "NO CHARGE MAINTAINED FOR THE SELECTED COMBINATION" : sChargesID;
            if (sChargesID == "NO CHARGE MAINTAINED FOR THE SELECTED COMBINATION")
            {
                return "NOCHARGE";
            }
            else
            {
                sReturn = new irsInternetPage().getCharges(Amount, Convert.ToInt32(sChargesID), Convert.ToInt32(TargetCurrency), Convert.ToInt32(ComputationLevel), false, true, false, "") + sChargesID;
                writeAudit("CHARGE VALUE: [ChargeID=" + sChargesID + "][ReturnValue=" + sReturn + "]", "", "", csAuditLog.Category.Select);
                if (!sReturn.Contains("|"))
                {
                    sReturn = "NOCHARGE";
                }
                else
                {
                    arrReturn = sReturn.Split('|');
                    sReturn = this.submitRemittanceCharges(Convert.ToInt32(SEQID));
                    writeAudit("SAVE ORD CHARGES: [SEQID=" + SEQID + "][ChargeID=" + sChargesID + "][ProductCode=" + ProductCode + "][ImplementationType=" + ImplementationType + "][TargetCurrency=" + TargetCurrency + "][ChargeType=" + ChargeType + "][PayMode=" + PayMode + "][Amount=" + Amount + "][NetToBeneAmount=" + arrReturn[29].ToString() + "][ORDCharge=" + arrReturn[38].ToString() + "][ReturnValue=" + sReturn + "]", "", "", csAuditLog.Category.Insert);
                }
                return sReturn;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string submitPaymentGateway(string BranchCode, string CIFID, string CardNo, string CardName, string ExpiryDate, string SecurityCode, string CardType, string CurrencyCode, string Amount, string Remarks)
        {
            string sReturn = string.Empty;
            string[] arrReturn;

            /* E2EAPICHANGE 20220209 Removed verifyOAAM */
            writeAudit("SWIPE CARD: [Region=" + getRegion(true) + "]" + "[BranchCode=" + BranchCode + "]" + "[CIFID=" + CIFID + "]" + "[CardNo=" + CardNo + "]" + "[CardName=" + CardName + "]" + "[ExpiryDate=" + ExpiryDate + "]" + "[CardType=" + CardType + "]" + "[Currency=" + CurrencyCode + "]" + "[Amount=" + Amount + "]" + "[Remarks=" + Remarks + "]", "", "", csAuditLog.Category.PaymentGateway);
            sReturn = new irsPaymentGateway().createRequest(getRegion(true), BranchCode, CIFID, CardNo, CardName, ExpiryDate, SecurityCode, CardType, CurrencyCode, Amount, Remarks, getBaseSiteUrl());
            arrReturn = sReturn.Split('|');

            /* E2EAPICHANGE 20220209 Removed condition */
            return sReturn;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string submiteXactPaymentGateway(string BranchCode, string CIFID, string CardNo, string CardName, string ExpiryDate, string SecurityCode, string CardType, string CurrencyCode, string Amount, string Remarks)
        {
            string sReturn = string.Empty;
            string[] arrReturn;

            /* E2EAPICHANGE 20220209 Removed verifyOAAM */
            writeAudit("SWIPE CARD: [Region=" + getRegion(true) + "]" + "[BranchCode=" + BranchCode + "]" + "[CIFID=" + CIFID + "]" + "[CardNo=" + CardNo + "]" + "[CardName=" + CardName + "]" + "[ExpiryDate=" + ExpiryDate + "]" + "[CardType=" + CardType + "]" + "[Currency=" + CurrencyCode + "]" + "[Amount=" + Amount + "]" + "[Remarks=" + Remarks + "]", "", "", csAuditLog.Category.PaymentGateway);

            //sReturn = new irsPaymentGateway().createRequest(getRegion(true), BranchCode, CIFID, CardNo, CardName, ExpiryDate, SecurityCode, CardType, CurrencyCode, Amount, Remarks, getBaseSiteUrl());

            sReturn = "EXACT|";

            arrReturn = sReturn.Split('|');
            /* E2EAPICHANGE 20220209 Removed condition */
            return sReturn;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void submitGFC(string ReferenceNo, string AccountNo, int Currency, string Amount)
        {
            /* FOR CBIP */
            /*
            bool isGFCSave = false;
            isGFCSave = new irsLinkToGFC().insert_GFC_Aggregate(ReferenceNo, AccountNo, Currency, float.Parse(Amount));
            writeAudit("SAVE GFC AGGREGATE: [Reference Number=" + ReferenceNo + "][Account Number=" + AccountNo + "][Currency=" + Currency.ToString() + "][Amount=" + Amount + "][ReturnValue=" + Convert.ToString(isGFCSave) + "]", "", "", csAuditLog.Category.Insert);
            */
        }

        /* E2EAPICHANGE 20220209 Removed verifyOAAMTransaction, verifyOAAM, updateOAAM */

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string updateDisclaimer(string RemitterCIFID, bool IsRead)
        {
            string sReturn = new irsInternetPage().updateDisclaimer(RemitterCIFID, IsRead);
            string[] arrReturn;
            writeAudit("UPDATE REMITTER DISCLAIMER: [RemitterCIFID=" + RemitterCIFID + "][IsRead=" + IsRead.ToString() + "][ReturnValue=" + sReturn + "]", "", "", csAuditLog.Category.Update);
            arrReturn = sReturn.Split('|');
            if (arrReturn[1].ToString().Trim() == "OK")
            {
                sReturn = "Record successfully updated. Disclaimer will be shown on next logon.";
            }
            else
            {
                sReturn = "Error while updating record. Error at " + sReturn + ". Please report to the nearest PNB branch and/or system administrator.";
            }
            return sReturn;
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
        public string submitRemittanceCards(string SEQID, string RemAcctSeqID, string ResponseNo, string Response, string AuthCode, bool IsCard)
        {
            string sReturn = "0";
            bool bReturn = false;
            try
            {
                TranClass.RemTranCardCommons irsCards = new IRSWorld.TranClass.RemTranCardCommons();
                bReturn = irsCards.registerCardResponse(SEQID, RemAcctSeqID, ResponseNo, Response, AuthCode, IsCard);
                if (bReturn == true)
                {
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
            writeAudit("SAVE REMITTANCE CARD: [SEQID=" + SEQID + "][RemitterAcctSEQID=" + RemAcctSeqID + "][ReturnValue=" + sReturn + "]", "", "", csAuditLog.Category.Insert);
            return sReturn;
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
        public void writeAudit(string Description, string NewValue, string OldValue, csAuditLog.Category Category)
        {
            string sDescription = "Location=" + getLocation() + " : " + Description;
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
        public string getEnetsResponse()
        {
            string sResponse = string.Empty;
            if (Session["ENETS_RESPONSE"] != null)
            {
                sResponse = (string)Session["ENETS_RESPONSE"];
                new csAuditLog().writeAudit("", "getEnetsResponse", "Session[ENETS_RESPONSE]", sResponse, "", csAuditLog.Category.Insert);
            }
            return sResponse;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void endEnetsSession()
        {
            Session["ENETS_RESPONSE"] = null;
            Session["ENETS_REQUEST"] = null;
            Session["ENETS_URL"] = null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getEXACTResponse()
        {
            string sResponse = string.Empty;
            if (Session["EXACT_RESPONSE"] != null)
            {
                sResponse = (string)Session["EXACT_RESPONSE"];
                new csAuditLog().writeAudit("", "getEXACTResponse", "Session[EXACT_RESPONSE]", sResponse, "", csAuditLog.Category.Insert);
            }
            return sResponse;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool checkEXACTResponse()
        {
            if (Session["EXACT_RESPONSE"] != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void endEXACTSession()
        {
            Session["EXACT_RESPONSE"] = null;
            Session["EXACT_AMOUNT"] = null;
            Session["EXACT_REFNO"] = null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getWebRemittanceChargeType()
        {
            DataTable dt = new irsInternetPage().getSubLookup(getLocation(), "ChargeType");
            string wr = "";
            try
            {
                wr = dt.Select("FldDescription='Web Remittance'")[0]["FldValue"].ToString();
            }
            catch { }
            return wr;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool pingSession()
        {
            if (Session["IRS_CLIENT_DATA"] != null && Session["InternetARMModule"] != null && Session["InternetARMModule"].ToString() == "ONLINE")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable FilteredChargeType(string productcode)
        {
            return new irsInternetPage().getChargeType(getLocation(), productcode);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String check_watchlist(string cifid, bool isbene)
        {
            sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = null;
            string sReturn = string.Empty;
            try
            {
                //ds = commons.getDataSet(commons.getSqlCommand("SELECT * FROM TBL_CIF_MASTER WITH (NOLOCK) WHERE CIFID ='" + cifid + "'", false), "TBL_INT_WATCHLIST");
                ds = commons.getDataSet("SELECT * FROM TBL_CIF_MASTER WITH (NOLOCK) WHERE CIFID ='" + cifid + "'", false, new object[] { });//Performance consideration when using irsSqlCommonsWS and sqlHelperWS
                //sReturn = this.get_hitlist(ds.Tables[0].Rows[0]["FirstName"].ToString(), ds.Tables[0].Rows[0]["LastName"].ToString(), ds.Tables[0].Rows[0]["MiddleName"].ToString(), getLocation());
                if (isbene)
                {
                    sReturn = get_hitlist_all("", cifid);
                }
                else
                {
                    sReturn = get_hitlist_all(cifid, "");
                }
                if (sReturn != string.Empty && isbene == false)
                {
                    sReturn = "Remitter Name: " + ds.Tables[0].Rows[0]["FirstName"].ToString() + " " + ds.Tables[0].Rows[0]["MiddleName"].ToString() + " " + ds.Tables[0].Rows[0]["LastName"].ToString() + " is in the WatchList";
                }
                else if (sReturn != string.Empty && isbene == true)
                {
                    sReturn = "Beneficiary Name: " + ds.Tables[0].Rows[0]["FirstName"].ToString() + " " + ds.Tables[0].Rows[0]["MiddleName"].ToString() + " " + ds.Tables[0].Rows[0]["LastName"].ToString() + " is in the WatchList";
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit(((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).UserId, "InternetPage", "check_watchlist", ex.Message, "", csAuditLog.Category.Select);
            }
            finally
            {
                ds.Dispose();
                ds = null;
            }
            return sReturn;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool isPNBReceipt(string BranchCode)
        {
            return new irsInternetPage().isPNBReceipt(BranchCode);
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

                //DataTable dt = commons.getDataSet(commons.getSqlCommand(s, false)).Tables[0];
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
        public string getRemitterAccountNo(Int64 seqID)
        {
            sqlHelperWs commons = new sqlHelperWs();
            if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
            {
                string s = "SELECT * FROM TBL_CIF_ACCOUNTS WHERE SEQ_ID = " + seqID;

                //DataTable dt = commons.getDataSet(commons.getSqlCommand(s, false)).Tables[0];
                DataTable dt = commons.getDataSet(s, false, new object[] { }).Tables[0];//Performance consideration when using irsSqlCommonsWS and sqlHelperWS
                try
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.Rows[0]["AccountNo"].ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                catch
                {
                    return "";
                }
                finally
                {
                    if (dt != null)
                    {
                        dt = null;
                    }
                }
            }
            else
            {
                return "";
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

                //string s = "SELECT DISTINCT " +
                //                "A.CIFID, " +
                //                "A.IDNO, " +
                //                "CONVERT(VARCHAR,A.EXPIRYDATE,101) AS EXPIRYDATE, " +
                //                "B.FLDDESCRIPTION " +
                //            "FROM TBL_CIF_IDS A " +
                //            "LEFT JOIN TBL_LOOKUP B ON B.FLDGROUP = 'IDTYPE' AND B.FLDVALUE = A.IDTYPE " +
                //            "WHERE ISNULL(A.Deleted, 0) = 0 AND A.CIFID = '" + remitter_id.Replace("'", "''") + "'";

                ////dt = commons.getDataSet(commons.getSqlCommand(s, false)).Tables[0];
                //dt = commons.getDataSet(s, false, new object[] { }).Tables[0];//Performance consideration when using irsSqlCommonsWS and sqlHelperWS

                if (Convert.ToInt32(commons.getDataSet("SELECT COUNT(1) FROM TBL_CIF_IDS WHERE CIFID = '" + remitter_id + "' AND ISNULL(DELETED,0) <> 1", false, new Object[] { }).Tables[0].Rows[0][0]) == 0)
                {
                    return "0~Remitter identification card not found.";
                }

                //ds = sqlkonek.getDataSet("otc_req_ids", true, new Object[] { remitter_id });
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
                    //else
                    //{
                    //    return_string = "0~Remittance required identification card not found.";
                    //}
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
        public bool checkBeneAddress(string bene_id, Int64 beneadd_seqid)
        {
            DataTable dt = null;
            sqlHelperWs commons = new sqlHelperWs();

            try
            {
                string s = "SELECT * FROM TBL_CIF_ADDRESS WHERE (ISNULL(address2,'')<>'' OR ISNULL(Zipcodeseqid,0)<>0) AND CIFID = '" + bene_id.Replace("'", "''") + "' AND SEQ_ID=" + beneadd_seqid;

                //dt = commons.getDataSet(commons.getSqlCommand(s, false)).Tables[0];
                dt = commons.getDataSet(s, false, new object[] { }).Tables[0];//Performance consideration when using irsSqlCommonsWS and sqlHelperWS
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
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
                    dt.Dispose();
                    dt = null;
                }
            }
            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool checkInternetRemittanceAggregateLimit(string remitterid, string remittingcurrency, string amount)
        {
            if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
            {
                DateTime branch_current_date = new irsInternetPage().getTransactionDate(getLocation(), true);//Convert.ToDateTime(new irsTimeZone().branchLocalTime(((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location));
                return new irsInternetPage().aggregateInternet(amount, remitterid, remittingcurrency, ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location, branch_current_date.ToString());
            }
            else
            {
                return false;
            }
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
        public DataTable RemitterCashAgg(string remitterid, string remittingcurrency, string amount)
        {
            if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
            {
                DateTime branch_current_date = new irsInternetPage().getTransactionDate(getLocation(), true); //Convert.ToDateTime(new irsTimeZone().branchLocalTime(((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location));
                return new irsInternetPage().CashAggregation(amount, remitterid, remittingcurrency, ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location, branch_current_date.ToString());
            }
            else
            {
                return null;
            }
        }

        /* E2EAPICHANGE 20220209 Removed PNBDebitAccount */

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

        private bool location_is_rci()
        {
            sqlHelperWs commons = null;

            string timezone_qry = timezone_qry = "select count(1) cnt_rec from tbl_branches where timeZone like '%US/%' and branchcode = '";
            string timezone_location = string.Empty;

            try
            {
                timezone_location = ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).Location;
                timezone_qry += timezone_location.Replace("'", "''") + "'";

                commons = new sqlHelperWs();

                if (Convert.ToInt32(commons.getDataSet(timezone_qry, false, new object[] { }, new string[] { }).Tables[0].Rows[0]["cnt_rec"]) > 0)
                {
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
            finally
            {
                commons = null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string checkSystemControl_RemitterIDs(string remitterID)//0=No ID, 1= ID OK, 2=ExpiredID
        {
            if (this.location_is_rci() == false)
            {
                return "1_RCC AGENT~"; //IF AGENT LOCATION IS CANADA NO NEED TO VALIDATE ID. NOT VALID FOR RCI ID CHECKING
            }

            DataTable remID = null;
            string sReturn = string.Empty;
            DateTime branch_current_date = new irsInternetPage().getTransactionDate(getLocation(), true);
            sqlHelperWs commons = new sqlHelperWs();
            int notExpired = 0;
            try
            {
                remID = commons.getDataSet("SELECT * FROM tbl_cif_ids where idtype in (15,27,32,33,34) AND CIFID ='" + remitterID + "' AND ISNULL(Deleted,0) = 0", false, new object[] { }).Tables[0];

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
            DateTime branch_current_date = new irsInternetPage().getTransactionDate(getLocation(), true);
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

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getBranchCountry()
        {
            string x_country = string.Empty;
            if (Session["IRS_CLIENT_DATA"] != null)
            {
                x_country = new irsInternetPage().getCountry(this.getLocation());
            }
            return x_country;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void end_sessions()
        {
            Session["InternetARMModule"] = null;
            Session["IRS_ReportsUI"] = null;
            Session["GFC_AGGREGATE_MESSAGE"] = null;
            Session["ARMLocalTime"] = null;
            Session["ORNo"] = null;
            Session["ENETS_REQUEST"] = null;
            Session["ENETS_URL"] = null;
            Session["EXACT_AMOUNT"] = null;
            Session["EXACT_REFNO"] = null;
            /* E2EAPICHANGE 20220209 Removed OAAM_Parameters, OAAM_Blocked */
            Session["Remittance"] = null;
            Session["ARM_Calculator"] = null;
            Session["ENETS_RESPONSE"] = null;

            Session["strNewEnrolledCIFPhonetics1"] = null;
            Session["strNewEnrolledCIFPhonetics2"] = null;
            Session["strNewEnrolledCIFPhonetics3"] = null;
            Session["strNewEnrolledCIFPhonetics4"] = null;
            Session["strNewEnrolledCIFPhonetics5"] = null;
            Session["strNewEnrolledCIFPhonetics6"] = null;
            Session["strNewEnrolledCIFPhonetics13"] = null;
            Session["strNewEnrolledCIFPhonetics14"] = null;
        }

        /* RFS BLOCK TIME DEPOSIT */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string isAccountTD(string AccountNo)
        {
            return new Internet.InternetRemitter().isAccountTD(AccountNo, false);
        }

        /* 3D THRESHOLD START */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_threshold_3d(string currency)
        {
            return new IRSWorld.TranClass.RemTranExtra().get_threshold_3d(currency);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_threshold_3d_inc(string currency)
        {
            return new IRSWorld.TranClass.RemTranExtra().get_threshold_3d_inc(currency);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_threshold_3d_min(string currency)
        {
            return new IRSWorld.TranClass.RemTranExtra().get_threshold_3d_min(currency);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        private string get3DThresholdFlag()
        {
            return new IRSWorld.TranClass.RemTranExtra().get_threshold_3d_flag();
        }
        /* 3D THRESHOLD END */

    }
}