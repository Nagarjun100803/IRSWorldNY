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

namespace IRSWorld.Admin
{
    public partial class Password : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Password));
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object[] verify_password(String userid, String pwd)
        {
            /* VAPT 20190326 */
            /*
            irsClientData id = (irsClientData)Session["IRS_CLIENT_DATA"];
            if (id.MFASuccess)
            {
                return new irsUsers().validate(userid, pwd);
            }
            else
            {
                return new irsUsers().validate_login(userid, pwd);
            }
            */

            /* DEC 2020 TO ADDRESS - CROSS SITE SCRIPTING REFLECTED */
            irsClientData id = null;

            try
            {
                id = (irsClientData)Session["IRS_CLIENT_DATA"];

                if (userid != null && userid != string.Empty)
                {
                    userid = Server.UrlEncode(userid);
                }

                if (id.MFASuccess)
                {
                    return new irsUsers().validate(userid, pwd);
                }
                else
                {
                    return new irsUsers().validate_login(userid, pwd);
                }
            }
            catch
            {
                return null;
            }
            /* DEC 2020 TO ADDRESS - CROSS SITE SCRIPTING REFLECTED */

        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object[] new_password(String userid, String new_pwd)
        {
            return new irsUsers().new_password(userid, new_pwd);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object[] change_password(String userid, String old_pwd, String new_pwd)
        {
            return new irsUsers().change_password(userid, old_pwd, new_pwd);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public bool is_from_login()
        {
            //return (Session["USER_ON_LOGIN"] != null | (Session["IRS_CLIENT_DATA"] != null && (((irsClientData)Session["IRS_CLIENT_DATA"]).UserId != null && ((irsClientData)Session["IRS_CLIENT_DATA"]).UserId.Length > 0)));
            /* VAPT IAG 20170516 */
            return (Session["VAPT_LOGIN"] != null | (Session["IRS_CLIENT_DATA"] != null && (((irsClientData)Session["IRS_CLIENT_DATA"]).UserId != null && ((irsClientData)Session["IRS_CLIENT_DATA"]).UserId.Length > 0)));
        }

        /* RFC-31394 - Privacy Violation: HTTP GET*/
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string retrieve_uid()
        {
            if (Session["xid"] != null)
            {
                return Convert.ToString(Session["xid"]);
            }
            return string.Empty;
        }
    }
}
