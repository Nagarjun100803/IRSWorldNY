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

namespace IRSWorld.GFC
{
    public partial class GFCSearch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(GFCSearch));
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool UnlockCK(String ckAccount)
        {
            try
            {
                if (doGFC.UnlockCK(ckAccount)) { return true; }
                else { throw new Exception(doGFC.message); }

            }
            catch //(Exception ex)  // commented to remove warning
            {   
                throw new Exception("Failed to unlock CK");
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String getLoginBranch()
        {
            try
            {
                return doGFC.get_irsclient_data("LOCATION");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region private method
        private irsGFC doGFC
        {
            get
            {
                if (Session["GFCLOCK"] == null)
                {
                    Session["GFCLOCK"] = new irsGFC();
                }
                return (irsGFC)Session["GFCLOCK"];
            }
        }
        #endregion
    }
}
