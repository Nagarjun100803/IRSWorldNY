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

namespace IRSWorld.ARS
{
    [Serializable]
    public partial class ARSW : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(ARSW));
            //Session["ARSWREMITTERID"] = "1ZRS00000001";
            //Session["ARSWBENEFICIARYID"] = "2ZRS00000001";
            //Session["ARSWPRODUCTID"] = "3162150";
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getSession()
        {
            if (Session["ARSWREMITTERID"] == null)
            {
                return "0";
            }
            return "1";
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getLocation()
        {
            return ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).Location;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getUserID()
        {
            return ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).UserId;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getTranxDate()
        {
            DateTime dt = new irsTimeZone().branchLocalTime(getLocation());
            return dt.ToString("yyyy-MM-dd");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DateTime getLocalDT()
        {
            DateTime dt = new irsTimeZone().branchLocalTime(getLocation());
            return dt;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DateTime getServerDT()
        {
            DateTime dt = new irsTimeZone().branchLocalTime("1072");
            return dt;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void writeAudit(string Description, string Module, string NewValue, string OldValue, csAuditLog.Category Category)
        {
            string sDescription = "Location=" + getLocation() + " : " + Description;
            string sModule = "ARSW Page";
            if (Module != string.Empty)
            {
                sModule = Module;
            }
            new csAuditLog().writeAudit("", sModule, sDescription, NewValue, OldValue, Category);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getPreReceiptHeader()
        {
            return new irsInternetPage().getBranchOperations(new irsInternetPage().getBranchValue(this.getLocation(), "BranchCode", this.getLocation(), "SEQ_ID"));
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getRemittingCurrency()
        {
            return new irsARSW().getRemittingCurrency(this.getLocation());
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getPaymentMethod()
        {
            return new irsARSW().getPaymentMethod(this.getLocation());
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getPurpose()
        {
            return new irsARSW().getPurpose();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getCIFInformation()
        {
            if (Session["ARSWREMITTERID"] != null && Session["ARSWBENEFICIARYID"] != null)
            {
                string sRemitterID = Session["ARSWREMITTERID"].ToString();
                string sBeneficiaryID = Session["ARSWBENEFICIARYID"].ToString();
                return new irsARSW().getCIFInformation(sRemitterID, sBeneficiaryID);
            }
            else
            {
                return null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getSelectedProduct()
        {
            if (Session["ARSWPRODUCTID"] != null)
            {
                return Session["ARSWPRODUCTID"].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getChargeType(string ProductCode)
        {
            return new irsARSW().getChargeType(this.getLocation(), ProductCode);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getForex(string RemittingCurrency, string TargetCurrency)
        {
            return new irsARSW().getForex(this.getLocation(), RemittingCurrency, TargetCurrency);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getCharges(string Amount, string Computation, string ProductCode, string ImplementationType, string SourceCurrency, string TargetCurrency, string ChargeType, string PayMode, bool ReCompute)
        {
            string sChargesID = new irsInternetPage().getChargesID(this.getLocation(), ProductCode, ImplementationType, SourceCurrency, ChargeType, PayMode);
            Session["ARSWCHARGESID"] = sChargesID;
            if (sChargesID != string.Empty)
            {
                return new irsARSW().getCharges(Amount, sChargesID, TargetCurrency, Computation, ReCompute) + ReCompute;
            }
            else
            {
                return "ERROR";
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool setOverride(string OldFXRates, string NewFXRates, string OldCommission, string NewCommission)
        {
            string sOverride = string.Empty;
            sOverride = "FOREX_RATES" + "~" + "remitting_to_account_rate" + "~" + NewFXRates + "|";
            sOverride += "FOREX_RATES" + "~" + "account_to_remitting_rate" + "~" + 1 / Convert.ToDecimal(NewFXRates) + "|";
            sOverride += "CHARGES_DATA" + "~" + "1~" + NewCommission;
            //writeAudit("OVERRIDE: [FXRates=" + OldFXRates + " to " + NewFXRates + "][Commission=" + OldCommission + " to " + NewCommission + "][OverrideParameter=" + sOverride + "]", "", "", csAuditLog.Category.Update);
            IRSWorld.TranClass.RemTranCommons irsOverride = new IRSWorld.TranClass.RemTranCommons();
            irsOverride.set_calculator_override_values(sOverride, "ARSWCALCULATOR");
            return true;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getRemittanceAggregate(string RemittingCurrency, string RemitterID)
        {
            return new irsARSW().getRemittanceAggregate(this.getLocation(), this.getLocalDT().ToString(), RemittingCurrency, RemitterID);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool validateOBP(string AccountNo)
        {
            return new irsOBPSTransactions().fOBPAccount(AccountNo);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string validateAccountStatus(string AccountNo, string BeneficiaryID, string Currency, string Amount)
        {
            string sReturn = new irsAccountInquiry().getFC(AccountNo, irsAccountInquiry.FCMethod.AccountStatusAndBalInquiry, 0, true);
            string sMessage = string.Empty;
            if (sReturn.ToUpper().IndexOf("ERROR") >= 0)
            {
                sMessage = "1|ERROR";
            }
            else
            {
                if (sReturn.ToUpper().IndexOf("FAILED") >= 0)
                {
                    sMessage = "2|FAILED";
                }
                else
                {
                    if (sReturn.ToUpper().IndexOf("DORMANT") >= 0)
                    {
                        sMessage = "3|DORMANT";
                    }
                    else
                    {
                        if (sReturn.ToUpper().IndexOf("CLOSED") >= 0)
                        {
                            sMessage = "4|CLOSED";
                        }
                        else
                        {
                            if (sReturn.ToUpper().IndexOf("INVALID") >= 0)
                            {
                                sMessage = "5|INVALID";
                            }
                            else
                            {
                                bool isGFCAggregate = false;
                                isGFCAggregate = new irsLinkToGFC().check_GFC_Aggregate(BeneficiaryID, AccountNo, Convert.ToInt32(Currency), float.Parse(Amount));
                                //writeAudit("CHECK GFC AGGREGATE: [Beneficiary ID=" + BeneCIFID + "][Account Number=" + AccountNo + "][Currency=" + Currency.ToString() + "][Amount=" + Amount + "][ReturnValue=" + Convert.ToString(isGFCAggregate) + "]", "", "", csAuditLog.Category.Select);
                                if (!isGFCAggregate)
                                {
                                    sMessage = "6|AGGREGATE";
                                }
                                else
                                {
                                    sMessage = "0|VALID";
                                }
                            }
                        }
                    }
                }
            }
            return sMessage;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool validateAgentAccount(string Amount, string Currency)
        {
            return true;
            string sAccountNo = new irsInternetPage().getAgentAccountBalance(this.getLocation(), Convert.ToInt32(Currency));
            bool ret = false;
            if (sAccountNo != string.Empty)
            {
                if (new irsAccountInquiry().getAgentAccountBalance(sAccountNo, Convert.ToDouble(Amount)) == true)
                {
                    ret = true;
                }
                else
                {
                    ret = false;
                }
            }
            else
            {
                ret = false;
            }
            //writeAudit("GET AGENT ACCOUNT BALANCE: [Amount=" + Amount.ToString() + "][AccountNo=" + sAccountNo + "][ReturnValue=" + ret.ToString() + "]", "", "", csAuditLog.Category.Select);
            return ret;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string submitTransaction(string RemitterID, string BeneficiaryID, string AccountSEQID, string TranxPurpose, string RemitterAddressSEQID, string RemitterContactSEQID, string BeneficiaryAddressSEQID, string BeneficiaryContactSEQID, string ClearingDate, string CheckNumber)
        {
            string ret = string.Empty;
            string ProductChargesID = string.Empty;
            if (Session["ARSWCHARGESID"] == null)
            {
                return "False|ERROR4: Charges ID expired.";
            }
            else
            {
                ProductChargesID = Session["ARSWCHARGESID"].ToString();
            }
            ret = new irsARSW().remit(this.getUserID(), "8", this.getLocation(), this.getLocalDT().ToString(), ProductChargesID, RemitterID, BeneficiaryID, AccountSEQID, TranxPurpose, this.getLocalDT().ToString(), this.getServerDT().ToString(), RemitterAddressSEQID, RemitterContactSEQID, BeneficiaryAddressSEQID, BeneficiaryContactSEQID, ClearingDate, CheckNumber);
            return ret;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getPreReceipt(string ReferenceNo)
        {
            return new irsARSW().getPreReceipt(ReferenceNo);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string sendOREmail(string ReferenceNo, string SEQID)
        {
            return new Internet.InternetPage().sendOREmail(ReferenceNo, SEQID);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getTransactionLimitForKeepingLogs()
        {
            return new irsARSW().getBranchSetup(this.getLocation());
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void redirectToCIF(string Module)
        {
            Session["ARSWMODULE"] = Module;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string closeSession()
        {
            Session["ARSWREMITTERID"] = null;
            Session["ARSWBENEFICIARYID"] = null;
            Session["ARSWPRODUCTID"] = null;
            Session["ARSWCHARGESID"] = null;
            Session["ARSWMODULE"] = null;
            return string.Empty;
        }
    }
}