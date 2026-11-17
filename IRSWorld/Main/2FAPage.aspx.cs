using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services.Protocols;
using IRSClassLib;
using System.Text;

namespace IRSWorld.Main
{
    public partial class _2FAPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(_2FAPage));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            /* VAPT 20180723 */
            irsClientData id = (irsClientData)Session["IRS_CLIENT_DATA"];
            id.MFAScreen = true;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool validate_log(string token, string machineInformation)
        {
            string userid = ((irsClientData)Session["IRS_CLIENT_DATA"]).UserId;
            bool _reply = new Entrust().checktoken(userid, machineInformation, token);

            /* VAPT 20180723 */
            irsClientData id = (irsClientData)Session["IRS_CLIENT_DATA"];
            id.MFASuccess = false;

            if (_reply == true)
            {
                Session["token_validity"] = _reply;
                /* VAPT 20180723 */
                id.MFASuccess = true;
                new csAuditLog().writeVAPTLogger(userid, "MFA", "2FA AUTHENTICATION", "USER WAS ABLE TO PROVIDE THE CORRECT TOKEN", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Information);
                return true;
            }
            else
            {
                new csAuditLog().writeVAPTLogger(userid, "MFA", "2FA AUTHENTICATION", "USER WAS NOT ABLE TO PROVIDE THE CORRECT TOKEN", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Alert);
                return false;
            }
        }
    }
}