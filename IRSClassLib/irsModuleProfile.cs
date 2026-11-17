using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading;

namespace IRSClassLib
{
    [Serializable]
    public class irsDefaultProfile : sqlHelperWs
    {
        #region PRIVATE VARIABLE
        private short search_role;
        private DataTable search_dt;
        /* RFC-31620 - IRSW Maker-Checker Function for User Admin */
        private string error_message = null;
        /* RFC-31620 - IRSW Maker-Checker Function for User Admin */
        #endregion

        /* RFC-31620 - IRSW Maker-Checker Function for User Admin */
        #region PROPERTY GET & SET
        public string messages
        {
            get { return this.error_message; }
        }

        public virtual bool setColumnModule(string col, object val)
        {
            this.TableName = "TBL_USER_PROFILE_DEFAULTS_TMP";
            this.IdentityColumn = "SEQ_ID";
            base.getUpdateDS("SELECT * FROM TBL_USER_PROFILE_DEFAULTS_TMP WHERE 1=0");

            if (isUpdatable)
            {
                DataRow row = (addMode) ? uprow : upds.Tables[0].Rows[selectedRow];
                return base.setColumn(row, col, val);
            }
            return false;
        }

        /* RFC-31620 - IRSW Maker-Checker Function for User Admin */
        #endregion


        private DataTable create_default_view(DataTable dt, short role)
        {
            try
            {
                DataTable dtnew = dt.Clone();

                DataTable dtmenu = new irsMenu().getMenuView();

                if (dtmenu != null && dtmenu.Rows.Count > 0)
                {
                    foreach (DataRow mrow in dtmenu.Rows)
                    {
                        DataRow urow = dtnew.NewRow();
                        urow["Role"] = role;
                        urow["SEQ_ID"] = mrow["SEQ_ID"];
                        urow["MenuGroup"] = mrow["MenuGroup"];
                        urow["MenuItemNo"] = mrow["MenuItemNo"];
                        urow["MenuParentId"] = mrow["MenuParentId"];
                        urow["MenuName"] = mrow["MenuName"];
                        urow["ModuleId"] = mrow["ModuleId"];
                        urow["ModuleName"] = mrow["ModuleName"];
                        short ac = 0;
                        if (dt.Rows.Count > 0)
                        {
                            DataRow[] trow = dt.Select("SEQ_ID = " + Convert.ToString(mrow["SEQ_ID"]));
                            if (trow.Length > 0)
                            {
                                ac = (short)trow[0]["AccessType"];
                            }
                        }
                        urow["AccessType"] = ac;
                        urow["EffDate"] = DBNull.Value;
                        dtnew.Rows.Add(urow);
                    }

                    return dtnew;
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }
            return null;
        }

        internal DataTable getRolePermissionView(short role)
        {
            DataSet ds = getDataSet("sp_getRolePermissionsView", true, new Object[] { role });
            if (ds != null && ds.Tables.Count > 0) return ds.Tables[0];
            return null;
        }

        public DataTable getDefaultProfile(short role)
        {
            try
            {
                search_role = role;

                DataTable dt = getRolePermissionView(role);
                if (dt != null && dt.Rows.Count >= 0)
                {
                    search_dt = create_default_view(dt, role);
                }
                else
                {
                    search_dt = dt;
                }

                return search_dt;
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return null;
        }

        public DataTable getUserRoles()
        {
            DataTable dt = getLookup(new Object[] { "User Roles", DBNull.Value, -1 });
            if (dt != null)
            {
                dt.DefaultView.Sort = "FldValue";
                return dt.DefaultView.ToTable();
            }
            return dt;
        }

        public DataTable getAccessTypes()
        {
            return getLookup(new Object[] { "User Access Types", DBNull.Value, -1 });
        }

        private int search_row(short sid)
        {
            for (int i = 0; i < search_dt.Rows.Count; i++)
            {
                short j = Convert.ToInt16(search_dt.Rows[i]["SEQ_ID"]);
                if (j == sid) return i;
            }

            return -1;
        }

        public String toMessage(ArrayList al)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Object o in al)
            {
                sb.Append(o.ToString() + "\n");
            }

            return sb.ToString();
        }

       

