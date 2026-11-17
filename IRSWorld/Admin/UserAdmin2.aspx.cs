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
    public partial class UserAdmin2 : System.Web.UI.Page
    {
        private irsUsers2 IRSUsers
        {
            get
            {
                if (Session["IRS_USERS"] == null)
                {
                    Session["IRS_USERS"] = new irsUsers2();
                }
                return (irsUsers2)Session["IRS_USERS"];
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

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(UserAdmin2));
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public Object[] setup(String ay)
        {
            bool is_setup = IRSUsers.is_setup();

            if (!is_setup)
            {
                irsClientData id = (irsClientData)Session["IRS_CLIENT_DATA"];

                if (id.UserId == null || id.UserId == String.Empty)
                {
                    throw new Exception("The session is expired. Please login again.");
                }
            }

            int xdays = getDefaultExpireDays();

            return new Object[] { is_setup, xdays };
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void close_session()
        {
            Session.Abandon();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void drop_irsusers()
        {
            Session["IRS_USERS"] = null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public bool user_exist(String key, String value)
        {
            return IRSUsers.user_exist(key, value);
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

        private String[] getRolesOptions()
        {
            irsUsers2 iu = IRSUsers;
            DataTable dt = iu.getUserRoles();
            if (dt == null) throw new Exception("GET ROLES: " + iu.Message);

            if (iu.is_setup())
            {
                DataRow[] rows = dt.Select("fldDescription = 'System Administrator'");
                if (rows.Length > 0)
                {
                    return new String[] { rows[0]["fldValue"].ToString().Trim() + "~" + rows[0]["fldDescription"].ToString().Trim() };
                }
                return new String[] { "1~System Administrator" };
            }
            else
            {
                if (IRSClientData != null && IRSClientData.UserId != null)
                {
                    if (IRSClientData.Role != 1)
                    {
                        DataRow[] rows = dt.Select("fldDescription = 'Client' OR fldDescription = 'Guest'");
                        if (rows.Length > 0)
                        {
                            return new String[] { rows[0]["fldValue"].ToString().Trim() + "~" + rows[0]["fldDescription"].ToString().Trim() };
                        }
                    }
                }
                else
                {
                    throw new Exception("Session Expired.");
                }
            }

            return create_options(dt.Rows, "fldValue", "fldDescription");
        }

        private String[] getFlagsOptions()
        {
            DataTable dt = IRSUsers.getUserFlags();
            if (dt == null) throw new Exception("GET Flags: " + IRSUsers.Message);
            return create_options(dt.Rows, "fldValue", "fldCode");
        }

        private DateTime getServerDate()
        {
            return IRSUsers.getServerDate();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public String[] getDefaultDates(int xdays)
        {
            DateTime cdt = IRSUsers.getServerDate();
            DateTime xdt = cdt.AddDays(xdays);

            return new String[] { cdt.ToString("MM/dd/yyyy hh:mm:ss tt"), xdt.ToString("MM/dd/yyyy hh:mm:ss tt") };
        }

        private int getDefaultExpireDays()
        {
            return IRSUsers.getDefaultExpireDays();
        }

        private String[] getBranchOptions()
        {
            DataTable dt = IRSUsers.getBranches();
            if (dt == null) throw new Exception("GET Branches: " + IRSUsers.Message);

            if (IRSClientData != null && IRSClientData.UserId != null && IRSClientData.Location != null)
            {
                if (IRSClientData.Role != 1)
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

        private DataTable getAccessOptions()
        {
            DataTable dt = IRSUsers.getAccessTypes();

            if (dt == null) throw new Exception("GET AccessTypes: " + IRSUsers.Message);
            return dt;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object[] getDdlData(String ay)
        {
            Object[] o = new Object[6];
            o[0] = getRolesOptions();
            o[1] = getFlagsOptions();
            o[2] = getBranchOptions();
            o[3] = getAccessOptions();

            try
            {
                irsClientData id = (irsClientData)Session["IRS_CLIENT_DATA"];
                o[4] = id.isSystemAdmin;
            }
            catch
            {
                o[4] = false;
            }

            try
            {
                irsClientData id = (irsClientData)Session["IRS_CLIENT_DATA"];
                o[5] = id.isBranchAdmin;
            }
            catch
            {
                o[5] = false;
            }

            return o;
        }

        private String setColumns(String[] cols)
        {
            ArrayList al = new ArrayList();
            int stcol = cols[0].Equals("add") ? 1 : 3;
            for (int i = stcol; i < cols.Length; i++)
            {
                String[] tt = cols[i].Split('=');
                if (tt[1].ToLower().Equals("undefined")) tt[1] = null;
                else
                {
                    if (tt[1].ToLower().Equals("on")) tt[1] = "true";
                    if (tt[1].ToLower().Equals("off")) tt[1] = "false";
                }
                if (!IRSUsers.setColumn(tt[0].Trim(), tt[1]))
                {
                    al.Add(tt[0] + ": " + IRSUsers.Message);
                }
            }

            if (al.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Object o in al)
                {
                    sb.Append((String)o + "\n");
                }
                return "Add Error:\n" + sb.ToString();
            }

            return String.Empty;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object save(String rec)
        {
            String msg = "";
            String[] ss = rec.Split('~');
            if (ss[0].Equals("add") && IRSUsers.new_user()) msg = this.setColumns(ss);
            else if (ss[0].Equals("edit"))
            {
                if (ss[1] != null && ss[1].Length > 0)
                {
                    try
                    {
                        DateTime dt = DateTime.Parse(ss[2]);
                        if (IRSUsers.edit_user(ss[1], dt)) msg = this.setColumns(ss);
                    }
                    catch (Exception e)
                    {
                        msg = "Edit Error:\n " + e.Message;
                    }
                }
            }
            else if (ss[0].Equals("delete"))
            {
                if (ss[1] != null && ss[1].Length > 0)
                {
                    try
                    {
                        if (!IRSUsers.delete_user(ss[1], int.Parse(ss[3].Trim())))
                        {
                            msg = "Delete Error:\n " + IRSUsers.Message;
                        }
                    }
                    catch (Exception e)
                    {
                        msg = "Delete Error:\n " + e.Message;
                    }
                }
            }
            else
            {
                msg = ss[0] + ": operation not supported!";
            }

            String retval = "";
            if (msg == String.Empty & !ss[0].Equals("delete"))
            {
                if (!IRSUsers.save_user(ss[0], out retval)) msg = IRSUsers.Message;
            }

            if (msg.Length > 0) throw new Exception(msg);

            return new Object[] { IRSUsers.getSearchTable(), retval };
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public Object[] getLocRole(String userid, int grow)
        {
            DataTable dt = IRSUsers.getLocRole(userid);
            if (dt != null)
            {
                return new Object[] { dt, grow };
            }

            throw new Exception(IRSUsers.Message);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public Object search(int category, String args, String brcode, int role, int flag, int logged)
        {
            irsClientData id = (irsClientData)Session["IRS_CLIENT_DATA"];
            String xbrcode = brcode;

            if (id == null || id.UserId == null) throw new Exception("Session Expired");

            if (!id.isSystemAdmin)
            {
                xbrcode = id.Location;
            }

            DataTable dt = IRSUsers.search(category, args, xbrcode, role, flag, logged);
            if (dt == null) throw new Exception(IRSUsers.Message);

            return dt;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public Object getModulePermissions(String userid, String brcode, String role)
        {
            short rolecd = short.Parse(role);
            DataTable dt = IRSUsers.getModulePermissions(userid, brcode, rolecd);
            if (dt == null) throw new Exception(IRSUsers.Message);

            return dt;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public bool savePermissions(String s)
        {
            if (s.Length > 0 & IRSUsers.savePermissions(s))
            {
                return true;
            }

            throw new Exception(IRSUsers.Message);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public Object reset_password(String userid)
        {
            if (Session["IRS_CLIENT_DATA"] == null) throw new Exception("Login is required to reset password");
            irsClientData icd = (irsClientData)Session["IRS_CLIENT_DATA"];
            if (icd.UserId != null && icd.UserId.Length > 0)
            {
                String retval = "";
                if (IRSUsers.resetPassword(userid, out retval))
                {
                    return new Object[] { true, retval };
                }
                else
                {
                    throw new Exception("Password Reset Failed. " + IRSUsers.Message);
                }
            }

            throw new Exception("Session Expired. Please login again.");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public String send_email(String userid, String emailad)
        {
            if (!IRSUsers.email_password(userid, emailad))
            {
                throw new Exception(IRSUsers.Message);
            }

            return emailad;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String disclose_password(String userid, String discloser_password)
        {
            String s = IRSUsers.disclose_password(userid, discloser_password);
            if (s == null)
            {
                throw new Exception(IRSUsers.Message);
            }

            return s;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool hasModuleProfile(String userid, String location, int role)
        {
            return new irsUsers2().hasModuleProfile(userid, location, (short)role);
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool removeModuleProfile(String userid, String location, int role)
        {
            return new irsUsers2().removeModuleProfile(userid, location, (short)role);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getEmptyLocRole()
        {
            DataTable dt = IRSUsers.define_search_details();
            if (dt != null) return dt;

            throw new Exception(IRSUsers.Message);
        }
    }
}
