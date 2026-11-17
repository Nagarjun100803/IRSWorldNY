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

namespace IRSWorld.RTracker
{
    public partial class RemTracker : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(RemTracker));
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getDetails(String TrxnNo)
        {
            try
            {
                irsGFC gfc = new irsGFC();

                DataSet ds = gfc.execProcedure("sp_RTracker_get_details", new Object[] { TrxnNo });

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds;
                }
                else
                {
                    if (gfc.message != null && gfc.message != "")
                    {
                        throw new Exception(gfc.message);
                    }
                    else
                    {
                        throw new Exception("No transaction found");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
