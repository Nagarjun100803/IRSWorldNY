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

namespace IRSWorld.Remittance
{
    public partial class RemTranSearch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(RemTranSearch));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(RemTranCommons));
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_remittance_page(int product_charge_id, string query_parameters)
        {
           // return "../../Remittance/" + "RemTranGeneric.aspx" + query_parameters; //sp_txn_search_txn_pages
            return "../Remittance/" + "RemTeller.aspx" + query_parameters; //sp_txn_search_txn_pages
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_remitter_name_and_address(string remitter_id)
        {
            SqlCommand sqcommands = null;
            sqlHelper sql = new sqlHelper();

            try
            {
                //sql.EnforceConstraint = false;
                sqcommands = sql.getSqlCommand("sp_txn_rem_search_infos"); sqcommands.Parameters.Add("@cifid", SqlDbType.VarChar, 100).Value = remitter_id; return sql.getDataSet(sqcommands, "REMITTER_INFOS");
            }
            catch
            {
                return null;
            }
            finally
            {
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_ProductType(string remittingbranch)
        {
            SqlCommand sqcommands = null;
            sqlHelper sql = new sqlHelper();
            try
            {
                sqcommands = sql.getSqlCommand("sp_load_product_type");
                sqcommands.Parameters.Add("@branch_code", SqlDbType.VarChar, 4).Value = remittingbranch;

                DataSet dt = sql.getDataSet(sqcommands, "ProductType");

                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    return dt;
                }
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_ImplemType(string remittingbranch)
        {
            SqlCommand sqcommands = null;
            sqlHelper sql = new sqlHelper();
            try
            {
                sqcommands = sql.getSqlCommand("sp_load_implem_type");
                sqcommands.Parameters.Add("@branch_code", SqlDbType.VarChar, 4).Value = remittingbranch;

                DataSet dt = sql.getDataSet(sqcommands, "ImplemType");

                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    return dt;
                }
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }

        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_Paymode(string remittingbranch)
        {
            SqlCommand sqcommands = null;
            sqlHelper sql = new sqlHelper();
            try
            {
                sqcommands = sql.getSqlCommand("sp_load_paymode");
                sqcommands.Parameters.Add("@branch_code", SqlDbType.VarChar, 4).Value = remittingbranch;

                DataSet dt = sql.getDataSet(sqcommands, "Paymode");

                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    return dt;
                }
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }

        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_ChargeType(string remittingbranch)
        {
            SqlCommand sqcommands = null;
            sqlHelper sql = new sqlHelper();
            try
            {
                sqcommands = sql.getSqlCommand("sp_load_charge_type");
                sqcommands.Parameters.Add("@branch_code", SqlDbType.VarChar, 4).Value = remittingbranch;

                DataSet dt = sql.getDataSet(sqcommands, "ChargeType");

                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    return dt;
                }
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }

        }
    }
}
