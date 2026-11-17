using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
    [Serializable]
    public class csPagIbigAPI
    {
        public DataSet dset = null;
        public string api_response_msg = string.Empty;
        private string get_request_string(string member_id, string pagibig_type)
        {           

            string s = string.Empty;
            string api_u = string.Empty;
            string api_p = string.Empty;
            string api_xlmns = string.Empty;
            string api_inquire_xlmns = string.Empty;

            string pay_type = string.Empty;
            string reference_id = string.Empty;

            reference_id = member_id;
            pay_type = pagibig_type;

            api_u = new System.Configuration.AppSettingsReader().GetValue("PAGIBIG_APIU", Type.GetType("System.String")).ToString();
            api_p = new System.Configuration.AppSettingsReader().GetValue("PAGIBIG_APIP", Type.GetType("System.String")).ToString();
            api_xlmns = new System.Configuration.AppSettingsReader().GetValue("PAGIBIG_APIXMLNS", Type.GetType("System.String")).ToString();
            api_inquire_xlmns = new System.Configuration.AppSettingsReader().GetValue("PAGIBIG_APIINQUIREXMLNS", Type.GetType("System.String")).ToString();

            s = @"<?xml version = ""1.0"" encoding = ""utf-8""?>
                        
                              <soap:Envelope xmlns:soap=""" + api_xlmns + @""">
                                <soap:Body>
                                        <Inquire_ID xmlns=""" + api_inquire_xlmns + @""">
                                                <userID>" + api_u + @"</userID>
                                                <pass>" + api_p + @"</pass>
                                                <paytype>" + pay_type + @"</paytype>
                                                <referenceID>" + reference_id + @"</referenceID>
                                        </Inquire_ID>
                                </soap:Body>
                        </soap:Envelope>";
            return s;
        }

        public string api_response_message()
        {
            return api_response_msg;
        }
        public DataTable get_pag_ibig_member_information()
        {
            try
            {
                if (dset != null && dset.Tables.Count > 0)
                {
                    return dset.Tables["member_information"];
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public string inquire_pagibig(string member_id, string pagibig_merchantcode)
        {           
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            System.Net.IWebProxy iwproxy = WebRequest.GetSystemWebProxy();
            HttpWebRequest request = null;           

            string url_string = string.Empty;
            string xml_string = string.Empty;
            string pagibig_type = string.Empty;
            string api_soap_action = string.Empty;


            try
            {
                api_response_msg = string.Empty;
                api_soap_action = string.Empty;
                api_soap_action = new System.Configuration.AppSettingsReader().GetValue("PAGIBIG_APISOAPACTION", Type.GetType("System.String")).ToString();


                switch (pagibig_merchantcode)
                {
                    case "PAG-IBIG CONTRIBUTION":
                        pagibig_type = "MC"; break;
                    case "PAG-IBIG MP2 SAVINGS":
                        pagibig_type = "M2"; break;
                    case "PAG-IBIG HOUSING - HAN":
                        pagibig_type = "HL"; break;
                    case "PAG-IBIG HOUSING - PRN":
                        pagibig_type = "HL"; break;
                    case "PAG-IBIG SHORT TERM LOAN":
                        pagibig_type = "ST"; break;
                    case "PAG-IBIG CALAMITY LOAN":
                        pagibig_type = "CL"; break;
                    default:
                        pagibig_type = "MC"; break;
                }

                try
                {

                    sqlHelperWs sqlkonek = new sqlHelperWs();

                    string tls_return = sqlkonek.getDataSet("select count(1) cnt_count from tbl_lookup with (nolock) where fldgroup = 'PAGIBIGAPITLSTOGGLE' and fldcode = '1'", false, new object[] { }).Tables[0].Rows[0]["cnt_count"].ToString();
                    
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

                url_string = new System.Configuration.AppSettingsReader().GetValue("PAGIBIG_APIURL", Type.GetType("System.String")).ToString();
                xml_string = this.get_request_string(member_id, pagibig_type);               

                XmlDocument xm = new XmlDocument();
                xm.LoadXml(xml_string);

                request = (HttpWebRequest)WebRequest.Create(url_string);
                request.Method = "POST";
                request.ContentType = "text/xml; charset=utf-8";                                
                request.Proxy = iwproxy;
                request.Credentials = CredentialCache.DefaultCredentials;
                request.Proxy.Credentials = CredentialCache.DefaultCredentials;
                request.Headers.Add("SOAPAction", api_soap_action);

                using (Stream requestStream = request.GetRequestStream())
                {
                    xm.Save(requestStream);
                }

                using (HttpWebResponse responde = (HttpWebResponse)request.GetResponse())
                {
                    string s = new StreamReader(responde.GetResponseStream()).ReadToEnd();

                    if (s != null && s != string.Empty)
                    {
                        dset = new DataSet();
                        dset.ReadXml( new XmlTextReader(new StringReader(s)));                
                    }
                }

                if (dset != null && dset.Tables["inquire_idresult"].Rows.Count > 0)
                {
                    if (Convert.ToBoolean(dset.Tables["inquire_idresult"].Rows[0]["isGet"]) == true)
                    {                        
                        api_response_msg = dset.Tables["inquire_idresult"].Rows[0]["resMsg"].ToString(); this.log_msg(api_response_msg + " VALID | " + member_id + " | " + pagibig_type);  return "OK";
                    }

                    if (Convert.ToBoolean(dset.Tables["inquire_idresult"].Rows[0]["isGet"]) == false)
                    {
                        this.log_msg("ERROR: " + dset.Tables["inquire_idresult"].Rows[0]["resMsg"].ToString() + " INVALID | " + member_id + " | " + pagibig_type);  return "ERROR: " + dset.Tables["inquire_idresult"].Rows[0]["resMsg"].ToString();                        
                    }
                } 
                       
                return "ERROR: Unable to validate entered PAG-IBIG Member / Loan / Account number";
            }
            catch(Exception e)
            {
                this.log_msg(e.Message); return "ERROR: " + e.Message;
            }
        }
        public void log_msg(string s)
        {
            csAuditLog audit = null;

            try
            {
                audit = new csAuditLog();
                audit.writeAudit("", "PAGIBIG API", s, "", "", csAuditLog.Category.Insert);
            }
            catch
            {
            }
        }
    }
}
