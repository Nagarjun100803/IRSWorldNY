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

namespace IRSWorld.BranchSetup
{
    public partial class BranchSetupMain : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(BranchSetupMain));
            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object getMenuData(Object[] items)
        {
            ArrayList al = new ArrayList();
            if (Session["IRS_CLIENT_DATA"] != null)
            {
                irsClientData id = (irsClientData) Session["IRS_CLIENT_DATA"];
                if (id.UserId != null && id.UserId.Length > 0)
                {
                    DataTable dt = new irsMenu().getMenuView(id.UserId);
                    for (int i = 0; i < items.Length; i++)
                    {
                        String s = items[i].ToString();
                        try
                        {
                            String[] ss = s.Split('/');
                            if (ss.Length > 1)
                            {
                                String mname = ss[1].Substring(0, ss[1].IndexOf("."));
                                if (mname != null && mname.Length > 0)
                                {
                                    DataRow[] rr = dt.Select("ModuleUrl like '%" + mname + "." + "%'");
                                    if (rr.Length > 0)
                                    {
                                        if (rr[0]["ModuleUrl"].ToString().IndexOf("BranchSetup") > -1)
                                        {
                                            //"[1,0,\"Branch Code Table\",\"javascript:loadframe(\\\"./Branch_Definition.aspx\\\");\"]"
                                            String[] tt = s.Split(',');
                                            String t = tt[0].Substring(tt[0].IndexOf("[") + 1);
                                            al.Add(int.Parse(t));
                                        }
                                    }
                                }
                            }
                        }
                        catch //(Exception e) // commented to remove warning
                        {}
                    }

                    return al;
                }
            }
            return null;
        }

    }
}
