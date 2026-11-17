using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.XPath;
using System.Data;
namespace IRSClassLib
{
    public class ACHWorks : sqlHelperWs
    {

        public ACHWorks()
        { }
        
        public bool connect_achworks(string remitterid,string accountno, string routingno, string bankname, string swift,string amount)
        {
            string verificationkey = new irsConfiguration().appsetting("verificationkey");
            string valid_achreturn = new irsConfiguration().appsetting("validresponsecode");


            WebACHWorks.CompanyInfo myComp = new WebACHWorks.CompanyInfo();
            
            myComp.Company = new irsConfiguration().appsetting("companyname");
            myComp.CompanyKey = new irsConfiguration().appsetting("companykey");

            //myComp.SSS = new irsConfiguration().appsetting("sss");
            myComp.LocID = new irsConfiguration().appsetting("locid");

            WebACHWorks.Iverservice ws = new WebACHWorks.Iverservice();
            ws.Proxy = WebProxy.GetDefaultProxy();
            ws.Proxy.Credentials = CredentialCache.DefaultCredentials;


            string response;
            try
            {
                ws.ConnectionCheck(myComp.LocID, myComp.Company, myComp.CompanyKey);
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("webremituser", "Web Remittance ACHWorks", ex.StackTrace + "-" + ex.Message, "Failed to check connection: " +  ex.Message, ex.Source, csAuditLog.Category.Select);
                Message = ex.Message;
                return false;
            }
            

            WebACHWorks.CustomerInfo customer = getRemitterDetails(remitterid);
            

            WebACHWorks.AcctInfo acct = new WebACHWorks.AcctInfo();
            acct.AcctNo = accountno;
            acct.AcctType = "C";
            acct.Amount = amount;
            acct.RoutingNo = routingno;

            WebACHWorks.ResultVer result = new WebACHWorks.ResultVer();

            try
            {
                result = ws.TSSVerify(verificationkey, myComp, customer, acct);
                string[] res = result.CKVXML.Split('\n');
                string response_code = "";
                if (res.Length >= 4)
                {
                    response_code = res[4].Replace("<code>", "").Replace("</code>", "");
                }
                new csAuditLog().writeAudit(((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId, "Web Remittance ACHWorks", response_code, result.CKVXML, "Success: " + result.ResultVerXML, csAuditLog.Category.Select);
                return (valid_achreturn.IndexOf(response_code.Trim()) > -1);
            }
            catch(Exception ex)
            {
                new csAuditLog().writeAudit(((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId, "Web Remittance ACHWorks", ex.StackTrace + "-" + ex.Message, result.CKVXML, ex.Source, csAuditLog.Category.Select);
                return false;
            }

            return false;
            
        }


        public  WebACHWorks.CustomerInfo getRemitterDetails(string rem_id)
        {
            WebACHWorks.CustomerInfo cust = new WebACHWorks.CustomerInfo();
            DataSet dsSSNDetails = getDataSet(getSqlCommand("select top 1 IDNo from tbl_cif_ids where idtype='15' and cifid = '" + rem_id + "'", false));
            if (dsSSNDetails.Tables[0].Rows.Count > 0)
            {
                DataSet dsMaster = getDataSet(getSqlCommand("select top 1 * from tbl_cif_master where cifid = '" + rem_id + "'", false));
                cust.SSN = Convert.ToString(dsSSNDetails.Tables[0].Rows[0]["IDNo"]);
                cust.DateOfBirth = Convert.ToDateTime(dsMaster.Tables[0].Rows[0]["BirthDate"]).ToString("MM/dd/yyyy");
                cust.CustomerName = Convert.ToString(dsMaster.Tables[0].Rows[0]["FullName"]);

            }
            return cust;
        }

        

    }
}
