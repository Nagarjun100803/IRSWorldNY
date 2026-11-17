using System;
using System.IO;
using System.Net;
using System.Text;
using System.Data;
using System.Security.Cryptography;

namespace IRSClassLib
{
    [Serializable]
    public class csGiftAway
    {
        public string giftaway_api_key = string.Empty;
        public string giftaway_prv_key = string.Empty;
        public string giftaway_url_merchant_info = string.Empty;
        public string giftaway_url_order_process = string.Empty;
        public string giftaway_fee = string.Empty;

        public string merchant_id = string.Empty;
        public string merchant_name = string.Empty;
        public string merchant_denomination = string.Empty;
        public string merchant_quantity = string.Empty;        

        public string sender_name = string.Empty;
        public string sender_mobile = string.Empty;
        public string sender_email = string.Empty;

        public string recipient_name = string.Empty;
        public string recipient_mobile = string.Empty;
        public string recipient_email = string.Empty;

        public string transaction_refno = string.Empty;
        public string personal_msg = string.Empty;
                
        public csGiftAway()
        {
            DataSet ds = null;

            sqlHelperWs sqlkonek = new sqlHelperWs();


            this.transaction_refno = string.Empty;

            this.giftaway_api_key = string.Empty;
            this.giftaway_prv_key = string.Empty;
            this.giftaway_url_merchant_info = string.Empty;
            this.giftaway_url_order_process = string.Empty;
            this.giftaway_fee = string.Empty;

            this.merchant_id = string.Empty;
            this.merchant_name = string.Empty;
            this.merchant_denomination = string.Empty;
            this.merchant_quantity = string.Empty;

            this.sender_name = string.Empty;
            this.sender_mobile = string.Empty;
            this.sender_email = string.Empty;

            this.recipient_name = string.Empty;
            this.recipient_mobile = string.Empty;
            this.recipient_email = string.Empty;

            this.personal_msg = string.Empty;
            try
            {
                ds = sqlkonek.getDataSet("select * from tbl_lookup with (nolock) where fldgroup = 'GIFTAWAYAPISETTINGS'", false, new object[] { });

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        switch  (ds.Tables[0].Rows[i]["fldcode"].ToString().ToUpper())
                        {
                            case "API_KEY":
                                this.giftaway_api_key = ds.Tables[0].Rows[i]["flddescription"].ToString(); break;
                            case "API_MER_INFO":
                                this.giftaway_url_merchant_info = ds.Tables[0].Rows[i]["flddescription"].ToString(); break;
                            case "API_MER_ORDER":
                                this.giftaway_url_order_process = ds.Tables[0].Rows[i]["flddescription"].ToString(); break;
                            case "API_PRIVKEY":
                                this.giftaway_prv_key = ds.Tables[0].Rows[i]["flddescription"].ToString(); break;
                            case "API_FEE":
                                this.giftaway_fee = ds.Tables[0].Rows[i]["flddescription"].ToString(); break;
                            case "SENDER_MOBILE":
                                this.sender_mobile = ds.Tables[0].Rows[i]["flddescription"].ToString();break;
                            case "SENDER_EMAIL":
                                this.sender_email = ds.Tables[0].Rows[i]["flddescription"].ToString();break;
                            default: break;
                        }
                    }
                }
            }
            catch(Exception e)
            {
                this.log_msg("csGiftaway Initialization: " + e.Message);              
            }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                    ds = null;
                }

                sqlkonek = null;
            }            
        }
        public string inquire_merchant_info()
        {            
            string[] keysplit = { "denominations", "denomination_id"};
            string[] denoms = null;

            string api_response = string.Empty;
            string api_param = string.Empty;           

            api_param = "apikey=" + this.giftaway_api_key;
            api_param += "&merchantid=" + this.merchant_id;

            api_response = this.invoke_api_call(this.giftaway_url_merchant_info, api_param);

            if (api_response != null && api_response != string.Empty && api_response.ToUpper().IndexOf("ERROR") < 0)
            {
                denoms = api_response.Split(keysplit, StringSplitOptions.RemoveEmptyEntries);
            }

            if (denoms != null)
            {
                if (this.merchant_name_ok(denoms) == false)
                {
                    return "Error: Invalid merchant name.";
                }

                if (this.merchant_denom_ok(denoms) == false)
                {
                    return "Error: Invalid merchant denomination or no longer accepting (" + merchant_denomination + ".) Please select other denomination.";
                }
            }

            return api_response;
        }

        public void place_order()
        {
            StringBuilder sb = new StringBuilder();
            string item_ordered = string.Empty;

            item_ordered = "[" + 
                                    "[" + 
                                            this.merchant_denomination + "," + 
                                            this.merchant_quantity + 
                                    "]" + 
                           "]";

            sb.Append("apikey=" + this.giftaway_api_key);
            sb.Append("&merchantid=" + this.merchant_id);
            sb.Append("&items=" + item_ordered);
            sb.Append("&referenceno=" + this.transaction_refno);
            sb.Append("&sendername=" + this.sender_name);
            sb.Append("&sendermobile=" + this.sender_mobile);
            sb.Append("&senderemail=" + this.sender_email);
            sb.Append("&recipientname=" + this.recipient_name);
            sb.Append("&recipientmobile=" + this.recipient_mobile);
            sb.Append("&recipientemail=" + this.recipient_email);
            sb.Append("&securehash=" + this.generate_api_hash());            
            sb.Append("&expirydatetime=");
            sb.Append("&message=" + this.personal_msg);
            sb.Append("&tags=");

            string order_response = this.invoke_api_call(this.giftaway_url_order_process, sb.ToString());

            if (order_response != null && order_response != string.Empty)
            {
                this.log_msg("place_order | request: " + sb.ToString() + " | reponse: " + order_response);

                string[] s = order_response.Split(',');
                
                if (s != null && s.Length >= 3)
                {
                    string to_save = string.Empty;

                    to_save = s[0] + " | " + s[1] + " | " + s[2];
                    to_save = to_save.Replace("\"", "");
                    to_save = to_save.Replace("{", "");
                    to_save = to_save.Replace("}", "");

                    this.insert_giftaway_response(to_save);                    
                }
                else if (s != null && s.Length == 2)
                {
                    string to_save = string.Empty;

                    to_save = s[0] + " | " + s[1];
                    to_save = to_save.Replace("\"", "");
                    to_save = to_save.Replace("{", "");
                    to_save = to_save.Replace("}", "");

                    this.insert_giftaway_response(to_save);
                }
                else
                {
                    this.insert_giftaway_response(order_response);
                }
            }            
        }

        private string invoke_api_call(string giftaway_api_method, string json_content)
        {
            IWebProxy iwproxy = null;
            HttpWebRequest request = null;

            try
            {
                sqlHelperWs sqlkonek = new sqlHelperWs();

                iwproxy = WebRequest.GetSystemWebProxy();
                request = (HttpWebRequest)WebRequest.Create(giftaway_api_method);

                string tls_return = string.Empty;

                try
                {
                    tls_return = sqlkonek.getDataSet("select count(1) cnt_count from tbl_lookup with (nolock) where fldgroup = 'GIFTAWAYAPISETTINGS' and fldcode = 'TLS_SETTINGS' and flddescription = '1'", false, new object[] { }).Tables[0].Rows[0]["cnt_count"].ToString();

                    if (tls_return != null && tls_return == "1")
                    {
                        ServicePointManager.Expect100Continue = true;

                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                           | (SecurityProtocolType)768  /*Tls11*/
                           | (SecurityProtocolType)3072 /*Tls12*/
                           | SecurityProtocolType.Ssl3;                           
                    }
                }
                catch(Exception e)
                {
                    this.log_msg("TLS ERROR: " + e.Message);
                }             

                System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
                Byte[] byteArray = encoding.GetBytes(json_content);

                request.Method = "POST";
                request.ContentLength = byteArray.Length;    
                request.ContentType = @"application/x-www-form-urlencoded";

                request.Proxy = iwproxy;
                request.Credentials = CredentialCache.DefaultCredentials;
                request.Proxy.Credentials = CredentialCache.DefaultCredentials;

                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }
            }
            catch(Exception e)
            {
                this.log_msg("ERROR: " + e.Message + " : API METHOD:" + giftaway_api_method + " : API PARAM:" + json_content);
            }

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    string s = new StreamReader(response.GetResponseStream()).ReadToEnd(); return s;
                }
            }
            catch (WebException e)
            {
                this.log_msg("ERROR: " + e.Message + " : API METHOD:" + giftaway_api_method + " : API PARAM:" + json_content);
            }

            return "Unable to connect to GIFTAWAY";
        }
        private string generate_api_hash()
        {
            string giftway_param = string.Empty;
            string giftaway_total = string.Empty;
                        
            try
            {
                giftaway_total = (Convert.ToDecimal(this.merchant_denomination) * Convert.ToInt32(this.merchant_quantity)).ToString();
                giftway_param = string.Format("{0}:{1}:{2}:{3}", this.giftaway_api_key, this.transaction_refno, Convert.ToDouble(giftaway_total), this.giftaway_prv_key);

                StringBuilder sb = new StringBuilder();
                HashAlgorithm algo = SHA1.Create();

                byte[] hash = algo.ComputeHash(Encoding.UTF8.GetBytes(giftway_param));

                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("x2"));
                }
                                   
                return sb.ToString();
            }
            catch
            {
                return string.Empty;
            }           
        }

        private string remove_special_char(string s)
        {
            try
            {                
                s = s.Replace(" ", "!");
                s = s.Replace("`", "");
                s = s.Replace("~", "");
                s = s.Replace("!", "");
                s = s.Replace("@", "");
                s = s.Replace("#", "");
                s = s.Replace("$", "");
                s = s.Replace("%", "");
                s = s.Replace("^", "");
                s = s.Replace("&", "");
                s = s.Replace("*", "");
                s = s.Replace("(", "");
                s = s.Replace(")", "");
                s = s.Replace("-", "");
                s = s.Replace("_", "");
                s = s.Replace("+", "");
                s = s.Replace("=", "");
                s = s.Replace("[", "");
                s = s.Replace("]", "");
                s = s.Replace("{", "");
                s = s.Replace("}", "");
                s = s.Replace(":", "");
                s = s.Replace("?", "");
                s = s.Replace(";", "");
                s = s.Replace(",", "");
                s = s.Replace("<", "");
                s = s.Replace(">", "");
                s = s.Replace("''", "");
                s = s.Replace("\"", "");
                s = s.Replace("\\", "");
                s = s.Replace("/", "");
                s = s.Replace("|", "");
                s = s.Replace("'", "");
                s = s.Replace(".", "");

                return s;
            }
            catch
            {
                return s;
            }
        }

        private bool merchant_name_ok(string[] s)
        {
            string[] res = null;

            if (s != null && s.Length > 0)
            {
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i].ToString().ToUpper().IndexOf("MERCHANT_ID") >= 0 && s[i].ToString().ToUpper().IndexOf("NAME") >= 0 && s[i].ToString().ToUpper().IndexOf("CATEGORY") >= 0)
                    {
                        try
                        {
                            res = s[i].ToString().Substring(s[i].ToString().IndexOf("name")).Split(',');
                        }
                        catch
                        {
                        }                        

                        if (res != null && res.Length > 0)
                        {
                            string name = res[0].ToString().Replace("name", "").Replace(":", "").Replace("\"", "");
                            string irsw_name_clean = this.merchant_name.ToUpper();
                            string api_name_clean = name.ToUpper();

                            irsw_name_clean = this.remove_special_char(irsw_name_clean);
                            api_name_clean = this.remove_special_char(api_name_clean);

                            if (irsw_name_clean.ToUpper().IndexOf(api_name_clean) >= 0)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }
        private bool merchant_denom_ok(string[] s)
        {
            string[] res = null;

            if (s != null && s.Length > 0)
            {
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i].ToString().ToUpper().IndexOf("LOGO_URL") >= 0 && s[i].ToString().ToUpper().IndexOf("NAME") >= 0 && s[i].ToString().ToUpper().IndexOf("VALUE") >= 0)
                    {
                        try
                        {
                            res = s[i].ToString().Substring(s[i].ToString().IndexOf("value")).Split(',');
                        }
                        catch
                        {
                            res = null;
                        }                        

                        if (res != null && res.Length > 0)
                        {
                            string amt = res[0].ToString().Replace("value", "").Replace(":","").Replace("\"","");

                            if (amt != null && amt != string.Empty && Convert.ToDecimal(amt) == Convert.ToDecimal(this.merchant_denomination))
                            {
                                return true;
                            }
                        }                       
                    }
                }
            }

            return false;
        }

        private void insert_giftaway_response(string api_response)
        {
            sqlHelperWs sqlkonek = null;

            try
            {
                sqlkonek = new sqlHelperWs();
                sqlkonek.execStoredProcedureUpdate("insert_giftaway_response", new object[] {this.transaction_refno, api_response });                
            }
            catch(Exception e)
            {
                sqlkonek = null; this.log_msg(api_response + " - Exception Message: " + e.Message + " Reference: " + this.transaction_refno);
            }
        }

        private void log_msg(string s)
        {
            csAuditLog audit = null;

            try
            {
                audit = new csAuditLog();
                audit.writeAudit("", "GIFTAWAYAPI", s, "", "", csAuditLog.Category.Insert);
            }
            catch
            {
            }
        }
    }
}
