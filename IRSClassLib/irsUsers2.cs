using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading;

namespace IRSClassLib
{
    [Serializable]
    public class irsUsers2 : sqlHelper
    {
        internal static TriDes getTriDes()
        {
            return new TriDes("xfm92.3", "IRSWorld");
        }

        public short FLAG_INVALID = 0;
        public short FLAG_NEW = 1;
        public short FLAG_ACTIVE = 2;
        public short FLAG_EXPIRED = 3;
        public short FLAG_DISABLED = 4;


        private Object[] search_args;

        private DataSet search_ds;
        private irsPasswordArchive archer;

        public bool hasModuleProfile(String userid, String brcode, short role)
        {
            return new irsModuleProfile().hasModulePermission(userid, brcode, role);
        }

        public bool removeModuleProfile(String userid, String brcode, short role)
        {
            return new irsModuleProfile().removeModulePermission(userid, brcode, role);
        }

        public bool isBranchAdmin(short role)
        {
            DataTable dt = getUserRoles(role);
            if (dt == null || dt.Rows.Count < 1) return false;

            String srole = dt.Rows[0]["fldCode"].ToString();
            return (srole.ToLower().IndexOf("branch admin") > -1);
        }

        public bool isSystemAdmin(short role)
        {
            DataTable dt = getUserRoles(role);
            if (dt == null || dt.Rows.Count < 1) return false;

            String srole = dt.Rows[0]["fldCode"].ToString();
            return (srole.ToLower().IndexOf("system admin") > -1);
        }

