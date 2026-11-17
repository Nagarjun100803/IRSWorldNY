using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Text;
using System.IO;
using System.Xml;
using System.Linq;
using System.Text.RegularExpressions;

namespace IRSClassLib
{
    public class irsAccountInquiry : sqlHelperWs
    {
        #region enum
        public enum FCMethod
        {
            BalanceInquiry,
            LastTransactionInquiry,
            HoldInquiry,
            FloatInquiry,
            AccountStatusInquiry,
            KBItoFC,
            GFCtoCK,
            AccountStatusAndBalInquiry,
            FCCIF,
            AccountStatusCode,
            /* E2EAPICHANGE 20220209 Removed AccountStatusAndName */
            AmountInquiry
        }

        public enum AccountType
        {
            Savings = 1,
            Current = 2,
            CashCard = 3,
            GFC = 4,
            TimeDeposit = 5
        }
        #endregion

        #region variables
        /* E2EAPICHANGE 20220209 Removed csFlexcubeTeller, _totalHold, _unclearBalance, _availableBalance, _currentBalance, _iAccount, _openDate, _sAccount */
        string _siAccount = string.Empty;
        string _acctStatus = string.Empty;
        string _acctBalance = string.Empty;
        string _acctType = string.Empty;
        string[] _arrPrintResponse = new string[20];
        bool _isST = false;
        int _acctTypeVal = 0;
        /* E2EAPICHANGE 20220209 Added E2EWSConnector */
        E2EAPIConnector.E2EAPIConnector _e2e = new E2EAPIConnector.E2EAPIConnector();
        #endregion

        #region Validation Systematics
        /// <summary>
        /// Method will validate SI accounts.
        /// </summary>
        public string isAccountValid(string AccountNo)
        {
            bool v = false;
            string ret = string.Empty;

            try
            {
                v = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["ACCOUNT_VALID"].ToString());
                /* E2EAPICHANGE 20220209 Updated Code */
                ret = this.isAccountValid(AccountNo, v);
            }
            catch (Exception ex)
            {
                this.writeAudit("METHOD: isAccountValid | PARAMETER(S): " + AccountNo + " | EXCEPTION: " + ex.Message, "isAccountValid", string.Empty, csAuditLog.Category.Systematics);
                return "0|" + ex.Message;
            }
            this.writeAudit("METHOD: isAccountValid | PARAMETER(S) " + AccountNo + " | RETURN: " + ret, "isAccountValid", string.Empty, csAuditLog.Category.Systematics);
            return ret;
        }

        /* RFS BLOCK TIME DEPOSIT */
        public bool isAccountTD(string AccountNo)
        {
            string FirstTwo = string.Empty;
            string Seventh = string.Empty;
            bool ret = false;

            try
            {
                if (AccountNo.Trim().Length == 16)
                {
                    FirstTwo = AccountNo.Substring(0, 2);
                    Seventh = AccountNo.Substring(6, 1);

                    if ((FirstTwo == "01" || FirstTwo == "02" || FirstTwo == "03") && (Seventh == "6" || Seventh == "7" || Seventh == "8"))
                    {
                        ret = true;
                        this.writeAudit("METHOD: isAccountTD | PARAMETER(S): " + AccountNo + " | Account is a TD account", "isAccountTD", string.Empty, csAuditLog.Category.Systematics);
                    }
                }
            }
            catch (Exception ex)
            {
                this.writeAudit("METHOD: isAccountTD | PARAMETER(S): " + AccountNo + " | EXCEPTION: " + ex.Message, "isAccountTD", string.Empty, csAuditLog.Category.Systematics);
            }
            return ret;
        }
        #endregion

