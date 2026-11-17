using System;
using System.Data;
using System.Data.SqlClient;
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

namespace IRSWorld.Cash
{
    public partial class TellerCashManagement : System.Web.UI.Page
    {

        private irsTellerCashManagement IRSTellerCashManagement
        {
            get
            {
                if (Session["CASH_MGT"] == null)
                {
                    Session["CASH_MGT"] = new irsTellerCashManagement();
                }
                return (irsTellerCashManagement)Session["CASH_MGT"];
            }
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object getCashMgt(string fldCode)
        {
            DataTable dt = IRSTellerCashManagement.getCashMgt(fldCode);


            if (dt == null)
            {
                throw new Exception("Get Modules Failed: " + IRSTellerCashManagement.Message);
            }

            return dt;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(TellerCashManagement));
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void close_session()
        {
            Session.Abandon();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public void drop_irsCashMgt()
        {
            Session["CASH_MGT"] = null;
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
                        bool ok = IRSTellerCashManagement.applyUpdates(crud, row, ht);
                        if (!ok) errs.Add(crud + " Row: " + row + " - " + IRSTellerCashManagement.Message);
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

            if (!IRSTellerCashManagement.saveUpdates())
            {
                throw new Exception("Save changes: " + IRSTellerCashManagement.Message);
            }

            return true;
        }


        //load ddl

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getdropdown_ddlCurrency()
        {
            sqlHelper commons = new sqlHelper();
            SqlCommand sqcommands = null;
            try
            {

                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_cm_Currency");
                    return commons.getDataSet(sqcommands, "dtcmCurrency");
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }

            return null;
        }


        protected void ddlCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
       

        }


    }
}
