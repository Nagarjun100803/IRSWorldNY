using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.XPath;
using System.Data;
using System.Configuration;

namespace IRSClassLib
{
    [Serializable]
    class BeamNGoAPI
    {
        csAuditLog audit = new csAuditLog();
        public BeamNGoAPI()
        {

        }

        private string proxy_user_name_ = null;
        private string proxy_password_ = null;
        private string proxy_host_ = null;
        private int proxy_port_;

        public string proxy_user_name
        {
            set
            {
                this.proxy_user_name_ = value;
            }
        }

        public string proxy_password
        {
            set
            {
                this.proxy_password_ = value;
            }
        }

        public int proxy_port
        {
            set
            {
                this.proxy_port_ = value;
            }
        }

        public string proxy_host
        {
            set
            {
                this.proxy_host_ = value;
            }
        }


        public string id_user_name
        {
            get
            {
                return new AppSettingsReader().GetValue("BEAM_USERNAME", Type.GetType("System.String")).ToString();
            }
        }

        public string id_user_pwd
        {
            get
            {
                return new AppSettingsReader().GetValue("BEAM_PASSWORD", Type.GetType("System.String")).ToString();
            }
        }

        public string id_uri_login
        {
            get
            {
                return new AppSettingsReader().GetValue("BEAM_URL_LOGIN", Type.GetType("System.String")).ToString();
            }
        }

        public string id_uri_payment
        {
            get
            {
                return new AppSettingsReader().GetValue("BEAM_URL_PAY", Type.GetType("System.String")).ToString();
            }
        }

