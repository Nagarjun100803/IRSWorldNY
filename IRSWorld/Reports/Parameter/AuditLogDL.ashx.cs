using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IRSClassLib;
using System.IO;

namespace IRSWorld.Reports.Parameter
{
    /// <summary>
    /// Summary description for AuditLogDL
    /// </summary>
    public class AuditLogDL : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string pdffilename = context.Request["pdffn"];

            if (!string.IsNullOrEmpty(pdffilename))
            {
                DownloadPDFFile(pdffilename);
            }

        }

        public void DownloadPDFFile(string pdffilename)
        {
            try
            {
                string finalpdf = new irsConfiguration().ConnectionStringNoRunMode("ReportExportPath");
                finalpdf = Path.Combine(finalpdf, "ParamAuditLog", pdffilename);
                HttpResponse Response = HttpContext.Current.Response;
                Response.ContentType = "application/octet-stream";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(finalpdf));
                Response.TransmitFile(finalpdf);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.SuppressContent = true;
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("Parameter Admin", "IRSW Parameter Approver ASHX", "Method: DownloadPDFFile", "StackTrace: " + ex.StackTrace, "ERROR: " + ex.Message, csAuditLog.Category.Select);
                HttpContext.Current.Response.Write("Error downloading report (.pdf)");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}