        public DataTable getBranches()
        {
            try
            {
                DataSet ds = getDataSet(getSqlCommand("get_dropdown_branches"));
                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            catch { }

            return null;
        }

        public int getDefaultExpireDays()
        {
            try
            {
                DataTable dt = getLookup(new Object[] { "Password", DBNull.Value, -1 });
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow[] rows = dt.Select("fldCode = 'Days Expired'");
                    if (rows.Length > 0)
                    {
                        int i = Convert.ToInt32(rows[0]["fldValue"]);
                        return Convert.ToInt32(rows[0]["fldValue"]);
                    }
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return 90;
        }

        public DateTime getServerDate()
        {
            return base.ServerDate;
        }

        private bool setPassword(String pwd, bool reset)
        {
            bool ok = setColumn("Password", irsUsers.getTriDes().Encrypt(pwd));
            {
                if (!reset)
                {
                    if (ok & archer != null)
                    {
                        String errmsg = "";
                        ok = archer.archivePassword(pwd, out errmsg);
                        if (!ok) Message = errmsg;
                        archer = null;
                    }
                }
            }

            return ok;
        }

        private bool setPassword(String pwd)
        {
            return setPassword(pwd, false);
        }

        public bool resetPassword(String userid, out String retval)
        {
            retval = "";

            if (user_exist("USERID", userid) && edit())
            {
                String username = "";
                try
                {
                    username = getUserName((String)getColumn("CIFID"));
                }
                catch { }

                if (setFlag(FLAG_NEW))
                {
                    if (setChangeDate(DateTime.Now))
                    {
                        return save_user("reset", out retval);
                    }
                }
            }
            return false;
        }

        private bool setChangeDate(DateTime cdate)
        {
            return setColumn("LastPasswordChange", cdate);
        }

        private bool setDateCreated(DateTime cdate)
        {
            return setColumn("DateCreated", cdate);
        }

        private bool setFlag(short sflag)
        {
            bool ok = setColumn("Flags", sflag);
            if (ok) ClientData._flag = sflag;
            return ok;
        }

        private bool setDisabled(String userid, bool disabled)
        {
            if (!isUpdatable || !getColumn("UserID").ToString().Equals(userid))
            {
                if (!user_exist("USERID", userid))
                {
                    Message = "User ID (" + userid + ") does not exist!";
                    return false;
                }
            }

            if (setFlag(((disabled) ? FLAG_DISABLED : FLAG_EXPIRED)))
            {
                return base.save(sqlHelper.RETAIN_DATASET);
            }

            return false;
        }

        private bool setLastAccess(String userid)
        {
            if (!isUpdatable || !getColumn("UserID").ToString().Equals(userid))
            {
                if (!user_exist("USERID", userid))
                {
                    Message = "User ID (" + userid + ") does not exist!";
                    return false;
                }
            }

            if (setColumn("LastAccessDate", DateTime.Now))
            {
                return base.save(sqlHelper.RETAIN_DATASET);
            }

            return false;
        }

        private bool setLogon(bool logged)
        {
            String msg = "";
            try
            {
                //Use Application state to set logon state
                if (ClientData.UserId == null || ClientData.UserId.Length == 0)
                {
                    Message = "UserId must be present to establish state.";
                    return false;
                }
                System.Web.HttpApplicationState astate = System.Web.HttpContext.Current.Application;
                Hashtable ht = (Hashtable)astate.Contents[ClientData.UserId];
                if (ht == null)
                {
                    ht = new Hashtable();
                    ht.Add(ClientData.Location, logged);
                }
                else
                {
                    ht[ClientData.Location] = logged;
                }
                astate.Lock();
                astate.Contents[ClientData.UserId] = ht;
                astate.UnLock();

                return true;
            }
            catch (Exception e)
            {
                msg = e.Message;
            }

            return false;
        }

        private bool isLoggedOn(String uid, String loc)
        {
            Hashtable ht = (Hashtable)System.Web.HttpContext.Current.Application.Contents[uid];
            if (ht == null) return false;

            /* disable as per uat
            try
            {
                return (bool)ht[loc];
            }
            catch { }
            */

            return false;
        }

        internal void logout(String userid)
        {
            if (user_exist("USERID", userid))
            {
                setLogon(false);
            }
            System.Web.HttpContext.Current.Session.Abandon();
        }

        private bool password_valid(String pwd, String orig_pwd, out String errmsg)
        {
            errmsg = "";
            bool ok = (pwd.Equals(irsUsers.getTriDes().Decrypt(orig_pwd)));
            if (!ok)
            {
                errmsg = "The Password supplied is not correct!";
            }
            else
            {
                int ndays = Convert.ToInt32(getColumn("DaysExpired"));
                DateTime dt = Convert.ToDateTime(getColumn("LastPasswordChange")).AddDays(ndays);
                if (DateTime.Today.CompareTo(dt) > 0)
                {
                    ok = setFlag(FLAG_EXPIRED);
                    if (ok)
                    {
                        ok = save(sqlHelper.RETAIN_DATASET);
                    }
                    errmsg = "The password is expired.";
                    ok = false;
                }
            }

            return ok;
        }

        public override bool setColumn(string col, object val)
        {
            if (!addMode & col.Equals("userid"))
            {
                Message = "USERID cannot be modified.";
                return false;
            }
            return base.setColumn(col, val);
        }

        public String getUserName(String cifid)
        {
            String sql = "SELECT LastName, FirstName, MiddleName FROM TBL_CIF_MASTER WHERE CIFID = '" + cifid + "'";
            DataSet ds = getDataSet(getSqlCommand(sql, false));
            if (ds != null)
            {
                String fn = ds.Tables[0].Rows[0]["FirstName"].ToString().Trim();
                String mn = ds.Tables[0].Rows[0]["MiddleName"].ToString().Trim();
                String ln = ds.Tables[0].Rows[0]["LastName"].ToString().Trim();

                String s = fn + " " + mn + " " + ln;

                return s.Trim();
            }

            return String.Empty;
        }

        private String getEmails(String cifid)
        {
            String sql = "SELECT EmailAddress FROM TBL_CIF_CONTACTS WHERE CIFID = '" + cifid + "'";
            DataSet ds = getDataSet(getSqlCommand(sql, false));
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (row["EmailAddress"] != DBNull.Value)
                    {
                        String s = row["EmailAddress"].ToString().Trim();
                        if (s.Length > 0)
                        {
                            sb.Append(s + ";");
                        }
                    }
                }

                return sb.ToString();
            }

            return String.Empty;
        }

        public bool save_user(String crud, out String retval)
        {
            retval = "";
            bool ok = true;
            if (crud.ToLower().Equals("add") | crud.ToLower().Equals("reset"))
            {
                String cifid = (String)getColumn("CIFID");
                String username = String.Empty;
                try
                {
                    username = getUserName(cifid);
                }
                catch { }
                String userid = (String)getColumn("UserID");
                String gen_pwd = getDefaultPassword(username, userid);

                if (setPassword(gen_pwd))
                {
                    retval = userid + "~" + username + "~" + cifid + "~" + getEmails(cifid); ;
                }
            }
            if (ok)
            {
                ok = base.save(sqlHelper.RETAIN_DATASET);
                if (ok & search_cmd != null)
                {
                    search_ds = getDataSet(search_cmd);
                }
            }

            return ok;
        }

