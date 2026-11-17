////////////////////////////////////////////////
/*     CREATED BY MICHAEL RAY C. DIZON        */
/*     API CLASS FOR SSS PRN                  */
/*     FEBRUARY 13, 2018                      */
/*     VERSION 1.0                            */
///////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.IO;
using System.Xml;
using System.Configuration;
using System.Data;
namespace IRSClassLib
{
    public class SSSPRN_API
    {

        /*Enumerate type of SSS Call*/
        public enum SSSPayment
        {
            INQUIRE_PRN,
            INQUIRE_VIA_SSS,
            SUBMIT_PRN

        }

        /*Declaring private member to be invoke by property*/
        private string _prnnumber;
        private string _username;
        private string _pw;
        private string _referenceno;
        private string _channel_id;
        private string _sss_number;
        private string _bankcode;
        private string _birthdate;
        private string _transDate;
        private string _uri;
        private string _tranno;
        private string _partner_payment_type;

        /*Public property for PRNumber*/
        public string PRNNumber
        {
            get
            {
                return _prnnumber;
            }
            set { _prnnumber = value; }
        }

        /*Public property for Username*/
        public string Username
        {
            get
            {
                return _username;
            }
            set { _username = value; }
        }

        /*Public property for Password*/
        public string Password
        {
            get
            {
                return _pw;
            }
            set { _pw = value; }
        }

        /*Public property for SSSNo*/
        public string SSSNo
        {
            get
            {
                return _sss_number;
            }
            set { _sss_number = value; }
        }

        /*Public property for Birthdate*/
        public string Birthdate
        {
            get
            {
                return _birthdate;
            }
            set { _birthdate = value; }
        }

        /*Public property for BankCode*/
        public string BankCode
        {
            get
            {
                return _bankcode;
            }
            set { _bankcode = value; }
        }
        public string ChannelID
        {
            get
            {
                return _channel_id;
            }
            set { _channel_id = value; }
        }

        /*Public property for TransactionDate*/
        public string TransactionDate
        {
            get
            {
                return _transDate;
            }
            set { _transDate = value; }
        }

        /*Public property for TransactionDate*/
        public string TransactionNumber
        {
            get
            {
                return _tranno;
            }
            set { _tranno = value; }
        }


        /*Public property for URI*/
        public string URI
        {
            get
            {
                return _uri;
            }
            set { _uri = value; }
        }

        public string SSS_TEST
        {
            get { return new System.Configuration.AppSettingsReader().GetValue("SSS_TEST", Type.GetType("System.String")).ToString(); }
        }

        public string PartnerPaymentType
        {
            get
            {
                return _partner_payment_type;
            }

            set
            {
                _partner_payment_type = value;
            }
        }

        /*Instantiate public class SSS PRN API*/
        public SSSPRN_API()
        {
            URI = new AppSettingsReader().GetValue("SSS_URI", Type.GetType("System.String")).ToString();
            Username = new AppSettingsReader().GetValue("SSS_UName", Type.GetType("System.String")).ToString();
            Password = new AppSettingsReader().GetValue("SSS_PW", Type.GetType("System.String")).ToString();
            ChannelID = new AppSettingsReader().GetValue("SSS_CHANNELID", Type.GetType("System.String")).ToString();
            BankCode = new AppSettingsReader().GetValue("SSS_BANKCODE", Type.GetType("System.String")).ToString();
            PartnerPaymentType = new AppSettingsReader().GetValue("SSS_PARTNER_PAYMENT_TYPE", Type.GetType("System.String")).ToString();
        }

        /*XML Request when calling Submit PRN*/
        private string SubmitPRN()
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
                <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"">
                <soapenv:Header>
                 <AuthInfo>
                        <userName>" + _username + @"</userName>
                        <userPass>" + _pw + @"</userPass>
                 </AuthInfo>
                </soapenv:Header>
                  <soapenv:Body>
                      <subIndivPRNReq>
                         <channelID>" + _channel_id + @"</channelID>
                         <payRefNumber>" + _prnnumber + @"</payRefNumber >
                         <bankCode>" + _bankcode + @"</bankCode >
                        <tranNumber>" + _tranno + @"</tranNumber>
                        <partnerPaymentType>" + _partner_payment_type + @"</partnerPaymentType>
                         <payDateTime>" + Convert.ToDateTime(_transDate).ToString("MM/dd/yyyy HH:mm:ss") + @"</payDateTime>
                      </subIndivPRNReq>
                   </soapenv:Body>
                 </soapenv:Envelope>";

