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
    public partial class APASPayoutPartnerFees : System.Web.UI.Page
    {
        private string _tbl_name = "tbl_payout_partner_fee_temp";
        //private string _param = null;
        //private string _error = null;
        private sqlHelperWs sql = new sqlHelperWs();

        private irsAPAS objAPAS
        {
            get
            {
                if (Session["partnerfees"] == null)
                {
                    Session["partnerfees"] = new irsAPAS(_tbl_name);
                }
                return (irsAPAS)Session["partnerfees"];
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

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(APASPayoutPartnerFees));
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool applyEdits(String adds, String dels, String eds, String brCode, string AddedBy)
        {
            bool deleted = false;
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
                        objAPAS.TableName = "tbl_payout_partner_fee_temp";
                        bool ok = objAPAS.partner_fee_setup(ht, brCode, deleted, isEdit, AddedBy, get_time_zone());
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

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet load_payout_partners()
        {
            DataSet ds = sql.getDataSet(sql.getSqlCommand("SELECT BranchCode,BranchName FROM vwParentBranchList", false));
            return ds;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DateTime get_time_zone()
        {
            return irsTimeZone.branchLocalTime(irsClientData.Location);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getPayoutCurrency()
        {
            return objAPAS.getPayoutCurrency();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable ViewPartnerFee(string brcode)
        {
            return objAPAS.ViewPartnerFee(brcode);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public bool has_pending(string branchcode, int currency,string feecode)
        {
            object o = sql.execScalar("SELECT count(*) FROM TBL_PAYOUT_PARTNER_FEE_TEMP WHERE BranchCode='" + branchcode + "' AND forApproval = 1 AND PayoutCurr = " + currency + " AND FeeCode = '"+ feecode +"'");
            return (Convert.ToInt32(o) > 0) ? true : false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public bool isExisting(string feecode)
        {
            object o = sql.execScalar("SELECT count(*) FROM TBL_PAYOUT_PARTNER_FEE WHERE FeeCode = '"+ feecode +"' AND ISNULL(Deleted,0) != 1");
            return (Convert.ToInt32(o) > 0) ? true : false;
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
    }
}
