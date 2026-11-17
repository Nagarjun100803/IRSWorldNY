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
    public partial class tranx_profile_create : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(tranx_profile_create));
            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable GetGetStates()
        {
            return new irsReportProfiler().GetStates();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getOFWReports()
        {
            return new irsReportProfiler().GetOFWReports();            
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public irsClientData clientData()
        {
            irsClientData clientData;

            clientData = (irsClientData)HttpContext.Current.Session["IRS_CLIENT_DATA"];
            return clientData;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getRemitMethod()
        {
            return new irsReportProfiler().GetRemitMethod();
        } 

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getIRSOverseas_Branches()
        {            
            return new irsReportProfiler().GetIRSOverseas_Branches(clientData().Location);
        } 

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getIRSDomestic_Branches()
        {
            return new irsReportProfiler().GetIRSDomestic_Branches();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getDateColumns()
        {
            return new irsReportProfiler().GetDateColumns();
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getMerchantName()
        {
            return new irsReportProfiler().GetMerchantName();
        }
        
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getAmtFilter()
        {
            return new irsReportProfiler().GetAmtFilter();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getComputation()
        {
            return new irsReportProfiler().GetComputation();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getRemittingCCY()
        {
            return new irsReportProfiler().GetRemittingCCY();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getTargetCCY()
        {
            return new irsReportProfiler().GetTargetCCY();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getBatchUploadMethods()
        {
            return new irsReportProfiler().GetBatchUploadMethods();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getProductCode()
        {
            return new irsReportProfiler().GetProductCode();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getTranxSource()
        {
            return new irsReportProfiler().GetTranxSource();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getPayMode()
        {
            return new irsReportProfiler().GetPayMode();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getCriteria()
        {
            DataTable dt = new irsReportProfiler().GetCriteria();
            return dt;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getTranxStatus()
        {
            return new irsReportProfiler().GetTranxStatus();
        }
        
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getReportField()
        {
            return new irsReportProfiler().GetReportField();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getProfileList()
        {
            return new irsReportProfiler().GetProfileList();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getserverDate()
        {
            return new irsReportProfiler().GetserverDate();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool CheckProfileName(string ProfileName)
        {
            return new irsReportProfiler().CheckProfileName(ProfileName);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getProfileDetails(string ProfileName)
        {
            return new irsReportProfiler().GetProfileDetails(ProfileName);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable GetBranchRegion(string brchRegion)
        {
            return new irsReportProfiler().GetBranchRegion(brchRegion);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable LoadBracnhFilterPerRegion()
        {
            return new irsReportProfiler().LoadBracnhFilterPerRegion();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String UpdateProfile(string[] RemBrCode, string[] CredBrCode,
            string DateFilter, string DateFrom, string DateTo, string RemittingCCY,
            string TargetCCY, string AmtComputation, string AmtFilter,
            string Amount, string[] RemitMethod, string[] BatchTypes, string[] ProductCode,
            string[] TransactionSource, string[] PayMode, string[] TransactionStatus,
            string RemitterID, string BeneficiaryID, string AccountNo, string[] ReportField,
            bool CheckMainBr, bool CheckRemBr, string profilename, string SQLSyntax,
            bool chkRemitMethod, bool chkBatchTypes, string Criteria, string MemberId, string[] MerchName,
            bool chkMerchants, bool chkProdCode, bool chkPayMode, bool chkTranSource, bool chkTranStatus, string Region, string [] States, bool chkStates)
        {
            return new irsReportProfiler().UpdateProfile(RemBrCode, CredBrCode, DateFilter,
                DateFrom, DateTo, RemittingCCY, TargetCCY, AmtComputation, AmtFilter, Amount,
                RemitMethod, BatchTypes, ProductCode, TransactionSource, PayMode, TransactionStatus,
                RemitterID, BeneficiaryID, AccountNo, ReportField, CheckMainBr, CheckRemBr, profilename,
                SQLSyntax, chkRemitMethod, chkBatchTypes, Criteria, MemberId, MerchName,
                chkMerchants, chkProdCode, chkPayMode, chkTranStatus, chkTranSource, clientData().UserId, Region, States, chkStates);
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string SaveReportProfile(string[] RemBrCode, string[] CredBrCode,
            string DateFilter, string DateFrom, string DateTo, string RemittingCCY,
            string TargetCCY, string AmtComputation, string AmtFilter,
            string Amount, string[] RemitMethod, string[] BatchTypes, string[] ProductCode,
            string[] TransactionSource, string[] PayMode, string[] TransactionStatus,
            string RemitterID, string BeneficiaryID, string AccountNo, string[] ReportField,
            bool CheckMainBr, bool CheckRemBr, string profilename, string SQLSyntax,
            bool chkRemitMethod, bool chkBatchTypes, string Criteria, string MemberId, string[] MerchName,
            bool chkMerchants, bool chkProdCode, bool chkPayMode, bool chkTranSource, bool chkTranStatus, string Region, string[] States, bool chkStates)
        {

            return new irsReportProfiler().SaveReportProfile(RemBrCode, CredBrCode, DateFilter,
                DateFrom, DateTo, RemittingCCY, TargetCCY, AmtComputation, AmtFilter, Amount,
                RemitMethod, BatchTypes, ProductCode, TransactionSource, PayMode, TransactionStatus,
                RemitterID, BeneficiaryID, AccountNo, ReportField, CheckMainBr, CheckRemBr, profilename,
                SQLSyntax, chkRemitMethod, chkBatchTypes, Criteria, MemberId, MerchName,
                chkMerchants, chkProdCode, chkPayMode, chkTranStatus, chkTranSource, clientData().UserId, Region, States, chkStates);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string deleteProfile(string profileName)
        {
            return new irsReportProfiler().DeleteProfile(profileName);
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string BuildSQLQuery(string[] RemBrCode, string[] CredBrCode,
            string DateFilter, string DateFrom, string DateTo, string RemittingCCY,
            string TargetCCY, string AmtComputation, string AmtFilter,
            string Amount, string[] RemitMethod, string[] BatchTypes,string[] ProductCode, 
            string[] TransactionSource, string[] PayMode,string[] TransactionStatus,
            string RemitterID, string BeneficiaryID, string AccountNo, string[] ReportField,
            bool CheckMainBr, bool CheckRemBr, string Criteria, string MemberId, string[] MerchName,
            bool chkMerchs,bool chkPCode, bool chkPMode, bool chkTSource, bool chkTStatus, string [] States, bool chkStates)
             
        {
            return new irsReportProfiler().BuildSQLQuery(RemBrCode, CredBrCode, DateFilter,
                DateFrom, DateTo, RemittingCCY, TargetCCY, AmtComputation, AmtFilter, Amount,
                RemitMethod, BatchTypes, ProductCode, TransactionSource, PayMode,TransactionStatus,
                RemitterID, BeneficiaryID, AccountNo, ReportField, CheckMainBr, CheckRemBr, Criteria,
                MemberId, MerchName, chkMerchs, chkPCode, chkPMode, chkTSource, chkTStatus, States, chkStates); 
              
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable ParseQuery(string[] RemBrCode, string[] CredBrCode,
            string DateFilter, string DateFrom, string DateTo, string RemittingCCY,
            string TargetCCY, string AmtComputation, string AmtFilter,
            string Amount, string[] RemitMethod, string[] BatchTypes,string[] ProductCode, 
            string[] TransactionSource, string[] PayMode,string[] TransactionStatus,
            string RemitterID, string BeneficiaryID, string AccountNo, string[] ReportField,
            bool CheckMainBr, bool CheckRemBr, string Criteria, string MemberId, string[] MerchName,
            bool chkMerch, bool chkPCode, bool chkPMode, bool chkTSource, bool chkTStatus, string[] States,bool chkStates)
        {

            DataTable DT = new irsReportProfiler().ParseQuery(clientData().UserId, RemBrCode, CredBrCode, DateFilter,
                DateFrom, DateTo, RemittingCCY, TargetCCY, AmtComputation, AmtFilter, Amount,
                RemitMethod, BatchTypes, ProductCode, TransactionSource, PayMode,TransactionStatus,
                RemitterID, BeneficiaryID, AccountNo, ReportField, CheckMainBr, CheckRemBr, Criteria,
                MemberId, MerchName, chkMerch, chkPCode, chkPMode, chkTSource, chkTStatus, States, chkStates);

            Session["DATA_SOURCE_DB"] = DT;
            Session["CURRENT_SEARCH_CRITERIA"] = null;
            Session["CURRENT_SORT_ORDER"] = null; 

            return DT;             
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string ExtractReporttoFile(string profilename)
        {
            string response = new irsReportProfiler().ExtractReporttoFile(profilename);
            Session["DL_REPORT_FILE_NAME"] = response.Substring(18).Trim();
            return response;
        }
    }
        
}
