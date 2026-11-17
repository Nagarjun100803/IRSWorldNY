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
using System.IO;

namespace IRSWorld.Report_Profiler
{
    public partial class AdhocReports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(AdhocReports));
            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getReportDDL()
        {
            return new irsAdhocReports().getReportDDL();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getAdhocReports(string reportName, string spName, string dtFrom, string dtTo)
        {
            string sError = string.Empty;
            string sReturn = string.Empty;
            DataSet ds = new irsAdhocReports().getAdhocReports(spName, dtFrom, dtTo).DataSet;
            sReturn = this.exportToExcel(reportName, ds, "EXTRACT", out sError);
            if (sError != string.Empty)
            {
                return sError;
            }
            else
            {
                return sReturn;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        private string getLocation()
        {
            return ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        private string getUserID()
        {
            return ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        private void writeAudit(string Description, string NewValue, string OldValue, csAuditLog.Category Category)
        {
            string sDescription = "Location=" + getLocation() + " : " + Description;
            new csAuditLog().writeAudit(getUserID(), "Adhoc Report", sDescription, NewValue, OldValue, Category);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        private void createDirectory(string path)
        {
            try
            {
                if (!(Directory.Exists(path)))
                {
                    DirectoryInfo di = Directory.CreateDirectory(path);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string exportToExcel(string fileName, DataSet ds, string sheetName, out string errorMsg)
        {
            errorMsg = string.Empty;
            string sPath = System.Configuration.ConfigurationManager.AppSettings["AdhocReportRoot"].ToString() +
                System.Configuration.ConfigurationManager.AppSettings["AdhocReportFolder"].ToString();

            this.createDirectory(sPath);
            fileName = fileName.Replace(" ", string.Empty);
            //fileName += new irsPaymentGateway().GenerateFile();
            fileName = sPath + @"\" + fileName;

            try
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count <= 65535)
                    {
                        fileName += ".xls";
                        IRSClassLib.csExcel.ExcelGenerator.CreateExcelXML(fileName, ds, sheetName, 65535);
                    }
                    else
                    {
                        fileName += ".csv";
                        if (!new IRSClassLib.csExcel().ExtractToCSV(ds, fileName, true, out errorMsg))
                        {
                            return errorMsg;
                        }
                    }
                }
                else
                {
                    errorMsg = "No record(s) found!";
                    return errorMsg;
                }
                Session["DOWNLOAD_PATH"] = fileName;
                return fileName;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            return errorMsg;
        }
    }
}