        private bool mergeUpdates(DataTable dt)
        {
            try /* OLD PROCESS */
            {
                foreach (DataRow row in dt.Rows)
                {
                    short sid = Convert.ToInt16(row["SEQ_ID"]);
                    selectedRow = findRow("PageID", sid);
                    if (selectedRow == -1)
                    {
                        base.newRow(sqlHelperWs.RETAIN_DATASET);
                    }
                    else
                    {
                        if (!table.Rows[selectedRow]["EffDate"].Equals(row["EffDate"]))
                        {
                            base.edit();
                        }
                        else
                        {
                            continue;
                        }
                    }
                    short err = 0;
                    ArrayList al = new ArrayList();
                    if (addMode)
                    {
                        if (!setColumn("Role", search_role)) { err++; al.Add(Message); }
                        if (!setColumn("PageID", sid)) { err++; al.Add(Message); }
                    }
                    if (!setColumn("AccessType", row["AccessType"])) { err++; al.Add(Message); }
                    if (!setColumn("EffDate", row["EffDate"])) { err++; al.Add(Message); }
                    if (err > 0)
                    {
                        Message = toMessage(al);
                        return false;
                    }
                    if (addMode)
                    {
                        table.Rows.Add(uprow);
                        addMode = false;
                    }
                }
                return save(sqlHelperWs.RETAIN_DATASET);
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return false;
        }

        /* RFC-31620 - IRSW Maker-Checker Function for User Admin */
        internal DataTable getUSerProfileDefaults (string role)
        {
            DataSet ds = getDataSet("sp_getUSerProfileDefaults", true, new Object[] { role });
            if (ds != null && ds.Tables.Count > 0) return ds.Tables[0];
            return null;
        }

        internal DataTable getModuleDescription(string role)
        {
            DataSet ds = getDataSet("sp_getModuleDescription", true, new Object[] { role });
            if (ds != null && ds.Tables.Count > 0) return ds.Tables[0];
            return null;
        }

        /* RFC-31620 - IRSW Maker-Checker Function for User Admin */
        private bool mergeUpdates_saveToTemp(DataTable dt, String userrole)
        {
            string dateNow = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            string role = string.Empty;
            string accesstype = string.Empty;
            string actiontype = string.Empty;
            bool ok = false;
            DataTable dtUSerProfileDefaults = getUSerProfileDefaults(userrole);

            irsUsers irsUsers = new irsUsers();
            string sessionUserId = ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId;

            try
            {
                int i = 0;
                int j = 0;
                bool found = false;
                bool isEdit = false;
                bool isAdd = false;
                string seq = string.Empty;

                for (i = 0; i <= (dt.Rows.Count - 1); i++) //Front-end
                {
                    for (j = 0; j <= (dtUSerProfileDefaults.Rows.Count - 1); j++) //Database
                    {
                        if (dtUSerProfileDefaults != null && dtUSerProfileDefaults.Rows.Count > 0)
                        {
                            if (dt.Rows[i]["SEQ_ID"].ToString().Trim() == dtUSerProfileDefaults.Rows[j]["PageID"].ToString().Trim())
                            {
                                if (dt.Rows[i]["AccessType"].ToString().Trim() == dtUSerProfileDefaults.Rows[j]["AccessType"].ToString().Trim())
                                {
                                    found = true;
                                }
                                else
                                {
                                    seq = dtUSerProfileDefaults.Rows[j]["SEQ_ID"].ToString();
                                    isEdit = true;
                                }
                            }
                        }
                        else
                        {
                            base.addMode = true;
                            this.setBase("SELECT TOP 1 * FROM TBL_USER_PROFILE_DEFAULTS_TMP", true);
                            setColumn("MAIN_SEQ_ID", "");
                            setColumn("Role", search_role);
                            setColumn("PageID", dt.Rows[i]["SEQ_ID"]);
                            setColumn("AccessType", dt.Rows[i]["AccessType"]);
                            setColumn("EffDate", dt.Rows[i]["EffDate"]);
                            setColumn("Maker", sessionUserId);
                            setColumn("Checker", "");
                            setColumn("DateAdded", DateTime.Now);
                            setColumn("CurrStatus", "For Approval");
                            setColumn("StatusDate", DateTime.Now);
                            setColumn("ActionType", ActionType.ADD.ToString());
                            setColumn("Module", "System Administration >> Default Profiles");
                            setColumn("MenuName", "User Profile Defaults Table");
                            setColumn("Remarks", "");
                            ok = base.save(sqlHelperWs.RETAIN_DATASET);
                            isAdd = true;
                        }
                    }
                    if (!found && !isAdd) //Insert
                    {
                        base.addMode = true;
                        this.setBase("SELECT TOP 1 * FROM TBL_USER_PROFILE_DEFAULTS_TMP", true);
                        setColumn("MAIN_SEQ_ID", seq);
                        setColumn("Role", search_role);
                        setColumn("PageID", dt.Rows[i]["SEQ_ID"]);
                        setColumn("AccessType", dt.Rows[i]["AccessType"]);
                        setColumn("EffDate", dt.Rows[i]["EffDate"]);
                        setColumn("Maker", sessionUserId);
                        setColumn("Checker", "");
                        setColumn("DateAdded", DateTime.Now);
                        setColumn("CurrStatus", "For Approval");
                        setColumn("StatusDate", DateTime.Now);
                        if (isEdit)
                        {
                            setColumn("ActionType", ActionType.EDIT.ToString());
                        }
                        else
                        {
                            setColumn("ActionType", ActionType.ADD.ToString());
                        }
                        setColumn("Module", "System Administration >> Default Profiles");
                        setColumn("MenuName", "User Profile Defaults Table");
                        setColumn("Remarks", "");
                        ok = base.save(sqlHelperWs.RETAIN_DATASET);
                    }
                    found = false;
                    isEdit = false;
                    isAdd = false;
                    seq = string.Empty;
                }
                
                if (ok)
                {
                    try
                    {
                        string receiver = new irsInternetPage().getLookupValue("AdminApproverEmail", "FldCode", "AdminRecipient", "FldDescription");
                        string receivercc = new irsInternetPage().getLookupValue("AdminApproverEmail", "FldCode", "AdminRecipientCC", "FldDescription");
                        string receiverbcc = new irsInternetPage().getLookupValue("AdminApproverEmail", "FldCode", "AdminRecipientBCC", "FldDescription");

                        string[] arrReceiver = receiver.Split(';');
                        string[] arrReceiverCC = receivercc.Split(';');
                        string[] arrReceiverBCC = receiverbcc.Split(';');
                        string subject = string.Empty;
                        string body = string.Empty;
                        List<string> recipient = new List<string>();
                        List<string> cc = new List<string>();
                        List<string> bcc = new List<string>();

                        foreach (var email in arrReceiver)
                        {
                            recipient.Add(email.ToString().Trim());
                        }

                        foreach (var emailcc in arrReceiverCC)
                        {
                            cc.Add(emailcc.ToString().Trim());
                        }

                        foreach (var emailbcc in arrReceiverBCC)
                        {
                            bcc.Add(emailbcc.ToString().Trim());
                        }


                        subject = new irsInternetPage().getLookupValue("AdminApproverEmail", "FldCode", "AdminSubject", "FldDescription").Replace("@Region", System.Configuration.ConfigurationManager.AppSettings["SMTPRegion"]);
                        body = new irsInternetPage().getLookupValue("AdminApproverEmail", "FldCode", "DefaultProfilesBody", "FldDescription");

                        body = body.Replace("@ModuleName", "System Administration >> Default Profiles");
                        body = body.Replace("@MakerID", sessionUserId);
                        body = body.Replace("@ActionType", ActionType.EDIT.ToString());
                        body = body.Replace("@UserRole", getUserRoleDescription(userrole).ToString());
                        body = body.Replace("@DateRequested", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));

                        irsUsers.SendEmail(subject, recipient, cc, bcc, body, null);
                    }
                    catch (Exception ex)
                    {
                        new csAuditLog().writeAudit("", "mergeUpdates_saveToTemp Email Sending Failed", "Maker Checker User Admin", ex.Message, ex.StackTrace, csAuditLog.Category.Email);
                    }
                    return true;
                }

            }
            catch (Exception e)
            {
                Message = e.Message;
                new csAuditLog().writeAudit("", "DEFAULTS PROFILE", "mergeUpdates_saveToTemp ERROR", e.Message, e.StackTrace, csAuditLog.Category.Select);

            }
            return false;
        }

