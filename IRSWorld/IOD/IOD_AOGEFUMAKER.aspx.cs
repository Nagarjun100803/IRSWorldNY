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
    public partial class IOD_AOGEFUMAKER : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(IOD_AOGEFUMAKER));
            DateTime tranxdate = new irsTimeZone().branchLocalTime(IRSClientData.Location);
            this.hfTranxDate.Value = tranxdate.ToShortDateString();
        }
        private irsGRSBatchEntries IRSGRSBatchEntry
        {
            get
            {
                if (Session["IRS_GRSBATCHENTRIES"] == null)
                {
                    Session["IRS_GRSBATCHENTRIES"] = new irsGRSBatchEntries();
                }
                return (irsGRSBatchEntries)Session["IRS_GRSBATCHENTRIES"];
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
       

        #region AJAX PUBLIC METHODS
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet load_lookup_values()
        {
            DateTime tranxdate = new irsTimeZone().branchLocalTime(IRSClientData.Location);
            return this.IRSGRSBatchEntry.load_lookup_values(tranxdate, "1170");         

            
        }
        

        #endregion
        #region PRIVATE METHODS
       
        #endregion
    }
}
