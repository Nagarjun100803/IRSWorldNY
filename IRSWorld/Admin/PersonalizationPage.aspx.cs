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
using Bharosa.VCrypt.Common.Util;
using IRSWorld.bharosa;
using IRSClassLib;

namespace IRSWorld.Admin
{

    public partial class PersonalizationPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BharosaTrace.Enter("PersonalizationPage.Page_Load()");

            AppSessionData sessionData = AppSessionData.GetInstance(Session);

            if ("true".Equals(sessionData["RegistrationOptional"]))
            {
                //this.btnSkip.Visible = true;
                this.skipPlaceholder.Visible = true;
            }
            else
            {
                //this.btnSkip.Visible = false;
                this.skipPlaceholder.Visible = false;
            }

            BharosaTrace.Exit("PersonalizationPage.Page_Load()");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //
            // Redirect the user to the page where he can register 
            // the image and phrase for the authenticator
            //
            Response.Redirect(BharosaConfig.get("register.image.phrase.page.url", "RegisterImagePhrase.aspx"));
        }

        protected void btnSkip_Click(object sender, EventArgs e)
        {
            //
            // Allow the user to skip personalization and 
            // redirect to the success page
            //
            //Response.Redirect(BharosaConfig.get("success.page.url", "Success.aspx"));
            Session["SHOW_IMAGE_PHRASE"] = null;
            new csControl().execScript(this.Page, "doOther", "goto_main();");
        }
    }
}
