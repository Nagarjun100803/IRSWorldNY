using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.XPath;
using System.Data;

namespace IRSWorld.TranClass
{
    [Serializable()]
    public class Idology
    {
        private ArrayList array_q_and_a = new ArrayList();

        private string idology_xml_response_ = string.Empty;
        private string idology_xml_ans_response = string.Empty;
        private string idology_class_exception_ = string.Empty;

        private string http_post_req_ = string.Empty;
        private string http_post_ans_ = string.Empty;

        public string http_post_req
        {
            get
            {
                return this.http_post_req_;
            }
        }

        public string http_post_ans
        {
            get
            {
                return this.http_post_ans_;
            }
        }


        #region HTTP CONNECTION PROXY

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

        #endregion

        #region IDOLOGY XML & CLASS EXCEPTION GETTER SETTER

        private DataSet ds_question_and_ans = null;
        private DataSet ds_summary_result_inq = null;
        private DataSet ds_result_inq = null;
        private DataSet ds_qualifier_inq = null;
        private DataSet ds_summary_result_qna = null;
        private DataSet ds_result_qna = null;
        private DataSet ds_qualifier_qna = null;
        private DataSet ds_idliveq_req_result_qna = null;
        private DataSet ds_iq_summary_result_qna = null;
        private DataSet ds_velocity_result_inq = null;

        public string idology_xml_response
        {
            get
            {
                return this.idology_xml_response_;
            }
        }

        public string idology_xml_answer_response
        {
            get
            {
                return this.idology_xml_ans_response;
            }
        }

        public DataSet ds_summary_results_qna
        {
            get
            {
                return this.ds_summary_result_qna;
            }
        }

        public DataSet ds_idliveq_req_results_qna
        {
            get
            {
                return this.ds_idliveq_req_result_qna;
            }
        }

        public DataSet ds_question_and_answer
        {
            get
            {
                return this.ds_question_and_ans;
            }
        }

        public DataSet ds_summary_results_inq
        {
            get
            {
                return this.ds_summary_result_inq;
            }
        }

        public DataSet ds_results_inq
        {
            get
            {
                return this.ds_result_inq;
            }
        }

        public DataSet ds_qualifiers_inq
        {
            get
            {
                return this.ds_qualifier_inq;
            }
        }

        public DataSet ds_iq_summary_results_qna
        {
            get
            {
                return this.ds_iq_summary_result_qna;
            }
        }

        public DataSet ds_results_qna
        {
            get
            {
                return this.ds_result_qna;
            }
        }

        public DataSet ds_qualifiers_qna
        {
            get
            {
                return this.ds_qualifier_qna;
            }
        }

        public DataSet ds_velocity_results_inq
        {
            get
            {
                return this.ds_velocity_result_inq;
            }
        }

        public string idology_class_exception
        {
            get
            {
                return idology_class_exception_;
            }
        }


        #endregion

        #region GETTER & SETTER IDOLOGY CREDENTIAL AND API PATH

        private string id_iq_url_ = string.Empty;
        private string id_iq_answer_url_ = string.Empty;
        private string id_user_name_ = string.Empty;
        private string id_user_pwd_ = string.Empty;

        public string id_iq_url
        {
            get
            {
                return this.id_iq_url_;
            }
            set
            {
                this.id_iq_url_ = value.Trim();
            }
        }

        public string id_iq_answer_url
        {
            get
            {
                return this.id_iq_answer_url_;
            }
            set
            {
                this.id_iq_answer_url_ = value.Trim();
            }
        }

        public string id_user_name
        {
            get
            {
                return id_user_name_;
            }
            set
            {
                this.id_user_name_ = value.Trim();
            }
        }

        public string id_user_pwd
        {
            get
            {
                return id_user_pwd_;
            }
            set
            {
                this.id_user_pwd_ = value.Trim();
            }
        }

        #endregion

        #region GETTER & SETTER IDOLOGY API PROPERTIES

        private string invoice_number_ = string.Empty;
        private string order_amount_ = string.Empty;
        private string shipping_amount_ = string.Empty;
        private string tax_amount_ = string.Empty;
        private string total_amount_ = string.Empty;
        private string id_type_ = string.Empty;
        private string id_issuer_ = string.Empty;
        private string id_number_ = string.Empty;
        private string payment_method_ = string.Empty;

        private string first_name_ = string.Empty;
        private string last_name_ = string.Empty;
        private string address_ = string.Empty;
        private string city_ = string.Empty;
        private string state_ = string.Empty;
        private string zip_ = string.Empty;

        private string ssn_last4_digit_ = string.Empty;
        private string ssn_full_digit_ = string.Empty;
        private string dob_month_ = string.Empty;
        private string dob_day_ = string.Empty;
        private string dob_year_ = string.Empty;
        private string ip_address_ = string.Empty;
        private string sku_ = string.Empty;
        private string uid_ = string.Empty;


