using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Data;
using Newtonsoft.Json;
using System.Net.Security;
using System.Net.Http;
using System.Security.Authentication;
using System.Globalization;

namespace IRSClassLib
{
    [Serializable]
    public class BillsPaymentApi : sqlHelperWs
    {
        private readonly string channelId = ConfigurationManager.AppSettings["SSSRTP_ChannelID"];
        private readonly string username = ConfigurationManager.AppSettings["SSSRTP_UName"];
        private readonly string password = ConfigurationManager.AppSettings["SSSRTP_PW"];
        public string _RemitterId = string.Empty;
        public string _BeneficiaryId = string.Empty;
        public string _MerchantCode = string.Empty;
        public string _ReferenceNo = string.Empty;

        public enum BillsPayment
        {
            VALIDATE,
            POST,
            GETSTATUS
        }

        // AUTH
        public class AuthRequest
        {
            public string username { get; set; }
            public string password { get; set; }
        }

        public class AuthResponse
        {
            public string token { get; set; }
            public string expires_in { get; set; }
            public string time_to_live { get; set; }
            public string responseCode { get; set; }
            public string responseDesc { get; set; }
        }


        // VALIDATE
        public class ValidateRequest
        {
            public string channelId { get; set; }
            public string billerCode { get; set; }
            public string subscriberName { get; set; }
            public string subscriberNumber { get; set; }
            public string txnAmount { get; set; }
            public string txnDate { get; set; }
            public string txnTime { get; set; }
        }

        public class ValidateResponse
        {
            public string instructionId { get; set; }
            public string txnDateTime { get; set; }
            public string txnStatus { get; set; }
            public string responseCode { get; set; }
            public string responseDesc { get; set; }
            [JsonProperty("extraFields")]
            public List<extraFields> extraFields { get; set; }
        }

        // POST
        public class PostRequest
        {
            public string instructionId { get; set; }
            public string channelId { get; set; }
            public string billerCode { get; set; }
            public string subscriberName { get; set; }
            public string subscriberNumber { get; set; }
            public string txnAmount { get; set; }
            public string txnDate { get; set; }
            public string txnTime { get; set; }
            [JsonProperty("extraFields")]
            public List<extraFields> extraFields { get; set; }
        }

        public class PostResponse
        {
            public string instructionId { get; set; }
            public string txnDateTime { get; set; }
            public string txnStatus { get; set; }
            public string responseCode { get; set; }
            public string responseDesc { get; set; }
        }

        // STATUS
        public class StatusResponse
        {
            public string txnStatus { get; set; }
            public string responseCode { get; set; }
            public string responseDesc { get; set; }
        }

        //extraFields
        public class extraFields
        {
            public string key { get; set; }
            public string value { get; set; }
        }



