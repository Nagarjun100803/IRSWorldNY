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
    public partial class APASLockedTransaction : System.Web.UI.Page
    {
        //private string _tbl_name = "v_APAS_trxn";
        //private string _param = null;
        private string _error = null;

        private irsAPAS objAPAS
        {
            get
            {
                if (Session["objAPAS"] == null)
                {
                    Session["objAPAS"] = new irsAPAS();
                }
                return (irsAPAS)Session["objAPAS"];
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

        private irsAPASTransactionStatus objAPASStat
        {
            get
            {
                if (Session["objAPASStat"] == null)
                {
                    Session["objAPASStat"] = new irsAPASTransactionStatus();
                }
                return (irsAPASTransactionStatus)Session["objAPASStat"];
            }
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(APASLockedTransaction));
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getLockStatus(string branchcode)
        {
            return objAPASStat.getAPASbyTransaction(5, branchcode,0,"",(branchcode == this.ordbranchcode()) ? true : false,""); // 5 is equivalent to locked transaction
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getAPASInfo(string refno, int status)
        {
            return objAPASStat.getAPASInfo(refno,status);
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
        public String do_process_apas(string instruction, string column_values, string trxn_id)
        {
            bool ibool = false;
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

        private void set_error()
        {
            Session["ServerError"] = null;
            Session["ServerError"] = this._error;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string ordbranchcode()
        {
            return new AppSettingsReader().GetValue("ORDBranchCode", Type.GetType("System.String")).ToString();
        }


    }
}