        #region Common Functions
        /* E2EAPICHANGE 20220209 Added Functions */
        public string UATDateSwitch()
        {
            string ret = string.Empty;
            base.EnforceConstraint = false;

            try
            {
                if (System.Web.HttpContext.Current.Session["UATDateSwitch"] != null)
                {
                    if ((string)System.Web.HttpContext.Current.Session["UATDateSwitch"] != string.Empty)
                    {
                        ret = (string)System.Web.HttpContext.Current.Session["UATDateSwitch"];
                    }
                }
                else
                {
                    DataTable dt = new irsInternetPage().getLookup("AccountInquiryDate");
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        System.Web.HttpContext.Current.Session["UATDateSwitch"] = dt.Rows[0]["FldCode"].ToString() + "|" + dt.Rows[0]["FldDescription"].ToString();
                        ret = dt.Rows[0]["FldCode"].ToString() + "|" + dt.Rows[0]["FldDescription"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "UATDateSwitch", "ERROR", ex.Message, ex.StackTrace, csAuditLog.Category.Select);
            }
            return ret;
        }

        private double E2EActivityToday(DataSet ds)
        {
            bool result = false;
            string e2eres = string.Empty;
            E2EWS e2e = new E2EWS();
            double ret = 0.0;
            int i = 0;
            //string dateToday = string.Format("{ 0:yyyyMMdd}", System.DateTime.Now.Date);
            string dateToday = System.DateTime.Now.ToString("yyyyMMdd");

            if (ds != null && ds.Tables.Count > 1)
            {
                result = (e2e.E2ESuccess(ds, out e2eres));
                if (result)
                {
                    for (i = 0; i <= (ds.Tables[1].Rows.Count - 1); i++)
                    {
                        if (ds.Tables[1].Rows[i]["PostingDate"].ToString() == dateToday)
                        {
                            if (ds.Tables[1].Rows[i]["TransactionDate"].ToString() == dateToday)
                            {
                                ret += Convert.ToDouble(ds.Tables[1].Rows[i]["AmountInAccountCurrency"]);
                            }
                        }
                    }
                }
            }
            return ret;
        }

        /* E2EAPICHANGE 20220209 Removed findField, convertXML, expandHash, getHash, getHashValue */
        #endregion

        #region XML Request and Response
        /* E2EAPICHANGE 20220209 Removed fc_chk_digit */

        public string getFC(string AccountNo, FCMethod FC, int TransactionCount, bool IsOnline)
        {
            /* E2EAPICHANGE 20220209 Removed FCCIF */
            this.writeAudit("ACCOUNT INQUIRY: [AccountNo=" + AccountNo + "][FCMethod=" + FC.ToString() + "][TransactionCount=" + TransactionCount + "][IsOnline=" + IsOnline + "]", "", "", csAuditLog.Category.Systematics);
            string sReturn = string.Empty;
            string[] param = null;
            E2EWS e2e = new E2EWS();
            DataSet dsInquiry = new DataSet();
            DataSet dsHistory = new DataSet();
            double activityToday = 0.0;
            string openDate = string.Empty;
            bool result = false;
            string e2eres = string.Empty;
            string err = string.Empty;
            StringBuilder sbResult = new StringBuilder();
            DataRow data = null;
            int i = 0;
            DateTime startDate = DateTime.Now.Date.AddMonths(-2);
            DateTime endDate = DateTime.Now.Date;
            string UATDate = string.Empty;
            string[] arrUAT = null;
            string acctType = string.Empty;

            if (AccountNo.Length < 12)
            {
                /* CHECK IF KBI HAS FC EQUIVALENT */
                string SIAccount = this.getKBItoFC(AccountNo);
                if (SIAccount != string.Empty)
                {
                    AccountNo = SIAccount;
                }
            }
            this._siAccount = AccountNo;

            string[] arr = this.isAccountValid(AccountNo).Split('|');
            if (arr.GetValue(0).ToString() == "1")
            {
                this._acctType = arr.GetValue(3).ToString();
            }

            if (IsOnline == true)
            {
                if (!Regex.IsMatch(AccountNo, @"^\d+$"))
                {
                    sReturn = "Account number is not valid. Error: " + AccountNo;
                    return sReturn;
                }

                param = new string[]
                {
                    AccountNo,
                    e2e.GenerateUserRef(AccountNo),
                    e2e.GetE2EConfiguration(E2EWS.E2EConfiguration.TransactionBranch),
                    e2e.GetE2EConfiguration(E2EWS.E2EConfiguration.Channel)
                };

                dsInquiry = _e2e.connect2E2E(Guid.NewGuid().ToString(), E2EAPIConnector.E2EAPIConnector.APIMethod.AccountInquiry, E2EAPIConnector.E2EAPIConnector.MediaType.json, param, out result, out err);
                string branchName = string.Empty;
                if (dsInquiry != null && dsInquiry.Tables.Count > 1)
                {
                    data = dsInquiry.Tables[1].AsEnumerable().FirstOrDefault();
                    sbResult.AppendLine("Account Number: " + AccountNo);
                    sbResult.AppendLine("Account ID: " + data.Table.Rows[0]["AcctNum"].ToString());
                    sbResult.AppendLine("Account Name: " + data.Table.Rows[0]["CustomerShortName"].ToString());
                    sbResult.AppendLine("");

                    this._arrPrintResponse.SetValue(AccountNo, 0);
                    this._arrPrintResponse.SetValue(data.Table.Rows[0]["AcctNum"].ToString(), 1);
                    this._arrPrintResponse.SetValue(data.Table.Rows[0]["CustomerShortName"].ToString().ToUpper(), 2);
                    branchName = this.getBranchName(data.Table.Rows[0]["BranchCode"].ToString());
                    if (branchName == string.Empty)
                    {
                        branchName = data.Table.Rows[0]["BranchName"].ToString();
                    }

                    if (data.Table.Rows[0]["AcctType"] != null && data.Table.Rows[0]["AcctType"] != DBNull.Value)
                    {
                        switch (data.Table.Rows[0]["AcctType"].ToString().ToUpper())
                        {
                            case "SDA":
                                this._acctTypeVal = 1;
                                acctType = "SAVINGS";
                                break;

                            case "DDA":
                                this._acctTypeVal = 2;
                                acctType = "CURRENT";
                                break;

                            case "CDA":
                                this._acctTypeVal = 5;
                                acctType = "TIMEDEPOSIT";
                                break;

                            default:
                                this._acctTypeVal = 1;
                                acctType = "SAVINGS";
                                break;
                        }
                    }
                }

                if (FC == FCMethod.BalanceInquiry || FC == FCMethod.LastTransactionInquiry || FC == FCMethod.AccountStatusAndBalInquiry)
                {
                    UATDate = UATDateSwitch();
                    if (UATDate != string.Empty)
                    {
                        arrUAT = UATDate.Split('|');
                        if (arrUAT.Length > 1 && arrUAT.GetValue(0).ToString() == "1")
                        {
                            startDate = new DateTime(
                                Convert.ToInt32(arrUAT.GetValue(1).ToString().Substring(0, 4)),
                                Convert.ToInt32(arrUAT.GetValue(1).ToString().Substring(5, 2)),
                                Convert.ToInt32(arrUAT.GetValue(1).ToString().Substring(8, 2))).AddMonths(-2);
                            endDate = startDate.AddMonths(+4);
                        }
                    }
                }

                switch (FC)
                {
                    case FCMethod.BalanceInquiry:
                        if (dsInquiry != null && dsInquiry.Tables.Count > 0)
                        {
                            result = (e2e.E2ESuccess(dsInquiry, out e2eres));
                            if (result)
                            {
                                param = new string[]
                                {
                                    e2e.GetE2EConfiguration(E2EWS.E2EConfiguration.Channel),
                                    e2e.GetE2EConfiguration(E2EWS.E2EConfiguration.TransactionBranch),
                                    e2e.GenerateUserRef(AccountNo),
                                    AccountNo,
                                    string.Format("{0:yyyyMMdd}", startDate),
                                    string.Format("{0:yyyyMMdd}", endDate),
                                    "1",
                                    "10"
                                };
                                dsHistory = _e2e.connect2E2E(Guid.NewGuid().ToString(), E2EAPIConnector.E2EAPIConnector.APIMethod.TransactionHistory, E2EAPIConnector.E2EAPIConnector.MediaType.json, param, out result, out err);
                                activityToday = E2EActivityToday(dsHistory);
                                openDate = data.Table.Rows[0]["OpeningDate"].ToString();

                                this._arrPrintResponse.SetValue(data.Table.Rows[0]["ProductName"].ToString().ToUpper(), 3);
                                this._arrPrintResponse.SetValue(data.Table.Rows[0]["CurrencyCode"].ToString().ToUpper(), 4);
                                this._arrPrintResponse.SetValue(data.Table.Rows[0]["AcctStatusDesc"].ToString().ToUpper(), 5);
                                this._arrPrintResponse.SetValue(activityToday.ToString("##,###.00"), 6);
                                this._arrPrintResponse.SetValue(Convert.ToDouble(data.Table.Rows[0]["LedgerBalance"]).ToString("##,###.00"), 7);
                                this._arrPrintResponse.SetValue(Convert.ToDouble(data.Table.Rows[0]["AvailableBalance"]).ToString("##,###.00"), 8);
                                this._arrPrintResponse.SetValue(Convert.ToDouble(data.Table.Rows[0]["UnclearFunds"]).ToString("##,###.00"), 9);
                                this._arrPrintResponse.SetValue(Convert.ToDouble(data.Table.Rows[0]["BalanceOnHold"]).ToString("##,###.00"), 10);
                                this._arrPrintResponse.SetValue((openDate != string.Empty ? openDate.Substring(4, 2) + "/" + openDate.Substring(6, 2) + "/" + openDate.Substring(0, 4) : ""), 11);

                                sbResult.AppendLine("Account Description: " + data.Table.Rows[0]["ProductName"].ToString());
                                sbResult.AppendLine("Account Currency: " + data.Table.Rows[0]["CurrencyCode"].ToString());
                                sbResult.AppendLine("Account Status: " + data.Table.Rows[0]["AcctStatusDesc"].ToString());
                                sbResult.AppendLine("Activity Today: " + activityToday.ToString("##,###.00"));
                                sbResult.AppendLine("Ledger Balance: " + Convert.ToDouble(data.Table.Rows[0]["LedgerBalance"]).ToString("##,###.00"));
                                sbResult.AppendLine("Balance on Hold: " + Convert.ToDouble(data.Table.Rows[0]["BalanceOnHold"]).ToString("##,###.00"));
                                sbResult.AppendLine("Available Balance: " + Convert.ToDouble(data.Table.Rows[0]["AvailableBalance"]).ToString("##,###.00"));
                                sbResult.AppendLine("Passbook Balance: " + Convert.ToDouble(data.Table.Rows[0]["PassBookBalance"]).ToString("##,###.00"));
                                sbResult.AppendLine("Unclear Funds: " + Convert.ToDouble(data.Table.Rows[0]["UnclearFunds"]).ToString("##,###.00"));
                                sbResult.AppendLine("Open Date: " + (openDate != string.Empty ? openDate.Substring(4, 2) + "/" + openDate.Substring(6, 2) + "/" + openDate.Substring(0, 4) : ""));
                                sReturn = sbResult.ToString();
                                _acctBalance =
                                    data.Table.Rows[0]["ProductName"].ToString() + "|" +
                                    data.Table.Rows[0]["CurrencyCode"].ToString() + "|" +
                                    data.Table.Rows[0]["AcctStatusDesc"].ToString() + "|" +
                                    activityToday + "|" +
                                    data.Table.Rows[0]["AvailableBalance"].ToString() + "|" +
                                    data.Table.Rows[0]["LedgerBalance"].ToString() + "|" +
                                    data.Table.Rows[0]["UnclearFunds"].ToString() + "|" +
                                    data.Table.Rows[0]["BalanceOnHold"].ToString() + "|" +
                                    (openDate != string.Empty ? openDate.Substring(4, 2) + "/" + openDate.Substring(6, 2) + "/" + openDate.Substring(0, 4) : "") + "|" +
                                    data.Table.Rows[0]["AcctNum"].ToString();
                            }
                            else
                            {
                                sReturn = "Failed to get account details. Error in getting account details from core banking. Core Banking response is " + e2eres;
                            }
                        }
                        else
                        {
                            sReturn = "Failed to get account details. Error in getting account details from core banking. Request has timed-out";
                        }
                        break;

                    case FCMethod.LastTransactionInquiry:
                        param = new string[]
                        {
                            e2e.GetE2EConfiguration(E2EWS.E2EConfiguration.Channel),
                            e2e.GetE2EConfiguration(E2EWS.E2EConfiguration.TransactionBranch),
                            e2e.GenerateUserRef(AccountNo),
                            AccountNo,
                            string.Format("{0:yyyyMMdd}", startDate),
                            string.Format("{0:yyyyMMdd}", endDate),
                            "1",
                            "10"
                        };
                        dsHistory = _e2e.connect2E2E(Guid.NewGuid().ToString(), E2EAPIConnector.E2EAPIConnector.APIMethod.TransactionHistory, E2EAPIConnector.E2EAPIConnector.MediaType.json, param, out result, out err);
                        result = (e2e.E2ESuccess(dsInquiry, out e2eres));

                        if (dsHistory != null && dsHistory.Tables.Count > 1)
                        {
                            if (result)
                            {
                                string transactionDate = string.Empty;
                                string postingDate = string.Empty;
                                string originatingBranch = string.Empty;
                                string mnemonic = string.Empty;
                                string amountInAccountCurrency = string.Empty;
                                string userReferenceNumber = string.Empty;

                                this._arrPrintResponse.SetValue(AccountNo, 0);
                                this._arrPrintResponse.SetValue(data.Table.Rows[0]["CustomerShortName"].ToString().ToUpper(), 1);
                                this._arrPrintResponse.SetValue("TRDT            NEG    MNEM   AMT            REF", 2);

                                sbResult.AppendLine("TRDT\tNEG\tMNEM\tAMT\tREF");
                                int k = 0;
                                string userRefNo = string.Empty;
                                for (i = 0; i <= (dsHistory.Tables[1].Rows.Count - 1); i++)
                                {
                                    if (k == (TransactionCount + 1))
                                    {
                                        break;
                                    }
                                    userRefNo = dsHistory.Tables[1].Rows[i]["UserReferenceNumber"].ToString();
                                    transactionDate = dsHistory.Tables[1].Rows[i]["TransactionDate"].ToString();
                                    originatingBranch = dsHistory.Tables[1].Rows[i]["OriginatingBranch"].ToString();
                                    mnemonic = dsHistory.Tables[1].Rows[i]["Mnemonic"].ToString();
                                    if (dsHistory.Tables[1].Rows[i]["AmountInAccountCurrency"] != DBNull.Value || dsHistory.Tables[1].Rows[i]["AmountInAccountCurrency"].ToString() != "")
                                    {
                                        amountInAccountCurrency = Convert.ToDouble(dsHistory.Tables[1].Rows[i]["AmountInAccountCurrency"]).ToString("##,###.00");
                                    }
                                    else
                                    {
                                        amountInAccountCurrency = "0.00";
                                    }
                                    if (userRefNo.IndexOf("IRSW") >= 0)
                                    {
                                        userReferenceNumber = userRefNo;
                                    }
                                    else
                                    {
                                        userReferenceNumber = "";
                                    }
                                    if (transactionDate != string.Empty)
                                    {
                                        sbResult.AppendLine(transactionDate + "\t" + originatingBranch + "\t" + mnemonic + "\t" + amountInAccountCurrency + "\t" + userReferenceNumber);
                                        k++;
                                        if (k <= 5)
                                        {
                                            this._arrPrintResponse.SetValue(transactionDate + "\t" + originatingBranch + "\t" + mnemonic + "\t" + amountInAccountCurrency + "\t" + userReferenceNumber, k + 2);
                                        }
                                    }
                                }
                                sReturn = sbResult.ToString();
                            }
                            else
                            {
                                sReturn = "Failed to get account details. Error in getting account details from core banking. Core Banking response is " + e2eres;
                            }
                        }
                        else
                        {
                            if (e2eres != string.Empty)
                            {
                                sReturn = "Failed to get account details. Error in getting account details from core banking. Core Banking response is " + e2eres;
                            }
                            else
                            {
                                sReturn = "Failed to get account details. Error in getting account details from core banking. Request has timed-out";
                            }
                        }
                        break;

                    case FCMethod.HoldInquiry:
                        this._arrPrintResponse.SetValue(Convert.ToDouble(data.Table.Rows[0]["BalanceOnHold"]).ToString("##,###.00"), 3);
                        sbResult.AppendLine("Total Hold Amount: " + Convert.ToDouble(data.Table.Rows[0]["BalanceOnHold"]).ToString("##,###.00"));
                        sReturn = sbResult.ToString();
                        break;

                    case FCMethod.FloatInquiry:
                        this._arrPrintResponse.SetValue(Convert.ToDouble(data.Table.Rows[0]["UnclearFunds"]).ToString("##,###.00"), 3);
                        sbResult.AppendLine("Total Hold Amount: " + Convert.ToDouble(data.Table.Rows[0]["UnclearFunds"]).ToString("##,###.00"));
                        sReturn = sbResult.ToString();
                        break;

                    case FCMethod.AccountStatusInquiry:
                        result = (e2e.E2ESuccess(dsInquiry, out e2eres));
                        if (result)
                        {
                            this._arrPrintResponse.SetValue(data.Table.Rows[0]["AcctStatusDesc"].ToString().ToUpper(), 3);
                            this._arrPrintResponse.SetValue(data.Table.Rows[0]["BranchCode"].ToString().ToUpper(), 4);
                            this._arrPrintResponse.SetValue(branchName, 5);
                            this._arrPrintResponse.SetValue(data.Table.Rows[0]["AcctTypeDesc"].ToString().ToUpper(), 6);
                            this._arrPrintResponse.SetValue(data.Table.Rows[0]["CurrencyCode"].ToString().ToUpper(), 7);

                            sbResult.AppendLine("Account Status: " + data.Table.Rows[0]["AcctStatusDesc"].ToString());
                            sbResult.AppendLine("Branch Code: " + data.Table.Rows[0]["BranchCode"].ToString());
                            sbResult.AppendLine("Branch Name: " + branchName);
                            sbResult.AppendLine("Account Type: " + data.Table.Rows[0]["AcctTypeDesc"].ToString());
                            sbResult.AppendLine("Account Currency: " + data.Table.Rows[0]["CurrencyCode"].ToString());
                            sReturn = sbResult.ToString();
                            _acctStatus =
                                data.Table.Rows[0]["AcctStatus"].ToString() + "|" +
                                data.Table.Rows[0]["BranchCode"].ToString() + "|" +
                                acctType + "|" +
                                data.Table.Rows[0]["CurrencyCode"].ToString() + "|" +
                                data.Table.Rows[0]["AcctNum"].ToString() + "|" +
                                data.Table.Rows[0]["CustomerShortName"].ToString();
                        }
                        else
                        {
                            if (e2eres != string.Empty)
                            {
                                sReturn = "Failed to get account details. Error in getting account details from core banking. Core Banking response is " + e2eres;
                            }
                            else
                            {
                                sReturn = "Failed to get account details. Error in getting account details from core banking. Request has timed-out";
                            }
                        }
                        break;

                    case FCMethod.KBItoFC:
                        if (data != null && data.Table.Rows.Count > 0 && data.Table.Rows[0]["AcctNum"] != DBNull.Value)
                        {
                            sReturn = data.Table.Rows[0]["AcctNum"].ToString();
                        }
                        else
                        {
                            sReturn = "";
                        }
                        break;

                    case FCMethod.AccountStatusAndBalInquiry:
                        if (dsInquiry != null && dsInquiry.Tables.Count > 0)
                        {
                            result = (e2e.E2ESuccess(dsInquiry, out e2eres));
                            if (result)
                            {
                                param = new string[]
                                {
                                    e2e.GetE2EConfiguration(E2EWS.E2EConfiguration.Channel),
                                    e2e.GetE2EConfiguration(E2EWS.E2EConfiguration.TransactionBranch),
                                    e2e.GenerateUserRef(AccountNo),
                                    AccountNo,
                                    string.Format("{0:yyyyMMdd}", startDate),
                                    string.Format("{0:yyyyMMdd}", endDate),
                                    "1",
                                    "10"
                                };
                                dsHistory = _e2e.connect2E2E(Guid.NewGuid().ToString(), E2EAPIConnector.E2EAPIConnector.APIMethod.TransactionHistory, E2EAPIConnector.E2EAPIConnector.MediaType.json, param, out result, out err);
                                activityToday = E2EActivityToday(dsHistory);
                                openDate = data.Table.Rows[0]["OpeningDate"].ToString();

                                sbResult.AppendLine("\r\n");
                                sbResult.AppendLine("Account Status: " + data.Table.Rows[0]["AcctStatusDesc"].ToString() + "\r\n");
                                sbResult.AppendLine("Branch Code: " + data.Table.Rows[0]["BranchCode"].ToString() + "\r\n");
                                sbResult.AppendLine("Branch Name: " + branchName + "\r\n");
                                sbResult.AppendLine("Account Type: " + data.Table.Rows[0]["AcctTypeDesc"].ToString() + "\r\n");
                                sbResult.AppendLine("Account Currency: " + data.Table.Rows[0]["CurrencyCode"].ToString() + "\r\n");
                                sbResult.AppendLine("Activity Today: " + activityToday.ToString("##,###.00") + "\r\n");
                                sbResult.AppendLine("Ledger Balance: " + Convert.ToDouble(data.Table.Rows[0]["LedgerBalance"]).ToString("##,###.00") + "\r\n");
                                sbResult.AppendLine("Balance on Hold: " + Convert.ToDouble(data.Table.Rows[0]["BalanceOnHold"]).ToString("##,###.00") + "\r\n");
                                sbResult.AppendLine("Available Balance: " + Convert.ToDouble(data.Table.Rows[0]["AvailableBalance"]).ToString("##,###.00") + "\r\n");
                                sbResult.AppendLine("Passbook Balance: " + Convert.ToDouble(data.Table.Rows[0]["PassBookBalance"]).ToString("##,###.00") + "\r\n");
                                sbResult.AppendLine("Unclear Funds: " + Convert.ToDouble(data.Table.Rows[0]["UnclearFunds"]).ToString("##,###.00") + "\r\n");
                                sbResult.AppendLine("Open Date: " + (openDate != string.Empty ? openDate.Substring(4, 2) + "/" + openDate.Substring(6, 2) + "/" + openDate.Substring(0, 4) : "") + "\r\n");
                                sReturn = sbResult.ToString();
                                _acctStatus =
                                    data.Table.Rows[0]["AcctStatus"].ToString() + "|" +
                                    data.Table.Rows[0]["BranchCode"].ToString() + "|" +
                                    acctType + "|" +
                                    data.Table.Rows[0]["CurrencyCode"].ToString() + "|" +
                                    data.Table.Rows[0]["AcctNum"].ToString();
                            }
                            else
                            {
                                sReturn = "Failed to get account details. Error in getting account details from core banking. Core Banking response is " + e2eres;
                            }
                        }
                        else
                        {
                            sReturn = "Failed to get account details. Error in getting account details from core banking. Request has timed-out";
                        }
                        break;

                    case FCMethod.AccountStatusCode:
                        if (data != null && data.Table.Rows.Count > 0 && data.Table.Rows[0]["AcctStatus"] != DBNull.Value)
                        {
                            sReturn = data.Table.Rows[0]["AcctStatus"].ToString();
                        }
                        else
                        {
                            sReturn = "";
                        }
                        break;

                    case FCMethod.AmountInquiry:
                        if (data != null && data.Table.Rows.Count > 0 && data.Table.Rows[0]["AcctStatus"] != DBNull.Value)
                        {
                            sReturn = data.Table.Rows[0]["AvailableBalance"].ToString();
                        }
                        else
                        {
                            sReturn = "0.00";
                        }
                        break;
                }
                this.saveResponse(FC, sReturn);
            }
            else
            {
                sReturn = this.getResponse(FC, AccountNo);
            }
            System.Web.HttpContext.Current.Session["ACCOUNTINQUIRY"] = _arrPrintResponse;
            this.writeAudit("INQUIRY RESPONSE: [Response=" + sReturn + "]", "", "", csAuditLog.Category.Systematics);
            return sReturn;
        }

        /* E2EAPICHANGE 20220209 Removed getBalanceMethod, cleanResponse, getActivityToday, getLastTransaction, getCIFDetails, getListTransactions */

        /* E2EAPICHANGE 20220209 Updated Code */
        public string saveCIFtoFC(string CIFID)
        {
            return string.Empty;
        }

        /* 
         * CHANGE 20220209 Updated Code */
        public bool testFC(out String errmsg)
        {
            string sAccountNo = getTestAccount();
            bool bReturn = false;
            string ret = string.Empty;
            errmsg = string.Empty;

            if (sAccountNo != string.Empty)
            {
                ret = this.getFC(sAccountNo, FCMethod.AccountStatusCode, 0, true);
                if (ret != string.Empty)
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
            this.writeAudit("TEST CONNECTION INQUIRY: [AccountNo=" + sAccountNo + "][ReturnValue=" + bReturn.ToString() + "]", "ErrorMessage=" + errmsg.ToString(), "", csAuditLog.Category.Systematics);
            return bReturn;
        }
        #endregion

        #region SQL Queries
        public string getKBItoFC(string AccountNo)
        {
            base.EnforceConstraint = false;
            /* FOR CBIP */
            DataSet ds = getDataSet("getKBItoFC", true, new Object[] { AccountNo });
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].Rows[0]["Flex_Acct"].ToString().Trim();
            }
            else
            {
                return string.Empty;
            }
        }

