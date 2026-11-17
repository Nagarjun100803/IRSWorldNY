using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

namespace IRSClassLib
{
    [Serializable]
    public class irsAuditLogger : irsSqlCommonsWs
    {
        private Dictionary<String, irsAuditLogger> keeptable;
        private bool force_auditlog;

        public enum CRUDS
        {
            LOGIN,
            INSERT,
            UPDATE,
            DELETE,
            LOGOUT,
            QUERY,
            ERROR
        }

        internal String[] CRUD_TYPES =
        {
            "LOGIN",
            "INSERT",
            "UPDATE",
            "DELETE",
            "LOGOUT",
            "QUERY",
            "ERROR"
        };

        internal int SessionId
        {
            get
            {
                return Math.Abs(System.Web.HttpContext.Current.Session.SessionID.GetHashCode());
            }
        }

        internal String UserId
        {
            get
            {
                irsClientData ld = ClientData;
                if (ld._is_setup) return "setup";
                return (ld == null) ? ld.MACAddress : ld.UserId;
            }
        }

        internal bool writeLog(Object[] values)
        {
            /* VAPT IAG 20170515 */
            bool is_winapp = false;
            is_winapp = base.IsWinApp;
            base.IsWinApp = true;
            int ra = execStoredProcedureUpdate("spAddAuditLog", values);
            base.IsWinApp = is_winapp;

            if (ra < 1)
            {
                Message = "WRITE LOG FAILED (" + (String)values[5] + ") " + ra;
                return false;
            }
            return true;
        }

        public bool writeLog(String tblname, String colname, int crudtype, String old_value, String new_value, String key_value)
        {
            Object[] values =
            {
                SessionId,
                UserId,
                DateTime.Now,
                tblname,
                colname,
                CRUD_TYPES[crudtype],
                old_value,
                new_value,
                key_value
            };
            return writeLog(values);
        }

        public bool writeLog(int crudtype)
        {
            Object[] values =
            {
                SessionId,
                UserId,
                DateTime.Now,
                DBNull.Value,
                DBNull.Value,
                CRUD_TYPES[crudtype],
                DBNull.Value,
                DBNull.Value,
                DBNull.Value
            };

            return writeLog(values);
        }

        public bool logError(String module_name, String method_name, String errmsg)
        {
            String xerrmsg = (errmsg == null) ? String.Empty : errmsg;
            String ov = (xerrmsg.Length > 255) ? xerrmsg.Substring(0, 255) : xerrmsg;
            String nv = (xerrmsg.Length > 255) ? ((xerrmsg.Length - 255) > 255) ? xerrmsg.Substring(255, 255) : xerrmsg.Substring(255) : null;
            return writeLog(module_name, method_name, (int)CRUDS.ERROR, ov, nv, String.Empty);
        }

        public bool sign_in()
        {
            /* VAPT 20180222 */
            new csAuditLog().writeVAPTLogger("", "LOGIN", "LOGIN SUCCESSFULLY", "", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Information);
            return writeLog((int)CRUDS.LOGIN);
        }

        public void sign_out()
        {
            bool ok = writeLog((int)CRUDS.LOGOUT);
            /* VAPT 20180222 */
            new csAuditLog().writeVAPTLogger("", "LOGOUT", "LOGOFF SUCCESSFULLY", "", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Information);
            System.Web.HttpContext.Current.Session["IRS_AUDIT_LOGGER"] = null;
            System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] = null;
        }

        internal irsDataKeep getKeeper(String tblname)
        {
            if (keeptable == null) keeptable = new Dictionary<string, irsAuditLogger>();
            if (keeptable.ContainsKey(tblname)) return (irsDataKeep)keeptable[tblname];
            irsDataKeep rd = new irsDataKeep(tblname, this);
            keeptable.Add(tblname, rd);
            return rd;
        }

        internal void disposeKeeper(String tblname)
        {
            keeptable.Remove(tblname);
        }

        internal bool forcedLog
        {
            get { return force_auditlog; }
            set { force_auditlog = value; }
        }

        public irsAuditLogger()
        {
            forcedLog = false;
            IsWinApp = true;
        }

        ~irsAuditLogger()
        {
            keeptable = null;
        }
    }
}
