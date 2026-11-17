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
namespace IRSWorld.BranchSetup
{
    public partial class ForApprovalTag : System.Web.UI.Page
    {
        sqlHelperWs sql = new sqlHelperWs();
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(ForApprovalTag));
            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet load_branches()
        {

            irsClientData clienData = (irsClientData)Session["IRS_CLIENT_DATA"];
            DataSet dt;
            if (clienData.Role == 1)
            {
                dt = sql.getDataSet(sql.getSqlCommand("select BranchCode,BranchCode + '-' + BranchName AS BranchName from tbl_branches order by BranchCode", false));
            }
            else
            {
                dt = sql.getDataSet(sql.getSqlCommand("select BranchCode,BranchCode + '-' + BranchName AS BranchName from tbl_branches where BranchCode='" + clienData.Location + "' order by BranchCode", false));
            }
            return dt;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool getForApprovalSetUp(string branchcode)
        {
            irsSqlCommonsWs common = new irsSqlCommonsWs();
            irsClientData clienData = (irsClientData)Session["IRS_CLIENT_DATA"];
            DataTable dt;
            if (branchcode != "" || branchcode != null)
            {
                dt = common.getDataSet(common.getSqlCommand("SELECT CASE WHEN BRANCHDEFVALUE = 'BranchDefaultSetup_true' THEN 1 ELSE 0 END AS BRANCHDEFVALUE FROM TBL_BRANCH_DEFAULTS WHERE BRANCHCODE = '" + branchcode + "' AND BRANCHDEFCODE = 9", false)).Tables[0];
                string branchdefvalue = Convert.ToString(dt.Rows[0]["BRANCHDEFVALUE"]);
                if (branchdefvalue != "0")
                {
                    return true;
                }
                else
                { return false; }
                    
            }
            else
            {
                dt = common.getDataSet(common.getSqlCommand("SELECT CASE WHEN BRANCHDEFVALUE = 'BranchDefaultSetup_true' THEN 1 ELSE 0 END AS BRANCHDEFVALUE FROM TBL_BRANCH_DEFAULTS WHERE BRANCHCODE = '" + clienData.Location + "' AND BRANCHDEFCODE = 9", false)).Tables[0];
                string branchdefvalue = Convert.ToString(dt.Rows[0]["BRANCHDEFVALUE"]);
                if (branchdefvalue != "0")
                {
                    return true;
                }
                else
                { return false; }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool apply_changes(bool all,string branchcode,bool ischeck)
        {
            irsSqlCommonsWs common = new irsSqlCommonsWs();
            irsClientData clienData = (irsClientData)Session["IRS_CLIENT_DATA"];

            if (all == true)
            {
                
                DataTable dt = common.getDataSet(common.getSqlCommand("select * from tbl_branches where branchcode='" + branchcode + "'", false)).Tables[0];
                if (dt != null)
                {
                    
                    //update all by negbranchcode
                    string fldgroup = "BranchDefaultSetup_" + ischeck.ToString().ToLower() + "'";
                    string negbranchcode = Convert.ToString(dt.Rows[0]["NegBranchCode"]);
                    string strupdate = "update tbl_branch_defaults set BranchDefValue='" + fldgroup + "' where (SUBSTRING(BranchDefValue,0,19) = 'BranchDefaultSetup' and BranchDefCode='9') AND BranchCode in (select branchcode from tbl_branches where negbranchcode='" + negbranchcode + "')";

                    sql.execScalar(strupdate);
                    return true;
                }
                return false;
            }
            else
            {
                if (branchcode != "")
                {
                    sql.execScalar("UPDATE TBL_BRANCH_DEFAULTS SET BRANCHDEFVALUE='BranchDefaultSetup_" + ischeck.ToString().ToLower() + "' WHERE BRANCHDEFCODE=9 AND SUBSTRING(BRANCHDEFVALUE,0,19)='BranchDefaultSetup' AND BRANCHCODE='" + branchcode + "'");
                    return true;
                }
                else {
                    sql.execScalar("UPDATE TBL_BRANCH_DEFAULTS SET BRANCHDEFVALUE='BranchDefaultSetup_" + ischeck.ToString().ToLower() + "' WHERE BRANCHDEFCODE=9 AND SUBSTRING(BRANCHDEFVALUE,0,19)='BranchDefaultSetup' AND BRANCHCODE='" + clienData.Location + "'");
                    return true;
                }
            }
            return false;
        }
    }
}
