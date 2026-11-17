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
using System.Linq;

namespace IRSWorld.BranchSetup
{
	public partial class Valid_IDs : System.Web.UI.Page
    {
        private irsValid_IDs irsValid_IDs
        {
            get
            {
                if (Session["ValidID"] == null)
                {
                    Session["ValidID"] = new irsValid_IDs();
                }
                return (irsValid_IDs)Session["ValidID"];
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void close_session()
        {
            Session.Abandon();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public void drop_irsUsers()
        {
            //Session["IRS_USERS"] = null; ???
            Session["ValidID"] = null;
        }

		protected void Page_Load(object sender, EventArgs e)
		{
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Valid_IDs));
            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getBranches(int roleid)
        {
            irsClientData clientData = (irsClientData)Session["IRS_CLIENT_DATA"];
            string location = "";
            bool sysAdm = clientData.isSystemAdmin;
            bool brAdm = clientData.isBranchAdmin;

            if (sysAdm) { location = ""; }
            else if (brAdm) { location = clientData.Location; }
            return irsValid_IDs.GetBranches(location);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getValidID(string brcode)
        {
            return irsValid_IDs.GetValidID(brcode);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object getFs()
        {
            String root = Server.MapPath("~");
            Object[] retval = irsValid_IDs.getFs(root);
            if (retval == null) throw new Exception(irsValid_IDs.Message);
            return new Object[] { root, retval };
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool DeleteValidId(int indx, string brcode)
        {
            return irsValid_IDs.DeleteValidId(indx, brcode);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String getStartUpValue()
        {
            irsClientData clientData = (irsClientData)Session["IRS_CLIENT_DATA"];
            return clientData.Location;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool detect_duplicate(string brcode,string iddesc,string validationdll)
        {
            return irsValid_IDs.detect_duplicate(brcode, iddesc, validationdll);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool applyEdits(String adds, String dels, String eds, String brCode,Boolean deleted)
        {
            char sep = '^';
            bool isEdit = false;
            ArrayList errs = new ArrayList();
            String[] ss = { adds, dels, eds };
            foreach (String s in ss)
            {
                if (s.Length > 0)
                {
                    String[] tt = s.Split('~');
                    String crud = tt[0].ToUpper();
                    for (int i = 1; i < tt.Length; i++)
                    {
                        String[] uu = tt[i].Split('|');
                        int row = int.Parse(uu[0]);
                        Hashtable ht = new Hashtable();
                        for (int j = 1; j < uu.Length; j++)
                        {
                            String[] cols = uu[j].Split(sep);
                            if (cols.Length < 2) continue;
                            {
                                ht.Add(cols[0], cols[1]);
                            }
                        }
                        if (tt[0] == "delete") { deleted = true; } else { deleted = false; }
                        if (tt[0] == "edit") { isEdit = true; }

                        bool ok = false;
                        DataTable dtLookup = new irsInternetPage().getLookup("ParamApproverFlag");
                        if (dtLookup.Rows.Count == 0 || dtLookup.Rows[0]["FldValue"].ToString() == "0")
                        {
                            ok = irsValid_IDs.valid_id_changes(ht, brCode, deleted, isEdit);
                        }//need to approve
                        else
                        {
                            if (new irsInternetPage().getParamForApproval(brCode, "Required IDs"))
                            {
                                throw new Exception("Cannot continue. There are pending for approval under branch " + brCode);
                            }
                            string userID = ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).UserId;
                            ok = irsValid_IDs.valid_id_changes_tmp(ht, brCode, deleted, isEdit, userID);
                            if (ok)
                            {
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
                            }
                        }

                        
                        if (!ok) 
                        {
                            throw new Exception(irsValid_IDs.Message); 
                        } 

                        //if (dels.Length > 0) { bool ok = irsHolidays.delete_holidays(ht); }
                        //else { bool ok = irsHolidays.save_holidays(ht,brCode); }
                    }
                }
            }
            if (errs.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Object msg in errs)
                {
                    sb.Append(msg.ToString() + "\n");
                }
                throw new Exception(sb.ToString());
            }

            if (!irsValid_IDs.saveUpdates())
            {
                throw new Exception("Save changes: " + irsValid_IDs.Message);
            }

            return true;
        }

	}
}