        public string first_name
        {
            get
            {
                return this.first_name_;
            }
            set
            {
                this.first_name_ = value.Trim();
            }
        }

        public string last_name
        {
            get
            {
                return this.last_name_;
            }
            set
            {
                this.last_name_ = value.Trim();
            }
        }

        public string address
        {
            get
            {
                return this.address_;
            }
            set
            {
                this.address_ = value.Trim();
            }
        }

        public string city
        {
            get
            {
                return this.city_;
            }
            set
            {
                this.city_ = value.Trim();
            }
        }

        public string state
        {
            get
            {
                return this.state_;
            }
            set
            {
                this.state_ = value.Trim();
            }
        }

        public string zip
        {
            get
            {
                return this.zip_;
            }
            set
            {
                this.zip_ = value.Trim();
            }
        }

        public string invoice_number
        {
            get
            {
                return this.invoice_number_;
            }
            set
            {
                this.invoice_number_ = value.Trim();
            }
        }

        public string order_amount
        {
            get
            {
                return this.order_amount_;
            }
            set
            {
                this.order_amount_ = value.Trim();
            }
        }


        public string shipping_amount
        {
            get
            {
                return this.shipping_amount_;
            }
            set
            {
                this.shipping_amount_ = value.Trim();
            }
        }

        public string total_amount
        {
            get
            {
                return this.total_amount_;
            }
            set
            {
                this.total_amount_ = value.Trim();
            }
        }

        public string tax_amount
        {
            get
            {
                return this.tax_amount_;
            }
            set
            {
                this.tax_amount_ = value.Trim();
            }
        }

        public string id_type
        {
            get
            {
                return this.id_type_;
            }
            set
            {
                this.id_type_ = value.Trim();
            }
        }

        public string id_issuer
        {
            get
            {
                return this.id_issuer_;
            }
            set
            {
                this.id_issuer_ = value.Trim();
            }
        }

        public string id_number
        {
            get
            {
                return this.id_number_;
            }
            set
            {
                this.id_number_ = value.Trim();
            }
        }

        public string payment_method
        {
            get
            {
                return this.payment_method_;
            }
            set
            {
                this.payment_method_ = value.Trim();
            }
        }

        public string ssn_last4_digit
        {
            get
            {
                return this.ssn_last4_digit_;
            }
            set
            {
                this.ssn_last4_digit_ = value.Trim();
            }
        }

        public string ssn_full_digit
        {
            get
            {
                return this.ssn_full_digit_;
            }
            set
            {
                this.ssn_full_digit_ = value.Trim();
            }
        }

        public string dob_month
        {
            get
            {
                return this.dob_month_;
            }
            set
            {
                this.dob_month_ = value.Trim();
            }
        }

        public string dob_day
        {
            get
            {
                return this.dob_day_;
            }
            set
            {
                this.dob_day_ = value.Trim();
            }
        }

        public string dob_year
        {
            get
            {
                return this.dob_year_;
            }
            set
            {
                this.dob_year_ = value.Trim();
            }
        }

        public string ip_address
        {
            get
            {
                return this.ip_address_;
            }
            set
            {
                this.ip_address_ = value.Trim();
            }
        }

        public string sku
        {
            get
            {
                return this.sku_;
            }
            set
            {
                this.sku_ = value.Trim();
            }
        }

        public string uid
        {
            get
            {
                return this.uid_;
            }
            set
            {
                this.uid_ = value.Trim();
            }
        }


        #endregion

        public Idology()
        {
            this.id_iq_url = "https://web.idologylive.com/api/idiq.svc";
            this.id_iq_answer_url = "https://web.idologylive.com/api/idliveq-answers.svc";
        }

        ~Idology()
        {
            if (this.ds_question_and_ans != null)
            {
                this.ds_question_and_ans.Dispose();
                this.ds_question_and_ans = null;
            }

            if (this.array_q_and_a != null)
            {
                this.array_q_and_a.Clear();
                this.array_q_and_a = null;
            }
        }

        public XPathNodeIterator navigate_response(string xml_string_response, string key_to_navigate)
        {
            return (new XPathDocument(new MemoryStream(UTF8Encoding.UTF8.GetBytes(xml_string_response)))).CreateNavigator().Select(key_to_navigate);
        }

