using IRSClassLib;
using System.Collections;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Text;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web;
using System;



namespace IRSWorld.Admin
{
    public partial class UserAdmin : System.Web.UI.Page
    {
        StreamWriter objwriter;
        private irsUsers IRSUsers
        {
            get
            {
                if (Session["IRS_USERS"] == null)
                {
                    if (Session["IRS_CLIENT_DATA"] == null) throw new Exception("Session Expired.");
                    Session["IRS_USERS"] = new irsUsers();
                }
                return (irsUsers)Session["IRS_USERS"];
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
            AjaxPro.Utility.RegisterTypeForAjax(typeof(UserAdmin));
            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);
        }

        /* RFC-31620 */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public int isAdminApproverTag()
        {
            int ret = 0;
            if (Session["AdminApprovalFlagUser"] == null)
            {
                if (new irsUsers().adminApproverSwitch())
                {
                    ret = 1;
                }
            }
            return ret;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object[] setup()
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
            irsUsers iu = IRSUsers;
            DataTable dt = iu.getUserRoles();
            if (dt == null) throw new Exception("GET ROLES: " + iu.Message);

            if (iu.is_setup())
            {
                DataRow[] rows = dt.Select("FldCode = 'System Administrator'");
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
                    /* VAPT IAG 20180730 */
                    if (IRSClientData.Role == 99) /* SUPERADMIN SEES ADMIN ONLY */
                    {
                        DataRow[] rows = dt.Select("FldCode IN ('System Administrator', 'System Administrator Approver')");
                        if (rows.Length > 0)
                        {
                            int i = 0;
                            string[] returnRow = new string[rows.Length];
                            for (i = 0; i <= (rows.Length - 1); i++)
                            {
                                returnRow.SetValue(rows[i]["fldValue"].ToString().Trim() + "~" + rows[i]["fldDescription"].ToString().Trim(), i);
                            }
                            return returnRow;
                        }
                    }
                    else
                    {
                        /* RFC-31620 */
                        DataRow[] rows = dt.Select("FldCode NOT IN ('SA')"); /* ADMIN CANNOT SEE SUPERADMIN DMP 01252022 RFC-20327 Removed System Administrator*/
                        if (rows.Length > 0)
                        {
                            int i = 0;
                            string[] returnRow = new string[rows.Length];

                            var query = dt.AsEnumerable().Where(x => x.Field<string>("FldCode") != "SA").OrderBy(c => c.Field<string>("fldDescription"));
                            DataView dv2 = query.AsDataView();

                            for (i = 0; i <= dv2.Count - 1; i++)// (rows.Length - 1); i++)
                            {
                                returnRow.SetValue(dv2[i].Row.ItemArray[3].ToString().Trim() + "~" + dv2[i].Row.ItemArray[4].ToString().Trim(), i);
                            }
                            return returnRow;
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
                /* VAPT IAG 20180730 */
                if (IRSClientData.Role == 99)
                {
                    string branchCode = new irsInternetPage().getLookupValue("ACDBranchCode", "FldCode", "ACD", "FldDescription");
                    DataRow[] rows = dt.Select("BranchCode = '" + branchCode + "'");
                    if (rows.Length > 0)
                    {
                        return new String[] { rows[0]["BranchCode"].ToString().Trim() + "~" + rows[0]["BranchName"].ToString().Trim() };
                    }
                }
                /* RFC-31620 */
                /*
                if (IRSClientData.Role != 1)
                {
                    DataRow[] rows = dt.Select("BranchCode = '" + IRSClientData.Location + "'");
                    if (rows.Length > 0)
                    {
                        return new String[] { rows[0]["BranchCode"].ToString().Trim() + "~" + rows[0]["BranchName"].ToString().Trim() };
                    }
                }
                */
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

        private String[] getLoginGroupOptions()
        {
            DataTable dt = IRSUsers.getLoginGroup();
            if (dt == null) throw new Exception("GET LoginGroups: " + IRSUsers.Message);
            return create_options(dt.Rows, "fldValue", "fldCode");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public Object[] getDdlData()
        {
            Object[] o = new Object[7];
            /* VAPT IAG 20180723 */
            if (Session["token_validity"] != null)
            {
                o[0] = getRolesOptions();
                o[1] = getFlagsOptions();
                o[2] = getBranchOptions();
                o[3] = getAccessOptions();
                
                try
                {
                    irsClientData id = (irsClientData)Session["IRS_CLIENT_DATA"];
                    o[4] = id.isSystemAdmin;
                    /* VAPT IAG 20180730 */
                    if (id.SuperAdmin)
                    {
                        o[4] = true;
                    }
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

                o[6] = getLoginGroupOptions();
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

        public String get_new_userid(String[] ss)
        {

            String[] uu = ss[3].Split('=');
            if (uu[0].Equals("USERID")) return uu[1];

            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object save(String rec)
        {
            String msg = "";
            String[] ss = rec.Split('~');
            /* RFC-31620 */
            Session["AdminApprovalFlagUser"] = null;
            string oldval = string.Empty;
            string newval = string.Empty;
            string action = string.Empty;

            if (ss[0].Equals("add") && IRSUsers.new_user(get_new_userid(ss)))
            {
                msg = this.setColumns(ss);
                /* VAPT 20190206 */
                int i = 0;
                string val = string.Empty;
                object[] arr = null;
                bool b = false;

                for (i = 0; i <= (ss.Length - 1); i++)
                {
                    if (ss.GetValue(i).ToString().ToUpper().IndexOf("MODE") >= 0)
                    {
                        b = true;
                    }
                    if (ss.GetValue(i).ToString().ToUpper().IndexOf("ROLE") >= 0)
                    {
                        arr = ss.GetValue(i).ToString().Split('=');
                        val += arr.GetValue(0).ToString() + "=" + new irsInternetPage().getLookupValue("User Roles", "FldValue", arr.GetValue(1).ToString(), "FldDescription") + " ~ ";
                        b = true;
                    }
                    else
                    {
                        if (ss.GetValue(i).ToString().ToUpper().IndexOf("FLAGS") >= 0)
                        {
                            arr = ss.GetValue(i).ToString().Split('=');
                            val += arr.GetValue(0).ToString() + "=" + new irsInternetPage().getLookupValue("User Flags", "FldValue", arr.GetValue(1).ToString(), "FldCode") + " ~ ";
                            b = true;
                        }
                    }
                    if (!b)
                    {
                        val += ss.GetValue(i).ToString() + " ~ ";
                    }
                    b = false;
                }
                action = "AN ADMIN CREATED A NEW USER";
                newval = val;
            }
            else if (ss[0].Equals("edit"))
            {
                if (ss[1] != null && ss[1].Length > 0)
                {
                    try
                    {
                        DateTime dt = DateTime.Parse(ss[2]);

                        if (IRSUsers.edit_user(ss[1], dt))
                        {
                            msg = this.setColumns(ss);
                            /* VAPT 20180222 */
                            int i = 0;
                            string val = string.Empty;
                            object[] arr;
                            bool b = false;

                            for (i = 0; i <= (ss.Length - 1); i++)
                            {
                                if (ss.GetValue(i).ToString().ToUpper().IndexOf("MODE") >= 0)
                                {
                                    b = true;
                                }
                                if (ss.GetValue(i).ToString().ToUpper().IndexOf("ROLE") >= 0)
                                {
                                    arr = ss.GetValue(i).ToString().Split('=');
                                    val += arr.GetValue(0).ToString() + "=" + new irsInternetPage().getLookupValue("User Roles", "FldValue", arr.GetValue(1).ToString(), "FldDescription") + " ~ ";
                                    b = true;
                                }
                                else
                                {
                                    if (ss.GetValue(i).ToString().ToUpper().IndexOf("FLAGS") >= 0)
                                    {
                                        arr = ss.GetValue(i).ToString().Split('=');
                                        val += arr.GetValue(0).ToString() + "=" + new irsInternetPage().getLookupValue("User Flags", "FldValue", arr.GetValue(1).ToString(), "FldCode") + " ~ ";
                                        b = true;
                                    }
                                }
                                if (!b)
                                {
                                    val += ss.GetValue(i).ToString() + " ~ ";
                                }
                                b = false;
                            }

                            string userID = string.Empty;
                            string oldValue = "old ~ ";
                            DataTable dtUsers = new DataTable();
                            int j = 0;

                            try
                            {
                                userID = val.Split('~').GetValue(1).ToString().Trim();
                                dtUsers = new irsUsers().getUserInfo(userID);
                                oldValue += userID + " ~ " + DateTime.Now + " ~ ";

                                if (dtUsers != null && dtUsers.Rows.Count > 0)
                                {
                                    for (j = 0; j <= (dtUsers.Columns.Count - 1); j++)
                                    {
                                        oldValue += dtUsers.Columns[j].ToString() + "=" + dtUsers.Rows[0][j].ToString() + " ~ ";
                                    }
                                }
                            }
                            catch {}

                            action = "AN ADMIN EDITED A USER";
                            oldval = oldValue;
                            newval = val;
                        } 
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
                        /* VAPT 20180222 */
                        int i = 0;
                        string val = string.Empty;

                        for (i = 0; i <= (ss.Length - 1); i++)
                        {
                            val += ss.GetValue(i).ToString() + " ~ ";
                        }

                        action = "AN ADMIN DELETED A USER";
                        newval = val;

                        /* RFC-31620 */
                        if (new irsUsers().adminApproverSwitch())
                        {
                            action = action + " (MAKER/CHECKER)";
                            string userId = ss[1].ToString();
                            if (new irsUsers().IsMakerAllowedUser(irsUsers.ActionType.DELETE, userId))
                            {
                                action = action + ". DELETE NOT SUCCESSFUL";
                                throw new Exception("There is a pending approval for the same user.");
                            }

                            if (!new irsUsers().saveUserMakerChecker(irsUsers.ActionType.DELETE, userId, null, ""))
                            {
                                msg = "Maker Checker saving [delete] failed: " + IRSUsers.Message;
                            }
                        }
                        else
                        {
                            if (!IRSUsers.delete_user(ss[1], int.Parse(ss[3].Trim())))
                            {
                                msg = "Delete Error:\n " + IRSUsers.Message;
                            }
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
                msg = ss[0] + ": operation not supported! " + IRSUsers.Message;
            }

            String retval = "";
            if (msg == String.Empty & !ss[0].Equals("delete"))
            {
                /* RFC-31620 */
                DataTable dt = new DataTable();
                DataRow dr;

                dt.Columns.Add("CIFID");
                dt.Columns.Add("UserID");
                dt.Columns.Add("Location");
                dt.Columns.Add("DateCreated");
                dt.Columns.Add("LastModified");
                dt.Columns.Add("LastAccessDate");
                dt.Columns.Add("LastPasswordChange");
                dt.Columns.Add("DaysExpired");
                dt.Columns.Add("Role");
                dt.Columns.Add("Flags");
                dt.Columns.Add("Mode");
                dt.Columns.Add("LoggedOn");
                dt.Columns.Add("LoginGroup");
                dt.Columns.Add("CurrentSessionID");

                string cifId = string.Empty;
                string userId = string.Empty;

                if (new irsUsers().adminApproverSwitch())
                {
                    action = action + " (MAKER/CHECKER)";
                    if (ss[0].Equals("add"))
                    {
                        cifId = ss[1].Split('=').GetValue(1).ToString();
                        userId = ss[3].Split('=').GetValue(1).ToString();

                        if (new irsUsers().IsMakerAllowedUser(irsUsers.ActionType.ADD, userId))
                        {
                            action = action + ". ADD NOT SUCCESSFUL";
                            throw new Exception("There is a pending approval for the same user.");
                        }

                        dr = dt.NewRow();
                        dr[0] = cifId;
                        dr[1] = userId;
                        dr[2] = ss[4].Split('=').GetValue(1).ToString();
                        dr[3] = DateTime.Now;
                        dr[4] = "";
                        dr[5] = "";
                        dr[6] = "";
                        dr[7] = ss[5].Split('=').GetValue(1).ToString();
                        dr[8] = ss[6].Split('=').GetValue(1).ToString();
                        dr[9] = "1";
                        dr[10] = ss[2].Split('=').GetValue(1).ToString();
                        dr[11] = ss[7].Split('=').GetValue(1).ToString();
                        dr[12] = "";
                        dr[13] = "";
                        dt.Rows.Add(dr);

                        if (!new irsUsers().saveUserMakerChecker(irsUsers.ActionType.ADD, userId, dt, cifId))
                        {
                            msg = "Maker Checker saving [add] failed: " + IRSUsers.Message;
                        }
                    }
                    if (ss[0].Equals("edit"))
                    {
                        cifId = ss[3].Split('=').GetValue(1).ToString();
                        userId = ss[1];
                        DataTable dtUserData = new irsUsers().getUserData(userId);
                        bool proceed = false;

                        if (dtUserData != null && dtUserData.Rows.Count > 0)
                        {
                            if (dtUserData.Rows[0]["Location"].ToString().Trim() != ss[5].Split('=').GetValue(1).ToString().Trim())
                            {
                                proceed = true;
                            }
                            else
                            {
                                if (dtUserData.Rows[0]["DaysExpired"].ToString().Trim() != ss[6].Split('=').GetValue(1).ToString().Trim())
                                {
                                    proceed = true;
                                }
                                else
                                {
                                    if (dtUserData.Rows[0]["Role"].ToString().Trim() != ss[7].Split('=').GetValue(1).ToString().Trim())
                                    {
                                        proceed = true;
                                    }
                                }
                            }
                        }

                        if (proceed)
                        {
                            if (new irsUsers().IsMakerAllowedUser(irsUsers.ActionType.EDIT, userId))
                            {
                                action = action + ". EDIT NOT SUCCESSFUL";
                                throw new Exception("There is a pending approval for the same user.");
                            }

                            dr = dt.NewRow();
                            dr[0] = cifId;
                            dr[1] = ss[1];
                            dr[2] = ss[5].Split('=').GetValue(1).ToString();
                            dr[3] = ss[8].Split('=').GetValue(1).ToString();
                            dr[4] = "";
                            dr[5] = "";
                            dr[6] = ss[9].Split('=').GetValue(1).ToString();
                            dr[7] = ss[6].Split('=').GetValue(1).ToString();
                            dr[8] = ss[7].Split('=').GetValue(1).ToString();
                            dr[9] = ss[10].Split('=').GetValue(1).ToString();
                            dr[10] = ss[4].Split('=').GetValue(1).ToString();
                            dr[11] = ss[11].Split('=').GetValue(1).ToString();
                            dr[12] = "";
                            dr[13] = "";
                            dt.Rows.Add(dr);

                            if (!new irsUsers().saveUserMakerChecker(irsUsers.ActionType.EDIT, ss[1].ToString(), dt, cifId))
                            {
                                msg = "Maker Checker saving [edit] failed: " + IRSUsers.Message;
                            }
                        }
                        else
                        {
                            /* RFC-31620 */
                            Session["AdminApprovalFlagUser"] = true;
                            if (!IRSUsers.save_user(ss[0], out retval))
                            {
                                msg = IRSUsers.Message;
                            }
                        }
                    }
                }
                else
                {
                    /* RFC-31620 */
                    Session["AdminApprovalFlagUser"] = true;
                    if (!IRSUsers.save_user(ss[0], out retval))
                    {
                        msg = IRSUsers.Message;
                    }
                }
            }
            new csAuditLog().writeVAPTLogger("", "ADMIN", action, oldval, newval, "", csAuditLog.AuditType.User, csAuditLog.AlertType.Information);

            if (msg.Length > 0) throw new Exception(msg);

            return new Object[] { IRSUsers.getSearchTable(), retval };
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object search(int category, String args, String brcode, int role, int flag, int logged)
        {
            irsClientData id = (irsClientData)Session["IRS_CLIENT_DATA"];
            String xbrcode = brcode;
            if (!id.isSystemAdmin)
            {
                xbrcode = id.Location;
            }
            /* VAPT IAG 20180730 */
            if (id.SuperAdmin)
            {
                xbrcode = new irsInternetPage().getLookupValue("ACDBranchCode", "FldCode", "ACD", "FldDescription");
            }

            DataTable dt = IRSUsers.search(category, args, xbrcode, role, flag, logged);
            if (dt == null) throw new Exception(IRSUsers.Message);

            return dt;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object getModulePermissions(String userid, String brcode, String role)
        {
            short rolecd = short.Parse(role);
            DataTable dt = IRSUsers.getModulePermissions(userid, brcode, rolecd);
            if (dt == null) throw new Exception(IRSUsers.Message);

            return dt;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool savePermissions(String s)
        {
            if (s.Length > 0 & IRSUsers.savePermissions(s))
            {
                return true;
            }

            throw new Exception(IRSUsers.Message);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
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
        
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string GenerateUsersReport(string Location, string UserRole, string Flag)
        {            
            SqlCommand sqlcommands = null;
            sqlHelperWs commons = new sqlHelperWs();
            DataSet myDataset = new DataSet();
            irsConfiguration config = new irsConfiguration();            

            string strPath = config.appsetting("ReportExportPath") + "\\UserInformationReports\\" + System.DateTime.Now.ToString("MMddyyyy");

            if (!Directory.Exists(strPath))
            {
                Directory.CreateDirectory(strPath);
            }

            sqlcommands = commons.getSqlCommand("EXEC SP_GET_USERS_INFORMATION '" + Location + "', '" + UserRole + "', '" + Flag + "'", false);

            myDataset = commons.getDataSet(sqlcommands, "users_information_report");

            if (myDataset.Tables[0].Rows.Count == 0)
            {
                return "";
            }
            else {
                string fileName = "Users_Information_" + System.DateTime.Now.ToString("MMddyyyy_hhmmss") + ".xls";
                string fullpath = strPath + "\\" + fileName;
                try
                {
                    objwriter = new StreamWriter(fullpath);

                    printHTML(true);
                    printBody(true);
                    printTable(true);

                    printTR(true);
                        printTD(true, "Users Informations Report ", "13", "center", "bold");
                        printTD(false);
                    printTR(false);

                    printTR(true);

                        printTD(true, "LOCATION", "1", "center", "bold");
                        printTD(false);

                        printTD(true, "USER ROLE", "1", "center", "bold");
                        printTD(false);

                        printTD(true, "FIRST NAME", "1", "center", "bold");
                        printTD(false);

                        printTD(true, "MIDDLE NAME", "1", "center", "bold");
                        printTD(false);

                        printTD(true, "LAST NAME", "1", "center", "bold");
                        printTD(false);

                        printTD(true, "USER FLAG", "1", "center", "bold");
                        printTD(false);

                        printTD(true, "CIFID", "1", "center", "bold");
                        printTD(false);

                        printTD(true, "USERID", "1", "center", "bold");
                        printTD(false);

                        printTD(true, "DATE CREATED", "1", "center", "bold");
                        printTD(false);

                        printTD(true, "LAST MODIFIED", "1", "center", "bold");
                        printTD(false);

                        printTD(true, "LAST ACCESS", "1", "center", "bold");
                        printTD(false);

                        printTD(true, "LAST PASSWORD CHANGED", "1", "center", "bold");
                        printTD(false);

                        printTD(true, "LoggedOn", "1", "center", "bold");
                        printTD(false);

                    printTR(false);

                    for (int r = 0; r < myDataset.Tables[0].Rows.Count; r++)
                    {
                        printTR(true);
                        for (int col = 1; col < 14; col++)
                        {
                            if (col == 1)
                            {
                                printTD(true, myDataset.Tables[0].Rows[r]["Location"].ToString(), "1", "left", "normal");
                            }
                            else if (col == 2)
                            {
                                printTD(true, myDataset.Tables[0].Rows[r]["UserRole"].ToString(), "1", "left", "normal");
                            }
                            else if (col == 3)
                            {
                                printTD(true, myDataset.Tables[0].Rows[r]["FirstName"].ToString(), "1", "left", "normal");
                            }
                            else if (col == 4)
                            {
                                printTD(true, myDataset.Tables[0].Rows[r]["MiddleName"].ToString(), "1", "left", "normal");
                            }
                            else if (col == 5)
                            {
                                printTD(true, myDataset.Tables[0].Rows[r]["LastName"].ToString(), "1", "left", "normal");
                            }
                            else if (col == 6)
                            {
                                printTD(true, myDataset.Tables[0].Rows[r]["UserFlag"].ToString(), "1", "left", "normal");
                            }
                            else if (col == 7)
                            {
                                printTD(true, "'" +  myDataset.Tables[0].Rows[r]["CIFID"].ToString(), "1", "left", "normal");
                            }
                            else if (col == 8)
                            {
                                printTD(true, myDataset.Tables[0].Rows[r]["UserID"].ToString(), "1", "left", "normal");
                            }
                            else if (col == 9)
                            {
                                printTD(true, myDataset.Tables[0].Rows[r]["DateCreated"].ToString(), "1", "left", "normal");
                            }
                            else if (col == 10)
                            {
                                printTD(true, myDataset.Tables[0].Rows[r]["LastModified"].ToString(), "1", "left", "normal");
                            }
                            else if (col == 11)
                            {
                                printTD(true, myDataset.Tables[0].Rows[r]["LastAccessDate"].ToString(), "1", "left", "normal");
                            }
                            else if (col == 12)
                            {
                                printTD(true, myDataset.Tables[0].Rows[r]["LastPasswordChange"].ToString(), "1", "left", "normal");
                            }
                            else if (col == 13)
                            {
                                printTD(true, myDataset.Tables[0].Rows[r]["LoggedOn"].ToString(), "1", "left", "normal");
                            }
                            printTD(false);
                        }
                        printTR(false);

                    }
                    printTable(false);
                    printBody(false);
                    printHTML(false);
                    objwriter.Close();
                }
                catch //(Exception ex) // commented to remove warning
                {
                    objwriter.Close();
                    return "";
                }
                finally
                {
                }

                return fileName + "~" + fullpath;
            }            
        }

        private void printTable(Boolean isStartTable)
        {
            if (isStartTable)
            {
                objwriter.WriteLine("\t\t<Table>");
            }
            else
            {
                objwriter.WriteLine("\t\t</Table>");
            }
        }

        private void printTD(Boolean isStartTD)
        {
            printTD(isStartTD, "", "", "", "");
        }

        private void printTD(Boolean isStartTD, string TDValue, string colspan, string Align, string Weight)
        {
            if (isStartTD)
            {
                objwriter.WriteLine("\t\t\t\t<td colspan=" + "\"" + colspan + "\"" + " style=\"text-align:" + Align + "; font-weight:" + Weight + "\">" + TDValue);
            }
            else
            {
                objwriter.WriteLine("\t\t\t\t</td>");
            }
        }

        private void printBody(Boolean isStartBody)
        {
            if (isStartBody)
            {
                objwriter.WriteLine("\t<body>");
            }
            else
            {
                objwriter.WriteLine("\t</body>");
            }
        }

        private void printHTML(Boolean isStartHTML)
        {
            if (isStartHTML)
            {
                objwriter.WriteLine("<html>");
            }
            else
            {
                objwriter.WriteLine("</html>");
            }
        }

        private void printTR(Boolean isStartTR)
        {
            if (isStartTR)
            {
                objwriter.WriteLine("\t\t\t<tr>");
            }
            else
            {
                objwriter.WriteLine("\t\t\t</tr>");
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String send_email(String userid, String emailad, String email_op)
        {
            if (!IRSUsers.email_password(userid, emailad, email_op))
            {
                /* VAPT 20180222 */
                new csAuditLog().writeVAPTLogger("", "EMAIL", "EMAIL OF PASSWORD SENDING FAILED", "UserID=[" + userid + "] EmailAddress=[" + emailad + "]", IRSUsers.Message, "", csAuditLog.AuditType.User, csAuditLog.AlertType.Information);
                throw new Exception(IRSUsers.Message);
            }
            else
            {
                /* VAPT 20180222 */
                new csAuditLog().writeVAPTLogger("", "EMAIL", "PASSWORD SUCCESSFULLY SENT TO USER", "UserID=[" + userid + "] EmailAddress=[" + emailad + "]", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Information);
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
            return IRSUsers.hasModuleProfile(userid, location, (short) role);
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool removeModuleProfile(String userid, String location, int role)
        {
            return IRSUsers.removeModuleProfile(userid, location, (short)role);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public short getLoginGroup(String brcode)
        {
            if (brcode == null || brcode == String.Empty) return 1;
            return IRSUsers.getLoginGroup(brcode);
        }
    }

}
