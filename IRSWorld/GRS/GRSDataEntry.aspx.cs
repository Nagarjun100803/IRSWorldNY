using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using IRSClassLib;
using IRSWorld.TranClass;

namespace IRSWorld.GRS
{
    public partial class GRSDataEntry : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(GRSDataEntry));
            this.hfSessionid.Value = HttpContext.Current.Session.SessionID.ToString();
            this.hfBranchCode.Value = IRSClientData.Location;
        }
        private irsGRSDataEntry IRSGRSDataEntry
        {
            get
            {
                if (Session["IRS_GRSDATAENTRY"] == null)
                {
                    Session["IRS_GRSDATAENTRY"] = new irsGRSDataEntry();
                }
                return (irsGRSDataEntry)Session["IRS_GRSDATAENTRY"];
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

        #region "AJAX METHOD"
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public Object construct_temporay_batch(int v_noofitems, decimal v_runamount)
        {
            DataTable dt = new DataTable();
            try
            {
                // dt = this.select_branch_lookup(branchcode).Tables[1];
                return dt;
            }
            catch
            {
                return null;
            }
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet load_lookup_values()
        {
            irsTimeZone timezone = new irsTimeZone();

            return this.IRSGRSDataEntry.load_lookup_values(
                    timezone.branchLocalTime(((irsClientData)Session["IRS_CLIENT_DATA"]).Location),
                    ((irsClientData)Session["IRS_CLIENT_DATA"]).Location);

            //return this.IRSGRSDataEntry.load_lookup_values(
            //        timezone.branchLocalTime("1170"),
            //        "1170");
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet get_implementation(string prodcode,string remcurr)
        {
            return IRSGRSDataEntry.load_implementation(prodcode, IRSClientData.Location, remcurr);
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet file_format()
        {
            return (DataSet)new irsUploadFile().uploaderFileFormat("IBATCH");
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet load_rate_type()
        {
            irsTimeZone timezone = new irsTimeZone();
            return (DataSet)new irsUploadFile().get_rate_type(
                 timezone.branchLocalTime(((irsClientData)Session["IRS_CLIENT_DATA"]).Location),
                ((irsClientData)Session["IRS_CLIENT_DATA"]).Location);
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet construct_batch_items(string guid, string noofitems, string runamt, string ratetype, string fileformat)
        {
            DateTime tranxdate = new irsTimeZone().branchLocalTime(IRSClientData.Location);

            return this.IRSGRSDataEntry.construct_batch_items(
                                                        guid,
                                                        IRSClientData.UserId,
                                                        tranxdate,
                                                        Convert.ToInt32(fileformat),
                                                        Convert.ToInt32(ratetype),
                                                        Convert.ToInt32(noofitems),
                                                        Convert.ToDecimal(runamt)
                                                        );
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public string create_guid()
        {
            return this.IRSGRSDataEntry.get_new_id();
        }


        #endregion
    }
}
