using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Data;

namespace IRSClassLib
{
    [Serializable]
    public class irsClientData
    {
        public enum LoginGroups : short
        {
            US = 1,
            EA = 2
        }

        internal String _macaddr;
        internal String _ipaddr;
        internal bool _is_arm;
        internal String _hostname;
        internal String _cifid;
        internal short _cifclass;
        internal String _userid;
        internal String _location;
        internal short _xdays;
        internal short _role;
        internal short _flag;
        internal byte _tries;
        internal bool _is_setup;
        internal short _client_tz;
        internal bool _no_logout;
        internal short _login_group;

        internal bool _oaam;

        internal String _apath;
        internal String _server;

        internal bool _disclaimer;

        /* 20160921 OAAM bypassed */
        internal bool _oaam_screen;
        internal bool _oaam_success;

        /* VAPT IAG 20180730 */
        internal bool _super_admin;

        /* VAPT IAG 20180723 */
        internal bool _mfa_screen;
        internal bool _mfa_success;

        /* VAPT IAG 20170515 */
        internal bool _user_exist;
        internal DataTable _user_db_datatable;
        internal DateTime _user_branch_dt;
        internal short _user_login_group;

        private byte _ctr;

        public bool IsReady()
        {
            int i = 0;
            int n = 5;
            int lim = 10 * 60000;
            while (_ctr < 2)
            {
                Thread.Sleep(n);
                i += n;
                if (i > lim) break;
            }
            return (_ctr > 1);
        }

        public short Flag
        {
            get { return _flag; }
        }

        public String CIFID
        {
            get { return _cifid; }
        }

        public String UserId
        {
            get { return _userid; }
        }

        public String MACAddress
        {
            get { IsReady(); return _macaddr; }
        }

        public String IPAddress
        {
            get { IsReady(); return _ipaddr; }
        }

        public String HostName
        {
            get { IsReady(); return _hostname; }
        }

        public bool IsArm
        {
            get { return _is_arm; }
        }

        public String Location
        {
            get { return _location; }
        }

        public short Role
        {
            get { return _role; }
        }

        public bool isBranchAdmin
        {
            get { return new irsUsers().isBranchAdmin(_role); }
        }

        public bool isSystemAdmin
        {
            get { return new irsUsers().isSystemAdmin(_role); }
        }

        public void logout()
        {
            if (UserId != null && UserId.Length > 0)
            {
                new irsUsers().logout(UserId);
            }
        }

        /* VAPT IAG 20180730 */
        public bool SuperAdmin
        {
            get { return _super_admin; }
            set { _super_admin = value; }
        }

        /* VAPT IAG 20180723 */
        public bool MFAScreen
        {
            get { return _mfa_screen; }
            set { _mfa_screen = value; }
        }

        /* VAPT IAG 20180723 */
        public bool MFASuccess
        {
            get { return _mfa_success; }
            set { _mfa_success = value; }
        }

        public bool OAAM
        {
            get { return _oaam; }
            set { _oaam = value; }
        }

        /* 20160922 OAAM bypassed */
        public bool OAAMScreen
        {
            get { return _oaam_screen; }
            set { _oaam_screen = value; }
        }

        /* 20160921 OAAM bypassed */
        public bool OAAMSuccess
        {
            get { return _oaam_success; }
            set { _oaam_success = value; }
        }

        /* VAPT IAG 20170515 */
        public bool UserExist
        {
            get { return _user_exist; }
            set { _user_exist = value; }
        }

        /* VAPT IAG 20170515 */
        public DataTable UserDBDatatable
        {
            get { return _user_db_datatable; }
            set { _user_db_datatable = value; }
        }

        /* VAPT IAG 20170515 */
        public DateTime UserBranchDateTime
        {
            get { return _user_branch_dt; }
            set { _user_branch_dt = value; }
        }

        /* VAPT IAG 20170515 */
        public short UserLoginGroup
        {
            get { return _user_login_group; }
            set { _user_login_group = value; }
        }

        public short ClientTZ
        {
            get { return _client_tz; }
            set { _client_tz = value; }
        }

        public bool IsSetup
        {
            get { return _is_setup; }
        }

        public short LoginGroup
        {
            get { return _login_group; }
        }

        public byte Tries
        {
            get { return _tries; }
        }

        public String AppPath
        {
            get
            {
                if (_apath == null) return String.Empty;
                if (_apath.Length == 1 && _apath.Equals("/")) return "Devel";
                if (_apath.StartsWith("/")) return _apath.Substring(1);
                return _apath;
            }
        }

        public bool Disclaimer
        {
            get { return _disclaimer; }
            set { _disclaimer = (((Object)value) == DBNull.Value) ? false : value; }
        }

        public short XDays
        {
            get { return _xdays; }
        }

        private void getClientInfo(Object octx) //()
        {
            irsConfiguration ic = new irsConfiguration();
            System.Web.HttpContext ctx = (System.Web.HttpContext)octx;
            _ipaddr = ic.getClientIPAddress(ctx);
            //_ipaddr = ic.getClientIPAddress();
            _hostname = ic.getHostNameByIPAddress(_ipaddr);
            _macaddr = ic.getClientMacAddress(_hostname);
            _is_arm = ic.isArm(_macaddr);

            try { _server = ctx.Request.Url.Host; } catch { }
            try { _apath = ctx.Request.ApplicationPath; } catch { }


            ctx = null;
            ic = null;
            _ctr++;
        }

        private void set_setup()
        {
            irsSqlCommonsWs coms = new irsSqlCommonsWs();
            coms.IsWinApp = true;
            _is_setup = coms.inSetupMode();
            coms = null;
            _ctr++;
        }

        private bool oaam_setting()
        {
            String s = new irsConfiguration().appsetting("OAAM_DOWN");
            if (String.IsNullOrEmpty(s)) return false;
            try { return bool.Parse(s); } catch { }
            return false;
        }

        public irsClientData()
        {
            _oaam = !oaam_setting();//false;
            _ctr = 0;
            _tries = 0;
            _no_logout = false;

            new Thread(new ParameterizedThreadStart(getClientInfo)).Start(System.Web.HttpContext.Current);
            //getClientInfo();
            new Thread(new ThreadStart(set_setup)).Start();
            //set_setup();
        }
    }
}
