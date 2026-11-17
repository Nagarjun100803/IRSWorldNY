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

namespace IRSWorld.GRS
{
    public partial class GRSAOGEFUEXTRACT : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(GRSAOGEFUEXTRACT));
            
        }

        private irsIOD IRSIOD  
        {
            get
            {
                if (Session["IRS_IOD"] == null)
                {
                    Session["IRS_IOD"] = new irsIOD();
                }
                return (irsIOD)Session["IRS_IOD"];
            }
        }
        private irsClientData IRSClientData
        {
            get
            {
                if (Session["IRS_CLIENT_DATA"] != null)
                {
                    return ((irsClientData)Session["IRS_CLIENT_DATA"]);
                }

                return null;
            }
        }
    }
}