        private String disclose_password(String userid)
        {
            String sql = "SELECT Password FROM tbl_users WHERE UserID = '" + userid + "'";
            Object o = execScalar(sql);
            if (o == null || o == DBNull.Value)
            {
                return null;
            }

            String spwd = irsUsers.getTriDes().Decrypt(o.ToString());
            return spwd;
        }

        public String disclose_password(String userid, String disc_pwd)
        {
            String disc_userid = ClientData.UserId;
            if (disc_userid == null || disc_userid == String.Empty)
            {
                Message = "The discloser is unknown!";
                return null;
            }

            Object[] rs = validate(disc_userid, disc_pwd, 0, true);

            if ((bool)rs[0])
            {
                return disclose_password(userid);
            }
            else
            {
                Message = (String)rs[1];
            }

            return null;
        }


        public bool email_password(String userid, String emailad)
        {
            try
            {
                if (user_exist("USERID", userid))
                {
                    String username = getUserName(getColumn("CIFID").ToString());
                    String emsg = "Dear " + username + ",";
                    emsg += "\r\n\r\nThank you for registering with IRSWorld.";
                    emsg += "\r\n\r\nYour User id: " + userid;
                    emsg += "\r\nYour Password: " + disclose_password(userid);
                    emsg += "\r\n\r\nTHIS IS A COMPUTER GENERATED MSG.\r\nNO REPLY REQUIRED.";
                    String[] to = new String[] { emailad };

                    String subject = "PNB IRSWorld Registration";
                    csMailer mail = new csMailer();
                    bool ok = mail.sendMail(subject, to, emsg);
                    if (!ok)
                    {
                        Message = mail.Message;
                    }
                    return ok;
                }
                Message = "User: " + userid + " does not exist.";
            }
            catch (Exception e)
            {
                Message = e.Message;
            }
            return false;
        }

        public bool edit_user(String userid, DateTime dt)
        {
            bool ok = false;
            if (user_exist("USERID", userid) && edit())
            {
                ok = setColumn("lastModified", dt);
            }
            return ok;
        }

        public bool delete_user(String userid, int rowIndex)
        {
            bool ok = false;

            if (user_exist("USERID", userid)) // load the row
            {
                ok = new irsModuleProfile().deletePermissions(userid);
                if (ok)
                {
                    selectedRow = 0;
                    ok = Delete(sqlHelper.RETAIN_DATASET);
                }
            }
            else
            {
                base.Message = "User " + userid + " does not exist";
            }

            return ok;
        }

        private String getDefaultPassword(String username, String userid)
        {
            csPasswordGen pwdgen = new csPasswordGen();
            String gen_password = pwdgen.generatePassword(username, userid);
            return gen_password;
        }

        public bool new_user()
        {
            // the user is assumed to have done a search prior to calling 
            // this method. Anyway try to detect if a search is done;
            bool retainds = (base.isUpdatable);
            if (!retainds)
            {
                base.Message = "A search must be done prior to adding a user";
                return false;
            }
            bool ok = base.newRow(retainds);
            if (ok)
            {
                String errmsg = "";
                if (!this.setDateCreated(DateTime.Now)) errmsg += "setDateCreated: " + this.Message + "\n";
                if (!this.setChangeDate(DateTime.Now)) errmsg += "setChangeDate: " + this.Message + "\n";
                if (!this.setFlag(this.FLAG_NEW)) errmsg += "setFlag: " + this.Message + "\n";

                if (errmsg.Length > 0)
                {
                    ok = false;
                    base.Message = errmsg;
                }
            }
            return ok;
        }

        public bool user_exist(String key, String value)
        {
            getUpdateDS("sp_getUser", new Object[] { key, value });

            if (hasRows)
            {
                selectedRow = 0;
                return true;
            }
            return false;
        }

        private bool isReusable(String userid, String pwd)
        {
            if (archer == null) archer = new irsPasswordArchive();
            String errmsg = "";
            if (!archer.isReusable(userid, pwd, out errmsg))
            {
                Message = errmsg;
                return false;
            }
            return true;
        }