        #region PROCESS
        public string ConnectToRTP(string subscriberName, string subscriberNumber, decimal amount, string bc_code, string instructionId, string referenceno, string endpoint)
        {
            AuthResponse auth = null;
            ValidateResponse validate = null;
            PostResponse post = null;
            StatusResponse status = null;
            DataTable dtRTPLookup = new DataTable();
            string ret = string.Empty;
            string billerCode = string.Empty;
            string txnDate = string.Empty;
            string txnTime = string.Empty;
            string txnAmount = string.Empty;
            int retry = 1;

            if (System.Web.HttpContext.Current.Session["RTPConfiguration"] == null)
            {
                GetRtpSetup();
                dtRTPLookup = (DataTable)System.Web.HttpContext.Current.Session["RTPConfiguration"];
            }
            else
            {
                dtRTPLookup = (DataTable)System.Web.HttpContext.Current.Session["RTPConfiguration"];
            }


            var lookupDict = dtRTPLookup.AsEnumerable()
                .ToDictionary(r => r.Field<string>("FldCode"),
                              r => r.Field<string>("FldDescription"));

            retry = Convert.ToInt32(lookupDict["RETRY"]);

            billerCode = lookupDict[bc_code].ToString();
            txnDate = DateTime.Now.ToString("MMdd");
            txnTime = DateTime.Now.ToString("HHmmss");
            txnAmount = (amount > 0 ? amount.ToString("F2") : "");
            _ReferenceNo = (referenceno != "" ? referenceno.ToString() : "");

            if (endpoint.ToUpper() == BillsPayment.VALIDATE.ToString())
            {
                for (int ConnectToRTP_VALIDATE_auth = 0; ConnectToRTP_VALIDATE_auth < retry; ConnectToRTP_VALIDATE_auth++)
                {
                    // AUTH
                    auth = Authenticate();
                    if (auth != null && auth.responseCode == "00")
                    {
                        // VALIDATE
                        validate = Validate(auth.token, channelId, billerCode,
                                                             subscriberName, subscriberNumber,
                                                             txnAmount, txnDate, txnTime);

                        if (validate?.extraFields != null)
                        {
                            ret = string.Join("~", validate.extraFields.Select(x => $"{x.value}"));
                        }
                        else
                        {
                            ret = validate.instructionId + "~" + validate.responseCode + "~" + validate.responseDesc + "~" + validate.txnStatus;

                        }
                        break;
                    }
                }
            }
            else if (endpoint.ToUpper() == BillsPayment.POST.ToString())
            {
                for (int ConnectToRTP_POST_auth = 0; ConnectToRTP_POST_auth < retry; ConnectToRTP_POST_auth++)
                {
                    // AUTH
                    auth = Authenticate();
                    if (auth != null && auth.responseCode == "00")
                    {
                        // POST
                        post = Post(auth.token, instructionId,
                                             channelId, billerCode,
                                             subscriberName, subscriberNumber,
                                             txnAmount, txnDate, txnTime);

                        // STATUS
                        if (post != null)
                        {
                            if (validate?.extraFields != null)
                            {
                                ret = post.instructionId + "~" + post.responseCode + "~" + post.responseDesc + "~" + post.txnStatus;
                            }
                            else
                            {
                                ret = post.instructionId + "~" + post.responseCode + "~" + post.responseDesc + "~" + post.txnStatus;

                            }
                            break;
                        }
                    }
                }
            }
            else if (endpoint.ToUpper() == BillsPayment.GETSTATUS.ToString())
            {
                for (int ConnectToRTP_POST_auth = 0; ConnectToRTP_POST_auth < retry; ConnectToRTP_POST_auth++)
                {
                    // AUTH
                    auth = Authenticate();
                    if (auth != null && auth.responseCode == "00")
                    {
                        status = GetStatus(instructionId, auth.token);
                        if (status.responseCode == "01")
                        {
                            ret = status.responseCode + "~" + status.responseDesc + "~" + status.txnStatus;
                            break;
                        }
                    }
                }
            }
            return ret;
        }


        #endregion

        #region AUTH


        private AuthResponse Authenticate()
        {
            try
            {
                string url = "auth";

                AuthRequest requestObj = new AuthRequest
                {
                    username = username,
                    password = password
                };

                object response = Send(url, "POST", JsonConvert.SerializeObject(requestObj), null);
                if (response != null)
                {
                    return JsonConvert.DeserializeObject<AuthResponse>(response.ToString());
                }
                else
                {
                    new csAuditLog().writeAudit("", "BillsPaymentAPI", "Authenticate", "RTP response is null", "", csAuditLog.Category.RTPAPI);
                    return null;
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "BillsPaymentAPI", "Authenticate", ex.Message.ToString(), ex.StackTrace.ToString(), csAuditLog.Category.RTPAPI);
                return null;
            }
        }

        #endregion

        #region VALIDATE

