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
using System.Data.SqlClient;

namespace IRSWorld.BillsPayment
{
    public partial class BillsPayment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(BillsPayment));
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataTable getDataTable(String SQL)
        {
            try
            {
                irsSqlCommonsWs com = new irsSqlCommonsWs(); //CHR - use ws version
                DataSet ds = com.getDataSet(com.getSqlCommand(SQL, false), "FileInd");

                return ds.Tables[0];
            }
            catch (Exception e)
            {
                throw new Exception("Failed to get datatable: " + e.Message);
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataTable getOBPData(String SQL)
        {
            try
            {
                /* CHR - ITG Password encrypriotn + use ws version
                SqlConnection cn = new SqlConnection(ConfigurationManager.AppSettings["OBPConnectionString"].ToString());
                SqlCommand cmd = new SqlCommand(SQL, cn);
                //irsSqlCommons com = new irsSqlCommons();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds= new DataSet(); //= com.getDataSet(cmd, "Merchants");
                da.Fill(ds, "Merchants");

                return ds.Tables[0];
                 * */

                irsSqlCommonsWs com = new irsSqlCommonsWs();
                com.constring_suffix = "OBPConnectionString";
                com.constring_noRunmode = true;
                SqlCommand cmd = new SqlCommand(SQL); //, cn);
                DataSet ds = com.getDataSet(cmd, "Merchants");
                if (ds != null && ds.Tables.Count > 0)
                    return ds.Tables[0];
                else
                    throw new Exception("Failed to get datatable: " + com.Message);
            }
            catch (Exception e)
            {
                throw new Exception("Failed to get datatable: " + e.Message);
            }
        }
    }
}
