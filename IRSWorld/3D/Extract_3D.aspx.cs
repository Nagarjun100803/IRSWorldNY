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
using System.Data.SqlClient;
using IRSClassLib;

namespace IRSWorld._3D
{
    public partial class Extract_3D : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {            
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Extract_3D));
        }
        
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet load_currency()
        {
            sqlHelper commons = new sqlHelper();
            SqlCommand cmd = new SqlCommand();

            try
            {
                cmd = commons.getSqlCommand("select flddescription,fldvalue  from tbl_lookup where fldgroup='Currency' and fldvalue in (1,2)", false);
                DataSet ds = commons.getDataSet(cmd, "Currency");
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return null;
        }

        public DataSet load_3D_data()
        {
            sqlHelper commons = new sqlHelper();
            SqlCommand cmd = new SqlCommand();

            try
            {
                cmd = commons.getSqlCommand("select RemitterCIFID, GrossRemAmount, NetRemAmount, NettoBeneAmount, TotalLocalCharge from tbl_remittance where referenceno = '1LDN022510000001'", false);
                DataSet ds = commons.getDataSet(cmd, "tbl_3D_Data");
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return null;
        }

    }
}