        public Object[] new_password(String userid, String new_pwd)
        {
            bool bflag = false;
            String reason = "";
            if (ClientData.UserId.Equals(userid) & (ClientData._flag == this.FLAG_NEW | ClientData._flag == this.FLAG_EXPIRED))
            {
                if (user_exist("USERID", userid))
                {
                    if (ClientData._flag == FLAG_EXPIRED & (!isReusable(userid, new_pwd)))
                    {
                        reason = "Password cannot be reused at this time.";
                    }
                    else
                    {
                        if (new csPasswordGen().password_valid(userid, new_pwd, out reason) && setPassword(new_pwd))
                        {
                            if (setChangeDate(DateTime.Now))
                            {
                                if (setFlag(this.FLAG_ACTIVE))
                                {
                                    String retval = "";
                                    if (save_user("Edit", out retval))
                                    {
                                        bflag = true;
                                        reason = "Active";
                                        register_clientdata(userid, true, FLAG_ACTIVE);
                                    }
                                    else
                                    {
                                        reason = "save_user: ";
                                    }
                                }
                                else
                                {
                                    reason = "setActive: ";
                                }
                            }
                            else
                            {
                                reason = "setChangeDate: ";
                            }
                        }
                        else
                        {
                            reason = "setPassword: " + reason + " ";
                        }
                        if (!bflag)
                        {
                            reason += Message;
                        }
                    }
                }
                else
                {
                    Message = "Inconsitent operation!" + userid + " does not exist.";
                    bflag = false;
                    reason = Message;
                }
            }
            return new Object[] { bflag, reason, userid };
        }

        public Object[] change_password(String userid, String old_pwd, String new_pwd)
        {
            bool bflag = false;
            String reason = String.Empty;

            if (!new csPasswordGen().password_valid(userid, new_pwd, out reason))
            {
                return new Object[] { false, reason };
            }

            Object[] ov = validate(userid, old_pwd, 1, false);
            if (!(bool)ov[0])
            {
                return new Object[] { ov[0], ov[1] };
            }

            try
            {
                short flag = ClientData._flag;
                if (flag > 0 & flag < 4)
                {
                    String uid = ClientData._userid;
                    if (uid != null && uid.Length > 0 && uid.Equals(userid))
                    {
                        if (isReusable(userid, new_pwd))
                        {
                            if (user_exist("USERID", userid))
                            {
                                if (setPassword(new_pwd))
                                {
                                    if (setChangeDate(DateTime.Now))
                                    {
                                        if (setFlag(FLAG_ACTIVE))
                                        {
                                            ClientData._flag = FLAG_ACTIVE;
                                            String retval = "";
                                            if (save_user("Edit", out retval))
                                            {
                                                bflag = true;
                                            }
                                            else
                                            {
                                                reason = "save failed: " + Message;
                                            }
                                        }
                                        else
                                        {
                                            reason = "setFlag failed: " + Message;
                                        }
                                    }
                                    else
                                    {
                                        reason = "setChangeDate failed: " + Message;
                                    }
                                }
                                else
                                {
                                    reason = "setPassword failed: " + Message;
                                }
                            }
                        }
                        else
                        {
                            reason = base.Message;
                        }
                    }
                    else
                    {
                        reason = "User must be logged-in for password change.";
                    }
                }
                else
                {
                    reason = "The user is locked or disabled";
                }
            }
            catch (Exception e)
            {
                base.Message = e.Message;
            }

            return new Object[] { bflag, reason };
        }

        public Object[] validate(String userid, String pwd, byte tries, bool ovride)
        {
            bool bflag = false;
            short iflag = 0;
            String reason = "";

            bool ok = true;

            if (!user_exist("USERID", userid))
            {
                Message = "User ID (" + userid + ") does not exist!";
                ok = false;
            }

            if (!ovride)
            {
                if (isLoggedOn(userid, (String)getColumn("Location")))
                {
                    Message = "User ID (" + userid + ") is currently logged on.";
                    ok = false;
                }
            }

            if (!ok) return new Object[] { false, Message, userid, tries, (short)-1 };

            if (tries <= 3)
            {
                iflag = (short)table.Rows[0]["Flags"];
                bflag = (iflag == FLAG_ACTIVE);
                if (iflag == FLAG_ACTIVE | iflag == FLAG_NEW)
                {
                    bflag = password_valid(pwd, (String)table.Rows[0]["Password"], out reason);
                    if (!bflag)
                    {
                        if (Convert.ToInt16(getColumn("Flags")) != FLAG_EXPIRED)
                        {
                            iflag = 0;
                        }
                    }
                    else
                    {
                        if (iflag == FLAG_NEW) bflag = false;
                    }
                }

                if (iflag > 0)
                {
                    DataTable lookup = null;
                    try
                    {
                        lookup = getLookup(new Object[] { "User Flags", DBNull.Value, iflag });
                    }
                    catch { }

                    if (lookup == null || lookup.Rows.Count == 0)
                    {
                        throw new Exception("The lookup table is not in sync. User Flag: " + iflag + ": " + base.Message);
                    }
                    reason = (bflag) ? (String)lookup.Rows[0]["FldCode"] : (String)lookup.Rows[0]["fldDescription"];
                }
            }
            else
            {
                bflag = false;
                reason = "Failed Attempts exceeded allowed tries.";
                setDisabled(userid, true);
            }

            return new Object[] { bflag, reason, userid, tries, iflag };
        }

