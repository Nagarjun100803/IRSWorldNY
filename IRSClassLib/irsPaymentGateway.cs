using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Net;
using System.IO;
using System.Data;
using com.wiz.enets2.transaction.umapi;
using com.wiz.enets2.transaction.umapi.data;

namespace IRSClassLib
{
    public class irsPaymentGateway : sqlHelperWs
    {
        public string createRequest(string Region, string BranchCode, string CIFID, string CardNo, string CardName, string ExpiryDate, string SecurityCode, string CardType, string CurrencyCode, string Amount, string Remarks, string CallBackPage)
        {
            string sReturn = string.Empty;
            string ORNo = string.Empty;
            string urlS = string.Empty;
            DataTable dt;
            DataTable dtRemitter;
            DataTable dtCardDate;
            DataTable dtPaymentGateway;
            DataRow dr;
            int i = 0;
            irsClientData cd = new irsClientData();

            ORNo = generateReference(BranchCode);
            dtRemitter = getRemitterInfo(CIFID);
            dtPaymentGateway = getPaymentGateway(Region, new irsInternetPage().getCountry(BranchCode));
            dtCardDate = getCardDate(CIFID, CardNo);
            CardType = checkCardType(CardType);            

            if (dtRemitter != null && dtPaymentGateway != null)
            {
                switch (Region)
                {
                    case "2": //Europe
                        try
                        {
                            System.Web.HttpContext.Current.Session["3DSecureURL"] = dtPaymentGateway.Rows[0]["SecureURL"].ToString().Trim();

                            dt = new DataTable();
                            dt.Columns.Add("VPSProtocol");
                            dt.Columns.Add("TxType");
                            dt.Columns.Add("Vendor");
                            dt.Columns.Add("VendorTxCode");
                            dt.Columns.Add("Amount");
                            dt.Columns.Add("Currency");
                            dt.Columns.Add("Description");
                            dt.Columns.Add("CardHolder");
                            dt.Columns.Add("CardNumber");
                            dt.Columns.Add("StartDate");
                            dt.Columns.Add("ExpiryDate");
                            dt.Columns.Add("IssueNumber");
                            dt.Columns.Add("CV2");
                            dt.Columns.Add("CardType");
                            dt.Columns.Add("BillingSurName");
                            dt.Columns.Add("BillingFirstnames");
                            dt.Columns.Add("BillingAddress1");
                            dt.Columns.Add("BillingAddress2");
                            dt.Columns.Add("BillingCity");
                            dt.Columns.Add("BillingPostCode");
                            dt.Columns.Add("BillingCountry");
                            dt.Columns.Add("BillingState");
                            dt.Columns.Add("BillingPhone");
                            dt.Columns.Add("DeliverySurname");
                            dt.Columns.Add("DeliveryFirstnames");
                            dt.Columns.Add("DeliveryAddress1");
                            dt.Columns.Add("DeliveryAddress2");
                            dt.Columns.Add("DeliveryCity");
                            dt.Columns.Add("DeliveryPostCode");
                            dt.Columns.Add("DeliveryCountry");
                            dt.Columns.Add("DeliveryState");
                            dt.Columns.Add("DeliveryPhone");
                            dt.Columns.Add("PayPalCallbackURL");
                            dt.Columns.Add("CustomerEMail");
                            dt.Columns.Add("Basket");
                            dt.Columns.Add("GiftAidPayment");
                            dt.Columns.Add("ApplyAVSCV2");
                            dt.Columns.Add("ClientIPAddress");
                            dt.Columns.Add("Apply3DSecure");
                            dt.Columns.Add("AccountType");

                            dr = dt.NewRow();
                            dr[0] = trimValue(dtPaymentGateway.Rows[0]["VPSProtocol"].ToString().Trim(), 4);
                            dr[1] = trimValue(dtPaymentGateway.Rows[0]["TxTypeDesc"].ToString().Trim(), 15);
                            dr[2] = trimValue(dtPaymentGateway.Rows[0]["Vendor"].ToString().Trim(), 15);
                            dr[3] = trimValue(ORNo, 40);
                            dr[4] = Amount;
                            dr[5] = trimValue(CurrencyCode, 3); //EUR
                            dr[6] = trimValue(Remarks, 100);
                            dr[7] = trimValue(CardName, 50); //TEST PROTX EURO
                            dr[8] = trimValue(CardNo, 20); //4929000000006
                            dr[9] = trimValue(dtCardDate.Rows[0]["IssueDate"].ToString(), 4);
                            dr[10] = trimValue(dtCardDate.Rows[0]["ExpiryDate"].ToString(), 4); //0510
                            dr[11] = trimValue(dtCardDate.Rows[0]["IssueNumber"].ToString(), 2);
                            dr[12] = trimValue(SecurityCode, 4); //123
                            dr[13] = trimValue(CardType, 15); //VISA
                            dr[14] = trimValue(dtRemitter.Rows[0]["LastName"].ToString(), 20);
                            dr[15] = trimValue(dtRemitter.Rows[0]["FirstName"].ToString(), 20);
                            dr[16] = trimValue(dtRemitter.Rows[0]["Address1"].ToString(), 100);
                            if (dtRemitter.Rows[0]["Address2"].ToString().Trim() == string.Empty)
                            {
                                dr[17] = trimValue(dtRemitter.Rows[0]["Address1"].ToString(), 100);
                            }
                            else
                            {
                                dr[17] = trimValue(dtRemitter.Rows[0]["Address2"].ToString(), 100);
                            }
                            dr[18] = trimValue((dtRemitter.Rows[0]["City"].ToString().Trim() == "") ? "n/a" : dtRemitter.Rows[0]["City"].ToString().Trim(), 40);
                            dr[19] = trimValue(dtRemitter.Rows[0]["PostCode"].ToString(), 10);
                            dr[20] = trimValue(dtRemitter.Rows[0]["CountryCode"].ToString(), 2);
                            dr[21] = trimValue(dtRemitter.Rows[0]["PostalCode"].ToString(), 2);
                            dr[22] = trimValue(dtRemitter.Rows[0]["BillingPhone"].ToString(), 20);
                            dr[23] = trimValue(dtRemitter.Rows[0]["LastName"].ToString(), 20);
                            dr[24] = trimValue(dtRemitter.Rows[0]["FirstName"].ToString(), 20);
                            dr[25] = trimValue(dtRemitter.Rows[0]["Address1"].ToString(), 100);
                            if (dtRemitter.Rows[0]["Address2"].ToString().Trim() == string.Empty)
                            {
                                dr[26] = trimValue(dtRemitter.Rows[0]["Address1"].ToString(), 100);
                            }
                            else
                            {
                                dr[26] = trimValue(dtRemitter.Rows[0]["Address2"].ToString(), 100);
                            }
                            dr[27] = trimValue((dtRemitter.Rows[0]["City"].ToString().Trim() == "") ? "n/a" : dtRemitter.Rows[0]["City"].ToString().Trim(), 40);
                            dr[28] = trimValue(dtRemitter.Rows[0]["PostCode"].ToString(), 10);
                            dr[29] = trimValue(dtRemitter.Rows[0]["CountryCode"].ToString(), 2);
                            dr[30] = trimValue(dtRemitter.Rows[0]["PostalCode"].ToString(), 2);
                            dr[31] = trimValue(dtRemitter.Rows[0]["BillingPhone"].ToString(), 20);
                            dr[32] = trimValue("", 255);
                            dr[33] = trimValue(dtRemitter.Rows[0]["EmailAddress"].ToString(), 255);
                            dr[34] = trimValue("", 7500);
                            dr[35] = trimValue((dtPaymentGateway.Rows[0]["GiftAidPayment"].ToString().Trim() == "true") ? "1" : "0", 1);
                            dr[36] = trimValue((dtPaymentGateway.Rows[0]["ApplyAVSCV2"].ToString().Trim() == "true") ? "1" : "0", 1);
                            dr[37] = trimValue(cd._ipaddr, 15);
                            dr[38] = trimValue((dtPaymentGateway.Rows[0]["Apply3DSecure"].ToString().Trim() == "true") ? "1" : "0", 1);
                            dr[39] = trimValue(dtPaymentGateway.Rows[0]["AccountTypeDesc"].ToString().Trim(), 1);
                            dt.Rows.Add(dr);

                            for (i = 0; i <= (dt.Columns.Count - 1); i++)
                            {
                                sReturn += dt.Columns[i].ToString() + "=" + dt.Rows[0][i].ToString() + "&";
                            }
                            sReturn = sReturn.Substring(0, sReturn.Length - 1);
                        }
                        catch (Exception ex)
                        {
                            Message = ex.Message;
                            new csAuditLog().writeAudit(ClientData._userid, "createRequest", "sReturn", ex.Message, sReturn, csAuditLog.Category.PaymentGateway);
                        }
                        break;

                    case "3": //USA and Canada
                        try
                        {
                            dt = new DataTable();
                            dt.Columns.Add("x_login");
                            dt.Columns.Add("x_tran_key");
                            dt.Columns.Add("x_version");
                            dt.Columns.Add("x_test_request");
                            dt.Columns.Add("x_delim_data");
                            dt.Columns.Add("x_delim_char");
                            dt.Columns.Add("x_relay_response");
                            dt.Columns.Add("x_first_name");
                            dt.Columns.Add("x_last_name");
                            dt.Columns.Add("x_address");
                            dt.Columns.Add("x_city");
                            dt.Columns.Add("x_state");
                            dt.Columns.Add("x_zip");
                            dt.Columns.Add("x_country");
                            dt.Columns.Add("x_email");
                            dt.Columns.Add("x_email_customer");
                            dt.Columns.Add("x_merchant_email");
                            dt.Columns.Add("x_amount");
                            dt.Columns.Add("x_currency_code");
                            dt.Columns.Add("x_method");
                            dt.Columns.Add("x_type");
                            dt.Columns.Add("x_card_num");
                            dt.Columns.Add("x_exp_date");
                            dt.Columns.Add("x_card_code");

                            dr = dt.NewRow();
                            dr[0] = trimValue(dtPaymentGateway.Rows[0]["x_login"].ToString().Trim(), 20); //"8En4VaAd9UR";
                            dr[1] = trimValue(dtPaymentGateway.Rows[0]["x_tran_key"].ToString().Trim(), 16); //"83RMQYxx43av92Q7"; 
                            dr[2] = trimValue(dtPaymentGateway.Rows[0]["x_version"].ToString().Trim(), 4);
                            dr[3] = trimValue(dtPaymentGateway.Rows[0]["x_test_request"].ToString().Trim(), 5); //TRUE = Test Environment, FALSE = Prod Environment
                            dr[4] = trimValue(dtPaymentGateway.Rows[0]["x_delim_data"].ToString().Trim(), 5);
                            dr[5] = trimValue(dtPaymentGateway.Rows[0]["x_delim_char"].ToString().Trim(), 20);
                            dr[6] = trimValue(dtPaymentGateway.Rows[0]["x_relay_response"].ToString().Trim(), 5);
                            dr[7] = trimValue(dtRemitter.Rows[0]["FirstName"].ToString(), 50);
                            dr[8] = trimValue(dtRemitter.Rows[0]["LastName"].ToString(), 50);
                            dr[9] = trimValue(dtRemitter.Rows[0]["Address1"].ToString(), 60);
                            dr[10] = trimValue(dtRemitter.Rows[0]["City"].ToString(), 40);
                            dr[11] = trimValue("", 40);
                            dr[12] = trimValue(dtRemitter.Rows[0]["PostCode"].ToString(), 20);
                            dr[13] = trimValue("", 60);
                            dr[14] = trimValue(dtRemitter.Rows[0]["EmailAddress"].ToString(), 255);
                            dr[15] = trimValue(dtPaymentGateway.Rows[0]["x_email_customer"].ToString().Trim(), 5);
                            dr[16] = trimValue(dtPaymentGateway.Rows[0]["merchant_email"].ToString().Trim(), 60);
                            dr[17] = Amount;
                            dr[18] = CurrencyCode; //USD
                            dr[19] = trimValue(dtPaymentGateway.Rows[0]["XMethodDesc"].ToString().Trim(), 60);
                            dr[20] = trimValue(dtPaymentGateway.Rows[0]["XTypeDesc"].ToString().Trim(), 60);
                            dr[21] = CardNo; //4111111111111111
                            dr[22] = trimValue(dtCardDate.Rows[0]["ExpiryDate"].ToString(), 4); //0115
                            dr[23] = SecurityCode; //SecurityCode
                            dt.Rows.Add(dr);

                            for (i = 0; i <= (dt.Columns.Count - 1); i++)
                            {
                                sReturn += dt.Columns[i].ToString() + "=" + dt.Rows[0][i].ToString() + "&";
                            }
                            sReturn = sReturn.Substring(0, sReturn.Length - 1);
                        }
                        catch (Exception ex)
                        {
                            Message = ex.Message;
                            new csAuditLog().writeAudit(ClientData._userid, "createRequest", "sReturn", ex.Message, sReturn, csAuditLog.Category.PaymentGateway);
                        }
                        break;
                    case "1":
                        switch (BranchCode)
                        {
                            case "760":
                                try
                                {
                                    DebitMerchant myDebitMerchant = null;
                                    TxnReq myTxnReq = null;
                                    DebitTxnReq myDebitTxnReq = null;
                                    myDebitMerchant = (DebitMerchant)Merchant.getInstance(Merchant.MERCHANT_TYPE_DEBIT, new irsConfiguration().appsetting("ENETS_Path_Config"));

                                    try
                                    {
                                        urlS = NETSConfig.get("payURL");
                                    }
                                    catch (Exception netsConfigE)
                                    { new csAuditLog().writeAudit(ClientData._userid,"createRequest","NETSConfig.get",netsConfigE.Message,"", csAuditLog.Category.Select);}

                                    myTxnReq = new TxnReq();
                                    myTxnReq.setNetsMid(dtPaymentGateway.Rows[0]["x_tran_key"].ToString().Trim());
                                    myTxnReq.setSubmissionMode(dtPaymentGateway.Rows[0]["x_version"].ToString().Trim());
                                    myTxnReq.setTxnAmount(Amount.Replace(",","").Replace(".",""));
                                    myTxnReq.setMerchantTxnRef(ORNo);
                                    myTxnReq.setMerchantCertId(dtPaymentGateway.Rows[0]["x_login"].ToString().Trim());
                                    myTxnReq.setPaymentMode(new irsOBPSTransactions().get_tbl_lookup_description("Authorized_Net_Method",dtPaymentGateway.Rows[0]["x_method"].ToString().Trim()));
                                    myTxnReq.setTid("");
                                    myTxnReq.setCurrencyCode(CurrencyCode);
                                    //myTxnReq.setMerchantTxnDtm(Request.Form["merchant_txn_dtm"]);
                                    myTxnReq.setSuccessUrl(CallBackPage + "Remittance/EnetsSuccess.aspx");
                                    myTxnReq.setSuccessUrlParams("");
                                    myTxnReq.setFailureUrl(CallBackPage + "Remittance/EnetsFailed.aspx");
                                    myTxnReq.setFailureUrlParams("");
                                    myTxnReq.setNotifyUrl("");
                                    myTxnReq.setNotifyUrlParams("");
                                    
                                    myDebitTxnReq = new DebitTxnReq();
                                    myDebitTxnReq.setMerchantTimeZone("+8:00");
                                    myDebitTxnReq.setMerchantTxnDate(new irsTimeZone().branchLocalTime(BranchCode).ToString("dd/MM/yyyy"));
                                    myDebitTxnReq.setMerchantTxnTime(new irsTimeZone().branchLocalTime(BranchCode).ToString("HH:mm:ss"));
                                    myDebitTxnReq.setParam1("");
                                    myDebitTxnReq.setParam2("");
                                    myDebitTxnReq.setParam3("");
                                    myDebitTxnReq.setParam4("");
                                    myDebitTxnReq.setParam5("");
                                    
                                    myTxnReq.setDebitTxnReq(myDebitTxnReq);
                                    sReturn = "ENETS|" + myDebitMerchant.formPayReq(myTxnReq) + "|" + urlS;
                                }
                                catch (Exception ex)
                                {
                                    new csAuditLog().writeAudit(ClientData._userid, "createRequest", "sReturn", ex.Message, sReturn, csAuditLog.Category.PaymentGateway);
                                }
                                return sReturn;
                        }
                        break;
                }
            }
            else
            {
                sReturn = "||NO PAYMENT GATEWAY SETUP. PLEASE REPORT TO THE NEAREST PNB BRANCH AND/OR SYSTEM ADMINISTRATOR.|";
                new csAuditLog().writeAudit(ClientData._userid, "createRequest Error", "sReturn", sReturn, "", csAuditLog.Category.PaymentGateway);
                return sReturn;
            }

            if (sReturn != string.Empty)
            {
                try
                {
                    new csAuditLog().writeAudit(ClientData._userid, "PaymentGateway Request", "[BranchCode=" + BranchCode + "][CIFID=" + CIFID + "][CardNo=" + CardNo + "][CardName=" + CardName + "][CardType=" + CardType + "][ExpiryDate=" + ExpiryDate + "][CurrencyCode=" + CurrencyCode + "][Amount=" + Amount + "][Remarks=" + Remarks + "]", "createRequest", sReturn, csAuditLog.Category.PaymentGateway);
                }
                catch (Exception ex)
                {
                    Message = ex.Message;
                }
                return sendRequest(Region, sReturn, dtPaymentGateway.Rows[0]["URL"].ToString().Trim(), CallBackPage);
            }
            else
            {
                sReturn = "||ERROR WHILE CREATING REQUEST TO PAYMENT GATEWAY.|";
                new csAuditLog().writeAudit(ClientData._userid, "createRequest Error", "sReturn", sReturn, "", csAuditLog.Category.PaymentGateway);
                return sReturn;
            }
        }

