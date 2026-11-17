using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using IRSClassLib;

namespace IRSWorld._3D
{
    public partial class Tran_3D_Details : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Tran_3D_Details));
        }

        //[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        //public DataTable load_3D_details()//string tranxstatus, string courier, string currency, string date_from, string date_to)
        //{

        //    sqlHelper commons = new sqlHelper();
        //    SqlCommand cmd = commons.getSqlCommand("sp_get_3D_Details");
        //    try
        //    {
        //        cmd.Parameters.Add("@v_tranxStatus", SqlDbType.VarChar).Value = "258";//tranxstatus;
        //        cmd.Parameters.Add("@v_courier", SqlDbType.VarChar).Value = "PNBC06";//courier;
        //        cmd.Parameters.Add("@v_currency", SqlDbType.Int).Value = 1;//currency;
        //        cmd.Parameters.Add("@v_remdatefrom", SqlDbType.VarChar).Value = "06/21/2010";//date_from;
        //        cmd.Parameters.Add("@v_remdateto", SqlDbType.VarChar).Value = "06/22/2010";//date_to;

        //        DataSet ds = commons.getDataSet(cmd, "Display");
        //        return ds.Tables[0];
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Unable to return 3D Details : " + ex.Message);
        //    }
        //}

    }
}
