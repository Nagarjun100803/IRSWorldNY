using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Authentication;
using System.Text;

namespace IRSClassLib
{
    [Serializable]
    public class irsRefinitiv : sqlHelperWs
    {
        public class ReplyStatus
        {
            public int statusCode { get; set; }
            public string message { get; set; }
            public string userRef { get; set; }
        }

        /**************************************************************/
        #region GroupId
        public class APIGroupIDDto
        {
            public string identifier { get; set; }
            public string groupName { get; set; }
        }

        public class GroupIDResponseDto
        {
            public ReplyStatus replyStatus { get; set; }
            public string responseBody { get; set; }
        }
        #endregion
        /**************************************************************/

        /**************************************************************/
        #region Provider
        public class APIProviderDto
        {
            public string identifier { get; set; }
        }

        public class ProviderResponseDto
        {
            public ReplyStatus replyStatus { get; set; }
            public List<ResponseBodyProvider> responseBody { get; set; }
        }

        public class ResponseBodyProvider
        {
            public string identifier { get; set; }
            public string code { get; set; }
            public string name { get; set; }
            public bool master { get; set; }
            public List<Source> sources { get; set; }
        }

        public class Source
        {
            public string abbreviation { get; set; }
            public DateTime creationDate { get; set; }
            public string identifier { get; set; }
            public string importIdentifier { get; set; }
            public string name { get; set; }
            public object provider { get; set; }
            public string providerSourceStatus { get; set; }
            public string regionOfAuthority { get; set; }
            public string subscriptionCategory { get; set; }
            public Type type { get; set; }
        }

        public class Type
        {
            public Category category { get; set; }
            public string identifier { get; set; }
            public string name { get; set; }
        }

        public class Category
        {
            public string description { get; set; }
            public string identifier { get; set; }
            public string name { get; set; }
            public List<object> providerSourceTypes { get; set; }
        }
        #endregion
        /**************************************************************/

        /**************************************************************/
        #region Reference
        public class APIResolveReferenceDto
        {
            public string identifier { get; set; }
            public string groupId { get; set; }
        }

        public class ResolveReferenceResponseDto
        {
            public ReplyStatus replyStatus { get; set; }
            public ResolveReferenceResponseBody responseBody { get; set; }
        }

        public class ResolveReferenceResponseBody
        {
            public WATCHLIST WATCHLIST { get; set; }
            public WATCHLIST CLIENT_WATCHLIST { get; set; }
        }

        public class WATCHLIST
        {
            public string groupId { get; set; }
            public string providerType { get; set; }
            [JsonProperty("resolutionFields")]
            public ResolutionFields resolutionFields { get; set; }
        }

        public class ResolutionFields
        {
            public List<Values> statuses { get; set; }
            public List<Values> risks { get; set; }
            public List<Values> reasons { get; set; }
        }

        public class ResolutionRules
        {
            public string[] reasons { get; set; }
            public string remarkRequired { get; set; }
            public string reasonRequired { get; set; }
            public string[] risks { get; set; }
        }

        public class Values
        {
            public string id { get; set; }
            public string label { get; set; }
            public string type { get; set; }
        }

        public class ResolutionToolkitResponseDto
        {
            public string groupId { get; set; }
            public string providerType { get; set; }
            [JsonProperty("resolutionFields")]
            public ResolutionFields resolutionFields { get; set; }

            public class ResolutionFields
            {
                [JsonProperty("statuses")]
                public List<Values> statuses { get; set; }
                public List<Values> risks { get; set; }
                public List<Values> reasons { get; set; }
            }

            public class Values
            {
                public string id { get; set; }
                public string label { get; set; }
                public string type { get; set; }
            }

            public class ResolutionRules
            {
                public string[] reasons { get; set; }
                public string remarkRequired { get; set; }
                public string reasonRequired { get; set; }
                public string[] risks { get; set; }
            }
        }
        #endregion
        /**************************************************************/

        /**************************************************************/
        #region APIScreenMyName
        public class APIScreenMyNameDto
        {
            public string identifier { get; set; }
            public string groupId { get; set; }
            public string entityType { get; set; }
            public string caseId { get; set; }
            public string providerTypes { get; set; }
            public string name { get; set; }
            public bool nameTransposition { get; set; }
            public string caseScreeningState { get; set; }
            public string gender { get; set; }
            public string dateOfBirth { get; set; }
            public string placeOfBirth { get; set; }
            public string countryOfBirth { get; set; }
            public string nationality { get; set; }
            public string organizationCountry { get; set; }
        }

        public class ScreeningResponseDto
        {
            public ReplyStatus replyStatus { get; set; }
            public ScreeningResponseBody responseBody { get; set; }
        }

        public class ScreeningResponseBody
        {
            public string caseId { get; set; }
            public string name { get; set; }
            public List<string> providerTypes { get; set; }
            public List<object> customFields { get; set; }
            public List<object> secondaryFields { get; set; }
            public string groupId { get; set; }
            public string entityType { get; set; }
            public string caseSystemId { get; set; }
            public CaseScreeningState caseScreeningState { get; set; }
            public string lifecycleState { get; set; }
            public Creator creator { get; set; }
            public Modifier modifier { get; set; }
            public object assignee { get; set; }
            public DateTime creationDate { get; set; }
            public DateTime modificationDate { get; set; }
            public bool nameTransposition { get; set; }
            public bool outstandingActions { get; set; }
            public LastScreenedDatesByProviderType lastScreenedDatesByProviderType { get; set; }
            public List<Result> results { get; set; }
        }

        public class LastScreenedDatesByProviderType
        {
            public DateTime WATCHLIST { get; set; }
            public DateTime CLIENT_WATCHLIST { get; set; }
        }

        public class CaseScreeningState
        {
            public string WATCHLIST { get; set; }
        }

        public class Creator
        {
            public string userId { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string fullName { get; set; }
            public string email { get; set; }
            public string status { get; set; }
        }

        public class Modifier
        {
            public string userId { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string fullName { get; set; }
            public string email { get; set; }
            public string status { get; set; }
        }

        public class Result
        {
            public string resultId { get; set; }
            public string referenceId { get; set; }
            public string matchScore { get; set; }
            public string matchStrength { get; set; }
            public string matchedTerm { get; set; }
            public List<MatchTerm> matchedTerms { get; set; }
            public string submittedTerm { get; set; }
            public string matchedNameType { get; set; }
            public List<object> secondaryFieldResults { get; set; }
            public List<string> sources { get; set; }
            public List<string> categories { get; set; }
            public DateTime creationDate { get; set; }
            public DateTime modificationDate { get; set; }
            public DateTime lastAlertDate { get; set; }
            public Resolution resolution { get; set; }
            public ResultReview resultReview { get; set; }
            public string primaryName { get; set; }
            public List<Event> events { get; set; }
            public List<CountryLink> countryLinks { get; set; }
            public List<IdentityDocument> identityDocuments { get; set; }
            public string category { get; set; }
            public string providerType { get; set; }
            public string gender { get; set; }
            public string entityCreationDate { get; set; }
            public string entityModificationDate { get; set; }
            public List<object> actionTypes { get; set; }
            public string pepStatus { get; set; }
        }

        public class MatchTerm
        {
            public string term { get; set; }
            public string type { get; set; }
        }

        public class ResultReview
        {
            public bool reviewRequired { get; set; }
            public DateTime reviewRequiredDate { get; set; }
            public object reviewRemark { get; set; }
            public object reviewDate { get; set; }
        }

        public class Resolution
        {
            public string statusId { get; set; }
            public string riskId { get; set; }
            public string reasonId { get; set; }
            public string resolutionRemark { get; set; }
            public DateTime resolutionDate { get; set; }
        }

        public class Event
        {
            public Address address { get; set; }
            public List<object> allegedAddresses { get; set; }
            public int? day { get; set; }
            public string fullDate { get; set; }
            public int? month { get; set; }
            public string type { get; set; }
            public int? year { get; set; }
        }

        public class CountryLink
        {
            public Country country { get; set; }
            public string countryText { get; set; }
            public string type { get; set; }
        }

        public class Country
        {
            public string code { get; set; }
            public string name { get; set; }
        }

        public class IdentityDocument
        {
            public string[] entity { get; set; }
            public string expiryDate { get; set; }
            public string issueDate { get; set; }
            public string issuer { get; set; }
            public LocationType locationType { get; set; }
            public string number { get; set; }
            public string type { get; set; }
        }

        public class LocationType
        {
            public Country country { get; set; }
            public string name { get; set; }
            public string type { get; set; }
            public string[] entityTypes { get; set; }
        }

        public class Address
        {
            public object city { get; set; }
            public Country country { get; set; }
            public object postCode { get; set; }
            public string region { get; set; }
            public object street { get; set; }
        }
        #endregion
        /**************************************************************/

        /**************************************************************/
        #region APIScreenMyNameBulk
        public class APIScreenMyNameBulkDto
        {
            public string identifier { get; set; }
            public string groupId { get; set; }
            public string providerTypes { get; set; }
            public bool nameTransposition { get; set; }
            public string caseScreeningState { get; set; }
            public List<Case> cases { get; set; }
        }

        public class Case
        {
            public string caseId { get; set; }
            public string entityType { get; set; }
            public string name { get; set; }
        }

        public class ScreeningResponseBulkDto
        {
            public ReplyStatus replyStatus { get; set; }
            public List<ScreeningResponseBulkBody> responseBody { get; set; }
        }

        public class ScreeningResponseBulkBody
        {
            public string name { get; set; }
            public string caseId { get; set; }
            public string caseSystemId { get; set; }
            public bool nameTransposition { get; set; }
            public CaseScreeningState caseScreeningState { get; set; }
        }
        #endregion

        /**************************************************************/

        /**************************************************************/
        #region Result
        public class APIResultDto
        {
            public string identifier { get; set; }
            public string groupId { get; set; }
            public string caseId { get; set; }
            public string caseSystemId { get; set; }
            public bool screenCase { get; set; }
        }

        public class ResultResponseDto
        {
            public ReplyStatus replyStatus { get; set; }
            public List<Result> responseBody { get; set; }
        }
        #endregion
        /**************************************************************/

        /**************************************************************/
        #region Resolve
        public class APIResolveDto
        {
            public string identifier { get; set; }
            public int resolveType { get; set; }
            public string groupId { get; set; }
            public string caseId { get; set; }
            public string caseSystemId { get; set; }
            public string resultId { get; set; }
            public string statusId { get; set; }
            public string riskId { get; set; }
            public string reasonId { get; set; }
            public string resolutionRemarks { get; set; }
            public bool skipValidation { get; set; }
        }

        public class ResolveResponseDto
        {
            public ReplyStatus replyStatus { get; set; }
            public string responseBody { get; set; }
        }
        #endregion
        /**************************************************************/

        /**************************************************************/
        #region ArchiveDelete
        public class APIArchiveDeleteDto
        {
            public string identifier { get; set; }
            public string caseSystemIds { get; set; }
            public bool archiveOnly { get; set; }
        }

        public class ArchiveDeleteResponseDto
        {
            public ReplyStatus replyStatus { get; set; }
            public List<ResponseBody> responseBody { get; set; }
        }

        public class ResponseBody
        {
            public string caseSystemId { get; set; }
            public string success { get; set; }
            public string error { get; set; }
        }
        #endregion
        /**************************************************************/

        /**************************************************************/
        #region Records
        public class APIRecordsDto
        {
            public string identifier { get; set; }
            public string recordId { get; set; }
        }

        public class RecordsResponseDto
        {
            public ReplyStatus replyStatus { get; set; }
            public RecordsResponseBody responseBody { get; set; }
        }

        public class RecordsResponseBody
        {
            public List<RecordsDetail> details { get; set; }
            public List<RecordsName> names { get; set; }
        }

        public class RecordsDetail
        {
            public string detailType { get; set; }
            public string text { get; set; }
        }

        public class RecordsName
        {
            public string fullName { get; set; }
        }
        #endregion
        /**************************************************************/

        public bool IsRefinitiv()
        {
            bool ret = false;
            DataTable dt = new DataTable();
            string flag = string.Empty;

            try
            {
                if (System.Web.HttpContext.Current.Session["RefinitivConfiguration"] == null)
                {
                    dt = new irsInternetPage().getLookup("RefinitivConfiguration");
                    System.Web.HttpContext.Current.Session["RefinitivConfiguration"] = dt;
                }
                else
                {
                    dt = (DataTable)System.Web.HttpContext.Current.Session["RefinitivConfiguration"];
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    flag = dt.AsEnumerable().Where(x => x.Field<string>("FldCode") == "Flag").FirstOrDefault().Field<string>("FldDescription");
                }

                if (flag == "1")
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "CHECK REFINITIV FLAG ERROR", "IsRefinitiv", ex.Message, ex.StackTrace, csAuditLog.Category.Refinitiv);
            }
            return ret;
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
                new csAuditLog().writeAudit("", "GET USER  ID ERROR", "getUserId", ex.Message + "|" + ex.StackTrace, "", csAuditLog.Category.Refinitiv);
            }
            return userId;
        }

        public enum httpRes
        {
            Timedout = 408,
            ServiceUnavilable = 503
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

        public bool validateISO3Country(string CountryCode)
        {
            bool found = false;
            try
            {
                DataTable dtISO3Country = new DataTable();
                int i = 0;
                DataRow data;

                if (System.Web.HttpContext.Current.Session["RefinitivISO3Country"] == null)
                {
                    dtISO3Country = new irsInternetPage().getLookup("SAS_ISO3_Country");
                    System.Web.HttpContext.Current.Session["RefinitivISO3Country"] = dtISO3Country;
                }
                else
                {
                    dtISO3Country = (DataTable)System.Web.HttpContext.Current.Session["RefinitivISO3Country"];
                }
                if (dtISO3Country != null && dtISO3Country.Rows.Count > 0)
                {
                    data = dtISO3Country.AsEnumerable().FirstOrDefault();
                    for (i = 0; i <= (dtISO3Country.Rows.Count - 1); i++)
                    {
                        if (dtISO3Country.Rows[i]["FldCode"].ToString().ToUpper() == CountryCode.ToUpper())
                        {
                            found = true;
                            break;
                        }
                    }
                }
            }
            catch { }
            return found;
        }

        private object ConnectToRefinitiv(string json, string endpoint, string timeout)
        {
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                | SecurityProtocolType.Tls11
                | SecurityProtocolType.Tls12
                | SecurityProtocolType.Tls13;

            object obj = null;
            HttpClient client = new HttpClient();
            HttpResponseMessage httpResponse = null;
            string response = string.Empty;

            try
            {
                HttpClientHandler clientHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; },
                    SslProtocols = SslProtocols.Tls13 | SslProtocols.Tls12,
                    UseProxy = false,
                    Proxy = null
                };

                json = System.Text.RegularExpressions.Regex.Unescape(json);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                string url = ConfigurationManager.AppSettings["RAPI_REFINITIV_URI"] + endpoint;

                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("UserID", ConfigurationManager.AppSettings["RAPI_IP_User"].ToString());
                client.DefaultRequestHeaders.Add("Password", ConfigurationManager.AppSettings["RAPI_IP_Pass"].ToString());
                client.Timeout = TimeSpan.FromMilliseconds(Convert.ToInt32(timeout));

                httpResponse = client.PostAsync(url, data).Result;
                byte[] byteArray = httpResponse.Content.ReadAsByteArrayAsync().Result;
                response = Encoding.UTF8.GetString(byteArray);

                if (response.IndexOf("The service is unavailable") >= 0)
                {
                    return httpRes.ServiceUnavilable;
                }
                if (response.IndexOf("WC1~ERROR: 0|Error|") >= 0)
                {
                    return httpRes.Timedout;
                }

