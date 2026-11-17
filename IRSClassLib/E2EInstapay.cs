using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Net;
using System.IO;
using IRSClassLib;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;

namespace IRSClassLib
{
    public class E2EInstapay
    {
        csAuditLog audit = new csAuditLog();
        public string InstapayTxnamt { get; set; } = "";
        public string InstapayTxndate { get; set; } = "";
        public string InstapayTxntime { get; set; } = "";
        public string InstapayMallid { get; set; } = "";
        public string InstapayMerchantid { get; set; } = "";
        public string InstapayParam1 { get; set; } = "";
        public string InstapayParam2 { get; set; } = "";
        public string InstapayParam3 { get; set; } = "";
        public string InstapayTraceno { get; set; } = "";
        public string InstapayReplyCode { get; set; } = "";
        public string InstapayReplyDescription { get;  set; } = "";
        public string InstapayReplyText { get; set; } = "";
        public string InstapayUserReferenceNumber { get; set; } = "";

        irsClientData clientdata = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];

        public enum InstapayService
        {
            Inquiry,
            FundTransfer
        }

        private HttpWebRequest CreateWebRequest(string url, string instapayRequest)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            Encoding encoding = new UTF8Encoding();
            byte[] data = encoding.GetBytes(instapayRequest);
            webRequest.Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["InstapayTimeout"]);
            webRequest.Method = "POST";
            webRequest.ContentType = "application/json";
            webRequest.Headers["UserName"] = ConfigurationManager.AppSettings["E2EInstapayUser"];
            webRequest.Headers["PassWord"] = ConfigurationManager.AppSettings["E2EInstapayEncryptPassword"];
            webRequest.ContentLength = data.Length;
            return webRequest;
        }

        public string instapayServices(InstapayService instapayService, string instapayRequest)
        {
            HttpWebRequest request = null;
            string ret = string.Empty;
            string res = string.Empty;

            try
            {
                int TLSVersion = Convert.ToInt32(ConfigurationManager.AppSettings["InstapayTLS"]);
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)TLSVersion;

                switch (instapayService)
                {
                    case InstapayService.Inquiry:
                        request = CreateWebRequest(ConfigurationManager.AppSettings["E2EInstapayInquiry"], instapayRequest);
                        break;

                    case InstapayService.FundTransfer:
                        request = CreateWebRequest(ConfigurationManager.AppSettings["E2EInstapayFundTransfer"], instapayRequest);
                        break;
                }

                using (Stream webStream = request.GetRequestStream())
                using (StreamWriter requestWriter = new StreamWriter(webStream, System.Text.Encoding.ASCII))
                {
                    requestWriter.Write(instapayRequest);
                }
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                    {
                        try
                        {
                            ret = rd.ReadToEnd();                          
                        }
                        catch (Exception ex)
                        {
                            res = ex.Message;
                            audit.writeAudit(clientdata.UserId, "instapayServices", "OTC TRANSACTIONS=[Details=" + instapayRequest + "]", "[Error Description=" + res + "]", "", csAuditLog.Category.Update);
                            ret = rd.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
                audit.writeAudit(clientdata.UserId, "instapayServices", "OTC TRANSACTIONS=[Details=" + instapayRequest + "]", "[Error Description=" + res + "]", "", csAuditLog.Category.Update);
            }
            return ret;
        }
        public void parseJsonResponse(string response)
        {
            string res = string.Empty;
            try
            {
                JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
                var result = jsSerializer.DeserializeObject(response);
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict = JsonHelper.DeserializeAndFlatten(response);
                object o;

                dict.TryGetValue("TransactionResp.Status.ReplyCode", out o);
                if (o != null && o.ToString() != string.Empty)
                {
                    this.InstapayReplyCode = o.ToString();
                }
                else { this.InstapayReplyCode = ""; }

                dict.TryGetValue("TransactionResp.Txnamt", out o);
                if (o != null && o.ToString() != string.Empty)
                {
                    this.InstapayTxnamt = o.ToString();
                }
                else { this.InstapayTxnamt = ""; }

                dict.TryGetValue("TransactionResp.Txndate", out o);
                if (o != null && o.ToString() != string.Empty)
                {
                    this.InstapayTxndate = o.ToString();
                }
                else { this.InstapayTxndate = ""; }

                dict.TryGetValue("TransactionResp.Txntime", out o);
                if (o != null && o.ToString() != string.Empty)
                {
                    this.InstapayTxntime = o.ToString();
                }
                else { this.InstapayTxntime = ""; }

                dict.TryGetValue("TransactionResp.Mallid", out o);
                if (o != null && o.ToString() != string.Empty)
                {
                    this.InstapayMallid = o.ToString();
                }
                else { this.InstapayMallid = ""; }

                dict.TryGetValue("TransactionResp.Merchantid", out o);
                if (o != null && o.ToString() != string.Empty)
                {
                    this.InstapayMerchantid = o.ToString();
                }
                else { this.InstapayMerchantid = ""; }

                dict.TryGetValue("TransactionResp.Param1", out o);
                if (o != null && o.ToString() != string.Empty)
                {
                    this.InstapayParam1 = o.ToString();
                }
                else { this.InstapayParam1 = ""; }

                dict.TryGetValue("TransactionResp.Param2", out o);
                if (o != null && o.ToString() != string.Empty)
                {
                    this.InstapayParam2 = o.ToString();
                }
                else { this.InstapayParam2 = ""; }

                dict.TryGetValue("TransactionResp.Param3", out o);
                if (o != null && o.ToString() != string.Empty)
                {
                    this.InstapayParam3 = o.ToString();
                }
                else { this.InstapayParam3 = ""; }

                dict.TryGetValue("TransactionResp.traceno", out o);
                if (o != null && o.ToString() != string.Empty)
                {
                    this.InstapayTraceno = o.ToString();
                }
                else { this.InstapayTraceno = ""; }

                dict.TryGetValue("TransactionResp.Status.ReplyCode", out o);
                if (o != null && o.ToString() != string.Empty)
                {
                    this.InstapayReplyCode = o.ToString();
                }
                else
                {
                    this.InstapayReplyCode = "";
                }

                dict.TryGetValue("TransactionResp.Status.ReplyText", out o);
                if (o != null && o.ToString() != string.Empty)
                {
                    this.InstapayReplyText = o.ToString();
                }
                else
                {
                    this.InstapayReplyText = "";
                }

                dict.TryGetValue("TransactionResp.Status.UserReferenceNumber", out o);
                if (o != null && o.ToString() != string.Empty)
                {
                    this.InstapayUserReferenceNumber = o.ToString();
                }
                else
                {
                    this.InstapayUserReferenceNumber = "";
                }
            }
            catch (Exception ex)
            {
                res = ex.ToString();
                audit.writeAudit(clientdata.UserId, "parseJsonResponse", "OTC TRANSACTIONS=[Details=" + response + "]", "[Error Description=" + res + "]", "", csAuditLog.Category.Update);
            }
        }
    }

    public class JsonHelper
    {
        public static Dictionary<string, object> DeserializeAndFlatten(string json)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            JToken token = JToken.Parse(json);
            FillDictionaryFromJToken(dict, token, "");
            return dict;
        }

        private static void FillDictionaryFromJToken(Dictionary<string, object> dict, JToken token, string prefix)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    foreach (JProperty prop in token.Children<JProperty>())
                    {
                        FillDictionaryFromJToken(dict, prop.Value, Join(prefix, prop.Name));
                    }
                    break;

                case JTokenType.Array:
                    int index = 0;
                    foreach (JToken value in token.Children())
                    {
                        FillDictionaryFromJToken(dict, value, Join(prefix, index.ToString()));
                        index++;
                    }
                    break;

                default:
                    dict.Add(prefix, ((JValue)token).Value);
                    break;
            }
        }

        private static string Join(string prefix, string name)
        {
            return (string.IsNullOrEmpty(prefix) ? name : prefix + "." + name);
        }
    }
}