        private void register_clientdata(String userid, bool bflag, short iflag)
        {
            ClientData._userid = userid;
            if (bflag)
            {
                ClientData._cifid = (String)table.Rows[0]["CIFID"];
                ClientData._cifclass = (short)table.Rows[0]["Mode"];
                ClientData._userid = (String)table.Rows[0]["UserId"];
                ClientData._location = (String)table.Rows[0]["Location"];
                ClientData._role = (short)table.Rows[0]["Role"];
                ClientData._flag = (short)table.Rows[0]["Flags"];
                ClientData._xdays = (short)table.Rows[0]["DaysExpired"];

                if (iflag > FLAG_INVALID & iflag < FLAG_DISABLED)
                {
                    setLogon(true);
                    setLastAccess(userid);
                }
            }
            else
            {
                ClientData._flag = iflag;
            }
            ClientData._tries++;
        }

        public Object[] validate(String userid, String pwd)
        {
            byte tries = ClientData._tries;
            Object[] rs = validate(userid, pwd, tries, false);
            bool bflag = (bool)rs[0];
            short iflag = (short)rs[4];
            if (tries <= 3)
            {
                register_clientdata(userid, bflag, iflag);
            }

            return rs;
        }

        internal String getUserLocation(String userid)
        {
            if (user_exist("USERID", userid))
            {
                try
                {
                    return table.Rows[0]["Location"].ToString();
                }
                catch (Exception e)
                {
                    Message = e.Message;
                }
            }

            return null;
        }

        internal short getUserRole(String userid)
        {
            if (user_exist("USERID", userid))
            {
                try
                {
                    return Convert.ToInt16(table.Rows[0]["Role"]);
                }
                catch (Exception e)
                {
                    Message = e.Message;
                }
            }

            return -1;
        }

        public DataTable getUserRoles()
        {
            return getLookup(new Object[] { "User Roles", DBNull.Value, -1 });
        }

        public DataTable getUserRoles(short role)
        {
            return getLookup(new Object[] { "User Roles", DBNull.Value, role });
        }

        public DataTable getUserFlags()
        {
            return getLookup(new Object[] { "User Flags", DBNull.Value, -1 });
        }

        public DataTable getAccessTypes()
        {
            return getLookup(new Object[] { "User Access Types", DBNull.Value, -1 });
        }

        public bool is_setup()
        {
            return ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"])._is_setup;
        }

        public DataTable getSearchTable()
        {
            return search_ds.Tables[0];
        }

        private DataTable define_search_master()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("CIFID", typeof(System.String)));
            dt.Columns.Add(new DataColumn("UserID", typeof(System.String)));
            dt.Columns.Add(new DataColumn("FullName", typeof(System.String)));
            dt.Columns.Add(new DataColumn("DateCreated", typeof(System.DateTime)));
            dt.Columns.Add(new DataColumn("LastModified", typeof(System.DateTime)));
            dt.Columns.Add(new DataColumn("LastAccessDate", typeof(System.DateTime)));
            dt.Columns.Add(new DataColumn("LastPasswordChange", typeof(System.DateTime)));
            dt.Columns.Add(new DataColumn("DaysExpired", typeof(System.Int16)));
            dt.Columns.Add(new DataColumn("DateExpired", typeof(System.DateTime)));
            dt.Columns.Add(new DataColumn("Mode", typeof(System.Int16)));
            dt.Columns.Add(new DataColumn("Flags", typeof(System.Int16)));
            dt.Columns.Add(new DataColumn("FlagDesc", typeof(System.String)));

