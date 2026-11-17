using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Net.Http;
using System.Data;
using System.IO;
using System.Diagnostics;

namespace E2EAPIConnector
{
    [Serializable]
    public class E2EAPIConnector
    {
        private static StreamWriter _swlog;
        private static Object _wlock = new Object();
        private static String _evSourceId = "Application";
        private static String _logname = ConfigurationManager.AppSettings["ApplicationName"];
        private static String _bankcode = "08";
        SQLHelper _sqlHelper = new SQLHelper();

        public E2EAPIConnector() { }

        #region Account Inquiry Payload
        public class AccountInquiryRequest
        {
            public string AccountId { get; set; }
            public string UserReferenceNumber { get; set; }
            public string TransactionBranch { get; set; }
            public string Channel { get; set; }
            public string BankCode { get; set; }
        }

        public class AccountInquiryResponse
        {
            public AccountInquiryAcctDetails AcctDetails { get; set; }
            public AccountInquiryTxnStatus TxnStatus { get; set; }
        }

        public class AccountInquiryAcctDetails
        {
            public string UserReferenceNumber { get; set; }
            public string AcctNum { get; set; }
            public string AcctStatus { get; set; }
            public string AcctStatusDesc { get; set; }
            public string BranchCode { get; set; }
            public string BranchName { get; set; }
            public string CurrencyCode { get; set; }
            public string ProductCode { get; set; }
            public string ProductName { get; set; }
            public string AcctType { get; set; }
            public string AcctTypeDesc { get; set; }
            public string OpeningDate { get; set; }
            public AccountInquiryAcctDetailsBalances Balances { get; set; }
            public string CustomerShortName { get; set; }
            public string CustomerID { get; set; }
            public string JointAccountInd { get; set; }
        }

        public class AccountInquiryAcctDetailsBalances
        {
            public string AvailableBalance { get; set; }
            public string LedgerBalance { get; set; }
            public string UnclearFunds { get; set; }
            public string PassBookBalance { get; set; }
            public string BalanceOnHold { get; set; }
        }

        public class AccountInquiryTxnStatus
        {
            public string ReplyCode { get; set; }
            public string ReplyText { get; set; }
            public string TransDate { get; set; }
        }
        #endregion

        #region Transaction History Payload
        public class TransactionHistoryRequest
        {
            public string AccountId { get; set; }
            public string UserReferenceNumber { get; set; }
            public string Channel { get; set; }
            public string TransactionBranch { get; set; }
            public string BankCode { get; set; }
            public string StartDateStr { get; set; }
            public string EndDateStr { get; set; }
            public string PageNumber { get; set; }
            public string PageSize { get; set; }
        }

        public class TransactionHistoryResponse
        {
            public TransactionHistoryListArray TransactionListArray { get; set; }
            public TranactionHistoryTxnStatus TxnStatus { get; set; }
        }

        public class TransactionHistoryListArray
        {
            public IEnumerable<TransactionHistoryListArrayTransactionList> TransactionList { get; set; }
        }

        public class TransactionHistoryListArrayTransactionList
        {
            public string Mnemonic { get; set; }
            public string TransactionDate { get; set; }
            public string AmountInAccountCurrency { get; set; }
            public string PostingDate { get; set; }
            public string OriginatingBranch { get; set; }
            public string UserReferenceNumber { get; set; }
            public string Timestamp { get; set; }
        }

        public class TranactionHistoryTxnStatus
        {
            public string ReplyCode { get; set; }
            public string ReplyText { get; set; }
            public string TransDate { get; set; }
        }
        #endregion

        #region Fund Transfer Payload
        public class FundTransferRequest
        {
            public string BankCode { get; set; }
            public string Channel { get; set; }
            public string TransactionBranch { get; set; }
            public string FromAccountId { get; set; }
            public string ToAccountId { get; set; }
            public string TransactionAmount { get; set; }
            public string Narrative { get; set; }
            public string UserReferenceNumber { get; set; }
            public string TransactionType { get; set; }
            public string TransactionMode { get; set; }
            public string RefIdent { get; set; }
        }

        public class FundTransferResponse
        {
            public string RefIdent { get; set; }
            public string UserReferenceNumber { get; set; }
            public FundTransferTxnStatus TxnStatus { get; set; }
        }

        public class FundTransferTxnStatus
        {
            public string ReplyCode { get; set; }
            public string ReplyText { get; set; }
            public string TransDate { get; set; }
        }
        #endregion

