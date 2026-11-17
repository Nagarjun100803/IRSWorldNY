using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using IRSClassLib;
using System.Net;
using System.Net.Security;
using System.Globalization;
using System.Threading;

namespace IRSWorld
{
    public class Global : System.Web.HttpApplication
    {
	/* E2EAPICHANGE 20220628 ADDED CODE */
        protected void Application_PreSendRequestHeaders()
        {
            Response.Headers.Remove("Server");
            Response.Headers.Remove("X-AspNet-Version");
            Response.Headers.Remove("X-AspNetMvc-Version");
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            /*
            if (ServicePointManager.SecurityProtocol.HasFlag(SecurityProtocolType.Tls12) == false)
            {
                ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol | SecurityProtocolType.Tls12;
            }
            */
            /* PNBS Integration Project */
            if (ConfigurationManager.AppSettings["EnableTLS"] == "1")
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                // TLS Handling
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                       | (SecurityProtocolType)768  /*Tls11*/
                       | (SecurityProtocolType)3072 /*Tls12*/
                       | SecurityProtocolType.Ssl3;
            }
        }

        public void Session_OnStart()
        {
            Session.RemoveAll();
            Session["IRS_CLIENT_DATA"] = new IRSClassLib.irsClientData();
            Session["IRS_AUDIT_LOGGER"] = new IRSClassLib.irsAuditLogger();
        }

        private void closeSessionById(String sesid)
        {
            //System.IO.StreamWriter stm = new System.IO.StreamWriter(@"c:\temp\session.txt", true);
            //stm.WriteLine("Session_OnEnd: " + sesid);

            irsSqlCommonsWs coms = new irsSqlCommonsWs(true);
            bool is_loggedon = false;
            /* VAPT IAG 20170515 */
            bool is_winapp = false;
            is_winapp = coms.IsWinApp;
            coms.IsWinApp = true;
            Object o = coms.execScalar("SELECT LoggedOn FROM tbl_users WHERE CurrentSessionID = '" + sesid + "'");
            coms.IsWinApp = is_winapp;
            is_winapp = false;

            try
            {
                is_loggedon = Convert.ToBoolean(o);
            }
            catch { }
            // stm.WriteLine("is logged on: " + is_loggedon);
            if (is_loggedon)
            {
                /* VAPT IAG 20170515 */
                is_winapp = coms.IsWinApp;
                coms.IsWinApp = true;
                int ra = coms.sqlUpdate("update tbl_users set LoggedOn = 0 where CurrentSessionID = '" + sesid + "'");
                coms.IsWinApp = is_winapp;

                // stm.WriteLine("logoff: " + ra + " " + coms.Message);
            }
            coms = null;
            // stm.Close();
        }

        public void Session_OnEnd()
        {
            try
            {
                if (Session["IRS_CLIENT_DATA"] != null)
                {
                    irsClientData icd = (IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"];
                    if (icd.UserId != null && icd.UserId.Length > 0)
                    {
                        icd.logout();
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(Session.SessionID))
                    {
                        closeSessionById(Session.SessionID);
                    }
                }
            }
            catch { }

            try
            {
                Session["IRS_CLIENT_DATA"] = null;
                Session["IRS_AUDIT_LOGGER"] = null;
                Session.RemoveAll();
            }
            catch
            { }
        }

        protected void Application_End(object sender, EventArgs e)
        {
            Session_OnEnd();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            CultureInfo newCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            newCulture.DateTimeFormat.ShortDatePattern = "MM-dd-yyyy";
            newCulture.DateTimeFormat.DateSeparator = "/";
            Thread.CurrentThread.CurrentCulture = newCulture;
        }
    }
}