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
using IRSWorld.bharosa;

namespace IRSWorld.Admin
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Login));

            this.Page.Response.Expires = 0;
            this.Page.Response.Cache.SetNoStore();
            this.Page.Response.AppendHeader("Pragma", "no-cache");

            if (Request.QueryString["uid"] != null)
            {
                string username = Request.QueryString["uid"].ToString();

                sqlHelperWs cmd = new sqlHelperWs();
                cmd.execStoredProcedure("activateUser", new Object[] { username });
            }

            Session["USER_ON_LOGIN"] = true;
            /* VAPT IAG 20180723 */
            Session["token_validity"] = null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void setSessionOnLogin()
        {
            Session["USER_ON_LOGIN"] = true;
        }

        /* VAPT IAG 20170515 */
        public void removeSessionOnLogin()
        {
            Session["USER_ON_LOGIN"] = null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public bool setMacAddress(String mac_address)
        {
            return new irsConfiguration().setMacAddress(mac_address);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool[] detect_setup(short tz)
        {
            bool oaam_down = true;
            try
            {
                irsClientData id = (irsClientData) Session["IRS_CLIENT_DATA"];
                id.ClientTZ = tz;
                oaam_down = getOaamSetting();
                id.OAAM = !oaam_down;
            }
            catch
            {
                throw new Exception("Session Expired.");
            }

            return new bool[] { new irsUsers().is_setup(), oaam_down };
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void close_session()
        {
            if (Session["IRS_CLIENT_DATA"] != null)
            {
                ((irsClientData)Session["IRS_CLIENT_DATA"]).logout();
            }
        }

        private bool getOaamSetting()
        {
            String s = new irsConfiguration().appsetting("OAAM_DOWN");
            if (s == null || s.Length == 0) return false;
            try
            {
                return bool.Parse(s);
            }
            catch { }
            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public String[] getClientProperties()
        {
            String msg = "";
            try
            {
                irsClientData cd = (irsClientData)Session["IRS_CLIENT_DATA"];
                if (cd == null) throw new Exception("Session Expired.");
                //bool oaam_down = getOaamSetting();
                //cd.OAAM = !oaam_down;

                bool oaam_down = !cd.OAAM;
                //====================================
                //RFC-10010 : SG Mobile 08192020
                //VAPT issues
                //return new String[] { cd.IPAddress, cd.HostName, (cd.IsArm ? "1" : "0"), cd.MACAddress, oaam_down.ToString() };
                return new String[] { "", "", (cd.IsArm ? "1" : "0"), "", oaam_down.ToString() };
                //=====
            }
            catch (Exception e)
            {
                msg = e.Message;
            }

            throw new Exception("Login@getClientProperties: " + msg);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public bool is_valid_gfmc(String CardNo)
        {
            return new irsARM().getGFMCRemitter(CardNo);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public bool is_valid_kiosk(String MacAddress)
        {
            return new irsARM().getKiosk(MacAddress);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public string getIRSWorldVersion()
        {
            /* PNBS Integration Project */
            String sver = new irsConfiguration().appsetting("IRSW_ReleaseDate");
            try
            {
                irsClientData id = (irsClientData)Session["IRS_CLIENT_DATA"];
                if (id != null)
                {
                    return (sver.StartsWith("version:")) ? (id.AppPath == String.Empty) ? sver : sver.Replace("version", id.AppPath) : id.AppPath + ": " + sver;
                }
            }
            catch { }

            return sver;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public string getGFMCCode(bool isOnline)
        {
            if (isOnline == false)
            {
                return new irsConfiguration().appsetting("GFCProductCode");
            }
            else
            {
                return new irsARM().getGFMCProductCode();
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool handleLogin(String userid)
        {
            try
            {
                //
                // clean the existing session
                //
                IRSWorld.bharosa.AppSessionData.GetInstance(Session).Reset(Session);
                AppSessionData sessionData = AppSessionData.GetInstance(Session);

                //
                // save the current page URL; this will be used in:
                // - LoginHandlerPage will redirect to this page in case of error
                // - LogoutPage       will redirect to this page after user logs out
                //
                AppUtil.GetInstance().SetLoginPage(sessionData, "~/Admin/Login.aspx");

                //
                // save the username; this will be validated in PasswordPage.
                //
                sessionData.LoginId = userid.Trim();

                //
                // redirect to login jump page, which will load flash and redirect to LoginHandlerPage
                //

                return true;
            }
            catch { }

            throw new Exception("Check OAAM."); 
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String getUserEmailAddress(String userid)
        {
            irsUsers iu = new irsUsers();
            try
            {
                String emailad = iu.getEmailsByUserId(userid);
                if (emailad != null) return emailad;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            throw new Exception(iu.Message);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String mailPassword(String userid, String emailad)
        {
            setSessionOnLogin();

            irsUsers iu = new irsUsers();
            try
            {
                if (iu.email_password(userid, emailad, "disc"))
                {
                    return "Password disclosed. Please check your email.";
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            throw new Exception(iu.Message);
        }

        private String getRedirectUrl(String apath)
        {
            irsConfiguration ic = new irsConfiguration();
            String ws = ic.appsetting("IRWorld_Alt_Site");
            if (ws != null && ws.Length > 0)
            {
                bool enabled = false;
                String protocol = "http";
                String url = "";
                String[] wss = ws.Split(',');
                if (wss.Length > 0)
                {
                    for (int i = 0; i < wss.Length; i++)
                    {
                        String[] tt = wss[i].Split('=');
                        if (tt[0].Trim().ToUpper().Equals("ENABLED")) try { Convert.ToBoolean(tt[1]); }
                            catch { }
                        if (tt[0].Trim().ToUpper().Equals("PROTOCOL")) protocol = tt[1].Trim();
                        if (tt[0].Trim().ToUpper().Equals("URL")) url = tt[1].Trim();
                    }
                    if (enabled) return protocol + "://" + url;
                }
            }
            return String.Empty;
        }

        public bool auto_disabled_user_prompt(string user_id)
        {
            sqlHelperWs sqlws = new sqlHelperWs();
            DataTable dt = null;
            string param_statement = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(user_id).Equals(true))
                {
                    user_id = "";
                }

                user_id = user_id.Replace("'", "");

                /* VAPT 20190206 */
                param_statement = "select count(1) cnt_user from tbl_users where lastaccessdate is null and role NOT IN (1, 99) and flags = 4 and userid = '" + user_id + "'";

                /* VAPT IAG 20170515 */
                setSessionOnLogin();
                dt = sqlws.getDataSet(param_statement, false, new object[] { }).Tables[0];
                removeSessionOnLogin();

                if (dt != null && dt.Rows.Count > 0)
                {
                    if (Convert.ToInt32(dt.Rows[0]["cnt_user"]) > 0)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                sqlws = null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool store_uid(string xid)
        {
            Session["xid"] = xid;
            return true;
        }

        public void set_login_last_access_date(string user_id)
        {
            DataTable dt = null;
            DateTime actual_dt;
            DateTime timezone_dt;
            sqlHelperWs sqlws = new sqlHelperWs();

            string branch_agent_timezone = string.Empty;
            string manila_server_timezone = string.Empty;

            Session["LOGIN_DISPLAY_LAST_ACCESS_DATE"] = null;

            try
            {
                /* VAPT IAG 20170515 */
                setSessionOnLogin();
                dt = sqlws.getDataSet("log_last_access_dt", true, new object[] { user_id }).Tables[0];
                removeSessionOnLogin();

                if (dt != null && dt.Rows.Count > 0)
                {
                    if (string.IsNullOrEmpty(dt.Rows[0]["actual_date"].ToString()).Equals(false))
                    {
                        if (string.IsNullOrEmpty(dt.Rows[0]["user_time_zone"].ToString()).Equals(false))
                        {
                            manila_server_timezone = new irsTimeZone().ServerTimezone;
                            branch_agent_timezone = dt.Rows[0]["user_time_zone"].ToString();
                            actual_dt = Convert.ToDateTime(dt.Rows[0]["actual_date"]);
                            timezone_dt = new irsTimeZone().tzConvert(actual_dt, manila_server_timezone, branch_agent_timezone);
                            Session["LOGIN_DISPLAY_LAST_ACCESS_DATE"] = "LAST LOGIN: " + timezone_dt.ToString("MMMM dd, yyyy hh:mm:ss tt");
                        }
                    }
                }
            }
            catch
            {
                Session["LOGIN_DISPLAY_LAST_ACCESS_DATE"] = null;
            }
            finally
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                sqlws = null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object[] useridExist(String userid)
        {
            /* VAPT 20190917 */
            string userIDAntiXSS = string.Empty;
            try
            {
                userIDAntiXSS = System.Web.HttpUtility.UrlEncode(userid);
            }
            catch
            {
                userIDAntiXSS = userid;
            }

            irsUsers iu = new irsUsers();
            /* VAPT 20190917 */
            bool exist = iu.user_exist_login("USERID", userIDAntiXSS);

            /* VAPT 20180222 */
            string http_x_forwarded_for = string.Empty;
            string remote_addr = string.Empty;
            string true_client_ip = string.Empty;
            try
            {
                http_x_forwarded_for = Context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            catch { }

            try
            {
                remote_addr = Context.Request.ServerVariables["REMOTE_ADDR"].ToString();
            }
            catch { }

            try
            {
                true_client_ip = Context.Request.ServerVariables["TRUE-CLIENT-IP"].ToString();
            }
            catch { }

            Session["IPAddress"] = http_x_forwarded_for + ";" + remote_addr + ";" + true_client_ip;
            /* VAPT 20190917 */
            new csAuditLog().writeVAPTLogger(userid, "LOGIN", "A USER ID WAS USED TO LOGIN", "USER EXISTS IN DATABASE? " + exist.ToString(), userIDAntiXSS, "", csAuditLog.AuditType.User, csAuditLog.AlertType.Information);
            Session["VAPT_LOGIN"] = true;
            Session["SES_USER_LOG_ID"] = null;

            if (exist)
            {
                /* VAPT 20190917 */
                Session["SES_USER_LOG_ID"] = userIDAntiXSS;
                short iflag = Convert.ToInt16(iu.getColumn("Flags"));

                /* VAPT 20190917 */
                if (this.auto_disabled_user_prompt(userIDAntiXSS) == true)
                {
                    /* VAPT 20190206 */
                    throw new Exception("Account has been locked due to inactivity or exceeded the maximum tries. Please call ACD to activate your user.");
                }

                if (iflag < iu.FLAG_NEW | iflag > iu.FLAG_EXPIRED)
                {
                    throw new Exception("User is disabled.");
                }

                /* VAPT 20190917 */
                this.set_login_last_access_date(userIDAntiXSS);

                /* VAPT 20190206 */
                if (iflag == iu.FLAG_EXPIRED)
                {
                    new csAuditLog().writeVAPTLogger(userid, "LOGIN", "USER PASSWORD HAS EXPIRED", "THE USER PASSWORD FOR " + userid + " HAS EXPIRED.", userIDAntiXSS, "", csAuditLog.AuditType.User, csAuditLog.AlertType.Alert);
                    throw new Exception("The user password has expired. Please call ACD to activate your user.");
                }
            }

            String apath = ((irsClientData)Session["IRS_CLIENT_DATA"]).AppPath;
            if (apath.Equals("Devel"))
            {
                apath = "/";
            }

            String rurl = getRedirectUrl(apath);

            return new Object[] { exist, rurl };
        }
    }
}