            return dt;
        }

        public DataTable define_search_details()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add(new DataColumn("UserID", typeof(System.String)));
            dt.Columns.Add(new DataColumn("location", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Role", typeof(System.Int16)));
            dt.Columns.Add(new DataColumn("LoggedOn", typeof(System.Boolean)));
            dt.Columns.Add(new DataColumn("setPrimary", typeof(System.Boolean)));
            dt.Columns.Add(new DataColumn("RoleDesc", typeof(System.String)));
            dt.Columns.Add(new DataColumn("BranchName", typeof(System.String)));
            dt.Columns.Add(new DataColumn("hasModuleProfile", typeof(System.Int32)));

            return dt;
        }

        public DataTable getLocRole(String userid)
        {
            DataTable dt = define_search_details();

            if (search_ds != null && search_ds.Tables.Count > 0 && search_ds.Tables[0].Rows.Count > 0)
            {
                DataRow[] rows = search_ds.Tables[0].Select("UserID = '" + userid + "'");
                if (rows.Length > 0)
                {
                    for (int i = 0; i < rows.Length; i++)
                    {
                        DataRow nrow = dt.NewRow();
                        foreach (DataColumn col in dt.Columns)
                        {
                            nrow[col.ColumnName] = rows[i][col.ColumnName];
                        }
                        dt.Rows.Add(nrow);
                    }
                }
            }

            return dt;
        }

        private bool not_distinct(DataTable dt, String userid)
        {
            if (dt.Rows.Count == 0) return false;

            return (dt.Select("UserID = '" + userid + "'").Length > 0);
        }

        private DataTable create_search_master()
        {
            DataTable dt = define_search_master();

            foreach (DataRow row in search_ds.Tables[0].Rows)
            {
                if (not_distinct(dt, row["UserId"].ToString())) continue;
                DataRow nrow = dt.NewRow();
                foreach (DataColumn col in dt.Columns)
                {
                    nrow[col.ColumnName] = row[col.ColumnName];
                }
                dt.Rows.Add(nrow);
            }

            return dt;
        }

        public DataTable search(int cat, String args, String brcode, int role, int flag, int logged)
        {
            String[] catab = { "USERID", "USERNAME", "DATECREATED", "DATEEXPIRED" };

            int cix = cat - 1;

            if (cat > catab.Length)
            {
                Message = "Category not supported";
                return null;
            }

            try
            {
                search_cmd = getSqlCommand("sp_SearchUsers2");
                deriveParameters(search_cmd, new Object[] { catab[cix], args, brcode, (short)role, (short)flag, (short)logged });
                search_ds = getDataSet(search_cmd);
                if (search_ds != null && search_ds.Tables.Count > 0)
                {
                    return create_search_master();
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return null;
        }

        public bool savePermissions(String s)
        {
            String[] ss = s.Split('~');
            if (ss.Length > 0)
            {
                Hashtable ht = new Hashtable();
                foreach (String t in ss)
                {
                    if (t.Length == 0) break;
                    String[] tt = t.Split('=');
                    ht.Add(tt[0], tt[1]);
                }

                return new irsModuleProfile().savePermissions(ht);
            }
            base.Message = "Save failed. Insufficient parameters.";
            return false;
        }

        public DataTable getModulePermissions(String userid, String brcode, short role)
        {
            return new irsModuleProfile().getModulePermissions(userid, brcode, role);
        }

        private void setup_flags()
        {
            try
            {
                DataTable lookup = getLookup(new Object[] { "User Flags", DBNull.Value, -1 });
                if (lookup != null && lookup.Rows.Count > 0)
                {
                    foreach (DataRow row in lookup.Rows)
                    {
                        if (row["fldCode"].ToString().Equals("New User")) FLAG_NEW = (short)row["fldValue"];
                        if (row["fldCode"].ToString().Equals("Active")) FLAG_ACTIVE = (short)row["fldValue"];
                        if (row["fldCode"].ToString().Equals("Expired")) FLAG_EXPIRED = (short)row["fldValue"];
                        if (row["fldCode"].ToString().Equals("Disabled")) FLAG_DISABLED = (short)row["fldValue"];
                    }
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }
        }

        internal DataTable listUsers()
        {
            try
            {
                if (ClientData.UserId != null && ClientData.UserId.Length > 0)
                {
                    if (ClientData.isSystemAdmin)
                    {
                        return search(1, String.Empty, String.Empty, 0, (int)FLAG_ACTIVE, 0);
                    }
                    else
                    {
                        return search(1, String.Empty, ClientData.Location, 0, (int)FLAG_ACTIVE, 0);
                    }
                }
                Message = "Loggin is required or Session is expired.";
            }
            catch (Exception e)
            {
                Message = "@listUsers: " + e.Message;
            }

            return null;
        }

        public irsUsers2()
        {
            base.TableName = "TBL_Users";
            base.IdentityColumn = "UserID";
            setup_flags();
        }
    }
}
