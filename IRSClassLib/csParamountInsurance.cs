using System;
using System.IO;
using System.Net;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
    public class csParamountInsurance:sqlHelperWs
    {
        private string get_paramount_usr
        {
            get
            {
                try
                {
                    return new AppSettingsReader().GetValue("PARAMOUNT_EMAIL", Type.GetType("System.String")).ToString();
                }
                catch
                {
                    return string.Empty;
                }                
            }
        }

        private string get_paramount_pwd
        {
            get
            {
                try
                {
                    return new AppSettingsReader().GetValue("PARAMOUNT_PASSWORD", Type.GetType("System.String")).ToString();
                }
                catch
                {
                    return string.Empty;
                }                
            }
        }
        private string get_uri_auth
        {
            get
            {
                try
                {
                    return new AppSettingsReader().GetValue("PARAMOUNT_URL_AUTH", Type.GetType("System.String")).ToString();
                }
                catch
                {
                    return string.Empty;
                }
            }            
        }

        private string get_uri_verify
        {
            get
            {
                try
                {
                    return new AppSettingsReader().GetValue("PARAMOUNT_URL_VERIFY", Type.GetType("System.String")).ToString();
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        private string get_uri_postpayment
        {
            get
            {
                try
                {
                    return new AppSettingsReader().GetValue("PARAMOUNT_URL_POSTPAYMENT", Type.GetType("System.String")).ToString();
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
        
        public string paramount_post_payment(string paramount_reference, string paramount_amount, string mode_of_payment)
        {
            StringBuilder paramountrequest = new StringBuilder();

            string email = this.get_paramount_usr;
            string pass = this.get_paramount_pwd;
            string paramount_tok = this.request_login_token();

            try
            {
                string tls_return = new AppSettingsReader().GetValue("PARAMOUNT_TLS12", Type.GetType("System.String")).ToString();

                if (tls_return != null && tls_return == "1")
                {
                    ServicePointManager.Expect100Continue = true;

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                       | (SecurityProtocolType)768  /*Tls11*/
                       | (SecurityProtocolType)3072 /*Tls12*/
                       | SecurityProtocolType.Ssl3;
                }
            }
            catch
            {
            }

            try
            {
                if (mode_of_payment != null || mode_of_payment != string.Empty)
                {
                    mode_of_payment = mode_of_payment.ToUpper().Trim();
                }

                if (paramountrequest != null)
                {
                    paramount_amount = paramount_amount.Replace(",", "");

                    paramountrequest.Clear();
                    paramountrequest.Append("reference_number=" + paramount_reference);
                    paramountrequest.Append("&mode_of_payment=" + mode_of_payment);
                    paramountrequest.Append("&amount=" + paramount_amount);

                    this.log_msg("paramount_post_payment request: " + paramountrequest.ToString());
                }

                Byte[] bytearr = Encoding.ASCII.GetBytes(paramountrequest.ToString());

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(this.get_uri_postpayment);

                request.Method = "POST";
                request.Headers.Add("Authorization", paramount_tok);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bytearr.Length;
                request.Proxy = WebProxy.GetDefaultProxy();
                request.Proxy.Credentials = CredentialCache.DefaultCredentials;

                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(bytearr, 0, bytearr.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                if (responseString != null && responseString != string.Empty)
                {
                    this.log_msg("paramount_post_payment: " + responseString);

                    if (responseString.ToLower().IndexOf("status") >= 0 && responseString.ToLower().IndexOf("ok") >= 0 && responseString.ToLower().IndexOf("message") >= 0 && responseString.ToLower().IndexOf("payment notice accepted") >= 0)
                    {
                        return responseString;
                    }
                }

                return "ERROR: " + responseString;

            }
            catch (Exception ex)
            {
                this.log_msg("paramount_post_payment: " + ex.Message); return "ERROR: " + ex.Message;
            }
        }

        public string chk_paramount_reference_num(string reference_number)
        {
            StringBuilder paramount_uricontent = new StringBuilder();

            string paramount_tok = string.Empty;
            string parpayment_amt = string.Empty;            

            try
            {
                paramount_tok = this.request_login_token();

                if (paramount_tok == null)
                {
                    return "ERROR: Unable to get response from Paramount. Please re-try.";
                }

                if (paramount_tok == "")
                {
                    return "ERROR: Unable to get response from Paramount. Please re-try.";
                }

                if (paramount_uricontent != null)
                {
                    paramount_uricontent.Clear();
                    paramount_uricontent.Append(this.get_uri_verify);
                    paramount_uricontent.Append("?");
                    paramount_uricontent.Append("reference_number=");
                    paramount_uricontent.Append(reference_number);

                    this.log_msg("chk_paramount_reference_num request: " + paramount_uricontent.ToString());
                }

                try
                {
                    string tls_return = new AppSettingsReader().GetValue("PARAMOUNT_TLS12", Type.GetType("System.String")).ToString();

                    if (tls_return != null && tls_return == "1")
                    {
                        if (tls_return != null && tls_return == "1")
                        {
                            ServicePointManager.Expect100Continue = true;

                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                               | (SecurityProtocolType)768  /*Tls11*/
                               | (SecurityProtocolType)3072 /*Tls12*/
                               | SecurityProtocolType.Ssl3;
                        }
                    }
                }
                catch
                {
                }

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(paramount_uricontent.ToString());

                request.Method = "GET";
                request.Headers.Add("Authorization", paramount_tok);
                request.Proxy = WebProxy.GetDefaultProxy();
                request.Proxy.Credentials = CredentialCache.DefaultCredentials;

                this.log_msg("check reference number: " + reference_number);                
                                
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        var val_reference = new StreamReader(stream).ReadToEnd();                        

                        if (val_reference != null && val_reference != string.Empty)
                        {
                            this.log_msg("chk_paramount_reference_num: api response - " + val_reference);

                            if (val_reference.ToLower().IndexOf("status") >= 0 && val_reference.ToLower().IndexOf("ok") >= 0 && val_reference.ToLower().IndexOf("message") >= 0 && val_reference.ToLower().IndexOf("payment_amount") >= 0)
                            {
                                string[] paramount_ret = val_reference.Split(',');

                                if (paramount_ret != null && paramount_ret.Length >= 3)
                                {
                                    parpayment_amt = paramount_ret[2];
                                    parpayment_amt = parpayment_amt.Replace("payment_amount", "");
                                    parpayment_amt = parpayment_amt.Replace("\"","");
                                    parpayment_amt = parpayment_amt.Replace("}","");
                                    parpayment_amt = parpayment_amt.Replace(":", "");
                                    parpayment_amt = parpayment_amt.Replace(",", "");

                                    return parpayment_amt;
                                }
                            }
                            else
                            {
                                return "ERROR: " + val_reference;
                            }
                        }
                    }
                }

                return "ERROR: Unable to get response from Paramount. Please re-try.";
            }
            catch(Exception e)
            {
                this.log_msg("chk_paramount_reference_num: " + e.Message); return "ERROR: " + e.Message;
            }
        }
        
        public string request_login_token()
        {
            StringBuilder paramountrequest = new StringBuilder();

            string email = this.get_paramount_usr;
            string pass = this.get_paramount_pwd;
            string paramount_token = string.Empty;

            try
            {
                string tls_return = new AppSettingsReader().GetValue("PARAMOUNT_TLS12", Type.GetType("System.String")).ToString();

                if (tls_return != null && tls_return == "1")
                {
                    if (tls_return != null && tls_return == "1")
                    {
                        ServicePointManager.Expect100Continue = true;

                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                           | (SecurityProtocolType)768  /*Tls11*/
                           | (SecurityProtocolType)3072 /*Tls12*/
                           | SecurityProtocolType.Ssl3;
                    }
                }
            }
            catch
            {
            }

            if (paramountrequest != null)
            {
                paramountrequest.Clear();
                paramountrequest.Append("email=" + email);
                paramountrequest.Append("&password=" + pass);
            }            

            Byte[] bytearr = Encoding.ASCII.GetBytes(paramountrequest.ToString());

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(this.get_uri_auth);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = bytearr.Length;            
            request.Proxy = WebProxy.GetDefaultProxy();
            request.Proxy.Credentials = CredentialCache.DefaultCredentials;

            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(bytearr, 0, bytearr.Length);
            }

            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                if (responseString.IndexOf("auth_token") >= 0)
                {
                    string[] s = null;

                    s = responseString.Split(':');

                    if (s != null && s.Length > 0)
                    {
                        paramount_token = s[1];
                        paramount_token = paramount_token.Replace("\"","").Trim();
                        paramount_token = paramount_token.Replace("}", "").Trim();
                        paramount_token = paramount_token.Replace("{", "").Trim();
                    }                    
                }

                this.log_msg("request_login_token:" + paramount_token);

                return paramount_token;

            }
            catch (WebException ex)
            {
                this.log_msg("request_login_token: " + ex.Message); return null;
            }
        }        

        public void log_msg(string s)
        {
            csAuditLog audit = null;

            try
            {
                audit = new csAuditLog();
                audit.writeAudit("", "PARAMOUNT API", s, "", "", csAuditLog.Category.Insert);
            }
            catch
            {
            }
        }
    }
}
