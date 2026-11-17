using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using IRSClassLib;

namespace IRSWorld
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {}

        public void Session_OnStart()
        {
            if (Session["IRS_CLIENT_DATA"] == null)
            {
                Session["IRS_CLIENT_DATA"] = new IRSClassLib.irsClientData();
                Session["IRS_AUDIT_LOGGER"] = new IRSClassLib.irsAuditLogger();
            }
        }

        public void Session_OnEnd()
        {
            try
            {
                Session["IRS_CLIENT_DATA"] = null;
                Session["IRS_AUDIT_LOGGER"] = null;
                Session.Clear();
            }
            catch 
            {}
        }

        protected void Application_End(object sender, EventArgs e)
        {
            Session_OnEnd();
        }
    }
}