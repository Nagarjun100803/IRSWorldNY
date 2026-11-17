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
    public partial class GRSApproval : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(GRSApproval));
        }

        #region ajax methods
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet getLookupValues()
        {
            try
            {
                String sql = "select FldValue as VALUE,	FldDescription as CODE from tbl_lookup where fldgroup = 'IBatch'; ";
                sql += "select FldValue as VALUE,	FldDescription as CODE from tbl_lookup where fldgroup = 'ImplementationType'; ";

                return IRSGRSApproval.getdynamicsql(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get lookup values");
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataTable get_batch_items(String parameter)
        {
            try
            {
                String[] param = parameter.Split('~');
                Object[] plist = 
                    {
                        param[0]
                        ,param[1]
                        ,param[2]
                        ,param[3]
                    };

                return IRSGRSApproval.execStoredProcedure("sp_grs_appr_batch_items", plist).Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get batch data");
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataTable get_batch_item_details(String SEQ_ID)
        {
            try
            {
                return IRSGRSApproval.execStoredProcedure("sp_grs_appr_get_batch_item_details", new Object[] { SEQ_ID }).Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get batch item details");
            }
        }

        #endregion

        #region private methods
        private irsGRSApproval IRSGRSApproval
        {
            get
            {
                if (Session["IRS_GRSAPPROVAL"] == null)
                {
                    Session["IRS_GRSAPPROVAL"] = new irsGRSApproval();
                }
                return (irsGRSApproval)Session["IRS_GRSAPPROVAL"];
            }
        }
        #endregion

    }
}
