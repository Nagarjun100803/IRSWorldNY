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
    public partial class GenericStatusInquiry : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(GenericStatusInquiry));
        }



        #region AJAX METHODS
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet load_lookup_values()
        {            
            return this.IRSGENERICSTATUS.load_lookup_values();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet search_genericstatus(string todate,string fromdate,string status)
        {

            int role = IRSClientData.Role ;
            return this.IRSGENERICSTATUS.load_search_values(
                    Convert.ToDateTime(todate),
                    Convert.ToDateTime(fromdate),
                    status ,role
                );
        }
        #endregion
        #region PUBLIC METHODS

        #endregion
        #region PRIVATE METHODS

        private irsGenericStatusInquiry IRSGENERICSTATUS
        {
            get
            {
                if (Session["IRS_GENERICSTATUS"] == null)
                {
                    Session["IRS_GENERICSTATUS"] = new irsGenericStatusInquiry("1170",IRSClientData.Role);
                }
                return (irsGenericStatusInquiry)Session["IRS_GENERICSTATUS"];
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
        #endregion

    }
}
