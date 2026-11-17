using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Text;
using System.Threading;

namespace IRSClassLib
{
    [Serializable]
    public class irsUsers : sqlHelperWs
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
        public short FLAG_FORGOT = 5; // used by PasswordPage and ChallengeUser.aspx
        private bool flag_ready;

        private Object[] search_args;

        private DataSet search_ds;
        private irsPasswordArchive archer;

        private bool no_multi_loggin = false;
        private int param_password_tries = 0;

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
            /* RFC-31620 */
            return (srole.ToLower().IndexOf("parameter admin") > -1);
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
                        //int i = Convert.ToInt32(rows[0]["fldValue"]);
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
                    if (ok && archer != null)
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

        private void flag_wait()
        {
            while (!flag_ready)
            {
                Thread.Sleep(10);
            }
        }

        public bool is_old_user(String userid)
        {
            Object o = execScalar("SELECT count(*) from tbl_Password_Archive WHERE userid = '" + userid + "'");
            try
            {
                int i = Convert.ToInt32(o);
                if (i > 0) return true;
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return false;
        }

        public bool resetPassword(String userid, out String retval)
        {
            retval = "";
            string sUserID = string.Empty;
            sUserID = ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId;

            if (user_exist("USERID", userid) && edit())
            {
                String username = "";
                try
                {
                    username = getUserName((String)getColumn("CIFID"));
                }
                catch { }

                flag_wait();

                if (setFlag(FLAG_NEW))
                {
                    if (setChangeDate(DateTime.Now))
                    {
                        //IRSWorld 16236-009 TAccounts
                        new csAuditLog().writeAuditSystem(sUserID, "USER ADMIN", "PASSWORD RESET " + userid, retval, " Password Reset Successful!", csAuditLog.Category.Update);
                        return save_user("reset", out retval);
                    }
                }
            }
            new csAuditLog().writeAuditSystem(sUserID, "USER ADMIN", "PASSWORD RESET", userid, retval + " Password Reset Failed!", csAuditLog.Category.Update);
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
            flag_wait();

            try
            {
                short xflag = Convert.ToInt16(getColumn("Flags"));
                if (xflag > 8) return false;
            }
            catch { }

            return setFlag_Login(((disabled) ? FLAG_DISABLED : FLAG_EXPIRED));
        }

        private bool setLastAccess(String userid)
        {
            /* VAPT IAG 20170515 */
            string[] column = { "LastAccessDate" };
            object[] val = { DateTime.Now };
            return updateUser(userid, column, val);
        }

        private bool setLogon(bool logged)
        {
            #region Not ideal in NLB Environment. Returned to db implem.
            /*
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

            */
            #endregion

            /* VAPT IAG 20170515 */
            bool ok = false;
            String cursesid = (logged) ? System.Web.HttpContext.Current.Session.SessionID : null;
            string[] column = { "LoggedOn", "CurrentSessionID" };
            if (cursesid == null)
            {
                cursesid = string.Empty;
            }
            object[] val = { logged, cursesid };
            ok = updateUser(ClientData.UserDBDatatable.Rows[0]["UserID"].ToString(), column, val);

            try
            {
                if (AutoLog)
                {
                    int crud = (logged) ? (int)irsAuditLogger.CRUDS.LOGIN : (int)irsAuditLogger.CRUDS.LOGOUT;
                    AuditLogger.writeLog(crud);

                    if (crud == (int)irsAuditLogger.CRUDS.LOGOUT)
                    {
                        /* VAPT 20190206 */
                        new csAuditLog().writeVAPTLogger(ClientData.UserDBDatatable.Rows[0]["UserID"].ToString(), "LOGOUT", "USER HAS LOGOUT SUCCESSFULLY", "", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Information);
                    }
                }
            }
            catch { }

            return ok;
        }

        private bool not_current_session()
        {
            try
            {
                /* VAPT IAG 20170515 */
                Object o = ClientData.UserDBDatatable.Rows[0]["CurrentSessionID"];
                String old_session = (o != null && (o != DBNull.Value || o.ToString().IndexOf("DBNull") == -1)) ? o.ToString() : "";
                return (!old_session.Equals(System.Web.HttpContext.Current.Session.SessionID));
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return false;
        }

        private bool isLoggedOn(String uid)//, String loc)
        {
            #region Not ideal in NLB environment, returned to db implem.
            /*
            Hashtable ht = (Hashtable)System.Web.HttpContext.Current.Application.Contents[uid];
            if (ht == null) return false;

            if (no_multi_loggin)
            {
                try
                {
                    return (bool)ht[loc];
                }
                catch { }
            }

            return false;
             * */
            #endregion

            if (no_multi_loggin && not_current_session())
            {
                try
                {
                    /* VAPT IAG 20170529 */
                    DateTime dtla = new DateTime();
                    if (ClientData.UserDBDatatable.Rows[0]["LastAccessDate"] != null && ClientData.UserDBDatatable.Rows[0]["LastAccessDate"].ToString() != string.Empty)
                    {
                        dtla = Convert.ToDateTime(ClientData.UserDBDatatable.Rows[0]["LastAccessDate"]);
                    }
                    TimeSpan elaps = DateTime.Now.Subtract(dtla);
                    if (Convert.ToBoolean(ClientData.UserDBDatatable.Rows[0]["LoggedOn"]))
                    {
                        if (elaps.TotalMinutes > System.Web.HttpContext.Current.Session.Timeout)
                        {
                            setLogon(false);
                            return false;
                        }

                        return true;
                    }
                }
                catch (Exception e)
                {
                    base.Message = "Could not set login state: " + e.Message;
                    throw new Exception(base.Message);
                }
            }

            return false;
        }

        internal void logout(String userid)
        {
            if (userid != null && user_exist("USERID", userid))
            {
                try
                {
                    if (!ClientData._no_logout)
                    {
                        setLogon(false);
                        System.Web.HttpContext.Current.Session.Abandon();
                    }
                }
                catch (Exception e)
                {
                    base.Message = "Logout: " + e.Message;
                }
            }
        }

        private bool setPasswordExpired(out String errmsg)
        {
            errmsg = "";

            flag_wait();

            /*
            bool ok = setFlag(FLAG_EXPIRED);
            if (ok)
            {
                ok = save(sqlHelperWs.RETAIN_DATASET);
            }
            */

            bool ok = false;
            string[] column = { "Flags" };
            object[] val = { FLAG_EXPIRED };
            ok = updateUser(ClientData.UserDBDatatable.Rows[0]["UserID"].ToString(), column, val);

            if (!ok)
            {
                errmsg = "Error: " + Message;
            }
            else
            {
                errmsg = "The password is expired.";
            }

            return ok;
        }

        private bool password_valid(String pwd, String orig_pwd, out String errmsg)
        {
            errmsg = "";
            bool ok = false;
            /* VAPT IAG 20180830 */
            string dbPass = string.Empty;

            try
            {
                int i = 0;
                for (i = 0; i <= 3; i++)
                {
                    pwd = System.Web.HttpUtility.UrlDecode(pwd);
                }
                dbPass = irsUsers.getTriDes().Decrypt(orig_pwd);
                for (i = 0; i <= 3; i++)
                {
                    dbPass = System.Web.HttpUtility.UrlDecode(dbPass);
                }
                ok = (pwd.Equals(dbPass));
            }
            catch { }
            if (!ok)
            {
                errmsg = "The Password supplied is not correct!";
            }
            else
            {
                try
                {
                    /* VAPT IAG 20170515 */
                    String uloc = ClientData.UserDBDatatable.Rows[0]["Location"].ToString();
                    if (!uloc.Equals("USW"))
                    {
                        /* VAPT IAG 20170515 */
                        int ndays = Convert.ToInt32(ClientData.UserDBDatatable.Rows[0]["DaysExpired"]);
                        DateTime dt = Convert.ToDateTime(ClientData.UserDBDatatable.Rows[0]["LastPasswordChange"]).AddDays(ndays);
                        /* VAPT 20190206 */
                        if (DateTime.Today.CompareTo(dt) > 0 && Convert.ToInt32(ClientData.UserDBDatatable.Rows[0]["Role"]) != 99)
                        {
                            ok = setPasswordExpired(out errmsg);
                        }
                        //else
                        //{
                        //    /* VAPT IAG 20170516 */
                        //    DateTime dta = Convert.ToDateTime(ClientData.UserDBDatatable.Rows[0]["LastAccessDate"]);
                        //    short xflag = Convert.ToInt16(ClientData.UserDBDatatable.Rows[0]["Flags"]);
                        //    if (xflag == FLAG_ACTIVE)
                        //    {
                        //        TimeSpan span = DateTime.Now.Subtract(dta);
                        //        /* VAPT 20190618 */
                        //        if (span.Days > 30 && Convert.ToString(ClientData.UserDBDatatable.Rows[0]["Role"]) != "99")
                        //        {
                        //            ok = setPasswordExpired(out errmsg);
                        //        }
                        //    }
                        //}
                    }
                }
                catch { }
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
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
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
            String sql = "SELECT EmailAddress FROM TBL_CIF_CONTACTS WHERE CIFID = '" + cifid + "' AND ISNULL(EmailAddress, '') <> '' ORDER BY PreferredNo DESC";
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

        public String getEmailsByUserId(String userid)
        {
            if (user_exist("USERID", userid))
            {
                String cifid = (String)getColumn("CIFID");
                return getEmails(cifid);
            }

            Message = "User " + userid + " does not exist.";
            return null;
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
                    retval = userid + "~" + username + "~" + cifid + "~" + getEmails(cifid);
                }
            }
            if (ok)
            {
                //11072016 Audit Trail Report
                if (crud.ToLower().Equals("edit"))
                {
                    LogUserAdmin(crud, ok);
                    ok = base.save(sqlHelperWs.RETAIN_DATASET);

                    if (ok & search_args != null) //search_cmd != null)
                    {
                        doSearch();
                    }
                }
                else
                {
                    ok = base.save(sqlHelperWs.RETAIN_DATASET);
                    if (ok & search_args != null) //search_cmd != null)
                    {
                        doSearch();
                    }
                }
            }

            return ok;
        }


        public void LogUserAdmin(string crud, bool ok)
        {
            //Log Audit Trail Report
            string sUserID = string.Empty;
            sUserID = ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId;
            csAuditLog AuditLog = new csAuditLog();

            //User Administration - User Profile Maintenance 11072016
            string ValUserID = getColumn("UserID").ToString();
            string sCIFID = getColumn("CIFID").ToString();
            string sMode = getColumn("MODE").ToString();
            string sLocation = getColumn("LOCATION").ToString();
            string sDaysExpired = getColumn("DAYSEXPIRED").ToString();
            string sROLE = getColumn("ROLE").ToString();
            string sDateCreated = getColumn("DATECREATED").ToString();
            string sLastPasswordChange = getColumn("LASTPASSWORDCHANGE") == null ? "" : getColumn("LASTPASSWORDCHANGE").ToString(); //RFC-24048, IR 47193 DMP 03/10/2022
            string sFlags = getColumn("FLAGS").ToString();
            string sLoggedon = getColumn("LOGGEDON").ToString();

            string ListNewValue = "UserID:" + ValUserID + " | CIFID:" + sCIFID + " | MODE:" + sMode + " | LOCATION:" + sLocation + " | DAYSEXPIRED:" + sDaysExpired + " | ROLE:" + sROLE + " | DATECREATED:" + sDateCreated + " | LASTPASSWORDCHANGE:" + sLastPasswordChange + " | FLAGS:" + sFlags + " | LOGGEDON:" + sLoggedon;

            if (crud.ToLower().Equals("edit"))
            {
                DataTable GetOldVal = getOldValue(ValUserID);
                string OCifid = GetOldVal.Rows[0]["CIFID"].ToString();
                string OUserID = GetOldVal.Rows[0]["UserID"].ToString();
                string OLOCATION = GetOldVal.Rows[0]["LOCATION"].ToString();
                string ODATECREATED = GetOldVal.Rows[0]["DATECREATED"].ToString();
                string OLASTMODIFIED = GetOldVal.Rows[0]["LASTMODIFIED"].ToString();
                string OLASTACCESSDATE = GetOldVal.Rows[0]["LASTACCESSDATE"].ToString();
                string OLASTPASSWORDCHANGE = GetOldVal.Rows[0]["LASTPASSWORDCHANGE"].ToString();
                string ODAYSEXPIRED = GetOldVal.Rows[0]["DAYSEXPIRED"].ToString();
                string OROLE = GetOldVal.Rows[0]["ROLE"].ToString();
                string OFLAGS = GetOldVal.Rows[0]["FLAGS"].ToString();
                string OMODE = GetOldVal.Rows[0]["MODE"].ToString();
                string OLOGGEDON = GetOldVal.Rows[0]["LOGGEDON"].ToString();
                string OLOGINGROUP = GetOldVal.Rows[0]["LOGINGROUP"].ToString();

                string ListOldValue = "CIFID:" + OCifid + " | USERID:" + OUserID + " | LOCATION:" + OLOCATION + " | DATECREATED:" + ODATECREATED + " | LASTMODIFIED:" + OLASTMODIFIED + " | LASTACCESSDATE:" + OLASTACCESSDATE + " | LASTPASSWORDCHANGE:" + OLASTPASSWORDCHANGE + " | DAYSEXPIRED:" + ODAYSEXPIRED + " | ROLE:" + OROLE + " | FLAGS:" + OFLAGS + " | MODE:" + OMODE + " | LOGGEDON:" + OLOGGEDON + " | LOGINGROUP:" + OLOGINGROUP;

                if (ok == true)
                {
                    AuditLog.writeAuditSystem(sUserID, "USER ADMINISTRATOR", "User Profile Maintenance", "Record Save!" + " = " + ListNewValue, ListOldValue, csAuditLog.Category.Update);
                }
                else
                {
                    AuditLog.writeAuditSystem(sUserID, "USER ADMINISTRATOR", "User Profile Maintenance", "Record Failed to Save", ListOldValue, csAuditLog.Category.Update);
                }
            }
            else
            {

            }
        }

        public DataTable getOldValue(string ValUserID)
        {
            try
            {
                sqlHelperWs commons = new sqlHelperWs();
                DataSet ds = commons.execStoredProcedure("GET_OLD_USER_VALUE", new object[] { ValUserID });
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
                return null;
            }
            catch
            {
            }
            return null;
        }

        private String disclose_password(String userid, bool expire)
        {
            if (!user_exist("USERID", userid))
            {
                Message = "User: " + userid + " does not exist.";
                return null;
            }

            String spwd = irsUsers.getTriDes().Decrypt(getColumn("Password").ToString());

            bool ok = true;

            if (expire)
            {
                ok = setColumn("Flags", FLAG_EXPIRED);
                if (ok)
                {
                    ok = save(sqlHelperWs.RETAIN_DATASET);
                }
            }

            if (ok)
            {
                return spwd;
            }

            return null;
        }

        private String disclose_password(String userid)
        {
            return disclose_password(userid, false);
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

        public bool email_password(String userid, String emailad, String email_op)
        {
            //String h1 = "\r\n\r\nThank you for using PNB Web Remit";
            String h1 = "\r\n\r\nAs requested, your login credentials to IRSWorld is hereby disclosed.";
            String h2 = "";
            String h3 = "";
            String subjx = "";
            String pwd = "";
            switch (email_op.ToLower())
            {
                case "new":
                    subjx = "PNB Web Remit Registration";
                    pwd = disclose_password(userid);
                    break;
                case "reset":
                    subjx = "PNB Web Remit Password Reset";
                    pwd = disclose_password(userid);
                    break;
                case "disc":
                    //subjx = "PNB Web Remit Password Disclosure";
                    subjx = "PNB IRSWorld Login Keys Disclosure";
                    h2 = "\r\n\r\nYou will be prompted to change your password upon login to IRSWorld.";
                    pwd = disclose_password(userid, true);
                    break;
            }

            /* VAPT IAG 20180829 */
            if (base.is_mfa_authenticated())
            {
                return email_password(userid, emailad, pwd, h1, h2, subjx);
            }
            else
            {
                return false;
            }
        }

        private String[] getEmailBcc()
        {
            String s = new irsConfiguration().appsetting("signup_bcc");
            if (s != null && s.Length > 0)
            {
                String[] ss = s.Split(';');
                if (ss.Length > 0)
                {
                    bool bcc_enabled = Convert.ToBoolean(ss[0]);
                    if (bcc_enabled)
                    {
                        if (ss.Length > 1 && ss[1].Length > 0)
                        {
                            String[] bcc = ss[1].Trim().Split(',');
                            return bcc;
                        }
                    }
                }
            }

            return null;
        }

        public bool email_password(String userid, String emailad, String pwd, String h1, String h2, String subjx)
        {
            if (pwd != null)
            {
                try
                {
                    if (user_exist("USERID", userid))
                    {
                        String username = getUserName(getColumn("CIFID").ToString());
                        String emsg = "Dear " + username + ",";
                        emsg += h1;
                        emsg += "\r\n\r\nYour User ID: " + userid;
                        emsg += "\r\nYour Password: " + pwd;
                        if (h2.Length > 0)
                        {
                            emsg += h2;
                        }
                        try
                        {
                            if (ClientData.Location.Equals("USW"))
                            {
                                emsg += "\r\n\r\n\r\nSincerely yours,\r\nCustomer Support Department";
                                emsg += "\r\nPNB Remittance Centers, Inc. Online";
                            }
                        }
                        catch { }
                        emsg += "\r\n\r\nTHIS IS A COMPUTER GENERATED MESSAGE.\r\nNO REPLY REQUIRED.";
                        String[] to = new String[] { emailad };

                        String subject = subjx;
                        csMailer mail = new csMailer();
                        String[] bcc = getEmailBcc();
                        bool ok = false;
                        if (bcc != null && bcc.Length > 0)
                        {
                            ok = mail.sendMail(subject, to, null, emsg, null, false, bcc);
                        }
                        else
                        {
                            ok = mail.sendMail(subject, to, emsg);
                        }
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
                    ok = Delete(sqlHelperWs.RETAIN_DATASET);
                    if (ok)
                    {
                        ok = new irsPasswordArchive().deleteArchive(userid);
                    }
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
            #region Commented to use Password Generator
            /* commented to use password generator
            String pwd_default = "Passw0rd";
            try
            {
                DataTable lookup = getLookup(new Object[] { "Password", "Initial", -1 });
                if (lookup != null & lookup.Rows.Count > 0)
                {
                    pwd_default = (String)lookup.Rows[0]["fldDescription"];
                }
            }
            catch { }
             * 
            return pwd_default;
             * */
            #endregion

            csPasswordGen pwdgen = new csPasswordGen();
            String gen_password = pwdgen.generatePassword(username, userid);
            return gen_password;
        }

        public String getUserGroupByRole(String userid)
        {
            short urole = this.getUserRole(userid);
            if (urole > 0)
            {
                /* VAPT IAG 20170515 */
                Object[] o = { "User Roles", "Client", urole };
                DataTable dt = getLookup(o);

                if (dt != null && dt.Rows.Count > 0)
                {
                    return "IRSClient";
                }

                /*
                Object o = execScalar("SELECT FldValue FROM TBL_LookUp WHERE FldCode = 'Client' AND FldGroup = 'User Roles'");
                if (o != DBNull.Value)
                {
                    try
                    {
                        short role = Convert.ToInt16(o);
                        if (urole == role) return "IRSClient";
                    }
                    catch { }
                }
                */
                return "IRS";
            }

            return null;
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
                /* VAPT 20190820 */
                //if (!this.setChangeDate(DateTime.Now)) errmsg += "setChangeDate: " + this.Message + "\n";

                flag_wait();

                if (!this.setFlag(this.FLAG_NEW)) errmsg += "setFlag: " + this.Message + "\n";

                if (errmsg.Length > 0)
                {
                    ok = false;
                    base.Message = errmsg;
                }
            }
            return ok;
        }

        private int getMinUseridLength()
        {
            int uid_min = 6;

            DataTable dt = base.getLookup(new Object[] { "USERID", "MIN", -1 });

            if (dt != null && dt.Rows.Count > 0)
            {
                int j = Convert.ToInt32(dt.Rows[0]["fldValue"]);
                uid_min = (j < 5 | j > 50) ? 6 : j;
            }

            return uid_min;
        }


        public bool new_user(String userid)
        {
            if (userid == null || userid == String.Empty)
            {
                Message = "Invalid userid for add operation.";
                return false;
            }

            int uid_min = getMinUseridLength();

            if (userid.Length < uid_min)
            {
                Message = "Userid length is invalid.";
                return false;
            }

            if (!user_exist("USERID", userid))
            {
                return new_user();
            }

            Message = "User: " + userid + " already exist.";
            return false;
        }

        public bool user_exist(String key, String value)
        {
            System.Web.HttpContext.Current.Session["SES_LOG_USER_ID"] = null;

            if (isUpdatable && hasRows && table.Rows.Count == 1)
            {
                String s = getColumn(key).ToString();
                if (s.Equals(value)) return true;
            }

            getUpdateDS("sp_getUser", new Object[] { key, value });

            if (hasRows)
            {
                selectedRow = 0;

                if (string.IsNullOrEmpty(key) == false && key.ToLower() == "userid")
                {
                    System.Web.HttpContext.Current.Session["SES_LOG_USER_ID"] = value;
                }

                return true;
            }
            return false;
        }

        private bool isReusable(String userid, String pwd)
        {
            if (archer == null) archer = new irsPasswordArchive();
            String errmsg = "";
            archer.IsWinApp = base.IsWinApp;
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
            flag_wait();
            irsClientData icd = base.ClientData;
            if (icd != null)
            {
                if (icd.UserId.ToUpper().Equals(userid.Trim().ToUpper()))
                {
                    if (icd._flag == this.FLAG_NEW | icd._flag == this.FLAG_EXPIRED)
                    {
                        Message = "";

                        if (user_exist("USERID", userid))
                        {
                            bool is_reusable = isReusable(userid, new_pwd);

                            if (icd._flag == FLAG_EXPIRED & (!is_reusable))
                            {
                                /* VAPT 20190502 */
                                new csAuditLog().writeVAPTLogger(userid, "USER", "USER ATTEMPTED TO USE A RECYCLED PASSWORD", "", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Alert);
                                this.setFlag_Active();
                                reason = "Passwords cannot be reused for at least 6 months.";
                            }
                            else
                            {
                                if (!is_reusable)
                                {
                                    // There is are garbage in tbl_password_archive
                                    /* VAPT 20190502 */
                                    new csAuditLog().writeVAPTLogger(userid, "USER", "USER ATTEMPTED TO USE A RECYCLED PASSWORD", "", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Alert);
                                    reason = "There are records with similar userid in the password archive.";
                                }
                                else if (new csPasswordGen().password_valid(userid, new_pwd, out reason) && setPassword(new_pwd))
                                {
                                    if (setChangeDate(DateTime.Now))
                                    {
                                        if (setFlag(this.FLAG_ACTIVE))
                                        {
                                            String retval = "";
                                            if (save_user("Edit", out retval))
                                            {
                                                /* VAPT 20180222 */
                                                new csAuditLog().writeVAPTLogger(userid, "USER", "PASSWORD WAS SUCCESSFULLY CHANGED", "", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Information);
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
                    else
                    {
                        reason = "Inconsitent operation! not new or expired user." + icd.Flag;
                    }
                }
                else
                {
                    reason = "Session (" + icd.UserId + ") and login userid did not match.";
                }
            }
            else
            {
                reason = "Session Expired. " + Message;
            }

            if (!bflag)
            {
                /* VAPT 20180222 */
                new csAuditLog().writeVAPTLogger(userid, "USER", "CHANGE PASSWORD FAILED", reason, "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Alert);
            }
            return new Object[] { bflag, reason, userid };
        }

        /* VAPT 20190502 */
        private bool setFlag_Active()
        {
            string[] column = { "Flags" };
            object[] val = { FLAG_ACTIVE };
            return updateUser(ClientData.UserDBDatatable.Rows[0]["UserID"].ToString(), column, val);
        }

        public Object[] change_password(String userid, String old_pwd, String new_pwd)
        {
            string sUserID = string.Empty;
            sUserID = ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId;

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
                    /* VAPT IAG 20180830 */
                    if (uid != null && uid.Length > 0 && uid.ToUpper().Equals(userid.ToUpper()))
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
                                                /* VAPT 20180222 */
                                                new csAuditLog().writeVAPTLogger(userid, "USER", "CHANGE PASSWORD SUCCESSFUL", reason, "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Information);
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

            if (!bflag)
            {
                /* VAPT 20180222 */
                new csAuditLog().writeVAPTLogger(userid, "USER", "CHANGE PASSWORD FAILED", reason, "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Alert);
            }
            new csAuditLog().writeAuditSystem(sUserID, "MY PASSWORD", "CHANGE PASSWORD", "", userid + " | " + reason, csAuditLog.Category.Logon);
            return new Object[] { bflag, reason };
        }

        /* 06-01-2020 MBSS FIX */
        private void set_super_admin_active(String userid)
        {
            DataTable dt = null;

            string[] column = { "Flags" };
            object[] val = { 2 };

            try
            {
                dt = new sqlHelperWs().getDataSet("sp_getUser", true, new object[] { "USERID", userid }).Tables[0];

                if (dt != null && dt.Rows.Count > 0 && Convert.ToInt32(dt.Rows[0]["Role"]) == 99)
                {
                    ClientData.SuperAdmin = true;

                    if (Convert.ToInt32(dt.Rows[0]["Flags"]) == 1 || Convert.ToInt32(dt.Rows[0]["Flags"]) == 2)
                    {
                        return;
                    }
                    else
                    {
                        bool bol = updateUser(userid, column, val);
                    }
                }
            }
            catch
            {
            }
        }
        /* 06-01-2020 MBSS FIX */

        public Object[] validate(String userid, String pwd, byte tries, bool ovride)
        {
            bool bflag = false;
            short iflag = 0;
            String reason = "";
            string sUserID = string.Empty;
            sUserID = ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId;
            bool xflag = false;
            bool ok = true;

            if (!user_exist("USERID", userid))
            {
                Message = "User ID (" + userid + ") does not exist!";
                ok = false;
                //Added for Audit Log Report 11/18/2016
                new csAuditLog().writeAuditSystem(sUserID, "LOGIN", "LOGIN", "", userid + "|" + Message, csAuditLog.Category.Logon);
                /* VAPT 20180222 */
                new csAuditLog().writeVAPTLogger(userid, "LOGIN", "USER ID USED DOES NOT EXISTS", "", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Alert);
            }

            if (!ovride)
            {
                if (isLoggedOn(userid))
                {
                    TimeSpan ts = DateTime.Now.Subtract(Convert.ToDateTime(getColumn("LastAccessDate")));
                    int trem = System.Web.HttpContext.Current.Session.Timeout - ts.Minutes;
                    //Message = "User ID (" + userid + ") is currently logged on. Try again after " + ((trem == 0) ? "" : trem.ToString()) + " minutes.";
                    Message = "THIS USER ID (" + userid + ") IS CURRENTLY LOGGED IN.";
                    if (ClientData.Location == "USW")
                    {
                        Message += " PLEASE TRY AGAIN LATER.";
                    }
                    ClientData._no_logout = true;
                    ok = false;

                    //Added for Audit Log Report 11/18/2016
                    new csAuditLog().writeAuditSystem(sUserID, "LOGIN", "LOGIN", "", userid + "|" + Message, csAuditLog.Category.Logon);

                    /* VAPT 20180222 */
                    new csAuditLog().writeVAPTLogger(userid, "LOGIN", "USER IS CURRENTLY LOGIN", "", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Alert);
                }
            }

            if (!ok) return new Object[] { false, Message, userid, tries, (short)-9 };

            /* 06-01-2020 MBSS FINDINGS FIX */
            param_password_tries = getPasswordTries();

            if (ClientData != null)
            {
                if (ClientData.SuperAdmin == true)
                {
                    param_password_tries = 0;
                    tries = 0;
                }
            }
            /* 06-01-2020 MBSS FINDINGS FIX */


            /* VAPT 20190206 */
            //if (tries <= getPasswordTries()) ORIGINAL CODE COMMENTED MBSS FIX 06-01-2020

            if (tries <= param_password_tries) /* 06-01-2020 MBSS FINDINGS FIX */
            {
                flag_wait();

                iflag = (short)table.Rows[0]["Flags"];
                xflag = (iflag == FLAG_EXPIRED); // expired

                bflag = (iflag == FLAG_ACTIVE);
                if (iflag == FLAG_ACTIVE | iflag == FLAG_NEW | iflag == FLAG_EXPIRED)
                {
                    bflag = password_valid(pwd, (String)table.Rows[0]["Password"], out reason);
                    if (!bflag)
                    {
                        iflag = 0;
                        /* VAPT 20180222 */
                        new csAuditLog().writeVAPTLogger(userid, "LOGIN", "USER ATTEMPTED TO LOGIN WITH AN INCORRECT PASSWORD", "", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Alert);

                        /* 06-01-2020 MBSS FIX */
                        if (ClientData.SuperAdmin == false)
                        {
                            if (tries == getPasswordTries())
                            {
                                bflag = false;

                                if ((short)ClientData.UserDBDatatable.Rows[0]["Flags"] != 1)//DMP RFC-20327
                                {
                                    reason = "Failed Attempts exceeded allowed tries.";
                                    new csAuditLog().writeVAPTLogger(userid, "LOGIN", "USER HAS EXCEEDED THE NUMBER OF ALLOWABLE TRIES", "", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Alert);
                                    setDisabled(userid, true);
                                }
                                else
                                {
                                    reason = "The Password supplied is not correct.";
                                    new csAuditLog().writeVAPTLogger(userid, "LOGIN", "USER ATTEMPTED TO LOGIN WITH AN INCORRECT PASSWORD", "", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Alert);
                                }
                                new csAuditLog().writeAuditSystem(sUserID, "LOGIN", "LOGIN", "", userid + "|" + reason, csAuditLog.Category.Logon);
                            }
                        }
                        else
                        {
                            bflag = false;
                            reason = "Failed Attempts made by user with super admin role - " + userid;
                        }
                        /* 06-01-2020 MBSS FIX */

                        /* VAPT 20190206 */
                        //if (tries == getPasswordTries())
                        //{
                        //    bflag = false;
                        //    reason = "Failed Attempts exceeded allowed tries.";
                        //    /* VAPT 20180222 */
                        //    new csAuditLog().writeVAPTLogger(userid, "LOGIN", "USER HAS EXCEEDED THE NUMBER OF ALLOWABLE TRIES", "", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Alert);
                        //    setDisabled(userid, true);
                        //    //Added for Audit Log Report 11/18/2016
                        //    new csAuditLog().writeAuditSystem(sUserID, "LOGIN", "LOGIN", "", userid + "|" + reason, csAuditLog.Category.Logon);
                        //}
                    }
                    else
                    {
                        /* 06-01-2020 MBSS FIX */
                        new csAuditLog().writeVAPTLogger(userid, "LOGIN", "USER AUTHENTICATED SUCCESSFULLY", "", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Information);
                        if (reason.Length > 0 && reason.IndexOf("expired") > -1)
                        {
                            irsClientData id = base.ClientData;

                            if (!id.SuperAdmin)
                            {
                                iflag = 3;
                                new csAuditLog().writeVAPTLogger(userid, "LOGIN", "PASSWORD IS EXPIRED", "", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Alert);
                            }
                        }
                        /* 06-01-2020 MBSS FIX */

                        /* VAPT 20180222 */
                        //new csAuditLog().writeVAPTLogger(userid, "LOGIN", "USER AUTHENTICATED SUCCESSFULLY", "", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Information);
                        //if (reason.Length > 0 && reason.IndexOf("expired") > -1)
                        //{
                        //    iflag = 3;
                        //    /* VAPT 20180222 */
                        //    irsClientData id = base.ClientData;
                        //    if (!id.SuperAdmin)
                        //    {
                        //        new csAuditLog().writeVAPTLogger(userid, "LOGIN", "PASSWORD IS EXPIRED", "", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Alert);
                        //    }
                        //}

                        if (!new Entrust().ipGeolocation(userid)) //added by mike to handle 2fa IP geolocation 3/15/2018
                        {
                            if (System.Web.HttpContext.Current.Session["IP_GEO_STAT"] != null)
                            {
                                ok = false;
                                iflag = 0;
                                Message = "Login failed, Please contact your System Administrator";
                                /* VAPT 20180723 */
                                new csAuditLog().writeVAPTLogger(userid, "LOGIN", "HIGH RISK ACTIVITY FOUND", "", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Alert);
                            }
                        }
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
                /* 06-01-2020 MBSS FIX */
                if (ClientData != null)
                {
                    if (ClientData.SuperAdmin == false)
                    {
                        bflag = false;

                        if ((short)ClientData.UserDBDatatable.Rows[0]["Flags"] != 1)//DMP RFC-20327
                        {
                            reason = "Failed Attempts exceeded allowed tries.";
                            new csAuditLog().writeVAPTLogger(userid, "LOGIN", "USER HAS EXCEEDED THE NUMBER OF ALLOWABLE TRIES", "", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Alert);
                            setDisabled(userid, true);
                        }
                        else
                        {
                            reason = "The Password supplied is not correct.";
                            new csAuditLog().writeVAPTLogger(userid, "LOGIN", "USER ATTEMPTED TO LOGIN WITH AN INCORRECT PASSWORD", "", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Alert);
                        }
                        new csAuditLog().writeAuditSystem(sUserID, "LOGIN", "LOGIN", "", userid + "|" + reason, csAuditLog.Category.Logon);
                    }
                }
                /* 06-01-2020 MBSS FIX */

                //bflag = false;
                //reason = "Failed Attempts exceeded allowed tries.";
                ///* VAPT 20180222 */
                //new csAuditLog().writeVAPTLogger(userid, "LOGIN", "USER HAS EXCEEDED THE NUMBER OF ALLOWABLE TRIES", "", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Alert);
                //setDisabled(userid, true);
                ////Added for Audit Log Report 11/18/2016
                //new csAuditLog().writeAuditSystem(sUserID, "LOGIN", "LOGIN", "", userid + "|" + reason, csAuditLog.Category.Logon);
            }

            return new Object[] { bflag, reason, userid, tries, iflag };
        }

        private void unregister_clientdata()
        {
            ClientData._userid = String.Empty;
            ClientData._cifid = String.Empty;
            ClientData._cifclass = 0;
            ClientData._location = String.Empty;
            ClientData._role = 0;
            ClientData._flag = 0;
            ClientData._xdays = 0;
            ClientData._tries = 0;
            ClientData._no_logout = false;
            ClientData._login_group = 0;
            ClientData._disclaimer = false;
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

                try
                {
                    if (table.Rows[0]["LoginGroup"] != null && table.Rows[0]["LoginGroup"] != DBNull.Value)
                    {
                        ClientData._login_group = (short)table.Rows[0]["LoginGroup"];
                    }
                    else
                    {
                        ClientData._login_group = getLoginGroup(ClientData.Location);
                    }
                }
                catch (Exception e)
                {
                    Message = e.Message;
                    ClientData._login_group = (short)irsClientData.LoginGroups.US;
                }

                try
                {
                    int ndays = Convert.ToInt32(ClientData._xdays);
                    DateTime dt = Convert.ToDateTime(table.Rows[0]["LastPasswordChange"]).AddDays(ndays);

                    ClientData._xdays = (short)((TimeSpan)dt.Subtract(DateTime.Today)).Days;
                }
                catch
                {
                    ClientData._xdays = 90;
                }

                ClientData._no_logout = false;
                flag_wait();
                if (iflag > FLAG_INVALID & iflag < FLAG_DISABLED)
                {
                    setLogon(true);
                    setLastAccess(userid);
                    System.Web.HttpContext.Current.Session["USER_ON_LOGIN"] = null;
                }

                try
                {
                    Object o = execScalar("SELECT Disclaimer FROM tbl_cif_master WHERE CIFID = '" + ClientData.CIFID + "'");
                    ClientData.Disclaimer = Convert.ToBoolean(o);
                }
                catch
                {
                    ClientData.Disclaimer = false;
                }
                ClientData._tries = 0;
                return;
            }
            else
            {
                ClientData._flag = iflag;
            }
            ClientData._tries++;
        }

        public Object[] validate(String userid, String pwd)
        {
            /* DEC 2020 TO ADDRESS - CROSS SITE SCRIPTING REFLECTED. ADDED TRY CATCH TO ENSURE THAT IT WILL NOT DISPLAY SERVER SIDE RELATED MESSAGE */

            Object[] rs = null;
            irsClientData icd = base.ClientData;

            try
            {
                if (icd != null)
                {
                    String cap_icd_userid = "";
                    String cap_userid = userid.Trim().ToUpper();
                    if (icd.UserId != null && icd.UserId.Length > 0)
                    {
                        cap_icd_userid = icd.UserId.ToUpper();
                        if (!cap_icd_userid.Equals(cap_userid))
                        {
                            unregister_clientdata();
                        }
                    }
                    byte tries = icd._tries;
                    rs = validate(cap_userid, pwd, tries, false);
                    bool bflag = (bool)rs[0];
                    short iflag = (short)rs[4];

                    /* 1. MBSS 4.1 AUGUST 2023 TO HANDLE MULTIPLE LOGIN */
                    if (bflag == false && rs != null && rs.Length > 2)
                    {
                        if (rs[1].ToString().ToUpper().IndexOf("CURRENTLY LOGGED IN") >= 0)
                        {
                            return rs;
                        }
                    }
                    /* 1. MBSS 4.1 AUGUST 2023 TO HANDLE MULTIPLE LOGIN */

                    /* 06-01-2020 MBSS FIX */
                    if (ClientData != null)
                    {
                        if (ClientData.SuperAdmin == true)
                        {
                            register_clientdata_login(cap_userid, bflag, iflag);
                        }

                        if (ClientData.SuperAdmin == false)
                        {
                            if (tries <= getPasswordTries())
                            {
                                register_clientdata_login(cap_userid, bflag, iflag);
                            }
                        }
                    }
                    /* 06-01-2020 MBSS FIX */
                }
                else
                {
                    rs = new Object[] { false, "Session Expired. " + base.Message, -9 };
                }

                return rs;
            }
            catch
            {
                return new Object[] { false, "Session Expired.", -9 };
            }
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
            //if (user_exist("USERID", userid))
            /* VAPT IAG 20170515 */
            if (ClientData.UserExist)
            {
                try
                {
                    //return Convert.ToInt16(table.Rows[0]["Role"]);
                    return Convert.ToInt16(ClientData.UserDBDatatable.Rows[0]["Role"]);
                }
                catch (Exception e)
                {
                    Message = e.Message;
                }
            }

            return -1;
        }

        /* VAPT 20190206 */
        public int getPasswordTries()
        {
            DataTable dt = getLookup(new Object[] { "Password", "Password Tries", -1 });
            int ret = 0;

            if (dt != null && dt.Rows.Count > 0)
            {
                ret = Convert.ToInt32(dt.Rows[0]["FldValue"]) - 1;
            }
            else
            {
                ret = 2;
            }
            return ret;
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

        public short getLoginGroup(String brcode)
        {
            String sql = "SELECT ChargeGroup FROM TBL_Branches WHERE BranchCode = '" + brcode + "'";
            Object o = execScalar(sql);
            if (o != null && o != DBNull.Value)
            {
                try
                {
                    short si = Convert.ToInt16(o);
                    if (si != 3) return 2;
                }
                catch { }
            }

            return 1;
        }

        public DataTable getLoginGroup()
        {
            DataTable dt = getLookup(new Object[] { "LoginGroup", DBNull.Value, -1 });
            if (dt != null && dt.Rows.Count == 0)
            {
                DataTable dtclon = dt.Clone();

                DataRow row = dtclon.NewRow();
                row["FldGroup"] = "LoginGroup";
                row["FldCode"] = "Default";
                row["FldValue"] = 1;
                row["FldDescription"] = "RCI Including RCC";
                dtclon.Rows.Add(row);

                row = dtclon.NewRow();
                row["FldGroup"] = "LoginGroup";
                row["FldCode"] = "Eurasia";
                row["FldValue"] = 2;
                row["FldDescription"] = "Europe And Asia";
                dtclon.Rows.Add(row);

                return dtclon;
            }

            return dt;
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

        private void doSearch()
        {
            if (search_args == null)
            {
                Message = "Set search_args before search.";
                search_ds = null;
                return;
            }

            /* VAPT IAG 20180829 */
            if (base.is_mfa_authenticated())
            {
                search_ds = getDataSet("sp_SearchUsers", true, search_args);
            }
            else
            {
                search_ds = null;
            }
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
                /* VAPT 20180801 */
                string userID = ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId;
                search_args = new Object[] { catab[cix], args, brcode, (short)role, (short)flag, (short)logged, userID };
                doSearch();

                if (search_ds != null && search_ds.Tables.Count > 0)
                {
                    return search_ds.Tables[0];
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
                no_multi_loggin = Convert.ToBoolean(new irsConfiguration().appsetting("NoMultiLogin"));
            }
            catch { }

            try
            {
                DataTable lookup = getUserFlags();
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

            flag_ready = true;
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

        public irsUsers()
        {
            base.TableName = "TBL_Users";
            flag_ready = false;
            new Thread(new ThreadStart(setup_flags)).Start();
        }

        public irsUsers(bool is_win_app)
        {
            base.TableName = "TBL_Users";
            flag_ready = false;
            IsWinApp = is_win_app;
            new Thread(new ThreadStart(setup_flags)).Start();
        }

        /* VAPT IAG 20170515 */
        public bool user_exist_login(String key, String value)
        {
            System.Web.HttpContext.Current.Session["SES_LOG_USER_ID"] = null;

            if (isUpdatable && hasRows && table.Rows.Count == 1)
            {
                String s = getColumn(key).ToString();
                if (s.Equals(value)) return true;
            }

            /* 06-01-2020 MBSS FIX */
            this.set_super_admin_active(value);
            /* 06-01-2020 MBSS FIX */

            getUpdateDS("sp_getUser", new Object[] { key, value });
            if (hasRows)
            {
                selectedRow = 0;
                ClientData.UserDBDatatable = table;
                ClientData.UserExist = true;
                ClientData.UserLoginGroup = getLoginGroup(ClientData.UserDBDatatable.Rows[0]["Location"].ToString());
                try
                {
                    Object o = execScalar("SELECT Disclaimer FROM tbl_cif_master WHERE CIFID = '" + ClientData.CIFID + "'");
                    ClientData.Disclaimer = Convert.ToBoolean(o);
                }
                catch
                {
                    ClientData.Disclaimer = false;
                }

                if (string.IsNullOrEmpty(key) == false && key.ToLower() == "userid")
                {
                    System.Web.HttpContext.Current.Session["SES_LOG_USER_ID"] = value;
                }
                System.Web.HttpContext.Current.Session["USER_ON_LOGIN"] = null;
                return true;
            }
            System.Web.HttpContext.Current.Session["USER_ON_LOGIN"] = null;
            return false;
        }

        /* VAPT IAG 20170515 */
        private bool setFlag_Login(short sflag)
        {
            ClientData._flag = sflag;
            string[] column = { "Flags" };
            object[] val = { sflag };
            return updateUser(ClientData.UserDBDatatable.Rows[0]["UserID"].ToString(), column, val);
        }

        /* VAPT IAG 20170515 */
        public bool updateUser(string userID, string[] column, object[] val)
        {
            bool is_winapp = false;
            string sql = "SELECT TOP 1 * FROM tbl_Users WHERE UserID = '" + userID + "'";
            int i = 0;
            bool ok = false;

            is_winapp = base.IsWinApp;
            base.IsWinApp = true;
            base.addMode = false;
            base.getUpdateDataSet(getSqlCommand(sql, false));
            selectedRow = 0;

            for (i = 0; i <= (column.Length - 1); i++)
            {
                setColumn(column.GetValue(i).ToString(), val.GetValue(i).ToString());
            }
            ok = base.save(sqlHelper.RETAIN_DATASET);
            /* VAPT 20190419 */
            ClientData._flag = FLAG_EXPIRED;
            base.IsWinApp = is_winapp;
            return ok;
        }

        /* VAPT IAG 20170515 */
        public Object[] validate_login(String userid, String pwd)
        {
            /* DEC 2020 TO ADDRESS - CROSS SITE SCRIPTING REFLECTED. ADDED TRY CATCH TO ENSURE THAT IT WILL NOT DISPLAY SERVER SIDE RELATED MESSAGE */

            Object[] rs = null;
            irsClientData icd = base.ClientData;

            try
            {
                if (icd != null)
                {
                    String cap_icd_userid = "";
                    String cap_userid = userid.Trim().ToUpper();
                    if (icd.UserId != null && icd.UserId.Length > 0)
                    {
                        cap_icd_userid = icd.UserId.ToUpper();
                        if (!cap_icd_userid.Equals(cap_userid))
                        {
                            unregister_clientdata();
                        }
                    }
                    byte tries = icd._tries;
                    rs = validate_login(cap_userid, pwd, tries, false);
                    bool bflag = (bool)rs[0];
                    short iflag = (short)rs[4];

                    /* 2. MBSS 4.1 AUGUST 2023 TO HANDLE MULTIPLE LOGIN */
                    if (bflag == false && rs != null && rs.Length > 2)
                    {
                        if (rs[1].ToString().ToUpper().IndexOf("CURRENTLY LOGGED IN") >= 0)
                        {
                            return rs;
                        }
                    }
                    /* 2. MBSS 4.1 AUGUST 2023 TO HANDLE MULTIPLE LOGIN */

                    /* 06-01-2020 MBSS FIX */
                    if (ClientData != null)
                    {
                        if (ClientData.SuperAdmin == true)
                        {
                            register_clientdata_login(cap_userid, bflag, iflag);
                        }

                        if (ClientData.SuperAdmin == false)
                        {
                            if (tries <= getPasswordTries())
                            {
                                register_clientdata_login(cap_userid, bflag, iflag);
                            }
                        }

                    }
                    /* 06-01-2020 MBSS FIX */
                }
                else
                {
                    rs = new Object[] { false, "Session Expired. " + base.Message, -9 };
                }

                return rs;
            }
            catch
            {
                return new Object[] { false, "Session Expired.", -9 };
            }
        }

        /* VAPT IAG 20170515 */
        public Object[] validate_login(String userid, String pwd, byte tries, bool ovride)
        {
            bool bflag = false;
            short iflag = 0;
            String reason = "";
            string sUserID = ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId;
            bool xflag = false;
            bool ok = true;

            /* VAPT IAG 20170515 */
            if (!ClientData.UserExist)
            {
                Message = "User ID (" + userid + ") does not exist!";
                ok = false;
                //Added for Audit Log Report 11/18/2016
                new csAuditLog().writeAuditSystem(userid, "LOGIN", "LOGIN", "", userid + "|" + Message, csAuditLog.Category.Logon);
                /* VAPT 20180222 */
                new csAuditLog().writeVAPTLogger(userid, "LOGIN", "USER ID USED DOES NOT EXISTS", "", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Alert);
            }

            if (!ovride)
            {
                if (isLoggedOn(userid))
                {
                    //TimeSpan ts = DateTime.Now.Subtract(Convert.ToDateTime(getColumn("LastAccessDate")));
                    TimeSpan ts = DateTime.Now.Subtract(Convert.ToDateTime(ClientData.UserDBDatatable.Rows[0]["LastAccessDate"]));
                    int trem = System.Web.HttpContext.Current.Session.Timeout - ts.Minutes;
                    //Message = "User ID (" + userid + ") is currently logged on. Try again after " + ((trem == 0) ? "" : trem.ToString()) + " minutes.";
                    Message = "THIS USER ID (" + userid + ") IS CURRENTLY LOGGED IN.";
                    if (ClientData.Location == "USW")
                    {
                        Message += " PLEASE TRY AGAIN LATER.";
                    }
                    ClientData._no_logout = true;
                    ok = false;

                    //Added for Audit Log Report 11/18/2016
                    new csAuditLog().writeAuditSystem(userid, "LOGIN", "LOGIN", "", userid + "|" + Message, csAuditLog.Category.Logon);

                    /* VAPT 20180222 */
                    new csAuditLog().writeVAPTLogger(userid, "LOGIN", "USER IS CURRENTLY LOGIN", "", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Alert);
                }
            }

            if (!ok) return new Object[] { false, Message, userid, tries, (short)-9 };

            /* 06-01-2020 MBSS FINDINGS FIX */
            int param_password_tries = getPasswordTries();

            if (ClientData != null)
            {
                if (ClientData.SuperAdmin == true)
                {
                    param_password_tries = 0;
                    tries = 0;
                }
            }
            /* 06-01-2020 MBSS FINDINGS FIX */

            //if (tries <= 2) ORIGINAL CODE
            if (tries <= param_password_tries) /* 06 - 01 - 2020 MBSS FINDINGS FIX */   
            {
                flag_wait();
                /* VAPT 20170515 */
                iflag = (short)ClientData.UserDBDatatable.Rows[0]["Flags"];
                xflag = (iflag == FLAG_EXPIRED); // expired

                bflag = (iflag == FLAG_ACTIVE);
                if (iflag == FLAG_ACTIVE | iflag == FLAG_NEW | iflag == FLAG_EXPIRED)
                {
                    /* VAPT 20170515 */
                    bflag = password_valid(pwd, (String)ClientData.UserDBDatatable.Rows[0]["Password"], out reason);
                    if (!bflag)
                    {
                        iflag = 0;
                        /* VAPT 20180222 */
                        new csAuditLog().writeVAPTLogger(userid, "LOGIN", "USER ATTEMPTED TO LOGIN WITH AN INCORRECT PASSWORD", "", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Alert);
                    }
                    else
                    {
                        /* 06-01-2020 MBSS FIX */
                        if (ClientData.SuperAdmin == false)
                        {
                            new csAuditLog().writeVAPTLogger(userid, "LOGIN", "USER AUTHENTICATED SUCCESSFULLY", "", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Information);

                            if (reason.Length > 0 && reason.IndexOf("expired") > -1)
                            {
                                iflag = 3;
                                new csAuditLog().writeVAPTLogger(userid, "LOGIN", "USER ID IS EXPIRED", "", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Alert);
                            }
                        }
                        /* 06-01-2020 MBSS FIX */

                        /* VAPT 20180222 */
                        //new csAuditLog().writeVAPTLogger(userid, "LOGIN", "USER AUTHENTICATED SUCCESSFULLY", "", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Information);
                        //if (reason.Length > 0 && reason.IndexOf("expired") > -1)
                        //{
                        //    iflag = 3;
                        //    /* VAPT 20180222 */
                        //    new csAuditLog().writeVAPTLogger(userid, "LOGIN", "USER ID IS EXPIRED", "", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Alert);
                        //}

                        if (!new Entrust().ipGeolocation(userid)) //added by mike to handle 2fa IP geolocation 3/15/2018
                        {
                            if (System.Web.HttpContext.Current.Session["IP_GEO_STAT"] != null)
                            {
                                ok = false;
                                iflag = 0;
                                Message = "Login failed, Please contact your System Administrator";//System.Web.HttpContext.Current.Session["IP_GEO_STAT"].ToString();

                            }

                        }
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
                /* 06-01-2020 MBSS FIX */
                if (ClientData != null)
                {
                    if (ClientData.SuperAdmin == false)
                    {
                        bflag = false;

                        if ((short)ClientData.UserDBDatatable.Rows[0]["Flags"] != 1)//DMP RFC-20327
                        {
                            reason = "Failed Attempts exceeded allowed tries.";
                            new csAuditLog().writeVAPTLogger(userid, "LOGIN", "USER HAS EXCEEDED THE NUMBER OF ALLOWABLE TRIES", "", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Alert);
                            setDisabled(userid, true);
                        }
                        else
                        {
                            reason = "The Password supplied is not correct.";
                            new csAuditLog().writeVAPTLogger(userid, "LOGIN", "USER ATTEMPTED TO LOGIN WITH AN INCORRECT PASSWORD", "", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Alert);
                        }
                        new csAuditLog().writeAuditSystem(sUserID, "LOGIN", "LOGIN", "", userid + "|" + reason, csAuditLog.Category.Logon);
                    }
                }
                /* 06-01-2020 MBSS FIX */

                //bflag = false;
                //reason = "Failed Attempts exceeded allowed tries.";
                ///* VAPT 20180222 */
                //new csAuditLog().writeVAPTLogger(userid, "LOGIN", "USER HAS EXCEEDED THE NUMBER OF ALLOWABLE TRIES", "", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Alert);
                //setDisabled(userid, true);
                ///* VAPT IAG 20170515 */
                //new csAuditLog().writeAuditSystem(userid, "LOGIN", "LOGIN", "", userid + "|" + reason, csAuditLog.Category.Logon);
            }

            return new Object[] { bflag, reason, userid, tries, iflag };
        }

        /* VAPT IAG 20170515 */
        private void register_clientdata_login(String userid, bool bflag, short iflag)
        {
            ClientData._userid = userid;
            if (bflag)
            {
                ClientData._cifid = (String)ClientData.UserDBDatatable.Rows[0]["CIFID"];
                ClientData._cifclass = (short)ClientData.UserDBDatatable.Rows[0]["Mode"];
                ClientData._userid = (String)ClientData.UserDBDatatable.Rows[0]["UserId"];
                ClientData._location = (String)ClientData.UserDBDatatable.Rows[0]["Location"];
                ClientData._role = (short)ClientData.UserDBDatatable.Rows[0]["Role"];
                ClientData._flag = (short)ClientData.UserDBDatatable.Rows[0]["Flags"];
                ClientData._xdays = (short)ClientData.UserDBDatatable.Rows[0]["DaysExpired"];

                try
                {
                    if (ClientData.UserDBDatatable.Rows[0]["LoginGroup"] != null && ClientData.UserDBDatatable.Rows[0]["LoginGroup"] != DBNull.Value)
                    {
                        ClientData._login_group = (short)ClientData.UserDBDatatable.Rows[0]["LoginGroup"];
                    }
                    else
                    {
                        //ClientData._login_group = getLoginGroup(ClientData.Location);
                        ClientData._login_group = ClientData.UserLoginGroup;
                    }
                }
                catch (Exception e)
                {
                    Message = e.Message;
                    ClientData._login_group = (short)irsClientData.LoginGroups.US;
                }

                try
                {
                    int ndays = Convert.ToInt32(ClientData._xdays);
                    DateTime dt = Convert.ToDateTime(ClientData.UserDBDatatable.Rows[0]["LastPasswordChange"]).AddDays(ndays);

                    ClientData._xdays = (short)((TimeSpan)dt.Subtract(DateTime.Today)).Days;
                }
                catch
                {
                    ClientData._xdays = 90;
                }

                ClientData._no_logout = false;
                flag_wait();
                if (iflag > FLAG_INVALID & iflag < FLAG_DISABLED)
                {
                    setLogon(true);
                    setLastAccess(userid);
                    System.Web.HttpContext.Current.Session["USER_ON_LOGIN"] = null;
                }
                ClientData._tries = 0;
                return;
            }
            else
            {
                ClientData._flag = iflag;
            }
            ClientData._tries++;
        }

        /* VAPT IAG 20170530 */
        public DataTable getRolePermissionView(short role)
        {
            DataSet ds = getDataSet("sp_getRolePermissionsView", true, new Object[] { role });
            if (ds != null && ds.Tables.Count > 0) return ds.Tables[0];
            return null;
        }

        public DataTable getUserAccessPermission(short role, string module)
        {
            DataSet ds = getDataSet("proc_Get_UserAccessPermission", true, new Object[] { role, module });
            if (ds != null && ds.Tables.Count > 0) return ds.Tables[0];
            return null;
        }

        /* VAPT 20190206 */
        public DataTable getUserInfo(string userID)
        {
            DataSet ds = getDataSet("it_getUserInfo", true, new Object[] { userID });
            if (ds != null && ds.Tables.Count > 0) return ds.Tables[0];
            return null;
        }

        /* RFC-31620 */
        public string CleanDataTable(DataTable dt, string fieldName)
        {
            string ret = string.Empty;
            try
            {
                if (dt.Rows[0][fieldName] != DBNull.Value)
                {
                    ret = dt.Rows[0][fieldName].ToString();
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "CleanDataTable", fieldName, ex.Message, ex.StackTrace, csAuditLog.Category.Select);
            }
            return ret;
        }

        /* RFC-31620 */
        public bool adminApproverSwitch()
        {
            bool allow = base.IsWinApp;
            string ret = string.Empty;
            base.IsWinApp = true;
            base.EnforceConstraint = false;

            try
            {
                if (ClientData != null)
                {
                    if (ClientData.SuperAdmin == true)
                    {
                        return false;
                    }
                }

                if (System.Web.HttpContext.Current.Session["AdminApproverSwitch"] != null)
                {
                    if ((string)System.Web.HttpContext.Current.Session["AdminApproverSwitch"] == "1")
                    {
                        base.IsWinApp = allow;
                        return true;
                    }
                    else
                    {
                        base.IsWinApp = allow;
                        return false;
                    }
                }
                else
                {
                    bool isAdmin = ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).isSystemAdmin;
                    string adminApproverFlag = string.Empty;

                    if (isAdmin)
                    {
                        adminApproverFlag = new irsInternetPage().getLookupValue("AdminApproverFlag", "FldCode", "1", "FldCode");
                    }
                    else
                    {
                        adminApproverFlag = new irsInternetPage().getLookupValue("ParamApproverFlag", "FldCode", "1", "FldCode");
                    }

                    if (adminApproverFlag != string.Empty)
                    {
                        System.Web.HttpContext.Current.Session["AdminApproverSwitch"] = adminApproverFlag;
                    }
                    if (adminApproverFlag == "1")
                    {
                        base.IsWinApp = allow;
                        return true;
                    }
                    else
                    {
                        base.IsWinApp = allow;
                        return false;
                    }
                }

            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "Admin Approver Switch", "ERROR", ex.Message, ex.StackTrace, csAuditLog.Category.Select);
                return false;
            }
        }

        /* RFC-31620 */
        public void SendEmail(string subject, List<string> recipients, List<string> cc, List<string> bcc, string body, string[] attachments)
        {
            string receiver = string.Empty;
            try
            {
                SmtpClient email = new SmtpClient()
                {
                    Host = System.Configuration.ConfigurationManager.AppSettings["SMTPHost"],
                    Port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SMTPPort"]),
                    Timeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SMTP:Timeout"])
                };

                MailAddress from = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["SMTPFrom"]);
                MailMessage mail = new MailMessage()
                {
                    From = from,
                    Subject = subject,
                    SubjectEncoding = Encoding.UTF8,
                    Body = body,
                    BodyEncoding = Encoding.UTF8,
                    IsBodyHtml = true,
                    Priority = MailPriority.High
                };

                foreach (var mailto in recipients)
                {
                    if (mailto != null)
                    {
                        receiver = mailto;
                        mail.To.Add(mailto);
                    }
                }
                if (cc != null)
                {
                    foreach (var mailcc in cc)
                    {
                        if (mailcc != null && mailcc != string.Empty)
                        {
                            mail.CC.Add(mailcc);
                        }
                    }
                }
                if (bcc != null)
                {
                    foreach (var mailbcc in bcc)
                    {
                        if (mailbcc != null && mailbcc != string.Empty)
                        {
                            mail.Bcc.Add(mailbcc);
                        }
                    }
                }

                if (attachments != null)
                {
                    foreach (var mailattach in attachments)
                    {
                        try
                        {
                            Attachment attach = new Attachment(mailattach);
                            mail.Attachments.Add(attach);
                        }
                        catch (Exception ex)
                        {
                            new csAuditLog().writeAudit("", "Send Email", "Maker/Checker Admin Error", ex.Message, ex.StackTrace, csAuditLog.Category.Email);
                        }
                    }
                }
                email.Send(mail);
                new csAuditLog().writeAudit("", "Send Email", "Maker/Checker Admin Successful", receiver + " with subject (" + subject + ")", "", csAuditLog.Category.Email);
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "Send Email Error", "Maker/Checker Admin", ex.Message, ex.StackTrace, csAuditLog.Category.Email);
            }
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

        /* RFC-31620 */
        public enum ActionType
        {
            ADD,
            EDIT,
            DELETE
        }

        /* RFC-31620 */
        public DataTable getUserData(string userId)
        {
            base.EnforceConstraint = false;
            getUpdateDS("sp_getUser", new Object[] { "USERID", userId });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return null;
            }
        }

        /* RFC-31620 */
        public bool IsMakerAllowedUser(ActionType actionType, string userId)
        {
            bool ret = false;
            try
            {
                base.EnforceConstraint = false;
                getUpdateDS("mc_UserAdminCheckMakerAllowed", new Object[] { userId, actionType.ToString() });
                if (hasRows)
                {
                    selectedRow = 0;
                    if (Convert.ToInt32(table.Rows[0]["Strict"].ToString()) > 0)
                    {
                        ret = true;
                    }
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "IsMakerAllowedUser Error", "Validate Strict Maker", ex.Message, ex.StackTrace, csAuditLog.Category.Email);
            }
            return ret;
        }

        /* RFC-31620 */
        public bool saveUserMakerChecker(ActionType actionType, string userId, DataTable dtAddEdit, string cifId)
        {
            try
            {
                bool ok = false;
                DataTable dt = new DataTable();
                string sessionUserId = ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId;
                string generatePass = string.Empty;

                if (actionType == ActionType.ADD || actionType == ActionType.EDIT)
                {
                    dt = dtAddEdit;
                    generatePass = irsUsers.getTriDes().Encrypt(getDefaultPassword(getUserName(cifId), userId));
                }
                else
                {
                    dt = getUserData(userId);
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    base.addMode = true;
                    this.setBase("SELECT TOP 1 * FROM TBL_USERS_TMP", true);
                    setColumn("CIFID", dt.Rows[0]["CIFID"]);
                    setColumn("UserID", dt.Rows[0]["UserID"]);
                    setColumn("Password", generatePass);
                    setColumn("Location", dt.Rows[0]["Location"]);
                    setColumn("DateCreated", dt.Rows[0]["DateCreated"]);
                    setColumn("LastModified", dt.Rows[0]["LastModified"]);
                    setColumn("LastAccessDate", dt.Rows[0]["LastAccessDate"]);
                    setColumn("LastPasswordChange", dt.Rows[0]["LastPasswordChange"]);
                    setColumn("DaysExpired", dt.Rows[0]["DaysExpired"]);
                    setColumn("Role", dt.Rows[0]["Role"]);
                    setColumn("Flags", dt.Rows[0]["Flags"]);
                    setColumn("Mode", dt.Rows[0]["Mode"]);
                    setColumn("LoggedOn", dt.Rows[0]["LoggedOn"]);
                    setColumn("LoginGroup", dt.Rows[0]["LoginGroup"]);
                    setColumn("CurrentSessionID", dt.Rows[0]["CurrentSessionID"]);
                    setColumn("DateAdded", DateTime.Now);
                    setColumn("CurrStatus", "For Approval");
                    setColumn("Maker", sessionUserId);
                    setColumn("Checker", "");
                    setColumn("StatusDate", DateTime.Now);
                    setColumn("ActionType", actionType.ToString());
                    setColumn("Module", "System Administration >> User Administration");
                    setColumn("MenuName", "User Table");
                    setColumn("Remarks", "");

                    ok = base.save(sqlHelperWs.RETAIN_DATASET);
                    if (ok)
                    {
                        try
                        {
                            cifId = dt.Rows[0]["CIFID"].ToString();
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
                            body = new irsInternetPage().getLookupValue("AdminApproverEmail", "FldCode", "UserAdminBody", "FldDescription");

                            body = body.Replace("@ModuleName", "System Administration >> User Administration");
                            body = body.Replace("@MakerID", sessionUserId);
                            body = body.Replace("@ActionType", actionType.ToString());
                            body = body.Replace("@UserID", userId);
                            body = body.Replace("@Name", getUserName(cifId));
                            body = body.Replace("@DateRequested", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                            SendEmail(subject, recipient, cc, bcc, body, null);
                        }
                        catch (Exception ex)
                        {
                            new csAuditLog().writeAudit("", "saveUserMakerChecker Email Sending Failed", "Maker Checker User Admin", ex.Message, ex.StackTrace, csAuditLog.Category.Email);
                        }
                        return true;
                    }
                    else
                    {
                        new csAuditLog().writeAudit("", "saveUserMakerChecker Error", "Maker Checker User Admin", userId, base.Message, csAuditLog.Category.Insert);
                        return false;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "saveUserMakerChecker Error", "Maker Checker User Admin", userId, ex.Message, csAuditLog.Category.Insert);
                return false;
            }
        }

        /* RFC-31620 */
        public DataTable GetCIFInformation(string seqId)
        {
            DataTable ret = new DataTable();
            try
            {
                base.EnforceConstraint = false;
                getUpdateDS("sp_cif_get_cifmaster", new Object[] { seqId });
                if (hasRows)
                {
                    selectedRow = 0;
                    ret = table;
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "GetCIFInformation Error", "Maker/Checker Admin " + seqId.ToString(), ex.Message, ex.StackTrace, csAuditLog.Category.Select);
            }
            return ret;
        }

        /* RFC-31620 */
        public bool IsMakerAllowedCIFMaster(ActionType actionType, string cifId)
        {
            bool ret = false;
            bool isAdmin = ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).isSystemAdmin;
            try
            {
                base.EnforceConstraint = false;
                if (((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).isSystemAdmin)
                getUpdateDS("mc_CIFMasterAdminCheckMakerAllowed", new Object[] { cifId, actionType.ToString(), isAdmin });
                if (hasRows)
                {
                    selectedRow = 0;
                    if (Convert.ToInt32(table.Rows[0]["Strict"].ToString()) > 0)
                    {
                        ret = true;
                    }
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "IsMakerAllowedCIFMaster Error", "Validate Strict Maker", ex.Message, ex.StackTrace, csAuditLog.Category.Email);
            }
            return ret;
        }

        /* RFC-31620 */
        public bool saveMasterMakerChecker(ActionType actionType, DataTable dt, string cifId)
        {
            try
            {
                bool ok = false;
                string sessionUserId = ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId;
                string branchCode = string.Empty;
                string remarks = string.Empty;
                string fullName = string.Empty;
                bool isAdmin = ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).isSystemAdmin;

                if (dt != null && dt.Rows.Count > 0)
                {
                    fullName = CleanDataTable(dt, "FirstName") + " " + CleanDataTable(dt, "MiddleName") + " " + CleanDataTable(dt, "LastName");
                    branchCode = CleanDataTable(dt, "BranchCode");
                    remarks = CleanDataTable(dt, "Remarks");

                    base.addMode = true;
                    this.setBase("SELECT TOP 1 * FROM TBL_CIF_MASTER_TMP", true);
                    setColumn("Main_SEQ_ID", (dt.Rows[0]["Main_SEQ_ID"] != DBNull.Value ? CleanDataTable(dt, "Main_SEQ_ID") : null));
                    setColumn("CIFID", (dt.Rows[0]["CIFID"] != DBNull.Value ? CleanDataTable(dt, "CIFID") : null));
                    setColumn("CIFType", (dt.Rows[0]["CIFType"] != DBNull.Value ? CleanDataTable(dt, "CIFType") : null));
                    setColumn("LastName", (dt.Rows[0]["LastName"] != DBNull.Value ? CleanDataTable(dt, "LastName") : null));
                    setColumn("FirstName", (dt.Rows[0]["FirstName"] != DBNull.Value ? CleanDataTable(dt, "FirstName") : null));
                    setColumn("MiddleName", (dt.Rows[0]["MiddleName"] != DBNull.Value ? CleanDataTable(dt, "MiddleName") : null));
                    setColumn("Gender", (dt.Rows[0]["Gender"] != DBNull.Value ? CleanDataTable(dt, "Gender") : null));
                    setColumn("Age", (dt.Rows[0]["Age"] != DBNull.Value ? CleanDataTable(dt, "Age") : null));
                    setColumn("CivilStatus", (dt.Rows[0]["CivilStatus"] != DBNull.Value ? CleanDataTable(dt, "CivilStatus") : null));
                    setColumn("Citizenship", (dt.Rows[0]["Citizenship"] != DBNull.Value ? CleanDataTable(dt, "Citizenship") : null));
                    setColumn("BirthDate", (dt.Rows[0]["BirthDate"] != DBNull.Value ? CleanDataTable(dt, "BirthDate") : null));
                    setColumn("BirthPlace", (dt.Rows[0]["BirthPlace"] != DBNull.Value ? CleanDataTable(dt, "BirthPlace") : null));
                    setColumn("Remarks", (dt.Rows[0]["Remarks"] != DBNull.Value ? CleanDataTable(dt, "Remarks") : null));
                    setColumn("CreatedBy", (dt.Rows[0]["CreatedBy"] != DBNull.Value ? CleanDataTable(dt, "CreatedBy") : null));
                    setColumn("DateCreated", (dt.Rows[0]["DateCreated"] != DBNull.Value ? CleanDataTable(dt, "DateCreated") : null));
                    setColumn("ModifiedBy", (dt.Rows[0]["ModifiedBy"] != DBNull.Value ? CleanDataTable(dt, "ModifiedBy") : null));
                    setColumn("DateModified", (dt.Rows[0]["DateModified"] != DBNull.Value ? CleanDataTable(dt, "DateModified") : null));
                    setColumn("Deleted", (dt.Rows[0]["Deleted"] != DBNull.Value ? CleanDataTable(dt, "Deleted") : null));
                    setColumn("Title", (dt.Rows[0]["Title"] != DBNull.Value ? CleanDataTable(dt, "Title") : null));
                    setColumn("Alias", (dt.Rows[0]["Alias"] != DBNull.Value ? CleanDataTable(dt, "Alias") : null));
                    setColumn("BranchCode", branchCode);
                    setColumn("Religion", (dt.Rows[0]["Religion"] != DBNull.Value ? CleanDataTable(dt, "Religion") : null));
                    setColumn("FullName", fullName);
                    setColumn("CIFClass", (dt.Rows[0]["CIFClass"] != DBNull.Value ? CleanDataTable(dt, "CIFClass") : null));
                    setColumn("Maker", sessionUserId);
                    setColumn("Checker", "");
                    setColumn("DateAdded", DateTime.Now);
                    setColumn("CurrStatus", "For Approval");
                    setColumn("StatusDate", DateTime.Now);
                    setColumn("ActionType", actionType.ToString());
                    setColumn("Module", "Maintenance >> CIF Maintenance");
                    setColumn("MenuName", "Personal Info Table");
                    setColumn("IsAdmin", isAdmin);

                    ok = base.save(sqlHelperWs.RETAIN_DATASET);
                    if (ok)
                    {
                        try
                        {
                            string receiver = string.Empty;
                            string receivercc = string.Empty;
                            string receiverbcc = string.Empty;
                            string subject = string.Empty;
                            string body = string.Empty;

                            cifId = dt.Rows[0]["CIFID"].ToString();
                            if (isAdmin)
                            {
                                receiver = new irsInternetPage().getLookupValue("AdminApproverEmail", "FldCode", "AdminRecipient", "FldDescription");
                                receivercc = new irsInternetPage().getLookupValue("AdminApproverEmail", "FldCode", "AdminRecipientCC", "FldDescription");
                                receiverbcc = new irsInternetPage().getLookupValue("AdminApproverEmail", "FldCode", "AdminRecipientBCC", "FldDescription");
                                subject = new irsInternetPage().getLookupValue("AdminApproverEmail", "FldCode", "AdminSubject", "FldDescription").Replace("@Region", System.Configuration.ConfigurationManager.AppSettings["SMTPRegion"]);
                                body = new irsInternetPage().getLookupValue("AdminApproverEmail", "FldCode", "CIFAdminBody", "FldDescription");
                            }
                            else
                            {
                                receiver = new irsInternetPage().getLookupValue("ParamApproverEmail", "FldCode", "ParamRecipient", "FldDescription");
                                receivercc = new irsInternetPage().getLookupValue("ParamApproverEmail", "FldCode", "ParamRecipientCC", "FldDescription");
                                receiverbcc = new irsInternetPage().getLookupValue("ParamApproverEmail", "FldCode", "ParamRecipientBCC", "FldDescription");
                                subject = new irsInternetPage().getLookupValue("ParamApproverEmail", "FldCode", "ParamSubject", "FldDescription").Replace("@Region", System.Configuration.ConfigurationManager.AppSettings["SMTPRegion"]);
                                body = new irsInternetPage().getLookupValue("ParamApproverEmail", "FldCode", "CIFParamBody", "FldDescription");
                            }

                            string[] arrReceiver = receiver.Split(';');
                            string[] arrReceiverCC = receivercc.Split(';');
                            string[] arrReceiverBCC = receiverbcc.Split(';');
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

                            body = body.Replace("@ModuleName", "CIF Maintenance >> Personal Info");
                            body = body.Replace("@MakerID", sessionUserId);
                            body = body.Replace("@ActionType", actionType.ToString());
                            body = body.Replace("@CIFID", (cifId == string.Empty) ? "N/A" : cifId);
                            body = body.Replace("@Name", fullName);
                            body = body.Replace("@BranchCode", branchCode);
                            body = body.Replace("@Remarks", remarks);
                            body = body.Replace("@DateRequested", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                            SendEmail(subject, recipient, cc, bcc, body, null);
                        }
                        catch (Exception ex)
                        {
                            new csAuditLog().writeAudit("", "saveMasterMakerChecker Email Sending Failed", "Maker Checker CIF Personal Info", ex.Message, ex.StackTrace, csAuditLog.Category.Email);
                        }
                        return true;
                    }
                    else
                    {
                        new csAuditLog().writeAudit("", "saveMasterMakerChecker Error", "Maker Checker CIF Personal Info", "", base.Message, csAuditLog.Category.Insert);
                        return false;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "saveMasterMakerChecker Error", "Maker Checker CIF Personal Info", "", ex.Message, csAuditLog.Category.Insert);
                return false;
            }
        }

        /* RFC-31620 */
        public DataTable GetCIFContacts(string seqId)
        {
            DataTable ret = new DataTable();
            try
            {
                base.EnforceConstraint = false;
                getUpdateDS("sp_cif_get_cifcontacts", new Object[] { seqId });
                if (hasRows)
                {
                    selectedRow = 0;
                    ret = table;
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "GetCIFContacts Error", "Maker/Checker Admin " + seqId.ToString(), ex.Message, ex.StackTrace, csAuditLog.Category.Select);
            }
            return ret;
        }

        /* RFC-31620 */
        public bool IsMakerAllowedCIFContacts(ActionType actionType, string seqId)
        {
            bool ret = false;
            bool isAdmin = ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).isSystemAdmin;
            try
            {
                base.EnforceConstraint = false;
                getUpdateDS("mc_CIFContactsAdminCheckMakerAllowed", new Object[] { seqId, actionType.ToString(), isAdmin });
                if (hasRows)
                {
                    selectedRow = 0;
                    if (Convert.ToInt32(table.Rows[0]["Strict"].ToString()) > 0)
                    {
                        ret = true;
                    }
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "IsMakerAllowedCIFContacts Error", "Validate Strict Maker", ex.Message, ex.StackTrace, csAuditLog.Category.Email);
            }
            return ret;
        }

        /* RFC-31620 */
        public bool saveContactsMakerChecker(ActionType actionType, DataTable dt, string cifId)
        {
            try
            {
                bool ok = false;
                string sessionUserId = ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId;
                string contactType = string.Empty;
                string contactNo = string.Empty;
                string emailAddress = string.Empty;
                string remarks = string.Empty;
                string fullName = string.Empty;
                bool isAdmin = ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).isSystemAdmin;

                if (dt != null && dt.Rows.Count > 0)
                {
                    contactType = new irsInternetPage().getLookupValue("ContactInfoType", "FldValue", dt.Rows[0]["ContactType"].ToString(), "FldDescription");
                    contactNo = CleanDataTable(dt, "ContactNo");
                    emailAddress = CleanDataTable(dt, "EmailAddress");
                    cifId = CleanDataTable(dt, "CIFID");
                    fullName = getUserName(cifId);
                    remarks = CleanDataTable(dt, "Remarks");

                    base.addMode = true;
                    this.setBase("SELECT TOP 1 * FROM TBL_CIF_CONTACTS_TMP", true);
                    setColumn("Main_SEQ_ID", CleanDataTable(dt, "Main_SEQ_ID"));
                    setColumn("CIFID", CleanDataTable(dt, "CIFID"));
                    setColumn("ContactType", CleanDataTable(dt, "ContactType"));
                    setColumn("ContactNo", CleanDataTable(dt, "ContactNo"));
                    setColumn("Remarks", CleanDataTable(dt, "Remarks"));
                    setColumn("EmailAddress", CleanDataTable(dt, "EmailAddress"));
                    setColumn("PreferredNo", CleanDataTable(dt, "PreferredNo"));
                    setColumn("Deleted", CleanDataTable(dt, "Deleted"));
                    setColumn("CIFClass", CleanDataTable(dt, "CIFClass"));
                    setColumn("CountryCode", CleanDataTable(dt, "CountryCode"));
                    setColumn("Maker", sessionUserId);
                    setColumn("Checker", "");
                    setColumn("DateAdded", DateTime.Now);
                    setColumn("CurrStatus", "For Approval");
                    setColumn("StatusDate", DateTime.Now);
                    setColumn("ActionType", actionType.ToString());
                    setColumn("Module", "Maintenance >> CIF Maintenance");
                    setColumn("MenuName", "Contacts Table");
                    setColumn("IsAdmin", isAdmin);

                    ok = base.save(sqlHelperWs.RETAIN_DATASET);
                    if (ok)
                    {
                        try
                        {
                            string receiver = string.Empty;
                            string receivercc = string.Empty;
                            string receiverbcc = string.Empty;
                            string subject = string.Empty;
                            string body = string.Empty;

                            cifId = dt.Rows[0]["CIFID"].ToString();
                            if (isAdmin)
                            {
                                receiver = new irsInternetPage().getLookupValue("AdminApproverEmail", "FldCode", "AdminRecipient", "FldDescription");
                                receivercc = new irsInternetPage().getLookupValue("AdminApproverEmail", "FldCode", "AdminRecipientCC", "FldDescription");
                                receiverbcc = new irsInternetPage().getLookupValue("AdminApproverEmail", "FldCode", "AdminRecipientBCC", "FldDescription");
                                subject = new irsInternetPage().getLookupValue("AdminApproverEmail", "FldCode", "AdminSubject", "FldDescription").Replace("@Region", System.Configuration.ConfigurationManager.AppSettings["SMTPRegion"]);
                                body = new irsInternetPage().getLookupValue("AdminApproverEmail", "FldCode", "ContactsAdminBody", "FldDescription");
                            }
                            else
                            {
                                receiver = new irsInternetPage().getLookupValue("ParamApproverEmail", "FldCode", "ParamRecipient", "FldDescription");
                                receivercc = new irsInternetPage().getLookupValue("ParamApproverEmail", "FldCode", "ParamRecipientCC", "FldDescription");
                                receiverbcc = new irsInternetPage().getLookupValue("ParamApproverEmail", "FldCode", "ParamRecipientBCC", "FldDescription");
                                subject = new irsInternetPage().getLookupValue("ParamApproverEmail", "FldCode", "ParamSubject", "FldDescription").Replace("@Region", System.Configuration.ConfigurationManager.AppSettings["SMTPRegion"]);
                                body = new irsInternetPage().getLookupValue("ParamApproverEmail", "FldCode", "ContactsAdminBody", "FldDescription");
                            }

                            string[] arrReceiver = receiver.Split(';');
                            string[] arrReceiverCC = receivercc.Split(';');
                            string[] arrReceiverBCC = receiverbcc.Split(';');
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

                            body = body.Replace("@ModuleName", "CIF Maintenance >> Contacts");
                            body = body.Replace("@MakerID", sessionUserId);
                            body = body.Replace("@ActionType", actionType.ToString());
                            body = body.Replace("@CIFID", (cifId == string.Empty) ? "N/A" : cifId);
                            body = body.Replace("@Name", fullName);
                            body = body.Replace("@ContactType", contactType);
                            body = body.Replace("@ContactNo", contactNo);
                            body = body.Replace("@EmailAddress", emailAddress);
                            body = body.Replace("@DateRequested", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                            SendEmail(subject, recipient, cc, bcc, body, null);
                        }
                        catch (Exception ex)
                        {
                            new csAuditLog().writeAudit("", "saveContactsMakerChecker Email Sending Failed", "Maker Checker CIF Contacts", ex.Message, ex.StackTrace, csAuditLog.Category.Email);
                        }
                        return true;
                    }
                    else
                    {
                        new csAuditLog().writeAudit("", "saveContactsMakerChecker Error", "Maker Checker CIF Contacts", "", base.Message, csAuditLog.Category.Insert);
                        return false;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "saveContactsMakerChecker Error", "Maker Checker CIF Contacts", "", ex.Message, csAuditLog.Category.Insert);
                return false;
            }
        }

        /* RFC-31620 */
        public DataTable GetCIFAccounts(string seqId)
        {
            DataTable ret = new DataTable();
            try
            {
                base.EnforceConstraint = false;
                getUpdateDS("mc_getCIFAccount", new Object[] { seqId });
                if (hasRows)
                {
                    selectedRow = 0;
                    ret = table;
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "GetCIFAccounts Error", "Maker/Checker Admin " + seqId.ToString(), ex.Message, ex.StackTrace, csAuditLog.Category.Select);
            }
            return ret;
        }

        /* RFC-31620 */
        public bool IsMakerAllowedCIFAccounts(ActionType actionType, string cifId)
        {
            bool ret = false;
            bool isAdmin = ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).isSystemAdmin;
            try
            {
                base.EnforceConstraint = false;
                if (((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).isSystemAdmin)
                getUpdateDS("mc_CIFAccountsAdminCheckMakerAllowed", new Object[] { cifId, actionType.ToString(), isAdmin });
                if (hasRows)
                {
                    selectedRow = 0;
                    if (Convert.ToInt32(table.Rows[0]["Strict"].ToString()) > 0)
                    {
                        ret = true;
                    }
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "IsMakerAllowedCIFAccounts Error", "Validate Strict Maker", ex.Message, ex.StackTrace, csAuditLog.Category.Email);
            }
            return ret;
        }

        /* RFC-31620 */
        public bool saveAccountsMakerChecker(ActionType actionType, DataTable dt, string cifId)
        {
            try
            {
                bool ok = false;
                string sessionUserId = ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId;
                string remarks = string.Empty;
                string fullName = string.Empty;
                string accountNo = string.Empty;
                string accountType = string.Empty;
                string pnbBranch = string.Empty;
                string currency = string.Empty;
                bool isAdmin = ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).isSystemAdmin;

                if (dt != null && dt.Rows.Count > 0)
                {
                    accountNo = CleanDataTable(dt, "AccountNo");
                    accountType = new irsInternetPage().getLookupValue("AccountType", "FldValue", dt.Rows[0]["AccountType"].ToString(), "FldDescription");
                    pnbBranch = CleanDataTable(dt, "BranchCode");
                    currency = new irsInternetPage().getLookupValue("Currency", "FldValue", dt.Rows[0]["Currency"].ToString(), "FldCode");
                    cifId = CleanDataTable(dt, "CIFID");
                    fullName = getUserName(cifId);
                    remarks = CleanDataTable(dt, "Remarks");

                    base.addMode = true;
                    this.setBase("SELECT TOP 1 * FROM TBL_CIF_ACCOUNTS_TMP", true);
                    setColumn("Main_SEQ_ID", CleanDataTable(dt, "Main_SEQ_ID"));
                    setColumn("CIFID", CleanDataTable(dt, "CIFID"));
                    setColumn("AccountNo", CleanDataTable(dt, "AccountNo"));
                    setColumn("AccountType", CleanDataTable(dt, "AccountType"));
                    setColumn("Currency", CleanDataTable(dt, "Currency"));
                    setColumn("BranchCode", CleanDataTable(dt, "BranchCode"));
                    setColumn("DomBankCode", CleanDataTable(dt, "DomBankCode"));
                    setColumn("Remarks", CleanDataTable(dt, "Remarks"));
                    setColumn("Status", CleanDataTable(dt, "Status"));
                    setColumn("ForBankCode", CleanDataTable(dt, "ForBankCode"));
                    setColumn("Deleted", CleanDataTable(dt, "Deleted"));
                    setColumn("IsGFC", CleanDataTable(dt, "IsGFC"));
                    setColumn("KBIAccount", CleanDataTable(dt, "KBIAccount"));
                    setColumn("OtherBankSeqId", CleanDataTable(dt, "OtherBankSeqId"));
                    setColumn("BranchType", CleanDataTable(dt, "BranchType"));
                    setColumn("BranchName", CleanDataTable(dt, "BranchName"));
                    setColumn("AccountRemarks", CleanDataTable(dt, "AccountRemarks"));
                    setColumn("AccountRemarksOthers", CleanDataTable(dt, "AccountRemarksOthers"));
                    setColumn("IsMigrated", CleanDataTable(dt, "IsMigrated"));
                    setColumn("Maker", sessionUserId);
                    setColumn("Checker", "");
                    setColumn("DateAdded", DateTime.Now);
                    setColumn("CurrStatus", "For Approval");
                    setColumn("StatusDate", DateTime.Now);
                    setColumn("ActionType", actionType.ToString());
                    setColumn("Module", "Maintenance >> CIF Maintenance");
                    setColumn("MenuName", "Accounts Table");
                    setColumn("IsAdmin", isAdmin);

                    ok = base.save(sqlHelperWs.RETAIN_DATASET);
                    if (ok)
                    {
                        try
                        {
                            string receiver = string.Empty;
                            string receivercc = string.Empty;
                            string receiverbcc = string.Empty;
                            string subject = string.Empty;
                            string body = string.Empty;

                            cifId = dt.Rows[0]["CIFID"].ToString();
                            if (isAdmin)
                            {
                                receiver = new irsInternetPage().getLookupValue("AdminApproverEmail", "FldCode", "AdminRecipient", "FldDescription");
                                receivercc = new irsInternetPage().getLookupValue("AdminApproverEmail", "FldCode", "AdminRecipientCC", "FldDescription");
                                receiverbcc = new irsInternetPage().getLookupValue("AdminApproverEmail", "FldCode", "AdminRecipientBCC", "FldDescription");
                                subject = new irsInternetPage().getLookupValue("AdminApproverEmail", "FldCode", "AdminSubject", "FldDescription").Replace("@Region", System.Configuration.ConfigurationManager.AppSettings["SMTPRegion"]);
                                body = new irsInternetPage().getLookupValue("AdminApproverEmail", "FldCode", "AccountsParamBody", "FldDescription");
                            }
                            else
                            {
                                receiver = new irsInternetPage().getLookupValue("ParamApproverEmail", "FldCode", "ParamRecipient", "FldDescription");
                                receivercc = new irsInternetPage().getLookupValue("ParamApproverEmail", "FldCode", "ParamRecipientCC", "FldDescription");
                                receiverbcc = new irsInternetPage().getLookupValue("ParamApproverEmail", "FldCode", "ParamRecipientBCC", "FldDescription");
                                subject = new irsInternetPage().getLookupValue("ParamApproverEmail", "FldCode", "ParamSubject", "FldDescription").Replace("@Region", System.Configuration.ConfigurationManager.AppSettings["SMTPRegion"]);
                                body = new irsInternetPage().getLookupValue("ParamApproverEmail", "FldCode", "AccountsParamBody", "FldDescription");
                            }

                            string[] arrReceiver = receiver.Split(';');
                            string[] arrReceiverCC = receivercc.Split(';');
                            string[] arrReceiverBCC = receiverbcc.Split(';');
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

                            body = body.Replace("@ModuleName", "CIF Maintenance >> Accounts");
                            body = body.Replace("@MakerID", sessionUserId);
                            body = body.Replace("@ActionType", actionType.ToString());
                            body = body.Replace("@CIFID", (cifId == string.Empty) ? "N/A" : cifId);
                            body = body.Replace("@Name", fullName);
                            body = body.Replace("@AccountNo", accountNo);
                            body = body.Replace("@AccountType", accountType);
                            body = body.Replace("@PNBBranch", pnbBranch);
                            body = body.Replace("@Currency", currency);
                            body = body.Replace("@Remarks", remarks);
                            body = body.Replace("@DateRequested", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                            SendEmail(subject, recipient, cc, bcc, body, null);
                        }
                        catch (Exception ex)
                        {
                            new csAuditLog().writeAudit("", "saveAccountsMakerChecker Email Sending Failed", "Maker Checker CIF Accounts", ex.Message, ex.StackTrace, csAuditLog.Category.Email);
                        }
                        return true;
                    }
                    else
                    {
                        new csAuditLog().writeAudit("", "saveAccountsMakerChecker Error", "Maker Checker CIF Accounts", "", base.Message, csAuditLog.Category.Insert);
                        return false;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "saveAccountsMakerChecker Error", "Maker Checker CIF Accounts", "", ex.Message, csAuditLog.Category.Insert);
                return false;
            }
        }

        /* RFC-31620 */
        public DataTable GetCIFMerchantsInfo(string seqId)
        {
            DataTable ret = new DataTable();
            try
            {
                base.EnforceConstraint = false;
                getUpdateDS("mc_getCIFMerchantsInfo", new Object[] { seqId });
                if (hasRows)
                {
                    selectedRow = 0;
                    ret = table;
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "GetCIFMerchantsInfo Error", "Maker/Checker Admin " + seqId.ToString(), ex.Message, ex.StackTrace, csAuditLog.Category.Select);
            }
            return ret;
        }

        /* RFC-31620 */
        public bool IsMakerAllowedCIFMerchantsInfo(ActionType actionType, string seqId)
        {
            bool ret = false;
            bool isAdmin = ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).isSystemAdmin;
            try
            {
                base.EnforceConstraint = false;
                getUpdateDS("mc_CIFMerchantsInfoAdminCheckMakerAllowed", new Object[] { seqId, actionType.ToString(), isAdmin });
                if (hasRows)
                {
                    selectedRow = 0;
                    if (Convert.ToInt32(table.Rows[0]["Strict"].ToString()) > 0)
                    {
                        ret = true;
                    }
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "IsMakerAllowedCIFMerchantsInfo Error", "Validate Strict Maker", ex.Message, ex.StackTrace, csAuditLog.Category.Email);
            }
            return ret;
        }

        /* RFC-31620 */
        public bool saveMerchantsInfoMakerChecker(ActionType actionType, DataTable dt, string cifId)
        {
            try
            {
                bool ok = false;
                string sessionUserId = ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId;
                string fullName = string.Empty;
                string merchantType = string.Empty;
                string merchantProdCode = string.Empty;
                string accountNo = string.Empty;
                bool isAdmin = ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).isSystemAdmin;
                DataTable dtCIFAccounts = new DataTable();

                if (dt != null && dt.Rows.Count > 0)
                {
                    merchantType = new irsInternetPage().getLookupValue("MerchantType", "FldValue", dt.Rows[0]["MerchantType"].ToString(), "FldDescription");
                    merchantProdCode = new irsInternetPage().getLookupValue("MerchantProdCode", "FldValue", dt.Rows[0]["MerchantProductCode"].ToString(), "FldDescription");
                    dtCIFAccounts = GetCIFAccounts(CleanDataTable(dt, "AccountSeqId"));
                    if (dtCIFAccounts != null && dtCIFAccounts.Rows.Count > 0)
                    {
                        accountNo = dtCIFAccounts.Rows[0]["AccountNo"].ToString();
                    }
                    cifId = CleanDataTable(dt, "CIFID");
                    fullName = getUserName(cifId);

                    base.addMode = true;
                    this.setBase("SELECT TOP 1 * FROM TBL_CIF_MERCHANTS_INFO_TMP", true);
                    setColumn("Main_SEQ_ID", CleanDataTable(dt, "Main_SEQ_ID"));
                    setColumn("CIFID", CleanDataTable(dt, "CIFID"));
                    setColumn("ProductCode", CleanDataTable(dt, "ProductCode"));
                    setColumn("AccountSeqId", CleanDataTable(dt, "AccountSeqId"));
                    setColumn("MerchantCode", CleanDataTable(dt, "MerchantCode"));
                    setColumn("ImplementationCode", CleanDataTable(dt, "ImplementationCode"));
                    setColumn("MerchantProductCode", CleanDataTable(dt, "MerchantProductCode"));
                    setColumn("MerchantType", CleanDataTable(dt, "MerchantType"));
                    setColumn("Deleted", CleanDataTable(dt, "Deleted"));
                    setColumn("Maker", sessionUserId);
                    setColumn("Checker", "");
                    setColumn("DateAdded", DateTime.Now);
                    setColumn("CurrStatus", "For Approval");
                    setColumn("StatusDate", DateTime.Now);
                    setColumn("ActionType", actionType.ToString());
                    setColumn("Module", "Maintenance >> CIF Maintenance");
                    setColumn("MenuName", "Merchant Info Table");
                    setColumn("IsAdmin", isAdmin);

                    ok = base.save(sqlHelperWs.RETAIN_DATASET);
                    if (ok)
                    {
                        try
                        {
                            cifId = dt.Rows[0]["CIFID"].ToString();
                            string receiver = new irsInternetPage().getLookupValue("ParamApproverEmail", "FldCode", "ParamRecipient", "FldDescription");
                            string receivercc = new irsInternetPage().getLookupValue("ParamApproverEmail", "FldCode", "ParamRecipientCC", "FldDescription");
                            string receiverbcc = new irsInternetPage().getLookupValue("ParamApproverEmail", "FldCode", "ParamRecipientBCC", "FldDescription");
                            string subject = new irsInternetPage().getLookupValue("ParamApproverEmail", "FldCode", "ParamSubject", "FldDescription").Replace("@Region", System.Configuration.ConfigurationManager.AppSettings["SMTPRegion"]);
                            string body = new irsInternetPage().getLookupValue("ParamApproverEmail", "FldCode", "MerchantsInfoParamBody", "FldDescription");
                            string[] arrReceiver = receiver.Split(';');
                            string[] arrReceiverCC = receivercc.Split(';');
                            string[] arrReceiverBCC = receiverbcc.Split(';');
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

                            body = body.Replace("@ModuleName", "CIF Maintenance >> Merchant Info");
                            body = body.Replace("@MakerID", sessionUserId);
                            body = body.Replace("@ActionType", actionType.ToString());
                            body = body.Replace("@CIFID", (cifId == string.Empty) ? "N/A" : cifId);
                            body = body.Replace("@Name", fullName);
                            body = body.Replace("@MerchantType", merchantType);
                            body = body.Replace("@MerchantProdCode", merchantProdCode);
                            body = body.Replace("@AccountNo", accountNo);
                            body = body.Replace("@DateRequested", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                            SendEmail(subject, recipient, cc, bcc, body, null);
                        }
                        catch (Exception ex)
                        {
                            new csAuditLog().writeAudit("", "saveMerchantsInfoMakerChecker Email Sending Failed", "Maker Checker CIF Merchants Info", ex.Message, ex.StackTrace, csAuditLog.Category.Email);
                        }
                        return true;
                    }
                    else
                    {
                        new csAuditLog().writeAudit("", "saveMerchantsInfoMakerChecker Error", "Maker Checker CIF Merchants Info", "", base.Message, csAuditLog.Category.Insert);
                        return false;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "saveMerchantsInfoMakerChecker Error", "Maker Checker CIF Merchants Info", "", ex.Message, csAuditLog.Category.Insert);
                return false;
            }
        }

        /* RFC-31620 */
        public bool IsMakerAllowedLookup(ActionType actionType, string seqId)
        {
            bool ret = false;
            try
            {
                base.EnforceConstraint = false;
                getUpdateDS("mc_LookupAdminCheckMakerAllowed", new Object[] { seqId, actionType.ToString() });
                if (hasRows)
                {
                    selectedRow = 0;
                    if (Convert.ToInt32(table.Rows[0]["Strict"].ToString()) > 0)
                    {
                        ret = true;
                    }
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "IsMakerAllowedLookup Error", "Validate Strict Maker", ex.Message, ex.StackTrace, csAuditLog.Category.Email);
            }
            return ret;
        }

        /* RFC-31620 */
        public bool saveLookupMakerChecker(ActionType actionType, DataTable dt, DataTable dtLookup)
        {
            try
            {
                bool ok = false;
                string sessionUserId = ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId;

                if (dt != null && dt.Rows.Count > 0)
                {
                    base.addMode = true;
                    this.setBase("SELECT TOP 1 * FROM TBL_LOOKUP_TMP", true);
                    setColumn("Main_SEQ_ID", CleanDataTable(dt, "Main_SEQ_ID"));
                    setColumn("FldGroup", CleanDataTable(dt, "FldGroup"));
                    setColumn("FldCode", CleanDataTable(dt, "FldCode"));
                    setColumn("FldValue", CleanDataTable(dt, "FldValue"));
                    setColumn("FldDescription", CleanDataTable(dt, "FldDescription"));
                    setColumn("Maker", sessionUserId);
                    setColumn("Checker", "");
                    setColumn("DateAdded", DateTime.Now);
                    setColumn("CurrStatus", "For Approval");
                    setColumn("StatusDate", DateTime.Now);
                    setColumn("ActionType", actionType.ToString());
                    setColumn("Module", "System Administration >> Lookup Maintenance");
                    setColumn("MenuName", "Lookup Table");

                    ok = base.save(sqlHelperWs.RETAIN_DATASET);
                    if (ok)
                    {
                        try
                        {
                            string receiver = new irsInternetPage().getLookupValue("ParamApproverEmail", "FldCode", "ParamRecipient", "FldDescription");
                            string receivercc = new irsInternetPage().getLookupValue("ParamApproverEmail", "FldCode", "ParamRecipientCC", "FldDescription");
                            string receiverbcc = new irsInternetPage().getLookupValue("ParamApproverEmail", "FldCode", "ParamRecipientBCC", "FldDescription");
                            string subject = new irsInternetPage().getLookupValue("ParamApproverEmail", "FldCode", "ParamSubject", "FldDescription").Replace("@Region", System.Configuration.ConfigurationManager.AppSettings["SMTPRegion"]);
                            string body = new irsInternetPage().getLookupValue("ParamApproverEmail", "FldCode", "LookupParamBody", "FldDescription");
                            string[] arrReceiver = receiver.Split(';');
                            string[] arrReceiverCC = receivercc.Split(';');
                            string[] arrReceiverBCC = receiverbcc.Split(';');
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

                            body = body.Replace("@ModuleName", "System Administration >> Lookup Maintenance");
                            body = body.Replace("@MakerID", sessionUserId);
                            body = body.Replace("@ActionType", actionType.ToString());
                            body = body.Replace("@FldGroup", CleanDataTable(dt, "FldGroup"));
                            body = body.Replace("@FldCode", CleanDataTable(dt, "FldCode"));
                            body = body.Replace("@FldValue", CleanDataTable(dt, "FldValue"));
                            body = body.Replace("@FldDescription", CleanDataTable(dt, "FldDescription"));
                            body = body.Replace("@DateRequested", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                            SendEmail(subject, recipient, cc, bcc, body, null);
                        }
                        catch (Exception ex)
                        {
                            new csAuditLog().writeAudit("", "saveLookupMakerChecker Email Sending Failed", "Maker Checker Lookup", ex.Message, ex.StackTrace, csAuditLog.Category.Email);
                        }
                        return true;
                    }
                    else
                    {
                        new csAuditLog().writeAudit("", "saveLookupMakerChecker Error", "Maker Checker Lookup", "", base.Message, csAuditLog.Category.Insert);
                        return false;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "saveLookupMakerChecker Error", "Maker Checker Lookup", "", ex.Message, csAuditLog.Category.Insert);
                return false;
            }
        }

        /* RFC-31620 */
        public bool saveReportSetupMakerChecker(ActionType actionType, DataTable dt)
        {
            try
            {
                bool ok = false;
                string sessionUserId = ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId;
                
                if (dt != null && dt.Rows.Count > 0)
                {
                    base.addMode = true;
                    this.setBase("SELECT TOP 1 * FROM TBL_REPORTS_PER_BRANCH_TMP", true);
                    setColumn("Main_SEQ_ID", CleanDataTable(dt, "Main_SEQ_ID"));
                    setColumn("REP_ID", CleanDataTable(dt, "REP_ID"));
                    setColumn("BranchCode", CleanDataTable(dt, "BranchCode"));
                    setColumn("Maker", sessionUserId);
                    setColumn("Checker", "");
                    setColumn("DateAdded", DateTime.Now);
                    setColumn("CurrStatus", "For Approval");
                    setColumn("StatusDate", DateTime.Now);
                    setColumn("ActionType", actionType.ToString());
                    setColumn("Module", "System Administration >> Reports Setup");
                    setColumn("MenuName", "Reports Per Branch Table");

                    ok = base.save(sqlHelperWs.RETAIN_DATASET);
                    if (ok)
                    {
                        try
                        {
                            string receiver = new irsInternetPage().getLookupValue("ParamApproverEmail", "FldCode", "ParamRecipient", "FldDescription");
                            string receivercc = new irsInternetPage().getLookupValue("ParamApproverEmail", "FldCode", "ParamRecipientCC", "FldDescription");
                            string receiverbcc = new irsInternetPage().getLookupValue("ParamApproverEmail", "FldCode", "ParamRecipientBCC", "FldDescription");
                            string subject = new irsInternetPage().getLookupValue("ParamApproverEmail", "FldCode", "ParamSubject", "FldDescription").Replace("@Region", System.Configuration.ConfigurationManager.AppSettings["SMTPRegion"]);
                            string body = new irsInternetPage().getLookupValue("ParamApproverEmail", "FldCode", "ReportSetupParamBody", "FldDescription");
                            string[] arrReceiver = receiver.Split(';');
                            string[] arrReceiverCC = receivercc.Split(';');
                            string[] arrReceiverBCC = receiverbcc.Split(';');
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

                            body = body.Replace("@ModuleName", "System Administration >> Reports Setup");
                            body = body.Replace("@MakerID", sessionUserId);
                            body = body.Replace("@ActionType", actionType.ToString());
                            body = body.Replace("@Sequence", CleanDataTable(dt, "REP_ID"));
                            body = body.Replace("@BranchCode", CleanDataTable(dt, "BranchCode"));
                            body = body.Replace("@ReportGroup", CleanDataTable(dt, "ReportGroup"));
                            body = body.Replace("@ReportName", CleanDataTable(dt, "ReportName"));
                            if (actionType.ToString() == "DELETE")
                            {
                                body = body.Replace("@Status", "NOT ACTIVE");
                            }
                            else
                            {
                                body = body.Replace("@Status", "ACTIVE");
                            }
                            body = body.Replace("@DateRequested", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                            SendEmail(subject, recipient, cc, bcc, body, null);
                        }
                        catch (Exception ex)
                        {
                            new csAuditLog().writeAudit("", "saveReportSetupMakerChecker Email Sending Failed", "Maker Checker Reports Setup", ex.Message, ex.StackTrace, csAuditLog.Category.Email);
                        }
                        return true;
                    }
                    else
                    {
                        new csAuditLog().writeAudit("", "saveReportSetupMakerChecker Error", "Maker Checker Reports Setup", "", base.Message, csAuditLog.Category.Insert);
                        return false;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "saveReportSetupMakerChecker Error", "Maker Checker Reports Setup", "", ex.Message, csAuditLog.Category.Insert);
                return false;
            }
        }

        /* RFC-31620 */
        public bool saveReportLookupMakerChecker(ActionType actionType, DataTable dt)
        {
            try
            {
                bool ok = false;
                string sessionUserId = ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId;

                if (dt != null && dt.Rows.Count > 0)
                {
                    base.addMode = true;
                    this.setBase("SELECT TOP 1 * FROM TBL_REPORTS_TMP", true);
                    setColumn("Main_SEQ_ID", CleanDataTable(dt, "Main_SEQ_ID"));
                    setColumn("MainGroupID", CleanDataTable(dt, "MainGroupID"));
                    setColumn("MainGroupDESC", CleanDataTable(dt, "MainGroupDESC"));
                    setColumn("SubGroupID", CleanDataTable(dt, "SubGroupID"));
                    setColumn("SubGroupDESC", CleanDataTable(dt, "SubGroupDESC"));
                    setColumn("TextDisplay", CleanDataTable(dt, "TextDisplay"));
                    setColumn("ParameterNames", CleanDataTable(dt, "ParameterNames"));
                    setColumn("ParameterTypes", CleanDataTable(dt, "ParameterTypes"));
                    setColumn("ParameterDisplay", CleanDataTable(dt, "ParameterDisplay"));
                    setColumn("sp_Name", CleanDataTable(dt, "sp_Name"));
                    setColumn("Role", CleanDataTable(dt, "Role"));
                    setColumn("SQLValue", CleanDataTable(dt, "SQLValue"));
                    setColumn("Maker", sessionUserId);
                    setColumn("Checker", "");
                    setColumn("DateAdded", DateTime.Now);
                    setColumn("CurrStatus", "For Approval");
                    setColumn("StatusDate", DateTime.Now);
                    setColumn("ActionType", actionType.ToString());
                    setColumn("Module", "System Administration >> Reports Lookup");
                    setColumn("MenuName", "Reports Table");

                    ok = base.save(sqlHelperWs.RETAIN_DATASET);
                    if (ok)
                    {
                        try
                        {
                            string receiver = new irsInternetPage().getLookupValue("ParamApproverEmail", "FldCode", "ParamRecipient", "FldDescription");
                            string receivercc = new irsInternetPage().getLookupValue("ParamApproverEmail", "FldCode", "ParamRecipientCC", "FldDescription");
                            string receiverbcc = new irsInternetPage().getLookupValue("ParamApproverEmail", "FldCode", "ParamRecipientBCC", "FldDescription");
                            string subject = new irsInternetPage().getLookupValue("ParamApproverEmail", "FldCode", "ParamSubject", "FldDescription").Replace("@Region", System.Configuration.ConfigurationManager.AppSettings["SMTPRegion"]);
                            string body = new irsInternetPage().getLookupValue("ParamApproverEmail", "FldCode", "ReportLookupParamBody", "FldDescription");
                            string[] arrReceiver = receiver.Split(';');
                            string[] arrReceiverCC = receivercc.Split(';');
                            string[] arrReceiverBCC = receiverbcc.Split(';');
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

                            body = body.Replace("@ModuleName", "System Administration >> Reports Lookup");
                            body = body.Replace("@MakerID", sessionUserId);
                            body = body.Replace("@ActionType", actionType.ToString());
                            body = body.Replace("@ReportName", CleanDataTable(dt, "TextDisplay"));
                            body = body.Replace("@DateRequested", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                            SendEmail(subject, recipient, cc, bcc, body, null);
                        }
                        catch (Exception ex)
                        {
                            new csAuditLog().writeAudit("", "saveReportLookupMakerChecker Email Sending Failed", "Maker Checker Reports Lookup", ex.Message, ex.StackTrace, csAuditLog.Category.Email);
                        }
                        return true;
                    }
                    else
                    {
                        new csAuditLog().writeAudit("", "saveReportLookupMakerChecker Error", "Maker Checker Reports Lookup", "", base.Message, csAuditLog.Category.Insert);
                        return false;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "saveReportLookupMakerChecker Error", "Maker Checker Reports Setup", "", ex.Message, csAuditLog.Category.Insert);
                return false;
            }
        }

        /* RFC-31620 */
        public bool IsMakerAllowedBranchSetup(ActionType actionType, string seqId)
        {
            bool ret = false;
            try
            {
                base.EnforceConstraint = false;
                getUpdateDS("mc_BranchSetupCheckMakerAllowed", new Object[] { seqId, actionType.ToString() });
                if (hasRows)
                {
                    selectedRow = 0;
                    if (Convert.ToInt32(table.Rows[0]["Strict"].ToString()) > 0)
                    {
                        ret = true;
                    }
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "IsMakerAllowedBranchSetup Error", "Validate Strict Maker", ex.Message, ex.StackTrace, csAuditLog.Category.Email);
            }
            return ret;
        }

        public DataTable getBranchSetup(string BranchCode)
        {
            try
            {
                base.EnforceConstraint = false;
                getUpdateDS("isp_getRemittanceLimit", new Object[] { BranchCode });
                if (hasRows)
                {
                    selectedRow = 0;
                    return table;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "getBranchSetup Error", "Validate Branch Setup", ex.Message, ex.StackTrace, csAuditLog.Category.Email);
                return null;
            }
        }

        /* RFC-31620 */
        public bool saveBranchSetupMakerChecker(ActionType actionType, DataTable dt)
        {
            try
            {
                bool ok = false;
                string sessionUserId = ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId;

                if (dt != null && dt.Rows.Count > 0)
                {
                    base.addMode = true;
                    this.setBase("SELECT TOP 1 * FROM TBL_BRANCH_SETUP_TMP", true);
                    setColumn("Main_SEQ_ID", CleanDataTable(dt, "Main_SEQ_ID"));
                    setColumn("BranchCode", CleanDataTable(dt, "BranchCode"));
                    setColumn("CutOffStart", CleanDataTable(dt, "CutOffStart"));
                    setColumn("CutOffEnd", CleanDataTable(dt, "CutOffEnd"));
                    setColumn("StalePeriod", CleanDataTable(dt, "StalePeriod"));
                    setColumn("CommSetup", CleanDataTable(dt, "CommSetup"));
                    setColumn("RemittanceAmtLimit", CleanDataTable(dt, "RemittanceAmtLimit"));
                    setColumn("AggregateAmtLimit", CleanDataTable(dt, "AggregateAmtLimit"));
                    setColumn("SupervisorLimit", CleanDataTable(dt, "SupervisorLimit"));
                    setColumn("TellerLimit", CleanDataTable(dt, "TellerLimit"));
                    setColumn("ARMLimit", CleanDataTable(dt, "ARMLimit"));
                    setColumn("InetLimit", CleanDataTable(dt, "InetLimit"));
                    setColumn("RequiredID", CleanDataTable(dt, "RequiredID"));
                    setColumn("FxDeviation", CleanDataTable(dt, "FxDeviation"));
                    setColumn("TDD_StImp_Min", CleanDataTable(dt, "TDD_StImp_Min"));
                    setColumn("TDD_StImp_Max", CleanDataTable(dt, "TDD_StImp_Max"));
                    setColumn("TDD_FirstTimeStImp_Min", CleanDataTable(dt, "TDD_FirstTimeStImp_Min"));
                    setColumn("TDD_FirstTImeStImp_Max", CleanDataTable(dt, "TDD_FirstTImeStImp_Max"));
                    setColumn("CashAggregationLimit", CleanDataTable(dt, "CashAggregationLimit"));
                    setColumn("CashAggregationAmtLimit", CleanDataTable(dt, "CashAggregationAmtLimit"));
                    setColumn("AgentSysControl", CleanDataTable(dt, "AgentSysControl"));
                    setColumn("AgentLogReport", CleanDataTable(dt, "AgentLogReport"));
                    setColumn("ThreeDayAggregation", CleanDataTable(dt, "ThreeDayAggregation"));
                    setColumn("ThirtyDayAggregation", CleanDataTable(dt, "ThirtyDayAggregation"));
                    setColumn("AgentAggAmtReject", CleanDataTable(dt, "AgentAggAmtReject"));
                    setColumn("DailyLimit", CleanDataTable(dt, "DailyLimit"));
                    setColumn("AgentCheckIDAmount", (CleanDataTable(dt, "AgentCheckIDAmount") != "" ? CleanDataTable(dt, "AgentCheckIDAmount") : "0.00"));
                    if (actionType.ToString() == "DELETE")
                    {
                        setColumn("Deleted", true);
                    }
                    else
                    {
                        setColumn("Deleted", false);
                    }
                    setColumn("Maker", sessionUserId);
                    setColumn("Checker", "");
                    setColumn("DateAdded", DateTime.Now);
                    setColumn("CurrStatus", "For Approval");
                    setColumn("StatusDate", DateTime.Now);
                    setColumn("ActionType", actionType.ToString());
                    setColumn("Module", "Branch Administration >> Branch Setup");
                    setColumn("MenuName", "Branch Setup Table");

                    ok = base.save(sqlHelperWs.RETAIN_DATASET);
                    if (ok)
                    {
                        try
                        {
                            string receiver = new irsInternetPage().getLookupValue("ParamApproverEmail", "FldCode", "ParamRecipient", "FldDescription");
                            string receivercc = new irsInternetPage().getLookupValue("ParamApproverEmail", "FldCode", "ParamRecipientCC", "FldDescription");
                            string receiverbcc = new irsInternetPage().getLookupValue("ParamApproverEmail", "FldCode", "ParamRecipientBCC", "FldDescription");
                            string subject = new irsInternetPage().getLookupValue("ParamApproverEmail", "FldCode", "ParamSubject", "FldDescription").Replace("@Region", System.Configuration.ConfigurationManager.AppSettings["SMTPRegion"]);
                            string body = new irsInternetPage().getLookupValue("ParamApproverEmail", "FldCode", "BranchSetupBody", "FldDescription");
                            string[] arrReceiver = receiver.Split(';');
                            string[] arrReceiverCC = receivercc.Split(';');
                            string[] arrReceiverBCC = receiverbcc.Split(';');
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

                            body = body.Replace("@ModuleName", "Branch Administration >> Branch Setup");
                            body = body.Replace("@MakerID", sessionUserId);
                            body = body.Replace("@ActionType", actionType.ToString());
                            body = body.Replace("@BranchCode", CleanDataTable(dt, "BranchCode"));
                            body = body.Replace("@DateRequested", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                            SendEmail(subject, recipient, cc, bcc, body, null);
                        }
                        catch (Exception ex)
                        {
                            new csAuditLog().writeAudit("", "saveBranchSetupMakerChecker Email Sending Failed", "Maker Checker Branch Setup", ex.Message, ex.StackTrace, csAuditLog.Category.Email);
                        }
                        return true;
                    }
                    else
                    {
                        new csAuditLog().writeAudit("", "saveBranchSetupMakerChecker Error", "Maker Checker Branch Setup", "", base.Message, csAuditLog.Category.Insert);
                        return false;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "saveBranchSetupMakerChecker Error", "Maker Checker Branch Setup", "", ex.Message, csAuditLog.Category.Insert);
                return false;
            }
        }
    }
}
