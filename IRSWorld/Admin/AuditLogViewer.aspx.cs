using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using IRSClassLib;

namespace IRSWorld.Admin
{
    public partial class AuditLogViewer : System.Web.UI.Page
    {
        /* why ??
        private irsUsers IRSUsers
        {
            get
            {
                if (Session["IRS_USERS"] == null)
                {
                    Session["IRS_USERS"] = new irsUsers();
                }
                return (irsUsers)Session["IRS_USERS"];
            }
        }
         * */

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(AuditLogViewer));
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getRolesOptions(string command)
        {
            irsSqlCommonsWs commons =     new irsSqlCommonsWs();
            //SqlCommand cmd    =     new SqlCommand(); // commented for irsSqlCommonsWs
            try
            {

                if (commons != null)
                {
                    //cmd = commons.getSqlCommand("SELECT fldcode,fldvalue  from tbl_lookup where fldgroup='USER ROLES'", false);
                    // cmd = commons.getSqlCommand("sp_cm_Currency", true);
                    //DataSet ds = commons.getDataSet(cmd, "Curency");

                    DataSet ds = commons.getDataSet("SELECT fldcode,fldvalue  from tbl_lookup where fldgroup='USER ROLES'", false, new String[] { }, new String[] { "Currency" });
                    return ds;
                }
            }
            catch(SqlException ex)
            {
                throw ex;
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getBranches(string command)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            //DataSet ds = commons.getDataSet(commons.getSqlCommand("get_dropdown_branches"),"tblBranches");
            DataSet ds = commons.getDataSet("get_dropdown_branches", true, new Object[] { }, new String[] { "tblBranches" });
            return ds;  
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void close_session()
        {
            Session.Abandon();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void drop_irsUsers()
        {
            //Session["IRS_USERS"] = null; ??
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataTable GetSearchInfo(string eventdate,string usernm,string userid,string location,string eventtype,string roleid,string logon)
        {
            irsSqlCommonsWs common = new irsSqlCommonsWs();
            DataSet ds =  common.execStoredProcedure("SP_SEARCH_AUDIT", new object[] { eventdate,usernm,userid,location,eventtype,roleid,logon });
            if (ds.Tables[0] == null)
            {
                throw new Exception("Get Modules Failed: " + common.Message);
            }
            return ds.Tables[0];
        }

    }
}