        #region Fund Transfer v2 Payload
        public class FundTransferV2Request
        {
            public string BankCode { get; set; }
            public string Channel { get; set; }
            public string TransactionBranch { get; set; }
            public string FromAccountId { get; set; }
            public string ToAccountId { get; set; }
            public string TransactionAmount { get; set; }
            public string Narrative { get; set; }
            public string UserReferenceNumber { get; set; }
            public string TransactionType { get; set; }
            public string TransactionMode { get; set; }
            public string RefIdent { get; set; }
            public string TransactionCode { get; set; } //blank = CASA to CASA, D = Debit Memo, C = Credit Memo
            public string CurrencyCode { get; set; } //applicable only to Debit/Credit Memo otherwise leave blank
        }

        public class TxnStatusResponse
        {
            public FailedTxnStatus TxnStatus { get; set; }

            public class FailedTxnStatus
            {
                public string ReplyCode { get; set; }
                public string ReplyText { get; set; }
                public string TransDate { get; set; }
            }
        }
        #endregion

        #region Enum
        public enum APIMethod
        {
            AccountInquiry,
            TransactionHistory,
            FundTransfer,
            FundTransferV2
        }

        public enum MediaType
        {
            json,
            form
        }

        public enum ErrorMessage
        {
            IncompleteParam,
            InvalidInput,
            ExceptionFound,
            NoRecordFound
        }
        #endregion

