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

namespace IRSWorld.Reports.Pages
{
    public partial class Report_Footer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Report_Footer));
        }
        #region AJAX METHOD
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet load_init_state()
        {
            DataSet ds = new DataSet();
            try
            {
                //ds = irsReportFooter.get_init_state(IRSClientData.Location, (IRSClientData.isSystemAdmin) ? 1 : 0);
                ds = irsReportFooter.get_init_state("TOK", 1);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    return ds;
                }
                else
                {
                    return null;
                }
            }
            catch 
            {
                return null;
            }
            finally
            {
                ds.Dispose();
            }
        }
        #endregion
        #region PRIVATE METHOD
        private irsReceipts irsReportFooter
        {
            get
            {
                if (Session["irsReportFooter"] == null)
                {
                    Session["irsReportFooter"] = new irsReceipts();
                }
                return (irsReceipts)Session["irsReportFooter"];
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
