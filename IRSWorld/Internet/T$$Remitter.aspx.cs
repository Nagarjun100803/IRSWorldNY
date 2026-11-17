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

namespace IRSWorld.Internet
{

    public partial class T__Remitter : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(T__Remitter));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(rem_other_details));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(rem_modal_check_payments));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(rem_ach_payments));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(RemTranSaving));
            Session["InternetARMModule"] = "ONLINE";
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
            string sReturn = new irsAccountInquiry().getFC(AccountNo, irsAccountInquiry.FCMethod.AccountStatusAndBalInquiry, 0, true);
            if (sReturn.ToUpper().IndexOf("ERROR") >= 0 || sReturn.ToUpper().IndexOf("FAILED") >= 0)
            {
                sReturn = "There was a problem retrieving your account information. Please report to the nearest PNB branch and/or system administrator.";
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
        public string getRemainingRemittance(string remitterid)
        {
            return new irsTDD().getRemainingRemittance(remitterid, getLocation()).ToString();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getTotalRemittance()
        {
            string remitterCIFID = ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).CIFID;
            return new irsTDD().getTotalRemittance(remitterCIFID, getLocation()).ToString();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string submitClearingDays(int txn_seqid, string data)
        {
            return new irsTDD().submitClearingDays(txn_seqid, data);
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
        public DataTable getBeneficiaryInfo(string RemitterCIFID)
        {
            return new irsTDD().getBeneficiaryInfo(RemitterCIFID);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getProducts(string RemitterCIFID,string BeneCIFID)
        {
            return new irsTDD().getProducts(RemitterCIFID, BeneCIFID, this.getUserRole());
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
        public DataTable getDCCard(string CIFID)
        {
            return new irsInternetPage().getDCCard(CIFID);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getDefaults(string BranchCode)
        {
            return new irsInternetPage().getDefaults(BranchCode);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getDisclaimer()
        {
            return new irsInternetPage().getDisclaimer(getRegion(true));
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getDisclaimer2(string productcode)
        {
            return new irsTDD().getDisclamer(productcode);
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
            bPaymentGateway = new irsTDD().getPaymentGateway(this.getRegion(true));
            writeAudit("PAYMENT GATEWAY: [Region=" + this.getRegion(false) + "][ReturnValue=" + bPaymentGateway.ToString() + "]", "", "", csAuditLog.Category.Select);
            return bPaymentGateway;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string submiteXactPaymentGateway(string BranchCode, string CIFID, string CardNo, string CardName, string ExpiryDate, string SecurityCode, string CardType, string CurrencyCode, string Amount, string Remarks)
        {
            string sReturn = string.Empty;
            string[] arrReturn;

            this.verifyOAAM(CardNo, CurrencyCode, Amount, TransactionType.PaymentGateway);
            writeAudit("SWIPE CARD: [Region=" + getRegion(true) + "]" + "[BranchCode=" + BranchCode + "]" + "[CIFID=" + CIFID + "]" + "[CardNo=" + CardNo + "]" + "[CardName=" + CardName + "]" + "[ExpiryDate=" + ExpiryDate + "]" + "[CardType=" + CardType + "]" + "[Currency=" + CurrencyCode + "]" + "[Amount=" + Amount + "]" + "[Remarks=" + Remarks + "]", "", "", csAuditLog.Category.PaymentGateway);

            //sReturn = new irsPaymentGateway().createRequest(getRegion(true), BranchCode, CIFID, CardNo, CardName, ExpiryDate, SecurityCode, CardType, CurrencyCode, Amount, Remarks, getBaseSiteUrl());

            sReturn = "EXACT|";

            arrReturn = sReturn.Split('|');
            if (arrReturn[0] == "OK")
            {
                this.updateOAAM(true, "", "");
            }
            else
            {
                if (arrReturn[0] != "3DSECURE")
                {
                    this.updateOAAM(false, "", "");
                }
                if (arrReturn[0] == "EXACT")
                {
                    Session["EXACT_AMOUNT"] = Amount;
                    Session["EXACT_REFNO"] = new irsPaymentGateway().generateReference(BranchCode);
                }
            }
            return sReturn;
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
            return Convert.ToString(((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).Role);
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
            DataTable dt = new irsInternetPage().getRemittance(ReferenceNo);
            CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

            if (dt.Rows.Count > 0)
            {
                sRemitterCIFID = dt.Rows[0]["RemitterCIFID"].ToString().Trim();
            }
            if (new irsInternetPage().getRemitterEmail(sRemitterCIFID) != string.Empty)
            {
                rpt = new TranClass.CrystalReportUtil().generate_otc_report(getLocation(), Txn_SEQID);
                bReturn = new CrystalReportUtil().OREmailSending(rpt, ReferenceNo, sRemitterCIFID, out sError);
            }
            else
            {
                sError = "No registered email address. Please contact PNB for more info.";
            }
            writeAudit("SEND OR EMAIL: [ReferenceNo=" + ReferenceNo + "][RemitterCIFID=" + sRemitterCIFID + "][Response=" + bReturn.ToString() + "][ErrorMessage=" + sError + "]", "", "", csAuditLog.Category.Email);
            return sError;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string generate_receipt_popup(string ReferenceNo, string Txn_SEQID)
        {
            string sReturn = string.Empty;
            bool bReturn = false;
            string sRemitterCIFID = string.Empty;
            string sError = string.Empty;
            DataTable dt = new irsInternetPage().getRemittance(ReferenceNo);
            CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

            if (dt.Rows.Count > 0)
            {
                sRemitterCIFID = dt.Rows[0]["RemitterCIFID"].ToString().Trim();
                rpt = new TranClass.CrystalReportUtil().generate_otc_report(getLocation(), Txn_SEQID);
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

            this.verifyOAAM(CardNo, CurrencyCode, Amount, TransactionType.PaymentGateway);
            writeAudit("SWIPE CARD: [Region=" + getRegion(true) + "]" + "[BranchCode=" + BranchCode + "]" + "[CIFID=" + CIFID + "]" + "[CardNo=" + CardNo + "]" + "[CardName=" + CardName + "]" + "[ExpiryDate=" + ExpiryDate + "]" + "[CardType=" + CardType + "]" + "[Currency=" + CurrencyCode + "]" + "[Amount=" + Amount + "]" + "[Remarks=" + Remarks + "]", "", "", csAuditLog.Category.PaymentGateway);
            sReturn = new irsPaymentGateway().createRequest(getRegion(true), BranchCode, CIFID, CardNo, CardName, ExpiryDate, SecurityCode, CardType, CurrencyCode, Amount, Remarks, getBaseSiteUrl());
            arrReturn = sReturn.Split('|');
            if (arrReturn[0] == "OK")
            {
                this.updateOAAM(true, "", "");
            }
            else
            {
                if (arrReturn[0] != "3DSECURE")
                {
                    this.updateOAAM(false, "", "");
                }
                if (arrReturn[0] == "ENETS")
                {
                    Session["ENETS_REQUEST"] = arrReturn[1];
                    Session["ENETS_URL"] = arrReturn[2];
                }
            }
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

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string verifyOAAMTransaction(string CreditAccount, string Currency, string Amount)
        {
            return this.verifyOAAM(CreditAccount, Currency, Amount, TransactionType.FundTransfer);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string verifyOAAM(string CreditAccountID, string Currency, string Amount, TransactionType TranType)
        {
            string sOAAM = new irsConfiguration().appsetting("OAAM_DOWN");
            bool bOAAM;
            int number;
            bool result;
            string DebitAccount = string.Empty;
            string CreditAccount = string.Empty;

            Object[] oaam_params = null;
            writeAudit("CHECK OAAM CONFIG VALUE: [OAAM=" + sOAAM + "]", "", "", csAuditLog.Category.OAAM);
            if (sOAAM != "true")
            {
                try
                {
                    result = int.TryParse(Currency, out number);
                    TranClass.rem_debit_account_screen OAAM = new rem_debit_account_screen();
                    DebitAccount = new irsInternetPage().getBranchNSVS(getLocation(), (result == true ? Convert.ToInt32(Currency) : Convert.ToInt32(new irsInternetPage().getLookupValue("Currency", "FldCode", Currency, "FldValue"))));
                    if (TranType == TransactionType.PaymentGateway)
                    {
                        CreditAccount = CreditAccountID;
                    }
                    else
                    {
                        CreditAccount = new irsInternetPage().getCIFAccount(CreditAccountID);
                    }
                    bOAAM = OAAM.oaam_verify_transaction(TranType.ToString(), DebitAccount, CreditAccount, Amount.Replace(",", ""), out oaam_params);
                    Session["OAAM_Parameters"] = oaam_params;
                    writeAudit("WRITE TO OAAM (VERIFY): [Transaction Type=" + TranType.ToString() + "][Debit Account=" + DebitAccount + "][Credit Account=" + CreditAccount + "][AccountSEQID=" + CreditAccountID + "][Currency=" + Currency + "][Amount=" + Amount + "][ReturnValue=" + bOAAM.ToString() + "]", "", "", csAuditLog.Category.OAAM);
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
        public int submitRemittance(string Remittance)
        {
            writeAudit("CREATE REMITTANCE: " + Remittance, "", "", csAuditLog.Category.Insert);
            return new irsTDD().submitRemittance(Remittance);
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
        public string getWebRemittanceChargeType()
        {
            DataTable dt = new irsInternetPage().getSubLookup(getLocation(), "ChargeType");
            string wr = "";
            try
            {
                DataRow[] dr = dt.Select("FldDescription='Web Remittance - RCI'");

                wr = dr[0]["FldValue"].ToString();
                //wr = dt.Select("")[0]["FldValue"].ToString();
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
        public DataTable getBranchSetup()
        {
            return new irsTDD().getBranchSetup(getLocation());
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String check_watchlist(string cifid, bool isbene)
        {
            sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = null;
            string sReturn = string.Empty;
            try
            {
                ds = commons.getDataSet(commons.getSqlCommand("SELECT * FROM TBL_CIF_MASTER WITH (NOLOCK) WHERE CIFID ='" + cifid + "'", false), "TBL_INT_WATCHLIST");
                sReturn = this.get_hitlist(ds.Tables[0].Rows[0]["FirstName"].ToString(), ds.Tables[0].Rows[0]["LastName"].ToString(), ds.Tables[0].Rows[0]["MiddleName"].ToString(), getLocation());
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
        public void endEXACTSession()
        {
            Session["EXACT_RESPONSE"] = null;
            Session["EXACT_AMOUNT"] = null;
            Session["EXACT_REFNO"] = null;
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

                DataTable dt = commons.getDataSet(commons.getSqlCommand(s, false)).Tables[0];

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
        public bool first_time_remitter(string remitterid)
        {
            return new irsTDD().is_first_time_remitter(remitterid);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool check_if_id_approved(string remitterid)
        {
            sqlHelperWs sql = new sqlHelperWs();
            DataTable dt = sql.getDataSet(sql.getSqlCommand("select top 1 * from tbl_cif_master where cifid='" + remitterid + "'", false)).Tables[0];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    return Convert.ToBoolean(dt.Rows[0]["ApprovedDocument"]);
                }
                return false;
            }
            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool connect_achworks(string remitterid,string accountno,string routingno, string bankname,string swift,string amount)
        {
            return new ACHWorks().connect_achworks(remitterid,accountno, routingno, bankname, swift,amount);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getChargeTypePerTargetCurrency(string valuename,string branchcode)
        {
            sqlHelperWs sql = new sqlHelperWs();
            DataTable dt = sql.getDataSet(sql.getSqlCommand("select * from tbl_branch_defaults where branchcode='"+ branchcode +"' AND BranchDefValue='"+ valuename +"'", false)).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return Convert.ToString(dt.Rows[0]["BranchDefCode"]);
            }
            return string.Empty;
        }





        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool send_email_transaction(string remitterid)
        {
            //irsTDD tdd = new irsTDD();
            //csMailer mailer = new csMailer();
            //string name = plist[2] + " " + plist[3] + " " + plist[1];
            //string UserID = plist[0].ToString();
            //string errmsg = "";
            //string encpass = getEncryPass(UserID).ToString();
            ////string password = tdd.DecryptPassword(encpass.ToString());
            //HttpContext context = HttpContext.Current;
            //string baseUrl = context.Request.Url.Scheme + "://" + context.Request.Url.Authority + context.Request.ApplicationPath.TrimEnd('/');

            //string Subject = "PNB Web Remittance Online Enrollment/Registration Summary.";
            //string email = getEmailAdd();
            //String[] arrEmail = { email };
            //string msg = " Congratulations   <i>" + name + "</i> <br><br>";
            //msg = msg + " You have successfully finished the PNB Remittance Center, Inc. Online Registration Process <br>";
            //msg = msg + " Please print a copy of this email for your reference <br><br>";
            //msg = msg + " Username: " + UserID + "<br>";
            //msg = msg + " Email: " + email + "<br><br>";
            //msg = msg + " Temporary Password: <br><br>";
            //msg = msg + " Please click on the link below to activate your web remittance account. <br><br>";
            //msg = msg + " <a href='" + baseUrl + "/Admin/Login.aspx?uid=" + UserID + "'>here</a><br>";
            //msg = msg + " For any concern, you may call 888-762-76363.<br><br>";
            //msg = msg + " Sincerely yours, <br>";
            //msg = msg + " Customer Support Department <br>";
            //msg = msg + " PNB Remittance Center, Inc, Online <br><br><br>";
            //msg = msg + " THIS IS A COMPUTER GENERATED MESSAGE <br>";
            //msg = msg + " NO REPLY REQUIRED <br>";

            //try
            //{
            //    return mailer.sendMail(Subject, arrEmail, null, msg, null, true);
               
            //}
            //catch (Exception ex)
            //{
            //    errmsg = ex.Message;
            //}
            //finally { }
            return false;
        }


        /*ACHWorks*/
        //[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        //public string connection_check()
        //{
        //    return new irsACHWorks().connection_check();
        //}

        /*END AchWorks*/

    }
}