        public void get_iq_response()
        {
            StringBuilder post_data = null;

            try
            {
                if (this.array_q_and_a != null)
                {
                    this.array_q_and_a.Clear();
                    this.array_q_and_a = null;
                }

                post_data = new StringBuilder();
                post_data.Append("username=" + HttpUtility.UrlEncode(this.id_user_name_));
                post_data.Append("&password=" + HttpUtility.UrlEncode(this.id_user_pwd_));
                post_data.Append("&firstName=" + HttpUtility.UrlEncode(this.first_name_));
                post_data.Append("&lastName=" + HttpUtility.UrlEncode(this.last_name_));
                post_data.Append("&address=" + HttpUtility.UrlEncode(this.address_));
                post_data.Append("&city=" + HttpUtility.UrlEncode(this.city_));
                post_data.Append("&state=" + HttpUtility.UrlEncode(this.state_));
                post_data.Append("&zip=" + HttpUtility.UrlEncode(this.zip_));

                post_data.Append("&ssnLast4=" + HttpUtility.UrlEncode(this.ssn_last4_digit_));
                post_data.Append("&ssn=" + HttpUtility.UrlEncode(this.ssn_full_digit_));
                post_data.Append("&dobMonth=" + HttpUtility.UrlEncode(this.dob_month_));
                post_data.Append("&dobDay=" + HttpUtility.UrlEncode(this.dob_day_));
                post_data.Append("&dobYear=" + HttpUtility.UrlEncode(this.dob_year_));
                post_data.Append("&ipAddress=" + HttpUtility.UrlEncode(this.ip_address_));
                post_data.Append("&sku=" + HttpUtility.UrlEncode(this.sku_));
                post_data.Append("&uid=" + HttpUtility.UrlEncode(this.uid_));

                post_data.Append("&invoice=" + HttpUtility.UrlEncode(this.invoice_number_));
                post_data.Append("&shipping=" + HttpUtility.UrlEncode(this.shipping_amount_));
                post_data.Append("&amount=" + HttpUtility.UrlEncode(this.order_amount_));
                post_data.Append("&tax=" + HttpUtility.UrlEncode(this.tax_amount_));
                post_data.Append("&total=" + HttpUtility.UrlEncode(this.total_amount_));
                post_data.Append("&idType=" + HttpUtility.UrlEncode(this.id_type_));
                post_data.Append("&idIssuer=" + HttpUtility.UrlEncode(this.id_issuer_));
                post_data.Append("&idNumber=" + HttpUtility.UrlEncode(this.id_number_));
                post_data.Append("&paymentMethod=" + HttpUtility.UrlEncode(this.payment_method_));

                this.http_post_req_ = post_data.ToString();
                this.idology_class_exception_ = null;
                this.idology_xml_response_ = this.post_and_receive(this.id_iq_url_, post_data);

                if (string.IsNullOrEmpty(this.idology_xml_response_).Equals(false))
                {
                    XPathNodeIterator node_item = this.navigate_response(this.idology_xml_response_, "//id-number");

                    if (node_item != null)
                    {
                        while (node_item.MoveNext())
                        {
                            this.id_number_ = node_item.Current.Value;
                        }
                    }
                }

                this.get_error_inq();

                this.set_result_inq();
                this.set_qualifiers_inq();
                this.set_summary_result_inq();
                this.set_velocity_result_inq();
                this.set_questionaire_response();
            }
            catch (Exception e)
            {
                this.idology_class_exception_ = e.Message;
            }
            finally
            {
                post_data = null;
            }
        }

        public void answer_question()
        {
            StringBuilder post_data = null;

            try
            {
                if (this.array_q_and_a != null && this.array_q_and_a.Count > 0)
                {
                    post_data = new StringBuilder();
                    post_data.Append("username=" + HttpUtility.UrlEncode(this.id_user_name_));
                    post_data.Append("&password=" + HttpUtility.UrlEncode(this.id_user_pwd_));
                    post_data.Append("&idNumber=" + HttpUtility.UrlEncode(this.id_number_));

                    for (int i = 0; i < this.array_q_and_a.Count; i++)
                    {
                        post_data.Append(this.array_q_and_a[i].ToString());
                    }

                    this.http_post_ans_ = post_data.ToString();
                    this.idology_class_exception_ = null;
                    this.idology_xml_ans_response = this.post_and_receive(this.id_iq_answer_url, post_data);

                    this.set_result_qna();
                    this.set_qualifiers_qna();
                    this.set_summary_result_qna();
                    this.set_idliveq_req_result_qna();
                    this.set_iq_summary_result_qna();
                }
            }
            catch (Exception e)
            {
                this.idology_class_exception_ = e.Message;
            }
            finally
            {
                post_data = null;

                if (this.array_q_and_a != null)
                {
                    this.array_q_and_a.Clear();
                    this.array_q_and_a = null;
                }
            }
        }

        public void construct_q_and_a(string question_type, string answer)
        {
            if (this.array_q_and_a == null)
            {
                this.array_q_and_a = new ArrayList();
            }

            if (string.IsNullOrEmpty(question_type).Equals(false) && string.IsNullOrEmpty(answer).Equals(false))
            {
                int row_num = 0;

                if (this.array_q_and_a != null)
                {
                    row_num = this.array_q_and_a.Count;
                }

                row_num += 1;

                if (row_num < 4)
                {
                    this.array_q_and_a.Add("&" + "question" + row_num.ToString() + "Type=" + HttpUtility.UrlEncode(question_type) + "&question" + row_num.ToString() + "Answer=" + HttpUtility.UrlEncode(answer));
                }
            }
        }

