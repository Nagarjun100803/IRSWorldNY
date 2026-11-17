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
namespace IRSWorld.Remittance
{
	public partial class RemTranSearch : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            Ajax.Utility.RegisterTypeForAjax(typeof(RemTranSearch));
		}

        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_remitters_information(string cifid)
        {
            sqlHelper commons = new sqlHelper();
            SqlCommand sqcommands = null;

            try
            {
                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_txn_rem_search_infos", true);

                    //return Convert.ToInt32(commons.getDataSet(sqcommands, "CIF_MASTER_SEQ_ID").Tables[0].Rows[0][0]);

                }
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null) 
                { 
                    commons = null; 
                }
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
        }


	}
}
