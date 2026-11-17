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
using IRSWorld.TranClass;
namespace IRSWorld.BranchSetup
{
    public partial class ReportsLookup : System.Web.UI.Page
    {
        #region VARIABLES
        private string tempbranchcode = "LDN";
        private string temprole = "1";
        #endregion
        private irsReports IRSReports
        {
            get
            {
                if (Session["IRS_Reports"] == null)
                {
                    Session["IRS_Reports"] = new irsReports();
                }
                return (irsReports)Session["IRS_Reports"];
            }
        }

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
            AjaxPro.Utility.RegisterTypeForAjax(typeof(ReportsLookup));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(IRSWorld.Admin.Modules));
            //this.txtbranchcode.Value = IRSClientData.Location; 

            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);
        }
        #region "AJAX PUBLIC METHOD"
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataTable load_sp_parameter(string spname)
        {
            DataTable  dt = new DataTable();
            try
            {
                //ds = IRSReports.load_report_group_lookup(IRSClientData.Location ,IRSClientData.Role.ToString() );
                dt = IRSReports.get_sp_params(spname);
                return dt;
            }
            catch
            {
                return null;
            }

        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet load_rep_group()
        {
            DataSet ds = new DataSet();
            try
            {
                //ds = IRSReports.load_report_group_lookup(IRSClientData.Location ,IRSClientData.Role.ToString() );
                ds = IRSReports.load_report_maingroup("REPTYPE");
                return ds;
            }
            catch
            {
                return null;
            }

        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet load_rep_all()
        {
            DataSet ds = new DataSet();
            try
            {
                //ds = IRSReports.load_report_group_lookup_per_branch(subgroupid, IRSClientData.Location);
                ds = IRSReports.load_report_maingroup("REPORTS");
                return ds;
            }
            catch
            {
                return null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet load_rep_group_per_branch(string subgroupid)
        {
            DataSet ds = new DataSet();
            try
            {
                //ds = IRSReports.load_report_group_lookup_per_branch(subgroupid, IRSClientData.Location);
                ds = IRSReports.load_report_group_lookup_per_branch(subgroupid, this.tempbranchcode, this.temprole);
                return ds;
            }
            catch
            {
                return null;
            }
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet load_rep_per_branch(string subgroupid)
        {
            DataSet ds = new DataSet();
            try
            {
                //ds = IRSReports.load_report_group_lookup_per_branch(subgroupid, IRSClientData.Location);
                ds = IRSReports.load_report_per_branch(subgroupid);
                return ds;
            }
            catch
            {
                return null;
            }

        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public bool check_reportentry(string reportname)
        {
            DataTable dt;
            
            try
            {
                //brl = new irsBranchLookup();
                //ds = IRSReports.load_report_group_lookup_per_branch(subgroupid, IRSClientData.Location);
                dt = IRSReports.check_report(reportname);

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        return true;
                    }

                }
                return false;
                
            }
            catch
            {
                return false;
            }
            finally
            {
                
            }

        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet load_branch(string branchcode)
        {
            DataSet ds = new DataSet();
            irsBranchLookup brl = null;
            try
            {
                brl = new irsBranchLookup();
                //ds = IRSReports.load_report_group_lookup_per_branch(subgroupid, IRSClientData.Location);
                ds = brl.get_branches(branchcode);
                return ds;
            }
            catch
            {
                return null;
            }
            finally
            {
                brl = null;
            }

        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public Object applyEdits(String adds, String dels, String eds,String  subgroupid)
        {
            char sep = '^';
            ArrayList errs = new ArrayList();
            String[] ss = { adds, dels, eds };            
            int norec = 0;
            foreach (String s in ss)
            {
                if (s.Length > 0)
                {
                    String[] tt = s.Split('~');
                    String crud = tt[0].ToUpper();
                    for (int i = 1; i < tt.Length; i++)
                    {

                        String[] uu = tt[i].Split('|');
                        if ((uu[0].ToString().Length == 0))
                        {
                            norec++;
                        }
                        else
                        {
                            int row = int.Parse(uu[0].Replace(",", ""));
                            Hashtable ht = new Hashtable();
                            for (int j = 1; j < (uu.Length - norec); j++)
                            {

                                String[] cols = uu[j].Split(sep);
                                if (cols.Length < 2)
                                {
                                    //ht.Add(cols[0], "");
                                    continue;
                                }

                                ht.Add(cols[0], cols[1]);
                            }

                            
                            bool ok = IRSReports.applyUpdates(crud, row, ht);
                            if (!ok) errs.Add(crud + " Row: " + row + " - " + IRSReports.Message);
                        }
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

            return IRSReports.saveUpdates(subgroupid);
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public Object getFs()
        {
            String root = Server.MapPath("~/Reports");
            Object[] retval = IRSReports.getFs(root);
            if (retval == null) throw new Exception(IRSReports.Message);            
            return new Object[] { root, retval };
        }
        
        #endregion

    }
}