        private ValidateResponse Validate(string token, string channelId, string billerCode,
                                          string name, string number, string amount,
                                          string date, string time)
        {
            /* From extraFields
            0 - payeeName
            1 - payType
            2 - respTime
            3 - amtDue
            4 - resCode
            5 - applDtFr
            6 - applDtTo
            7 - resMsg
            8 - prn 
            */
            string url = "validate";

            try
            {
                ValidateRequest req = new ValidateRequest
                {
                    channelId = channelId,
                    billerCode = billerCode,
                    subscriberName = name,
                    subscriberNumber = number,
                    txnAmount = amount,
                    txnDate = date,
                    txnTime = time
                };

                object response = Send(url, "POST", JsonConvert.SerializeObject(req), token);
                if (response != null)
                {
                    ValidateResponse res = JsonConvert.DeserializeObject<ValidateResponse>(response.ToString());
                    if (res.responseCode == "01")
                    {
                        string payType = res.extraFields.FirstOrDefault(x => x.key == "payType")?.value;
                        string amtDue = res.extraFields.FirstOrDefault(x => x.key == "amtDue")?.value;
                        string PaymentFrom = ConvertToDate(res.extraFields.FirstOrDefault(x => x.key == "applDtFr")?.value);
                        string PaymentTo = ConvertToDate(res.extraFields.FirstOrDefault(x => x.key == "applDtTo")?.value);
                        string MemberName = res.extraFields.FirstOrDefault(x => x.key == "payeeName")?.value;
                        saveBillerRTP(channelId, res.instructionId.ToString(), _ReferenceNo, number, MemberName, billerCode, "1", amtDue.Replace(",", ""), PaymentFrom, PaymentTo, JsonConvert.SerializeObject(req), JsonConvert.SerializeObject(res), 0, 0);
                        return res;
                    }
                    else if (res.responseCode == "00")
                    {
                        string payType = res.extraFields.FirstOrDefault(x => x.key == "payType")?.value;
                        string amtDue = res.extraFields.FirstOrDefault(x => x.key == "amtDue")?.value;
                        string PaymentFrom = res.extraFields.FirstOrDefault(x => x.key == "applDtFr")?.value;
                        string PaymentTo = res.extraFields.FirstOrDefault(x => x.key == "applDtTo")?.value;
                        string MemberName = res.extraFields.FirstOrDefault(x => x.key == "payeeName")?.value;
                        saveBillerRTP(channelId, res.instructionId.ToString(), _ReferenceNo, number, MemberName, billerCode, "1", amtDue, PaymentFrom, PaymentTo, JsonConvert.SerializeObject(req), JsonConvert.SerializeObject(res), 0, 0);
                        return res;
                    }
                    else
                    {
                        new csAuditLog().writeAudit("", "BillsPaymentAPI", "Validation", "PRN: " + number, "InstructionId: " + res.instructionId + "|Respose Status: " + res.txnStatus, csAuditLog.Category.RTPAPI);
                        return res;
                    }
                }
                else
                {
                    new csAuditLog().writeAudit("", "BillsPaymentAPI", "Validation", "PRN: " + number, "RTP Response is null", csAuditLog.Category.RTPAPI);
                    return null;
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "BillsPaymentAPI", "Validation", "PRN: " + number, ex.Message.ToString() + "|" + ex.StackTrace.ToString(), csAuditLog.Category.RTPAPI);
                return null;
            }

        }

        #endregion

        #region POST

