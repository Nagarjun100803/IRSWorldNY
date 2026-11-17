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
    public class irsRefinitiv_nWs : sqlHelper
    {
        public irsRefinitiv_nWs()
        {
            base.IsWinApp = true;
        }

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

        private string getUserId()
        {
            return ConfigurationManager.AppSettings["RAPI_REFINITIV_DEFAULT_USER"];
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

        public DataTable getLookup(string GroupName)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getLookup", new Object[] { GroupName });
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

        public string getLookupValue(string GroupName, string FindColumn, string LookValue, string ReturnColumn)
        {
            DataTable dt = this.getLookup(GroupName);
            string sReturn = string.Empty;
            int i = 0;

            if (dt != null && dt.Rows.Count > 0)
            {
                for (i = 0; i <= (dt.Rows.Count - 1); i++)
                {
                    if (dt.Rows[i][FindColumn].ToString().Trim() == LookValue)
                    {
                        sReturn = dt.Rows[i][ReturnColumn].ToString().Trim();
                    }
                }
            }
            return sReturn;
        }

        public void writeAudit(string Module, string Description, string NewValue, string OldValue)
        {
            string[] arrAudit = new string[7];
            try
            {
                arrAudit[0] = "0";
                arrAudit[1] = ConfigurationManager.AppSettings["RAPI_REFINITIV_DEFAULT_USER"];
                arrAudit[2] = Module;
                arrAudit[3] = Description;
                arrAudit[4] = NewValue;
                arrAudit[5] = OldValue;
                arrAudit[6] = "Refinitiv";
                base.execStoredProcedureUpdate("sp_Insert_Log", arrAudit);
            }
            catch { }
        }

        public bool validateISO3Country(string CountryCode)
        {
            bool found = false;
            try
            {
                DataTable dtISO3Country = new DataTable();
                int i = 0;
                DataRow data;

                dtISO3Country = getLookup("SAS_ISO3_Country");
                if (dtISO3Country != null && dtISO3Country.Rows.Count > 0)
                {
                    data = dtISO3Country.AsEnumerable().FirstOrDefault();
                    //data.Table.Rows[0]["AcctNum"].ToString()
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

        private object ConnectToRefinitiv(string json, string endpoint)
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
                client.Timeout = TimeSpan.FromMilliseconds(Convert.ToInt32(ConfigurationManager.AppSettings["RAPI_REFINITIV_Timeout"]));

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

                string log = getLookupValue("RefinitivConfiguration", "FldCode", "Log", "FldDescription");
                if (log == "1")
                {
                    writeAudit("REFINITIV", "REQUEST/RESPONSE", response, json + "~" + endpoint);
                }
                obj = JsonConvert.DeserializeObject<object>(response);
            }
            catch (Exception ex)
            {
                writeAudit("REFINITIV ERROR", "ConnectToRefinitiv", ex.Message + "|" + ex.StackTrace, json + "~" + endpoint);
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
                writeAudit("REFINITIV ERROR", "RemoveDiacritics", ex.Message + "|" + ex.StackTrace, text);
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
                writeAudit("REFINITIV ERROR", "StringToDate", ex.Message + "|" + ex.StackTrace, date);
                return "no data";
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

                dt = getLookup("RefinitivConfiguration");
                branchRegion = getBranchRegion(BranchCode);
                if (branchRegion == string.Empty)
                {
                    writeAudit("REFINITIV ERROR", "ScreenNameRefinitiv", "SETUP NOT FOUND FOR " + BranchCode, "");
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
                }

                if (groupId == string.Empty)
                {
                    APIGroupIDDto groupIdParam = new APIGroupIDDto
                    {
                        identifier = identifier + "_" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                        groupName = groupIdName
                    };

                    var jsonGroupID = JsonConvert.SerializeObject(groupIdParam);
                    object groupIdRes = ConnectToRefinitiv(jsonGroupID, ConfigurationManager.AppSettings["RAPI_REFINITIV_API_GROUPID"]);
                    if (groupIdRes != null && (groupId.ToString() == httpRes.ServiceUnavilable.ToString() || groupId.ToString() == httpRes.Timedout.ToString()))
                    {
                        IsDown = true;
                        return null;
                    }

                    if (groupIdRes != null)
                    {
                        GroupIDResponseDto g = JsonConvert.DeserializeObject<GroupIDResponseDto>(groupIdRes.ToString());
                        if (g.responseBody != string.Empty)
                        {
                            groupId = g.responseBody;
                        }
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
                object screeningResponseRes = ConnectToRefinitiv(json, ConfigurationManager.AppSettings["RAPI_REFINITIV_API_SCREENNAME"]);
                if (screeningResponseRes != null && (screeningResponseRes.ToString() == httpRes.ServiceUnavilable.ToString() || screeningResponseRes.ToString() == httpRes.Timedout.ToString()))
                {
                    IsDown = true;
                    return null;
                }

                if (screeningResponseRes != null)
                {
                    ScreeningResponseDto jsonRes = JsonConvert.DeserializeObject<ScreeningResponseDto>(screeningResponseRes.ToString());
                    var enumerable = new[] { jsonRes };
                    writeAudit("REFINITIV", "ScreenNameRefinitiv", jsonRes.replyStatus.message, FirstName + "~" + MiddleName + "~" + LastName);
                    return (IEnumerable<object>)enumerable.ToList();
                }
            }
            catch (Exception ex)
            {
                IsDown = true;
                writeAudit("REFINITIV ERROR", "ScreenNameRefinitiv", ex.Message + "|" + ex.StackTrace, FirstName + "~" + MiddleName + "~" + LastName);
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
            bool noResult = false;
            bool skip = false;
            int returnCount = 0;
            int count = 0;
            int i = 0;

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

                dt = getLookup("RefinitivConfiguration");
                branchRegion = getBranchRegion(BranchCode);
                if (branchRegion == string.Empty)
                {
                    writeAudit("REFINITIV ERROR", "GetResultsByCaseSystemId", "SETUP NOT FOUND FOR " + BranchCode, "");
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
                    returnCount = Convert.ToInt32(dt.AsEnumerable().Where(x => x.Field<string>("FldCode") == "ReturnCount").FirstOrDefault().Field<string>("FldDescription"));
                }

                if (groupId == string.Empty)
                {
                    APIGroupIDDto groupIdParam = new APIGroupIDDto
                    {
                        identifier = identifier + "_" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                        groupName = groupIdName
                    };

                    var jsonGroupID = JsonConvert.SerializeObject(groupIdParam);
                    object groupIdRes = ConnectToRefinitiv(jsonGroupID, ConfigurationManager.AppSettings["RAPI_REFINITIV_API_GROUPID"]);
                    if (groupIdRes != null)
                    {
                        GroupIDResponseDto g = JsonConvert.DeserializeObject<GroupIDResponseDto>(groupIdRes.ToString());
                        if (g.responseBody != string.Empty)
                        {
                            groupId = g.responseBody;
                        }
                    }
                }

                ProviderResponseDto provider = null;
                APIProviderDto providers = new APIProviderDto
                {
                    identifier = identifier + "_" + DateTime.Now.ToString("yyyyMMddHHmmss")
                };

                var jsonProvider = JsonConvert.SerializeObject(providers);
                object providerRes = ConnectToRefinitiv(jsonProvider, ConfigurationManager.AppSettings["RAPI_REFINITIV_API_PROVIDER"]);
                if (providerRes != null && (providerRes.ToString() == httpRes.ServiceUnavilable.ToString() || providerRes.ToString() == httpRes.Timedout.ToString()))
                {
                    writeAudit("REFINITIV ERROR", "GetResultsByCaseSystemId - Provider", "Refinitiv is down. Provider API response: " + providerRes.ToString(), "CaseSystemId=" + CaseSystemId);
                    IsDown = true;
                    return null;
                }
                else
                {
                    provider = JsonConvert.DeserializeObject<ProviderResponseDto>(providerRes.ToString());
                }
                
                ResolveReferenceResponseDto resolveReference = null;
                object referenceRes = null;
                List<string> lstPositive = new List<string>();

                APIResolveReferenceDto reference = new APIResolveReferenceDto
                {
                    identifier = identifier + "_" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                    groupId = groupId
                };

                try
                {
                    var json = JsonConvert.SerializeObject(reference);
                    referenceRes = ConnectToRefinitiv(json, ConfigurationManager.AppSettings["RAPI_REFINITIV_API_RESOLVE_REFERENCE"]);
                    resolveReference = JsonConvert.DeserializeObject<ResolveReferenceResponseDto>(referenceRes.ToString());

                    if (referenceRes != null)
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
                }
                catch (Exception ex)
                {
                    writeAudit("REFINITIV ERROR", "GetResultsByCaseSystemId - Resolve Reference", ex.Message + "|" + ex.StackTrace, "");
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
                object resultRes = ConnectToRefinitiv(jsonResult, ConfigurationManager.AppSettings["RAPI_REFINITIV_API_RESULTS"]);

                if (resultRes != null)
                {
                    ResultResponseDto jsonRes = JsonConvert.DeserializeObject<ResultResponseDto>(resultRes.ToString());
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
                                            var jsonRecords = JsonConvert.SerializeObject(records);
                                            recordsRes = ConnectToRefinitiv(jsonRecords, ConfigurationManager.AppSettings["RAPI_REFINITIV_API_RECORD"]);
                                            if (recordsRes != null && (recordsRes.ToString() == httpRes.ServiceUnavilable.ToString() || recordsRes.ToString() == httpRes.Timedout.ToString()))
                                            {
                                                writeAudit("REFINITIV ERROR", "GetResultsByCaseSystemId - Records", "Refinitiv is down. Provider API response: " + referenceId, "CaseSystemId=" + CaseSystemId);
                                            }
                                            else
                                            {
                                                record = JsonConvert.DeserializeObject<RecordsResponseDto>(recordsRes.ToString());
                                            }

                                            if (record != null)
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
                                        catch (Exception ex)
                                        {
                                            writeAudit("REFINITIV ERROR", "GetResultsByCaseSystemId - Records", ex.Message + "|" + ex.StackTrace, referenceId);
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
                }
            }
            catch (Exception ex)
            {
                IsDown = true;
                writeAudit("REFINITIV ERROR", "GetResultsByCaseSystemId", ex.Message + "|" + ex.StackTrace, "CaseSystemId=" + CaseSystemId);
            }
            return dtReturn;
        }

        public DataTable DisplayWatchlistBulk(string BranchCode, DataTable dtCIF, string BulkIdentifier, out bool IsDown)
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
            int i = 0;

            //dtCIF = Firstname, Lastname, CIFID

            dtReturn.Columns.Add("CIFID");
            dtReturn.Columns.Add("Hit");
            dtReturn.Columns.Add("Down");

            try
            {
                if (dtCIF != null && dtCIF.Rows.Count > 0)
                {
                    dt = getLookup("RefinitivConfiguration");
                    branchRegion = getBranchRegion(BranchCode);
                    if (branchRegion == string.Empty)
                    {
                        writeAudit("REFINITIV ERROR", "DisplayWatchlistBulk", "SETUP NOT FOUND FOR " + BranchCode, "");
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
                    }

                    if (groupId == string.Empty)
                    {
                        APIGroupIDDto groupIdParam = new APIGroupIDDto
                        {
                            identifier = identifier + "_" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                            groupName = groupIdName
                        };

                        var jsonGroupID = JsonConvert.SerializeObject(groupIdParam);
                        object groupIdRes = ConnectToRefinitiv(jsonGroupID, ConfigurationManager.AppSettings["RAPI_REFINITIV_API_GROUPID"]);
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
                    for (i = 0; i <= (dtCIF.Rows.Count - 1); i++)
                    {
                        Case c = new Case()
                        {
                            caseId = identifier + "_" + dtCIF.Rows[i][2].ToString() + "_" + dtCIF.Rows[i][3].ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                            entityType = entityType,
                            name = (dtCIF.Rows[i][0].ToString() + " " + dtCIF.Rows[i][1].ToString()).Trim()
                        };
                        lstCase.Add(c);
                    }
                    param.cases = lstCase;

                    var json = JsonConvert.SerializeObject(param);
                    object screeningResponseBulkRes = ConnectToRefinitiv(json, ConfigurationManager.AppSettings["RAPI_REFINITIV_API_SCREENNAME_BULK"]);
                    if (screeningResponseBulkRes != null && (screeningResponseBulkRes.ToString() == httpRes.ServiceUnavilable.ToString() || screeningResponseBulkRes.ToString() == httpRes.Timedout.ToString()))
                    {
                        IsDown = true;
                        return null;
                    }

                    if (screeningResponseBulkRes != null)
                    {
                        ScreeningResponseBulkDto jsonRes = JsonConvert.DeserializeObject<ScreeningResponseBulkDto>(screeningResponseBulkRes.ToString());
                        writeAudit("REFINITIV", "DisplayWatchlistBulk", jsonRes.replyStatus.message, BulkIdentifier);

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
                                        SaveWatchlistData(dt, dtCIF);
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
                IsDown = true;
                writeAudit("REFINITIV ERROR", "DisplayWatchlistBulk", ex.Message + "|" + ex.StackTrace, "");
            }
            return dtReturn;
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
            int returnCount = 0;
            bool skip = false;
            int i = 0;
            bool isDown = false;

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

                dt = getLookup("RefinitivConfiguration");
                branchRegion = getBranchRegion(BranchCode);
                if (branchRegion == string.Empty)
                {
                    writeAudit("REFINITIV ERROR", "DisplayWatchlist", "SETUP NOT FOUND FOR " + BranchCode, "");
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
                    returnCount = Convert.ToInt32(dt.AsEnumerable().Where(x => x.Field<string>("FldCode") == "ReturnCount").FirstOrDefault().Field<string>("FldDescription"));
                }

                if (groupId == string.Empty)
                {
                    APIGroupIDDto groupIdParam = new APIGroupIDDto
                    {
                        identifier = identifier + "_" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                        groupName = groupIdName
                    };

                    var jsonGroupID = JsonConvert.SerializeObject(groupIdParam);
                    object groupIdRes = ConnectToRefinitiv(jsonGroupID, ConfigurationManager.AppSettings["RAPI_REFINITIV_API_GROUPID"]);
                    if (groupIdRes != null)
                    {
                        GroupIDResponseDto g = JsonConvert.DeserializeObject<GroupIDResponseDto>(groupIdRes.ToString());
                        if (g.responseBody != string.Empty)
                        {
                            groupId = g.responseBody;
                        }
                    }
                }

                ProviderResponseDto provider = null;
                APIProviderDto providers = new APIProviderDto
                {
                    identifier = identifier + "_" + DateTime.Now.ToString("yyyyMMddHHmmss")
                };

                var json = JsonConvert.SerializeObject(providers);
                object providerRes = ConnectToRefinitiv(json, ConfigurationManager.AppSettings["RAPI_REFINITIV_API_PROVIDER"]);
                if (providerRes != null && (providerRes.ToString() == httpRes.ServiceUnavilable.ToString() || providerRes.ToString() == httpRes.Timedout.ToString()))
                {
                    writeAudit("REFINITIV ERROR", "DisplayWatchlist - Provider", "Refinitiv is down. Provider API response: " + providerRes.ToString(), "CIFID=" + CIFID + " FIRSTNAME=" + FName + " MIDDLENAME=" + MName + " LASTNAME=" + LName);
                    IsDown = true;
                    return null;
                }
                else
                {
                    provider = JsonConvert.DeserializeObject<ProviderResponseDto>(providerRes.ToString());
                }

                ResolveReferenceResponseDto resolveReference = null;
                object referenceRes = null;
                List<string> lstPositive = new List<string>();

                APIResolveReferenceDto reference = new APIResolveReferenceDto
                {
                    identifier = identifier + "_" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                    groupId = groupId
                };

                json = JsonConvert.SerializeObject(reference);
                referenceRes = ConnectToRefinitiv(json, ConfigurationManager.AppSettings["RAPI_REFINITIV_API_RESOLVE_REFERENCE"]);
                resolveReference = JsonConvert.DeserializeObject<ResolveReferenceResponseDto>(referenceRes.ToString());

                if (referenceRes != null)
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
                    object resultRes = ConnectToRefinitiv(jsonResult, ConfigurationManager.AppSettings["RAPI_REFINITIV_API_RESULTS"]);

                    if (resultRes != null)
                    {
                        ResultResponseDto jsonRes = JsonConvert.DeserializeObject<ResultResponseDto>(resultRes.ToString());
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
                                            json = JsonConvert.SerializeObject(records);
                                            recordsRes = ConnectToRefinitiv(json, ConfigurationManager.AppSettings["RAPI_REFINITIV_API_RECORD"]);
                                            if (recordsRes != null && (recordsRes.ToString() == httpRes.ServiceUnavilable.ToString() || recordsRes.ToString() == httpRes.Timedout.ToString()))
                                            {
                                                writeAudit("REFINITIV ERROR", "DisplayWatchlist - Records", "Refinitiv is down. Provider API response: " + recordsRes.ToString(), "CIFID=" + CIFID + " FIRSTNAME=" + FName + " MIDDLENAME=" + MName + " LASTNAME=" + LName);
                                            }
                                            else
                                            {
                                                record = JsonConvert.DeserializeObject<RecordsResponseDto>(recordsRes.ToString());
                                            }

                                            if (record != null)
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
                                        catch (Exception ex)
                                        {
                                            writeAudit("REFINITIV ERROR", "DisplayWatchlist - Records (!Screen)", ex.Message + "|" + ex.StackTrace, referenceId);
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
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
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
                                            json = JsonConvert.SerializeObject(records);
                                            recordsRes = ConnectToRefinitiv(json, ConfigurationManager.AppSettings["RAPI_REFINITIV_API_RECORD"]);
                                            if (recordsRes != null && (recordsRes.ToString() == httpRes.ServiceUnavilable.ToString() || recordsRes.ToString() == httpRes.Timedout.ToString()))
                                            {
                                                writeAudit("REFINITIV ERROR", "DisplayWatchlist - Records", "Refinitiv is down. Provider API response: " + recordsRes.ToString(), "CIFID=" + CIFID + " FIRSTNAME=" + FName + " MIDDLENAME=" + MName + " LASTNAME=" + LName);
                                            }
                                            else
                                            {
                                                record = JsonConvert.DeserializeObject<RecordsResponseDto>(recordsRes.ToString());
                                            }

                                            if (record != null)
                                            {
                                                foreach (var name in record.responseBody.names)
                                                {
                                                    alias += name.fullName + "; ";
                                                }
                                                foreach (var bio in record.responseBody.details)
                                                {
                                                    if (bio.detailType.ToUpper() == "BIOGRAPHY")
                                                    {
                                                        remarks += bio.text + "; ";
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
                                        catch (Exception ex)
                                        {
                                            writeAudit("REFINITIV ERROR", "DisplayWatchlist - Records (Screen)", ex.Message + "|" + ex.StackTrace, referenceId);
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
                writeAudit("REFINITIV ERROR", "DisplayWatchlist", ex.Message + "|" + ex.StackTrace, "");
                return null;
            }
            return dtReturn;
        }

        public bool SaveWatchlist(DataTable dtWatchlist, string CIFID, bool OverrideChecking, int hitCount = 0)
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

                for (i = 0; i <= (dtWatchlist.Rows.Count - 1); i++)
                {
                    if (!watchlist && dtWatchlist.Rows[i]["ProviderType"].ToString().ToUpper() == "WATCHLIST")
                    {
                        watchlist = true;
                    }
                    if (!client && dtWatchlist.Rows[i]["ProviderType"].ToString().ToUpper() == "CLIENT_WATCHLIST")
                    {
                        client = true;
                    }
                    caseId = dtWatchlist.Rows[i]["CaseID"].ToString();
                    caseSystemId = dtWatchlist.Rows[i]["CaseSystemID"].ToString();
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
                if (dtWatchlist != null && dtWatchlist.Rows.Count > 0)
                {
                    if (dtWatchlist.Rows.Count == 1 && (dtWatchlist.Rows[0][2] == DBNull.Value || dtWatchlist.Rows[0][2].ToString() == string.Empty))
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
                setColumn("HitCount", (hitCount > 0 ? hitCount : (withHit ? dtWatchlist.Rows.Count : 0)));
                setColumn("Archive", false);
                setColumn("Deleted", false);
                ok = base.save(sqlHelperWs.RETAIN_DATASET);
                if (!ok)
                {
                    writeAudit("REFINITIV ERROR", "SaveWatchlist", CIFID, base.Message);
                }
            }
            catch (Exception ex)
            {
                writeAudit("REFINITIV ERROR", "SaveWatchlist", ex.Message + "|" + ex.StackTrace, "CIFID=" + CIFID);
            }
            return ok;
        }

        public void SaveWatchlistData(DataTable dtWatchlist, DataTable dtCIF)
        {
            string CIFID = string.Empty;
            string lastName = string.Empty;
            string firstName = string.Empty;
            string providerType = string.Empty;
            int i = 0;
            int fileseqid = 0;
            bool watchlist = false;
            bool client = false;
            bool ok = false;
            string[] dr_caseid;
            
            try
            {
                for (i = 0; i <= (dtWatchlist.Rows.Count - 1); i++)
                {
                    //dtCIF = Firstname, Lastname, CIFID, FileSEQ_ID
                    if (dtCIF != null && dtCIF.Rows.Count > 0)
                    {
                        dr_caseid = dtWatchlist.Rows[i]["CaseID"].ToString().Trim().Split('_');
                        CIFID = dr_caseid[1];
                        
                        fileseqid = Convert.ToInt32(dtCIF.AsEnumerable().Where(x => x.Field<string>("CIFID") == CIFID).FirstOrDefault().Field<string>("FileSEQ_ID"));
                        lastName = dtCIF.AsEnumerable().Where(x => x.Field<string>("CIFID") == CIFID).FirstOrDefault().Field<string>("LastName");
                        firstName = dtCIF.AsEnumerable().Where(x => x.Field<string>("CIFID") == CIFID).FirstOrDefault().Field<string>("FirstName");                        
                    }

                    if (!watchlist && dtWatchlist.Rows[i]["ProviderType"].ToString().ToUpper() == "WATCHLIST")
                    {
                        watchlist = true;
                    }
                    if (!client && dtWatchlist.Rows[i]["ProviderType"].ToString().ToUpper() == "CLIENT_WATCHLIST")
                    {
                        client = true;
                    }

                    if (watchlist)
                    {
                        providerType = "WATCHLIST";
                    }
                    if (client)
                    {
                        providerType = "WATCHLIST,CLIENT_WATCHLIST";
                    }

                    this.setBase("SELECT TOP 1 * FROM TBL_REFINITIV_HITLIST_DETAILS", true);
                    setColumn("CIFID", CIFID);
                    setColumn("LastName", lastName);
                    setColumn("FirstName", firstName);
                    setColumn("MiddleName", "");
                    setColumn("Watchlist_Name", dtWatchlist.Rows[i][8].ToString());
                    setColumn("Watchlist_Type", dtWatchlist.Rows[i][9].ToString());
                    setColumn("Provider_Type", providerType);
                    setColumn("Alias_Name", dtWatchlist.Rows[i][17].ToString());
                    setColumn("Watchlist_Score", dtWatchlist.Rows[i][4].ToString());
                    setColumn("FileSEQ_ID", fileseqid);
                    ok = base.save(sqlHelperWs.RETAIN_DATASET);
                    if (!ok)
                    {
                        writeAudit("REFINITIV ERROR", "SaveWatchlistData", CIFID, base.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                writeAudit("REFINITIV ERROR", "SaveWatchlistData", ex.Message + "|" + ex.StackTrace, "CIFID=" + CIFID);
            }
        }

        public bool UpdateWatchlist(string CaseSystemId, bool ArchiveOnly, string CheckerId)
        {
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
                    writeAudit("REFINITIV ERROR", "SaveWatchlist", CaseSystemId, base.Message);
                }
            }
            catch (Exception ex)
            {
                writeAudit("REFINITIV ERROR", "SaveWatchlist", ex.Message + "|" + ex.StackTrace, "CaseSystemID=" + CaseSystemId);
            }
            return ok;
        }

        public void UpdateResolveWatchlist(string CaseSystemId, string CheckerId)
        {
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
                    writeAudit("REFINITIV ERROR", "UpdateResolveWatchlist", CaseSystemId, base.Message);
                }
            }
            catch (Exception ex)
            {
                writeAudit("REFINITIV ERROR", "UpdateResolveWatchlist", ex.Message + "|" + ex.StackTrace, "CaseSystemID=" + CaseSystemId);
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
                    writeAudit("REFINITIV ERROR", "UpdateWatchlistCIFID", CaseSystemId + "-" + CIFID, base.Message);
                }
            }
            catch (Exception ex)
            {
                writeAudit("REFINITIV ERROR", "UpdateWatchlistCIFID", ex.Message + "|" + ex.StackTrace, "CaseSystemID=" + CaseSystemId + "_CIFID=" + CIFID);
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
            int i = 0;

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

                dt = getLookup("RefinitivConfiguration");
                branchRegion = getBranchRegion(BranchCode);
                if (branchRegion == string.Empty)
                {
                    writeAudit("REFINITIV ERROR", "ResolveWatchlist", "SETUP NOT FOUND FOR " + BranchCode, "");
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
                    object groupIdRes = ConnectToRefinitiv(jsonGroupID, ConfigurationManager.AppSettings["RAPI_REFINITIV_API_GROUPID"]);
                    if (groupIdRes != null)
                    {
                        GroupIDResponseDto g = JsonConvert.DeserializeObject<GroupIDResponseDto>(groupIdRes.ToString());
                        if (g.responseBody != string.Empty)
                        {
                            groupId = g.responseBody;
                        }
                    }
                }

                ResolveReferenceResponseDto resolveReference = null;
                object referenceRes = null;

                APIResolveReferenceDto reference = new APIResolveReferenceDto
                {
                    identifier = identifier + "_" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                    groupId = groupId
                };

                var json = JsonConvert.SerializeObject(reference);
                referenceRes = ConnectToRefinitiv(json, ConfigurationManager.AppSettings["RAPI_REFINITIV_API_RESOLVE_REFERENCE"]);
                if (referenceRes != null && (referenceRes.ToString() == httpRes.ServiceUnavilable.ToString() || referenceRes.ToString() == httpRes.Timedout.ToString()))
                {
                    writeAudit("REFINITIV ERROR", "ResolveWatchlist - Reference", "Refinitiv is down. Reference API response: " + referenceRes.ToString(), "CIFID=" + CIFID);
                }
                else
                {
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
                    object resultRes = ConnectToRefinitiv(jsonResult, ConfigurationManager.AppSettings["RAPI_REFINITIV_API_RESULTS"]);
                    if (resultRes != null && (resultRes.ToString() == httpRes.ServiceUnavilable.ToString() || resultRes.ToString() == httpRes.Timedout.ToString()))
                    {
                        writeAudit("REFINITIV ERROR", "ResolveWatchlist - Results", "Refinitiv is down. Result API response: " + resultRes.ToString(), "CIFID=" + CIFID);
                    }
                    else
                    {
                        if (resultRes != null)
                        {
                            ResultResponseDto jsonRes = JsonConvert.DeserializeObject<ResultResponseDto>(resultRes.ToString());
                            foreach (var names in jsonRes.responseBody)
                            {
                                if (names.providerType == "CLIENT_WATCHLIST")
                                {
                                    lstLocal.Add(names.resultId.ToString());
                                }
                            }
                        }
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

                        var jsonResolve = JsonConvert.SerializeObject(resolve);
                        object resolveRes = ConnectToRefinitiv(jsonResolve, ConfigurationManager.AppSettings["RAPI_REFINITIV_API_RESOLVE"]);
                        if (resolveRes != null && (resolveRes.ToString() == httpRes.ServiceUnavilable.ToString() || resolveRes.ToString() == httpRes.Timedout.ToString()))
                        {
                            writeAudit("REFINITIV ERROR", "ResolveWatchlist - Resolve", "Refinitiv is down. Resolve API response: " + resolveRes.ToString(), "CIFID=" + CIFID);
                        }
                        else
                        {
                            ResolveResponseDto resolveResponse = JsonConvert.DeserializeObject<ResolveResponseDto>(resolveRes.ToString());
                            if (resolveResponse != null && resolveResponse.responseBody != null)
                            {
                                if (resolveResponse.responseBody.ToUpper() == "SUCCESSFUL")
                                {
                                    ok = true;
                                    if (arrProviderType.GetValue(i).ToString().ToUpper().Trim() == "WATCHLIST")
                                    {
                                        watchlist = true;
                                    }
                                }
                                writeAudit("REFINITIV", "ResolveWatchlist", resolveResponse.responseBody, CIFID + "~" + caseSystemId);
                                UpdateResolveWatchlist(caseSystemId, CheckerId);
                            }
                            else
                            {
                                writeAudit("REFINITIV ERROR", "ResolveWatchlist", resolveResponse.replyStatus.message, CIFID + "~" + caseSystemId);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                writeAudit("REFINITIV ERROR", "ResolveWatchlist", ex.Message + "|" + ex.StackTrace, "CIFID=" + CIFID);
            }
            return ok;
        }

        public bool ArchiveDelete(string CaseSystemId, bool ArchiveOnly, string CheckerId)
        {
            bool ok = false;
            try
            {
                string BranchCode = ConfigurationManager.AppSettings["RAPI_REFINITIV_DEFAULT_BRANCH"];
                string identifier = string.Empty;
                DataTable dt = new DataTable();
                string config = string.Empty;
                string branchRegion = getBranchRegion(BranchCode);

                if (branchRegion == string.Empty)
                {
                    writeAudit("REFINITIV ERROR", "ArchiveDelete", "SETUP NOT FOUND FOR " + BranchCode, "");
                }

                dt = getLookup("RefinitivConfiguration");
                if (dt != null && dt.Rows.Count > 0)
                {
                    config = dt.AsEnumerable().Where(x => x.Field<string>("FldCode") == branchRegion).FirstOrDefault().Field<string>("FldDescription");
                    if (config != string.Empty)
                    {
                        identifier = config.Split('|').GetValue(0).ToString();
                    }
                }
                identifier = identifier + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");

                APIArchiveDeleteDto archive = new APIArchiveDeleteDto
                {
                    identifier = identifier,
                    caseSystemIds = CaseSystemId,
                    archiveOnly = ArchiveOnly
                };

                var json = JsonConvert.SerializeObject(archive);
                object archiveRes = ConnectToRefinitiv(json, ConfigurationManager.AppSettings["RAPI_REFINITIV_API_ARCHIVE_DELETE"]);
                if (archiveRes != null && (archiveRes.ToString() == httpRes.ServiceUnavilable.ToString() || archiveRes.ToString() == httpRes.Timedout.ToString()))
                {
                    writeAudit("REFINITIV ERROR", "ArchiveDelete", "Refinitiv is down. Archive API response: " + archiveRes.ToString(), "CaseSystemId=" + CaseSystemId);
                }
                else
                {
                    ArchiveDeleteResponseDto archiveResponse = JsonConvert.DeserializeObject<ArchiveDeleteResponseDto>(archiveRes.ToString());
                    if (archiveResponse != null && archiveResponse.responseBody != null)
                    {
                        foreach (var item in archiveResponse.responseBody)
                        {
                            if (item.caseSystemId != null && item.success.ToUpper() == "TRUE")
                            {
                                ok = true;
                                string log = getLookupValue("RefinitivConfiguration", "FldCode", "Log", "FldDescription");
                                if (log == "1")
                                {
                                    writeAudit("REFINITIV", "ArchiveDelete", item.caseSystemId.ToUpper(), CaseSystemId);
                                }
                            }
                        }
                    }
                    if (ok)
                    {
                        UpdateWatchlist(CaseSystemId, ArchiveOnly, CheckerId);
                    }
                }
            }
            catch (Exception ex)
            {
                writeAudit("REFINITIV ERROR", "ArchiveDelete", ex.Message + "|" + ex.StackTrace, "CaseSystemId=" + CaseSystemId);
            }
            return ok;
        }
    }
}