        public string getTestAccount()
        {
            base.EnforceConstraint = false;
            DataSet ds = getDataSet("sp_irsAgent_GetAgentTestFCAccount", true, new Object[] { });
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].Rows[0][0].ToString().Trim();
            }
            else
            {
                ds = getDataSet("isp_getTestAccount", true, new Object[] { });
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    selectedRow = 0;
                    return table.Rows[0]["AccountNo"].ToString().Trim();
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /* E2EAPICHANGE 20220209 Removed getBranchName, getCIF */

        public string getBranchName(string BranchCode)
        {
            base.EnforceConstraint = false;
            /* FOR CBIP */
            DataSet ds = getDataSet("asp_getBranchName", true, new Object[] { BranchCode });
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].Rows[0]["BranchName"].ToString().Trim();
            }
            else
            {
                return string.Empty;
            }
        }

        private void saveResponse(FCMethod FC, string Response)
        {
            int i = 0;
            string[] arr = new string[3];

            try
            {
                Response = Response.Replace("\r\n", "@");
                Response = Response.Replace("\t", "^");
                arr[0] = FC.ToString();
                arr[1] = this._siAccount;
                arr[2] = Response.Replace("\r\n", "");

                i = execStoredProcedureUpdate("sp_Insert_Inquiry", arr);
                this.writeAudit("INSERT INQUIRY: [FCMethod=" + FC.ToString() + "][AccountNo=" + this._siAccount + "][Response=" + arr[2].ToString() + "]", "INSERT RESPONSE=" + i, "", csAuditLog.Category.Insert);
            }
            catch (Exception ex)
            {
                this.writeAudit("INSERT INQUIRY: [FCMethod=" + FC.ToString() + "][AccountNo=" + this._siAccount + "][Response=" + arr[2].ToString() + "]", "INSERT RESPONSE=" + ex.Message, "", csAuditLog.Category.Insert);
            }
        }

        private string getResponse(FCMethod FC, string AccountNo)
        {
            base.EnforceConstraint = false;
            int i = 0;
            string sReturn = "LAST INQUIRY AS OF: ";
            string arrValue = string.Empty;
            string sResponse = string.Empty;
            string sValue = string.Empty;
            DataSet ds = getDataSet("sp_getInquiry", true, new Object[] { FC.ToString(), AccountNo });
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                sResponse = ds.Tables[0].Rows[0]["Response"].ToString();
                for (i = 0; i <= (sResponse.Length - 1); i++)
                {
                    arrValue = sResponse.Substring(i, 1);
                    if (arrValue == "@")
                    {
                        sValue += "\n\r";
                    }
                    else
                    {
                        if (arrValue == "^")
                        {
                            sValue += "\t";
                        }
                        else
                        {
                            sValue += arrValue;
                        }
                    }
                }
                sReturn += ds.Tables[0].Rows[0]["DateCreated"].ToString() + "\n\r\n\r" + sValue;
            }
            else
            {
                return "There is no history of inquiry for account number " + AccountNo + ".";
            }
            return sReturn;
        }

        /* E2EAPICHANGE 20220209 Updated Code */
        public string getCK(string CardNo)
        {
            base.EnforceConstraint = false;
            DataSet ds = getDataSet("isp_getGFCtoCK", true, new Object[] { CardNo });
            string ret = string.Empty;
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                string ck = ds.Tables[0].Rows[0][0].ToString().Trim();
                if (ck != "-1")
                {
                    ret = ds.Tables[0].Rows[0][0].ToString().Trim();
                }
            }
            if (ret == string.Empty)
            {
                string val = string.Empty;
                string[] arr = null;

                // 00|ABAD|ELMIEZER|NARCISO|01/12/1986|498686600045||CLAIMED|Successfully retrieve card details.
                // 03||||||||Invalid GFC number.
                try
                {
                    GFC_SI_WSDL.GFC_WebService gfc_si_wsdl = new IRSClassLib.GFC_SI_WSDL.GFC_WebService();
                    gfc_si_wsdl.Url = ConfigurationManager.AppSettings["GFC_SI_WSDL"];
                    val = gfc_si_wsdl.GetCardDetails(CardNo, ConfigurationManager.AppSettings["GFC_USERID"].ToString());
                    arr = val.Split('|');
                    if (arr.GetValue(0).ToString() == ConfigurationManager.AppSettings["GFC_VALID_RESPONSE"].ToString())
                    {
                        ret = arr.GetValue(5).ToString();
                    }
                    else
                    {
                        ret = string.Empty;
                    }
                }
                catch (Exception ex)
                {
                    this.writeAudit("GFC CARD INQUIRY: [CardNo=" + CardNo + "][FCMethod=GFCtoCK]", ex.Message, "", csAuditLog.Category.Select);
                    ret = string.Empty;
                }
            }
            return ret;
        }
        #endregion

        #region Public Methods
        public string getBalanceInquiry(string AccountNo)
        {
            this.getFC(AccountNo, FCMethod.BalanceInquiry, 0, true);
            return _acctBalance;
        }

        //public string getAccountStatus(string AccountNo)
        //{
        //    string ret = this.getFC(AccountNo, FCMethod.AccountStatusInquiry, 0, true);
        //    string accountType = this._acctTypeVal.ToString();
        //    string accountStatus = string.Empty;

        //    if (_acctStatus != null && _acctStatus != string.Empty)
        //    {
        //        string[] arr = _acctStatus.Split('|');
        //        if (arr.GetValue(0).ToString() != string.Empty)
        //        {
        //            switch (accountType)
        //    {
        //                case "2":
        //                    accountStatus = new irsInternetPage().getLookupValue("AccountStatusIM", "FldCode", arr.GetValue(0).ToString(), "FldDescription");
        //                    break;

        //                default:
        //                    accountStatus = new irsInternetPage().getLookupValue("AccountStatusST", "FldCode", arr.GetValue(0).ToString(), "FldDescription");
        //                    break;
        //            }
        //    }
        //    }
        //    return _acctStatus + '|' + accountType + "|" + ret + "|" + accountStatus;
        //}

        public string getAccountStatus(string AccountNo)
        {
            string ret = this.getFC(AccountNo, FCMethod.AccountStatusInquiry, 0, true);
            string accountType = string.Empty;
            if (this._isST)
            {
                accountType = "1";
            }
            else
            {
                accountType = "2";
            }
            return _acctStatus + '|' + accountType + "|" + ret;
        }

        /* E2EAPICHANGE 20220209 Removed getAccountStatusAndName */

        public string getGFCtoCK(string CardNo, bool IsDelimited)
        {
            this.writeAudit("GFC CARD INQUIRY: [CardNo=" + CardNo + "][FCMethod=" + FCMethod.GFCtoCK.ToString() + "]", "", "", csAuditLog.Category.Select);
            string sReturn = string.Empty;
            string sAccountNo = string.Empty;

            sAccountNo = this.getCK(CardNo);
            if (sAccountNo != string.Empty)
            {
                sReturn = this.getGFCAccountStatus(sAccountNo, IsDelimited);
            }
            else
            {
                irsLinkToGFC ilg = new irsLinkToGFC();
                ilg.IsWinApp = base.IsWinApp;

                sAccountNo = ilg.GetGFCCKNumber(CardNo);
                if (sAccountNo != string.Empty)
                {
                    sReturn = this.getGFCAccountStatus(sAccountNo, IsDelimited);
                }
                else
                {
                    sReturn = "NO LINKED CK";
                }
            }
            if (IsDelimited == false)
            {
                if (sReturn.IndexOf("Account Number: " + sAccountNo) < 0)
                {
                    sReturn = (sAccountNo != string.Empty) ? "Account Number: " + sAccountNo + "\n\r\n\r FC Response: " + sReturn : sReturn;
                }
            }
            return sReturn;
        }

        public bool getAgentAccountBalance(string AccountNo, double Amount)
        {
            this.writeAudit("AGENT ACCOUNT BALANCE: [AccountNo=" + AccountNo + "][FCMethod=AgentAccountBalance][Amount=" + Amount.ToString() + "]", "", "", csAuditLog.Category.Systematics);
            string sReturn = this.getBalanceInquiry(AccountNo);
            string[] arrBalance;
            bool bReturn = false;
            double subtractAmount;
            if (_acctBalance != string.Empty)
            {
                arrBalance = _acctBalance.Split('|');
                subtractAmount = Convert.ToDouble(arrBalance[4].ToString()) - Amount;
                if (subtractAmount >= 1)
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
            this.writeAudit("AGENT ACCOUNT INQUIRY RESPONSE: [AccountNo=" + AccountNo + "][FCMethod=AgentAccountInquiry][Amount=" + Amount.ToString() + "][Response=" + sReturn + "][ReturnValue=" + bReturn.ToString() + "]", "", "", csAuditLog.Category.Systematics);
            return bReturn;
        }
        #endregion

        #region Private Methods
        private string getGFCAccountStatus(string AccountNo, bool IsDelimited)
        {
            string sReturn = string.Empty;
            sReturn = this.getFC(AccountNo, FCMethod.AccountStatusInquiry, 0, true);
            if (IsDelimited == true)
            {
                sReturn = _acctStatus;
            }
            return sReturn;
        }

        /* E2EAPICHANGE 20220209 Removed getAvailableBalance, getAccountDetails */

        private string getLocation()
        {
            if (base.IsWinApp)
            {
                return "IRSWorld";
            }
            else
            {
                return ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location;
            }
        }

        private void writeAudit(string Description, string NewValue, string OldValue, csAuditLog.Category Category)
        {
            string sDescription = "Location=" + getLocation() + " : " + Description;
            new csAuditLog().writeAudit("", "INQUIRY", sDescription, NewValue, OldValue, Category);
        }

        public bool isForApprovalTag()
        {
            DataTable dt = new irsBranchDefaults().getDefaultSetup(getLocation());
            bool forApprovalTag = false;
            int i = 0;

            if (dt != null && dt.Rows.Count > 0)
            {
                for (i = 0; i <= (dt.Rows.Count - 1); i++)
                {
                    if (dt.Rows[i]["FldDescription"].ToString().ToUpper() == "FOR APPROVAL TAG")
                    {
                        if (dt.Rows[i]["checked"].ToString() == "1")
                        {
                            forApprovalTag = true;
                            break;
                        }
                    }
                }
            }
            return forApprovalTag;
        }

        /* FOR CBIP 20170825 */
        public DataTable BlackOutMode()
        {
            base.EnforceConstraint = false;
            DataSet ds = getDataSet("isp_getLookup", true, new Object[] { "BlackOutMode" });
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }
        #endregion

        public void update_account_remarks(string account_no, string remarks)
        {
            string update_string = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(remarks).Equals(true))
                {
                    remarks = "";
                }

                if (string.IsNullOrEmpty(account_no).Equals(false))
                {
                    account_no = account_no.Replace("'", "");

                    if (string.IsNullOrEmpty(remarks).Equals(false))
                    {
                        update_string = "update tbl_cif_accounts set remarks = '" + remarks + "'" + " where accountno = '" + account_no + "'";
                    }
                    else
                    {
                        update_string = "update tbl_cif_accounts set remarks = null where accountno = '" + account_no + "'";
                    }

                    object o = execScalar(update_string);

                }
            }
            catch {}
        }

        public string isAccountValid(string AccountNo, bool IsValid)
        {
            int intAccountLength = 0;
            string sReturn = string.Empty;
            string sBranchCode = string.Empty;
            string sAccountType = string.Empty;
            int intAccountType = 0;
            string sCurrencyValue = string.Empty;
            string CTRL1 = string.Empty;
            string CTRL2 = string.Empty;
            string CTRL3 = string.Empty;
            string CTRL4 = string.Empty;
            string CTRL5 = string.Empty;
            string SystemFormat = string.Empty;

            try
            {
                AccountNo = AccountNo.Replace("-", string.Empty);
                intAccountLength = AccountNo.Length;
                int[] arrAccountNumber = new int[intAccountLength];
                int[] arrCheckDigit = new int[intAccountLength];
                int[] arrAccountNumberMultiplyCheckDigit = new int[intAccountLength];

                switch (intAccountLength)
                {
                    case 12:
                        arrCheckDigit = new int[] { 5, 5, 5, 5, 7, 9, 9, 9, 9, 9, 9 };
                        sBranchCode = AccountNo.Substring(0, 4);
                        sAccountType = AccountNo.Substring(4, 1);
                        CTRL5 = AccountNo.Substring(4, 8);
                        if (Convert.ToInt32(sAccountType) >= 0 && Convert.ToInt32(sAccountType) <= 5)
                        {
                            intAccountType = (int)AccountType.Savings;
                            sCurrencyValue = "001";
                            CTRL5 = CTRL5.PadLeft(14, '0');
                        }
                        else
                        {
                            if (Convert.ToInt32(sAccountType) == 6)
                            {
                                intAccountType = (int)AccountType.Savings;
                                sCurrencyValue = "002";
                                CTRL5 = CTRL5.PadLeft(14, '0');
                            }
                            else
                            {
                                if (Convert.ToInt32(sAccountType) >= 7 && Convert.ToInt32(sAccountType) <= 8)
                                {
                                    intAccountType = (int)AccountType.Current;
                                    sCurrencyValue = "001";
                                    CTRL5 = CTRL5.PadLeft(10, '0');
                                }
                                else
                                {
                                    if (Convert.ToInt32(sAccountType) >= 9)
                                    {
                                        intAccountType = (int)AccountType.Current;
                                        sCurrencyValue = "002";
                                        CTRL5 = CTRL5.PadLeft(10, '0');
                                    }
                                }
                            }
                        }
                        break;

                    case 14:
                        arrCheckDigit = new int[] { 3, 3, 5, 5, 5, 5, 7, 9, 9, 9, 9, 9, 9 };
                        sBranchCode = AccountNo.Substring(2, 4);
                        sAccountType = AccountNo.Substring(6, 1);
                        CTRL5 = AccountNo.Substring(6, 8);
                        if (Convert.ToInt32(sAccountType) == 6)
                        {
                            intAccountType = (int)AccountType.Savings;
                            sCurrencyValue = "0" + AccountNo.Substring(0, 2);
                            CTRL5 = CTRL5.PadLeft(14, '0');
                        }
                        else
                        {
                            if (Convert.ToInt32(sAccountType) == 9)
            {
                                intAccountType = (int)AccountType.Current;
                                sCurrencyValue = "0" + AccountNo.Substring(0, 2);
                                CTRL5 = CTRL5.PadLeft(10, '0');
                            }
                        }
                        break;

                    case 16:
                        arrCheckDigit = new int[] { 3, 3, 5, 5, 5, 5, 7, 9, 9, 9, 9, 9, 9, 9, 9 };
                        sBranchCode = AccountNo.Substring(2, 4);
                        sAccountType = AccountNo.Substring(6, 1);
                        sCurrencyValue = "0" + AccountNo.Substring(0, 2);
                        CTRL5 = AccountNo.Substring(6, 10);
                        CTRL5 = CTRL5.PadLeft(14, '0');
                        if (Convert.ToInt32(sAccountType) >= 0 && Convert.ToInt32(sAccountType) <= 2)
                        {
                            intAccountType = (int)AccountType.CashCard;
                        }
                        else
                        {
                            if (Convert.ToInt32(sAccountType) >= 3 && Convert.ToInt32(sAccountType) <= 5)
                            {
                                intAccountType = (int)AccountType.GFC;
                            }
                            else
                            {
                                if (Convert.ToInt32(sAccountType) >= 6 && Convert.ToInt32(sAccountType) <= 8)
                                {
                                    intAccountType = (int)AccountType.TimeDeposit;
                                }
                            }
            }
                        break;

                    default:
                        return "0|INVALID ACCOUNT NUMBER";
                }

                // Create System Format for SI.
                CTRL1 = "08"; //PNB CODE
                CTRL2 = sCurrencyValue;
                CTRL3 = "00" + sBranchCode.Substring(0, 1);
                CTRL4 = sBranchCode.Substring(1, 3);
                if (intAccountType == (int)AccountType.Current)
                {
                    CTRL3 = "000";
                    CTRL4 = sBranchCode;
                }
                else
                {
                    if (intAccountType == (int)AccountType.GFC)
                    {
                        CTRL3 = "000";
                        CTRL4 = "170";
                    }
                }
                SystemFormat = CTRL1 + "-" + CTRL2 + "-" + CTRL3 + "-" + CTRL4 + "-" + CTRL5;
                sReturn = "1|" + AccountNo + "|" + SystemFormat + "|" + intAccountType.ToString();
            }
            catch (Exception ex)
            {
                return "0|" + ex.Message;
            }
            return sReturn;
        }
    }
}