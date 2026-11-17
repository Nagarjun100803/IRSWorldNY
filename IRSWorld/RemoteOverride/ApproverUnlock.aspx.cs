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

namespace IRSWorld.RemoteOverride
{
    public partial class ApproverUnlock : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(ApproverUnlock));
            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);
        }

        private irsRemoteOverride irsRemoteOverride
        {
            get
            {
                if (Session["RemoteOverrideUnlock"] == null)
                {
                    Session["RemoteOverrideUnlock"] = new irsRemoteOverride();
                }
                return (irsRemoteOverride)Session["RemoteOverrideUnlock"];
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable checkLockedItems(string branchcode)
        {
            return irsRemoteOverride.checkLockedItems(branchcode);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getUserBranch()
        {
            irsClientData clientData = (irsClientData)Session["IRS_CLIENT_DATA"];
            return clientData.Location;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet saveUnlock(string seqid, string branchcode)
        {
            return irsRemoteOverride.saveUnlock(seqid, branchcode);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool isSystemAdmin()
        {
            irsClientData clientData = (irsClientData)Session["IRS_CLIENT_DATA"];
            return clientData.isSystemAdmin;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getBranches()
        {
            irsClientData clientData = (irsClientData)Session["IRS_CLIENT_DATA"];
            string location = "";
            bool sysAdm = clientData.isSystemAdmin;
            bool brAdm = clientData.isBranchAdmin;

            if (sysAdm) { location = ""; }
            else if (brAdm)
            {
                location = clientData.Location;
            }
            return irsRemoteOverride.getBranches(location);

        }
    }
}
