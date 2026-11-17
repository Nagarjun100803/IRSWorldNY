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
namespace IRSWorld.APAS
{
    public partial class APASTransactionStatus : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(APASTransactionStatus));
        }

        private irsAPASTransactionStatus irsAPAS_Trnx
        {
            get
            {
                if (Session["irsAPAS_Trnx"] == null)
                {
                    Session["irsAPAS_Trnx"] = new irsAPASTransactionStatus();
                }
                return (irsAPASTransactionStatus)Session["irsAPAS_Trnx"];
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
        public DataTable getAPASbyTransaction(int tranxcode,string branchcode,int cifclass,string whereclause)
        {
            string ord_branch = new AppSettingsReader().GetValue("ORDBranchCode", Type.GetType("System.String")).ToString();
            return irsAPAS_Trnx.getAPASbyTransaction(tranxcode, branchcode, cifclass, whereclause, (ord_branch == branchcode) ? true : false, get_irsclient_data("UID"));
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getAPASInfo(string refno,int status)
        {
            return irsAPAS_Trnx.getAPASInfo(refno,status);
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
                case "RoleID": s = irsClientData.Role.ToString(); break;
            }
            return s;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string CIFClass(string cifid)
        {
            return irsAPAS_Trnx.CIFClass(cifid);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void ShowReport(string refno)
        {
            string report_path = Server.MapPath("../Reports/APAS/APASReceipt.rpt");
            try
            {
                DataSet ds = objAPAS.getAPASReceipt(refno);
                irsReports _report = new irsReports(ds.Tables[0], report_path, true);
                //_report.display_report();

            }
            catch (Exception ex)
            {
                objAPAS.Message = ex.Message;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getFilter()
        {
            return irsAPAS_Trnx.getFilter();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string isAgent()
        {
            return new AppSettingsReader().GetValue("Agent", Type.GetType("System.String")).ToString();
        }


    }
}
