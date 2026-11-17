using System;
using System.Collections;
using System.Data;
using System.Configuration;
using System.Text;

using System.Management;
using ConnObjDecrypt;

namespace IRSClassLib
{
    [Serializable]
    public class irsConfiguration
    {
        private String constring_suffix = "ConnectionString";
        private String FTP_constring_suffix = "FTPConnectionString";
        private String SMS_constring_suffix = "SMSConnectionString";

        public bool isPasswordEncrypted
        {
            get
            {
                try
                {
                    String s = appsetting("ITG_PASSWORD_ENCRYPTION");
                    if (s == null || s.Length == 0) return false;

                    return Convert.ToBoolean(s);
                }
                catch { }

                return false;
            }
        }

        public String RunMode
        {
            get
            {
                return appsetting("RunMode");
            }
        }

        private String getConnectionString(String suffix)
        {
            try
            {
                String cstring = appsetting(RunMode + suffix);
                if (isPasswordEncrypted)
                {
                    return new Decryptor().DecryptConnectionString(cstring);
                }

                return cstring;
            }
            catch { }
            return null;
        }

        public String ConnectionString
        {
            get
            {
                return getConnectionString(constring_suffix);
            }
        }

        public String FTPConnectionString
        {
            get
            {
                return getConnectionString(FTP_constring_suffix);
            }
        }

        public String SMSConnectionString
        {
            get
            {
                return getConnectionString(SMS_constring_suffix);
            }
        }

        public String ConnectionStringNoRunMode(String keyname)
        {
            try
            {
                String fstring = appsetting(keyname);
                if (isPasswordEncrypted)
                {
                    return new Decryptor().DecryptConnectionString(fstring);
                }

                return fstring;
            }
            catch { }

            return null;
        }

        public String machine_name
        {
            get { return System.Environment.MachineName; }
        }

        public String appsetting(String key)
        {
            try
            {
                return (String) new AppSettingsReader().GetValue(key, typeof(System.String));
            }
            catch
            {}

            return null;
        }

        public String getClientIPAddress(System.Web.HttpContext ctx)
        {
            try
            {
                //String ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                String ip = ctx.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!String.IsNullOrEmpty(ip))
                {
                    String[] ipRange = ip.Split(',');
                    int le = ipRange.Length - 1;
                    string trueIP = ipRange[le];
                }
                else
                {
                    //ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                    ip = ctx.Request.ServerVariables["REMOTE_ADDR"];
                }

                if (ip != null && ip.Length > 0) return ip;
            }
            catch { }

            return null;
        }

        public String getClientIPAddress()
        {
            return this.getClientIPAddress(System.Web.HttpContext.Current);
        }

        public String getHostNameByIPAddress(String ipaddr)
        {
            try
            {
                String s = System.Net.Dns.GetHostEntry(ipaddr).HostName;
                if (s != null && s.Length > 0) return s;
            }
            catch { }

            return null;
        }

        public String getClientMacAddress(String hostname)
        {
            try
            {
                if (String.IsNullOrEmpty(hostname)) return null;

                String xname = hostname.Split('.')[0];
                ManagementScope ms = new ManagementScope("\\\\" + xname + "\\root\\cimv2");
                ObjectQuery oq = new ObjectQuery("SELECT MACAddress FROM Win32_NetworkAdapter");
                ManagementObjectSearcher mos = new ManagementObjectSearcher(ms, oq);
                ManagementObjectCollection moc = mos.Get();

                foreach (ManagementObject mo in moc)
                {
                    string macAdd = "MAC Address: " + mo["MACAddress"].ToString();
                    return mo["MACAddress"].ToString();
                }
            }
            catch
            {}

            return null;

        }

        public bool isArm(String mac)
        {
            bool is_arm = false;
            try
            {
                if (mac != null || mac.Length > 0)
                {
                    irsSqlCommonsWs coms = new irsSqlCommonsWs();
                    coms.IsWinApp = true;
                    is_arm = Convert.ToBoolean(coms.execScalar("SELECT CardReader FROM tbl_machines WHERE MacAddress = '" + mac + "'"));
                    coms = null;
                }
            }
            catch
            { }
            return is_arm;
        }

        public bool setMacAddress(String mac)
        {
            irsClientData cd = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
            if (cd != null)
            {
                cd._macaddr = mac;
                try
                {
                    if (mac == null || mac.Length == 0) cd._is_arm = false;
                    else
                    {
                        cd._is_arm = Convert.ToBoolean(new irsSqlCommonsWs().execScalar("SELECT CardReader FROM tbl_machines WHERE MacAddress = '" + mac + "'"));
                    }
                }
                catch
                { } 
                return cd.IsArm;
            }

            throw new Exception("irsConfiguration@setMacAddress: Unable to obtain session data");
        }

        public bool isArm()
        {
            try
            {
                return ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"])._is_arm;

            }
            catch { }
            return false;
        }

        public irsConfiguration()
        { }

        public irsConfiguration(String constring_suffix)
        {
            this.constring_suffix = constring_suffix;
        }
    }
}