        /* RFC-31620 */

        private void setBase(string SQL, bool IsAdd)
        {
            base.getUpdateDataSet(getSqlCommand(SQL, false));
            if (IsAdd)
            {
                base.newRow(sqlHelperWs.RETAIN_DATASET);
            }
            else
            {
                selectedRow = 0;
            }
        }

        /* RFC-31620 - IRSW Maker-Checker Function for User Admin */
        public enum ActionType
        {
            ADD,
            EDIT,
            DELETE
        }


        /* RFC-31620 - IRSW Maker-Checker Function for User Admin */
        public String getUserRoleDescription(String userrole)
        {
            String sql = "SELECT FldDescription FROM TBL_LOOKUP WHERE FLDGROUP = 'USER ROLES' AND FldValue = '" + userrole + "'";
            DataSet ds = getDataSet(getSqlCommand(sql, false));
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                String urole = ds.Tables[0].Rows[0]["FldDescription"].ToString().Trim();
                return urole;
            }
            return String.Empty;
        }

        public String getPageIDDescription(short PageID)
        {
            String sql = "SELECT MenuName FROM TBL_MENU WHERE SEQ_ID = " + PageID ;
            DataSet ds = getDataSet(getSqlCommand(sql, false));
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                String urole = ds.Tables[0].Rows[0]["MenuName"].ToString().Trim();
                return urole;
            }
            return String.Empty;
        }

        private bool applyUpdates(DataTable dt)
        {
            try
            {
                base.getUpdateDS("sp_getRolePermissions", new Object[] { search_role });
                if (isUpdatable)
                {
                    return mergeUpdates(dt);
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }
            return false;
        }

        /* RFC-31620 - IRSW Maker-Checker Function for User Admin */
        private bool applyUpdates_saveToTemp(DataTable dt, String role)
        {
            try
            {
                base.getUpdateDS("sp_getRolePermissions", new Object[] { search_role });
                return mergeUpdates_saveToTemp(dt, role);
            }
            catch (Exception e)
            {
                Message = e.Message;
                new csAuditLog().writeAudit("", "DEFAULTS PROFILE", "applyUpdates_saveToTemp ERROR", e.Message, e.StackTrace, csAuditLog.Category.Select);
            }
            return false;
        }

        public DataTable applyUpdates(String role, DataTable dt)
        {
            try
            {
                short srole = short.Parse(role.Trim());
                if (srole != search_role)
                {
                    Message = "Input role is not the same as searched.";
                }
                else if (dt.Rows.Count > 0)
                {
                    if (applyUpdates(dt))
                    {
                        
                        return this.getDefaultProfile(srole);
                    }
                }
                else
                {
                    Message = "Input does not contain rows.";
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return null;
        }

        public irsDefaultProfile()
        {
            this.TableName = "TBL_USER_PROFILE_DEFAULTs";
            this.IdentityColumn = "SEQ_ID";
        }

        /* RFC-31620 - IRSW Maker-Checker Function for User Admin */
        public DataTable applyUpdates_saveToTemp(String role, DataTable dt)
        {
            try
            {
                short srole = short.Parse(role.Trim());
                if (srole != search_role)
                {
                    Message = "Input role is not the same as searched.";
                }
                else if (dt.Rows.Count > 0)
                {
                    if (applyUpdates_saveToTemp(dt, role))
                    {
                        return this.getDefaultProfile(srole);
                    }
                }
                else
                {
                    Message = "Input does not contain rows.";
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
                new csAuditLog().writeAudit("", "DEFAULTS PROFILE", "applyUpdates_saveToTemp ERROR", e.Message, e.StackTrace, csAuditLog.Category.Select);

            }

            return null;
        }
    }

    [Serializable]
    internal class irsModuleProfile : sqlHelperWs
    {
        private short ACCESS_NONE = 0;
        private short ACCESS_ALLOWED = 1;
        private short ACCESS_HOME = 3;
        private bool ready;

        private void load_access_types()
        {
            DataTable dt = new irsUsers().getAccessTypes();
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    String s = row["FldCode"].ToString();
                    if (s.Trim().ToUpper().Equals("NONE")) ACCESS_NONE = Convert.ToInt16(row["FldValue"]);
                    if (s.Trim().ToUpper().Equals("ALLOWED")) ACCESS_ALLOWED = Convert.ToInt16(row["FldValue"]);
                    if (s.Trim().ToUpper().Equals("HOME")) ACCESS_HOME = Convert.ToInt16(row["FldValue"]);
                }
            }

            ready = true;
        }

        private DataTable filter_menu(DataTable dt, String uid)
        {
            while(! ready) 
            { 
                System.Threading.Thread.Sleep(20); 
            }

            DataTable dtnew = dt.Clone();

            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    short ac = Convert.ToInt16(row["AccessType"]);
                    if (ac == ACCESS_NONE) continue;

                    DataRow urow = dtnew.NewRow();
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (dt.Columns[i].ColumnName.Equals("ModuleUrl") & row[i] != DBNull.Value)
                        {
                            String url = (String)row[i];
                            if (url.ToUpper().IndexOf("$USERID") > -1)
                            {
                                url = url.Replace("$USERID", uid);
                            }
                            urow[i] = url;
                        }
                        else
                        {
                            urow[i] = row[i];
                        }
                    }
                    dtnew.Rows.Add(urow);
                }
                catch { }
            }

            return dtnew;
        }

        internal bool deletePermissions(String userid)
        {
            try
            {
                return (base.sqlUpdate("DELETE FROM " + TableName + 
                    " WHERE UserId = '" + userid + "'") > -1);
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return false;
        }

        public bool hasModulePermission(String userid, String brcode, short role)
        {
            Object o = execScalar("SELECT COUNT(*) AS COUNT_MOD FROM " + this.TableName + " WHERE userid = '" + userid + "' AND BranchCode = '" + brcode + "' AND Role = " + role);
            if (o != DBNull.Value)
            {
                return (Convert.ToInt32(o) > 0);
            }

            return false;
        }

        public bool removeModulePermission(String userid, String brcode, short role)
        {
            String sql = "DELETE FROM " + base.TableName + " WHERE userid = '" + userid + "' AND BranchCode = '" + brcode + "' AND Role = " + role;
            int ra = base.sqlUpdate(sql);
            return (ra > 0);
        }

        internal bool savePermissions(Hashtable ht)
        {
            try
            {
                String userid = (String)ht["USERID"];
                String brcode = (String)ht["BRANCHCODE"];
                short role = Convert.ToInt16(ht["ROLE"]);

                if (userid.Length > 0)
                {
                    base.getUpdateDS("sp_getUserPermissionsExt", new Object[] { userid, brcode, role });
                    if (isUpdatable)
                    {
                        foreach (Object key in ht.Keys)
                        {
                            if (key.ToString().Equals("USERID") | key.ToString().Equals("ROLE") | key.ToString().Equals("BRANCHCODE")) continue;
                            
                            String[] ss = ((String)ht[key]).Split('^');
                            short actype = 0;
                            try
                            {
                                actype = Convert.ToInt16(ss[0]);
                            }
                            catch { }

                            Object offdate = DBNull.Value;
                            if (ss[1].Length > 0)
                            {
                                try
                                {
                                    offdate = Convert.ToDateTime(ss[1]);
                                }
                                catch { }
                            }
                            
                            DataRow[] row = table.Select("PageId = " + key.ToString());
                            if (row.Length > 0)
                            {
                                row[0]["AccessType"] = actype;
                                row[0]["EffDate"] = offdate;
                            }
                            else if (base.newRow(sqlHelperWs.RETAIN_DATASET))
                            {
                                uprow["UserId"] = userid;
                                uprow["BranchCode"] = brcode;
                                uprow["Role"] = role;
                                uprow["PageId"] = Convert.ToInt16(key);
                                uprow["AccessType"] = actype;
                                uprow["EffDate"] = offdate;
                                table.Rows.Add(uprow);
                            }

                        }
                        base.addMode = false;
                        return base.save(sqlHelperWs.RETAIN_DATASET);
                    }
                }
                else
                {
                    Message = "USERID is missing.";
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return false;
        }

        private DataTable getDefaultProfile(String userid)
        {
            short role = new irsUsers().getUserRole(userid);
            if (role > -1)
            {
                return new irsDefaultProfile().getDefaultProfile(role);
            }

            Message = "User " + userid + " must have a role";
            return null;
        }

        private DataTable create_view(DataTable dt, String userid)
        {
            try
            {
                DataTable dtnew = dt.Clone();
                DataTable dtmenu = getDefaultProfile(userid); // new irsMenu().getMenuView(null);
                if (dtmenu != null && dtmenu.Rows.Count > 0)
                {
                    foreach (DataRow mrow in dtmenu.Rows)
                    {
                        DataRow urow = dtnew.NewRow();
                        urow["UserId"] = userid;
                        urow["SEQ_ID"] = mrow["SEQ_ID"];
                        urow["MenuGroup"] = mrow["MenuGroup"];
                        urow["MenuItemNo"] = mrow["MenuItemNo"];
                        urow["MenuParentId"] = mrow["MenuParentId"];
                        urow["MenuName"] = mrow["MenuName"];
                        urow["ModuleId"] = mrow["ModuleId"];
                        urow["ModuleName"] = mrow["ModuleName"];
                        short ac = 0;

                        if (dt.Rows.Count > 0)
                        {
                            DataRow[] trow = dt.Select("SEQ_ID = " + Convert.ToString(mrow["SEQ_ID"]));
                            if (trow.Length > 0)
                            {
                                ac = (short)trow[0]["AccessType"];
                            }
                        }
                        else
                        {
                            try
                            {
                                ac = Convert.ToInt16(mrow["AccessType"]);
                            }
                            catch { }
                        }
                        urow["AccessType"] = ac;
                        dtnew.Rows.Add(urow);
                    }

                    return dtnew;
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }
            return null;
        }

        private DataTable getUserPermissionsViewExt(String userid, String brcode, short role)
        {
            DataSet ds = getDataSet("sp_getUserPermissionsViewExt", true, new Object[] { userid, brcode, role });
            if (ds != null && ds.Tables.Count > 0) return ds.Tables[0];
            return null;
        }

        internal DataTable getModulePermissions(String userid, String brcode, short role)
        {
            try
            {
                DataTable dt = getUserPermissionsViewExt(userid, brcode, role);
                if (dt != null && dt.Rows.Count >= 0)
                {
                    return create_view(dt, userid);
                }
                else
                {
                    return dt;
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return null;
        }

        private DateTime[] getEffDates(DataRow dr_role, DataRow dr_user)
        {
            Object ro = null;
            Object uo = null;
            DateTime udt = DateTime.MinValue;
            DateTime rdt = DateTime.MinValue;
            try
            {
                uo = dr_user["EffDate"];
                ro = dr_role["EffDate"];
            }
            catch { }

            if (uo != null && uo != DBNull.Value) udt = Convert.ToDateTime(uo);
            if (ro != null && ro != DBNull.Value) rdt = Convert.ToDateTime(ro);

            return new DateTime[] { rdt, udt };
        }

        private DataTable updatePermissionView(DataTable dtroles, DataTable dtuser)
        {
            int add_count = 0;
            DataTable dt = dtroles.Copy();
            foreach (DataRow urow in dtuser.Rows)
            {
                int usid = Convert.ToInt32(urow["SEQ_ID"]);
                DataRow[] dtrows = dt.Select("SEQ_ID = " + usid);
                if (dtrows.Length > 0)
                {
                    int rac = Convert.ToInt16(dtrows[0]["AccessType"]);
                    int uac = Convert.ToInt16(urow["AccessType"]);
                    DateTime[] edt = getEffDates(dtrows[0], urow);
                    if (rac != ACCESS_NONE)
                    {
                        if (uac == ACCESS_NONE)
                        {
                            if (edt[1].CompareTo(edt[0]) >= 0) // user eff. date is newer
                            {
                                dtrows[0]["AccessType"] = urow["AccessType"];
                            }
                        }
                    }
                    else if (uac != ACCESS_NONE)
                    {
                        if (edt[1].CompareTo(edt[0]) >= 0) // user eff. date is newer
                        {
                            dtrows[0]["AccessType"] = urow["AccessType"];
                        }

                    }
                }
                else // do we need to add this? how can this happen
                {
                    DataRow arow = dt.NewRow();
                    for (int i = 0; i < dtuser.Columns.Count; i++)
                    {
                        if (dt.Columns[i].ColumnName.Equals("Role")) continue;
                        arow[i] = urow[i];
                    }
                    arow["Role"] = dtroles.Rows[0]["Role"];
                    dt.Rows.Add(arow);
                    add_count++;
                }
            }

            if (add_count > 0)
            {
                dt.DefaultView.Sort = "MenuGroup, MenuParentID, MenuItemNo";
                return dt.DefaultView.ToTable();
            }

            return dt;
        }

         private DataTable mergedPermissionView(String userid, String brcode, short role)
        {
            Message = String.Empty;

            DataTable dtroles = new irsDefaultProfile().getRolePermissionView(role);
            if (dtroles == null || dtroles.Rows.Count == 0)
            {
                Message = "Permissions unreadable for role: " + role + " " + Message;
                return null;
            }
            DataTable dtuser = getUserPermissionsViewExt(userid, brcode, role);
            if (dtuser == null || dtuser.Rows.Count == 0) return dtroles;

            return updatePermissionView(dtroles, dtuser);
        }

        internal DataTable getMenuPermissionView(String userid, String brcode, short role)
        {
            DataTable dt = null;

            try
            {
                dt = mergedPermissionView(userid, brcode, role);
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            if (dt != null && dt.Rows.Count > 0)
            {
                return filter_menu(dt, userid);
            }

            return dt;
        }

        internal irsModuleProfile()
        {
            this.TableName = "TBL_USER_PROFILE_EXT";
            ready = false;
            new Thread(new ThreadStart(load_access_types)).Start();
        }

        public void getReportLookup()
        {
            base.getUpdateDataSet("sp_get_user_profile_defaults_tmp");
        }
    }
}
