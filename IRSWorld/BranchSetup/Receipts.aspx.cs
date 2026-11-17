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
using System.Linq;

namespace IRSWorld.BranchSetup
{
	public partial class Receipts : System.Web.UI.Page
	{
        private irsReceipts irsReceipts
        {
            get
            {
                if (Session["RECEIPTS"] == null)
                {
                    Session["RECEIPTS"] = new irsReceipts();
                }
                return (irsReceipts)Session["RECEIPTS"];
            }
        }


		protected void Page_Load(object sender, EventArgs e)
		{
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Receipts));
            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getrolecode()
        {
            //1 - processor, 2-approver
            string roleid = ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).Role.ToString();
            string role = new irsInternetPage().getLookupValue("User Roles", "FldValue", roleid, "FldDescription");
            if (role.Equals("Parameter Administrator"))
            {
                Session["loginrolecode"] = "1";
            }
            else if (role.Equals("Parameter Administrator Approver"))
            {
                Session["loginrolecode"] = "2";
            }
            else
            {
                Session["loginrolecode"] = "0";
            }

            return Convert.ToString(Session["loginrolecode"]);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void close_session()
        {
            Session.Abandon();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void drop_irsUsers()
        {
            //Session["IRS_USERS"] = null; ???
            Session["RECEIPTS"] = null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getBranches()
        {

            irsClientData clientData = (irsClientData)Session["IRS_CLIENT_DATA"];
            string location = "";
            bool sysAdm = clientData.isSystemAdmin;
            bool brAdm = clientData.isBranchAdmin;

            if (sysAdm) { location = ""; }
            else if (brAdm) { location = clientData.Location; }
            return irsReceipts.GetBranches(location);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getValidID(string brCode)
        {
            return irsReceipts.GetValidID(brCode);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool SaveReceipts(string brcode, string values)
        {
            bool ok;

            DataTable dtLookup = new irsInternetPage().getLookup("ParamApproverFlag");

            if (dtLookup.Rows.Count == 0 || dtLookup.Rows[0]["FldValue"].ToString() == "0")
            {
                ok = irsReceipts.SaveReceipts(brcode, values);
            }//need to approve
            else
            {
                string userID = ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).UserId;
                ok = irsReceipts.saveToTemp(brcode, values, userID);
            }

            if (!ok)
            {
                throw new Exception(irsReceipts.Message);
                // commented to remove warning return false;
            }

            //email sending to approver
            try
            {
                irsSqlCommonsWs commons = new irsSqlCommonsWs();
                string[] sEmail = null;
                DataSet myDt = commons.getDataSet("sp_paramApproval_getApprEmailList", true, new object[] { });
                if (myDt.Tables[0].Rows.Count > 0)
                {
                    sEmail = myDt.Tables[0].Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();
                }
                csMailer imailer = new csMailer();
                imailer.sendMail("[NOTIFICATION] IRSW Parameter Change for Approval", sEmail, DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + " - Change in IRSW parameter has been submitted for review and approval\n\n");
            }
            catch { }
            return true;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getReceipts(string brcode)
        {
            return irsReceipts.getReceipts(brcode);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String getStartUpValue()
        {
            irsClientData clientData = (irsClientData)Session["IRS_CLIENT_DATA"];
            return clientData.Location;
        }

	}
}