        private PostResponse Post(string token, string instructionId,
                                  string channelId, string billerCode,
                                  string name, string number,
                                  string amount, string date, string time)
        {
            string url = "post";
            string paymentfrom = string.Empty;
            string paymentto = string.Empty;
            string MemberName = string.Empty;
            try
            {
                PostRequest req = new PostRequest
                {
                    instructionId = instructionId,
                    channelId = channelId,
                    billerCode = billerCode,
                    subscriberName = name,
                    subscriberNumber = number,
                    txnAmount = amount,
                    txnDate = date,
                    txnTime = time
                };
                object response = Send(url, "POST", JsonConvert.SerializeObject(req), token);
                if (response != null)
                {
                    PostResponse res = JsonConvert.DeserializeObject<PostResponse>(response.ToString());
                    if (res.responseCode == "00")
                    {
                        DataTable dt_InstructionIDDetails = this.getRTPInstructionID_details(number, _ReferenceNo);
                        if (dt_InstructionIDDetails != null && dt_InstructionIDDetails.Rows.Count > 0)
                        {
                            paymentfrom = dt_InstructionIDDetails.Rows[0]["PaymentFrom"].ToString();
                            paymentto = dt_InstructionIDDetails.Rows[0]["PaymentTo"].ToString();
                            MemberName = dt_InstructionIDDetails.Rows[0]["MemberName"].ToString();
                        }
                        if (paymentfrom == "" || paymentfrom == null)
                        {
                            paymentfrom = date;
                            paymentto = date;
                        }
                        if (MemberName == "" || MemberName == null)
                        { MemberName = name; }
                        saveBillerRTP(channelId, res.instructionId.ToString(), _ReferenceNo, number, MemberName, billerCode, "1", amount, paymentfrom, paymentto, JsonConvert.SerializeObject(req), JsonConvert.SerializeObject(res), 1, 1);
                        return res;
                    }
                    else if (res.responseCode == "01")
                    {
                        DataTable dt_InstructionIDDetails = this.getRTPInstructionID_details(number, _ReferenceNo);
                        if (dt_InstructionIDDetails != null && dt_InstructionIDDetails.Rows.Count > 0)
                        {
                            paymentfrom = dt_InstructionIDDetails.Rows[0]["PaymentFrom"].ToString();
                            paymentto = dt_InstructionIDDetails.Rows[0]["PaymentTo"].ToString();
                            MemberName = dt_InstructionIDDetails.Rows[0]["MemberName"].ToString();
                        }
                        if (paymentfrom == "" || paymentfrom == null)
                        {
                            paymentfrom = date;
                            paymentto = date;
                        }
                        if (MemberName == "" || MemberName == null)
                        { MemberName = name; }
                        saveBillerRTP(channelId, res.instructionId.ToString(), _ReferenceNo, number, name, billerCode, "1", amount, paymentfrom, paymentto, JsonConvert.SerializeObject(req), JsonConvert.SerializeObject(res), 1, 1);
                        return res;
                    }
                    else
                    {
                        new csAuditLog().writeAudit("", "BillsPaymentAPI", "Post", "PRN: " + number, "InstructionId: " + res.instructionId + "|Respose Status: " + res.txnStatus, csAuditLog.Category.RTPAPI);
                        return res;
                    }
                }
                else
                {
                    new csAuditLog().writeAudit("", "BillsPaymentAPI", "Post", "PRN: " + number, "RTP response is null", csAuditLog.Category.RTPAPI);
                    return null;
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "BillsPaymentAPI", "Post", "PRN: " + number, ex.Message.ToString() + "|" + ex.StackTrace.ToString(), csAuditLog.Category.RTPAPI);
                return null;
            }

        }

        #endregion

        #region STATUS

