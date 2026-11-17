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
using System.Text;

namespace IRSWorld.APAS
{
    public partial class APASPayoutPartnerSetup : System.Web.UI.Page
    {
        private string _tbl_name = "tbl_payout_partner_setup";
        //private string _param = null;
        private string _error = null;
        sqlHelperWs sql = new sqlHelperWs();

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(APASPayoutPartnerSetup));
        }

        private irsAPAS objAPAS
        {
            get
            {
                if (Session["partnersetup"] == null)
                {
                    Session["partnersetup"] = new irsAPAS(_tbl_name);
                }
                return (irsAPAS)Session["partnersetup"];
            }
        }

        private irsTimeZone irsTimeZone
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["IRS_TIME_ZONE"] == null)
                {
                    System.Web.HttpContext.Current.Session["IRS_TIME_ZONE"] = new irsTimeZone();
                }
                return (irsTimeZone)System.Web.HttpContext.Current.Session["IRS_TIME_ZONE"];
            }
        }

        private irsClientData irsClientData
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] == null)
                {
                    System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] = new irsClientData();
                }
                return (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String do_process_apas(string instruction, string column_values, string seq_id)
        {
            bool ibool = false;
            switch (instruction)
            {
                case "INSERT": ibool = objAPAS.insert_record(column_values); break;
                case "UPDATE": ibool = objAPAS.update_record(column_values, (Object)seq_id); break;
            }
            if (!ibool)
            {
                this._error = objAPAS.messages;
                this.set_error();
            }
            return this._error;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable viewSetup(string brcode)
        {
            return objAPAS.ViewSetup(brcode);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getPayoutCurrency(string branchcode)
        {
            return objAPAS.getPayoutCurrency(branchcode);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public Object getFs()
        {
            String root = Server.MapPath("~");
            Object[] retval = objAPAS.getFs(root);
            if (retval == null) throw new Exception(objAPAS.Message);
            return new Object[] { root, retval };
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet load_payout_partners(bool head_office)
        {
            string sqlcmd = "SELECT BranchCode,BranchName FROM vwParentBranchList"; //(head_office) ? "SELECT BranchCode,BranchName FROM TBL_BRANCHES WHERE BranchType in (4,7) And Classification=0" : "SELECT BranchCode,BranchName FROM TBL_BRANCHES WHERE BranchType in (4,7) And Classification=1";
            DataSet ds = sql.getDataSet(sql.getSqlCommand(sqlcmd, false));
            return ds;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet load_sub_branch(string parent_branch)
        {
            string sqlcmd = "SELECT BranchCode,BranchName FROM vwSubBranch Where MotherBranchCode = '"+ parent_branch +"'";
            DataSet ds = sql.getDataSet(sql.getSqlCommand(sqlcmd, false));
            return ds;
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public bool has_pending(string branchcode,int currency)
        {
            object o = sql.execScalar("SELECT count(*) FROM TBL_PAYOUT_PARTNER_SETUP_TEMP WHERE BranchCode='" + branchcode + "' AND forApproval = 1 AND Payout_curr = "+ currency +"");
            return (Convert.ToInt32(o) > 0) ? true : false;
        }

        private void set_error()
        {
            Session["ServerError"] = null;
            Session["ServerError"] = this._error;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DateTime get_time_zone()
        {
            return irsTimeZone.branchLocalTime(irsClientData.Location);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool applyEdits(String adds, String dels, String eds, String brCode, Boolean deleted,string AddedBy,string logo)
        {

            char sep = '^';
            bool isEdit = false;
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
                            {
                                ht.Add(cols[0], cols[1]);
                            }
                        }
                        if (tt[0] == "delete") { deleted = true; } else { deleted = false; }
                        if (tt[0] == "edit") { isEdit = true; }
                        objAPAS.TableName = "tbl_payout_partner_setup_temp";
                        bool ok = objAPAS.payout_setup(ht, brCode, deleted, isEdit, AddedBy, get_time_zone(),logo);
                        if (!ok)
                        {
                            throw new Exception(objAPAS.Message);
                        }
                        //if (dels.Length > 0) { bool ok = irsHolidays.delete_holidays(ht); }
                        //else { bool ok = irsHolidays.save_holidays(ht,brCode); }
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

            if (!objAPAS.saveUpdates())
            {
                throw new Exception("Save changes: " + objAPAS.Message);
            }

            return true;
            
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String get_irsclient_data(string param)
        {
            string s = null;
            switch (param)
            {
                case "UID": s = irsClientData.UserId; break;
                case "CIF": s = irsClientData.CIFID; break;
                case "LOCATION": s = irsClientData.Location; break;
                case "IP": s = irsClientData.IPAddress; break;
            }
            return s;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool check_account_number(string accountno)
        {
            return objAPAS.check_account_number(accountno);
        }

    }
}