            this.log_msg("SubmitPRN: " + xml);

            return xml;
        }

        /*XML Request when calling Inquire PRN*/
        private string InquirePRN()
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
                <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"">
                <soapenv:Header>
                 <AuthInfo>
                        <userName>" + _username + @"</userName>
                        <userPass>" + _pw + @"</userPass>
                 </AuthInfo>
                </soapenv:Header>
                  <soapenv:Body>
                      <inqIndivPRNReq>
                         <channelID>" + _channel_id + @"</channelID>
                         <indivPayRefNumber>" + _prnnumber + @"</indivPayRefNumber>
                         <bankCode>" + _bankcode + @"</bankCode >
                      </inqIndivPRNReq>
                   </soapenv:Body>
                 </soapenv:Envelope>";

            this.log_msg("InquirePRN: " + xml);
            return xml;
        }

        /*XML Request when calling SSSNumber*/
        private string InquireViaSSSNumber()
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
                <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"">
                <soapenv:Header>
                 <AuthInfo>
                        <userName>" + _username + @"</userName>
                        <userPass>" + _pw + @"</userPass>
                 </AuthInfo>
                </soapenv:Header>
                  <soapenv:Body>
                       <inqIndivSSReq>
                         <channelID>" + _channel_id + @"</channelID>
                         <ssNumber>" + _sss_number + @"</ssNumber>
                         <bankCode>" + _bankcode + @"</bankCode>
                         <memberDOB>" + Convert.ToDateTime(_birthdate).ToString("MM/dd/yyyy") + @"</memberDOB>
                      </inqIndivSSReq>
                   </soapenv:Body>
                 </soapenv:Envelope>";

            this.log_msg("InquireViaSSSNumber: " + xml);
            return xml;
        }

        private string response_data(string urn, string xmlresponse)
        {
            string ixmlresponse = @"<?xml version=""1.0"" encoding=""utf - 8""?><inqPRNResp><txnStatus><replyCode>0</replyCode><replyText> E - PRN is found on masterfile</replyText><transDate>02/12/2018 13:41:17</transDate></txnStatus><payRefNumber>50118000076398</payRefNumber><employeeIDNumber>0394917207</employeeIDNumber><employerName>LENIHAN ENTERPRISES</employerName><employerBranch>000</employerBranch><amount>1,495.00</amount><appMonth>012018</appMonth></inqPRNResp>";
            string ixmlresponse2 = @"<?xml version=""1.0"" encoding=""utf - 8""?><inqIndivSSNumResp><txnStatus><replyCode>0</replyCode><replyText>I-PRN is found on masterfile</replyText><transDate>02/08/2018 16:02:00</transDate></txnStatus><indivPayRefNumber>S0218000002582</indivPayRefNumber><employeeIDNumber>3412119337</employeeIDNumber><employeeName>FELIPE PNB MR</employeeName><amount>110</amount><appMonthFrom>012018</appMonthFrom><appMonthTo>012018</appMonthTo></inqIndivSSNumResp>";
            string ixmlresponse3 = @"<?xml version=""1.0"" encoding=""utf - 8""?><inqIndivSSNumResp><txnStatus><replyCode>99</replyCode><replyText>Authentication Error</replyText></txnStatus></inqIndivSSNumResp>";
            DataSet ds = null;
            if (this.SSS_TEST == "1")
            {
                if (urn.IndexOf("InquirePRN") > -1)
                {
                    xmlresponse = ixmlresponse;
                }
                else if (urn.IndexOf("InquireIndividualSSNum") > -1)
                {
                    xmlresponse = ixmlresponse2;
                }
                else
                {
                    xmlresponse = ixmlresponse3;
                }

            }


            if (urn.IndexOf("InquirePRN") > -1)
            {
                ds = this.ConvertXMLToDataSet(xmlresponse);
            }
            else
            {
                ds = this.ConvertXMLToDataSet(xmlresponse);
            }

            DataTable dt1 = null;
            DataTable dt2 = null;
            string decodedString = "";
            if (ds != null)
            {
                if (ds.Tables.Count > 1)
                {

                    dt1 = ds.Tables[0];
                    dt2 = ds.Tables[1];
                    if (urn.IndexOf("InquireIndividualPRN") > -1)
                    {
                        if (dt1.Columns.Count >= 6)
                        {
                            //decodedString = Convert.ToString(dt2.Rows[0][1]) + "~" + Convert.ToString(dt1.Rows[0][0]) + "~" + Convert.ToString(dt1.Rows[0][1]) + "~" + Convert.ToString(dt1.Rows[0][2]) + "~" + Convert.ToString(dt1.Rows[0][3]) + "~" + Convert.ToString(dt1.Rows[0][4]) + "~" + Convert.ToString(dt1.Rows[0][4]) + "~" + dt1.Rows[0][6].ToString().Substring(0, 2) + "/" + dt1.Rows[0][6].ToString().Substring(2, 2) + "/" + dt1.Rows[0][6].ToString().Substring(4, 2);
                            decodedString = Convert.ToString(dt2.Rows[0][1]) + "~" + Convert.ToString(dt1.Rows[0][0]) + "~" + Convert.ToString(dt1.Rows[0][1]) + "~" + Convert.ToString(dt1.Rows[0][2]) + "~" + Convert.ToString(dt1.Rows[0][3]) + "~" + Convert.ToString(dt1.Rows[0][4]) + "~" + Convert.ToString(dt1.Rows[0][4]) + "~" + dt1.Rows[0][5].ToString().Substring(0, 2) + "/" + dt1.Rows[0][5].ToString().Substring(2, 2) + "/" + dt1.Rows[0][5].ToString().Substring(4, 2) + "~" + dt1.Rows[0][6].ToString().Substring(0, 2) + "/" + dt1.Rows[0][6].ToString().Substring(2, 2) + "/" + dt1.Rows[0][6].ToString().Substring(4, 2);
                        }
                        else
                        {
                            decodedString = Convert.ToString(dt2.Rows[0][1]) + "~" + Convert.ToString(dt2.Rows[0][0]);
                        }
                    }
                    else if (urn.IndexOf("InquireIndividualSSNum") > -1)
                    {
                        if (dt1.Columns.Count >= 6)
                        {
                            decodedString = Convert.ToString(dt2.Rows[0][1]) + "~" + Convert.ToString(dt1.Rows[0][0]) + "~" + Convert.ToString(dt1.Rows[0][1]) + "~" + Convert.ToString(dt1.Rows[0][2]) + "~" + Convert.ToString(dt1.Rows[0][3]) + "~" + Convert.ToString(dt1.Rows[0][4]) + "~" + Convert.ToString(dt1.Rows[0][4]) + "~" + dt1.Rows[0][5].ToString().Substring(0, 2) + "/" + dt1.Rows[0][5].ToString().Substring(2, 2) + "/" + dt1.Rows[0][5].ToString().Substring(4, 2) + "~" + dt1.Rows[0][6].ToString().Substring(0, 2) + "/" + dt1.Rows[0][6].ToString().Substring(2, 2) + "/" + dt1.Rows[0][6].ToString().Substring(4, 2);
                        }
                        else
                        {
                            decodedString = Convert.ToString(dt2.Rows[0][1]) + "~" + Convert.ToString(dt2.Rows[0][0]);
                        }

                    }
                    else
                    {
                        decodedString = Convert.ToString(dt1.Rows[0][1]) + "~" + Convert.ToString(dt1.Rows[0][0]);
                    }

                    return decodedString;
                }
                else
                {
                    dt1 = ds.Tables[0];
                    if (dt2 != null)
                    {
                        decodedString = Convert.ToString(dt2.Rows[0][1]) + "~" + Convert.ToString(dt2.Rows[0][0]);
                    }
                    else
                    {
                        decodedString = Convert.ToString(dt1.Rows[0][1]) + "~" + Convert.ToString(dt1.Rows[0][0]);
                    }

                    return decodedString;
                }
            }
            return string.Empty;
        }

        /*Post Requested XML*/
        private string RequestToPost(string xml, string urn)
        {
            string servicepointmgr = string.Empty;

            try
            {
                servicepointmgr = new AppSettingsReader().GetValue("SSS_ServicePointManager", Type.GetType("System.String")).ToString();

                if (servicepointmgr != null && servicepointmgr.Trim() == "1")
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                }                
            }
            catch
            {
            }


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_uri);
            request.Method = "POST";

            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            request.ContentType = "text/xml; charset=utf-8";

            XmlDocument xm = new XmlDocument();
            xm.LoadXml(xml);

            request.Proxy = WebProxy.GetDefaultProxy();
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Proxy.Credentials = CredentialCache.DefaultCredentials;
            request.Headers.Add("SOAPAction", urn);
            /*FOR TESTING*/
            if (this.SSS_TEST == "1")
            {
                return response_data(urn, "");
            }

            /*FOR TESTING*/

          

            using (Stream dataStream = request.GetRequestStream())
            {
                xm.Save(dataStream);
            }
            long length = 0;
            try
            {
                string decodedString = "";
                // return response_data(urn, decodedString);
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    //length = response.ContentLength;
                    string s = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    xm.LoadXml(s);
                    byte[] data = Convert.FromBase64String(xm.ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText);
                    decodedString = Encoding.UTF8.GetString(data);
                    decodedString = response_data(urn, decodedString);
                    /*sample only*/

                    /*sample only*/
                    this.log_msg("RequestToPost: " + decodedString);
                    return decodedString;
                }
            }
            catch (WebException wex)
            {
                try
                {
                    string pageContent = new StreamReader(wex.Response.GetResponseStream()).ReadToEnd().ToString();
                    this.log_msg("RequestToPost: " + pageContent);                    
                    return pageContent;
                }
                catch(Exception e)
                {
                    this.log_msg("RequestToPost: " + e.Message);
                    return e.Message;
                }

                //csAuditLog audit = new csAuditLog();
                //string pageContent = new StreamReader(wex.Response.GetResponseStream()).ReadToEnd().ToString();
                //audit.writeAudit("", "SSS PRN", pageContent, "", "", csAuditLog.Category.Insert);
                //return pageContent;
            }
            //return string.Empty;
        }

        /*Submit Request, class Request To Post method to inquire with API*/
        public string submitRequest(SSSPayment payment)
        {
            if (payment == SSSPayment.INQUIRE_PRN)
            {
                return this.RequestToPost(InquirePRN(), "urn:InquireIndividualPRN");
            }
            else if (payment == SSSPayment.INQUIRE_VIA_SSS)
            {
                return this.RequestToPost(InquireViaSSSNumber(), "urn:InquireIndividualSSNum");
            }
            else if (payment == SSSPayment.SUBMIT_PRN)
            {
                return this.RequestToPost(SubmitPRN(), "urn:SubmitIndividualPRN");
            }
            return string.Empty;
        }

        // Function to convert passed XML data to dataset
        public DataSet ConvertXMLToDataSet(string xmlData)
        {
            StringReader stream = null;
            XmlTextReader reader = null;
            try
            {
                DataSet xmlDS = new DataSet();
                stream = new StringReader(xmlData);
                // Load the XmlTextReader from the stream
                reader = new XmlTextReader(stream);
                xmlDS.ReadXml(reader);
                return xmlDS;
            }
            catch(Exception e)
            {
                this.log_msg("ConvertXMLToDataSet: " + e.Message);
                return null;
            }
            finally
            {
                if (reader != null) reader.Close();
            }
        }// Use this function to get XML string from a dataset

        public String PesoFund(string val)
        {
            try
            {
                double amt = Convert.ToDouble(val);
                if (amt < 1000)
                {
                    return "1000";
                }
                else if((amt % 100) == 0)
                {
                    return "valid";
                }
            }
            catch (Exception ex)
            {
                this.log_msg("PesoFund: " + ex.Message);
            }

            return "0";

        }

        public void log_msg(string s)
        {
            csAuditLog audit = null;

            try
            {
                audit = new csAuditLog();
                audit.writeAudit("", "SSS PRN", s, "", "", csAuditLog.Category.Insert);
            }
            catch
            {
            }                        
        }
    }
}