        private String post_and_receive(String uri_string, StringBuilder post_data)
        {
            HttpWebRequest request = null;

            this.idology_class_exception_ = null;

            try
            {
                byte[] byte_data = UTF8Encoding.UTF8.GetBytes(post_data.ToString());

                request = (HttpWebRequest)WebRequest.Create(new Uri(uri_string));
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";

                if (this.proxy_host_ != string.Empty && this.proxy_user_name_ != string.Empty && this.proxy_password_ != string.Empty && this.proxy_port_ > 0)
                {
                    request.Proxy = new WebProxy(this.proxy_host_, this.proxy_port_);
                    request.Proxy.Credentials = new NetworkCredential(this.proxy_user_name_, this.proxy_password_);
                }
                else
                {
                    request.Proxy = WebProxy.GetDefaultProxy();
                    request.Proxy.Credentials = CredentialCache.DefaultCredentials;
                }

                request.ContentLength = byte_data.Length;

                using (Stream post_stream = request.GetRequestStream())
                {
                    post_stream.Write(byte_data, 0, byte_data.Length);
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    return new StreamReader(response.GetResponseStream()).ReadToEnd();
                }
            }
            catch (Exception e)
            {
                this.idology_class_exception_ = e.Message; return null;
            }
            finally
            {
                request = null;
                post_data = null;
            }
        }

