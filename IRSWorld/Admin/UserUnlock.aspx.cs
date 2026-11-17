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

namespace IRSWorld.Admin
{
    public partial class UserUnlock : System.Web.UI.Page
    {

        private irsUserUnlock IRSUserUnlock
        {
            get
            {
                if (Session["IRS_USER_UNLOCK"] == null)
                {
                    if (Session["IRS_CLIENT_DATA"] == null) throw new Exception("Session Expired.");
                    Session["IRS_USER_UNLOCK"] = new irsUserUnlock();
                }
                return (irsUserUnlock)Session["IRS_USER_UNLOCK"];
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

                throw new Exception("Session Expired.");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(UserUnlock));
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
        public void drop_irsusers()
        {
            Session["IRS_USER_UNLOCK"] = null;
        }

        private String[] getFlagsOptions()
        {
            DataTable dt = IRSUserUnlock.getUserFlags();
            if (dt == null) throw new Exception("GET Flags: " + IRSUserUnlock.Message);
            return create_options(dt.Rows, "fldValue", "fldCode");
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

        private String[] getBranchOptions()
        {
            DataTable dt = IRSUserUnlock.getBranches();
            if (dt == null) throw new Exception("GET Branches: " + IRSUserUnlock.Message);

            if (IRSClientData.UserId != null && IRSClientData.Location != null)
            {
                if (!IRSClientData.isSystemAdmin)
                {
                    DataRow[] rows = dt.Select("BranchCode = '" + IRSClientData.Location + "'");
                    if (rows.Length > 0)
                    {
                        return new String[] { rows[0]["BranchCode"].ToString().Trim() + "~" + rows[0]["BranchName"].ToString().Trim() };
                    }
                }
            }
            else
            {
                throw new Exception("Session Expired.");
            }

            return create_options(dt.Rows, "BranchCode", "BranchName");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object[] getSetupData()
        {
            return new Object[]
                {
                    IRSClientData.isSystemAdmin,
                    IRSClientData.isBranchAdmin,
                    getFlagsOptions(),
                    getBranchOptions(),
                    IRSClientData.Location
                };
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object search(String args, String brcode, int flag, int logged)
        {
            String xbrcode = brcode;
            if (!IRSClientData.isSystemAdmin)
            {
                xbrcode = IRSClientData.Location;
            }

            DataTable dt = IRSUserUnlock.search(args, xbrcode,flag, logged);
            if (dt == null) throw new Exception(IRSUserUnlock.Message);

            return dt;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable applyUpdates(String adds, String dels, String eds)
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
                        bool ok = IRSUserUnlock.applyUpdates(crud, row, ht);
                        if (!ok) errs.Add(crud + " Row: " + row + " - " + IRSUserUnlock.Message);
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

            /* VAPT 20180222 */
            string[] arrUsers = eds.Split('|');
            string[] arrValue;
            string oldValue = string.Empty;
            string newValue = string.Empty;
            string userID = string.Empty;
            int c = 0;

            for (c = 0; c <= (arrUsers.Length - 1); c++)
            {
                arrValue = arrUsers.GetValue(c).ToString().Split('^');
                if (arrValue.GetValue(0).ToString().ToLower().IndexOf("edit") < 0)
                {
                    if (arrValue.GetValue(0).ToString().ToLower().IndexOf("flags") >= 0)
                    {
                        newValue += arrValue.GetValue(0).ToString() + "=" + new irsInternetPage().getLookupValue("User Flags", "FldValue", arrValue.GetValue(1).ToString(), "FldDescription") + " ~ ";
                    }
                    else
                    {
                        if (arrValue.GetValue(0).ToString().ToLower().IndexOf("userid") >= 0)
                        {
                            userID = arrValue.GetValue(1).ToString();
                        }
                        newValue += arrValue.GetValue(0).ToString() + "=" + arrValue.GetValue(1).ToString() + " ~ ";
                    }
                }
            }

            DataSet ds = new irsSqlCommonsWs().getDataSet("SELECT A.CIFID, A.UserID, ISNULL(B.LastName, '') + ', ' + ISNULL(B.FirstName, '') + ' ' + ISNULL(B.MiddleName, '') AS FullName, A.Location, C.FldCode AS RoleDesc, D.FldDescription AS Flags, A.LoggedOn FROM tbl_Users A LEFT JOIN tbl_CIF_Master B ON A.CIFID = B.CIFID LEFT JOIN tbl_Lookup C ON A.Role = C.FldValue AND C.FldGroup = 'User Roles' LEFT JOIN tbl_Lookup D ON A.Flags = D.FldValue AND D.FldGroup = 'User Flags' WHERE A.UserID = '" + userID + "'", false, new Object[] { });
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                oldValue = "CIFID=" + ds.Tables[0].Rows[0][0].ToString() + " ~ ";
                oldValue += "UserID=" + ds.Tables[0].Rows[0][1].ToString() + " ~ ";
                oldValue += "FullName=" + ds.Tables[0].Rows[0][2].ToString() + " ~ ";
                oldValue += "Location=" + ds.Tables[0].Rows[0][3].ToString() + " ~ ";
                oldValue += "RoleDesc=" + ds.Tables[0].Rows[0][4].ToString() + " ~ ";
                oldValue += "Flags=" + ds.Tables[0].Rows[0][5].ToString() + " ~ ";
                oldValue += "LoggedOn=" + ds.Tables[0].Rows[0][6].ToString();
            }

            DataTable dt = IRSUserUnlock.saveUpdates();
            if (dt == null)
            {
                throw new Exception(IRSUserUnlock.Message);
            }

            /* VAPT 20180222 */
            new csAuditLog().writeVAPTLogger("", "USERS", "AN APPROVER UNLOCKED AN ID", oldValue, newValue, "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);

            return dt;
        }

    }
}
