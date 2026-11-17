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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using IRSClassLib;

// ********************************************************************************** //
// *** DEVELOPER    : Ronald James L. Tuazon                                      *** //
// *** DATE CREATED : December 11, 2009                                           *** //
// *** DATE MODIFIED: June 28, 2010                                               *** //
// *** DESCRIPTION  : Automated Remittance Machine - Web                          *** //
// *** ADDITIONALS  : ARM.js, ARMMain.aspx, irsARM.cs, irsPaymentGateway, ARM.css *** //
// ********************************************************************************** //

namespace IRSWorld.Main
{
    [Serializable]
    public partial class ARMMain : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(ARMMain));
            Session["InternetARMModule"] = "ARM";
            if (Session["SessionID"] == null)
            {
                Session["SessionID"] = DateTime.Now.ToString("yyyyMMddHHmmss");
            }
            this.setUserID();
            writeAudit("SUCCESSFUL LOGON", "", "", csAuditLog.Category.Logon);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void setUserID()
        {
            irsARM clArm = new irsARM();
            clArm.setUserID(Session["GFMCNo"].ToString());
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getARMLimit()
        {
            return new irsARM().getARMLimit(getBranchCode());
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getBeneficiaryAddress(string CIFID)
        {
            return new irsARM().getBeneficiaryAddress(CIFID);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getBaseSiteUrl()
        {
            HttpContext context = HttpContext.Current;
            string baseUrl = context.Request.Url.Scheme + "://" + context.Request.Url.Authority + context.Request.ApplicationPath.TrimEnd('/') + '/';
            return baseUrl;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getBranchCode()
        {
            irsARM clArm = new irsARM();
            clArm.setLocation(Session["BranchCode"].ToString());
            return Session["BranchCode"].ToString();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getCharges(string Amount, string BranchCode, string ProductCode, string ImplementationType, string SourceCurrency, string TargetCurrency, string ChargeType, string PayMode, string ComputationLevel, bool IsRemittance)
        {
            string sChargesID = new irsInternetPage().getChargesID(BranchCode, ProductCode, ImplementationType, SourceCurrency, ChargeType, PayMode);
            string sReturn = string.Empty;
            writeAudit("GET CHARGE ID: [ChargeID=" + sChargesID + "][BranchCode=" + BranchCode + "][ProductCode=" + ProductCode + "][ImplementationType=" + ImplementationType + "][SourceCurrency=" + SourceCurrency + "][TargetCurrency=" + TargetCurrency + "][ChargeType=" + ChargeType + "][PayMode=" + PayMode + "][ComputationLevel=" + ComputationLevel + "][IsRemittance=" + IsRemittance + "][Amount=" + Amount + "]", "", "", csAuditLog.Category.Select);
            if (sChargesID == string.Empty)
            {
                return "NOCHARGE";
            }
            else
            {
                sReturn = new irsInternetPage().getCharges(Amount, Convert.ToInt32(sChargesID), Convert.ToInt32(TargetCurrency), Convert.ToInt32(ComputationLevel), false, IsRemittance, false, "") + sChargesID;
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
        public DataTable getDefaults(string BranchCode)
        {
            return new irsInternetPage().getDefaults(BranchCode);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getExchangeRate()
        {
            return new irsARM().getExchangeRate(this.getLocalTime());
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool getGFCAggregate(string BeneCIFID, string AccountNo, int Currency, string Amount)
        {
            bool isGFCAggregate = false;
            //isGFCAggregate = new irsGFC().checkGFCAggregation(AccountNo, float.Parse(Amount));
            isGFCAggregate = new irsLinkToGFC().check_GFC_Aggregate(BeneCIFID, AccountNo, Currency, float.Parse(Amount));
            writeAudit("CHECK GFC AGGREGATE: [Beneficiary ID=" + BeneCIFID + "][Account Number=" + AccountNo + "][Currency=" + Currency.ToString() + "][Amount=" + Amount + "][ReturnValue=" + Convert.ToString(isGFCAggregate) + "]", "", "", csAuditLog.Category.Select);
            return isGFCAggregate;
        }
         
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getGFMC()
        {
            return Session["GFMCNo"].ToString();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getGFMCBeneficiary()
        {
            return new irsARM().getGFMCBeneficiary();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getGFMCRemitter()
        {
            return Session["RemitterName"].ToString();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getKioskCurrency()
        {
            return Session["DefaultCurrency"].ToString();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getLocalTime()
        {
            DateTime dtLocalTime = new irsTimeZone().branchLocalTime(getBranchCode());
            Session["ARMLocalTime"] = dtLocalTime;
            return Convert.ToString(dtLocalTime);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getLookup(string GroupName)
        {
            return new irsInternetPage().getLookup(GroupName);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getMerchants(string RemitterID)
        {
            return new irsARM().getMerchants(RemitterID);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getMerchantLookup(string MerchantName)
        {
            return new irsARM().getMerchantLookup(MerchantName);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getMerchantInfo(string MerchantName, string ProductCode)
        {
            return new irsARM().getMerchantInfo(MerchantName, ProductCode);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getORNo()
        {
            return Session["ORNo"].ToString();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getReferenceNo(string ORNo, string TranxDate)
        {
            return new irsARM().getReferenceNo(ORNo, Convert.ToDateTime(TranxDate));
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getRemitterCards()
        {
            return new irsARM().getRemitterCards();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getRemitterID()
        {
            return Session["RemitterID"].ToString();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getServerTime()
        {
            //DateTime dtServerTime = new irsTimeZone().branchLocalTime(new irsConfiguration().appsetting("ServerBranchCode"));
            DateTime dtServerTime = new irsTimeZone().branchLocalTime(new irsInternetPage().getORDHOBranch(true));
            return Convert.ToString(dtServerTime);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getSourceCurrency()
        {
            getExchangeRate();
            return Session["SourceCurrency"].ToString();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getSourceCurrencyFldCode()
        {
            getExchangeRate();
            return Session["SourceCurrencyFldCode"].ToString();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getSourceCurrencyCode()
        {
            getExchangeRate();
            return Session["SourceCurrencyCode"].ToString();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getTargetCurrencyCode()
        {
            getExchangeRate();
            return Session["TargetCurrencyCode"].ToString();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getTransactionDate()
        {
            DateTime dtTransactionDate = new irsInternetPage().getTransactionDate(getBranchCode(), true);
            return Convert.ToString(dtTransactionDate);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getTransactionPurpose()
        {
            return new irsARM().getTransactionPurpose();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getTransactionSource()
        {
            return new irsARM().getTransactionSource();
        }

        private string loadReport(DataTable dt)
        {
            try
            {
                if (dt != null)
                {
                    //IRSWorld.Reports.ARM.rptARMReceipt rptReceipt = new IRSWorld.Reports.ARM.rptARMReceipt();
                    //ConnectionInfo crConnectionInfo = new ConnectionInfo();
                    //Database crDatabase;
                    //Tables crTables;
                    //TableLogOnInfos crTableLogoninfos = new TableLogOnInfos();
                    //TableLogOnInfo crTableLogoninfo = new TableLogOnInfo();
                    //string sServerName = new irsARM().getAppConfigValue(new irsSqlCommons().ConnectionString, "Server=".ToLower(), ";");
                    //string sDatabaseName = new irsARM().getAppConfigValue(new irsSqlCommons().ConnectionString, "Database=".ToLower(), ";");
                    //string sUserID = new irsARM().getAppConfigValue(new irsSqlCommons().ConnectionString, "User=".ToLower(), ";");
                    //string sPassword = new irsARM().getAppConfigValue(new irsSqlCommons().ConnectionString, "Password=".ToLower(), ";");

                    //crConnectionInfo.ServerName = sServerName;
                    //crConnectionInfo.DatabaseName = sDatabaseName;
                    //crConnectionInfo.UserID = sUserID;
                    //crConnectionInfo.Password = sPassword;

                    //crDatabase = rptReceipt.Database;
                    //crTables = crDatabase.Tables;
                    //crTableLogoninfo.ConnectionInfo = crConnectionInfo;
                    //crTableLogoninfos.Add(crTableLogoninfo);
                    //rptReceipt.SetDataSource(dt);
                    //this.crvARM.ReportSource = rptReceipt;
                    //this.crvARM.RefreshReport();
                    ////rptReceipt.PrintToPrinter(1, false, 0, 0);
                    return "1";
                }
                else
                {
                    return "0";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable printReceipt(string ReferenceNo)
        {
            //string[] param = new string[1];
            //param[0] = ReferenceNo;
            return new irsARM().printReceipt(ReferenceNo);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string setMerchantORNo(string MerchantCode, string ProductCode)
        {
            string merchantORNo = string.Empty;
            merchantORNo = new irsOBPSTransactions().OBPSRefno(MerchantCode, ProductCode, getBranchCode());
            writeAudit("CREATE MERCHANT ORNO: [MerchantCode=" + MerchantCode + "][ProductCode=" + ProductCode + "][GeneratedORNo=" + merchantORNo + "]", "", "", csAuditLog.Category.Select);
            return merchantORNo;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string submitPaymentGateway(string BranchCode, string CIFID, string CardNo, string CardName, string ExpiryDate, string SecurityCode, string CardType, string Currency, string Amount, string Remarks)
        {
            writeAudit("SWIPE CARD: [Region=" + System.Web.HttpContext.Current.Session["ChargeGroup"].ToString() + "]" + "[BranchCode=" + BranchCode + "]" + "[CIFID=" + CIFID + "]" + "[CardNo=" + CardNo + "]" + "[CardName=" + CardName + "]" + "[ExpiryDate=" + ExpiryDate + "]" + "[CardType=" + CardType + "]" + "[Currency=" + Currency + "]" + "[Amount=" + Amount + "]" + "[Remarks=" + Remarks + "]", "", "", csAuditLog.Category.PaymentGateway);
            string sReturn = new irsPaymentGateway().createRequest(System.Web.HttpContext.Current.Session["ChargeGroup"].ToString(), BranchCode, CIFID, CardNo, CardName, ExpiryDate, SecurityCode, CardType, Currency, Amount, Remarks, getBaseSiteUrl());
            return sReturn;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public int submitRemittance(string Remittance)
        {
            writeAudit("CREATE REMITTANCE: " + Remittance, "", "", csAuditLog.Category.Insert);
            return new irsARM().submitRemittance(Remittance);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public int submitOBPS(string[] OBPS)
        {
            string BP = string.Empty;
            int i = 0;

            for (i = 0; i<= (OBPS.Length - 1); i++)
            {
                BP += OBPS.GetValue(i).ToString() + "|";
            }
            writeAudit("CREATE BILLS PAYMENT: " + BP, "", "", csAuditLog.Category.Insert);
            return new irsARM().submitOBPS(OBPS);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string submitRemittanceCharges(string SEQID)
        {
            irsCalculator irsCalc;
            string sReturn = "0";

            try
            {
                if (System.Web.HttpContext.Current.Session["ARM_Calculator"] != null)
                {
                    irsCalc = (irsCalculator)System.Web.HttpContext.Current.Session["ARM_Calculator"];
                    irsCalc.commit_remittance_charge_details(Convert.ToInt32(SEQID));
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
        public string submitExcessPayment(string ReferenceNo, string TargetAmount, string ActualAmount)
        {
            string sReturn = "0";
            bool bReturn = false;
            try
            {
                irsOBPSTransactions irsOBPS = new irsOBPSTransactions();
                bReturn = irsOBPS.saveOBPExcess(ReferenceNo, TargetAmount, ActualAmount);
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
            writeAudit("SAVE REMITTANCE EXCESS PAYMENT: [ReferenceNo=" + ReferenceNo + "][TargetAmount=" + TargetAmount + "][ActualAmount=" + ActualAmount + "][ReturnValue=" + sReturn + "]", "", "", csAuditLog.Category.Insert);
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
        public void writeLog(string Description, string Type)
        {
            switch (Type)
            {
                case "EXCHANGERATE":
                    writeAudit(Description, "", "", csAuditLog.Category.Select);
                    break;
                case "PAYMENTGATEWAY":
                    writeAudit(Description, "", "", csAuditLog.Category.PaymentGateway);
                    break;
                case "LOGOFF":
                    writeAudit("SUCCESSFUL LOGOFF", "", "", csAuditLog.Category.Logoff);
                    break;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void writeAudit(string Description, string NewValue, string OldValue, csAuditLog.Category Category)
        {
            string sDescription = string.Empty;
            if (Description == string.Empty)
            {
                sDescription = "Location=" + Session["BranchCode"].ToString();
            }
            else
            {
                sDescription = "Location=" + Session["BranchCode"].ToString() + " : " + Description;
            }
            new csAuditLog().writeAudit(Session["GFMCNo"].ToString(), "ARM", sDescription, NewValue, OldValue, Category);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void writeLogger(string Log, string TableName, string Key)
        {
            new csAuditLog().writeLogger(Log, TableName, Key);
        }
    }
}