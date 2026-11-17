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

namespace IRSWorld.Test
{
    [Serializable]
    public partial class PasswordDecrypt : System.Web.UI.Page
    {
        internal static IRSClassLib.TriDes getTriDes()
        {
            return new IRSClassLib.TriDes("xfm92.3", "IRSWorld");
        }

        private sqlHelperWs sqlkonek = new sqlHelperWs();

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(PasswordDecrypt));
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataTable getBranchList()
        {
            return new irsCurrencyRates().getBranches();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataTable getUserByBranch(string brCode)
        {
            return new irsUsers().search(1, string.Empty, brCode, 0, 2, 0);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataTable getUserRole(int userID)
        {
            return new irsUsers().getUserRoles((short)userID);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public string getUserPassword(string UserID)
        {
            string s = "";
            try
            {
                String sql = "SELECT Password FROM TBL_Users WHERE UserID = '" + UserID + "'";
                DataSet ds = sqlkonek.getDataSet(sqlkonek.getSqlCommand(sql, false));
                if (ds != null)
                {
                    String Pass = ds.Tables[0].Rows[0]["Password"].ToString().Trim();
                    s = getTriDes().Decrypt(Pass);
                    return s.Trim();
                }

                
            }
            catch
            {
            }
            return String.Empty;
        }
    }
}