        #region E2E API
        public DataSet connect2E2E(string identifier, APIMethod method, MediaType mediatype, string[] param, out bool success, out string err)
        {
            DataSet ds = new DataSet();
            success = false;
            err = "";

            try
            {
                this.logMe("START", identifier);
                this.BindCertificate();
                string url = string.Empty;
                string request = string.Empty;
                string response = string.Empty;
                HttpClient client = new HttpClient();
                HttpResponseMessage httpResponse = null;
                bool isGet = false;
                string type = string.Empty;
                string accountNo = string.Empty;

                AccountInquiryRequest inquiryRequest = new AccountInquiryRequest();
                TransactionHistoryRequest tranhistRequest = new TransactionHistoryRequest();
                FundTransferRequest ftRequest = new FundTransferRequest();
                FundTransferV2Request ftRequestv2 = new FundTransferV2Request();

                AccountInquiryResponse inquiryResponse = new AccountInquiryResponse();
                TransactionHistoryResponse tranhistResponse = new TransactionHistoryResponse();
                FundTransferResponse ftResponse = new FundTransferResponse();

                switch (method)
                {
                    case APIMethod.AccountInquiry:
                        if (param.Length == 4)
                        {
                            url = ConfigurationManager.AppSettings["URI_AccountInquiry"].ToString();
                            if (this.validate(APIMethod.AccountInquiry, param))
                            {
                                inquiryRequest.AccountId = param[0].ToString();
                                inquiryRequest.UserReferenceNumber = param[1].ToString();
                                inquiryRequest.TransactionBranch = param[2].ToString();
                                inquiryRequest.Channel = param[3].ToString();
                                inquiryRequest.BankCode = _bankcode;
                                request = Newtonsoft.Json.JsonConvert.SerializeObject(inquiryRequest);
                                accountNo = method.ToString() + "=" + inquiryRequest.AccountId;
                            }
                            else
                            {
                                success = false;
                                err = ErrorMessage.InvalidInput.ToString();
                                return null;
                            }
                        }
                        else
                        {
                            success = false;
                            err = ErrorMessage.IncompleteParam.ToString();
                            return null;
                        }
                        break;

                    case APIMethod.TransactionHistory:
                        url = ConfigurationManager.AppSettings["URI_TransactionHistory"].ToString();

                        if (param.Length == 8)
                        {
                            if (this.validate(APIMethod.TransactionHistory, param))
                            {
                                tranhistRequest.Channel = param[0].ToString();
                                tranhistRequest.TransactionBranch = param[1].ToString();
                                tranhistRequest.UserReferenceNumber = param[2].ToString();
                                tranhistRequest.AccountId = param[3].ToString();
                                tranhistRequest.StartDateStr = param[4].ToString();
                                tranhistRequest.EndDateStr = param[5].ToString();
                                tranhistRequest.PageNumber = param[6].ToString();
                                tranhistRequest.PageSize = param[7].ToString();
                                tranhistRequest.BankCode = _bankcode;
                                request = Newtonsoft.Json.JsonConvert.SerializeObject(tranhistRequest);
                                accountNo = method.ToString() + "=" + tranhistRequest.AccountId;
                            }
                            else
                            {
                                success = false;
                                err = ErrorMessage.InvalidInput.ToString();
                                return null;
                            }
                        }
                        else
                        {
                            success = false;
                            err = ErrorMessage.IncompleteParam.ToString();
                            return null;
                        }
                        break;

                    case APIMethod.FundTransfer:
                        url = ConfigurationManager.AppSettings["URI_FundTransfer"].ToString();

                        if (param.Length == 10)
                        {
                            if (this.validate(APIMethod.FundTransfer, param))
                            {
                                ftRequest.Channel = param[0].ToString();
                                ftRequest.TransactionBranch = param[1].ToString();
                                ftRequest.FromAccountId = param[2].ToString();
                                ftRequest.ToAccountId = param[3].ToString();
                                ftRequest.TransactionAmount = param[4].ToString();
                                ftRequest.Narrative = param[5].ToString();
                                ftRequest.UserReferenceNumber = param[6].ToString();
                                ftRequest.TransactionType = param[7].ToString();
                                ftRequest.TransactionMode = param[8].ToString();
                                ftRequest.RefIdent = param[9].ToString();
                                ftRequest.BankCode = _bankcode;
                                request = Newtonsoft.Json.JsonConvert.SerializeObject(ftRequest);
                                accountNo = method.ToString() + "=" + ftRequest.FromAccountId + ":" + ftRequest.ToAccountId;
                            }
                            else
                            {
                                success = false;
                                err = ErrorMessage.InvalidInput.ToString();
                                return null;
                            }
                        }
                        else
                        {
                            success = false;
                            err = ErrorMessage.IncompleteParam.ToString();
                            return null;
                        }
                        break;

                    case APIMethod.FundTransferV2:
                        url = ConfigurationManager.AppSettings["URI_FundTransferV2"].ToString();

                        if (param.Length == 12)
                        {
                            if (this.validate(APIMethod.FundTransferV2, param))
                            {
                                ftRequestv2.Channel = param[0].ToString();
                                ftRequestv2.TransactionBranch = param[1].ToString();
                                ftRequestv2.FromAccountId = param[2].ToString();
                                ftRequestv2.ToAccountId = param[3].ToString();
                                ftRequestv2.TransactionAmount = param[4].ToString();
                                ftRequestv2.Narrative = param[5].ToString();
                                ftRequestv2.UserReferenceNumber = param[6].ToString();
                                ftRequestv2.TransactionType = param[7].ToString();
                                ftRequestv2.TransactionMode = param[8].ToString();
                                ftRequestv2.RefIdent = param[9].ToString();
                                ftRequestv2.TransactionCode = param[10].ToString();
                                ftRequestv2.CurrencyCode = param[11].ToString();
                                ftRequestv2.BankCode = _bankcode;
                                request = Newtonsoft.Json.JsonConvert.SerializeObject(ftRequestv2);
                                accountNo = method.ToString() + "=" + ftRequestv2.FromAccountId + ":" + ftRequestv2.ToAccountId;
                            }
                            else
                            {
                                success = false;
                                err = ErrorMessage.InvalidInput.ToString();
                                return null;
                            }
                        }
                        else
                        {
                            success = false;
                            err = ErrorMessage.IncompleteParam.ToString();
                            return null;
                        }
                        break;
                }

                if (this.HostIsDown(accountNo))
                {
                    success = false;
                    err = "ERROR: Host is down";
                    return null;
                }

                switch (mediatype)
                {
                    case MediaType.form:
                        type = "application/x-www-form-urlencoded";
                        break;

                    case MediaType.json:
                        type = "application/json";
                        break;
                }

                this.logMe("REQUEST: " + method.ToString() + " " + request, identifier);
                StringContent data = new StringContent(request, Encoding.UTF8, type);
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(type));
                client.DefaultRequestHeaders.Add("API-Key", ConfigurationManager.AppSettings["API-Key"].ToString());
                client.Timeout = TimeSpan.FromMilliseconds(Convert.ToInt32(ConfigurationManager.AppSettings["Timeout"]));

                try
                {
                    if (isGet)
                    {
                        httpResponse = client.GetAsync(url).Result;
                    }
                    else
                    {
                        httpResponse = client.PostAsync(url, data).Result;
                    }
                }
                catch (Exception ex)
                {
                    this.logMe("ERROR: " + method.ToString() + " " + ex.Message + "|" + ex.StackTrace, identifier);
                    return null;
                }

                //response = httpResponse.Content.ReadAsStringAsync().Result;
                byte[] byteArray = httpResponse.Content.ReadAsByteArrayAsync().Result;
                response = Encoding.UTF8.GetString(byteArray);

                DataTable dtTxnStatus = new DataTable("E2EStatus");
                DataRow dr;
                if (httpResponse.IsSuccessStatusCode)
                {
                    this.logMe("RESPONSE: " + method.ToString() + " " + response, identifier);
                    switch (method)
                    {
                        case APIMethod.AccountInquiry:
                            AccountInquiryResponse accountInquiryResponse = JsonConvert.DeserializeObject<AccountInquiryResponse>(response);

                            if (accountInquiryResponse != null)
                            {
                                DataRow drAIResponse;
                                DataTable dtAcctDetails = new DataTable("AccountInquiry");
                                dtAcctDetails.Columns.Add("UserReferenceNumber");
                                dtAcctDetails.Columns.Add("AcctNum");
                                dtAcctDetails.Columns.Add("AcctStatus");
                                dtAcctDetails.Columns.Add("AcctStatusDesc");
                                dtAcctDetails.Columns.Add("BranchCode");
                                dtAcctDetails.Columns.Add("BranchName");
                                dtAcctDetails.Columns.Add("CurrencyCode");
                                dtAcctDetails.Columns.Add("ProductCode");
                                dtAcctDetails.Columns.Add("ProductName");
                                dtAcctDetails.Columns.Add("AcctType");
                                dtAcctDetails.Columns.Add("AcctTypeDesc");
                                dtAcctDetails.Columns.Add("OpeningDate");
                                dtAcctDetails.Columns.Add("CustomerShortName");
                                dtAcctDetails.Columns.Add("CustomerID");
                                dtAcctDetails.Columns.Add("JointAccountInd");
                                dtAcctDetails.Columns.Add("AvailableBalance");
                                dtAcctDetails.Columns.Add("LedgerBalance");
                                dtAcctDetails.Columns.Add("UnclearFunds");
                                dtAcctDetails.Columns.Add("PassBookBalance");
                                dtAcctDetails.Columns.Add("BalanceOnHold");

                                drAIResponse = dtAcctDetails.NewRow();
                                drAIResponse[0] = accountInquiryResponse.AcctDetails.UserReferenceNumber;
                                drAIResponse[1] = accountInquiryResponse.AcctDetails.AcctNum;
                                drAIResponse[2] = accountInquiryResponse.AcctDetails.AcctStatus;
                                drAIResponse[3] = accountInquiryResponse.AcctDetails.AcctStatusDesc;
                                drAIResponse[4] = accountInquiryResponse.AcctDetails.BranchCode;
                                drAIResponse[5] = accountInquiryResponse.AcctDetails.BranchName;
                                drAIResponse[6] = accountInquiryResponse.AcctDetails.CurrencyCode;
                                drAIResponse[7] = accountInquiryResponse.AcctDetails.ProductCode;
                                drAIResponse[8] = accountInquiryResponse.AcctDetails.ProductName;
                                drAIResponse[9] = accountInquiryResponse.AcctDetails.AcctType;
                                drAIResponse[10] = accountInquiryResponse.AcctDetails.AcctTypeDesc;
                                drAIResponse[11] = accountInquiryResponse.AcctDetails.OpeningDate;
                                drAIResponse[12] = accountInquiryResponse.AcctDetails.CustomerShortName;
                                drAIResponse[13] = accountInquiryResponse.AcctDetails.CustomerID;
                                drAIResponse[14] = accountInquiryResponse.AcctDetails.JointAccountInd;
                                drAIResponse[15] = accountInquiryResponse.AcctDetails.Balances.AvailableBalance;
                                drAIResponse[16] = accountInquiryResponse.AcctDetails.Balances.LedgerBalance;
                                drAIResponse[17] = accountInquiryResponse.AcctDetails.Balances.UnclearFunds;
                                drAIResponse[18] = accountInquiryResponse.AcctDetails.Balances.PassBookBalance;
                                drAIResponse[19] = accountInquiryResponse.AcctDetails.Balances.BalanceOnHold;
                                dtAcctDetails.Rows.Add(drAIResponse);

                                dtTxnStatus.Columns.Add("ReplyCode");
                                dtTxnStatus.Columns.Add("ReplyText");
                                dtTxnStatus.Columns.Add("TransDate");
                                dtTxnStatus.Columns.Add("UserRef");
                                dtTxnStatus.Columns.Add("GUID");

                                dr = dtTxnStatus.NewRow();
                                dr[0] = accountInquiryResponse.TxnStatus.ReplyCode;
                                dr[1] = accountInquiryResponse.TxnStatus.ReplyText;
                                dr[2] = accountInquiryResponse.TxnStatus.TransDate;
                                dr[3] = param[1].ToString();
                                dr[4] = identifier;
                                dtTxnStatus.Rows.Add(dr);

                                ds.Tables.AddRange(new DataTable[] { dtTxnStatus, dtAcctDetails });
                                success = true;
                            }
                            else
                            {
                                err = ErrorMessage.NoRecordFound.ToString();
                                success = false;
                            }
                            break;

                        case APIMethod.TransactionHistory:
                            TransactionHistoryResponse transactionHistoryResponse = JsonConvert.DeserializeObject<TransactionHistoryResponse>(response);

                            if (transactionHistoryResponse != null)
                            {
                                DataTable dtTranHist = new DataTable("TransactionHistory");
                                DataRow drTHResponse;

                                dtTranHist.Columns.Add("AmountInAccountCurrency");
                                dtTranHist.Columns.Add("Mnemonic");
                                dtTranHist.Columns.Add("OriginatingBranch");
                                dtTranHist.Columns.Add("PostingDate");
                                dtTranHist.Columns.Add("TransactionDate");
                                dtTranHist.Columns.Add("UserReferenceNumber");
                                dtTranHist.Columns.Add("Timestamp");

                                foreach (var tran in transactionHistoryResponse.TransactionListArray.TransactionList)
                                {
                                    drTHResponse = dtTranHist.NewRow();
                                    drTHResponse[0] = tran.AmountInAccountCurrency;
                                    drTHResponse[1] = tran.Mnemonic;
                                    drTHResponse[2] = tran.OriginatingBranch;
                                    drTHResponse[3] = tran.PostingDate;
                                    drTHResponse[4] = tran.TransactionDate;
                                    drTHResponse[5] = tran.UserReferenceNumber;
                                    drTHResponse[6] = tran.Timestamp;
                                    dtTranHist.Rows.Add(drTHResponse);
                                }

                                dtTxnStatus.Columns.Add("ReplyCode");
                                dtTxnStatus.Columns.Add("ReplyText");
                                dtTxnStatus.Columns.Add("TransDate");
                                dtTxnStatus.Columns.Add("UserRef");
                                dtTxnStatus.Columns.Add("GUID");

                                dr = dtTxnStatus.NewRow();
                                dr[0] = transactionHistoryResponse.TxnStatus.ReplyCode;
                                dr[1] = transactionHistoryResponse.TxnStatus.ReplyText;
                                dr[2] = transactionHistoryResponse.TxnStatus.TransDate;
                                dr[3] = param[2].ToString();
                                dr[4] = identifier;
                                dtTxnStatus.Rows.Add(dr);

                                ds.Tables.AddRange(new DataTable[] { dtTxnStatus, dtTranHist });
                                success = true;
                            }
                            else
                            {
                                err = ErrorMessage.NoRecordFound.ToString();
                                success = false;
                            }
                            break;

                        case APIMethod.FundTransfer:
                        case APIMethod.FundTransferV2:
                            FundTransferResponse fundTransferResponse = JsonConvert.DeserializeObject<FundTransferResponse>(response);

                            if (fundTransferResponse != null)
                            {
                                DataTable dtFundTransfer = new DataTable("FundTransfer");
                                DataRow drFundTransfer;

                                dtFundTransfer.Columns.Add("UserReferenceNumber");
                                dtFundTransfer.Columns.Add("RefIdent");

                                drFundTransfer = dtFundTransfer.NewRow();
                                drFundTransfer[0] = fundTransferResponse.UserReferenceNumber;
                                drFundTransfer[1] = fundTransferResponse.RefIdent;
                                dtFundTransfer.Rows.Add(drFundTransfer);

                                dtTxnStatus.Columns.Add("ReplyCode");
                                dtTxnStatus.Columns.Add("ReplyText");
                                dtTxnStatus.Columns.Add("TransDate");
                                dtTxnStatus.Columns.Add("UserRef");
                                dtTxnStatus.Columns.Add("GUID");

                                dr = dtTxnStatus.NewRow();
                                dr[0] = fundTransferResponse.TxnStatus.ReplyCode;
                                dr[1] = fundTransferResponse.TxnStatus.ReplyText;
                                dr[2] = fundTransferResponse.TxnStatus.TransDate;
                                dr[3] = param[6].ToString();
                                dr[4] = identifier;
                                dtTxnStatus.Rows.Add(dr);

                                ds.Tables.AddRange(new DataTable[] { dtTxnStatus, dtFundTransfer });
                                success = true;
                            }
                            else
                            {
                                err = ErrorMessage.NoRecordFound.ToString();
                                success = false;
                            }
                            break;
                    }
                }
                else
                {
                    dtTxnStatus.Columns.Add("ReplyCode");
                    dtTxnStatus.Columns.Add("ReplyText");
                    dtTxnStatus.Columns.Add("TransDate");
                    dtTxnStatus.Columns.Add("UserRef");
                    dtTxnStatus.Columns.Add("GUID");

                    string userref = string.Empty;
                    switch (method)
                    {
                        case APIMethod.AccountInquiry:
                            userref = param[1].ToString();
                            break;

                        case APIMethod.TransactionHistory:
                            userref = param[2].ToString();
                            break;

                        case APIMethod.FundTransfer:
                        case APIMethod.FundTransferV2:
                            userref = param[6].ToString();
                            break;
                    }

                    if (response == null || response == string.Empty)
                    {
                        dr = dtTxnStatus.NewRow();
                        dr[0] = "E99";
                        dr[1] = "No response from interface (E2E) with error: Status Code: " + (int)httpResponse.StatusCode + ", ReasonPhrase: " + httpResponse.ReasonPhrase;
                        dr[2] = DateTime.Now;
                        dr[3] = userref;
                        dr[4] = identifier;
                        dtTxnStatus.Rows.Add(dr);
                    }
                    else
                    {
                        TxnStatusResponse responseStatus = JsonConvert.DeserializeObject<TxnStatusResponse>(response);

                        dr = dtTxnStatus.NewRow();
                        dr[0] = responseStatus.TxnStatus.ReplyCode;
                        dr[1] = responseStatus.TxnStatus.ReplyText;
                        dr[2] = responseStatus.TxnStatus.TransDate;
                        dr[3] = userref;
                        dr[4] = identifier;
                        dtTxnStatus.Rows.Add(dr);
                    }

                    ds.Tables.Add(dtTxnStatus);
                    this.logMe("FAILED: " + method.ToString() + " " + response, identifier);
                }
            }
            catch (Exception ex)
            {
                this.logMe("ERROR: " + method.ToString() + " " + ex.Message + "|" + ex.StackTrace, identifier);
            }
            finally
            {
                this.logMe("END", identifier);
            }
            return ds;
        }

