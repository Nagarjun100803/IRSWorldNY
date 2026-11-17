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
    public partial class MessageGroup : System.Web.UI.Page
    {

        private irsMessageGroup IRSMessageGroup
        {
            get
            {
                if (Session["IRS_MESSAGE_GROUP"] == null)
                {
                    Session["IRS_MESSAGE_GROUP"] = new irsMessageGroup();
                }
                return (irsMessageGroup)Session["IRS_MESSAGE_GROUP"];
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
            AjaxPro.Utility.RegisterTypeForAjax(typeof(MessageGroup));
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
            Session["IRS_MESSAGE_GROUP"] = null;
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
        public Object[] getSetupData()
        {
            DataTable dt = IRSMessageGroup.getBranches();
            if (dt == null) throw new Exception("Unable to get branch table: " + IRSMessageGroup.Message);

            String[] brtab = create_options(dt.Rows, "BranchCode", "BranchName");

            dt = IRSMessageGroup.getMemberTypes();
            if (dt == null) throw new Exception("Unable to get MessageTargetTypes: " + IRSMessageGroup.Message);
            String[] ss = create_options(dt.Rows, "fldValue", "fldCode");

            String[] mtyps = new String[ss.Length - 1];
            int j = 0;
            for (int i = 0; i < ss.Length; i++)
            {
                if (ss[i].ToUpper().IndexOf("GROUP") > -1) continue;
                mtyps[j] = ss[i];
                j++;
            }

            return new Object[] { brtab, mtyps, IRSClientData.Location, IRSClientData.isSystemAdmin | IRSClientData.isBranchAdmin };
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object getMessageGroups(String brcode)
        {
            String xbrcode = (brcode == null || brcode.Length == 0) ? IRSClientData.Location : brcode;

            DataTable dt = IRSMessageGroup.getMessageGroups(xbrcode);
            if (dt == null)
            {
                throw new Exception(IRSMessageGroup.Message);
            }

            return dt;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object[] getGroupMembers(int groupid, int grow)
        {
            String errmsg = "";
            DataTable dt = IRSMessageGroup.getGroupMembers(groupid, out errmsg);
            if (dt == null)
            {
                throw new Exception(errmsg);
            }

            return new Object[] { dt, grow };
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object[] getUsers()
        {
            DataTable dt = IRSMessageGroup.getUsers();
            if (dt == null) throw new Exception("Unable to get users table: " + IRSMessageGroup.Message);

            return new object[] { dt, "User" };
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object[] getBranches()
        {
            DataTable dt = IRSMessageGroup.getBranches();
            if (dt == null) throw new Exception("Unable to get branch table: " + IRSMessageGroup.Message);

            return new Object[] { dt, "Branches" };
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object[] getPopGroups()
        {
            DataTable dt = IRSMessageGroup.getMessageGroups("ALL");
            if (dt == null)
            {
                throw new Exception("Unable to create options for groups: " + IRSMessageGroup.Message);
            }

            return new Object[] { dt, "Groups" };
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object applyEdits(String type, String adds, String dels, String eds)
        {
            if (type.Equals("Groups") | type.Equals("Members"))
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
                                if (cols[0].Equals("MsgMemberName")) continue;
                                ht.Add(cols[0], cols[1]);
                            }
                            bool ok = IRSMessageGroup.applyUpdates(type, crud, row, ht);
                            if (!ok) errs.Add(crud + " Row: " + row + " - " + IRSMessageGroup.Message);
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

                String errmsg = "";

                DataTable dt = IRSMessageGroup.saveUpdates(type, out errmsg);
                if (dt == null)
                {
                    throw new Exception("Save changes: " + errmsg);
                }

                return dt;
            }
            else
            {
                throw new Exception("Unknown type: " + type);
            }
        }
    }
}
