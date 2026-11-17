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
    public partial class APASMain_Apprvr : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(APASMain_Apprvr));
        }

        #region  PRIVATE METHODS, VARIABLES, PROPERTIES
        private string _tbl_name = "v_APAS_trxn";
        private string _param = null;
        private string _error = null;

        private irsIRIS doAPAS
        {
            get
            {
                if (Session["APAS"] == null)
                {
                    Session["APAS"] = new irsIRIS();
                }
                return (irsIRIS)Session["APAS"];
            }
        }
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
        
        private void irs_clientdata(Object[] data)
        {
            System.Web.HttpContext.Current.Session["CLIENTDATA"] = data;            
        }

        #endregion
        #region PUBLIC METHODS, PROPERTIES

        #endregion

        #region PUBLIC AJAX METHOD

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object clientdata()
        { return (Object)System.Web.HttpContext.Current.Session["CLIENTDATA"]; }
        
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object[] verify_password(String userid, String pwd)
        {
            string brcode   = "";
            int tries       = 1;
            string location = "";
            short role      = 0;
            string approver = "";
            string user     = "";
            
            irsUsers iusr = new irsUsers();
            Object[] oo = iusr.validate(userid, pwd, Convert.ToByte(tries), true);
            bool ok = (bool)oo[0];
            if (ok)
            {
                brcode = iusr.getColumn("Location").ToString();
                role = Convert.ToInt16(iusr.getColumn("Role"));
                if (irsClientData.Location != brcode)
                {
                    oo[0] = false;
                    oo[1] = "The user is not within the location.";
                }
                else
                {
                    if (role != 4 & role != 6 & role != 12 & role != 8)//approver
                    {
                        oo[0] = false;
                        oo[1] = "The users role is not a member of allowed approver.";
                    }                    
                }
            }

            approver        = userid; 
            location        = irsClientData.Location;
            user            = irsClientData.UserId;
            DateTime _date  = irsTimeZone.branchLocalTime(location);

            Object[] o = new Object[] { oo, brcode, role, approver, user, _date};
            this.irs_clientdata(o);
            return o;
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
            { this._error = objAPAS.messages; this.set_error(); }
            return this._error;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String get_irsclient_data(string param)
        {
            string s      = null;
            switch (param)
            {
                case "UID": s       = irsClientData.UserId;     break;
                case "CIF": s       = irsClientData.CIFID;      break;
                case "LOCATION": s  = irsClientData.Location;   break;
                case "IP": s        = irsClientData.IPAddress;  break;
            }
            return s;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DateTime get_time_zone()
        {
            return irsTimeZone.branchLocalTime(irsClientData.Location);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_details(string param)
        {
            try
            {
                this._param = param;
                DataSet ds = new DataSet();
                doAPAS.table_name = _tbl_name;
                doAPAS.param = _param;
                ds = doAPAS.getDataSet();
                if (ds != null)
                { return ds; }               
            }
            catch{}
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_apas_trxnstat()
        {
            try
            {   
                DataSet ds = new DataSet();
                doAPAS.table_name = "TBL_LOOKUP";
                doAPAS.param = " where FldGroup = 'APASTransactionStatus'";
                ds = doAPAS.getDataSet();
                if (ds != null)
                { return ds;}                
            }
            catch{}
            return null;
        }
        #endregion
    }
}
