using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using IRSClassLib;
namespace IRSWorld.Admin
{
    public partial class RegisterDevice : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(RegisterDevice));
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public object[] RegistrationData()
        {

            byte[] b = (byte[])Session["QRCode"];
            string qrpass = (string)Session["QRCodePassword"];
            string serialno = (string)Session["SerialNo"];
            string qrlink = (string)Session["QRLink"];
            string folder_directory = Server.MapPath(@"..\QRImage\");
            if (!Directory.Exists(folder_directory))
            {
                Directory.CreateDirectory(folder_directory);
            }

            File.WriteAllBytes(folder_directory + serialno + ".jpg", b);
            return new object[] { b, qrpass, serialno, @"..\QRImage\" + serialno + ".jpg" };

        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool validate_token_listener()
        {
            irsClientData id = (IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"];
            return new Entrust().tokenactivation(id.UserId);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool complete_registration(string registrationcode)
        {

            irsClientData id = (IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"];
            return new Entrust().CompleteActivation(id.UserId, registrationcode);

        }

    }
}