using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.XPath;
using System.Data;
using System.Security;
using System.Security.Cryptography;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using IRSClassLib;

namespace IRSWorld.TranClass
{
    [Serializable()]
    public class eXact: sqlHelperWs
    {
        #region E-XACT SETTINGS

        private string exact_transaction_key_ = string.Empty;
        private string exact_response_key_ = string.Empty;
        private string exact_url_ = string.Empty;
        private string exact_login_ = string.Empty;
        private string exact_hash_ = string.Empty;
        private string exact_timestamp_ = string.Empty;
        private string x_invoice_num_ = string.Empty;
        private int exact_sequence_ = 0;

        private int country_codes = 8;

        public int country_code
        {
            set
            {
                this.country_codes = value;
            }
        }

        public string exact_login
        {
            set
            {
                this.exact_login_ = value;
            }
        }

        public string exact_url
        {
            set
            {
                this.exact_url_ = value;
            }
        }

        public string exact_transaction_key
        {
            set
            {
                this.exact_transaction_key_ = value;
            }
        }

        public string exact_response_key
        {
            set
            {
                this.exact_response_key_ = value;
            }
        }

        public string x_invoice_num
        {
            set
            {
                this.x_invoice_num_ = value;
            }
        }

        #endregion

        #region E-XACT RESPONSE

        private string x_response_code_ = string.Empty;
        private string x_response_reason_text_ = string.Empty;        
        private string exact_issconf_ = string.Empty;
        private string exact_ctr_ = string.Empty;
        private string exact_issname_ = string.Empty;        

        private string expiry_dates = string.Empty;
        private string authorization_nums = string.Empty;
        private string merchant_countrys = string.Empty;
        private string card_holders_names = string.Empty;
        private string card_numbers = string.Empty;

        public string x_response_code
        {
            get
            {
                return this.x_response_code_;
            }
        }

        public string x_response_reason_text
        {
            get
            {
                return this.x_response_reason_text_;
            }
        }

        public string exact_issconf
        {
            get
            {
                return this.exact_issconf_;
            }
        }

        public string exact_ctr
        {
            get
            {
                return this.exact_ctr_;
            }
        }

        public string exact_issname
        {
            get
            {
                return this.exact_issname_;
            }
        }

        public string expiry_date
        {
            get
            {
                return this.expiry_dates;
            }
        }

        public string authorization_num
        {
            get
            {
                return this.authorization_nums;
            }
        }

        public string merchant_country
        {
            get
            {
                return this.merchant_countrys;
            }
        }

        public string card_holders_name
        {
            get
            {
                return this.card_holders_names;
            }
        }

        public string card_number
        {
            get
            {
                return this.card_numbers;
            }
        }

        #endregion        

        #region TRANSACTION SETTINGS

        private string error_message_ = string.Empty;
        private decimal transaction_amount_ = 0;

        public decimal transaction_amount
        {
            set
            {
                this.transaction_amount_ = value;
            }
            get
            {
                return this.transaction_amount_;
            }
        }       

        public string error_message
        {
            get
            {
                return this.error_message_;
            }
        }

        #endregion

        public eXact()
        {
        }

        private void get_connection()
        {            
            DataTable dt = null;

            try
            {
                dt = getDataSet(getSqlCommand("select * from tbl_branch_gateway_defaults where x_country = " + this.country_codes.ToString(), false)).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    this.exact_transaction_key_ = dt.Rows[0]["x_tran_key"].ToString();
                    this.exact_response_key_    = dt.Rows[0]["x_response_key"].ToString();
                    this.exact_url_             = dt.Rows[0]["url"].ToString();
                    this.exact_login_           = dt.Rows[0]["x_login"].ToString();
                }
            }
            catch(Exception e)
            {
                string s = e.Message;
            }
            finally
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
            }
        }

        public void get_response(HttpRequest httpreq)
        {
            this.x_response_code_        = httpreq.Form["x_response_code"];
            this.x_response_reason_text_ = httpreq.Form["x_response_reason_text"];
            this.x_invoice_num_          = httpreq.Form["x_invoice_num"];
            this.exact_issconf_          = httpreq.Form["exact_issconf"];
            this.exact_ctr_              = httpreq.Form["exact_ctr"];
            this.exact_issname_          = httpreq.Form["exact_issname"];
            this.expiry_dates            = httpreq.Form["Expiry_Date"];
            this.authorization_nums      = httpreq.Form["Authorization_Num"];
            this.merchant_countrys       = httpreq.Form["MerchantCountry"];
            this.card_holders_names      = httpreq.Form["CardHoldersName"];
            this.card_numbers            = httpreq.Form["Card_Number"];
        }

        public string post_request()
        {
            StringBuilder post_data = new StringBuilder();

            try
            {
                this.get_connection();
                
                this.exact_timestamp_ = this.utc_stamp().ToString();
                this.exact_sequence_  = new Random().Next(10000, 900000);
                this.exact_hash_      = this.compute_exact_hash(this.exact_login_, this.exact_sequence_.ToString(), this.exact_timestamp_, this.transaction_amount_.ToString(), string.Empty);

                post_data.Append("x_login="           + HttpUtility.UrlEncode(this.exact_login_));
                post_data.Append("&x_amount="         + HttpUtility.UrlEncode(this.transaction_amount_.ToString()));
                post_data.Append("&x_fp_sequence="    + HttpUtility.UrlEncode(this.exact_sequence_.ToString()));
                post_data.Append("&x_fp_timestamp="   + HttpUtility.UrlEncode(this.exact_timestamp_.ToString()));                
                post_data.Append("&x_fp_hash="        + HttpUtility.UrlEncode(this.exact_hash_));
                post_data.Append("&x_invoice_num="    + HttpUtility.UrlEncode(this.x_invoice_num_));
                post_data.Append("&x_show_form="      + HttpUtility.UrlEncode("PAYMENT_FORM"));
                post_data.Append("&x_relay_response=" + HttpUtility.UrlEncode("TRUE"));
                

                return exact_url_ + "?" + post_data.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        private string compute_exact_hash(string x_login, string x_fp_sequence, string x_fp_timestamp, string x_amount, string x_currency)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(x_login)
              .Append("^")
              .Append(x_fp_sequence)
              .Append("^")
              .Append(x_fp_timestamp)
              .Append("^")
              .Append(x_amount)
              .Append("^")
              .Append(x_currency);

            byte[] data = Encoding.UTF8.GetBytes(sb.ToString());
            byte[] key = Encoding.UTF8.GetBytes(this.exact_transaction_key_);

            HMACMD5 hmac = new HMACMD5(key);

            byte[] hashBytes = hmac.ComputeHash(data);
            return System.BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        private long utc_stamp()
        {
            return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }
    }
}
//private string exact_transaction_key_ = "z5~sFMVfF8rVWVOHFQwQ";
//private string exact_response_key_ = "8gvCWxUBZCSTSU~xFCoe";
//private string exact_url_ = "https://rpm-demo.e-xact.com/payment";
//private string exact_login_ = "HCO-PNB-D-377";
//http://users2.jabry.com/pnbuat/response.asp