        private StatusResponse GetStatus(string instructionId, string token)
        {
            string url = "status/" + instructionId;

            object response = Send(url, "GET", null, token);
            if (response != null)
            {
                return JsonConvert.DeserializeObject<StatusResponse>(response.ToString());
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region HTTP CORE


        private static readonly HttpClient client = new HttpClient(new HttpClientHandler
        {
            SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13
        });


        private object Send(string endpoint, string method, string json, string token)
        {
            try
            {
                DataTable dtRTPLookup = new DataTable();
                string url = null;
                if (System.Web.HttpContext.Current.Session["RTPConfiguration"] == null)
                {
                    GetRtpSetup();
                    dtRTPLookup = (DataTable)System.Web.HttpContext.Current.Session["RTPConfiguration"];
                }
                else
                {
                    dtRTPLookup = (DataTable)System.Web.HttpContext.Current.Session["RTPConfiguration"];
                }

                var lookupDict = dtRTPLookup.AsEnumerable()
                    .ToDictionary(r => r.Field<string>("FldCode"),
                                  r => r.Field<string>("FldDescription"));

                string baseURL = lookupDict["URI"];
                string version = lookupDict["VERSION"];
                string log = lookupDict["LOG"];
                string timeout = lookupDict["TIMEOUT"];
                url = baseURL + endpoint;

                var request = new HttpRequestMessage(
                    method == "POST" ? HttpMethod.Post : HttpMethod.Get,
                    url
                );

                // ✅ Set headers per request (SAFE)
                request.Headers.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
                );

                request.Headers.Add("API-Version", version);

                if (!string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

                // ✅ Set content
                if (method == "POST" && json != null)
                {
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                }

                // ✅ Send request
                var response = client.SendAsync(request).Result;
                if (response != null)
                {
                    var responseString = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<object>(responseString);
                }
                else
                {
                    new csAuditLog().writeAudit("", "BillsPaymentAPI", "Send.REQUEST/RESPONSE", "RPT response is null", json + "~" + url, csAuditLog.Category.RTPAPI);
                    return null;
                }

            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "BillsPaymentAPI", "Send", "jsonBody:" + json, ex.Message.ToString() + "|" + ex.StackTrace.ToString(), csAuditLog.Category.RTPAPI);
                return null;
            }
        }

        private string getUserId()
        {
            string userId = string.Empty;
            try
            {
                userId = ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId;
            }
            catch (Exception ex)
            {
                userId = "Session Expired";
                new csAuditLog().writeAudit("", "GET USER  ID ERROR", "getUserId", ex.Message + "|" + ex.StackTrace, "", csAuditLog.Category.RTPAPI);
            }
            return userId;
        }

        private string getLocation()
        {
            string location = string.Empty;
            try
            {
                location = ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"])._location;
            }
            catch (Exception ex)
            {
                location = "Session Expired";
                new csAuditLog().writeAudit("", "GET USER  ID ERROR", "getLocation", ex.Message + "|" + ex.StackTrace, "", csAuditLog.Category.RTPAPI);
            }
            return location;
        }

        #endregion

        #region DB STUBS

        private int saveBillerRTP(string ChannelID, string InstructionID, string ReferenceNo, string MemberID, string MemberName, string MerchantCode, string Currency, string Amount, string PaymentFrom, string PaymentTo, string RequestAPI, string ResponeAPI, int IsPost, int IsPaid)
        {
            int ok = 0;
            DateTime dtBranchTime = new irsTimeZone().branchLocalTime(getLocation());
            string userid = getUserId();
            try
            {
                ok = base.execStoredProcedureUpdate("saveBillerRTP", new Object[] { ChannelID, InstructionID, ReferenceNo, MemberID, MemberName, MerchantCode, Currency, Amount, PaymentFrom, PaymentTo, RequestAPI, ResponeAPI, IsPost, IsPaid });
                new csAuditLog().writeAudit(userid, "BillsPaymentAPI", "saveBillerRTP", "Failed to save MemberID: " + MemberID + "][InstructionId: " + InstructionID + "[MerchantCode = " + MerchantCode.ToString() + "]", "saveBillerRTP", csAuditLog.Category.RTPAPI);
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit(userid, "BillsPaymentAPI", "saveBillerRTP", "ERROR UPDATE REMITTANCE: [ErrorMessage=" + ex.Message + "][MemberID=" + MemberID.ToString() + "][MerchantCode=" + MerchantCode.ToString() + "]", "saveBillerRTP", csAuditLog.Category.RTPAPI);
                return ok;
            }
            return ok;
        }

        private void setBase(string SQL, bool IsAdd)
        {
            base.getUpdateDataSet(getSqlCommand(SQL, false));
            if (IsAdd)
            {
                base.newRow(sqlHelperWs.RETAIN_DATASET);
            }
            else
            {
                selectedRow = 0;
            }
        }

        public void GetRtpSetup()
        {
            if (System.Web.HttpContext.Current.Session["RTPConfiguration"] == null)
            {
                DataTable dtLookup = new irsInternetPage().getLookup("RTPConfiguration");
                if (dtLookup != null && dtLookup.Rows.Count > 0)
                {
                    System.Web.HttpContext.Current.Session["RTPConfiguration"] = dtLookup;
                }
            }
        }

        /* DMND0002382 -Remittance SSS Enhanced e-Collection Service */
        public DataTable getRTPInstructionID_details(string prn_number, string referenceno)
        {

            try
            {
                base.EnforceConstraint = false;
                base.upds = null;
                getUpdateDS("getRTPInstructionID_details", new Object[] { prn_number, referenceno });
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
            catch (Exception e)
            {
                return null;
            }
        }


        #endregion

        #region OTHER FUNCTIONS



        public static string ConvertToDate(string monthYear)
        {
            DateTime date = DateTime.ParseExact(
                monthYear,
                "MMMM yyyy",
                CultureInfo.InvariantCulture);

            return date.ToString("yyyy-MM-dd");
        }

        #endregion
    }
}