                string log = new irsInternetPage().getLookupValue("RefinitivConfiguration", "FldCode", "Log", "FldDescription");
                if (log == "1")
                {
                    new csAuditLog().writeAudit("", "REFINITIV", "REQUEST/RESPONSE", response, json + "~" + endpoint, csAuditLog.Category.Refinitiv);
                }
                obj = JsonConvert.DeserializeObject<object>(response);
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "REFINITIV ERROR", "ConnectToRefinitiv", ex.Message + "|" + ex.StackTrace, json + "~" + endpoint, csAuditLog.Category.Refinitiv);
                return null;
            }
            return obj;
        }

        public DataTable getRefinitivByCaseId(string CIFID)
        {
            base.EnforceConstraint = false;
            getUpdateDS("rapi_getRefinitivCaseId", new Object[] { CIFID });
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

        public DataTable getRefinitivByCaseSystemID(string CaseSystemID)
        {
            base.EnforceConstraint = false;
            getUpdateDS("rapi_getRefinitivCaseSystemId", new Object[] { CaseSystemID });
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

        public DataTable getCIFInfo(string CIFID)
        {
            base.EnforceConstraint = false;
            getUpdateDS("rapi_getCIFInfo", new Object[] { CIFID });
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

        public string getBranchRegion(string BranchCode)
        {
            base.EnforceConstraint = false;
            getUpdateDS("rapi_getBranch", new Object[] { BranchCode });
            if (hasRows)
            {
                selectedRow = 0;
                return table.Rows[0][0].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        public string getUserFullName(string UserID)
        {
            base.EnforceConstraint = false;
            base.upds = null;
            getUpdateDS("csp_getUser", new Object[] { UserID });
            if (hasRows)
            {
                selectedRow = 0;
                return table.Rows[0][0].ToString().Trim();
            }
            else
            {
                return UserID;
            }
        }

        public IEnumerable<object> ScreenNameRefinitiv(string BranchCode, string FirstName, string MiddleName, string LastName, bool IsIndividual, List<string> SecondaryFields, out bool IsDown)
        {
            IsDown = false;
            try
            {
                DataTable dt = new DataTable();
                string groupId = string.Empty;
                string groupIdName = string.Empty;
                string identifier = string.Empty;
                string providerType = string.Empty;
                string entityType = string.Empty;
                string config = string.Empty;
                string caseScreeningState = string.Empty;
                string branchRegion = string.Empty;
                string timeout = string.Empty;
                int retry = 1;

                if (System.Web.HttpContext.Current.Session["RefinitivConfiguration"] == null)
                {
                    dt = new irsInternetPage().getLookup("RefinitivConfiguration");
                    System.Web.HttpContext.Current.Session["RefinitivConfiguration"] = dt;
                }
                else
                {
                    dt = (DataTable)System.Web.HttpContext.Current.Session["RefinitivConfiguration"];
                }

                branchRegion = getBranchRegion(BranchCode);
                if (branchRegion == string.Empty)
                {
                    new csAuditLog().writeAudit("", "REFINITIV ERROR", "ScreenNameRefinitiv", "SETUP NOT FOUND FOR " + BranchCode, "", csAuditLog.Category.Refinitiv);
                    return null;
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    config = dt.AsEnumerable().Where(x => x.Field<string>("FldCode") == branchRegion).FirstOrDefault().Field<string>("FldDescription");
                    if (config != string.Empty)
                    {
                        identifier = config.Split('|').GetValue(0).ToString();
                        groupIdName = config.Split('|').GetValue(1).ToString();
                        groupId = config.Split('|').GetValue(2).ToString();
                    }
                    providerType = dt.AsEnumerable().Where(x => x.Field<string>("FldCode") == "ProviderType").FirstOrDefault().Field<string>("FldDescription");
                    if (IsIndividual)
                    {
                        entityType = dt.AsEnumerable().Where(x => x.Field<string>("FldCode") == "EntityTypeI").FirstOrDefault().Field<string>("FldDescription");
                    }
                    else
                    {
                        entityType = dt.AsEnumerable().Where(x => x.Field<string>("FldCode") == "EntityTypeB").FirstOrDefault().Field<string>("FldDescription");
                    }
                    caseScreeningState = dt.AsEnumerable().Where(x => x.Field<string>("FldCode") == "CaseScreeningState").FirstOrDefault().Field<string>("FldDescription");
                    timeout = dt.AsEnumerable().Where(x => x.Field<string>("FldCode") == "Timeout").FirstOrDefault().Field<string>("FldDescription");
                    retry = Convert.ToInt32(dt.AsEnumerable().Where(x => x.Field<string>("FldCode") == "Retry").FirstOrDefault().Field<string>("FldDescription"));
                }

                if (groupId == string.Empty)
                {
                    APIGroupIDDto groupIdParam = new APIGroupIDDto
                    {
                        identifier = identifier + "_" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                        groupName = groupIdName
                    };

                    var jsonGroupID = JsonConvert.SerializeObject(groupIdParam);
                    object groupIdRes = null;
                    for (int ScreenNameRefinitiv_groupIdRes = 0; ScreenNameRefinitiv_groupIdRes < retry; ScreenNameRefinitiv_groupIdRes++)
                    {
                        groupIdRes = ConnectToRefinitiv(jsonGroupID, ConfigurationManager.AppSettings["RAPI_REFINITIV_API_GROUPID"], timeout);
                        if (groupIdRes != null)
                        {
                            if (groupIdRes != null && (groupIdRes.ToString() != httpRes.ServiceUnavilable.ToString() && groupIdRes.ToString() != httpRes.Timedout.ToString()))
                            {
                                GroupIDResponseDto g = JsonConvert.DeserializeObject<GroupIDResponseDto>(groupIdRes.ToString());
                                if (g.replyStatus.statusCode == 0)
                                {
                                    if (g.responseBody != string.Empty || g.responseBody != null)
                                    {
                                        groupId = g.responseBody;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    if (groupIdRes != null && (groupIdRes.ToString() == httpRes.ServiceUnavilable.ToString() || groupIdRes.ToString() == httpRes.Timedout.ToString()))
                    {
                        new csAuditLog().writeAudit("", "REFINITIV ERROR", "ScreenNameRefinitiv - Group Id", "Refinitiv is down. groupIdRes API response: " + groupIdRes.ToString(), "GroupName=" + groupIdName, csAuditLog.Category.Refinitiv);
                    }
                }

                string gender = string.Empty;
                string dob = string.Empty;
                string cob = string.Empty;
                string pob = string.Empty;
                string nationality = string.Empty;
                string organization = string.Empty;

                if (SecondaryFields != null && SecondaryFields.Count > 0)
                {
                    foreach (var item in SecondaryFields)
                    {
                        if (item.ToString().ToUpper().Contains("GENDER"))
                        {
                            gender = item.ToString().Split('=').GetValue(1).ToString();
                        }
                        if (item.ToString().ToUpper().Contains("DOB"))
                        {
                            dob = item.ToString().Split('=').GetValue(1).ToString();
                        }
                        if (item.ToString().ToUpper().Contains("COB"))
                        {
                            cob = item.ToString().Split('=').GetValue(1).ToString();
                            if (!validateISO3Country(cob))
                            {
                                cob = string.Empty;
                            }
                        }
                        if (item.ToString().ToUpper().Contains("POB"))
                        {
                            pob = item.ToString().Split('=').GetValue(1).ToString();
                            if (!validateISO3Country(pob))
                            {
                                pob = string.Empty;
                            }
                        }
                        if (item.ToString().ToUpper().Contains("NATIONALITY"))
                        {
                            nationality = item.ToString().Split('=').GetValue(1).ToString();
                            if (!validateISO3Country(nationality))
                            {
                                nationality = string.Empty;
                            }
                        }
                        if (item.ToString().ToUpper().Contains("ORGANIZATION"))
                        {
                            organization = item.ToString().Split('=').GetValue(1).ToString();
                            if (!validateISO3Country(organization))
                            {
                                organization = string.Empty;
                            }
                        }
                    }
                }

                APIScreenMyNameDto param = new APIScreenMyNameDto()
                {
                    identifier = identifier + "_" + FirstName + MiddleName + LastName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                    groupId = groupId,
                    entityType = entityType,
                    caseId = identifier + "_" + FirstName + MiddleName + LastName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                    providerTypes = providerType,
                    name = FirstName + " " + MiddleName + " " + LastName,
                    caseScreeningState = caseScreeningState,
                    nameTransposition = true,
                    gender = gender,
                    dateOfBirth = dob,
                    countryOfBirth = cob,
                    placeOfBirth = pob,
                    nationality = nationality,
                    organizationCountry = organization
                };

                var json = JsonConvert.SerializeObject(param);
                object screeningResponseRes = null;
                DataTable dtGetResultsByCaseSystemId = new DataTable();
                for (int ScreenNameRefinitiv_screeningResponseRes = 0; ScreenNameRefinitiv_screeningResponseRes < retry; ScreenNameRefinitiv_screeningResponseRes++)
                {
                    dtGetResultsByCaseSystemId = GetResultsByCaseSystemId(BranchCode, param.caseId, "", true, out IsDown);
                    if (dtGetResultsByCaseSystemId != null && dtGetResultsByCaseSystemId.Rows.Count > 0)
                    {
                        new csAuditLog().writeAudit("", "REFINITIV", "ScreenNameRefinitiv: dtGetResultsByCaseSystemId", "CASE_ID:" + param.caseId, FirstName + "~" + MiddleName + "~" + LastName, csAuditLog.Category.Refinitiv);
                        return dtGetResultsByCaseSystemId.AsEnumerable();
                    }
                    else
                    {
                        screeningResponseRes = ConnectToRefinitiv(json, ConfigurationManager.AppSettings["RAPI_REFINITIV_API_SCREENNAME"], timeout);
                        if (screeningResponseRes != null)
                        {
                            if (screeningResponseRes != null && (screeningResponseRes.ToString() != httpRes.ServiceUnavilable.ToString() && screeningResponseRes.ToString() != httpRes.Timedout.ToString()))
                            {
                                ScreeningResponseDto jsonRes = JsonConvert.DeserializeObject<ScreeningResponseDto>(screeningResponseRes.ToString());
                                if (jsonRes.replyStatus.statusCode == 0)
                                {
                                    if (jsonRes.responseBody != null)
                                    {
                                        var enumerable = new[] { jsonRes };
                                        new csAuditLog().writeAudit("", "REFINITIV", "ScreenNameRefinitiv", jsonRes.replyStatus.message, FirstName + "~" + MiddleName + "~" + LastName, csAuditLog.Category.Refinitiv);
                                        return (IEnumerable<object>)enumerable.ToList();
                                    }
                                }
                            }
                        }
                    }
                }
                if (screeningResponseRes == null)
                {
                    IsDown = true;
                    new csAuditLog().writeAudit("", "REFINITIV ERROR", "ScreenNameRefinitiv - Group Id", "Refinitiv is down. API response: " + screeningResponseRes.ToString(), "Param=" + param.identifier, csAuditLog.Category.Refinitiv);
                    return null;
                }
                if (screeningResponseRes != null && (screeningResponseRes.ToString() == httpRes.ServiceUnavilable.ToString() || screeningResponseRes.ToString() == httpRes.Timedout.ToString()))
                {
                    IsDown = true;
                    new csAuditLog().writeAudit("", "REFINITIV ERROR", "ScreenNameRefinitiv - Group Id", "Refinitiv is down. API response: " + screeningResponseRes.ToString(), "Param=" + param.identifier, csAuditLog.Category.Refinitiv);
                    return null;
                }
            }
            catch (Exception ex)
            {
                IsDown = true;
                new csAuditLog().writeAudit("", "REFINITIV ERROR", "ScreenNameRefinitiv", ex.Message + "|" + ex.StackTrace, FirstName + "~" + MiddleName + "~" + LastName, csAuditLog.Category.Refinitiv);
            }
            return null;
        }

        public DataTable GetResultsByCaseSystemId(string BranchCode, string CaseId, string CaseSystemId, bool skipReference, out bool IsDown)
        {
            IsDown = false;
            DataTable dt = new DataTable();
            DataTable dtReturn = new DataTable();
            DataRow dr;
            string identifier = string.Empty;
            string groupId = string.Empty;
            string groupIdName = string.Empty;
            string config = string.Empty;
            string streamline = string.Empty;
            string listSources = string.Empty;
            string listCategories = string.Empty;
            string dateofbirth = string.Empty;
            string country = string.Empty;
            string providerType = string.Empty;
            string referenceId = string.Empty;
            string remarksSetup = string.Empty;
            string aliasSetup = string.Empty;
            string remarks = string.Empty;
            string alias = string.Empty;
            string resolution = string.Empty;
            string resolutionRemarks = string.Empty;
            string branchRegion = string.Empty;
            string entityType = string.Empty;
            string caseScreeningState = string.Empty;
            string timeout = string.Empty;
            bool noResult = false;
            bool skip = false;
            int returnCount = 0;
            int count = 0;
            int i = 0;
            int retry = 1;

            try
            {
                dtReturn.Columns.Add("CaseID");
                dtReturn.Columns.Add("CaseSystemID");
                dtReturn.Columns.Add("ResultID");
                dtReturn.Columns.Add("ReferenceID");
                dtReturn.Columns.Add("MatchScore");
                dtReturn.Columns.Add("MatchTerm");
                dtReturn.Columns.Add("MatchNameType");
                dtReturn.Columns.Add("SubmittedTerm");
                dtReturn.Columns.Add("PrimaryName");
                dtReturn.Columns.Add("Sources");
                dtReturn.Columns.Add("Category");
                dtReturn.Columns.Add("Categories");
                dtReturn.Columns.Add("Gender");
                dtReturn.Columns.Add("DOB");
                dtReturn.Columns.Add("Country");
                dtReturn.Columns.Add("PEPStatus");
                dtReturn.Columns.Add("ProviderType");
                dtReturn.Columns.Add("Alias");
                dtReturn.Columns.Add("Remarks");
                dtReturn.Columns.Add("Resolution");
                dtReturn.Columns.Add("ResolutionRemarks");
                dtReturn.Columns.Add("CreatedDate");
                dtReturn.Columns.Add("UpdatedDate");

                if (System.Web.HttpContext.Current.Session["RefinitivConfiguration"] == null)
                {
                    dt = new irsInternetPage().getLookup("RefinitivConfiguration");
                    System.Web.HttpContext.Current.Session["RefinitivConfiguration"] = dt;
                }
                else
                {
                    dt = (DataTable)System.Web.HttpContext.Current.Session["RefinitivConfiguration"];
                }

                branchRegion = getBranchRegion(BranchCode);
                if (branchRegion == string.Empty)
                {
                    new csAuditLog().writeAudit("", "REFINITIV ERROR", "GetResultsByCaseSystemId", "SETUP NOT FOUND FOR " + BranchCode, "", csAuditLog.Category.Refinitiv);
                    return null;
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    config = dt.AsEnumerable().Where(x => x.Field<string>("FldCode") == branchRegion).FirstOrDefault().Field<string>("FldDescription");
                    if (config != string.Empty)
                    {
                        identifier = config.Split('|').GetValue(0).ToString();
                        groupIdName = config.Split('|').GetValue(1).ToString();
                        groupId = config.Split('|').GetValue(2).ToString();
                        streamline = config.Split('|').GetValue(3).ToString();
                    }
                    remarksSetup = dt.AsEnumerable().Where(x => x.Field<string>("FldCode") == "Remarks").FirstOrDefault().Field<string>("FldDescription");
                    aliasSetup = dt.AsEnumerable().Where(x => x.Field<string>("FldCode") == "Alias").FirstOrDefault().Field<string>("FldDescription");
                    timeout = dt.AsEnumerable().Where(x => x.Field<string>("FldCode") == "Timeout").FirstOrDefault().Field<string>("FldDescription");
                    retry = Convert.ToInt32(dt.AsEnumerable().Where(x => x.Field<string>("FldCode") == "Retry").FirstOrDefault().Field<string>("FldDescription"));
                }

                if (groupId == string.Empty)
                {
                    APIGroupIDDto groupIdParam = new APIGroupIDDto
                    {
                        identifier = identifier + "_" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                        groupName = groupIdName
                    };

                    var jsonGroupID = JsonConvert.SerializeObject(groupIdParam);
                    object groupIdRes = null;
                    for (int countGetResultsByCaseSystemId_groupIdRes = 0; countGetResultsByCaseSystemId_groupIdRes < retry; countGetResultsByCaseSystemId_groupIdRes++)
                    {
                        groupIdRes = ConnectToRefinitiv(jsonGroupID, ConfigurationManager.AppSettings["RAPI_REFINITIV_API_GROUPID"], timeout);
                        if (groupIdRes != null)
                        {
                            if (groupIdRes != null && (groupIdRes.ToString() != httpRes.ServiceUnavilable.ToString() && groupIdRes.ToString() != httpRes.Timedout.ToString()))
                            {
                                GroupIDResponseDto g = JsonConvert.DeserializeObject<GroupIDResponseDto>(groupIdRes.ToString());
                                if (g.replyStatus.statusCode == 0)
                                {
                                    if (g.responseBody != string.Empty || g.responseBody != null)
                                    {
                                        groupId = g.responseBody;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (groupIdRes != null && (groupIdRes.ToString() == httpRes.ServiceUnavilable.ToString() || groupIdRes.ToString() == httpRes.Timedout.ToString()))
                    {
                        IsDown = true;
                        new csAuditLog().writeAudit("", "REFINITIV ERROR", "GetResultsByCaseSystemId - Group Id", "Refinitiv is down. groupIdRes API response: " + groupIdRes.ToString(), "GroupName=" + groupIdName, csAuditLog.Category.Refinitiv);
                        return null;
                    }
                }

                ProviderResponseDto provider = null;
                if (!skipReference)
                {
                    if (System.Web.HttpContext.Current.Session["RefinitivProvider"] == null)
                    {
                        APIProviderDto providers = new APIProviderDto
                        {
                            identifier = identifier + "_" + DateTime.Now.ToString("yyyyMMddHHmmss")
                        };

                        try
                        {
                            var json = JsonConvert.SerializeObject(providers);
                            object providerRes = null;
                            for (int countGetResultsByCaseSystemId_providerRes = 0; countGetResultsByCaseSystemId_providerRes < retry; countGetResultsByCaseSystemId_providerRes++)
                            {
                                providerRes = ConnectToRefinitiv(json, ConfigurationManager.AppSettings["RAPI_REFINITIV_API_PROVIDER"], timeout);
                                if (providerRes != null)
                                {
                                    if (providerRes != null && (providerRes.ToString() != httpRes.ServiceUnavilable.ToString() && providerRes.ToString() != httpRes.Timedout.ToString()))
                                    {
                                        provider = JsonConvert.DeserializeObject<ProviderResponseDto>(providerRes.ToString());
                                        if (provider.replyStatus.statusCode == 0)
                                        {
                                            if (provider.responseBody != null)
                                            {
                                                System.Web.HttpContext.Current.Session["RefinitivProvider"] = provider;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            if (providerRes != null && (providerRes.ToString() == httpRes.ServiceUnavilable.ToString() || providerRes.ToString() == httpRes.Timedout.ToString()))
                            {
                                new csAuditLog().writeAudit("", "REFINITIV ERROR", "GetResultsByCaseSystemId - Provider", "Refinitiv is down. providerRes API response: " + providerRes.ToString(), "", csAuditLog.Category.Refinitiv);
                            }
                        }
                        catch (Exception ex)
                        {
                            new csAuditLog().writeAudit("", "REFINITIV ERROR", "GetResultsByCaseSystemId - Provider", ex.Message + "|" + ex.StackTrace, "", csAuditLog.Category.Refinitiv);
                        }
                    }
                    else
                    {
                        provider = (ProviderResponseDto)System.Web.HttpContext.Current.Session["RefinitivProvider"];
                    }
                }

                ResolveReferenceResponseDto resolveReference = null;
                object referenceRes = null;
                List<string> lstPositive = new List<string>();

                if (System.Web.HttpContext.Current.Session["RefinitivResolveReference"] == null)
                {
                    APIResolveReferenceDto reference = new APIResolveReferenceDto
                    {
                        identifier = identifier + "_" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                        groupId = groupId
                    };

                    var json = JsonConvert.SerializeObject(reference);
                    referenceRes = null;
                    for (int countGetResultsByCaseSystemId_referenceRes = 0; countGetResultsByCaseSystemId_referenceRes < retry; countGetResultsByCaseSystemId_referenceRes++)
                    {
                        referenceRes = ConnectToRefinitiv(json, ConfigurationManager.AppSettings["RAPI_REFINITIV_API_RESOLVE_REFERENCE"], timeout);
                        if (referenceRes != null)
                        {
                            if (referenceRes != null && (referenceRes.ToString() != httpRes.ServiceUnavilable.ToString() && referenceRes.ToString() != httpRes.Timedout.ToString()))
                            {
                                resolveReference = JsonConvert.DeserializeObject<ResolveReferenceResponseDto>(referenceRes.ToString());
                                if (resolveReference.replyStatus.statusCode == 0)
                                {
                                    if (resolveReference.responseBody != null)
                                    {
                                        System.Web.HttpContext.Current.Session["RefinitivResolveReference"] = referenceRes;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (referenceRes != null && (referenceRes.ToString() == httpRes.ServiceUnavilable.ToString() || referenceRes.ToString() == httpRes.Timedout.ToString()))
                    {
                        new csAuditLog().writeAudit("", "REFINITIV ERROR", "GetResultsByCaseSystemId - Resolve Reference", "Refinitiv is down. referenceRes API response: " + referenceRes.ToString(), "", csAuditLog.Category.Refinitiv);
                    }
                }
                else
                {
                    referenceRes = System.Web.HttpContext.Current.Session["RefinitivResolveReference"];
                    resolveReference = JsonConvert.DeserializeObject<ResolveReferenceResponseDto>(referenceRes.ToString());
                }

                if (resolveReference != null)
                {
                    foreach (var item in resolveReference.responseBody.WATCHLIST.resolutionFields.statuses)
                    {
                        if (item.label.ToUpper() == "POSITIVE")
                        {
                            lstPositive.Add(item.id);
                            break;
                        }
                    }
                    foreach (var item in resolveReference.responseBody.CLIENT_WATCHLIST.resolutionFields.statuses)
                    {
                        if (item.label.ToUpper() == "POSITIVE")
                        {
                            lstPositive.Add(item.id);
                            break;
                        }
                    }
                }

                APIResultDto result = new APIResultDto
                {
                    identifier = identifier + "_" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                    groupId = groupId,
                    caseId = CaseId,
                    caseSystemId = CaseSystemId,
                    screenCase = true
                };

                var jsonResult = JsonConvert.SerializeObject(result);

                for (int countGetResultsByCaseSystemId_jsonResult = 0; countGetResultsByCaseSystemId_jsonResult < retry; countGetResultsByCaseSystemId_jsonResult++)
                {
                    object resultRes = ConnectToRefinitiv(jsonResult, ConfigurationManager.AppSettings["RAPI_REFINITIV_API_RESULTS"], timeout);
                    if (resultRes != null)
                    {
                        if (resultRes != null && (resultRes.ToString() != httpRes.ServiceUnavilable.ToString() && resultRes.ToString() != httpRes.Timedout.ToString()))
                        {
                            ResultResponseDto jsonRes = JsonConvert.DeserializeObject<ResultResponseDto>(resultRes.ToString());
                            if (jsonRes.replyStatus.statusCode == 0)
                            {
                                if (jsonRes != null && jsonRes.responseBody != null && jsonRes.responseBody.Count > 0)
                                {
                                    if (returnCount == 0)
                                    {
                                        returnCount = jsonRes.responseBody.Count;
                                    }
                                    foreach (var names in jsonRes.responseBody)
                                    {
                                        count++;
                                        if (count <= returnCount)
                                        {
                                            referenceId = names.referenceId;
                                            if (!skipReference)
                                            {
                                                if (referenceId != string.Empty && (remarksSetup == "1" || aliasSetup == "1"))
                                                {
                                                    RecordsResponseDto record = null;
                                                    object recordsRes = null;

                                                    APIRecordsDto records = new APIRecordsDto
                                                    {
                                                        identifier = identifier + "_" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                                                        recordId = referenceId
                                                    };

                                                    try
                                                    {
                                                        var json = JsonConvert.SerializeObject(records);
                                                        for (int countGetResultsByCaseSystemId_recordsRes = 0; countGetResultsByCaseSystemId_recordsRes < retry; countGetResultsByCaseSystemId_recordsRes++)
                                                        {
                                                            recordsRes = ConnectToRefinitiv(json, ConfigurationManager.AppSettings["RAPI_REFINITIV_API_RECORD"], timeout);
                                                            if (recordsRes != null)
                                                            {
                                                                if (recordsRes != null && (recordsRes.ToString() != httpRes.ServiceUnavilable.ToString() && recordsRes.ToString() != httpRes.Timedout.ToString()))
                                                                {
                                                                    record = JsonConvert.DeserializeObject<RecordsResponseDto>(recordsRes.ToString());
                                                                    if (record.replyStatus.statusCode == 0)
                                                                    {
                                                                        if (record.responseBody != null)
                                                                        {
                                                                            foreach (var name in record.responseBody.names)
                                                                            {
                                                                                if (name.fullName != null && alias.IndexOf(name.fullName) < 0)
                                                                                {
                                                                                    alias += name.fullName + "; ";
                                                                                }
                                                                            }
                                                                            foreach (var bio in record.responseBody.details)
                                                                            {
                                                                                if (bio.detailType.ToUpper() == "BIOGRAPHY")
                                                                                {
                                                                                    if (bio.text != null && remarks.IndexOf(bio.text) < 0)
                                                                                    {
                                                                                        remarks += bio.text + "; ";
                                                                                    }
                                                                                }
                                                                            }

                                                                            if (alias != string.Empty)
                                                                            {
                                                                                alias = alias.Trim().Substring(0, alias.Trim().Length - 1);
                                                                            }
                                                                            if (remarks != string.Empty)
                                                                            {
                                                                                remarks = remarks.Trim().Substring(0, remarks.Trim().Length - 1);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        new csAuditLog().writeAudit("", "REFINITIV ERROR", "GetResultsByCaseSystemId", ex.Message + "|" + ex.StackTrace, "ReferenceId=" + referenceId, csAuditLog.Category.Refinitiv);
                                                    }
                                                }
                                            }

                                            if (provider != null)
                                            {
                                                for (i = 0; i <= (names.sources.Count - 1); i++)
                                                {
                                                    foreach (var res in provider.responseBody)
                                                    {
                                                        foreach (var sources in res.sources)
                                                        {
                                                            if (names.sources[i].ToUpper() == sources.identifier.ToUpper())
                                                            {
                                                                if (sources.name != null && listSources.IndexOf(sources.name) < 0)
                                                                {
                                                                    listSources += sources.name + ";";
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            if (listSources != string.Empty)
                                            {
                                                listSources = listSources.Trim().Substring(0, listSources.Trim().Length - 1);
                                            }

                                            if (names != null && names.categories != null)
                                            {
                                                foreach (var c in names.categories)
                                                {
                                                    if (c != null && listCategories.ToUpper().IndexOf(c.ToUpper()) < 0)
                                                    {
                                                        if (listCategories.IndexOf(c) < 0)
                                                        {
                                                            listCategories += c + ";";
                                                        }
                                                    }
                                                }
                                                if (listCategories != string.Empty)
                                                {
                                                    listCategories = listCategories.Trim().Substring(0, listCategories.Trim().Length - 1);
                                                }
                                            }

                                            if (names != null && names.events != null)
                                            {
                                                foreach (var dob in names.events)
                                                {
                                                    if (dob.fullDate != null && dateofbirth.IndexOf(dob.fullDate) < 0)
                                                    {
                                                        dateofbirth += dob.fullDate + ";";
                                                    }
                                                }
                                                if (dateofbirth != string.Empty)
                                                {
                                                    dateofbirth = dateofbirth.Trim().Substring(0, dateofbirth.Trim().Length - 1);
                                                }
                                            }

                                            if (names != null && names.countryLinks != null)
                                            {
                                                foreach (var c in names.countryLinks)
                                                {
                                                    if (c.countryText != null && country.IndexOf(c.countryText) < 0)
                                                    {
                                                        country += c.countryText + ";";
                                                    }
                                                }
                                                if (country != string.Empty)
                                                {
                                                    country = country.Trim().Substring(0, country.Trim().Length - 1);
                                                }
                                            }

                                            if (names.resolution != null && !lstPositive.Contains(names.resolution.statusId))
                                            {
                                                skip = true;
                                            }

                                            if (!skip)
                                            {
                                                if (names.resolution != null)
                                                {
                                                    foreach (var item in resolveReference.responseBody.WATCHLIST.resolutionFields.statuses)
                                                    {
                                                        if (item.id == names.resolution.statusId)
                                                        {
                                                            resolution = item.label;
                                                            resolutionRemarks = (names.resolution.resolutionRemark != null ? names.resolution.resolutionRemark : "no data");
                                                        }
                                                    }

                                                    foreach (var item in resolveReference.responseBody.CLIENT_WATCHLIST.resolutionFields.statuses)
                                                    {
                                                        if (item.id == names.resolution.statusId)
                                                        {
                                                            resolution = item.label;
                                                            resolutionRemarks = (names.resolution.resolutionRemark != null ? names.resolution.resolutionRemark : "no data");
                                                        }
                                                    }
                                                }

                                                dr = dtReturn.NewRow();
                                                dr[0] = (CaseId != null && CaseId != string.Empty ? CaseId : "no data");
                                                dr[1] = (CaseSystemId != null && CaseSystemId != string.Empty ? CaseSystemId : "no data");
                                                dr[2] = (names.resultId != null && names.resultId != string.Empty ? names.resultId : "no data");
                                                dr[3] = (names.referenceId != null && names.referenceId != string.Empty ? names.referenceId : "no data");
                                                dr[4] = (names.matchScore != null && names.matchScore != string.Empty ? RemoveDiacritics(names.matchScore) : "no data");
                                                dr[5] = (names.matchedTerm != null && names.matchedTerm != string.Empty ? RemoveDiacritics(names.matchedTerm) : "no data");
                                                dr[6] = (names.matchedNameType != null && names.matchedNameType != string.Empty ? RemoveDiacritics(names.matchedNameType) : "no data");
                                                dr[7] = (names.submittedTerm != null && names.submittedTerm != string.Empty ? RemoveDiacritics(names.submittedTerm) : "no data");
                                                dr[8] = (names.primaryName != null && names.primaryName != string.Empty ? RemoveDiacritics(names.primaryName) : "no data");
                                                dr[9] = (listSources != null && listSources != string.Empty ? RemoveDiacritics(listSources) : "no data");
                                                dr[10] = (names.category != null && names.category != string.Empty ? RemoveDiacritics(names.category) : "no data");
                                                dr[11] = (listCategories != null && listCategories != string.Empty ? RemoveDiacritics(listCategories) : "no data");
                                                dr[12] = (names.gender != null && names.gender != string.Empty ? RemoveDiacritics(names.gender) : "no data");
                                                dr[13] = (dateofbirth != null && dateofbirth != string.Empty ? RemoveDiacritics(dateofbirth) : "no data");
                                                dr[14] = (country != null && country != string.Empty ? RemoveDiacritics(country) : "no data");
                                                dr[15] = (names.pepStatus != null && names.pepStatus != string.Empty ? RemoveDiacritics(names.pepStatus) : "no data");
                                                dr[16] = (names.providerType != null && names.providerType != string.Empty ? RemoveDiacritics(names.providerType) : "no data");
                                                dr[17] = (alias != null && alias != string.Empty ? RemoveDiacritics(alias) : (aliasSetup == "1" ? "no data" : "not enabled"));
                                                dr[18] = (remarks != null && remarks != string.Empty ? RemoveDiacritics(remarks) : (remarksSetup == "1" ? "no data" : "not enabled"));
                                                dr[19] = (resolution != null && resolution != string.Empty ? RemoveDiacritics(resolution) : "no data");
                                                dr[20] = (resolutionRemarks != null && resolutionRemarks != string.Empty ? RemoveDiacritics(resolutionRemarks) : "no data");
                                                dr[21] = (names.entityCreationDate != null && names.entityCreationDate != string.Empty ? StringToDate(names.entityCreationDate) : "no data");
                                                dr[22] = (names.entityModificationDate != null && names.entityModificationDate != string.Empty ? StringToDate(names.entityModificationDate) : "no data");
                                                dtReturn.Rows.Add(dr);
                                            }
                                            skip = false;
                                            listSources = "";
                                            listCategories = "";
                                            dateofbirth = "";
                                            country = "";
                                            alias = "";
                                            remarks = "";

                                            if (dtReturn.Rows.Count <= 0 && returnCount == count)
                                            {
                                                noResult = true;
                                            }
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    noResult = true;
                                }

                                if (noResult)
                                {
                                    dr = dtReturn.NewRow();
                                    dr[0] = CaseId;
                                    dr[1] = CaseSystemId;
                                    dtReturn.Rows.Add(dr);
                                }
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "REFINITIV ERROR", "GetResultsByCaseSystemId", ex.Message + "|" + ex.StackTrace, "CaseSystemId=" + CaseSystemId, csAuditLog.Category.Refinitiv);
            }
            return dtReturn;
        }

        public DataTable DisplayWatchlistBulk(string BranchCode, DataTable dtCIFName, string BulkIdentifier, bool InquireOnly, out bool IsDown)
        {
            IsDown = false;
            DataTable dt = new DataTable();
            DataTable dtReturn = new DataTable();
            DataRow dr;
            string identifier = string.Empty;
            string caseId = string.Empty;
            string caseSystemId = string.Empty;
            string groupId = string.Empty;
            string groupIdName = string.Empty;
            string entityType = string.Empty;
            string providerType = string.Empty;
            string config = string.Empty;
            string streamline = string.Empty;
            string caseScreeningState = string.Empty;
            string branchRegion = string.Empty;
            string timeout = string.Empty;
            int i = 0;
            int retry = 1;

            dtReturn.Columns.Add("CIFID");
            dtReturn.Columns.Add("Hit");
            dtReturn.Columns.Add("Down");

            try
            {
                if (dtCIFName != null && dtCIFName.Rows.Count > 0)
                {
                    if (System.Web.HttpContext.Current.Session["RefinitivConfiguration"] == null)
                    {
                        dt = new irsInternetPage().getLookup("RefinitivConfiguration");
                        System.Web.HttpContext.Current.Session["RefinitivConfiguration"] = dt;
                    }
                    else
                    {
                        dt = (DataTable)System.Web.HttpContext.Current.Session["RefinitivConfiguration"];
                    }

                    branchRegion = getBranchRegion(BranchCode);
                    if (branchRegion == string.Empty)
                    {
                        new csAuditLog().writeAudit("", "REFINITIV ERROR", "DisplayWatchlistBulk", "SETUP NOT FOUND FOR " + BranchCode, "", csAuditLog.Category.Refinitiv);
                        return null;
                    }

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        config = dt.AsEnumerable().Where(x => x.Field<string>("FldCode") == branchRegion).FirstOrDefault().Field<string>("FldDescription");
                        if (config != string.Empty)
                        {
                            identifier = config.Split('|').GetValue(0).ToString();
                            groupIdName = config.Split('|').GetValue(1).ToString();
                            groupId = config.Split('|').GetValue(2).ToString();
                            streamline = config.Split('|').GetValue(3).ToString();
                        }
                        providerType = dt.AsEnumerable().Where(x => x.Field<string>("FldCode") == "ProviderType").FirstOrDefault().Field<string>("FldDescription");
                        entityType = dt.AsEnumerable().Where(x => x.Field<string>("FldCode") == "EntityTypeI").FirstOrDefault().Field<string>("FldDescription");
                        caseScreeningState = dt.AsEnumerable().Where(x => x.Field<string>("FldCode") == "CaseScreeningState").FirstOrDefault().Field<string>("FldDescription");
                        timeout = dt.AsEnumerable().Where(x => x.Field<string>("FldCode") == "Timeout").FirstOrDefault().Field<string>("FldDescription");
                        retry = Convert.ToInt32(dt.AsEnumerable().Where(x => x.Field<string>("FldCode") == "Retry").FirstOrDefault().Field<string>("FldDescription"));

                    }

                    if (groupId == string.Empty)
                    {
                        APIGroupIDDto groupIdParam = new APIGroupIDDto
                        {
                            identifier = identifier + "_" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                            groupName = groupIdName
                        };

                        var jsonGroupID = JsonConvert.SerializeObject(groupIdParam);
                        object groupIdRes = ConnectToRefinitiv(jsonGroupID, ConfigurationManager.AppSettings["RAPI_REFINITIV_API_GROUPID"], timeout);
                        if (groupIdRes != null)
                        {
                            GroupIDResponseDto g = JsonConvert.DeserializeObject<GroupIDResponseDto>(groupIdRes.ToString());
                            if (g.responseBody != string.Empty)
                            {
                                groupId = g.responseBody;
                            }
                        }
                    }

                    APIScreenMyNameBulkDto param = new APIScreenMyNameBulkDto()
                    {
                        identifier = identifier + "_" + BulkIdentifier,
                        groupId = groupId,
                        providerTypes = providerType,
                        nameTransposition = true,
                        caseScreeningState = caseScreeningState
                    };

                    List<Case> lstCase = new List<Case>();
                    for (i = 0; i <= (dtCIFName.Rows.Count - 1); i++)
                    {
                        Case c = new Case()
                        {
                            caseId = identifier + "_" + dtCIFName.Rows[i][0].ToString() + "_" + dtCIFName.Rows[i][1].ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                            entityType = entityType,
                            name = dtCIFName.Rows[i][1].ToString()
                        };
                        lstCase.Add(c);
                    }
                    param.cases = lstCase;

                    var json = JsonConvert.SerializeObject(param);
                    object screeningResponseBulkRes = ConnectToRefinitiv(json, ConfigurationManager.AppSettings["RAPI_REFINITIV_API_SCREENNAME_BULK"], timeout);
                    if (screeningResponseBulkRes != null && (screeningResponseBulkRes.ToString() == httpRes.ServiceUnavilable.ToString() || screeningResponseBulkRes.ToString() == httpRes.Timedout.ToString()))
                    {
                        IsDown = true;
                        return null;
                    }

                    if (screeningResponseBulkRes != null)
                    {
                        ScreeningResponseBulkDto jsonRes = JsonConvert.DeserializeObject<ScreeningResponseBulkDto>(screeningResponseBulkRes.ToString());
                        new csAuditLog().writeAudit("", "REFINITIV", "DisplayWatchlistBulk", jsonRes.replyStatus.message, BulkIdentifier, csAuditLog.Category.Refinitiv);

                        if (jsonRes != null && jsonRes.responseBody != null)
                        {
                            foreach (var result in jsonRes.responseBody)
                            {
                                dt = GetResultsByCaseSystemId(BranchCode, result.caseId, result.caseSystemId, true, out IsDown);
                                if (IsDown)
                                {
                                    dr = dtReturn.NewRow();
                                    dr[0] = result.caseId.Split('_').GetValue(1).ToString();
                                    dr[1] = "1";
                                    dr[2] = "1";
                                    dtReturn.Rows.Add(dr);
                                }
                                else
                                {
                                    if (dt != null && dt.Rows.Count > 0)
                                    {
                                        SaveWatchlist(dt, result.caseId.Split('_').GetValue(1).ToString(), true, dt.Rows.Count);
                                        if (dt.Rows.Count == 1 && (dt.Rows[0][2] == DBNull.Value || dt.Rows[0][2].ToString() == string.Empty))
                                        {
                                            dr = dtReturn.NewRow();
                                            dr[0] = result.caseId.Split('_').GetValue(1).ToString();
                                            dr[1] = "0";
                                            dr[2] = "0";
                                            dtReturn.Rows.Add(dr);
                                        }
                                        else
                                        {
                                            dr = dtReturn.NewRow();
                                            dr[0] = result.caseId.Split('_').GetValue(1).ToString();
                                            dr[1] = "1";
                                            dr[2] = "0";
                                            dtReturn.Rows.Add(dr);
                                        }
                                    }
                                    else
                                    {
                                        dr = dtReturn.NewRow();
                                        dr[0] = result.caseId.Split('_').GetValue(1).ToString();
                                        dr[1] = "1";
                                        dr[2] = "1";
                                        dtReturn.Rows.Add(dr);
                                    }
                                }
                            }
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "REFINITIV ERROR", "DisplayWatchlistBulk", ex.Message + "|" + ex.StackTrace, "", csAuditLog.Category.Refinitiv);
            }
            return dtReturn;
        }

        private string RemoveDiacritics(string text)
        {
            try
            {
                if (string.IsNullOrEmpty(text))
                    return text;

                // Normalize the string to FormD (decomposed form)
                var normalizedString = text.Normalize(NormalizationForm.FormD);
                var stringBuilder = new StringBuilder();

                foreach (var c in normalizedString)
                {
                    // Check if the character is not a diacritic
                    if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    {
                        stringBuilder.Append(c);
                    }
                }

                // Normalize back to FormC (composed form)
                return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "REFINITIV ERROR", "RemoveDiacritics", ex.Message + "|" + ex.StackTrace, text, csAuditLog.Category.Refinitiv);
                return text;
            }
        }

        private string StringToDate(string date)
        {
            try
            {
                return Convert.ToDateTime(date).ToString("yyyy-MM-dd");
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "REFINITIV ERROR", "StringToDate", ex.Message + "|" + ex.StackTrace, date, csAuditLog.Category.Refinitiv);
                return "no data";
            }
        }

        public DataTable DisplayWatchlist(string BranchCode, IEnumerable<ScreeningResponseDto> Watchlist, string CIFID, bool OverrideName, string FName, string MName, string LName, bool IsIndividual, List<string> SecondaryFields, out bool IsDown)
        {
            IsDown = false;
            DataTable dt = new DataTable();
            DataTable dtReturn = new DataTable();
            DataRow dr;
            string identifier = string.Empty;
            string caseId = string.Empty;
            string caseSystemId = string.Empty;
            string groupId = string.Empty;
            string groupIdName = string.Empty;
            string config = string.Empty;
            string streamline = string.Empty;
            string listSources = string.Empty;
            string listCategories = string.Empty;
            string dateofbirth = string.Empty;
            string country = string.Empty;
            string providerType = string.Empty;
            string referenceId = string.Empty;
            string remarksSetup = string.Empty;
            string aliasSetup = string.Empty;
            string remarks = string.Empty;
            string alias = string.Empty;
            string resolution = string.Empty;
            string resolutionRemarks = string.Empty;
            string branchRegion = string.Empty;
            string timeout = string.Empty;
            int dtCaseIdHitCount = 0;
            int returnCount = 0;
            bool skip = false;
            int i = 0;
            bool isDown = false;
            int retry = 1;

            dtReturn.Columns.Add("CaseID");
            dtReturn.Columns.Add("CaseSystemID");
            dtReturn.Columns.Add("ResultID");
            dtReturn.Columns.Add("ReferenceID");
            dtReturn.Columns.Add("MatchScore");
            dtReturn.Columns.Add("MatchTerm");
            dtReturn.Columns.Add("MatchNameType");
            dtReturn.Columns.Add("SubmittedTerm");
            dtReturn.Columns.Add("PrimaryName");
            dtReturn.Columns.Add("Sources");
            dtReturn.Columns.Add("Category");
            dtReturn.Columns.Add("Categories");
            dtReturn.Columns.Add("Gender");
            dtReturn.Columns.Add("DOB");
            dtReturn.Columns.Add("Country");
            dtReturn.Columns.Add("PEPStatus");
            dtReturn.Columns.Add("ProviderType");
            dtReturn.Columns.Add("Alias");
            dtReturn.Columns.Add("Remarks");
            dtReturn.Columns.Add("Resolution");
            dtReturn.Columns.Add("ResolutionRemarks");
            dtReturn.Columns.Add("CreatedDate");
            dtReturn.Columns.Add("UpdatedDate");

            try
            {
                bool screen = false;
                string lastName = string.Empty;
                string firstName = string.Empty;
                string middleName = string.Empty;

                if (Watchlist == null && CIFID != string.Empty)
                {
                    DataTable dtCaseId = this.getRefinitivByCaseId(CIFID);
                    if (dtCaseId != null && dtCaseId.Rows.Count > 0)
                    {
                        caseId = dtCaseId.Rows[0]["CaseID"].ToString();
                        caseSystemId = dtCaseId.Rows[0]["CaseSystemID"].ToString();
                        dtCaseIdHitCount = Convert.ToInt32(dtCaseId.Rows[0]["HitCount"]);
                    }

                    List<string> secondary = new List<string>();
                    DataTable dtCIF = new DataTable();
                    if (CIFID != null && CIFID != string.Empty)
                    {
                        dtCIF = getCIFInfo(CIFID);
                        if (dtCIF != null && dtCIF.Rows.Count > 0)
                        {
                            lastName = dtCIF.Rows[0]["LName"].ToString();
                            firstName = dtCIF.Rows[0]["FName"].ToString();
                            middleName = dtCIF.Rows[0]["MName"].ToString();
                            if (dtCIF.Rows[0]["Gender"] != DBNull.Value && dtCIF.Rows[0]["Gender"].ToString() != string.Empty)
                            {
                                secondary.Add("GENDER=" + dtCIF.Rows[0]["Gender"].ToString().ToUpper());
                            }
                            if (dtCIF.Rows[0]["DOB"] != DBNull.Value && dtCIF.Rows[0]["DOB"].ToString() != string.Empty)
                            {
                                secondary.Add("DOB=" + dtCIF.Rows[0]["DOB"].ToString().ToUpper());
                            }
                            if (dtCIF.Rows[0]["COB"] != DBNull.Value && dtCIF.Rows[0]["COB"].ToString() != string.Empty)
                            {
                                secondary.Add("COB=" + dtCIF.Rows[0]["COB"].ToString().ToUpper());
                            }
                            if (dtCIF.Rows[0]["POB"] != DBNull.Value && dtCIF.Rows[0]["POB"].ToString() != string.Empty)
                            {
                                secondary.Add("POB=" + dtCIF.Rows[0]["POB"].ToString().ToUpper());
                            }
                            if (dtCIF.Rows[0]["NATIONALITY"] != DBNull.Value && dtCIF.Rows[0]["NATIONALITY"].ToString() != string.Empty)
                            {
                                secondary.Add("NATIONALITY=" + dtCIF.Rows[0]["NATIONALITY"].ToString().ToUpper());
                            }
                            if (dtCIF.Rows[0]["CIFType"] != DBNull.Value && dtCIF.Rows[0]["CIFType"].ToString() != string.Empty)
                            {
                                if (dtCIF.Rows[0]["CIFType"].ToString().Trim() == "1")
                                {
                                    IsIndividual = true;
                                }
                                else
                                {
                                    IsIndividual = false;
                                }
                            }
                        }
                    }

                    if (caseSystemId == string.Empty)
                    {
                        if (OverrideName)
                        {
                            screen = true;
                            Watchlist = (IEnumerable<ScreeningResponseDto>)this.ScreenNameRefinitiv(BranchCode, FName, MName, LName, IsIndividual, SecondaryFields, out isDown);
                        }
                        else
                        {
                            screen = true;
                            Watchlist = (IEnumerable<ScreeningResponseDto>)this.ScreenNameRefinitiv(BranchCode, firstName, middleName, lastName, IsIndividual, secondary, out isDown);
                        }
                    }
                    else
                    {
                        if (OverrideName)
                        {
                            screen = true;
                            Watchlist = (IEnumerable<ScreeningResponseDto>)this.ScreenNameRefinitiv(BranchCode, FName, MName, LName, IsIndividual, SecondaryFields, out isDown);
                        }
                    }

                    if (isDown || (Watchlist != null && (Watchlist.ToString() == httpRes.ServiceUnavilable.ToString() || Watchlist.ToString() == httpRes.Timedout.ToString())))
                    {
                        IsDown = true;
                        return null;
                    }
                }
                else
                {
                    screen = true;
                }

                if (System.Web.HttpContext.Current.Session["RefinitivConfiguration"] == null)
                {
                    dt = new irsInternetPage().getLookup("RefinitivConfiguration");
                    System.Web.HttpContext.Current.Session["RefinitivConfiguration"] = dt;
                }
                else
                {
                    dt = (DataTable)System.Web.HttpContext.Current.Session["RefinitivConfiguration"];
                }

                branchRegion = getBranchRegion(BranchCode);
                if (branchRegion == string.Empty)
                {
                    new csAuditLog().writeAudit("", "REFINITIV ERROR", "DisplayWatchlist", "SETUP NOT FOUND FOR " + BranchCode, "", csAuditLog.Category.Refinitiv);
                    return null;
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    config = dt.AsEnumerable().Where(x => x.Field<string>("FldCode") == branchRegion).FirstOrDefault().Field<string>("FldDescription");
                    if (config != string.Empty)
                    {
                        identifier = config.Split('|').GetValue(0).ToString();
                        groupIdName = config.Split('|').GetValue(1).ToString();
                        groupId = config.Split('|').GetValue(2).ToString();
                        streamline = config.Split('|').GetValue(3).ToString();
                    }
                    remarksSetup = dt.AsEnumerable().Where(x => x.Field<string>("FldCode") == "Remarks").FirstOrDefault().Field<string>("FldDescription");
                    aliasSetup = dt.AsEnumerable().Where(x => x.Field<string>("FldCode") == "Alias").FirstOrDefault().Field<string>("FldDescription");
                    timeout = dt.AsEnumerable().Where(x => x.Field<string>("FldCode") == "Timeout").FirstOrDefault().Field<string>("FldDescription");
                    retry = Convert.ToInt32(dt.AsEnumerable().Where(x => x.Field<string>("FldCode") == "Retry").FirstOrDefault().Field<string>("FldDescription"));

                }

                if (groupId == string.Empty)
                {
                    APIGroupIDDto groupIdParam = new APIGroupIDDto
                    {
                        identifier = identifier + "_" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                        groupName = groupIdName
                    };

                    var jsonGroupID = JsonConvert.SerializeObject(groupIdParam);
                    object groupIdRes = null;
                    for (int countDisplayWatchlist_groupIdRes = 0; countDisplayWatchlist_groupIdRes < retry; countDisplayWatchlist_groupIdRes++)
                    {
                        groupIdRes = ConnectToRefinitiv(jsonGroupID, ConfigurationManager.AppSettings["RAPI_REFINITIV_API_GROUPID"], timeout);
                        if (groupIdRes != null)
                        {
                            if (groupIdRes != null && (groupIdRes.ToString() != httpRes.ServiceUnavilable.ToString() && groupIdRes.ToString() != httpRes.Timedout.ToString()))
                            {
                                GroupIDResponseDto g = JsonConvert.DeserializeObject<GroupIDResponseDto>(groupIdRes.ToString());
                                if (g.replyStatus.statusCode == 0)
                                {
                                    if (g.responseBody != null)
                                    {
                                        groupId = g.responseBody;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

                ProviderResponseDto provider = null;
                if (System.Web.HttpContext.Current.Session["RefinitivProvider"] == null)
                {
                    APIProviderDto providers = new APIProviderDto
                    {
                        identifier = identifier + "_" + DateTime.Now.ToString("yyyyMMddHHmmss")
                    };

                    try
                    {
                        var json = JsonConvert.SerializeObject(providers);
                        object providerRes = null;
                        for (int countDisplayWatchlist_providerRes = 0; countDisplayWatchlist_providerRes < retry; countDisplayWatchlist_providerRes++)
                        {
                            providerRes = ConnectToRefinitiv(json, ConfigurationManager.AppSettings["RAPI_REFINITIV_API_PROVIDER"], timeout);
                            if (providerRes != null)
                            {
                                if (providerRes != null && (providerRes.ToString() != httpRes.ServiceUnavilable.ToString() && providerRes.ToString() != httpRes.Timedout.ToString()))
                                {
                                    provider = JsonConvert.DeserializeObject<ProviderResponseDto>(providerRes.ToString());
                                    if (provider.replyStatus.statusCode == 0)
                                    {
                                        if (provider.responseBody != null)
                                        {
                                            System.Web.HttpContext.Current.Session["RefinitivProvider"] = provider;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        new csAuditLog().writeAudit("", "REFINITIV ERROR", "DisplayWatchlist - Provider", ex.Message + "|" + ex.StackTrace, "", csAuditLog.Category.Refinitiv);
                    }
                }
                else
                {
                    provider = (ProviderResponseDto)System.Web.HttpContext.Current.Session["RefinitivProvider"];
                }

                ResolveReferenceResponseDto resolveReference = null;
                object referenceRes = null;
                List<string> lstPositive = new List<string>();

                if (System.Web.HttpContext.Current.Session["RefinitivResolveReference"] == null)
                {
                    APIResolveReferenceDto reference = new APIResolveReferenceDto
                    {
                        identifier = identifier + "_" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                        groupId = groupId
                    };

                    var json = JsonConvert.SerializeObject(reference);
                    referenceRes = null;
                    for (int countDisplayWatchlist_referenceRes = 0; countDisplayWatchlist_referenceRes < retry; countDisplayWatchlist_referenceRes++)
                    {
                        referenceRes = ConnectToRefinitiv(json, ConfigurationManager.AppSettings["RAPI_REFINITIV_API_RESOLVE_REFERENCE"], timeout);
                        if (referenceRes != null)
                        {
                            if (referenceRes != null && (referenceRes.ToString() != httpRes.ServiceUnavilable.ToString() && referenceRes.ToString() != httpRes.Timedout.ToString()))
                            {
                                resolveReference = JsonConvert.DeserializeObject<ResolveReferenceResponseDto>(referenceRes.ToString());
                                if (resolveReference.replyStatus.statusCode == 0)
                                {
                                    if (resolveReference.responseBody != null)
                                    {
                                        System.Web.HttpContext.Current.Session["RefinitivResolveReference"] = referenceRes;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    referenceRes = System.Web.HttpContext.Current.Session["RefinitivResolveReference"];
                    try
                    {
                        resolveReference = JsonConvert.DeserializeObject<ResolveReferenceResponseDto>(referenceRes.ToString());
                    }
                    catch (Exception ex)
                    {
                        IsDown = true;
                        new csAuditLog().writeAudit("", "REFINITIV ERROR", "DisplayWatchlist - resolveReference - DeserializeObject", "Refinitiv is down." + "CaseSystemID=" + caseSystemId + "Message=" + ex.Message, "StackTrace=" + ex.StackTrace, csAuditLog.Category.Refinitiv);
                    }
                }

                if (resolveReference != null)
                {
                    foreach (var item in resolveReference.responseBody.WATCHLIST.resolutionFields.statuses)
                    {
                        if (item.label.ToUpper() == "POSITIVE")
                        {
                            lstPositive.Add(item.id);
                            break;
                        }
                    }
                    foreach (var item in resolveReference.responseBody.CLIENT_WATCHLIST.resolutionFields.statuses)
                    {
                        if (item.label.ToUpper() == "POSITIVE")
                        {
                            lstPositive.Add(item.id);
                            break;
                        }
                    }
                }

                int count = 0;
                bool noResult = false;

                if (!screen)
                {
                    APIResultDto result = new APIResultDto
                    {
                        identifier = identifier + "_" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                        groupId = groupId,
                        caseId = caseId,
                        caseSystemId = caseSystemId,
                        screenCase = true
                    };

                    var jsonResult = JsonConvert.SerializeObject(result);
                    object resultRes = null; ;
                    for (int countDisplayWatchlist_resultRes = 0; countDisplayWatchlist_resultRes < retry; countDisplayWatchlist_resultRes++)
                    {
                        resultRes = ConnectToRefinitiv(jsonResult, ConfigurationManager.AppSettings["RAPI_REFINITIV_API_RESULTS"], timeout);
                        if (resultRes != null)
                        {
                            ResultResponseDto jsonRes = null;
                            if (resultRes != null && (resultRes.ToString() != httpRes.ServiceUnavilable.ToString() && resultRes.ToString() != httpRes.Timedout.ToString()))
                            {
                                jsonRes = JsonConvert.DeserializeObject<ResultResponseDto>(resultRes.ToString());
                                if (jsonRes.replyStatus.statusCode == 0)
                                {
                                    if (jsonRes != null && jsonRes.responseBody != null && jsonRes.responseBody.Count > 0)
                                    {
                                        if (returnCount == 0)
                                        {
                                            returnCount = jsonRes.responseBody.Count;
                                        }
                                        foreach (var names in jsonRes.responseBody)
                                        {
                                            count++;
                                            if (count <= returnCount)
                                            {
                                                referenceId = names.referenceId;
                                                if (referenceId != string.Empty && (remarksSetup == "1" || aliasSetup == "1"))
                                                {
                                                    RecordsResponseDto record = null;
                                                    object recordsRes = null;

                                                    APIRecordsDto records = new APIRecordsDto
                                                    {
                                                        identifier = identifier + "_" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                                                        recordId = referenceId
                                                    };

                                                    try
                                                    {
                                                        var json = JsonConvert.SerializeObject(records);
                                                        recordsRes = null;
                                                        for (int countDisplayWatchlist_recordsRess = 0; countDisplayWatchlist_recordsRess < retry; countDisplayWatchlist_recordsRess++)
                                                        {
                                                            recordsRes = ConnectToRefinitiv(json, ConfigurationManager.AppSettings["RAPI_REFINITIV_API_RECORD"], timeout);
                                                            if (recordsRes != null)
                                                            {
                                                                if (recordsRes != null && (recordsRes.ToString() != httpRes.ServiceUnavilable.ToString() && recordsRes.ToString() != httpRes.Timedout.ToString()))
                                                                {
                                                                    record = JsonConvert.DeserializeObject<RecordsResponseDto>(recordsRes.ToString());
                                                                    if (record.replyStatus.statusCode == 0)
                                                                    {
                                                                        if (record != null)
                                                                        {
                                                                            if (record.responseBody != null)
                                                                            {
                                                                                foreach (var name in record.responseBody.names)
                                                                                {
                                                                                    if (name.fullName != null && alias.IndexOf(name.fullName) < 0)
                                                                                    {
                                                                                        alias += name.fullName + "; ";
                                                                                    }
                                                                                }
                                                                                foreach (var bio in record.responseBody.details)
                                                                                {
                                                                                    if (bio.detailType.ToUpper() == "BIOGRAPHY")
                                                                                    {
                                                                                        if (bio.text != null && remarks.IndexOf(bio.text) < 0)
                                                                                        {
                                                                                            remarks += bio.text + "; ";
                                                                                        }
                                                                                    }
                                                                                }

                                                                                if (alias != string.Empty)
                                                                                {
                                                                                    alias = alias.Trim().Substring(0, alias.Trim().Length - 1);
                                                                                }
                                                                                if (remarks != string.Empty)
                                                                                {
                                                                                    remarks = remarks.Trim().Substring(0, remarks.Trim().Length - 1);
                                                                                }
                                                                            }
                                                                        }
                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        new csAuditLog().writeAudit("", "REFINITIV ERROR", "DisplayWatchlist - Records (!Screen)", ex.Message + "|" + ex.StackTrace, referenceId, csAuditLog.Category.Refinitiv);
                                                    }
                                                }

                                                if (provider != null)
                                                {
                                                    for (i = 0; i <= (names.sources.Count - 1); i++)
                                                    {
                                                        foreach (var res in provider.responseBody)
                                                        {
                                                            foreach (var sources in res.sources)
                                                            {
                                                                if (names.sources[i].ToUpper() == sources.identifier.ToUpper())
                                                                {
                                                                    if (sources.name != null && listSources.IndexOf(sources.name) < 0)
                                                                    {
                                                                        listSources += sources.name + ";";
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                if (listSources != string.Empty)
                                                {
                                                    listSources = listSources.Trim().Substring(0, listSources.Trim().Length - 1);
                                                }

                                                if (names != null && names.categories != null)
                                                {
                                                    foreach (var c in names.categories)
                                                    {
                                                        if (c != null && listCategories.ToUpper().IndexOf(c.ToUpper()) < 0)
                                                        {
                                                            if (listCategories.IndexOf(c) < 0)
                                                            {
                                                                listCategories += c + ";";
                                                            }
                                                        }
                                                    }
                                                    if (listCategories != string.Empty)
                                                    {
                                                        listCategories = listCategories.Trim().Substring(0, listCategories.Trim().Length - 1);
                                                    }
                                                }

                                                if (names != null && names.events != null)
                                                {
                                                    foreach (var dob in names.events)
                                                    {
                                                        if (dob.fullDate != null && dateofbirth.IndexOf(dob.fullDate) < 0)
                                                        {
                                                            dateofbirth += dob.fullDate + ";";
                                                        }
                                                    }
                                                    if (dateofbirth != string.Empty)
                                                    {
                                                        dateofbirth = dateofbirth.Trim().Substring(0, dateofbirth.Trim().Length - 1);
                                                    }
                                                }

                                                if (names != null && names.countryLinks != null)
                                                {
                                                    foreach (var c in names.countryLinks)
                                                    {
                                                        if (c.countryText != null && country.IndexOf(c.countryText) < 0)
                                                        {
                                                            country += c.countryText + ";";
                                                        }
                                                    }
                                                    if (country != string.Empty)
                                                    {
                                                        country = country.Trim().Substring(0, country.Trim().Length - 1);
                                                    }
                                                }

                                                if (names.resolution != null && !lstPositive.Contains(names.resolution.statusId) && streamline == "1")
                                                {
                                                    skip = true;
                                                }

                                                if (!skip)
                                                {
                                                    if (names.resolution != null)
                                                    {
                                                        foreach (var item in resolveReference.responseBody.WATCHLIST.resolutionFields.statuses)
                                                        {
                                                            if (item.id == names.resolution.statusId)
                                                            {
                                                                resolution = item.label;
                                                                resolutionRemarks = (names.resolution.resolutionRemark != null ? names.resolution.resolutionRemark : "no data");
                                                            }
                                                        }

                                                        foreach (var item in resolveReference.responseBody.CLIENT_WATCHLIST.resolutionFields.statuses)
                                                        {
                                                            if (item.id == names.resolution.statusId)
                                                            {
                                                                resolution = item.label;
                                                                resolutionRemarks = (names.resolution.resolutionRemark != null ? names.resolution.resolutionRemark : "no data");
                                                            }
                                                        }
                                                    }

                                                    dr = dtReturn.NewRow();
                                                    dr[0] = (caseId != null && caseId != string.Empty ? caseId : "no data");
                                                    dr[1] = (caseSystemId != null && caseSystemId != string.Empty ? caseSystemId : "no data");
                                                    dr[2] = (names.resultId != null && names.resultId != string.Empty ? names.resultId : "no data");
                                                    dr[3] = (names.referenceId != null && names.referenceId != string.Empty ? names.referenceId : "no data");
                                                    dr[4] = (names.matchScore != null && names.matchScore != string.Empty ? RemoveDiacritics(names.matchScore) : "no data");
                                                    dr[5] = (names.matchedTerm != null && names.matchedTerm != string.Empty ? RemoveDiacritics(names.matchedTerm) : "no data");
                                                    dr[6] = (names.matchedNameType != null && names.matchedNameType != string.Empty ? RemoveDiacritics(names.matchedNameType) : "no data");
                                                    dr[7] = (names.submittedTerm != null && names.submittedTerm != string.Empty ? RemoveDiacritics(names.submittedTerm) : "no data");
                                                    dr[8] = (names.primaryName != null && names.primaryName != string.Empty ? RemoveDiacritics(names.primaryName) : "no data");
                                                    dr[9] = (listSources != null && listSources != string.Empty ? RemoveDiacritics(listSources) : "no data");
                                                    dr[10] = (names.category != null && names.category != string.Empty ? RemoveDiacritics(names.category) : "no data");
                                                    dr[11] = (listCategories != null && listCategories != string.Empty ? RemoveDiacritics(listCategories) : "no data");
                                                    dr[12] = (names.gender != null && names.gender != string.Empty ? RemoveDiacritics(names.gender) : "no data");
                                                    dr[13] = (dateofbirth != null && dateofbirth != string.Empty ? RemoveDiacritics(dateofbirth) : "no data");
                                                    dr[14] = (country != null && country != string.Empty ? RemoveDiacritics(country) : "no data");
                                                    dr[15] = (names.pepStatus != null && names.pepStatus != string.Empty ? RemoveDiacritics(names.pepStatus) : "no data");
                                                    dr[16] = (names.providerType != null && names.providerType != string.Empty ? RemoveDiacritics(names.providerType) : "no data");
                                                    dr[17] = (alias != null && alias != string.Empty ? RemoveDiacritics(alias) : (aliasSetup == "1" ? "no data" : "not enabled"));
                                                    dr[18] = (remarks != null && remarks != string.Empty ? RemoveDiacritics(remarks) : (remarksSetup == "1" ? "no data" : "not enabled"));
                                                    dr[19] = (resolution != null && resolution != string.Empty ? RemoveDiacritics(resolution) : "no data");
                                                    dr[20] = (resolutionRemarks != null && resolutionRemarks != string.Empty ? RemoveDiacritics(resolutionRemarks) : "no data");
                                                    dr[21] = (names.entityCreationDate != null && names.entityCreationDate != string.Empty ? StringToDate(names.entityCreationDate) : "no data");
                                                    dr[22] = (names.entityModificationDate != null && names.entityModificationDate != string.Empty ? StringToDate(names.entityModificationDate) : "no data");
                                                    dtReturn.Rows.Add(dr);
                                                }
                                                skip = false;
                                                listSources = "";
                                                listCategories = "";
                                                dateofbirth = "";
                                                country = "";
                                                alias = "";
                                                remarks = "";

                                                if (dtReturn.Rows.Count <= 0 && returnCount == count)
                                                {
                                                    noResult = true;
                                                }
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    noResult = true;
                                }
                                if (noResult)
                                {
                                    dr = dtReturn.NewRow();
                                    dr[0] = caseId;
                                    dr[1] = caseSystemId;
                                    dtReturn.Rows.Add(dr);
                                }
                                else
                                {
                                    if (dtCaseIdHitCount <= 0)
                                    {
                                        UpdateWatchlist_CaseIDExist(caseId, caseSystemId, dtReturn, 0);
                                    }
                                }
                                break;
                            }
                        }
                    }
                    if (resultRes == null)
                    {
                        IsDown = true;
                        new csAuditLog().writeAudit("", "REFINITIV ERROR", "DisplayWatchlist - resultRes ()", "", referenceId, csAuditLog.Category.Refinitiv);
                        return null;
                    }
                    if (resultRes != null && (resultRes.ToString() == httpRes.ServiceUnavilable.ToString() || resultRes.ToString() == httpRes.Timedout.ToString()))
                    {
                        IsDown = true;
                        new csAuditLog().writeAudit("", "REFINITIV ERROR", "DisplayWatchlist - resultRes ()", "", referenceId, csAuditLog.Category.Refinitiv);
                        return null;
                    }
                }
                else
                {
                    foreach (var hit in Watchlist)
                    {
                        if (hit != null && hit.responseBody != null && hit.responseBody.results.Count > 0)
                        {
                            if (returnCount == 0)
                            {
                                returnCount = hit.responseBody.results.Count;
                            }
                            foreach (var names in hit.responseBody.results)
                            {
                                count++;
                                if (count <= returnCount)
                                {
                                    if (provider != null && provider.responseBody != null)
                                    {
                                        for (i = 0; i <= (names.sources.Count - 1); i++)
                                        {
                                            foreach (var res in provider.responseBody)
                                            {
                                                foreach (var sources in res.sources)
                                                {
                                                    if (names.sources[i].ToUpper() == sources.identifier.ToUpper())
                                                    {
                                                        if (sources.name != null && listSources.IndexOf(sources.name) < 0)
                                                        {
                                                            listSources += sources.name + ";";
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (listSources != string.Empty)
                                    {
                                        listSources = listSources.Trim().Substring(0, listSources.Trim().Length - 1);
                                    }

                                    if (names != null && names.categories != null)
                                    {
                                        foreach (var c in names.categories)
                                        {
                                            if (c != null && listCategories.ToUpper().IndexOf(c.ToUpper()) < 0)
                                            {
                                                if (listCategories.IndexOf(c) < 0)
                                                {
                                                    listCategories += c + ";";
                                                }
                                            }
                                        }
                                        if (listCategories != string.Empty)
                                        {
                                            listCategories = listCategories.Trim().Substring(0, listCategories.Trim().Length - 1);
                                        }
                                    }

                                    if (names != null && names.events != null)
                                    {
                                        foreach (var dob in names.events)
                                        {
                                            if (dob.fullDate != null && dateofbirth.IndexOf(dob.fullDate) < 0)
                                            {
                                                dateofbirth += dob.fullDate + ";";
                                            }
                                        }
                                        if (dateofbirth != string.Empty)
                                        {
                                            dateofbirth = dateofbirth.Trim().Substring(0, dateofbirth.Trim().Length - 1);
                                        }
                                    }

                                    if (names != null && names.countryLinks != null)
                                    {
                                        foreach (var c in names.countryLinks)
                                        {
                                            if (c.countryText != null && country.IndexOf(c.countryText) < 0)
                                            {
                                                country += c.countryText + ";";
                                            }
                                        }
                                        if (country != string.Empty)
                                        {
                                            country = country.Trim().Substring(0, country.Trim().Length - 1);
                                        }
                                    }

                                    referenceId = names.referenceId;
                                    if (referenceId != string.Empty && (remarksSetup == "1" || aliasSetup == "1"))
                                    {
                                        RecordsResponseDto record = null;
                                        object recordsRes = null;

                                        APIRecordsDto records = new APIRecordsDto
                                        {
                                            identifier = identifier + "_" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                                            recordId = referenceId
                                        };

                                        try
                                        {
                                            var json = JsonConvert.SerializeObject(records);
                                            recordsRes = null;
                                            for (int countDisplayWatchlist_recordsRes = 0; countDisplayWatchlist_recordsRes < retry; countDisplayWatchlist_recordsRes++)
                                            {
                                                recordsRes = ConnectToRefinitiv(json, ConfigurationManager.AppSettings["RAPI_REFINITIV_API_RECORD"], timeout);
                                                if (recordsRes != null)
                                                {
                                                    if (recordsRes != null && (recordsRes.ToString() != httpRes.ServiceUnavilable.ToString() && recordsRes.ToString() != httpRes.Timedout.ToString()))
                                                    {
                                                        record = JsonConvert.DeserializeObject<RecordsResponseDto>(recordsRes.ToString());
                                                        if (record.replyStatus.statusCode == 0)
                                                        {
                                                            if (record != null)
                                                            {
                                                                if (record.responseBody != null)
                                                                {
                                                                    foreach (var name in record.responseBody.names)
                                                                    {
                                                                        if (name.fullName != null && alias.IndexOf(name.fullName) < 0)
                                                                        {
                                                                            alias += name.fullName + "; ";
                                                                        }
                                                                    }
                                                                    foreach (var bio in record.responseBody.details)
                                                                    {
                                                                        if (bio.detailType.ToUpper() == "BIOGRAPHY")
                                                                        {
                                                                            if (bio.text != null && remarks.IndexOf(bio.text) < 0)
                                                                            {
                                                                                remarks += bio.text + "; ";
                                                                            }
                                                                        }
                                                                    }

                                                                    if (alias != string.Empty)
                                                                    {
                                                                        alias = alias.Trim().Substring(0, alias.Trim().Length - 1);
                                                                    }
                                                                    if (remarks != string.Empty)
                                                                    {
                                                                        remarks = remarks.Trim().Substring(0, remarks.Trim().Length - 1);
                                                                    }
                                                                }
                                                            }
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            if (recordsRes == null)
                                            {
                                                IsDown = true;
                                                new csAuditLog().writeAudit("", "REFINITIV ERROR", "DisplayWatchlist - recordsRes ()", "", referenceId, csAuditLog.Category.Refinitiv);
                                                return null;
                                            }
                                            if (recordsRes != null && (recordsRes.ToString() == httpRes.ServiceUnavilable.ToString() || recordsRes.ToString() == httpRes.Timedout.ToString()))
                                            {
                                                IsDown = true;
                                                new csAuditLog().writeAudit("", "REFINITIV ERROR", "DisplayWatchlist - recordsRes ()", "", referenceId, csAuditLog.Category.Refinitiv);
                                                return null;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            new csAuditLog().writeAudit("", "REFINITIV ERROR", "DisplayWatchlist - Records (Screen)", ex.Message + "|" + ex.StackTrace, referenceId, csAuditLog.Category.Refinitiv);
                                        }
                                    }

                                    if (names.resolution != null && !lstPositive.Contains(names.resolution.statusId))
                                    {
                                        skip = true;
                                    }

                                    if (!skip)
                                    {
                                        if (names.resolution != null)
                                        {
                                            foreach (var item in resolveReference.responseBody.WATCHLIST.resolutionFields.statuses)
                                            {
                                                if (item.id == names.resolution.statusId)
                                                {
                                                    resolution = item.label;
                                                    resolutionRemarks = (names.resolution.resolutionRemark != null ? names.resolution.resolutionRemark : "no data");
                                                }
                                            }

                                            foreach (var item in resolveReference.responseBody.CLIENT_WATCHLIST.resolutionFields.statuses)
                                            {
                                                if (item.id == names.resolution.statusId)
                                                {
                                                    resolution = item.label;
                                                    resolutionRemarks = (names.resolution.resolutionRemark != null ? names.resolution.resolutionRemark : "no data");
                                                }
                                            }
                                        }

                                        dr = dtReturn.NewRow();
                                        dr[0] = (hit.responseBody.caseId != null && hit.responseBody.caseId != string.Empty ? hit.responseBody.caseId : "no data");
                                        dr[1] = (hit.responseBody.caseSystemId != null && hit.responseBody.caseSystemId != string.Empty ? hit.responseBody.caseSystemId : "no data");
                                        dr[2] = (names.resultId != null && names.resultId != string.Empty ? names.resultId : "no data");
                                        dr[3] = (names.referenceId != null && names.referenceId != string.Empty ? names.referenceId : "no data");
                                        dr[4] = (names.matchScore != null && names.matchScore != string.Empty ? RemoveDiacritics(names.matchScore) : "no data");
                                        dr[5] = (names.matchedTerm != null && names.matchedTerm != string.Empty ? RemoveDiacritics(names.matchedTerm) : "no data");
                                        dr[6] = (names.matchedNameType != null && names.matchedNameType != string.Empty ? RemoveDiacritics(names.matchedNameType) : "no data");
                                        dr[7] = (names.submittedTerm != null && names.submittedTerm != string.Empty ? RemoveDiacritics(names.submittedTerm) : "no data");
                                        dr[8] = (names.primaryName != null && names.primaryName != string.Empty ? RemoveDiacritics(names.primaryName) : "no data");
                                        dr[9] = (listSources != null && listSources != string.Empty ? RemoveDiacritics(listSources) : "no data");
                                        dr[10] = (names.category != null && names.category != string.Empty ? RemoveDiacritics(names.category) : "no data");
                                        dr[11] = (listCategories != null && listCategories != string.Empty ? RemoveDiacritics(listCategories) : "no data");
                                        dr[12] = (names.gender != null && names.gender != string.Empty ? RemoveDiacritics(names.gender) : "no data");
                                        dr[13] = (dateofbirth != null && dateofbirth != string.Empty ? RemoveDiacritics(dateofbirth) : "no data");
                                        dr[14] = (country != null && country != string.Empty ? RemoveDiacritics(country) : "no data");
                                        dr[15] = (names.pepStatus != null && names.pepStatus != string.Empty ? RemoveDiacritics(names.pepStatus) : "no data");
                                        dr[16] = (names.providerType != null && names.providerType != string.Empty ? RemoveDiacritics(names.providerType) : "no data");
                                        dr[17] = (alias != null && alias != string.Empty ? RemoveDiacritics(alias) : (aliasSetup == "1" ? "no data" : "not enabled"));
                                        dr[18] = (remarks != null && remarks != string.Empty ? RemoveDiacritics(remarks) : (remarksSetup == "1" ? "no data" : "not enabled"));
                                        dr[19] = (resolution != null && resolution != string.Empty ? RemoveDiacritics(resolution) : "no data");
                                        dr[20] = (resolutionRemarks != null && resolutionRemarks != string.Empty ? RemoveDiacritics(resolutionRemarks) : "no data");
                                        dr[21] = (names.entityCreationDate != null && names.entityCreationDate != string.Empty ? StringToDate(names.entityCreationDate) : "no data");
                                        dr[22] = (names.entityModificationDate != null && names.entityModificationDate != string.Empty ? StringToDate(names.entityModificationDate) : "no data");
                                        dtReturn.Rows.Add(dr);
                                    }
                                    skip = false;
                                    listSources = "";
                                    listCategories = "";
                                    dateofbirth = "";
                                    country = "";
                                    alias = "";
                                    remarks = "";

                                    if (dtReturn.Rows.Count <= 0 && returnCount == count)
                                    {
                                        caseId = hit.responseBody.caseId;
                                        caseSystemId = hit.responseBody.caseSystemId;
                                        noResult = true;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        else
                        {
                            caseId = hit.responseBody.caseId;
                            caseSystemId = hit.responseBody.caseSystemId;
                            noResult = true;
                        }
                    }
                }

                if (noResult)
                {
                    dr = dtReturn.NewRow();
                    dr[0] = caseId;
                    dr[1] = caseSystemId;
                    dtReturn.Rows.Add(dr);
                    SaveWatchlist(dtReturn, CIFID, false, 0);
                }
            }
            catch (Exception ex)
            {
                IsDown = true;
                new csAuditLog().writeAudit("", "REFINITIV ERROR", "DisplayWatchlist", ex.Message + "|" + ex.StackTrace, "", csAuditLog.Category.Refinitiv);
                return null;
            }
            return dtReturn;
        }

        public bool SaveWatchlist(DataTable dt, string CIFID, bool OverrideChecking, int hitCount = 0)
        {
            bool ok = false;
            try
            {
                if (CIFID != string.Empty)
                {
                    DataTable dtCaseId = this.getRefinitivByCaseId(CIFID);
                    if (dtCaseId != null && dtCaseId.Rows.Count > 0)
                    {
                        if (!OverrideChecking)
                        {
                            return true;
                        }
                    }
                }

                base.addMode = true;
                int i = 0;
                string providerType = string.Empty;
                string caseId = string.Empty;
                string caseSystemId = string.Empty;
                bool watchlist = false;
                bool client = false;

                for (i = 0; i <= (dt.Rows.Count - 1); i++)
                {
                    if (!watchlist && dt.Rows[i]["ProviderType"].ToString().ToUpper() == "WATCHLIST")
                    {
                        watchlist = true;
                    }
                    if (!client && dt.Rows[i]["ProviderType"].ToString().ToUpper() == "CLIENT_WATCHLIST")
                    {
                        client = true;
                    }
                    caseId = dt.Rows[i]["CaseID"].ToString();
                    caseSystemId = dt.Rows[i]["CaseSystemID"].ToString();
                }

                if (watchlist)
                {
                    providerType = "WATCHLIST";
                }
                if (client)
                {
                    providerType = "WATCHLIST,CLIENT_WATCHLIST";
                }

                bool withHit = false;
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Rows.Count == 1 && (dt.Rows[0][2] == DBNull.Value || dt.Rows[0][2].ToString() == string.Empty))
                    {
                        withHit = false;
                    }
                    else
                    {
                        withHit = true;
                    }
                }

                this.setBase("SELECT TOP 1 * FROM TBL_REFINITIV_HITLIST", true);
                setColumn("MakerId", getUserId());
                setColumn("CIFID", CIFID);
                setColumn("CaseID", caseId);
                setColumn("CaseSystemID", caseSystemId);
                setColumn("ProviderType", providerType);
                setColumn("DateCreated", DateTime.Now);
                setColumn("Hit", withHit);
                setColumn("HitCount", (hitCount > 0 ? hitCount : (withHit ? dt.Rows.Count : 0)));
                setColumn("Archive", false);
                setColumn("Deleted", false);
                ok = base.save(sqlHelperWs.RETAIN_DATASET);
                if (!ok)
                {
                    new csAuditLog().writeAudit("", "REFINITIV ERROR", "SaveWatchlist", CIFID, base.Message, csAuditLog.Category.Refinitiv);
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "REFINITIV ERROR", "SaveWatchlist", ex.Message + "|" + ex.StackTrace, "CIFID=" + CIFID, csAuditLog.Category.Refinitiv);
            }
            return ok;
        }

        public bool UpdateWatchlist(string CaseSystemId, bool ArchiveOnly, string CheckerId)
        {
            new csAuditLog().writeAudit("", "REFINITIV", "UpdateWatchlist", "CaseSystemID=" + CaseSystemId + "|ArchiveOnly=" + ArchiveOnly + "|CaseSystemID=" + CheckerId, base.Message, csAuditLog.Category.Refinitiv);
            bool ok = false;
            try
            {
                base.addMode = false;
                this.setBase("SELECT TOP 1 * FROM TBL_REFINITIV_HITLIST WHERE CaseSystemID = '" + CaseSystemId + "'", false);
                if (ArchiveOnly)
                {
                    setColumn("Archive", true);
                }
                else
                {
                    setColumn("Deleted", true);
                }
                setColumn("CheckerId", CheckerId);
                setColumn("DateModified", DateTime.Now);
                ok = base.save(sqlHelperWs.RETAIN_DATASET);
                if (!ok)
                {
                    new csAuditLog().writeAudit("", "REFINITIV ERROR", "UpdateWatchlist", CaseSystemId, base.Message, csAuditLog.Category.Refinitiv);
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "REFINITIV ERROR", "TrancClass.irsRefinitiv.UpdateWatchlist", ex.Message + "|" + ex.StackTrace, "CaseSystemID=" + CaseSystemId, csAuditLog.Category.Refinitiv);
            }
            return ok;
        }

        public bool UpdateWatchlist_CaseIDExist(string CaseID, string CaseSystemID, DataTable dt, int hitCount = 0)
        {
            new csAuditLog().writeAudit("", "REFINITIV", "UpdateWatchlist_CaseIDExist", "CaseSystemID=" + CaseSystemID + "|CaseID=" + CaseID, base.Message, csAuditLog.Category.Refinitiv);
            bool ok = false;
            try
            {
                base.addMode = false;
                this.setBase("SELECT TOP 1 * FROM TBL_REFINITIV_HITLIST WHERE CaseSystemID = '" + CaseSystemID + "'", false);

                int i = 0;
                string providerType = string.Empty;
                string caseId = string.Empty;
                string caseSystemId = string.Empty;
                bool watchlist = false;
                bool client = false;

                for (i = 0; i <= (dt.Rows.Count - 1); i++)
                {
                    if (!watchlist && dt.Rows[i]["ProviderType"].ToString().ToUpper() == "WATCHLIST")
                    {
                        watchlist = true;
                    }
                    if (!client && dt.Rows[i]["ProviderType"].ToString().ToUpper() == "CLIENT_WATCHLIST")
                    {
                        client = true;
                    }
                }

                if (watchlist)
                {
                    providerType = "WATCHLIST";
                }
                if (client)
                {
                    providerType = "WATCHLIST,CLIENT_WATCHLIST";
                }

                bool withHit = false;
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Rows.Count == 1 && (dt.Rows[0][2] == DBNull.Value || dt.Rows[0][2].ToString() == string.Empty))
                    {
                        withHit = false;
                    }
                    else
                    {
                        withHit = true;
                    }
                }
                setColumn("ProviderType", providerType);
                setColumn("Hit", withHit);
                setColumn("HitCount", (hitCount > 0 ? hitCount : (withHit ? dt.Rows.Count : 0)));
                setColumn("DateModified", DateTime.Now);
                ok = base.save(sqlHelperWs.RETAIN_DATASET);
                if (!ok)
                {
                    new csAuditLog().writeAudit("", "REFINITIV ERROR", "UpdateWatchlist_CaseIDExist", CaseSystemID, base.Message, csAuditLog.Category.Refinitiv);
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "REFINITIV ERROR", "TrancClass.irsRefinitiv.UpdateWatchlist", ex.Message + "|" + ex.StackTrace, "CaseSystemID=" + CaseSystemID, csAuditLog.Category.Refinitiv);
            }
            return ok;
        }

        public void UpdateResolveWatchlist(string CaseSystemId, string CheckerId)
        {
            new csAuditLog().writeAudit("", "REFINITIV", "UpdateResolveWatchlist", "CaseSystemID=" + CaseSystemId + "|CaseSystemID=" + CheckerId, base.Message, csAuditLog.Category.Refinitiv);
            bool ok = false;
            try
            {
                base.addMode = false;
                this.setBase("SELECT TOP 1 * FROM TBL_REFINITIV_HITLIST WHERE CaseSystemID = '" + CaseSystemId + "'", false);
                setColumn("CheckerId", CheckerId);
                setColumn("DateResolve", DateTime.Now);
                ok = base.save(sqlHelperWs.RETAIN_DATASET);
                if (!ok)
                {
                    new csAuditLog().writeAudit("", "REFINITIV ERROR", "UpdateResolveWatchlist", CaseSystemId, base.Message, csAuditLog.Category.Refinitiv);
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "REFINITIV ERROR", "UpdateResolveWatchlist", ex.Message + "|" + ex.StackTrace, "CaseSystemID=" + CaseSystemId, csAuditLog.Category.Refinitiv);
            }
        }

        public bool UpdateWatchlistCIFID(string CaseSystemId, string CIFID)
        {
            bool ok = false;
            try
            {
                base.addMode = false;
                this.setBase("SELECT TOP 1 * FROM TBL_REFINITIV_HITLIST WHERE CaseSystemID = '" + CaseSystemId + "'", false);
                if (CIFID != "")
                {
                    setColumn("CIFID", CIFID);
                }
                ok = base.save(sqlHelperWs.RETAIN_DATASET);
                if (!ok)
                {
                    new csAuditLog().writeAudit("", "REFINITIV ERROR", "UpdateWatchlistCIFID", CaseSystemId + "-" + CIFID, base.Message, csAuditLog.Category.Refinitiv);
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "REFINITIV ERROR", "UpdateWatchlistCIFID", ex.Message + "|" + ex.StackTrace, "CaseSystemID=" + CaseSystemId + "_CIFID=" + CIFID, csAuditLog.Category.Refinitiv);
            }
            return ok;
        }

        public bool ResolveWatchlist(string BranchCode, string CIFID, bool Positive, string Remarks, string CheckerId)
        {
            bool ok = false;
            DataTable dt = new DataTable();
            string identifier = string.Empty;
            string caseId = string.Empty;
            string caseSystemId = string.Empty;
            string groupId = string.Empty;
            string groupIdName = string.Empty;
            string providerType = string.Empty;
            string config = string.Empty;
            string branchRegion = string.Empty;
            string[] arrProviderType = new string[2];
            string positiveMatchW = string.Empty;
            string[] arrPositiveMatchW = new string[4];
            string falseMatchW = string.Empty;
            string[] arrFalseMatchW = new string[4];
            string positiveMatchCW = string.Empty;
            string[] arrPositiveMatchCW = new string[4];
            string falseMatchCW = string.Empty;
            string[] arrFalseMatchCW = new string[4];
            string timeout = string.Empty;
            int i = 0;
            int retry = 1;
            try
            {
                DataTable dtCaseId = this.getRefinitivByCaseId(CIFID);
                if (dtCaseId != null && dtCaseId.Rows.Count > 0)
                {
                    caseId = dtCaseId.Rows[0]["CaseID"].ToString();
                    caseSystemId = dtCaseId.Rows[0]["CaseSystemID"].ToString();
                    providerType = dtCaseId.Rows[0]["ProviderType"].ToString();
                }

                if (providerType != string.Empty)
                {
                    arrProviderType = providerType.Split(',');
                }

                if (System.Web.HttpContext.Current.Session["RefinitivConfiguration"] == null)
                {
                    dt = new irsInternetPage().getLookup("RefinitivConfiguration");
                    System.Web.HttpContext.Current.Session["RefinitivConfiguration"] = dt;
                }
                else
                {
                    dt = (DataTable)System.Web.HttpContext.Current.Session["RefinitivConfiguration"];
                }

                branchRegion = getBranchRegion(BranchCode);
                if (branchRegion == string.Empty)
                {
                    new csAuditLog().writeAudit("", "REFINITIV ERROR", "ResolveWatchlist", "SETUP NOT FOUND FOR " + BranchCode, "", csAuditLog.Category.Refinitiv);
                    return false;
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    config = dt.AsEnumerable().Where(x => x.Field<string>("FldCode") == branchRegion).FirstOrDefault().Field<string>("FldDescription");
                    if (config != string.Empty)
                    {
                        identifier = config.Split('|').GetValue(0).ToString();
                        groupIdName = config.Split('|').GetValue(1).ToString();
                        groupId = config.Split('|').GetValue(2).ToString();
                    }
                    positiveMatchW = dt.AsEnumerable().Where(x => x.Field<string>("FldCode") == "Resolve-Positive-W").FirstOrDefault().Field<string>("FldDescription");
                    falseMatchW = dt.AsEnumerable().Where(x => x.Field<string>("FldCode") == "Resolve-False-W").FirstOrDefault().Field<string>("FldDescription");
                    positiveMatchCW = dt.AsEnumerable().Where(x => x.Field<string>("FldCode") == "Resolve-Positive-CW").FirstOrDefault().Field<string>("FldDescription");
                    falseMatchCW = dt.AsEnumerable().Where(x => x.Field<string>("FldCode") == "Resolve-False-CW").FirstOrDefault().Field<string>("FldDescription");
                    timeout = dt.AsEnumerable().Where(x => x.Field<string>("FldCode") == "Timeout").FirstOrDefault().Field<string>("FldDescription");
                    retry = Convert.ToInt32(dt.AsEnumerable().Where(x => x.Field<string>("FldCode") == "Retry").FirstOrDefault().Field<string>("FldDescription"));

                }

                if (positiveMatchW != string.Empty)
                {
                    arrPositiveMatchW = positiveMatchW.Split('~');
                }
                if (positiveMatchCW != string.Empty)
                {
                    arrPositiveMatchCW = positiveMatchCW.Split('~');
                }
                if (falseMatchW != string.Empty)
                {
                    arrFalseMatchW = falseMatchW.Split('~');
                }
                if (falseMatchCW != string.Empty)
                {
                    arrFalseMatchCW = falseMatchCW.Split('~');
                }

                if (groupId == string.Empty)
                {
                    APIGroupIDDto groupIdParam = new APIGroupIDDto
                    {
                        identifier = identifier + "_" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                        groupName = groupIdName
                    };

                    var jsonGroupID = JsonConvert.SerializeObject(groupIdParam);
                    object groupIdRes = null;
                    for (int ResolveWatchlist_groupIdRes = 0; ResolveWatchlist_groupIdRes < retry; ResolveWatchlist_groupIdRes++)
                    {
                        groupIdRes = ConnectToRefinitiv(jsonGroupID, ConfigurationManager.AppSettings["RAPI_REFINITIV_API_GROUPID"], timeout);
                        if (groupIdRes != null)
                        {
                            if (groupIdRes != null && (groupIdRes.ToString() != httpRes.ServiceUnavilable.ToString() && groupIdRes.ToString() != httpRes.Timedout.ToString()))
                            {
                                GroupIDResponseDto g = JsonConvert.DeserializeObject<GroupIDResponseDto>(groupIdRes.ToString());
                                if (g.replyStatus.statusCode == 0)
                                {
                                    if (g.responseBody.ToString() != string.Empty || g.responseBody != null)
                                    {
                                        groupId = g.responseBody;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

                ResolveReferenceResponseDto resolveReference = null;
                object referenceRes = null;

                if (System.Web.HttpContext.Current.Session["RefinitivResolveReference"] == null)
                {
                    APIResolveReferenceDto reference = new APIResolveReferenceDto
                    {
                        identifier = identifier + "_" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                        groupId = groupId
                    };

                    var json = JsonConvert.SerializeObject(reference);
                    referenceRes = null;
                    for (int ResolveWatchlist_referenceRes = 0; ResolveWatchlist_referenceRes < retry; ResolveWatchlist_referenceRes++)
                    {
                        referenceRes = ConnectToRefinitiv(json, ConfigurationManager.AppSettings["RAPI_REFINITIV_API_RESOLVE_REFERENCE"], timeout);
                        if (referenceRes != null)
                        {
                            if (referenceRes != null && (referenceRes.ToString() != httpRes.ServiceUnavilable.ToString() && referenceRes.ToString() != httpRes.Timedout.ToString()))
                            {
                                resolveReference = JsonConvert.DeserializeObject<ResolveReferenceResponseDto>(referenceRes.ToString());
                                if (resolveReference.replyStatus.statusCode == 0)
                                {
                                    if (resolveReference.responseBody.ToString() != string.Empty || resolveReference.responseBody != null)
                                    {
                                        System.Web.HttpContext.Current.Session["RefinitivResolveReference"] = referenceRes;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    referenceRes = System.Web.HttpContext.Current.Session["RefinitivResolveReference"];
                    resolveReference = JsonConvert.DeserializeObject<ResolveReferenceResponseDto>(referenceRes.ToString());
                }

                if (referenceRes != null)
                {
                    string resolveStatusW = string.Empty;
                    string resolveReasonW = string.Empty;
                    string resolveRiskW = string.Empty;
                    string resolveStatusWFP = string.Empty;
                    string resolveReasonWFP = string.Empty;
                    string resolveRiskWFP = string.Empty;
                    string resolveStatusCW = string.Empty;
                    string resolveReasonCW = string.Empty;
                    string resolveRiskCW = string.Empty;
                    string resolveStatusCWFP = string.Empty;
                    string resolveReasonCWFP = string.Empty;
                    string resolveRiskCWFP = string.Empty;

                    /* WATCHLIST - POSITIVE */
                    foreach (var item in resolveReference.responseBody.WATCHLIST.resolutionFields.statuses)
                    {
                        if (item.label.ToUpper().Trim() == arrPositiveMatchW.GetValue(0).ToString().ToUpper().Trim())
                        {
                            resolveStatusW = item.id;
                            break;
                        }
                    }
                    foreach (var item in resolveReference.responseBody.WATCHLIST.resolutionFields.reasons)
                    {
                        if (item.label.ToUpper().Trim() == arrPositiveMatchW.GetValue(1).ToString().ToUpper().Trim())
                        {
                            resolveReasonW = item.id;
                            break;
                        }
                    }
                    foreach (var item in resolveReference.responseBody.WATCHLIST.resolutionFields.risks)
                    {
                        if (item.label.ToUpper().Trim() == arrPositiveMatchW.GetValue(2).ToString().ToUpper().Trim())
                        {
                            resolveRiskW = item.id;
                            break;
                        }
                    }

                    /* WATCHLIST - FALSE-POSITIVE */
                    foreach (var item in resolveReference.responseBody.WATCHLIST.resolutionFields.statuses)
                    {
                        if (item.label.ToUpper().Trim() == arrFalseMatchW.GetValue(0).ToString().ToUpper().Trim())
                        {
                            resolveStatusWFP = item.id;
                            break;
                        }
                    }
                    foreach (var item in resolveReference.responseBody.WATCHLIST.resolutionFields.reasons)
                    {
                        if (item.label.ToUpper().Trim() == arrFalseMatchW.GetValue(1).ToString().ToUpper().Trim())
                        {
                            resolveReasonWFP = item.id;
                            break;
                        }
                    }
                    foreach (var item in resolveReference.responseBody.WATCHLIST.resolutionFields.risks)
                    {
                        if (item.label.ToUpper().Trim() == arrFalseMatchW.GetValue(2).ToString().ToUpper().Trim())
                        {
                            resolveRiskWFP = item.id;
                            break;
                        }
                    }

                    /* CLIENT WATCHLIST - POSITIVE */
                    foreach (var item in resolveReference.responseBody.CLIENT_WATCHLIST.resolutionFields.statuses)
                    {
                        if (item.label.ToUpper().Trim() == arrPositiveMatchCW.GetValue(0).ToString().ToUpper().Trim())
                        {
                            resolveStatusCW = item.id;
                            break;
                        }
                    }
                    foreach (var item in resolveReference.responseBody.CLIENT_WATCHLIST.resolutionFields.reasons)
                    {
                        if (item.label.ToUpper().Trim() == arrPositiveMatchCW.GetValue(1).ToString().ToUpper().Trim())
                        {
                            resolveReasonCW = item.id;
                            break;
                        }
                    }
                    foreach (var item in resolveReference.responseBody.CLIENT_WATCHLIST.resolutionFields.risks)
                    {
                        if (item.label.ToUpper().Trim() == arrPositiveMatchCW.GetValue(2).ToString().ToUpper().Trim())
                        {
                            resolveRiskCW = item.id;
                            break;
                        }
                    }

                    /* CLIENT WATCHLIST - FALSE-POSITIVE */
                    foreach (var item in resolveReference.responseBody.CLIENT_WATCHLIST.resolutionFields.statuses)
                    {
                        if (item.label.ToUpper().Trim() == arrFalseMatchCW.GetValue(0).ToString().ToUpper().Trim())
                        {
                            resolveStatusCWFP = item.id;
                            break;
                        }
                    }
                    foreach (var item in resolveReference.responseBody.CLIENT_WATCHLIST.resolutionFields.reasons)
                    {
                        if (item.label.ToUpper().Trim() == arrFalseMatchCW.GetValue(1).ToString().ToUpper().Trim())
                        {
                            resolveReasonCWFP = item.id;
                            break;
                        }
                    }
                    foreach (var item in resolveReference.responseBody.CLIENT_WATCHLIST.resolutionFields.risks)
                    {
                        if (item.label.ToUpper().Trim() == arrFalseMatchCW.GetValue(2).ToString().ToUpper().Trim())
                        {
                            resolveRiskCWFP = item.id;
                            break;
                        }
                    }

                    APIResultDto result = new APIResultDto
                    {
                        identifier = identifier + "_" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                        groupId = groupId,
                        caseId = caseId,
                        caseSystemId = caseSystemId,
                        screenCase = true
                    };

                    List<string> lstLocal = new List<string>();
                    string resultIds = string.Empty;
                    var jsonResult = JsonConvert.SerializeObject(result);

                    object resultRes = null;
                    for (int ResolveWatchlist_resultRes = 0; ResolveWatchlist_resultRes < retry; ResolveWatchlist_resultRes++)
                    {
                        resultRes = ConnectToRefinitiv(jsonResult, ConfigurationManager.AppSettings["RAPI_REFINITIV_API_RESULTS"], timeout);
                        if (resultRes != null)
                        {
                            if (resultRes != null && (resultRes.ToString() != httpRes.ServiceUnavilable.ToString() && resultRes.ToString() != httpRes.Timedout.ToString()))
                            {
                                ResultResponseDto jsonRes = JsonConvert.DeserializeObject<ResultResponseDto>(resultRes.ToString());
                                if (jsonRes.replyStatus.statusCode == 0)
                                {
                                    if (jsonRes.responseBody != null)
                                    {
                                        foreach (var names in jsonRes.responseBody)
                                        {
                                            if (names.providerType == "CLIENT_WATCHLIST")
                                            {
                                                lstLocal.Add(names.resultId.ToString());
                                            }
                                        }
                                        break;
                                    }
                                }

                            }
                        }
                    }

                    if (resultRes != null && (resultRes.ToString() == httpRes.ServiceUnavilable.ToString() || resultRes.ToString() == httpRes.Timedout.ToString()))
                    {
                        new csAuditLog().writeAudit("", "REFINITIV ERROR", "ResolveWatchlist", "Refinitiv is down. Result API response: " + resultRes.ToString(), "CIFID=" + CIFID, csAuditLog.Category.Refinitiv);
                    }

                    for (i = 0; i <= (lstLocal.Count - 1); i++)
                    {
                        resultIds += lstLocal[i].ToString() + ",";
                    }
                    if (resultIds != string.Empty)
                    {
                        resultIds = resultIds.Trim().Substring(0, resultIds.Trim().Length - 1);
                    }

                    APIResolveDto resolve = null;
                    bool watchlist = false;

                    for (i = 0; i <= (arrProviderType.Length - 1); i++)
                    {
                        if (!watchlist && arrProviderType.GetValue(i).ToString().ToUpper().Trim() == "WATCHLIST")
                        {
                            resolve = new APIResolveDto
                            {
                                identifier = identifier + "_" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                                resolveType = 2,
                                groupId = groupId,
                                caseId = caseId,
                                caseSystemId = caseSystemId,
                                resultId = resultIds,
                                statusId = (Positive ? resolveStatusW : resolveStatusWFP),
                                riskId = (Positive ? resolveRiskW : resolveRiskWFP),
                                reasonId = (Positive ? resolveReasonW : resolveReasonWFP),
                                resolutionRemarks = (CheckerId != string.Empty ? "Approved by: " + getUserFullName(CheckerId) : "") + " | " + (Remarks == string.Empty ? (Positive ? arrPositiveMatchW.GetValue(3).ToString() : arrFalseMatchW.GetValue(3).ToString()) : Remarks),
                                skipValidation = false
                            };
                        }
                        else
                        {
                            resolve = new APIResolveDto
                            {
                                identifier = identifier + "_" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                                resolveType = 3,
                                groupId = groupId,
                                caseId = caseId,
                                caseSystemId = caseSystemId,
                                resultId = resultIds,
                                statusId = (Positive ? resolveStatusCW : resolveStatusCWFP),
                                riskId = (Positive ? resolveRiskCW : resolveRiskCWFP),
                                reasonId = (Positive ? resolveReasonCW : resolveReasonCWFP),
                                resolutionRemarks = (CheckerId != string.Empty ? "Approved by: " + getUserFullName(CheckerId) : "") + " | " + (Remarks == string.Empty ? (Positive ? arrPositiveMatchCW.GetValue(3).ToString() : arrFalseMatchCW.GetValue(3).ToString()) : Remarks),
                                skipValidation = false
                            };
                        }

                        var json = JsonConvert.SerializeObject(resolve);
                        object resolveRes = null;
                        for (int ResolveWatchlist_resolveRes = 0; ResolveWatchlist_resolveRes < retry; ResolveWatchlist_resolveRes++)
                        {
                            resolveRes = ConnectToRefinitiv(json, ConfigurationManager.AppSettings["RAPI_REFINITIV_API_RESOLVE"], timeout);
                            if (resolveRes != null)
                            {
                                if (resolveRes != null && (resolveRes.ToString() != httpRes.ServiceUnavilable.ToString() && resolveRes.ToString() != httpRes.Timedout.ToString()))
                                {
                                    ResolveResponseDto resolveResponse = JsonConvert.DeserializeObject<ResolveResponseDto>(resolveRes.ToString());
                                    if (resolveResponse.replyStatus.statusCode == 0)
                                    {
                                        if (resolveResponse.responseBody != null)
                                        {
                                            if (resolveResponse.responseBody.ToUpper() == "SUCCESSFUL")
                                            {
                                                ok = true;
                                                if (arrProviderType.GetValue(i).ToString().ToUpper().Trim() == "WATCHLIST")
                                                {
                                                    watchlist = true;
                                                }
                                            }
                                            new csAuditLog().writeAudit("", "REFINITIV", "ResolveWatchlist", resolveResponse.responseBody, CIFID + "~" + caseSystemId, csAuditLog.Category.Refinitiv);
                                            UpdateResolveWatchlist(caseSystemId, CheckerId);
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        if (resolveRes != null && (resolveRes.ToString() == httpRes.ServiceUnavilable.ToString() || resolveRes.ToString() == httpRes.Timedout.ToString()))
                        {
                            new csAuditLog().writeAudit("", "REFINITIV ERROR", "ResolveWatchlist", "Refinitiv is down. Resolve API response: " + resolveRes.ToString(), "CIFID=" + CIFID, csAuditLog.Category.Refinitiv);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "REFINITIV ERROR", "ResolveWatchlist", ex.Message + "|" + ex.StackTrace, "CIFID=" + CIFID, csAuditLog.Category.Refinitiv);
            }
            return ok;
        }

        public bool ResolveWatchlist_CaseSystemID(string BranchCode, string CIFID, bool Positive, string Remarks, string CheckerId, string CaseSystemID)
        {
            bool ok = false;
            DataTable dt = new DataTable();
            string identifier = string.Empty;
            string caseId = string.Empty;
            string caseSystemId = string.Empty;
            string groupId = string.Empty;
            string groupIdName = string.Empty;
            string providerType = string.Empty;
            string config = string.Empty;
            string branchRegion = string.Empty;
            string[] arrProviderType = new string[2];
            string positiveMatchW = string.Empty;
            string[] arrPositiveMatchW = new string[4];
            string falseMatchW = string.Empty;
            string[] arrFalseMatchW = new string[4];
            string positiveMatchCW = string.Empty;
            string[] arrPositiveMatchCW = new string[4];
            string falseMatchCW = string.Empty;
            string[] arrFalseMatchCW = new string[4];
            string timeout = string.Empty;
            int i = 0;
            int retry = 1;
            try
            {
                if (CaseSystemID != null)
                {
                    DataTable dtCaseSytemID = this.getRefinitivByCaseSystemID(CaseSystemID);
                    if (dtCaseSytemID != null && dtCaseSytemID.Rows.Count > 0)
                    {
                        caseId = dtCaseSytemID.Rows[0]["CaseID"].ToString();
                        caseSystemId = dtCaseSytemID.Rows[0]["CaseSystemID"].ToString();
                        providerType = dtCaseSytemID.Rows[0]["ProviderType"].ToString();
                    }
                }
                else
                {
                    DataTable dtCaseId = this.getRefinitivByCaseId(CIFID);
                    if (dtCaseId != null && dtCaseId.Rows.Count > 0)
                    {
                        caseId = dtCaseId.Rows[0]["CaseID"].ToString();
                        caseSystemId = dtCaseId.Rows[0]["CaseSystemID"].ToString();
                        providerType = dtCaseId.Rows[0]["ProviderType"].ToString();
                    }
                }

                if (providerType != string.Empty)
                {
                    arrProviderType = providerType.Split(',');
                }

                if (System.Web.HttpContext.Current.Session["RefinitivConfiguration"] == null)
                {
                    dt = new irsInternetPage().getLookup("RefinitivConfiguration");
                    System.Web.HttpContext.Current.Session["RefinitivConfiguration"] = dt;
                }
                else
                {
                    dt = (DataTable)System.Web.HttpContext.Current.Session["RefinitivConfiguration"];
                }

                branchRegion = getBranchRegion(BranchCode);
                if (branchRegion == string.Empty)
                {
                    new csAuditLog().writeAudit("", "REFINITIV ERROR", "ResolveWatchlist", "SETUP NOT FOUND FOR " + BranchCode, "", csAuditLog.Category.Refinitiv);
                    return false;
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    config = dt.AsEnumerable().Where(x => x.Field<string>("FldCode") == branchRegion).FirstOrDefault().Field<string>("FldDescription");
                    if (config != string.Empty)
                    {
                        identifier = config.Split('|').GetValue(0).ToString();
                        groupIdName = config.Split('|').GetValue(1).ToString();
                        groupId = config.Split('|').GetValue(2).ToString();
                    }
                    positiveMatchW = dt.AsEnumerable().Where(x => x.Field<string>("FldCode") == "Resolve-Positive-W").FirstOrDefault().Field<string>("FldDescription");
                    falseMatchW = dt.AsEnumerable().Where(x => x.Field<string>("FldCode") == "Resolve-False-W").FirstOrDefault().Field<string>("FldDescription");
                    positiveMatchCW = dt.AsEnumerable().Where(x => x.Field<string>("FldCode") == "Resolve-Positive-CW").FirstOrDefault().Field<string>("FldDescription");
                    falseMatchCW = dt.AsEnumerable().Where(x => x.Field<string>("FldCode") == "Resolve-False-CW").FirstOrDefault().Field<string>("FldDescription");
                    timeout = dt.AsEnumerable().Where(x => x.Field<string>("FldCode") == "Timeout").FirstOrDefault().Field<string>("FldDescription");
                    retry = Convert.ToInt32(dt.AsEnumerable().Where(x => x.Field<string>("FldCode") == "Retry").FirstOrDefault().Field<string>("FldDescription"));

                }

                if (positiveMatchW != string.Empty)
                {
                    arrPositiveMatchW = positiveMatchW.Split('~');
                }
                if (positiveMatchCW != string.Empty)
                {
                    arrPositiveMatchCW = positiveMatchCW.Split('~');
                }
                if (falseMatchW != string.Empty)
                {
                    arrFalseMatchW = falseMatchW.Split('~');
                }
                if (falseMatchCW != string.Empty)
                {
                    arrFalseMatchCW = falseMatchCW.Split('~');
                }

                if (groupId == string.Empty)
                {
                    APIGroupIDDto groupIdParam = new APIGroupIDDto
                    {
                        identifier = identifier + "_" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                        groupName = groupIdName
                    };

                    var jsonGroupID = JsonConvert.SerializeObject(groupIdParam);
                    object groupIdRes = null;
                    for (int ResolveWatchlist_groupIdRes = 0; ResolveWatchlist_groupIdRes < retry; ResolveWatchlist_groupIdRes++)
                    {
                        groupIdRes = ConnectToRefinitiv(jsonGroupID, ConfigurationManager.AppSettings["RAPI_REFINITIV_API_GROUPID"], timeout);
                        if (groupIdRes != null)
                        {
                            if (groupIdRes != null && (groupIdRes.ToString() != httpRes.ServiceUnavilable.ToString() && groupIdRes.ToString() != httpRes.Timedout.ToString()))
                            {
                                GroupIDResponseDto g = JsonConvert.DeserializeObject<GroupIDResponseDto>(groupIdRes.ToString());
                                if (g.replyStatus.statusCode == 0)
                                {
                                    if (g.responseBody.ToString() != string.Empty || g.responseBody != null)
                                    {
                                        groupId = g.responseBody;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

                ResolveReferenceResponseDto resolveReference = null;
                object referenceRes = null;

                if (System.Web.HttpContext.Current.Session["RefinitivResolveReference"] == null)
                {
                    APIResolveReferenceDto reference = new APIResolveReferenceDto
                    {
                        identifier = identifier + "_" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                        groupId = groupId
                    };

                    var json = JsonConvert.SerializeObject(reference);
                    referenceRes = null;
                    for (int ResolveWatchlist_referenceRes = 0; ResolveWatchlist_referenceRes < retry; ResolveWatchlist_referenceRes++)
                    {
                        referenceRes = ConnectToRefinitiv(json, ConfigurationManager.AppSettings["RAPI_REFINITIV_API_RESOLVE_REFERENCE"], timeout);
                        if (referenceRes != null)
                        {
                            if (referenceRes != null && (referenceRes.ToString() != httpRes.ServiceUnavilable.ToString() && referenceRes.ToString() != httpRes.Timedout.ToString()))
                            {
                                resolveReference = JsonConvert.DeserializeObject<ResolveReferenceResponseDto>(referenceRes.ToString());
                                if (resolveReference.replyStatus.statusCode == 0)
                                {
                                    if (resolveReference.responseBody.ToString() != string.Empty || resolveReference.responseBody != null)
                                    {
                                        System.Web.HttpContext.Current.Session["RefinitivResolveReference"] = referenceRes;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    referenceRes = System.Web.HttpContext.Current.Session["RefinitivResolveReference"];
                    resolveReference = JsonConvert.DeserializeObject<ResolveReferenceResponseDto>(referenceRes.ToString());
                }

                if (referenceRes != null)
                {
                    string resolveStatusW = string.Empty;
                    string resolveReasonW = string.Empty;
                    string resolveRiskW = string.Empty;
                    string resolveStatusWFP = string.Empty;
                    string resolveReasonWFP = string.Empty;
                    string resolveRiskWFP = string.Empty;
                    string resolveStatusCW = string.Empty;
                    string resolveReasonCW = string.Empty;
                    string resolveRiskCW = string.Empty;
                    string resolveStatusCWFP = string.Empty;
                    string resolveReasonCWFP = string.Empty;
                    string resolveRiskCWFP = string.Empty;

                    /* WATCHLIST - POSITIVE */
                    foreach (var item in resolveReference.responseBody.WATCHLIST.resolutionFields.statuses)
                    {
                        if (item.label.ToUpper().Trim() == arrPositiveMatchW.GetValue(0).ToString().ToUpper().Trim())
                        {
                            resolveStatusW = item.id;
                            break;
                        }
                    }
                    foreach (var item in resolveReference.responseBody.WATCHLIST.resolutionFields.reasons)
                    {
                        if (item.label.ToUpper().Trim() == arrPositiveMatchW.GetValue(1).ToString().ToUpper().Trim())
                        {
                            resolveReasonW = item.id;
                            break;
                        }
                    }
                    foreach (var item in resolveReference.responseBody.WATCHLIST.resolutionFields.risks)
                    {
                        if (item.label.ToUpper().Trim() == arrPositiveMatchW.GetValue(2).ToString().ToUpper().Trim())
                        {
                            resolveRiskW = item.id;
                            break;
                        }
                    }

                    /* WATCHLIST - FALSE-POSITIVE */
                    foreach (var item in resolveReference.responseBody.WATCHLIST.resolutionFields.statuses)
                    {
                        if (item.label.ToUpper().Trim() == arrFalseMatchW.GetValue(0).ToString().ToUpper().Trim())
                        {
                            resolveStatusWFP = item.id;
                            break;
                        }
                    }
                    foreach (var item in resolveReference.responseBody.WATCHLIST.resolutionFields.reasons)
                    {
                        if (item.label.ToUpper().Trim() == arrFalseMatchW.GetValue(1).ToString().ToUpper().Trim())
                        {
                            resolveReasonWFP = item.id;
                            break;
                        }
                    }
                    foreach (var item in resolveReference.responseBody.WATCHLIST.resolutionFields.risks)
                    {
                        if (item.label.ToUpper().Trim() == arrFalseMatchW.GetValue(2).ToString().ToUpper().Trim())
                        {
                            resolveRiskWFP = item.id;
                            break;
                        }
                    }

                    /* CLIENT WATCHLIST - POSITIVE */
                    foreach (var item in resolveReference.responseBody.CLIENT_WATCHLIST.resolutionFields.statuses)
                    {
                        if (item.label.ToUpper().Trim() == arrPositiveMatchCW.GetValue(0).ToString().ToUpper().Trim())
                        {
                            resolveStatusCW = item.id;
                            break;
                        }
                    }
                    foreach (var item in resolveReference.responseBody.CLIENT_WATCHLIST.resolutionFields.reasons)
                    {
                        if (item.label.ToUpper().Trim() == arrPositiveMatchCW.GetValue(1).ToString().ToUpper().Trim())
                        {
                            resolveReasonCW = item.id;
                            break;
                        }
                    }
                    foreach (var item in resolveReference.responseBody.CLIENT_WATCHLIST.resolutionFields.risks)
                    {
                        if (item.label.ToUpper().Trim() == arrPositiveMatchCW.GetValue(2).ToString().ToUpper().Trim())
                        {
                            resolveRiskCW = item.id;
                            break;
                        }
                    }

                    /* CLIENT WATCHLIST - FALSE-POSITIVE */
                    foreach (var item in resolveReference.responseBody.CLIENT_WATCHLIST.resolutionFields.statuses)
                    {
                        if (item.label.ToUpper().Trim() == arrFalseMatchCW.GetValue(0).ToString().ToUpper().Trim())
                        {
                            resolveStatusCWFP = item.id;
                            break;
                        }
                    }
                    foreach (var item in resolveReference.responseBody.CLIENT_WATCHLIST.resolutionFields.reasons)
                    {
                        if (item.label.ToUpper().Trim() == arrFalseMatchCW.GetValue(1).ToString().ToUpper().Trim())
                        {
                            resolveReasonCWFP = item.id;
                            break;
                        }
                    }
                    foreach (var item in resolveReference.responseBody.CLIENT_WATCHLIST.resolutionFields.risks)
                    {
                        if (item.label.ToUpper().Trim() == arrFalseMatchCW.GetValue(2).ToString().ToUpper().Trim())
                        {
                            resolveRiskCWFP = item.id;
                            break;
                        }
                    }

                    APIResultDto result = new APIResultDto
                    {
                        identifier = identifier + "_" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                        groupId = groupId,
                        caseId = caseId,
                        caseSystemId = caseSystemId,
                        screenCase = true
                    };

                    List<string> lstLocal = new List<string>();
                    string resultIds = string.Empty;
                    var jsonResult = JsonConvert.SerializeObject(result);

                    object resultRes = null;
                    for (int ResolveWatchlist_resultRes = 0; ResolveWatchlist_resultRes < retry; ResolveWatchlist_resultRes++)
                    {
                        resultRes = ConnectToRefinitiv(jsonResult, ConfigurationManager.AppSettings["RAPI_REFINITIV_API_RESULTS"], timeout);
                        if (resultRes != null)
                        {
                            if (resultRes != null && (resultRes.ToString() != httpRes.ServiceUnavilable.ToString() && resultRes.ToString() != httpRes.Timedout.ToString()))
                            {
                                ResultResponseDto jsonRes = JsonConvert.DeserializeObject<ResultResponseDto>(resultRes.ToString());
                                if (jsonRes.replyStatus.statusCode == 0)
                                {
                                    if (jsonRes.responseBody != null)
                                    {
                                        foreach (var names in jsonRes.responseBody)
                                        {
                                            if (names.providerType == "CLIENT_WATCHLIST")
                                            {
                                                lstLocal.Add(names.resultId.ToString());
                                            }
                                        }
                                        break;
                                    }
                                }

                            }
                        }
                    }

                    if (resultRes != null && (resultRes.ToString() == httpRes.ServiceUnavilable.ToString() || resultRes.ToString() == httpRes.Timedout.ToString()))
                    {
                        new csAuditLog().writeAudit("", "REFINITIV ERROR", "ResolveWatchlist", "Refinitiv is down. Result API response: " + resultRes.ToString(), "CIFID=" + CIFID, csAuditLog.Category.Refinitiv);
                    }

                    for (i = 0; i <= (lstLocal.Count - 1); i++)
                    {
                        resultIds += lstLocal[i].ToString() + ",";
                    }
                    if (resultIds != string.Empty)
                    {
                        resultIds = resultIds.Trim().Substring(0, resultIds.Trim().Length - 1);
                    }

                    APIResolveDto resolve = null;
                    bool watchlist = false;

                    for (i = 0; i <= (arrProviderType.Length - 1); i++)
                    {
                        if (!watchlist && arrProviderType.GetValue(i).ToString().ToUpper().Trim() == "WATCHLIST")
                        {
                            resolve = new APIResolveDto
                            {
                                identifier = identifier + "_" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                                resolveType = 2,
                                groupId = groupId,
                                caseId = caseId,
                                caseSystemId = caseSystemId,
                                resultId = resultIds,
                                statusId = (Positive ? resolveStatusW : resolveStatusWFP),
                                riskId = (Positive ? resolveRiskW : resolveRiskWFP),
                                reasonId = (Positive ? resolveReasonW : resolveReasonWFP),
                                resolutionRemarks = (CheckerId != string.Empty ? "Approved by: " + getUserFullName(CheckerId) : "") + " | " + (Remarks == string.Empty ? (Positive ? arrPositiveMatchW.GetValue(3).ToString() : arrFalseMatchW.GetValue(3).ToString()) : Remarks),
                                skipValidation = false
                            };
                        }
                        else
                        {
                            resolve = new APIResolveDto
                            {
                                identifier = identifier + "_" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                                resolveType = 3,
                                groupId = groupId,
                                caseId = caseId,
                                caseSystemId = caseSystemId,
                                resultId = resultIds,
                                statusId = (Positive ? resolveStatusCW : resolveStatusCWFP),
                                riskId = (Positive ? resolveRiskCW : resolveRiskCWFP),
                                reasonId = (Positive ? resolveReasonCW : resolveReasonCWFP),
                                resolutionRemarks = (CheckerId != string.Empty ? "Approved by: " + getUserFullName(CheckerId) : "") + " | " + (Remarks == string.Empty ? (Positive ? arrPositiveMatchCW.GetValue(3).ToString() : arrFalseMatchCW.GetValue(3).ToString()) : Remarks),
                                skipValidation = false
                            };
                        }

                        var json = JsonConvert.SerializeObject(resolve);
                        object resolveRes = null;
                        for (int ResolveWatchlist_resolveRes = 0; ResolveWatchlist_resolveRes < retry; ResolveWatchlist_resolveRes++)
                        {
                            resolveRes = ConnectToRefinitiv(json, ConfigurationManager.AppSettings["RAPI_REFINITIV_API_RESOLVE"], timeout);
                            if (resolveRes != null)
                            {
                                if (resolveRes != null && (resolveRes.ToString() != httpRes.ServiceUnavilable.ToString() && resolveRes.ToString() != httpRes.Timedout.ToString()))
                                {
                                    ResolveResponseDto resolveResponse = JsonConvert.DeserializeObject<ResolveResponseDto>(resolveRes.ToString());
                                    if (resolveResponse.replyStatus.statusCode == 0)
                                    {
                                        if (resolveResponse.responseBody != null)
                                        {
                                            if (resolveResponse.responseBody.ToUpper() == "SUCCESSFUL")
                                            {
                                                ok = true;
                                                if (arrProviderType.GetValue(i).ToString().ToUpper().Trim() == "WATCHLIST")
                                                {
                                                    watchlist = true;
                                                }
                                            }
                                            new csAuditLog().writeAudit("", "REFINITIV", "ResolveWatchlist", resolveResponse.responseBody, CIFID + "~" + caseSystemId, csAuditLog.Category.Refinitiv);
                                            UpdateResolveWatchlist(caseSystemId, CheckerId);
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        if (resolveRes != null && (resolveRes.ToString() == httpRes.ServiceUnavilable.ToString() || resolveRes.ToString() == httpRes.Timedout.ToString()))
                        {
                            new csAuditLog().writeAudit("", "REFINITIV ERROR", "ResolveWatchlist", "Refinitiv is down. Resolve API response: " + resolveRes.ToString(), "CIFID=" + CIFID, csAuditLog.Category.Refinitiv);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "REFINITIV ERROR", "ResolveWatchlist", ex.Message + "|" + ex.StackTrace, "CIFID=" + CIFID, csAuditLog.Category.Refinitiv);
            }
            return ok;
        }

        public bool ArchiveDelete(string CaseSystemId, bool ArchiveOnly, string CheckerId)
        {
            bool ok = false;
            string timeout = string.Empty;
            int retry = 1;
            try
            {
                new csAuditLog().writeAudit("", "REFINITIV", "ArchiveDelete", "CaseSystemId" + CaseSystemId + " |ArchiveOnly=" + ArchiveOnly + " |CheckerId=" + CheckerId, "", csAuditLog.Category.Refinitiv);
                string BranchCode = ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location;
                string identifier = string.Empty;
                DataTable dt = new DataTable();
                string config = string.Empty;
                string branchRegion = getBranchRegion(BranchCode);

                if (branchRegion == string.Empty)
                {
                    new csAuditLog().writeAudit("", "REFINITIV ERROR", "irsRefinitiv.ArchiveDelete", "SETUP NOT FOUND FOR " + BranchCode, "", csAuditLog.Category.Refinitiv);
                }

                if (System.Web.HttpContext.Current.Session["RefinitivConfiguration"] == null)
                {
                    dt = new irsInternetPage().getLookup("RefinitivConfiguration");
                    System.Web.HttpContext.Current.Session["RefinitivConfiguration"] = dt;
                }
                else
                {
                    dt = (DataTable)System.Web.HttpContext.Current.Session["RefinitivConfiguration"];
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    config = dt.AsEnumerable().Where(x => x.Field<string>("FldCode") == branchRegion).FirstOrDefault().Field<string>("FldDescription");
                    if (config != string.Empty)
                    {
                        identifier = config.Split('|').GetValue(0).ToString();
                    }
                    timeout = dt.AsEnumerable().Where(x => x.Field<string>("FldCode") == "Timeout").FirstOrDefault().Field<string>("FldDescription");
                    retry = Convert.ToInt32(dt.AsEnumerable().Where(x => x.Field<string>("FldCode") == "Retry").FirstOrDefault().Field<string>("FldDescription"));

                }
                identifier = identifier + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");

                APIArchiveDeleteDto archive = new APIArchiveDeleteDto
                {
                    identifier = identifier,
                    caseSystemIds = CaseSystemId,
                    archiveOnly = ArchiveOnly
                };

                var json = JsonConvert.SerializeObject(archive);
                object archiveRes = null;
                for (int ArchiveDelete_resolveRes = 0; ArchiveDelete_resolveRes < retry; ArchiveDelete_resolveRes++)
                {
                    archiveRes = ConnectToRefinitiv(json, ConfigurationManager.AppSettings["RAPI_REFINITIV_API_ARCHIVE_DELETE"], timeout);
                    if (archiveRes != null)
                    {
                        if (archiveRes != null && (archiveRes.ToString() != httpRes.ServiceUnavilable.ToString() && archiveRes.ToString() != httpRes.Timedout.ToString()))
                        {
                            ArchiveDeleteResponseDto archiveResponse = JsonConvert.DeserializeObject<ArchiveDeleteResponseDto>(archiveRes.ToString());
                            if (archiveResponse.replyStatus.statusCode == 0)
                            {
                                if (archiveResponse.responseBody != null)
                                {
                                    foreach (var item in archiveResponse.responseBody)
                                    {
                                        if (item.caseSystemId != null && item.success.ToUpper() == "TRUE")
                                        {
                                            ok = true;
                                            string log = new irsInternetPage().getLookupValue("RefinitivConfiguration", "FldCode", "Log", "FldDescription");
                                            if (log == "1")
                                            {
                                                new csAuditLog().writeAudit("", "REFINITIV", "irsRefinitiv.ArchiveDelete", item.caseSystemId.ToUpper(), CaseSystemId, csAuditLog.Category.Refinitiv);
                                            }
                                        }
                                        if (ok)
                                        {
                                            UpdateWatchlist(CaseSystemId, ArchiveOnly, CheckerId);
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }

                if (archiveRes != null && (archiveRes.ToString() == httpRes.ServiceUnavilable.ToString() || archiveRes.ToString() == httpRes.Timedout.ToString()))
                {
                    new csAuditLog().writeAudit("", "REFINITIV ERROR", "irsRefinitiv.ArchiveDelete", "Refinitiv is down. Archive API response: " + archiveRes.ToString(), "CaseSystemId=" + CaseSystemId, csAuditLog.Category.Refinitiv);
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "REFINITIV ERROR", "irsRefinitiv.ArchiveDelete", ex.Message + "|" + ex.StackTrace, "CaseSystemId=" + CaseSystemId, csAuditLog.Category.Refinitiv);
            }
            return ok;
        }
    }
}