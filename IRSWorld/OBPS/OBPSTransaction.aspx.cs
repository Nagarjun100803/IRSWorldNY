using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using IRSClassLib;

namespace IRSWorld.OBPS
{
    public partial class OBPSTransaction : System.Web.UI.Page
    {
        private irsOBPSTransaction irsOBPSTransaction
        {
            get
            {
                if (Session["OBPSTransaction"] == null)
                {
                    Session["OBPSTransaction"] = new irsOBPSTransaction();
                }
                return (irsOBPSTransaction)Session["OBPSTransaction"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(OBPSTransaction));
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void close_session()
        {
            Session.Abandon();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public void drop_irsUsers()
        {
            Session["IRS_USERS"] = null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet getRemitterInfo()
        {
            return irsOBPSTransaction.getRemitterInfo();
        }


    }
}
