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
    public partial class APASApprovalWindow : System.Web.UI.Page
    {
        private string _error = null;

        private irsAPASApprovalWindow objApproval
        {
            get
            {
                if (Session["objApproval"] == null)
                {
                    Session["objApproval"] = new irsAPASApprovalWindow();
                }
                return (irsAPASApprovalWindow)Session["objApproval"];
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
                if (Session["objAPAS2"] == null)
                {
                    Session["objAPAS2"] = new irsAPAS();
                }
                return (irsAPAS)Session["objAPAS2"];
            }
        }

        private void set_error()
        {
            Session["ServerError"] = null;
            Session["ServerError"] = this._error;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(APASApprovalWindow));
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getTables(int tbl_indx)
        {
            return objApproval.getTables(tbl_indx);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String do_process_apas(string instruction, string column_values, string trxn_id,string tbl_indx)
        {
            bool ibool = false;
            get_table_by_indx(tbl_indx, trxn_id);
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

        private void get_table_by_indx(string tbl_indx,string seq_id)
        {
            switch (tbl_indx)
            {
                case "1":
                    objAPAS.TableName = "TBL_PAYOUT_PARTNER_SETUP_TEMP";
                    objAPAS.getUpdateDS("SELECT * FROM TBL_PAYOUT_PARTNER_SETUP_TEMP WHERE SEQ_ID=" + seq_id); break;
                case "2":
                    objAPAS.TableName = "TBL_PAYOUT_CURRENCY_TEMP";
                    objAPAS.getUpdateDS("SELECT * FROM TBL_PAYOUT_CURRENCY_TEMP WHERE SEQ_ID=" + seq_id); break;
                case "3":
                    objAPAS.TableName = "TBL_PAYOUT_ALLOWED_CURRENCIES_TEMP";
                    objAPAS.getUpdateDS("SELECT * FROM TBL_PAYOUT_ALLOWED_CURRENCIES_TEMP WHERE SEQ_ID=" + seq_id); break;
                case "4":
                    objAPAS.TableName = "TBL_PAYOUT_PARTNER_FEE_TEMP";
                    objAPAS.getUpdateDS("SELECT * FROM TBL_PAYOUT_PARTNER_FEE_TEMP WHERE SEQ_ID=" + seq_id); break;
                case "5":
                    objAPAS.TableName = "TBL_PAYOUT_ALLOWED_CURRENCIES_SUB_BRANCH_TEMP";
                    objAPAS.getUpdateDS("SELECT * FROM TBL_PAYOUT_ALLOWED_CURRENCIES_SUB_BRANCH_TEMP WHERE SEQ_ID=" + seq_id); break;

            }
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


       
    }
}
