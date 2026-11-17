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
    public partial class Messages : System.Web.UI.Page
    {

        private irsMessages IRSMessages
        {
            get
            {
                if (Session["IRS_MESSAGES"] == null)
                {
                    Session["IRS_MESSAGES"] = new irsMessages();
                }
                return (irsMessages)Session["IRS_MESSAGES"];
            }
        }

        private irsClientData IRSClientData
        {
            get
            {
                if (Session["IRS_CLIENT_DATA"] != null)
                {
                    return ((irsClientData)Session["IRS_CLIENT_DATA"]);
                }

                return null;
            }
        }

        private bool isAuthorized(String branch)
        {
            bool isSysad = IRSClientData.isSystemAdmin;

            if (isSysad) return true;

            bool isBrad = IRSClientData.isBranchAdmin;
            if (isBrad)
            {
                if (branch == null || branch == String.Empty) return true;
                return (IRSClientData.Location.Equals(branch));
            }

            return false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Messages));
            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void close_session()
        {
            Session.Abandon();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void drop_msg()
        {
            Session["IRS_MESSAGES"] = null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object[] getMessages(String orig, String dttype, String dtfrom, String dtto)
        {
            String xorig = "";
            try
            {
                xorig = (orig == null || orig.Length == 0) ? IRSClientData.UserId : orig;
            }
            catch 
            {
                xorig = "ALL";
            }

            DataTable dt = IRSMessages.getMessages(xorig, dttype, dtfrom, dtto);
            if (dt != null)
            {
                return new Object[] { dt, IRSMessages.dataset.HasChanges() };
            }
            
            throw new Exception (IRSMessages.Message);
        }

        private String[] create_options(DataRowCollection rows, String val_fld, String data_fld)
        {
            ArrayList al = new ArrayList();
            foreach (DataRow row in rows)
            {
                al.Add(row[val_fld].ToString().Trim() + "~" + row[data_fld].ToString().Trim());
            }
            String[] ss = new String[al.Count];
            al.CopyTo(ss);
            return ss;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object[] getOrigins()
        {
            DataTable dt = IRSMessages.getOrigins();
            if (dt == null)
            {
                throw new Exception(IRSMessages.Message);
            }

            String val = (Session["IRS_CLIENT_DATA"] != null && IRSClientData.UserId != null) ? IRSClientData.UserId : "All";
            String brcode = (Session["IRS_CLIENT_DATA"] != null && IRSClientData.Location != null) ? IRSClientData.Location : String.Empty;

            if ((Session["IRS_CLIENT_DATA"] != null && IRSClientData.isSystemAdmin || IRSClientData.isBranchAdmin) | (!IRSMessages.isin_prod))
            {
                DataRow uprow = dt.NewRow();
                uprow["origin"] = "All";
                dt.Rows.Add(uprow);
            }
            
            if (!val.Equals("All"))
            {
                DataRow[] rows = dt.Select("origin = '" + val + "'");
                if (rows.Length == 0)
                {
                    DataRow uprow = dt.NewRow();
                    uprow["origin"] = val;
                    dt.Rows.Add(uprow);
                }
            }

            String[] msg_types = null;
            DataTable dt_types = IRSMessages.getMessageTypes();
            if (dt_types != null)
            {
                msg_types = create_options(dt_types.Rows, "FldValue", "FldCode");
            }

            String[] msg_target_types = null;
            DataTable dt_targets = IRSMessages.getTargetTypes();
            if (dt_types != null)
            {
                msg_target_types = create_options(dt_targets.Rows, "FldValue", "FldCode");
            }

            return new Object[] { dt, val, brcode, msg_types, msg_target_types, IRSClientData.isSystemAdmin | IRSClientData.isBranchAdmin };
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object[] getTargets(String tgcode)
        {
            DataTable dt = IRSMessages.getTargetTypes(tgcode);
            if (dt == null) throw new Exception(IRSMessages.Message);

            return new Object[] { tgcode, dt };
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object[] applyEdits(String adds, String dels, String eds)
        {
            char sep = '^';
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
                            ht.Add(cols[0], cols[1]);
                        }
                        bool ok = false;
                        DataTable dtLookup = new irsInternetPage().getLookup("ParamApproverFlag");
                        if (dtLookup.Rows.Count == 0 || dtLookup.Rows[0]["FldValue"].ToString() == "0")
                        {
                            ok = IRSMessages.applyUpdates(crud, row, ht);
                        }//need to approve
                        else
                        {
                            string userID = ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).UserId;
                            ok = IRSMessages.applyUpdates_tmp(crud, row, ht, userID);
                        }

                        if (!ok) errs.Add(crud + " Row: " + row + " - " + IRSMessages.Message);

                    }
                }
            }
            if (errs.Count > 0)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                foreach (Object msg in errs)
                {
                    sb.Append(msg.ToString() + "\n");
                }
                throw new Exception(sb.ToString());
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

            return new Object[] { IRSMessages.regetMessages(), IRSMessages.dataset.HasChanges() };
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object[] saveUpdates(String adds, String dels, String eds)
        {
            if (applyEdits(adds, dels, eds) != null)
            {
                DataTable dt = IRSMessages.saveUpdates();
                if (dt == null)
                {
                    throw new Exception("Save failed. " + IRSMessages.Message);
                }
                return new Object[] { dt, IRSMessages.dataset.HasChanges() };
            }

            return null;
        }

    }
}
