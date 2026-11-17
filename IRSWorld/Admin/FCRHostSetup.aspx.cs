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
    public partial class FCRHostSetup : System.Web.UI.Page
    {
        private irsFCRSettings IRSFCRSettings
        {
            get
            {
                if (Session["IRS_FCR_SETUP"] == null)
                {
                    Session["IRS_FCR_SETUP"] = new irsFCRSettings();
                }
                return (irsFCRSettings)Session["IRS_FCR_SETUP"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(FCRHostSetup));
            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool deleteSettings(String setting_id)
        {
            if (!IRSFCRSettings.deleteSettings(setting_id))
            {
                throw new Exception("Delete Failed. " + IRSFCRSettings.Message);
            }

            return true;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object[] getInitData()
        {
            DataTable dtddl = IRSFCRSettings.getDdlData();
            DataTable dtdef = null;
            if (dtddl.Rows.Count > 0)
            {
                dtdef = IRSFCRSettings.getSettingsTable(dtddl.Rows[0][0].ToString());
            }

            if (dtdef != null) return new Object[] { dtddl, dtdef };

            throw new Exception("Error obtaining initial data.");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getSettings(String setting_id)
        {
            DataTable dt = IRSFCRSettings.getSettingsTable(setting_id);
            if (dt != null) return dt;

            throw new Exception (IRSFCRSettings.Message);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void close_session()
        {
            Session.Abandon();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void drop_fcrsetup()
        {
            Session["IRS_FCR_SETUP"] = null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool applyEdits(String adds, String dels, String eds)
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
                        bool ok = IRSFCRSettings.applyUpdates(crud, row, ht);
                        if (!ok) errs.Add(crud + " Row: " + row + " - " + IRSFCRSettings.Message);
                    }
                }
            }
            if (errs.Count > 0)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                foreach (Object msg in errs)
                {
                    sb.Append(msg.ToString() + "\n");
                }
                throw new Exception(sb.ToString());
            }

            return true;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable saveUpdates(String adds, String dels, String eds)
        {
            if (applyEdits(adds, dels, eds))
            {
                DataTable dt = IRSFCRSettings.saveUpdates();
                if (dt == null)
                {
                    throw new Exception("Save changes: " + IRSFCRSettings.Message);
                }
                return dt;
            }

            return null;
        }

    }
}
