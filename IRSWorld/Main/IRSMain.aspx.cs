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

namespace IRSWorld.Main
{
    public partial class IRSMain : System.Web.UI.Page
    {
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
            AjaxPro.Utility.RegisterTypeForAjax(typeof(IRSMain));
            /* VAPT IAG 20170516 */
            irsClientData id = (irsClientData)Session["IRS_CLIENT_DATA"];
            if (id.UserId != null)
            {
                if (id.MFAScreen) //MFA is enabled
                {
                    if (!id.MFASuccess) //MFA authenticated
                    {
                        new csControl().execScript(this.Page, "SPERR_PAGE", "spawn_errpage('Session Expired.');");
                    }
                    else
                    {
                        Session["VAPT_LOGIN"] = null;
                    }
                }
                else
                {
                    bool b = false;
                    try
                    {
                        /* VAPT 20191010 */
                        short role = 0;
                        try
                        {
                            if (Session["IRSMainRole"] != null)
                            {
                                role = (short)Session["IRSMainRole"];
                            }
                            else
                            {
                                role = Convert.ToInt16(new irsInternetPage().getLookupValue("User Roles", "FldCode", "SA", "FldValue"));
                                Session.Add("IRSMainRole", role);
                            }
                        }
                        catch
                        {
                            role = Convert.ToInt16(new irsInternetPage().getLookupValue("User Roles", "FldCode", "SA", "FldValue"));
                            Session.Add("IRSMainRole", role);
                        }
                        if (id.Role == role)
                        {
                            id.SuperAdmin = true;
                            Session["token_validity"] = true;
                            id.MFAScreen = true;
                            id.MFASuccess = true;
                            b = true;
                        }
                    }
                    catch { }
                    if (!b)
                    {
                        Session["VAPT_LOGIN"] = null;
                    }
                }
            }
        }

        public bool checkSession()
        {
            /* VAPT IAG 20170516 */
            irsClientData id = (irsClientData)Session["IRS_CLIENT_DATA"];
            bool b = false;
            string userID = ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).UserId;

