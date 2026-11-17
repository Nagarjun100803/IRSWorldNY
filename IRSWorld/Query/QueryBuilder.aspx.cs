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

// **************************************************************************************************** //
// *** DEVELOPER    : Ronald James L. Tuazon                                                        *** //
// *** DATE CREATED : September 24, 2010                                                            *** //
// *** DATE MODIFIED: September 24, 2010                                                            *** //
// *** DESCRIPTION  : Query Builder                                                                 *** //
// **************************************************************************************************** //

namespace IRSWorld.Query
{
    public partial class QueryBuilder : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(QueryBuilder));
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getOverseasBranch()
        {
            return new irsQueryBuilder().getOverseasBranch();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getDomesticBranch()
        {
            return new irsQueryBuilder().getDomesticBranch();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getAgentBranch()
        {
            return new irsQueryBuilder().getAgentBranch();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getRemitMethod()
        {
            return new irsInternetPage().getLookup("RemitMethod");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getProductCode()
        {
            return new irsInternetPage().getLookup("ProductCode");
        }
    }
}
