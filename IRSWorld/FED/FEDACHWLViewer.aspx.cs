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
using System.Data.SqlClient ;

namespace IRSWorld.FED
{
    public partial class FEDACHWLViewer : System.Web.UI.Page
    {
        csAuditLog log = new csAuditLog();

        public string ErrorMessage = "";

        

        protected void Page_Load(object sender, EventArgs e)
        {
            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);

            if (!Page.IsPostBack)
            {
                AjaxPro.Utility.RegisterTypeForAjax(typeof(FEDACHWLViewer));
            }
        }

        private irsClientData IRS_CLIENT_DATA()
        {
            if (Session["IRS_CLIENT_DATA"] != null)
                return (irsClientData)Session["IRS_CLIENT_DATA"];
            else
            {
                Response.Redirect("ErrorPage.aspx");
                return null;
            }
        }

        #region AJAX PUBLIC METHOD
        
        
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string maintenance_record(string seq_id,string accountno,string fullname,string mode)
        {

            sqlHelperWs sws = new sqlHelperWs();
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            try
            {
                this.writeAudit(mode + "~" + seq_id +"~" + accountno + "~" + fullname,csAuditLog.Category.Insert );

                int x = 0;
                seq_id = ((Int32.TryParse(seq_id,out x)) ? seq_id : "0");

                cmd.Parameters.Add("@v_seq_id", SqlDbType.BigInt ).Value = Convert.ToInt32(seq_id);
                cmd.Parameters.Add("@v_accountno", SqlDbType.VarChar, 40).Value = accountno;
                cmd.Parameters.Add("@v_mode", SqlDbType.VarChar, 10).Value = mode;
                cmd.Parameters.Add("@v_BeneficiaryName", SqlDbType.VarChar, 500).Value = fullname;
                cmd.Parameters.Add("@v_errmsg", SqlDbType.VarChar, 150).Direction = ParameterDirection.Output;
                cmd.CommandText = "proc_maintenance_fed_wl";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                dt = sws.getDataSet(cmd).Tables[0];

                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0][1].ToString();
                }
                return "";
            }
            catch(Exception e)
            {
                this.writeAudit("ERRRO:" + e.StackTrace + "~" + e.Message, csAuditLog.Category.Select ); 
                
                return "ERROR:" + e.Message;
            }
            finally { sws = null; }
            }

        #endregion
                
        

        #region PRIVATE METHOD
        private void writeAudit(string Description, csAuditLog.Category Category)
        {
            csAuditLog csa = new csAuditLog();
            try
            {
                if (IRS_CLIENT_DATA().UserId != null)
                {
                    csa.writeAudit(IRS_CLIENT_DATA().UserId, "FEDACH File Loader", Description, "", "", Category);
                }
                else
                {
                    csa.writeAudit("FEDWATCHLIST", "FEDACH File Loader", Description, "", "", Category);
                }
            }
            finally { csa = null; }
        }
        #endregion

        }
        
        
 
}
