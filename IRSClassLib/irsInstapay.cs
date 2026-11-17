using System;
using System.Data;
using System.Text;
using System.Net.Http;
using System.Configuration;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace IRSClassLib
{
    [Serializable]
    public class irsInstapay : sqlHelperWs
    {
        public class InstapayRequestDto
        {
            public string identifier { get; set; }
            public int channelCode { get; set; }
            public decimal amount { get; set; }
            public string referenceNo { get; set; }
            public string otherBankBIC { get; set; }
            public string remitterName { get; set; }
            public DateTime remitterDOB { get; set; }
            public string remitterCountry { get; set; }
            public string remitterStreet { get; set; }
            public string remitterBldg { get; set; }
            public string remitterZipCode { get; set; }
            public string remitterTown { get; set; }
            public string remitterCityOfBirth { get; set; }
            public string remitterCountryOfBirth { get; set; }
            public string debitAccount { get; set; }
            public string accountType { get; set; }
            public string beneficiaryName { get; set; }
            public string beneficiaryDOB { get; set; }
            public string beneficiaryCityOfBirth { get; set; }
            public string beneficiaryCountryOfBirth { get; set; }
            public string creditAccount { get; set; }
            public string purpose { get; set; }
            public bool statusRequest { get; set; }
        }

        public class InstapayResponseDto
        {
            public ReplyStatus replyStatus { get; set; }
            public ResponseBody responseBody { get; set; }
        }

        public class ReplyStatus
        {
            public int statusCode { get; set; }
            public string message { get; set; }
            public string userRef { get; set; }
        }

        public class ResponseBody
        {
            public string instructionIdentification { get; set; }
            public DateTime creationDateTime { get; set; }
            public string transactionStatus { get; set; }
            public string reason { get; set; }
        }

        public DataTable getInstapayRequestData(long seqId)
        {
            base.EnforceConstraint = false;
            getUpdateDS("api_getInstapayTxn", new Object[] { seqId });
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

        public string ConnectToInstapayPortal(long seqId)
        {
            string ret = ret = "3~";
            try
            {
                string url = ConfigurationManager.AppSettings["RAPI_IP_URI"];
                string response = string.Empty;
                string json = string.Empty;
                HttpClient client = new HttpClient();
                HttpResponseMessage httpResponse = null;
                string bankCode = string.Empty;
                InstapayRequestDto requestData = new InstapayRequestDto();
                InstapayResponseDto resInstapay = new InstapayResponseDto();
                string instructionIdent = string.Empty;
                string responseStatus = string.Empty;
                string responseMessage = string.Empty;
                bool retry = false;
                bool error = false;
                bool success = false;
                string rapiError = string.Empty;
                DataTable dtIPConfiguration = new irsInternetPage().getLookup("InstapayPortalConfiguration");
                string retryFlag = new irsInternetPage().getLookupValue("InstapayPortalConfiguration", "FldCode", "RETRY", "FldDescription");
                if (retryFlag == "1")
                {
                    retry = true;
                }

                DataTable dtTxnData = getInstapayRequestData(seqId);
                
                if (dtTxnData != null && dtTxnData.Rows.Count > 0)
                {
                    bankCode = dtTxnData.Rows[0]["BankCode"].ToString();
                    requestData = new InstapayRequestDto
                    {
  
                        identifier = dtTxnData.Rows[0]["Identifier"].ToString(),
                        channelCode = Convert.ToInt32(dtTxnData.Rows[0]["ChannelCode"]),
                        amount = Convert.ToDecimal(dtTxnData.Rows[0]["Amount"].ToString()),
                        referenceNo = dtTxnData.Rows[0]["ReferenceNo"].ToString(),
                        otherBankBIC = dtTxnData.Rows[0]["OtherBankBIC"].ToString(),
                        remitterName = dtTxnData.Rows[0]["RemitterName"].ToString(),
                        remitterDOB = Convert.ToDateTime(dtTxnData.Rows[0]["RemitterDOB"]),
                        remitterCountry = dtTxnData.Rows[0]["RemitterCountry"].ToString(),
                        remitterStreet = dtTxnData.Rows[0]["RemitterStreet"].ToString(),
                        remitterBldg = dtTxnData.Rows[0]["RemitterBldg"].ToString(),
                        remitterZipCode = dtTxnData.Rows[0]["RemitterZipCode"].ToString(),
                        remitterTown = dtTxnData.Rows[0]["RemitterTown"].ToString(),
                        remitterCityOfBirth = dtTxnData.Rows[0]["RemitterCityOfBirth"].ToString(),
                        remitterCountryOfBirth = dtTxnData.Rows[0]["RemitterCountryOfBirth"].ToString(),
                        debitAccount = dtTxnData.Rows[0]["DebitAccount"].ToString(),
                        accountType = dtTxnData.Rows[0]["AccountType"].ToString(),
                        beneficiaryName = dtTxnData.Rows[0]["BeneficiaryName"].ToString(),
                        beneficiaryDOB = dtTxnData.Rows[0]["BeneficiaryDOB"].ToString(),
                        beneficiaryCityOfBirth = dtTxnData.Rows[0]["BeneficiaryCityOfBirth"].ToString(),
                        beneficiaryCountryOfBirth = dtTxnData.Rows[0]["BeneficiaryCountryOfBirth"].ToString(),
                        creditAccount = dtTxnData.Rows[0]["CreditAccount"].ToString(),
                        purpose = dtTxnData.Rows[0]["Purpose"].ToString(),
                        statusRequest = retry

                    };
                    json = JsonConvert.SerializeObject(requestData);
                    json = System.Text.RegularExpressions.Regex.Unescape(json);
                }
                else
                {
                    return ret;
                }

                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("UserID", ConfigurationManager.AppSettings["RAPI_IP_User"].ToString());
                client.DefaultRequestHeaders.Add("Password", ConfigurationManager.AppSettings["RAPI_IP_Pass"].ToString());
                client.Timeout = TimeSpan.FromMilliseconds(Convert.ToInt32(ConfigurationManager.AppSettings["RAPI_IP_Timeout"]));

                try
                {
                    httpResponse = client.PostAsync(url, data).Result;
                    byte[] byteArray = httpResponse.Content.ReadAsByteArrayAsync().Result;
                    response = Encoding.UTF8.GetString(byteArray);
                    error = true;
                    success = false;
                    resInstapay = JsonConvert.DeserializeObject<InstapayResponseDto>(response);
                }
                catch (Exception ex)
                {
                    rapiError = ex.Message;
                    new csAuditLog().writeAudit("", "Instapay Error", "Instapay Return Message", seqId.ToString(), ex.Message + "|" + ex.StackTrace, csAuditLog.Category.Select);
                }

                try
                {
                    error = false;
                    instructionIdent = (resInstapay.responseBody != null ? resInstapay.responseBody.instructionIdentification : "");
                    responseStatus = (resInstapay.responseBody != null ? resInstapay.responseBody.transactionStatus : "");
                    responseMessage = (resInstapay.responseBody != null ? resInstapay.responseBody.reason : rapiError);

                    string successCode = new irsInternetPage().getLookupValue("InstapayPortalConfiguration", "FldCode", "SUCCESS", "FldDescription");
                    if (rapiError == string.Empty && successCode.Contains(resInstapay.responseBody.transactionStatus.ToUpper()))
                    {
                        //Success
                        ret = "1~" + instructionIdent + "~" + responseStatus + "~" + responseMessage;
                        success = true;
                    }
                    else
                    {
                        string rejectCode = new irsInternetPage().getLookupValue("InstapayPortalConfiguration", "FldCode", "REJECT", "FldDescription");
                        if (rapiError == string.Empty && rejectCode.Contains(resInstapay.responseBody.transactionStatus.ToUpper()))
                        {
                            //Reject
                            ret = "2~" + instructionIdent + "~" + responseStatus + "~" + responseMessage;
                        }
                        else
                        {
                            DataTable dtLookup = new irsInternetPage().getLookup("InstapayISO20022RejectCodes");
                            int i = 0;
                            bool found = false;
                            //Timeout
                            ret = "3~" + instructionIdent + "~" + responseStatus + "~" + responseMessage;
                            if (rapiError == string.Empty && dtLookup != null && dtLookup.Rows.Count > 0)
                            {
                                for (i = 0; i <= (dtLookup.Rows.Count - 1); i++)
                                {
                                    if (resInstapay.responseBody.transactionStatus.ToUpper() == dtLookup.Rows[i]["FldCode"].ToString().ToUpper())
                                    {
                                        found = true;
                                        break;
                                    }
                                }
                                if (found)
                                {
                                    //Reject
                                    ret = "2~" + instructionIdent + "~" + responseStatus + "~" + responseMessage;
                                }
                                else if (rejectCode.Contains(resInstapay.responseBody.transactionStatus.ToUpper()))
                                {
                                    //Reject
                                    ret = "2~" + instructionIdent + "~" + responseStatus + "~" + responseMessage;
                                }
                                else
                                {
                                    ret = "3~" + instructionIdent + "~" + responseStatus + "~" + responseMessage;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    new csAuditLog().writeAudit("", "Instapay Return Error", "Connect to Instapay RAPI", seqId.ToString() + "~" + ret, base.Message + "~" + ex.Message + "~" + ex.StackTrace, csAuditLog.Category.Select);
                }

                if (error)
                {
                    if (resInstapay.replyStatus != null)
                    {
                        List<string> lstIdent = resInstapay.replyStatus.message.Split('~').ToList();
                        if (lstIdent.Count > 3)
                        {
                            instructionIdent = lstIdent[2].ToString();
                        }
                    }
                    else
                    {
                        responseMessage = response;
                    }
                }

                bool ok = false;
                string channelCode = new irsInternetPage().getLookupValue("InstapayPortalChannelCode", "FldCode", requestData.channelCode.ToString(), "FldDescription");
                base.addMode = true;
                this.setBase("SELECT TOP 1 * FROM tbl_Instapay_API_Log", true);
                setColumn("TXN_SEQ_ID", seqId);
                setColumn("ReferenceNo", requestData.referenceNo);
                setColumn("ChannelCode", channelCode);
                setColumn("DebitAccount", requestData.debitAccount);
                setColumn("CreditAccount", requestData.creditAccount);
                setColumn("BankCode", bankCode);
                setColumn("BIC", requestData.otherBankBIC);
                setColumn("Amount", requestData.amount);
                setColumn("RequestAPI", json);
                setColumn("ResponseAPI", response);
                setColumn("InstructionIdent", instructionIdent);
                setColumn("ResponseStatus", responseStatus);
                setColumn("ResponseMessage", responseMessage);
                setColumn("Success", success);
                setColumn("DateCreated", DateTime.Now);
                ok = base.save(sqlHelperWs.RETAIN_DATASET);
                if (!ok)
                {
                    new csAuditLog().writeAudit("", "Instapay Log Error", "Save Instapay Transaction to Logs", seqId.ToString(), base.Message, csAuditLog.Category.Insert);
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "Instapay Error", "Send Transaction to Instapay Portal", seqId.ToString(), ex.Message + "|" + ex.StackTrace, csAuditLog.Category.Select);
            }
            return ret;
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
    }
}