        /* BOYET 06-10-2019 */
        public string id_uri_val_reference
        {
            get
            {
                try
                {
                    return new AppSettingsReader().GetValue("BEAM_URL_VAL_REF", Type.GetType("System.String")).ToString();
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        public string id_uri_get_balance
        {
            get
            {
                try
                {
                    return new AppSettingsReader().GetValue("BEAM_URL_BALANCE", Type.GetType("System.String")).ToString();
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
        /* BOYET 06-10-2019 */

        private string _response;
        public string response
        {
            set {
                _response = value;
            }
        }

        private string _fxrate;
        public string FXRate
        {
            get { return _fxrate; }
            set
            {
                _fxrate = value;
            }
        }

        private string _currency;
        public string Currency
        {
            get { return _currency; }
            set
            {
                _currency = value;
            }
        }

        private string _refno;
        public string ReferenceNo
        {
            get { return _refno; }
            set
            {
                _refno = value;
            }
        }

        private string _amountreceived;
        public string AmountReceived
        {
            get { return _amountreceived; }
            set
            {
                _amountreceived = value;
            }
        }

        private string _remarks;
        public string Remarks
        {
            get { return _remarks; }
            set
            {
                _remarks = value;
            }
        }

        /* ADDED BY BOYET BEAM AND GO 06-10-2019 */

        public string get_beam_balance()
        {
            string json = string.Empty;
            string[] ok_return = null;

            try
            {
                string BnG_Token = this.Request_Response_Beam_Login();
                string tranx = this.POST_Transaction(id_uri_get_balance, json, BnG_Token);
                string response = tranx.Split(',')[1].Split(':')[1];
                string response_code = tranx.Split(',')[0].Split(':')[2];

                this.log_msg("get_beam_balance: " + tranx);

                if (response_code.Trim() == "200")
                {
                    ok_return = tranx.Split(',');

                    if (ok_return != null && ok_return.Length >= 9)
                    {
                        string[] amt = null; amt = ok_return[7].Split(':'); return amt[1].Replace("}","");
                    }
                }
             
                return "ERROR: Unable to get Beam and Go balance amount";
            }
            catch(Exception e)
            {
                this.log_msg("get_beam_balance: " + e.Message); return "ERROR: Unable to get Beam and Go balance amount - " + e.Message;
            }
        }

        public string get_transaction_by_pin()
        {
            string json = string.Empty;
            string beam_api_value = string.Empty;
            string[] ok_return = null;

            string beam_amount_balance = this.get_beam_balance();

            try
            {
                if (beam_amount_balance == null)
                {
                    return "ERROR: Unable to connect to Beam and Go (get_transaction_by_pin > Get Balance)";
                }

                if (beam_amount_balance != null && beam_amount_balance.IndexOf("ERROR") >= 0)
                {
                    return beam_amount_balance;
                }

                json = "[{\"Key\":\"TransactionCode\"," + "\"Value\":\"" + this._refno + "\"}]";
                
                string BnG_Token = this.Request_Response_Beam_Login();
                string tranx = this.POST_Transaction(id_uri_val_reference, json, BnG_Token);
                string response = tranx.Split(',')[1].Split(':')[1];
                string response_code = tranx.Split(',')[0].Split(':')[2];

                this.log_msg("get_transaction_by_pin:" + tranx);

                if (response_code.Trim() == "200")
                {
                    ok_return = tranx.Split(',');
                }

                if (ok_return != null && ok_return.Length >= 32 && response_code != null && response_code.Trim() == "200")
                {
                    string api_purchase_res = string.Empty;
                    string customer_pay = ok_return[32].Split(':')[1];

                    if (AmountReceived != null && AmountReceived != string.Empty)
                    {
                        AmountReceived = AmountReceived.Replace(",", "");
                    }

                    if (customer_pay != null && customer_pay != string.Empty)
                    {
                        customer_pay = customer_pay.Replace(",", "");
                    }

                    if (AmountReceived.Trim() != customer_pay.Trim())
                    {
                        return "ERROR: Pay Amount Should be ( " + customer_pay + " )" ;
                    }

                    if (beam_amount_balance != null && beam_amount_balance != string.Empty)
                    {
                        beam_amount_balance = beam_amount_balance.Replace(",", "");
                    }

                    if (Convert.ToDecimal(beam_amount_balance) < Convert.ToDecimal(customer_pay))
                    {
                        return "ERROR: Insufficient Balance. Current Beam and Go Balance: ( " + beam_amount_balance + " ) is less than Pay Amount: ( " + customer_pay + " )";
                    }

                    if (AmountReceived.Trim() == customer_pay.Trim())
                    {
                        return "VALID";
                    }
                }
                
                return "ERROR: " + tranx.Replace("\"", ""); /* RETURN BEAM API ERROR MESSAGE TO FRONT END IF RESPONSE CODE IS NOT EQUAL TO 200 */
            }
            catch(Exception e)
            {
                this.log_msg("get_transaction_by_pin: " + e.Message); return "ERROR: " + e.Message;
            }
        }

        public string post_beam_and_go_reference()
        {
            string json = string.Empty;
            string reference_no = string.Empty;
            string fx_rate = string.Empty;
            string currency_code = string.Empty;
            string amount_received = string.Empty;
            string remarks = string.Empty;

            string bnb_token = string.Empty;
            string tranx = string.Empty;
            string response = string.Empty;
            string response_code = string.Empty;

            try
            {
                reference_no = this._refno;
                fx_rate = this._fxrate;
                currency_code = this._currency;
                amount_received = this._amountreceived;
                remarks = this._remarks;

                if (amount_received != null && amount_received != string.Empty)
                {
                    amount_received = amount_received.Replace(",", "");
                }

                json = "[{\"Key\":\"TransactionCode\"," + "\"Value\":\"" + reference_no + "\"},";
                json += "{\"Key\":\"FXRate\"," + "\"Value\":\"" + fx_rate + "\"},";
                json += "{\"Key\":\"AmountReceivedCurrencyCode\"," + "\"Value\":\"" + currency_code + "\"},";
                json += "{\"Key\":\"AmountReceived\"," + "\"Value\":\"" + amount_received + "\"},";
                json += "{\"Key\":\"Remarks\"," + "\"Value\":\"" + remarks + "\"}]";

                bnb_token = this.Request_Response_Beam_Login();
                tranx = this.POST_Transaction(id_uri_payment, json, bnb_token);
                response = tranx.Split(',')[1].Split(':')[1];
                response_code = tranx.Split(',')[0].Split(':')[2];
                this.log_msg("post_beam_and_go_reference:" + tranx);

                if (response_code != null && response_code.Trim() == "200")
                {
                    this.log_msg("BEAM TRANSACTION CODE PAID:" + this._refno); return "PAID";
                }

                return "ERROR: Unable to send payment to Beam and Go";

            }
            catch(Exception e)
            {
                this.log_msg("post_beam_and_go_reference:" + e.Message); return "ERROR: Unable to send payment to Beam and Go. " + e.Message;
            }
        }

        /* ADDED BY BOYET 06-10-2019 */

        public bool ProcessTransaction()
        {
            
            string refno = this._refno;
            string fxrate = this._fxrate;
            string currencycode = this._currency;
            string amountreceived = this._amountreceived;
            string remarks = this._remarks;

            string json = "[{\"Key\":\"TransactionCode\"," +
                          "\"Value\":\"" + refno + "\"},";

            json += "{\"Key\":\"FXRate\"," +
                          "\"Value\":\"" + fxrate + "\"},";

            json += "{\"Key\":\"AmountReceivedCurrencyCode\"," +
                          "\"Value\":\"" + currencycode + "\"},";

            json += "{\"Key\":\"AmountReceived\"," +
                          "\"Value\":\"" + amountreceived + "\"},";

            json += "{\"Key\":\"Remarks\"," +
                          "\"Value\":\"" + remarks + "\"}]";

            string BnG_Token = this.Request_Response_Beam_Login();
            string tranx = this.POST_Transaction(id_uri_payment, json, BnG_Token);//
            string response = tranx.Split(',')[1].Split(':')[1];

            string response_code = tranx.Split(',')[0].Split(':')[2];

            this.log_msg("ProcessTransaction" + json);
            return (response_code == "200") ? true : false;
        }

        private string POST_Transaction(string url, string jsonContent, string token)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";

            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(jsonContent);

            request.ContentLength = byteArray.Length;
            request.ContentType = @"application/json";
            request.Headers.Add("BnG_Token:" + token);
            request.Proxy = WebProxy.GetDefaultProxy();
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Proxy.Credentials = CredentialCache.DefaultCredentials;

            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    string s = new StreamReader(response.GetResponseStream()).ReadToEnd(); this.log_msg("POST_Transaction: " + s + " API Request: " + jsonContent); return s;
                }
            }
            catch (WebException ex)
            {
                new csAuditLog().writeAudit(((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId, "BEAM API", ex.Message, "", "", csAuditLog.Category.Select); return null;
            }
        }

        private string POST(string url, string jsonContent)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";

            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(jsonContent);

            request.ContentLength = byteArray.Length;
            request.ContentType = @"application/json";
            request.Proxy = WebProxy.GetDefaultProxy();
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Proxy.Credentials = CredentialCache.DefaultCredentials;

            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    string s = new StreamReader(response.GetResponseStream()).ReadToEnd(); this.log_msg("POST: " + s); return s;
                }
            }
            catch (WebException ex)
            {
                new csAuditLog().writeAudit(((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId, "Beam API", ex.Message, "", "", csAuditLog.Category.Select); return null;
                
            }
        }

        public string Request_Response_Beam_Login()
        {
            csAuditLog csAudit = new csAuditLog();
            StringBuilder post_data = null;
            post_data = new StringBuilder();

            string username = id_user_name;
            string password = id_user_pwd;

            
            string json = "[{\"Key\":\"Username\"," +
                          "\"Value\":\"" + username + "\"},";

            json += "{\"Key\":\"Password\"," +
                          "\"Value\":\"" + password + "\"}]";            


            string x = this.POST(this.id_uri_login, json);
            string[] arr = x.Split(',');
            string BnG_Token = "";

            if (arr.Length > 9)
            {
                BnG_Token = arr[9].Split(':')[1];
                BnG_Token = BnG_Token.Replace("\"", "");
            }

            this.log_msg("Request_Response_Beam_Login: " + BnG_Token); return BnG_Token;
        }

        public void log_msg(string s)
        {
            csAuditLog audit = null;

            try
            {
                audit = new csAuditLog();
                audit.writeAudit("", "BEAM API", s, "", "", csAuditLog.Category.Insert);
            }
            catch
            {
            }
        }
    }
}
