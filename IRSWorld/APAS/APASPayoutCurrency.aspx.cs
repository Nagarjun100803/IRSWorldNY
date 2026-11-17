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
    public partial class APASPayoutCurrency : System.Web.UI.Page
    {
        private string _tbl_name = "tbl_payout_currency_temp";
        //private string _param = null;
        private string _error = null;
        sqlHelperWs sql = new sqlHelperWs();


        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(APASPayoutCurrency));
        }

        private irsAPAS objAPAS
        {
            get
            {
                if (Session["payoutcurrencies"] == null)
                {
                    Session["payoutcurrencies"] = new irsAPAS(_tbl_name);
                }
                return (irsAPAS)Session["payoutcurrencies"];
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
        public bool have_pending_for_approval(int currency)
        {
            object o = sql.execScalar("SELECT count(*) FROM TBL_PAYOUT_CURRENCY_TEMP WHERE CURRENCY = " + currency + " AND IsForApproval = 1 AND ISNULL(ISCANCELLED,0) != 1");
            return (Convert.ToInt32(o) > 0) ? true : false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet load_currencies(string x)
        {
            
            DataSet ds =  sql.getDataSet(sql.getSqlCommand("SELECT fldvalue,fldcode + ' - ' + flddescription AS Description from tbl_lookup where fldgroup = 'Currency' ",false));
            return ds;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getCount(int value,int moduleid)
        {
            DataSet ds = sql.getDataSet(sql.getSqlCommand("SELECT * FROM TBL_PAYOUT_CURRENCY WHERE Currency='" + value + "' And ModuleId = "+ moduleid +"",false));
            return ds.Tables[0];
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool isPreviouslyCancelled(int currency, string accountnum)
        {
            object o = sql.execScalar("SELECT isCancelled FROM TBL_PAYOUT_CURRENCY_TEMP WHERE Currency= " + currency + " AND ORDAccountNo = '" + accountnum + "'");
            return (Convert.ToInt32(o) > 0) ? true : false;
        }

        

        private void set_error()
        {
            Session["ServerError"] = null;
            Session["ServerError"] = this._error;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DateTime get_branch_localtime(string brcode)
        {
            DateTime dt = new irsTimeZone().branchLocalTime(brcode);
            return dt;
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
