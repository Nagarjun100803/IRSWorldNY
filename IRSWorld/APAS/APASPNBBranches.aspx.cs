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
    public partial class APASPNBBranches : System.Web.UI.Page
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
                if (Session["objAPASPNBBranches"] == null)
                {
                    Session["objAPASPNBBranches"] = new irsAPAS();
                }
                return (irsAPAS)Session["objAPASPNBBranches"];
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(APASPNBBranches));
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getBranches()
        {
            return new irsBranchDefaults().GetBranches("");
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
        public DataTable getCurrencyList(string brcode)
        {
            DataSet ds = sql.execStoredProcedure("spGetAllowedCurrencies", new object[] { brcode });
            return ds.Tables[0];
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool isExisting(string brcode,int currency)
        {
            object o = sql.execScalar("SELECT Count(*) FROM TBL_PAYOUT_ALLOWED_CURRENCIES WHERE BranchCode='" + brcode + "' AND ISNULL(Deleted,0) != 1 AND Currency=" + currency );
            return (Convert.ToInt32(o)>0) ? true : false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool haspending(string brcode,int currency)
        {
            object o = sql.execScalar("SELECT Count(*) FROM TBL_PAYOUT_ALLOWED_CURRENCIES_TEMP WHERE BranchCode='"+ brcode +"' AND Currency="+ currency +" AND isForApproval = 1" );
            return (Convert.ToInt32(o) > 0) ? true : false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String do_process_apas(string instruction, string column_values, string trxn_id)
        {
            bool ibool = false;
            objAPAS.TableName = "TBL_PAYOUT_ALLOWED_CURRENCIES_TEMP";
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

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DateTime get_time_zone()
        {
            return irsTimeZone.branchLocalTime(irsClientData.Location);
        }

    }
}
