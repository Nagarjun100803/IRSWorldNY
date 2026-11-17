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

namespace IRSWorld.OBPS
{
    public partial class ObpsTranSearch : System.Web.UI.Page
    {
        private irsOBPSTransactions irsOBPSTransactions
        {
            get
            {
                if (Session["OBPSTransactions"] == null)
                {
                    Session["OBPSTransactions"] = new irsOBPSTransactions();
                }
                return (irsOBPSTransactions)Session["OBPSTransactions"];
            }
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(ObpsTranSearch));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(irsOBPSTransactions));
        }

        #region AJAX PUBLIC METHOD

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getdropdown_class(string fldgroup)
        {
            sqlHelper commons = new sqlHelper();
            SqlCommand sqcommands = null;
            try
            {

                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_OBPSClass");
                    sqcommands.Parameters.Add("@fld_group", SqlDbType.VarChar, 100).Value = fldgroup;

                    return commons.getDataSet(sqcommands, "OBPSClass");
                }
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

            return null;

        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getdropdown_idtype(string fldgroup)
        {
            sqlHelper commons = new sqlHelper();
            SqlCommand sqcommands = null;
            try
            {

                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_OBPSIdType");
                    sqcommands.Parameters.Add("@fld_group", SqlDbType.VarChar, 100).Value = fldgroup;

                    return commons.getDataSet(sqcommands, "OBPSIdType");
                }
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

            return null;

        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataTable getDataTable(String SQL)
        {
            try
            {
                irsSqlCommons com = new irsSqlCommons();
                DataSet ds = com.getDataSet(com.getSqlCommand(SQL, false), "FileInd");

                return ds.Tables[0];
            }
            catch (Exception e)
            {
                throw new Exception("Failed to get datatable: " + e.Message);
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataTable getOBPData(String sp, string merchcode, int prodcode)
        {
            try
            {
                return irsOBPSTransactions.getOptionalField(sp, merchcode, prodcode);
            }
            catch (Exception e)
            {
                throw new Exception("Failed to get datatable: " + e.Message);
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_contribution(string fldvalue)
        {
            sqlHelper commons = new sqlHelper();
            SqlCommand sqcommands = null;
            try
            {

                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_obps_contribution");
                    sqcommands.Parameters.Add("@fld_value", SqlDbType.Int).Value = fldvalue;

                    return commons.getDataSet(sqcommands, "OBPSClass");
                }
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

            return null;

        }

        #endregion
    }
}
