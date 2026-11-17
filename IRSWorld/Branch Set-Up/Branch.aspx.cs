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
using System.Text;
using IRSClassLib;

namespace IRSWorld.Branch_Set_Up
{
    public partial class Branch : System.Web.UI.Page
    {
        private irsBranch IRSBranch
        {
            get
            {
                if (Session["IRS_BRANCH"] == null)
                {
                    Session["IRS_BRANCH"] = new irsBranch();
                }
                return (irsBranch)Session["IRS_BRANCH"];
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            Ajax.Utility.RegisterTypeForAjax(typeof(Branch));
        }


        
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public Object searchBR(String brNB, String brBC, String brSBC, String brAC, String brSAC, String brLoc, String brTZ)
        {

            DataTable dt = IRSBranch.searchBR(brNB, brBC, brSBC, brAC, brSAC, brLoc, brTZ);
            if (dt == null) throw new Exception(IRSBranch.Message);


            return dt;
            //return null;

        }

  


    }
}
