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
using IRSWorld.TranClass;

namespace IRSWorld.Card
{
    public partial class GMFCMaintenance : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {            
            AjaxPro.Utility.RegisterTypeForAjax(typeof(GMFCMaintenance));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(RemTranCardCommons));
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataTable getGlobalVariable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("UserLogged");
            dt.Columns.Add("BranchCode");
            
            irsClientData irs = new irsClientData();
            dt.Rows.Add(new Object[] { irs.UserId, irs.Location });

            return dt;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataTable getDataTable(String SQL)
        {
            try
            {
                irsSqlCommons com = new irsSqlCommons();
                
                com.EnforceConstraint = false;

                DataSet ds = com.getDataSet(com.getSqlCommand(SQL, false), "TABLE");

                if (com.Message != null && com.Message != String.Empty)
                {
                    throw new Exception(com.Message);
                }

                return ds.Tables[0];
            }
            catch (Exception e)
            {
                throw new Exception("Failed to get datatable: " + e.Message);
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet ExecProcedure(String ProcName, String ProcParam)
        {
            Object[] plist = ProcParam.Split('~');
            
            try
            {
                irsSqlCommons com = new irsSqlCommons();

                com.EnforceConstraint = false;

                SqlCommand cmd = com.getSqlCommand(ProcName, true);
                com.deriveParameters(cmd, plist);
                DataSet ds = com.getDataSet(cmd, "TABLE");

                if (com.Message != null && com.Message != String.Empty)
                {
                    throw new Exception(com.Message);
                }

                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("Failed to get dataset : " + e.Message);
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public String ExecProcedureNoReturn(String ProcName, String ProcParam)
        {            
            Object[] plist = ProcParam.Split('~');

            try
            {
                irsSqlCommons com = new irsSqlCommons();
                SqlCommand cmd = com.getSqlCommand(ProcName, true);
                com.deriveParameters(cmd, plist);

                int ra = com.sqlUpdate(cmd);

                if (ra > 0)
                {
                    return "";
                }
                else
                {                   
                    if (com.Message != null && com.Message != String.Empty)
                    {
                        throw new Exception(com.Message);
                    }
                }

                return "";
            }
            catch (Exception e)
            {
                //throw new Exception("Failed to update data : " + e.Message);
                return "Failed to update data : " + e.Message;
            }
        }

    }
}
