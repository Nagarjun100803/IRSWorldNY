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



namespace IRSWorld.Report_Profiler
{
    public partial class CIF_profile_create : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(CIF_profile_create));
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getCIFClass()
        {
            return new irsCIFProfiler().getCIFClass();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getCIFBranch()
        {
            return new irsCIFProfiler().getCIFBranch();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getCIFType()
        {
            return new irsCIFProfiler().getCIFType();
            
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getCIFAccountType()
        {
            return new irsCIFProfiler().getCIFAcccountType();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getCIFProfiles()
        {
            return new irsCIFProfiler().getCIFProfiles();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getCIFFields()
        {
            return new irsCIFProfiler().loadCIFFields();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getCIFAcctCcy ()
        {
            return new irsCIFProfiler().getCIFAcctCcy();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getCIFDates()
        {
            return new irsCIFProfiler().getCIFDates();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getCIFAcctStatus()
        {
            return new irsCIFProfiler().getCIFAcctStatus();
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getCIFMerchs()
        {
            return new irsCIFProfiler().getCIFMerchs();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable parseQuery(string top,string idNo, string guid, string cifName, string acctNo, string acctType,string acctStatus,string acctCcy,string [] acctBranch,
                                    string cifClass, string[] cifType, string dateFilter,string dateFrom, string dateTo, string[] cifBranch, string[] selectField,
                                        bool chkAllCIFBranch, bool chkAllCIFType,bool chkAllAcctBranch,
                                        string remInputId,string merchMemID,string merchProdCode)
        {
            DataTable table = new irsCIFProfiler().parseQuery(top, idNo, guid, cifName, acctNo, acctType,acctStatus,acctCcy,acctBranch,
                                        cifClass, cifType, dateFilter,dateFrom, dateTo,
                                         cifBranch, selectField, chkAllCIFBranch, chkAllCIFType, chkAllAcctBranch, remInputId,
                                          merchMemID,merchProdCode);
            Session["DATA_SOURCE_DB"] = table;
            return table;
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String ExtractFile(string profileName)
        {
            string report = new irsCIFProfiler().ExtractFile(profileName);
            Session["DL_REPORT_FILE_NAME"] = report.Substring(18).Trim();
            return report;
            
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool isAvailable(string profileName)
        {
            return new irsCIFProfiler().isAvailable(profileName);
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable loadProfile(string profileName)
        {
            return new irsCIFProfiler().loadProfile(profileName);
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string saveProfile(string top, string idNo, string guid, string cifName, string acctNo, string acctType, string acctStatus, string acctCcy, string[] acctBranch,
                                    string cifClass, string[] cifType, string dateFilter, string dateFrom, string dateTo, string[] cifBranch, string[] selectText,
                                        bool chkAllCIFBranch, bool chkAllCIFType, bool chkAllAcctBranch,
                                        string remInputId, string merchMemID, string merchProdCode, string profileName)
        {
            return new irsCIFProfiler().saveProfile(top,idNo, guid, cifName, acctNo, acctType, acctStatus, acctCcy, acctBranch,
                                        cifClass, cifType, dateFilter, dateFrom, dateTo,
                                         cifBranch, selectText, chkAllCIFBranch, chkAllCIFType, chkAllAcctBranch, remInputId,
                                          merchMemID, merchProdCode, profileName);
        }


    }
}
