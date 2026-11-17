using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using IRSClassLib;

namespace IRSWorld.Admin
{
    public partial class Modules : System.Web.UI.Page
    {

        private irsModules IRSModules
        {
            get
            {
                if (Session["IRS_MODULES"] == null)
                {
                    Session["IRS_MODULES"] = new irsModules();
                }
                return (irsModules)Session["IRS_MODULES"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Modules));
            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void close_session()
        {
            Session.Abandon();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void drop_irsmodules()
        {
            Session["IRS_MODULES"] = null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object[] getIrsModules()
        {
            DataTable dt = IRSModules.getModules();
            if (dt == null)
            {
                throw new Exception("Get Modules Failed: " + IRSModules.Message);
            }

            return new Object[] { dt, IRSModules.dataset.HasChanges() | IRSModules.isUpdating };
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public Object getFs()
        {
            String root = Server.MapPath("~");
            Object[] retval = IRSModules.getFs(root);
            if (retval == null) throw new Exception(IRSModules.Message);
            return new Object[] { root, retval };
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool applyPost(String adds, String dels, String eds)
        {
            char sep = '^';
            ArrayList errs = new ArrayList();
            String[] ss = { adds, dels, eds };
            foreach (String s in ss)
            {
                if (s.Length > 0)
                {
                    String[] tt = s.Split('~');
                    String crud = tt[0].ToUpper();
                    for (int i = 1; i < tt.Length; i++)
                    {
                        String[] uu = tt[i].Split('|');
                        int row = int.Parse(uu[0]);
                        Hashtable ht = new Hashtable();
                        for (int j = 1; j < uu.Length; j++)
                        {
                            String[] cols = uu[j].Split(sep);
                            if (cols.Length < 2) continue;
                            ht.Add(cols[0], cols[1]);
                        }
                        bool ok = IRSModules.applyUpdates(crud, row, ht);
                        if (!ok) errs.Add(crud + " Row: " + row + " - " + IRSModules.Message);
                    }
                }
            }
            if (errs.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Object msg in errs)
                {
                    sb.Append(msg.ToString() + "\n");
                }
                throw new Exception(sb.ToString());
            }

            return true;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool saveUpdates(String adds, String dels, String eds)
        {
            if (applyPost(adds, dels, eds))
            {
                if (!IRSModules.saveUpdates())
                {
                    throw new Exception("Save changes: " + IRSModules.Message);
                }
                return true;
            }

            return false;
        }

        //=======================================================================
        //RFC-33165 : Phase 2_IRSW Maker-Checker for Parameter Admin Function
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool ParamApproverFlag()
        {
            irsModules module_approval = new irsModules();

            try
            {
                return module_approval.ParamApproverFlag();
            }
            catch (Exception e) { }
            finally {
                module_approval = null;
            }
            return false;
        }
        //=======================================================================
    }
}