        private string checkCardType(string CardType)
        {
            DataTable dtLookup = new irsInternetPage().getLookup("CardType");
            string sReturn = CardType;
            int i = 0;

            for (i = 0; i <= (dtLookup.Rows.Count - 1); i++)
            {
                if (dtLookup.Rows[i]["FldDescription"].ToString().Trim() == CardType.Trim())
                {
                    sReturn = dtLookup.Rows[i]["FldCode"].ToString().Trim();
                    break;
                }
            }
            return sReturn;
        }

        private string trimValue(string Value, int Length)
        {
            int i = 0;
            string sTrim = string.Empty;
            string sReturn = string.Empty;

            for (i = 0; i <= (Value.Length - 1); i++)
            {
                sTrim = Value.Substring(i, 1);
                if (i < Length)
                {
                    sReturn += sTrim;
                }
                else
                {
                    break;
                }
            }
            return sReturn;
        }

        private DataTable getRemitterInfo(string CIFID)
        {
            try
            {
                getUpdateDS("ksp_getRemitterInfo", new Object[] { CIFID });
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
            catch
            {
                return null;
            }
        }

        private DataTable getPaymentGateway(string Region)
        {
            try
            {
                getUpdateDS("ksp_getPaymentGateway", new Object[] { Region, "0" });
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
            catch
            {
                return null;
            }
        }

        private DataTable getPaymentGateway(string Region, string Country)
        {
            try
            {
                getUpdateDS("ksp_getPaymentGateway", new Object[] { Region, Country });
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
            catch
            {
                return null;
            }
        }

        private DataTable getCardDate(string CIFID, string CardNo)
        {
            try
            {
                getUpdateDS("ksp_getCardDate", new Object[] { CIFID, CardNo });
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
            catch
            {
                return null;
            }
        }

        public string generateReference(string BranchCode)
        {
            string sORNo = BranchCode + DateTime.Now.ToString("yyyyMMddHHmmssff");
            System.Web.HttpContext.Current.Session["ORNo"] = sORNo;
            return sORNo;
        }

        private string sendRequest(string Region, string Request, string URL, string CallBackPage)
        {
            string sReturn = string.Empty;
            string sURL = URL;

            try
            {
                HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(sURL);
                objRequest.Method = "POST";
                objRequest.ContentLength = Request.Length;
                objRequest.ContentType = "application/x-www-form-urlencoded";
                objRequest.Timeout = 30000;
                objRequest.KeepAlive = false;

                StreamWriter sw = new StreamWriter(objRequest.GetRequestStream());
                sw.Write(Request);
                sw.Close();

                HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
                StreamReader sr = new StreamReader(objResponse.GetResponseStream());
                string sResponse = sr.ReadToEnd();
                return InterpretResponse(Region, sResponse, CallBackPage);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string send3DRequest(string Request)
        {
            string sURL = System.Web.HttpContext.Current.Session["3DSecureURL"].ToString(); //https://test.sagepay.com/gateway/service/direct3dcallback.vsp

            try
            {
                HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(sURL);
                objRequest.Method = "POST";
                objRequest.ContentLength = Request.Length;
                objRequest.ContentType = "application/x-www-form-urlencoded";
                objRequest.Timeout = 30000;
                objRequest.KeepAlive = false;

                StreamWriter sw = new StreamWriter(objRequest.GetRequestStream());
                sw.Write(Request);
                sw.Close();

                HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
                StreamReader sr = new StreamReader(objResponse.GetResponseStream());
                string sResponse = sr.ReadToEnd();
                return Interpret3DResponse(sResponse);
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        private string Interpret3DResponse(string Message)
        {
            string sReturn = string.Empty;
            string sMessage = Message;
            string sFind = string.Empty;
            string sStatus = string.Empty;
            string sAuthCode = string.Empty;
            string sStatusDetail = string.Empty;

            sMessage = sMessage.Replace("\r\n", "•");
            sFind = "Status=";
            sStatus = GetResponse(sMessage, sMessage.IndexOf(sFind), sFind.Length);
            sFind = "TxAuthNo=";
            sAuthCode = GetResponse(sMessage, sMessage.IndexOf(sFind), sFind.Length);
            sFind = "StatusDetail=";
            sStatusDetail = GetResponse(sMessage, sMessage.IndexOf(sFind), sFind.Length);
            if (System.Web.HttpContext.Current.Session["3DSecureURL"] != null)
            {
                sReturn = sStatus + "|" + sAuthCode + "|" + sStatusDetail + "|" + Message + "|" + System.Web.HttpContext.Current.Session["3DSecureURL"].ToString();
            }
            else
            {
                sReturn = sStatus + "|" + sAuthCode + "|" + sStatusDetail + "|" + Message;
            }
            return sReturn;
        }

        private string InterpretResponse(string Region, string Message, string CallBackPage)
        {
            string sReturn = string.Empty;
            string sStatus = string.Empty;
            string sStatusDetail = string.Empty;
            string sArray = string.Empty;
            string sAuthCode = string.Empty;
            string sURL = string.Empty;
            string sPareq = string.Empty;
            string sMD = string.Empty;
            string sReturnSite = CallBackPage + "Internet/Internet3DCallback.aspx";
            string sMessage = Message;
            string sFind = string.Empty;
            string[] arrReturn;
            int i = 0;

            arrReturn = Message.Split('|');
            for (i = 0; i <= (arrReturn.Length - 1); i++)
            {
                sArray += arrReturn.GetValue(i).ToString() + " ";
            }

            switch (Region)
            { 
                case "2": //Europe
                    sMessage = sMessage.Replace("\r\n", "•");
                    sFind = "Status=";
                    sStatus = GetResponse(sMessage, sMessage.IndexOf(sFind), sFind.Length);
                    if (sStatus.IndexOf("3DAUTH") != -1)
                    {
                        //sReturnSite += "?";

                        sFind = "ACSURL=";
                        sURL = GetResponse(sMessage, sMessage.IndexOf(sFind), sFind.Length);

                        sFind = "PAReq=";
                        sPareq = GetResponse(sMessage, sMessage.IndexOf(sFind), sFind.Length);

                        sFind = "MD=";
                        sMD = GetResponse(sMessage, sMessage.IndexOf(sFind), sFind.Length);

                        StringBuilder sb = new StringBuilder();
                        //sb.Append("<SCRIPT LANGUAGE='Javascript'>");
                        //sb.Append("function OnLoadEvent()");
                        //sb.Append("{ document.form.submit(); }");
                        //sb.Append("</SCRIPT>");
                        //sb.Append("<html><head><title>3D Secure Verification</title></head>");
                        //sb.Append("<body OnLoad='OnLoadEvent();'>");
                        //sb.Append("<FORM name='form' method='POST' action='" + sURL + "'>");
                        sb.Append("<input type='hidden' name='PaReq' value='" + sPareq + "'/>");
                        sb.Append("<input type='hidden' name='TermUrl' value='" + sReturnSite + "'/>");
                        sb.Append("<input type='hidden' name='MD' value='" + sMD + "'/>");
                        //sb.Append("<NOSCRIPT></NOSCRIPT>");
                        //sb.Append("</form>");
                        //sb.Append("</body></html>");
                        
                        sReturn = sb.ToString();
                        sReturn = "3DSECURE" + "|" + sReturn + "|" + sURL;
                        return sReturn;
                    }
                    else
                    {
                        sFind = "TxAuthNo=";
                        sAuthCode = GetResponse(sMessage, sMessage.IndexOf(sFind), sFind.Length);
                    }
                    sFind = "StatusDetail=";
                    sStatusDetail = GetResponse(sMessage, sMessage.IndexOf(sFind), sFind.Length);

                    sReturn = sStatus + "|" + sAuthCode + "|" + sStatusDetail + "|" + Message;
                    break;

                case "3": //USA and Canada
                    if (arrReturn.GetValue(3).ToString().Contains("approved"))
                    {
                        sReturn = "OK" + "|" + arrReturn.GetValue(4) + "|" + arrReturn.GetValue(3) + "|" + sArray;
                    }
                    else
                    {
                        sReturn = arrReturn.GetValue(3).ToString() + "|" + arrReturn.GetValue(3).ToString() + "|" + arrReturn.GetValue(3).ToString() + "|";
                    }
                    break;
            }
            return sReturn;
        }

        private string GetResponse(string Message, int Index, int iField)
        {
            int i = 0;
            string sReturn = string.Empty;
            string sValue = string.Empty;

            for (i = (Index + iField); i <= (Message.Length - 1); i++)
            {
                sValue = Message.Substring(i, 1);
                if (sValue != "•")
                {
                    sReturn += sValue;
                }
                else
                {
                    break;
                }
            }
            return sReturn;
        }

        public void CreateDirectory(string Path)
        {
            try
            {
                if (!(Directory.Exists(Path)))
                {
                    DirectoryInfo di = Directory.CreateDirectory(Path);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GenerateFile()
        {
            string sYear = DateTime.Now.Year.ToString();
            string sMonth = DateTime.Now.Month.ToString();
            string sDay = DateTime.Now.Day.ToString();
            string sHour = DateTime.Now.Hour.ToString();
            string sMinute = DateTime.Now.Minute.ToString();
            string sSecond = DateTime.Now.Second.ToString();
            string sMillisecond = DateTime.Now.Millisecond.ToString();

            return sYear + sMonth + sDay + sHour + sMinute + sSecond + sMillisecond;
        }

        public string GetENETSResponse(string response)
        {
            string sReturn = String.Empty;
            try
            {
                //region prepare message
                string messageS = response;
                //endregion

                //region prepare umapi objects
                DebitMerchant myDebitMerchant = null;
                TxnRes myTxnRes = null;
                DebitTxnRes myDebitTxnRes = null;
                myDebitMerchant = (DebitMerchant)Merchant.getInstance(Merchant.MERCHANT_TYPE_DEBIT, "C:\\UMAPI .NET API\\etc");


                //region unpackResponse
                myTxnRes = myDebitMerchant.unpackResponse(messageS);

                myDebitTxnRes = myTxnRes.getDebitTxnRes();
                sReturn = myTxnRes.getNetsMid() + "|" + myTxnRes.getMerchantTxnRef()
                    + "|" + myTxnRes.getNetsTxnRef() + "|" + myTxnRes.getNetsTxnDtm() + "|"
                    + myTxnRes.getPaymentMode() + "|" + myTxnRes.getNetsTxnStatus() + "|"
                    + myTxnRes.getNetsTxnRespCode() + "|" + myTxnRes.getNetsTxnMsg() + "|"
                    + myDebitTxnRes.getBankId() + "|" + myDebitTxnRes.getBankRefCode() + "|"
                    + myDebitTxnRes.getBankRemarks() + "|" + myDebitTxnRes.getBankStatus() + "|"
                    + myDebitTxnRes.getBankTimeZone() + "|" + myDebitTxnRes.getBankTxnDate() + "|"
                    + myDebitTxnRes.getBankTxnTime() + "|" + myDebitTxnRes.getFuncDestExchangeRate() + "|"
                    + myDebitTxnRes.getGwTimeZone() + "|" + myDebitTxnRes.getGwTxnDate() + "|"
                    + myDebitTxnRes.getGwTxnTime() + "|" + myDebitTxnRes.getGwValueDate() + "|"
                    + myDebitTxnRes.getSrcFuncExchangeRate() + "|" + myDebitTxnRes.getTxnDestAmt() + "|"
                    + myDebitTxnRes.getTxnDestCurrencyCode() + "|" + myDebitTxnRes.getTxnFunCurrencyCode() + "|"
                    + myDebitTxnRes.getTxnFuncAmt() + "|" + myDebitTxnRes.getTxnSrcAmt() + "|"
                    + myDebitTxnRes.getTxnSrcCurrencyCode() + "|" + myDebitTxnRes.getRetry() + "|"
                    + myDebitTxnRes.getVersion() + "|" + myDebitTxnRes.getParam1() + "|"
                    + myDebitTxnRes.getParam2() + "|" + myDebitTxnRes.getParam3() + "|"
                    + myDebitTxnRes.getParam4() + "|" + myDebitTxnRes.getParam5() + "|"
                    + "|" + myDebitTxnRes.getParam11() + "|"
                    + myDebitTxnRes.getParam12() + "|" + myDebitTxnRes.getParam13() + "|"
                    + myDebitTxnRes.getParam14() + "|" + myDebitTxnRes.getParam15();
                //nets_mid.Text = myTxnRes.getNetsMid();
                //merchant_txn_ref.Text = myTxnRes.getMerchantTxnRef();
                //nets_txn_ref.Text = myTxnRes.getNetsTxnRef();
                //nets_txn_dtm.Text = myTxnRes.getNetsTxnDtm();
                //payment_mode.Text = myTxnRes.getPaymentMode();
                //nets_txn_status.Text = myTxnRes.getNetsTxnStatus();
                //nets_txn_resp_code.Text = myTxnRes.getNetsTxnRespCode();
                //nets_txn_msg.Text = myTxnRes.getNetsTxnMsg();
                ////endregion

                ////region DebitTxnRes
                //bank_id.Text = myDebitTxnRes.getBankId();
                //bank_ref_code.Text = myDebitTxnRes.getBankRefCode();
                //bank_remarks.Text = myDebitTxnRes.getBankRemarks();
                //bank_status.Text = myDebitTxnRes.getBankStatus();
                //bank_time_zone.Text = myDebitTxnRes.getBankTimeZone();
                //bank_txn_date.Text = myDebitTxnRes.getBankTxnDate();
                //bank_txn_time.Text = myDebitTxnRes.getBankTxnTime();
                //func_dest_exchange_rate.Text = myDebitTxnRes.getFuncDestExchangeRate();
                //gw_time_zone.Text = myDebitTxnRes.getGwTimeZone();
                //gw_txn_date.Text = myDebitTxnRes.getGwTxnDate();
                //gw_txn_time.Text = myDebitTxnRes.getGwTxnTime();
                //gw_value_date.Text = myDebitTxnRes.getGwValueDate();
                //src_func_exchange_rate.Text = myDebitTxnRes.getSrcFuncExchangeRate();
                //txn_dest_amt.Text = myDebitTxnRes.getTxnDestAmt();
                //txn_dest_currency_code.Text = myDebitTxnRes.getTxnDestCurrencyCode();
                //txn_fun_currency_code.Text = myDebitTxnRes.getTxnFunCurrencyCode();
                //txn_func_amt.Text = myDebitTxnRes.getTxnFuncAmt();
                //txn_src_amt.Text = myDebitTxnRes.getTxnSrcAmt();
                //txn_src_currency_code.Text = myDebitTxnRes.getTxnSrcCurrencyCode();
                //retry.Text = myDebitTxnRes.getRetry();
                //version.Text = myDebitTxnRes.getVersion();
                //param1.Text = myDebitTxnRes.getParam1();
                //param2.Text = myDebitTxnRes.getParam2();
                //param3.Text = myDebitTxnRes.getParam3();
                //param4.Text = myDebitTxnRes.getParam4();
                //param5.Text = myDebitTxnRes.getParam5();
                //param11.Text = myDebitTxnRes.getParam11();
                //param12.Text = myDebitTxnRes.getParam12();
                //param13.Text = myDebitTxnRes.getParam13();
                //param14.Text = myDebitTxnRes.getParam14();
                //param15.Text = myDebitTxnRes.getParam15();
                //endregion
            }
            catch (Exception e)
            { ;}
            return sReturn;
        }

        //private string GetResponse(string Region, string Message, int Index)
        //{
        //    int i = 0;
        //    bool boolFound = false;
        //    string sReturn = string.Empty;
        //    string sValue = string.Empty;

        //    for (i = Index; i <= (Message.Length - 1); i++)
        //    {
        //        sValue = Message.Substring(i, 1);
        //        if (boolFound == true && (sValue != "•"))
        //        {
        //            sReturn += sValue;
        //        }
        //        else
        //        {
        //            if (sValue == "=")
        //            {
        //                boolFound = true;
        //            }
        //            else
        //            {
        //                boolFound = false;
        //            }
        //        }
        //        if (boolFound == false && sValue == "•")
        //        {
        //            break;
        //        }
        //    }
        //    return sReturn;
        //}
    }
}