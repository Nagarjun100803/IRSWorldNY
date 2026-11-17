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

namespace IRSWorld.APAS
{
    public partial class APASSubBranch : System.Web.UI.Page
    {
        private string _error = null;
        sqlHelperWs sql = new sqlHelperWs();

        private void set_error()
        {
            Session["ServerError"] = null;
            Session["ServerError"] = this._error;
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

        private irsAPAS objAPAS
        {
            get
            {
                if (Session["objAPASSubBranches"] == null)
                {
                    Session["objAPASSubBranches"] = new irsAPAS();
                }
                return (irsAPAS)Session["objAPASSubBranches"];
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(APASSubBranch));
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
        public DateTime get_time_zone()
        {
            return irsTimeZone.branchLocalTime(irsClientData.Location);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getListPartners()
        {
            return objAPAS.getListPartners();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getSubBranch(string partner_code)
        {
            return objAPAS.getSubBranch(partner_code);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getCurrencyPerPartner(string partner_code, string subbranch)
        {
            return objAPAS.getCurrencyPerPartner(partner_code, subbranch);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool isExisting(string brcode, int currency)
        {
            object o = sql.execScalar("SELECT Count(*) FROM TBL_PAYOUT_ALLOWED_CURRENCIES_SUB_BRANCH WHERE SubBranchCode='" + brcode + "' AND ISNULL(Deleted,0) != 1 AND CurrencyCode=" + currency);
            return (Convert.ToInt32(o) > 0) ? true : false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String do_process_apas(string instruction, string column_values, string trxn_id)
        {
            bool ibool = false;
            objAPAS.TableName = "TBL_PAYOUT_ALLOWED_CURRENCIES_SUB_BRANCH_TEMP";
            switch (instruction)
            {
                case "INSERT": ibool = objAPAS.insert_record(column_values); break;
                case "UPDATE": ibool = objAPAS.update_record(column_values, (Object)trxn_id); break;
            }
            if (!ibool)
            {
                this._error = objAPAS.messages;
                this.set_error();
            }
            return this._error;
        }

    }
}
