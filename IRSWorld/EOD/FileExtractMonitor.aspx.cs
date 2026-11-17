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

namespace IRSWorld.EOD
{
    public partial class FileExtractMonitor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(FileExtractMonitor));
        }
        #region AJAX METHOD
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet load_extracted_file(string _tranxdate, string _tran_source,string _status)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = IRSFileExtractMonitor.load_extracted_file(  Convert.ToDateTime(_tranxdate),
                                                            _tran_source 
                                                            , Convert.ToInt32(_status));

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    return ds;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                ds.Dispose();
            }
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet load_init_state()
        {

            DataSet ds = new DataSet();
            try
            {
                ds = IRSFileExtractMonitor.load_init_state();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    return ds;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
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
        private irsFileExtractMonitor IRSFileExtractMonitor
        {            
            get
            {
                if (Session["IRSFileExtractMonitor"] == null)
                {
                    Session["IRSFileExtractMonitor"] = new irsFileExtractMonitor(IRSClientData.Location,
                                                            ((IRSClientData.isSystemAdmin) ? 1 : 0 ) );
                }
                return (irsFileExtractMonitor)Session["IRSFileExtractMonitor"];
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
