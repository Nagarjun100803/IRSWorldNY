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

namespace IRSWorld._3D
{
    public partial class _DOverride : System.Web.UI.Page
    {
        private sqlHelper sql = new sqlHelper();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(_DOverride));
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet load_Courier()
        {
            sqlHelper commons = new sqlHelper();
            SqlCommand cmd = new SqlCommand();

            try
            {
                cmd = commons.getSqlCommand("select a.alias, UPPER(a.alias + ' - ' + a.lastname) as Courier from tbl_cif_master a " +
                                            "inner join tbl_cif_class b on a.cifid = b.cifid where b.cifclass = 9 " +
                                            "and isnull(a.alias, '') <> '' order by alias", false);
                DataSet ds = commons.getDataSet(cmd, "Courier");
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get3DReason()
        {
            DataSet ds = sql.getDataSet(sql.getSqlCommand("SELECT FldValue,FldDescription FROM TBL_LOOKUP WHERE FldGroup = '3DReason'", false));
            return ds;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get3DStatus()
        {
            DataSet ds = sql.getDataSet(sql.getSqlCommand("SELECT FldValue,FldDescription FROM TBL_LOOKUP WHERE FldGroup = 'TranxStatus' AND (fldValue >=262 AND fldValue <=271)", false));
            return ds;
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable sp_get_3D_Details_for_override(string tranxstat, string courier, string dateproc,int tag)
        {
            sqlHelper commons = new sqlHelper();
            DataSet ds = commons.execStoredProcedure("sp_get_3D_Details_for_override", new object[] { tranxstat, courier, dateproc, tag });
            return ds.Tables[0];
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string update_status(Int32 seq_id, int status)
        {
            object msg = sql.execScalar("UPDATE TBL_REMITTANCE SET TranStatus = " + status + " WHERE SEQ_ID=" + seq_id + "");
            return string.Empty;
        }

    }
}
