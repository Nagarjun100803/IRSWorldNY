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
    public partial class tranx_profile_createV2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(tranx_profile_createV2));

        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getRemitMethod()
        {
            return new irsReportProfilerV2().getRemitMethod();
                                    
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getIRSOverseas_Branches()
        {
            irsClientData id = (irsClientData)HttpContext.Current.Session["IRS_CLIENT_DATA"];
            return new irsReportProfilerV2().getIRSOverseas_Branches(id.Location);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getIRSDomestic_Branches()
        {
            return new irsReportProfilerV2().getIRSDomestic_Branches();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getDateColumns()
        {
            return new irsReportProfilerV2().getDateColumns();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getAmtFilter()
        {
            return new irsReportProfilerV2().getAmtFilter();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getComputation()
        {
            return new irsReportProfilerV2().getComputation();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getRemittingCCY()
        {
            return new irsReportProfilerV2().getRemittingCCY();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getTargetCCY()
        {
            return new irsReportProfilerV2().getTargetCCY();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getBatchUploadMethods()
        {
            return new irsReportProfilerV2().getBatchUploadMethods();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getProductCode()
        {
            return new irsReportProfilerV2().getProductCode();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getTranxSource()
        {
            return new irsReportProfilerV2().getTranxSource();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getPayMode()
        {
            return new irsReportProfilerV2().getPayMode();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getTranxStatus()
        {
            return new irsReportProfilerV2().getTranxStatus();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getReportField()
        {
            return new irsReportProfilerV2().getReportField();
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getProfileList()
        {
            return new irsReportProfilerV2().getProfileList();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getserverDate()
        {
            return new irsReportProfilerV2().getserverDate();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool CheckProfileName(string ProfileName)
        {
            return new irsReportProfilerV2().CheckProfileName(ProfileName);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getProfileDetails(string ProfileName)
        {
            return new irsReportProfilerV2().getProfileDetails(ProfileName);
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string SaveReportProfile(string[] RemBrCode, string[] CredBrCode,
            string DateFilter, string DateFrom, string DateTo, string RemittingCCY,
            string TargetCCY, string AmtComputation, string AmtFilter,
            string Amount, string[] RemitMethod, string[] BatchTypes, string ProductCode,
            string TransactionSource, string PayMode, string TransactionStatus,
            string RemitterID, string BeneficiaryID, string AccountNo, string[] ReportField,
            bool CheckMainBr, bool CheckRemBr, string profilename, string SQLSyntax,
            bool chkRemitMethod, bool chkBatchTypes)
        {

            return new irsReportProfilerV2().SaveReportProfile(RemBrCode, CredBrCode, DateFilter,
                DateFrom, DateTo, RemittingCCY, TargetCCY, AmtComputation, AmtFilter, Amount,
                RemitMethod, BatchTypes, ProductCode, TransactionSource, PayMode, TransactionStatus,
                RemitterID, BeneficiaryID, AccountNo, ReportField, CheckMainBr, CheckRemBr, profilename,
                SQLSyntax, chkRemitMethod, chkBatchTypes);
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string BuildSQLQuery(string[] RemBrCode, string[] CredBrCode,
            string DateFilter, string DateFrom, string DateTo, string RemittingCCY,
            string TargetCCY, string AmtComputation, string AmtFilter,
            string Amount, string[] RemitMethod, string[] BatchTypes, string ProductCode,
            string TransactionSource, string PayMode, string TransactionStatus,
            string RemitterID, string BeneficiaryID, string AccountNo, string[] ReportField,
            bool CheckMainBr, bool CheckRemBr)
        {
            return new irsReportProfilerV2().BuildSQLQuery(RemBrCode, CredBrCode, DateFilter,
                DateFrom, DateTo, RemittingCCY, TargetCCY, AmtComputation, AmtFilter, Amount,
                RemitMethod, BatchTypes, ProductCode, TransactionSource, PayMode, TransactionStatus,
                RemitterID, BeneficiaryID, AccountNo, ReportField, CheckMainBr, CheckRemBr);

        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable ParseQuery(string[] RemBrCode, string[] CredBrCode,
            string DateFilter, string DateFrom, string DateTo, string RemittingCCY,
            string TargetCCY, string AmtComputation, string AmtFilter,
            string Amount, string[] RemitMethod, string[] BatchTypes, string ProductCode,
            string TransactionSource, string PayMode, string TransactionStatus,
            string RemitterID, string BeneficiaryID, string AccountNo, string[] ReportField,
            bool CheckMainBr, bool CheckRemBr)
        {

            DataTable DT = new irsReportProfilerV2().ParseQuery(RemBrCode, CredBrCode, DateFilter,
               DateFrom, DateTo, RemittingCCY, TargetCCY, AmtComputation, AmtFilter, Amount,
               RemitMethod, BatchTypes, ProductCode, TransactionSource, PayMode, TransactionStatus,
               RemitterID, BeneficiaryID, AccountNo, ReportField, CheckMainBr, CheckRemBr);

            Session["DATA_SOURCE_DB"] = DT;


            return DT;

        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string ExtractReporttoFile(string[] RemBrCode, string[] CredBrCode,
            string DateFilter, string DateFrom, string DateTo, string RemittingCCY,
            string TargetCCY, string AmtComputation, string AmtFilter,
            string Amount, string[] RemitMethod, string[] BatchTypes, string ProductCode,
            string TransactionSource, string PayMode, string TransactionStatus,
            string RemitterID, string BeneficiaryID, string AccountNo, string[] ReportField,
            bool CheckMainBr, bool CheckRemBr, string profilename)
        {

            string response;

            response = new irsReportProfilerV2().ExtractReporttoFile(RemBrCode, CredBrCode, DateFilter,
               DateFrom, DateTo, RemittingCCY, TargetCCY, AmtComputation, AmtFilter, Amount,
               RemitMethod, BatchTypes, ProductCode, TransactionSource, PayMode, TransactionStatus,
               RemitterID, BeneficiaryID, AccountNo, ReportField, CheckMainBr, CheckRemBr, profilename);

            Session["DL_REPORT_FILE_NAME"] = response.Substring(18).Trim();
            return response;


        }

        private void download(String path)
        {
            try
            {
                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + System.IO.Path.GetFileName(path));
                Response.AppendHeader("Content-Length", new System.IO.FileInfo(path).Length.ToString());
                Response.AppendHeader("Content-Transfer-Encoding", "binary");
                Response.TransmitFile(path);
                Response.End();
            }
            catch //(Exception ex) // commented to remove warning
            {

            }
        }
            
            
    }
}
