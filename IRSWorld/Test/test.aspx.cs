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
namespace IRSWorld.Test
{
    public partial class test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlHelper S = new sqlHelper();
            S.EnforceConstraint = false;

            SqlCommand sqcommands = S.getSqlCommand("sp_calculator_product_charges");
            
            sqcommands.Parameters.Add("@product_charges_id", SqlDbType.Int).Value = 1446;


            DataTable dt_charges = S.getDataSet(sqcommands, "PRODUCT_CHARGES").Tables[0];
        }
    }
}