            if (id.UserId != null)
            {
                if (this.check_if_token_exist()) //MFA
                {
                    b = true;
                }
                else
                {
                    /* VAPT 20191010 */
                    string MFA = string.Empty;
                    try
                    {
                        if (Session["IRSMainMFA"] != null)
                        {
                            MFA = (string)Session["IRSMainMFA"];
                        }
                        else
                        {
                            MFA = new irsInternetPage().getLookupValue("MFA", "FldDescription", id.Location, "FldCode");
                            Session.Add("IRSMainMFA", MFA);
                        }
                    }
                    catch
                    {
                        MFA = new irsInternetPage().getLookupValue("MFA", "FldDescription", id.Location, "FldCode");
                        Session.Add("IRSMainMFA", MFA);
                    }
                    
                    if (MFA == "0")
                    {
                        new csAuditLog().writeVAPTLogger(id.UserId, "MFA", "MFA DISABLED FOR LOCATION " + id.Location + ". MFA WILL NOT BE VALIDATED", "", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Alert);
                    }
                    else
                    {
                        new csAuditLog().writeVAPTLogger(id.UserId, "MFA", "USER WAS NOT ABLE TO PROVIDE THE CORRECT TOKEN FOR TWO FACTOR AUTHENTICATION", "", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Alert);
                    }
                    new csControl().execScript(this.Page, "SPERR_PAGE", "spawn_errpage('Session Expired.');");
                }
            }
            else
            {
                new csAuditLog().writeVAPTLogger(id.UserId, "LOGIN", "SESSION HAS TIMED-OUT", "", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Alert);
                new csControl().execScript(this.Page, "SPERR_PAGE", "spawn_errpage('Session Expired.');");
            }
            return b;
        }

        /* VAPT IAG 20170530 */
        public void isUserModule(Page page)
        {
            string module = GetCurrentPageName();
            short role = ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).Role;
            string userID = ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).UserId;
            /* VAPT 20191010 */
            DataTable dt = new DataTable();
            int i = 0;
            bool b = false;

            /* VAPT 20191010 */
            try
            {
                if (Session["IRSMainIsUserModule"] != null)
                {
                    dt = Session["IRSMainIsUserModule"] as DataTable;
                }
                else
                {
                    dt = new irsUsers().getRolePermissionView(role);
                    Session.Add("IRSMainIsUserModule", dt);
                }
            }
            catch
            {
                dt = new irsUsers().getRolePermissionView(role);
                Session.Add("IRSMainIsUserModule", dt);
            }
            
            DataTable dt_access = new DataTable();
            try
            {
                if (Session["IRSMainDTAccess"] != null)
                {
                    dt_access = Session["IRSMainDTAccess"] as DataTable;
                }
                else
                {
                    dt_access = new irsUsers().getUserAccessPermission(role, module);
                    Session.Add("IRSMainDTAccess", dt_access);
                }
            }
            catch
            {
                dt_access = new irsUsers().getUserAccessPermission(role, module);
                Session.Add("IRSMainDTAccess", dt_access);
            }

            /* VAPT 20190325 */
            if (dt != null && dt.Rows.Count > 0)
            {
                for (i = 0; i <= (dt.Rows.Count - 1); i++)
                {
                    if (dt.Rows[i]["ModuleURL"].ToString().ToLower().IndexOf(module.ToLower()) > 0)
                    {
                        if (dt.Rows[i]["AccessType"].ToString() == "1")
                        {
                            b = true;
                            break;
                        }
                        else
                        {
                            if (dt_access != null && dt_access.Rows.Count > 0)
                            {
                                b = true;
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                /* VAPT 20190325 */
                new csAuditLog().writeVAPTLogger("", module, "APPLICATION CANNOT CONNECT TO DATABASE", "", "", "", csAuditLog.AuditType.Systems, csAuditLog.AlertType.Alert);
                new csControl().execScript(page, "SPERR_PAGE", "spawn_errpage('Application cannot connect to database. Please report to System Administrator.');");
            }
            
            if (!b)
            {
                /* VAPT 20180222 */
                new csAuditLog().writeVAPTLogger("", module, "USER IS ACCESSING A RESTRICTED PAGE", "", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Alert);
                new csControl().execScript(page, "SPERR_PAGE", "spawn_errpage('You are not authorized to view this page. Access denied!');");
            }
            else
            {
                /* VAPT 20191010 */
                dt = new DataTable();
                try
                {
                    if (Session["IRSMaingetMenuView"] != null)
                    {
                        dt = Session["IRSMaingetMenuView"] as DataTable;
                    }
                    else
                    {
                        dt = new irsMenu().getMenuView();
                        Session.Add("IRSMaingetMenuView", dt);
                    }
                }
                catch
                {
                    dt = new irsMenu().getMenuView();
                    Session.Add("IRSMaingetMenuView", dt);
                }

                int j = 0;
                string moduleName = string.Empty;

                for (j = 0; j <= (dt.Rows.Count - 1); j++)
                {
                    if (dt.Rows[j]["ModuleURL"] != DBNull.Value)
                    {
                        if (dt.Rows[j]["ModuleURL"].ToString().IndexOf(module) >= 0)
                        {
                            moduleName = dt.Rows[j]["MenuName"].ToString();
                            break;
                        }
                    }
                }
                new csAuditLog().writeVAPTLogger("", moduleName, "USER ACCESSED A MODULE", "", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Information);
            }
        }

        /* VAPT IAG 20170530 */
        private string GetCurrentPageName()
        {
            string path = HttpContext.Current.Request.Url.AbsolutePath;
            System.IO.FileInfo fi = new System.IO.FileInfo(path);
            return fi.Name;
        }

        /* PNBS Integration Project */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getIRSWVersionNo()
        {
            return new irsConfiguration().appsetting("IRSW_VersionNo");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void close_session()
        {
            if (Session["IRS_CLIENT_DATA"] != null)
            {
                ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).logout();
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public Object getMenuData()
        {
            irsMenu menu = new irsMenu();
            if (Session["IRS_CLIENT_DATA"] != null)
            {
                irsClientData id = (IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"];

                DataTable dt = new DataTable();

                try
                {
                    dt = menu.getMenuView(id.UserId);
                    new csAuditLog().writeVAPTLogger("", "MAIN PAGE", "GET USER MENU", "", id.UserId, "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                }
                catch (Exception)
                {
                    dt = menu.getMenuView(id.UserId);
                }

                if (dt != null)
                {
                    return dt;
                }

                throw new Exception("GET MENU: " + menu.Message);
            }
            throw new Exception("Session Expired.");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public int isAuthenticated(String wname, bool reqLogin)
        {
            if (!reqLogin) return 4;

            if (new irsSqlCommonsWs().inSetupMode()) return 2;

            int valid = (Session["IRS_CLIENT_DATA"] != null && (((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).UserId != null && ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).UserId.Length > 0)) ? 1 : 0;
            if (valid == 1)
            {
                if (wname == null || wname == "")
                {
                    valid = 0;
                    throw new Exception("Multiple window session is not currently supported.");
                }
                else
                {
                    /* VAPT 20180723 */
                    if (!((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).MFASuccess && ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).MFAScreen)
                    {
                        valid = 0;
                        throw new Exception("You must be MFA authenticated before you can proceed. Session Closed!");
                    }
                    else
                    {
                        if (((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).MFAScreen)
                        {
                            String s = new irsUsers().getUserGroupByRole(((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).UserId);
                            if (s.IndexOf("Client") > -1) valid = 3;
                        }
                    }
                }
            }

            return valid;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public bool showImagePhrase()
        {
            if (IRSClientData.OAAM)
            {
                IRSWorld.bharosa.AppSessionData sessionData = IRSWorld.bharosa.AppSessionData.GetInstance(Session);
                string customerId = sessionData.CustomerId;
                Bharosa.VCrypt.Client.IBharosaProxy proxy = Bharosa.VCrypt.Client.BharosaClientFactory.getProxyInstance();
                Bharosa.VCrypt.Common.Auth.VCryptQuestionList questions = proxy.getAllMappedSignOnQuestions(customerId);

                if (questions != null && questions.Count == 0) return true;
            }

            if (Session["SHOW_IMAGE_PHRASE"] != null && ((bool)Session["SHOW_IMAGE_PHRASE"]) == true)
            {
                return true;
            }

            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public String getLocalTimeString()
        {
            if (IRSClientData != null && IRSClientData.UserId != null)
            {
                return new irsTimeZone().branchLocalTime(IRSClientData.Location).ToString("MM/dd/yyyy hh:mm tt");
            }

            throw new Exception("Session Expired.");
        }

        private String getBranchName(String brcode)
        {
            String sql = "SELECT BranchName FROM TBL_BRANCHES WHERE BranchCode = '" + brcode + "'";
            Object o = new irsSqlCommonsWs().execScalar(sql);
            if (o != DBNull.Value)
            {
                return (String)o;
            }

            return String.Empty;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object[] getLoginData()
        {
            String locdesc = this.getBranchName(IRSClientData.Location);
            DateTime bdt;
            try
            {
                bdt = new irsTimeZone().branchLocalTime(IRSClientData.Location);
            }
            catch
            {
                bdt = DateTime.Now;
            }
            int exal = 5;
            try
            {
                DataTable dt = new irsUsers().getLookup(new Object[] { "Password", "Expire Alert", -1 });
                if (dt != null && dt.Rows.Count > 0)
                {
                    exal = Convert.ToInt32(dt.Rows[0]["FldDescription"]);
                }
            }
            catch
            {
                exal = 5;
            }

            bool notifi = IRSClientData.XDays <= exal;
            /* VAPT IAG 20180730 */
            irsClientData id = (irsClientData)Session["IRS_CLIENT_DATA"];
            if (id.SuperAdmin)
            {
                notifi = false;
            }
            /* VAPT 20190206 */
            if (IRSClientData.XDays <= 0 && id.Role != 99)
            {
                string[] column = { "Flags" };
                object[] val = { 1 };
                new irsUsers().updateUser(IRSClientData.UserId, column, val);
            }

            /* VAPT 20190401 */
            if (id.SuperAdmin)
            {
                return new Object[] { IRSClientData.UserId, IRSClientData.Location + " " + locdesc, bdt, IRSClientData.AppPath, 90, notifi };
            }
            else
            {
                return new Object[] { IRSClientData.UserId, IRSClientData.Location + " " + locdesc, bdt, IRSClientData.AppPath, IRSClientData.XDays, notifi };
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String getInetPageUrl()
        {
            DataTable dt = (DataTable)this.getMenuData();
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow[] rows = dt.Select("MenuName = 'Internet Transaction'");
                if (IRSClientData.Location == "USW")
                {
                    rows = dt.Select("MenuName = 'TDD Remittance'");
                }
                if (rows.Length > 0)
                {
                    String s = (String)rows[0]["ModuleUrl"];
                    if (s != null && s.Length > 0) return s;
                }
            }

            throw new Exception("Internet Transaction page is not valid.");
        }

 	    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool MFAEnabled()
        {
            return new Entrust().confirm_mfa(IRSClientData.Location);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool check_if_token_exist()
        {
            if (Session["token_validity"] != null)
            {
                if (!Convert.ToBoolean(Session["token_validity"]))
                {
                    //wipe out client data if token validity is false.
                    Session["IRS_CLIENT_DATA"] = null;
                    return false;
                }
                else if (Session["user_exist"] != null)
                {
                    if (!Convert.ToBoolean(Session["user_exist"]))
                    {
                        return false;
                    }
                }
                return true;
            }

            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string check_ip_response()
        {
            if (Session["IP_GEO_STAT"] != null)
            {
                return Session["IP_GEO_STAT"].ToString();
            }
            return string.Empty;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool enroll_entrust()
        {
            if (Session["IRS_CLIENT_DATA"] != null)
            {
                // new Entrust().RegisterDevice(IRSClientData.UserId);
                if (new Entrust().UserCreate(IRSClientData.UserId))
                {
                    return new Entrust().StartActivation(IRSClientData.UserId);
                    //Entrust().StoreMachineInformation(deviceInformation, IRSClientData.UserId);
                }
                Session["user_exist"] = true;

            }
            return false;

        }

    }
}