        private bool HostIsDown(string accountNo)
        {
            try
            {
                if (ConfigurationManager.AppSettings["ConnectionString"] != null)
                {
                    _sqlHelper.connectionString = ConfigurationManager.AppSettings["ConnectionString"];
                }
                if (_sqlHelper.connectionString == null || _sqlHelper.connectionString == string.Empty)
                {
                    _sqlHelper.connectionString = ConfigurationManager.AppSettings["DevelConnectionString"];
                }

                /* VALIDATE FOR START TIME */
                string currentTime = System.DateTime.Now.ToString("HH:mm");
                TimeSpan tsCurrent = new TimeSpan(Convert.ToDateTime(currentTime).Hour, Convert.ToDateTime(currentTime).Minute, 0);
                string startTime = string.Empty;
                DataTable dtStart = _sqlHelper.execSQLParameterReader("isp_getLookup", new string[] { "FldGroup" }, new string[] { "HostActivityStart" });

                if (dtStart != null && dtStart.Rows.Count > 0)
                {
                    startTime = dtStart.Rows[0]["FldCode"].ToString();
                    TimeSpan tsStart = new TimeSpan(Convert.ToDateTime(startTime).Hour, Convert.ToDateTime(startTime).Minute, 0);

                    /* IF CURRENT TIME IS >= START TIME THEN PROCEED WITH CHECKING */
                    if (tsCurrent.Ticks >= tsStart.Ticks)
                    {
                        this.logMe("Host checking start", accountNo);
                        DataTable dt = _sqlHelper.execSQLParameterReader("api_hostisdown", null, null);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            /* 0 = START | 1 = STOP */
                            if (dt.Rows[0][0] != DBNull.Value && dt.Rows[0][0].ToString() != "0")
                            {
                                DataTable dtEnd = _sqlHelper.execSQLParameterReader("isp_getLookup", new string[] { "FldGroup" }, new string[] { "HostActivityTiming" });
                                string cutoffTime = string.Empty;

                                if (dtEnd != null && dtEnd.Rows.Count > 0)
                                {
                                    cutoffTime = dtEnd.Rows[0]["FldCode"].ToString();
                                    TimeSpan tsCutoff = new TimeSpan(Convert.ToDateTime(cutoffTime).Hour, Convert.ToDateTime(cutoffTime).Minute, 0);

                                    /* IF CURRENT TIME IS > CUTOFF TIME THEN PROCEED AS ONLINE [START=0] */
                                    if (tsCurrent.Ticks >= tsCutoff.Ticks)
                                    {
                                        this.logMe("Host is up [time-based]", accountNo);
                                        return false;
                                    }
                                }
                                this.logMe("Host is down [stop/start]", accountNo);
                                return true;
                            }
                        }
                        else
                        {
                            this.logMe("No setup found for stop/start", accountNo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.logMe("ERROR: " + ex.Message + "|" + ex.StackTrace, accountNo);
            }
            return false;
        }

        private void BindCertificate()
        {
            if (ConfigurationManager.AppSettings["UsePinnedCertificate"] == "1")
            {
                ServicePointManager.ServerCertificateValidationCallback = CertificateValidationCallBack;
            }
            else
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
            }
            // TLS Handling
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)Convert.ToInt32(ConfigurationManager.AppSettings["TLS"]);
        }

        private bool CertificateValidationCallBack(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            // Step 1 - Convert the .NET 1.1 cert object to a more advanced one (INTERMEDIATE CERT)
            //X509Certificate2 certificate2 = new X509Certificate2(certificate);

            // 1. Get the highest level Certification Authority (CA) in the chain (ROOT CERT)
            X509Certificate2 highestCACertificate = chain.ChainElements[chain.ChainElements.Count - 1].Certificate;

            // 2. Assure that the CA certificate is valid!
            bool isCACertificateValid = highestCACertificate.Verify();

            // 3. Compare the expected thumbprint to the certificate thumbprint. This compare order prevents null pointer exceptions.
            bool isThumbprintAsExpected = ConfigurationManager.AppSettings["CertificateThumbPrint"].ToUpper().Equals(highestCACertificate.Thumbprint.ToUpper());

            return isCACertificateValid && isThumbprintAsExpected;
        }
        #endregion

        #region Validation
        private bool validate(APIMethod method, string[] param)
        {
            bool ret = false;
            bool checknext = false;
            switch (method)
            {
                case APIMethod.AccountInquiry:
                    if (param[0] != null && param[0] != string.Empty && param[0].Length > 0)
                    {
                        checknext = true;
                    }
                    else
                    {
                        ret = false;
                    }
                    if (checknext && param[1] == null || param[1] == string.Empty || param[1].Length <= 0)
                    {
                        ret = false;
                    }
                    if (checknext && param[2] == null || param[2] == string.Empty || param[2].Length <= 0)
                    {
                        ret = false;
                    }
                    if (checknext && param[3] == null || param[3] == string.Empty || param[3].Length <= 0)
                    {
                        ret = false;
                    }
                    else
                    {
                        ret = true;
                    }
                    break;

                case APIMethod.TransactionHistory:
                    if (param[0] != null && param[0] != string.Empty && param[0].Length > 0)
                    {
                        checknext = true;
                    }
                    else
                    {
                        ret = false;
                    }
                    if (checknext && param[1] == null || param[1] == string.Empty || param[1].Length <= 0)
                    {
                        ret = false;
                    }
                    if (checknext && param[2] == null || param[2] == string.Empty || param[2].Length <= 0)
                    {
                        ret = false;
                    }
                    if (checknext && param[3] == null || param[3] == string.Empty || param[3].Length <= 0)
                    {
                        ret = false;
                    }
                    if (checknext && param[4] == null || param[4] == string.Empty || param[4].Length <= 0)
                    {
                        ret = false;
                    }
                    if (checknext && param[5] == null || param[5] == string.Empty || param[5].Length <= 0)
                    {
                        ret = false;
                    }
                    if (checknext && param[6] == null || param[6] == string.Empty || param[6].Length <= 0)
                    {
                        ret = false;
                    }
                    if (checknext && param[7] == null || param[7] == string.Empty || param[7].Length <= 0)
                    {
                        ret = false;
                    }
                    else
                    {
                        ret = true;
                    }
                    break;

                case APIMethod.FundTransfer:
                    if (param[0] != null && param[0] != string.Empty && param[0].Length > 0)
                    {
                        checknext = true;
                    }
                    else
                    {
                        ret = false;
                    }
                    if (checknext && param[1] == null || param[1] == string.Empty || param[1].Length <= 0)
                    {
                        ret = false;
                    }
                    if (checknext && param[2] == null || param[2] == string.Empty || param[2].Length <= 0)
                    {
                        ret = false;
                    }
                    if (checknext && param[3] == null || param[3] == string.Empty || param[3].Length <= 0)
                    {
                        ret = false;
                    }
                    if (checknext && param[4] == null || param[4] == string.Empty || param[4].Length <= 0)
                    {
                        ret = false;
                    }
                    if (checknext && param[5] == null || param[5] == string.Empty || param[5].Length <= 0)
                    {
                        ret = false;
                    }
                    if (checknext && param[6] == null || param[6] == string.Empty || param[6].Length <= 0)
                    {
                        ret = false;
                    }
                    if (checknext && param[7] == null || param[7] == string.Empty || param[7].Length <= 0)
                    {
                        ret = false;
                    }
                    double amountFT = 0.00;
                    try
                    {
                        double.TryParse(param[4], out amountFT);
                    }
                    catch
                    {
                        amountFT = -1;
                    }
                    if (checknext && amountFT <= 0)
                    {
                        ret = false;
                    }
                    else
                    {
                        ret = true;
                    }
                    break;

                case APIMethod.FundTransferV2:
                    if (param[0] != null && param[0] != string.Empty && param[0].Length > 0)
                    {
                        checknext = true;
                    }
                    else
                    {
                        ret = false;
                    }
                    if (checknext && param[1] == null || param[1] == string.Empty || param[1].Length <= 0)
                    {
                        ret = false;
                    }
                    if (checknext && param[2] == null || param[2] == string.Empty || param[2].Length <= 0)
                    {
                        ret = false;
                    }
                    if (checknext && (param[3] == null || param[3] == string.Empty || param[3].Length <= 0) && param[4] == null || param[4] == string.Empty || param[4].Length <= 0)
                    {
                        ret = false;
                    }
                    if (checknext && param[5] == null || param[5] == string.Empty || param[5].Length <= 0)
                    {
                        ret = false;
                    }
                    if (checknext && param[6] == null || param[6] == string.Empty || param[6].Length <= 0)
                    {
                        ret = false;
                    }
                    if (checknext && param[7] == null || param[7] == string.Empty || param[7].Length <= 0)
                    {
                        ret = false;
                    }
                    double amountFTv2 = 0.00;
                    try
                    {
                        double.TryParse(param[4], out amountFTv2);
                    }
                    catch
                    {
                        amountFTv2 = -1;
                    }
                    if (checknext && amountFTv2 <= 0)
                    {
                        ret = false;
                    }
                    else
                    {
                        ret = true;
                    }
                    if (checknext && param[7] == null || param[7] == string.Empty || param[7].Length <= 0)
                    {
                        ret = false;
                    }
                    else
                    {
                        if ((param[7] == "D" || param[7] == "C") && param[11] == null || param[11] == string.Empty || param[11].Length <= 0)
                        {
                            ret = false;
                        }
                    }
                    break;
            }
            return ret;
        }
        #endregion

        #region Logging
        private void logMe(string msg, string identifier)
        {
            try
            {
                string drive = ConfigurationManager.AppSettings["LogDrive"];
                string folder = ConfigurationManager.AppSettings["LogFolder"];
                string subfolder = ConfigurationManager.AppSettings["LogSubFolder"];

                try
                {
                    if (folder != string.Empty && !folder.EndsWith(@"\")) folder += @"\";
                    if (subfolder != string.Empty && !subfolder.EndsWith(@"\")) subfolder += @"\";

                    string path = drive + folder + subfolder;
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string file = path + ConfigurationManager.AppSettings["LogFileName"] + "_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                    FileStream fs = new FileStream(file, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                    string str = DateTime.Now.ToString("MMddyy hh:mm:ss") + " " + identifier + " " + msg;

                    _swlog = new StreamWriter(fs);
                    lock (_wlock)
                    {
                        _swlog.WriteLine(str);
                        _swlog.Flush();
                    }
                }
                catch (Exception ex)
                {
                    writeEvent(folder + ": " + ex.Message + "\r\n" + ex.StackTrace + "\r\n" + msg + "\r\n" + identifier);
                }
                finally
                {
                    if (_swlog != null)
                    {
                        _swlog.Close();
                        _swlog.Dispose();
                    }
                    _swlog = null;
                }
            }
            catch { return; }
        }

        private static void writeEvent(String msg)
        {
            try
            {
                if (!EventLog.SourceExists(_evSourceId))
                {
                    EventLog.CreateEventSource(_evSourceId, _logname);
                }
                EventLog.WriteEntry(_evSourceId, _logname + ": " + msg, EventLogEntryType.Error);
            }
            catch { }
        }
        #endregion
    }
}