        private void set_summary_result_inq()
        {
            DataTable dt = null;
            XmlDocument xml_doc = null;
            XmlNodeList xml_node_list = null;

            try
            {
                if (this.ds_summary_result_inq != null)
                {
                    this.ds_summary_result_inq.Clear();
                    this.ds_summary_result_inq.Dispose();
                    this.ds_summary_result_inq = null;
                }

                if (string.IsNullOrEmpty(this.idology_xml_response_).Equals(false))
                {
                    xml_doc = new XmlDocument();
                    xml_doc.LoadXml(this.idology_xml_response_);
                    xml_node_list = xml_doc.GetElementsByTagName("summary-result");

                    if (xml_node_list == null || xml_node_list.Count < 1)
                    {
                        return;
                    }

                    dt = new DataTable("summary_result_inq");
                    dt.Columns.Add("key", Type.GetType("System.String"));
                    dt.Columns.Add("message", Type.GetType("System.String"));

                    for (int i = 0; i < xml_node_list.Count; i++)
                    {
                        string xml = xml_node_list[i].OuterXml;

                        if (string.IsNullOrEmpty(xml).Equals(false))
                        {
                            XmlDocument xml_doc_inner = null;

                            xml_doc_inner = new XmlDocument();
                            xml_doc_inner.LoadXml(xml);

                            string key = xml_doc_inner.GetElementsByTagName("key").Item(0).InnerText;
                            string msg = xml_doc_inner.GetElementsByTagName("message").Item(0).InnerText;

                            xml_doc_inner.RemoveAll();
                            xml_doc_inner = null;

                            if (dt != null)
                            {
                                dt.Rows.Add(key, msg);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                this.idology_class_exception_ = e.Message;
            }
            finally
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    this.ds_summary_result_inq = new DataSet();
                    this.ds_summary_result_inq.Merge(dt);
                    this.ds_summary_result_inq.AcceptChanges();

                    dt.Clear();
                    dt.Dispose();
                    dt = null;
                }

                if (xml_doc != null)
                {
                    xml_doc.RemoveAll();
                    xml_doc = null;
                }

                xml_node_list = null;
            }
        }

        private void set_result_inq()
        {
            DataTable dt = null;
            XmlDocument xml_doc = null;
            XmlNodeList xml_node_list = null;

            try
            {
                if (this.ds_result_inq != null)
                {
                    this.ds_result_inq.Clear();
                    this.ds_result_inq.Dispose();
                    this.ds_result_inq = null;
                }

                if (string.IsNullOrEmpty(this.idology_xml_response_).Equals(false))
                {
                    xml_doc = new XmlDocument();
                    xml_doc.LoadXml(this.idology_xml_response_);
                    xml_node_list = xml_doc.GetElementsByTagName("results");

                    if (xml_node_list == null || xml_node_list.Count < 1)
                    {
                        return;
                    }

                    dt = new DataTable("result_inq");
                    dt.Columns.Add("key", Type.GetType("System.String"));
                    dt.Columns.Add("message", Type.GetType("System.String"));

                    for (int i = 0; i < xml_node_list.Count; i++)
                    {
                        string xml = xml_node_list[i].OuterXml;

                        if (string.IsNullOrEmpty(xml).Equals(false))
                        {
                            XmlDocument xml_doc_inner = null;

                            xml_doc_inner = new XmlDocument();
                            xml_doc_inner.LoadXml(xml);

                            string key = xml_doc_inner.GetElementsByTagName("key").Item(0).InnerText;
                            string msg = xml_doc_inner.GetElementsByTagName("message").Item(0).InnerText;

                            xml_doc_inner.RemoveAll();
                            xml_doc_inner = null;

                            if (dt != null)
                            {
                                dt.Rows.Add(key, msg);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                this.idology_class_exception_ = e.Message;
            }
            finally
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    this.ds_result_inq = new DataSet();
                    this.ds_result_inq.Merge(dt);
                    this.ds_result_inq.AcceptChanges();

                    dt.Clear();
                    dt.Dispose();
                    dt = null;
                }

                if (xml_doc != null)
                {
                    xml_doc.RemoveAll();
                    xml_doc = null;
                }

                xml_node_list = null;
            }
        }

        private void set_qualifiers_qna()
        {
            DataTable dt = null;
            XmlDocument xml_doc = null;
            XmlNodeList xml_node_list = null;

            try
            {
                if (this.ds_qualifier_qna != null)
                {
                    this.ds_qualifier_qna.Clear();
                    this.ds_qualifier_qna.Dispose();
                    this.ds_qualifier_qna = null;
                }

                if (string.IsNullOrEmpty(this.idology_xml_ans_response).Equals(false))
                {
                    xml_doc = new XmlDocument();
                    xml_doc.LoadXml(this.idology_xml_ans_response);
                    xml_node_list = xml_doc.GetElementsByTagName("qualifier");

                    if (xml_node_list == null || xml_node_list.Count < 1)
                    {
                        return;
                    }

                    dt = new DataTable("qualifiers_qna");
                    dt.Columns.Add("key", Type.GetType("System.String"));
                    dt.Columns.Add("message", Type.GetType("System.String"));

                    for (int i = 0; i < xml_node_list.Count; i++)
                    {
                        string xml = xml_node_list[i].OuterXml;

                        if (string.IsNullOrEmpty(xml).Equals(false))
                        {
                            XmlDocument xml_doc_inner = null;

                            xml_doc_inner = new XmlDocument();
                            xml_doc_inner.LoadXml(xml);

                            string key = xml_doc_inner.GetElementsByTagName("key").Item(0).InnerText;
                            string msg = xml_doc_inner.GetElementsByTagName("message").Item(0).InnerText;

                            xml_doc_inner.RemoveAll();
                            xml_doc_inner = null;

                            if (dt != null)
                            {
                                dt.Rows.Add(key, msg);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                this.idology_class_exception_ = e.Message;
            }
            finally
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    this.ds_qualifier_qna = new DataSet();
                    this.ds_qualifier_qna.Merge(dt);
                    this.ds_qualifier_qna.AcceptChanges();

                    dt.Clear();
                    dt.Dispose();
                    dt = null;
                }

                if (xml_doc != null)
                {
                    xml_doc.RemoveAll();
                    xml_doc = null;
                }

                xml_node_list = null;
            }
        }

        private void set_result_qna()
        {
            DataTable dt = null;
            XmlDocument xml_doc = null;
            XmlNodeList xml_node_list = null;

            try
            {
                if (this.ds_result_inq != null)
                {
                    this.ds_result_qna.Clear();
                    this.ds_result_qna.Dispose();
                    this.ds_result_qna = null;
                }

                if (string.IsNullOrEmpty(this.idology_xml_ans_response).Equals(false))
                {
                    xml_doc = new XmlDocument();
                    xml_doc.LoadXml(this.idology_xml_ans_response);
                    xml_node_list = xml_doc.GetElementsByTagName("results");

                    if (xml_node_list == null || xml_node_list.Count < 1)
                    {
                        return;
                    }

                    dt = new DataTable("result_qna");
                    dt.Columns.Add("key", Type.GetType("System.String"));
                    dt.Columns.Add("message", Type.GetType("System.String"));

                    for (int i = 0; i < xml_node_list.Count; i++)
                    {
                        string xml = xml_node_list[i].OuterXml;

                        if (string.IsNullOrEmpty(xml).Equals(false))
                        {
                            XmlDocument xml_doc_inner = null;

                            xml_doc_inner = new XmlDocument();
                            xml_doc_inner.LoadXml(xml);

                            string key = xml_doc_inner.GetElementsByTagName("key").Item(0).InnerText;
                            string msg = xml_doc_inner.GetElementsByTagName("message").Item(0).InnerText;

                            xml_doc_inner.RemoveAll();
                            xml_doc_inner = null;

                            if (dt != null)
                            {
                                dt.Rows.Add(key, msg);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                this.idology_class_exception_ = e.Message;
            }
            finally
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    this.ds_result_qna = new DataSet();
                    this.ds_result_qna.Merge(dt);
                    this.ds_result_qna.AcceptChanges();

                    dt.Clear();
                    dt.Dispose();
                    dt = null;
                }

                if (xml_doc != null)
                {
                    xml_doc.RemoveAll();
                    xml_doc = null;
                }

                xml_node_list = null;
            }
        }

        private void set_summary_result_qna()
        {
            DataTable dt = null;
            XmlDocument xml_doc = null;
            XmlNodeList xml_node_list = null;

            try
            {
                if (this.ds_summary_result_qna != null)
                {
                    this.ds_summary_result_qna.Clear();
                    this.ds_summary_result_qna.Dispose();
                    this.ds_summary_result_qna = null;
                }

                if (string.IsNullOrEmpty(this.idology_xml_ans_response).Equals(false))
                {
                    xml_doc = new XmlDocument();
                    xml_doc.LoadXml(this.idology_xml_ans_response);
                    xml_node_list = xml_doc.GetElementsByTagName("summary-result");

                    if (xml_node_list == null || xml_node_list.Count < 1)
                    {
                        return;
                    }

                    dt = new DataTable("summary_result_qna");
                    dt.Columns.Add("key", Type.GetType("System.String"));
                    dt.Columns.Add("message", Type.GetType("System.String"));

                    for (int i = 0; i < xml_node_list.Count; i++)
                    {
                        string xml = xml_node_list[i].OuterXml;

                        if (string.IsNullOrEmpty(xml).Equals(false))
                        {
                            XmlDocument xml_doc_inner = null;

                            xml_doc_inner = new XmlDocument();
                            xml_doc_inner.LoadXml(xml);

                            string key = xml_doc_inner.GetElementsByTagName("key").Item(0).InnerText;
                            string msg = xml_doc_inner.GetElementsByTagName("message").Item(0).InnerText;

                            xml_doc_inner.RemoveAll();
                            xml_doc_inner = null;

                            if (dt != null)
                            {
                                dt.Rows.Add(key, msg);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                this.idology_class_exception_ = e.Message;
            }
            finally
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    this.ds_summary_result_qna = new DataSet();
                    this.ds_summary_result_qna.Merge(dt);
                    this.ds_summary_result_qna.AcceptChanges();

                    dt.Clear();
                    dt.Dispose();
                    dt = null;
                }

                if (xml_doc != null)
                {
                    xml_doc.RemoveAll();
                    xml_doc = null;
                }

                xml_node_list = null;
            }
        }

        private void set_idliveq_req_result_qna()
        {
            DataTable dt = null;
            XmlDocument xml_doc = null;
            XmlNodeList xml_node_list = null;

            try
            {
                if (this.ds_idliveq_req_result_qna != null)
                {
                    this.ds_idliveq_req_result_qna.Clear();
                    this.ds_idliveq_req_result_qna.Dispose();
                    this.ds_idliveq_req_result_qna = null;
                }

                if (string.IsNullOrEmpty(this.idology_xml_ans_response).Equals(false))
                {
                    xml_doc = new XmlDocument();
                    xml_doc.LoadXml(this.idology_xml_ans_response);
                    xml_node_list = xml_doc.GetElementsByTagName("idliveq-result");

                    if (xml_node_list == null || xml_node_list.Count < 1)
                    {
                        return;
                    }

                    dt = new DataTable("idliveq_req_result_qna");
                    dt.Columns.Add("key", Type.GetType("System.String"));
                    dt.Columns.Add("message", Type.GetType("System.String"));

                    for (int i = 0; i < xml_node_list.Count; i++)
                    {
                        string xml = xml_node_list[i].OuterXml;

                        if (string.IsNullOrEmpty(xml).Equals(false))
                        {
                            XmlDocument xml_doc_inner = null;

                            xml_doc_inner = new XmlDocument();
                            xml_doc_inner.LoadXml(xml);

                            string key = xml_doc_inner.GetElementsByTagName("key").Item(0).InnerText;
                            string msg = xml_doc_inner.GetElementsByTagName("message").Item(0).InnerText;

                            xml_doc_inner.RemoveAll();
                            xml_doc_inner = null;

                            if (dt != null)
                            {
                                dt.Rows.Add(key, msg);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                this.idology_class_exception_ = e.Message;
            }
            finally
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    this.ds_idliveq_req_result_qna = new DataSet();
                    this.ds_idliveq_req_result_qna.Merge(dt);
                    this.ds_idliveq_req_result_qna.AcceptChanges();

                    dt.Clear();
                    dt.Dispose();
                    dt = null;
                }

                if (xml_doc != null)
                {
                    xml_doc.RemoveAll();
                    xml_doc = null;
                }

                xml_node_list = null;
            }

        }

        private void set_velocity_result_inq()
        {
            DataTable dt = null;
            XmlDocument xml_doc = null;
            XmlNodeList xml_node_list = null;

            try
            {

                if (this.ds_velocity_result_inq != null)
                {
                    this.ds_velocity_result_inq.Clear();
                    this.ds_velocity_result_inq.Dispose();
                    this.ds_velocity_result_inq = null;
                }

                if (string.IsNullOrEmpty(this.idology_xml_response).Equals(false))
                {
                    xml_doc = new XmlDocument();
                    xml_doc.LoadXml(this.idology_xml_response);
                    xml_node_list = xml_doc.GetElementsByTagName("velocity-result");

                    if (xml_node_list == null || xml_node_list.Count < 1)
                    {
                        return;
                    }

                    dt = new DataTable("velocity_result");
                    dt.Columns.Add("key", Type.GetType("System.String"));
                    dt.Columns.Add("message", Type.GetType("System.String"));

                    for (int i = 0; i < xml_node_list.Count; i++)
                    {
                        string xml = xml_node_list[i].OuterXml;

                        if (string.IsNullOrEmpty(xml).Equals(false))
                        {
                            XmlDocument xml_doc_inner = null;

                            xml_doc_inner = new XmlDocument();
                            xml_doc_inner.LoadXml(xml);

                            string key = xml_doc_inner.GetElementsByTagName("key").Item(0).InnerText;
                            string msg = xml_doc_inner.GetElementsByTagName("message").Item(0).InnerText;

                            xml_doc_inner.RemoveAll();
                            xml_doc_inner = null;

                            if (dt != null)
                            {
                                dt.Rows.Add(key, msg);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                this.idology_class_exception_ = e.Message;
            }
            finally
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    this.ds_velocity_result_inq = new DataSet();
                    this.ds_velocity_result_inq.Merge(dt);
                    this.ds_velocity_result_inq.AcceptChanges();

                    dt.Clear();
                    dt.Dispose();
                    dt = null;
                }

                if (xml_doc != null)
                {
                    xml_doc.RemoveAll();
                    xml_doc = null;
                }

                xml_node_list = null;
            }
        }

        private void set_iq_summary_result_qna()
        {
            DataTable dt = null;
            XmlDocument xml_doc = null;
            XmlNodeList xml_node_list = null;

            try
            {

                if (this.ds_iq_summary_result_qna != null)
                {
                    this.ds_iq_summary_result_qna.Clear();
                    this.ds_iq_summary_result_qna.Dispose();
                    this.ds_iq_summary_result_qna = null;
                }

                if (string.IsNullOrEmpty(this.idology_xml_ans_response).Equals(false))
                {
                    xml_doc = new XmlDocument();
                    xml_doc.LoadXml(this.idology_xml_ans_response);
                    xml_node_list = xml_doc.GetElementsByTagName("iq-summary-result");

                    if (xml_node_list == null || xml_node_list.Count < 1)
                    {
                        return;
                    }

                    dt = new DataTable("iq_summary_result_qna");
                    dt.Columns.Add("key", Type.GetType("System.String"));
                    dt.Columns.Add("message", Type.GetType("System.String"));

                    for (int i = 0; i < xml_node_list.Count; i++)
                    {
                        string xml = xml_node_list[i].OuterXml;

                        if (string.IsNullOrEmpty(xml).Equals(false))
                        {
                            XmlDocument xml_doc_inner = null;

                            xml_doc_inner = new XmlDocument();
                            xml_doc_inner.LoadXml(xml);

                            //string key = xml_doc_inner.GetElementsByTagName("key").Item(0).InnerText;
                            //string msg = xml_doc_inner.GetElementsByTagName("message").Item(0).InnerText;
                            string key = xml_doc_inner.InnerText;
                            string msg = xml_doc_inner.InnerText;

                            xml_doc_inner.RemoveAll();
                            xml_doc_inner = null;

                            if (dt != null)
                            {
                                dt.Rows.Add(key, msg);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                this.idology_class_exception_ = e.Message;
            }
            finally
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    this.ds_iq_summary_result_qna = new DataSet();
                    this.ds_iq_summary_result_qna.Merge(dt);
                    this.ds_iq_summary_result_qna.AcceptChanges();

                    dt.Clear();
                    dt.Dispose();
                    dt = null;
                }

                if (xml_doc != null)
                {
                    xml_doc.RemoveAll();
                    xml_doc = null;
                }

                xml_node_list = null;
            }
        }



        private void set_qualifiers_inq()
        {
            DataTable dt = null;
            XmlDocument xml_doc = null;
            XmlNodeList xml_node_list = null;

            try
            {
                if (this.ds_qualifier_inq != null)
                {
                    this.ds_qualifier_inq.Clear();
                    this.ds_qualifier_inq.Dispose();
                    this.ds_qualifier_inq = null;
                }

                if (string.IsNullOrEmpty(this.idology_xml_response_).Equals(false))
                {
                    xml_doc = new XmlDocument();
                    xml_doc.LoadXml(this.idology_xml_response_);
                    xml_node_list = xml_doc.GetElementsByTagName("qualifier");

                    if (xml_node_list == null || xml_node_list.Count < 1)
                    {
                        return;
                    }

                    dt = new DataTable("qualifiers_inq");
                    dt.Columns.Add("key", Type.GetType("System.String"));
                    dt.Columns.Add("message", Type.GetType("System.String"));

                    for (int i = 0; i < xml_node_list.Count; i++)
                    {
                        string xml = xml_node_list[i].OuterXml;

                        if (string.IsNullOrEmpty(xml).Equals(false))
                        {
                            XmlDocument xml_doc_inner = null;

                            xml_doc_inner = new XmlDocument();
                            xml_doc_inner.LoadXml(xml);

                            string key = xml_doc_inner.GetElementsByTagName("key").Item(0).InnerText;
                            string msg = xml_doc_inner.GetElementsByTagName("message").Item(0).InnerText;

                            xml_doc_inner.RemoveAll();
                            xml_doc_inner = null;

                            if (dt != null)
                            {
                                dt.Rows.Add(key, msg);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                this.idology_class_exception_ = e.Message;
            }
            finally
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    this.ds_qualifier_inq = new DataSet();
                    this.ds_qualifier_inq.Merge(dt);
                    this.ds_qualifier_inq.AcceptChanges();

                    dt.Clear();
                    dt.Dispose();
                    dt = null;
                }

                if (xml_doc != null)
                {
                    xml_doc.RemoveAll();
                    xml_doc = null;
                }

                xml_node_list = null;
            }
        }

        private void set_questionaire_response()
        {
            DataTable dt_q_and_a = null;

            if (this.ds_question_and_ans != null)
            {
                this.ds_question_and_ans.Clear();
                this.ds_question_and_ans.Dispose();
                this.ds_question_and_ans = null;
            }

            if (string.IsNullOrEmpty(this.idology_xml_response_) == false)
            {
                try
                {
                    XmlDocument xml_doc = null;
                    XmlNodeList xml_node_list = null;

                    xml_doc = new XmlDocument();
                    xml_doc.LoadXml(this.idology_xml_response_);

                    xml_node_list = xml_doc.GetElementsByTagName("question");

                    if (xml_doc == null)
                    {
                        return;
                    }

                    if (xml_node_list == null)
                    {
                        return;
                    }

                    dt_q_and_a = new DataTable("questionaire_response");
                    dt_q_and_a.Columns.Add("question_prompt", Type.GetType("System.String"));
                    dt_q_and_a.Columns.Add("question_type", Type.GetType("System.String"));
                    dt_q_and_a.Columns.Add("question_answer", Type.GetType("System.String"));

                    for (int i = 0; i < xml_node_list.Count; i++)
                    {
                        string outer_xml = xml_node_list[i].OuterXml;

                        XmlDocument xml_doc_inner = null;

                        if (string.IsNullOrEmpty(outer_xml).Equals(false))
                        {
                            xml_doc_inner = new XmlDocument();
                            xml_doc_inner.LoadXml(outer_xml);

                            string prompt_string = xml_doc_inner.GetElementsByTagName("prompt").Item(0).InnerText;
                            string type_string = xml_doc_inner.GetElementsByTagName("type").Item(0).InnerText;

                            XmlNodeList xml_node_list_2 = xml_doc_inner.GetElementsByTagName("answer");

                            for (int j = 0; j < xml_node_list_2.Count; j++)
                            {
                                string answer_string = xml_node_list_2.Item(j).InnerText;

                                dt_q_and_a.Rows.Add(prompt_string, type_string, answer_string);
                            }

                            xml_node_list_2 = null;
                        }

                        if (xml_doc_inner != null)
                        {
                            xml_doc_inner.RemoveAll();
                            xml_doc_inner = null;
                        }
                    }

                    if (xml_doc != null)
                    {
                        xml_doc.RemoveAll();
                        xml_doc = null;
                    }

                    xml_node_list = null;
                }
                catch (Exception e)
                {
                    this.idology_class_exception_ = e.Message;
                }
                finally
                {
                    if (dt_q_and_a != null && dt_q_and_a.Rows.Count > 0)
                    {
                        this.ds_question_and_ans = new DataSet();
                        this.ds_question_and_ans.Merge(dt_q_and_a);
                        this.ds_question_and_ans.AcceptChanges();

                        dt_q_and_a.Clear();
                        dt_q_and_a.Dispose();
                        dt_q_and_a = null;
                    }
                }
            }
        }

        private void get_error_inq()
        {
            XmlDocument xml_doc = null;
            XmlNodeList xml_node_list = null;

            if (string.IsNullOrEmpty(this.idology_xml_response_).Equals(false))
            {
                xml_doc = new XmlDocument();
                xml_doc.LoadXml(this.idology_xml_response_);
                xml_node_list = xml_doc.GetElementsByTagName("error");

                if (xml_node_list.Item(0) != null && string.IsNullOrEmpty(xml_node_list.Item(0).InnerText).Equals(false))
                {
                    this.idology_class_exception_ = xml_node_list.Item(0).InnerText;
                }
            }
        }
    }
}
