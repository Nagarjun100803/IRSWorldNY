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
    public partial class Override : System.Web.UI.Page
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
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Override));
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object[] verify_password(String userid, String pwd, int chid, int tries)
        {
            string brcode = "";
            short role = 0;
            irsUsers iusr = new irsUsers();
            Object[] oo = iusr.validate(userid, pwd, Convert.ToByte(tries), true);
            bool ok = (bool)oo[0];
            if (!ok)
            {
                short iflag = Convert.ToInt16(oo[4]);
                if (iflag == iusr.FLAG_EXPIRED)
                {
                    ok = true;
                    oo[0] = true;
                }
            }

            if (ok)
            {
                brcode = iusr.getColumn("Location").ToString();
                role = Convert.ToInt16(iusr.getColumn("Role"));
                if (IRSClientData.Location != brcode)
                {
                    oo[0] = false;
                    oo[1] = "The user is not within the location.";
                }
                else
                {
                    bool is_over = new irsChargesUsers((short) 2).isOverrider(chid, role);
                    if (!is_over)
                    {
                        oo[0] = false;
                        oo[1] = "The users role is not a member of allowed overriders.";
                    }
                }
            }

            return new Object[] { oo, brcode, role };
        }
    }
}
