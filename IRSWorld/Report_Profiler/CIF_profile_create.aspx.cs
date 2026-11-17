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
            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getCIFClass()
        {
            return new irsCIFProfiler().GetCIFClass();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getCIFBranch()
        {
            irsClientData irsData = (irsClientData)HttpContext.Current.Session["IRS_CLIENT_DATA"];
            return new irsCIFProfiler().GetCIFBranch(irsData.Location);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getCIFAcctbranch()
        {
            return new irsCIFProfiler().GetCIFAcctBranch();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getCIFType()
        {
            return new irsCIFProfiler().GetCIFType();
            
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getCIFAccountType()
        {
            return new irsCIFProfiler().GetCIFAcccountType();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getCIFProfiles()
        {
            return new irsCIFProfiler().GetCIFProfiles();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getCIFFields()
        {
            return new irsCIFProfiler().LoadCIFFields();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getCIFAcctCcy ()
        {
            return new irsCIFProfiler().GetCIFAcctCcy();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getCIFDates()
        {
            return new irsCIFProfiler().GetCIFDates();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getCIFAcctStatus()
        {
            return new irsCIFProfiler().GetCIFAcctStatus();
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getCIFMerchCode()
        {
            return new irsCIFProfiler().GetCIFMerchCode();
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getCIFMerchants()
        {
            return new irsCIFProfiler().GetCIFMerchants();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable parseQuery(string top,string idNo, string guid, string cifName, string acctNo, string[] acctType,string[] acctStatus,string[] acctCcy,string [] acctBranch,
                                    string[] cifClass, string[] cifType, string dateFilter,string dateFrom, string dateTo, string[] cifBranch, string[] selectField,
                                        bool chkAllCIFBranch, bool chkAllCIFType,bool chkAllAcctBranch,
                                        string remInputId,string merchMemID,string merchProdCode,string[] merchName,bool chkAllMerchs,
                                        bool chkAllCifClass,bool chkAllAcctStatus,bool chkAllAcctCcy,bool chkAllAcctType)
        {
            DataTable table = new irsCIFProfiler().ParseQuery(top, idNo, guid, cifName, acctNo, acctType,acctStatus,acctCcy,acctBranch,
                                        cifClass, cifType, dateFilter,dateFrom, dateTo,
                                         cifBranch, selectField, chkAllCIFBranch, chkAllCIFType, chkAllAcctBranch, remInputId,
                                          merchMemID, merchProdCode, merchName, chkAllMerchs, chkAllCifClass, chkAllAcctStatus, 
                                           chkAllAcctCcy, chkAllAcctType);
            Session["DATA_SOURCE_DB"] = table;
            Session["CURRENT_SEARCH_CRITERIA"] = null;
            Session["CURRENT_SORT_ORDER"] = null;
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
        public String deleteProfile(string profileName)
        {
            return new irsCIFProfiler().DeleteProfile(profileName);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool isUsed(string profileName)
        {
            return new irsCIFProfiler().IsUsed(profileName);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable loadProfile(string profileName)
        {
            return new irsCIFProfiler().LoadProfile(profileName);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string saveProfile(string top, string idNo, string guid, string cifName, string acctNo, string[] acctType, string[] acctStatus, string[] acctCcy, string[] acctBranch,
                                    string[] cifClass, string[] cifType, string dateFilter, string dateFrom, string dateTo, string[] cifBranch, string[] selectText,
                                        bool chkAllCIFBranch, bool chkAllCIFType, bool chkAllAcctBranch,string remInputId, string merchMemID, string merchProdCode, string profileName, 
                                        string[] merchName, bool chkAllMerchs, bool chkAllCifClass, bool chkAllAcctStatus ,bool chkAllAcctCcy, bool chkAllAcctType)
        {
            return new irsCIFProfiler().SaveProfile(top,idNo, guid, cifName, acctNo, acctType, acctStatus, acctCcy, acctBranch,
                                        cifClass, cifType, dateFilter, dateFrom, dateTo,
                                         cifBranch, selectText, chkAllCIFBranch, chkAllCIFType, chkAllAcctBranch, remInputId,
                                          merchMemID, merchProdCode, profileName, merchName, chkAllMerchs,chkAllCifClass,chkAllAcctStatus,chkAllAcctCcy,chkAllAcctType);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string UpdateProfile(string top, string idNo, string guid, string cifName, string acctNo, string[] acctType, string[] acctStatus, string[] acctCcy, string[] acctBranch,
                                    string[] cifClass, string[] cifType, string dateFilter, string dateFrom, string dateTo, string[] cifBranch, string[] selectText,
                                        bool chkAllCIFBranch, bool chkAllCIFType, bool chkAllAcctBranch, string remInputId, string merchMemID, string merchProdCode, string profileName,
                                        string[] merchName, bool chkAllMerchs, bool chkAllCifClass, bool chkAllAcctStatus, bool chkAllAcctCcy, bool chkAllAcctType)
        {
            return new irsCIFProfiler().UpdateProfile(top, idNo, guid, cifName, acctNo, acctType, acctStatus, acctCcy, acctBranch,
                                        cifClass, cifType, dateFilter, dateFrom, dateTo,
                                         cifBranch, selectText, chkAllCIFBranch, chkAllCIFType, chkAllAcctBranch, remInputId,
                                          merchMemID, merchProdCode, profileName, merchName, chkAllMerchs, chkAllCifClass, chkAllAcctStatus, chkAllAcctCcy, chkAllAcctType);
        }



    }
}
